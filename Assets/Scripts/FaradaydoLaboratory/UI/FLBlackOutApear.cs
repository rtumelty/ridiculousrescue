using UnityEngine;
using System.Collections;

public class FLBlackOutApear : MonoBehaviour 
{
	//*************************************************************//	
	private Material _myMaterial;
	private float _alpha = 0f;
	//*************************************************************//	
	void Awake () 
	{
		_myMaterial = renderer.material;
	}
	
	void Update () 
	{
		if ( _alpha >= 0.6f )
		{
			Destroy ( this );
			return;
		}
		
		_alpha = Mathf.Lerp ( _alpha, 0.6f, 0.035f );
		if ( _alpha > 1f ) _alpha = 0.6f;
		
		_myMaterial.color = new Color ( _myMaterial.color.r, _myMaterial.color.g, _myMaterial.color.b,  _alpha );
	}
}
