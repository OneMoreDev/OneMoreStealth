#region Copyright
// ******************************************************************************************
//
// 							SimplePath, Copyright © 2011, Alex Kring
//
// ******************************************************************************************
#endregion

using System.Collections.Generic;

namespace SimpleAI.Planning
{
	/// <summary>
	///Responsible for determining if a plan has succeded. If you would like to define your own success condition for a
	///plan, then inherit from this abstract class.
	/// </summary>
    public abstract class SuccessCondition
    {
		/// <summary>
		///Determine if the plan has succeded. Return true if the plan has succeeded, and false otherwise. 
		/// </summary>
		/// <param name="currentNode">
		///The node currently being evaluated by the planner.
		/// </param>
		/// <returns>
		/// A <see cref="System.Boolean"/>
		/// </returns>
        public abstract bool Evaluate(Node currentNode);
    }

    public class ReachedGoalNode_SuccessCondition : SuccessCondition
    {
        private Node m_goalNode;

        public void Awake(Node goalNode)
        {
            m_goalNode = goalNode;
        }

        public override bool Evaluate(Node currentNode)
        {
            if (m_goalNode == currentNode)
            {
                return true;
            }

            return false;
        }
    }
}
