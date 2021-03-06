﻿using UnityEngine;
using System.Collections;

public class FeliciaAttackScript : MonoBehaviour {

	InputScript inputScript;
	SpriteAnimator spriteAnimator;
	FighterStateMachineScript state;
	PlayerMovementScript PMS;
	public GameObject lightHitbox, mediumHitbox,mediumHitboxPart2, heavyHitbox, jumpLightHitbox,jumpLightHitboxPart2, jumpMediumHitbox, jumpHeavyHitbox,
	sp1Hitbox, sp1HitboxPart2, sp1HitboxPart3, sp2HitboxPart1, sp2HitboxPart2, sp3Hitbox, fireballGunpoint, throwHitbox, superHitbox,superHitbox2,superHitbox3, proximityBox;

	TimeManagerScript timeManager;

	public HitboxScript lightHitboxScript, mediumHitboxScript, heavyHitboxScript, throwHitboxScript;
	public bool lightHitboxHit= false, mediumHitboxHit= false, heavyHitboxHit = false, specialHitboxHit = false;
	public GameObject ThrowPoint;



	bool  mediumBuffer = false, sp2Buffer = false, lightBuffer = false, sp1Buffer = false;
	HealthScript health;
	Transform otherPlayer;
	// Use this for initialization
	void Start () {
		if (tag == "playerOne") {
			//SetPlayer (true);
			otherPlayer = GameObject.FindWithTag ("playerTwo").transform;
		} else {
			//SetPlayer (false);
			otherPlayer = GameObject.FindWithTag ("playerOne").transform;
		}

		health = GetComponentInChildren<HealthScript> ();
		health.SetHitFunc (CancelAttacks);
		state = GetComponent<FighterStateMachineScript>();
		timeManager = GameObject.Find ("MasterGameObject").GetComponent<TimeManagerScript> ();
		spriteAnimator = GetComponent<SpriteAnimator> ();
		PMS = GetComponent<PlayerMovementScript> ();
		inputScript = GetComponent<InputScript> ();
		inputScript.assignAXButton (Throw);
		inputScript.assignXButton (Light, null);
		inputScript.assignYButton (Medium, null);
		inputScript.assignRBButton (Heavy, null);

		inputScript.assignAButton (SpecialOne, null);
		inputScript.assignBButton (SpecialTwo, null);
		inputScript.assignRT (SpecialThree, null);

		inputScript.assignBYButton (Super);

		lightHitboxScript.SetOptFunc (LightHit);
		mediumHitboxScript.SetOptFunc (MediumHit);
		heavyHitboxScript.SetOptFunc (HeavyHit);
		throwHitboxScript.SetThrowFunc (ThrowHit);
		sp1Hitbox.GetComponent<HitboxScript> ().SetOptFunc (SpecialHit);
		sp3Hitbox.GetComponent<HitboxScript> ().SetOptFunc (SpecialHit);
		PMS.setAttackCancel (CancelAttacks);
	}
	void Update(){
		if (Input.GetKeyDown (KeyCode.UpArrow)) {
			health.exCurrent = 1000;
		}
	}
	public void Throw(){
		if (state.GetState () == "neutral"  || lightBuffer || sp1Buffer) {
			CancelAttacks ();
			StartCoroutine (ThrowEnum ());
		}
	}
	IEnumerator ThrowEnum(){
		proximityBox.SetActive (true);
		spriteAnimator.PlayThrowTry ();
		PMS.StopMovement ();
		state.SetState ("attack");
		for (int x = 0; x < 10;) {
			if (!timeManager.CheckIfTimePaused()) {
				// startup
				// active
				if (x == 6) {
					throwHitbox.SetActive (true);
				}
				// recovery
				if (x == 8) {
					throwHitbox.SetActive (false);
					proximityBox.SetActive (false);
				}

				x++;
			}

			yield return null;
		}
		state.SetState ("neutral");
	}
	void ThrowHit(Transform otherPlayerTmp){
		otherPlayer = otherPlayerTmp.transform.parent;
		CancelAttacks ();
		StartCoroutine (ThrowHitEnum());
	}
	IEnumerator ThrowHitEnum(){

		spriteAnimator.PlayThrowComplete ();
		PMS.DsableBodyBox ();
		ThrowPoint.transform.localPosition = new Vector2 (1.75f, 0);
		otherPlayer.position = ThrowPoint.transform.position;
		otherPlayer.GetComponent<BoxCollider2D> ().enabled = false;
		Vector2 endPoint = new Vector2(1.75f,0);
		for (int x = 0; x < 76;) {
			if (!timeManager.CheckIfTimePaused()) {
				
				if (x < 24) {
					Vector2 newPoint = Vector2.Lerp (ThrowPoint.transform.localPosition, endPoint, .6f);
					ThrowPoint.transform.localPosition = newPoint;
					otherPlayer.position = ThrowPoint.transform.position;
				}
				if (x == 3) {
					endPoint= new Vector2 (0, 1f);
				}	
				if (x == 7) {
					endPoint= new Vector2 (-2.5f, -1f);
				}
				if (x == 12) {
					endPoint = new Vector2 (0, -2f);
				}
				if (x == 16) {
					endPoint = new Vector2 (1.75f, 0);
				}
				if (x == 19) {
					endPoint= new Vector2 (0, .75f);
				}
				if (x == 22) {
					endPoint= new Vector2 (-2.8f, -.25f);
				}

				if (x == 24) {
					otherPlayer.GetComponent<BoxCollider2D> ().enabled = true;
					otherPlayer.GetComponent<PlayerMovementScript> ().MoveToward (15, 15);
				}

				if (x == 50){
					PMS.MoveToward (10, 0);
				}
				x++;
			}
			yield return null;
		}
		PMS.EnableBodyBox ();
		state.SetState ("neutral");
	}

