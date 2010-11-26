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
	public class DirectionalParticle : ParticleEffect
	{
		public DirectionalParticle()
		{
		}
		public DirectionalParticle(Texture2D texture, V2DInstance inst) : base(texture, inst)
		{
		}
		public override void Initialize()
		{
			base.Initialize();
			AutoStart = false;
			maxT = 1;
		}

		protected override void EffectSetup(Microsoft.Xna.Framework.GameTime gameTime)
		{
			base.EffectSetup(gameTime);
			textureCount = 4;
		}
		private float rotSpeed = 0;
		protected override void BatchUpdate(Microsoft.Xna.Framework.GameTime gameTime)
		{
			base.BatchUpdate(gameTime);

			pColor.A = (byte)Easing.Sin(t, 10f, 50f, .5f);
			//pColor.R = (byte)(Easing.Linear(t, 0, 255f) * (float)color.R);
			//pColor.G = (byte)(Easing.Linear(t, 255f, 0) * (float)color.G);
			effectOrigin.X = Easing.Linear(t, 0, r1 * 50);//r2 * 2;			
			//particleCount = (int)Easing.Sin(t, 0, 10000, .5f);
			rotSpeed = Easing.EaseInCubic(t, 0f, -20f);
			particleCount = (int)Easing.Linear(t, 5000f, 500f);
		}
		protected override void BatchDraw(Microsoft.Xna.Framework.Graphics.SpriteBatch batch)
		{
			base.BatchDraw(batch);
		}
		protected override void ParticleDraw(int index, Microsoft.Xna.Framework.Graphics.SpriteBatch batch)
		{
			sourceRectangle.X = (index % 4) * particleWidth;
			pRotation = Easing.EaseOutQuad(t, 0, r0 * (float)Math.PI * 100);

			float len = Easing.EaseOutElastic(t * .5f, 0, r0 * 100, 400) + (r0 * r1 * 20f);
			float pt = index / (float)particleCount;
			float dir = Easing.twoPi * (index % 7f) / 7f + r0 * 5f + rotSpeed;
			pScale.X = len / 200f;
			pScale.Y = pScale.X;
			pPosition.X = (float)Math.Sin(dir) * len + effectPosition.X;
			pPosition.Y = (float)Math.Cos(dir) * len + effectPosition.Y;

			base.ParticleDraw(index, batch);
		}
	}
}







