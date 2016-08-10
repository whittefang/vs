using UnityEngine;
using System.Collections;

public class CameraMoveScript : MonoBehaviour {
	Transform p1, p2;
	public float LeftBound, rightBound, lowPoint, HighPoint;
	// Use this for initialization
	void Start () {
		p1 = GameObject.FindGameObjectWithTag ("playerOne").transform;
		p2 = GameObject.FindGameObjectWithTag ("playerTwo").transform;

	}
	
	// Update is called once per frame
	void FixedUpdate () {
		Vector3 newPosition = new Vector3 (((p1.position.x + p2.position.x) / 2), ((p1.position.y + p2.position.y) / 2), -100);
		if (newPosition.x < LeftBound) {
			newPosition.x = LeftBound;
		}
		if (newPosition.x > rightBound) {
			newPosition.x = rightBound;
		}
		if (newPosition.y > HighPoint) {
			newPosition.y = HighPoint;
		}
		if (newPosition.y < lowPoint) {
			newPosition.y = lowPoint;
		}
		transform.position = newPosition;

	}
}
