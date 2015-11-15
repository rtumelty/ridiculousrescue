using UnityEngine;
using System.Collections;

public class FLMissionScreenDragRotateControl : MonoBehaviour 
{
	//*************************************************************//
	private Vector3 _lastMousePosition;
	private bool _mayRotate = false;
	private bool _blocked = false;
	private bool _rotate = false;
	//*************************************************************//
	private static FLMissionScreenDragRotateControl _meInstance;
	public static FLMissionScreenDragRotateControl getInstance ()
	{
		return _meInstance;
	}
	//*************************************************************//
	void Awake () 
	{
		FLMissionScreenDragRotateControl._meInstance = this;
		transform.Find ( "toy01" ).gameObject.SetActive ( false );
		transform.Find ( "toy02" ).gameObject.SetActive ( false );
		transform.Find ( "toy03" ).gameObject.SetActive ( false );
		transform.Find ( "toy04" ).gameObject.SetActive ( false );
	}
	
	void Update () 
	{
#if UNITY_ANDROID
		if (ChartboostSDK.Chartboost.isImpressionVisible()) return;
#endif

		if(Input.GetMouseButtonUp(0))
		{
			GameGlobalVariables.BACK_FROM_LEVEL = false;
		}

		if ( FLGlobalVariables.MISSION_SCREEN && ! FLGlobalVariables.TUTORIAL_MENU && ! FLGlobalVariables.POPUP_UI_SCREEN )
		{
			
#if UNITY_EDITOR
			if ( Input.GetMouseButtonDown ( 0 ))
			{
				_lastMousePosition = VectorTools.cloneVector3 ( Input.mousePosition );
				_mayRotate = true;
			}

			if ( ! Input.GetMouseButton ( 0 ))
			{
				_rotate = false;
			}

			if ( Input.GetMouseButton ( 0 ) && _mayRotate )
#else
			if ( Input.touchCount == 1 )
#endif
			{
#if UNITY_EDITOR
				if ( ! _blocked && Mathf.Abs (( _lastMousePosition - Input.mousePosition ).x ) > 45f && FLMissionRoomManager.getInstance ().allWorldsObject.transform.rotation.eulerAngles.y > 179f && FLMissionRoomManager.getInstance ().allWorldsObject.transform.rotation.eulerAngles.y < 217f + 3f * 2f )
				{
					_rotate = true;
				}

				if ( _rotate ) FLMissionRoomManager.getInstance ().rotateWorld (( _lastMousePosition - Input.mousePosition ).x / 100f );
#elif UNITY_ANDROID			
				if ( ! _blocked && Mathf.Abs ( Input.touches[0].deltaPosition.x ) > 9f && FLMissionRoomManager.getInstance ().allWorldsObject.transform.rotation.eulerAngles.y > 179f && FLMissionRoomManager.getInstance ().allWorldsObject.transform.rotation.eulerAngles.y < 217f + 3f * 2f )
				{
					_rotate = true;
				}

				if ( _rotate ) FLMissionRoomManager.getInstance ().rotateWorld ( -Input.touches[0].deltaPosition.x / 22f );
#elif UNITY_IPHONE	
				if ( ! _blocked && Mathf.Abs ( Input.touches[0].deltaPosition.x ) > 12f && FLMissionRoomManager.getInstance ().allWorldsObject.transform.rotation.eulerAngles.y > 179f && FLMissionRoomManager.getInstance ().allWorldsObject.transform.rotation.eulerAngles.y < 217f + 3f * 2f )
				{
					_rotate = true;
				}
				
				if ( _rotate )
				{
					FLMissionRoomManager.getInstance ().rotateWorld ( -Input.touches[0].deltaPosition.x / 22f );
				}
#endif
				/*
				if ( FLMissionRoomManager.getInstance ().allWorldsObject.transform.rotation.eulerAngles.y > 187f + 1f * 2f && FLMissionRoomManager.getInstance ().getCurrentWorldID () == 0 )
				{
					if ( ! _blocked ) FLMissionRoomManager.getInstance ().changeWorldInDirection ( new Vector3 ( -100f, 0f, 0f ));
				}
				else if ( FLMissionRoomManager.getInstance ().allWorldsObject.transform.rotation.eulerAngles.y <= 187f + 1f * 2f && FLMissionRoomManager.getInstance ().getCurrentWorldID () == 1 )
				{
					if ( ! _blocked ) FLMissionRoomManager.getInstance ().changeWorldInDirection ( new Vector3 ( 100f, 0f, 0f ));
				}
				else if ( FLMissionRoomManager.getInstance ().allWorldsObject.transform.rotation.eulerAngles.y > 200f + 1f * 3f && FLMissionRoomManager.getInstance ().getCurrentWorldID () == 1 )
				{
					if ( ! _blocked ) FLMissionRoomManager.getInstance ().changeWorldInDirection ( new Vector3 ( -100f, 0f, 0f ));
				}
				else if ( FLMissionRoomManager.getInstance ().allWorldsObject.transform.rotation.eulerAngles.y <= 200f + 1f * 3f && FLMissionRoomManager.getInstance ().getCurrentWorldID () == 2 )
				{
					if ( ! _blocked ) FLMissionRoomManager.getInstance ().changeWorldInDirection ( new Vector3 ( 100f, 0f, 0f ));
				} 
				*/
				
				_lastMousePosition = VectorTools.cloneVector3 ( Input.mousePosition );
			}
			else
			{
				_rotate = false;

				//print (FLMissionRoomManager.getInstance().getCurrentWorldID());
				//FLMissionRoomManager.getInstance ().setWorldRotation ();
				//===================Daves Edit======================
				if(GameGlobalVariables.BACK_FROM_LEVEL == false)
				{
					FLMissionRoomManager.getInstance ().swipeToCurrentWorld ();
					_mayRotate = false;
					FLGlobalVariables.SCREEN_DRAGGING = false;
					FLGlobalVariables.MAP_DRAGGED = false;
				}
				else
				{
					FLGlobalVariables.MAP_DRAGGED = false;
					FLMissionRoomManager.getInstance ().setMapRotation();
				}
			}	
		}
		else
		{
			//print (FLMissionRoomManager.getInstance().getCurrentWorldID());
			//FLMissionRoomManager.getInstance ().setWorldRotation ();
			if(GameGlobalVariables.BACK_FROM_LEVEL == false)
			{
				FLMissionRoomManager.getInstance ().swipeToCurrentWorld ();
				_mayRotate = false;
				FLGlobalVariables.SCREEN_DRAGGING = false;
				FLGlobalVariables.MAP_DRAGGED = false;
			}
			else
			{
				FLGlobalVariables.MAP_DRAGGED = false;
				FLMissionRoomManager.getInstance ().setMapRotation();
			}
			//===================Daves Edit======================
		}
	}
	
	public void blockDraging ()
	{
		_blocked = true;
		StartCoroutine ( "unblockAfterTime" );
	}
	
	private IEnumerator unblockAfterTime ()
	{
		yield return new WaitForSeconds ( 1.0f );
		_blocked = false;
	}
}