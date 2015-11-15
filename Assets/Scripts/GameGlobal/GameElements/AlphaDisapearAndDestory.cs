using UnityEngine;
using System.Collections;

public class AlphaDisapearAndDestory : MonoBehaviour 
{
	//*************************************************************//
	public float timeToStartFading = 0f;
	//*************************************************************//	
	private Material _myMaterial;
	private float _alpha = 0f;
	//*************************************************************//	
	void Awake () 
	{
		_myMaterial = renderer.material;
		_alpha = _myMaterial.color.a;
	}
	
	void Update () 
	{
		timeToStartFading -= Time.deltaTime;
		if ( timeToStartFading <= 0f )
		{
			if ( _alpha <= 0.05f )
			{
				Destroy ( this.gameObject );
				return;
			}
			
			_alpha = Mathf.Lerp ( _alpha, 0f, 0.075f );
			if ( _alpha < 0f ) _alpha = 0f;
			
			_myMaterial.color = new Color ( _myMaterial.color.r, _myMaterial.color.g, _myMaterial.color.b,  _alpha );
		}
	}
}


