﻿using UnityEngine;
using System.Collections;
using XInputDotNetPure; // Required in C#

public class LevelSelectScript : MonoBehaviour {

	public bool characterSelect = true, colorSelect = false;
	public int playerNumber = 0;
	public int currentSelection = 0;
	public int maxSelectionSize = 6;
	public int colorSelection = 0;
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

		if (state.ThumbSticks.Left.X > 0 && buffer && !selectionMade) {
			if (currentSelection < maxSelectionSize) {
				StartCoroutine (setBuffer ());
				sound.PlayChange ();
				// move cursor
				currentSelection++;

				transform.position = cursorPositions [currentSelection];
				UpdatePreview (currentSelection);
			}
		}
		if (state.ThumbSticks.Left.X < 0 && buffer && !selectionMade) {
			if (currentSelection > 0) {
				StartCoroutine (setBuffer ());
				sound.PlayChange ();
				currentSelection--;
				transform.position = cursorPositions [currentSelection];
				UpdatePreview (currentSelection);
			}
		}

		if (prevState.Buttons.A == ButtonState.Released && state.Buttons.A == ButtonState.Pressed && !selectionMade) {
			// set rounds script
			if (playerNumber == 0) {
				if (characterSelect) {
					if (ConvertToString (currentSelection) == "random") {
						roundScript.player1character = RandomizeCharacter ();
					} else {
						roundScript.player1character = ConvertToString (currentSelection);
					}
				} else {
					sceneLoader.SetScene (true, currentSelection + 4);
				}
				lockedIn = sceneLoader.SetReady (true, true);

			} else {
				if (characterSelect) {
					if (ConvertToString (currentSelection) == "random") {
						roundScript.player2character = RandomizeCharacter ();
					} else {
						roundScript.player2character = ConvertToString (currentSelection);
					}
				} else {
					sceneLoader.SetScene (false, currentSelection + 4);
				}
				lockedIn = sceneLoader.SetReady (false, true);
			}


			UpdateSelected (currentSelection);
			//colorPreviewScript.UpdateSelection (currentSelection, 0);
			sound.PlayConfirm ();
			selectionMade = true;
		}
	
		
		if (prevState.Buttons.B == ButtonState.Released && state.Buttons.B == ButtonState.Pressed && !lockedIn && selectionMade) {
		

			sound.PlayBack ();
			if (playerNumber == 0) {
				sceneLoader.SetReady (true, false);
			} else {
				sceneLoader.SetReady (false, false);
			}
			UpdatePreview (currentSelection);

			selectionMade = false;
			colorSelect = true;
			
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

		if (selectedsLoop.Length > character) {
			selectedsLoop [character].SetActive (true);
		}
		sound.PlayCharacterSelect (character);
	}
}
