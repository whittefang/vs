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
	public Sprite[] jumpLightFrames;
	public Sprite[] jumpMediumFrames;
	public Sprite[] jumpHeavyFrames;
	public Sprite[] SpecialOneFrames;
	public Sprite[] SpecialTwoFrames;
	public Sprite[] SpecialThreeFrames;
	public Sprite[] HitFrames;
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
		spriteAnimator.SetJumpLightAnimation (StartJumpLightAnim);
		spriteAnimator.SetJumpMediumAnimation (StartJumpMediumAnim);
		spriteAnimator.SetJumpHeavyAnimation (StartJumpHeavyAnim);
		spriteAnimator.SetSpecialOneAnimation (StartSpecialOneAnim);
		spriteAnimator.SetSpecialTwoAnimation (StartSpecialTwoAnim);
		spriteAnimator.SetSpecialThreeAnimation (StartSpecialThreeAnim);
		spriteAnimator.SetHitAnimation (StartHitAnim);
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
		for (int i = 0; i < 7; i++) {
			spriteRenderer.sprite = mediumFrames [i];
			for (int x = 0; x < 4; x++) {
				yield return null;
			}
		}
	}
	IEnumerator Heavy(){
		for (int i = 0; i < 14; i++) {
			spriteRenderer.sprite = heavyFrames [i];
			for (int x = 0; x < 3; x++) {
				yield return null;
			}
		}
	}
	IEnumerator JumpLight(){
		for (int i = 0; i < 5; i++) {
			spriteRenderer.sprite = jumpLightFrames [i];
			for (int x = 0; x < 3; x++) {
				yield return null;
			}
		}
	}
	IEnumerator JumpMedium(){
		for (int i = 0; i < 9; i++) {
			spriteRenderer.sprite = jumpMediumFrames [i];

			for (int x = 0; x < 3; x++) {
				yield return null;
			}
		}
	}
	IEnumerator JumpHeavy(){
		for (int i = 0; i < 7; i++) {
			spriteRenderer.sprite = jumpHeavyFrames [i];
			for (int x = 0; x < 3; x++) {
				yield return null;
			}
		}
	}
	IEnumerator SpecialOne(){
		for (int i = 0; i < 12; i++) {
			spriteRenderer.sprite = SpecialOneFrames [i];
			for (int x = 0; x < 3; x++) {
				yield return null;
			}
		}
	}
	IEnumerator SpecialTwo(){
		for (int i = 0; i < 13; i++) {
			spriteRenderer.sprite = SpecialTwoFrames [i];
			// hold on rising uppercut
			if (i == 5) {
				for (int x = 0; x < 12; x++) {
					yield return null;
				}
			}
			for (int x = 0; x < 3; x++) {
				yield return null;
			}
		}
	}
	IEnumerator Hit(){
		for (int i = 0; i < 4; i++) {
			spriteRenderer.sprite = HitFrames [i];
			for (int x = 0; x < 3; x++) {
				yield return null;
			}
		}
	}
	IEnumerator SpecialThree(){
		for (int i = 0; i < 4; i++) {
			spriteRenderer.sprite = SpecialThreeFrames [i];
			for (int x = 0; x < 3; x++) {
				yield return null;
			}
		}
		for (int ii = 0; ii < 3; ii++) {
			for (int i = 4; i < 12; i++) {
				spriteRenderer.sprite = SpecialThreeFrames [i];
				for (int x = 0; x < 3; x++) {
					yield return null;
				}
			}
		}
		for (int i = 12; i < 15; i++) {
			spriteRenderer.sprite = SpecialThreeFrames [i];
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
	public void StartJumpLightAnim(){
		EndAnimations ();
		StartCoroutine (JumpLight());
	}
	public void StartJumpMediumAnim(){
		EndAnimations ();
		StartCoroutine (JumpMedium());
	}
	public void StartJumpHeavyAnim(){
		EndAnimations ();
		StartCoroutine (JumpHeavy());
	}
	public void StartSpecialOneAnim(){
		EndAnimations ();
		StartCoroutine (SpecialOne());
	}
	public void StartSpecialTwoAnim(){
		EndAnimations ();
		StartCoroutine (SpecialTwo());
	}
	public void StartSpecialThreeAnim(){
		EndAnimations ();
		StartCoroutine (SpecialThree());
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
	public void StartHitAnim(){
		EndAnimations ();
		StartCoroutine (Hit());
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
