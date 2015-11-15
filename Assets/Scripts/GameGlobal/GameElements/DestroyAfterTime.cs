using UnityEngine;
using System.Collections;

public class DestroyAfterTime : MonoBehaviour 
{
	//*************************************************************//
	public float time;
	//*************************************************************//
	IEnumerator Start () 
	{
		yield return new WaitForSeconds ( time );
		Destroy ( this.gameObject );
	}
}
