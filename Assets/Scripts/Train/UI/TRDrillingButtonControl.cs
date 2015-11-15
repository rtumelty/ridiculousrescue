using UnityEngine;
using System.Collections;

public class TRDrillingButtonControl : MonoBehaviour 
{
	//*************************************************************//
	public Transform followStone;
	//*************************************************************//
	private bool _mouseDownOnMe = false;
	private bool _countTime = true;
	private float _time = 0f;
	private Material _tapMaterial;
	//*************************************************************//
	void Awake ()
	{
		_tapMaterial = transform.parent.renderer.material;
	}
	
	void Update ()
	{
		_time -= Time.deltaTime * 2f;
		
		if ( _time >= 6f )
		{
			_time = 6f;
			_tapMaterial.mainTexture = TRSpeedAndTrackOMetersManager.getInstance ().drillButtonAnimation[(int) _time];
			Handheld.Vibrate ();
			Instantiate (( GameObject ) Resources.Load ( "Particles/particlesRock" ), followStone.transform.position, Quaternion.identity );
			SoundManager.getInstance ().playSound ( SoundManager.BUM, -1, true );
			//Instantiate (( GameObject ) Resources.Load ( "Particles/bum" ), followStone.transform.position, Quaternion.identity );
			Destroy ( followStone.gameObject );

			if ( TREventsManager.getInstance ().getCurrentTutorialID () == TREventsManager.TUTORIAL_ID_TAP_ROCK )
			{
				TREventsManager.getInstance ().cleanTutorialForTapRockButton ();
			}

			Destroy ( this.transform.parent.gameObject );
			return;
		}
		else if ( _time <= 0f ) 
		{
			_time = 0f;
		}
		
		_tapMaterial.mainTexture = TRSpeedAndTrackOMetersManager.getInstance ().drillButtonAnimation[(int) _time];
		
		if ( followStone == null )
		{
			Destroy ( this.transform.parent.gameObject );
			return;
		}
		
		transform.parent.position = new Vector3 ( followStone.position.x, transform.parent.position.y, transform.parent.position.z );
	}
	
	void OnMouseDown ()
	{
		if ( TRGlobalVariables.checkForMenus ()) return;
		handleTouched ();
	}
	
	private void handleTouched ()
	{
		_mouseDownOnMe = true;
		_countTime = true;
		TRBozControl.getInstance ().drillOnOff ( true );
		_time += 1f;
	}

	void OnMouseUp ()
	{
		TRBozControl.getInstance ().drillOnOff ( false );
	}
	
	void OnDestroy ()
	{
		TRBozControl.getInstance ().drillOnOff ( false );
	}
}
