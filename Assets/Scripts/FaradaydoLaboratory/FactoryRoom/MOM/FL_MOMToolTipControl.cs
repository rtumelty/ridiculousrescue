using UnityEngine;
using System.Collections;

public class FL_MOMToolTipControl : MonoBehaviour 
{
	//*************************************************************//
	public int myElementID;
	//*************************************************************//
	private TextMesh _timeText;
	private TextMesh _nameText;
	
	private Material _myElement01Material;
	private Material _myElement02Material;
	private Material _myElement03Material;
	
	private TextMesh _element01HavingText;
	private TextMesh _element02HavingText;
	private TextMesh _element03HavingText;
	private TextMesh _element01RequiredText;
	private TextMesh _element02RequiredText;
	private TextMesh _element03RequiredText;

	private float _countUpdate = 0f;
	//*************************************************************//
	void Start () 
	{
		_timeText = transform.Find ( "textTime" ).GetComponent < TextMesh > ();
		_nameText = transform.Find ( "textName" ).GetComponent < TextMesh > ();
		
		_myElement01Material = transform.Find ( "element01" ).renderer.material;
		_myElement02Material = transform.Find ( "element02" ).renderer.material;
		_myElement03Material = transform.Find ( "element03" ).renderer.material;
		
		_element01HavingText = transform.Find ( "element01" ).Find ( "textNumberYouHave" ).GetComponent < TextMesh > ();
		_element02HavingText = transform.Find ( "element02" ).Find ( "textNumberYouHave" ).GetComponent < TextMesh > ();
		_element03HavingText = transform.Find ( "element03" ).Find ( "textNumberYouHave" ).GetComponent < TextMesh > ();
		
		_element01RequiredText = transform.Find ( "element01" ).Find ( "textNumberRequeired" ).GetComponent < TextMesh > ();
		_element02RequiredText = transform.Find ( "element02" ).Find ( "textNumberRequeired" ).GetComponent < TextMesh > ();
		_element03RequiredText = transform.Find ( "element03" ).Find ( "textNumberRequeired" ).GetComponent < TextMesh > ();
		
		_myElement01Material.mainTexture = FLLevelControl.getInstance ().gameElements[GameElements.ICON_METAL];
		_myElement02Material.mainTexture = FLLevelControl.getInstance ().gameElements[GameElements.ICON_PLASTIC];
		_myElement03Material.mainTexture = FLLevelControl.getInstance ().gameElements[GameElements.ICON_VINES];
		
		switch ( myElementID )
		{
			case GameElements.ICON_RECHARGEOCORES:
				_nameText.GetComponent < GameTextControl > ().myKey = "ui_name_factory_storage_rechargeocores";
				break;
			case GameElements.ICON_REDIRECTORS:
				_nameText.GetComponent < GameTextControl > ().myKey = "ui_name_factory_storage_redirectors";
				break;
		}
	}
	
	void Update ()
	{
		_countUpdate -= Time.deltaTime;
		
		if ( _countUpdate > 0f ) return;
		
		_countUpdate = 1f;

		if ( GameGlobalVariables.Stats.METAL_IN_CONTAINERS - FLElementsConstructionCosts.COSTS_VALUES[myElementID].metal < 0 )
		{
			_element01HavingText.renderer.material = GameGlobalVariables.FontMaterials.RED_TEXT_02;
		}
		else
		{
			_element01HavingText.renderer.material = GameGlobalVariables.FontMaterials.BLACK_TEXT_02;
		}
		
		if ( GameGlobalVariables.Stats.PLASTIC_IN_CONTAINERS - FLElementsConstructionCosts.COSTS_VALUES[myElementID].plastic < 0 )
		{
			_element02HavingText.renderer.material = GameGlobalVariables.FontMaterials.RED_TEXT_02;
		}
		else
		{
			_element02HavingText.renderer.material = GameGlobalVariables.FontMaterials.BLACK_TEXT_02;
		}
		
		if ( GameGlobalVariables.Stats.VINES_IN_CONTAINERS - FLElementsConstructionCosts.COSTS_VALUES[myElementID].vines < 0 )
		{
			_element03HavingText.renderer.material = GameGlobalVariables.FontMaterials.RED_TEXT_02;
		}
		else
		{
			_element03HavingText.renderer.material = GameGlobalVariables.FontMaterials.BLACK_TEXT_02;
		}
		
		_element01HavingText.text = GameGlobalVariables.Stats.METAL_IN_CONTAINERS.ToString ();
		_element02HavingText.text = GameGlobalVariables.Stats.PLASTIC_IN_CONTAINERS.ToString ();
		_element03HavingText.text = GameGlobalVariables.Stats.VINES_IN_CONTAINERS.ToString ();
		
		_element01RequiredText.text = "/ " + FLElementsConstructionCosts.COSTS_VALUES[myElementID].metal.ToString ();
		_element02RequiredText.text = "/ " + FLElementsConstructionCosts.COSTS_VALUES[myElementID].plastic.ToString ();
		_element03RequiredText.text = "/ " + FLElementsConstructionCosts.COSTS_VALUES[myElementID].vines.ToString ();
		
		_timeText.text = TimeScaleManager.getTimeString ( FLElementsConstructionCosts.COSTS_VALUES[myElementID].time );
	}
}
