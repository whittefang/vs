using UnityEngine;
using System.Collections;

public class paralaxScript : MonoBehaviour {
	public GameObject[] levelLayers;
	public float[] percentageToMove;
	GameObject mainCamera;
	// Use this for initialization
	void Start () {
		mainCamera = GameObject.Find ("Camera");
	}
	
	// Update is called once per frame
	void Update () {
		for(int i = 0; i < percentageToMove.Length; i++){
			levelLayers[i].transform.position = new Vector3 (mainCamera.transform.position.x * percentageToMove[i], levelLayers[i].transform.position.y, levelLayers[i].transform.position.z);
		}
	}
}
