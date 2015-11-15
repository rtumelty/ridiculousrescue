using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class FLCharactersManager : MonoBehaviour 
{
	//*************************************************************//
	public static List < CharacterData > CHARACTERS_IN_LABORATORY;
	//*************************************************************//
	void Start () 
	{
		FLLevelControl.getInstance ().levelGrid[FLLevelControl.GRID_LAYER_NORMAL][3][4 + 8] = GameElements.CHAR_FARADAYDO_1_IDLE;
		FLLevelControl.getInstance ().levelGrid[FLLevelControl.GRID_LAYER_NORMAL][5 + 12][1 + 8] = GameElements.CHAR_CORA_1_IDLE;
		FLLevelControl.getInstance ().levelGrid[FLLevelControl.GRID_LAYER_NORMAL][7 + 12][1 + 8] = GameElements.CHAR_MADRA_1_IDLE;
		//=============================Daves Edit=============================
		FLLevelControl.getInstance ().levelGrid[FLLevelControl.GRID_LAYER_NORMAL][22 + 12][1 + 8] = GameElements.CHAR_MINER_4_IDLE;
		//=============================Daves Edit=============================
		FLLevelControl.getInstance ().fillLevelWithAssets ();
	}
}
