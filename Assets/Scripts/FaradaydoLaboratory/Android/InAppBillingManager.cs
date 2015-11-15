using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Prime31;

public class InAppBillingManager : MonoBehaviour 
{
	/*
	#if UNITY_ANDROID
	//************************************************************* //
	public delegate void PurchaseCallBack ( bool success, bool timeOut = false );
	//************************************************************* //
	private const float _TIME_OUT_TIME = 10f;
	//************************************************************* //
	private PurchaseCallBack _currentCallbackOnFinishedTransaction;
	private string _currentProductToBeBought;
	private float _countTimeoutTime = _TIME_OUT_TIME;
	private bool _transactionInProgress;
	private GameObject _noInternetConnectionPopup;
	//************************************************************* //
	private static InAppBillingManager _meInstance;
	public static InAppBillingManager getInstance ()
	{
		if ( _meInstance == null )
		{
			_meInstance = GameObject.Find ( "__IN_APP_BILLING_MANAGER" ).GetComponent < InAppBillingManager > ();
		}
		
		return _meInstance;
	}
	//************************************************************* //
	public void initPurchase ( string product, PurchaseCallBack callbackOnFinishedTransaction )
	{
		string key = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAhbAq656j6rUzE2X4fB/KA9/KxKnAnQM9fmucAvzRKMm6wabOJhSXOK7a7f3qj4ScjlZV8ruIjwylHL/vjfN30i1YHRwKy7oYNZvRp294xqlPxqO8zYyPM2GJHURvST4bzRjzGMkLArBH80I24na6wCkrbnJgt4YZTNiSUjsDnSgCfgf8z3jdAQP/AC+PfiP87dVrC9KmjXgTBgcQBpEruAZ9r3sx5etUMpNQV/nqxsME27ak+QFoyVC91L35+Hck6qjwrNBsTaSg4FsLs+7qGrQq+CGdJRseWG7EhIG8NROWIfe7F3ICh48I6uPDD7dfZBSPwfX7OWvfvcgBBn6r1wIDAQAB";
		_currentCallbackOnFinishedTransaction = callbackOnFinishedTransaction;
		_currentProductToBeBought = product;
		GoogleIAB.init ( key );
		
		GoogleAnalytics.instance.LogScreen ( "Trasaction starts - product " + _currentProductToBeBought + " | Around - level " + LevelControl.SELECTED_LEVEL_NAME );
		
		_transactionInProgress = true;
		_countTimeoutTime = _TIME_OUT_TIME;
		
#if UNITY_EDITOR
		_currentCallbackOnFinishedTransaction ( true );
#endif
	}
	
	void Update ()
	{
		if ( _transactionInProgress )
		{
			_countTimeoutTime -= Time.deltaTime;
			
			if ( _countTimeoutTime <= 0f )
			{
				if ( _currentCallbackOnFinishedTransaction != null ) _currentCallbackOnFinishedTransaction ( false, true );
				_transactionInProgress = false;
				_countTimeoutTime = _TIME_OUT_TIME;
			}
		}
	}
	
	void OnEnable()
	{
		GoogleIABManager.billingSupportedEvent += billingSupportedEvent;
		GoogleIABManager.billingNotSupportedEvent += billingNotSupportedEvent;
		//GoogleIABManager.queryInventorySucceededEvent += queryInventorySucceededEvent;
		GoogleIABManager.queryInventoryFailedEvent += queryInventoryFailedEvent;
		GoogleIABManager.purchaseCompleteAwaitingVerificationEvent += purchaseCompleteAwaitingVerificationEvent;
		GoogleIABManager.purchaseSucceededEvent += purchaseSucceededEvent;
		GoogleIABManager.purchaseFailedEvent += purchaseFailedEvent;
		GoogleIABManager.consumePurchaseSucceededEvent += consumePurchaseSucceededEvent;
		GoogleIABManager.consumePurchaseFailedEvent += consumePurchaseFailedEvent;
	}

	void OnDisable()
	{
		GoogleIABManager.billingSupportedEvent -= billingSupportedEvent;
		GoogleIABManager.billingNotSupportedEvent -= billingNotSupportedEvent;
		//GoogleIABManager.queryInventorySucceededEvent -= queryInventorySucceededEvent;
		GoogleIABManager.queryInventoryFailedEvent -= queryInventoryFailedEvent;
		GoogleIABManager.purchaseCompleteAwaitingVerificationEvent -= purchaseCompleteAwaitingVerificationEvent;
		GoogleIABManager.purchaseSucceededEvent -= purchaseSucceededEvent;
		GoogleIABManager.purchaseFailedEvent -= purchaseFailedEvent;
		GoogleIABManager.consumePurchaseSucceededEvent -= consumePurchaseSucceededEvent;
		GoogleIABManager.consumePurchaseFailedEvent -= consumePurchaseFailedEvent;
	}

	void billingSupportedEvent()
	{
		ConsoleManager.getInstance ().addToExternalString ( "billingSupportedEvent" );
		
		string[] skus = new string[] { _currentProductToBeBought };
		//GoogleIAB.queryInventory ( skus );		
	}

	void billingNotSupportedEvent ( string error )
	{
		ConsoleManager.getInstance ().addToExternalString ( "billingNotSupportedEvent: " + error );
		_currentCallbackOnFinishedTransaction ( false );

		if ( error.Contains ( "3:Billing" ))
		{
			GameObject loginToGoogleAccountPopupScreen = ( GameObject ) Resources.Load ( "UI/Laboratory/noAccountUIScreen" );
			
			if ( FLUIControl.currentBlackOutUI )
			{
				FLUIControl.currentBlackOutUI.AddComponent < AlphaDisapearAndDestory > ();
				Destroy ( FLUIControl.currentBlackOutUI.GetComponent < BoxCollider > ());
				FLUIControl.getInstance ().putHeigherOrLowerCurrencyPanel ( false );
			}
			
			FLGlobalVariables.POPUP_UI_SCREEN = false;
			
			FLUIControl.getInstance ().createPopup ( loginToGoogleAccountPopupScreen, true );

			Application.OpenURL ( "market://" );
 		}

		GoogleAnalytics.instance.LogScreen ( "Trasaction failed - product " + _currentProductToBeBought );
		
		_transactionInProgress = false;
		_countTimeoutTime = _TIME_OUT_TIME;


	}

	void queryInventorySucceededEvent( List<GooglePurchase> purchases, List<GoogleSkuInfo> skus )
	{
		ConsoleManager.getInstance ().addToExternalString ( string.Format( "queryInventorySucceededEvent. total purchases: {0}, total skus: {1}", purchases.Count, skus.Count ));
		Prime31.Utils.logObject( purchases );
		Prime31.Utils.logObject( skus );
	}

	void queryInventoryFailedEvent( string error )
	{
		ConsoleManager.getInstance ().addToExternalString ( "queryInventoryFailedEvent: " + error );

		if ( error.Contains ( "6:Error" ))
		{
			_noInternetConnectionPopup = ( GameObject ) Resources.Load ( "UI/Laboratory/noInternetUIScreen" );

			if ( FLUIControl.currentBlackOutUI )
			{
				FLUIControl.currentBlackOutUI.AddComponent < AlphaDisapearAndDestory > ();
				Destroy ( FLUIControl.currentBlackOutUI.GetComponent < BoxCollider > ());
				FLUIControl.getInstance ().putHeigherOrLowerCurrencyPanel ( false );
			}

			FLGlobalVariables.POPUP_UI_SCREEN = false;
			
			FLUIControl.getInstance ().createPopup ( _noInternetConnectionPopup, true );
		}

		_currentCallbackOnFinishedTransaction ( false );

		Application.OpenURL ( "market://" );

		GoogleAnalytics.instance.LogScreen ( "Trasaction failed - product " + _currentProductToBeBought );
		
		_transactionInProgress = false;
		_countTimeoutTime = _TIME_OUT_TIME;
	}

	void purchaseCompleteAwaitingVerificationEvent( string purchaseData, string signature )
	{
		ConsoleManager.getInstance ().addToExternalString ( "purchaseCompleteAwaitingVerificationEvent. purchaseData: " + purchaseData + ", signature: " + signature );
	}

	void purchaseSucceededEvent( GooglePurchase purchase )
	{
		ConsoleManager.getInstance ().addToExternalString ( "purchaseSucceededEvent: " + purchase );
		//GoogleIAB.consumeProduct ( _currentProductToBeBought );
	}

	void purchaseFailedEvent( string error )
	{
		ConsoleManager.getInstance ().addToExternalString ( "purchaseFailedEvent: " + error );
		_currentCallbackOnFinishedTransaction ( false );

		Application.OpenURL ( "market://" );
		
		GoogleAnalytics.instance.LogScreen ( "Trasaction failed - product " + _currentProductToBeBought );
		
		_transactionInProgress = false;
		_countTimeoutTime = _TIME_OUT_TIME;
	}

	void consumePurchaseSucceededEvent( GooglePurchase purchase )
	{
		ConsoleManager.getInstance ().addToExternalString ( "consumePurchaseSucceededEvent: " + purchase );
		_currentCallbackOnFinishedTransaction ( true );
		
		GoogleAnalytics.instance.LogScreen ( "Trasaction success - product " + _currentProductToBeBought );
		
		_transactionInProgress = false;
		_countTimeoutTime = _TIME_OUT_TIME;
	}

	void consumePurchaseFailedEvent( string error )
	{
		ConsoleManager.getInstance ().addToExternalString ( "consumePurchaseFailedEvent: " + error );
		_currentCallbackOnFinishedTransaction ( false );

		Application.OpenURL ( "market://" );
		
		GoogleAnalytics.instance.LogScreen ( "Trasaction failed - product " + _currentProductToBeBought );
		
		_transactionInProgress = false;
		_countTimeoutTime = _TIME_OUT_TIME;	
	}
#endif
*/
}