using UnityEngine;
using System.Collections;

public class FLStorageContainerControl : MonoBehaviour 
{
	//*************************************************************//
	public FLStorageContainerClass myStorageContainerClass;
	//*************************************************************//
	private GameObject _myComboUIShort;
	private GameObject _myComboUIPanel;
	private GameObject _mySelectedContainer;
	private Material _myMaterial;
	private bool _checkIfMapDragged = false;
	private float _countUpdate = 0f;
	//*************************************************************//
	
	void Awake ()
	{
		_myMaterial = renderer.material;
	}
	
	void Start () 
	{
		//==============================Daves Edit===============================
		/*if ( ! FLStorageContainerClass.isMachineStand ( myStorageContainerClass.type ))
		{
			_myComboUIPanel = ( GameObject ) Instantiate ( FLFactoryRoomManager.getInstance ().storageComboUIPanelPrefab new GameObject(), new Vector3 ( transform.position.x + 12.3f, transform.position.y + 10f, transform.position.z + 4f + 20f), FLFactoryRoomManager.getInstance ().storageComboUIPanelPrefab.transform.rotation);
			_myComboUIPanel.GetComponent < FLStorageContainerPanelControl > ().myStorageContainerClass = myStorageContainerClass;
			_myComboUIPanel.transform.parent = Camera.main.transform.Find ( "UI" );
			_myComboUIPanel.transform.localPosition = new Vector3 ( _myComboUIPanel.transform.localPosition.x, 0.8625877f, 3.07f );
		}
		
		if ( myStorageContainerClass.type == FLStorageContainerClass.STORAGE_TYPE_METAL )
		{
			Transform factoryContainersPanelTrasnform = FLUIControl.getInstance ().transform.Find ( "factoryContainersPanel" );
			factoryContainersPanelTrasnform.parent= _myComboUIPanel.transform;
			factoryContainersPanelTrasnform.localPosition = new Vector3 ( -1.072327f, -0.02071434f, 0.07807392f );
		}*/
		//==============================Daves Edit===============================
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
		//==============================Daves Edit===============================

		FLUIControl.getInstance ().destoryCurrentUIElement ();

		if (FLGlobalVariables.CONTAINER_UI_OPEN == 0) 
		{
			GameGlobalVariables.lastStorageContainerClass = myStorageContainerClass;
			FLGlobalVariables.CONTAINER_UI_OPEN ++;
			Invoke ("handleTouched", 0.4F);
		}
		//==============================Daves Edit===============================
	}

	void OnMouseDown ()
	{
		_checkIfMapDragged = true;
	}

