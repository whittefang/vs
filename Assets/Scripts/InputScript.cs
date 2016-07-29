using UnityEngine;
using System.Collections;
using XInputDotNetPure; // Required in C#

public class InputScript : MonoBehaviour {
	public delegate void buttonDelegate();
	public delegate void ThumbstickDelegate(float x, float y);
	buttonDelegate aButtonPress, bButtonPress, xButtonPress, yButtonPress, aButtonRelease, bButtonRelease, xButtonRelease, yButtonRelease;
	ThumbstickDelegate leftStick;

	PlayerIndex playerIndex;
	GamePadState state;
	GamePadState prevState;
	public int playerNumber = 0;
	// Use this for initialization
	void Start () {

		playerIndex = (PlayerIndex)playerNumber;
	}

	// Update is called once per frame
	void FixedUpdate () {
		prevState = state;
		state = GamePad.GetState (playerIndex, GamePadDeadZone.None);

		// send input for movement(state.ThumbSticks.Left.X, state.ThumbSticks.Left.Y);
		leftStick(state.ThumbSticks.Left.X, state.ThumbSticks.Left.Y);
		// Detect if a button was pressed this frame
		if (prevState.Buttons.A == ButtonState.Released && state.Buttons.A == ButtonState.Pressed && aButtonPress!=null) {
			aButtonPress ();
		}
		// Detect if a button was released this frame
		if (prevState.Buttons.A == ButtonState.Pressed && state.Buttons.A == ButtonState.Released&& aButtonRelease!=null) {
			aButtonRelease ();
		}

		// Detect if a button was pressed this frame
		if (prevState.Buttons.X == ButtonState.Released && state.Buttons.X == ButtonState.Pressed&& xButtonPress!=null) {
			xButtonPress ();
		}
		// Detect if a button was released this frame
		if (prevState.Buttons.X == ButtonState.Pressed && state.Buttons.X == ButtonState.Released&& xButtonRelease!=null) {
			xButtonRelease ();
		}

		// Detect if a button was pressed this frame
		if (prevState.Buttons.Y == ButtonState.Released && state.Buttons.Y == ButtonState.Pressed&& yButtonPress!=null) {
			yButtonPress ();
		}
		// Detect if a button was released this frame
		if (prevState.Buttons.Y == ButtonState.Pressed && state.Buttons.Y == ButtonState.Released&& yButtonRelease!=null) {
			yButtonRelease ();
		}

		// x button press
		if (state.Buttons.B == ButtonState.Pressed && prevState.Buttons.B == ButtonState.Released&& bButtonPress!=null) {
			bButtonPress ();
		}
		// Detect if a button was released this frame
		if (prevState.Buttons.B == ButtonState.Pressed && state.Buttons.B == ButtonState.Released&& bButtonRelease!=null) {
			bButtonRelease ();
		}


	}

	// takes in function delegate and assigns them to appropriate buttons
	public void assignAButton(buttonDelegate aPress, buttonDelegate aRelease){
		aButtonPress = aPress;
		aButtonRelease = aRelease;
	}
	// takes in function delegate and assigns them to appropriate buttons
	public void assignBButton(buttonDelegate bPress, buttonDelegate bRelease){
		bButtonPress = bPress;
		bButtonRelease = bRelease;
	}
	// takes in function delegate and assigns them to appropriate buttons
	public void assignXButton(buttonDelegate xPress, buttonDelegate xRelease){
		xButtonPress = xPress;
		xButtonRelease = xRelease;
	}
	// takes in function delegate and assigns them to appropriate buttons
	public void assignYButton(buttonDelegate yPress, buttonDelegate yRelease){
		yButtonPress = yPress;
		yButtonRelease = yRelease;
	}
	public void SetThumbstick(ThumbstickDelegate newLeftStick){
		leftStick = newLeftStick;
	}
	public void SetPlayerNumber (int newNum){
		playerNumber = newNum;
	}
	public float GetX(bool raw = true){
		if (raw) {
			return state.ThumbSticks.Left.X;
		} else if (Mathf.Abs(state.ThumbSticks.Left.X) > .2f){
			return state.ThumbSticks.Left.X;
		}else {
			return 0;
		}

	}
	public float GetY(bool raw = true){
		if (raw) {
			return state.ThumbSticks.Left.Y;
		} else if (Mathf.Abs(state.ThumbSticks.Left.Y) > .2f){

			return state.ThumbSticks.Left.Y;
		}else {
			return 0;
		}
		
	}
}
