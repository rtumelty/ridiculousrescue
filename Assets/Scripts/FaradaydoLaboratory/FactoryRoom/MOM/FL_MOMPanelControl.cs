using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class FL_MOMPanelControl : MonoBehaviour 
{
	//*************************************************************//
	private const float _ADD_POSITION_FOR_NOT_ALL_SLOTS = -0.029f;
	private const float _ADD_POSITION_MULTIPLYER = 0.695f;
	//*************************************************************//
	public FL_MOMClass myMomClass;
	public GameObject myBuyButton;
	//*************************************************************//
	private GameObject _buyNewSlotButtonPrefab;
	private GameObject _buyNewSlotButtonInstant;
	private List < GameObject > _slotObjects;
	private GameObject _startSlot;
	private Vector3 _startSlotInitialScale;
	private List < GameObject > _elementObjects;
	private GameObject _myNotEnoughResourcesScreen;
	private GameObject _backPanel;
	private float _countUpdate = 0f;
	private bool posAdusted = false;
	//*************************************************************//
	void Start () 
	{
		myMomClass.myMomPanelControl = this;
		_backPanel = transform.Find ( "back" ).gameObject;

		_buyNewSlotButtonPrefab = ( GameObject ) Resources.Load ( "UI/Laboratory/buttonBuy" );
		
		_slotObjects = new List < GameObject > ();
		_startSlot = transform.Find ( "slot" ).gameObject;
		_startSlotInitialScale = VectorTools.cloneVector3 ( _startSlot.transform.localScale );
		_slotObjects.Add ( _startSlot );
		
		myBuyButton = transform.Find ( "buttonBuy" ).gameObject;
		myBuyButton.GetComponent < FL_MOMBuyWithPremiumButton > ().myMomClass = myMomClass;
		
		if (( myMomClass.myProductionSlots != null ) && ( myMomClass.myProductionSlots.Count > 0 ) && ( myMomClass.myProductionSlots[0] != null ))
		{
			myBuyButton.transform.Find ( "textAmount" ).GetComponent < TextMesh > ().text = FLElementsConstructionCosts.COSTS_VALUES[myMomClass.myProductionSlots[0].elementID].premiumCurrency.ToString ();
		}
		setButtonBuyActive ( false );
		
		_elementObjects = new List < GameObject > ();
		_elementObjects.Add ( transform.Find ( "element01" ).gameObject );
		_elementObjects.Add ( transform.Find ( "element02" ).gameObject );
		_elementObjects[0].renderer.material.mainTexture = FLLevelControl.getInstance ().gameElements[GameElements.ICON_RECHARGEOCORES];
		_elementObjects[1].renderer.material.mainTexture = FLLevelControl.getInstance ().gameElements[GameElements.ICON_REDIRECTORS];
		FL_MOMElementDragControl dragComponent01 = _elementObjects[0].AddComponent < FL_MOMElementDragControl > ();
		FL_MOMElementDragControl dragComponent02 = _elementObjects[1].AddComponent < FL_MOMElementDragControl > ();
		dragComponent01.myElementID = GameElements.ICON_RECHARGEOCORES;
		FLFactoryRoomManager.getInstance ().currentMoMRechargeOCoreObject = _elementObjects[0];
		dragComponent02.myElementID = GameElements.ICON_REDIRECTORS;
		dragComponent01.myMomClass = myMomClass;
		dragComponent02.myMomClass = myMomClass;		

		if ( _slotObjects.Count < myMomClass.numberOfSlots )
		{
			_backPanel.transform.localScale = new Vector3 ( _ADD_POSITION_FOR_NOT_ALL_SLOTS + ( myMomClass.numberOfSlotsUnblocked + 1 ) * _ADD_POSITION_MULTIPLYER, _backPanel.transform.localScale.y, _backPanel.transform.localScale.z );
			_backPanel.transform.position = new Vector3 (_backPanel.transform.position.x - (0.015f * ( _slotObjects.Count - 1)), _backPanel.transform.position.y, _backPanel.transform.position.z);
		}
		else
		{
			_backPanel.transform.localScale = new Vector3 ( _ADD_POSITION_FOR_NOT_ALL_SLOTS+ myMomClass.numberOfSlotsUnblocked * _ADD_POSITION_MULTIPLYER, _backPanel.transform.localScale.y, _backPanel.transform.localScale.z );
			_backPanel.transform.position = new Vector3 (_backPanel.transform.position.x - (0.015f * ( _slotObjects.Count - 1)), _backPanel.transform.position.y, _backPanel.transform.position.z);
		}

		for ( int i = 1; i < myMomClass.numberOfSlotsUnblocked; i++ )
		{
			GameObject currentSlot = ( GameObject ) Instantiate ( _startSlot, _startSlot.transform.position + Vector3.right * 1.59f * i, _startSlot.transform.rotation );
			currentSlot.transform.parent = _startSlot.transform.parent;
			currentSlot.transform.localScale = VectorTools.cloneVector3 ( _startSlotInitialScale );;
			currentSlot.transform.Find ( "textSlot" ).gameObject.AddComponent < FontDropShadowControl > ().whiteShadow = false;
			_slotObjects.Add ( currentSlot );
		}
		
		if ( _slotObjects.Count < myMomClass.numberOfSlots )
		{
			_buyNewSlotButtonInstant = ( GameObject ) Instantiate ( _buyNewSlotButtonPrefab, _slotObjects[0].transform.position + Vector3.right * myMomClass.numberOfSlotsUnblocked * 1.59f + Vector3.forward * 0.63f /*+ Vector3.right * 0.045f*/, _buyNewSlotButtonPrefab.transform.rotation );
			_buyNewSlotButtonInstant.transform.parent = this.transform;
			_buyNewSlotButtonInstant.GetComponent < FL_MOMBuyWithPremiumButton > ().buyElement = false;
			_buyNewSlotButtonInstant.GetComponent < FL_MOMBuyWithPremiumButton > ().myMomClass = myMomClass;
		}
		
		myBuyButton.transform.parent = _startSlot.transform;
		
		updateSlots ();
	}
	
	public void buyNewSlot ()
	{
		if ( _slotObjects.Count < myMomClass.numberOfSlots )
		{
			posAdusted = false;
			_backPanel.transform.localScale = new Vector3 ( _ADD_POSITION_FOR_NOT_ALL_SLOTS + ( myMomClass.numberOfSlotsUnblocked + 1 ) * _ADD_POSITION_MULTIPLYER, _backPanel.transform.localScale.y, _backPanel.transform.localScale.z );
			print ("or here");
			myMomClass.numberOfSlotsUnblocked++;
			
			SaveDataManager.save ( SaveDataManager.MOM_SLOTS_NUMBER_PREFIX + myMomClass.position[0].ToString () + "," + myMomClass.position[1].ToString (), myMomClass.numberOfSlotsUnblocked ); 
			
			GameObject currentSlot = ( GameObject ) Instantiate ( _startSlot, _startSlot.transform.position + Vector3.right * ( myMomClass.numberOfSlotsUnblocked - 1 ) * 1.85f , _startSlot.transform.rotation );
			currentSlot.transform.parent = _startSlot.transform.parent;
			currentSlot.transform.localScale = VectorTools.cloneVector3 ( _startSlotInitialScale );
			currentSlot.transform.Find ( "textSlot" ).gameObject.AddComponent < FontDropShadowControl > ().whiteShadow = false;
			_slotObjects.Add ( currentSlot );
			
			if ( currentSlot.transform.Find ( "buttonBuy" ))
			{
				Destroy ( currentSlot.transform.Find ( "buttonBuy" ).gameObject );
			}
			
			if ( ! currentSlot.transform.Find ( "textSlot" ).GetComponent < GameTextControl > ())
			{
				GameTextControl currentGameTextControl = currentSlot.transform.Find ( "textSlot" ).gameObject.AddComponent < GameTextControl > ();
				currentGameTextControl.myKey = "ui_sign_empty";
				currentGameTextControl.updateText ();
			}
			else
			{
				currentSlot.transform.Find ( "textSlot" ).GetComponent < GameTextControl > ().myKey = "ui_sign_empty";
				currentSlot.transform.Find ( "textSlot" ).GetComponent < GameTextControl > ().updateText ();
			}
			
			currentSlot.transform.Find ( "textSlot" ).localPosition = new Vector3 ( 0f, -0.67f, 0.45f );
			iTween.ScaleTo ( currentSlot, iTween.Hash ( "time", 0.3f, "easetype", iTween.EaseType.linear, "scale", _startSlotInitialScale ));
			
			currentSlot.transform.Find ( "element" ).renderer.enabled = false;
			
			if ( _slotObjects.Count >= myMomClass.numberOfSlots )
			{
				Destroy ( _buyNewSlotButtonInstant );
				return;
			}
			
			if ( _buyNewSlotButtonInstant == null )
			{
				if ( _slotObjects.Count < myMomClass.numberOfSlots )
				{
					print ("ThisPArt");
					_buyNewSlotButtonInstant = ( GameObject ) Instantiate ( _buyNewSlotButtonPrefab, _slotObjects[0].transform.position + Vector3.forward * 0.44f + Vector3.right * myMomClass.numberOfSlotsUnblocked * 1.59f, _buyNewSlotButtonPrefab.transform.rotation );
					_buyNewSlotButtonInstant.transform.parent = this.transform;
					_buyNewSlotButtonInstant.GetComponent < FL_MOMBuyWithPremiumButton > ().buyElement = false;
					_buyNewSlotButtonInstant.GetComponent < FL_MOMBuyWithPremiumButton > ().myMomClass = myMomClass;
				}
			}
			else
			{
				_buyNewSlotButtonInstant.transform.position += Vector3.right * 1.6f + Vector3.right * 0.22f;
			}
		}
		else
		{ 
			_backPanel.transform.localScale = new Vector3 ( _ADD_POSITION_FOR_NOT_ALL_SLOTS + myMomClass.numberOfSlotsUnblocked * _ADD_POSITION_MULTIPLYER, _backPanel.transform.localScale.y, _backPanel.transform.localScale.z );
			print ("here");
			Destroy ( _buyNewSlotButtonInstant );
		}
	}
	
	public bool startProduction ( int elementID )
	{
		bool enoughResources = ResourcesManager.getInstance ().handleMinusResources ( FLElementsConstructionCosts.COSTS_VALUES[elementID].metal, FLElementsConstructionCosts.COSTS_VALUES[elementID].plastic, FLElementsConstructionCosts.COSTS_VALUES[elementID].vines );

		if ((( enoughResources ) || FLGlobalVariables.TUTORIAL_MENU ) && ( myMomClass.myProductionSlots.Count < myMomClass.numberOfSlotsUnblocked ))
		{
			myMomClass.myProductionSlots.Add ( new FL_MOMControl.ProductionSlot ( elementID, FLElementsConstructionCosts.COSTS_VALUES[elementID].time ));
			
			SaveDataManager.save ( SaveDataManager.MOM_SLOTS_ELEMENT_PREFIX + ( myMomClass.myProductionSlots.Count - 1 ).ToString () + myMomClass.position[0].ToString () + "," + myMomClass.position[1].ToString (), elementID );
			
			_slotObjects[myMomClass.myProductionSlots.Count - 1].transform.Find ( "textSlot" ).localPosition = new Vector3 ( 0f, -0.67f, 1.05f );
			
			_slotObjects[myMomClass.myProductionSlots.Count - 1].transform.Find ( "element" ).renderer.enabled = true;
			_slotObjects[myMomClass.myProductionSlots.Count - 1].transform.Find ( "element" ).renderer.material.mainTexture = FLLevelControl.getInstance ().gameElements[elementID];
			
			if ( myMomClass.myProductionSlots.Count == 1 )
			{
				iTween.ScaleTo ( _slotObjects[myMomClass.myProductionSlots.Count - 1], iTween.Hash ( "time", 0.3f, "easetype", iTween.EaseType.linear, "scale", _startSlotInitialScale * 1.1f ));
				SaveDataManager.save ( SaveDataManager.MOM_SLOT_0_TIME_PREFIX + myMomClass.position[0].ToString () + "," + myMomClass.position[1].ToString (), (int) ( DateTime.UtcNow.Ticks / 10000000 ));
				
				_slotObjects[myMomClass.myProductionSlots.Count - 1].transform.Find ( "textSlot" ).GetComponent < TextMesh > ().text = myMomClass.myProductionSlots[myMomClass.myProductionSlots.Count - 1].timeLeft.ToString ( "F0" );
				myBuyButton.transform.Find ( "textAmount" ).GetComponent < TextMesh > ().text = FLElementsConstructionCosts.COSTS_VALUES[myMomClass.myProductionSlots[0].elementID].premiumCurrency.ToString ();
				setButtonBuyActive ( true );
				if ( FLGlobalVariables.TUTORIAL_MENU )
				{
					TutorialUIObjectTapComponent currentTutorialUIObjectTapComponent = myBuyButton.AddComponent < TutorialUIObjectTapComponent > ();
					currentTutorialUIObjectTapComponent.arrowFromAbove = true;
					currentTutorialUIObjectTapComponent.moveUp = 0.2f;
				}
			}
			else
			{
				_slotObjects[myMomClass.myProductionSlots.Count - 1].transform.Find ( "textSlot" ).GetComponent < TextMesh > ().text = "Waiting...";
			}
			
			return true;
		}
		else if ( myMomClass.myProductionSlots.Count < myMomClass.numberOfSlotsUnblocked )
		{
			_myNotEnoughResourcesScreen = FLUIControl.getInstance ().createPopup ( FLFactoryRoomManager.getInstance ().momNotEnoughResourcesPrefab );
			
			if ( _myNotEnoughResourcesScreen != null )
			{
				_myNotEnoughResourcesScreen.GetComponent < FL_MOMNotEoughResourcesPanelControl > ().myElementID = elementID;
				_myNotEnoughResourcesScreen.GetComponent < FL_MOMNotEoughResourcesPanelControl > ().myMomClass = myMomClass;
			}
			return false;
		}
		
		return false;
	}
	
	void Update ()
	{
		_countUpdate -= Time.deltaTime;
		
		if ( _countUpdate > 0f ) return;
		
		_countUpdate = 1f;

		if ( _slotObjects.Count < myMomClass.numberOfSlots )
		{
			_backPanel.transform.localScale = new Vector3 ( _ADD_POSITION_FOR_NOT_ALL_SLOTS + ( myMomClass.numberOfSlotsUnblocked + 1 ) * _ADD_POSITION_MULTIPLYER, _backPanel.transform.localScale.y, _backPanel.transform.localScale.z );
			if(posAdusted == false)
			{
				_backPanel.transform.position = new Vector3 (_backPanel.transform.position.x - (0.015f * ( _slotObjects.Count - 1)) , _backPanel.transform.position.y, _backPanel.transform.position.z);
				posAdusted = true;
			}
		}
		else
		{
			_backPanel.transform.localScale = new Vector3 ( _ADD_POSITION_FOR_NOT_ALL_SLOTS + myMomClass.numberOfSlotsUnblocked * _ADD_POSITION_MULTIPLYER, _backPanel.transform.localScale.y, _backPanel.transform.localScale.z );
			if(posAdusted == false)
			{
				_backPanel.transform.position = new Vector3 (_backPanel.transform.position.x - (0.015f * ( _slotObjects.Count - 1)), _backPanel.transform.position.y, _backPanel.transform.position.z);
				posAdusted = true;
			}
		}

		if (( myMomClass.myProductionSlots.Count > 0 ) && ( myMomClass.myProductionSlots[0] != null ))
		{
			if ( _slotObjects[0].transform.Find ( "textSlot" ).GetComponent < GameTextControl > ()) Destroy ( _slotObjects[0].transform.Find ( "textSlot" ).GetComponent < GameTextControl > ());
			_slotObjects[0].transform.Find ( "textSlot" ).GetComponent < TextMesh > ().text = TimeScaleManager.getTimeString ((int) myMomClass.myProductionSlots[0].timeLeft );
			_slotObjects[0].transform.Find ( "textSlot" ).localPosition = new Vector3 ( 0f, -0.67f, 1.1f );
			_slotObjects[0].transform.Find ( "slot" ).renderer.material.mainTexture = FLFactoryRoomManager.getInstance ().momWorkingSlotTexture;
		}
		else
		{
			_slotObjects[0].transform.Find ( "slot" ).renderer.material.mainTexture = FLFactoryRoomManager.getInstance ().momNotWorkingSlotTexture;
		}
	}
	
	public void updateSlots ()
	{
		for ( int i = 0; i < myMomClass.numberOfSlotsUnblocked; i++ )
		{
			if (( myMomClass.myProductionSlots.Count > i ) && ( myMomClass.myProductionSlots[i] != null ))
			{
				_slotObjects[i].transform.Find ( "element" ).renderer.enabled = true;
				_slotObjects[i].transform.Find ( "element" ).renderer.material.mainTexture = FLLevelControl.getInstance ().gameElements[myMomClass.myProductionSlots[i].elementID];
				
				if ( i == 0 )
				{
					_slotObjects[i].transform.Find ( "textSlot" ).localPosition = new Vector3 ( 0f, -0.67f, 1.1f );
					if ( _slotObjects[i].transform.Find ( "textSlot" ).GetComponent < GameTextControl > ()) Destroy ( _slotObjects[i].transform.Find ( "textSlot" ).GetComponent < GameTextControl > ());
					_slotObjects[i].transform.Find ( "textSlot" ).GetComponent < TextMesh > ().text = TimeScaleManager.getTimeString ((int) myMomClass.myProductionSlots[i].timeLeft );
					iTween.ScaleTo ( _slotObjects[i], iTween.Hash ( "time", 0.3f, "easetype", iTween.EaseType.linear, "scale", _startSlotInitialScale * 1.1f ));
					myBuyButton.transform.Find ( "textAmount" ).GetComponent < TextMesh > ().text = FLElementsConstructionCosts.COSTS_VALUES[myMomClass.myProductionSlots[i].elementID].premiumCurrency.ToString ();
					setButtonBuyActive ( true );
				}
				else
				{
					_slotObjects[i].transform.Find ( "textSlot" ).localPosition = new Vector3 ( 0f, -0.67f, 1.05f );
					if ( ! _slotObjects[i].transform.Find ( "textSlot" ).GetComponent < GameTextControl > ())
					{
						GameTextControl currentGameTextControl = _slotObjects[i].transform.Find ( "textSlot" ).gameObject.AddComponent < GameTextControl > ();
						currentGameTextControl.myKey = "ui_sign_waiting";
					}
					else
					{
						_slotObjects[i].transform.Find ( "textSlot" ).GetComponent < GameTextControl > ().myKey = "ui_sign_waiting";
					}
				}
			}
			else
			{				
				_slotObjects[i].transform.Find ( "element" ).renderer.enabled = false;
				
				if ( ! _slotObjects[i].transform.Find ( "textSlot" ).GetComponent < GameTextControl > ())
				{
					GameTextControl currentGameTextControl = _slotObjects[i].transform.Find ( "textSlot" ).gameObject.AddComponent < GameTextControl > ();
					currentGameTextControl.myKey = "ui_sign_empty";
					currentGameTextControl.updateText ();
				}
				else
				{
					_slotObjects[i].transform.Find ( "textSlot" ).GetComponent < GameTextControl > ().myKey = "ui_sign_empty";
					_slotObjects[i].transform.Find ( "textSlot" ).GetComponent < GameTextControl > ().updateText ();
				}
				
				_slotObjects[i].transform.Find ( "textSlot" ).localPosition = new Vector3 ( 0f, -0.67f, 0.45f );
				iTween.ScaleTo ( _slotObjects[i], iTween.Hash ( "time", 0.3f, "easetype", iTween.EaseType.linear, "scale", _startSlotInitialScale ));
			}
		}
		
		if ( myMomClass.myProductionSlots.Count <= 0 )
		{
			setButtonBuyActive ( false );
		}
	}

	public void update ()
	{
		_countUpdate = 0f;
	}
	
	private void setButtonBuyActive ( bool isOn )
	{
		myBuyButton.renderer.enabled = isOn;
		myBuyButton.collider.enabled = isOn;
		myBuyButton.transform.Find ( "textAmount" ).gameObject.SetActive ( isOn );
	}
}
