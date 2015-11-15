using UnityEngine;
using System.Collections;

public class MovesPanelComponent : MonoBehaviour 
{
	private Material myMat;
	private Material myShadowMat;
	private float alpha = 0;
	private bool fadeOut = false;
	private bool fadeIn = false;
	void Start () 
	{
		if(gameObject.name == "scorePanel(Clone)")
		{
			StartCoroutine("StopAnim");
		}
		myMat = GetComponent<MeshRenderer> ().material;
		myShadowMat = transform.Find ("dropShadow").GetComponent<MeshRenderer> ().material;
		myMat.color = new Color(myMat.color.r, myMat.color.g, myMat.color.b, alpha);
		myShadowMat.color = new Color(myShadowMat.color.r, myShadowMat.color.g, myShadowMat.color.b, alpha);
		StartCoroutine ("FadeOut");
		transform.Find ("dropShadow").GetComponent<TextMesh> ().text = GetComponent<TextMesh> ().text;
		SoundManager.getInstance ().playSound (SoundManager.LEVEL_NODE_UNLOCKED);
		switch(GameGlobalVariables.Stats.MOVES)
		{
		case 3:
			GetComponent<GameTextControl>().myKey = "ui_sign_3moves";
			transform.Find ("dropShadow").GetComponent<GameTextControl>().myKey = "ui_sign_3moves";
			break;
		case 2:
			GetComponent<GameTextControl>().myKey = "ui_sign_2moves";
			transform.Find ("dropShadow").GetComponent<GameTextControl>().myKey = "ui_sign_2moves";
			break;
		case 1:
			GetComponent<GameTextControl>().myKey = "ui_sign_1moves";
			transform.Find ("dropShadow").GetComponent<GameTextControl>().myKey = "ui_sign_1moves";
			break;
		case 0:
			GetComponent<GameTextControl>().myKey = "ui_sign_0moves";
			transform.Find ("dropShadow").GetComponent<GameTextControl>().myKey = "ui_sign_0moves";
			break;
		}
		Destroy (this.gameObject,2.7f);
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
			alpha -= 1f * Time.deltaTime * 10;
			myMat.color = new Color(myMat.color.r, myMat.color.g, myMat.color.b, alpha);
			myShadowMat.color = new Color(myShadowMat.color.r, myShadowMat.color.g, myShadowMat.color.b, 0);
		}
	}

	IEnumerator FadeOut ()
	{
		yield return new WaitForSeconds(1.2f);
		fadeOut = true;
	}

	IEnumerator StopAnim ()
	{
		yield return new WaitForSeconds(0.6f);
		//GetComponent<Animation>().animation["warningTextAnim"].speed = 0;
		GetComponent<Animation>().animation.Stop();
	}
}
