using UnityEngine;
using System.Collections;

public class TopDownPlayerController : MonoBehaviour {
	public float speed = 10f;
	GamePadScript gps;
	// Use this for initialization
	void Start() {
		gps = GameObject.FindObjectOfType<GamePadScript>();
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
		rigidbody2D.velocity = new Vector2(xMove * speed, yMove * speed);
	}
}
