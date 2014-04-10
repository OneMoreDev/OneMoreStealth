using UnityEngine;
using System.Linq;
using System.Collections;
using OneDescript;
using System.Collections.Generic;

namespace MapGeneration.Runner {
	public class RunnerMap : MonoBehaviour {
		public Sprite skybox;
		public string levelPath;
		public List<Platform> platforms;
		
		void Load() {
			if (platforms != null && platforms.Count > 0) {
				foreach (Platform platform in platforms) {

				}
			}
			platforms = new List<Platform>();

		}
		
		void Update() {
			
		}
	}

}
