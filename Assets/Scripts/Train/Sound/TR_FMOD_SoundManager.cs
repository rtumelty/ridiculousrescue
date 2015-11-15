using UnityEngine;
using System;
using System.Collections;
//using FMOD.Studio;

public class TR_FMOD_SoundManager : MonoBehaviour {

	/*
	public FMOD.Studio.EventInstance engine;
	public FMOD.Studio.EventInstance musicTheme;
	public FMOD.Studio.ParameterInstance mySpeed;
	public FMOD.Studio.ParameterInstance myVolume;
	private float trainSpeed;
	public float myVolSilder;
	
	void Start()
	{
		engine = FMOD_StudioSystem.instance.getEvent("event:/Environment/Train Ch");
		musicTheme = FMOD_StudioSystem.instance.getEvent("event:/Music/Train Music");
		//engine.start();
		engine.getParameter("Speed", out mySpeed);
		musicTheme.getParameter ("Volume", out myVolume);
		engine.start ();
		musicTheme.start ();
		InvokeRepeating("updateAudioSettings", 0, 0.2f);
		//music = FMOD_StudioSystem.instance.getEvent("event:/Environment/Train Ch");
	}
	
	void Update()
	{

	}

	void updateAudioSettings ()
	{
		trainSpeed = (TRSpeedAndTrackOMetersManager.getInstance ().getSpeed ()/4);
		mySpeed.setValue(trainSpeed);
		//myVolume.setValue (myVolSilder);
	}
	
	void OnDisable()
	{
		musicTheme.stop ();
		engine.stop ();
		CancelInvoke ("updateAudioSettings");
	}
	*/
}