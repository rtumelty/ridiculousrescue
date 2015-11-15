using UnityEngine;
using System.Collections;

public class FollowObject : MonoBehaviour 
{
	public Transform target;

	void LateUpdate () 
	{
		transform.position = target.position;
	}
}
