using UnityEngine;
using System.Collections;

public class FeliciaSuperScript : MonoBehaviour {
	public GameObject[] helpers;
	public GameObject attack;
	public GameObject cloud;
	FollowScript followScript;
	TimeManagerScript timeManager;

	// Use this for initialization
	void Awake () {
		followScript = GetComponent<FollowScript> ();
		timeManager = GameObject.Find ("MasterGameObject").GetComponent<TimeManagerScript> ();
		attack.GetComponent<HitboxScript> ().SetOptFunc (CompleteAttack);
	}
	void OnEnable(){
		StartAttack ();
	}
	public void StartAttack(){
		StartCoroutine (AttackFunc ());
	}
	IEnumerator AttackFunc(){
		// wait func
		for (int x = 0; x < 60;) {
			yield return null;
			if (!timeManager.CheckIfTimePaused()) {
				x++;
			}
		}

		// inital attack
		attack.SetActive(true);
		for (int x = 0; x < 25;) {
			yield return null;
			if (!timeManager.CheckIfTimePaused()) {
				x++;
			}
		}

		followScript.transformToFollow = null;
	}
	public void CompleteAttack(){
		StartCoroutine (CompleteAttackEnum());
	}
	IEnumerator CompleteAttackEnum(){
		foreach (GameObject g in helpers) {
			g.SetActive (true);
		}
		for (int x = 0; x < 30;) {
			yield return null;
			if (!timeManager.CheckIfTimePaused()) {
				x++;
			}
		}
		cloud.SetActive (true);

	}
}
