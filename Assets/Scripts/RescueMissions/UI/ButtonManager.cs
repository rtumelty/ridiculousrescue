using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class ButtonManager : MonoBehaviour 
{
	//*************************************************************//	
	public UIControl.ButtonCallBack myCallBack;
	public List < GameObject > mySetOfButtons;
	public bool destroyOnClick;
	//*************************************************************//
	//private Vector3 _initialScale;
	private Vector3 _initialPosition;
	//*************************************************************//
	
	void Start ()
	{
		//_initialScale = VectorTools.cloneVector3 ( transform.localScale );
		_initialPosition = VectorTools.cloneVector3 ( transform.localPosition );
	}
	
	void OnMouseUp ()
	{
		handleTouched ();
	}
	
	private void handleTouched ()
	{
		bool destoryOnlyMe = false;
		if ( myCallBack != null )
		{
			if ( ! myCallBack ()) destoryOnlyMe = true;
			UIControl.getInstance ().blockClicksForAMomentAfterUIClicked ();
		}
		
		if ( destroyOnClick )
		{
			if ( mySetOfButtons != null )
			{
				foreach ( GameObject button in mySetOfButtons )
				{
					if ( button != this.gameObject )
					{
						Destroy ( button );
					}
				}
			}
			
			Destroy ( gameObject );
		}
		
		if ( destoryOnlyMe ) Destroy ( gameObject );
	}
	
	void Update ()
	{
		//transform.localScale = new Vector3 ( _initialScale.x * ZoomAndLevelDrag.UI_SIZE_FACTOR, _initialScale.y * ZoomAndLevelDrag.UI_SIZE_FACTOR, _initialScale.z * ZoomAndLevelDrag.UI_SIZE_FACTOR );
		transform.localPosition = new Vector3 ( _initialPosition.x * ZoomAndLevelDrag.UI_SIZE_FACTOR, _initialPosition.y * ZoomAndLevelDrag.UI_SIZE_FACTOR, _initialPosition.z * ZoomAndLevelDrag.UI_SIZE_FACTOR );
	}
}
