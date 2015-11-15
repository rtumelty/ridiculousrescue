using UnityEngine;
using System.Collections;

public class ToyLazarusSequenceSkipOnTapControl : ObjectTapControl 
{
	void OnMouseUp () 
	{
		if ( _alreadyTouched ) return;
		if ( ResoultScreen.getInstance ().resoultScreenIsOn ) return;

		_alreadyTouched = true;
		
		ToyLazarusSequenceControl.getInstance ().skipAll ();
	}
}
