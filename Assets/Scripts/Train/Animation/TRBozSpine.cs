using UnityEngine;
using System.Collections;

public class TRBozSpine : MonoBehaviour 
{
	
	void Start () 
	{
		if(TRLevelControl.LEVEL_ID < 2)
		{
			this.gameObject.SetActive(false);
		}
	}

	void Update ()
	{
	
	}
}
