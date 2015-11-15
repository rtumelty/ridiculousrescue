using UnityEngine;
using System.Collections;

public class RandomMoveLockParts : MonoBehaviour 
{
	//*************************************************************//
	public Vector3 moveToPosition;
	//*************************************************************//
	private int _rotationDirection;
	//*************************************************************//
	void Awake () 
	{
		_rotationDirection = Random.Range ( 0, 3 );
		iTween.MoveTo ( this.gameObject, iTween.Hash ( "time", 3f, "easetype", iTween.EaseType.linear, "position", transform.position + moveToPosition ));
	}

	void Update ()
	{
		if ( _rotationDirection == 0 ) transform.Rotate ( 0f, 380f * Time.deltaTime, 0f );
		else if ( _rotationDirection == 1 ) transform.Rotate ( 0f, -380f * Time.deltaTime, 0f );
	}
}
