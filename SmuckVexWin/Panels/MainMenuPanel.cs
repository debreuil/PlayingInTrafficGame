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
using Smuck.Audio;

namespace Smuck.Panels
{
    public class MainMenuPanel : Panel
    {
        public ButtonTabGroup menuButtons;
        private MenuState[] buttonTargets = new MenuState[]
        {
            MenuState.QuickGame,
            //MenuState.NetworkGame, 
            MenuState.HighScores, 
            MenuState.UnlockTrial,
            MenuState.Options,
            MenuState.Exit,
            MenuState.Lobby, 
        };
        public MainMenuPanel(Texture2D texture, V2DInstance inst) : base(texture, inst) { }

        void menuButtons_OnClick(Button sender, int playerIndex, TimeSpan time)
        {
            MenuState ms = buttonTargets[sender.Index];
            ((StartScreen)parent).SetPanel(ms);
        }
        void menuButtons_OnFocusChanged(Button sender)
        {
            stage.audio.PlaySound(Sfx.navigateSound);
        }
        public override void Initialize()
        {
            base.Initialize();
        }
        public override void Activate()
        {
            base.Activate();
            menuButtons.SetFocus(0);
            menuButtons.OnClick += new ButtonEventHandler(menuButtons_OnClick);
            menuButtons.OnFocusChanged += new FocusChangedEventHandler(menuButtons_OnFocusChanged);
        }
        public override void Deactivate()
        {
            base.Deactivate();
			menuButtons.OnClick -= new ButtonEventHandler(menuButtons_OnClick);
            menuButtons.OnFocusChanged -= new FocusChangedEventHandler(menuButtons_OnFocusChanged);
        }
    }
}