using UnityEngine;
using System.Collections;
using XInputDotNetPure; // Required in C#

public class InputScript : MonoBehaviour {
	public delegate void buttonDelegate();
	public delegate void ThumbstickDelegate(float x, float y);
	buttonDelegate aButtonPress, bButtonPress, xButtonPress, yButtonPress, aButtonRelease, bButtonRelease, xButtonRelease, yButtonRelease,
					rbButtonPress, rbButtonRelease, rTriggerPress, rTriggerRelease, axButtonPress;
	ThumbstickDelegate leftStick;

	TimeManagerScript timeManager;
	PlayerIndex playerIndex;
	GamePadState state;
	GamePadState prevState;
	public int playerNumber = 0;
	// Use this for initialization
	void Start () {

		timeManager = GameObject.Find ("MasterGameObject").GetComponent<TimeManagerScript> ();
		playerIndex = (PlayerIndex)playerNumber;
	}

	// Update is called once per frame
	void FixedUpdate () {
		prevState = state;
		state = GamePad.GetState (playerIndex, GamePadDeadZone.None);

		// send input for movement(state.ThumbSticks.Left.X, state.ThumbSticks.Left.Y);
		leftStick(state.ThumbSticks.Left.X, state.ThumbSticks.Left.Y);

		// double button input a and x
		if (((prevState.Buttons.X == ButtonState.Released && state.Buttons.X == ButtonState.Pressed) && state.Buttons.A == ButtonState.Pressed)
			|| ((prevState.Buttons.A == ButtonState.Released && state.Buttons.A == ButtonState.Pressed) && state.Buttons.X == ButtonState.Pressed)
			|| ((prevState.Buttons.A == ButtonState.Released && state.Buttons.A == ButtonState.Pressed) && (prevState.Buttons.X == ButtonState.Released && state.Buttons.X == ButtonState.Pressed))){
			axButtonPress ();

		}

		// Detect if a button was pressed this frame
		if (prevState.Buttons.A == ButtonState.Released && state.Buttons.A == ButtonState.Pressed && aButtonPress!=null) {
			aButtonPress ();
			StartCoroutine (BufferButton(aButtonPress));
		}
		// Detect if a button was released this frame
		if (prevState.Buttons.A == ButtonState.Pressed && state.Buttons.A == ButtonState.Released&& aButtonRelease!=null) {
			aButtonRelease ();
			StartCoroutine (BufferButton(aButtonRelease));
		}

		// Detect if a button was pressed this frame
		if (prevState.Buttons.X == ButtonState.Released && state.Buttons.X == ButtonState.Pressed&& xButtonPress!=null) {
			
			xButtonPress ();
			StartCoroutine (BufferButton(xButtonPress));
		}
		// Detect if a button was released this frame
		if (prevState.Buttons.X == ButtonState.Pressed && state.Buttons.X == ButtonState.Released&& xButtonRelease!=null) {
			
			xButtonRelease ();
			StartCoroutine (BufferButton(xButtonRelease));
		}

		// Detect if a button was pressed this frame
		if (prevState.Buttons.Y == ButtonState.Released && state.Buttons.Y == ButtonState.Pressed&& yButtonPress!=null) {
			yButtonPress ();
			StartCoroutine (BufferButton(yButtonPress));
		}
		// Detect if a button was released this frame
		if (prevState.Buttons.Y == ButtonState.Pressed && state.Buttons.Y == ButtonState.Released&& yButtonRelease!=null) {
			yButtonRelease ();
			StartCoroutine (BufferButton(yButtonRelease));
		}

		// x button press
		if (state.Buttons.B == ButtonState.Pressed && prevState.Buttons.B == ButtonState.Released&& bButtonPress!=null) {
			bButtonPress ();
			StartCoroutine (BufferButton(bButtonPress));
		}
		// Detect if a button was released this frame
		if (prevState.Buttons.B == ButtonState.Pressed && state.Buttons.B == ButtonState.Released&& bButtonRelease!=null) {
			bButtonRelease ();
			StartCoroutine (BufferButton(bButtonRelease));
		}
		// x button press
		if (state.Buttons.RightShoulder == ButtonState.Pressed && prevState.Buttons.RightShoulder == ButtonState.Released&& rbButtonPress!=null) {
			rbButtonPress ();
			StartCoroutine (BufferButton(rbButtonPress));
		}
		// Detect if a button was released this frame
		if (prevState.Buttons.RightShoulder == ButtonState.Pressed && state.Buttons.RightShoulder == ButtonState.Released&& rbButtonRelease!=null) {
			rbButtonRelease ();
			StartCoroutine (BufferButton(rbButtonRelease));
		}
		// Detect if rt button was released this frame
		if (prevState.Triggers.Right > .5f && state.Triggers.Right < .5f && rTriggerRelease!=null) {
			rTriggerRelease ();
			StartCoroutine (BufferButton(rTriggerRelease));
		}
		// Detect if rt button was pressed this frame
		if (prevState.Triggers.Right < .5f && state.Triggers.Right > .5f && rTriggerPress!=null) {
			rTriggerPress ();
			StartCoroutine (BufferButton(rTriggerPress));
		}



	}

	public void assignAXButton(buttonDelegate axPress){
		axButtonPress = axPress;
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
	// takes in function delegate and assigns them to appropriate buttons
	public void assignRBButton(buttonDelegate rbPress, buttonDelegate rbRelease){
		rbButtonPress = rbPress;
		rbButtonRelease = rbRelease;
	}
	// takes in function delegate and assigns them to appropriate buttons
	public void assignRT(buttonDelegate rtPress, buttonDelegate rtRelease){
		rTriggerPress = rtPress;
		rTriggerRelease = rtRelease;
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

	IEnumerator BufferButton(buttonDelegate button){
		for (int x = 0; x < 6;){
			button ();
			yield return null;
			if (!timeManager.CheckIfTimePaused()) {
				x++;
			}
		}
	}
}
