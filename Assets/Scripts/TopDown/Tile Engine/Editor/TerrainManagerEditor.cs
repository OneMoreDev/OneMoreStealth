using UnityEditor;
using UnityEngine;
using System.Collections;
using System;

namespace Map.TopDown {
	
	[ExecuteInEditMode]
	[CustomEditor(typeof(TerrainManager))]
	public class TerrainManagerEditor : Editor {
		Action 	onPaintButtonPressed,
				onFillButtonPressed,
				onEntityButtonPressed;
		public override void OnInspectorGUI() {
			DrawFilePath();
			DrawToolbar();
		}

		public void OnSceneGUI() {
			HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));
		}

		void DrawFilePath() {
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Level path");
			GUI.enabled = false;
			EditorGUILayout.TextField((target as TerrainManager).mapFile);
			GUI.enabled = true;
			EditorGUILayout.EndHorizontal();
		}
		
		void DrawToolbar() {
			EditorGUILayout.BeginHorizontal();
			if(GUILayout.Button("Paint", EditorStyles.miniButtonLeft)) onPaintButtonPressed.Invoke();
			if(GUILayout.Button("Fill", EditorStyles.miniButtonMid)) onFillButtonPressed.Invoke();
			if(GUILayout.Button("Entity", EditorStyles.miniButtonRight)) onEntityButtonPressed.Invoke();
			EditorGUILayout.EndHorizontal();
		}
	}
}