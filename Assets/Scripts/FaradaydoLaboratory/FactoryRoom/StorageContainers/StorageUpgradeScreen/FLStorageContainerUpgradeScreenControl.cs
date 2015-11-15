using UnityEngine;
using System.Collections;

public class FLStorageContainerUpgradeScreenControl : MonoBehaviour 
{
	//*************************************************************//
	public FLStorageContainerClass myStorageContainerClass;
	//*************************************************************//
	private TextMesh _generalText;
	private TextMesh _capcityText;
	private TextMesh _newCapcityText;
	private TextMesh _containerInfoText;
	private TextMesh _costText;
	private TextMesh _notEnoughResourcesText;
	private TextMesh _timeText;
	private bool happenOnce = false;
	private Material _progressBarMaterial;
	private Material _containerIconMaterial;
	private Material _iconMaterial;
	
	private GameObject _warningBarObject;
	
	private BoxCollider _myConfirmButtonBoxCollider;
	private FLStorageContainerUpgradeConfirmButtonControl _myConfirmButtonControl;
	private Texture2D _myCurrentUpgradeTexture;

	private float _countUpdate = 0f;
	public bool allowed = false;
	//*************************************************************//
	void Awake ()
	{
		_generalText = transform.Find ( "textGeneral" ).GetComponent < TextMesh > ();
		_capcityText = transform.Find ( "textCapacity" ).GetComponent < TextMesh > ();
		_newCapcityText = transform.Find ( "textNewCapacity" ).GetComponent < TextMesh > ();
		_containerInfoText = transform.Find ( "textContainerInfo" ).GetComponent < TextMesh > ();
		_costText = transform.Find ( "confirmButton" ).Find ( "textCost" ).GetComponent < TextMesh > ();
		_notEnoughResourcesText = transform.Find ( "textNotEnoughResources" ).GetComponent < TextMesh > ();
		_timeText = transform.Find ( "textTime" ).GetComponent < TextMesh > ();
		
		_progressBarMaterial = transform.Find ( "progressBar" ).renderer.material;
		_containerIconMaterial = transform.Find ( "containerIcon" ).renderer.material;
		_iconMaterial = transform.Find ( "icon" ).renderer.material;
		
		_warningBarObject = transform.Find ( "warningBar" ).gameObject;
		
		_myConfirmButtonBoxCollider = transform.Find ( "confirmButton" ).GetComponent < BoxCollider > ();
		_myConfirmButtonControl = transform.Find ( "confirmButton" ).GetComponent < FLStorageContainerUpgradeConfirmButtonControl > ();
			
		FLGlobalVariables.POPUP_UI_SCREEN = true;
		
	}
	
	void Start ()
	{
		_myConfirmButtonControl.myStorageContainerClass = myStorageContainerClass;
		
		if ( myStorageContainerClass.upgrading )
		{
			transform.Find ( "confirmButton" ).gameObject.SetActive ( false );
		}
		else
		{
			transform.Find ( "confirmButton" ).gameObject.SetActive ( true );
		}
	}

	private void haveMats()
	{
		_notEnoughResourcesText.gameObject.SetActive ( false );
		_costText.renderer.material = GameGlobalVariables.FontMaterials.WHITE_TITLE;
		_myConfirmButtonBoxCollider.renderer.material.mainTexture = FLFactoryRoomManager.getInstance ().buttonGreenTexture;
		_myConfirmButtonBoxCollider.GetComponent < OnMouseDownButtonEffectControl > ().myOnMouseDownTexture = FLFactoryRoomManager.getInstance ().buttonGreenTextureOnMouseUp;
		_warningBarObject.renderer.enabled = false;
		if(happenOnce == false && allowed == true)
		{
			GameObject myGreenBar = transform.Find ("greenBar").transform.gameObject;
			GameObject myTimeText = transform.Find ("textTime").transform.gameObject;
			myGreenBar.transform.position = new Vector3 (myGreenBar.transform.position.x, myGreenBar.transform.position.y, myGreenBar.transform.position.z - 1.3f);
			myTimeText.transform.position = new Vector3(myTimeText.transform.position.x, myTimeText.transform.position.y, myGreenBar.transform.position.z + 0.17f);
			happenOnce = true;
		}
	}

