using UnityEngine;
using System.Collections;

public class ParryScript : MonoBehaviour {
	public BaikenAttackScript attackScript;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void OnTriggerEnter2D(Collider2D other){
		if (other.tag == "hit") {
			Debug.Log ("fuck    " + other.gameObject.name);
			attackScript.SpecialTwoComplete ();
		}
	}
}
