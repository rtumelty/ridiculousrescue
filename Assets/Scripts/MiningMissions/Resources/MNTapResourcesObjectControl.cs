using UnityEngine;
using System.Collections;

public class MNTapResourcesObjectControl : ObjectTapControl 
{
	//*************************************************************//
	public bool autoCollect = true;
	public bool popUp = false;
	public Vector3 myPopToPosition;
	//*************************************************************//
	private float _countTimeToAutoCollect = 0.3f;
	//*************************************************************//
	void Awake () 
	{
		iTween.ScaleFrom ( this.gameObject, iTween.Hash ( "time", Random.Range ( 0.4f, 1.5f ), "easetype", iTween.EaseType.easeOutBounce, "scale", transform.localScale * 0.1f ));
		myPopToPosition = new Vector3(transform.position.x, transform.position.y,transform.position.z + 1f);
	}
	
	void OnMouseUp ()
	{
		if ( _alreadyTouched ) return;
		handleTouched ();
	}

	private void handleTouched ()
	{
		if ( _alreadyTouched ) return;
		_alreadyTouched = true;
		SoundManager.getInstance ().playSound ( SoundManager.CONFIRM_BUTTON );
		StartCoroutine ("popUpFirst");
		/*switch ( GetComponent < IComponent > ().myID )
		{
			case GameElements.ICON_METAL:
				iTween.MoveTo ( this.gameObject, iTween.Hash ( "time", Random.Range ( 1.2f, 1.5f ), "easetype", iTween.EaseType.easeInSine, "position", MNMiningResourcesControl.getInstance ().iconMetalTransform.position, "oncomplete", "destoryOnComplete" ));
				break;
			case GameElements.ICON_PLASTIC:
				iTween.MoveTo ( this.gameObject, iTween.Hash ( "time", Random.Range ( 1.2f, 1.5f ), "easetype", iTween.EaseType.easeInSine, "position", MNMiningResourcesControl.getInstance ().iconPlasticTransform.position, "oncomplete", "destoryOnComplete" ));
				break;
			case GameElements.ICON_VINES:
				iTween.MoveTo ( this.gameObject, iTween.Hash ( "time", Random.Range ( 1.2f, 1.5f ), "easetype", iTween.EaseType.easeInSine, "position", MNMiningResourcesControl.getInstance ().iconVinesTransform.position, "oncomplete", "destoryOnComplete" ));
				break;
			case GameElements.ICON_TECHNOSEED:
				iTween.MoveTo ( this.gameObject, iTween.Hash ( "time", Random.Range ( 1.2f, 1.5f ), "easetype", iTween.EaseType.easeInSine, "position", MNMiningResourcesControl.getInstance ().iconVinesTransform.position, "oncomplete", "destoryOnComplete" ));
				break;
		}*/
	}

	IEnumerator popUpFirst()
	{
		popUp = true;
		yield return new WaitForSeconds (.75f);
		popUp = false;
		switch ( GetComponent < IComponent > ().myID )
		{
			case GameElements.ICON_METAL:
			iTween.MoveTo ( this.gameObject, iTween.Hash ( "time", Random.Range ( .8f, 1.1f ), "easetype", iTween.EaseType.easeInQuart, "position", MiningResourcesControl.getInstance ().iconMetalTransform.position, "oncomplete", "destoryOnComplete" ));
			break;
			case GameElements.ICON_PLASTIC:
			iTween.MoveTo ( this.gameObject, iTween.Hash ( "time", Random.Range ( .8f, 1.1f ), "easetype", iTween.EaseType.easeInQuart, "position", MiningResourcesControl.getInstance ().iconPlasticTransform.position, "oncomplete", "destoryOnComplete" ));
			break;
			case GameElements.ICON_VINES:
			iTween.MoveTo ( this.gameObject, iTween.Hash ( "time", Random.Range ( .8f, 1.1f ), "easetype", iTween.EaseType.easeInQuart, "position", MiningResourcesControl.getInstance ().iconVinesTransform.position, "oncomplete", "destoryOnComplete" ));
			break;
			case GameElements.ICON_TECHNOSEED:
			iTween.MoveTo ( this.gameObject, iTween.Hash ( "time", Random.Range ( .8f, 1.1f ), "easetype", iTween.EaseType.easeInQuart, "position", MiningResourcesControl.getInstance ().iconVinesTransform.position, "oncomplete", "destoryOnComplete" ));
			break;
		}
	}
	
	private void destoryOnComplete ()
	{
		switch ( GetComponent < IComponent > ().myID )
		{
			case GameElements.ICON_METAL:
			print ( GameGlobalVariables.Stats.NewResources.METAL );
				//GameGlobalVariables.Stats.NewResources.METAL++;
				GameGlobalVariables.Stats.METAL_IN_CONTAINERS++;
			SaveDataManager.save(SaveDataManager.METAL_IN_CONTAINERS, GameGlobalVariables.Stats.METAL_IN_CONTAINERS);
				break;
			case GameElements.ICON_PLASTIC:
			if ( GameGlobalVariables.Stats.NewResources.PLASTIC + 1 > ResourcesManager.CURRENT_MAX_PLASTIC )
			{
				if ( ! MNGlobalVariables.TUTORIAL_MENU )
				{
					TutorialsManager.getInstance ().triggerDialogForMiningTutorialTooMuchPlastic ();
				}
			}
			else GameGlobalVariables.Stats.NewResources.PLASTIC++;
				break;
			case GameElements.ICON_VINES:
			if ( GameGlobalVariables.Stats.NewResources.VINES + 1 > ResourcesManager.CURRENT_MAX_VINES )
			{
				if ( ! MNGlobalVariables.TUTORIAL_MENU )
				{
					TutorialsManager.getInstance ().triggerDialogForMiningTutorialTooMuchVines ();
				}
			}
			else GameGlobalVariables.Stats.NewResources.VINES++;
				break;
			case GameElements.ICON_TECHNOSEED:
				GameGlobalVariables.Stats.NewResources.PREMIUM_CURRENCY++;
				break;
		}
		
		Destroy ( transform.parent.gameObject );
	}
	
	private void destoryOnCompleteButDontAdd ()
	{
		Destroy ( transform.parent.gameObject );
	}
	
	void Update ()
	{
		if(popUp = true)
		{
			transform.position = Vector3.MoveTowards (transform.position, myPopToPosition, Time.deltaTime * 3f);
		}
		_countTimeToAutoCollect -= Time.deltaTime;
		
		if ( _countTimeToAutoCollect <= 0f )
		{
			if ( autoCollect )
			{
				handleTouched ();
			}
			else
			{
				if ( _alreadyTouched ) return;
				_alreadyTouched = true;
				SoundManager.getInstance ().playSound ( SoundManager.CANCEL_BUTTON );
				iTween.ScaleTo ( this.gameObject, iTween.Hash ( "time", Random.Range ( 0.2f, 0.5f ), "easetype", iTween.EaseType.easeInBack, "scale", Vector3.zero, "oncomplete", "destoryOnCompleteButDontAdd" ));
			}
		}
	}
}
