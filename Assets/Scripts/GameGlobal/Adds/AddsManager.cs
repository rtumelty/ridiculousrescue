using UnityEngine;
using System.Collections;
//using Chartboost;

public class AddsManager : MonoBehaviour 
{
	//*************************************************************//	
	private static AddsManager _meInstance;
	public static AddsManager getInstance ()
	{
		return _meInstance;
	}
	//*************************************************************//

	/*
	void Awake ()
	{
		_meInstance = this;
		OnEnable ();
	}

	void OnEnable()
	{
		// Listen to all impression-related events
		CBManager.didFailToLoadInterstitialEvent += didFailToLoadInterstitialEvent;
		CBManager.didDismissInterstitialEvent += didDismissInterstitialEvent;
		CBManager.didCloseInterstitialEvent += didCloseInterstitialEvent;
		CBManager.didClickInterstitialEvent += didClickInterstitialEvent;
		CBManager.didCacheInterstitialEvent += didCacheInterstitialEvent;
		CBManager.didShowInterstitialEvent += didShowInterstitialEvent;
		CBManager.didFailToLoadMoreAppsEvent += didFailToLoadMoreAppsEvent;
		CBManager.didDismissMoreAppsEvent += didDismissMoreAppsEvent;
		CBManager.didCloseMoreAppsEvent += didCloseMoreAppsEvent;
		CBManager.didClickMoreAppsEvent += didClickMoreAppsEvent;
		CBManager.didCacheMoreAppsEvent += didCacheMoreAppsEvent;
		CBManager.didShowMoreAppsEvent += didShowMoreAppsEvent;

#if UNITY_ANDROID
		CBBinding.init();
#elif UNITY_IPHONE
		//CBBinding.init("app_id", "app_signature");
#endif
	}

#if UNITY_ANDROID
	public void Update() 
	{
		if ( Application.platform == RuntimePlatform.Android ) 
		{
			if ( Input.GetKeyUp ( KeyCode.Escape )) 
			{
				if ( CBBinding.onBackPressed ()) return;
				else Application.Quit();
			}
		}
	}

	public void showAdd ()
	{
		Debug.Log ( "Add started" );
		CBBinding.showInterstitial ( null );
	}
#endif

#if UNITY_ANDROID || UNITY_IPHONE
	
	void OnDisable()
	{
		// Remove event handlers
		CBManager.didFailToLoadInterstitialEvent -= didFailToLoadInterstitialEvent;
		CBManager.didDismissInterstitialEvent -= didDismissInterstitialEvent;
		CBManager.didCloseInterstitialEvent -= didCloseInterstitialEvent;
		CBManager.didClickInterstitialEvent -= didClickInterstitialEvent;
		CBManager.didCacheInterstitialEvent -= didCacheInterstitialEvent;
		CBManager.didShowInterstitialEvent -= didShowInterstitialEvent;
		CBManager.didFailToLoadMoreAppsEvent -= didFailToLoadMoreAppsEvent;
		CBManager.didDismissMoreAppsEvent -= didDismissMoreAppsEvent;
		CBManager.didCloseMoreAppsEvent -= didCloseMoreAppsEvent;
		CBManager.didClickMoreAppsEvent -= didClickMoreAppsEvent;
		CBManager.didCacheMoreAppsEvent -= didCacheMoreAppsEvent;
		CBManager.didShowMoreAppsEvent -= didShowMoreAppsEvent;
	}
	
	void didFailToLoadInterstitialEvent( string location )
	{
		Debug.Log( "didFailToLoadInterstitialEvent: " + location );
	}
	
	void didDismissInterstitialEvent( string location )
	{
		Debug.Log( "didDismissInterstitialEvent: " + location );
	}
	
	void didCloseInterstitialEvent( string location )
	{
		Debug.Log( "didCloseInterstitialEvent: " + location );
	}
	
	void didClickInterstitialEvent( string location )
	{
		Debug.Log( "didClickInterstitialEvent: " + location );
	}
	
	void didCacheInterstitialEvent( string location )
	{
		Debug.Log( "didCacheInterstitialEvent: " + location );
	}
	
	void didShowInterstitialEvent( string location )
	{
		Debug.Log( "didShowInterstitialEvent: " + location );
	}
	
	void didFailToLoadMoreAppsEvent()
	{
		Debug.Log( "didFailToLoadMoreAppsEvent" );
	}
	
	void didDismissMoreAppsEvent()
	{
		Debug.Log( "didDismissMoreAppsEvent" );
	}
	
	void didCloseMoreAppsEvent()
	{
		Debug.Log( "didCloseMoreAppsEvent" );
	}
	
	void didClickMoreAppsEvent()
	{
		Debug.Log( "didClickMoreAppsEvent" );
	}
	
	void didCacheMoreAppsEvent()
	{
		Debug.Log( "didCacheMoreAppsEvent" );
	}
	
	void didShowMoreAppsEvent()
	{
		Debug.Log( "didShowMoreAppsEvent" );
	}
	
	#endif
	*/
}
