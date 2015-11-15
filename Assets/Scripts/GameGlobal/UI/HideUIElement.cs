using UnityEngine;
using System.Collections;

public class HideUIElement : MonoBehaviour 
{
	//*************************************************************//
	public bool slideDonw = false;
	//*************************************************************//
	private GameObject _currentBackOfMoMUICombo;
	//*************************************************************//
	void Start ()
	{
		if ( gameObject.name == "momUICombo(Clone)" )
		{
			transform.Find ( "momBlackOut" ).gameObject.AddComponent < AlphaDisapearAndDestory > ();
			_currentBackOfMoMUICombo = transform.Find ( "momBlackOut" ).gameObject;
			transform.Find ( "momBlackOut" ).parent = null;

			StartCoroutine ( "destoryMoMBack" );
		}

		if ( slideDonw ) iTween.MoveTo ( this.gameObject, iTween.Hash ( "time", 0.35f, "easetype", iTween.EaseType.easeInOutQuad, "position", transform.position + Vector3.back * 10f, "oncomplete", "onComplete" ));
		else iTween.ScaleTo ( this.gameObject, iTween.Hash ( "time", 0.45f, "easetype", iTween.EaseType.easeInOutQuad, "scale", Vector3.zero, "oncomplete", "onComplete" ));
	}
	
	private void onComplete ()
	{
		Destroy ( this.gameObject );
	}

	private IEnumerator destoryMoMBack ()
	{
		yield return new WaitForSeconds ( 0.5f );
		Destroy ( _currentBackOfMoMUICombo );
	}
}