	void Update ()
	{
		_countUpdate -= Time.deltaTime;
		
		if ( _countUpdate > 0f ) return;
		
		_countUpdate = 1f;

		if ( myStorageContainerClass.upgrading )
		{
			int startTimeForThisStorage = SaveDataManager.getValue ( SaveDataManager.STORAGE_CONTAINER_LEVEL_UP_TIME_PREFIX + myStorageContainerClass.position[0].ToString () + "," + myStorageContainerClass.position[1].ToString ());
			int deltaTime = (int) ( System.DateTime.UtcNow.Ticks / 10000000 ) - startTimeForThisStorage;
			myStorageContainerClass.upgradeTime = myStorageContainerClass.timeTotal - (float) deltaTime;
			
			if ( myStorageContainerClass.timeTotal <= deltaTime )
			{
				myStorageContainerClass.upgrading = false;
				myStorageContainerClass.level++;
				
				string path = "";
				switch ( myStorageContainerClass.type )
				{
					case FLStorageContainerClass.STORAGE_TYPE_METAL:
						path = "Textures/FactoryRoom/StorageContainers/Metal";
						break;
					case FLStorageContainerClass.STORAGE_TYPE_PLASTIC:
						path = "Textures/FactoryRoom/StorageContainers/Plastic";
						break;
					case FLStorageContainerClass.STORAGE_TYPE_VINES:
						path = "Textures/FactoryRoom/StorageContainers/Vines";
						break;
				}
				
				myStorageContainerClass.myCurrentLevelTextures = new Texture2D[5];
				UnityEngine.Object[] textureObjects = Resources.LoadAll ( path + "/lvl" + ( myStorageContainerClass.level + 1 ).ToString (), typeof ( Texture2D ));
				for ( int i = 0; i <  textureObjects.Length; i++ )
				{
					if ( textureObjects[i] is Texture2D )
					{
						myStorageContainerClass.myCurrentLevelTextures[i] = ( Texture2D ) textureObjects[i];
					}
				}
				
				Resources.UnloadUnusedAssets ();
				
				SaveDataManager.save ( SaveDataManager.STORAGE_CONTAINER_LEVEL_PREFIX + myStorageContainerClass.position[0].ToString () + "," + myStorageContainerClass.position[1].ToString (), myStorageContainerClass.level );
				SaveDataManager.save ( SaveDataManager.STORAGE_CONTAINER_LEVEL_UP_TIME_PREFIX + myStorageContainerClass.position[0].ToString () + "," + myStorageContainerClass.position[1].ToString (), 0 );
			}
		}

		int fullnessLevel = 0;

		/*
		switch ( myStorageContainerClass.type )
		{
			case FLStorageContainerClass.STORAGE_TYPE_METAL:
				ResourcesManager.CURRENT_MAX_METAL = FLStorageContainerClass.LEVELS_STATS_METAL[myStorageContainerClass.level].capacity;
				if ( myStorageContainerClass.amount > FLStorageContainerClass.LEVELS_STATS_METAL[myStorageContainerClass.level].capacity )
				{
					myStorageContainerClass.amount = FLStorageContainerClass.LEVELS_STATS_METAL[myStorageContainerClass.level].capacity;
				}

				fullnessLevel = (int) ((( myStorageContainerClass.amount * 100 ) / FLStorageContainerClass.LEVELS_STATS_METAL[myStorageContainerClass.level].capacity ) / 25 );
				if (( fullnessLevel == 0 ) && ( myStorageContainerClass.amount > 0 )) fullnessLevel = 1;
				_myMaterial.mainTexture = myStorageContainerClass.myCurrentLevelTextures[fullnessLevel];
				break;
			case FLStorageContainerClass.STORAGE_TYPE_PLASTIC:
				ResourcesManager.CURRENT_MAX_PLASTIC = FLStorageContainerClass.LEVELS_STATS_PLASTIC[myStorageContainerClass.level].capacity;
				if ( myStorageContainerClass.amount > FLStorageContainerClass.LEVELS_STATS_PLASTIC[myStorageContainerClass.level].capacity )
				{
					myStorageContainerClass.amount = FLStorageContainerClass.LEVELS_STATS_PLASTIC[myStorageContainerClass.level].capacity;
				}

				fullnessLevel = (int) ((( myStorageContainerClass.amount * 100 ) / FLStorageContainerClass.LEVELS_STATS_PLASTIC[myStorageContainerClass.level].capacity ) / 25 );
				if (( fullnessLevel == 0 ) && ( myStorageContainerClass.amount > 0 )) fullnessLevel = 1;
			_myMaterial.mainTexture = myStorageContainerClass.myCurrentLevelTextures[(int) ((( myStorageContainerClass.amount * 100 ) / FLStorageContainerClass.LEVELS_STATS_PLASTIC[myStorageContainerClass.level].capacity ) / 25 )];
				break;
			case FLStorageContainerClass.STORAGE_TYPE_VINES:
				ResourcesManager.CURRENT_MAX_VINES = FLStorageContainerClass.LEVELS_STATS_VINES[myStorageContainerClass.level].capacity;
				if ( myStorageContainerClass.amount > FLStorageContainerClass.LEVELS_STATS_VINES[myStorageContainerClass.level].capacity )
				{
					myStorageContainerClass.amount = FLStorageContainerClass.LEVELS_STATS_VINES[myStorageContainerClass.level].capacity;
				}

				fullnessLevel = (int) ((( myStorageContainerClass.amount * 100 ) / FLStorageContainerClass.LEVELS_STATS_VINES[myStorageContainerClass.level].capacity ) / 25 );
				if (( fullnessLevel == 0 ) && ( myStorageContainerClass.amount > 0 )) fullnessLevel = 1;
			_myMaterial.mainTexture = myStorageContainerClass.myCurrentLevelTextures[(int) ((( myStorageContainerClass.amount * 100 ) / FLStorageContainerClass.LEVELS_STATS_VINES[myStorageContainerClass.level].capacity ) / 25 )];
				break;
			case FLStorageContainerClass.STORAGE_TYPE_RECHARGEOCORE:
				fullnessLevel = (int) ((( myStorageContainerClass.amount * 100 ) / FLStorageContainerClass.MACHINE_STAND_STATS[myStorageContainerClass.type].capacity ) / 30 );
				if (( fullnessLevel == 0 ) && ( myStorageContainerClass.amount > 0 )) fullnessLevel = 1;
				_myMaterial.mainTexture = FLFactoryRoomManager.getInstance ().rechargeOCoreContainerTexture[fullnessLevel];
				break;
			case FLStorageContainerClass.STORAGE_TYPE_REDIRECTOR:
				fullnessLevel = (int) ((( myStorageContainerClass.amount * 100 ) / FLStorageContainerClass.MACHINE_STAND_STATS[myStorageContainerClass.type].capacity ) / 30 );
				if (( fullnessLevel == 0 ) && ( myStorageContainerClass.amount > 0 )) fullnessLevel = 1;
				_myMaterial.mainTexture = FLFactoryRoomManager.getInstance ().redirectorContainerTexture[fullnessLevel];
				break;
		}
		*/
	}

