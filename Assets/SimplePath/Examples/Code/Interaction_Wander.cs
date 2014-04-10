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
public class Interaction_Wander : MonoBehaviour 
{
	#region Unity Editor Fields
	public float							m_replanInterval = 0.5f;
	#endregion
	
	#region Fields
	private NavigationAgentComponent 		m_navigationAgent;
	private bool							m_bNavRequestCompleted;
	private PathGrid						m_pathGrid;
	#endregion
	
	#region MonoBehaviour Functions
	void Awake()
	{
		m_bNavRequestCompleted = true;
		m_navigationAgent = GetComponent<NavigationAgentComponent>();
	}
	
	// Use this for initialization
	void Start () 
	{
		if ( m_navigationAgent.PathTerrain == null || !(m_navigationAgent.PathTerrain is PathGrid) )
		{
			Debug.LogError("Interaction_Wander was built to work with a PathGrid terrain; can't use it on other terrain types.");
		}
		else
		{
			m_pathGrid = m_navigationAgent.PathTerrain as PathGrid;
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		if ( m_bNavRequestCompleted )
		{
			Vector3 destPos = ChooseRandomDestination();
			if ( m_navigationAgent.MoveToPosition(destPos, m_replanInterval) )
			{
				m_bNavRequestCompleted = false;
			}
		}
	}
	#endregion
	
	private Vector3 ChooseRandomDestination()
	{
		// Pick a random grid cell.
		int randomGridCell = Random.Range(0, m_pathGrid.NumberOfCells - 1);
		
		// Choose the center of that grid cell
		Vector3 dest = m_pathGrid.GetCellCenter( randomGridCell );
		
		return dest;
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
