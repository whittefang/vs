using UnityEngine;
using System.Collections;

public class PlayerMovementScript : MonoBehaviour {
	Rigidbody2D RB;
	public float speedMax = 0, jumpForce = 0, jumpDistance = 0, groundDistance = 0;
	float currentSpeed = 0;
	InputScript IS;
	SpriteAnimator spriteAnimator;
	SpriteRenderer SR;
	FighterStateMachineScript state;
	BoxCollider2D bodyBox;
	float deadSize = .15f;
	bool canMove = true, grounded = true;
	int groundedBuffer = 0;
	int groundMask = 1 << 8;
	int onGroundMask = 10;
	int jumpingMask = 9;
	public GameObject otherPlayer;
	public bool OnLeft;
	// Use this for initialization
	void Awake () {
		SR = GetComponent<SpriteRenderer> ();
		state = GetComponent<FighterStateMachineScript>();
		IS = GetComponent<InputScript> ();
		RB = GetComponent<Rigidbody2D> ();
		spriteAnimator = GetComponent<SpriteAnimator> ();
		bodyBox = GetComponent<BoxCollider2D> ();
		IS.SetThumbstick (ProcessMovement);
		ResetSpeed ();
		if (tag == "playerOne") {
			otherPlayer = GameObject.FindGameObjectWithTag ("playerTwo");
		} else {
			otherPlayer = GameObject.FindGameObjectWithTag ("playerOne");
		}
	}

	// Update is called once per frame
	void Update () {
	
	}
	public void ProcessMovement(float x,  float y){
		if (canMove && state.GetState () == "neutral") {
			if (x < deadSize && x > -deadSize) {
				x = 0;
			}
			if (y < deadSize && y > -deadSize) {
				y = 0;
			}
			CheckFacing ();
			JumpCheck (x, y);
			if (state.GetState () == "neutral") {
				if (x > 0) {
					RB.velocity = new Vector2 (currentSpeed, RB.velocity.y);
					if (OnLeft) {
						spriteAnimator.PlayWalkAnim ();
					} else {
						spriteAnimator.PlayWalkAwayAnim ();
					}
				} else if (x < 0) {
					RB.velocity = new Vector2 (-currentSpeed, RB.velocity.y);
					if (OnLeft) {
						spriteAnimator.PlayWalkAwayAnim ();
					}else  {
						spriteAnimator.PlayWalkAnim ();
					}
				} else if (x == 0) {
					RB.velocity = new Vector2 (0, RB.velocity.y);
					spriteAnimator.PlayNeutralAnim ();
				}
			}
		} else if (state.GetState () == "jumping" || state.GetState () == "jump attack" ||  state.GetState() == "falling hit") {
			
			GroundCheck ();
		}
	}
	public void SetSpeed(float newSpeed = 0){
		currentSpeed = newSpeed;
	}
	public void ResetSpeed(){
		currentSpeed = speedMax;
	}

	public void CheckFacing(){
		
		if (transform.position.x > otherPlayer.transform.position.x && state.GetState() == "neutral" && OnLeft == true) {
			OnLeft = false;
			SR.flipX = false;
		} else if (transform.position.x < otherPlayer.transform.position.x && state.GetState() == "neutral" && OnLeft == false){
			OnLeft = true;
			SR.flipX = true;
		}
	}
	public void JumpCheck(float x, float y){
		if (y > .4f) {
			if (x > .25f) {
				// forward
				TowardJump();

			} else if (x < -.25f) {
				// back
				AwayJump();
			} else {
				// neutral
				NeutralJump();
			} 
		}
	}
	public void NeutralJump(){
		if (state.GetState() == "neutral") {
			state.SetState ("jumping");
			gameObject.layer = jumpingMask;
			// prejump frames
			spriteAnimator.PlayJumpNeutral();
			RB.velocity = Vector2.zero;
			RB.AddForce (new Vector2 (0, jumpForce));
			grounded = false;
			groundedBuffer = 3;
		}
	}
	public void TowardJump(){
		if (state.GetState() == "neutral"){
			state.SetState ("jumping");
			gameObject.layer = jumpingMask;
			// prejump frames
			if (OnLeft) {
				spriteAnimator.PlayJumpToward ();
			}else{
				spriteAnimator.PlayJumpAway ();
			}
			RB.velocity = Vector2.zero;
			RB.AddForce (new Vector2(jumpDistance, jumpForce));
			grounded = false;
			groundedBuffer = 3;
		}
	}
	public void AwayJump(){
		if (state.GetState() == "neutral") {
			state.SetState ("jumping");
			gameObject.layer = jumpingMask;
			// prejump frames
			if (OnLeft) {
				spriteAnimator.PlayJumpAway();
			}else{
				spriteAnimator.PlayJumpToward ();
			}
			RB.velocity = Vector2.zero;
			RB.AddForce (new Vector2 (-jumpDistance, jumpForce));
			grounded = false;
			groundedBuffer = 3;
		}
	}
	void GroundCheck(){
		// do a check for ground
		//.35f
		RaycastHit2D groundCheck = Physics2D.Raycast (transform.position, Vector2.down, groundDistance, groundMask);

		if (groundCheck.collider != null && groundedBuffer <= 0) {

			// landing frames
			state.SetState("neutral");
			gameObject.layer = onGroundMask;
			grounded = true;
		} else if (groundedBuffer > 0) {
			groundedBuffer--;

		}
	}
	public bool ForceGroundCheck(){
		RaycastHit2D groundCheck = Physics2D.Raycast (transform.position, Vector2.down, groundDistance, groundMask);

		if (groundCheck.collider != null) {
			return true;
		} else {
			return false;
		}
	}

	public void StopMovement(){
		RB.velocity = Vector2.zero;
	}
	public void MoveToward(float speedx, float speedy = 0){
		if (OnLeft) {
			RB.velocity = new Vector2 (speedx, speedy);
		} else {
			RB.velocity = new Vector2 (-speedx, speedy);
		}
	}
	public bool CheckIfOnLeft(){
		return OnLeft;
	}
	public void EnableBodyBox(){
		gameObject.layer = onGroundMask;
	}

}
