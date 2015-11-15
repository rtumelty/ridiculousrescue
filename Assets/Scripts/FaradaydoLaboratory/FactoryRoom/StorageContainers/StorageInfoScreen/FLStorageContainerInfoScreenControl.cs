using UnityEngine;
using System.Collections;

public class FLStorageContainerInfoScreenControl : MonoBehaviour 
{
	//*************************************************************//
	public FLStorageContainerClass myStorageContainerClass;
	public bool maximumReachedInfo = false;
	//*************************************************************//
	private TextMesh _generalText;
	private TextMesh _infoText;
	private TextMesh _capcityText;
	
	private Material _progressBarMaterial;
	private Material _containerIconMaterial;
	private Material _iconMaterial;

	private float _countUpdate = 0f;
	//*************************************************************//
	void Awake ()
	{
		_generalText = transform.Find ( "textGeneral" ).GetComponent < TextMesh > ();
		_infoText = transform.Find ( "textInfo" ).GetComponent < TextMesh > ();
		_capcityText = transform.Find ( "textCapacity" ).GetComponent < TextMesh > ();
		
		_progressBarMaterial = transform.Find ( "progressBar" ).renderer.material;
		_containerIconMaterial = transform.Find ( "containerIcon" ).renderer.material;
		_iconMaterial = transform.Find ( "icon" ).renderer.material;
				
		FLGlobalVariables.POPUP_UI_SCREEN = true;
	}
	
