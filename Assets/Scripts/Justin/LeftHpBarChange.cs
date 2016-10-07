using UnityEngine;
using System.Collections;

public class LeftHpBarChange : MonoBehaviour {

	public GameObject LeftHpBar;

	//variables for changing hp
	public float maxHp = 200;
	public float newestHp = 200;
	public float currentLength = 8;
	public float maxLength = 8;
	public Renderer rendererLeft;
	public GameObject DND;
	public SpriteRenderer LeftBoarder;
	public GameObject[] leftBoarders;
	void Start(){
		rendererLeft = GetComponent<SpriteRenderer> ();
	}

	void Update(){
		if (Input.GetKeyDown ("h")) {
			changeBarLeft (20);
			Debug.Log(currentLength);
			//changeBar (damage, LeftHpBar);
			//changeBar2(damage, maxHpBar, LeftHpBar);
		}

	}
	public void SetLeftBoarderArt(string name){
		switch (name){
		case "ryu":
				leftBoarders [0].SetActive (true);
				break;
		case "felicia" :
				leftBoarders [1].SetActive (true);
				break;
		case "subzero" :
				leftBoarders [2].SetActive (true);
				break;
		case "hulk" :
				leftBoarders [3].SetActive (true);
				break;
		case "yukiko" :
				leftBoarders [4].SetActive (true);
			break;
		case "baiken" :
			leftBoarders [5].SetActive (true);
			break;
		}


	}

	public void setHpLeft (int hp){
		maxHp = hp;
		newestHp = hp;
	}
	public void changeBarLeft(int damageInt){
		float damage = damageInt;

		float originalLeftSpot = rendererLeft.bounds.max.x;
		//float correctLeftSpot = LeftHpBar.

		//(current hp / max hp) - ((newesthp - damage) / max hp) = how much % i need to decrease the bar
		float floatChange = 0;
		Debug.Log ("floatChange = " + floatChange + "newestHp = " + newestHp + "damage = " + damage + "maxHp = " + maxHp);


		floatChange = (newestHp/maxHp) - ((newestHp - damage) / maxHp);
		newestHp = newestHp - damage; 

		

		if (newestHp <= 0){
			newestHp = 0;
			LeftHpBar.transform.localScale = new Vector3 (0f, 0f, 0f);
			DND = GameObject.Find("DoNotDestroy");
			DND.GetComponent<Rounds>().PlayerTwoWin();
			

		}else{
			Debug.Log ("floatChange = " + floatChange + "newestHp = " + newestHp + "damage = " + damage + "maxHp = " + maxHp);
			//the bar at max x is 6, so we are decreaseing that number by a percentage and keeping track of how small it gets
			floatChange = maxLength * floatChange;
			currentLength = currentLength - floatChange;
			Debug.Log ("after change to length =" + currentLength);
			LeftHpBar.transform.localScale = new Vector3 (currentLength, LeftHpBar.transform.localScale.y, LeftHpBar.transform.localScale.z);

			//we need to remember where the bar is going so we can correct it to keep it on one side
			float newLeftSpot = rendererLeft.bounds.max.x;
			float findNewLeftSpot = newLeftSpot - originalLeftSpot;
			transform.Translate(new Vector3 (-findNewLeftSpot, 0f, 0f));

		}
	}
}
