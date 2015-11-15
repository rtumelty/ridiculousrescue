using UnityEngine;
using System.Collections;

public class TRTRainPartsAnimationControl : MonoBehaviour 
{
	//*************************************************************//
	void Start () 
	{
		iTween.MoveTo ( this.gameObject, iTween.Hash ( "time", Random.Range ( 0.1f, 0.3f ), "easetype", iTween.EaseType.linear, "position", transform.localPosition + Vector3.right * 0.01f, "oncomplete", "onCompleteTweenAnimationMoveToPositionRight", "islocal", true ));
	}
	
	private void onCompleteTweenAnimationMoveToPositionRight () 
	{
		iTween.MoveTo ( this.gameObject, iTween.Hash ( "time", Random.Range ( 0.1f, 0.3f ), "easetype", iTween.EaseType.linear, "position", transform.localPosition + Vector3.right * 0.01f, "oncomplete", "onCompleteTweenAnimationMoveToPositionLeft", "islocal", true ));
	}

	private void onCompleteTweenAnimationMoveToPositionLeft () 
	{
		iTween.MoveTo ( this.gameObject, iTween.Hash ( "time", Random.Range ( 0.1f, 0.3f ), "easetype", iTween.EaseType.linear, "position", transform.localPosition + Vector3.left * 0.01f, "oncomplete", "onCompleteTweenAnimationMoveToPositionRight", "islocal", true ));
	}
}
