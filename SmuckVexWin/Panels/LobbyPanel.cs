
using System;
using DDW.Input;
using DDW.V2D;
using Microsoft.Xna.Framework.Graphics;
using Smuck.Enums;
using Smuck.Screens;
using V2DRuntime.Components;
using V2DRuntime.Display;
using V2DRuntime.Network;

namespace Smuck.Panels
{
    public class LobbyPanel : Panel
	{
		//public ButtonTabGroup menuButtons;
		public TextBox txTitle;
		public TextBox txHost;
		public TextBox txPlayers;

        public LobbyPanel(Texture2D texture, V2DInstance inst) : base(texture, inst) { }

		public override bool OnPlayerInput(int playerIndex, Move move, TimeSpan time)
		{
			bool result = base.OnPlayerInput(playerIndex, move, time);
			if (result && move == Move.ButtonA)
			{
				((StartScreen)parent).SetPanel(MenuState.QuickGame);
			}
			return result;
		}
		//void menuButtons_OnClick(Button sender, int playerIndex, TimeSpan time)
		//{
		//    switch (sender.Index)
		//    {
		//        case 0:
		//            ((StartScreen)parent).SetPanel(MenuState.QuickGame);
		//            break;
		//        case 1:
		//            NetworkManager.Instance.LeaveSession();
		//            ((StartScreen)parent).SetPanel(MenuState.MainMenu);
		//            break;
		//    }
		//}
		private int prevCount = 0;
		public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
		{
			base.Update(gameTime);
			if (NetworkManager.Session != null)
			{
				if (NetworkManager.Session.LocalGamers.Count > 0 && 
					txHost.Text != NetworkManager.Session.LocalGamers[0].Gamertag)
				{
					txHost.Text = "Host: " + NetworkManager.Session.LocalGamers[0].Gamertag;
				}

				if (NetworkManager.Session.RemoteGamers.Count != prevCount)
				{
					prevCount = NetworkManager.Session.RemoteGamers.Count;
					string s = NetworkManager.Session.RemoteGamers[0].Gamertag;
					for (int i = 1; i < NetworkManager.Session.RemoteGamers.Count; i++)
					{
						s += "\n  " + NetworkManager.Session.RemoteGamers[i].Gamertag;
					}
					txPlayers.Text = "Players: \n  " + s;
				}
			}
		}
    }
}

