using UnityEngine;
using System.Collections;

public class FL_MOMRedirectorDisplayComponent : MonoBehaviour 
{
	private float _countUpdate = 0;
	void Update () 
	{
		_countUpdate -= Time.deltaTime;
		
		if ( _countUpdate > 0f ) return;
		
		_countUpdate = 1f;
	
		int worldID = SaveDataManager.getValue ( SaveDataManager.WORLD_COMPLETED_PREFIX + "1" );
		if( worldID < 1)
		{
			gameObject.SetActive(false);
		}
	}
}
