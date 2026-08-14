using System;
using System.Globalization;
using System.IO;
using System.IO.IsolatedStorage;
using System.Threading;
using AI;
using EGEngine;
using InputHandler;
using Joints;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
using Models;
using Networking;
using Physics;
using Pickups;
using Players;
using Rendering;
using Sounds;
using Structs;
using Textures;
using Threads;
using Util;
using Weapons;
using WindowsGame1;

namespace MainGame;

public class MainGame
{
	public const float MAX_AIRSPEED = 1000f;

	public const float BASE_TERRAIN_HEIGHT = 14f;

	public const float MAX_AIRPLANE_SPEED = 750f;

	public const float MAX_AIRPLANE_SPEED_DIV_4 = 187.5f;

	public const float MAX_BOMBDROP_SCORE = 90000f;

	public static byte debugRenderCrashCount = 0;

	public static byte debugUpdateCrashCount = 0;

	public static byte debugRestart = 0;

	public static byte lastUpdateBuffer;

	public static float turnBuffer = 0f;

	public static ushort numNavRoutes;

	public static dtStatNavMesh NavigationMesh;

	public static ushort[] routePolys;

	public static bool needToTallyMapVotes;

	public static bool mapManuallySet;

	public static bool linearProgression = true;

	public static bool bombViewEnabled = true;

	public static bool flapsDownOnStart = true;

	public static bool outOfRangeShown = false;

	public static bool inPlaneSelectionScreen = false;

	public static bool needToResetPlane = false;

	public static bool offRaceTrack;

	public static bool activateRetracts = false;

	public static bool bombDropOver;

	public static byte newPlaneID;

	public static byte bombDropMultSpeed;

	public static byte bombDropMultHeight;

	public static byte numWheelsTouching = 0;

	public static byte bombWeaponMount = 2;

	public static byte[] raceFinished = new byte[44];

	public static byte[] gearDown = new byte[44];

	public static byte[] mapVotes = new byte[4];

	public static byte[] playersMapChoice = new byte[4];

	public static int raceModeMultHeightDisplay;

	public static int uiTimeBonusAdditional;

	public static int[,] texGame;

	public static int[] bombsHit = new int[44];

	public static int[] bombsDropped = new int[44];

	public static float laserRangeStart;

	public static float laserRange;

	public static float turnPlaneAround = 0f;

	public static float lastGlitch1;

	public static float lastGlitch2;

	public static float lastGlitch3;

	public static float flaps = 0f;

	public static float planeVelocity;

	public static float[] batteryPower = new float[2];

	public static float[] raceModeMultHeight = new float[44];

	public static float[] racePenaltyTime = new float[44];

	public static sbyte[] Commander = new sbyte[5];

	public static short commanderSelect;

	public static float commanderTeleportTimer;

	public static float commanderTeleportEnergy;

	public static bool commanderIsNotTeleporting;

	public static bool commanderTeleportingPlayer;

	public static bool commanderTeleportReadyToDeploy;

	public static bool needToTeleport;

	public static bool isCommander = false;

	public static bool commanderMode = false;

	public static byte numCommanderObjectives = 4;

	public static StructsClass.Commander_Item[,] commanderObjectives = new StructsClass.Commander_Item[7, 4];

	public static StructsClass.Basic_Position commanderObjectivePosition = new StructsClass.Basic_Position();

	public static bool becameMPHost;

	public static bool saveSpWeapons = false;

	public static bool needToLoadWeapons = false;

	public static bool allowTeamKills = false;

	public static bool playIntro = true;

	public static bool primaryWeaponMountHasWeapon = false;

	public static bool secondaryWeaponMountHasWeapon;

	public static bool loadGlobalSettings = true;

	public static bool tauntingEnabled = false;

	public static bool storageDeviceNotChosen = false;

	public static bool playerSettingsLoaded = true;

	public static bool hostStartedGame = false;

	public static bool pilotView = false;

	public static bool overheadView = false;

	public static bool viewChanged = false;

	public static bool soundWhenEnemeyKilled = true;

	public static bool spSpawnCheckForEnemy = false;

	public static bool autoStartMPGame = false;

	public static bool needToLoadPlayerSettings = false;

	public static bool needToSavePlayerSettings = false;

	public static bool autoRespawn = false;

	public static bool freezeForTextureSwap = false;

	public static bool updateRunning = false;

	public static bool spGameReady = false;

	public static bool createNewSPRound = false;

	public static byte mpNumPrivateGamerSlots = 0;

	public static byte primaryObjectMount = 0;

	public static byte primaryWeaponMount = 0;

	public static byte secondaryWeaponMount = 0;

	public static byte curGameTip = 0;

	public static byte numGameTips = 0;

	public static byte numAchievementRewards = 0;

	public static byte lastAchievementReward;

	public static byte curAchievementReward;

	public static byte numTaunts = 0;

	public static byte curTaunt = 0;

	public static byte showGameFlags = 0;

	public static byte spLastLoadedLevel = byte.MaxValue;

	public static byte spLastLoadeGameType = byte.MaxValue;

	public static byte mpLastLoadedLevel = byte.MaxValue;

	public static byte mpLastLoadeGameType = byte.MaxValue;

	public static byte numTeams = 2;

	public static byte numRounds = 1;

	public static byte levelLapsToFinish;

	public static byte spSaving = 0;

	public static byte numSPLevels = 20;

	public static byte mp_numPlayers_index = 0;

	public static byte mp_timeLimit_index = 0;

	public static byte commanderTeleportPlayer = 1;

	public static byte commanderLevel = 0;

	public static byte commanderNumTeammates;

	public static byte objectiveUniqueID = 0;

	public static byte commanderItem;

	public static byte settingsTeamRace1;

	public static byte settingsTeamRace2;

	public static byte settingsMap;

	public static byte settingsTimeLimit;

	public static byte miscFunctionByte1 = 0;

	public static byte difficulty = 0;

	public static byte highestLevel = 0;

	public static byte gameLevel = 0;

	public static byte numLevels = 2;

	public static byte frameC1 = 0;

	public static byte localNetworkGamerID = 4;

	public static byte gameMode = byte.MaxValue;

	public static byte gameType = 0;

	public static byte maxGamePlayers = 44;

	public static byte maxHumanGamePlayers = 4;

	public static byte gameState = 0;

	public static short curSpLevel = -1;

	public static byte[] tauntIDs;

	public static byte[] lockedTauntLevels;

	public static byte[] startOfFrameMessages = new byte[10];

	public static byte[] endOfFrameMessages = new byte[10];

	public static byte[] Guards = new byte[44];

	public static byte[] showCrossHairs = new byte[4];

	public static byte[] mp_numPlayers = new byte[4] { 4, 8, 12, 16 };

	public static byte[] mp_numRemotePlayers = new byte[4] { 4, 8, 12, 16 };

	public static byte[] mp_timeLimit = new byte[4] { 5, 10, 15, 20 };

	public static byte[] laps = new byte[4];

	public static bool viewFollowingObject = false;

	public static bool unlimitedAmmo = false;

	public static bool raceCanExitForNewPlayers = false;

	public static bool raceParticipantsLocked = false;

	public static bool mpSetupReady = false;

	public static bool newRoundOnDeath = true;

	public static bool roundStarting = false;

	public static bool restartIsDeath = true;

	public static bool useFixedSpawnPoint = true;

	public static bool gameReady = false;

	public static bool trialMode = true;

	public static bool usingIronSights = false;

	public static bool quickScope = false;

	public static bool usingScope = false;

	public static bool sprinting = false;

	public static bool enteringSettingsMenu = false;

	public static bool isGuard = true;

	public static bool mpGameDataReady = false;

	public static bool mpGameSetupNeedsToExit = false;

	public static bool gameSetupRunning = false;

	public static bool haveSettings = false;

	public static bool sideStepping = false;

	public static bool walking = false;

	public static bool walkingBackwards = false;

	public static bool haveProgramData = false;

	public static bool haveAllPlayerStatus = false;

	public static bool roundOver;

	public static byte displayLaps;

	public static byte currentRaceStartTimer;

	public static byte numLives;

	public static sbyte maxLocalPlayerSpawnPoint;

	public static short curVboID = -1;

	public static short newVboID = 0;

	public static short inRecoil;

	public static int signedinGamerID = -1;

	public static int width;

	public static int height;

	public static int h1;

	public static int h2;

	public static int safeWidth;

	public static int safeHeight;

	public static int safeX;

	public static int safeY;

	public static short misFunctionShort1;

	public static short misFunctionShort2;

	public static short misFunctionShort3;

	public static short pickupsCollected;

	public static ushort numCollisionModels = 0;

	public static ushort maxMessages = 10;

	public static ushort numStartOfFrameMessages;

	public static ushort numEndOfFrameMessages;

	public static ushort viewFollowingObjectID;

	public static ushort maxPlayersPerTeam = 16;

	public static float tauntDistanceSqr = 100f;

	public static float idleTimeout;

	public static float curIdleTime;

	public static float gameTime;

	public static float frameTimePrioToPause;

	public static float misFunctionFloat1;

	public static float viewAngle = 0f;

	public static float targetAimingAngle;

	public static float targetAimingElevAngle;

	public static float raceStartTimer = 0f;

	public static float damageReduction = 0.15f;

	public static float damageIncrease = 1f;

	public static float respawnTime;

	public static float roundTimeLimit;

	public static float roundCurrentTime;

	public static float curTimeBeforeExitingMapOnDeath;

	public static float timeBeforeExitingMapOnDeath = 11f;

	public static float MaxLeft;

	public static float MaxRight;

	public static float MaxForward;

	public static float MaxRear;

	public static float MaxUp;

	public static float MaxDown;

	public static float cameraMovementSpeed;

	public static float cameraObjectMovementSpeed;

	public static float cameraMovementSpeedDefault = 2000f;

	public static float sendLapTime;

	public static float lobbyTimer;

	public static float lobbyDataTimer;

	public static float lobbyMapVoteTimer = 8f;

	public static float showResultsTimer;

	public static float showResultsTime = 7f;

	public static float xPos2;

	public static float yPos2;

	public static float zPos2;

	public static float relVelX;

	public static float relVelY;

	public static float relVelZ;

	public static float jointZ;

	public static float[] arcadeModeRotAngle = new float[44];

	public static float[] angularVelocity = new float[44];

	public static float[] arcadeModeRisingAngle = new float[44];

	public static double curFrameTime = 0.0;

	public static double healingAbility = 0.0;

	public static float frametime;

	public static float frameTimeAdjusted;

	public static int pointsForEnemyDeath = 1;

	public static int pointsForEnemyAiKill = 1;

	public static int pointsForOwnDeath = -1;

	public static int pointsForTeamKill = -1;

	public static int teamPointsForEnemyDeath = 1;

	public static int teamPointsForEnemyAiDeath = 1;

	public static int teamPointsForOwnDeath = -1;

	public static int teamPointsForTeamKill = -1;

	public static int roundScoreLimit;

	public static int roundMinScoreLimit;

	public static string lobbyMsg = "";

	public static StructsClass.Vehicle[] playerVehicles = new StructsClass.Vehicle[44];

	public static StructsClass.GameInfo gameData;

	public static StructsClass.GameInfo lastGameData;

	public static StructsClass.Game_Reward[] achievementRewards;

	public static ushort[] gameItem;

	public static ushort[] gameItemIndex;

	public static byte[] gameItemType;

	public static ushort numGameItems = 0;

	public static ushort numAllocatedGameItems = 0;

	public static Matrix[] itemPlacementMatrix = new Matrix[14];

	public static FileStream logOut;

	public static Game1.MasterCollection mainC;

	public static Random mainRandom;

	public static StorageDevice deviceGamer;

	public static long framestart = 0L;

	public static long mainTime;

	private Thread InitThread;

	private Thread InitPrograms;

	public void Init_Vars(Game1.MasterCollection masterC)
	{
		mainC = masterC;
		mainRandom = new Random((int)DateTime.Now.Ticks);
		global::Players.Players.thirdPersonXAdj = 1.3f;
		global::Players.Players.thirdPersonYAdj = -2.85f;
		global::Players.Players.thirdPersonZAdj = 2.8f;
		width = mainC.curGame.GraphicsDevice.Viewport.Width;
		height = mainC.curGame.GraphicsDevice.Viewport.Height;
		safeWidth = mainC.curGame.GraphicsDevice.Viewport.TitleSafeArea.Width;
		safeHeight = mainC.curGame.GraphicsDevice.Viewport.TitleSafeArea.Height;
		safeX = mainC.curGame.GraphicsDevice.Viewport.TitleSafeArea.X;
		safeY = mainC.curGame.GraphicsDevice.Viewport.TitleSafeArea.Y;
		for (byte b = 0; b < numCommanderObjectives; b++)
		{
			for (byte b2 = 0; b2 < 7; b2++)
			{
				commanderObjectives[b2, b] = default(StructsClass.Commander_Item);
				commanderObjectives[b2, b].active = false;
			}
		}
		mapVotes = new byte[19];
		InitThread = new Thread(Init_Variables);
		InitThread.Start();
	}

	public void Init_Variables()
	{
		byte threadID = 1;
		_ = global::Rendering.Rendering.uBufferID;
		mainC.playersMain.Init_Player_Preferences();
		// Xbox 360 hardware-thread affinity hint omitted on desktop.
		Load_Global_Settings();
		mainC.gameLogic.Game_Load_Starting();
		global::Players.Players.playerRankPointsSP = 0;
		global::Players.Players.playerRankPointsMP = 0;
		global::Players.Players.playerRankSP = 1;
		global::Players.Players.playerRankMP = 1;
		global::Players.Players.currentPlayerRank = 1;
		global::Players.Players.remotePlayerRanks[0] = global::Players.Players.currentPlayerRank;
		width = mainC.curGame.GraphicsDevice.Viewport.Width;
		height = mainC.curGame.GraphicsDevice.Viewport.Height;
		safeWidth = mainC.curGame.GraphicsDevice.Viewport.TitleSafeArea.Width;
		safeHeight = mainC.curGame.GraphicsDevice.Viewport.TitleSafeArea.Height;
		safeX = mainC.curGame.GraphicsDevice.Viewport.TitleSafeArea.X;
		safeY = mainC.curGame.GraphicsDevice.Viewport.TitleSafeArea.Y;
		for (byte b = 0; b < 44; b++)
		{
			playerVehicles[b].mv = new Matrix[2];
			ref Matrix reference = ref playerVehicles[b].mv[0];
			reference = Matrix.Identity;
			ref Matrix reference2 = ref playerVehicles[b].mv[1];
			reference2 = Matrix.Identity;
			playerVehicles[b].momentum = default(StructsClass.Momentum);
		}
		mainC.utilMain.Init_Utility();
		mainC.soundsMain.Init_Sounds();
		mainC.texturesMain.Init_Textures();
		mainC.collisionMain.Init_Collision();
		mainC.modelsMain.Init_Models();
		mainC.playersMain.Init_Players();
		mainC.renderingMain.Init_Rendering(threadID);
		mainC.gameobjectMain.Init_Objects(threadID);
		mainC.vehicles.Initialize_Vehicles();
		mainC.weaponsMain.Init_Weapons();
		mainC.jointsMain.Init_Joints(threadID);
		mainC.mapsMain.Init_Maps();
		mainC.aiMain.Initialize_AI();
		mainC.fontmoduleMain.Init_FontModule();
		mainC.threadingMain.Init_Threading();
		mainC.levelsMain.Init_Levels();
		Init_MainGame();
		mainC.avatarMain.Init_Avatars();
		mainC.graphingMain.Init_Graphs();
		mainC.zonesMain.Init_Zones();
		mainC.targetMain.Initialize_Targets();
		mainC.jointsMain.Load_Player_Joints();
		mainC.modelsMain.Create_Main_Model_VBO();
		mainC.playersMain.Set_Player_Race_Models();
		mainC.vehicles.Set_All_Vehicle_Models();
		mainC.playersMain.Initialize_All_Players_To_Default_Race();
		global::Weapons.Weapons.lSite.acceleration.v[0] = 0f;
		global::Weapons.Weapons.lSite.acceleration.v[1] = 0f;
		global::Weapons.Weapons.lSite.acceleration.v[2] = 0f;
		global::Weapons.Weapons.lSite.initialTime = 0.0;
		global::Players.Players.xRotation = 0f;
		global::Players.Players.yRotation = (global::Players.Players.zRotation = 0f);
		mainC.weaponsMain.Setup_Player_Ammo_Clips();
		mainC.pickupsMain.Initialize_Pickups();
		mainC.switchesMain.Initialize_Switches();
		global::Rendering.Rendering.framestart = -1.0;
		frametime = 0.033f;
		mainC.renderingMain.Handle_Screen_Resize();
		mainC.userInterface.Initialize_User_Interface();
		mainC.Explosions.Initialize_Explosions();
		mainC.networkingMain.Init_Networking();
		Thread.MemoryBarrier();
		mainC.inputMain.UI_Initiailize();
		global::Rendering.Rendering.initialLoading = 1;
		gameState = 1;
		NavigationMesh = new dtStatNavMesh();
		GC.Collect();
	}

	public void Main_Loop()
	{
		mainTime = DateTime.Now.Ticks;
		updateRunning = true;
		showCrossHairs[1] = 0;
		lastUpdateBuffer = global::Rendering.Rendering.uBufferID;
		Process_Start_Of_Frame_Messages();
		if (freezeForTextureSwap)
		{
			global::Textures.Textures.texChangeEnd.Reset();
			global::Textures.Textures.texChangeStart.Set();
			global::Textures.Textures.texChangeEnd.WaitOne();
			freezeForTextureSwap = false;
		}
		if (needToSavePlayerSettings)
		{
			mainC.maingameMain.Save_Player_Settings();
		}
		else if (needToLoadPlayerSettings)
		{
			mainC.maingameMain.Load_Player_Settings();
		}
		switch (gameState)
		{
		case 0:
			if (needToLoadPlayerSettings)
			{
				Load_Player_Settings();
			}
			try
			{
				if (signedinGamerID > -1 && signedinGamerID < Gamer.SignedInGamers.Count && Gamer.SignedInGamers[signedinGamerID] != null && Gamer.SignedInGamers[signedinGamerID].IsSignedInToLive)
				{
					Gamer.SignedInGamers[signedinGamerID].Presence.PresenceMode = GamerPresenceMode.AtMenu;
				}
			}
			catch (Exception)
			{
			}
			Calculate_Frame_Time();
			global::Threads.Threads.thread1End.Set();
			break;
		case 1:
			mainC.inputMain.Menu_Main();
			if (User_Interface.windows[1].status != 1 && !gameSetupRunning && gameState == 1)
			{
				User_Interface.mainMenuErrorTime += frametime;
				if (User_Interface.mainMenuErrorTime > 10f)
				{
					User_Interface.mainMenuErrorTime = 0f;
					mainC.userInterface.Show_Window(1, 1, resetButtons: false);
				}
			}
			Calculate_Frame_Time();
			global::Threads.Threads.thread1End.Set();
			break;
		case 21:
		{
			showCrossHairs[1] = 1;
			global::InputHandler.InputHandler.confirmEndGameScreen = false;
			byte uBufferID = global::Rendering.Rendering.uBufferID;
			_ = frametime;
			frametime = 0f;
			global::Threads.Threads.thread3End.Set();
			Main_Loop_Threaded_SP_Gameplay(0);
			if (!global::Players.Players.freezeCamera)
			{
				global::Rendering.Rendering.camPos[uBufferID].X = global::Rendering.Rendering.initialCamPosX;
				global::Rendering.Rendering.camPos[uBufferID].Y = global::Rendering.Rendering.initialCamPosY;
				global::Rendering.Rendering.camPos[uBufferID].Z = global::Rendering.Rendering.initialCamPosZ;
				global::Rendering.Rendering.camObject[uBufferID].X = global::Rendering.Rendering.initialCamObjX;
				global::Rendering.Rendering.camObject[uBufferID].Y = global::Rendering.Rendering.initialCamObjY;
				global::Rendering.Rendering.camObject[uBufferID].Z = global::Rendering.Rendering.initialCamObjZ;
				global::Rendering.Rendering.camUp[uBufferID].X = global::Rendering.Rendering.initialWorldX;
				global::Rendering.Rendering.camUp[uBufferID].Y = global::Rendering.Rendering.initialWorldY;
				global::Rendering.Rendering.camUp[uBufferID].Z = global::Rendering.Rendering.initialWorldZ;
			}
			autoRespawn = true;
			global::Players.Players.respawnEnabled = true;
			Reset_Select_Screens();
			gameState = 17;
			global::Threads.Threads.thread1End.Set();
			break;
		}
		case 17:
			global::InputHandler.InputHandler.confirmEndGameScreen = false;
			global::Threads.Threads.thread1Task = 6;
			global::Threads.Threads.thread1Start.Set();
			global::Threads.Threads.thread3Task = 2;
			global::Threads.Threads.thread3Start.Set();
			gameState = 2;
			break;
		case 2:
			global::Threads.Threads.thread1Task = 6;
			global::Threads.Threads.thread1Start.Set();
			global::Threads.Threads.thread3Task = 2;
			global::Threads.Threads.thread3Start.Set();
			break;
		case 3:
			Entering_Menu_State();
			mainC.userInterface.Load_In_Game_Menu();
			Calculate_Frame_Time();
			Save_Frame_Time();
			gameState = 4;
			global::Threads.Threads.thread1End.Set();
			break;
		case 4:
			Calculate_Frame_Time();
			mainC.inputMain.Handle_Menu(0);
			if (!global::InputHandler.InputHandler.inMenu)
			{
				mainC.maingameMain.Leaving_Menu_State();
			}
			mainC.soundsMain.Process_Sounds();
			mainC.gameLogic.Game_Update_Camera(0);
			global::Threads.Threads.thread1End.Set();
			break;
		case 5:
			Leaving_Menu_State();
			Restore_Frame_Time();
			global::Threads.Threads.thread1Task = 6;
			global::Threads.Threads.thread1Start.Set();
			global::Threads.Threads.thread3Task = 2;
			global::Threads.Threads.thread3Start.Set();
			gameState = 2;
			break;
		case 6:
			global::Rendering.Rendering.mbRespawn = false;
			if (spSaving > 3)
			{
				if (roundCurrentTime < 0f)
				{
					spSaving = 0;
					spGameReady = false;
					createNewSPRound = true;
					gameState = 7;
					mainC.inputMain.Switch_To_Menu(12);
				}
				if (linearProgression)
				{
					roundCurrentTime -= frametime;
				}
			}
			else if (spSaving == 3)
			{
				Save_SP_Game((byte)(numLives + 1));
			}
			else
			{
				spSaving++;
			}
			Calculate_Frame_Time();
			global::Threads.Threads.thread1End.Set();
			break;
		case 7:
			Main_Loop_Threaded_SP_LoadingLevel();
			Calculate_Frame_Time();
			global::Threads.Threads.thread1End.Set();
			break;
		case 8:
			global::Rendering.Rendering.renderMenuScreen = 1;
			if (roundCurrentTime < 0f)
			{
				gameState = 1;
				mainC.userInterface.Load_Main_Menu();
			}
			Calculate_Frame_Time();
			global::Threads.Threads.thread1End.Set();
			break;
		case 9:
			gameState = 2;
			Calculate_Frame_Time();
			global::Threads.Threads.thread1End.Set();
			break;
		case 10:
			roundCurrentTime -= frametime;
			global::Rendering.Rendering.mbRespawn = false;
			if (spSaving > 3)
			{
				if (roundCurrentTime < 0f)
				{
					global::Rendering.Rendering.renderMenuScreen = 4;
					if (global::InputHandler.InputHandler.controllerButtonBPressed)
					{
						spSaving = 0;
						gameState = 1;
						global::Rendering.Rendering.mbTrialOver = false;
						mainC.userInterface.Load_Main_Menu();
					}
					else if (global::InputHandler.InputHandler.controllerButtonAPressed)
					{
						global::Rendering.Rendering.mbTrialOver = false;
						mainC.soundsMain.Stop_All_Game_Sounds(stopNarrator: true);
						mainC.inputMain.Switch_To_Menu(17);
						gameState = 11;
					}
				}
			}
			else if (spSaving == 3)
			{
				mainC.weaponsMain.Move_Weapon_Rounds_To_Ammo_Clip_Surplus(0);
				Save_SP_Game((byte)(numLives + 1));
			}
			else
			{
				spSaving++;
			}
			Calculate_Frame_Time();
			global::Threads.Threads.thread1End.Set();
			break;
		case 11:
			if (!trialMode)
			{
				if (global::InputHandler.InputHandler.controllerButtonAPressed || global::InputHandler.InputHandler.controllerButtonBPressed)
				{
					spSaving = 1;
					roundCurrentTime = 2f;
					gameLevel = 3;
					gameState = 6;
					mainC.inputMain.Switch_To_Menu(16);
					mainC.gameLogic.Game_Show_Results_Window();
				}
			}
			else
			{
				mainC.inputMain.Handle_Menu(0);
				if (!global::InputHandler.InputHandler.inMenu)
				{
					gameState = 1;
					mainC.userInterface.Load_Main_Menu();
				}
			}
			Calculate_Frame_Time();
			global::Threads.Threads.thread1End.Set();
			break;
		case 12:
			Main_Loop_SP_Round_Over(0);
			global::Threads.Threads.thread1End.Set();
			break;
		case 13:
			Entering_Menu_State();
			mainC.inputMain.Switch_To_Menu(10);
			Calculate_Frame_Time();
			gameState = 14;
			global::Threads.Threads.thread1End.Set();
			break;
		case 14:
			Calculate_Frame_Time();
			mainC.inputMain.Handle_Menu(0);
			if (!global::InputHandler.InputHandler.inMenu)
			{
				mainC.maingameMain.Leaving_Menu_State();
			}
			global::Threads.Threads.thread1End.Set();
			break;
		case 15:
			Leaving_Menu_State();
			Calculate_Frame_Time();
			gameState = 12;
			global::Threads.Threads.thread1End.Set();
			break;
		case 18:
			global::InputHandler.InputHandler.confirmEndGameScreen = false;
			Main_Loop_Race_Countdown_SP(0);
			global::Threads.Threads.thread1End.Set();
			break;
		case 20:
			Calculate_Frame_Time();
			global::Threads.Threads.thread1End.Set();
			break;
		case 22:
			mainC.userInterface.Load_Game_Over();
			showCrossHairs[1] = 1;
			Sync_All_Game_Items();
			Reset_Select_Screens();
			Stop_Game_Functions(stopNarrator: false);
			Calculate_Frame_Time();
			gameState = 23;
			global::Threads.Threads.thread1End.Set();
			break;
		case 23:
			showCrossHairs[1] = 1;
			Calculate_Frame_Time();
			global::Threads.Threads.thread1End.Set();
			break;
		case 16:
			global::Threads.Threads.thread1End.Set();
			break;
		case 149:
			global::Rendering.Rendering.renderMenuScreen = 0;
			autoRespawn = true;
			global::Players.Players.respawnEnabled = true;
			if (global::Players.Players.players[0].onmap != 1)
			{
				byte b = gameType;
				if (b == 4)
				{
					gameState = 148;
					if (raceFinished[0] == 1)
					{
						gameState = 141;
					}
					Calculate_Frame_Time();
					global::Threads.Threads.thread1End.Set();
					break;
				}
				if (global::Players.Players.players[0].onmap != 1)
				{
					mainC.soundsMain.Play_Level_Music();
					Reset_Select_Screens();
					gameState = 141;
				}
			}
			if (!isCommander)
			{
				global::Threads.Threads.thread1Task = 0;
			}
			else
			{
				global::Threads.Threads.thread1Task = 3;
			}
			global::Threads.Threads.thread1Start.Set();
			global::Threads.Threads.thread3Task = 2;
			global::Threads.Threads.thread3Start.Set();
			break;
		case 141:
			global::Rendering.Rendering.renderMenuScreen &= 252;
			if (!isCommander)
			{
				global::Threads.Threads.thread1Task = 0;
			}
			else
			{
				global::Threads.Threads.thread1Task = 3;
			}
			global::Threads.Threads.thread1Start.Set();
			global::Threads.Threads.thread3Task = 2;
			global::Threads.Threads.thread3Start.Set();
			break;
		case 142:
			mainC.userInterface.Load_In_Game_Menu();
			Entering_Menu_State();
			global::Joints.Joints.Sync_Player_Matrices(0, global::Rendering.Rendering.rBufferID, global::Rendering.Rendering.uBufferID);
			Calculate_Frame_Time();
			global::Threads.Threads.thread1End.Set();
			break;
		case 143:
			if (!isCommander)
			{
				global::Threads.Threads.thread1Task = 1;
			}
			else
			{
				global::Threads.Threads.thread1Task = 4;
			}
			global::Threads.Threads.thread1Start.Set();
			global::Threads.Threads.thread3Task = 2;
			global::Threads.Threads.thread3Start.Set();
			break;
		case 144:
			global::Rendering.Rendering.renderMenuScreen &= 252;
			Leaving_Menu_State();
			Calculate_Frame_Time();
			global::Threads.Threads.thread1End.Set();
			break;
		case 138:
			global::Sounds.Sounds.soundEnabled[0] = false;
			global::Sounds.Sounds.soundEnabled[2] = false;
			Entering_Menu_State();
			Calculate_Frame_Time();
			global::Threads.Threads.thread1End.Set();
			break;
		case 139:
			if (!isCommander)
			{
				global::Threads.Threads.thread1Task = 2;
			}
			else
			{
				global::Threads.Threads.thread1Task = 4;
			}
			global::Threads.Threads.thread1Start.Set();
			global::Threads.Threads.thread3Task = 2;
			global::Threads.Threads.thread3Start.Set();
			break;
		case 140:
			global::Sounds.Sounds.soundEnabled[0] = true;
			global::Sounds.Sounds.soundEnabled[2] = true;
			global::Rendering.Rendering.renderMenuScreen &= 252;
			gameState = 149;
			if (!isCommander)
			{
				global::Threads.Threads.thread1Task = 0;
			}
			else
			{
				global::Threads.Threads.thread1Task = 3;
			}
			global::Threads.Threads.thread1Start.Set();
			global::Threads.Threads.thread3Task = 2;
			global::Threads.Threads.thread3Start.Set();
			break;
		case 129:
		case 133:
		case 136:
			Main_Loop_Threaded_MP_In_Lobby(0);
			global::Threads.Threads.thread1End.Set();
			break;
		case 130:
			global::Rendering.Rendering.renderMenuScreen &= 253;
			if (enteringSettingsMenu)
			{
				mainC.inputMain.Switch_To_Menu(2);
			}
			else
			{
				mainC.userInterface.Load_Main_Menu();
			}
			Entering_Menu_State();
			Calculate_Frame_Time();
			global::Threads.Threads.thread1End.Set();
			break;
		case 132:
			global::Rendering.Rendering.renderMenuScreen &= 254;
			global::Rendering.Rendering.renderMenuScreen |= 2;
			Leaving_Menu_State();
			Calculate_Frame_Time();
			global::Threads.Threads.thread1End.Set();
			break;
		case 145:
			Main_Loop_Threaded_MP_RoundOver(0);
			Calculate_Frame_Time();
			global::Threads.Threads.thread1End.Set();
			break;
		case 146:
			enteringSettingsMenu = false;
			global::Rendering.Rendering.renderMenuScreen = 2;
			Main_Loop_Threaded_MP_In_Lobby(0);
			Calculate_Frame_Time();
			global::Threads.Threads.thread1End.Set();
			break;
		case 147:
			global::Rendering.Rendering.renderMenuScreen = 1;
			if (roundCurrentTime < 0f)
			{
				gameState = 1;
				mainC.userInterface.Load_Main_Menu();
			}
			Calculate_Frame_Time();
			global::Threads.Threads.thread1End.Set();
			break;
		case 148:
			Main_Loop_Race_Countdown_MP(0);
			global::Threads.Threads.thread1End.Set();
			break;
		case 150:
			global::Rendering.Rendering.renderMenuScreen = 1;
			Calculate_Frame_Time();
			global::Threads.Threads.thread1End.Set();
			break;
		case 26:
		{
			curIdleTime = 0f;
			mainC.userInterface.Close_Window(9);
			mainC.userInterface.Close_Window(10);
			mainC.userInterface.Close_Window(0);
			mainC.userInterface.Close_Window(5);
			mainC.userInterface.Close_Window(24);
			if (mainC.gameLogic.Game_Showing_Vehicle_Select())
			{
				if (!User_Interface.vehicleSelectScreenOpen)
				{
					mainC.userInterface.Load_Vehicle_Select();
				}
			}
			else
			{
				User_Interface.vehicleSelectScreenOpen = false;
				User_Interface.vehicleSelectFinished = false;
				gameState = 25;
			}
			showCrossHairs[1] = 1;
			global::InputHandler.InputHandler.confirmEndGameScreen = false;
			_ = frametime;
			byte uBufferID = global::Rendering.Rendering.uBufferID;
			if ((showGameFlags & 1) > 0)
			{
				global::Rendering.Rendering.renderMenuScreen = 0;
				if (gameMode == 1)
				{
					if (!isCommander)
					{
						global::Threads.Threads.thread1Task = 1;
					}
					else
					{
						global::Threads.Threads.thread1Task = 4;
					}
				}
			}
			else
			{
				global::Rendering.Rendering.renderMenuScreen = 1;
				if (gameMode == 1)
				{
					if (!isCommander)
					{
						global::Threads.Threads.thread1Task = 2;
					}
					else
					{
						global::Threads.Threads.thread1Task = 4;
					}
				}
			}
			if (gameMode == 1)
			{
				global::Threads.Threads.thread1Start.Set();
				global::Threads.Threads.thread3Task = 2;
				global::Threads.Threads.thread3Start.Set();
			}
			else
			{
				Calculate_Frame_Time();
				global::Threads.Threads.thread1End.Set();
			}
			break;
		}
		case 25:
		{
			curIdleTime = 0f;
			if (mainC.gameLogic.Game_Showing_Weapon_Select())
			{
				if (!User_Interface.weaponSelectScreenOpen)
				{
					mainC.userInterface.Load_Weapon_Select();
				}
			}
			else
			{
				User_Interface.weaponSelectScreenOpen = false;
				User_Interface.weaponSelectFinished = false;
				gameState = 24;
			}
			showCrossHairs[1] = 1;
			global::InputHandler.InputHandler.confirmEndGameScreen = false;
			_ = frametime;
			byte uBufferID = global::Rendering.Rendering.uBufferID;
			if ((showGameFlags & 2) > 0)
			{
				global::Rendering.Rendering.renderMenuScreen = 0;
				if (gameMode == 1)
				{
					if (!isCommander)
					{
						global::Threads.Threads.thread1Task = 1;
					}
					else
					{
						global::Threads.Threads.thread1Task = 4;
					}
				}
			}
			else
			{
				global::Rendering.Rendering.renderMenuScreen = 1;
				if (gameMode == 1)
				{
					if (!isCommander)
					{
						global::Threads.Threads.thread1Task = 2;
					}
					else
					{
						global::Threads.Threads.thread1Task = 4;
					}
				}
			}
			if (gameMode == 1)
			{
				global::Threads.Threads.thread1Start.Set();
				global::Threads.Threads.thread3Task = 2;
				global::Threads.Threads.thread3Start.Set();
			}
			else
			{
				Calculate_Frame_Time();
				global::Threads.Threads.thread1End.Set();
			}
			break;
		}
		case 24:
			curIdleTime = 0f;
			if (mainC.gameLogic.Game_Showing_Mission_Objectives())
			{
				if (!User_Interface.missionObjectivesScreenOpen)
				{
					mainC.userInterface.Load_Mission_Objectives();
				}
			}
			else
			{
				User_Interface.missionObjectivesScreenOpen = false;
				User_Interface.missionObjectivesFinished = false;
				if (gameMode == 0)
				{
					gameState = 21;
				}
				else if (gameMode == 1)
				{
					gameState = 149;
				}
				else
				{
					mainC.userInterface.Load_Main_Menu();
					gameState = 1;
				}
			}
			if ((showGameFlags & 4) > 0)
			{
				global::Rendering.Rendering.renderMenuScreen = 0;
				if (gameMode == 1)
				{
					if (!isCommander)
					{
						global::Threads.Threads.thread1Task = 1;
					}
					else
					{
						global::Threads.Threads.thread1Task = 4;
					}
				}
			}
			else
			{
				global::Rendering.Rendering.renderMenuScreen = 1;
				if (gameMode == 1)
				{
					if (!isCommander)
					{
						global::Threads.Threads.thread1Task = 2;
					}
					else
					{
						global::Threads.Threads.thread1Task = 4;
					}
				}
			}
			if (gameMode == 1)
			{
				global::Threads.Threads.thread1Start.Set();
				global::Threads.Threads.thread3Task = 2;
				global::Threads.Threads.thread3Start.Set();
			}
			else
			{
				Calculate_Frame_Time();
				global::Threads.Threads.thread1End.Set();
			}
			break;
		case 254:
			mainC.userInterface.Close_Window(15);
			gameState = 1;
			global::Rendering.Rendering.renderMenuScreen = 1;
			if (gameMode == 1 && global::Networking.Networking.networkSessionReady)
			{
				mainC.networkingMain.XBOX_Close_Session();
			}
			mainC.userInterface.Load_Main_Menu();
			mainC.gameLogic.Game_Misc_Threaded(1);
			Calculate_Frame_Time();
			global::Threads.Threads.thread1End.Set();
			break;
		}
	}

