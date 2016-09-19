using UnityEngine;
using System.Collections;

public class HealthScript : MonoBehaviour {
	public int healthAmount;
	public int healthMax = 1000;
	public int exMax = 1000;
	public int exCurrent = 0;
	public delegate void DeathEvent();
	DeathEvent DeathFunc, HitFunc, ParryFunc;
	FighterStateMachineScript state;
	SpriteAnimator spriteAnimator;
	public PlayerMovementScript PMS, otherPlayerMovementScript;
	TimeManagerScript timeManager;
	ObjectPoolScript hitsparksPool, blocksparksPool;
	public SoundsPlayer sounds;
	TextMesh comboCounterText, comboDamageText,comboCounterShadowText, comboDamageShadowText, hitText;
	public int comboCounter = 0, comboDamage = 0, freezingCounter = 0;
	float comboScaling = 1, leftBound = -10.05f, rightBound = 10.05f;
	LeftHpBarChange hpLeft;
	RightHpBarChange hpRight;
	public bool useChildSpriteRenderer = false;
	public SpriteRenderer SR;
	ExMeter exBar;
	public bool tutorialMode = false;
	// Use this for initialization
	void OnEnable () {
		if (SR == null && !useChildSpriteRenderer) {
			SR = GetComponentInParent<SpriteRenderer> ();
		}
		if (timeManager == null) {
			timeManager = GameObject.Find ("MasterGameObject").GetComponent<TimeManagerScript> ();
		}
		if (state == null) {
			state = transform.parent.GetComponent<FighterStateMachineScript> ();
		}
		if (PMS == null) {
			PMS = transform.parent.GetComponent<PlayerMovementScript> ();
		}
		if (spriteAnimator == null) {
			spriteAnimator = transform.parent.GetComponent<SpriteAnimator> ();
		}
		healthAmount = healthMax ;

	}

