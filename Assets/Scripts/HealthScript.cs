using UnityEngine;
using System.Collections;

public class HealthScript : MonoBehaviour {
	public int healthAmount;
	public int healthMax = 1000;
	public delegate void DeathEvent();
	DeathEvent DeathFunc;
	FighterStateMachineScript state;
	SpriteAnimator spriteAnimator;
	PlayerMovementScript PMS;
	TimeManagerScript timeManager;
	ObjectPoolScript hitsparksPool;
	SoundsPlayer sounds;
	public int comboCounter = 0;
	public float comboScaling = 1;
	// Use this for initialization
	void OnEnable () {
		if (sounds == null) {
			if (tag == "PlayerOne") {
				sounds = GameObject.Find ("P2MasterObject").GetComponent<SoundsPlayer>();
				hitsparksPool = GameObject.Find ("P2MasterObject").GetComponent<ObjectPoolScript> ();
			} else {
				sounds = GameObject.Find ("P1MasterObject").GetComponent<SoundsPlayer>();
				hitsparksPool = GameObject.Find ("P1MasterObject").GetComponent<ObjectPoolScript> ();
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
	public void DealDamage(int amount = 1, int hitstun = 0, int blockstun = 0, Vector3 hitPosition = default(Vector3), string isProjectile = "attack"){
		// check for invincible or blocking
		if (state.GetState () != "invincible" && !PMS.CheckIfBlocking ()) {
			// player got hit
			// deal damage
			if (state.GetState () == "hitstun") {
				comboCounter++;
				comboScaling -= .1f;
				if (comboScaling < .4f) {
					comboScaling = .4f;
				}
			} else {
				comboCounter = 0;
				comboScaling = 1;
			}
			healthAmount -= (int)(amount * comboScaling);

			// play hit animation
			spriteAnimator.PlayHit (hitstun);
			// set hitstun
			StopAllCoroutines();
			StartCoroutine (InitiateHitstun (hitstun, hitPosition));

			// check for death
			CheckHealth ();
		} else if (PMS.CheckIfBlocking () || state.GetState() == "blockstun") {
			// player is blocking
			spriteAnimator.PlayBlock();
			StopAllCoroutines();
			StartCoroutine (InitiateBlockstun (blockstun));
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
	IEnumerator InitiateBlockstun(int stunFrames){
		state.SetState ("blockstun");
		sounds.PlayBlock ();
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
	IEnumerator InitiateHitstun(int stunFrames, Vector3 position){
		state.SetState ("hitstun");
		GameObject sparks = hitsparksPool.FetchObject ();
		sparks.transform.position = position + new Vector3(Random.Range(-.75f, .75f), Random.Range(-1f, 1f),0);
		sparks.SetActive(true);
		sounds.PlayHit ();
		timeManager.StopTime (5);
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
}
