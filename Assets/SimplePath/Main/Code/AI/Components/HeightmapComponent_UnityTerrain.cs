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

#if (!UNITY_IPHONE && !UNITY_ANDROID)

/// <summary>
/// This component allows you to add a heightmap to your terrain, using a Unity Terrain object. With this, the
/// agents can navigate across uneven terrain. Keep in mind that the Unity Terrain object does not work with Android
/// or iOS. If you want to create your own heightmap for either of those platforms, then you should create your own component
/// similar to this.
/// </summary>
public class HeightmapComponent_UnityTerrain : MonoBehaviour, IHeightmap 
{
	#region Unity Editor Fields
	public Terrain m_terrain;
	#endregion
	
	#region MonoBehaviour Functions
	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
	#endregion
	
	#region IHeightmap Implementation
	public float SampleHeight(Vector3 position)
	{
		return m_terrain.SampleHeight(position);	
	}
	#endregion
}

#endif
