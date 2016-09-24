using UnityEngine;
using System.Collections;

public class rainScript : MonoBehaviour {


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		transform.Translate (new Vector3 (0, -.4f, 0));
		if (transform.position.y < -5) {
			transform.Translate (new Vector3(0, 17, 0));
		}
	}
}
