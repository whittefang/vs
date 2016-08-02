using UnityEngine;
using System.Collections;

public class SpriteAnimator : MonoBehaviour {
	public delegate void AnimationDelegate();
	AnimationDelegate walkAnim, walkAwayAnim, stopAnimations, neutralAnim, jumpTowardAnim, jumpAwayAnim, jumpNeutralAnim,
						lightAnim, mediumAnim, heavyAnim;
	public string currentState = "blank";
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	//------------------------------
	// animation Play functions
	//------------------------------
	public void PlayWalkAnim(){
		if (currentState != "walk towards") {
			walkAnim ();
			currentState = "walk towards";
		}
	}
	public void PlayNeutralAnim(){
		if (currentState != "neutral") {
			neutralAnim ();
			currentState = "neutral";
		}
	}
	public void PlayWalkAwayAnim(){
		if (currentState != "walk away") {
			walkAwayAnim ();
			currentState = "walk away";
		}
	}
	public void PlayJumpToward(){
		currentState = "jump towards";
		jumpTowardAnim ();
	}
	public void PlayJumpAway(){
		currentState = "jump away";
		jumpAwayAnim ();
	}
	public void PlayJumpNeutral(){
		currentState = "jump neutral";
		jumpNeutralAnim ();
	}
	public void PlayLight(){
		currentState = "light";
		lightAnim();
	}
	public void PlayMedium(){
		currentState = "medium";
		mediumAnim ();
	}
	public void PlayHeavy(){
		currentState = "heavy";
		heavyAnim ();
	}
	public void StopAnimations(){
		stopAnimations ();
	}
	//------------------------------
	//animation set functions
	//------------------------------
	public void SetJumpTowardsAnim(AnimationDelegate newAnim){
		jumpTowardAnim = newAnim;
	}
	public void SetJumpNeutralAnim(AnimationDelegate newAnim){
		jumpNeutralAnim = newAnim;
	}
	public void SetJumpAwayAnim(AnimationDelegate newAnim){
		jumpAwayAnim = newAnim;
	}
	public void SetWalkAnim(AnimationDelegate newAnim){
		walkAnim = newAnim;
	}
	public void SetWalkAwayAnim(AnimationDelegate newAnim){
		walkAwayAnim = newAnim;
	}
	public void SetStopAnimations(AnimationDelegate newAnim){
		stopAnimations = newAnim;
	}
	public void SetNeutralAnimation(AnimationDelegate newAnim){
		neutralAnim = newAnim;
	}
	public void SetLightAnimation(AnimationDelegate newAnim){
		lightAnim = newAnim;
	}
	public void SetMediumAnimation(AnimationDelegate newAnim){
		mediumAnim = newAnim;
	}
	public void SetHeavyAnimation(AnimationDelegate newAnim){
		heavyAnim = newAnim;
	}
}
