using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneTransistionScript : MonoBehaviour {

	public int sceneToTransitionTo;
	int[] playerScenes;
	bool player1Ready = false, player2Ready = false, levelSelect = false;
	// Use this for initialization
	void Start () {
		playerScenes = new int[2];
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
			if (levelSelect) {
				Debug.Log (playerScenes [0] + "  " + playerScenes [1]);
				sceneToTransitionTo = playerScenes[Random.Range(0,2)];
				Debug.Log (sceneToTransitionTo);
			} 
				LoadScene (2, sceneToTransitionTo);
			
			return true;
		} else {
			return false;
		}
	}
	public void LoadScene(float delay ,int newScene){
		StartCoroutine (LoadSceneDelay( delay, newScene));
	}
	public void SetScene(bool isPlayerOne, int scene){
		if (isPlayerOne) {
			playerScenes[0] = scene;
		} else {
			playerScenes[1] = scene;
		}
		levelSelect = true;
	}
	IEnumerator LoadSceneDelay(float delay ,int newScene){
		yield return new WaitForSeconds (delay);
		SceneManager.LoadScene (newScene);
	}
}
