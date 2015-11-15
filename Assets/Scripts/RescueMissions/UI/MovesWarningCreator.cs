using UnityEngine;
using System.Collections;

public class MovesWarningCreator : MonoBehaviour 
{
	private float myCountDown = 0f;
	private int myLastMoveCount = 0;
	private GameObject warningPanel;
	private GameObject busyPanel;
	private GameObject scorePanel;
	private float _countUpdate = 0f;
	private bool threeRan = false;
	private bool twoRan = false;
	private bool oneRan = false;
	private bool zeroRan = false;
	private bool showWarn = false;
	GameObject currentBusyPanel;
	private static MovesWarningCreator _meInstance;
	public static MovesWarningCreator getInstance ()
	{
		if ( _meInstance == null )
		{
			_meInstance = Camera.main.GetComponent < MovesWarningCreator > ();
		}
		
		return _meInstance;
	}

	void Start () 
	{
		warningPanel = (GameObject)Resources.Load ("Prefabs/warningPanel");
		busyPanel = (GameObject)Resources.Load ("Prefabs/busyPanel");
		scorePanel = (GameObject)Resources.Load ("Prefabs/scorePanel");
	}
	
	public void resetWarnings ()
	{
		threeRan = false;
		twoRan = false;
		oneRan = false;
		zeroRan = false;
	}

	public IEnumerator ShowWarning ()
	{	
		if (currentBusyPanel == null) Destroy(currentBusyPanel);
		yield return new WaitForSeconds (0.2f);
		if(GlobalVariables.SCREEN_DRAGGING == false)
		{
			currentBusyPanel = (GameObject) Instantiate (busyPanel, transform.position, transform.rotation);
			currentBusyPanel.transform.parent = Camera.main.transform;
			currentBusyPanel.transform.position = currentBusyPanel.transform.position + Vector3.forward * 4 + Vector3.down * 4;
			showWarn = false;
			myCountDown = 0.8f;
		}
	}
	public IEnumerator ShowScore ()
	{	
		if (currentBusyPanel == null) Destroy(currentBusyPanel);
		yield return new WaitForSeconds (0.1f);
		currentBusyPanel = (GameObject) Instantiate (scorePanel, transform.position, transform.rotation);
		currentBusyPanel.transform.parent = Camera.main.transform;
		currentBusyPanel.transform.position = currentBusyPanel.transform.position + Vector3.forward * 4 + Vector3.down * 4;
		Destroy (currentBusyPanel.GetComponent<GameTextControl>());
		currentBusyPanel.GetComponent<TextMesh> ().text = GameGlobalVariables.Stats.SCORE.ToString ();

		GameObject dropShadow = currentBusyPanel.transform.FindChild ("dropShadow").gameObject;
		if (dropShadow != null) {
			Destroy (dropShadow.GetComponent<GameTextControl>());
			TextMesh textMesh = dropShadow.GetComponent<TextMesh>() as TextMesh;
			textMesh.text = GameGlobalVariables.Stats.SCORE.ToString ();
		}
	}


	void CreateWarning ()
	{
		GameObject currentWarningPanel = (GameObject) Instantiate (warningPanel, transform.position + Vector3.down * 4f, transform.rotation);
		currentWarningPanel.transform.parent = Camera.main.transform;
	}


	void Update () 
	{
	/*	if(Input.GetKeyDown("l"))
		{
			GameObject currentWarningPanel = (GameObject) Instantiate (warningPanel, transform.position + Vector3.down * 4f, transform.rotation);
			currentWarningPanel.transform.parent = Camera.main.transform;
		}*/
		myCountDown -= Time.deltaTime;
#if UNITY_EDITOR
		if(Input.GetMouseButtonUp(0))
#else
		if (Input.touchCount == 0) return;
	 	if(Input.touches[0].phase == TouchPhase.Ended)
#endif
		{
			/*print ("IS IT? : " + GameGlobalVariables.CHARACTER_RECENTLY_CHANGED);
			if(GlobalVariables.READY_FOR_NEXT_TURN == false && GameGlobalVariables.CHARACTER_RECENTLY_CHANGED == false)
			{
				GlobalVariables.checkForMenus();
				foreach(CharacterData myChars in LevelControl.getInstance().charactersOnLevel)
				{
					print (myChars.coraSlidingTroley);
					if((myChars.moving || myChars.interactAction) && !myChars.coraSlidingTroley)
					{
						showWarn = true;
					}
				}
				if(showWarn == true && myCountDown < 0)
				{
					MovesWarningCreator.getInstance().StartCoroutine("ShowWarning");
				}
			}*/
		}
		_countUpdate -= Time.deltaTime;
		
		if ( _countUpdate > 0f ) return;
		
		_countUpdate = 0.1f;

		if(GameGlobalVariables.Stats.MOVES < 4)
		{
//			print ("Step 1");
			switch(GameGlobalVariables.Stats.MOVES)
			{
			case 3:
				if(myLastMoveCount > GameGlobalVariables.Stats.MOVES)
				{
					CreateWarning();
				}
				break;

			case 2:
				if(myLastMoveCount > GameGlobalVariables.Stats.MOVES)
				{
					CreateWarning();
				}
				break;

			case 1:
				if(myLastMoveCount > GameGlobalVariables.Stats.MOVES)
				{
					CreateWarning();
				}
				break;

			case 0:
				if(myLastMoveCount > GameGlobalVariables.Stats.MOVES)
				{
					CreateWarning();
				}
				break;
			}
		}
		myLastMoveCount = GameGlobalVariables.Stats.MOVES;
	}
}
