using UnityEngine;
using System.Collections;

public class VisibleCheck : MonoBehaviour 
{
	void Update () 
	{
		if ( ! renderer.isVisible )
		{
			Camera.main.transform.Translate ( new Vector3 (( transform.position.x - Camera.main.transform.position.x ) * Time.deltaTime * 0.2f, ( transform.position.z - Camera.main.transform.position.z ) * Time.deltaTime * 0.2f, 0f ));
		}
	}
}
