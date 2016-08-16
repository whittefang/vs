using UnityEngine;
using System.Collections;

public class UIsounds : MonoBehaviour {

	AudioSource AS;
	public AudioClip change, confirm, back; 
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
}
