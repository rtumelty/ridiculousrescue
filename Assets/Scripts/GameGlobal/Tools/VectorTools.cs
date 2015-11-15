using UnityEngine;
using System.Collections;

/**
 * Framework Class
 * @author Jerry 
 */

public static class VectorTools
{
	public static float[] getNormalizedVectors2 (float vector01, float vector02)
	{
		float normalizedVector01;
		float normalizedVector02;
		
		if (Mathf.Abs(vector01) > Mathf.Abs(vector02))
		{
			if (Mathf.Abs(vector01) >= 1.0f)
			{
				normalizedVector01 = vector01 / Mathf.Abs(vector01);
				normalizedVector02 = vector02 / Mathf.Abs(vector01);
			}
			else
			{
				normalizedVector01 = vector01;
				normalizedVector02 = vector02 * Mathf.Abs(vector01);
			}
		}
		else if (Mathf.Abs(vector01) < Mathf.Abs(vector02))
		{
			if (Mathf.Abs(vector02) >= 1.0f)
			{
				normalizedVector01 = vector01 / Mathf.Abs(vector02);
				normalizedVector02 = vector02 / Mathf.Abs(vector02);
			}
			else
			{
				normalizedVector01 = vector01 * Mathf.Abs(vector02);
				normalizedVector02 = vector02;
			}
		}
		else if ((Mathf.Abs(vector01) == Mathf.Abs(vector02)) && (vector01 != 0.0f))
		{
			if (Mathf.Abs(vector01) >= 1.0f)
			{
				normalizedVector01 = vector01 / Mathf.Abs(vector01);
				normalizedVector02 = vector02 / Mathf.Abs(vector01);
			}
			else
			{
				normalizedVector01 = vector01;
				normalizedVector02 = vector02;
			}				
		}
		else
		{			
			normalizedVector01 = normalizedVector02 = 0.0f;
		}
		
		float[] vector2 = new float[2];
		vector2[0] = normalizedVector01;
		vector2[1] = normalizedVector02;
		
		return (vector2);
	}
	
	public static float[] getNormalizedVectors2Clamp01 (float vector01, float vector02)
	{
		float normalizedVector01;
		float normalizedVector02;
		
		if (vector01 > 0) normalizedVector01 = 1.0f;
		else if (vector01 < 0) normalizedVector01 = -1.0f;
		else normalizedVector01 = 0.0f;
		
		if (vector02 > 0) normalizedVector02 = 1.0f;
		else if (vector02 < 0) normalizedVector02 = -1.0f;
		else normalizedVector02 = 0.0f;
			
		float[] vector2 = new float[2];
		vector2[0] = normalizedVector01;
		vector2[1] = normalizedVector02;
		
		return (vector2);
	}
	
	public static Vector3 cloneVector3 ( Vector3 toBeClonedVector3, float addX = 0f, float addY = 0f, float addZ = 0f )
	{
		Vector3 returnVector3;
		
		returnVector3.x = toBeClonedVector3.x + addX;
		returnVector3.y = toBeClonedVector3.y + addY;;
		returnVector3.z = toBeClonedVector3.z + addZ;;
		
		return returnVector3;
	}
	
	public static Vector3 cloneVector3Override(Vector3 toBeClonedVector3, float oX = float.NaN, float oY = float.NaN, float oZ = float.NaN)
	{
		Vector3 returnVector3 = new Vector3();
		
		if(!float.IsNaN(oX)) returnVector3.x = oX;
		else returnVector3.x = toBeClonedVector3.x;
		
		
		if(!float.IsNaN(oY)) returnVector3.y = oY;
		else returnVector3.y = toBeClonedVector3.y;
		
		
		if(!float.IsNaN(oZ)) returnVector3.z = oZ;
		else returnVector3.z = toBeClonedVector3.z;
		
		return returnVector3;
	}
	
	public static Rect cloneRect ( Rect toBeClonedRect, float addX = 0f, float addY = 0f, float addWitdh = 0f, float addHeight = 0f )
	{
		Rect returnRect = new Rect ();
		
		returnRect.x = toBeClonedRect.x + addX;
		returnRect.y = toBeClonedRect.y + addY;;
		returnRect.width = toBeClonedRect.width + addWitdh;
		returnRect.height = toBeClonedRect.height + addHeight;
		
		return returnRect;
	}
	
	public static Color cloneColor ( Color toBeClonedColor, float addR = 0f, float addG = 0f, float addB = 0f, float addA = 0f )
	{
		Color returnColor;
		
		returnColor.r = toBeClonedColor.r + addR;
		returnColor.g = toBeClonedColor.g + addG;
		returnColor.b = toBeClonedColor.b + addB;
		returnColor.a = toBeClonedColor.a + addA;
		
		return returnColor;
	}
	
	public static Quaternion cloneQuaternion ( Quaternion toBeClonedQuaternion, float addX = 0f, float addY = 0f, float addZ = 0f, float addW = 0f )
	{
		Quaternion returnQuaternion;
		
		returnQuaternion.x = toBeClonedQuaternion.x + addX;
		returnQuaternion.y = toBeClonedQuaternion.y + addY;
		returnQuaternion.z = toBeClonedQuaternion.z + addZ;
		returnQuaternion.w = toBeClonedQuaternion.w + addW;
		
		return returnQuaternion;
	}
}
