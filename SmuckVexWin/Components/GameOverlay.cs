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
using V2DRuntime.Display;

namespace Smuck.Components
{
    [ScreenAttribute(depthGroup = 1000, isPersistantScreen = true)]
    public class GameOverlay : V2DScreen
    {
    //    [V2DSpriteAttribute(categoryBits = Category.BORDER, maskBits = Category.DEFAULT | Category.ICON | Category.PLAYER, isStatic = true, allowSleep = false)]
    //    protected V2DSprite[] border;

        [SpriteAttribute(depthGroup = 1000)]
        public TrialExpiredPanel trialPanel;

        [SpriteAttribute(depthGroup = 1000)]
        public PrescreenPanel prescreenPanel;

        [SpriteAttribute(depthGroup = 1000)]
        public PausedPanel pausedPanel;

        [SpriteAttribute(depthGroup = 1000)]
        public EndRoundPanel endRoundPanel;

        [SpriteAttribute(depthGroup = 100)]
        protected V2DSprite bushes;

        [SpriteAttribute(depthGroup = 100)]
        protected V2DSprite[] borderPanels;

        //[V2DShaderAttribute(shaderType = typeof(TextShader))]
        [V2DSpriteAttribute(depthGroup = 200)]
        public ScoreView scores;

        protected Panel[] panels;
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
            panels = new Panel[] { pausedPanel, prescreenPanel, endRoundPanel, trialPanel };
        }

        public override void Activate()
        {
            base.Activate();

            this.visible = true;

            for (int i = 0; i < scores.txScore.Count; i++)
            {
                scores.txScore[i].Text = "";
                scores.txScore[i].Visible = false;
                scores.laneCrossIndicator[i].Visible = false;
            }

            prescreenPanel.Continue += new EventHandler(prescreenPanel_Continue);
            pausedPanel.Unpause += new EventHandler(pausedPanel_Unpause);
            endRoundPanel.Continue += new EventHandler(endRoundPanel_Continue);

            SetPanel(PanelType.PreRound);
            //trialPanel.Deactivate();
            //pausedPanel.Deactivate();
            //endRoundPanel.Deactivate();
            //prescreenPanel.Activate();
            //hasActivePanel = true;
        }
        public override void Deactivate()
        {
            base.Deactivate();

            this.visible = false;

            SetPanel(PanelType.Empty);
            //prescreenPanel.Deactivate();
            //pausedPanel.Deactivate();
            //endRoundPanel.Deactivate();

            prescreenPanel.Continue -= new EventHandler(prescreenPanel_Continue);
            pausedPanel.Unpause -= new EventHandler(pausedPanel_Unpause);
            endRoundPanel.Continue -= new EventHandler(endRoundPanel_Continue);
        }

        public void SetLevel(Level levelEnum, uint levelNumber)
        {
            borderPanels[0].GotoAndStop((uint)levelEnum);
            borderPanels[1].GotoAndStop((uint)levelEnum);
            prescreenPanel.SetLevel(levelEnum, levelNumber);
            //prescreenPanel.GotoAndStop((uint)levelEnum - 1);
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
            SetPanel(PanelType.Paused);
        }
        public void ResumeGame()
        {
            SetPanel(PanelType.Empty);
            if (screen is BaseScreen)
            {
                ((BaseScreen)screen).ResumeAllVehicleSounds();
            }
        }
        protected void pausedPanel_Unpause(object sender, EventArgs e)
        {
            ResumeGame();
        }

        void prescreenPanel_Continue(object sender, EventArgs e)
        {
            SetPanel(PanelType.Empty);
        }

        public void EndRound()
        {
            SetPanel(PanelType.PostRound);
            stage.audio.PauseSound(Sfx.backgroundMusic);
        }
        void endRoundPanel_Continue(object sender, EventArgs e)
        {
            SetPanel(PanelType.Empty);
            if (stage.GetCurrentScreen() is SteamRollerScreen)
            {
                SmuckGame.instance.AllLevelsComplete();
            }
            else
            {
                stage.NextScreen();
            }
        }

        public void TrialExpired()
        {
            SetPanel(PanelType.TrialExpired);
        }

        private void SetPanel(PanelType panelType)
        {
            Panel nextPanel = null;
            hasActivePanel = true;
            switch (panelType)
            {
                case PanelType.Empty:
                    hasActivePanel = false;
                    break;
                case PanelType.Paused:
                    nextPanel = pausedPanel;
                    break;
                case PanelType.PreRound:
                    nextPanel = prescreenPanel;
                    break;
                case PanelType.PostRound:
                    nextPanel = endRoundPanel;
                    break;
                case PanelType.TrialExpired:
                    nextPanel = trialPanel;
                    break;
            }

            for (int i = 0; i < panels.Length; i++)
            {
                if (panels[i] == nextPanel)
                {
                    panels[i].Activate();
                }
                else
                {
                    panels[i].Deactivate();
                }
            }

            if (hasActivePanel)
            {
                stage.audio.PauseSound(Sfx.backgroundMusic);
            }
            else
            {
                stage.audio.ResumeSound(Sfx.backgroundMusic);
            }
        }

        private enum PanelType
        {
            Empty,
            Paused,
            PreRound,
            PostRound,
            TrialExpired,
        }
    }

}
