using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class OnMouseDownButtonEffectControlVertical : MonoBehaviour 
{
	//*************************************************************//
	public Texture2D myOnMouseDownTexture;
	public Texture2D myOnMouseUpTexture;
	//*************************************************************//
	private List < Transform > _myChildren;
	private bool _iAmTouched = false;
	//*************************************************************//
	void Awake ()
	{
		myOnMouseUpTexture = ( Texture2D ) renderer.material.mainTexture;
		_myChildren = new List < Transform > ();
		foreach ( Transform child in transform )
		{
			_myChildren.Add ( child );
		}
	}
	
	void OnMouseDown ()
	{
		_iAmTouched = true;
		if ( myOnMouseDownTexture != null ) renderer.material.mainTexture = myOnMouseDownTexture;
		foreach ( Transform child in _myChildren )
		{
			child.localPosition += Vector3.down * 0.066f;
		}
	}
	
	void Update ()
	{
		if ( ! _iAmTouched ) return;
		
		if ( myOnMouseDownTexture != null ) renderer.material.mainTexture = myOnMouseDownTexture;
	}
	
	void OnMouseUp ()
	{
		_iAmTouched = false;
		renderer.material.mainTexture = myOnMouseUpTexture;
		foreach ( Transform child in _myChildren )
		{
			child.localPosition += Vector3.up * 0.066f;
		}
	}
}