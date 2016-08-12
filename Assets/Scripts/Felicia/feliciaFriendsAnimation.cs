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

		// intro animation
		int currentFrame = 0;
		while (transform.position != endPos) {

			if (currentFrame >= startLoop.Length) {
				currentFrame = 0;
			}
			spriteRenderer.sprite = startLoop [currentFrame];
			currentFrame++;
			// number of frames to wait
			for (int x = 0; x < 3;) {
				yield return null;
				if (!timeManager.CheckIfTimePaused()) {
					transform.position = Vector2.MoveTowards (transform.position, endPos, .2f);
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
		while (transform.position != endPos) {
			if (currentFrame >= leaveLoop.Length) {
				currentFrame = 0;
			}
			spriteRenderer.sprite = leaveLoop [currentFrame];
			currentFrame++;
			// number of frames to wait
			for (int x = 0; x < 3;) {
				yield return null;
				if (!timeManager.CheckIfTimePaused()) {
					transform.position = Vector2.MoveTowards ( endPos, transform.position, .2f);
					x++;
				}
			}
		}
	}


}
