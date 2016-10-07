using UnityEngine;
using System.Collections;

public class ShanaAttackScript : MonoBehaviour {

	InputScript inputScript;
	SpriteAnimator spriteAnimator;
	FighterStateMachineScript state;
	PlayerMovementScript PMS;
	public GameObject fireball, fireExplosion, superFireball, lightHitbox, mediumHitbox, heavyHitbox, jumpLightHitbox, jumpMediumHitbox, jumpHeavyHitbox,
	sp1Hitbox, sp2HitboxPart1, sp2HitboxPart2, superHitbox, fireballGunpoint, throwHitbox, throwHitboxFollowup, proximityBox;
	public GameObject swingOneEffect,swingTwoEffect,swingThreeEffect,airDashEffect, sp2Effect, sp3Effect, airSwingTwo, airSwingThree;
	ProjectileScript fireballProjectileScript , superProjectileScript;
	TimeManagerScript timeManager;

	SoundsPlayer sounds;
	HitboxScript lightHitboxScript, mediumHitboxScript, heavyHitboxScript, throwHitboxScript;
	public bool lightHitboxHit= false, mediumHitboxHit= false,airMediumHitboxHit= false, heavyHitboxHit = false, specialHitboxHit = false;
	public GameObject ThrowPoint;
	public int airDashes = 2;
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
		fireballProjectileScript = fireball.GetComponent < ProjectileScript> ();
		superProjectileScript = superFireball.GetComponent<ProjectileScript> ();
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