	public void Light(){
		if (state.GetState () == "neutral") {
			PMS.CheckFacing ();
			StartCoroutine (lightEnum ());
		} else if (state.GetState () == "jumping") {
			StartCoroutine (jumpLightEnum ());
		}

	}
	IEnumerator lightEnum(){
		health.AddMeter (10);
		proximityBox.SetActive (true);
		spriteAnimator.PlayLight ();
		lightBuffer = true;
		PMS.StopMovement ();
		state.SetState ("attack");
		for (int x = 0; x < 15;) {
			if (!timeManager.CheckIfTimePaused()) {
				// startup
				// active
				if (x == 2){
					lightBuffer = false;
				}
				if (x == 3) {
					lightHitbox.SetActive (true);
				}
				// recovery
				if (x == 6) {
					lightHitbox.SetActive (false);
					state.SetState ("light recovery");
					proximityBox.SetActive (false);
				}

				x++;
			}
			yield return null;
		}
		lightHitboxHit = false;
		state.SetState ("neutral");
	}
	IEnumerator jumpLightEnum(){
		proximityBox.SetActive (true);
		spriteAnimator.PlayJumpLight ();
		state.SetState ("jump attack");
		for (int x = 0; x < 15; ) {
			if (!timeManager.CheckIfTimePaused()) {
				if (x == 9){
					jumpLightHitbox.SetActive (true);
				}
				if (x == 11){
					jumpLightHitbox.SetActive (false);
					jumpLightHitboxPart2.SetActive (true);
					proximityBox.SetActive (false);
				}
				if (x == 13){
					jumpLightHitbox.SetActive (false);
					jumpLightHitboxPart2.SetActive (true);
					proximityBox.SetActive (false);
				}
				x++;
			}
			yield return null;
		}

		jumpLightHitboxPart2.SetActive (false);
	}
	public void Medium(){
		
		if (state.GetState() == "neutral" || (state.GetState() =="light recovery" && lightHitboxHit)) {
			if (lightHitboxHit) {
				lightHitboxHit = false;
			}
			PMS.CheckFacing ();
			StopAllCoroutines ();
			StartCoroutine (mediumEnum ());
		} else if (state.GetState () == "jumping") {
			StartCoroutine (jumpMediumEnum ());
		}

	}
	IEnumerator mediumEnum(){
		health.AddMeter (15);
		mediumBuffer = true;
		proximityBox.SetActive (true);
		spriteAnimator.PlayMedium ();
		PMS.StopMovement ();
		state.SetState ("attack");
		// startup
		for (int x = 0; x < 33; ) {
			if (!timeManager.CheckIfTimePaused()) {
				if (x == 3){
					mediumBuffer = false;	
				}
				// active
				if (x == 9) {
					mediumHitbox.SetActive (true);
				}
				// recovery
				if (x == 11){
					mediumHitbox.SetActive (false);
				}

				if (x == 16) {
					mediumHitboxPart2.SetActive (true);
				}
				if (x == 18){
					mediumHitboxPart2.SetActive (false);
					state.SetState ("medium recovery");
					proximityBox.SetActive (false);
				}

				x++;
			}

			yield return null;
		}
		mediumHitboxHit = false;
		state.SetState ("neutral");
	}
	IEnumerator jumpMediumEnum(){
		proximityBox.SetActive (true);
		spriteAnimator.PlayJumpMedium ();
		state.SetState ("jump attack");
		for (int x = 0; x < 27;) {
			if (!timeManager.CheckIfTimePaused()) {
				if (x == 9) {
					jumpMediumHitbox.SetActive (true);
				}
				if (x == 20) {
					jumpMediumHitbox.SetActive (false);
					proximityBox.SetActive (false);
				}
				x++;
			}
			yield return null;
		}
	}
	public void Heavy(){
		if (state.GetState() == "neutral" || (state.GetState() =="medium recovery" && mediumHitboxHit)) {
			if (mediumHitboxHit) {
				mediumHitboxHit = false;
			}
			PMS.CheckFacing ();
			StopAllCoroutines ();
			StartCoroutine (heavyEnum ());
		} else if (state.GetState () == "jumping" && transform.position.y > 2.5f && !PMS.jumpAway) {
			StartCoroutine (jumpHeavyEnum ());
		}

	}
	IEnumerator heavyEnum(){

		health.AddMeter (20);
		proximityBox.SetActive (true);
		spriteAnimator.PlayHeavy ();
		state.SetState ("attack");
		//PMS.MoveToward (7.5f);
		for (int x = 0; x < 30;) {
			if (!timeManager.CheckIfTimePaused()) {
				if (x == 11) {
					heavyHitbox.SetActive (true);
					PMS.StopMovement ();
				}

				if (x == 14) {
					heavyHitbox.SetActive (false);
					state.SetState ("heavy recovery");
					proximityBox.SetActive (false);
				}

				x++;
			}
			yield return null;
		}
		heavyHitboxHit = false;
		state.SetState ("neutral");
	}
	IEnumerator jumpHeavyEnum(){
		proximityBox.SetActive (true);
		spriteAnimator.PlayJumpHeavy ();
		state.SetState ("jump attack");
		PMS.landingRecoveryFrames = 10;
		PMS.GetComponent<BoxCollider2D>().offset = new Vector2 (0, 0);
		PMS.GetComponent<BoxCollider2D>().size  = new Vector2 (1.15f, 1.5f);
		for (int x = 0; x < 21;) {

			if (!timeManager.CheckIfTimePaused()) {
				if (x == 7) {
					jumpHeavyHitbox.SetActive (true);
				}
				if (x == 20) {
					jumpHeavyHitbox.SetActive (false);
					proximityBox.SetActive (false);
				}

				if (x == 7) {
					PMS.MoveToward (17, -10);
				}
				x++;
			}

			yield return null;
		}
	}

