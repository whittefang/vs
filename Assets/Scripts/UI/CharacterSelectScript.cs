using UnityEngine;
using System.Collections;
using XInputDotNetPure; // Required in C#

public class CharacterSelectScript : MonoBehaviour {
	public int playerNumber = 0;
	public int currentSelection = 0;
	public Vector2[] cursorPositions;
	public CharacterDisplayScript spritePreview;
	Rounds roundScript;
	PlayerIndex playerIndex;
	GamePadState state;
	GamePadState prevState;
	bool buffer = true;


	// Use this for initialization
	void Awake () {
		playerIndex = (PlayerIndex)playerNumber;
		roundScript = GameObject.Find ("DoNotDestroy").GetComponent<Rounds> ();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		prevState = state;
		state = GamePad.GetState (playerIndex);

		// send input for movement(state.ThumbSticks.Left.X, state.ThumbSticks.Left.Y);
		if (state.ThumbSticks.Left.X > 0 && currentSelection < 6 && buffer){
			StartCoroutine (setBuffer());
			// move cursor
			currentSelection++;
			transform.position = cursorPositions [currentSelection];
			// update character preview
			spritePreview.UpdatePreview(ConvertToString(currentSelection));
		}
		if (state.ThumbSticks.Left.X < 0 && currentSelection > 0 && buffer){
			StartCoroutine (setBuffer());
			currentSelection--;
			transform.position = cursorPositions [currentSelection];
			spritePreview.UpdatePreview(ConvertToString(currentSelection));
		}

		if (prevState.Buttons.A == ButtonState.Released && state.Buttons.A == ButtonState.Pressed) {
			// set rounds script
			if (ConvertToString (currentSelection) == "random") {
				if (playerNumber == 0) {
					roundScript.player1character = RandomizeCharacter ();
				} else {
					roundScript.player2character = RandomizeCharacter ();
				}
			} else {
				if (playerNumber == 0) {
					roundScript.player1character = ConvertToString (currentSelection);
				} else {
					roundScript.player2character = ConvertToString (currentSelection);
				}
			}
		}
		if (prevState.Buttons.B == ButtonState.Released && state.Buttons.B == ButtonState.Pressed) {

		}
	}
	string RandomizeCharacter(){
		string pickedCharacter = "random";
		while (pickedCharacter == "random") {
			pickedCharacter = ConvertToString (Random.Range (0, 7));
		}
		return pickedCharacter;
	}
	void UpdateCursorPosition(){
	}
	string ConvertToString(int number){
		switch (number) {
		case 0: 
			return "felicia";
		case 1: 
			return "random";
		case 2: 
			return "ryu";
		case 3: 
			return "random";
		case 4: 
			return "random";
		case 5: 
			return "random";
		case 6: 
			return "random";
		default:
			return "random";	
		}
	}

	IEnumerator setBuffer(){
		buffer = false;
		yield return new WaitForSeconds (.15f);
		buffer = true;
	}
}
