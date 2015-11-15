using UnityEngine;
using System.Collections;

public class TRBatControl : MonoBehaviour 
{
	//************************************************//
	public const string IDLE_ANIMATION = "idle";
	public const string ATTACK_ANIMATION = "attack";
	public const string ATTACK_STAY_ANIMATION = "grab";
	public const string DESTROY_ANIMATION = "defeat";
	
	public const float TIME_TO_ATTACK_MIN = 2.5f;
	public const float TIME_TO_ATTACK_MAX = 3f;
	//************************************************//
	public CharacterData myCharacterToAttack;
	//************************************************//
	private float _countTimeToAttack;
	private float _countAttackTime = 3f;
	private bool _attackExecuted = false;
	private bool _atacking = false;
	private bool _destroyed = false;
	private float _countTimeToFirstAttack = 0f;
	private bool _firstAttackExecuted = false;
	private bool updatePos = false;
	private bool inAttack  = false;
	private bool attackStarted = false;
	
	private GameObject myTarget;
	//************************************************//
	void Awake ()
	{
		_countTimeToAttack = Random.Range ( TIME_TO_ATTACK_MIN, TIME_TO_ATTACK_MAX );
		_countTimeToFirstAttack = Random.Range ( TIME_TO_ATTACK_MIN, TIME_TO_ATTACK_MAX );
		
		playAnimation ( IDLE_ANIMATION );
	}
	
	void Start ()
	{
		//==============Daves edit===============================
		SoundManager.getInstance().playSound(SoundManager.BAT_SWOOP);
		if ( myCharacterToAttack.myID == GameElements.CHAR_JOSE_1_IDLE )
		{
			myTarget = GameObject.Find ("jose");
		}
		else if ( myCharacterToAttack.myID == GameElements.CHAR_FARADAYDO_1_IDLE )
		{
			myTarget = GameObject.Find ("Faradaydo");
		}
		else if ( myCharacterToAttack.myID == GameElements.CHAR_CORA_1_IDLE )
		{
			myTarget = GameObject.Find ("Cora");
		}
		
	}
	
	void Update () 
	{
		
		
		if( inAttack == false && _destroyed != true)
		{
			//transform.localPosition = new Vector3 (myTarget.transform.localPosition.x, transform.localPosition.y, transform.localPosition.z);
			transform.localPosition = Vector3.Lerp (transform.localPosition, new Vector3(myTarget.transform.localPosition.x, transform.localPosition.y, transform.localPosition.z),1);
			/*if(transform.position.x == myTarget.transform.localPosition.x + myTarget.transform.position.x)
			{
				transform.parent = myTarget.transform.parent;
				haveParent = true;
				print ("Parented");
			}*/
		}
		//==============Daves edit===============================
		_countTimeToAttack -= Time.deltaTime;
		if ( gameObject.GetComponent < SkeletonAnimation > ().animationName == DESTROY_ANIMATION )
		{
			transform.Translate ( Vector3.left * Time.deltaTime * TRSpeedAndTrackOMetersManager.getInstance ().getSpeed () );
			return;
		}
		
		if ( ! _firstAttackExecuted )
		{
			_countTimeToFirstAttack -= Time.deltaTime;
			if ( _countTimeToFirstAttack <= 0f )
			{
				_firstAttackExecuted = true;
				StartCoroutine ( "attackSequence" );
			}
		}
		
		if ( _atacking )
		{
			if ( _countTimeToAttack <= 0f )
			{
				_countAttackTime -= Time.deltaTime;
				if ( _countAttackTime <= 0f )
				{
					_countTimeToAttack = Random.Range ( TIME_TO_ATTACK_MIN, TIME_TO_ATTACK_MAX );
					_countAttackTime = 3f;

					_attackExecuted = false;
				}
				else if ( _countAttackTime <= 0.7f && TRResoultScreen.getInstance().itsOver == false)
				{
					if ( ! _attackExecuted && myCharacterToAttack != null)
					{
						_attackExecuted = true;
						if(myCharacterToAttack.characterValues[CharacterData.CHARACTER_ACTION_TYPE_POWER] > 0)
						{
							myCharacterToAttack.characterValues[CharacterData.CHARACTER_ACTION_TYPE_POWER] -= 1;
						}
						if ( myCharacterToAttack.myID == GameElements.CHAR_JOSE_1_IDLE )
						{
							myCharacterToAttack.characterObject.GetComponent < TRJoseControl > ().playElectrocuted ();
							SoundManager.getInstance ().playSound (SoundManager.BAT_MOUNT);
							SoundManager.getInstance ().playSound (SoundManager.ELECTROCUTED);
						}
						else if ( myCharacterToAttack.myID == GameElements.CHAR_FARADAYDO_1_IDLE )
						{
							myCharacterToAttack.characterObject.GetComponent < FaradaydoTrainControl > ().playElectrocuted ();
							SoundManager.getInstance ().playSound (SoundManager.BAT_MOUNT);
							SoundManager.getInstance ().playSound (SoundManager.ELECTROCUTED);
						}
						else if ( myCharacterToAttack.myID == GameElements.CHAR_CORA_1_IDLE )
						{
							myCharacterToAttack.characterObject.GetComponent < CoraTrainControl > ().playElectrocuted ();
							SoundManager.getInstance ().playSound (SoundManager.BAT_MOUNT);
							SoundManager.getInstance ().playSound (SoundManager.ELECTROCUTED);
						}
						
						if ( myCharacterToAttack.characterValues[CharacterData.CHARACTER_ACTION_TYPE_POWER] <= 0 && TRResoultScreen.getInstance().myCharacter == null)
						{
							TRResoultScreen.getInstance ().myCharacter = myCharacterToAttack;
							myCharacterToAttack.characterValues[CharacterData.CHARACTER_ACTION_TYPE_POWER] = 0;
							TRResoultScreen.getInstance ().startResoultScreen ( false );
						}
					}
				}
			}
		}
	}
	
