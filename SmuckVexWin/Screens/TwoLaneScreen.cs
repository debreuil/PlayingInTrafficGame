
using Smuck.Enums;
using V2DRuntime.Shaders;
using V2DRuntime.V2D;
using Smuck.Components;
using DDW.V2D;
using Smuck.Shaders;
namespace Smuck.Screens
{
    //[V2DShaderAttribute(shaderType = typeof(DesaturationShader), param0 = 1f)]
    [V2DScreenAttribute(gravityX = 0, gravityY = 0, backgroundColor = 0x000000, debugDraw = false)]
    public class TwoLaneScreen : BaseScreen
    {
        public TwoLaneScreen(V2DContent v2dContent)  : base(v2dContent)
        {
        }
        public TwoLaneScreen(SymbolImport si) : base(si)
        {
            SymbolImport = si;
        }
        protected override void LevelInit()
        {
            LevelKind = Level.TwoLane;
            SetGoals(3, 4);//6);
            this.endOnLastLane = true;
        }
        protected override void LaneInit()
        {
            // **note: don't call base, this is two lanes

            laneCount = 4;
            int laneTop = 120;
            int laneHeight = 240;
            if (lanes == null)
            {
                lanes = new Lane[laneCount];
            }

            lanes[0] = new Lane(0, laneTop, LaneKind.Empty);
            for (int i = 1; i < lanes.Length; i++)
            {
                lanes[i] = new Lane(i, laneHeight, LaneKind.WideCar);
                lanes[i].vehicleSpeed = 10;// rnd.Next(8, 15);
                lanes[i].minCreationDelay = (int)(10000 / lanes[i].vehicleSpeed);
                lanes[i].maxCreationDelay = 6000;
                lanes[i].movesRight = i >= lanes.Length / 2;
                lanes[i].yLocation = laneTop + lanes[i].laneHeight * (i - 1);
            }
            lanes[lanes.Length - 1].LaneKind = LaneKind.Empty;
            lanes[lanes.Length - 1].laneHeight = 120;
        }
    }
}








