using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using V2DRuntime.Components;
using V2DRuntime.Attributes;
using DDW.Display;
using DDW.V2D;
using Microsoft.Xna.Framework.Graphics;
using V2DRuntime.Shaders;
using Smuck.Shaders;

namespace Smuck.Components
{
    public class ScoreView : Sprite
    {
        public List<TextBox> txScore;
        public List<LaneCrossIndicator> laneCrossIndicator;

        public ScoreView(Texture2D texture, V2DInstance instance)  : base(texture, instance)
		{
		}
    }
}
