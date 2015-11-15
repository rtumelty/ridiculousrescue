using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class SingleImageButtonEffect : MonoBehaviour {



	private List < Transform > _myChildren;
	private bool _iAmTouched = false;

	public bool livesButton = false;
	//*************************************************************//
	void Awake ()
	{
		_myChildren = new List < Transform > ();
		/*_myChildren.Add (transform);
		foreach ( Transform child in transform )
		{
			_myChildren.Add ( child );
		}*/
	}
	
	void OnMouseDown ()
	{
		_iAmTouched = true;
		SoundManager.getInstance ().playSound (SoundManager.CONFIRM_BUTTON);
		/*foreach ( Transform child in _myChildren )
		{
			child.localPosition += Vector3.forward * 0.066f;
		}*/
		if(livesButton)
		{
			transform.localPosition -= Vector3.forward * 0.033f;
		}
		else
		{
			transform.localPosition += Vector3.forward * 0.066f;
		}
	}
	
	void OnMouseUp ()
	{
		_iAmTouched = false;
		/*foreach ( Transform child in _myChildren )
		{
			child.localPosition += Vector3.back * 0.066f;
		}*/
		if(livesButton)
		{
			transform.localPosition += Vector3.forward * 0.033f;
		}
		else
		{
			transform.localPosition -= Vector3.forward * 0.066f;
		}

	}
}
