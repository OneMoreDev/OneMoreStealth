#region Copyright
// ******************************************************************************************
//
// 							SimplePath, Copyright © 2011, Alex Kring
//
// ******************************************************************************************
#endregion

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using SimpleAI.Navigation;
using SimpleAI.Planning;
using SimpleAI;

public class PathManagerComponent : MonoBehaviour
{	
	#region Unity Editor Fields
	public PathTerrainComponent					m_pathTerrainComponent;
	public int									m_maxNumberOfNodesPerPlanner = 100;
	public int									m_maxNumberOfPlanners = 20;
	public int 									m_maxNumberOfCyclesPerFrame = 500;
    public int 									m_maxNumberOfCyclesPerPlanner = 50;
	#endregion
    
    #region Fields
    private Pool<PathPlanner>                   m_pathPlannerPool;
    private Queue<PathRequest>                  m_requestPool;
    private LinkedList<PathRequest>             m_activeRequests;
    private LinkedList<PathRequest>             m_completedRequests;    // successfully completed, or failed to complete
    private IPathTerrain                  		m_terrain;
	private bool								m_bInitialized;
    #endregion
	
	#region Properties
	public IPathTerrain PathTerrain
	{
		get { return m_pathTerrainComponent.PathTerrain; }
	}
	#endregion

    #region MonoBehaviour Functions
    void Awake()
    {
		m_bInitialized = false;
		
		if (m_maxNumberOfNodesPerPlanner == 0)
		{
			UnityEngine.Debug.LogError(" 'Max Number Of Nodes Per Planner' must be set to a value greater than 0. Try 100.");
			return;
		}
		
		if (m_maxNumberOfPlanners == 0)
		{
			UnityEngine.Debug.LogError(" 'Max Number Of Planners' must be set to a value greater than 0. Try 20.");
			return;
		}
		
        m_pathPlannerPool = new Pool<PathPlanner>(m_maxNumberOfPlanners);
        foreach (Pool<PathPlanner>.Node planner in m_pathPlannerPool.AllNodes)
        {
            planner.Item.Awake(m_maxNumberOfNodesPerPlanner);
        }
		
        m_requestPool = new Queue<PathRequest>(m_maxNumberOfPlanners);
        for (int i = 0; i < m_maxNumberOfPlanners; i++)
        {
            m_requestPool.Enqueue(new PathRequest());
        }
		
        m_activeRequests = new LinkedList<PathRequest>();
        m_completedRequests = new LinkedList<PathRequest>();
    }

    void Start()
    {
		if ( m_pathTerrainComponent == null )
		{
			UnityEngine.Debug.LogError("Must give the PathManagerComponent a reference to the PathTerrainComponent. You can do this through the Unity Editor.");
			return;	
		}
		
		m_terrain = m_pathTerrainComponent.PathTerrain;
		
		if ( m_terrain == null )
		{
			UnityEngine.Debug.LogError("PathTerrain is NULL in PathTerrainComponent. Make sure you have instantiated a path terrain inside the Awake function" +
						   			   "of your PathTerrainComponent");
			return;
		}
		
		foreach (Pool<PathPlanner>.Node planner in m_pathPlannerPool.AllNodes)
        {
            planner.Item.Start(m_terrain);
        }
		
		m_bInitialized = true;
    }
	
	public void Update()
    {
		if ( !m_bInitialized )
		{
			UnityEngine.Debug.LogError("PathManagerComponent failed to initialized. Can't call Update.");
			return;
		}
		
        // Update the active requests
        UpdateActiveRequests(Time.deltaTime);
        // Update the completed requests
        UpdateCompletedRequests(Time.deltaTime);

        // Update the path planners
        int numCyclesUsed = 0;
        LinkedList<PathRequest> requestsCompletedThisFrame = new LinkedList<PathRequest>();
        foreach (PathRequest request in m_activeRequests)
        {
            PathPlanner pathPlanner = request.PathPlanner.Item;
            int numCyclesUsedByPlanner = pathPlanner.Update(m_maxNumberOfCyclesPerPlanner);
            numCyclesUsed += numCyclesUsedByPlanner;

            if (pathPlanner.HasPlanFailed())
            {
				OnRequestCompleted(request, false);
                requestsCompletedThisFrame.AddFirst(request);
            }
            else if (pathPlanner.HasPlanSucceeded())
            {
                OnRequestCompleted(request, true);
                requestsCompletedThisFrame.AddFirst(request);
            }

            if (numCyclesUsed >= m_maxNumberOfCyclesPerFrame)
            {
                break;
            }
        }

        // Remove all the completed requests from the active list
        foreach (PathRequest request in requestsCompletedThisFrame)
        {
            m_activeRequests.Remove(request);
			
			// If the request failed, then remove it completely
			if ( request.HasFailed() )
			{
				RemoveRequest(request);
			}
        }
    }
    #endregion

