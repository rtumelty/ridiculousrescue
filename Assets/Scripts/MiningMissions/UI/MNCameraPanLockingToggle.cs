using UnityEngine;
using System.Collections;

public class MNCameraPanLockingToggle : MonoBehaviour 
{
	
	private float _countUpdate = 0;
	
	void Update () 
	{
		_countUpdate -= Time.deltaTime;
		
		if ( _countUpdate > 0f ) return;
		
		_countUpdate = 1f;
		if (Camera.main.orthographicSize < 5.5f)
		{
			MNZoomAndLevelDrag.getInstance().panUnlocked = true;
		}
		else
		{
			MNZoomAndLevelDrag.getInstance().panUnlocked = false;
		}
	}
}
