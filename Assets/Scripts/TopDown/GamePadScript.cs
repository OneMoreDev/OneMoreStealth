using UnityEngine;
using System.Collections;

public class GamePadScript : MonoBehaviour {

	public float XForce;
	public float YForce;
	public bool MouseOver = false;
	public bool OverridePlatformRestriction = false;
	void Start() {

	}
	// Update is called once per frame
	void FixedUpdate() {

	}
	void OnMouseOver() {
		MouseOver = true;
	}
	void OnMouseEnter() {
		MouseOver = true;
	}

	// to reset:
	void OnMouseExit() {
		MouseOver = false;
	}
	void OnGUI() {
		if (Application.platform == RuntimePlatform.Android
	|| Application.platform == RuntimePlatform.BB10Player
	|| Application.platform == RuntimePlatform.IPhonePlayer
	|| Application.platform == RuntimePlatform.WP8Player
	|| OverridePlatformRestriction) {
			Rect up = RelativeRect.GetRelative(10, 70, 10, 10);
			Rect upLeft = RelativeRect.GetRelative(0, 70, 5, 10);
			Rect upRight = RelativeRect.GetRelative(30, 70, 5, 10);
			Rect down = RelativeRect.GetRelative(10, 90, 10, 10);
			Rect downLeft = RelativeRect.GetRelative(0, 90, 5, 10);
			Rect downRight = RelativeRect.GetRelative(30, 90, 5, 10);
			Rect left = RelativeRect.GetRelative(0, 80, 10, 10);
			Rect right = RelativeRect.GetRelative(20, 80, 10, 10);
			if (GUI.Button(up, "Up") || up.Contains(Event.current.mousePosition)) {
				YForce = 1;
				XForce = 0;
				MouseOver = true;
			}
			if (GUI.Button(upLeft, "UpLeft") || upLeft.Contains(Event.current.mousePosition)) {
				YForce = 1;
				XForce = -1;
				MouseOver = true;
			}
			if (GUI.Button(upRight, "UpRight") || upRight.Contains(Event.current.mousePosition)) {
				YForce = 1;
				XForce = 1;
				MouseOver = true;
			}
			if (GUI.Button(down, "Down") || down.Contains(Event.current.mousePosition)) {
				YForce = -1;
				XForce = 0;
				MouseOver = true;
			}
			if (GUI.Button(downLeft, "DownLeft") || downLeft.Contains(Event.current.mousePosition)) {
				YForce = -1;
				XForce = -1;
				MouseOver = true;
			}
			if (GUI.Button(downRight, "DownRight") || downRight.Contains(Event.current.mousePosition)) {
				YForce = -1;
				XForce = 1;
				MouseOver = true;
			}
			if (GUI.Button(left, "Left") || left.Contains(Event.current.mousePosition)) {
				XForce = -1;
				YForce = 0;
				MouseOver = true;
			}
			if (GUI.Button(right, "Right") || right.Contains(Event.current.mousePosition)) {
				XForce = 1;
				YForce = 0;
				MouseOver = true;
			}
			if (!MouseOver) {
				XForce = 0;
				YForce = 0;
			}
			MouseOver = false;
		}
	}
}
