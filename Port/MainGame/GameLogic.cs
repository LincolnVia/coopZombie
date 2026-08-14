using System;
using System.Globalization;
using AI;
using FontModule;
using GameObjects;
using InputHandler;
using Joints;
using Maps;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using Microsoft.Xna.Framework.Net;
using Models;
using Networking;
using Physics;
using Pickups;
using Players;
using Programs;
using Rendering;
using Sounds;
using Structs;
using Textures;
using Util;
using Weapons;
using WindowsGame1;

namespace MainGame;

public class GameLogic
{
	public static bool skipLevel = false;

	public static bool activatedPickup;

	public static bool playingPrivateMatch;

	public static bool joinFailed;

	public static bool askedToBuy = false;

	public static bool playedIntro = false;

	public static byte hudWeaponStub;

	public static byte playerModelFPV;

	public static byte playerModelTPV;

	public static byte primaryWeaponIndex;

	public static byte secondaryWeaponIndex;

	public static byte numPrimaryWeapons;

	public static byte numSecondaryWeapons;

	public static byte curWeaponSelectArea;

	public static byte lastSecondaryWeaponSelected;

	public static ushort team0PlayerCount;

	public static ushort team1PlayerCount;

	public static ushort texHudGrenade;

	public static ushort texHudAmmo;

	public static ushort texHudAmmoClip;

	public static ushort texHudAmmoShell;

	public static ushort texHudAmmoBullet;

	public static ushort texHudHealth;

	public static ushort texHudHealthIcon;

	public static ushort texHudHealthBar;

	public static ushort texHudLife;

	public static ushort texHudZombies;

	public static ushort texHudWaypoint;

	public static byte[] weaponLevels = new byte[1] { 1 };

	public static byte[] team0Players = new byte[4];

	public static byte[] team1Players = new byte[4];

	public static float[] propRot = new float[44];

	public static float[] targetTimer = new float[44];

	public static float showZombiesTimer;

	public static float showZombiesPosition;

	public static float ironSightsAdjX;

	public static float ironSightsAdjY;

	public static float ironSightsAdjZ;

	public static float lightDirection;

	public static float wpnsltPrimaryAmbient;

	public static float wpnsltSecondaryAmbient;

	public static Color colorWhite;

	public static string loadingMsg = "Loading";

	public Game1.MasterCollection mainC;

	public void Init_Vars(Game1.MasterCollection master)
	{
		mainC = master;
	}

	public void Game_Load_Starting()
	{
		global::Models.Models.loadingScreenModelDegPerSec = 0.5f;
		global::Rendering.Rendering.loadingIconY = 445;
		MainGame.primaryWeaponMount = 0;
		MainGame.secondaryWeaponMount = 0;
		MainGame.primaryObjectMount = 1;
		MainGame.numTeams = 5;
		float[] dualWieldAdjX = new float[3];
		global::Players.Players.dualWieldAdjX = dualWieldAdjX;
		float[] dualWieldAdjY = new float[3];
		global::Players.Players.dualWieldAdjY = dualWieldAdjY;
		float[] dualWieldAdjZ = new float[3];
		global::Players.Players.dualWieldAdjZ = dualWieldAdjZ;
		byte[] lockedVehicleLevels = new byte[1];
		Vehicles.lockedVehicleLevels = lockedVehicleLevels;
		byte[] vehicelSelectVehicleIDs = new byte[1];
		Vehicles.vehicelSelectVehicleIDs = vehicelSelectVehicleIDs;
		global::Weapons.Weapons.lockedWeaponSkinLevels = new byte[12]
		{
			1, 5, 5, 5, 5, 5, 1, 5, 5, 5,
			5, 5
		};
		byte[] lockedWeaponLevels = new byte[12];
		global::Weapons.Weapons.lockedWeaponLevels = lockedWeaponLevels;
		global::Weapons.Weapons.weaponSelectWeaponIDs = new byte[8] { 0, 2, 6, 7, 8, 9, 10, 11 };
		MainGame.numGameTips = 0;
		hudWeaponStub = 0;
	}

	public void Game_Load_In_Progress()
	{
		global::Rendering.Rendering.rGraphics.Clear(Color.Black);
		mainC.renderingMain.Render_Splash();
		if (global::Rendering.Rendering.initialLoading == 0)
		{
			mainC.renderingMain.Render_Loading_Graphic();
		}
		mainC.fontmoduleMain.Reset_Graphics_Adapter_After_SpriteBatch();
	}

	public void Game_Load_Finished()
	{
		colorWhite = new Color(1f, 1f, 1f);
		MainGame.mp_numPlayers[0] = 44;
		MainGame.mp_numRemotePlayers[0] = 4;
		curWeaponSelectArea = 0;
		lightDirection = 0f;
		mainC.renderingMain.Load_Rendering_Data("Menu_Rendering_Data.txt");
		global::Pickups.Pickups.cosinePeriod = 2.5f;
		global::Weapons.Weapons.numMuzzleFlashTexturesMainPlayer = 4;
		global::Weapons.Weapons.muzzleFlashTexturesMainPlayer = new ushort[global::Weapons.Weapons.numMuzzleFlashTexturesMainPlayer];
		global::Weapons.Weapons.muzzleFlashTexturesMainPlayer[0] = (ushort)mainC.texturesMain.Find_Texture("MuzzleFlash_0", 0);
		global::Weapons.Weapons.muzzleFlashTexturesMainPlayer[1] = (ushort)mainC.texturesMain.Find_Texture("MuzzleFlash_1", 0);
		global::Weapons.Weapons.muzzleFlashTexturesMainPlayer[2] = (ushort)mainC.texturesMain.Find_Texture("MuzzleFlash_2", 0);
		global::Weapons.Weapons.muzzleFlashTexturesMainPlayer[3] = (ushort)mainC.texturesMain.Find_Texture("MuzzleFlash_3", 0);
		numPrimaryWeapons = 5;
		numSecondaryWeapons = 2;
		primaryWeaponIndex = 0;
		secondaryWeaponIndex = 0;
		global::Rendering.Rendering.weaponSelectMatrix3 = Matrix.CreateScale(205f) * Matrix.CreateTranslation(0f, -48.68f, 0f);
		global::Players.Players.players[0].primaryWeaponMountWeapon = (sbyte)mainC.weaponsMain.Get_Weapon_ID_By_Name("M1911", 0);
		global::Players.Players.rankUpPointLevels = new int[50]
		{
			0, 150, 300, 400, 500, 750, 750, 1000, 1000, 1000,
			1050, 1100, 1250, 1250, 1500, 1600, 1700, 1750, 1750, 2000,
			2100, 2200, 2300, 2400, 2500, 2700, 2700, 2800, 2900, 3000,
			3200, 3400, 3600, 3800, 4100, 4300, 4300, 4500, 4600, 4750,
			5000, 52000, 5400, 5600, 5750, 5750, 5750, 6000, 6500, 7500
		};
		global::Networking.Networking.posMessageType = 0;
		global::Sounds.Sounds.soundList = new string[6];
		global::Sounds.Sounds.soundList[0] = "Weapon_Shotgun_InsertShell";
		global::Sounds.Sounds.soundList[1] = "Weapon_Reload_MP5a";
		global::Sounds.Sounds.soundList[2] = "Weapon_Reload_MP5b";
		global::Sounds.Sounds.soundList[3] = "Weapon_Reload_MP5c";
		global::Sounds.Sounds.soundList[4] = "Weapon_Reload_Pistol";
		global::Sounds.Sounds.soundList[5] = "Grenade_Pin";
		global::Maps.Maps.aiPlayerToSpawnNear = 0;
		global::Maps.Maps.aiSpawnMode = 1;
		global::Maps.Maps.aiSpawnRadiusMaxSqr = 10000f;
		global::Maps.Maps.aiSpawnRadiusMinSqr = 900f;
		playerModelFPV = (byte)mainC.modelsMain.Find_Model("Guard0.txt");
		playerModelTPV = (byte)mainC.modelsMain.Find_Model("Guard0_Full.txt");
		global::Players.Players.controllerScheme = 1;
		global::Players.Players.playerRankMax = 50;
		for (ushort num = 0; num < 44; num++)
		{
			global::Players.Players.players[num].primaryWeaponMountWeapon = 6;
		}
		ref Matrix reference = ref global::Players.Players.players[0].mv[global::Rendering.Rendering.uBufferID];
		reference = Matrix.Identity;
		global::Joints.Joints.Reset_Joint_Rotations_To_Zero(0);
		global::Joints.Joints.Reset_Joint_Data(0);
		global::Joints.Joints.Moved_Joint_Calculations(0, global::Players.Players.players[0].numJoints);
		global::Joints.Joints.Sync_Player_Matrices(0, global::Rendering.Rendering.uBufferID, global::Rendering.Rendering.rBufferID);
		global::Rendering.Rendering.useShadowMap = false;
		global::Rendering.Rendering.renderGamerTags = 1;
		global::Rendering.Rendering.skyBoxType = 3;
		global::Rendering.Rendering.crossHairMovementSpeed = 600f;
		global::Rendering.Rendering.swapWeaponPos.X = 555f;
		global::Rendering.Rendering.swapWeaponPos.Y = 442f;
		global::AI.AI.aiTargetingMode = 0;
		global::AI.AI.endLevelOnLastAI = false;
		MainGame.saveSpWeapons = true;
		MainGame.allowTeamKills = false;
		MainGame.numAchievementRewards = 4;
		MainGame.achievementRewards = new StructsClass.Game_Reward[4];
		for (ushort num = 0; num < 4; num++)
		{
			MainGame.achievementRewards[num] = default(StructsClass.Game_Reward);
		}
		MainGame.achievementRewards[0].rewardLength = 15f;
		MainGame.achievementRewards[1].rewardLength = 25f;
		MainGame.achievementRewards[2].rewardLength = 30f;
		MainGame.achievementRewards[3].rewardLength = 6f;
		MainGame.lockedTauntLevels = new byte[4] { 0, 1, 2, 3 };
		MainGame.tauntIDs = new byte[4] { 10, 11, 12, 13 };
		MainGame.numTaunts = 4;
		MainGame.curTaunt = 0;
		MainGame.tauntingEnabled = false;
		MainGame.bombViewEnabled = false;
		MainGame.pointsForEnemyDeath = 75;
		MainGame.pointsForEnemyAiKill = 25;
		MainGame.pointsForOwnDeath = 25;
		MainGame.pointsForTeamKill = 0;
		MainGame.teamPointsForEnemyDeath = 75;
		MainGame.teamPointsForEnemyAiDeath = 25;
		MainGame.teamPointsForOwnDeath = 25;
		MainGame.teamPointsForTeamKill = 0;
		MainGame.soundWhenEnemeyKilled = false;
		MainGame.bombWeaponMount = 0;
		MainGame.linearProgression = true;
		MainGame.showGameFlags = 0;
		global::Rendering.Rendering.gamerTagFont = 1;
		global::Rendering.Rendering.renderMinimap = false;
		global::Rendering.Rendering.renderGamerTagMask = 4;
		global::Rendering.Rendering.renderHud = true;
		User_Interface.vehicleSelectAutoStartTime = 15f;
		global::GameObjects.GameObjects.playersCanDamageTeamObjects = true;
		global::Sounds.Sounds.randomLevelTracks = true;
		Game_Reset_Textures();
		if (global::Players.Players.currentView != 1 || global::Players.Players.currentView != 0)
		{
			global::Players.Players.currentView = 1;
			global::Players.Players.lastView = 1;
		}
		global::Weapons.Weapons.numBallisticStrikeTypes = 0;
		global::Weapons.Weapons.numBallisticStrikes = 0;
		ref Matrix reference2 = ref MainGame.itemPlacementMatrix[0];
		reference2 = Matrix.CreateRotationX((float)Math.PI / 90f);
		ref Matrix reference3 = ref MainGame.itemPlacementMatrix[1];
		reference3 = Matrix.Identity;
		ref Matrix reference4 = ref MainGame.itemPlacementMatrix[2];
		reference4 = Matrix.CreateRotationX(-(float)Math.PI / 15f) * Matrix.CreateRotationZ(0.00034906584f) * Matrix.CreateTranslation(-0.012f, 0.04f, 0.013f);
		ref Matrix reference5 = ref MainGame.itemPlacementMatrix[3];
		reference5 = Matrix.CreateTranslation(0.02f, 0.04f, -0.01f) * Matrix.CreateRotationX(-0.19198622f);
		ref Matrix reference6 = ref MainGame.itemPlacementMatrix[4];
		reference6 = Matrix.Identity;
		ref Matrix reference7 = ref MainGame.itemPlacementMatrix[5];
		reference7 = Matrix.Identity;
		ref Matrix reference8 = ref MainGame.itemPlacementMatrix[6];
		reference8 = Matrix.CreateTranslation(0.02f, 0.02f, 0.01f) * Matrix.CreateRotationY(0.0034906585f) * Matrix.CreateRotationX(-0.19198622f);
		ref Matrix reference9 = ref MainGame.itemPlacementMatrix[7];
		reference9 = Matrix.CreateRotationY((float)Math.PI / 45f) * Matrix.CreateRotationX(-0.2443461f) * Matrix.CreateRotationZ(0.17453292f) * Matrix.CreateTranslation(-0.05f, 0.28f, 0f);
		ref Matrix reference10 = ref MainGame.itemPlacementMatrix[12];
		reference10 = Matrix.Identity;
		ref Matrix reference11 = ref MainGame.itemPlacementMatrix[13];
		reference11 = Matrix.CreateRotationX(-(float)Math.PI / 6f) * Matrix.CreateRotationZ((float)Math.PI / 6f) * Matrix.CreateTranslation(0.05f, -0.08f, -0.1f);
		ref Matrix reference12 = ref MainGame.itemPlacementMatrix[8];
		reference12 = Matrix.CreateRotationY((float)Math.PI / 30f) * Matrix.CreateRotationX(2.3298008f) * Matrix.CreateRotationZ(-0.19198622f) * Matrix.CreateTranslation(0.248f, -0.4797997f, 0.0101f);
		ref Matrix reference13 = ref MainGame.itemPlacementMatrix[9];
		reference13 = Matrix.CreateRotationY((float)Math.PI / 30f) * Matrix.CreateRotationX(2.3298008f) * Matrix.CreateRotationZ(-0.19198622f) * Matrix.CreateTranslation(0.248f, -0.4797997f, 0.0101f);
		ref Matrix reference14 = ref MainGame.itemPlacementMatrix[10];
		reference14 = Matrix.CreateRotationY((float)Math.PI / 30f) * Matrix.CreateRotationX(2.3298008f) * Matrix.CreateRotationZ(-0.19198622f) * Matrix.CreateTranslation(0.248f, -0.4797997f, 0.0101f);
		ref Matrix reference15 = ref MainGame.itemPlacementMatrix[11];
		reference15 = Matrix.CreateRotationY((float)Math.PI * 53f / 90f) * Matrix.CreateRotationX(4.4331365f) * Matrix.CreateTranslation(0.43f, -0.39f, 0.05f);
		global::FontModule.FontModule.multiplayerMessages.bottomLeftX = 129f;
		global::FontModule.FontModule.multiplayerMessages.bottomLeftY = 440f;
		for (ushort num = 0; num < 4; num++)
		{
			global::Players.Players.remotePlayerPositionOffsetX[num] *= 2f;
			global::Players.Players.remotePlayerPositionOffsetY[num] *= 2f;
		}
	}

	public bool Game_Ready_To_Show_Main_Menu()
	{
		return true;
	}

	public void Game_Render_Splash()
	{
	}

	public void Game_MP_Initial_New_Game_Setup(byte threadID)
	{
		global::Players.Players.needSpawn = true;
		MainGame.numLives = 0;
		MainGame.commanderMode = false;
		MainGame.useFixedSpawnPoint = false;
		MainGame.restartIsDeath = true;
		MainGame.autoRespawn = true;
		MainGame.newRoundOnDeath = false;
		MainGame.respawnTime = 5f;
		MainGame.unlimitedAmmo = false;
		MainGame.maxLocalPlayerSpawnPoint = -1;
		global::Rendering.Rendering.watchingPlayer = 0;
		for (ushort num = 1; num < MainGame.maxHumanGamePlayers; num++)
		{
			mainC.playersMain.Set_Player_Race((byte)num, 0, 1);
			global::Players.Players.players[num].playerModel[0] = playerModelTPV;
		}
	}

	public void Game_SP_Initial_Setup()
	{
		global::AI.AI.killCountScale = 1f;
		MainGame.gameType = 7;
		MainGame.commanderMode = false;
		MainGame.useFixedSpawnPoint = true;
		MainGame.restartIsDeath = true;
		MainGame.autoRespawn = true;
		MainGame.newRoundOnDeath = false;
		MainGame.maxLocalPlayerSpawnPoint = 0;
		MainGame.spSpawnCheckForEnemy = true;
		MainGame.respawnTime = 8f;
		MainGame.unlimitedAmmo = false;
		if (global::InputHandler.InputHandler.newSPGame)
		{
			MainGame.numLives = 3;
		}
		global::Players.Players.respawnEnabled = false;
		global::Rendering.Rendering.watchingPlayer = 0;
		global::Players.Players.players[0].curVehicleIndex = 0;
		global::Players.Players.players[0].vehicles[0] = global::Players.Players.playerRaces[global::Players.Players.players[0].race].vehicleID[global::Players.Players.players[0].type];
		global::Players.Players.players[0].curVehicle = global::Players.Players.playerRaces[global::Players.Players.players[0].race].vehicleID[global::Players.Players.players[0].type];
		for (ushort num = 1; num < 3; num++)
		{
			global::Players.Players.players[0].vehicles[num] = global::Util.Util.maxUnsignedShortValue;
		}
	}

	public void Game_SP_Initial_Setup_Finished()
	{
	}

	public void Game_MP_New_Game_Setup_Finished(byte threadID)
	{
		Game_Update_Camera(threadID);
		mainC.renderingMain.Set_Camera_To_Camera_Goal_Positions();
		global::Players.Players.needSpawn = true;
		MainGame.autoStartMPGame = true;
		MainGame.commanderMode = false;
		MainGame.useFixedSpawnPoint = false;
		MainGame.restartIsDeath = true;
		MainGame.autoRespawn = true;
		MainGame.newRoundOnDeath = false;
		MainGame.respawnTime = 5f;
		global::Players.Players.currentPlayerRank = global::Players.Players.playerRankMP;
		global::Players.Players.remotePlayerRanks[0] = global::Players.Players.currentPlayerRank;
		for (ushort num = 0; num < global::AI.AI.numAI; num++)
		{
			global::AI.AI.ais[num].targetMode = 0;
			global::AI.AI.ais[num].targetID = 0;
			if (global::AI.AI.ais[num].playerID > -1)
			{
				targetTimer[global::AI.AI.ais[num].playerID] = 0f;
			}
		}
		mainC.aiMain.Update_KillCount_Scale();
	}

	public void Game_Reset_SP_Level_Do_First()
	{
		activatedPickup = false;
		global::Players.Players.players[0].onmap = 1;
		global::Players.Players.respawnEnabled = false;
	}

	public void Game_Reset_SP_Level_Do_Last(byte threadID)
	{
		Game_Reset_Scores();
		Game_Update_Camera(threadID);
		mainC.renderingMain.Set_Camera_To_Camera_Goal_Positions();
	}

	public void Game_Reset_MP_Level_Do_First()
	{
	}

	public void Game_Reset_MP_Level_Do_Last(byte threadID)
	{
		Game_Update_Camera(threadID);
		mainC.renderingMain.Set_Camera_To_Camera_Goal_Positions();
		mainC.playersMain.Adjust_Player_Damage_To_Zero(0, sendOnline: false);
		for (ushort num = 0; num < global::AI.AI.numAI; num++)
		{
			global::AI.AI.ais[num].targetMode = 0;
			if (global::AI.AI.ais[num].playerID > -1)
			{
				targetTimer[global::AI.AI.ais[num].playerID] = 0f;
			}
		}
	}

	public void Game_New_SP_Round(byte threadID)
	{
		showZombiesTimer = 0f;
		showZombiesPosition = 0f;
		global::Players.Players.playerRankSP = (byte)(MainGame.gameLevel + 1);
		global::Players.Players.currentPlayerRank = global::Players.Players.playerRankSP;
		mainC.soundsMain.Disable_Background_Sounds();
		mainC.soundsMain.Set_Background_Sound_Repeat_Interval(0, 10f);
		mainC.soundsMain.Start_Background_Sound(0, 0f, global::Sounds.Sounds.volume[0] * 0.5f);
		mainC.soundsMain.Set_Background_Sound_Repeat_Interval(1, 20f);
		mainC.soundsMain.Start_Background_Sound(1, 3f, global::Sounds.Sounds.volume[0] * 0.4f);
		MainGame.autoRespawn = false;
		global::Players.Players.respawnEnabled = false;
		Game_Reset_Scores();
		_ = global::Players.Players.players[0].primaryWeaponMountWeapon;
		_ = global::Weapons.Weapons.wp1[global::Players.Players.players[0].primaryWeaponMountWeapon].AnimationHolding;
		_ = global::Players.Players.players[0].programCollection;
		mainC.jointsMain.Update_Joints_For_New_Position(0);
		Game_Update_Camera(threadID);
		mainC.renderingMain.Set_Camera_To_Camera_Goal_Positions();
	}

	public void Game_New_MP_Round(byte threadID)
	{
		showZombiesTimer = 0f;
		showZombiesPosition = 0f;
		activatedPickup = false;
		Game_Update_Camera(threadID);
		mainC.renderingMain.Set_Camera_To_Camera_Goal_Positions();
		mainC.soundsMain.Disable_Background_Sounds();
		mainC.soundsMain.Set_Background_Sound_Repeat_Interval(0, 10f);
		mainC.soundsMain.Start_Background_Sound(0, 0f, global::Sounds.Sounds.volume[0] * 0.5f);
		mainC.soundsMain.Set_Background_Sound_Repeat_Interval(1, 20f);
		mainC.soundsMain.Start_Background_Sound(1, 3f, global::Sounds.Sounds.volume[0] * 0.4f);
	}

	public void Game_MP_Game_Cleanup_Before_Start()
	{
		global::Players.Players.respawnEnabled = true;
		try
		{
			if (MainGame.signedinGamerID > -1 && MainGame.signedinGamerID < Gamer.SignedInGamers.Count && Gamer.SignedInGamers[MainGame.signedinGamerID] != null && Gamer.SignedInGamers[MainGame.signedinGamerID].IsSignedInToLive)
			{
				Gamer.SignedInGamers[MainGame.signedinGamerID].Presence.PresenceMode = GamerPresenceMode.Multiplayer;
			}
		}
		catch (Exception)
		{
		}
	}

	public void Game_Initialize_GameData_Scores()
	{
		byte b = 1;
		byte b2 = 1;
		byte b3 = 1;
		MainGame.gameData.floatSize = b;
		MainGame.gameData.intSize = b2;
		MainGame.gameData.ushortDataSize = b3;
		MainGame.lastGameData.floatSize = b;
		MainGame.lastGameData.intSize = b2;
		MainGame.lastGameData.ushortDataSize = b3;
		for (ushort num = 0; num < 44; num++)
		{
			MainGame.gameData.players[num].scoresF = new float[b];
			MainGame.gameData.players[num].scoresI = new int[b2];
			MainGame.gameData.players[num].dataUS = new ushort[b3];
			MainGame.lastGameData.players[num].scoresF = new float[b];
			MainGame.lastGameData.players[num].scoresI = new int[b2];
			MainGame.lastGameData.players[num].dataUS = new ushort[b3];
		}
	}

	public void Game_Initialize_Vehicle_Data()
	{
		for (ushort num = 0; num < Vehicles.numVehicles; num++)
		{
			Vehicles.vehicles[num].data8 *= (float)Math.PI / 180f;
			Vehicles.vehicles[num].data12 *= (float)Math.PI / 180f;
		}
	}

	public bool Game_Showing_Mission_Objectives()
	{
		User_Interface.missionObjectivesFinished = true;
		return !User_Interface.missionObjectivesFinished;
	}

	public bool Game_Showing_Weapon_Select()
	{
		User_Interface.weaponSelectFinished = true;
		return !User_Interface.weaponSelectFinished;
	}

	public bool Game_Showing_Vehicle_Select()
	{
		return false;
	}

	public void Game_Init_Music()
	{
		global::Sounds.Sounds.numMusicCues = 3;
		global::Sounds.Sounds.numLevelMusic = 2;
		global::Sounds.Sounds.musicList = new string[global::Sounds.Sounds.numMusicCues];
		global::Sounds.Sounds.musicIsPlaying = new bool[global::Sounds.Sounds.numMusicCues];
		global::Sounds.Sounds.musicCue = new Cue[global::Sounds.Sounds.numMusicCues];
		global::Sounds.Sounds.levelMusic = new byte[global::Sounds.Sounds.numLevelMusic];
		global::Sounds.Sounds.musicList[0] = "Music1";
		global::Sounds.Sounds.musicList[1] = "Music2";
		global::Sounds.Sounds.musicList[2] = "Music_MainMenu";
		global::Sounds.Sounds.levelMusic[0] = 0;
		global::Sounds.Sounds.levelMusic[1] = 1;
		global::Sounds.Sounds.musicLoadingID = 2;
		global::Sounds.Sounds.musicMenuID = 2;
	}

	public void Game_Set_Vehicle_Weapons(ushort playerID)
	{
		if (playerID == 0)
		{
			if (MainGame.needToLoadWeapons)
			{
				MainGame.needToLoadWeapons = false;
				global::Players.Players.players[0].primaryWeaponMountWeapon = (sbyte)mainC.weaponsMain.Get_Weapon_ID_By_Name("M1911", 0);
				mainC.vehicles.Set_Mount_Weapon_Stub(0, 0, 0);
				mainC.vehicles.Add_Weapon_To_Player_Vehicle_Mount(playerID, MainGame.primaryWeaponMount, (byte)global::Players.Players.players[0].primaryWeaponMountWeapon, 0);
				mainC.vehicles.Add_Weapon_To_Player_Vehicle_Stub(playerID, 1, 1, 0);
				mainC.vehicles.Remove_Weapon_From_Player_Vehicle(playerID, 2);
			}
		}
		else
		{
			mainC.vehicles.Remove_Weapon_From_Player_Vehicle(playerID, 2);
			mainC.vehicles.Add_Weapon_To_Player_Vehicle_Mount(playerID, MainGame.primaryWeaponMount, (byte)global::Players.Players.players[playerID].primaryWeaponMountWeapon, 0);
			mainC.weaponsMain.Add_First_Available_Weapon_Attachment_To_All_Category_Attach_Points(playerID, MainGame.playerVehicles[playerID].mounts[MainGame.primaryWeaponMount].objectID, 0);
		}
		for (byte b = 0; b < MainGame.playerVehicles[playerID].numWeapons; b++)
		{
			if (MainGame.playerVehicles[playerID].weapons[b].active)
			{
				mainC.vehicles.Add_Weapon_To_Player_Vehicle_Stub(playerID, b, MainGame.playerVehicles[playerID].weapons[b].weaponID, 0);
				mainC.weaponsMain.Add_First_Available_Weapon_Attachment_To_All_Category_Attach_Points(playerID, b, 0);
			}
		}
		mainC.gameLogic.Game_Modify_Weapon_Programs_For_Attachments(playerID);
		mainC.vehicles.Remove_Object_In_Player_Vehicle_Mount(playerID, MainGame.primaryObjectMount);
		global::Players.Players.players[playerID].programSwitchWeapons = global::Weapons.Weapons.wp1[global::Players.Players.players[playerID].primaryWeaponMountWeapon].AnimationChangeWeapon;
		if (playerID == 0)
		{
			User_Interface.newScopeID = 0;
			if (global::Players.Players.players[0].primaryWeaponMountWeapon == 8)
			{
				User_Interface.newScopeID = 1;
			}
			mainC.weaponsMain.Add_Weapon_Attachments(0, MainGame.primaryWeaponMount);
			hudWeaponStub = MainGame.playerVehicles[0].mounts[MainGame.primaryWeaponMount].objectID;
		}
		Game_Modify_Weapon_Programs_For_Attachments(playerID);
		Game_Vehicle_Primary_Mount_Weapon_Changed(playerID);
	}

