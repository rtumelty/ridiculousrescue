using UnityEngine;
using System.Collections;

public class Load00 : MonoBehaviour 
{
	IEnumerator Start ()
	{
		yield return new WaitForSeconds ( 1f );
		Application.LoadLevel ( "00" );
	}
}
