using UnityEngine;
using System.Collections;

public class ObjectiveComponent : MonoBehaviour 
{
	public bool isUp = false;
	public bool runOnce = false;

	public Vector3 _centrePosition;
	public Vector3 _initialPosition;

	void Start ()
	{
		_initialPosition = transform.localPosition;
		_centrePosition = new Vector3 (transform.localPosition.x, transform.localPosition.y, transform.localPosition.z + 6);
	}

	IEnumerator PlayMyAnim ()
	{
		yield return new WaitForSeconds (1f);
		iTween.MoveTo ( gameObject, iTween.Hash ( "time", 0.7f, "easetype", iTween.EaseType.easeOutBounce, "position", _centrePosition, "islocal", true ));
		isUp = true;


		yield return new WaitForSeconds (3f);
		isUp = false;
		iTween.MoveTo ( gameObject, iTween.Hash ( "time", 0.7f, "easetype", iTween.EaseType.easeInSine, "position", _initialPosition, "islocal", true ));
		yield return new WaitForSeconds (0.7f);
		gameObject.SetActive (false);
		TimerControl.getInstance ().startTimer (true);
		TutorialComponent.getInstance ().CustomStart ();
	}

	IEnumerator QuickTapOffScreen ()
	{
		yield return new WaitForSeconds (0f);
		iTween.MoveTo ( gameObject, iTween.Hash ( "time", 0.7f, "easetype", iTween.EaseType.easeInSine, "position", _initialPosition, "islocal", true ));
		yield return new WaitForSeconds (0.7f);
		gameObject.SetActive (false);
		TimerControl.getInstance ().startTimer (true);
		TutorialComponent.getInstance ().CustomStart ();
	}

	void OnMouseDown ()
	{
		if(isUp == true)
		{
			StartCoroutine ("QuickTapOffScreen");
		}
	}	

	void Update ()
	{
		if(GlobalVariables.checkForMenus() == true)
		{
			return;
		}
		else if(runOnce == false)
		{
			StartCoroutine ("PlayMyAnim");
			runOnce = true;
		}
	}
}
