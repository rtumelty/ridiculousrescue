using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class FL_MOMAnimationComponent : MonoBehaviour 
{
	//*************************************************************//
	public const int WORKING_ANIMATION = 0;
	//*************************************************************//
	private Material _myMaterial;
	private List < Texture2D > _texturesWorking;
	private List < Texture2D > _currentAnimationTextures;
	
	private float _countTimeToNextFrame = 0f;
	private int _frameID = 0;
	private int _currentAnimationID = -1;
	//*************************************************************//	
	void Start () 
	{		
		_myMaterial = renderer.material;
		
		string path = "Textures/FactoryRoom/MOM/Working";
		
		_texturesWorking = new List < Texture2D > ();
		
		UnityEngine.Object[] textureMomWorkingObjects = Resources.LoadAll ( path, typeof ( Texture2D ));
		
		foreach ( UnityEngine.Object textureObject in textureMomWorkingObjects )
		{
			if ( textureObject is Texture2D )
			{
				_texturesWorking.Add (( Texture2D ) textureObject );
			}
		}
	}
	
	public void playAnimation ( int animationID )
	{
		_currentAnimationID = animationID;
		_frameID = 0;
		switch ( _currentAnimationID )
		{
			case WORKING_ANIMATION:
				_currentAnimationTextures = _texturesWorking;
				break;
		}
	}
	
	public void stopAnimation ()
	{
		_currentAnimationID = -1;
		_frameID = 0;
	}
	
	public int getCurrentAnimation ()
	{
		return _currentAnimationID;
	}
	
	void Update () 
	{
		_countTimeToNextFrame += Time.deltaTime;
		
		if ( _countTimeToNextFrame >= ( 1f / 24f ))
		{
			_countTimeToNextFrame = 0f;
		
			if ( _currentAnimationID == -1 )
			{

			}
			else
			{
				_myMaterial.mainTexture = _currentAnimationTextures[_frameID];
				_frameID++;
				if ( _frameID >= _currentAnimationTextures.Count )
				{
					_currentAnimationID = -1;
					_frameID = 0;
				}
			}
		}
	}
}
