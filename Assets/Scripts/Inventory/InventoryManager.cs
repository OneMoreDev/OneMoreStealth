using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Inventory;

public class InventoryManager : MonoBehaviour {

	public Dictionary<int, InventoryItem> Inventory = new Dictionary<int, InventoryItem>();
	public int lastEntry = -1;
	// Use this for initialization
	void Start() {
	}

	// Update is called once per frame
	void FixedUpdate() {

	}
	void OnGUI(){
		int i = 0;
		GUI.skin.button.wordWrap = true;
		foreach (InventoryItem item in Inventory.Values) {
			GUI.skin.button.alignment = TextAnchor.MiddleCenter;
			if (GUI.Button(RelativeRect.GetRelative(i++ * 20, 0, 10, 10), item.Name)) {
				InventoryItem temp = item;
				if (!string.IsNullOrEmpty(temp.Information)) {
					lastEntry = temp.ObjectCorolation;
				}
			}
		} 
		if (lastEntry > -1) {
			if (Inventory[lastEntry].Type == InventoryItem.ItemType.Paper) {
				if (!string.IsNullOrEmpty(Inventory[lastEntry].Information)) {
					GUI.skin.button.alignment = TextAnchor.UpperLeft;
					if (GUI.Button(RelativeRect.GetRelative(50, 10, 70, 70), Inventory[lastEntry].Information)) {
						lastEntry = -1;
					}
				}
			}
		}
	}
}
