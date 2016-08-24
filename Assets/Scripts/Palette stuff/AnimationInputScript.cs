using UnityEngine;
using System.Collections;

public class AnimationInputScript : MonoBehaviour {
	InputScript IS;
	SpriteAnimator SA;
	// Use this for initialization
	void Start () {
		IS = GetComponent<InputScript>();
		SA = GetComponent<SpriteAnimator> ();

		IS.SetThumbstick(StickInput);
		IS.assignXButton (SA.PlayLight, null);
		IS.assignYButton (SA.PlayMedium, null);
		IS.assignRBButton (SA.PlayHeavy, null);
		IS.assignAButton (SA.PlaySpecialOne, null);
		IS.assignBButton (SA.PlaySpecialTwo, null);
		IS.assignRT (SA.PlaySpecialThree, null);
		//IS.assignAXButton (SA.PlayThrowComplete);
		//IS.assignBYButton (SA.PlaySuper);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void StickInput(float x, float y){

		if (y > .5f && x > .5f) {
			SA.PlayJumpAway ();
		}else if (y > .5f && x < -.5f) {
			SA.PlayJumpToward ();
		}else if (y > .5f && x > -.1f && x < .1f) {
			SA.PlayJumpNeutral ();
		}else if (x > .5f) {
			SA.PlayWalkAwayAnim ();
		}else if (x < -.5f) {
			SA.PlayWalkAnim ();
		}else if (x > -.1f && x < .1f && y < -.5f) {
			SA.PlayNeutralAnim ();
		}
	}
}
