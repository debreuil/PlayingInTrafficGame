using System;
using System.Collections.Generic;
using V2DRuntime.Shaders;
using DDW.V2D;
using Smuck.Shaders;
using V2DRuntime.V2D;
using V2DRuntime.Attributes;
using Microsoft.Xna.Framework;
using Smuck.Components;
using Smuck.Enums;

namespace Smuck.Screens
{
    //[V2DShaderAttribute(shaderType = typeof(DesaturationShader), param0 = 1f)]
    [V2DScreenAttribute(gravityX = 0, gravityY = 0, backgroundColor = 0x000000, debugDraw = false)]
    public class TwoTrainScreen : BaseScreen
    {
        public TwoTrainScreen(V2DContent v2dContent)  : base(v2dContent)
        {
        }
        public TwoTrainScreen(SymbolImport si) : base(si)
        {
            SymbolImport = si;
        }
        protected override void LevelInit()
        {
            LevelKind = Level.TwoTrain;
        }
        protected override void LaneInit()
        {
            base.LaneInit();

            lanes[4].LaneKind = LaneKind.Train;
            lanes[4].minCreationDelay = 2000;
            lanes[4].maxCreationDelay = 8000;
            lanes[5].LaneKind = LaneKind.Train;
            lanes[5].minCreationDelay = 3000;
            lanes[5].maxCreationDelay = 6000;
        }

        /*
        private TrainVehicle CreateTrain(int lane)
        {
            TrainVehicle result = null;
            V2DInstance vInst = CreateInstanceDefinition("trainFull0", "train");
            vInst.Depth = 15;
            if (lane < trainLaneCount / 2f)
            {
                vInst.Rotation = (float)System.Math.PI * 2;
            }
            else
            {
                vInst.Rotation = (float)-System.Math.PI;
            }

            result = (TrainVehicle)AddInstance(vInst, this);

            result.Y = firstTrainLane + laneHeight * lane;// laneLocations[lane];
            if (lane < trainLaneCount / 2f)
            {
                result.X = ClientSize.X - 10;
                result.Y += (laneHeight - vInst.Definition.Height) / 2f;
            }
            else
            {
                result.X = 10;
                result.Y += laneHeight - (laneHeight - vInst.Definition.Height) / 2f;
            }

            result.direction = new Vector2((float)System.Math.Cos(vInst.Rotation), (float)System.Math.Sin(vInst.Rotation));
            return result;
        }
        private TrainVehicle RecreateTrain(TrainVehicle oldTrain)
        {
            TrainVehicle result = null;
            this.RemoveInstance(oldTrain);
            result = CreateTrain(rnd.Next(0, trainLaneCount));
            return result;
        }
        private void SetNetTrainTime(GameTime gameTime)
        {
            nextTrain = gameTime.TotalGameTime + new TimeSpan(0, 0, rnd.Next(4, 10)); 
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (nextTrain == TimeSpan.Zero)
            {
                CreateTrain(1); 
                SetNetTrainTime(gameTime);
            }

            if (gameTime.TotalGameTime > nextTrain)
            {
                RecreateTrain(train);
                SetNetTrainTime(gameTime);
            }
        }
         */
    }
}




