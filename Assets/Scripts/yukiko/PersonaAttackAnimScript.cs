using UnityEngine;
using System.Collections;

public class PersonaAttackAnimScript : MonoBehaviour {
	public Sprite[] SendoutFrames;
	public Sprite[] Attack1Frames;
	public Sprite[] Attack2Frames;
	public Sprite[] Attack3Frames;
	public Sprite[] SpecialOneFrames;
	public Sprite[] SpecialTwoFrames;
	public Sprite[] SpecialThreeFrames;
	public Sprite[] superFrames;
	public Sprite[] jumpMediumFrames;
	public playerDetectionScript detector;
	public GameObject sp1Fireball, sp3TrapPre, sp3TrapAttack, attack1Hitbox,attack2Hitbox, attack3Hitbox, mediumHitbox
					, dpHitbox, superHitbox, superEffects;
	SpriteRenderer spriteRenderer;
	TimeManagerScript timeManager;
	Rigidbody2D RB;
	Transform otherPlayer;
	public bool isActive = false;
	bool alive = true;
	public Transform fireballPos, trapPos;
	int attackStage =0;
	int maxCards = 4;
	int currentCards = 4;
	GameObject hitboxes;
	public PersonaCardsScript Cards;
	public YukikoAttackScript yukiAttack;
	// Use this for initialization
	void Awake () {
		spriteRenderer = GetComponent<SpriteRenderer> ();
		RB = GetComponent<Rigidbody2D> ();
		timeManager = GameObject.Find ("MasterGameObject").GetComponent<TimeManagerScript> ();
		hitboxes = attack1Hitbox.transform.parent.gameObject;
	}
	public void SetOtherPlayer(bool isP1){
		Cards.transform.parent = GameObject.Find ("Camera").transform;

		if (isP1) {
			Cards.transform.eulerAngles = new Vector3 (0, 180, 0);
			tag = "P1Persona";
			otherPlayer = GameObject.FindWithTag ("playerTwo").transform;
			detector.SetTagToDetect("playerTwoHurtbox");
		} else {
			tag = "P2Persona";

			Cards.transform.eulerAngles = new Vector3 (0, 0, 0);
			otherPlayer = GameObject.FindWithTag ("playerOne").transform;
			detector.SetTagToDetect("playerOneHurtbox");
		}

		Cards.transform.position = new Vector3(Cards.transform.parent.position.x, Cards.transform.parent.position.y, -1f);

	}
	IEnumerator SendOut(){
		attackStage=-1;
		spriteRenderer.sprite = SendoutFrames [0];
		detector.gameObject.SetActive (true);
		Vector2 direction;
		if (transform.position.x > otherPlayer.position.x) {
			direction = new Vector2 (-17, 0);
			transform.position = new Vector3 (transform.position.x + 3, transform.position.y, transform.position.z);
		} else {
			direction = new Vector2 (17, 0);
			transform.position = new Vector3 (transform.position.x - 3, transform.position.y, transform.position.z);					
		}

		int counter = 0;
		for (int i = 0; i < 60; i++) {
			if (counter == 3) {
				spriteRenderer.sprite = SendoutFrames [1];
			} else if (counter == 6) {
				spriteRenderer.sprite = SendoutFrames [0];
				counter = 0;
			}
		
			counter++;
			RB.velocity = direction;
			yield return null;
		}
		StartCoroutine (TimedUnsummon ());
	}
	IEnumerator JumpMedium(){
		Vector2 direction;
		if (transform.position.x > otherPlayer.position.x) {
			direction = new Vector2 (-20, 0);
			transform.position = new Vector3 (transform.position.x + 2, transform.position.y, transform.position.z);
		} else {
			direction = new Vector2 (20, 0);
			transform.position = new Vector3 (transform.position.x - 2, transform.position.y, transform.position.z);					
		}

		int animationFrame = 0;
		for (int i = 0; i < 15; i++) {
			if (i%3 == 0) {
				spriteRenderer.sprite = jumpMediumFrames [animationFrame];
				animationFrame++;
			}
			if (i == 10) {
				mediumHitbox.SetActive (true);
			}
			if (i == 12) {
				mediumHitbox.SetActive (false);
			}
			RB.velocity = direction;
			yield return null;
		}
		RB.velocity = Vector2.zero;
		StartCoroutine (TimedUnsummon ());
	}
	IEnumerator Attack1(){
		int animationFrame = 0;
		attackStage=-1;
		spriteRenderer.sprite = Attack1Frames [0];
		for (int i = 0; i < 12;) {

			if (i%2 == 0) {
				if (animationFrame < Attack1Frames.Length) {
					spriteRenderer.sprite = Attack1Frames [animationFrame];
				}
				animationFrame++;
			}
			if (i == 10) {
				attack1Hitbox.SetActive (true);
			}
			if (i == 12) {
				attack1Hitbox.SetActive (false);
			}
			if (i == 15) {
				attackStage=2;
			}
			yield return null;
			if (!timeManager.CheckIfTimePaused()) {
				i++;
			}

		}
		StartCoroutine (Attack2 ());
		//StartCoroutine (TimedUnsummon (15));
	}

