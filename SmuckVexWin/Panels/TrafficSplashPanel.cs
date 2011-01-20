using DDW.V2D;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using V2DRuntime.Panels;
using Smuck.Screens;
using Smuck.Enums;

namespace Smuck.Panels
{
	public class TrafficSplashPanel : VideoPanel
	{
		public TrafficSplashPanel(Texture2D texture, V2DInstance inst) : base(texture, inst) { }

        public override void Initialize()
        {
            base.Initialize();
            videoName = "trafficSplash";
        }
        public override void Activate()
        {
            base.Activate();
        }
        public override bool OnPlayerInput(int playerIndex, DDW.Input.Move move, System.TimeSpan time)
        {
            bool result = base.OnPlayerInput(playerIndex, move, time );
            if (move.Releases == Microsoft.Xna.Framework.Input.Buttons.A)
            {
                result = false;
                videoPlayer.Stop();
            }
            return result;
        }
        protected override void OnVideoEnded()
        {
            base.OnVideoEnded();
            ((StartScreen)parent).nextState = MenuState.MainMenu;
        }
	}
}
