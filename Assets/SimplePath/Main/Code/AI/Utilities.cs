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
using SystemDebug = System.Diagnostics.Debug;

namespace SimpleAI
{
    public static class FlagUtils
    {
        public static bool TestFlag(int flags, int flag)
        {
            return ((flags & flag) != 0);
        }

        public static int SetFlag(int flags, int flag)
        {
            return (flags |= flag);
        }

        public static int ClearFlag(int flags, int flag)
        {
            return (flags & ~flag);
        }
    }
	
	public static class PathUtils
	{
		public static bool AreVertsTheSame(Vector3 v1, Vector3 v2)
		{
			const float eps = 0.001f * 0.001f;
			return ((v1 - v2).sqrMagnitude < eps);
		}
		
		/// <summary>
		/// Get the clockwise angle from dir1 to dir2, in radians. This assumes +y is the up dir.
		/// </summary>
		/// <param name="dir1">
		/// Start vector. Must be normalized.
		/// </param>
		/// <param name="dir2">
		/// End vector. Rotate toward this vector from v1. Must be normalized.
		/// </param>
		/// <returns>
		/// Clockwise angle in radians
		/// </returns>
		public static float CalcClockwiseAngle(Vector3 dir1, Vector3 dir2)
		{
		   dir1.y = 0.0f;
		   dir2.y = 0.0f;
		   dir1.Normalize();
		   dir2.Normalize();
		
		   // Find the clockwise angle
		   Vector3 dir1Normal = Vector3.Cross(dir1, Vector3.up);
		   dir1Normal.Normalize();
		   float checkDirectionDot = Vector3.Dot(dir2, dir1Normal);    // dot for checking the direction of rotation (CW or CCW)
		   float cwAngle = 0.0f;
		   float dot = Vector3.Dot(dir1, dir2);                        // actual dot between the two vectors
		   if (checkDirectionDot > 0.0f)
		   {
		      cwAngle = Mathf.PI * 2.0f - Mathf.Acos(dot);
		   }
		   else
		   {
		      cwAngle = Mathf.Acos(dot);
		   }
		
		   return cwAngle;
		}	
	}
}
