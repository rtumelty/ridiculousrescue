using UnityEngine;
using System.Collections;

public class TRTentacleRocksParticlesControl : MonoBehaviour 
{
	//*************************************************************//
	private Vector3 _initialScale;
	private Vector3 _initialPosition;
	private float _countTimeToMoveBack = 0.15f;
	//*************************************************************//
	void Start () 
	{
		_initialScale = VectorTools.cloneVector3 ( transform.localScale );
		_initialPosition = VectorTools.cloneVector3 ( transform.localPosition ); 
		iTween.ScaleTo ( this.gameObject, iTween.Hash ( "time", 0.3f, "easetype", iTween.EaseType.linear, "scale", _initialScale * 3.2f, "oncompletetarget", this.gameObject,  "oncomplete", "onComplete02" ));
	}

	void Update ()
	{
		transform.Translate ( Vector3.right * Time.deltaTime * 10f + Vector3.back * Time.deltaTime * 3f );
	}

	void onComplete01 () 
	{
		iTween.ScaleTo ( this.gameObject, iTween.Hash ( "time", 0.3f, "easetype", iTween.EaseType.easeOutQuad, "scale", _initialScale  * 3.2f, "oncompletetarget", this.gameObject,  "oncomplete", "onComplete02" ));
	}

	void onComplete02 () 
	{
		transform.localPosition = VectorTools.cloneVector3 ( _initialPosition ); 
		transform.localScale = VectorTools.cloneVector3 ( _initialScale );
		onComplete01 ();
	}
}
