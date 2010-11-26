using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DDW.V2D;
using DDW.Display;
using V2DRuntime.Components;
using DDW.Input;
using Smuck.Panels;
using V2DRuntime.Display;
using Smuck.Enums;
using V2DRuntime.Network;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.GamerServices;
using Smuck.Audio;
using Smuck.Components;
using V2DRuntime.Attributes;

namespace Smuck.Screens
{
    public class StartScreen : V2DScreen
    {
        public Sprite bkg2;
		public BeginPanel beginPanel;
        public MainMenuPanel mainMenuPanel;
        public LobbyPanel lobbyPanel;
        public NetworkGamePanel networkPanel;
        public OptionsPanel optionsPanel;
        public HighScorePanel highScorePanel;
		public InstructionsPanel instructionsPanel;
		public ExitPanel exitPanel;

		private bool firstTimeDisplayed = true;
		public Sprite buttonGuide;

        public Panel[] panels;
		MenuState curState;

        //private Panel curPanel;
		private Stack<Panel> panelStack = new Stack<Panel>();

        public StartScreen(V2DContent v2dContent) : base(v2dContent)
        {
        }
        public StartScreen(SymbolImport si) : base(si)
        {
            SymbolImport = si;
		}
		public override void Initialize()
		{
			base.Initialize();
		}
		protected override void OnAddToStageComplete()
		{
			base.OnAddToStageComplete();
            
			if (firstTimeDisplayed)
			{
				panels = new Panel[] { beginPanel, mainMenuPanel, /*networkPanel, lobbyPanel,*/ optionsPanel, highScorePanel, instructionsPanel, exitPanel };
				SetPanel(MenuState.Begin);
				firstTimeDisplayed = false;
			}
			else
			{
				SetPanel(MenuState.MainMenu);
			}
		}
		public override void AddedToStage(EventArgs e)
		{
			base.AddedToStage(e);
            NetworkManager.Instance.OnGameStarted += new NetworkManager.GameStartedDelegate(OnStartGame);
//            AudioManager.PlaySound(AudioManager.backgroundMusic);
		}
		public override void RemovedFromStage(EventArgs e)
		{
			base.RemovedFromStage(e);
			SetPanel(MenuState.Empty);
			NetworkManager.Instance.OnGameStarted -= new NetworkManager.GameStartedDelegate(OnStartGame);
		}

        public override bool OnPlayerInput(int playerIndex, Move move, TimeSpan time)
		{
			//base.OnPlayerInput(playerIndex, move, time);

			if (!Guide.IsVisible)
			{
				if (curState != MenuState.MainMenu && move == Move.ButtonB)
				{
					if (curState != MenuState.Begin)
					{
                        SetPanel(MenuState.MainMenu);
                        stage.audio.PlaySound(Sfx.closePanel);
					}
					else
					{
						panelStack.Peek().OnPlayerInput(playerIndex, move, time);
					}
				}
				else if (curState == MenuState.MainMenu && move == Move.ButtonB)
				{
					SetPanel(MenuState.Begin);
                    stage.audio.PlaySound(Sfx.closePanel);
				}
				else
				{
                    panelStack.Peek().OnPlayerInput(playerIndex, move, time);
				}
			}
			return true;
        }

        public void SetPanel(MenuState state)
        {
            switch (state)
            {
				case MenuState.Empty:
					panelStack.Clear();
                    //curPanel = mainMenuPanel;
                    break;
                case MenuState.Begin:
					panelStack.Push(beginPanel);
                    //curPanel = mainMenuPanel;
                    break;
                case MenuState.MainMenu:
					panelStack.Push(mainMenuPanel);
                    //curPanel = mainMenuPanel;
                    break;
                case MenuState.HighScores:
					panelStack.Push(highScorePanel);
					//curPanel = highScorePanel;
                    break;
                case MenuState.Instructions:
					panelStack.Push(instructionsPanel);
					//curPanel = instructionsPanel;
                    break;
                case MenuState.NetworkGame:
					panelStack.Push(networkPanel);
					//curPanel = networkPanel;
                    break;
                case MenuState.Lobby:
					panelStack.Push(lobbyPanel);
					//curPanel = lobbyPanel;
                    break;
                case MenuState.HostGame:
                    //curPanel = ;
                    break;
                case MenuState.JoinGame:
                    //curPanel = ;
                    break;

                case MenuState.Options:
					panelStack.Push(optionsPanel);
					//curPanel = optionsPanel;
                    break;

                case MenuState.Exit:
					panelStack.Push(exitPanel);
					//curPanel = exitPanel;
                    break;

                case MenuState.QuickGame:
                    panelStack.Pop();
                    if (NetworkManager.Session == null)
                    {
                        NetworkManager.Instance.CreateSession(NetworkSessionType.Local);
                    }
                    NetworkSession ns = NetworkManager.Session;
                    if (ns != null && ns.IsHost && ns.IsEveryoneReady && ns.SessionState == NetworkSessionState.Lobby)
                    {
                        ns.Update();
                        ns.StartGame();
                        ns.Update();
                    }
                    else
                    {
                        stage.NextScreen();
                    }
                    break;

                case MenuState.UnlockTrial:
					//Guide.ShowMarketplace(Microsoft.Xna.Framework.PlayerIndex);
                    break;
            }

			buttonGuide.Visible = (state == MenuState.Begin) ? false : true;

			Panel cp = panelStack.Count > 0 ? panelStack.Peek() : null;
            for (int i = 0; i < panels.Length; i++)
            {
				if (panels[i] == cp)
                {
					if (!children.Contains(panels[i]))
					{
						AddChild(panels[i]);
                        panels[i].Activate();
					}
                    stage.audio.PlaySound(Sfx.openPanel);
                }
                else
                {
					if (panels[i].IsOnStage)
					{
					    panels[i].Deactivate();
						RemoveChild(panels[i]);
					}
                }
            }

            if (state == MenuState.MainMenu)
            {
                panelStack.Clear();
                panelStack.Push(mainMenuPanel);
            }

			curState = state;
        }
		protected void OnStartGame()
        {
            //AudioManager.PlaySound(AudioManager.beginPlay);
            //stage.SetScreen("level1Screen");
			stage.NextScreen();
		}
    }
}
