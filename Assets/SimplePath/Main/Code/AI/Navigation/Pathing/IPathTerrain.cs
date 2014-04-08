#region Copyright
// ******************************************************************************************
//
// 							SimplePath, Copyright © 2011, Alex Kring
//
// ******************************************************************************************
#endregion

using UnityEngine;
using System;
using System.Collections.Generic;

namespace SimpleAI.Navigation
{
	/// <summary>
	///Defines the interface to the planning world. If you want to define your own terrain (ex: a nav mesh), then you need
	///to inherit from this interface.
	/// </summary>
	public interface IPlanningWorld 
	{
		/// <summary>
		///Get the neighbors of a node
		/// </summary>
		/// <param name="index">
		///Node index
		/// </param>
		/// <param name="neighbors">
		///Returns the neighbors of the node
		/// </param>
		/// <returns>
		///Number of neighbors this node has
		/// </returns>
	    int GetNeighbors(int index, ref int[] neighbors);
		
		/// <summary>
		///Get the total number of nodes in the planning world
		/// </summary>
		/// <returns>
		/// A <see cref="System.Int32"/>
		/// </returns>
	    int GetNumNodes();
		
		/// <summary>
		///Get the heuristic cost to traverse from one node to another
		/// </summary>
		/// <param name="startNodeIndex">
		///Index of the start node
		/// </param>
		/// <param name="destNodeIndex">
		///Index of the goal node
		/// </param>
		/// <returns>
		///The cost to travel between these two nodes
		/// </returns>
	    float GetHCost(int startNodeIndex, int destNodeIndex);
		
		/// <summary>
		///Get the actual cost to traverse from one node to another
		/// </summary>
		/// <param name="startNodeIndex">
		///Index of the start node
		/// </param>
		/// <param name="destNodeIndex">
		///Index of the goal node
		/// </param>
		/// <returns>
		///The cost to travel between these two nodes
		/// </returns>
		float GetGCost(int startNodeIndex, int destNodeIndex);
		
		/// <summary>
		///Determine if a node is blocked / disabled (ex: an obstacle is placed on top of a grid cell, which will disable the node representing that cell)
		/// </summary>
		/// <param name="index">
		///Node index
		/// </param>
		/// <returns>
		///True if the node is blocked, and false otherwise
		/// </returns>
	    bool IsNodeBlocked(int index);
	}
	
	/// <summary>
	///Defines the interface to the path planning world
	/// </summary>
	public interface IPathTerrain : IPlanningWorld
	{
		/// <summary>
		///Get the node closest to a position (assuming the position is inside the terrain)
		/// </summary>
		/// <param name="pos">
		///Position
		/// </param>
		/// <returns>
		///Index of the node closest to the passed position
		/// </returns>
	    int GetPathNodeIndex(Vector3 pos);
		
		/// <summary>
		///Get the position of a node 
		/// </summary>
		/// <param name="index">
		///Node index
		/// </param>
		/// <returns>
		///Position of the node
		/// </returns>
	    Vector3 GetPathNodePos(int index);
		
		/// <summary>
		///Compute the portals along a path. "Portal" is a term used in pathsmoothing, and was first defined by the
		///funnel algorithm. For example, let's assume the terrain is a grid. Then the first portal will be the edge
		///of the grid cell that intersects the vector (roughPath[1] - roughPath[0]). This function can be defined
		///for a nav mesh as well, and any terrain type in general. We only need the portals for pathsmoothing. If you
		///are not using pathsmoothing, then this function can be ignored.
		/// </summary>
		/// <param name="roughPath">
		///Find all portals that belong to this path
		/// </param>
		/// <param name="aPortalLeftEndPts">
		///Returns the left end points of all the portals, going from the start of the roughPath to the end of the roughPath
		/// </param>
		/// <param name="aPortalRightEndPts">
		///Returns the right end points of all the portals, going from the start of the roughPath to the end of the roughPath
		/// </param>
		void ComputePortalsForPathSmoothing(Vector3[] roughPath, out Vector3[] aPortalLeftEndPts, out Vector3[] aPortalRightEndPts);
		
		/// <summary>
		///Determine if the passed position is within the terrain boundaries. 
		/// </summary>
		/// <param name="position">
		/// A <see cref="Vector3"/>
		/// </param>
		/// <returns>
		/// A <see cref="System.Boolean"/>
		/// </returns>
		bool IsInBounds(Vector3 position);
		
		/// <summary>
		///Get the floor position of the path terrain, at the passed position.
		///In other words, project the passed position onto the PathTerrain. This function also clamps the
		///position to be within the terrain space. For the best results, this function should return a position that is
		///unobstructed.
		/// </summary>
		/// <param name="position">
		/// A <see cref="Vector3"/>
		/// </param>
		/// <returns>
		/// A <see cref="Vector3"/>
		/// </returns>
		Vector3 GetValidPathFloorPos(Vector3 position);
		
		/// <summary>
		///Get the height of the terrain at the specified position (only the x and z values of the passed position matter).
		///For example, if there is a heightmap attached to the terrain, then this function should look at the heightmap.
		/// </summary>
		/// <param name="position">
		/// A <see cref="Vector3"/>
		/// </param>
		/// <returns>
		/// A <see cref="System.Single"/>
		/// </returns>
		float GetTerrainHeight(Vector3 position);
	}
	
	/// <summary>
	///Defines the heightmap, used for moving across uneven terrain. 
	/// </summary>
	public interface IHeightmap
	{
		/// <summary>
		///Given a position, find the height of the terrain at that position. 
		/// </summary>
		/// <param name="position">
		/// Find the height at this position. We only care about the XZ of this parameter, and we return the Y.
		/// </param>
		/// <returns>
		/// Height of the terrain at the passed point.
		/// </returns>
		float SampleHeight(Vector3 position);
	}
}