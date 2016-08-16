using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneTransistionScript : MonoBehaviour {

	public int sceneToTransitionTo;
	bool player1Ready = false, player2Ready = false;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	// returns true if both players are ready and the scene will be loaded, false if only 1 player is ready
	public bool SetReady(bool isP1, bool ready){
		if (isP1) {
			player1Ready = ready;
		} else {
			player2Ready = ready;
		}

		if (player1Ready && player2Ready) {
			LoadScene (2, sceneToTransitionTo);
			return true;
		} else {
			return false;
		}
	}
	public void LoadScene(float delay ,int newScene){
		StartCoroutine (LoadSceneDelay( delay, newScene));
	}
	IEnumerator LoadSceneDelay(float delay ,int newScene){
		yield return new WaitForSeconds (delay);
		SceneManager.LoadScene (newScene);
	}
}
