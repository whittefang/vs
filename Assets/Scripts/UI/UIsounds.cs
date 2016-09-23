using UnityEngine;
using System.Collections;

public class UIsounds : MonoBehaviour {

	AudioSource AS;
	public AudioClip change, confirm, back, loadSound; 
	public AudioClip[] selectSounds;
	// Use this for initialization
	void Start () {
		AS = GetComponent<AudioSource> ();
	}
	public void PlayChange(){
		AS.PlayOneShot (change);
	}
	public void PlayConfirm(){
		AS.PlayOneShot (confirm);
	}
	public void PlayBack(){
		AS.PlayOneShot (back);
	}
	public void PlayLoad(){
		AS.PlayOneShot (loadSound);
	}
	public void PlayCharacterSelect(int character){
		AS.PlayOneShot (selectSounds [character]);
	}
}
