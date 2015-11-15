using UnityEngine;
using System.Collections;

public class MNSuccessScreenControl : MonoBehaviour 
{
	//*************************************************************//	
	private TextMesh _text01;
	private TextMesh _text02;
	private TextMesh _text03;
	private GameObject icon01;
	private GameObject icon02;
	private GameObject icon03;
	private GameObject textX1;
	private GameObject textX2;
	private GameObject textX3;
	private GameObject textX1Clone;
	private GameObject textX2Clone;
	private GameObject textX3Clone;
	private GameObject textIcon01;
	private GameObject textIcon02;
	private GameObject textIcon03;
	private GameObject textIconClone01;
	private GameObject textIconClone02;
	private GameObject textIconClone03;
	private GameObject myButton;
	private float _levelTime;
	//*************************************************************//	
	private static MNSuccessScreenControl _meInstance;
	public static MNSuccessScreenControl getInstance ()
	{
		if ( _meInstance == null )
		{
			_meInstance = GameObject.Find ( "CameraResoultScreen" ).transform.Find ( "SuccessScreen" ).GetComponent < MNSuccessScreenControl > ();
		}
		
		return _meInstance;
	}
	//*************************************************************//

	void Update ()
	{
		_levelTime += Time.deltaTime;
	}

	public void CustomStart () 
	{
		SaveDataManager.save ( SaveDataManager.LEVEL_MINING_FINISHED_PREFIX + MNLevelControl.CURRENT_LEVEL_CLASS.myName, 1 );
		myButton = transform.Find ("buttonLab").gameObject;
		_text01 = transform.Find ( "textAmount01" ).GetComponent < TextMesh > ();
		_text02 = transform.Find ( "textAmount02" ).GetComponent < TextMesh > ();
		_text03 = transform.Find ( "textAmount03" ).GetComponent < TextMesh > ();

		icon01 = transform.Find ("icon01").gameObject;
		icon02 = transform.Find ("icon02").gameObject;
		icon03 = transform.Find ( "icon03" ).gameObject;

		textX1 = transform.Find ("textX1").gameObject;
		textX2 = transform.Find ("textX2").gameObject;
		textX3 = transform.Find ("textX3").gameObject;
		textX1Clone = textX1.transform.Find ("textX1(Clone)").gameObject;
		textX2Clone = textX2.transform.Find ("textX2(Clone)").gameObject;
		textX3Clone = textX3.transform.Find ("textX3(Clone)").gameObject;


		textIcon01 = transform.Find ("textAmount01").gameObject;
		textIcon02 = transform.Find ("textAmount02").gameObject;
		textIcon03 = transform.Find ("textAmount03").gameObject;
		textIconClone01 = textIcon01.transform.Find("textAmount01(Clone)").gameObject;
		textIconClone02 = textIcon02.transform.Find("textAmount02(Clone)").gameObject;
		textIconClone03 = textIcon03.transform.Find("textAmount03(Clone)").gameObject;

		_text01.text = GameGlobalVariables.Stats.NewResources.PLASTIC.ToString ();
		_text02.text = GameGlobalVariables.Stats.NewResources.METAL.ToString ();
		_text03.text = GameGlobalVariables.Stats.NewResources.VINES.ToString ();

		myButton.SetActive (false);
		icon01.renderer.enabled = false;
		icon02.renderer.enabled = false;
		icon03.renderer.enabled = false;
		textX1.renderer.enabled = false;
		textX2.renderer.enabled = false;
		textX3.renderer.enabled = false;
		textX1Clone.renderer.enabled = false;
		textX2Clone.renderer.enabled = false;
		textX3Clone.renderer.enabled = false;
		textIcon01.renderer.enabled = false;
		textIcon02.renderer.enabled = false;
		textIcon03.renderer.enabled = false;
		textIconClone01.renderer.enabled = false;
		textIconClone02.renderer.enabled = false;
		textIconClone03.renderer.enabled = false;

		StartCoroutine("startAnimationSequence");
	}

