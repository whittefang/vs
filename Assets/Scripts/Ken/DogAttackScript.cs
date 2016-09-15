using UnityEngine;
using System.Collections;

public class DogAttackScript : MonoBehaviour {
	public GameObject sp1Hitbox, sp2Hitbox, sp3Hitbox, proximityBox;
	TimeManagerScript timeManager;
	SoundsPlayer sounds;
	Rigidbody2D RB;
	DogAnimScript spriteAnimator;
	FighterStateMachineScript state;
	// Use this for initialization
	void Start () {
		state = GetComponent<FighterStateMachineScript> ();
		spriteAnimator = GetComponent<DogAnimScript> ();
		RB = GetComponent<Rigidbody2D> ();
		sounds = GetComponent<SoundsPlayer> ();
		timeManager = GameObject.Find ("MasterGameObject").GetComponent<TimeManagerScript> ();
	}
	
	public void StartSp1(){
		if (state.GetState() == "neutral") {
			StartCoroutine (Sp1 ());
		}
	}
	IEnumerator Sp1(){
		proximityBox.SetActive (true);
		spriteAnimator.StartSp1Anim ();
		StopMovement ();
		state.SetState ("attack");
		for (int x = 0; x < 15;) {
			if (!timeManager.CheckIfTimePaused()) {				
				if (x == 4) {
					sp1Hitbox.SetActive (true);
				}
				if (x == 6) {
					sp1Hitbox.SetActive (false);
					proximityBox.SetActive (false);
				}

				x++;
			}
			yield return null;
		}
		state.SetState ("neutral");
	}
	public void StartSp2(){
		if (state.GetState() == "neutral") {
			StartCoroutine (Sp2 ());
		}
	}
	IEnumerator Sp2(){
		proximityBox.SetActive (true);
		spriteAnimator.StartSp2Anim ();
		StopMovement ();
		state.SetState ("attack");
		for (int x = 0; x < 15;) {
			if (!timeManager.CheckIfTimePaused()) {				
				if (x == 4) {
					sp2Hitbox.SetActive (true);
				}
				if (x == 6) {
					sp2Hitbox.SetActive (false);
					proximityBox.SetActive (false);
				}
				x++;
			}
			yield return null;
		}
		state.SetState ("neutral");
	}
	public void StartSp3(){
		if (state.GetState() == "neutral") {
			StartCoroutine (Sp3 ());
		}
	}
	IEnumerator Sp3(){
		proximityBox.SetActive (true);
		spriteAnimator.StartSp3Anim ();
		StopMovement ();
		state.SetState ("attack");
		for (int x = 0; x < 15;) {
			if (!timeManager.CheckIfTimePaused()) {				
				if (x == 4) {
					sp3Hitbox.SetActive (true);
				}
				if (x == 6) {
					sp3Hitbox.SetActive (false);
					proximityBox.SetActive (false);
				}
				x++;
			}
			yield return null;
		}
		state.SetState ("neutral");
	}
	void StopMovement(){
		RB.velocity = Vector2.zero;
	}
}
