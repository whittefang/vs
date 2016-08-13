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
		if (followY && transformToFollow != null) {
			transform.position = transformToFollow.transform.position;
		} else if ( transformToFollow != null){
			transform.position = new Vector3(transformToFollow.transform.position.x , transform.position.y, transform.position.z);
		}
	}
}
