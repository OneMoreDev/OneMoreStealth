#region Copyright
// ******************************************************************************************
//
// 							SimplePath, Copyright © 2011, Alex Kring
//
// ******************************************************************************************
#endregion

using UnityEngine;
using System;
using System.Collections.Generic;
using SimpleAI.Planning;

namespace SimpleAI.Navigation
{
    /// <summary>
    ///These are the objects that can navigate. Inherit from this interface to define your own type
	///of entity that can navigate around the world.
    /// </summary>
    public interface IPathAgent
    {
		/// <summary>
		///Get the position of the path agent's feet (the position where his feet touch the ground).
		/// </summary>
		/// <returns>
		/// A <see cref="Vector3"/>
		/// </returns>
        Vector3 GetPathAgentFootPos();
		
		/// <summary>
		///Called when the agent's path request successfully completes. 
		/// </summary>
		/// <param name="request">
		/// A <see cref="IPathRequestQuery"/>
		/// </param>
		void OnPathAgentRequestSucceeded(IPathRequestQuery request);
		
		/// <summary>
		///Called when the agent's path request fails to complete.
		/// </summary>
		void OnPathAgentRequestFailed();
    }
}
