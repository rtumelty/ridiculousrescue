using UnityEngine;
using System.Collections;

public class AutoDestruct : MonoBehaviour 
{
	public float destroyAfterSeconds = 2f;
	
	void Start () 
	{
		StartCoroutine ( "waitBeforeDestory" );
	}
	
	private IEnumerator waitBeforeDestory () 
	{
		yield return new WaitForSeconds ( destroyAfterSeconds );
		Destroy ( this.gameObject );
	}
}