	void Update ()
	{
		_countUpdate -= Time.deltaTime;
		
		if ( _countUpdate > 0f ) return;
		
		_countUpdate = 1f;

		if ( ! FLStorageContainerClass.isMachineStand ( myStorageContainerClass.type ))
		{
			switch ( myStorageContainerClass.type )
			{
				case FLStorageContainerClass.STORAGE_TYPE_METAL:
					_generalText.GetComponent < GameTextControl > ().myKey = "ui_sign_factory_storage_metal";
				_capcityText.GetComponent < GameTextControl > ().myKey = "ui_sign_factory_storage_materials";
				_capcityText.GetComponent < GameTextControl > ().addText = " " + myStorageContainerClass.amount.ToString () + "/" + FLStorageContainerClass.LEVELS_STATS_METAL[myStorageContainerClass.level].capacity.ToString ();
					break;
				case FLStorageContainerClass.STORAGE_TYPE_PLASTIC:
					_generalText.GetComponent < GameTextControl > ().myKey = "ui_sign_factory_storage_plastic";
				print ("capacityText updated");
				_capcityText.GetComponent < GameTextControl > ().myKey = "ui_sign_factory_storage_materials";
				_capcityText.GetComponent < GameTextControl > ().addText = " " + myStorageContainerClass.amount.ToString () + "/" + FLStorageContainerClass.LEVELS_STATS_PLASTIC[myStorageContainerClass.level].capacity.ToString ();
					break;
				case FLStorageContainerClass.STORAGE_TYPE_VINES:
					_generalText.GetComponent < GameTextControl > ().myKey = "ui_sign_factory_storage_vines";
				_capcityText.GetComponent < GameTextControl > ().myKey = "ui_sign_factory_storage_materials";
				_capcityText.GetComponent < GameTextControl > ().addText = " " + myStorageContainerClass.amount.ToString () + "/" + FLStorageContainerClass.LEVELS_STATS_VINES[myStorageContainerClass.level].capacity.ToString ();
					break;
			}
			if(myStorageContainerClass.upgrading == false)
			{
				print ("added");
				_generalText.GetComponent < GameTextControl > ().addText = " " + ( myStorageContainerClass.level + 1 ).ToString () + ")";
			}
			else
			{
				_generalText.GetComponent < GameTextControl > ().addText = " " + ( myStorageContainerClass.level + 1 ).ToString () + ")";
			}
		}
		else
		{
			if ( maximumReachedInfo )
			{
				switch ( myStorageContainerClass.type )
				{
					case FLStorageContainerClass.STORAGE_TYPE_RECHARGEOCORE:
						_infoText.GetComponent < GameTextControl > ().myKey = "ui_sign_max_capacity_for_rechargeocores_reached";
						_infoText.GetComponent < GameTextControl > ().lineLength = 35;
						_capcityText.GetComponent < GameTextControl > ().addText = " " + (GameGlobalVariables.Stats.RECHARGEOCORES_IN_CONSTRUCTION + GameGlobalVariables.Stats.RECHARGEOCORES_IN_CONTAINERS).ToString() + "/" + FLStorageContainerClass.MACHINE_STAND_STATS[myStorageContainerClass.type].capacity.ToString ();
						break;
					case FLStorageContainerClass.STORAGE_TYPE_REDIRECTOR:
						_infoText.GetComponent < GameTextControl > ().myKey = "ui_sign_max_capacity_for_redirectors_reached";
						_infoText.GetComponent < GameTextControl > ().lineLength = 35;
						_capcityText.GetComponent < GameTextControl > ().addText = " " + myStorageContainerClass.amount.ToString () + "/" + FLStorageContainerClass.MACHINE_STAND_STATS[myStorageContainerClass.type].capacity.ToString ();
						break;
				}
				
				_generalText.GetComponent < GameTextControl > ().myKey = "ui_sign_max_capacity_reached";

				
			}
			else
			{
				switch ( myStorageContainerClass.type )
				{
					case FLStorageContainerClass.STORAGE_TYPE_RECHARGEOCORE:
						_generalText.GetComponent < GameTextControl > ().myKey = "ui_sign_factory_storage_rechargeocores";
						break;
					case FLStorageContainerClass.STORAGE_TYPE_REDIRECTOR:
						_generalText.GetComponent < GameTextControl > ().myKey = "ui_sign_factory_storage_redirectors";
						break;
				}
				_capcityText.GetComponent < GameTextControl > ().addText = " " + myStorageContainerClass.amount.ToString () + "/" + FLStorageContainerClass.MACHINE_STAND_STATS[myStorageContainerClass.type].capacity.ToString ();
			}
			
			_capcityText.GetComponent < GameTextControl > ().myKey = "ui_sign_factory_storage_capacity";

		}
		
		if ( ! maximumReachedInfo )
		{
			_infoText.GetComponent < GameTextControl > ().myKey = FLStorageContainerClass.INFO_TEXT_KEYS[myStorageContainerClass.type];
			_infoText.GetComponent < GameTextControl > ().lineLength = 45;
		}
		
		_iconMaterial.mainTexture = myStorageContainerClass.iconTexture;
		
		switch ( myStorageContainerClass.type )
		{
			case FLStorageContainerClass.STORAGE_TYPE_METAL:
				_containerIconMaterial.mainTexture = myStorageContainerClass.myCurrentLevelTextures[0];
				break;
			case FLStorageContainerClass.STORAGE_TYPE_PLASTIC:
				_containerIconMaterial.mainTexture = myStorageContainerClass.myCurrentLevelTextures[0];
				break;
			case FLStorageContainerClass.STORAGE_TYPE_VINES:
				_containerIconMaterial.mainTexture = myStorageContainerClass.myCurrentLevelTextures[0];
				break;
			case FLStorageContainerClass.STORAGE_TYPE_RECHARGEOCORE:
				_containerIconMaterial.mainTexture = FLFactoryRoomManager.getInstance ().rechargeOCoreContainerTexture[0];
				break;
			case FLStorageContainerClass.STORAGE_TYPE_REDIRECTOR:
				_containerIconMaterial.mainTexture = FLFactoryRoomManager.getInstance ().redirectorContainerTexture[0];
				break;
		}
		
		if (! FLStorageContainerClass.isMachineStand (myStorageContainerClass.type)) 
		{
			//=============================================Daves Edit============================================
			switch ( myStorageContainerClass.type )
			{
			case FLStorageContainerClass.STORAGE_TYPE_METAL:
				_progressBarMaterial.mainTexture = FLFactoryRoomManager.getInstance ().powerBarPaneltextures [(int)(((myStorageContainerClass.amount * 100) / FLStorageContainerClass.LEVELS_STATS_METAL [myStorageContainerClass.level].capacity) / 5)];
				break;
			case FLStorageContainerClass.STORAGE_TYPE_PLASTIC:
				_progressBarMaterial.mainTexture = FLFactoryRoomManager.getInstance ().powerBarPaneltextures [(int)(((myStorageContainerClass.amount * 100) / FLStorageContainerClass.LEVELS_STATS_PLASTIC [myStorageContainerClass.level].capacity) / 5)];
				break;
			case FLStorageContainerClass.STORAGE_TYPE_VINES:
				_progressBarMaterial.mainTexture = FLFactoryRoomManager.getInstance ().powerBarPaneltextures [(int)(((myStorageContainerClass.amount * 100) / FLStorageContainerClass.LEVELS_STATS_VINES [myStorageContainerClass.level].capacity) / 5)];
				break;
			}

		}
		else if (myStorageContainerClass.type == "Recharg-O-Cores")
		{
			_progressBarMaterial.mainTexture = FLFactoryRoomManager.getInstance ().powerBarPaneltextures[(int) ((((( myStorageContainerClass.amount ) + GameGlobalVariables.Stats.RECHARGEOCORES_IN_CONSTRUCTION )*100 )/ FLStorageContainerClass.MACHINE_STAND_STATS[myStorageContainerClass.type].capacity ) / 5 )];
			//_progressBarMaterial.mainTexture = FLFactoryRoomManager.getInstance ().powerBarPaneltextures[(int) (((myStorageContainerClass.amount * 100) / FLStorageContainerClass.LEVELS_STATS [myStorageContainerClass.level].capacity) / 5)];
		}   //=============================================Daves Edit============================================
		else
		{
			_progressBarMaterial.mainTexture = FLFactoryRoomManager.getInstance ().powerBarPaneltextures[(int) ((( myStorageContainerClass.amount * 100 ) / FLStorageContainerClass.MACHINE_STAND_STATS[myStorageContainerClass.type].capacity ) / 5 )];

		}
	}
}
