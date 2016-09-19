using UnityEngine;
using System.Collections;

public class SuperAnimScriptFixed : MonoBehaviour {

	public GameObject portrait, background;
	FollowScript follow;
	public int DeactivateDelay = 180;
	// Use this for initialization
	void Start () {
		follow = GetComponent<FollowScript> ();
		follow.transformToFollow = GameObject.Find ("Camera");
	}
	void OnEnable(){
		StartCoroutine (SuperAnimate ());
	}
	// Update is called once per frame
	void Update () {

	}
	IEnumerator SuperAnimate(){
		portrait.SetActive (true);
		for (int x = 0; x < DeactivateDelay;x++) {
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
