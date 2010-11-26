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
using Smuck.Screens;
using Smuck.Enums;
using V2DRuntime.Network;

namespace Smuck.Panels
{
	public class BeginPanel : Panel
	{
		private bool needsUpdate = true;
		public TextBox[] txName;
		public TextBox[] txState;
		public bool[] states = new bool[]{false, false, false, false};

		public BeginPanel(Texture2D texture, V2DInstance inst) : base(texture, inst) { }

		public override void Initialize()
		{
			base.Initialize();
			SetText();
		}
		public override void AddedToStage(EventArgs e)
		{
			base.AddedToStage(e);
			SignedInGamer.SignedIn += new EventHandler<SignedInEventArgs>(SignedInGamer_SignedIn);
			SignedInGamer.SignedOut += new EventHandler<SignedOutEventArgs>(SignedInGamer_SignedOut);
		}
		public override void Removed(EventArgs e)
		{
			base.Removed(e);
			SignedInGamer.SignedIn -= new EventHandler<SignedInEventArgs>(SignedInGamer_SignedIn);
			SignedInGamer.SignedOut -= new EventHandler<SignedOutEventArgs>(SignedInGamer_SignedOut);
		}
		public bool PlayersDetected()
		{
			bool result = false;
			for (int i = 0; i < states.Length; i++)
			{
				if (states[i])
				{
					result = true;
					break;
				}
			}
			return result;
		}
		public void ShowSignIn()
		{
			if (!Guide.IsVisible)
			{
				Guide.ShowSignIn(4, false);
			}
		}

		private void SetText()
		{
			for (int i = 0; i < screen.inputManagers.Length; i++)
			{
				InputManager im = screen.inputManagers[i];
				if (im != null)
				{
					string name = "Player " + (i + 1);
					if( SignedInGamer.SignedInGamers.Count > i && 
						SignedInGamer.SignedInGamers[i] != null &&
						SignedInGamer.SignedInGamers[i].Gamertag != null)
					{
						name = SignedInGamer.SignedInGamers[i].Gamertag;
					}
					txName[i].Text = name;
					txState[i].Text = states[i] ? "Ready" : "Press 'A' to Begin";
				}
				else
				{
					txName[i].Text = "Player " + (i + 1);
					txState[i].Text = "Not Connected";
				}
			}
		}
		public override bool OnPlayerInput(int playerIndex, DDW.Input.Move move, TimeSpan time)
		{			
			bool result = base.OnPlayerInput(playerIndex, move, time);
			if (result && isActive)
			{
				if (move == Move.ButtonA)
				{
                    if (SignedInGamer.SignedInGamers.Count == 0)
                    {
                        ShowSignIn();
                    }
                    else
                    {
                        result = false;
                        if (!states[playerIndex])
                        {
                            states[playerIndex] = true;
                            needsUpdate = true;
                        }
                        else
                        {
                            ((StartScreen)parent).SetPanel(MenuState.MainMenu);
                        }
                    }
				}
				else if (move == Move.ButtonB || ((int)move.Releases & (int)Microsoft.Xna.Framework.Input.Buttons.B) > 0)
				{
					result = false;
					if(states[playerIndex])
					{
						states[playerIndex] = false;
                        needsUpdate = true;
					}
				}
				if (move == Move.ButtonX)
				{
					ShowSignIn();
				}
			}
			return result;
		}
		public override void Activate()
		{
			base.Activate();
			needsUpdate = true;
		}
		public override void Deactivate()
		{
			base.Deactivate();
			needsUpdate = true;
		}
		void SignedInGamer_SignedIn(object sender, SignedInEventArgs e)
		{
			states[(int)e.Gamer.PlayerIndex] = true;
			needsUpdate = true;
		}
		void SignedInGamer_SignedOut(object sender, SignedOutEventArgs e)
		{
			states[(int)e.Gamer.PlayerIndex] = false;
			needsUpdate = true;
		}
		public override void Update(GameTime gameTime)
		{
			base.Update(gameTime);
			if (needsUpdate)
			{
				needsUpdate = false;
				SetText();
			}
		}
	}
}
