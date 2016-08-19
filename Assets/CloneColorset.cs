using UnityEngine;
using System.Collections;

public class CloneColorset : MonoBehaviour {

	// Use this for initialization
	void Awake () {
		GetComponent<SpriteRenderer>().material.SetFloat ("_EffectAmount", 1);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
