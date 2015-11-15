using UnityEngine;
using System.Collections;

public class TRTimerControl : MonoBehaviour 
{
	//*************************************************************//	
	private TextMesh _myText;
	private bool _startCounting = false;
	private float _secondsPassed = 0f;
	private bool _timePassedAlready = false;
	//*************************************************************//	
	private static TRTimerControl _meInstance;
	public static TRTimerControl getInstance ()
	{
		if ( _meInstance == null )
		{
			_meInstance = GameObject.Find ( "Camera" ).transform.Find ( "UI" ).transform.Find ( "TimerPanel" ).GetComponent < TRTimerControl > ();
		}
		
		return _meInstance;
	}
	//*************************************************************//
	void Awake () 
	{
		_myText = transform.Find ( "text" ).GetComponent < TextMesh > ();
	}
	//======================================Daves Edit======================================
	public void Start()
	{
		startTimer (true);
	}
	//======================================Daves Edit======================================
	public void startTimer ( bool isOn )
	{
		if ( GlobalVariables.TUTORIAL_MENU ) return;
		_startCounting = isOn;
		iTween.ScaleFrom ( gameObject, iTween.Hash ( "time", 0.5f, "easetype", iTween.EaseType.easeOutQuad, "scale", transform.localScale * 1.2f ));
	}
	
	public int getCurrentTime ()
	{
		return (int) _secondsPassed;
	}
	
	void Update () 
	{
		if ( ! _startCounting ) return;
		//if (TRSpeedAndTrackOMetersManager.getInstance ()._reachedEnd) return;
		if ( TRLevelControl.CURRENT_LEVEL_CLASS == null ) return;
		
		_secondsPassed += Time.deltaTime;
		_myText.text = TimeScaleManager.getTimeString ((int) _secondsPassed );

		if (( ! _timePassedAlready ) && ( _secondsPassed > TRLevelControl.CURRENT_LEVEL_CLASS.star04 ))
		{
			_timePassedAlready = true;
			renderer.material.mainTexture = TRUIControl.getInstance ().textureUIClockTimePassed; 
		}
	}
}
