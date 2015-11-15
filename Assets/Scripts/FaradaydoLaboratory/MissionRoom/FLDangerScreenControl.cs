using UnityEngine;
using System.Collections;

public class FLDangerScreenControl : MonoBehaviour 
{
	//*************************************************************//
	public string timeString;
	//*************************************************************//
	private TextMesh _myTimeText;
	//*************************************************************//
	void Awake ()
	{
		_myTimeText = transform.Find ( "textTime" ).GetComponent < TextMesh > ();
	}
	
	void Update () 
	{
		_myTimeText.text = timeString;
	}
}
