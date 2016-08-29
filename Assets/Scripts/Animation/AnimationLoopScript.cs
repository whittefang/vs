using UnityEngine;
using System.Collections;

public class AnimationLoopScript : MonoBehaviour {
	public Sprite[] frames, introFrames;
	public bool useTransition = false, useDelay = false, loopBackwardsAfterFinish = false;
	SpriteRenderer  SR;
	public int timeBetweenFrames = 4, delay = 0;
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
		if (useDelay) {
			SR.sprite = null;
			for (int x = 0; x < delay; x++) {
				yield return null;
			}
		}
		while (true) {
			for (int i = 0; i < frames.Length; i++) {
				SR.sprite = frames [i];

				for (int x = 0; x < timeBetweenFrames; x++) {
					yield return null;
				}
			}
			if (loopBackwardsAfterFinish) {
				for (int i = frames.Length-1; i > 0; i--) {
					SR.sprite = frames [i];

					for (int x = 0; x < timeBetweenFrames; x++) {
						yield return null;
					}
				}
			}

		}
	}

}
