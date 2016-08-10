﻿using UnityEngine;
using System.Collections;

public class ProjectileScript : MonoBehaviour {
	public Vector2 direction;
	public float speed = .1f;
	bool movementEnabled = true;
	public float lifeDuration = 0;
	public bool useLimitedLife = false;
	public  GameObject bodyToTurnOff;
	TimeManagerScript timeManager;

	void OnEnable(){
		if (timeManager == null) {
			timeManager = GameObject.Find ("MasterGameObject").GetComponent<TimeManagerScript> ();
		}
		if (useLimitedLife) {
			if (bodyToTurnOff == null){
				bodyToTurnOff = this.gameObject;
			}
			Invoke("TurnOffSelf", lifeDuration);
		}

		movementEnabled = true;
		GetComponent<BoxCollider2D> ().enabled = true;
	}
	void OnDisable(){
		CancelInvoke ();
	}
	void TurnOffSelf(){
		bodyToTurnOff.SetActive (false);
	}
	public bool MovementEnabled{
		get{
			return movementEnabled;
		}
		set{
			movementEnabled = value;
		}
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (MovementEnabled) {
			Move ();
		}
	}
	void Move(){
		if (!timeManager.CheckIfTimePaused ()) {
			transform.position += (Vector3)direction * speed;
		}
			
	}
	public void SetDirection(Vector2 newDirection){
		direction = newDirection;
	}
	public void SetSpeed(float newSpeed){
		speed = newSpeed;
	}
	void OnTriggerEnter2D(Collider2D other){
		if (other.tag == "fireballKiller"){
			gameObject.SetActive (false);
		}else if (other.tag == "projectile"){
			other.GetComponent<ProjectileScript> ().Kill ();
			Kill ();
		}
	}
	public void Kill(){
		movementEnabled = false;
		GetComponentInChildren<AnimationLoopScript> ().StopAnimation ();
		GetComponentInChildren<AnimateOnce> ().Animate ();
		GetComponent<BoxCollider2D> ().enabled = false;
	}
}
