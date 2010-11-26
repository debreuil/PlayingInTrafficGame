using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using V2DRuntime.Particles;
using Microsoft.Xna.Framework.Graphics;
using DDW.V2D;
using V2DRuntime.Tween;

namespace Smuck.Particles
{
	public class StarParticles : ParticleEffect
	{
        public float particleScale = 1;
		public StarParticles()
		{
		}
		public StarParticles(Texture2D texture, V2DInstance inst) : base(texture, inst)
		{
		}
		public override void Initialize()
		{
			base.Initialize();
			AutoStart = true;
			maxT = 1;
			steps = 60;
		}

		protected override void EffectSetup(Microsoft.Xna.Framework.GameTime gameTime)
		{
			base.EffectSetup(gameTime);
			textureCount = 4;
            particleCount = 10;
		}
		private float rotSpeed = 0;
		protected override void BatchUpdate(Microsoft.Xna.Framework.GameTime gameTime)
		{
            base.BatchUpdate(gameTime);

            pColor.A = (byte)Easing.Linear(t, 100f, -100f);
            effectOrigin.X = 10f * r0;// Easing.Linear(t, 0, r1 * 90);//r2 * 2;			
            //particleCount = (int)Easing.Sin(t, 0, 10000, .5f);
            rotSpeed = Easing.EaseInCubic(t, 0f, -20f);
            particleCount = (int)Easing.Linear(t, 0f, 20f);
		}
		protected override void BatchDraw(Microsoft.Xna.Framework.Graphics.SpriteBatch batch)
		{
			base.BatchDraw(batch);
		}
		protected override void ParticleDraw(int index, Microsoft.Xna.Framework.Graphics.SpriteBatch batch)
		{
            sourceRectangle.X = (index % 4) * particleWidth;
            pRotation = Easing.EaseOutQuad(t, 0, r0 * (float)Math.PI);

            float len = Easing.Linear(t * .25f, 0, r0 * 40f);
            float pt = index / (float)particleCount;
            float dir = Easing.twoPi * r0 + rotSpeed;
            pScale.X = Easing.EaseOutBounce(t, .1f, 3f) * particleScale;
            pScale.Y = pScale.X; // r0 + 1f;
            pPosition.X = (float)Math.Sin(dir) * len + effectPosition.X;
            pPosition.Y = (float)Math.Cos(dir) * len + effectPosition.Y;

            base.ParticleDraw(index, batch);
		}
	}
}
