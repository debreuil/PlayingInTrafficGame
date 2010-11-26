using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DDW.V2D;
using DDW.Display;
using Microsoft.Xna.Framework.Graphics;

namespace Smuck.Components
{
    public class SteamRoller : LaneVehicle
    {
        protected List<Sprite> tire;
        protected Sprite roller;
        protected Sprite stevie;

        public SteamRoller(Texture2D texture, V2DInstance instance) : base(texture, instance)
		{
		}
        public override void Initialize()
        {
            base.Initialize();
        }
        protected override void OnAddToStageComplete()
        {
            base.OnAddToStageComplete();
            Play();
        }
        public override void Play()
        {
            base.Play();
            tire[0].Play();
            tire[1].Play();
            stevie.Play();
            roller.Play();
        }
        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Update(gameTime);
            //Play();
        }
    }
}
