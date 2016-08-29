using UnityEngine;
using System.Collections;

public class SpriteAnimator : MonoBehaviour {
	public delegate void AnimationDelegate();
	public delegate void AnimationDelegateInt(int x);
	AnimationDelegate walkAnim, walkAwayAnim, stopAnimations, neutralAnim, jumpTowardAnim, jumpAwayAnim, jumpNeutralAnim,
					  lightAnim, mediumAnim, heavyAnim, jumpLightAnim, jumpMediumAnim, jumpHeavyAnim, specialOneAnim,
					  specialTwoAnim,specialThreeAnim, throwTryAnim,throwCompleteAnim, superAnim,  blockAnim, winAnimation,
					  landingAnimation, extraAnim1,extraAnim2,extraAnim3,extraAnim4;
	AnimationDelegateInt hitAnim;
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
			if (neutralAnim != null) {
				neutralAnim ();
			}
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
	public void PlayJumpLight(){
		currentState = "jumpLight";
		jumpLightAnim ();
	}
	public void PlayJumpMedium(){
		currentState = "jumpMedium";
		jumpMediumAnim ();
	}
	public void PlayJumpHeavy(){
		currentState = "jumpHeavy";
		jumpHeavyAnim ();
	}
	public void PlaySpecialOne(){
		currentState = "special one";
		specialOneAnim ();
	}
	public void PlaySpecialTwo(){
		currentState = "special two";
		specialTwoAnim ();
	}
	public void PlaySpecialThree(){
		currentState = "special three";
		specialThreeAnim ();
	}
	public void PlayThrowTry(){
		currentState = "throw";
		throwTryAnim ();
	}
	public void PlayThrowComplete(){
		currentState = "throw";
		throwCompleteAnim ();
	}
	public void PlaySuper(){
		currentState = "super";
		superAnim ();
	}
	public void PlayHit(int duration){
		currentState = "hitstun";
		hitAnim(duration);
	}
	public void PlayBlock(){
		currentState = "Blockstun";
		blockAnim();
	}
	public void PlayWin(){
		currentState = "win";
		winAnimation();
	}
	public void PlayLanding(){
		landingAnimation();
	}
	public void PlayExtra1(){
		extraAnim1();
	}
	public void PlayExtra2(){
		extraAnim2();
	}
	public void PlayExtra3(){
		extraAnim3();
	}
	public void PlayExtra4(){
		extraAnim4();
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
		PlayNeutralAnim ();
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
	public void SetJumpLightAnimation(AnimationDelegate newAnim){
		jumpLightAnim = newAnim;
	}
	public void SetJumpMediumAnimation(AnimationDelegate newAnim){
		jumpMediumAnim = newAnim;
	}
	public void SetJumpHeavyAnimation(AnimationDelegate newAnim){
		jumpHeavyAnim = newAnim;
	}
	public void SetSpecialOneAnimation(AnimationDelegate newAnim){
		specialOneAnim = newAnim;
	}
	public void SetSpecialTwoAnimation(AnimationDelegate newAnim){
		specialTwoAnim = newAnim;
	}
	public void SetSpecialThreeAnimation(AnimationDelegate newAnim){
		specialThreeAnim = newAnim;
	}
	public void SetSuperAnimation(AnimationDelegate newAnim){
		superAnim = newAnim;
	}
	public void SetHitAnimation(AnimationDelegateInt newAnim){
		hitAnim = newAnim;
	}
	public void SetBlockAnimation(AnimationDelegate newAnim){
		blockAnim = newAnim;
	}
	public void SetThrowTryAnimation(AnimationDelegate newAnim){
		throwTryAnim = newAnim;
	}
	public void SetThrowCompleteAnimation(AnimationDelegate newAnim){
		throwCompleteAnim = newAnim;
	}
	public void setWinAnimation(AnimationDelegate newAnim){
		winAnimation = newAnim;	
	}
	public void setLandingAnimation(AnimationDelegate newAnim){
		landingAnimation = newAnim;
	}
	public void setExtra2Animation(AnimationDelegate newAnim){
		extraAnim2 = newAnim;
	}
	public void setExtra1Animation(AnimationDelegate newAnim){
		extraAnim1 = newAnim;
	}
	public void setExtra3Animation(AnimationDelegate newAnim){
		extraAnim3 = newAnim;
	}
	public void setExtra4Animation(AnimationDelegate newAnim){
		extraAnim4 = newAnim;
	}
}
