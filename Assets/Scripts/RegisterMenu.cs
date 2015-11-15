using UnityEngine;
using System.Collections;

public class RegisterMenu : MonoBehaviour {
	BackButtonHandler backButton;

	void OnEnable () {
		backButton = FindObjectOfType<BackButtonHandler> ();
		backButton.gameState = BackButtonHandler.GameState.Menu;
		backButton.menuObject = gameObject;
	}

	void OnDisable () {
		backButton.menuObject = null;
	
	}
}
