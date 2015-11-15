using UnityEngine;
using System.Collections;

public class TapStarsButtonControl : MonoBehaviour 
{
	IEnumerator OnMouseUp ()
	{
		SoundManager.getInstance ().playSound ( SoundManager.CONFIRM_BUTTON );
		yield return new WaitForSeconds (0.3f);
		SoundManager.getInstance ().playSound (SoundManager.LEVEL_NODE_UNLOCKED);
		handleTouched ();
	}
	
	private void handleTouched ()
	{
		GoogleAnalytics.instance.LogScreen ( "User has enough STARS and unlocked level 13" );

		FLMissionScreenMapDialogManager.getInstance ().startUnlockingProcedure ();
	}
}
