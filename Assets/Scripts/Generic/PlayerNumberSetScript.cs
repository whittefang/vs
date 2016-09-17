using UnityEngine;
using System.Collections;



public class PlayerNumberSetScript : MonoBehaviour {
	public string Player;

	public delegate void voidDel(bool b);
	voidDel optFunc;

	public GameObject body, hurtboxes, hitboxes,  soundPlayer;
	public bool testMode = false;
	public GameObject testOtherPlayer;
	bool showHitboxes = false;
	// Use this for initialization
	void Awake () {
		if (testMode){
			SetPlayer(Player,testOtherPlayer );
			body.GetComponent<InputScript> ().inputEnabled  = (true);
		}

	}
	public void SetPlayer(string playerTag, GameObject otherPlayer){
		Player = playerTag;
		if (Player == "playerOne") {
			GameObject.Find ("MasterGameObject").GetComponent<TimeManagerScript> ().p1Body = body.GetComponent<Rigidbody2D> ();
			body.GetComponentInChildren<HealthScript>().sounds = otherPlayer.GetComponent<PlayerNumberSetScript>().soundPlayer.GetComponent<SoundsPlayer>();
			soundPlayer.tag = "P1Sound";
			body.GetComponent<InputScript> ().SetPlayerNumber (0);
			body.GetComponent<PlayerMovementScript>().otherPlayer = otherPlayer.transform.GetChild(0).gameObject;
			//body.GetComponent<PlayerMovementScript> ().CheckFacing ();
			foreach (Transform child in hurtboxes.transform) {
				child.tag = "playerOneHurtbox";
			}
			foreach (HitboxScript child in GetComponentsInChildren<HitboxScript>(true)) {
				child.AddTagToDamage("playerTwoHurtbox");
				child.pnum = 0;
			}
			foreach (ProximityBlockScript child in GetComponentsInChildren<ProximityBlockScript>(true)) {
				child.tagToDamage = ("playerTwo");
			}
			foreach (ProjectileScript child in GetComponentsInChildren<ProjectileScript>(true)) {
				child.projectileOwner = 0;
			}
			if (optFunc != null) {
				optFunc(true);
			}
		} else {
			GameObject.Find ("MasterGameObject").GetComponent<TimeManagerScript> ().p2Body = body.GetComponent<Rigidbody2D> ();
			soundPlayer.tag = "P2Sound";
			body.GetComponentInChildren<HealthScript>().sounds = otherPlayer.GetComponent<PlayerNumberSetScript>().soundPlayer.GetComponent<SoundsPlayer>();
			body.GetComponent<InputScript> ().SetPlayerNumber (1);
			body.GetComponent<PlayerMovementScript>().otherPlayer = otherPlayer.transform.GetChild(0).gameObject;
			//body.GetComponent<PlayerMovementScript> ().CheckFacing ();
			foreach (Transform child in hurtboxes.transform) {
				child.tag = "playerTwoHurtbox";
			}
			foreach (HitboxScript child in GetComponentsInChildren<HitboxScript>(true)) {
				child.AddTagToDamage( "playerOneHurtbox");
				child.pnum = 1;
			}
			foreach (ProximityBlockScript child in GetComponentsInChildren<ProximityBlockScript>(true)) {
				child.tagToDamage = ("playerOne");
			}
			foreach (ProjectileScript child in GetComponentsInChildren<ProjectileScript>(true)) {
				child.projectileOwner = 1;
			}
			if (optFunc != null) {
				optFunc(false);
			}
		}

		body.tag = Player;
		hurtboxes.GetComponent<HealthScript> ().SetPlayer (Player, otherPlayer);
	}
	// Update is called once per frame
	void FixedUpdate () {
		if (Input.GetKeyDown(KeyCode.F1)){
			showHitboxes = !showHitboxes; 
			foreach (SpriteRenderer child in hurtboxes.GetComponentsInChildren<SpriteRenderer>(true)) {
				child.enabled = showHitboxes;
			}
			foreach (SpriteRenderer child in hitboxes.GetComponentsInChildren<SpriteRenderer>(true)) {
				child.enabled = showHitboxes;
			}
		}
	}
	public void SetOptFunc(voidDel newFunc){
		optFunc = newFunc;
	}
}
