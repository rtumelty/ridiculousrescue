using UnityEngine;
using System.Collections;

public class FL_MOMNotEoughResourcesPanelControl : MonoBehaviour 
{
	//*************************************************************//
	public int myElementID;
	public FL_MOMClass myMomClass;
	public FLStorageContainerClass myStorageContainerClass;
	public bool fromContainer = false;
	public int myContainerUpgradeCost = 0;
	public Transform centrePos;
	public int activeElement = 0;
	//*************************************************************//
	//private TextMesh _infoText;
	private TextMesh _costText;
	private GameObject element1;
	private GameObject element2;
	private GameObject element3;
	private TextMesh _element01HavingText;
	private TextMesh _element02HavingText;
	private TextMesh _element03HavingText;
	private TextMesh _element01RequiredText;
	private TextMesh _element02RequiredText;
	private TextMesh _element03RequiredText;
	private float _countUpdate = 0f;
	//*************************************************************//
	void Awake ()
	{
		FLGlobalVariables.POPUP_UI_SCREEN = true;
	}
	
	void Start () 
	{
		//_infoText = transform.Find ( "textInfo" ).GetComponent < TextMesh > ();
		_costText = transform.Find( "confirmButton" ).transform.Find ( "textCost" ).GetComponent < TextMesh > ();
		
		_element01HavingText = transform.Find ( "element01" ).Find ( "textNumberYouHave" ).GetComponent < TextMesh > ();
		_element02HavingText = transform.Find ( "element02" ).Find ( "textNumberYouHave" ).GetComponent < TextMesh > ();
		_element03HavingText = transform.Find ( "element03" ).Find ( "textNumberYouHave" ).GetComponent < TextMesh > ();
		
		_element01RequiredText = transform.Find ( "element01" ).Find ( "textNumberRequeired" ).GetComponent < TextMesh > ();
		_element02RequiredText = transform.Find ( "element02" ).Find ( "textNumberRequeired" ).GetComponent < TextMesh > ();
		_element03RequiredText = transform.Find ( "element03" ).Find ( "textNumberRequeired" ).GetComponent < TextMesh > ();
		
		transform.transform.Find ( "confirmButton" ).GetComponent < FL_MOMNotEnoughResourcesConfirmButtonControl > ().myElementID = myElementID;
		transform.transform.Find ( "confirmButton" ).GetComponent < FL_MOMNotEnoughResourcesConfirmButtonControl > ().myMomClass = myMomClass;

		element1 = transform.Find("element01").gameObject;
		element2 = transform.Find("element02").gameObject;
		element3 = transform.Find("element03").gameObject;
		centrePos = element2.transform;
	}
	
