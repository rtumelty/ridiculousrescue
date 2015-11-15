using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class FL_MOMClass 
{	
	//*************************************************************//	
	public int numberOfSlots;
	public int numberOfSlotsUnblocked;
	public int[] position;
	public GameObject momObject;
	public FL_MOMPanelControl myMomPanelControl;
	public FL_MOMControl myMomControl;
	public List < FL_MOMControl.ProductionSlot > myProductionSlots = new List < FL_MOMControl.ProductionSlot > (); 
	//*************************************************************//	
	public FL_MOMClass ( int numberOfSlotsValue, int numberOfSlotsUnblockedValue, int[] positionValue )
	{
		numberOfSlots = numberOfSlotsValue;
		numberOfSlotsUnblocked = numberOfSlotsUnblockedValue;
		position = positionValue;
	}
}
