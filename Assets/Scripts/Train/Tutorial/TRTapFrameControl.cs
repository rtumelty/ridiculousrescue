using UnityEngine;
using System.Collections;

public class TRTapFrameControl : MonoBehaviour 
{
	void OnMouseUp ()
	{
		SoundManager.getInstance ().playSound ( SoundManager.HEADER_TAP );
		Destroy ( transform.parent.gameObject );

		Time.timeScale = 1f;

		Camera.main.transform.Find ( "jumpingArrow" ).gameObject.SetActive ( false );
	}
}