	public void Game_Vehicle_Primary_Mount_Weapon_Changed(ushort playerID)
	{
		byte weaponID = MainGame.playerVehicles[playerID].weapons[MainGame.playerVehicles[playerID].mounts[MainGame.primaryWeaponMount].objectID].weaponID;
		if (playerID == 0)
		{
			global::Players.Players.players[0].primaryWeaponMountWeapon = (sbyte)weaponID;
			hudWeaponStub = MainGame.playerVehicles[playerID].mounts[MainGame.primaryWeaponMount].objectID;
			MainGame.curTaunt = (byte)User_Interface.newTauntID;
			global::Rendering.Rendering.viewMatrixID = 0;
			global::Players.Players.dualWieldAdjX[0] = 0f;
			global::Players.Players.dualWieldAdjX[1] = 0f;
			global::Players.Players.dualWieldAdjX[2] = 0f;
			global::Players.Players.dualWieldAdjY[0] = 0f;
			global::Players.Players.dualWieldAdjY[1] = 0f;
			global::Players.Players.dualWieldAdjY[2] = 0f;
			global::Players.Players.dualWieldAdjZ[0] = 0f;
			global::Players.Players.dualWieldAdjZ[1] = 0f;
			global::Players.Players.dualWieldAdjZ[2] = 0f;
			global::Rendering.Rendering.gunRunAdjX = 0f;
			global::Rendering.Rendering.gunRunAdjY = 0f;
			global::Rendering.Rendering.gunRunAdjZ = 0f;
			global::Rendering.Rendering.weaponViewTime = 0.3167f;
		}
		switch (MainGame.playerVehicles[playerID].weapons[MainGame.playerVehicles[playerID].mounts[MainGame.primaryWeaponMount].objectID].weaponID)
		{
		case 0:
			if (playerID == 0)
			{
				ironSightsAdjX = -0.238f;
				ironSightsAdjY = -0.417f;
				ironSightsAdjZ = 0.57f;
				global::Rendering.Rendering.viewAdjX = -0.088f;
				global::Rendering.Rendering.viewAdjY = -0.439f;
				global::Rendering.Rendering.viewAdjZ = 0.073f;
				global::Rendering.Rendering.gunRunAdjZ = -0.1f;
				mainC.weaponsMain.Set_Weapon_View_Variables(MainGame.playerVehicles[playerID].mounts[MainGame.primaryWeaponMount].objectID);
			}
			MainGame.playerVehicles[playerID].mounts[MainGame.primaryWeaponMount].itemPlacmentMatrixID = 0;
			MainGame.playerVehicles[playerID].mounts[MainGame.primaryObjectMount].itemPlacmentMatrixID = 8;
			break;
		case 1:
			MainGame.playerVehicles[playerID].mounts[MainGame.primaryWeaponMount].itemPlacmentMatrixID = 1;
			MainGame.playerVehicles[playerID].mounts[MainGame.primaryObjectMount].itemPlacmentMatrixID = 8;
			break;
		case 2:
			if (playerID == 0)
			{
				ironSightsAdjX = -0.218f;
				ironSightsAdjY = -0.441f;
				ironSightsAdjZ = 0.638f;
				global::Rendering.Rendering.viewAdjX = 0.181f;
				global::Rendering.Rendering.viewAdjY = -0.438f;
				global::Rendering.Rendering.viewAdjZ = 0.336f;
				global::Rendering.Rendering.gunRunAdjZ = -0.07f;
				mainC.weaponsMain.Set_Weapon_View_Variables(MainGame.playerVehicles[playerID].mounts[MainGame.primaryWeaponMount].objectID);
			}
			MainGame.playerVehicles[playerID].mounts[MainGame.primaryWeaponMount].itemPlacmentMatrixID = 2;
			MainGame.playerVehicles[playerID].mounts[MainGame.primaryObjectMount].itemPlacmentMatrixID = 8;
			break;
		case 6:
			if (playerID == 0)
			{
				ironSightsAdjX = -0.308f;
				ironSightsAdjY = -1.217f;
				ironSightsAdjZ = 0.356f;
				global::Rendering.Rendering.viewAdjX = 0.116f;
				global::Rendering.Rendering.viewAdjY = 0.14f;
				global::Rendering.Rendering.viewAdjZ = 0.118f;
				global::Rendering.Rendering.gunRunAdjZ = -0.13f;
				mainC.weaponsMain.Set_Weapon_View_Variables(MainGame.playerVehicles[playerID].mounts[MainGame.primaryWeaponMount].objectID);
			}
			MainGame.playerVehicles[playerID].mounts[MainGame.primaryWeaponMount].itemPlacmentMatrixID = 3;
			MainGame.playerVehicles[playerID].mounts[MainGame.primaryObjectMount].itemPlacmentMatrixID = 11;
			break;
		case 7:
			if (playerID == 0)
			{
				ironSightsAdjX = -0.307f;
				ironSightsAdjY = -0.716f;
				ironSightsAdjZ = 0.609f;
				global::Rendering.Rendering.viewAdjX = -0.017f;
				global::Rendering.Rendering.viewAdjY = -0.038f;
				global::Rendering.Rendering.viewAdjZ = 0.131f;
				global::Rendering.Rendering.gunRunAdjZ = -0.07f;
				mainC.weaponsMain.Set_Weapon_View_Variables(MainGame.playerVehicles[playerID].mounts[MainGame.primaryWeaponMount].objectID);
			}
			MainGame.playerVehicles[playerID].mounts[MainGame.primaryWeaponMount].itemPlacmentMatrixID = 4;
			MainGame.playerVehicles[playerID].mounts[MainGame.primaryObjectMount].itemPlacmentMatrixID = 8;
			break;
		case 8:
			if (playerID == 0)
			{
				ironSightsAdjX = -0.238f;
				ironSightsAdjY = -0.417f;
				ironSightsAdjZ = 0.57f;
				global::Rendering.Rendering.viewAdjX = -0.017f;
				global::Rendering.Rendering.viewAdjY = -0.038f;
				global::Rendering.Rendering.viewAdjZ = 0.131f;
				global::Rendering.Rendering.gunRunAdjZ = -0.07f;
				mainC.weaponsMain.Set_Weapon_View_Variables(MainGame.playerVehicles[playerID].mounts[MainGame.primaryWeaponMount].objectID);
			}
			MainGame.playerVehicles[playerID].mounts[MainGame.primaryWeaponMount].itemPlacmentMatrixID = 5;
			MainGame.playerVehicles[playerID].mounts[MainGame.primaryObjectMount].itemPlacmentMatrixID = 8;
			break;
		case 9:
			if (playerID == 0)
			{
				ironSightsAdjX = -0.328f;
				ironSightsAdjY = -1.147f;
				ironSightsAdjZ = 0.41f;
				global::Rendering.Rendering.viewAdjX = 0.066f;
				global::Rendering.Rendering.viewAdjY = 0.13f;
				global::Rendering.Rendering.viewAdjZ = 0.093f;
				global::Rendering.Rendering.gunRunAdjZ = -0.13f;
				mainC.weaponsMain.Set_Weapon_View_Variables(MainGame.playerVehicles[playerID].mounts[MainGame.primaryWeaponMount].objectID);
			}
			MainGame.playerVehicles[playerID].mounts[MainGame.primaryWeaponMount].itemPlacmentMatrixID = 6;
			MainGame.playerVehicles[playerID].mounts[MainGame.primaryObjectMount].itemPlacmentMatrixID = 11;
			break;
		case 10:
			if (playerID == 0)
			{
				ironSightsAdjX = -0.197f;
				ironSightsAdjY = -0.589f;
				ironSightsAdjZ = 0.397f;
				global::Rendering.Rendering.viewAdjX = 0.189f;
				global::Rendering.Rendering.viewAdjY = -0.077f;
				global::Rendering.Rendering.viewAdjZ = 0.388f;
				global::Rendering.Rendering.gunRunAdjZ = -0.1f;
				mainC.weaponsMain.Set_Weapon_View_Variables(MainGame.playerVehicles[playerID].mounts[MainGame.primaryWeaponMount].objectID);
			}
			MainGame.playerVehicles[playerID].mounts[MainGame.primaryWeaponMount].itemPlacmentMatrixID = 7;
			MainGame.playerVehicles[playerID].mounts[MainGame.primaryObjectMount].itemPlacmentMatrixID = 8;
			break;
		case 11:
			if (playerID == 0)
			{
				global::Rendering.Rendering.gunRunAdjZ = 0f;
				mainC.weaponsMain.Set_Weapon_View_Variables(MainGame.playerVehicles[playerID].mounts[MainGame.primaryWeaponMount].objectID);
			}
			MainGame.playerVehicles[playerID].mounts[MainGame.primaryWeaponMount].itemPlacmentMatrixID = 12;
			break;
		case 16:
		case 17:
			if (playerID == 0)
			{
				global::Rendering.Rendering.gunRunAdjZ = 0f;
				mainC.weaponsMain.Set_Weapon_View_Variables(MainGame.playerVehicles[playerID].mounts[MainGame.primaryWeaponMount].objectID);
			}
			MainGame.playerVehicles[playerID].mounts[MainGame.primaryWeaponMount].itemPlacmentMatrixID = 13;
			break;
		}
		mainC.weaponsMain.Add_First_Available_Weapon_Attachment_To_All_Category_Attach_Points(playerID, MainGame.playerVehicles[playerID].mounts[MainGame.primaryWeaponMount].objectID, 0);
		Game_Modify_Weapon_Programs_For_Attachments(playerID);
		global::Players.Players.players[playerID].primaryWeaponMountWeapon = (sbyte)weaponID;
		global::Players.Players.players[playerID].programSwitchWeapons = global::Weapons.Weapons.wp1[weaponID].AnimationChangeWeapon;
	}

	public void Game_Modify_Weapon_Programs_For_Attachments(ushort playerID)
	{
	}

	public void Game_Set_Avatar_Vehicle_Pose(ushort playerID)
	{
	}

	public void Game_Set_Avatar_Arm_Overrides()
	{
	}

	public void Game_Load_Custom_Avatar_Animations()
	{
	}

	public void Game_Player_Hit(ushort playerID, ushort shooterID)
	{
	}

	public void Game_Check_Player_Off_Map_Status()
	{
		if (global::Players.Players.players[0].onmap == 1 && !global::Players.Players.players[0].dead)
		{
			switch (MainGame.gameState)
			{
			case 2:
			case 17:
			case 21:
			case 141:
			case 149:
				global::Players.Players.players[0].dead = true;
				break;
			}
		}
	}

	public bool Game_Points_Pickup(short actionID, short id, short refID, ushort playerID, bool bool1)
	{
		return false;
	}

	public void Game_Lap_Checkpoint(short actionID, short id, short refID, bool bool1, byte threadID)
	{
		if (actionID != 0)
		{
			return;
		}
		if (bool1)
		{
			MainGame.laps[0]++;
			MainGame.displayLaps = MainGame.laps[0];
			mainC.maingameMain.Send_Special_Messages(4);
		}
		if (MainGame.laps[0] <= MainGame.levelLapsToFinish)
		{
			return;
		}
		MainGame.laps[0] = (byte)(MainGame.levelLapsToFinish + 1);
		MainGame.displayLaps = MainGame.levelLapsToFinish;
		switch (MainGame.gameMode)
		{
		case 0:
			mainC.maingameMain.Set_SP_Level_To_Completed();
			break;
		case 1:
		{
			MainGame.raceFinished[0] = 1;
			mainC.networkingMain.XBOX_Send_Network_Message35(35);
			ushort num = 1;
			while (num < MainGame.maxGamePlayers && (!global::Players.Players.players[num].active || MainGame.raceFinished[num] != 0))
			{
				num++;
			}
			global::Players.Players.players[0].charP.velocity.v[0] = 0f;
			global::Players.Players.players[0].charP.velocity.v[1] = 0f;
			global::Players.Players.players[0].charP.velocity.v[2] = 0f;
			mainC.mapsMain.Get_Spawn_Point(ref global::Players.Players.players[0].charP.position, global::Players.Players.players[0].team, ref global::Players.Players.zRotation, (sbyte)global::Networking.Networking.networkPlayers[0].playerArrayPosition, checkForEnemy: true, global::Players.Players.playerRaces[global::Players.Players.players[0].race].spawnHeight[global::Players.Players.players[0].type], 0f);
			mainC.vehicles.Reset_Player_Vehicle_Variables(0);
			mainC.vehicles.Set_Vehicle_Position(ref MainGame.playerVehicles[0], global::Players.Players.players[0].charP.position.v[0], global::Players.Players.players[0].charP.position.v[1], global::Players.Players.players[0].charP.position.v[2], Vehicles.vehicles[global::Players.Players.players[0].curVehicle].data8, 0f, global::Players.Players.zRotation * ((float)Math.PI / 180f));
			Game_Reset_Joints_And_Programs();
			mainC.inputMain.GamePad_Vibration_Stop();
			break;
		}
		}
	}

	public void Game_Init_New_Player(byte playerID)
	{
		global::Players.Players.players[playerID].curVehicleIndex = 0;
		global::Players.Players.players[playerID].vehicles[0] = global::Players.Players.playerRaces[global::Players.Players.players[playerID].race].vehicleID[global::Players.Players.players[playerID].type];
		global::Players.Players.players[playerID].curVehicle = global::Players.Players.playerRaces[global::Players.Players.players[playerID].race].vehicleID[global::Players.Players.players[playerID].type];
		for (int i = 1; i < 3; i++)
		{
			global::Players.Players.players[playerID].vehicles[i] = global::Util.Util.maxUnsignedShortValue;
		}
		switch (MainGame.gameState)
		{
		case 141:
		case 142:
		case 143:
		case 144:
		case 145:
		case 148:
		case 149:
		{
			if (MainGame.gameMode != 1)
			{
				break;
			}
			mainC.soundsMain.Play_Sound_NonPositional("NewPlayer");
			if (!global::Networking.Networking.isHost)
			{
				break;
			}
			float num = 1f;
			if (global::Networking.Networking.networkSession.RemoteGamers.Count > 0)
			{
				num = 1f + ((float)global::Networking.Networking.networkSession.RemoteGamers.Count + 1f) / 4f * 2f;
				if (num > 3f)
				{
					num = 3f;
				}
			}
			mainC.pickupsMain.Update_Time_Modifier(num);
			break;
		}
		case 146:
		case 147:
			break;
		}
	}

	public void Game_Respawn_Last(byte threadID)
	{
		if (global::Weapons.Weapons.wp1[MainGame.playerVehicles[0].weapons[hudWeaponStub].weaponID].maxAmmo < 1)
		{
			MainGame.playerVehicles[0].weapons[hudWeaponStub].currentRounds = global::Players.Players.players[0].ammoClips[MainGame.playerVehicles[0].weapons[hudWeaponStub].curClip].count;
		}
		else
		{
			MainGame.playerVehicles[0].weapons[hudWeaponStub].currentRounds = global::Weapons.Weapons.wp1[MainGame.playerVehicles[0].weapons[hudWeaponStub].weaponID].maxAmmo;
		}
		global::Players.Players.currentView = global::Players.Players.lastView;
		ref Matrix reference = ref global::Players.Players.players[0].mv[1];
		reference = global::Players.Players.players[0].mv[0];
		global::Players.Players.enemyTeamMask = ~global::Players.Players.players[0].teamMask;
		global::Players.Players.freezeCamera = false;
		global::Players.Players.invincible = false;
		MainGame.viewAngle = 0f;
		MainGame.laserRange = 1f - global::Weapons.Weapons.wp1[global::Players.Players.players[0].primaryWeaponMountWeapon].fireRateAdjLowPerc;
		MainGame.laserRangeStart = global::Weapons.Weapons.wp1[global::Players.Players.players[0].primaryWeaponMountWeapon].fireRateAdjLowPerc;
		Game_Update_Camera(threadID);
		mainC.renderingMain.Set_Camera_To_Camera_Goal_Positions();
		global::Players.Players.players[0].playerModel[0] = playerModelTPV;
		if (global::Players.Players.currentView == 1)
		{
			global::Players.Players.players[0].playerModel[0] = playerModelFPV;
		}
		if (MainGame.gameMode == 0)
		{
			for (ushort num = MainGame.maxHumanGamePlayers; num < MainGame.maxGamePlayers; num++)
			{
				if (global::Players.Players.players[num].onmap == 4 && global::Players.Players.players[num].team != global::Players.Players.players[0].team)
				{
					mainC.mapsMain.Get_AI_Spawn_Point(ref global::Players.Players.players[num].charP.position, global::Players.Players.players[num].team, ref global::Players.Players.players[num].zRotation, MainGame.maxLocalPlayerSpawnPoint, checkForEnemy: false, global::Players.Players.playerRaces[global::Players.Players.players[num].race].spawnHeight[global::Players.Players.players[num].type]);
				}
			}
			global::Players.Players.players[0].textureID[0] = (ushort)mainC.texturesMain.Find_Texture("guard0_body3", 0);
			global::Players.Players.players[0].textureID[2] = (ushort)mainC.texturesMain.Find_Texture("guard0_body3_cap", 0);
			return;
		}
		for (ushort num = 0; num < 1; num++)
		{
			if (global::Networking.Networking.networkPlayers[num].haveRemotePlayerArrayPosition)
			{
				switch (global::Networking.Networking.networkPlayers[num].playerArrayPosition)
				{
				case 1:
					global::Players.Players.players[num].textureID[0] = (ushort)mainC.texturesMain.Find_Texture("guard0_body1", 0);
					global::Players.Players.players[num].textureID[2] = (ushort)mainC.texturesMain.Find_Texture("guard0_body1_cap", 0);
					break;
				case 2:
					global::Players.Players.players[num].textureID[0] = (ushort)mainC.texturesMain.Find_Texture("guard0_body2", 0);
					global::Players.Players.players[num].textureID[2] = (ushort)mainC.texturesMain.Find_Texture("guard0_body2_cap", 0);
					break;
				case 3:
					global::Players.Players.players[num].textureID[0] = (ushort)mainC.texturesMain.Find_Texture("guard0_body3", 0);
					global::Players.Players.players[num].textureID[2] = (ushort)mainC.texturesMain.Find_Texture("guard0_body3_cap", 0);
					break;
				default:
					global::Players.Players.players[num].textureID[0] = (ushort)mainC.texturesMain.Find_Texture("guard0_body", 0);
					global::Players.Players.players[num].textureID[2] = (ushort)mainC.texturesMain.Find_Texture("guard0_cap", 0);
					break;
				}
			}
		}
	}

	public void Game_Respawn_From_Network_Last(ushort playerID)
	{
		if (playerID < MainGame.maxHumanGamePlayers)
		{
			mainC.playersMain.Set_Player_Race((byte)playerID, 0, 1);
			global::Players.Players.players[playerID].playerModel[0] = playerModelTPV;
			switch (global::Networking.Networking.networkPlayers[playerID].playerArrayPosition)
			{
			case 1:
				global::Players.Players.players[playerID].textureID[0] = (ushort)mainC.texturesMain.Find_Texture("guard0_body1", 0);
				global::Players.Players.players[playerID].textureID[2] = (ushort)mainC.texturesMain.Find_Texture("guard0_body1_cap", 0);
				break;
			case 2:
				global::Players.Players.players[playerID].textureID[0] = (ushort)mainC.texturesMain.Find_Texture("guard0_body2", 0);
				global::Players.Players.players[playerID].textureID[2] = (ushort)mainC.texturesMain.Find_Texture("guard0_body2_cap", 0);
				break;
			case 3:
				global::Players.Players.players[playerID].textureID[0] = (ushort)mainC.texturesMain.Find_Texture("guard0_body3", 0);
				global::Players.Players.players[playerID].textureID[2] = (ushort)mainC.texturesMain.Find_Texture("guard0_body3_cap", 0);
				break;
			default:
				global::Players.Players.players[playerID].textureID[0] = (ushort)mainC.texturesMain.Find_Texture("guard0_body", 0);
				global::Players.Players.players[playerID].textureID[2] = (ushort)mainC.texturesMain.Find_Texture("guard0_cap", 0);
				break;
			}
		}
	}

	public void Game_Ammo_Clip_Loaded(ushort vhID, ushort playerID, ushort curStub)
	{
	}

	public void Game_Player_Over_Last(ushort playerID)
	{
		ushort pickupID;
		if (playerID == 0)
		{
			MainGame.needToLoadWeapons = true;
			global::Players.Players.currentView = global::Players.Players.lastView;
			global::Players.Players.players[0].playerModel[0] = playerModelTPV;
			float num = global::Players.Players.players[0].charP.position.v[0];
			float num2 = global::Players.Players.players[0].charP.position.v[1];
			float z = global::Players.Players.players[0].charP.position.v[2];
			float num3 = 360f / (float)(int)MainGame.playerVehicles[0].numWeapons;
			float num4 = 0f;
			for (pickupID = 0; pickupID < MainGame.playerVehicles[0].numWeapons; pickupID++)
			{
				float num5 = (float)Math.Sin(num4) * 2.5f * 1f;
				float num6 = (float)Math.Cos(num4) * 2.5f * 1f;
				if (MainGame.playerVehicles[0].weapons[pickupID].active && MainGame.playerVehicles[0].weapons[pickupID].weaponID != 6 && MainGame.playerVehicles[0].weapons[pickupID].weaponID != 1)
				{
					mainC.pickupsMain.Player_Drops_Weapon(MainGame.playerVehicles[0].weapons[pickupID].weaponID, num + num5, num2 + num6, z, sendToNetwork: true);
					mainC.vehicles.Remove_Weapon_From_Player_Vehicle(0, (byte)pickupID);
					if (MainGame.playerVehicles[0].mounts[MainGame.primaryWeaponMount].objectID == pickupID)
					{
						for (ushort num7 = 0; num7 < MainGame.playerVehicles[0].numWeapons; num7++)
						{
							if (MainGame.playerVehicles[0].weapons[num7].active && MainGame.playerVehicles[0].weapons[num7].weaponID == 6)
							{
								MainGame.playerVehicles[0].mounts[MainGame.primaryWeaponMount].objectID = (byte)num7;
								break;
							}
						}
					}
				}
				num4 += num3;
			}
		}
		if (global::Networking.Networking.isHost && mainC.pickupsMain.Find_First_Pickup_Of_Type_And_RefID(17, 6, 0, out pickupID))
		{
			pickupID++;
			while (pickupID < global::Pickups.Pickups.numPickups && mainC.pickupsMain.Find_First_Pickup_Of_Type_And_RefID(17, 6, pickupID, out pickupID))
			{
				global::Pickups.Pickups.pick1[pickupID].onmap = false;
				global::Pickups.Pickups.pick1[pickupID].enabled = false;
				mainC.pickupsMain.Send_Pickup_Acquired(pickupID);
				pickupID++;
			}
		}
	}

	public bool Game_Pickup_Type_14_SP(short id, ushort id2, short refID, short actionID)
	{
		return false;
	}

	public bool Game_Pickup_Type_14_MP(short id, ushort id2, short refID, short actionID)
	{
		return true;
	}

	public void Game_Scored_Kill()
	{
	}

	public void Game_Use_Perk()
	{
	}

	public void Game_Scored_Target()
	{
		mainC.soundsMain.Play_Sound_NonPositional("Scored_Objective");
		Game_Update_Objective_Count();
	}

	public void Game_Update_Objective_Count()
	{
	}

	public ushort Get_Objective_Count()
	{
		return 0;
	}

	public bool Game_Is_SP_Round_Over()
	{
		if (global::AI.AI.aiCompleted)
		{
			if (MainGame.gameLevel >= 18)
			{
				mainC.maingameMain.Set_SP_Level_To_Completed();
				return true;
			}
			if (!activatedPickup)
			{
				if (MainGame.gameLevel == 0)
				{
					mainC.userInterface.Set_All_Component_Status(7, 16, 0);
					mainC.userInterface.Set_Component_Status(7, 16, 5, 1);
					mainC.userInterface.Show_Window_Specified_Time(16, 16, resetButtons: false, 3f);
					mainC.userInterface.Ignore_Stick_Input(16);
				}
				mainC.soundsMain.Play_Sound_NonPositional("Level_Finished");
				mainC.pickupsMain.Activate_End_Of_Level_Pickup();
				mainC.pickupsMain.Set_Will_Respawn_False(2);
				mainC.pickupsMain.Set_Will_Respawn_False(16);
			}
			activatedPickup = true;
			return false;
		}
		activatedPickup = false;
		return false;
	}

	public bool Game_Is_MP_Round_Over()
	{
		if (MainGame.gameType == 2)
		{
			if (global::AI.AI.aiCompleted && MainGame.gameLevel >= 18)
			{
				return true;
			}
			return false;
		}
		if (global::Players.Players.teamPoints[0] >= MainGame.roundScoreLimit || global::Players.Players.teamPoints[2] >= MainGame.roundScoreLimit || global::Players.Players.teamPoints[3] >= MainGame.roundScoreLimit || global::Players.Players.teamPoints[4] >= MainGame.roundScoreLimit || MainGame.roundCurrentTime <= 0f)
		{
			return true;
		}
		return false;
	}

	public void Game_Reset_Perks_For_Death()
	{
		MainGame.achievementRewards[0].rewardTimer = 0f;
		MainGame.achievementRewards[1].rewardTimer = 0f;
		MainGame.achievementRewards[2].rewardTimer = 0f;
		MainGame.achievementRewards[2].status = 0;
		MainGame.achievementRewards[3].status = 0;
	}

	public void Game_New_Weapon_Picked_Up(byte weaponStub)
	{
		if (mainC.vehicles.Get_Player_Vehicle_Primary_Weapon_Mount_Stub(0) != weaponStub && !MainGame.sprinting && global::Players.Players.players[0].animations[global::Players.Players.players[0].programSwitchWeapons].status < 2)
		{
			global::Players.Players.changingWeapons = true;
			mainC.weaponsMain.Check_Weapon_Views();
			if (global::Players.Players.players[0].primaryWeaponMountWeapon == MainGame.playerVehicles[0].weapons[0].weaponID)
			{
				global::Players.Players.players[0].animations[global::Players.Players.players[0].programSwitchWeapons].var1 = MainGame.playerVehicles[0].weapons[2].weaponID;
			}
			else
			{
				global::Players.Players.players[0].animations[global::Players.Players.players[0].programSwitchWeapons].var1 = MainGame.playerVehicles[0].weapons[0].weaponID;
			}
			global::Players.Players.players[0].animations[global::Players.Players.players[0].programSwitchWeapons].callBackType = 12;
			global::Players.Players.players[0].animations[global::Players.Players.players[0].programSwitchWeapons].callBack = MainGame.primaryWeaponMount;
			mainC.programsMain.Start_Animation(0, ref global::Players.Players.players[0].jt1, ref global::Players.Players.players[0].animations, global::Players.Players.players[0].programCollection, global::Players.Players.players[0].programSwitchWeapons, 1f, 1f);
		}
	}

