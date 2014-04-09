using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace MapGeneration.Runner {
	public class Platform{
		public float width {
			get; set; //TODO: Reflect changes on GameObject
		}
		public float height {
			get; set; //TODO: Reflect changes on GameObject
		}
		public float offset {
			get; set; //TODO: Reflect changes on GameObject
		}
		public bool floating {
			get; set; //TODO: Reflect changes on GameObject
		}
		public string style {
			get; set; //TODO: Reflect changes on GameObject
		}
		private List<Entity> entities;

		public GameObject instance;

		public Platform() {
			entities = new List<Entity>();
		}
	}
}
