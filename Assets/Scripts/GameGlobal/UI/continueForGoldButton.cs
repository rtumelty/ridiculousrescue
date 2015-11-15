using UnityEngine;
using System.Collections;

public class continueForGoldButton : MonoBehaviour 
{
	public bool allowPress = true;
	private GameObject winScreen;
	void Start ()
	{
		winScreen = ResoultScreen.getInstance ().winScreen;
		transform.Find("continueButton").transform.Find("textCountDown").GetComponent<TextMesh> ().text = GameGlobalVariables.Stats.COST_OF_CONTINUE.ToString();
	}
	void OnMouseUp ()
	{
		if (allowPress == true) {
			if (GameGlobalVariables.Stats.METAL_IN_CONTAINERS >= GameGlobalVariables.Stats.COST_OF_CONTINUE) {
					allowPress = false;
					GameGlobalVariables.Stats.METAL_IN_CONTAINERS -= GameGlobalVariables.Stats.COST_OF_CONTINUE;
					SaveDataManager.save (SaveDataManager.METAL_IN_CONTAINERS, GameGlobalVariables.Stats.METAL_IN_CONTAINERS);
					GameGlobalVariables.Stats.MOVES += GameGlobalVariables.Stats.MOVES_BOUGHT_FOR_CONTINUE;
					StartCoroutine ("WaitForAnim");
			} else {
					GameObject.Find ("GoldAddButton").SendMessage ("handleTouched", GameObject.Find ("CameraResoultScreen").transform.Find ("background").transform.Find ("uiPivot2").transform);
			}
		}
	}

	IEnumerator WaitForAnim ()
	{
		ResoultScreen.getInstance ().StartCoroutine ("CoraFailScreenAnim");
		yield return new WaitForSeconds (1.6f);
		MovesWarningCreator.getInstance ().resetWarnings ();
		Main.getInstance ().restorePlay ();
		ResoultScreen.getInstance ().RestoreWinScreen ();
		yield return new WaitForSeconds (1f);
		allowPress = true;
		GlobalVariables.MENU_FOR_RESOULT_SCREEN = false;
		GlobalVariables.READY_FOR_NEXT_TURN = true;
		print ("Ok Again");
	}
}
