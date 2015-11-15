using UnityEngine;
using System.Collections;

public class JumpingArrowAbove : MonoBehaviour 
{
	//*************************************************************//
	public bool doNotParentThisArrow = false;
	public bool doNotJump = false;
	private float _countUpdate = 0;
	//*************************************************************//
	private GameObject _jumpingArrowPrefab;
	private GameObject _jumpingArrowInstant;
	private GameObject _lastArrow;
	//*************************************************************//
	void Awake () 
	{
		_jumpingArrowPrefab = ( GameObject ) Resources.Load ( "UI/jumpingArrowMap" );
	}
	
	void Start ()
	{
		_lastArrow = GameObject.Find ("jumpingArrowMap(Clone)");
		Destroy (_lastArrow);
	
		_jumpingArrowInstant = ( GameObject ) Instantiate ( _jumpingArrowPrefab, transform.position, transform.rotation);
		_jumpingArrowInstant.transform.position += _jumpingArrowInstant.transform.forward * - 0.8f + _jumpingArrowInstant.transform.up * 1.2f;
     	/*if(Quaternion.Angle(transform.rotation,GameObject.Find("sky").transform.rotation) > 1)
		{
			_jumpingArrowInstant = ( GameObject ) Instantiate ( _jumpingArrowPrefab, transform.position + Vector3.forward * 0.8f + Vector3.up * 1.2f + Vector3.left * .33f,transform.rotation );
		}
		else
		{
			_jumpingArrowInstant = ( GameObject ) Instantiate ( _jumpingArrowPrefab, transform.position + Vector3.forward * 1.0f + Vector3.up * 1.2f , _jumpingArrowPrefab.transform.rotation );
		}*/
		//print(Quaternion.Angle(transform.rotation,GameObject.Find("sky").transform.rotation));
		/*
		if ( doNotJump )
		{
			Destroy ( _jumpingArrowInstant.GetComponent < JumpingUIElement > ());
			_jumpingArrowPrefab.transform.Rotate ( 0f, 180f, 0f );
		}
		else _jumpingArrowInstant.GetComponent < JumpingUIElement > ().typeFromAbove = true;
		*/

		if ( ! doNotParentThisArrow ) _jumpingArrowInstant.transform.parent = transform;
	}

	public void destroyJumpingArrow ()
	{
		Destroy ( _jumpingArrowInstant );
	}
}
