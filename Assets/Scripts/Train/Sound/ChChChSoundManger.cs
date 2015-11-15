using UnityEngine;
using System.Collections;

public class ChChChSoundManger : MonoBehaviour 
{
	//************************************************//
	private float _countTimeToNextCh = 0f;
	public AudioSource mySource;
	public TRSpeedAndTrackOMetersManager myspeedTracker;
	private static ChChChSoundManger _meInstance;
	public static ChChChSoundManger getInstance ()
	{
		if ( _meInstance == null )
		{
			_meInstance = GameObject.Find ( "_CHCHManager" ).GetComponent < ChChChSoundManger > ();
		}
		
		return _meInstance;
	}
	//************************************************//
	void Start () 
	{

		mySource = GetComponent < AudioSource > ();
		mySource.pitch = 0.8f;
		myspeedTracker = TRSpeedAndTrackOMetersManager.getInstance ();
	}
	
	void Update () 
	{
		if ( TRSpeedAndTrackOMetersManager.getInstance ().isEnd ()) return;

		/*_countTimeToNextCh -= Time.deltaTime;

		if ( _countTimeToNextCh <= 0f )
		{
			_countTimeToNextCh = ( 5f / ( Mathf.Round ( TRSpeedAndTrackOMetersManager.getInstance ().getSpeed () + 5f )));
			StartCoroutine ( "playChCh" );
		}*/

		if(TRSpeedAndTrackOMetersManager.getInstance().pitchBlocker == false)
		{
			if(myspeedTracker.getSpeed ()/6f < 0.8f)
			{
				mySource.pitch = 0.8f;
			}
			else
			{
				mySource.pitch = (myspeedTracker.getSpeed ()/6f);
			}
		}
	}

	private IEnumerator playChCh ()
	{
		SoundManager.getInstance ().playSound ( SoundManager.CHCH, -1, true );
		yield return new WaitForSeconds ( 0.2f );
		SoundManager.getInstance ().playSound ( SoundManager.CHCH, -1, true );
	}
}
