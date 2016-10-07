using UnityEngine;
using System.Collections;

public class ShanaAnim : MonoBehaviour {

	public Sprite[] walkFrames;
	public Sprite[] walkAwayFrames;
	public Sprite[] neutralFrames;
	public Sprite[] neutralJumpFrames;
	public Sprite[] towardJumpFrames;
	public Sprite[] awayJumpFrames;
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
	public Sprite[] SuperFrames;
	public Sprite[] HitFrames;
	public Sprite[] BlockFrames;
	public Sprite[] introFrames;
	public Sprite[] winFrames;
	public Sprite[] deathFrames;
	public Sprite[] getUpFrames;
	public SpriteAnimator spriteAnimator;
	public Transform hurtboxBody;
	public Transform hurtboxLimb;
	public GameObject SuperBG;
	public GameObject superEffect;
	Vector3 hurtboxBodyOriginalPosition, hurtboxBodyOriginalScale;


	CameraMoveScript cameraMove;
	TimeManagerScript timeManager;
	SpriteRenderer spriteRenderer;
	SoundsPlayer sound;
	PlayerMovementScript PMS;
	// Use this for initialization
	void Start () {
		PMS = GetComponent<PlayerMovementScript> ();
		hurtboxBodyOriginalPosition = hurtboxBody.transform.localPosition;
		hurtboxBodyOriginalScale = hurtboxBody.transform.localScale;

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
		spriteAnimator.setKnockdownAnimation (StartknockdownAnim);
		spriteAnimator.setGetupAnimation (StartGetUpAnim);
		spriteAnimator.setExtra1Animation (StartSpecialTwoAirAnim);
		if (hurtboxBody.gameObject.GetComponentInParent<HealthScript> () != null) {
			hurtboxBody.gameObject.GetComponentInParent<HealthScript> ().SetDeathFunc (StartDeathAnim);
		}
		StartIntroAnim ();
		sound = GetComponent<SoundsPlayer> ();
		timeManager = GameObject.Find ("MasterGameObject").GetComponent<TimeManagerScript> ();
		if (GameObject.Find ("Camera") != null) {
			cameraMove = GameObject.Find ("Camera").GetComponent<CameraMoveScript> ();
		}
	}


