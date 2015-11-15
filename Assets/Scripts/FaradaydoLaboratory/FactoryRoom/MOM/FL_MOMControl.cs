using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class FL_MOMControl : MonoBehaviour 
{
	//*************************************************************//
	public class ProductionSlot
	{
		public int elementID;
		public float timeLeft;
		public float timeTotal;
		
		public ProductionSlot ( int elementIDValue, int timeLeftValue )
		{
			elementID = elementIDValue;
			timeLeft = timeTotal = ( float ) timeLeftValue;
		}
	}
	//*************************************************************//
	public FL_MOMClass myMomClass;
	//*************************************************************//
	private GameObject _myComboUI;
	private GameObject _currentReadyElement;
	private float _countUpdate = 0f;
	private bool _checkIfMapDragged = false;
	//private GameObject blackDrop;
	private GameObject playButton;
	private GameObject factoryButton;
	private GameObject missionButton;
	private GameObject garageButton;
	private GameObject playButtonText;
	private GameObject factoryButtonText;
	private GameObject missionButtonText;
	private GameObject garageButtonText;
	public bool allowPan = true;
	public bool momPressed = false;
	public bool iAmActive = false;
	//*************************************************************//

	public void ToggleFocus (bool momIsFocus)
	{
		if(momIsFocus == true)
		{
			/*print ("ON");
			playButton.renderer.enabled = false;
			playButtonText.SetActive(false);
			factoryButton.renderer.enabled = false;
			factoryButtonText.SetActive(false);
			missionButton.renderer.enabled = false;
			missionButtonText.SetActive(false);
			garageButton.renderer.enabled = false;
			garageButtonText.SetActive(false);*/
			allowPan = false;
			FLUIControl.getInstance ().fromMom = true;
		}
		if(momIsFocus == false)
		{
			/*print ("OFF");
			playButton.renderer.enabled = true;
			playButtonText.SetActive(true);
			factoryButton.renderer.enabled = true;
			factoryButtonText.SetActive(true);
			missionButton.renderer.enabled = true;
			missionButtonText.SetActive(true);
			garageButton.renderer.enabled = true;
			garageButtonText.SetActive(true);*/
			allowPan = true;
		}
	}
	void Start ()
	{
		myMomClass.myMomControl = this;
		playButton = GameObject.Find ("PlayRoomButton");
		factoryButton = GameObject.Find ("FactoryRoomButton");
		missionButton = GameObject.Find ("MissionRoomButton");
		garageButton = GameObject.Find ("GarageRoomButton");
		playButtonText = playButton.transform.Find ("3DText").gameObject;
		factoryButtonText = factoryButton.transform.Find ("3DText").gameObject;
		missionButtonText = missionButton.transform.Find ("3DText").gameObject;
		garageButtonText = garageButton.transform.Find ("3DText").gameObject;
		ToggleFocus (false);

		int numberOfReadyBatteries = SaveDataManager.getValue ( SaveDataManager.BATTERIES_AT_MOM_READY_TO_TAP );

		for ( int i = 0; i < numberOfReadyBatteries; i++ )
		{
			createReadyElement ( GameElements.ICON_RECHARGEOCORES, true );
		}

		int numberOfReadyredirectors = SaveDataManager.getValue ( SaveDataManager.REDIRECTORS_AT_MOM_READY_TO_TAP );
		
		for ( int i = 0; i < numberOfReadyredirectors; i++ )
		{
			createReadyElement ( GameElements.ICON_REDIRECTORS, true );
		}

	//=================================Daves Edit====================================
		for ( int i = 0; i < myMomClass.numberOfSlotsUnblocked; i++ )
		{
			if (( myMomClass.myProductionSlots.Count > i ) && ( myMomClass.myProductionSlots[i] != null ))
			{
				if(myMomClass.myProductionSlots[i].elementID == 64);
				{
					GameGlobalVariables.Stats.RECHARGEOCORES_IN_CONSTRUCTION ++;
				}
			}
		}
	//=================================Daves Edit====================================
	}
	
	void OnMouseUp ()
	{
		if ( _checkIfMapDragged )
		{
			if ( FLGlobalVariables.SCREEN_DRAGGED )
			{
				FLGlobalVariables.SCREEN_DRAGGED = false;
				_checkIfMapDragged = false;
				return;
			}
		}

		if ( FLGlobalVariables.checkForMenus ()) return;
		handleTouched ();
		//ToggleFocus (momPressed);
	}

	void OnMouseDown ()
	{
		_checkIfMapDragged = true;
	}
	
	private void handleTouched ()
	{
		iAmActive = true;
		//ToggleFocus (true);
		FLMain.getInstance ().unselectCurrentCharacter ();
		//transform.position = new Vector3 (transform.position.x, transform.position.y + 5, transform.position.z);
		FLUIControl.getInstance ().unselectCurrentGameElement ();
		gameObject.GetComponent < SelectedComponenent > ().setSelected ( true, true, true, true );
		FLUIControl.currentSelectedGameElement = this.gameObject;
		
		if ( _myComboUI != null )
		{
			if ( _myComboUI.GetComponent < HideUIElement > () != null )
			{
				Destroy ( _myComboUI );
			}
			else return;
		}
		
		FLUIControl.getInstance ().destoryCurrentUIElement ();
		
		_myComboUI = ( GameObject ) Instantiate ( FLFactoryRoomManager.getInstance ().momUIComboPrefab, transform.position + Vector3.up * 10f + Vector3.back * 1.25f + Vector3.right * 1.3f, FLFactoryRoomManager.getInstance ().momUIComboPrefab.transform.rotation );
		_myComboUI.GetComponent < FL_MOMPanelControl > ().myMomClass = myMomClass;
		myMomClass.myMomPanelControl = _myComboUI.GetComponent < FL_MOMPanelControl > ();
		FLUIControl.currentUIElement = _myComboUI;
		renderer.material.mainTexture = FLFactoryRoomManager.getInstance ().momTextureOff;
	}
	
	void Update ()
	{

		if (( myMomClass.myProductionSlots.Count > 0 ) && ( myMomClass.myProductionSlots[0] != null ))
		{
			int startTimeForThisSlot = SaveDataManager.getValue ( SaveDataManager.MOM_SLOT_0_TIME_PREFIX + myMomClass.position[0].ToString () + "," + myMomClass.position[1].ToString ());
			int deltaTime = (int) ( DateTime.UtcNow.Ticks / 10000000 ) - startTimeForThisSlot;
			myMomClass.myProductionSlots[0].timeLeft = myMomClass.myProductionSlots[0].timeTotal - (float) deltaTime;

			if ( myMomClass.myProductionSlots[0].timeTotal <= deltaTime )
			{
			}
			else
			{
				if ( gameObject.GetComponent < FL_MOMAnimationComponent > ().getCurrentAnimation () != FL_MOMAnimationComponent.WORKING_ANIMATION )
				{
					gameObject.GetComponent < FL_MOMAnimationComponent > ().playAnimation ( FL_MOMAnimationComponent.WORKING_ANIMATION );
				}
			}
		}
		if(FLUIControl.currentSelectedGameElement == this.gameObject)
		{
			//ToggleFocus(true);
			//Camera.main.orthographicSize = 5.3f;
			//Camera.main.transform.position = Vector3.MoveTowards(Camera.main.transform.position, new Vector3(gameObject.transform.position.x + 3f, Camera.main.transform.position.y, gameObject.transform.position.z + 0.2f), 15 * Time.deltaTime);
			//FLUIControl.getInstance().momPanLock = true;
		}
		else
		{
			//ToggleFocus(false);
			//FLUIControl.getInstance().momPanLock = false;
		}
		_countUpdate -= Time.deltaTime;
		
		if ( _countUpdate > 0f ) return;
		
		_countUpdate = 1f;


		if (( myMomClass.myProductionSlots.Count > 0 ) && ( myMomClass.myProductionSlots[0] != null ))
		{
			int startTimeForThisSlot = SaveDataManager.getValue ( SaveDataManager.MOM_SLOT_0_TIME_PREFIX + myMomClass.position[0].ToString () + "," + myMomClass.position[1].ToString ());
			int deltaTime = (int) ( DateTime.UtcNow.Ticks / 10000000 ) - startTimeForThisSlot;
			myMomClass.myProductionSlots[0].timeLeft = myMomClass.myProductionSlots[0].timeTotal - (float) deltaTime;
			
			if ( myMomClass.myProductionSlots[0].timeTotal <= deltaTime )
			{
				if ( renderer.material.mainTexture.name != FLFactoryRoomManager.getInstance ().momTextureReady.name ) 
				{
					renderer.material.mainTexture = FLFactoryRoomManager.getInstance ().momTextureReady;
				}
				
				createReadyElement ( myMomClass.myProductionSlots[0].elementID );
				
				myMomClass.myProductionSlots[0].timeLeft = 0f;
				myMomClass.myProductionSlots.Remove ( myMomClass.myProductionSlots[0] );

				float totalTimeDeltaAbove = ( float ) deltaTime - myMomClass.myProductionSlots[0].timeTotal;

				if ( myMomClass.myProductionSlots.Count > 0 )
				{
					SaveDataManager.save ( SaveDataManager.MOM_SLOT_0_TIME_PREFIX + myMomClass.position[0].ToString () + "," + myMomClass.position[1].ToString (), (int) ( DateTime.UtcNow.Ticks / 10000000 ) - ( int ) totalTimeDeltaAbove );
				}
				
				if ( myMomClass.myMomPanelControl != null )
				{
					myMomClass.myMomPanelControl.updateSlots ();
				}
			}
			else
			{
				if ( gameObject.GetComponent < FL_MOMAnimationComponent > ().getCurrentAnimation () != FL_MOMAnimationComponent.WORKING_ANIMATION )
				{
					gameObject.GetComponent < FL_MOMAnimationComponent > ().playAnimation ( FL_MOMAnimationComponent.WORKING_ANIMATION );
				}
			}
		}
		
		for ( int i = 0; i < myMomClass.numberOfSlotsUnblocked; i++ )
		{
			if (( myMomClass.myProductionSlots.Count > i ) && ( myMomClass.myProductionSlots[i] != null ))
			{
				SaveDataManager.save ( SaveDataManager.MOM_SLOTS_ELEMENT_PREFIX + ( i ).ToString () + myMomClass.position[0].ToString () + "," + myMomClass.position[1].ToString (), myMomClass.myProductionSlots[i].elementID );
			}
			else
			{
				SaveDataManager.save ( SaveDataManager.MOM_SLOTS_ELEMENT_PREFIX + ( i ).ToString () + myMomClass.position[0].ToString () + "," + myMomClass.position[1].ToString (), GameElements.EMPTY );
			}
		}
	}
	
	public void externallyCreateReadyElement ()
	{
		GoogleAnalytics.instance.LogScreen ( "Speed up - product " + ( myMomClass.myProductionSlots[0].elementID == GameElements.ICON_RECHARGEOCORES ? "BATTERY" : "REDIRECTOR" ));

		createReadyElement ( myMomClass.myProductionSlots[0].elementID );
				
		myMomClass.myProductionSlots[0].timeLeft = 0f;
		myMomClass.myProductionSlots.Remove ( myMomClass.myProductionSlots[0] );
		
		if ( myMomClass.myMomPanelControl != null )
		{
			myMomClass.myMomPanelControl.updateSlots ();
		}
	}
	
	private void createReadyElement ( int elementID, bool withoutAddingToRegister = false )
	{
		gameObject.GetComponent < FL_MOMAnimationComponent > ().stopAnimation ();
		if ( renderer.material.mainTexture.name != FLFactoryRoomManager.getInstance ().momTextureReady.name ) 
		{
			renderer.material.mainTexture = FLFactoryRoomManager.getInstance ().momTextureReady;
		}
		
		GameObject readeyElementObject = FLLevelControl.getInstance ().createFreeObjectOnPosition ( elementID, new int[] { myMomClass.position[0] + 1, myMomClass.position[1] });
		_currentReadyElement = readeyElementObject;
		FL_MOMTapReadyElement currentFL_MOMTapReadyElement = readeyElementObject.transform.Find ( "tile" ).gameObject.AddComponent < FL_MOMTapReadyElement > ();
		currentFL_MOMTapReadyElement.myElementID = elementID;

		if ( ! withoutAddingToRegister )
		{
			switch ( elementID )
			{
			case GameElements.ICON_RECHARGEOCORES:
				int numberOfReadyBatteries = SaveDataManager.getValue ( SaveDataManager.BATTERIES_AT_MOM_READY_TO_TAP );
				numberOfReadyBatteries++;
				SaveDataManager.save ( SaveDataManager.BATTERIES_AT_MOM_READY_TO_TAP, numberOfReadyBatteries );
				break;
			case GameElements.ICON_REDIRECTORS:
				int numberOfReadyRedirectors = SaveDataManager.getValue ( SaveDataManager.REDIRECTORS_AT_MOM_READY_TO_TAP );
				numberOfReadyRedirectors++;
				SaveDataManager.save ( SaveDataManager.REDIRECTORS_AT_MOM_READY_TO_TAP, numberOfReadyRedirectors );
				break;
			}
		}
		if ( FLGlobalVariables.TUTORIAL_MENU )
		{
			TutorialUIObjectTapComponent currentTutorialUIObjectTapComponent = readeyElementObject.transform.Find ( "tile" ).gameObject.AddComponent < TutorialUIObjectTapComponent > ();
			currentTutorialUIObjectTapComponent.arrowFromAbove = true;
			currentTutorialUIObjectTapComponent.dontShowArrow = true;
		}
	}
	public void OnDestroy()
	{
		for ( int i = 0; i < myMomClass.numberOfSlotsUnblocked; i++ )
		{
			if (( myMomClass.myProductionSlots.Count > i ) && ( myMomClass.myProductionSlots[i] != null ))
			{
				if(myMomClass.myProductionSlots[i].elementID == 64);
				{
					GameGlobalVariables.Stats.RECHARGEOCORES_IN_CONSTRUCTION --;
				}
			}
		}
	}
}
