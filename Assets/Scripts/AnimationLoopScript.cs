using UnityEngine;
using System.Collections;

public class AnimationLoopScript : MonoBehaviour {
	public Sprite[] frames;
	SpriteRenderer  SR;
	int currentFrame = 0;
	// Use this for initialization
	void OnEnable () {
		if (SR == null) {
			SR = GetComponent<SpriteRenderer> ();
		}
		StartCoroutine (loop ());
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		
	}
	public void StopAnimation(){
		StopAllCoroutines ();
	}
	IEnumerator loop(){
		while (true) {
			SR.sprite = frames [currentFrame];
			currentFrame++;
			if (currentFrame >= frames.Length) {
				currentFrame = 0;
			}
			for (int x = 0; x < 4; x++) {
				yield return null;
			}
		}
	}

}
