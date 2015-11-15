using UnityEngine;
using System.Collections;

public class TestAnim : MonoBehaviour {
	
	void Start () 
	{
		//InvokeRepeating ("callAnim", 0, 5);
		//transform.Find ("side").gameObject.SetActive(false);
		callAnim ();
	}

	void callAnim()
	{
		GetComponent < CharacterAnimationControl > ().playAnimation ( CharacterAnimationControl.IDLE_01_ANIMATION );
		//print ("called");
	}
}
