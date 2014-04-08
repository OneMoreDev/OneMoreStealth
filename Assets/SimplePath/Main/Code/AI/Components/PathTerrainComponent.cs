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

public abstract class PathTerrainComponent : MonoBehaviour
{
	#region Fields
	protected IPathTerrain			m_pathTerrain;
	#endregion
	
	#region Properties
	public IPathTerrain PathTerrain
	{
		get { return m_pathTerrain; }
	}
	#endregion
}
