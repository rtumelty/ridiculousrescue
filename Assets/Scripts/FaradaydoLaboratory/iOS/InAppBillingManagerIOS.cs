using UnityEngine;
using System.Collections.Generic;
using Prime31;

public class InAppBillingManagerIOS : MonoBehaviour 
{
#if UNITY_IPHONE
	// array of product ID's from iTunesConnect.  MUST match exactly what you have there!
	private string[] _productIdentifiers = {"01seeds", "02seeds", "03seeds", "04seeds", "05seeds", "06seeds", "unlocklevel13ios"};

	//*************************************************************//
	public delegate void PurchaseCallBack ( bool success, bool timeOut = false );
	//*************************************************************//
	private List<StoreKitProduct> _products;
	private PurchaseCallBack _currentCallbackOnFinishedTransaction;
	private string _currentProductToBeBought;
	//*************************************************************//
	private static InAppBillingManagerIOS _meInstance;
	public static InAppBillingManagerIOS getInstance ()
	{
		if ( _meInstance == null )
		{
			_meInstance = GameObject.Find ( "__IN_APP_BILLING_MANAGER" ).GetComponent < InAppBillingManagerIOS > ();
		}
		
		return _meInstance;
	}
	//*************************************************************//
	void Start()
	{
		// you cannot make any purchases until you have retrieved the products from the server with the requestProductData method
		// we will store the products locally so that we will know what is purchaseable and when we can purchase the products
		StoreKitManager.productListReceivedEvent += allProducts =>
		{
			Debug.Log( "received total products: " + allProducts.Count );
			_products = allProducts;

			parseProducts();
		};

		Debug.Log ("In App Manager start");

		if ( _products == null || _products.Count == 0 )  
		{
			StoreKitBinding.requestProductData ( _productIdentifiers );
		}
	}

	public void parseProducts()
	{
		//TODO: fetch info from store kit products for purchase and display purpose
	}

	public void initPurchase ( string productName, PurchaseCallBack callbackOnFinishedTransaction )
	{
		//bool canMakePayments = StoreKitBinding.canMakePayments();

#if UNITY_EDITOR
		callbackOnFinishedTransaction ( true );
#endif

		/*
		if ( ! canMakePayments )
		{
			callbackOnFinishedTransaction ( false );
			return;
		}
		*/

		StoreKitProduct productToBuy;

		bool foundProduct = false;

		foreach ( StoreKitProduct product in _products )
		{
			Debug.Log ( product.productIdentifier );

			if ( productName == product.productIdentifier )
			{
				foundProduct = true;

				productToBuy = product;
				Debug.Log( "preparing to purchase product: " + productToBuy.productIdentifier );
				StoreKitBinding.purchaseProduct( productToBuy.productIdentifier, 1 );

				_currentCallbackOnFinishedTransaction = callbackOnFinishedTransaction;
				_currentProductToBeBought = productName;

				break;
			}
		}

		if ( ! foundProduct )
		{
			Debug.Log ( "no product found | lenght of products array: " + _products.Count );
			callbackOnFinishedTransaction ( false );
		}
	}
	
	void OnEnable()
	{
		// Listens to all the StoreKit events.  All event listeners MUST be removed before this object is disposed!
		StoreKitManager.productPurchaseAwaitingConfirmationEvent += productPurchaseAwaitingConfirmationEvent;
		StoreKitManager.purchaseSuccessfulEvent += purchaseSuccessful;
		StoreKitManager.purchaseCancelledEvent += purchaseCancelled;
		StoreKitManager.purchaseFailedEvent += purchaseFailed;
		StoreKitManager.productListReceivedEvent += productListReceivedEvent;
		StoreKitManager.productListRequestFailedEvent += productListRequestFailed;
		StoreKitManager.restoreTransactionsFailedEvent += restoreTransactionsFailed;
		StoreKitManager.restoreTransactionsFinishedEvent += restoreTransactionsFinished;
		StoreKitManager.paymentQueueUpdatedDownloadsEvent += paymentQueueUpdatedDownloadsEvent;
	}
	
	void OnDisable()
	{
		// Remove all the event handlers
		StoreKitManager.productPurchaseAwaitingConfirmationEvent -= productPurchaseAwaitingConfirmationEvent;
		StoreKitManager.purchaseSuccessfulEvent -= purchaseSuccessful;
		StoreKitManager.purchaseCancelledEvent -= purchaseCancelled;
		StoreKitManager.purchaseFailedEvent -= purchaseFailed;
		StoreKitManager.productListReceivedEvent -= productListReceivedEvent;
		StoreKitManager.productListRequestFailedEvent -= productListRequestFailed;
		StoreKitManager.restoreTransactionsFailedEvent -= restoreTransactionsFailed;
		StoreKitManager.restoreTransactionsFinishedEvent -= restoreTransactionsFinished;
		StoreKitManager.paymentQueueUpdatedDownloadsEvent -= paymentQueueUpdatedDownloadsEvent;
	}
	
	void productListReceivedEvent( List<StoreKitProduct> productList )
	{
		Debug.Log( "productListReceivedEvent. total products received: " + productList.Count );
		
		// print the products to the console
		foreach( StoreKitProduct product in productList )
			Debug.Log( product.ToString() + "\n" );

		_products = productList;
	}
	
	void productListRequestFailed( string error )
	{
		Debug.Log( "productListRequestFailed: " + error );
	}
	
	void purchaseFailed( string error )
	{
		Debug.Log( "purchase failed with error: " + error );
		_currentCallbackOnFinishedTransaction ( false );
	}
	
	void purchaseCancelled( string error )
	{
		Debug.Log( "purchase cancelled with error: " + error );
		_currentCallbackOnFinishedTransaction ( false );
	}
	
	void productPurchaseAwaitingConfirmationEvent( StoreKitTransaction transaction )
	{
		Debug.Log( "productPurchaseAwaitingConfirmationEvent: " + transaction );
	}
	
	void purchaseSuccessful( StoreKitTransaction transaction )
	{
		Debug.Log( "purchased product: " + transaction );
		_currentCallbackOnFinishedTransaction ( true );
	}
	
	void restoreTransactionsFailed( string error )
	{
		Debug.Log( "restoreTransactionsFailed: " + error );
	}
	
	void restoreTransactionsFinished()
	{
		Debug.Log( "restoreTransactionsFinished" );
	}
	
	void paymentQueueUpdatedDownloadsEvent( List<StoreKitDownload> downloads )
	{
		Debug.Log( "paymentQueueUpdatedDownloadsEvent: " );
		foreach( var dl in downloads )
			Debug.Log( dl );
	}
#endif
}

