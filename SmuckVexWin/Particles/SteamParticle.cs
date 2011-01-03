using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using V2DRuntime.Particles;
using Microsoft.Xna.Framework.Graphics;
using DDW.V2D;
using V2DRuntime.Tween;
using Microsoft.Xna.Framework;

namespace Smuck.Particles
{
	public class SteamParticle : ParticleEffect
	{
        public float particleScale = 1;
        public Vector2 direction = Vector2.Zero;
        public SteamParticle()
		{
		}
		public SteamParticle(Texture2D texture, V2DInstance inst) : base(texture, inst)
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
            particleCount = 3;
		}
		protected override void BatchUpdate(Microsoft.Xna.Framework.GameTime gameTime)
		{
			base.BatchUpdate(gameTime);

            float a = Easing.EaseOutQuint(t, .8f, -.8f);
            pColor = new Color(a, a, a, a);

			effectOrigin.X = 10f * r0;
			pScale.X =Easing.Linear(t, 0f, 15f) * particleScale;
			pScale.Y = pScale.X; // r0 + 1f;
		}
		protected override void BatchDraw(Microsoft.Xna.Framework.Graphics.SpriteBatch batch)
		{
			base.BatchDraw(batch);
		}
		protected override void ParticleDraw(int index, Microsoft.Xna.Framework.Graphics.SpriteBatch batch)
		{
			sourceRectangle.X = (index % 4) * particleWidth;
			pRotation = Easing.EaseOutQuad(t, 0, r0 * (float)Math.PI);

            float len = Easing.Linear(t, 0, r0 * 200f);
			Vector2 dir = direction;// + 20 * r0 - 10;
			pPosition.X = (float)(dir.X * len + effectPosition.X);
			pPosition.Y = (float)(dir.Y * len + effectPosition.Y);

			base.ParticleDraw(index, batch);
		}
	}
}
