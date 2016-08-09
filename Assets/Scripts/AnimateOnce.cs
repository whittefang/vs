using UnityEngine;
using System.Collections;

public class AnimateOnce : MonoBehaviour {
	SpriteRenderer SR;
	public Sprite[] frames;
	TimeManagerScript timeManager;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void OnEnable(){
		if (SR == null) {
			SR = GetComponent<SpriteRenderer> ();
		}
		if (timeManager == null) {
			timeManager = GameObject.Find ("MasterGameObject").GetComponent<TimeManagerScript> ();
		}
		StartCoroutine (animate ());
	}
	IEnumerator animate(){
		for (int x = 0; x < frames.Length; x++) {
			SR.sprite = frames [x];
			for (int i =0; i < 3;){
				if (!timeManager.CheckIfTimePaused()) {
					i++;
				}
				yield return null;
			}
		}
		gameObject.SetActive (false);
	}
}
