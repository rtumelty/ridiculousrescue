using UnityEngine;
using System.Collections;

public class TRTentacleTapControl : MonoBehaviour 
{
	void OnMouseOver ()
	{
		transform.parent.gameObject.SendMessage ( "OnMouseDown" );
	}
}
