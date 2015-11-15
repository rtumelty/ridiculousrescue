using UnityEngine;
using System.Collections;

public class TRJoseControl : MonoBehaviour 
{
	//*************************************************************//
	public const int IDLE_ANIMATION = 0;
	public const int IDLE_SEPCIAL = 1;

	public const string STAND_IDLE_DRIVE_ANIMATION = "drive";
	public const string STAND_IDLE_DRIVE__AND_EAT_ANIMATION = "drive_eat";
	public const string STAND_DEACTIVE_ANIMATION = "deactive";
	public const string ELECTROCUETED_ANIMATION = "Electric";
	//*************************************************************//
	private float _countTimeToNextFrame = 0f;
	private int _frameID = 0;
	private int _animationID;
	private float _countTimeForSpecialIdle = 10f;
	private float _countTimeOfSpecialIdle = 2f;
	private bool _electroucyed = false;
	//*************************************************************//	
	private static TRJoseControl _meInstance;
	public static TRJoseControl getInstance ()
	{
		return _meInstance;
	}
	//*************************************************************//
	void Awake () 
	{
		TRJoseControl._meInstance = this;
		eatOnOff ( false );

		GetComponent < SkeletonAnimation > ().animationName = STAND_IDLE_DRIVE_ANIMATION;
	}
	
	void Update () 
	{
		if ( _electroucyed )
		{
			_electroucyed = true;
			StartCoroutine ( "waitTilBakcTiIdle" );
			return;
		}

		_countTimeForSpecialIdle -= Time.deltaTime;

		if ( _countTimeForSpecialIdle <= 0f )
		{
			eatOnOff ( true );
			_countTimeOfSpecialIdle -= Time.deltaTime;

			if ( _countTimeOfSpecialIdle <= 0f )
			{
				eatOnOff ( false );
				_countTimeOfSpecialIdle = 2f;
				_countTimeForSpecialIdle = 10f;
			}
		}

		_countTimeToNextFrame += Time.deltaTime;
		if ( _countTimeToNextFrame >= ( 1f / 24f ))
		{
			_countTimeToNextFrame = 0f;
			
			switch ( _animationID )
			{
				case IDLE_ANIMATION:
					GetComponent < SkeletonAnimation > ().animationName = STAND_IDLE_DRIVE_ANIMATION;
					break;
				case IDLE_SEPCIAL:
					GetComponent < SkeletonAnimation > ().animationName = STAND_IDLE_DRIVE__AND_EAT_ANIMATION;
					break;
			}
		}
	}

	private IEnumerator waitTilBakcTiIdle ()
	{
		yield return new WaitForSeconds ( 1f );
		GetComponent < SkeletonAnimation > ().animationName = STAND_IDLE_DRIVE_ANIMATION;
	}


	public void playElectrocuted ()
	{
		_electroucyed = true;
		StartCoroutine ( "waitBeofreIdle" );
	}
	
	private IEnumerator waitBeofreIdle ()
	{
		transform.renderer.enabled = false;
		transform.Find ( "front" ).gameObject.SetActive ( true );
		transform.Find ( "front" ).GetComponent < SkeletonAnimation > ().animationName = ELECTROCUETED_ANIMATION;
		yield return new WaitForSeconds ( 1f );
		transform.renderer.enabled = true;
		transform.Find ( "front" ).gameObject.SetActive ( false );
	}
	
	public void eatOnOff ( bool isOn )
	{
		if ( isOn )
		{
			_animationID = IDLE_SEPCIAL;
		}
		else
		{
			_animationID = IDLE_ANIMATION;
		}
	}
}
