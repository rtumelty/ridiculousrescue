using UnityEngine;
using System.Collections;

public class LockScreenControl : MonoBehaviour 
{
	//*************************************************************//
	public static int NUMBER_OF_STARS = 0;
	//*************************************************************//
	void Update () 
	{
		transform.Find ( "scaleSolver" ).Find ( "unlockWithStarsButton" ).Find ( "textStars" ).GetComponent < TextMesh > ().text = NUMBER_OF_STARS.ToString () + "/30";
		if ( NUMBER_OF_STARS >= 30 )
		{
			transform.Find ( "scaleSolver" ).Find ( "unlockWithStarsButton" ).collider.enabled = true;
			transform.Find ( "scaleSolver" ).Find ( "unlockWithStarsButton" ).renderer.material.mainTexture = FLMissionScreenMapDialogManager.getInstance ().buttonNormal;
			transform.Find ( "scaleSolver" ).Find ( "unlockWithStarsButton" ).Find ( "iconStar" ).renderer.material.mainTexture = FLMissionScreenMapDialogManager.getInstance ().starNormal;
		}
		else
		{
			transform.Find ( "scaleSolver" ).Find ( "unlockWithStarsButton" ).collider.enabled = false;
			transform.Find ( "scaleSolver" ).Find ( "unlockWithStarsButton" ).renderer.material.mainTexture = FLMissionScreenMapDialogManager.getInstance ().buttonGrayedOut;
			transform.Find ( "scaleSolver" ).Find ( "unlockWithStarsButton" ).Find ( "iconStar" ).renderer.material.mainTexture = FLMissionScreenMapDialogManager.getInstance ().starGrayedOut;
		}
	}
}
