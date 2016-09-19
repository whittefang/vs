using UnityEngine;
using System.Collections;

public class CameraMoveScript : MonoBehaviour {
	public Transform p1, p2;
	public float LeftBound, rightBound, lowPoint, HighPoint;
	bool playersAreSet = false, cameraEnabled = true;
	public bool testMode = false;
	CameraWallScript cameraWallScript;
	Camera thisCamera;
	// Use this for initialization
	void Start (){
		thisCamera = GetComponent<Camera> ();
		cameraWallScript = GetComponentInChildren<CameraWallScript>();
		cameraWallScript.transform.parent = null;
		if (testMode) {
			SetPlayers (p1.gameObject, p2.gameObject);
		}
	}
	// Update is called once per frame
	void FixedUpdate () {
		if (playersAreSet && cameraEnabled){
			transform.position = FindLegalPositionBetweenPlayers();
		}


	}
	Vector3 FindLegalPositionBetweenPlayers(){
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

		if (p1.position.x < newPosition.x - 4.85f || p1.position.x > newPosition.x + 4.85f ||
		    p2.position.x < newPosition.x - 4.85f || p2.position.x > newPosition.x + 4.85f) {
			cameraWallScript.StopFollow ();
		} else {
			cameraWallScript.EngangeFollow ();
		}
		return newPosition;
	}
	public void SetPlayers(GameObject playerOne, GameObject playerTwo){
		p1 = playerOne.transform.GetChild(0).transform;
		p2 = playerTwo.transform.GetChild(0).transform;
		playersAreSet = true;
	}
	public void EnableCameraMovement(bool enabled){
		cameraEnabled = enabled;
	}
	public void FocusForSuper(Vector3 objectToFocus, int timeToHoldView){
		StartCoroutine (FocusForSuperEnum(objectToFocus, timeToHoldView));
	}
	IEnumerator FocusForSuperEnum(Vector3 objectToFocus, int timeToHoldView){
		cameraEnabled = false;
		cameraWallScript.StopFollow ();
		while (Mathf.Abs (transform.position.x - objectToFocus.x) > .1f) {
			Vector3 newPos = Vector2.Lerp (transform.position, objectToFocus, .1f);
			newPos.z = transform.position.z;
			transform.position = newPos;
			if (thisCamera.orthographicSize > 3) {
				thisCamera.orthographicSize -= .05f;
			}
			yield return null;
			Debug.Log ("move in");
		}

		for (int x = 0; x < timeToHoldView; x++) {
			yield return null;
		}

		while (Mathf.Abs (transform.position.x - FindLegalPositionBetweenPlayers().x) > .1f) {
			transform.position = Vector3.Lerp(transform.position, FindLegalPositionBetweenPlayers(), .1f);
			if (thisCamera.orthographicSize < 3.75) {
				thisCamera.orthographicSize += .05f;
			}
			yield return null;
			Debug.Log ("move out");
		}
		thisCamera.orthographicSize = 3.75f;
		Debug.Log ("done");
		cameraWallScript.EngangeFollow();
		cameraEnabled = true;
	}
}
