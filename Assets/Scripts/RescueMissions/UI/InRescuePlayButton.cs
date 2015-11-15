using UnityEngine;
using System.Collections;

public class InRescuePlayButton : MonoBehaviour {

	void OnMouseUp ()
	{
		Destroy (UIControl.currentBlackOutUI);
		Destroy (UIControl.currentPopupUI);
		UIControl.currentPopupUI = null;
		//UIControl.getInstance ().createPopup (UIControl.getInstance ()._getLivesUI, true, "NULL", transform.parent.transform.parent);
		UIControl.getInstance().createPopup(UIControl.getInstance()._getLivesUIAlt, true);
	}
}
