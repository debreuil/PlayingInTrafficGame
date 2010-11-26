
using System;
using System.Collections.Generic;
using V2DRuntime.Shaders;
using DDW.V2D;
using Smuck.Shaders;
using V2DRuntime.V2D;
using Smuck.Enums;
using V2DRuntime.Attributes;
using DDW.Display;

namespace Smuck.Screens
{
    //[V2DShaderAttribute(shaderType = typeof(DesaturationShader), param0 = 1f)]
    [V2DScreenAttribute(gravityX = 0, gravityY = 0, backgroundColor = 0x000000, debugDraw = false)]
    public class TwoCanalTwoBoulevardScreen : BaseScreen
    {
        [V2DSpriteAttribute(depthGroup = 20, linearDamping = 30, angularDamping = 40, categoryBits = Category.PLAYER, maskBits = Category.DEFAULT | Category.VEHICLE | Category.ICON | Category.PLAYER)]
        public V2DSprite[] park;
        [V2DSpriteAttribute(depthGroup = 120, isStatic = true, categoryBits = Category.PLAYER, maskBits = Category.DEFAULT | Category.BORDER | Category.VEHICLE | Category.ICON | Category.PLAYER)]
        public V2DSprite[] tree;
        [V2DSpriteAttribute(depthGroup = 20, isStatic = true, categoryBits = Category.PLAYER, maskBits = Category.DEFAULT | Category.BORDER | Category.VEHICLE | Category.ICON | Category.PLAYER)]
        public V2DSprite[] bench;

        [V2DSpriteAttribute(isStatic = true, categoryBits = Category.WATER, maskBits = Category.PLAYER, isSensor = true)]
        protected V2DSprite[] water;
        [V2DSpriteAttribute(isStatic = true, categoryBits = Category.WATER, maskBits = Category.PLAYER)]
        protected V2DSprite[] bridgeRail;
        [SpriteAttribute(depthGroup = 14)]
        protected Sprite[] bridge;

        public TwoCanalTwoBoulevardScreen(V2DContent v2dContent)  : base(v2dContent)
        {
        }
        public TwoCanalTwoBoulevardScreen(SymbolImport si) : base(si)
        {
            SymbolImport = si;
        }

        protected override void LevelInit()
        {
            LevelKind = Level.TwoCanalTwoBoulevard;
            //SetGoals(9, 2);
        }
        protected override void  LaneInit()
        {
            base.LaneInit();
            lanes[2].LaneKind = LaneKind.DrownWater;
            lanes[2].yLocation += 7;
            lanes[2].minCreationDelay = 2000;
            lanes[2].maxCreationDelay = 8000;
            lanes[2].oneVehiclePerLane = true;

            lanes[3].yLocation += 5;
            lanes[6].yLocation += 5;
            lanes[8].yLocation += 5;

            lanes[4].LaneKind = LaneKind.Empty;
            lanes[5].LaneKind = LaneKind.Empty;

            lanes[7].LaneKind = LaneKind.DrownWater;
            lanes[7].minCreationDelay = 3000;
            lanes[7].maxCreationDelay = 6000;
            lanes[7].oneVehiclePerLane = true;
        }
    }
}








