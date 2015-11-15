using UnityEngine;
using System.Collections;

public class PersistThroughSceneComponent : MonoBehaviour 
{
	public static string lastActiveNode = "";
	private static PersistThroughSceneComponent _meInstance;
	public static PersistThroughSceneComponent getInstance ()
	{
		return _meInstance;
	}

	void Awake ()
	{
		_meInstance = this;
		DontDestroyOnLoad (gameObject);
	}

	void Update ()
	{
		if(Input.GetKeyDown("space"))
		{
			print (lastActiveNode);
		}
	}
}