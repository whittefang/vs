using UnityEngine;
using System.Collections;

public class SimpleMoveScript : MonoBehaviour {
	public Vector2 direction;
	public float speed = .1f;
	public bool limitLife = false;
	public float lifeTime = 0;
	// Use this for initialization
	void Start () {
	
	}
	void OnEnable(){
		if (limitLife) {
			Invoke ("Disable", lifeTime);
		}

	}
	void Disable(){
		gameObject.SetActive (false);
	}
	// Update is called once per frame
	void FixedUpdate () {
		transform.position += (Vector3)direction * speed;
			
	}
}
