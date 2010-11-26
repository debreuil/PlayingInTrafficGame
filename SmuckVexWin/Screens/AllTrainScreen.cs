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
    
    public class AllTrainScreen : BaseScreen
    {
        [SpriteAttribute(depthGroup = 90)]
        protected Sprite popupStevie;

        public AllTrainScreen(V2DContent v2dContent): base(v2dContent)
        {
        }
        public AllTrainScreen(SymbolImport si): base(si)
        {
            SymbolImport = si;
        }
        protected override void LevelInit()
        { 
            LevelKind = Level.AllTrain;
            SetGoals(9, 4);
        }
        protected override void LaneInit()
        {
            base.LaneInit(); // new

            int[] speeds = new int[] { 0, 15, 20, 25, 290, 290, 25, 20, 15, 0 };
            //int[] maxCreation = new int[] { 0, 7000, 8000, 9000, 15000, 15000, 8000, 7000, 7000, 0 };
            int[] maxCreation = new int[] { 0, 10000, 15000, 19000, 23000, 23000, 19000, 15000, 10000, 0 };
            for (int i = 1; i < lanes.Length - 1; i++)
            {                
                lanes[i].LaneKind = LaneKind.Train;
                lanes[i].minCreationDelay = 6000;
                lanes[i].maxCreationDelay = maxCreation[i];
                lanes[i].vehicleSpeed = speeds[i];
            }
            lanes[4].LaneKind = LaneKind.Shinkansen;
            lanes[4].minCreationDelay = 10000;
            lanes[5].LaneKind = LaneKind.Shinkansen;
            lanes[5].minCreationDelay = 10000;
        }
        bool steviePlayed = false;
        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Update(gameTime);
            if (steviePlayed && popupStevie.isPlaying && popupStevie.CurChildFrame == popupStevie.FrameCount - 1)
            {
                popupStevie.Stop();
            }
            else if(popupStevie.CurChildFrame == 2)
            {
                steviePlayed = true;
            }
        }
        protected override Smuck.Components.SmuckPlayer CreatePlayer(Microsoft.Xna.Framework.Net.NetworkGamer g, int gamerIndex)
        {
            if (popupStevie.CurFrame == 0)
            {
                popupStevie.GotoAndPlay(1);
                steviePlayed = false;
                stage.audio.PlaySound("stevie_dodge");
            }
            return base.CreatePlayer(g, gamerIndex);
        }
    }
}




