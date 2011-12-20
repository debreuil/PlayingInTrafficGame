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
using DDW.Display;

namespace Smuck.Panels
{
    public class InstructionsPanel : Panel
    {
        protected Sprite[] borderPanels;
        protected Sprite[] bushes;
        protected Sprite road;
        protected Sprite instructions;

        public InstructionsPanel(Texture2D texture, V2DInstance inst) : base(texture, inst) { }

        public override bool OnPlayerInput(int playerIndex, DDW.Input.Move move, TimeSpan time)
        {
            if (move == DDW.Input.Move.ButtonA)
            {
                MenuState ms = MenuState.QuickGame;
                ((StartScreen)parent).SetPanel(ms, playerIndex);
            }
            return true;
        }
    }
}
