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
    public class CrosswalkScreen : BaseScreen
    {
        public CrosswalkScreen(V2DContent v2dContent)  : base(v2dContent)
        {
        }
        public CrosswalkScreen(SymbolImport si) : base(si)
        {
            SymbolImport = si;
        }
        protected override void LevelInit()
        {
            LevelKind = Level.Crosswalk;
            //vehicleCount = 3;
            //laneCount = 8;
            //firstLaneY = 61;
            //laneHeight = 67;

            //laneLocations = new int[laneCount];
            //for (int i = 0; i < laneCount; i++)
            //{
            //    laneLocations[i] = firstLaneY + i * laneHeight;
            //}
        }
        protected override void LaneInit()
        {
            base.LaneInit();
            for (int i = 3; i < lanes.Length - 3; i++)
            {
                lanes[i].vehicleSpeed -= 10;
            }
        }
    }
}