	public void Game_Render_HUD()
	{
		Vector2 position = default(Vector2);
		Rectangle destinationRectangle = default(Rectangle);
		switch (MainGame.gameState)
		{
		case 6:
		case 7:
		case 8:
			return;
		}
		if (!global::Rendering.Rendering.renderHud || global::Players.Players.currentView == 3)
		{
			return;
		}
		try
		{
			byte rBufferID = global::Rendering.Rendering.rBufferID;
			global::Rendering.Rendering.splashSprite.Begin();
			position.Y = 72f;
			int num;
			for (byte b = 0; b < global::Players.Players.numRankedPlayers[rBufferID]; b++)
			{
				num = global::Players.Players.playerRankings[b, rBufferID];
				if (global::Players.Players.players[num].id > -1 && mainC.playersMain.Get_Player_Index(global::Players.Players.players[num].id, -1) > -1)
				{
					position.X = 128f;
					global::Rendering.Rendering.splashSprite.Draw(global::Networking.Networking.networkPlayers[num].gamerPicture, position, Color.White);
					position.X = 200f;
					position.Y += 16f;
					if (global::Networking.Networking.networkPlayers[num].playerLoaded)
					{
						mainC.fontmoduleMain.Draw_String_Centered_Vertically(global::Players.Players.players[num].abreviateName, ref position, ref colorWhite, 1);
						position.Y += 25f;
						mainC.fontmoduleMain.Draw_String_Centered_Vertically(MainGame.gameData.players[num].scoresI[0].ToString(CultureInfo.InvariantCulture), ref position, ref colorWhite, 1);
						position.Y += 39f;
					}
					else
					{
						mainC.fontmoduleMain.Draw_String_Centered_Vertically(loadingMsg, ref position, ref colorWhite, 1);
						position.Y += 64f;
					}
				}
			}
			if (MainGame.gameMode == 0)
			{
				position.X = 1091f;
				position.Y = 524f;
				for (num = 0; num < MainGame.numLives; num++)
				{
					global::Rendering.Rendering.splashSprite.Draw(global::Textures.Textures.texMain.texData[texHudLife], position, Color.White);
					position.X -= 46f;
				}
			}
			if (global::AI.AI.numAiKillsForLevelToEnd > 0)
			{
				position.Y = 494f;
				num = (int)((float)(int)global::AI.AI.numAiKillsForLevelToEnd * global::AI.AI.killCountScale - (float)(int)global::AI.AI.levelKillCount);
				if (global::AI.AI.aiCompleted)
				{
					num = 0;
				}
				if (num > 0 || !global::AI.AI.aiCompleted)
				{
					position.X = 141f;
					showZombiesTimer = 0f;
					showZombiesPosition = 0f;
					if (num > 0)
					{
						mainC.utilMain.Get_Digit_Indexes(num);
					}
					else
					{
						mainC.utilMain.Get_Digit_Indexes(0);
					}
				}
				else
				{
					mainC.utilMain.Get_Digit_Indexes(0);
					showZombiesTimer += MainGame.frametime;
					if (showZombiesTimer < 0.25f)
					{
						showZombiesPosition += showZombiesTimer * 25f;
					}
					else if (showZombiesTimer > 2f)
					{
						showZombiesTimer = 2f;
					}
					else
					{
						showZombiesPosition -= showZombiesTimer * 40f;
						if (showZombiesPosition < -250f)
						{
							showZombiesPosition = -250f;
						}
					}
					position.X = 141f + showZombiesPosition;
				}
				global::Rendering.Rendering.splashSprite.Draw(global::Textures.Textures.texMain.texData[texHudZombies], position, Color.White);
				position.X = 178f + (float)(int)global::Util.Util.numDigits * 19f + showZombiesPosition;
				position.Y = 514f;
				for (num = 0; num < global::Util.Util.numDigits; num++)
				{
					global::Rendering.Rendering.splashSprite.Draw(global::Textures.Textures.texMain.texData[global::Textures.Textures.texDigits[global::Util.Util.digits[num]]], position, Color.White);
					position.X -= 19f;
				}
			}
			num = (int)Math.Round(184f * (1f - global::Players.Players.players[0].damagePercentageCapped));
			destinationRectangle.Width = num;
			if (destinationRectangle.Width > 184)
			{
				destinationRectangle.Height = 184;
			}
			destinationRectangle.X = 954;
			destinationRectangle.Y = 599;
			destinationRectangle.Height = 20;
			global::Rendering.Rendering.splashSprite.Draw(global::Textures.Textures.texMain.texData[texHudHealthBar], destinationRectangle, Color.White);
			position.X = 905f;
			position.Y = 589f;
			global::Rendering.Rendering.splashSprite.Draw(global::Textures.Textures.texMain.texData[texHudHealthIcon], position, Color.White);
			position.X = 88f;
			position.Y = 499f;
			num = MainGame.playerVehicles[0].weapons[1].currentRounds;
			position.X = 189f;
			position.Y = 545f;
			for (int i = 0; i < num; i++)
			{
				global::Rendering.Rendering.splashSprite.Draw(global::Textures.Textures.texMain.texData[texHudGrenade], position, Color.White);
				position.X += 34f;
			}
			if (global::AI.AI.numAiKillsForLevelToEnd > 0 && global::Players.Players.players[0].onmap == 4)
			{
				Vector3 source = default(Vector3);
				if (mainC.pickupsMain.Find_Nearest_Pickup_Of_Type(6, global::Players.Players.players[0].posX[global::Rendering.Rendering.rBufferID], global::Players.Players.players[0].posY[global::Rendering.Rendering.rBufferID], global::Players.Players.players[0].posZ[global::Rendering.Rendering.rBufferID], out source.X, out source.Y, out source.Z, out var pickupDistance))
				{
					source.Z += 3f;
					global::Rendering.Rendering.rgtV2 = global::Rendering.Rendering.rGraphics.Viewport.Project(source, global::Rendering.Rendering.matrixP, global::Rendering.Rendering.matrixV, global::Rendering.Rendering.matrixI);
					if (global::Rendering.Rendering.rgtV2.Z < 1f)
					{
						mainC.utilMain.Get_Digit_Indexes((int)pickupDistance);
						position.X = global::Rendering.Rendering.rgtV2.X;
						position.Y = global::Rendering.Rendering.rgtV2.Y;
						global::Rendering.Rendering.splashSprite.Draw(global::Textures.Textures.texMain.texData[texHudWaypoint], position, Color.White);
						position.X += 20 * global::Util.Util.numDigits;
						position.Y -= 6f;
						for (num = 0; num < global::Util.Util.numDigits; num++)
						{
							global::Rendering.Rendering.splashSprite.Draw(global::Textures.Textures.texMain.texData[global::Textures.Textures.texDigits[global::Util.Util.digits[num]]], position, Color.White);
							position.X -= 19f;
						}
					}
				}
			}
			if (global::Weapons.Weapons.ammo[global::Weapons.Weapons.ammoClips[MainGame.playerVehicles[0].weapons[hudWeaponStub].curClip].ammoIndex].type != 9 && global::Weapons.Weapons.ammo[global::Weapons.Weapons.ammoClips[MainGame.playerVehicles[0].weapons[hudWeaponStub].curClip].ammoIndex].type != 15)
			{
				if (MainGame.playerVehicles[0].weapons[hudWeaponStub].weaponID == 6 || MainGame.playerVehicles[0].weapons[hudWeaponStub].weaponID == 9)
				{
					global::Players.Players.players[0].ammoClips[MainGame.playerVehicles[0].weapons[hudWeaponStub].curClip].numClips = 4;
				}
				int i = global::Players.Players.players[0].ammoClips[MainGame.playerVehicles[0].weapons[hudWeaponStub].curClip].numClips * global::Players.Players.players[0].ammoClips[MainGame.playerVehicles[0].weapons[hudWeaponStub].curClip].count + global::Players.Players.players[0].ammoClips[MainGame.playerVehicles[0].weapons[hudWeaponStub].curClip].surplus;
				num = i / 100;
				int num2 = (i - num * 100) / 10;
				int num3 = i - num * 100 - num2 * 10;
				if (num > 9)
				{
					num = 9;
				}
				if (num2 > 9)
				{
					num2 = 9;
				}
				if (num3 > 9)
				{
					num3 = 9;
				}
				position.X = 131f;
				position.Y = 583f;
				if (MainGame.playerVehicles[0].weapons[hudWeaponStub].weaponID == 10)
				{
					global::Rendering.Rendering.splashSprite.Draw(global::Textures.Textures.texMain.texData[texHudAmmoShell], position, Color.White);
				}
				else
				{
					global::Rendering.Rendering.splashSprite.Draw(global::Textures.Textures.texMain.texData[texHudAmmoClip], position, Color.White);
				}
				position.X = 189f;
				position.Y = 598f;
				if (num > 0)
				{
					global::Rendering.Rendering.splashSprite.Draw(global::Textures.Textures.texMain.texData[global::Textures.Textures.texDigits[num]], position, Color.White);
					switch (num)
					{
					case 1:
						position.X += 15f;
						break;
					case 4:
						position.X += 17f;
						break;
					default:
						position.X += 19f;
						break;
					}
				}
				if (num2 > 0 || num > 0)
				{
					global::Rendering.Rendering.splashSprite.Draw(global::Textures.Textures.texMain.texData[global::Textures.Textures.texDigits[num2]], position, Color.White);
					switch (num2)
					{
					case 1:
						position.X += 15f;
						break;
					case 4:
						position.X += 17f;
						break;
					default:
						position.X += 19f;
						break;
					}
				}
				global::Rendering.Rendering.splashSprite.Draw(global::Textures.Textures.texMain.texData[global::Textures.Textures.texDigits[num3]], position, Color.White);
				position.X = 266f;
				if (i < 100)
				{
					position.X = 251f;
				}
				num = MainGame.playerVehicles[0].weapons[hudWeaponStub].currentRounds;
				position.Y = 604f;
				for (byte b = 0; b < num; b++)
				{
					global::Rendering.Rendering.splashSprite.Draw(global::Textures.Textures.texMain.texData[texHudAmmoBullet], position, Color.White);
					position.X += 7f;
				}
			}
			global::Rendering.Rendering.splashSprite.End();
		}
		catch (Exception)
		{
			global::InputHandler.InputHandler.tw = 1f;
			try
			{
				global::Rendering.Rendering.splashSprite.End();
			}
			catch
			{
			}
		}
	}

	public void Game_SP_Start_Of_Update()
	{
		Game_Start_Of_Update_Common();
	}

	public void Game_MP_Start_Of_Update()
	{
		Game_Start_Of_Update_Common();
	}

	public void Game_Start_Of_Update_Common()
	{
	}

	public void Game_SP_End_Of_Update(byte threadID)
	{
		Game_End_Of_Update_Common();
		Game_Is_SP_Round_Over();
	}

	public void Game_MP_End_Of_Update(byte threadID)
	{
		if (MainGame.gameType == 7)
		{
			MainGame.roundCurrentTime -= MainGame.frametime;
		}
		else if (MainGame.gameType == 2)
		{
			if (global::AI.AI.aiCompleted)
			{
				if (!activatedPickup)
				{
					if (MainGame.gameLevel == 0)
					{
						mainC.userInterface.Set_All_Component_Status(7, 16, 0);
						mainC.userInterface.Set_Component_Status(7, 16, 5, 1);
						mainC.userInterface.Show_Window_Specified_Time(16, 16, resetButtons: false, 3f);
						mainC.userInterface.Ignore_Stick_Input(16);
					}
					mainC.soundsMain.Play_Sound_NonPositional("Level_Finished");
					mainC.pickupsMain.Activate_End_Of_Level_Pickup();
					mainC.pickupsMain.Set_Will_Respawn_False(2);
					mainC.pickupsMain.Set_Will_Respawn_False(16);
				}
				activatedPickup = true;
			}
			else
			{
				activatedPickup = false;
			}
		}
		for (ushort num = 0; num < global::AI.AI.numAI; num++)
		{
			short playerID;
			if (global::AI.AI.ais[num].locallyControlled && (playerID = global::AI.AI.ais[num].playerID) > -1 && global::Players.Players.players[playerID].onmap == 4)
			{
				targetTimer[playerID] += MainGame.frametime;
				if (targetTimer[playerID] > 1.5f)
				{
					targetTimer[playerID] = 0f;
					global::AI.AI.ais[num].targetID = (short)mainC.playersMain.Find_Closest_Player(0, 1, (ushort)playerID, 0, global::Players.Players.players[playerID].charP.position.v[0], global::Players.Players.players[playerID].charP.position.v[1], global::Players.Players.players[playerID].charP.position.v[2], global::AI.AI.ais[num].targetID);
				}
			}
		}
		Game_End_Of_Update_Common();
		if (skipLevel)
		{
			skipLevel = false;
			mainC.networkingMain.XBOX_MP_Round_Over();
		}
		if (global::Networking.Networking.isHost && Game_Is_MP_Round_Over())
		{
			mainC.networkingMain.XBOX_MP_Round_Over();
		}
	}

	public void Game_End_Of_Update_Common()
	{
		if (global::Players.Players.players[0].onmap == 4 && global::Players.Players.players[0].charP.position.v[2] < -1f)
		{
			switch (MainGame.gameMode)
			{
			case 0:
				if (MainGame.useFixedSpawnPoint)
				{
					mainC.mapsMain.Get_Spawn_Point(ref global::Players.Players.players[0].charP.position, global::Players.Players.players[0].team, ref global::Players.Players.zRotation, MainGame.maxLocalPlayerSpawnPoint, checkForEnemy: false, global::Players.Players.playerRaces[global::Players.Players.players[0].race].spawnHeight[global::Players.Players.players[0].type], 0f);
				}
				else
				{
					mainC.mapsMain.Get_Spawn_Point(ref global::Players.Players.players[0].charP.position, global::Players.Players.players[0].team, ref global::Players.Players.zRotation, -1, MainGame.spSpawnCheckForEnemy, global::Players.Players.playerRaces[global::Players.Players.players[0].race].spawnHeight[global::Players.Players.players[0].type], global::Players.Players.playerRaces[global::Players.Players.players[0].race].boundingRadius[global::Players.Players.players[0].type]);
				}
				break;
			case 1:
				if (MainGame.useFixedSpawnPoint)
				{
					mainC.mapsMain.Get_Spawn_Point(ref global::Players.Players.players[0].charP.position, global::Players.Players.players[0].team, ref global::Players.Players.zRotation, (sbyte)global::Networking.Networking.networkPlayers[0].playerArrayPosition, checkForEnemy: false, global::Players.Players.playerRaces[global::Players.Players.players[0].race].spawnHeight[global::Players.Players.players[0].type], 0f);
				}
				else
				{
					mainC.mapsMain.Get_Spawn_Point(ref global::Players.Players.players[0].charP.position, global::Players.Players.players[0].team, ref global::Players.Players.zRotation, -1, checkForEnemy: true, global::Players.Players.playerRaces[global::Players.Players.players[0].race].spawnHeight[global::Players.Players.players[0].type], global::Players.Players.playerRaces[global::Players.Players.players[0].race].boundingRadius[global::Players.Players.players[0].type]);
				}
				break;
			}
			global::Players.Players.players[0].posX[0] = global::Players.Players.players[0].charP.position.v[0];
			global::Players.Players.players[0].posY[0] = global::Players.Players.players[0].charP.position.v[1];
			global::Players.Players.players[0].posZ[0] = global::Players.Players.players[0].charP.position.v[2];
			global::Players.Players.players[0].posX[1] = global::Players.Players.players[0].charP.position.v[0];
			global::Players.Players.players[0].posY[1] = global::Players.Players.players[0].charP.position.v[1];
			global::Players.Players.players[0].posZ[1] = global::Players.Players.players[0].charP.position.v[2];
			mainC.vehicles.Set_Vehicle_Position(ref MainGame.playerVehicles[0], global::Players.Players.players[0].charP.position.v[0], global::Players.Players.players[0].charP.position.v[1], global::Players.Players.players[0].charP.position.v[2], 0f, 0f, global::Players.Players.zRotation * ((float)Math.PI / 180f));
		}
		mainC.aiMain.Check_AI_To_See_If_Fell_Through_Map(-1f);
	}

	public void Game_End_Of_Avatar_Update()
	{
	}

