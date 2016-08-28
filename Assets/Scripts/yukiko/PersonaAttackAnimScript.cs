using UnityEngine;
using System.Collections;

public class PersonaAttackAnimScript : MonoBehaviour {
	public Sprite[] mediumFrames;
	public Sprite[] heavyFrames;
	public Sprite[] SpecialOneFrames;
	public Sprite[] SpecialTwoFrames;
	public Sprite[] SpecialThreeFrames;
	SpriteRenderer spriteRenderer;
	TimeManagerScript timeManager;
	SoundsPlayer sound;
	Rigidbody2D RB;
	// Use this for initialization
	void Awake () {
		spriteRenderer = GetComponent<SpriteRenderer> ();
		sound = GetComponent<SoundsPlayer> ();
		RB = GetComponent<Rigidbody2D> ();
		timeManager = GameObject.Find ("MasterGameObject").GetComponent<TimeManagerScript> ();
	}
	IEnumerator Medium(){
		int animationFrame = 0;
		spriteRenderer.sprite = mediumFrames [0];
		Vector2 direction = new Vector2 (15, 0);
		bool notHit = true;
		int counter = 0;
		for (int i = 0; i < 40; i++) {
			if (counter == 3) {
				spriteRenderer.sprite = mediumFrames [1];
			} else if (counter == 6) {
				spriteRenderer.sprite = mediumFrames [0];
				counter = 0;
			}
			counter++;
			RB.velocity = direction;
			yield return null;
		}
		RB.velocity = Vector2.zero;
		for (int i = 0; i < 33;) {

			if (i%3 == 0) {
				spriteRenderer.sprite = mediumFrames [animationFrame];
				animationFrame++;
			}

			yield return null;
			if (!timeManager.CheckIfTimePaused()) {
				i++;
			}
		}
		StartCoroutine (TimedUnsummon (30));
	}
	IEnumerator Heavy(){
		int animationFrame = 0;
		spriteRenderer.sprite = heavyFrames [0];
		for (int i = 0; i < 27;) {

			if (i%3 == 0) {
				spriteRenderer.sprite = heavyFrames [animationFrame];
				animationFrame++;
			}

			yield return null;
			if (!timeManager.CheckIfTimePaused()) {
				i++;
			}

		}
		StartCoroutine (TimedUnsummon (30));
	}
	IEnumerator SpecialOne(){
		int animationFrame = 0;
		spriteRenderer.sprite = SpecialOneFrames [0];
		for (int i = 0; i < 21;) {

			if (i%3 == 0) {
				spriteRenderer.sprite = SpecialOneFrames [animationFrame];
				animationFrame++;
			}

			yield return null;
			if (!timeManager.CheckIfTimePaused()) {
				i++;
			}

		}
		StartCoroutine (TimedUnsummon (30));
	}
	IEnumerator SpecialTwo(){
		int animationFrame = 0;
		spriteRenderer.sprite = SpecialTwoFrames [0];
		for (int i = 0; i < 18;) {

			if (i%3 == 0) {
				spriteRenderer.sprite = SpecialTwoFrames [animationFrame];
				animationFrame++;
			}

			yield return null;
			if (!timeManager.CheckIfTimePaused()) {
				i++;
			}

		}
		StartCoroutine (TimedUnsummon (30));
	}
	IEnumerator SpecialThree(){
		int animationFrame = 0;
		spriteRenderer.sprite = SpecialThreeFrames [0];
		for (int i = 0; i < 42;) {

			if (i%3 == 0) {
				spriteRenderer.sprite = SpecialThreeFrames [animationFrame];
				animationFrame++;
			}

			yield return null;
			if (!timeManager.CheckIfTimePaused()) {
				i++;
			}
		}
		RB.AddForce (new Vector2(-5, 0));
		StartCoroutine (TimedUnsummon (30));
	}
	IEnumerator TimedUnsummon(int delay =0){
		for (int i = 0; i < delay;) {
			
			yield return null;
			if (!timeManager.CheckIfTimePaused()) {
				i++;
			}
		}
		for (int i = 0; i < 10;) {
			yield return null;
			if (!timeManager.CheckIfTimePaused()) {
				i++;
				spriteRenderer.color = new Color(spriteRenderer.color.r,spriteRenderer.color.g,spriteRenderer.color.b, spriteRenderer.color.a - .1f); ;

			}
		}
		gameObject.SetActive (false);
	}
	public void Summon(){
		StartCoroutine (SummonEffect ());

	}
	IEnumerator SummonEffect(){	
		spriteRenderer.color  = new Color(spriteRenderer.color.r,spriteRenderer.color.g,spriteRenderer.color.b,0);
			
		for (int i = 0; i < 10;) {
			yield return null;
			if (!timeManager.CheckIfTimePaused()) {
				i++;
				spriteRenderer.color  = new Color(spriteRenderer.color.r,spriteRenderer.color.g,spriteRenderer.color.b, spriteRenderer.color.a + .1f);
				Debug.Log (spriteRenderer.color.a);
			}
		}

	}
	public void UnSummon(){
		
	}
	public void StartMediumAnim(){
		EndAnimations ();
		StartCoroutine (Medium());
	}
	public void StartHeavyAnim(){
		EndAnimations ();
		StartCoroutine (Heavy());
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
	public void EndAnimations(){
		RB.velocity = Vector2.zero;
		StopAllCoroutines ();
	}
}
