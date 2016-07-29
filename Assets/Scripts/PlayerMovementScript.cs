using UnityEngine;
using System.Collections;

public class PlayerMovementScript : MonoBehaviour {
	Rigidbody2D RB;
	public float speedMax = 0;
	float currentSpeed = 0;
	InputScript IS;
	float deadSize = .15f;
	bool canMove = true;
	// Use this for initialization
	void Awake () {
		IS = GetComponent<InputScript> ();
		RB = GetComponent<Rigidbody2D> ();
		IS.SetThumbstick (ProcessMovement);
		ResetSpeed ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public void ProcessMovement(float x,  float y){
		if (canMove) {
			if (x < deadSize && x > -deadSize) {
				x = 0;
			}
			if (y < deadSize && y > -deadSize) {
				y = 0;
			}
			RB.velocity = new Vector2 (x * currentSpeed, y * currentSpeed);
		}
	}
	public void SetSpeed(float newSpeed = 0){
		currentSpeed = newSpeed;
	}
	public void ResetSpeed(){
		currentSpeed = speedMax;
	}
	public void Charge(float speed,Vector2 direction, float duration){
		StartCoroutine (ChargeNumer (speed, direction, duration));
	}
	IEnumerator ChargeNumer(float speed, Vector2 direction, float duration){
		canMove = false;
		RB.velocity = direction * speed;
		while (duration > 0){
			yield return null;
			duration -= .0167f;

		}
		canMove = true;
		
	}
}
