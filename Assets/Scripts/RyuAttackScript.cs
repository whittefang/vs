using UnityEngine;
using System.Collections;

public class RyuAttackScript : MonoBehaviour {

	InputScript inputScript;
	SpriteAnimator spriteAnimator;
	FighterStateMachineScript state;
	PlayerMovementScript PMS;
	public GameObject fireball, lightHitbox, mediumHitbox, heavyHitbox, jumpLightHitbox, jumpMediumHitbox, jumpHeavyHitbox,
					sp1Hitbox, sp2HitboxPart1, sp2HitboxPart2, sp3Hitbox, fireballGunpoint, throwHitbox;
	ProjectileScript fireballProjectileScript;
	TimeManagerScript timeManager;

	SoundsPlayer sounds;
	public HitboxScript lightHitboxScript, mediumHitboxScript, heavyHitboxScript, throwHitboxScript;
	public bool lightHitboxHit= false, mediumHitboxHit= false, heavyHitboxHit = false;
	public GameObject ThrowPoint;

	Transform otherPlayer;
	// Use this for initialization
	void Start () {
		if (tag == "playerOne") {
			SetPlayer (true);
		} else {
			SetPlayer (false);
		}
			
		GetComponentInChildren<HealthScript> ().SetHitFunc (CancelAttacks);
		sounds = GetComponent<SoundsPlayer>();
		state = GetComponent<FighterStateMachineScript>();
		timeManager = GameObject.Find ("MasterGameObject").GetComponent<TimeManagerScript> ();
		spriteAnimator = GetComponent<SpriteAnimator> ();
		fireballProjectileScript = fireball.GetComponent < ProjectileScript> ();
		PMS = GetComponent<PlayerMovementScript> ();
		inputScript = GetComponent<InputScript> ();
		inputScript.assignAXButton (Throw);
		inputScript.assignXButton (Light, null);
		inputScript.assignYButton (Medium, null);
		inputScript.assignRBButton (Heavy, null);

		inputScript.assignAButton (SpecialOne, null);
		inputScript.assignBButton (SpecialTwo, null);
		inputScript.assignRT (SpecialThree, null);

		lightHitboxScript.SetOptFunc (LightHit);
		mediumHitboxScript.SetOptFunc (MediumHit);
		heavyHitboxScript.SetOptFunc (HeavyHit);
		throwHitboxScript.SetThrowFunc (ThrowHit);
		PMS.setAttackCancel (CancelAttacks);
	}

	public void Throw(){
		if (state.GetState () == "neutral") {
			StartCoroutine (ThrowEnum ());
		}
	}
	IEnumerator ThrowEnum(){
		spriteAnimator.PlayThrowTry ();
		PMS.StopMovement ();
		state.SetState ("attack");
		for (int x = 0; x < 10;) {
			// startup
			// active
			if (x == 6) {
				throwHitbox.SetActive (true);
			}
			// recovery
			if (x == 8) {
				throwHitbox.SetActive (false);
			}

			yield return null;
			if (!timeManager.CheckIfTimePaused()) {
				x++;
			}
		}
		state.SetState ("neutral");
	}
	void ThrowHit(Transform otherPlayerTmp){
		otherPlayer = otherPlayerTmp;
		CancelAttacks ();
		StartCoroutine (ThrowHitEnum());
	}
	IEnumerator ThrowHitEnum(){
		
		spriteAnimator.PlayThrowComplete ();
		PMS.DsableBodyBox ();
		ThrowPoint.transform.localPosition = new Vector2 (-1, 0);
		otherPlayer.position = ThrowPoint.transform.position;
		for (int x = 0; x < 42;) {
			if (x == 6) {
				ThrowPoint.transform.localPosition = new Vector2 (-2, 0);
				otherPlayer.position = ThrowPoint.transform.position;
			}
			if (x == 9) {
				ThrowPoint.transform.localPosition = new Vector2 (-3f, 0);
				otherPlayer.position = ThrowPoint.transform.position;
			}
			if (x == 12) {
				ThrowPoint.transform.localPosition = new Vector2 (-3, 0);
				otherPlayer.position = ThrowPoint.transform.position;
			}
			if (x == 15) {
				ThrowPoint.transform.localPosition = new Vector2 (-3, .75f);
				otherPlayer.position = ThrowPoint.transform.position;
			}
			if (x == 18) {
				ThrowPoint.transform.localPosition = new Vector2 (-2f, .5f);
				otherPlayer.position = ThrowPoint.transform.position;
			}
			if (x == 21) {
				ThrowPoint.transform.localPosition = new Vector2 (1,0f);
				otherPlayer.position = ThrowPoint.transform.position;
			}
			if (x == 24) {
				ThrowPoint.transform.localPosition = new Vector2 (1,-1f);
				otherPlayer.position = ThrowPoint.transform.position;
			}
			yield return null;
			if (!timeManager.CheckIfTimePaused()) {
				x++;
			}
		}
		PMS.EnableBodyBox ();
		state.SetState ("neutral");
	}

