using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class TutorialScript : MonoBehaviour {
	public InputScript inputScript;
	public InputScript inputScriptOpponent;
	public TextMesh tutorialText;
	public TextMesh titleText;
	int spot = 0;
	// Use this for initialization
	void Start () {
		inputScript.assignbackButton (ReverseText, null);
		inputScript.assignStartButton (AdvanceText, null);
		ChangeText ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public void AdvanceText(){
		spot++;
		if (spot > 9) {
			spot = 9;
		}
		ChangeText ();

	}
	public void ReverseText(){
		spot--;
		if (spot < 0) {
			spot = 0;
		}
		ChangeText ();

	}

	public void ChangeText(){
		CancelInvoke ();
		switch (spot) {
		case 0:
			titleText.text = "Ground Movement";
			tutorialText.text = "Push the left analog Stick left and right to move back and forth";
			break;
		case 1:
			titleText.text = "Air Movement";
			tutorialText.text = "Push left analog Stick up to jump";
			break;
		case 2:
			titleText.text = "Normal Attacks";
			tutorialText.text = "X button is your light attack \n" +
				"Y button is your medium attack \n" +
				"Right Bumper is your heavy attack\n" +
				"These are referred to as normal attacks";
			break;
		case 3:
			titleText.text = "Special Attacks";
			tutorialText.text = "A button is your first special attack \n" +
				"B button is your second special attack \n" +
				"Right Trigger is your third special attack\n" +
				"These are referred to as special attacks";
			break;
		case 4:
			titleText.text = "Throws";
			tutorialText.text = "Pressing the X and A button simultaneously will cause you to preform a throw! \n" +
				"Throws can not be blocked so they are good to use on defensive opponents";
			break;
		case 5:
			titleText.text = "Super Attack";
			tutorialText.text = "Pressing the Y and B buttons together will cause you to preform a super!\n" +
				"Super attacks deal high damage but require you to have a full super meter";
			break;
		case 6:
			titleText.text = "Blocking";
			tutorialText.text = "Blocking is a key technique that is very important to use in order to obtain victory\n" +
				"To block you hold the left analog stick in the opposite direction of the opposing fighter \n" +
				"Try now to block the opponents fireball";
			InvokeRepeating ("ThrowFireball", 8f, 3);
			break;
		case 7:
			titleText.text = "Basic Combos";
			tutorialText.text = "When you hit an opponent with a normal attack you will be able to do a combo\n" +
				"Start with a light attack(X), then medium attack (Y), and then heavy attack(Right Bumper)\n" +
				"If you have done this right you will see text reading 3 hits appear ";
			break;
		case 8:
			titleText.text = "Advanced Combos";
			tutorialText.text = "Special attacks can be used to extend your combo even further\n" +
				"When you hit with any normal attack you can use a special attack (A, B, And Right Trigger)\n" +
				"Try this with the medium normal attack (Y) and then using the fireball special(A)\n" +
				"If you have done this right you will see text reading 2 hits appear ";
			break;
		case 9:
			SceneManager.LoadScene (1);
			break;

		}
	}
	void ThrowFireball(){
		inputScriptOpponent.useAbuttonFunc ();
	}
}
