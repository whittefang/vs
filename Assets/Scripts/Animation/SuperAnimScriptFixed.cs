using UnityEngine;
using System.Collections;

public class SuperAnimScriptFixed : MonoBehaviour {

	public GameObject portrait, background;
	TimeManagerScript timeManager;
	FollowScript follow;
	// Use this for initialization
	void Start () {
		follow = GetComponent<FollowScript> ();
		follow.transformToFollow = GameObject.Find ("Camera");
		timeManager = GameObject.Find ("MasterGameObject").GetComponent<TimeManagerScript> ();
	}
	void OnEnable(){
		StartCoroutine (SuperAnimate ());
	}
	// Update is called once per frame
	void Update () {

	}
	IEnumerator SuperAnimate(){
		portrait.SetActive (true);
		for (int x = 0; x < 180;x++) {
			yield return null;

			if (x == 30){
				portrait.SetActive (false);
				background.SetActive (true);
			}
		}
		background.SetActive (false);
		gameObject.SetActive (false);

	}
}
