using UnityEngine;
using System.Collections;

public class ZoomAndLevelDrag : MonoBehaviour 
{
	//*************************************************************//
	public static float UI_SIZE_FACTOR;
	public static float UI_POSITION_X_FACTOR;
	public static float UI_POSITION_Z_FACTOR;
	//*************************************************************//	
	public const float MINIMUM_CAMERA_ZOOM = 3f;
	public const float MAXIMUM_CAMERA_ZOOM = 6f;
	public static float MAXIMUM_CAMERA_X = 7.5f;
	public const float MINIMUM_CAMERA_X = 3.5f;
	public const float MAXIMUM_CAMERA_Z = 4f;
	public const float MINIMUM_CAMERA_Z = 4f;
	//*************************************************************//	
	public float momentumDecayRate = 1f;

	public int testInt = 0;
	public int amThrough = 0;
	public int testValue = 6;

	private bool _startZoom = false;
	private bool _startDrag = false;
	public bool panUnlocked = false;
	public bool coraMoving = false;
	public bool allowFollow = false;
	public bool boundryStopFollow = false;
	private bool nearScreenEdge = true;
	private float minMagnitudeForFollow = 0;
	private float _touchesInitialDistance;
	private float _actualCameraSize;
	private float _touchesBetweenDistance;
	public float cameraAccelerationRate = 1f;
	private Vector3 _lastMousePosition;
	private Vector3 myDefaultCamPosition;
	private Vector3 momentumDirection;
	public float momentumHolderX;
	private float momentumHolderZ;
	private float lastSize = 0;
	public TextMesh myPrintOut;
	private float timer = 0.2f;
	private bool hasNotRun = true;
	private float panUnlockTimer = 0.21f;

	private TextMesh info4;
	private TextMesh info5;
	private TextMesh info6;
	private TextMesh info7;

