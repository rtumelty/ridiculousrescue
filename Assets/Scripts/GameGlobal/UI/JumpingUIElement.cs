using UnityEngine;
using System.Collections;

public class JumpingUIElement : MonoBehaviour 
{
	//*************************************************************//	
	public bool typeFromAbove = true;
	//*************************************************************//	
	private Vector3 _initialPosition;
	//*************************************************************//	
	void Start () 
	{
		if ( typeFromAbove ) transform.Rotate ( 0f, 180f, 0f );
		else transform.Rotate ( 0f, 0f, 0f );
		
		_initialPosition = VectorTools.cloneVector3 ( transform.localPosition );
		onCompleteGoingDown ();
	}
	
	private void onCompleteGoingDown ()
	{
		iTween.MoveTo ( this.gameObject, iTween.Hash ( "time", 1.3f, "easetype", iTween.EaseType.easeOutQuad, "position", _initialPosition + Vector3.forward * 0.4f, "islocal", true, "oncomplete", "onCompleteGoingUp" ));
	}
	
	private void onCompleteGoingUp () 
	{
		iTween.MoveTo ( this.gameObject, iTween.Hash ( "time", 1.3f, "easetype", iTween.EaseType.easeOutQuad, "position", _initialPosition, "islocal", true, "oncomplete", "onCompleteGoingDown" ));
	}


	public void onCompleteGoingDownFromDialogBox ()
	{
		iTween.MoveTo ( this.gameObject, iTween.Hash ( "time", 1.3f, "easetype", iTween.EaseType.easeOutQuad, "position", _initialPosition + Vector3.down * 0.4f, "islocal", true, "oncomplete", "onCompleteGoingUpFromDialogBox" ));
	}
	
	public void onCompleteGoingUpFromDialogBox () 
	{
		iTween.MoveTo ( this.gameObject, iTween.Hash ( "time", 1.3f, "easetype", iTween.EaseType.easeOutQuad, "position", _initialPosition, "islocal", true, "oncomplete", "onCompleteGoingDownFromDialogBox" ));
	}
}
