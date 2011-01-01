using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DDW.V2D;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Box2D.XNA;
using Microsoft.Xna.Framework.Net;
using V2DRuntime.Game;
using Smuck.Enums;
using DDW.Display;
using V2DRuntime.Attributes;
using Smuck.Particles;
using V2DRuntime.Enums;
using V2DRuntime.Display;
using Smuck.Screens;
using Microsoft.Xna.Framework.Audio;
using Smuck.Audio;
using Smuck.Shaders;

namespace Smuck.Components
{
    public class SmuckPlayer : Player
	{
		public V2DSprite shadow;

		private SpeedLevel speedLevel= SpeedLevel.Normal;
        private float metersPerStep = .7f;
        private float maxSpeed = 30f;
		private Vector2 lastStepPos;
		public bool goalIsTop;

		public int totalScore;
        public int roundFinalScore;
		public int roundScore;
		protected int roundBestScore;
        protected int previousTotalScore;

		private float[] speeds = { 10f, 20f, 30f, 45f, 60f };
        public int laneCount;
        public const float PI = (float)Math.PI;
        public float dampingRatio;
        public float impulseScale;

        public bool skipDeathMarker = false;
        bool isSettled = false;
        bool isSettling = false;
        bool atMaxSpeed = false;
        public bool autoCenterOnLane = true;
        public bool isExploding = false;

        private bool isOnWater = false;
        public LaneVehicle aboardVehicle;

        public float snapLaneY;

		//public bool CanJump = false;
		//private bool isJumping = false;
		//private bool isJumpingUp = false;

        public SmuckPlayer(Texture2D texture, V2DInstance instance) : base(texture, instance)
        {
        }

        public bool IsOnWater
        {
            get {return isOnWater;}
            set
            {
                isOnWater = value;
                SetView();
            }
        }
        public override LivingState LivingState
        {
            get
            {
                return base.LivingState;
            }
            set
            {
                if (base.LivingState != value) // avoid setview unless state changed
                {
                    base.LivingState = value;
                    SetView();
                    if (value != LivingState.Alive && starParticles != null)
                    {
                        starParticles.isComplete = true;
                    }
                }
                else
                {
                    base.LivingState = value;
                }
            }
        }
		public SpeedLevel SpeedLevel
		{
			get { return speedLevel; }
			set { speedLevel = value; maxSpeed = speeds[(int)speedLevel]; }
        }
		public void Reset(float x, float y, float rot)
		{
			this.roundScore = 0;
            this.lane = null;
            this.playerSkin = PlayerSkin.None;
            this.isExploding = false;
            this.skipDeathMarker = false;
            this.autoCenterOnLane = true;
            this.dampingRatio = .7f;
            this.impulseScale = 50f;
			this.X = x;
			this.Y = y;
			this.Rotation = rot;
            this.SpeedLevel = SpeedLevel.Normal;
            lastStepPos = body.GetPosition();
            goalIsTop = !(lastStepPos.Y < this.screen.ClientSize.Y / 2);
            this.SpeedLevel = SpeedLevel.Normal;
            aboardVehicle = null;
            this.LivingState = LivingState.Alive;
		}
        private Lane lane;
		public Lane Lane
		{
			get { return lane; }
			set 
			{
				if (lane != value)
				{
                    bool newLaneKind = true;

                    if (lane != null)
                    {
                        newLaneKind = lane.LaneKind != value.LaneKind;
					    int dif = value.laneIndex - lane.laneIndex;
					    roundScore += goalIsTop ? -dif : dif;

                        if (roundScore > roundBestScore)
                        {
                            roundBestScore = roundScore;
                        }
                    }

                    goalIsTop = (value.laneIndex >= laneCount - 1) ? true : (value.laneIndex < 1) ? false : goalIsTop;

					lane = value;
                    
                    if (newLaneKind) // minimize calls for SetView
                    {
                        SetView();
                    }
				}
			}
		}

