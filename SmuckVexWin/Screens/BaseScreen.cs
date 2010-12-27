using System;
using System.Collections.Generic;
using Box2D.XNA;
using DDW.Display;
using DDW.Input;
using DDW.V2D;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Net;
using Smuck.Components;
using Smuck.Enums;
using Smuck.Panels;
using Smuck.Particles;
using Smuck.Shaders;
using Smuck.Utils;
using V2DRuntime.Attributes;
using V2DRuntime.Components;
using V2DRuntime.Enums;
using V2DRuntime.Network;
using V2DRuntime.Shaders;
using V2DRuntime.Display;
using Microsoft.Xna.Framework.Audio;
using Smuck.Audio;
using Microsoft.Xna.Framework.GamerServices;

namespace Smuck.Screens
{
    public abstract class BaseScreen : V2DScreen
    {
        public Level LevelKind = Level.Default;

        public static Random rnd = new Random((int)DateTime.Now.Ticks);

        //[SpriteAttribute(depthGroup = 1000)]
        public GameOverlay gameOverlay;

        [V2DSpriteAttribute(categoryBits = Category.BORDER, maskBits = Category.DEFAULT | Category.ICON | Category.PLAYER, isStatic = true, allowSleep = false)]
        protected V2DSprite[] border;

        [V2DSpriteAttribute(depthGroup = 30, categoryBits = Category.VEHICLE, maskBits = Category.DEFAULT | Category.VEHICLE | Category.PLAYER | Category.WATER | Category.GUIDERAIL, angularDamping = 9F)] // todo: mave ang damp to LV attribute
        //[V2DShaderAttribute(shaderType = typeof(DesaturationShader), param0 = .2f)]
        public LaneVehicle vehicle;

        [V2DSpriteAttribute(depthGroup = 35, categoryBits = Category.VEHICLE, maskBits = Category.VEHICLE | Category.PLAYER, angularDamping = 1000F)]
        protected TrainVehicle train;

        [V2DSpriteAttribute(depthGroup = 13, categoryBits = Category.VEHICLE, maskBits = Category.DEFAULT | Category.PLAYER | Category.GUIDERAIL, angularDamping = 100F, isSensor = true)]
        public LaneVehicle boat;

        [V2DSpriteAttribute(depthGroup = 15, categoryBits = Category.ICON, maskBits = Category.DEFAULT, isStatic = true)]
        public List<DeadIcon> deadIcons = new List<DeadIcon>();

        [V2DSpriteAttribute(depthGroup = 20, isBullet = true, linearDamping = 2, categoryBits = Category.PLAYER, maskBits = Category.DEFAULT | Category.BORDER | Category.VEHICLE | Category.PLAYER | Category.WATER | Category.GUIDERAIL)]
        public static List<SmuckPlayer> players = new List<SmuckPlayer>();
        
        [SpriteAttribute(depthGroup = 0)]
        protected V2DSprite road;

        [V2DSpriteAttribute(depthGroup = 15)]
        public StarParticleGroup starParticles;

        public Lane[] lanes;
        public bool collideWithBoats = false;

        protected int laneCount;

        protected bool endOnLastLane = false;
        protected bool isLevelOver = false;
        protected bool summaryPosted = false;
        protected SmuckContactListener contactListener;

        private int iconDepthCount = 0;
        private int playerDepthCount = 0;

        private float[,] startLocations = { { 525, 0, (float)(Math.PI) }, { 675, 0, (float)(Math.PI) }, { 525, 9, 0 }, { 675, 9, 0 } };

        public BaseScreen(V2DContent v2dContent) : base(v2dContent)
        {
        }
        public BaseScreen(SymbolImport si) : base(si)
        {
            SymbolImport = si;
        }

