using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace MapGeneration.Runner {
	public class Platform{
		public float width, height, offset;

		public bool floating;
		private List<Entity> entities;

		public Platform() {
			entities = new List<Entity>();
		}
	}
}
