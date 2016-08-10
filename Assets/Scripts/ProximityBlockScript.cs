using UnityEngine;
using System.Collections;

public class ProximityBlockScript : MonoBehaviour {
	public string tagToDamage;
	PlayerMovementScript PMS;
	// Use this for initialization
	void Start () {
//		if (transform.parent.GetComponent<playerCheck> ().IsPlayerOne ()) {
//			tagToDamage = "playerTwo";
//		} else {
//			tagToDamage = "playerOne";
//		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void OnTriggerEnter2D(Collider2D other){
		if (other.tag == tagToDamage) {
			PMS = other.GetComponent<PlayerMovementScript> ();
			PMS.setProximityBlock (true);
		}
	}
	void OnTriggerExit2D(Collider2D other){
		if (other.tag == tagToDamage) {
			if (PMS != null) {
				PMS.setProximityBlock (false);
			}
		}
	}
	void OnDisable(){
		if (PMS != null) {
			PMS.setProximityBlock (false);
		}
	}
}
