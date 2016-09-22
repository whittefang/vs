using UnityEngine;
using System.Collections;

public class PersonaCardsScript : MonoBehaviour {
	public GameObject[] Cards;
	int spot = 0;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public void ResetCards(){
		spot = 0;
		foreach (GameObject g in Cards) {
			g.SetActive (true);
		}
	}
	public void RemoveCard(){
		if (spot < 3){
			Cards [spot].SetActive (false);
			spot++;
		}
	}
}
