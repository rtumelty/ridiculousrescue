using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class MemoryManager : MonoBehaviour 
{
	//*************************************************************//
	private class RecordClass
	{
		//*************************************************************//
		public int ID;
		public int animationID;
		public int animationPositionInReseredID;
		//*************************************************************//
		public RecordClass ( int IDValue, int animationIDValue, int positionIDValue )
		{
			ID = IDValue;
			animationID = animationIDValue;
			animationPositionInReseredID = positionIDValue;
		}
	}
	//*************************************************************//
	public List < List < Texture2D >> reservedTextures;
	//*************************************************************//
	private List < RecordClass > _record;
	//*************************************************************//
	private static MemoryManager _meInstance;
	public static MemoryManager getInstance ()
	{
		if ( _meInstance == null )
		{
			_meInstance = GameObject.Find ( "__MEMORY_MANAGER" ).GetComponent < MemoryManager > ();
		}
		
		return _meInstance;
	}
	//*************************************************************//	
	void Awake () 
	{
		reservedTextures = new List < List < Texture2D >> ();
		_record = new List < RecordClass > ();
	}
	
	void Start ()
	{
		clean ();
	}

	public void clean ()
	{
		Resources.UnloadUnusedAssets ();
		GC.Collect ();
	}
	
	public int reserveTexturesForMeAndGiveMeID ( int ID, int animationID, string path )
	{
		foreach ( RecordClass record in _record )
		{
			if ( record.ID == ID && record.animationID == animationID )
			{
				return record.animationPositionInReseredID;
			}
		}
		
		reservedTextures.Add ( new List < Texture2D > ());
		UnityEngine.Object[] textureObjects = Resources.LoadAll ( path, typeof ( Texture2D ));
		foreach ( UnityEngine.Object textureObject in textureObjects )
		{
			if ( textureObject is Texture2D )
			{
				reservedTextures[reservedTextures.Count - 1].Add (( Texture2D ) textureObject );
			}
		}
		
		_record.Add ( new RecordClass ( ID, animationID, reservedTextures.Count - 1 ));
		return reservedTextures.Count - 1;

	}
}
