using UnityEngine;
using System.Collections;

public class OpeningSequenceManger : MonoBehaviour 
{
	//*************************************************************//
	public Transform[] pathForBounce;
	//*************************************************************//
	private GameObject _tileInteractivePrefab;
	private bool _zoomOut = false;
	private GameObject _madraObject;
	private GameObject _tentacleObject;
	private GameObject _coraObject;
	private Vector3 _coraPosition;
	private Vector3 _flagPosition;
	private GameObject _deleteObject;
	private bool _finishing = false;

	private bool _putOnPath = false;
	private float _percenatge = 0f;
	private bool _finishingBounce = false;
	//*************************************************************//	
	private static OpeningSequenceManger _meInstance;
	public static OpeningSequenceManger getInstance ()
	{
		if ( _meInstance == null )
		{
			_meInstance = GameObject.Find ( "_OpeningSeqence" ).GetComponent < OpeningSequenceManger > ();
		}
		
		return _meInstance;
	}
	//*************************************************************//
	/*
	void Start () 
	{
		_tileInteractivePrefab = ( GameObject ) Resources.Load ( "Tile/tileInteractive" );
	}

	public void startOpeningSequence () 
	{
		StartCoroutine ( "startSequence" );
	}

	public void destroy ()
	{
		Destroy ( this.gameObject );
	}


	private IEnumerator startSequence ()
	{
		GlobalVariables.OPENING_SEQUENCE = true;

		_coraObject = LevelControl.getInstance ().getCharacterObjectFromLevel ( GameElements.CHAR_CORA_1_IDLE );
		_coraPosition = VectorTools.cloneVector3 ( _coraObject.transform.position );
		_coraObject.transform.position = new Vector3 ( -100f, 0f, 0f );

		_tentacleObject = LevelControl.getInstance ().gameElementsOnLevel[LevelControl.GRID_LAYER_NORMAL][11][3];
		_tentacleObject.transform.position = new Vector3 ( -100f, 0f, 0f );

		_madraObject = LevelControl.getInstance ().gameElementsOnLevel[LevelControl.GRID_LAYER_NORMAL][9][3];
		_madraObject.transform.position = new Vector3 ( -100f, 0f, 0f );

		_flagPosition = VectorTools.cloneVector3 ( LevelControl.getInstance ().flagOnLevel.transform.position );
		LevelControl.getInstance ().flagOnLevel.transform.position = new Vector3 ( -100f, 0f, 0f );

		_tileInteractivePrefab = ( GameObject ) Resources.Load ( "Spine/Madra/prefab" );
		_deleteObject = ( GameObject ) Instantiate ( _tileInteractivePrefab, new Vector3 (( float ) 6, ( LevelControl.LEVEL_HEIGHT - 2 ), ( float ) 2  - 0.5f ), _tileInteractivePrefab.transform.rotation );
		GameObject interactiveObjectMesh = _deleteObject.transform.Find ( "tile" ).gameObject;
		interactiveObjectMesh.AddComponent < IComponent > ().myID = GameElements.CHAR_MADRA_1_IDLE;
		interactiveObjectMesh.AddComponent < CharacterAnimationControl > ();
		_zoomOut = true;
		yield return new WaitForSeconds ( 0.01f );
		_zoomOut = false;
		interactiveObjectMesh.GetComponent < CharacterAnimationControl > ().playAnimation ( CharacterAnimationControl.MOVE_RIGHT );
		iTween.MoveTo ( _deleteObject, iTween.Hash ( "time", 0.7f, "easetype", iTween.EaseType.linear, "position", _deleteObject.transform.position + Vector3.right * 2f ));
		yield return new WaitForSeconds ( 0.7f );
		interactiveObjectMesh.GetComponent < CharacterAnimationControl > ().playAnimation ( CharacterAnimationControl.MOVE_UP );
		iTween.MoveTo ( _deleteObject, iTween.Hash ( "time", 0.35f, "easetype", iTween.EaseType.linear, "position", _deleteObject.transform.position + Vector3.forward * 1f ));
		yield return new WaitForSeconds ( 0.35f );
		interactiveObjectMesh.GetComponent < CharacterAnimationControl > ().playAnimation ( CharacterAnimationControl.MOVE_RIGHT );
		iTween.MoveTo ( _deleteObject, iTween.Hash ( "time", 0.7f, "easetype", iTween.EaseType.linear, "position", _deleteObject.transform.position + Vector3.right * 2f ));
		yield return new WaitForSeconds ( 0.1f );
		iTween.MoveTo ( Camera.main.gameObject, iTween.Hash ( "time", 2f, "easetype", iTween.EaseType.easeOutBack, "position", new Vector3 ( _deleteObject.transform.position.x, Camera.main.transform.position.y, _deleteObject.transform.position.z )));
		yield return new WaitForSeconds ( 0.3f );
		_tentacleObject.transform.position = new Vector3 ( 11f, 8f - 3f, 3f - 0.5f );
		_tentacleObject.transform.Find ( "tile" ).GetComponent < EnemyAnimationComponent > ().playAnimation ( EnemyAnimationComponent.SPAWN_ANIMATION );
		yield return new WaitForSeconds ( 0.3f );
		_tentacleObject.transform.Find ( "tile" ).GetComponent < EnemyAnimationComponent > ().playAnimation ( EnemyAnimationComponent.ATTACK_ANIMATION_LEFT );
		_tentacleObject.transform.Find ( "tile" ).GetComponent < EnemyAnimationComponent > ().playAnimation ( EnemyAnimationComponent.ATTACK_ANIMATION );
		interactiveObjectMesh.GetComponent < CharacterAnimationControl > ().playAnimation ( CharacterAnimationControl.IDLE_01_ANIMATION );

		SoundManager.getInstance ().playSound ( SoundManager.TENTACLE_DRAINER_ATTACK );

		yield return new WaitForSeconds ( 0.3f );
		SoundManager.getInstance ().playSound ( SoundManager.ELECTROCUTED );

		interactiveObjectMesh.GetComponent < CharacterAnimationControl > ().playAnimation ( CharacterAnimationControl.ELECTROCUTED_ANIMATION, 2f );
		interactiveObjectMesh.AddComponent < SelectedComponenent > ().electrickShock ( false, 2f );
		yield return new WaitForSeconds ( 2.0f );
		_madraObject.transform.position = _deleteObject.transform.position;
		Destroy ( _deleteObject );
		_putOnPath = true;

		yield return new WaitForSeconds ( 0.9f );

		StartCoroutine ( "finish" );
	}

	private IEnumerator finish ()
	{
		if ( ! _finishing )
		{
			_finishing = true;

			StopCoroutine ( "startSequence" );

			_tentacleObject.transform.position = new Vector3 ( 11f, 8f - 3f, 3f - 0.5f );
			if ( _deleteObject ) Destroy ( _deleteObject );
			_madraObject.transform.position = new Vector3 ( 9f, 8f - 3f, 3f - 0.5f );
			_zoomOut = true;
			_coraObject.transform.position = _coraPosition;
			iTween.MoveTo ( Camera.main.gameObject, iTween.Hash ( "time", 2f, "easetype", iTween.EaseType.easeOutBack, "position", new Vector3 ( 5.5f, Camera.main.transform.position.y, 4f )));
			yield return new WaitForSeconds ( 2.3f );
			GlobalVariables.OPENING_SEQUENCE = false;
			TutorialsManager.getInstance ().StartTutorial ();
			LevelControl.getInstance ().flagOnLevel.transform.position = _flagPosition;
			Destroy ( this.gameObject );
		}
	}
	
	void Update ()
	{
#if UNITY_EDITOR
		if ( Input.GetMouseButtonDown ( 0 ))
		{
			StartCoroutine ( "finish" );
		}
#else
		if ( Input.touchCount > 0 )
		{
			StartCoroutine ( "finish" );
		}
#endif
		if ( ! _zoomOut )
		{
			Camera.main.orthographicSize = Mathf.Lerp ( Camera.main.orthographicSize, 2.5f, 0.01f );
		}
		else
		{
			Camera.main.orthographicSize = Mathf.Lerp ( Camera.main.orthographicSize, ZoomAndLevelDrag.MAXIMUM_CAMERA_ZOOM, 0.02f );
		}
		 
		if ( ! _putOnPath || _finishingBounce ) return;
		_percenatge += Time.deltaTime * 2f;
		if ( _percenatge > 1f )
		{
			_finishingBounce = true;
			_percenatge = 1f;
			iTween.PutOnPath ( _madraObject, pathForBounce, _percenatge );
			_madraObject.transform.position = new Vector3 ( 9f, 8f - 3f, 3f - 0.5f );
			return;
		}
		
		iTween.PutOnPath ( _madraObject, pathForBounce, _percenatge );
	}
	*/
}
