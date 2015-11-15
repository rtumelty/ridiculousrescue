using UnityEngine;
using System.Collections;

public class TutorialComponent : MonoBehaviour 
{
	private GameObject tutorialUIPanel;
	private GameObject hand1;
	private GameObject hand2;
	private GameObject hand3;

	private GameObject handHolder2A;
	private GameObject handHolder2B;
	private GameObject handHolder2C;
	private GameObject handHolder3A;
	private GameObject handHolder3B;
	private GameObject handHolder3C;

	private GameObject colliderSet3A;
	private GameObject colliderSet3B;

	private GameObject frame;
	private GameTextControl myTip;
	private int movesRecord;
	public int takeAStep = 0;

	private bool onceOff = false;
	private bool onceOff2 = false;
	private bool onceOff3 = false;
	private bool onceOff4 = false;
	private bool allowUpdate = false;

	private static TutorialComponent myTutorialComponent;
	public static TutorialComponent getInstance ()
	{
		if ( myTutorialComponent == null )
		{	
			if(Camera.main != null)
			{
				myTutorialComponent = GameObject.Find("tutorialUIPanel").GetComponent < TutorialComponent > ();
			}
		}
		return myTutorialComponent;
	}


	void Start()
	{
		tutorialUIPanel = GameObject.Find ("tutorialUIPanel");
		colliderSet3A = GameObject.Find ("tutorialCollider3a");
		colliderSet3B = GameObject.Find ("tutorialCollider3b");
		colliderSet3A.SetActive (false);
		colliderSet3B.SetActive (false);
		movesRecord = GameGlobalVariables.Stats.MOVES;
		hand1 = GameObject.Find ("level1Hand").gameObject;
		hand2 = GameObject.Find ("level2Hand").gameObject;
		hand3 = GameObject.Find ("level3Hand").gameObject;

		handHolder2A = hand2.transform.Find ("handHolder2A").gameObject;
		handHolder2B = hand2.transform.Find ("handHolder2B").gameObject;
		handHolder2C = hand2.transform.Find ("handHolder2C").gameObject;
		//handHolder3A = hand3.transform.Find ("handHolder3A").gameObject;
		//handHolder3B = hand3.transform.Find ("handHolder3B").gameObject;
		//handHolder3C = hand3.transform.Find ("handHolder3C").gameObject;
		frame = GameObject.Find ("frame").gameObject;
		myTip = frame.transform.Find("textTask").GetComponent<GameTextControl>();
		
		hand1.SetActive (false);
		hand2.SetActive (false);
		hand3.SetActive (false);
		frame.SetActive (false);
	}


	public void CustomStart()
	{
		if(LevelControl.CURRENT_LEVEL_CLASS.type == GameGlobalVariables.RESCUE)
		{
			if(LevelControl.CURRENT_LEVEL_CLASS.myName == "1" || LevelControl.CURRENT_LEVEL_CLASS.myName == "3" || LevelControl.CURRENT_LEVEL_CLASS.myName == "5"|| LevelControl.CURRENT_LEVEL_CLASS.myName == "6")
			{
				StartCoroutine("PlayTutorial");
			}
			else
			{
				gameObject.SetActive(false);
			}
		}
	}


	IEnumerator PlayTutorial()
	{
		yield return new WaitForSeconds (0.5f);
		allowUpdate = true;
		frame.SetActive (true);
	}


