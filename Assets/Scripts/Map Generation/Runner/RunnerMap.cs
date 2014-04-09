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
			DescriptorGroup data = OneDescriptorSerializer.Deserialize(System.IO.File.OpenRead(levelPath));
			Descriptor root = data[0];
			if ((string)root["type"].GetValue() == "runner_map") {
				List<Descriptor> platformDescriptors = data.Dereference((List<int>)root["platforms"].GetValue());
				foreach (Descriptor platform in platformDescriptors) {
					if ((string)platform["type"].GetValue() == "platform") {
						Platform plat = new Platform();
						plat.floating = (int)platform["floating"].GetValue() > 0;
						plat.height = (float)platform["height"].GetValue();
						plat.width = (float)platform["width"].GetValue();
						plat.offset = (float)platform["offset"].GetValue();
						plat.style = (string)platform["style"].GetValue();
						List<Descriptor> entityDescriptors = data.Dereference((List<int>)platform["entities"].GetValue());
						foreach (Descriptor entity in entityDescriptors) {
							if ((string)entity["type"].GetValue() == "entity") {

							}
						}
					}
				}
			} else {
				Debug.LogError("Provided file is not a sidescroller map");
			}
		}
		
		void Update() {
			
		}
	}

}
