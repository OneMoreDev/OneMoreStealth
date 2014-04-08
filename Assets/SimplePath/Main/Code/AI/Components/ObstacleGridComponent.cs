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

[RequireComponent(typeof(PathGridComponent))]
public class ObstacleGridComponent : MonoBehaviour
{
	#region Unity Editor Fields
	public Color			m_obstructedCellColor = Color.red;
	public bool 			m_show = false;
	public bool 			m_rasterizeEveryFrame = true;
	#endregion
	
	#region Fields
	private PathGrid		m_pathGrid;
	#endregion
	
	#region MonoBehaviour Functions
	void Start()
	{
		m_pathGrid = GetComponent<PathGridComponent>().PathGrid;
		
		Rasterize();
	}

	void Update () 
	{
		if ( m_rasterizeEveryFrame )
		{
			Rasterize();
		}
	}
	
	/// <summary>
	/// Rasterize the obstacles into the grid.
	/// </summary>
	public void Rasterize()
	{
		// Clear the path grid from all blockages
		for (int i = 0; i < m_pathGrid.NumberOfCells; i++)
		{
			m_pathGrid.SetSolidity(i, false);
		}
		
		// Render all of the footprints into the path grid as blocked
		FootprintComponent[] footprintArray = (FootprintComponent[])GameObject.FindObjectsOfType(typeof(FootprintComponent));
		foreach (FootprintComponent footprint in footprintArray)
		{	
			int numObstructedCells = 0;
			int[] obstructedCells = footprint.GetObstructedCells(m_pathGrid, out numObstructedCells);
			
			for ( int i = 0; i < numObstructedCells; i++ )
			{
				int obstructedCellIndex = obstructedCells[i];
				if ( m_pathGrid.IsInBounds(obstructedCellIndex) )
				{
					m_pathGrid.SetSolidity(obstructedCellIndex, true);
				}
			}
		}	
	}
	
	void OnDrawGizmos()
	{
		Gizmos.color = m_obstructedCellColor;
		
		if ( m_show && m_pathGrid != null )
		{
			for ( int i = 0; i < m_pathGrid.NumberOfCells; i++ )
			{
				if ( m_pathGrid.IsBlocked(i) )
				{
					Vector3 cellPos = m_pathGrid.GetCellCenter(i);
					Vector3 cellSize3 = new Vector3(m_pathGrid.CellSize, m_pathGrid.CellSize, 0.5f);
					Gizmos.DrawCube(cellPos, cellSize3);
				}
			}
		}
	}
	#endregion
}