//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34003
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using System;
using UnityEngine;


namespace AssemblyCSharp
{
		public class DirectedBoolean
		{
		public DirectedBoolean ()
		{

		}
		public DirectedBoolean (bool activated, Vector2 force,DirectedBoolean oppositeBool)
		{
			_activated = activated;
			_force = force;
			_oppositeBool = oppositeBool;
		}
		bool _activated;
		Vector2 _force;
		DirectedBoolean _oppositeBool;

		public bool Activated {
			get {
				return _activated;
			}
			set {
				_activated = value;
			}
		}

		public Vector2 Force {
			get {
				return _force;
			}
			set {
				_force = value;
			}
		}

		public DirectedBoolean OppositeBool {
			get {
				return _oppositeBool;
			}
			set {
				_oppositeBool = value;
			}
		}
		}
}

