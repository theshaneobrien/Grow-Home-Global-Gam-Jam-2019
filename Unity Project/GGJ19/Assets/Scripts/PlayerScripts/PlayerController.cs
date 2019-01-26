using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (SpriteRenderer))]
[RequireComponent (typeof (Rigidbody2D))]
[RequireComponent (typeof (Animator))]
[RequireComponent (typeof (AudioSource))]

public class PlayerController : MonoBehaviour
{

	//Player Move Vars
	[SerializeField]
	private float walkSpeed = 4f;
	[SerializeField]
	private float runSpeed = 7f;
	[SerializeField]
	private float crouchSpeed = 2.5f;
	private float currentSpeed = 0f;
	private float targetSpeed = 0f;
	private float targetJump = 0f;
	[SerializeField]
	private float speedRamp = 2f;
	[SerializeField]
	private float jumpHeight = 3f;

	private float absoluteHorizontalAxis;
	private float horizontalAxis;

	private bool isRunning = false;
	private bool isGrounded = false;
	private bool isCrouching = false;

	private Vector2 targetForce;

	private Animator playerAnimator;
	private Rigidbody2D playerRigidbody;
	private AudioSource playerAudio;


	// Use this for initialization
	private void Start()
	{
		playerAnimator = this.gameObject.GetComponent<Animator>();
		playerRigidbody = this.gameObject.GetComponent<Rigidbody2D>();
		playerAudio = this.gameObject.GetComponent<AudioSource>();
	}

	// Update is called once per frame
	private void FixedUpdate()
	{
		horizontalAxis = Input.GetAxisRaw("Horizontal");
		absoluteHorizontalAxis = Mathf.Abs(horizontalAxis);

		TranslatePlayer();
		GetInputs();
	}

	private void GetInputs()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			Debug.Log("isJumping");
			Jump();
		}

		//If the left or right key is pressed and the shift key is not pressed
		if (absoluteHorizontalAxis >= 1 && !isRunning)
		{
			Walk();
		}
		else
		{
			Idle();
		}
	}

	private void Idle()
	{
		targetSpeed = 0;

		//CLARE
		//Set the "" to whatever you named the parameter in the Animator. It should be a Bool
		playerAnimator.SetFloat("Speed", currentSpeed);
	}

	private void Walk()
	{
		targetSpeed = walkSpeed * (Input.GetAxis("Horizontal"));
		//CLARE
		//Set the "" to whatever you named the parameter in the Animator. It should be a Float
		playerAnimator.SetFloat("Speed", currentSpeed);

		//Flips the character sprite based on the last received input
		transform.rotation = Quaternion.Euler(0, horizontalAxis > 0 ? 180 : 0, 0);
	}

	private void Jump()
	{
		targetJump = jumpHeight;
		//CLARE
		//Set the "" to whatever you named the parameter in the Animator. It should be a Bool
		playerAnimator.SetBool("", true);
	}

	private void TranslatePlayer()
	{
		targetForce = new Vector2(targetSpeed, playerRigidbody.velocity.y + targetJump);
		playerRigidbody.velocity = targetForce;
		currentSpeed = Mathf.Abs(playerRigidbody.velocity.x);
	}
}