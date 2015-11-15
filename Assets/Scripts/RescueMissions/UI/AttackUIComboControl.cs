using UnityEngine;
using System.Collections;

public class AttackUIComboControl : ObjectTapControl 
{
	//*************************************************************//	
	private GameObject _progressMarker;
	private GameObject _greenBox;
	private Vector3 _progressMarkerStartPosition;
	private GameObject _tutorialHandPrefab;
	private GameObject _tutorialHandInstant;
	private EnemyComponent.HandleAttackExecuted _callBackWhenAttackExecuted;
	private EnemyData _enemyAttacked;
	private CharacterData _characterAttacking;
	private bool _startProgressBar = false;
	private bool _alreadyJumping = false;
	private float _restartCount = 1f;
	private bool _stopProgressBar = false;
	//*************************************************************//	
	void Awake () 
	{
		_tutorialHandPrefab = ( GameObject ) Resources.Load ( "UI/hand" );
		_progressMarker = transform.Find ( "marker" ).gameObject;
		_greenBox = transform.Find ( "greenBox" ).gameObject;
		_progressMarkerStartPosition = VectorTools.cloneVector3 ( _progressMarker.transform.localPosition );
	}
	
	public void initAttackObjects ( EnemyComponent.HandleAttackExecuted callBackWhenAttackExecuted, EnemyData enemyAttacked, CharacterData attackingCharacter = null )
	{
		_callBackWhenAttackExecuted = callBackWhenAttackExecuted;
		_enemyAttacked = enemyAttacked;
		_characterAttacking = attackingCharacter;
		_startProgressBar = true;
		
		if ((( GameGlobalVariables.CURRENT_GAME_PART == GameGlobalVariables.RESCUE && GlobalVariables.TUTORIAL_MENU ) ||  GameGlobalVariables.CURRENT_GAME_PART == GameGlobalVariables.MINING && MNGlobalVariables.TUTORIAL_MENU ) /*====Daves Edit=====*/&& (LevelControl.LEVEL_ID == 2)/*====Daves Edit=====*/ &&( TutorialsManager.getInstance ().getCurrentTutorialStep ().type != TutorialsManager.TUTORIAL_OBJECT_TYPE_DESTROY_OBJECTS ))
		{
			if ( LevelControl.LEVEL_ID != 16 )
			{
				_tutorialHandInstant = ( GameObject ) Instantiate ( _tutorialHandPrefab, transform.position + Vector3.right * 1f + Vector3.up * 1f + Vector3.back * 2f, transform.rotation );
				_tutorialHandInstant.transform.parent = transform;
			}
		}
	}
	
	void Update () 
	{
		if ( _startProgressBar )
		{
			if ( ! _stopProgressBar ) _progressMarker.transform.Translate ( Vector3.left * Time.deltaTime * 0.4f * _enemyAttacked.enemyValues[EnemyData.ENEMY_ACTION_TYPE_TIME_BAR_SPEED]);
			
			if ((( GameGlobalVariables.CURRENT_GAME_PART == GameGlobalVariables.RESCUE && GlobalVariables.TUTORIAL_MENU ) ||  GameGlobalVariables.CURRENT_GAME_PART == GameGlobalVariables.MINING && MNGlobalVariables.TUTORIAL_MENU ) && ( TutorialsManager.getInstance ().getCurrentTutorialStep ().type != TutorialsManager.TUTORIAL_OBJECT_TYPE_DESTROY_OBJECTS ))
			{
				if ( LevelControl.LEVEL_ID != 16 )
				{
					if ( _progressMarker.transform.localPosition.x <= 0.185f )
					{
						_progressMarker.transform.localPosition = new Vector3 ( 0.185f, _progressMarker.transform.localPosition.y, _progressMarker.transform.localPosition.z );
						if(!(MNLevelControl.LEVEL_ID > 0))
						{
							if ( _tutorialHandInstant.GetComponent < SimulateTapControl > () == null ) _tutorialHandInstant.AddComponent < SimulateTapControl > ();
						}

						_restartCount -= Time.deltaTime;
						
						if ( _restartCount <= 0f )
						{
							_restartCount = 1f;
							if(LevelControl.LEVEL_ID == 2)/*====Daves Edit=====*/
							{
								iTween.Stop ( _tutorialHandInstant );
								Destroy ( _tutorialHandInstant.GetComponent < SimulateTapControl > ());
								_tutorialHandInstant.transform.position = transform.position + Vector3.right * 1f + Vector3.up * 1f + Vector3.back * 2f;
								_progressMarker.transform.localPosition = VectorTools.cloneVector3 ( _progressMarkerStartPosition );
							}
						}
						
						return;
					}
					else
					{
						if(LevelControl.LEVEL_ID == 2)/*====Daves Edit=====*/
						{
							_tutorialHandInstant.transform.Translate (( Vector3.right + Vector3.back * 0.7f ) * Time.deltaTime * 1f );
						}
					}
				}
			}
			
			if (( _progressMarker.transform.localPosition.x <= 0.23f ) && ( _progressMarker.transform.localPosition.x >= 0.12f ) && ! _alreadyJumping )
			{
				_alreadyJumping = true;
				iTween.ScaleFrom ( _greenBox, iTween.Hash ( "time", 0.5f, "easetype", iTween.EaseType.easeOutBounce, "scale", new Vector3 ( 0.18f, 1f, 0.98f ) * 1.5f, "oncompletetarget", this.gameObject, "oncomplete", "onCompleteScaleGreenBox" ));
			}
			
			if ( _progressMarker.transform.localPosition.x <= -0.1f )
			{
				_startProgressBar = false;
				_progressMarker.transform.localPosition = new Vector3 ( 0.5f, _progressMarker.transform.localPosition.y, _progressMarker.transform.localPosition.z );
				
				_callBackWhenAttackExecuted ( false );
				if (( GameGlobalVariables.CURRENT_GAME_PART == GameGlobalVariables.RESCUE && ! GlobalVariables.TUTORIAL_MENU ) || ( GameGlobalVariables.CURRENT_GAME_PART == GameGlobalVariables.MINING && ! MNGlobalVariables.TUTORIAL_MENU ) ||  ( TutorialsManager.getInstance ().getCurrentTutorialStep ().type == TutorialsManager.TUTORIAL_OBJECT_TYPE_DESTROY_OBJECTS ) || LevelControl.LEVEL_ID == 16 )
				{
					Destroy ( this.gameObject );
				}
			}
		}
	}
	
