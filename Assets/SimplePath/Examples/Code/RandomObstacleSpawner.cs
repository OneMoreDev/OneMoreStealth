#region Copyright
// ******************************************************************************************
//
// 							SimplePath, Copyright © 2011, Alex Kring
//
// ******************************************************************************************
#endregion

using UnityEngine;
using System.Collections;

public class RandomObstacleSpawner : MonoBehaviour 
{
	#region Unity Editor Fields
	public PathGridComponent	m_pathGridComponent;
	public GameObject			m_obstacle;
	public float				m_spawnInterval = 2.0f;
	public int					m_spawnCount = 10;
	#endregion
	
	#region Fields
	private float				m_timeBeforeNextSpawn;
	private int					m_numObstaclesSpawned;
	#endregion
	
	#region MonoBehaviour Functions
	void Awake()
	{
		m_timeBeforeNextSpawn = m_spawnInterval;	
		m_numObstaclesSpawned = 0;
	}
	
	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		if ( m_numObstaclesSpawned < m_spawnCount )
		{
			m_timeBeforeNextSpawn -= Time.deltaTime;
			if ( m_timeBeforeNextSpawn <= 0.0f )
			{
				Vector3 spawnPos = ChooseRandomSpawnPosition();
				if ( spawnPos != Vector3.zero )
				{
					GameObject.Instantiate(m_obstacle, spawnPos, Quaternion.identity);
					m_timeBeforeNextSpawn = m_spawnInterval;	
					m_numObstaclesSpawned++;
				}
				else
				{
					m_numObstaclesSpawned = m_spawnCount;
				}
			}
		}
	}
	#endregion
	
	private Vector3 ChooseRandomSpawnPosition()
	{
		PatrolNodeComponent[] patrolNodesArray = (PatrolNodeComponent[])GameObject.FindObjectsOfType(typeof(PatrolNodeComponent));
		PathAgentComponent[] pathAgentsArray = (PathAgentComponent[])GameObject.FindObjectsOfType(typeof(PathAgentComponent));
		
		const int kNumSpawnTries = 100;
		Vector3 spawnPos = Vector3.zero;
		for ( int i = 0; i < kNumSpawnTries; i++ )
		{
			// Pick a random grid cell.
			int randomGridCell = Random.Range(0, m_pathGridComponent.PathGrid.NumberOfCells - 1);
			
			// Don't spawn on a position where another obstacle is located
			if ( m_pathGridComponent.PathGrid.IsBlocked(randomGridCell) )
			{
				continue;
			}
			
			// Make sure we dont't spawn on top of another patrol nodes
			bool bSpawningOnAnotherObject = false;
			for ( int j = 0; j < patrolNodesArray.Length; j++ )
			{
				Vector3 patrolNodePos = patrolNodesArray[j].transform.position;
				int patrolNodeCell = m_pathGridComponent.PathGrid.GetCellIndex(patrolNodePos);
				if ( patrolNodeCell == randomGridCell )
				{
					bSpawningOnAnotherObject = true;
					break;
				}
			}
			
			if ( bSpawningOnAnotherObject )
			{
				continue;	
			}
			
			// Make sure we don't spawn on top of the actor
			for ( int j = 0; j < pathAgentsArray.Length; j++ )
			{
				Vector3 pathAgentPos = pathAgentsArray[j].transform.position;
				int pathAgentCell = m_pathGridComponent.PathGrid.GetCellIndex(pathAgentPos);
				if ( pathAgentCell == randomGridCell )
				{
					bSpawningOnAnotherObject = true;
					break;
				}
			}
			
			if ( bSpawningOnAnotherObject )
			{
				continue;	
			}
			
			// We found a good cell. Choose the center of that grid cell
			spawnPos = m_pathGridComponent.PathGrid.GetCellCenter( randomGridCell );
			break;
		}
		
		return spawnPos;
	}
}
