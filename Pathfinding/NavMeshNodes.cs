﻿// ================================================================================================================================
// File:        NavMeshNodes.cs
// Description: Tracks all of the mesh node objects that define the entire current navigation mesh
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
    public class NavMeshNodes
    {
        //Stores a list of every nav mesh node object that has been added thus far
        public static List<NavMeshNode> MeshNodes = new List<NavMeshNode>();
        public static Dictionary<Vector3, NavMeshNode> LocationMeshNodes = new Dictionary<Vector3, NavMeshNode>();

        //Gets a mesh node from the dictionary with its world location
        public static NavMeshNode GetNode(Vector3 NodePosition)
        {
            //If this node doesnt exist yet it needs to be created first
            if (IsLocationAvailable(NodePosition))
            {
                //Whenever a new mesh node is created it needs to be stored into the list and dictionary for later use
                NavMeshNode NewNode = new NavMeshNode(NodePosition);
                MeshNodes.Add(NewNode);
                LocationMeshNodes.Add(NodePosition, NewNode);
                return NewNode;
            }

            //Otherwise, we just return the already existing node
            return LocationMeshNodes[NodePosition];
        }

        //Lets 2 mesh nodes know that they are neighbours to one another
        public static void AssignNeighbours(NavMeshNode Node1, NavMeshNode Node2)
        {
            Node1.AddNeighbour(Node2);
            Node2.AddNeighbour(Node1);
        }

        //Lets a set of 3 mesh nodes know that they are all neighbours to one another
        public static void AssignNeighbours(NavMeshNode Node1, NavMeshNode Node2, NavMeshNode Node3)
        {
            //Assign 2 and 3 as neighbours of 1
            Node1.AddNeighbour(Node2);
            Node1.AddNeighbour(Node3);
            //Assign 1 and 3 as neighbours of 2
            Node2.AddNeighbour(Node1);
            Node2.AddNeighbour(Node3);
            //assign 1 and 2 as neighbours of 3
            Node3.AddNeighbour(Node1);
            Node3.AddNeighbour(Node2);
        }

        //Returns a list of all the nodes except for 1 that will not be in the list
        public static List<NavMeshNode> GetOtherNodes(NavMeshNode TargetNode)
        {
            List<NavMeshNode> OtherNodes = new List<NavMeshNode>();
            for (int i = 0; i < MeshNodes.Count; i++)
            {
                if (MeshNodes[i] != TargetNode)
                    OtherNodes.Add(MeshNodes[i]);
            }
            return OtherNodes;
        }

        //Checks that none of the existing mesh nodes are in this location
        public static bool IsLocationAvailable(Vector3 Location)
        {
            foreach(NavMeshNode Node in MeshNodes)
            {
                if (Node.NodeLocation == Location)
                    return false;
            }
            return true;
        }

        public static NavMeshNode GetNearestNode(List<NavMeshNode> NodeList)
        {
            NavMeshNode ClosestNode = NodeList[0];
            for (int i = 0; i < NodeList.Count; i++)
            {
                if (NodeList[i].NeighbourDistance < ClosestNode.NeighbourDistance)
                    ClosestNode = NodeList[i];
            }
            return ClosestNode;
        }

        //Returns whatever nav mesh node is the closest to the target location
        public static NavMeshNode GetNearbyMeshNode(Vector3 NodeLocation)
        {
            NavMeshNode NearbyNode = MeshNodes[0];
            float NodeDistance = Vector3.Distance(NearbyNode.NodeLocation, NodeLocation);
            for (int i = 1; i < MeshNodes.Count; i++)
            {
                NavMeshNode NodeCompare = MeshNodes[i];
                float CompareDistance = Vector3.Distance(NodeCompare.NodeLocation, NodeLocation);
                if (CompareDistance < NodeDistance)
                {
                    NearbyNode = NodeCompare;
                    NodeDistance = CompareDistance;
                }
            }
            return NearbyNode;
        }
    }
}