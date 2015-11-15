using UnityEngine;
using System.Collections;

public class TRTentacleControl : MonoBehaviour 
{
	private const float _START_GOING_WITH_TRAIN_DISTANCE = 5.4f;
	private const float _START_ATTACKING_DISTANCE = 8f;
	//**************************************************************************************************************//
	public bool stopingTrain = false;
	//**************************************************************************************************************//
	public bool dummyScript = false;
	//**************************************************************************************************************//
	private bool _attacking = false;
	private bool _mouseDownOnMe = false;
	private bool _swipeSeqence = false;
	private bool _grabHoleShowed = false;
	private float _countTimeToMoveGrabHole05 = 0.2f;
	private bool _startMovingGrabHole05 = false;
	private Transform _currentGrabHoleToAnimate;
	private Vector3 _initialPositionForGrabHole05;
	private bool _particlesAreOn = false;
	private bool _wobbleOn = false;
	private Vector3 _initialRotation;
	private float mySpeed;
	private Transform train;
	private bool loopOver = true;
#if UNITY_EDITOR
	private Vector3 _lastMousePosition;
#endif
	//**************************************************************************************************************//

	void Start ()
	{
		train = GameObject.Find ("tentacleReference").transform;
		transform.FindChild ("tile").GetComponent <AudioSource> ().enabled = false;

	}