	public void SetPlayer(string player, GameObject otherPlayer){
		exBar = GameObject.Find("DoNotDestroy").GetComponent<ExMeter>();
		if (player == "playerOne") {
			exCurrent =	(int)exBar.GetEx(true);
			hpLeft = GameObject.Find ("LeftHpBar").GetComponentInChildren<LeftHpBarChange> ();
			hpLeft.setHpLeft (healthMax);
			hitsparksPool = GameObject.Find ("P2MasterObject").GetComponent<ObjectPoolScript> ();
			blocksparksPool = GameObject.Find ("P2BlockSparksObject").GetComponent<ObjectPoolScript> ();
			comboCounterText = GameObject.Find ("P1ComboCount").GetComponent<TextMesh> ();
			comboDamageText = GameObject.Find ("P1Damage").GetComponent<TextMesh> ();
			comboCounterShadowText = GameObject.Find ("P1ComboCountShadow").GetComponent<TextMesh> ();
			comboDamageShadowText = GameObject.Find ("P1DamageShadow").GetComponent<TextMesh> ();
			hitText = GameObject.Find ("rhit").GetComponent<TextMesh> ();
			otherPlayerMovementScript = otherPlayer.GetComponentInChildren<PlayerMovementScript> ();
		} else {
			exCurrent =	(int)exBar.GetEx(false);
			hpRight = GameObject.Find ("RightHpBar").GetComponentInChildren<RightHpBarChange> ();
			hpRight.setHpRight (healthMax);
			hitsparksPool = GameObject.Find ("P1MasterObject").GetComponent<ObjectPoolScript> ();
			blocksparksPool = GameObject.Find ("P1BlockSparksObject").GetComponent<ObjectPoolScript> ();
			comboCounterText = GameObject.Find ("P2ComboCount").GetComponent<TextMesh> ();
			comboDamageText = GameObject.Find ("P2Damage").GetComponent<TextMesh> ();
			comboCounterShadowText = GameObject.Find ("P2ComboCountShadow").GetComponent<TextMesh> ();
			comboDamageShadowText = GameObject.Find ("P2DamageShadow").GetComponent<TextMesh> ();
			hitText = GameObject.Find ("Lhit").GetComponent<TextMesh> ();
			otherPlayerMovementScript = otherPlayer.GetComponentInChildren<PlayerMovementScript> ();
		}
		HideComboText ();
	}
	// Update is called once per frame
	void FixedUpdate () {
		if (tutorialMode && exCurrent < 1000) {
			AddMeter (1000 - exCurrent);
		}
	}
	public void HideComboText(){
		comboCounterText.gameObject.SetActive (false);
		comboDamageShadowText.gameObject.SetActive (false);
		comboCounterShadowText.gameObject.SetActive (false);
		comboDamageText.gameObject.SetActive (false);
		hitText.gameObject.SetActive (false);
	}
	// if attack is blocked returns true else it returns false
	public bool DealDamage(int amount = 1, int hitstun = 0, int blockstun = 0, Vector3 hitPosition = default(Vector3), 
		Vector2 hitPushback = default(Vector2), Vector2 blockPushback = default(Vector2), bool isProjectile = false, bool isThrow = false, 
		bool useCornerKnockback = true, bool freezingAttack = false, bool launcher = false, bool isKnockdownAttack = false, Vector2 optionalPosition = default(Vector2),
		int hitStopAmount = 0, AudioClip hitSound = default(AudioClip), float hitPitch =1, float blockPitch =1){

		if (state.GetState () == "parry" && ParryFunc != null && !isThrow) {
			ParryFunc ();
			
			// check for invincible or blocking
		} else if ((state.GetState () != "invincible" && !PMS.CheckIfBlocking () && state.GetState () != "blockstun" && !isThrow && !(state.GetState () == "projectile invulnerable" && isProjectile)) ||
		          (isThrow && state.GetState () != "invincible" && state.GetState () != "hitstun" && state.GetState () != "blockstun" && state.GetState () != "jumping" && state.GetState () != "jump attack" && state.GetState () != "prejump")) {

			// player got hit
			// deal damage
			if (state.GetState () == "hitstun" || state.GetState () == "falling hit" || state.GetState () == "frozen") {
				CancelInvoke ();
				comboCounterText.gameObject.SetActive (true);
				comboDamageShadowText.gameObject.SetActive (true);
				comboCounterShadowText.gameObject.SetActive (true);
				comboDamageText.gameObject.SetActive (true);
				hitText.gameObject.SetActive (true);
				comboCounter++;
				comboScaling -= .1f;

				if (comboScaling < .4f) {
					comboScaling = .4f;
				}
				comboDamage += (int)(amount * comboScaling);
				comboDamageText.text = comboDamage.ToString ();
				comboCounterText.text = comboCounter.ToString ();
				comboDamageShadowText.text = comboDamage.ToString ();
				comboCounterShadowText.text = comboCounter.ToString ();
			} else {
				comboCounter = 1;
				comboScaling = 1;
				freezingCounter = 0;
				comboDamage = amount;
			}
			if (freezingAttack) {
				freezingCounter++;
			}
			healthAmount -= (int)(amount * comboScaling);

			AddMeter ((int)((amount * comboScaling) * 1.3f));
			if (hpLeft != null) {
				hpLeft.changeBarLeft ((int)(amount * comboScaling));
			} else {
				hpRight.changeBarRight ((int)(amount * comboScaling));
			}
			HealUp ();



			// set hitstun
			StopAllCoroutines ();
			PMS.landingRecoveryFrames = 0;

			if (freezingAttack && freezingCounter <= 1) {
				SR.material.SetFloat ("_EffectAmount", 1);
				spriteAnimator.StopAnimations ();
			} else if (freezingCounter > 1) {
				hitstun = 5;
				SR.material.SetFloat ("_EffectAmount", 0);
				spriteAnimator.PlayHit (hitstun);
			} else {
				SR.material.SetFloat ("_EffectAmount", 0);
				// play hit animation
				if (isThrow) {
					sounds.PlayThrowHit ();	
					spriteAnimator.PlayHit (hitstun);
				} else if (isKnockdownAttack || state.GetState () == "jumping" || state.GetState () == "jumping attack" || state.GetState () == "falling hit") {
					spriteAnimator.PlayKnockdown ();
				} else {
					spriteAnimator.PlayHit (hitstun);
				}
			}

			StartCoroutine (InitiateHitstun (hitstun, hitPosition, hitPushback, isProjectile, useCornerKnockback, freezingAttack, launcher, optionalPosition, hitStopAmount, hitSound, hitPitch));

			// check for death
			CheckHealth ();
			//
			return false;

		} else if (PMS.CheckIfBlocking () || state.GetState () == "blockstun") {
			// player is blocking
			spriteAnimator.PlayBlock ();
			StopAllCoroutines ();
			StartCoroutine (InitiateBlockstun (blockstun, hitPosition, blockPushback, isProjectile, useCornerKnockback, optionalPosition, hitStopAmount, blockPitch));
			healthAmount -= (int)((float)amount * .05f);
			if (hpLeft != null) {
				hpLeft.changeBarLeft ((int)((float)amount * .05f));
			} else {
				hpRight.changeBarRight ((int)((float)amount * .05f));
			}
			HealUp ();
			// check for death
			CheckHealth ();
			return true;
		} else if (isThrow) {
			Debug.Log ("thro returrn true");
			return true;
		} else if (state.GetState () == "invincible") {
			return true;
		}
		return false;
	}
	public void CheckHealth(){
		if (healthAmount <= 0) {
			if (DeathFunc != null) {
				DeathFunc ();
			}
			Debug.Log ("died");
			foreach (Transform child in transform) {
				child.gameObject.SetActive (false);
			}

			PMS.EndGame ();
			otherPlayerMovementScript.gameObject.GetComponent<InputScript> ().inputEnabled = false;
			otherPlayerMovementScript.EndGame ();
			otherPlayerMovementScript.GetComponent<SpriteAnimator> ().PlayWin ();
			GetComponentInParent<InputScript> ().inputEnabled = false;
			//gameObject.SetActive (false);
		}
	}
	IEnumerator InitiateBlockstun(int stunFrames, Vector3 position, Vector2 bockPush, bool isProjectile, bool useCornerKockback = true, Vector2 optionalPosition = default(Vector2), int hitStopAmount = 5, float pitch=1){
		state.SetState ("blockstun");
		sounds.PlayBlock (pitch);
		GameObject sparks = blocksparksPool.FetchObject ();
		sparks.transform.position =  new Vector3 (transform.position.x +Random.Range (-.75f, .75f), position.y, -.2f);
		sparks.SetActive(true);

		timeManager.StopTime (hitStopAmount);

		PMS.CheckFacing ();
		PMS.MoveToward (-bockPush.x, bockPush.y);
		// if in the corner push attacker back
		Debug.Log((transform.position.x + " " + rightBound + "  " + transform.position.x + "  "+ leftBound));
		if((transform.position.x > rightBound || transform.position.x < leftBound) && useCornerKockback){
			Debug.Log ("pushcorner");
			PMS.CheckFacing ();
			otherPlayerMovementScript.MoveToward(-7.5f);	
		}
		for (int x = 0; x < stunFrames;) {
			if (PMS.ForceGroundCheck ()) {
				PMS.EnableBodyBox ();
			}
			if (!timeManager.CheckIfTimePaused()) {
				x++;
			}
			yield return null;
		}

		if (PMS.ForceGroundCheck ()) {
			state.SetState ("neutral");
		} else {
			state.SetState ("falling hit");
		}

	}
	IEnumerator InitiateHitstun(int stunFrames, Vector3 position, Vector2 hitPush, bool isProjectile, bool useCornerKockback = true, bool freezingAttack = false, bool launcher= false, Vector2 optionalPosition = default(Vector2), int hitStopAmount = 5, AudioClip hitSound = default(AudioClip), float pitch =0){

		 if (freezingAttack){
			state.SetState ("frozen");
		}else if (state.GetState () == "jumping" || state.GetState () == "jumping attack" || state.GetState () == "falling hit") {
			state.SetState ("falling hit");
		}else {
			state.SetState ("hitstun");
		}
		sounds.PlayHit (hitSound, pitch);
		if (HitFunc != null) {
			HitFunc ();
		}
			

		if (!freezingAttack) {
			GameObject sparks = hitsparksPool.FetchObject ();
			sparks.transform.position =  new Vector3 (transform.position.x +Random.Range (-.75f, .75f), position.y, -.2f);
			sparks.SetActive (true);
		}
		timeManager.StopTime (hitStopAmount);
		PMS.CheckFacing ();
		if (!freezingAttack) {
			if (optionalPosition == default(Vector2) || (optionalPosition.x > transform.position.x && PMS.CheckIfOnLeft()) || (optionalPosition.x < transform.position.x && !PMS.CheckIfOnLeft())){
				PMS.MoveToward (-hitPush.x, hitPush.y);
			} else{
				PMS.MoveToward(hitPush.x, hitPush.y);
			} 
		} else {
			PMS.StopMovement ();
		}
		// if in the corner push attacker back
		if((transform.position.x > rightBound || transform.position.x < leftBound) && useCornerKockback){
			Debug.Log ("pushcorner");
			PMS.CheckFacing ();
			otherPlayerMovementScript.MoveToward(-7.5f);	
		}
		for (int x = 0; x < stunFrames;) {	
			if (x == 5) {
				if (launcher) {
					state.SetState ("falling hit");
					PMS.DsableBodyBox ();
				}
			}
			if (PMS.ForceGroundCheck ()) {
				PMS.EnableBodyBox ();
			}
			if (!timeManager.CheckIfTimePaused()) {
				x++;
			}
			yield return null;
		}

		if (PMS.ForceGroundCheck () && (state.GetState() == "hitstun" || state.GetState() == "frozen")) {
			state.SetState ("neutral");
			PMS.EnableBodyBox ();
		} else if (state.GetState() != "invincible"){
			state.SetState ("falling hit");
		}
		if (freezingAttack) {
			SR.material.SetFloat ("_EffectAmount", 0);
			spriteAnimator.PlayNeutralAnim ();
		}
		Invoke ("HideComboText", .5f);
	}
	public void SetHitFunc(DeathEvent newFunc){
		HitFunc = newFunc;
	}
	public void SetParryFunc(DeathEvent newFunc){
		ParryFunc = newFunc;
	}
	public void SetDeathFunc(DeathEvent newFunc){
		DeathFunc = newFunc;
	}
	public void AddMeter(int amountToAdd){
		exCurrent += amountToAdd;
		if (exCurrent > 1000){
			exCurrent = 1000;
		}
		if (hpLeft != null) {
			exBar.ExMeterChange(amountToAdd, true);
		}else {
			exBar.ExMeterChange(amountToAdd, false);

		}
	}
	void HealUp(){
		if (healthAmount < healthMax && tutorialMode) {
			int missing = healthMax - healthAmount;
			healthAmount = healthMax;

			if (hpLeft != null) {
				hpLeft.changeBarLeft (-missing);
			} else {
				hpRight.changeBarRight (-missing);
			}
		}
	}

}
