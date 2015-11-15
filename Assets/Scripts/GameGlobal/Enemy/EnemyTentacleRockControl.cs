using UnityEngine;
using System.Collections;

public class EnemyTentacleRockControl : MonoBehaviour 
{
	//*************************************************************//
	public GameObject parentGameObject;
	public Vector3 addPosition = Vector3.forward * 0.95f;
	//*************************************************************//
	void LateUpdate () 
	{
		if ( parentGameObject.activeSelf == false ) this.gameObject.SetActive(false);
		else transform.position = parentGameObject.transform.position + Vector3.up * 0.25f + addPosition;
	}
}
