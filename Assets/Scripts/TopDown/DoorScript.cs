﻿using UnityEngine;
using System.Collections;

public class DoorScript : MonoBehaviour {
	public bool Open = false;
	public bool StayOpen = true;
	public bool Locked = false;
	public int MatchingKeyNumber;
	private bool lockWarning;
	Vector3 originalPosition;
	// Use this for initialization
	void Start () {
		originalPosition = transform.position;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
	}
	void OnCollisionEnter2D(Collision2D obj) {
		if (obj.gameObject.layer == 9) {
			if (!Locked) {
				if (Open == false) {
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
	void OnGui() {
		if (lockWarning) {
			if (GUI.Button(RelativeRect.GetRelative(40, 50, 10, 10), "Its locked!")) {
				lockWarning = false;
			}
		}
	}
}
