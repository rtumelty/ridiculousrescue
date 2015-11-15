using UnityEngine;
using System.Collections;

public class IComponent : MonoBehaviour 
{
	//*************************************************************//	
	public int myID;
	public int[] position;
	public int[][] aditionalTiles;
	public CharacterData myCharacterData;
	public EnemyData myEnemyData;
	//*************************************************************//	
	protected void Awake ()
	{
		position = new int[2] { Mathf.RoundToInt ( transform.root.position.x ), Mathf.RoundToInt ( transform.root.position.z + 0.5f )};
	}
}
