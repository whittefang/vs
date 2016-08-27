using UnityEngine;
using System.Collections;

public class RandomMusic : MonoBehaviour {
	public AudioClip[] songs;
	AudioSource AS;
	// Use this for initialization
	void Start () {
		AS = GetComponent<AudioSource> ();
		AS.clip = songs[Random.Range (0, songs.Length)];
		AS.Play ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
