using UnityEngine;
using System.Collections;

public class GameTextControl : MonoBehaviour 
{
	//*************************************************************//
	public string myKey;
	public int lineLength = 0;
	public string addText = "";
	public string addPreText = "";
	public string minusText = "";
	public string characterName = "";
	//*************************************************************//
	private TextMesh _myText;
	private string _previousText;
	//*************************************************************//
	void Awake () 
	{
		_myText = gameObject.GetComponent < TextMesh > ();
		_myText.text = "";
	}
	
	void Start ()
	{
		GameTextManager.getInstance ().registerGameText ( this );
		updateText ();
	}
	
	void Update () 
	{
		if ( myKey == "" ) return;
		
		string completeText = "";
		if ( lineLength != 0 )
		{
			string[] words = GameTextManager.getInstance ().getText ( myKey, characterName ).Split ( " "[0] );
			
			string line = "";
			foreach ( string word in words )
			{
				line += ( word + " " );
				if ( line.Length >= lineLength )
				{
					completeText += line + "\n";
					line = "";
				}
			}
			completeText += line;
		}
		else
		{
			completeText = GameTextManager.getInstance ().getText ( myKey, characterName );
		}
		
		if ( minusText != "" ) completeText = completeText.Replace ( minusText, "" );
		completeText += addText;
		completeText = (addPreText + completeText);
		if ( _previousText != completeText )
		{
			updateText ();
		}
	}
	
	public void updateText ()
	{
		if ( myKey == "" ) return;
		
		string completeText = "";
		if ( lineLength != 0 )
		{
			string[] words = GameTextManager.getInstance ().getText ( myKey, characterName ).Split ( " "[0] );
			
			string line = "";
			foreach ( string word in words )
			{
				line += ( word + " " );
				if ( line.Length >= lineLength )
				{
					completeText += line + "\n";
					line = "";
				}
			}
			
			completeText += line;
		}
		else
		{
			completeText = GameTextManager.getInstance ().getText ( myKey, characterName );
		}
		
		if ( minusText != "" ) completeText = completeText.Replace ( minusText, "" );
		_myText.text = addPreText + completeText + addText;
		_previousText = _myText.text;
	}
}