	public GameObject coraL;
	public GameObject coraR;
	//*************************************************************//	
	private static ZoomAndLevelDrag _meInstance;
	public static ZoomAndLevelDrag getInstance ()
	{
		if ( _meInstance == null )
		{
			_meInstance = GameObject.Find ( "_MainObject" ).GetComponent < ZoomAndLevelDrag > ();
		}
		
		return _meInstance;
	}
	//*************************************************************//
	//Camera Centering on ZoomOut.
	void Start ()
	{
		myDefaultCamPosition = Camera.main.transform.position;
//		print (gameObject.name);
		myPrintOut = GameObject.Find ( "info3" ).GetComponent<TextMesh> ();
		info4 = GameObject.Find ( "info4" ).GetComponent<TextMesh> ();
		info5 = GameObject.Find ( "info5" ).GetComponent<TextMesh> ();
		info6 = GameObject.Find ( "info6" ).GetComponent<TextMesh> ();
		info7 = GameObject.Find ( "info7" ).GetComponent<TextMesh> ();

		coraL = GameObject.FindWithTag ("CoraL");
		coraR = GameObject.FindWithTag ("CoraR");

	}
	void Update () 
	{
//		print("Right" + Mathf.Abs (coraR.transform.position.x - (CameraMovement.getInstance ().camBlockerR.transform.position.x)));
//		print("Left " + Mathf.Abs (coraL.transform.position.x - (CameraMovement.getInstance ().camBlockerL.transform.position.x)));
		if(hasNotRun == true)
		{
			//Camera.main.transform.position = new Vector3 ( 7.5f, Camera.main.transform.position.y, Camera.main.transform.position.z );
			hasNotRun = false;
		}
		if(Input.mousePosition != _lastMousePosition)
		{
			timer -= Time.deltaTime;
			if(timer <= 0)
			{
				timer = 0.2f;
				_lastMousePosition = Input.mousePosition;
			}
		}
		//myPrintOut.text = GlobalVariables.READY_FOR_NEXT_TURN.ToString();

		/*if(Camera.main.orthographicSize > lastSize)
		{
			Camera.main.transform.position = Vector3.MoveTowards(Camera.main.transform.position, myDefaultCamPosition, 10 * Time.deltaTime);
		}*/
		lastSize = Camera.main.orthographicSize;
		//Camera Centering on ZoomOut.
		UI_SIZE_FACTOR = Camera.main.orthographicSize / 4.5f;
		//UI_POSITION_X_FACTOR = Camera.main.transform.position.x - 5.5f;
		//UI_POSITION_Z_FACTOR = Camera.main.transform.position.z - 4f;

		if ( GlobalVariables.TUTORIAL_MENU || GlobalVariables.TOY_LAZARUS_SEQUENCE || GlobalVariables.START_SEQUENCE || GlobalVariables.POPUP_UI_SCREEN ) return;


		//if ( Input.touchCount > 1 )
		if(CheckInputType.getInstance().touchType == CheckInputType.DUALTOUCH_TYPE)
		{
			if ( _startDrag ) _startDrag = false;
			
			if ( ! _startZoom )
			{
				GlobalVariables.SCREEN_DRAGGING = _startZoom = true;
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
		}
#if UNITY_EDITOR
		//else if ( Input.GetMouseButton ( 0 ) && ( ! GlobalVariables.DRAGGING_OBJECT ) && ( ! GlobalVariables.CORA_SWIPED ))
		else if ( CheckInputType.getInstance().touchType == CheckInputType.DRAG_TYPE && ( ! GlobalVariables.DRAGGING_OBJECT ) && ( ! GlobalVariables.CORA_SWIPED ))
#else
		//else if (( Input.touchCount == 1 ) && ( ! GlobalVariables.DRAGGING_OBJECT ) && ( ! GlobalVariables.CORA_SWIPED ))
			else if ( CheckInputType.getInstance().touchType == CheckInputType.DRAG_TYPE && ( ! GlobalVariables.DRAGGING_OBJECT ) && ( ! GlobalVariables.CORA_SWIPED ))
#endif
		{
			if ( _startZoom ) _startZoom = false;
#if UNITY_EDITOR
			GameObject gameObjectUnderTouch = ScreenWorldTools.getGameObjectFromScreenEveryLayer ( Input.mousePosition );
#else
			GameObject gameObjectUnderTouch = ScreenWorldTools.getGameObjectFromScreenEveryLayer ( Input.touches[0].position );
#endif			
			if (( gameObjectUnderTouch != null ) && ( gameObjectUnderTouch.tag == GlobalVariables.Tags.UI )) return;
#if UNITY_EDITOR
			if (CheckInputType.getInstance().touchType == CheckInputType.DRAG_TYPE && ( ! GlobalVariables.DRAGGING_OBJECT ) && ( ! GlobalVariables.CORA_SWIPED))
			{
				GlobalVariables.SCREEN_DRAGGING = _startDrag = true;
			}
#else
			if ( Input.touches[0].deltaPosition.magnitude > 0f && CheckInputType.getInstance().touchType == CheckInputType.DRAG_TYPE && ( ! GlobalVariables.DRAGGING_OBJECT ) && ( ! GlobalVariables.CORA_SWIPED)) 
			{
				GlobalVariables.SCREEN_DRAGGING = _startDrag = true;
			}
#endif
			if ( _startDrag  && CameraMovement.getInstance().levelSections > 1)
			{
#if UNITY_EDITOR
				momentumDirection = new Vector3( -( _lastMousePosition - Input.mousePosition ).x / 35f, 0f, -( _lastMousePosition - Input.mousePosition ).y / 35f );
				Camera.main.transform.position += momentumDirection;

				momentumHolderX =/* Mathf.Lerp(momentumHolderX, momentumDirection.x, Time.deltaTime * 20f);/*/ momentumDirection.x;
				momentumHolderZ =/* Mathf.Lerp(momentumHolderZ, momentumDirection.z, Time.deltaTime * 20f);/*/ momentumDirection.z;

				//print ("Y" + momentumHolderZ);

#else
				if(Input.touches[0].phase == TouchPhase.Moved)
				{
					momentumDirection = new Vector3 ( -Input.touches[0].deltaPosition.x / 35f, 0f, -Input.touches[0].deltaPosition.y / 35f );
					Camera.main.transform.position += momentumDirection;
				
					momentumHolderX = /*Mathf.Lerp(momentumHolderX, momentumDirection.x, Time.deltaTime * 20f);/*/ momentumDirection.x;
					momentumHolderZ = /* Mathf.Lerp(momentumHolderZ, momentumDirection.z, Time.deltaTime * 20f);/*/ momentumDirection.z;

				}
#endif
			}
			
			_lastMousePosition = VectorTools.cloneVector3 ( Input.mousePosition );
		}
		else
		{
			if ( _startZoom ) _startZoom = false;
			if ( _startDrag ) _startDrag = false;
			
			GlobalVariables.SCREEN_DRAGGING = false;
		}
		//Clamping Camera boundries
		if(CameraMovement.getInstance().levelSections > 1)
		{
			if ( Camera.main.transform.position.x > CameraMovement.getInstance().camBlockerR.transform.position.x - CameraMovement.getInstance().buffer ) 
			{
//				print ("check 1");
				Camera.main.transform.position = new Vector3 ( CameraMovement.getInstance().camBlockerR.transform.position.x - CameraMovement.getInstance().buffer, Camera.main.transform.position.y, Camera.main.transform.position.z );
			}
			else if ( Camera.main.transform.position.x < CameraMovement.getInstance().camBlockerL.transform.position.x + CameraMovement.getInstance().buffer2 ) 
			{
//				print ("check 2");
				Camera.main.transform.position = new Vector3 ( CameraMovement.getInstance().camBlockerL.transform.position.x + CameraMovement.getInstance().buffer2, Camera.main.transform.position.y, Camera.main.transform.position.z );
			}
			
			if ( Camera.main.transform.position.z > CameraMovement.getInstance().camBlockerT.transform.position.z - CameraMovement.getInstance().buffer3) 
			{
//				print ("check 3");
				Camera.main.transform.position = new Vector3 ( Camera.main.transform.position.x, Camera.main.transform.position.y, CameraMovement.getInstance().camBlockerT.transform.position.z - CameraMovement.getInstance().buffer3 );
			}
			else if ( Camera.main.transform.position.z < CameraMovement.getInstance().camBlockerB.transform.position.z + CameraMovement.getInstance().buffer4) 
			{
//				print ("check 4");
				Camera.main.transform.position = new Vector3 ( Camera.main.transform.position.x, Camera.main.transform.position.y, CameraMovement.getInstance().camBlockerB.transform.position.z + CameraMovement.getInstance().buffer4 );
			}
			//coraMoving = CameraMovement.getInstance ().coraReference.GetComponent<IComponent> ().myCharacterData.coraSlidingTroley;

			if(/*coraMoving == true && */allowFollow == true)
			{
				minMagnitudeForFollow = Mathf.Abs(CameraMovement.getInstance().coraReference.transform.position.x - Camera.main.transform.position.x);
				if(minMagnitudeForFollow > 7.5)
				{
					nearScreenEdge = true;
				}
				if(Mathf.Abs( CameraMovement.getInstance().coraReference.transform.position.x - (CameraMovement.getInstance().camBlockerR.transform.position.x )) < 7f ||
				   Mathf.Abs( CameraMovement.getInstance().coraReference.transform.position.x - (CameraMovement.getInstance().camBlockerL.transform.position.x )) < 7f)
				{
					if(Mathf.Abs( CameraMovement.getInstance().coraReference.transform.position.x - (CameraMovement.getInstance().camBlockerR.transform.position.x )) < 7f)
					{
						//print ("MOVE 1 " + (Mathf.Abs( CameraMovement.getInstance().coraReference.transform.position.x + (CameraMovement.getInstance().camBlockerL.transform.position.x ))));
						Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, new Vector3 (CameraMovement.getInstance().camBlockerR.transform.position.x - CameraMovement.getInstance().buffer, Camera.main.transform.position.y, Camera.main.transform.position.z), cameraAccelerationRate * Time.deltaTime/5);
					}
					else if(Mathf.Abs( CameraMovement.getInstance().coraReference.transform.position.x - (CameraMovement.getInstance().camBlockerL.transform.position.x )) < 7f)
					{
						//print ("MOVE 2 " + (Mathf.Abs( CameraMovement.getInstance().coraReference.transform.position.x + (CameraMovement.getInstance().camBlockerL.transform.position.x ))));
						Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, new Vector3 (CameraMovement.getInstance().camBlockerL.transform.position.x + CameraMovement.getInstance().buffer2, Camera.main.transform.position.y, Camera.main.transform.position.z), cameraAccelerationRate * Time.deltaTime/5);
					}
				}
				else if(nearScreenEdge == true)
				{
					//print ("MOVE 3");
					Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, new Vector3 (CameraMovement.getInstance().coraReference.transform.position.x, Camera.main.transform.position.y, Camera.main.transform.position.z), cameraAccelerationRate * Time.deltaTime/25);
					cameraAccelerationRate += 6f;
				}
			}
			else
			{
				nearScreenEdge = false;
				cameraAccelerationRate = 1f;
			}
		}
		//CheckInputType.getInstance ().sample1.text = ("Allow: " + CheckInputType.allowMomentum.ToString ()); 
		if(CheckInputType.allowMomentum == true)
		{

			if ( Camera.main.transform.position.x < CameraMovement.getInstance().camBlockerR.transform.position.x - CameraMovement.getInstance().buffer &&
			    Camera.main.transform.position.x > CameraMovement.getInstance().camBlockerL.transform.position.x + CameraMovement.getInstance().buffer2 )
			{
				amThrough++;
				CheckInputType.getInstance ().sample1.text = amThrough.ToString();
				/*
#if UNITY_EDITOR
				info4.text = "In Unity Ok";
				if(Input.GetMouseButtonDown(0))
				{
					info6.text = "Past Touch count";
					momentumHolderX = 0;
					
				}
#else
					info5.text = "In Else Ok";
				if(Input.touchCount > 0)
				{
					info6.text = "Past Touch count";
					momentumHolderX = 0;
					
				}
#endif
				*/

				Camera.main.transform.position += new Vector3(momentumHolderX , 0, 0);
				testInt++;
					//CheckInputType.getInstance().sample2.text = "Running";
				
				if(momentumHolderX > -0.005 && momentumHolderX < 0.005f)
				{
					myPrintOut.text = "Set to Zero";
//					print(momentumHolderX);
					momentumHolderX = 0;
					//CheckInputType.allowMomentum = false;
				}

				CheckInputType.getInstance().sample2.text = momentumHolderX.ToString();
				if(momentumHolderX < -0.005f)
				{

					momentumHolderX += Time.deltaTime / 1.2f;
				}

				else if(momentumHolderX > 0.005f)
				{
					//testInt++;
					//myPrintOut.text = testInt.ToString();
					momentumHolderX -= Time.deltaTime / 1.2f;
				}
			}

//			print ("Cam Z : " + (Camera.main.transform.position.z) + "  Blocking Point : " + (CameraMovement.getInstance().camBlockerT.transform.position.z - CameraMovement.getInstance().buffer3));
//			print ("Cam Z : " + (Camera.main.transform.position.z) + "  Blocking Point : " + (CameraMovement.getInstance().camBlockerB.transform.position.z + CameraMovement.getInstance().buffer4));
			if(Camera.main.transform.position.z < CameraMovement.getInstance().camBlockerT.transform.position.z - CameraMovement.getInstance().buffer3 &&
			   Camera.main.transform.position.z > CameraMovement.getInstance().camBlockerB.transform.position.z + CameraMovement.getInstance().buffer4 )
			{

#if UNITY_EDITOR
				if(Input.GetMouseButtonDown(0))
#else
				if(Input.touchCount > 0)
#endif
				{
					momentumHolderZ = 0;
				}
//				print ("Now" + momentumHolderZ);
				Camera.main.transform.position += new Vector3(0, 0, momentumHolderZ);
//				print ("WWWWWWWWWTFFFFFFFFFFFFFFFFFFFFFFFFFF");
				if(momentumHolderZ > -0.05 && momentumHolderZ < 0.05f)
				{
					momentumHolderZ = 0;
					//CheckInputType.allowMomentum = false;
				}
				else if(momentumHolderZ < -0.05f)
				{
					momentumHolderZ += Time.deltaTime / 2;
				}
				else if(momentumHolderZ > 0.05f)
				{
					momentumHolderZ -= Time.deltaTime / 2;
				}
			}
			if(momentumHolderX == 0 && momentumHolderZ == 0)
			{
				CheckInputType.allowMomentum = false;
			}
		}
		else
		{
			//CheckInputType.getInstance().sample2.text = "has stopped";
		}
	}
}
//==================================== END ===========================================
//==================================== END ===========================================
//==================================== END ===========================================
//==================================== END ===========================================
//==================================== END ===========================================

/*if(momentumHolderX != 0)
				{
					if(Mathf.Abs(momentumDirection.x) < 0.1f)
					{
						momentumDirection = new Vector3(Mathf.Sign(momentumDirection.x)/10f, 0, 0);
					}
				}*/
