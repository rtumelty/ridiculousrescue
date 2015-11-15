using UnityEngine;
using System.Collections;

public class UnlockLevelSequenceManager : MonoBehaviour 
{
	//*************************************************************//
	public static int LEVEL_I_AM_ON = 1;
	public static GameObject LEVEL_NODE_I_AM_ON_GAME_OBJECT;
	public static bool PROCEED_WITH_UNLOCKING = true;
	//*************************************************************//
	public bool forceSequence = false;
	//*************************************************************//
	private GameObject _nodeFxParticles01;
	private GameObject _nodeFxParticles02;
	private GameObject _nodeFxParticles01Instant;
	private GameObject _nodeFxParticles02Instant;
	//*************************************************************//
	void Awake ()
	{
		PROCEED_WITH_UNLOCKING = true;

		_nodeFxParticles01 = ( GameObject ) Resources.Load ( "Particles/SparkleRising" );
		_nodeFxParticles02 = ( GameObject ) Resources.Load ( "Particles/Sparks" );
	}
	
	IEnumerator Start ()
	{
		bool proceedWithUnlockicg = true;

		if (  GetComponent < FLMissionScreenNodeManager > () != null )
		{
			switch ( GetComponent < FLMissionScreenNodeManager > ().type )
			{
				case FLMissionScreenNodeManager.TYPE_MINING_NODE:
					if ( ! GameGlobalVariables.CUT_DOWN_GAME && GetComponent < FLMissionScreenMiningLevelIconControl > ().myLevelClass.myName == "1" && SaveDataManager.getValue ( SaveDataManager.LEVEL_MINING_FINISHED_PREFIX + "1" ) == 1 && SaveDataManager.getValue ( SaveDataManager.LEVEL_WAS_UNLOCKED_PREFIX + "6" ) != 1 )
					{
						LEVEL_I_AM_ON = -779;
						LEVEL_NODE_I_AM_ON_GAME_OBJECT = Camera.main.transform.Find ( "world" ).Find ( "allWorlds" ).Find ( "0" ).Find ( "levels" ).Find ( "lab" ).gameObject;

						FLMissionRoomManager.DO_NOT_TRIGGER_MAP_DIALOG = true;
						FLMissionScreenMapDialogManager.getInstance ().triggerDialog ( LEVEL_I_AM_ON, LEVEL_NODE_I_AM_ON_GAME_OBJECT );
						
						LEVEL_NODE_I_AM_ON_GAME_OBJECT.AddComponent < JumpingArrowAbove > ().doNotJump = true;
					}
					else if ( GetComponent < FLMissionScreenMiningLevelIconControl > ().myLevelClass.myName == "2" && SaveDataManager.getValue ( SaveDataManager.LEVEL_MINING_FINISHED_PREFIX + "2" ) != 1 && SaveDataManager.getValue ( SaveDataManager.LEVEL_WAS_UNLOCKED_PREFIX + "23" ) != 1 )
					{
						LEVEL_I_AM_ON = -782;
						LEVEL_NODE_I_AM_ON_GAME_OBJECT = gameObject;
						
						FLMissionRoomManager.DO_NOT_TRIGGER_MAP_DIALOG = true;
						FLMissionScreenMapDialogManager.getInstance ().triggerDialog ( LEVEL_I_AM_ON, LEVEL_NODE_I_AM_ON_GAME_OBJECT );
						
						LEVEL_NODE_I_AM_ON_GAME_OBJECT.AddComponent < JumpingArrowAbove > ().doNotJump = true;
					}
					else if ( ! SaveDataManager.keyExists ( SaveDataManager.LEVEL_WAS_UNLOCKED_PREFIX + "MN" + GetComponent < FLMissionScreenMiningLevelIconControl > ().myLevelClass.myName ))
					{
						FLMissionRoomManager.getInstance ().blockOrUnblockSwiping ( true );
						yield return new WaitForSeconds ( 0.2f );
						_nodeFxParticles01Instant = ( GameObject ) Instantiate ( _nodeFxParticles01, transform.position + Vector3.up, _nodeFxParticles01.transform.rotation ); 
						yield return new WaitForSeconds ( 1.7f );
						SoundManager.getInstance ().playSound ( SoundManager.LEVEL_NODE_UNLOCKED );
						yield return new WaitForSeconds ( 0.3f );
						Destroy ( _nodeFxParticles01Instant );
						//=================================================Daves Edit==============================================
						//iTween.ScaleFrom ( this.gameObject, iTween.Hash ( "time", 1f, "easetype", iTween.EaseType.easeOutBounce, "scale", new Vector3 ( 1.25f, 1.25f, 1.25f )));
						//   **********This Line was interupting an already running itween component stopping the loop after 1 cycle when level was first unlocked.*********
						//=================================================Daves Edit==============================================
						_nodeFxParticles02Instant = ( GameObject ) Instantiate ( _nodeFxParticles02, transform.position + Vector3.up, _nodeFxParticles02.transform.rotation ); 
						yield return new WaitForSeconds ( 1f );
						FLMissionRoomManager.getInstance ().blockOrUnblockSwiping ( false );
						
					}

					GetComponent < FLMissionScreenMiningLevelIconControl > ().unlocked = true;
					SaveDataManager.save ( SaveDataManager.LEVEL_WAS_UNLOCKED_PREFIX + "MN" + GetComponent < FLMissionScreenMiningLevelIconControl > ().myLevelClass.myName, 1 );

					break;

				case FLMissionScreenNodeManager.TYPE_TRAIN_NODE:
					if ( GetComponent < FLMissionScreenTrainLevelIconControl > ().myLevelClass.myName == "1" && SaveDataManager.getValue ( SaveDataManager.LEVEL_FINISHED_PREFIX + "TR" + "1" ) != 1 && ! SaveDataManager.keyExists ( SaveDataManager.LEVEL_MAP_DIALOG_PLAYED_PREFIX + (-780).ToString ()))
					{
						LEVEL_I_AM_ON = -780;
						LEVEL_NODE_I_AM_ON_GAME_OBJECT = gameObject;
						
						FLMissionRoomManager.DO_NOT_TRIGGER_MAP_DIALOG = true;
						FLMissionScreenMapDialogManager.getInstance ().triggerDialog ( LEVEL_I_AM_ON, LEVEL_NODE_I_AM_ON_GAME_OBJECT );

						proceedWithUnlockicg = false;
					}
					else if ( GetComponent < FLMissionScreenTrainLevelIconControl > ().myLevelClass.myName == "2" && SaveDataManager.getValue ( SaveDataManager.LEVEL_FINISHED_PREFIX + "TR" + "2" ) != 1 && ! SaveDataManager.keyExists ( SaveDataManager.LEVEL_MAP_DIALOG_PLAYED_PREFIX + (-781).ToString ()))
					{
						LEVEL_I_AM_ON = -781;
						LEVEL_NODE_I_AM_ON_GAME_OBJECT = gameObject;
						
						FLMissionRoomManager.DO_NOT_TRIGGER_MAP_DIALOG = true;
						FLMissionScreenMapDialogManager.getInstance ().triggerDialog ( LEVEL_I_AM_ON, LEVEL_NODE_I_AM_ON_GAME_OBJECT );
						
						PROCEED_WITH_UNLOCKING = proceedWithUnlockicg = false;
					}
					else if ( GetComponent < FLMissionScreenTrainLevelIconControl > ().myLevelClass.myName == "3" && SaveDataManager.getValue ( SaveDataManager.LEVEL_FINISHED_PREFIX + "TR" + "3" ) != 1 && ! SaveDataManager.keyExists ( SaveDataManager.LEVEL_MAP_DIALOG_PLAYED_PREFIX + (-783).ToString ()))
					{
						LEVEL_I_AM_ON = -783;
						LEVEL_NODE_I_AM_ON_GAME_OBJECT = gameObject;
						
						FLMissionRoomManager.DO_NOT_TRIGGER_MAP_DIALOG = true;
						FLMissionScreenMapDialogManager.getInstance ().triggerDialog ( LEVEL_I_AM_ON, LEVEL_NODE_I_AM_ON_GAME_OBJECT );
						
						proceedWithUnlockicg = false;
					}
					else if ( ! SaveDataManager.keyExists ( SaveDataManager.LEVEL_WAS_UNLOCKED_PREFIX + "TR" + GetComponent < FLMissionScreenTrainLevelIconControl > ().myLevelClass.myName ))
					{
						FLMissionRoomManager.getInstance ().blockOrUnblockSwiping ( true );
						yield return new WaitForSeconds ( 0.2f );
						_nodeFxParticles01Instant = ( GameObject ) Instantiate ( _nodeFxParticles01, transform.position + Vector3.up, _nodeFxParticles01.transform.rotation ); 
						yield return new WaitForSeconds ( 1.7f );
						SoundManager.getInstance ().playSound ( SoundManager.LEVEL_NODE_UNLOCKED );
						yield return new WaitForSeconds ( 0.3f );
						Destroy ( _nodeFxParticles01Instant );
						iTween.ScaleFrom ( this.gameObject, iTween.Hash ( "time", 1f, "easetype", iTween.EaseType.easeOutBounce, "scale", new Vector3 ( 1.25f, 1.25f, 1.25f )));
						_nodeFxParticles02Instant = ( GameObject ) Instantiate ( _nodeFxParticles02, transform.position + Vector3.up, _nodeFxParticles02.transform.rotation ); 
						yield return new WaitForSeconds ( 1f );
						FLMissionRoomManager.getInstance ().blockOrUnblockSwiping ( false );
					}

					if ( proceedWithUnlockicg )
					{
						GetComponent < FLMissionScreenTrainLevelIconControl > ().mayStart = true;
						SaveDataManager.save ( SaveDataManager.LEVEL_WAS_UNLOCKED_PREFIX + "TR" + GetComponent < FLMissionScreenTrainLevelIconControl > ().myLevelClass.myName, 1 );
					}
					break;

				case FLMissionScreenNodeManager.TYPE_REGULAR_NODE:
					int levelID = 0; 
					int.TryParse ( GetComponent < FLMissionScreenLevelIconControl > ().myLevelClass.myName, out levelID );
					if ( GetComponent < FLMissionScreenLevelIconControl > ().myLevelClass.myName == "13" && ! SaveDataManager.keyExists ( SaveDataManager.LOCK_ON_LEVEL_DESTROYED_PREFIX + "13" ))
					{
						LEVEL_I_AM_ON = 13;
						LEVEL_NODE_I_AM_ON_GAME_OBJECT = gameObject;

						FLMissionRoomManager.DO_NOT_TRIGGER_MAP_DIALOG = true;
						FLMissionScreenMapDialogManager.getInstance ().triggerDialog ( LEVEL_I_AM_ON, LEVEL_NODE_I_AM_ON_GAME_OBJECT );
						proceedWithUnlockicg = false;
					}
					else if ( GetComponent < FLMissionScreenLevelIconControl > ().myLevelClass.myName == "11" && SaveDataManager.getValue ( SaveDataManager.LEVEL_FINISHED_PREFIX + "11" ) == 1 && SaveDataManager.getValue ( SaveDataManager.LEVEL_FINISHED_PREFIX + "TR" + "1" ) != 1 )
					{
						LEVEL_I_AM_ON = -780;
						LEVEL_NODE_I_AM_ON_GAME_OBJECT = Camera.main.transform.Find ( "world" ).Find ( "allWorlds" ).Find ( "0" ).Find ( "levels" ).Find ( "train01" ).gameObject;

						FLMissionRoomManager.DO_NOT_TRIGGER_MAP_DIALOG = true;
						FLMissionScreenMapDialogManager.getInstance ().triggerDialog ( LEVEL_I_AM_ON, LEVEL_NODE_I_AM_ON_GAME_OBJECT );
						proceedWithUnlockicg = false;
					}
					else if ( GetComponent < FLMissionScreenLevelIconControl > ().myLevelClass.myName == "18" && SaveDataManager.getValue ( SaveDataManager.LEVEL_FINISHED_PREFIX + "18" ) == 1 && SaveDataManager.getValue ( SaveDataManager.LEVEL_FINISHED_PREFIX + "TR" + "2" ) != 1 )
					{
						LEVEL_I_AM_ON = -781;
						LEVEL_NODE_I_AM_ON_GAME_OBJECT = Camera.main.transform.Find ( "world" ).Find ( "allWorlds" ).Find ( "1" ).Find ( "levels" ).Find ( "train02" ).gameObject;
						
						FLMissionRoomManager.DO_NOT_TRIGGER_MAP_DIALOG = true;
						FLMissionScreenMapDialogManager.getInstance ().triggerDialog ( LEVEL_I_AM_ON, LEVEL_NODE_I_AM_ON_GAME_OBJECT );
						PROCEED_WITH_UNLOCKING = proceedWithUnlockicg = false;
					}
					else if ( GetComponent < FLMissionScreenLevelIconControl > ().myLevelClass.myName == "25" && SaveDataManager.getValue ( SaveDataManager.LEVEL_FINISHED_PREFIX + "25" ) == 1 && SaveDataManager.getValue ( SaveDataManager.LEVEL_FINISHED_PREFIX + "TR" + "3" ) != 1 )
					{
						LEVEL_I_AM_ON = -783;
						LEVEL_NODE_I_AM_ON_GAME_OBJECT = Camera.main.transform.Find ( "world" ).Find ( "allWorlds" ).Find ( "1" ).Find ( "levels" ).Find ( "train03" ).gameObject;
						
						FLMissionRoomManager.DO_NOT_TRIGGER_MAP_DIALOG = true;
						FLMissionScreenMapDialogManager.getInstance ().triggerDialog ( LEVEL_I_AM_ON, LEVEL_NODE_I_AM_ON_GAME_OBJECT );
						proceedWithUnlockicg = false;
					}
					else if ( ! GameGlobalVariables.CUT_DOWN_GAME && GetComponent < FLMissionScreenLevelIconControl > ().myLevelClass.myName == "5" && GameGlobalVariables.BLOCK_LAB_ENTERED )
					{
						LEVEL_I_AM_ON = -777;
						LEVEL_NODE_I_AM_ON_GAME_OBJECT = Camera.main.transform.Find ( "world" ).Find ( "allWorlds" ).Find ( "0" ).Find ( "levels" ).Find ( "lab" ).gameObject;

						FLMissionRoomManager.DO_NOT_TRIGGER_MAP_DIALOG = true;
						FLMissionScreenMapDialogManager.getInstance ().triggerDialog ( LEVEL_I_AM_ON, LEVEL_NODE_I_AM_ON_GAME_OBJECT );

						LEVEL_NODE_I_AM_ON_GAME_OBJECT.AddComponent < JumpingArrowAbove > ().doNotJump = true;

						proceedWithUnlockicg = false;
					}
					else if ( ! GameGlobalVariables.CUT_DOWN_GAME && GetComponent < FLMissionScreenLevelIconControl > ().myLevelClass.myName == "5" && SaveDataManager.getValue ( SaveDataManager.LEVEL_MINING_FINISHED_PREFIX + "1" ) != 1 && SaveDataManager.getValue ( SaveDataManager.LEVEL_FINISHED_PREFIX + "5" ) == 1 )
					{
						LEVEL_I_AM_ON = -778;
						LEVEL_NODE_I_AM_ON_GAME_OBJECT = Camera.main.transform.Find ( "world" ).Find ( "allWorlds" ).Find ( "0" ).Find ( "levels" ).Find ( "mining01" ).gameObject;
;
						FLMissionRoomManager.DO_NOT_TRIGGER_MAP_DIALOG = true;
						FLMissionScreenMapDialogManager.getInstance ().triggerDialog ( LEVEL_I_AM_ON, LEVEL_NODE_I_AM_ON_GAME_OBJECT );
						
						LEVEL_NODE_I_AM_ON_GAME_OBJECT.AddComponent < JumpingArrowAbove > ().doNotJump = true;
					}
					else if ( ! GameGlobalVariables.CUT_DOWN_GAME && GetComponent < FLMissionScreenLevelIconControl > ().myLevelClass.myName == "22" && SaveDataManager.getValue ( SaveDataManager.LEVEL_MINING_FINISHED_PREFIX + "2" ) != 1 && SaveDataManager.getValue ( SaveDataManager.LEVEL_FINISHED_PREFIX + "22" ) == 1 )
					{
						LEVEL_I_AM_ON = -782;
						LEVEL_NODE_I_AM_ON_GAME_OBJECT = Camera.main.transform.Find ( "world" ).Find ( "allWorlds" ).Find ( "1" ).Find ( "levels" ).Find ( "mining02" ).gameObject;
						
						FLMissionRoomManager.DO_NOT_TRIGGER_MAP_DIALOG = true;
						FLMissionScreenMapDialogManager.getInstance ().triggerDialog ( LEVEL_I_AM_ON, LEVEL_NODE_I_AM_ON_GAME_OBJECT );
						
						LEVEL_NODE_I_AM_ON_GAME_OBJECT.AddComponent < JumpingArrowAbove > ().doNotJump = true;
					}
					else if ( GetComponent < FLMissionScreenLevelIconControl > ().myLevelClass.myName != "1" )
					{
						if ( ! SaveDataManager.keyExists ( SaveDataManager.LEVEL_WAS_UNLOCKED_PREFIX + GetComponent < FLMissionScreenLevelIconControl > ().myLevelClass.myName ) || forceSequence )
						{
							SaveDataManager.save ( SaveDataManager.LEVEL_WAS_UNLOCKED_PREFIX + GetComponent < FLMissionScreenLevelIconControl > ().myLevelClass.myName, 1 );
							FLMissionRoomManager.DO_NOT_TRIGGER_MAP_DIALOG = true;

							if ( LEVEL_I_AM_ON < levelID )
							{
								LEVEL_I_AM_ON = levelID;
								LEVEL_NODE_I_AM_ON_GAME_OBJECT = gameObject;
							}

							if ( proceedWithUnlockicg && PROCEED_WITH_UNLOCKING )
							{
								FLMissionRoomManager.getInstance ().blockOrUnblockSwiping ( true );
								yield return new WaitForSeconds ( 0.2f );
								_nodeFxParticles01Instant = ( GameObject ) Instantiate ( _nodeFxParticles01, transform.position + Vector3.up, _nodeFxParticles01.transform.rotation ); 
								yield return new WaitForSeconds ( 1.7f );
								SoundManager.getInstance ().playSound ( SoundManager.LEVEL_NODE_UNLOCKED );
								yield return new WaitForSeconds ( 0.3f );
								Destroy ( _nodeFxParticles01Instant );
								iTween.ScaleFrom ( this.gameObject, iTween.Hash ( "time", 1f, "easetype", iTween.EaseType.easeOutBounce, "scale", new Vector3 ( 1.25f, 1.25f, 1.25f )));
								_nodeFxParticles02Instant = ( GameObject ) Instantiate ( _nodeFxParticles02, transform.position + Vector3.up, _nodeFxParticles02.transform.rotation ); 
								yield return new WaitForSeconds ( 1f );
								FLMissionRoomManager.getInstance ().blockOrUnblockSwiping ( false );
								FLMissionScreenMapDialogManager.getInstance ().triggerDialog ( LEVEL_I_AM_ON, LEVEL_NODE_I_AM_ON_GAME_OBJECT );

								if ( LEVEL_NODE_I_AM_ON_GAME_OBJECT != null )
								{
									if ( LEVEL_NODE_I_AM_ON_GAME_OBJECT.GetComponent < SelectedComponenent > () == null )
									{
										LEVEL_NODE_I_AM_ON_GAME_OBJECT.AddComponent < SelectedComponenent > ().setSelectedForUIHighlight ( true, 1f );
									}
									else 
									{
										LEVEL_NODE_I_AM_ON_GAME_OBJECT.GetComponent < SelectedComponenent > ().setSelectedForUIHighlight ( true, 1f  );
									}
								}
							}
						}
						else
						{
							FLMissionRoomManager.DO_NOT_TRIGGER_MAP_DIALOG = false;
						}
					}
					
					if ( proceedWithUnlockicg && PROCEED_WITH_UNLOCKING )
					{
						if ( LEVEL_I_AM_ON < levelID )
						{
							LEVEL_I_AM_ON = levelID;
							LEVEL_NODE_I_AM_ON_GAME_OBJECT = gameObject;
						}
						
						GetComponent < FLMissionScreenLevelIconControl > ().mayStart = true;
						SaveDataManager.save ( SaveDataManager.LEVEL_WAS_UNLOCKED_PREFIX + GetComponent < FLMissionScreenLevelIconControl > ().myLevelClass.myName, 1 );
						
						if ( ! GetComponent < FLMissionScreenLevelIconControl > ().myLevelClass.iAmFinished )
						{
							if ( gameObject.GetComponent < JumpingArrowAbove > () == null ) gameObject.AddComponent < JumpingArrowAbove > ().doNotJump = true;
						}
					}
					break;

				case FLMissionScreenNodeManager.TYPE_BONUS_NODE:
					if ( ! SaveDataManager.keyExists ( SaveDataManager.LEVEL_WAS_UNLOCKED_PREFIX + "B" + GetComponent < FLMissionScreenLevelIconControl > ().myLevelClass.myName ))
					{
						FLMissionRoomManager.getInstance ().blockOrUnblockSwiping ( true );
						yield return new WaitForSeconds ( 0.2f );
						_nodeFxParticles01Instant = ( GameObject ) Instantiate ( _nodeFxParticles01, transform.position + Vector3.up, _nodeFxParticles01.transform.rotation ); 
						yield return new WaitForSeconds ( 1.7f );
						SoundManager.getInstance ().playSound ( SoundManager.LEVEL_NODE_UNLOCKED );
						yield return new WaitForSeconds ( 0.3f );
						Destroy ( _nodeFxParticles01Instant );
						iTween.ScaleFrom ( this.gameObject, iTween.Hash ( "time", 1f, "easetype", iTween.EaseType.easeOutBounce, "scale", new Vector3 ( 1.25f, 1.25f, 1.25f )));
						_nodeFxParticles02Instant = ( GameObject ) Instantiate ( _nodeFxParticles02, transform.position + Vector3.up, _nodeFxParticles02.transform.rotation ); 
						yield return new WaitForSeconds ( 1f );
						FLMissionRoomManager.getInstance ().blockOrUnblockSwiping ( false );
					}
					GetComponent < FLMissionScreenLevelIconControl > ().mayStart = true;
					SaveDataManager.save ( SaveDataManager.LEVEL_WAS_UNLOCKED_PREFIX + "B" + GetComponent < FLMissionScreenLevelIconControl > ().myLevelClass.myName, 1 );
					
					break;
			}
		}
		
		Destroy ( _nodeFxParticles02Instant );

		if ( proceedWithUnlockicg /*&& PROCEED_WITH_UNLOCKING*/ )
		{
			if ( GetComponent < FLMissionScreenNodeManager > () != null )
			{
				switch ( GetComponent < FLMissionScreenNodeManager > ().type )
				{
					case FLMissionScreenNodeManager.TYPE_REGULAR_NODE:
						renderer.material.mainTexture = FLMissionRoomManager.getInstance ().finishedLevelTexture;
						break;
					case FLMissionScreenNodeManager.TYPE_BONUS_NODE:
						renderer.material.mainTexture = FLMissionRoomManager.getInstance ().finishedLevelBonusTexture;
						break;
					case FLMissionScreenNodeManager.TYPE_MINING_NODE:
						renderer.material.mainTexture = FLMissionRoomManager.getInstance ().finishedLevelMiningTexture;
						break;
					case FLMissionScreenNodeManager.TYPE_TRAIN_NODE:
						renderer.material.mainTexture = FLMissionRoomManager.getInstance ().finishedLevelTrainTexture;				
						break;
				}
			}
			renderer.enabled = true;
		}
	}
}
