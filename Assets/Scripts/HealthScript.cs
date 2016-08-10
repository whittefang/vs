using UnityEngine;
using System.Collections;

public class HealthScript : MonoBehaviour {
	public int healthAmount;
	public int healthMax = 1000;
	public delegate void DeathEvent();
	DeathEvent DeathFunc, HitFunc;
	FighterStateMachineScript state;
	SpriteAnimator spriteAnimator;
	PlayerMovementScript PMS, otherPlayerMovementScript;
	TimeManagerScript timeManager;
	ObjectPoolScript hitsparksPool, blocksparksPool;
	SoundsPlayer sounds;
	TextMesh comboCounterText, comboDamageText;
	public int comboCounter = 0, comboDamage = 0;
	public float comboScaling = 1, leftBound = -7.5f, rightBound = 7.5f;
	// Use this for initialization
	void OnEnable () {
		if (sounds == null) {
			if (tag == "PlayerOne") {
				sounds = GameObject.Find ("P2MasterObject").GetComponent<SoundsPlayer>();
				hitsparksPool = GameObject.Find ("P2MasterObject").GetComponent<ObjectPoolScript> ();
				blocksparksPool = GameObject.Find ("P2BlockSparksObject").GetComponent<ObjectPoolScript> ();
				comboCounterText = GameObject.Find ("P1ComboCount").GetComponent<TextMesh> ();
				comboDamageText = GameObject.Find ("P1Damage").GetComponent<TextMesh> ();
				otherPlayerMovementScript = GameObject.FindWithTag ("playerTwo").GetComponent<PlayerMovementScript>();
			} else {
				sounds = GameObject.Find ("P1MasterObject").GetComponent<SoundsPlayer>();
				hitsparksPool = GameObject.Find ("P1MasterObject").GetComponent<ObjectPoolScript> ();
				blocksparksPool = GameObject.Find ("P1BlockSparksObject").GetComponent<ObjectPoolScript> ();
				comboCounterText = GameObject.Find ("P2ComboCount").GetComponent<TextMesh> ();
				comboDamageText = GameObject.Find ("P2Damage").GetComponent<TextMesh> ();
				otherPlayerMovementScript = GameObject.FindWithTag ("playerOne").GetComponent<PlayerMovementScript>();
			}
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
	
	// Update is called once per frame
	void Update () {
	
	}
	public void DealDamage(int amount = 1, int hitstun = 0, int blockstun = 0, Vector3 hitPosition = default(Vector3), 
		Vector2 hitPushback = default(Vector2), Vector2 blockPushback = default(Vector2), bool isProjectile = false, bool isThrow = false){

		// check for invincible or blocking
		if ((state.GetState () != "invincible" && !PMS.CheckIfBlocking () && state.GetState() != "blockstun") || 
			( isThrow && state.GetState() != "hitstun" && state.GetState() != "blockstun" && state.GetState() != "jumping" && state.GetState() != "jump attack"  )) {
			// player got hit
			// deal damage
			if (state.GetState () == "hitstun") {
				comboCounter++;
				comboScaling -= .1f;
				if (comboScaling < .4f) {
					comboScaling = .4f;
				}
				comboDamage += (int)(amount * comboScaling);
				comboDamageText.text = comboDamage.ToString();
				comboCounterText.text = comboCounter.ToString();
			} else {
				comboCounter = 1;
				comboScaling = 1;
				comboDamage = amount;
			}
			healthAmount -= (int)(amount * comboScaling);

			// play hit animation
			if (isThrow) {
				sounds.PlayThrowHit ();	
			} else {
				sounds.PlayHit ();
			}
			spriteAnimator.PlayHit (hitstun);
			// set hitstun
			StopAllCoroutines();
			StartCoroutine (InitiateHitstun (hitstun, hitPosition, hitPushback, isProjectile));

			// check for death
			CheckHealth ();
			//

		} else if (PMS.CheckIfBlocking () || state.GetState() == "blockstun") {
			// player is blocking
			spriteAnimator.PlayBlock();
			StopAllCoroutines();
			StartCoroutine (InitiateBlockstun (blockstun, hitPosition, blockPushback, isProjectile));
			healthAmount -= (int)((float)amount * .05f);
		}
	}
	public void CheckHealth(){
		if (healthAmount <= 0) {
			if (DeathFunc != null) {
				DeathFunc ();
			}
			gameObject.SetActive (false);
		}
	}
	IEnumerator InitiateBlockstun(int stunFrames, Vector3 position, Vector2 bockPush, bool isProjectile){
		state.SetState ("blockstun");
		sounds.PlayBlock ();
		GameObject sparks = blocksparksPool.FetchObject ();
		sparks.transform.position = position + new Vector3(Random.Range(-.75f, .75f), Random.Range(-1f, 1f),0);
		sparks.SetActive(true);
		PMS.MoveToward (-bockPush.x, bockPush.y);
		// if in the corner push attacker back
		if((transform.position.x > rightBound || transform.position.x < leftBound) && !isProjectile){
			otherPlayerMovementScript.MoveToward(-5);	
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
	IEnumerator InitiateHitstun(int stunFrames, Vector3 position, Vector2 hitPush, bool isProjectile){
		state.SetState ("hitstun");
		if (HitFunc != null) {
			HitFunc ();
		}
		GameObject sparks = hitsparksPool.FetchObject ();
		sparks.transform.position = position + new Vector3(Random.Range(-.75f, .75f), Random.Range(-1f, 1f),0);
		sparks.SetActive(true);

		timeManager.StopTime (5);
		PMS.MoveToward (-hitPush.x, hitPush.y);
		// if in the corner push attacker back
		Debug.Log(transform.position.x);
		if((transform.position.x > rightBound || transform.position.x < leftBound) && !isProjectile){
			otherPlayerMovementScript.MoveToward(-5);	
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
	public void SetHitFunc(DeathEvent newFunc){
		HitFunc = newFunc;
	}

}