	void SetJumpHitbox(){
		hurtboxBody.transform.localPosition = new Vector2 (0, 0);
		hurtboxBody.transform.localScale  = new Vector2 (1.75f, 1.75f);
	}
	void SetHurtbox(Vector2 position, Vector2 scale, Transform hurtbox){
		hurtbox.localPosition = position;
		hurtbox.localScale = scale;
		hurtbox.gameObject.SetActive (true);
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
			for (int x = 0; x < 3;) {
				yield return null;
				if (!timeManager.CheckIfTimePaused()) {
					x++;
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
	IEnumerator WinAnim(){
		for (int x = 0; x < 10;) {
			yield return null;
			if (!timeManager.CheckIfTimePaused ()) {
				x++;
			}
		}
		for(int i = 0; i < winFrames.Length; i++){
			spriteRenderer.sprite = winFrames [i];
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
		for(int i = 0; i < deathFrames.Length; i++){
			spriteRenderer.sprite = deathFrames [i];
			for (int x = 0; x < 5; x++) {
				yield return null;
			}
		}
	}
	IEnumerator KnockdownAnim(){
		for(int i = 0; i < deathFrames.Length; ){
			spriteRenderer.sprite = deathFrames [i];
			if (i == 3 && PMS.ForceGroundCheck ()) {
				i++;
			} else if (i != 3) {
				i++;
			}
			for (int x = 0; x < 3;) {
				if (!timeManager.CheckIfTimePaused ()) {
					x++;
				}
				yield return null;
			}
		}
	}
	IEnumerator GetUpAnim(){
		for(int i = 0; i < getUpFrames.Length; i++){
			spriteRenderer.sprite = getUpFrames [i];
			for (int x = 0; x < 3; x++) {
				yield return null;
			}
		}
	}
	IEnumerator introAnim(){
		spriteRenderer.sprite = introFrames [0];
		for (int x = 0; x < 70;) {
			yield return null;
			if (!timeManager.CheckIfTimePaused ()) {
				x++;
			}
		}
		for(int i = 0; i < introFrames.Length; i++){
			spriteRenderer.sprite = introFrames [i];
			for (int x = 0; x < 6;) {
				yield return null;
				if (!timeManager.CheckIfTimePaused ()) {
					x++;
				}
			}
		}
		StartNeutralAnim ();
	}
	IEnumerator FallingAnim(){
		while (true) {
			for (int i = 6; i < 8; i++) {
				spriteRenderer.sprite = neutralJumpFrames [i];
				for (int x = 0; x < 3;) {
					yield return null;
					if (!timeManager.CheckIfTimePaused()) {
						x++;
					}
				}
			}
		}
	}
	IEnumerator JumpTowards(){
		SetJumpHitbox ();
		spriteRenderer.sprite = towardJumpFrames [0];
		for (int x = 0; x < 3;) {
			yield return null;
			if (!timeManager.CheckIfTimePaused()) {
				x++;
			}
		}
		spriteRenderer.sprite = towardJumpFrames [1];
		for (int x = 0; x < 5;) {
			yield return null;
			if (!timeManager.CheckIfTimePaused()) {
				x++;
			}
		}
		for (int i = 0; i < 12; i++) {
			spriteRenderer.sprite = towardJumpFrames [i+2];
			for (int x = 0; x < 3;) {
				yield return null;
				if (!timeManager.CheckIfTimePaused()) {
					x++;
				}
			}
		}
	}
	IEnumerator JumpAway(){
		SetJumpHitbox ();
		spriteRenderer.sprite = towardJumpFrames [13];
		for (int x = 0; x < 3;) {
			yield return null;
			if (!timeManager.CheckIfTimePaused()) {
				x++;
			}
		}
		spriteRenderer.sprite = towardJumpFrames [12];
		for (int x = 10; x < 10;) {
			yield return null;
			if (!timeManager.CheckIfTimePaused()) {
				x++;
			}
		}
		for (int i = 11; i > 0; i--) {
			spriteRenderer.sprite = towardJumpFrames [i];
			for (int x = 0; x < 3;) {
				yield return null;
				if (!timeManager.CheckIfTimePaused()) {
					x++;
				}
			}
		}
		spriteRenderer.sprite = towardJumpFrames [13];
	}
	IEnumerator JumpNeutral(){

		SetJumpHitbox ();
		spriteRenderer.sprite = neutralJumpFrames [0];
		for (int x = 0; x < 3;) {
			yield return null;
			if (!timeManager.CheckIfTimePaused()) {
				x++;
			}
		}
		spriteRenderer.sprite = neutralJumpFrames [1];
		for (int x = 0; x < 6;) {
			yield return null;
			if (!timeManager.CheckIfTimePaused()) {
				x++;
			}
		}
		for (int i = 2; i < 8; i++) {
			spriteRenderer.sprite = neutralJumpFrames [i];
			for (int x = 0; x < 3;) {
				yield return null;
				if (!timeManager.CheckIfTimePaused()) {
					x++;
				}
			}
		}

		StartCoroutine (FallingAnim ());
	}
	IEnumerator Hit(int duration){
		for (int i = 0; i < 4; i++) {
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
		for (int i = 0; i < 3; i++) {
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
		//SetHurtbox(new Vector2 (1.5f, .5f), new Vector2 (2, .75f), hurtboxLimb);

		for (int i = 0; i < 4; i++) {
			spriteRenderer.sprite = lightFrames [i];
			if (i == 1) {
				sound.PlayLight ();
			}
			for (int x = 0; x < 3;) {
				yield return null;
				if (!timeManager.CheckIfTimePaused()) {
					x++;
				}
			}
		}
		hurtboxLimb.gameObject.SetActive (false);
	}
	IEnumerator Medium(){
		//SetHurtbox(new Vector2 (0f, -1.2f), new Vector2 (1.8f, 2f), hurtboxBody);
		for (int i = 0; i < 7; i++) {
			if (i == 1) {
				sound.PlayMedium ();
			}
			spriteRenderer.sprite = mediumFrames [i];

			if (i == 2) {
				//SetHurtbox (new Vector2 (2f, -1.8f), new Vector2 (2.7f, 1f),hurtboxLimb);
			}
			if (i == 5) {
				hurtboxLimb.gameObject.SetActive (false);
			}
			for (int x = 0; x < 3;) {
				yield return null;
				if (!timeManager.CheckIfTimePaused()) {
					x++;
				}
			}
		}
	}
	IEnumerator Heavy(){
		for (int i = 0; i < 7; i++) {
			if (i == 1) {
				sound.PlayHeavy ();
			}
			spriteRenderer.sprite = heavyFrames [i];
			if (i == 2) {
				for (int x = 0; x < 8;) {
					yield return null;
					if (!timeManager.CheckIfTimePaused()) {
						x++;
					}
				}
			}
			for (int x = 0; x < 4;) {
				yield return null;
				if (!timeManager.CheckIfTimePaused()) {
					x++;
				}
			}
		}
	}
	IEnumerator JumpLight(){
		SetJumpHitbox ();
		for (int ii = 0; ii < 2; ii++) {
			for (int i = 0; i < 3; i++) {
				spriteRenderer.sprite = jumpLightFrames [i];
				for (int x = 0; x < 3;) {
					yield return null;
					if (!timeManager.CheckIfTimePaused ()) {
						x++;
					}
				}
			}
		}
		StartCoroutine (FallingAnim ());
	}

	IEnumerator JumpMedium(){
		SetJumpHitbox ();
		for (int i = 0; i < 6; i++) {
			spriteRenderer.sprite = jumpMediumFrames [i];

			for (int x = 0; x < 2;) {
				yield return null;
				if (!timeManager.CheckIfTimePaused()) {
					x++;
				}
			}
		}
		StartCoroutine (FallingAnim ());
	}

	IEnumerator JumpHeavy(){
		SetJumpHitbox ();
		for (int i = 0; i < 5; i++) {

			//SetHurtbox(new Vector2 (1.8f, -.3f), new Vector2 (2f, 1f), hurtboxLimb);
			spriteRenderer.sprite = jumpHeavyFrames [i];
			for (int x = 0; x < 2;) {
				yield return null;
				if (!timeManager.CheckIfTimePaused()) {
					x++;
				}
			}
		}

		StartCoroutine (FallingAnim ());
		//hurtboxLimb.gameObject.SetActive (false);
	}
	IEnumerator SpecialOne(){
		for (int i = 0; i < 2; i++) {
			spriteRenderer.sprite = SpecialOneFrames [i];

			for (int x = 0; x < 6;) {
				yield return null;
				if (!timeManager.CheckIfTimePaused()) {
					x++;
				}
			}
		}

		sound.PlaySP1 ();
		for (int ii = 0; ii < 5; ii++) {		
			for (int i = 2; i < 4; i++) {
				spriteRenderer.sprite = SpecialOneFrames [i];
				for (int x = 0; x < 3;) {
					yield return null;
					if (!timeManager.CheckIfTimePaused ()) {
						x++;
					}
				}
			}
		}


		spriteRenderer.sprite = SpecialOneFrames [5];

	}


	IEnumerator SpecialTwo(){
		for (int i = 0; i < 2; i++) {
			if (i == 1) {
				sound.PlaySP2 ();
			}
			spriteRenderer.sprite = SpecialTwoFrames [i];
			for (int x = 0; x < 3;) {
				yield return null;
				if (!timeManager.CheckIfTimePaused()) {
					x++;
				}
			}
		}
		for (int ii = 0; ii < 2; ii++) {
			for (int i = 2; i < 5; i++) {
			
				spriteRenderer.sprite = SpecialTwoFrames [i];
				for (int x = 0; x < 3;) {
					yield return null;
					if (!timeManager.CheckIfTimePaused ()) {
						x++;
					}
				}
			}
		}
		for (int i = 5; i < 9; i++) {
			
			spriteRenderer.sprite = SpecialTwoFrames [i];
			for (int x = 0; x < 3;) {
				yield return null;
				if (!timeManager.CheckIfTimePaused()) {
					x++;
				}
			}
		}
	}
	IEnumerator SpecialTwoAir(){
		for (int i = 0; i < 2; i++) {
			if (i == 1) {
				sound.PlaySP2 ();
			}
			spriteRenderer.sprite = SpecialTwoFrames [i];
			for (int x = 0; x < 3;) {
				yield return null;
				if (!timeManager.CheckIfTimePaused()) {
					x++;
				}
			}
		}
		for (int ii = 0; ii < 2; ii++) {
			for (int i = 2; i < 5; i++) {

				spriteRenderer.sprite = SpecialTwoFrames [i];
				for (int x = 0; x < 3;) {
					yield return null;
					if (!timeManager.CheckIfTimePaused ()) {
						x++;
					}
				}
			}
		}
		StartCoroutine (FallingAnim());
	}
	IEnumerator SpecialThree(){
		sound.PlaySP3 ();
		for (int i = 0; i < 2; i++) {
			spriteRenderer.sprite = SpecialThreeFrames [i];
			for (int x = 0; x < 3;) {
				yield return null;
				if (!timeManager.CheckIfTimePaused()) {
					x++;
				}
			}
		}

		//SetHurtbox(new Vector2 (1.8f, 0f), new Vector2 (2f, 1f), hurtboxLimb);
		for (int ii = 0; ii < 2; ii++) {
			for (int i = 2; i < 5; i++) {
				spriteRenderer.sprite = SpecialThreeFrames [i];
				for (int x = 0; x < 3;) {
					yield return null;
					if (!timeManager.CheckIfTimePaused()) {
						x++;
					}
				}
			}
		}

		spriteRenderer.sprite = SpecialThreeFrames [5];
	}
	IEnumerator ThrowTry(){
		for (int i = 0; i < 6; i++) {

			spriteRenderer.sprite = throwFrames [i];
		
			for (int x = 0; x < 3;) {
				yield return null;
				if (!timeManager.CheckIfTimePaused ()) {
					x++;
				}
			}
		}
	}
	IEnumerator ThrowComplete(){
		Debug.Log ("throw");
		if (cameraMove != null) {
			cameraMove.EnableCameraMovement (false);
		}
		for (int i = 6; i < 14; i++) {
			spriteRenderer.sprite = throwFrames [i];

			for (int x = 0; x < 5;) {
				yield return null;
				if (!timeManager.CheckIfTimePaused()) {
					x++;
				}
			}
		}

		if (cameraMove != null) {
			cameraMove.EnableCameraMovement (true);
		}
	}
	IEnumerator SuperAnim(){
		// super anim
		// super sound
		sound.PlaySuperBg();
		SuperBG.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + 2);
		SuperBG.SetActive(true);
		superEffect.SetActive (true);
		cameraMove.FocusForSuper (transform.position, 20);
		timeManager.StopTime (75);
		sound.PlaySuperWord ();
		//sound.PlaySP1 ();
		for (int i = 0; i < 3; i++) {
			spriteRenderer.sprite = SuperFrames [i];
			for (int x = 0; x < 3;x++) {
				yield return null;
					
			}
		}
		//standing loop
		for (int ii = 0; ii < 4; ii++) {		
			for (int i = 3; i < 5; i++) {
				spriteRenderer.sprite = SuperFrames [i];
				for (int x = 0; x < 3;x++) {
					yield return null;

				}
			}
		}
		for (int i = 5; i < 7; i++) {
			spriteRenderer.sprite = SuperFrames [i];
			for (int x = 0; x < 3;) {
				yield return null;
				if (!timeManager.CheckIfTimePaused ()) {
					PMS.MoveToward(0, 10f);
					x++;
				}
			}
		}
		sound.PlayExtra3 ();
		sound.PlayExtra ();
		//air loop
		for (int ii = 0; ii < 10; ii++) {		
			for (int i = 7; i < 9; i++) {
				spriteRenderer.sprite = SuperFrames [i];
				PMS.MoveToward(0, .01f);
				for (int x = 0; x < 3;) {
					yield return null;
					if (!timeManager.CheckIfTimePaused ()) {
						x++;
					}
				}
			}
		}
		for (int i = 9; i < 11; i++) {
			spriteRenderer.sprite = SuperFrames [i];
			for (int x = 0; x < 3;) {
				yield return null;
				if (!timeManager.CheckIfTimePaused ()) {
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
	public void StartSpecialTwoAirAnim(){
		EndAnimations ();
		StartCoroutine (SpecialTwoAir());
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
		StartCoroutine (JumpNeutral());
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
		StartCoroutine (loopAnimation (walkAwayFrames));
	}

	public void StartNeutralAnim(){
		EndAnimations ();
		StartCoroutine (loopAnimation (neutralFrames));
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
		StartCoroutine (WinAnim());
	}
	public void StartIntroAnim(){
		EndAnimations ();
		StartCoroutine (introAnim());
	}
	public void StartSuperAnim(){
		EndAnimations ();
		StartCoroutine (SuperAnim());
	}
	public void StartknockdownAnim(){
		EndAnimations ();
		StartCoroutine (KnockdownAnim());
	}
	public void StartGetUpAnim(){
		EndAnimations ();
		StartCoroutine (GetUpAnim());
	}
	public void EndAnimations(){
		StopAllCoroutines ();
		hurtboxBody.transform.localScale  = hurtboxBodyOriginalScale;
		hurtboxBody.transform.localPosition  = hurtboxBodyOriginalPosition;
		hurtboxLimb.gameObject.SetActive (false);
	}
}
