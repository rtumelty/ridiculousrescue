using UnityEditor;
using UnityEngine;

public class VersionControlWindow : EditorWindow 
{
	private string _versionString;
	
	[ MenuItem ( "Version/Version Window" )]
	public static void ShowWindow ()
	{
		EditorWindow.GetWindow ( typeof ( VersionControlWindow ));
	}
	
	void OnGUI ()
	{
		_versionString = EditorGUI.TextField ( new Rect ( 10f, 10f, 200f, 25f ), _versionString );
		if ( GUI.Button ( new Rect ( 10f, 40f, 100f, 25f ), "SET" ))
		{
			// do something with all versions
		}
	}
}
