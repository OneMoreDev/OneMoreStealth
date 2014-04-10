using UnityEngine;
using System.Collections;
using Inventory;

public class InventoryModifier : MonoBehaviour {
	public string Name;
	public string Description;
	public string Information;
	public int ObjectCorolation;
	public float ItemWeight;
	public bool DestructsAfterUse;
	public bool DestroyThisModfier;
	public InventoryItem.ItemType Type;
	public InventoryItem Item;
	// Use this for initialization
	void Start () {
		Item = new InventoryItem(Name, Description, Information, ObjectCorolation, ItemWeight, DestructsAfterUse, Type);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
	
	}
	void OnCollisionEnter2D(Collision2D obj) {
		if (obj.gameObject.layer == 9) {
			InventoryManager inventory = obj.gameObject.GetComponent<InventoryManager>();
			if (inventory != null) {
				inventory.Inventory.Add(Item.ObjectCorolation, Item);
				inventory.lastEntry = Item.ObjectCorolation;
				if (DestroyThisModfier) {
					Destroy(gameObject);
				}
			}
		}
	}
}
