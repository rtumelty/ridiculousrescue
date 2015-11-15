using UnityEngine;
using System.Collections;

public class TimerControl : MonoBehaviour 
{
	//*************************************************************//	
	private TextMesh _myText;
	private bool _startCounting = false;
	private float _secondsPassed = 0f;
	private bool _timePassedAlready = false;
	//*************************************************************//	
	private static TimerControl _meInstance;
	public static TimerControl getInstance ()
	{
		return _meInstance;
	}
	//*************************************************************//
	void Awake () 
	{
		_meInstance = this;

		_myText = transform.Find ( "number" ).GetComponent < TextMesh > ();
		_secondsPassed = 0;
		//TimerControl.getInstance ().startTimer ( true );
	}
	
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

	public bool timerBeaten ()
	{
		return !_timePassedAlready;
	}

	public void pause (bool isPaused)
	{
		if(isPaused == true)
		{
			_startCounting = false;
		}
		else
		{
			_startCounting = true;
		}
	}
	
	void Update () 
	{
		if ( ! _startCounting ) return;
		if ( LevelControl.CURRENT_LEVEL_CLASS == null ) return;

		_secondsPassed += Time.deltaTime;
		//print (_secondsPassed);
		_myText.text = TimeScaleManager.getTimeString ((int) _secondsPassed );
		
		if (( ! _timePassedAlready ) && ( _secondsPassed > LevelControl.CURRENT_LEVEL_CLASS.timeLimitForStar ))
		{
			_timePassedAlready = true;
		}
	}
}
