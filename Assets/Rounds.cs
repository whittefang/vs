using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Rounds : MonoBehaviour {
	//variable for prefab
	public GameObject FeliciaPrefab, RyuPrefab;
	public string player1character = "ryu";
	public string player2character = "ryu";

	public bool FalseMeansTest = false;
	int RoundFlag = 0;
	int P1W = 0;
	int P2W = 0;
	public int sceneToload = 2;
	GameObject P1Star1;
	GameObject P1Star2;
	GameObject P2Star1;
	GameObject P2Star2;
	delegate void voidDel();
	voidDel win;
	void Awake(){
		if (FalseMeansTest == true) {
			Debug.Log("object can not be destroyed");
			DontDestroyOnLoad(gameObject);
			SceneManager.activeSceneChanged += loadx;
		}
	}
	void loadx(Scene x, Scene y){
		Debug.Log("i loaded");
		if (SceneManager.GetActiveScene().buildIndex == 2) {
			PlayersSpawn();
		}

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
		if (FalseMeansTest == true){
			//RoundChange();

			//instead load into character select

			SceneManager.LoadScene(1);
		}

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
	public GameObject CharacterSpawn(bool isPlayerOne){
		Vector3 spawnPosition = new Vector3(5, -1, 0);
		string character = player2character;
		if (isPlayerOne){
			spawnPosition.x = -5;
			character = player1character;
		}
		GameObject player;
		switch(character){
		case "ryu":
			player = Instantiate(RyuPrefab, spawnPosition, Quaternion.identity) as GameObject;
			break;
		case "felicia":
			player = Instantiate(FeliciaPrefab, spawnPosition, Quaternion.identity) as GameObject;
			break;
		default :
			player = new GameObject();
			Debug.Log("error in characterspawn. misspelling.");
			break;

		}
		return player;
	}

	public void PlayersSpawn(){
		GameObject p1, p2;
		p1 = CharacterSpawn(true);
		p2 = CharacterSpawn(false);
		p1.GetComponent<PlayerNumberSetScript>().SetPlayer("playerOne", p2);
		p2.GetComponent<PlayerNumberSetScript>().SetPlayer("playerTwo", p1);
		p2.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().color = new Color(.7f, .7f , .7f);
		GameObject.Find("Camera").GetComponent<CameraMoveScript>().SetPlayers(p1, p2);
	}



}