	public void Game_Update_Camera(byte threadID)
	{
		float y = 0f;
		Matrix identity = Matrix.Identity;
		byte uBufferID = global::Rendering.Rendering.uBufferID;
		_ = global::Players.Players.xRotation;
		global::Rendering.Rendering.renderMainPlayer[uBufferID] = true;
		global::Rendering.Rendering.cameraSpeed = MainGame.playerVehicles[0].velocity;
		global::Rendering.Rendering.camerObjectSpeed = MainGame.playerVehicles[0].velocity;
		MainGame.cameraMovementSpeed = 5500f / global::Physics.Physics.timeMod;
		MainGame.cameraObjectMovementSpeed = 5200f / global::Physics.Physics.timeMod;
		identity = Matrix.CreateRotationZ(global::Players.Players.zRotation * ((float)Math.PI / 180f)) * Matrix.CreateTranslation(global::Players.Players.players[global::Rendering.Rendering.watchingPlayer].charP.position.v[0], global::Players.Players.players[global::Rendering.Rendering.watchingPlayer].charP.position.v[1], global::Players.Players.players[global::Rendering.Rendering.watchingPlayer].charP.position.v[2]);
		if (MainGame.viewChanged)
		{
			MainGame.viewAngle = 0f;
			MainGame.viewChanged = false;
		}
		if (MainGame.viewFollowingObject)
		{
			global::Rendering.Rendering.projectionNearPlane[uBufferID] = 13.5f;
			if (global::Weapons.Weapons.projectileViewTimer)
			{
				byte rBufferID = global::Rendering.Rendering.rBufferID;
				if (global::Weapons.Weapons.viewFollowingTimer < 0.75f)
				{
					global::Rendering.Rendering.camObjectGoal[uBufferID].X = global::Rendering.Rendering.camObjectGoal[rBufferID].X;
					global::Rendering.Rendering.camObjectGoal[uBufferID].Y = global::Rendering.Rendering.camObjectGoal[rBufferID].Y;
					global::Rendering.Rendering.camObjectGoal[uBufferID].Z = global::Rendering.Rendering.camObjectGoal[rBufferID].Z;
					global::Rendering.Rendering.camPosGoal[uBufferID].X = global::Rendering.Rendering.camPosGoal[rBufferID].X;
					global::Rendering.Rendering.camPosGoal[uBufferID].Y = global::Rendering.Rendering.camPosGoal[rBufferID].Y;
					global::Rendering.Rendering.camPosGoal[uBufferID].Z = global::Rendering.Rendering.camPosGoal[rBufferID].Z;
					global::Rendering.Rendering.camUp[uBufferID].X = global::Rendering.Rendering.camUp[rBufferID].X;
					global::Rendering.Rendering.camUp[uBufferID].Y = global::Rendering.Rendering.camUp[rBufferID].Y;
					global::Rendering.Rendering.camUp[uBufferID].Z = global::Rendering.Rendering.camUp[rBufferID].Z;
				}
				global::Weapons.Weapons.viewFollowingTimer -= MainGame.frametime / global::Physics.Physics.timeMod;
				if (global::Weapons.Weapons.viewFollowingTimer < 0f)
				{
					global::Weapons.Weapons.projectileViewTimer = false;
					MainGame.viewFollowingObject = false;
					global::Rendering.Rendering.moveViewToNewLocation = true;
					MainGame.showCrossHairs[3] = 0;
					MainGame.viewAngle = 0f;
					global::Players.Players.controlsInUse = false;
					mainC.inputMain.UI_HUD_Show_Exit_View_Message(showMessage: false);
					mainC.inputMain.UI_HUD_Show_Guided_Bomb_Message(showMessage: false);
				}
			}
		}
		if (!global::Players.Players.freezeCamera)
		{
			if (!MainGame.overheadView && !global::Players.Players.players[global::Rendering.Rendering.watchingPlayer].taunting)
			{
				switch (global::Players.Players.currentView)
				{
				case 0:
					if (!global::Players.Players.players[0].dead)
					{
						if (global::Rendering.Rendering.viewPositionX == 0f)
						{
							global::Rendering.Rendering.viewPositionX = global::Weapons.Weapons.recoilSide * 1.3f + Explosions.cameraShakeX;
							global::Rendering.Rendering.viewVelocityX = Math.Abs(global::Rendering.Rendering.viewPositionX) / 0.083f * (float)(-Math.Sign(global::Rendering.Rendering.viewPositionX));
						}
						if (global::Rendering.Rendering.viewPositionY == 0f)
						{
							global::Rendering.Rendering.viewPositionY = global::Weapons.Weapons.recoilUp * 1.3f + Explosions.cameraShakeY;
							global::Rendering.Rendering.viewVelocityY = Math.Abs(global::Rendering.Rendering.viewPositionY) / 0.083f * (float)(-Math.Sign(global::Rendering.Rendering.viewPositionY));
						}
						if ((global::Players.Players.players[global::Rendering.Rendering.watchingPlayer].onmap & 0xC) == 0 || global::Players.Players.thirdPersonViewDistanceSqr[uBufferID] < 0.01f)
						{
							global::Rendering.Rendering.renderMainPlayer[uBufferID] = false;
						}
						identity = Matrix.CreateTranslation(global::Players.Players.thirdPersonXAdj, global::Players.Players.thirdPersonYAdj, global::Players.Players.thirdPersonZAdj) * Matrix.CreateRotationX(global::Players.Players.players[global::Rendering.Rendering.watchingPlayer].xRotation * ((float)Math.PI / 180f)) * Matrix.CreateRotationZ(global::Players.Players.players[global::Rendering.Rendering.watchingPlayer].zRotation * ((float)Math.PI / 180f)) * Matrix.CreateTranslation(global::Players.Players.players[global::Rendering.Rendering.watchingPlayer].charP.position.v[0], global::Players.Players.players[global::Rendering.Rendering.watchingPlayer].charP.position.v[1], global::Players.Players.players[global::Rendering.Rendering.watchingPlayer].charP.position.v[2] + global::Players.Players.players[0].jt1[0].mv[0].M43);
						global::Rendering.Rendering.camPosGoal[uBufferID].X = identity.M41;
						global::Rendering.Rendering.camPosGoal[uBufferID].Y = identity.M42;
						global::Rendering.Rendering.camPosGoal[uBufferID].Z = identity.M43;
						global::Rendering.Rendering.camObjectGoal[uBufferID].X = identity.M21 + identity.M41;
						global::Rendering.Rendering.camObjectGoal[uBufferID].Y = identity.M22 + identity.M42;
						global::Rendering.Rendering.camObjectGoal[uBufferID].Z = identity.M23 + identity.M43;
						global::Rendering.Rendering.camUp[uBufferID].X = identity.M31;
						global::Rendering.Rendering.camUp[uBufferID].Y = identity.M32;
						global::Rendering.Rendering.camUp[uBufferID].Z = identity.M33;
						global::Rendering.Rendering.curWeaponViewTime -= MainGame.frametime / global::Physics.Physics.timeMod;
						if (global::Rendering.Rendering.curWeaponViewTime < 0f)
						{
							global::Rendering.Rendering.curWeaponViewTime = 0f;
						}
						float num3 = global::Rendering.Rendering.curWeaponViewTime / global::Rendering.Rendering.weaponViewTime;
						global::Rendering.Rendering.scopeValue = num3 * global::Rendering.Rendering.lastWeaponViewValue + (1f - num3) * ((float)Math.PI / 4f);
					}
					else
					{
						global::Rendering.Rendering.camPosGoal[uBufferID].X = global::Players.Players.players[global::Rendering.Rendering.watchingPlayer].charP.position.v[0] - identity.M21 * 10f;
						global::Rendering.Rendering.camPosGoal[uBufferID].Y = global::Players.Players.players[global::Rendering.Rendering.watchingPlayer].charP.position.v[1] - identity.M22 * 10f;
						global::Rendering.Rendering.camPosGoal[uBufferID].Z = global::Players.Players.players[global::Rendering.Rendering.watchingPlayer].charP.position.v[2] + 10f;
						global::Rendering.Rendering.camObjectGoal[uBufferID].X = global::Players.Players.players[global::Rendering.Rendering.watchingPlayer].charP.position.v[0];
						global::Rendering.Rendering.camObjectGoal[uBufferID].Y = global::Players.Players.players[global::Rendering.Rendering.watchingPlayer].charP.position.v[1];
						global::Rendering.Rendering.camObjectGoal[uBufferID].Z = global::Players.Players.players[global::Rendering.Rendering.watchingPlayer].charP.position.v[2] + 5f;
						global::Rendering.Rendering.camUp[uBufferID].X = identity.M31;
						global::Rendering.Rendering.camUp[uBufferID].Y = identity.M32;
						global::Rendering.Rendering.camUp[uBufferID].Z = identity.M33;
					}
					MainGame.showCrossHairs[2] = 0;
					global::Rendering.Rendering.projectionNearPlane[uBufferID] = 0.1f;
					break;
				case 1:
					if (!global::Players.Players.players[0].dead)
					{
						float num3 = (global::Players.Players.xRotation + 60f) / 120f * (float)(global::Programs.Programs.pgC[1].pg1[0].numSteps - 1);
						ushort num4 = (ushort)num3;
						if (num3 == (float)(int)num4)
						{
							num3 = 1f;
						}
						else
						{
							num3 -= (float)(int)num4;
							num4++;
						}
						mainC.programsMain.Set_Joints_To_Program_Step_Percentage(ref global::Rendering.Rendering.cameraJoint, ref global::Rendering.Rendering.cameraProgram, 1, 0, (short)num4, num3, loop: false);
						global::Joints.Joints.Translate_Joints(ref global::Rendering.Rendering.cameraJoint, ref global::Rendering.Rendering.cameraJointCollection);
						if ((global::Players.Players.players[global::Rendering.Rendering.watchingPlayer].onmap & 0xC) == 0)
						{
							global::Rendering.Rendering.renderMainPlayer[uBufferID] = false;
						}
						global::Rendering.Rendering.curWeaponViewTime -= MainGame.frametime / global::Physics.Physics.timeMod;
						if (global::Rendering.Rendering.curWeaponViewTime < 0f)
						{
							global::Rendering.Rendering.curWeaponViewTime = 0f;
						}
						float num5 = global::Rendering.Rendering.curWeaponViewTime / global::Rendering.Rendering.weaponViewTime;
						global::Rendering.Rendering.scopeValue = num5 * global::Rendering.Rendering.lastWeaponViewValue + (1f - num5) * ((float)Math.PI / 4f);
						if (global::Rendering.Rendering.viewPositionX == 0f)
						{
							global::Rendering.Rendering.viewPositionX = global::Weapons.Weapons.recoilSide + Explosions.cameraShakeX;
							global::Rendering.Rendering.viewVelocityX = Math.Abs(global::Rendering.Rendering.viewPositionX) / 0.083f * (float)(-Math.Sign(global::Rendering.Rendering.viewPositionX));
						}
						if (global::Rendering.Rendering.viewPositionY == 0f)
						{
							global::Rendering.Rendering.viewPositionY = global::Weapons.Weapons.recoilUp + Explosions.cameraShakeY;
							global::Rendering.Rendering.viewVelocityY = Math.Abs(global::Rendering.Rendering.viewPositionY) / 0.083f * (float)(-Math.Sign(global::Rendering.Rendering.viewPositionY));
						}
						if (global::Players.Players.players[global::Rendering.Rendering.watchingPlayer].onmap != 8)
						{
							num3 = global::Players.Players.firstPersonViewX + num5 * global::Rendering.Rendering.viewAdjX;
							y = global::Players.Players.firstPersonViewY + num5 * global::Rendering.Rendering.viewAdjY;
							float num6 = global::Players.Players.firstPersonViewZ + num5 * global::Rendering.Rendering.viewAdjZ;
							if (global::Players.Players.players[0].playerIsMoving == 8)
							{
								num3 += global::Rendering.Rendering.gunRunAdjX;
								y += global::Rendering.Rendering.gunRunAdjY;
								num6 += global::Rendering.Rendering.gunRunAdjZ;
							}
							identity = Matrix.CreateTranslation(num3, y, num6) * global::Rendering.Rendering.cameraJoint[2].mv[uBufferID] * Matrix.CreateRotationZ(global::Players.Players.zRotation * ((float)Math.PI / 180f)) * Matrix.CreateTranslation(global::Players.Players.players[global::Rendering.Rendering.watchingPlayer].charP.position.v[0], global::Players.Players.players[global::Rendering.Rendering.watchingPlayer].charP.position.v[1], global::Players.Players.players[global::Rendering.Rendering.watchingPlayer].charP.position.v[2]);
						}
						else
						{
							identity = Matrix.CreateTranslation(global::Players.Players.thirdPersonXAdj, global::Players.Players.thirdPersonYAdj, global::Players.Players.thirdPersonZAdj) * Matrix.CreateRotationX(global::Players.Players.xRotation * ((float)Math.PI / 180f)) * Matrix.CreateRotationZ(global::Players.Players.zRotation * ((float)Math.PI / 180f)) * Matrix.CreateTranslation(global::Players.Players.players[global::Rendering.Rendering.watchingPlayer].charP.position.v[0], global::Players.Players.players[global::Rendering.Rendering.watchingPlayer].charP.position.v[1], global::Players.Players.players[global::Rendering.Rendering.watchingPlayer].charP.position.v[2]);
							global::Players.Players.players[0].playerModel[0] = playerModelTPV;
						}
						global::Rendering.Rendering.camPosGoal[uBufferID].X = identity.M41;
						global::Rendering.Rendering.camPosGoal[uBufferID].Y = identity.M42;
						global::Rendering.Rendering.camPosGoal[uBufferID].Z = identity.M43;
						global::Rendering.Rendering.camObjectGoal[uBufferID].X = identity.M21 + identity.M41;
						global::Rendering.Rendering.camObjectGoal[uBufferID].Y = identity.M22 + identity.M42;
						global::Rendering.Rendering.camObjectGoal[uBufferID].Z = identity.M23 + identity.M43;
						global::Rendering.Rendering.camUp[uBufferID].X = identity.M31;
						global::Rendering.Rendering.camUp[uBufferID].Y = identity.M32;
						global::Rendering.Rendering.camUp[uBufferID].Z = identity.M33;
					}
					else
					{
						global::Rendering.Rendering.camPosGoal[uBufferID].X = global::Players.Players.players[global::Rendering.Rendering.watchingPlayer].charP.position.v[0] - identity.M21 * 10f;
						global::Rendering.Rendering.camPosGoal[uBufferID].Y = global::Players.Players.players[global::Rendering.Rendering.watchingPlayer].charP.position.v[1] - identity.M22 * 10f;
						global::Rendering.Rendering.camPosGoal[uBufferID].Z = global::Players.Players.players[global::Rendering.Rendering.watchingPlayer].charP.position.v[2] + 10f;
						global::Rendering.Rendering.camObjectGoal[uBufferID].X = global::Players.Players.players[global::Rendering.Rendering.watchingPlayer].charP.position.v[0];
						global::Rendering.Rendering.camObjectGoal[uBufferID].Y = global::Players.Players.players[global::Rendering.Rendering.watchingPlayer].charP.position.v[1];
						global::Rendering.Rendering.camObjectGoal[uBufferID].Z = global::Players.Players.players[global::Rendering.Rendering.watchingPlayer].charP.position.v[2] + 5f;
						global::Rendering.Rendering.camUp[uBufferID].X = identity.M31;
						global::Rendering.Rendering.camUp[uBufferID].Y = identity.M32;
						global::Rendering.Rendering.camUp[uBufferID].Z = identity.M33;
					}
					MainGame.showCrossHairs[2] = 0;
					global::Rendering.Rendering.projectionNearPlane[uBufferID] = 0.1f;
					break;
				case 2:
				{
					if (global::Rendering.Rendering.viewPositionX == 0f)
					{
						global::Rendering.Rendering.viewPositionX = global::Weapons.Weapons.recoilSide + Explosions.cameraShakeX;
						global::Rendering.Rendering.viewVelocityX = Math.Abs(global::Rendering.Rendering.viewPositionX) / 0.083f * (float)(-Math.Sign(global::Rendering.Rendering.viewPositionX));
					}
					if (global::Rendering.Rendering.viewPositionY == 0f)
					{
						global::Rendering.Rendering.viewPositionY = global::Weapons.Weapons.recoilUp + Explosions.cameraShakeY;
						global::Rendering.Rendering.viewVelocityY = Math.Abs(global::Rendering.Rendering.viewPositionY) / 0.083f * (float)(-Math.Sign(global::Rendering.Rendering.viewPositionY));
					}
					if (global::Rendering.Rendering.viewPositionZ == 0f)
					{
						global::Rendering.Rendering.viewPositionZ = global::Weapons.Weapons.recoilBack + Explosions.cameraShakeZ;
						global::Rendering.Rendering.viewVelocityZ = Math.Abs(global::Rendering.Rendering.viewPositionZ) / 0.083f * (float)(-Math.Sign(global::Rendering.Rendering.viewPositionZ));
					}
					global::Rendering.Rendering.curWeaponViewTime += MainGame.frametime / global::Physics.Physics.timeMod;
					if (global::Rendering.Rendering.curWeaponViewTime > global::Rendering.Rendering.weaponViewTime)
					{
						global::Rendering.Rendering.curWeaponViewTime = global::Rendering.Rendering.weaponViewTime;
					}
					float num3 = global::Rendering.Rendering.curWeaponViewTime / global::Rendering.Rendering.weaponViewTime;
					y = 1f - num3;
					global::Rendering.Rendering.lastWeaponViewValue = 0.5235987f;
					global::Rendering.Rendering.scopeValue = y * ((float)Math.PI / 4f) + num3 * global::Rendering.Rendering.lastWeaponViewValue;
					identity = Matrix.CreateTranslation(ironSightsAdjX * y + global::Players.Players.ironSightsViewX * num3, ironSightsAdjY * y + global::Players.Players.ironSightsViewY * num3, ironSightsAdjZ * y + global::Players.Players.ironSightsViewZ * num3) * MainGame.playerVehicles[0].mounts[MainGame.primaryWeaponMount].mvCurrent[uBufferID] * Matrix.CreateTranslation(global::Players.Players.players[global::Rendering.Rendering.watchingPlayer].charP.position.v[0], global::Players.Players.players[global::Rendering.Rendering.watchingPlayer].charP.position.v[1], global::Players.Players.players[global::Rendering.Rendering.watchingPlayer].charP.position.v[2]);
					global::Rendering.Rendering.camPosGoal[uBufferID].X = identity.M41;
					global::Rendering.Rendering.camPosGoal[uBufferID].Y = identity.M42;
					global::Rendering.Rendering.camPosGoal[uBufferID].Z = identity.M43;
					global::Rendering.Rendering.camObjectGoal[uBufferID].X = identity.M21 + identity.M41;
					global::Rendering.Rendering.camObjectGoal[uBufferID].Y = identity.M22 + identity.M42;
					global::Rendering.Rendering.camObjectGoal[uBufferID].Z = identity.M23 + identity.M43;
					global::Rendering.Rendering.camUp[uBufferID].X = 0f;
					global::Rendering.Rendering.camUp[uBufferID].Y = 0f;
					global::Rendering.Rendering.camUp[uBufferID].Z = 1f;
					MainGame.showCrossHairs[2] = 1;
					global::Rendering.Rendering.projectionNearPlane[uBufferID] = 0.1f;
					break;
				}
				case 3:
				{
					global::Rendering.Rendering.renderMainPlayer[uBufferID] = false;
					_ = global::Players.Players.players[0].primaryWeaponMountWeapon;
					if (global::Rendering.Rendering.viewPositionX == 0f)
					{
						global::Rendering.Rendering.viewPositionX = global::Weapons.Weapons.recoilSide + Explosions.cameraShakeX;
						global::Rendering.Rendering.viewVelocityX = Math.Abs(global::Rendering.Rendering.viewPositionX) / 0.083f * (float)(-Math.Sign(global::Rendering.Rendering.viewPositionX));
					}
					if (global::Rendering.Rendering.viewPositionY == 0f)
					{
						global::Rendering.Rendering.viewPositionY = global::Weapons.Weapons.recoilUp + Explosions.cameraShakeY;
						global::Rendering.Rendering.viewVelocityY = Math.Abs(global::Rendering.Rendering.viewPositionY) / 0.083f * (float)(-Math.Sign(global::Rendering.Rendering.viewPositionY));
					}
					global::Rendering.Rendering.curWeaponViewTime += MainGame.frametime / global::Physics.Physics.timeMod;
					float num3;
					if (global::Rendering.Rendering.curWeaponViewTime > global::Rendering.Rendering.weaponViewTime)
					{
						global::Rendering.Rendering.curWeaponViewTime = global::Rendering.Rendering.weaponViewTime;
						num3 = 1f;
						y = 0f;
						global::Rendering.Rendering.lastWeaponViewValue = (float)Math.PI / 4f / (float)Math.Pow(2.0, (int)global::Players.Players.scopeValue);
						identity = Matrix.CreateTranslation(ironSightsAdjX * y + global::Players.Players.scopeViewX * num3, ironSightsAdjY * y + global::Players.Players.scopeViewY * num3, ironSightsAdjZ * y + global::Players.Players.scopeViewZ * num3) * MainGame.playerVehicles[0].mounts[MainGame.primaryWeaponMount].mvCurrent[uBufferID] * Matrix.CreateTranslation(global::Players.Players.players[global::Rendering.Rendering.watchingPlayer].charP.position.v[0], global::Players.Players.players[global::Rendering.Rendering.watchingPlayer].charP.position.v[1], global::Players.Players.players[global::Rendering.Rendering.watchingPlayer].charP.position.v[2]);
					}
					else
					{
						num3 = global::Rendering.Rendering.curWeaponViewTime / global::Rendering.Rendering.weaponViewTime;
						y = 1f - num3;
						identity = Matrix.CreateTranslation(ironSightsAdjX * y + global::Players.Players.scopeViewX * num3, ironSightsAdjY * y + global::Players.Players.scopeViewY * num3, ironSightsAdjZ * y + global::Players.Players.scopeViewZ * num3) * MainGame.playerVehicles[0].mounts[MainGame.primaryWeaponMount].mvCurrent[uBufferID] * Matrix.CreateTranslation(global::Players.Players.players[global::Rendering.Rendering.watchingPlayer].charP.position.v[0], global::Players.Players.players[global::Rendering.Rendering.watchingPlayer].charP.position.v[1], global::Players.Players.players[global::Rendering.Rendering.watchingPlayer].charP.position.v[2]);
					}
					global::Rendering.Rendering.camPosGoal[uBufferID].X = identity.M41;
					global::Rendering.Rendering.camPosGoal[uBufferID].Y = identity.M42;
					global::Rendering.Rendering.camPosGoal[uBufferID].Z = identity.M43;
					global::Rendering.Rendering.camObjectGoal[uBufferID].X = identity.M21 + identity.M41;
					global::Rendering.Rendering.camObjectGoal[uBufferID].Y = identity.M22 + identity.M42;
					global::Rendering.Rendering.camObjectGoal[uBufferID].Z = identity.M23 + identity.M43;
					global::Rendering.Rendering.camUp[uBufferID].X = identity.M31;
					global::Rendering.Rendering.camUp[uBufferID].Y = identity.M32;
					global::Rendering.Rendering.camUp[uBufferID].Z = identity.M33;
					global::Rendering.Rendering.lastWeaponViewValue = (y * ((float)Math.PI / 4f) + num3 * ((float)Math.PI / 4f / (float)Math.Pow(2.0, (int)global::Players.Players.scopeValue))) / 2f;
					global::Rendering.Rendering.scopeValue = global::Rendering.Rendering.lastWeaponViewValue;
					MainGame.showCrossHairs[2] = 1;
					global::Rendering.Rendering.projectionNearPlane[uBufferID] = 0.1f;
					break;
				}
				case 4:
					global::Rendering.Rendering.renderMainPlayer[uBufferID] = true;
					if (MainGame.viewFollowingObject && MainGame.bombViewEnabled)
					{
						break;
					}
					if (!MainGame.pilotView)
					{
						global::Rendering.Rendering.cameraSpeed = MainGame.planeVelocity;
						float num = (float)Math.Sin(MainGame.viewAngle * ((float)Math.PI / 180f)) * 100f;
						float num2 = (float)Math.Cos(MainGame.viewAngle * ((float)Math.PI / 180f)) * -100f;
						global::Rendering.Rendering.camPosGoal[uBufferID].X = global::Players.Players.players[global::Rendering.Rendering.watchingPlayer].charP.position.v[0];
						global::Rendering.Rendering.camPosGoal[uBufferID].Y = global::Players.Players.players[global::Rendering.Rendering.watchingPlayer].charP.position.v[1];
						if ((float)(int)global::Rendering.Rendering.watchingPlayer != 0f)
						{
							global::Rendering.Rendering.cameraAdjustmentHeight = 0f;
						}
						global::Rendering.Rendering.camPosGoal[uBufferID].Z = global::Players.Players.players[global::Rendering.Rendering.watchingPlayer].charP.position.v[2] + global::Rendering.Rendering.cameraAdjustmentHeight;
						identity = global::Players.Players.players[global::Rendering.Rendering.watchingPlayer].mv[uBufferID];
						y = (float)Math.Acos((double)identity.M22 / Math.Sqrt(identity.M21 * identity.M21 + identity.M22 * identity.M22)) * 57.29578f;
						if (identity.M21 > 0f)
						{
							y = 360f - y;
						}
						float num3 = y - global::Players.Players.cameraRotationZ;
						if (num3 > 180f)
						{
							num3 = -360f + num3;
						}
						else if (num3 < -180f)
						{
							num3 = 360f + num3;
						}
						if (Math.Abs(num3) > 360f * MainGame.frametime)
						{
							num3 = (float)Math.Sign(num3) * 360f * MainGame.frametime;
						}
						global::Players.Players.cameraRotationZ += num3 * 0.9f;
						if (global::Rendering.Rendering.moveViewToNewLocation)
						{
							global::Players.Players.cameraRotationZ = y;
						}
						if (global::Players.Players.cameraRotationZ < 0f)
						{
							global::Players.Players.cameraRotationZ += 360f;
						}
						else if (global::Players.Players.cameraRotationZ > 360f)
						{
							global::Players.Players.cameraRotationZ -= 360f;
						}
						y = (float)Math.PI / 180f * global::Players.Players.cameraRotationZ;
						num3 = (float)Math.Cos(y);
						y = (float)Math.Sin(y);
						global::Rendering.Rendering.camPosGoal[uBufferID].X += num * num3 - num2 * y;
						global::Rendering.Rendering.camPosGoal[uBufferID].Y += num2 * num3 + num * y;
						global::Rendering.Rendering.camPosGoal[uBufferID].Z += 10f;
						global::Rendering.Rendering.camObjectGoal[uBufferID].X = global::Players.Players.players[global::Rendering.Rendering.watchingPlayer].charP.position.v[0];
						global::Rendering.Rendering.camObjectGoal[uBufferID].Y = global::Players.Players.players[global::Rendering.Rendering.watchingPlayer].charP.position.v[1];
						global::Rendering.Rendering.camObjectGoal[uBufferID].Z = global::Players.Players.players[global::Rendering.Rendering.watchingPlayer].charP.position.v[2];
						global::Rendering.Rendering.camUp[uBufferID].X = 0f;
						global::Rendering.Rendering.camUp[uBufferID].Y = 0f;
						global::Rendering.Rendering.camUp[uBufferID].Z = 1f;
						global::Rendering.Rendering.projectionNearPlane[uBufferID] = 20f;
					}
					else
					{
						global::Rendering.Rendering.cameraSpeed = MainGame.planeVelocity;
						identity = global::Players.Players.players[global::Rendering.Rendering.watchingPlayer].jt1[global::Players.Players.playerRaces[global::Players.Players.players[global::Rendering.Rendering.watchingPlayer].race].firstPersonViewJoint1[global::Players.Players.players[global::Rendering.Rendering.watchingPlayer].type]].mv[uBufferID];
						global::Rendering.Rendering.camPosGoal[uBufferID].X = global::Players.Players.players[global::Rendering.Rendering.watchingPlayer].charP.position.v[0] + identity.M41;
						global::Rendering.Rendering.camPosGoal[uBufferID].Y = global::Players.Players.players[global::Rendering.Rendering.watchingPlayer].charP.position.v[1] + identity.M42;
						global::Rendering.Rendering.camPosGoal[uBufferID].Z = global::Players.Players.players[global::Rendering.Rendering.watchingPlayer].charP.position.v[2] + identity.M43;
						global::Rendering.Rendering.camObjectGoal[uBufferID].X = global::Rendering.Rendering.camPosGoal[uBufferID].X + identity.M21;
						global::Rendering.Rendering.camObjectGoal[uBufferID].Y = global::Rendering.Rendering.camPosGoal[uBufferID].Y + identity.M22;
						global::Rendering.Rendering.camObjectGoal[uBufferID].Z = global::Rendering.Rendering.camPosGoal[uBufferID].Z + identity.M23;
						global::Rendering.Rendering.camUp[uBufferID].X = identity.M31;
						global::Rendering.Rendering.camUp[uBufferID].Y = identity.M32;
						global::Rendering.Rendering.camUp[uBufferID].Z = identity.M33;
						global::Rendering.Rendering.projectionNearPlane[uBufferID] = 7f;
					}
					break;
				case 7:
					MainGame.showCrossHairs[2] = 0;
					global::Weapons.Weapons.showTargetCrosshairTimer = 1f;
					global::Rendering.Rendering.camPosGoal[uBufferID].X = global::Rendering.Rendering.satelliteViewX;
					global::Rendering.Rendering.camPosGoal[uBufferID].Y = global::Rendering.Rendering.satelliteViewY;
					global::Rendering.Rendering.camPosGoal[uBufferID].Z = global::Rendering.Rendering.satelliteViewZ;
					global::Rendering.Rendering.camObjectGoal[uBufferID].X = global::Rendering.Rendering.camPosGoal[uBufferID].X;
					global::Rendering.Rendering.camObjectGoal[uBufferID].Y = global::Rendering.Rendering.camPosGoal[uBufferID].Y;
					global::Rendering.Rendering.camObjectGoal[uBufferID].Z = global::Rendering.Rendering.camPosGoal[uBufferID].Z - 1f;
					global::Rendering.Rendering.camUp[uBufferID].X = 0f;
					global::Rendering.Rendering.camUp[uBufferID].Y = 1f;
					global::Rendering.Rendering.camUp[uBufferID].Z = 0f;
					global::Rendering.Rendering.renderMainPlayer[uBufferID] = true;
					global::Rendering.Rendering.projectionNearPlane[uBufferID] = 0.1f;
					global::Rendering.Rendering.scopeValue = (float)Math.PI / 4f;
					break;
				}
			}
			else if (global::Players.Players.players[global::Rendering.Rendering.watchingPlayer].taunting)
			{
				float num3 = global::Players.Players.playerRaces[global::Players.Players.players[global::Rendering.Rendering.watchingPlayer].race].centerPoint[global::Players.Players.players[global::Rendering.Rendering.watchingPlayer].type];
				identity = Matrix.CreateTranslation(num3 * -3f, num3 * 4f, num3 * 2f) * Matrix.CreateRotationZ(global::Players.Players.players[global::Rendering.Rendering.watchingPlayer].zRotation * ((float)Math.PI / 180f)) * Matrix.CreateTranslation(global::Players.Players.players[global::Rendering.Rendering.watchingPlayer].charP.position.v[0], global::Players.Players.players[global::Rendering.Rendering.watchingPlayer].charP.position.v[1], global::Players.Players.players[global::Rendering.Rendering.watchingPlayer].charP.position.v[2]);
				global::Rendering.Rendering.camPosGoal[uBufferID].X = identity.M41;
				global::Rendering.Rendering.camPosGoal[uBufferID].Y = identity.M42;
				global::Rendering.Rendering.camPosGoal[uBufferID].Z = identity.M43;
				global::Rendering.Rendering.camObjectGoal[uBufferID].X = global::Players.Players.players[global::Rendering.Rendering.watchingPlayer].charP.position.v[0];
				global::Rendering.Rendering.camObjectGoal[uBufferID].Y = global::Players.Players.players[global::Rendering.Rendering.watchingPlayer].charP.position.v[1];
				global::Rendering.Rendering.camObjectGoal[uBufferID].Z = global::Players.Players.players[global::Rendering.Rendering.watchingPlayer].charP.position.v[2] + num3;
				global::Rendering.Rendering.camUp[uBufferID].X = identity.M31;
				global::Rendering.Rendering.camUp[uBufferID].Y = identity.M32;
				global::Rendering.Rendering.camUp[uBufferID].Z = identity.M33;
				MainGame.showCrossHairs[2] = 0;
				global::Rendering.Rendering.projectionNearPlane[uBufferID] = 0.008f;
				global::Rendering.Rendering.scopeValue = (float)Math.PI / 4f;
			}
			else
			{
				identity = global::Players.Players.players[global::Rendering.Rendering.watchingPlayer].mv[uBufferID];
				global::Rendering.Rendering.camPosGoal[uBufferID].X = global::Players.Players.players[global::Rendering.Rendering.watchingPlayer].charP.position.v[0];
				global::Rendering.Rendering.camPosGoal[uBufferID].Y = global::Players.Players.players[global::Rendering.Rendering.watchingPlayer].charP.position.v[1];
				global::Rendering.Rendering.camPosGoal[uBufferID].Z = global::Players.Players.players[global::Rendering.Rendering.watchingPlayer].charP.position.v[2] + 20f;
				global::Rendering.Rendering.camObjectGoal[uBufferID].X = global::Rendering.Rendering.camPosGoal[uBufferID].X;
				global::Rendering.Rendering.camObjectGoal[uBufferID].Y = global::Rendering.Rendering.camPosGoal[uBufferID].Y;
				global::Rendering.Rendering.camObjectGoal[uBufferID].Z = global::Rendering.Rendering.camPosGoal[uBufferID].Z - 1f;
				float num3 = (float)Math.Sqrt(identity.M21 * identity.M21 + identity.M22 * identity.M22);
				if (num3 != 0f)
				{
					y = identity.M22 / num3;
					num3 = identity.M21 / num3;
				}
				global::Rendering.Rendering.camUp[uBufferID].X = num3;
				global::Rendering.Rendering.camUp[uBufferID].Y = y;
				global::Rendering.Rendering.camUp[uBufferID].Z = 0f;
				global::Rendering.Rendering.renderMainPlayer[uBufferID] = true;
				global::Rendering.Rendering.projectionNearPlane[uBufferID] = 0.1f;
			}
		}
		else
		{
			byte rBufferID = global::Rendering.Rendering.rBufferID;
			global::Rendering.Rendering.camPosGoal[uBufferID].X = global::Rendering.Rendering.camPos[rBufferID].X;
			global::Rendering.Rendering.camPosGoal[uBufferID].Y = global::Rendering.Rendering.camPos[rBufferID].Y;
			global::Rendering.Rendering.camPosGoal[uBufferID].Z = global::Rendering.Rendering.camPos[rBufferID].Z;
			global::Rendering.Rendering.camObjectGoal[uBufferID].X = global::Rendering.Rendering.camObject[rBufferID].X;
			global::Rendering.Rendering.camObjectGoal[uBufferID].Y = global::Rendering.Rendering.camObject[rBufferID].Y;
			global::Rendering.Rendering.camObjectGoal[uBufferID].Z = global::Rendering.Rendering.camObject[rBufferID].Z;
			global::Rendering.Rendering.camUp[uBufferID].X = global::Rendering.Rendering.camUp[rBufferID].X;
			global::Rendering.Rendering.camUp[uBufferID].Y = global::Rendering.Rendering.camUp[rBufferID].Y;
			global::Rendering.Rendering.camUp[uBufferID].Z = global::Rendering.Rendering.camUp[rBufferID].Z;
			global::Rendering.Rendering.renderMainPlayer[uBufferID] = global::Rendering.Rendering.renderMainPlayer[rBufferID];
		}
		if (global::Players.Players.currentView != 7 && !global::Players.Players.freezeCamera)
		{
			mainC.playersMain.Move_Camera_Past_Obstructions(threadID);
		}
		mainC.renderingMain.Update_Camera_Position(MainGame.cameraMovementSpeed, MainGame.cameraObjectMovementSpeed, threadID);
		if (global::Players.Players.players[0].onmap == 16)
		{
			global::Rendering.Rendering.renderMainPlayer[uBufferID] = false;
		}
	}

	public void Game_SP_Round_Over()
	{
		switch (MainGame.gameLevel)
		{
		case 3:
			MainGame.numLives++;
			break;
		case 7:
			MainGame.numLives++;
			break;
		case 12:
			MainGame.numLives++;
			break;
		case 17:
			MainGame.numLives++;
			break;
		case 18:
			MainGame.numLives++;
			break;
		}
		global::Rendering.Rendering.watchingPlayer = 0;
		for (ushort num = 0; num < MainGame.maxGamePlayers; num++)
		{
			global::Joints.Joints.Sync_Player_Matrices(num, global::Rendering.Rendering.rBufferID, global::Rendering.Rendering.uBufferID);
		}
	}

	public void Game_MP_Round_Over()
	{
		mainC.weaponsMain.Move_Weapon_Rounds_To_Ammo_Clip_Surplus(0);
		if (MainGame.gameLevel == 18 && MainGame.gameType == 2)
		{
			MainGame.gameState = 147;
			mainC.soundsMain.Stop_Music();
			mainC.gameLogic.Game_Show_Results_Window();
			return;
		}
		MainGame.numLives = 5;
		byte gameType = MainGame.gameType;
		if (gameType == 2)
		{
			if (++MainGame.gameLevel >= 19)
			{
				MainGame.gameLevel = 18;
			}
		}
		else if (++MainGame.gameLevel >= 4)
		{
			MainGame.gameLevel = 0;
		}
		if (global::Networking.Networking.isHost)
		{
			global::Networking.Networking.networkSession.SessionProperties[3] = MainGame.gameLevel;
		}
		global::Rendering.Rendering.watchingPlayer = 0;
		for (ushort num = 1; num < MainGame.maxGamePlayers; num++)
		{
			global::Joints.Joints.Sync_Player_Matrices(num, global::Rendering.Rendering.rBufferID, global::Rendering.Rendering.uBufferID);
		}
		Game_Show_Results_Window();
	}

	public void Game_SP_Over()
	{
		global::Rendering.Rendering.watchingPlayer = 0;
		for (ushort num = 0; num < MainGame.maxGamePlayers; num++)
		{
			global::Joints.Joints.Sync_Player_Matrices(num, global::Rendering.Rendering.rBufferID, global::Rendering.Rendering.uBufferID);
		}
	}

	public void Game_SP_Game_Ended()
	{
		MainGame.gameState = 22;
	}

	public void Game_SP_Game_Finished()
	{
		global::Rendering.Rendering.watchingPlayer = 0;
	}

	public void Game_SP_Trial_Over()
	{
		global::Rendering.Rendering.watchingPlayer = 0;
	}

