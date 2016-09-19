using UnityEngine;
using System.Collections;

public class ExMeter : MonoBehaviour {
	
	float currentExLeft;
	float currentExRight;
	public float currentLengthLeft = 0;
	public float currentLengthRight = 0;
	public Renderer exLeftRenderer;
	public Renderer exRightRenderer;
 	float originalLeftSpot;
 	float originalRightSpot;
 	float maxBarLength = 1.97f;
 	public GameObject LeftAnimation, RightAnimation;

	void Start () {
		LeftAnimation.SetActive(false);
		RightAnimation.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown ("e")) {
			ExMeterChange (20, true);
			//changeBar (damage, LeftHpBar);
			//changeBar2(damage, maxHpBar, LeftHpBar);
		}
		if (Input.GetKeyDown ("r")) {
			ExMeterChange (20, false);
			//changeBar (damage, LeftHpBar);
			//changeBar2(damage, maxHpBar, LeftHpBar);
		}
	
	}

	public void setExMetersToZero(){
		currentExLeft = 0;
		currentExRight = 0;
		ExMeterChange(0,true);
		ExMeterChange(0,false);
	}
	//returns the ammount as a float for start of eatch round to dalts script.
	public float GetEx(bool isP1){
		if (isP1){
			return (currentExLeft);
		}
		else{
			return (currentExRight);
		}
	}
	/*
	public void ZeroExBar(bool isP1){
		if (isP1){
			currentExLeft = 0;
			ExMeterChange(0,true);
		}
		else{
			currentExRight = 0;
			ExMeterChange(0, false);
		}

	*/

	public void ExMeterChange(int meter, bool isPlayerOne){
		originalLeftSpot = exLeftRenderer.bounds.max.x;
		originalRightSpot = exRightRenderer.bounds.min.x;

		if (isPlayerOne){
				currentExLeft += meter;
			if (currentExLeft >= 1000){
				currentExLeft = 1000;
				LeftAnimation.SetActive(true);
			}
			else{
				LeftAnimation.SetActive(false);
			}
			exLeftRenderer.transform.localScale = new Vector3(currentExLeft / 1000 * maxBarLength, exLeftRenderer.transform.localScale.y, exLeftRenderer.transform.localScale.z);
			float newLeftSpot = exLeftRenderer.bounds.max.x;
			float findNewLeftSpot = newLeftSpot - originalLeftSpot;
			exLeftRenderer.transform.Translate(new Vector3 (-findNewLeftSpot, 0f, 0f),Space.Self);

		}
		else {
				currentExRight += meter;
			if (currentExRight >= 1000){
				currentExRight = 1000;
				RightAnimation.SetActive(true);
			}
			else{
				RightAnimation.SetActive(false);
			}
			exRightRenderer.transform.localScale = new Vector3(currentExRight / 1000 * maxBarLength, exLeftRenderer.transform.localScale.y, exLeftRenderer.transform.localScale.z);
			float newRightSpot = exRightRenderer.bounds.min.x;
			float findNewRightSpot = newRightSpot - originalRightSpot;
			exRightRenderer.transform.Translate(new Vector3 (-findNewRightSpot, 0f, 0f), Space.Self);
		}
	}
}
