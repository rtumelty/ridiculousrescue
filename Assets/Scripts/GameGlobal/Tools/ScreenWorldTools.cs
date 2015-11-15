using UnityEngine;
using System.Collections;

/**
 * Framework Class
 * @author Jerry 
 */

public static class ScreenWorldTools 
{	
	public static Vector3 getWorldPointOnMeshFromScreenEveryLayer ( Vector2 pointOnScreenValue )
	{
		Ray ray = Camera.main.ScreenPointToRay ( pointOnScreenValue ); 
		
		Vector3 hitPoint = Vector3.zero;
					
		RaycastHit hit;
		
		int layerMask = ( 1 << GlobalVariables.Layers.GROUND );
		
		if ( Physics.Raycast ( ray, out hit, 50f, layerMask ))
		{
			hitPoint = hit.point;
		}
		
		return ( hitPoint );
	}
	
	public static GameObject getGameObjectFromScreenEveryLayer ( Vector2 pointOnScreenValue )
	{
		Ray ray = Camera.main.ScreenPointToRay ( pointOnScreenValue ); 
		
		GameObject hitObject = null;
					
		RaycastHit hit;
		
		//int layerMask = ( 1 << GlobalVariables.Layers.GROUND );
		
		if ( Physics.Raycast ( ray, out hit, 50f /*, layerMask*/ ))
		{
			hitObject = hit.collider.gameObject;
		}
		
		return ( hitObject );
	}
	
	public static GameObject getObjectOnPath ( Vector3 sourcePosition, Vector3 direction, float distance )
	{	
		RaycastHit hit;	
		
		if ( Physics.Raycast ( sourcePosition, direction, out hit, distance ))
		{
			return hit.transform.root.gameObject;	
		}
		
		return null;
	}
}
