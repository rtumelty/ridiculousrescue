using UnityEngine;
using System.Collections;

public class FLInteractiveDestroySelectedCharHighlight : MonoBehaviour 
{
	public GameObject myTarget;

	void OnMouseDown ()
	{
		if ( FLGlobalVariables.checkForMenus ()) return;
		if ( gameObject.GetComponent < CharacterTapTutorialControl > () != null ) return;
		resetSelectedChar ();
	}

	void resetSelectedChar ()
	{
		Destroy (myTarget);
	}

	void Update ()
	{
		if (myTarget == null) 
		{
			myTarget = GameObject.Find ("tileMarkerCharacters(Clone)");
		}
	}
}
