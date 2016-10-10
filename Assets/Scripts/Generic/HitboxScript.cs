using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HitboxScript : MonoBehaviour {
	public int damage;
	public int pnum;
	public int hitstun;
	public int blockstun;
	public int hitstop = 5;
	public Vector2 hitPush, blockPush;
	public bool disableOnHit = false, isEnabled = true, isProjectile = false, isThrow, multiHit = false, 
	useCornerPushback = true, omitOptFuncOnBlock = false, isFreezingAttack = false, launcher = false,
	knockdownAttack = false, useOptionalPosition, juggle = false;
	Vector2 optionalPosition;
	public int multihitAmount = 0;
	public int multihitFrameBetween = 1;
	public List<string> tagsToDamage;
	public AudioClip hitSound;
	public float hitPitch =1;
	public float blockPitch =1;
	public int attackStrength =0;
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
		Debug.Log (other.name);
		bool match = false;
		// check if the tag is in the damage list
		foreach (string x in tagsToDamage) {
			if (x == other.tag) {
				match = true;
			}
		}

		// do work if tags match
		if (match && isEnabled) {
			

			// deal the damage
			if (useOptionalPosition){
				optionalPosition = transform.position;
			}
			bool blocked = other.GetComponentInParent<HealthScript> ().DealDamage (damage, hitstun, blockstun, transform.position, hitPush, blockPush,isProjectile, isThrow, useCornerPushback, isFreezingAttack, launcher, knockdownAttack,optionalPosition, hitstop, hitSound, hitPitch, blockPitch, juggle, attackStrength);

			if (isThrow && !blocked){
				throwFunc (other.transform.parent.transform);
			}
			// run optional function
			if (optionalFunc != null){
				// only run when opt is allowed on block and move is not blocked or hits
				if (omitOptFuncOnBlock && blocked) {
					//ran when 
				} else {
					optionalFunc ();
				}
			}

			if (multiHit && multihitFlag && (!isProjectile || (isProjectile && other.GetComponentInParent<FighterStateMachineScript>().GetState() != "projectile invulnerable")) ) {
				StartCoroutine (multiHitEnum());			
			} else {
				// turn off object
				if (disableOnHit) {
					//Debug.Log ("turnoff");
					gameObject.SetActive (false);
				}
				if (isProjectile &&!multiHit && other.GetComponentInParent<FighterStateMachineScript>().GetState() != "projectile invulnerable" && other.GetComponentInParent<FighterStateMachineScript>().GetState() != "invincible") {
					if (GetComponentInParent<ProjectileScript> () != null) {
						GetComponent<BoxCollider2D> ().enabled = false;
						GetComponentInParent<ProjectileScript> ().Kill ();
					}
				}
			}
			if (!multiHit) {
				gameObject.SetActive (false);
			}
		}

		if (other.tag == "P1Persona" && pnum == 1) {
			other.GetComponent<DogHealthScript> ().DealDamage ();
		} else if (other.tag == "P2Persona" && pnum == 0) {
			other.GetComponent<DogHealthScript> ().DealDamage ();
		}
	}
	public void AddTagToDamage(string newTag){
		tagsToDamage.Add (newTag);
	}
	IEnumerator multiHitEnum(){
		multihitFlag = false;
		if (isProjectile) {
			GetComponentInParent<ProjectileScript> ().movementEnabled = false;
		}
		for (int x = 0; x < multihitAmount; x++) {
			hitbox.enabled = false;
			hitbox.enabled = true;
			for (int i = 0; i < multihitFrameBetween;) {
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
