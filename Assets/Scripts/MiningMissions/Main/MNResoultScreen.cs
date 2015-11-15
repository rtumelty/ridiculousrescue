using UnityEngine;
using System.Collections;

public class MNResoultScreen : MonoBehaviour 
{
	public GameObject madraSpine;
	public GameObject faraSpine;
	public GameObject bozSpine;
	public GameObject coraSpine;
	private TextMesh _missionFailText;
	//*************************************************************//	
	private static MNResoultScreen _meInstance;
	public static MNResoultScreen getInstance ()
	{
		if ( _meInstance == null )
		{
			_meInstance = GameObject.Find ( "CameraResoultScreen" ).GetComponent < MNResoultScreen > ();
		}
		
		return _meInstance;
	}
	void Start()
	{
		madraSpine = GameObject.Find ( "madra" ).gameObject;
		faraSpine = GameObject.Find ( "fara" ).gameObject;
		bozSpine = GameObject.Find ( "boz" ).gameObject;
		coraSpine = GameObject.Find ( "cora" ).gameObject;

		madraSpine.SetActive(false);
		faraSpine.SetActive(false);
		bozSpine.SetActive(false);
		coraSpine.SetActive(false);
	}
	//*************************************************************//
	public void startResoultScreen ()
	{
		MNGlobalVariables.MENU_FOR_RESOULT_SCREEN = true;

		camera.depth = 100;
		Camera.main.depth = 0;
		
		if ( charOutOfPower ())
		{
			int numberOfMiningFailedd = SaveDataManager.getValue ( SaveDataManager.MINING_FAILED );
			numberOfMiningFailedd++;
			SaveDataManager.save ( SaveDataManager.MINING_FAILED, numberOfMiningFailedd );
			GoogleAnalytics.instance.LogScreen ( "Mining - Fail - Level " + MNLevelControl.SELECTED_LEVEL_NAME );

			SoundManager.getInstance ().playSound ( SoundManager.MISSION_FAILED );

			transform.Find ( "FailedScreenAllToysDeanimated" ).gameObject.SetActive ( true );
			transform.Find ( "FailedScreenNoMaterials" ).gameObject.SetActive ( false );
			transform.Find ( "SuccessScreen" ).gameObject.SetActive ( false );
			GameGlobalVariables.Stats.NewResources.reset ();
		}
		else if ( noMaterialsGathered ())
		{
			int numberOfMiningFailedd = SaveDataManager.getValue ( SaveDataManager.MINING_FAILED );
			numberOfMiningFailedd++;
			SaveDataManager.save ( SaveDataManager.MINING_FAILED, numberOfMiningFailedd );
			GoogleAnalytics.instance.LogScreen ( "Mining - Fail - Level " + MNLevelControl.SELECTED_LEVEL_NAME );

			SoundManager.getInstance ().playSound ( SoundManager.MISSION_FAILED );

			transform.Find ( "FailedScreenNoMaterials" ).gameObject.SetActive ( true );
			transform.Find ( "FailedScreenAllToysDeanimated" ).gameObject.SetActive ( false );
			transform.Find ( "SuccessScreen" ).gameObject.SetActive ( false );
		}
		else
		{
			int numberOfMiningSuccess = SaveDataManager.getValue ( SaveDataManager.MINING_SUCCESS );
			numberOfMiningSuccess++;
			SaveDataManager.save ( SaveDataManager.MINING_SUCCESS, numberOfMiningSuccess );
			GoogleAnalytics.instance.LogScreen ( "Mining - Win - Level " + MNLevelControl.SELECTED_LEVEL_NAME );

			transform.Find ( "SuccessScreen" ).gameObject.SetActive ( true );
			transform.Find ( "FailedScreenNoMaterials" ).gameObject.SetActive ( false );
			transform.Find ( "FailedScreenAllToysDeanimated" ).gameObject.SetActive ( false );
			MNSuccessScreenControl.getInstance ().CustomStart ();
		}
	}
	
	public bool charOutOfPower ()
	{
		_missionFailText = GameObject.Find( "failText" ).GetComponent < TextMesh > ();
		foreach ( CharacterData character in MNLevelControl.getInstance ().charactersOnLevel )
		{
			if ( character.characterValues[CharacterData.CHARACTER_ACTION_TYPE_POWER] < 1 )
			{
				if(character.myID == GameElements.CHAR_FARADAYDO_1_IDLE)
				{
					_missionFailText.GetComponent < GameTextControl > ().myKey = "ui_sign_mission_failed_faradaydo_deanimated";
					faraSpine.SetActive(true);
		//			faraSpine.GetComponent < CharacterAnimationControl >().playAnimation( CharacterAnimationControl.dea );
				}
				else if(character.myID == GameElements.CHAR_MADRA_1_IDLE)
				{
					_missionFailText.GetComponent < GameTextControl > ().myKey = "ui_sign_mission_failed_madra_deanimated";
					madraSpine.SetActive(true);
		//			madraSpine.GetComponent < CharacterAnimationControl >().playAnimation( CharacterAnimationControl.MADRA_DEACTIVE );
				}
				else if(character.myID == GameElements.CHAR_BOZ_1_IDLE)
				{
					_missionFailText.GetComponent < GameTextControl > ().myKey = "ui_sign_mission_failed_boz_deanimated";
					bozSpine.SetActive(true);
		//			bozSpine.GetComponent < CharacterAnimationControl >().playAnimation( CharacterAnimationControl.BOZ_DEACTIVE );
				}
				else if(character.myID == GameElements.CHAR_CORA_1_IDLE)
				{
					_missionFailText.GetComponent < GameTextControl > ().myKey = "ui_sign_mission_failed_cora_deanimated";
					coraSpine.SetActive(true);
//					coraSpine.GetComponent < CharacterAnimationControl >().playAnimation(CharacterAnimationControl.CORA_DEACTIVE);
				}
				return true;
			}
		}
		
		return false;
	}
		
	public bool noMaterialsGathered ()
	{
		return GameGlobalVariables.Stats.NewResources.METAL == 0 && GameGlobalVariables.Stats.NewResources.PLASTIC == 0 && GameGlobalVariables.Stats.NewResources.VINES == 0;
	}
}

