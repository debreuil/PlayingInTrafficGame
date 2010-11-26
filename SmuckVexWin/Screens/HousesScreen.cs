using System;
using System.Collections.Generic;
using V2DRuntime.Shaders;
using DDW.V2D;
using Smuck.Shaders;
using V2DRuntime.V2D;
using V2DRuntime.Attributes;
using Smuck.Enums;

namespace Smuck.Screens
{
    //[V2DShaderAttribute(shaderType = typeof(DesaturationShader), param0 = 1f)]
    [V2DScreenAttribute(gravityX = 0, gravityY = 0, backgroundColor = 0x000000, debugDraw = false)]
    public class HousesScreen : BaseScreen
    {
        [V2DSpriteAttribute(isStatic = true, categoryBits = Category.WATER, maskBits = Category.PLAYER)]
        protected V2DSprite[] house;

        public HousesScreen(V2DContent v2dContent) : base(v2dContent)
        {
        }
        public HousesScreen(SymbolImport si) : base(si)
        {
            SymbolImport = si;
        }

        protected override void LevelInit()
        {
            LevelKind = Level.Houses;
        }
        protected override void LaneInit()
        {
            base.LaneInit();
            lanes[3].LaneKind = LaneKind.Empty;
            lanes[6].LaneKind = LaneKind.Empty;

            lanes[2].minCreationDelay = 400;
            lanes[2].maxCreationDelay = 2000;
            lanes[7].minCreationDelay = 400;
            lanes[7].maxCreationDelay = 2000;

            lanes[4].vehicleSpeed = 60;
            lanes[5].vehicleSpeed = 60;
        }
    }
}




