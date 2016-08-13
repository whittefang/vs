using UnityEngine;
using System.Collections;



public class PlayerNumberSetScript : MonoBehaviour {
	public string Player;

	public delegate void voidDel(bool);
	voidDel optFunc;

	public GameObject body, hurtbox,  soundPlayer;

	// Use this for initialization
	void Start () {
		SetPlayer (Player);

	}
	public void SetPlayer(string playerTag){
		Player = playerTag;
		if (Player == "playerOne") {
			soundPlayer.tag = "P1Sound";
			body.GetComponent<InputScript> ().SetPlayerNumber (0);
			hurtbox.tag = "playerOneHurtbox";
			if (optFunc != null) {
				optFunc(true);
			}
		} else {
			soundPlayer.tag = "P2Sound";
			body.GetComponent<InputScript> ().SetPlayerNumber (1);
			hurtbox.tag = "playerTwoHurtbox";
			if (optFunc != null) {
				optFunc(false);
			}
		}

		body.tag = Player;
		hurtbox.GetComponent<HealthScript> ().SetPlayer (Player);
	}
	// Update is called once per frame
	void Update () {
	
	}
	public void SetOptFunc(voidDel newFunc){
		optFunc = newFunc;
	}
}
