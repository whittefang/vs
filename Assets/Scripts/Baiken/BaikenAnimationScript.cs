using UnityEngine;
using System.Collections;

public class BaikenAnimationScript : MonoBehaviour {

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
	public Sprite[] HitFrames;
	public Sprite[] BlockFrames;
	public Sprite[] introFrames;
	public Sprite[] winFrames;
	public Sprite[] deathFrames;
	public Sprite[] getUpFrames;
	public Sprite[] superFrames;
	public SpriteAnimator spriteAnimator;
	public Transform hurtboxBody;
	public Transform hurtboxLimb;
	public GameObject superBackgroundBlack;
	public GameObject SuperBG;
	Vector3 hurtboxBodyOriginalPosition, hurtboxBodyOriginalScale;


	CameraMoveScript cameraMove;
	TimeManagerScript timeManager;
	public SpriteRenderer spriteRenderer;
	SoundsPlayer sound;
	PlayerMovementScript PMS;
	// Use this for initialization
	void Start () {
		PMS = GetComponent<PlayerMovementScript> ();
		hurtboxBodyOriginalPosition = hurtboxBody.transform.localPosition;
		hurtboxBodyOriginalScale = hurtboxBody.transform.localScale;

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
		spriteAnimator.setExtra1Animation (StartSpecialTwoCompleteAnim);
		spriteAnimator.setExtra2Animation (StartSpecialThreeHitAnim);
		spriteAnimator.setExtra3Animation (StartSpecialThreeWhiffAnim);
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
		hurtboxBody.transform.localPosition = new Vector2 (0, 2.1f);
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
	IEnumerator loopAnimationForwardBack(Sprite[] animationFrames){

		int currentFrame = 0;
		while (true) {

			for (int i = 0; i < animationFrames.Length; i++) {			
				spriteRenderer.sprite = animationFrames [i];
				// number of frames to wait
				for (int x = 0; x < 3;) {
					yield return null;
					if (!timeManager.CheckIfTimePaused ()) {
						x++;
					}
				}
			}
			for (int i = animationFrames.Length - 1; i > 0 ; i--) {			
				spriteRenderer.sprite = animationFrames [i];
				// number of frames to wait
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
	IEnumerator WinAnim(){
		for (int x = 0; x < 10;) {
			yield return null;
			if (!timeManager.CheckIfTimePaused ()) {
				x++;
			}
		}
		for(int i = 0; i < winFrames.Length; i++){
			spriteRenderer.sprite = winFrames [i];
			for (int x = 0; x < 6;) {
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
			if (i == 7 && PMS.ForceGroundCheck ()) {
				i++;
			} else if (i != 7) {
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
		if (GetComponent<InputScript> () != null) {
			GetComponent<InputScript> ().inputEnabled = true;
		}
		StartNeutralAnim ();
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

		for (int i = 0; i < 8; i++) {
			spriteRenderer.sprite = neutralJumpFrames [i];
			for (int x = 0; x < 3;) {
				yield return null;
				if (!timeManager.CheckIfTimePaused()) {
					x++;
				}
			}
		}
	}
	IEnumerator Hit(int duration){
		for (int i = 0; i < 2; i++) {
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
		for (int i = 3; i > 0; i--) {
			spriteRenderer.sprite = lightFrames [i];
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
		for (int i = 0; i < 8; i++) {
			spriteRenderer.sprite = mediumFrames [i];
			if (i == 1) {
				sound.PlayMedium ();
			}
			if (i == 2) {
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
		
		for (int i = 0; i < 14; i++) {
			spriteRenderer.sprite = heavyFrames [i];
			if (i == 1) {
				sound.PlayHeavy ();
			}
			if (i < 2) {
				for (int x = 0; x < 2;) {
					yield return null;
					if (!timeManager.CheckIfTimePaused()) {
						x++;
					}
				}
			}
			if (i > 8) {
				for (int x = 0; x < 2;) {
					yield return null;
					if (!timeManager.CheckIfTimePaused()) {
						x++;
					}
				}
			}
			if (i == 10) {
				hurtboxLimb.gameObject.SetActive (false);
			}
			if (i == 12) {
				for (int x = 0; x < 4;) {
					yield return null;
					if (!timeManager.CheckIfTimePaused()) {
						x++;
					}
				}
			}
			for (int x = 0; x < 2;) {
				yield return null;
				if (!timeManager.CheckIfTimePaused()) {
					x++;
				}
			}
		}
	}
	IEnumerator JumpLight(){
		SetJumpHitbox ();
		for (int i = 0; i < 7; i++) {
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
		SetJumpHitbox ();
		for (int i = 0; i < 12; i++) {
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
		SetJumpHitbox ();
		for (int i = 0; i < 9; i++) {

			spriteRenderer.sprite = jumpHeavyFrames [i];
			for (int x = 0; x < 3;) {
				yield return null;
				if (!timeManager.CheckIfTimePaused()) {
					x++;
				}
			}
		}
		hurtboxLimb.gameObject.SetActive (false);
	}
	IEnumerator SpecialOne(){
		for (int i = 0; i < 8; i++) {
			if (i == 1){
				sound.PlaySP1 ();
			}
			spriteRenderer.sprite = SpecialOneFrames [i];
			if (i == 5) {
				for (int x = 0; x < 6;) {
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
	}


	IEnumerator SpecialTwo(){
		for (int i = 0; i < 3; i++) {
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
	}
	IEnumerator SpecialTwoComplete(){
		for (int i = 3; i < 16 ; i++) {
			if (i == 1) {
				sound.PlayExtra2 ();
			}
			spriteRenderer.sprite = SpecialTwoFrames [i];
			for (int x = 0; x < 3;) {
				yield return null;
				if (!timeManager.CheckIfTimePaused()) {
					x++;
				}
			}
		}
	}
	IEnumerator SpecialThree(){
		sound.PlaySP3 ();
		for (int i = 0; i < 4; i++) {
			

			spriteRenderer.sprite = SpecialThreeFrames [i];
			for (int x = 0; x < 4;) {
				yield return null;
				if (!timeManager.CheckIfTimePaused()) {
					x++;
				}
			}
		}
		while (true) {
			for (int i = 4; i < 6; i++) {
				spriteRenderer.sprite = SpecialThreeFrames [i];
				for (int x = 0; x < 3;) {
					yield return null;
					if (!timeManager.CheckIfTimePaused()) {
						x++;
					}
				}
			}
		}
	}
	IEnumerator SpecialThreewWhiff(){
		for (int i = 5; i < 12; i++) {
			spriteRenderer.sprite = SpecialThreeFrames [i];
			for (int x = 0; x < 3;) {
				yield return null;
				if (!timeManager.CheckIfTimePaused()) {
					x++;
				}
			}
		}
	}
	IEnumerator SpecialThreeHit(){
		sound.PlayExtra3 ();
		for (int i = 5; i < 27; i++) {
			spriteRenderer.sprite = SpecialThreeFrames [i];
			for (int x = 0; x < 3;) {
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
		if (cameraMove != null) {
			cameraMove.EnableCameraMovement (false);
		}
		for (int i = 1; i < 19; i++) {
			spriteRenderer.sprite = throwFrames [i];

			for (int x = 0; x < 3;) {
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
		timeManager.StopTime (90);
		sound.PlaySuperWord ();

		//sound.PlaySP1 ();
		spriteRenderer.sprite = superFrames [0];
		for (int x = 0; x < 30; x++) {
			yield return null;
		}
		for (int i = 0; i < 6; i++) {
			spriteRenderer.sprite = superFrames [i];
			for (int x = 0; x < 9;) {
				yield return null;
				x++;
			}
		}
		for (int x = 0; x < 10; x++) {
			if (x == 40){
				superBackgroundBlack.SetActive (true);
			}
			yield return null;
		}
		for (int i = 6; i < 10; i++) {
			spriteRenderer.sprite = superFrames [i];
			for (int x = 0; x < 9;) {
				yield return null;
				x++;
			}
		}
		superBackgroundBlack.SetActive (false);
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
		StartCoroutine (loopAnimationForwardBack (walkFrames));
	}
	public void StartWalkAwayAnim(){
		EndAnimations ();
		StartCoroutine (loopAnimationForwardBack (walkAwayFrames));
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
	public void StartSpecialTwoCompleteAnim(){
		EndAnimations ();
		StartCoroutine (SpecialTwoComplete());
	}
	public void StartSpecialThreeHitAnim(){
		EndAnimations ();
		StartCoroutine (SpecialThreeHit());
	}
	public void StartSpecialThreeWhiffAnim(){
		EndAnimations ();
		StartCoroutine (SpecialThreewWhiff());
	}
	public void EndAnimations(){
		StopAllCoroutines ();
		hurtboxBody.transform.localScale  = hurtboxBodyOriginalScale;
		hurtboxBody.transform.localPosition  = hurtboxBodyOriginalPosition;
		hurtboxLimb.gameObject.SetActive (false);
		superBackgroundBlack.SetActive (false);
	}
}
