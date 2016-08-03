using UnityEngine;
using System.Collections;

public class PlayerMovementScript : MonoBehaviour {
	Rigidbody2D RB;
	public float speedMax = 0, jumpForce = 0, jumpDistance = 0, groundDistance = 0;
	float currentSpeed = 0;
	InputScript IS;
	SpriteAnimator spriteAnimator;
	FighterStateMachineScript state;
	float deadSize = .15f;
	bool canMove = true, grounded = true;
	int groundedBuffer = 0;
	int groundMask = 1 << 8;
	// Use this for initialization
	void Awake () {
		state = GetComponent<FighterStateMachineScript>();
		IS = GetComponent<InputScript> ();
		RB = GetComponent<Rigidbody2D> ();
		spriteAnimator = GetComponent<SpriteAnimator> ();
		IS.SetThumbstick (ProcessMovement);
		ResetSpeed ();
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
			JumpCheck (x, y);
			if (grounded) {
				
				if (x > 0) {
					RB.velocity = new Vector2 (currentSpeed, RB.velocity.y);
					spriteAnimator.PlayWalkAnim ();
				} else if (x < 0) {
					RB.velocity = new Vector2 (-currentSpeed, RB.velocity.y);
					spriteAnimator.PlayWalkAwayAnim ();
				} else if (x == 0) {
					RB.velocity = new Vector2 (0, RB.velocity.y);
					spriteAnimator.PlayNeutralAnim ();
				}
			}
		} else if (state.GetState () == "jumping" || state.GetState () == "jump attack") {
			GroundCheck ();
		}
	}
	public void SetSpeed(float newSpeed = 0){
		currentSpeed = newSpeed;
	}
	public void ResetSpeed(){
		currentSpeed = speedMax;
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
		if (grounded) {
			state.SetState ("jumping");
			RB.velocity = Vector2.zero;

			// prejump frames
			spriteAnimator.PlayJumpNeutral();
			RB.AddForce (new Vector2 (0, jumpForce));
			grounded = false;
			groundedBuffer = 3;
		}
	}
	public void TowardJump(){
		if (grounded){
			state.SetState ("jumping");
			RB.velocity = Vector2.zero;
			// prejump frames
			spriteAnimator.PlayJumpToward();
			RB.AddForce (new Vector2(jumpDistance, jumpForce));
			grounded = false;
			groundedBuffer = 3;
		}
	}
	public void AwayJump(){
		if (grounded) {
			state.SetState ("jumping");
			RB.velocity = Vector2.zero;
			// prejump frames
			spriteAnimator.PlayJumpAway();
			RB.AddForce (new Vector2 (-jumpDistance, jumpForce));
			grounded = false;
			groundedBuffer = 3;
		}
	}
	public void GroundCheck(){
		if (!grounded) {
			// do a check for ground
			//.35f
			RaycastHit2D groundCheck = Physics2D.Raycast (transform.position, Vector2.down, groundDistance, groundMask);

			if (groundCheck.collider != null && groundedBuffer <= 0) {

				// landing frames
				state.SetState("neutral");
				grounded = true;
			} else if (groundedBuffer > 0) {
				groundedBuffer--;

			}
		}
	}
	public void StopMovement(){
		RB.velocity = Vector2.zero;
	}
	public void MoveToward(float speedx, float speedy = 0){
		RB.velocity = new Vector2 (speedx, speedy);
	}

}
