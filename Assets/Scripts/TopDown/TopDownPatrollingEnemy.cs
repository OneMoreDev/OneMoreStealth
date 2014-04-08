using UnityEngine;
using System.Linq;
using System.Collections;
using AssemblyCSharp;
using System.Collections.Generic;

public class TopDownPatrollingEnemy : MonoBehaviour {
	public float speed = 10f;
	List<DirectedBoolean> bools = new List<DirectedBoolean>();
	Vector3 lastLocation;
	Vector3 movementDirection;
	// Use this for initialization
	void Start() {
		movementDirection = GetRandomDirection();
	}

	void Update() {
		Debug.DrawRay(transform.position, new Vector3(movementDirection.x, movementDirection.y, 0)*0.3f);
	}

	// Update is called once per frame
	void FixedUpdate() {
		rigidbody2D.AddForce(movementDirection*speed);
		float angle = Mathf.Atan2(movementDirection.y, movementDirection.x)*Mathf.Rad2Deg + 90f;
		transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
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
		difference.Scale(GetRandomDirection());
		movementDirection = difference.normalized;
	}

	Vector2 GetRandomDirection(){
		float 	randX = Random.Range(0, 1000)/1000f,
				randY = Random.Range(0, 1000)/1000f;
		return new Vector2(randX, randY).normalized;
	}
}
