using UnityEngine;
using System.Collections;

public class RyuAnimations : MonoBehaviour {
	public Sprite[] walkFrames;
	public Sprite[] walkAwayFrames;
	public Sprite[] neutralFrames;
	public Sprite[] neutralJumpFrames;
	public Sprite[] towardJumpFrames;
	public Sprite[] awayJumpFrames;
	public Sprite[] lightFrames;
	public Sprite[] mediumFrames;
	public Sprite[] heavyFrames;
	public SpriteAnimator spriteAnimator;

	SpriteRenderer spriteRenderer;
	// Use this for initialization
	void Start () {
		spriteRenderer = GetComponent<SpriteRenderer> ();
		spriteAnimator = GetComponent<SpriteAnimator> ();
		spriteAnimator.SetNeutralAnimation (StartNeutralAnim);
		spriteAnimator.SetWalkAnim (StartWalkAnim);
		spriteAnimator.SetStopAnimations (EndAnimations);
		spriteAnimator.SetWalkAwayAnim (StartWalkAwayAnim);
		spriteAnimator.SetJumpTowardsAnim (StartTowardJumpAnim);
		spriteAnimator.SetJumpAwayAnim (StartAwayJumpAnim);
		spriteAnimator.SetJumpNeutralAnim (StartNeutralJumpAnim);
		spriteAnimator.SetLightAnimation (StartLightAnim);
		spriteAnimator.SetMediumAnimation (StartMediumAnim);
		spriteAnimator.SetHeavyAnimation (StartHeavyAnim);
	}
	


	IEnumerator loopAnimation(Sprite[] animationFrames){
		int currentFrame = 0;
		while (true) {
			if (currentFrame >= animationFrames.Length) {
				currentFrame = 0;
			}
			spriteRenderer.sprite = animationFrames [currentFrame];
			currentFrame++;
			// number of frames to wait
			for (int x = 0; x < 3; x++) {
				yield return null;
			}
		}
	}
	IEnumerator JumpTowards(){
		spriteRenderer.sprite = towardJumpFrames [0];
		for (int x = 0; x < 3; x++) {
			yield return null;
		}
		spriteRenderer.sprite = towardJumpFrames [1];
		for (int x = 0; x < 10; x++) {
			yield return null;
		}
		for (int i = 0; i < 12; i++) {
			spriteRenderer.sprite = towardJumpFrames [i+2];
			for (int x = 0; x < 3; x++) {
				yield return null;
			}
		}
	}
	IEnumerator JumpAway(){
		spriteRenderer.sprite = towardJumpFrames [13];
		for (int x = 0; x < 3; x++) {
			yield return null;
		}
		spriteRenderer.sprite = towardJumpFrames [12];
		for (int x = 10; x < 10; x++) {
			yield return null;
		}
		for (int i = 11; i > 0; i--) {
			spriteRenderer.sprite = towardJumpFrames [i];
			for (int x = 0; x < 3; x++) {
				yield return null;
			}
		}
		spriteRenderer.sprite = towardJumpFrames [13];
	}
	IEnumerator JumpNeutral(){
		
		spriteRenderer.sprite = neutralJumpFrames [0];
		for (int x = 0; x < 3; x++) {
			yield return null;
		}
		spriteRenderer.sprite = neutralJumpFrames [1];
		for (int x = 0; x < 6; x++) {
			yield return null;
		}
		for (int i = 0; i < 9; i++) {
			spriteRenderer.sprite = neutralJumpFrames [i+2];
			for (int x = 0; x < 3; x++) {
				yield return null;
			}
		}
	}
	IEnumerator Light(){
		for (int i = 0; i < 5; i++) {
			spriteRenderer.sprite = lightFrames [i];
			for (int x = 0; x < 3; x++) {
				yield return null;
			}
		}
	}
	IEnumerator Medium(){
		for (int i = 0; i < 5; i++) {
			spriteRenderer.sprite = lightFrames [i];
			for (int x = 0; x < 3; x++) {
				yield return null;
			}
		}
	}
	IEnumerator Heavy(){
		for (int i = 0; i < 5; i++) {
			spriteRenderer.sprite = lightFrames [i];
			for (int x = 0; x < 3; x++) {
				yield return null;
			}
		}
	}
	public void StartLightAnim(){
		EndAnimations ();
		StartCoroutine (Light());
	}
	public void StartMediumAnim(){
		EndAnimations ();
		StartCoroutine (Medium());
	}
	public void StartHeavyAnim(){
		EndAnimations ();
		StartCoroutine (Heavy());
	}
	public void StartTowardJumpAnim(){
		EndAnimations ();
		StartCoroutine (JumpTowards());
	}
	public void StartAwayJumpAnim(){
		EndAnimations ();
		StartCoroutine (JumpAway());
	}
	public void StartNeutralJumpAnim(){
		EndAnimations ();
		StartCoroutine (JumpNeutral());
	}
	public void StartWalkAnim(){
		EndAnimations ();
		StartCoroutine (loopAnimation (walkFrames));
	}
	public void StartWalkAwayAnim(){
		EndAnimations ();
		StartCoroutine (loopAnimation (walkAwayFrames));
	}

	public void StartNeutralAnim(){
		EndAnimations ();
		StartCoroutine (loopAnimation (neutralFrames));
	}
	public void EndAnimations(){
		StopAllCoroutines ();
	}

}
