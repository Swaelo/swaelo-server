﻿// ================================================================================================================================
// File:        NavMeshNode.cs
// Description: Tracks the information of a single mesh node in the navigation mesh, its pathfinding values, neighbours etc.
// Author:      Harley Laurie          
// Notes:       
// ================================================================================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Swaelo_Server
{
    public class NavMeshNode
    {
        public Vector3 NodeLocation = new Vector3(0, 0, 0);
        public Sphere NodeDisplay;
        public List<NavMeshNode> Neighbours = new List<NavMeshNode>();
        
        public NavMeshNode CameFrom = null; //Each node knows which other node it can most efficiently be reached from
        public float GScore = float.MaxValue;   //For each node, the cost of to it from the start node
        public float FScore = float.MaxValue;   //For each node, the total cost of getting to it from the start note

        public Vector2 NodeIndex = new Vector2(0, 0);

        //distance from this node to its parent during neighbour calculations in astar search
        public float NeighbourDistance = 0f;

        public NavMeshNode(Vector3 Location)
        {
            NodeLocation = Location;
            NodeDisplay = new Sphere(Location, 0.1f);
        }

        //Adds the new node to our list of neighbours, if it isnt already one
        public void AddNeighbour(NavMeshNode NewNeighbour)
        {
            //Add the NewNeighbour as one of our Neighbours if we havnt already
            if (!Neighbours.Contains(NewNeighbour))
                Neighbours.Add(NewNeighbour);
        }
    }
}