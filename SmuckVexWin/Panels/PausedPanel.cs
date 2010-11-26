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

namespace Smuck.Panels
{
	public class PausedPanel : Panel
	{
		public ButtonTabGroup menuButtons;
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
					//V2DGame.instance.in
					//foreach (NetworkGamer ng in V2DGame.instance.gamers)
					//{
					//    if(ng.
					//}
					Guide.ShowMarketplace((PlayerIndex)playerIndex);
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
	}
}
