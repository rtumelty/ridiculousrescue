using UnityEngine;
using System.Collections;

public class MoveMadraPointer : MonoBehaviour 
{
	//*************************************************************//
	private GameObject _slideArrowPrefab;
	private GameObject _slideArrowInstant;
	//*************************************************************//
	void Awake () 
	{
		_slideArrowPrefab = ( GameObject ) Resources.Load ( "UI/slideArrow" );
		print ("MMEEEEEEEEEEEEEEEEEEEE");
	}

	IEnumerator Start () 
	{
		_slideArrowInstant = ( GameObject ) Instantiate ( _slideArrowPrefab, new Vector3 ( transform.position.x, 15f, transform.position.z ), Quaternion.identity );
		Destroy ( _slideArrowInstant.GetComponent < SlideArrowUIElement > ());

		while ( LevelControl.getInstance ().getCharacter ( GameElements.CHAR_MADRA_1_IDLE ).position[0] != 7 || LevelControl.getInstance ().getCharacter ( GameElements.CHAR_MADRA_1_IDLE ).position[1] != 3 )
		{ 
			_slideArrowInstant.transform.position = new Vector3 ( transform.position.x, 15f, transform.position.z );
			_slideArrowInstant.AddComponent < SimulateTapControl > ().scale = VectorTools.cloneVector3 ( _slideArrowInstant.transform.localScale );
			yield return new WaitForSeconds ( 1.5f );
			_slideArrowInstant.transform.position = new Vector3 ( 7f, 15f, 3f );
			_slideArrowInstant.GetComponent < SimulateTapControl > ().resetScale ();
			yield return new WaitForSeconds ( 1.5f );
		}
	}

	void Update ()
	{
		if ( LevelControl.getInstance ().getCharacter ( GameElements.CHAR_MADRA_1_IDLE ).position[0] == 7 && LevelControl.getInstance ().getCharacter ( GameElements.CHAR_MADRA_1_IDLE ).position[1] == 3 )
		{
			Destroy ( _slideArrowInstant );
			Destroy ( this );
		}
	}
}
