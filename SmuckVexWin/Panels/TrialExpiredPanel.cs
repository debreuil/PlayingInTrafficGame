
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using DDW.V2D;
using V2DRuntime.Display;
using DDW.Input;
using DDW.Display;
using V2DRuntime.Components;
using Smuck.Components;
using Microsoft.Xna.Framework.Input;
using Smuck.Enums;
using V2DRuntime.Panels;
using Microsoft.Xna.Framework.GamerServices;

namespace Smuck.Panels
{
    public class TrialExpiredPanel : VideoPanel
    {
        public ButtonTabGroup endTrialButtons;

        private ExpiredState expiredState;

        public TrialExpiredPanel(Texture2D texture, V2DInstance inst) : base(texture, inst) { }

        public override void Initialize()
        {
            base.Initialize();
            expiredState = ExpiredState.Deciding;
            videoName = "trialPitch";
        }
        public override void Activate()
        {
            base.Activate();
            endTrialButtons.OnClick += new ButtonEventHandler(endTrialButtons_OnClick);
            endTrialButtons.Deactivate();
            endTrialButtons.Visible = false;
        }
        public override void Deactivate()
        {
            base.Deactivate();
            endTrialButtons.OnClick -= new ButtonEventHandler(endTrialButtons_OnClick);
        }
        void endTrialButtons_OnClick(Button sender, int playerIndex, TimeSpan time)
        {
            endTrialButtons.Deactivate();
            switch (sender.Index)
            {
                case 0:
                    SmuckGame.instance.UnlockTrial(playerIndex);
                    break;

                case 1:
                    expiredState = ExpiredState.Ending;
                    videoName = "trialDecline";
                    Activate();
                    break;
            }

        }

        protected override void OnVideoEnded()
        {
            base.OnVideoEnded();
            switch (expiredState)
            {
                case ExpiredState.Deciding:
                    endTrialButtons.Visible = true;
                    endTrialButtons.Activate();
                    endTrialButtons.SetFocus(0);
                    break;
                case ExpiredState.Purchased:
                    SmuckGame.GameOverlay.ResumeGame();
                    //UpgradeTrial(this, null);
                    break;

                default:
                case ExpiredState.Ending:
                    endTrialButtons.Visible = false;
                    Deactivate();
                    SmuckGame.instance.ExitGame();
                    //EndTrial(this, null);
                    break;
            }
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Update(gameTime);
            if (expiredState != ExpiredState.Purchased && !Guide.IsTrialMode)
            {
                expiredState = ExpiredState.Purchased;
                videoName = "trialPurchase";
                Activate();
            }
            else if (!endTrialButtons.isActive && !Guide.IsVisible && expiredState == ExpiredState.Deciding)
            {
                endTrialButtons.Visible = true;
                endTrialButtons.Activate();
                endTrialButtons.SetFocus(0);
            }
        }

        private enum ExpiredState
        {
            Deciding,
            Purchasing,
            Ending,
            Purchased,
        }
    }
}
