using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Rounds : MonoBehaviour {
	//variable for prefab
	public GameObject FeliciaPrefab, HulkPrefab, RyuPrefab,ShanaPrefab, SubzeroPrefab, kenPrefab, BaikenPrefab;
	public string player1character = "ryu";
	public string player2character = "ryu";
	public TextMesh textShadow;
	public bool FalseMeansTest = false;
	int RoundFlag = 1;
	int P1W = 0;
	int P2W = 0;
	public int sceneToload = 2;
	GameObject P1Star1;
	GameObject P1Star2;
	GameObject P2Star1;
	GameObject P2Star2;
	delegate void voidDel();
	voidDel win;
	ExMeter ex;
	FollowScript fs;
	public Texture2D[] feliciaColor;
	public Texture2D[] hulkColor;
	public Texture2D[] ryuColor;
	public Texture2D[] ryuFireballColor;
	public Texture2D[] ShanaColor;
	public Texture2D[] subzeroColor;
	public Texture2D[] kenColor;
	public Texture2D[] kenDogColor;
	public Texture2D[] baikenColor;
	public int player1ColorNumber = 0;
	public int player2ColorNumber = 0;
	LeftHpBarChange leftHpBarSprite;
	RightHpBarChange rightHpBarSprite;

	public GameObject leftGreen, leftRed;
	public GameObject rightGreen, rightRed;

	public GameObject exLeftBorder, exRightBorder; 
	public int tutorialPlayerNumber = 0;

	public GameObject round1Anim, round2Anim, round3Anim, readyAnim, fightAnim;

	GameObject p1, p2;

	void Awake(){
		if (FalseMeansTest == true) {
			Debug.Log ("object can not be destroyed");
			DontDestroyOnLoad (gameObject);
			SceneManager.activeSceneChanged += loadx;
		} else {
			StartCoroutine(SetRoundText("Round 1"));
		}

		ex = GetComponent<ExMeter>();
		fs = GetComponent<FollowScript>();
	}
	void loadx(Scene x, Scene y){
		Debug.Log("i loaded");
		if (SceneManager.GetActiveScene().buildIndex >= 4) {
			if (RoundFlag == 1){
				StartCoroutine(SetRoundText("Round 1"));
			}
			PlayersSpawn();
			fs.transformToFollow = GameObject.Find("Camera");
			
			exLeftBorder.SetActive(true);
			exRightBorder.SetActive(true);
		}else{
			if (SceneManager.GetActiveScene ().buildIndex == 3) {
				GameObject.Find ("RyuTutorialPlayer").GetComponentInChildren<InputScript> ().SetPlayerNumber (tutorialPlayerNumber);
			}
			exLeftBorder.SetActive(false);
			exRightBorder.SetActive(false);
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
				
				break;
			case 2:
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
				StartCoroutine(SetRoundText("Round 2"));
				break;
			case 3:
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
				StartCoroutine(SetRoundText("Round 3"));
				break;
			case 4:
				
				P1W = 0;
				P2W = 0;
				RoundFlag = 1;
				SceneManager.LoadScene(1);
				ex.setExMetersToZero();
				P1Star1.SetActive (false);
				P1Star2.SetActive (false);
				P2Star1.SetActive (false);
				P2Star2.SetActive (false);
				break;
			case 5:
				
				P1W = 0;
				P2W = 0;
				RoundFlag = 1;
				SceneManager.LoadScene(1);
				ex.setExMetersToZero();
				P1Star1.SetActive (false);
				P1Star2.SetActive (false);
				P2Star1.SetActive (false);
				P2Star2.SetActive (false);
				break;
			default:
				Debug.Log("Something went out of range inside round.");
				break;
		}
	}
	public void PlayerOneWin(){
		Debug.Log ("p1Win");
		//StartCoroutine(SetRoundText("Player 1 Wins!"));
		StartCoroutine (wait (P1WinFunc));

	}
	void P1WinFunc(){
		
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
		//StartCoroutine(SetRoundText("Player 2 Wins!"));
		Debug.Log ("p2Win");
		StartCoroutine(wait(P2WinFunc));

	}
	void P2WinFunc(){
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
			if (isPlayerOne){
				leftHpBarSprite = GameObject.Find("LeftHpBar").GetComponent<LeftHpBarChange>();
				leftHpBarSprite.SetLeftBoarderArt("ryu");
				foreach (ColorPaletteSwap tmp in player.GetComponentsInChildren<ColorPaletteSwap>(true)){
					if (tmp.gameObject.name == "Body") {
						tmp.LoadColors (ryuColor[player1ColorNumber]);
					}else if (tmp.gameObject.name == "fireballSprite"){
						tmp.LoadColors (ryuFireballColor[player1ColorNumber]);
					}
				}
			}else{
				rightHpBarSprite = GameObject.Find("RightHpBar").GetComponent<RightHpBarChange>();
				rightHpBarSprite.SetRightBoarderArt("ryu");
				foreach (ColorPaletteSwap tmp in player.GetComponentsInChildren<ColorPaletteSwap>(true)){
					if (tmp.gameObject.name == "Body") {
						tmp.LoadColors (ryuColor[player2ColorNumber]);
					}else if (tmp.gameObject.name == "fireballSprite"){
						tmp.LoadColors (ryuFireballColor[player2ColorNumber]);
					}
				}
			}
			break;
		case "felicia":
			player = Instantiate(FeliciaPrefab, spawnPosition, Quaternion.identity) as GameObject;
			if (isPlayerOne){
				player.GetComponentInChildren<ColorPaletteSwap>().LoadColors(feliciaColor[player1ColorNumber]);
				leftHpBarSprite = GameObject.Find("LeftHpBar").GetComponent<LeftHpBarChange>();
				leftHpBarSprite.SetLeftBoarderArt("felicia");
			}
			else{
			player.GetComponentInChildren<ColorPaletteSwap>().LoadColors(feliciaColor[player2ColorNumber]);
			rightHpBarSprite = GameObject.Find("RightHpBar").GetComponent<RightHpBarChange>();
			rightHpBarSprite.SetRightBoarderArt("felicia");
			}
			break;
		case "hulk":
			player = Instantiate(HulkPrefab, spawnPosition, Quaternion.identity) as GameObject;
			if (isPlayerOne){
				player.GetComponentInChildren<ColorPaletteSwap>().LoadColors(hulkColor[player1ColorNumber]);
				leftHpBarSprite = GameObject.Find("LeftHpBar").GetComponent<LeftHpBarChange>();
				leftHpBarSprite.SetLeftBoarderArt("hulk");
			}
			else{
				player.GetComponentInChildren<ColorPaletteSwap>().LoadColors(hulkColor[player2ColorNumber]);
				rightHpBarSprite = GameObject.Find("RightHpBar").GetComponent<RightHpBarChange>();
				rightHpBarSprite.SetRightBoarderArt("hulk");
			}
			break;
		case "subzero":
			player = Instantiate(SubzeroPrefab, spawnPosition, Quaternion.identity) as GameObject;
			if (isPlayerOne){
				player.GetComponentInChildren<ColorPaletteSwap>().LoadColors(subzeroColor[player1ColorNumber]);
				leftHpBarSprite = GameObject.Find("LeftHpBar").GetComponent<LeftHpBarChange>();
				leftHpBarSprite.SetLeftBoarderArt("subzero");
			}
			else{
			rightHpBarSprite = GameObject.Find("RightHpBar").GetComponent<RightHpBarChange>();
			rightHpBarSprite.SetRightBoarderArt("subzero");
			player.GetComponentInChildren<ColorPaletteSwap>().LoadColors(subzeroColor[player2ColorNumber]);
			}
			break;
		case "yukiko":
			player = Instantiate(kenPrefab, spawnPosition, Quaternion.identity) as GameObject;
			if (isPlayerOne){
				foreach (ColorPaletteSwap tmp in player.GetComponentsInChildren<ColorPaletteSwap>()){
					if (tmp.gameObject.name == "Body") {
						tmp.LoadColors (kenColor [player1ColorNumber]);
					}else if (tmp.gameObject.name == "DogSprite"){
						tmp.LoadColors (kenDogColor [player1ColorNumber]);
					}
				}
				leftHpBarSprite = GameObject.Find("LeftHpBar").GetComponent<LeftHpBarChange>();
				leftHpBarSprite.SetLeftBoarderArt("yukiko");
//				
//				leftGreen = GameObject.Find("LeftHpBar");
//				leftGreen.transform.eulerAngles = new Vector3( 0f, 0f, -2.306f);
//				leftGreen.transform.localPosition = new Vector3(.79f, -.03f, -1.1f);
//				leftGreen.transform.localScale = new Vector3(6.5f, .5f, 0f);
//
//				leftRed = GameObject.Find("LeftHpBarRed");
//				leftRed.transform.eulerAngles = new Vector3( 0f, 0f, -2.306f);
//				leftRed.transform.localPosition = new Vector3(.79f, -.03f, -.5f);
//				leftRed.transform.localScale = new Vector3(6.5f, .5f, 0f);	
			}
			else{
				foreach (ColorPaletteSwap tmp in player.GetComponentsInChildren<ColorPaletteSwap>()){
					if (tmp.gameObject.name == "Body") {
						tmp.LoadColors (kenColor [player2ColorNumber]);
					}else if (tmp.gameObject.name == "DogSprite"){
						tmp.LoadColors (kenDogColor [player2ColorNumber]);
					}
				}

				rightHpBarSprite = GameObject.Find("RightHpBar").GetComponent<RightHpBarChange>();
				rightHpBarSprite.SetRightBoarderArt("yukiko");
//
//				rightGreen = GameObject.Find("RightHpBar");
//				rightGreen.transform.eulerAngles = new Vector3( 0f, 0f, 1.888f);
//				rightGreen.transform.localPosition = new Vector3(-.53f, -.07f, -.7f);
//				rightGreen.transform.localScale = new Vector3(6.5f, .5f, -.4f);
//
//				rightRed = GameObject.Find("RightHpBarRed");
//				rightRed.transform.eulerAngles = new Vector3( 0f, 0f, 1.888f);
//				rightRed.transform.localPosition = new Vector3(-.53f, -.07f, 3f);
//				rightRed.transform.localScale = new Vector3(6.5f, .5f, 0f);
				
			}
			break;
		case "baiken":
			player = Instantiate(BaikenPrefab, spawnPosition, Quaternion.identity) as GameObject;
			if (isPlayerOne){
				player.GetComponentInChildren<ColorPaletteSwap>().LoadColors(baikenColor[player1ColorNumber]);
				leftHpBarSprite = GameObject.Find("LeftHpBar").GetComponent<LeftHpBarChange>();
				leftHpBarSprite.SetLeftBoarderArt("baiken");
			}
			else{
				rightHpBarSprite = GameObject.Find("RightHpBar").GetComponent<RightHpBarChange>();
				rightHpBarSprite.SetRightBoarderArt("baiken");
				player.GetComponentInChildren<ColorPaletteSwap>().LoadColors(baikenColor[player2ColorNumber]);
			}
			break;
		case "shana":
			player = Instantiate(ShanaPrefab, spawnPosition, Quaternion.identity) as GameObject;
			if (isPlayerOne){
				player.GetComponentInChildren<ColorPaletteSwap>().LoadColors(ShanaColor[player1ColorNumber]);
				leftHpBarSprite = GameObject.Find("LeftHpBar").GetComponent<LeftHpBarChange>();
				leftHpBarSprite.SetLeftBoarderArt("shana");
			}
			else{
				rightHpBarSprite = GameObject.Find("RightHpBar").GetComponent<RightHpBarChange>();
				rightHpBarSprite.SetRightBoarderArt("shana");
				player.GetComponentInChildren<ColorPaletteSwap>().LoadColors(ShanaColor[player2ColorNumber]);
			}
			break;
		default :
			player = new GameObject();
			Debug.Log("error in characterspawn. misspelling.");
			break;

		}
		return player;
	}

	public void PlayersSpawn(){
		p1 = CharacterSpawn(true);
		p2 = CharacterSpawn(false);
		p1.GetComponent<PlayerNumberSetScript>().SetPlayer("playerOne", p2);
		p2.GetComponent<PlayerNumberSetScript>().SetPlayer("playerTwo", p1);

		//greying out player 2
		//p2.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().color = new Color(.7f, .7f , .7f);
		GameObject.Find("Camera").GetComponent<CameraMoveScript>().SetPlayers(p1, p2);
	}

	IEnumerator SetRoundText(string roundText){
		yield return new WaitForSeconds (.5f);
		if (roundText == "Round 1"){
			round1Anim.SetActive(true);
		}else if (roundText == "Round 2"){
			round2Anim.SetActive(true);
		}else if (roundText == "Round 3"){
			round3Anim.SetActive(true);
		}
//		GetComponentInChildren<TextMesh> ().text = roundText;
//		textShadow.text = roundText;
		yield return new WaitForSeconds(.6f);
		readyAnim.SetActive (true);
		yield return new WaitForSeconds(.8f);
		fightAnim.SetActive (true);
		yield return new WaitForSeconds(1f);
		//turn on inputs
		if (FalseMeansTest) {
			p1.GetComponentInChildren<InputScript> ().inputEnabled = true;
			p2.GetComponentInChildren<InputScript> ().inputEnabled = true;
		}
//		GetComponentInChildren<TextMesh> ().text = "";
//		textShadow.text = "";

	}

}

