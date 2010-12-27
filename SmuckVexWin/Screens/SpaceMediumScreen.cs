using System;
using System.Collections.Generic;
using V2DRuntime.Shaders;
using DDW.V2D;
using Smuck.Shaders;
using V2DRuntime.V2D;
using V2DRuntime.Attributes;
using Smuck.Enums;
using Smuck.Components;
using Microsoft.Xna.Framework.Graphics;
using Smuck.Particles;
using DDW.Display;
using V2DRuntime.Game;
using V2DRuntime.Enums;

namespace Smuck.Screens
{
    [V2DScreenAttribute(gravityX = 0, gravityY = 0, backgroundColor = 0x000000, debugDraw = false)]
    public class SpaceMediumScreen : BaseScreen
    {
        [V2DSpriteAttribute(depthGroup = 15)]
        public SteamParticle steamParticle;
        [V2DSpriteAttribute(depthGroup = 15)]
        public SteamParticleGroup steamParticles;

        //[V2DShaderAttribute(shaderType = typeof(DesaturationShader), param0 = .3f)]
        [V2DSpriteAttribute(depthGroup = 30, categoryBits = Category.VEHICLE, maskBits = Category.DEFAULT | Category.PLAYER | Category.WATER, angularDamping = 9F)] // todo: mave ang damp to LV attribute
        new public SpaceVehicle vehicle;

        public SpaceMediumScreen(V2DContent v2dContent) : base(v2dContent)
        {
        }
        public SpaceMediumScreen(SymbolImport si) : base(si)
        {
            SymbolImport = si;
        }

        protected override void LevelInit()
        {
            LevelKind = Level.SpaceMedium;

            for (int i = 1; i < lanes.Length - 1; i++)
            {                
                lanes[i].LaneKind = LaneKind.Spaceship;
                lanes[i].minCreationDelay = 1000;
                lanes[i].maxCreationDelay = 6000;
                lanes[i].movesRight = rnd.Next(2) > 0;
                lanes[i].vehicleSpeed = 15;
            }
            lanes[0].LaneKind = LaneKind.Spaceship;
            lanes[0].allowVehicles = false;
            lanes[lanes.Length - 1].LaneKind = LaneKind.Spaceship;
            lanes[lanes.Length - 1].allowVehicles = false;

            lanes[4].vehicleSpeed = 30;
            lanes[5].vehicleSpeed = 30;
        }

        protected override SmuckPlayer CreatePlayer(int gamerIndex)
        {
            SmuckPlayer p = base.CreatePlayer(gamerIndex);
            p.dampingRatio = .99f;
            p.autoCenterOnLane = false;
            p.impulseScale = 10f;
            return p;
        }
        //public override void DestroyElement(DisplayObject obj)
        //{
        //    base.DestroyElement(obj);
        //    if (obj is SteamParticleGroup)
        //    {
        //        this.steamParticles = null;
        //        //((DesaturationShader)defaultShader).level = 1f;
        //    }
        //}
        public override void Removed(EventArgs e)
        {
            base.Removed(e);
            if (steamParticles != null)
            {
                RemoveChild(steamParticles);
            }
        }
        protected override void DrawChild(DDW.Display.DisplayObject d, SpriteBatch batch)
        {
            base.DrawChild(d, batch);
        }
        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Update(gameTime);
        }
    }
}




