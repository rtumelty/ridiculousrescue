using UnityEngine;
using System.Collections;

public class FLZoomAndLevelDrag : MonoBehaviour 
{
	//*************************************************************//	
	public static float UI_SIZE_FACTOR;
	public static float UI_POSITION_X_FACTOR;
	public static float UI_POSITION_Z_FACTOR;
	//*************************************************************//	
	public const float MINIMUM_CAMERA_ZOOM = 3f;
	public const float MAXIMUM_CAMERA_ZOOM = 5.69f;
	public const float MAXIMUM_CAMERA_X = 7.5f + 12f * 4f;
	public const float MINIMUM_CAMERA_X = 3.5f;
	public const float MAXIMUM_CAMERA_Z = 6f + 8f;
	public const float MINIMUM_CAMERA_Z = 2f;
	//*************************************************************//	
	private bool _startZoom = false;
	private bool _startDrag = false;
	private float _touchesInitialDistance;
	private float _actualCameraSize;
	private float _touchesBetweenDistance;
	private Vector3 _lastMousePosition;
	//*************************************************************//	
	private static FLZoomAndLevelDrag _meInstance;
	public static FLZoomAndLevelDrag getInstance ()
	{
		if ( _meInstance == null )
		{
			_meInstance = GameObject.Find ( "_MainObject" ).GetComponent < FLZoomAndLevelDrag > ();
		}
		
		return _meInstance;
	}
	//*************************************************************//
	void Update () 
	{
		if (FLUIControl.getInstance ().momPanLock = false) 
		{
		print ("shite");
		
		Camera.main.orthographicSize = 5.5f;
		}
		UI_SIZE_FACTOR = Camera.main.orthographicSize / 4.5f;
		UI_POSITION_X_FACTOR = Camera.main.transform.position.x - 5.5f;
		UI_POSITION_Z_FACTOR = Camera.main.transform.position.z - 4f;
		
		if ( FLGlobalVariables.DRAGGING_OBJECT || FLGlobalVariables.POPUP_UI_SCREEN || FLGlobalVariables.MISSION_SCREEN ) return;
		if ( FLGlobalVariables.TUTORIAL_MENU )
		{
			if ( TutorialsManager.getInstance ().getCurrentTutorialStep () != null && TutorialsManager.getInstance ().getCurrentTutorialStep ().type != TutorialsManager.TUTORIAL_OBJECT_TYPE_SWIPE_TO_ROOM )
			{
				return;
			}
			else
			{
				return;
			}
		}
			
		if ( Input.touchCount > 1 )
		{
			return;
			/*
			if ( _startDrag ) _startDrag = false;
			
			if ( ! _startZoom )
			{
				FLGlobalVariables.SCREEN_DRAGGING = _startZoom = true;
				_touchesInitialDistance = Vector2.Distance ( Input.touches[0].position, Input.touches[1].position );
				_actualCameraSize = Camera.main.orthographicSize;
			}
			else
			{
				_touchesBetweenDistance = Vector2.Distance ( Input.touches[0].position, Input.touches[1].position );
				float cameraAddValue = ( _touchesInitialDistance - _touchesBetweenDistance ) / 70f;
				Camera.main.orthographicSize = _actualCameraSize + cameraAddValue;
				
				if ( Camera.main.orthographicSize > MAXIMUM_CAMERA_ZOOM ) Camera.main.orthographicSize = MAXIMUM_CAMERA_ZOOM;
				else if ( Camera.main.orthographicSize < MINIMUM_CAMERA_ZOOM ) Camera.main.orthographicSize = MINIMUM_CAMERA_ZOOM;
			}
			*/
		}
#if UNITY_EDITOR
		else if ( Input.GetMouseButton ( 0 ))
#else
		else if ( Input.touchCount == 1 )
#endif
		{
			if ( _startZoom ) _startZoom = false;
#if UNITY_EDITOR
			GameObject gameObjectUnderTouch = ScreenWorldTools.getGameObjectFromScreenEveryLayer ( Input.mousePosition );
#else
			GameObject gameObjectUnderTouch = ScreenWorldTools.getGameObjectFromScreenEveryLayer ( Input.touches[0].position );
#endif			
			if (( gameObjectUnderTouch != null ) && ( gameObjectUnderTouch.tag == FLGlobalVariables.Tags.UI )) return;
#if UNITY_EDITOR
			if (( _lastMousePosition - Input.mousePosition ).magnitude > 500f )
			{
				FLGlobalVariables.SCREEN_DRAGGED = FLGlobalVariables.SCREEN_DRAGGING = _startDrag = true;
			}
#else
			if ( Input.touches[0].deltaPosition.magnitude > 18f ) FLGlobalVariables.SCREEN_DRAGGED = FLGlobalVariables.SCREEN_DRAGGING = _startDrag = true;
#endif
			
			if ( _startDrag )
			{
#if UNITY_EDITOR
				FLUIControl.getInstance ().changeRoomInDirection ( _lastMousePosition - Input.mousePosition );
#else
				FLUIControl.getInstance ().changeRoomInDirection ( Input.touches[0].deltaPosition );
#endif
			}
			
			_lastMousePosition = VectorTools.cloneVector3 ( Input.mousePosition );
		}
		else
		{
			if ( _startZoom ) _startZoom = false;
			if ( _startDrag ) _startDrag = false;
			
			FLGlobalVariables.SCREEN_DRAGGING = false;
		}		
	}
}