	private void handleTouched ()
	{
		FLMain.getInstance ().unselectCurrentCharacter ();
		FLUIControl.getInstance ().unselectCurrentGameElement ();
		gameObject.GetComponent < SelectedComponenent > ().setSelected ( true, true, true, true );
		FLUIControl.currentSelectedGameElement = this.gameObject;
		
		if ( _myComboUIShort != null )
		{
			if ( _myComboUIShort.GetComponent < HideUIElement > () != null )
			{
				Destroy ( _myComboUIShort );
			}
			else return;
		}
		//==============================Daves Edit===============================
		//FLUIControl.getInstance ().destoryCurrentUIElement (); Moved this above to OnMouseUp ()
		//==============================Daves Edit===============================

		if ( ! FLStorageContainerClass.isMachineStand ( myStorageContainerClass.type ))
		{
			switch ( myStorageContainerClass.type )
			{
				case FLStorageContainerClass.STORAGE_TYPE_METAL:
				//======================Daves Edit====================================
					_myComboUIShort = ( GameObject ) Instantiate ( FLFactoryRoomManager.getInstance ().storageComboUIShortMetalPrefab, transform.position + Vector3.up * 10f + Vector3.back * 1.25f, FLFactoryRoomManager.getInstance ().storageComboUIShortMetalPrefab.transform.rotation );
					_myComboUIShort.transform.Find ( "StorageContainerInfoText01" ).GetComponent < GameTextControl > ().myKey = "ui_sign_factory_storage_metal";
					_myComboUIShort.transform.Find ( "StorageContainerInfoText01" ).GetComponent < GameTextControl > ().addText = "(" + GameTextManager.getInstance ().getText ( "ui_sign_level" ) + " " + ( myStorageContainerClass.level + 1 ).ToString () + ")";
					_mySelectedContainer = GameObject.Find( "textMetal" );
					if(myStorageContainerClass.level < FLStorageContainerClass.MAXIMUM_LEVEL)
					{
						_mySelectedContainer.GetComponent < TextMesh >().text = FLStorageContainerClass.LEVELS_STATS_METAL[myStorageContainerClass.level + 1].cost.ToString ();
					}
					else
					{
						_mySelectedContainer.GetComponent < TextMesh >().text = "";
					}
				break;

				case FLStorageContainerClass.STORAGE_TYPE_PLASTIC:
				//======================Daves Edit====================================
					_myComboUIShort = ( GameObject ) Instantiate ( FLFactoryRoomManager.getInstance ().storageComboUIShortPlasticPrefab, transform.position + Vector3.up * 10f + Vector3.back * 1.25f, FLFactoryRoomManager.getInstance ().storageComboUIShortPlasticPrefab.transform.rotation );
					_myComboUIShort.transform.Find ( "StorageContainerInfoText01" ).GetComponent < GameTextControl > ().myKey = "ui_sign_factory_storage_plastic";
					_myComboUIShort.transform.Find ( "StorageContainerInfoText01" ).GetComponent < GameTextControl > ().addText = "(" + GameTextManager.getInstance ().getText ( "ui_sign_level" ) + " " + ( myStorageContainerClass.level + 1 ).ToString () + ")";
					_mySelectedContainer = GameObject.Find( "textPlastic" );
					if(myStorageContainerClass.level < FLStorageContainerClass.MAXIMUM_LEVEL)
					{
						_mySelectedContainer.GetComponent < TextMesh >().text = FLStorageContainerClass.LEVELS_STATS_PLASTIC[myStorageContainerClass.level + 1].cost.ToString ();
					}
				else
				{
					_mySelectedContainer.GetComponent < TextMesh >().text = "";
				}
				break;

				case FLStorageContainerClass.STORAGE_TYPE_VINES:
				//======================Daves Edit====================================
					_myComboUIShort = ( GameObject ) Instantiate ( FLFactoryRoomManager.getInstance ().storageComboUIShortPlasticPrefab, transform.position + Vector3.up * 10f + Vector3.back * 1.25f, FLFactoryRoomManager.getInstance ().storageComboUIShortVinesPrefab.transform.rotation );
					_myComboUIShort.transform.Find ( "StorageContainerInfoText01" ).GetComponent < GameTextControl > ().myKey = "ui_sign_factory_storage_vines";
					_myComboUIShort.transform.Find ( "StorageContainerInfoText01" ).GetComponent < GameTextControl > ().addText = "(" + GameTextManager.getInstance ().getText ( "ui_sign_level" ) + " " + ( myStorageContainerClass.level + 1 ).ToString () + ")";
					_mySelectedContainer = GameObject.Find( "textPlastic" );
					if(myStorageContainerClass.level < FLStorageContainerClass.MAXIMUM_LEVEL)
					{
						print (myStorageContainerClass.level);
						_mySelectedContainer.GetComponent < TextMesh >().text = FLStorageContainerClass.LEVELS_STATS_VINES[myStorageContainerClass.level + 1].cost.ToString ();
					}
					else
					{
						_mySelectedContainer.GetComponent < TextMesh >().text = "";
					}
				break;

				default:
					_myComboUIShort = ( GameObject ) Instantiate ( FLFactoryRoomManager.getInstance ().storageComboUIStorageInfoScreenPrefab, transform.position + Vector3.up * 10f + Vector3.back * 1.25f, FLFactoryRoomManager.getInstance ().storageComboUIStorageInfoScreenPrefab.transform.rotation );
					break;
			}
		}
		else
		{
			//======================Daves Edit====================================
			_myComboUIShort = ( GameObject ) Instantiate ( FLFactoryRoomManager.getInstance ().storageComboUIShortPrefab, transform.position + Vector3.up * 10f + Vector3.back * 1.25f, FLFactoryRoomManager.getInstance ().storageComboUIShortPrefab.transform.rotation );
			_myComboUIShort.transform.Find ( "infoButton" ).transform.position += Vector3.left * 0.71f; 
			switch ( myStorageContainerClass.type )
			{
				case FLStorageContainerClass.STORAGE_TYPE_RECHARGEOCORE:
					_myComboUIShort.transform.Find ( "StorageContainerInfoText01" ).GetComponent < GameTextControl > ().myKey = "ui_sign_factory_storage_rechargeocores";
					break;
				case FLStorageContainerClass.STORAGE_TYPE_REDIRECTOR:
					_myComboUIShort.transform.Find ( "StorageContainerInfoText01" ).GetComponent < GameTextControl > ().myKey = "ui_sign_factory_storage_redirectors";
					break;
			}
		}
		
		_myComboUIShort.transform.Find ( "infoButton" ).GetComponent < FLStorageContainerInfoButton > ().myStorageContainerClass = myStorageContainerClass;
		
		if ( !FLStorageContainerClass.isMachineStand ( myStorageContainerClass.type ))
		{
			_myComboUIShort.transform.Find ( "upgradeButton" ).GetComponent < FLStorageContainerUpgradeButton > ().myStorageContainerClass = myStorageContainerClass;
		}
		else Destroy ( _myComboUIShort.transform.Find ( "upgradeButton" ).gameObject );
		
		FLUIControl.currentUIElement = _myComboUIShort;		
	}
}
