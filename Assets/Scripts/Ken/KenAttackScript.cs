using UnityEngine;
using System.Collections;

public class KenAttackScript : MonoBehaviour {

	InputScript inputScript;
	SpriteAnimator spriteAnimator;
	FighterStateMachineScript state;
	PlayerMovementScript PMS;
	public GameObject lightHitbox, mediumHitbox, heavyHitbox,heavyHitboxP2, jumpLightHitbox,jumpLightHitbox2, jumpMediumHitbox, jumpHeavyHitbox,
	superHitbox, superHitbox2, throwHitbox, proximityBox;
	TimeManagerScript timeManager;

	public GameObject ThrowPoint;
	public DogAttackScript Komaru;

	bool  mediumBuffer = false, sp2Buffer = false, lightBuffer = false, sp1Buffer = false;
	bool lightHitboxHit = false, mediumHitboxHit = false, heavyHitboxHit = false;
	HealthScript health;
	Transform otherPlayer;
	// Use this for initialization
	void Start () {
		if (tag == "playerOne") {
			//SetPlayer (true);
			otherPlayer = GameObject.FindWithTag ("playerTwo").transform;
			Komaru.SetPlayer(true);
		} else {
			//SetPlayer (false);
			otherPlayer = GameObject.FindWithTag ("playerOne").transform;
			Komaru.SetPlayer(false);
		}

		health = GetComponentInChildren<HealthScript> ();
		health.SetHitFunc (CancelAttacks);
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

		lightHitbox.GetComponent<HitboxScript>().SetOptFunc (LightHit);
		mediumHitbox.GetComponent<HitboxScript>().SetOptFunc (MediumHit);
		heavyHitbox.GetComponent<HitboxScript>().SetOptFunc (HeavyHit);
		throwHitbox.GetComponent<HitboxScript>().SetThrowFunc (ThrowHit);


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
		state.SetState ("no dog attack");
		for (int x = 0; x < 15;) {
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
		ThrowPoint.transform.localPosition = new Vector2 (2.4f, 3);
		otherPlayer.position = ThrowPoint.transform.position;
		Vector2 endPoint = new Vector2(2.4f,3);
		for (int x = 0; x < 42;) {
			if (x == 10) {
				state.SetState ("attack");
			}
			if (!timeManager.CheckIfTimePaused()) {
				if (x < 24) {
					Vector2 newPoint = Vector2.Lerp (ThrowPoint.transform.localPosition, endPoint, .6f);
					ThrowPoint.transform.localPosition = newPoint;
					otherPlayer.position = ThrowPoint.transform.position;
				}
				if (x == 15) {
					endPoint = new Vector2 (2, 3f);
				}
				if (x == 18) {
					endPoint = new Vector2 (-.8f, 5f);
				}
				if (x == 21) {
					endPoint = new Vector2 (-2.3f,2.7f);
				}
				if (x == 24) {
					endPoint = new Vector2 (-1.8f,1.3f);
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
		for (int x = 0; x < 15;) {
			// startup
			// active

			if (!timeManager.CheckIfTimePaused()) {
				if (x == 3){
					lightBuffer = false;
				}
				if (x == 6) {
					lightHitbox.SetActive (true);
				}
				// recovery
				if (x == 8) {
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
		for (int x = 0; x < 16; ) {

			if (!timeManager.CheckIfTimePaused()) {
				if (x == 6){
					jumpLightHitbox.SetActive (true);
				}
				if (x == 9){
					jumpLightHitbox.SetActive (false);
				}
				if (x == 12) {
					jumpLightHitbox2.SetActive (true);

				}
				if (x == 15) {
					jumpLightHitbox2.SetActive (false);
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
		for (int x = 0; x < 39; ) {

			if (!timeManager.CheckIfTimePaused()) {
				if (x == 3){
					mediumBuffer = false;	
				}
				// active
				if (x == 16) {
					PMS.StopMovement ();
					mediumHitbox.SetActive (true);
				}
				// recovery
				if (x == 18){
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
				if (x == 12) {
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
		for (int x = 0; x < 60;) {
			if (!timeManager.CheckIfTimePaused()) {
				if (x == 21) {
					heavyHitbox.SetActive (true);
				}
				if (x == 24) {
					PMS.StopMovement ();
					heavyHitbox.SetActive (false);
				}
				if (x == 30) {
					heavyHitboxP2.SetActive (true);
				}
				if (x == 33) {
					heavyHitboxP2.SetActive (false);
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
				if (x == 15) {
					jumpHeavyHitbox.SetActive (true);
				}
				if (x == 18) {
					jumpHeavyHitbox.SetActive (false);
					proximityBox.SetActive (false);
				}

				x++;
			}
			yield return null;
		}
	}

	public void SpecialOne(){
		if (state.GetState() == "neutral" || state.GetState() =="light recovery"  || state.GetState() =="medium recovery"  
			|| state.GetState() =="heavy recovery" || state.GetState() == "attack") {

			StartCoroutine (SP1BufferEnum ());
		}

	}
	IEnumerator SP1BufferEnum(){
		yield return null;
		yield return null;
		yield return null;
		yield return null;
		Komaru.StartSp1 ();
	}
	public void SpecialTwo(){
		if (state.GetState() == "neutral" || state.GetState() =="light recovery"  || state.GetState() =="medium recovery"  
			|| state.GetState() =="heavy recovery" || state.GetState() == "attack") {
			StartCoroutine (SP2BufferEnum ());
		}

	}
	IEnumerator SP2BufferEnum(){
		yield return null;
		yield return null;
		yield return null;
		yield return null;
		Komaru.StartSp2 ();
	}
	public void SpecialThree(){
		if (state.GetState() == "neutral" || state.GetState() =="light recovery"  || state.GetState() =="medium recovery"  
			|| state.GetState() =="heavy recovery" || state.GetState() == "attack") {
			Komaru.StartSp3 ();
		}

	}


	public void Super(){
		if (health.exCurrent >= 1000 && ((state.GetState() == "neutral" || (state.GetState() =="light recovery" && lightHitboxHit) || 
			(state.GetState() =="medium recovery" && mediumHitboxHit) || 
			(state.GetState() =="heavy recovery" && heavyHitboxHit) || 
			mediumBuffer || sp2Buffer))) {
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
		state.SetState ("no dog attack");
		Komaru.StartSuper();

		PlayerMovementScript otherPMS = otherPlayer.GetComponent<PlayerMovementScript> ();
		for (int x = 0; x < 110;) {
			// active
			if (!timeManager.CheckIfTimePaused()) {
				if (x < 32 && (x % 2 == 0)) {
					superHitbox.SetActive (false);
					superHitbox.SetActive (true);

				}
				if (x < 32) {
					if (Mathf.Abs (otherPlayer.position.x - transform.position.x) > 2.5f) {
						if (otherPlayer.transform.position.x > transform.position.x) {
							otherPlayer.transform.Translate (new Vector3 (-.2f, 0, 0));
						} else {
							otherPlayer.transform.Translate (new Vector3 (.2f, 0, 0));			
						}
					}
				}
				if (x == 34) {
					superHitbox.SetActive (false);
					superHitbox2.SetActive (true);
				}
				if (x == 36) {
					superHitbox2.SetActive (false);
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
		heavyHitboxP2.SetActive (false);
		jumpLightHitbox.SetActive (false);
		jumpLightHitbox2.SetActive (false);
		jumpMediumHitbox.SetActive (false);
		jumpHeavyHitbox.SetActive (false);
		superHitbox.SetActive (false);
		superHitbox2.SetActive (false);

		throwHitbox.SetActive (false);
		proximityBox.SetActive (false);
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
