using UnityEngine;
using System.Collections;

public class ProjectileScript : MonoBehaviour {
	public Vector2 direction;
	public float speed = .1f;
	float savedSpeed =0;
	public bool movementEnabled = true;
	public float lifeDuration = 0;
	public bool useLimitedLife = false, enableFireballKiller = true, flyOffOnDeath = false;
	public int projecttileStrength = 1, projectileOwner = 0;
	public  GameObject bodyToTurnOff;
	public BoxCollider2D[] hitboxsToTurnOff;
	TimeManagerScript timeManager;

	void OnEnable(){
		if (timeManager == null) {
			timeManager = GameObject.Find ("MasterGameObject").GetComponent<TimeManagerScript> ();
		}
		if (useLimitedLife) {
			if (bodyToTurnOff == null){
				bodyToTurnOff = this.gameObject;
			}
			Invoke("TurnOffSelf", lifeDuration);
		}
		speed = savedSpeed;
		movementEnabled = true;
		foreach (BoxCollider2D b in hitboxsToTurnOff) {
			b.enabled = true;
		}
	}
	void Awake(){
		savedSpeed = speed;
	}
	void OnDisable(){
		CancelInvoke ();
	}

	public bool MovementEnabled{
		get{
			return movementEnabled;
		}
		set{
			movementEnabled = value;
		}
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (MovementEnabled) {
			Move ();
		}
	}
	void Move(){
		if (!timeManager.CheckIfTimePaused ()) {
			transform.position += (Vector3)direction * speed;
		}
			
	}
	public void SetDirection(Vector2 newDirection){
		direction = newDirection;
	}
	public void SetSpeed(float newSpeed){
		speed = newSpeed;
	}
	void TurnOffSelf(){
		Kill ();
	}
	void OnTriggerEnter2D(Collider2D other){
		if (other.tag == "fireballKiller" && enableFireballKiller){
			gameObject.SetActive (false);
		}else if (other.tag == "projectile" && projectileOwner != other.GetComponent<ProjectileScript>().projectileOwner){
			int otherStrength = other.GetComponent<ProjectileScript> ().projecttileStrength;
			if (projecttileStrength > otherStrength) {
				other.GetComponent<ProjectileScript> ().Kill ();
			} else if (projecttileStrength == otherStrength) {

				other.GetComponent<ProjectileScript> ().Kill ();
				Kill ();
			} else {
				Kill ();
			}
		}
	}
	public void Kill(){
		movementEnabled = false;
		foreach (BoxCollider2D b in hitboxsToTurnOff) {
			b.enabled = false;
		}
		if (GetComponentInChildren<AnimationLoopScript> () != null) {
			GetComponentInChildren<AnimationLoopScript> ().StopAnimation ();
		}
		if (flyOffOnDeath) {
			movementEnabled = true;
			SetSpeed (.1f);
			SetDirection (new Vector2 (Random.Range (-1f, 1f), -1f));
		}

		GetComponentInChildren<AnimateOnce> ().Animate ();
	}
}
