using UnityEngine;
using System.Collections;

public class ScaleUIElementControl : MonoBehaviour 
{
	//*************************************************************//
	public bool scalePosition = false;
	//*************************************************************//
	private Vector3 _initialScale;
	private Vector3 _initialPosition;
	//*************************************************************//
	void Start ()
	{
		_initialScale = VectorTools.cloneVector3 ( transform.localScale );
		_initialPosition = VectorTools.cloneVector3 ( transform.localPosition );
	}
	
	void LateUpdate () 
	{
		if ( gameObject.GetComponent < HideUIElement > () != null ) return;
		
		if ( GameGlobalVariables.CURRENT_GAME_PART == GameGlobalVariables.RESCUE )
		{
			transform.localScale = VectorTools.cloneVector3 ( _initialScale * ZoomAndLevelDrag.UI_SIZE_FACTOR );
			if ( scalePosition ) transform.localPosition = new Vector3 ( transform.localPosition.x, _initialPosition.y * ( ZoomAndLevelDrag.UI_SIZE_FACTOR ),  transform.localPosition.z );
		}
		else if ( GameGlobalVariables.CURRENT_GAME_PART == GameGlobalVariables.LABORATORY )
		{
			transform.localScale = VectorTools.cloneVector3 ( _initialScale * FLZoomAndLevelDrag.UI_SIZE_FACTOR );
		}
		else if ( GameGlobalVariables.CURRENT_GAME_PART == GameGlobalVariables.MINING )
		{
			transform.localScale = VectorTools.cloneVector3 ( _initialScale * MNZoomAndLevelDrag.UI_SIZE_FACTOR );
		}
	}
}
