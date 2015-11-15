using UnityEngine;
using System.Collections;

public class PlugComponent : CoresComponent 
{
	void Update () 
	{
		if (( connected ) && ( ! GlobalVariables.PLUG_CONNECTED ))
		{
			SoundManager.getInstance ().playSound ( SoundManager.POWER_PLUG_CONNECTED );
			
			GameObject faradaydoObject = LevelControl.getInstance ().getCharacterObjectFromLevel ( GameElements.CHAR_FARADAYDO_1_IDLE );
			faradaydoObject.transform.Find ( "tile" ).GetComponent < SelectedComponenent> ().setSelectedForCelebration ( true );
			GlobalVariables.PLUG_CONNECTED = true;
			//LevelControl.getInstance ().coresOnLevel.transform.Find ( "tile" ).GetComponent < TipOnClickComponent > ().deActivate ();
			Main.getInstance ().checkForWinConditions ();
			
			GetComponent < StaticObjectWithAnimation > ().startAnimation ();
		}
	}
}
