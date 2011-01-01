using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Smuck.Enums
{
    public enum Level
    {
        Default = 0,
        TwoLane = 1, // must be 1
        WideBoulevard = 2,
        TwoBoulevard = 3,
        Crosswalk = 4,
        Houses = 5,
        TwoTrainTwoBoulevard = 6,
        TwoTrain = 7,
        AllCars = 8,
        TwoCanalTwoBoulevard = 9,
        TwoCanal = 10,
        TwoCanalTwoTrain = 11,
        AllTrain = 12,
        LaneChange = 13, // must be 13
        AllWater = 14,
        SpaceMedium = 15,
        SteamRoller = 16,

        Zombie = 17,
        SpaceBoulevardSlow = 18,
        ParkingLot = 19,
        SpaceFast = 20,
    }
}
