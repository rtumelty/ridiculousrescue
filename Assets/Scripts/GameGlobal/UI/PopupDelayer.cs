using UnityEngine;
using System.Collections;

public class PopupDelayer : MonoBehaviour 
{
	IEnumerator Start () 
	{
		yield return new WaitForSeconds ( 0.1f );
		transform.position += Vector3.back * 12f;
		
		Destroy ( this );
	}
}
