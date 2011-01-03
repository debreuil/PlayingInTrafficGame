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
	public class DesaturationShader : V2DShader
	{
		public float level;
		public DesaturationShader(params float[] parameters) : base(parameters)
		{
			level = (parameters.Length > 0) ? parameters[0] : 1;
			LoadContent();
		}
		protected override void LoadContent()
		{
			effect = V2DGame.contentManager.Load<Effect>("Test");
            effect.CurrentTechnique = effect.Techniques[0];
		}
        public override void Begin(SpriteBatch batch)
		{
			effect.Parameters["satLevel"].SetValue(level);

            batch.Begin(
                 SpriteSortMode.Deferred,
                 BlendState.AlphaBlend, //BlendState.NonPremultiplied,
                 null, //SamplerState.AnisotropicClamp, 
                 null, //DepthStencilState.None, 
                 null, //RasterizerState.CullNone, 
                 effect,
                 Stage.SpriteBatchMatrix);

		}
        public override void End(SpriteBatch batch)
		{
			batch.End();
		}
		public override bool Equals(object obj)
		{
			bool result = base.Equals(obj);
			if (result && obj is DesaturationShader)
			{
				result = level.Equals(((DesaturationShader)obj).level);
			}
			return result;
		}
		public override int GetHashCode()
		{
			return base.GetHashCode() + effect.GetHashCode() + (int)(level * 17);
		}
	}
}
