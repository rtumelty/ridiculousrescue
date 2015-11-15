using UnityEngine;
using System.Collections;

public class SimulateTapControl : MonoBehaviour 
{
	//*************************************************************//
	public Vector3 scale = new Vector3 ( 0.31f, 1.47f, 2.08f );
	public float timeFactor = 1f;
	//*************************************************************//
	void Start () 
	{
		iTween.ScaleFrom ( this.gameObject, iTween.Hash ( "time", 0.5f * timeFactor, "easetype", iTween.EaseType.linear, "scale", scale * 1.3f, "oncomplete", "onComplete02" ));
	}

	private void onComplete01 ()
	{
		transform.localScale = VectorTools.cloneVector3 ( scale );
		iTween.ScaleFrom ( this.gameObject, iTween.Hash ( "time", 0.5f * timeFactor, "easetype", iTween.EaseType.linear, "scale", scale * 1.3f, "oncomplete", "onComplete02" ));
	}

	private void onComplete02 ()
	{
		iTween.ScaleTo ( this.gameObject, iTween.Hash ( "time", 1.2f * timeFactor, "easetype", iTween.EaseType.linear, "scale", scale * 1.3f, "oncomplete", "onComplete01" ));
	}

	public void resetScale ()
	{
		transform.localScale = VectorTools.cloneVector3 ( scale );
	}
}
