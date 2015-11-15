using UnityEngine;
using System.Collections;

public class MovesUIControl : MonoBehaviour 
{
	//*************************************************************//
	private TextMesh _number;
	public bool startCounting = false;
	public bool overridden = false;
	//*************************************************************//
	private static MovesUIControl _meInstance;
	public static MovesUIControl getInstance ()
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
		if ( ! startCounting ) return;
		if ( GameGlobalVariables.Stats.MOVES <= 0 )
		{
			GameGlobalVariables.Stats.MOVES = 0;
			StartCoroutine("CheckIfWin");
		}
		if(overridden == false)
		{
			_number.text = GameGlobalVariables.Stats.MOVES.ToString ();
		}
	}

	IEnumerator CheckIfWin()
	{
		yield return new WaitForSeconds (0.5f);
		if(GlobalVariables.TOY_RESCUED == false)
		{
			Main.getInstance ().showFaliedScreen ();
			print ("Fail called");
		}
	}
}
