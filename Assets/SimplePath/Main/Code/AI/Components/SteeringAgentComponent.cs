#region Copyright
// ******************************************************************************************
//
// 							SimplePath, Copyright © 2011, Alex Kring
//
// ******************************************************************************************
#endregion

using UnityEngine;
using System.Collections;
using SimpleAI.Steering;
using SimpleAI.Navigation;

[RequireComponent(typeof(Rigidbody))]
public class SteeringAgentComponent : MonoBehaviour 
{
	#region Unity Editor Fields
	public float									m_arrivalDistance = 0.25f;
	public float									m_maxSpeed = 2.0f;
	public float									m_lookAheadDistance = 0.50f;
	public float									m_slowingDistance = 1.0f;
	public float									m_accelerationRate = 25.0f;
	public float									m_gravitationalAccelerationRate = 0.0f;
	public Color									m_debugPathColor = Color.yellow;
	public Color									m_debugGoalColor = Color.red;
	public bool										m_debugShowPath = true;
	public bool										m_debugShowVelocity = false;
	#endregion
	
	#region Fields
	private SimpleAI.Steering.PolylinePathway		m_path;
	private bool									m_bArrived;
	private IPathTerrain							m_pathTerrain;
	#endregion
	
	#region MonoBehaviour Functions
	void Awake()
	{
		m_path = null;
		m_bArrived = false;
		m_pathTerrain = null;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if ( m_bArrived )
		{
			gameObject.rigidbody.velocity = Vector3.zero;
		}
		else if ( m_path != null )
		{
			// Compute the seek position (the position we are traveling toward)
	        float currentDistAlongPath = m_path.MapPointToPathDistance(transform.position);
	        float futureDist = currentDistAlongPath + m_lookAheadDistance;
	        Vector3 seekPos = m_path.MapPathDistanceToPoint(futureDist);
			
			// Set the height of the seek pos, based on the terrain.
			seekPos.y = m_pathTerrain.GetTerrainHeight(seekPos);
			
			// Determine the new velocity
			Vector3 newVelocity = Vector3.zero;
			Vector3 destPos = ( m_path.Points[m_path.Points.Length - 1] );
			destPos.y = m_pathTerrain.GetTerrainHeight(destPos);
			Vector3 currentFloorPosition = transform.position;
			currentFloorPosition.y = m_pathTerrain.GetTerrainHeight(currentFloorPosition);
			float distToDestPos = Vector3.Distance(currentFloorPosition, destPos);
			if ( distToDestPos <= m_arrivalDistance )
			{
				if (!m_bArrived)
				{
					// No velocity if we are at our destination
					newVelocity = Vector3.zero;
					OnArrived();
				}
			}
			else
			{
				newVelocity = ComputeArrivalVelocity( seekPos, destPos, currentFloorPosition, rigidbody.velocity );
			}
			
			rigidbody.velocity = newVelocity;
		}
	}
	
	void OnDrawGizmos()
	{
		if ( m_debugShowPath )
		{
			if ( m_path != null )
			{
				// Draw the active path, if it is solved.
				Gizmos.color = m_debugPathColor;
				for (int i = 1; i < m_path.PointCount; i++)
				{
					Vector3 start = m_path.Points[i-1] + Vector3.up * 0.1f;
					Vector3 end = m_path.Points[i] + Vector3.up * 0.1f;
					Gizmos.DrawLine(start, end);
				}
				
				// Draw the goal pos, if there is a path request.
				Gizmos.color = m_debugGoalColor;
				Vector3 goal = m_path.Points[m_path.PointCount - 1] + Vector3.up * 0.1f;
				Gizmos.DrawWireSphere(goal, m_arrivalDistance);
			}
		}
		
		if ( m_debugShowVelocity )
		{
			Gizmos.DrawRay(transform.position, rigidbody.velocity);
		}
	}
	#endregion
	
	public void SteerAlongPath(Vector3[] path, IPathTerrain pathTerrain)
	{
		m_pathTerrain = pathTerrain;
		m_bArrived = false;
		m_path = new PolylinePathway(path.Length, path);
	}
	
	public void StopSteering()
	{
		m_bArrived = false;
		m_path = null;
		rigidbody.velocity = Vector3.zero;
	}
	
	/// <summary>
	/// Called when we arrive at our destination position.
	/// </summary>
	private void OnArrived()
	{
		m_bArrived = true;
		SendMessageUpwards("OnSteeringRequestSucceeded", SendMessageOptions.DontRequireReceiver);
	}
	
	private Vector3 ComputeArrivalVelocity(Vector3 seekPos, Vector3 target, Vector3 position, Vector3 currentVelocity)
	{
		Vector3 targetOffset = target - position;
		float distance = targetOffset.magnitude;
		float rampedSpeed = m_maxSpeed * (distance / m_slowingDistance);
		float minSpeed = m_maxSpeed / 4.0f;
		float clippedSpeed = Mathf.Clamp(rampedSpeed, minSpeed, m_maxSpeed);
		Vector3 accelerationDir = seekPos - position;
		accelerationDir.y = 0.0f;
		accelerationDir.Normalize();
		Vector3 gravitationalForce = -Vector3.up * m_gravitationalAccelerationRate * rigidbody.mass;
		Vector3 acceleration = m_accelerationRate * accelerationDir + gravitationalForce;
		Vector3 newVelocity = currentVelocity + acceleration * Time.deltaTime;
		newVelocity = PolylinePathway.TruncateLength(newVelocity, clippedSpeed);
		return newVelocity;
	}
	
}
