using UnityEngine;
using System.Collections;

public class FLStorageContainerUpgradeButton : MonoBehaviour 
{
	//*************************************************************//
	public FLStorageContainerClass myStorageContainerClass;
	//*************************************************************//
	private GameObject _myComboUIInfoScreen;
	private TextMesh _upgradeCostTextMesh;

	private float _countUpdate = 0f;
	//*************************************************************//
	
	void Start ()
	{
		//=========================================Daves Edit===========================================
		switch (myStorageContainerClass.type) 
		{
		case FLStorageContainerClass.STORAGE_TYPE_METAL:
			_upgradeCostTextMesh = transform.Find ( "iconMetal" ).transform.Find ( "textMetal" ).GetComponent < TextMesh > ();

				break;
		case FLStorageContainerClass.STORAGE_TYPE_PLASTIC:
			_upgradeCostTextMesh = transform.Find ( "iconPlastic" ).transform.Find ( "textPlastic" ).GetComponent < TextMesh > ();

				break;
		case FLStorageContainerClass.STORAGE_TYPE_VINES:
			_upgradeCostTextMesh = transform.Find ( "iconPlastic" ).transform.Find ( "textPlastic" ).GetComponent < TextMesh > ();

				break;
		default:
			//_upgradeCostTextMesh = transform.Find ( "iconTechnoSeed" ).transform.Find ( "textTechnoSeed" ).GetComponent < TextMesh > ();
			break;
		}
		//=========================================Daves Edit===========================================
	}
	
	void OnMouseUp ()
	{
		if ( FLGlobalVariables.checkForMenus ()) return;
		SoundManager.getInstance ().playSound ( SoundManager.HEADER_TAP );
		
		handleTouched ();
	}
	
	private void handleTouched ()
	{
		if(myStorageContainerClass.level < FLStorageContainerClass.MAXIMUM_LEVEL)
		{
			switch (myStorageContainerClass.type) 
			{
			case FLStorageContainerClass.STORAGE_TYPE_METAL:
					_myComboUIInfoScreen = FLUIControl.getInstance ().createPopup (FLFactoryRoomManager.getInstance ().storageComboUIStorageUpgradeScreenPrefab);
					break;
			default:
					_myComboUIInfoScreen = FLUIControl.getInstance ().createPopup (FLFactoryRoomManager.getInstance ().storageComboUIStorageUpgradeScreenPlasticPrefab);
					break;
			}
			if(myStorageContainerClass.upgrading == true)
			{
				GameObject myWarningBar = _myComboUIInfoScreen.transform.Find ("warningBar").transform.gameObject;
				GameObject myGreenBar = _myComboUIInfoScreen.transform.Find ("greenBar").transform.gameObject;
				GameObject myTimeText = _myComboUIInfoScreen.transform.Find ("textTime").transform.gameObject;
				myGreenBar.transform.position = new Vector3 (myGreenBar.transform.position.x, myGreenBar.transform.position.y, myWarningBar.transform.position.z);
				myTimeText.transform.position = new Vector3(myTimeText.transform.position.x, myTimeText.transform.position.y, myGreenBar.transform.position.z + 0.17f);
				_myComboUIInfoScreen.transform.Find ("textNotEnoughResources").transform.position = new Vector3(myTimeText.transform.position.x, myTimeText.transform.position.y, myGreenBar.transform.position.z + 1000);
				_myComboUIInfoScreen.transform.Find ( "textGeneral" ).GetComponent < TextMesh > ().GetComponent < GameTextControl > ().myKey = "ui_sign_upgrade_in_progress";
				myWarningBar.SetActive(false);
			}
			if ( _myComboUIInfoScreen != null ) 
			{
				_myComboUIInfoScreen.GetComponent < FLStorageContainerUpgradeScreenControl > ().myStorageContainerClass = myStorageContainerClass;
			}
		}
	}
	
