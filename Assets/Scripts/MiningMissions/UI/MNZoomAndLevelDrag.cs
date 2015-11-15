using UnityEngine;
using System.Collections;

public class MNZoomAndLevelDrag : MonoBehaviour 
{
	//*************************************************************//
	public static float UI_SIZE_FACTOR;
	public static float UI_POSITION_X_FACTOR;
	public static float UI_POSITION_Z_FACTOR;
	//*************************************************************//	
	public const float MINIMUM_CAMERA_ZOOM = 3f;
	public const float MAXIMUM_CAMERA_ZOOM = 6f;
	public const float MAXIMUM_CAMERA_X = 7.5f;
	public const float MINIMUM_CAMERA_X = 3.5f;
	public const float MAXIMUM_CAMERA_Z = 6f;
	public const float MINIMUM_CAMERA_Z = 2f;
	//*************************************************************//	
	private bool _startZoom = false;
	private bool _startDrag = false;
	public bool panUnlocked = false;
	private float _touchesInitialDistance;
	private float _actualCameraSize;
	private float _touchesBetweenDistance;
	private Vector3 _lastMousePosition;
	private Vector3 myDefaultCamPosition;
	private float lastSize = 0;
	//*************************************************************//	
	private static MNZoomAndLevelDrag _meInstance;
	public static MNZoomAndLevelDrag getInstance ()
	{
		if ( _meInstance == null )
		{
			_meInstance = GameObject.Find ( "_MainObject" ).GetComponent < MNZoomAndLevelDrag > ();
		}
		
		return _meInstance;
	}
	//*************************************************************//
	//Camera Centering on ZoomOut.
	void Start ()
	{
		myDefaultCamPosition = Camera.main.transform.position;
	}
	void Update ()
	{
		if(Camera.main.orthographicSize > lastSize)
		{
			Camera.main.transform.position = Vector3.MoveTowards(Camera.main.transform.position, myDefaultCamPosition, 10 * Time.deltaTime);
		}
		lastSize = Camera.main.orthographicSize;
		//Camera Centering on ZoomOut.
		UI_SIZE_FACTOR = Camera.main.orthographicSize / 4.5f;
		UI_POSITION_X_FACTOR = Camera.main.transform.position.x - 5.5f;
		UI_POSITION_Z_FACTOR = Camera.main.transform.position.z - 4f;
		
		if ( MNGlobalVariables.TUTORIAL_MENU || MNGlobalVariables.TOY_LAZARUS_SEQUENCE || MNGlobalVariables.START_SEQUENCE || MNGlobalVariables.POPUP_UI_SCREEN ) return;

		if ( Input.touchCount > 1 )
		{
			if ( _startDrag ) _startDrag = false;
			
			if ( ! _startZoom )
			{
				MNGlobalVariables.SCREEN_DRAGGING = _startZoom = true;
				_touchesInitialDistance = Vector2.Distance ( Input.touches[0].position, Input.touches[1].position );
				_actualCameraSize = Camera.main.orthographicSize;
			}
			else
			{
				_touchesBetweenDistance = Vector2.Distance ( Input.touches[0].position, Input.touches[1].position );
				float cameraAddValue = ( _touchesInitialDistance - _touchesBetweenDistance ) / 70f;
				Camera.main.orthographicSize = _actualCameraSize + cameraAddValue;
				
				if ( Camera.main.orthographicSize > MAXIMUM_CAMERA_ZOOM ) 
				{
					Camera.main.orthographicSize = MAXIMUM_CAMERA_ZOOM;
				}
				else if ( Camera.main.orthographicSize < MINIMUM_CAMERA_ZOOM )
				{
					Camera.main.orthographicSize = MINIMUM_CAMERA_ZOOM;
				}
			}
		}
#if UNITY_EDITOR
		else if ( Input.GetMouseButton ( 0 ) && ( ! MNGlobalVariables.DRAGGING_OBJECT ) && ( ! MNGlobalVariables.CORA_SWIPED ))
#else
		else if (( Input.touchCount == 1 ) && ( ! MNGlobalVariables.DRAGGING_OBJECT ) && ( ! MNGlobalVariables.CORA_SWIPED ))
#endif
		{
			if ( _startZoom ) _startZoom = false;
#if UNITY_EDITOR
			GameObject gameObjectUnderTouch = ScreenWorldTools.getGameObjectFromScreenEveryLayer ( Input.mousePosition );
#else
			GameObject gameObjectUnderTouch = ScreenWorldTools.getGameObjectFromScreenEveryLayer ( Input.touches[0].position );
#endif			
			if (( gameObjectUnderTouch != null ) && ( gameObjectUnderTouch.tag == MNGlobalVariables.Tags.UI )) return;
#if UNITY_EDITOR
			//MNGlobalVariables.SCREEN_DRAGGING = _startDrag = true;
#else
			if ( Input.touches[0].deltaPosition.magnitude > 15f ) MNGlobalVariables.SCREEN_DRAGGING = _startDrag = true;
#endif
			
			if ( _startDrag && panUnlocked == true)
			{
#if UNITY_EDITOR
				Camera.main.transform.position += new Vector3 ( -( _lastMousePosition - Input.mousePosition ).x / 70f, 0f, -( _lastMousePosition - Input.mousePosition ).y / 70f ); 
#else
				Camera.main.transform.position += new Vector3 ( -Input.touches[0].deltaPosition.x / 70f, 0f, -Input.touches[0].deltaPosition.y / 70f ); 
#endif
				if ( Camera.main.transform.position.x > MAXIMUM_CAMERA_X ) 
				{
					Camera.main.transform.position = new Vector3 ( MAXIMUM_CAMERA_X, Camera.main.transform.position.y, Camera.main.transform.position.z );
				}
				else if ( Camera.main.transform.position.x < MINIMUM_CAMERA_X ) 
				{
					Camera.main.transform.position = new Vector3 ( MINIMUM_CAMERA_X, Camera.main.transform.position.y, Camera.main.transform.position.z );
				}
				
				if ( Camera.main.transform.position.z > MAXIMUM_CAMERA_Z ) 
				{
					Camera.main.transform.position = new Vector3 ( Camera.main.transform.position.x, Camera.main.transform.position.y, MAXIMUM_CAMERA_Z );
				}
				else if ( Camera.main.transform.position.z < MINIMUM_CAMERA_Z )
				{
					Camera.main.transform.position = new Vector3 ( Camera.main.transform.position.x, Camera.main.transform.position.y, MINIMUM_CAMERA_Z );
				}
			}
			
			_lastMousePosition = VectorTools.cloneVector3 ( Input.mousePosition );
		}
		else
		{
			if ( _startZoom ) _startZoom = false;
			if ( _startDrag ) _startDrag = false;
			
			MNGlobalVariables.SCREEN_DRAGGING = false;
		}
	}
}
