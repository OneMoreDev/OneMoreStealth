using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using OneDescript;
using System;

public class TileEngine : MonoBehaviour {
	public string FilePath;
	/*public string FilePath {
		get {
			return FilePath;
		}
		set {
			FilePath = value;
		}
	}*/
	// Use this for initialization
	void Start() {

	}

	// Update is called once per frame
	void Update() {

	}
	void SaveLevel(string filePath, List<Tile> tiles) {
		DescriptorGroup group = new DescriptorGroup();
		int i = 0;
		int failed = 0;
		foreach (Tile gameObject in tiles) {
			if (gameObject.name != this.name) {
				Descriptor desc = group[i];
				desc["name"] = new OValue(OValueType.STRING, gameObject.name);
				desc["layer"] = new OValue(OValueType.INT, gameObject.layer);
				desc["yAxis"] = new OValue(OValueType.FLOAT, gameObject.position.y);
				desc["xAxis"] = new OValue(OValueType.FLOAT, gameObject.position.x);
				desc["zAxis"] = new OValue(OValueType.FLOAT, gameObject.position.z);
				desc["rotation"] = new OValue(OValueType.FLOAT, gameObject.rotation);
			}
			i++;
		}
		OneDescript.OneDescriptorSerializer.Serialize(group, File.OpenWrite(filePath));
	}
	List<GameObject> OpenLevel(string filePath) {
		List<GameObject> gameObjects = new List<GameObject>();
		DescriptorGroup group = OneDescript.OneDescriptorSerializer.Deserialize(File.OpenRead(filePath));
		foreach (Descriptor descriptor in group) {
			string name = Convert.ToString(descriptor["name"].GetValue(OValueType.STRING));
			int layer = Convert.ToInt32(descriptor["layer"].GetValue(OValueType.INT));
			float rotation = Convert.ToSingle(descriptor["rotation"].GetValue(OValueType.FLOAT));
			Vector3 position = new Vector3(Convert.ToSingle(descriptor["xAxis"].GetValue(OValueType.FLOAT)),
				Convert.ToSingle(descriptor["yAxis"].GetValue(OValueType.FLOAT)),
				Convert.ToSingle(descriptor["zAxis"].GetValue(OValueType.FLOAT)));
			Instantiate(Resources.Load("TopDown/" + name), position, Quaternion.AngleAxis(rotation, position));
		}
		return gameObjects;
	}
}