        private void SetView()
        {
            if (lane == null) return;

            if (LivingState == LivingState.Alive)
            {
                switch (lane.LaneKind)
                {
                    case LaneKind.DrownWater:
                        PlayerSkin = PlayerSkin.Normal; // bridge or boat or death
                        break;
                    case LaneKind.SwimWater:
                        PlayerSkin = PlayerSkin.Swimming;
                        break;
                    case LaneKind.Spaceship:
                        PlayerSkin = PlayerSkin.Spacesuit;
                        break;
                    case LaneKind.Train:
                    case LaneKind.Shinkansen:
                        PlayerSkin = PlayerSkin.TrainTrack;
                        break;
                    default :
                        PlayerSkin = PlayerSkin.Normal;
                        break;
                }
            }
            else if (LivingState == LivingState.Dying)
            {
                switch (lane.LaneKind)
                {
                    case LaneKind.SwimWater:
                        skipDeathMarker = true;
                        PlayerSkin = PlayerSkin.Drowning;
                        break;
                    case LaneKind.DrownWater:
                        if (isOnWater)
                        {
                            skipDeathMarker = true;
                            PlayerSkin = PlayerSkin.Drowning;
                        }
                        else
                        {
                            PlayerSkin = PlayerSkin.Tumbling;
                        }
                        break;
                    case LaneKind.Spaceship:
                        PlayerSkin = PlayerSkin.Spacesuit;
                        break;
                    default:
                        PlayerSkin = PlayerSkin.Tumbling;
                        break;
                }
            }
        }
        private PlayerSkin playerSkin;
        private PlayerSkin PlayerSkin
        {
            get { return playerSkin; }
            set
            {
                if (playerSkin != value)
                {
                    playerSkin = value;
                    switch (playerSkin)
                    {
                        case PlayerSkin.None:
                            DestroyView();
                            break;
                        case PlayerSkin.Normal:
                            ReplaceView("player" + (int)gamePadIndex);
                            break;
                        case PlayerSkin.TrainTrack:
                            ReplaceView("traintrackPlayer" + (int)gamePadIndex);
                            break;
                        case PlayerSkin.Swimming:
                            ReplaceView("swimPlayer" + (int)gamePadIndex);
                            break;
                        case PlayerSkin.Tumbling:
                            ReplaceView("deadPlayer" + (int)gamePadIndex);
                            break;
                        case PlayerSkin.Spacesuit:
                            ReplaceView("spaceman" + (int)gamePadIndex);
                            CreateSteamEffect();
                            break;
                        case PlayerSkin.Drowning:
                            if (lane.LaneKind == LaneKind.SwimWater)
                            {
                                ReplaceView("drowningFloatPlayer" + (int)gamePadIndex);
                            }
                            else
                            {
                                ReplaceView("drowningPlayer" + (int)gamePadIndex);
                            }
                            stage.audio.PlaySound(Sfx.drowning);
                            PlayAll();
                            PlayheadWrap += new AnimationEvent(OnEndDieSequence);
                            body.SetLinearVelocity(Vector2.Zero);
                            this.Y = lane.yLocation + lane.laneHeight / 2;
                            break;
                    }
                }
            }
        }

        void OnEndDieSequence(DisplayObjectContainer sender)
        {
            sender.PlayheadWrap -= new AnimationEvent(OnEndDieSequence);
            sender.Stop();
            sender.DestroyAfterUpdate();
        }

