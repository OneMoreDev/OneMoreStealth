#region Copyright
// ******************************************************************************************
//
// 							SimplePath, Copyright © 2011, Alex Kring
//
// ******************************************************************************************
#endregion

using UnityEngine;
using System.Collections;
using SimpleAI.Planning;
using SimpleAI.Navigation;

public class PathAgentComponent : MonoBehaviour, IPathAgent
{
	#region Unity Editor Fields
	public PathManagerComponent				m_pathManager;
	public Color							m_debugPathColor = Color.yellow;
	public Color							m_debugGoalColor = Color.red;
	public bool								m_debugShowPath = false;
	#endregion
	
	#region Fields
	private IPathRequestQuery 				m_query;
	private bool							m_bInitialized;
	#endregion
	
	#region Properties
	public IPathRequestQuery PathRequestQuery
	{
		get { return m_query; }
	}
	
	public PathManagerComponent PathManager
	{
		get { return m_pathManager; }
	}
	
	public bool ShowPath
	{
		get { return m_debugShowPath; }
		set { m_debugShowPath = value; }
	}
	#endregion
	
	#region MonoBehaviour Functions
	void Awake()
	{
		m_bInitialized = false;
		m_query = null;
		
		if ( m_pathManager == null )
		{
			Debug.LogError("PathAgentComponent does not have a PathManagerComponent set. You need to set this up through the Inspector window");
		}
		else
		{
			m_bInitialized = true;
		}
	}
	
	void OnDrawGizmos()
	{
		if ( m_debugShowPath && m_bInitialized)
		{
			// Draw the active path, if it is solved.
			if ( HasActiveRequest() && PathRequestQuery.HasSuccessfullyCompleted() )
			{
				Gizmos.color = m_debugPathColor;
				Vector3[] path = PathRequestQuery.GetSolutionPath( PathManager.PathTerrain );
				for (int i = 1; i < path.Length; i++)
				{
					Vector3 start = path[i-1];
					Vector3 end = path[i];
					Gizmos.DrawLine(start, end);
				}
			}	
		}
	}
	#endregion
	
	public bool RequestPath(PathPlanParams pathPlanParams)
	{
		if ( !m_bInitialized )
		{
			return false;
		}
		
	    m_pathManager.RemoveRequest(m_query);
		m_query = m_pathManager.RequestPathPlan(pathPlanParams, this);
		if ( m_query == null )
		{
			return false;
		}
		return true;
	}

	/// <summary>
	/// True if the agent has an active path request (completed, or running), and false otherwise (hasn't started a path request).
	/// </summary>
	/// <returns>
	/// A <see cref="System.Boolean"/>
	/// </returns>
	public bool HasActiveRequest()
	{
		return (m_query != null);
	}
	
	public void CancelActiveRequest()
	{
		if ( m_query == null )
		{
			return;
		}
			
		m_pathManager.RemoveRequest(m_query);	
		m_query = null;
	}
	
	#region IPathAgent Implementation
	public Vector3 GetPathAgentFootPos()
	{
		return m_pathManager.PathTerrain.GetValidPathFloorPos( transform.position );
	}
	
	public void OnPathAgentRequestSucceeded(IPathRequestQuery request)
	{
		SendMessageUpwards("OnPathRequestSucceeded", request, SendMessageOptions.DontRequireReceiver);
	}
	
	public void OnPathAgentRequestFailed()
	{
		m_query = null;
		SendMessageUpwards("OnPathRequestFailed", SendMessageOptions.DontRequireReceiver);
	}
	#endregion
}
		