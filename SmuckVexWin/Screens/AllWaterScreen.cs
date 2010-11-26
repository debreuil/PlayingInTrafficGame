using System;
using System.Collections.Generic;
using V2DRuntime.Shaders;
using DDW.V2D;
using Smuck.Shaders;
using V2DRuntime.V2D;
using V2DRuntime.Attributes;
using Smuck.Enums;
using DDW.Display;
using Smuck.Components;

namespace Smuck.Screens
{
    [V2DScreenAttribute(gravityX = 0, gravityY = 0, backgroundColor = 0x000000, debugDraw = false)]
    public class AllWaterScreen : BaseScreen
    {
        [V2DSpriteAttribute(depthGroup = 25, categoryBits = Category.VEHICLE, maskBits = Category.DEFAULT | Category.PLAYER, angularDamping = 100F)]
        public new LaneVehicle boat;
        [V2DSpriteAttribute(depthGroup = 30, categoryBits = Category.VEHICLE, maskBits = Category.DEFAULT | Category.PLAYER, angularDamping = 100F)]
        public LaneVehicle carrier;

        //[V2DSpriteAttribute(isStatic = true, categoryBits = Category.WATER, maskBits = Category.PLAYER, isSensor = true)]
        //protected V2DSprite[] water;
        //[V2DSpriteAttribute(isStatic = true, categoryBits = Category.WATER, maskBits = Category.PLAYER)]
        //protected V2DSprite[] bridgeRail;

        //[SpriteAttribute(depthGroup = 14)]
        //protected Sprite[] bridge;

        public AllWaterScreen(V2DContent v2dContent) : base(v2dContent)
        {
        }
        public AllWaterScreen(SymbolImport si) : base(si)
        {
            SymbolImport = si;
        }

        protected override void LevelInit()
        {
            LevelKind = Level.AllWater;

            collideWithBoats = true;

            int[] speeds = new int[] { 0, 20, 5, 8, 70, 70, 8, 5, 20, 0 };
            //int[] maxCreation = new int[] { 0, 7000, 8000, 9000, 15000, 15000, 8000, 7000, 7000, 0 };
            int[] minCreation = new int[] { 0, 4000,  15000,  3000,  3000,  1000,  3000,  15000,  4000,  0 };
            int[] maxCreation = new int[] { 0, 6000,  40000, 5000,  8000,  9000,  5000,  40000, 6000,  0 };
            //int[] maxCreation = new int[] { 0, 30000, 15000, 19000, 10000, 10000, 19000, 15000, 30000, 0 };
            for (int i = 1; i < lanes.Length - 1; i++)
            {
                lanes[i].LaneKind = LaneKind.SwimWater;
                lanes[i].minCreationDelay = minCreation[i];
                lanes[i].maxCreationDelay = maxCreation[i];
                lanes[i].vehicleSpeed = speeds[i];
            }
            lanes[0].LaneKind = LaneKind.SwimWater;
            lanes[0].allowVehicles = false;
            //lanes[4].minCreationDelay = 7000;
            lanes[9].LaneKind = LaneKind.SwimWater;
            lanes[9].allowVehicles = false;
            //lanes[5].minCreationDelay = 7000;

            lanes[2].oneVehiclePerLane = true;
            lanes[2].createVehicleOnStartup = false;
            lanes[7].oneVehiclePerLane = true;
            lanes[7].createVehicleOnStartup = false;

            for (int i = 0; i < lanes.Length; i++)
            {
                lanes[i].playerSpeedDamping = .1f;                
            }            
        }
        //protected override Smuck.Components.SmuckPlayer CreatePlayer(Microsoft.Xna.Framework.Net.NetworkGamer g, int gamerIndex)
        //{
        //    SmuckPlayer p = base.CreatePlayer(g, gamerIndex);
        //    p.ReplaceView("swimPlayer0");
        //    return p;
        //}
    }
}




