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
    public class TwoBoulevardScreen : BaseScreen
    {
        [V2DSpriteAttribute(depthGroup = 20, linearDamping = 30, angularDamping = 40, categoryBits = Category.PLAYER, maskBits = Category.DEFAULT | Category.VEHICLE | Category.ICON | Category.PLAYER)]
        public V2DSprite[] park;
        [V2DSpriteAttribute(depthGroup = 120, isStatic = true, categoryBits = Category.PLAYER, maskBits = Category.DEFAULT | Category.BORDER | Category.VEHICLE | Category.ICON | Category.PLAYER)]
        public V2DSprite[] tree;
        [V2DSpriteAttribute(depthGroup = 20, isStatic = true, categoryBits = Category.PLAYER, maskBits = Category.DEFAULT | Category.BORDER | Category.VEHICLE | Category.ICON | Category.PLAYER)]
        public V2DSprite[] bench;

        public TwoBoulevardScreen(V2DContent v2dContent) : base(v2dContent)
        {
        }
        public TwoBoulevardScreen(SymbolImport si) : base(si)
        {
            SymbolImport = si;
        }
        protected override void LevelInit()
        {
            LevelKind = Level.TwoBoulevard;
        }
        protected override void LaneInit()
        {
            base.LaneInit();
            lanes[4].LaneKind = LaneKind.Empty;
            lanes[5].LaneKind = LaneKind.Empty;
        }
    }
}




