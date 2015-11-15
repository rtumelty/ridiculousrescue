using UnityEngine;
using System.Collections;

public class BusyWarningComponent : MonoBehaviour 
{
	private Material myMat;
	private Material myShadowMat;
	private float alpha = 0;
	private bool fadeOut = false;
	private bool fadeIn = false;
	void Start () 
	{
		myMat = GetComponent<MeshRenderer> ().material;
		myShadowMat = transform.Find ("dropShadow").GetComponent<MeshRenderer> ().material;
		myMat.color = new Color(myMat.color.r, myMat.color.g, myMat.color.b, alpha);
		myShadowMat.color = new Color(myShadowMat.color.r, myShadowMat.color.g, myShadowMat.color.b, alpha);
		StartCoroutine ("FadeOut");

		Destroy (this.gameObject,2.0f);
	}
	
	void Update ()
	{
		if(fadeIn == false)
		{
			alpha += 1f * Time.deltaTime * 4;
			myMat.color = new Color(myMat.color.r, myMat.color.g, myMat.color.b, alpha);
			myShadowMat.color = new Color(myShadowMat.color.r, myShadowMat.color.g, myShadowMat.color.b, alpha);
			if(alpha >= 1)
			{
				fadeIn = true;
			}
		}
		
		if(fadeOut == true)
		{
			alpha -= 1f * Time.deltaTime * 12;
			myMat.color = new Color(myMat.color.r, myMat.color.g, myMat.color.b, alpha);
			myShadowMat.color = new Color(myShadowMat.color.r, myShadowMat.color.g, myShadowMat.color.b, 0);
		}
	}
	
	IEnumerator FadeOut ()
	{
		yield return new WaitForSeconds(1.2f);
		fadeOut = true;
	}
}
