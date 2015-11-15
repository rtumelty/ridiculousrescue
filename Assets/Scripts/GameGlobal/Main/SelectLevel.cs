using UnityEngine;
using System;
using System.Xml;
using System.Collections;
using System.Collections.Generic;

public class SelectLevel : MonoBehaviour 
{
	//*************************************************************//	
	public static Dictionary < int, string > ALL_LEVELS;
	public Dictionary < int, string > myEntry;
	public static Dictionary < int, string > ALL_BONUS_LEVELS;
	private int myXMLLevelID = 0;
	private int myOldLevel = 0;
	//*************************************************************//	
	public int gamePartID; 
	public int mnLevelID;
	public Dictionary < int, string >[] myDictionarys;
	private List < XmlDocument > _levelsFromDatabase;
	private Vector2 _scrollPosition;
	//*************************************************************//	
	//======================================Daves edit======================================
	public string ReplaceFirst(string text1, string search, string replace)
	{
		int pos = text1.IndexOf(search);
		if (pos < 0)
		{
			return text1;
		}
		return text1.Substring(0, pos) + replace + text1.Substring(pos + search.Length);
	}

	public int GetLevelID(string text2, string search, string replace, string endMark)
	{
		int pos = text2.IndexOf(search);
		if (pos < 0)
		{
			return pos;
		}
		string output = text2.Substring(0, pos) + replace + text2.Substring(pos + search.Length);
		output = output.Substring(0, output.IndexOf(endMark));
//		print (output);
		return int.Parse(output);
	}
	public int GetVariantID(string text3, string startMark, string endMark)
	{
		int pos = text3.IndexOf(startMark);
		int endPos = text3.IndexOf (endMark);
		if (pos < 0)
		{
			return pos;
		}
		int lengthOfID = (text3.Substring (0, endPos).Length) - (text3.Substring (0, pos).Length);
		string output = text3.Substring(pos + 1, lengthOfID - 1);
		return int.Parse(output);
	}
	//======================================Daves edit======================================

	void Start () 
	{
		myDictionarys = new Dictionary < int, string > [100];

		if ( ALL_LEVELS != null ) 
		{
			switch ( gamePartID )
			{
				case 0:
					Application.LoadLevel ( "01" );
					break;
				case 1:
					Application.LoadLevel ( "FL01" );
					break;
				case 2:
					break;
			}
		}
		else
		{
			ALL_LEVELS = new Dictionary < int, string > ();
			ALL_BONUS_LEVELS = new Dictionary < int, string > ();

			_levelsFromDatabase = new List < XmlDocument > ();

			if ( GameGlobalVariables.RELEASE )
			{
				TextAsset levelsStringTextAsset = ( TextAsset ) Resources.Load ( "GameText/levels", typeof ( TextAsset ));
				string levelsString = WWW.UnEscapeURL ( levelsStringTextAsset.text );
				
				string[] levels = levelsString.Split ( new char[] { '|' });
				int mapID = 0;

				foreach ( string level in levels )
				{
//					print (level);
					if ( level.Length > 400 )
					{
						XmlDocument levelXml = new XmlDocument ();
						levelXml.LoadXml ( level );
						_levelsFromDatabase.Add ( levelXml );

						if ( levelXml.ChildNodes[1].ChildNodes[1].InnerText.Contains ( "long" ))
						{
							string longLevelnumberString = levelXml.ChildNodes[1].ChildNodes[1].InnerText.Replace ( "long", "" );
							int longLevelnumber = 0;
							if ( int.TryParse ( longLevelnumberString, out longLevelnumber ))
							{
								ALL_LEVELS.Add ( longLevelnumber, levelXml.InnerXml );
							}
							else
							{
								string bonusLevelnumberString = levelXml.ChildNodes[1].ChildNodes[1].InnerText.Replace ( "bon", "" );
								int bonusLevelnumber = 0;
								if ( int.TryParse ( bonusLevelnumberString, out bonusLevelnumber ))
								{
									ALL_BONUS_LEVELS.Add ( bonusLevelnumber, levelXml.InnerXml );
								}
							}
						}

						else if ( levelXml.ChildNodes[1].ChildNodes[1].InnerText.Contains ( "extra" ))
						{
							string bonusLevelnumberString = levelXml.ChildNodes[1].ChildNodes[1].InnerText.Replace ( "extra", "" );
							int bonusLevelnumber = 0;
							if ( int.TryParse ( bonusLevelnumberString, out bonusLevelnumber ))
							{
								ALL_BONUS_LEVELS.Add ( bonusLevelnumber, levelXml.InnerXml );
							}
						}
						
					}

					switch ( gamePartID )
					{
						case 0:
							Application.LoadLevel ( "01" );
							break;
						case 1:
							Application.LoadLevel ( "FL01" );
							break;
						case 2:
							break;
					}
				}
			}
		}
	}
}
