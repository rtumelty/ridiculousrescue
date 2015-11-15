using UnityEngine;
using System.Collections;

public class TREngineDisplay : MonoBehaviour 
{
	private GameObject blueMotor;
	private GameObject redMotor;


	void Start () 
	{
		blueMotor = transform.Find ("blueMotor").gameObject;
		redMotor = transform.Find ("redMotor").gameObject;
		redMotor.SetActive (false);
		if(TRLevelControl.LEVEL_ID == 3)
		{
			blueMotor.SetActive(false);
			redMotor.SetActive(true);
		}
	}
	

}