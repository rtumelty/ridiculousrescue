using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour 
{
	//*************************************************************//	
	private Transform _target;
	private Transform _targetToFollowOnXAxis;
	private float _countTimeToFollow;
	private GameObject staticCam;
	private bool camDropped = false;
	private float staticCamsX = 0;
	private GameObject defaultTarget;
	private int myLowInt = 1;
	private int myHighInt = 10;
	public GameObject camBlockerR;
	public GameObject camBlockerL;
	public GameObject camBlockerT;
	public GameObject camBlockerB;
	public GameObject coraReference;

	private string myString;
	private float myGap = 0;
	private int startPoint = 0;
	public float buffer = 9.5f;
	public float buffer2 = 9f;
	public float buffer3 = 4f;
	public float buffer4 = 3f;
	public int levelSections = 0;

	//*************************************************************//	
	private static CameraMovement _meInstance;
	public static CameraMovement getInstance ()
	{
		if ( _meInstance == null )
		{	
			if(Camera.main != null)
			{
				_meInstance = Camera.main.GetComponent < CameraMovement > ();
			}
			else
			{
				_meInstance = GameObject.Find("Camera").GetComponent < CameraMovement > ();
			}
		}
		
		return _meInstance;
	}

	void Start()
	{
		camBlockerL = GameObject.FindWithTag ("LevelEndLeft");
		camBlockerT = GameObject.FindWithTag ("LevelEndTop");
		camBlockerB = GameObject.FindWithTag ("LevelEndBot");

		defaultTarget = GameObject.Find ("defaultCamPosition");
		if(Application.loadedLevelName == "TR01")
		{
			staticCam = GameObject.Find ("camHolderStatic");
			if(staticCamsX != null)
			{
				staticCamsX = staticCam.transform.position.x;
				staticCam.GetComponent <Camera> ().enabled = false;
			}
		}
		foreach( GameObject end in GameObject.FindGameObjectsWithTag("LevelEndRight"))
		{
			levelSections += 1;
			if(end.transform.position.x > startPoint)
			{
				camBlockerR = end;
				startPoint = (int) end.transform.position.x;
			}
//			print ("level Section " + levelSections);
		}
		foreach(GameObject character in GameObject.FindGameObjectsWithTag("Character"))
		{
			if(character.GetComponent<IComponent>().myID == GameElements.CHAR_CORA_1_IDLE && levelSections > 1)
			{
				coraReference = character;
				Camera.main.transform.position = new Vector3(character.transform.position.x, transform.position.y, transform.position.z);
			}
		}
	}
	//*************************************************************//
	
	public void followObjectForSeconds ( Transform toFollowObject, float time )
	{
		if ( GlobalVariables.TUTORIAL_MENU ) return;
		_countTimeToFollow = time;
		_target = toFollowObject;
	}
	//================================Daves Edit==================================
	public void dropCam ()
	{
		staticCam.GetComponent <Camera> ().enabled = true;
		GameObject.Find ("Camera").GetComponent <Camera> ().enabled = false;
		camDropped = true;
	}

	public void centreCam (float time)
	{
		if ( GlobalVariables.TUTORIAL_MENU ) return;
		_countTimeToFollow = time;
		_target = defaultTarget.transform;
	}

	public void followOnXAxis ( Transform target )
	{
//		print ( 0 );
		_targetToFollowOnXAxis = target;
	}

	public void centreOnMe ( Transform target )
	{
		print ("EH???");
		foreach( GameObject end in GameObject.FindGameObjectsWithTag("LevelEndRight"))
		{
			if(end.transform.position.x > startPoint)
			{
				print ("Ran");
				camBlockerR = end;
				startPoint = (int) end.transform.position.x;
			}
		}
//		print ("my back end thingy " + camBlocker.name);
		myGap = Mathf.Abs (camBlockerR.transform.position.x - target.position.x);
//		print ("MyGap " + myGap);
		if (target.position.x > camBlockerR.transform.position.x - buffer)
		{
//			print (" first pos ");
			transform.position = new Vector3 (target.position.x - buffer, transform.position.y, transform.position.z);
		}
		else if (target.position.x < camBlockerL.transform.position.x + buffer)
		{
//			print (" second pos ");
			transform.position = new Vector3 (target.position.x, transform.position.y, transform.position.z);
		}
		else
		{
//			print (" third pos ");
			transform.position = new Vector3 (target.position.x, transform.position.y, transform.position.z);
		}
	}

	public void resetCam ()
	{
		staticCam.GetComponent <Camera> ().enabled = false;
		GameObject.Find ("Camera").GetComponent <Camera> ().enabled = true;
	}
	//================================Daves Edit==================================
	void Update () 
	{
		buffer = Mathf.Lerp (4.5f, 9.5f, (Camera.main.orthographicSize - 3) / 3);
		buffer2 = Mathf.Lerp (4.2f, 9.2f, (Camera.main.orthographicSize - 3) / 3);
		buffer3 = Mathf.Lerp (1.5f, 4f, (Camera.main.orthographicSize - 3) / 3);
		buffer4 = Mathf.Lerp (0.5f, 4f, (Camera.main.orthographicSize - 3) / 3);
		/*if ( _targetToFollowOnXAxis != null && !GlobalVariables.SCREEN_DRAGGING && false)
		{
//#if UNITY_EDITOR
			if(_targetToFollowOnXAxis.position.x > camBlockerR.transform.position.x - buffer)
			{
//				print ("well?");
				//_targetToFollowOnXAxis.position = new Vector3(camBlocker.transform.position.x - buffer, _targetToFollowOnXAxis.position.y, _targetToFollowOnXAxis.position.z);
				transform.position = Vector3.Lerp ( transform.position, new Vector3 ( camBlockerR.transform.position.x - buffer, transform.position.y, transform.position.z ), 0.04f );
			}
			else
			{
				transform.position = Vector3.Lerp ( transform.position, new Vector3 ( _targetToFollowOnXAxis.position.x, transform.position.y, transform.position.z ), 0.04f );
			}
			if(_targetToFollowOnXAxis.position.x < camBlockerL.transform.position.x + buffer2)
			{
				//print ("targets pos " + _targetToFollowOnXAxis.position.x + " less than? " + (camBlockerL.transform.position.x + buffer2));
				//_targetToFollowOnXAxis.position = new Vector3(camBlocker.transform.position.x - buffer, _targetToFollowOnXAxis.position.y, _targetToFollowOnXAxis.position.z);
				transform.position = Vector3.Lerp ( transform.position, new Vector3 ( camBlockerL.transform.position.x + buffer, transform.position.y, transform.position.z ), 0.04f );
			}
			else
			{
				transform.position = Vector3.Lerp ( transform.position, new Vector3 ( _targetToFollowOnXAxis.position.x, transform.position.y, transform.position.z ), 0.04f );
			}
//#endif
		}
		//print ("Check before build");
		/*if ( _countTimeToFollow > 0f )
		{
			_countTimeToFollow -= Time.deltaTime;																								//Speed Multiplier?
			transform.position = Vector3.Lerp ( transform.position, new Vector3 ( _target.position.x, transform.position.y, _target.position.z ), 0.04f );
		}*/
		//================================Daves Edit==================================
		if(Application.loadedLevelName == "TR01" )
		{
			if (transform.position.x >= staticCamsX && camDropped == false && staticCamsX != null) 
			{
				dropCam();
			}
		}
		//================================Daves Edit==================================
	}
}
