using UnityEngine;
using System.Collections;

public class ToyLazarusSequenceCoraMove : MonoBehaviour 
{
	//************************************************//
	private GameObject _toBeRescuedObject;
	private bool _putOnPath = false;
	private float _percenatge = 0f;
	private bool _finishing = false;
	//************************************************//
	void Start () 
	{
		transform.Find ( "tile" ).GetComponent < CharacterAnimationControl > ().playAnimation ( CharacterAnimationControl.INTERACT_LEFT_ANIMATION_02 );
		transform.position = new Vector3 ( 0f, (float) LevelControl.LEVEL_HEIGHT, 1 - 0.5f );
		Vector3 positionToMove = new Vector3 ( -16f, (float) LevelControl.LEVEL_HEIGHT, 1 - 0.5f );
		iTween.MoveTo ( this.gameObject, iTween.Hash ( "time", 1f, "easetype", iTween.EaseType.easeOutSine, "position", positionToMove, "oncomplete", "onCompleteTweenAnimationMoveToPosition01"));
	}

	void Update ()
	{
		if ( ! _putOnPath || _finishing ) return;
		_percenatge += Time.deltaTime * 2f;
		if ( _percenatge > 1f )
		{
			_finishing = true;
			_percenatge = 1f;
			moveCoraRight ();
			iTween.PutOnPath ( _toBeRescuedObject, ToyLazarusSequenceControl.getInstance ().pathForBounce, _percenatge );
			return;
		}

		iTween.PutOnPath ( _toBeRescuedObject, ToyLazarusSequenceControl.getInstance ().pathForBounce, _percenatge );
	}
	
	private void onCompleteTweenAnimationMoveToPosition01 () 
	{
		Vector3 positionToMove = new Vector3 ( -16f, 5f, 2.66f );
		iTween.MoveTo ( this.gameObject, iTween.Hash ( "time", 1f, "easetype", iTween.EaseType.easeOutSine, "position", positionToMove, "oncomplete", "onCompleteTweenAnimationMoveToPosition02"));
	}
	
	private void onCompleteTweenAnimationMoveToPosition02 () 
	{
		Vector3 positionToMove = new Vector3 ( -17.5f, 5f, 2.9f );
		iTween.MoveTo ( this.gameObject, iTween.Hash ( "time", 0.4f, "easetype", iTween.EaseType.linear, "position", positionToMove, "oncomplete", "onCompleteTweenAnimationMoveToPosition03"));
	}
	
	private void onCompleteTweenAnimationMoveToPosition03 () 
	{	
		_toBeRescuedObject = transform.Find ( "toBeRescued" ).gameObject;
		transform.Find ( "tile" ).localScale = VectorTools.cloneVector3 ( RescuerComponent.CORA_WITHOUT_TROLEY_SCALE );
		_toBeRescuedObject.transform.parent = null;
		transform.Find ( "tile" ).GetComponent < CharacterAnimationControl > ().changeState ( CharacterAnimationControl.INTERACT_LEFT );
		_putOnPath = true;

		SoundManager.getInstance ().playSound ( SoundManager.TOY_DEPOSITED );
	}

	private void moveCoraRight ()
	{
		transform.Find ( "tile" ).GetComponent < CharacterAnimationControl > ().playAnimation ( CharacterAnimationControl.MOVE_RIGHT );
		Vector3 positionToMove = new Vector3 ( -16f, 5f, 2.66f );
		iTween.MoveTo ( this.gameObject, iTween.Hash ( "time", 0.45f, "easetype", iTween.EaseType.easeOutSine, "position", positionToMove, "oncomplete", "onCompleteTweenAnimationMoveToPositionAndNextStep" ));
	}

	private void onCompleteJumpToyRescue ()
	{
		moveCoraRight ();
	}

	private void onCompleteTweenAnimationMoveToPositionAndNextStep ()
	{
		transform.Find ( "tile" ).GetComponent < CharacterAnimationControl > ().changeState ( CharacterAnimationControl.LEFT );
		ToyLazarusSequenceControl.getInstance ().goToNextStep ();
		Destroy ( this );
	}
}
