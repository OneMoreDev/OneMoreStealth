#region Copyright
// ******************************************************************************************
//
// 							SimplePath, Copyright © 2011, Alex Kring
//
// ******************************************************************************************
#endregion

using UnityEngine;
using System.Collections.Generic;
using System.Diagnostics;
using SimpleAI.Planning;

namespace SimpleAI.Navigation
{
    public class PathPlanner : AStarPlanner
    {
        #region Fields
        private IPathTerrain m_pathTerrain;
        #endregion
		
		#region Properties
		public IPathTerrain PathTerrain
		{
			get { return m_pathTerrain; }
		}
		#endregion

        public override void Start(Navigation.IPlanningWorld world)
        {
            base.Start(world);

            System.Diagnostics.Debug.Assert(world is IPathTerrain);
            m_pathTerrain = world as IPathTerrain;
        }
    }
}
