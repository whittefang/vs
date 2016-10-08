using UnityEngine;
using System.Collections;

public class LoadScreenScript : MonoBehaviour {
	public GameObject backgroundBottom, backgroundTop, stageImage;
	public GameObject[] characterPortraitsP1,characterPortraitsP2, levelImages;
	GameObject playerOneChar, PlayerTwoChar;
	Rounds rounds;
	// Use this for initialization
	void Start () {
		rounds = GameObject.Find ("DoNotDestroy").GetComponent<Rounds> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void OnEnable(){
		StartCoroutine (AnimateBackGround ());	
	}
	IEnumerator AnimateBackGround(){
		backgroundTop.transform.position = new Vector3 (0, 10, -2);
		backgroundBottom.transform.position = new Vector3 (0, -10, -2);
		for (int i = 0; i < 30; i++) {
			backgroundTop.transform.Translate (new Vector3 (0, -.33f, 0));
			backgroundBottom.transform.Translate (new Vector3 (0, .33f, 0));
			yield return null;
		}
		GameObject.Find ("Main Camera").GetComponent<UIsounds> ().PlayLoad ();
		backgroundBottom.transform.position =  new Vector3 (0, 0, -2);
		backgroundTop.transform.position=  new Vector3 (0, 0, -2);
		int charNumP1 = ConvertCharStringToInt (rounds.player1character);
		int charNumP2 = ConvertCharStringToInt (rounds.player2character);
		characterPortraitsP1 [charNumP1].SetActive (true);
		characterPortraitsP2 [charNumP2].SetActive (true);
		characterPortraitsP1 [charNumP1].transform.position = new Vector3( -10, 0, -3);
		characterPortraitsP2 [charNumP2].transform.position = new Vector3( 10, 0, -3);
		for (int i = 0; i < 60; i++) {
			characterPortraitsP1[charNumP1].transform.position = Vector3.Lerp(characterPortraitsP1[charNumP1].transform.position, new Vector3(-3,0,-3),  .08f);
			characterPortraitsP2[charNumP2].transform.position = Vector3.Lerp(characterPortraitsP2[charNumP2].transform.position, new Vector3(3,0,-3), .08f);
			yield return null;
		}

	}
	int ConvertCharStringToInt(string Char){
		switch (Char) {
		case "felicia": 
			return 0;
		case "hulk": 
			return 1;
		case "ryu":
			return 2;
		case "subzero": 
			return 3;
		case "yukiko": 
			return 4;
		case "baiken": 
			return 5;
		case "shana": 
			return 6;
		default:
			return 0;
		}
	}
}