	void Update ()
	{
		_countUpdate -= Time.deltaTime;
		
		if ( _countUpdate > 0f ) return;
		
		_countUpdate = 1f;

		if ( myStorageContainerClass.level + 1 <= FLStorageContainerClass.MAXIMUM_LEVEL )
		{
			//=====================================Daves Edit==========================================
			switch( myStorageContainerClass.type )
			{
			case FLStorageContainerClass.STORAGE_TYPE_METAL:
				if(! ResourcesManager.getInstance ().handleMinusResources ( FLStorageContainerClass.LEVELS_STATS_METAL[myStorageContainerClass.level + 1].cost, 0, 0, true))
				{
					_upgradeCostTextMesh.renderer.material = GameGlobalVariables.FontMaterials.RED_TEXT;
					_upgradeCostTextMesh.text = FLStorageContainerClass.LEVELS_STATS_METAL[myStorageContainerClass.level + 1].cost.ToString ();
				}
				break;
			case FLStorageContainerClass.STORAGE_TYPE_PLASTIC:
				if(! ResourcesManager.getInstance ().handleMinusResources ( 0, FLStorageContainerClass.LEVELS_STATS_PLASTIC[myStorageContainerClass.level + 1].cost, 0, true))
				{
					_upgradeCostTextMesh.renderer.material = GameGlobalVariables.FontMaterials.RED_TEXT;
					_upgradeCostTextMesh.text = FLStorageContainerClass.LEVELS_STATS_PLASTIC[myStorageContainerClass.level + 1].cost.ToString ();
				}
				break;
			case FLStorageContainerClass.STORAGE_TYPE_VINES:
				if(! ResourcesManager.getInstance ().handleMinusResources ( 0, 0, FLStorageContainerClass.LEVELS_STATS_VINES[myStorageContainerClass.level + 1].cost, true))
				{
					_upgradeCostTextMesh.renderer.material = GameGlobalVariables.FontMaterials.RED_TEXT;
					_upgradeCostTextMesh.text = FLStorageContainerClass.LEVELS_STATS_VINES[myStorageContainerClass.level + 1].cost.ToString ();
				}
				break;
			default:
				{
					//_upgradeCostTextMesh.renderer.material = GameGlobalVariables.FontMaterials.BLACK_TEXT;
					//_upgradeCostTextMesh.text = FLStorageContainerClass.LEVELS_STATS[myStorageContainerClass.level + 1].cost.ToString ();
				}
				break;
			}
			//=====================================Daves Edit==========================================
			/*if ( ! ResourcesManager.getInstance ().handleBuyWithPremiumCurrency ( FLStorageContainerClass.LEVELS_STATS[myStorageContainerClass.level + 1].cost, true ))
			{
				_upgradeCostTextMesh.renderer.material = GameGlobalVariables.FontMaterials.RED_TEXT;
				_upgradeCostTextMesh.text = FLStorageContainerClass.LEVELS_STATS[myStorageContainerClass.level + 1].cost.ToString ();
			}
			else
			{
				_upgradeCostTextMesh.renderer.material = GameGlobalVariables.FontMaterials.BLACK_TEXT;
				_upgradeCostTextMesh.text = FLStorageContainerClass.LEVELS_STATS[myStorageContainerClass.level + 1].cost.ToString ();
			}*/
		}
		else
		{
			_upgradeCostTextMesh.renderer.material = GameGlobalVariables.FontMaterials.BLACK_TEXT;

			switch( myStorageContainerClass.type )
			{
			case FLStorageContainerClass.STORAGE_TYPE_METAL:
				if(myStorageContainerClass.level < FLStorageContainerClass.MAXIMUM_LEVEL)
				{
					_upgradeCostTextMesh.text = FLStorageContainerClass.LEVELS_STATS_METAL[myStorageContainerClass.level + 1].cost.ToString ();
				}	
					break;
			case FLStorageContainerClass.STORAGE_TYPE_PLASTIC:
				if(myStorageContainerClass.level < FLStorageContainerClass.MAXIMUM_LEVEL)
				{
					_upgradeCostTextMesh.text = FLStorageContainerClass.LEVELS_STATS_PLASTIC[myStorageContainerClass.level + 1].cost.ToString ();
				}
				break;
			case FLStorageContainerClass.STORAGE_TYPE_VINES:
				if(myStorageContainerClass.level < FLStorageContainerClass.MAXIMUM_LEVEL)
				{
					_upgradeCostTextMesh.text = FLStorageContainerClass.LEVELS_STATS_VINES[myStorageContainerClass.level + 1].cost.ToString ();
				}
				break;
			}
		}
	}
}
