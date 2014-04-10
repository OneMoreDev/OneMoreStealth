#region Copyright
// ******************************************************************************************
//
// 							SimplePath, Copyright © 2011, Alex Kring
//
// ******************************************************************************************
#endregion

using UnityEngine;
using SimpleAI.Navigation;

namespace SimpleAI.Planning
{
	/// <summary>
	///Responsible for generating a plan 
	/// </summary>
    public abstract class Planner
    {
        public enum ePlanStatus
        {
            kInvalid = -1,
            kPlanning,
            kPlanSucceeded,
            kPlanFailed
        };

        #region Fields
        protected ePlanStatus 		m_planStatus;
		protected int 				m_maxNumberOfNodes;
        private IPlanningWorld 		m_world;
        #endregion

        #region Properties
        protected IPlanningWorld World
        {
            get { return m_world; }
        }
        #endregion

        public Planner()
        {
            m_planStatus = ePlanStatus.kInvalid;
			m_maxNumberOfNodes = 0;
        }

        public virtual void Awake(int maxNumberOfNodes)
        {
			m_maxNumberOfNodes = maxNumberOfNodes;
        }

        public virtual void Start(IPlanningWorld world)
        {
            m_world = world;
        }

        /// <summary>
        /// Update the planner by one step
        /// </summary>
        /// <param name="numCyclesToConsume">Maximum number of planning cycles the planner can consume</param>
        /// <returns>The number of planning cycles that were actually consumed</returns>
        public virtual int Update(int numCyclesToConsume)
        {
            return 0;
        }

        public virtual void OnDrawGizmos()
        {

        }

        public bool HasPlanSucceeded()
        {
            return (m_planStatus == ePlanStatus.kPlanSucceeded);
        }

        public bool HasPlanFailed()
        {
            return (m_planStatus == ePlanStatus.kPlanFailed);
        }

        public bool HasPlanCompleted()
        {
            return (m_planStatus == ePlanStatus.kPlanFailed || m_planStatus == ePlanStatus.kPlanSucceeded);
        }
    }
}
