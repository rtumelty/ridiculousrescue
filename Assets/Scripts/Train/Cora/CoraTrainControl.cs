using UnityEngine;
using System.Collections;

public class CoraTrainControl : MonoBehaviour 
{
	//*************************************************************//
	public const string IDLE_ANIMATION = "idle";
	public const string SWIPE_ANIMATION = "attack_swipe";
	public const string SPIN_ANIMATION = "attack_spin";
	public const string ELECTROCUETED_ANIMATION = "electrocuted";
	//*************************************************************//
	private bool _mouseDownOnMe = false;
	//*************************************************************//	
#if UNITY_EDITOR
	private Vector3 _lastMousePosition;
#endif

	void Start () 
	{
		GetComponent < SkeletonAnimation > ().animationName = IDLE_ANIMATION;
	}

	void Update ()
	{
		if ( _mouseDownOnMe )
		{
#if UNITY_EDITOR
			if ( Input.GetMouseButton ( 0 ))
			{
				if ( _lastMousePosition != Input.mousePosition )
				{
					if ( Vector3.Distance ( _lastMousePosition, Input.mousePosition ) > 100f )
					{
						int xDirection = (int) ( Input.mousePosition.x - _lastMousePosition.x );
						int zDirection = (int) ( Input.mousePosition.y - _lastMousePosition.y );
#else
			if ( Input.touchCount == 1 )
			{
				if ( Input.touches[0].phase == TouchPhase.Moved )
				{
					if ( Input.touches[0].deltaPosition.magnitude > 18f )
					{
						int xDirection = (int) Input.touches[0].deltaPosition.x;
						int zDirection = (int) Input.touches[0].deltaPosition.y;
#endif
						if ( Mathf.Abs ( xDirection ) > Mathf.Abs ( zDirection ))
						{
							_mouseDownOnMe = false;
							StartCoroutine ( "spinSequence" );
						}
						else
						{
							if ( zDirection > 0 ) 
							{
								GetComponent < SkeletonAnimation > ().animationName = SWIPE_ANIMATION;
								SoundManager.getInstance().playSound(SoundManager.CORA_CLAP);
								StartCoroutine ( "swipeSequence" );
								_mouseDownOnMe = false;
							}
						}
					}
				}
			}
			else
			{
				_mouseDownOnMe = false;
			}
						
#if UNITY_EDITOR
			_lastMousePosition = VectorTools.cloneVector3 ( Input.mousePosition );
#endif
		}
	}

	private IEnumerator swipeSequence ()
	{
		if ( TREventsManager.getInstance ().getCurrentTutorialID () == TREventsManager.TUTORIAL_ID_SWIPE_CORA )
		{
			TREventsManager.getInstance ().cleanTutorialSwipeCora ();
		}

		TREventsManager.BatOnLevelClass batClassAboveCora = null;
		yield return new WaitForSeconds ( 0.2f );
		foreach ( TREventsManager.BatOnLevelClass batClass in TREventsManager.getInstance ().batsOnLevel )
		{
			if ( batClass.myCharacterToAttack.myID == GameElements.CHAR_CORA_1_IDLE )
			{
				batClass.batObject.GetComponent < TRBatControl > ().playAnimation ( TRBatControl.DESTROY_ANIMATION );
				batClassAboveCora = batClass;
				break;
			}
		}

		yield return new WaitForSeconds ( 0.3f );
		
		if ( batClassAboveCora != null )
		{
			TREventsManager.getInstance ().batsOnLevel.Remove ( batClassAboveCora );
			TREventsManager.getInstance ().coraHasBatAbove = false;
			Destroy ( batClassAboveCora.batObject, 1.5f );
		}

		GetComponent < SkeletonAnimation > ().animationName = IDLE_ANIMATION;
	}

	private IEnumerator spinSequence ()
	{
		if ( TREventsManager.getInstance ().getCurrentTutorialID () == TREventsManager.TUTORIAL_ID_TAP_CORA )
		{
			TREventsManager.getInstance ().cleanTutorialTapCora ();
		}
		
		GetComponent < SkeletonAnimation > ().animationName = SPIN_ANIMATION;

		TREventsManager.BatOnLevelClass batClassAboveFaradaydo = null;
		TREventsManager.BatOnLevelClass batClassAboveJose = null;
		yield return new WaitForSeconds ( 0.2f );
		foreach ( TREventsManager.BatOnLevelClass batClass in TREventsManager.getInstance ().batsOnLevel )
		{
			if ( batClass.myCharacterToAttack.myID == GameElements.CHAR_FARADAYDO_1_IDLE )
			{
				SoundManager.getInstance ().playSound (SoundManager.CORA_CLAP);
				batClass.batObject.GetComponent < TRBatControl > ().playAnimation ( TRBatControl.DESTROY_ANIMATION );
				batClassAboveFaradaydo = batClass;
			}
			else if ( batClass.myCharacterToAttack.myID == GameElements.CHAR_JOSE_1_IDLE )
			{
				SoundManager.getInstance ().playSound (SoundManager.CORA_CLAP);
				batClass.batObject.GetComponent < TRBatControl > ().playAnimation ( TRBatControl.DESTROY_ANIMATION );
				batClassAboveJose = batClass;
			}
		}
		if (batClassAboveJose == null && batClassAboveFaradaydo == null) 
		{
			SoundManager.getInstance().playSound(SoundManager.CORA_MISS);
		}
		yield return new WaitForSeconds ( 0.3f );


		if ( batClassAboveFaradaydo != null )
		{
			TREventsManager.getInstance ().batsOnLevel.Remove ( batClassAboveFaradaydo );
			TREventsManager.getInstance ().faradaydoHasBatAbove = false;
			Destroy ( batClassAboveFaradaydo.batObject, 1.5f );
		}

		if ( batClassAboveJose != null )
		{
			TREventsManager.getInstance ().batsOnLevel.Remove ( batClassAboveJose );
			TREventsManager.getInstance ().joseHasBatAbove = false;
			Destroy ( batClassAboveJose.batObject, 1.5f );
		}
		
		GetComponent < SkeletonAnimation > ().animationName = IDLE_ANIMATION;
	}

	public void playElectrocuted ()
	{
		StartCoroutine ( "waitBeofreIdle" );
	}

	private IEnumerator waitBeofreIdle ()
	{
		GetComponent < SkeletonAnimation > ().animationName = ELECTROCUETED_ANIMATION;
		yield return new WaitForSeconds ( 1f );
		GetComponent < SkeletonAnimation > ().animationName = IDLE_ANIMATION;
	}
	
	void OnMouseDown ()
	{
		if ( TRGlobalVariables.checkForMenus ()) return;

		handleTouched ();
	}

	private void handleTouched ()
	{
		_mouseDownOnMe = true;
#if UNITY_EDITOR
		_lastMousePosition = VectorTools.cloneVector3 ( Input.mousePosition );
#endif
	}
}
