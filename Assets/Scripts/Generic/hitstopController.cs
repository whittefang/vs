using UnityEngine;
using System.Collections;

public class hitstopController : MonoBehaviour {



	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Space)){
			StopForTime ();
		}
	}
	public void StopForTime(){
		StopAllCoroutines ();
		StartCoroutine(StopTime());
	}
	IEnumerator StopTime(){
		Time.timeScale = 0;
		Debug.Log ("stop");
		for (int x = 0; x < 50; x++) {
			yield return null;
		}

		Debug.Log ("start");
		Time.timeScale = 1;
	}
}
