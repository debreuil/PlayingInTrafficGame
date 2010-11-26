using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using DDW.V2D;
using Microsoft.Xna.Framework;

namespace Smuck.Components
{
    public class SpaceVehicle : LaneVehicle
    {
        public SpaceVehicle(Texture2D texture, V2DInstance instance) : base(texture, instance)
		{
		}

        protected override void ApplyImpulse()
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
            if (dir < maxSpeed)
            {
                body.ApplyLinearImpulse(direction * impulseSpeed, body.GetWorldCenter());
            }
            else
            {
                body.ApplyLinearImpulse(-direction * impulseSpeed, body.GetWorldCenter());
            }
        }
        protected override void ApplyTorque()
        {
        }
    }
}
