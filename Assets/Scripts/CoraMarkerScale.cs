using UnityEngine;
using System.Collections;

public class CoraMarkerScale : MonoBehaviour {
	public enum Direction {
		NONE,
		UP,
		DOWN,
		LEFT,
		RIGHT
	}

	static Direction lastDir = Direction.LEFT;

	//[SerializeField] Transform markerParent;
	//[SerializeField] GameObject marker;
	[SerializeField] GameObject arrow;
	[SerializeField] float _customPulsingSizeFactor = 1.5f;

	void Start() {
		UpdateScaleAndPosition (Direction.NONE);
		
		//onCompleteTweenAnimationDownPulseCharacterMark (marker);
		onCompleteTweenAnimationDownPulseCharacterMark (arrow);
	}

	public void UpdateScaleAndPosition (Direction d = Direction.NONE) {
		if (d == Direction.NONE)
				d = lastDir;
		else
				lastDir = d;

		//marker.SetActive (true);
		switch (d) {
		case Direction.UP:
		//	markerParent.localScale = new Vector3(1,1,2);
		//	markerParent.position = transform.position + new Vector3(0, 0, -.5f);
			arrow.transform.position = transform.position + new Vector3(0, 0, -.5f);
			break;
		case Direction.DOWN:
		//	markerParent.localScale = new Vector3(1,1,2);
		//	markerParent.position = transform.position + new Vector3(0, 0, .5f);
			arrow.transform.position = transform.position + new Vector3(0, 0, .5f);
			break;
		case Direction.LEFT:
		//	markerParent.localScale = new Vector3(2,1,1);
		//	markerParent.position = transform.position + new Vector3(0.5f, 0, 0f);
			arrow.transform.position = transform.position + new Vector3(0.5f, 0, -0f);
			break;
		case Direction.RIGHT:
		//	markerParent.localScale = new Vector3(2,1,1);
		//	markerParent.position = transform.position + new Vector3(-0.5f, 0, 0f);
			arrow.transform.position = transform.position + new Vector3(-.5f, 0, 0f);
			break;

		}
	}

	private void onCompleteTweenAnimationDownPulseCharacterMark (GameObject target)
	{
		iTween.ScaleTo (target, iTween.Hash ("time", 1.4f, "easetype", iTween.EaseType.easeOutQuad, "scale", Vector3.one * _customPulsingSizeFactor, "looptype", iTween.LoopType.pingPong));//"oncompletetarget", this.gameObject, "oncompleteparams", target, "oncomplete", "onCompleteTweenAnimationPulseCharacterMark"));
	}
	
	private void onCompleteTweenAnimationPulseCharacterMark (GameObject target)
	{
		iTween.ScaleTo ( target, iTween.Hash ( "time", 1.4f, "easetype", iTween.EaseType.easeOutQuad, "scale", Vector3.one, "looptype", iTween.LoopType.pingPong));//"oncompletetarget", this.gameObject, "oncompleteparams", target, "oncomplete", "onCompleteTweenAnimationDownPulseCharacterMark"));
	}

}