	public void SpecialOne(){
		if ((state.GetState() == "neutral" || (state.GetState() =="light recovery" && lightHitboxHit) || (state.GetState() =="medium recovery" && mediumHitboxHit) || (state.GetState() =="heavy recovery" && heavyHitboxHit))) {
			lightHitboxHit = false;
			mediumHitboxHit = false;
			heavyHitboxHit = false;
			PMS.CheckFacing ();
			StopAllCoroutines ();
			StartCoroutine (SpecialOneEnum ());
		}

	}
	IEnumerator SpecialOneEnum(){

		health.AddMeter (45);
		sp1Buffer = true;
		proximityBox.SetActive (true);
		spriteAnimator.PlaySpecialOne ();
		PMS.StopMovement ();
		state.SetState ("attack");
		for (int x = 0; x < 81;) {

			if (!timeManager.CheckIfTimePaused()) {
				if (x == 3){
					sp1Buffer = false;
				}
				// active
				if (x == 9) {
					sp1Hitbox.SetActive (true);
				}
				if (x == 12) {
					sp1Hitbox.SetActive (false);
				}
				if (x == 15) {
					sp1Hitbox.SetActive (true);
				}
				if (x == 18) {
					sp1Hitbox.SetActive (false);
				}
				if (x == 21) {
					sp1Hitbox.SetActive (true);
				}
				if (x == 24) {
					sp1Hitbox.SetActive (false);
				}
				if (x == 27) {
					sp1Hitbox.SetActive (true);
				}
				if (x == 30) {
					sp1Hitbox.SetActive (false);
				}

				if (x == 35) {
					sp1HitboxPart2.SetActive (true);
				}
				if (x == 38) {
					sp1HitboxPart2.SetActive (false);
				}

				if (x == 41) {
					sp1HitboxPart3.SetActive (true);
				}
				if (x == 43) {
					sp1HitboxPart3.SetActive (false);
					proximityBox.SetActive (false);
				}


				if (x == 36) {
					PMS.DsableBodyBox ();
					PMS.MoveToward (1f, 25);
				}
				if (x < 35 ){
					PMS.MoveToward (10f, 0);
				}
				if (x == 51) {
					PMS.MoveToward (-1.5f, 0);
				}

				x++;

			}

			yield return null;
		}
		PMS.EnableBodyBox ();
		state.SetState ("neutral");
	}
	public void SpecialTwo(){
		if (state.GetState() == "neutral" ||  (state.GetState() =="light recovery" && lightHitboxHit) || (state.GetState() =="medium recovery" && mediumHitboxHit) 
			|| (state.GetState() =="heavy recovery" && heavyHitboxHit)){
			lightHitboxHit = false;
			mediumHitboxHit = false;
			heavyHitboxHit = false;
			PMS.CheckFacing ();
			StopAllCoroutines ();
			StartCoroutine (SpecialTwoEnum ());
		}

	}
	IEnumerator SpecialTwoEnum(){

		health.AddMeter (15);
		spriteAnimator.PlaySpecialTwo ();
		PMS.StopMovement ();
		sp2Buffer = true;
		transform.position  = new Vector3(transform.position.x, transform.position.y, -1);
		//state.SetState ("attack");
		// needs jumpbox turned off
		PMS.DsableBodyBox ();
		state.SetState ("attack");
		for (int x = 0; x < 18;) {

			if (!timeManager.CheckIfTimePaused()) {
				if (x == 3) {
					sp2Buffer = false;
				}


				if (x < 15) {
					PMS.MoveToward (15f, 0);
				} 
				x++;
			}else {
				PMS.StopMovement ();
			}

			yield return null;
		}
		transform.position  = new Vector3(transform.position.x, transform.position.y, 0);
		state.SetState ("neutral");
		PMS.EnableBodyBox ();
	}
	public void SpecialThree(){
		if (state.GetState() == "neutral" ||  (state.GetState() =="light recovery" && lightHitboxHit) || (state.GetState() =="medium recovery" && mediumHitboxHit) 
			|| (state.GetState() =="heavy recovery" && heavyHitboxHit)) {
			lightHitboxHit = false;
			mediumHitboxHit = false;
			heavyHitboxHit = false;
			PMS.CheckFacing ();
			StopAllCoroutines ();
			StartCoroutine (SpecialthreeEnum ());
		}

	}
	IEnumerator SpecialthreeEnum(){

		health.AddMeter (30);
		proximityBox.SetActive (true);
		spriteAnimator.PlaySpecialThree ();
		PMS.StopMovement ();
		state.SetState ("attack");
		for (int x = 0; x < 27;) {

			if (!timeManager.CheckIfTimePaused()) {
				if (x == 6) {
					state.SetState ("projectile invulnerable");
				}
				if (x == 12) {

					sp3Hitbox.SetActive(true);
				}
				if (x == 16) {
					sp3Hitbox.SetActive(false);
					state.SetState ("attack");
					proximityBox.SetActive (false);

				}

				if (x > 6 ) {
					PMS.MoveToward (10);
				}

				x++;
			} else {
				PMS.StopMovement ();
			}

			yield return null;
		}
		PMS.StopMovement ();
		state.SetState ("neutral");
	}

