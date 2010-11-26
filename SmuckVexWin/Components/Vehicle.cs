using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DDW.V2D;
using Microsoft.Xna.Framework.Graphics;
using Box2D.XNA;
using Smuck.Enums;
using Microsoft.Xna.Framework;
using DDW.Display;

namespace Smuck.Components
{
    public class Vehicle : V2DSprite
	{
		public Vector2 direction = new Vector2(1, 0);
		public float VehicleHeight;
		public float VehicleWidth;
        public int vehicleStyleIndex;

        protected bool isFirstUpdate = false;
		protected float maxSpeed = 35f; // about 10 ~ 50
		protected float impulseSpeed = 100f;

		private static Random rnd = new Random((int)DateTime.Now.Ticks);

		public Vehicle(Texture2D texture, V2DInstance instance) : base(texture, instance)
		{
			this.restitution = .30f;
			this.friction = 0f;
			this.angularDamping = 3;
			this.linearDamping = .1f;
		}

        public float MaxSpeed
		{
			get { return maxSpeed; }
			set
			{
				maxSpeed = value;
                impulseSpeed = maxSpeed * 4;
			}
		}
		protected virtual void ApplyImpulse()
        {
            float a = body.GetAngle() - (float)System.Math.PI;
            direction = new Vector2((float)System.Math.Cos(a), (float)System.Math.Sin(a));

            // keep from wiggling
            Vector2 lv = body.GetLinearVelocity();
            Vector2 perp = new Vector2(-direction.Y, direction.X);
            Vector2 yComp = Vector2.Dot(direction, lv) * direction;
            Vector2 xComp = Vector2.Dot(perp, lv) * perp;
            body.SetLinearVelocity(lv - xComp * 0.2F - yComp * 0F);

            // set speed
            Vector2 normDir = direction;
            normDir.Normalize();
            float dir = Vector2.Dot(normDir, lv);
            if(dir < maxSpeed)
            {
                body.ApplyLinearImpulse(direction * impulseSpeed, body.GetWorldCenter());
            }
            else
            {
                body.ApplyLinearImpulse(-direction * impulseSpeed, body.GetWorldCenter());
            }
		}
		protected virtual void ApplyTorque()
		{
		}

		public override void AddedToStage(EventArgs e)
		{
			base.AddedToStage(e);
			VehicleWidth = VisibleWidth;
            VehicleHeight = VisibleHeight;
			SetRandomColor();
		}

		public void SetRandomColor()
		{
			uint frame = (uint)rnd.Next((int)LastChildFrame);
			GotoAndStop(frame);
		}

		public void HitWall(Body wall)
		{
			int frame = (int)wall.GetUserData();
			GotoAndStop((uint)frame);
		}
		public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
		{
			base.Update(gameTime);
            if (isFirstUpdate)
            {
                float a = body.GetAngle() - (float)System.Math.PI;
                //Vec2 v2 = new Vec2((float)(System.Math.Cos(a) * 800), (float)(System.Math.Sin(a) * 800));
                float massThrust = body.GetMass() * maxSpeed;
                Vector2 v2 = new Vector2((float)(System.Math.Cos(a) * massThrust), (float)(System.Math.Sin(a) * massThrust));
                body.ApplyLinearImpulse(v2, body.GetWorldCenter());
                isFirstUpdate = false; 
            }
            else
            {
                ApplyImpulse();
                ApplyTorque();
            }
		}
    }
}