	public void Init_MainGame()
	{
		gameData.numPlayers = 1;
		gameData.players = new StructsClass.GameInfoPlayer[44];
		gameData.numAllocatedPlayers = 44;
		gameData.floatSize = 0;
		gameData.intSize = 0;
		gameData.ushortDataSize = 0;
		lastGameData.numPlayers = 1;
		lastGameData.players = new StructsClass.GameInfoPlayer[44];
		lastGameData.numAllocatedPlayers = 44;
		lastGameData.floatSize = 0;
		lastGameData.intSize = 0;
		lastGameData.ushortDataSize = 0;
		for (ushort num = 0; num < 5; num++)
		{
			Commander[num] = -1;
		}
		mainC.gameLogic.Game_Initialize_GameData_Scores();
	}

	public static void Main_Loop_Threaded_SP_Gameplay(byte threadID)
	{
		if (Guide.IsVisible || curIdleTime > idleTimeout)
		{
			mainC.maingameMain.Pause_Game();
		}
		gameTime += frameTimeAdjusted;
		byte uBufferID = global::Rendering.Rendering.uBufferID;
		global::InputHandler.InputHandler.lookMode = 0;
		mainC.renderingMain.Process_Particles();
		mainC.playersMain.New_Frame_SP();
		mainC.gameLogic.Game_SP_Start_Of_Update();
		mainC.gameLogic.Game_SP_Handle_Input(threadID);
		mainC.weaponsMain.Process_Ballistics(threadID);
		mainC.Explosions.Process_Explosions(threadID);
		mainC.playersMain.Move_Main_Player(frametime, threadID);
		mainC.weaponsMain.Get_LaserSite_Position(0f, 1, commanderObjectivePosition, threadID);
		mainC.weaponsMain.Process_Player_Weapons(0, global::Players.Players.players[0].primaryWeaponMountWeapon);
		global::Joints.Joints.Do_Joint_Basic_Calculations(frametime);
		mainC.gameobjectMain.Process_Objects(threadID);
		mainC.targetMain.Process_Targets(frametime);
		mainC.aiMain.Process_AI_SP(frametime, threadID);
		mainC.playersMain.Process_Players(frametime, threadID);
		if (!global::Players.Players.freezeCamera)
		{
			mainC.soundsMain.Set_Listener_Position(global::Rendering.Rendering.camPos[uBufferID].X, global::Rendering.Rendering.camPos[uBufferID].Y, global::Rendering.Rendering.camPos[uBufferID].Z, ref global::Players.Players.players[0].charP.velocity);
		}
		mainC.pickupsMain.Process_Pickups_SP(threadID);
		global::AI.AI.finishRoute = true;
		mainC.soundsMain.Update_Sounds(global::Players.Players.players[0].charP.position.v[0] + 20f, global::Players.Players.players[0].charP.position.v[1] + 10f, global::Players.Players.players[0].charP.position.v[2]);
		mainC.maingameMain.Calculate_Frame_Time();
		mainC.soundsMain.Process_Sounds();
		global::Players.Players.players[0].posX[uBufferID] = global::Players.Players.players[0].charP.position.v[0];
		global::Players.Players.players[0].posY[uBufferID] = global::Players.Players.players[0].charP.position.v[1];
		global::Players.Players.players[0].posZ[uBufferID] = global::Players.Players.players[0].charP.position.v[2];
		if (global::Players.Players.players[0].voiceCueID > -1)
		{
			mainC.soundsMain.Update_Voice_Position(global::Players.Players.players[0].voiceCueID, global::Players.Players.players[0].charP.position.v[0], global::Players.Players.players[0].charP.position.v[1], global::Players.Players.players[0].charP.position.v[2]);
		}
		if (tauntingEnabled)
		{
			mainC.playersMain.Check_For_Taunting(0, tauntDistanceSqr);
		}
		mainC.vehicles.Update_Vehicle_Avatar_Matrix(0);
		for (byte b = 1; b < maxGamePlayers; b++)
		{
			if (global::Players.Players.players[b].active)
			{
				global::Players.Players.players[b].posX[uBufferID] = global::Players.Players.players[b].charP.position.v[0];
				global::Players.Players.players[b].posY[uBufferID] = global::Players.Players.players[b].charP.position.v[1];
				global::Players.Players.players[b].posZ[uBufferID] = global::Players.Players.players[b].charP.position.v[2];
				mainC.vehicles.Update_Vehicle_Avatar_Matrix(b);
				if (global::Players.Players.players[b].voiceCueID > -1)
				{
					mainC.soundsMain.Update_Voice_Position(global::Players.Players.players[b].voiceCueID, global::Players.Players.players[b].charP.position.v[0], global::Players.Players.players[b].charP.position.v[1], global::Players.Players.players[b].charP.position.v[2]);
				}
			}
		}
		mainC.gameLogic.Game_Update_Camera(threadID);
		mainC.gameLogic.Game_SP_End_Of_Update(threadID);
		if (roundOver)
		{
			mainC.maingameMain.SP_Level_Complete(threadID);
		}
		else if (global::Players.Players.players[0].onmap == 1 && gameState == 2)
		{
			global::Players.Players.respawnEnabled = false;
			gameState = 26;
		}
		global::Threads.Threads.thread3End.WaitOne();
	}

	public static void Main_Loop_SP_Round_Over(byte threadID)
	{
		_ = global::Rendering.Rendering.uBufferID;
		_ = global::Rendering.Rendering.rBufferID;
		mainC.userInterface.Close_Windows_For_End_Of_Round();
		mainC.avatarMain.Process_Avatars();
		global::Rendering.Rendering.renderMenuScreen = 1;
		usingIronSights = false;
		usingScope = false;
		quickScope = false;
		overheadView = false;
		mainC.playersMain.Reset_Local_Player_Variables_On_Round_Over();
		mainC.maingameMain.Calculate_Frame_Time();
	}

	public static void Main_Loop_Threaded_SP_LoadingLevel()
	{
		if (spGameReady)
		{
			if (linearProgression)
			{
				numLives++;
			}
			gameState = 26;
			global::Rendering.Rendering.mbRespawn = true;
			global::Rendering.Rendering.rotateSplash = true;
			mainC.soundsMain.Level_Reset();
		}
		else if (createNewSPRound)
		{
			mainC.userInterface.Close_Window(10);
			createNewSPRound = false;
			global::Threads.Threads.thread2Task = 1;
			global::Threads.Threads.thread2Start.Set();
		}
		mainC.soundsMain.Process_Sounds();
	}

	public void Main_Loop_Race_Countdown_SP(byte threadID)
	{
		byte uBufferID = global::Rendering.Rendering.uBufferID;
		global::InputHandler.InputHandler.lookMode = 0;
		gameTime += frameTimeAdjusted;
		mainC.renderingMain.Process_Particles();
		if (global::Players.Players.respawnEnabled && global::Players.Players.players[0].dead && global::Players.Players.players[0].onmap == 1)
		{
			mainC.playersMain.Player_Respawn(threadID);
		}
		mainC.weaponsMain.Process_Ballistics(threadID);
		mainC.Explosions.Process_Explosions(threadID);
		mainC.weaponsMain.Process_Player_Weapons(0, global::Players.Players.players[0].primaryWeaponMountWeapon);
		global::Joints.Joints.Do_Joint_Basic_Calculations(frametime);
		mainC.gameobjectMain.Process_Objects(threadID);
		mainC.playersMain.Process_Players(frametime, threadID);
		mainC.gameLogic.Game_Update_Camera(threadID);
		if (!global::Players.Players.freezeCamera)
		{
			switch (global::Players.Players.currentView)
			{
			case 0:
			case 1:
				mainC.soundsMain.Set_Listener_Position(global::Rendering.Rendering.cameraPositionsX[global::Players.Players.currentView], global::Rendering.Rendering.cameraPositionsY[global::Players.Players.currentView], global::Rendering.Rendering.cameraPositionsZ[global::Players.Players.currentView], ref global::Players.Players.players[0].charP.velocity);
				break;
			case 2:
			case 3:
				mainC.soundsMain.Set_Listener_Position(global::Rendering.Rendering.camPos[uBufferID].X, global::Rendering.Rendering.camPos[uBufferID].Y, global::Rendering.Rendering.camPos[uBufferID].Z, ref global::Players.Players.players[0].charP.velocity);
				break;
			}
		}
		mainC.pickupsMain.Process_Pickups_SP(threadID);
		global::AI.AI.finishRoute = true;
		mainC.soundsMain.Process_Sounds();
		for (byte b = 0; b < maxGamePlayers; b++)
		{
			if (global::Players.Players.players[b].active)
			{
				global::Players.Players.players[b].posX[uBufferID] = global::Players.Players.players[b].charP.position.v[0];
				global::Players.Players.players[b].posY[uBufferID] = global::Players.Players.players[b].charP.position.v[1];
				global::Players.Players.players[b].posZ[uBufferID] = global::Players.Players.players[b].charP.position.v[2];
			}
		}
		mainC.maingameMain.Calculate_Frame_Time();
		raceStartTimer -= frametime;
	}

	public static void Main_Loop_Threaded_MP_Gameplay(byte threadID)
	{
		byte uBufferID = global::Rendering.Rendering.uBufferID;
		global::InputHandler.InputHandler.lookMode = 0;
		gameTime += frameTimeAdjusted;
		mainC.renderingMain.Process_Particles();
		mainC.gameLogic.Game_MP_Start_Of_Update();
		mainC.playersMain.New_Frame_MP();
		mainC.gameLogic.Game_MP_Handle_Input(threadID);
		mainC.weaponsMain.Reset_Network_Player_Particle_Check();
		mainC.networkingMain.XBOX_Process_Networking(0);
		mainC.weaponsMain.Process_Ballistics(threadID);
		mainC.Explosions.Process_Explosions(threadID);
		if (commanderMode)
		{
			Commander_Process_Objectives();
		}
		mainC.playersMain.Move_Main_Player(frametime, threadID);
		mainC.playersMain.Send_Player_Location();
		mainC.weaponsMain.Get_LaserSite_Position(0f, 1, commanderObjectivePosition, threadID);
		mainC.weaponsMain.Process_Player_Weapons(0, global::Players.Players.players[0].primaryWeaponMountWeapon);
		global::Joints.Joints.Do_Joint_Basic_Calculations(frametime);
		mainC.gameobjectMain.Process_Objects(threadID);
		mainC.targetMain.Process_Targets(frametime);
		if (global::AI.AI.numAI > 0)
		{
			mainC.aiMain.Process_AI_MP(frametime, threadID);
		}
		mainC.playersMain.Process_Players(frametime, threadID);
		if (!global::Players.Players.freezeCamera)
		{
			mainC.soundsMain.Set_Listener_Position(global::Rendering.Rendering.camPos[uBufferID].X, global::Rendering.Rendering.camPos[uBufferID].Y, global::Rendering.Rendering.camPos[uBufferID].Z, ref global::Players.Players.players[0].charP.velocity);
		}
		mainC.pickupsMain.Process_Pickups_MP(threadID);
		if (global::Players.Players.players[0].onmap == 1 && gameState == 141)
		{
			global::Players.Players.respawnEnabled = false;
			gameState = 26;
		}
		if (tauntingEnabled)
		{
			mainC.playersMain.Check_For_Taunting(0, tauntDistanceSqr);
		}
		Main_Loop_Common_MP_End(threadID);
		global::Threads.Threads.thread3End.WaitOne();
		if (global::AI.AI.sendAIRoute)
		{
			mainC.aiMain.Send_AI_New_Route_Info();
		}
	}

	public static void Main_Loop_Threaded_MP_Gameplay_Paused_For_Menus(byte threadID)
	{
		byte uBufferID = global::Rendering.Rendering.uBufferID;
		gameTime += frameTimeAdjusted;
		if (global::Players.Players.players[0].dead || global::Players.Players.players[0].onmap != 4)
		{
			sprinting = false;
			global::Players.Players.moving = 0;
			global::Players.Players.playerSpeedSideways = 0f;
			global::Players.Players.playerSpeed = 0f;
			sideStepping = false;
			walking = false;
			walkingBackwards = false;
			global::Rendering.Rendering.viewMovement = 0f;
			global::Players.Players.footStepTimer = 0f;
			global::InputHandler.InputHandler.controllerStickRightValX = 0f;
			global::InputHandler.InputHandler.controllerStickRightValY = 0f;
			global::Players.Players.playerSpeedRotateRightStick = 0f;
			global::Players.Players.playerSpeedElevateRightStick = 0f;
			global::Players.Players.xRotation = 0f;
		}
		mainC.renderingMain.Process_Particles();
		mainC.gameLogic.Game_MP_Start_Of_Update();
		mainC.inputMain.Handle_Menu(threadID);
		if (!global::InputHandler.InputHandler.inMenu)
		{
			mainC.maingameMain.Leaving_Menu_State();
		}
		if (global::Players.Players.players[0].dead || global::Players.Players.players[0].onmap != 4)
		{
			if (global::Players.Players.players[0].dead && (global::Players.Players.players[0].onmap & 9) > 0)
			{
				if (global::Players.Players.players[0].onmap == 1)
				{
					global::Players.Players.respawnTimer -= frametime;
					if (global::Players.Players.respawnTimer < 0f)
					{
						global::InputHandler.InputHandler.controllerButtonAPressed = true;
					}
				}
				else
				{
					curTimeBeforeExitingMapOnDeath -= frametime;
					if (curTimeBeforeExitingMapOnDeath < 0f)
					{
						global::Players.Players.players[0].onmap = 2;
						global::Players.Players.players[0].transporter = 2f;
						global::Players.Players.players[0].transporterDirection = -1;
						global::Players.Players.players[0].respawnParticle = mainC.renderingMain.New_Particle_Spawn(9, global::Players.Players.players[0].respawnParticle, 0, global::Players.Players.players[0].charP.position.v[0], global::Players.Players.players[0].charP.position.v[1], global::Players.Players.players[0].charP.position.v[2]);
					}
				}
			}
			if (global::Players.Players.respawnEnabled && (global::InputHandler.InputHandler.controllerButtonAPressed || autoRespawn) && global::Players.Players.players[0].dead && (global::Players.Players.players[0].onmap & 3) > 0)
			{
				if (global::Players.Players.players[0].onmap == 2)
				{
					global::Players.Players.players[0].timeBeforeRespawn[uBufferID] -= frametime;
					if (global::Players.Players.players[0].timeBeforeRespawn[uBufferID] < 0f)
					{
						mainC.playersMain.Player_Spawn_Time_Over(0);
					}
				}
				else
				{
					mainC.playersMain.Player_Respawn(threadID);
				}
			}
		}
		global::Players.Players.players[0].shooting = false;
		mainC.weaponsMain.firingStoppedAllPlayerWeapons(0);
		mainC.networkingMain.XBOX_Process_Networking(0);
		mainC.weaponsMain.Process_Ballistics(threadID);
		mainC.Explosions.Process_Explosions(threadID);
		mainC.playersMain.Move_Main_Player(frametime, threadID);
		mainC.playersMain.Send_Player_Location();
		mainC.weaponsMain.Process_Player_Weapons(0, global::Players.Players.players[0].primaryWeaponMountWeapon);
		mainC.gameobjectMain.Process_Objects(threadID);
		mainC.targetMain.Process_Targets(frametime);
		if (global::AI.AI.numAI > 0)
		{
			mainC.aiMain.Process_AI_MP(frametime, threadID);
		}
		mainC.playersMain.Process_Players(frametime, threadID);
		if (!global::Players.Players.freezeCamera)
		{
			switch (global::Players.Players.currentView)
			{
			case 0:
			case 1:
				mainC.soundsMain.Set_Listener_Position(global::Rendering.Rendering.cameraPositionsX[global::Players.Players.currentView], global::Rendering.Rendering.cameraPositionsY[global::Players.Players.currentView], global::Rendering.Rendering.cameraPositionsZ[global::Players.Players.currentView], ref global::Players.Players.players[0].charP.velocity);
				break;
			case 2:
			case 3:
				mainC.soundsMain.Set_Listener_Position(global::Rendering.Rendering.camPos[uBufferID].X, global::Rendering.Rendering.camPos[uBufferID].Y, global::Rendering.Rendering.camPos[uBufferID].Z, ref global::Players.Players.players[0].charP.velocity);
				break;
			}
		}
		mainC.pickupsMain.Process_Pickups_MP(threadID);
		Main_Loop_Common_MP_End(threadID);
		global::Threads.Threads.thread3End.WaitOne();
		if (global::AI.AI.sendAIRoute)
		{
			mainC.aiMain.Send_AI_New_Route_Info();
		}
	}

	public static void Main_Loop_Threaded_MP_Paused_For_Menus_Not_Playing(byte threadID)
	{
		_ = global::Rendering.Rendering.uBufferID;
		gameTime += frameTimeAdjusted;
		mainC.renderingMain.Process_Particles();
		mainC.gameLogic.Game_MP_Start_Of_Update();
		if (global::InputHandler.InputHandler.controllerButtonStartPressed || global::InputHandler.InputHandler.controllerButtonBackPressed)
		{
			mainC.maingameMain.Leaving_Menu_State();
		}
		else
		{
			mainC.inputMain.Handle_Menu(threadID);
			if (!global::InputHandler.InputHandler.inMenu)
			{
				mainC.maingameMain.Leaving_Menu_State();
			}
		}
		if (global::Players.Players.players[0].dead || global::Players.Players.players[0].onmap == 1)
		{
			global::Players.Players.respawnTimer -= frametime;
			if (global::Players.Players.respawnTimer < 0f)
			{
				global::InputHandler.InputHandler.controllerButtonAPressed = true;
			}
		}
		global::Players.Players.players[0].shooting = false;
		mainC.weaponsMain.firingStoppedAllPlayerWeapons(0);
		mainC.networkingMain.XBOX_Process_Networking(0);
		mainC.weaponsMain.Process_Ballistics(threadID);
		mainC.Explosions.Process_Explosions(threadID);
		mainC.gameobjectMain.Process_Objects(threadID);
		mainC.targetMain.Process_Targets(frametime);
		if (global::AI.AI.numAI > 0)
		{
			mainC.aiMain.Process_AI_MP(frametime, threadID);
		}
		mainC.playersMain.Process_Players(frametime, threadID);
		mainC.pickupsMain.Process_Pickups_MP(threadID);
		Main_Loop_Common_MP_End(threadID);
		for (ushort num = 0; num < maxGamePlayers; num++)
		{
			global::Players.Players.players[num].posX[0] = global::Players.Players.players[num].charP.position.v[0];
			global::Players.Players.players[num].posY[0] = global::Players.Players.players[num].charP.position.v[1];
			global::Players.Players.players[num].posZ[0] = global::Players.Players.players[num].charP.position.v[2];
			global::Players.Players.players[num].posX[1] = global::Players.Players.players[num].charP.position.v[0];
			global::Players.Players.players[num].posY[1] = global::Players.Players.players[num].charP.position.v[1];
			global::Players.Players.players[num].posZ[1] = global::Players.Players.players[num].charP.position.v[2];
		}
		global::Threads.Threads.thread3End.WaitOne();
		if (global::AI.AI.sendAIRoute)
		{
			mainC.aiMain.Send_AI_New_Route_Info();
		}
	}

	public static void Main_Loop_Threaded_MP_In_Lobby(byte threadID)
	{
		bool flag = false;
		if (global::Networking.Networking.networkState == 0)
		{
			mainC.userInterface.Close_All_Windows();
			gameState = 254;
			return;
		}
		if (mpGameDataReady && haveProgramData && haveAllPlayerStatus && global::Networking.Networking.networkPlayers[0].haveRemotePlayerArrayPosition)
		{
			flag = true;
		}
		if (global::Networking.Networking.isHost && !global::Networking.Networking.wasHost)
		{
			becameMPHost = true;
		}
		if (mpGameDataReady && !haveAllPlayerStatus)
		{
			haveAllPlayerStatus = true;
			for (ushort num = 1; num < 4; num++)
			{
				if (global::Players.Players.players[num].active && !global::Networking.Networking.networkPlayers[num].haveAllRemotePlayerDataForStart)
				{
					if (!global::Networking.Networking.networkPlayers[num].haveRemotePlayerStatus || !global::Networking.Networking.networkPlayers[num].haveRemotePlayerPosition || !global::Networking.Networking.networkPlayers[num].haveRemotePlayerArrayPosition || !global::Networking.Networking.networkPlayers[num].haveRemotePlayerTeam)
					{
						haveAllPlayerStatus = false;
						break;
					}
					global::Networking.Networking.networkPlayers[num].haveAllRemotePlayerDataForStart = true;
				}
			}
		}
		switch (gameState)
		{
		case 129:
			if (lobbyMapVoteTimer >= 0f)
			{
				lobbyMapVoteTimer -= frametime;
				needToTallyMapVotes = true;
			}
			mainC.gameLogic.Game_UI_Update_Results_Window_Network_Info(10);
			if (global::Networking.Networking.isHost)
			{
				haveProgramData = true;
				if (mainC.networkingMain.All_Players_Ready())
				{
					autoStartMPGame = true;
				}
				else
				{
					float num2 = mainC.networkingMain.Percentage_Of_Ready_Players();
					lobbyTimer -= frametime * (1f + num2 * 12.5f);
					if (lobbyTimer < 0.5f)
					{
						autoStartMPGame = true;
					}
				}
				if (global::Networking.Networking.networkState == 2)
				{
					gameState = 133;
				}
				if (!hostStartedGame && !autoStartMPGame)
				{
					break;
				}
				if (needToTallyMapVotes && !mapManuallySet && !linearProgression)
				{
					mainC.maingameMain.Calculate_Map_Votes();
					ushort selected_Map = mainC.maingameMain.Get_Selected_Map();
					if (selected_Map != gameLevel)
					{
						needToTallyMapVotes = true;
						mpGameDataReady = false;
						gameLevel = (byte)selected_Map;
						global::Networking.Networking.networkSession.SessionProperties[3] = gameLevel;
						mainC.gameLogic.Game_Send_GameSettings(1);
					}
					needToTallyMapVotes = false;
				}
				mapManuallySet = false;
				byte b = gameType;
				if (b == 4)
				{
					mainC.maingameMain.Send_Race_Starting_Participants();
				}
				autoStartMPGame = false;
				hostStartedGame = false;
				roundCurrentTime = roundTimeLimit;
				mainC.gameLogic.Game_Send_GameSettings(1);
				mainC.networkingMain.XBOX_Start_Game();
			}
			else if (global::Networking.Networking.networkState == 2)
			{
				gameState = 133;
				lobbyTimer = 2f;
				lobbyDataTimer = 3f;
			}
			break;
		case 133:
		case 136:
			if (flag)
			{
				if (becameMPHost)
				{
					becameMPHost = false;
					mainC.aiMain.Update_AI_Controlling_Players();
					if (!global::Networking.Networking.networkPlayers[0].haveRemotePlayerTeam)
					{
						global::Players.Players.players[0].team = mainC.playersMain.Assign_Team(-1);
						mainC.playersMain.LocalPlayer_Team_Change();
					}
				}
				MP_Game_Cleanup_Before_Start(threadID);
				mainC.aiMain.Process_AI_MP_Lobby(frametime, threadID);
				mainC.pickupsMain.Process_Pickups_MP_Lobby();
			}
			else if (mpGameDataReady)
			{
				if (global::Networking.Networking.isHost)
				{
					haveProgramData = true;
					if (!haveAllPlayerStatus)
					{
						lobbyDataTimer -= frametime;
						if (lobbyDataTimer < 0f)
						{
							lobbyDataTimer = 3f;
							for (ushort num = 1; num < 4; num++)
							{
								if (global::Players.Players.players[num].active && !global::Networking.Networking.networkPlayers[num].haveAllRemotePlayerDataForStart)
								{
									mainC.playersMain.Request_Remote_Payer_Status(num);
								}
							}
						}
					}
					else if (becameMPHost)
					{
						if (!global::Networking.Networking.networkPlayers[0].haveRemotePlayerTeam)
						{
							global::Players.Players.players[0].team = mainC.playersMain.Assign_Team(-1);
							mainC.playersMain.LocalPlayer_Team_Change();
						}
						if (global::Networking.Networking.networkPlayers[0].playerArrayPosition < 0)
						{
							mainC.playersMain.Set_Hosts_Array_Position(0);
							if (global::Networking.Networking.networkPlayers[0].playerArrayPosition < 0)
							{
								mainC.userInterface.Close_All_Windows();
								gameState = 254;
								return;
							}
						}
						mainC.aiMain.Update_AI_Controlling_Players();
						becameMPHost = false;
						MP_Game_Cleanup_Before_Start(threadID);
					}
					else
					{
						MP_Game_Cleanup_Before_Start(threadID);
					}
					mainC.aiMain.Process_AI_MP_Lobby(frametime, threadID);
					mainC.pickupsMain.Process_Pickups_MP_Lobby();
					break;
				}
				if (!haveProgramData)
				{
					lobbyDataTimer -= frametime;
					if (lobbyDataTimer < 0f)
					{
						lobbyDataTimer = 3f;
						mainC.networkingMain.XBOX_Send_Network_Message_To_Host(28);
					}
				}
				else if (!haveAllPlayerStatus)
				{
					lobbyDataTimer -= frametime;
					if (lobbyDataTimer < 0f)
					{
						lobbyDataTimer = 3f;
						for (ushort num = 1; num < 4; num++)
						{
							if (global::Players.Players.players[num].active && !global::Networking.Networking.networkPlayers[num].haveAllRemotePlayerDataForStart)
							{
								mainC.playersMain.Request_Remote_Payer_Status(num);
							}
						}
					}
				}
				else if (!global::Networking.Networking.networkPlayers[0].haveRemotePlayerArrayPosition)
				{
					lobbyDataTimer -= frametime;
					if (lobbyDataTimer < 0f)
					{
						mainC.networkingMain.XBOX_Send_Network_Message_To_Host(82);
						lobbyDataTimer = 3f;
					}
				}
				mainC.aiMain.Process_AI_MP_Lobby(frametime, threadID);
				mainC.pickupsMain.Process_Pickups_MP_Lobby();
			}
			else if (!gameSetupRunning)
			{
				mainC.userInterface.Close_Window(10);
				mainC.maingameMain.Multiplayer_Start_New_Game_Process();
			}
			break;
		case 146:
			global::InputHandler.InputHandler.messageBox_ExitToTitle = false;
			gameState = 129;
			break;
		}
		mainC.maingameMain.Calculate_Frame_Time();
		mainC.maingameMain.Update_Network_Message_Time(0.1f);
		mainC.playersMain.Send_Player_Info_For_Lobby();
		mainC.networkingMain.XBOX_Process_Networking(threadID);
		mainC.soundsMain.Process_Sounds();
	}

	public static void Main_Loop_Threaded_MP_RoundOver(byte threadID)
	{
		mainC.userInterface.Close_Windows_For_End_Of_Round();
		lobbyTimer -= frametime;
		if (lobbyTimer < 0.5f)
		{
			MP_Back_To_Lobby_From_Game_Over();
		}
		mainC.maingameMain.Update_Network_Message_Time(0.1f);
		mainC.playersMain.Send_Player_Info_For_Lobby();
		mainC.networkingMain.XBOX_Process_Networking(threadID);
		mainC.soundsMain.Process_Sounds();
	}

	public static void Main_Loop_Common_MP_End(byte threadID)
	{
		byte uBufferID = global::Rendering.Rendering.uBufferID;
		mainC.maingameMain.Rank_Online_Players_By_Score();
		mainC.maingameMain.Calculate_Frame_Time();
		mainC.maingameMain.Update_Network_Message_Time(1f / 30f);
		mainC.soundsMain.Update_Sounds(global::Players.Players.players[0].charP.position.v[0] + 20f, global::Players.Players.players[0].charP.position.v[1] + 10f, global::Players.Players.players[0].charP.position.v[2]);
		mainC.soundsMain.Process_Sounds();
		global::Players.Players.players[0].posX[uBufferID] = global::Players.Players.players[0].charP.position.v[0];
		global::Players.Players.players[0].posY[uBufferID] = global::Players.Players.players[0].charP.position.v[1];
		global::Players.Players.players[0].posZ[uBufferID] = global::Players.Players.players[0].charP.position.v[2];
		if (global::Players.Players.players[0].voiceCueID > -1)
		{
			mainC.soundsMain.Update_Voice_Position(global::Players.Players.players[0].voiceCueID, global::Players.Players.players[0].charP.position.v[0], global::Players.Players.players[0].charP.position.v[1], global::Players.Players.players[0].charP.position.v[2]);
		}
		mainC.vehicles.Update_Vehicle_Avatar_Matrix(0);
		for (byte b = 1; b < maxGamePlayers; b++)
		{
			if (global::Players.Players.players[b].active)
			{
				global::Players.Players.players[b].posX[uBufferID] = global::Players.Players.players[b].charP.position.v[0];
				global::Players.Players.players[b].posY[uBufferID] = global::Players.Players.players[b].charP.position.v[1];
				global::Players.Players.players[b].posZ[uBufferID] = global::Players.Players.players[b].charP.position.v[2];
				mainC.vehicles.Update_Vehicle_Avatar_Matrix(b);
				if (global::Players.Players.players[b].voiceCueID > -1)
				{
					mainC.soundsMain.Update_Voice_Position(global::Players.Players.players[b].voiceCueID, global::Players.Players.players[b].charP.position.v[0], global::Players.Players.players[b].charP.position.v[1], global::Players.Players.players[b].charP.position.v[2]);
				}
			}
		}
		mainC.gameLogic.Game_Update_Camera(threadID);
		curIdleTime += frametime;
		if (curIdleTime > idleTimeout)
		{
			mainC.maingameMain.Exit_To_Title_From_Menu();
		}
		else if (idleTimeout - curIdleTime < 10f)
		{
			mainC.userInterface.Load_Idle_Timeout_Window();
		}
		else
		{
			mainC.userInterface.Close_Window(15);
		}
		mainC.gameLogic.Game_MP_End_Of_Update(threadID);
	}

	public void Main_Loop_Race_Countdown_MP(byte threadID)
	{
		byte uBufferID = global::Rendering.Rendering.uBufferID;
		global::InputHandler.InputHandler.lookMode = 0;
		gameTime += frameTimeAdjusted;
		mainC.renderingMain.Process_Particles();
		if (global::Players.Players.respawnEnabled && global::Players.Players.players[0].dead && global::Players.Players.players[0].onmap == 1)
		{
			mainC.playersMain.Player_Respawn(threadID);
		}
		mainC.weaponsMain.Reset_Network_Player_Particle_Check();
		mainC.networkingMain.XBOX_Process_Networking(0);
		mainC.weaponsMain.Process_Ballistics(threadID);
		mainC.Explosions.Process_Explosions(threadID);
		if (commanderMode)
		{
			Commander_Process_Objectives();
		}
		mainC.weaponsMain.Process_Player_Weapons(0, global::Players.Players.players[0].primaryWeaponMountWeapon);
		global::Joints.Joints.Do_Joint_Basic_Calculations(frametime);
		mainC.gameobjectMain.Process_Objects(threadID);
		mainC.playersMain.Process_Players(frametime, threadID);
		mainC.gameLogic.Game_Update_Camera(threadID);
		if (!global::Players.Players.freezeCamera)
		{
			switch (global::Players.Players.currentView)
			{
			case 0:
			case 1:
				mainC.soundsMain.Set_Listener_Position(global::Rendering.Rendering.cameraPositionsX[global::Players.Players.currentView], global::Rendering.Rendering.cameraPositionsY[global::Players.Players.currentView], global::Rendering.Rendering.cameraPositionsZ[global::Players.Players.currentView], ref global::Players.Players.players[0].charP.velocity);
				break;
			case 2:
			case 3:
				mainC.soundsMain.Set_Listener_Position(global::Rendering.Rendering.camPos[uBufferID].X, global::Rendering.Rendering.camPos[uBufferID].Y, global::Rendering.Rendering.camPos[uBufferID].Z, ref global::Players.Players.players[0].charP.velocity);
				break;
			}
		}
		mainC.maingameMain.Calculate_Frame_Time();
		mainC.maingameMain.Update_Network_Message_Time(1f / 30f);
		mainC.soundsMain.Process_Sounds();
		global::Players.Players.players[0].posX[uBufferID] = global::Players.Players.players[0].charP.position.v[0];
		global::Players.Players.players[0].posY[uBufferID] = global::Players.Players.players[0].charP.position.v[1];
		global::Players.Players.players[0].posZ[uBufferID] = global::Players.Players.players[0].charP.position.v[2];
		for (byte b = 1; b < maxGamePlayers; b++)
		{
			if (global::Players.Players.players[b].active)
			{
				global::Players.Players.players[b].posX[uBufferID] = global::Players.Players.players[b].charP.position.v[0];
				global::Players.Players.players[b].posY[uBufferID] = global::Players.Players.players[b].charP.position.v[1];
				global::Players.Players.players[b].posZ[uBufferID] = global::Players.Players.players[b].charP.position.v[2];
			}
		}
		raceStartTimer -= frametime;
		if (raceStartTimer < (float)(int)currentRaceStartTimer && currentRaceStartTimer <= 0)
		{
			currentRaceStartTimer = byte.MaxValue;
			gameState = 141;
		}
	}

