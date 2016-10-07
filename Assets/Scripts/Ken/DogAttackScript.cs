using UnityEngine;
using System.Collections;

public class DogAttackScript : MonoBehaviour {
	public GameObject sp1Hitbox, sp2Hitbox, sp3Hitbox, superHitbox, superHitbox2, proximityBox, detectionBox;
	public SpriteRenderer spriteRenderer;
	public Transform Ken;

	TimeManagerScript timeManager;
	public SoundsPlayer sounds;
	Rigidbody2D RB;
	DogAnimScript spriteAnimator;
	DogHealthScript healthScript;
	FighterStateMachineScript state;
	public FighterStateMachineScript kenState;
	Transform otherPlayer;
	bool onLeft = true;
	public SpriteRenderer sp1Effect, sp2Effect, sp3Effect;

	// Use this for initialization
	void Awake () {
		state = GetComponent<FighterStateMachineScript> ();
		spriteAnimator = GetComponent<DogAnimScript> ();
		RB = GetComponent<Rigidbody2D> ();
		timeManager = GameObject.Find ("MasterGameObject").GetComponent<TimeManagerScript> ();
		timeManager.AddBody (RB);
		sp1Hitbox.GetComponent<HitboxScript> ().SetOptFunc (StartSp1Complete);
		healthScript = GetComponentInChildren<DogHealthScript> ();
	}

	void FixedUpdate(){
		if (state.GetState () == "neutral" && Mathf.Abs(transform.position.x - Ken.position.x) > 2f && healthScript.CheckAlive() 
			&& kenState.GetState() != "hitstun" &&  kenState.GetState() != "blockstun"  &&  kenState.GetState() != "jumping" ) {
			StartCoroutine (Turn ());
		}

	}