        private int pointsToWinRound;
        public void SetGoals(int pointsPerCrossing, int totalCrossings)
        {
            pointsToWinRound = pointsPerCrossing * totalCrossings;
            gameOverlay.SetGoals(pointsPerCrossing, totalCrossings);
        }
        public override void SetBounds(float x, float y, float w, float h)
        {
        }
        public override void Initialize()
        {
            base.Initialize();
            gameOverlay = SmuckGame.GameOverlay;
            gameOverlay.Visible = true;
            isLevelOver = false;
            summaryPosted = false;
            SetGoals(9, 4);

            LaneInit();
            LevelInit();
            gameOverlay.SetLevel(LevelKind);
        }
        public override void Activate()
        {
            base.Activate(); 
        }
        public override void Deactivate()
        {
            base.Deactivate();
        }
        protected virtual void LaneInit()
        {
            // default impl, don't call base if you don't want 8 lanes of traffic like this
            laneCount = 10;
            if (lanes == null)
            {
                lanes = new Lane[laneCount];
            }

            int[] speeds = new int[] { 5, 15, 20, 30, 40, 40, 30, 20, 15, 5 };

            lanes[0] = new Lane(0, 67, LaneKind.Sidewalk);
            lanes[0].vehicleSpeed = speeds[0];
            lanes[0].minCreationDelay = 15000;
            lanes[0].maxCreationDelay = 25000;
            lanes[0].movesRight = false;
            lanes[0].yLocation = 27;

            for (int i = 1; i < lanes.Length; i++)
            {
                lanes[i] = new Lane(i, 67, LaneKind.Car);
                lanes[i].vehicleSpeed = speeds[i] + rnd.Next(10);
                lanes[i].minCreationDelay = (int)(60000 / lanes[i].vehicleSpeed);
                lanes[i].maxCreationDelay = 6000;
                lanes[i].movesRight = i >= lanes.Length / 2;
                lanes[i].yLocation = 88 + lanes[i].laneHeight * (i - 1);
            }
            lanes[lanes.Length - 1].LaneKind = LaneKind.Sidewalk;
            lanes[lanes.Length - 1].vehicleSpeed = speeds[lanes.Length - 1];
            lanes[lanes.Length - 1].minCreationDelay = 15000;
            lanes[lanes.Length - 1].maxCreationDelay = 25000;
        }

        protected abstract void LevelInit();

        public override void AddedToStage(EventArgs e)
        {
            base.AddedToStage(e);

            foreach (NetworkGamer g in NetworkManager.Session.AllGamers)//SmuckGame.instance.gamers)
            {
                int gamerIndex = NetworkManager.Instance.GetGamerIndex(g);
                CreatePlayer(gamerIndex);
            }
//            AudioManager.PlaySound(AudioManager.backgroundMusic);
//            AudioManager.PlaySound(AudioManager.trafficRumble);

        }
        public override void Removed(EventArgs e)
        {
            base.RemovedFromStage(e);

//            AudioManager.StopSound(AudioManager.backgroundMusic);
//            AudioManager.StopSound(AudioManager.trafficRumble);

            foreach (SmuckPlayer p in players)
            {
                this.RemoveChild(p);
                p.body = null;
            }
            for (int i = 0; i < lanes.Length; i++)
            {
                for (int j = 0; i < lanes[i].vehicles.Count; j++)
                {
                    if (lanes[i].vehicles[j] != null && this.Contains(lanes[i].vehicles[j]))
                    {
                        RemoveChild(lanes[i].vehicles[j]);
                    }
                    lanes[i].vehicles[j] = null;
                    lanes[i].vehicles.Clear();
                }
                lanes[i] = null;
            }
            lanes = null;
            vehicle = null;

            for (int i = 0; i < deadIcons.Count; i++)
            {
                RemoveChild(deadIcons[i]);
            }
            deadIcons.Clear();

            if (starParticles != null)
            {
                RemoveChild(starParticles);
            }
            //foreach (StarParticleGroup spg in starParticles)
            //{
            //    RemoveChild(spg);
            //}
        }


