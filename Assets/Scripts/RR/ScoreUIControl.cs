using UnityEngine;
using System.Collections;

public class ScoreUIControl : MonoBehaviour 
{
	//*************************************************************//
	private TextMesh _number;
	//*************************************************************//
	private static ScoreUIControl _meInstance;
	public static ScoreUIControl getInstance ()
	{
		return _meInstance;
	}
	//*************************************************************//
	
	void Awake () 
	{
		_meInstance = this;
	}
	
	void Start () 
	{
		_number = transform.Find ( "number" ).GetComponent < TextMesh > ();
	}
	
	void Update () 
	{
		if ( GameGlobalVariables.Stats.SCORE <= 0 )
		{
			GameGlobalVariables.Stats.SCORE = 0;
		}

		_number.text = GameGlobalVariables.Stats.SCORE.ToString ();

	}
}
