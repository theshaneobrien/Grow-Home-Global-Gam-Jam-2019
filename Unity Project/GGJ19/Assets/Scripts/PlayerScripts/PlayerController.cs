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
	private float walkSpeed = 6f;
	private float currentSpeed = 0f;
	private float targetSpeed = 0f;
	private float targetJump = 0f;
	[SerializeField]
	private float jumpHeight = 13f;
	[SerializeField]
	private float fallSpeed = -2f;

	private int followDelay = 30;
	private int frameCounter = 0;
	public int followerOrder = 0;

	private float absoluteHorizontalAxis;
	private float horizontalAxis;

	private bool isGrounded = false;
	public bool isCurrentlyControlled;

	private Vector2 targetForce;

	private Animator playerAnimator;
	private Rigidbody2D playerRigidbody;
	private AudioSource playerAudio;

	private List<string> controllerBuffer;


	// Use this for initialization
	private void Start()
	{
		controllerBuffer = new List<string>();
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
	}

	private void Update()
	{
		GetInputs();
		if (isCurrentlyControlled)
		{
			ProcessInputs();
		}
		else
		{
			ProcessInputsDelayed();
		}

		frameCounter++;
	}

	private void GetInputs()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			AddInputToBuffer("jump");
			//Jump();
		}

		//If the left or right key is pressed and the shift key is not pressed
		if (horizontalAxis > 0)
		{
			AddInputToBuffer("walkRight");
			//Walk();
		}
		else if (horizontalAxis < 0)
		{
			AddInputToBuffer("walkLeft");
		}
		else
		{
			AddInputToBuffer("idle");
			//Idle();
		}
	}

	private void AddInputToBuffer(string input)
	{
		controllerBuffer.Add(input);
	}

	private void ProcessInputs()
	{
		if (controllerBuffer.Count > 0)
		{
			switch (controllerBuffer[(int)frameCounter])
			{
				case "walkLeft":
					Walk(-1f);
					break;
				case "walkRight":
					Walk(1f);
					break;
				case "jump":
					Jump();
					break;
				case "idle":
					Idle();
					break;
			}
		}
	}

	private void ProcessInputsDelayed()
	{
		if (controllerBuffer.Count > followDelay + (followerOrder * 3))
		{
			switch (controllerBuffer[frameCounter - (followerOrder*3)])
			{
				case "walkLeft":
					Walk(-1f);
					break;
				case "walkRight":
					Walk(1f);
					break;
				case "jump":
					Jump();
					break;
				case "idle":
					Idle();
					break;
			}
		}
	}

	private void Idle()
	{
		targetSpeed = 0;

		//CLARE
		//Set the "" to whatever you named the parameter in the Animator. It should be a Bool
		playerAnimator.SetFloat("Speed", currentSpeed);
	}

	private void Walk(float direction)
	{
		targetSpeed = walkSpeed * direction;
		//CLARE
		//Set the "" to whatever you named the parameter in the Animator. It should be a Float
		playerAnimator.SetFloat("Speed", currentSpeed);

		//Flips the character sprite based on the last received input
		transform.rotation = Quaternion.Euler(0, horizontalAxis > 0 ? 180 : 0, 0);
	}

	private void Jump()
	{
		if (isGrounded)
		{
			targetJump = jumpHeight;
		}
	}

	private void TranslatePlayer()
	{
		targetForce = new Vector2(targetSpeed, playerRigidbody.velocity.y + targetJump);
		playerRigidbody.velocity = targetForce;
		currentSpeed = Mathf.Abs(playerRigidbody.velocity.x);

		//CLARE
		//Set the "" to whatever you named the parameter in the Animator. It should be a Float
		playerAnimator.SetFloat("Y Velocity", playerRigidbody.velocity.y);
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.tag == "Ground")
		{
			isGrounded = true;

			//CLARE
			//Set the "" to whatever you named the parameter in the Animator. It should be a Bool
			playerAnimator.SetBool("Is Grounded", isGrounded);
			targetJump = 0;
			Debug.Log("Hit Ground");
		}
	}

	private void OnCollisionExit2D(Collision2D collision)
	{
		Debug.Log("Test");
		if (collision.gameObject.tag == "Ground")
		{
            isGrounded = false;

            //CLARE
            //Set the "" to whatever you named the parameter in the Animator. It should be a Bool
            playerAnimator.SetBool("Is Grounded", isGrounded);
			targetJump = fallSpeed;
		}
	}
}