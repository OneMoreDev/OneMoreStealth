using UnityEngine;
using System.Collections;

public class TopDownHumanoidAnim : MonoBehaviour {

	Animator anim;
	public GameObject target;
	// Use this for initialization
	void Start() {
		anim = target.GetComponent<Animator>();
	}

	void Update() {
		Debug.DrawRay(transform.position, new Vector3(rigidbody2D.velocity.x, rigidbody2D.velocity.y, 0));
		float angle = Mathf.Atan2(rigidbody2D.velocity.y, rigidbody2D.velocity.x)*Mathf.Rad2Deg + 90f;
		transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
		anim.SetFloat("Movement Speed", rigidbody2D.velocity.magnitude);
	}
}
