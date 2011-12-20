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
using V2DRuntime.Panels;
using Microsoft.Xna.Framework;

namespace Smuck.Screens
{
    public class StartScreen : V2DScreen
    {
        public Sprite bkg2;
        public Sprite trafficLogo;
		public TrafficSplashPanel splashPanel;
		public BeginPanel beginPanel;
        public MainMenuPanel mainMenuPanel;
        public LobbyPanel lobbyPanel;
        public NetworkGamePanel networkPanel;
        public OptionsPanel optionsPanel;
        public HighScorePanel highScorePanel;
        public InstructionsPanel instructionsPanel;
        public TrialExpiredPanel trialPanel;
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

            panels = new Panel[] { splashPanel, beginPanel, mainMenuPanel, trialPanel, optionsPanel, highScorePanel, instructionsPanel, exitPanel };
 		}
        public bool allLevelsComplete = false;
		protected override void OnAddToStageComplete()
		{
			base.OnAddToStageComplete();

            SetPanel(MenuState.Empty, -1);

			if (firstTimeDisplayed)
			{
                SetPanel(MenuState.Splash, -1);
				firstTimeDisplayed = false;
			}
            else if (allLevelsComplete)
            {
                SetPanel(MenuState.HighScores, -1);
                allLevelsComplete = false;
            }
            else
            {
                SetPanel(MenuState.MainMenu, -1);
            }
		}
		public override void AddedToStage(EventArgs e)
		{
			base.AddedToStage(e);
            NetworkManager.Instance.OnGameStarted += new NetworkManager.GameStartedDelegate(OnStartGame);
//          AudioManager.PlaySound(AudioManager.backgroundMusic);
		}
		public override void RemovedFromStage(EventArgs e)
		{
			base.RemovedFromStage(e);
            SetPanel(MenuState.Empty, -1);
            panels = null;
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
                        SetPanel(MenuState.MainMenu, playerIndex);
                        stage.audio.PlaySound(Sfx.closePanel);
					}
					else
					{
                        if (panelStack.Count > 0)
                        {
                            panelStack.Peek().OnPlayerInput(playerIndex, move, time);
                        }
					}
				}
				else if (curState == MenuState.MainMenu && move == Move.ButtonB)
				{
                    SetPanel(MenuState.Begin, playerIndex);
                    stage.audio.PlaySound(Sfx.closePanel);
				}
				else
				{
                    if (panelStack.Count > 0)
                    {
                        panelStack.Peek().OnPlayerInput(playerIndex, move, time);
                    }
				}
			}
			return true;
        }

        public void SetPanel(MenuState state, int playerIndex)
        {
            //Console.WriteLine(playerIndex.ToString());
            //return;
            trafficLogo.Visible = (state != MenuState.Splash);

            switch (state)
            {
				case MenuState.Empty:
					panelStack.Clear();
                    break;
                case MenuState.Splash:
                    panelStack.Push(splashPanel);
                    break;
                case MenuState.Begin:
                    panelStack.Push(beginPanel);
                    break;
                case MenuState.MainMenu:
					panelStack.Push(mainMenuPanel);
                    break;
                case MenuState.HighScores:
                    highScorePanel.SetActivePlayer((PlayerIndex)(playerIndex));
					panelStack.Push(highScorePanel);
                    break;
                case MenuState.Instructions:
					panelStack.Push(instructionsPanel);
                    break;

                case MenuState.UnlockTrial:
                    SmuckGame.instance.UnlockTrial(playerIndex);
                    break;

                case MenuState.Options:
					panelStack.Push(optionsPanel);
                    break;

                case MenuState.Exit:
					panelStack.Push(exitPanel);
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
                        OnStartGame();
                    }
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
            for (int i = 0; i < inputManagers.Length; i++)
            {
                if (inputManagers[i] != null && inputManagers[i].Player != null)
                {
                    ((SmuckPlayer)inputManagers[i].Player).ResetGameScore();
                }
            }
			stage.NextScreen();
        }

        public MenuState nextState = MenuState.Empty;
        public override void OnUpdateComplete(GameTime gameTime)
        {
            base.OnUpdateComplete(gameTime);
            if (nextState != MenuState.Empty)
            {
                MenuState tempState = nextState;
                nextState = MenuState.Empty;
                SetPanel(tempState, -1);
            }
        }
    }
}
