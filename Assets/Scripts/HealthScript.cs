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
	// Use this for initialization
	void OnEnable () {
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
	public void DealDamage(int amount = 1, int hitstun = 0, string isProjectile = "attack"){
		// check for invincible or blocking
		if (state.GetState () != "invincible") {
			
			// play hit animation
			spriteAnimator.PlayHit();
			// set hitstun
			StartCoroutine(InitateHitstun(hitstun));

			// deal damage
			healthAmount -= amount;
			// check for death
			CheckHealth ();
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
	IEnumerator InitateHitstun(int stunFrames){
		state.SetState ("hitstun");
		for (int x = 0; x < stunFrames; x++) {
			if (PMS.ForceGroundCheck ()) {
				PMS.EnableBodyBox ();
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