	public static void Main_Loop_Threaded_MP_Commander_Gameplay(byte threadID)
	{
		_ = global::Rendering.Rendering.uBufferID;
		gameTime += frameTimeAdjusted;
		mainC.playersMain.New_Frame_MP();
		mainC.renderingMain.Process_Particles();
		if (global::InputHandler.InputHandler.controllerButtonBackPressed)
		{
			global::InputHandler.InputHandler.controllerButtonBackPressed = false;
		}
		if (global::InputHandler.InputHandler.controllerButtonStartPressed)
		{
			mainC.maingameMain.Entering_Menu_State();
		}
		if (global::InputHandler.InputHandler.controllerDPadUpPressed && global::Players.Players.scopeValue < 4)
		{
			global::Players.Players.scopeValue++;
			global::Rendering.Rendering.scopeValue = (float)Math.PI / 4f / (float)Math.Pow(2.0, (int)global::Players.Players.scopeValue);
			global::Players.Players.scopeViewAdj = 1f / (float)Math.Pow(2.0, (int)global::Players.Players.scopeValue);
		}
		else if (global::InputHandler.InputHandler.controllerDPadDownPressed && global::Players.Players.scopeValue > 0)
		{
			global::Players.Players.scopeValue--;
			global::Rendering.Rendering.scopeValue = (float)Math.PI / 4f / (float)Math.Pow(2.0, (int)global::Players.Players.scopeValue);
			global::Players.Players.scopeViewAdj = 1f / (float)Math.Pow(2.0, (int)global::Players.Players.scopeValue);
		}
		if (global::InputHandler.InputHandler.controllerDPadRightPressed)
		{
			short num = global::Players.Players.Find_Next_Team_Player((short)(global::Players.Players.commanderViewingPlayer + 1), global::Players.Players.enemyTeamMask);
			if (num == -1)
			{
				num = global::Players.Players.Find_Next_Team_Player(1, global::Players.Players.enemyTeamMask);
			}
			if (num > -1)
			{
				global::Players.Players.commanderViewingPlayer = (byte)num;
			}
		}
		else if (global::InputHandler.InputHandler.controllerDPadLeftPressed)
		{
			short num = global::Players.Players.Find_Previous_Team_Player((short)(global::Players.Players.commanderViewingPlayer - 1), global::Players.Players.enemyTeamMask);
			if (num < 1)
			{
				num = global::Players.Players.Find_Previous_Team_Player((short)(maxGamePlayers - 1), global::Players.Players.enemyTeamMask);
			}
			if (num > 0)
			{
				global::Players.Players.commanderViewingPlayer = (byte)num;
			}
		}
		if (global::InputHandler.InputHandler.controllerButtonAPressed)
		{
			if (global::Players.Players.players[0].dead)
			{
				mainC.playersMain.Player_Respawn(threadID);
			}
			else if (commanderLevel > 0)
			{
				commanderLevel--;
				global::Players.Players.players[0].charP.position.v[2] = 50f + (float)(int)commanderLevel * 100f;
			}
		}
		if (global::InputHandler.InputHandler.controllerButtonYPressed)
		{
			if (global::Players.Players.players[0].dead)
			{
				mainC.playersMain.Player_Respawn(threadID);
			}
			else
			{
				commanderLevel++;
				global::Players.Players.players[0].charP.position.v[2] = 50f + (float)(int)commanderLevel * 100f;
				if (global::Players.Players.players[0].charP.position.v[2] >= MaxUp)
				{
					global::Players.Players.players[0].charP.position.v[2] -= 100f;
					commanderLevel--;
				}
			}
		}
		if (global::InputHandler.InputHandler.controllerButtonRightShoulderPressed)
		{
			commanderTeleportTimer = 0f;
			commanderTeleportingPlayer = false;
			commanderSelect = -1;
			commanderItem <<= 1;
			if (commanderItem > 32)
			{
				commanderItem = 1;
			}
		}
		else if (global::InputHandler.InputHandler.controllerButtonLeftShoulderPressed)
		{
			commanderTeleportTimer = 0f;
			commanderTeleportingPlayer = false;
			commanderSelect = -1;
			commanderItem >>= 1;
			if (commanderItem < 1)
			{
				commanderItem = 32;
			}
		}
		float num3;
		float num2;
		if (global::InputHandler.InputHandler.controllerStickButtonRightPressed)
		{
			switch (global::Players.Players.commanderView)
			{
			case 0:
				global::Players.Players.commanderView = 1;
				global::Players.Players.xRotation = 0f;
				if (global::Players.Players.commanderViewingPlayer > 0 && global::Players.Players.commanderViewingPlayer < maxGamePlayers)
				{
					short num = global::Players.Players.commanderViewingPlayer;
					if ((global::Players.Players.players[num].onmap & 0xC) > 1)
					{
						global::Players.Players.zRotation = global::Players.Players.players[num].zRotation;
						num2 = (float)Math.Cos(global::Players.Players.zRotation * ((float)Math.PI / 180f));
						num3 = (float)Math.Sin(global::Players.Players.zRotation * ((float)Math.PI / 180f));
						global::Players.Players.players[0].charP.position.v[0] = global::Players.Players.players[num].charP.position.v[0] + 40f * num3 + 20f * num2;
						global::Players.Players.players[0].charP.position.v[1] = global::Players.Players.players[num].charP.position.v[1] - 40f * num2 + 20f * num3;
						global::Players.Players.commanderX = global::Players.Players.players[num].charP.position.v[0];
						global::Players.Players.commanderY = global::Players.Players.players[num].charP.position.v[1];
					}
				}
				break;
			case 1:
				global::Players.Players.commanderView = 0;
				global::Players.Players.xRotation = -60f;
				global::Players.Players.commanderX = 0f;
				global::Players.Players.commanderY = 0f;
				global::Players.Players.commanderZ = 0f;
				commanderLevel = 0;
				global::Players.Players.players[0].charP.position.v[2] = 50f;
				break;
			}
		}
		num3 = global::InputHandler.InputHandler.controllerStickLeftValueY;
		if (num3 < 0.2f)
		{
			sprinting = false;
		}
		if (sprinting || global::InputHandler.InputHandler.controllerStickButtonLeftPressed)
		{
			global::Players.Players.moving |= 1;
			global::Players.Players.playerSpeed = 30f;
			global::Rendering.Rendering.viewMovement = 1.25f;
		}
		else
		{
			num2 = global::InputHandler.InputHandler.controllerStickLeftValueX;
			if (num2 != 0f)
			{
				if (global::InputHandler.InputHandler.slowSideStep && global::InputHandler.InputHandler.lookMode == 1)
				{
					num2 *= global::InputHandler.InputHandler.lookSensitivity[1];
				}
				global::Players.Players.moving |= 2;
				global::Players.Players.playerSpeedSideways = 20f * num2;
			}
			if (num3 != 0f)
			{
				global::Players.Players.moving |= 1;
				global::Players.Players.playerSpeed = 20f * num3;
			}
			global::Players.Players.playerSpeedRotateLeftStick = 0f;
			if (Math.Abs(num2) > 0.01f)
			{
				global::Players.Players.playerSpeedRotateLeftStick = -140f * num2 * global::InputHandler.InputHandler.lookSensitivity[global::InputHandler.InputHandler.lookMode] * global::Players.Players.scopeViewAdj;
			}
			num2 = Math.Abs(num2);
			num3 = Math.Abs(num3);
			if (num2 != 0f && num2 > num3)
			{
				sideStepping = true;
				global::Rendering.Rendering.viewMovement = global::Players.Players.playerSpeedSideways / 20f;
			}
			else if (num3 != 0f)
			{
				walking = true;
				walkingBackwards = false;
				if (global::Players.Players.playerSpeed < 0f)
				{
					walking = false;
					walkingBackwards = true;
				}
				global::Rendering.Rendering.viewMovement = global::Players.Players.playerSpeed / 20f;
			}
		}
		num2 = global::InputHandler.InputHandler.controllerStickRightValueX;
		if (Math.Abs(num2) > 0.15f)
		{
			global::InputHandler.InputHandler.controllerStickRightSmoothX = num2 - global::InputHandler.InputHandler.controllerStickRightValX;
			num2 = (global::InputHandler.InputHandler.controllerStickRightValX = ((!(Math.Abs(num2) - Math.Abs(global::InputHandler.InputHandler.controllerStickRightValX) > 0f)) ? (global::InputHandler.InputHandler.controllerStickRightValX + 0.5f * global::InputHandler.InputHandler.controllerStickRightSmoothX) : (global::InputHandler.InputHandler.controllerStickRightValX + 0.1f * global::InputHandler.InputHandler.controllerStickRightSmoothX)));
		}
		else
		{
			global::InputHandler.InputHandler.controllerStickRightValX = num2;
			num2 *= 0.4f;
		}
		global::Players.Players.playerSpeedRotateRightStick = 0f;
		if (Math.Abs(num2) > 0.01f)
		{
			global::Players.Players.moving |= 4;
			global::Players.Players.playerSpeedRotateRightStick = -140f * num2 * global::Players.Players.scopeViewAdj * global::InputHandler.InputHandler.lookSensitivity[global::InputHandler.InputHandler.lookMode];
		}
		num2 = global::InputHandler.InputHandler.controllerStickRightValueY * global::Players.Players.invertY;
		if (Math.Abs(num2) > 0.15f)
		{
			global::InputHandler.InputHandler.controllerStickRightSmoothY = num2 - global::InputHandler.InputHandler.controllerStickRightValY;
			num2 = (global::InputHandler.InputHandler.controllerStickRightValY = ((!(Math.Abs(num2) - Math.Abs(global::InputHandler.InputHandler.controllerStickRightValY) > 0f)) ? (global::InputHandler.InputHandler.controllerStickRightValY + 0.2f * global::InputHandler.InputHandler.controllerStickRightSmoothY) : (global::InputHandler.InputHandler.controllerStickRightValY + 0.05f * global::InputHandler.InputHandler.controllerStickRightSmoothY)));
		}
		else
		{
			global::InputHandler.InputHandler.controllerStickRightValY = num2;
			num2 *= 0.4f;
		}
		global::Players.Players.xRotation += 225f * num2 * global::InputHandler.InputHandler.lookSensitivity[global::InputHandler.InputHandler.lookMode] * global::Players.Players.scopeViewAdj * frametime;
		if (global::Players.Players.xRotation < -90f)
		{
			global::Players.Players.xRotation = -90f;
		}
		else if (global::Players.Players.xRotation > 90f)
		{
			global::Players.Players.xRotation = 90f;
		}
		if (!commanderTeleportingPlayer)
		{
			commanderTeleportEnergy += frametime;
		}
		if (commanderTeleportEnergy > 10f)
		{
			commanderTeleportEnergy = 10f;
		}
		global::Rendering.Rendering.commanderTeleporterEnergyVal = commanderTeleportEnergy / 10f;
		showCrossHairs[0] = 0;
		commanderNumTeammates = 0;
		ushort team = global::Players.Players.players[0].team;
		for (byte b = 1; b < maxGamePlayers; b++)
		{
			if (global::Players.Players.players[b].team == team)
			{
				commanderNumTeammates++;
			}
		}
		mainC.weaponsMain.Reset_Network_Player_Particle_Check();
		mainC.networkingMain.XBOX_Process_Networking(0);
		global::Rendering.Rendering.renderTransporterBar = false;
		commanderTeleportReadyToDeploy = true;
		switch (commanderItem)
		{
		case 4:
		{
			short num;
			if ((num = mainC.playersMain.Is_Commander_Targeting_Player(threadID, global::Players.Players.players[0].team)) > -1)
			{
				commanderSelect = num;
			}
			break;
		}
		case 8:
		{
			short num;
			if ((num = mainC.playersMain.Is_Commander_Targeting_Team(threadID, global::Players.Players.enemyTeamMask)) > -1)
			{
				commanderSelect = num;
			}
			break;
		}
		case 32:
			if (commanderTeleportingPlayer)
			{
				if (commanderTeleportPlayer > 0 && global::Players.Players.players[commanderTeleportPlayer].active)
				{
					global::Rendering.Rendering.renderTransporterBar = true;
					if (global::Players.Players.players[commanderTeleportPlayer].onmap == 1)
					{
						commanderTeleportTimer -= frametime;
					}
					else
					{
						commanderTeleportReadyToDeploy = false;
					}
					if (commanderTeleportTimer < 0f)
					{
						commanderTeleportTimer = 0f;
						global::InputHandler.InputHandler.controllerTriggerRightPressed = true;
					}
					global::Rendering.Rendering.commanderTeleporterVal = commanderTeleportTimer / 5f;
				}
				else
				{
					commanderTeleportTimer = 0f;
					commanderTeleportingPlayer = false;
					commanderTeleportPlayer = 0;
				}
				break;
			}
			commanderSelect = -1;
			if (!(commanderTeleportEnergy >= 10f))
			{
				break;
			}
			commanderSelect = mainC.playersMain.Is_Commander_Targeting_Team(threadID, global::Players.Players.enemyTeamMask);
			if (commanderSelect > 0 && global::Players.Players.players[commanderSelect].onmap == 4)
			{
				global::Rendering.Rendering.renderTransporterBar = true;
				commanderTeleportTimer += frametime;
				if (commanderTeleportTimer > 3f)
				{
					commanderTeleportTimer = 3f;
				}
				global::Rendering.Rendering.commanderTeleporterVal = commanderTeleportTimer / 3f;
			}
			else
			{
				commanderTeleportTimer = 0f;
			}
			break;
		}
		if (global::InputHandler.InputHandler.controllerTriggerRightPressed)
		{
			switch (commanderItem)
			{
			case 1:
				mainC.weaponsMain.Get_LaserSite_Position(0f, 0, commanderObjectivePosition, threadID);
				Commander_Add_Objective(0);
				break;
			case 2:
				mainC.weaponsMain.Get_LaserSite_Position(0f, 0, commanderObjectivePosition, threadID);
				Commander_Add_Objective(1);
				break;
			case 4:
				if (commanderSelect > -1)
				{
					Commander_Add_Objective(2);
				}
				break;
			case 8:
				if (commanderSelect > -1)
				{
					Commander_Add_Objective(3);
				}
				break;
			case 16:
				mainC.weaponsMain.Get_LaserSite_Position(0f, 0, commanderObjectivePosition, threadID);
				Commander_Add_Objective(4);
				break;
			case 32:
				if (commanderTeleportingPlayer)
				{
					if (global::Players.Players.players[commanderTeleportPlayer].onmap == 1)
					{
						commanderTeleportTimer = 0f;
						commanderTeleportingPlayer = false;
						mainC.weaponsMain.Get_LaserSite_Position(0f, 0, commanderObjectivePosition, threadID);
						ref HalfSingle reference = ref global::Networking.Networking.networkHS[0];
						reference = new HalfSingle(commanderObjectivePosition.x);
						ref HalfSingle reference2 = ref global::Networking.Networking.networkHS[1];
						reference2 = new HalfSingle(commanderObjectivePosition.y);
						ref HalfSingle reference3 = ref global::Networking.Networking.networkHS[2];
						reference3 = new HalfSingle(commanderObjectivePosition.z + 28.5f);
						mainC.networkingMain.XBOX_Send_Network_Message_To_Gamer(25, mainC.networkingMain.XBOX_Get_Gamer_By_Player_Index(commanderTeleportPlayer));
					}
				}
				else if (commanderSelect > 0 && commanderTeleportTimer >= 3f)
				{
					commanderTeleportEnergy = 0f;
					commanderTeleportingPlayer = true;
					commanderTeleportTimer = 5f;
					commanderTeleportPlayer = (byte)commanderSelect;
					global::Networking.Networking.networkBytes[0] = (byte)global::Players.Players.players[commanderSelect].id;
					mainC.networkingMain.XBOX_Send_Network_Message24(24);
				}
				break;
			default:
				mainC.weaponsMain.Get_LaserSite_Position(0f, 0, commanderObjectivePosition, threadID);
				Commander_Add_Objective(5);
				break;
			}
		}
		if (global::Rendering.Rendering.renderTransporterBar)
		{
			showCrossHairs[0] = 1;
		}
		global::Players.Players.players[0].shooting = false;
		mainC.weaponsMain.Process_Ballistics(threadID);
		mainC.Explosions.Process_Explosions(threadID);
		global::Joints.Joints.Do_Joint_Basic_Calculations(frametime);
		mainC.gameobjectMain.Process_Objects(threadID);
		mainC.playersMain.Process_Players(frametime, threadID);
		mainC.playersMain.Move_Commander(frametime, threadID);
		mainC.weaponsMain.Process_Player_Weapons(0, global::Players.Players.players[0].primaryWeaponMountWeapon);
		Commander_Process_Objectives();
		Main_Loop_Common_MP_End(threadID);
	}

	public static void Main_Loop_Threaded_MP_Commander_Gameplay_Paused_For_Menus(byte threadID)
	{
		_ = global::Rendering.Rendering.uBufferID;
		gameTime += frameTimeAdjusted;
		mainC.playersMain.New_Frame_MP();
		mainC.renderingMain.Process_Particles();
		if (global::InputHandler.InputHandler.controllerButtonStartPressed || global::InputHandler.InputHandler.controllerButtonBackPressed)
		{
			mainC.maingameMain.Leaving_Menu_State();
			mainC.inputMain.Leave_Menu_Completely();
		}
		else
		{
			mainC.inputMain.Handle_Menu(threadID);
			if (!global::InputHandler.InputHandler.inMenu)
			{
				mainC.maingameMain.Leaving_Menu_State();
			}
		}
		if (global::InputHandler.InputHandler.controllerButtonBackPressed)
		{
			mainC.Quit_Game();
		}
		if (global::InputHandler.InputHandler.controllerButtonStartPressed)
		{
			mainC.maingameMain.Entering_Menu_State();
		}
		if (global::InputHandler.InputHandler.controllerDPadRightPressed)
		{
			short num = global::Players.Players.Find_Next_Team_Player((short)(global::Players.Players.commanderViewingPlayer + 1), global::Players.Players.enemyTeamMask);
			if (num == -1)
			{
				num = global::Players.Players.Find_Next_Team_Player(1, global::Players.Players.enemyTeamMask);
			}
			if (num > -1)
			{
				global::Players.Players.commanderViewingPlayer = (byte)num;
			}
		}
		else if (global::InputHandler.InputHandler.controllerDPadLeftPressed)
		{
			short num = global::Players.Players.Find_Previous_Team_Player((short)(global::Players.Players.commanderViewingPlayer - 1), global::Players.Players.enemyTeamMask);
			if (num < 1)
			{
				num = global::Players.Players.Find_Previous_Team_Player((short)(maxGamePlayers - 1), global::Players.Players.enemyTeamMask);
			}
			if (num > 0)
			{
				global::Players.Players.commanderViewingPlayer = (byte)num;
			}
		}
		if (global::InputHandler.InputHandler.controllerButtonBackPressed)
		{
			global::InputHandler.InputHandler.controllerButtonBackPressed = false;
		}
		if (global::InputHandler.InputHandler.controllerButtonRightShoulderPressed)
		{
			commanderSelect = -1;
			commanderItem <<= 1;
			if (commanderItem > 32)
			{
				commanderItem = 1;
			}
		}
		else if (global::InputHandler.InputHandler.controllerButtonLeftShoulderPressed)
		{
			commanderSelect = -1;
			commanderItem >>= 1;
			if (commanderItem < 1)
			{
				commanderItem = 32;
			}
		}
		float controllerStickLeftValueY = global::InputHandler.InputHandler.controllerStickLeftValueY;
		if (controllerStickLeftValueY < 0.2f)
		{
			global::InputHandler.InputHandler.controllerStickButtonLeft = false;
		}
		float num2;
		if (global::InputHandler.InputHandler.controllerStickButtonLeft)
		{
			global::Players.Players.moving |= 1;
			global::Players.Players.playerSpeed = 30f;
			global::Rendering.Rendering.viewMovement = 1.25f;
		}
		else
		{
			num2 = global::InputHandler.InputHandler.controllerStickLeftValueX;
			if (num2 != 0f)
			{
				if (global::InputHandler.InputHandler.slowSideStep && global::InputHandler.InputHandler.lookMode == 1)
				{
					num2 *= global::InputHandler.InputHandler.lookSensitivity[1];
				}
				global::Players.Players.moving |= 2;
				global::Players.Players.playerSpeedSideways = 20f * num2;
			}
			if (controllerStickLeftValueY != 0f)
			{
				global::Players.Players.moving |= 1;
				global::Players.Players.playerSpeed = 20f * controllerStickLeftValueY;
			}
			global::Players.Players.playerSpeedRotateLeftStick = 0f;
			if (Math.Abs(num2) > 0.01f)
			{
				global::Players.Players.playerSpeedRotateLeftStick = -140f * num2 * global::InputHandler.InputHandler.lookSensitivity[global::InputHandler.InputHandler.lookMode] * global::Players.Players.scopeViewAdj;
			}
			num2 = Math.Abs(num2);
			controllerStickLeftValueY = Math.Abs(controllerStickLeftValueY);
			if (num2 != 0f && num2 > controllerStickLeftValueY)
			{
				sideStepping = true;
				global::Rendering.Rendering.viewMovement = global::Players.Players.playerSpeedSideways / 20f;
			}
			else if (controllerStickLeftValueY != 0f)
			{
				walking = true;
				walkingBackwards = false;
				if (global::Players.Players.playerSpeed < 0f)
				{
					walking = false;
					walkingBackwards = true;
				}
				global::Rendering.Rendering.viewMovement = global::Players.Players.playerSpeed / 20f;
			}
		}
		num2 = global::InputHandler.InputHandler.controllerStickRightValueX;
		if (Math.Abs(num2) > 0.15f)
		{
			global::InputHandler.InputHandler.controllerStickRightSmoothX = num2 - global::InputHandler.InputHandler.controllerStickRightValX;
			num2 = (global::InputHandler.InputHandler.controllerStickRightValX = ((!(Math.Abs(num2) - Math.Abs(global::InputHandler.InputHandler.controllerStickRightValX) > 0f)) ? (global::InputHandler.InputHandler.controllerStickRightValX + 0.5f * global::InputHandler.InputHandler.controllerStickRightSmoothX) : (global::InputHandler.InputHandler.controllerStickRightValX + 0.1f * global::InputHandler.InputHandler.controllerStickRightSmoothX)));
		}
		else
		{
			global::InputHandler.InputHandler.controllerStickRightValX = num2;
			num2 *= 0.35f;
		}
		if (!(Math.Abs(num2) > 0.01f))
		{
			num2 = (global::InputHandler.InputHandler.controllerStickRightValX = 0f);
		}
		else
		{
			global::Players.Players.moving |= 4;
			global::Players.Players.playerSpeedRotateRightStick = -140f * num2 * global::InputHandler.InputHandler.lookSensitivity[global::InputHandler.InputHandler.lookMode];
		}
		num2 = global::InputHandler.InputHandler.controllerStickRightValueY * global::Players.Players.invertY;
		if (Math.Abs(num2) > 0.1f)
		{
			global::InputHandler.InputHandler.controllerStickRightSmoothY = num2 - global::InputHandler.InputHandler.controllerStickRightValY;
			num2 = (global::InputHandler.InputHandler.controllerStickRightValY = ((!(Math.Abs(num2) - Math.Abs(global::InputHandler.InputHandler.controllerStickRightValY) > 0f)) ? (global::InputHandler.InputHandler.controllerStickRightValY + 0.5f * global::InputHandler.InputHandler.controllerStickRightSmoothY) : (global::InputHandler.InputHandler.controllerStickRightValY + 0.1f * global::InputHandler.InputHandler.controllerStickRightSmoothY)));
		}
		else
		{
			global::InputHandler.InputHandler.controllerStickRightValY = num2;
			num2 *= 0.35f;
		}
		global::Players.Players.xRotation += 225f * num2 * global::InputHandler.InputHandler.lookSensitivity[global::InputHandler.InputHandler.lookMode] * frametime;
		if (global::Players.Players.xRotation < -90f)
		{
			global::Players.Players.xRotation = -90f;
		}
		else if (global::Players.Players.xRotation > 90f)
		{
			global::Players.Players.xRotation = 90f;
		}
		if (global::InputHandler.InputHandler.controllerButtonAPressed && global::Players.Players.players[0].dead)
		{
			mainC.playersMain.Player_Respawn(threadID);
		}
		mainC.weaponsMain.Reset_Network_Player_Particle_Check();
		mainC.networkingMain.XBOX_Process_Networking(0);
		global::Players.Players.players[0].shooting = false;
		mainC.weaponsMain.Process_Ballistics(threadID);
		mainC.Explosions.Process_Explosions(threadID);
		mainC.playersMain.Move_Commander(frametime, threadID);
		mainC.weaponsMain.Process_Player_Weapons(0, global::Players.Players.players[0].primaryWeaponMountWeapon);
		global::Joints.Joints.Do_Joint_Basic_Calculations(frametime);
		mainC.gameobjectMain.Process_Objects(threadID);
		mainC.playersMain.Process_Players(frametime, threadID);
		Commander_Process_Objectives();
		if (global::AI.AI.numAI > 0)
		{
			mainC.aiMain.Process_AI_MP(frametime, threadID);
		}
		mainC.pickupsMain.Process_Pickups_MP(threadID);
		Main_Loop_Common_MP_End(threadID);
		global::Threads.Threads.thread3End.WaitOne();
	}

	public static void Commander_Receive_Update_Of_PlayerHealth(int actID)
	{
		short num = mainC.playersMain.Get_Player_Index(actID, -1);
		if (num >= 0)
		{
			mainC.playersMain.Adjust_Player_Damage_To_Fixed_Amount((ushort)num, global::Networking.Networking.networkHS[0].ToSingle(), sendOnline: false);
		}
	}

	public static void Commander_Add_Objective(byte type)
	{
		bool flag = false;
		byte b;
		for (b = 0; b < numCommanderObjectives; b++)
		{
			if (commanderObjectives[type, b].active)
			{
				commanderObjectives[type, b].order++;
				if (commanderObjectives[type, b].order >= numCommanderObjectives)
				{
					commanderObjectives[type, b].active = false;
					switch (type)
					{
					case 2:
					case 3:
						if (commanderObjectives[type, b].value > -1 && commanderObjectives[type, b].value < maxGamePlayers)
						{
							global::Players.Players.players[commanderObjectives[type, b].value].commanderTargeted = false;
						}
						break;
					}
				}
			}
		}
		for (b = 0; b < numCommanderObjectives; b++)
		{
			if (!commanderObjectives[type, b].active)
			{
				flag = true;
				break;
			}
		}
		if (!flag)
		{
			b = 0;
		}
		commanderObjectives[type, b].active = true;
		commanderObjectives[type, b].order = 0;
		commanderObjectives[type, b].x = commanderObjectivePosition.x;
		commanderObjectives[type, b].y = commanderObjectivePosition.y;
		commanderObjectives[type, b].z = commanderObjectivePosition.z;
		commanderObjectives[type, b].rotation = 0f;
		commanderObjectives[type, b].type = type;
		commanderObjectives[type, b].points = 0f;
		commanderObjectives[type, b].id = objectiveUniqueID;
		switch (type)
		{
		case 0:
		{
			commanderObjectives[type, b].timeToLive = 30f;
			commanderObjectives[type, b].points = (int)commanderNumTeammates;
			global::Networking.Networking.networkBytes[0] = 0;
			global::Networking.Networking.networkBytes[1] = objectiveUniqueID;
			ref HalfSingle reference4 = ref global::Networking.Networking.networkHS[0];
			reference4 = new HalfSingle(commanderObjectivePosition.x);
			ref HalfSingle reference5 = ref global::Networking.Networking.networkHS[1];
			reference5 = new HalfSingle(commanderObjectivePosition.y);
			ref HalfSingle reference6 = ref global::Networking.Networking.networkHS[2];
			reference6 = new HalfSingle(commanderObjectivePosition.z);
			mainC.networkingMain.XBOX_Send_Network_Message_ToTeam(14, global::Players.Players.players[0].team);
			break;
		}
		case 1:
		{
			commanderObjectives[type, b].timeToLive = 10f;
			global::Networking.Networking.networkBytes[0] = 1;
			global::Networking.Networking.networkBytes[1] = objectiveUniqueID;
			ref HalfSingle reference10 = ref global::Networking.Networking.networkHS[0];
			reference10 = new HalfSingle(commanderObjectivePosition.x);
			ref HalfSingle reference11 = ref global::Networking.Networking.networkHS[1];
			reference11 = new HalfSingle(commanderObjectivePosition.y);
			ref HalfSingle reference12 = ref global::Networking.Networking.networkHS[2];
			reference12 = new HalfSingle(commanderObjectivePosition.z);
			mainC.networkingMain.XBOX_Send_Network_Message_ToTeam(14, global::Players.Players.players[0].team);
			break;
		}
		case 2:
			global::Players.Players.players[commanderSelect].commanderTargeted = true;
			commanderObjectives[type, b].timeToLive = 30f;
			commanderObjectives[type, b].value = commanderSelect;
			global::Networking.Networking.networkBytes[0] = 2;
			global::Networking.Networking.networkBytes[1] = objectiveUniqueID;
			global::Networking.Networking.networkInts[0] = global::Players.Players.players[commanderSelect].id;
			mainC.networkingMain.XBOX_Send_Network_Message_ToTeam(14, global::Players.Players.players[0].team);
			break;
		case 3:
			global::Players.Players.players[commanderSelect].commanderTargeted = true;
			commanderObjectives[type, b].timeToLive = 30f;
			commanderObjectives[type, b].value = commanderSelect;
			global::Networking.Networking.networkBytes[0] = 3;
			global::Networking.Networking.networkBytes[1] = objectiveUniqueID;
			global::Networking.Networking.networkInts[0] = global::Players.Players.players[commanderSelect].id;
			mainC.networkingMain.XBOX_Send_Network_Message_ToTeam(14, global::Players.Players.players[0].team);
			break;
		case 4:
		{
			commanderObjectives[type, b].timeToLive = 60f;
			global::Networking.Networking.networkBytes[0] = 4;
			global::Networking.Networking.networkBytes[1] = objectiveUniqueID;
			ref HalfSingle reference7 = ref global::Networking.Networking.networkHS[0];
			reference7 = new HalfSingle(commanderObjectivePosition.x);
			ref HalfSingle reference8 = ref global::Networking.Networking.networkHS[1];
			reference8 = new HalfSingle(commanderObjectivePosition.y);
			ref HalfSingle reference9 = ref global::Networking.Networking.networkHS[2];
			reference9 = new HalfSingle(commanderObjectivePosition.z);
			mainC.networkingMain.XBOX_Send_Network_Message_ToTeam(14, global::Players.Players.players[0].team);
			break;
		}
		default:
		{
			commanderObjectives[type, b].active = false;
			short num;
			if ((num = (short)mainC.playersMain.Find_Vacant_Player(0)) > -1)
			{
				global::Networking.Networking.networkMsg = "Test " + b;
				mainC.playersMain.Reset_Player((ushort)num, isActive: true, 0, 0);
				commanderObjectives[type, b].active = true;
				commanderObjectives[type, b].value = num;
				commanderObjectives[type, b].timeToLive = 120f;
				global::Networking.Networking.networkBytes[0] = 5;
				ref HalfSingle reference = ref global::Networking.Networking.networkHS[0];
				reference = new HalfSingle(commanderObjectivePosition.x);
				ref HalfSingle reference2 = ref global::Networking.Networking.networkHS[1];
				reference2 = new HalfSingle(commanderObjectivePosition.y);
				ref HalfSingle reference3 = ref global::Networking.Networking.networkHS[2];
				reference3 = new HalfSingle(commanderObjectivePosition.z);
				global::Players.Players.mpData[num].currentPosX = (global::Players.Players.players[num].charP.position.v[0] = commanderObjectivePosition.x);
				global::Players.Players.mpData[num].currentPosY = (global::Players.Players.players[num].charP.position.v[1] = commanderObjectivePosition.y);
				global::Players.Players.players[num].charP.position.v[2] = commanderObjectivePosition.z + 35f;
				global::Players.Players.players[num].onmap = 4;
				global::Players.Players.players[num].dead = false;
				global::Players.Players.players[num].playerIsMoving = 0;
				mainC.programsMain.Reset_Programs(ref global::Players.Players.players[num].pg1, ref global::Players.Players.players[num].animations, global::Players.Players.players[num].programCollection);
				global::Players.Players.players[num].animations[global::Players.Players.players[num].programStationaryArms].status = 2;
				global::Players.Players.players[num].animations[global::Players.Players.players[num].programStationaryLegsBody].status = 2;
				mainC.programsMain.Start_Animation((ushort)num, ref global::Players.Players.players[num].jt1, ref global::Players.Players.players[num].animations, global::Players.Players.players[num].programCollection, global::Players.Players.players[num].programStationaryArms, 1f, 1f);
				mainC.programsMain.Start_Animation((ushort)num, ref global::Players.Players.players[num].jt1, ref global::Players.Players.players[num].animations, global::Players.Players.players[num].programCollection, global::Players.Players.players[num].programStationaryLegsBody, 1f, 1f);
				global::Players.Players.players[num].jt1[6].rotX = (global::Players.Players.players[num].jt1[6].targetAngle = 90f);
				global::Players.Players.players[num].team = global::Players.Players.players[0].team;
				global::Players.Players.players[num].teamMask = mainC.playersMain.Get_Team_Mask(global::Players.Players.players[0].team);
				global::Players.Players.players[num].zRotation = global::Players.Players.zRotation;
				mainC.networkingMain.XBOX_Send_Network_Message_ToTeam(14, global::Players.Players.players[0].team);
			}
			break;
		}
		}
		objectiveUniqueID++;
	}

