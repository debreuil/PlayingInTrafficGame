using Smuck.Enums;
using V2DRuntime.Shaders;
using V2DRuntime.V2D;
using Smuck.Components;
using DDW.V2D;
using Smuck.Shaders;
using DDW.Display;
using System.Collections.Generic;
using V2DRuntime.Attributes;
namespace Smuck.Screens
{
    //[V2DShaderAttribute(shaderType = typeof(DesaturationShader), param0 = 1f)]
    [V2DScreenAttribute(gravityX = 0, gravityY = 0, backgroundColor = 0x000000, debugDraw = false)]
    public class SteamRollerScreen : BaseScreen
    {
        [V2DSpriteAttribute(depthGroup = 30, categoryBits = Category.VEHICLE, maskBits = Category.DEFAULT | Category.VEHICLE | Category.PLAYER | Category.WATER, angularDamping = 9F)] // todo: mave ang damp to LV attribute
        protected SteamRoller steamRoller;

        public SteamRollerScreen(V2DContent v2dContent)  : base(v2dContent)
        {
        }
        public SteamRollerScreen(SymbolImport si) : base(si)
        {
            SymbolImport = si;
        }
        protected override void LevelInit()
        {
            LevelKind = Level.SteamRoller;
            SetGoals(9, 100);
        }
        protected override void LaneInit()
        {
            base.LaneInit();

            lanes[0].LaneKind = LaneKind.Empty;
            lanes[1].LaneKind = LaneKind.Empty;
            lanes[2].LaneKind = LaneKind.Empty;
            lanes[3].LaneKind = LaneKind.Empty;
            lanes[4].LaneKind = LaneKind.Empty;
            lanes[5].LaneKind = LaneKind.Empty;
            lanes[6].LaneKind = LaneKind.Empty;
            lanes[7].LaneKind = LaneKind.Empty;
            lanes[8].LaneKind = LaneKind.Empty;
            lanes[9].LaneKind = LaneKind.Empty;

            int steamLane = 4;
            lanes[steamLane].LaneKind = LaneKind.SteamRoller;
            lanes[steamLane].oneTimeVehicle = true;
            lanes[steamLane].vehicleSpeed = 15;
            lanes[steamLane].minCreationDelay = 2000;
            lanes[steamLane].maxCreationDelay = 6000;
            lanes[steamLane].movesRight = false;
        }
    }
}