	public void Game_SP_Handle_Input(byte threadID)
	{
		bool flag = false;
		bool flag2 = false;
		bool flag3 = false;
		bool flag4 = false;
		byte uBufferID = global::Rendering.Rendering.uBufferID;
		if (global::Players.Players.players[0].onmap == 2)
		{
			global::Players.Players.players[0].timeBeforeRespawn[uBufferID] -= MainGame.frametime;
			if (global::Players.Players.players[0].timeBeforeRespawn[uBufferID] < 0f)
			{
				mainC.playersMain.Player_Spawn_Time_Over(0);
			}
		}
		if (!global::Players.Players.players[0].dead && global::Players.Players.players[0].onmap == 4)
		{
			flag = true;
		}
		else if (global::Players.Players.players[0].onmap == 8)
		{
			MainGame.curTimeBeforeExitingMapOnDeath -= MainGame.frametime;
			if (MainGame.curTimeBeforeExitingMapOnDeath < 0f)
			{
				global::Players.Players.players[0].onmap = 2;
				global::Players.Players.players[0].transporter = 2f;
				global::Players.Players.players[0].transporterDirection = -1;
				global::Players.Players.players[0].respawnParticle = mainC.renderingMain.New_Particle_Spawn(9, global::Players.Players.players[0].respawnParticle, 0, global::Players.Players.players[0].charP.position.v[0], global::Players.Players.players[0].charP.position.v[1], global::Players.Players.players[0].charP.position.v[2]);
			}
		}
		else if (global::Players.Players.players[0].onmap == 1 && !global::Players.Players.players[0].dead)
		{
			global::Players.Players.players[0].dead = true;
		}
		else if (global::Players.Players.players[0].onmap == 2 && global::Players.Players.players[0].dead && global::Players.Players.players[0].transporterDirection == 1)
		{
			global::Players.Players.players[0].dead = false;
		}
		if (global::InputHandler.InputHandler.controllerButtonStartPressed && !global::InputHandler.InputHandler.confirmEndGameScreen)
		{
			mainC.maingameMain.Entering_Menu_State();
		}
		byte objectID = MainGame.playerVehicles[0].mounts[MainGame.primaryWeaponMount].objectID;
		byte objectID2 = MainGame.playerVehicles[0].mounts[MainGame.secondaryWeaponMount].objectID;
		if (global::InputHandler.InputHandler.controllerTriggerRight && MainGame.playerVehicles[0].weapons[objectID].weaponID != 1)
		{
			if (flag && (MainGame.playerVehicles[0].weapons[objectID].fullyAutomatic || !MainGame.playerVehicles[0].weapons[objectID].triggerPulled))
			{
				if (MainGame.playerVehicles[0].weapons[objectID].currentRounds < 1 && MainGame.playerVehicles[0].weapons[objectID].shooting)
				{
					mainC.programsMain.Stop_Animation(ref global::Players.Players.players[0].animations, MainGame.playerVehicles[0].weapons[objectID].AnimationFire);
				}
				flag3 = true;
				MainGame.sprinting = false;
				MainGame.playerVehicles[0].weapons[objectID].shooting = true;
				if (MainGame.playerVehicles[0].mounts[MainGame.secondaryWeaponMount].objectAttached == 1)
				{
					MainGame.playerVehicles[0].weapons[objectID2].shooting = true;
				}
			}
			else if (MainGame.playerVehicles[0].weapons[objectID].shooting)
			{
				mainC.weaponsMain.firingStopped(0, objectID);
				if (MainGame.playerVehicles[0].mounts[MainGame.secondaryWeaponMount].objectAttached == 1)
				{
					mainC.weaponsMain.firingStopped(0, objectID2);
				}
				MainGame.playerVehicles[0].weapons[objectID].shooting = false;
				MainGame.playerVehicles[0].weapons[objectID2].shooting = false;
			}
			MainGame.playerVehicles[0].weapons[objectID].triggerPulled = true;
		}
		else
		{
			if (MainGame.playerVehicles[0].weapons[objectID].shooting)
			{
				mainC.weaponsMain.firingStopped(0, objectID);
				if (MainGame.playerVehicles[0].mounts[MainGame.secondaryWeaponMount].objectAttached == 1)
				{
					mainC.weaponsMain.firingStopped(0, objectID2);
				}
				MainGame.playerVehicles[0].weapons[objectID].shooting = false;
				MainGame.playerVehicles[0].weapons[objectID2].shooting = false;
			}
			MainGame.playerVehicles[0].weapons[objectID].triggerPulled = false;
		}
		bool usingIronSights = MainGame.usingIronSights;
		MainGame.usingIronSights = false;
		if (global::Weapons.Weapons.weaponViewEnabled)
		{
			if (global::InputHandler.InputHandler.controllerTriggerLeft && global::Players.Players.players[0].onmap == 4 && !global::Players.Players.players[0].taunting)
			{
				MainGame.sprinting = false;
				MainGame.usingIronSights = true;
			}
		}
		else if (global::Weapons.Weapons.scopeViewEnabled)
		{
			if (global::InputHandler.InputHandler.controllerTriggerLeft && !global::Players.Players.players[0].taunting)
			{
				if (global::Weapons.Weapons.scopeViewEnabled && !MainGame.usingScope && global::Players.Players.players[0].onmap == 4)
				{
					global::Players.Players.scopeValue = global::Players.Players.players[0].weapon2[global::Players.Players.players[0].wpnIndex].scopeLow;
					flag2 = true;
					MainGame.quickScope = true;
					MainGame.sprinting = false;
				}
			}
			else if (MainGame.usingScope && MainGame.quickScope)
			{
				flag2 = true;
			}
		}
		if (usingIronSights || MainGame.usingIronSights || MainGame.usingScope || flag2)
		{
			byte animationID = (byte)MainGame.playerVehicles[0].weapons[objectID].AnimationIronSights;
			if (MainGame.usingIronSights || MainGame.usingScope != flag2)
			{
				mainC.programsMain.Start_Animation(0, ref global::Players.Players.players[0].jt1, ref global::Players.Players.players[0].animations, global::Players.Players.players[0].programCollection, animationID, 1f, 1f);
			}
			else
			{
				mainC.programsMain.Set_Animation_To_Reverse_Direction(ref global::Players.Players.players[0].animations, animationID);
			}
		}
		if (flag)
		{
			if (global::InputHandler.InputHandler.controllerButtonYPressed && MainGame.playerVehicles[0].weapons[1].currentRounds > 0 && !global::Players.Players.changingWeapons && global::Players.Players.players[0].animations[110].status < 2)
			{
				global::Players.Players.changingWeapons = true;
				mainC.weaponsMain.Check_Weapon_Views();
				global::Players.Players.players[0].animations[global::Players.Players.players[0].programSwitchWeapons].callBackType = 14;
				global::Players.Players.players[0].animations[global::Players.Players.players[0].programSwitchWeapons].callBack = 110;
				global::Players.Players.players[0].animations[110].callBack = MainGame.playerVehicles[0].weapons[MainGame.playerVehicles[0].mounts[MainGame.primaryWeaponMount].objectID].weaponID;
				mainC.programsMain.Start_Animation(0, ref global::Players.Players.players[0].jt1, ref global::Players.Players.players[0].animations, global::Players.Players.players[0].programCollection, global::Players.Players.players[0].programSwitchWeapons, 1f, 1f);
			}
			if (global::InputHandler.InputHandler.controllerButtonBPressed)
			{
				if (MainGame.usingScope)
				{
					mainC.weaponsMain.Stop_Using_Weapon_Scope();
				}
				else if (global::Players.Players.playerViewingDevice)
				{
					mainC.playersMain.Player_Stops_Viewing_Device();
					global::Weapons.Weapons.showTargetCrosshairTimer = 0f;
				}
			}
			if (global::InputHandler.InputHandler.controllerButtonXPressed)
			{
				if (!global::Pickups.Pickups.playerPickupWeaponEnabled)
				{
					MainGame.playerVehicles[0].weapons[MainGame.playerVehicles[0].mounts[MainGame.primaryWeaponMount].objectID].needToReload = true;
					global::Players.Players.needToReload = true;
				}
				else
				{
					global::Pickups.Pickups.playerPickingUp = true;
					byte animationID = (byte)global::Weapons.Weapons.wp1[MainGame.playerVehicles[0].weapons[MainGame.playerVehicles[0].mounts[MainGame.primaryWeaponMount].objectID].weaponID].AnimationReload;
					if (global::Players.Players.players[0].animations[animationID].status == 2)
					{
						mainC.programsMain.Stop_Animation(ref global::Players.Players.players[0].animations, animationID);
						mainC.callbackMain.CallBack(0, global::Players.Players.players[0].animations[animationID].cancelledCallBackType, global::Players.Players.players[0].animations[animationID].cancelledCallBack, global::Players.Players.players[0].animations[animationID].var1, 1);
					}
				}
			}
			global::Players.Players.jumping = false;
		}
		else if (global::Players.Players.players[0].dead && global::Players.Players.players[0].onmap == 1)
		{
			if (global::Players.Players.respawnEnabled && (global::InputHandler.InputHandler.controllerButtonAPressed || MainGame.autoRespawn))
			{
				if (MainGame.numLives < 1 && MainGame.linearProgression)
				{
					mainC.gameLogic.Game_SP_Game_Ended();
				}
				else
				{
					mainC.playersMain.Player_Respawn(threadID);
				}
			}
		}
		else
		{
			Game_Check_Player_Off_Map_Status();
		}
		if ((global::InputHandler.InputHandler.controllerButtonRightShoulderPressed || global::InputHandler.InputHandler.controllerButtonLeftShoulderPressed) && mainC.vehicles.Player_Vehicle_Stub_Has_Weapon(0, 0) && mainC.vehicles.Player_Vehicle_Stub_Has_Weapon(0, 2) && !MainGame.sprinting && global::Players.Players.players[0].animations[global::Players.Players.players[0].programSwitchWeapons].status < 2)
		{
			global::Players.Players.changingWeapons = true;
			mainC.weaponsMain.Check_Weapon_Views();
			if (global::Players.Players.players[0].primaryWeaponMountWeapon == MainGame.playerVehicles[0].weapons[0].weaponID)
			{
				global::Players.Players.players[0].animations[global::Players.Players.players[0].programSwitchWeapons].var1 = MainGame.playerVehicles[0].weapons[2].weaponID;
			}
			else
			{
				global::Players.Players.players[0].animations[global::Players.Players.players[0].programSwitchWeapons].var1 = MainGame.playerVehicles[0].weapons[0].weaponID;
			}
			global::Players.Players.players[0].animations[global::Players.Players.players[0].programSwitchWeapons].callBackType = 12;
			global::Players.Players.players[0].animations[global::Players.Players.players[0].programSwitchWeapons].callBack = MainGame.primaryWeaponMount;
			mainC.programsMain.Start_Animation(0, ref global::Players.Players.players[0].jt1, ref global::Players.Players.players[0].animations, global::Players.Players.players[0].programCollection, global::Players.Players.players[0].programSwitchWeapons, 1f, 1f);
		}
		global::Players.Players.players[0].shooting = flag3 || flag4;
		if (global::InputHandler.InputHandler.controllerStickButtonLeftPressed && global::Players.Players.runTime == 4f && !MainGame.usingIronSights && !MainGame.usingScope)
		{
			global::Players.Players.runTime = 4f;
			MainGame.sprinting = true;
		}
		if (flag)
		{
			float controllerStickLeftValueY = global::InputHandler.InputHandler.controllerStickLeftValueY;
			if (controllerStickLeftValueY < 0.2f || global::Players.Players.runTime < 0f)
			{
				MainGame.sprinting = false;
			}
			float controllerStickLeftValueX;
			if (MainGame.sprinting)
			{
				global::Players.Players.runTime -= MainGame.frametime;
				global::Players.Players.moving |= 1;
				global::Players.Players.playerSpeed = 30f;
				global::Rendering.Rendering.viewMovement = 1.25f;
				global::Players.Players.footStepTimer += 1.55f * (MainGame.frametime / global::Physics.Physics.timeMod);
				global::Players.Players.playerSpeedRotateLeftStick = 0f;
				MainGame.walking = false;
				MainGame.walkingBackwards = false;
				MainGame.sideStepping = false;
			}
			else
			{
				global::Players.Players.runTime += MainGame.frametime;
				if (global::Players.Players.runTime > 4f)
				{
					global::Players.Players.runTime = 4f;
				}
				controllerStickLeftValueX = global::InputHandler.InputHandler.controllerStickLeftValueX;
				if (controllerStickLeftValueX != 0f)
				{
					global::Players.Players.moving |= 2;
				}
				global::Players.Players.playerSpeedSideways = 20f * controllerStickLeftValueX * global::InputHandler.InputHandler.lookSensitivity[global::InputHandler.InputHandler.lookMode] * global::Players.Players.scopeViewAdj;
				if (controllerStickLeftValueY != 0f)
				{
					global::Players.Players.moving |= 1;
					global::Players.Players.playerSpeed = 20f * controllerStickLeftValueY;
				}
				global::Players.Players.playerSpeedRotateLeftStick = -140f * controllerStickLeftValueX * global::InputHandler.InputHandler.lookSensitivity[global::InputHandler.InputHandler.lookMode] * global::Players.Players.scopeViewAdj;
				controllerStickLeftValueX = Math.Abs(controllerStickLeftValueX);
				controllerStickLeftValueY = Math.Abs(controllerStickLeftValueY);
				if (controllerStickLeftValueX != 0f && controllerStickLeftValueX > 0.9f)
				{
					MainGame.sideStepping = true;
					global::Rendering.Rendering.viewMovement = global::Players.Players.playerSpeedSideways / 20f;
					global::Players.Players.footStepTimer += (0.5f + 0.5f * Math.Abs(global::Rendering.Rendering.viewMovement)) * (MainGame.frametime / global::Physics.Physics.timeMod);
				}
				else if (controllerStickLeftValueY != 0f)
				{
					MainGame.walking = true;
					MainGame.walkingBackwards = false;
					if (global::Players.Players.playerSpeed < 0f)
					{
						MainGame.walking = false;
						MainGame.walkingBackwards = true;
					}
					global::Rendering.Rendering.viewMovement = global::Players.Players.playerSpeed / 20f;
					global::Players.Players.footStepTimer += (0.5f + 0.5f * Math.Abs(global::Rendering.Rendering.viewMovement)) * (MainGame.frametime / global::Physics.Physics.timeMod);
				}
			}
			if (global::Players.Players.footStepTimer > 0.6f && (global::Players.Players.players[0].playerIsMoving & 0x20) == 0)
			{
				global::Players.Players.footStepTimer = 0f;
				mainC.soundsMain.Play_Sound("Footstep0", global::Players.Players.players[0].charP.position.v[0], global::Players.Players.players[0].charP.position.v[1], global::Players.Players.players[0].charP.position.v[2], 0f, 0f, 0f);
			}
			controllerStickLeftValueX = global::InputHandler.InputHandler.controllerStickLeftValueY;
			controllerStickLeftValueX = (global::InputHandler.InputHandler.controllerStickRightValX = global::InputHandler.InputHandler.controllerStickRightValueX);
			if (controllerStickLeftValueX != 0f)
			{
				global::Players.Players.moving |= 4;
			}
			global::Players.Players.playerSpeedRotateRightStick = -140f * controllerStickLeftValueX * global::InputHandler.InputHandler.lookSensitivity[global::InputHandler.InputHandler.lookMode] * global::Players.Players.scopeViewAdj;
			controllerStickLeftValueX = (global::InputHandler.InputHandler.controllerStickRightValY = global::InputHandler.InputHandler.controllerStickRightValueY * global::Players.Players.invertY);
			if (controllerStickLeftValueX != 0f)
			{
				global::Players.Players.moving |= 8;
			}
			global::Players.Players.playerSpeedElevateRightStick = 225f * controllerStickLeftValueX * global::InputHandler.InputHandler.lookSensitivity[global::InputHandler.InputHandler.lookMode] * global::Players.Players.scopeViewAdj;
		}
		else
		{
			MainGame.sprinting = false;
			global::Players.Players.moving = 0;
			global::Players.Players.playerSpeedSideways = 0f;
			global::Players.Players.playerSpeed = 0f;
			MainGame.sideStepping = false;
			MainGame.walking = false;
			MainGame.walkingBackwards = false;
			global::Rendering.Rendering.viewMovement = 0f;
			global::Players.Players.footStepTimer = 0f;
			global::InputHandler.InputHandler.controllerStickRightValX = 0f;
			global::InputHandler.InputHandler.controllerStickRightValY = 0f;
			global::Players.Players.playerSpeedRotateRightStick = 0f;
			global::Players.Players.playerSpeedElevateRightStick = 0f;
			global::Players.Players.xRotation = 0f;
		}
		if (global::Players.Players.currentView == 0)
		{
			global::Players.Players.players[0].playerModel[0] = playerModelTPV;
			if (MainGame.usingIronSights)
			{
				global::Players.Players.players[0].playerModel[0] = playerModelFPV;
			}
		}
		mainC.weaponsMain.Change_Weapon_View(flag2);
	}

	public void Game_MP_Handle_Input(byte threadID)
	{
		bool flag = false;
		bool flag2 = false;
		bool flag3 = false;
		bool flag4 = false;
		byte uBufferID = global::Rendering.Rendering.uBufferID;
		if (global::Players.Players.players[0].onmap == 2)
		{
			global::Players.Players.players[0].timeBeforeRespawn[uBufferID] -= MainGame.frametime;
			if (global::Players.Players.players[0].timeBeforeRespawn[uBufferID] < 0f)
			{
				mainC.playersMain.Player_Spawn_Time_Over(0);
			}
		}
		if (!global::Players.Players.players[0].dead && global::Players.Players.players[0].onmap == 4)
		{
			flag = true;
		}
		else if (global::Players.Players.players[0].dead && (global::Players.Players.players[0].onmap & 9) > 0)
		{
			if (global::Players.Players.players[0].onmap == 1)
			{
				global::Players.Players.respawnTimer -= MainGame.frametime;
				if (global::Players.Players.respawnTimer < 0f)
				{
					global::InputHandler.InputHandler.controllerButtonAPressed = true;
				}
			}
			else
			{
				MainGame.curTimeBeforeExitingMapOnDeath -= MainGame.frametime;
				if (MainGame.curTimeBeforeExitingMapOnDeath < 0f)
				{
					global::Players.Players.players[0].onmap = 2;
					global::Players.Players.players[0].transporter = 2f;
					global::Players.Players.players[0].transporterDirection = -1;
					global::Players.Players.players[0].respawnParticle = mainC.renderingMain.New_Particle_Spawn(9, global::Players.Players.players[0].respawnParticle, 0, global::Players.Players.players[0].charP.position.v[0], global::Players.Players.players[0].charP.position.v[1], global::Players.Players.players[0].charP.position.v[2]);
				}
			}
		}
		if (global::InputHandler.InputHandler.controllerButtonStartPressed && !global::InputHandler.InputHandler.confirmEndGameScreen)
		{
			mainC.maingameMain.Entering_Menu_State();
		}
		if (MainGame.usingScope)
		{
			if (global::InputHandler.InputHandler.controllerDPadUpPressed)
			{
				if (global::Players.Players.scopeValue < MainGame.playerVehicles[0].weapons[MainGame.playerVehicles[0].mounts[MainGame.primaryWeaponMount].objectID].scopeHigh)
				{
					global::Players.Players.scopeValue++;
				}
				global::Players.Players.scopeViewAdj = 1f / (float)Math.Pow(2.0, (int)global::Players.Players.scopeValue);
			}
			else if (global::InputHandler.InputHandler.controllerDPadDownPressed)
			{
				if (global::Players.Players.scopeValue > MainGame.playerVehicles[0].weapons[MainGame.playerVehicles[0].mounts[MainGame.primaryWeaponMount].objectID].scopeLow)
				{
					global::Players.Players.scopeValue--;
				}
				global::Players.Players.scopeViewAdj = 1f / (float)Math.Pow(2.0, (int)global::Players.Players.scopeValue);
			}
		}
		else if (global::Weapons.Weapons.scopeViewEnabled)
		{
			if (global::InputHandler.InputHandler.controllerDPadUpPressed && global::Players.Players.players[0].onmap == 4 && !global::Players.Players.players[0].taunting)
			{
				global::Players.Players.scopeValue = MainGame.playerVehicles[0].weapons[MainGame.playerVehicles[0].mounts[MainGame.primaryWeaponMount].objectID].scopeHigh;
				flag2 = true;
				MainGame.quickScope = false;
			}
			else if (global::InputHandler.InputHandler.controllerDPadDownPressed && global::Players.Players.players[0].onmap == 4 && !global::Players.Players.players[0].taunting)
			{
				global::Players.Players.scopeValue = MainGame.playerVehicles[0].weapons[MainGame.playerVehicles[0].mounts[MainGame.primaryWeaponMount].objectID].scopeLow;
				flag2 = true;
				MainGame.quickScope = false;
			}
		}
		byte objectID = MainGame.playerVehicles[0].mounts[MainGame.primaryWeaponMount].objectID;
		byte objectID2 = MainGame.playerVehicles[0].mounts[MainGame.secondaryWeaponMount].objectID;
		if (global::InputHandler.InputHandler.controllerTriggerRight && MainGame.playerVehicles[0].weapons[objectID].weaponID != 1)
		{
			if (flag && (MainGame.playerVehicles[0].weapons[objectID].fullyAutomatic || !MainGame.playerVehicles[0].weapons[objectID].triggerPulled))
			{
				if (MainGame.playerVehicles[0].weapons[objectID].currentRounds < 1 && MainGame.playerVehicles[0].weapons[objectID].shooting)
				{
					mainC.programsMain.Stop_Animation(ref global::Players.Players.players[0].animations, MainGame.playerVehicles[0].weapons[objectID].AnimationFire);
				}
				flag3 = true;
				MainGame.sprinting = false;
				if (!MainGame.playerVehicles[0].weapons[objectID].shooting)
				{
					if (MainGame.playerVehicles[0].weapons[objectID].currentRounds > 0)
					{
						mainC.playersMain.Send_Player_Shooting_Message(objectID, shooting: true);
					}
				}
				else if (MainGame.playerVehicles[0].weapons[objectID].currentRounds < 1)
				{
					mainC.playersMain.Send_Player_Shooting_Message(objectID, shooting: false);
				}
				MainGame.playerVehicles[0].weapons[objectID].shooting = true;
				if (MainGame.playerVehicles[0].mounts[MainGame.secondaryWeaponMount].objectAttached == 1)
				{
					MainGame.playerVehicles[0].weapons[objectID2].shooting = true;
				}
			}
			else if (MainGame.playerVehicles[0].weapons[objectID].shooting)
			{
				mainC.playersMain.Send_Player_Shooting_Message(objectID, shooting: false);
				mainC.weaponsMain.firingStopped(0, objectID);
				if (MainGame.playerVehicles[0].mounts[MainGame.secondaryWeaponMount].objectAttached == 1)
				{
					mainC.weaponsMain.firingStopped(0, objectID2);
				}
				MainGame.playerVehicles[0].weapons[objectID].shooting = false;
				MainGame.playerVehicles[0].weapons[objectID2].shooting = false;
			}
			MainGame.playerVehicles[0].weapons[objectID].triggerPulled = true;
		}
		else
		{
			if (MainGame.playerVehicles[0].weapons[objectID].shooting)
			{
				mainC.playersMain.Send_Player_Shooting_Message(objectID, shooting: false);
				mainC.weaponsMain.firingStopped(0, objectID);
				if (MainGame.playerVehicles[0].mounts[MainGame.secondaryWeaponMount].objectAttached == 1)
				{
					mainC.weaponsMain.firingStopped(0, objectID2);
				}
				MainGame.playerVehicles[0].weapons[objectID].shooting = false;
				MainGame.playerVehicles[0].weapons[objectID2].shooting = false;
			}
			MainGame.playerVehicles[0].weapons[objectID].triggerPulled = false;
		}
		bool usingIronSights = MainGame.usingIronSights;
		MainGame.usingIronSights = false;
		if (global::Weapons.Weapons.weaponViewEnabled)
		{
			if (global::InputHandler.InputHandler.controllerTriggerLeft && global::Players.Players.players[0].onmap == 4 && !global::Players.Players.players[0].taunting)
			{
				MainGame.sprinting = false;
				MainGame.usingIronSights = true;
				MainGame.sprinting = false;
			}
		}
		else if (global::Weapons.Weapons.scopeViewEnabled)
		{
			if (global::InputHandler.InputHandler.controllerTriggerLeft && !global::Players.Players.players[0].taunting && !MainGame.sprinting)
			{
				if (global::Weapons.Weapons.scopeViewEnabled && !MainGame.usingScope && global::Players.Players.players[0].onmap == 4)
				{
					global::Players.Players.scopeValue = global::Players.Players.players[0].weapon2[global::Players.Players.players[0].wpnIndex].scopeLow;
					flag2 = true;
					MainGame.quickScope = true;
				}
			}
			else if (MainGame.usingScope && MainGame.quickScope)
			{
				flag2 = true;
			}
		}
		if (usingIronSights || MainGame.usingIronSights || MainGame.usingScope || flag2)
		{
			byte animationID = (byte)MainGame.playerVehicles[0].weapons[objectID].AnimationIronSights;
			if (MainGame.usingIronSights || MainGame.usingScope != flag2)
			{
				mainC.programsMain.Start_Animation(0, ref global::Players.Players.players[0].jt1, ref global::Players.Players.players[0].animations, global::Players.Players.players[0].programCollection, animationID, 1f, 1f);
			}
			else
			{
				mainC.programsMain.Set_Animation_To_Reverse_Direction(ref global::Players.Players.players[0].animations, animationID);
			}
		}
		if (flag)
		{
			if (global::InputHandler.InputHandler.controllerButtonYPressed && MainGame.playerVehicles[0].weapons[1].currentRounds > 0 && !global::Players.Players.changingWeapons && global::Players.Players.players[0].animations[110].status < 2)
			{
				global::Players.Players.changingWeapons = true;
				mainC.weaponsMain.Check_Weapon_Views();
				global::Players.Players.players[0].animations[global::Players.Players.players[0].programSwitchWeapons].callBackType = 14;
				global::Players.Players.players[0].animations[global::Players.Players.players[0].programSwitchWeapons].callBack = 110;
				global::Players.Players.players[0].animations[110].callBack = MainGame.playerVehicles[0].weapons[MainGame.playerVehicles[0].mounts[MainGame.primaryWeaponMount].objectID].weaponID;
				mainC.programsMain.Start_Animation(0, ref global::Players.Players.players[0].jt1, ref global::Players.Players.players[0].animations, global::Players.Players.players[0].programCollection, global::Players.Players.players[0].programSwitchWeapons, 1f, 1f);
			}
			if (global::InputHandler.InputHandler.controllerButtonBPressed)
			{
				if (MainGame.usingScope)
				{
					mainC.weaponsMain.Stop_Using_Weapon_Scope();
				}
				else if (global::Players.Players.playerViewingDevice)
				{
					mainC.playersMain.Player_Stops_Viewing_Device();
					global::Weapons.Weapons.showTargetCrosshairTimer = 0f;
				}
			}
			if (global::InputHandler.InputHandler.controllerButtonXPressed)
			{
				if (!global::Pickups.Pickups.playerPickupWeaponEnabled)
				{
					MainGame.playerVehicles[0].weapons[MainGame.playerVehicles[0].mounts[MainGame.primaryWeaponMount].objectID].needToReload = true;
					global::Players.Players.needToReload = true;
				}
				else
				{
					global::Pickups.Pickups.playerPickingUp = true;
					byte animationID = (byte)global::Weapons.Weapons.wp1[MainGame.playerVehicles[0].weapons[MainGame.playerVehicles[0].mounts[MainGame.primaryWeaponMount].objectID].weaponID].AnimationReload;
					if (global::Players.Players.players[0].animations[animationID].status == 2)
					{
						mainC.programsMain.Stop_Animation(ref global::Players.Players.players[0].animations, animationID);
						mainC.callbackMain.CallBack(0, global::Players.Players.players[0].animations[animationID].cancelledCallBackType, global::Players.Players.players[0].animations[animationID].cancelledCallBack, global::Players.Players.players[0].animations[animationID].var1, 1);
					}
				}
			}
			global::Players.Players.jumping = false;
			if (global::InputHandler.InputHandler.controllerButtonAPressed && global::Players.Players.fallingTimer != 0f)
			{
			}
		}
		else if (global::Players.Players.players[0].dead && global::Players.Players.players[0].onmap == 1)
		{
			if (global::Players.Players.respawnEnabled && (global::InputHandler.InputHandler.controllerButtonAPressed || MainGame.autoRespawn))
			{
				mainC.playersMain.Player_Respawn(threadID);
				MainGame.autoRespawn = false;
				MainGame.gameState = 149;
			}
		}
		else
		{
			Game_Check_Player_Off_Map_Status();
		}
		global::Players.Players.players[0].shooting = flag3 || flag4;
		if ((global::InputHandler.InputHandler.controllerButtonRightShoulderPressed || global::InputHandler.InputHandler.controllerButtonLeftShoulderPressed) && mainC.vehicles.Player_Vehicle_Stub_Has_Weapon(0, 0) && mainC.vehicles.Player_Vehicle_Stub_Has_Weapon(0, 2) && !MainGame.sprinting && global::Players.Players.players[0].animations[global::Players.Players.players[0].programSwitchWeapons].status < 2)
		{
			global::Players.Players.changingWeapons = true;
			mainC.weaponsMain.Check_Weapon_Views();
			if (global::Players.Players.players[0].primaryWeaponMountWeapon == MainGame.playerVehicles[0].weapons[0].weaponID)
			{
				global::Players.Players.players[0].animations[global::Players.Players.players[0].programSwitchWeapons].var1 = MainGame.playerVehicles[0].weapons[2].weaponID;
			}
			else
			{
				global::Players.Players.players[0].animations[global::Players.Players.players[0].programSwitchWeapons].var1 = MainGame.playerVehicles[0].weapons[0].weaponID;
			}
			global::Players.Players.players[0].animations[global::Players.Players.players[0].programSwitchWeapons].callBackType = 12;
			global::Players.Players.players[0].animations[global::Players.Players.players[0].programSwitchWeapons].callBack = MainGame.primaryWeaponMount;
			mainC.programsMain.Start_Animation(0, ref global::Players.Players.players[0].jt1, ref global::Players.Players.players[0].animations, global::Players.Players.players[0].programCollection, global::Players.Players.players[0].programSwitchWeapons, 1f, 1f);
		}
		if (global::InputHandler.InputHandler.controllerStickButtonLeftPressed && global::Players.Players.runTime == 4f && !MainGame.usingIronSights && !MainGame.usingScope)
		{
			global::Players.Players.runTime = 4f;
			MainGame.sprinting = true;
		}
		if (flag)
		{
			float num;
			if (global::Players.Players.players[0].onmap == 4)
			{
				float controllerStickLeftValueY = global::InputHandler.InputHandler.controllerStickLeftValueY;
				if (controllerStickLeftValueY < 0.2f || global::Players.Players.runTime < 0f)
				{
					MainGame.sprinting = false;
				}
				if (MainGame.sprinting)
				{
					global::Players.Players.runTime -= MainGame.frametime;
					global::Players.Players.moving |= 1;
					global::Players.Players.playerSpeed = 30f;
					global::Rendering.Rendering.viewMovement = 1.25f;
					global::Players.Players.footStepTimer += 1.55f * (MainGame.frametime / global::Physics.Physics.timeMod);
					global::Players.Players.playerSpeedRotateLeftStick = 0f;
					MainGame.walking = false;
					MainGame.walkingBackwards = false;
					MainGame.sideStepping = false;
				}
				else
				{
					global::Players.Players.runTime += MainGame.frametime;
					if (global::Players.Players.runTime > 4f)
					{
						global::Players.Players.runTime = 4f;
					}
					num = global::InputHandler.InputHandler.controllerStickLeftValueX;
					if (num != 0f)
					{
						if (global::InputHandler.InputHandler.slowSideStep && global::InputHandler.InputHandler.lookMode == 1)
						{
							num *= global::InputHandler.InputHandler.lookSensitivity[1];
						}
						global::Players.Players.moving |= 2;
						global::Players.Players.playerSpeedSideways = 20f * num;
					}
					if (controllerStickLeftValueY != 0f)
					{
						global::Players.Players.moving |= 1;
						global::Players.Players.playerSpeed = 20f * controllerStickLeftValueY;
					}
					global::Players.Players.playerSpeedRotateLeftStick = 0f;
					if (Math.Abs(num) > 0.01f)
					{
						global::Players.Players.playerSpeedRotateLeftStick = -140f * num * global::InputHandler.InputHandler.lookSensitivity[global::InputHandler.InputHandler.lookMode] * global::Players.Players.scopeViewAdj;
					}
					num = Math.Abs(num);
					controllerStickLeftValueY = Math.Abs(controllerStickLeftValueY);
					if (num != 0f && num > controllerStickLeftValueY)
					{
						MainGame.sideStepping = true;
						global::Rendering.Rendering.viewMovement = global::Players.Players.playerSpeedSideways / 20f;
						global::Players.Players.footStepTimer += (0.5f + 0.5f * Math.Abs(global::Rendering.Rendering.viewMovement)) * (MainGame.frametime / global::Physics.Physics.timeMod);
					}
					else if (controllerStickLeftValueY != 0f)
					{
						MainGame.walking = true;
						MainGame.walkingBackwards = false;
						if (global::Players.Players.playerSpeed < 0f)
						{
							MainGame.walking = false;
							MainGame.walkingBackwards = true;
						}
						global::Rendering.Rendering.viewMovement = global::Players.Players.playerSpeed / 20f;
						global::Players.Players.footStepTimer += (0.5f + 0.5f * Math.Abs(global::Rendering.Rendering.viewMovement)) * (MainGame.frametime / global::Physics.Physics.timeMod);
					}
				}
				if (global::Players.Players.footStepTimer > 0.6f && (global::Players.Players.players[0].playerIsMoving & 0x20) == 0)
				{
					global::Players.Players.footStepTimer = 0f;
					mainC.soundsMain.Play_Sound("Footstep0", global::Players.Players.players[0].charP.position.v[0], global::Players.Players.players[0].charP.position.v[1], global::Players.Players.players[0].charP.position.v[2], 0f, 0f, 0f);
				}
			}
			num = (global::InputHandler.InputHandler.controllerStickRightValX = global::InputHandler.InputHandler.controllerStickRightValueX);
			if (num != 0f)
			{
				global::Players.Players.moving |= 4;
			}
			global::Players.Players.playerSpeedRotateRightStick = -140f * num * global::InputHandler.InputHandler.lookSensitivity[global::InputHandler.InputHandler.lookMode] * global::Players.Players.scopeViewAdj;
			num = (global::InputHandler.InputHandler.controllerStickRightValY = global::InputHandler.InputHandler.controllerStickRightValueY * global::Players.Players.invertY);
			if (num != 0f)
			{
				global::Players.Players.moving |= 8;
			}
			global::Players.Players.playerSpeedElevateRightStick = 225f * num * global::InputHandler.InputHandler.lookSensitivity[global::InputHandler.InputHandler.lookMode] * global::Players.Players.scopeViewAdj;
		}
		else
		{
			MainGame.sprinting = false;
			global::Players.Players.moving = 0;
			global::Players.Players.playerSpeedSideways = 0f;
			global::Players.Players.playerSpeed = 0f;
			MainGame.sideStepping = false;
			MainGame.walking = false;
			MainGame.walkingBackwards = false;
			global::Rendering.Rendering.viewMovement = 0f;
			global::Players.Players.footStepTimer = 0f;
			global::InputHandler.InputHandler.controllerStickRightValX = 0f;
			global::InputHandler.InputHandler.controllerStickRightValY = 0f;
			global::Players.Players.playerSpeedRotateRightStick = 0f;
			global::Players.Players.playerSpeedElevateRightStick = 0f;
			global::Players.Players.xRotation = 0f;
		}
		if (global::Players.Players.currentView == 0)
		{
			global::Players.Players.players[0].playerModel[0] = playerModelTPV;
			if (MainGame.usingIronSights)
			{
				global::Players.Players.players[0].playerModel[0] = playerModelFPV;
			}
		}
		mainC.weaponsMain.Change_Weapon_View(flag2);
	}

	public void Game_Render_Additional_Window_Objects(ushort windowID)
	{
		switch (windowID)
		{
		case 255:
			mainC.renderingMain.Render_Vehicle_Select(MainGame.frametime);
			break;
		case 11:
			mainC.renderingMain.Render_Weapon_Select(MainGame.frametime);
			break;
		}
	}

	public void Game_Close_Window(ushort windowID)
	{
		switch (windowID)
		{
		case 1:
			Game_Handle_Main_Menu(1);
			break;
		case 0:
			Game_Handle_Options(1, 0, 0f);
			break;
		case 5:
			Game_Handle_In_Game_Menu(1);
			break;
		case 7:
			Game_Handle_Play_Menu(1);
			break;
		case 23:
			Game_Handle_Game_Over(1);
			break;
		}
	}

	public ushort Game_Handle_Choose_Map(ushort windowID)
	{
		return 0;
	}

