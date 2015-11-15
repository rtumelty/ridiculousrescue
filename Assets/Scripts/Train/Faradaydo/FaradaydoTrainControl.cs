using UnityEngine;
using System.Collections;

public class FaradaydoTrainControl : MonoBehaviour 
{
	//*************************************************************//
	public const string IDLE_ANIMATION = "idle";
	public const string ELECTROCUETED_ANIMATION = "electrocuted";
	//*************************************************************//
	void Start () 
	{
		GetComponent < SkeletonAnimation > ().animationName = IDLE_ANIMATION;
	}

	public void playElectrocuted ()
	{
		StartCoroutine ( "waitBeofreIdle" );
	}
	
	private IEnumerator waitBeofreIdle ()
	{
		GetComponent < SkeletonAnimation > ().animationName = ELECTROCUETED_ANIMATION;
		yield return new WaitForSeconds ( 1f );
		GetComponent < SkeletonAnimation > ().animationName = IDLE_ANIMATION;
	}

}
