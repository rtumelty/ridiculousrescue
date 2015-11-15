using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class FLUIControl : MonoBehaviour 
{
	//*************************************************************//
	public static GameObject currentReservingUIElmentObject;
	public static GameObject currentPopupUI;
	public static GameObject currentBlackOutUI;
	public static GameObject currentUIElement;
	public static GameObject currentSelectedGameElement;
	public static GameObject currentCharacterTutorialFrame;
	public GameObject _buyLivesPrefab;
	//*************************************************************//	
	public Texture2D textureUIButtonSelectedUp;
	public Texture2D textureUIButtonSelectedDown;
	public Texture2D textureUIButtonNotSelectedUp;
	public Texture2D textureUIButtonNotSelectedDown;
	public Material transparentMaterial;
	public int currentGaragePart;
	//*************************************************************//
	private bool _mayDrag = true;
	public bool momPanLock = false;
	private bool _resetUIClicked = false;
	private GameObject _blackOutUIPrefab;
	public bool fromMom;
	//*************************************************************//
	private static FLUIControl _meInstance;
	public static FLUIControl getInstance ()
	{
		if ( _meInstance == null )
		{
			_meInstance = GameObject.Find ( "Camera" ).transform.Find ( "UI" ).GetComponent < FLUIControl > ();
		}
		
		return _meInstance;
	}
	//*************************************************************//	
	
	void Awake ()
	{
		_blackOutUIPrefab = ( GameObject ) Resources.Load ( "UI/PopupBlackOutBackUI" );
		_buyLivesPrefab = ( GameObject ) Resources.Load ( "UI/Laboratory/getLivesUIPanel" );
	}
	
	void Update () 
	{
		if ( FLGlobalVariables.UI_CLICKED )
		{
			if ( ! _resetUIClicked )
			{
				StartCoroutine ( "unblockAfterDelay" );
			}
		}
	}
	
	public void LateUpdate () 
	{
		if(momPanLock = false)
		{
	//		print ("isThis?");
			float uiSizeFactor = Camera.main.orthographicSize / 4.5f;
			transform.localScale = new Vector3 ( 24f * uiSizeFactor, 1f, 1f * uiSizeFactor );
			transform.localPosition = new Vector3 ( transform.localPosition.x, Camera.main.orthographicSize - 0.5f * uiSizeFactor, transform.localPosition.z );
		}
	}
	
	public GameObject createPopup ( GameObject popupPrefab, bool destroyCurrentPopup = false )
	{
	//	print ("IBIBIUYV");
		if ( destroyCurrentPopup ) Destroy ( FLUIControl.currentPopupUI );
		else if ( FLUIControl.currentPopupUI != null ) return null;
		
		FLGlobalVariables.POPUP_UI_SCREEN = true;
		blockClicksForAMomentAfterUIClicked ();

		GameObject popup = ( GameObject ) Instantiate ( popupPrefab, Camera.main.transform.position + Vector3.down * 4f + Vector3.forward * 12f, popupPrefab.transform.rotation );
		popup.transform.parent = Camera.main.transform;
		popup.AddComponent < PopupDelayer > ();
		FLUIControl.currentPopupUI = popup;
		
		GameObject blackOutScreen = ( GameObject ) Instantiate ( _blackOutUIPrefab, Camera.main.transform.position + Vector3.down * 5.5f, _blackOutUIPrefab.transform.rotation );
		blackOutScreen.transform.parent = Camera.main.transform;
		FLUIControl.currentBlackOutUI = blackOutScreen;
		
		return popup;
	}
	
	public void putHeigherOrLowerCurrencyPanel ( bool isOn )
	{
		Transform currencyPanelTransform = transform.Find ( "PremiumCurrencyStatusPanel" );
		if ( isOn )
		{
			currencyPanelTransform.localPosition = new Vector3 ( currencyPanelTransform.localPosition.x, 10.7f, currencyPanelTransform.localPosition.z );
		}
		else
		{
			currencyPanelTransform.localPosition = new Vector3 ( currencyPanelTransform.localPosition.x, 0f, currencyPanelTransform.localPosition.z );
		}
	}
	
	public void blockClicksForAMomentAfterUIClicked ()
	{
		FLGlobalVariables.UI_CLICKED = true;
		StopCoroutine ( "unblockAfterDelay" );
		StartCoroutine ( "unblockAfterDelay" );
	}
	
	private IEnumerator unblockAfterDelay ()
	{
		_resetUIClicked = true;
		yield return new WaitForSeconds ( 0.25f );
		FLGlobalVariables.UI_CLICKED = false;	
	}
	
	public void destoryCurrentUIElement ()
	{
		if ( FLUIControl.currentUIElement == null ) return;
		if(FLGlobalVariables.CONTAINER_UI_OPEN > 0)
		{
			FLGlobalVariables.CONTAINER_UI_OPEN --;
		}

		FLUIControl.currentUIElement.AddComponent < HideUIElement > ();
		FLUIControl.currentUIElement = null;
		if(fromMom == true)
		{
			Camera.main.transform.Find ("UI").transform.Find ("FactoryRoomButton").SendMessage ("handleTouched",true);
			fromMom = false;
		}


	}
	
	public void unselectCurrentGameElement ()
	{
		if ( currentSelectedGameElement == null ) return;
		currentSelectedGameElement.GetComponent < SelectedComponenent > ().setSelected ( false );
		currentSelectedGameElement = null;
	}
	
	public int currentCameraAboveRoom ()
	{
		if(momPanLock == true)
		{
			return -1;
		}
		if ( Camera.main.transform.position.x < -0.5f + 12f )
		{
			return FLNavigateButton.ROOM_ID_FACTORY;
		}

		else if (( Camera.main.transform.position.x >= -0.5f + 12f ) && ( Camera.main.transform.position.x < 11.5f + 12f ))
		{
			if ( Camera.main.transform.position.z >= -0.5f + 8f ) return FLNavigateButton.ROOM_ID_MISSION;
			else return FLNavigateButton.ROOM_ID_PLAY;
		}
		else return FLNavigateButton.ROOM_ID_GARAGE;
	}
	
	public void changeRoomInDirection ( Vector3 direction )
	{
		if ( ! _mayDrag || momPanLock) return;
		_mayDrag = false;
		//print ("possible?");
		FLUIControl.getInstance ().unselectCurrentGameElement ();
		FLUIControl.getInstance ().destoryCurrentUIElement ();
		
		Vector3 positionToGo = VectorTools.cloneVector3 ( Camera.main.transform.position );
		
		int xDirection = 0;
		int zDirection = 0;
		
		bool mayMove = false;
		
		if ( Mathf.Abs ( direction.x ) > Mathf.Abs ( direction.y ))
		{
			if ( direction.x > 0f )
			{
				xDirection = -1;
				zDirection = 0;
			}
			else
			{
				xDirection = 1;
				zDirection = 0;
			}
		}
		else if ( Mathf.Abs ( direction.x ) < Mathf.Abs ( direction.y ))
		{
			if ( direction.y > 0f )
			{
				zDirection = -1;
				xDirection = 0;
			}
			else
			{
				zDirection = 1;
				xDirection = 0;
			}
		}
		
		switch ( currentCameraAboveRoom ())
		{
			case FLNavigateButton.ROOM_ID_FACTORY:
				if ( xDirection != 0 )
				{
					if ( xDirection > 0 )
					{
						mayMove = true;
						positionToGo = new Vector3 ( -6.5f + 12f + 12f * xDirection, Camera.main.transform.position.y, 4f + 9.25f );
					}
				}
				break;
			case FLNavigateButton.ROOM_ID_MISSION:
				if ( xDirection != 0 )
				{
					mayMove = true;
					positionToGo = new Vector3 ( 5.5f + 12f + 12f * xDirection, Camera.main.transform.position.y, 4f + 9.25f );
				}
				else if ( zDirection != 0 )
				{
					if ( zDirection < 0 )
					{
						mayMove = true;
						positionToGo = new Vector3 ( 5.5f + 12f, Camera.main.transform.position.y, 4f + 8f + 9.25f * zDirection );
					}
				}
				break;
			case FLNavigateButton.ROOM_ID_PLAY:
				if ( zDirection != 0 )
				{
					if ( zDirection > 0 )
					{
						mayMove = true;
						positionToGo = new Vector3 ( 5.5f + 12f, Camera.main.transform.position.y, -4f + 8f + 9.25f * zDirection );
					}
				}
				break;
			case FLNavigateButton.ROOM_ID_GARAGE:
				if ( xDirection != 0 )
				{
					if (( currentGaragePart < 0 ) || ( xDirection < 0 ))
					{
						mayMove = true;
						positionToGo = new Vector3 ( 5.5f + 24f + 12f * xDirection, Camera.main.transform.position.y, 4f + 9.25f );
						if ( xDirection > 0 )
						{
							currentGaragePart++;
						}
						else
						{
							currentGaragePart--;
							if ( currentGaragePart < 0 )
							{
								currentGaragePart = 0;
							}
						}
					}
				}
				break;
		}
		
		if ( mayMove ) iTween.MoveTo ( Camera.main.gameObject, iTween.Hash ( "time", 0.4f, "easetype", iTween.EaseType.easeInOutBack, "position", positionToGo ));
		
		StartCoroutine ( "unlockDraggingAfterAWhile" );
	}
	
	private IEnumerator unlockDraggingAfterAWhile ()
	{
		yield return new WaitForSeconds ( 1f );
		_mayDrag = true;
	}

	public void triggerActionOnLab02VistTutorial ()
	{
		FLGlobalVariables.AFTER_LAB_VISIT_02 = true;

		transform.Find ( "MissionRoomButton" ).GetComponent < FLNavigateButton > ().triggerActionOnLab02VistTutorial ();
		transform.Find ( "triggerDialogLabVisit02" ).gameObject.SetActive ( true );

		FLMissionTableControl.getInstance ().GetComponent < SelectedComponenent > ().setSelectedForTutorial ( true, true, true, true, false  );
	}
}