	void Update ()
	{
		_countUpdate -= Time.deltaTime;
		
		if ( _countUpdate > 0f ) return;
		
		_countUpdate = 1f;
		if(fromContainer == true)
		{
			print ("from container");
			_element01HavingText.text = GameGlobalVariables.Stats.METAL_IN_CONTAINERS.ToString ();
			_element02HavingText.text = GameGlobalVariables.Stats.PLASTIC_IN_CONTAINERS.ToString ();
			_element03HavingText.text = GameGlobalVariables.Stats.VINES_IN_CONTAINERS.ToString ();
			
			_element01RequiredText.text = "/ " + myContainerUpgradeCost.ToString ();
			_element02RequiredText.text = "/ " + myContainerUpgradeCost.ToString ();
			_element03RequiredText.text = "/ " + myContainerUpgradeCost.ToString ();

			switch(activeElement)
			{
			case 1:
			{
				element1.transform.position = centrePos.position;
				element2.SetActive(false);
				element3.SetActive(false);
				_element01HavingText.renderer.material = GameGlobalVariables.FontMaterials.RED_TEXT;
				transform.Find ( "confirmButton" ).GetComponent < FL_MOMNotEnoughResourcesConfirmButtonControl > ().cost = myContainerUpgradeCost - GameGlobalVariables.Stats.METAL_IN_CONTAINERS;
				transform.Find ( "confirmButton" ).GetComponent < FL_MOMNotEnoughResourcesConfirmButtonControl > ().myStorageContainerClass = myStorageContainerClass;
				_costText.text = ( myContainerUpgradeCost - GameGlobalVariables.Stats.METAL_IN_CONTAINERS ).ToString ();
			}
				break;
			case 2:
			{
				element1.SetActive(false);
				element3.SetActive(false);
				_element02HavingText.renderer.material = GameGlobalVariables.FontMaterials.RED_TEXT;
				transform.Find ( "confirmButton" ).GetComponent < FL_MOMNotEnoughResourcesConfirmButtonControl > ().cost = myContainerUpgradeCost - GameGlobalVariables.Stats.PLASTIC_IN_CONTAINERS;
				transform.Find ( "confirmButton" ).GetComponent < FL_MOMNotEnoughResourcesConfirmButtonControl > ().myStorageContainerClass = myStorageContainerClass;
				_costText.text = ( myContainerUpgradeCost - GameGlobalVariables.Stats.PLASTIC_IN_CONTAINERS ).ToString ();
			}
				break;
			case 3:
			{
				element3.transform.position = centrePos.position;
				element1.SetActive(false);
				element2.SetActive(false);
				_element03HavingText.renderer.material = GameGlobalVariables.FontMaterials.RED_TEXT;
				transform.Find ( "confirmButton" ).GetComponent < FL_MOMNotEnoughResourcesConfirmButtonControl > ().cost = myContainerUpgradeCost - GameGlobalVariables.Stats.VINES_IN_CONTAINERS;
				transform.Find ( "confirmButton" ).GetComponent < FL_MOMNotEnoughResourcesConfirmButtonControl > ().myStorageContainerClass = myStorageContainerClass;
				_costText.text = ( myContainerUpgradeCost - GameGlobalVariables.Stats.VINES_IN_CONTAINERS ).ToString ();
			}
				break;
			}
		}
		if(fromContainer == false)
		{
			//int totalMissing = 0;
			if ( GameGlobalVariables.Stats.METAL_IN_CONTAINERS - FLElementsConstructionCosts.COSTS_VALUES[myElementID].metal < 0 )
			{
				//totalMissing -= GameGlobalVariables.Stats.METAL_IN_CONTAINERS - FLElementsConstructionCosts.COSTS_VALUES[myElementID].metal;
				_element01HavingText.renderer.material = GameGlobalVariables.FontMaterials.RED_TEXT;
			}
			else
			{
				_element01HavingText.renderer.material = GameGlobalVariables.FontMaterials.BLACK_TEXT;
			}
			
			if ( GameGlobalVariables.Stats.PLASTIC_IN_CONTAINERS - FLElementsConstructionCosts.COSTS_VALUES[myElementID].plastic < 0 )
			{
				//totalMissing -= GameGlobalVariables.Stats.PLASTIC_IN_CONTAINERS - FLElementsConstructionCosts.COSTS_VALUES[myElementID].plastic;
				_element02HavingText.renderer.material = GameGlobalVariables.FontMaterials.RED_TEXT;
			}
			else
			{
				_element02HavingText.renderer.material = GameGlobalVariables.FontMaterials.BLACK_TEXT;
			}
			
			if ( GameGlobalVariables.Stats.VINES_IN_CONTAINERS - FLElementsConstructionCosts.COSTS_VALUES[myElementID].vines < 0 )
			{
				//totalMissing -= GameGlobalVariables.Stats.VINES_IN_CONTAINERS - FLElementsConstructionCosts.COSTS_VALUES[myElementID].vines;
				_element03HavingText.renderer.material = GameGlobalVariables.FontMaterials.RED_TEXT;
			}
			else
			{
				_element03HavingText.renderer.material = GameGlobalVariables.FontMaterials.BLACK_TEXT;
			}
			
			_element01HavingText.text = GameGlobalVariables.Stats.METAL_IN_CONTAINERS.ToString ();
			_element02HavingText.text = GameGlobalVariables.Stats.PLASTIC_IN_CONTAINERS.ToString ();
			_element03HavingText.text = GameGlobalVariables.Stats.VINES_IN_CONTAINERS.ToString ();
			
			_element01RequiredText.text = "/ " + FLElementsConstructionCosts.COSTS_VALUES[myElementID].metal.ToString ();
			_element02RequiredText.text = "/ " + FLElementsConstructionCosts.COSTS_VALUES[myElementID].plastic.ToString ();
			_element03RequiredText.text = "/ " + FLElementsConstructionCosts.COSTS_VALUES[myElementID].vines.ToString ();

			int numberOfSeedsRequired = 0;
			if ( GameGlobalVariables.Stats.METAL_IN_CONTAINERS < FLElementsConstructionCosts.COSTS_VALUES[myElementID].metal ) numberOfSeedsRequired += ( FLElementsConstructionCosts.COSTS_VALUES[myElementID].metal - GameGlobalVariables.Stats.METAL_IN_CONTAINERS );
			if ( GameGlobalVariables.Stats.PLASTIC_IN_CONTAINERS < FLElementsConstructionCosts.COSTS_VALUES[myElementID].plastic ) numberOfSeedsRequired += ( FLElementsConstructionCosts.COSTS_VALUES[myElementID].plastic - GameGlobalVariables.Stats.PLASTIC_IN_CONTAINERS );
			if ( GameGlobalVariables.Stats.VINES_IN_CONTAINERS < FLElementsConstructionCosts.COSTS_VALUES[myElementID].vines ) numberOfSeedsRequired += ( FLElementsConstructionCosts.COSTS_VALUES[myElementID].vines - GameGlobalVariables.Stats.VINES_IN_CONTAINERS );

			switch ( myElementID )
			{
			case GameElements.ICON_RECHARGEOCORES:
				if ( ! ResourcesManager.getInstance ().handleBuyWithPremiumCurrency ( numberOfSeedsRequired, true ))
				{
					_costText.renderer.material = GameGlobalVariables.FontMaterials.RED_TEXT;
				}
				else
				{
					_costText.renderer.material = GameGlobalVariables.FontMaterials.BLACK_TEXT;
				}

				transform.Find ( "confirmButton" ).GetComponent < FL_MOMNotEnoughResourcesConfirmButtonControl > ().cost = numberOfSeedsRequired;
				_costText.text = ( numberOfSeedsRequired ).ToString ();
				break;
			case GameElements.ICON_REDIRECTORS:
				if ( ! ResourcesManager.getInstance ().handleBuyWithPremiumCurrency ( numberOfSeedsRequired, true ))
				{
					_costText.renderer.material = GameGlobalVariables.FontMaterials.RED_TEXT;
				}
				else
				{
					_costText.renderer.material = GameGlobalVariables.FontMaterials.BLACK_TEXT;
				}

				transform.transform.Find ( "confirmButton" ).GetComponent < FL_MOMNotEnoughResourcesConfirmButtonControl > ().cost = numberOfSeedsRequired;
				_costText.text = ( numberOfSeedsRequired ).ToString ();
				break;
			}
		}
	}
}
