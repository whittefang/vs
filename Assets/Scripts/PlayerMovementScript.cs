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
	float deadSize = .15f;
	bool canMove = true;
	public bool LeftFacingSprites = true;
	int groundedBuffer = 0;
	int groundMask = 1 << 8;
	int onGroundMask = 10;
	int jumpingMask = 9;
	public GameObject otherPlayer;
	public Transform attacksObject;
	public bool OnLeft, canProximityBlock;
	TimeManagerScript timeManager;
	public delegate void vDelegate();
	 vDelegate cancelAttacks;

	// Use this for initialization
	void Awake () {
		timeManager = GameObject.Find ("MasterGameObject").GetComponent<TimeManagerScript> ();
		SR = GetComponent<SpriteRenderer> ();
		state = GetComponent<FighterStateMachineScript>();
		IS = GetComponent<InputScript> ();
		RB = GetComponent<Rigidbody2D> ();
		spriteAnimator = GetComponent<SpriteAnimator> ();
		IS.SetThumbstick (ProcessMovement);
		ResetSpeed ();
		if (tag == "playerOne") {
			otherPlayer = GameObject.FindGameObjectWithTag ("playerTwo");
		} else {
			otherPlayer = GameObject.FindGameObjectWithTag ("playerOne");
		}

		if (OnLeft) {
			if (LeftFacingSprites) {
				SR.flipX = true;
			} else {
				SR.flipX = false;
			}
			attacksObject.eulerAngles = new Vector2(0, 0);
		}else {
			if (LeftFacingSprites) {
				SR.flipX = false;
			} else {
				SR.flipX = true;
			}
			attacksObject.eulerAngles = new Vector2(0, 180);
		}
		spriteAnimator.PlayNeutralAnim ();
	}
	// Update is called once per frame
	void Update () {
	
	}
	public void ProcessMovement(float x,  float y){
		if (canMove && state.GetState () == "neutral" && !timeManager.CheckIfTimePaused()) {
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
					if (OnLeft) {
						spriteAnimator.PlayWalkAnim ();
						RB.velocity = new Vector2 (currentSpeed, RB.velocity.y);
					} else if (!canProximityBlock){
						spriteAnimator.PlayWalkAwayAnim ();
						RB.velocity = new Vector2 (currentSpeed, RB.velocity.y);
					}else {
						spriteAnimator.PlayBlock ();
						RB.velocity = new Vector2 (0, RB.velocity.y);
					}
				} else if (x < 0) {
					
					if (OnLeft && !canProximityBlock) {
						spriteAnimator.PlayWalkAwayAnim ();
						RB.velocity = new Vector2 (-currentSpeed, RB.velocity.y);
					}else if (!OnLeft) {
						spriteAnimator.PlayWalkAnim ();
						RB.velocity = new Vector2 (-currentSpeed, RB.velocity.y);
					}else {
						spriteAnimator.PlayBlock ();
						RB.velocity = new Vector2 (0, RB.velocity.y);
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
			if (LeftFacingSprites) {
				SR.flipX = false;
			} else {
				SR.flipX = true;
			}
			attacksObject.eulerAngles = new Vector2(0, 180);
		} else if (transform.position.x < otherPlayer.transform.position.x && state.GetState() == "neutral" && OnLeft == false){
			OnLeft = true;
			if (LeftFacingSprites) {
				SR.flipX = true;
			} else {
				SR.flipX = false;
			}
			attacksObject.eulerAngles = new Vector2(0, 0);
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
			groundedBuffer = 3;
		}
	}
	void GroundCheck(){
		// do a check for ground
		//.35f
		RaycastHit2D groundCheck = Physics2D.Raycast (transform.position, Vector2.down, groundDistance, groundMask);

		if (groundCheck.collider != null && groundedBuffer <= 0) {

			// landing frames
			cancelAttacks();
			state.SetState("neutral");
			gameObject.layer = onGroundMask;
			CheckFacing ();
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
		if (speedy == 0) {
			speedy = RB.velocity.y;
		}
		if (OnLeft) {
			RB.velocity = new Vector2 (speedx, speedy);
		} else {
			RB.velocity = new Vector2 (-speedx, speedy);
		}
	}
	public bool CheckIfOnLeft(){
		return OnLeft;
	}
	public bool CheckIfBlocking(){
		if (OnLeft && (state.GetState () == "neutral" || state.GetState() == "blockstun") && IS.GetX () < -.25f) {
			return true;
		}else if (!OnLeft && (state.GetState() == "neutral" || state.GetState() == "blockstun") && IS.GetX() > .25f){
			return true;
		}else  {
			return false;
		}
	}
	public void EnableBodyBox(){
		gameObject.layer = onGroundMask;
	}
	public void DsableBodyBox(){
		gameObject.layer = jumpingMask;
	}
	public void setAttackCancel(vDelegate newFunc){
		cancelAttacks = newFunc;
	}
	public void setProximityBlock(bool enable = false){
		canProximityBlock = enable;
	}
	public void EndGame(){
		canMove = false;
	}

}
