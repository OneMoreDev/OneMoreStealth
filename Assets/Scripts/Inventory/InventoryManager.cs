using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Inventory;

public class InventoryManager : MonoBehaviour {

	public Dictionary<int,InventoryItem> Inventory = new Dictionary<int,InventoryItem>();
	// Use this for initialization
	void Start () {
		InventoryItem item = new InventoryItem("key", "This key opens up a locked door", "", 1, false, InventoryItem.ItemType.Key);
		Inventory.Add(item.ObjectCorolation, item);
		}
	
	// Update is called once per frame
	void Update () {
	
	}
}