	private void dontHaveMats()
	{
		if(myStorageContainerClass.upgrading == false)
		{
			_notEnoughResourcesText.gameObject.SetActive ( true );
			_costText.renderer.material = GameGlobalVariables.FontMaterials.RED_TITLE;
			_warningBarObject.renderer.enabled = true;
			_myConfirmButtonBoxCollider.renderer.material.mainTexture = FLFactoryRoomManager.getInstance ().buttonGrayedOutTexture;
			_myConfirmButtonBoxCollider.GetComponent < OnMouseDownButtonEffectControl > ().myOnMouseDownTexture = FLFactoryRoomManager.getInstance ().buttonGrayedOutTextureOnMouseUp;
			//transform.Find ("confirmButton").GetComponent<FLStorageContainerUpgradeConfirmButtonControl> ().myStorageContainerClass = myStorageContainerClass;
		}
		else if(myStorageContainerClass.upgrading == true && happenOnce == false && allowed == true)
		{
			GameObject myGreenBar = transform.Find ("greenBar").transform.gameObject;
			GameObject myTimeText = transform.Find ("textTime").transform.gameObject;
			myGreenBar.transform.position = new Vector3 (myGreenBar.transform.position.x, myGreenBar.transform.position.y, myGreenBar.transform.position.z - 1.3f);
			myTimeText.transform.position = new Vector3(myTimeText.transform.position.x, myTimeText.transform.position.y, myGreenBar.transform.position.z + 0.17f);
			happenOnce = true;
		}
	}
	
