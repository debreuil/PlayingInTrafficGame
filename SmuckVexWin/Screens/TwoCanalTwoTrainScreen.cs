using System;
using System.Collections.Generic;
using V2DRuntime.Shaders;
using DDW.V2D;
using Smuck.Shaders;
using V2DRuntime.V2D;
using V2DRuntime.Attributes;
using Smuck.Enums;
using DDW.Display;

namespace Smuck.Screens
{
    [V2DScreenAttribute(gravityX = 0, gravityY = 0, backgroundColor = 0x000000, debugDraw = false)]
    public class TwoCanalTwoTrainScreen : BaseScreen
    {
        [V2DSpriteAttribute(isStatic = true, categoryBits = Category.WATER, maskBits = Category.PLAYER, isSensor = true)]
        protected V2DSprite[] water;
        [V2DSpriteAttribute(isStatic = true, categoryBits = Category.WATER, maskBits = Category.PLAYER)]
        protected V2DSprite[] bridgeRail;

        [SpriteAttribute(depthGroup = 14)]
        protected Sprite[] bridge;

        public TwoCanalTwoTrainScreen(V2DContent v2dContent) : base(v2dContent)
        {
        }
        public TwoCanalTwoTrainScreen(SymbolImport si) : base(si)
        {
            SymbolImport = si;
        }

        protected override void LevelInit()
        {
            LevelKind = Level.TwoCanalTwoTrain;

            lanes[3].LaneKind = LaneKind.DrownWater;
            lanes[3].vehicleSpeed = 5;
            lanes[3].minCreationDelay = 8000;
            lanes[3].maxCreationDelay = 15000;

            lanes[4].LaneKind = LaneKind.Train;
            lanes[4].minCreationDelay = 2000;
            lanes[4].maxCreationDelay = 8000;
            lanes[5].LaneKind = LaneKind.Train;
            lanes[5].minCreationDelay = 3000;
            lanes[5].maxCreationDelay = 6000;

            lanes[6].LaneKind = LaneKind.DrownWater;
            lanes[6].vehicleSpeed = 5;
            lanes[6].minCreationDelay = 8000;
            lanes[6].maxCreationDelay = 15000;

        }
    }
}