	public static void Commander_Add_Objective_From_Network()
	{
		bool flag = false;
		byte b = global::Networking.Networking.networkBytes[0];
		byte b2;
		for (b2 = 0; b2 < numCommanderObjectives; b2++)
		{
			if (commanderObjectives[b, b2].active)
			{
				commanderObjectives[b, b2].order++;
				if (commanderObjectives[b, b2].order >= numCommanderObjectives)
				{
					commanderObjectives[b, b2].active = false;
					switch (b)
					{
					case 2:
					case 3:
						if (commanderObjectives[b, b2].value > -1 && commanderObjectives[b, b2].value < maxGamePlayers)
						{
							global::Players.Players.players[commanderObjectives[b, b2].value].commanderTargeted = false;
						}
						break;
					}
				}
			}
		}
		for (b2 = 0; b2 < numCommanderObjectives; b2++)
		{
			if (!commanderObjectives[b, b2].active)
			{
				flag = true;
				break;
			}
		}
		if (!flag)
		{
			b2 = 0;
		}
		commanderObjectives[b, b2].active = true;
		commanderObjectives[b, b2].type = b;
		commanderObjectives[b, b2].order = 0;
		commanderObjectives[b, b2].points = 0f;
		commanderObjectives[b, b2].id = global::Networking.Networking.networkBytes[1];
		short num;
		switch (b)
		{
		case 0:
			commanderObjectives[b, b2].timeToLive = 30f;
			commanderObjectives[0, b2].rotation = 0f;
			commanderObjectives[0, b2].x = global::Networking.Networking.networkHS[0].ToSingle();
			commanderObjectives[0, b2].y = global::Networking.Networking.networkHS[1].ToSingle();
			commanderObjectives[0, b2].z = global::Networking.Networking.networkHS[2].ToSingle();
			return;
		case 1:
			commanderObjectives[1, b2].timeToLive = 10f;
			commanderObjectives[1, b2].rotation = 0f;
			commanderObjectives[1, b2].x = global::Networking.Networking.networkHS[0].ToSingle();
			commanderObjectives[1, b2].y = global::Networking.Networking.networkHS[1].ToSingle();
			commanderObjectives[1, b2].z = global::Networking.Networking.networkHS[2].ToSingle();
			return;
		case 2:
			num = mainC.playersMain.Get_Player_Index(global::Networking.Networking.networkInts[0], -1);
			commanderObjectives[2, b2].active = false;
			if (num > -1)
			{
				commanderObjectives[2, b2].active = true;
				commanderObjectives[2, b2].value = num;
				commanderObjectives[2, b2].timeToLive = 30f;
				global::Players.Players.players[num].commanderTargeted = true;
				global::Players.Players.players[1].charP.position.v[2] += 30f;
			}
			return;
		case 3:
			num = mainC.playersMain.Get_Player_Index(global::Networking.Networking.networkInts[0], -1);
			commanderObjectives[3, b2].active = false;
			if (num > 0)
			{
				commanderObjectives[4, b2].rotation = 0f;
				commanderObjectives[3, b2].active = true;
				commanderObjectives[3, b2].value = num;
				commanderObjectives[3, b2].timeToLive = 30f;
				global::Players.Players.players[num].commanderTargeted = true;
			}
			return;
		case 4:
			commanderObjectives[4, b2].timeToLive = 60f;
			commanderObjectives[4, b2].rotation = 0f;
			commanderObjectives[4, b2].x = global::Networking.Networking.networkHS[0].ToSingle();
			commanderObjectives[4, b2].y = global::Networking.Networking.networkHS[1].ToSingle();
			commanderObjectives[4, b2].z = global::Networking.Networking.networkHS[2].ToSingle();
			return;
		}
		commanderObjectives[b, b2].active = false;
		if ((num = (short)mainC.playersMain.Find_Vacant_Player(0)) > -1)
		{
			try
			{
				global::Networking.Networking.networkMsg = "Test " + b2;
				mainC.playersMain.Reset_Player((ushort)num, isActive: true, 0, 0);
				commanderObjectives[b, b2].active = true;
				commanderObjectives[b, b2].value = num;
				commanderObjectives[b, b2].timeToLive = 120f;
				commanderObjectives[b, b2].x = global::Networking.Networking.networkHS[0].ToSingle();
				commanderObjectives[b, b2].y = global::Networking.Networking.networkHS[1].ToSingle();
				commanderObjectives[b, b2].z = global::Networking.Networking.networkHS[2].ToSingle();
				global::Players.Players.mpData[num].currentPosX = (global::Players.Players.players[num].charP.position.v[0] = commanderObjectives[b, b2].x);
				global::Players.Players.mpData[num].currentPosY = (global::Players.Players.players[num].charP.position.v[1] = commanderObjectives[b, b2].y);
				global::Players.Players.players[num].charP.position.v[2] = commanderObjectives[b, b2].z + 35f;
				global::Players.Players.players[num].onmap = 4;
				global::Players.Players.players[num].dead = false;
				global::Players.Players.players[num].playerIsMoving = 0;
				mainC.programsMain.Reset_Programs(ref global::Players.Players.players[num].pg1, ref global::Players.Players.players[num].animations, global::Players.Players.players[num].programCollection);
				global::Players.Players.players[num].animations[global::Players.Players.players[num].programStationaryArms].status = 2;
				global::Players.Players.players[num].animations[global::Players.Players.players[num].programStationaryLegsBody].status = 2;
				mainC.programsMain.Start_Animation((ushort)num, ref global::Players.Players.players[num].jt1, ref global::Players.Players.players[num].animations, global::Players.Players.players[num].programCollection, global::Players.Players.players[num].programStationaryArms, 1f, 1f);
				mainC.programsMain.Start_Animation((ushort)num, ref global::Players.Players.players[num].jt1, ref global::Players.Players.players[num].animations, global::Players.Players.players[num].programCollection, global::Players.Players.players[num].programStationaryLegsBody, 1f, 1f);
				global::Players.Players.players[num].jt1[6].rotX = (global::Players.Players.players[num].jt1[6].targetAngle = 90f);
				global::Players.Players.players[num].team = global::Players.Players.players[0].team;
				global::Players.Players.players[num].teamMask = mainC.playersMain.Get_Team_Mask(global::Players.Players.players[0].team);
				global::Players.Players.players[num].zRotation = global::Players.Players.players[Commander[global::Players.Players.players[0].team]].zRotation;
			}
			catch (Exception)
			{
				global::Players.Players.players[0].charP.position.v[2] += 30f;
			}
		}
	}

	public static void Commander_Remove_Objective_From_Network()
	{
		byte b = global::Networking.Networking.networkBytes[0];
		byte b2 = global::Networking.Networking.networkBytes[1];
		for (byte b3 = 0; b3 < numCommanderObjectives; b3++)
		{
			if (commanderObjectives[b2, b3].active && commanderObjectives[b2, b3].id == b)
			{
				if (commanderObjectives[b2, b3].value > -1 && commanderObjectives[b2, b3].value < maxGamePlayers)
				{
					global::Players.Players.players[commanderObjectives[b2, b3].value].commanderTargeted = false;
				}
				commanderObjectives[b2, b3].active = false;
				break;
			}
		}
	}

	public static void Commander_Process_Objectives()
	{
		bool flag = false;
		sbyte b = 0;
		byte b2 = 0;
		for (byte b3 = 0; b3 < numCommanderObjectives; b3++)
		{
			if (commanderObjectives[b2, b3].active)
			{
				commanderObjectives[b2, b3].timeToLive -= frametime;
				if (commanderObjectives[b2, b3].timeToLive < 0f)
				{
					commanderObjectives[b2, b3].active = false;
				}
				else
				{
					commanderObjectives[b2, b3].rotation += 120f * frametime;
					if (commanderObjectives[b2, b3].rotation > 360f)
					{
						commanderObjectives[b2, b3].rotation -= 360f;
					}
					if (!isCommander)
					{
						float num = global::Players.Players.players[0].charP.position.v[0] - commanderObjectives[b2, b3].x;
						float num2 = global::Players.Players.players[0].charP.position.v[1] - commanderObjectives[b2, b3].y;
						float num3 = global::Players.Players.players[0].charP.position.v[2] - commanderObjectives[b2, b3].z;
						num = num * num + num2 * num2 + num3 * num3;
						if (num < 10000f && Commander[global::Players.Players.players[0].team] > 0)
						{
							global::Networking.Networking.networkBytes[0] = commanderObjectives[b2, b3].id;
							mainC.networkingMain.XBOX_Send_Network_Message_To_Gamer(21, mainC.networkingMain.XBOX_Get_Gamer_By_Act_ID(global::Players.Players.players[Commander[global::Players.Players.players[0].team]].id));
							commanderObjectives[b2, b3].active = false;
						}
					}
				}
			}
		}
		b2 = 1;
		for (byte b3 = 0; b3 < numCommanderObjectives; b3++)
		{
			if (commanderObjectives[b2, b3].active)
			{
				commanderObjectives[b2, b3].timeToLive -= frametime;
				if (commanderObjectives[b2, b3].timeToLive < 0f)
				{
					commanderObjectives[b2, b3].active = false;
				}
				else
				{
					commanderObjectives[b2, b3].rotation += 120f * frametime;
					if (commanderObjectives[b2, b3].rotation > 360f)
					{
						commanderObjectives[b2, b3].rotation -= 360f;
					}
				}
			}
		}
		b2 = 2;
		for (byte b3 = 0; b3 < numCommanderObjectives; b3++)
		{
			if (commanderObjectives[b2, b3].active)
			{
				commanderObjectives[b2, b3].timeToLive -= frametime;
				if (commanderObjectives[b2, b3].timeToLive < 0f)
				{
					short value = commanderObjectives[b2, b3].value;
					if (value > -1 && value < maxGamePlayers)
					{
						global::Players.Players.players[value].commanderTargeted = false;
					}
					commanderObjectives[b2, b3].active = false;
				}
				else
				{
					commanderObjectives[b2, b3].rotation += 120f * frametime;
					if (commanderObjectives[b2, b3].rotation > 360f)
					{
						commanderObjectives[b2, b3].rotation -= 360f;
					}
				}
			}
		}
		b2 = 3;
		for (byte b3 = 0; b3 < numCommanderObjectives; b3++)
		{
			if (commanderObjectives[b2, b3].active)
			{
				commanderObjectives[b2, b3].timeToLive -= frametime;
				if (commanderObjectives[b2, b3].timeToLive < 0f)
				{
					short value = commanderObjectives[b2, b3].value;
					if (value > -1 && value < maxGamePlayers)
					{
						global::Players.Players.players[value].commanderTargeted = false;
					}
					commanderObjectives[b2, b3].active = false;
				}
				else
				{
					commanderObjectives[b2, b3].rotation += 120f * frametime;
					if (commanderObjectives[b2, b3].rotation > 360f)
					{
						commanderObjectives[b2, b3].rotation -= 360f;
					}
					if (!isCommander)
					{
						short value = commanderObjectives[b2, b3].value;
						if (value > -1 && value < maxGamePlayers)
						{
							float num = global::Players.Players.players[0].charP.position.v[0] - global::Players.Players.players[value].charP.position.v[0];
							float num2 = global::Players.Players.players[0].charP.position.v[1] - global::Players.Players.players[value].charP.position.v[1];
							float num3 = global::Players.Players.players[0].charP.position.v[2] - global::Players.Players.players[value].charP.position.v[2];
							num = num * num + num2 * num2 + num3 * num3;
							if (num < 1700f && healingAbility > 60.0)
							{
								global::Rendering.Rendering.popUps[0] = 1;
								if (global::InputHandler.InputHandler.controllerButtonXPressed)
								{
									global::Rendering.Rendering.popUps[0] = 0;
									commanderObjectives[b2, b3].active = false;
									global::Players.Players.needToReload = false;
									healingAbility = 0f - frametime;
									if (Commander[global::Players.Players.players[0].team] > 0)
									{
										global::Networking.Networking.networkBytes[0] = commanderObjectives[b2, b3].id;
										mainC.networkingMain.XBOX_Send_Network_Message_To_Gamer(17, mainC.networkingMain.XBOX_Get_Gamer_By_Act_ID(global::Players.Players.players[Commander[global::Players.Players.players[0].team]].id));
									}
								}
							}
						}
					}
				}
			}
		}
		b2 = 4;
		for (byte b3 = 0; b3 < numCommanderObjectives; b3++)
		{
			if (commanderObjectives[b2, b3].active)
			{
				commanderObjectives[b2, b3].timeToLive -= frametime;
				if (commanderObjectives[b2, b3].timeToLive < 0f)
				{
					commanderObjectives[b2, b3].active = false;
				}
				else
				{
					commanderObjectives[b2, b3].rotation += 120f * frametime;
					if (commanderObjectives[b2, b3].rotation > 360f)
					{
						commanderObjectives[b2, b3].rotation -= 360f;
					}
					if (!isCommander)
					{
						float num = global::Players.Players.players[0].charP.position.v[0] - commanderObjectives[b2, b3].x;
						float num2 = global::Players.Players.players[0].charP.position.v[1] - commanderObjectives[b2, b3].y;
						float num3 = global::Players.Players.players[0].charP.position.v[2] - commanderObjectives[b2, b3].z;
						num = num * num + num2 * num2 + num3 * num3;
						if (num < 22500f)
						{
							commanderObjectives[b2, b3].points += frametime;
							if (commanderObjectives[b2, b3].points > 1f)
							{
								commanderObjectives[b2, b3].points -= 1f;
								b++;
								flag = true;
							}
						}
					}
				}
			}
		}
		if (flag)
		{
			mainC.playersMain.XBOX_Send_Update_Of_Player_ObjectivePoints_To_Host(global::Players.Players.players[0].id, 0, b);
		}
		healingAbility += frametime;
	}

	public static void Commander_Objective_Heal_Completed(int act)
	{
		byte b = 3;
		byte b2 = global::Networking.Networking.networkBytes[0];
		for (byte b3 = 0; b3 < numCommanderObjectives; b3++)
		{
			if (commanderObjectives[b, b3].active && commanderObjectives[b, b3].id == b2)
			{
				commanderObjectives[b, b3].active = false;
				if (commanderObjectives[b, b3].value > -1 && commanderObjectives[b, b3].value < maxGamePlayers)
				{
					global::Players.Players.players[commanderObjectives[b, b3].value].commanderTargeted = false;
				}
				global::Networking.Networking.networkBytes[1] = 3;
				mainC.networkingMain.XBOX_Send_Network_Message_ToTeam(20, global::Players.Players.players[0].team);
				if (!global::Networking.Networking.isHost)
				{
					global::Networking.Networking.networkSBytes[0] = 5;
					global::Networking.Networking.networkInts[0] = act;
					mainC.networkingMain.XBOX_Send_Network_Message_To_Host(18);
				}
				else
				{
					short num = mainC.playersMain.Get_Player_Index(act, -1);
					if (num > -1 && num < maxGamePlayers)
					{
						global::Players.Players.players[num].objectivePoints += 5;
						global::Networking.Networking.networkInts[0] = act;
						global::Networking.Networking.networkInts[1] = global::Players.Players.players[num].roundPts;
						global::Networking.Networking.networkInts[2] = global::Players.Players.players[num].objectivePoints;
						mainC.networkingMain.XBOX_Send_Network_Message50(50);
					}
				}
				global::Networking.Networking.networkBytes[0] = 75;
				mainC.networkingMain.XBOX_Send_Network_Message_To_Gamer(19, mainC.networkingMain.XBOX_Get_Gamer_By_Act_ID(commanderObjectives[b, b3].value));
				break;
			}
		}
	}

	public static void Commander_Objective_Waypoint_Completed(int act)
	{
		byte b = 0;
		byte b2 = global::Networking.Networking.networkBytes[0];
		for (byte b3 = 0; b3 < numCommanderObjectives; b3++)
		{
			if (commanderObjectives[b, b3].active && commanderObjectives[b, b3].id == b2)
			{
				if (!global::Networking.Networking.isHost)
				{
					global::Networking.Networking.networkSBytes[0] = (sbyte)commanderObjectives[b, b3].points;
					global::Networking.Networking.networkInts[0] = act;
					mainC.networkingMain.XBOX_Send_Network_Message_To_Host(18);
				}
				else
				{
					short num = mainC.playersMain.Get_Player_Index(act, -1);
					if (num > -1 && num < maxGamePlayers)
					{
						global::Players.Players.players[num].objectivePoints += (sbyte)commanderObjectives[b, b3].points;
						global::Networking.Networking.networkInts[0] = act;
						global::Networking.Networking.networkInts[1] = global::Players.Players.players[num].roundPts;
						global::Networking.Networking.networkInts[2] = global::Players.Players.players[num].objectivePoints;
						mainC.networkingMain.XBOX_Send_Network_Message50(50);
					}
				}
				commanderObjectives[b, b3].points -= 1f;
				if (commanderObjectives[b, b3].points < 1f)
				{
					commanderObjectives[b, b3].active = false;
				}
				break;
			}
		}
	}

	public static void Commander_Points_Received_From_Network()
	{
		short num = mainC.playersMain.Get_Player_Index(global::Networking.Networking.networkInts[0], -1);
		if (num > -1 && num < maxGamePlayers)
		{
			global::Players.Players.players[num].objectivePoints += global::Networking.Networking.networkSBytes[0];
			global::Networking.Networking.networkInts[1] = global::Players.Players.players[num].roundPts;
			global::Networking.Networking.networkInts[2] = global::Players.Players.players[num].objectivePoints;
			mainC.networkingMain.XBOX_Send_Network_Message50(50);
		}
	}

	public static void Commander_Render_Objectives(bool transparent)
	{
		global::Rendering.Rendering.effect1.CurrentTechnique = global::Rendering.Rendering.effect1.Techniques["Main"];
		global::Rendering.Rendering.effect1.CurrentTechnique.Passes[0].Apply();
		global::Rendering.Rendering.effect1.Parameters["ViewProjection"].SetValue(global::Rendering.Rendering.matrixVP);
		if (transparent)
		{
			global::Rendering.Rendering.rGraphics.DepthStencilState.DepthBufferEnable = false;
		}
		byte b = 0;
		if (transparent)
		{
			global::Rendering.Rendering.far4[0] = 1f;
			global::Rendering.Rendering.far4[1] = 0.5f;
			global::Rendering.Rendering.far4[2] = 0f;
			global::Rendering.Rendering.far4[3] = 0.25f;
		}
		else
		{
			global::Rendering.Rendering.far4[0] = 0.282f;
			global::Rendering.Rendering.far4[1] = 0.631f;
			global::Rendering.Rendering.far4[2] = 0.282f;
			global::Rendering.Rendering.far4[3] = 1f;
		}
		global::Rendering.Rendering.effect1.Parameters["BaseTexture"].SetValue(global::Textures.Textures.texMain.texData[global::Models.Models.mod1[global::Models.Models.modMarker].texID]);
		global::Rendering.Rendering.effect1.Parameters["MaterialTexture"].SetValue(global::Textures.Textures.texMain.texData[global::Models.Models.mod1[global::Models.Models.modMarker].texNormalID]);
		global::Rendering.Rendering.effect1.Parameters["ColorAdjust"].SetValue(global::Rendering.Rendering.far4);
		float value;
		for (byte b2 = 0; b2 < numCommanderObjectives; b2++)
		{
			if (commanderObjectives[b, b2].active)
			{
				Matrix mv = Matrix.CreateRotationZ(commanderObjectives[b, b2].rotation * ((float)Math.PI / 180f));
				mv.M41 = commanderObjectives[b, b2].x;
				mv.M42 = commanderObjectives[b, b2].y;
				mv.M43 = commanderObjectives[b, b2].z;
				value = commanderObjectives[b, b2].timeToLive / 30f;
				global::Rendering.Rendering.effect1.Parameters["AlphaAdjust"].SetValue(value);
				mainC.modelsMain.Render_Model_Basic_With_Matrix(global::Models.Models.modMarker, ref mv);
			}
		}
		b = 1;
		global::Rendering.Rendering.rGraphics.BlendState = global::Rendering.Rendering.blendSourceAlpha;
		if (transparent)
		{
			global::Rendering.Rendering.far4[0] = 1f;
			global::Rendering.Rendering.far4[1] = 0.5f;
			global::Rendering.Rendering.far4[2] = 0f;
			global::Rendering.Rendering.far4[3] = 0.25f;
		}
		else
		{
			global::Rendering.Rendering.far4[0] = 0.71f;
			global::Rendering.Rendering.far4[1] = 0.008f;
			global::Rendering.Rendering.far4[2] = 0.008f;
			global::Rendering.Rendering.far4[3] = 1f;
		}
		global::Rendering.Rendering.effect1.Parameters["BaseTexture"].SetValue(global::Textures.Textures.texMain.texData[global::Models.Models.mod1[global::Models.Models.modMarker2].texID]);
		global::Rendering.Rendering.effect1.Parameters["MaterialTexture"].SetValue(global::Textures.Textures.texMain.texData[global::Models.Models.mod1[global::Models.Models.modMarker2].texNormalID]);
		global::Rendering.Rendering.effect1.Parameters["ColorAdjust"].SetValue(global::Rendering.Rendering.far4);
		for (byte b2 = 0; b2 < numCommanderObjectives; b2++)
		{
			if (commanderObjectives[b, b2].active)
			{
				Matrix mv = Matrix.CreateRotationZ(commanderObjectives[b, b2].rotation * ((float)Math.PI / 180f));
				mv.M41 = commanderObjectives[b, b2].x;
				mv.M42 = commanderObjectives[b, b2].y;
				mv.M43 = commanderObjectives[b, b2].z;
				value = commanderObjectives[b, b2].timeToLive / 10f;
				global::Rendering.Rendering.effect1.Parameters["AlphaAdjust"].SetValue(value);
				mainC.modelsMain.Render_Model_Basic_With_Matrix(global::Models.Models.modMarker2, ref mv);
			}
		}
		b = 2;
		if (transparent)
		{
			global::Rendering.Rendering.far4[0] = 1f;
			global::Rendering.Rendering.far4[1] = 0.5f;
			global::Rendering.Rendering.far4[2] = 0f;
			global::Rendering.Rendering.far4[3] = 0.25f;
		}
		else
		{
			global::Rendering.Rendering.far4[0] = 1f;
			global::Rendering.Rendering.far4[1] = 1f;
			global::Rendering.Rendering.far4[2] = 1f;
			global::Rendering.Rendering.far4[3] = 1f;
		}
		global::Rendering.Rendering.effect1.Parameters["BaseTexture"].SetValue(global::Textures.Textures.texMain.texData[global::Models.Models.mod1[global::Models.Models.modMarker3].texID]);
		global::Rendering.Rendering.effect1.Parameters["MaterialTexture"].SetValue(global::Textures.Textures.texMain.texData[global::Models.Models.mod1[global::Models.Models.modMarker3].texNormalID]);
		global::Rendering.Rendering.effect1.Parameters["ColorAdjust"].SetValue(global::Rendering.Rendering.far4);
		for (byte b2 = 0; b2 < numCommanderObjectives; b2++)
		{
			if (commanderObjectives[b, b2].active)
			{
				short value2 = commanderObjectives[b, b2].value;
				if (value2 > -1 && value2 < maxGamePlayers)
				{
					Matrix mv = Matrix.CreateRotationZ(commanderObjectives[b, b2].rotation * ((float)Math.PI / 180f));
					mv.M41 = global::Players.Players.players[value2].charP.position.v[0];
					mv.M42 = global::Players.Players.players[value2].charP.position.v[1];
					mv.M43 = global::Players.Players.players[value2].charP.position.v[2] + 21f;
					mv = Matrix.CreateScale(0.483f, 0.483f, 0.483f) * mv;
					value = commanderObjectives[b, b2].timeToLive / 10f;
					global::Rendering.Rendering.effect1.Parameters["AlphaAdjust"].SetValue(value);
					mainC.modelsMain.Render_Model_Basic_With_Matrix(global::Models.Models.modMarker3, ref mv);
				}
			}
		}
		b = 3;
		if (transparent)
		{
			global::Rendering.Rendering.far4[0] = 1f;
			global::Rendering.Rendering.far4[1] = 0.5f;
			global::Rendering.Rendering.far4[2] = 0f;
			global::Rendering.Rendering.far4[3] = 0.25f;
		}
		else
		{
			global::Rendering.Rendering.far4[0] = 0.71f;
			global::Rendering.Rendering.far4[1] = 0.008f;
			global::Rendering.Rendering.far4[2] = 0.008f;
			global::Rendering.Rendering.far4[3] = 1f;
		}
		global::Rendering.Rendering.effect1.Parameters["BaseTexture"].SetValue(global::Textures.Textures.texMain.texData[global::Models.Models.mod1[global::Models.Models.modMarker4].texID]);
		global::Rendering.Rendering.effect1.Parameters["MaterialTexture"].SetValue(global::Textures.Textures.texMain.texData[global::Models.Models.mod1[global::Models.Models.modMarker4].texNormalID]);
		global::Rendering.Rendering.effect1.Parameters["ColorAdjust"].SetValue(global::Rendering.Rendering.far4);
		for (byte b2 = 0; b2 < numCommanderObjectives; b2++)
		{
			if (commanderObjectives[b, b2].active)
			{
				short value2 = commanderObjectives[b, b2].value;
				if (value2 > -1 && value2 < maxGamePlayers)
				{
					Matrix mv = Matrix.CreateRotationZ(commanderObjectives[b, b2].rotation * ((float)Math.PI / 180f));
					mv.M41 = global::Players.Players.players[value2].charP.position.v[0];
					mv.M42 = global::Players.Players.players[value2].charP.position.v[1];
					mv.M43 = global::Players.Players.players[value2].charP.position.v[2] + 35f;
					value = commanderObjectives[b, b2].timeToLive / 10f;
					global::Rendering.Rendering.effect1.Parameters["AlphaAdjust"].SetValue(value);
					mainC.modelsMain.Render_Model_Basic_With_Matrix(global::Models.Models.modMarker4, ref mv);
				}
			}
		}
		b = 4;
		if (transparent)
		{
			global::Rendering.Rendering.far4[0] = 0.4f;
			global::Rendering.Rendering.far4[1] = 0.8f;
			global::Rendering.Rendering.far4[2] = 1f;
			global::Rendering.Rendering.far4[3] = 0.25f;
		}
		else
		{
			global::Rendering.Rendering.far4[0] = 0.149f;
			global::Rendering.Rendering.far4[1] = 0.384f;
			global::Rendering.Rendering.far4[2] = 0.808f;
			global::Rendering.Rendering.far4[3] = 1f;
		}
		global::Rendering.Rendering.effect1.Parameters["BaseTexture"].SetValue(global::Textures.Textures.texMain.texData[global::Models.Models.mod1[global::Models.Models.modMarker5].texID]);
		global::Rendering.Rendering.effect1.Parameters["MaterialTexture"].SetValue(global::Textures.Textures.texMain.texData[global::Models.Models.mod1[global::Models.Models.modMarker5].texNormalID]);
		global::Rendering.Rendering.effect1.Parameters["ColorAdjust"].SetValue(global::Rendering.Rendering.far4);
		for (byte b2 = 0; b2 < numCommanderObjectives; b2++)
		{
			if (commanderObjectives[b, b2].active)
			{
				Matrix mv = Matrix.CreateRotationZ(commanderObjectives[b, b2].rotation * ((float)Math.PI / 180f));
				mv.M41 = commanderObjectives[b, b2].x;
				mv.M42 = commanderObjectives[b, b2].y;
				mv.M43 = commanderObjectives[b, b2].z;
				mainC.modelsMain.Render_Model_Basic_With_Matrix(global::Models.Models.modMarker5, ref mv);
			}
		}
		value = 1f;
		global::Rendering.Rendering.effect1.Parameters["AlphaAdjust"].SetValue(value);
		global::Rendering.Rendering.far4[0] = 1f;
		global::Rendering.Rendering.far4[1] = 1f;
		global::Rendering.Rendering.far4[2] = 1f;
		global::Rendering.Rendering.far4[3] = 1f;
		global::Rendering.Rendering.effect1.Parameters["ColorAdjust"].SetValue(global::Rendering.Rendering.far4);
		if (transparent)
		{
			global::Rendering.Rendering.rGraphics.DepthStencilState = DepthStencilState.Default;
		}
	}

	public static void Commander_Teleporting_Player()
	{
		short num = mainC.playersMain.Get_Player_Index(global::Networking.Networking.networkBytes[0], -1);
		if (num >= 0)
		{
			if (num == 0)
			{
				commanderTeleportTimer = 5f;
				commanderTeleportPlayer = 0;
				commanderIsNotTeleporting = false;
				mainC.playersMain.Teleport(-1);
			}
			else
			{
				mainC.playersMain.Player_Teleporting_Out();
			}
		}
	}

	public static void Toggle_Commander_Mode()
	{
		if (global::Networking.Networking.isHost)
		{
			for (ushort num = 0; num < 5; num++)
			{
				Commander[num] = -1;
			}
			commanderMode = !commanderMode;
			if (commanderMode)
			{
				Commander[global::Players.Players.players[0].team] = 0;
				mainC.playersMain.Setup_Players_For_Commander_Mode();
			}
			else
			{
				mainC.playersMain.Clear_CommanderMode_Players();
			}
			mainC.gameLogic.Game_Send_GameSettings(5);
		}
	}

	public static void Commander_Reset_Ojbectives()
	{
		for (byte b = 0; b < 7; b++)
		{
			for (byte b2 = 0; b2 < numCommanderObjectives; b2++)
			{
				commanderObjectives[b, b2].order = 0;
				commanderObjectives[b, b2].active = false;
			}
		}
	}

	public void New_Team_Commander()
	{
		if (global::Networking.Networking.networkInts[0] < 0)
		{
			Commander[global::Networking.Networking.networkBytes[0]] = -1;
			return;
		}
		short num = mainC.playersMain.Get_Player_Index(global::Networking.Networking.networkInts[0], -1);
		if (num > -1)
		{
			Commander[global::Networking.Networking.networkBytes[0]] = (sbyte)num;
		}
	}

	public void Send_Player_Loaded(byte localPlayerID)
	{
		global::Networking.Networking.networkPlayers[localPlayerID].playerLoaded = true;
		mainC.networkingMain.XBOX_Send_Network_Message38(38);
	}

	public static void MP_Game_Started(byte threadID)
	{
		switch (gameState)
		{
		case 129:
		case 130:
		case 132:
			gameState = 133;
			break;
		case 131:
			break;
		}
	}

	public static void MP_Game_Cleanup_Before_Start(byte threadID)
	{
		if (gameState == 133)
		{
			for (int i = 0; i < maxHumanGamePlayers; i++)
			{
				mainC.playersMain.Set_Player_Race((byte)i, global::Players.Players.players[i].race, global::Players.Players.players[i].type);
				global::Players.Players.players[i].roundPts = 0;
				global::Players.Players.players[i].objectivePoints = 0;
			}
			mainC.maingameMain.Reset_Team_Scores();
		}
		for (int i = 1; i < maxHumanGamePlayers; i++)
		{
			global::Players.Players.players[i].charP.position.v[0] = global::Players.Players.mpData[i].currentPosX;
			global::Players.Players.players[i].charP.position.v[1] = global::Players.Players.mpData[i].currentPosY;
			global::Players.Players.players[i].charP.position.v[2] = global::Players.Players.mpData[i].currentPosZ;
		}
		roundOver = false;
		sendLapTime = 0f;
		global::InputHandler.InputHandler.messageBox_ExitToTitle = false;
		global::Rendering.Rendering.renderMenuScreen = 2;
		ref Vector3 reference = ref global::Rendering.Rendering.camUp[0];
		reference = global::Rendering.Rendering.worldUp;
		ref Vector3 reference2 = ref global::Rendering.Rendering.camUp[1];
		reference2 = global::Rendering.Rendering.worldUp;
		mainC.playersMain.Set_Voice_For_All_Remote_Players();
		mainC.fontmoduleMain.Clear_Onscreen_Text();
		global::InputHandler.InputHandler.confirmEndGameScreen = false;
		mainC.gameLogic.Game_MP_Game_Cleanup_Before_Start();
		if (global::Networking.Networking.networkSession.IsHost)
		{
			mainC.aiMain.Update_AI_Controlling_Players();
		}
		gameState = 26;
	}

	public void MP_Game_Ended()
	{
		Reset_MP_Map_Votes();
		mainC.playersMain.Reset_Local_Player_Variables_On_Round_Over();
		roundCurrentTime = 0f;
		roundOver = true;
		lobbyTimer = 0f;
		lobbyDataTimer = 3f;
		showResultsTimer = showResultsTime;
		gameState = 145;
		global::Rendering.Rendering.renderMenuScreen = 2;
		Reset_Select_Screens();
		Stop_Game_Functions(stopNarrator: false);
		mainC.playersMain.Sync_All_Player_Positions();
		mainC.playersMain.Sync_Local_Player_View();
		mainC.renderingMain.Sync_Particles();
		mainC.weaponsMain.Sync_Bullets();
		mainC.renderingMain.Sync_Rendering_Variables();
		mainC.vehicles.Sync_All_Vehicle_Matrices();
		mainC.weaponsMain.Sync_Weapon_Mounts();
		mainC.avatarMain.Sync_Avatar_Positions_With_Rendering_Frame();
		mainC.vehicles.Sync_Player_Vehicle_Mount_Matrices();
		mainC.maingameMain.Add_End_Of_Frame_Message(3);
		mainC.playersMain.Set_Player_Out_Of_Map(0);
		mainC.soundsMain.Set_Music(global::Sounds.Sounds.musicMenuID);
		mainC.gameLogic.Game_MP_Round_Over();
	}

	public static void MP_Back_To_Lobby_From_Game_Over()
	{
		mpGameDataReady = false;
		mpGameSetupNeedsToExit = false;
		autoStartMPGame = false;
		haveProgramData = false;
		lobbyTimer = 25f;
		lobbyMapVoteTimer = 8f;
		global::Rendering.Rendering.renderMenuScreen = 2;
		mainC.fontmoduleMain.Clear_Onscreen_Text();
		mainC.networkingMain.XBOX_Reset_Ready_Flags();
		mainC.networkingMain.XBOX_Enable_Voice_All();
		mainC.maingameMain.Copy_GameScore_Data(ref gameData, ref lastGameData);
		gameState = 129;
	}

	public static void Toggle_Multiplayer_GameType()
	{
		if (global::Networking.Networking.isHost)
		{
			switch (gameType)
			{
			case 0:
				gameType = 1;
				mainC.playersMain.Setup_Players_For_PrisonBreak();
				mainC.gameLogic.Game_Send_GameSettings(1);
				break;
			case 1:
				mainC.playersMain.Setup_Players_For_Commander_Mode();
				gameType = 0;
				mainC.playersMain.Clear_PrisonBreak_Players();
				mainC.gameLogic.Game_Send_GameSettings(1);
				break;
			}
		}
	}

