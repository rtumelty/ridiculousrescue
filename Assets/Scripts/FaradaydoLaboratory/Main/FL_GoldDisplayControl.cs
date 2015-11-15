using UnityEngine;
using System.Collections;

public class FL_GoldDisplayControl : MonoBehaviour 
{
	private int myGold = 0;
	public TextMesh myGoldDisplay;
	private float _countUpdate = 0f;

	void Start () 
	{
		myGoldDisplay = transform.Find ("textGoldAmount").GetComponent<TextMesh> ();
		myGold = GameGlobalVariables.Stats.METAL_IN_CONTAINERS;
		myGoldDisplay.text = myGold.ToString();
	}
	
	// Update is called once per frame
	void Update () 
	{
		_countUpdate -= Time.deltaTime;
		
		if ( _countUpdate > 0f ) return;
		
		_countUpdate = 0.1f;

		if(myGold != GameGlobalVariables.Stats.METAL_IN_CONTAINERS)
		{
			myGold = GameGlobalVariables.Stats.METAL_IN_CONTAINERS;
			myGoldDisplay.text = myGold.ToString();
		}

		if(GameGlobalVariables.Stats.METAL_IN_CONTAINERS < 0)
		{
			GameGlobalVariables.Stats.METAL_IN_CONTAINERS = 0;
		}
		else if(GameGlobalVariables.Stats.METAL_IN_CONTAINERS > GameGlobalVariables.GOLD_CAP)
		{
			GameGlobalVariables.Stats.METAL_IN_CONTAINERS = GameGlobalVariables.GOLD_CAP;
		}
	}
}
