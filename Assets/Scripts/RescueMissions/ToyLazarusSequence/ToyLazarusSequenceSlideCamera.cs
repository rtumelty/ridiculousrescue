using UnityEngine;
using System.Collections;

public class ToyLazarusSequenceSlideCamera : MonoBehaviour 
{
	void Start () 
	{
		Vector3 positionToMove = new Vector3 ( -20f, transform.position.y, 3.5f );
		iTween.MoveTo ( this.gameObject, iTween.Hash ( "time", 0.4f, "easetype", iTween.EaseType.easeOutBack, "position", positionToMove, "oncomplete", "onCompleteTweenAnimationMoveToPosition"));
	}
	
	private void onCompleteTweenAnimationMoveToPosition () 
	{
		ToyLazarusSequenceControl.getInstance ().goToNextStep ();
		Destroy ( this );
	}
}
