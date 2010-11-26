using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Smuck.Screens;
using V2DRuntime.Display;
using V2DRuntime.Components;
using Smuck.Enums;
using Microsoft.Xna.Framework.Graphics;
using DDW.V2D;
using Smuck.Components;

namespace Smuck.Panels
{
    public class HighScorePanel : Panel
    {
        public TextBox[] txName;
        public TextBox[] txScore;
        public HighScorePanel(Texture2D texture, V2DInstance inst) : base(texture, inst) { }

        public override void Initialize()
        {
            base.Initialize();
            List<HighScoreElement> hs = ((SmuckGame)SmuckGame.instance).GetHighScores();
            for (int i = 0; i < txName.Length; i++)
            {
                if (i < hs.Count)
                {
                    txName[i].Visible = true;
                    txScore[i].Visible = true;
                    txName[i].Text = hs[i].name;
                    txScore[i].Text = hs[i].score.ToString();
                }
                else
                {
                    txName[i].Visible = false;
                    txScore[i].Visible = false;
                }
            }
        }
		//void menuButtons_OnClick(Button sender, int playerIndex, TimeSpan time)
		//{
		//    ((StartScreen)parent).SetPanel(MenuState.MainMenu);
		//}

    }
}
