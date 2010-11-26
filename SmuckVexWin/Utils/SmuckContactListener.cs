using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Box2D.XNA;
using Smuck.Components;

namespace Smuck.Utils
{
    public delegate void VehicleCollisionHandler(Vehicle vehicle, Body body);
    public delegate void DeadIconHandler(DeadIcon deadIcon, SmuckPlayer player);

	public class SmuckContactListener// : IContactListener
	{
		//public event VehicleCollisionHandler VehicleCollision;
		//public event DeadIconHandler DeadIconCollision;

		//public override void Add(ContactPoint point)
		//{
		//    base.Add(point);
		//    Body b1 = point.Shape1.GetBody();
		//    Body b2 = point.Shape2.GetBody();
		//    object obj1 = b1.GetUserData();
		//    object obj2 = b2.GetUserData();

		//    if (obj1 is Vehicle && !(obj2 is Vehicle))
		//    {
		//        VehicleCollision((Vehicle)obj1, b2);
		//    }
		//    else if(obj2 is Vehicle && !(obj1 is Vehicle))
		//    {
		//        VehicleCollision((Vehicle)obj2, b1);
		//    }
		//    else if (obj1 is DeadIcon && obj2 is SmuckPlayer)
		//    {
		//        DeadIconCollision((DeadIcon)obj1, (SmuckPlayer)obj2);
		//    }
		//    else if (obj2 is DeadIcon && obj1 is SmuckPlayer)
		//    {
		//        DeadIconCollision((DeadIcon)obj2, (SmuckPlayer)obj1);
		//    }

		//}
	}
}
