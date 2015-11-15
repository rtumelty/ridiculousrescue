using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class TREventsManager : MonoBehaviour
{
	//*************************************************************//	
	public class TrainEventClass 
	{
		//*************************************************************//
		public const int TYPE_STONE = 0;
		public const int TYPE_BAT = 1;
		public const int TYPE_TENTACLE = 2;
		//*************************************************************//
		public float meterOfOccure;
		public int type;

		public TrainEventClass ( float meterOfOccureValue, int typeValue )
		{
			meterOfOccure = meterOfOccureValue;
			type = typeValue;
		}
	}
	//*************************************************************//
	public class BatOnLevelClass
	{
		public GameObject batObject;
		public CharacterData myCharacterToAttack;

		public BatOnLevelClass ( GameObject batObjectValue, CharacterData myCharacterToAttackValue ) 
		{
			batObject = batObjectValue;
			myCharacterToAttack = myCharacterToAttackValue;
		}
	}
	//*************************************************************//
	public const int TUTORIAL_ID_TAP_CORA = 1;
	public const int TUTORIAL_ID_SWIPE_TENTACLE = 2;
	public const int TUTORIAL_ID_TAP_ROCK = 3;
	public const int TUTORIAL_ID_SWIPE_CORA = 4;
	//*************************************************************//
	public List < TrainEventClass > events;
	public List < GameObject > stonesOnLevel;
	public List < BatOnLevelClass > batsOnLevel;
	public GameObject tentacleOnlevel;
	public GameObject faradaydo;
	public GameObject cora;
	public GameObject jose;
	public CharacterData faradyadoCharacterData;
	public CharacterData coraCharacterData;
	public CharacterData joseCharacterData;
	public bool faradaydoHasBatAbove = false;
	public bool coraHasBatAbove = false;
	public bool joseHasBatAbove = false;

	public Texture2D tentacleRoad01;
	public Texture2D tentacleRoad02;
	public Texture2D tentacleRoad03;
	public Texture2D tentacleRoad04;

	public GameObject tentalceParticle;

	public bool start = false;

	public bool tutorialTapCoraPlayed = false;
	public bool tutorialSwipeTentaclePlayed = false;
	public bool tutorialTapRockPlayed = false;
	public bool tutorialSwipeCoraPlayed = false;
	//*************************************************************//
	private int _currentTutorialID = 0;
	private bool _lastTutrialScreenPlayed = false;

	private GameObject _stonePrefab;
	private GameObject _batPrefab;
	private GameObject _tentaclePrefab;
	private GameObject _tutorialComboUIPrefab;
	private GameObject _tutorialUIComboObjectInstant;
	private GameObject _tutorialHandPrefab;
	private GameObject _tutorialHandInstant;
	public bool testOn = false;
	//*************************************************************//	
	private static TREventsManager _meInstance;
	public static TREventsManager getInstance ()
	{
		return _meInstance;
	}
	//*************************************************************//
	void Awake () 
	{
		TREventsManager._meInstance = this;

		batsOnLevel = new List < BatOnLevelClass > ();

		_stonePrefab = ( GameObject ) Resources.Load ( "Tile/stone" );
		_batPrefab = ( GameObject ) Resources.Load ( "Spine/bat/batPrefab" );
		_tentaclePrefab = ( GameObject ) Resources.Load ( "Tile/tentacle" );

		_tutorialComboUIPrefab = ( GameObject ) Resources.Load ( "UI/TRTutorialComboUIDemi" );
		_tutorialHandPrefab = ( GameObject ) Resources.Load ( "UI/hand" );

		// to prevent delay in loading tentacle in runtime
		GameObject tentacleInstant = ( GameObject ) Instantiate ( _tentaclePrefab, new Vector3 ( -100f, -100f, -100f ), _tentaclePrefab.transform.rotation );
		GameObject rockObjectPrefab = ( GameObject ) Resources.Load ( "Tile/tentacleRock" );
		GameObject rockObject = ( GameObject ) Instantiate ( rockObjectPrefab, tentacleInstant.transform.position + Vector3.up * 0.25f + Vector3.forward * 1f, rockObjectPrefab.transform.rotation );
		rockObject.AddComponent < EnemyTentacleRockControl > ().parentGameObject = tentacleInstant;
		rockObject.GetComponent < EnemyTentacleRockControl > ().addPosition = Vector3.back * 0.8f; 
		tentacleInstant.AddComponent < TRTentacleControl > ().dummyScript = true;

		rockObject.SetActive ( false );
	}
	
	void Start ()
	{
		events = new List < TrainEventClass > ();
		switch ( TRLevelControl.LEVEL_ID )
		{
		case 1:
			events.Add  ( new TrainEventClass ( 150f, TrainEventClass.TYPE_TENTACLE ));			
			events.Add  ( new TrainEventClass ( 375f, TrainEventClass.TYPE_TENTACLE ));
			events.Add  ( new TrainEventClass ( 525f, TrainEventClass.TYPE_BAT));
			events.Add  ( new TrainEventClass ( 675f, TrainEventClass.TYPE_TENTACLE ));
			events.Add  ( new TrainEventClass ( 750f, TrainEventClass.TYPE_BAT ));
			events.Add  ( new TrainEventClass ( 900f, TrainEventClass.TYPE_BAT ));
			events.Add  ( new TrainEventClass ( 975f, TrainEventClass.TYPE_BAT ));
			events.Add  ( new TrainEventClass ( 1050f, TrainEventClass.TYPE_TENTACLE ));
			events.Add  ( new TrainEventClass ( 1350f, TrainEventClass.TYPE_BAT ));
			events.Add  ( new TrainEventClass ( 1380f, TrainEventClass.TYPE_TENTACLE ));
			events.Add  ( new TrainEventClass ( 1410f, TrainEventClass.TYPE_BAT ));
			events.Add  ( new TrainEventClass ( 1500f, TrainEventClass.TYPE_TENTACLE ));
			events.Add  ( new TrainEventClass ( 1575f, TrainEventClass.TYPE_BAT ));
			events.Add  ( new TrainEventClass ( 1605f, TrainEventClass.TYPE_BAT ));
			events.Add  ( new TrainEventClass ( 1635f, TrainEventClass.TYPE_BAT ));
			events.Add  ( new TrainEventClass ( 1650f, TrainEventClass.TYPE_BAT ));
			events.Add  ( new TrainEventClass ( 1680f, TrainEventClass.TYPE_BAT ));
			break;

		case 2:
			events.Add  ( new TrainEventClass ( 75f, TrainEventClass.TYPE_TENTACLE ));			
			events.Add  ( new TrainEventClass ( 105f, TrainEventClass.TYPE_BAT ));
			events.Add  ( new TrainEventClass ( 150f, TrainEventClass.TYPE_STONE ));
			events.Add  ( new TrainEventClass ( 165f, TrainEventClass.TYPE_STONE ));
			events.Add  ( new TrainEventClass ( 180f, TrainEventClass.TYPE_STONE ));
			events.Add  ( new TrainEventClass ( 270f, TrainEventClass.TYPE_BAT ));
			events.Add  ( new TrainEventClass ( 285f, TrainEventClass.TYPE_TENTACLE ));
			events.Add  ( new TrainEventClass ( 300f, TrainEventClass.TYPE_STONE ));
			events.Add  ( new TrainEventClass ( 330f, TrainEventClass.TYPE_STONE ));
			events.Add  ( new TrainEventClass ( 345f, TrainEventClass.TYPE_TENTACLE ));
			events.Add  ( new TrainEventClass ( 360f, TrainEventClass.TYPE_BAT ));
			events.Add  ( new TrainEventClass ( 390f, TrainEventClass.TYPE_BAT ));
			events.Add  ( new TrainEventClass ( 405f, TrainEventClass.TYPE_BAT ));
			events.Add  ( new TrainEventClass ( 420f, TrainEventClass.TYPE_BAT ));
			events.Add  ( new TrainEventClass ( 435f, TrainEventClass.TYPE_STONE ));
			events.Add  ( new TrainEventClass ( 480f, TrainEventClass.TYPE_BAT ));
			events.Add  ( new TrainEventClass ( 495f, TrainEventClass.TYPE_BAT ));
			events.Add  ( new TrainEventClass ( 510f, TrainEventClass.TYPE_BAT ));
			events.Add  ( new TrainEventClass ( 540f, TrainEventClass.TYPE_TENTACLE ));
			events.Add  ( new TrainEventClass ( 570f, TrainEventClass.TYPE_STONE ));
			events.Add  ( new TrainEventClass ( 600f, TrainEventClass.TYPE_TENTACLE ));
			events.Add  ( new TrainEventClass ( 630f, TrainEventClass.TYPE_STONE ));
			events.Add  ( new TrainEventClass ( 690f, TrainEventClass.TYPE_BAT ));
			events.Add  ( new TrainEventClass ( 705f, TrainEventClass.TYPE_BAT ));
			events.Add  ( new TrainEventClass ( 720f, TrainEventClass.TYPE_STONE ));
			events.Add  ( new TrainEventClass ( 735f, TrainEventClass.TYPE_STONE ));
			events.Add  ( new TrainEventClass ( 765f, TrainEventClass.TYPE_TENTACLE ));
			events.Add  ( new TrainEventClass ( 780f, TrainEventClass.TYPE_BAT ));
			events.Add  ( new TrainEventClass ( 795f, TrainEventClass.TYPE_STONE ));
			events.Add  ( new TrainEventClass ( 810f, TrainEventClass.TYPE_STONE ));
			events.Add  ( new TrainEventClass ( 855f, TrainEventClass.TYPE_TENTACLE ));
			events.Add  ( new TrainEventClass ( 870f, TrainEventClass.TYPE_BAT ));
			events.Add  ( new TrainEventClass ( 885f, TrainEventClass.TYPE_BAT ));
			events.Add  ( new TrainEventClass ( 990f, TrainEventClass.TYPE_TENTACLE ));
			events.Add  ( new TrainEventClass ( 1005f, TrainEventClass.TYPE_STONE ));
			events.Add  ( new TrainEventClass ( 1020f, TrainEventClass.TYPE_STONE ));
			events.Add  ( new TrainEventClass ( 1035f, TrainEventClass.TYPE_BAT ));
			events.Add  ( new TrainEventClass ( 1050f, TrainEventClass.TYPE_STONE ));
			events.Add  ( new TrainEventClass ( 1065f, TrainEventClass.TYPE_BAT ));
			events.Add  ( new TrainEventClass ( 1080f, TrainEventClass.TYPE_BAT ));
			events.Add  ( new TrainEventClass ( 1155f, TrainEventClass.TYPE_STONE ));
			events.Add  ( new TrainEventClass ( 1170f, TrainEventClass.TYPE_STONE ));
			events.Add  ( new TrainEventClass ( 1185f, TrainEventClass.TYPE_BAT ));
			events.Add  ( new TrainEventClass ( 1200f, TrainEventClass.TYPE_BAT ));
			events.Add  ( new TrainEventClass ( 1215f, TrainEventClass.TYPE_BAT ));
			events.Add  ( new TrainEventClass ( 1230f, TrainEventClass.TYPE_TENTACLE ));
			events.Add  ( new TrainEventClass ( 1245f, TrainEventClass.TYPE_BAT ));
			events.Add  ( new TrainEventClass ( 1275f, TrainEventClass.TYPE_BAT ));
			events.Add  ( new TrainEventClass ( 1305f, TrainEventClass.TYPE_STONE ));
			events.Add  ( new TrainEventClass ( 1335f, TrainEventClass.TYPE_BAT ));
			events.Add  ( new TrainEventClass ( 1395f, TrainEventClass.TYPE_BAT ));
			events.Add  ( new TrainEventClass ( 1410f, TrainEventClass.TYPE_TENTACLE ));
			events.Add  ( new TrainEventClass ( 1425f, TrainEventClass.TYPE_BAT ));
			events.Add  ( new TrainEventClass ( 1455f, TrainEventClass.TYPE_STONE ));
			events.Add  ( new TrainEventClass ( 1470f, TrainEventClass.TYPE_TENTACLE ));
			events.Add  ( new TrainEventClass ( 1485f, TrainEventClass.TYPE_STONE ));
			events.Add  ( new TrainEventClass ( 1530f, TrainEventClass.TYPE_TENTACLE ));
			events.Add  ( new TrainEventClass ( 1545f, TrainEventClass.TYPE_BAT ));
			events.Add  ( new TrainEventClass ( 1560f, TrainEventClass.TYPE_STONE ));
			events.Add  ( new TrainEventClass ( 1575f, TrainEventClass.TYPE_STONE ));
			events.Add  ( new TrainEventClass ( 1605f, TrainEventClass.TYPE_BAT ));
			events.Add  ( new TrainEventClass ( 1620f, TrainEventClass.TYPE_BAT ));
			events.Add  ( new TrainEventClass ( 1650f, TrainEventClass.TYPE_BAT ));
			events.Add  ( new TrainEventClass ( 1665f, TrainEventClass.TYPE_BAT ));
			break;

		case 3:
			events.Add  ( new TrainEventClass ( 95f, TrainEventClass.TYPE_STONE ));
			events.Add  ( new TrainEventClass ( 105f, TrainEventClass.TYPE_STONE ));
			events.Add  ( new TrainEventClass ( 135f, TrainEventClass.TYPE_STONE ));
			events.Add  ( new TrainEventClass ( 150f, TrainEventClass.TYPE_BAT ));
			events.Add  ( new TrainEventClass ( 165f, TrainEventClass.TYPE_STONE ));
			events.Add  ( new TrainEventClass ( 180f, TrainEventClass.TYPE_BAT ));
			events.Add  ( new TrainEventClass ( 240f, TrainEventClass.TYPE_STONE ));
			events.Add  ( new TrainEventClass ( 255f, TrainEventClass.TYPE_STONE ));
			events.Add  ( new TrainEventClass ( 270f, TrainEventClass.TYPE_STONE ));
			events.Add  ( new TrainEventClass ( 300f, TrainEventClass.TYPE_BAT ));
			events.Add  ( new TrainEventClass ( 315f, TrainEventClass.TYPE_BAT ));
			events.Add  ( new TrainEventClass ( 330f, TrainEventClass.TYPE_TENTACLE ));
			events.Add  ( new TrainEventClass ( 390f, TrainEventClass.TYPE_TENTACLE ));
			events.Add  ( new TrainEventClass ( 435f, TrainEventClass.TYPE_TENTACLE ));
			events.Add  ( new TrainEventClass ( 480f, TrainEventClass.TYPE_TENTACLE ));
			events.Add  ( new TrainEventClass ( 495f, TrainEventClass.TYPE_BAT ));
			events.Add  ( new TrainEventClass ( 525f, TrainEventClass.TYPE_TENTACLE ));
			events.Add  ( new TrainEventClass ( 540f, TrainEventClass.TYPE_BAT ));
			events.Add  ( new TrainEventClass ( 555f, TrainEventClass.TYPE_STONE ));
			events.Add  ( new TrainEventClass ( 585f, TrainEventClass.TYPE_TENTACLE ));
			events.Add  ( new TrainEventClass ( 600f, TrainEventClass.TYPE_BAT ));
			events.Add  ( new TrainEventClass ( 615f, TrainEventClass.TYPE_STONE ));
			events.Add  ( new TrainEventClass ( 630f, TrainEventClass.TYPE_STONE ));
			events.Add  ( new TrainEventClass ( 660f, TrainEventClass.TYPE_STONE ));
			events.Add  ( new TrainEventClass ( 675f, TrainEventClass.TYPE_STONE ));
			events.Add  ( new TrainEventClass ( 690f, TrainEventClass.TYPE_STONE ));
			events.Add  ( new TrainEventClass ( 705f, TrainEventClass.TYPE_STONE ));
			events.Add  ( new TrainEventClass ( 720f, TrainEventClass.TYPE_BAT ));
			events.Add  ( new TrainEventClass ( 735f, TrainEventClass.TYPE_BAT ));
			events.Add  ( new TrainEventClass ( 750f, TrainEventClass.TYPE_BAT ));
			events.Add  ( new TrainEventClass ( 840f, TrainEventClass.TYPE_BAT ));
			events.Add  ( new TrainEventClass ( 855f, TrainEventClass.TYPE_BAT ));
			events.Add  ( new TrainEventClass ( 870f, TrainEventClass.TYPE_BAT ));
			events.Add  ( new TrainEventClass ( 885f, TrainEventClass.TYPE_STONE ));
			events.Add  ( new TrainEventClass ( 900f, TrainEventClass.TYPE_TENTACLE ));
			events.Add  ( new TrainEventClass ( 960f, TrainEventClass.TYPE_BAT ));
			events.Add  ( new TrainEventClass ( 975f, TrainEventClass.TYPE_BAT ));
			events.Add  ( new TrainEventClass ( 990f, TrainEventClass.TYPE_BAT ));
			events.Add  ( new TrainEventClass ( 1005f, TrainEventClass.TYPE_STONE ));
			events.Add  ( new TrainEventClass ( 1020f, TrainEventClass.TYPE_TENTACLE ));
			events.Add  ( new TrainEventClass ( 1065f, TrainEventClass.TYPE_STONE ));
			events.Add  ( new TrainEventClass ( 1095f, TrainEventClass.TYPE_STONE ));
			events.Add  ( new TrainEventClass ( 1110f, TrainEventClass.TYPE_STONE ));
			events.Add  ( new TrainEventClass ( 1140f, TrainEventClass.TYPE_BAT ));
			events.Add  ( new TrainEventClass ( 1155f, TrainEventClass.TYPE_BAT ));
			events.Add  ( new TrainEventClass ( 1170f, TrainEventClass.TYPE_BAT ));
			events.Add  ( new TrainEventClass ( 1215f, TrainEventClass.TYPE_TENTACLE ));
			events.Add  ( new TrainEventClass ( 1230f, TrainEventClass.TYPE_BAT ));
			events.Add  ( new TrainEventClass ( 1245f, TrainEventClass.TYPE_BAT ));
			events.Add  ( new TrainEventClass ( 1260f, TrainEventClass.TYPE_STONE ));
			events.Add  ( new TrainEventClass ( 1275f, TrainEventClass.TYPE_STONE ));
			events.Add  ( new TrainEventClass ( 1320f, TrainEventClass.TYPE_STONE ));
			events.Add  ( new TrainEventClass ( 1350f, TrainEventClass.TYPE_BAT ));
			events.Add  ( new TrainEventClass ( 1380f, TrainEventClass.TYPE_STONE ));
			events.Add  ( new TrainEventClass ( 1410f, TrainEventClass.TYPE_BAT ));
			events.Add  ( new TrainEventClass ( 1440f, TrainEventClass.TYPE_STONE ));
			events.Add  ( new TrainEventClass ( 1470f, TrainEventClass.TYPE_TENTACLE ));
			events.Add  ( new TrainEventClass ( 1485f, TrainEventClass.TYPE_BAT ));
			events.Add  ( new TrainEventClass ( 1500f, TrainEventClass.TYPE_BAT ));
			events.Add  ( new TrainEventClass ( 1515f, TrainEventClass.TYPE_BAT ));
			events.Add  ( new TrainEventClass ( 1530f, TrainEventClass.TYPE_STONE ));
			events.Add  ( new TrainEventClass ( 1545f, TrainEventClass.TYPE_STONE ));
			events.Add  ( new TrainEventClass ( 1575f, TrainEventClass.TYPE_TENTACLE ));
			events.Add  ( new TrainEventClass ( 1590f, TrainEventClass.TYPE_STONE ));
			events.Add  ( new TrainEventClass ( 1605f, TrainEventClass.TYPE_TENTACLE ));
			events.Add  ( new TrainEventClass ( 1635f, TrainEventClass.TYPE_BAT ));
			events.Add  ( new TrainEventClass ( 1650f, TrainEventClass.TYPE_BAT ));
			events.Add  ( new TrainEventClass ( 1665f, TrainEventClass.TYPE_BAT ));
			events.Add  ( new TrainEventClass ( 1680f, TrainEventClass.TYPE_BAT ));
			break;
		}
	}

	void Update () 
	{
		if ( ! start ) return;

		if ( events.Count > 0 && TRSpeedAndTrackOMetersManager.getInstance ().getTrackDistance () >= events[0].meterOfOccure )
		{
			createEvent ( events[0].type );
			events.Remove ( events[0] );
		}

		int countStones = 0;
		for ( int i = 0; i < stonesOnLevel.Count; i++ )
		{
			if ( stonesOnLevel[i] == null )
			{
				stonesOnLevel.RemoveAt ( i );
				if ( stonesOnLevel.Count > 0 ) stonesOnLevel[0].SendMessage ( "removeAdditionalX" );
			}
			else
			{
				countStones++;
				if ( countStones > 5 )
				{
					Destroy ( stonesOnLevel[i] );
				}
			}
		}
	}

	private void createEvent ( int type )
	{
		GameObject batObject = null;

		if ( _currentTutorialID != 0 ) return;

		switch ( type )
		{
			case TrainEventClass.TYPE_TENTACLE:
			print ("Taint");
			if ( GameGlobalVariables.ACTIVE_TENTACLE == false )
			{
				print ("Tis Null");
				GameObject tentacleInstant = ( GameObject ) Instantiate ( _tentaclePrefab, new Vector3 ( 15f, 12f, 3.4f ), _tentaclePrefab.transform.rotation );
				tentacleOnlevel = tentacleInstant;
				GameObject rockObjectPrefab = ( GameObject ) Resources.Load ( "Tile/tentacleRock" );
				GameObject rockObject = ( GameObject ) Instantiate ( rockObjectPrefab, tentacleInstant.transform.position + Vector3.up * 0.25f + Vector3.forward * 1f, rockObjectPrefab.transform.rotation );
				rockObject.AddComponent < EnemyTentacleRockControl > ().parentGameObject = tentacleInstant;
				rockObject.GetComponent < EnemyTentacleRockControl > ().addPosition = Vector3.back * 0.8f; 
				tentacleInstant.AddComponent < TRTentacleControl > ();

				if ( TRTutorialManager.getInstance ().getCurrentTutorialID () == 1 && ! tutorialSwipeTentaclePlayed )
				{
					_tutorialUIComboObjectInstant = ( GameObject ) Instantiate ( _tutorialComboUIPrefab, Vector3.zero, _tutorialComboUIPrefab.transform.rotation );
					_tutorialUIComboObjectInstant.transform.parent = Camera.main.transform;
					_tutorialUIComboObjectInstant.transform.localPosition = new Vector3 ( 0f, 2.0f, 2f ) + Vector3.forward * 3f;
					
					_tutorialUIComboObjectInstant.transform.Find ( "frameText" ).GetComponent < GameTextControl > ().myKey = "train_tutorial_swipe_across";
					_tutorialUIComboObjectInstant.transform.Find ( "tapText" ).renderer.enabled = false;
					_tutorialUIComboObjectInstant.transform.Find ( "frame" ).collider.enabled = false;
					
					_tutorialHandInstant = ( GameObject ) Instantiate ( _tutorialHandPrefab, new Vector3 ( tentacleInstant.transform.position.x - 1f, tentacleInstant.transform.position.y + 1f, tentacleInstant.transform.position.z - 1.5f ), _tutorialHandPrefab.transform.rotation );
					_tutorialHandInstant.transform.parent = tentacleInstant.transform;
					_tutorialHandInstant.AddComponent < SimulateDrag > ().targetPosition = VectorTools.cloneVector3 (  new Vector3 ( _tutorialHandInstant.transform.localPosition.x - 3f, _tutorialHandInstant.transform.localPosition.y, _tutorialHandInstant.transform.localPosition.z ) );
					
					_currentTutorialID = TUTORIAL_ID_SWIPE_TENTACLE;
					tutorialSwipeTentaclePlayed = true;
				}
			}
			break;
			case TrainEventClass.TYPE_STONE:
				GameObject _stoneObject = ( GameObject ) Instantiate ( _stonePrefab, new Vector3 ( 15f, 8f, 3.8f ), _stonePrefab.transform.rotation );
				stonesOnLevel.Add ( _stoneObject );
				if ( stonesOnLevel.Count > 1 ) _stoneObject.GetComponent < TRStoneControl > ().doNotProduceButton = true;
				
				if ( TRTutorialManager.getInstance ().getCurrentTutorialID () == 2 && ! tutorialTapRockPlayed )
				{
					_tutorialUIComboObjectInstant = ( GameObject ) Instantiate ( _tutorialComboUIPrefab, Vector3.zero, _tutorialComboUIPrefab.transform.rotation );
					_tutorialUIComboObjectInstant.transform.parent = Camera.main.transform;
					_tutorialUIComboObjectInstant.transform.localPosition = new Vector3 ( 0f, -2.0f, 2f ) + Vector3.forward * 3f;
					
					_tutorialUIComboObjectInstant.transform.Find ( "frameText" ).GetComponent < GameTextControl > ().myKey = "train_tutorial_repeatedly";
					_tutorialUIComboObjectInstant.transform.Find ( "tapText" ).renderer.enabled = false;
					_tutorialUIComboObjectInstant.transform.Find ( "frame" ).collider.enabled = false;
					
					_currentTutorialID = TUTORIAL_ID_TAP_ROCK;
					tutorialTapRockPlayed = true;
				}
				break;

			case TrainEventClass.TYPE_BAT:
			{
				int randomCharacter =  UnityEngine.Random.Range ( 0, 3 );

				if ( TRTutorialManager.getInstance ().getCurrentTutorialID () == 1 && ! tutorialTapCoraPlayed )
				{
					_tutorialUIComboObjectInstant = ( GameObject ) Instantiate ( _tutorialComboUIPrefab, Vector3.zero, _tutorialComboUIPrefab.transform.rotation );
					_tutorialUIComboObjectInstant.transform.parent = Camera.main.transform;
					_tutorialUIComboObjectInstant.transform.localPosition = new Vector3 ( 0f, -2.0f, 2f ) + Vector3.forward * 3f;
						
					_tutorialUIComboObjectInstant.transform.Find ( "frameText" ).GetComponent < GameTextControl > ().myKey = "train_tutorial_tap_cora";
					_tutorialUIComboObjectInstant.transform.Find ( "tapText" ).renderer.enabled = false;
					_tutorialUIComboObjectInstant.transform.Find ( "frame" ).collider.enabled = false;

					_tutorialHandInstant = ( GameObject ) Instantiate ( _tutorialHandPrefab, new Vector3 ( cora.transform.position.x - 0.5f, cora.transform.position.y + 1f, cora.transform.position.z ), _tutorialHandPrefab.transform.rotation );
					_tutorialHandInstant.transform.parent = cora.transform;
					_tutorialHandInstant.AddComponent < SimulateDrag > ().targetPosition = VectorTools.cloneVector3 (  new Vector3 ( _tutorialHandInstant.transform.localPosition.x + 3f, _tutorialHandInstant.transform.localPosition.y, _tutorialHandInstant.transform.localPosition.z ));

					_currentTutorialID = TUTORIAL_ID_TAP_CORA;
					tutorialTapCoraPlayed = true;
				}

				switch ( randomCharacter )
				{
				case 0:
				{
					if ( ! faradaydoHasBatAbove )
					{
						batObject = ( GameObject ) Instantiate ( _batPrefab, new Vector3 ( faradaydo.transform.position.x, faradaydo.transform.position.y + 0.1f, 4.4f ), _batPrefab.transform.rotation );
						batObject.transform.parent = faradaydo.transform.parent;
						batsOnLevel.Add ( new BatOnLevelClass ( batObject, faradyadoCharacterData ));
						batObject.GetComponent < TRBatControl > ().myCharacterToAttack = faradyadoCharacterData;
						batsOnLevel[batsOnLevel.Count - 1].myCharacterToAttack.characterObject = faradaydo;
						faradaydoHasBatAbove = true;
					}
					//=====================================Daves Edit====================================
					else if ( ! coraHasBatAbove && TRLevelControl.LEVEL_ID >= 3 )
					{
						if ( TRTutorialManager.getInstance ().getCurrentTutorialID () == 3 && ! tutorialSwipeCoraPlayed )
						{
							_tutorialUIComboObjectInstant = ( GameObject ) Instantiate ( _tutorialComboUIPrefab, Vector3.zero, _tutorialComboUIPrefab.transform.rotation );
							_tutorialUIComboObjectInstant.transform.parent = Camera.main.transform;
							_tutorialUIComboObjectInstant.transform.localPosition = new Vector3 ( 0f, -2.0f, 2f ) + Vector3.forward * 3f;
							
							_tutorialUIComboObjectInstant.transform.Find ( "frameText" ).GetComponent < GameTextControl > ().myKey = "train_tutorial_swipe_upward";
							_tutorialUIComboObjectInstant.transform.Find ( "tapText" ).renderer.enabled = false;
							_tutorialUIComboObjectInstant.transform.Find ( "frame" ).collider.enabled = false;
							
							_tutorialHandInstant = ( GameObject ) Instantiate ( _tutorialHandPrefab, new Vector3 ( cora.transform.position.x, cora.transform.position.y + 1f, cora.transform.position.z - 0.5f ), _tutorialHandPrefab.transform.rotation );
							_tutorialHandInstant.transform.parent = cora.transform;
							_tutorialHandInstant.AddComponent < SimulateDrag > ().targetPosition = VectorTools.cloneVector3 (  new Vector3 ( _tutorialHandInstant.transform.localPosition.x, _tutorialHandInstant.transform.localPosition.y + 2.5f, _tutorialHandInstant.transform.localPosition.z  ));

							_currentTutorialID = TUTORIAL_ID_SWIPE_CORA;
							tutorialSwipeCoraPlayed = true;
						}

						batObject = ( GameObject ) Instantiate ( _batPrefab, new Vector3 ( cora.transform.position.x, cora.transform.position.y + 0.1f, 5.01f ), _batPrefab.transform.rotation );
						batObject.transform.parent = cora.transform.parent;
						batsOnLevel.Add ( new BatOnLevelClass ( batObject, coraCharacterData ));
						batObject.GetComponent < TRBatControl > ().myCharacterToAttack = coraCharacterData;
						batsOnLevel[batsOnLevel.Count - 1].myCharacterToAttack.characterObject = cora;
						coraHasBatAbove = true;
					}
					//=====================================Daves Edit====================================

					else if( ! joseHasBatAbove )
					{
						batObject = ( GameObject ) Instantiate ( _batPrefab, new Vector3 ( jose.transform.position.x, jose.transform.position.y + 0.1f, 4.7f ), _batPrefab.transform.rotation );
						batObject.transform.parent = jose.transform.parent;
						batsOnLevel.Add ( new BatOnLevelClass ( batObject, joseCharacterData ));
						batObject.GetComponent < TRBatControl > ().myCharacterToAttack = joseCharacterData;
						batsOnLevel[batsOnLevel.Count - 1].myCharacterToAttack.characterObject = jose;
						joseHasBatAbove = true;
					}
					else
					{
						// all occupied so do nothing
					}
					break;
				}
				case 1:
				{
				//=====================================Daves Edit====================================
					if ( ! coraHasBatAbove && TRLevelControl.LEVEL_ID >= 3 )
					{
						if ( TRTutorialManager.getInstance ().getCurrentTutorialID () == 3 && ! tutorialSwipeCoraPlayed )
						{
							_tutorialUIComboObjectInstant = ( GameObject ) Instantiate ( _tutorialComboUIPrefab, Vector3.zero, _tutorialComboUIPrefab.transform.rotation );
							_tutorialUIComboObjectInstant.transform.parent = Camera.main.transform;
							_tutorialUIComboObjectInstant.transform.localPosition = new Vector3 ( 0f, -2.0f, 2f ) + Vector3.forward * 3f;
							
							_tutorialUIComboObjectInstant.transform.Find ( "frameText" ).GetComponent < GameTextControl > ().myKey = "train_tutorial_swipe_upward";
							_tutorialUIComboObjectInstant.transform.Find ( "tapText" ).renderer.enabled = false;
							_tutorialUIComboObjectInstant.transform.Find ( "frame" ).collider.enabled = false;
							
							_tutorialHandInstant = ( GameObject ) Instantiate ( _tutorialHandPrefab, new Vector3 ( cora.transform.position.x, cora.transform.position.y + 1f, cora.transform.position.z - 0.5f ), _tutorialHandPrefab.transform.rotation );
							_tutorialHandInstant.transform.parent = cora.transform;
							_tutorialHandInstant.AddComponent < SimulateDrag > ().targetPosition = VectorTools.cloneVector3 (  new Vector3 ( _tutorialHandInstant.transform.localPosition.x, _tutorialHandInstant.transform.localPosition.y + 2.5f, _tutorialHandInstant.transform.localPosition.z  ));

							_currentTutorialID = TUTORIAL_ID_SWIPE_CORA;
							tutorialSwipeCoraPlayed = true;
						}

						batObject = ( GameObject ) Instantiate ( _batPrefab, new Vector3 ( cora.transform.position.x, cora.transform.position.y + 0.1f, 5.01f ), _batPrefab.transform.rotation );
						batObject.transform.parent = cora.transform.parent;
						batsOnLevel.Add ( new BatOnLevelClass ( batObject, coraCharacterData ));
						batObject.GetComponent < TRBatControl > ().myCharacterToAttack = coraCharacterData;
						batsOnLevel[batsOnLevel.Count - 1].myCharacterToAttack.characterObject = cora;
						coraHasBatAbove = true;
					}
					//=====================================Daves Edit====================================

					else if ( ! faradaydoHasBatAbove )
					{
						batObject = ( GameObject ) Instantiate ( _batPrefab, new Vector3 ( faradaydo.transform.position.x, faradaydo.transform.position.y + 0.1f, 4.4f ), _batPrefab.transform.rotation );
						batObject.transform.parent = faradaydo.transform.parent;
						batsOnLevel.Add ( new BatOnLevelClass ( batObject, faradyadoCharacterData ));
						batObject.GetComponent < TRBatControl > ().myCharacterToAttack = faradyadoCharacterData;
						batsOnLevel[batsOnLevel.Count - 1].myCharacterToAttack.characterObject = faradaydo;
						
						faradaydoHasBatAbove = true;
					}
					else if ( ! joseHasBatAbove )
					{
						batObject = ( GameObject ) Instantiate ( _batPrefab, new Vector3 ( jose.transform.position.x, jose.transform.position.y + 0.1f, 4.7f ), _batPrefab.transform.rotation );
						batObject.transform.parent = jose.transform.parent;
						batsOnLevel.Add ( new BatOnLevelClass ( batObject, joseCharacterData ));
						batsOnLevel[batsOnLevel.Count - 1].myCharacterToAttack.characterObject = jose;
						batObject.GetComponent < TRBatControl > ().myCharacterToAttack = joseCharacterData;
						joseHasBatAbove = true;
					}
					else
					{
						// all occupied so do nothing
					}
					break;
				}
				case 2:
				{
					if ( ! joseHasBatAbove )
					{
						batObject = ( GameObject ) Instantiate ( _batPrefab, new Vector3 ( jose.transform.position.x, jose.transform.position.y + 0.1f, 4.7f ), _batPrefab.transform.rotation );
						batObject.transform.parent = jose.transform.parent;
						batsOnLevel.Add ( new BatOnLevelClass ( batObject, joseCharacterData ));
						batObject.GetComponent < TRBatControl > ().myCharacterToAttack = joseCharacterData;
						batsOnLevel[batsOnLevel.Count - 1].myCharacterToAttack.characterObject = jose;
						joseHasBatAbove	= true;
					}
					
					else if ( ! faradaydoHasBatAbove )
					{
						batObject = ( GameObject ) Instantiate ( _batPrefab, new Vector3 ( faradaydo.transform.position.x, faradaydo.transform.position.y + 0.1f, 4.4f ), _batPrefab.transform.rotation );
						batObject.transform.parent = faradaydo.transform.parent;
						batsOnLevel.Add ( new BatOnLevelClass ( batObject, faradyadoCharacterData ));
						batsOnLevel[batsOnLevel.Count - 1].myCharacterToAttack.characterObject = faradaydo;
						batObject.GetComponent < TRBatControl > ().myCharacterToAttack = faradyadoCharacterData;
						faradaydoHasBatAbove = true;
					}
					//=====================================Daves Edit====================================
					else if ( ! coraHasBatAbove && TRLevelControl.LEVEL_ID >= 3 )
					{
						if ( TRTutorialManager.getInstance ().getCurrentTutorialID () == 3 && ! tutorialSwipeCoraPlayed )
						{
							_tutorialUIComboObjectInstant = ( GameObject ) Instantiate ( _tutorialComboUIPrefab, Vector3.zero, _tutorialComboUIPrefab.transform.rotation );
							_tutorialUIComboObjectInstant.transform.parent = Camera.main.transform;
							_tutorialUIComboObjectInstant.transform.localPosition = new Vector3 ( 0f, -2.0f, 2f ) + Vector3.forward * 3f;
							
							_tutorialUIComboObjectInstant.transform.Find ( "frameText" ).GetComponent < GameTextControl > ().myKey = "train_tutorial_swipe_upward";
							_tutorialUIComboObjectInstant.transform.Find ( "tapText" ).renderer.enabled = false;
							_tutorialUIComboObjectInstant.transform.Find ( "frame" ).collider.enabled = false;
							
							_tutorialHandInstant = ( GameObject ) Instantiate ( _tutorialHandPrefab, new Vector3 ( cora.transform.position.x, cora.transform.position.y + 1f, cora.transform.position.z - 0.5f ), _tutorialHandPrefab.transform.rotation );
							_tutorialHandInstant.transform.parent = cora.transform;
							_tutorialHandInstant.AddComponent < SimulateDrag > ().targetPosition = VectorTools.cloneVector3 (  new Vector3 ( _tutorialHandInstant.transform.localPosition.x, _tutorialHandInstant.transform.localPosition.y + 2.5f, _tutorialHandInstant.transform.localPosition.z  ));
							
							_currentTutorialID = TUTORIAL_ID_SWIPE_CORA;
							tutorialSwipeCoraPlayed = true;
						}

						batObject = ( GameObject ) Instantiate ( _batPrefab,new Vector3 ( cora.transform.position.x, cora.transform.position.y + 0.1f, 5.01f ), _batPrefab.transform.rotation );
						batObject.transform.parent = cora.transform.parent;
						batsOnLevel.Add ( new BatOnLevelClass ( batObject, coraCharacterData ));
						batsOnLevel[batsOnLevel.Count - 1].myCharacterToAttack.characterObject = cora;
						batObject.GetComponent < TRBatControl > ().myCharacterToAttack = coraCharacterData;
						coraHasBatAbove = true;
					}
					//=====================================Daves Edit====================================
					else
					{
						// all occupied so do nothing
					}
				}
				break;
			}

			break;
		}
			break;
		}

		if ( batObject !=null ) iTween.MoveFrom ( batObject, iTween.Hash ( "time", 0.45f, "easetype", iTween.EaseType.easeOutBack, "position", batObject.transform.position + Vector3.forward * 2.5f, "oncompletetarget", this.gameObject, "oncomplete", "onCompleteMoveBat" ));
	}

	public int getCurrentTutorialID ()
	{
		return _currentTutorialID;
	}

	public void cleanTutorialTapCora ()
	{
		if ( _lastTutrialScreenPlayed ) return;

		_currentTutorialID = 0;
		Destroy ( _tutorialHandInstant );
		Destroy ( _tutorialUIComboObjectInstant );

		if ( tutorialSwipeTentaclePlayed && tutorialTapCoraPlayed )
		{
			_lastTutrialScreenPlayed = true;
			StartCoroutine ( "waitBeforeLastTutorialScreen" );
		}
	}

	public void cleanTutorialSwipeTentacle ()
	{
		if ( _lastTutrialScreenPlayed ) return;

		_currentTutorialID = 0;
		Destroy ( _tutorialHandInstant );
		Destroy ( _tutorialUIComboObjectInstant );

		if ( tutorialSwipeTentaclePlayed && tutorialTapCoraPlayed )
		{
			_lastTutrialScreenPlayed = true;
			StartCoroutine ( "waitBeforeLastTutorialScreen" );
		}
	}

	public void cleanTutorialForTapRockButton ()
	{
		if ( _lastTutrialScreenPlayed ) return;
		
		_currentTutorialID = 0;
		Destroy ( _tutorialUIComboObjectInstant );
		
		_lastTutrialScreenPlayed = true;
		StartCoroutine ( "waitBeforeLastTutorialScreen" );
	}

	public void cleanTutorialSwipeCora ()
	{
		if ( _lastTutrialScreenPlayed ) return;
		
		_currentTutorialID = 0;
		Destroy ( _tutorialHandInstant );
		Destroy ( _tutorialUIComboObjectInstant );
		
		_lastTutrialScreenPlayed = true;
		StartCoroutine ( "waitBeforeLastTutorialScreen" );
	}

	private IEnumerator waitBeforeLastTutorialScreen ()
	{
		yield return new WaitForSeconds ( 2f );

		_tutorialUIComboObjectInstant = ( GameObject ) Instantiate ( _tutorialComboUIPrefab, Vector3.zero, _tutorialComboUIPrefab.transform.rotation );
		_tutorialUIComboObjectInstant.transform.parent = Camera.main.transform;
		_tutorialUIComboObjectInstant.transform.localPosition = new Vector3 ( 0f, -2.0f, 2f ) + Vector3.forward * 3f;
		
		_tutorialUIComboObjectInstant.transform.Find ( "frameText" ).GetComponent < GameTextControl > ().myKey = "train_tutorial_keep_the_speed";
		_tutorialUIComboObjectInstant.transform.Find ( "frame" ).gameObject.AddComponent < TRTapFrameControl > ();
		_tutorialUIComboObjectInstant.transform.Find ( "frame" ).collider.enabled = true;

		Time.timeScale = 0.15f;

		Camera.main.transform.Find ( "jumpingArrow" ).gameObject.SetActive ( true );

		TRTriggerDialogueControl.getInstance ().turnOn ();
	}

	private void onCompleteMoveBat ()
	{

	}
}
