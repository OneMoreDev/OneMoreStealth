using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace OneDescript {
	public class Test : MonoBehaviour {
		void Start() {
			Save();
			Load();
		}

		void Save() {
			DescriptorGroup group = new DescriptorGroup();
			for (int i = 0; i < 10; i++) {
				Descriptor desc = new Descriptor();
				desc["text"] = new OValue(OValueType.STRING, "derp");
				desc["prob"] = new OValue(OValueType.FLOAT, 1.1);
				desc["numb"] = new OValue(OValueType.INT, i);
				List<int> refs = new List<int>();
				for (int j = i; j < 10; j++) {
					refs.Add(j);
				}
				desc["refs"] = new OValue(OValueType.REFLIST, refs);
				group[i] = desc;
			}
			FileStream stream = File.Create("C:/test.omd");
			OneDescriptorSerializer.Serialize(group, stream);
		}

		void Load() {
			FileStream stream = File.OpenRead("C:/test.omd");
			DescriptorGroup group = OneDescriptorSerializer.Deserialize(stream);
			Debug.Log(group[0]["numb"]);
		}
	}
}