	private IEnumerator attackSequence ()
	{
		playAnimation ( ATTACK_ANIMATION );
		yield return new WaitForSeconds ( 1.1f );
		playAnimation ( ATTACK_STAY_ANIMATION );
		_atacking = true;
	}

	private void loopBatFlap()
	{
		SoundManager.getInstance ().playSound (SoundManager.BAT_FLAP);
//		print ("flapped");
	}
	
	public void playAnimation ( string animationID )
	{
		switch ( animationID )
		{
		case IDLE_ANIMATION:
			InvokeRepeating("loopBatFlap",0,2.142f);
//			print ("flap timing");
			if ( gameObject.GetComponent < SkeletonAnimation > ().animationName != IDLE_ANIMATION )
			{
//				print ("num1");
				//======Daves Edit=======

				gameObject.GetComponent < SkeletonAnimation > ().animationName = ATTACK_STAY_ANIMATION;
				gameObject.GetComponent < SkeletonAnimation > ().animationName = IDLE_ANIMATION;
				//======Daves Edit=======
			}
			break;
		case DESTROY_ANIMATION:
			CancelInvoke("loopBatFlap");
			if ( gameObject.GetComponent < SkeletonAnimation > ().animationName != DESTROY_ANIMATION )
			{
//				print ("num2");
				SoundManager.getInstance().playSound(SoundManager.BAT_DEFEAT);
				transform.localScale = new Vector3 ( -transform.localScale.x, transform.localScale.y, transform.localScale.z );
				_destroyed = true;
				StopCoroutine ( "attackSequence" );
				gameObject.GetComponent < SkeletonAnimation > ().animationName = ATTACK_STAY_ANIMATION;
				gameObject.GetComponent < SkeletonAnimation > ().animationName = DESTROY_ANIMATION;
			}
			break;
		case ATTACK_STAY_ANIMATION:
			CancelInvoke("loopBatFlap");
			if ( gameObject.GetComponent < SkeletonAnimation > ().animationName != ATTACK_STAY_ANIMATION )
			{
				inAttack = true;
				transform.position += Vector3.left * 1.26f;
				gameObject.GetComponent < SkeletonAnimation > ().animationName = IDLE_ANIMATION;
				gameObject.GetComponent < SkeletonAnimation > ().animationName = ATTACK_STAY_ANIMATION;
				
			}
			break;
		case ATTACK_ANIMATION:
			CancelInvoke("loopBatFlap");
			if ( gameObject.GetComponent < SkeletonAnimation > ().animationName != ATTACK_ANIMATION )
			{
				SoundManager.getInstance().playSound(SoundManager.BAT_BITE );
//				print ("num4");
				inAttack = true;
				attackStarted = true;
				transform.position += Vector3.right * 1.2f;
				gameObject.GetComponent < SkeletonAnimation > ().animationName = ATTACK_STAY_ANIMATION;
				gameObject.GetComponent < SkeletonAnimation > ().animationName = ATTACK_ANIMATION;
			}
			break;
		}
	}
}
