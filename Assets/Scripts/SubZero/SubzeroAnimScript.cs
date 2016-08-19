using UnityEngine;
using System.Collections;

public class SubzeroAnimScript : MonoBehaviour {

	public Sprite[] walkFrames;
	public Sprite[] neutralFrames;
	public Sprite[] neutralJumpFrames;
	public Sprite[] throwFrames;
	public Sprite[] lightFrames;
	public Sprite[] mediumFrames;
	public Sprite[] heavyFrames;
	public Sprite[] jumpLightFrames;
	public Sprite[] jumpMediumFrames;
	public Sprite[] jumpHeavyFrames;
	public Sprite[] SpecialOneFrames;
	public Sprite[] SpecialTwoFrames;
	public Sprite[] SpecialThreeFrames;
	public Sprite[] superFrames;
	public Sprite[] HitFrames;
	public Sprite[] BlockFrames;
	public Sprite[] introFrames;
	public Sprite[] winFrames;
	public Sprite[] deathFrames;
	public SpriteAnimator spriteAnimator;
	public BoxCollider2D hurtbox;
	public GameObject head;
	public GameObject SuperBG;


	CameraMoveScript cameraMove;
	TimeManagerScript timeManager;
	SpriteRenderer spriteRenderer;
	SoundsPlayer sound;

	void Update(){
		if (Input.GetKeyDown (KeyCode.DownArrow)) {
			StartDeathAnim ();
		}
	}

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
		spriteAnimator.SetBlockAnimation (StartBlockAnim);
		spriteAnimator.SetThrowTryAnimation (StartThrowTryAnim);
		spriteAnimator.SetThrowCompleteAnimation (StartThrowCompleteAnim);
		spriteAnimator.SetSuperAnimation (StartSuperAnim);
		spriteAnimator.setWinAnimation (StartWinAnim);
		hurtbox.gameObject.GetComponent<HealthScript> ().SetDeathFunc (StartDeathAnim);
		StartIntroAnim ();
		sound = GetComponent<SoundsPlayer> ();
		timeManager = GameObject.Find ("MasterGameObject").GetComponent<TimeManagerScript> ();

