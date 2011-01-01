using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using V2DRuntime.Shaders;
using DDW.V2D;
using Microsoft.Xna.Framework.Graphics;
using DDW.Display;

namespace Smuck.Shaders
{
	public class ParticleShader : V2DShader
	{
		public ParticleShader(params float[] parameters) : base(parameters)
		{
			LoadContent();
		}
		protected override void LoadContent()
		{
			//effect = V2DGame.contentManager.Load<Effect>("Particle");
            //effect.CurrentTechnique = effect.Techniques[0];
		}
        public override void Begin(SpriteBatch batch)
		{
            batch.Begin(
                 SpriteSortMode.Deferred,
                 BlendState.NonPremultiplied,
                 null, //SamplerState.AnisotropicClamp, 
                 null, //DepthStencilState.None, 
                 null, //RasterizerState.CullNone, 
                 null,
                 Stage.SpriteBatchMatrix);

		}
        public override void End(SpriteBatch batch)
		{
			batch.End();
		}
		public override bool Equals(object obj)
		{
			bool result = base.Equals(obj);
            if (result && obj is ParticleShader)
			{
                result = true;// level.Equals(((ParticleShader)obj).level);
			}
			return result;
		}
		public override int GetHashCode()
		{
            return base.GetHashCode();// +effect.GetHashCode();// +(int)(level * 17);
		}
	}
}
