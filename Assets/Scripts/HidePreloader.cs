using UnityEngine;
using System.Collections;

public class HidePreloader : MonoBehaviour {

	// Use this for initialization
	void Start () {
		AndroidNativeUtility.HidePreloader ();
	}
}
