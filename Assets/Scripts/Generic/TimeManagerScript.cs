using UnityEngine;
using System.Collections;

public class TimeManagerScript : MonoBehaviour {

	public bool timePaused = false;
	public bool canPauseTime = true;
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
		for (int x = 0; x < numberOfFrames; x++) {
			yield return null;
		}
		timePaused = false;
		
	}
	public bool CheckIfTimePaused(){
		return timePaused;
	}
}
