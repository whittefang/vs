using UnityEngine;
using System.Collections;

public class DogAnimScript : MonoBehaviour {
	public Sprite[] walkFrames;
	public Sprite[] sp1Frames;
	public Sprite[] sp2Frames;
	public Sprite[] sp3Frames;
	public Sprite[] hitFrames;
	public Sprite[] getupFrames;
	public Sprite[] neutralFrames;
	public Sprite[] turnFrames;
	public Sprite[] winFrames;
	public Sprite[] superFrames;
	public GameObject sp1Effect;
	public GameObject sp2Effect;
	public GameObject sp3Effect;
	public GameObject hurtbox;



	Vector3 hurtboxBodyOriginalPosition, hurtboxBodyOriginalScale;
	TimeManagerScript timeManager;
	SpriteRenderer spriteRenderer;

	// Use this for initialization
	void Start () {
		spriteRenderer = GetComponentInChildren<SpriteRenderer> ();
		hurtboxBodyOriginalPosition = hurtbox.transform.localPosition;
		hurtboxBodyOriginalScale = hurtbox.transform.localScale;
		timeManager = GameObject.Find ("MasterGameObject").GetComponent<TimeManagerScript> ();
		StartNeutralAnim ();

	}


	IEnumerator Sp1(){
		spriteRenderer.sprite = sp1Frames [0];
		for (int x = 0; x < 10;) {
			yield return null;
			if (!timeManager.CheckIfTimePaused()) {
				x++;
			}
		}
		sp1Effect.SetActive (true);
		while (true) {
			for (int i = 1; i < 3; i++) {
				spriteRenderer.sprite = sp1Frames [i];
				for (int x = 0; x < 3;) {
					yield return null;
					if (!timeManager.CheckIfTimePaused ()) {
						x++;
					}
				}
			}
		}
		sp1Effect.SetActive (false);
	}
	IEnumerator Sp1Complete(){
		sp1Effect.SetActive (false);
		for (int i = 3; i < 13; i++) {
			spriteRenderer.sprite = sp1Frames [i];
			if (i == 5) {
				for (int iii = 0; iii < 1; iii++) {
					for (int ii = 5; ii < 9; ii++) {
						spriteRenderer.sprite = sp1Frames [ii];

						for (int x = 0; x < 3;) {
							yield return null;
							if (!timeManager.CheckIfTimePaused ()) {
								x++;
							}
						}
					}
				}
			}

			for (int x = 0; x < 3;) {
				yield return null;
				if (!timeManager.CheckIfTimePaused()) {
					x++;
				}
			}
		}
		
	}
	IEnumerator Sp2(){
		for (int i = 0; i < 10; i++) {
			spriteRenderer.sprite = sp2Frames [i];
			if (i == 0) {
				for (int x = 0; x < 6;) {
					yield return null;
					if (!timeManager.CheckIfTimePaused()) {
						x++;
					}
				}
			}
			if (i == 4) {
				sp2Effect.SetActive (true);
			}
			if (i == 7) {
				for (int x = 0; x < 21;) {
					yield return null;
					if (!timeManager.CheckIfTimePaused()) {
						x++;
					}
				}
			}
			for (int x = 0; x < 3;) {
				yield return null;
				if (!timeManager.CheckIfTimePaused()) {
					x++;
				}
			}
		}
		spriteRenderer.sprite = sp1Frames [12];
	}	
	IEnumerator Sp3(){
		for (int ii = 0; ii < 4; ii++) {
			for (int i = 0; i < 2; i++) {
				spriteRenderer.sprite = sp3Frames [i];
				for (int x = 0; x < 3;) {
					yield return null;
					if (!timeManager.CheckIfTimePaused()) {
						x++;
					}
				}
			}
		}

		for (int i = 2; i < 4; i++) {
			spriteRenderer.sprite = sp3Frames [i];
			for (int x = 0; x < 5;) {
				yield return null;
				if (!timeManager.CheckIfTimePaused()) {
					x++;
				}
			}
		}

		for (int ii = 0; ii < 3; ii++) {
			for (int i = 4; i < 7; i++) {
				spriteRenderer.sprite = sp3Frames [i];
				for (int x = 0; x < 3;) {
					yield return null;
					if (!timeManager.CheckIfTimePaused ()) {
						x++;
					}
				}
			}
		}
		spriteRenderer.sprite = sp1Frames [12];
	}
	IEnumerator Neutral(){
		while (true) {
			for (int i = 0; i < 6; i++) {
				spriteRenderer.sprite = neutralFrames [i];
				for (int x = 0; x < 5;) {
					yield return null;
					if (!timeManager.CheckIfTimePaused ()) {
						x++;
					}
				}
			}
		}
	}
	IEnumerator Hit(){
		for (int i = 0; i < 19; i++) {
			spriteRenderer.sprite = hitFrames [i];
			for (int x = 0; x < 3;) {
				yield return null;
				if (!timeManager.CheckIfTimePaused()) {
					x++;
				}
			}
		}
	}
	IEnumerator Walk(){
		while (true) {
			for (int i = 0; i < 5; i++) {
				spriteRenderer.sprite = walkFrames [i];
				for (int x = 0; x < 5;) {
					yield return null;
					if (!timeManager.CheckIfTimePaused ()) {
						x++;
					}
				}
			}
		}
	}
	IEnumerator Getup(){
		for (int i = 0; i < 3; i++) {
			spriteRenderer.sprite = getupFrames [i];
			for (int x = 0; x < 3;) {
				yield return null;
				if (!timeManager.CheckIfTimePaused()) {
					x++;
				}
			}
		}
	}
	IEnumerator Turn(){
		for (int i = 0; i < 3; i++) {
			spriteRenderer.sprite = turnFrames [i];
			for (int x = 0; x < 3;) {
				yield return null;
				if (!timeManager.CheckIfTimePaused()) {
					x++;
				}
			}
		}
	}
	IEnumerator Win(){
		for (int i = 0; i < 10; i++) {
			spriteRenderer.sprite = winFrames [i];
			for (int x = 0; x < 3;) {
				yield return null;
				if (!timeManager.CheckIfTimePaused()) {
					x++;
				}
			}
		}
	}
	IEnumerator Super(){
		// intro buildup
		for (int i = 0; i < 2; i++) {
			spriteRenderer.sprite = superFrames [i];
			for (int x = 0; x < 3;) {
				yield return null;
				if (!timeManager.CheckIfTimePaused ()) {
					x++;
				}
			}
		}
		// wait on pulling out kunai
		for (int x = 0; x < 24;) {
			yield return null;
			if (!timeManager.CheckIfTimePaused ()) {
				x++;
			}
		}
		// weapon loop
		sp3Effect.SetActive(true);
		for (int ii = 0; ii < 5; ii++) {			
			for (int i = 2; i < 7; i++) {
				spriteRenderer.sprite = superFrames [i];
				for (int x = 0; x < 2;) {
					yield return null;
					if (!timeManager.CheckIfTimePaused ()) {
						x++;
					}
				}
			}
		}
		sp3Effect.SetActive(false);
		// last hit
		for (int i = 7; i < 13; i++) {
			spriteRenderer.sprite = superFrames [i];
			for (int x = 0; x < 3;) {
				yield return null;
				if (!timeManager.CheckIfTimePaused ()) {
					x++;
				}
			}
		}

	}


