#region Copyright
// ******************************************************************************************
//
// 							SimplePath, Copyright © 2011, Alex Kring
//
// ******************************************************************************************
#endregion

using UnityEngine;
using System.Collections.Generic;

namespace SimpleAI.Navigation
{
	/// <summary>
	///This interface embodies a target that an agent can navigate toward. For example,
	///the agent can navigate toward a position, or an object. Inherit from this interface
	///to define your own type of navigation target.
	/// </summary>
    public interface INavTarget
    {
		/// <summary>
		///Get the current position of the navigation target 
		/// </summary>
		/// <returns>
		/// A <see cref="Vector3"/>
		/// </returns>
        Vector3 GetNavTargetPosition();
    }

    public class NavTargetPos : INavTarget
    {
        private Vector3 m_targetPos;
		private IPathTerrain m_pathTerrain;

        public NavTargetPos(Vector3 targetPos, IPathTerrain pathTerrain)
        {
            m_targetPos = targetPos;
			m_pathTerrain = pathTerrain;
        }

        #region ITarget Members
        public Vector3 GetNavTargetPosition()
        {
            return m_pathTerrain.GetValidPathFloorPos( m_targetPos );
        }
        #endregion
    }
	
	public class NavTargetGameObject : INavTarget
    {
        private GameObject m_targetGameObject;
		private IPathTerrain m_pathTerrain;

        public NavTargetGameObject(GameObject targetGameObject, IPathTerrain pathTerrain)
        {
            m_targetGameObject = targetGameObject;
			m_pathTerrain = pathTerrain;
        }

        #region ITarget Members
        public Vector3 GetNavTargetPosition()
        {
            return m_pathTerrain.GetValidPathFloorPos( m_targetGameObject.transform.position );
        }
        #endregion
    }
}
