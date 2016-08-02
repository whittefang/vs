using UnityEngine;
using System.Collections;

public class RyuAttackScript : MonoBehaviour {

	InputScript inputScript;
	SpriteAnimator spriteAnimator;
	FighterStateMachineScript state;
	// Use this for initialization
	void Start () {
		state = GetComponent<FighterStateMachineScript>();
		spriteAnimator = GetComponent<SpriteAnimator> ();
		inputScript = GetComponent<InputScript> ();
		inputScript.assignXButton (Light, null);
	}
	
	public void Light(){
		if (state.GetState() == "neutral") {
			StartCoroutine (lightEnum ());
		}

	}
	IEnumerator lightEnum(){
		spriteAnimator.PlayLight ();
		state.SetState ("attack");
		for (int x = 0; x < 15; x++) {
			yield return null;
		}
		state.SetState ("neutral");
	}

}
