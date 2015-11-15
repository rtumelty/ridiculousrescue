using UnityEngine;
using System.Collections;

public class ToyLazarusSequenceToBeRescuedSlide : MonoBehaviour 
{
	void Start () 
	{
		Vector3 positionToMove = new Vector3 ( -10.5f, 5f, 2f );
		iTween.MoveTo ( this.gameObject, iTween.Hash ( "time", 1f, "easetype", iTween.EaseType.easeOutSine, "position", positionToMove, "oncomplete", "onCompleteTweenAnimationMoveToPosition"));
	}
	
	private void onCompleteTweenAnimationMoveToPosition () 
	{
		ToyLazarusSequenceControl.getInstance ().goToNextStep ();
		Destroy ( this );
	}
}
