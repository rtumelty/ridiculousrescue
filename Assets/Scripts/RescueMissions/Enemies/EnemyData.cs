using UnityEngine;
using System.Collections;

public class EnemyData 
{
	//*************************************************************//	
	public const int ENEMY_ACTION_TYPE_ENDURANCE = 0;
	public const int ENEMY_ACTION_TYPE_MOVING = 1;
	public const int ENEMY_ACTION_TYPE_MOVING_RANGE = 2;
	public const int ENEMY_ACTION_TYPE_DAMAGE = 3;
	public const int ENEMY_ACTION_TYPE_ATTACK_RANGE = 4;
	public const int ENEMY_ACTION_TYPE_LENGHT_TIME_BAR = 5;
	public const int ENEMY_ACTION_TYPE_TIME_BAR_SPEED = 6;
	//*************************************************************//	
	public int myID;
	public string name;
	public int[] enemyValues;
	public bool attacking;
	public int myHoleID = -1;
	public bool dieInProgress = false;
	//*************************************************************//	
	public EnemyData ( int IDValue, string nameValue, int enduranceValue, int movingValue, int movingRangeValue, int damageValue, int attackRangeValue, int lenghtOtTimeBarValue, int timeBarSpeedValue )
	{
		this.myID = IDValue;
		this.name = nameValue;
		enemyValues = new int[] { enduranceValue, movingValue, movingRangeValue, damageValue, attackRangeValue, lenghtOtTimeBarValue, timeBarSpeedValue };
	}
}
