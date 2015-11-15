using UnityEngine;
using System.Collections;

public class RegisterSettings : MonoBehaviour {
	BackButtonHandler backButton;

	void OnEnable () {
		backButton = FindObjectOfType<BackButtonHandler> ();
		backButton.gameState = BackButtonHandler.GameState.Settings;
		backButton.settingsObject = gameObject;
	}

	void OnDisable () {
		backButton.settingsObject = null;
	
	}
}
