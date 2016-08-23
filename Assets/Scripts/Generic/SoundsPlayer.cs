using UnityEngine;
using System.Collections;

public class SoundsPlayer : MonoBehaviour {
	AudioSource AS;
	public AudioClip SP1, SP2, SP3, lighta, medium, heavy, extra, block, throwHit, superBg, superWord, death; 
	public AudioClip[] hits;
	// Use this for initialization
	void Start () {
		AS = GameObject.Find ("MasterGameObject").GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public void PlaySP1(){
		AS.PlayOneShot (SP1);
	}
	public void PlaySP2(){
		AS.PlayOneShot (SP2);
	}
	public void PlaySP3(){
		AS.PlayOneShot (SP3);
	}
	public void PlayLight(){
		AS.PlayOneShot (lighta);
	}
	public void PlayMedium(){
		AS.PlayOneShot (medium);
	}
	public void PlayHeavy(){
		AS.PlayOneShot (heavy);
	}
	public void PlayExtra(){
		AS.PlayOneShot (extra);
	}
	public void PlayHit(){
		int clipToPlay =  Random.Range (0, hits.Length);
		AS.PlayOneShot (hits [clipToPlay]);
	}
	public void PlayBlock(){
		AS.PlayOneShot(block);
	}
	public void PlayThrowHit(){
		AS.PlayOneShot (throwHit);
	}
	public void PlaySuperBg(){
		AS.PlayOneShot (superBg);
	}
	public void PlaySuperWord(){
		AS.PlayOneShot (superWord);
	}
	public void PlayDeath(){
		AS.PlayOneShot (death);
	}
}
