using UnityEngine;
using System.Collections;

public class TRBozControl : MonoBehaviour 
{
	//*************************************************************//
	public const int IDLE_ANIMATION = 0;
	public const int DRILLING_ANIMATION = 1;
	//*************************************************************//
	public Texture2D idleTexture;
	public Texture2D[] drillingAnimation;
	//*************************************************************//
	private float _countTimeToNextFrame = 0f;
	private int _frameID = 0;
	private int _animationID;
	private Material _myMaterial;
	private float _countTimeToEndDrillingAnimation = 0f;
	private bool _countTimeToTurnOffAnimation;
	public bool stillClicking = false;
	private bool loopActive = false;
	public SkeletonAnimation myAnimations;
	//*************************************************************//	
	private static TRBozControl _meInstance;
	public static TRBozControl getInstance ()
	{
		return _meInstance;
	}
	//*************************************************************//
	void Awake () 
	{
		TRBozControl._meInstance = this;
		_myMaterial = renderer.material;
		myAnimations = GetComponent < SkeletonAnimation > ();
	}

	void Start ()
	{
		if(TRLevelControl.LEVEL_ID < 2)
		{
			this.gameObject.SetActive(false);
			//transform.GetComponent < AudioSource > ().enabled = false;
		}
		myAnimations.animationName = "standing";
	}
	
	/*void Update () 
	{
		_countTimeToNextFrame += Time.deltaTime;
		if ( _countTimeToNextFrame >= ( 1f / 24f ))
		{
			_countTimeToNextFrame = 0f;

			switch ( _animationID )
			{
				case DRILLING_ANIMATION:
					_myMaterial.mainTexture = drillingAnimation[_frameID];
					_frameID++;
					if ( _frameID >= drillingAnimation.Length ) _frameID = 0;
				    break;
				case IDLE_ANIMATION:
					_myMaterial.mainTexture = idleTexture;
					break;
			}
		}

		if ( _countTimeToTurnOffAnimation )
		{
			_countTimeToEndDrillingAnimation -= Time.deltaTime;
			
			if ( _countTimeToEndDrillingAnimation <= 0f )
			{
				_animationID = IDLE_ANIMATION;
			}
		}
	}

	private IEnumerator PlayLoop ()
	{
		yield return new WaitForSeconds (0f);
		loopActive = true;
		SoundManager.getInstance().playSound(SoundManager.BOZ_DRILL);
		yield return new WaitForSeconds (1.017f);
		if(stillClicking == false)
		{
			SoundManager.getInstance().playSound(SoundManager.BOZ_FAIL);
		}
		else
		{
			loopActive = false;
			stillClicking = false;
			StartCoroutine("Playloop");
		}
		loopActive = false;
		stillClicking = false;



	}

	public void drillOnOff ( bool isOn )
	{
		if ( isOn )
		{
			_countTimeToTurnOffAnimation = false;
			_animationID = DRILLING_ANIMATION;

			if(loopActive == false)
			{
				StartCoroutine("PlayLoop");
			}
			else
			{
				stillClicking = true;
			}
		}
		else
		{
			stillClicking = false;
			_countTimeToTurnOffAnimation = true;
			_countTimeToEndDrillingAnimation = 0.3f;
		}
	}*/
	void Update () 
	{

	}

	private IEnumerator PlayLoop ()
	{
		yield return new WaitForSeconds (0f);
		loopActive = true;
		SoundManager.getInstance().playSound(SoundManager.BOZ_DRILL);
		yield return new WaitForSeconds (1.017f);
		if(stillClicking == false)
		{
			SoundManager.getInstance().playSound(SoundManager.BOZ_FAIL);
		}
		else
		{
			loopActive = false;
			stillClicking = false;
			StartCoroutine("Playloop");
		}
		loopActive = false;
		stillClicking = false;



	}

	public void drillOnOff ( bool isOn )
	{
		if ( isOn )
		{
			GetComponent < SkeletonAnimation > ().animationName = "drill";

			if(loopActive == false)
			{
				StartCoroutine("PlayLoop");
			}
			else
			{
				stillClicking = true;
			}
		}
		else
		{
			stillClicking = false;
			StartCoroutine("stopDrill");
		}
	}
	public IEnumerator stopDrill ()
	{
		yield return new WaitForSeconds (0.5f);
		if(!stillClicking)
		{
			GetComponent < SkeletonAnimation > ().animationName = "standing";
		}
	}
}