	public void Reset_MP_Map_Votes()
	{
		needToTallyMapVotes = false;
		for (ushort num = 0; num < 4; num++)
		{
			playersMapChoice[num] = byte.MaxValue;
		}
		ushort num_MP_Levels = mainC.maingameMain.Get_Num_MP_Levels();
		for (ushort num = 0; num < num_MP_Levels; num++)
		{
			mapVotes[num] = 0;
		}
	}

	public void Calculate_Map_Votes()
	{
		ushort num_MP_Levels = mainC.maingameMain.Get_Num_MP_Levels();
		for (ushort num = 0; num < num_MP_Levels; num++)
		{
			mapVotes[num] = 0;
		}
		for (ushort num = 0; num < 4; num++)
		{
			if (playersMapChoice[num] < num_MP_Levels)
			{
				mapVotes[playersMapChoice[num]]++;
			}
		}
	}

	public ushort Get_Selected_Map()
	{
		ushort num = gameLevel;
		ushort num2 = 0;
		ushort num_MP_Levels = mainC.maingameMain.Get_Num_MP_Levels();
		for (ushort num3 = 0; num3 < num_MP_Levels; num3++)
		{
			if (mapVotes[num3] > num2 || (mapVotes[num3] == num2 && num == gameLevel))
			{
				num = num3;
				num2 = mapVotes[num3];
			}
		}
		return num;
	}

	public ushort Get_Num_MP_Levels()
	{
		byte b = gameType;
		if (b == 2)
		{
			return 19;
		}
		return 4;
	}

	public void Clear_Game_Items()
	{
		numGameItems = 0;
	}

	public ushort Register_Game_Item(byte type, ushort id, ushort index)
	{
		if (numGameItems == numAllocatedGameItems)
		{
			byte[] array = new byte[numGameItems];
			ushort[] array2 = new ushort[numGameItems];
			ushort[] array3 = new ushort[numGameItems];
			for (ushort num = 0; num < numGameItems; num++)
			{
				array[num] = gameItemType[num];
				array2[num] = gameItem[num];
				array3[num] = gameItemIndex[num];
			}
			numAllocatedGameItems += 50;
			gameItemType = new byte[numAllocatedGameItems];
			gameItem = new ushort[numAllocatedGameItems];
			gameItemIndex = new ushort[numAllocatedGameItems];
			for (ushort num = 0; num < numGameItems; num++)
			{
				gameItemType[num] = array[num];
				gameItem[num] = array2[num];
				gameItemIndex[num] = array3[num];
			}
		}
		gameItemType[numGameItems] = type;
		gameItem[numGameItems] = id;
		gameItemIndex[numGameItems] = index;
		return numGameItems++;
	}

	public ushort Get_Game_Item(byte type, ushort id, ushort defaultValue)
	{
		for (ushort num = 0; num < numGameItems; num++)
		{
			if (gameItemType[num] == type && gameItem[num] == id)
			{
				return num;
			}
		}
		return defaultValue;
	}

	public byte Get_Game_Item_Type(ushort gid)
	{
		return gameItemType[gid];
	}

	public ushort Get_Game_Item_Index(ushort gid)
	{
		return gameItemIndex[gid];
	}

	public void Set_Current_Achievement_Reward(byte direction)
	{
		if (direction == 1)
		{
			for (byte b = curAchievementReward; b < numAchievementRewards; b++)
			{
				if (achievementRewards[b].status == 1)
				{
					curAchievementReward = b;
					return;
				}
			}
			for (byte b = 0; b < curAchievementReward; b++)
			{
				if (achievementRewards[b].status == 1)
				{
					curAchievementReward = b;
					return;
				}
			}
		}
		else
		{
			for (byte b = curAchievementReward; b < numAchievementRewards; b--)
			{
				if (achievementRewards[b].status == 1)
				{
					curAchievementReward = b;
					return;
				}
			}
			for (byte b = (byte)(numAchievementRewards - 1); b > curAchievementReward; b--)
			{
				if (achievementRewards[b].status == 1)
				{
					curAchievementReward = b;
					return;
				}
			}
		}
		curAchievementReward = 0;
	}

	public void Reset_Game_Achievement_Rewards()
	{
		for (byte b = 0; b < numAchievementRewards; b++)
		{
			achievementRewards[b].rewardTimer = 0f;
			achievementRewards[b].status = 0;
		}
	}

	public void Activate_Achievement_Reward(byte rewardID)
	{
		if (achievementRewards[rewardID].status == 1)
		{
			achievementRewards[rewardID].status = 2;
			achievementRewards[rewardID].rewardTimer = achievementRewards[rewardID].rewardLength;
			Set_Current_Achievement_Reward(1);
			mainC.soundsMain.Play_Sound_NonPositional(achievementRewards[rewardID].activationSound);
		}
	}

	public bool Is_Achievement_Reward_Available(byte rewardID)
	{
		return achievementRewards[rewardID].status == 1;
	}

	public bool Can_Achievement_Reward_Be_Acquired(byte rewardID)
	{
		return achievementRewards[rewardID].status != 1;
	}

	public void Enable_Achievement_Reward(byte rewardID)
	{
		achievementRewards[rewardID].status = 1;
		lastAchievementReward = rewardID;
		Set_Current_Achievement_Reward(1);
	}

	public void Deactivate_Achievement_Reward(byte rewardID)
	{
		if (achievementRewards[rewardID].status != 1)
		{
			achievementRewards[rewardID].status = 0;
			achievementRewards[rewardID].rewardTimer = 0f;
			Set_Current_Achievement_Reward(1);
		}
	}

	public void Multiplayer_Start_Game_Creation_Process()
	{
		if (gameSetupRunning)
		{
			mpGameSetupNeedsToExit = true;
			return;
		}
		mpGameDataReady = false;
		mpGameSetupNeedsToExit = false;
		global::Threads.Threads.thread3Task = 6;
		global::Threads.Threads.thread3Start.Set();
	}

	public void Multiplayer_Start_Join_First_Game_Process()
	{
		if (gameSetupRunning)
		{
			mpGameSetupNeedsToExit = true;
			return;
		}
		mpGameDataReady = false;
		mpGameSetupNeedsToExit = false;
		global::Threads.Threads.thread3Task = 7;
		global::Threads.Threads.thread3Start.Set();
	}

	public void Multiplayer_Start_Join_Game_Process()
	{
		if (gameSetupRunning)
		{
			mpGameSetupNeedsToExit = true;
			return;
		}
		mpGameDataReady = false;
		mpGameSetupNeedsToExit = false;
		global::Threads.Threads.thread3Task = 9;
		global::Threads.Threads.thread3Start.Set();
	}

	public void Multiplayer_Start_Join_Game_Invite_Process()
	{
		if (gameSetupRunning)
		{
			mpGameSetupNeedsToExit = true;
			return;
		}
		mainC.maingameMain.Add_End_Of_Frame_Message(4);
		mpGameDataReady = false;
		mpGameSetupNeedsToExit = false;
		gameState = 150;
		global::Threads.Threads.thread3Task = 10;
		global::Threads.Threads.thread3Start.Set();
	}

	public void Multiplayer_Start_New_Game_Process()
	{
		if (gameSetupRunning)
		{
			mpGameSetupNeedsToExit = true;
			return;
		}
		mpGameDataReady = false;
		mpGameSetupNeedsToExit = false;
		global::Threads.Threads.thread3Task = 8;
		global::Threads.Threads.thread3Start.Set();
	}

	public void Multiplayer_Pre_Setup(byte threadID)
	{
		gameMode = 1;
		mainC.maingameMain.Clear_Game_Items();
		mainC.networkingMain.XBOX_Close_Session();
		mainC.playersMain.Reset_Team_Counts_To_Zero();
		ushort num;
		for (num = 0; num < 1; num++)
		{
			mainC.playersMain.Reset_Player(num, isActive: true, global::Players.Players.players[num].race, (byte)global::Players.Players.players[num].type);
		}
		while (num < 4)
		{
			mainC.playersMain.Reset_Player(num, isActive: false, global::Players.Players.players[num].race, (byte)global::Players.Players.players[num].type);
			num++;
		}
		Reset_GameScore_Data();
		global::AI.AI.hostNeedsToResetAiMP = false;
		global::Players.Players.currentPlayerRank = global::Players.Players.playerRankMP;
		global::Players.Players.remotePlayerRanks[0] = global::Players.Players.currentPlayerRank;
		becameMPHost = false;
		needToLoadPlayerSettings = false;
		haveProgramData = false;
		haveAllPlayerStatus = false;
		lobbyDataTimer = 3f;
		global::Players.Players.remotePlayerRanks[0] = global::Players.Players.currentPlayerRank;
		mainC.playersMain.Reset_Multiplayer_Player_Info();
		Stop_Game_Functions(stopNarrator: true);
		Reset_Game_Variables();
		Reset_MP_Map_Votes();
		mainC.gameLogic.Game_MP_Initial_New_Game_Setup(threadID);
		try
		{
			if (signedinGamerID > -1 && signedinGamerID < Gamer.SignedInGamers.Count && Gamer.SignedInGamers[signedinGamerID] != null && Gamer.SignedInGamers[signedinGamerID].IsSignedInToLive)
			{
				Gamer.SignedInGamers[signedinGamerID].Presence.PresenceMode = GamerPresenceMode.Multiplayer;
			}
		}
		catch (Exception)
		{
		}
		mainC.renderingMain.Free_Rendering_VBOs();
	}

	public void Multiplayer_Create_Game_Session(byte threadID)
	{
		gameSetupRunning = true;
		Multiplayer_Pre_Setup(threadID);
		if (!Mulitplayer_Create_Online_Network_Session())
		{
			gameState = 1;
			gameSetupRunning = false;
			return;
		}
		ushort num_MP_Levels = mainC.maingameMain.Get_Num_MP_Levels();
		maxGamePlayers = mp_numPlayers[mp_numPlayers_index];
		if (maxGamePlayers > 44)
		{
			maxGamePlayers = 44;
		}
		maxHumanGamePlayers = mp_numRemotePlayers[mp_numPlayers_index];
		if (maxHumanGamePlayers > 4)
		{
			maxHumanGamePlayers = 4;
		}
		if (gameLevel >= num_MP_Levels)
		{
			gameLevel = (byte)(num_MP_Levels - 1);
		}
		global::Players.Players.players[0].team = mainC.playersMain.Assign_Team(-1);
		global::Networking.Networking.networkPlayers[0].playerLoaded = true;
		global::Networking.Networking.networkPlayers[0].haveAllRemotePlayerDataForStart = true;
		global::Networking.Networking.networkPlayers[0].haveRemotePlayerArrayPosition = true;
		global::Networking.Networking.networkPlayers[0].haveRemotePlayerTeam = true;
		mainC.playersMain.LocalPlayer_Team_Change();
		haveProgramData = true;
		haveAllPlayerStatus = true;
		mainC.levelsMain.Set_Level(gameLevel, threadID);
		if (debugRestart > 0)
		{
			gameState = 1;
			mainC.userInterface.Load_Main_Menu();
			return;
		}
		mainC.playersMain.New_MultiPlayer_Round(threadID);
		gameState = 129;
		mainC.gameLogic.Game_MP_New_Game_Setup_Finished(threadID);
		lobbyTimer = 25f;
		lobbyMapVoteTimer = 8f;
		mpGameDataReady = true;
		gameSetupRunning = false;
	}

	public void Multiplayer_Join_First_Game_Session(byte threadID)
	{
		gameSetupRunning = true;
		Multiplayer_Pre_Setup(threadID);
		if (Mulitplayer_Join_First_Online_Network_Session())
		{
			mainC.userInterface.Close_Window(8);
			mainC.userInterface.Close_Window(7);
			mainC.maingameMain.Add_Start_Of_Frame_Message(1);
			gameState = 129;
			if (global::Networking.Networking.networkSession.SessionState == NetworkSessionState.Playing)
			{
				gameState = 136;
			}
			haveProgramData = false;
			haveAllPlayerStatus = false;
			global::Pickups.Pickups.receivedMPData = false;
			global::Pickups.Pickups.receivedPickupData = false;
			global::Pickups.Pickups.receivedPickupWeaponData = false;
			mainC.levelsMain.Set_Level(gameLevel, threadID);
			if (debugRestart > 0)
			{
				gameState = 1;
				mainC.userInterface.Load_Main_Menu();
				return;
			}
			mainC.playersMain.New_MultiPlayer_Round(threadID);
			mainC.gameLogic.Game_MP_New_Game_Setup_Finished(threadID);
			lobbyTimer = 25f;
			lobbyMapVoteTimer = 8f;
			if (mpLastLoadedLevel == gameLevel && mpLastLoadeGameType == gameType)
			{
				mainC.playersMain.Activate_All_Remote_Players_After_Load();
				mpGameDataReady = true;
				mainC.playersMain.Send_Player_Rank();
				Send_Player_Loaded(0);
			}
		}
		else
		{
			mainC.gameLogic.Game_JoinFirstGame_Failed();
			mainC.userInterface.Close_Window_After_Specified_Time(8, 1f);
			gameState = 1;
		}
		gameSetupRunning = false;
	}

	public void Multiplayer_Join_Game_Session(byte threadID)
	{
		gameSetupRunning = true;
		Multiplayer_Pre_Setup(threadID);
		if (Mulitplayer_Join_Online_Network_Session())
		{
			gameState = 129;
			if (global::Networking.Networking.networkSession.SessionState == NetworkSessionState.Playing)
			{
				gameState = 136;
			}
			haveProgramData = false;
			haveAllPlayerStatus = false;
			global::Pickups.Pickups.receivedMPData = false;
			global::Pickups.Pickups.receivedPickupData = false;
			global::Pickups.Pickups.receivedPickupWeaponData = false;
			mainC.levelsMain.Set_Level(gameLevel, threadID);
			if (debugRestart > 0)
			{
				gameState = 1;
				return;
			}
			mainC.playersMain.New_MultiPlayer_Round(threadID);
			mainC.gameLogic.Game_MP_New_Game_Setup_Finished(threadID);
			lobbyTimer = 25f;
			lobbyMapVoteTimer = 8f;
			Send_Special_Messages(5);
			if (mpLastLoadedLevel == gameLevel && mpLastLoadeGameType == gameType)
			{
				mainC.playersMain.Activate_All_Remote_Players_After_Load();
				mpGameDataReady = true;
				mainC.playersMain.Send_Player_Rank();
				Send_Player_Loaded(0);
			}
		}
		else
		{
			mainC.gameLogic.Game_JoinGame_Failed();
			gameState = 1;
		}
		gameSetupRunning = false;
	}

	public void Multiplayer_Join_Game_Invite(byte threadID)
	{
		gameSetupRunning = true;
		Multiplayer_Pre_Setup(threadID);
		if (Multiplayer_Join_Game_Invite())
		{
			gameState = 129;
			if (global::Networking.Networking.networkSession.SessionState == NetworkSessionState.Playing)
			{
				gameState = 136;
			}
			haveProgramData = false;
			haveAllPlayerStatus = false;
			global::Pickups.Pickups.receivedMPData = false;
			global::Pickups.Pickups.receivedPickupData = false;
			global::Pickups.Pickups.receivedPickupWeaponData = false;
			mainC.levelsMain.Set_Level(gameLevel, threadID);
			if (debugRestart > 0)
			{
				gameState = 1;
				return;
			}
			mainC.playersMain.New_MultiPlayer_Round(threadID);
			mainC.gameLogic.Game_MP_New_Game_Setup_Finished(threadID);
			lobbyTimer = 25f;
			lobbyMapVoteTimer = 8f;
			Send_Special_Messages(5);
			if (mpLastLoadedLevel == gameLevel && mpLastLoadeGameType == gameType)
			{
				mainC.playersMain.Activate_All_Remote_Players_After_Load();
				mpGameDataReady = true;
				mainC.playersMain.Send_Player_Rank();
				Send_Player_Loaded(0);
			}
		}
		else
		{
			mainC.gameLogic.Game_JoinGame_Failed();
			mainC.userInterface.Show_Window(1, 1, resetButtons: true);
			gameState = 1;
		}
		gameSetupRunning = false;
	}

	public void Multiplayer_Start_New_Game(byte threadID)
	{
		gameSetupRunning = true;
		ushort num_MP_Levels = mainC.maingameMain.Get_Num_MP_Levels();
		maxGamePlayers = mp_numPlayers[mp_numPlayers_index];
		if (maxGamePlayers > 44)
		{
			maxGamePlayers = 44;
		}
		maxHumanGamePlayers = mp_numRemotePlayers[mp_numPlayers_index];
		if (maxHumanGamePlayers > 4)
		{
			maxHumanGamePlayers = 4;
		}
		if (gameLevel >= num_MP_Levels)
		{
			gameLevel = (byte)(num_MP_Levels - 1);
		}
		Reset_Game_Variables();
		Reset_GameScore_Data();
		if (mainC.maingameMain.Game_Settings_Have_Changed())
		{
			mainC.levelsMain.Set_Level(gameLevel, threadID);
			if (debugRestart > 0)
			{
				return;
			}
		}
		else
		{
			mainC.soundsMain.Level_Reset();
		}
		mainC.playersMain.New_MultiPlayer_Round(threadID);
		mainC.gameLogic.Game_MP_New_Game_Setup_Finished(threadID);
		mainC.playersMain.Send_Player_Rank();
		Send_Player_Loaded(0);
		Send_Special_Messages(5);
		haveProgramData = true;
		lobbyTimer = 25f;
		lobbyMapVoteTimer = 8f;
		mpGameDataReady = true;
		gameSetupRunning = false;
	}

	public bool Mulitplayer_Create_Online_Network_Session()
	{
		global::Networking.Networking.networkPlayers[0].playerArrayPosition = 0;
		if (global::InputHandler.InputHandler.mpLive)
		{
			mainC.networkingMain.XBOX_Create_Session(NetworkSessionType.PlayerMatch, mpNumPrivateGamerSlots);
		}
		else
		{
			mainC.networkingMain.XBOX_Create_Session(NetworkSessionType.SystemLink, mpNumPrivateGamerSlots);
		}
		if (global::Networking.Networking.networkSessionReady)
		{
			mainC.networkingMain.XBOX_Reset_Ready_Flags();
			try
			{
				if (signedinGamerID > -1 && signedinGamerID < Gamer.SignedInGamers.Count && Gamer.SignedInGamers[signedinGamerID] != null && Gamer.SignedInGamers[signedinGamerID].IsSignedInToLive)
				{
					Gamer.SignedInGamers[signedinGamerID].Presence.PresenceMode = GamerPresenceMode.Multiplayer;
				}
			}
			catch (Exception)
			{
			}
			global::Networking.Networking.networkSession.LocalGamers[localNetworkGamerID].IsReady = false;
			switch (global::Networking.Networking.networkSession.SessionState)
			{
			case NetworkSessionState.Lobby:
				global::Networking.Networking.networkState = 1;
				global::Networking.Networking.networkSession.LocalGamers[localNetworkGamerID].IsReady = false;
				break;
			case NetworkSessionState.Playing:
				global::Networking.Networking.networkState = 2;
				break;
			case NetworkSessionState.Ended:
				global::Networking.Networking.networkState = 3;
				break;
			}
			return true;
		}
		return false;
	}

	public bool Mulitplayer_Join_First_Online_Network_Session()
	{
		global::Networking.Networking.networkPlayers[0].playerArrayPosition = -1;
		if (global::InputHandler.InputHandler.mpLive)
		{
			mainC.networkingMain.XBOX_Join_First_Session(NetworkSessionType.PlayerMatch);
		}
		else
		{
			mainC.networkingMain.XBOX_Join_First_Session(NetworkSessionType.SystemLink);
		}
		if (global::Networking.Networking.networkSessionReady)
		{
			mainC.avatarMain.Send_Avatar_Description_To_All_Players(0);
			try
			{
				if (signedinGamerID > -1 && signedinGamerID < Gamer.SignedInGamers.Count && Gamer.SignedInGamers[signedinGamerID] != null && Gamer.SignedInGamers[signedinGamerID].IsSignedInToLive)
				{
					Gamer.SignedInGamers[signedinGamerID].Presence.PresenceMode = GamerPresenceMode.Multiplayer;
				}
			}
			catch (Exception)
			{
			}
			switch (global::Networking.Networking.networkSession.SessionState)
			{
			case NetworkSessionState.Lobby:
				global::Networking.Networking.networkState = 1;
				global::Networking.Networking.networkSession.LocalGamers[localNetworkGamerID].IsReady = false;
				break;
			case NetworkSessionState.Playing:
				global::Networking.Networking.networkState = 2;
				break;
			case NetworkSessionState.Ended:
				global::Networking.Networking.networkState = 3;
				break;
			}
			return true;
		}
		return false;
	}

	public bool Mulitplayer_Join_Online_Network_Session()
	{
		global::Networking.Networking.networkPlayers[0].playerArrayPosition = -1;
		if (global::InputHandler.InputHandler.mpLive)
		{
			mainC.networkingMain.XBOX_Join_Session(global::InputHandler.InputHandler.chosenSessionID, NetworkSessionType.PlayerMatch);
		}
		else
		{
			mainC.networkingMain.XBOX_Join_Session(global::InputHandler.InputHandler.chosenSessionID, NetworkSessionType.SystemLink);
		}
		if (global::Networking.Networking.networkSessionReady)
		{
			mainC.avatarMain.Send_Avatar_Description_To_All_Players(0);
			try
			{
				if (signedinGamerID > -1 && signedinGamerID < Gamer.SignedInGamers.Count && Gamer.SignedInGamers[signedinGamerID] != null && Gamer.SignedInGamers[signedinGamerID].IsSignedInToLive)
				{
					Gamer.SignedInGamers[signedinGamerID].Presence.PresenceMode = GamerPresenceMode.Multiplayer;
				}
			}
			catch (Exception)
			{
			}
			switch (global::Networking.Networking.networkSession.SessionState)
			{
			case NetworkSessionState.Lobby:
				global::Networking.Networking.networkState = 1;
				global::Networking.Networking.networkSession.LocalGamers[localNetworkGamerID].IsReady = false;
				break;
			case NetworkSessionState.Playing:
				global::Networking.Networking.networkState = 2;
				break;
			case NetworkSessionState.Ended:
				global::Networking.Networking.networkState = 3;
				break;
			}
			return true;
		}
		return false;
	}

	public bool Multiplayer_Join_Game_Invite()
	{
		global::Networking.Networking.networkPlayers[0].playerArrayPosition = -1;
		mainC.networkingMain.XBOX_Join_Game_Invite();
		if (global::Networking.Networking.networkSessionReady)
		{
			mainC.avatarMain.Send_Avatar_Description_To_All_Players(0);
			try
			{
				if (signedinGamerID > -1 && signedinGamerID < Gamer.SignedInGamers.Count && Gamer.SignedInGamers[signedinGamerID] != null && Gamer.SignedInGamers[signedinGamerID].IsSignedInToLive)
				{
					Gamer.SignedInGamers[signedinGamerID].Presence.PresenceMode = GamerPresenceMode.Multiplayer;
				}
			}
			catch (Exception)
			{
			}
			switch (global::Networking.Networking.networkSession.SessionState)
			{
			case NetworkSessionState.Lobby:
				global::Networking.Networking.networkState = 1;
				global::Networking.Networking.networkSession.LocalGamers[localNetworkGamerID].IsReady = false;
				break;
			case NetworkSessionState.Playing:
				global::Networking.Networking.networkState = 2;
				break;
			case NetworkSessionState.Ended:
				global::Networking.Networking.networkState = 3;
				break;
			}
			return true;
		}
		return false;
	}

	public void Multiplayer_Failed(byte reason)
	{
		mainC.inputMain.Multiplayer_Failed(reason);
		try
		{
			if (signedinGamerID > -1 && signedinGamerID < Gamer.SignedInGamers.Count && Gamer.SignedInGamers[signedinGamerID] != null && Gamer.SignedInGamers[signedinGamerID].IsSignedInToLive)
			{
				Gamer.SignedInGamers[signedinGamerID].Presence.PresenceMode = GamerPresenceMode.AtMenu;
			}
		}
		catch (Exception)
		{
		}
	}

	public void Stop_Game_Functions(bool stopNarrator)
	{
		mainC.renderingMain.Game_Over_Cleanup();
		mainC.soundsMain.Stop_All_Game_Sounds(stopNarrator);
		mainC.inputMain.GamePad_Vibration_Stop();
	}

	public void Reset_Game_Variables()
	{
		hostStartedGame = false;
		roundOver = false;
		displayLaps = 1;
		if (commanderMode)
		{
			isCommander = false;
			commanderItem = 0;
			Commander[0] = -1;
			Commander[1] = -1;
			Commander[2] = -1;
			Commander_Reset_Ojbectives();
		}
		byte b = gameType;
		if (b == 1)
		{
			isGuard = true;
			Guards[0] = 1;
			global::Players.Players.numGuards = 1;
			global::Players.Players.numPrisoners = 0;
			for (byte b2 = 1; b2 < 44; b2++)
			{
				Guards[b2] = 0;
			}
		}
	}

	public void Reset_GameScore_Data()
	{
		gameData.difficulty = difficulty;
		gameData.gameMode = gameMode;
		gameData.gameType = gameType;
		gameData.level = gameLevel;
		gameData.numPlayers = maxGamePlayers;
		gameData.numRounds = numRounds;
		gameData.numTeams = numTeams;
		gameData.roundTime = roundTimeLimit;
		gameData.timeRemaining = roundTimeLimit;
		gameData.commanderMode = commanderMode;
		mainC.gameLogic.Game_Reset_Scores();
	}

	public void Update_GameScore_Main_Data()
	{
		gameData.difficulty = difficulty;
		gameData.gameMode = gameMode;
		gameData.gameType = gameType;
		gameData.level = gameLevel;
		gameData.numPlayers = maxGamePlayers;
		gameData.numRounds = numRounds;
		gameData.numTeams = numTeams;
		gameData.roundTime = roundTimeLimit;
		gameData.timeRemaining = roundTimeLimit;
		gameData.commanderMode = commanderMode;
	}

	public void Copy_GameScore_Data(ref StructsClass.GameInfo src, ref StructsClass.GameInfo dst)
	{
		dst.gameMode = src.gameMode;
		dst.gameType = src.gameType;
		dst.numTeams = src.numRounds;
		dst.difficulty = src.difficulty;
		dst.roundsWon = src.roundsWon;
		dst.commanderMode = src.commanderMode;
		dst.numPlayers = src.numPlayers;
		dst.level = src.level;
		dst.roundTime = src.roundTime;
		dst.floatSize = src.floatSize;
		dst.intSize = src.intSize;
		dst.ushortDataSize = src.ushortDataSize;
		ushort floatSize = dst.floatSize;
		ushort intSize = dst.intSize;
		ushort ushortDataSize = dst.ushortDataSize;
		ushort numPlayers = dst.numPlayers;
		for (ushort num = 0; num < numPlayers; num++)
		{
			dst.players[num].xboxID = src.players[num].xboxID;
			dst.players[num].timePlayed = src.players[num].timePlayed;
			dst.players[num].id = src.players[num].id;
			for (ushort num2 = 0; num2 < floatSize; num2++)
			{
				dst.players[num].scoresF[num2] = src.players[num].scoresF[num2];
			}
			for (ushort num2 = 0; num2 < intSize; num2++)
			{
				dst.players[num].scoresI[num2] = src.players[num].scoresI[num2];
			}
			for (ushort num2 = 0; num2 < ushortDataSize; num2++)
			{
				dst.players[num].dataUS[num2] = src.players[num].dataUS[num2];
			}
		}
	}

	public bool Game_Settings_Have_Changed()
	{
		switch (gameMode)
		{
		case 0:
			if (spLastLoadedLevel != gameLevel || spLastLoadeGameType != gameType)
			{
				return true;
			}
			break;
		case 1:
			if (mpLastLoadedLevel != gameLevel || mpLastLoadeGameType != gameType)
			{
				return true;
			}
			break;
		}
		return false;
	}

	public void Restart_Level_From_Menu()
	{
		if (gameMode == 1)
		{
			mainC.playersMain.MP_Player_Suicide();
		}
		else
		{
			mainC.maingameMain.SP_Restart(0);
		}
		mainC.inputMain.Leave_Menu_Completely();
		mainC.maingameMain.Leaving_Menu_State();
	}

	public void Exit_To_Title_From_Menu()
	{
		gameState = 254;
	}

	public void Respawn_From_Menu()
	{
		mainC.soundsMain.Stop_Continual_Sound(0);
		if (gameMode == 1)
		{
			mainC.playersMain.MP_Player_Suicide();
		}
		else
		{
			mainC.playersMain.SP_Player_Suicide();
		}
		mainC.playersMain.Player_Over(0, playerDied: true, 0);
		mainC.programsMain.Reset_Programs(ref global::Players.Players.players[0].pg1, ref global::Players.Players.players[0].animations, global::Players.Players.players[0].programCollection);
		global::Players.Players.players[0].onmap = 1;
		global::Players.Players.mainPlayerDeathTimer = 2.5f;
		mainC.inputMain.Leave_Menu_Completely();
		mainC.maingameMain.Leaving_Menu_State();
		global::Players.Players.respawnEnabled = false;
		gameState = 26;
		if (gameMode == 0 && numLives < 1 && linearProgression)
		{
			mainC.gameLogic.Game_SP_Game_Ended();
		}
	}

	public void SP_Initial_Setup(byte threadID)
	{
		gameSetupRunning = true;
		global::Players.Players.currentPlayerRank = global::Players.Players.playerRankSP;
		mainC.maingameMain.Clear_Game_Items();
		mainC.inputMain.Setup_For_SP();
		mainC.playersMain.Clear_Players();
		mainC.networkingMain.Setup_For_SP();
		mpSetupReady = false;
		curSpLevel = 255;
		gameMode = 0;
		if (global::InputHandler.InputHandler.newSPGame)
		{
			gameLevel = 0;
		}
		mainC.levelsMain.Set_Level(gameLevel, threadID);
		if (debugRestart <= 0)
		{
			mainC.playersMain.New_SinglePlayer_Round(minorRestart: false, 0);
			mainC.renderingMain.Set_Camera_To_Initial_View();
			if (global::InputHandler.InputHandler.newSPGame)
			{
				mainC.weaponsMain.Set_Minimum_Ammo(0);
				Save_SP_Game(numLives);
			}
			global::Networking.Networking.networkPlayers[0].playerLoaded = true;
			mainC.gameLogic.Game_SP_Initial_Setup_Finished();
			mainC.inputMain.Singleplayer_Session_Ready();
			gameSetupRunning = false;
		}
	}

	public void Set_SP_Level_To_Completed()
	{
		roundOver = true;
	}

	public void SP_Level_Complete(byte threadID)
	{
		Sync_All_Game_Items();
		Reset_Select_Screens();
		Stop_Game_Functions(stopNarrator: false);
		mainC.weaponsMain.Move_Weapon_Rounds_To_Ammo_Clip_Surplus(0);
		roundStarting = true;
		roundOver = true;
		roundCurrentTime = 2f;
		global::AI.AI.levelKillCount = 0;
		switch (mainC.levelsMain.Get_Next_SP_Level())
		{
		case 0:
			spSaving = 1;
			gameState = 6;
			mainC.gameLogic.Game_SP_Round_Over();
			mainC.gameLogic.Game_Show_Results_Window();
			break;
		case 1:
			gameState = 8;
			mainC.soundsMain.Stop_Music();
			mainC.gameLogic.Game_SP_Game_Finished();
			mainC.gameLogic.Game_Show_Results_Window();
			break;
		case 2:
			spSaving = 1;
			gameState = 10;
			mainC.gameLogic.Game_SP_Trial_Over();
			mainC.gameLogic.Game_Show_Results_Window();
			break;
		}
	}

	public void SP_Restart(byte threadID)
	{
		global::Rendering.Rendering.renderMenuScreen = 2;
		if (linearProgression)
		{
			Load_SP_Game();
		}
		mainC.gameLogic.Game_Reset_SP_Level_Do_First();
		if (mainC.maingameMain.Game_Settings_Have_Changed())
		{
			mainC.levelsMain.Set_Level(gameLevel, threadID);
			if (debugRestart > 0)
			{
				return;
			}
		}
		else
		{
			mainC.soundsMain.Level_Reset();
		}
		global::AI.AI.levelKillCount = 0;
		mainC.mapsMain.Reset_Round_Maps_Section(threadID);
		mainC.playersMain.New_SinglePlayer_Round(minorRestart: false, 0);
		byte b = gameType;
		if (b == 4)
		{
			global::Rendering.Rendering.renderMenuScreen = 0;
			gameState = 18;
		}
		else
		{
			gameState = 26;
		}
		mainC.gameLogic.Game_Reset_SP_Level_Do_Last(threadID);
	}

	public void Leaving_Menu_State()
	{
		switch (gameMode)
		{
		case 0:
			switch (gameState)
			{
			case 1:
			{
				byte b = gameType;
				if (b == 4)
				{
					gameState = 18;
				}
				else
				{
					gameState = 26;
				}
				switch (gameState)
				{
				default:
					global::Rendering.Rendering.renderMenuScreen = 0;
					break;
				case 25:
				case 26:
					break;
				}
				mainC.soundsMain.Play_Level_Music();
				break;
			}
			case 4:
				gameState = 5;
				break;
			case 5:
				global::Rendering.Rendering.renderMenuScreen = 0;
				gameState = 2;
				mainC.soundsMain.Play_Level_Music();
				break;
			case 14:
				gameState = 15;
				break;
			case 15:
				global::Rendering.Rendering.renderMenuScreen = 0;
				gameState = 2;
				mainC.soundsMain.Play_Level_Music();
				break;
			}
			break;
		case 1:
			switch (gameState)
			{
			case 1:
				global::Rendering.Rendering.renderMenuScreen = 2;
				gameState = 136;
				if (global::Networking.Networking.isHost || global::Networking.Networking.networkState != 2)
				{
					gameState = 129;
				}
				break;
			case 143:
				gameState = 144;
				break;
			case 144:
				switch (global::Players.Players.currentView)
				{
				default:
					global::Rendering.Rendering.renderMenuScreen = 0;
					gameState = 141;
					mainC.soundsMain.Play_Level_Music();
					break;
				}
				break;
			case 139:
				gameState = 140;
				break;
			case 140:
				global::Rendering.Rendering.renderMenuScreen = 2;
				gameState = 137;
				break;
			case 132:
				gameState = 129;
				break;
			}
			break;
		}
	}

	public void Entering_Menu_State()
	{
		switch (gameMode)
		{
		case 0:
			switch (gameState)
			{
			case 2:
				gameState = 3;
				break;
			case 12:
				gameState = 13;
				break;
			}
			break;
		case 1:
			switch (gameState)
			{
			case 129:
				if (enteringSettingsMenu)
				{
					settingsTimeLimit = mp_timeLimit_index;
					settingsMap = gameLevel;
				}
				gameState = 130;
				break;
			case 137:
				gameState = 138;
				break;
			case 138:
				gameState = 139;
				break;
			case 141:
			case 149:
				gameState = 142;
				break;
			case 142:
				switch (global::Players.Players.currentView)
				{
				default:
					gameState = 143;
					break;
				}
				break;
			}
			break;
		}
	}

