using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DDW.Display;
using Microsoft.Xna.Framework.Graphics;
using DDW.V2D;

namespace Smuck.Components
{
    public class PlayerIndicatorLights : Sprite
    {
        public Sprite piLightBkg;
        public Sprite[] piLight;

        public PlayerIndicatorLights(Texture2D texture, V2DInstance instance)  : base(texture, instance)
		{
        }
        public override void Initialize()
        {
            base.Initialize();
            piLight[0].GotoAndStop(1);
        }
    }
}
