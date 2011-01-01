using System;
using System.Collections.Generic;
using V2DRuntime.Shaders;
using DDW.V2D;
using Smuck.Shaders;
using V2DRuntime.V2D;
using Smuck.Enums;
using V2DRuntime.Attributes;
using Smuck.Components;
using DDW.Display;

namespace Smuck.Screens
{
    //[V2DShaderAttribute(shaderType = typeof(DesaturationShader), param0 = 1f)]
    [V2DScreenAttribute(gravityX = 0, gravityY = 0, backgroundColor = 0x000000, debugDraw = false)]
    public class LaneChangeScreen : BaseScreen
    {
        [V2DSpriteAttribute(isStatic = true, isSensor = true, categoryBits = Category.GUIDERAIL, maskBits = Category.VEHICLE)]
        protected List<V2DSprite> laneChanger;

        [V2DSpriteAttribute(depthGroup = 25, linearDamping = 100, angularDamping = 100, categoryBits = Category.GUIDERAIL, maskBits = Category.VEHICLE | Category.PLAYER | Category.GUIDERAIL)]
        protected List<V2DSprite> pylon;

        public LaneChangeScreen(V2DContent v2dContent)  : base(v2dContent)
        {
        }
        public LaneChangeScreen(SymbolImport si) : base(si)
        {
            SymbolImport = si;
        }
        protected override void LevelInit()
        {
            LevelKind = Level.LaneChange;
            SetGoals(9, 8);
            lanes[4].createVehicleOnStartup = false;
            lanes[5].createVehicleOnStartup = false;
        }

        public override void BeginContact(Box2D.XNA.Contact contact)
        {
            base.BeginContact(contact);

            LaneVehicle v = null;
            Sprite ch = null;

            if (collisionObjA is LaneVehicle && collisionObjA.RootName == "laneChanger")
            {
                v = (LaneVehicle)collisionObjA;
                ch = collisionObjB;
            }
            else if (collisionObjB is LaneVehicle && collisionObjA.RootName == "laneChanger")
            {
                v = (LaneVehicle)collisionObjB;
                ch = collisionObjA;  
            }

            if (v != null)
            {
                if (ch.Index == 0)
                {
                    v.laneY = lanes[v.Lane.laneIndex - 1].yLocation + (lanes[v.Lane.laneIndex - 1].laneHeight - v.VisibleHeight) / 2f;
                }
                else
                {
                    v.laneY = lanes[v.Lane.laneIndex + 1].yLocation + (lanes[v.Lane.laneIndex + 1].laneHeight - v.VisibleHeight) / 2f + v.VisibleHeight;
                }
            }
        }
    }
}








