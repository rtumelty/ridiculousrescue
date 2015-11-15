using UnityEngine;
using System.Collections;

public class MoveUpAndDown : MonoBehaviour 
{

	public bool goingUp = true;

	private Vector3 _initialPosition;

	void Awake ()
	{
		_initialPosition = VectorTools.cloneVector3 ( transform.localPosition );
	}

	void Start () 
	{
		iTween.MoveTo ( this.gameObject, iTween.Hash ( "time", 2f, "easetype", iTween.EaseType.linear, "position", _initialPosition + ( goingUp ? Vector3.forward * 0.08f : Vector3.back * 0.08f ), "oncomplete", "onComplete01", "isLocal", true ));
	}
	
	void onComplete01 () 
	{
		iTween.MoveTo ( this.gameObject, iTween.Hash ( "time", 2f, "easetype", iTween.EaseType.linear, "position", _initialPosition + ( goingUp ? Vector3.back * 0.08f : Vector3.forward * 0.08f ), "oncomplete", "onComplete02", "isLocal", true ));
	}

	void onComplete02 () 
	{
		iTween.MoveTo ( this.gameObject, iTween.Hash ( "time", 2f, "easetype", iTween.EaseType.linear, "position", _initialPosition + ( goingUp ? Vector3.forward * 0.08f : Vector3.back * 0.08f ), "oncomplete", "onComplete01", "isLocal", true ));
	}
}
