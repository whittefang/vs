using UnityEngine;
using System.Collections;

public class HealthScript : MonoBehaviour {
	public int healthAmount;
	public int healthMax = 1000;
	public delegate void DeathEvent();
	DeathEvent DeathFunc;
	FighterStateMachineScript state;
	SpriteAnimator spriteAnimator;
	// Use this for initialization
	void OnEnable () {
		if (state == null) {
			state = GetComponent<FighterStateMachineScript> ();
		}
		if (spriteAnimator == null) {
			spriteAnimator = GetComponent<SpriteAnimator> ();
		}
		healthAmount = healthMax ;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public void DealDamage(int amount = 1, int hitstun = 0, string isProjectile = "attack"){
		// check for invincible or blocking
		if (state.GetState () != "invincible") {
			// set hitstun
			StartCoroutine(InitateHitstun(hitstun));
			// play hit animation
			spriteAnimator.PlayHit();
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
		state.SetState ("hitStun");
		for (int x = 0; x < stunFrames; x++) {
			yield return null;
		}
		state.SetState ("neutral");
	}
}