        public override bool OnPlayerInput(int playerIndex, Move move, TimeSpan time)
        {
            bool result = base.OnPlayerInput(playerIndex, move, time);

            if (result)
            {
                SmuckPlayer pl = players.Find(p => p.gamePadIndex == (PlayerIndex)playerIndex);
                if (move == Move.Start)
                {
                    PauseAllVehicleSounds();
                    gameOverlay.PauseGame();
                }
                else if (move == Move.ButtonA && !CheckRoundOver())
                {
                    if (pl == null || (pl != null && pl.LivingState == LivingState.Dead) )
                    {
                        InputManager im = inputManagers[playerIndex];
                        CreatePlayer(playerIndex);
                    }
                }
                else if (move == Move.ButtonY && !isLevelOver && pl.LivingState == LivingState.Alive)
                {
                    if (rnd.Next(10) == 0)
                    {
                        Cue c = stage.audio.PlaySound(Sfx.screamTaz, onTazFinished);
                        pl.isExploding = true;
                        KillPlayer(pl);
                    }
                    else
                    {
                        stage.audio.PlaySound(Sfx.scream);
                    }
                }
                //else if (move == Move.ButtonA)
                //{
                //    if (pl != null)
                //    {
                //        pl.Jump();
                //    }
                //}
                //else if (move == Move.ButtonY)
                //{
                //   pl.CreateFlameEffect();
                //}
            }
            return result;
        }

        void onTazFinished(Cue c)
        {
            stage.audio.PlaySound(Sfx.explode);
        }
        public override void BroadcastMove(int playerIndex, Move move, TimeSpan time)
        {
        }

        public override void DestroyElement(DisplayObject obj)
        {
            base.DestroyElement(obj);
            if (obj is SmuckPlayer)
            {
                SmuckPlayer p = (SmuckPlayer)obj;
                p.LivingState = LivingState.Dead;
                if (p.roundScore >= pointsToWinRound && p.skipDeathMarker)
                {
                    isLevelOver = true;
                }
                else if(!p.skipDeathMarker)
                {
                    CreateDeadIcon(p, DeadIconType.Death);
                }
            }
        }
        protected bool CheckRoundOver()
        {
            bool result = false;
            foreach (SmuckPlayer p in players)
            {
                if (p.LivingState != LivingState.Alive && p.roundScore >= pointsToWinRound)
                {
                    isLevelOver = true;
                    result = true;
                    break;
                }
            }
            return result;
        }
        private void CreateDeadIcon(SmuckPlayer p, DeadIconType type)
        {
            int dIndex = deadIcons.Count;
            DeadIcon di;
            if (this is SteamRollerScreen)
            {
                di = (DeadIcon)CreateInstanceAt("flatteningPlayer", "deadIcons" + dIndex, p.X, p.Y, 0, iconDepthCount++);
            }
            else if (p.isExploding)
            {
                di = (DeadIcon)CreateInstanceAt("explodingPlayer", "deadIcons" + dIndex, p.X, p.Y, 0, iconDepthCount++);
            }
            else
            {
                di = (DeadIcon)CreateInstanceAt("deadIcon", "deadIcons" + dIndex, p.X, p.Y, 0, iconDepthCount++);
            }

            // Compact framework doesn't support Enum.GetValues
            di.IconType = (DeadIconType)1;// (DeadIconType)rnd.Next(deadIconCount);
            di.Depth = iconDepthCount;
            di.PlayheadWrap += new AnimationEvent(di_PlayheadWrap);
            di.Play();
        }

        void di_PlayheadWrap(DisplayObjectContainer sender)
        {
            sender.PlayheadWrap -= new AnimationEvent(di_PlayheadWrap);
            sender.DestroyAfterUpdate();
            CheckRoundOver();
        }