	public float Get_Terrain_Height(float x, float y, byte threadID)
	{
		return mainC.terrainMain.Get_Terrain_Height(x, y, threadID);
	}

	public float Get_Map_Height(float x, float y, byte threadID)
	{
		return mainC.mapsMain.Get_Map_Height(x, y, threadID);
	}

	public void Sync_All_Game_Items()
	{
		mainC.playersMain.Sync_All_Player_Positions();
		mainC.playersMain.Sync_Local_Player_View();
		mainC.renderingMain.Sync_Particles();
		mainC.weaponsMain.Sync_Bullets();
		mainC.renderingMain.Sync_Rendering_Variables();
		mainC.vehicles.Sync_All_Vehicle_Matrices();
		mainC.weaponsMain.Sync_Weapon_Mounts();
		mainC.avatarMain.Sync_Avatar_Positions_With_Rendering_Frame();
		mainC.vehicles.Sync_Player_Vehicle_Mount_Matrices();
		mainC.maingameMain.Add_End_Of_Frame_Message(3);
	}

	public void Reset_Select_Screens()
	{
		User_Interface.weaponSelectScreenOpen = false;
		User_Interface.vehicleSelectScreenOpen = false;
		User_Interface.missionObjectivesScreenOpen = false;
		User_Interface.vehicleSelectFinished = false;
		User_Interface.weaponSelectFinished = false;
	}

	public void Process_Start_Of_Frame_Messages()
	{
		for (byte b = 0; b < numStartOfFrameMessages; b++)
		{
			switch (startOfFrameMessages[b])
			{
			case 0:
				mainC.userInterface.Load_User_Interface("UI.txt");
				break;
			case 1:
				mainC.userInterface.Close_Window(1);
				break;
			}
		}
		numStartOfFrameMessages = 0;
	}

	public void Add_Start_Of_Frame_Message(byte messageID)
	{
		if (numStartOfFrameMessages >= maxMessages)
		{
			if (maxMessages + 10 > global::Util.Util.maxUnsignedShortValue)
			{
				return;
			}
			ushort num = (ushort)(maxMessages + 10);
			byte[] array = new byte[num];
			for (ushort num2 = 0; num2 < maxMessages; num2++)
			{
				array[num2] = startOfFrameMessages[num2];
			}
			startOfFrameMessages = new byte[num];
			for (ushort num2 = 0; num2 < maxMessages; num2++)
			{
				startOfFrameMessages[num2] = array[num2];
			}
			maxMessages = num;
		}
		startOfFrameMessages[numStartOfFrameMessages] = messageID;
		numStartOfFrameMessages++;
	}

	public void Process_End_Of_Frame_Messages()
	{
		for (byte b = 0; b < numEndOfFrameMessages; b++)
		{
			switch (endOfFrameMessages[b])
			{
			case 0:
			{
				byte rBufferID = global::Rendering.Rendering.uBufferID;
				byte uBufferID = global::Rendering.Rendering.rBufferID;
				global::Rendering.Rendering.rBufferID = rBufferID;
				global::Rendering.Rendering.uBufferID = uBufferID;
				mainC.playersMain.Process_Sync_Local_Player_View_Message();
				mainC.renderingMain.Set_Camera_To_Camera_Goal_Positions();
				global::Rendering.Rendering.rBufferID = uBufferID;
				global::Rendering.Rendering.uBufferID = rBufferID;
				break;
			}
			case 1:
				global::Rendering.Rendering.renderMenuScreen = 0;
				break;
			case 2:
			{
				byte rBufferID = global::Rendering.Rendering.rBufferID;
				byte uBufferID = global::Rendering.Rendering.uBufferID;
				global::Rendering.Rendering.uBufferID = lastUpdateBuffer;
				global::Rendering.Rendering.rBufferID = (byte)((lastUpdateBuffer + 1) % 2);
				mainC.renderingMain.Set_Camera_To_Camera_Goal_Positions();
				global::Rendering.Rendering.rBufferID = rBufferID;
				global::Rendering.Rendering.uBufferID = uBufferID;
				break;
			}
			case 3:
				global::Players.Players.players[0].posX[global::Rendering.Rendering.rBufferID] = global::Players.Players.players[0].posX[global::Rendering.Rendering.uBufferID];
				global::Players.Players.players[0].posY[global::Rendering.Rendering.rBufferID] = global::Players.Players.players[0].posY[global::Rendering.Rendering.uBufferID];
				global::Players.Players.players[0].posZ[global::Rendering.Rendering.rBufferID] = global::Players.Players.players[0].posZ[global::Rendering.Rendering.uBufferID];
				global::Rendering.Rendering.viewPositionX = 0f;
				global::Rendering.Rendering.viewPositionY = 0f;
				global::Rendering.Rendering.viewPositionZ = 0f;
				break;
			case 4:
				mainC.userInterface.Close_All_Windows();
				break;
			}
		}
		numEndOfFrameMessages = 0;
	}

	public void Add_End_Of_Frame_Message(byte messageID)
	{
		if (numEndOfFrameMessages >= maxMessages)
		{
			if (maxMessages + 10 > global::Util.Util.maxUnsignedShortValue)
			{
				return;
			}
			ushort num = (ushort)(maxMessages + 10);
			byte[] array = new byte[num];
			for (ushort num2 = 0; num2 < maxMessages; num2++)
			{
				array[num2] = endOfFrameMessages[num2];
			}
			endOfFrameMessages = new byte[num];
			for (ushort num2 = 0; num2 < maxMessages; num2++)
			{
				endOfFrameMessages[num2] = array[num2];
			}
			maxMessages = num;
		}
		endOfFrameMessages[numEndOfFrameMessages] = messageID;
		numEndOfFrameMessages++;
	}

	public void Controller_Disconnected()
	{
		Pause_Game();
	}

	public void Pause_Game()
	{
		switch (gameState)
		{
		case 2:
			gameState = 3;
			break;
		case 5:
			gameState = 4;
			break;
		case 141:
			gameState = 142;
			break;
		case 144:
			gameState = 143;
			break;
		case 140:
			global::Sounds.Sounds.soundEnabled[0] = true;
			global::Sounds.Sounds.soundEnabled[2] = true;
			global::Rendering.Rendering.renderMenuScreen = 0;
			mainC.soundsMain.Play_Level_Music();
			gameState = 143;
			break;
		}
	}

	public void Player_Opened_Guide()
	{
		byte b = gameState;
		if (b == 2)
		{
			global::InputHandler.InputHandler.controllerButtonStartPressed = true;
		}
	}

	public void Calculate_Frame_Time()
	{
		frametime = (float)(mainTime - framestart) * 1E-07f;
		framestart = mainTime;
		if (frametime > 0.0667f)
		{
			frametime = 0.0667f;
		}
		else if (frametime == 0f)
		{
			frametime = 0.0001f;
		}
		frameTimeAdjusted = frametime / global::Physics.Physics.timeMod;
	}

	public void Update_Network_Message_Time(float timeToWait)
	{
		curFrameTime += frametime;
		while (curFrameTime > (double)timeToWait)
		{
			curFrameTime -= timeToWait;
			frameC1++;
		}
	}

	public void Save_Frame_Time()
	{
		frameTimePrioToPause = frametime;
	}

	public void Restore_Frame_Time()
	{
		framestart = mainTime - (int)(frameTimePrioToPause * 10000000f);
		frametime = frameTimePrioToPause;
		frameTimeAdjusted = frametime / global::Physics.Physics.timeMod;
	}

	public void Reset_Round()
	{
		roundOver = false;
		roundStarting = true;
		roundCurrentTime = roundTimeLimit;
		cameraMovementSpeed = cameraMovementSpeedDefault;
		raceStartTimer = 3f;
		currentRaceStartTimer = 3;
		if (commanderMode)
		{
			Commander[1] = -1;
			Commander[2] = -1;
			commanderSelect = -1;
			Commander_Reset_Ojbectives();
		}
		Reset_Team_Scores();
		mainC.inputMain.GamePad_Vibration_Stop();
	}

	public void Reset_Team_Scores()
	{
		for (ushort num = 0; num < 5; num++)
		{
			global::Players.Players.teamPoints[num] = 0;
		}
	}

	public void Close_All()
	{
		mainC.soundsMain.Close_Sound();
		mainC.threadingMain.Close();
		Save_Global_Settings();
	}

	public void Tell_A_Friend()
	{
		try
		{
			if (signedinGamerID > -1 && signedinGamerID < Gamer.SignedInGamers.Count && Gamer.SignedInGamers[signedinGamerID] != null && Gamer.SignedInGamers[signedinGamerID].IsSignedInToLive)
			{
				Guide.ShowComposeMessage(Gamer.SignedInGamers[signedinGamerID].PlayerIndex, "Hey! I'm playing a game called The Co-Op Zombie Game! I thought you would like it too. Go to the Games Marketplace, then Indie Games to download the FREE Trial.", null);
			}
			else if (!Guide.IsVisible)
			{
				Guide.ShowSignIn(1, onlineOnly: false);
			}
		}
		catch (Exception)
		{
		}
	}

	public void Receive_Map_Vote(byte mapID, byte actID)
	{
		short num;
		if ((num = mainC.playersMain.Get_Player_Index(actID, -1)) >= 0)
		{
			playersMapChoice[num] = mapID;
			mainC.maingameMain.Calculate_Map_Votes();
		}
	}

	public static void Send_ProgramData_RoundTimer_To_Player(NetworkGamer newGamer)
	{
		ref HalfSingle reference = ref global::Networking.Networking.networkHS[0];
		reference = new HalfSingle(roundCurrentTime);
		mainC.networkingMain.XBOX_Send_Network_Message_To_Gamer(27, newGamer);
		mainC.programsMain.XBOX_Send_Program_Status(newGamer);
	}

	public void Receive_LobbyTimer_Update()
	{
		lobbyTimer = global::Networking.Networking.networkFloats[0] - 0.25f;
		lobbyMapVoteTimer = global::Networking.Networking.networkFloats[1] - 0.25f;
	}

	public void Send_Special_Messages(byte type)
	{
		ushort num = (ushort)(1 << (int)type);
		for (int i = 1; i < 4; i++)
		{
			global::Players.Players.mpData[i].specialData |= num;
		}
	}

	public void Receive_Update_Remote_Player_Score()
	{
		int num = mainC.playersMain.Get_Player_Index(global::Networking.Networking.networkBytes[0], -1);
		if (num != -1)
		{
			float num2 = global::Networking.Networking.networkHS[0].ToSingle();
			if (num2 >= gameData.players[num].scoresF[0])
			{
				gameData.players[num].scoresF[0] = num2;
			}
			else
			{
				gameData.players[num].scoresF[0] -= frametime * (raceModeMultHeight[num] + racePenaltyTime[num]);
			}
		}
	}

	public void Player_Finished_Lap()
	{
		int num = mainC.playersMain.Get_Player_Index(global::Networking.Networking.networkBytes[0], -1);
		if (num != -1)
		{
			laps[num] = global::Networking.Networking.networkBytes[1];
		}
	}

	public void Player_Finished_Race()
	{
		int num = mainC.playersMain.Get_Player_Index(global::Networking.Networking.networkBytes[0], -1);
		if (num != -1)
		{
			raceFinished[num] = 1;
			laps[num] = (byte)(levelLapsToFinish + 1);
		}
	}

	public void Send_Race_Starting_Participants()
	{
		for (ushort num = 0; num < maxHumanGamePlayers; num++)
		{
			if (global::Players.Players.players[num].active)
			{
				raceFinished[num] = 0;
				global::Networking.Networking.networkInts[0] = global::Players.Players.players[num].id;
				global::Networking.Networking.networkBytes[0] = 0;
				mainC.networkingMain.XBOX_Send_Network_Message37(37);
			}
		}
	}

	public void Send_Race_Participants_Status_To_NewGamer(NetworkGamer newGamer)
	{
		for (ushort num = 0; num < maxHumanGamePlayers; num++)
		{
			if (global::Players.Players.players[num].active)
			{
				global::Networking.Networking.networkInts[0] = global::Players.Players.players[num].id;
				global::Networking.Networking.networkBytes[0] = raceFinished[num];
				mainC.networkingMain.XBOX_Send_Network_Message_To_Gamer(37, newGamer);
			}
		}
	}

	public void Receive_Race_Participant_Status()
	{
		int num = mainC.playersMain.Get_Player_Index(global::Networking.Networking.networkInts[0], -1);
		if (num != -1)
		{
			raceFinished[num] = global::Networking.Networking.networkBytes[0];
		}
	}

	public void Send_New_Race_Participant_Status(NetworkGamer newGamer)
	{
		if (global::Networking.Networking.networkState != 1)
		{
			ushort num = (ushort)mainC.playersMain.Get_Player_Index(newGamer.Id, -1);
			if (num >= 0)
			{
				raceFinished[num] = 1;
				global::Networking.Networking.networkInts[0] = global::Players.Players.players[num].id;
				global::Networking.Networking.networkBytes[0] = 1;
				mainC.networkingMain.XBOX_Send_Network_Message37(37);
			}
		}
	}

	public void Receive_Highscroe_And_FavoriteAward()
	{
		int num = mainC.networkingMain.XBOX_Get_RemoteGamer_Index(global::Networking.Networking.networkBytes[0], -1);
		_ = -1;
		num = mainC.playersMain.Get_Player_Index(global::Networking.Networking.networkBytes[0], -1);
		_ = -1;
	}

	public void Choose_Gamer_Storage_Container()
	{
		if (Guide.IsVisible || User_Interface.windows[14].status > 0)
		{
			return;
		}
		try
		{
			switch (global::InputHandler.InputHandler.gpadID)
			{
			case 0:
				StorageDevice.BeginShowSelector(PlayerIndex.One, Gamer_Device_Ready, null);
				break;
			case 1:
				StorageDevice.BeginShowSelector(PlayerIndex.Two, Gamer_Device_Ready, null);
				break;
			case 2:
				StorageDevice.BeginShowSelector(PlayerIndex.Three, Gamer_Device_Ready, null);
				break;
			case 3:
				StorageDevice.BeginShowSelector(PlayerIndex.Four, Gamer_Device_Ready, null);
				break;
			}
		}
		catch (Exception)
		{
		}
	}

	public void Gamer_Device_Ready(IAsyncResult result)
	{
		try
		{
			deviceGamer = StorageDevice.EndShowSelector(result);
			if (deviceGamer == null)
			{
				mainC.userInterface.Show_Window(14, 14, resetButtons: true);
			}
		}
		catch (Exception)
		{
		}
	}

	public static StorageContainer Open_Gamer_Container()
	{
		IAsyncResult asyncResult = deviceGamer.BeginOpenContainer("The Co-Op Zombie Game", null, null);
		asyncResult.AsyncWaitHandle.WaitOne();
		StorageContainer result = deviceGamer.EndOpenContainer(asyncResult);
		asyncResult.AsyncWaitHandle.Close();
		return result;
	}

	public bool Save_Player_Settings()
	{
		if (storageDeviceNotChosen)
		{
			needToSavePlayerSettings = false;
			return true;
		}
		if (signedinGamerID < 0 || Gamer.SignedInGamers[signedinGamerID].IsGuest)
		{
			return Save_Player_Settings_IsolatedStorage();
		}
		StorageContainer[] array = new StorageContainer[1];
		try
		{
			if (deviceGamer == null || !deviceGamer.IsConnected)
			{
				Choose_Gamer_Storage_Container();
				return false;
			}
			Save_Global_Settings();
			needToSavePlayerSettings = false;
			array[0] = Open_Gamer_Container();
			Stream stream = array[0].OpenFile("PlayerData.txt", FileMode.Create);
			if (stream.CanWrite)
			{
				string text = "3rd_Person\t" + string.Format(CultureInfo.InvariantCulture, "{0:F3}", new object[1] { global::Players.Players.thirdPersonXAdj }) + "\t" + string.Format(CultureInfo.InvariantCulture, "{0:F3}", new object[1] { global::Players.Players.thirdPersonYAdj }) + "\t" + string.Format(CultureInfo.InvariantCulture, "{0:F3}", new object[1] { global::Players.Players.thirdPersonZAdj }) + "\r\n";
				stream.Write(mainC.utilMain.String_To_Byte_Array(text), 0, text.Length);
				text = "invertY\t" + string.Format(CultureInfo.InvariantCulture, "{0:F3}", new object[1] { global::Players.Players.invertY }) + "\r\n";
				stream.Write(mainC.utilMain.String_To_Byte_Array(text), 0, text.Length);
				text = "invertYSecondary\t" + string.Format(CultureInfo.InvariantCulture, "{0:F3}", new object[1] { global::Players.Players.invertYSecondary }) + "\r\n";
				stream.Write(mainC.utilMain.String_To_Byte_Array(text), 0, text.Length);
				text = "lookSensitivity0\t" + string.Format(CultureInfo.InvariantCulture, "{0:F3}", new object[1] { global::InputHandler.InputHandler.lookSensitivity[0] }) + "\r\n";
				stream.Write(mainC.utilMain.String_To_Byte_Array(text), 0, text.Length);
				text = "lookSensitivity1\t" + string.Format(CultureInfo.InvariantCulture, "{0:F3}", new object[1] { global::InputHandler.InputHandler.lookSensitivity[1] }) + "\r\n";
				stream.Write(mainC.utilMain.String_To_Byte_Array(text), 0, text.Length);
				text = "brightness\t" + string.Format(CultureInfo.InvariantCulture, "{0:F3}", new object[1] { global::Rendering.Rendering.brightness }) + "\r\n";
				stream.Write(mainC.utilMain.String_To_Byte_Array(text), 0, text.Length);
				text = "Rumble\t" + global::InputHandler.InputHandler.rumble.ToString(CultureInfo.InvariantCulture) + "\r\n";
				stream.Write(mainC.utilMain.String_To_Byte_Array(text), 0, text.Length);
				text = "SwapSticks\t0\r\n";
				if (global::InputHandler.InputHandler.swapSticks)
				{
					text = "SwapSticks\t1\r\n";
				}
				stream.Write(mainC.utilMain.String_To_Byte_Array(text), 0, text.Length);
				text = "gunView\t" + string.Format(CultureInfo.InvariantCulture, "{0:F3}", new object[1] { global::Players.Players.firstPersonViewAdjX }) + "\t" + string.Format(CultureInfo.InvariantCulture, "{0:F3}", new object[1] { global::Players.Players.firstPersonViewAdjY }) + "\t" + string.Format(CultureInfo.InvariantCulture, "{0:F3}", new object[1] { global::Players.Players.firstPersonViewAdjZ }) + "\r\n";
				stream.Write(mainC.utilMain.String_To_Byte_Array(text), 0, text.Length);
				if (global::Players.Players.lastView != 1 && global::Players.Players.lastView != 0)
				{
					global::Players.Players.lastView = 1;
				}
				text = "CurrentView\t" + global::Players.Players.lastView.ToString(CultureInfo.InvariantCulture) + "\r\n";
				stream.Write(mainC.utilMain.String_To_Byte_Array(text), 0, text.Length);
				text = ((!global::Sounds.Sounds.soundEnabled[0]) ? "SoundEnabled\t0\r\n" : "SoundEnabled\t1\r\n");
				stream.Write(mainC.utilMain.String_To_Byte_Array(text), 0, text.Length);
				text = ((!global::Sounds.Sounds.soundEnabled[1]) ? "MusicEnabled\t0\r\n" : "MusicEnabled\t1\r\n");
				stream.Write(mainC.utilMain.String_To_Byte_Array(text), 0, text.Length);
				text = ((!global::Sounds.Sounds.soundEnabled[2]) ? "VoiceEnabled\t0\r\n" : "VoiceEnabled\t1\r\n");
				stream.Write(mainC.utilMain.String_To_Byte_Array(text), 0, text.Length);
				text = "SoundVolume\t" + string.Format(CultureInfo.InvariantCulture, "{0:F1}", new object[1] { global::Sounds.Sounds.volume[0] }) + "\r\n";
				stream.Write(mainC.utilMain.String_To_Byte_Array(text), 0, text.Length);
				text = "MusicVolume\t" + string.Format(CultureInfo.InvariantCulture, "{0:F1}", new object[1] { global::Sounds.Sounds.volume[1] }) + "\r\n";
				stream.Write(mainC.utilMain.String_To_Byte_Array(text), 0, text.Length);
				text = "VoiceVolume\t" + string.Format(CultureInfo.InvariantCulture, "{0:F1}", new object[1] { global::Sounds.Sounds.volume[2] }) + "\r\n";
				stream.Write(mainC.utilMain.String_To_Byte_Array(text), 0, text.Length);
				text = "RankSp\t" + global::Players.Players.playerRankSP.ToString(CultureInfo.InvariantCulture) + "\r\n";
				stream.Write(mainC.utilMain.String_To_Byte_Array(text), 0, text.Length);
				text = "RankSpPoints\t" + global::Players.Players.playerRankPointsSP.ToString(CultureInfo.InvariantCulture) + "\r\n";
				stream.Write(mainC.utilMain.String_To_Byte_Array(text), 0, text.Length);
				text = "RankMp\t" + global::Players.Players.playerRankMP.ToString(CultureInfo.InvariantCulture) + "\r\n";
				stream.Write(mainC.utilMain.String_To_Byte_Array(text), 0, text.Length);
				text = "RankMpPoints\t" + global::Players.Players.playerRankPointsMP.ToString(CultureInfo.InvariantCulture) + "\r\n";
				stream.Write(mainC.utilMain.String_To_Byte_Array(text), 0, text.Length);
				text = "SP_Vehicle\t" + global::Players.Players.lastSPVehicle.ToString(CultureInfo.InvariantCulture) + "\r\n";
				stream.Write(mainC.utilMain.String_To_Byte_Array(text), 0, text.Length);
				text = "MP_Vehicle\t" + global::Players.Players.lastMPVehicle.ToString(CultureInfo.InvariantCulture) + "\r\n";
				stream.Write(mainC.utilMain.String_To_Byte_Array(text), 0, text.Length);
				text = "SP_Weapon\t" + global::Players.Players.lastSPWeapon.ToString(CultureInfo.InvariantCulture) + "\r\n";
				stream.Write(mainC.utilMain.String_To_Byte_Array(text), 0, text.Length);
				text = "SP_Weapon2\t" + global::Players.Players.lastSPWeapon2.ToString(CultureInfo.InvariantCulture) + "\r\n";
				stream.Write(mainC.utilMain.String_To_Byte_Array(text), 0, text.Length);
				text = "MP_Weapon\t" + global::Players.Players.lastMPWeapon.ToString(CultureInfo.InvariantCulture) + "\r\n";
				stream.Write(mainC.utilMain.String_To_Byte_Array(text), 0, text.Length);
				text = "MP_Weapon2\t" + global::Players.Players.lastMPWeapon2.ToString(CultureInfo.InvariantCulture) + "\r\n";
				stream.Write(mainC.utilMain.String_To_Byte_Array(text), 0, text.Length);
				text = "numWeapons\t" + global::Players.Players.playerPrefsSP[0].numWeapons.ToString(CultureInfo.InvariantCulture) + "\r\n";
				stream.Write(mainC.utilMain.String_To_Byte_Array(text), 0, text.Length);
				ushort numWeapons = global::Players.Players.playerPrefsSP[0].numWeapons;
				for (ushort num = 0; num < numWeapons; num++)
				{
					text = "weaponPreferenceSP\t" + global::Players.Players.playerPrefsSP[0].weapons[num].weaponID.ToString(CultureInfo.InvariantCulture) + "\t" + (byte)0 + "\t" + global::Players.Players.playerPrefsSP[0].weapons[num].scopeID.ToString(CultureInfo.InvariantCulture) + "\r\n";
					stream.Write(mainC.utilMain.String_To_Byte_Array(text), 0, text.Length);
					text = "weaponPreferenceSP\t" + global::Players.Players.playerPrefsSP[0].weapons[num].weaponID.ToString(CultureInfo.InvariantCulture) + "\t" + (byte)1 + "\t" + global::Players.Players.playerPrefsSP[0].weapons[num].foreGripID.ToString(CultureInfo.InvariantCulture) + "\r\n";
					stream.Write(mainC.utilMain.String_To_Byte_Array(text), 0, text.Length);
					text = "weaponPreferenceSP\t" + global::Players.Players.playerPrefsSP[0].weapons[num].weaponID.ToString(CultureInfo.InvariantCulture) + "\t" + (byte)2 + "\t" + global::Players.Players.playerPrefsSP[0].weapons[num].barrelID.ToString(CultureInfo.InvariantCulture) + "\r\n";
					stream.Write(mainC.utilMain.String_To_Byte_Array(text), 0, text.Length);
					text = "weaponPreferenceSP\t" + global::Players.Players.playerPrefsSP[0].weapons[num].weaponID.ToString(CultureInfo.InvariantCulture) + "\t" + (byte)3 + "\t" + global::Players.Players.playerPrefsSP[0].weapons[num].energyDeviceID.ToString(CultureInfo.InvariantCulture) + "\r\n";
					stream.Write(mainC.utilMain.String_To_Byte_Array(text), 0, text.Length);
					text = "weaponPreferenceSP\t" + global::Players.Players.playerPrefsSP[0].weapons[num].weaponID.ToString(CultureInfo.InvariantCulture) + "\t" + (byte)4 + "\t" + global::Players.Players.playerPrefsSP[0].weapons[num].skinID.ToString(CultureInfo.InvariantCulture) + "\r\n";
					stream.Write(mainC.utilMain.String_To_Byte_Array(text), 0, text.Length);
					text = "weaponPreferenceSP\t" + global::Players.Players.playerPrefsSP[0].weapons[num].weaponID.ToString(CultureInfo.InvariantCulture) + "\t" + (byte)5 + "\t" + curTaunt.ToString(CultureInfo.InvariantCulture) + "\r\n";
					stream.Write(mainC.utilMain.String_To_Byte_Array(text), 0, text.Length);
				}
				numWeapons = global::Players.Players.playerPrefsMP[0].numWeapons;
				for (ushort num = 0; num < numWeapons; num++)
				{
					text = "weaponPreferenceMP\t" + global::Players.Players.playerPrefsMP[0].weapons[num].weaponID.ToString(CultureInfo.InvariantCulture) + "\t" + (byte)0 + "\t" + global::Players.Players.playerPrefsMP[0].weapons[num].scopeID.ToString(CultureInfo.InvariantCulture) + "\r\n";
					stream.Write(mainC.utilMain.String_To_Byte_Array(text), 0, text.Length);
					text = "weaponPreferenceMP\t" + global::Players.Players.playerPrefsMP[0].weapons[num].weaponID.ToString(CultureInfo.InvariantCulture) + "\t" + (byte)1 + "\t" + global::Players.Players.playerPrefsMP[0].weapons[num].foreGripID.ToString(CultureInfo.InvariantCulture) + "\r\n";
					stream.Write(mainC.utilMain.String_To_Byte_Array(text), 0, text.Length);
					text = "weaponPreferenceMP\t" + global::Players.Players.playerPrefsMP[0].weapons[num].weaponID.ToString(CultureInfo.InvariantCulture) + "\t" + (byte)2 + "\t" + global::Players.Players.playerPrefsMP[0].weapons[num].barrelID.ToString(CultureInfo.InvariantCulture) + "\r\n";
					stream.Write(mainC.utilMain.String_To_Byte_Array(text), 0, text.Length);
					text = "weaponPreferenceMP\t" + global::Players.Players.playerPrefsMP[0].weapons[num].weaponID.ToString(CultureInfo.InvariantCulture) + "\t" + (byte)3 + "\t" + global::Players.Players.playerPrefsMP[0].weapons[num].energyDeviceID.ToString(CultureInfo.InvariantCulture) + "\r\n";
					stream.Write(mainC.utilMain.String_To_Byte_Array(text), 0, text.Length);
					text = "weaponPreferenceMP\t" + global::Players.Players.playerPrefsMP[0].weapons[num].weaponID.ToString(CultureInfo.InvariantCulture) + "\t" + (byte)4 + "\t" + global::Players.Players.playerPrefsMP[0].weapons[num].skinID.ToString(CultureInfo.InvariantCulture) + "\r\n";
					stream.Write(mainC.utilMain.String_To_Byte_Array(text), 0, text.Length);
					text = "weaponPreferenceMP\t" + global::Players.Players.playerPrefsMP[0].weapons[num].weaponID.ToString(CultureInfo.InvariantCulture) + "\t" + (byte)5 + "\t" + curTaunt.ToString(CultureInfo.InvariantCulture) + "\r\n";
					stream.Write(mainC.utilMain.String_To_Byte_Array(text), 0, text.Length);
				}
				text = "taunt\t" + curTaunt.ToString(CultureInfo.InvariantCulture) + "\r\n";
				stream.Write(mainC.utilMain.String_To_Byte_Array(text), 0, text.Length);
				stream.Close();
			}
			array[0].Dispose();
		}
		catch (Exception)
		{
			try
			{
				if (!array[0].IsDisposed)
				{
					array[0].Dispose();
				}
			}
			catch (Exception)
			{
			}
			return true;
		}
		return true;
	}

