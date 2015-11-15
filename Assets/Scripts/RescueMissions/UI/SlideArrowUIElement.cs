using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class SlideArrowUIElement : MonoBehaviour 
{
	//*************************************************************//
	public List <int[]> targetTiles;
	public bool longPath = false;
	public bool fromTutorial = true;
	public bool fromLevelO1or02 = false;
	//*************************************************************//	
	private Vector3 _initialPosition;
	private int _stepOnPathID = 0;
	private float _countTimeToShowAgain = 0f;
	private bool _countTime = false;
	private int _countShows = 0;
	private int _countShowsGeneral = 0;
	//*************************************************************//	
	void Start () 
	{
		_initialPosition = VectorTools.cloneVector3 ( transform.position );
		onComplete ();
	}
	
	private void onCompleteGoingDown ()
	{
		if ( targetTiles[0].Length == 0 ) return;
		if ( ! fromLevelO1or02 )
		{
			if ( _countShows >= 1 )
			{
				_countShowsGeneral++;
				_countShows = 0;
				
				if ( fromTutorial ) _countTimeToShowAgain = 2f; 
				else
				{
					if ( _countShowsGeneral >= 2 )
					{
						_countTimeToShowAgain = 99999999f;
					}
					else
					{
						_countTimeToShowAgain = 2f;
					}
				}
				
				_countTime = true;
				transform.position = new Vector3 ( 100f, 100f, 100f );
				return;
			}
			
			_countShows++;
		}
		
		iTween.MoveTo ( this.gameObject, iTween.Hash ( "time", ( fromLevelO1or02 ? 0.5f : 1.0f ), "easetype", iTween.EaseType.easeOutQuad, "position", new Vector3 ((float) targetTiles[0][0], transform.position.y, (float) targetTiles[0][1] ), "oncomplete", "onComplete" ));
	}
	
	private void onComplete () 
	{
		if ( longPath )
		{
			if ( _stepOnPathID > targetTiles.Count - 1 )
			{
				_stepOnPathID = 0;
				_countTimeToShowAgain = TutorialsManager.getInstance ().getCurrentTutorialStep ().waitBeforeShowSlideArrow;
				_countTime = true;
				transform.position = new Vector3 ( 100f, 100f, 100f );
				return;
			}

			iTween.MoveTo ( this.gameObject, iTween.Hash ( "time", ( fromLevelO1or02 ? 0.5f : 1.0f ), "easetype", iTween.EaseType.easeOutQuad, "position", new Vector3 ((float) targetTiles[_stepOnPathID][0], transform.position.y, (float) targetTiles[_stepOnPathID][1] ), "oncomplete", "onComplete" ));
			_stepOnPathID++;
		}
		else
		{
			if ( fromTutorial && ! fromLevelO1or02 )
			{
				_countTimeToShowAgain = 5f;
				_countTime = true;
				//transform.position = new Vector3 ( 100f, 100f, 100f );
				iTween.MoveTo ( this.gameObject, iTween.Hash ( "time", ( fromLevelO1or02 ? 0.5f : 1.0f ), "easetype", iTween.EaseType.easeOutQuad, "position", new Vector3 ((float) targetTiles[0][0], transform.position.y, (float) targetTiles[0][1] ), "oncomplete", "onCompleteGoingDown" ));
			}
			else
			{
				_countTimeToShowAgain = TutorialsManager.getInstance ().getCurrentTutorialStep ().waitBeforeShowSlideArrow;
				_countTime = true;
				transform.position = new Vector3 ( 100f, 100f, 100f );
			}
		}
	}

	void Update ()
	{
		if ( ! _countTime ) return;
		_countTimeToShowAgain -= Time.deltaTime;

		if ( _countTimeToShowAgain <= 0f && longPath )
		{
			transform.position = VectorTools.cloneVector3 ( _initialPosition );
			_countTime = false;
			onComplete ();

		}
		else if ( _countTimeToShowAgain <= 0f && ! longPath )
		{
			transform.position = VectorTools.cloneVector3 ( _initialPosition );
			_countTime = false;
			onCompleteGoingDown ();
		}
	}
	
	void LateUpdate ()
	{
		if ( longPath )
		{
			if ( _stepOnPathID == 0 ) return;
			transform.LookAt ( new Vector3 ((float) targetTiles[_stepOnPathID - 1][0], transform.position.y, (float) targetTiles[_stepOnPathID - 1][1] ));
		}
		else
		{
			if ( targetTiles[0].Length == 0 ) return;
			transform.LookAt ( new Vector3 ((float) targetTiles[0][0], transform.position.y, (float) targetTiles[0][1] ));
		}
	}
}
