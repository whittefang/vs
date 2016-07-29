using UnityEngine;
using System.Collections;

public class HitboxScript : MonoBehaviour {
	public int damage;
	public int pnum;
	public bool disableOnHit = false, isEnabled = true;
	public string[] tagsToDamage;
	public delegate void voidDel();
	voidDel optionalFunc;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public void EnableScript(bool newState){
		isEnabled = newState;
	}
	public void SetOptFunc(voidDel newOptFunc){
		optionalFunc = newOptFunc;
	}

	void OnTriggerEnter2D(Collider2D other){
		bool match = false;
		// check if the tag is in the damage list
		foreach (string x in tagsToDamage) {
			if (x == other.tag) {
				match = true;
			}
		}

		// do work if tags match
		if (match && isEnabled) {

			// run optional function
			if (optionalFunc != null){
				optionalFunc ();
			}

			// deal the damage
			other.GetComponent<HealthScript> ().DealDamage (damage);

			// turn off object
			if (disableOnHit) {
				gameObject.SetActive (false);
			}

		}
	}

}
