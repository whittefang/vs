using UnityEngine;
using System.Collections;

public class playerDetectionScript : MonoBehaviour {
	PersonaAttackAnimScript persona;
	public string tagToDetect = "empty";
	// Use this for initialization
	void Awake () {
		persona = GetComponentInParent<PersonaAttackAnimScript> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public void SetTagToDetect(string newTag){
		tagToDetect = newTag;
	}
	void OnTriggerEnter2D(Collider2D other){
		if (other.tag == tagToDetect) {
			persona.StartAttacksChain ();
			gameObject.SetActive (false);
		}
	}
}
