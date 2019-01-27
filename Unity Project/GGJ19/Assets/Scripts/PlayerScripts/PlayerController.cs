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
	public int followerOrder = 0;

	private float absoluteHorizontalAxis;
	private float horizontalAxis;

	private bool isGrounded = false;
	public bool isCurrentlyControlled;
	public bool isControllable = true;

	//Sprout
	public bool canDoubleJump = false;
	public bool hasDoubleJumped = false;
	public float doubleJumpStrength = 1.2f;

	//Ray
	public bool canGrow = false;
	public bool hasGrown = false;

	public bool onSpruce = false;

	private Vector2 targetForce;

	public CharacterState parentController;
	public Animator playerAnimator;
	private Rigidbody2D playerRigidbody;
	public AudioSource playerAudio;
	private CircleCollider2D groundCollider;
	private SpriteRenderer playerSprite;

	private List<string> controllerBuffer;
	private int controllerBufferSize = 90;

	private float distanceFromNextCharacter = 0;
	public Transform nextCharacter;
	private bool shouldFollow;

	[SerializeField]
	private Transform topLeft;
	[SerializeField]
	private Transform bottomRight;
	[SerializeField]
	private LayerMask layer;

	public List<AudioClip> walk;
	public AudioClip change;
	public AudioClip hurt1;
	public AudioClip hurt2;
	public AudioClip shrink;
	public AudioClip grow;
	public AudioClip charThud;

	public AudioClip jump;

	private float timeBetweenSteps = 0.2f;
	private float timeSinceLastStep = 0;

	// Use this for initialization
	private void Start()
	{
		playerSprite = this.gameObject.GetComponent<SpriteRenderer>();
		parentController = this.gameObject.GetComponentInParent<CharacterState>();
		groundCollider = this.gameObject.GetComponent<CircleCollider2D>();
		controllerBuffer = new List<string>();
		playerAnimator = this.gameObject.GetComponent<Animator>();
		playerRigidbody = this.gameObject.GetComponent<Rigidbody2D>();
		playerAudio = this.gameObject.GetComponent<AudioSource>();
	}

	private void FixedUpdate()
	{

		isGrounded = Physics2D.OverlapArea(topLeft.position, bottomRight.position, layer);
		horizontalAxis = Input.GetAxisRaw("Horizontal");
		absoluteHorizontalAxis = Mathf.Abs(horizontalAxis);

		TranslatePlayer();

		if (this.gameObject.name == "Sprout" && Vector3.Distance(this.transform.position, GameObject.Find("Spruce").transform.position) > 3f)
		{
			onSpruce = false;
		}
	}

	private void Update()
	{
		CheckDistance();
		GetInputs();
		if (isCurrentlyControlled)
		{
			ProcessInputs();
		}
		else
		{
			//ProcessInputsDelayed();
		}

		if (isGrounded && Mathf.Abs(playerRigidbody.velocity.x) > 0.01f)
		{
			timeSinceLastStep += Time.deltaTime;
			int stepNumber = 0;
			if (timeSinceLastStep > timeBetweenSteps)
			{
				stepNumber++;
				if (stepNumber > 1)
				{
					stepNumber = 0;
				}
				playerAudio.PlayOneShot(walk[stepNumber],0.2f);
				timeSinceLastStep = 0;
			}
		}
	}

	private void GetInputs()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			AddInputToBuffer("jump");
		}
		if (Input.GetKeyDown(KeyCode.LeftControl))
		{
			if (hasGrown)
			{
				UnPlant();
			}
			else
			{
				Plant();
			}
		}
		else if (horizontalAxis > 0)
		{
			AddInputToBuffer("walkRight");
		}
		else if (horizontalAxis < 0)
		{
			AddInputToBuffer("walkLeft");
		}
		else
		{
			AddInputToBuffer("idle");
		}
	}

	private void AddInputToBuffer(string input)
	{
		controllerBuffer.Add(input);
		if (controllerBuffer.Count >= controllerBufferSize)
		{
			controllerBuffer.RemoveAt(0);
		}
	}

	private void ProcessInputs()
	{
		if (controllerBuffer.Count > 1)
		{
			switch (controllerBuffer[controllerBuffer.Count - 2])
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

		if (controllerBuffer.Count >= controllerBufferSize)
		{
			controllerBuffer.RemoveAt(0);
		}
		//CLARE
		//Set the "" to whatever you named the parameter in the Animator. It should be a Float
		playerAnimator.SetFloat("Y Velocity", playerRigidbody.velocity.y);
	}

	private void ProcessInputsDelayed()
	{
		if (controllerBuffer.Count > followDelay + followerOrder)
		{
			if (shouldFollow)
			{
				switch (controllerBuffer[controllerBuffer.Count - followerOrder])
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
			else
			{
				Idle();
			}
		}
		if (controllerBuffer.Count >= controllerBufferSize)
		{
			controllerBuffer.RemoveAt(0);
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
		transform.rotation = Quaternion.Euler(0, direction > 0 ? 180 : 0, 0);
	}

	private void Jump()
	{
		if (isGrounded)
		{
			targetJump = jumpHeight;

			playerAudio.PlayOneShot(jump, 0.5f);
		}
		else
		{
			if (canDoubleJump)
			{
				if (!hasDoubleJumped)
				{
					targetJump = doubleJumpStrength;

					hasDoubleJumped = true;
				}
			}
		}
	}

	private void Plant()
	{
		if (isGrounded && canGrow && isCurrentlyControlled)
		{
			if (!hasGrown)
			{
				targetSpeed = 0;
				isControllable = false;
				groundCollider.enabled = false;
				hasGrown = true;
			}
			playerAudio.PlayOneShot(grow);
			playerAnimator.SetBool("hasGrown", hasGrown);
		}
	}

	private void UnPlant()
	{
		if (canGrow && hasGrown && isCurrentlyControlled)
		{
			isControllable = true;
			playerRigidbody.AddForce(new Vector2(0, 5));
			groundCollider.enabled = true;
		}
		hasGrown = false;

		playerAudio.PlayOneShot(shrink);
		playerAnimator.SetBool("hasGrown", hasGrown);
	}

	public void SaveOneChar()
	{
		if (gameObject.name == "Sprout")
		{
			parentController.sproutSaved = true;
			playerRigidbody.simulated = false;
		}
		else if (gameObject.name == "Ray")
		{
			parentController.raySaved = true;
			playerRigidbody.simulated = false;
		}
		else
		{
			parentController.spruceSaved = true;
			playerRigidbody.simulated = false;
		}

		parentController.CheckWin();
		parentController.GetNextCharacter();
		this.enabled = false;
	}

	private void TranslatePlayer()
	{
		if (isCurrentlyControlled)
		{
			if (isControllable)
			{
				targetForce = new Vector2(targetSpeed, playerRigidbody.velocity.y + targetJump);
				playerRigidbody.velocity = targetForce;
				currentSpeed = Mathf.Abs(playerRigidbody.velocity.x);
			}
		}
		else
		{
			targetSpeed = 0;
			targetForce = new Vector2(targetSpeed, playerRigidbody.velocity.y + targetJump);
			playerRigidbody.velocity = targetForce;
		}
		if (targetJump != -2)
		{
			targetJump = -2;
		}
	}

	private void CheckDistance()
	{
		if (nextCharacter != null)
		{
			distanceFromNextCharacter = Vector2.Distance(this.transform.position, (Vector2)nextCharacter.position);
		}

		if (distanceFromNextCharacter < 3f)
		{
			shouldFollow = false;
		}
		else
		{
			shouldFollow = true;
		}
	}

	public void SwitchBackToSprout()
	{
		if (onSpruce)
		{
			transform.SetParent(parentController.transform);
			playerRigidbody.simulated = true;
		}
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.relativeVelocity.y > 6)
		{
			playerAudio.PlayOneShot(charThud, 0.5f);
			playerAudio.PlayOneShot(hurt2,0.5f);
		}
		if (collision.gameObject.tag == "Ground")
		{
			//CLARE
			//Set the "" to whatever you named the parameter in the Animator. It should be a Bool
			playerAnimator.SetBool("Is Grounded", isGrounded);
			//targetJump = 0;
			hasDoubleJumped = false;
		}

		if (collision.gameObject.name == "Spruce" && !onSpruce)
		{
			if (this.gameObject.name == "Sprout" && !onSpruce)
			{
				transform.SetParent(collision.gameObject.transform);

				Debug.Log("On Spruce");
				playerRigidbody.simulated = false;
				onSpruce = true;
				parentController.GetNextCharacter();
			}
		}
	}

	private void OnCollisionExit2D(Collision2D collision)
	{
		if (collision.gameObject.tag == "Ground")
		{
            //CLARE
            //Set the "" to whatever you named the parameter in the Animator. It should be a Bool
            playerAnimator.SetBool("Is Grounded", isGrounded);
			targetJump = fallSpeed;
		}

		if (collision.gameObject.name == "Spruce")
		{
			if (this.gameObject.name != "Spruce")
			{
				Debug.Log("Off Spruce");
				
			}
		}
	}
}