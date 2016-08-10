﻿using UnityEngine;
using System.Collections;

public class RightHpBarChange : MonoBehaviour {

	public GameObject RightHpBar;

	//public float damageR = 10;
	public float maxHpR = 200;
	public float newestHpR = 200;
	public float currentLengthR = 6;
	public Renderer rendererRight;

	void Start () {
		rendererRight  = GetComponent<SpriteRenderer> ();
	}

	void Update () {
	//	if (Input.GetKeyDown ("j")) {
			//changeBarRight ();
			//changeBar (damage, LeftHpBar);
			//changeBar2(damage, maxHpBar, LeftHpBar);
		//}
	}

	public void setHpRight (int hp){
		maxHpR = hp;
		newestHpR = hp;
	}
	public void changeBarRight (int damageInt){
		float damageR = damageInt;
		float originalLeftSpot = rendererRight.bounds.max.x;
		//float correctLeftSpot = LeftHpBar.

		//(current hp / max hp) - ((newesthp - damage) / max hp) = how much % i need to decrease the bar
		float floatChange = 0;
		Debug.Log ("floatChange = " + floatChange + "newestHp = " + newestHpR + "damage = " + damageR + "maxHp = " + maxHpR);


		floatChange = (newestHpR/maxHpR) - ((newestHpR - damageR) / maxHpR);
		newestHpR = newestHpR - damageR; 

		Debug.Log ("floatChange = " + floatChange + "newestHp = " + newestHpR + "damage = " + damageR + "maxHp = " + maxHpR);
		//the bar at max x is 6, so we are decreaseing that number by a percentage and keeping track of how small it gets
		floatChange = 6 * floatChange;
		currentLengthR = currentLengthR - floatChange;
		Debug.Log ("after change to length =" + currentLengthR);
		RightHpBar.transform.localScale = new Vector3 (currentLengthR, RightHpBar.transform.localScale.y, RightHpBar.transform.localScale.z);

		//we need to remember where the bar is going so we can correct it to keep it on one side
		float newLeftSpot = rendererRight.bounds.max.x;
		float findNewLeftSpot = newLeftSpot - originalLeftSpot;
		transform.Translate(new Vector3 (-findNewLeftSpot, 0f, 0f));

	}
}