    #region Requests
    public IPathRequestQuery RequestPathPlan(PathPlanParams pathPlanParams, IPathAgent agent)
    {
		if ( !m_bInitialized )
		{
			UnityEngine.Debug.LogError("PathManagerComponent isn't yet fully intialized. Wait until Start() has been called. Can't call RequestPathPlan.");
			return null;
		}
		
		if ( m_requestPool.Count == 0 )
		{
			UnityEngine.Debug.Log("RequestPathPlan failed because it is already servicing the maximum number of requests: " +
			                      m_maxNumberOfPlanners.ToString());
			return null;
		}
		
		if ( m_pathPlannerPool.AvailableCount == 0 )
		{
			UnityEngine.Debug.Log("RequestPathPlan failed because it is already servicing the maximum number of path requests: " +
			                      m_maxNumberOfPlanners.ToString());
			return null;
		}
		
		// Clamp the start and goal positions within the terrain space, and make sure they are on the floor.
		pathPlanParams.UpdateStartAndGoalPos( m_terrain.GetValidPathFloorPos(pathPlanParams.StartPos) );
		
		// Make sure this agent does not have an active request
		if ( m_activeRequests.Count	 > 0 )
		{
			foreach ( PathRequest pathRequest in m_activeRequests )
			{
				if ( pathRequest.Agent.GetHashCode() == agent.GetHashCode() )
				{
					System.Diagnostics.Debug.Assert(false, "Each agent can only have one path request at a time.");
					return null;
				}
			}
		}
		
		// Make sure this agent does not have a completed request
		if ( m_activeRequests.Count	 > 0 )
		{
			foreach ( PathRequest pathRequest in m_completedRequests )
			{
				if ( pathRequest.Agent.GetHashCode() == agent.GetHashCode() )
				{
					System.Diagnostics.Debug.Assert(false, "Each agent can only have one path request at a time.");
					return null;
				}
			}
		}
		
        // Create the new request
        Pool<PathPlanner>.Node pathPlanner = m_pathPlannerPool.Get();
        PathRequest request = m_requestPool.Dequeue();
        request.Set(pathPlanParams, pathPlanner, agent);
        m_activeRequests.AddFirst(request);

        // Start the request
        int startNodeIndex = m_terrain.GetPathNodeIndex(pathPlanParams.StartPos);
        int goalNodeIndex = m_terrain.GetPathNodeIndex(pathPlanParams.GoalPos);
        pathPlanner.Item.StartANewPlan(startNodeIndex, goalNodeIndex);

        return request;
    }

    public void RemoveRequest(IPathRequestQuery requestQuery)
    {
		if ( !m_bInitialized )
		{
			UnityEngine.Debug.LogError("PathManagerComponent isn't yet fully intialized. Wait until Start() has been called. Can't call ConsumeRequest.");
			return;
		}
		
		if ( requestQuery == null )
		{
			return;
		}
		
        PathRequest request = GetPathRequest(requestQuery);
		
		if ( request == null )
		{
			return;
		}

        // Request is no longer "completed"
        m_completedRequests.Remove(request);
		
		// In case the request is active, remove it.
		m_activeRequests.Remove(request);

        // Return the request back to the pool so that it can be used again.
        m_pathPlannerPool.Return(request.PathPlanner);
        m_requestPool.Enqueue(request);
    }
    #endregion

    #region Notifications
    private void OnRequestCompleted(PathRequest request, bool bSucceeded)
    {
        if (bSucceeded)
        {
            OnRequestSucceeded(request);
        }
        else
        {
            OnRequestFailed(request);
        }
    }

    private void OnRequestFailed(PathRequest request)
    {
		request.Agent.OnPathAgentRequestFailed();
    }

    private void OnRequestSucceeded(PathRequest request)
    {
        // Hold on to this request until it gets consumed.
        System.Diagnostics.Debug.Assert(!m_completedRequests.Contains(request));
        m_completedRequests.AddFirst(request);
		
		request.Agent.OnPathAgentRequestSucceeded(request);
    }
    #endregion

    #region Update Helpers
    private void UpdateActiveRequests(float deltaTimeInSeconds)
    {
        foreach (PathRequest request in m_activeRequests)
        {
            request.Update(deltaTimeInSeconds);
        }
    }

    private void UpdateCompletedRequests(float deltaTimeInSeconds)
    {
        LinkedList<PathRequest> removeCompletedRequests = new LinkedList<PathRequest>();
        foreach (PathRequest request in m_completedRequests)
        {
            request.Update(deltaTimeInSeconds);
            if (request.IsReadyToReplan())
            {
                // and replan toward that pos.
                request.PlanParams.UpdateStartAndGoalPos(request.Agent.GetPathAgentFootPos());
                request.RestartReplanTimer();

                // Change the queues to mark this request as active.
                removeCompletedRequests.AddFirst(request);
                m_activeRequests.AddFirst(request);

                // Start the request
                int startNodeIndex = m_terrain.GetPathNodeIndex(request.PlanParams.StartPos);
                int goalNodeIndex = m_terrain.GetPathNodeIndex(request.PlanParams.GoalPos);
				request.PathPlanner.Item.StartANewPlan(startNodeIndex, goalNodeIndex);
            }
        }

        foreach (PathRequest request in removeCompletedRequests)
        {
            m_completedRequests.Remove(request);
        }
    }
    #endregion

    #region Misc Helpers
    private PathRequest GetPathRequest(IPathRequestQuery requestQuery)
    {
        PathRequest foundRequest = null;
        System.Diagnostics.Debug.Assert(requestQuery is PathRequest);
        if (requestQuery is PathRequest)
        {
            foundRequest = requestQuery as PathRequest;
        }
        return foundRequest;
    }
    #endregion
}
