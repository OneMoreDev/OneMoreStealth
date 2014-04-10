using UnityEngine;
using System.Collections;

public class Player_Controller : MonoBehaviour 
{
	public float maxSpeed = 10f;
	public float jumpForce = 700f;
	public Transform groundCheck;
	public LayerMask groundLayer;
	GamePadScript gps;
	bool facingRight = true;
	bool isGrounded = false;
	float groundRadius = 0.2f;
	Animator anim;

	void Start()
	{
		anim = GetComponent<Animator>();
		gps = GameObject.FindObjectOfType<GamePadScript>();
	}

	void Update()
	{
		// JUMPING
		if(isGrounded && (Input.GetKeyDown ("space") || gps.YForce == 1))
		{
			rigidbody2D.AddForce(new Vector2(0, jumpForce));
			gps.YForce = 0;
		}
	}
	
	void FixedUpdate()
	{
		isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, groundLayer);

		float move = Input.GetAxis("Horizontal");
		if (Application.platform == RuntimePlatform.Android
			|| Application.platform == RuntimePlatform.BB10Player
			|| Application.platform == RuntimePlatform.IPhonePlayer
			|| Application.platform == RuntimePlatform.WP8Player
			|| gps.OverridePlatformRestriction) {
			move = gps.XForce;		
		}
		rigidbody2D.velocity = new Vector2(move * maxSpeed, rigidbody2D.velocity.y);

		anim.SetFloat("Speed", Mathf.Abs(move));

		if(move > 0 && !facingRight)
			Flip();
		else if(move < 0 && facingRight)
			Flip();
	}

	void Flip()
	{
		facingRight = !facingRight;
		Vector3 scale = transform.localScale;
		scale.x *= -1;
		transform.localScale = scale;

	}
}
