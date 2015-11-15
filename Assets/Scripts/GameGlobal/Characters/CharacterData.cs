using UnityEngine;
using System.Collections;

public class CharacterData 
{
	//*************************************************************//	
	public const int CHARACTER_ACTION_TYPE_POWER = 0;
	public const int CHARACTER_ACTION_TYPE_DAMAGE = 1;
	public const int CHARACTER_ACTION_TYPE_RANGE = 2;
	public const int CHARACTER_ACTION_TYPE_AREA = 3;
	public const int CHARACTER_ACTION_TYPE_ATTACK_RATE = 4;
	public const int CHARACTER_ACTION_TYPE_BUILD_RATE = 5;
	public const int CHARACTER_ACTION_TYPE_DEMOLISH_RATE = 6;
	public const int CHARACTER_ACTION_TYPE_RAPAIR_RATE = 7;
	public const int CHARACTER_ACTION_TYPE_RESCUING = 8;
	//*************************************************************//	
	public int myID;
	public bool selected = false;
	public string name;
	public bool blocked = false;
	public bool moving = false;
	public bool interactAction
	{
		set
		{
			_interactAction = value;
		}
		get
		{
			return _interactAction;
		}
	}
	private bool _interactAction = false;
	//=====================================Daves Edit========================================
	public bool _interactTrolley = false;
	//=====================================Daves Edit========================================
	public bool attacking = false;
	public bool doNotUnblockThisCharacter = false;
	public bool coraSlidingTroley = false;
	public int[] position;
	public int[] characterValues;
	public Main.HandleChoosenCharacter myCallBack;
	public CharacterButton myCharacterButton;
	public int carryCompacity;
	public int totalMovesPerfromed;
	public GameObject characterObject;
	public GameObject myCurrentAttackBar;
	public float countTimeToNextPossibleAttack = 0f;
	public GameObject myTileMarker;
	public float countTimeToNextPossibleMoveAfterAttack = 0f;
	//*************************************************************//	
	public CharacterData ( int IDValue, string nameValue, int x, int z, int powerValue, int damageValue, int rangeValue, int areaValue, int attackRateValue, int buildRateValue, int demolishRateValue, int repairRateValue, int rescuingValue, int carryCompacityValue = 10 )
	{
		this.myID = IDValue;
		this.name = nameValue;
		position = new int[2] { x, z };
		carryCompacity = carryCompacityValue;
		characterValues = new int[] { powerValue, damageValue, rangeValue, areaValue, attackRateValue, buildRateValue, demolishRateValue, repairRateValue, rescuingValue };
	}
	
	public CharacterData getClone ()
	{
		CharacterData characterToReturn = new CharacterData ( myID, name, position[0], position[1], characterValues[CHARACTER_ACTION_TYPE_POWER], characterValues[CHARACTER_ACTION_TYPE_DAMAGE],
			characterValues[CHARACTER_ACTION_TYPE_RANGE], characterValues[CHARACTER_ACTION_TYPE_AREA], characterValues[CHARACTER_ACTION_TYPE_ATTACK_RATE], characterValues[CHARACTER_ACTION_TYPE_BUILD_RATE],
			characterValues[CHARACTER_ACTION_TYPE_DEMOLISH_RATE], characterValues[CHARACTER_ACTION_TYPE_RAPAIR_RATE], characterValues[CHARACTER_ACTION_TYPE_RESCUING] );
		
		return characterToReturn;
	}
}
