using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class EnemyAnimationComponent : MonoBehaviour 
{
	//*************************************************************//
	public const int ATTACK_ANIMATION = 0;
	public const int DESTROY_ANIMATION = 1;
	public const int SPAWN_ANIMATION = 2;
	public const int ATTACK_ANIMATION_UP = 3;
	public const int ATTACK_ANIMATION_DOWN = 4;
	public const int ATTACK_ANIMATION_RIGHT = 5;
	public const int ATTACK_ANIMATION_LEFT = 6;
	public const int MOVE_ANIMATION_UP = 7;
	public const int MOVE_ANIMATION_DOWN = 8;
	public const int MOVE_ANIMATION_RIGHT = 9;
	public const int MOVE_ANIMATION_LEFT = 10;
	public const int IDLE_ANIMATION_RIGHT = 11;
	public const int IDLE_ANIMATION_LEFT = 12;
	public const int IDLE_ANIMATION = 13;
	public const int IDLE_ANIMATION_UP = 14;
	public const int IDLE_ANIMATION_DOWN = 15;
	//=========================Daves Work===========================
	public const int ATTACK_ANIMATION_CHARGE = 16;
	public const int ATTACK_ANIMATION_HOLD = 17;
	public const int ATTACK_ANIMATION_CHARGE_LEFT = 18;
	public const int ATTACK_ANIMATION_HOLD_LEFT = 19;
	//=========================Daves Work===========================

	public const string ANIMATION_NAME_SLUG_MOVE_SIDE = "walk";
	public const string ANIMATION_NAME_SLUG_DIE_SIDE = "die";
	public const string ANIMATION_NAME_SLUG_ATTACK_SIDE = "attack";

	public const string ANIMATION_NAME_SLUG_WALK_FRONT = "walk_front";
	public const string ANIMATION_NAME_SLUG_WALK_BACK = "walk_back";

	public const string ANIMATION_NAME_SLUG_ATTACK_FRONT = "attack_front";
	public const string ANIMATION_NAME_SLUG_ATTACK_BACK = "attack_back";
	//*************************************************************//
	private const int _UP = 2;
	private const int _DOWN = 3;
	private const int _RIGHT = 0;
	private const int _LEFT = 1;
	//*************************************************************//
	public CharacterData currentAttackingCharacter;
	//*************************************************************//
	private Material _myMaterial;
	private IComponent _myIComponent;
	private List < Texture2D > _currentAnimationTextures;
	private float _countTimeToNextFrame = 0f;
	private int _frameID = 0;
	private int _currentAnimationID = -1;
	private int _currentIdleDirection = _LEFT;
	private Vector3 _initialScale;
	private Vector3 _initialRotation;
	private Quaternion _initialRotationQuaternion;
	
	private string _myPathToTextures = "";
	private int[] _myAnimationIDs;
	private GameObject _slugSideAnimation;
	private GameObject _slugFrontAnimation;
	private bool _destoryAnimation = false;
	//*************************************************************//	
	void Start () 
	{	
		_myIComponent = gameObject.GetComponent < IComponent > ();

		switch ( _myIComponent.myID )
		{
		case GameElements.ENEM_TENTACLEDRAINER_01:
		case GameElements.ENEM_TENTACLEDRAINER_02:
			_initialScale = VectorTools.cloneVector3 ( transform.localScale );
			_initialRotation = VectorTools.cloneVector3 ( transform.parent.rotation.eulerAngles );
			_initialRotationQuaternion = VectorTools.cloneQuaternion ( transform.parent.rotation );
			//========================Daves Work=====================
			_myAnimationIDs = new int[20];
			//========================Daves Work=====================
			_myMaterial = renderer.material;
			
			switch ( _myIComponent.myID )
			{
			case GameElements.ENEM_TENTACLEDRAINER_01:
				_myPathToTextures = "Textures/Enemies/Tentacle01/idle/enem_tentacledrainer_01_idle0002";
				break;
			case GameElements.ENEM_TENTACLEDRAINER_02:
				_myPathToTextures = "Textures/Enemies/Tentacle02/idle/enem_tentacledrainer_01_idle0001";
				break;
			}
			
			_myAnimationIDs[ATTACK_ANIMATION] = MemoryManager.getInstance ().reserveTexturesForMeAndGiveMeID ( _myIComponent.myID, ATTACK_ANIMATION, "Textures/Enemies/Tentacle01/attack" );
			//========================Daves Work=====================
			//print (_myIComponent.myID);
			//print (ATTACK_ANIMATION_CHARGE);
			_myAnimationIDs[ATTACK_ANIMATION_HOLD] = MemoryManager.getInstance ().reserveTexturesForMeAndGiveMeID ( _myIComponent.myID, ATTACK_ANIMATION_HOLD, "Textures/Enemies/Tentacle01/hold" );
			_myAnimationIDs[ATTACK_ANIMATION_CHARGE] = MemoryManager.getInstance ().reserveTexturesForMeAndGiveMeID ( _myIComponent.myID, ATTACK_ANIMATION_CHARGE, "Textures/Enemies/Tentacle01/charging" );
			_myAnimationIDs[ATTACK_ANIMATION_HOLD_LEFT] = MemoryManager.getInstance ().reserveTexturesForMeAndGiveMeID ( _myIComponent.myID, ATTACK_ANIMATION_HOLD, "Textures/Enemies/Tentacle01/hold" );
			_myAnimationIDs[ATTACK_ANIMATION_CHARGE_LEFT] = MemoryManager.getInstance ().reserveTexturesForMeAndGiveMeID ( _myIComponent.myID, ATTACK_ANIMATION_CHARGE, "Textures/Enemies/Tentacle01/charging" );
			//========================Daves Work=====================
			_myAnimationIDs[DESTROY_ANIMATION] = MemoryManager.getInstance ().reserveTexturesForMeAndGiveMeID ( _myIComponent.myID, DESTROY_ANIMATION, _myPathToTextures/*"Textures/Enemies/Tentacle01/destroyed"*/ );
			_myAnimationIDs[SPAWN_ANIMATION] = MemoryManager.getInstance ().reserveTexturesForMeAndGiveMeID ( _myIComponent.myID, SPAWN_ANIMATION, _myPathToTextures/* + "/spawn" */);
			_myAnimationIDs[ATTACK_ANIMATION_UP] = MemoryManager.getInstance ().reserveTexturesForMeAndGiveMeID ( _myIComponent.myID, ATTACK_ANIMATION_UP, _myPathToTextures /*+ "/attack/up"*/ );
			_myAnimationIDs[ATTACK_ANIMATION_DOWN] = MemoryManager.getInstance ().reserveTexturesForMeAndGiveMeID ( _myIComponent.myID, ATTACK_ANIMATION_DOWN, _myPathToTextures/* + "/attack/down"*/ );
			_myAnimationIDs[ATTACK_ANIMATION_RIGHT] = MemoryManager.getInstance ().reserveTexturesForMeAndGiveMeID ( _myIComponent.myID, ATTACK_ANIMATION_RIGHT, _myPathToTextures/* + "/attack/right"*/ );
			_myAnimationIDs[MOVE_ANIMATION_UP] = MemoryManager.getInstance ().reserveTexturesForMeAndGiveMeID ( _myIComponent.myID, MOVE_ANIMATION_UP, _myPathToTextures/* + "/walk/up"*/ );
			_myAnimationIDs[MOVE_ANIMATION_DOWN] = MemoryManager.getInstance ().reserveTexturesForMeAndGiveMeID ( _myIComponent.myID, MOVE_ANIMATION_DOWN, _myPathToTextures/* + "/walk/down"*/ );
			_myAnimationIDs[MOVE_ANIMATION_RIGHT] = MemoryManager.getInstance ().reserveTexturesForMeAndGiveMeID ( _myIComponent.myID, MOVE_ANIMATION_RIGHT, _myPathToTextures/* + "/walk/right" */);
			_myAnimationIDs[IDLE_ANIMATION_RIGHT] = MemoryManager.getInstance ().reserveTexturesForMeAndGiveMeID ( _myIComponent.myID, IDLE_ANIMATION_RIGHT, _myPathToTextures/* + "/idle/right" */);
			_myAnimationIDs[IDLE_ANIMATION] = MemoryManager.getInstance ().reserveTexturesForMeAndGiveMeID ( _myIComponent.myID, IDLE_ANIMATION, _myPathToTextures/* + "/idle" */);

			break;
		case GameElements.ENEM_SLUG_01:
			_initialScale = VectorTools.cloneVector3 ( transform.localScale );
			_slugSideAnimation = transform.Find ( "side" ).gameObject;
			_slugFrontAnimation = transform.Find ( "front" ).gameObject;
			break;
		}

		playAnimation ( SPAWN_ANIMATION );
	}

	private IEnumerator destroyAfterTime ()
	{
		yield return new WaitForSeconds ( 0.6f );
		if ( GameGlobalVariables.CURRENT_GAME_PART == GameGlobalVariables.RESCUE )
		{
			Main.getInstance ().handleObjectDestory ( _myIComponent.myID, transform.gameObject );
		}
		else if ( GameGlobalVariables.CURRENT_GAME_PART == GameGlobalVariables.MINING )
		{
			MNMain.getInstance ().handleObjectDestory ( _myIComponent.myID, transform.gameObject );
		}
	}
	
	public void playAnimation ( int animationID )
	{
		if ( _currentAnimationID == DESTROY_ANIMATION ) return;
		_currentAnimationID = animationID;

		if ( _myIComponent.myID == GameElements.ENEM_SLUG_01 )
		{
			switch ( _currentAnimationID )
			{
			case DESTROY_ANIMATION:
				_destoryAnimation = true;
				transform.localScale = new Vector3 ( _initialScale.x, _initialScale.y, _initialScale.z );
				_slugSideAnimation.SetActive ( true );
				_slugFrontAnimation.SetActive ( false );
				_slugSideAnimation.GetComponent < SkeletonAnimation > ().animationName = ANIMATION_NAME_SLUG_MOVE_SIDE;
				_slugSideAnimation.GetComponent < SkeletonAnimation > ().animationName = ANIMATION_NAME_SLUG_DIE_SIDE;
				iTween.Stop ( this.transform.gameObject );
				StartCoroutine ( "destroyAfterTime" );
				break;
			case SPAWN_ANIMATION:
				transform.localScale = new Vector3 ( -_initialScale.x, _initialScale.y, _initialScale.z );
				_slugSideAnimation.SetActive ( true );
				_slugFrontAnimation.SetActive ( false );
				_slugSideAnimation.GetComponent < SkeletonAnimation > ().animationName = ANIMATION_NAME_SLUG_DIE_SIDE;
				_slugSideAnimation.GetComponent < SkeletonAnimation > ().animationName = ANIMATION_NAME_SLUG_MOVE_SIDE;
				break;
			case ATTACK_ANIMATION_UP:
				transform.localScale = new Vector3 ( _initialScale.x, _initialScale.y, _initialScale.z );
				_slugSideAnimation.SetActive ( false );
				_slugFrontAnimation.SetActive ( true );
				_slugFrontAnimation.GetComponent < SkeletonAnimation > ().animationName = ANIMATION_NAME_SLUG_ATTACK_FRONT;
				_slugFrontAnimation.GetComponent < SkeletonAnimation > ().animationName = ANIMATION_NAME_SLUG_ATTACK_BACK;
				break;
			case ATTACK_ANIMATION_DOWN:
				transform.localScale = new Vector3 ( _initialScale.x, _initialScale.y, _initialScale.z );
				_slugSideAnimation.SetActive ( false );
				_slugFrontAnimation.SetActive ( true );
				_slugFrontAnimation.GetComponent < SkeletonAnimation > ().animationName = ANIMATION_NAME_SLUG_ATTACK_BACK;
				_slugFrontAnimation.GetComponent < SkeletonAnimation > ().animationName = ANIMATION_NAME_SLUG_ATTACK_FRONT;
				break;
			case ATTACK_ANIMATION_RIGHT:
				transform.localScale = new Vector3 ( _initialScale.x, _initialScale.y, _initialScale.z );
				_slugSideAnimation.SetActive ( true );
				_slugFrontAnimation.SetActive ( false );
				_slugSideAnimation.GetComponent < SkeletonAnimation > ().animationName = ANIMATION_NAME_SLUG_DIE_SIDE;
				_slugSideAnimation.GetComponent < SkeletonAnimation > ().animationName = ANIMATION_NAME_SLUG_ATTACK_SIDE;
				break;
			case ATTACK_ANIMATION_LEFT:
				transform.localScale = new Vector3 ( -_initialScale.x, _initialScale.y, _initialScale.z );
				_slugSideAnimation.SetActive ( true );
				_slugFrontAnimation.SetActive ( false );
				_slugSideAnimation.GetComponent < SkeletonAnimation > ().animationName = ANIMATION_NAME_SLUG_DIE_SIDE;
				_slugSideAnimation.GetComponent < SkeletonAnimation > ().animationName = ANIMATION_NAME_SLUG_ATTACK_SIDE;
				break;
			case MOVE_ANIMATION_UP:
				transform.localScale = new Vector3 ( _initialScale.x, _initialScale.y, _initialScale.z );
				_slugSideAnimation.SetActive ( false );
				_slugFrontAnimation.SetActive ( true );

				if ( _slugSideAnimation.GetComponent < SkeletonAnimation > ().animationName == ANIMATION_NAME_SLUG_WALK_BACK )
				{
					// do nothing
				}
				else
				{
					_slugFrontAnimation.GetComponent < SkeletonAnimation > ().animationName = ANIMATION_NAME_SLUG_WALK_FRONT;
					_slugFrontAnimation.GetComponent < SkeletonAnimation > ().animationName = ANIMATION_NAME_SLUG_WALK_BACK;
				}
				break;
			case MOVE_ANIMATION_DOWN:
				transform.localScale = new Vector3 ( _initialScale.x, _initialScale.y, _initialScale.z );
				_slugSideAnimation.SetActive ( false );
				_slugFrontAnimation.SetActive ( true );

				if ( _slugSideAnimation.GetComponent < SkeletonAnimation > ().animationName == ANIMATION_NAME_SLUG_WALK_FRONT )
				{
					// do nothing
				}
				else
				{
					_slugFrontAnimation.GetComponent < SkeletonAnimation > ().animationName = ANIMATION_NAME_SLUG_WALK_BACK;
					_slugFrontAnimation.GetComponent < SkeletonAnimation > ().animationName = ANIMATION_NAME_SLUG_WALK_FRONT;
				}
				break;
			case MOVE_ANIMATION_RIGHT:
				transform.localScale = new Vector3 ( _initialScale.x, _initialScale.y, _initialScale.z );
				_slugSideAnimation.SetActive ( true );
				_slugFrontAnimation.SetActive ( false );

				if ( _slugSideAnimation.GetComponent < SkeletonAnimation > ().animationName == ANIMATION_NAME_SLUG_MOVE_SIDE )
				{
					// do nothing
				}
				else
				{
					_slugSideAnimation.GetComponent < SkeletonAnimation > ().animationName = ANIMATION_NAME_SLUG_DIE_SIDE;
					_slugSideAnimation.GetComponent < SkeletonAnimation > ().animationName = ANIMATION_NAME_SLUG_MOVE_SIDE;
				}
				break;
			case MOVE_ANIMATION_LEFT:
				transform.localScale = new Vector3 ( -_initialScale.x, _initialScale.y, _initialScale.z );
				_slugSideAnimation.SetActive ( true );
				_slugFrontAnimation.SetActive ( false );

				if ( _slugSideAnimation.GetComponent < SkeletonAnimation > ().animationName == ANIMATION_NAME_SLUG_MOVE_SIDE )
				{
					// do nothing
				}
				else
				{
					_slugSideAnimation.GetComponent < SkeletonAnimation > ().animationName = ANIMATION_NAME_SLUG_DIE_SIDE;
					_slugSideAnimation.GetComponent < SkeletonAnimation > ().animationName = ANIMATION_NAME_SLUG_MOVE_SIDE;
				}
				break;
			case IDLE_ANIMATION_LEFT:
				_slugSideAnimation.SetActive ( true );
				_slugFrontAnimation.SetActive ( false );
				_slugSideAnimation.GetComponent < SkeletonAnimation > ().animationName = ANIMATION_NAME_SLUG_DIE_SIDE;
				_slugSideAnimation.GetComponent < SkeletonAnimation > ().animationName = ANIMATION_NAME_SLUG_MOVE_SIDE;
				transform.localScale = new Vector3 ( -_initialScale.x, _initialScale.y, _initialScale.z );
				break;
			case IDLE_ANIMATION_RIGHT:
				_slugSideAnimation.SetActive ( true );
				_slugFrontAnimation.SetActive ( false );
				_slugSideAnimation.GetComponent < SkeletonAnimation > ().animationName = ANIMATION_NAME_SLUG_DIE_SIDE;
				_slugSideAnimation.GetComponent < SkeletonAnimation > ().animationName = ANIMATION_NAME_SLUG_MOVE_SIDE;
				transform.localScale = new Vector3 ( _initialScale.x, _initialScale.y, _initialScale.z );
				break;
			case IDLE_ANIMATION_UP:
				transform.localScale = new Vector3 ( _initialScale.x, _initialScale.y, _initialScale.z );
				_slugSideAnimation.SetActive ( false );
				_slugFrontAnimation.SetActive ( true );
				_slugFrontAnimation.GetComponent < SkeletonAnimation > ().animationName = ANIMATION_NAME_SLUG_WALK_FRONT;
				_slugFrontAnimation.GetComponent < SkeletonAnimation > ().animationName = ANIMATION_NAME_SLUG_WALK_BACK;
				break;
			case IDLE_ANIMATION_DOWN:
				transform.localScale = new Vector3 ( _initialScale.x, _initialScale.y, _initialScale.z );
				_slugSideAnimation.SetActive ( false );
				_slugFrontAnimation.SetActive ( true );
				_slugFrontAnimation.GetComponent < SkeletonAnimation > ().animationName = ANIMATION_NAME_SLUG_WALK_BACK;
				_slugFrontAnimation.GetComponent < SkeletonAnimation > ().animationName = ANIMATION_NAME_SLUG_WALK_FRONT;
				break;
			}
		}
		else
		{
			switch ( _currentAnimationID )
			{
				case ATTACK_ANIMATION:
					iTween.Stop ( this.transform.parent.gameObject );
					transform.parent.rotation = VectorTools.cloneQuaternion ( _initialRotationQuaternion );
					_currentAnimationTextures = MemoryManager.getInstance ().reservedTextures[_myAnimationIDs[ATTACK_ANIMATION]];
					_frameID = 0;
					break;
				case DESTROY_ANIMATION:
//				print ("Tentacle Death");
					iTween.Stop ( this.transform.parent.gameObject );
					transform.parent.rotation = VectorTools.cloneQuaternion ( _initialRotationQuaternion );
					_currentAnimationTextures = MemoryManager.getInstance ().reservedTextures[_myAnimationIDs[DESTROY_ANIMATION]];
					_frameID = 0;
					break;
				case SPAWN_ANIMATION:
					switch ( _myIComponent.myID )
					{
						case GameElements.ENEM_TENTACLEDRAINER_01:
					//========================Daves Work=====================

							_currentAnimationTextures = MemoryManager.getInstance ().reservedTextures[_myAnimationIDs[DESTROY_ANIMATION]];
							_myMaterial.mainTexture = _currentAnimationTextures[0];
							_frameID = _currentAnimationTextures.Count - 1;
					//========================Daves Work=====================

							break;
						case GameElements.ENEM_TENTACLEDRAINER_02:
							_currentAnimationTextures = MemoryManager.getInstance ().reservedTextures[_myAnimationIDs[DESTROY_ANIMATION]];
							_myMaterial.mainTexture = _currentAnimationTextures[0];
							_frameID = _currentAnimationTextures.Count - 1;
							break;
						case GameElements.ENEM_SLUG_01:
							transform.localScale = new Vector3 ( _initialScale.x, _initialScale.y, _initialScale.z );
							_currentAnimationTextures = MemoryManager.getInstance ().reservedTextures[_myAnimationIDs[SPAWN_ANIMATION]];
							_myMaterial.mainTexture = _currentAnimationTextures[0];
							_frameID = 0;
							break;
					}
					break;
				case ATTACK_ANIMATION_UP:
					transform.localScale = new Vector3 ( _initialScale.x, _initialScale.y, _initialScale.z );
					_currentAnimationTextures = MemoryManager.getInstance ().reservedTextures[_myAnimationIDs[ATTACK_ANIMATION_UP]];
					_frameID = 0;
					break;
				case ATTACK_ANIMATION_DOWN:
					transform.localScale = new Vector3 ( _initialScale.x, _initialScale.y, _initialScale.z );
					_currentAnimationTextures = MemoryManager.getInstance ().reservedTextures[_myAnimationIDs[ATTACK_ANIMATION_DOWN]];
					_frameID = 0;
					break;
				case ATTACK_ANIMATION_RIGHT:
					transform.localScale = new Vector3 ( _initialScale.x, _initialScale.y, _initialScale.z );
					_currentAnimationTextures = MemoryManager.getInstance ().reservedTextures[_myAnimationIDs[ATTACK_ANIMATION_RIGHT]];
					_frameID = 0;
					break;
				case ATTACK_ANIMATION_LEFT:
					transform.localScale = new Vector3 ( -_initialScale.x, _initialScale.y, _initialScale.z );
					_currentAnimationTextures = MemoryManager.getInstance ().reservedTextures[_myAnimationIDs[ATTACK_ANIMATION_RIGHT]];
					_frameID = 0;
					break;
				case MOVE_ANIMATION_UP:
					transform.localScale = new Vector3 ( _initialScale.x, _initialScale.y, _initialScale.z );
					_currentAnimationTextures = MemoryManager.getInstance ().reservedTextures[_myAnimationIDs[MOVE_ANIMATION_UP]];
					_frameID = 0;
					break;
				case MOVE_ANIMATION_DOWN:
					transform.localScale = new Vector3 ( _initialScale.x, _initialScale.y, _initialScale.z );
					_currentAnimationTextures = MemoryManager.getInstance ().reservedTextures[_myAnimationIDs[MOVE_ANIMATION_DOWN]];
					_frameID = 0;
					break;
				case MOVE_ANIMATION_RIGHT:
					transform.localScale = new Vector3 ( _initialScale.x, _initialScale.y, _initialScale.z );
					_currentAnimationTextures = MemoryManager.getInstance ().reservedTextures[_myAnimationIDs[MOVE_ANIMATION_RIGHT]];
					_frameID = 0;
					break;
				case MOVE_ANIMATION_LEFT:
					transform.localScale = new Vector3 ( -_initialScale.x, _initialScale.y, _initialScale.z );
					_currentAnimationTextures = MemoryManager.getInstance ().reservedTextures[_myAnimationIDs[MOVE_ANIMATION_RIGHT]];
					_frameID = 0;
					break;
				case IDLE_ANIMATION_LEFT:
					transform.localScale = new Vector3 ( -_initialScale.x, _initialScale.y, _initialScale.z );
					_currentAnimationTextures = MemoryManager.getInstance ().reservedTextures[_myAnimationIDs[IDLE_ANIMATION_RIGHT]];
					_currentIdleDirection = _LEFT;
					_frameID = 0;
					break;
				case IDLE_ANIMATION_RIGHT:
					transform.localScale = new Vector3 ( _initialScale.x, _initialScale.y, _initialScale.z );
					_currentAnimationTextures = MemoryManager.getInstance ().reservedTextures[_myAnimationIDs[IDLE_ANIMATION_RIGHT]];
					_currentIdleDirection = _RIGHT;
					_frameID = 0;
					break;
				//======================Daves Work===================
				case ATTACK_ANIMATION_CHARGE:
					transform.localScale = new Vector3 ( _initialScale.x, _initialScale.y, _initialScale.z );
					_currentAnimationTextures = MemoryManager.getInstance ().reservedTextures[_myAnimationIDs[ATTACK_ANIMATION_CHARGE]];
					_frameID = 0;
					break;
				case ATTACK_ANIMATION_HOLD:
					transform.localScale = new Vector3 ( _initialScale.x, _initialScale.y, _initialScale.z );
					_currentAnimationTextures = MemoryManager.getInstance ().reservedTextures[_myAnimationIDs[ATTACK_ANIMATION_HOLD]];
					_frameID = 0;
					break;
				case ATTACK_ANIMATION_CHARGE_LEFT:
					transform.localScale = new Vector3 ( -_initialScale.x, _initialScale.y, _initialScale.z );
					_currentAnimationTextures = MemoryManager.getInstance ().reservedTextures[_myAnimationIDs[ATTACK_ANIMATION_CHARGE]];
					_frameID = 0;
					break;
				case ATTACK_ANIMATION_HOLD_LEFT:
					transform.localScale = new Vector3 ( -_initialScale.x, _initialScale.y, _initialScale.z );
					_currentAnimationTextures = MemoryManager.getInstance ().reservedTextures[_myAnimationIDs[ATTACK_ANIMATION_HOLD]];
					_frameID = 0;
					break;
				//======================Daves Work===================
			}
		}
	}
	
	void Update () 
	{
		if ( _myIComponent.myID == GameElements.ENEM_SLUG_01 )
		{
			if ( _destoryAnimation )
			{
				transform.localScale = new Vector3 ( _initialScale.x, _initialScale.y, _initialScale.z );
				_slugSideAnimation.SetActive ( true );
				_slugFrontAnimation.SetActive ( false );
				_slugSideAnimation.GetComponent < SkeletonAnimation > ().animationName = ANIMATION_NAME_SLUG_DIE_SIDE;
				iTween.Stop ( this.transform.gameObject );
			}
		}
		else
		{
			_countTimeToNextFrame += Time.deltaTime;
			
			if ( _countTimeToNextFrame >= ( 1f / 24f ))
			{
				_countTimeToNextFrame = 0f;
			
				if ( _currentAnimationID == -1 )
				{
					switch ( _myIComponent.myID )
					{
						case GameElements.ENEM_TENTACLEDRAINER_01:
						case GameElements.ENEM_TENTACLEDRAINER_02:
							_myMaterial.mainTexture = _currentAnimationTextures[_frameID];
							_frameID++;
							if ( _frameID >= _currentAnimationTextures.Count ) _frameID = 0;
							break;
						case GameElements.ENEM_SLUG_01:
							_myMaterial.mainTexture = _currentAnimationTextures[_frameID];
							_frameID++;
							if ( _frameID >= _currentAnimationTextures.Count ) _frameID = 0;
							break;
					}
				}
				else
				{
					if ( _currentAnimationTextures.Count < _frameID + 1 )
					{
					}
					else _myMaterial.mainTexture = _currentAnimationTextures[_frameID];
					
					if ( _currentAnimationID == SPAWN_ANIMATION )
					{	
						switch ( _myIComponent.myID )
						{
							case GameElements.ENEM_TENTACLEDRAINER_01:
							case GameElements.ENEM_TENTACLEDRAINER_02:
								_frameID--;
								if ( _frameID <= 0 )
								{
									_currentAnimationID = -1;
									_currentAnimationTextures = MemoryManager.getInstance ().reservedTextures[_myAnimationIDs[IDLE_ANIMATION]];
									//iTween.RotateTo ( this.transform.parent.gameObject, iTween.Hash ( "time", 0.5f, "easetype", iTween.EaseType.linear, "rotation", new Vector3 ( _initialRotation.x, _initialRotation.y + 0.75f, _initialRotation.z ), "oncompletetarget", this.gameObject, "oncomplete", "onCompleteTweenAnimationRotateToRight" ));
									_frameID = 0;
								}
								break;
							case GameElements.ENEM_SLUG_01:
								_frameID++;
								if ( _frameID >= _currentAnimationTextures.Count )
								{
									_currentAnimationID = -1;
									_frameID = 0;
									if ( _currentIdleDirection == _LEFT )
									{
										transform.localScale = new Vector3 ( -_initialScale.x, _initialScale.y, _initialScale.z );
										_currentAnimationTextures = MemoryManager.getInstance ().reservedTextures[_myAnimationIDs[IDLE_ANIMATION_RIGHT]];
									}
									else
									{
										transform.localScale = new Vector3 ( _initialScale.x, _initialScale.y, _initialScale.z );
										_currentAnimationTextures = MemoryManager.getInstance ().reservedTextures[_myAnimationIDs[IDLE_ANIMATION_RIGHT]];
									}
								}
								break;
						}
					}
					else
					{
						_frameID++;
						
						if ( _frameID >= _currentAnimationTextures.Count )
						{
							if ( _currentAnimationID == DESTROY_ANIMATION )
							{
								currentAttackingCharacter.interactAction = false;
								if ( GameGlobalVariables.CURRENT_GAME_PART == GameGlobalVariables.RESCUE )
								{
//									print ("called");
									Main.getInstance ().handleObjectDestory ( _myIComponent.myID, transform.parent.gameObject );
								}
								else if ( GameGlobalVariables.CURRENT_GAME_PART == GameGlobalVariables.MINING )
								{
									MNMain.getInstance ().handleObjectDestory ( _myIComponent.myID, transform.parent.gameObject );
								}
							}
							switch ( _myIComponent.myID )
							{
								case GameElements.ENEM_TENTACLEDRAINER_01:
								case GameElements.ENEM_TENTACLEDRAINER_02:
									_currentAnimationTextures = MemoryManager.getInstance ().reservedTextures[_myAnimationIDs[IDLE_ANIMATION]];
									//iTween.RotateTo ( this.transform.parent.gameObject, iTween.Hash ( "time", 0.5f, "easetype", iTween.EaseType.linear, "rotation", new Vector3 ( _initialRotation.x, _initialRotation.y + 0.75f, _initialRotation.z ), "oncompletetarget", this.gameObject, "oncomplete", "onCompleteTweenAnimationRotateToRight" ));
									break;
								case GameElements.ENEM_SLUG_01:
									if ( _currentAnimationID == MOVE_ANIMATION_LEFT || _currentAnimationID == MOVE_ANIMATION_RIGHT || _currentAnimationID == MOVE_ANIMATION_UP || _currentAnimationID == MOVE_ANIMATION_DOWN )
									{
										
									}
									else
									{
										if ( _currentIdleDirection == _LEFT )
										{
											transform.localScale = new Vector3 ( -_initialScale.x, _initialScale.y, _initialScale.z );
											_currentAnimationTextures = MemoryManager.getInstance ().reservedTextures[_myAnimationIDs[IDLE_ANIMATION_RIGHT]];
										}
										else
										{
											transform.localScale = new Vector3 ( _initialScale.x, _initialScale.y, _initialScale.z );
											_currentAnimationTextures = MemoryManager.getInstance ().reservedTextures[_myAnimationIDs[IDLE_ANIMATION_RIGHT]];
										}
									}
									break;
							}
							
							if ( _currentAnimationID == MOVE_ANIMATION_LEFT || _currentAnimationID == MOVE_ANIMATION_RIGHT || _currentAnimationID == MOVE_ANIMATION_UP || _currentAnimationID == MOVE_ANIMATION_DOWN )
							{
								
							}
							else
							{
								_currentAnimationID = -1;
							}
							
							_frameID = 0;
						}
					}
				}
			}
		}
	}
	
	private void onCompleteTweenAnimationRotateToRight ()
	{
		//iTween.RotateTo ( this.transform.parent.gameObject, iTween.Hash ( "time", 0.5f, "easetype", iTween.EaseType.linear, "rotation", new Vector3 ( _initialRotation.x, _initialRotation.y - 1.5f, _initialRotation.z ), "oncompletetarget", this.gameObject, "oncomplete", "onCompleteTweenAnimationRotateToLeft" ));
	}
	
	private void onCompleteTweenAnimationRotateToLeft ()
	{
		//iTween.RotateTo ( this.transform.parent.gameObject, iTween.Hash ( "time", 0.5f, "easetype", iTween.EaseType.linear, "rotation", new Vector3 ( _initialRotation.x, _initialRotation.y + 1.5f, _initialRotation.z ), "oncompletetarget", this.gameObject, "oncomplete", "onCompleteTweenAnimationRotateToRight" ));
	}
}
