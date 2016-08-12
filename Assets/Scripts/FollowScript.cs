using UnityEngine;
using System.Collections;

public class FollowScript : MonoBehaviour {
	public GameObject transformToFollow;
	public bool followY = true;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (followY) {
			transform.position = transformToFollow.transform.position;
		} else {
			transform.position = new Vector3(transformToFollow.transform.position.x , transform.position.y, transform.position.z);
		}
	}
}
