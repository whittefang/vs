using UnityEngine;
using System.Collections;

public class HealthBarAnim : MonoBehaviour {

	float maxHpR = 200;
	float newestHpR = 200;
	float currentLengthR = 3.85f;
	public float maxLengthR = 8;
	Renderer renderer;
	// Use this for initialization
	void Start () {
		renderer  = GetComponent<SpriteRenderer> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public void setHp (int hp){
		maxHpR = hp;
		newestHpR = hp;
	}
	public void DealDamage(int newAmount){
		StartCoroutine (SlowDrain (newAmount));
	}
	IEnumerator SlowDrain(int newAmount){
		int damageDelt = (int)newestHpR - newAmount;
		for (int x = 0; x < 30; x++) {
			changeBar (damageDelt / 30);
			yield return null;
		}changeBar ((int)newestHpR - newAmount);
	}
	void changeBar (int damageInt){
		float damageR = damageInt;
		float originalLeftSpot = renderer.bounds.min.x;
		float floatChange = 0;

		floatChange = (newestHpR/maxHpR) - ((newestHpR - damageR) / maxHpR);
		newestHpR = newestHpR - damageR; 


		if (newestHpR <= 0){
			newestHpR = 0;
			transform.localScale = new Vector3 (0f, 0f, 0f);
		}else{
			floatChange = maxLengthR * floatChange;
			currentLengthR = currentLengthR - floatChange;
			transform.localScale = new Vector3 (currentLengthR, transform.localScale.y, transform.localScale.z);
			float newLeftSpot = renderer.bounds.min.x;
			float findNewLeftSpot = newLeftSpot - originalLeftSpot;
			transform.Translate(new Vector3 (-findNewLeftSpot, 0f, 0f));
		}

	}
}
