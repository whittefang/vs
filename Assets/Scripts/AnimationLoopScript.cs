using UnityEngine;
using System.Collections;

public class AnimationLoopScript : MonoBehaviour {
	public Sprite[] frames, introFrames;
	public bool useTransition = false;
	SpriteRenderer  SR;
	int currentFrame = 0;
	public int timeBetweenFrames = 4;
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
	IEnumerator Intro(){
		for (int x = 0; x < introFrames.Length; x++) {
			SR.sprite = frames [x];
			for (int i = 0; i < timeBetweenFrames;) {
				i++;
			}
				yield return null;
		}
		StartCoroutine(loop());
	}
	IEnumerator loop(){
		while (true) {
			SR.sprite = frames [currentFrame];
			currentFrame++;
			if (currentFrame >= frames.Length) {
				currentFrame = 0;
			}
			for (int x = 0; x < timeBetweenFrames; x++) {
				yield return null;
			}
		}
	}

}