	public void Game_Handle_Main_Menu(ushort action)
	{
		if (MainGame.trialMode)
		{
			mainC.userInterface.Swap_Text_Buttons(1, 5, 7);
		}
		else
		{
			mainC.userInterface.Swap_Text_Buttons(1, 7, 5);
		}
		mainC.userInterface.Reset_Text_Buttons_Font(1, 1);
		mainC.userInterface.Set_Text_Button_Font(1, User_Interface.windows[1].curTextButton, 3);
		switch (action)
		{
		case 0:
			switch (User_Interface.windows[1].state)
			{
			case 1:
				if (User_Interface.windows[1].returnValue == 1)
				{
					mainC.Quit_Game();
				}
				else if (User_Interface.windows[1].returnValue == 2)
				{
					User_Interface.windows[1].state = 0;
				}
				break;
			case 0:
				break;
			}
			break;
		case 1:
			global::InputHandler.InputHandler.checkForOtherControllers = false;
			break;
		case 2:
			Game_Show_Options_Window(1);
			break;
		case 3:
			if (MainGame.trialMode)
			{
				Game_Show_BuyMe_Window(0, 1);
			}
			else
			{
				mainC.maingameMain.Tell_A_Friend();
			}
			break;
		case 4:
			User_Interface.windows[1].returnValue = 0;
			User_Interface.windows[1].state = 1;
			mainC.userInterface.Show_Window(3, 1, resetButtons: true);
			mainC.userInterface.Hide_Text_Areas(3);
			mainC.userInterface.Set_Component_Status(7, 3, 1, 1);
			mainC.userInterface.Set_Component_Status(7, 3, 7, 1);
			mainC.userInterface.Set_Component_Status(7, 3, 8, 1);
			break;
		case 5:
			Game_Show_Play_Menu(1);
			break;
		case 6:
			Game_Show_Instructions_Window(1);
			break;
		case 7:
			Game_Show_Credits_Window(1);
			break;
		case 8:
			mainC.userInterface.Show_Window(6, 1, resetButtons: true);
			break;
		}
	}

	public byte Game_Handle_Play_Menu(ushort action)
	{
		switch (action)
		{
		case 0:
			switch (User_Interface.windows[7].state)
			{
			case 0:
				if (global::InputHandler.InputHandler.controllerButtonBPressed || global::InputHandler.InputHandler.controllerButtonBackPressed || global::InputHandler.InputHandler.controllerButtonStartPressed)
				{
					mainC.inputMain.Reset_Second_Controller_Checks();
				}
				if (global::InputHandler.InputHandler.controllerButtonBPressed)
				{
					mainC.userInterface.Close_Window(7);
				}
				break;
			case 10:
				MainGame.needToLoadWeapons = true;
				MainGame.allowTeamKills = false;
				MainGame.mapManuallySet = true;
				MainGame.maxGamePlayers = 44;
				MainGame.maxHumanGamePlayers = 4;
				MainGame.mp_numPlayers_index = 0;
				MainGame.gameLevel = 0;
				MainGame.difficulty = 0;
				MainGame.difficulty = 0;
				MainGame.mpNumPrivateGamerSlots = 0;
				global::Rendering.Rendering.renderMenuScreen = 1;
				mainC.maingameMain.Multiplayer_Start_Game_Creation_Process();
				try
				{
					if (MainGame.signedinGamerID > -1 && MainGame.signedinGamerID < Gamer.SignedInGamers.Count && Gamer.SignedInGamers[MainGame.signedinGamerID] != null && Gamer.SignedInGamers[MainGame.signedinGamerID].IsSignedInToLive)
					{
						Gamer.SignedInGamers[MainGame.signedinGamerID].Presence.PresenceMode = GamerPresenceMode.WaitingInLobby;
					}
				}
				catch (Exception)
				{
				}
				User_Interface.windows[7].state = 41;
				break;
			case 11:
				switch (mainC.networkingMain.XBOX_Signed_In(creatingGame: true))
				{
				case 1:
					if (playingPrivateMatch)
					{
						User_Interface.windows[7].state = 40;
						break;
					}
					User_Interface.windows[7].state = 15;
					mainC.userInterface.Show_Window(8, 7, resetButtons: true);
					mainC.userInterface.Hide_Text_Areas(8);
					mainC.userInterface.Set_Component_Status(7, 8, 0, 1);
					break;
				case 2:
					User_Interface.windows[7].state = 0;
					mainC.userInterface.Close_Window(8);
					break;
				}
				break;
			case 12:
				if (global::Networking.Networking.gameListSearchFinished)
				{
					if (global::Networking.Networking.numMPGames > 0)
					{
						mainC.userInterface.Show_Window(8, 7, resetButtons: true);
						mainC.userInterface.Hide_Text_Areas(8);
						mainC.userInterface.Set_Component_Status(7, 8, 1, 1);
						User_Interface.windows[7].state = 20;
					}
					else if (global::Networking.Networking.multiplayerNewGameStatus == 2)
					{
						mainC.userInterface.Show_Window(8, 7, resetButtons: true);
						mainC.userInterface.Hide_Text_Areas(8);
						mainC.userInterface.Set_Component_Status(7, 8, 2, 1);
						User_Interface.windows[7].state = 10;
					}
					else
					{
						mainC.userInterface.Close_Window(8);
						User_Interface.windows[7].state = 0;
					}
				}
				else
				{
					mainC.networkingMain.XBOX_Reset_Guide_Status();
					User_Interface.windows[7].state = 11;
				}
				break;
			case 13:
				if (!mainC.networkingMain.XBOX_Profile_Valid(creatingGame: true))
				{
					User_Interface.windows[7].state = 14;
					User_Interface.windows[7].returnValue = 0;
					mainC.userInterface.Show_Window(3, 7, resetButtons: true);
					mainC.userInterface.Hide_Text_Areas(3);
					mainC.userInterface.Set_Component_Status(7, 3, 4, 1);
					mainC.userInterface.Set_Component_Status(7, 3, 7, 1);
					mainC.userInterface.Set_Component_Status(7, 3, 8, 1);
				}
				else
				{
					User_Interface.windows[7].state = 11;
				}
				break;
			case 14:
				if (User_Interface.windows[7].returnValue == 1)
				{
					User_Interface.windows[7].state = 11;
				}
				else if (User_Interface.windows[7].returnValue == 2)
				{
					User_Interface.windows[7].state = 0;
				}
				break;
			case 15:
				User_Interface.windows[7].state = 12;
				mainC.networkingMain.XBOX_Get_Game_List(global::Networking.Networking.onlineSessionType);
				break;
			case 16:
				switch (mainC.maingameMain.SaveGame_Exists())
				{
				case 2:
					User_Interface.windows[7].state = 33;
					break;
				case 1:
					MainGame.gameLevel = 0;
					global::InputHandler.InputHandler.newSPGame = true;
					User_Interface.windows[7].state = 30;
					break;
				}
				break;
			case 17:
				if (MainGame.playerSettingsLoaded)
				{
					playingPrivateMatch = false;
					global::InputHandler.InputHandler.checkForOtherControllers = false;
					global::InputHandler.InputHandler.mpLive = false;
					mainC.networkingMain.XBOX_Reset_Guide_Status();
					global::Networking.Networking.onlineSessionType = NetworkSessionType.SystemLink;
					User_Interface.windows[7].state = 13;
				}
				else
				{
					MainGame.needToLoadPlayerSettings = true;
				}
				break;
			case 18:
				if (MainGame.playerSettingsLoaded)
				{
					playingPrivateMatch = false;
					global::InputHandler.InputHandler.checkForOtherControllers = false;
					global::InputHandler.InputHandler.mpLive = true;
					mainC.networkingMain.XBOX_Reset_Guide_Status();
					global::Networking.Networking.onlineSessionType = NetworkSessionType.PlayerMatch;
					User_Interface.windows[7].state = 13;
				}
				else
				{
					MainGame.needToLoadPlayerSettings = true;
				}
				break;
			case 19:
				if (MainGame.playerSettingsLoaded)
				{
					playingPrivateMatch = true;
					global::InputHandler.InputHandler.checkForOtherControllers = false;
					global::InputHandler.InputHandler.mpLive = true;
					mainC.networkingMain.XBOX_Reset_Guide_Status();
					global::Networking.Networking.onlineSessionType = NetworkSessionType.PlayerMatch;
					User_Interface.windows[7].state = 13;
				}
				else
				{
					MainGame.needToLoadPlayerSettings = true;
				}
				break;
			case 20:
				User_Interface.windows[7].state = 21;
				mainC.userInterface.Show_Window(8, 7, resetButtons: true);
				mainC.userInterface.Hide_Text_Areas(8);
				mainC.userInterface.Set_Component_Status(7, 8, 1, 1);
				MainGame.difficulty = 0;
				global::Rendering.Rendering.renderMenuScreen = 1;
				joinFailed = false;
				mainC.maingameMain.Multiplayer_Start_Join_First_Game_Process();
				try
				{
					if (MainGame.signedinGamerID > -1 && MainGame.signedinGamerID < Gamer.SignedInGamers.Count && Gamer.SignedInGamers[MainGame.signedinGamerID] != null && Gamer.SignedInGamers[MainGame.signedinGamerID].IsSignedInToLive)
					{
						Gamer.SignedInGamers[MainGame.signedinGamerID].Presence.PresenceMode = GamerPresenceMode.WaitingInLobby;
					}
				}
				catch (Exception)
				{
				}
				break;
			case 21:
				if (joinFailed)
				{
					mainC.userInterface.Show_Window(8, 7, resetButtons: true);
					mainC.userInterface.Hide_Text_Areas(8);
					mainC.userInterface.Set_Component_Status(7, 8, 2, 1);
					User_Interface.windows[7].state = 10;
				}
				break;
			case 30:
				if (MainGame.playerSettingsLoaded)
				{
					if (global::InputHandler.InputHandler.newSPGame)
					{
						MainGame.needToLoadWeapons = true;
					}
					MainGame.linearProgression = true;
					User_Interface.windows[7].state = 0;
					global::InputHandler.InputHandler.checkForOtherControllers = false;
					global::Networking.Networking.onlineSessionType = NetworkSessionType.Local;
					MainGame.maxGamePlayers = 44;
					MainGame.maxHumanGamePlayers = 1;
					MainGame.mp_numPlayers_index = 0;
					MainGame.numTeams = 2;
					MainGame.difficulty = 0;
					global::InputHandler.InputHandler.lastGpadID = global::InputHandler.InputHandler.gpadID;
					mainC.inputMain.Switch_To_Menu(0);
					global::InputHandler.InputHandler.menuStat = 2;
					mainC.userInterface.Close_Window(7);
					mainC.maingameMain.Add_Start_Of_Frame_Message(1);
				}
				else
				{
					MainGame.needToLoadPlayerSettings = true;
				}
				break;
			case 31:
				if (User_Interface.windows[7].returnValue == 1)
				{
					global::InputHandler.InputHandler.newSPGame = true;
					User_Interface.windows[7].state = 30;
				}
				else if (User_Interface.windows[7].returnValue == 2)
				{
					Game_UI_Update_Play_Window(7);
					User_Interface.windows[7].state = 0;
				}
				break;
			case 32:
				if (mainC.maingameMain.Load_SP_Game())
				{
					User_Interface.windows[7].state = 30;
				}
				break;
			case 33:
				mainC.userInterface.Set_Component_Status(6, 7, 0, 0);
				mainC.userInterface.Set_Component_Status(6, 7, 1, 0);
				mainC.userInterface.Set_Component_Status(6, 7, 2, 0);
				mainC.userInterface.Set_Component_Status(6, 7, 3, 0);
				mainC.userInterface.Set_Component_Status(6, 7, 4, 0);
				mainC.userInterface.Set_Component_Status(6, 7, 5, 0);
				mainC.userInterface.Set_Component_Status(6, 7, 6, 0);
				mainC.userInterface.Set_Component_Status(6, 7, 7, 2);
				mainC.userInterface.Set_Component_Status(6, 7, 8, 1);
				mainC.userInterface.Set_Component_Status(7, 7, 0, 0);
				mainC.userInterface.Set_Component_Status(7, 7, 1, 0);
				User_Interface.windows[7].curTextButton = 7;
				User_Interface.windows[7].state = 34;
				break;
			case 34:
				if (global::InputHandler.InputHandler.controllerButtonBackPressed || global::InputHandler.InputHandler.controllerButtonBPressed)
				{
					Game_UI_Update_Play_Window(7);
					User_Interface.windows[7].state = 0;
				}
				break;
			case 40:
				MainGame.needToLoadWeapons = true;
				MainGame.allowTeamKills = false;
				MainGame.mapManuallySet = true;
				MainGame.maxGamePlayers = 44;
				MainGame.maxHumanGamePlayers = 4;
				MainGame.mp_numPlayers_index = 0;
				MainGame.gameLevel = 0;
				MainGame.difficulty = 0;
				MainGame.mpNumPrivateGamerSlots = 3;
				global::Rendering.Rendering.renderMenuScreen = 1;
				mainC.maingameMain.Multiplayer_Start_Game_Creation_Process();
				try
				{
					if (MainGame.signedinGamerID > -1 && MainGame.signedinGamerID < Gamer.SignedInGamers.Count && Gamer.SignedInGamers[MainGame.signedinGamerID] != null && Gamer.SignedInGamers[MainGame.signedinGamerID].IsSignedInToLive)
					{
						Gamer.SignedInGamers[MainGame.signedinGamerID].Presence.PresenceMode = GamerPresenceMode.WaitingInLobby;
					}
				}
				catch (Exception)
				{
				}
				User_Interface.windows[7].state = 0;
				mainC.userInterface.Close_Window(8);
				mainC.userInterface.Close_Window(7);
				mainC.maingameMain.Add_Start_Of_Frame_Message(1);
				break;
			case 41:
				if (global::Networking.Networking.multiplayerNewGameStatus == 1)
				{
					User_Interface.windows[7].state = 0;
					mainC.maingameMain.Add_Start_Of_Frame_Message(1);
					mainC.userInterface.Close_Window(7);
					mainC.userInterface.Close_Window_After_Specified_Time(16, 0.5f);
					mainC.userInterface.Close_Window_After_Specified_Time(8, 0.5f);
				}
				else if (global::Networking.Networking.multiplayerNewGameStatus == 5)
				{
					User_Interface.windows[7].state = 0;
					mainC.userInterface.Close_Window(8);
				}
				break;
			}
			break;
		case 1:
			User_Interface.windows[7].state = 0;
			break;
		case 3:
			if (User_Interface.windows[7].state == 0)
			{
				if (MainGame.trialMode)
				{
					User_Interface.windows[7].state = 0;
					Game_Show_BuyMe_Window(1, 7);
					break;
				}
				MainGame.linearProgression = true;
				MainGame.needToLoadWeapons = true;
				MainGame.numTeams = 2;
				MainGame.gameType = 2;
				User_Interface.windows[7].state = 18;
			}
			break;
		case 4:
			if (User_Interface.windows[7].state == 0)
			{
				if (MainGame.trialMode)
				{
					User_Interface.windows[7].state = 0;
					Game_Show_BuyMe_Window(1, 7);
					break;
				}
				MainGame.linearProgression = true;
				MainGame.needToLoadWeapons = true;
				MainGame.numTeams = 2;
				MainGame.gameType = 2;
				User_Interface.windows[7].state = 17;
			}
			break;
		case 5:
			if (User_Interface.windows[7].state == 0)
			{
				User_Interface.windows[7].state = 16;
			}
			break;
		case 6:
			if (User_Interface.windows[7].state == 34)
			{
				global::InputHandler.InputHandler.newSPGame = false;
				User_Interface.windows[7].state = 32;
			}
			break;
		case 7:
			if (User_Interface.windows[7].state == 34)
			{
				User_Interface.windows[7].state = 31;
				User_Interface.windows[7].returnValue = 0;
				mainC.userInterface.Show_Window(3, 7, resetButtons: true);
				mainC.userInterface.Hide_Text_Areas(3);
				mainC.userInterface.Set_Component_Status(7, 3, 6, 1);
				mainC.userInterface.Set_Component_Status(7, 3, 7, 1);
				mainC.userInterface.Set_Component_Status(7, 3, 8, 1);
			}
			break;
		case 8:
			if (User_Interface.windows[7].state == 0)
			{
				if (MainGame.trialMode)
				{
					User_Interface.windows[7].state = 0;
					Game_Show_BuyMe_Window(1, 7);
					break;
				}
				MainGame.linearProgression = true;
				MainGame.needToLoadWeapons = true;
				MainGame.numTeams = 2;
				MainGame.gameType = 2;
				User_Interface.windows[7].state = 19;
			}
			break;
		case 9:
			if (User_Interface.windows[7].state == 0)
			{
				if (MainGame.trialMode)
				{
					User_Interface.windows[7].state = 0;
					Game_Show_BuyMe_Window(1, 7);
					break;
				}
				MainGame.linearProgression = true;
				MainGame.needToLoadWeapons = true;
				MainGame.numTeams = 5;
				MainGame.gameType = 7;
				User_Interface.windows[7].state = 17;
			}
			break;
		case 10:
			if (User_Interface.windows[7].state == 0)
			{
				if (MainGame.trialMode)
				{
					User_Interface.windows[7].state = 0;
					Game_Show_BuyMe_Window(1, 7);
					break;
				}
				MainGame.linearProgression = true;
				MainGame.needToLoadWeapons = true;
				MainGame.numTeams = 5;
				MainGame.gameType = 7;
				User_Interface.windows[7].state = 18;
			}
			break;
		case 11:
			if (User_Interface.windows[7].state == 0)
			{
				if (MainGame.trialMode)
				{
					User_Interface.windows[7].state = 0;
					Game_Show_BuyMe_Window(1, 7);
					break;
				}
				MainGame.linearProgression = true;
				MainGame.needToLoadWeapons = true;
				MainGame.numTeams = 5;
				MainGame.gameType = 7;
				User_Interface.windows[7].state = 19;
			}
			break;
		case 255:
			if (User_Interface.windows[7].state == 0)
			{
				return 0;
			}
			return 2;
		}
		return 2;
	}

	public void Game_Handle_BuyMe_Window(ushort action)
	{
		switch (action)
		{
		case 0:
			switch (User_Interface.windows[4].state)
			{
			case 1:
				if (mainC.networkingMain.XBOX_SignedIn_And_CanBuy())
				{
					mainC.networkingMain.XBOX_Reset_Guide_Status();
					User_Interface.windows[4].state = 2;
				}
				break;
			case 2:
				if (!mainC.networkingMain.XBOX_SignedIn_And_CanBuy())
				{
					mainC.userInterface.Show_Window(2, 4, resetButtons: true);
					User_Interface.windows[4].state = 1;
				}
				else if (MainGame.trialMode)
				{
					if (mainC.networkingMain.XBOX_Purchase_Game() == 3)
					{
						User_Interface.windows[4].state = 0;
					}
				}
				else
				{
					mainC.userInterface.Close_Window(4);
				}
				break;
			case 0:
				break;
			}
			break;
		case 1:
			if (!mainC.networkingMain.XBOX_SignedIn_And_CanBuy())
			{
				mainC.userInterface.Show_Window(2, 4, resetButtons: true);
			}
			User_Interface.windows[4].state = 1;
			break;
		case 2:
			mainC.userInterface.Close_Window(4);
			break;
		}
	}

	public void Game_Handle_SignIn_Window(ushort action)
	{
		switch (action)
		{
		case 0:
			switch (User_Interface.windows[2].state)
			{
			case 1:
			{
				byte b = mainC.networkingMain.XBOX_SignIn_To_Buy();
				if (b == 1 || b == 2)
				{
					mainC.userInterface.Close_Window(2);
				}
				break;
			}
			case 0:
				break;
			}
			break;
		case 1:
			User_Interface.windows[2].state = 1;
			break;
		case 2:
			mainC.userInterface.Close_Window(2);
			break;
		}
	}

	public byte Game_Handle_Confirm_Window(ushort action)
	{
		switch (action)
		{
		case 1:
			mainC.userInterface.Set_Window_Return_Value(3, 1);
			return 0;
		case 2:
			mainC.userInterface.Set_Window_Return_Value(3, 2);
			return 0;
		default:
			return 2;
		}
	}

	public void Game_Handle_Options(ushort action, ushort componentID, float value)
	{
		switch (action)
		{
		case 2:
			switch (componentID)
			{
			case 0:
				mainC.inputMain.UI_Set_Music_Status(value == 1f);
				break;
			case 1:
				mainC.inputMain.UI_Set_Sound_Effects_Status(value == 1f);
				break;
			case 2:
				mainC.inputMain.UI_Set_Music_Volume(value);
				break;
			case 3:
				mainC.inputMain.UI_Set_Sound_Effects_Volume(value);
				break;
			case 4:
				mainC.inputMain.UI_Set_Brightness(value);
				break;
			case 5:
				mainC.inputMain.UI_Set_Sensitivity(value);
				break;
			case 11:
				mainC.inputMain.UI_Set_Rumble((byte)value);
				break;
			case 12:
				mainC.inputMain.UI_Set_Invert_Y((byte)value);
				break;
			case 13:
				mainC.inputMain.UI_Set_Default_First_Person_View((byte)value);
				global::Players.Players.players[0].playerModel[0] = playerModelTPV;
				if (global::Players.Players.currentView == 1 && global::Players.Players.players[global::Rendering.Rendering.watchingPlayer].onmap != 8)
				{
					global::Players.Players.players[0].playerModel[0] = playerModelFPV;
				}
				break;
			case 14:
				mainC.inputMain.UI_Set_SwapSticks(value == 1f);
				break;
			case 6:
			case 7:
			case 8:
			case 9:
			case 10:
				break;
			}
			break;
		case 1:
			MainGame.needToSavePlayerSettings = true;
			break;
		}
	}

	public byte Game_Handle_Game_Over(ushort action)
	{
		switch (User_Interface.windows[23].state)
		{
		case 0:
		case 1:
			switch (action)
			{
			case 1:
				if (MainGame.gameMode == 1)
				{
					mainC.maingameMain.Exit_To_Title_From_Menu();
				}
				else
				{
					mainC.soundsMain.Stop_Narrator_Voice();
					MainGame.gameState = 1;
					mainC.userInterface.Load_Main_Menu();
				}
				return 0;
			case 2:
				User_Interface.windows[23].state = 2;
				mainC.soundsMain.Stop_Narrator_Voice();
				global::Rendering.Rendering.renderMenuScreen = 1;
				mainC.userInterface.Close_Window(23);
				mainC.maingameMain.Restart_Level_From_Menu();
				break;
			case 3:
				User_Interface.windows[23].state = 1;
				Game_Show_BuyMe_Window(2, 11);
				break;
			default:
				if (User_Interface.windows[23].state == 1 && !MainGame.trialMode)
				{
					MainGame.gameLevel++;
					User_Interface.windows[23].state = 2;
					mainC.soundsMain.Stop_Narrator_Voice();
					global::Rendering.Rendering.renderMenuScreen = 1;
					mainC.userInterface.Close_Window(23);
					MainGame.spSaving = 1;
					MainGame.gameState = 6;
					mainC.gameLogic.Game_SP_Round_Over();
					mainC.gameLogic.Game_Show_Results_Window();
				}
				else if (MainGame.gameState == 8 || MainGame.gameState == 147)
				{
					User_Interface.windows[23].state = 3;
				}
				break;
			}
			break;
		case 3:
			if (action == 1)
			{
				if (MainGame.gameMode == 1)
				{
					mainC.maingameMain.Exit_To_Title_From_Menu();
				}
				else
				{
					MainGame.gameState = 1;
					mainC.userInterface.Load_Main_Menu();
				}
				return 0;
			}
			break;
		}
		return 2;
	}

	public byte Game_Handle_In_Game_Menu(ushort action)
	{
		switch (action)
		{
		case 0:
			switch (User_Interface.windows[5].state)
			{
			case 1:
				if (User_Interface.windows[5].returnValue == 1)
				{
					float num = global::Players.Players.players[0].charP.position.v[0];
					float num2 = global::Players.Players.players[0].charP.position.v[1];
					float z = global::Players.Players.players[0].charP.position.v[2];
					float num3 = 360f / (float)(int)MainGame.playerVehicles[0].numWeapons;
					float num4 = 0f;
					for (ushort num5 = 0; num5 < MainGame.playerVehicles[0].numWeapons; num5++)
					{
						if (MainGame.playerVehicles[0].mounts[MainGame.primaryWeaponMount].objectID != num5)
						{
							float num6 = (float)Math.Sin(num3) * 2.5f * 1f;
							float num7 = (float)Math.Cos(num3) * 2.5f * 1f;
							if (MainGame.playerVehicles[0].weapons[num5].active && MainGame.playerVehicles[0].weapons[num5].weaponID != 6 && MainGame.playerVehicles[0].weapons[num5].weaponID != 1)
							{
								mainC.pickupsMain.Player_Drops_Weapon(MainGame.playerVehicles[0].weapons[num5].weaponID, num + num6, num2 + num7, z, sendToNetwork: true);
							}
							num4 += num3;
						}
					}
					User_Interface.windows[5].state = 0;
					mainC.userInterface.Close_Window(5);
					mainC.maingameMain.Exit_To_Title_From_Menu();
				}
				else if (User_Interface.windows[5].returnValue == 2)
				{
					User_Interface.windows[5].state = 0;
				}
				break;
			case 2:
				if (User_Interface.windows[5].returnValue == 1)
				{
					User_Interface.windows[5].state = 0;
					mainC.userInterface.Close_Window(5);
					mainC.maingameMain.Respawn_From_Menu();
				}
				else if (User_Interface.windows[5].returnValue == 2)
				{
					User_Interface.windows[5].state = 0;
				}
				break;
			case 3:
				if (User_Interface.windows[5].returnValue == 1)
				{
					User_Interface.windows[5].state = 0;
					mainC.userInterface.Close_Window(5);
					mainC.maingameMain.Restart_Level_From_Menu();
				}
				else if (User_Interface.windows[5].returnValue == 2)
				{
					User_Interface.windows[5].state = 0;
				}
				break;
			}
			break;
		case 1:
			MainGame.needToSavePlayerSettings = true;
			mainC.inputMain.Leave_Menu_Completely();
			mainC.maingameMain.Leaving_Menu_State();
			break;
		case 7:
			mainC.userInterface.Show_Window(6, 5, resetButtons: true);
			break;
		case 2:
			Game_Show_Options_Window(5);
			break;
		case 3:
			User_Interface.windows[5].returnValue = 0;
			User_Interface.windows[5].state = 1;
			mainC.userInterface.Show_Window(3, 5, resetButtons: true);
			mainC.userInterface.Hide_Text_Areas(3);
			mainC.userInterface.Set_Component_Status(7, 3, 0, 1);
			mainC.userInterface.Set_Component_Status(7, 3, 7, 1);
			mainC.userInterface.Set_Component_Status(7, 3, 8, 1);
			break;
		case 4:
			User_Interface.windows[5].returnValue = 0;
			User_Interface.windows[5].state = 2;
			mainC.userInterface.Show_Window(3, 5, resetButtons: true);
			mainC.userInterface.Hide_Text_Areas(3);
			mainC.userInterface.Set_Component_Status(7, 3, 3, 1);
			mainC.userInterface.Set_Component_Status(7, 3, 7, 1);
			mainC.userInterface.Set_Component_Status(7, 3, 8, 1);
			break;
		case 5:
			User_Interface.windows[5].returnValue = 0;
			User_Interface.windows[5].state = 3;
			mainC.userInterface.Show_Window(3, 5, resetButtons: true);
			mainC.userInterface.Hide_Text_Areas(3);
			mainC.userInterface.Set_Component_Status(7, 3, 2, 1);
			mainC.userInterface.Set_Component_Status(7, 3, 7, 1);
			mainC.userInterface.Set_Component_Status(7, 3, 8, 1);
			break;
		case 6:
			if (MainGame.trialMode)
			{
				Game_Show_BuyMe_Window(0, 5);
			}
			else
			{
				mainC.maingameMain.Tell_A_Friend();
			}
			break;
		}
		return 2;
	}

	public void Game_Handle_Instructions_Window(ushort action)
	{
		User_Interface.windows[13].textAreas[0].lines[0] = "Objective";
		User_Interface.windows[13].textAreas[1].lines[0] = "Multiplayer";
		User_Interface.windows[13].textAreas[2].lines[0] = "Weapons";
		User_Interface.windows[13].textAreas[3].numLines = 3;
		User_Interface.windows[13].textAreas[3].lines = new string[3];
		User_Interface.windows[13].textAreas[3].lines[0] = "Clear the area of zombies then reach the debris pile to move on. You can carry two weapons";
		User_Interface.windows[13].textAreas[3].lines[1] = "at a time. If you die, you will drop any weapons you have picked up. You can find new weapons and";
		User_Interface.windows[13].textAreas[3].lines[2] = "extra ammo inside of crates by shooting or smashing them.";
		User_Interface.windows[13].textAreas[4].numLines = 3;
		User_Interface.windows[13].textAreas[4].lines = new string[3];
		User_Interface.windows[13].textAreas[4].lines[0] = "In co-op, you and your teammates work together to clear the areas.  Once an area is cleared,";
		User_Interface.windows[13].textAreas[4].lines[1] = "move towards the debris pile to continue. In free for all, you are fighting against the other players as ";
		User_Interface.windows[13].textAreas[4].lines[2] = "well as the zombies. You get more points for human kills than zombie kills.";
		User_Interface.windows[13].textAreas[5].numLines = 4;
		User_Interface.windows[13].textAreas[5].lines = new string[4];
		User_Interface.windows[13].textAreas[5].lines[0] = "You always start with a pistol and can acquire other weapons throughout the level and any ";
		User_Interface.windows[13].textAreas[5].lines[1] = "dropped weapons will remain where you left them. Melee weapons such as the crowbar are useful";
		User_Interface.windows[13].textAreas[5].lines[2] = "since they never run out of ammo and can always open crates.";
		User_Interface.windows[13].textAreas[5].lines[3] = "";
	}

