using UnityEngine;
using System.Collections;

public class FireTrapScript : MonoBehaviour {
	public GameObject hitbox, preEffect, activeEffect;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void OnEnable(){
		StopAllCoroutines ();
		StartCoroutine (StartTrap ());
	}
	IEnumerator StartTrap(){
		preEffect.SetActive (true);
		yield return new WaitForSeconds(1);
		activeEffect.SetActive (true);
		for (int i = 0; i <30; i++) {
			if (i % 5 == 0) {
				hitbox.SetActive (false);
				hitbox.SetActive (true);
			}
			yield return null;
		}
		hitbox.SetActive (false);
		yield return new WaitForSeconds (.8f);
		preEffect.SetActive (false);
		activeEffect.SetActive (false);
		gameObject.SetActive (false);
	}
}
