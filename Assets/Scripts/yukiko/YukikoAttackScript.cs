﻿using UnityEngine;
using System.Collections;

public class YukikoAttackScript : MonoBehaviour {

	InputScript inputScript;
	SpriteAnimator spriteAnimator;
	FighterStateMachineScript state;
	PlayerMovementScript PMS;
	public GameObject fan1, fan2, sp1Fireball, sp3Trap, mediumHitbox, heavyHitbox, jumpMediumHitbox, jumpHeavyHitbox,
	sp2Hitbox, fireballGunpoint, throwHitbox, proximityBox;

	ProjectileScript sp1FireballProjectileScript , fan1ProjectileScript, fan2ProjectileScript;
	TimeManagerScript timeManager;

	SoundsPlayer sounds;
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
		} else {
			//SetPlayer (false);
		}

		health = GetComponentInChildren<HealthScript> ();
		health.SetHitFunc (CancelAttacks);
		sounds = GetComponent<SoundsPlayer>();
		state = GetComponent<FighterStateMachineScript>();
		timeManager = GameObject.Find ("MasterGameObject").GetComponent<TimeManagerScript> ();
		spriteAnimator = GetComponent<SpriteAnimator> ();
		fan1ProjectileScript = fan1.GetComponent < ProjectileScript> ();
		fan2ProjectileScript = fan2.GetComponent < ProjectileScript> ();
		sp1FireballProjectileScript = sp1Fireball.GetComponent<ProjectileScript> ();
		PMS = GetComponent<PlayerMovementScript> ();
		PMS.setAttackCancel (CancelAttacks);
		inputScript = GetComponent<InputScript> ();
		inputScript.assignAXButton (Throw);
		inputScript.assignXButton (Light, null);
		inputScript.assignYButton (Medium, null);
		inputScript.assignRBButton (Heavy, null);

		inputScript.assignAButton (SpecialOne, null);
		inputScript.assignBButton (SpecialTwo, null);
		inputScript.assignRT (SpecialThree, null);

		inputScript.assignBYButton (Super);

