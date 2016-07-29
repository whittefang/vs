using UnityEngine;
using System.Collections;

public class HealthScript : MonoBehaviour {
	public int healthAmount;
	public int healthMax = 100;
	public delegate void DeathEvent();
	DeathEvent DeathFunc;
	// Use this for initialization
	void OnEnable () {
		healthAmount = healthMax ;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public void DealDamage(int amount = 1){
		healthAmount -= amount;
		CheckHealth ();
	}
	public void CheckHealth(){
		if (healthAmount <= 0) {
			if (DeathFunc != null) {
				DeathFunc ();
			}
			gameObject.SetActive (false);
		}
	}
}
