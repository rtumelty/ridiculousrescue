using UnityEngine;
using System.Collections;

public class TREndAnimController : MonoBehaviour {
	
	public GameObject CoraFront;
	public GameObject FaraFront;
	public GameObject BozFront;
	public GameObject JoseFront;
	public GameObject MadraSide;
	public GameObject CoraSide;
	public GameObject FaraSide;
	public GameObject BozSide;
	public GameObject JoseSide;
	private static TREndAnimController _meInstance;
	public static TREndAnimController getInstance ()
	{
		return _meInstance;
	}
	void Awake ()
	{
		_meInstance = this;
	}
	void Start () 
	{
		if(transform.Find ("Madra2") != null)
		{
			MadraSide = transform.Find ("Madra2").gameObject;
		}
		if(transform.Find ("part01") != null)
		{
			if(transform.Find ("part01").transform.Find ("jose") != null)
			{
				JoseSide = transform.Find ("part01").transform.Find ("jose").gameObject;
			}
			if(transform.Find ("part01").transform.Find ("Boz") != null)
			{
				BozSide = transform.Find ("part01").transform.Find ("Boz").transform.Find ("tile").transform.Find ("side").gameObject;
				if(TRLevelControl.LEVEL_ID < 2)
				{
					this.gameObject.SetActive(false);
				}
			}
		}
		if(transform.Find ("part02") != null)
		{
			if(transform.Find ("part02").transform.Find ("Cora") != null)
			{
				CoraFront = transform.Find ("part02").transform.Find ("Cora").gameObject;
			}
			if(transform.Find ("part02").transform.Find ("Cora").transform.Find ("CoraSide") != null)
			{
				CoraSide = transform.Find ("part02").transform.Find ("Cora").transform.Find ("CoraSide").gameObject;
				CoraSide.renderer.enabled = false;
			}
		}
		if(transform.Find ("part03") != null)
		{
			if(transform.Find ("part03").transform.Find ("Faradaydo") != null)
			{
				FaraFront = transform.Find ("part03").transform.Find ("Faradaydo").gameObject;
			}
			if(transform.Find ("part03").transform.Find ("Faradaydo").transform.Find ("FaraSide") != null)
			{
				FaraSide = transform.Find ("part03").transform.Find ("Faradaydo").transform.Find ("FaraSide").gameObject;
				FaraSide.renderer.enabled = false;
			}
		}
		gameObject.SetActive (false);
	}

	void Update () 
	{
		if (Input.GetKeyUp ("u")) 
		{
			GameObject.Find("train2").GetComponent < Animation >().Play("TrainFail");
			StartCoroutine("FailAnims");
		}
		if (Input.GetKeyUp ("i")) 
		{
			GameObject.Find("train2").GetComponent < Animation >().Play("TrainSuccess");
		}
	}

	public IEnumerator FailAnims ()
	{
		InvokeRepeating ("AudioDropper",0,0.16f);
		yield return new WaitForSeconds(.28f);
		Time.timeScale = .8f;
		//--------------------------
		CoraFront.renderer.enabled = false;
		CoraSide.renderer.enabled = true;
		CoraSide.GetComponent < SkeletonAnimation > ().animationName = "cora_walk";
		//--------------------------
		//>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
		FaraFront.renderer.enabled = false;
		FaraSide.renderer.enabled = true;
		FaraSide.GetComponent < SkeletonAnimation > ().animationName = "walking";
		//>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
		//++++++++++++++++++++++++++++++++++++++
		MadraSide.GetComponent < SkeletonAnimation > ().animationName = "walk4";
		//++++++++++++++++++++++++++++++++++++++
		//<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
		JoseSide.GetComponent < SkeletonAnimation > ().animationName = "run";
		//<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
		//?????????????????????????????????????????????????????????????
		if(TRLevelControl.LEVEL_ID > 1)
		{
			BozSide.GetComponent < SkeletonAnimation > ().animationName = "run";
		}
		//?????????????????????????????????????????????????????????????
		//=================================================================================
		yield return new WaitForSeconds(.54f);
		//--------------------------
		CoraSide.renderer.enabled = false;
		CoraFront.renderer.enabled = true;
		CoraFront.GetComponent < SkeletonAnimation > ().animationName = "standing";
		//--------------------------
		//>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
		FaraSide.renderer.enabled = false;
		FaraFront.renderer.enabled = true;
		FaraFront.GetComponent < SkeletonAnimation > ().animationName = "idle";
		//>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
		//++++++++++++++++++++++++++++++++++++++
		MadraSide.GetComponent < SkeletonAnimation > ().animationName = "standing";
		//++++++++++++++++++++++++++++++++++++++
		//<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
		JoseSide.GetComponent < SkeletonAnimation > ().animationName = "animation";
		//<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
		//?????????????????????????????????????????????????????????????
		if(TRLevelControl.LEVEL_ID > 1)
		{
			BozSide.GetComponent < SkeletonAnimation > ().animationName = "standing";
		}
		//?????????????????????????????????????????????????????????????
		//=================================================================================
		yield return new WaitForSeconds(.02f);
		//?????????????????????????????????????????????????????????????
		if(TRLevelControl.LEVEL_ID > 1)
		{
			BozSide.GetComponent < SkeletonAnimation > ().animationName = "bump";
		}
		//?????????????????????????????????????????????????????????????
	}
	public IEnumerator WinAnims ()
	{
		if(TRLevelControl.LEVEL_ID > 1)
		{
			BozSide.GetComponent < SkeletonAnimation > ().animationName = "standing";
		}
		//++++++++++++++++++++++++++++++++++++++
		MadraSide.GetComponent < SkeletonAnimation > ().animationName = "standing";
		//++++++++++++++++++++++++++++++++++++++
		yield return new WaitForSeconds(.28f);
		Time.timeScale = 0.5f;
		//++++++++++++++++++++++++++++++++++++++
		MadraSide.GetComponent < SkeletonAnimation > ().animationName = "walk4";
		//++++++++++++++++++++++++++++++++++++++
		yield return new WaitForSeconds(.54f);

		//++++++++++++++++++++++++++++++++++++++
		MadraSide.GetComponent < SkeletonAnimation > ().animationName = "standing";
		//++++++++++++++++++++++++++++++++++++++
		
		
		yield return new WaitForSeconds(0.02f);
		yield return new WaitForSeconds(0.5f);
		SoundManager.getInstance ().playSound (SoundManager.TRAIN_HORN_SHORT);

	}
	public void AudioDropper ()
	{
		if(SoundManager.getInstance ()._mainAudioSource.volume > 0)
		{
			SoundManager.getInstance ()._mainAudioSource.volume -= .01f;
		}
	}
}
