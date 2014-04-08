#region Copyright
// ----------------------------------------------------------------------------
//
// Modifications made by: Alex Kring (c) 2011, for SimplePath
//
// OpenSteerDotNet - pure .net port
// Port by Simon Oliver - http://www.handcircus.com
//
// OpenSteer -- Steering Behaviors for Autonomous Characters
//
// Copyright (c) 2002-2003, Sony Computer Entertainment America
// Original author: Craig Reynolds <craig_reynolds@playstation.sony.com>
//
// Permission is hereby granted, free of charge, to any person obtaining a
// copy of this software and associated documentation files (the "Software"),
// to deal in the Software without restriction, including without limitation
// the rights to use, copy, modify, merge, publish, distribute, sublicense,
// and/or sell copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.  IN NO EVENT SHALL
// THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
// DEALINGS IN THE SOFTWARE.
//
//
// ----------------------------------------------------------------------------
#endregion

using UnityEngine;
using System.Collections;
using System;
using System.Text;

namespace SimpleAI.Steering
{
    public class Pathway
    {
        // given a distance along the path, convert it to a point on the path
        public virtual Vector3 MapPathDistanceToPoint(float pathDistance) { return Vector3.zero; }

        // Given an arbitrary point, convert it to a distance along the path.
        public virtual float MapPointToPathDistance(Vector3 point) { return 0; }
    }

    public class PolylinePathway : Pathway
    {
		#region Fields
        int pointCount;
        Vector3[] points;

        float segmentLength;
        float segmentProjection;
        Vector3 local;
        Vector3 chosen;
        Vector3 segmentNormal;

        float[] lengths;
        Vector3[] normals;
        float totalPathLength;
		#endregion
		
		#region Properties
		public Vector3[] Points
		{
			get { return points; }
		}
		
		public int PointCount
		{
			get { return pointCount; }
		}
		#endregion

        public PolylinePathway () 
        {
            
        }
		
		public PolylinePathway (int _pointCount)
		{
			Initialize(_pointCount, null);
		}
		
        // construct a PolylinePathway given the number of points (vertices),
        // an array of points
        public PolylinePathway (int _pointCount, Vector3[] _points)
        {
            Initialize (_pointCount, _points);
        }
		
		public void ReInitialize (int _pointCount, Vector3[] _points)
		{
			Initialize( _pointCount, _points );
		}

        // utility for constructors in derived classes
        void Initialize (int _pointCount, Vector3[] _points)
        {
            // set data members, allocate arrays
            pointCount = _pointCount;
            totalPathLength = 0;
            lengths = new float [pointCount];
            points  = new Vector3 [pointCount];
            normals = new Vector3 [pointCount];
			
			if ( _points == null )
			{
				return;
			}

            // loop over all points
            for (int i = 0; i < pointCount; i++)
            {
                // copy in point locations, closing cycle when appropriate
                bool closeCycle = false;
                int j = closeCycle ? 0 : i;
                points[i] = _points[j];

                // for the end of each segment
                if (i > 0)
                {
                    // compute the segment length
                    normals[i] = points[i] - points[i-1];
                    lengths[i] = normals[i].magnitude;

                    // find the normalized vector parallel to the segment
                    normals[i] *= 1 / lengths[i];

                    // keep running total of segment lengths
                    totalPathLength += lengths[i];
                }
            }
        }

        // utility methods

        // assessor for total path length;
        public float GetTotalPathLength () {return totalPathLength;}

        public override float MapPointToPathDistance(Vector3 point)
        {
            float d;
            float minDistance = float.MaxValue;
            float segmentLengthTotal = 0;
            float pathDistance = 0;

            for (int i = 1; i < pointCount; i++)
            {
                segmentLength = lengths[i];
                segmentNormal = normals[i];
                d = PointToSegmentDistance (point, points[i-1], points[i]);
                if (d < minDistance)
                {
                    minDistance = d;
                    pathDistance = segmentLengthTotal + segmentProjection;
                }
                segmentLengthTotal += segmentLength;
            }

            // return distance along path of onPath point
            return pathDistance;
        }

        public override Vector3 MapPathDistanceToPoint(float pathDistance)
        {
            // clip or wrap given path distance
            float remaining = pathDistance;
            if (pathDistance < 0) return points[0];
            if (pathDistance >= totalPathLength) return points [pointCount-1];

            // step through segments, subtracting off segment lengths until
            // locating the segment that contains the original pathDistance.
            // Interpolate along that segment to find 3d point value to return.
            Vector3 result = Vector3.zero;
            for (int i = 1; i < pointCount; i++)
            {
                segmentLength = lengths[i];
                if (segmentLength < remaining)
                {
                    remaining -= segmentLength;
                }
                else
                {
                    float ratio = remaining / segmentLength;
                    result = interpolate(ratio, points[i - 1], points[i]);
                    break;
                }
            }
            return result;
        }

        // ----------------------------------------------------------------------------
        // computes distance from a point to a line segment 
        //
        // (I considered moving this to the vector library, but its too
        // tangled up with the internal state of the PolylinePathway instance)

        public float PointToSegmentDistance (Vector3 point, Vector3 ep0, Vector3 ep1)
        {
            // convert the test point to be "local" to ep0
            local = point - ep0;

            // find the projection of "local" onto "segmentNormal"
            segmentProjection = Vector3.Dot(segmentNormal, local);

            // handle boundary cases: when projection is not on segment, the
            // nearest point is one of the endpoints of the segment
            if (segmentProjection < 0)
            {
                chosen = ep0;
                segmentProjection = 0;
                return (point- ep0).magnitude;
            }
            if (segmentProjection > segmentLength)
            {
                chosen = ep1;
                segmentProjection = segmentLength;
                return (point-ep1).magnitude;
            }

            // otherwise nearest point is projection point on segment
            chosen = segmentNormal * segmentProjection;
            chosen +=  ep0;
            return (point-chosen).magnitude;
        }
		
		public static float interpolate(float alpha, float x0, float x1)
		{
		    return x0 + ((x1 - x0) * alpha);
		}
		
		public static Vector3 interpolate(float alpha, Vector3 x0, Vector3 x1)
        {
            return x0 + ((x1 - x0) * alpha);
        }
		
		public static Vector3 TruncateLength(Vector3 vec, float maxLength)
		{
		    float length = vec.magnitude;
		    Vector3 returnVector = vec;
		    if (length > maxLength)
		    {
		        returnVector.Normalize();
		        returnVector *= maxLength;
		    }
		    return returnVector;
		}
    }
}