	private IEnumerator startAnimationSequence ()
	{
		yield return new WaitForSeconds (0f);
		SoundManager.getInstance ().silenceMusicTillNewScene();
		SoundManager.getInstance ().playSound (SoundManager.MISSION_COMPLETE);
		SoundManager.getInstance ().playSound ( SoundManager.STAR_01, -1, true );
		//SoundManager.getInstance ().playSound (SoundManager.BUM, -1, true );
		icon01.renderer.enabled = true;
		iTween.ScaleFrom ( icon01, iTween.Hash ( "time", 0.3f, "easetype", iTween.EaseType.easeOutElastic, "scale", icon01.transform.localScale * 3f ));
		textX1.renderer.enabled = true;
		textX1Clone.renderer.enabled = true;
		textIcon01.renderer.enabled = true;
		textIconClone01.renderer.enabled = true;
		//SoundManager.getInstance ().playSound (SoundManager.BUM, -1, true );
		iTween.ScaleFrom ( textIcon01, iTween.Hash ( "time", 0.3f, "easetype", iTween.EaseType.easeOutElastic, "scale", textIcon01.transform.localScale * 3f ));

		yield return new WaitForSeconds (.60f);
		SoundManager.getInstance ().playSound ( SoundManager.STAR_02, -1, true );
		//SoundManager.getInstance ().playSound (SoundManager.BUM, -1, true );
		icon02.renderer.enabled = true;
		iTween.ScaleFrom ( icon02, iTween.Hash ( "time", 0.3f, "easetype", iTween.EaseType.easeOutElastic, "scale", icon02.transform.localScale * 3f ));
		textX2.renderer.enabled = true;
		textX2Clone.renderer.enabled = true;
		textIcon02.renderer.enabled = true;
		textIconClone02.renderer.enabled = true;
		//SoundManager.getInstance ().playSound (SoundManager.BUM, -1, true );
		iTween.ScaleFrom ( textIcon02, iTween.Hash ( "time", 0.3f, "easetype", iTween.EaseType.easeOutElastic, "scale", textIcon02.transform.localScale * 3f ));

		yield return new WaitForSeconds (.60f);
		SoundManager.getInstance ().playSound ( SoundManager.STAR_03, -1, true );
		icon03.renderer.enabled = true;
		iTween.ScaleFrom ( icon03, iTween.Hash ( "time", 0.3f, "easetype", iTween.EaseType.easeOutElastic, "scale", icon03.transform.localScale * 3f ));
		textX3.renderer.enabled = true;
		textX3Clone.renderer.enabled = true;
		textIcon03.renderer.enabled = true;
		textIconClone03.renderer.enabled = true;
		//SoundManager.getInstance ().playSound (SoundManager.BUM, -1, true );
		iTween.ScaleFrom ( textIcon03, iTween.Hash ( "time", 0.3f, "easetype", iTween.EaseType.easeOutElastic, "scale", textIcon03.transform.localScale * 3f ));

		//*************************************************************//
		yield return new WaitForSeconds ( 0.5f );
		SoundManager.getInstance ().playSound ( SoundManager.BUTTONS_APAERE, -1, true );
		myButton.SetActive (true);
		iTween.ScaleFrom ( myButton, iTween.Hash ( "time", 0.3f, "easetype", iTween.EaseType.easeOutElastic, "scale", myButton.transform.localScale * 3f ));

		//iTween.ScaleFrom ( transform.Find ( "buttonRestart" ).gameObject, iTween.Hash ( "time", 0.3f, "easetype", iTween.EaseType.easeOutElastic, "scale", transform.Find ( "buttonRestart" ).localScale * 3f ));
		//iTween.ScaleFrom ( transform.Find ( "buttonLab" ).gameObject, iTween.Hash ( "time", 0.3f, "easetype", iTween.EaseType.easeOutElastic, "scale", transform.Find ( "buttonLab" ).localScale * 3f ));

	}

	public float getLevelTime ()
	{
		return _levelTime;
	}
}
