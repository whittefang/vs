using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Rounds : MonoBehaviour {
	int RoundFlag = 0;
	int P1W = 0;
	int P2W = 0;
	GameObject P1Star1;
	GameObject P1Star2;
	GameObject P2Star1;
	GameObject P2Star2;
	void Awake(){
		DontDestroyOnLoad(gameObject);
	}
	// Use this for initialization
	void Start () {
		RoundChange();
		P1Star1 = GameObject.Find("Player1Star1");
		P1Star1.SetActive(false);
		P1Star2 = GameObject.Find("Player1Star2");
		P1Star2.SetActive(false);
		P2Star1 = GameObject.Find("Player2Star1");
		P2Star1.SetActive(false);
		P2Star2 = GameObject.Find("Player2Star2");
		P2Star2.SetActive(false);
	}
	public void	RoundChange(){
		RoundFlag ++;
		switch (RoundFlag){
			case 1:
				SceneManager.LoadScene(1);
				GetComponentInChildren<TextMesh>().text = "Round 1";
				break;
			case 2:
				SceneManager.LoadScene(1);
				GetComponentInChildren<TextMesh>().text = "Round 2";
				break;
			case 3:
				SceneManager.LoadScene(1);
				GetComponentInChildren<TextMesh>().text = "Round 3";
				break;
			case 4:
				GetComponentInChildren<TextMesh>().text = "Player 1 Won!";
				break;
			case 5:
				GetComponentInChildren<TextMesh>().text = "Player 2 Won!";
				break;
			default:
				Debug.Log("Something went out of range inside round.");
				break;
		}
	}
	public void PlayerOneWin(){
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
}

