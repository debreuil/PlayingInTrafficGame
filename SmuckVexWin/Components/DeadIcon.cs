using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using V2DRuntime.Enums;
using DDW.V2D;
using Microsoft.Xna.Framework.Graphics;

namespace Smuck.Components
{
	public class DeadIcon : V2DSprite
	{
		private DeadIconType iconType;

		public DeadIcon(Texture2D texture, V2DInstance instance) : base(texture, instance)
		{
		}

		public DeadIconType IconType
		{
			get { return iconType; }
			set { iconType = value; GotoAndStop((uint)iconType); }
		}
	}
}
