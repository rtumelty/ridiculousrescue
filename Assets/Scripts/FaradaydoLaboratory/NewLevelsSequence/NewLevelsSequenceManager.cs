using UnityEngine;
using System.Collections;

public class NewLevelsSequenceManager : MonoBehaviour 
{
	//*************************************************************//
	private GameObject _sunNova;
	private GameObject _sunRays;
	private GameObject _sign;
	//*************************************************************//
	private static NewLevelsSequenceManager _meInstance;
	public static NewLevelsSequenceManager getInstance ()
	{
		return _meInstance;
	}
	//*************************************************************//
	void Awake () 
	{
		NewLevelsSequenceManager._meInstance = this;
		
		//_sunNova = transform.Find ( "sunNova" ).gameObject;
		//_sunRays = transform.Find ( "sunRays" ).gameObject;
		_sign = transform.Find ( "textInfo" ).gameObject;
		
		//_sunNova.SetActive ( true );
		//_sunRays.SetActive ( true );
		_sign.SetActive ( false );
		
		//_sunRays.GetComponent < AlphaApear > ().customControl = true;
		//_sunNova.GetComponent < AlphaApear > ().customControl = true;
	}
	
	public void updateSeqence ( float worldRotationY ) 
	{
		worldRotationY -= ( 209f + 3f * 2f );
		if ( 3.5f + (( worldRotationY + 10f ) / 6f ) > 3.5f )
		{
			//Camera.main.transform.Find ( "world" ).Find ( "sign" ).transform.localPosition = new Vector3 ( 0f, 3.5f + (( worldRotationY + 10f ) / 6f ), 2.2f );
		}
		if ( ! FLMissionRoomManager.getInstance ().checkIfReachedEndOfWorlds ())
		{
			//_sunRays.transform.Rotate ( new Vector3 ( 0f, worldRotationY / 6f, 0f ));
		}
		//_sunNova.transform.localScale = new Vector3 ( 13f * (( worldRotationY + 10f ) / 10f ), 13f * (( worldRotationY + 10f ) / 10f ), 13f * (( worldRotationY + 10f ) / 10f ) );
		//_sunRays.GetComponent < AlphaApear > ().alpha = ( worldRotationY + 10f ) / 18f;
		//_sunNova.GetComponent < AlphaApear > ().alpha = ( worldRotationY + 10f ) / 18f;
		//_sign.transform.localPosition = new Vector3 ( 36f - ( worldRotationY + 10f ) * 2.6f, 2.8f, -1f );
	}

} 
