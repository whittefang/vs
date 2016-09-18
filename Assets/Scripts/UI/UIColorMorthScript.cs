using UnityEngine;
using System.Collections;

public class UIColorMorthScript : MonoBehaviour {
	public Color startColor, endColor;
	SpriteRenderer SR;
	int colorChangeIterations = 15;
	int currentIterations = 0;
	bool up = true;
	// Use this for initialization
	void Start () {
		SR = GetComponent < SpriteRenderer> ();
		SR.color = startColor;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (up) {
			SR.color = Color.Lerp (SR.color, endColor, .05f);
			if (currentIterations > colorChangeIterations) {
				up = false;
			}
			currentIterations++;
		} else {
			SR.color = Color.Lerp (SR.color, startColor, .05f);
			if (currentIterations < 0) {
				up = true;
			}
			currentIterations--;
		}
	}

}
