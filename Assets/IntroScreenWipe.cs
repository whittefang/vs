using UnityEngine;
using System.Collections;

public class IntroScreenWipe : MonoBehaviour {

	// Use this for initialization
	void Start () {
		StartCoroutine (Animate ());
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	IEnumerator Animate(){
		transform.position = new Vector3 (0, 0, -9);
		for (int i = 0; i < 30; i++) {
			transform.Translate (new Vector2(0, .5f));
			yield return null;
		}
		gameObject.SetActive (false);
	}
}
