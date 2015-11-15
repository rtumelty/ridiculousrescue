using UnityEngine;
using System.Collections;

public class FLGarageRoomLazarusGridFiller : MonoBehaviour 
{

	void Start () 
	{
		FLLevelControl.getInstance ().levelGrid[FLLevelControl.GRID_LAYER_NORMAL][29][11] = GameElements.UNWALKABLE;
		FLLevelControl.getInstance ().levelGrid[FLLevelControl.GRID_LAYER_NORMAL][29 + 1][11] = GameElements.UNWALKABLE;
		FLLevelControl.getInstance ().levelGrid[FLLevelControl.GRID_LAYER_NORMAL][29 + 2][11] = GameElements.UNWALKABLE;
		FLLevelControl.getInstance ().levelGrid[FLLevelControl.GRID_LAYER_NORMAL][29 + 3][11] = GameElements.UNWALKABLE;
		FLLevelControl.getInstance ().levelGrid[FLLevelControl.GRID_LAYER_NORMAL][29 + 4][11] = GameElements.UNWALKABLE;

		FLLevelControl.getInstance ().levelGrid[FLLevelControl.GRID_LAYER_NORMAL][29][12] = GameElements.UNWALKABLE;
		FLLevelControl.getInstance ().levelGrid[FLLevelControl.GRID_LAYER_NORMAL][29 + 1][12] = GameElements.UNWALKABLE;
		FLLevelControl.getInstance ().levelGrid[FLLevelControl.GRID_LAYER_NORMAL][29 + 2][12] = GameElements.UNWALKABLE;
		FLLevelControl.getInstance ().levelGrid[FLLevelControl.GRID_LAYER_NORMAL][29 + 3][12] = GameElements.UNWALKABLE;
		FLLevelControl.getInstance ().levelGrid[FLLevelControl.GRID_LAYER_NORMAL][29 + 4][12] = GameElements.UNWALKABLE;
	}
}
