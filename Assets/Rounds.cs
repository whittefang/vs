using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Rounds : MonoBehaviour {
	int RoundFlag = 0;
	int P1W = 0;
	int P2W = 0;
	int sceneToload = 2;
	GameObject P1Star1;
	GameObject P1Star2;
	GameObject P2Star1;
	GameObject P2Star2;
	delegate void voidDel();
	voidDel win;
	void Awake(){
		DontDestroyOnLoad(gameObject);
	}
	// Use this for initialization
	void Start () {
		P1Star1 = GameObject.Find("Player1Star1");
		P1Star1.SetActive(false);
		P1Star2 = GameObject.Find("Player1Star2");
		P1Star2.SetActive(false);
		P2Star1 = GameObject.Find("Player2Star1");
		P2Star1.SetActive(false);
		P2Star2 = GameObject.Find("Player2Star2");
		P2Star2.SetActive(false);
		RoundChange();
	}
	public void	RoundChange(){
		RoundFlag ++;
		switch (RoundFlag){
			case 1:
				P1Star1.SetActive (false);
				P1Star2.SetActive (false);
				P2Star1.SetActive (false);
				P2Star2.SetActive (false);
				SceneManager.LoadScene(sceneToload);
				GetComponentInChildren<TextMesh>().text = "Round 1";
				break;
			case 2:
				SceneManager.LoadScene(sceneToload);
				GetComponentInChildren<TextMesh>().text = "Round 2";
				break;
			case 3:
				SceneManager.LoadScene(sceneToload);
				GetComponentInChildren<TextMesh>().text = "Round 3";
				break;
			case 4:
				GetComponentInChildren<TextMesh> ().text = "Player 1 Won!";
				P1W = 0;
				P2W = 0;
				RoundFlag = 0;
				RoundChange ();
				break;
			case 5:
				GetComponentInChildren<TextMesh> ().text = "Player 2 Won!";
				P1W = 0;
				P2W = 0;
				RoundFlag = 0;	
				RoundChange ();
				break;
			default:
				Debug.Log("Something went out of range inside round.");
				break;
		}
	}
	public void PlayerOneWin(){
		Debug.Log ("p1Win");
		StartCoroutine (wait (P2WinFunc));

	}
	void P2WinFunc(){
		
		P1W ++;
		switch(P1W){
		case 1:
			P1Star1.SetActive(true);
			RoundChange();
			break;
		case 2:
			P1Star2.SetActive(true);
			RoundFlag = 3;
			RoundChange();
			break;
		}
	}
	public void PlayerTwoWin(){
		Debug.Log ("p2Win");
		StartCoroutine(wait(P1WinFunc));

	}
	void P1WinFunc(){
		P2W ++;
		switch(P2W){
		case 1:
			P2Star1.SetActive(true);
			RoundChange();
			break;
		case 2:
			P2Star2.SetActive(true);
			RoundFlag = 4;
			RoundChange();
			break;
		}
	}
	IEnumerator wait(voidDel func){
		yield return new WaitForSeconds(3);
		func();
	}
}

