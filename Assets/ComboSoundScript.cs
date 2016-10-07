using UnityEngine;
using System.Collections;

public class ComboSoundScript : MonoBehaviour {

	public AudioClip[] comboSounds; 
	AudioSource AS;
	// Use this for initialization
	void Start () {
		AS = GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public void playComboNoise(int amount){
		int SoundToPlay = 0;
		Debug.Log (amount);

		if (amount == 3) {
			SoundToPlay = 0;
		}else if (amount == 4) {
			SoundToPlay = 1;
		}else if (amount == 5) {
			SoundToPlay = 2;
		}else if (amount == 6) {
			SoundToPlay = 3;
		}else if (amount >=30) {
			SoundToPlay = 6;
		}else if (amount >= 10) {
			SoundToPlay = 5;
		}else if (amount >= 7) {
			SoundToPlay = 4;
		}

		Debug.Log (SoundToPlay);
		if (amount >= 3) {
			AS.PlayOneShot (comboSounds [SoundToPlay]);
		}
	}
}