	public void Super(){
		if (health.exCurrent >= 1000 && ((state.GetState() == "neutral" || (state.GetState() =="light recovery" && lightHitboxHit) || 
			(state.GetState() =="medium recovery" && mediumHitboxHit) || 
			(state.GetState() =="heavy recovery" && heavyHitboxHit) ||
			specialHitboxHit) || mediumBuffer || sp2Buffer)) {
			health.AddMeter (-1000);
			CancelAttacks ();
			PMS.CheckFacing ();
			mediumBuffer = false;
			sp2Buffer = false;
			lightHitboxHit = false;
			mediumHitboxHit = false;
			heavyHitboxHit = false;
			StopAllCoroutines ();
			StartCoroutine (SuperEnum ());
		}
	}
	IEnumerator SuperEnum(){
		spriteAnimator.PlaySuper ();
		PMS.StopMovement ();
		state.SetState ("attack");
		for (int x = 0; x < 180;) {

			if (!timeManager.CheckIfTimePaused()) {
				// active
//				if (x == 1) {
//					superHitbox.GetComponent<FeliciaSuperScript> ().SetTarget (otherPlayer.gameObject);
//					superHitbox.GetComponent<FollowScript>().transformToFollow = otherPlayer.gameObject;
//					superHitbox.SetActive (true);
//				}
				if (x < 26){
					PMS.MoveToward (15);
					if (x % 5 == 0) {
						superHitbox.SetActive (false);
						superHitbox.SetActive (true);
					}
				}
				if (x == 26) {
					superHitbox.SetActive (false);
				
				}
				if (x == 30) {
					superHitbox.SetActive (false);
					superHitbox.SetActive (true);
				}
				if (x == 40) {
					superHitbox.SetActive (false);
					superHitbox.SetActive (true);
				}
				if (x == 50) {
					superHitbox.SetActive (false);
					superHitbox.SetActive (true);
				}
				if (x == 60) {
					superHitbox.SetActive (false);
					superHitbox.SetActive (true);
				}
				if (x == 70) {
					superHitbox.SetActive (false);
					superHitbox.SetActive (true);
				}
				if (x == 80) {
					superHitbox.SetActive (false);
					superHitbox.SetActive (true);
				}
				if (x == 100) {
					superHitbox.SetActive (false);
					superHitbox.SetActive (true);
				}
				if (x == 120) {
					superHitbox.SetActive (false);
					superHitbox.SetActive (true);
				}if (x == 136) {
					superHitbox.SetActive (false);
					superHitbox2.SetActive (true);
				}
				if (x == 138) {
					superHitbox2.SetActive (false);
				}
				if (x == 140) {
					PMS.MoveToward (0, 24);
					PMS.DsableBodyBox ();
				}
				if (x == 144){
					superHitbox3.SetActive (true);
				}
				if (x == 150){
					superHitbox3.SetActive (false);
				}

				x++;
			}
			yield return null;
		}
		superHitbox.SetActive (false);
		superHitbox2.SetActive (false);
		superHitbox3.SetActive (false);
		PMS.EnableBodyBox ();
		state.SetState ("neutral");
	}
	public void SpecialHit(){
		specialHitboxHit = true;
	}
	public void LightHit(){
		lightHitboxHit = true;
	}
	public void MediumHit(){
		mediumHitboxHit = true;
	}
	public void HeavyHit(){
		heavyHitboxHit = true;
	}
	public void CancelAttacks(){
		PMS.EnableBodyBox ();
		mediumBuffer = false;
		sp2Buffer = false;
		lightBuffer = false;
		sp1Buffer = false;
		StopAllCoroutines ();
		lightHitbox.SetActive (false);
		jumpLightHitboxPart2.SetActive (false);
		mediumHitbox.SetActive (false);
		mediumHitboxPart2.SetActive (false);
		heavyHitbox.SetActive (false);
		jumpLightHitbox.SetActive (false);
		jumpMediumHitbox.SetActive (false);
		jumpHeavyHitbox.SetActive (false);
		sp2HitboxPart1.SetActive (false);
		sp2HitboxPart2.SetActive (false);
		sp3Hitbox.SetActive (false);
		throwHitbox.SetActive (false);
		proximityBox.SetActive (false);
		sp1Hitbox.SetActive (false);
		sp1HitboxPart2.SetActive (false);
		sp1HitboxPart3.SetActive (false);
		superHitbox.SetActive (false);
		superHitbox2.SetActive (false);
		superHitbox3.SetActive (false);
		specialHitboxHit = false;
		lightHitboxHit = false;
		mediumHitboxHit = false;
		heavyHitboxHit = false;
		PMS.GetComponent<BoxCollider2D>().offset = new Vector2 (0, -1.2f);
		PMS.GetComponent<BoxCollider2D>().size  = new Vector2 (1.6f, 2f);
	}

