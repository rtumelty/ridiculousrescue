using UnityEngine;
using System.Collections;

public class AnOtherFeaturesPreview : MonoBehaviour {

	public GameObject image;
	public Texture2D helloWorldTexture;



	void Awake() {
		AndroidCamera.instance.OnImagePicked += OnImagePicked;
	}

	public void SaveToGalalry() {
		AndroidCamera.instance.SaveImageToGalalry(helloWorldTexture);
		AndroidNative.showMessage("Saved", "Image saved to gallery");
		SA_StatusBar.text =  "Image saved to gallery";
	}

	public void SaveScreenshot() {
		AndroidCamera.instance.SaveScreenshotToGallery();
		AndroidNative.showMessage("Saved", "Screenshot saved to gallery");
		SA_StatusBar.text =  "Screenshot saved to gallery";
	}


	public void GetImageFromGallery() {
		AndroidCamera.instance.GetImageFromGallery();
	}
	
	
	
	public void GetImageFromCamera() {
		AndroidCamera.instance.GetImageFromCamera();
	}






	private void EnableImmersiveMode() {
		ImmersiveMode.instance.EnableImmersiveMode();
	}
	



	private void LoadAppInfo() {

		AndroidAppInfoLoader.instance.addEventListener (AndroidAppInfoLoader.PACKAGE_INFO_LOADED, OnPackageInfoLoaded);
		AndroidAppInfoLoader.instance.LoadPackageInfo ();
	}


	private void LoadAdressBook() {
		AddressBookController.instance.LoadContacts ();
		AddressBookController.instance.OnContactsLoadedAction += OnContactsLoaded;

	}

	void OnContactsLoaded () {
		AddressBookController.instance.OnContactsLoadedAction -= OnContactsLoaded;
		AndroidNative.showMessage("On Contacts Loaded" , "Andress book has " + AddressBookController.instance.contacts.Count + " Contacts");
	}
	

	private void OnImagePicked(AndroidImagePickResult result) {
		Debug.Log("OnImagePicked");
		if(result.IsSucceeded) {
			image.renderer.material.mainTexture = result.image;
		}
	}

	private void OnPackageInfoLoaded() {
		AndroidAppInfoLoader.instance.removeEventListener (AndroidAppInfoLoader.PACKAGE_INFO_LOADED, OnPackageInfoLoaded);

		string msg = "";
		msg += "versionName: " + AndroidAppInfoLoader.instance.PacakgeInfo.versionName + "\n";
		msg += "versionCode: " + AndroidAppInfoLoader.instance.PacakgeInfo.versionCode + "\n";
		msg += "packageName" + AndroidAppInfoLoader.instance.PacakgeInfo.packageName + "\n";
		msg += "lastUpdateTime:" + System.Convert.ToString(AndroidAppInfoLoader.instance.PacakgeInfo.lastUpdateTime) + "\n";
		msg += "sharedUserId" + AndroidAppInfoLoader.instance.PacakgeInfo.sharedUserId + "\n";
		msg += "sharedUserLabel"  + AndroidAppInfoLoader.instance.PacakgeInfo.sharedUserLabel;

		AndroidNative.showMessage("App Info Loaded", msg);
	}

}