	public void Game_Handle_Results_Window(ushort action)
	{
		if (User_Interface.windows[10].needsUpdating)
		{
			Game_UI_Update_Results_Window(10);
		}
		if (MainGame.gameMode == 0 && MainGame.roundCurrentTime > 0f)
		{
			MainGame.roundCurrentTime = 2f;
			mainC.userInterface.Set_Component_Status(6, 10, 1, 2);
			mainC.userInterface.Set_Component_To_Current_Component(6, 10, 1);
		}
		else if (MainGame.gameMode == 1 && MainGame.showResultsTimer >= 0f)
		{
			ushort num = (ushort)Math.Ceiling(MainGame.showResultsTimer);
			User_Interface.windows[10].textAreas[5].lines[0] = num.ToString(CultureInfo.InvariantCulture) + " Seconds";
			if (global::Networking.Networking.networkSession.RemoteGamers.Count < 1)
			{
				MainGame.showResultsTimer = -1f;
			}
			MainGame.showResultsTimer -= MainGame.frametime;
			if (MainGame.showResultsTimer < 0f)
			{
				MainGame.showResultsTimer = -1f;
				if (global::Networking.Networking.networkSession.RemoteGamers.Count < 1)
				{
					mainC.userInterface.Set_Component_Status(6, 10, 1, 2);
					mainC.userInterface.Set_Component_To_Current_Component(6, 10, 1);
				}
				else
				{
					mainC.userInterface.Set_Component_Status(6, 10, 0, 2);
					mainC.userInterface.Set_Component_To_Current_Component(6, 10, 0);
				}
				mainC.userInterface.Set_Component_Status(7, 10, 5, 0);
				mainC.userInterface.Set_Component_Status(8, 10, 4, 0);
				mainC.userInterface.Set_Component_Status(8, 10, 5, 1);
			}
		}
		switch (action)
		{
		case 0:
			switch (User_Interface.windows[10].state)
			{
			case 1:
				if (User_Interface.windows[10].returnValue == 1)
				{
					User_Interface.windows[10].state = 0;
					mainC.userInterface.Close_Window(10);
					mainC.maingameMain.Exit_To_Title_From_Menu();
				}
				else if (User_Interface.windows[10].returnValue == 2)
				{
					User_Interface.windows[10].state = 0;
				}
				break;
			case 0:
				break;
			}
			break;
		case 1:
			if (MainGame.gameMode == 0)
			{
				MainGame.roundCurrentTime = -1f;
			}
			else
			{
				if (MainGame.gameMode != 1 || global::Networking.Networking.networkState != 1)
				{
					break;
				}
				if (!global::Networking.Networking.isHost || global::Networking.Networking.networkSession.RemoteGamers.Count > 0)
				{
					if (MainGame.localNetworkGamerID < global::Networking.Networking.networkSession.LocalGamers.Count)
					{
						global::Networking.Networking.networkSession.LocalGamers[MainGame.localNetworkGamerID].IsReady = !global::Networking.Networking.networkSession.LocalGamers[MainGame.localNetworkGamerID].IsReady;
					}
				}
				else
				{
					MainGame.hostStartedGame = true;
				}
			}
			break;
		case 2:
			User_Interface.windows[10].returnValue = 0;
			User_Interface.windows[10].state = 1;
			mainC.userInterface.Show_Window(3, 10, resetButtons: true);
			mainC.userInterface.Hide_Text_Areas(3);
			mainC.userInterface.Set_Component_Status(7, 3, 0, 1);
			mainC.userInterface.Set_Component_Status(7, 3, 7, 1);
			mainC.userInterface.Set_Component_Status(7, 3, 8, 1);
			break;
		case 3:
		case 4:
			break;
		}
	}

	public byte Game_Handle_Mission_Objectives(ushort action)
	{
		if (action == 1)
		{
			User_Interface.missionObjectivesFinished = true;
			mainC.soundsMain.Stop_Narrator_Voice();
			return 0;
		}
		switch (User_Interface.windows[22].state)
		{
		case 0:
			if (MainGame.roundStarting)
			{
				mainC.soundsMain.Play_Narrator_Voice("Level_" + MainGame.gameLevel.ToString(CultureInfo.InvariantCulture));
			}
			User_Interface.windows[22].state = 1;
			break;
		case 1:
			if (!mainC.soundsMain.Is_Narrator_Playing())
			{
				mainC.userInterface.Swap_Buttons(22, 1, 0);
				User_Interface.windows[22].state = 2;
			}
			break;
		}
		return 2;
	}

	public byte Game_Handle_Vehicle_Select(ushort action)
	{
		if (MainGame.gameMode == 1)
		{
			User_Interface.vehicleSelectTimer += MainGame.frametime;
			ushort num = (ushort)(User_Interface.vehicleSelectAutoStartTime - User_Interface.vehicleSelectTimer);
			User_Interface.windows[255].textAreas[4].lines[0] = "Auto Start in " + num.ToString(CultureInfo.InvariantCulture) + " Seconds";
			if (User_Interface.vehicleSelectTimer > 15f)
			{
				if (global::Players.Players.currentPlayerRank < Vehicles.lockedVehicleLevels[User_Interface.curVehicleSelect])
				{
					if (global::Players.Players.currentPlayerRank >= Vehicles.lockedVehicleLevels[User_Interface.lastVehicleSelected])
					{
						User_Interface.curVehicleSelect = User_Interface.lastVehicleSelected;
					}
					else
					{
						User_Interface.curVehicleSelect = 0;
						while (User_Interface.curVehicleSelect < Vehicles.numVehicles && global::Players.Players.currentPlayerRank < Vehicles.lockedVehicleLevels[User_Interface.curVehicleSelect])
						{
							User_Interface.curVehicleSelect++;
						}
					}
					Game_UI_Update_Vehicle_Select_Set();
				}
				mainC.userInterface.Close_Window(255);
				return 0;
			}
		}
		else if (MainGame.gameMode == 0)
		{
			User_Interface.hideVehicle = false;
			if (MainGame.trialMode && global::Players.Players.playerRankSP >= 3 && !askedToBuy)
			{
				User_Interface.hideVehicle = true;
				Game_Show_BuyMe_Window(3, 255);
				askedToBuy = true;
			}
		}
		switch (action)
		{
		case 0:
			if (Vehicles.numVehicles > 0)
			{
				if (global::InputHandler.InputHandler.controllerDPadRightPressed)
				{
					User_Interface.curVehicleSelect++;
					if (User_Interface.curVehicleSelect >= Vehicles.numVehicles)
					{
						User_Interface.curVehicleSelect = 0;
					}
					Game_UI_Update_Vehicle_Select_Set();
				}
				else if (global::InputHandler.InputHandler.controllerDPadLeftPressed)
				{
					User_Interface.curVehicleSelect--;
					if (User_Interface.curVehicleSelect >= Vehicles.numVehicles)
					{
						User_Interface.curVehicleSelect = (ushort)(Vehicles.numVehicles - 1);
					}
					Game_UI_Update_Vehicle_Select_Set();
				}
				if (global::Players.Players.currentPlayerRank < Vehicles.lockedVehicleLevels[User_Interface.curVehicleSelect])
				{
					User_Interface.windows[255].buttons[0].status = 0;
				}
				else
				{
					User_Interface.windows[255].buttons[0].status = 1;
				}
			}
			return 2;
		case 1:
			if (global::Players.Players.currentPlayerRank >= Vehicles.lockedVehicleLevels[User_Interface.curVehicleSelect])
			{
				return 0;
			}
			return 2;
		default:
			return 2;
		}
	}

	public byte Game_Handle_Weapon_Select(ushort action)
	{
		return 2;
	}

	public void Game_Handle_Scores_Window()
	{
		if (User_Interface.windows[9].needsUpdating)
		{
			Game_UI_Update_Scores_Window(9);
		}
	}

	public void Game_Show_BuyMe_Window(byte mode, ushort parentID)
	{
		User_Interface.windows[4].textAreas[1].status = 0;
		User_Interface.windows[4].textAreas[4].status = 0;
		switch (mode)
		{
		case 1:
			User_Interface.windows[4].textAreas[4].status = 1;
			break;
		case 2:
			User_Interface.windows[4].textAreas[1].status = 1;
			break;
		case 3:
			User_Interface.windows[4].textAreas[1].status = 1;
			break;
		}
		mainC.userInterface.Show_Window(4, parentID, resetButtons: true);
	}

	public void Game_Show_Options_Window(ushort parentID)
	{
		Game_UI_Update_Options_Window(0);
		mainC.userInterface.Show_Window(0, parentID, resetButtons: false);
	}

	public void Game_Show_MainMenu_Window()
	{
		if (MainGame.trialMode)
		{
			mainC.userInterface.Swap_Text_Buttons(1, 5, 7);
		}
		else
		{
			mainC.userInterface.Swap_Text_Buttons(1, 7, 5);
		}
		mainC.userInterface.Reset_Text_Buttons_Font(1, 1);
		mainC.userInterface.Set_Text_Button_Font(1, User_Interface.windows[1].curTextButton, 3);
		mainC.userInterface.Show_Window(1, 1, resetButtons: true);
	}

	public void Game_Show_Scores_Window()
	{
		byte b = 9;
		Game_UI_Update_Scores_Window(b);
		mainC.userInterface.Show_Window(b, b, resetButtons: false);
	}

	public void Game_Show_Results_Window()
	{
		if (MainGame.gameMode == 0)
		{
			switch (MainGame.gameState)
			{
			case 6:
				MainGame.needToSavePlayerSettings = true;
				Game_UI_Update_Results_Window(10);
				mainC.userInterface.Show_Window(10, 10, resetButtons: false);
				break;
			case 8:
			case 10:
				mainC.userInterface.Load_Game_Over();
				break;
			case 7:
			case 9:
				break;
			}
		}
		else
		{
			byte gameState = MainGame.gameState;
			if (gameState == 147)
			{
				mainC.userInterface.Load_Game_Over();
				return;
			}
			MainGame.needToSavePlayerSettings = true;
			Game_UI_Update_Results_Window(10);
			mainC.userInterface.Show_Window(10, 10, resetButtons: false);
		}
	}

	public void Game_Show_Credits_Window(ushort parentID)
	{
		mainC.userInterface.Show_Window(12, parentID, resetButtons: false);
	}

	public void Game_Show_Instructions_Window(ushort parentID)
	{
		User_Interface.curInstructionPage = 0;
		Game_Handle_Instructions_Window(0);
		mainC.userInterface.Show_Window(13, parentID, resetButtons: false);
	}

	public void Game_Show_Play_Menu(ushort parentID)
	{
		global::InputHandler.InputHandler.checkForOtherControllers = false;
		Game_UI_Update_Play_Window(7);
		User_Interface.windows[7].state = 0;
		mainC.userInterface.Show_Window(7, parentID, resetButtons: false);
		global::Networking.Networking.multiplayerNewGameStatus = 0;
	}

	public void Game_Update_Windows_For_Purchase()
	{
		if (User_Interface.numWindows >= 1)
		{
			mainC.userInterface.Swap_Text_Buttons(1, 5, 7);
		}
	}

	public void Game_UI_Update_Game_Over(ushort windowID)
	{
		User_Interface.windows[windowID].state = 0;
		mainC.userInterface.Set_Component_Status(8, 23, 2, 0);
		switch (MainGame.gameState)
		{
		case 8:
		case 147:
			mainC.userInterface.Set_Component_Status(7, windowID, 0, 0);
			mainC.userInterface.Set_Component_Status(7, windowID, 1, 1);
			mainC.userInterface.Set_Component_Status(7, windowID, 2, 1);
			mainC.userInterface.Set_Component_Status(6, windowID, 0, 0);
			mainC.userInterface.Set_Component_Status(6, windowID, 1, 2);
			mainC.userInterface.Set_Component_Status(6, windowID, 2, 0);
			mainC.userInterface.Set_Component_To_Current_Component(6, windowID, 1);
			User_Interface.windows[windowID].textButtons[1].componentDown = 1;
			User_Interface.windows[windowID].textButtons[1].componentUp = 1;
			break;
		case 10:
			mainC.userInterface.Set_Component_Status(7, windowID, 0, 1);
			mainC.userInterface.Set_Component_Status(7, windowID, 1, 0);
			mainC.userInterface.Set_Component_Status(7, windowID, 2, 0);
			mainC.userInterface.Set_Component_Status(6, windowID, 0, 0);
			mainC.userInterface.Set_Component_Status(6, windowID, 1, 1);
			mainC.userInterface.Set_Component_Status(6, windowID, 2, 2);
			mainC.userInterface.Set_Component_To_Current_Component(6, windowID, 2);
			User_Interface.windows[windowID].textButtons[1].componentDown = 2;
			User_Interface.windows[windowID].textButtons[1].componentUp = 2;
			break;
		case 22:
			mainC.userInterface.Set_Component_Status(7, windowID, 0, 0);
			mainC.userInterface.Set_Component_Status(7, windowID, 1, 1);
			mainC.userInterface.Set_Component_Status(7, windowID, 2, 0);
			mainC.userInterface.Set_Component_Status(6, windowID, 0, 2);
			mainC.userInterface.Set_Component_Status(6, windowID, 1, 1);
			mainC.userInterface.Set_Component_Status(6, windowID, 2, 0);
			mainC.userInterface.Set_Component_To_Current_Component(6, windowID, 0);
			User_Interface.windows[windowID].textButtons[1].componentDown = 0;
			User_Interface.windows[windowID].textButtons[1].componentUp = 0;
			break;
		default:
			mainC.userInterface.Set_Component_Status(7, windowID, 0, 0);
			mainC.userInterface.Set_Component_Status(8, windowID, 0, 1);
			mainC.userInterface.Set_Component_Status(8, windowID, 1, 0);
			mainC.userInterface.Set_Component_Status(1, windowID, 0, 0);
			mainC.userInterface.Set_Component_Status(1, windowID, 2, 0);
			User_Interface.windows[windowID].curButton = 1;
			User_Interface.windows[windowID].buttons[1].componentDown = 1;
			User_Interface.windows[windowID].buttons[1].componentUp = 1;
			User_Interface.windows[windowID].buttons[1].status = 2;
			break;
		}
	}

	public void Game_UI_Update_Play_Window(ushort windowID)
	{
		mainC.userInterface.Set_Component_Status(6, 7, 0, 1);
		mainC.userInterface.Set_Component_Status(6, 7, 1, 1);
		mainC.userInterface.Set_Component_Status(6, 7, 2, 1);
		mainC.userInterface.Set_Component_Status(6, 7, 3, 1);
		mainC.userInterface.Set_Component_Status(6, 7, 4, 1);
		mainC.userInterface.Set_Component_Status(6, 7, 5, 1);
		mainC.userInterface.Set_Component_Status(6, 7, 6, 1);
		mainC.userInterface.Set_Component_Status(6, 7, 7, 0);
		mainC.userInterface.Set_Component_Status(6, 7, 8, 0);
		mainC.userInterface.Set_Component_Status(7, 7, 0, 1);
		mainC.userInterface.Set_Component_Status(7, 7, 1, 1);
		mainC.userInterface.Set_Component_Status(1, 7, 0, 0);
		mainC.userInterface.Set_Component_Status(1, 7, 1, 0);
		mainC.userInterface.Set_Component_Status(1, 7, 2, 0);
		mainC.userInterface.Set_Component_Status(1, 7, 3, 0);
		mainC.userInterface.Set_Component_Status(1, 7, 4, 0);
		mainC.userInterface.Set_Component_Status(1, 7, 5, 0);
		mainC.userInterface.Set_Component_Status(1, 7, 6, 0);
		if (User_Interface.windows[7].curTextButton > 6)
		{
			User_Interface.windows[7].curTextButton = 0;
		}
		mainC.userInterface.Set_Component_Status(6, 7, User_Interface.windows[7].curTextButton, 2);
	}

	public void Game_UI_Update_Results_Window(ushort windowID)
	{
		Game_Rank_Players(0);
		ushort num = team0PlayerCount;
		for (ushort num2 = 0; num2 < 5; num2++)
		{
			User_Interface.windows[windowID].textAreas[num2].numLines = num;
			User_Interface.windows[windowID].textAreas[num2].lines = new string[num];
		}
		User_Interface.windows[windowID].textAreas[11].numLines = num;
		User_Interface.windows[windowID].textAreas[11].lines = new string[num];
		ushort num3 = 0;
		num = 0;
		while (num3 < team0PlayerCount)
		{
			ushort num2 = team0Players[num3];
			User_Interface.windows[windowID].textAreas[0].lines[num] = global::Players.Players.players[num2].abreviateName;
			User_Interface.windows[windowID].textAreas[1].lines[num] = string.Format(CultureInfo.InvariantCulture, "{0:###,##0}", new object[1] { MainGame.gameData.players[num2].numKills });
			User_Interface.windows[windowID].textAreas[2].lines[num] = string.Format(CultureInfo.InvariantCulture, "{0:###,##0}", new object[1] { MainGame.gameData.players[num2].numDeaths });
			if (MainGame.gameData.players[num2].shotsFired != 0)
			{
				ushort num4 = (ushort)((float)(int)MainGame.gameData.players[num2].shotsHit / (float)(int)MainGame.gameData.players[num2].shotsFired * 100f);
				User_Interface.windows[windowID].textAreas[3].lines[num] = num4.ToString(CultureInfo.InvariantCulture) + "%";
			}
			else
			{
				User_Interface.windows[windowID].textAreas[3].lines[num] = "0";
			}
			User_Interface.windows[windowID].textAreas[4].lines[num] = string.Format(CultureInfo.InvariantCulture, "{0:###,##0}", new object[1] { MainGame.gameData.players[num2].scoresI[0] });
			User_Interface.windows[windowID].textAreas[11].lines[num] = string.Format(CultureInfo.InvariantCulture, "{0:###,##0}", new object[1] { global::Players.Players.maxKillStreak[num2] });
			num3++;
			num++;
		}
		mainC.userInterface.Set_Component_Status(8, 10, 4, 0);
		mainC.userInterface.Set_Component_Status(8, 10, 5, 1);
		if (MainGame.showResultsTimer > 0f)
		{
			mainC.userInterface.Set_Component_Status(8, 10, 4, 1);
			mainC.userInterface.Set_Component_Status(8, 10, 5, 0);
		}
		if (MainGame.gameMode == 0 || global::Networking.Networking.networkSession.RemoteGamers.Count < 1)
		{
			mainC.userInterface.Set_Component_Status(6, 10, 0, 0);
			mainC.userInterface.Set_Component_Status(6, 10, 1, 1);
			mainC.userInterface.Set_Component_Status(7, 10, 5, 0);
			for (ushort num2 = 0; num2 < 4; num2++)
			{
				mainC.userInterface.Set_Component_Status(8, windowID, num2, 0);
			}
		}
		else
		{
			mainC.userInterface.Set_Component_Status(7, 10, 5, 1);
			mainC.userInterface.Set_Component_Status(6, 10, 0, 1);
			mainC.userInterface.Set_Component_Status(6, 10, 1, 0);
		}
		User_Interface.windows[10].needsUpdating = false;
	}

	public void Game_UI_Update_Results_Window_Network_Info(ushort windowID)
	{
		if (!global::Networking.Networking.networkSessionReady)
		{
			return;
		}
		for (int i = 0; i < 4; i++)
		{
			mainC.userInterface.Set_Component_Status(8, windowID, (ushort)i, 0);
		}
		int j = 0;
		int num = 0;
		for (; j < team0PlayerCount; j++)
		{
			int i = team0Players[j];
			if (i >= 1)
			{
				num = mainC.playersMain.Get_RemoteGamer_Index((byte)global::Players.Players.players[i].id, -1);
				if (num > -1 && global::Networking.Networking.networkSession.RemoteGamers[num].IsReady)
				{
					mainC.userInterface.Set_Component_Status(8, windowID, (ushort)j, 1);
				}
			}
			else if (MainGame.localNetworkGamerID < global::Networking.Networking.networkSession.LocalGamers.Count && global::Networking.Networking.networkSession.LocalGamers[MainGame.localNetworkGamerID].IsReady)
			{
				mainC.userInterface.Set_Component_Status(8, windowID, (ushort)j, 1);
			}
		}
	}

	public void Game_UI_Update_Scores_Window(ushort windowID)
	{
	}

	public void Game_UI_Update_Options_Window(ushort windowID)
	{
		User_Interface.windows[windowID].checkBoxes[0].value = (byte)(global::Sounds.Sounds.soundEnabled[1] ? 1 : 0);
		User_Interface.windows[windowID].checkBoxes[1].value = (byte)(global::Sounds.Sounds.soundEnabled[0] ? 1 : 0);
		User_Interface.windows[windowID].checkBoxes[2].value = global::InputHandler.InputHandler.rumble;
		User_Interface.windows[windowID].checkBoxes[3].value = ((global::Players.Players.invertY != 1f) ? ((byte)1) : ((byte)0));
		User_Interface.windows[windowID].checkBoxes[4].value = (byte)((global::Players.Players.lastView != 0) ? 1 : 0);
		User_Interface.windows[windowID].checkBoxes[5].value = (byte)(global::InputHandler.InputHandler.swapSticks ? 1 : 0);
		User_Interface.windows[windowID].sliders[0].value = (global::Sounds.Sounds.volume[1] + 96f) / 102f;
		User_Interface.windows[windowID].sliders[1].value = (global::Sounds.Sounds.volume[0] + 96f) / 102f;
		User_Interface.windows[windowID].sliders[2].value = global::Rendering.Rendering.brightness;
		User_Interface.windows[windowID].sliders[3].value = (global::InputHandler.InputHandler.lookSensitivity[global::InputHandler.InputHandler.lookModeAdj] - 0.15f) / 0.85f;
		global::Players.Players.players[0].playerModel[0] = playerModelTPV;
		if (global::Players.Players.currentView == 1 && global::Players.Players.players[global::Rendering.Rendering.watchingPlayer].onmap != 8)
		{
			global::Players.Players.players[0].playerModel[0] = playerModelFPV;
		}
	}

	public void Game_UI_Update_Vehicle_Select_Set()
	{
		User_Interface.windows[255].textAreas[4].status = 0;
		if (MainGame.gameMode == 1)
		{
			User_Interface.windows[255].textAreas[4].status = 1;
			ushort num = (ushort)(User_Interface.vehicleSelectAutoStartTime - User_Interface.vehicleSelectTimer);
			User_Interface.windows[255].textAreas[4].lines[0] = "Auto Start in " + num + " Seconds";
		}
		ushort numPlayerRaces = global::Players.Players.numPlayerRaces;
		for (ushort num = 0; num < numPlayerRaces; num++)
		{
			ushort num2 = 0;
			ushort numTypes = global::Players.Players.playerRaces[num].numTypes;
			while (num2 < numTypes)
			{
				if (global::Players.Players.playerRaces[num].vehicleID[num2] == Vehicles.vehicelSelectVehicleIDs[User_Interface.curVehicleSelect])
				{
					mainC.playersMain.Set_Player_Race(0, (byte)num, (sbyte)num2);
					break;
				}
				num2++;
			}
		}
		global::Joints.Joints.Reset_Joint_Data(0);
		global::Joints.Joints.Moved_Joint_Calculations(0, global::Joints.Joints.playerJoints[global::Players.Players.players[0].jointPackage].numJoints);
		global::Joints.Joints.Sync_Player_Matrices(0, global::Rendering.Rendering.uBufferID, global::Rendering.Rendering.rBufferID);
		switch (Vehicles.vehicelSelectVehicleIDs[User_Interface.curVehicleSelect])
		{
		case 0:
			global::Rendering.Rendering.vehicleSelectMatrix = Matrix.CreateScale(57f) * Matrix.CreateTranslation(0f, 0f, -120f);
			User_Interface.windows[255].textAreas[1].lines[0] = "Speed:";
			User_Interface.windows[255].textAreas[2].lines[0] = "Average (150/200)";
			User_Interface.windows[255].textAreas[1].lines[1] = "Armor:";
			User_Interface.windows[255].textAreas[2].lines[1] = "Average (125/200)";
			User_Interface.windows[255].textAreas[1].lines[2] = "Speed Boost:";
			User_Interface.windows[255].textAreas[2].lines[2] = "Average";
			User_Interface.windows[255].textAreas[1].lines[3] = "";
			User_Interface.windows[255].textAreas[1].lines[4] = "A good all around aircraft.  Effective against both ground";
			User_Interface.windows[255].textAreas[1].lines[5] = "and air targets. Handles very well.";
			User_Interface.windows[255].textAreas[1].lines[6] = "";
			User_Interface.windows[255].textAreas[1].lines[7] = "";
			User_Interface.windows[255].textAreas[1].lines[8] = "";
			break;
		case 1:
			global::Rendering.Rendering.vehicleSelectMatrix = Matrix.CreateScale(57f) * Matrix.CreateTranslation(0f, 0f, -120f);
			User_Interface.windows[255].textAreas[1].lines[0] = "Speed:";
			User_Interface.windows[255].textAreas[2].lines[0] = "Slowest (125/200)";
			User_Interface.windows[255].textAreas[1].lines[1] = "Armor:";
			User_Interface.windows[255].textAreas[2].lines[1] = "Best (200/200)";
			User_Interface.windows[255].textAreas[1].lines[2] = "Speed Boost:";
			User_Interface.windows[255].textAreas[2].lines[2] = "Minimal";
			User_Interface.windows[255].textAreas[1].lines[3] = "";
			User_Interface.windows[255].textAreas[1].lines[4] = "A more combat ready aircraft.  Effective against both ground";
			User_Interface.windows[255].textAreas[1].lines[5] = "and air targets. Handles a bit sluggish due to it's heavier.";
			User_Interface.windows[255].textAreas[1].lines[6] = "armor.";
			User_Interface.windows[255].textAreas[1].lines[7] = "";
			User_Interface.windows[255].textAreas[1].lines[8] = "";
			break;
		case 2:
			global::Rendering.Rendering.vehicleSelectMatrix = Matrix.CreateScale(13f) * Matrix.CreateRotationY(-0.2268928f) * Matrix.CreateRotationX((float)Math.PI * -4f / 45f) * Matrix.CreateTranslation(0f, 32f, 8f);
			User_Interface.windows[255].textAreas[1].lines[0] = "Speed:";
			User_Interface.windows[255].textAreas[2].lines[0] = "Fast (175/200)";
			User_Interface.windows[255].textAreas[1].lines[1] = "Armor:";
			User_Interface.windows[255].textAreas[2].lines[1] = "Weak (80/200)";
			User_Interface.windows[255].textAreas[1].lines[2] = "Speed Boost:";
			User_Interface.windows[255].textAreas[2].lines[2] = "Best";
			User_Interface.windows[255].textAreas[1].lines[3] = "";
			User_Interface.windows[255].textAreas[1].lines[4] = "A quick and agile aircraft.  Effective against both ground";
			User_Interface.windows[255].textAreas[1].lines[5] = "and air targets. It is best used for quick hit and run attacks";
			User_Interface.windows[255].textAreas[1].lines[6] = "against ground targets due to it's limited armor.";
			User_Interface.windows[255].textAreas[1].lines[7] = "";
			User_Interface.windows[255].textAreas[1].lines[8] = "";
			break;
		case 3:
			global::Rendering.Rendering.vehicleSelectMatrix = Matrix.CreateScale(13.7f) * Matrix.CreateRotationY(-0.2268928f) * Matrix.CreateRotationX((float)Math.PI * -4f / 45f) * Matrix.CreateTranslation(0f, -4f, -56f);
			User_Interface.windows[255].textAreas[1].lines[0] = "Speed:";
			User_Interface.windows[255].textAreas[2].lines[0] = "Below Average (130/200)";
			User_Interface.windows[255].textAreas[1].lines[1] = "Armor:";
			User_Interface.windows[255].textAreas[2].lines[1] = "Weak (80/200)";
			User_Interface.windows[255].textAreas[1].lines[2] = "Speed Boost:";
			User_Interface.windows[255].textAreas[2].lines[2] = "Constant";
			User_Interface.windows[255].textAreas[1].lines[3] = "";
			User_Interface.windows[255].textAreas[1].lines[4] = "The helicopter is useful mainly against ground targets.  It";
			User_Interface.windows[255].textAreas[1].lines[5] = "is extremely effective at capturing or repairing structures due";
			User_Interface.windows[255].textAreas[1].lines[6] = "to its ability to hover.  The helicopter can also fly at full";
			User_Interface.windows[255].textAreas[1].lines[7] = "speed without overheating. In combat, planes can easily avoid";
			User_Interface.windows[255].textAreas[1].lines[8] = "its targeting by climbing and diving rapidly.";
			break;
		case 4:
			global::Rendering.Rendering.vehicleSelectMatrix = Matrix.CreateScale(57f) * Matrix.CreateTranslation(0f, 0f, -120f);
			User_Interface.windows[255].textAreas[1].lines[0] = "Speed:";
			User_Interface.windows[255].textAreas[2].lines[0] = "Below Average (130/200)";
			User_Interface.windows[255].textAreas[1].lines[1] = "Armor:";
			User_Interface.windows[255].textAreas[2].lines[1] = "Above Average (175/200)";
			User_Interface.windows[255].textAreas[1].lines[2] = "Speed Boost:";
			User_Interface.windows[255].textAreas[2].lines[2] = "Average";
			User_Interface.windows[255].textAreas[1].lines[3] = "";
			User_Interface.windows[255].textAreas[1].lines[4] = "A very easy to fly aircraft which is effective against both";
			User_Interface.windows[255].textAreas[1].lines[5] = "ground and air targets. A good starter aircraft which is";
			User_Interface.windows[255].textAreas[1].lines[6] = "moderately armored and has decent speed.";
			User_Interface.windows[255].textAreas[1].lines[7] = "";
			User_Interface.windows[255].textAreas[1].lines[8] = "";
			break;
		case 5:
			global::Rendering.Rendering.vehicleSelectMatrix = Matrix.CreateScale(16f) * Matrix.CreateRotationY(-0.2268928f) * Matrix.CreateRotationX((float)Math.PI * -4f / 45f) * Matrix.CreateTranslation(0f, 16f, 0f);
			User_Interface.windows[255].textAreas[1].lines[0] = "Speed:";
			User_Interface.windows[255].textAreas[2].lines[0] = "Fast (170/200)";
			User_Interface.windows[255].textAreas[1].lines[1] = "Armor:";
			User_Interface.windows[255].textAreas[2].lines[1] = "Below Average (100/200)";
			User_Interface.windows[255].textAreas[1].lines[2] = "Speed Boost:";
			User_Interface.windows[255].textAreas[2].lines[2] = "Below Average";
			User_Interface.windows[255].textAreas[1].lines[3] = "";
			User_Interface.windows[255].textAreas[1].lines[4] = "A fast aircraft which is highly effective against air targets.";
			User_Interface.windows[255].textAreas[1].lines[5] = "The jet sacrifices missiles and bombs to accomodate a quad";
			User_Interface.windows[255].textAreas[1].lines[6] = " barrel machine gun system. It is a very agile aircraft and";
			User_Interface.windows[255].textAreas[1].lines[7] = " not suited for inexperienced pilots.";
			User_Interface.windows[255].textAreas[1].lines[8] = "";
			break;
		case 6:
			global::Rendering.Rendering.vehicleSelectMatrix = Matrix.CreateScale(11f) * Matrix.CreateRotationY(-0.2268928f) * Matrix.CreateRotationX((float)Math.PI * -4f / 45f) * Matrix.CreateTranslation(0f, 8f, 0f);
			User_Interface.windows[255].textAreas[1].lines[0] = "Speed:";
			User_Interface.windows[255].textAreas[2].lines[0] = "Fastest (200/200)";
			User_Interface.windows[255].textAreas[1].lines[1] = "Armor:";
			User_Interface.windows[255].textAreas[2].lines[1] = "Poor (70/200)";
			User_Interface.windows[255].textAreas[1].lines[2] = "Speed Boost:";
			User_Interface.windows[255].textAreas[2].lines[2] = "Average";
			User_Interface.windows[255].textAreas[1].lines[3] = "";
			User_Interface.windows[255].textAreas[1].lines[4] = "A fast aircraft which is highly effective against ground targets";
			User_Interface.windows[255].textAreas[1].lines[5] = "due to it's tri-laser system. It is not great against moving";
			User_Interface.windows[255].textAreas[1].lines[6] = "targets due to the slower moving laser shots which must be";
			User_Interface.windows[255].textAreas[1].lines[7] = "aimed ahead of the intended target.";
			User_Interface.windows[255].textAreas[1].lines[8] = "";
			break;
		default:
			global::Rendering.Rendering.vehicleSelectMatrix = Matrix.CreateScale(11f) * Matrix.CreateRotationY(-0.2268928f) * Matrix.CreateRotationX((float)Math.PI * -4f / 45f);
			break;
		}
		if (global::Players.Players.currentPlayerRank < Vehicles.lockedVehicleLevels[User_Interface.curVehicleSelect])
		{
			User_Interface.windows[255].textAreas[3].lines[0] = "Locked Until Rank " + Vehicles.lockedVehicleLevels[User_Interface.curVehicleSelect];
			User_Interface.windows[255].textAreas[3].lines[1] = "Current Rank:  " + global::Players.Players.currentPlayerRank;
			User_Interface.windows[255].textAreas[3].status = 1;
		}
		else
		{
			User_Interface.windows[255].textAreas[3].status = 0;
		}
	}

