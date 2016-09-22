using UnityEngine;
using System.Collections;

public class RockParent : MonoBehaviour {

	// Use this for initialization
	void Awake () {
		foreach (HitboxScript hit in GetComponentsInChildren<HitboxScript>(true)) {
			hit.SetOptFunc (StopProjectile);
		}
	}
	void OnEnable(){
		StartCoroutine (PlayRocks ());
	}
	IEnumerator PlayRocks(){

		//yield return new WaitForSeconds (.5f);
		foreach (BoxCollider2D x in GetComponentsInChildren<BoxCollider2D>(true)) {
			x.enabled = true;
			x.transform.gameObject.SetActive (true);
			yield return new WaitForSeconds (.25f);
		}

		yield return new WaitForSeconds (1f);
		gameObject.SetActive (false);
	}
	void StopProjectile(){
		StopAllCoroutines ();
		foreach (BoxCollider2D hit in GetComponentsInChildren<BoxCollider2D>()) {
			hit.enabled = false;
		}
		Invoke ("DisableSelf", 1f);
	}
	void DisableSelf(){
		gameObject.SetActive (false);
	}
	// Update is called once per frame
	void Update () {
	
	}
}
