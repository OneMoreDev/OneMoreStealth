using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using System;

namespace OneDescript {
	[CustomEditor(typeof(DescriptorComponent))]
	public class DescriptorEditor : Editor {
		private GUIContent[] oValueTypes = new GUIContent[] {
			new GUIContent("Integer"),
			new GUIContent("Floating Point"),
			new GUIContent("Text (String)"),
			new GUIContent("Reference List"),
		};
		private bool willAddObject;
		private int objectToDelete = -1;
		public override void OnInspectorGUI() {
			DescriptorComponent component = (DescriptorComponent)target;
			DescriptorGroup group = component.group;

			if (willAddObject) { //Since we can't add to a collection while iterating it, we defer to before the next iter
				willAddObject = false;
				int id = 0;
				foreach (Descriptor desc in group) {
					if (desc.ID >= id) {
						id = desc.ID+1;
					}
				}
				group[id] = new Descriptor();
			}
			if (objectToDelete >= 0) {
				group.RemoveAt(objectToDelete);
				objectToDelete = -1;
			}

			if (group.Count > 0) {
				foreach (Descriptor descriptor in group) {
					EditorGUILayout.BeginVertical();
					EditorGUILayout.BeginHorizontal();
					EditorGUILayout.LabelField("ID");
					descriptor.ID = EditorGUILayout.IntField(descriptor.ID, EditorStyles.numberField);
					if (GUILayout.Button("x", EditorStyles.miniButtonRight, GUILayout.Width(20))) {
						objectToDelete = descriptor.ID;
					}
					EditorGUILayout.EndHorizontal();
					EditorGUILayout.BeginHorizontal();
					EditorGUILayout.LabelField("Properties");
					if (GUILayout.Button("Add", EditorStyles.miniButton)) {
						descriptor["newproperty"] = new OValue(OValueType.STRING, "");
					}
					EditorGUILayout.EndHorizontal();
					List<KeyValuePair<string, OValue>> keyValuePairs = new List<KeyValuePair<string, OValue>>(descriptor);
					foreach (KeyValuePair<string, OValue> kvp in keyValuePairs) {
						EditorGUILayout.BeginHorizontal();
						string key = EditorGUILayout.TextField(kvp.Key, EditorStyles.miniTextField);
						OValueType type = OValueType.FromByte((byte)EditorGUILayout.Popup((int)kvp.Value.type.typeByte, oValueTypes));
						object value = null;
						if (type.Equals(kvp.Value.type)) {
							value = kvp.Value.GetValue(kvp.Value.type);
						} else {
							value = DefaultValue(type);
						}
						if (type.Equals(OValueType.INT)) {
							value = EditorGUILayout.IntField((int)value);
						} else if (type.Equals(OValueType.FLOAT)) {
							value = EditorGUILayout.FloatField((float)value);
						} else if (type.Equals(OValueType.STRING)) {
							value = EditorGUILayout.TextField((string)value);
						} else if (type.Equals(OValueType.REFLIST)) {
							EditorGUILayout.BeginVertical();
							List<int> references = (List<int>)value;
							for (int i = 0; i < references.Count; i++) {
								EditorGUILayout.BeginHorizontal();
								references[i] = EditorGUILayout.IntField(references[i], EditorStyles.miniTextField);
								if (GUILayout.Button("-", EditorStyles.miniButtonRight)) {
									references.RemoveAt(i);
								}
								EditorGUILayout.EndHorizontal();
							}
							if (GUILayout.Button("Add Reference", EditorStyles.miniButton)) {
								references.Add(0);
							}
							EditorGUILayout.EndVertical();
						}
						descriptor.Remove(kvp.Key);
						descriptor[key] = new OValue(type, value);
						EditorGUILayout.EndHorizontal();
					}
					EditorGUILayout.EndVertical();
					EditorGUILayout.Separator();
				}
			}
			if (GUILayout.Button("Add Object")) {
				willAddObject = true;
			}
		}

		object DefaultValue(OValueType type) {
			if (type.Equals(OValueType.INT)) {
				return 0;
			}
			if (type.Equals(OValueType.FLOAT)) {
				return 0.0f;
			}
			if (type.Equals(OValueType.STRING)) {
				return "";
			}
			if (type.Equals(OValueType.REFLIST)) {
				return new List<int>();
			}
			return null;
		}
	}
}