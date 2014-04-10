#region Copyright
// ******************************************************************************************
//
// 							SimplePath, Copyright © 2011, Alex Kring
//
// ******************************************************************************************
#endregion

using UnityEngine;
using System.Collections.Generic;
using SimpleAI.Navigation;

namespace SimpleAI.Navigation
{
    /// <summary>
    ///Defines the characteristics of a path request
    /// </summary>
    public class PathPlanParams
    {
        #region Fields
        Vector3         m_startPos;
        Vector3         m_goalPos;
        INavTarget      m_target;           // ex: a position, or a GameObject.
        float           m_replanInterval;   // number of seconds between each replan
        #endregion

        #region Properties
        public Vector3 StartPos 
        {
            get { return m_startPos; }
        }

        public Vector3 GoalPos
        {
            get { return m_goalPos; }
        }

        public float ReplanInterval
        {
            get { return m_replanInterval; }
        }
        #endregion

        public PathPlanParams(Vector3 startPos, INavTarget target, float replanInterval)
        {
            System.Diagnostics.Debug.Assert(replanInterval > 0.0f);
            m_startPos = startPos;
            m_target = target;
            m_goalPos = target.GetNavTargetPosition();
            m_replanInterval = replanInterval;
        }
		
		/// <summary>
		///Paths are replanned. When they are replanned, we need to recompute the new start and goal position of the path,
		///since the world has changed since the path was last planned. This function handles recomputing those values.
		/// </summary>
		/// <param name="newStartPos">
		///The new start position of the path plan (likely the agent's current position)
		/// </param>
        public void UpdateStartAndGoalPos(Vector3 newStartPos)
        {
            m_startPos = newStartPos;
            m_goalPos = m_target.GetNavTargetPosition();
        }
    }
}
