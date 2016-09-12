using UnityEngine;
using System.Collections;

public class RockParent : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	void OnEnable(){
		foreach (Transform x in transform.GetComponentsInChildren<Transform>(true)) {
			x.transform.gameObject.SetActive (true);
		}
		Invoke("DisableSelf", 1.5f);
	}
	void DisableSelf(){
		gameObject.SetActive (false);
	}
	// Update is called once per frame
	void Update () {
	
	}
}
