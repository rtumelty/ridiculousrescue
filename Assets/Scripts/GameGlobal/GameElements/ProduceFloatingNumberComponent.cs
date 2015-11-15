using UnityEngine;
using System.Collections;

public class ProduceFloatingNumberComponent : MonoBehaviour 
{
	public void produceFloatinNumber ( int number )
	{
		/*
		GameObject floatingNumberPrefab = ( GameObject ) Resources.Load ( "UI/floatingNumber" );
		GameObject floatingNumberObject = ( GameObject ) Instantiate ( floatingNumberPrefab, transform.position + Vector3.forward * 2f + Vector3.up * 5f, floatingNumberPrefab.transform.rotation );
		
		StartCoroutine ( "prepareFadeAfterTime", floatingNumberObject );
		*/
	}
	
	private IEnumerator prepareFadeAfterTime ( GameObject floatingNumberObject )
	{
		yield return new WaitForSeconds ( 0.2f );
		/*
		floatingNumberObject.transform.Find ( "floatingNumber(Clone)(Clone)" ).renderer.material = GameGlobalVariables.FontMaterials.BLACK_TEXT;
		Destroy ( floatingNumberObject.transform.Find ( "floatingNumber(Clone)(Clone)" ).GetComponent < AlphaDisapearAndDestory > ());
		AlphaDisapearAndDestory currentAlphaDisapearAndDestory = floatingNumberObject.transform.Find ( "floatingNumber(Clone)(Clone)" ).gameObject.AddComponent < AlphaDisapearAndDestory > ();
		currentAlphaDisapearAndDestory.timeToStartFading = floatingNumberObject.GetComponent < AlphaDisapearAndDestory > ().timeToStartFading;*/
	}
}
