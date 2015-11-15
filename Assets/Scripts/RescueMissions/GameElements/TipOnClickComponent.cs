using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class TipOnClickComponent : MonoBehaviour 
{
	//*************************************************************//
	public static TipOnClickComponent CURRENT_TIP;
	//*************************************************************//
	public int tipID = -1;
	public Texture2D myTipTexture;
	//*************************************************************//
	private GameObject _jumpingArrowPrefab;
	private GameObject _jumpingArrowInstant;
	private GameObject _screenUIPrefab;
	private GameObject _screenUIInstant;
	//*************************************************************//
	void Awake () 
	{
		collider.enabled = true; 
		_jumpingArrowPrefab = ( GameObject ) Resources.Load ( "UI/jumpingArrow" );
		_screenUIPrefab = ( GameObject ) Resources.Load ( "UI/screenTip" );
	}
	
	void Start ()
	{
		_jumpingArrowInstant = ( GameObject ) Instantiate ( _jumpingArrowPrefab, transform.root.position + Vector3.forward * 1.5f + Vector3.up, Quaternion.identity );
	}
	
	void OnMouseUp ()
	{
		if ( GlobalVariables.checkForMenus ()) return;
		handleTouched ();
	}
	
	void Update ()
	{
		if ( GlobalVariables.TUTORIAL_MENU ) _jumpingArrowInstant.renderer.enabled = false;
		else _jumpingArrowInstant.renderer.enabled = true;
	}
	
	private void handleTouched ()
	{
		if ( GlobalVariables.TUTORIAL_MENU ) return;
		GlobalVariables.MENU_FOR_TIP = true;
		CURRENT_TIP = this;
		
		GetComponent < SelectedComponenent > ().setSelected ( true, true );
		
		_screenUIInstant = ( GameObject ) Instantiate ( _screenUIPrefab, Vector3.zero, Quaternion.identity );
		_screenUIInstant.transform.Rotate ( 90f, -90f, 90f );
		_screenUIInstant.renderer.material.mainTexture = myTipTexture;
		_screenUIInstant.transform.parent = Camera.main.transform;
		_screenUIInstant.transform.localPosition = new Vector3 ( 0f, 10f, 2f );
		_screenUIInstant.AddComponent < CloseOnClick > ();
		iTween.MoveTo ( _screenUIInstant, iTween.Hash ( "time", 0.4f, "easetype", iTween.EaseType.easeOutExpo, "position", new Vector3 ( 0f, 0f, 2f ), "islocal", true ));
	
		switch ( tipID )
		{
			case GameElements.UI_TIP_RESCUER:
			case GameElements.UI_TIP_BUILDER:
				_screenUIInstant.transform.localScale = new Vector3 ( _screenUIInstant.transform.localScale.x, 3.25f, _screenUIInstant.transform.localScale.z );
				break;
			case GameElements.UI_TIP_ATTACKER:
			case GameElements.UI_TIP_DEMOLISHER:
				_screenUIInstant.transform.localScale = new Vector3 ( _screenUIInstant.transform.localScale.x, 2.1f, _screenUIInstant.transform.localScale.z );
				break;
		}
	}
	
	public void deActivate ()
	{
		GlobalVariables.MENU_FOR_TIP = false;
		
		if ( Array.IndexOf ( GameElements.ENEMIES, GetComponent < IComponent > ().myID ) == -1 )
		{
			collider.enabled = false;
		}
		
		Destroy ( _screenUIInstant );
		Destroy ( _jumpingArrowInstant );
		Destroy ( this );
	}
}
