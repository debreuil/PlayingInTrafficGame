using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using V2DRuntime.Display;
using Microsoft.Xna.Framework.GamerServices;
using V2DRuntime.Components;
using Microsoft.Xna.Framework.Graphics;
using DDW.V2D;
using Smuck.Audio;
using DDW.Display;

namespace Smuck.Panels
{
	public class ExitPanel : Panel
	{
		public ButtonTabGroup menuButtons;
        public Sprite xIcon;

		private enum ExitButton { Exit, Back, UnlockTrial };

		public ExitPanel(Texture2D texture, V2DInstance inst) : base(texture, inst) { }

		void menuButtons_OnClick(Button sender, int playerIndex, TimeSpan time)
		{
			switch ((ExitButton)sender.Index)
			{
				case ExitButton.Exit:
					SmuckGame.instance.ExitGame();
					break;
				case ExitButton.Back:
					SmuckGame.instance.ExitToMainMenu();
					break;
                case ExitButton.UnlockTrial:
                    SmuckGame.instance.UnlockTrial(playerIndex);
					break;
			}
        }
        void menuButtons_OnFocusChanged(Button sender)
        {
            stage.audio.PlaySound(Sfx.navigateSound);
        }
		public override void AddedToStage(EventArgs e)
		{
			base.AddedToStage(e);
			menuButtons.SetFocus(0);
            menuButtons.OnClick += new ButtonEventHandler(menuButtons_OnClick);
            menuButtons.OnFocusChanged += new FocusChangedEventHandler(menuButtons_OnFocusChanged);

		}
		public override void RemovedFromStage(EventArgs e)
		{
			base.RemovedFromStage(e);
            menuButtons.OnClick -= new ButtonEventHandler(menuButtons_OnClick);
            menuButtons.OnFocusChanged -= new FocusChangedEventHandler(menuButtons_OnFocusChanged);
		}

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Update(gameTime);
            if (menuButtons.element[2].Visible == true && !Guide.IsTrialMode)
            {
                menuButtons.element[2].Visible = false;
                xIcon.Visible = false;
                menuButtons.SetFocus(1);
            }
        }

	}
}