	public void SetPlayer(bool playerOne){
		if (playerOne) {
			jumpLightHitbox.GetComponent<HitboxScript>().AddTagToDamage("playerTwoHurtbox");
			jumpLightHitboxPart2.GetComponent<HitboxScript>().AddTagToDamage("playerTwoHurtbox");
			jumpMediumHitbox.GetComponent<HitboxScript>().AddTagToDamage("playerTwoHurtbox");
			jumpHeavyHitbox.GetComponent<HitboxScript>().AddTagToDamage("playerTwoHurtbox");
			lightHitbox.GetComponent<HitboxScript>().AddTagToDamage("playerTwoHurtbox");
			mediumHitbox.GetComponent<HitboxScript>().AddTagToDamage("playerTwoHurtbox");
			mediumHitboxPart2.GetComponent<HitboxScript>().AddTagToDamage("playerTwoHurtbox");
			heavyHitbox.GetComponent<HitboxScript>().AddTagToDamage("playerTwoHurtbox");
			sp1Hitbox.GetComponent<HitboxScript>().AddTagToDamage("playerTwoHurtbox");
			sp1HitboxPart2.GetComponent<HitboxScript>().AddTagToDamage("playerTwoHurtbox");
			sp1HitboxPart3.GetComponent<HitboxScript>().AddTagToDamage("playerTwoHurtbox");
			sp2HitboxPart1.GetComponent<HitboxScript>().AddTagToDamage("playerTwoHurtbox");
			sp2HitboxPart2.GetComponent<HitboxScript>().AddTagToDamage("playerTwoHurtbox");
			sp3Hitbox.GetComponent<HitboxScript>().AddTagToDamage("playerTwoHurtbox");
			throwHitbox.GetComponent<HitboxScript>().AddTagToDamage("playerTwoHurtbox");
			HitboxScript[] hits = superHitbox.GetComponentsInChildren<HitboxScript> (true);
			proximityBox.GetComponent<ProximityBlockScript>().tagToDamage = "playerTwo";
			foreach (HitboxScript h in hits) {
				h.AddTagToDamage ("playerTwoHurtbox");
			}
		} else {
			jumpLightHitbox.GetComponent<HitboxScript>().AddTagToDamage("playerOneHurtbox");
			jumpLightHitboxPart2.GetComponent<HitboxScript>().AddTagToDamage("playerOneHurtbox");
			jumpMediumHitbox.GetComponent<HitboxScript>().AddTagToDamage("playerOneHurtbox");
			jumpHeavyHitbox.GetComponent<HitboxScript>().AddTagToDamage("playerOneHurtbox");
			lightHitbox.GetComponent<HitboxScript>().AddTagToDamage("playerOneHurtbox");
			mediumHitbox.GetComponent<HitboxScript>().AddTagToDamage("playerOneHurtbox");
			mediumHitboxPart2.GetComponent<HitboxScript>().AddTagToDamage("playerOneHurtbox");
			heavyHitbox.GetComponent<HitboxScript>().AddTagToDamage("playerOneHurtbox");
			sp1Hitbox.GetComponent<HitboxScript>().AddTagToDamage("playerOneHurtbox");
			sp1HitboxPart2.GetComponent<HitboxScript>().AddTagToDamage("playerOneHurtbox");
			sp1HitboxPart3.GetComponent<HitboxScript>().AddTagToDamage("playerOneHurtbox");
			sp2HitboxPart1.GetComponent<HitboxScript>().AddTagToDamage("playerOneHurtbox");
			sp2HitboxPart2.GetComponent<HitboxScript>().AddTagToDamage("playerOneHurtbox");
			sp3Hitbox.GetComponent<HitboxScript>().AddTagToDamage("playerOneHurtbox");
			throwHitbox.GetComponent<HitboxScript>().AddTagToDamage("playerOneHurtbox");
			HitboxScript[] hits = superHitbox.GetComponentsInChildren<HitboxScript> (true);
			proximityBox.GetComponent<ProximityBlockScript>().tagToDamage = "playerOne";
			foreach (HitboxScript h in hits) {
				h.AddTagToDamage ("playerOneHurtbox");
			}	
		}
	}
}
