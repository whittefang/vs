using UnityEngine;
using System.Collections;

public class SubzeroSuperScript : MonoBehaviour {
	ObjectPoolScript pool;
	// Use this for initialization
	void Start () {
		pool = GetComponent<ObjectPoolScript> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void OnEnable(){
		StartCoroutine (Animate ());
	}
	IEnumerator Animate(){
		for (int i = 0; i < 100; i++) {
			yield return null;

			GameObject tmp = pool.FetchObject ();
			tmp.transform.position = new Vector3 (Random.Range (-16f, 10f), 5, -1);
			tmp.transform.eulerAngles = new Vector3 (0, 0, -45);
			tmp.SetActive (true);

			tmp = pool.FetchObject ();
			tmp.transform.position = new Vector3 (Random.Range (-16f, 10f), 5, -1);
			tmp.transform.eulerAngles = new Vector3 (0, 0, -45);
			tmp.SetActive (true);
		}
		yield return new WaitForSeconds (.7f);
		gameObject.SetActive (false);
	}

}
 