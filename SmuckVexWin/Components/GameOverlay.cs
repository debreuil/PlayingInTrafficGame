using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DDW.Display;
using V2DRuntime.Attributes;
using V2DRuntime.Shaders;
using Smuck.Shaders;
using Smuck.Enums;
using Microsoft.Xna.Framework.Graphics;
using DDW.V2D;
using Smuck.Panels;
using Smuck.Screens;
using Smuck.Audio;
using V2DRuntime.V2D;

namespace Smuck.Components
{
    [ScreenAttribute(depthGroup = 1000, isPersistantScreen = true)]
    public class GameOverlay : V2DScreen
    {
    //    [V2DSpriteAttribute(categoryBits = Category.BORDER, maskBits = Category.DEFAULT | Category.ICON | Category.PLAYER, isStatic = true, allowSleep = false)]
    //    protected V2DSprite[] border;
        [SpriteAttribute(depthGroup = 1000)]
        public PrescreenPanel prescreenPanel;
        [SpriteAttribute(depthGroup = 1000)]
        public PausedPanel pausedPanel;
        [SpriteAttribute(depthGroup = 1000)]
        public EndRoundPanel endRoundPanel;
        [SpriteAttribute(depthGroup = 100)]
        protected V2DSprite bushes;
        [SpriteAttribute(depthGroup = 100)]
        protected V2DSprite[] panel;
        [V2DShaderAttribute(shaderType = typeof(TextShader))]
        [V2DSpriteAttribute(depthGroup = 200)]
        public ScoreView scores; 
        
        public bool hasActivePanel = false;

        public GameOverlay(V2DContent v2dContent) : base(v2dContent)
        {
            isPersistantScreen = true;
        }
        public GameOverlay(SymbolImport si) : base(si)
        {
            SymbolImport = si; 
            isPersistantScreen = true;
		}

        public override void Initialize()
        {
            base.Initialize();

            for (int i = 0; i < scores.txScore.Count; i++)
            {
                scores.txScore[i].Text = "";
                scores.txScore[i].Visible = false;
                scores.laneCrossIndicator[i].Visible = false;
            }

            prescreenPanel.Continue += new EventHandler(prescreenPanel_Continue);
            prescreenPanel.Activate();
            hasActivePanel = true;

            pausedPanel.Unpause += new EventHandler(pausedPanel_Unpause);
            pausedPanel.Deactivate();

            endRoundPanel.Continue += new EventHandler(endRoundPanel_Continue);
            endRoundPanel.Deactivate();
        }

        void prescreenPanel_Continue(object sender, EventArgs e)
        {
//            prescreenPanel.Continue -= new EventHandler(prescreenPanel_Continue);
            prescreenPanel.Deactivate();
            hasActivePanel = false;
        }

        public void SetLevel(Level levelEnum)
        {
            panel[0].GotoAndStop((uint)levelEnum);
            panel[1].GotoAndStop((uint)levelEnum);
            prescreenPanel.GotoAndStop((uint)levelEnum - 1);
            prescreenPanel.Activate();
            hasActivePanel = true;
        }
        public void SetPlayer(SmuckPlayer p)
        {
            endRoundPanel.scoreBox[(int)p.gamePadIndex].Player = p;
            scores.txScore[(int)p.gamePadIndex].Visible = true;
            scores.laneCrossIndicator[(int)p.gamePadIndex].Visible = true;
            SetScore(p);
        }
        private int pointsPerCrossing;
        public void SetGoals(int pointsPerCrossing, int totalCrossings)
        {
            this.pointsPerCrossing = pointsPerCrossing;
            for (int i = 0; i < scores.laneCrossIndicator.Count; i++)
            {
                scores.laneCrossIndicator[i].TargetCount = totalCrossings;
            }
        }
        public void SetScore(SmuckPlayer p)
        {
            int pIndex = (int)p.gamePadIndex;
            scores.txScore[pIndex].Text = p.roundScore.ToString();// +"/" + pointsToWinRound.ToString();
            scores.laneCrossIndicator[pIndex].Count = (int)((p.roundScore + 1) / pointsPerCrossing);
        }
        public void PauseGame()
        {
            if (prescreenPanel.isActive)
            {
                prescreenPanel.Deactivate();
            }
            pausedPanel.Activate();
            stage.audio.PauseSound(Sfx.backgroundMusic);
            hasActivePanel = true;
        }
        protected void pausedPanel_Unpause(object sender, EventArgs e)
        {
            pausedPanel.Deactivate();
            if (screen is BaseScreen)
            {
                ((BaseScreen)screen).ResumeAllVehicleSounds();
            }
            stage.audio.ResumeSound(Sfx.backgroundMusic);
            hasActivePanel = false;
        }

        public void EndRound()
        {
            endRoundPanel.Activate();
            stage.audio.PauseSound(Sfx.backgroundMusic);
            hasActivePanel = true;
        }
        void endRoundPanel_Continue(object sender, EventArgs e)
        {
            endRoundPanel.Deactivate();
            hasActivePanel = false;
            stage.NextScreen();
        }

    }
}
