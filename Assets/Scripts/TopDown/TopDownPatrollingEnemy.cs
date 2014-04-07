using UnityEngine;
using System.Collections;
using AssemblyCSharp;
using System.Collections.Generic;

public class TopDownPatrollingEnemy : MonoBehaviour {
	float xDirection = 0;
	float yDirection = 0;
	float speed = 2f;
	List<DirectedBoolean> bools = new List<DirectedBoolean>();
	Vector3 lastLocation;
	// Use this for initialization
	void Start() {
		var downBool = new DirectedBoolean();
		var upBool = new DirectedBoolean ();
		var rightBool = new DirectedBoolean();
		var leftBool = new DirectedBoolean ();
		var downLeftBool = new DirectedBoolean();
		var upLeftBool = new DirectedBoolean ();
		var downRightBool = new DirectedBoolean();
		var upRightBool = new DirectedBoolean ();
		//--------
		downBool.Activated = false;
		downBool.Force = new Vector2 (0, speed * 1);
		downBool.OppositeBool = upBool;
		//--------
		upBool.Activated = false;
		upBool.Force = new Vector2 (0, speed * -1);
		upBool.OppositeBool = downBool;
		//--------
		rightBool.Activated = false;
		rightBool.Force = new Vector2 (speed * -1, 0);
		rightBool.OppositeBool = leftBool;
		//--------
		leftBool.Activated = false;
		leftBool.Force = new Vector2 (speed * 1, 0);
		leftBool.OppositeBool = rightBool;
		//--------
		upLeftBool.Activated = false;
		upLeftBool.Force = new Vector2 (speed * -1, speed * -1);
		upLeftBool.OppositeBool = downRightBool;
		//--------
		downRightBool.Activated = false;
		downRightBool.Force = new Vector2 (speed * -1, speed * 1);
		downRightBool.OppositeBool = upLeftBool;
		//--------
		upRightBool.Activated = false;
		upRightBool.Force = new Vector2 (speed * -1, speed * -1);
		upRightBool.OppositeBool = downLeftBool;
		//--------
		downLeftBool.Activated = false;
		downLeftBool.Force = new Vector2 (speed * -1, speed * 1);
		downLeftBool.OppositeBool = upRightBool;
		//--------
		bools.Add (downBool);
		bools.Add (upBool);
		bools.Add (rightBool);
		bools.Add (leftBool);
		bools.Add (downLeftBool);
		bools.Add (upLeftBool);
		bools.Add (downRightBool);
		bools.Add (upRightBool);
		PickDirection ();
	}

	// Update is called once per frame
	void FixedUpdate() {
		foreach (var boolean in bools) {
			if(boolean.Activated){
				rigidbody2D.AddForce(boolean.Force);
			}
			if (transform.position.x == lastLocation.x && transform.position.y == lastLocation.y) {
				boolean.Activated = false;
				PickDirection();		
			}
		}
		lastLocation = transform.position;
	}

	void OnCollisionEnter2D(Collision2D obj) {
		foreach (var boolean in bools) {
			if(boolean.Activated){
				rigidbody2D.AddForce(new Vector2(boolean.OppositeBool.Force.x * 2, boolean.OppositeBool.Force.y * 2));
				boolean.Activated = false;
			}
			if (transform.position.x == lastLocation.x && transform.position.y == lastLocation.y) {
				boolean.Activated = false;
				PickDirection();		
			}
		}
	}
	void PickDirection(){
		int rand = Random.Range (0, bools.Count - 1);
		bools [rand].Activated = true;
		rigidbody2D.AddForce(bools [rand].Force);
	}
}
