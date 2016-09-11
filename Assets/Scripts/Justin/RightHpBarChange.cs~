using UnityEngine;
using System.Collections;

public class RightHpBarChange : MonoBehaviour {

	public GameObject RightHpBar;

	//public float damageR = 10;
	public float maxHpR = 200;
	public float newestHpR = 200;
	public float currentLengthR = 8;
	public float maxLengthR = 8;
	public Renderer rendererRight;
	public GameObject DND;
	public SpriteRenderer RightBoarder;
	public Sprite[] rightBoarders;

	void Start () {
		rendererRight  = GetComponent<SpriteRenderer> ();
	}

	void Update () {
		if (Input.GetKeyDown ("j")) {
			changeBarRight (20);
			//changeBar (damage, LeftHpBar);
			//changeBar2(damage, maxHpBar, LeftHpBar);
		}
	}

	public void SetRightBoarderArt(string name){
		switch (name){
			case "ryu" :
				RightBoarder.sprite = rightBoarders[0];
				RightBoarder.transform.localPosition = new Vector3(0f, .04f, 0f);
				break;
			case "felicia" :
				RightBoarder.sprite = rightBoarders[1];
				//RightBoarder.transform.localPosition = new Vector3(-.26f, -.04f, 0f);
				RightBoarder.transform.localPosition = new Vector3(1.25f, -.03f, 0f);

				break;
			case "subzero" :
				RightBoarder.sprite = rightBoarders[2];
				//RightBoarder.transform.localPosition = new Vector3(.33f, .46f, 0f);
				RightBoarder.transform.localPosition = new Vector3(.63f, .46f, 0f);
				break;
			case "hulk" :
				RightBoarder.sprite = rightBoarders[3];
				RightBoarder.transform.localPosition = new Vector3(.82f, -.38f, 0f);
					
				break;
			case "yukiko" :
				RightBoarder.sprite = rightBoarders[4];
				RightBoarder.transform.localPosition = new Vector3(.4f, -.41f, 0f);
				currentLengthR = 6.5f;
				maxLengthR = 6.5f;
				break;
		}


	}



	public void setHpRight (int hp){
		maxHpR = hp;
		newestHpR = hp;
	}
	public void changeBarRight (int damageInt){
		float damageR = damageInt;
		float originalLeftSpot = rendererRight.bounds.min.x;
		//float correctLeftSpot = LeftHpBar.

		//(current hp / max hp) - ((newesthp - damage) / max hp) = how much % i need to decrease the bar
		float floatChange = 0;
		//Debug.Log ("floatChange = " + floatChange + "newestHp = " + newestHpR + "damage = " + damageR + "maxHp = " + maxHpR);

		// newestHpR -= damageR;
		// newsetHpR < 0 
		// (maxHpR - damageR)/ maxHpR
		// newScale = (newestHpR/maxHpR) * 6
		// newscal < 0

		floatChange = (newestHpR/maxHpR) - ((newestHpR - damageR) / maxHpR);
		newestHpR = newestHpR - damageR; 


		if (newestHpR <= 0){
			newestHpR = 0;
			RightHpBar.transform.localScale = new Vector3 (0f, 0f, 0f);

			DND = GameObject.Find("DoNotDestroy");
			DND.GetComponent<Rounds>().PlayerOneWin();
		}else{
			//Debug.Log ("floatChange = " + floatChange + "newestHp = " + newestHpR + "damage = " + damageR + "maxHp = " + maxHpR);
			//the bar at max x is 6, so we are decreaseing that number by a percentage and keeping track of how small it gets
			floatChange = maxLengthR * floatChange;
			currentLengthR = currentLengthR - floatChange;
			//Debug.Log ("after change to length =" + currentLengthR);
			RightHpBar.transform.localScale = new Vector3 (currentLengthR, RightHpBar.transform.localScale.y, RightHpBar.transform.localScale.z);
			//we need to remember where the bar is going so we can correct it to keep it on one side
			float newLeftSpot = rendererRight.bounds.min.x;
			float findNewLeftSpot = newLeftSpot - originalLeftSpot;
			transform.Translate(new Vector3 (-findNewLeftSpot, 0f, 0f));
		}

	}
}
