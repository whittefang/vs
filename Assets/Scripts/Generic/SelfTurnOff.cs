using UnityEngine;
using System.Collections;

public class SelfTurnOff : MonoBehaviour {
	public float delay = 0;
	// Use this for initialization
	void OnEnable () {
		Invoke ("TurnOff", delay);
	}
	void TurnOff(){
		gameObject.SetActive (false);
	}
	// Update is called once per frame
	void Update () {
	
	}

}
