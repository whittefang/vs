using UnityEngine;
using System.Collections;

public class TimeManagerScript : MonoBehaviour {

	public bool timePaused = false;
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
		// frame step
		if (Input.GetKeyDown(KeyCode.RightArrow)){
			StartCoroutine (StepFrame ());
		}
	}
	public void StopTime(int numberOfFrames){
		StartCoroutine (StopTimeEnum (numberOfFrames));
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
