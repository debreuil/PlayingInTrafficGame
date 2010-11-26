using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using V2DRuntime.Display;
using Smuck.Screens;
using V2DRuntime.Components;
using Smuck.Enums;
using Microsoft.Xna.Framework.Graphics;
using DDW.V2D;

namespace Smuck.Panels
{
    public class InstructionsPanel : Panel
    {
        //public ButtonTabGroup menuButtons;
        public InstructionsPanel(Texture2D texture, V2DInstance inst) : base(texture, inst) { }

		//void menuButtons_OnClick(Button sender, int playerIndex, TimeSpan time)
		//{
		//    ((StartScreen)parent).SetPanel(MenuState.MainMenu);
		//}

    }
}
