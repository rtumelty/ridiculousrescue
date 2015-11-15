using UnityEngine;
using System.Collections;

public class CheckInputType : MonoBehaviour 
{
	public const int NO_TYPE = -1;
	public const int TAP_TYPE = 0;
	public const int DRAG_TYPE = 1;
	public const int DUALTOUCH_TYPE = 2;

	public TextMesh sample1;
	public TextMesh sample2;

	public bool justZoomed = false;
	private bool iAmDragging = false;
	private bool dualTouch = false;
	public static bool allowMomentum = false;

	private Vector3 myInputPos;

	public int touchType;
	public int lastType;
	private const float TAP_TIME = 0.3f;
	[SerializeField] private float checkForTapRelease = 0.3f;

	private static CheckInputType meInstance;
	public static CheckInputType getInstance()
	{
		if(meInstance == null)
		{
			meInstance = GameObject.Find ("_MainObject").GetComponent <CheckInputType>();
		}
		return meInstance;
	}


	void Start ()
	{
		sample1 = GameObject.Find ("info1").GetComponent<TextMesh> ();
		sample2 = GameObject.Find ("info2").GetComponent<TextMesh> ();
	}


	void Update()
	{
		#if UNITY_ANDROID
		if (ChartboostSDK.Chartboost.isImpressionVisible()) return;
		#endif

//		print ("MOMO"+ allowMomentum);
		//================== START OF ZOOM CHECK ==================
#if UNITY_EDITOR
		if(false)//Zoom control not complete for Editor, temp false.
#else
		if (Input.touchCount == 0) return;
		if(Input.touchCount > 1)
#endif
		{
		//	sample1.text = "Touches > 1";
			dualTouch = true;
			touchType = DUALTOUCH_TYPE;
		}
		else
		{
			dualTouch = false;
		}
		//================== START OF ZOOM CHECK ==================



		//================== START OF DRAG CHECK ==================
#if UNITY_EDITOR
		if(Input.GetMouseButtonDown(0))
		
#else
		if(Input.touches[0].phase == TouchPhase.Began)
				
#endif
		{
			//sample2.text = "Phase Begin";
			ZoomAndLevelDrag.getInstance().momentumHolderX = 0;
			allowMomentum = false;
			ZoomAndLevelDrag.getInstance().allowFollow = false;
			
			myInputPos =  VectorTools.cloneVector3(Input.mousePosition);
		}
		//================== END OF DRAG CHECK ====================



		//================== START OF TAP CHECK ===================
#if UNITY_EDITOR
		if(Input.GetMouseButton(0))

#else
		if(Input.touchCount == 1)

#endif
		{
		//	sample1.text = "Touches = 1";
			checkForTapRelease -= Time.deltaTime;
			if(/*checkForTapRelease > 0 &&*/ !dualTouch && !iAmDragging)
			{
				touchType = TAP_TYPE;
			}
			else if(/*checkForTapRelease <= 0 &&*/ !dualTouch && !iAmDragging)
			{
				touchType = NO_TYPE;
			}
//			print (Vector3.Distance(Input.mousePosition, myInputPos));

			if(Vector3.Distance(Input.mousePosition, myInputPos) > 9.5f && justZoomed == false)
			{
				iAmDragging = true;
				touchType = DRAG_TYPE;
			}
			else
			{
				iAmDragging = false;
			}
			lastType = touchType;
		}
		//================== END OF TAP CHECK ===================


#if UNITY_EDITOR
		if(Input.GetMouseButtonUp(0))

#else
		if(Input.touches[0].phase == TouchPhase.Ended)

#endif	
		{
			if(touchType == DUALTOUCH_TYPE)
			{
				justZoomed = true;
				StartCoroutine("ZoomToFlickDampen");
			}
			StartCoroutine ("WipeTouchStatus");
			//sample2.text = "Phase end";

			allowMomentum = true;

			if(checkForTapRelease > 0 && !dualTouch && !iAmDragging)
			{
				touchType = TAP_TYPE;
			}
			else
			{
				touchType = NO_TYPE;
			}
			checkForTapRelease = TAP_TIME;
		}
	}

	IEnumerator WipeTouchStatus()
	{
		yield return new WaitForSeconds (0.1f);
		touchType = NO_TYPE;
		yield return new WaitForSeconds (3.9f);
		allowMomentum = false;
	}

	IEnumerator ZoomToFlickDampen()
	{
		yield return new WaitForSeconds (0.1f);
		justZoomed = false;
	}
}