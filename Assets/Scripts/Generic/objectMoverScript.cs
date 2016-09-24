using UnityEngine;
using System.Collections;

public class objectMoverScript : MonoBehaviour {
	// Use this for initialization
	bool movingUp = false;
	public Vector3 startPosition;
	public Vector3 endPosition;
	public float lerpAmount;
	void Start () {
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (movingUp) {
			transform.localPosition = Vector3.Lerp (transform.localPosition, endPosition, lerpAmount);
			if (Vector3.Distance(transform.localPosition, endPosition) < .3f){
				movingUp = false;
			}
		} else {
			transform.localPosition = Vector3.Lerp (transform.localPosition, startPosition, lerpAmount);
			if (Vector3.Distance(transform.localPosition, startPosition) < .3f){
				movingUp = true;
			}
		}
	}
}
