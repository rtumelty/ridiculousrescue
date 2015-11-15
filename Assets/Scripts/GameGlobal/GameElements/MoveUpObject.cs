using UnityEngine;
using System.Collections;

public class MoveUpObject : MonoBehaviour 
{
	IEnumerator Start () 
	{
		yield return new WaitForSeconds ( 0.1f );
		iTween.MoveTo ( this.gameObject, iTween.Hash ( "time", 2f, "easetype", iTween.EaseType.linear, "position", transform.position + Vector3.forward * 4f + Vector3.up * 4f ));
	}
}
