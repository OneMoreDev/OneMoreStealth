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
public class Interaction_Chase : MonoBehaviour 
{
	#region Unity Editor Fields
	public GameObject						m_chaseObject;
	public float							m_replanInterval = 0.5f;
	#endregion
	
	#region Fields
	private NavigationAgentComponent 		m_navigationAgent;
	private bool							m_bNavRequestCompleted;
	#endregion
	
	#region MonoBehaviour Functions
	void Awake()
	{
		m_bNavRequestCompleted = true;
		m_navigationAgent = GetComponent<NavigationAgentComponent>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if ( m_bNavRequestCompleted )
		{
			if ( m_navigationAgent.MoveToGameObject(m_chaseObject, m_replanInterval) )
			{
				m_bNavRequestCompleted = false;
			}
		}
	}
	#endregion
	
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
