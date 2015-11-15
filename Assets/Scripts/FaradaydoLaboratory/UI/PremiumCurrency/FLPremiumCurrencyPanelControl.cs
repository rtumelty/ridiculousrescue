using UnityEngine;
using System.Collections;

public class FLPremiumCurrencyPanelControl : MonoBehaviour 
{
	//*************************************************************//
	private TextMesh _amountText;
	private int _previousAmount = -1;
	private GameObject _nodeFxParticles;
	private GameObject _nodeFxParticlesInstant;

	private Vector3 _initialScale;
	//*************************************************************//
	void Awake ()
	{
		_nodeFxParticles = ( GameObject ) Resources.Load ( "Particles/SparkleRising" );
		_amountText = transform.Find ( "textAmount" ).GetComponent < TextMesh > ();
		_previousAmount = GameGlobalVariables.Stats.PREMIUM_CURRENCY;

		_initialScale = VectorTools.cloneVector3 ( transform.localScale );
	}

	void Start ()
	{
		_previousAmount = GameGlobalVariables.Stats.PREMIUM_CURRENCY;
	}
	
	void Update ()
	{
		_amountText.text = GameGlobalVariables.Stats.PREMIUM_CURRENCY.ToString ();
		if ( _previousAmount < GameGlobalVariables.Stats.PREMIUM_CURRENCY )
		{
			highlightCurrency ( true );
			_previousAmount = GameGlobalVariables.Stats.PREMIUM_CURRENCY;
		}
		else if ( _previousAmount > GameGlobalVariables.Stats.PREMIUM_CURRENCY )
		{
			highlightCurrency ( false );
			_previousAmount = GameGlobalVariables.Stats.PREMIUM_CURRENCY;
		}
	}
	
	public void highlightCurrency ( bool goingUp )
	{
		if ( goingUp )
		{
			if ( _nodeFxParticlesInstant ) Destroy ( _nodeFxParticlesInstant );
			iTween.Stop ( this.gameObject );

			transform.localScale = VectorTools.cloneVector3 ( _initialScale );

			SoundManager.getInstance ().playSound ( SoundManager.LEVEL_NODE_UNLOCKED );
			_nodeFxParticlesInstant = ( GameObject ) Instantiate ( _nodeFxParticles, transform.position + Vector3.up, _nodeFxParticles.transform.rotation ); 
			iTween.ScaleFrom ( this.gameObject, iTween.Hash ( "time", 1.5f, "easetype", iTween.EaseType.easeOutElastic, "scale", transform.localScale * 2f, "oncomplete", "finishedBumpCurrencyPanel" ));
		}
		else
		{
			iTween.ScaleFrom ( _amountText.gameObject, iTween.Hash ( "time", 1.0f, "easetype", iTween.EaseType.easeOutElastic, "scale", _amountText.transform.localScale * 2f ));
		}
	}

	private void finishedBumpCurrencyPanel ()
	{
		iTween.ScaleFrom ( _amountText.gameObject, iTween.Hash ( "time", 1.0f, "easetype", iTween.EaseType.easeOutElastic, "scale", _amountText.transform.localScale * 2f ));
		Destroy ( _nodeFxParticlesInstant );
	}
}
