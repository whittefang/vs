using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HitboxScript : MonoBehaviour {
	public int damage;
	public int pnum;
	public int hitstun;
	public int blockstun;
	public Vector2 hitPush, blockPush;
	public bool disableOnHit = false, isEnabled = true, isProjectile = false, isThrow, multiHit = false;
	public int multihitAmount = 0;
	public List<string> tagsToDamage;
	public delegate void voidDel();
	public delegate void voidArgDel(Transform pos);
	voidDel optionalFunc;
	voidArgDel throwFunc;
	TimeManagerScript timeManager;
	bool multihitFlag;
	BoxCollider2D hitbox;

	// Use this for initialization
	void Start () {


	}
	void OnEnable(){
		if (isProjectile) {
			GetComponent<BoxCollider2D> ().enabled = true;
		}
		if (timeManager == null){

			timeManager = GameObject.Find ("MasterGameObject").GetComponent<TimeManagerScript> ();
		}
		hitbox = GetComponent<BoxCollider2D> ();
		multihitFlag = true;
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
	public void SetThrowFunc(voidArgDel newThrowFunc){
		throwFunc = newThrowFunc;
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
			if (isThrow){
				throwFunc (other.transform.parent.transform);
			}
			// deal the damage

			other.GetComponent<HealthScript> ().DealDamage (damage, hitstun, blockstun, other.transform.position, hitPush, blockPush,isProjectile, isThrow);
			
			if (multiHit && multihitFlag) {
				StartCoroutine (multiHitEnum());
			} else {
				// turn off object
				if (disableOnHit) {
					//Debug.Log ("turnoff");
					gameObject.SetActive (false);
				}
				if (isProjectile) {
					GetComponent<BoxCollider2D> ().enabled = false;
					GetComponentInParent<ProjectileScript> ().Kill ();
				}
			}

		}
	}
	public void AddTagToDamage(string newTag){
		tagsToDamage.Add (newTag);
	}
	IEnumerator multiHitEnum(){
		multihitFlag = false;
		for (int x = 0; x < multihitAmount; x++) {
			hitbox.enabled = false;
			hitbox.enabled = true;
			for (int i = 0; i < 1;) {
				yield return null;
				if (!timeManager.CheckIfTimePaused ()) {
					i++;
				}
			}
		}
		if (isProjectile) {
			hitbox.enabled = false;
			GetComponentInParent<ProjectileScript> ().Kill ();
		}
	}

}