	void Update()
	{
		if(allowUpdate == false)
		{
			return;
		}
		print ("Step" + takeAStep);
		if(Main.getInstance() != null)
		{
			if(LevelControl.CURRENT_LEVEL_CLASS.myName == "1")
			{
				if(Main.getInstance().CurrentChar().myID == GameElements.CHAR_MADRA_1_IDLE && onceOff == false)
				{
					takeAStep ++;
					onceOff = true;
				}
				if(GameGlobalVariables.Stats.MOVES < movesRecord)
				{
					takeAStep ++;
				}
			}
			if(LevelControl.CURRENT_LEVEL_CLASS.myName == "3")
			{
				if(Main.getInstance().getCurrentCharacter() != null)
				{
					if(Main.getInstance().getCurrentCharacter().myID == GameElements.CHAR_MADRA_1_IDLE && onceOff == false)
					{
						takeAStep ++;
						onceOff = true;
					}
					if(Main.getInstance().getCurrentCharacter().myID == GameElements.CHAR_MADRA_1_IDLE && 
					   onceOff2 == false && 
					   Main.getInstance().getCurrentCharacter().position[0] == 8 &&
					   Main.getInstance().getCurrentCharacter().position[1] == 6 &&
					   Main.getInstance().getCurrentCharacter().moving == false)
					{
						takeAStep ++;
						onceOff2 = true;
					}
					print ("difuhviudhvu" + Main.getInstance().getCurrentCharacter().myID);
					if(Main.getInstance().getCurrentCharacter().myID == GameElements.CHAR_CORA_1_IDLE && 
					   Main.getInstance().getCurrentCharacter().position[0] == 6 &&
					   onceOff3 == false &&
					   Main.getInstance().getCurrentCharacter().position[1] == 6)
					{
						print ("Last step taken!");
						takeAStep ++;
						onceOff3 = true;
					}
	#if UNITY_EDITOR
					if(takeAStep >= 3 &&
					   Input.GetMouseButtonDown(0))
					{
						takeAStep++;
					}

	#else
					if(takeAStep >= 3 &&
					   CheckInputType.getInstance().touchType == CheckInputType.TAP_TYPE)
					{
						takeAStep++;
					}
#endif
				}
			}
			switch (takeAStep)
			{
				//================================================== STEP 0
			case 0:
				switch(LevelControl.CURRENT_LEVEL_CLASS.myName)
				{
				case "1":
					hand1.SetActive(true);
					myTip.myKey = "rescue_tutorial_cora_01";
					break;
				case "3":
					colliderSet3A.SetActive (true);
					hand2.SetActive(true);
					myTip.myKey = "rescue_tutorial_madra_01";
					handHolder2A.SetActive (true);
					handHolder2B.SetActive (false);
					handHolder2C.SetActive (false);
					break;
				case "5":
					hand3.SetActive(true);
					myTip.myKey = "rescue_tutorial_fara_01";
					break;
				case "6":
					hand2.SetActive(true);
					handHolder2A.SetActive (false);
					handHolder2B.SetActive (true);
					handHolder2C.SetActive (false);
					handHolder2B.transform.position = new Vector3(20.7f, handHolder2B.transform.position.y, 5.8f);
					myTip.myKey = "rescue_tutorial_madra_05";
					break;
				}
				break;
				//================================================== STEP 1
			case 1:
				switch(LevelControl.CURRENT_LEVEL_CLASS.myName)
				{
				case "1":
					hand1.SetActive(false);
					gameObject.SetActive(false);
					break;
				case "3":
					GlobalVariables.RESTRICT_MADRA_TUTORIAL_LEVEL_3 = true;
					handHolder2A.SetActive (false);
					handHolder2B.SetActive (true);
					handHolder2B.transform.position = new Vector3(8.7f, handHolder2B.transform.position.y, 5.8f);
					handHolder2C.SetActive (false);
					myTip.myKey = "rescue_tutorial_madra_02";
					break;
				case "5":
					hand3.SetActive(false);
					gameObject.SetActive(false);
					break;
				case "6":
					hand2.SetActive(false);
					gameObject.SetActive(false);
					break;
				}
				break;
				//================================================== STEP 2
			case 2:
				switch(LevelControl.CURRENT_LEVEL_CLASS.myName)
				{
				case "1":
					hand1.SetActive(false);
					break;
				case "3":
					GlobalVariables.RESTRICT_MADRA_TUTORIAL_LEVEL_3 = false;
					colliderSet3A.SetActive(false);
					colliderSet3B.SetActive(true);
					if(onceOff4 == false)
					{
						Main.getInstance().deselectAllChars();
						onceOff4 = true;
					}
					handHolder2A.SetActive (false);
					handHolder2B.SetActive (false);
					hand1.SetActive(true);
					hand1.transform.position = new Vector3(3.7f, hand1.transform.position.y, hand1.transform.position.z);
					myTip.myKey = "rescue_tutorial_madra_03";
					break;
				case "5":
					hand3.SetActive(true);
					break;
				}
				break;
			case 3:
				switch(LevelControl.CURRENT_LEVEL_CLASS.myName)
				{
				case "1":
					hand1.SetActive(false);
					break;
				case "3":
					colliderSet3B.SetActive(false);
					handHolder2A.SetActive (false);
					handHolder2B.SetActive (false);
					handHolder2C.SetActive (false);
					hand1.SetActive(false);
					myTip.myKey = "rescue_tutorial_madra_04";
					//gameObject.SetActive(false);
					break;
				case "5":
					hand3.SetActive(true);
					break;
				}
				break;
			case 4:
				switch(LevelControl.CURRENT_LEVEL_CLASS.myName)
				{
				case "1":
					hand1.SetActive(false);
					break;
				case "3":
					handHolder2A.SetActive (false);
					handHolder2B.SetActive (false);
					handHolder2C.SetActive (false);
					hand1.SetActive(false);
					gameObject.SetActive(false);
					break;
				case "5":
					hand3.SetActive(true);
					break;
				}
				break;
			}
			movesRecord = GameGlobalVariables.Stats.MOVES;
		}
	}
}
