using UnityEngine;
using System.Collections;

public class ErrorMarkControl : MonoBehaviour 
{
	//*************************************************************//	
	public float targetScale = 1.3f;
	public GameObject gridSqureMark;
	//*************************************************************//	
	private float _alpha = 1f;
	private Material _myMaterial;
	//*************************************************************//	
	void Start () 
	{
		_myMaterial = renderer.material;
		iTween.ScaleTo ( this.gameObject, iTween.Hash ( "time", 1f, "easetype", iTween.EaseType.linear, "scale", transform.localScale * targetScale, "oncomplete", "onCompleteTweenAnimationScaleUp"));
	}
	
	private void onCompleteTweenAnimationScaleUp ()
	{
		if ( gridSqureMark ) Destroy ( gridSqureMark );
		Destroy ( this.gameObject );
	}
	
	void Update () 
	{
		_myMaterial.color = new Color ( 1f, 1f, 1f, _alpha = Mathf.Lerp ( _alpha, 0.0f, 0.07f ));
		if ( gridSqureMark ) gridSqureMark.renderer.material.color = new Color ( gridSqureMark.renderer.material.color.r, gridSqureMark.renderer.material.color.g, gridSqureMark.renderer.material.color.b, _myMaterial.color.a / 2f );
	}
}
