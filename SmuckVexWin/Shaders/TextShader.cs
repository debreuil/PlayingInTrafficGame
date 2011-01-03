using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using V2DRuntime.Shaders;
using DDW.V2D;
using Microsoft.Xna.Framework.Graphics;
using DDW.Display;
using Microsoft.Xna.Framework;

namespace Smuck.Shaders
{
	public class TextShader : V2DShader
	{
		public TextShader(params float[] parameters) : base(parameters)
		{
			LoadContent();
		}
		protected override void LoadContent()
		{
            effect = V2DGame.contentManager.Load<Effect>("Text");
            effect.CurrentTechnique = effect.Techniques[0];
		}
		public override void Begin(SpriteBatch batch)
		{
            //batch.GraphicsDevice.Textures


            batch.Begin(
                 SpriteSortMode.Deferred,
                 BlendState.AlphaBlend,
                 SamplerState.LinearWrap, 
                 null, //DepthStencilState.None, 
                 null, //RasterizerState.CullNone, 
                 null,
                 Stage.SpriteBatchMatrix);

		}
		public override void End(SpriteBatch batch)
		{
            //effect.CurrentTechnique.Passes[0].End();
            //effect.End();
            batch.End();
            //batch.GraphicsDevice.BlendState = BlendState.NonPremultiplied;
		}
		public override bool Equals(object obj)
		{
			bool result = base.Equals(obj);
            if (result && obj is TextShader)
			{
                result = true;// level.Equals(((TextShader)obj).level);
			}
			return result;
		}
		public override int GetHashCode()
		{
            return base.GetHashCode() + effect.GetHashCode();// +(int)(level * 17);
		}
	}
}
