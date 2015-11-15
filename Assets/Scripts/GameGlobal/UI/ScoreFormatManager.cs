using UnityEngine;
using System.Collections;

public class ScoreFormatManager
{
	public static string returnFormatedScoreString (int unformattedScore)
	{
		string myString = "Some bug";
		if(unformattedScore < 1000)
		{
			myString = unformattedScore.ToString();
		}
		else if(unformattedScore > 999 && unformattedScore < 10000)
		{
			myString = unformattedScore.ToString().Substring(0,1) + "," + unformattedScore.ToString().Substring(1,3);
		}
		else if(unformattedScore > 9999 && unformattedScore < 1000000)
		{
			myString = unformattedScore.ToString().Substring(0,2) + "," + unformattedScore.ToString().Substring(2,3);
		}
		else if(unformattedScore > 99999 && unformattedScore < 10000000)
		{
			myString = unformattedScore.ToString().Substring(0,3) + "," + unformattedScore.ToString().Substring(3,3);
		}
		else if(unformattedScore > 999999 )
		{
			myString = "999999";
		}
		return myString;
	}
}
