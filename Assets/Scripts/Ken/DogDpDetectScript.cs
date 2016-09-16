using UnityEngine;
using System.Collections;

public class DogDpDetectScript : MonoBehaviour {
	public DogAttackScript dogAttack;
	public string TagToDetect;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public void OnTriggerEnter2D(Collider2D other){
		if (other.tag == TagToDetect) {
			dogAttack.StartSp2Complete ();
		}
	}
}
