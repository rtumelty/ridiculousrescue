using UnityEngine;
using System.Collections;

public class starsNotifierUIControl : MonoBehaviour 
{
	private Vector3 startPosition;
	private Vector3 endPosition;

	private GameObject starIconObject;

	private GameObject charIconObject;
	public Texture coraIcon;
	public Texture madraIcon;
	public Texture faraIcon;
	public Texture bozIcon;
	public Texture timeIcon;

	private TextMesh part01;
	private TextMesh part02;

	private bool popIn = false;
	private bool popOut = false;
	private bool changeColour = false;
	private bool lastPopUpComplete = true;

	void Start () 
	{
		startPosition = transform.position;
		endPosition = new Vector3 (startPosition.x, startPosition.y, startPosition.z + 1.7f);
		starIconObject = transform.Find ("starObject").gameObject;
		charIconObject = transform.Find ("charIcon").gameObject;
	}

	void Update () 
	{
		if(Input.GetKeyDown("u"))
		{
			StartCoroutine("PopInPopOut");
		}
		if(popIn == true)
		{
			popOut = false;
			transform.position = Vector3.MoveTowards(startPosition, endPosition, Time.deltaTime * 2f);
		}
		if(popOut == true)
		{
			popIn = false;
			transform.position = Vector3.MoveTowards(endPosition, startPosition, Time.deltaTime * 2f);
		}

	}

	IEnumerator PopInPopOut ()
	{
		if(lastPopUpComplete == true)
		{
			lastPopUpComplete = false;
			popIn = true;

			yield return new WaitForSeconds (2f);
			popIn = false;
			changeColour = true;

			yield return new WaitForSeconds (2f);
			popOut = true;

			yield return new WaitForSeconds (2f);
			popOut = false;
			changeColour = false;
			lastPopUpComplete = true;
		}
		else
		{
			yield return new WaitForSeconds (0.5f);
			StartCoroutine("PopInPopOut");
		}
	}
}
