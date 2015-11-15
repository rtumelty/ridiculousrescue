using UnityEngine;
using System.Collections;

public class ScoreHandler : MonoBehaviour 
{
	private bool firstPass = true;
	private bool allowLerp = false;
	private TextMesh scoreText;
	private TextMesh movesText;
	private int movesHolder;
	private float _countUpdate = 1;
	private float decayRate = .3f;
	private GameObject scoreParent1;
	private GameObject movesParent1;
	private GameObject scoreParent2;
	private GameObject movesParent2;
	private GameObject scoreToShake;
	private GameObject movesToShake;
	private static ScoreHandler _meInstance;
	public static ScoreHandler getInstance ()
	{
		if ( _meInstance == null )
		{
			_meInstance = GameObject.Find ( "mainPanel" ).GetComponent < ScoreHandler > ();
		}
		
		return _meInstance;
	}

	void Start()
	{
		scoreText = transform.Find ("scorePanel").transform.Find ("number").GetComponent<TextMesh> ();
		movesText = transform.Find ("movesPanel").transform.Find ("number").GetComponent<TextMesh> ();
		scoreToShake = transform.Find ("scorePanel").gameObject;
		movesToShake = transform.Find ("movesPanel").gameObject;
	}


	void Update()
	{
		if(allowLerp == false)
		{
			return;
		}
		print ("Step 22222222");
		print (movesHolder);

		if(movesHolder > 0)
		{
			_countUpdate -= Time.deltaTime;
			
			if ( _countUpdate > 0f ) return;
			
			_countUpdate = decayRate;
			if(decayRate > .3f)
			decayRate -= .1f;
			print ("WTF");
			movesHolder -= 1;
			iTween.PunchScale ( movesToShake, iTween.Hash("z", 0.25f, "x", 0.07f, "time", 0.29f));
			iTween.PunchScale ( scoreToShake, iTween.Hash("z", 0.25f, "x", 0.07f, "time", 0.29f));
			//iTween.PunchScale(movesText.gameObject.transform.parent.gameObject,iTween.Hash("z", 0.25f, "time", 0.29f));
			//iTween.PunchScale(scoreText.gameObject.transform.parent.gameObject,iTween.Hash("z", 0.25f, "time", 0.29f));
			GameGlobalVariables.Stats.SCORE += 100;
			SoundManager.getInstance().playSound(SoundManager.SCORE_PLINK);
		}
		movesText.text = movesHolder.ToString ();
		if(movesHolder == 0 && firstPass == true)
		{
			print ("Step 33333333");
			firstPass = false;
			allowLerp = false;
			MovesWarningCreator.getInstance().StartCoroutine("ShowScore");
			StartCoroutine ("SlightPause");
		}
	}


	IEnumerator SlightPause ()
	{
		yield return new WaitForSeconds (2.5f);
		print ("Step 4444444");
		ToyLazarusSequenceControl.getInstance ().initToyLazarusSequence ( LevelControl.getInstance ().toBeRescuedOnLevel.transform.Find ( "tile" ).GetComponent < IComponent > ().myID );
	}


	public void DoScoreSequence ()
	{
		MovesUIControl.getInstance ().overridden = true;
		firstPass = true;
		movesHolder = GameGlobalVariables.Stats.MOVES;
		allowLerp = true;
		print ("Step 111111");
	}
}
