using UnityEngine;
using System.Collections;

public class ProjectileScript : MonoBehaviour {
	public Vector2 direction;
	public float speed = .1f;
	bool movementEnabled = true;
	public float lifeDuration = 0;
	public bool useLimitedLife = false;
	public  GameObject bodyToTurnOff;
	void OnEnable(){
		if (useLimitedLife) {
			if (bodyToTurnOff == null){
				bodyToTurnOff = this.gameObject;
			}
			Invoke("TurnOffSelf", lifeDuration);
		}
	}
	void OnDisable(){
		CancelInvoke ();
	}
	void TurnOffSelf(){
		bodyToTurnOff.SetActive (false);
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
		transform.position += (Vector3)direction * speed;
	}
	public void SetDirection(Vector2 newDirection){
		direction = newDirection;
	}
	public void SetSpeed(float newSpeed){
		speed = newSpeed;
	}
}
