using UnityEngine;
using System.Collections;

public class TRMadraControl : MonoBehaviour 
{
	//*************************************************************//	
	public const string IDLE_ANIMATION = "standing";
	public const string MOVE_ANIMATION = "walk4";
	public const string ATTACK_ANIMATION = "attack_jump";
	//*************************************************************//	
	private CharacterAnimationControl _myCharacterAnimationControl;
	private bool _jumped;
	private Vector3 _startPosition;
	//*************************************************************//	
	private static TRMadraControl _meInstance;
	public static TRMadraControl getInstance ()
	{
		return _meInstance;
	}
	//*************************************************************//
	void Awake () 
	{
		TRMadraControl._meInstance = this;
		_myCharacterAnimationControl = GetComponent < CharacterAnimationControl > ();

		_startPosition = VectorTools.cloneVector3 ( transform.localPosition );
	}

	IEnumerator Start ()
	{
		yield return new WaitForSeconds ( 0.1f );
		GetComponent < SkeletonAnimation > ().animationName = MOVE_ANIMATION;
	}

	public void jumpToCora () 
	{
		if ( _jumped ) return;

		_jumped = true;
		Transform jumpTransform = Camera.main.transform.Find ( "train" ).transform.Find ( "part03" ).transform.Find ( "Faradaydo" );
		iTween.MoveTo ( this.gameObject, iTween.Hash ( "time", 0.3f, "easetype", iTween.EaseType.easeOutExpo, "position", jumpTransform.position + Vector3.forward * 0.85f, "oncomplete", "onCompleteTweenAnimationMoveToPositionRight" ));
	}

	private void onCompleteTweenAnimationMoveToPositionRight ()
	{
		GetComponent < SkeletonAnimation > ().animationName = IDLE_ANIMATION;
	}

	public void stop ()
	{
		GetComponent < SkeletonAnimation > ().animationName = IDLE_ANIMATION;
	}

	public void startRuning ()
	{
		GetComponent < SkeletonAnimation > ().animationName = MOVE_ANIMATION;
	}

	public void attackTentacle ()
	{
		iTween.MoveTo ( this.gameObject, iTween.Hash ( "time", 0.25f, "easetype", iTween.EaseType.linear, "position", TREventsManager.getInstance ().tentacleOnlevel.transform.position + Vector3.back * 2f, "oncomplete", "onComplete" ));
		GetComponent < SkeletonAnimation > ().animationName = ATTACK_ANIMATION;
	}

	private void onComplete ()
	{
		//SoundManager.getInstance ().playSound ( SoundManager.CHARACTER_ATTACK, 39 );
		Instantiate ( TREventsManager.getInstance ().tentalceParticle, TREventsManager.getInstance ().tentacleOnlevel.transform.position, TREventsManager.getInstance ().tentalceParticle.transform.rotation );
		iTween.MoveTo ( this.gameObject, iTween.Hash ( "time", 0.12f, "easetype", iTween.EaseType.linear, "position", _startPosition, "islocal", true ));
	}
}
