using UnityEngine;
using System.Collections;

public class RyuAttackScript : MonoBehaviour {

	InputScript inputScript;
	SpriteAnimator spriteAnimator;
	FighterStateMachineScript state;
	PlayerMovementScript PMS;
	public GameObject fireball, lightHitbox, mediumHitbox, heavyHitbox, sp1Hitbox, sp2HitboxPart1, sp2HitboxPart2, sp3Hitbox;
	ProjectileScript fireballProjectileScript;

	// Use this for initialization
	void Start () {
		state = GetComponent<FighterStateMachineScript>();
		spriteAnimator = GetComponent<SpriteAnimator> ();
		fireballProjectileScript = fireball.GetComponent < ProjectileScript> ();
		PMS = GetComponent<PlayerMovementScript> ();
		inputScript = GetComponent<InputScript> ();
		inputScript.assignXButton (Light, null);
		inputScript.assignYButton (Medium, null);
		inputScript.assignRBButton (Heavy, null);

		inputScript.assignAButton (SpecialOne, null);
		inputScript.assignBButton (SpecialTwo, null);
		inputScript.assignRT (SpecialThree, null);
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
		for (int x = 0; x < 15; x++) {
			if (x == 4) {
				lightHitbox.SetActive (true);
			}
			if (x == 6) {
				lightHitbox.SetActive (false);
				state.SetState ("light recovery");
			}
			yield return null;
		}
		state.SetState ("neutral");
	}
	IEnumerator jumpLightEnum(){
		spriteAnimator.PlayJumpLight ();
		state.SetState ("jump attack");
		for (int x = 0; x < 15; x++) {
			yield return null;
		}
	}
	public void Medium(){
		if (state.GetState() == "neutral" || state.GetState() =="light recovery") {
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

		for (int x = 0; x < 9; x++) {
			yield return null;
		}
		mediumHitbox.SetActive (true);

		for (int x = 0; x < 18; x++) {
			if (x == 2) {
				mediumHitbox.SetActive (false);
				state.SetState ("medium recovery");
			}
			yield return null;
		}
		state.SetState ("neutral");
	}
	IEnumerator jumpMediumEnum(){
		spriteAnimator.PlayJumpMedium ();
		state.SetState ("jump attack");
		for (int x = 0; x < 27; x++) {
			yield return null;
		}
	}
	public void Heavy(){
		if (state.GetState() == "neutral" || state.GetState() =="medium recovery") {
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
		for (int x = 0; x < 22; x++) {
			if (x == 18) {
				heavyHitbox.SetActive (true);
			}
			yield return null;
		}

		PMS.StopMovement ();
		heavyHitbox.SetActive (false);
		state.SetState ("heavy recovery");
		for (int x = 0; x < 20; x++) {
			yield return null;
		}
		state.SetState ("neutral");
	}
	IEnumerator jumpHeavyEnum(){
		spriteAnimator.PlayJumpHeavy ();
		state.SetState ("jump attack");
		for (int x = 0; x < 21; x++) {
			yield return null;
		}
	}

	public void SpecialOne(){
		if ((state.GetState() == "neutral" || state.GetState() =="light recovery" || state.GetState() =="medium recovery" || state.GetState() =="heavy recovery") && !fireball.activeSelf) {
			StopAllCoroutines ();
			StartCoroutine (SpecialOneEnum ());
		}

	}
	IEnumerator SpecialOneEnum(){
		spriteAnimator.PlaySpecialOne ();
		PMS.StopMovement ();
		state.SetState ("attack");
		for (int x = 0; x < 12; x++) {
			yield return null;
		}
		fireball.transform.position = transform.position;
		if (PMS.CheckIfOnLeft ()) {
			fireball.GetComponentInChildren<SpriteRenderer> ().flipX = true;
			fireballProjectileScript.direction = new Vector2 (1, 0);
		} else {
			fireball.GetComponentInChildren<SpriteRenderer> ().flipX = false;
			fireballProjectileScript.direction = new Vector2 (-1, 0);
		
		}
		fireball.SetActive (true);
		for (int x = 0; x < 24; x++) {
			yield return null;
		}
		state.SetState ("neutral");
	}
	public void SpecialTwo(){
		if (state.GetState() == "neutral" || state.GetState() =="light recovery" || state.GetState() =="medium recovery" || state.GetState() =="heavy recovery"){
			StopAllCoroutines ();
			StartCoroutine (SpecialTwoEnum ());
		}

	}
	IEnumerator SpecialTwoEnum(){
		spriteAnimator.PlaySpecialTwo ();
		PMS.StopMovement ();
		state.SetState ("attack");
		for (int x = 0; x < 14; x++) {
			if (x == 3) {
				state.SetState ("invincible");
				sp2HitboxPart1.SetActive (true);
			}

			yield return null;
		}


		PMS.MoveToward (5, 25);
		for (int x = 0; x < 46; x++) {
			if (x == 1) {
				state.SetState ("attack");
				sp2HitboxPart1.SetActive (false);
				sp2HitboxPart2.SetActive (true);
			}
			if (x == 15) {
				sp2HitboxPart2.SetActive (false);
			}
			yield return null;
		}
		state.SetState ("neutral");
	}
	public void SpecialThree(){
		if (state.GetState() == "neutral" || state.GetState() == "light recovery" || state.GetState() =="medium recovery" || state.GetState() =="heavy recovery") {
			StopAllCoroutines ();
			StartCoroutine (SpecialthreeEnum ());
		}

	}
	IEnumerator SpecialthreeEnum(){
		spriteAnimator.PlaySpecialThree ();
		PMS.StopMovement ();
		state.SetState ("attack");
		for (int x = 0; x < 6; x++) {
			yield return null;
		}
		for (int x = 0; x < 80; x++) {
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
			PMS.MoveToward (8);
			yield return null;
		}
		PMS.StopMovement ();
		for (int x = 0; x < 10; x++) {
			yield return null;
		}
		PMS.StopMovement ();
		state.SetState ("neutral");
	}

}