		lightHitbox.GetComponent<HitboxScript>().SetOptFunc (LightHit);
		mediumHitbox.GetComponent<HitboxScript>().SetOptFunc (MediumHit);
		heavyHitbox.GetComponent<HitboxScript>().SetOptFunc (HeavyHit);
		throwHitbox.GetComponent<HitboxScript>().SetThrowFunc (ThrowHit);
		jumpMediumHitbox.GetComponent<HitboxScript>().SetOptFunc (MediumAirHit);
		fireball.GetComponentInChildren<HitboxScript> ().SetOptFunc (SpecialHit);
		//sp2HitboxPart1.GetComponent<HitboxScript> ().SetOptFunc (SpecialHit);


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
		for (int x = 0; x < 42;) {

			if (!timeManager.CheckIfTimePaused()) {
				
				if (x == 24) {
					throwHitboxFollowup.SetActive (true);
				}
				if (x == 24) {
					throwHitboxFollowup.SetActive (true);
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
		} else if (state.GetState () == "jumping" && transform.position.y > 1.5f && airDashes > 0) {
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
		for (int x = 0; x < 15;) {
			// startup
			// active

			if (!timeManager.CheckIfTimePaused()) {
				if (x == 3){
					swingOneEffect.SetActive (true);
					lightBuffer = false;
				}
				if (x == 4) {
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
		PMS.ForceFlip ();
		spriteAnimator.PlayJumpLight ();
		state.SetState ("jump attack");
		airDashes--;

		PMS.TurnOffGravity (true);
		PMS.StopMovement();
		airDashEffect.SetActive (true);
		for (int x = 0; x < 18; ) {
			if (!timeManager.CheckIfTimePaused()) {
				PMS.MoveToward (20, 0);

				x++;
			}
			yield return null;
		}
		PMS.StopMovement ();
		PMS.TurnOffGravity (false);
		state.SetState ("jumping");
	}
	public void Medium(){
		if (state.GetState() == "neutral" || (state.GetState() =="light recovery" && lightHitboxHit)) {
			if (lightHitboxHit) {
				lightHitboxHit = false;
			}
			PMS.CheckFacing ();
			StopAllCoroutines ();
			StartCoroutine (mediumEnum ());
		} else if (state.GetState () == "jumping" ) {
			PMS.ForceFlip();
			StopAllCoroutines ();
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
				if (x >= 3 && x < 9) {
					PMS.MoveToward (10f,0);
				}
				// active
				if (x == 9) {
					PMS.StopMovement ();
					swingTwoEffect.SetActive (true);
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
				if (x == 6) {
					airSwingTwo.SetActive (true);
					jumpMediumHitbox.SetActive (true);
				}
				if (x == 10) {
					jumpMediumHitbox.SetActive (false);
					proximityBox.SetActive (false);
				}
				x++;
			}

			yield return null;
		}
		airMediumHitboxHit = false;
	}
	public void Heavy(){
		if (state.GetState() == "neutral" || (state.GetState() =="medium recovery" && mediumHitboxHit)) {
			if (mediumHitboxHit) {
				mediumHitboxHit = false;
			}
			PMS.CheckFacing ();
			StopAllCoroutines ();
			StartCoroutine (heavyEnum ());
		} else if (state.GetState () == "jumping"  || (airMediumHitboxHit)) {
			StopAllCoroutines();
			airMediumHitboxHit = false;
			PMS.ForceFlip();
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
				if (x == 9) {
					swingThreeEffect.SetActive (true);
					heavyHitbox.SetActive (true);
				}
				if (x == 12) {
					heavyHitbox.SetActive (false);
					proximityBox.SetActive (false);
					state.SetState ("heavy recovery");
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
				if (x == 4) {
					airSwingThree.SetActive (true);
					jumpHeavyHitbox.SetActive (true);
				}
				if (x == 8) {
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
			|| (state.GetState() =="heavy recovery" && heavyHitboxHit)) && !fireball.activeSelf) {
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
		for (int x = 0; x < 35;) {

			if (!timeManager.CheckIfTimePaused()) {
				// active
				if (x == 3){
					sp1Buffer = false;
				}
				if (x == 12 && canShoot) {
					canShoot = false;
					sounds.PlayExtra ();
					fireball.transform.position = fireballGunpoint.transform.position;
					if (PMS.CheckIfOnLeft ()) {
						fireball.transform.eulerAngles = new Vector2(0, 0);
						fireballProjectileScript.direction = new Vector2 (1, 0);
					} else {
						fireball.transform.eulerAngles = new Vector2(0, 180);
						fireballProjectileScript.direction = new Vector2 (-1, 0);

					}
					PMS.MoveToward (-10, 15);
					fireball.SetActive (true);
					proximityBox.SetActive (false);
				}
				if (x > 12 && x < 30) {
					PMS.MoveToward (-5f, 0);
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
		PMS.MoveToward (5f,17.5f);

		state.SetState ("attack");
		PMS.DsableBodyBox ();
		for (int x = 0; x < 50;) {

			if (!timeManager.CheckIfTimePaused()) {
				if (x == 3) {
					sp2Buffer = false;
					//PMS.StopMovement ();
				}
				if (x == 8){
					sp2Effect.SetActive (true);
				}
				if (x >=  8 & x < 24) {
					if (x % 4 == 0) {
						sp2HitboxPart1.SetActive (false);
						sp2HitboxPart1.SetActive (true);
					}
				}

				if (x == 26){
					sp2HitboxPart1.SetActive (false);
					sp2HitboxPart2.SetActive (true);
				}
				if (x == 28) {
					sp2HitboxPart2.SetActive (false);
					proximityBox.SetActive (false);
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
		state.SetState ("attack");
		for (int x = 0; x < 30;) {
			if (!timeManager.CheckIfTimePaused()) {
				if (x == 20) {
					fireExplosion.transform.position = fireballGunpoint.transform.position;
					fireExplosion.transform.Translate (new Vector3 (0, .5f, 0));
					fireExplosion.SetActive (true);
				}
				x++;
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
			(specialHitboxHit)) || mediumBuffer || sp2Buffer || state.GetState() == "jump attack" || state.GetState() == "jumping" )) {
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
		for (int x = 0; x < 80;) {
			// active

			if (!timeManager.CheckIfTimePaused()) {
				if (x == 5) {
					sounds.PlayExtra ();
					superFireball.transform.position = new Vector3( transform.position.x, superFireball.transform.position.y, superFireball.transform.position.z);
					superFireball.SetActive (true);
					proximityBox.SetActive (false);
					PMS.MoveToward(0, 15f);
				}
				if (x % 2==0 && x > 5 && x < 60) {
					superHitbox.SetActive (false);
					superHitbox.SetActive (true);
					PMS.MoveToward(0, 5f);
				}
				if (x == 62) {
					superHitbox.SetActive (false);
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
	public void MediumAirHit(){
		airMediumHitboxHit = true;
	}
	public void CancelAttacks(){
		PMS.EnableBodyBox ();
		mediumBuffer = false;
		sp2Buffer = false;
		lightBuffer = false;
		sp1Buffer = false;
		StopAllCoroutines ();
		lightHitbox.SetActive (false);
		mediumHitbox.SetActive (false);
		heavyHitbox.SetActive (false);
		jumpLightHitbox.SetActive (false);
		jumpMediumHitbox.SetActive (false);
		jumpHeavyHitbox.SetActive (false);

		sp2HitboxPart1.SetActive (false);
		sp2HitboxPart2.SetActive (false);
		throwHitbox.SetActive (false);
		proximityBox.SetActive (false);
		specialHitboxHit = false;
		lightHitboxHit = false;
		mediumHitboxHit = false;
		heavyHitboxHit = false;
		airMediumHitboxHit = false;
		superHitbox.SetActive (false);



		PMS.TurnOffGravity (false);
		airDashes = 2;
	}

	public void SetPlayer(bool playerOne){
		if (playerOne) {
			fireball.GetComponent<ProjectileScript> ().projectileOwner = 0;
			superFireball.GetComponent<ProjectileScript> ().projectileOwner = 0;

			jumpLightHitbox.GetComponent<HitboxScript>().AddTagToDamage("playerTwoHurtbox");
			jumpMediumHitbox.GetComponent<HitboxScript>().AddTagToDamage("playerTwoHurtbox");
			jumpHeavyHitbox.GetComponent<HitboxScript>().AddTagToDamage("playerTwoHurtbox");
			lightHitbox.GetComponent<HitboxScript>().AddTagToDamage("playerTwoHurtbox");
			mediumHitbox.GetComponent<HitboxScript>().AddTagToDamage("playerTwoHurtbox");
			heavyHitbox.GetComponent<HitboxScript>().AddTagToDamage("playerTwoHurtbox");
			//sp1Hitbox.GetComponent<HitboxScript>().AddTagToDamage("playerTwoHurtbox");
			sp2HitboxPart1.GetComponent<HitboxScript>().AddTagToDamage("playerTwoHurtbox");
			sp2HitboxPart2.GetComponent<HitboxScript>().AddTagToDamage("playerTwoHurtbox");
			fireball.GetComponentInChildren<HitboxScript>().AddTagToDamage("playerTwoHurtbox");
			throwHitbox.GetComponent<HitboxScript>().AddTagToDamage("playerTwoHurtbox");
			superFireball.GetComponentInChildren<HitboxScript>().AddTagToDamage("playerTwoHurtbox");
			fireball.GetComponentInChildren<ProximityBlockScript>().tagToDamage = "playerTwo";
			proximityBox.GetComponent<ProximityBlockScript>().tagToDamage = "playerTwo";

		} else {
			fireball.GetComponent<ProjectileScript> ().projectileOwner = 1;
			superFireball.GetComponent<ProjectileScript> ().projectileOwner = 1;

			jumpLightHitbox.GetComponent<HitboxScript>().AddTagToDamage("playerOneHurtbox");
			jumpMediumHitbox.GetComponent<HitboxScript>().AddTagToDamage("playerOneHurtbox");
			jumpHeavyHitbox.GetComponent<HitboxScript>().AddTagToDamage("playerOneHurtbox");
			lightHitbox.GetComponent<HitboxScript>().AddTagToDamage("playerOneHurtbox");
			mediumHitbox.GetComponent<HitboxScript>().AddTagToDamage("playerOneHurtbox");
			heavyHitbox.GetComponent<HitboxScript>().AddTagToDamage("playerOneHurtbox");
			//sp1Hitbox.GetComponent<HitboxScript>().AddTagToDamage("playerOneHurtbox");
			sp2HitboxPart1.GetComponent<HitboxScript>().AddTagToDamage("playerOneHurtbox");
			sp2HitboxPart2.GetComponent<HitboxScript>().AddTagToDamage("playerOneHurtbox");
			fireball.GetComponentInChildren<HitboxScript>().AddTagToDamage("playerOneHurtbox");
			throwHitbox.GetComponent<HitboxScript>().AddTagToDamage("playerOneHurtbox");
			superFireball.GetComponentInChildren<HitboxScript>().AddTagToDamage("playerOneHurtbox");

			fireball.GetComponentInChildren<ProximityBlockScript>().tagToDamage = "playerOne";
			proximityBox.GetComponent<ProximityBlockScript>().tagToDamage = "playerOne";
		}
	}
}
