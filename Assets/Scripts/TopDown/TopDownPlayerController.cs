using UnityEngine;
using System.Collections;
using Inventory;

public class TopDownPlayerController : MonoBehaviour {
	public float speed = 10f;
	GamePadScript gps;
	InventoryManager inventory;
	// Use this for initialization
	void Start() {
		gps = GameObject.FindObjectOfType<GamePadScript>();
		inventory = GameObject.FindObjectOfType<InventoryManager>();
	}

	// Update is called once per frame
	void FixedUpdate() {
		float xMove = Input.GetAxis("Horizontal");
		float yMove = Input.GetAxis("Vertical");
		if (Application.platform == RuntimePlatform.Android
			|| Application.platform == RuntimePlatform.BB10Player
			|| Application.platform == RuntimePlatform.IPhonePlayer
			|| Application.platform == RuntimePlatform.WP8Player
			|| gps.OverridePlatformRestriction) {
			xMove = gps.XForce;
			yMove = gps.YForce;
		}
		float newSpeed = speed;
		foreach (InventoryItem item in inventory.Inventory.Values) {
			newSpeed -= item.Weight;		
		}
		rigidbody2D.velocity = new Vector2(xMove, yMove) * newSpeed;
	}
}
