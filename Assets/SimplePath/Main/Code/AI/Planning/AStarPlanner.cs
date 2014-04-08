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
using SimpleAI.Navigation;

namespace SimpleAI.Planning
{
	/// <summary>
	///Generates plans using the A* search algorithm.
	/// </summary>
    public class AStarPlanner : Planner
    {
        #region Fields
        protected BinaryHeap<Node>                    m_openNodes;
        protected Pool<Node>                          m_nodePool;
        protected Dictionary<int, Pool<Node>.Node >   m_expandedNodes;
        protected Node                                m_startNode;
        protected Node                                m_goalNode;
        protected LinkedList<Node>                    m_solution;
        protected SuccessCondition                    m_successCondition;
        private ReachedGoalNode_SuccessCondition      m_reachedGoalNodeSuccessCondition;    // Ideally this would be stored outside the planner.
        #endregion

        #region Properties
        public LinkedList<Node> Solution
        {
            get { return m_solution; }
        }
        #endregion

        public AStarPlanner()
        {
            
        }

        public override void Awake(int maxNumberOfNodes)
        {
            base.Awake(maxNumberOfNodes);
			
            m_openNodes = new BinaryHeap<Node>();
            m_openNodes.Capacity = maxNumberOfNodes;
            m_nodePool = new Pool<Node>(maxNumberOfNodes);
            m_expandedNodes = new Dictionary<int, Pool<Node>.Node>(maxNumberOfNodes);
            m_solution = new LinkedList<Node>();
            m_reachedGoalNodeSuccessCondition = new ReachedGoalNode_SuccessCondition();
        }

        public override void Start(IPlanningWorld world)
        {
            base.Start(world);
        }

        public override int Update(int numCyclesToConsume)
        {
            if (m_planStatus != ePlanStatus.kPlanning)
            {
                return 0;
            }

            int numCyclesConsumed = 0;

            while (numCyclesConsumed < numCyclesToConsume)
            {
                m_planStatus = RunSingleAStarCycle();
                numCyclesConsumed++;
                if (m_planStatus == ePlanStatus.kPlanFailed)
                {
                    break;
                }
                else if (m_planStatus == ePlanStatus.kPlanSucceeded)
                {
                    break;
                }
            }

            return numCyclesConsumed;
        }

        public void StartANewPlan(int startNodeIndex, int goalNodeIndex)
        {
			if ( startNodeIndex	== Node.kInvalidIndex || goalNodeIndex == Node.kInvalidIndex )
			{
				m_planStatus = Planner.ePlanStatus.kPlanFailed;
				return;
			}
			
            // Clear out the old data
            m_nodePool.Clear();
            m_openNodes.Clear();
            m_solution.Clear();
            m_expandedNodes.Clear();

            // Set the new data
            m_startNode = GetNode(startNodeIndex).Item;
            m_goalNode = GetNode(goalNodeIndex).Item;

            // Initialize the success condition
            m_reachedGoalNodeSuccessCondition.Awake(m_goalNode);
            m_successCondition = m_reachedGoalNodeSuccessCondition;

            // Put the start node on the open list
            m_startNode.G = 0.0f;
            m_startNode.H = World.GetHCost(m_startNode.Index, m_goalNode.Index);
            m_startNode.F = m_startNode.G + m_startNode.H;
            m_startNode.Parent = null;
            OpenNode(m_startNode);

            m_planStatus = ePlanStatus.kPlanning;
        }

