using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class StaticObjectWithAnimation : MonoBehaviour 
{
	//*************************************************************//
	public Texture2D[] animationTextures;
	public float speedFactor = 1f;
	//*************************************************************//	
	private List < Texture2D > _texturesForAnimation;
	private float _countTimeToNextFrame = 0f;
	private int _frameID = 0;
	private Material _myMaterial;
	private bool _startAnimating = false;
	private bool useIsnpektorTableAnimation = false;
	//*************************************************************//	
	void Awake ()
	{
		_myMaterial = renderer.material;
		if ( animationTextures != null && animationTextures.Length > 0 )
		{
			useIsnpektorTableAnimation = true;
			_startAnimating = true;
		}
	}

	public void uploadAnimation ( string path, bool doNotStartThough = false ) 
	{		
		_texturesForAnimation = new List < Texture2D > ();
		
		UnityEngine.Object[] texturesObjects = Resources.LoadAll ( path, typeof ( Texture2D ));
		
		foreach ( UnityEngine.Object textureObject in texturesObjects )
		{
			if ( textureObject is Texture2D )
			{
				_texturesForAnimation.Add (( Texture2D ) textureObject );
			}
		}
		
		if ( ! doNotStartThough ) _startAnimating = true;
	}
	
	public void startAnimation ()
	{
		_startAnimating = true;
	}
	
	public void stopAnimation ()
	{
		_startAnimating = false;
	}
	
	void Update () 
	{
		if ( ! _startAnimating ) return;
		
		_countTimeToNextFrame += Time.deltaTime;
		
		if ( _countTimeToNextFrame >= ( 1f / ( 24f * speedFactor )))
		{
			_countTimeToNextFrame = 0f;
		
			_myMaterial.mainTexture = ( useIsnpektorTableAnimation ? animationTextures[_frameID] : _texturesForAnimation[_frameID] );
			_frameID++;
			if ( _frameID >= ( useIsnpektorTableAnimation ? animationTextures.Length : _texturesForAnimation.Count )) _frameID = 0;
		}
	}
}
