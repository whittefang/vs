using UnityEngine;
using System.Collections;
using XInputDotNetPure; // Required in C#
using UnityEngine.SceneManagement;

public class CharacterSelectScript : MonoBehaviour {
	public bool characterSelect = true, colorSelect = false;
	public int playerNumber = 0;
	public int currentSelection = 0;
	public int maxSelectionSize = 6;
	public int colorSelection = 0;
	public GameObject ColorSelectorBlock;
	public Transform[] cursorPositions;
	Rounds roundScript;
	PlayerIndex playerIndex;
	GamePadState state;
	GamePadState prevState;
	UIsounds sound;
	SceneTransistionScript sceneLoader;
	bool buffer = true, selectionMade = false, lockedIn = false;
	public GameObject[] previewsLoop;
	public GameObject[] selectedsLoop;
	public GameObject[] SpecialsLoop;
	public ColorPreviewScript colorPreviewScript;
	public GameObject loadscreenObject;


	enum uiState{
		CharacterSelect,
		ColorSelect,
		LockedIn
	}
	uiState currentState = uiState.CharacterSelect;

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

		if (state.Buttons.Back == ButtonState.Pressed && prevState.Buttons.Back == ButtonState.Released) {
			loadscreenObject.SetActive (true);
			roundScript.tutorialPlayerNumber = playerNumber;
			SceneManager.LoadScene (3);
		}

		// send input for movement(state.ThumbSticks.Left.X, state.ThumbSticks.Left.Y);
		if (state.ThumbSticks.Left.X > 0 && buffer && !selectionMade){
			MoveCursorForward();	
		}
		if (state.ThumbSticks.Left.X < 0  && buffer && !selectionMade){
			MoveCursorBackward ();
		}

		if (prevState.Buttons.A == ButtonState.Released && state.Buttons.A == ButtonState.Pressed && !selectionMade) {
			ConfirmSelection ();
		}
		if (prevState.Buttons.B == ButtonState.Released && state.Buttons.B == ButtonState.Pressed && !lockedIn) {
			BackoutOfSelection ();
		}
	}
	string RandomizeCharacter(){
		string pickedCharacter = "random";
		while (pickedCharacter == "random") {
			pickedCharacter = ConvertToString (Random.Range (0, 7));
		}
		Debug.Log (pickedCharacter);
		return pickedCharacter;
	}

	void MoveCursorForward(){
		if (currentState == uiState.CharacterSelect && currentSelection < maxSelectionSize ) {
			StartCoroutine (setBuffer ());
			sound.PlayChange ();
			// move cursor
			currentSelection++;
			transform.position = cursorPositions [currentSelection].position;
			UpdatePreview (currentSelection);
		}else if (currentState == uiState.ColorSelect) {
			colorPreviewScript.UpdateSelection (currentSelection, 1);
			StartCoroutine (setBuffer ());
		}
	}

	void MoveCursorBackward(){
		if (currentState == uiState.CharacterSelect && currentSelection > 0) {
			StartCoroutine (setBuffer ());
			sound.PlayChange ();
			currentSelection--;
			transform.position = cursorPositions [currentSelection].position;
			UpdatePreview (currentSelection);
		} else if (currentState == uiState.ColorSelect) {
			colorPreviewScript.UpdateSelection (currentSelection, -1);
			StartCoroutine (setBuffer ());
		}

	}
	void ConfirmSelection(){
	// set rounds script
		if (currentState == uiState.CharacterSelect) {
			currentState = uiState.ColorSelect;
			SpecialsLoop [currentSelection].SetActive (false);
			sound.PlayConfirm ();
			ColorSelectorBlock.SetActive (true);
		} else if (currentState == uiState.ColorSelect) {
			currentState = uiState.LockedIn;
			if (playerNumber == 0) {
				if (ConvertToString (currentSelection) == "random") {
					roundScript.player1character = RandomizeCharacter ();
				} else {
					roundScript.player1character = ConvertToString (currentSelection);
					roundScript.player1ColorNumber = colorPreviewScript.GetColorNumber (currentSelection);
				}
				lockedIn = sceneLoader.SetReady (true, true);
			} else {
				if (ConvertToString (currentSelection) == "random") {
					roundScript.player2character = RandomizeCharacter ();
				} else {
					roundScript.player2character = ConvertToString (currentSelection);
					roundScript.player2ColorNumber = colorPreviewScript.GetColorNumber (currentSelection);
				}
				lockedIn = sceneLoader.SetReady (false, true);
			}
			ColorSelectorBlock.SetActive (false);
			UpdateSelected (currentSelection);
		}
		sound.PlayConfirm ();

	}
	void BackoutOfSelection(){
		if (currentState == uiState.ColorSelect) {
			currentState = uiState.CharacterSelect;
			ColorSelectorBlock.SetActive (false);
			UpdatePreview (currentSelection);
			sound.PlayBack ();
		}else if (currentState == uiState.LockedIn) {
			currentState = uiState.CharacterSelect;

			if (playerNumber == 0) {
				sceneLoader.SetReady (true, false);
			} else {
				sceneLoader.SetReady (false, false);
			}
			UpdatePreview (currentSelection);
			sound.PlayBack ();
		}
	}

	string ConvertToString(int number){
		switch (number) {
		case 0: 
			return "felicia";
		case 1: 
			return "hulk";
		case 2: 
			return "ryu";
		case 3: 
			return "random";
		case 4: 
			return "subzero";
		case 5: 
			return "yukiko";
		case 6: 
			return "baiken";
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
		foreach (GameObject g in SpecialsLoop) {
			g.SetActive (false);
		}
		previewsLoop [character].SetActive (true);
		SpecialsLoop [character].SetActive (true);
	}

	void UpdateSelected(int character){
		foreach (GameObject g in previewsLoop) {
			g.SetActive (false);
		}
		foreach (GameObject g in selectedsLoop) {
			g.SetActive (false);
		}
		foreach (GameObject g in SpecialsLoop) {
			g.SetActive (false);
		}
		if (selectedsLoop.Length > character) {
			selectedsLoop [character].SetActive (true);
		}
		sound.PlayCharacterSelect (character);
	}
}