        /// <summary>
        /// Update the current path plan by running a single cycle of the A* search. A "single A* cycle"
        /// expands a single node, and all of its neighbors. To run a full A* search, just run this function
        /// repeatedly until the function returns kSuccessfullySolvedPath, or  kFailedToSolvePath. 
        /// Note that the openNodes variable is a binary heap data structure.
        /// 
        /// Assumptions:		The start node has already been added to the openNodes, and the start node is the only node currently
        ///                     stored inside openNodes.
        /// </summary>
        /// <returns>
        /// Return the status of the path being solved. The path has either been solved, we failed to solve the path, or
        /// we are still in progress of solving the path.
        /// </returns>
        protected ePlanStatus RunSingleAStarCycle()
        {
			// Note: This failure condition must be tested BEFORE we remove an item from the open heap.
			if (m_openNodes.Count == 0)
            {
                return ePlanStatus.kPlanFailed;
            }
			
            // The current least costing pathnode is considered the "current node", which gets removed from the open list and added to the closed list.
            Node currentNode = m_openNodes.Remove();
            CloseNode(currentNode);

            if ( PlanSucceeded(currentNode) )
            {
                ConstructSolution();
                return ePlanStatus.kPlanSucceeded;
            }
            else if ( PlanFailed(currentNode) )
            {
                return ePlanStatus.kPlanFailed;
            }

            int[] neighbors = null;
            int numNeighbors = World.GetNeighbors(currentNode.Index, ref neighbors);
            for (int i = 0; i < numNeighbors; i++)
            {
                float actualCostFromCurrentNodeToNeighbor, testCost;
                int neighborIndex = neighbors[i];
                if (neighborIndex == Node.kInvalidIndex)
                {
                    // This neighbor is off the map.
                    continue;
                }

                Pool<Node>.Node neighbor = GetNode(neighborIndex);

                if (m_expandedNodes.Count == m_maxNumberOfNodes)
                {
					UnityEngine.Debug.LogWarning("Pathplan failed because it reached the max node count. Try increasing " +
                             "the Max Number Of Nodes Per Planner variable on the PathManager, through " +
                             "the Inspector window.");
                    return ePlanStatus.kPlanFailed;
                }

                switch (neighbor.Item.State)
                {
                    case Node.eState.kBlocked:
                    case Node.eState.kClosed:
                        // Case 1: Ignore
                        continue;

                    case Node.eState.kUnvisited:
                        // Case 2: Add to open list
                        RecordParentNodeAndPathCosts(neighbor.Item, currentNode);
                        OpenNode(neighbor.Item);
                        break;

                    case Node.eState.kOpen:
                        // Case 3: Update scores
                        actualCostFromCurrentNodeToNeighbor = World.GetGCost(currentNode.Index, neighbor.Item.Index);
                        testCost = currentNode.G + actualCostFromCurrentNodeToNeighbor;
                        if (testCost < neighbor.Item.G)
                        {
                            RecordParentNodeAndPathCosts(neighbor.Item, currentNode);
                            // Maintain the heap property.
                            m_openNodes.Remove(neighbor.Item);
                            m_openNodes.Add(neighbor.Item);
                        }

                        break;

                    default:
                        System.Diagnostics.Debug.Assert(false, "PathNode is in an invalid state when running a single cycle of A*");
                        break;
                };
            }

            return ePlanStatus.kPlanning;
        }

        protected void RecordParentNodeAndPathCosts(Node node, Node parentNode)
        {
            node.G = parentNode.G + World.GetGCost(parentNode.Index, node.Index);
            node.H = World.GetHCost(node.Index, m_goalNode.Index);
            node.F = node.G + node.H;
            node.Parent = parentNode;
        }

        protected Pool<Node>.Node GetNode(int nodeIndex)
        {
            Pool<Node>.Node node;
            if ( !m_expandedNodes.TryGetValue(nodeIndex, out node) )
            {
                // If there is no existing node with this index, then we create a node
                node = CreateNode(nodeIndex);
            }
            return node;
        }

        protected bool PlanFailed(Node currentNode)
        {
            // The plan failed if we reached the maximum search depth, or if we explored the entire search space.

            // can't fail on the first node
            if (currentNode == m_startNode)
            {
                return false;
            }

            if (m_openNodes.Count == m_maxNumberOfNodes)
            {
				UnityEngine.Debug.LogWarning("Pathplan failed because it reached the max node count. Try increasing " +
				                             "the Max Number Of Nodes Per Planner variable on the PathManager, through " +
				                             "the Inspector window.");
                return true;
            }

            return false;
        }

        protected void ConstructSolution()
        {
            for (Node nextNode = m_goalNode; nextNode != m_startNode; nextNode = nextNode.Parent)
            {
                m_solution.AddFirst(nextNode);
            }

            m_solution.AddFirst(m_startNode);
        }

        protected bool PlanSucceeded(Node currentNode)
        {
            return m_successCondition.Evaluate(currentNode);
        }

        protected Pool<Node>.Node CreateNode(int nodeIndex)
        {
            System.Diagnostics.Debug.Assert(m_nodePool.ActiveCount == m_expandedNodes.Count);

            Pool<Node>.Node newNode = m_nodePool.Get();
            m_expandedNodes[nodeIndex] = newNode;
            Node.eState nodeState = Node.eState.kUnvisited;
            if (World.IsNodeBlocked(nodeIndex))
            {
                nodeState = Node.eState.kBlocked;
            }
            newNode.Item.Awake(nodeIndex, nodeState);

            System.Diagnostics.Debug.Assert(m_nodePool.ActiveCount == m_expandedNodes.Count);

            return newNode;
        }

        protected void OpenNode(Node node)
        {
            System.Diagnostics.Debug.Assert(node.State != Node.eState.kOpen);
            node.State = Node.eState.kOpen;
            m_openNodes.Add(node);
        }

        protected void CloseNode(Node node)
        {
            node.State = Node.eState.kClosed;
        }
    }
}
