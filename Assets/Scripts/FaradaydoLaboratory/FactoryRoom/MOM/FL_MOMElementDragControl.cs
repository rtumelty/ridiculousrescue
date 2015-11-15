using UnityEngine;
using System.Collections;

public class FL_MOMElementDragControl : MonoBehaviour 
{
	//*************************************************************//	
	public int myElementID;
	public FL_MOMClass myMomClass;
	//*************************************************************//	
#if UNITY_EDITOR
	private Vector3 _lastMousePosition;
#endif
	private bool _mouseDownOnMe = false;
	private Transform _myInitialParent;
	private GameObject _myToolTip;
	private Vector3 _initialPosition;
	//*************************************************************//	
	void Awake ()
	{
		_myInitialParent = transform.parent;
		_initialPosition = VectorTools.cloneVector3 ( transform.localPosition );
	}
	
	void Update () 
	{
		if ( _mouseDownOnMe )
		{

#if UNITY_EDITOR
			if ( Input.GetMouseButton ( 0 ))
			{
				if ( _lastMousePosition != Input.mousePosition )
				{
#else
			if ( Input.touchCount == 1 )
			{
				if ( Input.touches[0].phase == TouchPhase.Moved )
				{
#endif
					FLGlobalVariables.DRAGGING_OBJECT = true;
					
					animation.enabled = false;
					animation.Stop ();

					Vector3 hitPosition = ScreenWorldTools.getWorldPointOnMeshFromScreenEveryLayer ( Input.mousePosition );
					if ( hitPosition != Vector3.zero )
					{
						transform.root.position = new Vector3 ( hitPosition.x, 26f, hitPosition.z );		
					}
				}
#if UNITY_EDITOR
				_lastMousePosition = VectorTools.cloneVector3 ( Input.mousePosition );
#endif
			}
			else
			{
				_mouseDownOnMe = false;
				transform.parent = _myInitialParent;
				FLGlobalVariables.DRAGGING_OBJECT = false;
				placeObjectOnGrid ();
				
				Destroy ( _myToolTip );
			}
		}		
	}
			
	private void placeObjectOnGrid ()
	{
		GameObject hitGameObject = ScreenWorldTools.getObjectOnPath ( transform.position, Vector3.down, 90f );
		transform.parent = _myInitialParent;
		if ( hitGameObject == myMomClass.momObject )
		{
			if ( FLGlobalVariables.TUTORIAL_MENU )
			{
				myMomClass.myMomPanelControl.startProduction ( myElementID );
				transform.localPosition = VectorTools.cloneVector3 ( _initialPosition );
				gameObject.SendMessage ( "externallyFinishStep" ); // tutorial step, becouse it is only posibility right now that this element was draged during tutorial
			}
			else
			{
				if ( FLFactoryRoomManager.getInstance ().momsOnLevel[0].myMomPanelControl.transform.Find ( "slideArrow(Clone)" ) != null )
				{
					Destroy ( FLFactoryRoomManager.getInstance ().momsOnLevel[0].myMomPanelControl.transform.Find ( "slideArrow(Clone)" ).gameObject );
					Destroy ( FLFactoryRoomManager.getInstance ().currentMoMRechargeOCoreObject.GetComponent < TutorialDragMoMObjectComponenet > ());
				}

		//=============================================Daves Edit===============================================
				if(myElementID == 64 && (GameGlobalVariables.Stats.RECHARGEOCORES_IN_CONTAINERS + GameGlobalVariables.Stats.RECHARGEOCORES_IN_CONSTRUCTION) < 3)
				{
					FLStorageContainerClass currentFLStorageContainerClass = FLFactoryRoomManager.getInstance ().handleFindStoreForReadyElement ( myElementID );
	
					if ( myMomClass.myMomPanelControl.startProduction ( myElementID ))
					{	
						GameGlobalVariables.Stats.RECHARGEOCORES_IN_CONSTRUCTION ++;
						transform.localPosition = VectorTools.cloneVector3 ( _initialPosition );
					}
					else
					{
						iTween.MoveTo ( gameObject, iTween.Hash ( "time", 0.3f, "easetype", iTween.EaseType.easeOutBounce, "position", _initialPosition, "islocal", true ));
					}
				}
				else if(myElementID == 64 && GameGlobalVariables.Stats.RECHARGEOCORES_IN_CONTAINERS == 3)
				{
					FLStorageContainerClass currentFLStorageContainerClass = FLFactoryRoomManager.getInstance ().handleFindStoreForReadyElement ( myElementID );
				
						iTween.MoveTo ( gameObject, iTween.Hash ( "time", 0.3f, "easetype", iTween.EaseType.easeOutBounce, "position", _initialPosition, "islocal", true ));
						
						SoundManager.getInstance ().playSound ( SoundManager.HEADER_TAP );
						GameObject comboUIInfoScreen = FLUIControl.getInstance ().createPopup ( FLFactoryRoomManager.getInstance ().storageComboUIStorageInfoScreenPrefab );
						if ( comboUIInfoScreen != null )
						{
							comboUIInfoScreen.GetComponent < FLStorageContainerInfoScreenControl > ().myStorageContainerClass = FLFactoryRoomManager.getInstance ().findStorageOfThisElement ( myElementID );
							comboUIInfoScreen.GetComponent < FLStorageContainerInfoScreenControl > ().maximumReachedInfo = true;
						}
				}
				else if (myElementID != 64)
				{
					FLStorageContainerClass currentFLStorageContainerClass = FLFactoryRoomManager.getInstance ().handleFindStoreForReadyElement ( myElementID );
					if ( currentFLStorageContainerClass == null )
					{
						iTween.MoveTo ( gameObject, iTween.Hash ( "time", 0.3f, "easetype", iTween.EaseType.easeOutBounce, "position", _initialPosition, "islocal", true ));
						
						SoundManager.getInstance ().playSound ( SoundManager.HEADER_TAP );
						GameObject comboUIInfoScreen = FLUIControl.getInstance ().createPopup ( FLFactoryRoomManager.getInstance ().storageComboUIStorageInfoScreenPrefab );
						if ( comboUIInfoScreen != null )
						{
							comboUIInfoScreen.GetComponent < FLStorageContainerInfoScreenControl > ().myStorageContainerClass = FLFactoryRoomManager.getInstance ().findStorageOfThisElement ( myElementID );
							comboUIInfoScreen.GetComponent < FLStorageContainerInfoScreenControl > ().maximumReachedInfo = true;
						}
					}
					else
					{
						if ( myMomClass.myMomPanelControl.startProduction ( myElementID ))
						{
							transform.localPosition = VectorTools.cloneVector3 ( _initialPosition );
						}
						else
						{
							iTween.MoveTo ( gameObject, iTween.Hash ( "time", 0.3f, "easetype", iTween.EaseType.easeOutBounce, "position", _initialPosition, "islocal", true ));
						}
					}	
				}
		//=============================================Daves Edit===============================================
				else
				{
					iTween.MoveTo ( gameObject, iTween.Hash ( "time", 0.3f, "easetype", iTween.EaseType.easeOutBounce, "position", _initialPosition, "islocal", true ));
					GameObject comboUIInfoScreen = FLUIControl.getInstance ().createPopup ( FLFactoryRoomManager.getInstance ().storageComboUIStorageInfoScreenPrefab );
					if ( comboUIInfoScreen != null )
					{
						comboUIInfoScreen.GetComponent < FLStorageContainerInfoScreenControl > ().myStorageContainerClass = FLFactoryRoomManager.getInstance ().findStorageOfThisElement ( myElementID );
						comboUIInfoScreen.GetComponent < FLStorageContainerInfoScreenControl > ().maximumReachedInfo = true;
					}
				}
			}			
		}
		else
		{
			iTween.MoveTo ( gameObject, iTween.Hash ( "time", 0.3f, "easetype", iTween.EaseType.easeOutBounce, "position", _initialPosition, "islocal", true ));
		}
	}
			
	private void externallyStartProduction ()
	{
		placeObjectOnGrid ();
	}
			
	void OnMouseDown ()
	{
		if ( FLGlobalVariables.checkForMenus ()) return;
		GetComponent <Animation>().Stop();
				animation.enabled = false;
		handleTouched ();
	}
		
	void OnMouseUp ()
	{
		animation.enabled = true;
		animation.Play ("momItemWobbleAnim");
	}

	private void handleTouched ()
	{
		if ( _myToolTip != null ) return;

#if UNITY_EDITOR
		_lastMousePosition = VectorTools.cloneVector3 ( Input.mousePosition );
#endif
		FLGlobalVariables.DRAGGING_OBJECT = true;		
		
		_mouseDownOnMe = true;
				animation.enabled = false;

		transform.parent = null;

		_myToolTip = ( GameObject ) Instantiate ( FLFactoryRoomManager.getInstance ().momUIToolTipPrefab, transform.position + Vector3.forward * 3f, FLFactoryRoomManager.getInstance ().momUIToolTipPrefab.transform.rotation );
		_myToolTip.GetComponent < FL_MOMToolTipControl > ().myElementID = myElementID;
		_myToolTip.transform.parent = transform;
	}
}