	public void StartNeutralAnim(){
		EndAnimations ();
		StartCoroutine (Neutral());
	}
	public void StartSp1Anim(){
		EndAnimations ();
		StartCoroutine (Sp1());
	}
	public void StartSp1HitAnim(){
		EndAnimations ();
		StartCoroutine (Sp1Complete());
	}
	public void StartSp2Anim(){
		EndAnimations ();
		StartCoroutine (Sp2());
	}public void StartSp3Anim(){
		EndAnimations ();
		StartCoroutine (Sp3());
	}public void StartWalkAnim(){
		EndAnimations ();
		StartCoroutine (Walk());
	}public void StartWinAnim(){
		EndAnimations ();
		StartCoroutine (Win());
	}
	public void StartHitAnim(){
		EndAnimations ();
		StartCoroutine (Hit());
	}public void StarTurnAnim(){
		EndAnimations ();
		StartCoroutine (Turn());
	}public void StartGetupAnim(){
		EndAnimations ();
		StartCoroutine (Getup());
	}
	public void StartSuperAnim(){
		EndAnimations ();
		StartCoroutine (Super());
	}






	public void EndAnimations(){
		StopAllCoroutines ();
		sp1Effect.SetActive (false);
		sp2Effect.SetActive (false);
		sp3Effect.SetActive (false);
		hurtbox.transform.localScale  = hurtboxBodyOriginalScale;
		hurtbox.transform.localPosition  = hurtboxBodyOriginalPosition;
	}
}
