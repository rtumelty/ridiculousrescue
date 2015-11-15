using UnityEngine;
using System.Collections;

public class AlphaApear : MonoBehaviour 
{
	//*************************************************************//	
	public float alpha = 0f;
	public bool destroyOnStart = true;
	public bool customControl = false;
	//*************************************************************//	
	private Material _myMaterial;
	private bool _fadeOut = false;
	//*************************************************************//	
	void Awake () 
	{
		if ( destroyOnStart ) Destroy ( this );
		_myMaterial = renderer.material;
	}
	
	void Update () 
	{
		if ( _fadeOut )
		{
			if ( ! customControl ) alpha = Mathf.Lerp ( alpha, 0f, 0.155f );
			if ( alpha < 0.05f )
			{
				alpha = 0f;
			}
		}
		else
		{
			if ( ! customControl ) alpha = Mathf.Lerp ( alpha, 1f, 0.155f );
			if ( alpha > 0.95f ) alpha = 1f;
		}
		
		_myMaterial.color = new Color ( _myMaterial.color.r, _myMaterial.color.g, _myMaterial.color.b,  alpha );
	}
	
	public void startFadeOut ()
	{
		_fadeOut = true;
	}
	
	public void startFadeIn ()
	{
		_fadeOut = false;
	}
}
