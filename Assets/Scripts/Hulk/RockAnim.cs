using UnityEngine;
using System.Collections;

public class RockAnim : MonoBehaviour {
	public float delay =0;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void OnEnable(){
		StartCoroutine (Move ());
	}
	IEnumerator Move(){
		transform.position = new Vector3 (transform.position.x, -5, transform.position.z);
		yield return new WaitForSeconds (delay);
		int framesToMove = 6;
		for (int x = 0; x < framesToMove; x++) {
			transform.position = new Vector3 (transform.position.x, transform.position.y + .7f, transform.position.z);
			yield return null;
		}
		for (int x = 0; x < framesToMove; x++) {
			transform.position = new Vector3 (transform.position.x, transform.position.y - .4f, transform.position.z);
			yield return null;
			yield return null;
		}
	}
}
