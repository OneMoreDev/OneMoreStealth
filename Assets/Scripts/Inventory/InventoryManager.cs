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
		if (lastEntry > -1) {
			if (Inventory[lastEntry].Type == InventoryItem.ItemType.Paper) {
				if (!string.IsNullOrEmpty(Inventory[lastEntry].Information)) {
					if (GUI.Button(RelativeRect.GetRelative(50, 50, 40, 40), Inventory[lastEntry].Information)) {
						lastEntry = -1;
					}
				}
			}
		}
	}
}
