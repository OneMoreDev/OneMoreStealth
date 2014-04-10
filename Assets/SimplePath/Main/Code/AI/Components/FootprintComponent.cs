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

public class FootprintComponent : MonoBehaviour 
{
	#region Unity Editor Fields
	public bool				m_showBoundingBox = true;
	public float			m_scale = 0.0f;
	#endregion
	
	#region Fields
	private int[]			m_obstructedCellPool;
	private int				m_numObstructedCellPoolRows;
	private int				m_numObstructedCellPoolColumns;
	#endregion
	
	void Awake()
	{
		m_numObstructedCellPoolRows = 0;
		m_numObstructedCellPoolColumns = 0;
		m_obstructedCellPool = null;
	}
	
	public int[] GetObstructedCells(Grid grid, out int numObstructedCells)
	{
		numObstructedCells = 0;
		
		if ( collider == null )
		{
			return null;
		}
		
		Bounds bounds = collider.bounds;
		bounds.Expand(m_scale);
		
		// lowerLeftPos Represents the center of the first cell that is covered, in the lower left corner. We march right and up
		// from this cell.
		Vector3 upperLeftPos = new Vector3( bounds.min.x, grid.Origin.y, bounds.max.z );
		Vector3 upperRightPos = new Vector3( bounds.max.x, grid.Origin.y, bounds.max.z );
		Vector3 lowerLeftPos = new Vector3( bounds.min.x, grid.Origin.y, bounds.min.z );
		Vector3 lowerRightPos = new Vector3( bounds.max.x, grid.Origin.y, bounds.min.z );
		Vector3 horizDir = (upperRightPos - upperLeftPos).normalized;
		Vector3 vertDir = (upperLeftPos - lowerLeftPos).normalized;
		float horizLength = bounds.size.x;
		float vertLength = bounds.size.z;
		
		if ( m_showBoundingBox )
		{
			Debug.DrawLine(upperLeftPos, upperRightPos);
			Debug.DrawLine(upperRightPos, lowerRightPos);
			Debug.DrawLine(lowerRightPos, lowerLeftPos);
			Debug.DrawLine(lowerLeftPos, upperLeftPos);
		}
		
		UpdateObstructedCellPool(grid);
		
		// Determine which cells are actually obstructed
		for ( int rowCount = 0; rowCount < m_numObstructedCellPoolRows; rowCount++ )
		{
			float currentVertLength = rowCount * grid.CellSize;
			
			for ( int colCount = 0; colCount < m_numObstructedCellPoolColumns; colCount++ )
			{
				float currentHorizLength = colCount * grid.CellSize;
				Vector3 testPos = lowerLeftPos + horizDir * currentHorizLength + vertDir * currentVertLength;
				testPos.x = Mathf.Clamp(testPos.x, bounds.min.x, bounds.max.x);
				testPos.z = Mathf.Clamp(testPos.z, bounds.min.z, bounds.max.z);
				if ( grid.IsInBounds(testPos) )
				{
					int obstructedCellIndex = grid.GetCellIndex(testPos);
					m_obstructedCellPool[numObstructedCells] = obstructedCellIndex;
					numObstructedCells++;
				}
				
				if ( currentHorizLength > horizLength )
				{
					break;
				}
			}
			
			if ( currentVertLength > vertLength )
			{
				break;
			}
		}
		
		return m_obstructedCellPool;
	}
	
	// Compute the maximum number of obstructed cells, based on the object's collider.
	private void UpdateObstructedCellPool(Grid grid)
	{
		// Create a new cell pool, if the collider shape has changed.
		Bounds bounds = collider.bounds;
		bounds.Expand(m_scale);
		int maxNumObstructedRows = (int)( bounds.size.z / grid.CellSize ) + 2;
		int maxNumObstructedCols = (int)( bounds.size.x / grid.CellSize ) + 2;
		int maxNumObstructedCells = maxNumObstructedRows * maxNumObstructedCols;
		if ( m_obstructedCellPool == null || (maxNumObstructedCells != m_obstructedCellPool.Length) )
		{
			m_obstructedCellPool = new int[maxNumObstructedCells];	
			m_numObstructedCellPoolRows = maxNumObstructedRows;
			m_numObstructedCellPoolColumns = maxNumObstructedCols;
		}
		
		// Clear the contents of the pool
		for ( int i = 0; i < m_obstructedCellPool.Length; i++ )
		{
			m_obstructedCellPool[i] = -1;	
		}
	}
}
