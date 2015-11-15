using UnityEngine;
using System.Collections;

public class RestartedBriefScreenControl : MonoBehaviour 
{
	private TextMesh slot1Data;
	private TextMesh slot2Data;
	private TextMesh slot3Data;

	private GameTextControl slot1Text;
	private GameTextControl slot2Text;
	private GameTextControl slot3Text;

	private GameTextControl titleText;

	private int levelValue = 0;

	void Start ()
	{
		slot1Data = transform.Find ("slot01").transform.Find ("data").gameObject.GetComponent<TextMesh>();
		slot1Text = transform.Find ("slot01").transform.Find ("text").gameObject.GetComponent<GameTextControl>();

		slot2Data = transform.Find ("slot02").transform.Find ("data").gameObject.GetComponent<TextMesh>();
		slot2Text = transform.Find ("slot02").transform.Find ("text").gameObject.GetComponent<GameTextControl>();

		slot3Data = transform.Find ("slot03").transform.Find ("data").gameObject.GetComponent<TextMesh>();
		slot3Text = transform.Find ("slot03").transform.Find ("text").gameObject.GetComponent<GameTextControl>();

		if(int.TryParse(LevelControl.CURRENT_LEVEL_CLASS.myName, out levelValue))
		{
			titleText = transform.Find ("levelTitle").transform.Find ("title").gameObject.GetComponent<GameTextControl>();

			if(LevelControl.CURRENT_LEVEL_CLASS.type == 0)
			{
				titleText.myKey = "ui_sign_mission";
				titleText.addText = " " + levelValue.ToString();
			}
			else if(LevelControl.CURRENT_LEVEL_CLASS.type == 1)
			{
				titleText.myKey = "ui_sign_bonus_mission";
				titleText.addText = " " + levelValue.ToString();
			}
		}
		//

		slot1Data.text = LevelControl.CURRENT_LEVEL_CLASS.movesLimitForStar.ToString();

		slot2Data.text = TimeScaleManager.getTimeString ((int) LevelControl.CURRENT_LEVEL_CLASS.timeLimitForStar);

		slot3Data.text = LevelControl.CURRENT_LEVEL_CLASS.scoreLimitForStar.ToString();

	}
}
