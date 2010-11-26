using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Smuck.Enums;
using DDW.V2D;
using Microsoft.Xna.Framework;
using Smuck.Screens;

namespace Smuck.Components
{
    public class Lane
    {
        public static Random rnd = new Random((int)DateTime.Now.Ticks);

        public int laneIndex;
        public float yLocation;
        public float laneHeight;
        public int laneScore = 1;
        public float playerSpeedDamping = 1;
        /// <summary>
        /// Between about 20 and 50
        /// </summary>
        public float vehicleSpeed;
        public bool movesRight;
        public bool oneTimeVehicle = false;
        public bool oneVehiclePerLane = false;
        public bool createVehicleOnStartup = true;
        public bool allowVehicles = true;
        private int vehicleStyleIndex;
        private bool firstVehicle = true;
        private int laneWidth = 1024;
        public List<LaneVehicle> vehicles = new List<LaneVehicle>();

        public int minCreationDelay;
        public int maxCreationDelay;

        private TimeSpan nextCreation = TimeSpan.Zero;
        private TimeSpan lastUpdate = TimeSpan.MaxValue;

        private static TimeSpan pauseDetection = new TimeSpan(0, 0, 2);
        private static int nextDepth = 0;
        private const float twoPi = (float)(System.Math.PI * 2);
        private const float leftAngle = twoPi;
        private const float rightAngle = twoPi + (float)System.Math.PI;

        public Lane(int laneIndex, int laneHeight, LaneKind laneKind)
        {
            this.laneIndex = laneIndex;
            this.laneHeight = laneHeight;
            this.LaneKind = laneKind;
        }

        private LaneKind laneKind;
        public LaneKind LaneKind
        {
            get
            {
                return laneKind;
            }
            set
            {
                laneKind = value;
                if (laneKind == LaneKind.Train)
                {
                    playerSpeedDamping = .1f;
                }
            }
        }

        public void SetNextVehicleTime(TimeSpan totalGameTime)
        {
            nextCreation = totalGameTime + new TimeSpan(0, 0, 0, 0, rnd.Next(minCreationDelay, maxCreationDelay)); 
        }

        public bool CanCreateVehicle()
        {
            bool result = true;
            if (!allowVehicles || (oneTimeVehicle && vehicles.Count > 0))
            {
                result = false;
            }
            else if (vehicles.Count >= 1 && oneVehiclePerLane)
            {
                result = false;
            }
            else if (vehicles.Count > 0)
            {
                int xOrigin;
                if (movesRight)
                {
                    xOrigin = 30; // include some buffer
                }
                else
                {
                    xOrigin = laneWidth - 30;
                }

                for (int i = 0; i < vehicles.Count; i++)
                {
                    if (movesRight && vehicles[i].X > xOrigin && vehicles[i].X - vehicles[i].VisibleWidth < xOrigin)
                    {
                        result = false;
                        nextCreation += new TimeSpan(0, 0, 0, 0, rnd.Next(500, minCreationDelay + 500));
                        break;
                    }
                    else if (!movesRight && vehicles[i].X < xOrigin && vehicles[i].X + vehicles[i].VisibleWidth > xOrigin)
                    {
                        result = false;
                        nextCreation += new TimeSpan(0, 0, 0, 0, rnd.Next(500, minCreationDelay + 500));
                        break;
                    }
                }
            }
            return result;
        }

