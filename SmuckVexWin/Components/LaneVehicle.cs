using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DDW.V2D;
using Microsoft.Xna.Framework.Graphics;
using Box2D.XNA;
using Microsoft.Xna.Framework;
using V2DRuntime.Attributes;
using Smuck.Enums;
using SharpNeatLib.Maths;
using Microsoft.Xna.Framework.Audio;
using Smuck.Audio;

namespace Smuck.Components
{
	public class LaneVehicle : Vehicle
	{
        protected Lane lane;
		public float laneY;

        private static AudioListener audioListener;
        private AudioEmitter audioEmitter;
        private Cue cue;

		public LaneVehicle(Texture2D texture, V2DInstance instance) : base(texture, instance)
		{   
		}
        public Lane Lane
        {
            get { return lane; }
            set
            {
                this.lane = value;
                SetSound();
            }
        }
        public virtual void SetSound()
        {
            if (lane.LaneKind == LaneKind.Car)
            {
                if (vehicleStyleIndex == 1 && this.CurChildFrame == 0)
                {
                    cue = stage.audio.PlaySound(Sfx.genLee, audioListener, audioEmitter);
                }
                else if (vehicleStyleIndex == 11)
                {
                    cue = stage.audio.PlaySound(Sfx.siren, audioListener, audioEmitter);
                }
                else if (vehicleStyleIndex == 12)
                {
                    cue = stage.audio.PlaySound(Sfx.batman, audioListener, audioEmitter);
                }
                else
                {
                    cue = stage.audio.PlaySound(Sfx.carRumble, audioListener, audioEmitter);
                }
            }
        }
        public virtual void PauseSound()
        {
            if (cue != null && cue.IsPlaying)
            {
                cue.Pause();
            }
        }
        public virtual void ResumeSound()
        {
            if (cue != null && cue.IsPaused)
            {
                cue.Resume();
            }
        }
        public virtual void StopSound()
        {
            if (cue != null && cue.IsPlaying)
            {
                cue.Stop(AudioStopOptions.Immediate);
                cue = null;
                audioEmitter = null;
            }
        }

        public override void AddedToStage(EventArgs e)
        {
            base.AddedToStage(e);

            if (audioListener == null)
            {
                audioListener = new AudioListener();
                audioListener.Position = new Vector3(600, 0, 0);
                audioListener.Velocity = new Vector3(0, 0, 0);
                audioListener.Forward = new Vector3(-1, 0, 0);
            }
            audioEmitter = new AudioEmitter();
            audioEmitter.Forward = new Vector3(1, 0, 0);
        }
        public override void RemovedFromStage(EventArgs e)
        {
            base.RemovedFromStage(e);
            if (cue != null)
            {
                cue.Stop(AudioStopOptions.Immediate);
                cue = null;
                audioEmitter = null;
                //audioListener = null;
            }
        }
		protected override void ApplyTorque()
		{
            if (!body.IsFixedRotation())
            {
                // todo: optimize this, no locals
                float angle = body.GetAngle() - (float)System.Math.PI;
                Vector2 pointing = new Vector2((float)System.Math.Cos(angle), (float)System.Math.Sin(angle));
                int dir = lane.movesRight ? 1 : -1;
                Vector2 position = body.GetPosition() * 15F;
                Vector2 goal = new Vector2(position.X + dir * 100, laneY + .5f);
                Vector2 difference = position - goal;
                float turnAmount = Vector2.Dot(new Vector2(-pointing.Y, pointing.X), difference);
                body.ApplyTorque((float)System.Math.Sign(turnAmount) * -1000F * (float)System.Math.Sqrt(System.Math.Abs(turnAmount)));
            }
		}
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (cue != null)
            {
                audioEmitter.Position = new Vector3(Position.X, 0f, 0f);
                cue.Apply3D(audioListener, audioEmitter);
            }
        }
	}
}
