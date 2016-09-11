using UnityEngine;
using System.Collections;

public class YukikoAttackScript : MonoBehaviour {

	InputScript inputScript;
	SpriteAnimator spriteAnimator;
	FighterStateMachineScript state;
	PlayerMovementScript PMS;
	public GameObject fan1, fan2, sp1Fireball, sp3Trap, sp3TrapPre, medium1Hitbox,medium2Hitbox,medium3Hitbox, heavyHitbox1,heavyHitbox2,heavyHitbox3, jumpMediumHitbox, jumpHeavyHitbox,
	sp2Hitbox, fireballGunpoint, throwHitbox, proximityBox;

	ProjectileScript sp1FireballProjectileScript , fan1ProjectileScript, fan2ProjectileScript;
	TimeManagerScript timeManager;

	SoundsPlayer sounds;
	public HitboxScript lightHitboxScript, mediumHitboxScript, heavyHitboxScript, throwHitboxScript;
	public bool lightHitboxHit= false, mediumHitboxHit= false, heavyHitboxHit = false, specialHitboxHit = false;
	public GameObject ThrowPoint;

	bool  mediumBuffer = false, sp2Buffer = false, lightBuffer = false, sp1Buffer = false;
	public bool trapActive= false;
	HealthScript health;
	Transform otherPlayer;
	public PersonaAttackAnimScript persona;
	int attackStage =0;
	public delegate void vDel();
	// Use this for initialization
	void Start () {
		if (tag == "playerOne") {
			otherPlayer = GameObject.FindWithTag ("playerTwo").transform;
			persona.SetOtherPlayer (true);
		} else {
			otherPlayer = GameObject.FindWithTag ("playerOne").transform;
			persona.SetOtherPlayer (false);
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
		inputScript.assignRT (SpecialThree, TrapDetonate);

		inputScript.assignBYButton (Super);

//		lightHitboxScript.SetOptFunc (LightHit);
		medium1Hitbox.GetComponent<HitboxScript>().SetOptFunc (MediumHit);
		medium2Hitbox.GetComponent<HitboxScript>().SetOptFunc (MediumHit);
		medium3Hitbox.GetComponent<HitboxScript>().SetOptFunc (MediumHit);
		heavyHitbox1.GetComponent<HitboxScript>().SetOptFunc (HeavyHit);
		heavyHitbox2.GetComponent<HitboxScript>().SetOptFunc (HeavyHit);
		heavyHitbox3.GetComponent<HitboxScript>().SetOptFunc (HeavyHit);
		heavyHitboxScript.SetOptFunc (HeavyHit);
		throwHitboxScript.SetThrowFunc (ThrowHit);
		sp1Fireball.GetComponentInChildren<HitboxScript> ().SetOptFunc (SpecialHit);
		sp2Hitbox.GetComponent<HitboxScript> ().SetOptFunc (SpecialHit);
		sp3Trap.GetComponentInChildren<HitboxScript> ().SetOptFunc (SpecialHit);


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
		for (int x = 0; x < 36;) {

			if (!timeManager.CheckIfTimePaused()) {
				if (x == 12) {

					otherPlayer.GetComponent<PlayerMovementScript> ().MoveToward (-10);
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
		} else if (state.GetState () == "light recovery"){
			PMS.CheckFacing ();
			StopAllCoroutines ();
			StartCoroutine (light2Enum ());
		}else if (state.GetState () == "jumping") {
			StartCoroutine (jumpLightEnum ());
		}

	}
	IEnumerator lightEnum(){
		health.AddMeter (10);
		lightBuffer = true;
		sounds.PlayLight ();
		PMS.StopMovement ();
		spriteAnimator.PlayLight ();
		state.SetState ("attack");
		for (int x = 0; x < 45;) {
			// startup
			// active

			if (!timeManager.CheckIfTimePaused()) {
				if (x == 3){
					lightBuffer = false;
				}

				// recovery
				if (x == 11) {
					fan1.transform.position = fireballGunpoint.transform.position;
					if (PMS.CheckIfOnLeft ()) {
						fan1.transform.eulerAngles = new Vector2(0, 0);
						fan1ProjectileScript.direction = new Vector2 (1, 0);
					} else {
						fan1.transform.eulerAngles = new Vector2(0, 180);
						fan1ProjectileScript.direction = new Vector2 (-1, 0);

					}
					fan1.SetActive (true);
					state.SetState ("light recovery");
				}
				if (x == 20) {
					state.SetState ("attack");
				}
				x++;
			}
			yield return null;
		}
		lightHitboxHit = false;
		state.SetState ("neutral");
	}
	IEnumerator light2Enum(){
		spriteAnimator.PlayExtra1 ();
		PMS.StopMovement ();
		state.SetState ("attack");
		sounds.PlayMedium ();
		for (int x = 0; x < 42;) {
			if (!timeManager.CheckIfTimePaused()) {
				if (x == 6) {
					fan2.transform.position = fireballGunpoint.transform.position;
					if (PMS.CheckIfOnLeft ()) {
						fan2.transform.eulerAngles = new Vector2(0, 0);
						fan2ProjectileScript.direction = new Vector2 (1, 0);
					} else {
						fan2.transform.eulerAngles = new Vector2(0, 180);
						fan2ProjectileScript.direction = new Vector2 (-1, 0);
					}
					fan2.SetActive (true);
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
				if (x == 10){
					fan2.transform.position = new Vector3( fireballGunpoint.transform.position.x, fireballGunpoint.transform.position.y-.5f, fireballGunpoint.transform.position.z);
					if (PMS.CheckIfOnLeft ()) {
						fan2.transform.eulerAngles = new Vector3(0, 0, -45);
						fan2ProjectileScript.direction = new Vector2 (.75f, -1f);
					} else {
						fan2.transform.eulerAngles = new Vector3(0, 180, -45);
						fan2ProjectileScript.direction = new Vector2 (-.75f, -1f);

					}
					fan2.SetActive (true);
				}
				x++;
			}
			yield return null;
		}
	}
	public void Medium(){
		if (state.GetState() == "neutral") {
			PMS.CheckFacing ();
			StopAllCoroutines ();
			StartCoroutine (medium1Enum ());
		}else if(state.GetState() == "medium recovery" && attackStage == 1 && mediumHitboxHit){
			PMS.CheckFacing ();
			mediumHitboxHit = false;
			StopAllCoroutines ();
			StartCoroutine (medium2Enum ());
		}else if(state.GetState() == "medium recovery" && attackStage == 2 && mediumHitboxHit){
			PMS.CheckFacing ();
			mediumHitboxHit = false;
			StopAllCoroutines ();
			StartCoroutine (medium3Enum ());
		}else if (state.GetState () == "jumping") {
			StartCoroutine (jumpMediumEnum ());
		}

	}
	IEnumerator medium1Enum(){
		health.AddMeter (15);
		mediumBuffer = true;
		proximityBox.SetActive (true);
		spriteAnimator.PlayMedium ();
		state.SetState ("attack");
		// startup
		PMS.MoveToward(5, 0);
		for (int x = 0; x < 27; ) {

			if (!timeManager.CheckIfTimePaused()) {
				if (x == 3){
					mediumBuffer = false;	
				}
				// active
				if (x == 9) {
					PMS.StopMovement ();
					medium1Hitbox.SetActive (true);
				}
				// recovery
				if (x == 11){
					medium1Hitbox.SetActive (false);
					attackStage = 1;
					state.SetState ("medium recovery");
					PMS.StopMovement ();
					proximityBox.SetActive (false);
				}
				x++;
			}

			yield return null;
		}
		mediumHitboxHit = false;
		state.SetState ("neutral");
	}
	IEnumerator medium2Enum(){
		health.AddMeter (15);
		mediumBuffer = true;
		proximityBox.SetActive (true);
		spriteAnimator.PlayExtra2 ();
		state.SetState ("attack");
		// startup
		PMS.MoveToward(5, 0);

		//ActivatePersona(persona.StartMediumAnim);
		for (int x = 0; x < 33; ) {

			if (!timeManager.CheckIfTimePaused()) {
				if (x == 3){
					mediumBuffer = false;	
				}
				// active
				if (x == 12) {
					PMS.StopMovement ();
					medium2Hitbox.SetActive (true);
				}
				// recovery
				if (x == 14){
					medium2Hitbox.SetActive (false);
					attackStage = 2;
					state.SetState ("medium recovery");
					PMS.StopMovement ();
					proximityBox.SetActive (false);
				}
				x++;
			}

			yield return null;
		}
		mediumHitboxHit = false;
		state.SetState ("neutral");
	}
	IEnumerator medium3Enum(){
		health.AddMeter (15);
		mediumBuffer = true;
		proximityBox.SetActive (true);
		spriteAnimator.PlayExtra3 ();
		state.SetState ("attack");
		// startup
		PMS.MoveToward(5, 0);

		//ActivatePersona(persona.StartMediumAnim);
		for (int x = 0; x < 39; ) {

			attackStage = 0;
			if (!timeManager.CheckIfTimePaused()) {
				if (x == 3){
					mediumBuffer = false;	
				}
				// active
				if (x == 12) {
					PMS.StopMovement ();
					medium3Hitbox.SetActive (true);
				}
				// recovery
				if (x == 15){
					medium3Hitbox.SetActive (false);
					state.SetState ("medium recovery");
					PMS.StopMovement ();
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
	public void Heavy(){
		if ((state.GetState() == "attack" || state.GetState() == "neutral" || state.GetState() == "light recovery" || state.GetState() == "medium recovery") && persona.GetAttackState() != -1 && persona.CheckAlive()) {
//			if (persona.GetAttackState () == 0 && state.GetState() == "neutral") {
//				PMS.CheckFacing ();
//				StopAllCoroutines ();
//				StartCoroutine (heavyEnum ());
//			} else if (persona.GetAttackState () != 0){
			if (!persona.isActive){
				sounds.PlayHeavy ();
			}
				ActivatePersona(persona.StartAttacksAnim);
//			}
//		} else if (state.GetState () == "jumping" && persona.CheckAlive()) {
//			StartCoroutine (jumpHeavyEnum ());
		}

	}
	IEnumerator heavyEnum(){

		health.AddMeter (20);
		proximityBox.SetActive (true);
		spriteAnimator.PlayHeavy ();
		
		PMS.StopMovement ();
		state.SetState ("heavy recovery");
		ActivatePersona(persona.StartAttacksAnim);
		for (int x = 0; x < 30;) {
			if (!timeManager.CheckIfTimePaused()) {
				if (x == 7) {

					state.SetState ("attack");

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

		ActivatePersona (persona.StartJumpMediumAnim );
		for (int x = 0; x <  27;) {
			if (!timeManager.CheckIfTimePaused()) {
				

				x++;
			}
			yield return null;
		}
	}

	public void SpecialOne(){
		if ((state.GetState() == "neutral" || (state.GetState() =="medium recovery" && mediumHitboxHit) 
			|| (state.GetState() =="heavy recovery"&& heavyHitboxHit)) && !sp1Fireball.activeSelf && persona.CheckAlive() && persona.GetAttackState() != -2) {
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

					ActivatePersona(persona.StartSpecialOneAnim);
					proximityBox.SetActive (false);
					canShoot = false;
				}
				x++;
			}
			yield return null;

		}
		specialHitboxHit = false;
		state.SetState ("neutral");
	}
	public void SpecialTwo(){
		if ((state.GetState() == "neutral" ||  (state.GetState() =="medium recovery" && mediumHitboxHit) 
			|| (state.GetState() =="heavy recovery"&& heavyHitboxHit)) && persona.CheckAlive() && persona.GetAttackState() != -2){
			lightHitboxHit = false;
			mediumHitboxHit = false;
			heavyHitboxHit = false;
			PMS.CheckFacing ();
			StopAllCoroutines ();
			if (!persona.CheckActive ()) {
				StartCoroutine (SpecialTwoEnum ());
			} else {

				StartCoroutine (SpecialTwoReturnEnum ());
			}
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
		//ActivatePersona(persona.StartSpecialTwoAnim);

		for (int x = 0; x < 50;) {

			if (!timeManager.CheckIfTimePaused ()) {
				if (x == 3) {
					state.SetState ("attack");
				}
				if (x == 3) {
					sp2Buffer = false;
					//PMS.StopMovement ();
					//sp2HitboxPart1.SetActive (true);
				}

				if (x == 10) {
					state.SetState ("attack");
					ActivatePersona (persona.StartSpecialTwoAnim);
				}


			


				if (x == 14) {
					proximityBox.SetActive (false);
				}
				x++;
			}
			yield return null;
		}
		state.SetState ("neutral");
		PMS.EnableBodyBox ();
	}
	IEnumerator SpecialTwoReturnEnum(){

		health.AddMeter (30);
		proximityBox.SetActive (true);
		spriteAnimator.PlaySpecialTwo ();
		PMS.StopMovement ();
		sp2Buffer = true;
		//state.SetState ("attack");
		// needs jumpbox turned off

		state.SetState ("attack");
		persona.UnSummon ();

		for (int x = 0; x < 30;) {

			if (!timeManager.CheckIfTimePaused()) {
				x++;
			}
			yield return null;
		}
		state.SetState ("neutral");
		PMS.EnableBodyBox ();
	}
	public void SpecialThree(){
		if ((state.GetState() == "neutral" || (state.GetState() =="medium recovery" && mediumHitboxHit) 
			|| (state.GetState() =="heavy recovery" )) && persona.CheckAlive() && persona.GetAttackState() != -2) {
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

		ActivatePersona(persona.StartSpecialThreeAnim);


		for (int x = 0; x < 25;) {

			if (!timeManager.CheckIfTimePaused()) {
				if (x == 15){

					proximityBox.SetActive (false);
				}

				x++;
			}

			yield return null;
		}
		PMS.StopMovement ();
		specialHitboxHit = false;
		PMS.StopMovement ();
		state.SetState ("neutral");
	}
	public void SetTrap(){
		trapActive = true;
		if (!inputScript.CheckRTRelease()) {
			Invoke("TrapDetonate", .1f);
		}
	}
	public void TrapDetonate(){
		if (trapActive) {
			trapActive = false;
			sp3TrapPre.SetActive (false);
			sp3Trap.SetActive (true);
		}
	}

	public void Super(){
		if (health.exCurrent >= 1000 && ((state.GetState() == "neutral" || 
			(state.GetState() =="medium recovery" && mediumHitboxHit) || 
			(state.GetState() =="heavy recovery" && heavyHitboxHit) || 
			(specialHitboxHit)) || mediumBuffer || sp2Buffer) && persona.CheckAlive()) {
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
		for (int x = 0; x < 45;) {
			// active

			if (!timeManager.CheckIfTimePaused()) {
				if (x == 12) {
					sounds.PlayExtra ();
					ActivatePersona (persona.StartSuperAnim);
				}
				x++;
			}
			yield return null;

		}
		proximityBox.SetActive (false);
		state.SetState ("neutral");
	}
	void ActivatePersona(vDel move){
		if (!persona.CheckActive()) {
			persona.transform.position = new Vector3 (transform.position.x, transform.position.y + 2f, transform.position.z);
			persona.gameObject.SetActive (true);
			move ();
			persona.Summon ();

		} else {
			persona.transform.position = new Vector3 (persona.transform.position.x, transform.position.y + 2.5f, transform.position.z);
			move ();
		}
	}
	public void LightHit(){
		lightHitboxHit = true;
	}
	public void MediumHit(){
		mediumHitboxHit = true;
	}
	public void HeavyHit(){
		Debug.Log ("heavy");
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
		medium1Hitbox.SetActive (false);
		medium2Hitbox.SetActive (false);
		medium3Hitbox.SetActive (false);
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
