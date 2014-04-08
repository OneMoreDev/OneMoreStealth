#region Copyright
// ******************************************************************************************
//
// 							SimplePath, Copyright © 2011, Alex Kring
//
// ******************************************************************************************
#endregion

using UnityEngine;
using System.Collections;
using SimpleAI.Navigation;
using SimpleAI.Planning;
using SimpleAI.Steering;

[RequireComponent(typeof(PathAgentComponent))]
[RequireComponent(typeof(SteeringAgentComponent))]
[RequireComponent(typeof(Rigidbody))]
public class NavigationAgentComponent : MonoBehaviour 
{	
	#region Unity Editor Fields
	public bool								m_usePathSmoothing = true;
	#endregion
	
	#region Fields
	private PathAgentComponent 				m_pathAgent;
	private SteeringAgentComponent 			m_steeringAgent;
	#endregion
	
	#region Properties
	public IPathTerrain PathTerrain
	{
		get { return m_pathAgent.PathManager.PathTerrain; }
	}
	#endregion
	
	#region MonoBehaviour Functions
	void Awake()
	{
		// Find a reference to the path and steering agent
		m_pathAgent = GetComponent<PathAgentComponent>();
		m_steeringAgent = GetComponent<SteeringAgentComponent>();
	}
	#endregion
	
	#region Movement Requests
	public bool MoveToPosition(Vector3 targetPosition, float replanInterval)
	{
		NavTargetPos navTargetPos = new NavTargetPos(targetPosition, PathTerrain);
		PathPlanParams planParams = new PathPlanParams(transform.position, navTargetPos, replanInterval);
		return m_pathAgent.RequestPath(planParams);
	}
	
	public bool MoveToGameObject(GameObject gameObject, float replanInterval)
	{
		NavTargetGameObject navTargetGameObject = new NavTargetGameObject(gameObject, PathTerrain);
		PathPlanParams planParams = new PathPlanParams(transform.position, navTargetGameObject, replanInterval);
		return m_pathAgent.RequestPath(planParams);
	}
	
	public void CancelActiveRequest()
	{
		m_steeringAgent.StopSteering();
		m_pathAgent.CancelActiveRequest();
	}
	#endregion
	
	#region Messages
	private void OnPathRequestSucceeded(IPathRequestQuery request)
	{
		Vector3[] roughPath = request.GetSolutionPath(m_pathAgent.PathManager.PathTerrain);
		Vector3[] steeringPath = roughPath;
		
		if ( m_usePathSmoothing )
		{
			// Smooth the path
			Vector3[] aLeftPortalEndPts;
			Vector3[] aRightPortalEndPts;
			m_pathAgent.PathManager.PathTerrain.ComputePortalsForPathSmoothing( roughPath, out aLeftPortalEndPts, out aRightPortalEndPts );
			steeringPath = PathSmoother.Smooth(roughPath, request.GetStartPos(), request.GetGoalPos(), aLeftPortalEndPts, aRightPortalEndPts);
		}
		
		// Begin steering along this path
		m_steeringAgent.SteerAlongPath( steeringPath, m_pathAgent.PathManager.PathTerrain );
	}
	
	private void OnPathRequestFailed()
	{
		m_steeringAgent.StopSteering();
		SendMessageUpwards("OnNavigationRequestFailed", SendMessageOptions.DontRequireReceiver);
	}
	
	private void OnSteeringRequestSucceeded()
	{
		SendMessageUpwards("OnNavigationRequestSucceeded", SendMessageOptions.DontRequireReceiver);
	}
	#endregion
}
