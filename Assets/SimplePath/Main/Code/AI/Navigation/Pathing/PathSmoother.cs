#region Copyright
// ******************************************************************************************
//
// 							SimplePath, Copyright © 2011, Alex Kring
//
// ******************************************************************************************
#endregion

using UnityEngine;
using System;
using System.Collections;
using System.Diagnostics;
using SimpleAI;
using SimpleAI.Planning;
using System.Collections.Generic;
using SystemDebug = System.Diagnostics.Debug;

namespace SimpleAI.Navigation
{
	public static class PathSmoother
	{
		/// <summary>
		/// Take in a rough path, and turn it into a smooth path.
		/// </summary>
		public static Vector3[] Smooth(Vector3[] roughPath, Vector3 startPos, Vector3 goalPos,
		                   		  	   Vector3[] aLeftPortalEndPts, Vector3[] aRightPortalEndPts)
		{			
			Vector3[] smoothPath = null;
			
			// Handle the base case when the agent is moving within his current node.
			if (roughPath.Length <= 2)
			{
				smoothPath = new Vector3[2];
				smoothPath[0] = startPos;
				smoothPath[1] = goalPos;
				return smoothPath;
			}
		   
			smoothPath = ApplyFunnelPathSmoothing(
									aLeftPortalEndPts,
									aRightPortalEndPts,
									roughPath.Length + 1,
									startPos, 
									goalPos);
			
			return smoothPath;
		}
		
		/// <summary>
		/// Apply path smoothing using the funnel algorithm. This code is an adaptation of the code on
		/// Mikko Monnonen's blog: http://digestingduck.blogspot.com/2010/03/simple-stupid-funnel-algorithm.html
		/// To see the paper paper on the algorithm, go here:
		/// 	http://webdocs.cs.ualberta.ca/~mburo/ps/thesis_demyen_2006.pdf.
		///  	http://digestingduck.blogspot.com/2010/03/simple-stupid-funnel-algorithm.html
		/// To see this algorithm running interactively, go here: 
		/// 	http://www.cs.wustl.edu/~suri/cs506/projects/sp.html.
		/// This function is O(n) in time, whre n is the number of points in the input path.
		/// </summary>
		private static Vector3[] ApplyFunnelPathSmoothing(Vector3[] aLeftEndPts, Vector3[] aRightEndPts, int maxNumSmoothPts, 
		                                     		      Vector3 startPos, Vector3 destPos)
		{
			int numPortalPts = aLeftEndPts.Length + aRightEndPts.Length + 1*2 + 1*2; // +1 for start +1 for goal.
			Vector3[] portals = new Vector3[numPortalPts];
			portals[0] = startPos;
			portals[1] = startPos;
			int endPtCounter = 0;
			for ( int i = 2; i < numPortalPts-2; i += 2 )
			{
				portals[i] = aLeftEndPts[endPtCounter];
				portals[i+1] = aRightEndPts[endPtCounter];
				endPtCounter++;
			}
			portals[numPortalPts-2] = destPos;
			portals[numPortalPts-1] = destPos;
			Vector3[] smoothPathPts = new Vector3[maxNumSmoothPts];
			int numSmoothPathPts = StringPull(portals, numPortalPts, smoothPathPts, maxNumSmoothPts);
			
			Vector3[] smoothPath = new Vector3[numSmoothPathPts];
			for ( int i = 0; i < numSmoothPathPts; i++ )
			{
				smoothPath[i] = smoothPathPts[i];
			}
			
			return smoothPath;
		}
		
		static float TriArea2(Vector3 a, Vector3 b, Vector3 c)
		{
			float ax = b.x - a.x;
			float az = b.z - a.z;
			float bx = c.x - a.x;
			float bz = c.z - a.z;
			return bx*az - ax*bz;
		}
		
		public static int StringPull(Vector3[] portals, int nportalPts, Vector3[] pts, int maxPts)
		{
			// Find straight path.
			int npts = 0;
			// Init scan state
			Vector3 portalApex = portals[0];
			Vector3 portalLeft = portals[2];
			Vector3 portalRight = portals[3];
			int apexIndex = 0, leftIndex = 0, rightIndex = 0;
		
			// Add start point.
			pts[0] = portalApex;
			npts++;
			
			for (int i = 1; ((i < (nportalPts/2)) && (npts < maxPts)); ++i)
			{
				Vector3 left = portals[i*2+0];
				Vector3 right = portals[i*2+1];
				
				// Update right vertex.
				if (TriArea2(portalApex, portalRight, right) <= 0.0f)
				{
					if (PathUtils.AreVertsTheSame(portalApex, portalRight) || TriArea2(portalApex, portalLeft, right) > 0.0f)
					{
						// Tighten the funnel.
						portalRight = right;
						rightIndex = i;
					}
					else
					{
						 // Right over left, insert left to path and restart scan from portal left point.
						pts[npts] = portalLeft;
						npts++;
						// Make current left the new apex.
						portalApex = portalLeft;
						apexIndex = leftIndex;
						// Reset portal
						portalLeft = portalApex;
						portalRight = portalApex;
						leftIndex = apexIndex;
						rightIndex = apexIndex;
						// Restart scan
						i = apexIndex;
						continue;
					}
				}
		
				// Update left vertex.
				if (TriArea2(portalApex, portalLeft, left) >= 0.0f)
				{
					if (PathUtils.AreVertsTheSame(portalApex, portalLeft) || TriArea2(portalApex, portalRight, left) < 0.0f)
					{
						// Tighten the funnel.
						portalLeft = left;
						leftIndex = i;
					}
					else
					{
						// Left over right, insert right to path and restart scan from portal right point.
						pts[npts] = portalRight;
						npts++;
						// Make current right the new apex.
						portalApex = portalRight;
						apexIndex = rightIndex;
						// Reset portal
						portalLeft = portalApex;
						portalRight = portalApex;
						leftIndex = apexIndex;
						rightIndex = apexIndex;
						// Restart scan
						i = apexIndex;
						continue;
					}
				}
			}
			
			// Append last point to path.
			if (npts < maxPts)
			{
				pts[npts] = portals[nportalPts-1];
				npts++;
			}
			
			return npts;
		}
		
	}
	
	
}