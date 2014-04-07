using UnityEngine;
using System.Collections;

public class Player_Controller : MonoBehaviour 
{
	public float maxSpeed = 10f;
	public float jumpForce = 700f;
	public Transform groundCheck;
	public LayerMask groundLayer;

	bool facingRight = true;
	bool isGrounded = false;
	float groundRadius = 0.2f;
	Animator anim;

	void Start()
	{
		anim = GetComponent<Animator>();
	}

	void Update()
	{
		// JUMPING
		if(isGrounded && Input.GetKeyDown(KeyCode.Space))
		{
			rigidbody2D.AddForce(new Vector2(0, jumpForce));
		}
	}
	
	void FixedUpdate()
	{
		isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, groundLayer);

		float move = Input.GetAxis("Horizontal");
		
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
