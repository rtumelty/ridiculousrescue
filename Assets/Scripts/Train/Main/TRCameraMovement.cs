using UnityEngine;
using System.Collections;

public class TRCameraMovement : MonoBehaviour 
{
//*************************************************************//	
private Transform _target;
private float _countTimeToFollow;
private GameObject staticCam;
private bool camDropped = false;
private float staticCamsX = 0;
public bool releaseCamNow = false;
public bool releaseCamFail = false;
private Transform sneekPeek;
private Transform sneekPeek2;
private Transform sneekPeek3;
private Transform train2;
public bool peek = false;
public bool goBack = false;
public bool follow = false;
public bool goBack2 = false;
//*************************************************************//	
private static TRCameraMovement _meInstance;
public static TRCameraMovement getInstance ()
{
	if ( _meInstance == null )
	{	
		if(Camera.main != null)
		{
				_meInstance = Camera.main.GetComponent < TRCameraMovement > ();
		}
		else
		{
				_meInstance = GameObject.Find("Camera").GetComponent < TRCameraMovement > ();
		}
	}
	
	return _meInstance;
}

void Start()
{
	train2 = GameObject.Find ("train2").transform;
	sneekPeek = GameObject.Find ("CamQuickPeekPos").transform;
	sneekPeek2 = GameObject.Find ("CamQuickPeekPos2").transform;
	sneekPeek3 = GameObject.Find ("CamQuickPeekPos3").transform;
	staticCam = GameObject.Find ("camHolderStatic");
	staticCamsX = staticCam.transform.position.x;
	staticCam.GetComponent <Camera> ().enabled = false;
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

public void resetCam ()
{
	staticCam.GetComponent <Camera> ().enabled = false;
	GameObject.Find ("Camera").GetComponent <Camera> ().enabled = true;
}

public IEnumerator takeAPeek ()
{
	yield return new WaitForSeconds (0f);
		peek = true;
		SoundManager.getInstance ().playSound (SoundManager.TRAIN_HORN);
	yield return new WaitForSeconds (0.2f);
		peek = false;
	yield return new WaitForSeconds (0.05f);
		goBack = true;
	yield return new WaitForSeconds (0.2f);
		goBack = false;
		follow = true;
	yield return new WaitForSeconds (0.3f);
		InvokeRepeating ("rampUpSpeed",0,0.16f);
}
	public IEnumerator failAPeek ()
	{
		yield return new WaitForSeconds (0f);
		peek = true;
		SoundManager.getInstance ().playSound (SoundManager.TRAIN_HORN);
		yield return new WaitForSeconds (0.2f);
		peek = false;
		yield return new WaitForSeconds (0.05f);
		goBack2 = true;
		yield return new WaitForSeconds (0.2f);
		goBack2 = false;
	}

public void rampUpSpeed()
	{
		if(Time.timeScale < 0.5f)
		{
			Time.timeScale += 0.1f;
		}
		if(ChChChSoundManger.getInstance ().mySource.pitch < 1.66f)
		{
			ChChChSoundManger.getInstance ().mySource.pitch += 0.1f;
		}
	}
//================================Daves Edit==================================
void Update () 
{
	if(releaseCamNow == true)
	{
		StartCoroutine("takeAPeek");
		GameObject.Find("CamQuickPeekPos").transform.parent = null;
		releaseCamNow = false;
	}
	if(releaseCamFail == true)
	{
		StartCoroutine("failAPeek");
		GameObject.Find("CamQuickPeekPos").transform.parent = null;
		releaseCamFail = false;
	}
	if(peek)
	{

		transform.position = Vector3.MoveTowards(transform.position,sneekPeek.position, Time.deltaTime * 100);
	}
	else if(goBack)
	{
	
		transform.position = Vector3.MoveTowards(transform.position,sneekPeek2.position, Time.deltaTime * 100);
	}
	else if(goBack2)
	{

		transform.position = Vector3.MoveTowards(transform.position,sneekPeek3.position, Time.deltaTime * 100);
	}
	else if(follow)
	{

		transform.position = Vector3.MoveTowards(transform.position,sneekPeek.position, Time.deltaTime * 20);
	}
	
}
}