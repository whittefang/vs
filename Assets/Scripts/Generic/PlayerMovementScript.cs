using UnityEngine;
using System.Collections;

public class PlayerMovementScript : MonoBehaviour {
	Rigidbody2D RB;
	public float speedMax = 0, jumpForce = 0, jumpDistance = 0, groundDistance = 0;
	float currentSpeed = 0;
	InputScript IS;
	SpriteAnimator spriteAnimator;
	public SpriteRenderer SR;
	FighterStateMachineScript state;
	float deadSize = .25f;
	bool canMove = true, allowMoveTowards = true;
	public bool LeftFacingSprites = true;
	int groundedBuffer = 0;
	int groundMask = 1 << 8;
	int onGroundMask = 10;
	int jumpingMask = 9;
	public int landingRecoveryFrames = 0;
	public GameObject otherPlayer;
	public Transform attacksObject, hurtboxes;
	public bool OnLeft, canProximityBlock, jumpAway = false, useChildSpriteRenderer = false;
	public Vector2 moveboxOffset, moveboxSize;
	public GameObject jumpEffect;
	public float groundY = -3.2f;
	TimeManagerScript timeManager;
	public delegate void vDelegate();
	 vDelegate cancelAttacks;
	BoxCollider2D movementBox;
	bool gettingUp = false;
	public bool forceCollisionOff = false;
	public Vector3 groundSpritePosition, airSpritePosition;
	public GameObject optionalDog;
	// Use this for initialization
	void Awake () {
		movementBox = GetComponent<BoxCollider2D> ();
		timeManager = GameObject.Find ("MasterGameObject").GetComponent<TimeManagerScript> ();
		if (!useChildSpriteRenderer) {
			SR = GetComponent<SpriteRenderer> ();
		}
		state = GetComponent<FighterStateMachineScript>();
		IS = GetComponent<InputScript> ();
		RB = GetComponent<Rigidbody2D> ();
		spriteAnimator = GetComponent<SpriteAnimator> ();
		IS.SetThumbstick (ProcessMovement);
		ResetSpeed ();


		//CheckFacing ();
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
	void FixedUpdate () {
		
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
				if (x > deadSize) {
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
				} else if (x < -deadSize) {
					
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
				} else {
					RB.velocity = new Vector2 (0, RB.velocity.y);
					spriteAnimator.PlayNeutralAnim ();
				}
				gameObject.layer = onGroundMask;
			}
		} 
		if (!forceCollisionOff) {

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
		
		if (transform.position.x > otherPlayer.transform.position.x && (state.GetState() == "neutral" || state.GetState() == "hitstun") && OnLeft == true) {
			OnLeft = false;
			if (LeftFacingSprites) {
				SR.flipX = false;
				if (useChildSpriteRenderer) {
					SR.transform.localPosition = new Vector3 (1, 0, 0);
				}
			} else {
				SR.flipX = true;
				if (useChildSpriteRenderer) {
					SR.transform.localPosition = new Vector3 (-1, 0, 0);
				}
			}
			attacksObject.eulerAngles = new Vector2(0, 180);
			hurtboxes.eulerAngles = new Vector2(0, 180);
		} else if (transform.position.x < otherPlayer.transform.position.x && (state.GetState() == "neutral"|| state.GetState() == "hitstun") && OnLeft == false){
			OnLeft = true;
			if (LeftFacingSprites) {
				SR.flipX = true;
				if (useChildSpriteRenderer) {
					SR.transform.localPosition = new Vector3 (-1, 0, 0);
				}
			} else {
				SR.flipX = false;
				if (useChildSpriteRenderer) {
					SR.transform.localPosition = new Vector3 (1, 0, 0);
				}
			}
			attacksObject.eulerAngles = new Vector2(0, 0);
			hurtboxes.eulerAngles = new Vector2(0, 0);
		}
	}
	public void ForceFlip(){

		if (transform.position.x > otherPlayer.transform.position.x && OnLeft == true) {
			OnLeft = false;
			if (LeftFacingSprites) {
				SR.flipX = false;
			} else {
				SR.flipX = true;
			}
			attacksObject.eulerAngles = new Vector2 (0, 180);
			hurtboxes.eulerAngles = new Vector2 (0, 180);
		} else if (transform.position.x < otherPlayer.transform.position.x && OnLeft == false) {
			OnLeft = true;
			if (LeftFacingSprites) {
				SR.flipX = true;
			} else {
				SR.flipX = false;
			}
			attacksObject.eulerAngles = new Vector2 (0, 0);
			hurtboxes.eulerAngles = new Vector2 (0, 0);
		}
	}
	public void JumpCheck(float x, float y){
		if (y > .4f && state.GetState() == "neutral") {
			StartCoroutine (PreJump (x));
		}
	}
	IEnumerator PreJump(float x){
		state.SetState ("prejump");
		for (int i = 0; i < 4;) {
			yield return null;
			if (!timeManager.CheckIfTimePaused ()) {
				i++;
			}
		}
		if (state.GetState () == "prejump") {

			jumpEffect.transform.position = new Vector3 (transform.position.x, groundY, 1);
			jumpEffect.SetActive (true);
			if (x > .25f) {
				// forward
				jumpAway = false;
				TowardJump ();

			} else if (x < -.25f) {
				// back
				AwayJump ();
			} else {
				// neutral
				jumpAway = false;
				NeutralJump ();
			}
		}
	}
	public void NeutralJump(){
			state.SetState ("jumping");
		forceCollisionOff = true;
		if (useChildSpriteRenderer) {
			if (OnLeft) {
				SR.transform.localPosition = airSpritePosition;
			} else {
				SR.transform.localPosition = new Vector3 (-airSpritePosition.x, 0, 0);
			}
		}
			gameObject.layer = jumpingMask;
			// prejump frames
			spriteAnimator.PlayJumpNeutral();
			RB.velocity = Vector2.zero;
			RB.AddForce (new Vector2 (0, jumpForce));
		groundedBuffer = 3;
		forceCollisionOff = false;

	}
	public void TowardJump(){
		state.SetState ("jumping");
		forceCollisionOff = true;
		if (useChildSpriteRenderer) {
			if (OnLeft) {
				SR.transform.localPosition = airSpritePosition;
			} else {
				SR.transform.localPosition = new Vector3 (-airSpritePosition.x, 0, 0);
			}
		}
			gameObject.layer = jumpingMask;
			// prejump frames
			if (OnLeft) {

				jumpAway = false;
				spriteAnimator.PlayJumpToward ();
			}else{

				jumpAway = true;
				spriteAnimator.PlayJumpAway ();
			}
			RB.velocity = Vector2.zero;
			RB.AddForce (new Vector2(jumpDistance, jumpForce));
		groundedBuffer = 3;

		Invoke ("TurnForceBackOff", .1f);
	}
	void TurnForceBackOff(){
		forceCollisionOff = false;
	}
	public void AwayJump(){
		state.SetState ("jumping");
		forceCollisionOff = true;
		if (useChildSpriteRenderer) {
			if (OnLeft) {
				SR.transform.localPosition = airSpritePosition;
			} else {
				SR.transform.localPosition = new Vector3 (-airSpritePosition.x, 0, 0);
			}
		}
			gameObject.layer = jumpingMask;
			// prejump frames
			if (OnLeft) {
				jumpAway = true;

				spriteAnimator.PlayJumpAway();
			}else{

				jumpAway = false;
				spriteAnimator.PlayJumpToward ();
			}
			RB.velocity = Vector2.zero;
			RB.AddForce (new Vector2 (-jumpDistance, jumpForce));
		groundedBuffer = 3;
		Invoke ("TurnForceBackOff", .1f);

	}
	void GroundCheck(){
		// do a check for ground
		//.35f
		RaycastHit2D groundCheck = Physics2D.Raycast (transform.position, Vector2.down, groundDistance, groundMask);
		if (groundCheck.collider != null) {
			gameObject.layer = onGroundMask;
		} else {
			gameObject.layer = jumpingMask;
		}

		if (state.GetState () == "jumping" || state.GetState () == "jump attack" ||  state.GetState() == "falling hit") {
			if (groundCheck.collider != null && groundedBuffer <= 0 && !timeManager.CheckIfTimePaused()) {

				// landing frames
				cancelAttacks();
				if (landingRecoveryFrames > 0) {
					StopMovement ();
					spriteAnimator.PlayLanding ();
					landingRecoveryFrames--;
				} else if (state.GetState () == "falling hit" && !gettingUp) {
					state.SetState ("invincible");
					StopAllCoroutines ();
					RB.gravityScale = 7;
					if (useChildSpriteRenderer) {
						if (OnLeft) {
							SR.transform.localPosition = groundSpritePosition;
						} else {
							SR.transform.localPosition = new Vector3 (-groundSpritePosition.x, 0, 0);
						}
					}
					StartCoroutine (GetUpAfterKnockdown ());
				}else if (!gettingUp){

					if (useChildSpriteRenderer) {
						if (OnLeft) {
							SR.transform.localPosition = groundSpritePosition;
						} else {
							SR.transform.localPosition = new Vector3 (-groundSpritePosition.x, 0, 0);
						}
					}
					state.SetState ("neutral");
				}
				movementBox.offset = moveboxOffset;
				movementBox.size = moveboxSize;
				gameObject.layer = onGroundMask;
				CheckFacing ();
			} else if (groundedBuffer > 0  && !timeManager.CheckIfTimePaused()) {
				groundedBuffer--;

			}
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

	public void TurnOffGravity(bool enabled){
		if (enabled) {
			RB.gravityScale = 0;
		} else {
			RB.gravityScale = 7;
		}
	}

	public void StopMovement(){
		RB.velocity = Vector2.zero;
	}
	public void MoveToward(float speedx, float speedy = 0){
		if (allowMoveTowards) {

			if (speedy == 0) {
				speedy = RB.velocity.y;
			}
			StartCoroutine(MoveTowardsEnum(speedx, speedy));
		}
	}
	IEnumerator MoveTowardsEnum(float speedx, float speedy){
		while (timeManager.CheckIfTimePaused()) {
			yield return null;
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
	public void EnableCollider(bool enabled){
		movementBox.isTrigger = enabled;
		if (enabled) {
			RB.gravityScale = 0;	
		} else {
			RB.gravityScale = 7;
		}
	}

	public void setAttackCancel(vDelegate newFunc){
		Debug.Log ("setcancel");
		cancelAttacks = newFunc;
	}
	public void MoveTowardsEnabled(bool enabled){
		if (!enabled) {
			RB.isKinematic = true;
		} else {
			RB.isKinematic = false;
			
		}
		allowMoveTowards = enabled;
	}
	public void setProximityBlock(bool enable = false){
		canProximityBlock = enable;
	}
	public void EndGame(){
		cancelAttacks ();
		if (optionalDog != null) {
			optionalDog.GetComponentInChildren<DogAttackScript> ().TurnOff ();
		}
		canMove = false;
	}
	IEnumerator GetUpAfterKnockdown(){
		//Debug.Log ("landed");
		gettingUp = true;
		foreach (Transform hurtbox in hurtboxes.GetComponentsInChildren<Transform>()) {
			if (hurtbox.name == "Body") {
				hurtbox.gameObject.SetActive (false);
			}
		}
		yield return new WaitForSeconds (.5f);
		spriteAnimator.PlayGetup ();
		//Debug.Log ("startgetup");
		for (int i = 0; i < 30;) {
			yield return null;
			if (!timeManager.CheckIfTimePaused ()) {
				i++;
			}
			//Debug.Log ("getup");
		}
		state.SetState ("neutral");
		spriteAnimator.PlayNeutralAnim ();
		foreach (Transform hurtbox in hurtboxes.GetComponentsInChildren<Transform>(true)) {
			if (hurtbox.name == "Body") {
				hurtbox.gameObject.SetActive (true);
			}
		}
		gettingUp = false;
	}
	public void PauseRigidBody(Vector2 velocity){
		StopAllCoroutines ();
		StartCoroutine (PauseRigidBodyEnum (velocity));
	}

	IEnumerator PauseRigidBodyEnum(Vector2 velocity){

		RB.gravityScale = 0;
		RB.velocity = Vector2.zero;
		for (int i = 0; i < 3;) {
			if (!timeManager.CheckIfTimePaused ()) {
				i++;
			}
			yield return null;

			//RB.isKinematic = true;
		}
		RB.gravityScale = 7;
		//RB.isKinematic = false;
		MoveToward (-velocity.x, velocity.y);
	}
}
