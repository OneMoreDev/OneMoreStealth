using UnityEngine;
using System.Collections;

public class TopDownPlayerController : MonoBehaviour {
	public float speed = 10f;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		float xMove	= Input.GetAxis("Horizontal");
		float yMove = Input.GetAxis ("Vertical");
		rigidbody2D.velocity = new Vector2 (xMove * speed, yMove * speed);
	}
}
