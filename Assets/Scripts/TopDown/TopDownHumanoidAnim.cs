using UnityEngine;
using System.Collections;

public class TopDownHumanoidAnim : MonoBehaviour {

	Animator anim;
	bool left = false;
	bool right = false;
	bool up = false;
	bool down = false;
	bool diagonalUpLeft = false;
	bool diagonalUpRight = false;
	bool diagonalDownLeft = false;
	bool diagonalDownRight = false;
	Vector3 lastPosition;
	// Use this for initialization
	void Start() {
		anim = GetComponent<Animator>();
		lastPosition = new Vector3 ();
	}

	// Update is called once per frame
	void FixedUpdate() {
		if (transform.position.x < lastPosition.x && !left) {
			left = true;
			right = false;
			up = false;
			down = false;
			diagonalUpLeft = false;
			diagonalUpRight = false;
			diagonalDownLeft = false;
			diagonalDownRight = false;
			transform.rotation = Quaternion.Euler(Vector3.forward * -90);
		}
		if (transform.position.x > lastPosition.x && !right) {
			left = false;
			right = true;
			up = false;
			down = false;
			diagonalUpLeft = false;
			diagonalUpRight = false;
			diagonalDownLeft = false;
			diagonalDownRight = false;
			transform.rotation = Quaternion.Euler(Vector3.forward * -270);
		}
		if (transform.position.y < lastPosition.y && !up) {
			left = false;
			right = false;
			up = true;
			down = false;
			diagonalUpLeft = false;
			diagonalUpRight = false;
			diagonalDownLeft = false;
			diagonalDownRight = false;
			transform.rotation = Quaternion.Euler(Vector3.forward * 0);
		}
		if (transform.position.y > lastPosition.y && !down) {
			left = false;
			right = false;
			up = false;
			down = true;
			diagonalUpLeft = false;
			diagonalUpRight = false;
			diagonalDownLeft = false;
			diagonalDownRight = false;
			transform.rotation = Quaternion.Euler(Vector3.forward * -180);
		}
		if (transform.position.y < lastPosition.y && transform.position.x < lastPosition.x && !diagonalUpLeft) {
			left = false;
			right = false;
			up = false;
			down = false;
			diagonalUpLeft = true;
			diagonalUpRight = false;
			diagonalDownLeft = false;
			diagonalDownRight = false;
			transform.rotation = Quaternion.Euler(Vector3.forward * -30);
		}
		if (transform.position.y < lastPosition.y && transform.position.x > lastPosition.x && !diagonalUpRight) {
			left = false;
			right = false;
			up = false;
			down = false;
			diagonalUpLeft = false;
			diagonalUpRight = true;
			diagonalDownLeft = false;
			diagonalDownRight = false;
			transform.rotation = Quaternion.Euler(Vector3.forward * -300);
		}
		if (transform.position.y > lastPosition.y && transform.position.x < lastPosition.x && !diagonalDownLeft) {
			left = false;
			right = false;
			up = false;
			down = false;
			diagonalUpLeft = false;
			diagonalUpRight = false;
			diagonalDownLeft = true;
			diagonalDownRight = false;
			transform.rotation = Quaternion.Euler(Vector3.forward * -110);
		}
		if (transform.position.y > lastPosition.y && transform.position.x > lastPosition.x && !diagonalDownRight) {
			left = false;
			right = false;
			up = false;
			down = false;
			diagonalUpLeft = false;
			diagonalUpRight = false;
			diagonalDownLeft = false;
			diagonalDownRight = true;
			transform.rotation = Quaternion.Euler(Vector3.forward * -210);
		}
		if (rigidbody2D.velocity.x + rigidbody2D.velocity.y != 0f || (transform.position.x != lastPosition.x && transform.position.y != lastPosition.y)) {
			anim.SetFloat("Speed", 0.02f);
		} else {
			anim.SetFloat("Speed", 0.00f);
		}
		lastPosition = transform.position;
	}
}
