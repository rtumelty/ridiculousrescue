using UnityEngine;
using System.Collections;

public class TimeScaleManager
{
	public static string getTimeString ( int seconds )
	{
		if ( seconds > 60 * 60 * 24 )
		{
			int days = (int) ( seconds / ( 60 * 60 * 24 ));
			int hours = (int) ( seconds - ( days * 60 * 60 * 24 )) / ( 60 * 60 );
			
			return days.ToString () + "d " + ( hours == 0 ? "" : hours.ToString () + "h" );
		}
		else if ( seconds > 60 * 60 )
		{
			int hours = (int) ( seconds / ( 60 * 60 ));
			int minutes = (int) ( seconds - ( hours * 60 * 60 )) / ( 60 );
			
			return hours.ToString () + "h " + ( minutes == 0 ? "" : minutes.ToString () + "m" );
		}
		else if ( seconds > 60 )
		{
			int minutes = seconds / 60 ;
			int secondsLeft = ( seconds - ( minutes * 60 ));
			
			return minutes.ToString () + "m " + ( secondsLeft == 0 ? "" : secondsLeft.ToString () + "s" );
		}
		else
		{
			return seconds.ToString () + "s ";
		}
	}

	public static string getDigitalTimeString ( int seconds )
	{
		if ( seconds > 60 * 60 * 24 )
		{
			int days = (int) ( seconds / ( 60 * 60 * 24 ));
			int hours = (int) ( seconds - ( days * 60 * 60 * 24 )) / ( 60 * 60 );
			
			return days.ToString () + "" + ( hours == 0 ? "" : hours.ToString () + "" );
		}
		else if ( seconds > 60 * 60 )
		{
			int hours = (int) ( seconds / ( 60 * 60 ));
			int minutes = (int) ( seconds - ( hours * 60 * 60 )) / ( 60 );
			
			return hours.ToString () + "" + ( minutes == 0 ? "" : minutes.ToString () + "" );
		}
		else if ( seconds > 60 )
		{
			int minutes = seconds / 60 ;
			int secondsLeft = ( seconds - ( minutes * 60 ));
			
			return minutes.ToString () + ":" + ( secondsLeft == 0 ? "" : secondsLeft.ToString () );
		}
		else
		{
			return "00:" + seconds.ToString ();
		}
	}
}
