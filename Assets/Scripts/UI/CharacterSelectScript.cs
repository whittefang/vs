using UnityEngine;
using System.Collections;
using XInputDotNetPure; // Required in C#

public class CharacterSelectScript : MonoBehaviour {
	public int playerNumber = 0;
	public int currentSelection = 0;
	public Vector2[] cursorPositions;
	Rounds roundScript;
	PlayerIndex playerIndex;
	GamePadState state;
	GamePadState prevState;
	UIsounds sound;

	SceneTransistionScript sceneLoader;
	bool buffer = true, selectionMade = false, lockedIn = false;
	public GameObject[] previewsLoop;
	public GameObject[] selectedsLoop;

	// Use this for initialization
	void Awake () {
		sceneLoader = GameObject.Find ("Main Camera").GetComponent<SceneTransistionScript> ();
		playerIndex = (PlayerIndex)playerNumber;
		sound = GameObject.Find ("Main Camera").GetComponent<UIsounds> ();
		roundScript = GameObject.Find ("DoNotDestroy").GetComponent<Rounds> ();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		prevState = state;
		state = GamePad.GetState (playerIndex);

		// send input for movement(state.ThumbSticks.Left.X, state.ThumbSticks.Left.Y);
		if (state.ThumbSticks.Left.X > 0 && currentSelection < 6 && buffer && !selectionMade){
			StartCoroutine (setBuffer());
			sound.PlayChange ();
			// move cursor
			currentSelection++;
			transform.position = cursorPositions [currentSelection];
			// update character preview
			UpdatePreview(currentSelection);
		}
		if (state.ThumbSticks.Left.X < 0 && currentSelection > 0 && buffer && !selectionMade){
			StartCoroutine (setBuffer());
			sound.PlayChange ();
			currentSelection--;
			transform.position = cursorPositions [currentSelection];
			UpdatePreview(currentSelection);
		}

		if (prevState.Buttons.A == ButtonState.Released && state.Buttons.A == ButtonState.Pressed && !selectionMade) {
			// set rounds script
			if (ConvertToString (currentSelection) == "random") {
				if (playerNumber == 0) {
					roundScript.player1character = RandomizeCharacter ();
					lockedIn = sceneLoader.SetReady (true, true);
				} else {
					roundScript.player2character = RandomizeCharacter ();
					lockedIn = sceneLoader.SetReady (false, true);
				}
			} else {
				if (playerNumber == 0) {
					roundScript.player1character = ConvertToString (currentSelection);
					lockedIn = sceneLoader.SetReady (true, true);
				} else {
					roundScript.player2character = ConvertToString (currentSelection);
					lockedIn = sceneLoader.SetReady (false, true);
				}
			}
			UpdateSelected(currentSelection);
			selectionMade = true;
			sound.PlayConfirm ();
		}
		if (prevState.Buttons.B == ButtonState.Released && state.Buttons.B == ButtonState.Pressed && selectionMade && !lockedIn) {
			sound.PlayBack ();
			selectionMade = false;
			if (playerNumber == 0) {
				sceneLoader.SetReady (true, false);
			} else {
				sceneLoader.SetReady (false, false);
			}
			UpdatePreview (currentSelection);
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
			return "subzero";
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
	public void UpdatePreview(int character){
		foreach (GameObject g in previewsLoop) {
			g.SetActive (false);
		}
		foreach (GameObject g in selectedsLoop) {
			g.SetActive (false);
		}
		previewsLoop [character].SetActive (true);
	}
	void UpdateSelected(int character){
		foreach (GameObject g in previewsLoop) {
			g.SetActive (false);
		}
		foreach (GameObject g in selectedsLoop) {
			g.SetActive (false);
		}
		selectedsLoop [character].SetActive (true);
		sound.PlayCharacterSelect (character);
	}
}
