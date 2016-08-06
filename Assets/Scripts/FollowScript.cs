using UnityEngine;
using System.Collections;

public class FollowScript : MonoBehaviour {
	public GameObject transformToFollow;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = transformToFollow.transform.position;
	}
}
