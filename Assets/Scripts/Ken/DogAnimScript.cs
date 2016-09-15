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

	public GameObject hurtbox;



	Vector3 hurtboxBodyOriginalPosition, hurtboxBodyOriginalScale;
	SoundsPlayer sound;
	TimeManagerScript timeManager;
	SpriteRenderer spriteRenderer;

	// Use this for initialization
	void Start () {
		spriteRenderer = GetComponentInChildren<SpriteRenderer> ();
		hurtboxBodyOriginalPosition = hurtbox.transform.localPosition;
		hurtboxBodyOriginalScale = hurtbox.transform.localScale;
		sound = GetComponent<SoundsPlayer> ();
		timeManager = GameObject.Find ("MasterGameObject").GetComponent<TimeManagerScript> ();
		StartNeutralAnim ();

	}
	

	IEnumerator Sp1(){
		for (int i = 0; i < 13; i++) {
			spriteRenderer.sprite = sp1Frames [i];
			for (int x = 0; x < 3;) {
				yield return null;
				if (!timeManager.CheckIfTimePaused()) {
					x++;
				}
			}
		}
	}
	IEnumerator Sp2(){
		for (int i = 0; i < 17; i++) {
			spriteRenderer.sprite = sp2Frames [i];
			for (int x = 0; x < 3;) {
				yield return null;
				if (!timeManager.CheckIfTimePaused()) {
					x++;
				}
			}
		}
	}	IEnumerator Sp3(){
		for (int i = 0; i < 7; i++) {
			spriteRenderer.sprite = sp3Frames [i];
			for (int x = 0; x < 3;) {
				yield return null;
				if (!timeManager.CheckIfTimePaused()) {
					x++;
				}
			}
		}
	}
	IEnumerator Neutral(){
		while (true) {
			for (int i = 0; i < 6; i++) {
				spriteRenderer.sprite = neutralFrames [i];
				for (int x = 0; x < 3;) {
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
		for (int i = 0; i < 5; i++) {
			spriteRenderer.sprite = walkFrames [i];
			for (int x = 0; x < 3;) {
				yield return null;
				if (!timeManager.CheckIfTimePaused()) {
					x++;
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



	public void StartNeutralAnim(){
		EndAnimations ();
		StartCoroutine (Neutral());
	}
	public void StartSp1Anim(){
		EndAnimations ();
		StartCoroutine (Sp1());
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






	public void EndAnimations(){
		StopAllCoroutines ();
		hurtbox.transform.localScale  = hurtboxBodyOriginalScale;
		hurtbox.transform.localPosition  = hurtboxBodyOriginalPosition;
	}
}
