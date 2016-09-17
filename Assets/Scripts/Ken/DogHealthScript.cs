using UnityEngine;
using System.Collections;

public class DogHealthScript : MonoBehaviour {
	int maxCards = 4;
	int currentCards = 4;
	public PersonaCardsScript Cards;
	bool alive = true;
	DogAttackScript dogAttackScript;
	FighterStateMachineScript state;
	// Use this for initialization
	void Start () {
		dogAttackScript = GetComponentInParent<DogAttackScript> ();
		state = GetComponentInParent<FighterStateMachineScript>();
	}
	public void SetOtherPlayer(bool isP1){
		Cards.transform.parent = GameObject.Find ("Camera").transform;
		if (isP1) {
			Cards.transform.eulerAngles = new Vector3 (0, 180, 0);
			tag = "P1Persona";
		} else {
			Cards.transform.eulerAngles = new Vector3 (0, 0, 0);
			tag = "P2Persona";
		}
		Cards.transform.position = new Vector3(Cards.transform.parent.position.x, Cards.transform.parent.position.y, -1f);
	}
	// Update is called once per frame
	void Update () {
	
	}
	public bool CheckAlive(){
		return alive;
	}
	void Reactivate(){
		alive = true;
		currentCards = maxCards;
		dogAttackScript.StartRecover ();
	}
	public void DealDamage(){
		if (state.GetState () != "invincible") {
			currentCards--;
			dogAttackScript.StartHit ();
			Cards.RemoveCard ();
			//play hit sound(glass break)
			if (currentCards <= 0) {
				alive = false;
				Invoke ("Reactivate", 7);
			}
		}
	}

}
