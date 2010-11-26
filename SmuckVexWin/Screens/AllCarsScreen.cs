using System;
using System.Collections.Generic;
using V2DRuntime.Shaders;
using DDW.V2D;
using Smuck.Shaders;
using V2DRuntime.V2D;
using Smuck.Enums;

namespace Smuck.Screens
{
    //[V2DShaderAttribute(shaderType = typeof(DesaturationShader), param0 = 1f)]
    [V2DScreenAttribute(gravityX = 0, gravityY = 0, backgroundColor = 0x000000, debugDraw = false)]
    public class AllCarsScreen : BaseScreen
    {
        public AllCarsScreen(V2DContent v2dContent)  : base(v2dContent)
        {
        }
        public AllCarsScreen(SymbolImport si) : base(si)
        {
            SymbolImport = si;
        }
        protected override void LevelInit()
        {
            LevelKind = Level.AllCars;
            SetGoals(9, 8);
            //PointsToWinRound = 9 * 8;

            //lanes[1].minCreationDelay = 1000;
            //lanes[1].maxCreationDelay = lanes[1].minCreationDelay;
            //lanes[2].LaneKind = LaneKind.Empty;
            //lanes[3].LaneKind = LaneKind.Empty;
            //lanes[4].LaneKind = LaneKind.Empty;
            //lanes[5].LaneKind = LaneKind.Empty;
            //lanes[6].LaneKind = LaneKind.Empty;
            //lanes[7].LaneKind = LaneKind.Empty;
            //lanes[8].LaneKind = LaneKind.Empty;

        }
    }
}