		protected override void OnAddToStageComplete()
		{
			base.OnAddToStageComplete();
		}
        public void RoundComplete()
        {
            totalScore = previousTotalScore + roundBestScore;
            previousTotalScore = totalScore;
            roundFinalScore = roundBestScore;
            roundBestScore = 0;
        }
        //public override bool OnPlayerInput(int playerIndex, DDW.Input.Move move, TimeSpan time)
        //{
        //    return base.OnPlayerInput(playerIndex, move, time);
        //}
        protected Cue spacePsst;
        public override void UpdateLocalPlayer(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (screen.isActive)
            {
                if (LivingState == LivingState.Alive && body != null)
                {
                    GamePadState gps = GamePad.GetState(gamePadIndex);
                    Vector2 lv = body.GetLinearVelocity();
                    if (aboardVehicle != null && isOnWater)
                    {
                        lv = aboardVehicle.body.GetLinearVelocity();
                    }

                    atMaxSpeed = lv.LengthSquared() > maxSpeed * maxSpeed * Lane.playerSpeedDamping;

                    if (atMaxSpeed)
                    {
                        body.ApplyLinearImpulse(new Vector2(gps.ThumbSticks.Left.X * impulseScale, gps.ThumbSticks.Left.Y * -impulseScale), body.GetWorldCenter());
                        lv = body.GetLinearVelocity();

                        body.SetLinearVelocity(new Vector2(lv.X * dampingRatio, lv.Y * dampingRatio));
                        body.SetAngularVelocity(body.GetAngularVelocity() * dampingRatio);
                        isSettling = false;
                        isSettled = false;
                    }
                    else if (gps.ThumbSticks.Left.LengthSquared() > .01)
                    {
                        body.ApplyLinearImpulse(new Vector2(gps.ThumbSticks.Left.X * impulseScale, gps.ThumbSticks.Left.Y * -impulseScale), body.GetWorldCenter());
                        isSettling = false;
                        isSettled = false;
                    }
                    else if (autoCenterOnLane && Math.Abs(Y - snapLaneY) > 2)
                    {
                        float dir = ((snapLaneY - Y) / 4f + lv.Y) * dampingRatio;
                        body.SetLinearVelocity(new Vector2(lv.X * dampingRatio, dir));
                        body.SetAngularVelocity(body.GetAngularVelocity() * dampingRatio);
                        isSettling = true;
                        isSettled = false;
                    }
                    else
                    {
                        body.SetLinearVelocity(new Vector2(lv.X * dampingRatio, lv.Y * dampingRatio));
                        body.SetAngularVelocity(body.GetAngularVelocity() * dampingRatio);
                        isSettling = true;
                        isSettled = true;
                    }

                    if (lv.LengthSquared() == 0)
                    {
                        CurChildFrame = 0; // legs in when not moving
                    }
                    else if ((lastStepPos - body.GetPosition()).Length() > metersPerStep)
                    {
                        CurChildFrame = ++CurChildFrame % LastChildFrame;
                        lastStepPos = body.GetPosition();

                        if (lane.LaneKind == LaneKind.SwimWater)
                        {
                            if (CurChildFrame % 5 == 0)
                            {
                                stage.audio.PlaySound(Sfx.splash);
                            }
                        }
                        else if (lane.LaneKind == LaneKind.Spaceship)
                        {
                            // do nothing
                        }
                        else
                        {
                            if (CurChildFrame % 2 == 1)
                            {
                                stage.audio.PlaySound(Smuck.Audio.Sfx.step + "0");// + (int)gamePadIndex);
                            }
                        }

                    }

                    if (!isSettling)
                    {
                        if (LivingState == LivingState.Alive)
                        {
                            if (lv.LengthSquared() > maxSpeed) // less than sqrt speed - this stops jitter
                            {
                                this.Rotation = (float)(System.Math.Atan2(lv.Y, lv.X) + PI / 2);
                            }
                            else
                            {
                                this.Rotation = (float)(System.Math.Atan2(gps.ThumbSticks.Left.Y, -gps.ThumbSticks.Left.X) - PI / 2);
                            }

                            if (playerSkin == PlayerSkin.Spacesuit && lv.LengthSquared() > .2f && rnd.Next(2) == 0)
                            {
                                steamParticles.CreateInstanceAt("particle4", "pe", X, Y, 0, 0);
                                steamParticles.SetParticleScale(gps.ThumbSticks.Left.Length() / 2);
                                steamParticles.SetParticleDirection(new Vector2(-gps.ThumbSticks.Left.X, gps.ThumbSticks.Left.Y));
                            }
                        }
                        else
                        {
                            this.Rotation = body.GetAngle();
                        }
                    }
                    else if (isSettled)
                    {
                        // bring rotation to face goal direction
                        if (goalIsTop && Rotation != 0)
                        {
                            if (Rotation > PI)
                            {
                                Rotation -= PI * 2;
                            }

                            Rotation -= Rotation * .3f;
                            if (Math.Abs(Rotation) < .1)
                            {
                                Rotation = 0;
                            }
                        }
                        else if (!goalIsTop && Rotation != PI)
                        {
                            if (Rotation < 0)
                            {
                                Rotation += PI * 2;
                            }

                            Rotation += (PI - Rotation) * .3f;
                            if (Math.Abs(Rotation - PI) < .1)
                            {
                                Rotation = PI;
                            }
                        }
                    }

                    if (playerSkin == PlayerSkin.Spacesuit)
                    {
                        if (spacePsst == null && gps.ThumbSticks.Left.LengthSquared() > .01)
                        {
                            spacePsst = stage.audio.PlaySound(Sfx.psstLoop);
                        }
                        else if (spacePsst != null && gps.ThumbSticks.Left.LengthSquared() < .01)
                        {
                            spacePsst.Stop(AudioStopOptions.Immediate);
                            spacePsst = null;
                        }
                    }
                }
                else if (playerSkin == PlayerSkin.Drowning)
                {
                    // just play
                }
                else if ((lastStepPos - body.GetPosition()).Length() > metersPerStep)
                {
                    // cycle dead icons when dead
                    CurChildFrame = ++CurChildFrame % LastChildFrame;
                    lastStepPos = body.GetPosition();
                }
            }
        }

