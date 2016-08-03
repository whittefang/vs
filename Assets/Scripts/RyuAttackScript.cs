using UnityEngine;
using System.Collections;

public class RyuAttackScript : MonoBehaviour {

	InputScript inputScript;
	SpriteAnimator spriteAnimator;
	FighterStateMachineScript state;
	PlayerMovementScript PMS;
	public GameObject fireball;

	// Use this for initialization
	void Start () {
		state = GetComponent<FighterStateMachineScript>();
		spriteAnimator = GetComponent<SpriteAnimator> ();
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
		if (state.GetState() == "neutral") {
			StartCoroutine (mediumEnum ());
		} else if (state.GetState () == "jumping") {
			StartCoroutine (jumpMediumEnum ());
		}

	}
	IEnumerator mediumEnum(){
		spriteAnimator.PlayMedium ();
		PMS.StopMovement ();
		state.SetState ("attack");

		for (int x = 0; x < 6; x++) {
			yield return null;
		}
		PMS.MoveToward (5);
		for (int x = 0; x < 6; x++) {
			yield return null;
		}
		PMS.StopMovement ();
		for (int x = 0; x < 12; x++) {
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
		if (state.GetState() == "neutral") {
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
			yield return null;
		}
		PMS.StopMovement ();
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
		if (state.GetState() == "neutral") {
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
		fireball.SetActive (true);
		for (int x = 0; x < 24; x++) {
			yield return null;
		}
		state.SetState ("neutral");
	}
	public void SpecialTwo(){
		if (state.GetState() == "neutral") {
			StartCoroutine (SpecialTwoEnum ());
		}

	}
	IEnumerator SpecialTwoEnum(){
		spriteAnimator.PlaySpecialTwo ();
		PMS.StopMovement ();
		state.SetState ("attack");
		for (int x = 0; x < 14; x++) {
			yield return null;
		}
		PMS.MoveToward (5, 20);
		for (int x = 0; x < 46; x++) {
			yield return null;
		}
		state.SetState ("neutral");
	}
	public void SpecialThree(){
		if (state.GetState() == "neutral") {
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