//		lightHitboxScript.SetOptFunc (LightHit);
//		mediumHitboxScript.SetOptFunc (MediumHit);
//		heavyHitboxScript.SetOptFunc (HeavyHit);
		throwHitboxScript.SetThrowFunc (ThrowHit);
		sp1Fireball.GetComponentInChildren<HitboxScript> ().SetOptFunc (SpecialHit);
		sp2Hitbox.GetComponent<HitboxScript> ().SetOptFunc (SpecialHit);
		sp3Trap.GetComponent<HitboxScript> ().SetOptFunc (SpecialHit);


	}
	void Update(){
		if (Input.GetKeyDown (KeyCode.UpArrow)) {
			health.exCurrent = 1000;
		}
	}

	public void Throw(){
		if (state.GetState () == "neutral" || lightBuffer || sp1Buffer) {
			CancelAttacks ();
			StartCoroutine (ThrowEnum ());
		}
	}
	IEnumerator ThrowEnum(){
		proximityBox.SetActive (true);
		spriteAnimator.PlayThrowTry ();
		PMS.StopMovement ();
		state.SetState ("attack");
		for (int x = 0; x < 27;) {
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
		ThrowPoint.transform.localPosition = new Vector2 (-1, 0);
		otherPlayer.position = ThrowPoint.transform.position;
		Vector2 endPoint = new Vector2(1f,0);
		for (int x = 0; x < 42;) {

			if (!timeManager.CheckIfTimePaused()) {
				if (x < 24) {
					Vector2 newPoint = Vector2.Lerp (ThrowPoint.transform.localPosition, endPoint, .6f);
					ThrowPoint.transform.localPosition = newPoint;
					otherPlayer.position = ThrowPoint.transform.position;
				}
				if (x == 1) {
					endPoint= new Vector2 (-3f, 0);
				}
				if (x == 15) {
					endPoint = new Vector2 (-3, 2f);
				}
				if (x == 18) {
					endPoint = new Vector2 (-2f, 1.5f);
				}
				if (x == 21) {
					endPoint = new Vector2 (1,1f);
				}
				if (x == 24) {
					endPoint = new Vector2 (1,-1f);
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
		lightBuffer = true;
		spriteAnimator.PlayLight ();
		PMS.StopMovement ();
		state.SetState ("attack");
		for (int x = 0; x < 45;) {
			// startup
			// active

			if (!timeManager.CheckIfTimePaused()) {
				if (x == 3){
					lightBuffer = false;
				}
				if (x == 4) {
					//lightHitbox.SetActive (true);
				}
				// recovery
				if (x == 6) {
					//lightHitbox.SetActive (false);
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
				if (x == 4){
					//jumpLightHitbox.SetActive (true);
				}
				if (x == 10){
					//jumpLightHitbox.SetActive (false);
					proximityBox.SetActive (false);
				}
				x++;
			}
			yield return null;
		}
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
		state.SetState ("attack");
		// startup
		for (int x = 0; x < 27; ) {

			if (!timeManager.CheckIfTimePaused()) {
				if (x == 3){
					mediumBuffer = false;	
				}
				// active
				if (x == 9) {
					PMS.StopMovement ();
					mediumHitbox.SetActive (true);
				}
				// recovery
				if (x == 11){
					mediumHitbox.SetActive (false);
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
		} else if (state.GetState () == "jumping") {
			StartCoroutine (jumpHeavyEnum ());
		}

	}
	IEnumerator heavyEnum(){

		health.AddMeter (20);
		proximityBox.SetActive (true);
		spriteAnimator.PlayHeavy ();
		PMS.StopMovement ();
		state.SetState ("attack");
		for (int x = 0; x < 42;) {
			if (!timeManager.CheckIfTimePaused()) {
				if (x == 18) {
					heavyHitbox.SetActive (true);
				}

				if (x == 22) {
					PMS.StopMovement ();
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
		for (int x = 0; x < 21;) {
			if (!timeManager.CheckIfTimePaused()) {
				if (x == 7) {
					jumpHeavyHitbox.SetActive (true);
				}
				if (x == 20) {
					jumpHeavyHitbox.SetActive (false);
					proximityBox.SetActive (false);
				}

				x++;
			}
			yield return null;
		}
	}

	public void SpecialOne(){
		if ((state.GetState() == "neutral" || (state.GetState() =="light recovery" && lightHitboxHit) || (state.GetState() =="medium recovery" && mediumHitboxHit) 
			|| (state.GetState() =="heavy recovery" && heavyHitboxHit)) && !sp1Fireball.activeSelf) {
			lightHitboxHit = false;
			mediumHitboxHit = false;
			heavyHitboxHit = false;
			PMS.CheckFacing ();
			StopAllCoroutines ();
			StartCoroutine (SpecialOneEnum ());
		}

	}
	IEnumerator SpecialOneEnum(){
		sp1Buffer = true;
		health.AddMeter (30);
		proximityBox.SetActive (true);
		spriteAnimator.PlaySpecialOne ();
		PMS.StopMovement ();
		state.SetState ("attack");
		bool canShoot = true;
		for (int x = 0; x < 45;) {

			if (!timeManager.CheckIfTimePaused()) {
				// active
				if (x == 3){
					sp1Buffer = false;
				}
				if (x == 12 && canShoot) {
					canShoot = false;
					sounds.PlayExtra ();
					//fireball.transform.position = fireballGunpoint.transform.position;
					if (PMS.CheckIfOnLeft ()) {
						//fireball.transform.eulerAngles = new Vector2(0, 0);
						//fireballProjectileScript.direction = new Vector2 (1, 0);
					} else {
						//fireball.transform.eulerAngles = new Vector2(0, 180);
						//fireballProjectileScript.direction = new Vector2 (-1, 0);

					}
					//fireball.SetActive (true);
					proximityBox.SetActive (false);
				}
				x++;
			}
			yield return null;

		}
		specialHitboxHit = false;
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

		health.AddMeter (30);
		proximityBox.SetActive (true);
		spriteAnimator.PlaySpecialTwo ();
		PMS.StopMovement ();
		sp2Buffer = true;
		//state.SetState ("attack");
		// needs jumpbox turned off

		state.SetState ("attack");
		PMS.DsableBodyBox ();
		for (int x = 0; x < 50;) {

			if (!timeManager.CheckIfTimePaused()) {
				if (x == 2) {
					state.SetState ("invincible");
				}
				if (x == 3) {
					sp2Buffer = false;
					//PMS.StopMovement ();
					//sp2HitboxPart1.SetActive (true);
				}

				if (x == 10){
					state.SetState ("attack");
				}


				if (x == 15){

					specialHitboxHit = false;
					//state.SetState ("attack");
					sp2Hitbox.SetActive (false);
					sp2Hitbox.SetActive (true);
				}
				if (x == 23) {
					sp2Hitbox.SetActive (false);
					proximityBox.SetActive (false);
				}

				if (x == 14) {
				}
				x++;
			}
			yield return null;
		}
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
		state.SetState ("projectile invulnerable");
		for (int x = 0; x < 6;) {
			yield return null;
			if (!timeManager.CheckIfTimePaused()) {
				x++;
			}
		}
		for (int x = 0; x < 80;) {

			if (!timeManager.CheckIfTimePaused()) {
				

				x++;
			}

			yield return null;
		}
		PMS.StopMovement ();
		for (int x = 0; x < 10;) {
			yield return null;
			if (!timeManager.CheckIfTimePaused()) {
				x++;
			}
		}
		specialHitboxHit = false;
		PMS.StopMovement ();
		state.SetState ("neutral");
	}

	public void Super(){
		if (health.exCurrent >= 1000 && ((state.GetState() == "neutral" || (state.GetState() =="light recovery" && lightHitboxHit) || 
			(state.GetState() =="medium recovery" && mediumHitboxHit) || 
			(state.GetState() =="heavy recovery" && heavyHitboxHit) || 
			(specialHitboxHit)) || mediumBuffer || sp2Buffer)) {
			health.AddMeter (-1000);
			CancelAttacks ();
			PMS.CheckFacing ();
			mediumBuffer = false;
			sp2Buffer = false;
			Debug.Log ("super2");
			lightHitboxHit = false;
			mediumHitboxHit = false;
			heavyHitboxHit = false;
			StopAllCoroutines ();
			StartCoroutine (SuperEnum ());
		}
	}
	IEnumerator SuperEnum(){
		proximityBox.SetActive (true);
		spriteAnimator.PlaySuper ();
		PMS.StopMovement ();
		state.SetState ("attack");
		bool canShoot = true;
		for (int x = 0; x < 45;) {
			// active

			if (!timeManager.CheckIfTimePaused()) {
				if (x == 12 && canShoot) {
					canShoot = false;
					sounds.PlayExtra ();
				
					proximityBox.SetActive (false);
				}
				x++;
			}
			yield return null;

		}
		state.SetState ("neutral");
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
	public void SpecialHit(){
		specialHitboxHit = true;
	}
	public void CancelAttacks(){
		mediumBuffer = false;
		sp2Buffer = false;
		lightBuffer = false;
		sp1Buffer = false;
		StopAllCoroutines ();
		mediumHitbox.SetActive (false);
		heavyHitbox.SetActive (false);
		jumpMediumHitbox.SetActive (false);
		jumpHeavyHitbox.SetActive (false);
		sp2Hitbox.SetActive (false);
		throwHitbox.SetActive (false);
		proximityBox.SetActive (false);
		specialHitboxHit = false;
		lightHitboxHit = false;
		mediumHitboxHit = false;
		heavyHitboxHit = false;

	}
}