	public void Game_UI_Update_Weapon_Select_Set()
	{
	}

	public void Game_UI_Update_Mission_Objectives(byte missionObjectiveWindowID)
	{
		mainC.userInterface.Set_Component_Status(1, missionObjectiveWindowID, 0, 1);
		mainC.userInterface.Set_Component_Status(1, missionObjectiveWindowID, 1, 0);
		User_Interface.windows[22].state = 0;
		switch (MainGame.gameLevel)
		{
		case 0:
			User_Interface.windows[missionObjectiveWindowID].textAreas[0].numLines = 3;
			User_Interface.windows[missionObjectiveWindowID].textAreas[0].lines = new string[3];
			User_Interface.windows[missionObjectiveWindowID].textAreas[0].lines[0] = "The elevators are not working and Jake is attempting to";
			User_Interface.windows[missionObjectiveWindowID].textAreas[0].lines[1] = "override the shutdown.  Protect him while he works to";
			User_Interface.windows[missionObjectiveWindowID].textAreas[0].lines[2] = "get the systems back online.";
			break;
		case 1:
			User_Interface.windows[missionObjectiveWindowID].textAreas[0].numLines = 3;
			User_Interface.windows[missionObjectiveWindowID].textAreas[0].lines = new string[3];
			User_Interface.windows[missionObjectiveWindowID].textAreas[0].lines[0] = "Jake is close to having the elevators working again.";
			User_Interface.windows[missionObjectiveWindowID].textAreas[0].lines[1] = "Continue to protect him while he works.  Once the";
			User_Interface.windows[missionObjectiveWindowID].textAreas[0].lines[2] = "elevators are working, he will be able to find supplies.";
			break;
		case 2:
			User_Interface.windows[missionObjectiveWindowID].textAreas[0].numLines = 4;
			User_Interface.windows[missionObjectiveWindowID].textAreas[0].lines = new string[4];
			User_Interface.windows[missionObjectiveWindowID].textAreas[0].lines[0] = "Jake has managed to get the elevators workings despite";
			User_Interface.windows[missionObjectiveWindowID].textAreas[0].lines[1] = "the safety shutdown.  He has gone to the basement to";
			User_Interface.windows[missionObjectiveWindowID].textAreas[0].lines[2] = "look for weapons and supplies.  Keep the zombies";
			User_Interface.windows[missionObjectiveWindowID].textAreas[0].lines[3] = "occupied while he is gone.";
			break;
		case 4:
		case 6:
		case 8:
		case 10:
		case 12:
		case 14:
			User_Interface.windows[missionObjectiveWindowID].textAreas[0].numLines = 2;
			User_Interface.windows[missionObjectiveWindowID].textAreas[0].lines = new string[2];
			User_Interface.windows[missionObjectiveWindowID].textAreas[0].lines[0] = "Jake has gone to the basement looking for weapons.";
			User_Interface.windows[missionObjectiveWindowID].textAreas[0].lines[1] = "Hold out until he returns.";
			break;
		case 3:
		case 5:
		case 7:
		case 9:
		case 11:
		case 13:
		case 15:
			User_Interface.windows[missionObjectiveWindowID].textAreas[0].numLines = 3;
			User_Interface.windows[missionObjectiveWindowID].textAreas[0].lines = new string[3];
			User_Interface.windows[missionObjectiveWindowID].textAreas[0].lines[0] = "Jake is breaking into a weapons crate. Protect him while";
			User_Interface.windows[missionObjectiveWindowID].textAreas[0].lines[1] = "he works. You will have access to new weapons once he";
			User_Interface.windows[missionObjectiveWindowID].textAreas[0].lines[2] = "is finished.";
			break;
		case 16:
			User_Interface.windows[missionObjectiveWindowID].textAreas[0].numLines = 2;
			User_Interface.windows[missionObjectiveWindowID].textAreas[0].lines = new string[2];
			User_Interface.windows[missionObjectiveWindowID].textAreas[0].lines[0] = "Jake has gone to the garage to find transportation.";
			User_Interface.windows[missionObjectiveWindowID].textAreas[0].lines[1] = "Hold out until he returns.";
			break;
		case 17:
			User_Interface.windows[missionObjectiveWindowID].textAreas[0].numLines = 2;
			User_Interface.windows[missionObjectiveWindowID].textAreas[0].lines = new string[2];
			User_Interface.windows[missionObjectiveWindowID].textAreas[0].lines[0] = "Jake has found a van in the garage and is attempting";
			User_Interface.windows[missionObjectiveWindowID].textAreas[0].lines[1] = "to hotwire it. Hold out while he secures a way out.";
			break;
		case 18:
			User_Interface.windows[missionObjectiveWindowID].textAreas[0].numLines = 3;
			User_Interface.windows[missionObjectiveWindowID].textAreas[0].lines = new string[3];
			User_Interface.windows[missionObjectiveWindowID].textAreas[0].lines[0] = "Jake is working to hack the garage security system.";
			User_Interface.windows[missionObjectiveWindowID].textAreas[0].lines[1] = "Keep the zombies occupied so he can work on opening";
			User_Interface.windows[missionObjectiveWindowID].textAreas[0].lines[2] = "the main door.";
			break;
		case 19:
			User_Interface.windows[missionObjectiveWindowID].textAreas[0].numLines = 3;
			User_Interface.windows[missionObjectiveWindowID].textAreas[0].lines = new string[3];
			User_Interface.windows[missionObjectiveWindowID].textAreas[0].lines[0] = "Jake is loading the survivors in the van and will";
			User_Interface.windows[missionObjectiveWindowID].textAreas[0].lines[1] = "be around front soon. Stay alive until he shows up";
			User_Interface.windows[missionObjectiveWindowID].textAreas[0].lines[2] = "with the van.";
			break;
		}
	}

	public void Game_UI_Update_In_Game_Window(ushort windowID)
	{
		if (MainGame.trialMode)
		{
			mainC.userInterface.Swap_Text_Buttons(windowID, 6, 4);
		}
		else
		{
			mainC.userInterface.Swap_Text_Buttons(windowID, 4, 6);
		}
		switch (MainGame.gameMode)
		{
		case 0:
			mainC.userInterface.Show_TextButton(windowID, 2);
			break;
		case 1:
			mainC.userInterface.Hide_TextButton(windowID, 2);
			break;
		}
	}

	public void Game_UI_Render_Weapon_Select(float frameTime)
	{
	}

	public void Game_Rank_Players(byte sortMode)
	{
		bool flag;
		byte b;
		ushort num;
		if (sortMode == 0)
		{
			num = 0;
			team0PlayerCount = 0;
			while (num < MainGame.maxHumanGamePlayers)
			{
				if (global::Players.Players.players[num].active)
				{
					team0Players[team0PlayerCount++] = (byte)num;
				}
				num++;
			}
			flag = true;
			b = 0;
			while (flag && b++ < 250)
			{
				flag = false;
				for (num = 0; num < team0PlayerCount - 1; num++)
				{
					if (MainGame.gameData.players[team0Players[num]].scoresI[0] < MainGame.gameData.players[team0Players[num + 1]].scoresI[0])
					{
						byte b2 = team0Players[num];
						team0Players[num] = team0Players[num + 1];
						team0Players[num + 1] = b2;
						flag = true;
					}
				}
			}
			return;
		}
		num = 0;
		team0PlayerCount = 0;
		while (num < MainGame.maxHumanGamePlayers)
		{
			if (global::Players.Players.players[num].active && global::Players.Players.players[num].team == 0)
			{
				team0Players[team0PlayerCount++] = (byte)num;
			}
			num++;
		}
		num = 0;
		team1PlayerCount = 0;
		while (num < MainGame.maxHumanGamePlayers)
		{
			if (global::Players.Players.players[num].active && global::Players.Players.players[num].team == 1)
			{
				team1Players[team1PlayerCount++] = (byte)num;
			}
			num++;
		}
		flag = true;
		b = 0;
		while (flag && b++ < 250)
		{
			flag = false;
			for (num = 0; num < team0PlayerCount - 1; num++)
			{
				if (MainGame.gameData.players[team0Players[num]].scoresI[0] < MainGame.gameData.players[team0Players[num + 1]].scoresI[0])
				{
					byte b2 = team0Players[num];
					team0Players[num] = team0Players[num + 1];
					team0Players[num + 1] = b2;
					flag = true;
				}
			}
		}
		flag = true;
		b = 0;
		while (flag && b++ < 250)
		{
			flag = false;
			for (num = 0; num < team1PlayerCount - 1; num++)
			{
				if (MainGame.gameData.players[team1Players[num]].scoresI[0] < MainGame.gameData.players[team1Players[num + 1]].scoresI[0])
				{
					byte b2 = team1Players[num];
					team1Players[num] = team1Players[num + 1];
					team1Players[num + 1] = b2;
					flag = true;
				}
			}
		}
	}

	public byte Game_Map_Program_To_Avatar_Animation(ushort programID)
	{
		return 0;
	}

	public void Game_Reset_Textures()
	{
		texHudZombies = (ushort)mainC.texturesMain.Find_Texture("HUD_Zombies", 0);
		texHudLife = (ushort)mainC.texturesMain.Find_Texture("HUD_Life", 0);
		texHudHealthBar = (ushort)mainC.texturesMain.Find_Texture("HUD_HealthBar", 0);
		texHudHealthIcon = (ushort)mainC.texturesMain.Find_Texture("HUD_Health_Icon", 0);
		texHudGrenade = (ushort)mainC.texturesMain.Find_Texture("HUD_Grenade", 0);
		texHudAmmoClip = (ushort)mainC.texturesMain.Find_Texture("HUD_Ammo_Clip", 0);
		texHudAmmoShell = (ushort)mainC.texturesMain.Find_Texture("HUD_Ammo_Shell", 0);
		texHudAmmoBullet = (ushort)mainC.texturesMain.Find_Texture("HUD_Ammo_Bullet", 0);
		texHudWaypoint = (ushort)mainC.texturesMain.Find_Texture("HUD_Waypoint", 0);
	}

	public void Game_Airplane_Crashed()
	{
		Game_Misc(8);
	}

	public void Game_End_MP_Round_Prematurely()
	{
	}

	public void Game_End_SP_Round()
	{
	}

	public void Game_Misc(byte action)
	{
	}

	public void Game_Misc_Threaded(byte action)
	{
		switch (action)
		{
		case 0:
		{
			float num = global::Players.Players.playerRaces[MainGame.miscFunctionByte1].spawnHeight[global::Players.Players.players[0].type] - (global::Players.Players.playerRaces[global::Players.Players.players[0].race].spawnHeight[global::Players.Players.players[0].type] - global::Players.Players.playerRaces[global::Players.Players.players[0].race].centerPoint[global::Players.Players.players[0].type]);
			float num2 = (float)Math.Asin(MainGame.playerVehicles[0].mv[global::Rendering.Rendering.uBufferID].M23);
			if (mainC.playersMain.Set_Player_Race(0, MainGame.miscFunctionByte1, global::Players.Players.players[0].type))
			{
				global::Players.Players.playerRot *= Quaternion.CreateFromYawPitchRoll(0f, 0f - num2, 0f);
				Game_Reset_Joints_And_Programs();
				global::Players.Players.players[0].charP.position.v[2] += num;
				MainGame.playerVehicles[0].ph1.z += num;
			}
			global::Players.Players.playerRot = Quaternion.CreateFromYawPitchRoll(0f, Vehicles.vehicles[global::Players.Players.players[0].curVehicle].data8 * ((float)Math.PI / 180f), 0f);
			Matrix.CreateFromQuaternion(ref global::Players.Players.playerRot, out MainGame.playerVehicles[0].mv[global::Rendering.Rendering.uBufferID]);
			break;
		}
		case 1:
		{
			ref Matrix reference = ref global::Players.Players.players[0].mv[global::Rendering.Rendering.uBufferID];
			reference = Matrix.Identity;
			global::Joints.Joints.Reset_Joint_Rotations_To_Zero(0);
			global::Joints.Joints.Reset_Joint_Data(0);
			global::Joints.Joints.Moved_Joint_Calculations(0, global::Players.Players.players[0].numJoints);
			global::Joints.Joints.Sync_Player_Matrices(0, global::Rendering.Rendering.uBufferID, global::Rendering.Rendering.rBufferID);
			mainC.renderingMain.Load_Rendering_Data("Menu_Rendering_Data.txt");
			break;
		}
		case 2:
			break;
		}
	}

	public void Game_Reset_Joints_And_Programs()
	{
		global::Joints.Joints.Reset_Joint_Rotations_To_Zero(0);
		mainC.jointsMain.Update_Joints_For_New_Position(0);
	}

	public void Game_Reset_Scores()
	{
		for (ushort num = 0; num < 44; num++)
		{
			MainGame.gameData.players[num].scoresF[0] = 0f;
			MainGame.gameData.players[num].scoresI[0] = 0;
			MainGame.gameData.players[num].numKills = 0;
			MainGame.gameData.players[num].shotsHit = 0;
			MainGame.gameData.players[num].shotsFired = 0;
			MainGame.gameData.players[num].numDeaths = 0;
			MainGame.gameData.players[num].teamKills = 0;
			MainGame.gameData.players[num].selfKills = 0;
			MainGame.lastGameData.players[num].scoresF[0] = 0f;
			MainGame.lastGameData.players[num].scoresI[0] = 0;
			MainGame.lastGameData.players[num].numKills = 0;
			MainGame.lastGameData.players[num].shotsHit = 0;
			MainGame.lastGameData.players[num].shotsFired = 0;
			MainGame.lastGameData.players[num].numDeaths = 0;
			MainGame.lastGameData.players[num].teamKills = 0;
			MainGame.lastGameData.players[num].selfKills = 0;
		}
		if (MainGame.gameMode == 0)
		{
			MainGame.gameData.players[0].numKills += global::AI.AI.levelKillCount;
		}
	}

	public void Game_Reset_Player_Score(ushort playerID)
	{
		MainGame.gameData.players[playerID].scoresF[0] = 0f;
		MainGame.gameData.players[playerID].scoresI[0] = 0;
		MainGame.gameData.players[playerID].numKills = 0;
		MainGame.gameData.players[playerID].shotsHit = 0;
		MainGame.gameData.players[playerID].shotsFired = 0;
		MainGame.gameData.players[playerID].numDeaths = 0;
		MainGame.gameData.players[playerID].teamKills = 0;
		MainGame.gameData.players[playerID].selfKills = 0;
		MainGame.lastGameData.players[playerID].scoresF[0] = 0f;
		MainGame.lastGameData.players[playerID].scoresI[0] = 0;
		MainGame.lastGameData.players[playerID].numKills = 0;
		MainGame.lastGameData.players[playerID].shotsHit = 0;
		MainGame.lastGameData.players[playerID].shotsFired = 0;
		MainGame.lastGameData.players[playerID].numDeaths = 0;
		MainGame.lastGameData.players[playerID].teamKills = 0;
		MainGame.lastGameData.players[playerID].selfKills = 0;
	}

	public void Game_Avatar_Animation_Finished(ushort playerID, byte animationID)
	{
	}

	public void Game_Avatar_Animation_Canceled(ushort playerID, byte animationID)
	{
	}

	public void Game_New_Player_Ready(NetworkGamer newGamer)
	{
	}

	public void Game_Received_Player_Weapon_Update(ushort playerID, byte stubID)
	{
		if (stubID == MainGame.playerVehicles[playerID].mounts[MainGame.primaryWeaponMount].objectID)
		{
			Game_Vehicle_Primary_Mount_Weapon_Changed(playerID);
		}
	}

	public void Game_JoinGame_Failed()
	{
		joinFailed = true;
	}

	public void Game_JoinFirstGame_Failed()
	{
		joinFailed = true;
	}

	public void Game_Send_Final_Player_Stats()
	{
		global::Networking.Networking.numUshortsToSend = 7;
		global::Networking.Networking.networkUShorts[0] = MainGame.gameData.players[0].numKills;
		global::Networking.Networking.networkUShorts[1] = MainGame.gameData.players[0].shotsHit;
		global::Networking.Networking.networkUShorts[2] = MainGame.gameData.players[0].shotsFired;
		global::Networking.Networking.networkUShorts[3] = MainGame.gameData.players[0].numDeaths;
		global::Networking.Networking.networkUShorts[4] = MainGame.gameData.players[0].teamKills;
		global::Networking.Networking.networkUShorts[5] = MainGame.gameData.players[0].selfKills;
		global::Networking.Networking.networkUShorts[6] = global::Players.Players.maxKillStreak[0];
		mainC.networkingMain.XBOX_Send_Network_Message7(7);
	}

	public void Game_Receive_Final_Player_Stats(int actID)
	{
		short num = mainC.playersMain.Get_Player_Index(actID, -1);
		if (num >= 0)
		{
			MainGame.gameData.players[num].numKills = global::Networking.Networking.networkUShorts[0];
			MainGame.gameData.players[num].shotsHit = global::Networking.Networking.networkUShorts[1];
			MainGame.gameData.players[num].shotsFired = global::Networking.Networking.networkUShorts[2];
			MainGame.gameData.players[num].numDeaths = global::Networking.Networking.networkUShorts[3];
			MainGame.gameData.players[num].teamKills = global::Networking.Networking.networkUShorts[4];
			MainGame.gameData.players[num].selfKills = global::Networking.Networking.networkUShorts[5];
			global::Players.Players.maxKillStreak[num] = global::Networking.Networking.networkUShorts[6];
		}
	}

	public void Game_Send_GameSettings(byte whatToSend)
	{
		if ((whatToSend & 1) > 0)
		{
			global::Networking.Networking.networkBytes[0] = 0;
			global::Networking.Networking.networkBytes[1] = MainGame.gameType;
			global::Networking.Networking.networkBytes[2] = MainGame.gameLevel;
			global::Networking.Networking.networkBytes[3] = MainGame.mp_numPlayers_index;
			global::Networking.Networking.networkBytes[4] = MainGame.mp_timeLimit_index;
			global::Networking.Networking.networkBytes[5] = MainGame.difficulty;
			global::Networking.Networking.networkBytes[6] = MainGame.numTeams;
			ref HalfSingle reference = ref global::Networking.Networking.networkHS[0];
			reference = new HalfSingle(MainGame.roundCurrentTime);
			mainC.networkingMain.XBOX_Send_Network_Message15(15);
		}
		if ((whatToSend & 4) > 0)
		{
			global::Networking.Networking.networkBytes[0] = 1;
			global::Networking.Networking.networkBytes[1] = 1;
			global::Networking.Networking.networkBytes[2] = 1;
			mainC.networkingMain.XBOX_Send_Network_Message15(15);
		}
	}

	public void Game_Update_GameSettings_From_Main_NetworkSession()
	{
		MainGame.mp_numPlayers_index = (byte)global::Networking.Networking.networkSession.SessionProperties[1].Value;
		MainGame.mp_timeLimit_index = (byte)global::Networking.Networking.networkSession.SessionProperties[2].Value;
		MainGame.gameLevel = (byte)global::Networking.Networking.networkSession.SessionProperties[3].Value;
		MainGame.gameType = (byte)global::Networking.Networking.networkSession.SessionProperties[4].Value;
		MainGame.difficulty = (byte)global::Networking.Networking.networkSession.SessionProperties[6].Value;
		MainGame.numTeams = (byte)global::Networking.Networking.networkSession.SessionProperties[7].Value;
		Game_MP_Game_Settings_Changed();
	}

	public void Game_Send_GameSettings_To_NewPlayer(byte whatToSend, NetworkGamer newGamer)
	{
		if ((whatToSend & 1) > 0)
		{
			global::Networking.Networking.networkBytes[0] = 0;
			global::Networking.Networking.networkBytes[1] = MainGame.gameType;
			global::Networking.Networking.networkBytes[2] = MainGame.gameLevel;
			global::Networking.Networking.networkBytes[3] = MainGame.mp_numPlayers_index;
			global::Networking.Networking.networkBytes[4] = MainGame.mp_timeLimit_index;
			global::Networking.Networking.networkBytes[5] = MainGame.difficulty;
			global::Networking.Networking.networkBytes[6] = MainGame.numTeams;
			ref HalfSingle reference = ref global::Networking.Networking.networkHS[0];
			reference = new HalfSingle(MainGame.roundCurrentTime);
			mainC.networkingMain.XBOX_Send_Network_Message_To_Gamer(15, newGamer);
		}
	}

	public void Game_Receive_Update_Of_GameSettings()
	{
		switch (global::Networking.Networking.networkBytes[0])
		{
		case 0:
			MainGame.gameType = global::Networking.Networking.networkBytes[1];
			MainGame.gameLevel = global::Networking.Networking.networkBytes[2];
			MainGame.mp_numPlayers_index = global::Networking.Networking.networkBytes[3];
			MainGame.mp_timeLimit_index = global::Networking.Networking.networkBytes[4];
			MainGame.difficulty = global::Networking.Networking.networkBytes[5];
			MainGame.numTeams = global::Networking.Networking.networkBytes[6];
			MainGame.roundCurrentTime = global::Networking.Networking.networkHS[0].ToSingle();
			Game_MP_Game_Settings_Changed();
			break;
		case 3:
			MainGame.roundCurrentTime = global::Networking.Networking.networkHS[0].ToSingle();
			break;
		}
	}

	public void Game_MP_Game_Settings_Changed()
	{
		MainGame.maxGamePlayers = MainGame.mp_numPlayers[MainGame.mp_numPlayers_index];
		if (MainGame.maxGamePlayers > 44)
		{
			MainGame.maxGamePlayers = 44;
		}
		MainGame.maxHumanGamePlayers = MainGame.mp_numRemotePlayers[MainGame.mp_numPlayers_index];
		if (MainGame.maxHumanGamePlayers > 4)
		{
			MainGame.maxHumanGamePlayers = 4;
		}
		byte gameType = MainGame.gameType;
		if (gameType == 2)
		{
			MainGame.linearProgression = true;
			MainGame.numTeams = 2;
			if (MainGame.gameLevel >= 19)
			{
				MainGame.gameLevel = 18;
			}
		}
		else
		{
			MainGame.linearProgression = true;
			MainGame.numTeams = 5;
			if (MainGame.gameLevel >= 4)
			{
				MainGame.gameLevel = 0;
			}
		}
		MainGame.allowTeamKills = false;
		mainC.maingameMain.Update_GameScore_Main_Data();
	}

	public void Game_Send_Player_Status(NetworkGamer sender)
	{
		global::Networking.Networking.networkUShorts[0] = mainC.playersMain.Make_Player_Status_Bytes_FPS();
		mainC.networkingMain.XBOX_Send_Network_Message_To_Gamer(43, sender);
	}

	public void Game_Remove_Player_From_Game(ushort playerID)
	{
		if (!global::Networking.Networking.isHost)
		{
			return;
		}
		float num = 1f;
		if (global::Networking.Networking.networkSession.RemoteGamers.Count > 0)
		{
			num = 1f + ((float)global::Networking.Networking.networkSession.RemoteGamers.Count + 1f) / 4f * 2f;
			if (num > 3f)
			{
				num = 3f;
			}
		}
		mainC.pickupsMain.Update_Time_Modifier(num);
		float num2 = global::Players.Players.players[playerID].charP.position.v[0];
		float num3 = global::Players.Players.players[playerID].charP.position.v[1];
		float z = global::Players.Players.players[playerID].charP.position.v[2];
		float num4 = 360f / (float)(int)MainGame.playerVehicles[playerID].numWeapons;
		float num5 = 0f;
		for (ushort num6 = 0; num6 < MainGame.playerVehicles[playerID].numWeapons; num6++)
		{
			float num7 = (float)Math.Sin(num5) * 2.5f * 1f;
			float num8 = (float)Math.Cos(num5) * 2.5f * 1f;
			if (MainGame.playerVehicles[playerID].weapons[num6].active && MainGame.playerVehicles[playerID].weapons[num6].weaponID != 6 && MainGame.playerVehicles[playerID].weapons[num6].weaponID != 1)
			{
				mainC.pickupsMain.Player_Drops_Weapon(MainGame.playerVehicles[playerID].weapons[num6].weaponID, num2 + num7, num3 + num8, z, sendToNetwork: true);
			}
			num5 += num4;
		}
	}

	public void Game_Render_Last()
	{
		if (MainGame.gameMode == 0)
		{
			return;
		}
		mainC.renderingMain.Render_Damage_Bar_3D_Setup();
		global::Rendering.Rendering.effect1.Parameters["depth"].SetValue(0f);
		global::Rendering.Rendering.effect1.Parameters["BaseTexture"].SetValue(global::Textures.Textures.texMain.texData[global::Textures.Textures.texWhite]);
		for (ushort num = 1; num < MainGame.maxHumanGamePlayers; num++)
		{
			if (global::Players.Players.players[num].onmap == 4)
			{
				mainC.renderingMain.Render_Damage_Bar_3D(global::Players.Players.players[num].damagePercentageCapped, global::Players.Players.players[num].posX[global::Rendering.Rendering.rBufferID], global::Players.Players.players[num].posY[global::Rendering.Rendering.rBufferID], global::Players.Players.players[num].posZ[global::Rendering.Rendering.rBufferID] + 6f, 33, 8, 10);
			}
		}
		mainC.renderingMain.Render_Damage_Bar_3D_Cleanup();
	}

	public float Game_Adjust_AI_KillCount_Scale(float currentKillCountScale)
	{
		return currentKillCountScale;
	}
}
