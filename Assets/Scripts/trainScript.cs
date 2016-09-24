using UnityEngine;
using System.Collections;

public class trainScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		StartCoroutine (MoveTrain ());
	}

	// Update is called once per frame
	void Update () {

	}
	IEnumerator MoveTrain(){
		while (true) {
			transform.position = new Vector3 (65, -1.25f, 99.5f);
			for (int i = 0; i < 280; i++) {
				transform.Translate (-.5f, 0, 0);
				yield return null;
			}
			yield return new WaitForSeconds (15);
		}
	}
}
