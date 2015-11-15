using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class FLElementsConstructionCosts 
{
	//*************************************************************//
	public class Cost
	{
		public int metal;
		public int plastic;
		public int vines;
		public int premiumCurrency;
		public int time;
		
		public Cost ( int metalValue, int plasticValue, int vinesValue, int premiumCurrencyValue, int timeValue )
		{
			metal = metalValue;
			plastic = plasticValue;
			vines = vinesValue;
			premiumCurrency = premiumCurrencyValue;
			time = timeValue;
		}
	}
	//*************************************************************//
	public static Dictionary < int, Cost > COSTS_VALUES;
	//*************************************************************//
}