	public void Light(){
		if (state.GetState () == "neutral") {
			StartCoroutine (lightEnum ());
		} else if (state.GetState () == "jumping") {
			StartCoroutine (jumpLightEnum ());
		}

	}
	IEnumerator lightEnum(){
		spriteAnimator.PlayLight ();
		PMS.StopMovement ();
		state.SetState ("attack");
		for (int x = 0; x < 15;) {
			// startup
			// active
			if (x == 4) {
				lightHitbox.SetActive (true);
			}
			// recovery
			if (x == 6) {
				lightHitbox.SetActive (false);
				state.SetState ("light recovery");
			}

			yield return null;
			if (!timeManager.CheckIfTimePaused()) {
				x++;
			}
		}
		lightHitboxHit = false;
		state.SetState ("neutral");
	}
	IEnumerator jumpLightEnum(){
		spriteAnimator.PlayJumpLight ();
		state.SetState ("jump attack");
		for (int x = 0; x < 15; ) {
			if (x == 4){
				jumpLightHitbox.SetActive (true);
			}
			if (x == 10){
				jumpLightHitbox.SetActive (false);
			}
			yield return null;
			if (!timeManager.CheckIfTimePaused()) {
				x++;
			}
		}
	}
	public void Medium(){
		if (state.GetState() == "neutral" || (state.GetState() =="light recovery" && lightHitboxHit)) {
			if (lightHitboxHit) {
				lightHitboxHit = false;
			}
			StopAllCoroutines ();
			StartCoroutine (mediumEnum ());
		} else if (state.GetState () == "jumping") {
			StartCoroutine (jumpMediumEnum ());
		}

	}
	IEnumerator mediumEnum(){
		spriteAnimator.PlayMedium ();
		PMS.StopMovement ();
		state.SetState ("attack");
		// startup
		for (int x = 0; x < 27; ) {

			// active
			if (x == 9) {
				mediumHitbox.SetActive (true);
			}
			// recovery
			if (x == 11){
				mediumHitbox.SetActive (false);
				state.SetState ("medium recovery");
			}
			yield return null;
			if (!timeManager.CheckIfTimePaused()) {
				x++;
			}
		}
		mediumHitboxHit = false;
		state.SetState ("neutral");
	}
	IEnumerator jumpMediumEnum(){
		spriteAnimator.PlayJumpMedium ();
		state.SetState ("jump attack");
		for (int x = 0; x < 27;) {
			if (x == 9) {
				jumpMediumHitbox.SetActive (true);
			}
			if (x == 20) {
				jumpMediumHitbox.SetActive (false);
			}
			yield return null;
			if (!timeManager.CheckIfTimePaused()) {
				x++;
			}
		}
	}
	public void Heavy(){
		if (state.GetState() == "neutral" || (state.GetState() =="medium recovery" && mediumHitboxHit)) {
			if (mediumHitboxHit) {
				mediumHitboxHit = false;
			}
			StopAllCoroutines ();
			StartCoroutine (heavyEnum ());
		} else if (state.GetState () == "jumping") {
			StartCoroutine (jumpHeavyEnum ());
		}

	}
	IEnumerator heavyEnum(){
		spriteAnimator.PlayHeavy ();
		PMS.StopMovement ();
		state.SetState ("attack");
		PMS.MoveToward (10);
		for (int x = 0; x < 42;) {
			if (x == 18) {
				heavyHitbox.SetActive (true);
			}

			if (x == 22) {
				PMS.StopMovement ();
				heavyHitbox.SetActive (false);
				state.SetState ("heavy recovery");
			}

			yield return null;
			if (!timeManager.CheckIfTimePaused()) {
				x++;
			}
		}
		heavyHitboxHit = false;
		state.SetState ("neutral");
	}
	IEnumerator jumpHeavyEnum(){
		spriteAnimator.PlayJumpHeavy ();
		state.SetState ("jump attack");
		for (int x = 0; x < 21;) {
			if (x == 7) {
				jumpHeavyHitbox.SetActive (true);
			}
			if (x == 15) {
				jumpHeavyHitbox.SetActive (false);
			}

			yield return null;
			if (!timeManager.CheckIfTimePaused()) {
				x++;
			}
		}
	}