        protected virtual SmuckPlayer CreatePlayer(int gamerIndex)
        {
            NetworkGamer g = null;
            GamerCollection<LocalNetworkGamer> lngc = NetworkManager.Session.LocalGamers;
            for (int i = 0; i < lngc.Count; i++)
            {
                if (lngc[i].SignedInGamer != null && (int)lngc[i].SignedInGamer.PlayerIndex == gamerIndex)
                {
                    g = lngc[i];
                    break;
                }
            }

            SmuckPlayer result = players.Find(p => (int)p.gamePadIndex == gamerIndex);

            if (result != null)
            {
                //result.manualPlayerDestroy = true;
                //result.ReplaceView(playerDefinitionName + gamerIndex);
                result.Stop();
                if (result.body != null)
                {
                    result.body.SetLinearVelocity(Vector2.Zero);
                }

                if (!this.Contains(result))
                {
                    AddChild(result);
                }

                stage.audio.PlaySound(Sfx.respawn);
                //result.manualPlayerDestroy = false;
            }
            else
            {
                //PlayerIndex gamePadIndex = (PlayerIndex)(players.FindAll(p => p.isLocal).Count);
                result = (SmuckPlayer)CreateInstanceAt("player" + gamerIndex, "players" + gamerIndex,
                    startLocations[gamerIndex, 0],
                    startLocations[gamerIndex, 1],
                    startLocations[gamerIndex, 2],
                    playerDepthCount++);

                result.isLocal = g.IsLocal;
                result.NetworkGamer = g;
                result.NetworkId = g.Id;
                result.Depth = playerDepthCount;

                if (g is LocalNetworkGamer)
                {
                    result.gamePadIndex = ((LocalNetworkGamer)g).SignedInGamer.PlayerIndex;

                    // sometimes that isn't right, or even connected it seems, so find a connected controller
                    if (!GamePad.GetState(result.gamePadIndex).IsConnected)
                    {
                        for (PlayerIndex pi = PlayerIndex.One; pi < PlayerIndex.Four; pi++)
                        {
                            if (GamePad.GetState(pi).IsConnected)
                            {
                                result.gamePadIndex = pi;
                                break;
                            }
                        }
                        GamePadCapabilities gpt = GamePad.GetCapabilities(result.gamePadIndex);
                    }
                }


                g.Tag = result;

                // compact framework doesn't support Array.Find
                // InputManager im = Array.Find(inputManagers, m => (m != null) && (m.NetworkGamer == g));
                InputManager im = null;
                for (int i = 0; i < inputManagers.Length; i++)
                {
                    if (inputManagers[i] != null && inputManagers[i].NetworkGamer == g)
                    {
                        im = inputManagers[i];
                        break;
                    }
                }
                im.Player = result;

            }

            result.laneCount = lanes.Length;
            int startLane = (int)startLocations[gamerIndex, 1];
            result.snapLaneY = lanes[startLane].yLocation + lanes[startLane].laneHeight / 2;
            result.Reset(startLocations[gamerIndex, 0], result.snapLaneY, startLocations[gamerIndex, 2]);
            result.Lane = lanes[GetLaneFromY((int)result.Y)];

            gameOverlay.SetPlayer(result);

            return result;
        }

        //void OnPlayerDrowned(DisplayObjectContainer sender)
        //{
        //    sender.PlayheadWrap -= new AnimationEvent(OnPlayerDrowned);
        //    sender.Stop();
        //    sender.DestroyAfterUpdate();
        //}
        private void KillPlayer(SmuckPlayer p)
        {
            p.LivingState = LivingState.Dying;
            if (this is SteamRollerScreen || p.isExploding)
            {
                p.body.SetLinearVelocity(Vector2.Zero);
            }
            //if (p.IsNaked)
            //{
            //    p.LivingState = LivingState.Dead;
            //}
            //else
            //{
            //    p.LivingState = LivingState.Dying;
            //}
        //    if (p.LivingState == LivingState.Alive)
        //    {
        //        if (p.DefinitionName.StartsWith("swim"))
        //        {
        //            p.ReplaceView("drowningPlayer");
        //            p.PlayAll();
        //            p.PlayheadWrap += new AnimationEvent(OnPlayerDrowned);
        //        }
        //        else
        //        {
        //            p.ReplaceView("deadPlayer0");
        //        }
        //        p.LivingState = LivingState.Dying;
        //    }
        }

        //public override void PostSolve(Contact contact, ref ContactImpulse impulse)
        //{
        //    base.PostSolve(contact, ref impulse);
        //}
        //public override void PreSolve(Contact contact, ref Manifold oldManifold)
        //{
        //    base.PreSolve(contact, ref oldManifold);
        //}
        protected V2DSprite collisionObjA;
        protected V2DSprite collisionObjB;