	void Update () 
	{
		if ( dummyScript )
		{
			this.gameObject.SetActive(false);
			return;
		}
		GameGlobalVariables.ACTIVE_TENTACLE = true;
		if ( transform.position.x > _START_GOING_WITH_TRAIN_DISTANCE )
		{
			mySpeed = TRSpeedAndTrackOMetersManager.getInstance ().getSpeed ();
			transform.Translate ( Vector3.right * Time.deltaTime * mySpeed );
		}
		else
		{
			transform.Find("tile").GetComponent<AudioSource> ().volume = ChChChSoundManger.getInstance ().GetComponent<AudioSource> ().volume;
			transform.FindChild ("tile").GetComponent <AudioSource> ().enabled = true;
			transform.parent = train.transform;
			if ( ! _grabHoleShowed )
			{
				StartCoroutine ( "showGrabHole" );
				SoundManager.getInstance().playSound(SoundManager.TENTACLE_POPUP);
				_grabHoleShowed = true;
			}

			if ( _startMovingGrabHole05 )
			{
				_countTimeToMoveGrabHole05 -= Time.deltaTime;
				if ( _countTimeToMoveGrabHole05 <= 0f )
				{
					_countTimeToMoveGrabHole05 = 0.2f;
					_currentGrabHoleToAnimate.transform.localPosition = VectorTools.cloneVector3 ( _initialPositionForGrabHole05 );
				}

				_currentGrabHoleToAnimate.Translate ( Vector3.right * Time.deltaTime * TRSpeedAndTrackOMetersManager.getInstance ().getSpeed () );
			}

			stopingTrain = true;
			if ( ! _particlesAreOn )
			{
				_particlesAreOn = true;
				transform.Find ( "particlesTentacleTrain" ).gameObject.SetActive ( true );
			}

			if ( ! _wobbleOn )
			{
				_wobbleOn = true;
				_initialRotation = VectorTools.cloneVector3 ( transform.rotation.eulerAngles );
				iTween.RotateTo ( this.gameObject, iTween.Hash ( "time", 0.1f, "easetype", iTween.EaseType.linear, "rotation", _initialRotation + new Vector3 ( 0f, 1f, 0f ), "oncompletetarget", this.gameObject,  "oncomplete", "onComplete01" ));
			}
		}

		if ( transform.position.x <= _START_ATTACKING_DISTANCE && ! _attacking )
		{
			_attacking = true;
			transform.Find ( "tile" ).GetComponent < EnemyAnimationComponent > ().playAnimation ( EnemyAnimationComponent.ATTACK_ANIMATION );
			StartCoroutine ( "waitBeforeGrab" );
		}

		if ( _mouseDownOnMe )
		{
#if UNITY_EDITOR
			if ( Input.GetMouseButton ( 0 ))
			{
				if ( _lastMousePosition != Input.mousePosition )
				{
					if ( Vector3.Distance ( _lastMousePosition, Input.mousePosition ) > 200f )
					{
						int xDirection = (int) ( Input.mousePosition.x - _lastMousePosition.x ); 
						int zDirection = (int) ( Input.mousePosition.y - _lastMousePosition.y );
#else
			if ( Input.touchCount == 1 )
			{
				if ( Input.touches[0].phase == TouchPhase.Moved )
				{
					if ( Input.touches[0].deltaPosition.magnitude > 28f )
					{
						int xDirection = (int) Input.touches[0].deltaPosition.x;
						int zDirection = (int) Input.touches[0].deltaPosition.y;
#endif
						if ( Mathf.Abs ( xDirection ) > Mathf.Abs ( zDirection ) && ! _swipeSeqence )
						{
							StartCoroutine ( "swipeSeqence" );
						}
						else
						{
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
		else
		{
			_mouseDownOnMe = false;
		}
	}

	private void onComplete01 ()
	{
		iTween.RotateTo ( this.gameObject, iTween.Hash ( "time", 0.1f, "easetype", iTween.EaseType.linear, "rotation", _initialRotation + new Vector3 ( 0f, -1f, 0f ), "oncompletetarget", this.gameObject,  "oncomplete", "onComplete02" ));
	}

	private void onComplete02 ()
	{
		iTween.RotateTo ( this.gameObject, iTween.Hash ( "time", 0.1f, "easetype", iTween.EaseType.linear, "rotation", _initialRotation + new Vector3 ( 0f, 1f, 0f ), "oncompletetarget", this.gameObject,  "oncomplete", "onComplete01" ));
	}

	private IEnumerator showGrabHole ()
	{
		yield return new WaitForSeconds ( 0.1f );
		_currentGrabHoleToAnimate = transform.Find ( "hole" ).transform.Find ( "hole01" );
		transform.Find ( "hole" ).transform.Find ( "hole01" ).gameObject.SetActive ( true );
		yield return new WaitForSeconds ( 0.1f );
					_currentGrabHoleToAnimate = transform.Find ( "hole" ).transform.Find ( "hole02" );
					transform.Find ( "hole" ).transform.Find ( "hole01" ).gameObject.SetActive ( false );
					transform.Find ( "hole" ).transform.Find ( "hole02" ).gameObject.SetActive ( true );
		yield return new WaitForSeconds ( 0.1f );
					_currentGrabHoleToAnimate = transform.Find ( "hole" ).transform.Find ( "hole03" );
					transform.Find ( "hole" ).transform.Find ( "hole02" ).gameObject.SetActive ( false );
					transform.Find ( "hole" ).transform.Find ( "hole03" ).gameObject.SetActive ( true );
		yield return new WaitForSeconds ( 0.1f );
					_currentGrabHoleToAnimate = transform.Find ( "hole" ).transform.Find ( "hole04" );
					transform.Find ( "hole" ).transform.Find ( "hole03" ).gameObject.SetActive ( false );
					transform.Find ( "hole" ).transform.Find ( "hole04" ).gameObject.SetActive ( true );
		yield return new WaitForSeconds ( 0.1f );
					_currentGrabHoleToAnimate = transform.Find ( "hole" ).transform.Find ( "hole05" );
					transform.Find ( "hole" ).transform.Find ( "hole04" ).gameObject.SetActive ( false );
					transform.Find ( "hole" ).transform.Find ( "hole05" ).gameObject.SetActive ( true );
		yield return new WaitForSeconds ( 0.1f );
					_currentGrabHoleToAnimate = transform.Find ( "hole" ).transform.Find ( "hole06" );
					transform.Find ( "hole" ).transform.Find ( "hole05" ).gameObject.SetActive ( false );
					transform.Find ( "hole" ).transform.Find ( "hole06" ).gameObject.SetActive ( true );
		yield return new WaitForSeconds ( 0.1f );
					_currentGrabHoleToAnimate = transform.Find ( "hole" ).transform.Find ( "hole07" );
					transform.Find ( "hole" ).transform.Find ( "hole06" ).gameObject.SetActive ( false );
					transform.Find ( "hole" ).transform.Find ( "hole07" ).gameObject.SetActive ( true );
		yield return new WaitForSeconds ( 0.1f );
					_currentGrabHoleToAnimate = transform.Find ( "hole" ).transform.Find ( "hole08" );
					transform.Find ( "hole" ).transform.Find ( "hole07" ).gameObject.SetActive ( false );
					transform.Find ( "hole" ).transform.Find ( "hole08" ).gameObject.SetActive ( true );
		yield return new WaitForSeconds ( 0.1f );
					_initialPositionForGrabHole05 = VectorTools.cloneVector3 (_currentGrabHoleToAnimate.transform.localPosition );
					_startMovingGrabHole05 = true;
					_currentGrabHoleToAnimate = transform.Find ( "hole" ).transform.Find ( "hole09" );
					transform.Find ( "hole" ).transform.Find ( "hole08" ).gameObject.SetActive ( false );
					transform.Find ( "hole" ).transform.Find ( "hole09" ).gameObject.SetActive ( true );
	}

	private IEnumerator swipeSeqence ()
	{
		_swipeSeqence = true;

		if ( TREventsManager.getInstance ().getCurrentTutorialID () == TREventsManager.TUTORIAL_ID_SWIPE_TENTACLE )
		{
			TREventsManager.getInstance ().cleanTutorialSwipeTentacle ();
		}

		TRMadraControl.getInstance ().attackTentacle ();
		yield return new WaitForSeconds ( 0.4f );
		TRMadraControl.getInstance ().startRuning ();
		SoundManager.getInstance ().playSound (SoundManager.MADRA_BITE);
		SoundManager.getInstance ().playSound (SoundManager.TENTACLE_DRAINER_DEFATED);
		
		_swipeSeqence = false;
		stopingTrain = false;
		this.gameObject.SetActive(false );
		GameGlobalVariables.ACTIVE_TENTACLE = false;
		print ("well?");
	}

	private IEnumerator waitBeforeGrab ()
	{
		yield return new WaitForSeconds ( 0.4f );
		Destroy ( transform.Find ( "tile" ).GetComponent < EnemyAnimationComponent > ());
		transform.Find ( "tile" ).renderer.material.mainTexture = TRLevelControl.getInstance ().tentacleGrabTexture;
		transform.Find ( "tile" ).localScale = new Vector3 ( 2.5f, 1f, 2.5f );
		transform.position = new Vector3 ( transform.position.x, transform.position.y, 3.89f );
		Destroy ( GameObject.Find ( "tentacleRock(Clone)" ).gameObject );
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
