using UnityEngine;
using System.Collections;

public class CloseOnClick : MonoBehaviour 
{
	void OnMouseDown ()
	{
		UIControl.getInstance ().blockClicksForAMomentAfterUIClicked ();
		GlobalVariables.MENU_FOR_TIP = false;
		iTween.MoveTo ( gameObject, iTween.Hash ( "time", 0.3f, "easetype", iTween.EaseType.easeInExpo, "position", new Vector3 ( 0f, 10f, 2f ), "islocal", true, "oncomplete", "destroyOnComplete" ));
	}
	
	private void destroyOnComplete ()
	{
		Destroy ( gameObject );
	}
}
