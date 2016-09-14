using UnityEngine;
using System.Collections;

public class TimeManagerScript : MonoBehaviour {

	public bool timePaused = false;
	public bool canPauseTime = true;
	public Rigidbody2D p1Body, p2Body;
	// Use this for initialization
	void Awake () {
		QualitySettings.vSyncCount = 0;
		Application.targetFrameRate = 60;

	}
	
	// Update is called once per frame
	void Update () {
		// game pause
		if (Input.GetKeyDown(KeyCode.Space)){
			timePaused = !timePaused;
		}

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
		Vector2 p1Velocity, p2Velocity;
		p1Velocity = p1Body.velocity;
		p2Velocity = p2Body.velocity;
		p1Body.velocity = Vector2.zero;
		p2Body.velocity = Vector2.zero;
		p1Body.isKinematic = true;
		p2Body.isKinematic = true;
		for (int x = 0; x < numberOfFrames; x++) {
			yield return null;
		}

		p1Body.velocity = p1Velocity;
		p2Body.velocity = p2Velocity;
		p1Body.isKinematic = false;
		p2Body.isKinematic = false;
		timePaused = false;
		
	}
	public bool CheckIfTimePaused(){
		return timePaused;
	}
}
