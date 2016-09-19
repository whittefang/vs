using UnityEngine;
using System.Collections;

public class CameraWallScript : MonoBehaviour {
	FollowScript followScript;
	Transform transformToFollow;
	// Use this for initialization
	void Awake () {
		followScript = GetComponent<FollowScript> ();
		transformToFollow = followScript.transformToFollow.transform;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public void StopFollow(){
		followScript.transformToFollow = null;
	}
	public void EngangeFollow (){
		followScript.transformToFollow = transformToFollow.gameObject;
	}
}
