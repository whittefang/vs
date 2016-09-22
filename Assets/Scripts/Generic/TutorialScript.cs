using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class TutorialScript : MonoBehaviour {
	public InputScript inputScript;
	public InputScript inputScriptOpponent;
	public TextMesh tutorialText;
	public TextMesh titleText;
	public GameObject normalsSlide, specialsSlide, throwSlide, superSlide, comboSlide,  advancedComboSlide;
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
		specialsSlide.SetActive (false);
		normalsSlide.SetActive (false);
		throwSlide.SetActive (false);
		superSlide.SetActive (false);
		comboSlide.SetActive (false);
		advancedComboSlide.SetActive (false);
		switch (spot) {
		case 0:
			titleText.text = "Ground Movement";
			tutorialText.text = "Use the left stick to move";
			break;
		case 1:
			titleText.text = "Air Movement";
			tutorialText.text = "Move the left stick up to jump";
			break;
		case 2:
			titleText.text = "Normal Attacks";
			tutorialText.text = "";
			normalsSlide.SetActive (true);
			break;
		case 3:
			titleText.text = "Special Attacks";
			tutorialText.text = "";
			specialsSlide.SetActive (true);
			break;
		case 4:
			titleText.text = "Throws";
			tutorialText.text = "";
			throwSlide.SetActive (true);
			break;
		case 5:
			titleText.text = "Super Attack";
			tutorialText.text = "";
			superSlide.SetActive (true);
			break;
		case 6:
			titleText.text = "Blocking";
			tutorialText.text =	"To block you hold the left stick in the opposite direction of the enemy \n" +
				"Try now to block the enemies fireball";
			InvokeRepeating ("ThrowFireball", 5f, 3);
			break;
		case 7:
			titleText.text = "Basic Combos";
			tutorialText.text = "";
			comboSlide.SetActive (true);
			break;
		case 8:
			titleText.text = "Advanced Combos";
			tutorialText.text = "";
			advancedComboSlide.SetActive (true);
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
