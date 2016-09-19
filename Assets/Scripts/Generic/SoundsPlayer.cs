using UnityEngine;
using System.Collections;

public class SoundsPlayer : MonoBehaviour {
	AudioSource AS;
	public AudioClip SP1, SP2, SP3, lightA, medium, heavy, extra, extra2, extra3, block, throwHit, superBg, superWord, death; 
	public AudioClip[] hits;
	// Use this for initialization
	void Start () {
		AS = GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public void PlaySP1(){
		AS.pitch = 1;
		AS.PlayOneShot (SP1);
	}
	public void PlaySP2(){
		AS.pitch = 1;
		AS.PlayOneShot (SP2);
	}
	public void PlaySP3(){
		AS.pitch = 1;
		AS.PlayOneShot (SP3);
	}
	public void PlayLight(){
		if (RandomChance (50)) {
			AS.pitch = 1;
			AS.PlayOneShot (lightA);
		}
	}
	public void PlayMedium(){
		if (RandomChance (70)) {
			AS.pitch = 1;
			AS.PlayOneShot (medium);
		}
	}
	public void PlayHeavy(){
		if (RandomChance (85)) {
			AS.pitch = 1;
			AS.PlayOneShot (heavy);
		}
	}
	public void PlayExtra(){
		AS.pitch = 1;
		AS.PlayOneShot (extra);
	}
	public void PlayExtra2(){
		AS.pitch = 1;
		AS.PlayOneShot (extra2);
	}
	public void PlayExtra3(){
		AS.pitch = 1;
		AS.PlayOneShot (extra3);
	}
	public void PlayHit(AudioClip sound, float pitch =1){
		AS.pitch = pitch;
		//int clipToPlay =  Random.Range (0, hits.Length);
		AS.PlayOneShot (sound);
	}
	public void PlayRandomHit(){
		AS.pitch = 1;
		AS.PlayOneShot (hits[Random.Range (0, hits.Length)]);
	}
	public void PlayBlock(float pitch =1){
		AS.pitch = pitch;
		AS.PlayOneShot(block);
	}
	public void PlayThrowHit(){
		AS.pitch = 1;
		AS.PlayOneShot (throwHit);
	}
	public void PlaySuperBg(){
		AS.pitch = 1;
		AS.PlayOneShot (superBg);
	}
	public void PlaySuperWord(){
		AS.pitch = 1;
		AS.PlayOneShot (superWord);
	}
	public void PlayDeath(){
		AS.pitch = 1;
		AS.PlayOneShot (death);
	}
	bool RandomChance(int chance = 100){
		if (Random.Range (0, 100) < chance) {
			return true;
		} else {
			return false;
		}
	}
}
