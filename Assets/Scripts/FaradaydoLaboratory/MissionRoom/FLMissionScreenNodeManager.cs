using UnityEngine;
using System.Collections;

public class FLMissionScreenNodeManager : MonoBehaviour 
{
	//*************************************************************//
	public const int TYPE_REGULAR_NODE = 0;
	public const int TYPE_BONUS_NODE = 1;
	public const int TYPE_MINING_NODE = 2;
	public const int TYPE_TRAIN_NODE = 3;
	//*************************************************************//
	public int type = 0;
	public FLMissionScreenNodeManager[] unlockingNodes;
	public bool unlocked = false;
	//*************************************************************//
	public void startUnlockingProcedure ( bool forceStart = false ) 
	{
		UnlockLevelSequenceManager sequanceManger = GetComponent < UnlockLevelSequenceManager > ();
		if ( sequanceManger == null ) sequanceManger = gameObject.AddComponent < UnlockLevelSequenceManager > ();

		if ( forceStart )
		{
			GameGlobalVariables.BLOCK_LAB_ENTERED = false;
			GameGlobalVariables.LAB_ENTERED = 2;
			SaveDataManager.save ( SaveDataManager.LABORATORY_ENTERED, GameGlobalVariables.LAB_ENTERED );

			Destroy ( sequanceManger );
			sequanceManger = gameObject.AddComponent < UnlockLevelSequenceManager > ();
		}
	}
}
