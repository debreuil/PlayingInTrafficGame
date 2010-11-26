using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Smuck.Screens;
using V2DRuntime.Components;
using V2DRuntime.Display;
using Microsoft.Xna.Framework.Graphics;
using DDW.V2D;
using Smuck.Enums;

namespace Smuck.Panels
{
    public class OptionsPanel : Panel
    {
       // public ButtonTabGroup menuButtons;
        public OptionsPanel(Texture2D texture, V2DInstance inst) : base(texture, inst) { }

		//void menuButtons_OnClick(Button sender, int playerIndex, TimeSpan time)
		//{
		//    ((StartScreen)parent).SetPanel(MenuState.MainMenu);
		//}

    }
}
