using UnityEngine;
using System.Collections;

public class RyuAttackScript : MonoBehaviour {

	InputScript inputScript;
	SpriteAnimator spriteAnimator;
	FighterStateMachineScript state;
	PlayerMovementScript PMS;
	public GameObject fireball, superFireball, lightHitbox, mediumHitbox, heavyHitbox, jumpLightHitbox, jumpMediumHitbox, jumpHeavyHitbox,
					sp1Hitbox, sp2HitboxPart1, sp2HitboxPart2, sp3Hitbox, fireballGunpoint, throwHitbox, proximityBox;

	ProjectileScript fireballProjectileScript , superProjectileScript;
	TimeManagerScript timeManager;

	SoundsPlayer sounds;
	public HitboxScript lightHitboxScript, mediumHitboxScript, heavyHitboxScript, throwHitboxScript;
	public bool lightHitboxHit= false, mediumHitboxHit= false, heavyHitboxHit = false;
	public GameObject ThrowPoint;

	bool  mediumBuffer = false, sp2Buffer = false;
	HealthScript health;
	Transform otherPlayer;
	// Use this for initialization
	void Start () {
		if (tag == "playerOne") {
			SetPlayer (true);
		} else {
			SetPlayer (false);
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

		PMS.setAttackCancel (CancelAttacks);
	}

	public void Throw(){
		if (state.GetState () == "neutral") {
			StartCoroutine (ThrowEnum ());
		}
	}
	IEnumerator ThrowEnum(){
		proximityBox.SetActive (true);
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
				proximityBox.SetActive (false);
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
		health.AddMeter (10);
		proximityBox.SetActive (true);
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
				proximityBox.SetActive (false);
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
		proximityBox.SetActive (true);
		spriteAnimator.PlayJumpLight ();
		state.SetState ("jump attack");
		for (int x = 0; x < 15; ) {
			if (x == 4){
				jumpLightHitbox.SetActive (true);
			}
			if (x == 10){
				jumpLightHitbox.SetActive (false);
				proximityBox.SetActive (false);
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
		health.AddMeter (15);
		mediumBuffer = true;
		proximityBox.SetActive (true);
		spriteAnimator.PlayMedium ();
		PMS.MoveToward (5f,0);
		state.SetState ("attack");
		// startup
		for (int x = 0; x < 27; ) {
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
			yield return null;
			if (!timeManager.CheckIfTimePaused()) {
				x++;
			}
		}
		mediumHitboxHit = false;
		state.SetState ("neutral");
	}
	IEnumerator jumpMediumEnum(){
		proximityBox.SetActive (true);
		spriteAnimator.PlayJumpMedium ();
		state.SetState ("jump attack");
		for (int x = 0; x < 27;) {
			if (x == 9) {
				jumpMediumHitbox.SetActive (true);
			}
			if (x == 20) {
				jumpMediumHitbox.SetActive (false);
				proximityBox.SetActive (false);
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

		health.AddMeter (20);
		proximityBox.SetActive (true);
		spriteAnimator.PlayHeavy ();
		PMS.StopMovement ();
		state.SetState ("attack");
		PMS.MoveToward (15);
		for (int x = 0; x < 42;) {
			if (x == 18) {
				heavyHitbox.SetActive (true);
			}

			if (x == 22) {
				PMS.StopMovement ();
				heavyHitbox.SetActive (false);
				state.SetState ("heavy recovery");
				proximityBox.SetActive (false);
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
		proximityBox.SetActive (true);
		spriteAnimator.PlayJumpHeavy ();
		state.SetState ("jump attack");
		for (int x = 0; x < 21;) {
			if (x == 7) {
				jumpHeavyHitbox.SetActive (true);
			}
			if (x == 15) {
				jumpHeavyHitbox.SetActive (false);
				proximityBox.SetActive (false);
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

		health.AddMeter (30);
		proximityBox.SetActive (true);
		spriteAnimator.PlaySpecialOne ();
		PMS.StopMovement ();
		state.SetState ("attack");
		bool canShoot = true;
		for (int x = 0; x < 45;) {
			// active
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
				fireball.SetActive (true);
				proximityBox.SetActive (false);
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

		health.AddMeter (30);
		proximityBox.SetActive (true);
		spriteAnimator.PlaySpecialTwo ();
		PMS.StopMovement ();
		sp2Buffer = true;
		//state.SetState ("attack");
		// needs jumpbox turned off
		PMS.MoveToward (5f,0);

		state.SetState ("attack");
		PMS.DsableBodyBox ();
		for (int x = 0; x < 50;) {
			if (x == 2) {
				state.SetState ("invincible");
			}
			if (x == 3) {
				sp2Buffer = false;
				//PMS.StopMovement ();
				sp2HitboxPart1.SetActive (true);
			}

			if (x == 10){
				state.SetState ("attack");
			}


			if (x == 15){
				//state.SetState ("attack");
				sp2HitboxPart1.SetActive (false);
				sp2HitboxPart2.SetActive (true);
			}
			if (x == 23) {
				sp2HitboxPart2.SetActive (false);
				proximityBox.SetActive (false);
			}

			yield return null;
			if (!timeManager.CheckIfTimePaused()) {
				if (x == 14) {
					PMS.MoveToward (2.5f, 20);
				}
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
				proximityBox.SetActive (false);
			}
			yield return null;
			if (!timeManager.CheckIfTimePaused()) {

				PMS.MoveToward (7.5f);
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

	public void Super(){
		Debug.Log ("super");
		if (health.exCurrent >= 1000 && ((state.GetState() == "neutral" || (state.GetState() =="light recovery" && lightHitboxHit) || 
			(state.GetState() =="medium recovery" && mediumHitboxHit) || 
			(state.GetState() =="heavy recovery" && heavyHitboxHit)) || mediumBuffer || sp2Buffer)) {
			health.AddMeter (-1000);
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
			if (x == 12 && canShoot) {
				canShoot = false;
				sounds.PlayExtra ();
				superFireball.transform.position = fireballGunpoint.transform.position;
				if (PMS.CheckIfOnLeft ()) {
					superFireball.transform.eulerAngles = new Vector2(0, 0);
					superProjectileScript.direction = new Vector2 (1, 0);
				} else {
					superFireball.transform.eulerAngles = new Vector2(0, 180);
					superProjectileScript.direction = new Vector2 (-1, 0);
				}
				superFireball.SetActive (true);
				proximityBox.SetActive (false);
			}
			yield return null;
			if (!timeManager.CheckIfTimePaused()) {
				x++;
			}

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
	public void CancelAttacks(){
		mediumBuffer = false;
		sp2Buffer = false;
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
		proximityBox.SetActive (false);
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
			fireball.GetComponentInChildren<HitboxScript>().AddTagToDamage("playerTwoHurtbox");
			throwHitbox.GetComponent<HitboxScript>().AddTagToDamage("playerTwoHurtbox");
			superFireball.GetComponentInChildren<HitboxScript>().AddTagToDamage("playerTwoHurtbox");
			fireball.GetComponentInChildren<ProximityBlockScript>().tagToDamage = "playerTwo";
			proximityBox.GetComponent<ProximityBlockScript>().tagToDamage = "playerTwo";

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
			fireball.GetComponentInChildren<HitboxScript>().AddTagToDamage("playerOneHurtbox");
			throwHitbox.GetComponent<HitboxScript>().AddTagToDamage("playerOneHurtbox");
			superFireball.GetComponentInChildren<HitboxScript>().AddTagToDamage("playerOneHurtbox");
			fireball.GetComponentInChildren<ProximityBlockScript>().tagToDamage = "playerOne";
			proximityBox.GetComponent<ProximityBlockScript>().tagToDamage = "playerOne";
		}
	}
}
