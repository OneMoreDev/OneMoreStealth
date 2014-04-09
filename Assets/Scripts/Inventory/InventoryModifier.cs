using UnityEngine;
using System.Collections;
using Inventory;

public class InventoryModifier : MonoBehaviour {
	public string Name;
	public string Description;
	public string Information;
	public int ObjectCorolation;
	public bool DestructsAfterUse;
	public InventoryItem.ItemType Type;
	public InventoryItem Item;
	// Use this for initialization
	void Start () {
		Item = new InventoryItem(Name, Description, Information, ObjectCorolation, DestructsAfterUse, Type);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void OnCollisionEnter2D(Collision2D obj) {
		if (obj.gameObject.layer == 9) {
			InventoryManager inventory = obj.gameObject.GetComponent<InventoryManager>();
			if (inventory != null) {
				inventory.Inventory.Add(Item.ObjectCorolation, Item);
			}
		}
	}
}
