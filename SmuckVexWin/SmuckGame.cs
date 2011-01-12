#define LOCALGAME

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DDW.V2D;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Smuck.Screens;
using Microsoft.Xna.Framework.GamerServices;
using V2DRuntime.Network;
using Microsoft.Xna.Framework.Net;
using DDW.Display;
using Smuck.Components;
using Smuck.Enums;
using System.Reflection;
using System.IO;
using Microsoft.Xna.Framework.Storage;

namespace Smuck
{
    public class SmuckGame : V2DGame
    {
        public static List<Level> Levels;
        // static bool[] ReadyPlayers = new bool[] { false, false, false, false };

		public SmuckGame():base()
		{
		}

        private static GameOverlay gameOverlay;
        public static GameOverlay GameOverlay
        {
            get
            {
                if (gameOverlay == null)
                {
                    SymbolImport si = new SymbolImport("titleScreen", "gameOverlay");
                    gameOverlay = new GameOverlay(si);
                    gameOverlay.Visible = false;
                    stage.AddScreen(gameOverlay);
                }
                return gameOverlay;
            }
        }
		protected override void LoadContent()
		{
			base.LoadContent();
			FontManager.Instance.AddFont("Arial Black", V2DGame.contentManager.Load<SpriteFont>(@"ArialBlack"));
            stage.InitializeAudio(@"Content\audio\smuck.xgs", @"Content\audio\Wave Bank.xwb", @"Content\audio\Sound Bank.xsb");

            Guide.SimulateTrialMode = true;
		}

        StartScreen titleScreen;
        protected override void CreateScreens()
        {
            SymbolImport si = new SymbolImport("titleScreen", "entryScreen");
            titleScreen = new StartScreen(si);
            stage.AddScreen(titleScreen);

            levelNumber = 12;
            //AddLevel("allCarsScreen", typeof(AllCarsScreen));
            //AddLevel("wideBoulevardScreen", typeof(WideBoulevardScreen));
            //AddLevel("spaceMediumScreen", typeof(SpaceMediumScreen));
            AddLevel("steamRollerScreen", typeof(SteamRollerScreen));

            levelNumber = 0;
            AddLevel("twoLaneScreen", typeof(TwoLaneScreen)); // must be first
             
            AddLevel("wideBoulevardScreen", typeof(WideBoulevardScreen));
            AddLevel("crosswalkScreen", typeof(CrosswalkScreen));
            AddLevel("twoTrainTwoRestScreen", typeof(TwoTrainTwoRestScreen));
            AddLevel("twoCanaltwoBoulScreen", typeof(TwoCanalTwoBoulevardScreen));
            AddLevel("spaceMediumScreen", typeof(SpaceMediumScreen));

            AddLevel("allCarsScreen", typeof(AllCarsScreen));
            AddLevel("twoCanalScreen", typeof(TwoCanalScreen));
            AddLevel("housesScreen", typeof(HousesScreen));
            AddLevel("allWaterScreen", typeof(AllWaterScreen));
            AddLevel("twoTrainScreen", typeof(TwoTrainScreen));

            AddLevel("allTrainScreen", typeof(AllTrainScreen));
            AddLevel("laneChangeScreen", typeof(LaneChangeScreen)); // must be 12th
            AddLevel("twoCanalTwoTrainScreen", typeof(TwoCanalTwoTrainScreen));
            AddLevel("twoBoulevardScreen", typeof(TwoBoulevardScreen));
            AddLevel("steamRollerScreen", typeof(SteamRollerScreen));
        }
        private uint levelNumber = 0;
        private void AddLevel(string levelName, Type levelType)
        {
            SymbolImport si = new SymbolImport("screens", levelName);
            ConstructorInfo ci = levelType.GetConstructor(new Type[]{si.GetType()});
            object o = ci.Invoke(new object[] { si });
            ((BaseScreen)o).levelNumber = levelNumber++;
            stage.AddScreen((BaseScreen)o);
        }
        protected override void Initialize()
        {
			base.Initialize();
			this.isFullScreen = false;

			NetworkManager.Instance.OnNewGamer += new NetworkManager.NewGamerDelegate(NewGamerHandler);
			NetworkManager.Instance.OnGamerLeft += new NetworkManager.GamerLeftDelegate(GamerLeftHandler);
        }

        public override void AddingScreen(Screen screen)
        {
            base.AddingScreen(screen);
            GameOverlay.Deactivate();
        }
        public override void RemovingScreen(Screen screen)
        {
            base.RemovingScreen(screen);
            if (screen is BaseScreen && screen.Contains(gameOverlay))
            {
                GameOverlay.Deactivate();              
            }
        }

		void NewGamerHandler(Microsoft.Xna.Framework.Net.NetworkGamer gamer, int gamerIndex)
		{
			AddGamer(gamer, gamerIndex);
		}
		void GamerLeftHandler(Microsoft.Xna.Framework.Net.NetworkGamer gamer, int gamerIndex)
		{
			RemoveGamer(gamer);
		}

		public override void AddGamer(NetworkGamer gamer, int gamerIndex)
		{
			base.AddGamer(gamer, gamerIndex);
			if (gamer.IsLocal)
			{
				gamer.IsReady = true;
			}
			// todo: possible on the fly additions
			//CreatePlayer(gamer, gamerIndex);
		}
		public override void RemoveGamer(NetworkGamer gamer)
		{
			base.RemoveGamer(gamer);
		}


        public override void ExitToMainMenu()
        {
            base.ExitToMainMenu();
            stage.SetScreen("entryScreen");
        }
        public override void AllLevelsComplete()
        {
            base.ExitToMainMenu();
            titleScreen.allLevelsComplete = true;
            stage.SetScreen("entryScreen");
        }

        protected override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Update(gameTime);
        }
        protected override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
		{
			//GraphicsDevice.RenderState.DepthBufferEnable = true;
            base.Draw(gameTime);
        }
    }
}