	void Update ()
	{
		_countUpdate -= Time.deltaTime;
		
		if ( _countUpdate > 0f ) return;
		
		_countUpdate = 1f;

		_iconMaterial.mainTexture = myStorageContainerClass.iconTexture;
		
		if ( myStorageContainerClass.level + 1 <= FLStorageContainerClass.MAXIMUM_LEVEL )
		{
			if(myStorageContainerClass.upgrading == false)
			{
				_generalText.GetComponent < GameTextControl > ().addText = " " + ( myStorageContainerClass.level + 1 + 1 ).ToString () + "?";
			}

			if ( myStorageContainerClass.upgrading )
			{
				transform.Find ( "confirmButton" ).gameObject.SetActive ( false );
				_timeText.GetComponent < GameTextControl > ().addText = " " + TimeScaleManager.getTimeString ((int) myStorageContainerClass.upgradeTime );
			}
			else
			{
				transform.Find ( "confirmButton" ).gameObject.SetActive ( true );

				switch ( myStorageContainerClass.type )
				{
				case FLStorageContainerClass.STORAGE_TYPE_METAL:
					_costText.text = FLStorageContainerClass.LEVELS_STATS_METAL[myStorageContainerClass.level + 1].cost.ToString ();
					_timeText.GetComponent < GameTextControl > ().addText = " " + TimeScaleManager.getTimeString ( FLStorageContainerClass.LEVELS_STATS_METAL[myStorageContainerClass.level + 1].buildTime );
					break;
				case FLStorageContainerClass.STORAGE_TYPE_PLASTIC:
					_costText.text = FLStorageContainerClass.LEVELS_STATS_PLASTIC[myStorageContainerClass.level + 1].cost.ToString ();
					_timeText.GetComponent < GameTextControl > ().addText = " " + TimeScaleManager.getTimeString ( FLStorageContainerClass.LEVELS_STATS_PLASTIC[myStorageContainerClass.level + 1].buildTime );
					break;
				case FLStorageContainerClass.STORAGE_TYPE_VINES:
					_costText.text = FLStorageContainerClass.LEVELS_STATS_VINES[myStorageContainerClass.level + 1].cost.ToString ();
					_timeText.GetComponent < GameTextControl > ().addText = " " + TimeScaleManager.getTimeString ( FLStorageContainerClass.LEVELS_STATS_VINES[myStorageContainerClass.level + 1].buildTime );

					break;
				}
			}
			
			UnityEngine.Object[] textureObjects = null;
			string path = "";
			//==============================Daves Edit===============================

			switch ( myStorageContainerClass.type )
			{
				case FLStorageContainerClass.STORAGE_TYPE_METAL:
				_progressBarMaterial.mainTexture = FLFactoryRoomManager.getInstance ().powerBarPaneltextures[(int) ((( FLStorageContainerClass.LEVELS_STATS_METAL[myStorageContainerClass.level].capacity * 100 ) / FLStorageContainerClass.LEVELS_STATS_METAL[myStorageContainerClass.level + 1].capacity ) / 5 )];

				_capcityText.GetComponent < GameTextControl > ().addText = " " + FLStorageContainerClass.LEVELS_STATS_METAL[myStorageContainerClass.level].capacity.ToString ();
				_newCapcityText.GetComponent < GameTextControl > ().addText = " " + (FLStorageContainerClass.LEVELS_STATS_METAL[myStorageContainerClass.level + 1].capacity - FLStorageContainerClass.LEVELS_STATS_METAL[myStorageContainerClass.level].capacity) .ToString ();	
				path = "Textures/FactoryRoom/StorageContainers/Metal";
					textureObjects = Resources.LoadAll ( path + "/lvl" + ( myStorageContainerClass.level + 2 ).ToString (), typeof ( Texture2D ));
					_myCurrentUpgradeTexture = ( Texture2D ) textureObjects[0];
				
					_containerInfoText.GetComponent < GameTextControl > ().myKey = "ui_sign_factory_storage_metal";
					_containerInfoText.GetComponent < GameTextControl > ().addText = " " + ( myStorageContainerClass.level + 1 + 1 ).ToString () + ")";
					_containerIconMaterial.mainTexture = _myCurrentUpgradeTexture;
					if ( FLStorageContainerClass.LEVELS_STATS_METAL[myStorageContainerClass.level + 1].cost > GameGlobalVariables.Stats.METAL_IN_CONTAINERS)
					{
						dontHaveMats();
					}
					else
					{
						haveMats();
					}
					break;
				case FLStorageContainerClass.STORAGE_TYPE_PLASTIC:
				_progressBarMaterial.mainTexture = FLFactoryRoomManager.getInstance ().powerBarPaneltextures[(int) ((( FLStorageContainerClass.LEVELS_STATS_PLASTIC[myStorageContainerClass.level].capacity * 100 ) / FLStorageContainerClass.LEVELS_STATS_PLASTIC[myStorageContainerClass.level + 1].capacity ) / 5 )];

				_capcityText.GetComponent < GameTextControl > ().addText = " " + FLStorageContainerClass.LEVELS_STATS_PLASTIC[myStorageContainerClass.level].capacity.ToString ();
				_newCapcityText.GetComponent < GameTextControl > ().addText = " " + ( FLStorageContainerClass.LEVELS_STATS_PLASTIC[myStorageContainerClass.level + 1].capacity - FLStorageContainerClass.LEVELS_STATS_PLASTIC[myStorageContainerClass.level].capacity ).ToString ();

				path = "Textures/FactoryRoom/StorageContainers/Plastic";
					textureObjects = Resources.LoadAll ( path + "/lvl" + ( myStorageContainerClass.level + 2 ).ToString (), typeof ( Texture2D ));
					_myCurrentUpgradeTexture = ( Texture2D ) textureObjects[0];
				
					_containerInfoText.GetComponent < GameTextControl > ().myKey = "ui_sign_factory_storage_plastic";
					_containerInfoText.GetComponent < GameTextControl > ().addText = " " + ( myStorageContainerClass.level + 1 + 1 ).ToString () + ")";
					_containerIconMaterial.mainTexture = _myCurrentUpgradeTexture;
					if ( FLStorageContainerClass.LEVELS_STATS_PLASTIC[myStorageContainerClass.level + 1].cost > GameGlobalVariables.Stats.PLASTIC_IN_CONTAINERS)
					{
						dontHaveMats();
					}
					else
					{
						haveMats();
					}
					break;
				case FLStorageContainerClass.STORAGE_TYPE_VINES:
				_progressBarMaterial.mainTexture = FLFactoryRoomManager.getInstance ().powerBarPaneltextures[(int) ((( FLStorageContainerClass.LEVELS_STATS_VINES[myStorageContainerClass.level].capacity * 100 ) / FLStorageContainerClass.LEVELS_STATS_VINES[myStorageContainerClass.level + 1].capacity ) / 5 )];

				_capcityText.GetComponent < GameTextControl > ().addText = " " + FLStorageContainerClass.LEVELS_STATS_VINES[myStorageContainerClass.level].capacity.ToString ();
				_newCapcityText.GetComponent < GameTextControl > ().addText = " " + ( FLStorageContainerClass.LEVELS_STATS_VINES[myStorageContainerClass.level + 1].capacity - FLStorageContainerClass.LEVELS_STATS_VINES[myStorageContainerClass.level].capacity ).ToString ();

				path = "Textures/FactoryRoom/StorageContainers/Vines";
					textureObjects = Resources.LoadAll ( path + "/lvl" + ( myStorageContainerClass.level + 2 ).ToString (), typeof ( Texture2D ));
					_myCurrentUpgradeTexture = ( Texture2D ) textureObjects[0];
				
					_containerInfoText.GetComponent < GameTextControl > ().myKey = "ui_sign_factory_storage_vines";
					_containerInfoText.GetComponent < GameTextControl > ().addText = " " + ( myStorageContainerClass.level + 1 + 1 ).ToString () + ")";
					_containerIconMaterial.mainTexture = _myCurrentUpgradeTexture;
					if ( FLStorageContainerClass.LEVELS_STATS_PLASTIC[myStorageContainerClass.level + 1].cost > GameGlobalVariables.Stats.PLASTIC_IN_CONTAINERS)
					{
						dontHaveMats();
					}
					else
					{
						haveMats();
					}
					break;
			}

			/*if ( FLStorageContainerClass.LEVELS_STATS[myStorageContainerClass.level + 1].cost > GameGlobalVariables.Stats.PREMIUM_CURRENCY )
			{
				_notEnoughResourcesText.gameObject.SetActive ( true );
				_costText.renderer.material = GameGlobalVariables.FontMaterials.RED_TITLE;
				_warningBarObject.renderer.enabled = true;
				_myConfirmButtonBoxCollider.renderer.material.mainTexture = FLFactoryRoomManager.getInstance ().buttonGrayedOutTexture;   // This has been added to the switch above to suit non-premium currency.
			}
			else
			{
				_notEnoughResourcesText.gameObject.SetActive ( false );
				_costText.renderer.material = GameGlobalVariables.FontMaterials.WHITE_TITLE;
				_myConfirmButtonBoxCollider.renderer.material.mainTexture = FLFactoryRoomManager.getInstance ().buttonGreenTexture;
				_warningBarObject.renderer.enabled = false;
			}*/
			//==============================Daves Edit===============================
		}
		else
		{
			_generalText.GetComponent < GameTextControl > ().myKey = "ui_sign_factory_storage_already_maximum_level";
			_generalText.GetComponent < GameTextControl > ().addText = "";
			_timeText.GetComponent < GameTextControl > ().myKey = "ui_sign_factory_storage_already_maximum_level";
			_timeText.GetComponent < GameTextControl > ().addText = "";

			switch ( myStorageContainerClass.type )
			{
				case FLStorageContainerClass.STORAGE_TYPE_METAL:
					_progressBarMaterial.mainTexture = FLFactoryRoomManager.getInstance ().powerBarPaneltextures[(int) ((( FLStorageContainerClass.LEVELS_STATS_METAL[myStorageContainerClass.level].capacity * 100 ) / FLStorageContainerClass.LEVELS_STATS_METAL[myStorageContainerClass.level].capacity ) / 5 )];

					_capcityText.GetComponent < GameTextControl > ().addText = " " + FLStorageContainerClass.LEVELS_STATS_METAL[myStorageContainerClass.level].capacity.ToString ();

					_containerInfoText.GetComponent < GameTextControl > ().myKey = "ui_sign_factory_storage_metal";
					_containerInfoText.GetComponent < GameTextControl > ().addText = " " + ( myStorageContainerClass.level + 1 ).ToString () + ")";
					_containerIconMaterial.mainTexture = myStorageContainerClass.myCurrentLevelTextures[0];
					break;
				case FLStorageContainerClass.STORAGE_TYPE_PLASTIC:
					_progressBarMaterial.mainTexture = FLFactoryRoomManager.getInstance ().powerBarPaneltextures[(int) ((( FLStorageContainerClass.LEVELS_STATS_PLASTIC[myStorageContainerClass.level].capacity * 100 ) / FLStorageContainerClass.LEVELS_STATS_PLASTIC[myStorageContainerClass.level].capacity ) / 5 )];

					_capcityText.GetComponent < GameTextControl > ().addText = " " + FLStorageContainerClass.LEVELS_STATS_PLASTIC[myStorageContainerClass.level].capacity.ToString ();

					_containerInfoText.GetComponent < GameTextControl > ().myKey = "ui_sign_factory_storage_plastic";
					_containerInfoText.GetComponent < GameTextControl > ().addText = " " + ( myStorageContainerClass.level + 1 ).ToString () + ")";
					_containerIconMaterial.mainTexture = myStorageContainerClass.myCurrentLevelTextures[0];
					break;
				case FLStorageContainerClass.STORAGE_TYPE_VINES:
					_progressBarMaterial.mainTexture = FLFactoryRoomManager.getInstance ().powerBarPaneltextures[(int) ((( FLStorageContainerClass.LEVELS_STATS_VINES[myStorageContainerClass.level].capacity * 100 ) / FLStorageContainerClass.LEVELS_STATS_VINES[myStorageContainerClass.level].capacity ) / 5 )];

					_capcityText.GetComponent < GameTextControl > ().addText = " " + FLStorageContainerClass.LEVELS_STATS_VINES[myStorageContainerClass.level].capacity.ToString ();

					_containerInfoText.GetComponent < GameTextControl > ().myKey = "ui_sign_factory_storage_vines";
					_containerInfoText.GetComponent < GameTextControl > ().addText = " " + ( myStorageContainerClass.level + 1 ).ToString () + ")";
					_containerIconMaterial.mainTexture = myStorageContainerClass.myCurrentLevelTextures[0];
					break;
			}
			
			_notEnoughResourcesText.gameObject.SetActive ( false );
			_costText.gameObject.SetActive ( false );
			
			_myConfirmButtonBoxCollider.gameObject.SetActive ( false );

			_warningBarObject.SetActive ( false );
		}
	}
}
