using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DDW.Display;
using V2DRuntime.Components;
using DDW.Input;
using DDW.V2D;
using Microsoft.Xna.Framework.Graphics;
using V2DRuntime.Display;
using Smuck.Screens;
using Smuck.Enums;
using Microsoft.Xna.Framework.Net;
using V2DRuntime.Network;

namespace Smuck.Panels
{
	public class NetworkGamePanel : Panel
	{
		public ButtonTabGroup menuButtons;
		private enum NetworkButton { Host, Join };

		public NetworkGamePanel(Texture2D texture, V2DInstance inst) : base(texture, inst) { }

		void menuButtons_OnClick(Button sender, int playerIndex, TimeSpan time)
		{
			switch ((NetworkButton)sender.Index)
			{
				case NetworkButton.Host:
					NetworkManager.Instance.CreateSession(NetworkSessionType.Local);
					if (NetworkManager.Session != null)
					{
						((StartScreen)parent).SetPanel(MenuState.Lobby, playerIndex);
					}
					break;
				case NetworkButton.Join:
					NetworkManager.Instance.JoinSession();
					if (NetworkManager.Session != null)
					{
						((StartScreen)parent).SetPanel(MenuState.Lobby, playerIndex);
					}
					break;
			}
		}
		public override void AddedToStage(EventArgs e)
		{
			base.AddedToStage(e);
			menuButtons.SetFocus(0);
			menuButtons.OnClick += new ButtonEventHandler(menuButtons_OnClick);
		}
		public override void RemovedFromStage(EventArgs e)
		{
			base.RemovedFromStage(e);
			menuButtons.OnClick -= new ButtonEventHandler(menuButtons_OnClick);
		}

	}
}