		cameraMove = GameObject.Find ("Camera").GetComponent<CameraMoveScript>();
	}


	//IEnumerator 
	IEnumerator loopAnimation(Sprite[] animationFrames){

		int currentFrame = 0;
		while (true) {

			if (currentFrame >= animationFrames.Length) {
				currentFrame = 0;
			}
			spriteRenderer.sprite = animationFrames [currentFrame];
			currentFrame++;
			// number of frames to wait
			for (int x = 0; x < 5;) {
				yield return null;
				if (!timeManager.CheckIfTimePaused()) {
					x++;
				}
			}
		}

	}
	IEnumerator loopBackwardsAnimation(Sprite[] animationFrames){

		int currentFrame = animationFrames.Length -1;
		while (true) {

			if (currentFrame <= 0 ) {
				currentFrame = animationFrames.Length -1;
			}
			spriteRenderer.sprite = animationFrames [currentFrame];
			currentFrame--;
			// number of frames to wait
			for (int x = 0; x < 5;) {
				yield return null;
				if (!timeManager.CheckIfTimePaused()) {
					x++;
				}
			}
		}

	}
	IEnumerator NeutralLoop(){
		while (true) {
			for(int i = 0; i < neutralFrames.Length; i++){
				spriteRenderer.sprite = neutralFrames [i];
				for (int x = 0; x < 3;) {
					yield return null;
					if (!timeManager.CheckIfTimePaused ()) {
						x++;
					}
				}
			}
			for(int i = neutralFrames.Length -1; i >0; i--){
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
	IEnumerator AnimateOnce (Sprite[] animationFrames){
		for(int i = 0; i < animationFrames.Length; i++){
			spriteRenderer.sprite = animationFrames [i];
			for (int x = 0; x < 3;) {
				yield return null;
				if (!timeManager.CheckIfTimePaused ()) {
					x++;
				}
			}
		}
	}
	IEnumerator DeathAnim(){
		Debug.Log ("death");
		sound.PlayDeath ();

		timeManager.StopTimeForce (60);
		head.transform.position = transform.position;
		head.SetActive (true);
		head.GetComponent<Rigidbody2D> ().AddForceAtPosition (new Vector2(0, 250), new Vector2(.01f,1));
		for(int i = 0; i < deathFrames.Length; i++){
			spriteRenderer.sprite = deathFrames [i];
			for (int x = 0; x < 5; x++) {
				yield return null;
			}
		}
	}
	IEnumerator introAnim(){
		spriteRenderer.sprite = introFrames [0];

		for(int i = 0; i < introFrames.Length; i++){
			spriteRenderer.sprite = introFrames [i];
			for (int x = 0; x < 6;) {
				yield return null;
				if (!timeManager.CheckIfTimePaused ()) {
					x++;
				}
			}
		}
		GetComponent<InputScript> ().inputEnabled = true;
		StartNeutralAnim ();
	}

	IEnumerator JumpNeutral(){

		hurtbox.offset = new Vector2 (0, 0);
		hurtbox.size  = new Vector2 (1.15f, 1.5f);
		spriteRenderer.sprite = neutralJumpFrames [0];
		for (int ii = 0; ii < 2; ii++) {
			for (int i = 0; i < 7; i++) {
				spriteRenderer.sprite = neutralJumpFrames [i];
				for (int x = 0; x < 3;) {
					yield return null;
					if (!timeManager.CheckIfTimePaused()) {
						x++;
					}
				}
			}	
		}

		hurtbox.offset = new Vector2 ( 0, -.66f);
		hurtbox.size  = new Vector2 (1.15f, 3.3f);
	}
	IEnumerator JumpAway(){

		hurtbox.offset = new Vector2 (0, 0);
		hurtbox.size  = new Vector2 (1.15f, 1.5f);
		spriteRenderer.sprite = neutralJumpFrames [neutralJumpFrames.Length-1];
		for (int ii = 0; ii < 2; ii++) {
			for (int i = neutralJumpFrames.Length-1; i >= 0; i--) {
				spriteRenderer.sprite = neutralJumpFrames [i];
				for (int x = 0; x < 3;) {
					yield return null;
					if (!timeManager.CheckIfTimePaused()) {
						x++;
					}
				}
			}	
		}

		hurtbox.offset = new Vector2 ( 0, -.66f);
		hurtbox.size  = new Vector2 (1.15f, 3.3f);
	}
	IEnumerator Hit(int duration){
		for (int i = 0; i < 3; i++) {
			spriteRenderer.sprite = HitFrames [i];
			for (int x = 0; x < 3;) {
				yield return null;
				if (!timeManager.CheckIfTimePaused()) {
					x++;
					duration--;
				}	
			}
		}
	}
	IEnumerator Block(){
		for (int i = 0; i < 2; i++) {
			spriteRenderer.sprite = BlockFrames [i];
			for (int x = 0; x < 3;) {
				yield return null;
				if (!timeManager.CheckIfTimePaused()) {
					x++;
				}
			}
		}
	}
	// depricated
	IEnumerator Light(){
		sound.PlayHit ();
		spriteRenderer.sprite = lightFrames [0];
		for (int x = 0; x < 4;) {
			yield return null;
			if (!timeManager.CheckIfTimePaused()) {
				x++;
			}
		}
		spriteRenderer.sprite = lightFrames [1];
		for (int x = 0; x < 7;) {
			yield return null;
			if (!timeManager.CheckIfTimePaused()) {
				x++;
			}
		}
		spriteRenderer.sprite = lightFrames [0];
	}
	IEnumerator Medium(){
		sound.PlayHit ();
		for (int i = 0; i < 7; i++) {
			spriteRenderer.sprite = mediumFrames [i];
			for (int x = 0; x < 4;) {
				yield return null;
				if (!timeManager.CheckIfTimePaused()) {
					x++;
				}
			}
		}
	}
	IEnumerator Heavy(){
		sound.PlayHit ();
		for (int i = 0; i < 5; i++) {
			spriteRenderer.sprite = heavyFrames [i];
			for (int x = 0; x < 3;) {
				yield return null;
				if (!timeManager.CheckIfTimePaused()) {
					x++;
				}
			}
		}
	}
	IEnumerator JumpLight(){
		hurtbox.offset = new Vector2 (0, 0);
		hurtbox.size  = new Vector2 (1.15f, 1.5f);
		for (int i = 0; i < 2; i++) {
			spriteRenderer.sprite = jumpLightFrames [i];
			for (int x = 0; x < 3;) {
				yield return null;
				if (!timeManager.CheckIfTimePaused()) {
					x++;
				}
			}
		}
	}
	IEnumerator JumpMedium(){
		hurtbox.offset = new Vector2 (0, 0);
		hurtbox.size  = new Vector2 (1.15f, 1.5f);
		for (int i = 0; i < 2; i++) {
			spriteRenderer.sprite = jumpMediumFrames [i];

			for (int x = 0; x < 3;) {
				yield return null;
				if (!timeManager.CheckIfTimePaused()) {
					x++;
				}
			}
		}
	}
	IEnumerator JumpHeavy(){
		hurtbox.offset = new Vector2 (0, 0);
		hurtbox.size  = new Vector2 (1.15f, 1.5f);
		for (int i = 0; i < 2; i++) {
			spriteRenderer.sprite = jumpHeavyFrames [i];
			for (int x = 0; x < 3;) {
				yield return null;
				if (!timeManager.CheckIfTimePaused()) {
					x++;
				}
			}
		}
	}
	IEnumerator SpecialOne(){
		for (int i = 0; i < 4; i++) {
			spriteRenderer.sprite = SpecialOneFrames [i];
			for (int x = 0; x < 4;) {
				yield return null;
				if (!timeManager.CheckIfTimePaused()) {
					x++;
				}
			}
		}
	}


	IEnumerator SpecialTwo(){
		for (int i = 0; i < 3; i++) {
			if (i == 1) {
				sound.PlaySP2 ();
			}
			spriteRenderer.sprite = SpecialTwoFrames [i];
			for (int x = 0; x < 5;) {
				yield return null;
				if (!timeManager.CheckIfTimePaused()) {
					x++;
				}
			}
		}
	}

	IEnumerator SpecialThree(){
		sound.PlaySP3 ();
		for (int i = 0; i < 3; i++) {
			spriteRenderer.sprite = SpecialThreeFrames [i];
			for (int x = 0; x < 4;) {
				yield return null;
				if (!timeManager.CheckIfTimePaused()) {
					x++;
				}
			}
		}
	}
	IEnumerator ThrowTry(){
		spriteRenderer.sprite = throwFrames [0];
		for (int x = 0; x < 10;) {
			yield return null;
			if (!timeManager.CheckIfTimePaused()) {
				x++;
			}
		}
	}
	IEnumerator ThrowComplete(){
		Debug.Log ("throw");

		cameraMove.EnableCameraMovement (false);
		for (int i = 1; i < 7; i++) {
			spriteRenderer.sprite = throwFrames [i];

			for (int x = 0; x < 3;) {
				yield return null;
				if (!timeManager.CheckIfTimePaused()) {
					x++;
				}
			}
		}

		cameraMove.EnableCameraMovement (true);
	}
	IEnumerator SuperAnim(){
		// super anim
		// super sound
		sound.PlaySuperBg();
		SuperBG.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + 2);
		SuperBG.SetActive(true);
		timeManager.StopTime (30);
		sound.PlaySuperWord ();

		//sound.PlaySP1 ();
		for (int i = 0; i < 3; i++) {
			spriteRenderer.sprite = superFrames [i];

			for (int x = 0; x < 5;) {
				yield return null;
				if (!timeManager.CheckIfTimePaused()) {
					x++;
				}
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
		StartCoroutine (JumpNeutral());
	}
	public void StartAwayJumpAnim(){
		EndAnimations ();
		StartCoroutine (JumpAway());
	}
	public void StartNeutralJumpAnim(){
		EndAnimations ();
		StartCoroutine (JumpNeutral());
	}
	public void StartHitAnim(int duration){
		EndAnimations ();
		StartCoroutine (Hit(duration));
	}
	public void StartBlockAnim(){
		EndAnimations ();
		StartCoroutine (Block());
	}
	public void StartWalkAnim(){
		EndAnimations ();
		StartCoroutine (loopAnimation (walkFrames));
	}
	public void StartWalkAwayAnim(){
		EndAnimations ();
		StartCoroutine (loopBackwardsAnimation (walkFrames));
	}

	public void StartNeutralAnim(){
		EndAnimations ();
		StartCoroutine (NeutralLoop());
	}
	public void StartThrowTryAnim(){
		EndAnimations ();
		StartCoroutine (ThrowTry ());
	}
	public void StartThrowCompleteAnim(){
		EndAnimations ();
		StartCoroutine (ThrowComplete ());
	}
	public void StartDeathAnim(){
		EndAnimations ();
		StartCoroutine (DeathAnim());
	}
	public void StartWinAnim(){
		Debug.Log ("win");
		EndAnimations ();
		StartCoroutine (AnimateOnce(winFrames));
	}
	public void StartIntroAnim(){
		EndAnimations ();
		StartCoroutine (introAnim());
	}
	public void StartSuperAnim(){
		EndAnimations ();
		StartCoroutine (SuperAnim());
	}
	public void EndAnimations(){
		hurtbox.size  = new Vector2 (1.15f, 3.3f);
		StopAllCoroutines ();
	}
}
