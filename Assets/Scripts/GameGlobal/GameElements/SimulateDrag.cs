using UnityEngine;
using System.Collections;

public class SimulateDrag : MonoBehaviour 
{
	//*************************************************************//
	public Vector3 targetPosition;
	//*************************************************************//
	private Vector3 _initialPosition;
	//*************************************************************//
	void Start () 
	{
		_initialPosition = VectorTools.cloneVector3 ( transform.localPosition );
		iTween.MoveTo ( this.gameObject, iTween.Hash ( "time", 0.5f, "easetype", iTween.EaseType.linear, "position", targetPosition, "oncomplete", "onComplete01", "isLocal", true ));
	}
	
	void onComplete01 () 
	{
		transform.localPosition = VectorTools.cloneVector3 ( _initialPosition );
		iTween.MoveTo ( this.gameObject, iTween.Hash ( "time", 0.5f, "easetype", iTween.EaseType.linear, "position", targetPosition, "oncomplete", "onComplete01", "isLocal", true ));
	}
}
