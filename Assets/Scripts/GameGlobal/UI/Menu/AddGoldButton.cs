using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class AddGoldButton : MonoBehaviour 
{
	#if UNITY_ANDROID
	private static Dictionary<string, int> values;
	[SerializeField] private int quantityOfGoldPurchased = 0;
	[SerializeField] private string storeProductId = "";
	
	private GameObject _noInternetConnectionPopup;

	private static bool _transactionInProgress = false;

	private static bool transactionActive = false;
	private static bool billingSupported = false;
	private static bool listenersAdded = false;

	private const float _TIME_OUT_TIME = 10f;

	void Awake() {
		if (values == null)
			values = new Dictionary<string, int> ();

		if (!values.ContainsKey(storeProductId))
			values.Add (storeProductId, quantityOfGoldPurchased);

		ConsoleManager.getInstance ().addToExternalString ("Adding product: " + storeProductId);
		AndroidInAppPurchaseManager.instance.addProduct (storeProductId);
	}

	void Start() {
		foreach (KeyValuePair<string, int> pair in values) {
			Debug.LogError(pair.Key + " " + pair.Value);
		}

		if (!listenersAdded) {
			//listening for purchase and consume events
			ConsoleManager.getInstance ().addToExternalString ("Adding event listeners");
//			AndroidInAppPurchaseManager.instance.addEventListener (AndroidInAppPurchaseManager.ON_PRODUCT_PURCHASED, OnProductPurchased);
//			AndroidInAppPurchaseManager.instance.addEventListener (AndroidInAppPurchaseManager.ON_PRODUCT_CONSUMED, OnProductConsumed);

			//initilaizing store
//			AndroidInAppPurchaseManager.instance.addEventListener (AndroidInAppPurchaseManager.ON_BILLING_SETUP_FINISHED, OnBillingConnected);
			
			ConsoleManager.getInstance ().addToExternalString ("Loading store");
			//you may use loadStore function without parametr if you have filled base64EncodedPublicKey in plugin settings
			AndroidInAppPurchaseManager.instance.loadStore ();

			listenersAdded = true;
		}		
	}

	void OnMouseUp ()
	{
		if ( FLGlobalVariables.TUTORIAL_MENU || FLMissionRoomManager.AFTER_INTRO ) return;
		SoundManager.getInstance ().playSound ( SoundManager.CANCEL_BUTTON );
		
		handleTouched ();
	}
	
	private void handleTouched ()
	{
		SoundManager.getInstance ().playSound (SoundManager.CONFIRM_BUTTON);

		//if (billingSupported) {
				StartCoroutine (StartTransaction ());
				transactionActive = true;  	
		/*} else {
			try {
				ConsoleManager.getInstance ().addToExternalString ("In App Billing not supported / uninitialized");
			} catch {}
		}*/
	}

	private IEnumerator StartTransaction() {
		_transactionInProgress = true;
		float transactionTime = 0f;
		
		AndroidInAppPurchaseManager.instance.purchase (storeProductId);
		try {
			ConsoleManager.getInstance ().addToExternalString ("Transaction sent: productId = " + storeProductId);
		} catch {}

		while (_transactionInProgress && transactionTime < _TIME_OUT_TIME) {
			transactionTime += Time.deltaTime;
			yield return new WaitForSeconds(Time.deltaTime);
		}

		if (transactionTime >= _TIME_OUT_TIME) {
			//TIMED OUT
			try {
				ConsoleManager.getInstance ().addToExternalString ("Transaction timed out."); 
			} catch {}
		}
	}

	private static void OnProcessingPurchasedProduct(GooglePurchaseTemplate purchase) {
		AndroidInAppPurchaseManager.instance.consume (purchase.SKU);
	}
	
	private static void OnProcessingConsumeProduct(GooglePurchaseTemplate purchase) {
		int addedGold = 0;
		if (values.TryGetValue (purchase.SKU, out addedGold)) {
			GameGlobalVariables.Stats.METAL_IN_CONTAINERS += addedGold;
			SaveDataManager.save (SaveDataManager.METAL_IN_CONTAINERS, GameGlobalVariables.Stats.METAL_IN_CONTAINERS);
		}
	}

	public static void OnProductPurchased(BillingResult result) {
		
		//this flag will tell you if purchase is avaliable
		//result.isSuccess
		
		
		//infomation about purchase stored here
		//result.purchase
		
		//here is how for example you can get product SKU
		//result.purchase.SKU
		
		
		if(result.isSuccess) {
			_transactionInProgress = false;
			try {
				ConsoleManager.getInstance ().addToExternalString ( "Purchase suceeded: " + result.response.ToString() + " " + result.message );
			} catch {}
			OnProcessingPurchasedProduct (result.purchase);
		} else {
			try {
				ConsoleManager.getInstance ().addToExternalString ( "Purchase failed: " + result.response.ToString() + " " + result.message );
			} catch {}
			
			_transactionInProgress = false;
			
			Application.OpenURL ( "market://" );
			
			GoogleAnalytics.instance.LogScreen ( "Trasaction failed - product " + result.purchase.SKU );
		}
	}
	
	
	public static void OnProductConsumed(BillingResult result) {
		
		if(result.isSuccess) {
			try {
				ConsoleManager.getInstance ().addToExternalString ( "Consume succeeded: " + result.response.ToString() + " " + result.message );
			} catch {}
			OnProcessingConsumeProduct (result.purchase);
		} else {
			try {
				ConsoleManager.getInstance ().addToExternalString ( "Consume product failed: " + result.response.ToString() + " " + result.message );
			} catch {}
			
			_transactionInProgress = false;	
			
			Application.OpenURL ( "market://" );
			
			GoogleAnalytics.instance.LogScreen ( "Trasaction failed - product " + result.purchase.SKU );
		}
		
		Debug.Log ("Consume Response: " + result.response.ToString() + " " + result.message);
	}
	
	
	public static void OnBillingConnected(BillingResult result) {
		
		ConsoleManager.getInstance ().addToExternalString ("Billing connection result");

//		AndroidInAppPurchaseManager.instance.removeEventListener (AndroidInAppPurchaseManager.ON_BILLING_SETUP_FINISHED, OnBillingConnected);
		
		
		if (result.isSuccess) {
			//Store connection is Successful. Next we loading product and customer purchasing details
			//AndroidInAppPurchaseManager.instance.addEventListener (AndroidInAppPurchaseManager.ON_RETRIEVE_PRODUC_FINISHED, OnRetriveProductsFinised);
			AndroidInAppPurchaseManager.instance.retrieveProducDetails ();

			billingSupported = true;
			try {
				ConsoleManager.getInstance ().addToExternalString ( "Billing supported." );
			} catch {}

	
		} else {
			billingSupported = false;
			
			try {
				ConsoleManager.getInstance ().addToExternalString ( "Billing not supported: " + result.response.ToString() + " " + result.message );
			} catch {}
			
			_transactionInProgress = false;
			
			if ( (result.response.ToString() + " " + result.message).Contains ( "3:Billing" ))
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
			
			GoogleAnalytics.instance.LogScreen ( "Transaction failed");

		}
	}
	
	
	
	
	public static void OnRetriveProductsFinised(BillingResult result) {
//		AndroidInAppPurchaseManager.instance.removeEventListener (AndroidInAppPurchaseManager.ON_RETRIEVE_PRODUC_FINISHED, OnRetriveProductsFinised);
		
		if(result.isSuccess) {
			
			UpdateStoreData();
			
			try {
				ConsoleManager.getInstance ().addToExternalString ( "Product list received successfully:" + result.response.ToString() + " " + result.message);
			} catch {}
			
			
		} else {
			try {
				ConsoleManager.getInstance ().addToExternalString ( "Product list retrieval failed: " + result.response.ToString() + " " + result.message );
			} catch {}
		}
		
	}
	
	
	
	private static void UpdateStoreData() {
		//chisking if we already own some consuamble product but forget to consume those
		try {
			ConsoleManager.getInstance ().addToExternalString ( "Consuming any unused products" );
		} catch {}

		foreach (KeyValuePair<string, int> pair in values) {
			if (AndroidInAppPurchaseManager.instance.inventory.IsProductPurchased (pair.Key)) {
				AndroidInAppPurchaseManager.instance.consume (pair.Key);
			}
		}
	}

#endif
}