        public bool CreateVehicle(V2DScreen screen, TimeSpan totalGameTime)
        {
            bool result = false;
            if (laneKind == LaneKind.Empty) return result;

            if (!((BaseScreen)screen).WaitingOnPanel)
            {
                if (firstVehicle)
                {
                    firstVehicle = false;
                    laneWidth = (int)screen.ClientSize.X;
                    if (!createVehicleOnStartup || rnd.Next(maxCreationDelay) < minCreationDelay)
                    {
                        SetNextVehicleTime(totalGameTime);
                        return result;
                    }
                }

                if (CanCreateVehicle())
                {
                    // todo: move this into vehicles
                    V2DInstance vInst;
                    switch (laneKind)
                    {
                        case LaneKind.Sidewalk:
                            vehicleStyleIndex = rnd.Next(4);
                            vInst = screen.CreateInstanceDefinition("sidewalker" + vehicleStyleIndex, "vehicle" + nextDepth);
                            vInst.Rotation = movesRight ? rightAngle : leftAngle;
                            break;

                        case LaneKind.Car:
                            if (rnd.Next(20) == 0)
                            {
                                // special vehicles
                                vehicleStyleIndex = rnd.Next(2) + 11;
                            }
                            else
                            {
                                vehicleStyleIndex = rnd.Next(11);
                            }
                            vInst = screen.CreateInstanceDefinition("vehicle" + vehicleStyleIndex, "vehicle" + nextDepth);
                            vInst.Rotation = movesRight ? rightAngle : leftAngle;
                            break;

                        case LaneKind.Train:
                            vehicleStyleIndex = 0;
                            vInst = screen.CreateInstanceDefinition("trainFull" + vehicleStyleIndex, "vehicle" + nextDepth);
                            vInst.Rotation = movesRight ? rightAngle : leftAngle;
                            break;

                        case LaneKind.Shinkansen:
                            vehicleStyleIndex = 1;
                            vInst = screen.CreateInstanceDefinition("trainFull" + vehicleStyleIndex, "vehicle" + nextDepth);
                            vInst.Rotation = movesRight ? rightAngle : leftAngle;
                            break;

                        case LaneKind.Spaceship:
                            vehicleStyleIndex = rnd.Next(8);
                            vInst = screen.CreateInstanceDefinition("spaceship" + vehicleStyleIndex, "vehicle" + nextDepth);
                            vInst.Rotation = movesRight ? rightAngle + (float)rnd.Next(1000) / 2000f - .25f : leftAngle + (float)rnd.Next(1000) / 2000f - .25f;
                            break;

                        case LaneKind.DrownWater:
                            vehicleStyleIndex = rnd.Next(3);
                            vInst = screen.CreateInstanceDefinition("boat" + vehicleStyleIndex, "boat" + nextDepth);
                            vInst.Rotation = movesRight ? rightAngle : leftAngle;
                            break;

                        case LaneKind.SwimWater:
                            vehicleStyleIndex = FindBoatIndex(screen);
                            if (vehicleStyleIndex == -1)
                            {
                                return result;
                            }

                            if (vehicleStyleIndex == 5)
                            {
                                vInst = screen.CreateInstanceDefinition("boat" + vehicleStyleIndex, "carrier" + nextDepth);
                            }
                            else
                            {
                                vInst = screen.CreateInstanceDefinition("boat" + vehicleStyleIndex, "boat" + nextDepth);
                            }
                            vInst.Rotation = movesRight ? rightAngle : leftAngle;
                            break;

                        case LaneKind.WideCar:
                            vehicleStyleIndex = 0;
                            vInst = screen.CreateInstanceDefinition("wideVehicle" + vehicleStyleIndex, "vehicle" + nextDepth);
                            vInst.Rotation = movesRight ? rightAngle : leftAngle;
                            break;

                        case LaneKind.SteamRoller:
                            vInst = screen.CreateInstanceDefinition("steamRoller", "steamRoller" + nextDepth);
                            vInst.Rotation = movesRight ? rightAngle : leftAngle;
                            break;

                        default:
                            throw new Exception("Unsupported vehicle: " + laneKind);
                    }

                    vInst.Depth = nextDepth++;

                    vInst.Y = yLocation;
                    Vector4 shapeRect = vInst.Definition.GetShapeRectangle();
                    float h = shapeRect.W;
                    if (movesRight)
                    {
                        vInst.X = 10;
                        vInst.Y += laneHeight - (laneHeight - h - shapeRect.Y) / 2f;// vInst.Definition.Height) / 2f;
                    }
                    else
                    {
                        vInst.X = laneWidth - 10;
                        vInst.Y += (laneHeight - h - shapeRect.Y) / 2f;//vInst.Definition.Height) / 2f;
                    }

                    if (firstVehicle && !oneTimeVehicle)
                    {
                        vInst.X = rnd.Next(0, laneWidth);
                        firstVehicle = false;
                    }

                    LaneVehicle v = (LaneVehicle)screen.AddInstance(vInst, screen);
                    v.vehicleStyleIndex = vehicleStyleIndex;

                    // trains shouldn't move off track
                    if (v.VisibleWidth > 500)
                    {
                        v.body.SetFixedRotation(true);
                    }

                    v.Lane = this;
                    v.laneY = v.Y;
                    v.direction = new Vector2((float)System.Math.Cos(vInst.Rotation), (float)System.Math.Sin(vInst.Rotation));
                    v.MaxSpeed = vehicleSpeed;

                    vehicles.Add(v);

                    if (laneKind == LaneKind.WideCar || laneKind == LaneKind.DrownWater || laneKind == LaneKind.SwimWater || laneKind == LaneKind.Sidewalk)
                    {
                        v.PlayAll();
                        if ((laneKind == LaneKind.DrownWater) && vehicleStyleIndex == 1)
                        {
                            v.MaxSpeed = 5;
                        }
                    }
                    else if (laneKind == LaneKind.Car && vehicleStyleIndex >= 11)
                    {
                        v.PlayAll();
                    }

                    SetNextVehicleTime(totalGameTime);
                    result = true;
                }
            }
            return result;
        }

        protected int FindBoatIndex(V2DScreen screen)
        {
            int result = -1;
            if (laneIndex == 1 || laneIndex == 8)
            {
                result = rnd.Next(2) + 1;
            }
            else if (laneIndex == 2 || laneIndex == 7)
            {
                result = 5;
            }
            else if (laneIndex == 3 || laneIndex == 6)
            {
                result = 4;
            }
            else if (laneIndex == 4 || laneIndex == 5)
            {
                result = 0;
            }
            else
            {
                result = rnd.Next(3);
            }

            // no other ships when aircraft carrier is present
            if (laneIndex == 1 || laneIndex == 3)
            {
                if (((BaseScreen)screen).lanes[2].vehicles.Count > 0 && ((BaseScreen)screen).lanes[2].vehicles[0].X > 100)
                {
                    return -1;
                }
            }
            else if (laneIndex == 6 || laneIndex == 8)
            {
                if (((BaseScreen)screen).lanes[7].vehicles.Count > 0 && ((BaseScreen)screen).lanes[7].vehicles[0].X < 1400)
                {
                    return -1;
                }
            }

            return result;
        }

        public void Update(GameTime gameTime, V2DScreen screen)
        {
            if ((gameTime.TotalGameTime - lastUpdate) > pauseDetection)
            {
                nextCreation += (gameTime.TotalGameTime - lastUpdate);
            }

            // remove expired vehicles
            for (int i = 0; i < vehicles.Count; i++)
            {
                if( (movesRight && vehicles[i].X - vehicles[i].VehicleWidth > screen.ClientSize.X)  || 
                    (!movesRight && vehicles[i].X + vehicles[i].VehicleWidth < 0))
                {
                    screen.RemoveInstance(vehicles[i]);
                    vehicles.Remove(vehicles[i]);
                }
            }

            if (gameTime.TotalGameTime > nextCreation)
            {
                CreateVehicle(screen, gameTime.TotalGameTime);
            }
            lastUpdate = gameTime.TotalGameTime;
        }

    }
}
