using UnityEngine;
using System.Collections;

public class feliciaFriendsAnimation : MonoBehaviour {
	public Sprite[] startLoop;
	public Sprite[] hit;
	public Sprite[] recovery;
	public Sprite[] leaveLoop;
	SpriteRenderer spriteRenderer;
	TimeManagerScript timeManager;
	public Vector3 startPos, endPos;
	// Use this for initialization
	void Awake () {
		spriteRenderer = GetComponent<SpriteRenderer> ();
		timeManager = GameObject.Find ("MasterGameObject").GetComponent<TimeManagerScript> ();
		Animate ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Animate(){
		StartCoroutine (loopAnimation ());
	}

	//IEnumerator 
	IEnumerator loopAnimation(){
		transform.localPosition = startPos;
		// intro animation
		int currentFrame = 0;
		while (transform.localPosition != endPos) {

			if (currentFrame >= startLoop.Length) {
				currentFrame = 0;
			}
			spriteRenderer.sprite = startLoop [currentFrame];
			currentFrame++;
			// number of frames to wait
			for (int x = 0; x < 2;) {
				yield return null;
				if (!timeManager.CheckIfTimePaused()) {
					transform.localPosition = Vector2.MoveTowards (transform.localPosition, endPos,.7f);
					x++;
				}
			}
		}

		// hit animation
		for(int i = 0; i < hit.Length; i++){
			spriteRenderer.sprite = hit [i];
			for (int x = 0; x < 3;) {
				yield return null;
				if (!timeManager.CheckIfTimePaused ()) {
					x++;
				}
			}
		}

		// wait to deal damage
		for (int x = 0; x < 60;) {
			yield return null;
			if (!timeManager.CheckIfTimePaused ()) {
				x++;
			}
		}

		// recover anitation
		for(int i = 0; i < recovery.Length; i++){
			spriteRenderer.sprite = recovery [i];
			for (int x = 0; x < 3;) {
				yield return null;
				if (!timeManager.CheckIfTimePaused ()) {
					x++;
				}
			}
		}
		// leave animation
		currentFrame = 0;
		while (transform.localPosition != startPos) {
			if (currentFrame >= leaveLoop.Length) {
				currentFrame = 0;
			}
			spriteRenderer.sprite = leaveLoop [currentFrame];
			currentFrame++;
			// number of frames to wait
			for (int x = 0; x < 3;) {
				yield return null;
				if (!timeManager.CheckIfTimePaused()) {
					transform.localPosition = Vector2.MoveTowards ( transform.localPosition, startPos, .7f);
					x++;
				}
			}
		}
	
		gameObject.SetActive (false);
	}


}