        public void CreateFlameEffect()
        {
            if (starParticles == null)// && LivingState == LivingState.Alive)
            {
                starParticles = (StarParticleGroup)parent.CreateInstance(null, "starParticles" + Index, 0, 0, 0);
            }
        }
        public void CreateSteamEffect()
        {
            if (steamParticles == null)// && LivingState == LivingState.Alive)
            {
                steamParticles = (SteamParticleGroup)parent.CreateInstance(null, "steamParticles", 0, 0, 0);
                //steamParticles = (SteamParticleGroup)parent.CreateInstance("particle4", "steamParticles", 0, 0, 0);
            }
        }
        public override void DestroyElement(DisplayObject obj)
        {
            base.DestroyElement(obj);
            if (obj is StarParticleGroup)
            {
                this.starParticles = null;
            }
        }

        private StarParticleGroup starParticles;
        private SteamParticleGroup steamParticles;

        int prevMs = 0;
        public static Random rnd = new Random((int)DateTime.Now.Ticks);
        
		public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
		{
            base.Update(gameTime);

            if (screen.isActive && steamParticles != null)
            {
                if (steamParticles.isComplete)
                {
                    steamParticles = null;
                }
            }
            else if (screen.isActive && starParticles != null)
            {
                if (starParticles.isComplete)
                {
                    starParticles = null;
                } 
                else
                {
                    int msDelay = 15;
                    prevMs += gameTime.ElapsedGameTime.Milliseconds;
                    if (prevMs > msDelay)
                    {
                        prevMs -= msDelay;
                        int pi = (int)Math.Min(roundScore / (laneCount - 1f), 7);
                        //starParticles.CreateInstanceAt("plpart" + pi, "pe", X + rnd.Next(40) - 20, Y + rnd.Next(40) - 20, 0, 0);
                        starParticles.CreateInstanceAt("plpart7", "pe", X + rnd.Next(40) - 20, Y + rnd.Next(40) - 20, 0, 0);
                        starParticles.SetParticleScale(pi / ((laneCount - 1f) * 3f) + .33f);
                    }
                }
            }
            else
            {
                this.SpeedLevel = SpeedLevel.Normal;
            }
		}
		public override void UpdateRemotePlayer(int framesBetweenPackets, bool enablePrediction)
		{
			// todo: lerp from network data
			//SetB2DPosition(rawPostion.X, rawPostion.Y);
			this.X = rawPostion.X;
			this.Y = rawPostion.Y;

			if (rawVelocity.LengthSquared() > .1)
			{
				this.State.Rotation = (float)(System.Math.Atan2(rawVelocity.Y, rawVelocity.X) + PI / 2);
			}

			if ((lastStepPos - rawPostion).Length() > metersPerStep)
			{
				CurChildFrame = ++CurChildFrame % LastChildFrame;
				lastStepPos = rawPostion;
			}
		}
		public override void Draw(SpriteBatch batch)
		{
			base.Draw(batch);
		}

		public override float Rotation
		{
			get
			{
				return base.Rotation;
			}
			set
			{
				base.Rotation = value;
			}
		}

        public override void Removed(EventArgs e)
        {
            base.Removed(e);
            if (starParticles != null && children.Contains(starParticles))
            {
                RemoveChild(starParticles);
            }
            starParticles = null;

            if (spacePsst != null)
            {
                spacePsst.Stop(AudioStopOptions.Immediate);
                spacePsst = null;
            }
        }
    }
    public enum PlayerSkin
    {
        None,
        Normal,
        Tumbling,
        TrainTrack,
        Spacesuit,
        Swimming,
        Drowning,
        Naked,
    }

}
