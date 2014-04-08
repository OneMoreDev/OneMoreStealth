#region Copyright
// ******************************************************************************************
//
// 							SimplePath, Copyright © 2011, Alex Kring
//
// ******************************************************************************************
#endregion

using UnityEngine;
using System.Collections;
using SimpleAI;
using SimpleAI.Navigation;

public class PathGridComponent : PathTerrainComponent 
{
	#region Unity Editor Fields
	public int 				m_numberOfRows = 10;
	public int 				m_numberOfColumns = 10;
	public float 			m_cellSize = 1;
	public bool 			m_debugShow = true;
	public Color			m_debugColor = Color.white;
	#endregion
	
	#region Properties
	public PathGrid PathGrid
	{
		get { return m_pathTerrain as PathGrid; }
	}
	#endregion
	
	#region MonoBehaviour Functions
	void Awake()
	{
		m_pathTerrain = new PathGrid();
		
#if (!UNITY_IPHONE && !UNITY_ANDROID)
		HeightmapComponent_UnityTerrain heightmapComponent = GetComponent<HeightmapComponent_UnityTerrain>();
		PathGrid.Awake(transform.position, m_numberOfRows, m_numberOfColumns, m_cellSize, m_debugShow, heightmapComponent);
#else
		PathGrid.Awake(transform.position, m_numberOfRows, m_numberOfColumns, m_cellSize, m_debugShow, null);
#endif
	}
	
	void OnDrawGizmos()
	{
		Gizmos.color = m_debugColor;
		
		if ( m_debugShow )
		{
			Grid.DebugDraw(transform.position, m_numberOfRows, m_numberOfColumns, m_cellSize, Gizmos.color);
		}
		
		Gizmos.DrawCube(transform.position, new Vector3(0.25f, 0.25f, 0.25f));
	}
	#endregion
}