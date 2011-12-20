using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using V2DRuntime.Display;
using Microsoft.Xna.Framework.GamerServices;
using V2DRuntime.Components;
using Microsoft.Xna.Framework.Graphics;
using DDW.V2D;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework;
using DDW.Input;
using Microsoft.Xna.Framework.Input;
using DDW.Display;

namespace Smuck.Panels
{
	public class PausedPanel : Panel
	{
        public ButtonTabGroup menuButtons;
        public Sprite xIcon;

		private enum ExitButton { Back, Exit, UnlockTrial };

		public PausedPanel(Texture2D texture, V2DInstance inst) : base(texture, inst) { }

		public override bool OnPlayerInput(int playerIndex, DDW.Input.Move move, TimeSpan time)
		{			
			bool result = base.OnPlayerInput(playerIndex, move, time);
			if (result && isActive)
			{
				if (move == Move.Start)
				{
					Unpause(this, null);
					result = false;
				}
			}
			return result;
		}
		void menuButtons_OnClick(Button sender, int playerIndex, TimeSpan time)
		{
			switch ((ExitButton)sender.Index)
			{
				case ExitButton.Exit:
					SmuckGame.instance.ExitToMainMenu();
					break;
				case ExitButton.Back:
					Unpause(this, null);
					break;
                case ExitButton.UnlockTrial:
                    SmuckGame.instance.UnlockTrial(playerIndex);
					break;
			}
		}

		public override void Activate()
		{
			base.Activate();
			menuButtons.SetFocus(0);
			menuButtons.OnClick += new ButtonEventHandler(menuButtons_OnClick);
		}
		public override void Deactivate()
		{
			base.Deactivate();
			menuButtons.OnClick -= new ButtonEventHandler(menuButtons_OnClick);
		}
        public event EventHandler Unpause;

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Update(gameTime);
            if (menuButtons.element[2].Visible == true && !Guide.IsTrialMode)
            {
                menuButtons.element[2].Visible = false;
                xIcon.Visible = false;
                menuButtons.SetFocus(0);
            }
        }
	}
}
