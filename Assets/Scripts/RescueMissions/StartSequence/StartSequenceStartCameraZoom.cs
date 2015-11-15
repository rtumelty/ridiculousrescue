using UnityEngine;
using System.Collections;

public class StartSequenceStartCameraZoom : MonoBehaviour 
{
	//*************************************************************//
	private bool _zoomOut = false;
	private GameObject _uiPanelObject;
	private GameObject myTutorialBbox;
	public bool fromQuestionClick = false;
	public bool putCamBack = false;
	public bool ignoreTimer = false;
	public bool hideFrame = true;
	public GameObject myTapText;
	public GameObject myFrameText;
	public bool UIvisible = false;
	//*************************************************************//	
	void Start () 
	{
		_uiPanelObject = transform.Find ( "UI" ).gameObject;
		iTween.MoveTo ( _uiPanelObject, iTween.Hash ( "time", 0.5f, "easetype", iTween.EaseType.easeInOutSine, "position", new Vector3 ( _uiPanelObject.transform.position.x, 7f, _uiPanelObject.transform.position.z ), "islocal", true ));
		Vector3 positionToMove = new Vector3 ( LevelControl.getInstance ().toBeRescuedOnLevel.transform.position.x, transform.position.y, LevelControl.getInstance ().toBeRescuedOnLevel.transform.position.z + 1f );
		iTween.MoveTo ( this.gameObject, iTween.Hash ( "time", 1f, "easetype", iTween.EaseType.easeOutBack, "position", positionToMove, "oncomplete", "onCompleteTweenAnimationMoveToPosition01" ));

		if(GameObject.Find ("tutorialComboUIFullBig(Clone)") != null)
		{
			myTutorialBbox = GameObject.Find ("tutorialComboUIFullBig(Clone)");
			iTween.MoveTo ( myTutorialBbox, iTween.Hash ( "time", 0.5f, "easetype", iTween.EaseType.easeInOutBounce, "position", new Vector3 ( myTutorialBbox.transform.position.x - 5.5f, -2.5f, myTutorialBbox.transform.position.z - 6 ), "islocal", true ));
			print ("happened");

			myTapText = myTutorialBbox.transform.Find("tapText").gameObject;
			myTapText.renderer.enabled = false;

			myFrameText = myTutorialBbox.transform.Find("frameText").gameObject;
			myFrameText.renderer.enabled = false;


			myTutorialBbox.transform.Find("frame").renderer.enabled = false;
			hideFrame = true;
		}
	}
	
	private void onCompleteTweenAnimationMoveToPosition01 ()
	{
		if(fromQuestionClick == true)
		{
			TriggerDialogueControl.getInstance().waitOnSequenceEnd = true;
			print ("%%%%%%%%%%%%"+TriggerDialogueControl.getInstance().waitOnSequenceEnd);
			UIvisible = true;
			myTutorialBbox.transform.Find("tapText").renderer.enabled = true;
			myTutorialBbox.transform.Find("frameText").renderer.enabled = true;
			myTutorialBbox.transform.Find("frame").renderer.enabled = true;
			myTapText.transform.position = new Vector3 (myTapText.transform.position.x, myTapText.transform.position.y, myTapText.transform.position.z + 0.9f);
			myFrameText.transform.position = new Vector3 (myFrameText.transform.position.x, myFrameText.transform.position.y, myFrameText.transform.position.z + 0.6f);
			GameObject.Find ("hand(Clone)").renderer.enabled = true;
			GameObject.Find ("jumpingArrow(Clone)").GetComponent<JumpingUIElement>().onCompleteGoingDownFromDialogBox();
			hideFrame = false;
		}
		LevelControl.getInstance ().toBeRescuedOnLevel.transform.Find ( "tile" ).GetComponent < SelectedComponenent > ().setSelected ( true );
		StartCoroutine ( "waitBeforeMoveCameraBack" );
	}
	
	private IEnumerator waitBeforeMoveCameraBack ()
	{
		if(fromQuestionClick == false)
		{
			if(ignoreTimer == false)
			{
				yield return new WaitForSeconds ( 1f );
			}
			_zoomOut = true;
			Vector3 positionToMove = new Vector3 ( 5.5f, transform.position.y, 4f );
			iTween.MoveTo ( this.gameObject, iTween.Hash ( "time", 1.5f, "easetype", iTween.EaseType.easeOutBack, "position", positionToMove, "oncomplete", "onCompleteTweenAnimationMoveToPosition02" ));
			ignoreTimer = false;
		}
	}
	
	private void onCompleteTweenAnimationMoveToPosition02 ()
	{
		StartSequenceManager.getInstance ().goToNextStep ();
		Destroy ( this );
	}
	
	void Update ()
	{
		if(fromQuestionClick == true && hideFrame == true && GameObject.Find("hand(Clone)") != null)
		{
			GameObject.Find("hand(Clone)").renderer.enabled = false;
		}
		if ( ! _zoomOut )
		{
			camera.orthographicSize = Mathf.Lerp ( camera.orthographicSize, 2.5f, 0.07f );
		}
		else
		{
			camera.orthographicSize = Mathf.Lerp ( camera.orthographicSize, ZoomAndLevelDrag.MAXIMUM_CAMERA_ZOOM, 0.12f );
		}
		if(putCamBack == true)
		{
			fromQuestionClick = false;
			ignoreTimer = true;
			StartCoroutine("waitBeforeMoveCameraBack");

			putCamBack = false;
			print ("MUCH SAD");
			TriggerDialogueControl.getInstance().StartCoroutine("releaseQuestionMarkButton");
		}
	}
}
