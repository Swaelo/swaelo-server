﻿// ================================================================================================================================
// File:        WorldSimulator.cs
// Description: Handles all the server side physics simulation / game logic while the server is running
// ================================================================================================================================

using System;
using System.Diagnostics;
using BEPUphysics;
using BEPUphysics.Entities.Prefabs;
using BEPUutilities;
using BEPUutilities.Threading;
using Server.Items;
using Server.Networking;

namespace Server.Physics
{
    public static class WorldSimulator
    {
        private static Rendering.Window Window;     //monogame window used to render whats going on with the server
        private static int AccumulatedPhysicsFrames;
        private static double AccumulatedPhysicsTime;
        private static double PreviousTimeMeasurement;
        private static ParallelLooper ParallelLooper;   //assigns however many system threads are available so they can be used by the server to perform jobs
        public static double PhysicsTime { get; private set; }
        public static Space Space;
        
        public static Pathfinding.NavMesh TestLevelNavMesh; //Navigation mesh used for AI pathfinding
        public static Pathfinding.NavMesh WorldNavMesh;
        public static Data.TerrainMesh LevelMesh;   //Level collision mesh used for physics

        public static Entities.EnemyEntity Enemy;   //test enemy
        
        public static Rendering.FPSCharacterControls FPSController; //FPS controller which contains fps camera inside it to move around the server with

        public static void InitializeSimulation(Rendering.Window GameWindow)
        {
            Window = GameWindow;

            //Initialize the parallel looper and assign any extra cpu threads to it
            ParallelLooper = new ParallelLooper();
            if (Environment.ProcessorCount > 1)
            {
                for (int i = 0; i < Environment.ProcessorCount; i++)
                    ParallelLooper.AddThread();
            }
            
            //Initialize the item manager and load all its nessacery information out from the database
            Items.ItemManager.InitializeItemManager();
            
            //Set up the bepu physics world space simulation
            Space = new Space(ParallelLooper);
            Space.ForceUpdater.Gravity = new Vector3(0, -9.81f, 0);

            //Position the scene camera then attach it to the fps controller
            Rendering.Window.Instance.Camera.Position = new Vector3(-3.49f, 5.14f, 4.50f);
            Rendering.Window.Instance.Camera.ViewDirection = new Vector3(0.92f, -0.16f, -0.33f);
            FPSController = new Rendering.FPSCharacterControls(Space, Rendering.Window.Instance.Camera, Rendering.Window.Instance);
            FPSController.CharacterController.Body.Tag = "noDisplayObject";

            //Place a ground plane for everyone to stand upon
            Box Ground = new Box(Vector3.Zero, 50, -1, 50);
            Ground.BecomeKinematic();
            GameWindow.ModelDrawer.Add(Ground);
            WorldSimulator.Space.Add(Ground);

            //Place an enemy entity into the level
            Enemy = new Entities.EnemyEntity(new Vector3(10, 0, 10));

            //Place one of each item into the game world
            int CurrentItemNumber = 1;
            Vector3 CurrentSpawnLocation = new Vector3(-10, 0, -2.5f);
            for(int i = CurrentItemNumber; i < 23; i++)
            {
                //Add in each new item into the game
                ItemManager.AddItemPickup(CurrentItemNumber, CurrentSpawnLocation);

                //Offset the item number and spawn location for the next item spawn
                CurrentItemNumber++;
                CurrentSpawnLocation.X -= 1;
            }
        }

        public static void Update(float DeltaTime)
        {
            //Log what time this update function started
            long UpdateStart = Stopwatch.GetTimestamp();
            
            //Update all the entities AI which are active in the game world right now
            Entities.EntityManager.UpdateEntities(DeltaTime);
            
            //Update the physics scene
            Space.Update();

            //update the camera and debug character controller
            Rendering.Window Game = Rendering.Window.Instance;
            FPSController.Update(DeltaTime, Game.PreviousKeyboardInput, Game.KeyboardInput);

            //Calculate the physics time values based on how long this frame update took
            long UpdateEnd = Stopwatch.GetTimestamp();
            AccumulatedPhysicsTime += (UpdateEnd - UpdateStart) / (double)Stopwatch.Frequency;
            AccumulatedPhysicsFrames++;
            PreviousTimeMeasurement += DeltaTime;
            if(PreviousTimeMeasurement > .3f)
            {
                PreviousTimeMeasurement -= .3f;
                PhysicsTime = AccumulatedPhysicsTime / AccumulatedPhysicsFrames;
                AccumulatedPhysicsTime = 0;
                AccumulatedPhysicsFrames = 0;
            }
        }

        public static void CleanUp()
        {
            if(ParallelLooper != null)
                ParallelLooper.Dispose();
        }
    }
}
