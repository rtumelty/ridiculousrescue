using UnityEngine;
using System.Collections;

public class TRCleanMemoryAndStartTrainLevel : MonoBehaviour 
{
	void Start () 
	{
		MemoryManager.getInstance ().clean ();
		Application.LoadLevel ( "TR01" );
	}
}
