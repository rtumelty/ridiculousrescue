using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class FL_LivesControl : MonoBehaviour 
{
	private float _countUpdate = 0f;
	public int timeForLifeGen = 0;
	//private int myLives = 0;

	private long myStartTime = 0;
	private long myLastRecordedTime = 0;
	private int timeDelta = 0;

	private long myLastTick = 0;
	private int currentDelta = 0;
	public TextMesh myTimeDisplay;
	public TextMesh myLivesDisplay;

	void Start ()
	{
//		print ("Start lives from script " + GameGlobalVariables.Stats.LIVES_AVAILABLE);
//		print ("Start lives from save " + SaveDataManager.getValue (SaveDataManager.LIVES_AVAILABLE));

		myStartTime = DateTime.UtcNow.Ticks / 10000000;

		myLastRecordedTime = SaveDataManager.getValue (SaveDataManager.LAST_RECORDED_TIME);
		timeDelta = (int)(myStartTime - myLastRecordedTime);


		myTimeDisplay = transform.Find ("textHeartTime").GetComponent<TextMesh> ();
		myLivesDisplay = transform.Find ("textHeartAmount").GetComponent<TextMesh> ();

		timeForLifeGen = SaveDataManager.getValue (SaveDataManager.LEFTOVER_TIME);

		if (timeForLifeGen < timeDelta && GameGlobalVariables.Stats.LIVES_AVAILABLE < 5) 
		{
			timeDelta = timeDelta - timeForLifeGen;
			GameGlobalVariables.Stats.LIVES_AVAILABLE ++;


			while(GameGlobalVariables.Stats.LIVES_AVAILABLE < 5 && timeDelta > GameGlobalVariables.Stats.TIME_FOR_LIFE_GENERATION)
			{
				timeDelta = timeDelta - GameGlobalVariables.Stats.TIME_FOR_LIFE_GENERATION;
				GameGlobalVariables.Stats.LIVES_AVAILABLE ++;
			}

			if(GameGlobalVariables.Stats.LIVES_AVAILABLE < 5)
			{
				timeForLifeGen = GameGlobalVariables.Stats.TIME_FOR_LIFE_GENERATION - (timeDelta % GameGlobalVariables.Stats.TIME_FOR_LIFE_GENERATION);
				print ("Modulus result: " + (timeDelta % GameGlobalVariables.Stats.TIME_FOR_LIFE_GENERATION));
			}
		}
		else
		{
			timeForLifeGen = SaveDataManager.getValue(SaveDataManager.LEFTOVER_TIME) - timeDelta;
			if(timeForLifeGen > GameGlobalVariables.Stats.TIME_FOR_LIFE_GENERATION)
			{
				timeForLifeGen = GameGlobalVariables.Stats.TIME_FOR_LIFE_GENERATION;
			}
		}
		//This loads the saved life variable if a previous save exists
		if(PlayerPrefs.HasKey(SaveDataManager.LIVES_AVAILABLE))
		{
			GameGlobalVariables.Stats.LIVES_AVAILABLE = SaveDataManager.getValue (SaveDataManager.LIVES_AVAILABLE);
		}
		myLivesDisplay.text = GameGlobalVariables.Stats.LIVES_AVAILABLE.ToString ();
	}


	void CheckForEvidenceOfSleep ()
	{
		currentDelta = (int)(DateTime.UtcNow.Ticks / 10000000 - myLastTick);
		if(currentDelta > 2)
		{
			timeForLifeGen = timeForLifeGen - currentDelta;
		}
	}


	void Update ()
	{

		_countUpdate -= Time.deltaTime;
		
		if ( _countUpdate > 0f) return;
		_countUpdate = 2f;
		CheckForEvidenceOfSleep ();
		SaveDataManager.save ( SaveDataManager.LAST_RECORDED_TIME, (int) ( DateTime.UtcNow.Ticks / 10000000 ));

		if(GameGlobalVariables.Stats.LIVES_AVAILABLE == 5)
		{
			myTimeDisplay.text = "FULL";
			SaveDataManager.save(SaveDataManager.LAST_RECORDED_TIME, 0);
			SaveDataManager.save ( SaveDataManager.LIVES_AVAILABLE, GameGlobalVariables.Stats.LIVES_AVAILABLE);
			myLivesDisplay.text = GameGlobalVariables.Stats.MAX_LIVES.ToString();

			return;
		}

		timeForLifeGen --;

		SaveDataManager.save ( SaveDataManager.LEFTOVER_TIME, timeForLifeGen);
		myTimeDisplay.text = TimeScaleManager.getTimeString (timeForLifeGen);



		if(timeForLifeGen <= 0)
		{
			timeForLifeGen = GameGlobalVariables.Stats.TIME_FOR_LIFE_GENERATION;
			if(GameGlobalVariables.Stats.LIVES_AVAILABLE < 5)
			{
				GameGlobalVariables.Stats.LIVES_AVAILABLE ++;
				SaveDataManager.save ( SaveDataManager.LIVES_AVAILABLE, GameGlobalVariables.Stats.LIVES_AVAILABLE);
				myLivesDisplay.text = GameGlobalVariables.Stats.LIVES_AVAILABLE.ToString();
			}
		}

		myLivesDisplay.text = GameGlobalVariables.Stats.LIVES_AVAILABLE.ToString();
		myLastTick = DateTime.UtcNow.Ticks / 10000000;
	}
}
