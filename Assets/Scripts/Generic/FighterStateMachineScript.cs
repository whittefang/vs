using UnityEngine;
using System.Collections;

public class FighterStateMachineScript : MonoBehaviour {
	public string currentState = "neutral";
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public string GetState(){
		return currentState;
	}
	public void SetState(string newState){
		currentState = newState;
	}
}
