 using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TimeManagerScript : MonoBehaviour {

	public bool timePaused = false;
	public bool canPauseTime = true;

	Vector2 p1Velocity, p2Velocity;
	List<Rigidbody2D> extraBodies;
	List<Vector2> extraBodiesVelocities;
	public Rigidbody2D p1Body, p2Body;
	// Use this for initialization
	void Awake () {
		QualitySettings.vSyncCount = 0;
		Application.targetFrameRate = 60;
		if (extraBodies == null || extraBodiesVelocities == null) {
			extraBodies = new List<Rigidbody2D> ();
			extraBodiesVelocities = new List<Vector2> ();
		}
	}
	
	// Update is called once per frame
	void Update () {
		// game pause
		if (Input.GetKeyDown(KeyCode.Space)){
			timePaused = !timePaused;
		}

	}
	public void AddBody(Rigidbody2D newBody){
		if (extraBodies == null || extraBodiesVelocities == null) {
			extraBodies = new List<Rigidbody2D> ();
			extraBodiesVelocities = new List<Vector2> ();
		}
		extraBodies.Add (newBody);
		extraBodiesVelocities.Add(new Vector2());
	}
	public void StopTime(int numberOfFrames){
		if (canPauseTime) {
			StopAllCoroutines ();
			StartCoroutine (StopTimeEnum (numberOfFrames));
		}
	}
	public void StopTimeForce(int numberOfFrames){
		if (canPauseTime) {
			StopAllCoroutines ();
			StartCoroutine (StopTimeEnum (numberOfFrames));
		}
	}
	IEnumerator StepFrame(){
		timePaused = false;
		yield return null;
		timePaused = true;
			
	}
	IEnumerator StopTimeEnum(int numberOfFrames){
		timePaused = true;
		if (p1Velocity == Vector2.zero){
			p1Velocity = p1Body.velocity;
		}
		if (p2Velocity == Vector2.zero) {
			p2Velocity = p2Body.velocity;
		}
		p1Body.velocity = Vector2.zero;
		p2Body.velocity = Vector2.zero;
		p1Body.isKinematic = true;
		p2Body.isKinematic = true;
		int currentBody = 0;
		foreach (Rigidbody2D r in extraBodies) {
			extraBodiesVelocities[currentBody] =  r.velocity;
			Debug.Log (extraBodiesVelocities [currentBody]);
			r.velocity = Vector2.zero;
			r.isKinematic = true;
			currentBody++;
		}
		for (int x = 0; x < numberOfFrames; x++) {
			yield return null;
		}
		currentBody = 0;
		foreach (Rigidbody2D r in extraBodies) {
			r.isKinematic = false;
			Debug.Log (extraBodiesVelocities [currentBody]);
			r.velocity = extraBodiesVelocities[currentBody];
			currentBody++;
		}
		p1Body.velocity = p1Velocity;
		p2Body.velocity = p2Velocity;
		p1Body.isKinematic = false;
		p2Body.isKinematic = false;
		timePaused = false;
		p1Velocity = Vector2.zero;
		p2Velocity = Vector2.zero;
	}
	public bool CheckIfTimePaused(){
		return timePaused;
	}
}