	public void SetPlayer(bool isP1){
		if (isP1) {
			detectionBox.GetComponent<DogDpDetectScript> ().TagToDetect = "playerTwoHurtbox";
			otherPlayer = GameObject.FindGameObjectWithTag ("playerTwo").transform;
		} else {
			detectionBox.GetComponent<DogDpDetectScript> ().TagToDetect = "playerOneHurtbox";
			otherPlayer = GameObject.FindGameObjectWithTag ("playerOne").transform;
		}
		healthScript.SetOtherPlayer (isP1);
			
	}
	void ForceFlipOpponent(){
		if (otherPlayer.position.x > transform.position.x) {
			onLeft = true;
			FlipSprites (true);
		} else {
			onLeft = false;
			FlipSprites (false);
		}
	}
	void FlipSprites(bool flip){

		spriteRenderer.flipX = flip;
		sp1Effect.flipX = flip;
		sp2Effect.flipX = flip;
		sp3Effect.flipX = flip;
	}
	IEnumerator Turn(){
		state.SetState ("walking");
		bool doFlip = false;
		if (Ken.position.x > transform.position.x) {
			if (!onLeft) {
				doFlip = true;
			}
			onLeft = true;
			FlipSprites (true);
		} else {
			if (onLeft) {
				doFlip = true;
			}
			onLeft = false;
			FlipSprites (false);
		}
		if (doFlip) {
			spriteAnimator.StarTurnAnim ();
			for (int i = 0; i < 9; ) {
				if (!timeManager.CheckIfTimePaused ()) {
					i++;
				}
				yield return null;
			}
		}
		StartCoroutine (WalkToKen ());
	}
	IEnumerator WalkToKen(){
		spriteAnimator.StartWalkAnim ();
		while (Mathf.Abs(Ken.position.x - transform.position.x) > .3f) {
			if (Ken.position.x > transform.position.x) {
				RB.velocity = new Vector2 (11, 0);
			} else {
				RB.velocity = new Vector2 (-11, 0);
			}
			yield return null;
		}
		Turn ();
		StopMovement ();
		state.SetState ("neutral");
		spriteAnimator.StartNeutralAnim ();
	}
	public void StartSp1(){
		if ((state.GetState() == "neutral" || state.GetState() == "walking") && healthScript.CheckAlive()) {
			StopAllCoroutines ();
			StartCoroutine (Sp1 ());
		}
	}
	IEnumerator Sp1(){
		sounds.PlaySP1 ();
		proximityBox.SetActive (true);
		spriteAnimator.StartSp1Anim ();
		StopMovement ();
		state.SetState ("attack");
		ForceFlipOpponent ();
		for (int x = 0; x < 60;) {			
			if (!timeManager.CheckIfTimePaused()) {		
				
				if (x == 10) {
					sp1Hitbox.SetActive (true);
					MoveTowards (25, 12);
				}
				if (x >10){
					MoveTowards (25);
				}
				x++;
			}
			yield return null;
		}
		proximityBox.SetActive (false);
		sp1Hitbox.SetActive (false);
		state.SetState ("neutral");
		spriteAnimator.StartNeutralAnim ();
	}
	public void StartSp1Complete(){
		StopAllCoroutines ();
		Debug.Log ("hit");
		StartCoroutine (Sp1Complete());
	}
	IEnumerator Sp1Complete(){
		StopMovement ();
		state.SetState ("attack");
		sp1Hitbox.SetActive (false);
		proximityBox.SetActive (false);
		spriteAnimator.StartSp1HitAnim ();
		sounds.PlayRandomHit ();
		for (int x = 0; x < 70;) {
			if (x == 35) {
				StopMovement ();
			}
			if (!timeManager.CheckIfTimePaused()) {	
				if (x ==1) {
					MoveTowards (-10, 22);
				}
				x++;

			}
			yield return null;
		}
		proximityBox.SetActive (false);
		sp1Hitbox.SetActive (false);
		state.SetState ("neutral");
		spriteAnimator.StartNeutralAnim ();
	}
	public void StartSp2(){
		if ((state.GetState() == "neutral" || state.GetState() == "walking") && healthScript.CheckAlive()) {
			StopAllCoroutines ();
			StartCoroutine (Sp2 ());
		}
	}
	IEnumerator Sp2(){
		proximityBox.SetActive (true);
		StopMovement ();
		sounds.PlaySP2 ();
		state.SetState ("attack");
		ForceFlipOpponent ();
		spriteAnimator.StartWalkAnim ();
		detectionBox.SetActive (true);
		for (int x = 0; x < 35;) {
			if (!timeManager.CheckIfTimePaused()) {
				
				MoveTowards (15, 0);
				x++;
			}
			yield return null;
		}
		detectionBox.SetActive (false);
		state.SetState ("neutral");
		spriteAnimator.StartNeutralAnim ();
	}
	public void StartSp2Complete(){
		if (healthScript.CheckAlive ()) {
			StopAllCoroutines ();
			StartCoroutine (Sp2Complete ());
		}
	}
	IEnumerator Sp2Complete(){
		proximityBox.SetActive (true);
		detectionBox.SetActive (false);
		StopMovement ();
		sounds.PlayRandomHit ();
		state.SetState ("attack");
		spriteAnimator.StartSp2Anim ();
		for (int x = 0; x < 80;) {
			if (!timeManager.CheckIfTimePaused()) {
				if (x == 15) {
					MoveTowards (7.5f, 20);
					sp2Hitbox.SetActive (true);
				}if (x == 21) {
					MoveTowards (-5, RB.velocity.y);
				}
				if (x == 24) {
					sp2Hitbox.SetActive (false);
					proximityBox.SetActive (false);
				}
				if (x == 46) {
					StopMovement ();
				}
				x++;
			}
			yield return null;
		}
		state.SetState ("neutral");
		spriteAnimator.StartNeutralAnim ();
	}
	public void StartSp3(){
		if ((state.GetState() == "neutral" || state.GetState() == "walking") && healthScript.CheckAlive()) {
			StopAllCoroutines ();
			StartCoroutine (Sp3 ());
		}
	}
	IEnumerator Sp3(){
		proximityBox.SetActive (true);
		spriteAnimator.StartSp3Anim ();
		StopMovement ();
		sounds.PlaySP3 ();
		state.SetState ("attack");
		ForceFlipOpponent ();
		for (int x = 0; x < 110;) {
			if (!timeManager.CheckIfTimePaused()) {
				if (x == 50){
					sounds.PlayRandomHit ();
				}
				if (x == 27) {
					MoveTowards (10, 20);
				}
				if (x == 40) {
					sp3Hitbox.SetActive (true);
				}
				if (x == 42) {
					sp3Hitbox.SetActive (false);
					sp3Hitbox.SetActive (true);
				}
				if (x == 44) {
					sp3Hitbox.SetActive (false);
					sp3Hitbox.SetActive (true);
				}
				if (x == 46) {
					sp3Hitbox.SetActive (false);
					sp3Hitbox.SetActive (true);
				}
				if (x == 48) {
					sp3Hitbox.SetActive (false);
					sp3Hitbox.SetActive (true);
				}
				if (x == 50) {
					sp3Hitbox.SetActive (false);
					sp3Hitbox.SetActive (true);
				}
				if (x == 52) {
					sp3Hitbox.SetActive (false);
					sp3Hitbox.SetActive (true);
				}
				if (x == 54) {
					sp3Hitbox.SetActive (false);
					sp3Hitbox.SetActive (true);
				}
				if (x == 56) {
					sp3Hitbox.SetActive (false);
					proximityBox.SetActive (false);
					StopMovement ();
				}
				x++;
			}
			yield return null;
		}
		state.SetState ("neutral");
		spriteAnimator.StartNeutralAnim ();
	}
	public void StartSuper(){
		if ((state.GetState() == "neutral" || state.GetState() == "walking") && healthScript.CheckAlive()) {
			StopAllCoroutines ();
			StartCoroutine (Super ());
		}
	}
	IEnumerator Super(){
		proximityBox.SetActive (true);
		spriteAnimator.StartSuperAnim ();
		StopMovement ();
		state.SetState ("attack");
		ForceFlipOpponent ();
		for (int x = 0; x < 118;) {
			if (!timeManager.CheckIfTimePaused()) {
				if (x ==30 ){
					MoveTowards (12f, 0);
				}
				if (x >= 30 && x <= 80 && (x % 5 == 0)) {
					superHitbox.SetActive (false);
					superHitbox.SetActive (true);
				}

				if (x > 35 && x < 80) {
					MoveTowards (7.5f, 0);
				}
				if (x == 85) {
					StopMovement ();
				}
				if (x == 90) {
					superHitbox.SetActive (false);
					superHitbox2.SetActive (true);
				}
				if (x == 92) {
					superHitbox.SetActive (false);
					superHitbox2.SetActive (false);
				}
				x++;
			}
			yield return null;
		}
		state.SetState ("neutral");
		spriteAnimator.StartNeutralAnim ();
	}
	public void StartHit(){
		StopAllCoroutines ();
		StartCoroutine (Hit ());
	}
	public void TurnOff(){
		StopAllCoroutines ();
		state.SetState ("off");
		proximityBox.SetActive (false);
		sp1Hitbox.SetActive (false);
		sp2Hitbox.SetActive (false);
		sp3Hitbox.SetActive (false);
		superHitbox.SetActive (false);
		superHitbox2.SetActive (false);
		detectionBox.SetActive (false);
		StopMovement ();
	}
	IEnumerator Hit(){
		state.SetState ("invincible");
		proximityBox.SetActive (false);
		sp1Hitbox.SetActive (false);
		sp2Hitbox.SetActive (false);
		sp3Hitbox.SetActive (false);
		superHitbox.SetActive (false);
		superHitbox2.SetActive (false);
		detectionBox.SetActive (false);
		spriteAnimator.StartHitAnim ();
		StopMovement ();
		sounds.PlayExtra ();
		for (int x = 0; x < 60;) {			
			if (!timeManager.CheckIfTimePaused()) {
				if (x == 0) {
					MoveTowards (-5, 20);
				}
				if (x == 30) {
					StopMovement ();
				}
				x++;
			}
			yield return null;
		}
		if (healthScript.CheckAlive()){
			state.SetState ("neutral");
			spriteAnimator.StartNeutralAnim ();
		}
	}
	void MoveTowards(float x, float y = 0){
		if (y == 0) {
			y = RB.velocity.y;
		}
		if (onLeft) {
			RB.velocity = new Vector2 (x, y);
		}else {
			RB.velocity = new Vector2 (-x, y);
		}
	}
	void StopMovement(){
		RB.velocity = Vector2.zero;
	}
	public void StartRecover(){
		spriteAnimator.StartNeutralAnim ();
		state.SetState ("neutral");
	}
}
