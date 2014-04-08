using UnityEngine;
using System.Linq;
using System.Collections;
using AssemblyCSharp;
using System.Collections.Generic;

public class TopDownPatrollingEnemy : MonoBehaviour {
	public float speed = 10f;
	Vector3 lastLocation;
	Vector3 movementDirection;
	// Use this for initialization
	void Start() {
		movementDirection = GetRandomDirection();
	}

	void Update() {
	}

	// Update is called once per frame
	void FixedUpdate() {
		rigidbody2D.AddForce(movementDirection*speed);
	}

	void OnCollisionEnter2D(Collision2D obj) {
		RepickDirectionFromCollision(obj);
	}
	void OnCollisonStay2D(Collision2D obj) {
		RepickDirectionFromCollision(obj);
	}

	void RepickDirectionFromCollision (Collision2D obj) {
		Vector2 collisionMidpoint = obj.contacts
			.Select(contact => contact.point)
				.Aggregate((accumluator, value) => accumluator + value);
		Vector2 difference = new Vector2(transform.position.x, transform.position.y) - collisionMidpoint;
		difference.Scale(GetRandomDirection()*10f);
		movementDirection = difference.normalized;
	}

	Vector2 GetRandomDirection(){
		float 	randX = Random.Range(0, 1000)/1000f,
				randY = Random.Range(0, 1000)/1000f;
		return new Vector2(randX, randY).normalized;
	}
}
