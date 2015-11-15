using UnityEngine;
using System.Collections;

public class ButtonSkipControl : MonoBehaviour 
{
	void OnMouseUp ()
	{
		handleTouched ();
	}
	
	private void handleTouched ()
	{
		IntroManger.getInstance ().skip ();
	}
}
