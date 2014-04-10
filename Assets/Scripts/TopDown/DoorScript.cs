using UnityEngine;
using System.Collections;

public class DoorScript : KeyBoundGameObject {
	public bool Open = false;
	public bool StayOpen = true;
	public bool Locked = false;
	public int ObjectCorolation;
	private bool lockWarning;
	Vector3 originalPosition;
	// Use this for initialization
	void Start () {
		originalPosition = transform.position;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (lockWarning) {
			if (GUI.Button(RelativeRect.GetRelative(40, 50, 10, 10), "Its locked!")) {
				lockWarning = false;
			}
		}
	}
	void OnCollisionStay2D(Collision2D obj) {
		if (obj.gameObject.layer == 9) {
			if (Input.GetKeyDown(KeyBind)) {
			if (Locked) {
					InventoryManager manager = obj.gameObject.GetComponent<InventoryManager>();
					if (manager.Inventory.ContainsKey(ObjectCorolation)) {
						Locked = false;
						if (manager.Inventory[ObjectCorolation].DestructsAfterUse) {
							manager.Inventory.Remove(ObjectCorolation);
						}
					}
				}
				if (!Locked) {
					if (!Open) {
						Open = true;
						rigidbody2D.fixedAngle = false;
						rigidbody2D.isKinematic = false;
						//rigidbody2D.AddForce(new Vector2(-100,-50));
					}
				} else {
					lockWarning = true;
				}
			}
		}
	}
	void OnGUI() {
		if (lockWarning) {
			if (GUI.Button(RelativeRect.GetRelative(40, 50, 10, 10), "Its locked!")) {
				lockWarning = false;
			}
		}
	}
}
