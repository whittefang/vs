using UnityEngine;
using System.Collections;



public class PlayerNumberSetScript : MonoBehaviour {
	public string Player;

	public delegate void voidDel(bool b);
	voidDel optFunc;

	public GameObject body, hurtbox,  soundPlayer;

	// Use this for initialization
	void Start () {
		//SetPlayer(Player);

	}
	public void SetPlayer(string playerTag, GameObject otherPlayer){
		Player = playerTag;
		if (Player == "playerOne") {
			
			body.GetComponentInChildren<HealthScript>().sounds = otherPlayer.GetComponent<PlayerNumberSetScript>().soundPlayer.GetComponent<SoundsPlayer>();
			soundPlayer.tag = "P1Sound";
			body.GetComponent<InputScript> ().SetPlayerNumber (0);
			body.GetComponent<PlayerMovementScript>().otherPlayer = otherPlayer.transform.GetChild(0).gameObject;
			hurtbox.tag = "playerOneHurtbox";
			if (optFunc != null) {
				optFunc(true);
			}
		} else {
			soundPlayer.tag = "P2Sound";
			body.GetComponentInChildren<HealthScript>().sounds = otherPlayer.GetComponent<PlayerNumberSetScript>().soundPlayer.GetComponent<SoundsPlayer>();
			body.GetComponent<InputScript> ().SetPlayerNumber (1);
			body.GetComponent<PlayerMovementScript>().otherPlayer = otherPlayer.transform.GetChild(0).gameObject;
			hurtbox.tag = "playerTwoHurtbox";
			if (optFunc != null) {
				optFunc(false);
			}
		}

		body.tag = Player;
		hurtbox.GetComponent<HealthScript> ().SetPlayer (Player, otherPlayer);
	}
	// Update is called once per frame
	void Update () {
	
	}
	public void SetOptFunc(voidDel newFunc){
		optFunc = newFunc;
	}
}
