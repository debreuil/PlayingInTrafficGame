using System;
using System.Collections.Generic;
using System.Text;
using DDW.V2D;
using Microsoft.Xna.Framework.Graphics;
using DDW.Display;
using V2DRuntime.Components;

namespace Smuck.Components
{
    public class ScoreBox : Sprite
    {
        public TextBox txScore;
        public TextBox txTotal;
        public Sprite faces;

        public ScoreBox(Texture2D texture, V2DInstance instance)  : base(texture, instance)
		{
        }
        public override void Initialize()
        {
            base.Initialize();
            txScore.Text = "";
            txTotal.Text = "";
            faces.GotoAndStop(1);
            this.Visible = false;
        }

        private SmuckPlayer player;
        public SmuckPlayer Player
        {
            get
            {
                return player;
            }
            set
            {
                player = value;
                if (player == null)
                {
                    this.Visible = false;
                }
                else
                {
                    this.Visible = true;
                    faces.GotoAndStop((uint)player.Index);
                }
            }
        }
        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Update(gameTime);
            if (player != null)
            {
                if (txScore.Text != player.roundFinalScore.ToString())
                {
                    txScore.Text = player.roundFinalScore.ToString();
                }

                if (txTotal.Text != player.totalScore.ToString())
                {
                    txTotal.Text = player.totalScore.ToString();
                }
            }
        }
    }
}
