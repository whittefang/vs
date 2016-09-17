using UnityEngine;
using System.Collections;

public class DogAttackScript : MonoBehaviour {
	public GameObject sp1Hitbox, sp2Hitbox, sp3Hitbox, proximityBox, detectionBox;
	public SpriteRenderer spriteRenderer;
	public Transform Ken;

	TimeManagerScript timeManager;
	SoundsPlayer sounds;
	Rigidbody2D RB;
	DogAnimScript spriteAnimator;
	DogHealthScript healthScript;
	FighterStateMachineScript state;
	Transform otherPlayer;
	bool onLeft = true;

	// Use this for initialization
	void Awake () {
		state = GetComponent<FighterStateMachineScript> ();
		spriteAnimator = GetComponent<DogAnimScript> ();
		RB = GetComponent<Rigidbody2D> ();
		sounds = GetComponent<SoundsPlayer> ();
		timeManager = GameObject.Find ("MasterGameObject").GetComponent<TimeManagerScript> ();
		timeManager.AddBody (RB);
		sp1Hitbox.GetComponent<HitboxScript> ().SetOptFunc (StartSp1Complete);
		healthScript = GetComponentInChildren<DogHealthScript> ();
	}

	void FixedUpdate(){
		if (state.GetState () == "neutral" && Mathf.Abs(transform.position.x - Ken.position.x) > 2f && healthScript.CheckAlive()) {
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
			spriteRenderer.flipX = true;
		} else {
			onLeft = false;
			spriteRenderer.flipX = false;
		}
	}
	IEnumerator Turn(){
		state.SetState ("walking");
		bool doFlip = false;
		if (Ken.position.x > transform.position.x) {
			if (!onLeft) {
				doFlip = true;
			}
			onLeft = true;
			spriteRenderer.flipX = true;
		} else {
			if (onLeft) {
				doFlip = true;
			}
			onLeft = false;
			spriteRenderer.flipX = false;
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
		proximityBox.SetActive (true);
		spriteAnimator.StartSp1Anim ();
		StopMovement ();
		state.SetState ("attack");
		ForceFlipOpponent ();
		for (int x = 0; x < 60;) {			
			if (!timeManager.CheckIfTimePaused()) {		
				if (x == 30) {
					sp1Hitbox.SetActive (true);
				}
				if (x >=30){
					MoveTowards (25);
				}
				x++;
			}
			yield return null;
		}
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
		for (int x = 0; x < 60;) {
			if (x == 35) {
				StopMovement ();
			}
			if (!timeManager.CheckIfTimePaused()) {	
				if (x ==1) {
					MoveTowards (-5, 24);
				}
				x++;

			}
			yield return null;
		}
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
		state.SetState ("attack");
		ForceFlipOpponent ();
		spriteAnimator.StartWalkAnim ();
		detectionBox.SetActive (true);
		for (int x = 0; x < 45;) {
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
		state.SetState ("attack");
		spriteAnimator.StartSp2Anim ();
		for (int x = 0; x < 50;) {
			if (!timeManager.CheckIfTimePaused()) {
				if (x == 9) {
					MoveTowards (7.5f, 20);
					sp2Hitbox.SetActive (true);
				}if (x == 15) {
					MoveTowards (-5, RB.velocity.y);
				}
				if (x == 18) {
					sp2Hitbox.SetActive (false);
					proximityBox.SetActive (false);
				}
				if (x == 40) {
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
		state.SetState ("attack");
		ForceFlipOpponent ();
		for (int x = 0; x < 100;) {
			if (!timeManager.CheckIfTimePaused()) {
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
	public void StartHit(){
		StopAllCoroutines ();
		StartCoroutine (Hit ());
	}
	IEnumerator Hit(){
		state.SetState ("invincible");
		proximityBox.SetActive (false);
		spriteAnimator.StartHitAnim ();
		StopMovement ();
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