	IEnumerator Attack2(){
		int animationFrame = 0;
		Vector2 direction;
		if (transform.position.x > otherPlayer.position.x) {
			direction = new Vector2 (-10, 10);
		} else {
			direction = new Vector2 (10, 10);				
		}
		attackStage=-1;
		spriteRenderer.sprite = Attack2Frames [0];
		for (int i = 0; i < 24;) {

			if (i%2 == 0) {
				if (animationFrame < Attack2Frames.Length) {
					spriteRenderer.sprite = Attack2Frames [animationFrame];
				}
				animationFrame++;
			}
			if (i == 13) {
				attack2Hitbox.SetActive (true);
			}
			if (i == 15) {
				attack2Hitbox.SetActive (false);
			}
			if (i == 17) {

				attackStage = 3;
			}
			yield return null;
			if (!timeManager.CheckIfTimePaused()) {
				if (i < 10) {
					RB.velocity = direction;
				} else {
					RB.velocity = Vector2.zero;
				}
				if (direction.y > -5) {
					direction.y -= 3;
				}
				i++;
			}

		}
		StartCoroutine (Attack3 ());
		attackStage = 3;
		//StartCoroutine (TimedUnsummon (30));
	}
	IEnumerator Attack3(){
		transform.position = new Vector3 (transform.position.x, transform.position.y -1.2f, transform.position.z);
		int animationFrame = 0;
		Vector2 direction;
		if (transform.position.x > otherPlayer.position.x) {
			direction = new Vector2 (-10, 0);
		} else {
			direction = new Vector2 (10, 0);				
		}
		attackStage= -1;
		spriteRenderer.sprite = Attack3Frames [0];
		for (int i = 0; i < 18;) {
			
			if (i%2 == 0) {
				if (animationFrame < Attack3Frames.Length) {
					spriteRenderer.sprite = Attack3Frames [animationFrame];
				}
				animationFrame++;
			}
			if (i == 10) {
				attack3Hitbox.SetActive (true);
			}
			if (i == 12) {
				attack3Hitbox.SetActive (false);
			}
			yield return null;
			if (!timeManager.CheckIfTimePaused()) {
				if (i < 10) {
					RB.velocity = direction;
				} else {
					RB.velocity = Vector2.zero;
				}
				i++;
			}

		}
		attackStage = -1;
		StartCoroutine (TimedUnsummon (30));
	}
	IEnumerator SpecialOne(){

		attackStage = -2;
		int animationFrame = 0;
		spriteRenderer.sprite = SpecialOneFrames [0];
		for (int i = 0; i < 21;) {
			if (!timeManager.CheckIfTimePaused()) {
				if (i%3 == 0) {
					if (animationFrame < SpecialOneFrames.Length) {
						spriteRenderer.sprite = SpecialOneFrames [animationFrame];
					}
					animationFrame++;
				}
				if (i == 18) {
					if (transform.position.x > otherPlayer.position.x) {
						sp1Fireball.GetComponent<ProjectileScript>().direction = new Vector2(-1,0);
					} else {
						sp1Fireball.GetComponent<ProjectileScript>().direction = new Vector2(1,0);						
					}
					sp1Fireball.transform.position = fireballPos.position;
					sp1Fireball.gameObject.SetActive(true);
				}

				i++;
			}

			yield return null;

		}
		StartCoroutine (TimedUnsummon (30));
	}
	IEnumerator SpecialTwo(){
		attackStage = -2;
		if (transform.position.x > otherPlayer.position.x) {
			transform.position = new Vector3 (transform.position.x+1f, transform.position.y + 1f, transform.position.z);
		}else {
			transform.position = new Vector3 (transform.position.x-1f, transform.position.y + 1f, transform.position.z);

		}
		int animationFrame = 0;
		spriteRenderer.sprite = SpecialTwoFrames [0];
		for (int i = 0; i < 18;) {

			if (i%3 == 0) {

				if (animationFrame < SpecialTwoFrames.Length) {
					spriteRenderer.sprite = SpecialTwoFrames [animationFrame];
				}
				animationFrame++;
			}
			if (i == 5) {
				dpHitbox.SetActive (true);
			}
			if (i == 7) {
				dpHitbox.SetActive (false);
			}

			yield return null;
			if (!timeManager.CheckIfTimePaused()) {
				i++;
			}

		}
		StartCoroutine (TimedUnsummon (30));
	}
	IEnumerator SpecialThree(){
		attackStage = -2;
		int animationFrame = 0;
		spriteRenderer.sprite = SpecialThreeFrames [0];
		for (int i = 0; i < 42;) {

			if (i%3 == 0) {
				if (animationFrame < SpecialThreeFrames.Length) {
					spriteRenderer.sprite = SpecialThreeFrames [animationFrame];
				}
				animationFrame++;
			}

			yield return null;
			if (!timeManager.CheckIfTimePaused()) {
				if (i == 21) {
					sp3TrapPre.transform.position = trapPos.position;
					sp3TrapAttack.transform.position = trapPos.position;
					yukiAttack.SetTrap();
					sp3TrapPre.gameObject.SetActive(true);
					if (transform.position.x > otherPlayer.position.x) {
						RB.velocity =  (new Vector2(3, 0));
					} else {
						RB.velocity =  (new Vector2(-3, 0));					
					}
				}
				i++;
			}
		}
		StartCoroutine (TimedUnsummon (15));
	}
	IEnumerator Super(){
		int animationFrame = 0;
		superEffects.SetActive (true);
		for (int i = 0; i < 60;) {
			

			yield return null;
			if (!timeManager.CheckIfTimePaused()) {
				if (i > 10) {
					RB.velocity = new Vector2 (0, 10);
				}
				if (i % 5 == 0) {
					superHitbox.SetActive (false);
					superHitbox.SetActive (true);
				}
				if (i%3 == 0) {
					spriteRenderer.sprite = superFrames [animationFrame];
					animationFrame++;
					if (animationFrame >= 5) {
						animationFrame = 0;
					}
				}
				i++;
			}
		}
		StartCoroutine (TimedUnsummon (15));
	}
	IEnumerator TimedUnsummon(int delay =0){
		for (int i = 0; i < delay;) {
			
			yield return null;
			if (!timeManager.CheckIfTimePaused()) {
				i++;
			}
		}
		// turn off hitboxes
		attack1Hitbox.SetActive(false);
		attack2Hitbox.SetActive(false);
		attack3Hitbox.SetActive(false);
		mediumHitbox.SetActive (false);
		superHitbox.SetActive (false);
		dpHitbox.SetActive (false);


		isActive = false;
		for (int i = 0; i < 10;) {
			yield return null;
			if (!timeManager.CheckIfTimePaused()) {
				i++;
				spriteRenderer.color = new Color(spriteRenderer.color.r,spriteRenderer.color.g,spriteRenderer.color.b, spriteRenderer.color.a - .1f); ;

			}
		}
		gameObject.SetActive (false);
		attackStage = 0;
	}
	public void Summon(){
		StartCoroutine (SummonEffect ());

	}
	IEnumerator SummonEffect(){	

		CheckFacing ();
		yield return null;
//		spriteRenderer.color  = new Color(spriteRenderer.color.r,spriteRenderer.color.g,spriteRenderer.color.b,0);
//			
//		for (int i = 0; i < 10;) {
//			yield return null;
//			if (!timeManager.CheckIfTimePaused()) {
//				i++;
//				spriteRenderer.color  = new Color(spriteRenderer.color.r,spriteRenderer.color.g,spriteRenderer.color.b, spriteRenderer.color.a + .1f);
//			
//			}
//		}

	}
	void CheckFacing(){
		if (otherPlayer.position.x > transform.position.x) {
			transform.eulerAngles = new Vector3 (0, 0, 0);
			hitboxes.transform.eulerAngles = new Vector3 (0, 0, 0);
			//spriteRenderer.flipX = true;
		} else {
			transform.eulerAngles = new Vector3 (0, 180, 0);
			hitboxes.transform.eulerAngles = new Vector3 (0, 180, 0);
			//spriteRenderer.flipX = false;
		}
	}
	public bool CheckActive(){
		return isActive;
	}
	public bool CheckAlive(){
		return alive;
	}
	public void UnSummon(){
		StartCoroutine (TimedUnsummon ());
	}
	public int GetAttackState(){
		return attackStage;
	}

