using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BaikenAttackScript : MonoBehaviour {

	InputScript inputScript;
	SpriteAnimator spriteAnimator;
	FighterStateMachineScript state;
	PlayerMovementScript PMS;
	public GameObject lightHitbox, mediumHitbox, mediumHitbox2, heavyHitbox, jumpLightHitbox, jumpMediumHitbox, jumpHeavyHitbox,
	sp1Hitbox, sp2Hitbox, sp2ParryBox, sp3HookHitbox, sp3Hook,sp3hitboxComplete, sp3hitboxComplete2,fireballGunpoint,
	superHitbox,superHitbox2, throwHitbox, proximityBox;
	public GameObject jumpMediumEffect, sp1Effect;

	ProjectileScript fireballProjectileScript , superProjectileScript;
	TimeManagerScript timeManager;

	SoundsPlayer sounds;
	public HitboxScript lightHitboxScript, mediumHitboxScript, heavyHitboxScript, throwHitboxScript;
	public bool lightHitboxHit= false, mediumHitboxHit= false, heavyHitboxHit = false, specialHitboxHit = false;
	public GameObject ThrowPoint, superKanji;

	bool  mediumBuffer = false, sp2Buffer = false, lightBuffer = false, sp1Buffer = false, isCountering = false;
	HealthScript health;
	Transform otherPlayer;
	bool hitChain = false;
	List<GameObject> chainLinks;
	public GameObject linkPrefab;
	float chainGrabDistance = 6;
	// Use this for initialization
	void Start () {
		if (tag == "playerOne") {
			otherPlayer = GameObject.FindWithTag ("playerTwo").transform;
		} else {
			otherPlayer = GameObject.FindWithTag ("playerOne").transform;
		}

		health = GetComponentInChildren<HealthScript> ();
		health.SetHitFunc (CancelAttacks);
		sounds = GetComponent<SoundsPlayer>();
		state = GetComponent<FighterStateMachineScript>();
		timeManager = GameObject.Find ("MasterGameObject").GetComponent<TimeManagerScript> ();
		spriteAnimator = GetComponent<SpriteAnimator> ();
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
		
		GetComponentInChildren<HealthScript>().SetParryFunc(SpecialTwoComplete);

		lightHitbox.GetComponent<HitboxScript>().SetOptFunc (LightHit);
		mediumHitbox.GetComponent<HitboxScript>().SetOptFunc (MediumHit);
		heavyHitbox.GetComponent<HitboxScript>().SetOptFunc (HeavyHit);
		throwHitbox.GetComponent<HitboxScript>().SetThrowFunc (ThrowHit);
		sp1Hitbox.GetComponent<HitboxScript> ().SetOptFunc (SpecialHit);
		sp3HookHitbox.GetComponentInChildren<HitboxScript> (true).SetOptFunc (ChainHit);
		int max = (int)(chainGrabDistance / .53f);
		chainLinks = new List<GameObject> ();
		for (int i = 0; i < max; i++) {
			chainLinks.Add (Instantiate (linkPrefab) as GameObject);
			chainLinks [i].SetActive (false);
		}

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
		PMS.DsableBodyBox ();
		for (int x = 0; x < 60;) {

			if (!timeManager.CheckIfTimePaused()) {

				x++;
			}

			yield return null;
		}
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
		for (int x = 0; x < 18;) {
			// startup
			// active

			if (!timeManager.CheckIfTimePaused()) {
				if (x == 3){
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
		proximityBox.SetActive (true);
		spriteAnimator.PlayJumpLight ();
		state.SetState ("jump attack");
		for (int x = 0; x < 26; ) {

			if (!timeManager.CheckIfTimePaused()) {
				if (x == 15){
					jumpLightHitbox.SetActive (true);
				}
				if (x == 25){
					jumpLightHitbox.SetActive (false);
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
					PMS.MoveToward (15, 0);
				}
				// active
				if (x == 9) {
					mediumHitbox.SetActive (true);
				}
				if (x == 12) {

					PMS.StopMovement ();
					mediumHitbox.SetActive (false);
					state.SetState ("medium recovery");
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
					PMS.MoveToward (-5, 10);
				}
				if (x == 12) {
					jumpMediumEffect.SetActive (true);
				}
				if (x == 15) {
					jumpMediumHitbox.SetActive (true);
				}
				if (x == 17) {
					jumpMediumHitbox.SetActive (false);
					proximityBox.SetActive (false);
					PMS.MoveToward (-10, 0);
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
			CancelAttacks ();
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
				if (x == 15) {
					heavyHitbox.SetActive (true);
				}

				if (x == 17) {
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
				if (x == 12) {
					jumpHeavyHitbox.transform.localPosition = new Vector3 (-.75f, -1.3f, 0);
					jumpHeavyHitbox.SetActive (true);
				}
				if (x == 15) {
					jumpHeavyHitbox.transform.localPosition = new Vector3 (-3.7f, -.3f, 0);
					jumpHeavyHitbox.SetActive (true);
				}
				if (x == 18) {
					jumpHeavyHitbox.transform.localPosition = new Vector3 (-4.1f, 2.8f, 0);
					jumpHeavyHitbox.SetActive (true);
				}
				if (x == 21){
					jumpHeavyHitbox.SetActive (false);
				}

				x++;
			}
			yield return null;
		}
	}

	public void SpecialOne(){
		if ((state.GetState() == "neutral" || (state.GetState() =="light recovery" && lightHitboxHit) || (state.GetState() =="medium recovery" && mediumHitboxHit) 
			|| (state.GetState() =="heavy recovery" && heavyHitboxHit))) {
			lightHitboxHit = false;
			mediumHitboxHit = false;
			heavyHitboxHit = false;
			CancelAttacks ();
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
		for (int x = 0; x < 36;) {

			if (!timeManager.CheckIfTimePaused()) {
				// active
				if (x == 3){
					sp1Buffer = false;
				}
				if (x == 15) {
					sp1Effect.SetActive (true);
				}
				if (x == 18) {
					sp1Hitbox.SetActive (true);
				}
	
				if (x == 27) {
					sp1Hitbox.SetActive (false);
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
			CancelAttacks ();
			PMS.CheckFacing ();
			StopAllCoroutines ();
			StartCoroutine (SpecialTwoEnum ());
		}

	}
	IEnumerator SpecialTwoEnum(){

		health.AddMeter (30);
	//	proximityBox.SetActive (true);
		spriteAnimator.PlaySpecialTwo ();
		PMS.StopMovement ();
		sp2Buffer = true;
		//state.SetState ("attack");
		// needs jumpbox turned off

		state.SetState ("attack");
		PMS.DsableBodyBox ();
		isCountering = false;
		for (int x = 0; x < 50;) {

			if (!timeManager.CheckIfTimePaused()) {
				if (x == 2) {
					state.SetState ("parry");
				//	sp2ParryBox.SetActive (true);
				}
				if (x == 3) {
					sp2Buffer = false;
				}
				if (x == 15) {
					state.SetState ("attack");
		//			sp2ParryBox.SetActive (false);
				}
					
				x++;
			}
			yield return null;
		}
		state.SetState ("neutral");
		PMS.EnableBodyBox ();
	}
	public void SpecialTwoComplete(){
		if (!isCountering) {
			isCountering = true;
			CancelAttacks ();
			StartCoroutine (SpecialTwoCompleteEnum ());
		}
	}
	IEnumerator SpecialTwoCompleteEnum(){

		sp2ParryBox.SetActive (false);
		health.AddMeter (30);
		proximityBox.SetActive (true);
		spriteAnimator.PlayExtra1 ();
		PMS.StopMovement ();
		//state.SetState ("attack");
		// needs jumpbox turned off
		PMS.MoveToward (5f,25f);

		state.SetState ("invincible");
		PMS.DsableBodyBox ();
		sp2Hitbox.SetActive (true);
		for (int x = 0; x < 30;) {

			if (!timeManager.CheckIfTimePaused()) {
				if (x == 2) {
					state.SetState ("invincible");
				}
				if (x == 15) {
					sp2Hitbox.SetActive (false);
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
			CancelAttacks ();
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

		// pre throw frames
		for (int x = 0; x < 16;) {
			if (!timeManager.CheckIfTimePaused ()) {
				x++;
			}
			yield return null;
		}

		hitChain = false;
		int linkSpot = 0;
		// active hook going out
		sp3HookHitbox.SetActive(true);
		sp3Hook.SetActive(true);
		sp3Hook.transform.localPosition = new Vector3 (.85f, .66f, 0);
		while (sp3Hook.transform.localPosition.x < chainGrabDistance && !hitChain) {
			chainLinks [linkSpot].transform.position = sp3Hook.transform.position;
			chainLinks [linkSpot].SetActive (true);
			linkSpot++;
			sp3Hook.transform.localPosition += new Vector3 (.53f, 0, 0);
			yield return null;
		}
		// active hook retracting
		sp3HookHitbox.SetActive(false);
		while (sp3Hook.transform.localPosition.x > .85 ) {
			sp3Hook.transform.localPosition -= new Vector3 (.53f, 0, 0);
			chainLinks [linkSpot].SetActive (false);
			linkSpot--;
			if (hitChain){
				otherPlayer.transform.position = new Vector3(sp3Hook.transform.position.x, otherPlayer.transform.position.y, 0);
			}
			yield return null;
		}
		if (linkSpot >= 0) {
			chainLinks [linkSpot].SetActive (false);
		}
		sp3Hook.SetActive(false);
		if (hitChain){

			ThrowPoint.transform.localPosition = new Vector3 (1.5f,0 , 0);
			otherPlayer.transform.position = new Vector3(ThrowPoint.transform.position.x, otherPlayer.transform.position.y, 0);
		}
		sp3Hook.SetActive(false);

		int recovery = 0;
		if (hitChain) {
			spriteAnimator.PlayExtra2 ();
			recovery = 60;
		} else {
			spriteAnimator.PlayExtra3 ();

			recovery = 27;
		}

		for (int x = 0; x < recovery;) {
			if (x == 34 && hitChain){
				sp3hitboxComplete.SetActive (true);
			}
			if (x == 36 && hitChain) {
				sp3hitboxComplete.SetActive (false);
			}
			if (x == 52 && hitChain){
				sp3hitboxComplete2.SetActive (true);
			}
			if (x == 54 && hitChain){
				sp3hitboxComplete2.SetActive (false);
			}
			if (!timeManager.CheckIfTimePaused ()) {
				x++;
			}
			yield return null;
		}
		PMS.StopMovement ();
		specialHitboxHit = false;
		proximityBox.SetActive (false);
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
		for (int x = 0; x < 120;) {
			// active
			if (x == 5){
				transform.position = new Vector3(transform.position.x,transform.position.y,1000);
			}
			if (x == 10) {
				superHitbox.SetActive (true);
				superKanji.transform.position = new Vector3(otherPlayer.transform.position.x, superKanji.transform.position.y, superKanji.transform.position.z);
				superKanji.SetActive (true);
			}
			if (x == 15){
				if (PMS.OnLeft) {
					otherPlayer.transform.position = new Vector3 (otherPlayer.transform.position.x - 2.5f, otherPlayer.transform.position.y, 0);
					transform.position = new Vector3 (otherPlayer.transform.position.x + 2.5f, transform.position.y, 0);
				} else {
					otherPlayer.transform.position =  new Vector3 (otherPlayer.transform.position.x + 2.5f, otherPlayer.transform.position.y, 0);
					transform.position = new Vector3 (otherPlayer.transform.position.x - 2.5f, transform.position.y, 0);
				}
			}
			if (x == 20) {
				superKanji.SetActive (false);
			}
			if (!timeManager.CheckIfTimePaused()) {
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
	public void ChainHit(){
		specialHitboxHit = true;
		hitChain = true;
		sp3HookHitbox.SetActive(true);

	}
	public void CancelAttacks(){
		mediumBuffer = false;
		sp2Buffer = false;
		lightBuffer = false;
		sp1Buffer = false;

		StopAllCoroutines ();
		lightHitbox.SetActive (false);
		mediumHitbox.SetActive (false);
		mediumHitbox2.SetActive (false);
		heavyHitbox.SetActive (false);
		jumpLightHitbox.SetActive (false);
		jumpMediumHitbox.SetActive (false);
		jumpHeavyHitbox.SetActive (false);
		sp1Hitbox.SetActive (false);
		sp2Hitbox.SetActive (false);
		sp2ParryBox.SetActive (false);
		sp3Hook.SetActive (false);
		superHitbox.SetActive (false);
		superHitbox2.SetActive (false);
		throwHitbox.SetActive (false);
		proximityBox.SetActive (false);

		specialHitboxHit = false;
		lightHitboxHit = false;
		mediumHitboxHit = false;
		heavyHitboxHit = false;
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
			throwHitbox.GetComponent<HitboxScript>().AddTagToDamage("playerTwoHurtbox");
			proximityBox.GetComponent<ProximityBlockScript>().tagToDamage = "playerTwo";

		} else {

			jumpLightHitbox.GetComponent<HitboxScript>().AddTagToDamage("playerOneHurtbox");
			jumpMediumHitbox.GetComponent<HitboxScript>().AddTagToDamage("playerOneHurtbox");
			jumpHeavyHitbox.GetComponent<HitboxScript>().AddTagToDamage("playerOneHurtbox");
			lightHitbox.GetComponent<HitboxScript>().AddTagToDamage("playerOneHurtbox");
			mediumHitbox.GetComponent<HitboxScript>().AddTagToDamage("playerOneHurtbox");
			heavyHitbox.GetComponent<HitboxScript>().AddTagToDamage("playerOneHurtbox");
			//sp1Hitbox.GetComponent<HitboxScript>().AddTagToDamage("playerOneHurtbox");
			throwHitbox.GetComponent<HitboxScript>().AddTagToDamage("playerOneHurtbox");

			proximityBox.GetComponent<ProximityBlockScript>().tagToDamage = "playerOne";
		}
	}
}
