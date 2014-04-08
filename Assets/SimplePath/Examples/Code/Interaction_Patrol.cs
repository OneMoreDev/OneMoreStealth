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

[RequireComponent(typeof(NavigationAgentComponent))]
public class Interaction_Patrol : MonoBehaviour 
{
	#region Unity Editor Fields
	public GameObject[]						m_patrolNodes;
	public float							m_replanInterval = float.MaxValue;
	#endregion
	
	#region Fields
	private NavigationAgentComponent 		m_navigationAgent;
	private bool							m_bNavRequestCompleted;
	private int								m_currentPatrolNodeGoalIndex;
	#endregion
	
	#region MonoBehaviour Functions
	void Awake()
	{
		m_currentPatrolNodeGoalIndex = 0;
		m_bNavRequestCompleted = true;
		m_navigationAgent = GetComponent<NavigationAgentComponent>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if ( m_patrolNodes == null || m_patrolNodes.Length == 0 )
		{
			Debug.LogError("No patrol nodes are set");
			return;
		}
		
		if ( m_bNavRequestCompleted )
		{
			m_currentPatrolNodeGoalIndex = ( m_currentPatrolNodeGoalIndex + 1 ) % m_patrolNodes.Length;
			Vector3 destPos = GetPatrolNodePosition( m_currentPatrolNodeGoalIndex );
			m_navigationAgent.MoveToPosition(destPos, m_replanInterval);
			m_bNavRequestCompleted = false;
		}
	}
	#endregion
	
	private Vector3 GetPatrolNodePosition(int index)
	{
		if ( m_patrolNodes == null || m_patrolNodes.Length == 0 )
		{
			Debug.LogError("No patrol nodes are set");
			return transform.position;
		}
		
		if ( index < 0 || index >= m_patrolNodes.Length	)
		{
			Debug.LogError("PatrolNode index out of bounds");
			return transform.position;
		}
		
		Vector3 patrolNodePosition = m_patrolNodes[index].transform.position;
		
		return patrolNodePosition;
	}
	
	#region Messages
	private void OnNavigationRequestSucceeded()
	{
		m_bNavRequestCompleted = true;
	}
	
	private void OnNavigationRequestFailed()
	{
		m_bNavRequestCompleted = true;
	}
	#endregion
}
