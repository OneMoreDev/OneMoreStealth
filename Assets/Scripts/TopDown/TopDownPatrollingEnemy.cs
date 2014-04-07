using UnityEngine;
using System.Collections;

public class TopDownPatrollingEnemy : MonoBehaviour {
	float xDirection = 0;
	float yDirection = 0;
	float speed = 1f;
	Vector3 lastLocation;
	// Use this for initialization
	void Start() {
		Random.seed = System.DateTime.Now.Millisecond;
		xDirection = Random.Range(-2, 2);
		yDirection = Random.Range(-2, 2);
		lastLocation = new Vector3();
	}

	// Update is called once per frame
	void FixedUpdate() {
		rigidbody2D.AddForce(new Vector2(xDirection * speed, yDirection * speed));
		lastLocation = transform.position;
		if (transform.position.x == lastLocation.x && transform.position.y == lastLocation.y) {
			rigidbody2D.AddForce(new Vector2(xDirection * speed, yDirection * speed));
		}
	}

	void OnCollisionEnter2D(Collision2D obj) {
		Random.seed = System.DateTime.Now.Millisecond;
		if (transform.position.x == lastLocation.x && transform.position.y == lastLocation.y) {
			xDirection *= -1;
			yDirection *= -1;
			rigidbody2D.AddForce(new Vector2(obj.transform.position.x * xDirection * speed * 10, obj.transform.position.y * yDirection * speed * 10));
		} else {
			xDirection = Random.Range(-2, 2);
			yDirection = Random.Range(-2, 2);
			rigidbody2D.AddForce(new Vector2(xDirection * speed, yDirection * speed));
		}
		lastLocation = transform.position;
	}
}
