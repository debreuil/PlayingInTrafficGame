using System;
using System.Collections.Generic;
using V2DRuntime.Shaders;
using DDW.V2D;
using Smuck.Shaders;
using V2DRuntime.V2D;
using Smuck.Enums;
using V2DRuntime.Attributes;
using DDW.Display;
using Microsoft.Xna.Framework;
using V2DRuntime.Tween;

namespace Smuck.Screens
{
    //[V2DShaderAttribute(shaderType = typeof(DesaturationShader), param0 = 1f)]
    [V2DScreenAttribute(gravityX = 0, gravityY = 0, backgroundColor = 0x000000, debugDraw = false)]
    public class WideBoulevardScreen : BaseScreen
    {
        [V2DSpriteAttribute(depthGroup = 20, linearDamping = 30, angularDamping = 40, categoryBits = Category.PLAYER, maskBits = Category.DEFAULT | Category.VEHICLE | Category.ICON | Category.PLAYER)]
        public V2DSprite[] park;
        [V2DSpriteAttribute(depthGroup = 120, isStatic = true, categoryBits = Category.PLAYER, maskBits = Category.DEFAULT | Category.BORDER | Category.VEHICLE | Category.ICON | Category.PLAYER)]
        public V2DSprite[] tree;
        [V2DSpriteAttribute(depthGroup = 20, isStatic = true, categoryBits = Category.PLAYER, maskBits = Category.DEFAULT | Category.BORDER | Category.VEHICLE | Category.ICON | Category.PLAYER)]
        public V2DSprite[] bench;

        [SpriteAttribute(depthGroup = 500)]
        public Sprite chopper;

        public WideBoulevardScreen(V2DContent v2dContent)  : base(v2dContent)
        {
        }
        public WideBoulevardScreen(SymbolImport si) : base(si)
        {
            SymbolImport = si;
        }
        protected override void LevelInit()
        {
            LevelKind = Level.WideBoulevard;
            TweenWorker tw = chopper.AnimateTo( new V2DRuntime.Tween.TweenState( new Vector2(300, -400), this.Rotation, this.Scale, this.Alpha, this.CurChildFrame), 5000);
            tw.TweenComplete += new V2DRuntime.Display.TweenEvent(tw_TweenComplete);
            chopper.PlayAll();
        }

        void tw_TweenComplete(DisplayObject sender, TweenWorker worker)
        {
            sender.tweenWorker = null;
            sender.DestroyAfterUpdate();
        }
        protected override void LaneInit()
        {
            base.LaneInit();
            lanes[3].LaneKind = LaneKind.Empty;
            lanes[4].LaneKind = LaneKind.Empty;
            lanes[5].LaneKind = LaneKind.Empty;
            lanes[6].LaneKind = LaneKind.Empty;
        }
    }
}








