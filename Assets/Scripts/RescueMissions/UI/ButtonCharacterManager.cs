using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class ButtonCharacterManager : ButtonManager 
{
	//*************************************************************//
	public UIControl.ButtonCharacterCallBack myCallBackCharacter;
	public CharacterData character;
	//*************************************************************//
	
	void OnMouseUp ()
	{
		if ( myCallBackCharacter != null ) myCallBackCharacter ( character );
		
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
}