	private void onCompleteScaleGreenBox ()
	{
		_alreadyJumping = false;
		_greenBox.transform.localScale = new Vector3 ( 0.18f, 1f, 0.98f );
	}
	
	public bool correctTapAlready ()
	{
		return ! _startProgressBar;
	}

	void OnMouseDown ()
	{
		if ( _characterAttacking != null ) _characterAttacking.countTimeToNextPossibleMoveAfterAttack = 1.5f;
	}

	void OnMouseUp ()
	{
		if ( _alreadyTouched ) return;
		StartCoroutine ( "waitBeforeAction" );
	}
	
	private IEnumerator waitBeforeAction ()
	{
		_stopProgressBar = true;
		if ( GameGlobalVariables.CURRENT_GAME_PART == GameGlobalVariables.RESCUE )
		{
			_greenBox.renderer.material.mainTexture = UIControl.getInstance ().attackGreenBoxDown;
		}
		else if ( GameGlobalVariables.CURRENT_GAME_PART == GameGlobalVariables.MINING )
		{
			_greenBox.renderer.material.mainTexture = MNUIControl.getInstance ().attackGreenBoxDown;
		}
		
		yield return new WaitForSeconds ( 0.2f );
		_stopProgressBar = false;
		
		if ( GameGlobalVariables.CURRENT_GAME_PART == GameGlobalVariables.RESCUE )
		{
			_greenBox.renderer.material.mainTexture = UIControl.getInstance ().attackGreenBoxUp;
		}
		else if ( GameGlobalVariables.CURRENT_GAME_PART == GameGlobalVariables.MINING )
		{
			_greenBox.renderer.material.mainTexture = MNUIControl.getInstance ().attackGreenBoxUp;
		}
		
		handleTouched ();
	}
	
	private void handleTouched ()
	{
		if ( _alreadyTouched ) return;
		
		if ((( GameGlobalVariables.CURRENT_GAME_PART == GameGlobalVariables.RESCUE && GlobalVariables.TUTORIAL_MENU ) ||  GameGlobalVariables.CURRENT_GAME_PART == GameGlobalVariables.MINING && MNGlobalVariables.TUTORIAL_MENU ) && ( TutorialsManager.getInstance ().getCurrentTutorialStep ().type != TutorialsManager.TUTORIAL_OBJECT_TYPE_DESTROY_OBJECTS ))
		{
			if ( LevelControl.LEVEL_ID != 16 )
			{
				if (( _progressMarker.transform.localPosition.x <= 0.23f ) && ( _progressMarker.transform.localPosition.x >= 0.12f ))
				{
				}
				else
				{
					_alreadyTouched = true;
					StartCoroutine ( "unblockAlreadyTouched" );
					return;
				}
			}
		}
		
		if ( GameGlobalVariables.CURRENT_GAME_PART == GameGlobalVariables.RESCUE )
		{
			UIControl.getInstance ().blockClicksForAMomentAfterUIClicked ();
		}
		else if ( GameGlobalVariables.CURRENT_GAME_PART == GameGlobalVariables.MINING )
		{
			MNUIControl.getInstance ().blockClicksForAMomentAfterUIClicked ();
		}
		
		_alreadyTouched = true;
		_startProgressBar = false;
		
		if (( _progressMarker.transform.localPosition.x <= 0.23f ) && ( _progressMarker.transform.localPosition.x >= 0.12f ))
		{
			_callBackWhenAttackExecuted ( true );
		}
		else
		{
			_callBackWhenAttackExecuted ( false );
		}
		
		if (( GameGlobalVariables.CURRENT_GAME_PART == GameGlobalVariables.RESCUE && ! GlobalVariables.TUTORIAL_MENU ) || ( GameGlobalVariables.CURRENT_GAME_PART == GameGlobalVariables.MINING && ! MNGlobalVariables.TUTORIAL_MENU ) || ( TutorialsManager.getInstance ().getCurrentTutorialStep ().type == TutorialsManager.TUTORIAL_OBJECT_TYPE_DESTROY_OBJECTS ) || LevelControl.LEVEL_ID == 16 )
		{
			Destroy ( this.gameObject );
		}
	}
	
	private IEnumerator unblockAlreadyTouched ()
	{
		yield return new WaitForSeconds ( 1.0f );
		_alreadyTouched = false;
	}
}