	public void DealDamage(){
		currentCards--;
		StopAllCoroutines ();
		UnSummon ();
		Cards.RemoveCard ();
		//play hit sound(glass break)
		if (currentCards <= 0) {
			alive = false;
			Invoke ("ReActivate", 7);
		}
	}
	void ReActivate(){
		currentCards = maxCards;
		Cards.ResetCards ();
		alive = true;
	}

	public void StartAttacksAnim(){
		Debug.Log("hi");
		if (alive && attackStage != -2) {
			if (attackStage != -1) {
				EndAnimations ();
			}
			if (!isActive) {
				attackStage = 0;
			}
			CheckFacing ();
			switch (attackStage) {
			case 0: 
				StartCoroutine (SendOut ());
				break;
			case 1: 
				StartCoroutine (Attack1 ());
				break;
			case 2: 
				StartCoroutine (Attack2 ());
				break;
			case 3: 
				StartCoroutine (Attack3 ());
				break;
			}

			isActive = true;
		}
	}
	public void SendoutPersona(){
		EndAnimations ();
		isActive = true;
		StartCoroutine (SendOut ());
	}
	public void StartAttacksChain(){
		EndAnimations ();
		StartCoroutine (Attack1 ());
	}
	public void StartJumpMediumAnim(){
		if (alive) {
			EndAnimations ();
			CheckFacing ();
			isActive = true;
			StartCoroutine (JumpMedium ());
		}
	}
		public void StartSpecialOneAnim(){
		if (alive && attackStage != -2) {
			EndAnimations ();
			CheckFacing ();
			isActive = true;
			StartCoroutine (SpecialOne ());
		}
	}
	public void StartSpecialTwoAnim(){
		if (alive&& attackStage != -2) {
			EndAnimations ();
			CheckFacing ();
			isActive = true;
			StartCoroutine (SpecialTwo ());
		}
	}
	public void StartSpecialThreeAnim(){
		if (alive&& attackStage != -2) {
			EndAnimations ();
			CheckFacing ();
			isActive = true;
			StartCoroutine (SpecialThree ());
		}
	}
	public void StartSuperAnim(){
		if (alive) {
			EndAnimations ();
			CheckFacing ();
			isActive = true;
			StartCoroutine (Super ());
		}
	}
	public void EndAnimations(){
		RB.velocity = Vector2.zero;
		attack1Hitbox.SetActive (false);
		attack2Hitbox.SetActive (false);
		attack3Hitbox.SetActive (false);
		StopAllCoroutines ();
	}
}