	public bool Save_Player_Settings_IsolatedStorage()
	{
		IsolatedStorageFileStream isolatedStorageFileStream = null;
		IsolatedStorageFile userStoreForApplication = IsolatedStorageFile.GetUserStoreForApplication();
		try
		{
			Save_Global_Settings();
			isolatedStorageFileStream = null;
			isolatedStorageFileStream = userStoreForApplication.CreateFile("PlayerData.txt");
			if (isolatedStorageFileStream == null)
			{
				return true;
			}
			needToSavePlayerSettings = false;
			if (isolatedStorageFileStream.CanWrite)
			{
				string text = "3rd_Person\t" + string.Format(CultureInfo.InvariantCulture, "{0:F3}", new object[1] { global::Players.Players.thirdPersonXAdj }) + "\t" + string.Format(CultureInfo.InvariantCulture, "{0:F3}", new object[1] { global::Players.Players.thirdPersonYAdj }) + "\t" + string.Format(CultureInfo.InvariantCulture, "{0:F3}", new object[1] { global::Players.Players.thirdPersonZAdj }) + "\r\n";
				isolatedStorageFileStream.Write(mainC.utilMain.String_To_Byte_Array(text), 0, text.Length);
				text = "invertY\t" + string.Format(CultureInfo.InvariantCulture, "{0:F3}", new object[1] { global::Players.Players.invertY }) + "\r\n";
				isolatedStorageFileStream.Write(mainC.utilMain.String_To_Byte_Array(text), 0, text.Length);
				text = "invertYSecondary\t" + string.Format(CultureInfo.InvariantCulture, "{0:F3}", new object[1] { global::Players.Players.invertYSecondary }) + "\r\n";
				isolatedStorageFileStream.Write(mainC.utilMain.String_To_Byte_Array(text), 0, text.Length);
				text = "lookSensitivity0\t" + string.Format(CultureInfo.InvariantCulture, "{0:F3}", new object[1] { global::InputHandler.InputHandler.lookSensitivity[0] }) + "\r\n";
				isolatedStorageFileStream.Write(mainC.utilMain.String_To_Byte_Array(text), 0, text.Length);
				text = "lookSensitivity1\t" + string.Format(CultureInfo.InvariantCulture, "{0:F3}", new object[1] { global::InputHandler.InputHandler.lookSensitivity[1] }) + "\r\n";
				isolatedStorageFileStream.Write(mainC.utilMain.String_To_Byte_Array(text), 0, text.Length);
				text = "brightness\t" + string.Format(CultureInfo.InvariantCulture, "{0:F3}", new object[1] { global::Rendering.Rendering.brightness }) + "\r\n";
				isolatedStorageFileStream.Write(mainC.utilMain.String_To_Byte_Array(text), 0, text.Length);
				text = "Rumble\t" + global::InputHandler.InputHandler.rumble.ToString(CultureInfo.InvariantCulture) + "\r\n";
				isolatedStorageFileStream.Write(mainC.utilMain.String_To_Byte_Array(text), 0, text.Length);
				text = "SwapSticks\t0\r\n";
				if (global::InputHandler.InputHandler.swapSticks)
				{
					text = "SwapSticks\t1\r\n";
				}
				isolatedStorageFileStream.Write(mainC.utilMain.String_To_Byte_Array(text), 0, text.Length);
				text = "gunView\t" + string.Format(CultureInfo.InvariantCulture, "{0:F3}", new object[1] { global::Players.Players.firstPersonViewAdjX }) + "\t" + string.Format(CultureInfo.InvariantCulture, "{0:F3}", new object[1] { global::Players.Players.firstPersonViewAdjY }) + "\t" + string.Format(CultureInfo.InvariantCulture, "{0:F3}", new object[1] { global::Players.Players.firstPersonViewAdjZ }) + "\r\n";
				isolatedStorageFileStream.Write(mainC.utilMain.String_To_Byte_Array(text), 0, text.Length);
				if (global::Players.Players.lastView != 1 && global::Players.Players.lastView != 0)
				{
					global::Players.Players.lastView = 1;
				}
				text = "CurrentView\t" + global::Players.Players.lastView.ToString(CultureInfo.InvariantCulture) + "\r\n";
				isolatedStorageFileStream.Write(mainC.utilMain.String_To_Byte_Array(text), 0, text.Length);
				text = ((!global::Sounds.Sounds.soundEnabled[0]) ? "SoundEnabled\t0\r\n" : "SoundEnabled\t1\r\n");
				isolatedStorageFileStream.Write(mainC.utilMain.String_To_Byte_Array(text), 0, text.Length);
				text = ((!global::Sounds.Sounds.soundEnabled[1]) ? "MusicEnabled\t0\r\n" : "MusicEnabled\t1\r\n");
				isolatedStorageFileStream.Write(mainC.utilMain.String_To_Byte_Array(text), 0, text.Length);
				text = ((!global::Sounds.Sounds.soundEnabled[2]) ? "VoiceEnabled\t0\r\n" : "VoiceEnabled\t1\r\n");
				isolatedStorageFileStream.Write(mainC.utilMain.String_To_Byte_Array(text), 0, text.Length);
				text = "SoundVolume\t" + string.Format(CultureInfo.InvariantCulture, "{0:F1}", new object[1] { global::Sounds.Sounds.volume[0] }) + "\r\n";
				isolatedStorageFileStream.Write(mainC.utilMain.String_To_Byte_Array(text), 0, text.Length);
				text = "MusicVolume\t" + string.Format(CultureInfo.InvariantCulture, "{0:F1}", new object[1] { global::Sounds.Sounds.volume[1] }) + "\r\n";
				isolatedStorageFileStream.Write(mainC.utilMain.String_To_Byte_Array(text), 0, text.Length);
				text = "VoiceVolume\t" + string.Format(CultureInfo.InvariantCulture, "{0:F1}", new object[1] { global::Sounds.Sounds.volume[2] }) + "\r\n";
				isolatedStorageFileStream.Write(mainC.utilMain.String_To_Byte_Array(text), 0, text.Length);
				text = "RankSp\t" + global::Players.Players.playerRankSP.ToString(CultureInfo.InvariantCulture) + "\r\n";
				isolatedStorageFileStream.Write(mainC.utilMain.String_To_Byte_Array(text), 0, text.Length);
				text = "RankSpPoints\t" + global::Players.Players.playerRankPointsSP.ToString(CultureInfo.InvariantCulture) + "\r\n";
				isolatedStorageFileStream.Write(mainC.utilMain.String_To_Byte_Array(text), 0, text.Length);
				text = "RankMp\t" + global::Players.Players.playerRankMP.ToString(CultureInfo.InvariantCulture) + "\r\n";
				isolatedStorageFileStream.Write(mainC.utilMain.String_To_Byte_Array(text), 0, text.Length);
				text = "RankMpPoints\t" + global::Players.Players.playerRankPointsMP.ToString(CultureInfo.InvariantCulture) + "\r\n";
				isolatedStorageFileStream.Write(mainC.utilMain.String_To_Byte_Array(text), 0, text.Length);
				text = "SP_Vehicle\t" + global::Players.Players.lastSPVehicle.ToString(CultureInfo.InvariantCulture) + "\r\n";
				isolatedStorageFileStream.Write(mainC.utilMain.String_To_Byte_Array(text), 0, text.Length);
				text = "MP_Vehicle\t" + global::Players.Players.lastMPVehicle.ToString(CultureInfo.InvariantCulture) + "\r\n";
				isolatedStorageFileStream.Write(mainC.utilMain.String_To_Byte_Array(text), 0, text.Length);
				text = "SP_Weapon\t" + global::Players.Players.lastSPWeapon.ToString(CultureInfo.InvariantCulture) + "\r\n";
				isolatedStorageFileStream.Write(mainC.utilMain.String_To_Byte_Array(text), 0, text.Length);
				text = "SP_Weapon2\t" + global::Players.Players.lastSPWeapon2.ToString(CultureInfo.InvariantCulture) + "\r\n";
				isolatedStorageFileStream.Write(mainC.utilMain.String_To_Byte_Array(text), 0, text.Length);
				text = "MP_Weapon\t" + global::Players.Players.lastMPWeapon.ToString(CultureInfo.InvariantCulture) + "\r\n";
				isolatedStorageFileStream.Write(mainC.utilMain.String_To_Byte_Array(text), 0, text.Length);
				text = "MP_Weapon2\t" + global::Players.Players.lastMPWeapon2.ToString(CultureInfo.InvariantCulture) + "\r\n";
				isolatedStorageFileStream.Write(mainC.utilMain.String_To_Byte_Array(text), 0, text.Length);
				text = "numWeapons\t" + global::Players.Players.playerPrefsSP[0].numWeapons.ToString(CultureInfo.InvariantCulture) + "\r\n";
				isolatedStorageFileStream.Write(mainC.utilMain.String_To_Byte_Array(text), 0, text.Length);
				ushort numWeapons = global::Players.Players.playerPrefsSP[0].numWeapons;
				for (ushort num = 0; num < numWeapons; num++)
				{
					text = "weaponPreferenceSP\t" + global::Players.Players.playerPrefsSP[0].weapons[num].weaponID.ToString(CultureInfo.InvariantCulture) + "\t" + (byte)0 + "\t" + global::Players.Players.playerPrefsSP[0].weapons[num].scopeID.ToString(CultureInfo.InvariantCulture) + "\r\n";
					isolatedStorageFileStream.Write(mainC.utilMain.String_To_Byte_Array(text), 0, text.Length);
					text = "weaponPreferenceSP\t" + global::Players.Players.playerPrefsSP[0].weapons[num].weaponID.ToString(CultureInfo.InvariantCulture) + "\t" + (byte)1 + "\t" + global::Players.Players.playerPrefsSP[0].weapons[num].foreGripID.ToString(CultureInfo.InvariantCulture) + "\r\n";
					isolatedStorageFileStream.Write(mainC.utilMain.String_To_Byte_Array(text), 0, text.Length);
					text = "weaponPreferenceSP\t" + global::Players.Players.playerPrefsSP[0].weapons[num].weaponID.ToString(CultureInfo.InvariantCulture) + "\t" + (byte)2 + "\t" + global::Players.Players.playerPrefsSP[0].weapons[num].barrelID.ToString(CultureInfo.InvariantCulture) + "\r\n";
					isolatedStorageFileStream.Write(mainC.utilMain.String_To_Byte_Array(text), 0, text.Length);
					text = "weaponPreferenceSP\t" + global::Players.Players.playerPrefsSP[0].weapons[num].weaponID.ToString(CultureInfo.InvariantCulture) + "\t" + (byte)3 + "\t" + global::Players.Players.playerPrefsSP[0].weapons[num].energyDeviceID.ToString(CultureInfo.InvariantCulture) + "\r\n";
					isolatedStorageFileStream.Write(mainC.utilMain.String_To_Byte_Array(text), 0, text.Length);
					text = "weaponPreferenceSP\t" + global::Players.Players.playerPrefsSP[0].weapons[num].weaponID.ToString(CultureInfo.InvariantCulture) + "\t" + (byte)4 + "\t" + global::Players.Players.playerPrefsSP[0].weapons[num].skinID.ToString(CultureInfo.InvariantCulture) + "\r\n";
					isolatedStorageFileStream.Write(mainC.utilMain.String_To_Byte_Array(text), 0, text.Length);
					text = "weaponPreferenceSP\t" + global::Players.Players.playerPrefsSP[0].weapons[num].weaponID.ToString(CultureInfo.InvariantCulture) + "\t" + (byte)5 + "\t" + curTaunt.ToString(CultureInfo.InvariantCulture) + "\r\n";
					isolatedStorageFileStream.Write(mainC.utilMain.String_To_Byte_Array(text), 0, text.Length);
				}
				numWeapons = global::Players.Players.playerPrefsMP[0].numWeapons;
				for (ushort num = 0; num < numWeapons; num++)
				{
					text = "weaponPreferenceMP\t" + global::Players.Players.playerPrefsMP[0].weapons[num].weaponID.ToString(CultureInfo.InvariantCulture) + "\t" + (byte)0 + "\t" + global::Players.Players.playerPrefsMP[0].weapons[num].scopeID.ToString(CultureInfo.InvariantCulture) + "\r\n";
					isolatedStorageFileStream.Write(mainC.utilMain.String_To_Byte_Array(text), 0, text.Length);
					text = "weaponPreferenceMP\t" + global::Players.Players.playerPrefsMP[0].weapons[num].weaponID.ToString(CultureInfo.InvariantCulture) + "\t" + (byte)1 + "\t" + global::Players.Players.playerPrefsMP[0].weapons[num].foreGripID.ToString(CultureInfo.InvariantCulture) + "\r\n";
					isolatedStorageFileStream.Write(mainC.utilMain.String_To_Byte_Array(text), 0, text.Length);
					text = "weaponPreferenceMP\t" + global::Players.Players.playerPrefsMP[0].weapons[num].weaponID.ToString(CultureInfo.InvariantCulture) + "\t" + (byte)2 + "\t" + global::Players.Players.playerPrefsMP[0].weapons[num].barrelID.ToString(CultureInfo.InvariantCulture) + "\r\n";
					isolatedStorageFileStream.Write(mainC.utilMain.String_To_Byte_Array(text), 0, text.Length);
					text = "weaponPreferenceMP\t" + global::Players.Players.playerPrefsMP[0].weapons[num].weaponID.ToString(CultureInfo.InvariantCulture) + "\t" + (byte)3 + "\t" + global::Players.Players.playerPrefsMP[0].weapons[num].energyDeviceID.ToString(CultureInfo.InvariantCulture) + "\r\n";
					isolatedStorageFileStream.Write(mainC.utilMain.String_To_Byte_Array(text), 0, text.Length);
					text = "weaponPreferenceMP\t" + global::Players.Players.playerPrefsMP[0].weapons[num].weaponID.ToString(CultureInfo.InvariantCulture) + "\t" + (byte)4 + "\t" + global::Players.Players.playerPrefsMP[0].weapons[num].skinID.ToString(CultureInfo.InvariantCulture) + "\r\n";
					isolatedStorageFileStream.Write(mainC.utilMain.String_To_Byte_Array(text), 0, text.Length);
					text = "weaponPreferenceMP\t" + global::Players.Players.playerPrefsMP[0].weapons[num].weaponID.ToString(CultureInfo.InvariantCulture) + "\t" + (byte)5 + "\t" + curTaunt.ToString(CultureInfo.InvariantCulture) + "\r\n";
					isolatedStorageFileStream.Write(mainC.utilMain.String_To_Byte_Array(text), 0, text.Length);
				}
				text = "taunt\t" + curTaunt.ToString(CultureInfo.InvariantCulture) + "\r\n";
				isolatedStorageFileStream.Write(mainC.utilMain.String_To_Byte_Array(text), 0, text.Length);
				isolatedStorageFileStream.Close();
			}
		}
		catch (Exception)
		{
			try
			{
				isolatedStorageFileStream.Close();
			}
			catch (Exception)
			{
			}
			return true;
		}
		return true;
	}

