using UnityEngine;
using System.Collections;

public class AnimateOnce : MonoBehaviour {
	public SpriteRenderer SR;
	public Sprite[] frames;
	TimeManagerScript timeManager;
	public bool startOnEnable = true;
	public bool animateDurtingPause = false, loopBackToBeginning = false, disableOnFinish = true;
	public bool useDelay = false;
	public float delay = 0;
	public int speed = 3;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void OnEnable(){
		if (timeManager == null) {
			timeManager = GameObject.Find ("MasterGameObject").GetComponent<TimeManagerScript> ();
		}
		if (startOnEnable) {
			StartCoroutine (animate ());
		}

	}
	public void Animate(){
		StartCoroutine (animate ());
	}
	IEnumerator animate(){
		if (useDelay) {
			yield return new WaitForSeconds (delay);	
		}
		for (int x = 0; x < frames.Length; x++) {

			Debug.Log ("anim");
			SR.sprite = frames [x];
			for (int i =0; i < speed;){
				if (!timeManager.CheckIfTimePaused()) {
					i++;
				}else if (animateDurtingPause){
					i++;
				}
				yield return null;
			}
		}
		if (loopBackToBeginning) {
			for (int x = frames.Length-1; x > 0; x--) {
				SR.sprite = frames [x];
				for (int i =0; i < speed;){
					if (!timeManager.CheckIfTimePaused()) {
						i++;
					}else if (animateDurtingPause){
						i++;
					}
					yield return null;
				}
			}
		}
		if (disableOnFinish) {
			gameObject.SetActive (false);
		}
	}
}
