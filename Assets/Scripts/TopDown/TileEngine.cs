using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using OneDescript;
using System;

public class TileEngine : MonoBehaviour {
	private string _filePath;
	public string FilePath {
		get {
			return _filePath;
		}
		set {
			_filePath = value;
		}
	}
	// Use this for initialization
	void Start() {

	}

	// Update is called once per frame
	void Update() {

	}
	void SaveLevel(string filePath, List<GameObject> gameObjects) {
		DescriptorGroup group = new DescriptorGroup();
		int i = 0;
		foreach (GameObject gameObject in gameObjects) {
			group[i]["name"] = new OValue(OValueType.STRING, gameObject.name);
			group[i]["yAxis"] = new OValue(OValueType.FLOAT, gameObject.transform.position.y);
			group[i]["xAxis"] = new OValue(OValueType.FLOAT, gameObject.transform.position.x);
			group[i]["zAxis"] = new OValue(OValueType.FLOAT, gameObject.transform.position.z);
		}
		OneDescript.OneDescriptorSerializer.Serialize(group, File.OpenWrite(filePath));
	}
	List<GameObject> OpenLevel(string filePath) {
		List<GameObject> gameObjects = new List<GameObject>();
		DescriptorGroup group = OneDescript.OneDescriptorSerializer.Deserialize(File.OpenRead(filePath));
		foreach (Descriptor descriptor in group) {
			string name = Convert.ToString(descriptor["name"].GetValue(OValueType.STRING));
			Vector3 position = new Vector3(Convert.ToSingle(descriptor["xAxis"].GetValue(OValueType.FLOAT)),
				Convert.ToSingle(descriptor["yAxis"].GetValue(OValueType.FLOAT)),
				Convert.ToSingle(descriptor["zAxis"].GetValue(OValueType.FLOAT)));
			Instantiate(Resources.Load("TopDown/" + name));
		}
		return gameObjects;
	}
}
