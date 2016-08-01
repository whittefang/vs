using UnityEngine;
using System.Collections;

public class PlayerMovementScript : MonoBehaviour {
	Rigidbody2D RB;
	public float speedMax = 0, jumpForce = 0, jumpDistance = 0;
	float currentSpeed = 0;
	InputScript IS;
	float deadSize = .15f;
	bool canMove = true, grounded = true;
	int groundedBuffer = 0;
	int groundMask = 1 << 8;
	// Use this for initialization
	void Awake () {
		IS = GetComponent<InputScript> ();
		RB = GetComponent<Rigidbody2D> ();
		IS.SetThumbstick (ProcessMovement);
		ResetSpeed ();
	}

	// Update is called once per frame
	void Update () {
	
	}
	public void ProcessMovement(float x,  float y){
		if (canMove) {
			if (x < deadSize && x > -deadSize) {
				x = 0;
			}
			if (y < deadSize && y > -deadSize) {
				y = 0;
			}
			GroundCheck ();
			JumpCheck (x, y);
			if (grounded) {
				RB.velocity = new Vector2 (x * currentSpeed, RB.velocity.y);
			}
		}
	}
	public void SetSpeed(float newSpeed = 0){
		currentSpeed = newSpeed;
	}
	public void ResetSpeed(){
		currentSpeed = speedMax;
	}
	public void JumpCheck(float x, float y){
		if (y > .25f) {
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

			// prejump frames
			RB.AddForce (new Vector2 (0, jumpForce));
			grounded = false;
			groundedBuffer = 3;
		}
	}
	public void TowardJump(){
		if (grounded){

			// prejump frames
			RB.AddForce (new Vector2(jumpDistance, jumpForce));
			grounded = false;
			groundedBuffer = 3;
		}
	}
	public void AwayJump(){
		if (grounded) {
			// prejump frames
			RB.AddForce (new Vector2 (-jumpDistance, jumpForce));
			grounded = false;
			groundedBuffer = 3;
		}
	}
	public void GroundCheck(){
		if (!grounded) {
			// do a check for ground
			//.35f
			Debug.Log("beep");
			RaycastHit2D groundCheck = Physics2D.Raycast (transform.position, Vector2.down, .7f, groundMask);

			if (groundCheck.collider != null && groundedBuffer <= 0) {

				// landing frames

				Debug.Log("biip");
				grounded = true;
			} else if (groundedBuffer > 0) {
				groundedBuffer--;

				Debug.Log("boop");
			}
		}
	}

}
