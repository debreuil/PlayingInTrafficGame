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
	public class TextShader : V2DShader
	{
		public float level;
		public TextShader(params float[] parameters) : base(parameters)
		{
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
                 BlendState.NonPremultiplied,
                 null, //SamplerState.AnisotropicClamp, 
                 null, //DepthStencilState.None, 
                 null, //RasterizerState.CullNone, 
                 effect,
                 Stage.SpriteBatchMatrix);
		}
		public override void End(SpriteBatch batch)
		{
            //effect.CurrentTechnique.Passes[0].End();
            //effect.End();
            batch.End();
		}
		public override bool Equals(object obj)
		{
			bool result = base.Equals(obj);
            if (result && obj is TextShader)
			{
                result = level.Equals(((TextShader)obj).level);
			}
			return result;
		}
		public override int GetHashCode()
		{
			return base.GetHashCode() + effect.GetHashCode() + (int)(level * 17);
		}
	}
}