	public void SpecialOne(){
		if ((state.GetState() == "neutral" || (state.GetState() =="light recovery" && lightHitboxHit) || (state.GetState() =="medium recovery" && mediumHitboxHit) || (state.GetState() =="heavy recovery" && heavyHitboxHit)) && !fireball.activeSelf) {
			lightHitboxHit = false;
			mediumHitboxHit = false;
			heavyHitboxHit = false;
			StopAllCoroutines ();
			StartCoroutine (SpecialOneEnum ());
		}

	}
	IEnumerator SpecialOneEnum(){
		spriteAnimator.PlaySpecialOne ();
		PMS.StopMovement ();
		state.SetState ("attack");
		bool canShoot = true;
		for (int x = 0; x < 26;) {
			// active
			if (x == 12 && canShoot) {
				canShoot = false;
				sounds.PlayExtra ();
				fireball.transform.position = fireballGunpoint.transform.position;
				if (PMS.CheckIfOnLeft ()) {
					fireball.GetComponentInChildren<SpriteRenderer> ().flipX = true;
					fireballProjectileScript.direction = new Vector2 (1, 0);
				} else {
					fireball.GetComponentInChildren<SpriteRenderer> ().flipX = false;
					fireballProjectileScript.direction = new Vector2 (-1, 0);
		
				}
				fireball.SetActive (true);
			}
			yield return null;
			if (!timeManager.CheckIfTimePaused()) {
				x++;
			}

		}
		state.SetState ("neutral");
	}
	public void SpecialTwo(){
		if (state.GetState() == "neutral" || state.GetState() =="light recovery" || state.GetState() =="medium recovery" || state.GetState() =="heavy recovery"){
			lightHitboxHit = false;
			mediumHitboxHit = false;
			heavyHitboxHit = false;
			StopAllCoroutines ();
			StartCoroutine (SpecialTwoEnum ());
		}

	}
	IEnumerator SpecialTwoEnum(){
		spriteAnimator.PlaySpecialTwo ();
		PMS.StopMovement ();

		//state.SetState ("attack");
		// needs jumpbox turned off
		state.SetState ("invincible");
		for (int x = 0; x < 60;) {
			if (x == 3) {
				
				sp2HitboxPart1.SetActive (true);
			}

			if (x == 14) {
				PMS.DsableBodyBox ();
				PMS.MoveToward (5, 25);
			}

			if (x == 15){
				state.SetState ("attack");
				sp2HitboxPart1.SetActive (false);
				sp2HitboxPart2.SetActive (true);
			}
			if (x == 30) {
				sp2HitboxPart2.SetActive (false);
			}

			yield return null;
			if (!timeManager.CheckIfTimePaused()) {
				x++;
			}
		}
		state.SetState ("neutral");
		PMS.EnableBodyBox ();
	}
	public void SpecialThree(){
		if (state.GetState() == "neutral" || state.GetState() == "light recovery" || state.GetState() =="medium recovery" || state.GetState() =="heavy recovery") {
			lightHitboxHit = false;
			mediumHitboxHit = false;
			heavyHitboxHit = false;
			StopAllCoroutines ();
			StartCoroutine (SpecialthreeEnum ());
		}

	}
	IEnumerator SpecialthreeEnum(){
		spriteAnimator.PlaySpecialThree ();
		PMS.StopMovement ();
		state.SetState ("attack");
		for (int x = 0; x < 6;) {
			yield return null;
			if (!timeManager.CheckIfTimePaused()) {
				x++;
			}
		}
		for (int x = 0; x < 80;) {
			if (x == 9) {
				sp3Hitbox.SetActive(true);
			}
			if (x == 12) {
				sp3Hitbox.SetActive(false);
			}
			if (x == 18) {
				sp3Hitbox.SetActive(true);
			}
			if (x == 21) {
				sp3Hitbox.SetActive(false);
			}
			if (x == 33) {
				sp3Hitbox.SetActive(true);
			}
			if (x == 35) {
				sp3Hitbox.SetActive(false);
			}
			if (x == 42) {
				sp3Hitbox.SetActive(true);
			}
			if (x == 45) {
				sp3Hitbox.SetActive(false);
			}
			if (x == 59) {
				sp3Hitbox.SetActive(true);
			}
			if (x == 61) {
				sp3Hitbox.SetActive(false);
			}
			if (x == 66) {
				sp3Hitbox.SetActive(true);
			}
			if (x == 68) {
				sp3Hitbox.SetActive(false);
			}
			PMS.MoveToward (5);
			yield return null;
			if (!timeManager.CheckIfTimePaused()) {
				x++;
			}
		}
		PMS.StopMovement ();
			for (int x = 0; x < 10;) {
			yield return null;
			if (!timeManager.CheckIfTimePaused()) {
				x++;
			}
		}
		PMS.StopMovement ();
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
	public void CancelAttacks(){
		StopAllCoroutines ();
		lightHitbox.SetActive (false);
		mediumHitbox.SetActive (false);
		heavyHitbox.SetActive (false);
		jumpLightHitbox.SetActive (false);
		jumpMediumHitbox.SetActive (false);
		jumpHeavyHitbox.SetActive (false);
		sp2HitboxPart1.SetActive (false);
		sp2HitboxPart2.SetActive (false);
		sp3Hitbox.SetActive (false);
		throwHitbox.SetActive (false);
	}

	public void SetPlayer(bool playerOne){
		if (playerOne) {
			jumpLightHitbox.GetComponent<HitboxScript>().AddTagToDamage("playerTwoHurtbox");
			jumpMediumHitbox.GetComponent<HitboxScript>().AddTagToDamage("playerTwoHurtbox");
			jumpHeavyHitbox.GetComponent<HitboxScript>().AddTagToDamage("playerTwoHurtbox");
			lightHitbox.GetComponent<HitboxScript>().AddTagToDamage("playerTwoHurtbox");
			mediumHitbox.GetComponent<HitboxScript>().AddTagToDamage("playerTwoHurtbox");
			heavyHitbox.GetComponent<HitboxScript>().AddTagToDamage("playerTwoHurtbox");
			//sp1Hitbox.GetComponent<HitboxScript>().AddTagToDamage("playerTwoHurtbox");
			sp2HitboxPart1.GetComponent<HitboxScript>().AddTagToDamage("playerTwoHurtbox");
			sp2HitboxPart2.GetComponent<HitboxScript>().AddTagToDamage("playerTwoHurtbox");
			sp3Hitbox.GetComponent<HitboxScript>().AddTagToDamage("playerTwoHurtbox");
			fireball.GetComponent<HitboxScript>().AddTagToDamage("playerTwoHurtbox");

		} else {
			jumpLightHitbox.GetComponent<HitboxScript>().AddTagToDamage("playerOneHurtbox");
			jumpMediumHitbox.GetComponent<HitboxScript>().AddTagToDamage("playerOneHurtbox");
			jumpHeavyHitbox.GetComponent<HitboxScript>().AddTagToDamage("playerOneHurtbox");
			lightHitbox.GetComponent<HitboxScript>().AddTagToDamage("playerOneHurtbox");
			mediumHitbox.GetComponent<HitboxScript>().AddTagToDamage("playerOneHurtbox");
			heavyHitbox.GetComponent<HitboxScript>().AddTagToDamage("playerOneHurtbox");
			//sp1Hitbox.GetComponent<HitboxScript>().AddTagToDamage("playerOneHurtbox");
			sp2HitboxPart1.GetComponent<HitboxScript>().AddTagToDamage("playerOneHurtbox");
			sp2HitboxPart2.GetComponent<HitboxScript>().AddTagToDamage("playerOneHurtbox");
			sp3Hitbox.GetComponent<HitboxScript>().AddTagToDamage("playerOneHurtbox");
			fireball.GetComponent<HitboxScript>().AddTagToDamage("playerOneHurtbox");
		}
	}
}
