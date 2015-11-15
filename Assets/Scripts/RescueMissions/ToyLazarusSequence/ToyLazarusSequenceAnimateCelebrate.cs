using UnityEngine;
using System.Collections;

public class ToyLazarusSequenceAnimateCelebrate : MonoBehaviour 
{
	//*************************************************************//
	private Vector3 _initialPosition;
	private int _countJumping;
	private int _maxJumps;
	private float _slwoFactor;
	//*************************************************************//
	void Awake () 
	{
		_maxJumps = Random.Range ( 2, 6 );
		_slwoFactor = Random.Range ( 1f, 1.5f );
		GlobalVariables.CHARACTER_CELEBRATING = true;
		transform.Find ( "tile" ).GetComponent < CharacterAnimationControl > ().playAnimation ( CharacterAnimationControl.JUMP_ANIMATION );
		_initialPosition = VectorTools.cloneVector3 ( transform.position );
		onCompleteTweenAnimationJumpDownCelebration ();
	}
	
	private void onCompleteTweenAnimationJumpDownCelebration ()
	{
		if ( _countJumping >= _maxJumps )
		{
			GlobalVariables.CHARACTER_CELEBRATING = false;
			transform.Find ( "tile" ).GetComponent < CharacterAnimationControl > ().changeState ( CharacterAnimationControl.DOWN );
			_countJumping = 0;
			return;
		}
		
		_countJumping++;
		iTween.MoveTo ( this.gameObject, iTween.Hash ( "time", 0.09f * _slwoFactor, "easetype", iTween.EaseType.easeOutCubic, "position", _initialPosition + Vector3.forward, "oncomplete", "onCompleteTweenAnimationJumpUpCelebration"));
	}
	
	private void onCompleteTweenAnimationJumpUpCelebration ()
	{
		iTween.MoveTo ( this.gameObject, iTween.Hash ( "time", 0.18f * _slwoFactor, "easetype", iTween.EaseType.easeOutBounce, "position", _initialPosition, "oncomplete", "onCompleteTweenAnimationJumpDownCelebration"));
	}
}
