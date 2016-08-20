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

	void Start () {
		originalLeftSpot = exLeftRenderer.bounds.min.x;
		originalRightSpot = exRightRenderer.bounds.max.x;
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

	public void ExMeterChange(int meter, bool isPlayerOne){
		float tempMeter = meter;

		if (isPlayerOne){
				currentExLeft += meter;
			if (currentExLeft > 1000){
				currentExLeft = 1000;
			}
			exLeftRenderer.transform.localScale = new Vector3(currentExLeft / 1000 * 4, exLeftRenderer.transform.localScale.y, exLeftRenderer.transform.localScale.z);
			float newLeftSpot = exLeftRenderer.bounds.min.x;
			float findNewLeftSpot = newLeftSpot - originalLeftSpot;
			exLeftRenderer.transform.Translate(new Vector3 (-findNewLeftSpot, 0f, 0f));

		}
		else {
				currentExRight += meter;
			if (currentExRight > 1000){
				currentExRight = 1000;
			}
			exRightRenderer.transform.localScale = new Vector3(currentExRight / 1000 * 4, exLeftRenderer.transform.localScale.y, exLeftRenderer.transform.localScale.z);
			float newRightSpot = exRightRenderer.bounds.max.x;
			float findNewRightSpot = newRightSpot - originalRightSpot;
			exRightRenderer.transform.Translate(new Vector3 (-findNewRightSpot, 0f, 0f));
		}







	}
}
