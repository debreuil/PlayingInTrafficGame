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
        [SpriteAttribute(depthGroup = 90)]
        protected Sprite popupStevie;

        bool steviePlayed = false;
        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Update(gameTime);
            if (steviePlayed && popupStevie.isPlaying && popupStevie.CurChildFrame == popupStevie.FrameCount - 1)
            {
                popupStevie.Stop();
            }
            else if (popupStevie.CurChildFrame == 2)
            {
                steviePlayed = true;
            }
        }
        protected override Smuck.Components.SmuckPlayer CreatePlayer(int gamerIndex)
        {
            int rn = rnd.Next(100);
            Console.WriteLine(rn);
            if (popupStevie.CurFrame == 0 && rn < 5)
            {
                popupStevie.GotoAndPlay(1);
                steviePlayed = false;
                stage.audio.PlaySound("stevie_dodge");
            }
            return base.CreatePlayer(gamerIndex);
        }
    }
}