        public override void BeginContact(Contact contact)
        {
            collisionObjA = (V2DSprite)contact.GetFixtureA().GetBody().GetUserData();
            collisionObjB = (V2DSprite)contact.GetFixtureB().GetBody().GetUserData();

            SmuckPlayer p = null;
            V2DSprite nonPlayerObj = null;
            if(collisionObjA is SmuckPlayer)
            {
                p = (SmuckPlayer)collisionObjA; 
                nonPlayerObj = collisionObjB;
            }
            else if(collisionObjB is SmuckPlayer)
            {
                p = (SmuckPlayer)collisionObjB; 
                nonPlayerObj = collisionObjA;
            }

            if (p != null)
            {
                LaneVehicle v = nonPlayerObj is LaneVehicle ? (LaneVehicle)nonPlayerObj : null;
                if (v != null)
                {
                    if (v.Lane.LaneKind == LaneKind.DrownWater && !collideWithBoats)
                    {
                        if (p.LivingState == LivingState.Alive)
                        {
                            p.aboardVehicle = v;
                        }
                        else
                        {
                            p.DestroyAfterUpdate(); // dont want drowning amin over boats
                        }
                    }
                    else
                    {
                        Manifold m;
                        contact.GetManifold(out m);
                        Vector2 dir = m._localNormal * (p == collisionObjA ? 20 : -20) + v.body.GetLinearVelocity() * 10;
                        if (Math.Abs(dir.Y) < 60)
                        {
                            dir.Y += rnd.Next(-400, 400);
                        }
                        p.isExploding = false;
                        p.body.ApplyLinearImpulse(dir, p.body.GetPosition());
                        float torque = dir.Y > 0 ? 1 : -1;
                        p.body.ApplyTorque(torque);
                        p.body.SetAngularVelocity(rnd.Next(15) * torque);

                        if (p.LivingState == LivingState.Alive) // first hit a whack
                        {
                            stage.audio.PlaySound(Sfx.whack);
                        }
                        stage.audio.PlaySound(Sfx.secondWhack);

                        this.KillPlayer(p);
                    }
                }
                else
                {
                    if (nonPlayerObj.InstanceName.StartsWith("water"))
                    {
                        p.IsOnWater = true;
                        if (p.aboardVehicle == null)
                        {
                            p.Lane = lanes[GetLaneFromY((int)nonPlayerObj.Y)];
                            p.LivingState = LivingState.Dying;
                            stage.audio.PlaySound(Sfx.bigSplash);
                        }
                    }
                    else if (nonPlayerObj.InstanceName.StartsWith("tree"))
                    {
                        if (rnd.Next(20) == 1)
                        {
                            p.IsNaked = true;
                        }
                    }
                    else if (p.LivingState == LivingState.Dying && nonPlayerObj.InstanceName.StartsWith("border"))
                    {
                        // no death icon when flying off left or right side of highway
                        if (nonPlayerObj.Index == 0 || nonPlayerObj.Index == 2) 
                        {
                            p.skipDeathMarker = true;
                        }
                        p.DestroyAfterUpdate();
                    }
                }
            }
            else if (collisionObjA is LaneVehicle && collisionObjB is LaneVehicle)
            {
                LaneVehicle vA = (LaneVehicle)collisionObjA;
                LaneVehicle vB = (LaneVehicle)collisionObjB;
                const float boost = 15;
                if (vA.Lane.movesRight && vB.Lane.movesRight)
                {
                    if (vA.Position.X > vB.Position.X)
                    {
                        vA.MaxSpeed = vA.Lane.vehicleSpeed + boost;
                        vB.MaxSpeed = vA.MaxSpeed - boost;
                    }
                    else
                    {
                        vB.MaxSpeed = vB.Lane.vehicleSpeed + boost;
                        vA.MaxSpeed = vB.MaxSpeed - boost;
                    }
                }
                else if (!vA.Lane.movesRight && !vB.Lane.movesRight)
                {
                    if (vA.Position.X > vB.Position.X)
                    {
                        vB.MaxSpeed = vB.Lane.vehicleSpeed + boost;
                        vA.MaxSpeed = vB.MaxSpeed - boost;
                    }
                    else
                    {
                        vA.MaxSpeed = vA.Lane.vehicleSpeed + boost;
                        vB.MaxSpeed = vA.MaxSpeed - boost;
                    }
                }
            }
        }
        public override void EndContact(Contact contact)
        {
            V2DSprite objA = (V2DSprite)contact.GetFixtureA().GetBody().GetUserData();
            V2DSprite objB = (V2DSprite)contact.GetFixtureB().GetBody().GetUserData();

            SmuckPlayer p = objA is SmuckPlayer ? (SmuckPlayer)objA : objB is SmuckPlayer ? (SmuckPlayer)objB : null;
            LaneVehicle v = objA is LaneVehicle ? (LaneVehicle)objA : objB is LaneVehicle ? (LaneVehicle)objB : null;
            V2DSprite w = objA.InstanceName.StartsWith("water") ? objA : objB.InstanceName.StartsWith("water") ? objB : null;

            if (p != null)
            {
                if (w != null)
                {
                    p.IsOnWater = false;
                }
                else if (v != null)
                {
                    if (v.Lane.LaneKind == LaneKind.DrownWater)
                    {
                        p.aboardVehicle = null;
                        if (p.IsOnWater)
                        {
                            p.LivingState = LivingState.Dying;
                            stage.audio.PlaySound(Sfx.bigSplash);
                        }
                    }
                    else
                    {
                        this.KillPlayer(p);
                    }
                }
            }
        }
        private int GetLaneFromY(int yPosition)
        {
            int result = lanes.Length - 1;
            for (int i = 1; i < lanes.Length; i++)
            {
                if (yPosition < lanes[i].yLocation)
                {
                    result = i - 1;
                    break;
                }
            }
            return result;
        }
        public override void OnUpdateComplete(GameTime gameTime)
        {
            base.OnUpdateComplete(gameTime);
            if (isLevelOver && !summaryPosted)
            {
                summaryPosted = true;
                foreach (SmuckPlayer p in players)
                {
                    p.RoundComplete();
                }
                StopAllVehicleSounds();
                gameOverlay.EndRound();
            }
        }
        protected void PauseAllVehicleSounds()
        {
            foreach (Lane ln in lanes)
            {
                foreach (LaneVehicle v in ln.vehicles)
                {
                    v.PauseSound();
                }                
            }
        }
        public void ResumeAllVehicleSounds()
        {
            foreach (Lane ln in lanes)
            {
                foreach (LaneVehicle v in ln.vehicles)
                {
                    v.ResumeSound();
                }                
            }
        }
        protected void StopAllVehicleSounds()
        {
            foreach (Lane ln in lanes)
            {
                foreach (LaneVehicle v in ln.vehicles)
                {
                    v.StopSound();
                }                
            }
        }
        public bool WaitingOnPanel
        {
            get { return gameOverlay.hasActivePanel; }
        }
        private bool firstUpdate = true;
        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            if (!firstUpdate && gameOverlay.hasActivePanel)
            {
                base.ManageInput(gameTime);
                gameOverlay.Update(gameTime);
            }
            else if (isActive)
            {
                base.Update(gameTime);
                firstUpdate = false;
                for (int i = 0; i < lanes.Length; i++)
                {
                    lanes[i].Update(gameTime, this);
                }

                for (int i = 0; i < players.Count; i++)
                {
                    SmuckPlayer p = players[i];

                    if (p.LivingState == LivingState.Alive)
                    {
                        int curLane = GetLaneFromY((int)(p.Y + p.Height / 2));
                        if (curLane != p.Lane.laneIndex)
                        {
                            p.snapLaneY = lanes[curLane].yLocation + lanes[curLane].laneHeight / 2;
                            gameOverlay.SetScore(p);                            
                            p.Lane = lanes[curLane];

                            // make it harder and harder after success level of points
                            if (p.roundScore >= pointsToWinRound)
                            {
                                if (endOnLastLane)
                                {
                                    isLevelOver = true;
                                }

                                if (p.Lane.LaneKind != LaneKind.Sidewalk)
                                {
                                    p.Lane.minCreationDelay = (int)(p.Lane.minCreationDelay * .9);
                                    p.Lane.maxCreationDelay = (int)(p.Lane.maxCreationDelay * .9);
                                    p.Lane.vehicleSpeed += 3;
                                }
                            }
                        }
                    }
                    else if (p.IsOnStage && !p.skipDeathMarker && p.LivingState == LivingState.Dying)
                    {
                        float vel = p.body.GetLinearVelocity().LengthSquared();
                        if (p.body.GetLinearVelocity().LengthSquared() < 100)
                        {
                            p.DestroyAfterUpdate();
                        }
                    }

                }
            }
            stage.audio.Update();
        }
        public override void Draw(SpriteBatch batch)
        {
            base.Draw(batch);
        }
    }
}








