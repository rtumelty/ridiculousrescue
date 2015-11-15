using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class TRSpeedAndTrackOMetersManager : MonoBehaviour 
{
	//*************************************************************//
	public Texture2D[] speedOMeterTextures;
	public Texture2D[] trackOMeterTextures;
	public Texture2D[] stoneTextures;
	public Texture2D[] drillButtonAnimation;
	public Texture2D[] brakingSparks;

	public GameObject trainObject;
	public GameObject trainAnimObject;
	public GameObject myNewParent;
	private Transform localToWorld;

	//=============================Daves Edit=============================
	private float sparksNum1 = 0f;
	private float sparksNum2 = 0f;
	public bool isRestricted = false;
	public GameObject frontWheel;
	public GameObject backWheel;
	public float speedDivider = 4.2f;
	public Material sparksMatFront;
	public Material sparksMatBack;
	private float tentacleRestriction = 0;
	private bool escapeTentacle = false;
	private bool snaredTentacle = false;
	private int tickCounter = 0;
	private float tentMin = 2;
	private float tentMax = 4;
	//=============================Daves Edit=============================

	public float substarctFromFullSpeed = 0f;
	//*************************************************************//
	private Material _speedOMeterMaterial;
	private Material _trackOMeterMaterial;
	private GameObject _backObject;
	private GameObject _backDownRocksObject;
	private GameObject _stalactitObject;
	private float _timeElapsed = 0f;
	private float _countSpeedTime = 0f;
	public float _finishedTime = 0;
	private float _countTrack = 0f;
	private float _fullTrackLength = 1825f;
	private List < GameObject > _stonesThatAreStopingMe;
	public bool _reachedEnd = false;
	private bool _failed = false;
	private bool _win = false;
	private bool _failedSequenceStarted = false;
	private bool _stoped = false;
	public bool animStarted = false;
	private Transform finalPosition;
	private Transform train2;
	private GameObject train2obj;
	public bool pitchBlocker = false;
	
	//*************************************************************//	
	private static TRSpeedAndTrackOMetersManager _meInstance;
	public static TRSpeedAndTrackOMetersManager getInstance ()
	{
		return _meInstance;
	}
	//*************************************************************//
	void Awake () 
	{
		TRSpeedAndTrackOMetersManager._meInstance = this;

		_stonesThatAreStopingMe = new List < GameObject > ();

		_speedOMeterMaterial = transform.Find ( "speed" ).renderer.material;
		_trackOMeterMaterial = transform.Find ( "track" ).renderer.material;

		//Resources.Load ("ParticlesControl/brakingSparks");

		trainObject = Camera.main.transform.Find ( "trainHolder" ).gameObject;
		trainAnimObject = Camera.main.transform.Find ( "trainHolder" ).Find ("train").Find ( "trainAnim" ).gameObject;

		_backObject = Camera.main.transform.Find ( "back" ).gameObject;
		_backDownRocksObject = Camera.main.transform.Find ( "downrocks" ).gameObject;
		_stalactitObject = Camera.main.transform.Find ( "stalactit" ).gameObject;
	}
	//=============================Daves Edit=============================
	void Start ()
	{
		train2obj = GameObject.Find ("train2").gameObject;
		train2 = GameObject.Find ("train2").transform;
		myNewParent = GameObject.Find("animCamPoint");
		trainAnimObject.SetActive (false);
		frontWheel = GameObject.Find ("sparkContainer01");
		backWheel = GameObject.Find ("sparkContainer02");
		sparksMatFront = frontWheel.renderer.material;
		sparksMatBack = backWheel.renderer.material;
		frontWheel.SetActive(false);
		backWheel.SetActive (false);
		sparksNum1 = brakingSparks.Length;
		sparksNum2 = sparksNum1 -1;

		//print ("Stars" + SaveDataManager.getValue ( SaveDataManager.TRAIN_LEVEL_STARS_PREFIX + ( TRLevelControl.CURRENT_LEVEL_CLASS.type == FLMissionScreenNodeManager.TYPE_TRAIN_NODE ? "" : "B" ) + TRLevelControl.CURRENT_LEVEL_CLASS.myName ));
	}
	//=============================Daves Edit=============================
	void Update ()
	{
		//==========================Daves Edit=============================
		if(Input.GetKey("space"))
		{
			Time.timeScale = .15f;
		}
		if(Input.GetKey("return"))
		{
			Time.timeScale = 15f;
		}
		if(Input.GetKey("n"))
		{
			Time.timeScale = 1f;
		}
		//==========================Daves Edit=============================

		if (! _reachedEnd) 
		{
			_countSpeedTime += Time.deltaTime;
			_timeElapsed += Time.deltaTime;
		}
		else
		{
			if(TRResoultScreen.getInstance().alreadyResultOpen == true)
			{
				return;
			}
			//if ( trainObject.transform.Find ( "part01" ).position.x >= _backObject.transform.Find ( "madraJump" ).transform.position.x )
			//{
				//TRMadraControl.getInstance ().jumpToCora ();
			//}
	
			if ( trainObject.transform.Find ("train").Find ( "part01" ).position.x >= _backObject.transform.Find("endCollision").transform.Find ( "stonesBegins" ).transform.position.x && GameObject.Find ( "stones1" ) != null )
			{
				Destroy ( GameObject.Find ( "stones1" ).gameObject );
				Destroy ( GameObject.Find ( "stones2" ).gameObject );
				Destroy ( GameObject.Find ( "stones3" ).gameObject );
				Instantiate (( GameObject ) Resources.Load ( "Particles/particlesRock" ), GameObject.Find ( "stones1" ).transform.position, Quaternion.identity );
				Instantiate (( GameObject ) Resources.Load ( "Particles/particlesRock" ), GameObject.Find ( "stones2" ).transform.position, Quaternion.identity );
				Instantiate (( GameObject ) Resources.Load ( "Particles/particlesRock" ), GameObject.Find ( "stones3" ).transform.position, Quaternion.identity );
				StartCoroutine("RocksAudio");
			}

			if ( trainObject.transform.Find ("train").Find ( "part01" ).position.x >= _backObject.transform.Find("endCollision").transform.Find ( "chasm" ).transform.position.x )
			{
				animStarted = true;
				if ( _countSpeedTime < 10f )
				{
					if ( !_failedSequenceStarted ) StartCoroutine ( "startFailedSequence" );
					_failed = true;
				}
				else
				{
					if ( ! _win ) StartCoroutine ( "startWinSequence" );
					_win = true;
				}
			}
		}
		substarctFromFullSpeed = 0f;
		foreach ( GameObject stone in _stonesThatAreStopingMe )
		{
			if ( stone != null )
			{
				substarctFromFullSpeed += 2f;
				isRestricted = true;
			}
		}

		if (TREventsManager.getInstance ().tentacleOnlevel != null && TREventsManager.getInstance ().tentacleOnlevel.GetComponent< TRTentacleControl > ().stopingTrain) 
		{
			if(tickCounter == 0)
			{
				snaredTentacle = true;
				tickCounter++;
			}
			if(snaredTentacle == true)
			{
				tentacleRestriction += Mathf.Lerp(0, tentMax, Mathf.Sin(1 * Time.deltaTime * Mathf.PI * 0.5f));

				if(tentacleRestriction >= tentMax)
				{
					tentacleRestriction = tentMax;
					escapeTentacle = true;
				}
			}
			if(escapeTentacle == true)
			{
				snaredTentacle = false;
				tentacleRestriction -= Mathf.Lerp(tentMax, tentMin, 1.0f - Mathf.Cos(1 * Time.deltaTime * Mathf.PI * 0.5f));
				if(tentacleRestriction <= tentMin)
				{
					tentacleRestriction = tentMin;
				}
			}

		}
		else
		{
			tentacleRestriction = 0;
			tickCounter = 0;
			snaredTentacle = false;
			escapeTentacle = false;
		}
		//=============================Daves Edit=============================

		if ((TREventsManager.getInstance ().tentacleOnlevel != null && TREventsManager.getInstance ().tentacleOnlevel.GetComponent< TRTentacleControl > ().stopingTrain && !animStarted|| isRestricted == true) && !animStarted) 
		{
			frontWheel.SetActive(true);
			backWheel.SetActive (true);
			sparksNum1 += 1 * Time.deltaTime * 20;
			sparksNum2 += 1 * Time.deltaTime * 22;
			if(sparksNum1 > brakingSparks.Length)
			{
				sparksNum1 = 0;
			}
			if(sparksNum2 > brakingSparks.Length)
			{
				sparksNum2 = 0;
			}
			updateBrakes(sparksNum1,sparksNum2);
	
		}
		else
		{
			frontWheel.SetActive(false);
			backWheel.SetActive (false);
			isRestricted = false;
		}
		//=============================Daves Edit=============================


		if ( _countSpeedTime > 10f - substarctFromFullSpeed - tentacleRestriction) _countSpeedTime = 10f - substarctFromFullSpeed - tentacleRestriction;

		if ( _countSpeedTime <= 1f )
		{
			_countSpeedTime = 1f;
			if ( ! _stoped )
			{
				_stoped = true;
				TRMadraControl.getInstance ().stop ();
			}
		}
		else
		{
			if ( _stoped )
			{
				_stoped = false;
				TRMadraControl.getInstance ().startRuning ();
			}
		}

		_countTrack += _countSpeedTime * Time.deltaTime * 3f;
		if ( _countTrack >= _fullTrackLength )
		{
			_reachedEnd = true;
		}

		updateTrack ( _countTrack );
		updateSpeed ( _countSpeedTime );

		if ( ! _failed && ! _win )
		{
			//print ("this");
			_backObject.transform.Translate ( Vector3.right * _countSpeedTime * Time.deltaTime );
			_backDownRocksObject.transform.Translate ( Vector3.right * _countSpeedTime * Time.deltaTime * 1.5f);
			_stalactitObject.transform.Translate ( Vector3.right * _countSpeedTime * Time.deltaTime * 0.6f); 
		}
		else
		{
			if(Camera.main != null)
			{
				//Camera.main.transform.position = new Vector3 ( trainAnimObject.transform.Find ( "train" ).position.x, Camera.main.transform.position.y, Camera.main.transform.position.z );
				if(_win)
				{
					//Camera.main.transform.position = new Vector3 (myNewParent.transform.position.x, Camera.main.transform.position.y, Camera.main.transform.position.z);
					
					//Camera.main.transform.position = Vector3.Lerp (Camera.main.transform.position, new Vector3 (myNewParent.transform.position.x, Camera.main.transform.position.y, Camera.main.transform.position.z),1);
				}
				else if(_failed)
				{
					//Camera.main.transform.position = new Vector3 (trainAnimObject.transform.Find ( "train2" ).transform.position.x, Camera.main.transform.position.y, Camera.main.transform.position.z);
				}
			}
			else
			{
			}
		}

		if ( ! _reachedEnd )
		{
			if ( _backObject.transform.localPosition.x <= 0.6594868f )
			{
				_backObject.transform.localPosition = new Vector3 ( 24f, _backObject.transform.localPosition.y, _backObject.transform.localPosition.z );
			}

			if ( _backDownRocksObject.transform.localPosition.x <= 0.6594868f )
			{
				_backDownRocksObject.transform.localPosition = new Vector3 ( 24f, _backDownRocksObject.transform.localPosition.y, _backDownRocksObject.transform.localPosition.z );
			}

			if ( _stalactitObject.transform.localPosition.x <= 0.6594868f )
			{
				_stalactitObject.transform.localPosition = new Vector3 ( 24f, _stalactitObject.transform.localPosition.y, _stalactitObject.transform.localPosition.z );
			}
		}
	}
	public IEnumerator RocksAudio()
	{
		SoundManager.getInstance ().playSound (SoundManager.BOZ_WIN,-1,true);
		yield return new WaitForSeconds (0.02f);
		SoundManager.getInstance ().playSound (SoundManager.BOZ_WIN,-1,true);
		yield return new WaitForSeconds (0.03f);
		SoundManager.getInstance ().playSound (SoundManager.BOZ_WIN,-1,true);
	}

	private void playAnim ( bool win )
	{
		Transform newLevelHolder = GameObject.Find ("CamQuickPeekPos").transform;
		newLevelHolder.transform.parent = null;
		Camera.main.transform.Find ("back").transform.parent = newLevelHolder.transform;
		GameObject.Find ("downrocks").transform.parent = newLevelHolder.transform;
		GameObject.Find ("MusicSource").transform.parent = newLevelHolder.transform;
		GameObject.Find ("stalactit").transform.parent = null;
		Time.timeScale = 0.15f;
		ChChChSoundManger.getInstance ().mySource.pitch = 0.5f;
//		print ("done");
		GameObject.Find ("train").SetActive (false);
		train2obj.SetActive (true);
		if ( ! win )
		{
			TREndAnimController.getInstance().StartCoroutine("FailAnims");
			GameObject.Find("train2").GetComponent < Animation >().Play("TrainFail");
			TRCameraMovement.getInstance ().releaseCamFail = true;
		}
		else
		{
			GameObject.Find ("train2").SetActive (true);
			GameObject.Find("train2").GetComponent < Animation >().Play("TrainSuccess");
			TREndAnimController.getInstance().StartCoroutine("WinAnims");
			TRCameraMovement.getInstance ().releaseCamNow = true;
		}

	}

	private IEnumerator startFailedSequence ()
	{
		_failedSequenceStarted = true;
		trainAnimObject.SetActive (true);
		playAnim ( false );

	//	Camera.main.transform.Find ( "back" ).transform.parent = null;
	//	Camera.main.transform.Find ( "downrocks" ).transform.parent = null;
	//	Camera.main.transform.Find ( "stalactit" ).transform.parent = null;
	//	Camera.main.transform.Find ( "trainHolder" ).transform.parent = null;

		trainObject.transform.Find ( "train" ).Find ( "part01" ).gameObject.SetActive ( false );
		trainObject.transform.Find ( "train" ).Find ( "part02" ).gameObject.SetActive ( false );
		trainObject.transform.Find ( "train" ).Find ( "part03" ).gameObject.SetActive ( false );
		trainObject.transform.Find ( "train" ).Find ( "part04" ).gameObject.SetActive ( false );
		trainObject.transform.Find ( "train" ).Find ( "Madra" ).gameObject.SetActive ( false );
		yield return new WaitForSeconds ( 1.1f );
		TRResoultScreen.getInstance ().startResoultScreen ( false );
	}

	private IEnumerator startWinSequence ()
	{
//		print ("started");
		_win = true;
		_finishedTime = _timeElapsed;
		trainAnimObject.SetActive (true);
		playAnim ( true );

//		Camera.main.transform.Find ( "back" ).transform.parent = null;
	//	Camera.main.transform.Find ( "downrocks" ).transform.parent = null;
	//	Camera.main.transform.Find ( "stalactit" ).transform.parent = null;
	//	Camera.main.transform.Find ( "trainHolder" ).transform.parent = null;

		trainObject.transform.Find ( "train" ).Find ( "part01" ).gameObject.SetActive ( false );
		trainObject.transform.Find ( "train" ).Find ( "part02" ).gameObject.SetActive ( false );
		trainObject.transform.Find ( "train" ).Find ( "part03" ).gameObject.SetActive ( false );
		trainObject.transform.Find ( "train" ).Find ( "part04" ).gameObject.SetActive ( false );
		trainObject.transform.Find ( "train" ).Find ( "Madra" ).gameObject.SetActive ( false );
//		print ("here?");
		yield return new WaitForSeconds ( 2.5f );
		TRResoultScreen.getInstance ().startResoultScreen ( true );
	}


	public void updateBrakes(float sparksNum1,float sparksNum2)
	{
		sparksMatFront.mainTexture = brakingSparks[(int)sparksNum1];
		sparksMatBack.mainTexture = brakingSparks[(int)sparksNum2];
	}

	public void updateSpeed ( float speed )
	{
		_speedOMeterMaterial.mainTexture = speedOMeterTextures[(int) speed];
		trainObject.transform.Find ("train").gameObject.transform.localPosition = new Vector3 (-3.2f + (speed / (speedDivider / _countSpeedTime * 6)), trainObject.transform.Find ("train").gameObject.transform.localPosition.y, trainObject.transform.Find ("train").gameObject.transform.localPosition.z);
		//localToWorld.transform.position = trainObject.transform.TransformPoint (trainObject.transform.localPosition);
	}

	public void updateTrack ( float track )
	{
		if ( track > _fullTrackLength ) track = _fullTrackLength;
		_trackOMeterMaterial.mainTexture = trackOMeterTextures[(int) ((( track * 100f ) / _fullTrackLength ) / 5f )];
	}

	public float getTrackDistance ()
	{
		return _countTrack;
	}

	public float getSpeed ()
	{
		return _countSpeedTime;
	}

	public void slowDown ( GameObject stone )
	{
		if ( _stonesThatAreStopingMe.Contains ( stone )) return;
		_stonesThatAreStopingMe.Add ( stone );
	}

	public Vector3 getPositionOfTrainPart01 ()
	{
		return trainObject.transform.Find ( "train" ).Find ( "part01" ).position;
	}

	public bool isEnd ()
	{
		return ( _failed || _win );
	}
}
