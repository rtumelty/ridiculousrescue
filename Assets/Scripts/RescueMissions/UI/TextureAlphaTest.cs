using UnityEngine;
using System;
using System.Collections;
using System.Runtime.CompilerServices;

public class TextureAlphaTest
{
	public static bool testPixelAlpha ( Vector2 screenPosition, GameObject objectWithTexture, float pMinAlpha = 0.5f)
	{
		Vector2 lPixelPosition = screenPosition;
		Texture2D lTexture2D = ( Texture2D ) objectWithTexture.renderer.material.mainTexture;
		
		MonoBehaviour.print(lTexture2D.GetPixel(Convert.ToInt32(lPixelPosition.x ), Convert.ToInt32(lPixelPosition.y)).a + " : " + lTexture2D);
		if(lTexture2D != null)
			if(lTexture2D.GetPixel(Convert.ToInt32(lPixelPosition.x ), Convert.ToInt32(lPixelPosition.y)).a < pMinAlpha)
				return true;
		return false;
	}
	
	public static float showPixelAlpha ( Vector2 screenPosition, GameObject objectWithTexture, float pMinAlpha = 0.5f)
	{
		Vector2 lPixelPosition = screenPosition;
		Texture2D lTexture2D = ( Texture2D ) objectWithTexture.renderer.material.mainTexture;
		
		return (lTexture2D.GetPixel(Convert.ToInt32(lPixelPosition.x ), Convert.ToInt32(lPixelPosition.y)).a);
	}
}