	public void Load_Player_Settings()
	{
		if (storageDeviceNotChosen)
		{
			playerSettingsLoaded = true;
			needToLoadPlayerSettings = false;
			return;
		}
		if (signedinGamerID < 0 || Gamer.SignedInGamers[signedinGamerID].IsGuest)
		{
			Load_Player_Settings_IsolatedStorage();
			return;
		}
		StorageContainer[] array = new StorageContainer[1];
		try
		{
			if (deviceGamer == null || !deviceGamer.IsConnected)
			{
				Choose_Gamer_Storage_Container();
				return;
			}
			playerSettingsLoaded = true;
			needToLoadPlayerSettings = false;
			byte b = 0;
			array[0] = Open_Gamer_Container();
			if (!array[0].FileExists("PlayerData.txt"))
			{
				array[0].Dispose();
				return;
			}
			Stream stream = array[0].OpenFile("PlayerData.txt", FileMode.Open);
			byte[] array2 = new byte[stream.Length];
			for (int i = 0; i < 3; i++)
			{
				global::Sounds.Sounds.soundEnabled[i] = true;
			}
			if (stream.CanRead)
			{
				mainC.playersMain.Init_Player_Preferences();
				stream.Read(array2, 0, array2.Length);
				stream.Close();
				string text = mainC.utilMain.Byte_Array_To_String(array2);
				string[] array3 = text.Split('\n', '\r');
				int i = 0;
				int num = 0;
				for (; i < array3.Length; i++)
				{
					if (array3[i].Length > 0)
					{
						num++;
					}
				}
				if (num < 1)
				{
					return;
				}
				string[] array4 = new string[num];
				i = 0;
				num = 0;
				for (; i < array3.Length; i++)
				{
					if (array3[i].Length > 0)
					{
						array4[num++] = array3[i];
					}
				}
				for (i = 0; i < num; i++)
				{
					array3 = array4[i].Split(' ', '\t');
					int j = 0;
					int num2 = 0;
					for (; j < array3.Length; j++)
					{
						if (array3[j].Length > 0)
						{
							num2++;
						}
					}
					if (num2 < 1)
					{
						continue;
					}
					string[] array5 = new string[num2];
					j = 0;
					num2 = 0;
					for (; j < array3.Length; j++)
					{
						if (array3[j].Length > 0)
						{
							array5[num2++] = array3[j];
						}
					}
					int num3 = 0;
					if (array5[0].Equals("3rd_Person", StringComparison.OrdinalIgnoreCase))
					{
						num3 = 1;
					}
					else if (array5[0].Equals("SwapSticks", StringComparison.OrdinalIgnoreCase))
					{
						num3 = 2;
					}
					else if (array5[0].Equals("numWeapons", StringComparison.OrdinalIgnoreCase))
					{
						num3 = 3;
					}
					else if (array5[0].Equals("invertYSecondary", StringComparison.OrdinalIgnoreCase))
					{
						num3 = 4;
					}
					else if (array5[0].Equals("invertY", StringComparison.OrdinalIgnoreCase))
					{
						num3 = 5;
					}
					else if (array5[0].Equals("lookSensitivity0", StringComparison.OrdinalIgnoreCase))
					{
						num3 = 6;
					}
					else if (array5[0].Equals("lookSensitivity1", StringComparison.OrdinalIgnoreCase))
					{
						num3 = 7;
					}
					else if (array5[0].Equals("brightness", StringComparison.OrdinalIgnoreCase))
					{
						num3 = 8;
					}
					else if (array5[0].Equals("SlowSidestep", StringComparison.OrdinalIgnoreCase))
					{
						num3 = 10;
					}
					else if (array5[0].Equals("Rumble", StringComparison.OrdinalIgnoreCase))
					{
						num3 = 11;
					}
					else if (array5[0].Equals("Settings", StringComparison.OrdinalIgnoreCase))
					{
						num3 = 12;
					}
					else if (array5[0].Equals("gunView", StringComparison.OrdinalIgnoreCase))
					{
						num3 = 13;
					}
					else if (array5[0].Equals("CurrentView", StringComparison.OrdinalIgnoreCase))
					{
						num3 = 14;
					}
					else if (array5[0].Equals("SoundVolume", StringComparison.OrdinalIgnoreCase))
					{
						num3 = 15;
					}
					else if (array5[0].Equals("MusicVolume", StringComparison.OrdinalIgnoreCase))
					{
						num3 = 16;
					}
					else if (array5[0].Equals("VoiceVolume", StringComparison.OrdinalIgnoreCase))
					{
						num3 = 17;
					}
					else if (array5[0].Equals("SoundEnabled", StringComparison.OrdinalIgnoreCase))
					{
						num3 = 18;
					}
					else if (array5[0].Equals("MusicEnabled", StringComparison.OrdinalIgnoreCase))
					{
						num3 = 19;
					}
					else if (array5[0].Equals("VoiceEnabled", StringComparison.OrdinalIgnoreCase))
					{
						num3 = 20;
					}
					else if (array5[0].Equals("RankSP", StringComparison.OrdinalIgnoreCase))
					{
						num3 = 21;
					}
					else if (array5[0].Equals("RankSPPoints", StringComparison.OrdinalIgnoreCase))
					{
						num3 = 22;
					}
					else if (array5[0].Equals("RankMP", StringComparison.OrdinalIgnoreCase))
					{
						num3 = 23;
					}
					else if (array5[0].Equals("RankMPPoints", StringComparison.OrdinalIgnoreCase))
					{
						num3 = 24;
					}
					else if (array5[0].Equals("SP_Vehicle", StringComparison.OrdinalIgnoreCase))
					{
						num3 = 25;
					}
					else if (array5[0].Equals("MP_Vehicle", StringComparison.OrdinalIgnoreCase))
					{
						num3 = 26;
					}
					else if (array5[0].Equals("SP_Weapon", StringComparison.OrdinalIgnoreCase))
					{
						num3 = 27;
					}
					else if (array5[0].Equals("MP_Weapon", StringComparison.OrdinalIgnoreCase))
					{
						num3 = 28;
					}
					else if (array5[0].Equals("weaponPreferenceSP", StringComparison.OrdinalIgnoreCase))
					{
						num3 = 29;
					}
					else if (array5[0].Equals("weaponPreferenceMP", StringComparison.OrdinalIgnoreCase))
					{
						num3 = 30;
					}
					else if (array5[0].Equals("taunt", StringComparison.OrdinalIgnoreCase))
					{
						num3 = 31;
					}
					else if (array5[0].Equals("SP_Weapon2", StringComparison.OrdinalIgnoreCase))
					{
						num3 = 32;
					}
					else if (array5[0].Equals("MP_Weapon2", StringComparison.OrdinalIgnoreCase))
					{
						num3 = 33;
					}
					switch (num3)
					{
					case 1:
						if (array5.Length <= 3 || array5[1].Length <= 0)
						{
							break;
						}
						global::Players.Players.thirdPersonXAdj = float.Parse(array5[1], CultureInfo.InvariantCulture.NumberFormat);
						if (array5[2].Length > 0)
						{
							global::Players.Players.thirdPersonYAdj = float.Parse(array5[2], CultureInfo.InvariantCulture.NumberFormat);
							if (array5[3].Length > 0)
							{
								global::Players.Players.thirdPersonZAdj = float.Parse(array5[3], CultureInfo.InvariantCulture.NumberFormat);
							}
						}
						break;
					case 2:
						if (array5.Length > 1)
						{
							byte b2 = byte.Parse(array5[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
							global::InputHandler.InputHandler.swapSticks = false;
							if (b2 == 1)
							{
								global::InputHandler.InputHandler.swapSticks = true;
							}
						}
						break;
					case 3:
						if (array5.Length > 1)
						{
							b = byte.Parse(array5[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
							mainC.weaponsMain.Initialize_Player_Weapon_Preferences(b);
						}
						break;
					case 4:
						if (array5.Length > 1 && array5[1].Length > 0)
						{
							global::Players.Players.invertYSecondary = float.Parse(array5[1], CultureInfo.InvariantCulture.NumberFormat);
						}
						break;
					case 5:
						if (array5.Length > 1 && array5[1].Length > 0)
						{
							global::InputHandler.InputHandler.ySetFromFile = true;
							global::Players.Players.invertY = float.Parse(array5[1], CultureInfo.InvariantCulture.NumberFormat);
						}
						break;
					case 6:
						if (array5.Length > 1 && array5[1].Length > 0)
						{
							global::InputHandler.InputHandler.lookSensitivity[0] = float.Parse(array5[1], CultureInfo.InvariantCulture.NumberFormat);
							if (global::InputHandler.InputHandler.lookSensitivity[0] > 1f)
							{
								global::InputHandler.InputHandler.lookSensitivity[0] = 1f;
							}
							if (global::InputHandler.InputHandler.lookSensitivity[0] < 0.15f)
							{
								global::InputHandler.InputHandler.lookSensitivity[0] = 0.15f;
							}
						}
						break;
					case 7:
						if (array5.Length > 1 && array5[1].Length > 0)
						{
							global::InputHandler.InputHandler.lookSensitivity[1] = float.Parse(array5[1], CultureInfo.InvariantCulture.NumberFormat);
							if (global::InputHandler.InputHandler.lookSensitivity[1] > 1f)
							{
								global::InputHandler.InputHandler.lookSensitivity[1] = 1f;
							}
							if (global::InputHandler.InputHandler.lookSensitivity[1] < 0.15f)
							{
								global::InputHandler.InputHandler.lookSensitivity[1] = 0.15f;
							}
						}
						break;
					case 8:
						if (array5.Length > 1 && array5[1].Length > 0)
						{
							global::Rendering.Rendering.brightness = float.Parse(array5[1], CultureInfo.InvariantCulture.NumberFormat);
							mainC.renderingMain.Set_Brightness();
						}
						break;
					case 10:
						if (array5.Length > 1 && array5[1].Length > 0)
						{
							global::InputHandler.InputHandler.slowSideStep = false;
							if (int.Parse(array5[1], CultureInfo.InvariantCulture.NumberFormat) == 1)
							{
								global::InputHandler.InputHandler.slowSideStep = true;
							}
						}
						break;
					case 11:
						if (array5.Length > 1 && array5[1].Length > 0)
						{
							global::InputHandler.InputHandler.rumble = byte.Parse(array5[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
						}
						break;
					case 12:
						haveSettings = true;
						break;
					case 13:
						if (array5.Length > 3 && array5[1].Length > 0)
						{
							global::Players.Players.firstPersonViewAdjX = 0f;
							global::Players.Players.firstPersonViewAdjY = 0f;
							global::Players.Players.firstPersonViewAdjZ = 0f;
						}
						break;
					case 14:
						if (array5.Length > 1 && array5[1].Length > 0)
						{
							global::Players.Players.lastView = byte.Parse(array5[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
							global::Players.Players.currentView = global::Players.Players.lastView;
						}
						break;
					case 15:
						if (array5.Length > 1 && array5[1].Length > 0)
						{
							global::Sounds.Sounds.volume[0] = float.Parse(array5[1], CultureInfo.InvariantCulture.NumberFormat);
							if (global::Sounds.Sounds.volume[0] < -96f)
							{
								global::Sounds.Sounds.volume[0] = -96f;
							}
							if (global::Sounds.Sounds.volume[0] > 6f)
							{
								global::Sounds.Sounds.volume[0] = 6f;
							}
						}
						break;
					case 16:
						if (array5.Length > 1 && array5[1].Length > 0)
						{
							global::Sounds.Sounds.volume[1] = float.Parse(array5[1], CultureInfo.InvariantCulture.NumberFormat);
							if (global::Sounds.Sounds.volume[1] < -96f)
							{
								global::Sounds.Sounds.volume[1] = -96f;
							}
							if (global::Sounds.Sounds.volume[1] > 6f)
							{
								global::Sounds.Sounds.volume[1] = 6f;
							}
						}
						break;
					case 17:
						if (array5.Length > 1 && array5[1].Length > 0)
						{
							global::Sounds.Sounds.volume[2] = float.Parse(array5[1], CultureInfo.InvariantCulture.NumberFormat);
							if (global::Sounds.Sounds.volume[2] < -96f)
							{
								global::Sounds.Sounds.volume[2] = -96f;
							}
							if (global::Sounds.Sounds.volume[2] > 6f)
							{
								global::Sounds.Sounds.volume[2] = 6f;
							}
						}
						break;
					case 18:
						if (array5.Length > 1 && array5[1].Length > 0)
						{
							global::Sounds.Sounds.soundEnabled[0] = false;
							if (int.Parse(array5[1], CultureInfo.InvariantCulture.NumberFormat) == 1)
							{
								global::Sounds.Sounds.soundEnabled[0] = true;
							}
						}
						break;
					case 19:
						if (array5.Length > 1 && array5[1].Length > 0)
						{
							global::Sounds.Sounds.soundEnabled[1] = false;
							if (int.Parse(array5[1], CultureInfo.InvariantCulture.NumberFormat) == 1)
							{
								global::Sounds.Sounds.soundEnabled[1] = true;
							}
						}
						break;
					case 20:
						if (array5.Length > 1 && array5[1].Length > 0)
						{
							global::Sounds.Sounds.soundEnabled[2] = false;
							if (int.Parse(array5[1], CultureInfo.InvariantCulture.NumberFormat) == 1)
							{
								global::Sounds.Sounds.soundEnabled[2] = true;
							}
						}
						break;
					case 21:
						if (array5.Length > 1)
						{
							global::Players.Players.playerRankSP = byte.Parse(array5[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
						}
						break;
					case 22:
						if (array5.Length > 1)
						{
							global::Players.Players.playerRankPointsSP = int.Parse(array5[1], CultureInfo.InvariantCulture.NumberFormat);
						}
						break;
					case 23:
						if (array5.Length > 1)
						{
							global::Players.Players.playerRankMP = byte.Parse(array5[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
						}
						break;
					case 24:
						if (array5.Length > 1)
						{
							global::Players.Players.playerRankPointsMP = int.Parse(array5[1], CultureInfo.InvariantCulture.NumberFormat);
						}
						break;
					case 25:
						if (array5.Length > 1)
						{
							global::Players.Players.lastSPVehicle = ushort.Parse(array5[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
						}
						break;
					case 26:
						if (array5.Length > 1)
						{
							global::Players.Players.lastMPVehicle = ushort.Parse(array5[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
						}
						break;
					case 27:
						if (array5.Length > 1)
						{
							global::Players.Players.lastSPWeapon = ushort.Parse(array5[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
						}
						break;
					case 28:
						if (array5.Length > 1)
						{
							global::Players.Players.lastMPWeapon = ushort.Parse(array5[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
						}
						break;
					case 29:
						if (array5.Length > 3 && b > 0)
						{
							mainC.weaponsMain.Set_Weapon_Preference(0, 0, byte.Parse(array5[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat), byte.Parse(array5[2], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat), byte.Parse(array5[3], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat));
						}
						break;
					case 30:
						if (array5.Length > 3 && b > 0)
						{
							mainC.weaponsMain.Set_Weapon_Preference(1, 0, byte.Parse(array5[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat), byte.Parse(array5[2], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat), byte.Parse(array5[3], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat));
						}
						break;
					case 31:
						if (array5.Length > 1)
						{
							curTaunt = byte.Parse(array5[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
						}
						break;
					case 32:
						if (array5.Length > 1)
						{
							global::Players.Players.lastSPWeapon2 = ushort.Parse(array5[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
						}
						break;
					case 33:
						if (array5.Length > 1)
						{
							global::Players.Players.lastMPWeapon2 = ushort.Parse(array5[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
						}
						break;
					}
				}
			}
			mainC.soundsMain.Update_Sound_Settings(0);
			mainC.soundsMain.Update_Sound_Settings(1);
			mainC.soundsMain.Update_Sound_Settings(2);
			mainC.soundsMain.Set_Music_Volume();
			mainC.gameLogic.Game_UI_Update_Options_Window(0);
			array[0].Dispose();
		}
		catch (Exception)
		{
			try
			{
				if (!array[0].IsDisposed)
				{
					array[0].Dispose();
				}
			}
			catch (Exception)
			{
			}
		}
	}

	public void Load_Player_Settings_IsolatedStorage()
	{
		IsolatedStorageFileStream isolatedStorageFileStream = null;
		IsolatedStorageFile userStoreForApplication = IsolatedStorageFile.GetUserStoreForApplication();
		try
		{
			playerSettingsLoaded = true;
			needToLoadPlayerSettings = false;
			byte b = 0;
			isolatedStorageFileStream = null;
			isolatedStorageFileStream = userStoreForApplication.OpenFile("PlayerData.txt", FileMode.OpenOrCreate);
			if (isolatedStorageFileStream == null)
			{
				return;
			}
			byte[] array = new byte[isolatedStorageFileStream.Length];
			for (int i = 0; i < 3; i++)
			{
				global::Sounds.Sounds.soundEnabled[i] = true;
			}
			if (isolatedStorageFileStream.CanRead)
			{
				mainC.playersMain.Init_Player_Preferences();
				isolatedStorageFileStream.Read(array, 0, array.Length);
				isolatedStorageFileStream.Close();
				string text = mainC.utilMain.Byte_Array_To_String(array);
				string[] array2 = text.Split('\n', '\r');
				int i = 0;
				int num = 0;
				for (; i < array2.Length; i++)
				{
					if (array2[i].Length > 0)
					{
						num++;
					}
				}
				if (num < 1)
				{
					return;
				}
				string[] array3 = new string[num];
				i = 0;
				num = 0;
				for (; i < array2.Length; i++)
				{
					if (array2[i].Length > 0)
					{
						array3[num++] = array2[i];
					}
				}
				for (i = 0; i < num; i++)
				{
					array2 = array3[i].Split(' ', '\t');
					int j = 0;
					int num2 = 0;
					for (; j < array2.Length; j++)
					{
						if (array2[j].Length > 0)
						{
							num2++;
						}
					}
					if (num2 < 1)
					{
						continue;
					}
					string[] array4 = new string[num2];
					j = 0;
					num2 = 0;
					for (; j < array2.Length; j++)
					{
						if (array2[j].Length > 0)
						{
							array4[num2++] = array2[j];
						}
					}
					int num3 = 0;
					if (array4[0].Equals("3rd_Person", StringComparison.OrdinalIgnoreCase))
					{
						num3 = 1;
					}
					else if (array4[0].Equals("SwapSticks", StringComparison.OrdinalIgnoreCase))
					{
						num3 = 2;
					}
					else if (array4[0].Equals("numWeapons", StringComparison.OrdinalIgnoreCase))
					{
						num3 = 3;
					}
					else if (array4[0].Equals("invertYSecondary", StringComparison.OrdinalIgnoreCase))
					{
						num3 = 4;
					}
					else if (array4[0].Equals("invertY", StringComparison.OrdinalIgnoreCase))
					{
						num3 = 5;
					}
					else if (array4[0].Equals("lookSensitivity0", StringComparison.OrdinalIgnoreCase))
					{
						num3 = 6;
					}
					else if (array4[0].Equals("lookSensitivity1", StringComparison.OrdinalIgnoreCase))
					{
						num3 = 7;
					}
					else if (array4[0].Equals("brightness", StringComparison.OrdinalIgnoreCase))
					{
						num3 = 8;
					}
					else if (array4[0].Equals("SlowSidestep", StringComparison.OrdinalIgnoreCase))
					{
						num3 = 10;
					}
					else if (array4[0].Equals("Rumble", StringComparison.OrdinalIgnoreCase))
					{
						num3 = 11;
					}
					else if (array4[0].Equals("Settings", StringComparison.OrdinalIgnoreCase))
					{
						num3 = 12;
					}
					else if (array4[0].Equals("gunView", StringComparison.OrdinalIgnoreCase))
					{
						num3 = 13;
					}
					else if (array4[0].Equals("CurrentView", StringComparison.OrdinalIgnoreCase))
					{
						num3 = 14;
					}
					else if (array4[0].Equals("SoundVolume", StringComparison.OrdinalIgnoreCase))
					{
						num3 = 15;
					}
					else if (array4[0].Equals("MusicVolume", StringComparison.OrdinalIgnoreCase))
					{
						num3 = 16;
					}
					else if (array4[0].Equals("VoiceVolume", StringComparison.OrdinalIgnoreCase))
					{
						num3 = 17;
					}
					else if (array4[0].Equals("SoundEnabled", StringComparison.OrdinalIgnoreCase))
					{
						num3 = 18;
					}
					else if (array4[0].Equals("MusicEnabled", StringComparison.OrdinalIgnoreCase))
					{
						num3 = 19;
					}
					else if (array4[0].Equals("VoiceEnabled", StringComparison.OrdinalIgnoreCase))
					{
						num3 = 20;
					}
					else if (array4[0].Equals("RankSP", StringComparison.OrdinalIgnoreCase))
					{
						num3 = 21;
					}
					else if (array4[0].Equals("RankSPPoints", StringComparison.OrdinalIgnoreCase))
					{
						num3 = 22;
					}
					else if (array4[0].Equals("RankMP", StringComparison.OrdinalIgnoreCase))
					{
						num3 = 23;
					}
					else if (array4[0].Equals("RankMPPoints", StringComparison.OrdinalIgnoreCase))
					{
						num3 = 24;
					}
					else if (array4[0].Equals("SP_Vehicle", StringComparison.OrdinalIgnoreCase))
					{
						num3 = 25;
					}
					else if (array4[0].Equals("MP_Vehicle", StringComparison.OrdinalIgnoreCase))
					{
						num3 = 26;
					}
					else if (array4[0].Equals("SP_Weapon", StringComparison.OrdinalIgnoreCase))
					{
						num3 = 27;
					}
					else if (array4[0].Equals("MP_Weapon", StringComparison.OrdinalIgnoreCase))
					{
						num3 = 28;
					}
					else if (array4[0].Equals("weaponPreferenceSP", StringComparison.OrdinalIgnoreCase))
					{
						num3 = 29;
					}
					else if (array4[0].Equals("weaponPreferenceMP", StringComparison.OrdinalIgnoreCase))
					{
						num3 = 30;
					}
					else if (array4[0].Equals("taunt", StringComparison.OrdinalIgnoreCase))
					{
						num3 = 31;
					}
					else if (array4[0].Equals("SP_Weapon2", StringComparison.OrdinalIgnoreCase))
					{
						num3 = 32;
					}
					else if (array4[0].Equals("MP_Weapon2", StringComparison.OrdinalIgnoreCase))
					{
						num3 = 33;
					}
					switch (num3)
					{
					case 1:
						if (array4.Length <= 3 || array4[1].Length <= 0)
						{
							break;
						}
						global::Players.Players.thirdPersonXAdj = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
						if (array4[2].Length > 0)
						{
							global::Players.Players.thirdPersonYAdj = float.Parse(array4[2], CultureInfo.InvariantCulture.NumberFormat);
							if (array4[3].Length > 0)
							{
								global::Players.Players.thirdPersonZAdj = float.Parse(array4[3], CultureInfo.InvariantCulture.NumberFormat);
							}
						}
						break;
					case 2:
						if (array4.Length > 1)
						{
							byte b2 = byte.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
							global::InputHandler.InputHandler.swapSticks = false;
							if (b2 == 1)
							{
								global::InputHandler.InputHandler.swapSticks = true;
							}
						}
						break;
					case 3:
						if (array4.Length > 1)
						{
							b = byte.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
							mainC.weaponsMain.Initialize_Player_Weapon_Preferences(b);
						}
						break;
					case 4:
						if (array4.Length > 1 && array4[1].Length > 0)
						{
							global::Players.Players.invertYSecondary = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
						}
						break;
					case 5:
						if (array4.Length > 1 && array4[1].Length > 0)
						{
							global::InputHandler.InputHandler.ySetFromFile = true;
							global::Players.Players.invertY = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
						}
						break;
					case 6:
						if (array4.Length > 1 && array4[1].Length > 0)
						{
							global::InputHandler.InputHandler.lookSensitivity[0] = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
							if (global::InputHandler.InputHandler.lookSensitivity[0] > 1f)
							{
								global::InputHandler.InputHandler.lookSensitivity[0] = 1f;
							}
							if (global::InputHandler.InputHandler.lookSensitivity[0] < 0.15f)
							{
								global::InputHandler.InputHandler.lookSensitivity[0] = 0.15f;
							}
						}
						break;
					case 7:
						if (array4.Length > 1 && array4[1].Length > 0)
						{
							global::InputHandler.InputHandler.lookSensitivity[1] = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
							if (global::InputHandler.InputHandler.lookSensitivity[1] > 1f)
							{
								global::InputHandler.InputHandler.lookSensitivity[1] = 1f;
							}
							if (global::InputHandler.InputHandler.lookSensitivity[1] < 0.15f)
							{
								global::InputHandler.InputHandler.lookSensitivity[1] = 0.15f;
							}
						}
						break;
					case 8:
						if (array4.Length > 1 && array4[1].Length > 0)
						{
							global::Rendering.Rendering.brightness = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
							mainC.renderingMain.Set_Brightness();
						}
						break;
					case 10:
						if (array4.Length > 1 && array4[1].Length > 0)
						{
							global::InputHandler.InputHandler.slowSideStep = false;
							if (int.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat) == 1)
							{
								global::InputHandler.InputHandler.slowSideStep = true;
							}
						}
						break;
					case 11:
						if (array4.Length > 1 && array4[1].Length > 0)
						{
							global::InputHandler.InputHandler.rumble = byte.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
						}
						break;
					case 12:
						haveSettings = true;
						break;
					case 13:
						if (array4.Length > 3 && array4[1].Length > 0)
						{
							global::Players.Players.firstPersonViewAdjX = 0f;
							global::Players.Players.firstPersonViewAdjY = 0f;
							global::Players.Players.firstPersonViewAdjZ = 0f;
						}
						break;
					case 14:
						if (array4.Length > 1 && array4[1].Length > 0)
						{
							global::Players.Players.lastView = byte.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
							global::Players.Players.currentView = global::Players.Players.lastView;
						}
						break;
					case 15:
						if (array4.Length > 1 && array4[1].Length > 0)
						{
							global::Sounds.Sounds.volume[0] = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
							if (global::Sounds.Sounds.volume[0] < -96f)
							{
								global::Sounds.Sounds.volume[0] = -96f;
							}
							if (global::Sounds.Sounds.volume[0] > 6f)
							{
								global::Sounds.Sounds.volume[0] = 6f;
							}
						}
						break;
					case 16:
						if (array4.Length > 1 && array4[1].Length > 0)
						{
							global::Sounds.Sounds.volume[1] = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
							if (global::Sounds.Sounds.volume[1] < -96f)
							{
								global::Sounds.Sounds.volume[1] = -96f;
							}
							if (global::Sounds.Sounds.volume[1] > 6f)
							{
								global::Sounds.Sounds.volume[1] = 6f;
							}
						}
						break;
					case 17:
						if (array4.Length > 1 && array4[1].Length > 0)
						{
							global::Sounds.Sounds.volume[2] = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
							if (global::Sounds.Sounds.volume[2] < -96f)
							{
								global::Sounds.Sounds.volume[2] = -96f;
							}
							if (global::Sounds.Sounds.volume[2] > 6f)
							{
								global::Sounds.Sounds.volume[2] = 6f;
							}
						}
						break;
					case 18:
						if (array4.Length > 1 && array4[1].Length > 0)
						{
							global::Sounds.Sounds.soundEnabled[0] = false;
							if (int.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat) == 1)
							{
								global::Sounds.Sounds.soundEnabled[0] = true;
							}
						}
						break;
					case 19:
						if (array4.Length > 1 && array4[1].Length > 0)
						{
							global::Sounds.Sounds.soundEnabled[1] = false;
							if (int.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat) == 1)
							{
								global::Sounds.Sounds.soundEnabled[1] = true;
							}
						}
						break;
					case 20:
						if (array4.Length > 1 && array4[1].Length > 0)
						{
							global::Sounds.Sounds.soundEnabled[2] = false;
							if (int.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat) == 1)
							{
								global::Sounds.Sounds.soundEnabled[2] = true;
							}
						}
						break;
					case 21:
						if (array4.Length > 1)
						{
							global::Players.Players.playerRankSP = byte.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
						}
						break;
					case 22:
						if (array4.Length > 1)
						{
							global::Players.Players.playerRankPointsSP = int.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
						}
						break;
					case 23:
						if (array4.Length > 1)
						{
							global::Players.Players.playerRankMP = byte.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
						}
						break;
					case 24:
						if (array4.Length > 1)
						{
							global::Players.Players.playerRankPointsMP = int.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
						}
						break;
					case 25:
						if (array4.Length > 1)
						{
							global::Players.Players.lastSPVehicle = ushort.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
						}
						break;
					case 26:
						if (array4.Length > 1)
						{
							global::Players.Players.lastMPVehicle = ushort.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
						}
						break;
					case 27:
						if (array4.Length > 1)
						{
							global::Players.Players.lastSPWeapon = ushort.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
						}
						break;
					case 28:
						if (array4.Length > 1)
						{
							global::Players.Players.lastMPWeapon = ushort.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
						}
						break;
					case 29:
						if (array4.Length > 3 && b > 0)
						{
							mainC.weaponsMain.Set_Weapon_Preference(0, 0, byte.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat), byte.Parse(array4[2], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat), byte.Parse(array4[3], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat));
						}
						break;
					case 30:
						if (array4.Length > 3 && b > 0)
						{
							mainC.weaponsMain.Set_Weapon_Preference(1, 0, byte.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat), byte.Parse(array4[2], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat), byte.Parse(array4[3], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat));
						}
						break;
					case 31:
						if (array4.Length > 1)
						{
							curTaunt = byte.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
						}
						break;
					case 32:
						if (array4.Length > 1)
						{
							global::Players.Players.lastSPWeapon2 = ushort.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
						}
						break;
					case 33:
						if (array4.Length > 1)
						{
							global::Players.Players.lastMPWeapon2 = ushort.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
						}
						break;
					}
				}
			}
			mainC.soundsMain.Update_Sound_Settings(0);
			mainC.soundsMain.Update_Sound_Settings(1);
			mainC.soundsMain.Update_Sound_Settings(2);
			mainC.soundsMain.Set_Music_Volume();
			mainC.gameLogic.Game_UI_Update_Options_Window(0);
		}
		catch (Exception)
		{
		}
	}

	public bool Load_SP_Game()
	{
		needToLoadWeapons = true;
		if (storageDeviceNotChosen || signedinGamerID < 0 || Gamer.SignedInGamers[signedinGamerID].IsGuest)
		{
			return Load_SP_Game_IsolatedStorage();
		}
		StorageContainer[] array = new StorageContainer[1];
		try
		{
			if (deviceGamer == null || !deviceGamer.IsConnected)
			{
				Choose_Gamer_Storage_Container();
				return false;
			}
			gameLevel = 0;
			array[0] = Open_Gamer_Container();
			if (!array[0].FileExists("GameSave.txt"))
			{
				array[0].Dispose();
				return true;
			}
			Stream stream = array[0].OpenFile("GameSave.txt", FileMode.Open);
			byte[] array2 = new byte[stream.Length];
			if (stream.CanRead)
			{
				stream.Read(array2, 0, array2.Length);
				stream.Close();
				string text = mainC.utilMain.Byte_Array_To_String(array2);
				string[] array3 = text.Split('\n', '\r');
				int num = array3.Length;
				int i = 0;
				int num2 = 0;
				for (; i < num; i++)
				{
					if (array3[i].Length > 0)
					{
						num2++;
					}
				}
				if (num2 < 1)
				{
					return true;
				}
				string[] array4 = new string[num2];
				i = 0;
				num2 = 0;
				for (; i < num; i++)
				{
					if (array3[i].Length > 0)
					{
						array4[num2++] = array3[i];
					}
				}
				for (i = 0; i < num2; i++)
				{
					array3 = array4[i].Split(' ', '\t');
					int j = 0;
					int num3 = 0;
					for (; j < array3.Length; j++)
					{
						if (array3[j].Length > 0)
						{
							num3++;
						}
					}
					if (num3 < 1)
					{
						continue;
					}
					string[] array5 = new string[num3];
					j = 0;
					num3 = 0;
					for (; j < array3.Length; j++)
					{
						if (array3[j].Length > 0)
						{
							array5[num3++] = array3[j];
						}
					}
					int num4 = 0;
					if (array5[0].Equals("Level", StringComparison.OrdinalIgnoreCase))
					{
						num4 = 1;
					}
					else if (array5[0].Equals("Damage", StringComparison.OrdinalIgnoreCase))
					{
						num4 = 2;
					}
					else if (array5[0].Equals("Lives", StringComparison.OrdinalIgnoreCase))
					{
						num4 = 3;
					}
					else if (array5[0].Equals("AmmoClip", StringComparison.OrdinalIgnoreCase))
					{
						num4 = 4;
					}
					else if (array5[0].Equals("LevelHigh", StringComparison.OrdinalIgnoreCase))
					{
						num4 = 5;
					}
					else if (array5[0].Equals("AIKills", StringComparison.OrdinalIgnoreCase))
					{
						num4 = 6;
					}
					else if (array5[0].Equals("MountedWeapon", StringComparison.OrdinalIgnoreCase))
					{
						num4 = 7;
					}
					else if (array5[0].Equals("Weapon", StringComparison.OrdinalIgnoreCase))
					{
						num4 = 8;
					}
					switch (num4)
					{
					case 1:
						if (array5.Length > 1)
						{
							gameLevel = byte.Parse(array5[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
							if (trialMode && gameLevel > 2)
							{
								gameLevel = 2;
							}
						}
						break;
					case 2:
						if (array5.Length > 1)
						{
							global::Players.Players.players[0].damage = float.Parse(array5[1], CultureInfo.InvariantCulture.NumberFormat);
							mainC.playersMain.Adjust_Player_Damage_To_Fixed_Amount(0, global::Players.Players.players[0].damage, sendOnline: false);
						}
						break;
					case 3:
						if (array5.Length > 1)
						{
							numLives = byte.Parse(array5[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
						}
						break;
					case 4:
						if (array5.Length > 3)
						{
							int num5 = int.Parse(array5[1], CultureInfo.InvariantCulture.NumberFormat);
							if (num5 > -1 && num5 < global::Weapons.Weapons.curAmmoClips)
							{
								global::Players.Players.players[0].ammoClips[num5].numClips = ushort.Parse(array5[2], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
								global::Players.Players.players[0].ammoClips[num5].surplus = ushort.Parse(array5[3], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
							}
						}
						break;
					case 5:
						if (array5.Length > 1)
						{
							byte b = byte.Parse(array5[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
							if (b > highestLevel)
							{
								highestLevel = b;
							}
						}
						break;
					case 6:
						if (array5.Length > 1)
						{
							global::AI.AI.levelKillCount = ushort.Parse(array5[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
						}
						break;
					case 7:
						if (array5.Length > 2)
						{
							byte b = byte.Parse(array5[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
							playerVehicles[0].mounts[b].objectID = byte.Parse(array5[2], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
							playerVehicles[0].mounts[b].objectAttached = 1;
						}
						break;
					case 8:
						if (array5.Length > 3)
						{
							needToLoadWeapons = false;
							byte b = byte.Parse(array5[2], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
							playerVehicles[0].weapons[b].weaponID = byte.Parse(array5[3], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
							playerVehicles[0].weapons[b].active = byte.Parse(array5[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat) == 1;
						}
						break;
					}
				}
			}
			array[0].Dispose();
			if (!needToLoadWeapons)
			{
				global::Players.Players.players[0].primaryWeaponMountWeapon = (sbyte)playerVehicles[0].weapons[playerVehicles[0].mounts[primaryWeaponMount].objectID].weaponID;
			}
		}
		catch (Exception)
		{
			try
			{
				if (!array[0].IsDisposed)
				{
					array[0].Dispose();
				}
			}
			catch (Exception)
			{
			}
		}
		return true;
	}

	public bool Load_SP_Game_IsolatedStorage()
	{
		IsolatedStorageFileStream isolatedStorageFileStream = null;
		IsolatedStorageFile userStoreForApplication = IsolatedStorageFile.GetUserStoreForApplication();
		try
		{
			gameLevel = 0;
			isolatedStorageFileStream = null;
			isolatedStorageFileStream = userStoreForApplication.OpenFile("GameSave.txt", FileMode.OpenOrCreate);
			if (isolatedStorageFileStream == null)
			{
				return true;
			}
			byte[] array = new byte[isolatedStorageFileStream.Length];
			if (isolatedStorageFileStream.CanRead)
			{
				isolatedStorageFileStream.Read(array, 0, array.Length);
				isolatedStorageFileStream.Close();
				string text = mainC.utilMain.Byte_Array_To_String(array);
				string[] array2 = text.Split('\n', '\r');
				int num = array2.Length;
				int i = 0;
				int num2 = 0;
				for (; i < num; i++)
				{
					if (array2[i].Length > 0)
					{
						num2++;
					}
				}
				if (num2 < 1)
				{
					return true;
				}
				string[] array3 = new string[num2];
				i = 0;
				num2 = 0;
				for (; i < num; i++)
				{
					if (array2[i].Length > 0)
					{
						array3[num2++] = array2[i];
					}
				}
				for (i = 0; i < num2; i++)
				{
					array2 = array3[i].Split(' ', '\t');
					int j = 0;
					int num3 = 0;
					for (; j < array2.Length; j++)
					{
						if (array2[j].Length > 0)
						{
							num3++;
						}
					}
					if (num3 < 1)
					{
						continue;
					}
					string[] array4 = new string[num3];
					j = 0;
					num3 = 0;
					for (; j < array2.Length; j++)
					{
						if (array2[j].Length > 0)
						{
							array4[num3++] = array2[j];
						}
					}
					int num4 = 0;
					if (array4[0].Equals("Level", StringComparison.OrdinalIgnoreCase))
					{
						num4 = 1;
					}
					else if (array4[0].Equals("Damage", StringComparison.OrdinalIgnoreCase))
					{
						num4 = 2;
					}
					else if (array4[0].Equals("Lives", StringComparison.OrdinalIgnoreCase))
					{
						num4 = 3;
					}
					else if (array4[0].Equals("AmmoClip", StringComparison.OrdinalIgnoreCase))
					{
						num4 = 4;
					}
					else if (array4[0].Equals("LevelHigh", StringComparison.OrdinalIgnoreCase))
					{
						num4 = 5;
					}
					else if (array4[0].Equals("AIKills", StringComparison.OrdinalIgnoreCase))
					{
						num4 = 6;
					}
					else if (array4[0].Equals("MountedWeapon", StringComparison.OrdinalIgnoreCase))
					{
						num4 = 7;
					}
					else if (array4[0].Equals("Weapon", StringComparison.OrdinalIgnoreCase))
					{
						num4 = 8;
					}
					switch (num4)
					{
					case 1:
						if (array4.Length > 1)
						{
							gameLevel = byte.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
							if (trialMode && gameLevel > 2)
							{
								gameLevel = 2;
							}
						}
						break;
					case 2:
						if (array4.Length > 1)
						{
							global::Players.Players.players[0].damage = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
							mainC.playersMain.Adjust_Player_Damage_To_Fixed_Amount(0, global::Players.Players.players[0].damage, sendOnline: false);
						}
						break;
					case 3:
						if (array4.Length > 1)
						{
							numLives = byte.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
						}
						break;
					case 4:
						if (array4.Length > 3)
						{
							int num5 = int.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
							if (num5 > -1 && num5 < global::Weapons.Weapons.curAmmoClips)
							{
								global::Players.Players.players[0].ammoClips[num5].numClips = ushort.Parse(array4[2], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
								global::Players.Players.players[0].ammoClips[num5].surplus = ushort.Parse(array4[3], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
							}
						}
						break;
					case 5:
						if (array4.Length > 1)
						{
							byte b = byte.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
							if (b > highestLevel)
							{
								highestLevel = b;
							}
						}
						break;
					case 6:
						if (array4.Length > 1)
						{
							global::AI.AI.levelKillCount = ushort.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
						}
						break;
					case 7:
						if (array4.Length > 2)
						{
							byte b = byte.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
							playerVehicles[0].mounts[b].objectID = byte.Parse(array4[2], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
							playerVehicles[0].mounts[b].objectAttached = 1;
						}
						break;
					case 8:
						if (array4.Length > 3)
						{
							needToLoadWeapons = false;
							byte b = byte.Parse(array4[2], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
							playerVehicles[0].weapons[b].weaponID = byte.Parse(array4[3], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
							playerVehicles[0].weapons[b].active = byte.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat) == 1;
						}
						break;
					}
				}
			}
		}
		catch (Exception)
		{
		}
		return true;
	}

	public void Save_SP_Game(byte numLivesToSave)
	{
		if (storageDeviceNotChosen || signedinGamerID < 0 || Gamer.SignedInGamers[signedinGamerID].IsGuest)
		{
			Save_SP_Game_IsolatedStorage(numLivesToSave);
			return;
		}
		StorageContainer[] array = new StorageContainer[1];
		try
		{
			if (deviceGamer == null || !deviceGamer.IsConnected)
			{
				Choose_Gamer_Storage_Container();
				return;
			}
			array[0] = Open_Gamer_Container();
			Stream stream = array[0].OpenFile("GameSave.txt", FileMode.Create);
			if (stream.CanWrite)
			{
				string text = "Level\t" + gameLevel.ToString(CultureInfo.InvariantCulture) + "\r\n";
				stream.Write(mainC.utilMain.String_To_Byte_Array(text), 0, text.Length);
				if (gameLevel > highestLevel)
				{
					highestLevel = gameLevel;
				}
				text = "LevelHigh\t" + highestLevel.ToString(CultureInfo.InvariantCulture) + "\r\n";
				stream.Write(mainC.utilMain.String_To_Byte_Array(text), 0, text.Length);
				text = "Damage\t" + string.Format(CultureInfo.InvariantCulture, "{0:F3}", new object[1] { global::Players.Players.players[0].damage }) + "\r\n";
				stream.Write(mainC.utilMain.String_To_Byte_Array(text), 0, text.Length);
				text = "Lives\t" + numLivesToSave.ToString(CultureInfo.InvariantCulture) + "\r\n";
				stream.Write(mainC.utilMain.String_To_Byte_Array(text), 0, text.Length);
				text = "AIKills\t" + global::AI.AI.levelKillCount.ToString(CultureInfo.InvariantCulture) + "\r\n";
				stream.Write(mainC.utilMain.String_To_Byte_Array(text), 0, text.Length);
				for (ushort num = 0; num < global::Weapons.Weapons.curAmmoClips; num++)
				{
					text = "AmmoClip\t" + num.ToString(CultureInfo.InvariantCulture) + "\t" + global::Players.Players.players[0].ammoClips[num].numClips.ToString(CultureInfo.InvariantCulture) + "\t" + global::Players.Players.players[0].ammoClips[num].surplus.ToString(CultureInfo.InvariantCulture) + "\r\n";
					stream.Write(mainC.utilMain.String_To_Byte_Array(text), 0, text.Length);
				}
				if (saveSpWeapons)
				{
					for (ushort num = 0; num < playerVehicles[0].numMounts; num++)
					{
						if (playerVehicles[0].mounts[num].type == 1 && playerVehicles[0].mounts[num].objectAttached == 1)
						{
							text = "MountedWeapon\t" + num.ToString(CultureInfo.InvariantCulture) + "\t" + playerVehicles[0].mounts[num].objectID.ToString(CultureInfo.InvariantCulture) + "\r\n";
							stream.Write(mainC.utilMain.String_To_Byte_Array(text), 0, text.Length);
						}
					}
					for (ushort num = 0; num < playerVehicles[0].numWeapons; num++)
					{
						byte b = (byte)(playerVehicles[0].weapons[num].active ? 1u : 0u);
						text = "Weapon\t" + b.ToString(CultureInfo.InvariantCulture) + "\t" + num.ToString(CultureInfo.InvariantCulture) + "\t" + playerVehicles[0].weapons[num].weaponID.ToString(CultureInfo.InvariantCulture) + "\r\n";
						stream.Write(mainC.utilMain.String_To_Byte_Array(text), 0, text.Length);
					}
				}
				stream.Close();
			}
			array[0].Dispose();
			spSaving++;
		}
		catch (Exception)
		{
			spSaving++;
			try
			{
				if (!array[0].IsDisposed)
				{
					array[0].Dispose();
				}
			}
			catch (Exception)
			{
			}
		}
	}

	public void Save_SP_Game_IsolatedStorage(byte numLivesToSave)
	{
		IsolatedStorageFileStream isolatedStorageFileStream = null;
		IsolatedStorageFile userStoreForApplication = IsolatedStorageFile.GetUserStoreForApplication();
		try
		{
			isolatedStorageFileStream = null;
			isolatedStorageFileStream = userStoreForApplication.CreateFile("GameSave.txt");
			if (isolatedStorageFileStream == null)
			{
				spSaving++;
				return;
			}
			if (isolatedStorageFileStream.CanWrite)
			{
				string text = "Level\t" + gameLevel.ToString(CultureInfo.InvariantCulture) + "\r\n";
				isolatedStorageFileStream.Write(mainC.utilMain.String_To_Byte_Array(text), 0, text.Length);
				if (gameLevel > highestLevel)
				{
					highestLevel = gameLevel;
				}
				text = "LevelHigh\t" + highestLevel.ToString(CultureInfo.InvariantCulture) + "\r\n";
				isolatedStorageFileStream.Write(mainC.utilMain.String_To_Byte_Array(text), 0, text.Length);
				text = "Damage\t" + string.Format(CultureInfo.InvariantCulture, "{0:F3}", new object[1] { global::Players.Players.players[0].damage }) + "\r\n";
				isolatedStorageFileStream.Write(mainC.utilMain.String_To_Byte_Array(text), 0, text.Length);
				text = "Lives\t" + numLivesToSave.ToString(CultureInfo.InvariantCulture) + "\r\n";
				isolatedStorageFileStream.Write(mainC.utilMain.String_To_Byte_Array(text), 0, text.Length);
				text = "AIKills\t" + global::AI.AI.levelKillCount.ToString(CultureInfo.InvariantCulture) + "\r\n";
				isolatedStorageFileStream.Write(mainC.utilMain.String_To_Byte_Array(text), 0, text.Length);
				for (ushort num = 0; num < global::Weapons.Weapons.curAmmoClips; num++)
				{
					text = "AmmoClip\t" + num.ToString(CultureInfo.InvariantCulture) + "\t" + global::Players.Players.players[0].ammoClips[num].numClips.ToString(CultureInfo.InvariantCulture) + "\t" + global::Players.Players.players[0].ammoClips[num].surplus.ToString(CultureInfo.InvariantCulture) + "\r\n";
					isolatedStorageFileStream.Write(mainC.utilMain.String_To_Byte_Array(text), 0, text.Length);
				}
				if (saveSpWeapons)
				{
					for (ushort num = 0; num < playerVehicles[0].numMounts; num++)
					{
						if (playerVehicles[0].mounts[num].type == 1 && playerVehicles[0].mounts[num].objectAttached == 1)
						{
							text = "MountedWeapon\t" + num.ToString(CultureInfo.InvariantCulture) + "\t" + playerVehicles[0].mounts[num].objectID.ToString(CultureInfo.InvariantCulture) + "\r\n";
							isolatedStorageFileStream.Write(mainC.utilMain.String_To_Byte_Array(text), 0, text.Length);
						}
					}
					for (ushort num = 0; num < playerVehicles[0].numWeapons; num++)
					{
						_ = playerVehicles[0].weapons[num].active;
						text = "Weapon\t" + playerVehicles[0].weapons[num].weaponID.ToString(CultureInfo.InvariantCulture) + "\r\n";
						isolatedStorageFileStream.Write(mainC.utilMain.String_To_Byte_Array(text), 0, text.Length);
					}
				}
				isolatedStorageFileStream.Close();
			}
			spSaving++;
		}
		catch (Exception)
		{
			spSaving++;
			try
			{
				isolatedStorageFileStream.Close();
			}
			catch
			{
			}
		}
	}

	public byte SaveGame_Exists()
	{
		if (storageDeviceNotChosen || signedinGamerID < 0 || Gamer.SignedInGamers[signedinGamerID].IsGuest)
		{
			return SaveGame_Exists_IsolatedStorage();
		}
		try
		{
			if (deviceGamer == null || !deviceGamer.IsConnected)
			{
				Choose_Gamer_Storage_Container();
				return 0;
			}
			StorageContainer storageContainer = Open_Gamer_Container();
			if (storageContainer.FileExists("GameSave.txt"))
			{
				storageContainer.Dispose();
				return 2;
			}
			storageContainer.Dispose();
			return 1;
		}
		catch (Exception)
		{
		}
		return 1;
	}

	public byte SaveGame_Exists_IsolatedStorage()
	{
		try
		{
			IsolatedStorageFile userStoreForApplication = IsolatedStorageFile.GetUserStoreForApplication();
			if (userStoreForApplication.FileExists("GameSave.txt"))
			{
				return 2;
			}
		}
		catch (Exception)
		{
		}
		return 1;
	}

	public void Load_Global_Settings()
	{
		IsolatedStorageFileStream isolatedStorageFileStream = null;
		try
		{
			curGameTip = 0;
			loadGlobalSettings = false;
			IsolatedStorageFile userStoreForApplication = IsolatedStorageFile.GetUserStoreForApplication();
			isolatedStorageFileStream = null;
			isolatedStorageFileStream = userStoreForApplication.OpenFile("GlobalSettings.txt", FileMode.OpenOrCreate);
			if (isolatedStorageFileStream == null)
			{
				global::Sounds.Sounds.soundEnabled[0] = true;
				global::Sounds.Sounds.soundEnabled[1] = true;
				global::Sounds.Sounds.soundEnabled[2] = true;
				return;
			}
			byte[] array = new byte[isolatedStorageFileStream.Length];
			int i;
			for (i = 0; i < 3; i++)
			{
				global::Sounds.Sounds.soundEnabled[i] = true;
			}
			if (!isolatedStorageFileStream.CanRead)
			{
				return;
			}
			isolatedStorageFileStream.Read(array, 0, array.Length);
			isolatedStorageFileStream.Close();
			string text = mainC.utilMain.Byte_Array_To_String(array);
			string[] array2 = text.Split('\n', '\r');
			i = 0;
			int num = 0;
			for (; i < array2.Length; i++)
			{
				if (array2[i].Length > 0)
				{
					num++;
				}
			}
			if (num < 1)
			{
				loadGlobalSettings = false;
				return;
			}
			string[] array3 = new string[num];
			i = 0;
			num = 0;
			for (; i < array2.Length; i++)
			{
				if (array2[i].Length > 0)
				{
					array3[num++] = array2[i];
				}
			}
			for (i = 0; i < num; i++)
			{
				array2 = array3[i].Split(' ', '\t');
				int j = 0;
				int num2 = 0;
				for (; j < array2.Length; j++)
				{
					if (array2[j].Length > 0)
					{
						num2++;
					}
				}
				if (num2 < 1)
				{
					continue;
				}
				string[] array4 = new string[num2];
				j = 0;
				num2 = 0;
				for (; j < array2.Length; j++)
				{
					if (array2[j].Length > 0)
					{
						array4[num2++] = array2[j];
					}
				}
				int num3 = 0;
				if (array4[0].Equals("brightness", StringComparison.OrdinalIgnoreCase))
				{
					num3 = 1;
				}
				else if (array4[0].Equals("SoundVolume", StringComparison.OrdinalIgnoreCase))
				{
					num3 = 2;
				}
				else if (array4[0].Equals("MusicVolume", StringComparison.OrdinalIgnoreCase))
				{
					num3 = 3;
				}
				else if (array4[0].Equals("VoiceVolume", StringComparison.OrdinalIgnoreCase))
				{
					num3 = 4;
				}
				else if (array4[0].Equals("SoundEnabled", StringComparison.OrdinalIgnoreCase))
				{
					num3 = 5;
				}
				else if (array4[0].Equals("MusicEnabled", StringComparison.OrdinalIgnoreCase))
				{
					num3 = 6;
				}
				else if (array4[0].Equals("VoiceEnabled", StringComparison.OrdinalIgnoreCase))
				{
					num3 = 7;
				}
				else if (array4[0].Equals("CurrentGameTip", StringComparison.OrdinalIgnoreCase))
				{
					num3 = 8;
				}
				else if (array4[0].Equals("PlayIntro", StringComparison.OrdinalIgnoreCase))
				{
					num3 = 9;
				}
				switch (num3)
				{
				case 1:
					if (array4.Length > 1 && array4[1].Length > 0)
					{
						global::Rendering.Rendering.brightness = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
						mainC.renderingMain.Set_Brightness();
					}
					break;
				case 2:
					if (array4.Length > 1 && array4[1].Length > 0)
					{
						global::Sounds.Sounds.volume[0] = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
						if (global::Sounds.Sounds.volume[0] < -96f)
						{
							global::Sounds.Sounds.volume[0] = -96f;
						}
						if (global::Sounds.Sounds.volume[0] > 6f)
						{
							global::Sounds.Sounds.volume[0] = 6f;
						}
					}
					break;
				case 3:
					if (array4.Length > 1 && array4[1].Length > 0)
					{
						global::Sounds.Sounds.volume[1] = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
						if (global::Sounds.Sounds.volume[1] < -96f)
						{
							global::Sounds.Sounds.volume[1] = -96f;
						}
						if (global::Sounds.Sounds.volume[1] > 6f)
						{
							global::Sounds.Sounds.volume[1] = 6f;
						}
					}
					break;
				case 4:
					if (array4.Length > 1 && array4[1].Length > 0)
					{
						global::Sounds.Sounds.volume[2] = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
						if (global::Sounds.Sounds.volume[2] < -96f)
						{
							global::Sounds.Sounds.volume[2] = -96f;
						}
						if (global::Sounds.Sounds.volume[2] > 6f)
						{
							global::Sounds.Sounds.volume[2] = 6f;
						}
					}
					break;
				case 5:
					if (array4.Length > 1 && array4[1].Length > 0)
					{
						global::Sounds.Sounds.soundEnabled[0] = false;
						if (int.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat) == 1)
						{
							global::Sounds.Sounds.soundEnabled[0] = true;
						}
					}
					break;
				case 6:
					if (array4.Length > 1 && array4[1].Length > 0)
					{
						global::Sounds.Sounds.soundEnabled[1] = false;
						if (int.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat) == 1)
						{
							global::Sounds.Sounds.soundEnabled[1] = true;
						}
					}
					break;
				case 7:
					if (array4.Length > 1 && array4[1].Length > 0)
					{
						global::Sounds.Sounds.soundEnabled[2] = false;
						if (int.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat) == 1)
						{
							global::Sounds.Sounds.soundEnabled[2] = true;
						}
					}
					break;
				case 8:
					if (array4.Length > 1 && array4[1].Length > 0)
					{
						curGameTip = byte.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 9:
					if (array4.Length > 1 && array4[1].Length > 0)
					{
						playIntro = false;
						if (int.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat) == 1)
						{
							playIntro = true;
						}
					}
					break;
				}
			}
			if (global::Sounds.Sounds.soundSystemLoaded)
			{
				mainC.soundsMain.Update_Sound_Settings(0);
				mainC.soundsMain.Update_Sound_Settings(1);
				mainC.soundsMain.Update_Sound_Settings(2);
				mainC.soundsMain.Set_Music_Volume();
				mainC.soundsMain.Play_Music(global::Sounds.Sounds.musicLoadingID);
			}
		}
		catch (Exception)
		{
			global::Sounds.Sounds.soundEnabled[0] = true;
			global::Sounds.Sounds.soundEnabled[1] = true;
			global::Sounds.Sounds.soundEnabled[2] = true;
		}
	}

	public void Save_Global_Settings()
	{
		IsolatedStorageFileStream isolatedStorageFileStream = null;
		try
		{
			IsolatedStorageFile userStoreForApplication = IsolatedStorageFile.GetUserStoreForApplication();
			isolatedStorageFileStream = null;
			isolatedStorageFileStream = userStoreForApplication.CreateFile("GlobalSettings.txt");
			if (isolatedStorageFileStream != null && isolatedStorageFileStream.CanWrite)
			{
				string text = "brightness\t" + string.Format(CultureInfo.InvariantCulture, "{0:F3}", new object[1] { global::Rendering.Rendering.brightness }) + "\r\n";
				isolatedStorageFileStream.Write(mainC.utilMain.String_To_Byte_Array(text), 0, text.Length);
				text = ((!global::Sounds.Sounds.soundEnabled[0]) ? "SoundEnabled\t0\r\n" : "SoundEnabled\t1\r\n");
				isolatedStorageFileStream.Write(mainC.utilMain.String_To_Byte_Array(text), 0, text.Length);
				text = ((!global::Sounds.Sounds.soundEnabled[1]) ? "MusicEnabled\t0\r\n" : "MusicEnabled\t1\r\n");
				isolatedStorageFileStream.Write(mainC.utilMain.String_To_Byte_Array(text), 0, text.Length);
				text = ((!global::Sounds.Sounds.soundEnabled[2]) ? "VoiceEnabled\t0\r\n" : "VoiceEnabled\t1\r\n");
				isolatedStorageFileStream.Write(mainC.utilMain.String_To_Byte_Array(text), 0, text.Length);
				text = ((!playIntro) ? "PlayIntro\t0\r\n" : "PlayIntro\t1\r\n");
				isolatedStorageFileStream.Write(mainC.utilMain.String_To_Byte_Array(text), 0, text.Length);
				text = "SoundVolume\t" + string.Format(CultureInfo.InvariantCulture, "{0:F1}", new object[1] { global::Sounds.Sounds.volume[0] }) + "\r\n";
				isolatedStorageFileStream.Write(mainC.utilMain.String_To_Byte_Array(text), 0, text.Length);
				text = "MusicVolume\t" + string.Format(CultureInfo.InvariantCulture, "{0:F1}", new object[1] { global::Sounds.Sounds.volume[1] }) + "\r\n";
				isolatedStorageFileStream.Write(mainC.utilMain.String_To_Byte_Array(text), 0, text.Length);
				text = "VoiceVolume\t" + string.Format(CultureInfo.InvariantCulture, "{0:F1}", new object[1] { global::Sounds.Sounds.volume[2] }) + "\r\n";
				isolatedStorageFileStream.Write(mainC.utilMain.String_To_Byte_Array(text), 0, text.Length);
				text = "CurrentGameTip\t" + curGameTip.ToString(CultureInfo.InvariantCulture) + "\r\n";
				isolatedStorageFileStream.Write(mainC.utilMain.String_To_Byte_Array(text), 0, text.Length);
				isolatedStorageFileStream.Close();
			}
		}
		catch (Exception)
		{
			try
			{
				isolatedStorageFileStream.Close();
			}
			catch
			{
			}
		}
	}

	public void Save_Buffer_Data(ref byte[] buffer, byte bufferType, int count, string fileName)
	{
	}

	public void Save_Buffer_Config_Data(byte type, string fileName)
	{
	}

	public static bool Load_Buffer_Data(ref byte[] buffer, byte bufferType, string fileName)
	{
		return false;
	}

	public static bool Load_Buffer_Config_Data(byte type, string fileName)
	{
		return false;
	}

	public void Game_Object_Drops_Item(ushort itemID, float x, float y, float z)
	{
		mainC.pickupsMain.Activate_Pickup(itemID, x, y, z);
	}

	public void Set_Spawn_Points_Active_Status(ushort start, ushort end, bool status)
	{
		mainC.mapsMain.Set_Spawn_Points_Active_Status(start, end, status);
	}

	public void Trigger_AI_Wave(ushort aiWaveID)
	{
		mainC.aiMain.Trigger_AI_Wave(aiWaveID);
	}

	public void Authorize_Remote_AI_Respawn(ushort remoteGamerID)
	{
		mainC.networkingMain.XBOX_Send_Network_Message_To_Gamer(78, global::Networking.Networking.networkSession.RemoteGamers[remoteGamerID]);
	}

	public void Receive_AI_Respawn_Authorization_From_Host()
	{
		mainC.aiMain.Receive_Ai_Respawn_Authorization();
	}

	public void Rank_Online_Players_By_Score()
	{
		mainC.playersMain.Rank_Online_Players_By_Score();
	}
}
