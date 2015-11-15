using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class FLMissionBriefScreenControl : MonoBehaviour 
{
	//*************************************************************//
	public GameGlobalVariables.Missions.LevelClass myLevelClass;
	//*************************************************************//
	private TextMesh _infoText;
	private TextMesh _generaltext;
	
	private GameObject _buttonConfirm;
	private GameObject _buttonsStar;
	
	private GameObject _notEnoughResourcesObject;
	
	private GameObject _elementStart;
	private List < GameObject > _elements;
	//*************************************************************//
	void Start ()
	{
		_notEnoughResourcesObject = transform.Find ( "notEnoughResourcesBack" ).gameObject;
		_buttonConfirm = transform.Find ( "buttonConfirm" ).gameObject;
		_buttonsStar = transform.Find ( "buttonConfirm" ).Find ( "iconStar" ).gameObject;
		FLMissionScreenConfirmButtonControl currentFLMissionScreenConfirmButtonControl = _buttonConfirm.AddComponent < FLMissionScreenConfirmButtonControl > ();
		currentFLMissionScreenConfirmButtonControl.myLevelClass = myLevelClass;

		_buttonConfirm.AddComponent < FLMissionGoToFactoryButtonControl > ();
		
		_infoText = transform.Find ( "textInfo" ).GetComponent < TextMesh > ();
		_generaltext = transform.Find ( "textGeneral" ).GetComponent < TextMesh > ();
		
		_elementStart = transform.Find ( "element" ).gameObject;
		
		_elements = new List < GameObject > ();
		_elementStart.renderer.material.mainTexture = FLLevelControl.getInstance ().gameElements[myLevelClass.requiredElements[0].elementID];
		_elementStart.transform.Find ( "textElementNeeded" ).GetComponent < TextMesh > ().text = "/ " + myLevelClass.requiredElements[0].amountRequiered.ToString ();
		_elements.Add ( _elementStart );
		
		for ( int i = 1; i < myLevelClass.requiredElements.Count; i++ )
		{
			GameObject currentElementObject = ( GameObject ) Instantiate ( _elementStart, _elementStart.transform.position + Vector3.right * i * 2f, _elementStart.transform.rotation );
			currentElementObject.transform.parent = _elementStart.transform.parent;
			currentElementObject.transform.localScale = _elementStart.transform.localScale;
			currentElementObject.renderer.material.mainTexture = FLLevelControl.getInstance ().gameElements[myLevelClass.requiredElements[i].elementID];
			currentElementObject.transform.Find ( "textElementNeeded" ).GetComponent < TextMesh > ().text = "/ " + myLevelClass.requiredElements[i].amountRequiered.ToString ();
			_elements.Add ( currentElementObject );
		}
		
		int j = 0;
		foreach ( GameObject element in _elements )
		{
			switch ( myLevelClass.requiredElements[j].elementID )
			{
				case GameElements.ICON_RECHARGEOCORES:
					if ( GameGlobalVariables.Stats.RECHARGEOCORES_IN_CONTAINERS - myLevelClass.requiredElements[j].amountRequiered < 0 && ! GameGlobalVariables.BLOCK_LAB_ENTERED )
					{
						element.transform.Find ( "textElementYouHave" ).renderer.material = GameGlobalVariables.FontMaterials.RED_TEXT_02;
						element.transform.Find ( "textElementYouHave" ).GetComponent < TextMesh > ().text = GameGlobalVariables.Stats.RECHARGEOCORES_IN_CONTAINERS.ToString ();
					}
					else
					{
						element.transform.Find ( "textElementYouHave" ).renderer.material = GameGlobalVariables.FontMaterials.BLACK_TEXT_02;
						element.transform.Find ( "textElementYouHave" ).GetComponent < TextMesh > ().text = myLevelClass.requiredElements[j].amountRequiered.ToString ();
					}	
					break;
				case GameElements.ICON_REDIRECTORS:
					if ( GameGlobalVariables.Stats.REDIRECTORS_IN_CONTAINERS - myLevelClass.requiredElements[j].amountRequiered < 0 )
					{
						element.transform.Find ( "textElementYouHave" ).renderer.material = GameGlobalVariables.FontMaterials.RED_TEXT_02;
						element.transform.Find ( "textElementYouHave" ).GetComponent < TextMesh > ().text = GameGlobalVariables.Stats.REDIRECTORS_IN_CONTAINERS.ToString ();
					}
					else
					{
						element.transform.Find ( "textElementYouHave" ).renderer.material = GameGlobalVariables.FontMaterials.BLACK_TEXT_02;
						element.transform.Find ( "textElementYouHave" ).GetComponent < TextMesh > ().text = myLevelClass.requiredElements[j].amountRequiered.ToString ();
					}	
					break;
			}
			
			j++;
		}
		
		foreach ( GameObject element in _elements )
		{
			element.transform.position += Vector3.left * j + Vector3.right;
		}

		if ( myLevelClass.type == FLMissionScreenNodeManager.TYPE_BONUS_NODE )
		{
			_generaltext.GetComponent < GameTextControl > ().myKey = "ui_sign_bonus_mission";
		}
		else
		{
			_generaltext.GetComponent < GameTextControl > ().myKey = "ui_sign_mission";
		}
		_generaltext.GetComponent < GameTextControl > ().addText = " " + myLevelClass.myName;
		
		FLGlobalVariables.POPUP_UI_SCREEN = true;
	}
	
	void Update ()
	{
		int j = 0;
		bool allResourcesIHave = true;
		foreach ( GameObject element in _elements )
		{
			switch ( myLevelClass.requiredElements[j].elementID )
			{
				case GameElements.ICON_RECHARGEOCORES:
					if ( GameGlobalVariables.Stats.RECHARGEOCORES_IN_CONTAINERS - myLevelClass.requiredElements[j].amountRequiered < 0 && ! GameGlobalVariables.BLOCK_LAB_ENTERED )
					{
						allResourcesIHave = false;
						element.transform.Find ( "textElementYouHave" ).renderer.material = GameGlobalVariables.FontMaterials.RED_TEXT_02;
						element.transform.Find ( "textElementYouHave" ).GetComponent < TextMesh > ().text = GameGlobalVariables.Stats.RECHARGEOCORES_IN_CONTAINERS.ToString ();
					}
					else
					{
						element.transform.Find ( "textElementYouHave" ).renderer.material = GameGlobalVariables.FontMaterials.BLACK_TEXT_02;
						element.transform.Find ( "textElementYouHave" ).GetComponent < TextMesh > ().text = myLevelClass.requiredElements[j].amountRequiered.ToString ();
					}
					break;
				case GameElements.ICON_REDIRECTORS:
					if ( GameGlobalVariables.Stats.REDIRECTORS_IN_CONTAINERS - myLevelClass.requiredElements[j].amountRequiered < 0 )
					{
						allResourcesIHave = false;
						element.transform.Find ( "textElementYouHave" ).renderer.material = GameGlobalVariables.FontMaterials.RED_TEXT_02;
						element.transform.Find ( "textElementYouHave" ).GetComponent < TextMesh > ().text = GameGlobalVariables.Stats.REDIRECTORS_IN_CONTAINERS.ToString ();
					}
					else
					{
						element.transform.Find ( "textElementYouHave" ).renderer.material = GameGlobalVariables.FontMaterials.BLACK_TEXT_02;
						element.transform.Find ( "textElementYouHave" ).GetComponent < TextMesh > ().text = myLevelClass.requiredElements[j].amountRequiered.ToString ();
					}
					break;
			}
			
			j++;
		}
		
		if ( allResourcesIHave )
		{
			_notEnoughResourcesObject.SetActive ( false );
			_infoText.GetComponent < GameTextControl > ().myKey = "ui_sign_mission_all_resources";
			_buttonConfirm.renderer.material.mainTexture = FLMissionRoomManager.getInstance ().activeButton;
			
			if ( _buttonConfirm.GetComponent < FLMissionScreenConfirmButtonControl > () == null )
			{
				FLMissionScreenConfirmButtonControl currentFLMissionScreenConfirmButtonControl = _buttonConfirm.AddComponent < FLMissionScreenConfirmButtonControl > ();
				currentFLMissionScreenConfirmButtonControl.myLevelClass = myLevelClass;
			}
			
			if ( _buttonConfirm.GetComponent < FLMissionGoToFactoryButtonControl > () != null )
			{
				Destroy ( _buttonConfirm.GetComponent < FLMissionGoToFactoryButtonControl > ());				
			}
		
			_buttonsStar.SetActive ( true );
		}
		else
		{
			_notEnoughResourcesObject.SetActive ( true );
			
			_buttonConfirm.transform.Find ( "textStart" ).GetComponent < GameTextControl > ().myKey = "ui_button_go_to_factory";	
			_buttonConfirm.renderer.material.mainTexture = FLMissionRoomManager.getInstance ().activeButton;
			
			if ( _buttonConfirm.GetComponent < FLMissionScreenConfirmButtonControl > () != null )
			{
				Destroy ( _buttonConfirm.GetComponent < FLMissionScreenConfirmButtonControl > ());
			}
			
			if ( _buttonConfirm.GetComponent < FLMissionGoToFactoryButtonControl > () == null )
			{
				_buttonConfirm.AddComponent < FLMissionGoToFactoryButtonControl > ();		
			}
			
			_buttonsStar.SetActive ( false );
		}
	}
}
