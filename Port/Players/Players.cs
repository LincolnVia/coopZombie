using System;
using System.Globalization;
using System.IO;
using AI;
using Collision;
using GameObjects;
using InputHandler;
using Joints;
using MainGame;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using Microsoft.Xna.Framework.Net;
using Models;
using Networking;
using Physics;
using Pickups;
using Programs;
using Rendering;
using Structs;
using Textures;
using Util;
using Weapons;
using WindowsGame1;

namespace Players;

public class Players
{
	private float px1;

	private float py1;

	private float pz1;

	private float px2;

	private float py2;

	private float pz2;

	private float nx1;

	private float ny1;

	private float nz1;

	private float onGroundTimer;

	public static int collisionObjectID;

	public static byte gearSound = byte.MaxValue;

	public static Quaternion playerRot = Quaternion.Identity;

	public static Quaternion playerWing1Rot = Quaternion.Identity;

	public static Quaternion playerWing2Rot = Quaternion.Identity;

	public static Quaternion rot;

	public static bool needSpawn = true;

	public static bool playerViewingDevice = false;

	public static bool controlsInUse = false;

	public static bool targetingEnemy = false;

	public static bool respawnEnabled = true;

	public static bool throwingGrenade = false;

	public static bool invincible = false;

	public static bool resetView = false;

	public static bool needToReload = false;

	public static bool needToChamber = false;

	public static bool fullyAutomatic = true;

	public static bool haveActID = false;

	public static bool freezeCamera = false;

	public static bool jumping;

	public static bool pushingOff;

	public static bool crouching;

	public static bool incover;

	public static bool changingWeapons;

	public static bool reloading;

	public static bool chambering;

	public static byte currentPlayerRank;

	public static byte playerRankMax = 50;

	public static byte playerRankSP = 0;

	public static byte playerRankMP = 0;

	public static byte controllerScheme = 0;

	public static byte numPlayerRaces = 0;

	public static byte numAllocatedPlayerRaces = 0;

	public static byte commanderView = 0;

	public static byte nextRemoteGamer;

	public static byte scopeValue = 0;

	public static byte playerViewJoint1;

	public static byte lastView = 0;

	public static byte currentView = 1;

	public static byte commanderViewingPlayer;

	public static byte numGuards = 0;

	public static byte numPrisoners = 0;

	public static byte raceWithMostJoints = 0;

	public static byte raceTypeWithMostJoints = 0;

	public static byte eyeJoint;

	public static byte humanoidBackJoint;

	public static byte shoulderJointL;

	public static byte shoulderJointR;

	public static byte headJoint;

	public static byte[] remotePlayerRanks = new byte[4];

	public static sbyte[] scoreBoardValues = new sbyte[16];

	public static string playerkMsg;

	public static ushort lastMPVehicle = 0;

	public static ushort lastSPVehicle = 0;

	public static ushort lastMPWeapon = 0;

	public static ushort lastMPWeapon2 = 0;

	public static ushort lastSPWeapon = 0;

	public static ushort lastSPWeapon2 = 0;

	public static ushort maxNumPlayerRaceJoints = 0;

	public static ushort[] numRankedPlayers = new ushort[2];

	public static ushort[] killStreak = new ushort[44];

	public static ushort[] maxKillStreak = new ushort[44];

	public static ushort[] texTauntIcons;

	public static ushort[,] playerRankings = new ushort[4, 2];

	public static short numAllocLvlSortArray = 2;

	public static int[] playerConnectors;

	public static int[] remotePlayerPositions = new int[4];

	public static int[] teamPoints = new int[5];

	public static int[] rankUpPointLevels = new int[50];

	public static int moving = 0;

	public static int camo1;

	public static int camo2;

	public static int camo3;

	public static int enemyTeamTexture;

	public static int playerTeamTexture;

	public static int frameCnt1;

	public static int playerRankPointsSP = 0;

	public static int playerRankPointsMP = 0;

	public static ulong enemyTeamMask;

	public static float playerRankingsTimer;

	public static float spawningInvincibleTime = 0f;

	public static float groundCheckDistance;

	public static float walkSpeedSquared = 400f;

	public static float stickSpringVelY = 0f;

	public static float stickSpringVelX = 0f;

	public static float stickSpringAccelY = 0f;

	public static float stickSpringAccelX = 0f;

	public static float previousStickValueX;

	public static float previousStickValueY;

	public static float stickTimerX;

	public static float stickTimerY;

	public static float outOfBoundsTimer;

	public static float stepOver = 0f;

	public static float outOfBoundsTimerRandom;

	public static float mainPlayerDeathTimer;

	public static float arcadeModePreviousSetting = 0f;

	public static float collisionWithGround = 0f;

	public static float cameraRotationZ = 0f;

	public static float thirdPersonViewAdjustFactor;

	public static float fallingTimer;

	public static float footStepTimer;

	public static float xRotMovement;

	public static float runTime = 0f;

	public static float adjustmentAngleX;

	public static float adjustmentAngleZ;

	public static float scopeViewAdj;

	public static float viewAdjX;

	public static float viewAdjY;

	public static float viewAdjZ;

	public static float xRotation;

	public static float zRotationDesired;

	public static float yRotation;

	public static float zRotation = 0f;

	public static float playerSpeed = 0f;

	public static float playerSpeedSideways = 0f;

	public static float playerSpeedRotateRightStick = 0f;

	public static float playerSpeedRotateLeftStick = 0f;

	public static float playerSpeedElevateRightStick = 0f;

	public static float invertY = 1f;

	public static float invertYSecondary = 1f;

	public static float commanderX;

	public static float commanderY;

	public static float commanderZ;

	public static float jetPack = 0f;

	public static float respawnTimer;

	public static float[] dualWieldAdjX;

	public static float[] dualWieldAdjY;

	public static float[] dualWieldAdjZ;

	public static float[] remotePlayerPositionOffsetX = new float[4];

	public static float[] remotePlayerPositionOffsetY = new float[4];

	public static float[] thirdPersonViewDistanceSqr = new float[2];

	public static float[] lvlLightSortArray = new float[2];

	public static float thirdPersonXAdj = 10f;

	public static float thirdPersonYAdj = 90.5f;

	public static float thirdPersonZAdj = -7.5f;

	public static float scopeViewX = 0f;

	public static float scopeViewY = 0f;

	public static float scopeViewZ = 0f;

	public static float firstPersonViewX = 0f;

	public static float firstPersonViewY = 0f;

	public static float firstPersonViewZ = 0f;

	public static float firstPersonViewAdjX = 0f;

	public static float firstPersonViewAdjY = 0f;

	public static float firstPersonViewAdjZ = 0f;

	public static float ironSightsViewX;

	public static float ironSightsViewY;

	public static float ironSightsViewZ;

	public static float ironSightsViewAdjX = 0f;

	public static float ironSightsViewAdjY = 0f;

	public static float ironSightsViewAdjZ = 0f;

	public static float rotReturn;

	public static StructsClass.player[] players = new StructsClass.player[44];

	public static StructsClass.Player_Races[] playerRaces;

	public static StructsClass.particle_list[] rpoP1 = new StructsClass.particle_list[5];

	public static StructsClass.vtex ppV1 = new StructsClass.vtex();

	public static StructsClass.particle_list cpiP2 = default(StructsClass.particle_list);

	public static StructsClass.particle_list[] cpiP2T = new StructsClass.particle_list[5];

	public static ushort[] pluTeam = new ushort[5];

	public static byte[] spsC1 = new byte[1];

	public static byte[] spsC2 = new byte[1];

	public static StructsClass.vtex targetVec1 = new StructsClass.vtex();

	public static StructsClass.vtex targetVec2 = new StructsClass.vtex();

	public static StructsClass.vtex targetVec3 = new StructsClass.vtex();

	public static StructsClass.particle_list viewBox;

	public static StructsClass.particle_list p1AIHumanoid = default(StructsClass.particle_list);

	public static StructsClass.vtex[] phV3 = new StructsClass.vtex[5];

	public static Vector2 scorePostion = default(Vector2);

	public static StructsClass.player_preference[] playerPrefsSP;

	public static StructsClass.player_preference[] playerPrefsMP;

	public static StructsClass.Multiplayer_Data[] mpData = new StructsClass.Multiplayer_Data[4];

	public static Matrix[] playerViewMatrix = new Matrix[2];

	public static bool showBoundingBox = false;

	public Game1.MasterCollection mainC;

	public static Vector2 ipVec = default(Vector2);

	public void Init_Vars(Game1.MasterCollection master)
	{
		mainC = master;
		for (int i = 0; i < 5; i++)
		{
			cpiP2T[i] = default(StructsClass.particle_list);
			StructsClass.Initialize_ParticleList(ref cpiP2T[i]);
			rpoP1[i] = default(StructsClass.particle_list);
			phV3[i] = new StructsClass.vtex();
		}
		StructsClass.Initialize_ParticleList(ref cpiP2);
		for (int i = 0; i < 4; i++)
		{
			mpData[i] = new StructsClass.Multiplayer_Data();
		}
	}

	public void Init_Player_Preferences()
	{
		if (playerPrefsSP == null)
		{
			playerPrefsSP = new StructsClass.player_preference[1];
			for (ushort num = 0; num < 1; num++)
			{
				playerPrefsSP[num].numWeapons = 0;
			}
		}
		if (playerPrefsMP == null)
		{
			playerPrefsMP = new StructsClass.player_preference[1];
			for (ushort num = 0; num < 1; num++)
			{
				playerPrefsMP[num].numWeapons = 0;
			}
		}
	}

	public void Init_Players()
	{
		_ = global::Rendering.Rendering.uBufferID;
		eyeJoint = 0;
		humanoidBackJoint = 0;
		headJoint = 0;
		shoulderJointL = 0;
		shoulderJointR = 0;
		playerViewJoint1 = 0;
		viewBox = default(StructsClass.particle_list);
		StructsClass.Initialize_ParticleList(ref p1AIHumanoid);
		StructsClass.Initialize_ParticleList(ref viewBox);
		viewBox.v1 = new StructsClass.vtex[1];
		viewBox.v1[0] = new StructsClass.vtex();
		viewBox.v1[0].v[0] = 0f;
		viewBox.v1[0].v[1] = 0f;
		viewBox.v1[0].v[2] = 0f;
		viewBox.numP = 1L;
		viewBox.numUsed = 1L;
		Load_Player_Race_Data("Player_Races.txt");
		for (int i = 0; i < 44; i++)
		{
			players[i] = new StructsClass.player();
			Adjust_Player_Damage_To_Zero(0, sendOnline: false);
			players[i].username = "";
			players[i].abreviateName = "";
			players[i].renderScale = 1f;
			players[i].curVehicleIndex = 0;
			players[i].curVehicle = global::Util.Util.maxUnsignedShortValue;
			for (int j = 0; j < 3; j++)
			{
				players[i].vehicles[j] = global::Util.Util.maxUnsignedShortValue;
			}
			players[i].shootingAccuracy = 1f;
			players[i].aiID = byte.MaxValue;
			players[i].numAllocatedJoints = 0;
			players[i].numJoints = 0;
			players[i].numAmmoClips = 0;
			players[i].lastParticleCount = 0;
			players[i].torqueJoint = 0;
			players[i].humanoidBackJoint = 0;
			players[i].headJoint = 0;
			players[i].type = -1;
			players[i].race = byte.MaxValue;
			players[i].damageType = 1;
			players[i].shooting = false;
			players[i].shotOnce = 0;
			players[i].usingTracers = false;
			players[i].playerIsMoving = 0;
			players[i].programStationaryLegsBody = 13;
			players[i].programStationaryArms = 5;
			players[i].programWalk = 14;
			players[i].programJump = 9;
			players[i].programTurnLeft = 10;
			players[i].programTurnRight = 11;
			players[i].programWalkBackwards = 16;
			players[i].programSidestep = 2;
			players[i].programRun = 0;
			players[i].programSwitchWeapons = 0;
			players[i].programCollection = -1;
			players[i].jointPackage = -1;
			players[i].charMain.numP = 0L;
			players[i].charMain.numUsed = 0L;
			players[i].lastParticleCount = 0;
			players[i].charMain.bbDirty = true;
			players[i].team = 0;
			players[i].teamMask = Get_Team_Mask(players[i].team);
			players[i].maxDamage = 100f;
			players[i].roundPts = 0;
			players[i].objectivePoints = 0;
			players[i].onmap = 1;
			players[i].dead = false;
			players[i].weapon1.jointID = 0;
			players[i].inRecoil = 0;
			players[i].transportParticle = -1;
			players[i].voiceCueID = -1;
			players[i].velX = 0f;
			players[i].velY = 0f;
			players[i].velZ = 0f;
			players[i].falling = false;
			players[i].active = false;
			players[i].id = -128;
			players[i].xRotation = 0f;
			for (int j = 0; j < 10; j++)
			{
				players[i].particles[j] = -1;
			}
			for (int j = 1; j < players[i].numAvailableWeapons; j++)
			{
				players[i].weaponList[j] = -1;
			}
			players[i].weaponList[0] = players[i].primaryWeaponMountWeapon;
			players[i].wpnIndex = 0;
			ref Matrix reference = ref players[i].mv[0];
			reference = Matrix.Identity;
			ref Matrix reference2 = ref players[i].mv[1];
			reference2 = Matrix.Identity;
			players[i].projectileResistance = 0.5f;
		}
		players[0].active = true;
		enemyTeamMask = ~players[0].teamMask;
		global::Collision.Collision.thirdPersonView.v1 = new StructsClass.vtex[8];
		for (int i = 0; i < 8; i++)
		{
			global::Collision.Collision.thirdPersonView.v1[i] = new StructsClass.vtex();
		}
		global::Collision.Collision.thirdPersonView.numP = 1L;
		global::Collision.Collision.thirdPersonView.numUsed = 1L;
		global::Collision.Collision.thirdPersonView.v1[0].v[0] = 0f;
		global::Collision.Collision.thirdPersonView.v1[0].v[1] = 0f;
		global::Collision.Collision.thirdPersonView.v1[0].v[2] = 0f;
		float num = (float)Math.PI / 2f;
		for (int i = 0; i < 4; i++)
		{
			remotePlayerPositionOffsetX[i] = (float)Math.Sin(num * (float)i);
			remotePlayerPositionOffsetY[i] = (float)Math.Cos(num * (float)i);
		}
	}

	public void Init_New_Multiplayer_Gamer(string un, int playerID, short actID)
	{
		players[playerID].id = actID;
		global::Networking.Networking.networkPlayers[playerID].playerLoaded = false;
		global::Networking.Networking.networkPlayers[playerID].haveRemotePlayerTeam = false;
		global::Networking.Networking.networkPlayers[playerID].haveRemotePlayerArrayPosition = false;
		global::Networking.Networking.networkPlayers[playerID].haveAllRemotePlayerDataForStart = false;
		if (global::Networking.Networking.networkSessionReady)
		{
			if (global::Networking.Networking.isHost)
			{
				bool flag = false;
				int i;
				for (i = 0; i < 4; i++)
				{
					if (remotePlayerPositions[i] == actID)
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					for (i = 0; i < 4; i++)
					{
						if (remotePlayerPositions[i] == -1)
						{
							remotePlayerPositions[i] = actID;
							flag = true;
							break;
						}
					}
				}
				if (flag)
				{
					mainC.networkingMain.Send_Player_Array_Position(actID, (short)i);
					global::Networking.Networking.networkPlayers[playerID].playerArrayPosition = (short)i;
					global::Networking.Networking.networkPlayers[playerID].haveRemotePlayerArrayPosition = true;
				}
			}
			else
			{
				for (int i = 0; i < 4; i++)
				{
					if (remotePlayerPositions[i] == actID)
					{
						global::Networking.Networking.networkPlayers[playerID].playerArrayPosition = (short)i;
						global::Networking.Networking.networkPlayers[playerID].haveRemotePlayerArrayPosition = true;
						break;
					}
				}
			}
		}
		players[playerID].username = un;
		Set_Player_Abbreviated_Name((ushort)playerID);
		mainC.gameLogic.Game_Init_New_Player((byte)playerID);
	}

	public void Set_Hosts_Array_Position(byte localPlayerID)
	{
		if (localPlayerID >= global::Networking.Networking.networkSession.LocalGamers.Count)
		{
			localPlayerID = 0;
		}
		players[localPlayerID].id = global::Networking.Networking.networkSession.LocalGamers[localPlayerID].Id;
		short id = players[localPlayerID].id;
		bool flag = false;
		int i;
		for (i = 0; i < 4; i++)
		{
			if (remotePlayerPositions[i] == id)
			{
				flag = true;
				break;
			}
		}
		if (!flag)
		{
			for (i = 0; i < 4; i++)
			{
				if (remotePlayerPositions[i] == -1)
				{
					remotePlayerPositions[i] = id;
					flag = true;
					break;
				}
			}
		}
		if (flag)
		{
			mainC.networkingMain.Send_Player_Array_Position(id, (short)i);
			global::Networking.Networking.networkPlayers[localPlayerID].playerArrayPosition = (short)i;
			global::Networking.Networking.networkPlayers[localPlayerID].haveRemotePlayerArrayPosition = true;
		}
	}

	public void Init_Player_Program(int startID, int endID)
	{
		for (int i = startID; i < endID; i++)
		{
			int programCollection = players[i].programCollection;
			int numAnimations = global::Programs.Programs.pgC[programCollection].numAnimations;
			players[i].animations = new StructsClass.animation_instance[numAnimations];
			for (int j = 0; j < numAnimations; j++)
			{
				players[i].animations[j].callBack = global::Programs.Programs.pgC[programCollection].animation1[j].callBack;
				players[i].animations[j].callBackType = global::Programs.Programs.pgC[programCollection].animation1[j].callBackType;
				players[i].animations[j].cancelledCallBack = global::Programs.Programs.pgC[programCollection].animation1[j].cancelledCallBack;
				players[i].animations[j].cancelledCallBackType = global::Programs.Programs.pgC[programCollection].animation1[j].cancelledCallBackType;
				players[i].animations[j].group = global::Programs.Programs.pgC[programCollection].animation1[j].group;
				players[i].animations[j].loop = global::Programs.Programs.pgC[programCollection].animation1[j].loop;
				players[i].animations[j].staysActiveAtEnd = global::Programs.Programs.pgC[programCollection].animation1[j].staysActiveAtEnd;
				players[i].animations[j].status = 0;
				players[i].animations[j].curTime = 0f;
				int numAnimationSequences = global::Programs.Programs.pgC[programCollection].animation1[j].numAnimationSequences;
				players[i].animations[j].curFrames = new ushort[numAnimationSequences];
				for (int k = 0; k < numAnimationSequences; k++)
				{
					players[i].animations[j].curFrames[k] = 0;
				}
				numAnimationSequences = global::Programs.Programs.pgC[programCollection].animation1[j].numActions;
				players[i].animations[j].actionComplete = new bool[numAnimationSequences];
				for (int k = 0; k < numAnimationSequences; k++)
				{
					players[i].animations[j].actionComplete[k] = false;
				}
			}
			numAnimations = global::Programs.Programs.pgC[programCollection].numPrograms;
			players[i].pg1 = new StructsClass.program_instance[numAnimations];
			for (int j = 0; j < numAnimations; j++)
			{
				players[i].pg1[j] = default(StructsClass.program_instance);
				players[i].pg1[j].callBack = global::Programs.Programs.pgC[programCollection].pg1[j].callBack;
				players[i].pg1[j].callBackType = global::Programs.Programs.pgC[programCollection].pg1[j].callBackType;
				players[i].pg1[j].reverse = global::Programs.Programs.pgC[programCollection].pg1[j].reverse;
				players[i].pg1[j].staysActiveAtEnd = global::Programs.Programs.pgC[programCollection].pg1[j].staysActiveAtEnd;
				players[i].pg1[j].inReverse = global::Programs.Programs.pgC[programCollection].pg1[j].inReverse;
				players[i].pg1[j].curStep = 0;
				players[i].pg1[j].status = global::Programs.Programs.pgC[programCollection].pg1[j].status;
				players[i].pg1[j].status = 1;
				int numAnimationSequences = global::Programs.Programs.pgC[programCollection].pg1[j].numJoints * global::Programs.Programs.pgC[programCollection].pg1[j].numSteps;
				if (numAnimationSequences < 1)
				{
					players[i].pg1[j].status = 0;
				}
			}
		}
	}

	public void Init_Player_Joints(int startID, int endID)
	{
		bool flag = false;
		for (int i = startID; i < endID; i++)
		{
			flag = false;
			int jointPackage = players[i].jointPackage;
			byte b = (byte)global::Joints.Joints.playerJoints[jointPackage].numJoints;
			if (players[i].numAllocatedJoints < maxNumPlayerRaceJoints)
			{
				flag = true;
				players[i].jt1 = new StructsClass.joint[maxNumPlayerRaceJoints];
				players[i].jColT = new float[4, maxNumPlayerRaceJoints];
				players[i].jVect1T = new StructsClass.vtex[4, maxNumPlayerRaceJoints];
				players[i].jVect2T = new StructsClass.vtex[4, maxNumPlayerRaceJoints];
				players[i].jVect3T = new StructsClass.vtex[4, maxNumPlayerRaceJoints];
				players[i].numAllocatedJoints = (byte)maxNumPlayerRaceJoints;
			}
			players[i].numJoints = b;
			int j;
			for (j = 0; j < maxNumPlayerRaceJoints; j++)
			{
				if (flag)
				{
					players[i].jt1[j] = new StructsClass.joint();
					for (int k = 0; k < 4; k++)
					{
						for (int l = 0; l < maxNumPlayerRaceJoints; l++)
						{
							players[i].jVect1T[k, l] = new StructsClass.vtex();
							players[i].jVect2T[k, l] = new StructsClass.vtex();
							players[i].jVect3T[k, l] = new StructsClass.vtex();
						}
					}
				}
				jointPackage = ((j >= b) ? playerRaces[raceWithMostJoints].jointPackage[raceTypeWithMostJoints] : players[i].jointPackage);
				players[i].jt1[j].numVertices = global::Joints.Joints.playerJoints[jointPackage].jt1[j].numVertices;
				players[i].jt1[j].numIndexes = global::Joints.Joints.playerJoints[jointPackage].jt1[j].numIndexes;
				players[i].jt1[j].numPrimitives = global::Joints.Joints.playerJoints[jointPackage].jt1[j].numPrimitives;
				players[i].jt1[j].adjustEndRing = global::Joints.Joints.playerJoints[jointPackage].jt1[j].adjustEndRing;
				players[i].jt1[j].adjustStartRing = global::Joints.Joints.playerJoints[jointPackage].jt1[j].adjustStartRing;
				players[i].jt1[j].rotX = global::Joints.Joints.playerJoints[jointPackage].jt1[j].rotX;
				players[i].jt1[j].angleSpeed = global::Joints.Joints.playerJoints[jointPackage].jt1[j].angleSpeed;
				players[i].jt1[j].angleX = global::Joints.Joints.playerJoints[jointPackage].jt1[j].angleX;
				players[i].jt1[j].angleY = global::Joints.Joints.playerJoints[jointPackage].jt1[j].angleY;
				players[i].jt1[j].angleZ = global::Joints.Joints.playerJoints[jointPackage].jt1[j].angleZ;
				players[i].jt1[j].mvBase = global::Joints.Joints.playerJoints[jointPackage].jt1[j].mvBase;
				players[i].jt1[j].curSub = global::Joints.Joints.playerJoints[jointPackage].jt1[j].curSub;
				players[i].jt1[j].damageMultiplier = global::Joints.Joints.playerJoints[jointPackage].jt1[j].damageMultiplier;
				players[i].jt1[j].damageJoint = global::Joints.Joints.playerJoints[jointPackage].jt1[j].damageJoint;
				players[i].jt1[j].len = global::Joints.Joints.playerJoints[jointPackage].jt1[j].len;
				players[i].jt1[j].lenSquared = global::Joints.Joints.playerJoints[jointPackage].jt1[j].lenSquared;
				players[i].jt1[j].maxAngle = global::Joints.Joints.playerJoints[jointPackage].jt1[j].maxAngle;
				players[i].jt1[j].maxPinH = global::Joints.Joints.playerJoints[jointPackage].jt1[j].maxPinH;
				players[i].jt1[j].maxPivot = global::Joints.Joints.playerJoints[jointPackage].jt1[j].maxPivot;
				players[i].jt1[j].maxPivot2 = global::Joints.Joints.playerJoints[jointPackage].jt1[j].maxPivot2;
				players[i].jt1[j].minAngle = global::Joints.Joints.playerJoints[jointPackage].jt1[j].minAngle;
				players[i].jt1[j].minPivot = global::Joints.Joints.playerJoints[jointPackage].jt1[j].minPivot;
				players[i].jt1[j].minPivot2 = global::Joints.Joints.playerJoints[jointPackage].jt1[j].minPivot2;
				players[i].jt1[j].modID = global::Joints.Joints.playerJoints[jointPackage].jt1[j].modID;
				players[i].jt1[j].parentID = global::Joints.Joints.playerJoints[jointPackage].jt1[j].parentID;
				players[i].jt1[j].pinAngleD = global::Joints.Joints.playerJoints[jointPackage].jt1[j].pinAngleD;
				players[i].jt1[j].pinOffset = global::Joints.Joints.playerJoints[jointPackage].jt1[j].pinOffset;
				players[i].jt1[j].rotZ = global::Joints.Joints.playerJoints[jointPackage].jt1[j].rotZ;
				players[i].jt1[j].rotY = global::Joints.Joints.playerJoints[jointPackage].jt1[j].rotY;
				players[i].jt1[j].pivot2Speed = global::Joints.Joints.playerJoints[jointPackage].jt1[j].pivot2Speed;
				players[i].jt1[j].pivotSpeed = global::Joints.Joints.playerJoints[jointPackage].jt1[j].pivotSpeed;
				players[i].jt1[j].pListStart = global::Joints.Joints.playerJoints[jointPackage].jt1[j].pListStart;
				players[i].jt1[j].pSkip1 = global::Joints.Joints.playerJoints[jointPackage].jt1[j].pSkip1;
				players[i].jt1[j].pSkip2 = global::Joints.Joints.playerJoints[jointPackage].jt1[j].pSkip2;
				players[i].jt1[j].ringPtCnt = global::Joints.Joints.playerJoints[jointPackage].jt1[j].ringPtCnt;
				players[i].jt1[j].rings = global::Joints.Joints.playerJoints[jointPackage].jt1[j].rings;
				players[i].jt1[j].ringYD = global::Joints.Joints.playerJoints[jointPackage].jt1[j].ringYD;
				players[i].jt1[j].status = global::Joints.Joints.playerJoints[jointPackage].jt1[j].status;
				players[i].jt1[j].subIDCount = global::Joints.Joints.playerJoints[jointPackage].jt1[j].subIDCount;
				players[i].jt1[j].targetAngle = global::Joints.Joints.playerJoints[jointPackage].jt1[j].targetAngle;
				players[i].jt1[j].targetPivot = global::Joints.Joints.playerJoints[jointPackage].jt1[j].targetPivot;
				players[i].jt1[j].targetPivot2 = global::Joints.Joints.playerJoints[jointPackage].jt1[j].targetPivot2;
				players[i].jt1[j].texID = global::Joints.Joints.playerJoints[jointPackage].jt1[j].texID;
				players[i].jt1[j].x = global::Joints.Joints.playerJoints[jointPackage].jt1[j].x;
				players[i].jt1[j].y = global::Joints.Joints.playerJoints[jointPackage].jt1[j].y;
				players[i].jt1[j].z = global::Joints.Joints.playerJoints[jointPackage].jt1[j].z;
				ref Matrix reference = ref players[i].jt1[j].mv[0];
				reference = Matrix.Identity;
				ref Matrix reference2 = ref players[i].jt1[j].mv[1];
				reference2 = Matrix.Identity;
				players[i].jt1[j].dirX = global::Joints.Joints.playerJoints[jointPackage].jt1[j].dirX;
				players[i].jt1[j].dirY = global::Joints.Joints.playerJoints[jointPackage].jt1[j].dirY;
				players[i].jt1[j].dirZ = global::Joints.Joints.playerJoints[jointPackage].jt1[j].dirZ;
				players[i].jt1[j].radius = global::Joints.Joints.playerJoints[jointPackage].jt1[j].radius;
				players[i].jt1[j].radSqr = global::Joints.Joints.playerJoints[jointPackage].jt1[j].radSqr;
				players[i].jt1[j].idList = new short[players[i].jt1[j].subIDCount];
				for (int m = 0; m < players[i].jt1[j].subIDCount; m++)
				{
					players[i].jt1[j].idList[m] = global::Joints.Joints.playerJoints[jointPackage].jt1[j].idList[m];
				}
				long num = players[i].jt1[j].rings * players[i].jt1[j].ringPtCnt;
				players[i].jt1[j].ringPts = new StructsClass.vtex[num];
				players[i].jt1[j].ringPins = new float[num];
				players[i].jt1[j].angles = new float[players[i].jt1[j].rings];
				players[i].jt1[j].ringNorms = new StructsClass.vnorm[num];
				players[i].jt1[j].tangent = new StructsClass.vtex[num];
				players[i].jt1[j].bitangent = new StructsClass.vtex[num];
				for (int m = 0; m < num; m++)
				{
					players[i].jt1[j].ringPins[m] = global::Joints.Joints.playerJoints[jointPackage].jt1[j].ringPins[m];
					players[i].jt1[j].ringPts[m] = new StructsClass.vtex();
					players[i].jt1[j].tangent[m] = new StructsClass.vtex();
					players[i].jt1[j].bitangent[m] = new StructsClass.vtex();
					players[i].jt1[j].ringNorms[m] = new StructsClass.vnorm();
				}
				short parentCount = global::Joints.Joints.playerJoints[jointPackage].jt1[j].parentCount;
				players[i].jt1[j].parentCount = parentCount;
				players[i].jt1[j].parentList = new short[parentCount];
				for (int m = 0; m < parentCount; m++)
				{
					players[i].jt1[j].parentList[m] = global::Joints.Joints.playerJoints[jointPackage].jt1[j].parentList[m];
				}
			}
			jointPackage = players[i].jointPackage;
			j = global::Joints.Joints.playerJoints[jointPackage].numJointPoints;
			if (players[i].charMain.numP < j)
			{
				players[i].charMain.numP = j;
				players[i].charMain.v1 = new StructsClass.vtex[j];
				players[i].particlePrev = new StructsClass.vtex[j];
				for (int m = 0; m < j; m++)
				{
					players[i].charMain.v1[m] = new StructsClass.vtex();
					players[i].particlePrev[m] = new StructsClass.vtex();
				}
			}
			players[i].charMain.numUsed = j;
			players[i].lastParticleCount = (short)j;
			players[i].charMain.bbDirty = true;
			for (j = 0; j < b; j++)
			{
				global::Joints.Joints.Translate_Player_Joint_Vertex(i, j, copyToParticleList: true);
			}
			players[i].ct1.x = global::Joints.Joints.playerJoints[jointPackage].ct1.x;
			players[i].ct1.y = global::Joints.Joints.playerJoints[jointPackage].ct1.y;
			players[i].ct1.z = global::Joints.Joints.playerJoints[jointPackage].ct1.z;
			players[i].ct1.angleX = global::Joints.Joints.playerJoints[jointPackage].ct1.angleX;
			players[i].ct1.angleY = global::Joints.Joints.playerJoints[jointPackage].ct1.angleY;
			players[i].ct1.angleZ = global::Joints.Joints.playerJoints[jointPackage].ct1.angleZ;
			players[i].ct1.modID = global::Joints.Joints.playerJoints[jointPackage].ct1.modID;
			players[i].ct1.texID = global::Joints.Joints.playerJoints[jointPackage].ct1.texID;
			players[i].ct1.mv = global::Joints.Joints.playerJoints[jointPackage].ct1.mv;
		}
	}

	public void Set_Player_Array_Position_From_Network()
	{
		int num = Get_Player_Index(global::Networking.Networking.networkInts[0], -1);
		if (num > -1)
		{
			global::Networking.Networking.networkPlayers[num].playerLoaded = true;
			global::Networking.Networking.networkPlayers[num].playerArrayPosition = global::Networking.Networking.networkShorts[0];
			remotePlayerPositions[global::Networking.Networking.networkPlayers[num].playerArrayPosition] = global::Networking.Networking.networkInts[0];
			global::Networking.Networking.networkPlayers[num].haveRemotePlayerArrayPosition = true;
		}
	}

	public void Send_Local_Player_Array_Position_To_New_Gamer(NetworkGamer remoteGamer)
	{
		if (global::Networking.Networking.networkPlayers[0].playerArrayPosition >= 0)
		{
			global::Networking.Networking.networkInts[0] = players[0].id;
			global::Networking.Networking.networkShorts[0] = global::Networking.Networking.networkPlayers[0].playerArrayPosition;
			mainC.networkingMain.XBOX_Send_Network_Message_To_Gamer(39, remoteGamer);
		}
	}

	public void Send_Remote_Player_Array_Position_To_New_Gamer(NetworkGamer remoteGamer)
	{
		ushort num = (ushort)Get_Player_Index(remoteGamer.Id, -1);
		if (num >= 0 && global::Networking.Networking.networkPlayers[num].playerArrayPosition >= 0)
		{
			global::Networking.Networking.networkInts[0] = players[num].id;
			global::Networking.Networking.networkShorts[0] = global::Networking.Networking.networkPlayers[num].playerArrayPosition;
			mainC.networkingMain.XBOX_Send_Network_Message_To_Gamer(39, remoteGamer);
		}
	}

	public void Initialize_All_Players_To_Default_Race()
	{
		global::MainGame.MainGame.needToLoadWeapons = true;
		for (byte b = 0; b < 44; b++)
		{
			Set_Player_Race(b, 0, 0);
		}
	}

	public void Load_Player_Race_Data(string fileName)
	{
		int num = -1;
		int num2 = 0;
		int num3 = 0;
		_ = global::Rendering.Rendering.uBufferID;
		for (int i = 0; i < numPlayerRaces; i++)
		{
			playerRaces[i].numTypes = 0;
			playerRaces[i].numBloodModels = 0;
			playerRaces[i].soundDeath = null;
			playerRaces[i].soundHurt = null;
		}
		maxNumPlayerRaceJoints = 0;
		Stream stream = TitleContainer.OpenStream("The_CoOp_Zombie_Game\\Config_Files\\" + fileName);
		byte[] array = new byte[stream.Length];
		if (stream.CanRead)
		{
			stream.Read(array, 0, array.Length);
			string text = mainC.utilMain.Byte_Array_To_String(array);
			string[] array2 = text.Split('\n', '\r');
			int j = 0;
			int num4 = 0;
			for (; j < array2.Length; j++)
			{
				if (array2[j].Length > 0)
				{
					num4++;
				}
			}
			if (num4 < 1)
			{
				stream.Close();
				return;
			}
			string[] array3 = new string[num4];
			j = 0;
			num4 = 0;
			for (; j < array2.Length; j++)
			{
				if (array2[j].Length > 0)
				{
					array3[num4++] = array2[j];
				}
			}
			for (j = 0; j < num4; j++)
			{
				array2 = array3[j].Split(' ', '\t');
				int k = 0;
				int num5 = 0;
				for (; k < array2.Length; k++)
				{
					if (array2[k].Length > 0)
					{
						num5++;
					}
				}
				if (num5 < 1)
				{
					continue;
				}
				string[] array4 = new string[num5];
				k = 0;
				num5 = 0;
				for (; k < array2.Length; k++)
				{
					if (array2[k].Length > 0)
					{
						array4[num5++] = array2[k];
					}
				}
				int num6 = 0;
				if (array4[0].Equals("numPlayerRaces", StringComparison.OrdinalIgnoreCase))
				{
					num6 = 1;
				}
				else if (array4[0].Equals("Race", StringComparison.OrdinalIgnoreCase))
				{
					num6 = 2;
				}
				else if (array4[0].Equals("numTypes", StringComparison.OrdinalIgnoreCase))
				{
					num6 = 3;
				}
				else if (array4[0].Equals("teleportInSound", StringComparison.OrdinalIgnoreCase))
				{
					num6 = 4;
				}
				else if (array4[0].Equals("teleportOutSound", StringComparison.OrdinalIgnoreCase))
				{
					num6 = 5;
				}
				else if (array4[0].Equals("bloodColor", StringComparison.OrdinalIgnoreCase))
				{
					num6 = 6;
				}
				else if (array4[0].Equals("bloodSize", StringComparison.OrdinalIgnoreCase))
				{
					num6 = 7;
				}
				else if (array4[0].Equals("bloodChange", StringComparison.OrdinalIgnoreCase))
				{
					num6 = 8;
				}
				else if (array4[0].Equals("weaponDischarge", StringComparison.OrdinalIgnoreCase))
				{
					num6 = 9;
				}
				else if (array4[0].Equals("type", StringComparison.OrdinalIgnoreCase))
				{
					num6 = 10;
				}
				else if (array4[0].Equals("jointPackage", StringComparison.OrdinalIgnoreCase))
				{
					num6 = 11;
				}
				else if (array4[0].Equals("programCollection", StringComparison.OrdinalIgnoreCase))
				{
					num6 = 12;
				}
				else if (array4[0].Equals("bloodModels", StringComparison.OrdinalIgnoreCase))
				{
					num6 = 13;
				}
				else if (array4[0].Equals("torqueJoint", StringComparison.OrdinalIgnoreCase))
				{
					num6 = 14;
				}
				else if (array4[0].Equals("headJoint", StringComparison.OrdinalIgnoreCase))
				{
					num6 = 15;
				}
				else if (array4[0].Equals("thirdPesonJoint", StringComparison.OrdinalIgnoreCase))
				{
					num6 = 16;
				}
				else if (array4[0].Equals("eyeJoint", StringComparison.OrdinalIgnoreCase))
				{
					num6 = 17;
				}
				else if (array4[0].Equals("shoulderJointL", StringComparison.OrdinalIgnoreCase))
				{
					num6 = 18;
				}
				else if (array4[0].Equals("shoulderJointR", StringComparison.OrdinalIgnoreCase))
				{
					num6 = 19;
				}
				else if (array4[0].Equals("playerViewJoint1", StringComparison.OrdinalIgnoreCase))
				{
					num6 = 20;
				}
				else if (array4[0].Equals("weaponJoint", StringComparison.OrdinalIgnoreCase))
				{
					num6 = 21;
				}
				else if (array4[0].Equals("iconHeight", StringComparison.OrdinalIgnoreCase))
				{
					num6 = 22;
				}
				else if (array4[0].Equals("centerPoint", StringComparison.OrdinalIgnoreCase))
				{
					num6 = 23;
				}
				else if (array4[0].Equals("spawnHeight", StringComparison.OrdinalIgnoreCase))
				{
					num6 = 24;
				}
				else if (array4[0].Equals("deathSound", StringComparison.OrdinalIgnoreCase))
				{
					num6 = 25;
				}
				else if (array4[0].Equals("programStationaryLegsBody", StringComparison.OrdinalIgnoreCase))
				{
					num6 = 26;
				}
				else if (array4[0].Equals("programStationaryArms", StringComparison.OrdinalIgnoreCase))
				{
					num6 = 27;
				}
				else if (array4[0].Equals("programWalk", StringComparison.OrdinalIgnoreCase))
				{
					num6 = 28;
				}
				else if (array4[0].Equals("programJump", StringComparison.OrdinalIgnoreCase))
				{
					num6 = 29;
				}
				else if (array4[0].Equals("programTurnLeft", StringComparison.OrdinalIgnoreCase))
				{
					num6 = 30;
				}
				else if (array4[0].Equals("programTurnRight", StringComparison.OrdinalIgnoreCase))
				{
					num6 = 31;
				}
				else if (array4[0].Equals("programWalkBackwards", StringComparison.OrdinalIgnoreCase))
				{
					num6 = 32;
				}
				else if (array4[0].Equals("programSidestep", StringComparison.OrdinalIgnoreCase))
				{
					num6 = 33;
				}
				else if (array4[0].Equals("programRun", StringComparison.OrdinalIgnoreCase))
				{
					num6 = 34;
				}
				else if (array4[0].Equals("programDeath", StringComparison.OrdinalIgnoreCase))
				{
					num6 = 35;
				}
				else if (array4[0].Equals("Sound_Main", StringComparison.OrdinalIgnoreCase))
				{
					num6 = 36;
				}
				else if (array4[0].Equals("humanoidBackJoint", StringComparison.OrdinalIgnoreCase))
				{
					num6 = 37;
				}
				else if (array4[0].Equals("terminalVelocity", StringComparison.OrdinalIgnoreCase))
				{
					num6 = 38;
				}
				else if (array4[0].Equals("terminalVelocityThreshold", StringComparison.OrdinalIgnoreCase))
				{
					num6 = 39;
				}
				else if (array4[0].Equals("damageType", StringComparison.OrdinalIgnoreCase))
				{
					num6 = 40;
				}
				else if (array4[0].Equals("vehicleID", StringComparison.OrdinalIgnoreCase))
				{
					num6 = 41;
				}
				else if (array4[0].Equals("particleEffect", StringComparison.OrdinalIgnoreCase))
				{
					num6 = 42;
				}
				else if (array4[0].Equals("Melee_Radius", StringComparison.OrdinalIgnoreCase))
				{
					num6 = 43;
				}
				else if (array4[0].Equals("Sound_Hurt", StringComparison.OrdinalIgnoreCase))
				{
					num6 = 44;
				}
				else if (array4[0].Equals("BoundingBox_Corners", StringComparison.OrdinalIgnoreCase))
				{
					num6 = 45;
				}
				else if (array4[0].Equals("RenderScale", StringComparison.OrdinalIgnoreCase))
				{
					num6 = 46;
				}
				else if (array4[0].Equals("projectileResistance", StringComparison.OrdinalIgnoreCase))
				{
					num6 = 47;
				}
				else if (array4[0].Equals("minimapIcon", StringComparison.OrdinalIgnoreCase))
				{
					num6 = 48;
				}
				else if (array4[0].Equals("gamerTagHeight", StringComparison.OrdinalIgnoreCase))
				{
					num6 = 49;
				}
				else if (array4[0].Equals("soundIndex", StringComparison.OrdinalIgnoreCase))
				{
					num6 = 50;
				}
				else if (array4[0].Equals("programDeathBlownAway", StringComparison.OrdinalIgnoreCase))
				{
					num6 = 51;
				}
				else if (array4[0].Equals("programBulletImpact", StringComparison.OrdinalIgnoreCase))
				{
					num6 = 52;
				}
				else if (array4[0].Equals("numDeathAnimaions", StringComparison.OrdinalIgnoreCase))
				{
					num6 = 53;
				}
				else if (array4[0].Equals("programSwitchWeapons", StringComparison.OrdinalIgnoreCase))
				{
					num6 = 54;
				}
				else if (array4[0].Equals("deathCollisionPoint", StringComparison.OrdinalIgnoreCase))
				{
					num6 = 55;
				}
				else if (array4[0].Equals("height", StringComparison.OrdinalIgnoreCase))
				{
					num6 = 56;
				}
				else if (array4[0].Equals("numHitImpactAnimaions", StringComparison.OrdinalIgnoreCase))
				{
					num6 = 57;
				}
				else if (array4[0].Equals("meleeDistance", StringComparison.OrdinalIgnoreCase))
				{
					num6 = 58;
				}
				else if (array4[0].Equals("playerSeparationDistance", StringComparison.OrdinalIgnoreCase))
				{
					num6 = 59;
				}
				switch (num6)
				{
				case 1:
				{
					if (array4.Length <= 1)
					{
						break;
					}
					int num7 = int.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
					if (num7 > numAllocatedPlayerRaces)
					{
						playerRaces = new StructsClass.Player_Races[num7];
						for (int i = 0; i < num7; i++)
						{
							playerRaces[i].numTypes = 0;
							playerRaces[i].bloodColor = new float[4] { 1f, 0f, 0f, 1f };
							playerRaces[i].numBloodModels = 0;
						}
						numAllocatedPlayerRaces = (byte)num7;
					}
					numPlayerRaces = (byte)num7;
					break;
				}
				case 2:
					num++;
					if (num < 0 || num >= numPlayerRaces)
					{
						num = -1;
					}
					break;
				case 3:
				{
					if (num <= -1 || array4.Length <= 1)
					{
						break;
					}
					int num7 = int.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
					num3 = num7;
					num2 = 0;
					if (num7 > playerRaces[num].numAllocatedTypes)
					{
						playerRaces[num].vehicleID = new byte[num7];
						playerRaces[num].jointPackage = new byte[num7];
						playerRaces[num].jointPackageName = new string[num7];
						playerRaces[num].programCollection = new byte[num7];
						playerRaces[num].torqueJoint = new byte[num7];
						playerRaces[num].humanoidBackJoint = new byte[num7];
						playerRaces[num].headJoint = new byte[num7];
						playerRaces[num].thirdPesonJoint = new byte[num7];
						playerRaces[num].eyeJoint = new byte[num7];
						playerRaces[num].shoulderJointL = new byte[num7];
						playerRaces[num].shoulderJointR = new byte[num7];
						playerRaces[num].firstPersonViewJoint1 = new byte[num7];
						playerRaces[num].weaponJoint = new byte[num7];
						playerRaces[num].centerPoint = new float[num7];
						playerRaces[num].iconHeight = new float[num7];
						playerRaces[num].spawnHeight = new float[num7];
						playerRaces[num].particleEffect = new byte[num7];
						playerRaces[num].damageType = new byte[num7];
						playerRaces[num].playerSeparationDistance = new float[num7];
						playerRaces[num].meleeDistance = new float[num7];
						playerRaces[num].boundingRadius = new float[num7];
						playerRaces[num].bBox = new float[num7, 6];
						playerRaces[num].renderScale = new float[num7];
						playerRaces[num].projectileResistance = new float[num7];
						playerRaces[num].gamerTagHeight = new float[num7];
						playerRaces[num].playerHeight = new float[num7];
						playerRaces[num].deathParticle = new byte[num7];
						playerRaces[num].programStationaryLegsBody = new byte[num7];
						playerRaces[num].programStationaryArms = new byte[num7];
						playerRaces[num].programWalk = new byte[num7];
						playerRaces[num].programJump = new byte[num7];
						playerRaces[num].programTurnLeft = new byte[num7];
						playerRaces[num].programTurnRight = new byte[num7];
						playerRaces[num].programWalkBackwards = new byte[num7];
						playerRaces[num].programSidestep = new byte[num7];
						playerRaces[num].programRun = new byte[num7];
						playerRaces[num].programSwitchWeapons = new byte[num7];
						playerRaces[num].programDeath = new byte[num7, 1];
						playerRaces[num].programDeathBlownAway = new byte[num7, 1];
						playerRaces[num].soundDeath = new string[num7];
						playerRaces[num].soundHurt = new string[num7];
						playerRaces[num].soundMain = new string[num7];
						playerRaces[num].hurtTimerLength = new float[num7];
						playerRaces[num].mainSoundTimerLength = new float[num7];
						playerRaces[num].numAllocatedTypes = (byte)num7;
						playerRaces[num].miniMapIcon = new string[num7];
						playerRaces[num].miniMapIconID = new ushort[num7];
						playerRaces[num].soundIndex = new byte[num7];
						playerRaces[num].numDeathAnimations1 = 1;
						playerRaces[num].numDeathAnimations2 = 1;
						playerRaces[num].numBulletImpactAnimations = 0;
						int i;
						for (i = 0; i < num7; i++)
						{
							playerRaces[num].miniMapIcon[i] = "";
							playerRaces[num].miniMapIconID[i] = 0;
							playerRaces[num].gamerTagHeight[i] = 5f;
							playerRaces[num].playerHeight[i] = 5.8f;
							playerRaces[num].deathParticle[i] = 0;
							playerRaces[num].meleeDistance[i] = 1f;
							playerRaces[num].playerSeparationDistance[i] = 1f;
						}
						playerRaces[num].whiskers = new float[num7 * 9];
						i = 0;
						int num8 = 0;
						for (; i < num7; i++)
						{
							playerRaces[num].whiskers[num8++] = -5f;
							playerRaces[num].whiskers[num8++] = 20f;
							playerRaces[num].whiskers[num8++] = 20f;
							playerRaces[num].whiskers[num8++] = 25f;
							playerRaces[num].whiskers[num8++] = 20f;
							playerRaces[num].whiskers[num8++] = 20f;
							playerRaces[num].whiskers[num8++] = 10f;
							playerRaces[num].whiskers[num8++] = 20f;
							playerRaces[num].whiskers[num8++] = 20f;
						}
					}
					playerRaces[num].numTypes = (byte)num7;
					break;
				}
				case 4:
					if (num > -1 && array4.Length > 1)
					{
						playerRaces[num].teleportInSound = array4[1];
					}
					break;
				case 5:
					if (num > -1 && array4.Length > 1)
					{
						playerRaces[num].teleportOutSound = array4[1];
					}
					break;
				case 6:
					if (num > -1 && array4.Length > 4)
					{
						playerRaces[num].bloodColor[0] = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
						playerRaces[num].bloodColor[1] = float.Parse(array4[2], CultureInfo.InvariantCulture.NumberFormat);
						playerRaces[num].bloodColor[2] = float.Parse(array4[3], CultureInfo.InvariantCulture.NumberFormat);
						playerRaces[num].bloodColor[3] = float.Parse(array4[4], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 7:
					if (num > -1 && array4.Length > 1)
					{
						playerRaces[num].bloodSize = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 8:
					if (num > -1 && array4.Length > 1)
					{
						playerRaces[num].bloodSizeChange = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 9:
					if (num > -1 && array4.Length > 4)
					{
						playerRaces[num].weaponDischarge = new float[4];
						playerRaces[num].weaponDischarge[0] = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
						playerRaces[num].weaponDischarge[1] = float.Parse(array4[2], CultureInfo.InvariantCulture.NumberFormat);
						playerRaces[num].weaponDischarge[2] = float.Parse(array4[3], CultureInfo.InvariantCulture.NumberFormat);
						playerRaces[num].weaponDischarge[3] = float.Parse(array4[4], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 10:
					if (num > -1 && num3 > 0 && array4.Length > 1)
					{
						num2 = int.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
						if (num2 >= num3)
						{
							num2 = 0;
						}
					}
					break;
				case 11:
					if (num > -1 && num3 > 0 && array4.Length > 1)
					{
						playerRaces[num].jointPackageName[num2] = array4[1];
					}
					break;
				case 12:
					if (num > -1 && num3 > 0 && array4.Length > 1)
					{
						playerRaces[num].programCollection[num2] = byte.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 13:
				{
					if (num <= -1 || num3 <= 0 || array4.Length <= 1)
					{
						break;
					}
					int num7 = int.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
					playerRaces[num].numBloodModels = (byte)num7;
					if (array4.Length > num7 + 1)
					{
						playerRaces[num].bloodModels = new string[num7];
						playerRaces[num].bloodModelIDs = new ushort[num7];
						int i = 0;
						int num8 = 2;
						while (i < num7)
						{
							playerRaces[num].bloodModels[i] = array4[num8];
							playerRaces[num].bloodModelIDs[i] = mainC.modelsMain.Find_Model(array4[num8]);
							i++;
							num8++;
						}
					}
					break;
				}
				case 14:
					if (num > -1 && num3 > 0 && array4.Length > 1)
					{
						playerRaces[num].torqueJoint[num2] = byte.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 15:
					if (num > -1 && num3 > 0 && array4.Length > 1)
					{
						playerRaces[num].headJoint[num2] = byte.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 16:
					if (num > -1 && num3 > 0 && array4.Length > 1)
					{
						playerRaces[num].thirdPesonJoint[num2] = byte.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 17:
					if (num > -1 && num3 > 0 && array4.Length > 1)
					{
						playerRaces[num].eyeJoint[num2] = byte.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 18:
					if (num > -1 && num3 > 0 && array4.Length > 1)
					{
						playerRaces[num].shoulderJointL[num2] = byte.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 19:
					if (num > -1 && num3 > 0 && array4.Length > 1)
					{
						playerRaces[num].shoulderJointR[num2] = byte.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 20:
					if (num > -1 && num3 > 0 && array4.Length > 1)
					{
						playerRaces[num].firstPersonViewJoint1[num2] = byte.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 21:
					if (num > -1 && num3 > 0 && array4.Length > 1)
					{
						playerRaces[num].weaponJoint[num2] = byte.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 22:
					if (num > -1 && num3 > 0 && array4.Length > 1)
					{
						playerRaces[num].iconHeight[num2] = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 23:
					if (num > -1 && num3 > 0 && array4.Length > 1)
					{
						playerRaces[num].centerPoint[num2] = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 24:
					if (num > -1 && num3 > 0 && array4.Length > 1)
					{
						playerRaces[num].spawnHeight[num2] = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 25:
					if (num > -1 && num3 > 0 && array4.Length > 1)
					{
						playerRaces[num].soundDeath[num2] = array4[1];
					}
					break;
				case 26:
					if (num > -1 && num3 > 0 && array4.Length > 1)
					{
						playerRaces[num].programStationaryLegsBody[num2] = byte.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 27:
					if (num > -1 && num3 > 0 && array4.Length > 1)
					{
						playerRaces[num].programStationaryArms[num2] = byte.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 28:
					if (num > -1 && num3 > 0 && array4.Length > 1)
					{
						playerRaces[num].programWalk[num2] = byte.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 29:
					if (num > -1 && num3 > 0 && array4.Length > 1)
					{
						playerRaces[num].programJump[num2] = byte.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 30:
					if (num > -1 && num3 > 0 && array4.Length > 1)
					{
						playerRaces[num].programTurnLeft[num2] = byte.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 31:
					if (num > -1 && num3 > 0 && array4.Length > 1)
					{
						playerRaces[num].programTurnRight[num2] = byte.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 32:
					if (num > -1 && num3 > 0 && array4.Length > 1)
					{
						playerRaces[num].programWalkBackwards[num2] = byte.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 33:
					if (num > -1 && num3 > 0 && array4.Length > 1)
					{
						playerRaces[num].programSidestep[num2] = byte.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 34:
					if (num > -1 && num3 > 0 && array4.Length > 1)
					{
						playerRaces[num].programRun[num2] = byte.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 35:
					if (num > -1 && num3 > 0 && array4.Length > 1)
					{
						int num7 = playerRaces[num].numDeathAnimations1;
						for (int i = 0; i < num7; i++)
						{
							playerRaces[num].programDeath[num2, i] = byte.Parse(array4[1 + i], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
						}
					}
					break;
				case 36:
					if (num > -1 && num3 > 0 && array4.Length > 2)
					{
						playerRaces[num].mainSoundTimerLength[num2] = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
						playerRaces[num].soundMain[num2] = array4[2];
					}
					break;
				case 37:
					if (num > -1 && num3 > 0 && array4.Length > 1)
					{
						playerRaces[num].humanoidBackJoint[num2] = byte.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 38:
					if (num > -1 && array4.Length > 1)
					{
						playerRaces[num].velocityTerminal = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 39:
					if (num > -1 && array4.Length > 1)
					{
						playerRaces[num].velocityTerminalThreshold = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 40:
					if (num > -1 && num3 > 0 && array4.Length > 1)
					{
						playerRaces[num].damageType[num2] = byte.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 41:
					if (num > -1 && num3 > 0 && array4.Length > 1)
					{
						playerRaces[num].vehicleID[num2] = byte.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 42:
					if (num > -1 && num3 > 0 && array4.Length > 1)
					{
						playerRaces[num].particleEffect[num2] = byte.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 43:
					if (num > -1 && array4.Length > 1)
					{
						playerRaces[num].boundingRadius[num2] = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 44:
					if (num > -1 && num3 > 0 && array4.Length > 2)
					{
						playerRaces[num].hurtTimerLength[num2] = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
						playerRaces[num].soundHurt[num2] = array4[2];
					}
					break;
				case 45:
					if (num > -1 && num3 > 0 && array4.Length > 2)
					{
						playerRaces[num].bBox[num2, 0] = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
						playerRaces[num].bBox[num2, 1] = float.Parse(array4[2], CultureInfo.InvariantCulture.NumberFormat);
						playerRaces[num].bBox[num2, 2] = float.Parse(array4[3], CultureInfo.InvariantCulture.NumberFormat);
						playerRaces[num].bBox[num2, 3] = float.Parse(array4[4], CultureInfo.InvariantCulture.NumberFormat);
						playerRaces[num].bBox[num2, 4] = float.Parse(array4[5], CultureInfo.InvariantCulture.NumberFormat);
						playerRaces[num].bBox[num2, 5] = float.Parse(array4[6], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 46:
					if (num > -1 && array4.Length > 1)
					{
						playerRaces[num].renderScale[num2] = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 47:
					if (num > -1 && array4.Length > 1)
					{
						playerRaces[num].projectileResistance[num2] = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 48:
					if (num > -1 && array4.Length > 1)
					{
						playerRaces[num].miniMapIcon[num2] = array4[1];
						playerRaces[num].miniMapIconID[num2] = (ushort)mainC.texturesMain.Find_Texture(array4[1], 0);
					}
					break;
				case 49:
					if (num > -1 && array4.Length > 1)
					{
						playerRaces[num].gamerTagHeight[num2] = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 50:
					if (num > -1 && num3 > 0 && array4.Length > 1)
					{
						playerRaces[num].soundIndex[num2] = byte.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 51:
					if (num > -1 && num3 > 0 && array4.Length > 1)
					{
						int num7 = playerRaces[num].numDeathAnimations2;
						for (int i = 0; i < num7; i++)
						{
							playerRaces[num].programDeathBlownAway[num2, i] = byte.Parse(array4[1 + i], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
						}
					}
					break;
				case 52:
					if (num > -1 && num3 > 0 && array4.Length > 1)
					{
						int num7 = playerRaces[num].numBulletImpactAnimations;
						for (int i = 0; i < num7; i++)
						{
							playerRaces[num].programBulletHit[num2, i] = byte.Parse(array4[1 + i], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
						}
					}
					break;
				case 53:
					if (num > -1 && num3 > 0 && array4.Length > 2)
					{
						playerRaces[num].numDeathAnimations1 = byte.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
						playerRaces[num].numDeathAnimations2 = byte.Parse(array4[2], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
						playerRaces[num].programDeath = new byte[playerRaces[num].numTypes, playerRaces[num].numDeathAnimations1];
						playerRaces[num].programDeathBlownAway = new byte[playerRaces[num].numTypes, playerRaces[num].numDeathAnimations2];
					}
					break;
				case 54:
					if (num > -1 && num3 > 0 && array4.Length > 1)
					{
						playerRaces[num].programSwitchWeapons[num2] = byte.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 55:
					if (num > -1 && num3 > 0 && array4.Length > 1)
					{
						playerRaces[num].deathParticle[num2] = byte.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 56:
					if (num > -1 && array4.Length > 1)
					{
						playerRaces[num].playerHeight[num2] = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 57:
					if (num > -1 && num3 > 0 && array4.Length > 1)
					{
						playerRaces[num].numBulletImpactAnimations = byte.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
						if (playerRaces[num].numBulletImpactAnimations > 0)
						{
							playerRaces[num].programBulletHit = new byte[playerRaces[num].numTypes, playerRaces[num].numBulletImpactAnimations];
						}
					}
					break;
				case 58:
					if (num > -1 && array4.Length > 1)
					{
						playerRaces[num].meleeDistance[num2] = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 59:
					if (num > -1 && array4.Length > 1)
					{
						playerRaces[num].playerSeparationDistance[num2] = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				}
			}
		}
		stream.Close();
	}

	public void Set_Player_Race_Models()
	{
		for (ushort num = 0; num < numPlayerRaces; num++)
		{
			for (ushort num2 = 0; num2 < playerRaces[num].numBloodModels; num2++)
			{
				playerRaces[num].bloodModelIDs[num2] = mainC.modelsMain.Find_Model(playerRaces[num].bloodModels[num2]);
			}
		}
	}

	public void Reset_Team_Counts_To_Zero()
	{
		for (ushort num = 0; num < 5; num++)
		{
			pluTeam[num] = 0;
		}
	}

	public void Verify_Player_Team_Counts()
	{
		ushort num;
		for (num = 0; num < 5; num++)
		{
			pluTeam[num] = 0;
		}
		for (num = 0; num < global::MainGame.MainGame.maxHumanGamePlayers; num++)
		{
			if (global::Networking.Networking.networkPlayers[num].haveRemotePlayerTeam)
			{
				try
				{
					pluTeam[players[num].team]++;
				}
				catch
				{
					global::InputHandler.InputHandler.tw = 0f;
				}
			}
		}
		while (num < global::MainGame.MainGame.maxGamePlayers)
		{
			try
			{
				pluTeam[players[num].team]++;
			}
			catch
			{
				global::InputHandler.InputHandler.tw = 0f;
			}
			num++;
		}
	}

	public ushort Get_Team_With_Fewest_Players(ushort startTeam)
	{
		ushort result = 0;
		ushort num = 0;
		for (ushort num2 = 0; num2 < global::MainGame.MainGame.numTeams; num2++)
		{
			if (pluTeam[num2] > 0)
			{
				result = num2;
				num = pluTeam[num2];
				break;
			}
		}
		if (num == 0)
		{
			num = 1;
		}
		for (ushort num2 = startTeam; num2 < global::MainGame.MainGame.numTeams; num2++)
		{
			if (pluTeam[num2] < num)
			{
				result = num2;
				num = pluTeam[num2];
			}
		}
		for (ushort num2 = 0; num2 < startTeam; num2++)
		{
			if (pluTeam[num2] < num)
			{
				result = num2;
				num = pluTeam[num2];
			}
		}
		return result;
	}

	public ushort Assign_Team(int oldTeam)
	{
		ushort num = Get_Team_With_Fewest_Players(global::MainGame.MainGame.numTeams);
		pluTeam[num]++;
		if (oldTeam > -1)
		{
			pluTeam[oldTeam]--;
		}
		return num;
	}

	public ulong Get_Team_Mask(ushort teamID)
	{
		return (ulong)(1 << (int)teamID);
	}

	public void Host_Changes_Team()
	{
		switch (global::MainGame.MainGame.gameType)
		{
		case 0:
		case 7:
		{
			ushort num = mainC.playersMain.Assign_Team(players[0].team);
			if (num != players[0].team)
			{
				if (global::MainGame.MainGame.commanderMode && global::MainGame.MainGame.Commander[players[0].team] == 0)
				{
					global::MainGame.MainGame.Commander[players[0].team] = -1;
					Setup_Players_For_Commander_Mode();
				}
				LocalPlayer_Team_Change();
			}
			break;
		}
		case 1:
			if (global::MainGame.MainGame.Guards[0] == 1)
			{
				global::MainGame.MainGame.Guards[0] = 2;
				global::MainGame.MainGame.isGuard = false;
			}
			else
			{
				global::MainGame.MainGame.Guards[0] = 1;
				global::MainGame.MainGame.isGuard = true;
			}
			mainC.gameLogic.Game_Send_GameSettings(2);
			break;
		}
	}

	public void Set_Team_From_Network()
	{
		players[0].team = global::Networking.Networking.networkBytes[0];
		LocalPlayer_Team_Change();
	}

	public void Update_Player_Team_From_Network()
	{
		int num = mainC.playersMain.Get_Player_Index(global::Networking.Networking.networkBytes[0], -1);
		if (num != -1)
		{
			players[num].team = global::Networking.Networking.networkUShorts[0];
			players[num].teamMask = mainC.playersMain.Get_Team_Mask(players[num].team);
			global::Networking.Networking.networkPlayers[num].haveRemotePlayerTeam = true;
			Set_Voice_For_Player((ushort)num);
		}
	}

	public void LocalPlayer_Team_Change()
	{
		players[0].teamMask = mainC.playersMain.Get_Team_Mask(players[0].team);
		enemyTeamMask = ~players[0].teamMask;
		if (global::Networking.Networking.networkSessionReady)
		{
			Set_Voice_For_All_Remote_Players();
			Send_Team_Information();
		}
		global::Networking.Networking.networkPlayers[0].haveRemotePlayerTeam = true;
	}

	public void Team_Change_Request(int account, NetworkGamer remoteGamer)
	{
		short num = Get_Player_Index(account, -1);
		if (num <= -1)
		{
			return;
		}
		switch (global::MainGame.MainGame.gameType)
		{
		case 0:
		{
			ushort num2 = Assign_Team(players[num].team);
			if (num2 != players[num].team)
			{
				players[num].teamMask = Get_Team_Mask(players[num].team);
				if (global::MainGame.MainGame.commanderMode && global::MainGame.MainGame.Commander[players[num].team] == num)
				{
					global::MainGame.MainGame.Commander[players[num].team] = -1;
					Setup_Players_For_Commander_Mode();
				}
				global::Networking.Networking.networkBytes[0] = (byte)num2;
				mainC.networkingMain.XBOX_Send_Network_Message_To_Gamer(11, remoteGamer);
			}
			global::Networking.Networking.networkPlayers[num].haveRemotePlayerTeam = true;
			break;
		}
		case 1:
			if (global::MainGame.MainGame.Guards[num] == 1)
			{
				global::MainGame.MainGame.Guards[num] = 2;
			}
			else
			{
				global::MainGame.MainGame.Guards[num] = 1;
			}
			mainC.gameLogic.Game_Send_GameSettings(2);
			break;
		}
	}

	public void Host_Assigns_Team_To_New_Remote_Player(NetworkGamer newGamer)
	{
		ushort num = (ushort)Get_Player_Index(newGamer.Id, -1);
		if (num >= 0)
		{
			byte gameType = global::MainGame.MainGame.gameType;
			if (gameType == 0 || gameType == 7)
			{
				players[num].team = Assign_Team(-1);
				players[num].teamMask = Get_Team_Mask(players[num].team);
			}
			else
			{
				players[num].team = 0;
				players[num].teamMask = Get_Team_Mask(players[num].team);
				global::Networking.Networking.networkSession.LocalGamers[global::MainGame.MainGame.localNetworkGamerID].EnableSendVoice(newGamer, enable: true);
			}
			global::Networking.Networking.networkPlayers[num].haveRemotePlayerTeam = true;
			Verify_Player_Team_Counts();
			if (global::MainGame.MainGame.commanderMode)
			{
				Setup_Players_For_Commander_Mode();
			}
		}
	}

	public void Set_Voice_For_All_Remote_Players()
	{
		int count = global::Networking.Networking.networkSession.RemoteGamers.Count;
		for (int i = 0; i < count; i++)
		{
			int num = mainC.playersMain.Get_Player_Index(global::Networking.Networking.networkSession.RemoteGamers[i].Id, -1);
			if (num > -1)
			{
				Set_Voice_For_Player((ushort)num);
			}
		}
	}

	public void Set_Voice_For_Player(ushort playerID)
	{
		int num = Get_RemoteGamer_Index((byte)players[playerID].id, -1);
		if (num < 0)
		{
			return;
		}
		switch (global::MainGame.MainGame.gameType)
		{
		case 0:
			if (!global::Networking.Networking.networkPlayers[playerID].haveRemotePlayerTeam || (players[playerID].teamMask & enemyTeamMask) != 0)
			{
				global::Networking.Networking.networkSession.LocalGamers[global::MainGame.MainGame.localNetworkGamerID].EnableSendVoice(global::Networking.Networking.networkSession.RemoteGamers[num], enable: false);
			}
			else
			{
				global::Networking.Networking.networkSession.LocalGamers[global::MainGame.MainGame.localNetworkGamerID].EnableSendVoice(global::Networking.Networking.networkSession.RemoteGamers[num], enable: true);
			}
			return;
		case 1:
			if (!global::Networking.Networking.networkPlayers[playerID].haveRemotePlayerTeam || (players[playerID].teamMask & enemyTeamMask) != 0)
			{
				global::Networking.Networking.networkSession.LocalGamers[global::MainGame.MainGame.localNetworkGamerID].EnableSendVoice(global::Networking.Networking.networkSession.RemoteGamers[num], enable: false);
			}
			else
			{
				global::Networking.Networking.networkSession.LocalGamers[global::MainGame.MainGame.localNetworkGamerID].EnableSendVoice(global::Networking.Networking.networkSession.RemoteGamers[num], enable: true);
			}
			Setup_Players_For_PrisonBreak();
			return;
		}
		ushort num2 = 0;
		int count = global::Networking.Networking.networkSession.RemoteGamers.Count;
		int i;
		for (i = 0; i < count; i++)
		{
			if (num2 >= 7)
			{
				break;
			}
			short num3 = mainC.playersMain.Get_Player_Index(global::Networking.Networking.networkSession.RemoteGamers[i].Id, -1);
			if (num3 > 0)
			{
				num2++;
				global::Networking.Networking.networkSession.LocalGamers[global::MainGame.MainGame.localNetworkGamerID].EnableSendVoice(global::Networking.Networking.networkSession.RemoteGamers[i], enable: true);
			}
		}
		for (; i < count; i++)
		{
			short num3 = mainC.playersMain.Get_Player_Index(global::Networking.Networking.networkSession.RemoteGamers[i].Id, -1);
			if (num3 > 0)
			{
				global::Networking.Networking.networkSession.LocalGamers[global::MainGame.MainGame.localNetworkGamerID].EnableSendVoice(global::Networking.Networking.networkSession.RemoteGamers[i], enable: false);
			}
		}
		i = count - 1;
		while (i > -1 && num2 < 7)
		{
			short num3 = mainC.playersMain.Get_Player_Index(global::Networking.Networking.networkSession.RemoteGamers[i].Id, -1);
			if (num3 > 0)
			{
				num2++;
				global::Networking.Networking.networkSession.LocalGamers[global::MainGame.MainGame.localNetworkGamerID].EnableSendVoice(global::Networking.Networking.networkSession.RemoteGamers[i], enable: true);
			}
			i--;
		}
	}

	public void Rank_Online_Players_By_Score()
	{
		try
		{
			byte uBufferID = global::Rendering.Rendering.uBufferID;
			playerRankingsTimer -= global::MainGame.MainGame.frametime;
			ushort num;
			if (playerRankingsTimer > 0f)
			{
				byte rBufferID = global::Rendering.Rendering.rBufferID;
				numRankedPlayers[uBufferID] = numRankedPlayers[rBufferID];
				for (num = 0; num < numRankedPlayers[uBufferID]; num++)
				{
					playerRankings[num, uBufferID] = playerRankings[num, rBufferID];
				}
				return;
			}
			playerRankingsTimer += 1f;
			num = 0;
			numRankedPlayers[uBufferID] = 0;
			while (num < global::MainGame.MainGame.maxHumanGamePlayers)
			{
				if (players[num].id > -1 && mainC.playersMain.Get_Player_Index(players[num].id, -1) > -1)
				{
					playerRankings[numRankedPlayers[uBufferID]++, uBufferID] = num;
				}
				num++;
			}
			bool flag = false;
			while (!flag)
			{
				flag = true;
				num = 0;
				for (ushort num2 = 1; num2 < numRankedPlayers[uBufferID]; num2++)
				{
					if (global::MainGame.MainGame.gameData.players[playerRankings[num, uBufferID]].scoresI[0] < global::MainGame.MainGame.gameData.players[playerRankings[num2, uBufferID]].scoresI[0])
					{
						ushort num3 = playerRankings[num, uBufferID];
						playerRankings[num, uBufferID] = playerRankings[num2, uBufferID];
						playerRankings[num2, uBufferID] = num3;
						flag = false;
					}
					num++;
				}
			}
		}
		catch (Exception)
		{
			numRankedPlayers[global::Rendering.Rendering.uBufferID] = 0;
			global::InputHandler.InputHandler.tw = 5f;
		}
	}

	public bool Check_For_Rank_Up()
	{
		if (global::MainGame.MainGame.gameMode == 0)
		{
			if (playerRankSP >= playerRankMax)
			{
				return false;
			}
			if (playerRankPointsSP + global::MainGame.MainGame.gameData.players[0].scoresI[0] >= rankUpPointLevels[playerRankSP])
			{
				if (global::MainGame.MainGame.trialMode && playerRankSP > 2)
				{
					playerRankSP = 3;
					currentPlayerRank = playerRankSP;
					return false;
				}
				playerRankSP++;
				currentPlayerRank = playerRankSP;
				return true;
			}
		}
		else if (playerRankMP < playerRankMax && playerRankPointsMP + global::MainGame.MainGame.gameData.players[0].scoresI[0] >= rankUpPointLevels[playerRankMP])
		{
			playerRankMP++;
			currentPlayerRank = playerRankMP;
			remotePlayerRanks[0] = currentPlayerRank;
			Send_Player_Rank();
			return true;
		}
		return false;
	}

	public void Player_Stops_Viewing_Device()
	{
		global::Rendering.Rendering.moveViewToNewLocation = true;
		controlsInUse = false;
		playerViewingDevice = false;
		currentView = lastView;
		mainC.weaponsMain.Check_Weapon_Views();
	}

	public void Player_Starts_Viewing_Device()
	{
		controlsInUse = true;
		playerViewingDevice = true;
		if (global::MainGame.MainGame.usingIronSights || global::MainGame.MainGame.usingScope)
		{
			mainC.weaponsMain.Stop_Using_Iron_Sights_Or_Weapon_Scope();
		}
		global::Weapons.Weapons.weaponViewEnabled = false;
		global::Weapons.Weapons.scopeViewEnabled = false;
	}

	public void Set_Player_Abbreviated_Name(ushort playerID)
	{
		players[playerID].abreviateName = players[playerID].username;
	}

	public void Activate_All_Remote_Players_After_Load()
	{
		for (ushort num = 1; num < 4; num++)
		{
			if (players[num].id < 0 || !global::Networking.Networking.networkPlayers[num].playerLoaded)
			{
				players[num].active = false;
			}
			else
			{
				players[num].active = true;
			}
		}
	}

	public void Reset_Player(ushort playerID, bool isActive, byte race, byte pType)
	{
		global::MainGame.MainGame.angularVelocity[playerID] = 0f;
		global::MainGame.MainGame.arcadeModeRisingAngle[playerID] = 0f;
		global::MainGame.MainGame.arcadeModeRotAngle[playerID] = 0f;
		global::MainGame.MainGame.gearDown[playerID] = 1;
		Adjust_Player_Damage_To_Zero(playerID, sendOnline: false);
		players[playerID].taunting = false;
		players[playerID].invincibleTimer = 0f;
		players[playerID].invincible = false;
		players[playerID].aiID = byte.MaxValue;
		players[playerID].active = isActive;
		players[playerID].charMain.bbDirty = true;
		players[playerID].commanderTargeted = false;
		players[playerID].dead = true;
		players[playerID].falling = false;
		players[playerID].inRecoil = 0;
		players[playerID].lastParticleCount = 0;
		players[playerID].objectivePoints = 0;
		players[playerID].onmap = 1;
		players[playerID].playerIsMoving = 0;
		players[playerID].renderWeapon = 0;
		players[playerID].roundPts = 0;
		players[playerID].shotImpulse = 0f;
		players[playerID].shotTorque = 0f;
		players[playerID].shooting = false;
		players[playerID].shotOnce = 0;
		players[playerID].xRotation = 0f;
		players[playerID].zRotation = 0f;
		players[playerID].deathTimer = 0f;
		players[playerID].curBulletHit = 0;
		players[playerID].voiceCueID = -1;
		players[playerID].shootingAccuracy = 1f;
		players[playerID].usingTracers = false;
		players[playerID].velX = 0f;
		players[playerID].velY = 0f;
		players[playerID].velZ = 0f;
		players[playerID].charP.fx = 0f;
		players[playerID].charP.fy = 0f;
		players[playerID].charP.fz = 0f;
		players[playerID].charP.rx = 0f;
		players[playerID].charP.ry = 0f;
		players[playerID].charP.rz = 0f;
		players[playerID].charP.position.v[0] = 0f;
		players[playerID].charP.position.v[1] = 0f;
		players[playerID].charP.position.v[2] = 0f;
		players[playerID].charP.velocity.v[0] = 0f;
		players[playerID].charP.velocity.v[1] = 0f;
		players[playerID].charP.velocity.v[2] = 0f;
		players[playerID].charP.angularVelocity.v[0] = 0f;
		players[playerID].charP.angularVelocity.v[1] = 0f;
		players[playerID].charP.angularVelocity.v[2] = 0f;
		players[playerID].charP.acceleration.v[0] = 0f;
		players[playerID].charP.acceleration.v[1] = 0f;
		players[playerID].charP.acceleration.v[2] = -32.15223f;
		players[playerID].charP.angularAcceleration.v[0] = 0f;
		players[playerID].charP.angularAcceleration.v[1] = 0f;
		players[playerID].charP.angularAcceleration.v[2] = 0f;
		players[playerID].charP.initialTime = 0.0;
		ref Matrix reference = ref players[playerID].mv[0];
		reference = Matrix.Identity;
		ref Matrix reference2 = ref players[playerID].mv[1];
		reference2 = Matrix.Identity;
		int i;
		for (i = 0; i < 10; i++)
		{
			players[playerID].particles[i] = -1;
		}
		i = players[playerID].transportParticle;
		if (i > -1 && global::Rendering.Rendering.particles[0, i].type == 8 && global::Rendering.Rendering.particles[0, i].refID == playerID)
		{
			global::Rendering.Rendering.particles[0, i].lifeTime = -1f;
		}
		if (i > -1 && global::Rendering.Rendering.particles[1, i].type == 8 && global::Rendering.Rendering.particles[1, i].refID == playerID)
		{
			global::Rendering.Rendering.particles[1, i].lifeTime = -1f;
		}
		players[playerID].transportParticle = -1;
		players[playerID].respawnParticle = -1;
		if (playerID < 4)
		{
			mpData[playerID].dataThisRound = false;
			mpData[playerID].delayedPointsSend = false;
			mpData[playerID].lastUpdate = -1L;
			mpData[playerID].rotVelX = (mpData[playerID].rotVelZ = (mpData[playerID].velX = (mpData[playerID].velY = (mpData[playerID].velZ = 0f))));
			mpData[playerID].currentPosX = players[playerID].charP.position.v[0];
			mpData[playerID].currentPosY = players[playerID].charP.position.v[1];
			mpData[playerID].currentPosZ = players[playerID].charP.position.v[2];
			mpData[playerID].specialData = 0;
			mpData[playerID].mv = Matrix.Identity;
		}
		Set_Player_Race((byte)playerID, race, (sbyte)pType);
		if (Vehicles.vehicles[players[0].curVehicle].type == 0)
		{
			if (players[playerID].programStationaryArms < global::Programs.Programs.pgC[players[playerID].programCollection].numPrograms)
			{
				mainC.programsMain.Start_Animation(playerID, ref players[playerID].jt1, ref players[playerID].animations, players[playerID].programCollection, players[playerID].programStationaryArms, 1f, 1f);
			}
			if (players[playerID].programStationaryLegsBody < global::Programs.Programs.pgC[players[playerID].programCollection].numPrograms)
			{
				mainC.programsMain.Start_Animation(playerID, ref players[playerID].jt1, ref players[playerID].animations, players[playerID].programCollection, players[playerID].programStationaryLegsBody, 1f, 1f);
			}
			mainC.jointsMain.Update_Joints_For_New_Position((short)playerID);
		}
		mainC.vehicles.Reset_Player_Vehicle_Variables(playerID);
		mainC.vehicles.Set_Vehicle_Position(ref global::MainGame.MainGame.playerVehicles[playerID], players[playerID].charP.position.v[0], players[playerID].charP.position.v[1], players[playerID].charP.position.v[2], 0f, 0f, 0f);
		mainC.weaponsMain.firingStoppedAllPlayerWeapons(playerID);
		mainC.gameLogic.Game_Reset_Player_Score(playerID);
		mainC.avatarMain.Reset_Player(playerID);
	}

	public void Reset_Multiplayer_Player_Info()
	{
		int num = 0;
		for (num = 0; num < 1; num++)
		{
			remotePlayerPositions[num] = -1;
			global::Networking.Networking.networkPlayers[num].haveRemotePlayerTeam = false;
			global::Networking.Networking.networkPlayers[num].haveRemotePlayerStatus = false;
			global::Networking.Networking.networkPlayers[num].gamerPicture = global::Textures.Textures.texMain.texData[global::Textures.Textures.texDefaultGamerPicture];
			global::Networking.Networking.networkPlayers[num].haveAllRemotePlayerDataForStart = false;
			global::Networking.Networking.networkPlayers[num].playerLoaded = false;
			global::Networking.Networking.networkPlayers[num].haveRemotePlayerArrayPosition = false;
		}
		for (num = 1; num < 4; num++)
		{
			players[num].active = false;
			players[num].id = -128;
			players[num].username = "";
			players[num].abreviateName = "";
			players[num].team = 0;
			players[num].teamMask = Get_Team_Mask(0);
			remotePlayerRanks[num] = 0;
			remotePlayerPositions[num] = -1;
			global::Networking.Networking.networkPlayers[num].haveRemotePlayerTeam = false;
			global::Networking.Networking.networkPlayers[num].playerArrayPosition = -1;
			global::Networking.Networking.networkPlayers[num].haveRemotePlayerStatus = false;
			global::Networking.Networking.networkPlayers[num].gamerPicture = global::Textures.Textures.texMain.texData[global::Textures.Textures.texDefaultGamerPicture];
			global::Networking.Networking.networkPlayers[num].haveAllRemotePlayerDataForStart = false;
			global::Networking.Networking.networkPlayers[num].playerLoaded = false;
			global::Networking.Networking.networkPlayers[num].haveRemotePlayerArrayPosition = false;
		}
	}

	public void Set_Player_Textures()
	{
		for (short num = 0; num < numPlayerRaces; num++)
		{
			for (short num2 = 0; num2 < playerRaces[num].numTypes; num2++)
			{
				playerRaces[num].miniMapIconID[num2] = (ushort)mainC.texturesMain.Find_Texture(playerRaces[num].miniMapIcon[num2], 0);
			}
		}
	}

	public void Clear_Players()
	{
		int num = 0;
		for (ushort num2 = 0; num2 < 44; num2++)
		{
			Adjust_Player_Damage_To_Zero(num2, sendOnline: false);
			players[num2].active = false;
			players[num2].onmap = 1;
			players[num2].aiID = byte.MaxValue;
			players[num2].username = "";
			players[num2].abreviateName = "";
			players[num2].id = -128;
			players[num2].team = 0;
			players[num2].teamMask = Get_Team_Mask(0);
			players[num2].dead = true;
			players[num2].transportParticle = -1;
			players[num2].renderWeapon = 0;
			players[num2].shooting = false;
			players[num2].shotOnce = 0;
			players[num2].roundPts = 0;
			players[num2].objectivePoints = 0;
			players[num2].voiceCueID = -1;
			for (num = 0; num < 10; num++)
			{
				players[num2].particles[num] = -1;
			}
		}
		Reset_Team_Counts_To_Zero();
		mainC.inputMain.UI_Remove_All_Players_From_HUD();
		players[0].active = true;
		players[0].team = mainC.playersMain.Assign_Team(-1);
		LocalPlayer_Team_Change();
	}

	public void Confine_Player_Position_ToBoundaries(short playerID, bool postCollision, byte threadID)
	{
		float num = mainC.terrainMain.Get_Terrain_Height(players[playerID].charP.position.v[0], players[playerID].charP.position.v[1], threadID);
		if (num < global::MainGame.MainGame.MaxDown)
		{
			num = global::MainGame.MainGame.MaxDown;
		}
		float num2;
		if ((num2 = players[playerID].charMain.b2.v[0] - global::MainGame.MainGame.MaxRight) > 0f)
		{
			players[playerID].charP.position.v[0] -= num2;
			players[playerID].charMain.b1.v[0] -= num2;
			players[playerID].charMain.b2.v[0] -= num2;
		}
		else if ((num2 = global::MainGame.MainGame.MaxLeft - players[playerID].charMain.b1.v[0]) > 0f)
		{
			players[playerID].charP.position.v[0] += num2;
			players[playerID].charMain.b1.v[0] += num2;
			players[playerID].charMain.b2.v[0] += num2;
		}
		if ((num2 = players[playerID].charMain.b2.v[1] - global::MainGame.MainGame.MaxForward) > 0f)
		{
			players[playerID].charP.position.v[1] -= num2;
			players[playerID].charMain.b1.v[1] -= num2;
			players[playerID].charMain.b2.v[1] -= num2;
		}
		else if ((num2 = global::MainGame.MainGame.MaxRear - players[playerID].charMain.b1.v[1]) > 0f)
		{
			players[playerID].charP.position.v[1] += num2;
			players[playerID].charMain.b1.v[1] += num2;
			players[playerID].charMain.b2.v[1] += num2;
		}
		if ((num2 = players[playerID].charMain.b2.v[2] - global::MainGame.MainGame.MaxUp) > 0f)
		{
			players[playerID].charP.position.v[2] -= num2;
			players[playerID].charMain.b1.v[2] -= num2;
			players[playerID].charMain.b2.v[2] -= num2;
		}
		else if ((num2 = num - players[playerID].charMain.b1.v[2]) > 0f)
		{
			if (!postCollision)
			{
				num2 -= 1f;
			}
			players[playerID].charP.position.v[2] += num2;
			players[playerID].charMain.b1.v[2] += num2;
			players[playerID].charMain.b2.v[2] += num2;
		}
	}

	public byte Confine_Player_Position_ToBoundaries_New(ref StructsClass.physics_new ph1, float terrainHeight, short playerID)
	{
		byte b = 0;
		if (terrainHeight < global::MainGame.MainGame.MaxDown)
		{
			terrainHeight = global::MainGame.MainGame.MaxDown;
		}
		float num;
		if ((num = players[playerID].charMain.b2.v[0] - global::MainGame.MainGame.MaxRight) > 0f)
		{
			ph1.x -= num;
			ph1.velocityX = 0f;
			players[playerID].charMain.b1.v[0] -= num;
			players[playerID].charMain.b2.v[0] -= num;
			b = 1;
		}
		else if ((num = global::MainGame.MainGame.MaxLeft - players[playerID].charMain.b1.v[0]) > 0f)
		{
			ph1.x += num;
			ph1.velocityX = 0f;
			players[playerID].charMain.b1.v[0] += num;
			players[playerID].charMain.b2.v[0] += num;
			b = 2;
		}
		if ((num = players[playerID].charMain.b2.v[1] - global::MainGame.MainGame.MaxForward) > 0f)
		{
			ph1.y -= num;
			ph1.velocityY = 0f;
			players[playerID].charMain.b1.v[1] -= num;
			players[playerID].charMain.b2.v[1] -= num;
			b |= 4;
		}
		else if ((num = global::MainGame.MainGame.MaxRear - players[playerID].charMain.b1.v[1]) > 0f)
		{
			ph1.y += num;
			ph1.velocityY = 0f;
			players[playerID].charMain.b1.v[1] += num;
			players[playerID].charMain.b2.v[1] += num;
			b |= 8;
		}
		if ((num = players[playerID].charMain.b2.v[2] - global::MainGame.MainGame.MaxUp) > 0f)
		{
			ph1.z -= num;
			ph1.velocityZ = 0f;
			players[playerID].charMain.b1.v[2] -= num;
			players[playerID].charMain.b2.v[2] -= num;
			b |= 0x10;
		}
		else if ((num = terrainHeight - players[playerID].charMain.b1.v[2]) > 0f)
		{
			ph1.z += num;
			ph1.velocityZ = 0f;
			players[playerID].charMain.b1.v[2] += num;
			players[playerID].charMain.b2.v[2] += num;
			b |= 0x20;
		}
		return b;
	}

	public int Find_Vacant_Player(ushort startingIndex)
	{
		for (ushort num = startingIndex; num < global::MainGame.MainGame.maxGamePlayers; num++)
		{
			if (!players[num].active && players[num].id < 0)
			{
				return num;
			}
		}
		return -1;
	}

	public short Get_Player_Index(int account, short defaultVal)
	{
		for (short num = 0; num < 4; num++)
		{
			if (players[num].id == account)
			{
				return num;
			}
		}
		return defaultVal;
	}

	public int Get_RemoteGamer_Index(byte account, int defaultVal)
	{
		int count = global::Networking.Networking.networkSession.RemoteGamers.Count;
		for (int i = 0; i < count; i++)
		{
			if (global::Networking.Networking.networkSession.RemoteGamers[i].Id == account)
			{
				return i;
			}
		}
		return defaultVal;
	}

	public void Update_Player_BoundingBox(int playerID, float x, float y, float z, byte threadID)
	{
		int race = players[playerID].race;
		int type = players[playerID].type;
		players[playerID].charMain.b1.v[0] = x + playerRaces[race].bBox[type, 0];
		players[playerID].charMain.b1.v[1] = y + playerRaces[race].bBox[type, 1];
		players[playerID].charMain.b1.v[2] = z + playerRaces[race].bBox[type, 2];
		players[playerID].charMain.b2.v[0] = x + playerRaces[race].bBox[type, 3];
		players[playerID].charMain.b2.v[1] = y + playerRaces[race].bBox[type, 4];
		players[playerID].charMain.b2.v[2] = z + playerRaces[race].bBox[type, 5];
	}

	public void Update_Player_Vehicle_BoundingBox(int playerID, byte threadID, ushort numUsed, ref Matrix mvR)
	{
		long num = 0L;
		ref StructsClass.particle_list reference = ref rpoP1[threadID];
		reference = players[playerID].charMain;
		float num2 = global::MainGame.MainGame.playerVehicles[playerID].momentum.collisionPoints[0];
		float num3 = global::MainGame.MainGame.playerVehicles[playerID].momentum.collisionPoints[1];
		float num4 = global::MainGame.MainGame.playerVehicles[playerID].momentum.collisionPoints[2];
		float num5 = num2 * mvR.M11 + num3 * mvR.M21 + num4 * mvR.M31;
		float num6 = num2 * mvR.M12 + num3 * mvR.M22 + num4 * mvR.M32;
		float num7 = num2 * mvR.M13 + num3 * mvR.M23 + num4 * mvR.M33;
		rpoP1[threadID].b1.v[0] = num5;
		rpoP1[threadID].b1.v[1] = num6;
		rpoP1[threadID].b1.v[2] = num7;
		rpoP1[threadID].b2.v[0] = num5;
		rpoP1[threadID].b2.v[1] = num6;
		rpoP1[threadID].b2.v[2] = num7;
		num = 1L;
		long num8 = 3L;
		for (; num < numUsed; num++)
		{
			num2 = global::MainGame.MainGame.playerVehicles[playerID].momentum.collisionPoints[num8++];
			num3 = global::MainGame.MainGame.playerVehicles[playerID].momentum.collisionPoints[num8++];
			num4 = global::MainGame.MainGame.playerVehicles[playerID].momentum.collisionPoints[num8++];
			num5 = num2 * mvR.M11 + num3 * mvR.M21 + num4 * mvR.M31;
			num6 = num2 * mvR.M12 + num3 * mvR.M22 + num4 * mvR.M32;
			num7 = num2 * mvR.M13 + num3 * mvR.M23 + num4 * mvR.M33;
			if (num5 < rpoP1[threadID].b1.v[0])
			{
				rpoP1[threadID].b1.v[0] = num5;
			}
			if (num5 > rpoP1[threadID].b2.v[0])
			{
				rpoP1[threadID].b2.v[0] = num5;
			}
			if (num6 < rpoP1[threadID].b1.v[1])
			{
				rpoP1[threadID].b1.v[1] = num6;
			}
			if (num6 > rpoP1[threadID].b2.v[1])
			{
				rpoP1[threadID].b2.v[1] = num6;
			}
			if (num7 < rpoP1[threadID].b1.v[2])
			{
				rpoP1[threadID].b1.v[2] = num7;
			}
			if (num7 > rpoP1[threadID].b2.v[2])
			{
				rpoP1[threadID].b2.v[2] = num7;
			}
		}
		players[playerID].charMain.b1.v[0] += players[playerID].charP.position.v[0];
		players[playerID].charMain.b1.v[1] += players[playerID].charP.position.v[1];
		players[playerID].charMain.b1.v[2] += players[playerID].charP.position.v[2];
		players[playerID].charMain.b2.v[0] += players[playerID].charP.position.v[0];
		players[playerID].charMain.b2.v[1] += players[playerID].charP.position.v[1];
		players[playerID].charMain.b2.v[2] += players[playerID].charP.position.v[2];
		rpoP1[threadID].bbDirty = false;
	}

	public void Get_Player_PrevParticles_BoundingBox(int playerID, byte threadID, ref StructsClass.vtex b1, ref StructsClass.vtex b2)
	{
		b1.v[0] = players[playerID].particlePrev[0].v[0];
		b1.v[1] = players[playerID].particlePrev[0].v[1];
		b1.v[2] = players[playerID].particlePrev[0].v[2];
		b2.v[0] = players[playerID].particlePrev[0].v[0];
		b2.v[1] = players[playerID].particlePrev[0].v[1];
		b2.v[2] = players[playerID].particlePrev[0].v[2];
		for (long num = 1L; num < players[playerID].charMain.numUsed; num++)
		{
			if (players[playerID].particlePrev[num].v[0] < b1.v[0])
			{
				b1.v[0] = players[playerID].particlePrev[num].v[0];
			}
			if (players[playerID].particlePrev[num].v[0] > b2.v[0])
			{
				b2.v[0] = players[playerID].particlePrev[num].v[0];
			}
			if (players[playerID].particlePrev[num].v[1] < b1.v[1])
			{
				b1.v[1] = players[playerID].particlePrev[num].v[1];
			}
			if (players[playerID].particlePrev[num].v[1] > b2.v[1])
			{
				b2.v[1] = players[playerID].particlePrev[num].v[1];
			}
			if (players[playerID].particlePrev[num].v[2] < b1.v[2])
			{
				b1.v[2] = players[playerID].particlePrev[num].v[2];
			}
			if (players[playerID].particlePrev[num].v[2] > b2.v[2])
			{
				b2.v[2] = players[playerID].particlePrev[num].v[2];
			}
		}
		b1.v[0] += players[playerID].charP.position.v[0];
		b1.v[1] += players[playerID].charP.position.v[1];
		b1.v[2] += players[playerID].charP.position.v[2];
		b2.v[0] += players[playerID].charP.position.v[0];
		b2.v[1] += players[playerID].charP.position.v[1];
		b2.v[2] += players[playerID].charP.position.v[2];
	}

	public static short Find_Next_Player_Not_On_This_Team(short startID, ushort teamID)
	{
		for (short num = startID; num < global::MainGame.MainGame.maxGamePlayers; num++)
		{
			if (players[num].active && (players[num].onmap & 4) > 0 && players[num].team != teamID)
			{
				return num;
			}
		}
		return -1;
	}

	public static short Find_Next_Team_Player(short startID, ulong teamMask)
	{
		for (short num = startID; num < global::MainGame.MainGame.maxGamePlayers; num++)
		{
			if (players[num].active && (players[num].onmap & 4) > 0 && (players[num].team & teamMask) == 0)
			{
				return num;
			}
		}
		return -1;
	}

	public static short Find_Previous_Player(short startID, byte teamID)
	{
		for (short num = startID; num > 0; num--)
		{
			if (players[num].active && (players[num].onmap & 6) > 0 && players[num].team != teamID)
			{
				return num;
			}
		}
		return -1;
	}

	public static short Find_Previous_Team_Player(short startID, ulong teamMask)
	{
		for (short num = startID; num > 0; num--)
		{
			if (players[num].active && (players[num].onmap & 6) > 0 && (players[num].team & teamMask) == 0)
			{
				return num;
			}
		}
		return -1;
	}

	public static float Find_Player_Within_Distance(short startID, ushort team, float maxDistanceSqr, float x, float y, float z)
	{
		for (short num = startID; num < global::MainGame.MainGame.maxGamePlayers; num++)
		{
			if (players[num].active && (players[num].onmap & 6) > 0 && players[num].team != team)
			{
				float num2 = x - players[num].charP.position.v[0];
				float num3 = y - players[num].charP.position.v[1];
				float num4 = z - players[num].charP.position.v[2];
				num2 = num2 * num2 + num3 * num3 + num4 * num4;
				if (num2 < maxDistanceSqr)
				{
					return num2;
				}
			}
		}
		return maxDistanceSqr;
	}

	public int Find_Closest_Player(ushort startID, ushort endID, ushort ignoreID, ushort team, float x, float y, float z, int defaultPlayer)
	{
		float num = global::Collision.Collision.maxDistanceSqr;
		for (ushort num2 = startID; num2 <= endID; num2++)
		{
			if (players[num2].active && (players[num2].onmap & 6) > 0 && players[num2].team == team && num2 != ignoreID)
			{
				float num3 = x - players[num2].charP.position.v[0];
				float num4 = y - players[num2].charP.position.v[1];
				float num5 = z - players[num2].charP.position.v[2];
				num3 = num3 * num3 + num4 * num4 + num5 * num5;
				if (num3 < num)
				{
					defaultPlayer = num2;
					num = num3;
				}
			}
		}
		return defaultPlayer;
	}

	public void Remove_Remote_Player_From_Game(short playerID)
	{
		if (playerID < 0 || playerID >= 4)
		{
			return;
		}
		if (global::Networking.Networking.networkPlayers[playerID].playerArrayPosition > -1)
		{
			remotePlayerPositions[global::Networking.Networking.networkPlayers[playerID].playerArrayPosition] = -1;
		}
		global::Networking.Networking.networkPlayers[playerID].playerLoaded = false;
		global::Networking.Networking.networkPlayers[playerID].gamerPicture = global::Textures.Textures.texMain.texData[global::Textures.Textures.texDefaultGamerPicture];
		global::Networking.Networking.networkPlayers[playerID].playerArrayPosition = -1;
		global::Networking.Networking.networkPlayers[playerID].haveRemotePlayerTeam = false;
		global::Networking.Networking.networkPlayers[playerID].haveRemotePlayerArrayPosition = false;
		global::Networking.Networking.networkPlayers[playerID].haveAllRemotePlayerDataForStart = false;
		remotePlayerRanks[playerID] = 0;
		if (global::Rendering.Rendering.watchingPlayer == playerID)
		{
			global::Rendering.Rendering.watchingPlayer = 0;
		}
		players[playerID].active = false;
		players[playerID].shooting = false;
		players[playerID].shotOnce = 0;
		players[playerID].onmap = 1;
		players[playerID].username = "";
		players[playerID].abreviateName = "";
		players[playerID].id = -128;
		players[playerID].team = 0;
		players[playerID].teamMask = Get_Team_Mask(0);
		players[playerID].transportParticle = -1;
		mainC.weaponsMain.firingStoppedAllPlayerWeapons((ushort)playerID);
		Verify_Player_Team_Counts();
		if (global::Networking.Networking.isHost)
		{
			if (global::MainGame.MainGame.commanderMode)
			{
				Setup_Players_For_Commander_Mode();
			}
			byte gameType = global::MainGame.MainGame.gameType;
			if (gameType == 1)
			{
				Setup_Players_For_PrisonBreak();
			}
		}
		mainC.gameLogic.Game_Remove_Player_From_Game((ushort)playerID);
	}

	public void Set_All_Player_Races_For_MP(byte threadID, byte race1, byte race2)
	{
		byte maxHumanGamePlayers = global::MainGame.MainGame.maxHumanGamePlayers;
		for (byte b = 0; b < maxHumanGamePlayers; b++)
		{
			Set_Player_Race(b, players[b].race, players[b].type);
		}
	}

	public void XBOX_Update_Local_Player_GamerTag(short account, string userName)
	{
		players[0].id = account;
		players[0].username = userName;
		Set_Player_Abbreviated_Name(0);
	}

	public void Setup_Players_For_Commander_Mode()
	{
		for (ushort num = 0; num < global::MainGame.MainGame.numTeams; num++)
		{
			if (global::MainGame.MainGame.Commander[num] > -1 && (global::MainGame.MainGame.Commander[num] >= 44 || !players[global::MainGame.MainGame.Commander[num]].active || pluTeam[num] < 2))
			{
				global::MainGame.MainGame.Commander[num] = -1;
				global::Networking.Networking.networkInts[0] = -1;
				if (pluTeam[num] > 1)
				{
					for (byte b = 0; b < global::MainGame.MainGame.maxGamePlayers; b++)
					{
						if (players[b].active && players[b].team == num)
						{
							global::MainGame.MainGame.Commander[num] = (sbyte)b;
							global::Networking.Networking.networkInts[0] = players[b].id;
							break;
						}
					}
				}
				mainC.gameLogic.Game_Send_GameSettings(4);
			}
		}
	}

	public void Clear_PrisonBreak_Players()
	{
		for (byte b = 0; b < 44; b++)
		{
			global::MainGame.MainGame.Guards[b] = 0;
		}
	}

	public void Clear_CommanderMode_Players()
	{
		global::MainGame.MainGame.Commander[1] = -1;
		global::MainGame.MainGame.Commander[2] = -1;
	}

	public bool Setup_Players_For_PrisonBreak()
	{
		bool flag = false;
		numGuards = 0;
		numPrisoners = 0;
		for (byte b = 0; b < 44; b++)
		{
			if (!players[b].active)
			{
				if (global::MainGame.MainGame.Guards[b] > 0)
				{
					global::MainGame.MainGame.Guards[b] = 0;
					flag = true;
				}
			}
			else if (global::MainGame.MainGame.Guards[b] < 1)
			{
				if (numGuards < 1)
				{
					players[b].team = 0;
					players[b].teamMask = Get_Team_Mask(0);
					global::MainGame.MainGame.Guards[b] = 1;
					numGuards++;
				}
				else if (numPrisoners < 1)
				{
					players[b].team = 1;
					players[b].teamMask = Get_Team_Mask(1);
					global::MainGame.MainGame.Guards[b] = 2;
					numPrisoners++;
				}
				else if (numGuards < numPrisoners * 2)
				{
					players[b].team = 0;
					players[b].teamMask = Get_Team_Mask(0);
					global::MainGame.MainGame.Guards[b] = 1;
					numGuards++;
				}
				else if (numPrisoners < numGuards / 2)
				{
					players[b].team = 1;
					players[b].teamMask = Get_Team_Mask(1);
					global::MainGame.MainGame.Guards[b] = 2;
					numPrisoners++;
				}
				else
				{
					players[b].team = 0;
					players[b].teamMask = Get_Team_Mask(0);
					global::MainGame.MainGame.Guards[b] = 1;
					numGuards++;
				}
				flag = true;
			}
			else if (global::MainGame.MainGame.Guards[b] > 1)
			{
				players[b].team = 1;
				players[b].teamMask = Get_Team_Mask(1);
				numPrisoners++;
			}
			else
			{
				players[b].team = 0;
				players[b].teamMask = Get_Team_Mask(0);
				numGuards++;
			}
		}
		byte b2 = 16;
		while (numPrisoners < numGuards / 2 && numGuards > 1 && b2 > 0)
		{
			for (byte b = 0; b < 44; b++)
			{
				if (global::MainGame.MainGame.Guards[b] == 1)
				{
					players[b].team = 1;
					players[b].teamMask = Get_Team_Mask(1);
					global::MainGame.MainGame.Guards[b] = 2;
					numPrisoners++;
					numGuards--;
					flag = true;
					break;
				}
			}
			b2--;
		}
		b2 = 16;
		while (numGuards <= numPrisoners * 2 && numPrisoners > 1 && b2 > 0)
		{
			for (byte b = 0; b < 44; b++)
			{
				if (global::MainGame.MainGame.Guards[b] == 2)
				{
					players[b].team = 0;
					players[b].teamMask = Get_Team_Mask(0);
					global::MainGame.MainGame.Guards[b] = 1;
					numPrisoners--;
					numGuards++;
					flag = true;
					break;
				}
			}
			b2--;
		}
		if (flag)
		{
			mainC.gameLogic.Game_Send_GameSettings(2);
		}
		return flag;
	}

	public void Set_Player_As_Guard(int account)
	{
		short num = Get_Player_Index(account, -1);
		if (num > -1 && num < 44)
		{
			global::MainGame.MainGame.Guards[num] = 1;
		}
	}

	public void Set_Player_As_Prisoner(int account)
	{
		short num = Get_Player_Index(account, -1);
		if (num > -1 && num < 44)
		{
			global::MainGame.MainGame.Guards[num] = 2;
		}
	}

	public bool Set_Player_Race(byte playerID, byte race, sbyte type)
	{
		bool flag = false;
		if (race >= numPlayerRaces)
		{
			race = 0;
		}
		if (type < 0 || type > playerRaces[race].numTypes)
		{
			type = 0;
		}
		if (players[playerID].race != race || players[playerID].type != type)
		{
			flag = true;
			players[playerID].race = race;
			players[playerID].type = type;
			players[playerID].projectileResistance = playerRaces[race].projectileResistance[type];
			players[playerID].renderScale = playerRaces[race].renderScale[type];
			players[playerID].torqueJoint = playerRaces[race].torqueJoint[type];
			players[playerID].humanoidBackJoint = playerRaces[race].humanoidBackJoint[type];
			players[playerID].headJoint = playerRaces[race].headJoint[type];
			players[playerID].thirdPesonJoint = playerRaces[race].thirdPesonJoint[type];
			players[playerID].eyeJoint = playerRaces[race].eyeJoint[type];
			players[playerID].shoulderJointL = playerRaces[race].shoulderJointL[type];
			players[playerID].shoulderJointR = playerRaces[race].shoulderJointR[type];
			players[playerID].weaponJoint = playerRaces[race].weaponJoint[type];
			players[playerID].damageType = playerRaces[race].damageType[type];
			players[playerID].playerSeparationDistanceSqr = playerRaces[race].playerSeparationDistance[type] * playerRaces[race].playerSeparationDistance[type];
			players[playerID].playerMeleeDistance = playerRaces[race].meleeDistance[type];
			players[playerID].playerBoudingRadius = playerRaces[race].boundingRadius[type];
			players[playerID].playerBoudingRadiusSqr = players[playerID].playerBoudingRadius * players[playerID].playerBoudingRadius;
			players[playerID].playerBoudingRadiusTimes2Sqr = players[playerID].playerBoudingRadius * players[playerID].playerBoudingRadius * 4f;
			players[playerID].velocityTerminal = playerRaces[race].velocityTerminal;
			players[playerID].velocityTerminalThreshold = playerRaces[race].velocityTerminalThreshold;
			if (players[playerID].jointPackage != playerRaces[race].jointPackage[type])
			{
				players[playerID].jointPackage = playerRaces[race].jointPackage[type];
				Init_Player_Joints(playerID, playerID + 1);
			}
			if (players[playerID].programCollection != playerRaces[race].programCollection[type])
			{
				players[playerID].programCollection = playerRaces[race].programCollection[type];
				Init_Player_Program(playerID, playerID + 1);
			}
			players[playerID].programStationaryLegsBody = playerRaces[race].programStationaryLegsBody[type];
			players[playerID].programStationaryArms = playerRaces[race].programStationaryArms[type];
			players[playerID].programSwitchWeapons = playerRaces[race].programSwitchWeapons[type];
			players[playerID].programWalk = playerRaces[race].programWalk[type];
			players[playerID].programJump = playerRaces[race].programJump[type];
			players[playerID].programTurnLeft = playerRaces[race].programTurnLeft[type];
			players[playerID].programTurnRight = playerRaces[race].programTurnRight[type];
			players[playerID].programWalkBackwards = playerRaces[race].programWalkBackwards[type];
			players[playerID].programSidestep = playerRaces[race].programSidestep[type];
			players[playerID].programRun = playerRaces[race].programRun[type];
			players[playerID].programDeath = playerRaces[race].programDeath[type, 0];
			players[playerID].programDeathBlownAway = playerRaces[race].programDeathBlownAway[type, 0];
		}
		global::Joints.Joints.Reset_Joint_Rotations_To_Zero(playerID);
		mainC.programsMain.Reset_Programs(ref players[playerID].pg1, ref players[playerID].animations, players[playerID].programCollection);
		mainC.programsMain.Set_Joints_To_Animation_Start(ref players[playerID].jt1, players[playerID].programCollection, players[playerID].programStationaryLegsBody, playerID, 1f);
		mainC.programsMain.Set_Joints_To_Animation_Start(ref players[playerID].jt1, players[playerID].programCollection, players[playerID].programStationaryArms, playerID, 1f);
		ushort num = playerRaces[players[playerID].race].vehicleID[players[playerID].type];
		players[playerID].curVehicleIndex = 0;
		players[playerID].vehicles[0] = num;
		players[playerID].curVehicle = num;
		if (flag)
		{
			mainC.vehicles.Clone_Vehicle_Data(ref global::MainGame.MainGame.playerVehicles[playerID], (byte)num);
		}
		mainC.gameLogic.Game_Set_Vehicle_Weapons(playerID);
		players[playerID].maxDamage = Vehicles.vehicles[num].maxDamage;
		mainC.soundsMain.Set_Continual_Sounds_Player_Index(playerID, playerRaces[race].soundIndex[type]);
		mainC.gameLogic.Game_Set_Avatar_Vehicle_Pose(playerID);
		ushort num2;
		for (num2 = 1; num2 < 3; num2++)
		{
			players[playerID].vehicles[num2] = global::Util.Util.maxUnsignedShortValue;
		}
		num2 = Vehicles.vehicles[players[playerID].curVehicle].numModels;
		if (num2 > 0)
		{
			players[playerID].playerModel = new short[num2];
			for (ushort num3 = 0; num3 < num2; num3++)
			{
				players[playerID].playerModel[num3] = Vehicles.vehicles[num].vehicleModel[num3];
			}
			players[playerID].textureNormalID = global::Models.Models.mod1[players[playerID].playerModel[0]].texNormalID;
			players[playerID].textureSpecularID = global::Models.Models.mod1[players[playerID].playerModel[0]].texSpecularID;
			mainC.vehicles.Create_Vehicle_Texture_List(num, out players[playerID].textureID);
		}
		return flag;
	}

	public void Set_Closest_Level_Lights_To_Player()
	{
		byte uBufferID = global::Rendering.Rendering.uBufferID;
		for (short num = 0; num < global::Rendering.Rendering.numPtLight_lvl; num++)
		{
			global::Rendering.Rendering.closestLevelLightsIndices[uBufferID, num] = -1;
		}
		for (short num = 0; num < global::Rendering.Rendering.numPtLight_lvl; num++)
		{
			float num2 = players[0].charP.position.v[0] - global::Rendering.Rendering.ptLight_lvl[num, 0];
			float num3 = players[0].charP.position.v[1] - global::Rendering.Rendering.ptLight_lvl[num, 1];
			float num4 = players[0].charP.position.v[2] - global::Rendering.Rendering.ptLight_lvl[num, 2];
			float num5 = num2 * num2 + num3 * num3 + num4 * num4;
			for (short num6 = 0; num6 < global::Rendering.Rendering.numPtLight_lvl; num6++)
			{
				if (global::Rendering.Rendering.closestLevelLightsIndices[uBufferID, num6] == -1)
				{
					lvlLightSortArray[num6] = num5;
					global::Rendering.Rendering.closestLevelLightsIndices[uBufferID, num6] = num;
					break;
				}
				if (lvlLightSortArray[num6] > num5)
				{
					for (short num7 = (short)(global::Rendering.Rendering.numPtLight_lvl - 1); num7 > num6; num7--)
					{
						lvlLightSortArray[num7] = lvlLightSortArray[num7 - 1];
						global::Rendering.Rendering.closestLevelLightsIndices[uBufferID, num7] = global::Rendering.Rendering.closestLevelLightsIndices[uBufferID, num7 - 1];
					}
					lvlLightSortArray[num6] = num5;
					global::Rendering.Rendering.closestLevelLightsIndices[uBufferID, num6] = num;
					break;
				}
			}
		}
	}

	public void Draw_Player_Scores_ToScreen(float leftX, float midX, float topY)
	{
	}

	public void Draw_Player_Info_To_Lobby_Screen(int leftX, int midX, int topY)
	{
	}

	public void Render_Main_Player_ToSetDepth(int vboType)
	{
		if (players[0].dead || players[0].onmap < 4)
		{
			return;
		}
		byte rBufferID = global::Rendering.Rendering.rBufferID;
		int numJoints = global::Joints.Joints.playerJoints[players[0].jointPackage].numJoints;
		Matrix matrix = Matrix.CreateTranslation(players[0].posX[rBufferID], players[0].posY[rBufferID], players[0].posZ[rBufferID]);
		switch (vboType)
		{
		case 3:
		{
			global::Rendering.Rendering.effect1.Parameters["World"].SetValue(matrix);
			global::Rendering.Rendering.effect1.CurrentTechnique.Passes[0].Apply();
			mainC.modelsMain.Render_Model_Basic(players[0].ct1.modID);
			for (int i = 0; i < numJoints; i++)
			{
				if (players[0].jt1[i].modID > -1)
				{
					global::Rendering.Rendering.effect1.Parameters["World"].SetValue(players[0].jt1[i].mv[rBufferID] * matrix);
					global::Rendering.Rendering.effect1.CurrentTechnique.Passes[0].Apply();
					mainC.modelsMain.Render_Model_Basic(players[0].jt1[i].modID);
				}
			}
			break;
		}
		case 7:
		{
			if (players[0].ct1.modID > -1 && global::Models.Models.mod1[players[0].ct1.modID].defaultColor[3] == 1f)
			{
				global::Rendering.Rendering.effect1.Parameters["World"].SetValue(matrix);
				global::Rendering.Rendering.effect1.CurrentTechnique.Passes[0].Apply();
				mainC.modelsMain.Render_Model_Basic(players[0].ct1.modID);
			}
			for (int i = 0; i < numJoints; i++)
			{
				if (players[0].jt1[i].modID > -1)
				{
					Matrix mv;
					if (global::Models.Models.mod1[players[0].jt1[i].modID].defaultColor[3] == 1f)
					{
						mv = players[0].jt1[i].mv[rBufferID] * matrix;
						mainC.modelsMain.Render_Model(players[0].jt1[i].modID, ref mv);
						continue;
					}
					mv = players[0].jt1[i].mv[rBufferID] * matrix;
					global::Rendering.Rendering.effect1.Parameters["AlphaAdjust"].SetValue(global::Models.Models.mod1[players[0].jt1[i].modID].defaultColor[3]);
					mainC.modelsMain.Render_Model(players[0].jt1[i].modID, ref mv);
					global::Rendering.Rendering.effect1.Parameters["AlphaAdjust"].SetValue(1f);
				}
			}
			break;
		}
		}
	}

	public void Process_Players(float frameTime, byte threadID)
	{
		float num = 0f;
		try
		{
			byte uBufferID = global::Rendering.Rendering.uBufferID;
			byte rBufferID = global::Rendering.Rendering.rBufferID;
			global::Rendering.Rendering.hitColor[3] -= 2f * frameTime;
			if (global::Rendering.Rendering.hitColor[3] < 0f)
			{
				global::Rendering.Rendering.hitColor[3] = 0f;
			}
			ushort num2 = 1;
			if (global::MainGame.MainGame.gameMode != 0)
			{
				num2 = global::MainGame.MainGame.maxHumanGamePlayers;
				bool flag = false;
				for (short num3 = 1; num3 < global::MainGame.MainGame.maxHumanGamePlayers; num3++)
				{
					if (players[num3].active)
					{
						ushort curVehicle = players[num3].curVehicle;
						if (global::Networking.Networking.isHost && mpData[num3].delayedPointsSend)
						{
							mpData[num3].delayedPointsTime -= global::MainGame.MainGame.frametime;
							if (mpData[num3].delayedPointsTime < 0f)
							{
								mpData[num3].delayedPointsSend = false;
								flag = true;
								Send_Player_Points((ushort)num3);
							}
						}
						players[num3].charP.mass = global::MainGame.MainGame.playerVehicles[players[num3].curVehicle].ph1.mass;
						if (mpData[num3].dataThisRound)
						{
							switch (global::Networking.Networking.posMessageType)
							{
							case 0:
							{
								ref Matrix reference = ref players[num3].mv[uBufferID];
								reference = Matrix.CreateRotationZ(players[num3].zRotation * ((float)Math.PI / 180f));
								break;
							}
							case 1:
								players[num3].mv[uBufferID].M11 = mpData[num3].mv.M11;
								players[num3].mv[uBufferID].M12 = mpData[num3].mv.M12;
								players[num3].mv[uBufferID].M13 = mpData[num3].mv.M13;
								players[num3].mv[uBufferID].M21 = mpData[num3].mv.M21;
								players[num3].mv[uBufferID].M22 = mpData[num3].mv.M22;
								players[num3].mv[uBufferID].M23 = mpData[num3].mv.M23;
								players[num3].mv[uBufferID].M31 = mpData[num3].mv.M31;
								players[num3].mv[uBufferID].M32 = mpData[num3].mv.M32;
								players[num3].mv[uBufferID].M33 = mpData[num3].mv.M33;
								players[num3].mv[uBufferID].M41 = 0f;
								players[num3].mv[uBufferID].M42 = 0f;
								players[num3].mv[uBufferID].M43 = 0f;
								break;
							}
						}
						else
						{
							mpData[num3].currentPosX += mpData[num3].velX * frameTime / global::Physics.Physics.timeMod;
							mpData[num3].currentPosY += mpData[num3].velY * frameTime / global::Physics.Physics.timeMod;
							mpData[num3].currentPosZ += mpData[num3].velZ * frameTime / global::Physics.Physics.timeMod;
							switch (global::Networking.Networking.posMessageType)
							{
							case 0:
							{
								ref Matrix reference2 = ref players[num3].mv[uBufferID];
								reference2 = players[num3].mv[rBufferID];
								break;
							}
							case 1:
								players[num3].mv[uBufferID].M11 = players[num3].mv[rBufferID].M11;
								players[num3].mv[uBufferID].M12 = players[num3].mv[rBufferID].M12;
								players[num3].mv[uBufferID].M13 = players[num3].mv[rBufferID].M13;
								players[num3].mv[uBufferID].M21 = players[num3].mv[rBufferID].M21;
								players[num3].mv[uBufferID].M22 = players[num3].mv[rBufferID].M22;
								players[num3].mv[uBufferID].M23 = players[num3].mv[rBufferID].M23;
								players[num3].mv[uBufferID].M31 = players[num3].mv[rBufferID].M31;
								players[num3].mv[uBufferID].M32 = players[num3].mv[rBufferID].M32;
								players[num3].mv[uBufferID].M33 = players[num3].mv[rBufferID].M33;
								players[num3].mv[uBufferID].M41 = 0f;
								players[num3].mv[uBufferID].M42 = 0f;
								players[num3].mv[uBufferID].M43 = 0f;
								break;
							}
						}
						mpData[num3].springX = mpData[num3].currentPosX - players[num3].charP.position.v[0];
						mpData[num3].springY = mpData[num3].currentPosY - players[num3].charP.position.v[1];
						mpData[num3].springZ = mpData[num3].currentPosZ - players[num3].charP.position.v[2];
						players[num3].charP.fx = players[num3].charP.mass * (mpData[num3].springX * 48f - players[num3].charP.velocity.v[0] * 10f);
						players[num3].charP.fy = players[num3].charP.mass * (mpData[num3].springY * 48f - players[num3].charP.velocity.v[1] * 10f);
						players[num3].charP.fz = players[num3].charP.mass * (mpData[num3].springZ * 48f - players[num3].charP.velocity.v[2] * 10f);
						mainC.physicsMain.getPosition(ref players[num3].charP, frameTime * global::Physics.Physics.timeMod);
						ref Matrix reference3 = ref global::MainGame.MainGame.playerVehicles[num3].mv[uBufferID];
						reference3 = players[num3].mv[uBufferID];
						byte currentWeapon = (byte)players[num3].primaryWeaponMountWeapon;
						players[num3].weapon2[players[num3].wpnIndex].roundChambered = true;
						players[num3].needToChamber = true;
						players[num3].speakingTimer -= frameTime;
						if (players[num3].speakingTimer < 0f)
						{
							players[num3].speakingTimer = 0f;
						}
						switch (players[num3].onmap)
						{
						case 2:
							players[num3].timeBeforeRespawn[uBufferID] -= frameTime;
							if (players[num3].timeBeforeRespawn[uBufferID] < 0f)
							{
								mainC.playersMain.Player_Spawn_Time_Over((ushort)num3);
							}
							global::Joints.Joints.Sync_Player_Matrices(num3, rBufferID, uBufferID);
							break;
						case 4:
							switch (global::MainGame.MainGame.playerVehicles[curVehicle].type)
							{
							case 0:
							case 8:
								if (!players[num3].taunting)
								{
									float num4 = players[num3].charP.velocity.v[0];
									float num5 = players[num3].charP.velocity.v[1];
									float num6 = players[num3].charP.velocity.v[2];
									float num7 = (float)Math.Sqrt(num4 * num4 + num5 * num5 + num6 * num6);
									global::MainGame.MainGame.playerVehicles[num3].ph1.velocity = num7;
									byte b;
									if (num7 > 1.25f)
									{
										num4 /= num7;
										num5 /= num7;
										num6 /= num7;
										num = num4 * players[num3].mv[uBufferID].M21 + num5 * players[num3].mv[uBufferID].M22 + num6 * players[num3].mv[uBufferID].M23;
										if (Math.Abs(num) > 0.707f)
										{
											b = 1;
										}
										else
										{
											num = num4 * players[num3].mv[uBufferID].M11 + num5 * players[num3].mv[uBufferID].M12 + num6 * players[num3].mv[uBufferID].M13;
											b = 2;
										}
									}
									else
									{
										b = 0;
									}
									switch (b)
									{
									case 1:
										if (num7 < 35f)
										{
											if (num >= 0f)
											{
												if (players[num3].playerIsMoving != 2)
												{
													players[num3].animationStopTimer -= frameTime;
													if (players[num3].animationStopTimer < 0f)
													{
														players[num3].playerIsMoving = 2;
														players[num3].animationStopTimer = 0.05f;
														mainC.programsMain.Start_Animation((ushort)num3, ref players[num3].jt1, ref players[num3].animations, players[num3].programCollection, players[num3].programWalk, 1f, 1f);
														mainC.programsMain.Start_Animation((ushort)num3, ref players[num3].jt1, ref players[num3].animations, players[num3].programCollection, global::MainGame.MainGame.playerVehicles[num3].weapons[global::MainGame.MainGame.playerVehicles[num3].mounts[global::MainGame.MainGame.primaryWeaponMount].objectID].AnimationWalk, 1f, 1f);
													}
												}
												else
												{
													mainC.avatarMain.Avatar_Movement_By_List_ID((byte)num3, 1, loop: true, 1, cancelOtherGroupAnimations: true);
												}
											}
											else if (num < 0f)
											{
												if (players[num3].playerIsMoving != 16)
												{
													players[num3].animationStopTimer -= frameTime;
													if (players[num3].animationStopTimer < 0f)
													{
														players[num3].playerIsMoving = 16;
														players[num3].animationStopTimer = 0.05f;
														mainC.programsMain.Start_Animation((ushort)num3, ref players[num3].jt1, ref players[num3].animations, players[num3].programCollection, players[num3].programWalkBackwards, -1f, 1f);
														mainC.programsMain.Start_Animation((ushort)num3, ref players[num3].jt1, ref players[num3].animations, players[num3].programCollection, global::MainGame.MainGame.playerVehicles[num3].weapons[global::MainGame.MainGame.playerVehicles[num3].mounts[global::MainGame.MainGame.primaryWeaponMount].objectID].AnimationWalk, 1f, 1f);
													}
												}
												else
												{
													mainC.avatarMain.Avatar_Movement_By_List_ID((byte)num3, 1, loop: true, 1, cancelOtherGroupAnimations: true);
												}
											}
											mainC.avatarMain.Avatar_Speed_Adjustment_By_List_ID((ushort)num3, 1, num7 / 20f * 1.3f, 0.3f);
										}
										else if (players[num3].playerIsMoving != 8)
										{
											players[num3].animationStopTimer -= frameTime;
											if (players[num3].animationStopTimer < 0f)
											{
												players[num3].playerIsMoving = 8;
												players[num3].animationStopTimer = 0.1f;
												mainC.programsMain.Start_Animation((ushort)num3, ref players[num3].jt1, ref players[num3].animations, players[num3].programCollection, players[num3].programRun, 1f, 1f);
												mainC.programsMain.Start_Animation((ushort)num3, ref players[num3].jt1, ref players[num3].animations, players[num3].programCollection, global::MainGame.MainGame.playerVehicles[num3].weapons[global::MainGame.MainGame.playerVehicles[num3].mounts[global::MainGame.MainGame.primaryWeaponMount].objectID].AnimationRun, 1f, 1f);
											}
										}
										else
										{
											mainC.avatarMain.Avatar_Movement_By_List_ID((byte)num3, 1, loop: true, 2, cancelOtherGroupAnimations: true);
										}
										break;
									case 2:
										if (num >= 0f)
										{
											if (players[num3].playerIsMoving != 4)
											{
												players[num3].animationStopTimer -= frameTime;
												if (players[num3].animationStopTimer < 0f)
												{
													mainC.programsMain.Start_Animation((ushort)num3, ref players[num3].jt1, ref players[num3].animations, players[num3].programCollection, players[num3].programSidestep, 1f, 1f);
													mainC.programsMain.Start_Animation((ushort)num3, ref players[num3].jt1, ref players[num3].animations, players[num3].programCollection, global::MainGame.MainGame.playerVehicles[num3].weapons[global::MainGame.MainGame.playerVehicles[num3].mounts[global::MainGame.MainGame.primaryWeaponMount].objectID].AnimationWalk, 1f, 1f);
													players[num3].playerIsMoving = 4;
													players[num3].animationStopTimer = 0.1f;
												}
											}
											else
											{
												mainC.avatarMain.Avatar_Movement_By_List_ID((byte)num3, 1, loop: true, 4, cancelOtherGroupAnimations: true);
											}
											mainC.avatarMain.Avatar_Speed_Adjustment_By_List_ID((ushort)num3, 4, num7 / 20f * 1.3f, 0.3f);
										}
										else
										{
											if (!(num < 0f))
											{
												break;
											}
											if (players[num3].playerIsMoving != 512)
											{
												players[num3].animationStopTimer -= frameTime;
												if (players[num3].animationStopTimer < 0f)
												{
													mainC.programsMain.Start_Animation((ushort)num3, ref players[num3].jt1, ref players[num3].animations, players[num3].programCollection, players[num3].programSidestep, 1f, 1f);
													mainC.programsMain.Start_Animation((ushort)num3, ref players[num3].jt1, ref players[num3].animations, players[num3].programCollection, global::MainGame.MainGame.playerVehicles[num3].weapons[global::MainGame.MainGame.playerVehicles[num3].mounts[global::MainGame.MainGame.primaryWeaponMount].objectID].AnimationWalk, 1f, 1f);
													players[num3].playerIsMoving = 512;
													players[num3].animationStopTimer = 0.1f;
												}
											}
											else
											{
												mainC.avatarMain.Avatar_Movement_By_List_ID((byte)num3, 1, loop: true, 3, cancelOtherGroupAnimations: true);
											}
											mainC.avatarMain.Avatar_Speed_Adjustment_By_List_ID((ushort)num3, 3, num7 / 20f * 1.3f, 0.3f);
										}
										break;
									case 0:
										if (players[num3].playerIsMoving != 1)
										{
											players[num3].animationStopTimer -= frameTime;
											if (players[num3].animationStopTimer < 0f)
											{
												players[num3].playerIsMoving = 1;
												players[num3].animationStopTimer = 0.15f;
											}
										}
										else
										{
											players[num3].animationStopTimer -= frameTime;
											if (players[num3].animationStopTimer < 0f)
											{
												players[num3].animationStopTimer = 0f;
											}
										}
										if (players[num3].playerIsMoving == 1)
										{
											mainC.programsMain.Start_Animation((ushort)num3, ref players[num3].jt1, ref players[num3].animations, players[num3].programCollection, players[num3].programStationaryLegsBody, 1f, 1f);
											mainC.programsMain.Start_Animation((ushort)num3, ref players[num3].jt1, ref players[num3].animations, players[num3].programCollection, global::Weapons.Weapons.wp1[players[num3].primaryWeaponMountWeapon].AnimationHolding, 1f, 1f);
										}
										break;
									}
								}
								else if (Math.Abs(mpData[num3].velX) + Math.Abs(mpData[num3].velY) + Math.Abs(mpData[num3].velZ) > 0.05f)
								{
									players[num3].taunting = false;
								}
								break;
							case 1:
							case 2:
							case 5:
							case 6:
							case 7:
								if (players[num3].playerIsMoving != 1)
								{
									players[num3].playerIsMoving = 1;
									mainC.gameLogic.Game_Set_Avatar_Vehicle_Pose((ushort)num3);
								}
								break;
							}
							if (players[num3].xRotation < -60f)
							{
								players[num3].xRotation = -60f;
							}
							else if (players[num3].xRotation > 60f)
							{
								players[num3].xRotation = 60f;
							}
							mainC.programsMain.Set_Joints_To_Animation_Step_Percentage(ref players[num3].jt1, players[num3].programCollection, playerRaces[players[num3].race].torqueJoint[players[num3].type], (players[num3].xRotation + 60f) / 120f);
							global::Joints.Joints.Process_Joints_Threaded(num3, frameTime, threadID);
							players[num3].invincibleTimer -= frameTime;
							if (players[num3].invincibleTimer < 0f)
							{
								players[num3].invincible = false;
								players[num3].invincibleTimer = 0f;
							}
							switch (global::MainGame.MainGame.playerVehicles[curVehicle].type)
							{
							case 0:
								mainC.vehicles.Update_Vehicle_Matrix((ushort)num3);
								break;
							case 1:
							case 6:
							case 7:
							{
								byte b = playerRaces[players[num3].race].programTurnRight[players[num3].type];
								GameLogic.propRot[num3] = 0f;
								if (b != byte.MaxValue)
								{
									float num4 = 465f + 14900f * global::MainGame.MainGame.playerVehicles[num3].throttleSpeed;
									GameLogic.propRot[num3] = num4 * frameTime;
									players[num3].jt1[b].pivot2Speed = num4;
									players[num3].jt1[b].targetPivot2 += num4;
									while (players[num3].jt1[b].targetPivot2 > 360f)
									{
										players[num3].jt1[b].targetPivot2 -= 360f;
									}
								}
								break;
							}
							case 8:
								global::Joints.Joints.Process_Joints_Threaded(num3, frameTime, threadID);
								break;
							}
							_ = players[num3].mv[uBufferID];
							mainC.weaponsMain.Process_Player_Weapons(num3, currentWeapon);
							global::MainGame.MainGame.playerVehicles[num3].ph1.x = players[num3].charP.position.v[0];
							global::MainGame.MainGame.playerVehicles[num3].ph1.y = players[num3].charP.position.v[1];
							global::MainGame.MainGame.playerVehicles[num3].ph1.z = players[num3].charP.position.v[2];
							switch (global::MainGame.MainGame.playerVehicles[curVehicle].type)
							{
							case 0:
							case 8:
								Update_Player_BoundingBox(num3, players[num3].charP.position.v[0], players[num3].charP.position.v[1], players[num3].charP.position.v[2], threadID);
								break;
							case 1:
							case 2:
							case 5:
							case 6:
							case 7:
							{
								float num4 = (float)global::MainGame.MainGame.mainRandom.Next(0, 32000) / 32000f * 0.2f;
								num4 = global::MainGame.MainGame.playerVehicles[num3].throttleSpeed * 0.8f + num4;
								mainC.soundsMain.Play_Moving_Continual_Sound((ushort)num3, 2, stop: false, 0.5f + global::MainGame.MainGame.playerVehicles[num3].throttleSpeed * 0.5f, num4 * 100f, global::MainGame.MainGame.playerVehicles[num3].ph1.x, global::MainGame.MainGame.playerVehicles[num3].ph1.y, global::MainGame.MainGame.playerVehicles[num3].ph1.z, global::MainGame.MainGame.playerVehicles[num3].ph1.velocityX, global::MainGame.MainGame.playerVehicles[num3].ph1.velocityY, global::MainGame.MainGame.playerVehicles[num3].ph1.velocityZ);
								Update_Player_Vehicle_BoundingBox(num3, threadID, (ushort)(Vehicles.vehicles[curVehicle].numWheels + Vehicles.vehicles[curVehicle].numColPoints), ref players[num3].mv[global::Rendering.Rendering.uBufferID]);
								if (players[num3].damagePercentageCapped > 0.1f)
								{
									Damage_Particles_For_Damaged_Player_Vehicle((ushort)num3, threadID);
								}
								break;
							}
							}
							break;
						case 8:
							GameLogic.propRot[num3] = 0f;
							global::MainGame.MainGame.playerVehicles[num3].ph1.x = players[num3].charP.position.v[0];
							global::MainGame.MainGame.playerVehicles[num3].ph1.y = players[num3].charP.position.v[1];
							global::MainGame.MainGame.playerVehicles[num3].ph1.z = players[num3].charP.position.v[2];
							global::Joints.Joints.Reset_Joint_Data(num3);
							global::Joints.Joints.Process_Joints_Threaded(num3, frameTime, threadID);
							switch (global::MainGame.MainGame.playerVehicles[curVehicle].type)
							{
							case 0:
							case 8:
								mainC.vehicles.Update_Vehicle_Matrix((ushort)num3);
								Update_Player_BoundingBox(num3, players[num3].charP.position.v[0], players[num3].charP.position.v[1], players[num3].charP.position.v[2], threadID);
								break;
							case 1:
							case 2:
							case 5:
							case 6:
							case 7:
								Update_Player_Vehicle_BoundingBox(num3, threadID, (ushort)(Vehicles.vehicles[curVehicle].numWheels + Vehicles.vehicles[curVehicle].numColPoints), ref players[num3].mv[global::Rendering.Rendering.uBufferID]);
								if (players[num3].damagePercentageCapped > 0.1f)
								{
									Damage_Particles_For_Damaged_Player_Vehicle((ushort)num3, threadID);
								}
								break;
							}
							break;
						}
						Confine_Player_Position_ToBoundaries(num3, postCollision: true, threadID);
					}
				}
				if (global::Networking.Networking.isHost && flag)
				{
					Send_Team_Points();
				}
			}
			for (short num3 = (short)num2; num3 < global::MainGame.MainGame.maxGamePlayers; num3++)
			{
				if (players[num3].active)
				{
					players[num3].speakingTimer -= frameTime;
					if (players[num3].speakingTimer < 0f)
					{
						players[num3].speakingTimer = 0f;
					}
				}
			}
			if (global::Networking.Networking.isHost && mpData[0].delayedPointsSend)
			{
				mpData[0].delayedPointsTime -= global::MainGame.MainGame.frametime;
				if (mpData[0].delayedPointsTime < 0f)
				{
					mpData[0].delayedPointsSend = false;
					bool flag = true;
					Send_Player_Points(0);
				}
			}
			for (short num3 = 0; num3 < global::MainGame.MainGame.maxGamePlayers; num3++)
			{
				if (global::Rendering.Rendering.muzzleFlashes[uBufferID, num3].timeRemaining > 0f)
				{
					global::Rendering.Rendering.muzzleFlashes[uBufferID, num3].timeRemaining = 0f;
				}
			}
			for (short num3 = 0; num3 < global::MainGame.MainGame.maxGamePlayers; num3++)
			{
				for (ushort num8 = 0; num8 < global::MainGame.MainGame.playerVehicles[num3].numMounts; num8++)
				{
					if (global::MainGame.MainGame.playerVehicles[num3].mounts[num8].type == 1 && global::MainGame.MainGame.playerVehicles[num3].mounts[num8].objectAttached == 1)
					{
						ushort objectID = global::MainGame.MainGame.playerVehicles[num3].mounts[num8].objectID;
						if (global::MainGame.MainGame.playerVehicles[num3].weapons[objectID].muzzleFlashTimer > 0f)
						{
							mainC.weaponsMain.Add_Particles_For_Fired_Weapon(num3, (byte)objectID, threadID);
							global::MainGame.MainGame.playerVehicles[num3].weapons[objectID].muzzleFlashTimer -= frameTime;
						}
					}
				}
			}
			for (short num3 = 0; num3 < 1; num3++)
			{
				if (players[num3].active)
				{
					players[num3].speakingTimer -= frameTime;
					if (players[num3].speakingTimer < 0f)
					{
						players[num3].speakingTimer = 0f;
					}
				}
			}
		}
		catch
		{
		}
	}

	public void Move_Main_Player(float frameTime, byte threadID)
	{
		players[0].invincibleTimer -= frameTime;
		if (players[0].invincibleTimer < 0f)
		{
			players[0].invincible = false;
			players[0].invincibleTimer = 0f;
		}
		switch (Vehicles.vehicles[players[0].curVehicle].type)
		{
		case 0:
		case 8:
			Move_MainPlayer_Humanoid(frameTime, threadID);
			break;
		case 1:
			Move_MainPlayer_Airplane(frameTime, threadID);
			break;
		case 2:
			Move_MainPlayer_SkateBoard(frameTime, threadID);
			break;
		case 3:
			Move_MainPlayer_FixedTurret(frameTime, threadID);
			break;
		case 5:
			Move_MainPlayer_SpaceShip(frameTime, threadID);
			break;
		case 6:
			Move_MainPlayer_ArcadeStyle_Airplane(frameTime, threadID);
			break;
		case 7:
			Move_MainPlayer_ArcadeStyle_Helicopter(frameTime, threadID);
			break;
		}
		mainC.vehicles.Update_Vehicle_Matrix(0);
	}

	public float Get_Minimum_Angle_To_Enemy(byte threadID)
	{
		StructsClass.Object_Position p = new StructsClass.Object_Position
		{
			x1 = global::Rendering.Rendering.matrixVInverse.M41,
			y1 = global::Rendering.Rendering.matrixVInverse.M42,
			z1 = global::Rendering.Rendering.matrixVInverse.M43
		};
		float num = 0f - global::Rendering.Rendering.matrixVInverse.M31;
		float num2 = 0f - global::Rendering.Rendering.matrixVInverse.M32;
		float num3 = 0f - global::Rendering.Rendering.matrixVInverse.M33;
		float num4 = 0f;
		float num5 = 0f;
		float num6 = (float)Math.Sqrt(num * num + num2 * num2);
		if (num6 != 0f)
		{
			num4 = num / num6;
			num5 = num2 / num6;
		}
		float num7 = 360f;
		for (ushort num8 = 1; num8 < global::MainGame.MainGame.maxGamePlayers; num8++)
		{
			if (players[num8].onmap == 4 && (players[num8].teamMask & enemyTeamMask) != 0)
			{
				num6 = global::MainGame.MainGame.playerVehicles[num8].ph1.x - p.x1;
				float num9 = global::MainGame.MainGame.playerVehicles[num8].ph1.y - p.y1;
				float num10 = global::MainGame.MainGame.playerVehicles[num8].ph1.z - p.z1;
				if (num6 * num + num9 * num2 + num10 * num3 > 0f)
				{
					p.x2 = global::MainGame.MainGame.playerVehicles[num8].ph1.x;
					p.y2 = global::MainGame.MainGame.playerVehicles[num8].ph1.y;
					p.z2 = global::MainGame.MainGame.playerVehicles[num8].ph1.z;
					if (mainC.collisionMain.CheckCollision_Detailed_List_Single(ref p, 3, threadID) == 0)
					{
						float num11 = (float)Math.Sqrt(num6 * num6 + num9 * num9);
						if (num11 != 0f)
						{
							num6 /= num11;
							num9 /= num11;
						}
						num6 = (float)Math.Acos(num6 * num4 + num9 * num5) * 57.29578f;
						if (num6 < num7)
						{
							num7 = num6;
						}
					}
				}
			}
		}
		return num7;
	}

	public void Move_MainPlayer_Humanoid(float frameTime, byte threadID)
	{
		float num = 1f;
		float num2 = 1f;
		float num3 = 0f;
		float num4 = 0f;
		float num5 = 0f;
		float num6 = 0f;
		float num7 = 0f;
		float value = 0f;
		float value2 = 0f;
		float value3 = 0f;
		float num8 = 0f;
		float num9 = 0f;
		float num10 = 0f;
		StructsClass.Object_Position p = default(StructsClass.Object_Position);
		byte uBufferID = global::Rendering.Rendering.uBufferID;
		byte rBufferID = global::Rendering.Rendering.rBufferID;
		float num11 = zRotation;
		frameTime = global::MainGame.MainGame.frametime;
		global::MainGame.MainGame.playerVehicles[0].ph1.forceZ = -32.15223f * Vehicles.vehicles[players[0].curVehicle].ph1.mass;
		float num12 = Math.Abs(global::InputHandler.InputHandler.controllerStickRightValX);
		float num13 = Math.Abs(global::InputHandler.InputHandler.controllerStickRightValY);
		float num14 = num12 * global::InputHandler.InputHandler.controllerStickRightValX;
		if (controlsInUse)
		{
			if (currentView == 7)
			{
				global::Rendering.Rendering.satelliteCrossHairX[uBufferID] = global::Rendering.Rendering.satelliteCrossHairX[rBufferID];
				global::Rendering.Rendering.satelliteCrossHairY[uBufferID] = global::Rendering.Rendering.satelliteCrossHairY[rBufferID];
				float num15 = num14 * (0.75f + 3.75f * num12) * global::InputHandler.InputHandler.lookSensitivity[global::InputHandler.InputHandler.lookMode];
				num14 = num13 * global::InputHandler.InputHandler.controllerStickRightValY;
				float num16 = num14 * (0.75f + 3.75f * num13) * global::InputHandler.InputHandler.lookSensitivity[global::InputHandler.InputHandler.lookMode] * invertY;
				global::Rendering.Rendering.satelliteCrossHairX[uBufferID] = global::Rendering.Rendering.satelliteCrossHairX[uBufferID] + num15 * global::Rendering.Rendering.middleOfScreenX * global::MainGame.MainGame.frametime;
				global::Rendering.Rendering.satelliteCrossHairY[uBufferID] = global::Rendering.Rendering.satelliteCrossHairY[uBufferID] - num16 * global::Rendering.Rendering.middleOfScreenY * global::MainGame.MainGame.frametime;
				num12 = Math.Abs(global::InputHandler.InputHandler.controllerStickLeftValueX);
				num14 = num12 * global::InputHandler.InputHandler.controllerStickLeftValueX;
				num15 = num14 * (0.75f + 3.75f * num12) * global::InputHandler.InputHandler.lookSensitivity[global::InputHandler.InputHandler.lookMode];
				num13 = Math.Abs(global::InputHandler.InputHandler.controllerStickLeftValueY);
				num14 = num13 * global::InputHandler.InputHandler.controllerStickLeftValueY;
				num16 = num14 * (0.75f + 3.75f * num13) * global::InputHandler.InputHandler.lookSensitivity[global::InputHandler.InputHandler.lookMode];
				global::Rendering.Rendering.satelliteCrossHairX[uBufferID] = global::Rendering.Rendering.satelliteCrossHairX[uBufferID] + num15 * global::Rendering.Rendering.middleOfScreenX * global::MainGame.MainGame.frametime;
				global::Rendering.Rendering.satelliteCrossHairY[uBufferID] = global::Rendering.Rendering.satelliteCrossHairY[uBufferID] - num16 * global::Rendering.Rendering.middleOfScreenY * global::MainGame.MainGame.frametime;
			}
			moving = 0;
			playerSpeed = 0f;
			playerSpeedSideways = 0f;
		}
		float num17;
		if ((moving & 0xC) > 0)
		{
			switch (controllerScheme)
			{
			case 0:
			{
				float num15 = num14 * (50f + 215f * num12) * global::InputHandler.InputHandler.lookSensitivity[global::InputHandler.InputHandler.lookMode] * scopeViewAdj;
				num17 = Set_Speed_If_Targeting_Enemy(0, frameTime, 225f, 14400f, 300f, doCollisionCheck: true, num15 * stickSpringVelY * global::Weapons.Weapons.mobilityFactor, threadID);
				float num18 = 0f - num15;
				num = num17;
				num2 = num17;
				previousStickValueX += (num18 - previousStickValueX) * num17;
				num14 = num13 * global::InputHandler.InputHandler.controllerStickRightValY;
				float num19 = num17 * num14 * (50f + 250f * num13) * global::InputHandler.InputHandler.lookSensitivity[global::InputHandler.InputHandler.lookMode] * scopeViewAdj;
				if (num12 > 0.968f + global::InputHandler.InputHandler.lookSensitivity[global::InputHandler.InputHandler.lookMode] * 0.01f)
				{
					stickSpringAccelY += frameTime * (50f + global::InputHandler.InputHandler.lookSensitivity[global::InputHandler.InputHandler.lookMode] * 40f);
					stickSpringVelY += frameTime * stickSpringAccelY;
					if (stickSpringVelY > 4.9f)
					{
						stickSpringVelY = 4.9f;
					}
				}
				else
				{
					stickSpringVelY -= frameTime * 50f;
					if (stickSpringVelY < 1f)
					{
						stickSpringVelY = 1f;
						stickSpringAccelY = 0f;
					}
				}
				float num20 = 0f;
				if (num12 < 0.4f)
				{
					num20 = (0f - num12) * 0.1f;
				}
				if (num13 > 0.96f + num20)
				{
					stickSpringAccelX += frameTime * (50f + global::InputHandler.InputHandler.lookSensitivity[global::InputHandler.InputHandler.lookMode] * 40f);
					stickSpringVelX += frameTime * stickSpringAccelX;
					if (stickSpringVelX > 4.9f)
					{
						stickSpringVelX = 4.9f;
					}
				}
				else
				{
					stickSpringVelX -= frameTime * 50f;
					if (stickSpringVelX < 1f)
					{
						stickSpringVelX = 1f;
						stickSpringAccelX = 0f;
					}
				}
				num6 = num * previousStickValueX * stickSpringVelY * global::Weapons.Weapons.mobilityFactor * frameTime;
				zRotation += num6;
				if (!global::MainGame.MainGame.sprinting || currentView == 0)
				{
					num7 = num2 * num19 * stickSpringVelX * global::Weapons.Weapons.mobilityFactor * frameTime;
					xRotation += num7;
				}
				break;
			}
			case 1:
				num17 = mainC.graphingMain.Get_Point_2D_Float_Graph(num12, 0f, 0, 0);
				num14 = Get_Minimum_Angle_To_Enemy(threadID);
				if (Math.Sign(stickSpringVelX) != Math.Sign(global::InputHandler.InputHandler.controllerStickRightValX))
				{
					stickTimerX = 0f;
				}
				stickSpringVelX = global::InputHandler.InputHandler.controllerStickRightValX;
				if (num12 > 0.95f)
				{
					stickTimerX += frameTime;
					if (stickTimerX >= 0.25f + global::InputHandler.InputHandler.lookSensitivity[global::InputHandler.InputHandler.lookMode] * 0.1f)
					{
						stickTimerX = 1f;
						stickSpringAccelX += frameTime * 2.5f;
						if (stickSpringAccelX > 3f)
						{
							stickSpringAccelX = 3f;
						}
					}
				}
				if (num14 < 15f)
				{
					num12 = previousStickValueX + (num12 - previousStickValueX) * 0.15f;
				}
				previousStickValueX = num12;
				num17 = (100f + 140f * global::InputHandler.InputHandler.lookSensitivity[global::InputHandler.InputHandler.lookMode]) * num12 * num12;
				if (num14 < 15f)
				{
					num17 = ((!(num12 < 0.95f)) ? (num17 * 0.8f) : (num17 * 0.5f));
					stickSpringAccelX = 1f;
				}
				else if (num14 < 30f)
				{
					num17 = ((!(num12 < 0.95f)) ? (num17 * 0.9f) : (num17 * 0.75f));
					stickSpringAccelX = 1f;
				}
				num6 = num17 * stickSpringAccelX * (0f - global::InputHandler.InputHandler.controllerStickRightValX) * global::Weapons.Weapons.mobilityFactor * scopeViewAdj * frameTime;
				zRotation += num6;
				num17 = mainC.graphingMain.Get_Point_2D_Float_Graph(num13, 0f, 0, 0);
				if (Math.Sign(stickSpringVelY) != Math.Sign(global::InputHandler.InputHandler.controllerStickRightValY))
				{
					stickTimerY = 0f;
				}
				stickSpringVelY = global::InputHandler.InputHandler.controllerStickRightValY;
				if (num13 > 0.95f)
				{
					stickTimerY += frameTime;
					if (stickTimerY >= 0.25f + global::InputHandler.InputHandler.lookSensitivity[global::InputHandler.InputHandler.lookMode] * 0.1f)
					{
						stickTimerY = 1f;
						stickSpringAccelY += frameTime * 2.5f;
						if (stickSpringAccelY > 3f)
						{
							stickSpringAccelY = 3f;
						}
					}
				}
				if (num14 < 15f)
				{
					num13 = previousStickValueY + (num13 - previousStickValueY) * 0.4f;
				}
				previousStickValueY = num13;
				num17 = (100f + 140f * global::InputHandler.InputHandler.lookSensitivity[global::InputHandler.InputHandler.lookMode]) * num13 * num13;
				if (num14 < 15f)
				{
					num17 = ((!(num13 < 0.9f)) ? (num17 * 0.9f) : (num17 * 0.5f));
					stickSpringAccelY = 1f;
				}
				num7 = num17 * stickSpringAccelY * global::InputHandler.InputHandler.controllerStickRightValY * global::Weapons.Weapons.mobilityFactor * scopeViewAdj * frameTime;
				xRotation += num7;
				break;
			}
			if (zRotation > 360f)
			{
				zRotation -= 360f;
			}
			if (zRotation < 0f)
			{
				zRotation += 360f;
			}
			if (xRotation < -60f)
			{
				xRotation = -60f;
			}
			else if (xRotation > 60f)
			{
				xRotation = 60f;
			}
		}
		else
		{
			switch (controllerScheme)
			{
			case 0:
				stickSpringVelY = 1f;
				stickSpringAccelY = 0f;
				stickSpringVelX = 1f;
				stickSpringAccelX = 0f;
				break;
			case 1:
				stickTimerX = 0f;
				stickTimerY = 0f;
				previousStickValueX = 0f;
				previousStickValueY = 0f;
				stickSpringAccelX = 1f;
				stickSpringAccelY = 1f;
				stickSpringVelX = 0f;
				stickSpringVelY = 0f;
				break;
			}
		}
		if (global::MainGame.MainGame.sprinting && currentView != 0)
		{
			num7 = (0f - xRotation) * 3f * frameTime;
			xRotation += num7;
		}
		xRotMovement = num7;
		ref Matrix reference = ref players[0].mv[uBufferID];
		reference = Matrix.CreateRotationZ(zRotation * ((float)Math.PI / 180f));
		Matrix matrix = players[0].mv[uBufferID];
		if ((players[0].playerIsMoving & 0x7FFE) > 0)
		{
			mainC.weaponsMain.Check_Weapon_Views();
		}
		if (players[0].onmap == 4)
		{
			if (players[0].playerIsMoving != 32 && players[0].playerIsMoving != 256)
			{
				if ((moving & 3) != 0)
				{
					if (global::MainGame.MainGame.walking)
					{
						if (players[0].playerIsMoving != 2)
						{
							players[0].playerIsMoving = 2;
							footStepTimer = 0.48f;
							mainC.avatarMain.Avatar_Movement_By_List_ID(0, 1, loop: true, 1, cancelOtherGroupAnimations: true);
						}
						mainC.programsMain.Start_Animation(0, ref players[0].jt1, ref players[0].animations, players[0].programCollection, players[0].programWalk, Math.Abs(global::InputHandler.InputHandler.controllerStickLeftValueY), 0.5f + 0.5f * Math.Abs(global::InputHandler.InputHandler.controllerStickLeftValueY));
						mainC.programsMain.Start_Animation(0, ref players[0].jt1, ref players[0].animations, players[0].programCollection, global::MainGame.MainGame.playerVehicles[0].weapons[global::MainGame.MainGame.playerVehicles[0].mounts[global::MainGame.MainGame.primaryWeaponMount].objectID].AnimationWalk, 1f, 1f);
					}
					else if (global::MainGame.MainGame.walkingBackwards)
					{
						if (players[0].playerIsMoving != 16)
						{
							players[0].playerIsMoving = 16;
							footStepTimer = 0.48f;
							mainC.avatarMain.Avatar_Movement_By_List_ID(0, 1, loop: true, 1, cancelOtherGroupAnimations: true);
						}
						mainC.programsMain.Start_Animation(0, ref players[0].jt1, ref players[0].animations, players[0].programCollection, players[0].programWalkBackwards, 0f - Math.Abs(global::InputHandler.InputHandler.controllerStickLeftValueY), 0.5f + 0.5f * Math.Abs(global::InputHandler.InputHandler.controllerStickLeftValueY));
						mainC.programsMain.Start_Animation(0, ref players[0].jt1, ref players[0].animations, players[0].programCollection, global::MainGame.MainGame.playerVehicles[0].weapons[global::MainGame.MainGame.playerVehicles[0].mounts[global::MainGame.MainGame.primaryWeaponMount].objectID].AnimationWalk, 1f, 1f);
					}
					else if (global::MainGame.MainGame.sideStepping)
					{
						if (players[0].playerIsMoving != 4)
						{
							players[0].playerIsMoving = 4;
							footStepTimer = 0.48f;
							if (playerSpeedSideways <= 0f)
							{
								mainC.avatarMain.Avatar_Movement_By_List_ID(0, 1, loop: true, 3, cancelOtherGroupAnimations: true);
							}
							else
							{
								mainC.avatarMain.Avatar_Movement_By_List_ID(0, 1, loop: true, 4, cancelOtherGroupAnimations: true);
							}
						}
						mainC.programsMain.Start_Animation(0, ref players[0].jt1, ref players[0].animations, players[0].programCollection, global::MainGame.MainGame.playerVehicles[0].weapons[global::MainGame.MainGame.playerVehicles[0].mounts[global::MainGame.MainGame.primaryWeaponMount].objectID].AnimationWalk, 1f, 1f);
					}
					else if (global::MainGame.MainGame.sprinting)
					{
						global::MainGame.MainGame.showCrossHairs[1] = 1;
						if (players[0].playerIsMoving != 8)
						{
							mainC.weaponsMain.Stop_Using_Iron_Sights_Or_Weapon_Scope();
							players[0].playerIsMoving = 8;
							footStepTimer = 0.744f;
							mainC.avatarMain.Avatar_Movement_By_List_ID(0, 1, loop: true, 2, cancelOtherGroupAnimations: true);
						}
						mainC.programsMain.Start_Animation(0, ref players[0].jt1, ref players[0].animations, players[0].programCollection, players[0].programRun, 1f, 1f);
						mainC.programsMain.Start_Animation(0, ref players[0].jt1, ref players[0].animations, players[0].programCollection, global::MainGame.MainGame.playerVehicles[0].weapons[global::MainGame.MainGame.playerVehicles[0].mounts[global::MainGame.MainGame.primaryWeaponMount].objectID].AnimationRun, 1f, 1f);
					}
				}
				else if ((moving & 4) == 4)
				{
					if (num6 > 0f)
					{
						if (players[0].playerIsMoving != 64)
						{
							players[0].playerIsMoving = 64;
							mainC.avatarMain.Avatar_Movement_By_List_ID(0, 1, loop: true, 0, cancelOtherGroupAnimations: true);
						}
						mainC.programsMain.Start_Animation(0, ref players[0].jt1, ref players[0].animations, players[0].programCollection, global::MainGame.MainGame.playerVehicles[0].weapons[global::MainGame.MainGame.playerVehicles[0].mounts[global::MainGame.MainGame.primaryWeaponMount].objectID].AnimationWalk, 1f, 1f);
					}
					else if (num6 < 0f)
					{
						if (players[0].playerIsMoving != 128)
						{
							players[0].playerIsMoving = 128;
							mainC.avatarMain.Avatar_Movement_By_List_ID(0, 1, loop: true, 0, cancelOtherGroupAnimations: true);
						}
						mainC.programsMain.Start_Animation(0, ref players[0].jt1, ref players[0].animations, players[0].programCollection, global::MainGame.MainGame.playerVehicles[0].weapons[global::MainGame.MainGame.playerVehicles[0].mounts[global::MainGame.MainGame.primaryWeaponMount].objectID].AnimationWalk, 1f, 1f);
					}
				}
				else
				{
					if (players[0].playerIsMoving != 1)
					{
						players[0].playerIsMoving = 1;
						mainC.avatarMain.Avatar_Movement_By_List_ID(0, 1, loop: true, 0, cancelOtherGroupAnimations: true);
					}
					mainC.programsMain.Start_Animation(0, ref players[0].jt1, ref players[0].animations, players[0].programCollection, players[0].programStationaryLegsBody, 1f, 1f);
					mainC.programsMain.Start_Animation(0, ref players[0].jt1, ref players[0].animations, players[0].programCollection, global::MainGame.MainGame.playerVehicles[0].weapons[global::MainGame.MainGame.playerVehicles[0].mounts[global::MainGame.MainGame.primaryWeaponMount].objectID].AnimationHolding, 1f, 1f);
				}
			}
			if (needToReload)
			{
				if (!reloading)
				{
					reloading = mainC.playersMain.Player_Needs_To_Reload(0);
				}
				mainC.weaponsMain.Check_Weapon_Views();
				if (reloading)
				{
					global::MainGame.MainGame.sprinting = false;
					global::MainGame.MainGame.showCrossHairs[0] = 1;
				}
				needToReload = false;
			}
			else if (needToChamber)
			{
				if (!chambering)
				{
					chambering = Player_Needs_To_Chamber(0);
				}
				mainC.weaponsMain.Check_Weapon_Views();
				if (chambering)
				{
					global::MainGame.MainGame.sprinting = false;
				}
				needToChamber = false;
			}
		}
		global::MainGame.MainGame.playerVehicles[0].ph1.forceX = 0f;
		global::MainGame.MainGame.playerVehicles[0].ph1.forceY = 0f;
		global::MainGame.MainGame.playerVehicles[0].ph1.forceZ = 0f;
		global::MainGame.MainGame.playerVehicles[0].ph1.torqueX = 0f;
		global::MainGame.MainGame.playerVehicles[0].ph1.torqueY = 0f;
		global::MainGame.MainGame.playerVehicles[0].ph1.torqueZ = 0f;
		int num21 = 0;
		bool flag = false;
		float num22;
		float num23;
		float num24;
		float num25;
		float num26;
		float num27;
		if (players[0].onmap == 4)
		{
			if (collisionWithGround == 0f)
			{
				p.x1 = global::MainGame.MainGame.playerVehicles[0].ph1.x;
				p.y1 = global::MainGame.MainGame.playerVehicles[0].ph1.y;
				p.z1 = global::MainGame.MainGame.playerVehicles[0].ph1.z - playerRaces[players[0].race].spawnHeight[players[0].type] + 0.1f;
				p.x2 = p.x1;
				p.y2 = p.y1;
				p.z2 = p.z1 - groundCheckDistance;
				if (mainC.collisionMain.CheckCollision_Detailed_List_Single(ref p, 0, threadID) > 0)
				{
					flag = true;
					if (global::Collision.Collision.floatArDir[threadID, 0, 2] >= 0.7f)
					{
						num22 = global::Collision.Collision.floatArDir[threadID, 0, 1] * matrix.M23 - global::Collision.Collision.floatArDir[threadID, 0, 2] * matrix.M22;
						num23 = global::Collision.Collision.floatArDir[threadID, 0, 2] * matrix.M21 - global::Collision.Collision.floatArDir[threadID, 0, 0] * matrix.M23;
						num24 = global::Collision.Collision.floatArDir[threadID, 0, 0] * matrix.M22 - global::Collision.Collision.floatArDir[threadID, 0, 1] * matrix.M21;
						num17 = (0f - playerSpeedSideways) * global::Weapons.Weapons.movementSpeedFactor;
						global::MainGame.MainGame.playerVehicles[0].ph1.velocityX = num17 * num22;
						global::MainGame.MainGame.playerVehicles[0].ph1.velocityY = num17 * num23;
						global::MainGame.MainGame.playerVehicles[0].ph1.velocityZ = num17 * num24;
						num25 = num23 * global::Collision.Collision.floatArDir[threadID, 0, 2] - num24 * global::Collision.Collision.floatArDir[threadID, 0, 1];
						num26 = num24 * global::Collision.Collision.floatArDir[threadID, 0, 0] - num22 * global::Collision.Collision.floatArDir[threadID, 0, 2];
						num27 = num22 * global::Collision.Collision.floatArDir[threadID, 0, 1] - num23 * global::Collision.Collision.floatArDir[threadID, 0, 0];
						num17 = playerSpeed * global::Weapons.Weapons.movementSpeedFactor;
						global::MainGame.MainGame.playerVehicles[0].ph1.velocityX += num17 * num25;
						global::MainGame.MainGame.playerVehicles[0].ph1.velocityY += num17 * num26;
						global::MainGame.MainGame.playerVehicles[0].ph1.velocityZ += num17 * num27;
						num21 = 1;
					}
				}
			}
			else if (collisionWithGround == 1f)
			{
				num17 = playerSpeedSideways * global::Weapons.Weapons.movementSpeedFactor;
				global::MainGame.MainGame.playerVehicles[0].ph1.velocityX = num17 * matrix.M11;
				global::MainGame.MainGame.playerVehicles[0].ph1.velocityY = num17 * matrix.M12;
				global::MainGame.MainGame.playerVehicles[0].ph1.velocityZ = 0f;
				num17 = playerSpeed * global::Weapons.Weapons.movementSpeedFactor;
				global::MainGame.MainGame.playerVehicles[0].ph1.velocityX += num17 * matrix.M21;
				global::MainGame.MainGame.playerVehicles[0].ph1.velocityY += num17 * matrix.M22;
				num21 = 1;
			}
		}
		else
		{
			global::MainGame.MainGame.playerVehicles[0].ph1.velocityX = 0f;
			global::MainGame.MainGame.playerVehicles[0].ph1.velocityY = 0f;
		}
		if (jumping)
		{
			jumping = false;
			fallingTimer = 0.001f;
			global::MainGame.MainGame.playerVehicles[0].ph1.velocityZ += 10f;
		}
		if (num21 == 0)
		{
			global::MainGame.MainGame.playerVehicles[0].ph1.forceZ = -32.15223f * global::MainGame.MainGame.playerVehicles[0].ph1.mass;
		}
		float velocityX = global::MainGame.MainGame.playerVehicles[0].ph1.velocityX;
		float velocityY = global::MainGame.MainGame.playerVehicles[0].ph1.velocityY;
		float velocityZ = global::MainGame.MainGame.playerVehicles[0].ph1.velocityZ;
		global::MainGame.MainGame.playerVehicles[0].ph1.velocity = (float)Math.Sqrt(velocityX * velocityX + velocityY * velocityY + velocityZ * velocityZ);
		float num28 = velocityX;
		float num29 = velocityY;
		num17 = (float)Math.Sqrt(num28 * num28 + num29 * num29);
		if (num17 != 0f)
		{
			num28 /= num17;
			num29 /= num17;
		}
		_ = global::MainGame.MainGame.playerVehicles[0].ph1.x;
		_ = global::MainGame.MainGame.playerVehicles[0].ph1.y;
		_ = global::MainGame.MainGame.playerVehicles[0].ph1.z;
		global::MainGame.MainGame.playerVehicles[0].ph1.z += stepOver;
		players[0].charMain.pos1.v[0] = global::MainGame.MainGame.playerVehicles[0].ph1.x;
		players[0].charMain.pos1.v[1] = global::MainGame.MainGame.playerVehicles[0].ph1.y;
		players[0].charMain.pos1.v[2] = global::MainGame.MainGame.playerVehicles[0].ph1.z;
		int i;
		if (players[0].onmap == 4)
		{
			num3 = 0f;
			num4 = 0f;
			for (i = 1; i < global::MainGame.MainGame.maxGamePlayers; i++)
			{
				if (players[i].onmap != 4)
				{
					continue;
				}
				num17 = players[0].charP.position.v[0] - players[i].charP.position.v[0];
				num14 = players[0].charP.position.v[1] - players[i].charP.position.v[1];
				num12 = players[0].charP.position.v[2] - players[i].charP.position.v[2];
				num13 = num17 * num17 + num14 * num14 + num12 * num12;
				if (!(num13 < players[0].playerSeparationDistanceSqr))
				{
					continue;
				}
				num13 = num17 * num17 + num14 * num14;
				if (num13 != 0f)
				{
					num13 = (float)Math.Sqrt(num13);
					num17 /= num13;
					num14 /= num13;
				}
				else
				{
					num17 = 1f;
				}
				num13 = num17 * num28 + num14 * num29;
				if (num13 < 0f)
				{
					num13 *= 0f - global::MainGame.MainGame.playerVehicles[0].ph1.velocity;
					if (num3 == 0f)
					{
						num3 = num13 * num17;
						global::MainGame.MainGame.playerVehicles[0].ph1.velocityX += num3;
					}
					else if (Math.Sign(num13 * num17) != Math.Sign(num3))
					{
						global::MainGame.MainGame.playerVehicles[0].ph1.velocityX = 0f;
					}
					if (num4 == 0f)
					{
						num4 = num13 * num14;
						global::MainGame.MainGame.playerVehicles[0].ph1.velocityY += num4;
					}
					else if (Math.Sign(num13 * num14) != Math.Sign(num4))
					{
						global::MainGame.MainGame.playerVehicles[0].ph1.velocityY = 0f;
					}
				}
			}
		}
		mainC.physicsMain.getPosition_new(ref global::MainGame.MainGame.playerVehicles[0].ph1, frameTime);
		if (players[0].onmap == 4)
		{
			i = playerRaces[players[0].race].torqueJoint[players[0].type];
			if (xRotation < -60f)
			{
				xRotation = -60f;
			}
			else if (xRotation > 60f)
			{
				xRotation = 60f;
			}
			mainC.programsMain.Set_Joints_To_Animation_Step_Percentage(ref players[0].jt1, players[0].programCollection, (ushort)i, (xRotation + 60f) / 120f);
		}
		global::Joints.Joints.Process_Joints_Threaded(0, frameTime, threadID);
		players[0].shotImpulse = 0f;
		players[0].shotTorque = 0f;
		int num30 = (int)players[0].charMain.numUsed;
		short num31 = (short)num30;
		num30 *= 3;
		i = 0;
		num21 = 0;
		int num32 = 129;
		while (i < num30)
		{
			global::Collision.Collision.floatAr[threadID, i++] = players[0].particlePrev[num21].v[0];
			global::Collision.Collision.floatAr[threadID, i++] = players[0].particlePrev[num21].v[1];
			global::Collision.Collision.floatAr[threadID, i++] = players[0].particlePrev[num21].v[2];
			global::Collision.Collision.floatAr[threadID, num32++] = players[0].charMain.v1[num21].v[0];
			global::Collision.Collision.floatAr[threadID, num32++] = players[0].charMain.v1[num21].v[1];
			global::Collision.Collision.floatAr[threadID, num32++] = players[0].charMain.v1[num21].v[2];
			num21++;
		}
		if (players[0].onmap == 8)
		{
			num32 = 129;
			global::Collision.Collision.floatAr[threadID, 0] = players[0].particlePrev[playerRaces[players[0].race].deathParticle[players[0].type]].v[0];
			global::Collision.Collision.floatAr[threadID, 1] = players[0].particlePrev[playerRaces[players[0].race].deathParticle[players[0].type]].v[1];
			global::Collision.Collision.floatAr[threadID, 2] = players[0].particlePrev[playerRaces[players[0].race].deathParticle[players[0].type]].v[2];
			global::Collision.Collision.floatAr[threadID, num32++] = players[0].charMain.v1[playerRaces[players[0].race].deathParticle[players[0].type]].v[0];
			global::Collision.Collision.floatAr[threadID, num32++] = players[0].charMain.v1[playerRaces[players[0].race].deathParticle[players[0].type]].v[1];
			global::Collision.Collision.floatAr[threadID, num32++] = players[0].charMain.v1[playerRaces[players[0].race].deathParticle[players[0].type]].v[2];
		}
		Update_Player_BoundingBox(0, global::MainGame.MainGame.playerVehicles[0].ph1.x, global::MainGame.MainGame.playerVehicles[0].ph1.y, global::MainGame.MainGame.playerVehicles[0].ph1.z, threadID);
		px1 = global::MainGame.MainGame.playerVehicles[0].ph1.velocityX;
		py1 = global::MainGame.MainGame.playerVehicles[0].ph1.velocityY;
		pz1 = global::MainGame.MainGame.playerVehicles[0].ph1.velocityZ;
		nx1 = global::MainGame.MainGame.playerVehicles[0].ph1.x;
		ny1 = global::MainGame.MainGame.playerVehicles[0].ph1.y;
		nz1 = global::MainGame.MainGame.playerVehicles[0].ph1.z;
		px2 = velocityX;
		py2 = velocityY;
		pz2 = velocityZ;
		num22 = players[0].charMain.pos1.v[0];
		num23 = players[0].charMain.pos1.v[1];
		num24 = players[0].charMain.pos1.v[2];
		num25 = nx1;
		num26 = ny1;
		num27 = nz1;
		velocityX = px2;
		velocityY = py2;
		velocityZ = pz2;
		global::MainGame.MainGame.playerVehicles[0].ph1.x = nx1;
		global::MainGame.MainGame.playerVehicles[0].ph1.y = ny1;
		global::MainGame.MainGame.playerVehicles[0].ph1.z = nz1;
		global::MainGame.MainGame.playerVehicles[0].ph1.velocityX = px1;
		global::MainGame.MainGame.playerVehicles[0].ph1.velocityY = py1;
		global::MainGame.MainGame.playerVehicles[0].ph1.velocityZ = pz1;
		players[0].charMain.pos1.v[0] = num22;
		players[0].charMain.pos1.v[1] = num23;
		players[0].charMain.pos1.v[2] = num24;
		players[0].charMain.pos2.v[0] = num25;
		players[0].charMain.pos2.v[1] = num26;
		players[0].charMain.pos2.v[2] = num27;
		collisionWithGround = 0f;
		mainC.collisionMain.ResetIgnoreList(threadID, (ushort)num31);
		num31 = 1;
		i = mainC.collisionMain.CheckCollision_Detailed_List(ref players[0].charMain, num31, 0, 143, 0, threadID);
		_ = global::Collision.Collision.colIDT[threadID];
		collisionWithGround += global::Collision.Collision.floatArMovDir[threadID, 0] * (float)(int)global::Collision.Collision.floatArStatus[threadID, 0];
		if (stepOver != 0f && i > 0)
		{
			byte b = 0;
			num14 = players[0].charMain.pos2.v[0] - players[0].charMain.pos1.v[0];
			num12 = players[0].charMain.pos2.v[1] - players[0].charMain.pos1.v[1];
			num17 = (float)Math.Sqrt(num14 * num14 + num12 * num12);
			if (num17 > 1f)
			{
				num14 /= num17;
				num12 /= num17;
				num17 = num28 * num14 + num29 * num12;
				if (num17 >= 1f)
				{
					b = 1;
				}
			}
			if (b == 0)
			{
				players[0].charMain.pos1.v[0] = num22;
				players[0].charMain.pos1.v[1] = num23;
				players[0].charMain.pos1.v[2] = num24 - stepOver;
				players[0].charMain.pos2.v[0] = num25;
				players[0].charMain.pos2.v[1] = num26;
				players[0].charMain.pos2.v[2] = num27;
				collisionWithGround = 0f;
				i = mainC.collisionMain.CheckCollision_Detailed_List(ref players[0].charMain, num31, 0, 143, 0, threadID);
				_ = global::Collision.Collision.colIDT[threadID];
				collisionWithGround += global::Collision.Collision.floatArMovDir[threadID, 0] * (float)(int)global::Collision.Collision.floatArStatus[threadID, 0];
			}
		}
		if (i > 0)
		{
			global::MainGame.MainGame.playerVehicles[0].ph1.forceX = 0f;
			global::MainGame.MainGame.playerVehicles[0].ph1.forceY = 0f;
			global::MainGame.MainGame.playerVehicles[0].ph1.forceZ = 0f;
			num3 = players[0].charMain.pos2.v[0] - players[0].charMain.pos1.v[0];
			num4 = players[0].charMain.pos2.v[1] - players[0].charMain.pos1.v[1];
			num5 = players[0].charMain.pos2.v[2] - players[0].charMain.pos1.v[2];
			num17 = mainC.physicsMain.getTimeForDistanceTraveled(velocityX, velocityY, velocityZ, global::MainGame.MainGame.playerVehicles[0].ph1.accelerationX, global::MainGame.MainGame.playerVehicles[0].ph1.accelerationY, global::MainGame.MainGame.playerVehicles[0].ph1.accelerationZ, num3, num4, num5, frameTime);
			int num33 = 3;
			num17 = frameTime - num17;
			while (num17 > 0f && num33-- > 0)
			{
				float num15 = global::MainGame.MainGame.playerVehicles[0].ph1.velocityX;
				float num16 = global::MainGame.MainGame.playerVehicles[0].ph1.velocityY;
				float num20 = global::MainGame.MainGame.playerVehicles[0].ph1.velocityZ;
				for (i = 0; i < num31; i++)
				{
					if (global::Collision.Collision.floatArStatus[threadID, i] != 1)
					{
						continue;
					}
					num3 = global::Collision.Collision.floatArDir[threadID, i, 0];
					num4 = global::Collision.Collision.floatArDir[threadID, i, 1];
					num5 = global::Collision.Collision.floatArDir[threadID, i, 2];
					if (Math.Abs(num3) < 0.0001f)
					{
						num3 = 0f;
					}
					if (Math.Abs(num4) < 0.0001f)
					{
						num4 = 0f;
					}
					if (Math.Abs(num5) < 0.0001f)
					{
						num5 = 0f;
					}
					num14 = num15 * num3 + num16 * num4 + num20 * num5;
					if ((float)(Math.Sign(num3) * Math.Sign(value)) + num8 < 0f)
					{
						num15 = 0f;
						num3 = 0f;
						num8 = -10f;
					}
					if ((float)(Math.Sign(num4) * Math.Sign(value2)) + num9 < 0f)
					{
						num16 = 0f;
						num4 = 0f;
						num9 = -10f;
					}
					if ((float)(Math.Sign(num5) * Math.Sign(value3)) + num10 < 0f)
					{
						num20 = 0f;
						num5 = 0f;
						num10 = -10f;
					}
					if (num14 < 0f)
					{
						if (num3 != 0f)
						{
							value = num3;
						}
						if (num4 != 0f)
						{
							value2 = num4;
						}
						if (num5 != 0f)
						{
							value3 = num5;
						}
						num15 -= num14 * num3;
						num16 -= num14 * num4;
						num20 -= num14 * num5;
					}
					_ = global::Collision.Collision.floatArID[threadID, i];
				}
				global::MainGame.MainGame.playerVehicles[0].ph1.velocityX = num15;
				global::MainGame.MainGame.playerVehicles[0].ph1.velocityY = num16;
				global::MainGame.MainGame.playerVehicles[0].ph1.velocityZ = num20;
				velocityX = num15;
				velocityY = num16;
				velocityZ = num20;
				global::MainGame.MainGame.playerVehicles[0].ph1.x = players[0].charMain.pos2.v[0];
				global::MainGame.MainGame.playerVehicles[0].ph1.y = players[0].charMain.pos2.v[1];
				global::MainGame.MainGame.playerVehicles[0].ph1.z = players[0].charMain.pos2.v[2];
				players[0].charMain.pos1.v[0] = global::MainGame.MainGame.playerVehicles[0].ph1.x;
				players[0].charMain.pos1.v[1] = global::MainGame.MainGame.playerVehicles[0].ph1.y;
				players[0].charMain.pos1.v[2] = global::MainGame.MainGame.playerVehicles[0].ph1.z;
				mainC.physicsMain.getPosition_new(ref global::MainGame.MainGame.playerVehicles[0].ph1, num17);
				players[0].charMain.pos2.v[0] = global::MainGame.MainGame.playerVehicles[0].ph1.x;
				players[0].charMain.pos2.v[1] = global::MainGame.MainGame.playerVehicles[0].ph1.y;
				players[0].charMain.pos2.v[2] = global::MainGame.MainGame.playerVehicles[0].ph1.z;
				int num34 = mainC.collisionMain.CheckCollision_Detailed_List(ref players[0].charMain, num31, 0, 135, 0, threadID);
				collisionWithGround += global::Collision.Collision.floatArMovDir[threadID, 0] * (float)(int)global::Collision.Collision.floatArStatus[threadID, 0];
				num14 = num17;
				num17 = 0f;
				if (num34 > 0)
				{
					num3 = players[0].charMain.pos2.v[0] - players[0].charMain.pos1.v[0];
					num4 = players[0].charMain.pos2.v[1] - players[0].charMain.pos1.v[1];
					num5 = players[0].charMain.pos2.v[2] - players[0].charMain.pos1.v[2];
					num17 = num14 - mainC.physicsMain.getTimeForDistanceTraveled(velocityX, velocityY, velocityZ, global::MainGame.MainGame.playerVehicles[0].ph1.accelerationX, global::MainGame.MainGame.playerVehicles[0].ph1.accelerationY, global::MainGame.MainGame.playerVehicles[0].ph1.accelerationZ, num3, num4, num5, num14);
				}
			}
		}
		num31 = (short)players[0].charMain.numUsed;
		if (players[0].onmap == 8)
		{
			num31 = 1;
		}
		players[0].charMain.pos1.v[0] = players[0].charMain.pos2.v[0];
		players[0].charMain.pos1.v[1] = players[0].charMain.pos2.v[1];
		players[0].charMain.pos1.v[2] = players[0].charMain.pos2.v[2];
		num10 = players[0].charMain.pos2.v[2];
		players[0].charMain.pos1.v[2] += 3f;
		if (!mainC.collisionMain.CheckCollision_Detailed_List_Final_Pass(ref players[0].charMain, num31, 129, 143, 0, rotationalCheck: true, threadID))
		{
			zRotation = num11;
			ref Matrix reference2 = ref players[0].mv[uBufferID];
			reference2 = Matrix.CreateRotationZ(zRotation * ((float)Math.PI / 180f));
			global::Joints.Joints.Moved_Joint_Calculations(0, global::Joints.Joints.playerJoints[players[0].jointPackage].numJoints);
			mainC.collisionMain.CheckCollision_Detailed_List_Final_Pass(ref players[0].charMain, num31, 129, 143, 0, rotationalCheck: false, threadID);
		}
		if ((num10 = players[0].charMain.pos2.v[2] - num10) < 0f)
		{
			num10 = 0f;
		}
		global::Joints.Joints.Save_Player_Joint_Points(0);
		global::MainGame.MainGame.playerVehicles[0].ph1.x = players[0].charMain.pos2.v[0];
		global::MainGame.MainGame.playerVehicles[0].ph1.y = players[0].charMain.pos2.v[1];
		global::MainGame.MainGame.playerVehicles[0].ph1.z = players[0].charMain.pos2.v[2];
		ref Matrix reference3 = ref global::MainGame.MainGame.playerVehicles[0].mv[uBufferID];
		reference3 = players[0].mv[uBufferID];
		Update_Player_BoundingBox(0, global::MainGame.MainGame.playerVehicles[0].ph1.x, global::MainGame.MainGame.playerVehicles[0].ph1.y, global::MainGame.MainGame.playerVehicles[0].ph1.z, threadID);
		Confine_Player_Position_ToBoundaries_New(ref global::MainGame.MainGame.playerVehicles[0].ph1, mainC.terrainMain.Get_Terrain_Height(global::MainGame.MainGame.playerVehicles[0].ph1.x, global::MainGame.MainGame.playerVehicles[0].ph1.y, threadID), 0);
		players[0].charP.position.v[0] = global::MainGame.MainGame.playerVehicles[0].ph1.x;
		players[0].charP.position.v[1] = global::MainGame.MainGame.playerVehicles[0].ph1.y;
		players[0].charP.position.v[2] = global::MainGame.MainGame.playerVehicles[0].ph1.z;
		players[0].zRotation = zRotation;
		players[0].xRotation = xRotation;
		if (collisionWithGround != 0f || flag || num10 > 0f)
		{
			if (fallingTimer != 0f)
			{
				footStepTimer = 0f;
				mainC.soundsMain.Play_Sound("Footstep0", global::MainGame.MainGame.playerVehicles[0].ph1.x, global::MainGame.MainGame.playerVehicles[0].ph1.y, global::MainGame.MainGame.playerVehicles[0].ph1.z, 0f, 0f, 0f);
			}
			global::MainGame.MainGame.playerVehicles[0].ph1.forceZ = 0f;
			p.x1 = global::MainGame.MainGame.playerVehicles[0].ph1.x;
			p.y1 = global::MainGame.MainGame.playerVehicles[0].ph1.y;
			p.z1 = global::MainGame.MainGame.playerVehicles[0].ph1.z - playerRaces[players[0].race].spawnHeight[players[0].type] + 0.1f;
			p.x2 = p.x1;
			p.y2 = p.y1;
			groundCheckDistance = 0.25f + num10;
			groundCheckDistance += global::MainGame.MainGame.playerVehicles[0].ph1.velocityZ * frameTime * 1f;
			num17 = (float)Math.Sqrt(global::MainGame.MainGame.playerVehicles[0].ph1.velocityX * global::MainGame.MainGame.playerVehicles[0].ph1.velocityX + global::MainGame.MainGame.playerVehicles[0].ph1.velocityY * global::MainGame.MainGame.playerVehicles[0].ph1.velocityY) * frameTime;
			groundCheckDistance += num17 * 0.9801961f * 1f;
			p.z2 = p.z1 - groundCheckDistance - stepOver;
			if (mainC.collisionMain.CheckCollision_Detailed_List_Single(ref p, 0, threadID) > 0)
			{
				global::MainGame.MainGame.playerVehicles[0].ph1.z = global::Collision.Collision.floatArDir[threadID, 0, 6];
				players[0].charP.position.v[2] = global::MainGame.MainGame.playerVehicles[0].ph1.z;
				collisionWithGround = 0f;
			}
			fallingTimer = 0f;
			num17 = (float)Math.Sqrt(global::MainGame.MainGame.playerVehicles[0].ph1.velocityX * global::MainGame.MainGame.playerVehicles[0].ph1.velocityX + global::MainGame.MainGame.playerVehicles[0].ph1.velocityY * global::MainGame.MainGame.playerVehicles[0].ph1.velocityY);
			num14 = 0f;
			num12 = 0f;
			if (num17 != 0f)
			{
				num14 = global::MainGame.MainGame.playerVehicles[0].ph1.velocityX / num17;
				num12 = global::MainGame.MainGame.playerVehicles[0].ph1.velocityY / num17;
			}
			num17 = num28 * num14 + num29 * num12;
			if (num17 < 0.999f && (num28 != 0f || num29 != 0f))
			{
				stepOver += 1.5f;
				if (stepOver > 1.5f)
				{
					stepOver = 0f;
				}
			}
			else
			{
				stepOver = 0f;
			}
		}
		else
		{
			collisionWithGround = 0f;
			footStepTimer = 0f;
			value3 = 0f;
			stepOver = 0f;
			if (players[0].onmap == 4)
			{
				fallingTimer += frameTime;
			}
		}
		players[0].charP.velocity.v[0] = global::MainGame.MainGame.playerVehicles[0].ph1.velocityX;
		players[0].charP.velocity.v[1] = global::MainGame.MainGame.playerVehicles[0].ph1.velocityY;
		players[0].charP.velocity.v[2] = global::MainGame.MainGame.playerVehicles[0].ph1.velocityZ;
		global::MainGame.MainGame.playerVehicles[0].velocity = (float)Math.Sqrt(global::MainGame.MainGame.playerVehicles[0].ph1.velocityX * global::MainGame.MainGame.playerVehicles[0].ph1.velocityX + global::MainGame.MainGame.playerVehicles[0].ph1.velocityY * global::MainGame.MainGame.playerVehicles[0].ph1.velocityY + global::MainGame.MainGame.playerVehicles[0].ph1.velocityZ * global::MainGame.MainGame.playerVehicles[0].ph1.velocityZ);
		Set_Closest_Level_Lights_To_Player();
	}

	public void Move_MainPlayer_Airplane(float frameTime, byte threadID)
	{
		bool flag = false;
		byte b = 3;
		byte b2 = 8;
		byte b3 = 6;
		float num = 0f;
		float num2 = 0f;
		float num3 = 0f;
		float num4 = 0f;
		float num5 = 0f;
		float num6 = 0f;
		float num7 = 0f;
		float num8 = 0f;
		float num9 = 0f;
		float num10 = 0f;
		float num11 = 0f;
		float num12 = 0f;
		float value = 0f;
		float value2 = 0f;
		float value3 = 0f;
		float num13 = 0f;
		float num14 = 0f;
		float num15 = 0f;
		Matrix identity = Matrix.Identity;
		byte uBufferID = global::Rendering.Rendering.uBufferID;
		byte rBufferID = global::Rendering.Rendering.rBufferID;
		if ((players[0].onmap & 0x11) == 0)
		{
			if (global::InputHandler.InputHandler.controllerButtonRightShoulder)
			{
				global::MainGame.MainGame.playerVehicles[0].throttleSpeed += global::MainGame.MainGame.frametime * 0.5f;
			}
			else if (global::InputHandler.InputHandler.controllerButtonLeftShoulder)
			{
				global::MainGame.MainGame.playerVehicles[0].throttleSpeed -= global::MainGame.MainGame.frametime * 0.5f;
			}
			if (global::MainGame.MainGame.playerVehicles[0].throttleSpeed < 0f)
			{
				global::MainGame.MainGame.playerVehicles[0].throttleSpeed = 0f;
			}
			else if (global::MainGame.MainGame.playerVehicles[0].throttleSpeed > 1f)
			{
				global::MainGame.MainGame.playerVehicles[0].throttleSpeed = 1f;
			}
			if (!controlsInUse)
			{
				switch (controllerScheme)
				{
				case 0:
					num5 = global::InputHandler.InputHandler.controllerStickRightValX;
					num4 = (0f - global::InputHandler.InputHandler.controllerStickLeftValueY) * invertY;
					num6 = global::InputHandler.InputHandler.controllerStickLeftValueX;
					break;
				case 1:
					num5 = global::InputHandler.InputHandler.controllerStickLeftValueX;
					num4 = (0f - global::InputHandler.InputHandler.controllerStickLeftValueY) * invertY;
					num6 = global::InputHandler.InputHandler.controllerStickRightValueX;
					break;
				case 2:
					num5 = global::InputHandler.InputHandler.controllerStickRightValX;
					num4 = (0f - global::InputHandler.InputHandler.controllerStickRightValY) * invertY;
					num6 = global::InputHandler.InputHandler.controllerStickLeftValueX;
					break;
				case 3:
					num5 = global::InputHandler.InputHandler.controllerStickLeftValueX;
					num4 = (0f - global::InputHandler.InputHandler.controllerStickRightValY) * invertY;
					num6 = global::InputHandler.InputHandler.controllerStickRightValueX;
					break;
				}
			}
			ushort curVehicle = players[0].curVehicle;
			Vehicles.vehicles[curVehicle].balanceFactor = 0.15f;
			Vehicles.vehicles[curVehicle].wheelTouchingFactor = 0.15f;
			byte numWheels = Vehicles.vehicles[curVehicle].numWheels;
			short numColPoints = Vehicles.vehicles[curVehicle].numColPoints;
			short num16 = (short)(numWheels + numColPoints);
			identity = players[0].mv[rBufferID];
			float terrainHeight = mainC.terrainMain.Get_Terrain_Height(global::MainGame.MainGame.playerVehicles[0].ph1.x, global::MainGame.MainGame.playerVehicles[0].ph1.y, threadID);
			_ = playerSpeedRotateLeftStick;
			_ = global::InputHandler.InputHandler.lookSensitivity[global::InputHandler.InputHandler.lookMode];
			if (players[0].onmap == 8)
			{
				num5 = 0.05f;
				num4 = -0.1f * identity.M33;
			}
			float mass = Vehicles.vehicles[curVehicle].ph1.mass;
			float data = Vehicles.vehicles[curVehicle].data1;
			_ = Vehicles.vehicles[curVehicle].data2;
			_ = Vehicles.vehicles[curVehicle].data3;
			_ = Vehicles.vehicles[curVehicle].data4;
			_ = Vehicles.vehicles[curVehicle].data5;
			float data2 = Vehicles.vehicles[curVehicle].data6;
			float data3 = Vehicles.vehicles[curVehicle].data7;
			_ = Vehicles.vehicles[curVehicle].data8;
			float data4 = Vehicles.vehicles[curVehicle].data9;
			float data5 = Vehicles.vehicles[curVehicle].data10;
			float data6 = Vehicles.vehicles[curVehicle].data11;
			float data7 = Vehicles.vehicles[curVehicle].data12;
			float num17 = 0.03824f * data;
			float num18 = num17 * 2f;
			int num19 = playerRaces[players[0].race].programTurnLeft[players[0].type];
			if (num19 < 255)
			{
				if (global::MainGame.MainGame.activateRetracts)
				{
					global::MainGame.MainGame.activateRetracts = false;
					mainC.programsMain.Set_Animation_To_Reverse_Direction(ref players[0].animations, num19);
				}
				if (players[0].animations[num19].status == 2)
				{
					b = players[0].headJoint;
					b3 = players[0].shoulderJointL;
					b2 = players[0].shoulderJointR;
					mainC.vehicles.Update_Wheel_Positions(ref global::MainGame.MainGame.playerVehicles[0], curVehicle, 0, players[0].jt1[b].rotX, players[0].jt1[b].rotY, players[0].jt1[b].rotZ);
					mainC.vehicles.Update_Wheel_Positions(ref global::MainGame.MainGame.playerVehicles[0], curVehicle, 1, players[0].jt1[b2].rotX, players[0].jt1[b2].rotY, players[0].jt1[b2].rotZ);
					mainC.vehicles.Update_Wheel_Positions(ref global::MainGame.MainGame.playerVehicles[0], curVehicle, 2, players[0].jt1[b3].rotX, players[0].jt1[b3].rotY, players[0].jt1[b3].rotZ);
				}
			}
			if (needToReload)
			{
				if (mainC.weaponsMain.Player_Has_Ammo_For_Weapon(0) > 1)
				{
					byte b4 = (byte)players[0].wpnIndex;
					if (global::Weapons.Weapons.ammo[global::Weapons.Weapons.ammoClips[players[0].weapon2[b4].curClip].ammoIndex].single)
					{
						players[0].weapon2[b4].fired = false;
						mainC.weaponsMain.Load_Ammo_Clip_Into_Player_Weapon(b4, (byte)players[0].primaryWeaponMountWeapon, 0, players[0].ammoClips[players[0].weapon2[b4].curClip].numClips);
					}
					else
					{
						mainC.weaponsMain.Weapon_Reloaded(b4, 0);
					}
				}
				needToReload = false;
				needToChamber = false;
			}
			float num24;
			if ((global::MainGame.MainGame.viewFollowingObject || controlsInUse) && players[0].onmap != 8 && global::MainGame.MainGame.playerVehicles[0].wheelTouchingTimer == 0f)
			{
				if (identity.M23 < 0f)
				{
					num4 = 0.3f;
				}
				else
				{
					float num20 = mainC.terrainMain.Get_Terrain_Height(global::MainGame.MainGame.playerVehicles[0].ph1.x, global::MainGame.MainGame.playerVehicles[0].ph1.y, threadID);
					if (global::MainGame.MainGame.playerVehicles[0].ph1.z < num20 + 500f)
					{
						float num21 = 10f;
						float num22 = mainC.terrainMain.Get_Terrain_Height(global::MainGame.MainGame.playerVehicles[0].ph1.x + 10f * identity.M21, global::MainGame.MainGame.playerVehicles[0].ph1.y + 10f * identity.M22, threadID);
						float num23 = mainC.terrainMain.Get_Terrain_Height(global::MainGame.MainGame.playerVehicles[0].ph1.x + 30f * identity.M21, global::MainGame.MainGame.playerVehicles[0].ph1.y + 30f * identity.M22, threadID);
						if ((num23 - num20) / 30f > (num22 - num20) / 10f)
						{
							num22 = num23;
							num21 = 30f;
						}
						num23 = mainC.terrainMain.Get_Terrain_Height(global::MainGame.MainGame.playerVehicles[0].ph1.x + 60f * identity.M21, global::MainGame.MainGame.playerVehicles[0].ph1.y + 60f * identity.M22, threadID);
						if ((num23 - num20) / 60f > (num22 - num20) / num21)
						{
							num22 = num23;
							num21 = 60f;
						}
						num24 = (float)Math.Sqrt(identity.M21 * identity.M21 + identity.M22 * identity.M22);
						num23 = num21 / num24 * identity.M23;
						if (num23 < num22)
						{
							num4 = 0.3f;
						}
					}
				}
			}
			float num25 = frameTime / global::Physics.Physics.timeMod;
			float num26 = 0.001f;
			while (num25 > 0f)
			{
				if (num26 > num25)
				{
					num26 = num25;
				}
				Matrix matrix = identity;
				players[0].charMain.pos1.v[0] = global::MainGame.MainGame.playerVehicles[0].ph1.x;
				players[0].charMain.pos1.v[1] = global::MainGame.MainGame.playerVehicles[0].ph1.y;
				players[0].charMain.pos1.v[2] = global::MainGame.MainGame.playerVehicles[0].ph1.z;
				float m = matrix.M21;
				float m2 = matrix.M22;
				float m3 = matrix.M23;
				float m4 = matrix.M31;
				float m5 = matrix.M32;
				float m6 = matrix.M33;
				float m7 = matrix.M11;
				float m8 = matrix.M12;
				float m9 = matrix.M13;
				num = (float)Math.Sqrt(global::MainGame.MainGame.playerVehicles[0].ph1.velocityX * global::MainGame.MainGame.playerVehicles[0].ph1.velocityX + global::MainGame.MainGame.playerVehicles[0].ph1.velocityY * global::MainGame.MainGame.playerVehicles[0].ph1.velocityY + global::MainGame.MainGame.playerVehicles[0].ph1.velocityZ * global::MainGame.MainGame.playerVehicles[0].ph1.velocityZ);
				if (num < 1E-11f || num > 1E+10f)
				{
					global::MainGame.MainGame.playerVehicles[0].ph1.velocityX = 0f;
					global::MainGame.MainGame.playerVehicles[0].ph1.velocityY = 0f;
					global::MainGame.MainGame.playerVehicles[0].ph1.velocityZ = 0f;
					num = 0f;
				}
				global::MainGame.MainGame.playerVehicles[0].ph1.velocity = num;
				num7 = 0f;
				num8 = 0f;
				num9 = 0f;
				if (num > 0f)
				{
					num7 = global::MainGame.MainGame.playerVehicles[0].ph1.velocityX / num;
					num8 = global::MainGame.MainGame.playerVehicles[0].ph1.velocityY / num;
					num9 = global::MainGame.MainGame.playerVehicles[0].ph1.velocityZ / num;
				}
				num10 = global::MainGame.MainGame.playerVehicles[0].ph1.velocityX;
				num11 = global::MainGame.MainGame.playerVehicles[0].ph1.velocityY;
				num12 = global::MainGame.MainGame.playerVehicles[0].ph1.velocityZ;
				float num27 = num7 * m4 + num8 * m5 + num9 * m6;
				float num28 = num7 * m + num8 * m2 + num9 * m3;
				float num29 = num7 * m7 + num8 * m8 + num9 * m9;
				num2 = num * num28;
				num3 = num2 * num2;
				Matrix matrix2 = Matrix.CreateRotationY(data7) * matrix;
				float num30 = num7 * matrix2.M11 + num8 * matrix2.M12 + num9 * matrix2.M13;
				Matrix matrix3 = Matrix.CreateRotationY(0f - data7) * matrix;
				float num31 = num7 * matrix3.M11 + num8 * matrix3.M12 + num9 * matrix3.M13;
				num24 = global::MainGame.MainGame.playerVehicles[0].ph1.angularVelocityX * global::MainGame.MainGame.playerVehicles[0].ph1.angularVelocityX * 90f;
				if (num24 / global::MainGame.MainGame.playerVehicles[0].ph1.momentInertiaAxisX * num26 > Math.Abs(global::MainGame.MainGame.playerVehicles[0].ph1.angularVelocityX))
				{
					num24 = global::MainGame.MainGame.playerVehicles[0].ph1.momentInertiaAxisX * (Math.Abs(global::MainGame.MainGame.playerVehicles[0].ph1.angularVelocityX) / num26);
				}
				global::MainGame.MainGame.playerVehicles[0].ph1.torqueX -= (float)Math.Sign(global::MainGame.MainGame.playerVehicles[0].ph1.angularVelocityX) * num24;
				num24 = global::MainGame.MainGame.playerVehicles[0].ph1.angularVelocityY * global::MainGame.MainGame.playerVehicles[0].ph1.angularVelocityY * 40f;
				if (num24 / global::MainGame.MainGame.playerVehicles[0].ph1.momentInertiaAxisY * num26 > Math.Abs(global::MainGame.MainGame.playerVehicles[0].ph1.angularVelocityY))
				{
					num24 = global::MainGame.MainGame.playerVehicles[0].ph1.momentInertiaAxisY * (Math.Abs(global::MainGame.MainGame.playerVehicles[0].ph1.angularVelocityY) / num26);
				}
				global::MainGame.MainGame.playerVehicles[0].ph1.torqueY -= (float)Math.Sign(global::MainGame.MainGame.playerVehicles[0].ph1.angularVelocityY) * num24;
				num24 = global::MainGame.MainGame.playerVehicles[0].ph1.angularVelocityZ * global::MainGame.MainGame.playerVehicles[0].ph1.angularVelocityZ * 90f;
				if (num24 / global::MainGame.MainGame.playerVehicles[0].ph1.momentInertiaAxisZ * num26 > Math.Abs(global::MainGame.MainGame.playerVehicles[0].ph1.angularVelocityZ))
				{
					num24 = global::MainGame.MainGame.playerVehicles[0].ph1.momentInertiaAxisZ * (Math.Abs(global::MainGame.MainGame.playerVehicles[0].ph1.angularVelocityZ) / num26);
				}
				global::MainGame.MainGame.playerVehicles[0].ph1.torqueZ -= (float)Math.Sign(global::MainGame.MainGame.playerVehicles[0].ph1.angularVelocityZ) * num24;
				num24 = num * num29;
				float num32 = global::MainGame.MainGame.playerVehicles[0].ph1.velocityX - m7 * num24;
				float num33 = global::MainGame.MainGame.playerVehicles[0].ph1.velocityY - m8 * num24;
				float num34 = global::MainGame.MainGame.playerVehicles[0].ph1.velocityZ - m9 * num24;
				float num35 = (float)Math.Sqrt(num32 * num32 + num33 * num33 + num34 * num34);
				if (num35 != 0f)
				{
					num32 /= num35;
					num33 /= num35;
					num34 /= num35;
				}
				num35 *= 15f / 22f;
				num35 *= num35;
				float num22 = m * num32 + m2 * num33 + m3 * num34;
				if (Math.Abs(num22) > 1f)
				{
					num22 = Math.Sign(num22);
				}
				float num20 = (float)Math.Acos(num22) * 57.29578f;
				if (num20 > 90f)
				{
					num20 = 180f - num20;
				}
				float num21 = Math.Sign(num32 * m4 + num33 * m5 + num34 * m6);
				if (num21 == 0f)
				{
					num21 = 1f;
				}
				if ((float)Math.Sign(num22) <= 0f)
				{
					num21 *= -1f;
				}
				num22 = (float)Math.Sign(num22) * (1f - Math.Abs(num22));
				num20 = num * num27;
				num24 = Math.Sign(global::MainGame.MainGame.playerVehicles[0].ph1.angularVelocityX);
				global::MainGame.MainGame.playerVehicles[0].ph1.angularVelocityX += num21 * num20 * num20 * num22 * 0.025f * num26;
				if (num24 != (float)Math.Sign(global::MainGame.MainGame.playerVehicles[0].ph1.angularVelocityX))
				{
					global::MainGame.MainGame.playerVehicles[0].ph1.angularVelocityX = 0f;
				}
				float num23;
				if (num28 > 0f)
				{
					num24 = num * num31;
					num32 = global::MainGame.MainGame.playerVehicles[0].ph1.velocityX - matrix3.M11 * num24;
					num33 = global::MainGame.MainGame.playerVehicles[0].ph1.velocityY - matrix3.M12 * num24;
					num34 = global::MainGame.MainGame.playerVehicles[0].ph1.velocityZ - matrix3.M13 * num24;
					num35 = (float)Math.Sqrt(num32 * num32 + num33 * num33 + num34 * num34);
					if (num35 != 0f)
					{
						num32 /= num35;
						num33 /= num35;
						num34 /= num35;
					}
					num35 *= 15f / 22f;
					num35 *= num35;
					num22 = matrix3.M21 * num32 + matrix3.M22 * num33 + matrix3.M23 * num34;
					if (Math.Abs(num22) > 1f)
					{
						num22 = Math.Sign(num22);
					}
					num20 = (float)Math.Acos(num22) * 57.29578f;
					if (num20 > 90f)
					{
						num20 = 180f - num20;
					}
					num21 = mainC.graphingMain.Get_Point_2D_Float_Graph_Fixed_Width_X_Axis(num20, 0f, 0, 1);
					num24 = mainC.graphingMain.Get_Point_2D_Float_Graph_Fixed_Width_X_Axis(num20, 0f, 0, 3);
					num21 = num21 * (1f - global::MainGame.MainGame.flaps) + num24 * global::MainGame.MainGame.flaps;
					num24 = num21 * num18 * num35 * 0.5f;
					if ((float)Math.Sign(num22) <= 0f)
					{
						num24 *= -1f;
					}
					global::MainGame.MainGame.playerVehicles[0].ph1.forceX -= num24 * m;
					global::MainGame.MainGame.playerVehicles[0].ph1.forceY -= num24 * m2;
					global::MainGame.MainGame.playerVehicles[0].ph1.forceZ -= num24 * m3;
					num21 = mainC.graphingMain.Get_Point_2D_Float_Graph_Fixed_Width_X_Axis(num20, 0f, 0, 0);
					num24 = mainC.graphingMain.Get_Point_2D_Float_Graph_Fixed_Width_X_Axis(num20, 0f, 0, 2);
					num21 = num21 * (1f - global::MainGame.MainGame.flaps) + num24 * global::MainGame.MainGame.flaps;
					num24 = num21 * num17 * num35;
					num21 = Math.Sign(num32 * matrix3.M31 + num33 * matrix3.M32 + num34 * matrix3.M33);
					if (num21 == 0f)
					{
						num21 = 1f;
					}
					if ((float)Math.Sign(num22) <= 0f)
					{
						num21 *= -1f;
					}
					num24 *= 0f - num21;
					num23 = num24;
					global::MainGame.MainGame.playerVehicles[0].ph1.forceX += num24 * matrix3.M31;
					global::MainGame.MainGame.playerVehicles[0].ph1.forceY += num24 * matrix3.M32;
					global::MainGame.MainGame.playerVehicles[0].ph1.forceZ += num24 * matrix3.M33;
					num24 = num * num30;
					num32 = global::MainGame.MainGame.playerVehicles[0].ph1.velocityX - matrix2.M11 * num24;
					num33 = global::MainGame.MainGame.playerVehicles[0].ph1.velocityY - matrix2.M12 * num24;
					num34 = global::MainGame.MainGame.playerVehicles[0].ph1.velocityZ - matrix2.M13 * num24;
					num35 = (float)Math.Sqrt(num32 * num32 + num33 * num33 + num34 * num34);
					if (num35 != 0f)
					{
						num32 /= num35;
						num33 /= num35;
						num34 /= num35;
					}
					num35 *= 15f / 22f;
					num35 *= num35;
					num22 = matrix2.M21 * num32 + matrix2.M22 * num33 + matrix2.M23 * num34;
					if (Math.Abs(num22) > 1f)
					{
						num22 = Math.Sign(num22);
					}
					num20 = (float)Math.Acos(num22) * 57.29578f;
					if (num20 > 90f)
					{
						num20 = 180f - num20;
					}
					num21 = mainC.graphingMain.Get_Point_2D_Float_Graph_Fixed_Width_X_Axis(num20, 0f, 0, 1);
					num24 = mainC.graphingMain.Get_Point_2D_Float_Graph_Fixed_Width_X_Axis(num20, 0f, 0, 3);
					num21 = num21 * (1f - global::MainGame.MainGame.flaps) + num24 * global::MainGame.MainGame.flaps;
					num24 = num21 * num18 * num35 * 0.5f;
					if ((float)Math.Sign(num22) <= 0f)
					{
						num24 *= -1f;
					}
					global::MainGame.MainGame.playerVehicles[0].ph1.forceX -= num24 * m;
					global::MainGame.MainGame.playerVehicles[0].ph1.forceY -= num24 * m2;
					global::MainGame.MainGame.playerVehicles[0].ph1.forceZ -= num24 * m3;
					num21 = mainC.graphingMain.Get_Point_2D_Float_Graph_Fixed_Width_X_Axis(num20, 0f, 0, 0);
					num24 = mainC.graphingMain.Get_Point_2D_Float_Graph_Fixed_Width_X_Axis(num20, 0f, 0, 2);
					num21 = num21 * (1f - global::MainGame.MainGame.flaps) + num24 * global::MainGame.MainGame.flaps;
					num24 = num21 * num17 * num35;
					num21 = Math.Sign(num32 * matrix2.M31 + num33 * matrix2.M32 + num34 * matrix2.M33);
					if (num21 == 0f)
					{
						num21 = 1f;
					}
					if ((float)Math.Sign(num22) <= 0f)
					{
						num21 *= -1f;
					}
					num24 *= 0f - num21;
					global::MainGame.MainGame.playerVehicles[0].ph1.forceX += num24 * matrix2.M31;
					global::MainGame.MainGame.playerVehicles[0].ph1.forceY += num24 * matrix2.M32;
					global::MainGame.MainGame.playerVehicles[0].ph1.forceZ += num24 * matrix2.M33;
					global::MainGame.MainGame.playerVehicles[0].ph1.torqueY -= num * (num23 - num24) * 5E-09f;
				}
				num24 = num * num29;
				global::MainGame.MainGame.playerVehicles[0].ph1.torqueZ -= (float)Math.Sign(num24) * num24 * num24 * 0.075f;
				if ((float)(int)global::MainGame.MainGame.gearDown[0] != 0f)
				{
					if (global::MainGame.MainGame.playerVehicles[0].wheelTouchingTimer > 0f)
					{
						num22 = data6 * num28 * num;
						global::MainGame.MainGame.playerVehicles[0].ph1.forceX -= num22 * m;
						global::MainGame.MainGame.playerVehicles[0].ph1.forceY -= num22 * m2;
						global::MainGame.MainGame.playerVehicles[0].ph1.forceZ -= num22 * m3;
						num22 = num * num29;
						global::MainGame.MainGame.playerVehicles[0].ph1.velocityX -= num22 * m7;
						global::MainGame.MainGame.playerVehicles[0].ph1.velocityY -= num22 * m8;
						global::MainGame.MainGame.playerVehicles[0].ph1.velocityZ -= num22 * m9;
					}
					else
					{
						num24 = num * num27;
						num32 = global::MainGame.MainGame.playerVehicles[0].ph1.velocityX - m4 * num24;
						num33 = global::MainGame.MainGame.playerVehicles[0].ph1.velocityY - m5 * num24;
						num34 = global::MainGame.MainGame.playerVehicles[0].ph1.velocityZ - m6 * num24;
						num35 = (float)Math.Sqrt(num32 * num32 + num33 * num33 + num34 * num34);
						if (num35 != 0f)
						{
							num32 /= num35;
							num33 /= num35;
							num34 /= num35;
						}
						num22 = data4 * num35 * num35;
						global::MainGame.MainGame.playerVehicles[0].ph1.forceX -= num22 * num32;
						global::MainGame.MainGame.playerVehicles[0].ph1.forceY -= num22 * num33;
						global::MainGame.MainGame.playerVehicles[0].ph1.forceZ -= num22 * num34;
					}
				}
				float num36 = global::MainGame.MainGame.playerVehicles[0].throttleSpeed * data2 * ((data3 - num2) / data3);
				global::MainGame.MainGame.playerVehicles[0].ph1.forceX += m * num36;
				global::MainGame.MainGame.playerVehicles[0].ph1.forceY += m2 * num36;
				global::MainGame.MainGame.playerVehicles[0].ph1.forceZ += m3 * num36;
				num24 = num * num * 0.464876f;
				num21 = num24 * num28 * data5;
				global::MainGame.MainGame.playerVehicles[0].ph1.forceX -= num21 * m;
				global::MainGame.MainGame.playerVehicles[0].ph1.forceY -= num21 * m2;
				global::MainGame.MainGame.playerVehicles[0].ph1.forceZ -= num21 * m3;
				if (global::MainGame.MainGame.playerVehicles[0].wheelTouchingTimer == 0f)
				{
					num24 = num24 * num27 * data;
					global::MainGame.MainGame.playerVehicles[0].ph1.forceX -= num24 * m4;
					global::MainGame.MainGame.playerVehicles[0].ph1.forceY -= num24 * m5;
					global::MainGame.MainGame.playerVehicles[0].ph1.forceZ -= num24 * m6;
				}
				global::MainGame.MainGame.playerVehicles[0].ph1.forceZ += -32.15223f * mass;
				num24 = 0f - num5;
				num20 = Math.Abs(num24);
				if (num20 > 1f)
				{
					num24 = Math.Sign(num24);
				}
				num22 = (byte)(num20 * 10f) switch
				{
					0 => 0.5f + 0.1f * num20, 
					1 => 0.51f + 0.125f * num20, 
					2 => 0.535f + 0.15f * num20, 
					3 => 0.58f + 0.175f * num20, 
					4 => 0.65f + 0.2f * num20, 
					5 => 0.75f + 0.225f * num20, 
					6 => 1.885f + 0.25f * num20, 
					7 => 3.06f + 0.275f * num20, 
					8 => 4.28f + 0.3f * num20, 
					9 => 5.55f + 0.325f * num20, 
					_ => 6.875f, 
				};
				num21 = num4;
				num20 = Math.Abs(num21);
				if (num20 > 1f)
				{
					num21 = Math.Sign(num21);
				}
				num23 = (byte)(num20 * 10f) switch
				{
					0 => 0.5f + 0.1f * num20, 
					1 => 0.51f + 0.125f * num20, 
					2 => 0.535f + 0.15f * num20, 
					3 => 0.58f + 0.175f * num20, 
					4 => 0.65f + 0.2f * num20, 
					5 => 0.75f + 0.225f * num20, 
					6 => 0.885f + 0.25f * num20, 
					7 => 1.06f + 0.275f * num20, 
					8 => 2.28f + 0.3f * num20, 
					9 => 3.55f + 0.325f * num20, 
					_ => 4.875f, 
				};
				if (global::MainGame.MainGame.playerVehicles[0].wheelTouchingTimer > 0f)
				{
					global::MainGame.MainGame.playerVehicles[0].ph1.torqueZ -= num6 * num6 * num6 * num2 * 0.001f * global::InputHandler.InputHandler.lookSensitivity[global::InputHandler.InputHandler.lookMode];
				}
				else
				{
					global::MainGame.MainGame.playerVehicles[0].ph1.torqueZ -= num6 * num2 * 0.001f * global::InputHandler.InputHandler.lookSensitivity[global::InputHandler.InputHandler.lookMode];
				}
				global::MainGame.MainGame.playerVehicles[0].ph1.torqueY += (0f - num24) * num22 * 0.00045f * num3 * global::InputHandler.InputHandler.lookSensitivity[global::InputHandler.InputHandler.lookMode];
				global::MainGame.MainGame.playerVehicles[0].ph1.torqueX += num21 * num23 * 0.00012f * num3 * global::InputHandler.InputHandler.lookSensitivity[global::InputHandler.InputHandler.lookMode];
				global::MainGame.MainGame.turnPlaneAround = 0f;
				if (float.IsNaN(global::MainGame.MainGame.playerVehicles[0].ph1.forceX) || float.IsNaN(global::MainGame.MainGame.playerVehicles[0].ph1.forceY) || float.IsNaN(global::MainGame.MainGame.playerVehicles[0].ph1.forceZ))
				{
					global::MainGame.MainGame.playerVehicles[0].ph1.forceX = 0f;
					global::MainGame.MainGame.playerVehicles[0].ph1.forceY = 0f;
					global::MainGame.MainGame.playerVehicles[0].ph1.forceZ = 0f;
				}
				mainC.physicsMain.getPosition_new(ref global::MainGame.MainGame.playerVehicles[0].ph1, num26);
				playerRot *= Quaternion.CreateFromYawPitchRoll(global::MainGame.MainGame.playerVehicles[0].ph1.angularVelocityY * num26, global::MainGame.MainGame.playerVehicles[0].ph1.angularVelocityX * num26, global::MainGame.MainGame.playerVehicles[0].ph1.angularVelocityZ * num26);
				Matrix.CreateFromQuaternion(ref playerRot, out identity);
				global::MainGame.MainGame.playerVehicles[0].ph1.forceX = 0f;
				global::MainGame.MainGame.playerVehicles[0].ph1.forceY = 0f;
				global::MainGame.MainGame.playerVehicles[0].ph1.forceZ = 0f;
				global::MainGame.MainGame.playerVehicles[0].ph1.torqueX = 0f;
				global::MainGame.MainGame.playerVehicles[0].ph1.torqueY = 0f;
				global::MainGame.MainGame.playerVehicles[0].ph1.torqueZ = 0f;
				_ = global::MainGame.MainGame.playerVehicles[0].ph1.x;
				_ = players[0].charMain.pos1.v[0];
				_ = global::MainGame.MainGame.playerVehicles[0].ph1.y;
				_ = players[0].charMain.pos1.v[1];
				_ = global::MainGame.MainGame.playerVehicles[0].ph1.z;
				_ = players[0].charMain.pos1.v[2];
				global::MainGame.MainGame.playerVehicles[0].xBalanceTimer -= num26;
				global::MainGame.MainGame.playerVehicles[0].yBalanceTimer -= num26;
				global::MainGame.MainGame.playerVehicles[0].zBalanceTimer -= num26;
				global::MainGame.MainGame.playerVehicles[0].wheelTouchingTimer -= num26;
				if (global::MainGame.MainGame.playerVehicles[0].xBalanceTimer <= 0f)
				{
					global::MainGame.MainGame.playerVehicles[0].xBalanceTimer = 0f;
				}
				if (global::MainGame.MainGame.playerVehicles[0].yBalanceTimer <= 0f)
				{
					global::MainGame.MainGame.playerVehicles[0].yBalanceTimer = 0f;
				}
				if (global::MainGame.MainGame.playerVehicles[0].zBalanceTimer <= 0f)
				{
					global::MainGame.MainGame.playerVehicles[0].zBalanceTimer = 0f;
				}
				if (global::MainGame.MainGame.playerVehicles[0].wheelTouchingTimer <= 0f)
				{
					global::MainGame.MainGame.playerVehicles[0].wheelTouchingTimer = 0f;
				}
				if (global::MainGame.MainGame.playerVehicles[0].wheelTouchingTimer > 0f)
				{
					num24 = Math.Abs(num2 / 200f);
					if (num24 > 1f)
					{
						num24 = 1f;
					}
					else if (num24 < 0.3f)
					{
						num24 = 0f;
					}
					mainC.inputMain.GamePad_Vibration_Set_Low(1f * num24);
				}
				float num37 = 0f;
				collisionObjectID = -1;
				players[0].charMain.pos2.v[0] = global::MainGame.MainGame.playerVehicles[0].ph1.x;
				players[0].charMain.pos2.v[1] = global::MainGame.MainGame.playerVehicles[0].ph1.y;
				players[0].charMain.pos2.v[2] = global::MainGame.MainGame.playerVehicles[0].ph1.z;
				global::MainGame.MainGame.playerVehicles[0].newVelX = global::MainGame.MainGame.playerVehicles[0].ph1.velocityX;
				global::MainGame.MainGame.playerVehicles[0].newVelY = global::MainGame.MainGame.playerVehicles[0].ph1.velocityY;
				global::MainGame.MainGame.playerVehicles[0].newVelZ = global::MainGame.MainGame.playerVehicles[0].ph1.velocityZ;
				int num38 = numWheels * 3;
				short num39 = 0;
				int num40 = 0;
				int num41 = 0;
				int num42 = 129;
				while (num39 < num38)
				{
					num24 = global::MainGame.MainGame.playerVehicles[0].momentum.collisionPoints[num41++];
					num21 = global::MainGame.MainGame.playerVehicles[0].momentum.collisionPoints[num41++];
					num20 = global::MainGame.MainGame.playerVehicles[0].momentum.collisionPoints[num41++];
					global::Collision.Collision.floatAr[threadID, num39++] = num24 * matrix.M11 + num21 * matrix.M21 + num20 * matrix.M31;
					global::Collision.Collision.floatAr[threadID, num39++] = num24 * matrix.M12 + num21 * matrix.M22 + num20 * matrix.M32;
					global::Collision.Collision.floatAr[threadID, num39++] = num24 * matrix.M13 + num21 * matrix.M23 + num20 * matrix.M33;
					global::Collision.Collision.floatAr[threadID, num42++] = num24 * identity.M11 + num21 * identity.M21 + num20 * identity.M31;
					global::Collision.Collision.floatAr[threadID, num42++] = num24 * identity.M12 + num21 * identity.M22 + num20 * identity.M32;
					global::Collision.Collision.floatAr[threadID, num42++] = num24 * identity.M13 + num21 * identity.M23 + num20 * identity.M33;
					num40++;
				}
				num38 = num16 * 3;
				while (num39 < num38)
				{
					num24 = global::MainGame.MainGame.playerVehicles[0].momentum.collisionPoints[num41++];
					num21 = global::MainGame.MainGame.playerVehicles[0].momentum.collisionPoints[num41++];
					num20 = global::MainGame.MainGame.playerVehicles[0].momentum.collisionPoints[num41++];
					global::Collision.Collision.floatAr[threadID, num39++] = num24 * matrix.M11 + num21 * matrix.M21 + num20 * matrix.M31;
					global::Collision.Collision.floatAr[threadID, num39++] = num24 * matrix.M12 + num21 * matrix.M22 + num20 * matrix.M32;
					global::Collision.Collision.floatAr[threadID, num39++] = num24 * matrix.M13 + num21 * matrix.M23 + num20 * matrix.M33;
					global::Collision.Collision.floatAr[threadID, num42++] = num24 * identity.M11 + num21 * identity.M21 + num20 * identity.M31;
					global::Collision.Collision.floatAr[threadID, num42++] = num24 * identity.M12 + num21 * identity.M22 + num20 * identity.M32;
					global::Collision.Collision.floatAr[threadID, num42++] = num24 * identity.M13 + num21 * identity.M23 + num20 * identity.M33;
					num40++;
				}
				Update_Player_Vehicle_BoundingBox(0, threadID, (ushort)num16, ref identity);
				flag = Confine_Player_Position_ToBoundaries_New(ref global::MainGame.MainGame.playerVehicles[0].ph1, terrainHeight, 0) > 0 || flag;
				float num43 = players[0].charMain.pos1.v[0];
				float num44 = players[0].charMain.pos1.v[1];
				float num45 = players[0].charMain.pos1.v[2];
				float num46 = players[0].charMain.pos2.v[0];
				float num47 = players[0].charMain.pos2.v[1];
				float num48 = players[0].charMain.pos2.v[2];
				players[0].charMain.pos1.v[0] = num43;
				players[0].charMain.pos1.v[1] = num44;
				players[0].charMain.pos1.v[2] = num45;
				players[0].charMain.pos2.v[0] = num46;
				players[0].charMain.pos2.v[1] = num47;
				players[0].charMain.pos2.v[2] = num48;
				global::Collision.Collision.hitGround = false;
				mainC.collisionMain.ResetIgnoreList(threadID, (ushort)num16);
				int num49 = mainC.collisionMain.CheckCollision_Detailed_List(ref players[0].charMain, num16, 129, 143, 0, threadID);
				global::MainGame.MainGame.numWheelsTouching = 0;
				for (num19 = 0; num19 < numWheels; num19++)
				{
					if (global::Collision.Collision.floatArStatus[threadID, num19] == 1)
					{
						global::MainGame.MainGame.numWheelsTouching++;
						global::MainGame.MainGame.playerVehicles[0].wheelTouchingTimer = Vehicles.vehicles[curVehicle].wheelTouchingFactor;
					}
				}
				if (num49 > 0)
				{
					flag = true;
					if (num > 100f)
					{
						num39 = 0;
						num40 = 0;
						while (num39 < num38)
						{
							if (global::Collision.Collision.floatArStatus[threadID, num40] == 1)
							{
								mainC.renderingMain.New_Particle_New(2, global::MainGame.MainGame.playerVehicles[0].ph1.x + global::Collision.Collision.floatAr[threadID, num39], global::MainGame.MainGame.playerVehicles[0].ph1.y + global::Collision.Collision.floatAr[threadID, num39 + 1], global::MainGame.MainGame.playerVehicles[0].ph1.z + global::Collision.Collision.floatAr[threadID, num39 + 2], 0f, 0f, global::MainGame.MainGame.playerVehicles[0].ph1.velocityZ, 0, threadID);
							}
							num39 += 3;
							num40++;
						}
					}
					num41 = 0;
					num35 = 0f;
					for (num39 = 0; num39 < num16; num39++)
					{
						if (global::Collision.Collision.floatArID[threadID, num39] > -1)
						{
							collisionObjectID = global::Collision.Collision.floatArID[threadID, num39];
						}
						if (global::Collision.Collision.floatArStatus[threadID, num39] == 1)
						{
							num41++;
							num35 += Math.Abs(global::MainGame.MainGame.playerVehicles[0].ph1.velocityX * global::Collision.Collision.floatArDir[threadID, num39, 0] + global::MainGame.MainGame.playerVehicles[0].ph1.velocityY * global::Collision.Collision.floatArDir[threadID, num39, 1] + global::MainGame.MainGame.playerVehicles[0].ph1.velocityZ * global::Collision.Collision.floatArDir[threadID, num39, 2]);
							if (num39 >= numWheels)
							{
								num35 = (float)num41 * players[0].velocityTerminalThreshold + 100f;
							}
						}
					}
					if (num41 > 0)
					{
						num35 /= (float)num41;
						num37 = num35;
					}
					num32 = players[0].charMain.pos2.v[0] - players[0].charMain.pos1.v[0];
					num33 = players[0].charMain.pos2.v[1] - players[0].charMain.pos1.v[1];
					num34 = players[0].charMain.pos2.v[2] - players[0].charMain.pos1.v[2];
					num24 = mainC.physicsMain.getTimeForDistanceTraveled(num10, num11, num12, global::MainGame.MainGame.playerVehicles[0].ph1.accelerationX, global::MainGame.MainGame.playerVehicles[0].ph1.accelerationY, global::MainGame.MainGame.playerVehicles[0].ph1.accelerationZ, num32, num33, num34, num26);
					mainC.vehicles.Calculate_Vehicle_Collision_Data(ref global::MainGame.MainGame.playerVehicles[0], curVehicle, uBufferID, threadID);
					num19 = 3;
					num24 = num26 - num24;
					while (num24 > 0f && num19-- > 0)
					{
						float num50 = global::MainGame.MainGame.playerVehicles[0].newVelX;
						float num51 = global::MainGame.MainGame.playerVehicles[0].newVelY;
						float num52 = global::MainGame.MainGame.playerVehicles[0].newVelZ;
						for (num39 = 0; num39 < num16; num39++)
						{
							if (global::Collision.Collision.floatArStatus[threadID, num39] == 1)
							{
								num32 = global::Collision.Collision.floatArDir[threadID, num39, 0];
								num33 = global::Collision.Collision.floatArDir[threadID, num39, 1];
								num34 = global::Collision.Collision.floatArDir[threadID, num39, 2];
								if (Math.Abs(num32) < 0.0001f)
								{
									num32 = 0f;
								}
								if (Math.Abs(num33) < 0.0001f)
								{
									num33 = 0f;
								}
								if (Math.Abs(num34) < 0.0001f)
								{
									num34 = 0f;
								}
								num21 = num50 * num32 + num51 * num33 + num52 * num34;
								if ((float)(Math.Sign(num32) * Math.Sign(value)) + num13 < 0f)
								{
									num50 = 0f;
									num32 = 0f;
									num13 = -10f;
								}
								if ((float)(Math.Sign(num33) * Math.Sign(value2)) + num14 < 0f)
								{
									num51 = 0f;
									num33 = 0f;
									num14 = -10f;
								}
								if ((float)(Math.Sign(num34) * Math.Sign(value3)) + num15 < 0f)
								{
									num52 = 0f;
									num34 = 0f;
									num15 = -10f;
								}
								if (num21 < 0f)
								{
									if (num32 != 0f)
									{
										value = num32;
									}
									if (num33 != 0f)
									{
										value2 = num33;
									}
									if (num34 != 0f)
									{
										value3 = num34;
									}
									num50 -= num21 * num32;
									num51 -= num21 * num33;
									num52 -= num21 * num34;
								}
								_ = global::Collision.Collision.floatArID[threadID, num39];
							}
						}
						global::MainGame.MainGame.playerVehicles[0].ph1.velocityX = num50;
						global::MainGame.MainGame.playerVehicles[0].ph1.velocityY = num51;
						global::MainGame.MainGame.playerVehicles[0].ph1.velocityZ = num52;
						global::MainGame.MainGame.playerVehicles[0].ph1.x = players[0].charMain.pos2.v[0];
						global::MainGame.MainGame.playerVehicles[0].ph1.y = players[0].charMain.pos2.v[1];
						global::MainGame.MainGame.playerVehicles[0].ph1.z = players[0].charMain.pos2.v[2];
						players[0].charMain.pos1.v[0] = global::MainGame.MainGame.playerVehicles[0].ph1.x;
						players[0].charMain.pos1.v[1] = global::MainGame.MainGame.playerVehicles[0].ph1.y;
						players[0].charMain.pos1.v[2] = global::MainGame.MainGame.playerVehicles[0].ph1.z;
						num10 = global::MainGame.MainGame.playerVehicles[0].ph1.velocityX;
						num11 = global::MainGame.MainGame.playerVehicles[0].ph1.velocityY;
						num12 = global::MainGame.MainGame.playerVehicles[0].ph1.velocityZ;
						mainC.physicsMain.getPosition_new(ref global::MainGame.MainGame.playerVehicles[0].ph1, num24);
						players[0].charMain.pos2.v[0] = global::MainGame.MainGame.playerVehicles[0].ph1.x;
						players[0].charMain.pos2.v[1] = global::MainGame.MainGame.playerVehicles[0].ph1.y;
						players[0].charMain.pos2.v[2] = global::MainGame.MainGame.playerVehicles[0].ph1.z;
						mainC.collisionMain.ResetIgnoreList(threadID, (ushort)num16);
						num49 = mainC.collisionMain.CheckCollision_Detailed_List(ref players[0].charMain, num16, 129, 135, 0, threadID);
						collisionWithGround += global::Collision.Collision.floatArMovDir[threadID, 0];
						num21 = num24;
						num24 = 0f;
						if (num49 <= 0)
						{
							continue;
						}
						num32 = players[0].charMain.pos2.v[0] - players[0].charMain.pos1.v[0];
						num33 = players[0].charMain.pos2.v[1] - players[0].charMain.pos1.v[1];
						num34 = players[0].charMain.pos2.v[2] - players[0].charMain.pos1.v[2];
						num20 = mainC.physicsMain.getTimeForDistanceTraveled(num10, num11, num12, global::MainGame.MainGame.playerVehicles[0].ph1.accelerationX, global::MainGame.MainGame.playerVehicles[0].ph1.accelerationY, global::MainGame.MainGame.playerVehicles[0].ph1.accelerationZ, num32, num33, num34, num21);
						mainC.vehicles.Calculate_Vehicle_Collision_Data(ref global::MainGame.MainGame.playerVehicles[0], curVehicle, uBufferID, threadID);
						num24 = num21 - num20;
						num41 = 0;
						num22 = 0f;
						for (num39 = 0; num39 < num16; num39++)
						{
							if (global::Collision.Collision.floatArID[threadID, num39] > -1)
							{
								collisionObjectID = global::Collision.Collision.floatArID[threadID, num39];
							}
							if (global::Collision.Collision.floatArStatus[threadID, num39] == 1)
							{
								num41++;
								num22 += Math.Abs(global::MainGame.MainGame.playerVehicles[0].ph1.velocityX * global::Collision.Collision.floatArDir[threadID, num39, 0] + global::MainGame.MainGame.playerVehicles[0].ph1.velocityY * global::Collision.Collision.floatArDir[threadID, num39, 1] + global::MainGame.MainGame.playerVehicles[0].ph1.velocityZ * global::Collision.Collision.floatArDir[threadID, num39, 2]);
							}
						}
						if (num41 > 0)
						{
							num22 /= (float)num41;
							num37 = num22;
						}
					}
					if (num37 > players[0].velocityTerminalThreshold && Player_Injured_Threaded(num37))
					{
						Player_Over(0, playerDied: true, threadID);
						mainC.vehicles.Splash_Damage_From_Vehicle_Explosion(global::MainGame.MainGame.playerVehicles[0].ph1.x, global::MainGame.MainGame.playerVehicles[0].ph1.y, global::MainGame.MainGame.playerVehicles[0].ph1.z + 10f, curVehicle, 0, threadID);
					}
				}
				num = (float)Math.Sqrt(global::MainGame.MainGame.playerVehicles[0].ph1.velocityX * global::MainGame.MainGame.playerVehicles[0].ph1.velocityX + global::MainGame.MainGame.playerVehicles[0].ph1.velocityY * global::MainGame.MainGame.playerVehicles[0].ph1.velocityY + global::MainGame.MainGame.playerVehicles[0].ph1.velocityZ * global::MainGame.MainGame.playerVehicles[0].ph1.velocityZ);
				if (num < 1E-11f || num > 1E+10f)
				{
					global::MainGame.MainGame.playerVehicles[0].ph1.velocityX = 0f;
					global::MainGame.MainGame.playerVehicles[0].ph1.velocityY = 0f;
					global::MainGame.MainGame.playerVehicles[0].ph1.velocityZ = 0f;
					num = 0f;
					num7 = 0f;
					num8 = 0f;
					num9 = 0f;
				}
				global::MainGame.MainGame.playerVehicles[0].ph1.x = players[0].charMain.pos2.v[0];
				global::MainGame.MainGame.playerVehicles[0].ph1.y = players[0].charMain.pos2.v[1];
				global::MainGame.MainGame.playerVehicles[0].ph1.z = players[0].charMain.pos2.v[2];
				num25 -= num26;
			}
			num19 = playerRaces[players[0].race].programTurnRight[players[0].type];
			GameLogic.propRot[0] = 0f;
			if (num19 != 255)
			{
				num24 = 465f + 14900f * global::MainGame.MainGame.playerVehicles[0].throttleSpeed;
				GameLogic.propRot[0] = num24 * frameTime;
				players[0].jt1[num19].pivot2Speed = num24;
				players[0].jt1[num19].targetPivot2 += num24;
				while (players[0].jt1[num19].targetPivot2 > 360f)
				{
					players[0].jt1[num19].targetPivot2 -= 360f;
				}
			}
			players[0].mv[uBufferID] = identity;
			global::Joints.Joints.Reset_Joint_Data(0);
			global::Joints.Joints.Process_Joints_Threaded(0, frameTime, threadID);
			global::MainGame.MainGame.playerVehicles[0].ph1.velocityX = global::MainGame.MainGame.playerVehicles[0].newVelX;
			global::MainGame.MainGame.playerVehicles[0].ph1.velocityY = global::MainGame.MainGame.playerVehicles[0].newVelY;
			global::MainGame.MainGame.playerVehicles[0].ph1.velocityZ = global::MainGame.MainGame.playerVehicles[0].newVelZ;
			players[0].charP.velocity.v[0] = global::MainGame.MainGame.playerVehicles[0].newVelX;
			players[0].charP.velocity.v[1] = global::MainGame.MainGame.playerVehicles[0].newVelY;
			players[0].charP.velocity.v[2] = global::MainGame.MainGame.playerVehicles[0].newVelZ;
			zRotation = (float)Math.Acos((double)identity.M22 / Math.Sqrt(identity.M21 * identity.M21 + identity.M22 * identity.M22)) * 57.29578f;
			if (identity.M21 > 0f)
			{
				zRotation = 360f - zRotation;
			}
			players[0].zRotation = zRotation;
			global::MainGame.MainGame.playerVehicles[0].velocity = num;
			global::MainGame.MainGame.planeVelocity = num;
			Update_Player_Vehicle_BoundingBox(0, threadID, (ushort)num16, ref identity);
			flag = Confine_Player_Position_ToBoundaries_New(ref global::MainGame.MainGame.playerVehicles[0].ph1, terrainHeight, 0) > 0 || flag;
			players[0].charP.position.v[0] = global::MainGame.MainGame.playerVehicles[0].ph1.x;
			players[0].charP.position.v[1] = global::MainGame.MainGame.playerVehicles[0].ph1.y;
			players[0].charP.position.v[2] = global::MainGame.MainGame.playerVehicles[0].ph1.z;
			if (global::Rendering.Rendering.watchingPlayer == 0)
			{
				mainC.inputMain.UI_HUD_Set_Player_Velocity(num2);
			}
			else
			{
				mainC.inputMain.UI_HUD_Set_Player_Velocity(global::MainGame.MainGame.playerVehicles[global::Rendering.Rendering.watchingPlayer].ph1.velocity);
			}
			mainC.inputMain.UI_HUD_Set_Player_Height(global::MainGame.MainGame.playerVehicles[global::Rendering.Rendering.watchingPlayer].ph1.z);
			num24 = num2 / data3 * 0.8f;
			num24 = global::MainGame.MainGame.playerVehicles[0].throttleSpeed * 0.4f + num24;
			mainC.soundsMain.Play_Moving_Continual_Sound(0, 1, stop: false, 0.5f + global::MainGame.MainGame.playerVehicles[0].throttleSpeed * 0.5f, num24 * 100f, global::MainGame.MainGame.playerVehicles[0].ph1.x, global::MainGame.MainGame.playerVehicles[0].ph1.y, global::MainGame.MainGame.playerVehicles[0].ph1.z, global::MainGame.MainGame.playerVehicles[0].ph1.velocityX, global::MainGame.MainGame.playerVehicles[0].ph1.velocityY, global::MainGame.MainGame.playerVehicles[0].ph1.velocityZ);
			if (players[0].damagePercentageCapped > 0.1f)
			{
				Damage_Particles_For_Damaged_Player_Vehicle(0, threadID);
			}
		}
		if ((players[0].onmap & 0x18) > 0)
		{
			if (mainPlayerDeathTimer == 0f)
			{
				if (players[0].damage >= global::MainGame.MainGame.playerVehicles[0].damageThresholdForExplosion)
				{
					flag = true;
				}
				if (flag)
				{
					if (global::MainGame.MainGame.gameMode == 1)
					{
						mainC.networkingMain.XBOX_Send_Network_Message53(53);
					}
					mainC.soundsMain.Stop_Continual_Sound(0);
					mainC.renderingMain.New_Particle_New(16, global::MainGame.MainGame.playerVehicles[0].ph1.x, global::MainGame.MainGame.playerVehicles[0].ph1.y, global::MainGame.MainGame.playerVehicles[0].ph1.z, 1f, 0f, 0f, 0, threadID);
					mainC.soundsMain.Play_Priority_Sound("Airplane_Crash_MainPlayer", players[0].charP.position.v[0], players[0].charP.position.v[1], players[0].charP.position.v[2], 0f, 0f, 0f);
					mainC.vehicles.Splash_Damage_From_Vehicle_Explosion(global::MainGame.MainGame.playerVehicles[0].ph1.x, global::MainGame.MainGame.playerVehicles[0].ph1.y, global::MainGame.MainGame.playerVehicles[0].ph1.z + 10f, players[0].curVehicle, 0, threadID);
					mainPlayerDeathTimer += frameTime / global::Physics.Physics.timeMod;
					global::MainGame.MainGame.playerVehicles[0].ph1.x = players[0].posX[rBufferID];
					global::MainGame.MainGame.playerVehicles[0].ph1.y = players[0].posY[rBufferID];
					global::MainGame.MainGame.playerVehicles[0].ph1.z = players[0].posZ[rBufferID];
					players[0].charP.position.v[0] = players[0].posX[rBufferID];
					players[0].charP.position.v[1] = players[0].posY[rBufferID];
					players[0].charP.position.v[2] = players[0].posZ[rBufferID];
					ref Matrix reference = ref players[0].mv[uBufferID];
					reference = players[0].mv[rBufferID];
					mainC.gameLogic.Game_Airplane_Crashed();
					players[0].onmap = 16;
				}
			}
			else if (mainPlayerDeathTimer > 0f)
			{
				mainPlayerDeathTimer += frameTime / global::Physics.Physics.timeMod;
				global::MainGame.MainGame.playerVehicles[0].ph1.velocityX = players[0].posX[rBufferID];
				global::MainGame.MainGame.playerVehicles[0].ph1.velocityY = players[0].posY[rBufferID];
				global::MainGame.MainGame.playerVehicles[0].ph1.velocityZ = players[0].posZ[rBufferID];
				players[0].charP.position.v[0] = players[0].posX[rBufferID];
				players[0].charP.position.v[1] = players[0].posY[rBufferID];
				players[0].charP.position.v[2] = players[0].posZ[rBufferID];
				ref Matrix reference2 = ref players[0].mv[uBufferID];
				reference2 = players[0].mv[rBufferID];
				Sync_Local_Player_View();
				if (mainPlayerDeathTimer > 2.5f)
				{
					players[0].onmap = 1;
				}
			}
		}
		global::MainGame.MainGame.debugUpdateCrashCount = 0;
	}

	public void Move_MainPlayer_ArcadeStyle_Airplane(float frameTime, byte threadID)
	{
		bool flag = false;
		byte b = 3;
		byte b2 = 8;
		byte b3 = 6;
		float num = 1f;
		float num2 = 0f;
		float num3 = 0f;
		float num4 = 0f;
		float num5 = 0f;
		float num6 = 0f;
		float num7 = 0f;
		float num8 = 0f;
		float num9 = 0f;
		float num10 = 0f;
		float num11 = 0f;
		float value = 0f;
		float value2 = 0f;
		float value3 = 0f;
		float num12 = 0f;
		float num13 = 0f;
		float num14 = 0f;
		Matrix identity = Matrix.Identity;
		byte uBufferID = global::Rendering.Rendering.uBufferID;
		byte rBufferID = global::Rendering.Rendering.rBufferID;
		if ((players[0].onmap & 0x11) == 0)
		{
			ushort curVehicle = players[0].curVehicle;
			Vehicles.vehicles[curVehicle].balanceFactor = 0.15f;
			Vehicles.vehicles[curVehicle].wheelTouchingFactor = 0.15f;
			byte numWheels = Vehicles.vehicles[curVehicle].numWheels;
			short numColPoints = Vehicles.vehicles[curVehicle].numColPoints;
			short num15 = (short)(numWheels + numColPoints);
			identity = players[0].mv[rBufferID];
			float terrainHeight = mainC.terrainMain.Get_Terrain_Height(global::MainGame.MainGame.playerVehicles[0].ph1.x, global::MainGame.MainGame.playerVehicles[0].ph1.y, threadID);
			_ = Vehicles.vehicles[curVehicle].ph1.mass;
			_ = Vehicles.vehicles[curVehicle].data1;
			_ = Vehicles.vehicles[curVehicle].data2;
			_ = Vehicles.vehicles[curVehicle].data3;
			_ = Vehicles.vehicles[curVehicle].data4;
			_ = Vehicles.vehicles[curVehicle].data5;
			_ = Vehicles.vehicles[curVehicle].data6;
			float data = Vehicles.vehicles[curVehicle].data7;
			_ = Vehicles.vehicles[curVehicle].data8;
			_ = Vehicles.vehicles[curVehicle].data9;
			_ = Vehicles.vehicles[curVehicle].data10;
			_ = Vehicles.vehicles[curVehicle].data11;
			_ = Vehicles.vehicles[curVehicle].data12;
			float data2 = Vehicles.vehicles[curVehicle].data16;
			global::MainGame.MainGame.playerVehicles[0].throttleSpeed = (global::InputHandler.InputHandler.controllerStickLeftValueY + 1f) * 0.75f;
			global::Rendering.Rendering.cameraSpringDistance += (global::MainGame.MainGame.playerVehicles[0].throttleSpeed - global::Rendering.Rendering.cameraSpringDistance) * 0.75f * global::MainGame.MainGame.frametime;
			float damage;
			if (global::InputHandler.InputHandler.controllerStickLeftValueY > 0.25f)
			{
				global::MainGame.MainGame.playerVehicles[0].curHeat += global::InputHandler.InputHandler.controllerStickLeftValueY * global::MainGame.MainGame.playerVehicles[0].heatGeneration * frameTime;
				if (global::MainGame.MainGame.playerVehicles[0].curHeat > (float)(int)global::MainGame.MainGame.playerVehicles[0].maxHeat)
				{
					global::MainGame.MainGame.playerVehicles[0].curHeat = (int)global::MainGame.MainGame.playerVehicles[0].maxHeat;
					if (players[0].damagePercentageCapped < 0.95f)
					{
						damage = global::MainGame.MainGame.playerVehicles[0].overHeatingDamage * frameTime;
						if (Player_Vehicle_Damaged(damage))
						{
							Player_Over(0, playerDied: true, threadID);
							mainC.vehicles.Splash_Damage_From_Vehicle_Explosion(global::MainGame.MainGame.playerVehicles[0].ph1.x, global::MainGame.MainGame.playerVehicles[0].ph1.y, global::MainGame.MainGame.playerVehicles[0].ph1.z + 10f, curVehicle, 0, threadID);
						}
					}
				}
			}
			else
			{
				global::MainGame.MainGame.playerVehicles[0].curHeat -= (1f - global::InputHandler.InputHandler.controllerStickLeftValueY * 0.5f) * global::MainGame.MainGame.playerVehicles[0].heatDissipation * frameTime;
				if (global::MainGame.MainGame.playerVehicles[0].curHeat < 0f)
				{
					global::MainGame.MainGame.playerVehicles[0].curHeat = 0f;
				}
			}
			global::MainGame.MainGame.playerVehicles[0].throttleSpeed += 0.5f;
			if (!controlsInUse)
			{
				num = Set_Speed_If_Targeting_Enemy(0, frameTime, 225f, 14400f, 300f, doCollisionCheck: false, global::InputHandler.InputHandler.controllerStickRightValX, threadID);
				float num16 = 0.5f * frameTime * frameTime;
				damage = global::InputHandler.InputHandler.controllerStickRightValueX - global::InputHandler.InputHandler.stickRightX;
				float num17 = damage * global::MainGame.MainGame.playerVehicles[0].controllerSpring - global::MainGame.MainGame.playerVehicles[0].controllerDampening * global::InputHandler.InputHandler.stickRightXVel;
				global::InputHandler.InputHandler.stickRightX += global::InputHandler.InputHandler.stickRightXVel * frameTime + num17 * num16;
				global::InputHandler.InputHandler.stickRightXVel += num17 * frameTime;
				num5 = global::InputHandler.InputHandler.stickRightX;
				damage = (Math.Abs(global::InputHandler.InputHandler.stickRightX) - global::InputHandler.InputHandler.controllerStickRightSmoothX) * frameTime;
				damage = ((!(damage > 0f)) ? (damage * 5f) : (damage * (1f + Math.Abs(global::MainGame.MainGame.arcadeModeRotAngle[0] / ((float)Math.PI / 2f)) * 0.5f + Math.Abs(global::InputHandler.InputHandler.stickRightX) * 0.5f)));
				global::InputHandler.InputHandler.controllerStickRightSmoothX += damage;
				if (global::InputHandler.InputHandler.controllerStickRightSmoothX > 1f)
				{
					global::InputHandler.InputHandler.controllerStickRightSmoothX = 1f;
				}
				else if (global::InputHandler.InputHandler.controllerStickRightSmoothX < 0f)
				{
					global::InputHandler.InputHandler.controllerStickRightSmoothX = 0f;
				}
				num5 *= 0.05f + global::InputHandler.InputHandler.controllerStickRightSmoothX * 0.95f;
				num5 += (global::InputHandler.InputHandler.controllerStickRightValueX - num5) * global::InputHandler.InputHandler.lookSensitivity[global::InputHandler.InputHandler.lookMode];
				damage = global::InputHandler.InputHandler.controllerStickRightValueY - global::InputHandler.InputHandler.stickRightY;
				num17 = damage * global::MainGame.MainGame.playerVehicles[0].controllerSpring - global::MainGame.MainGame.playerVehicles[0].controllerDampening * global::InputHandler.InputHandler.stickRightYVel;
				global::InputHandler.InputHandler.stickRightY += global::InputHandler.InputHandler.stickRightYVel * frameTime + num17 * num16;
				global::InputHandler.InputHandler.stickRightYVel += num17 * frameTime;
				num4 = (0f - global::InputHandler.InputHandler.stickRightY) * invertY;
				damage = (Math.Abs(global::InputHandler.InputHandler.stickRightY) - global::InputHandler.InputHandler.controllerStickRightSmoothY) * frameTime;
				damage = ((!(damage > 0f)) ? (damage * 5f) : (damage * (1f + Math.Abs(global::MainGame.MainGame.arcadeModeRisingAngle[0] / ((float)Math.PI * 89f / 180f)) * 0.5f + Math.Abs(global::InputHandler.InputHandler.stickRightY) * 0.5f)));
				global::InputHandler.InputHandler.controllerStickRightSmoothY += damage;
				if (global::InputHandler.InputHandler.controllerStickRightSmoothY > 1f)
				{
					global::InputHandler.InputHandler.controllerStickRightSmoothY = 1f;
				}
				else if (global::InputHandler.InputHandler.controllerStickRightSmoothY < 0f)
				{
					global::InputHandler.InputHandler.controllerStickRightSmoothY = 0f;
				}
				num4 *= 0.05f + global::InputHandler.InputHandler.controllerStickRightSmoothY * 0.95f;
				num4 += ((0f - global::InputHandler.InputHandler.controllerStickRightValueY) * invertY - num4) * global::InputHandler.InputHandler.lookSensitivity[global::InputHandler.InputHandler.lookMode] * 0.5f;
			}
			if (players[0].onmap == 8)
			{
				num5 = 0.05f * (float)Math.Sign(global::MainGame.MainGame.arcadeModeRotAngle[0]);
				num4 = -0.3f * Math.Abs(-1f - identity.M23);
			}
			int num18 = playerRaces[players[0].race].programTurnLeft[players[0].type];
			if (num18 < 255)
			{
				if (global::MainGame.MainGame.activateRetracts)
				{
					global::MainGame.MainGame.activateRetracts = false;
					mainC.programsMain.Set_Animation_To_Reverse_Direction(ref players[0].animations, num18);
				}
				if (players[0].animations[num18].status == 2)
				{
					b = players[0].headJoint;
					b3 = players[0].shoulderJointL;
					b2 = players[0].shoulderJointR;
					mainC.vehicles.Update_Wheel_Positions(ref global::MainGame.MainGame.playerVehicles[0], curVehicle, 0, players[0].jt1[b].rotX, players[0].jt1[b].rotY, players[0].jt1[b].rotZ);
					mainC.vehicles.Update_Wheel_Positions(ref global::MainGame.MainGame.playerVehicles[0], curVehicle, 1, players[0].jt1[b2].rotX, players[0].jt1[b2].rotY, players[0].jt1[b2].rotZ);
					mainC.vehicles.Update_Wheel_Positions(ref global::MainGame.MainGame.playerVehicles[0], curVehicle, 2, players[0].jt1[b3].rotX, players[0].jt1[b3].rotY, players[0].jt1[b3].rotZ);
				}
			}
			if (needToReload)
			{
				if (!reloading)
				{
					reloading = mainC.playersMain.Player_Needs_To_Reload(0);
				}
				mainC.weaponsMain.Check_Weapon_Views();
				needToReload = false;
			}
			else if (needToChamber)
			{
				if (!chambering)
				{
					chambering = Player_Needs_To_Chamber(0);
				}
				mainC.weaponsMain.Check_Weapon_Views();
				needToChamber = false;
			}
			if ((global::MainGame.MainGame.viewFollowingObject || controlsInUse) && players[0].onmap != 8 && global::MainGame.MainGame.playerVehicles[0].wheelTouchingTimer == 0f)
			{
				if (identity.M23 < 0f)
				{
					num4 = 0.3f;
				}
				else
				{
					float num16 = mainC.terrainMain.Get_Terrain_Height(global::MainGame.MainGame.playerVehicles[0].ph1.x, global::MainGame.MainGame.playerVehicles[0].ph1.y, threadID);
					if (global::MainGame.MainGame.playerVehicles[0].ph1.z < num16 + 500f)
					{
						float num17 = 10f;
						float num19 = mainC.terrainMain.Get_Terrain_Height(global::MainGame.MainGame.playerVehicles[0].ph1.x + 10f * identity.M21, global::MainGame.MainGame.playerVehicles[0].ph1.y + 10f * identity.M22, threadID);
						float num20 = mainC.terrainMain.Get_Terrain_Height(global::MainGame.MainGame.playerVehicles[0].ph1.x + 30f * identity.M21, global::MainGame.MainGame.playerVehicles[0].ph1.y + 30f * identity.M22, threadID);
						if ((num20 - num16) / 30f > (num19 - num16) / 10f)
						{
							num19 = num20;
							num17 = 30f;
						}
						num20 = mainC.terrainMain.Get_Terrain_Height(global::MainGame.MainGame.playerVehicles[0].ph1.x + 60f * identity.M21, global::MainGame.MainGame.playerVehicles[0].ph1.y + 60f * identity.M22, threadID);
						if ((num20 - num16) / 60f > (num19 - num16) / num17)
						{
							num19 = num20;
							num17 = 60f;
						}
						damage = (float)Math.Sqrt(identity.M21 * identity.M21 + identity.M22 * identity.M22);
						num20 = num17 / damage * identity.M23;
						if (num20 < num19)
						{
							num4 = 0.3f;
						}
					}
				}
			}
			float num21 = frameTime / global::Physics.Physics.timeMod;
			float num22 = num21;
			while (num21 > 0f)
			{
				if (num22 > num21)
				{
					num22 = num21;
				}
				Matrix matrix = identity;
				players[0].charMain.pos1.v[0] = global::MainGame.MainGame.playerVehicles[0].ph1.x;
				players[0].charMain.pos1.v[1] = global::MainGame.MainGame.playerVehicles[0].ph1.y;
				players[0].charMain.pos1.v[2] = global::MainGame.MainGame.playerVehicles[0].ph1.z;
				float m = matrix.M21;
				float m2 = matrix.M22;
				float m3 = matrix.M23;
				float m4 = matrix.M31;
				float m5 = matrix.M32;
				float m6 = matrix.M33;
				float m7 = matrix.M11;
				float m8 = matrix.M12;
				float m9 = matrix.M13;
				num2 = (float)Math.Sqrt(global::MainGame.MainGame.playerVehicles[0].ph1.velocityX * global::MainGame.MainGame.playerVehicles[0].ph1.velocityX + global::MainGame.MainGame.playerVehicles[0].ph1.velocityY * global::MainGame.MainGame.playerVehicles[0].ph1.velocityY + global::MainGame.MainGame.playerVehicles[0].ph1.velocityZ * global::MainGame.MainGame.playerVehicles[0].ph1.velocityZ);
				if (num2 < 1E-11f || num2 > 1E+10f)
				{
					global::MainGame.MainGame.playerVehicles[0].ph1.velocityX = 0f;
					global::MainGame.MainGame.playerVehicles[0].ph1.velocityY = 0f;
					global::MainGame.MainGame.playerVehicles[0].ph1.velocityZ = 0f;
					num2 = 0f;
				}
				global::MainGame.MainGame.playerVehicles[0].ph1.velocity = num2;
				num6 = 0f;
				num7 = 0f;
				num8 = 0f;
				if (num2 > 0f)
				{
					num6 = global::MainGame.MainGame.playerVehicles[0].ph1.velocityX / num2;
					num7 = global::MainGame.MainGame.playerVehicles[0].ph1.velocityY / num2;
					num8 = global::MainGame.MainGame.playerVehicles[0].ph1.velocityZ / num2;
				}
				num9 = global::MainGame.MainGame.playerVehicles[0].ph1.velocityX;
				num10 = global::MainGame.MainGame.playerVehicles[0].ph1.velocityY;
				num11 = global::MainGame.MainGame.playerVehicles[0].ph1.velocityZ;
				float num23 = num6 * m + num7 * m2 + num8 * m3;
				num3 = num2 * num23;
				damage = global::MainGame.MainGame.playerVehicles[0].throttleSpeed * data - num2;
				num2 += damage * global::MainGame.MainGame.playerVehicles[0].accelerationFactor * num22;
				global::MainGame.MainGame.arcadeModeRotAngle[0] += num5 * (4f + global::InputHandler.InputHandler.lookSensitivity[global::InputHandler.InputHandler.lookMode]) * num22;
				global::MainGame.MainGame.arcadeModeRisingAngle[0] += num4 * 1.5f * num22;
				if (Math.Abs(global::MainGame.MainGame.arcadeModeRotAngle[0]) > (float)Math.PI / 2f)
				{
					global::InputHandler.InputHandler.stickRightXVel = 0f;
					global::InputHandler.InputHandler.stickRightX = global::InputHandler.InputHandler.controllerStickRightValueX;
					global::MainGame.MainGame.arcadeModeRotAngle[0] = (float)Math.PI / 2f * (float)Math.Sign(global::MainGame.MainGame.arcadeModeRotAngle[0]);
				}
				if (Math.Abs(global::MainGame.MainGame.arcadeModeRisingAngle[0]) > (float)Math.PI * 89f / 180f)
				{
					global::InputHandler.InputHandler.stickRightYVel = 0f;
					global::InputHandler.InputHandler.stickRightY = global::InputHandler.InputHandler.controllerStickRightValueY;
					global::MainGame.MainGame.arcadeModeRisingAngle[0] = (float)Math.PI * 89f / 180f * (float)Math.Sign(global::MainGame.MainGame.arcadeModeRisingAngle[0]);
				}
				if (global::InputHandler.InputHandler.controllerStickRightValueX == 0f && Math.Abs(global::MainGame.MainGame.arcadeModeRotAngle[0]) < 0.08726646f)
				{
					global::MainGame.MainGame.arcadeModeRotAngle[0] -= global::MainGame.MainGame.arcadeModeRotAngle[0] * 0.85f * frameTime;
				}
				global::MainGame.MainGame.playerVehicles[0].ph1.torqueX = 0f;
				global::MainGame.MainGame.playerVehicles[0].ph1.torqueY = 0f;
				global::MainGame.MainGame.playerVehicles[0].ph1.torqueZ = 0f;
				players[0].zRotation -= 90f * (global::MainGame.MainGame.arcadeModeRotAngle[0] / ((float)Math.PI / 2f)) * data2 * num22 * num;
				identity = Matrix.CreateRotationY(global::MainGame.MainGame.arcadeModeRotAngle[0]) * Matrix.CreateRotationX(global::MainGame.MainGame.arcadeModeRisingAngle[0]) * Matrix.CreateRotationZ(players[0].zRotation * ((float)Math.PI / 180f));
				global::MainGame.MainGame.playerVehicles[0].ph1.velocityX = identity.M21 * num2;
				global::MainGame.MainGame.playerVehicles[0].ph1.velocityY = identity.M22 * num2;
				global::MainGame.MainGame.playerVehicles[0].ph1.velocityZ = identity.M23 * num2;
				mainC.physicsMain.getPosition_new(ref global::MainGame.MainGame.playerVehicles[0].ph1, num22);
				global::MainGame.MainGame.playerVehicles[0].ph1.forceX = 0f;
				global::MainGame.MainGame.playerVehicles[0].ph1.forceY = 0f;
				global::MainGame.MainGame.playerVehicles[0].ph1.forceZ = 0f;
				global::MainGame.MainGame.playerVehicles[0].ph1.torqueX = 0f;
				global::MainGame.MainGame.playerVehicles[0].ph1.torqueY = 0f;
				global::MainGame.MainGame.playerVehicles[0].ph1.torqueZ = 0f;
				_ = global::MainGame.MainGame.playerVehicles[0].ph1.x;
				_ = players[0].charMain.pos1.v[0];
				_ = global::MainGame.MainGame.playerVehicles[0].ph1.y;
				_ = players[0].charMain.pos1.v[1];
				_ = global::MainGame.MainGame.playerVehicles[0].ph1.z;
				_ = players[0].charMain.pos1.v[2];
				global::MainGame.MainGame.playerVehicles[0].xBalanceTimer -= num22;
				global::MainGame.MainGame.playerVehicles[0].yBalanceTimer -= num22;
				global::MainGame.MainGame.playerVehicles[0].zBalanceTimer -= num22;
				global::MainGame.MainGame.playerVehicles[0].wheelTouchingTimer -= num22;
				if (global::MainGame.MainGame.playerVehicles[0].xBalanceTimer <= 0f)
				{
					global::MainGame.MainGame.playerVehicles[0].xBalanceTimer = 0f;
				}
				if (global::MainGame.MainGame.playerVehicles[0].yBalanceTimer <= 0f)
				{
					global::MainGame.MainGame.playerVehicles[0].yBalanceTimer = 0f;
				}
				if (global::MainGame.MainGame.playerVehicles[0].zBalanceTimer <= 0f)
				{
					global::MainGame.MainGame.playerVehicles[0].zBalanceTimer = 0f;
				}
				if (global::MainGame.MainGame.playerVehicles[0].wheelTouchingTimer <= 0f)
				{
					global::MainGame.MainGame.playerVehicles[0].wheelTouchingTimer = 0f;
				}
				if (global::MainGame.MainGame.playerVehicles[0].wheelTouchingTimer > 0f)
				{
					damage = Math.Abs(num3 / 200f);
					if (damage > 1f)
					{
						damage = 1f;
					}
					else if (damage < 0.3f)
					{
						damage = 0f;
					}
					mainC.inputMain.GamePad_Vibration_Set_Low(1f * damage);
				}
				float num24 = 0f;
				collisionObjectID = -1;
				players[0].charMain.pos2.v[0] = global::MainGame.MainGame.playerVehicles[0].ph1.x;
				players[0].charMain.pos2.v[1] = global::MainGame.MainGame.playerVehicles[0].ph1.y;
				players[0].charMain.pos2.v[2] = global::MainGame.MainGame.playerVehicles[0].ph1.z;
				global::MainGame.MainGame.playerVehicles[0].newVelX = global::MainGame.MainGame.playerVehicles[0].ph1.velocityX;
				global::MainGame.MainGame.playerVehicles[0].newVelY = global::MainGame.MainGame.playerVehicles[0].ph1.velocityY;
				global::MainGame.MainGame.playerVehicles[0].newVelZ = global::MainGame.MainGame.playerVehicles[0].ph1.velocityZ;
				int num25 = numWheels * 3;
				short num26 = 0;
				int num27 = 0;
				int num28 = 0;
				int num29 = 129;
				while (num26 < num25)
				{
					damage = global::MainGame.MainGame.playerVehicles[0].momentum.collisionPoints[num28++];
					float num17 = global::MainGame.MainGame.playerVehicles[0].momentum.collisionPoints[num28++];
					float num16 = global::MainGame.MainGame.playerVehicles[0].momentum.collisionPoints[num28++];
					global::Collision.Collision.floatAr[threadID, num26++] = damage * matrix.M11 + num17 * matrix.M21 + num16 * matrix.M31;
					global::Collision.Collision.floatAr[threadID, num26++] = damage * matrix.M12 + num17 * matrix.M22 + num16 * matrix.M32;
					global::Collision.Collision.floatAr[threadID, num26++] = damage * matrix.M13 + num17 * matrix.M23 + num16 * matrix.M33;
					global::Collision.Collision.floatAr[threadID, num29++] = damage * identity.M11 + num17 * identity.M21 + num16 * identity.M31;
					global::Collision.Collision.floatAr[threadID, num29++] = damage * identity.M12 + num17 * identity.M22 + num16 * identity.M32;
					global::Collision.Collision.floatAr[threadID, num29++] = damage * identity.M13 + num17 * identity.M23 + num16 * identity.M33;
					num27++;
				}
				num25 = num15 * 3;
				while (num26 < num25)
				{
					damage = global::MainGame.MainGame.playerVehicles[0].momentum.collisionPoints[num28++];
					float num17 = global::MainGame.MainGame.playerVehicles[0].momentum.collisionPoints[num28++];
					float num16 = global::MainGame.MainGame.playerVehicles[0].momentum.collisionPoints[num28++];
					global::Collision.Collision.floatAr[threadID, num26++] = damage * matrix.M11 + num17 * matrix.M21 + num16 * matrix.M31;
					global::Collision.Collision.floatAr[threadID, num26++] = damage * matrix.M12 + num17 * matrix.M22 + num16 * matrix.M32;
					global::Collision.Collision.floatAr[threadID, num26++] = damage * matrix.M13 + num17 * matrix.M23 + num16 * matrix.M33;
					global::Collision.Collision.floatAr[threadID, num29++] = damage * identity.M11 + num17 * identity.M21 + num16 * identity.M31;
					global::Collision.Collision.floatAr[threadID, num29++] = damage * identity.M12 + num17 * identity.M22 + num16 * identity.M32;
					global::Collision.Collision.floatAr[threadID, num29++] = damage * identity.M13 + num17 * identity.M23 + num16 * identity.M33;
					num27++;
				}
				Update_Player_Vehicle_BoundingBox(0, threadID, (ushort)num15, ref identity);
				flag = Confine_Player_Position_ToBoundaries_New(ref global::MainGame.MainGame.playerVehicles[0].ph1, terrainHeight, 0) > 0 || flag;
				float num30 = players[0].charMain.pos1.v[0];
				float num31 = players[0].charMain.pos1.v[1];
				float num32 = players[0].charMain.pos1.v[2];
				float num33 = players[0].charMain.pos2.v[0];
				float num34 = players[0].charMain.pos2.v[1];
				float num35 = players[0].charMain.pos2.v[2];
				players[0].charMain.pos1.v[0] = num30;
				players[0].charMain.pos1.v[1] = num31;
				players[0].charMain.pos1.v[2] = num32;
				players[0].charMain.pos2.v[0] = num33;
				players[0].charMain.pos2.v[1] = num34;
				players[0].charMain.pos2.v[2] = num35;
				global::Collision.Collision.hitGround = false;
				mainC.collisionMain.ResetIgnoreList(threadID, (ushort)num15);
				int num36 = mainC.collisionMain.CheckCollision_Detailed_List(ref players[0].charMain, num15, 129, 143, 0, threadID);
				global::MainGame.MainGame.numWheelsTouching = 0;
				for (num18 = 0; num18 < numWheels; num18++)
				{
					if (global::Collision.Collision.floatArStatus[threadID, num18] == 1)
					{
						global::MainGame.MainGame.numWheelsTouching++;
						global::MainGame.MainGame.playerVehicles[0].wheelTouchingTimer = Vehicles.vehicles[curVehicle].wheelTouchingFactor;
					}
				}
				if (num36 > 0)
				{
					flag = true;
					if (num2 > 100f)
					{
						num26 = 0;
						num27 = 0;
						while (num26 < num25)
						{
							if (global::Collision.Collision.floatArStatus[threadID, num27] == 1)
							{
								mainC.renderingMain.New_Particle_New(2, global::MainGame.MainGame.playerVehicles[0].ph1.x + global::Collision.Collision.floatAr[threadID, num26], global::MainGame.MainGame.playerVehicles[0].ph1.y + global::Collision.Collision.floatAr[threadID, num26 + 1], global::MainGame.MainGame.playerVehicles[0].ph1.z + global::Collision.Collision.floatAr[threadID, num26 + 2], 0f, 0f, global::MainGame.MainGame.playerVehicles[0].ph1.velocityZ, 0, threadID);
							}
							num26 += 3;
							num27++;
						}
					}
					num28 = 0;
					float num37 = 0f;
					for (num26 = 0; num26 < num15; num26++)
					{
						if (global::Collision.Collision.floatArID[threadID, num26] > -1)
						{
							collisionObjectID = global::Collision.Collision.floatArID[threadID, num26];
						}
						if (global::Collision.Collision.floatArStatus[threadID, num26] == 1)
						{
							num28++;
							num37 += Math.Abs(global::MainGame.MainGame.playerVehicles[0].ph1.velocityX * global::Collision.Collision.floatArDir[threadID, num26, 0] + global::MainGame.MainGame.playerVehicles[0].ph1.velocityY * global::Collision.Collision.floatArDir[threadID, num26, 1] + global::MainGame.MainGame.playerVehicles[0].ph1.velocityZ * global::Collision.Collision.floatArDir[threadID, num26, 2]);
							if (num26 >= numWheels)
							{
								num37 = (float)num28 * players[0].velocityTerminalThreshold + 100f;
							}
						}
					}
					if (num28 > 0)
					{
						num37 /= (float)num28;
						num24 = num37;
					}
					float dX = players[0].charMain.pos2.v[0] - players[0].charMain.pos1.v[0];
					float dY = players[0].charMain.pos2.v[1] - players[0].charMain.pos1.v[1];
					float dZ = players[0].charMain.pos2.v[2] - players[0].charMain.pos1.v[2];
					damage = mainC.physicsMain.getTimeForDistanceTraveled(num9, num10, num11, global::MainGame.MainGame.playerVehicles[0].ph1.accelerationX, global::MainGame.MainGame.playerVehicles[0].ph1.accelerationY, global::MainGame.MainGame.playerVehicles[0].ph1.accelerationZ, dX, dY, dZ, num22);
					mainC.vehicles.Calculate_Vehicle_Collision_Data(ref global::MainGame.MainGame.playerVehicles[0], curVehicle, uBufferID, threadID);
					num18 = 3;
					damage = num22 - damage;
					while (damage > 0f && num18-- > 0)
					{
						float num38 = global::MainGame.MainGame.playerVehicles[0].newVelX;
						float num39 = global::MainGame.MainGame.playerVehicles[0].newVelY;
						float num40 = global::MainGame.MainGame.playerVehicles[0].newVelZ;
						float num17;
						for (num26 = 0; num26 < num15; num26++)
						{
							if (global::Collision.Collision.floatArStatus[threadID, num26] == 1)
							{
								dX = global::Collision.Collision.floatArDir[threadID, num26, 0];
								dY = global::Collision.Collision.floatArDir[threadID, num26, 1];
								dZ = global::Collision.Collision.floatArDir[threadID, num26, 2];
								if (Math.Abs(dX) < 0.0001f)
								{
									dX = 0f;
								}
								if (Math.Abs(dY) < 0.0001f)
								{
									dY = 0f;
								}
								if (Math.Abs(dZ) < 0.0001f)
								{
									dZ = 0f;
								}
								num17 = num38 * dX + num39 * dY + num40 * dZ;
								if ((float)(Math.Sign(dX) * Math.Sign(value)) + num12 < 0f)
								{
									num38 = 0f;
									dX = 0f;
									num12 = -10f;
								}
								if ((float)(Math.Sign(dY) * Math.Sign(value2)) + num13 < 0f)
								{
									num39 = 0f;
									dY = 0f;
									num13 = -10f;
								}
								if ((float)(Math.Sign(dZ) * Math.Sign(value3)) + num14 < 0f)
								{
									num40 = 0f;
									dZ = 0f;
									num14 = -10f;
								}
								if (num17 < 0f)
								{
									if (dX != 0f)
									{
										value = dX;
									}
									if (dY != 0f)
									{
										value2 = dY;
									}
									if (dZ != 0f)
									{
										value3 = dZ;
									}
									num38 -= num17 * dX;
									num39 -= num17 * dY;
									num40 -= num17 * dZ;
								}
								_ = global::Collision.Collision.floatArID[threadID, num26];
							}
						}
						global::MainGame.MainGame.playerVehicles[0].ph1.velocityX = num38;
						global::MainGame.MainGame.playerVehicles[0].ph1.velocityY = num39;
						global::MainGame.MainGame.playerVehicles[0].ph1.velocityZ = num40;
						global::MainGame.MainGame.playerVehicles[0].ph1.x = players[0].charMain.pos2.v[0];
						global::MainGame.MainGame.playerVehicles[0].ph1.y = players[0].charMain.pos2.v[1];
						global::MainGame.MainGame.playerVehicles[0].ph1.z = players[0].charMain.pos2.v[2];
						players[0].charMain.pos1.v[0] = global::MainGame.MainGame.playerVehicles[0].ph1.x;
						players[0].charMain.pos1.v[1] = global::MainGame.MainGame.playerVehicles[0].ph1.y;
						players[0].charMain.pos1.v[2] = global::MainGame.MainGame.playerVehicles[0].ph1.z;
						num9 = global::MainGame.MainGame.playerVehicles[0].ph1.velocityX;
						num10 = global::MainGame.MainGame.playerVehicles[0].ph1.velocityY;
						num11 = global::MainGame.MainGame.playerVehicles[0].ph1.velocityZ;
						mainC.physicsMain.getPosition_new(ref global::MainGame.MainGame.playerVehicles[0].ph1, damage);
						players[0].charMain.pos2.v[0] = global::MainGame.MainGame.playerVehicles[0].ph1.x;
						players[0].charMain.pos2.v[1] = global::MainGame.MainGame.playerVehicles[0].ph1.y;
						players[0].charMain.pos2.v[2] = global::MainGame.MainGame.playerVehicles[0].ph1.z;
						mainC.collisionMain.ResetIgnoreList(threadID, (ushort)num15);
						num36 = mainC.collisionMain.CheckCollision_Detailed_List(ref players[0].charMain, num15, 129, 135, 0, threadID);
						collisionWithGround += global::Collision.Collision.floatArMovDir[threadID, 0];
						num17 = damage;
						damage = 0f;
						if (num36 <= 0)
						{
							continue;
						}
						dX = players[0].charMain.pos2.v[0] - players[0].charMain.pos1.v[0];
						dY = players[0].charMain.pos2.v[1] - players[0].charMain.pos1.v[1];
						dZ = players[0].charMain.pos2.v[2] - players[0].charMain.pos1.v[2];
						float num16 = mainC.physicsMain.getTimeForDistanceTraveled(num9, num10, num11, global::MainGame.MainGame.playerVehicles[0].ph1.accelerationX, global::MainGame.MainGame.playerVehicles[0].ph1.accelerationY, global::MainGame.MainGame.playerVehicles[0].ph1.accelerationZ, dX, dY, dZ, num17);
						mainC.vehicles.Calculate_Vehicle_Collision_Data(ref global::MainGame.MainGame.playerVehicles[0], curVehicle, uBufferID, threadID);
						damage = num17 - num16;
						num28 = 0;
						float num19 = 0f;
						for (num26 = 0; num26 < num15; num26++)
						{
							if (global::Collision.Collision.floatArID[threadID, num26] > -1)
							{
								collisionObjectID = global::Collision.Collision.floatArID[threadID, num26];
							}
							if (global::Collision.Collision.floatArStatus[threadID, num26] == 1)
							{
								num28++;
								num19 += Math.Abs(global::MainGame.MainGame.playerVehicles[0].ph1.velocityX * global::Collision.Collision.floatArDir[threadID, num26, 0] + global::MainGame.MainGame.playerVehicles[0].ph1.velocityY * global::Collision.Collision.floatArDir[threadID, num26, 1] + global::MainGame.MainGame.playerVehicles[0].ph1.velocityZ * global::Collision.Collision.floatArDir[threadID, num26, 2]);
							}
						}
						if (num28 > 0)
						{
							num19 /= (float)num28;
							num24 = num19;
						}
					}
					if (num24 > players[0].velocityTerminalThreshold && Player_Injured_Threaded(num24))
					{
						Player_Over(0, playerDied: true, threadID);
						mainC.vehicles.Splash_Damage_From_Vehicle_Explosion(global::MainGame.MainGame.playerVehicles[0].ph1.x, global::MainGame.MainGame.playerVehicles[0].ph1.y, global::MainGame.MainGame.playerVehicles[0].ph1.z + 10f, curVehicle, 0, threadID);
					}
				}
				num2 = (float)Math.Sqrt(global::MainGame.MainGame.playerVehicles[0].ph1.velocityX * global::MainGame.MainGame.playerVehicles[0].ph1.velocityX + global::MainGame.MainGame.playerVehicles[0].ph1.velocityY * global::MainGame.MainGame.playerVehicles[0].ph1.velocityY + global::MainGame.MainGame.playerVehicles[0].ph1.velocityZ * global::MainGame.MainGame.playerVehicles[0].ph1.velocityZ);
				if (num2 < 1E-11f || num2 > 1E+10f)
				{
					global::MainGame.MainGame.playerVehicles[0].ph1.velocityX = 0f;
					global::MainGame.MainGame.playerVehicles[0].ph1.velocityY = 0f;
					global::MainGame.MainGame.playerVehicles[0].ph1.velocityZ = 0f;
					num2 = 0f;
					num6 = 0f;
					num7 = 0f;
					num8 = 0f;
				}
				global::MainGame.MainGame.playerVehicles[0].ph1.x = players[0].charMain.pos2.v[0];
				global::MainGame.MainGame.playerVehicles[0].ph1.y = players[0].charMain.pos2.v[1];
				global::MainGame.MainGame.playerVehicles[0].ph1.z = players[0].charMain.pos2.v[2];
				num21 -= num22;
			}
			players[0].mv[uBufferID] = identity;
			global::MainGame.MainGame.playerVehicles[0].mv[uBufferID] = identity;
			global::Joints.Joints.Reset_Joint_Data(0);
			global::Joints.Joints.Process_Joints_Threaded(0, frameTime, threadID);
			global::MainGame.MainGame.playerVehicles[0].ph1.velocityX = global::MainGame.MainGame.playerVehicles[0].newVelX;
			global::MainGame.MainGame.playerVehicles[0].ph1.velocityY = global::MainGame.MainGame.playerVehicles[0].newVelY;
			global::MainGame.MainGame.playerVehicles[0].ph1.velocityZ = global::MainGame.MainGame.playerVehicles[0].newVelZ;
			players[0].charP.velocity.v[0] = global::MainGame.MainGame.playerVehicles[0].newVelX;
			players[0].charP.velocity.v[1] = global::MainGame.MainGame.playerVehicles[0].newVelY;
			players[0].charP.velocity.v[2] = global::MainGame.MainGame.playerVehicles[0].newVelZ;
			zRotation = (float)Math.Acos((double)identity.M22 / Math.Sqrt(identity.M21 * identity.M21 + identity.M22 * identity.M22)) * 57.29578f;
			if (identity.M21 > 0f)
			{
				zRotation = 360f - zRotation;
			}
			players[0].zRotation = zRotation;
			global::MainGame.MainGame.playerVehicles[0].velocity = num2;
			global::MainGame.MainGame.planeVelocity = num2;
			Update_Player_Vehicle_BoundingBox(0, threadID, (ushort)num15, ref identity);
			flag = Confine_Player_Position_ToBoundaries_New(ref global::MainGame.MainGame.playerVehicles[0].ph1, terrainHeight, 0) > 0 || flag;
			players[0].charP.position.v[0] = global::MainGame.MainGame.playerVehicles[0].ph1.x;
			players[0].charP.position.v[1] = global::MainGame.MainGame.playerVehicles[0].ph1.y;
			players[0].charP.position.v[2] = global::MainGame.MainGame.playerVehicles[0].ph1.z;
			num18 = playerRaces[players[0].race].programTurnRight[players[0].type];
			GameLogic.propRot[0] = 0f;
			if (num18 != 255)
			{
				damage = 495f + 18900f * global::MainGame.MainGame.playerVehicles[0].throttleSpeed;
				GameLogic.propRot[0] = damage * frameTime;
				players[0].jt1[num18].pivot2Speed = damage;
				players[0].jt1[num18].targetPivot2 += damage;
				while (players[0].jt1[num18].targetPivot2 > 360f)
				{
					players[0].jt1[num18].targetPivot2 -= 360f;
				}
			}
			damage = num3 / data * 0.8f;
			damage = global::MainGame.MainGame.playerVehicles[0].throttleSpeed * 0.2f + damage;
			mainC.soundsMain.Play_Moving_Continual_Sound(0, 1, stop: false, 0.5f + global::MainGame.MainGame.playerVehicles[0].throttleSpeed * 0.5f, damage * 100f, global::MainGame.MainGame.playerVehicles[0].ph1.x, global::MainGame.MainGame.playerVehicles[0].ph1.y, global::MainGame.MainGame.playerVehicles[0].ph1.z, global::MainGame.MainGame.playerVehicles[0].ph1.velocityX, global::MainGame.MainGame.playerVehicles[0].ph1.velocityY, global::MainGame.MainGame.playerVehicles[0].ph1.velocityZ);
			if (players[0].damagePercentageCapped > 0.1f)
			{
				Damage_Particles_For_Damaged_Player_Vehicle(0, threadID);
			}
		}
		if ((players[0].onmap & 0x18) > 0)
		{
			if (mainPlayerDeathTimer == 0f)
			{
				if (players[0].damage >= global::MainGame.MainGame.playerVehicles[0].damageThresholdForExplosion)
				{
					flag = true;
				}
				if (flag)
				{
					if (global::MainGame.MainGame.gameMode == 1)
					{
						mainC.networkingMain.XBOX_Send_Network_Message53(53);
					}
					Player_Vehicle_Explodes(0, threadID);
					mainPlayerDeathTimer += frameTime / global::Physics.Physics.timeMod;
					global::MainGame.MainGame.playerVehicles[0].ph1.x = players[0].posX[rBufferID];
					global::MainGame.MainGame.playerVehicles[0].ph1.y = players[0].posY[rBufferID];
					global::MainGame.MainGame.playerVehicles[0].ph1.z = players[0].posZ[rBufferID];
					players[0].charP.position.v[0] = players[0].posX[rBufferID];
					players[0].charP.position.v[1] = players[0].posY[rBufferID];
					players[0].charP.position.v[2] = players[0].posZ[rBufferID];
					ref Matrix reference = ref players[0].mv[uBufferID];
					reference = players[0].mv[rBufferID];
					mainC.gameLogic.Game_Airplane_Crashed();
					players[0].onmap = 16;
				}
			}
			else if (mainPlayerDeathTimer > 0f)
			{
				mainPlayerDeathTimer += frameTime / global::Physics.Physics.timeMod;
				global::MainGame.MainGame.playerVehicles[0].ph1.x = players[0].posX[rBufferID];
				global::MainGame.MainGame.playerVehicles[0].ph1.y = players[0].posY[rBufferID];
				global::MainGame.MainGame.playerVehicles[0].ph1.z = players[0].posZ[rBufferID];
				players[0].charP.position.v[0] = players[0].posX[rBufferID];
				players[0].charP.position.v[1] = players[0].posY[rBufferID];
				players[0].charP.position.v[2] = players[0].posZ[rBufferID];
				ref Matrix reference2 = ref players[0].mv[uBufferID];
				reference2 = players[0].mv[rBufferID];
				Sync_Local_Player_View();
				if (mainPlayerDeathTimer > 2.5f)
				{
					players[0].onmap = 1;
				}
			}
		}
		global::MainGame.MainGame.debugUpdateCrashCount = 0;
	}

	public void Move_MainPlayer_ArcadeStyle_Helicopter(float frameTime, byte threadID)
	{
		bool flag = false;
		byte b = 3;
		byte b2 = 8;
		byte b3 = 6;
		float num = 1f;
		float num2 = 0f;
		float num3 = 0f;
		float num4 = 0f;
		float num5 = 0f;
		float num6 = 0f;
		float num7 = 0f;
		float num8 = 0f;
		float num9 = 0f;
		float num10 = 0f;
		float num11 = 0f;
		float num12 = 0f;
		float value = 0f;
		float value2 = 0f;
		float value3 = 0f;
		float num13 = 0f;
		float num14 = 0f;
		float num15 = 0f;
		float num16 = 0f;
		Matrix identity = Matrix.Identity;
		byte uBufferID = global::Rendering.Rendering.uBufferID;
		byte rBufferID = global::Rendering.Rendering.rBufferID;
		if ((players[0].onmap & 0x11) == 0)
		{
			ushort curVehicle = players[0].curVehicle;
			Vehicles.vehicles[curVehicle].balanceFactor = 0.15f;
			Vehicles.vehicles[curVehicle].wheelTouchingFactor = 0.15f;
			byte numWheels = Vehicles.vehicles[curVehicle].numWheels;
			short numColPoints = Vehicles.vehicles[curVehicle].numColPoints;
			short num17 = (short)(numWheels + numColPoints);
			identity = players[0].mv[rBufferID];
			float terrainHeight = mainC.terrainMain.Get_Terrain_Height(global::MainGame.MainGame.playerVehicles[0].ph1.x, global::MainGame.MainGame.playerVehicles[0].ph1.y, threadID);
			_ = playerSpeedRotateLeftStick;
			_ = global::InputHandler.InputHandler.lookSensitivity[global::InputHandler.InputHandler.lookMode];
			_ = Vehicles.vehicles[curVehicle].ph1.mass;
			_ = Vehicles.vehicles[curVehicle].data1;
			_ = Vehicles.vehicles[curVehicle].data2;
			_ = Vehicles.vehicles[curVehicle].data3;
			_ = Vehicles.vehicles[curVehicle].data4;
			_ = Vehicles.vehicles[curVehicle].data5;
			_ = Vehicles.vehicles[curVehicle].data6;
			float data = Vehicles.vehicles[curVehicle].data7;
			_ = Vehicles.vehicles[curVehicle].data8;
			_ = Vehicles.vehicles[curVehicle].data9;
			_ = Vehicles.vehicles[curVehicle].data10;
			_ = Vehicles.vehicles[curVehicle].data11;
			_ = Vehicles.vehicles[curVehicle].data12;
			float num18 = Vehicles.vehicles[curVehicle].data16;
			float num19 = 0.25f;
			float num21;
			if (!controlsInUse)
			{
				num = Set_Speed_If_Targeting_Enemy(0, frameTime, 225f, 14400f, 4000f, doCollisionCheck: false, global::InputHandler.InputHandler.controllerStickRightValX, threadID);
				global::Rendering.Rendering.cameraSpringDistance += ((global::InputHandler.InputHandler.controllerStickRightValueY + 1f) * 0.75f - global::Rendering.Rendering.cameraSpringDistance) * 0.75f * global::MainGame.MainGame.frametime;
				float num20 = 0.5f * frameTime * frameTime;
				num21 = global::InputHandler.InputHandler.controllerStickRightValueX - global::InputHandler.InputHandler.stickRightX;
				float num22 = num21 * global::MainGame.MainGame.playerVehicles[0].controllerSpring - global::MainGame.MainGame.playerVehicles[0].controllerDampening * global::InputHandler.InputHandler.stickRightXVel;
				global::InputHandler.InputHandler.stickRightX += global::InputHandler.InputHandler.stickRightXVel * frameTime + num22 * num20;
				global::InputHandler.InputHandler.stickRightXVel += num22 * frameTime;
				num5 = global::InputHandler.InputHandler.stickRightX;
				num21 = (Math.Abs(global::InputHandler.InputHandler.stickRightX) - global::InputHandler.InputHandler.controllerStickRightSmoothX) * frameTime * 0.1f;
				num21 = ((!(num21 > 0f)) ? (num21 * 5f) : (num21 * (1f + Math.Abs(global::MainGame.MainGame.arcadeModeRotAngle[0] / 0.08726646f) * 0.5f + Math.Abs(global::InputHandler.InputHandler.stickRightX) * 0.5f)));
				global::InputHandler.InputHandler.controllerStickRightSmoothX += num21;
				if (global::InputHandler.InputHandler.controllerStickRightSmoothX > 1f)
				{
					global::InputHandler.InputHandler.controllerStickRightSmoothX = 1f;
				}
				else if (global::InputHandler.InputHandler.controllerStickRightSmoothX < 0f)
				{
					global::InputHandler.InputHandler.controllerStickRightSmoothX = 0f;
				}
				num5 *= 0.05f + global::InputHandler.InputHandler.controllerStickRightSmoothX * 0.95f;
				num5 += (global::InputHandler.InputHandler.controllerStickRightValueX - num5) * (1f - Math.Abs(global::InputHandler.InputHandler.controllerStickRightValueY) * 0.95f) * global::InputHandler.InputHandler.lookSensitivity[global::InputHandler.InputHandler.lookMode];
				num21 = global::InputHandler.InputHandler.controllerStickRightValueY - global::InputHandler.InputHandler.stickRightY;
				num22 = num21 * global::MainGame.MainGame.playerVehicles[0].controllerSpring - global::MainGame.MainGame.playerVehicles[0].controllerDampening * global::InputHandler.InputHandler.stickRightYVel;
				global::InputHandler.InputHandler.stickRightY += global::InputHandler.InputHandler.stickRightYVel * frameTime + num22 * num20;
				global::InputHandler.InputHandler.stickRightYVel += num22 * frameTime;
				num4 = 0f - global::InputHandler.InputHandler.stickRightY;
				num21 = (Math.Abs(global::InputHandler.InputHandler.stickRightY) - global::InputHandler.InputHandler.controllerStickRightSmoothY) * frameTime;
				num21 = ((!(num21 > 0f)) ? (num21 * 5f) : (num21 * (1f + Math.Abs(global::MainGame.MainGame.arcadeModeRisingAngle[0] / 0.08726646f) * 0.5f + Math.Abs(global::InputHandler.InputHandler.stickRightY) * 0.5f)));
				global::InputHandler.InputHandler.controllerStickRightSmoothY += num21;
				if (global::InputHandler.InputHandler.controllerStickRightSmoothY > 1f)
				{
					global::InputHandler.InputHandler.controllerStickRightSmoothY = 1f;
				}
				else if (global::InputHandler.InputHandler.controllerStickRightSmoothY < 0f)
				{
					global::InputHandler.InputHandler.controllerStickRightSmoothY = 0f;
				}
				num4 *= 0.05f + global::InputHandler.InputHandler.controllerStickRightSmoothY * 0.95f;
				num4 += (0f - global::InputHandler.InputHandler.controllerStickRightValueY - num4) * global::InputHandler.InputHandler.lookSensitivity[global::InputHandler.InputHandler.lookMode];
				num6 = global::InputHandler.InputHandler.controllerStickLeftValueY * invertYSecondary;
			}
			if (players[0].onmap == 8)
			{
				num5 = ((global::MainGame.MainGame.arcadeModeRotAngle[0] == 0f) ? 0.5f : ((float)Math.Sign(global::MainGame.MainGame.arcadeModeRotAngle[0]) * 0.5f));
				num4 = 1f;
				num19 = 0.5f;
				num6 = -1f;
				num18 *= 3f;
			}
			int num23 = playerRaces[players[0].race].programTurnLeft[players[0].type];
			if (num23 < 255)
			{
				if (global::MainGame.MainGame.activateRetracts)
				{
					global::MainGame.MainGame.activateRetracts = false;
					mainC.programsMain.Set_Animation_To_Reverse_Direction(ref players[0].animations, num23);
				}
				if (players[0].animations[num23].status == 2)
				{
					b = players[0].headJoint;
					b3 = players[0].shoulderJointL;
					b2 = players[0].shoulderJointR;
					mainC.vehicles.Update_Wheel_Positions(ref global::MainGame.MainGame.playerVehicles[0], curVehicle, 0, players[0].jt1[b].rotX, players[0].jt1[b].rotY, players[0].jt1[b].rotZ);
					mainC.vehicles.Update_Wheel_Positions(ref global::MainGame.MainGame.playerVehicles[0], curVehicle, 1, players[0].jt1[b2].rotX, players[0].jt1[b2].rotY, players[0].jt1[b2].rotZ);
					mainC.vehicles.Update_Wheel_Positions(ref global::MainGame.MainGame.playerVehicles[0], curVehicle, 2, players[0].jt1[b3].rotX, players[0].jt1[b3].rotY, players[0].jt1[b3].rotZ);
				}
			}
			if (needToReload)
			{
				if (!reloading)
				{
					reloading = mainC.playersMain.Player_Needs_To_Reload(0);
				}
				mainC.weaponsMain.Check_Weapon_Views();
				needToReload = false;
			}
			else if (needToChamber)
			{
				if (!chambering)
				{
					chambering = Player_Needs_To_Chamber(0);
				}
				mainC.weaponsMain.Check_Weapon_Views();
				needToChamber = false;
			}
			if ((global::MainGame.MainGame.viewFollowingObject || controlsInUse) && players[0].onmap != 8 && global::MainGame.MainGame.playerVehicles[0].wheelTouchingTimer == 0f)
			{
				if (identity.M23 < 0f)
				{
					num4 = 0.3f;
				}
				else
				{
					float num20 = mainC.terrainMain.Get_Terrain_Height(global::MainGame.MainGame.playerVehicles[0].ph1.x, global::MainGame.MainGame.playerVehicles[0].ph1.y, threadID);
					if (global::MainGame.MainGame.playerVehicles[0].ph1.z < num20 + 500f)
					{
						float num22 = 10f;
						float num24 = mainC.terrainMain.Get_Terrain_Height(global::MainGame.MainGame.playerVehicles[0].ph1.x + 10f * identity.M21, global::MainGame.MainGame.playerVehicles[0].ph1.y + 10f * identity.M22, threadID);
						float num25 = mainC.terrainMain.Get_Terrain_Height(global::MainGame.MainGame.playerVehicles[0].ph1.x + 30f * identity.M21, global::MainGame.MainGame.playerVehicles[0].ph1.y + 30f * identity.M22, threadID);
						if ((num25 - num20) / 30f > (num24 - num20) / 10f)
						{
							num24 = num25;
							num22 = 30f;
						}
						num25 = mainC.terrainMain.Get_Terrain_Height(global::MainGame.MainGame.playerVehicles[0].ph1.x + 60f * identity.M21, global::MainGame.MainGame.playerVehicles[0].ph1.y + 60f * identity.M22, threadID);
						if ((num25 - num20) / 60f > (num24 - num20) / num22)
						{
							num24 = num25;
							num22 = 60f;
						}
						num21 = (float)Math.Sqrt(identity.M21 * identity.M21 + identity.M22 * identity.M22);
						num25 = num22 / num21 * identity.M23;
						if (num25 < num24)
						{
							num4 = 0.3f;
						}
					}
				}
			}
			float num26 = frameTime / global::Physics.Physics.timeMod;
			float num27 = num26;
			while (num26 > 0f)
			{
				if (num27 > num26)
				{
					num27 = num26;
				}
				Matrix matrix = identity;
				players[0].charMain.pos1.v[0] = global::MainGame.MainGame.playerVehicles[0].ph1.x;
				players[0].charMain.pos1.v[1] = global::MainGame.MainGame.playerVehicles[0].ph1.y;
				players[0].charMain.pos1.v[2] = global::MainGame.MainGame.playerVehicles[0].ph1.z;
				float num28 = matrix.M21;
				float num29 = matrix.M22;
				num21 = (float)Math.Sqrt(num28 * num28 + num29 * num29);
				if (num21 != 0f)
				{
					num28 /= num21;
					num29 /= num21;
				}
				float num30 = matrix.M11;
				float num31 = matrix.M12;
				num21 = (float)Math.Sqrt(num30 * num30 + num31 * num31);
				if (num21 != 0f)
				{
					num30 /= num21;
					num31 /= num21;
				}
				num2 = (float)Math.Sqrt(global::MainGame.MainGame.playerVehicles[0].ph1.velocityX * global::MainGame.MainGame.playerVehicles[0].ph1.velocityX + global::MainGame.MainGame.playerVehicles[0].ph1.velocityY * global::MainGame.MainGame.playerVehicles[0].ph1.velocityY + global::MainGame.MainGame.playerVehicles[0].ph1.velocityZ * global::MainGame.MainGame.playerVehicles[0].ph1.velocityZ);
				if (num2 < 1E-11f || num2 > 1E+10f)
				{
					global::MainGame.MainGame.playerVehicles[0].ph1.velocityX = 0f;
					global::MainGame.MainGame.playerVehicles[0].ph1.velocityY = 0f;
					global::MainGame.MainGame.playerVehicles[0].ph1.velocityZ = 0f;
					num2 = 0f;
				}
				global::MainGame.MainGame.playerVehicles[0].ph1.velocity = num2;
				num7 = 0f;
				num8 = 0f;
				num9 = 0f;
				if (num2 > 0f)
				{
					num7 = global::MainGame.MainGame.playerVehicles[0].ph1.velocityX / num2;
					num8 = global::MainGame.MainGame.playerVehicles[0].ph1.velocityY / num2;
					num9 = global::MainGame.MainGame.playerVehicles[0].ph1.velocityZ / num2;
				}
				num10 = global::MainGame.MainGame.playerVehicles[0].ph1.velocityX;
				num11 = global::MainGame.MainGame.playerVehicles[0].ph1.velocityY;
				num12 = global::MainGame.MainGame.playerVehicles[0].ph1.velocityZ;
				float num32 = num9;
				float num33 = num7 * num28 + num8 * num29;
				float num34 = num7 * num30 + num8 * num31;
				num16 = num2 * num33;
				float num35 = num2 * num34;
				float num36 = num2 * num32;
				num21 = (0f - num4) * data - num16;
				num16 += num21 * global::MainGame.MainGame.playerVehicles[0].accelerationFactor * num27;
				num21 = global::InputHandler.InputHandler.controllerStickLeftValueX * data * 0.5f - num35;
				num35 += num21 * global::MainGame.MainGame.playerVehicles[0].accelerationFactor * num27;
				num21 = num6 * data * num19 - num36;
				num36 += num21 * global::MainGame.MainGame.playerVehicles[0].accelerationFactor * num27;
				global::MainGame.MainGame.arcadeModeRotAngle[0] += num5 * (0.222f + 0.0556f * global::InputHandler.InputHandler.lookSensitivity[global::InputHandler.InputHandler.lookMode]) * num27;
				global::MainGame.MainGame.arcadeModeRisingAngle[0] += num4 * 0.083f * num27;
				if (Math.Abs(global::MainGame.MainGame.arcadeModeRotAngle[0]) > 0.08726646f)
				{
					global::InputHandler.InputHandler.stickRightXVel = 0f;
					global::InputHandler.InputHandler.stickRightX = global::InputHandler.InputHandler.controllerStickRightValueX;
					global::MainGame.MainGame.arcadeModeRotAngle[0] = 0.08726646f * (float)Math.Sign(global::MainGame.MainGame.arcadeModeRotAngle[0]);
				}
				if (Math.Abs(global::MainGame.MainGame.arcadeModeRisingAngle[0]) > 0.04363323f)
				{
					global::InputHandler.InputHandler.stickRightYVel = 0f;
					global::InputHandler.InputHandler.stickRightY = global::InputHandler.InputHandler.controllerStickRightValueY;
					global::MainGame.MainGame.arcadeModeRisingAngle[0] = 0.04363323f * (float)Math.Sign(global::MainGame.MainGame.arcadeModeRisingAngle[0]);
				}
				if (global::InputHandler.InputHandler.controllerStickRightValueX == 0f && Math.Abs(global::MainGame.MainGame.arcadeModeRotAngle[0]) < 0.08726646f)
				{
					global::MainGame.MainGame.arcadeModeRotAngle[0] -= global::MainGame.MainGame.arcadeModeRotAngle[0] * 0.85f * frameTime;
				}
				players[0].zRotation -= num5 * num18 * num27 * num;
				identity = Matrix.CreateRotationY(global::MainGame.MainGame.arcadeModeRotAngle[0]) * Matrix.CreateRotationX(global::MainGame.MainGame.arcadeModeRisingAngle[0]) * Matrix.CreateRotationZ(players[0].zRotation * ((float)Math.PI / 180f));
				global::MainGame.MainGame.playerVehicles[0].ph1.velocityX = identity.M21 * num16 + identity.M11 * num35;
				global::MainGame.MainGame.playerVehicles[0].ph1.velocityY = identity.M22 * num16 + identity.M12 * num35;
				global::MainGame.MainGame.playerVehicles[0].ph1.velocityZ = num36;
				global::MainGame.MainGame.playerVehicles[0].ph1.forceX = 0f;
				global::MainGame.MainGame.playerVehicles[0].ph1.forceY = 0f;
				global::MainGame.MainGame.playerVehicles[0].ph1.forceZ = 0f;
				global::MainGame.MainGame.playerVehicles[0].ph1.torqueX = 0f;
				global::MainGame.MainGame.playerVehicles[0].ph1.torqueY = 0f;
				global::MainGame.MainGame.playerVehicles[0].ph1.torqueZ = 0f;
				mainC.physicsMain.getPosition_new(ref global::MainGame.MainGame.playerVehicles[0].ph1, num27);
				global::MainGame.MainGame.playerVehicles[0].ph1.forceX = 0f;
				global::MainGame.MainGame.playerVehicles[0].ph1.forceY = 0f;
				global::MainGame.MainGame.playerVehicles[0].ph1.forceZ = 0f;
				global::MainGame.MainGame.playerVehicles[0].ph1.torqueX = 0f;
				global::MainGame.MainGame.playerVehicles[0].ph1.torqueY = 0f;
				global::MainGame.MainGame.playerVehicles[0].ph1.torqueZ = 0f;
				_ = global::MainGame.MainGame.playerVehicles[0].ph1.x;
				_ = players[0].charMain.pos1.v[0];
				_ = global::MainGame.MainGame.playerVehicles[0].ph1.y;
				_ = players[0].charMain.pos1.v[1];
				_ = global::MainGame.MainGame.playerVehicles[0].ph1.z;
				_ = players[0].charMain.pos1.v[2];
				global::MainGame.MainGame.playerVehicles[0].xBalanceTimer -= num27;
				global::MainGame.MainGame.playerVehicles[0].yBalanceTimer -= num27;
				global::MainGame.MainGame.playerVehicles[0].zBalanceTimer -= num27;
				global::MainGame.MainGame.playerVehicles[0].wheelTouchingTimer -= num27;
				if (global::MainGame.MainGame.playerVehicles[0].xBalanceTimer <= 0f)
				{
					global::MainGame.MainGame.playerVehicles[0].xBalanceTimer = 0f;
				}
				if (global::MainGame.MainGame.playerVehicles[0].yBalanceTimer <= 0f)
				{
					global::MainGame.MainGame.playerVehicles[0].yBalanceTimer = 0f;
				}
				if (global::MainGame.MainGame.playerVehicles[0].zBalanceTimer <= 0f)
				{
					global::MainGame.MainGame.playerVehicles[0].zBalanceTimer = 0f;
				}
				if (global::MainGame.MainGame.playerVehicles[0].wheelTouchingTimer <= 0f)
				{
					global::MainGame.MainGame.playerVehicles[0].wheelTouchingTimer = 0f;
				}
				if (global::MainGame.MainGame.playerVehicles[0].wheelTouchingTimer > 0f)
				{
					num21 = Math.Abs(num3 / 200f);
					if (num21 > 1f)
					{
						num21 = 1f;
					}
					else if (num21 < 0.3f)
					{
						num21 = 0f;
					}
					mainC.inputMain.GamePad_Vibration_Set_Low(1f * num21);
				}
				float num37 = 0f;
				collisionObjectID = -1;
				players[0].charMain.pos2.v[0] = global::MainGame.MainGame.playerVehicles[0].ph1.x;
				players[0].charMain.pos2.v[1] = global::MainGame.MainGame.playerVehicles[0].ph1.y;
				players[0].charMain.pos2.v[2] = global::MainGame.MainGame.playerVehicles[0].ph1.z;
				global::MainGame.MainGame.playerVehicles[0].newVelX = global::MainGame.MainGame.playerVehicles[0].ph1.velocityX;
				global::MainGame.MainGame.playerVehicles[0].newVelY = global::MainGame.MainGame.playerVehicles[0].ph1.velocityY;
				global::MainGame.MainGame.playerVehicles[0].newVelZ = global::MainGame.MainGame.playerVehicles[0].ph1.velocityZ;
				int num38 = numWheels * 3;
				short num39 = 0;
				int num40 = 0;
				int num41 = 0;
				int num42 = 129;
				while (num39 < num38)
				{
					num21 = global::MainGame.MainGame.playerVehicles[0].momentum.collisionPoints[num41++];
					float num22 = global::MainGame.MainGame.playerVehicles[0].momentum.collisionPoints[num41++];
					float num20 = global::MainGame.MainGame.playerVehicles[0].momentum.collisionPoints[num41++];
					global::Collision.Collision.floatAr[threadID, num39++] = num21 * matrix.M11 + num22 * matrix.M21 + num20 * matrix.M31;
					global::Collision.Collision.floatAr[threadID, num39++] = num21 * matrix.M12 + num22 * matrix.M22 + num20 * matrix.M32;
					global::Collision.Collision.floatAr[threadID, num39++] = num21 * matrix.M13 + num22 * matrix.M23 + num20 * matrix.M33;
					global::Collision.Collision.floatAr[threadID, num42++] = num21 * identity.M11 + num22 * identity.M21 + num20 * identity.M31;
					global::Collision.Collision.floatAr[threadID, num42++] = num21 * identity.M12 + num22 * identity.M22 + num20 * identity.M32;
					global::Collision.Collision.floatAr[threadID, num42++] = num21 * identity.M13 + num22 * identity.M23 + num20 * identity.M33;
					num40++;
				}
				num38 = num17 * 3;
				while (num39 < num38)
				{
					num21 = global::MainGame.MainGame.playerVehicles[0].momentum.collisionPoints[num41++];
					float num22 = global::MainGame.MainGame.playerVehicles[0].momentum.collisionPoints[num41++];
					float num20 = global::MainGame.MainGame.playerVehicles[0].momentum.collisionPoints[num41++];
					global::Collision.Collision.floatAr[threadID, num39++] = num21 * matrix.M11 + num22 * matrix.M21 + num20 * matrix.M31;
					global::Collision.Collision.floatAr[threadID, num39++] = num21 * matrix.M12 + num22 * matrix.M22 + num20 * matrix.M32;
					global::Collision.Collision.floatAr[threadID, num39++] = num21 * matrix.M13 + num22 * matrix.M23 + num20 * matrix.M33;
					global::Collision.Collision.floatAr[threadID, num42++] = num21 * identity.M11 + num22 * identity.M21 + num20 * identity.M31;
					global::Collision.Collision.floatAr[threadID, num42++] = num21 * identity.M12 + num22 * identity.M22 + num20 * identity.M32;
					global::Collision.Collision.floatAr[threadID, num42++] = num21 * identity.M13 + num22 * identity.M23 + num20 * identity.M33;
					num40++;
				}
				Update_Player_Vehicle_BoundingBox(0, threadID, (ushort)num17, ref identity);
				flag = Confine_Player_Position_ToBoundaries_New(ref global::MainGame.MainGame.playerVehicles[0].ph1, terrainHeight, 0) > 0 || flag;
				float num43 = players[0].charMain.pos1.v[0];
				float num44 = players[0].charMain.pos1.v[1];
				float num45 = players[0].charMain.pos1.v[2];
				float num46 = players[0].charMain.pos2.v[0];
				float num47 = players[0].charMain.pos2.v[1];
				float num48 = players[0].charMain.pos2.v[2];
				players[0].charMain.pos1.v[0] = num43;
				players[0].charMain.pos1.v[1] = num44;
				players[0].charMain.pos1.v[2] = num45;
				players[0].charMain.pos2.v[0] = num46;
				players[0].charMain.pos2.v[1] = num47;
				players[0].charMain.pos2.v[2] = num48;
				global::Collision.Collision.hitGround = false;
				mainC.collisionMain.ResetIgnoreList(threadID, (ushort)num17);
				int num49 = mainC.collisionMain.CheckCollision_Detailed_List(ref players[0].charMain, num17, 129, 143, 0, threadID);
				global::MainGame.MainGame.numWheelsTouching = 0;
				for (num23 = 0; num23 < numWheels; num23++)
				{
					if (global::Collision.Collision.floatArStatus[threadID, num23] == 1)
					{
						global::MainGame.MainGame.numWheelsTouching++;
						global::MainGame.MainGame.playerVehicles[0].wheelTouchingTimer = Vehicles.vehicles[curVehicle].wheelTouchingFactor;
					}
				}
				if (num49 > 0)
				{
					flag = true;
					if (num2 > 100f)
					{
						num39 = 0;
						num40 = 0;
						while (num39 < num38)
						{
							if (global::Collision.Collision.floatArStatus[threadID, num40] == 1)
							{
								mainC.renderingMain.New_Particle_New(2, global::MainGame.MainGame.playerVehicles[0].ph1.x + global::Collision.Collision.floatAr[threadID, num39], global::MainGame.MainGame.playerVehicles[0].ph1.y + global::Collision.Collision.floatAr[threadID, num39 + 1], global::MainGame.MainGame.playerVehicles[0].ph1.z + global::Collision.Collision.floatAr[threadID, num39 + 2], 0f, 0f, global::MainGame.MainGame.playerVehicles[0].ph1.velocityZ, 0, threadID);
							}
							num39 += 3;
							num40++;
						}
					}
					num41 = 0;
					float num50 = 0f;
					for (num39 = 0; num39 < num17; num39++)
					{
						if (global::Collision.Collision.floatArID[threadID, num39] > -1)
						{
							collisionObjectID = global::Collision.Collision.floatArID[threadID, num39];
						}
						if (global::Collision.Collision.floatArStatus[threadID, num39] == 1)
						{
							num41++;
							num50 += Math.Abs(global::MainGame.MainGame.playerVehicles[0].ph1.velocityX * global::Collision.Collision.floatArDir[threadID, num39, 0] + global::MainGame.MainGame.playerVehicles[0].ph1.velocityY * global::Collision.Collision.floatArDir[threadID, num39, 1] + global::MainGame.MainGame.playerVehicles[0].ph1.velocityZ * global::Collision.Collision.floatArDir[threadID, num39, 2]);
							if (num39 >= numWheels)
							{
								num50 = (float)num41 * players[0].velocityTerminalThreshold + 100f;
							}
						}
					}
					if (num41 > 0)
					{
						num50 /= (float)num41;
						num37 = num50;
					}
					float dX = players[0].charMain.pos2.v[0] - players[0].charMain.pos1.v[0];
					float dY = players[0].charMain.pos2.v[1] - players[0].charMain.pos1.v[1];
					float dZ = players[0].charMain.pos2.v[2] - players[0].charMain.pos1.v[2];
					num21 = mainC.physicsMain.getTimeForDistanceTraveled(num10, num11, num12, global::MainGame.MainGame.playerVehicles[0].ph1.accelerationX, global::MainGame.MainGame.playerVehicles[0].ph1.accelerationY, global::MainGame.MainGame.playerVehicles[0].ph1.accelerationZ, dX, dY, dZ, num27);
					mainC.vehicles.Calculate_Vehicle_Collision_Data(ref global::MainGame.MainGame.playerVehicles[0], curVehicle, uBufferID, threadID);
					num23 = 3;
					num21 = num27 - num21;
					while (num21 > 0f && num23-- > 0)
					{
						float num51 = global::MainGame.MainGame.playerVehicles[0].newVelX;
						float num52 = global::MainGame.MainGame.playerVehicles[0].newVelY;
						float num53 = global::MainGame.MainGame.playerVehicles[0].newVelZ;
						float num22;
						for (num39 = 0; num39 < num17; num39++)
						{
							if (global::Collision.Collision.floatArStatus[threadID, num39] == 1)
							{
								dX = global::Collision.Collision.floatArDir[threadID, num39, 0];
								dY = global::Collision.Collision.floatArDir[threadID, num39, 1];
								dZ = global::Collision.Collision.floatArDir[threadID, num39, 2];
								if (Math.Abs(dX) < 0.0001f)
								{
									dX = 0f;
								}
								if (Math.Abs(dY) < 0.0001f)
								{
									dY = 0f;
								}
								if (Math.Abs(dZ) < 0.0001f)
								{
									dZ = 0f;
								}
								num22 = num51 * dX + num52 * dY + num53 * dZ;
								if ((float)(Math.Sign(dX) * Math.Sign(value)) + num13 < 0f)
								{
									num51 = 0f;
									dX = 0f;
									num13 = -10f;
								}
								if ((float)(Math.Sign(dY) * Math.Sign(value2)) + num14 < 0f)
								{
									num52 = 0f;
									dY = 0f;
									num14 = -10f;
								}
								if ((float)(Math.Sign(dZ) * Math.Sign(value3)) + num15 < 0f)
								{
									num53 = 0f;
									dZ = 0f;
									num15 = -10f;
								}
								if (num22 < 0f)
								{
									if (dX != 0f)
									{
										value = dX;
									}
									if (dY != 0f)
									{
										value2 = dY;
									}
									if (dZ != 0f)
									{
										value3 = dZ;
									}
									num51 -= num22 * dX;
									num52 -= num22 * dY;
									num53 -= num22 * dZ;
								}
								_ = global::Collision.Collision.floatArID[threadID, num39];
							}
						}
						global::MainGame.MainGame.playerVehicles[0].ph1.velocityX = num51;
						global::MainGame.MainGame.playerVehicles[0].ph1.velocityY = num52;
						global::MainGame.MainGame.playerVehicles[0].ph1.velocityZ = num53;
						global::MainGame.MainGame.playerVehicles[0].ph1.x = players[0].charMain.pos2.v[0];
						global::MainGame.MainGame.playerVehicles[0].ph1.y = players[0].charMain.pos2.v[1];
						global::MainGame.MainGame.playerVehicles[0].ph1.z = players[0].charMain.pos2.v[2];
						players[0].charMain.pos1.v[0] = global::MainGame.MainGame.playerVehicles[0].ph1.x;
						players[0].charMain.pos1.v[1] = global::MainGame.MainGame.playerVehicles[0].ph1.y;
						players[0].charMain.pos1.v[2] = global::MainGame.MainGame.playerVehicles[0].ph1.z;
						num10 = global::MainGame.MainGame.playerVehicles[0].ph1.velocityX;
						num11 = global::MainGame.MainGame.playerVehicles[0].ph1.velocityY;
						num12 = global::MainGame.MainGame.playerVehicles[0].ph1.velocityZ;
						mainC.physicsMain.getPosition_new(ref global::MainGame.MainGame.playerVehicles[0].ph1, num21);
						players[0].charMain.pos2.v[0] = global::MainGame.MainGame.playerVehicles[0].ph1.x;
						players[0].charMain.pos2.v[1] = global::MainGame.MainGame.playerVehicles[0].ph1.y;
						players[0].charMain.pos2.v[2] = global::MainGame.MainGame.playerVehicles[0].ph1.z;
						mainC.collisionMain.ResetIgnoreList(threadID, (ushort)num17);
						num49 = mainC.collisionMain.CheckCollision_Detailed_List(ref players[0].charMain, num17, 129, 135, 0, threadID);
						collisionWithGround += global::Collision.Collision.floatArMovDir[threadID, 0];
						num22 = num21;
						num21 = 0f;
						if (num49 <= 0)
						{
							continue;
						}
						dX = players[0].charMain.pos2.v[0] - players[0].charMain.pos1.v[0];
						dY = players[0].charMain.pos2.v[1] - players[0].charMain.pos1.v[1];
						dZ = players[0].charMain.pos2.v[2] - players[0].charMain.pos1.v[2];
						float num20 = mainC.physicsMain.getTimeForDistanceTraveled(num10, num11, num12, global::MainGame.MainGame.playerVehicles[0].ph1.accelerationX, global::MainGame.MainGame.playerVehicles[0].ph1.accelerationY, global::MainGame.MainGame.playerVehicles[0].ph1.accelerationZ, dX, dY, dZ, num22);
						mainC.vehicles.Calculate_Vehicle_Collision_Data(ref global::MainGame.MainGame.playerVehicles[0], curVehicle, uBufferID, threadID);
						num21 = num22 - num20;
						num41 = 0;
						float num24 = 0f;
						for (num39 = 0; num39 < num17; num39++)
						{
							if (global::Collision.Collision.floatArID[threadID, num39] > -1)
							{
								collisionObjectID = global::Collision.Collision.floatArID[threadID, num39];
							}
							if (global::Collision.Collision.floatArStatus[threadID, num39] == 1)
							{
								num41++;
								num24 += Math.Abs(global::MainGame.MainGame.playerVehicles[0].ph1.velocityX * global::Collision.Collision.floatArDir[threadID, num39, 0] + global::MainGame.MainGame.playerVehicles[0].ph1.velocityY * global::Collision.Collision.floatArDir[threadID, num39, 1] + global::MainGame.MainGame.playerVehicles[0].ph1.velocityZ * global::Collision.Collision.floatArDir[threadID, num39, 2]);
							}
						}
						if (num41 > 0)
						{
							num24 /= (float)num41;
							num37 = num24;
						}
					}
					if (num37 > players[0].velocityTerminalThreshold && Player_Injured_Threaded(num37))
					{
						Player_Over(0, playerDied: true, threadID);
						mainC.vehicles.Splash_Damage_From_Vehicle_Explosion(global::MainGame.MainGame.playerVehicles[0].ph1.x, global::MainGame.MainGame.playerVehicles[0].ph1.y, global::MainGame.MainGame.playerVehicles[0].ph1.z + 10f, curVehicle, 0, threadID);
					}
				}
				num2 = (float)Math.Sqrt(global::MainGame.MainGame.playerVehicles[0].ph1.velocityX * global::MainGame.MainGame.playerVehicles[0].ph1.velocityX + global::MainGame.MainGame.playerVehicles[0].ph1.velocityY * global::MainGame.MainGame.playerVehicles[0].ph1.velocityY + global::MainGame.MainGame.playerVehicles[0].ph1.velocityZ * global::MainGame.MainGame.playerVehicles[0].ph1.velocityZ);
				if (num2 < 1E-11f || num2 > 1E+10f)
				{
					global::MainGame.MainGame.playerVehicles[0].ph1.velocityX = 0f;
					global::MainGame.MainGame.playerVehicles[0].ph1.velocityY = 0f;
					global::MainGame.MainGame.playerVehicles[0].ph1.velocityZ = 0f;
					num2 = 0f;
					num7 = 0f;
					num8 = 0f;
					num9 = 0f;
				}
				global::MainGame.MainGame.playerVehicles[0].ph1.x = players[0].charMain.pos2.v[0];
				global::MainGame.MainGame.playerVehicles[0].ph1.y = players[0].charMain.pos2.v[1];
				global::MainGame.MainGame.playerVehicles[0].ph1.z = players[0].charMain.pos2.v[2];
				num26 -= num27;
			}
			players[0].mv[uBufferID] = identity;
			global::MainGame.MainGame.playerVehicles[0].mv[uBufferID] = identity;
			global::Joints.Joints.Reset_Joint_Data(0);
			global::Joints.Joints.Process_Joints_Threaded(0, frameTime, threadID);
			global::MainGame.MainGame.playerVehicles[0].ph1.velocityX = global::MainGame.MainGame.playerVehicles[0].newVelX;
			global::MainGame.MainGame.playerVehicles[0].ph1.velocityY = global::MainGame.MainGame.playerVehicles[0].newVelY;
			global::MainGame.MainGame.playerVehicles[0].ph1.velocityZ = global::MainGame.MainGame.playerVehicles[0].newVelZ;
			players[0].charP.velocity.v[0] = global::MainGame.MainGame.playerVehicles[0].newVelX;
			players[0].charP.velocity.v[1] = global::MainGame.MainGame.playerVehicles[0].newVelY;
			players[0].charP.velocity.v[2] = global::MainGame.MainGame.playerVehicles[0].newVelZ;
			zRotation = (float)Math.Acos((double)identity.M22 / Math.Sqrt(identity.M21 * identity.M21 + identity.M22 * identity.M22)) * 57.29578f;
			if (identity.M21 > 0f)
			{
				zRotation = 360f - zRotation;
			}
			players[0].zRotation = zRotation;
			global::MainGame.MainGame.playerVehicles[0].velocity = num2;
			global::MainGame.MainGame.planeVelocity = num2;
			Update_Player_Vehicle_BoundingBox(0, threadID, (ushort)num17, ref identity);
			flag = Confine_Player_Position_ToBoundaries_New(ref global::MainGame.MainGame.playerVehicles[0].ph1, terrainHeight, 0) > 0 || flag;
			players[0].charP.position.v[0] = global::MainGame.MainGame.playerVehicles[0].ph1.x;
			players[0].charP.position.v[1] = global::MainGame.MainGame.playerVehicles[0].ph1.y;
			players[0].charP.position.v[2] = global::MainGame.MainGame.playerVehicles[0].ph1.z;
			if (global::Rendering.Rendering.watchingPlayer == 0)
			{
				mainC.inputMain.UI_HUD_Set_Player_Velocity(num3);
			}
			else
			{
				mainC.inputMain.UI_HUD_Set_Player_Velocity(global::MainGame.MainGame.playerVehicles[global::Rendering.Rendering.watchingPlayer].ph1.velocity);
			}
			mainC.inputMain.UI_HUD_Set_Player_Height(global::MainGame.MainGame.playerVehicles[global::Rendering.Rendering.watchingPlayer].ph1.z);
			num23 = playerRaces[players[0].race].programTurnRight[players[0].type];
			GameLogic.propRot[0] = 0f;
			if (num23 != 255)
			{
				num21 = 495f + 18900f * global::MainGame.MainGame.playerVehicles[0].throttleSpeed;
				GameLogic.propRot[0] = num21 * frameTime;
				players[0].jt1[num23].pivot2Speed = num21;
				players[0].jt1[num23].targetPivot2 += num21;
				while (players[0].jt1[num23].targetPivot2 > 360f)
				{
					players[0].jt1[num23].targetPivot2 -= 360f;
				}
			}
			num21 = Math.Abs(global::InputHandler.InputHandler.controllerStickRightValueY) + Math.Abs(global::InputHandler.InputHandler.controllerStickLeftValueY);
			if (num21 > 1f)
			{
				num21 = 1f;
			}
			mainC.soundsMain.Play_Moving_Continual_Sound(0, 1, stop: false, 0.5f + num21 * 0.5f, 0.01f + num21 * 30f, global::MainGame.MainGame.playerVehicles[0].ph1.x, global::MainGame.MainGame.playerVehicles[0].ph1.y, global::MainGame.MainGame.playerVehicles[0].ph1.z, global::MainGame.MainGame.playerVehicles[0].ph1.velocityX, global::MainGame.MainGame.playerVehicles[0].ph1.velocityY, global::MainGame.MainGame.playerVehicles[0].ph1.velocityZ);
			if (players[0].damagePercentageCapped > 0.1f)
			{
				Damage_Particles_For_Damaged_Player_Vehicle(0, threadID);
			}
		}
		if ((players[0].onmap & 0x18) > 0)
		{
			if (mainPlayerDeathTimer == 0f)
			{
				if (players[0].damage >= global::MainGame.MainGame.playerVehicles[0].damageThresholdForExplosion)
				{
					flag = true;
				}
				if (flag)
				{
					if (global::MainGame.MainGame.gameMode == 1)
					{
						mainC.networkingMain.XBOX_Send_Network_Message53(53);
					}
					Player_Vehicle_Explodes(0, threadID);
					mainPlayerDeathTimer += frameTime / global::Physics.Physics.timeMod;
					global::MainGame.MainGame.playerVehicles[0].ph1.x = players[0].posX[rBufferID];
					global::MainGame.MainGame.playerVehicles[0].ph1.y = players[0].posY[rBufferID];
					global::MainGame.MainGame.playerVehicles[0].ph1.z = players[0].posZ[rBufferID];
					players[0].charP.position.v[0] = players[0].posX[rBufferID];
					players[0].charP.position.v[1] = players[0].posY[rBufferID];
					players[0].charP.position.v[2] = players[0].posZ[rBufferID];
					ref Matrix reference = ref players[0].mv[uBufferID];
					reference = players[0].mv[rBufferID];
					mainC.gameLogic.Game_Airplane_Crashed();
					players[0].onmap = 16;
				}
			}
			else if (mainPlayerDeathTimer > 0f)
			{
				mainPlayerDeathTimer += frameTime / global::Physics.Physics.timeMod;
				global::MainGame.MainGame.playerVehicles[0].ph1.x = players[0].posX[rBufferID];
				global::MainGame.MainGame.playerVehicles[0].ph1.y = players[0].posY[rBufferID];
				global::MainGame.MainGame.playerVehicles[0].ph1.z = players[0].posZ[rBufferID];
				players[0].charP.position.v[0] = players[0].posX[rBufferID];
				players[0].charP.position.v[1] = players[0].posY[rBufferID];
				players[0].charP.position.v[2] = players[0].posZ[rBufferID];
				ref Matrix reference2 = ref players[0].mv[uBufferID];
				reference2 = players[0].mv[rBufferID];
				Sync_Local_Player_View();
				if (mainPlayerDeathTimer > 2.5f)
				{
					players[0].onmap = 1;
				}
			}
		}
		global::MainGame.MainGame.debugUpdateCrashCount = 0;
	}

	public void Move_MainPlayer_Airplane_New(float frameTime, byte threadID)
	{
		bool flag = false;
		byte b = 3;
		byte b2 = 8;
		byte b3 = 6;
		float num = 0f;
		float num2 = 0f;
		float num3 = 0f;
		float num4 = 0f;
		float num5 = 0f;
		float num6 = 0f;
		float num7 = 0f;
		float num8 = 0f;
		float num9 = 0f;
		float num10 = 0f;
		float num11 = 0f;
		float num12 = 0f;
		float value = 0f;
		float value2 = 0f;
		float value3 = 0f;
		float num13 = 0f;
		float num14 = 0f;
		float num15 = 0f;
		Matrix identity = Matrix.Identity;
		byte uBufferID = global::Rendering.Rendering.uBufferID;
		byte rBufferID = global::Rendering.Rendering.rBufferID;
		if ((players[0].onmap & 0x11) == 0)
		{
			if (global::InputHandler.InputHandler.controllerButtonRightShoulder)
			{
				global::MainGame.MainGame.playerVehicles[0].throttleSpeed += global::MainGame.MainGame.frametime * 0.5f;
			}
			else if (global::InputHandler.InputHandler.controllerButtonLeftShoulder)
			{
				global::MainGame.MainGame.playerVehicles[0].throttleSpeed -= global::MainGame.MainGame.frametime * 0.5f;
			}
			if (global::InputHandler.InputHandler.controllerDPadUpPressed)
			{
				global::MainGame.MainGame.playerVehicles[0].throttleSpeed += 0.15f;
			}
			else if (global::InputHandler.InputHandler.controllerDPadDownPressed)
			{
				global::MainGame.MainGame.playerVehicles[0].throttleSpeed -= 0.15f;
			}
			if (global::MainGame.MainGame.playerVehicles[0].throttleSpeed < 0f)
			{
				global::MainGame.MainGame.playerVehicles[0].throttleSpeed = 0f;
			}
			else if (global::MainGame.MainGame.playerVehicles[0].throttleSpeed > 1f)
			{
				global::MainGame.MainGame.playerVehicles[0].throttleSpeed = 1f;
			}
			float num17;
			if (!controlsInUse)
			{
				switch (controllerScheme)
				{
				case 0:
					num5 = global::InputHandler.InputHandler.controllerStickRightValX;
					num4 = (0f - global::InputHandler.InputHandler.controllerStickLeftValueY) * invertY;
					num6 = 0f - global::InputHandler.InputHandler.controllerStickLeftValueX;
					break;
				case 1:
					num5 = global::InputHandler.InputHandler.controllerStickLeftValueX;
					num4 = (0f - global::InputHandler.InputHandler.controllerStickLeftValueY) * invertY;
					num6 = 0f - global::InputHandler.InputHandler.controllerStickRightValueX;
					break;
				case 2:
					num5 = global::InputHandler.InputHandler.controllerStickRightValX;
					num4 = (0f - global::InputHandler.InputHandler.controllerStickRightValY) * invertY;
					num6 = 0f - global::InputHandler.InputHandler.controllerStickLeftValueX;
					break;
				case 3:
					num5 = global::InputHandler.InputHandler.controllerStickLeftValueX;
					num4 = (0f - global::InputHandler.InputHandler.controllerStickRightValY) * invertY;
					num6 = 0f - global::InputHandler.InputHandler.controllerStickRightValueX;
					break;
				}
				float num16 = 0.5f * frameTime * frameTime;
				num17 = global::InputHandler.InputHandler.controllerStickRightValueX - global::InputHandler.InputHandler.stickRightX;
				float num18 = num17 * global::MainGame.MainGame.playerVehicles[0].controllerSpring - global::MainGame.MainGame.playerVehicles[0].controllerDampening * global::InputHandler.InputHandler.stickRightXVel;
				global::InputHandler.InputHandler.stickRightX += global::InputHandler.InputHandler.stickRightXVel * frameTime + num18 * num16;
				global::InputHandler.InputHandler.stickRightXVel += num18 * frameTime;
				num5 = global::InputHandler.InputHandler.stickRightX;
				num17 = global::InputHandler.InputHandler.controllerStickRightValueY - global::InputHandler.InputHandler.stickRightY;
				num18 = num17 * global::MainGame.MainGame.playerVehicles[0].controllerSpring - global::MainGame.MainGame.playerVehicles[0].controllerDampening * global::InputHandler.InputHandler.stickRightYVel;
				global::InputHandler.InputHandler.stickRightY += global::InputHandler.InputHandler.stickRightYVel * frameTime + num18 * num16;
				global::InputHandler.InputHandler.stickRightYVel += num18 * frameTime;
				num4 = 0f - global::InputHandler.InputHandler.stickRightY;
			}
			num5 *= invertY;
			if (players[0].onmap == 8)
			{
				num5 = 0.05f;
				num4 = -0.1f * identity.M33;
			}
			global::MainGame.MainGame.flaps = 0f;
			ushort curVehicle = players[0].curVehicle;
			Vehicles.vehicles[curVehicle].balanceFactor = 0.15f;
			Vehicles.vehicles[curVehicle].wheelTouchingFactor = 0.15f;
			byte numWheels = Vehicles.vehicles[curVehicle].numWheels;
			short numColPoints = Vehicles.vehicles[curVehicle].numColPoints;
			short num19 = (short)(numWheels + numColPoints);
			identity = players[0].mv[rBufferID];
			float terrainHeight = mainC.terrainMain.Get_Terrain_Height(global::MainGame.MainGame.playerVehicles[0].ph1.x, global::MainGame.MainGame.playerVehicles[0].ph1.y, threadID);
			_ = playerSpeedRotateLeftStick;
			_ = global::InputHandler.InputHandler.lookSensitivity[global::InputHandler.InputHandler.lookMode];
			if (players[0].onmap == 8)
			{
				num5 = 0.05f;
				num4 = -0.1f * identity.M33;
			}
			float mass = Vehicles.vehicles[curVehicle].ph1.mass;
			float data = Vehicles.vehicles[curVehicle].data1;
			_ = Vehicles.vehicles[curVehicle].data2;
			_ = Vehicles.vehicles[curVehicle].data3;
			_ = Vehicles.vehicles[curVehicle].data4;
			_ = Vehicles.vehicles[curVehicle].data5;
			float data2 = Vehicles.vehicles[curVehicle].data6;
			float data3 = Vehicles.vehicles[curVehicle].data7;
			_ = Vehicles.vehicles[curVehicle].data8;
			_ = Vehicles.vehicles[curVehicle].data9;
			_ = Vehicles.vehicles[curVehicle].data10;
			_ = Vehicles.vehicles[curVehicle].data11;
			float num20 = Vehicles.vehicles[curVehicle].data12 * ((float)Math.PI / 180f);
			float num21 = 1f;
			float num22 = 12f;
			float num23 = 13f;
			float num24 = 19.2f;
			float num25 = 12.25f;
			float num26 = 10600f;
			float num27 = 10600f;
			float num28 = 3500f;
			float num29 = 0.03824f * data;
			int num30 = playerRaces[players[0].race].programTurnLeft[players[0].type];
			if (num30 < 255)
			{
				if (global::MainGame.MainGame.activateRetracts)
				{
					global::MainGame.MainGame.activateRetracts = false;
					if (players[0].animations[num30].status != 2)
					{
						gearSound = mainC.soundsMain.Play_Priority_Sound("Hydraulics", global::MainGame.MainGame.playerVehicles[0].ph1.x, global::MainGame.MainGame.playerVehicles[0].ph1.y, global::MainGame.MainGame.playerVehicles[0].ph1.z, 0f, 0f, 0f);
					}
					mainC.programsMain.Set_Animation_To_Reverse_Direction(ref players[0].animations, num30);
				}
				if (players[0].animations[num30].status == 2)
				{
					b = players[0].headJoint;
					b3 = players[0].shoulderJointL;
					b2 = players[0].shoulderJointR;
					mainC.vehicles.Update_Wheel_Positions(ref global::MainGame.MainGame.playerVehicles[0], curVehicle, 0, players[0].jt1[b].rotX, players[0].jt1[b].rotY, players[0].jt1[b].rotZ);
					mainC.vehicles.Update_Wheel_Positions(ref global::MainGame.MainGame.playerVehicles[0], curVehicle, 1, players[0].jt1[b2].rotX, players[0].jt1[b2].rotY, players[0].jt1[b2].rotZ);
					mainC.vehicles.Update_Wheel_Positions(ref global::MainGame.MainGame.playerVehicles[0], curVehicle, 2, players[0].jt1[b3].rotX, players[0].jt1[b3].rotY, players[0].jt1[b3].rotZ);
				}
			}
			if (needToReload)
			{
				if (mainC.weaponsMain.Player_Has_Ammo_For_Weapon(0) > 1)
				{
					byte b4 = (byte)players[0].wpnIndex;
					if (global::Weapons.Weapons.ammo[global::Weapons.Weapons.ammoClips[players[0].weapon2[b4].curClip].ammoIndex].single)
					{
						players[0].weapon2[b4].fired = false;
						mainC.weaponsMain.Load_Ammo_Clip_Into_Player_Weapon(b4, (byte)players[0].primaryWeaponMountWeapon, 0, players[0].ammoClips[players[0].weapon2[b4].curClip].numClips);
					}
					else
					{
						mainC.weaponsMain.Weapon_Reloaded(b4, 0);
					}
				}
				needToReload = false;
				needToChamber = false;
			}
			if ((global::MainGame.MainGame.viewFollowingObject || controlsInUse) && players[0].onmap != 8 && global::MainGame.MainGame.playerVehicles[0].wheelTouchingTimer == 0f)
			{
				if (identity.M23 < 0f)
				{
					num4 = 0.3f;
				}
				else
				{
					float num16 = mainC.terrainMain.Get_Terrain_Height(global::MainGame.MainGame.playerVehicles[0].ph1.x, global::MainGame.MainGame.playerVehicles[0].ph1.y, threadID);
					if (global::MainGame.MainGame.playerVehicles[0].ph1.z < num16 + 500f)
					{
						float num18 = 10f;
						float num31 = mainC.terrainMain.Get_Terrain_Height(global::MainGame.MainGame.playerVehicles[0].ph1.x + 10f * identity.M21, global::MainGame.MainGame.playerVehicles[0].ph1.y + 10f * identity.M22, threadID);
						float num32 = mainC.terrainMain.Get_Terrain_Height(global::MainGame.MainGame.playerVehicles[0].ph1.x + 30f * identity.M21, global::MainGame.MainGame.playerVehicles[0].ph1.y + 30f * identity.M22, threadID);
						if ((num32 - num16) / 30f > (num31 - num16) / 10f)
						{
							num31 = num32;
							num18 = 30f;
						}
						num32 = mainC.terrainMain.Get_Terrain_Height(global::MainGame.MainGame.playerVehicles[0].ph1.x + 60f * identity.M21, global::MainGame.MainGame.playerVehicles[0].ph1.y + 60f * identity.M22, threadID);
						if ((num32 - num16) / 60f > (num31 - num16) / num18)
						{
							num31 = num32;
							num18 = 60f;
						}
						num17 = (float)Math.Sqrt(identity.M21 * identity.M21 + identity.M22 * identity.M22);
						num32 = num18 / num17 * identity.M23;
						if (num32 < num31)
						{
							num4 = 0.3f;
						}
					}
				}
			}
			float num33 = frameTime / global::Physics.Physics.timeMod;
			float num34 = 0.001f;
			while (num33 > 0f)
			{
				if (num34 > num33)
				{
					num34 = num33;
				}
				Matrix matrix = identity;
				players[0].charMain.pos1.v[0] = global::MainGame.MainGame.playerVehicles[0].ph1.x;
				players[0].charMain.pos1.v[1] = global::MainGame.MainGame.playerVehicles[0].ph1.y;
				players[0].charMain.pos1.v[2] = global::MainGame.MainGame.playerVehicles[0].ph1.z;
				float m = matrix.M21;
				float m2 = matrix.M22;
				float m3 = matrix.M23;
				float m4 = matrix.M31;
				float m5 = matrix.M32;
				float m6 = matrix.M33;
				float m7 = matrix.M11;
				float m8 = matrix.M12;
				float m9 = matrix.M13;
				num = (float)Math.Sqrt(global::MainGame.MainGame.playerVehicles[0].ph1.velocityX * global::MainGame.MainGame.playerVehicles[0].ph1.velocityX + global::MainGame.MainGame.playerVehicles[0].ph1.velocityY * global::MainGame.MainGame.playerVehicles[0].ph1.velocityY + global::MainGame.MainGame.playerVehicles[0].ph1.velocityZ * global::MainGame.MainGame.playerVehicles[0].ph1.velocityZ);
				if (num < 1E-11f || num > 1E+10f)
				{
					global::MainGame.MainGame.playerVehicles[0].ph1.velocityX = 0f;
					global::MainGame.MainGame.playerVehicles[0].ph1.velocityY = 0f;
					global::MainGame.MainGame.playerVehicles[0].ph1.velocityZ = 0f;
					num = 0f;
				}
				global::MainGame.MainGame.playerVehicles[0].ph1.velocity = num;
				num7 = 0f;
				num8 = 0f;
				num9 = 0f;
				if (num > 0f)
				{
					num7 = global::MainGame.MainGame.playerVehicles[0].ph1.velocityX / num;
					num8 = global::MainGame.MainGame.playerVehicles[0].ph1.velocityY / num;
					num9 = global::MainGame.MainGame.playerVehicles[0].ph1.velocityZ / num;
				}
				num10 = global::MainGame.MainGame.playerVehicles[0].ph1.velocityX;
				num11 = global::MainGame.MainGame.playerVehicles[0].ph1.velocityY;
				num12 = global::MainGame.MainGame.playerVehicles[0].ph1.velocityZ;
				float num35 = num7 * m4 + num8 * m5 + num9 * m6;
				float num36 = num7 * m + num8 * m2 + num9 * m3;
				float num37 = num7 * m7 + num8 * m8 + num9 * m9;
				num2 = num * num36;
				num3 = num2 * num2;
				Matrix matrix2 = Matrix.CreateRotationY(num20) * matrix;
				float num38 = num7 * matrix2.M11 + num8 * matrix2.M12 + num9 * matrix2.M13;
				Matrix matrix3 = Matrix.CreateRotationY(0f - num20) * matrix;
				float num39 = num7 * matrix3.M11 + num8 * matrix3.M12 + num9 * matrix3.M13;
				num17 = global::MainGame.MainGame.playerVehicles[0].ph1.angularVelocityX * num22;
				num17 = num17 * num17 * 1.28f * 0.5f * 0.07648f * num27;
				if (num17 / global::MainGame.MainGame.playerVehicles[0].ph1.momentInertiaAxisX * num34 > Math.Abs(global::MainGame.MainGame.playerVehicles[0].ph1.angularVelocityX))
				{
					num17 = global::MainGame.MainGame.playerVehicles[0].ph1.momentInertiaAxisX * (Math.Abs(global::MainGame.MainGame.playerVehicles[0].ph1.angularVelocityX) / num34);
				}
				global::MainGame.MainGame.playerVehicles[0].ph1.torqueX -= (float)Math.Sign(global::MainGame.MainGame.playerVehicles[0].ph1.angularVelocityX) * num17;
				num17 = global::MainGame.MainGame.playerVehicles[0].ph1.angularVelocityY * 6.5f;
				num17 = num17 * num17 * 0.64f * 0.07648f * num26;
				if (num17 / global::MainGame.MainGame.playerVehicles[0].ph1.momentInertiaAxisY * num34 > Math.Abs(global::MainGame.MainGame.playerVehicles[0].ph1.angularVelocityY))
				{
					num17 = global::MainGame.MainGame.playerVehicles[0].ph1.momentInertiaAxisY * (Math.Abs(global::MainGame.MainGame.playerVehicles[0].ph1.angularVelocityY) / num34);
				}
				global::MainGame.MainGame.playerVehicles[0].ph1.torqueY -= (float)Math.Sign(global::MainGame.MainGame.playerVehicles[0].ph1.angularVelocityY) * num17;
				num17 = global::MainGame.MainGame.playerVehicles[0].ph1.angularVelocityZ * num23;
				num17 = num17 * num17 * 1.28f * 0.5f * 0.07648f * num28;
				if (num17 / global::MainGame.MainGame.playerVehicles[0].ph1.momentInertiaAxisZ * num34 > Math.Abs(global::MainGame.MainGame.playerVehicles[0].ph1.angularVelocityZ))
				{
					num17 = global::MainGame.MainGame.playerVehicles[0].ph1.momentInertiaAxisZ * (Math.Abs(global::MainGame.MainGame.playerVehicles[0].ph1.angularVelocityZ) / num34);
				}
				global::MainGame.MainGame.playerVehicles[0].ph1.torqueZ -= (float)Math.Sign(global::MainGame.MainGame.playerVehicles[0].ph1.angularVelocityZ) * num17;
				num17 = num * num37;
				float num40 = global::MainGame.MainGame.playerVehicles[0].ph1.velocityX - m7 * num17;
				float num41 = global::MainGame.MainGame.playerVehicles[0].ph1.velocityY - m8 * num17;
				float num42 = global::MainGame.MainGame.playerVehicles[0].ph1.velocityZ - m9 * num17;
				float num43 = (float)Math.Sqrt(num40 * num40 + num41 * num41 + num42 * num42);
				if (num43 != 0f)
				{
					num40 /= num43;
					num41 /= num43;
					num42 /= num43;
				}
				float num32 = num43;
				num43 *= num43;
				float num31 = num40 * m + num41 * m2 + num42 * m3;
				if (Math.Abs(num31) > 1f)
				{
					num31 = Math.Sign(num31);
				}
				float num18 = (float)Math.Acos(num31) * 57.29578f;
				num17 = num40 * m4 + num41 * m5 + num42 * m6;
				if (num17 > 0f)
				{
					num18 *= -1f;
				}
				num18 -= (float)Math.Sign(num4) * Math.Abs(num4) * 20f;
				if (num4 >= 0f)
				{
					num18 *= -1f;
				}
				float num16 = mainC.graphingMain.Get_Point_2D_Float_Graph_Fixed_Width_X_Axis(num18, 0f, 0, 5);
				num17 = num43 * num16 * 0.5f * 0.07648f * num24;
				global::MainGame.MainGame.playerVehicles[0].ph1.forceX -= num17 * m;
				global::MainGame.MainGame.playerVehicles[0].ph1.forceY -= num17 * m2;
				global::MainGame.MainGame.playerVehicles[0].ph1.forceZ -= num17 * m3;
				num16 = mainC.graphingMain.Get_Point_2D_Float_Graph_Fixed_Width_X_Axis(num18, 0f, 0, 4);
				num17 = num43 * num16 * 0.5f * 0.07648f * num24;
				if (num4 >= 0f)
				{
					num17 *= -1f;
				}
				global::MainGame.MainGame.playerVehicles[0].ph1.forceX += num17 * m4;
				global::MainGame.MainGame.playerVehicles[0].ph1.forceY += num17 * m5;
				global::MainGame.MainGame.playerVehicles[0].ph1.forceZ += num17 * m6;
				global::MainGame.MainGame.playerVehicles[0].ph1.torqueX -= num17 * num22;
				num17 = num40 * m4 + num41 * m5 + num42 * m6;
				num32 *= num17;
				num18 = (float)Math.Sign(num17) * (num32 * num32 * 1.28f * 0.5f * 0.07648f * num24);
				global::MainGame.MainGame.playerVehicles[0].ph1.forceX -= num18 * m4;
				global::MainGame.MainGame.playerVehicles[0].ph1.forceY -= num18 * m5;
				global::MainGame.MainGame.playerVehicles[0].ph1.forceZ -= num18 * m6;
				global::MainGame.MainGame.playerVehicles[0].ph1.torqueX += num18 * num22;
				if (num36 > 0f)
				{
					num17 = num * num39;
					num40 = global::MainGame.MainGame.playerVehicles[0].ph1.velocityX - matrix3.M11 * num17;
					num41 = global::MainGame.MainGame.playerVehicles[0].ph1.velocityY - matrix3.M12 * num17;
					num42 = global::MainGame.MainGame.playerVehicles[0].ph1.velocityZ - matrix3.M13 * num17;
					num43 = (float)Math.Sqrt(num40 * num40 + num41 * num41 + num42 * num42);
					if (num43 != 0f)
					{
						num40 /= num43;
						num41 /= num43;
						num42 /= num43;
					}
					num43 *= num43;
					num31 = matrix3.M21 * num40 + matrix3.M22 * num41 + matrix3.M23 * num42;
					if (Math.Abs(num31) > 1f)
					{
						num31 = Math.Sign(num31);
					}
					num16 = (float)Math.Acos(num31) * 57.29578f;
					num18 = num40 * matrix3.M31 + num41 * matrix3.M32 + num42 * matrix3.M33;
					if (num18 > 0f)
					{
						num16 *= -1f;
					}
					num18 = mainC.graphingMain.Get_Point_2D_Float_Graph_Fixed_Width_X_Axis(num16, 0f, 0, 1);
					num17 = mainC.graphingMain.Get_Point_2D_Float_Graph_Fixed_Width_X_Axis(num16, 0f, 0, 3);
					num18 = num18 * (1f - global::MainGame.MainGame.flaps) + num17 * global::MainGame.MainGame.flaps;
					num17 = num18 * num29 * num43;
					global::MainGame.MainGame.playerVehicles[0].ph1.forceX -= num17 * m;
					global::MainGame.MainGame.playerVehicles[0].ph1.forceY -= num17 * m2;
					global::MainGame.MainGame.playerVehicles[0].ph1.forceZ -= num17 * m3;
					num18 = mainC.graphingMain.Get_Point_2D_Float_Graph_Fixed_Width_X_Axis(num16, 0f, 0, 0);
					num17 = mainC.graphingMain.Get_Point_2D_Float_Graph_Fixed_Width_X_Axis(num16, 0f, 0, 2);
					num18 = num18 * (1f - global::MainGame.MainGame.flaps) + num17 * global::MainGame.MainGame.flaps;
					num17 = num18 * num29 * num43;
					num32 = num17;
					global::MainGame.MainGame.playerVehicles[0].ph1.forceX += num17 * matrix3.M31;
					global::MainGame.MainGame.playerVehicles[0].ph1.forceY += num17 * matrix3.M32;
					global::MainGame.MainGame.playerVehicles[0].ph1.forceZ += num17 * matrix3.M33;
					global::MainGame.MainGame.playerVehicles[0].ph1.torqueX -= num17 * num21;
					num17 = num * num38;
					num40 = global::MainGame.MainGame.playerVehicles[0].ph1.velocityX - matrix2.M11 * num17;
					num41 = global::MainGame.MainGame.playerVehicles[0].ph1.velocityY - matrix2.M12 * num17;
					num42 = global::MainGame.MainGame.playerVehicles[0].ph1.velocityZ - matrix2.M13 * num17;
					num43 = (float)Math.Sqrt(num40 * num40 + num41 * num41 + num42 * num42);
					if (num43 != 0f)
					{
						num40 /= num43;
						num41 /= num43;
						num42 /= num43;
					}
					num43 *= num43;
					num31 = matrix2.M21 * num40 + matrix2.M22 * num41 + matrix2.M23 * num42;
					if (Math.Abs(num31) > 1f)
					{
						num31 = Math.Sign(num31);
					}
					num16 = (float)Math.Acos(num31) * 57.29578f;
					num18 = num40 * matrix2.M31 + num41 * matrix2.M32 + num42 * matrix2.M33;
					if (num18 > 0f)
					{
						num16 *= -1f;
					}
					num18 = mainC.graphingMain.Get_Point_2D_Float_Graph_Fixed_Width_X_Axis(num16, 0f, 0, 1);
					num17 = mainC.graphingMain.Get_Point_2D_Float_Graph_Fixed_Width_X_Axis(num16, 0f, 0, 3);
					num18 = num18 * (1f - global::MainGame.MainGame.flaps) + num17 * global::MainGame.MainGame.flaps;
					num17 = num18 * num29 * num43;
					global::MainGame.MainGame.playerVehicles[0].ph1.forceX -= num17 * m;
					global::MainGame.MainGame.playerVehicles[0].ph1.forceY -= num17 * m2;
					global::MainGame.MainGame.playerVehicles[0].ph1.forceZ -= num17 * m3;
					num18 = mainC.graphingMain.Get_Point_2D_Float_Graph_Fixed_Width_X_Axis(num16, 0f, 0, 0);
					num17 = mainC.graphingMain.Get_Point_2D_Float_Graph_Fixed_Width_X_Axis(num16, 0f, 0, 2);
					num18 = num18 * (1f - global::MainGame.MainGame.flaps) + num17 * global::MainGame.MainGame.flaps;
					num17 = num18 * num29 * num43;
					global::MainGame.MainGame.playerVehicles[0].ph1.forceX += num17 * matrix2.M31;
					global::MainGame.MainGame.playerVehicles[0].ph1.forceY += num17 * matrix2.M32;
					global::MainGame.MainGame.playerVehicles[0].ph1.forceZ += num17 * matrix2.M33;
					global::MainGame.MainGame.playerVehicles[0].ph1.torqueX -= num17 * num21;
					global::MainGame.MainGame.playerVehicles[0].ph1.torqueY -= (num32 - num17) * 0.05f;
				}
				num17 = num * num35;
				num40 = global::MainGame.MainGame.playerVehicles[0].ph1.velocityX - m4 * num17;
				num41 = global::MainGame.MainGame.playerVehicles[0].ph1.velocityY - m5 * num17;
				num42 = global::MainGame.MainGame.playerVehicles[0].ph1.velocityZ - m6 * num17;
				num43 = (float)Math.Sqrt(num40 * num40 + num41 * num41 + num42 * num42);
				if (num43 != 0f)
				{
					num40 /= num43;
					num41 /= num43;
					num42 /= num43;
				}
				num43 *= 15f / 22f;
				num32 = num43;
				num43 *= num43;
				num31 = num40 * m + num41 * m2 + num42 * m3;
				if (Math.Abs(num31) > 1f)
				{
					num31 = Math.Sign(num31);
				}
				num18 = (float)Math.Acos(num31) * 57.29578f;
				num17 = num40 * m7 + num41 * m8 + num42 * m9;
				if (num17 > 0f)
				{
					num18 *= -1f;
				}
				num18 += (float)Math.Sign(num6) * Math.Abs(num6) * 30f;
				if (num6 < 0f)
				{
					num18 *= -1f;
				}
				num16 = mainC.graphingMain.Get_Point_2D_Float_Graph_Fixed_Width_X_Axis(num18, 0f, 0, 5);
				num17 = num43 * num16 * 0.5f * 0.07648f * num25;
				global::MainGame.MainGame.playerVehicles[0].ph1.forceX -= num17 * m;
				global::MainGame.MainGame.playerVehicles[0].ph1.forceY -= num17 * m2;
				global::MainGame.MainGame.playerVehicles[0].ph1.forceZ -= num17 * m3;
				num16 = mainC.graphingMain.Get_Point_2D_Float_Graph_Fixed_Width_X_Axis(num18, 0f, 0, 4);
				num17 = num43 * num16 * 0.5f * 0.07648f * num25;
				if (num6 < 0f)
				{
					num17 *= -1f;
				}
				global::MainGame.MainGame.playerVehicles[0].ph1.forceX += num17 * m7;
				global::MainGame.MainGame.playerVehicles[0].ph1.forceY += num17 * m8;
				global::MainGame.MainGame.playerVehicles[0].ph1.forceZ += num17 * m9;
				global::MainGame.MainGame.playerVehicles[0].ph1.torqueZ += num17 * num23;
				num17 = num40 * m7 + num41 * m8 + num42 * m9;
				num32 *= num17;
				num18 = (float)Math.Sign(num17) * (num32 * num32 * 1.28f * 0.5f * 0.07648f * num25);
				global::MainGame.MainGame.playerVehicles[0].ph1.torqueZ -= num18 * num23;
				num18 *= 4f;
				global::MainGame.MainGame.playerVehicles[0].ph1.forceX -= num18 * m7;
				global::MainGame.MainGame.playerVehicles[0].ph1.forceY -= num18 * m8;
				global::MainGame.MainGame.playerVehicles[0].ph1.forceZ -= num18 * m9;
				_ = (float)(int)global::MainGame.MainGame.gearDown[0];
				_ = 0f;
				float num44 = global::MainGame.MainGame.playerVehicles[0].throttleSpeed * data2 * ((data3 - num2) / data3);
				global::MainGame.MainGame.playerVehicles[0].ph1.forceX += m * num44;
				global::MainGame.MainGame.playerVehicles[0].ph1.forceY += m2 * num44;
				global::MainGame.MainGame.playerVehicles[0].ph1.forceZ += m3 * num44;
				global::MainGame.MainGame.playerVehicles[0].ph1.forceZ += -32.15223f * mass;
				num17 = 0f - num5;
				num16 = Math.Abs(num17);
				if (num16 > 1f)
				{
					num17 = Math.Sign(num17);
				}
				num31 = (byte)(num16 * 10f) switch
				{
					0 => 0.5f + 0.1f * num16, 
					1 => 0.51f + 0.125f * num16, 
					2 => 0.535f + 0.15f * num16, 
					3 => 0.58f + 0.175f * num16, 
					4 => 0.65f + 0.2f * num16, 
					5 => 0.75f + 0.225f * num16, 
					6 => 1.885f + 0.25f * num16, 
					7 => 3.06f + 0.275f * num16, 
					8 => 4.28f + 0.3f * num16, 
					9 => 5.55f + 0.325f * num16, 
					_ => 6.875f, 
				};
				num18 = num4;
				num16 = Math.Abs(num18);
				if (num16 > 1f)
				{
					num18 = Math.Sign(num18);
				}
				switch ((byte)(num16 * 10f))
				{
				case 0:
					num32 = 0.5f + 0.1f * num16;
					break;
				case 1:
					num32 = 0.51f + 0.125f * num16;
					break;
				case 2:
					num32 = 0.535f + 0.15f * num16;
					break;
				case 3:
					num32 = 0.58f + 0.175f * num16;
					break;
				case 4:
					num32 = 0.65f + 0.2f * num16;
					break;
				case 5:
					num32 = 0.75f + 0.225f * num16;
					break;
				case 6:
					num32 = 0.885f + 0.25f * num16;
					break;
				case 7:
					num32 = 1.06f + 0.275f * num16;
					break;
				case 8:
					num32 = 2.28f + 0.3f * num16;
					break;
				case 9:
					num32 = 3.55f + 0.325f * num16;
					break;
				default:
					num32 = 4.875f;
					break;
				}
				global::MainGame.MainGame.playerVehicles[0].ph1.torqueY += (0f - num17) * num31 * 20f * num3 * global::InputHandler.InputHandler.lookSensitivity[global::InputHandler.InputHandler.lookMode];
				global::MainGame.MainGame.turnPlaneAround = 0f;
				if (float.IsNaN(global::MainGame.MainGame.playerVehicles[0].ph1.forceX) || float.IsNaN(global::MainGame.MainGame.playerVehicles[0].ph1.forceY) || float.IsNaN(global::MainGame.MainGame.playerVehicles[0].ph1.forceZ))
				{
					global::MainGame.MainGame.playerVehicles[0].ph1.forceX = 0f;
					global::MainGame.MainGame.playerVehicles[0].ph1.forceY = 0f;
					global::MainGame.MainGame.playerVehicles[0].ph1.forceZ = 0f;
				}
				mainC.physicsMain.getPosition_new(ref global::MainGame.MainGame.playerVehicles[0].ph1, num34);
				playerRot *= Quaternion.CreateFromYawPitchRoll(global::MainGame.MainGame.playerVehicles[0].ph1.angularVelocityY * num34, global::MainGame.MainGame.playerVehicles[0].ph1.angularVelocityX * num34, global::MainGame.MainGame.playerVehicles[0].ph1.angularVelocityZ * num34);
				Matrix.CreateFromQuaternion(ref playerRot, out identity);
				global::MainGame.MainGame.playerVehicles[0].ph1.forceX = 0f;
				global::MainGame.MainGame.playerVehicles[0].ph1.forceY = 0f;
				global::MainGame.MainGame.playerVehicles[0].ph1.forceZ = 0f;
				global::MainGame.MainGame.playerVehicles[0].ph1.torqueX = 0f;
				global::MainGame.MainGame.playerVehicles[0].ph1.torqueY = 0f;
				global::MainGame.MainGame.playerVehicles[0].ph1.torqueZ = 0f;
				_ = global::MainGame.MainGame.playerVehicles[0].ph1.x;
				_ = players[0].charMain.pos1.v[0];
				_ = global::MainGame.MainGame.playerVehicles[0].ph1.y;
				_ = players[0].charMain.pos1.v[1];
				_ = global::MainGame.MainGame.playerVehicles[0].ph1.z;
				_ = players[0].charMain.pos1.v[2];
				global::MainGame.MainGame.playerVehicles[0].xBalanceTimer -= num34;
				global::MainGame.MainGame.playerVehicles[0].yBalanceTimer -= num34;
				global::MainGame.MainGame.playerVehicles[0].zBalanceTimer -= num34;
				global::MainGame.MainGame.playerVehicles[0].wheelTouchingTimer -= num34;
				if (global::MainGame.MainGame.playerVehicles[0].xBalanceTimer <= 0f)
				{
					global::MainGame.MainGame.playerVehicles[0].xBalanceTimer = 0f;
				}
				if (global::MainGame.MainGame.playerVehicles[0].yBalanceTimer <= 0f)
				{
					global::MainGame.MainGame.playerVehicles[0].yBalanceTimer = 0f;
				}
				if (global::MainGame.MainGame.playerVehicles[0].zBalanceTimer <= 0f)
				{
					global::MainGame.MainGame.playerVehicles[0].zBalanceTimer = 0f;
				}
				if (global::MainGame.MainGame.playerVehicles[0].wheelTouchingTimer <= 0f)
				{
					global::MainGame.MainGame.playerVehicles[0].wheelTouchingTimer = 0f;
				}
				if (global::MainGame.MainGame.playerVehicles[0].wheelTouchingTimer > 0f)
				{
					num17 = Math.Abs(num2 / 200f);
					if (num17 > 1f)
					{
						num17 = 1f;
					}
					else if (num17 < 0.3f)
					{
						num17 = 0f;
					}
					mainC.inputMain.GamePad_Vibration_Set_Low(1f * num17);
				}
				float num45 = 0f;
				collisionObjectID = -1;
				players[0].charMain.pos2.v[0] = global::MainGame.MainGame.playerVehicles[0].ph1.x;
				players[0].charMain.pos2.v[1] = global::MainGame.MainGame.playerVehicles[0].ph1.y;
				players[0].charMain.pos2.v[2] = global::MainGame.MainGame.playerVehicles[0].ph1.z;
				global::MainGame.MainGame.playerVehicles[0].newVelX = global::MainGame.MainGame.playerVehicles[0].ph1.velocityX;
				global::MainGame.MainGame.playerVehicles[0].newVelY = global::MainGame.MainGame.playerVehicles[0].ph1.velocityY;
				global::MainGame.MainGame.playerVehicles[0].newVelZ = global::MainGame.MainGame.playerVehicles[0].ph1.velocityZ;
				int num46 = numWheels * 3;
				short num47 = 0;
				int num48 = 0;
				int num49 = 0;
				int num50 = 129;
				while (num47 < num46)
				{
					num17 = global::MainGame.MainGame.playerVehicles[0].momentum.collisionPoints[num49++];
					num18 = global::MainGame.MainGame.playerVehicles[0].momentum.collisionPoints[num49++];
					num16 = global::MainGame.MainGame.playerVehicles[0].momentum.collisionPoints[num49++];
					global::Collision.Collision.floatAr[threadID, num47++] = num17 * matrix.M11 + num18 * matrix.M21 + num16 * matrix.M31;
					global::Collision.Collision.floatAr[threadID, num47++] = num17 * matrix.M12 + num18 * matrix.M22 + num16 * matrix.M32;
					global::Collision.Collision.floatAr[threadID, num47++] = num17 * matrix.M13 + num18 * matrix.M23 + num16 * matrix.M33;
					global::Collision.Collision.floatAr[threadID, num50++] = num17 * identity.M11 + num18 * identity.M21 + num16 * identity.M31;
					global::Collision.Collision.floatAr[threadID, num50++] = num17 * identity.M12 + num18 * identity.M22 + num16 * identity.M32;
					global::Collision.Collision.floatAr[threadID, num50++] = num17 * identity.M13 + num18 * identity.M23 + num16 * identity.M33;
					num48++;
				}
				num46 = num19 * 3;
				while (num47 < num46)
				{
					num17 = global::MainGame.MainGame.playerVehicles[0].momentum.collisionPoints[num49++];
					num18 = global::MainGame.MainGame.playerVehicles[0].momentum.collisionPoints[num49++];
					num16 = global::MainGame.MainGame.playerVehicles[0].momentum.collisionPoints[num49++];
					global::Collision.Collision.floatAr[threadID, num47++] = num17 * matrix.M11 + num18 * matrix.M21 + num16 * matrix.M31;
					global::Collision.Collision.floatAr[threadID, num47++] = num17 * matrix.M12 + num18 * matrix.M22 + num16 * matrix.M32;
					global::Collision.Collision.floatAr[threadID, num47++] = num17 * matrix.M13 + num18 * matrix.M23 + num16 * matrix.M33;
					global::Collision.Collision.floatAr[threadID, num50++] = num17 * identity.M11 + num18 * identity.M21 + num16 * identity.M31;
					global::Collision.Collision.floatAr[threadID, num50++] = num17 * identity.M12 + num18 * identity.M22 + num16 * identity.M32;
					global::Collision.Collision.floatAr[threadID, num50++] = num17 * identity.M13 + num18 * identity.M23 + num16 * identity.M33;
					num48++;
				}
				Update_Player_Vehicle_BoundingBox(0, threadID, (ushort)num19, ref identity);
				flag = Confine_Player_Position_ToBoundaries_New(ref global::MainGame.MainGame.playerVehicles[0].ph1, terrainHeight, 0) > 0 || flag;
				float num51 = players[0].charMain.pos1.v[0];
				float num52 = players[0].charMain.pos1.v[1];
				float num53 = players[0].charMain.pos1.v[2];
				float num54 = players[0].charMain.pos2.v[0];
				float num55 = players[0].charMain.pos2.v[1];
				float num56 = players[0].charMain.pos2.v[2];
				players[0].charMain.pos1.v[0] = num51;
				players[0].charMain.pos1.v[1] = num52;
				players[0].charMain.pos1.v[2] = num53;
				players[0].charMain.pos2.v[0] = num54;
				players[0].charMain.pos2.v[1] = num55;
				players[0].charMain.pos2.v[2] = num56;
				global::Collision.Collision.hitGround = false;
				mainC.collisionMain.ResetIgnoreList(threadID, (ushort)num19);
				int num57 = mainC.collisionMain.CheckCollision_Detailed_List(ref players[0].charMain, num19, 129, 143, 0, threadID);
				global::MainGame.MainGame.numWheelsTouching = 0;
				for (num30 = 0; num30 < numWheels; num30++)
				{
					if (global::Collision.Collision.floatArStatus[threadID, num30] == 1)
					{
						global::MainGame.MainGame.numWheelsTouching++;
						global::MainGame.MainGame.playerVehicles[0].wheelTouchingTimer = Vehicles.vehicles[curVehicle].wheelTouchingFactor;
					}
				}
				if (num57 > 0)
				{
					flag = true;
					if (num > 100f)
					{
						num47 = 0;
						num48 = 0;
						while (num47 < num46)
						{
							if (global::Collision.Collision.floatArStatus[threadID, num48] == 1)
							{
								mainC.renderingMain.New_Particle_New(2, global::MainGame.MainGame.playerVehicles[0].ph1.x + global::Collision.Collision.floatAr[threadID, num47], global::MainGame.MainGame.playerVehicles[0].ph1.y + global::Collision.Collision.floatAr[threadID, num47 + 1], global::MainGame.MainGame.playerVehicles[0].ph1.z + global::Collision.Collision.floatAr[threadID, num47 + 2], 0f, 0f, global::MainGame.MainGame.playerVehicles[0].ph1.velocityZ, 0, threadID);
							}
							num47 += 3;
							num48++;
						}
					}
					num49 = 0;
					num43 = 0f;
					for (num47 = 0; num47 < num19; num47++)
					{
						if (global::Collision.Collision.floatArID[threadID, num47] > -1)
						{
							collisionObjectID = global::Collision.Collision.floatArID[threadID, num47];
						}
						if (global::Collision.Collision.floatArStatus[threadID, num47] == 1)
						{
							num49++;
							num43 += Math.Abs(global::MainGame.MainGame.playerVehicles[0].ph1.velocityX * global::Collision.Collision.floatArDir[threadID, num47, 0] + global::MainGame.MainGame.playerVehicles[0].ph1.velocityY * global::Collision.Collision.floatArDir[threadID, num47, 1] + global::MainGame.MainGame.playerVehicles[0].ph1.velocityZ * global::Collision.Collision.floatArDir[threadID, num47, 2]);
							if (num47 >= numWheels)
							{
								num43 = (float)num49 * players[0].velocityTerminalThreshold + 100f;
							}
						}
					}
					if (num49 > 0)
					{
						num43 /= (float)num49;
						num45 = num43;
					}
					num40 = players[0].charMain.pos2.v[0] - players[0].charMain.pos1.v[0];
					num41 = players[0].charMain.pos2.v[1] - players[0].charMain.pos1.v[1];
					num42 = players[0].charMain.pos2.v[2] - players[0].charMain.pos1.v[2];
					num17 = mainC.physicsMain.getTimeForDistanceTraveled(num10, num11, num12, global::MainGame.MainGame.playerVehicles[0].ph1.accelerationX, global::MainGame.MainGame.playerVehicles[0].ph1.accelerationY, global::MainGame.MainGame.playerVehicles[0].ph1.accelerationZ, num40, num41, num42, num34);
					mainC.vehicles.Calculate_Vehicle_Collision_Data(ref global::MainGame.MainGame.playerVehicles[0], curVehicle, uBufferID, threadID);
					num30 = 3;
					num17 = num34 - num17;
					while (num17 > 0f && num30-- > 0)
					{
						float num58 = global::MainGame.MainGame.playerVehicles[0].newVelX;
						float num59 = global::MainGame.MainGame.playerVehicles[0].newVelY;
						float num60 = global::MainGame.MainGame.playerVehicles[0].newVelZ;
						for (num47 = 0; num47 < num19; num47++)
						{
							if (global::Collision.Collision.floatArStatus[threadID, num47] == 1)
							{
								num40 = global::Collision.Collision.floatArDir[threadID, num47, 0];
								num41 = global::Collision.Collision.floatArDir[threadID, num47, 1];
								num42 = global::Collision.Collision.floatArDir[threadID, num47, 2];
								if (Math.Abs(num40) < 0.0001f)
								{
									num40 = 0f;
								}
								if (Math.Abs(num41) < 0.0001f)
								{
									num41 = 0f;
								}
								if (Math.Abs(num42) < 0.0001f)
								{
									num42 = 0f;
								}
								num18 = num58 * num40 + num59 * num41 + num60 * num42;
								if ((float)(Math.Sign(num40) * Math.Sign(value)) + num13 < 0f)
								{
									num58 = 0f;
									num40 = 0f;
									num13 = -10f;
								}
								if ((float)(Math.Sign(num41) * Math.Sign(value2)) + num14 < 0f)
								{
									num59 = 0f;
									num41 = 0f;
									num14 = -10f;
								}
								if ((float)(Math.Sign(num42) * Math.Sign(value3)) + num15 < 0f)
								{
									num60 = 0f;
									num42 = 0f;
									num15 = -10f;
								}
								if (num18 < 0f)
								{
									if (num40 != 0f)
									{
										value = num40;
									}
									if (num41 != 0f)
									{
										value2 = num41;
									}
									if (num42 != 0f)
									{
										value3 = num42;
									}
									num58 -= num18 * num40;
									num59 -= num18 * num41;
									num60 -= num18 * num42;
								}
								_ = global::Collision.Collision.floatArID[threadID, num47];
							}
						}
						global::MainGame.MainGame.playerVehicles[0].ph1.velocityX = num58;
						global::MainGame.MainGame.playerVehicles[0].ph1.velocityY = num59;
						global::MainGame.MainGame.playerVehicles[0].ph1.velocityZ = num60;
						global::MainGame.MainGame.playerVehicles[0].ph1.x = players[0].charMain.pos2.v[0];
						global::MainGame.MainGame.playerVehicles[0].ph1.y = players[0].charMain.pos2.v[1];
						global::MainGame.MainGame.playerVehicles[0].ph1.z = players[0].charMain.pos2.v[2];
						players[0].charMain.pos1.v[0] = global::MainGame.MainGame.playerVehicles[0].ph1.x;
						players[0].charMain.pos1.v[1] = global::MainGame.MainGame.playerVehicles[0].ph1.y;
						players[0].charMain.pos1.v[2] = global::MainGame.MainGame.playerVehicles[0].ph1.z;
						num10 = global::MainGame.MainGame.playerVehicles[0].ph1.velocityX;
						num11 = global::MainGame.MainGame.playerVehicles[0].ph1.velocityY;
						num12 = global::MainGame.MainGame.playerVehicles[0].ph1.velocityZ;
						mainC.physicsMain.getPosition_new(ref global::MainGame.MainGame.playerVehicles[0].ph1, num17);
						players[0].charMain.pos2.v[0] = global::MainGame.MainGame.playerVehicles[0].ph1.x;
						players[0].charMain.pos2.v[1] = global::MainGame.MainGame.playerVehicles[0].ph1.y;
						players[0].charMain.pos2.v[2] = global::MainGame.MainGame.playerVehicles[0].ph1.z;
						mainC.collisionMain.ResetIgnoreList(threadID, (ushort)num19);
						num57 = mainC.collisionMain.CheckCollision_Detailed_List(ref players[0].charMain, num19, 129, 135, 0, threadID);
						collisionWithGround += global::Collision.Collision.floatArMovDir[threadID, 0];
						num18 = num17;
						num17 = 0f;
						if (num57 <= 0)
						{
							continue;
						}
						num40 = players[0].charMain.pos2.v[0] - players[0].charMain.pos1.v[0];
						num41 = players[0].charMain.pos2.v[1] - players[0].charMain.pos1.v[1];
						num42 = players[0].charMain.pos2.v[2] - players[0].charMain.pos1.v[2];
						num16 = mainC.physicsMain.getTimeForDistanceTraveled(num10, num11, num12, global::MainGame.MainGame.playerVehicles[0].ph1.accelerationX, global::MainGame.MainGame.playerVehicles[0].ph1.accelerationY, global::MainGame.MainGame.playerVehicles[0].ph1.accelerationZ, num40, num41, num42, num18);
						mainC.vehicles.Calculate_Vehicle_Collision_Data(ref global::MainGame.MainGame.playerVehicles[0], curVehicle, uBufferID, threadID);
						num17 = num18 - num16;
						num49 = 0;
						num31 = 0f;
						for (num47 = 0; num47 < num19; num47++)
						{
							if (global::Collision.Collision.floatArID[threadID, num47] > -1)
							{
								collisionObjectID = global::Collision.Collision.floatArID[threadID, num47];
							}
							if (global::Collision.Collision.floatArStatus[threadID, num47] == 1)
							{
								num49++;
								num31 += Math.Abs(global::MainGame.MainGame.playerVehicles[0].ph1.velocityX * global::Collision.Collision.floatArDir[threadID, num47, 0] + global::MainGame.MainGame.playerVehicles[0].ph1.velocityY * global::Collision.Collision.floatArDir[threadID, num47, 1] + global::MainGame.MainGame.playerVehicles[0].ph1.velocityZ * global::Collision.Collision.floatArDir[threadID, num47, 2]);
							}
						}
						if (num49 > 0)
						{
							num31 /= (float)num49;
							num45 = num31;
						}
					}
					if (num45 > players[0].velocityTerminalThreshold && Player_Injured_Threaded(num45))
					{
						Player_Over(0, playerDied: true, threadID);
						mainC.vehicles.Splash_Damage_From_Vehicle_Explosion(global::MainGame.MainGame.playerVehicles[0].ph1.x, global::MainGame.MainGame.playerVehicles[0].ph1.y, global::MainGame.MainGame.playerVehicles[0].ph1.z + 10f, curVehicle, 0, threadID);
					}
				}
				num = (float)Math.Sqrt(global::MainGame.MainGame.playerVehicles[0].ph1.velocityX * global::MainGame.MainGame.playerVehicles[0].ph1.velocityX + global::MainGame.MainGame.playerVehicles[0].ph1.velocityY * global::MainGame.MainGame.playerVehicles[0].ph1.velocityY + global::MainGame.MainGame.playerVehicles[0].ph1.velocityZ * global::MainGame.MainGame.playerVehicles[0].ph1.velocityZ);
				if (num < 1E-11f || num > 1E+10f)
				{
					global::MainGame.MainGame.playerVehicles[0].ph1.velocityX = 0f;
					global::MainGame.MainGame.playerVehicles[0].ph1.velocityY = 0f;
					global::MainGame.MainGame.playerVehicles[0].ph1.velocityZ = 0f;
					num = 0f;
					num7 = 0f;
					num8 = 0f;
					num9 = 0f;
				}
				global::MainGame.MainGame.playerVehicles[0].ph1.x = players[0].charMain.pos2.v[0];
				global::MainGame.MainGame.playerVehicles[0].ph1.y = players[0].charMain.pos2.v[1];
				global::MainGame.MainGame.playerVehicles[0].ph1.z = players[0].charMain.pos2.v[2];
				num33 -= num34;
			}
			num30 = playerRaces[players[0].race].programTurnRight[players[0].type];
			GameLogic.propRot[0] = 0f;
			if (num30 != 255)
			{
				num17 = 465f + 14900f * global::MainGame.MainGame.playerVehicles[0].throttleSpeed;
				GameLogic.propRot[0] = num17 * frameTime;
				players[0].jt1[num30].pivot2Speed = num17;
				players[0].jt1[num30].targetPivot2 += num17;
				while (players[0].jt1[num30].targetPivot2 > 360f)
				{
					players[0].jt1[num30].targetPivot2 -= 360f;
				}
			}
			players[0].mv[uBufferID] = identity;
			global::Joints.Joints.Reset_Joint_Data(0);
			global::Joints.Joints.Process_Joints_Threaded(0, frameTime, threadID);
			global::MainGame.MainGame.playerVehicles[0].ph1.velocityX = global::MainGame.MainGame.playerVehicles[0].newVelX;
			global::MainGame.MainGame.playerVehicles[0].ph1.velocityY = global::MainGame.MainGame.playerVehicles[0].newVelY;
			global::MainGame.MainGame.playerVehicles[0].ph1.velocityZ = global::MainGame.MainGame.playerVehicles[0].newVelZ;
			players[0].charP.velocity.v[0] = global::MainGame.MainGame.playerVehicles[0].newVelX;
			players[0].charP.velocity.v[1] = global::MainGame.MainGame.playerVehicles[0].newVelY;
			players[0].charP.velocity.v[2] = global::MainGame.MainGame.playerVehicles[0].newVelZ;
			zRotation = (float)Math.Acos((double)identity.M22 / Math.Sqrt(identity.M21 * identity.M21 + identity.M22 * identity.M22)) * 57.29578f;
			if (identity.M21 > 0f)
			{
				zRotation = 360f - zRotation;
			}
			players[0].zRotation = zRotation;
			global::MainGame.MainGame.playerVehicles[0].velocity = num;
			global::MainGame.MainGame.planeVelocity = num;
			Update_Player_Vehicle_BoundingBox(0, threadID, (ushort)num19, ref identity);
			flag = Confine_Player_Position_ToBoundaries_New(ref global::MainGame.MainGame.playerVehicles[0].ph1, terrainHeight, 0) > 0 || flag;
			players[0].charP.position.v[0] = global::MainGame.MainGame.playerVehicles[0].ph1.x;
			players[0].charP.position.v[1] = global::MainGame.MainGame.playerVehicles[0].ph1.y;
			players[0].charP.position.v[2] = global::MainGame.MainGame.playerVehicles[0].ph1.z;
			if (global::Rendering.Rendering.watchingPlayer == 0)
			{
				mainC.inputMain.UI_HUD_Set_Player_Velocity(num2);
			}
			else
			{
				mainC.inputMain.UI_HUD_Set_Player_Velocity(global::MainGame.MainGame.playerVehicles[global::Rendering.Rendering.watchingPlayer].ph1.velocity);
			}
			mainC.inputMain.UI_HUD_Set_Player_Height(global::MainGame.MainGame.playerVehicles[global::Rendering.Rendering.watchingPlayer].ph1.z);
			num17 = num2 / data3 * 0.8f;
			num17 = global::MainGame.MainGame.playerVehicles[0].throttleSpeed * 0.4f + num17;
			mainC.soundsMain.Play_Moving_Continual_Sound(0, 1, stop: false, 0.5f + global::MainGame.MainGame.playerVehicles[0].throttleSpeed * 0.5f, num17 * 100f, global::MainGame.MainGame.playerVehicles[0].ph1.x, global::MainGame.MainGame.playerVehicles[0].ph1.y, global::MainGame.MainGame.playerVehicles[0].ph1.z, global::MainGame.MainGame.playerVehicles[0].ph1.velocityX, global::MainGame.MainGame.playerVehicles[0].ph1.velocityY, global::MainGame.MainGame.playerVehicles[0].ph1.velocityZ);
			if (players[0].damagePercentageCapped > 0.1f)
			{
				Damage_Particles_For_Damaged_Player_Vehicle(0, threadID);
			}
		}
		if ((players[0].onmap & 0x18) > 0)
		{
			if (mainPlayerDeathTimer == 0f)
			{
				if (players[0].damage >= global::MainGame.MainGame.playerVehicles[0].damageThresholdForExplosion)
				{
					flag = true;
				}
				if (flag)
				{
					if (global::MainGame.MainGame.gameMode == 1)
					{
						mainC.networkingMain.XBOX_Send_Network_Message53(53);
					}
					Player_Vehicle_Explodes(0, threadID);
					mainPlayerDeathTimer += frameTime / global::Physics.Physics.timeMod;
					global::MainGame.MainGame.playerVehicles[0].ph1.x = players[0].posX[rBufferID];
					global::MainGame.MainGame.playerVehicles[0].ph1.y = players[0].posY[rBufferID];
					global::MainGame.MainGame.playerVehicles[0].ph1.z = players[0].posZ[rBufferID];
					players[0].charP.position.v[0] = players[0].posX[rBufferID];
					players[0].charP.position.v[1] = players[0].posY[rBufferID];
					players[0].charP.position.v[2] = players[0].posZ[rBufferID];
					ref Matrix reference = ref players[0].mv[uBufferID];
					reference = players[0].mv[rBufferID];
					mainC.gameLogic.Game_Airplane_Crashed();
					players[0].onmap = 16;
				}
			}
			else if (mainPlayerDeathTimer > 0f)
			{
				mainPlayerDeathTimer += frameTime / global::Physics.Physics.timeMod;
				global::MainGame.MainGame.playerVehicles[0].ph1.velocityX = players[0].posX[rBufferID];
				global::MainGame.MainGame.playerVehicles[0].ph1.velocityY = players[0].posY[rBufferID];
				global::MainGame.MainGame.playerVehicles[0].ph1.velocityZ = players[0].posZ[rBufferID];
				players[0].charP.position.v[0] = players[0].posX[rBufferID];
				players[0].charP.position.v[1] = players[0].posY[rBufferID];
				players[0].charP.position.v[2] = players[0].posZ[rBufferID];
				ref Matrix reference2 = ref players[0].mv[uBufferID];
				reference2 = players[0].mv[rBufferID];
				Sync_Local_Player_View();
				if (mainPlayerDeathTimer > 2.5f)
				{
					players[0].onmap = 1;
				}
			}
		}
		global::MainGame.MainGame.debugUpdateCrashCount = 0;
	}

	public void Move_MainPlayer_SpaceShip(float frameTime, byte threadID)
	{
		bool flag = false;
		byte b = 3;
		byte b2 = 8;
		byte b3 = 6;
		float num = 1f;
		float num2 = 0f;
		float num3 = 0f;
		float num4 = 0f;
		float num5 = 0f;
		float num6 = 0f;
		float num7 = 0f;
		float num8 = 0f;
		float num9 = 0f;
		float num10 = 0f;
		float num11 = 0f;
		float value = 0f;
		float value2 = 0f;
		float value3 = 0f;
		float num12 = 0f;
		float num13 = 0f;
		float num14 = 0f;
		Matrix identity = Matrix.Identity;
		byte uBufferID = global::Rendering.Rendering.uBufferID;
		byte rBufferID = global::Rendering.Rendering.rBufferID;
		if ((players[0].onmap & 0x11) == 0)
		{
			ushort curVehicle = players[0].curVehicle;
			Vehicles.vehicles[curVehicle].balanceFactor = 0.15f;
			Vehicles.vehicles[curVehicle].wheelTouchingFactor = 0.15f;
			byte numWheels = Vehicles.vehicles[curVehicle].numWheels;
			short numColPoints = Vehicles.vehicles[curVehicle].numColPoints;
			short num15 = (short)(numWheels + numColPoints);
			identity = players[0].mv[rBufferID];
			float terrainHeight = mainC.terrainMain.Get_Terrain_Height(global::MainGame.MainGame.playerVehicles[0].ph1.x, global::MainGame.MainGame.playerVehicles[0].ph1.y, threadID);
			_ = playerSpeedRotateLeftStick;
			_ = global::InputHandler.InputHandler.lookSensitivity[global::InputHandler.InputHandler.lookMode];
			_ = Vehicles.vehicles[curVehicle].ph1.mass;
			_ = Vehicles.vehicles[curVehicle].data1;
			_ = Vehicles.vehicles[curVehicle].data2;
			_ = Vehicles.vehicles[curVehicle].data3;
			_ = Vehicles.vehicles[curVehicle].data4;
			_ = Vehicles.vehicles[curVehicle].data5;
			float data = Vehicles.vehicles[curVehicle].data6;
			float data2 = Vehicles.vehicles[curVehicle].data7;
			_ = Vehicles.vehicles[curVehicle].data8;
			_ = Vehicles.vehicles[curVehicle].data9;
			_ = Vehicles.vehicles[curVehicle].data10;
			_ = Vehicles.vehicles[curVehicle].data11;
			_ = Vehicles.vehicles[curVehicle].data12;
			float data3 = Vehicles.vehicles[curVehicle].data16;
			global::MainGame.MainGame.playerVehicles[0].throttleSpeed = 0.5f + (global::InputHandler.InputHandler.controllerStickLeftValueY + 1f) * 0.25f;
			float damage;
			if (global::InputHandler.InputHandler.controllerStickLeftValueY > 0.25f)
			{
				global::MainGame.MainGame.playerVehicles[0].curHeat += global::InputHandler.InputHandler.controllerStickLeftValueY * global::MainGame.MainGame.playerVehicles[0].heatGeneration * frameTime;
				if (global::MainGame.MainGame.playerVehicles[0].curHeat > (float)(int)global::MainGame.MainGame.playerVehicles[0].maxHeat)
				{
					global::MainGame.MainGame.playerVehicles[0].curHeat = (int)global::MainGame.MainGame.playerVehicles[0].maxHeat;
					if (players[0].damagePercentageCapped < 0.95f)
					{
						damage = global::MainGame.MainGame.playerVehicles[0].curHeat / (float)(int)global::MainGame.MainGame.playerVehicles[0].maxHeat * global::MainGame.MainGame.playerVehicles[0].overHeatingDamage * frameTime;
						if (Player_Vehicle_Damaged(damage))
						{
							Player_Over(0, playerDied: true, threadID);
							mainC.vehicles.Splash_Damage_From_Vehicle_Explosion(global::MainGame.MainGame.playerVehicles[0].ph1.x, global::MainGame.MainGame.playerVehicles[0].ph1.y, global::MainGame.MainGame.playerVehicles[0].ph1.z + 10f, curVehicle, 0, threadID);
						}
						Adjust_Player_Damage(0, damage, sendOnline: true);
					}
				}
			}
			else
			{
				global::MainGame.MainGame.playerVehicles[0].curHeat -= (1f - global::InputHandler.InputHandler.controllerStickLeftValueY * 0.5f) * global::MainGame.MainGame.playerVehicles[0].heatDissipation * frameTime;
				if (global::MainGame.MainGame.playerVehicles[0].curHeat < 0f)
				{
					global::MainGame.MainGame.playerVehicles[0].curHeat = 0f;
				}
			}
			if (!controlsInUse)
			{
				num = Set_Speed_If_Targeting_Enemy(0, frameTime, 225f, 14400f, 300f, doCollisionCheck: false, global::InputHandler.InputHandler.controllerStickRightValX, threadID);
				float num16 = 0.5f * frameTime * frameTime;
				damage = global::InputHandler.InputHandler.controllerStickRightValueX - global::InputHandler.InputHandler.stickRightX;
				float num17 = damage * global::MainGame.MainGame.playerVehicles[0].controllerSpring - global::MainGame.MainGame.playerVehicles[0].controllerDampening * global::InputHandler.InputHandler.stickRightXVel;
				global::InputHandler.InputHandler.stickRightX += global::InputHandler.InputHandler.stickRightXVel * frameTime + num17 * num16;
				global::InputHandler.InputHandler.stickRightXVel += num17 * frameTime;
				num5 = global::InputHandler.InputHandler.stickRightX;
				damage = (Math.Abs(global::InputHandler.InputHandler.stickRightX) - global::InputHandler.InputHandler.controllerStickRightSmoothX) * frameTime;
				damage = ((!(damage > 0f)) ? (damage * 5f) : (damage * (1f + Math.Abs(global::MainGame.MainGame.arcadeModeRotAngle[0] / ((float)Math.PI / 2f)) * 0.5f + Math.Abs(global::InputHandler.InputHandler.stickRightX) * 0.5f)));
				global::InputHandler.InputHandler.controllerStickRightSmoothX += damage;
				if (global::InputHandler.InputHandler.controllerStickRightSmoothX > 1f)
				{
					global::InputHandler.InputHandler.controllerStickRightSmoothX = 1f;
				}
				else if (global::InputHandler.InputHandler.controllerStickRightSmoothX < 0f)
				{
					global::InputHandler.InputHandler.controllerStickRightSmoothX = 0f;
				}
				num5 *= 0.05f + global::InputHandler.InputHandler.controllerStickRightSmoothX * 0.95f;
				num5 += (global::InputHandler.InputHandler.controllerStickRightValueX - num5) * global::InputHandler.InputHandler.lookSensitivity[global::InputHandler.InputHandler.lookMode];
				damage = global::InputHandler.InputHandler.controllerStickRightValueY - global::InputHandler.InputHandler.stickRightY;
				num17 = damage * global::MainGame.MainGame.playerVehicles[0].controllerSpring - global::MainGame.MainGame.playerVehicles[0].controllerDampening * global::InputHandler.InputHandler.stickRightYVel;
				global::InputHandler.InputHandler.stickRightY += global::InputHandler.InputHandler.stickRightYVel * frameTime + num17 * num16;
				global::InputHandler.InputHandler.stickRightYVel += num17 * frameTime;
				num4 = (0f - global::InputHandler.InputHandler.stickRightY) * invertY;
				damage = (Math.Abs(global::InputHandler.InputHandler.stickRightY) - global::InputHandler.InputHandler.controllerStickRightSmoothY) * frameTime;
				damage = ((!(damage > 0f)) ? (damage * 5f) : (damage * (1f + Math.Abs(global::MainGame.MainGame.arcadeModeRisingAngle[0] / ((float)Math.PI * 89f / 180f)) * 0.5f + Math.Abs(global::InputHandler.InputHandler.stickRightY) * 0.5f)));
				global::InputHandler.InputHandler.controllerStickRightSmoothY += damage;
				if (global::InputHandler.InputHandler.controllerStickRightSmoothY > 1f)
				{
					global::InputHandler.InputHandler.controllerStickRightSmoothY = 1f;
				}
				else if (global::InputHandler.InputHandler.controllerStickRightSmoothY < 0f)
				{
					global::InputHandler.InputHandler.controllerStickRightSmoothY = 0f;
				}
				num4 *= 0.05f + global::InputHandler.InputHandler.controllerStickRightSmoothY * 0.95f;
				num4 += (0f - global::InputHandler.InputHandler.controllerStickRightValueY - num4) * global::InputHandler.InputHandler.lookSensitivity[global::InputHandler.InputHandler.lookMode];
			}
			int num18 = playerRaces[players[0].race].programTurnLeft[players[0].type];
			if (num18 < 255)
			{
				if (global::MainGame.MainGame.activateRetracts)
				{
					global::MainGame.MainGame.activateRetracts = false;
					global::MainGame.MainGame.gearDown[0] = 0;
					players[0].pg1[num18].inReverse = false;
					global::Networking.Networking.networkBytes[0] = (byte)num18;
					global::Networking.Networking.networkBytes[1] = 1;
					global::Networking.Networking.networkBytes[2] = players[0].race;
					global::Networking.Networking.networkBytes[3] = (byte)players[0].type;
					global::Networking.Networking.networkShorts[0] = players[0].pg1[num18].curStep;
					if (players[0].pg1[num18].status != 2)
					{
						gearSound = mainC.soundsMain.Play_Priority_Sound("Hydraulics", global::MainGame.MainGame.playerVehicles[0].ph1.x, global::MainGame.MainGame.playerVehicles[0].ph1.y, global::MainGame.MainGame.playerVehicles[0].ph1.z, 0f, 0f, 0f);
					}
					if (players[0].pg1[num18].curStep < 1)
					{
						global::MainGame.MainGame.gearDown[0] = 1;
						players[0].pg1[num18].inReverse = true;
						global::Networking.Networking.networkBytes[1] = 0;
					}
					mainC.programsMain.Set_Animation_To_Reverse_Direction(ref players[0].animations, num18);
				}
				if (players[0].animations[num18].status == 2)
				{
					b = players[0].headJoint;
					b3 = players[0].shoulderJointL;
					b2 = players[0].shoulderJointR;
					mainC.vehicles.Update_Wheel_Positions(ref global::MainGame.MainGame.playerVehicles[0], curVehicle, 0, players[0].jt1[b].rotX, players[0].jt1[b].rotY, players[0].jt1[b].rotZ);
					mainC.vehicles.Update_Wheel_Positions(ref global::MainGame.MainGame.playerVehicles[0], curVehicle, 1, players[0].jt1[b2].rotX, players[0].jt1[b2].rotY, players[0].jt1[b2].rotZ);
					mainC.vehicles.Update_Wheel_Positions(ref global::MainGame.MainGame.playerVehicles[0], curVehicle, 2, players[0].jt1[b3].rotX, players[0].jt1[b3].rotY, players[0].jt1[b3].rotZ);
				}
			}
			if (needToReload)
			{
				if (!reloading)
				{
					reloading = mainC.playersMain.Player_Needs_To_Reload(0);
				}
				mainC.weaponsMain.Check_Weapon_Views();
				needToReload = false;
			}
			else if (needToChamber)
			{
				if (!chambering)
				{
					chambering = Player_Needs_To_Chamber(0);
				}
				mainC.weaponsMain.Check_Weapon_Views();
				needToChamber = false;
			}
			if ((global::MainGame.MainGame.viewFollowingObject || controlsInUse) && players[0].onmap != 8 && global::MainGame.MainGame.playerVehicles[0].wheelTouchingTimer == 0f)
			{
				if (identity.M23 < 0f)
				{
					num4 = 0.3f;
				}
				else
				{
					float num16 = mainC.terrainMain.Get_Terrain_Height(global::MainGame.MainGame.playerVehicles[0].ph1.x, global::MainGame.MainGame.playerVehicles[0].ph1.y, threadID);
					if (global::MainGame.MainGame.playerVehicles[0].ph1.z < num16 + 500f)
					{
						float num17 = 10f;
						float num19 = mainC.terrainMain.Get_Terrain_Height(global::MainGame.MainGame.playerVehicles[0].ph1.x + 10f * identity.M21, global::MainGame.MainGame.playerVehicles[0].ph1.y + 10f * identity.M22, threadID);
						float num20 = mainC.terrainMain.Get_Terrain_Height(global::MainGame.MainGame.playerVehicles[0].ph1.x + 30f * identity.M21, global::MainGame.MainGame.playerVehicles[0].ph1.y + 30f * identity.M22, threadID);
						if ((num20 - num16) / 30f > (num19 - num16) / 10f)
						{
							num19 = num20;
							num17 = 30f;
						}
						num20 = mainC.terrainMain.Get_Terrain_Height(global::MainGame.MainGame.playerVehicles[0].ph1.x + 60f * identity.M21, global::MainGame.MainGame.playerVehicles[0].ph1.y + 60f * identity.M22, threadID);
						if ((num20 - num16) / 60f > (num19 - num16) / num17)
						{
							num19 = num20;
							num17 = 60f;
						}
						damage = (float)Math.Sqrt(identity.M21 * identity.M21 + identity.M22 * identity.M22);
						num20 = num17 / damage * identity.M23;
						if (num20 < num19)
						{
							num4 = 0.3f;
						}
					}
				}
			}
			float num21 = frameTime / global::Physics.Physics.timeMod;
			float num22 = num21;
			while (num21 > 0f)
			{
				if (num22 > num21)
				{
					num22 = num21;
				}
				Matrix matrix = identity;
				players[0].charMain.pos1.v[0] = global::MainGame.MainGame.playerVehicles[0].ph1.x;
				players[0].charMain.pos1.v[1] = global::MainGame.MainGame.playerVehicles[0].ph1.y;
				players[0].charMain.pos1.v[2] = global::MainGame.MainGame.playerVehicles[0].ph1.z;
				float m = matrix.M21;
				float m2 = matrix.M22;
				float m3 = matrix.M23;
				float m4 = matrix.M31;
				float m5 = matrix.M32;
				float m6 = matrix.M33;
				float m7 = matrix.M11;
				float m8 = matrix.M12;
				float m9 = matrix.M13;
				num2 = (float)Math.Sqrt(global::MainGame.MainGame.playerVehicles[0].ph1.velocityX * global::MainGame.MainGame.playerVehicles[0].ph1.velocityX + global::MainGame.MainGame.playerVehicles[0].ph1.velocityY * global::MainGame.MainGame.playerVehicles[0].ph1.velocityY + global::MainGame.MainGame.playerVehicles[0].ph1.velocityZ * global::MainGame.MainGame.playerVehicles[0].ph1.velocityZ);
				if (num2 < 1E-11f || num2 > 1E+10f)
				{
					global::MainGame.MainGame.playerVehicles[0].ph1.velocityX = 0f;
					global::MainGame.MainGame.playerVehicles[0].ph1.velocityY = 0f;
					global::MainGame.MainGame.playerVehicles[0].ph1.velocityZ = 0f;
					num2 = 0f;
				}
				global::MainGame.MainGame.playerVehicles[0].ph1.velocity = num2;
				num6 = 0f;
				num7 = 0f;
				num8 = 0f;
				if (num2 > 0f)
				{
					num6 = global::MainGame.MainGame.playerVehicles[0].ph1.velocityX / num2;
					num7 = global::MainGame.MainGame.playerVehicles[0].ph1.velocityY / num2;
					num8 = global::MainGame.MainGame.playerVehicles[0].ph1.velocityZ / num2;
				}
				num9 = global::MainGame.MainGame.playerVehicles[0].ph1.velocityX;
				num10 = global::MainGame.MainGame.playerVehicles[0].ph1.velocityY;
				num11 = global::MainGame.MainGame.playerVehicles[0].ph1.velocityZ;
				float num23 = num6 * m + num7 * m2 + num8 * m3;
				num3 = num2 * num23;
				damage = global::MainGame.MainGame.playerVehicles[0].throttleSpeed * data2 - num2;
				float num17 = data * data2 * (float)Math.Sign(damage);
				if (Math.Abs(damage) < Math.Abs(num17))
				{
					num17 = damage;
				}
				num2 += num17 * num22;
				global::MainGame.MainGame.arcadeModeRotAngle[0] += num5 * (4f + global::InputHandler.InputHandler.lookSensitivity[global::InputHandler.InputHandler.lookMode]) * num22;
				global::MainGame.MainGame.arcadeModeRisingAngle[0] += num4 * 1.5f * num22;
				if (Math.Abs(global::MainGame.MainGame.arcadeModeRotAngle[0]) > (float)Math.PI / 2f)
				{
					global::InputHandler.InputHandler.stickRightXVel = 0f;
					global::InputHandler.InputHandler.stickRightX = global::InputHandler.InputHandler.controllerStickRightValueX;
					global::MainGame.MainGame.arcadeModeRotAngle[0] = (float)Math.PI / 2f * (float)Math.Sign(global::MainGame.MainGame.arcadeModeRotAngle[0]);
				}
				if (Math.Abs(global::MainGame.MainGame.arcadeModeRisingAngle[0]) > (float)Math.PI * 89f / 180f)
				{
					global::InputHandler.InputHandler.stickRightYVel = 0f;
					global::InputHandler.InputHandler.stickRightY = global::InputHandler.InputHandler.controllerStickRightValueY;
					global::MainGame.MainGame.arcadeModeRisingAngle[0] = (float)Math.PI * 89f / 180f * (float)Math.Sign(global::MainGame.MainGame.arcadeModeRisingAngle[0]);
				}
				if (global::InputHandler.InputHandler.controllerStickRightValueX == 0f && Math.Abs(global::MainGame.MainGame.arcadeModeRotAngle[0]) < 0.08726646f)
				{
					global::MainGame.MainGame.arcadeModeRotAngle[0] -= global::MainGame.MainGame.arcadeModeRotAngle[0] * 0.85f * frameTime;
				}
				global::MainGame.MainGame.playerVehicles[0].ph1.torqueX = 0f;
				global::MainGame.MainGame.playerVehicles[0].ph1.torqueY = 0f;
				global::MainGame.MainGame.playerVehicles[0].ph1.torqueZ = 0f;
				players[0].zRotation -= 90f * (global::MainGame.MainGame.arcadeModeRotAngle[0] / ((float)Math.PI / 2f)) * data3 * num22 * num;
				identity = Matrix.CreateRotationY(global::MainGame.MainGame.arcadeModeRotAngle[0]) * Matrix.CreateRotationX(global::MainGame.MainGame.arcadeModeRisingAngle[0]) * Matrix.CreateRotationZ(players[0].zRotation * ((float)Math.PI / 180f));
				global::MainGame.MainGame.playerVehicles[0].ph1.velocityX = identity.M21 * num2;
				global::MainGame.MainGame.playerVehicles[0].ph1.velocityY = identity.M22 * num2;
				global::MainGame.MainGame.playerVehicles[0].ph1.velocityZ = identity.M23 * num2;
				mainC.physicsMain.getPosition_new(ref global::MainGame.MainGame.playerVehicles[0].ph1, num22);
				global::MainGame.MainGame.playerVehicles[0].ph1.forceX = 0f;
				global::MainGame.MainGame.playerVehicles[0].ph1.forceY = 0f;
				global::MainGame.MainGame.playerVehicles[0].ph1.forceZ = 0f;
				global::MainGame.MainGame.playerVehicles[0].ph1.torqueX = 0f;
				global::MainGame.MainGame.playerVehicles[0].ph1.torqueY = 0f;
				global::MainGame.MainGame.playerVehicles[0].ph1.torqueZ = 0f;
				_ = global::MainGame.MainGame.playerVehicles[0].ph1.x;
				_ = players[0].charMain.pos1.v[0];
				_ = global::MainGame.MainGame.playerVehicles[0].ph1.y;
				_ = players[0].charMain.pos1.v[1];
				_ = global::MainGame.MainGame.playerVehicles[0].ph1.z;
				_ = players[0].charMain.pos1.v[2];
				global::MainGame.MainGame.playerVehicles[0].xBalanceTimer -= num22;
				global::MainGame.MainGame.playerVehicles[0].yBalanceTimer -= num22;
				global::MainGame.MainGame.playerVehicles[0].zBalanceTimer -= num22;
				global::MainGame.MainGame.playerVehicles[0].wheelTouchingTimer -= num22;
				if (global::MainGame.MainGame.playerVehicles[0].xBalanceTimer <= 0f)
				{
					global::MainGame.MainGame.playerVehicles[0].xBalanceTimer = 0f;
				}
				if (global::MainGame.MainGame.playerVehicles[0].yBalanceTimer <= 0f)
				{
					global::MainGame.MainGame.playerVehicles[0].yBalanceTimer = 0f;
				}
				if (global::MainGame.MainGame.playerVehicles[0].zBalanceTimer <= 0f)
				{
					global::MainGame.MainGame.playerVehicles[0].zBalanceTimer = 0f;
				}
				if (global::MainGame.MainGame.playerVehicles[0].wheelTouchingTimer <= 0f)
				{
					global::MainGame.MainGame.playerVehicles[0].wheelTouchingTimer = 0f;
				}
				if (global::MainGame.MainGame.playerVehicles[0].wheelTouchingTimer > 0f)
				{
					damage = Math.Abs(num3 / 200f);
					if (damage > 1f)
					{
						damage = 1f;
					}
					else if (damage < 0.3f)
					{
						damage = 0f;
					}
					mainC.inputMain.GamePad_Vibration_Set_Low(1f * damage);
				}
				float num24 = 0f;
				collisionObjectID = -1;
				players[0].charMain.pos2.v[0] = global::MainGame.MainGame.playerVehicles[0].ph1.x;
				players[0].charMain.pos2.v[1] = global::MainGame.MainGame.playerVehicles[0].ph1.y;
				players[0].charMain.pos2.v[2] = global::MainGame.MainGame.playerVehicles[0].ph1.z;
				global::MainGame.MainGame.playerVehicles[0].newVelX = global::MainGame.MainGame.playerVehicles[0].ph1.velocityX;
				global::MainGame.MainGame.playerVehicles[0].newVelY = global::MainGame.MainGame.playerVehicles[0].ph1.velocityY;
				global::MainGame.MainGame.playerVehicles[0].newVelZ = global::MainGame.MainGame.playerVehicles[0].ph1.velocityZ;
				int num25 = numWheels * 3;
				short num26 = 0;
				int num27 = 0;
				int num28 = 0;
				int num29 = 129;
				while (num26 < num25)
				{
					damage = global::MainGame.MainGame.playerVehicles[0].momentum.collisionPoints[num28++];
					num17 = global::MainGame.MainGame.playerVehicles[0].momentum.collisionPoints[num28++];
					float num16 = global::MainGame.MainGame.playerVehicles[0].momentum.collisionPoints[num28++];
					global::Collision.Collision.floatAr[threadID, num26++] = damage * matrix.M11 + num17 * matrix.M21 + num16 * matrix.M31;
					global::Collision.Collision.floatAr[threadID, num26++] = damage * matrix.M12 + num17 * matrix.M22 + num16 * matrix.M32;
					global::Collision.Collision.floatAr[threadID, num26++] = damage * matrix.M13 + num17 * matrix.M23 + num16 * matrix.M33;
					global::Collision.Collision.floatAr[threadID, num29++] = damage * identity.M11 + num17 * identity.M21 + num16 * identity.M31;
					global::Collision.Collision.floatAr[threadID, num29++] = damage * identity.M12 + num17 * identity.M22 + num16 * identity.M32;
					global::Collision.Collision.floatAr[threadID, num29++] = damage * identity.M13 + num17 * identity.M23 + num16 * identity.M33;
					num27++;
				}
				num25 = num15 * 3;
				while (num26 < num25)
				{
					damage = global::MainGame.MainGame.playerVehicles[0].momentum.collisionPoints[num28++];
					num17 = global::MainGame.MainGame.playerVehicles[0].momentum.collisionPoints[num28++];
					float num16 = global::MainGame.MainGame.playerVehicles[0].momentum.collisionPoints[num28++];
					global::Collision.Collision.floatAr[threadID, num26++] = damage * matrix.M11 + num17 * matrix.M21 + num16 * matrix.M31;
					global::Collision.Collision.floatAr[threadID, num26++] = damage * matrix.M12 + num17 * matrix.M22 + num16 * matrix.M32;
					global::Collision.Collision.floatAr[threadID, num26++] = damage * matrix.M13 + num17 * matrix.M23 + num16 * matrix.M33;
					global::Collision.Collision.floatAr[threadID, num29++] = damage * identity.M11 + num17 * identity.M21 + num16 * identity.M31;
					global::Collision.Collision.floatAr[threadID, num29++] = damage * identity.M12 + num17 * identity.M22 + num16 * identity.M32;
					global::Collision.Collision.floatAr[threadID, num29++] = damage * identity.M13 + num17 * identity.M23 + num16 * identity.M33;
					num27++;
				}
				Update_Player_Vehicle_BoundingBox(0, threadID, (ushort)num15, ref identity);
				flag = Confine_Player_Position_ToBoundaries_New(ref global::MainGame.MainGame.playerVehicles[0].ph1, terrainHeight, 0) > 0 || flag;
				float num30 = players[0].charMain.pos1.v[0];
				float num31 = players[0].charMain.pos1.v[1];
				float num32 = players[0].charMain.pos1.v[2];
				float num33 = players[0].charMain.pos2.v[0];
				float num34 = players[0].charMain.pos2.v[1];
				float num35 = players[0].charMain.pos2.v[2];
				players[0].charMain.pos1.v[0] = num30;
				players[0].charMain.pos1.v[1] = num31;
				players[0].charMain.pos1.v[2] = num32;
				players[0].charMain.pos2.v[0] = num33;
				players[0].charMain.pos2.v[1] = num34;
				players[0].charMain.pos2.v[2] = num35;
				global::Collision.Collision.hitGround = false;
				mainC.collisionMain.ResetIgnoreList(threadID, (ushort)num15);
				int num36 = mainC.collisionMain.CheckCollision_Detailed_List(ref players[0].charMain, num15, 129, 143, 0, threadID);
				global::MainGame.MainGame.numWheelsTouching = 0;
				for (num18 = 0; num18 < numWheels; num18++)
				{
					if (global::Collision.Collision.floatArStatus[threadID, num18] == 1)
					{
						global::MainGame.MainGame.numWheelsTouching++;
						global::MainGame.MainGame.playerVehicles[0].wheelTouchingTimer = Vehicles.vehicles[curVehicle].wheelTouchingFactor;
					}
				}
				if (num36 > 0)
				{
					flag = true;
					if (num2 > 100f)
					{
						num26 = 0;
						num27 = 0;
						while (num26 < num25)
						{
							if (global::Collision.Collision.floatArStatus[threadID, num27] == 1)
							{
								mainC.renderingMain.New_Particle_New(2, global::MainGame.MainGame.playerVehicles[0].ph1.x + global::Collision.Collision.floatAr[threadID, num26], global::MainGame.MainGame.playerVehicles[0].ph1.y + global::Collision.Collision.floatAr[threadID, num26 + 1], global::MainGame.MainGame.playerVehicles[0].ph1.z + global::Collision.Collision.floatAr[threadID, num26 + 2], 0f, 0f, global::MainGame.MainGame.playerVehicles[0].ph1.velocityZ, 0, threadID);
							}
							num26 += 3;
							num27++;
						}
					}
					num28 = 0;
					float num37 = 0f;
					for (num26 = 0; num26 < num15; num26++)
					{
						if (global::Collision.Collision.floatArID[threadID, num26] > -1)
						{
							collisionObjectID = global::Collision.Collision.floatArID[threadID, num26];
						}
						if (global::Collision.Collision.floatArStatus[threadID, num26] == 1)
						{
							num28++;
							num37 += Math.Abs(global::MainGame.MainGame.playerVehicles[0].ph1.velocityX * global::Collision.Collision.floatArDir[threadID, num26, 0] + global::MainGame.MainGame.playerVehicles[0].ph1.velocityY * global::Collision.Collision.floatArDir[threadID, num26, 1] + global::MainGame.MainGame.playerVehicles[0].ph1.velocityZ * global::Collision.Collision.floatArDir[threadID, num26, 2]);
							if (num26 >= numWheels)
							{
								num37 = (float)num28 * players[0].velocityTerminalThreshold + 100f;
							}
						}
					}
					if (num28 > 0)
					{
						num37 /= (float)num28;
						num24 = num37;
					}
					float dX = players[0].charMain.pos2.v[0] - players[0].charMain.pos1.v[0];
					float dY = players[0].charMain.pos2.v[1] - players[0].charMain.pos1.v[1];
					float dZ = players[0].charMain.pos2.v[2] - players[0].charMain.pos1.v[2];
					damage = mainC.physicsMain.getTimeForDistanceTraveled(num9, num10, num11, global::MainGame.MainGame.playerVehicles[0].ph1.accelerationX, global::MainGame.MainGame.playerVehicles[0].ph1.accelerationY, global::MainGame.MainGame.playerVehicles[0].ph1.accelerationZ, dX, dY, dZ, num22);
					mainC.vehicles.Calculate_Vehicle_Collision_Data(ref global::MainGame.MainGame.playerVehicles[0], curVehicle, uBufferID, threadID);
					num18 = 3;
					damage = num22 - damage;
					while (damage > 0f && num18-- > 0)
					{
						float num38 = global::MainGame.MainGame.playerVehicles[0].newVelX;
						float num39 = global::MainGame.MainGame.playerVehicles[0].newVelY;
						float num40 = global::MainGame.MainGame.playerVehicles[0].newVelZ;
						for (num26 = 0; num26 < num15; num26++)
						{
							if (global::Collision.Collision.floatArStatus[threadID, num26] == 1)
							{
								dX = global::Collision.Collision.floatArDir[threadID, num26, 0];
								dY = global::Collision.Collision.floatArDir[threadID, num26, 1];
								dZ = global::Collision.Collision.floatArDir[threadID, num26, 2];
								if (Math.Abs(dX) < 0.0001f)
								{
									dX = 0f;
								}
								if (Math.Abs(dY) < 0.0001f)
								{
									dY = 0f;
								}
								if (Math.Abs(dZ) < 0.0001f)
								{
									dZ = 0f;
								}
								num17 = num38 * dX + num39 * dY + num40 * dZ;
								if ((float)(Math.Sign(dX) * Math.Sign(value)) + num12 < 0f)
								{
									num38 = 0f;
									dX = 0f;
									num12 = -10f;
								}
								if ((float)(Math.Sign(dY) * Math.Sign(value2)) + num13 < 0f)
								{
									num39 = 0f;
									dY = 0f;
									num13 = -10f;
								}
								if ((float)(Math.Sign(dZ) * Math.Sign(value3)) + num14 < 0f)
								{
									num40 = 0f;
									dZ = 0f;
									num14 = -10f;
								}
								if (num17 < 0f)
								{
									if (dX != 0f)
									{
										value = dX;
									}
									if (dY != 0f)
									{
										value2 = dY;
									}
									if (dZ != 0f)
									{
										value3 = dZ;
									}
									num38 -= num17 * dX;
									num39 -= num17 * dY;
									num40 -= num17 * dZ;
								}
								_ = global::Collision.Collision.floatArID[threadID, num26];
							}
						}
						global::MainGame.MainGame.playerVehicles[0].ph1.velocityX = num38;
						global::MainGame.MainGame.playerVehicles[0].ph1.velocityY = num39;
						global::MainGame.MainGame.playerVehicles[0].ph1.velocityZ = num40;
						global::MainGame.MainGame.playerVehicles[0].ph1.x = players[0].charMain.pos2.v[0];
						global::MainGame.MainGame.playerVehicles[0].ph1.y = players[0].charMain.pos2.v[1];
						global::MainGame.MainGame.playerVehicles[0].ph1.z = players[0].charMain.pos2.v[2];
						players[0].charMain.pos1.v[0] = global::MainGame.MainGame.playerVehicles[0].ph1.x;
						players[0].charMain.pos1.v[1] = global::MainGame.MainGame.playerVehicles[0].ph1.y;
						players[0].charMain.pos1.v[2] = global::MainGame.MainGame.playerVehicles[0].ph1.z;
						num9 = global::MainGame.MainGame.playerVehicles[0].ph1.velocityX;
						num10 = global::MainGame.MainGame.playerVehicles[0].ph1.velocityY;
						num11 = global::MainGame.MainGame.playerVehicles[0].ph1.velocityZ;
						mainC.physicsMain.getPosition_new(ref global::MainGame.MainGame.playerVehicles[0].ph1, damage);
						players[0].charMain.pos2.v[0] = global::MainGame.MainGame.playerVehicles[0].ph1.x;
						players[0].charMain.pos2.v[1] = global::MainGame.MainGame.playerVehicles[0].ph1.y;
						players[0].charMain.pos2.v[2] = global::MainGame.MainGame.playerVehicles[0].ph1.z;
						mainC.collisionMain.ResetIgnoreList(threadID, (ushort)num15);
						num36 = mainC.collisionMain.CheckCollision_Detailed_List(ref players[0].charMain, num15, 129, 135, 0, threadID);
						collisionWithGround += global::Collision.Collision.floatArMovDir[threadID, 0];
						num17 = damage;
						damage = 0f;
						if (num36 <= 0)
						{
							continue;
						}
						dX = players[0].charMain.pos2.v[0] - players[0].charMain.pos1.v[0];
						dY = players[0].charMain.pos2.v[1] - players[0].charMain.pos1.v[1];
						dZ = players[0].charMain.pos2.v[2] - players[0].charMain.pos1.v[2];
						float num16 = mainC.physicsMain.getTimeForDistanceTraveled(num9, num10, num11, global::MainGame.MainGame.playerVehicles[0].ph1.accelerationX, global::MainGame.MainGame.playerVehicles[0].ph1.accelerationY, global::MainGame.MainGame.playerVehicles[0].ph1.accelerationZ, dX, dY, dZ, num17);
						mainC.vehicles.Calculate_Vehicle_Collision_Data(ref global::MainGame.MainGame.playerVehicles[0], curVehicle, uBufferID, threadID);
						damage = num17 - num16;
						num28 = 0;
						float num19 = 0f;
						for (num26 = 0; num26 < num15; num26++)
						{
							if (global::Collision.Collision.floatArID[threadID, num26] > -1)
							{
								collisionObjectID = global::Collision.Collision.floatArID[threadID, num26];
							}
							if (global::Collision.Collision.floatArStatus[threadID, num26] == 1)
							{
								num28++;
								num19 += Math.Abs(global::MainGame.MainGame.playerVehicles[0].ph1.velocityX * global::Collision.Collision.floatArDir[threadID, num26, 0] + global::MainGame.MainGame.playerVehicles[0].ph1.velocityY * global::Collision.Collision.floatArDir[threadID, num26, 1] + global::MainGame.MainGame.playerVehicles[0].ph1.velocityZ * global::Collision.Collision.floatArDir[threadID, num26, 2]);
							}
						}
						if (num28 > 0)
						{
							num19 /= (float)num28;
							num24 = num19;
						}
					}
					if (num24 > players[0].velocityTerminalThreshold && Player_Injured_Threaded(num24))
					{
						Player_Over(0, playerDied: true, threadID);
						mainC.vehicles.Splash_Damage_From_Vehicle_Explosion(global::MainGame.MainGame.playerVehicles[0].ph1.x, global::MainGame.MainGame.playerVehicles[0].ph1.y, global::MainGame.MainGame.playerVehicles[0].ph1.z + 10f, curVehicle, 0, threadID);
					}
				}
				num2 = (float)Math.Sqrt(global::MainGame.MainGame.playerVehicles[0].ph1.velocityX * global::MainGame.MainGame.playerVehicles[0].ph1.velocityX + global::MainGame.MainGame.playerVehicles[0].ph1.velocityY * global::MainGame.MainGame.playerVehicles[0].ph1.velocityY + global::MainGame.MainGame.playerVehicles[0].ph1.velocityZ * global::MainGame.MainGame.playerVehicles[0].ph1.velocityZ);
				if (num2 < 1E-11f || num2 > 1E+10f)
				{
					global::MainGame.MainGame.playerVehicles[0].ph1.velocityX = 0f;
					global::MainGame.MainGame.playerVehicles[0].ph1.velocityY = 0f;
					global::MainGame.MainGame.playerVehicles[0].ph1.velocityZ = 0f;
					num2 = 0f;
					num6 = 0f;
					num7 = 0f;
					num8 = 0f;
				}
				global::MainGame.MainGame.playerVehicles[0].ph1.x = players[0].charMain.pos2.v[0];
				global::MainGame.MainGame.playerVehicles[0].ph1.y = players[0].charMain.pos2.v[1];
				global::MainGame.MainGame.playerVehicles[0].ph1.z = players[0].charMain.pos2.v[2];
				num21 -= num22;
			}
			players[0].mv[uBufferID] = identity;
			global::MainGame.MainGame.playerVehicles[0].mv[uBufferID] = identity;
			global::Joints.Joints.Reset_Joint_Data(0);
			global::Joints.Joints.Process_Joints_Threaded(0, frameTime, threadID);
			global::MainGame.MainGame.playerVehicles[0].ph1.velocityX = global::MainGame.MainGame.playerVehicles[0].newVelX;
			global::MainGame.MainGame.playerVehicles[0].ph1.velocityY = global::MainGame.MainGame.playerVehicles[0].newVelY;
			global::MainGame.MainGame.playerVehicles[0].ph1.velocityZ = global::MainGame.MainGame.playerVehicles[0].newVelZ;
			players[0].charP.velocity.v[0] = global::MainGame.MainGame.playerVehicles[0].newVelX;
			players[0].charP.velocity.v[1] = global::MainGame.MainGame.playerVehicles[0].newVelY;
			players[0].charP.velocity.v[2] = global::MainGame.MainGame.playerVehicles[0].newVelZ;
			zRotation = (float)Math.Acos((double)identity.M22 / Math.Sqrt(identity.M21 * identity.M21 + identity.M22 * identity.M22)) * 57.29578f;
			if (identity.M21 > 0f)
			{
				zRotation = 360f - zRotation;
			}
			players[0].zRotation = zRotation;
			global::MainGame.MainGame.playerVehicles[0].velocity = num2;
			global::MainGame.MainGame.planeVelocity = num2;
			Update_Player_Vehicle_BoundingBox(0, threadID, (ushort)num15, ref identity);
			flag = Confine_Player_Position_ToBoundaries_New(ref global::MainGame.MainGame.playerVehicles[0].ph1, terrainHeight, 0) > 0 || flag;
			players[0].charP.position.v[0] = global::MainGame.MainGame.playerVehicles[0].ph1.x;
			players[0].charP.position.v[1] = global::MainGame.MainGame.playerVehicles[0].ph1.y;
			players[0].charP.position.v[2] = global::MainGame.MainGame.playerVehicles[0].ph1.z;
			if (global::Rendering.Rendering.watchingPlayer == 0)
			{
				mainC.inputMain.UI_HUD_Set_Player_Velocity(num3);
			}
			else
			{
				mainC.inputMain.UI_HUD_Set_Player_Velocity(global::MainGame.MainGame.playerVehicles[global::Rendering.Rendering.watchingPlayer].ph1.velocity);
			}
			mainC.inputMain.UI_HUD_Set_Player_Height(global::MainGame.MainGame.playerVehicles[global::Rendering.Rendering.watchingPlayer].ph1.z);
			num18 = playerRaces[players[0].race].programTurnRight[players[0].type];
			GameLogic.propRot[0] = 0f;
			if (num18 != 255)
			{
				damage = 465f + 14900f * global::MainGame.MainGame.playerVehicles[0].throttleSpeed;
				GameLogic.propRot[0] = damage * frameTime;
				players[0].jt1[num18].pivot2Speed = damage;
				players[0].jt1[num18].targetPivot2 += damage;
				while (players[0].jt1[num18].targetPivot2 > 360f)
				{
					players[0].jt1[num18].targetPivot2 -= 360f;
				}
			}
			damage = num3 / data2 * 0.8f;
			damage = global::MainGame.MainGame.playerVehicles[0].throttleSpeed * 0.4f + damage;
			mainC.soundsMain.Play_Moving_Continual_Sound(0, 1, stop: false, 0.5f + global::MainGame.MainGame.playerVehicles[0].throttleSpeed * 0.5f, damage * 100f, global::MainGame.MainGame.playerVehicles[0].ph1.x, global::MainGame.MainGame.playerVehicles[0].ph1.y, global::MainGame.MainGame.playerVehicles[0].ph1.z, global::MainGame.MainGame.playerVehicles[0].ph1.velocityX, global::MainGame.MainGame.playerVehicles[0].ph1.velocityY, global::MainGame.MainGame.playerVehicles[0].ph1.velocityZ);
			if (players[0].damagePercentageCapped > 0.1f)
			{
				Damage_Particles_For_Damaged_Player_Vehicle(0, threadID);
			}
		}
		if ((players[0].onmap & 0x18) > 0)
		{
			if (mainPlayerDeathTimer == 0f)
			{
				if (global::MainGame.MainGame.gameMode == 1)
				{
					mainC.networkingMain.XBOX_Send_Network_Message53(53);
				}
				Player_Vehicle_Explodes(0, threadID);
				mainPlayerDeathTimer += frameTime / global::Physics.Physics.timeMod;
				global::MainGame.MainGame.playerVehicles[0].ph1.x = players[0].posX[rBufferID];
				global::MainGame.MainGame.playerVehicles[0].ph1.y = players[0].posY[rBufferID];
				global::MainGame.MainGame.playerVehicles[0].ph1.z = players[0].posZ[rBufferID];
				players[0].charP.position.v[0] = players[0].posX[rBufferID];
				players[0].charP.position.v[1] = players[0].posY[rBufferID];
				players[0].charP.position.v[2] = players[0].posZ[rBufferID];
				ref Matrix reference = ref players[0].mv[uBufferID];
				reference = players[0].mv[rBufferID];
				mainC.gameLogic.Game_Airplane_Crashed();
				players[0].onmap = 16;
			}
			else if (mainPlayerDeathTimer > 0f)
			{
				mainPlayerDeathTimer += frameTime / global::Physics.Physics.timeMod;
				global::MainGame.MainGame.playerVehicles[0].ph1.x = players[0].posX[rBufferID];
				global::MainGame.MainGame.playerVehicles[0].ph1.y = players[0].posY[rBufferID];
				global::MainGame.MainGame.playerVehicles[0].ph1.z = players[0].posZ[rBufferID];
				players[0].charP.position.v[0] = players[0].posX[rBufferID];
				players[0].charP.position.v[1] = players[0].posY[rBufferID];
				players[0].charP.position.v[2] = players[0].posZ[rBufferID];
				ref Matrix reference2 = ref players[0].mv[uBufferID];
				reference2 = players[0].mv[rBufferID];
				Sync_Local_Player_View();
				if (mainPlayerDeathTimer > 2.5f)
				{
					players[0].onmap = 1;
				}
			}
		}
		global::MainGame.MainGame.debugUpdateCrashCount = 0;
	}

	public void Move_MainPlayer_SkateBoard(float frameTime, byte threadID)
	{
	}

	public void Move_MainPlayer_FixedTurret(float frameTime, byte threadID)
	{
		float rotX = 0f;
		float num = 0f;
		byte uBufferID = global::Rendering.Rendering.uBufferID;
		byte rBufferID = global::Rendering.Rendering.rBufferID;
		float num2 = Set_Speed_If_Targeting_Enemy(0, frameTime, 225f, 14400f, 300f, doCollisionCheck: true, global::InputHandler.InputHandler.controllerStickRightValX, threadID);
		if ((moving & 0xC) > 0)
		{
			float num3 = num2;
			float num4 = num2;
			num = num3 * playerSpeedRotateRightStick * frameTime;
			rotX = num4 * playerSpeedElevateRightStick * frameTime;
			mainC.weaponsMain.Move_Weapon_Mount_Player(0, Vehicles.vehicles[players[0].curVehicle].weaponMounts[0], rotX, 0f, num);
			players[0].charP.position.v[0] = players[0].mv[uBufferID].M41;
			players[0].charP.position.v[1] = players[0].mv[uBufferID].M42;
			players[0].charP.position.v[2] = players[0].mv[uBufferID].M43;
			global::MainGame.MainGame.playerVehicles[0].ph1.x = players[0].mv[uBufferID].M41;
			global::MainGame.MainGame.playerVehicles[0].ph1.y = players[0].mv[uBufferID].M42;
			global::MainGame.MainGame.playerVehicles[0].ph1.z = players[0].mv[uBufferID].M43;
			players[0].mv[uBufferID].M41 = 0f;
			players[0].mv[uBufferID].M42 = 0f;
			players[0].mv[uBufferID].M43 = 0f;
			zRotation = (float)Math.Acos((double)players[0].mv[uBufferID].M22 / Math.Sqrt(players[0].mv[uBufferID].M21 * players[0].mv[uBufferID].M21 + players[0].mv[uBufferID].M22 * players[0].mv[uBufferID].M22)) * 57.29578f;
			if (players[0].mv[uBufferID].M21 > 0f)
			{
				zRotation = 360f - zRotation;
			}
			players[0].zRotation = zRotation;
			global::Joints.Joints.Reset_Joint_Data(0);
			global::Joints.Joints.Process_Joints_Threaded(0, frameTime, threadID);
		}
		else
		{
			ref Matrix reference = ref players[0].mv[uBufferID];
			reference = players[0].mv[rBufferID];
			global::Joints.Joints.Reset_Joint_Data(0);
			global::Joints.Joints.Process_Joints_Threaded(0, frameTime, threadID);
		}
		xRotMovement = rotX;
	}

	public void Check_For_Taunting(ushort playerID, float distanceSqr)
	{
		for (ushort num = 0; num < global::MainGame.MainGame.maxGamePlayers; num++)
		{
			players[num].renderWeapon = (byte)(players[num].renderWeapon & -3);
			if (players[num].taunting)
			{
				players[num].renderWeapon = (byte)(players[num].renderWeapon | 2);
			}
		}
		global::Rendering.Rendering.showTauntMessage[global::Rendering.Rendering.uBufferID] = false;
		if (players[playerID].taunting || players[playerID].dead || players[playerID].onmap != 4)
		{
			return;
		}
		ulong teamMask = players[playerID].teamMask;
		float num2 = players[playerID].charP.position.v[0];
		float num3 = players[playerID].charP.position.v[1];
		float num4 = players[playerID].charP.position.v[2];
		for (ushort num = 0; num < global::MainGame.MainGame.maxGamePlayers; num++)
		{
			if (num != playerID && players[num].active && (players[num].onmap & 8) > 0 && (players[num].teamMask & teamMask) == 0)
			{
				float num5 = num2 - players[num].charP.position.v[0];
				float num6 = num3 - players[num].charP.position.v[1];
				float num7 = num4 - players[num].charP.position.v[2];
				if (num5 * num5 + num6 * num6 + num7 * num7 < distanceSqr)
				{
					global::Rendering.Rendering.showTauntMessage[global::Rendering.Rendering.uBufferID] = true;
					break;
				}
			}
		}
	}

	public void Taunt_Player()
	{
		global::Rendering.Rendering.showTauntMessage[global::Rendering.Rendering.uBufferID] = false;
		players[0].taunting = true;
	}

	public byte Get_Random_Taunt()
	{
		byte b = (byte)global::MainGame.MainGame.mainRandom.Next(0, global::MainGame.MainGame.numTaunts);
		for (byte b2 = b; b2 < global::MainGame.MainGame.numTaunts; b2++)
		{
			if (currentPlayerRank >= global::MainGame.MainGame.lockedTauntLevels[b2])
			{
				return b2;
			}
		}
		for (byte b2 = 0; b2 <= b; b2++)
		{
			if (currentPlayerRank >= global::MainGame.MainGame.lockedTauntLevels[b2])
			{
				return b2;
			}
		}
		return 0;
	}

	public void Calculate_Adjustment_Angle_X()
	{
		bool flag = false;
		short num = 1;
		byte uBufferID = global::Rendering.Rendering.uBufferID;
		byte b = playerRaces[players[0].race].shoulderJointR[players[0].type];
		float num2 = players[0].jt1[b].mv[uBufferID].M41 - players[0].weapon1.offset[0, 2].v[0];
		float num3 = players[0].jt1[b].mv[uBufferID].M42 - players[0].weapon1.offset[0, 2].v[1];
		float num4 = players[0].jt1[b].mv[uBufferID].M43 - players[0].weapon1.offset[0, 2].v[2];
		if (num4 < 0f)
		{
			num = -1;
		}
		float num5 = num2 * num2 + num3 * num3;
		float num6 = (float)Math.Sqrt(num5 + num4 * num4);
		num2 = global::Weapons.Weapons.laserPosX[uBufferID] - (players[0].jt1[b].mv[uBufferID].M41 + players[0].charP.position.v[0]);
		num3 = global::Weapons.Weapons.laserPosY[uBufferID] - (players[0].jt1[b].mv[uBufferID].M42 + players[0].charP.position.v[1]);
		num4 = global::Weapons.Weapons.laserPosZ[uBufferID] - (players[0].jt1[b].mv[uBufferID].M43 + players[0].charP.position.v[2]);
		float num7 = (float)Math.Sqrt(num2 * num2 + num3 * num3 + num4 * num4);
		if (num7 == 0f || num6 == 0f || num7 < num6)
		{
			adjustmentAngleX = 0f;
			return;
		}
		float num8 = (float)Math.Sqrt(num5);
		float num9 = (float)Math.Acos(num8 / num6) * 57.29578f;
		float num10 = 180f - num9;
		float num11 = (float)Math.Asin(players[0].weapon1.offset[0, 5].v[2]) * 57.29578f;
		num11 *= (float)num;
		if (num10 - num11 > 180f)
		{
			num10 = 360f - (num10 - num11);
			num11 = 0f;
			flag = true;
		}
		float value = (float)Math.Asin(num6 * (float)Math.Sin((num10 - num11) * ((float)Math.PI / 180f)) / num7) * 57.29578f;
		float num12 = 180f - Math.Abs(value) - (num10 - num11);
		float num13 = 90f - num9;
		float num14 = (flag ? (0f - (90f - num13 + num12)) : (0f - (90f - num13 - num12)));
		num14 *= (float)num;
		float num15 = (float)Math.Asin(num4 / num7) * 57.29578f;
		float num16 = num15 - num14 + (players[0].jt1[shoulderJointL].targetAngle - players[0].jt1[shoulderJointL].rotX);
		adjustmentAngleX += num16 * 12f * global::MainGame.MainGame.frametime;
		if (adjustmentAngleX > 90f)
		{
			adjustmentAngleX = 90f;
		}
		else if (adjustmentAngleX < -90f)
		{
			adjustmentAngleX = -90f;
		}
	}

	public void Calculate_Adjustment_Angle_Z()
	{
	}

	public void New_SinglePlayer_Round(bool minorRestart, byte threadID)
	{
		_ = global::Rendering.Rendering.uBufferID;
		_ = global::Rendering.Rendering.rBufferID;
		if (minorRestart)
		{
			Adjust_Player_Damage_To_Zero(0, sendOnline: false);
			mainC.weaponsMain.Set_Minimum_Ammo(0);
			mainC.soundsMain.Restore_Level_Music();
		}
		if (global::MainGame.MainGame.curSpLevel != global::MainGame.MainGame.gameLevel)
		{
			mainC.levelsMain.Set_Level(global::MainGame.MainGame.gameLevel, threadID);
		}
		for (ushort num = 0; num < global::MainGame.MainGame.maxGamePlayers; num++)
		{
			Reset_Player(num, isActive: false, players[num].race, (byte)players[num].type);
		}
		players[0].active = true;
		mainC.programsMain.Reset_Round(minorRestart);
		mainC.weaponsMain.Reset_Round();
		mainC.mapsMain.Reset_Round();
		mainC.maingameMain.Reset_Round();
		mainC.gameobjectMain.Reset_Round(minorRestart, threadID);
		mainC.fontmoduleMain.Reset_Round();
		mainC.pickupsMain.Reset_Round();
		mainC.targetMain.Reset_Round();
		mainC.switchesMain.Reset_Round(minorRestart);
		mainC.aiMain.Reset_Round(threadID);
		mainC.renderingMain.Reset_Round();
		mainC.soundsMain.Reset_Round(stopNarrator: true);
		mainC.vehicles.Reset_Round();
		mainC.maingameMain.Reset_Game_Achievement_Rewards();
		mainC.avatarMain.Reset_Round();
		needSpawn = true;
		respawnEnabled = false;
		global::MainGame.MainGame.viewChanged = false;
		moving = 0;
		global::MainGame.MainGame.roundOver = false;
		global::MainGame.MainGame.commanderSelect = -1;
		global::Weapons.Weapons.recoilUp = 0f;
		global::Weapons.Weapons.recoilSide = 0f;
		respawnTimer = global::MainGame.MainGame.respawnTime;
		global::MainGame.MainGame.curTimeBeforeExitingMapOnDeath = -1f;
		for (ushort num = 0; num < global::MainGame.MainGame.maxGamePlayers; num++)
		{
			maxKillStreak[num] = 0;
			Matrix.CreateRotationZ(players[num].zRotation * ((float)Math.PI / 180f), out players[num].mv[0]);
			ref Matrix reference = ref players[num].mv[1];
			reference = players[num].mv[0];
			players[num].posX[0] = players[num].charP.position.v[0];
			players[num].posX[1] = players[num].charP.position.v[0];
			players[num].posY[0] = players[num].charP.position.v[1];
			players[num].posY[1] = players[num].charP.position.v[1];
			players[num].posZ[0] = players[num].charP.position.v[2];
			players[num].posZ[1] = players[num].charP.position.v[2];
		}
		if (!minorRestart)
		{
			mainC.soundsMain.Play_Level_Music();
		}
		mainC.gameLogic.Game_New_SP_Round(threadID);
		GC.Collect();
		global::MainGame.MainGame.spGameReady = true;
	}

	public void New_MultiPlayer_Round(byte threadID)
	{
		_ = global::Rendering.Rendering.uBufferID;
		_ = global::Rendering.Rendering.rBufferID;
		moving = 0;
		respawnTimer = global::MainGame.MainGame.respawnTime;
		global::Weapons.Weapons.recoilUp = 0f;
		global::Weapons.Weapons.recoilSide = 0f;
		global::MainGame.MainGame.commanderSelect = -1;
		global::MainGame.MainGame.curTimeBeforeExitingMapOnDeath = -1f;
		global::MainGame.MainGame.raceStartTimer = 3f;
		global::MainGame.MainGame.currentRaceStartTimer = 3;
		global::MainGame.MainGame.raceParticipantsLocked = false;
		global::MainGame.MainGame.raceCanExitForNewPlayers = false;
		global::MainGame.MainGame.cameraMovementSpeed = global::MainGame.MainGame.cameraMovementSpeedDefault;
		mainC.programsMain.Reset_Round(minorRestart: false);
		mainC.weaponsMain.Reset_Round();
		mainC.mapsMain.Reset_Round();
		mainC.maingameMain.Reset_Round();
		mainC.gameobjectMain.Reset_Round(minorReset: false, threadID);
		mainC.renderingMain.Reset_Round();
		mainC.fontmoduleMain.Reset_Round();
		mainC.pickupsMain.Reset_Round();
		mainC.targetMain.Reset_Round();
		mainC.switchesMain.Reset_Round(minorRestart: false);
		mainC.aiMain.Reset_Round(threadID);
		mainC.soundsMain.Reset_Round(stopNarrator: true);
		mainC.vehicles.Reset_Round();
		mainC.maingameMain.Reset_Game_Achievement_Rewards();
		mainC.avatarMain.Reset_Round();
		for (ushort num = 0; num < global::MainGame.MainGame.maxHumanGamePlayers; num++)
		{
			maxKillStreak[num] = 0;
			Reset_Player(num, players[num].active, players[num].race, (byte)players[num].type);
		}
		if (global::Networking.Networking.networkPlayers[0].haveRemotePlayerTeam)
		{
			if (global::MainGame.MainGame.useFixedSpawnPoint)
			{
				mainC.mapsMain.Get_Spawn_Point(ref players[0].charP.position, players[0].team, ref zRotation, (sbyte)global::Networking.Networking.networkPlayers[0].playerArrayPosition, checkForEnemy: true, playerRaces[players[0].race].spawnHeight[players[0].type], 0f);
				players[0].charP.position.v[0] += remotePlayerPositionOffsetX[global::Networking.Networking.networkPlayers[0].playerArrayPosition];
				players[0].charP.position.v[1] += remotePlayerPositionOffsetY[global::Networking.Networking.networkPlayers[0].playerArrayPosition];
				needSpawn = false;
			}
			if (zRotation < 0f)
			{
				zRotation += 360f;
			}
			if (zRotation > 360f)
			{
				zRotation -= 360f;
			}
			players[0].zRotation = zRotation;
			players[0].posX[0] = players[0].charP.position.v[0];
			players[0].posY[0] = players[0].charP.position.v[1];
			players[0].posZ[0] = players[0].charP.position.v[2];
			players[0].posX[1] = players[0].charP.position.v[0];
			players[0].posY[1] = players[0].charP.position.v[1];
			players[0].posZ[1] = players[0].charP.position.v[2];
		}
		global::MainGame.MainGame.flaps = 0f;
		xRotation = 0f;
		players[0].active = true;
		numRankedPlayers[0] = 0;
		numRankedPlayers[1] = 0;
		playerRankingsTimer = 0f;
		mainC.gameLogic.Game_New_MP_Round(threadID);
		GC.Collect();
	}

	public void Move_AI_Humanoid_Player(short playerID, short aiID, float frameTime, byte threadID)
	{
		byte b = 0;
		float num = 0f;
		float num2 = 0f;
		float num3 = 0f;
		float num4 = 0f;
		byte uBufferID = global::Rendering.Rendering.uBufferID;
		int num5 = (int)players[playerID].charMain.numUsed * 3;
		int num6 = 0;
		int num7 = 0;
		while (num6 < num5)
		{
			global::Collision.Collision.floatAr[threadID, num6++] = players[playerID].charMain.v1[num7].v[0];
			global::Collision.Collision.floatAr[threadID, num6++] = players[playerID].charMain.v1[num7].v[1];
			global::Collision.Collision.floatAr[threadID, num6++] = players[playerID].charMain.v1[num7].v[2];
			num7++;
		}
		if (!global::AI.AI.ais[aiID].locallyControlled && (players[playerID].onmap & 7) != 0)
		{
			players[playerID].charP.mass = global::MainGame.MainGame.playerVehicles[players[playerID].curVehicle].ph1.mass;
			if (global::AI.AI.mpData[aiID].dataThisRound)
			{
				ref Matrix reference = ref players[playerID].mv[uBufferID];
				reference = Matrix.CreateRotationZ(players[playerID].zRotation * ((float)Math.PI / 180f));
			}
			else
			{
				global::AI.AI.mpData[aiID].currentPosX += global::AI.AI.mpData[aiID].velX * frameTime / global::Physics.Physics.timeMod;
				global::AI.AI.mpData[aiID].currentPosY += global::AI.AI.mpData[aiID].velY * frameTime / global::Physics.Physics.timeMod;
				global::AI.AI.mpData[aiID].currentPosZ += global::AI.AI.mpData[aiID].velZ * frameTime / global::Physics.Physics.timeMod;
				ref Matrix reference2 = ref players[playerID].mv[uBufferID];
				reference2 = players[playerID].mv[global::Rendering.Rendering.rBufferID];
			}
			global::AI.AI.mpData[aiID].springX = global::AI.AI.mpData[aiID].currentPosX - players[playerID].charP.position.v[0];
			global::AI.AI.mpData[aiID].springY = global::AI.AI.mpData[aiID].currentPosY - players[playerID].charP.position.v[1];
			global::AI.AI.mpData[aiID].springZ = global::AI.AI.mpData[aiID].currentPosZ - players[playerID].charP.position.v[2];
			players[playerID].charP.fx = players[playerID].charP.mass * (global::AI.AI.mpData[aiID].springX * 150f - players[playerID].charP.velocity.v[0] * 20f);
			players[playerID].charP.fy = players[playerID].charP.mass * (global::AI.AI.mpData[aiID].springY * 150f - players[playerID].charP.velocity.v[1] * 20f);
			players[playerID].charP.fz = players[playerID].charP.mass * (global::AI.AI.mpData[aiID].springZ * 50f - players[playerID].charP.velocity.v[2] * 10f);
			mainC.physicsMain.getPosition(ref players[playerID].charP, frameTime * global::Physics.Physics.timeMod);
			ref Matrix reference3 = ref global::MainGame.MainGame.playerVehicles[playerID].mv[uBufferID];
			reference3 = players[playerID].mv[uBufferID];
		}
		else
		{
			if (global::AI.AI.ais[aiID].aiRoute.routeError)
			{
				mainC.mapsMain.Get_AI_Spawn_Point(ref players[playerID].charP.position, players[playerID].team, ref players[playerID].zRotation, global::MainGame.MainGame.maxLocalPlayerSpawnPoint, checkForEnemy: false, playerRaces[players[playerID].race].spawnHeight[players[playerID].type]);
				global::AI.AI.ais[aiID].aiRoute.routeError = false;
			}
			byte cType = 2;
			p1AIHumanoid.pos1.v[0] = players[playerID].charP.position.v[0];
			p1AIHumanoid.pos1.v[1] = players[playerID].charP.position.v[1];
			p1AIHumanoid.pos1.v[2] = players[playerID].charP.position.v[2];
			if (players[playerID].onmap == 4)
			{
				p1AIHumanoid.pos1.v[2] += 2f;
				if (global::AI.AI.ais[aiID].resetSpeed)
				{
					players[playerID].charP.velocity.v[0] = 0f;
					players[playerID].charP.velocity.v[1] = 0f;
					players[playerID].velX = 0f;
					players[playerID].velY = 0f;
					global::AI.AI.ais[aiID].resetSpeed = false;
				}
				players[playerID].charP.velocity.v[0] = (players[playerID].charP.velocity.v[0] * global::MainGame.MainGame.playerVehicles[playerID].ph1.mass + players[playerID].impactX) / global::MainGame.MainGame.playerVehicles[playerID].ph1.mass;
				players[playerID].charP.velocity.v[1] = (players[playerID].charP.velocity.v[1] * global::MainGame.MainGame.playerVehicles[playerID].ph1.mass + players[playerID].impactY) / global::MainGame.MainGame.playerVehicles[playerID].ph1.mass;
				players[playerID].charP.velocity.v[2] = (players[playerID].charP.velocity.v[2] * global::MainGame.MainGame.playerVehicles[playerID].ph1.mass + players[playerID].impactZ) / global::MainGame.MainGame.playerVehicles[playerID].ph1.mass;
				float num8 = 0.5f * global::MainGame.MainGame.playerVehicles[playerID].ph1.mass;
				players[playerID].impactX = 0f;
				players[playerID].impactY = 0f;
				players[playerID].impactZ = 0f;
				if (global::Collision.Collision.floatArMovDir[threadID, 0] == 0f)
				{
					players[playerID].impactX = 0f;
					players[playerID].impactY = 0f;
					players[playerID].impactZ = 0f;
				}
				num2 = players[playerID].charP.velocity.v[0];
				num3 = players[playerID].charP.velocity.v[1];
				num4 = players[playerID].charP.velocity.v[2];
				num8 = (float)Math.Sqrt(num2 * num2 + num3 * num3 + num4 * num4);
				global::MainGame.MainGame.playerVehicles[playerID].ph1.velocity = num8;
				if (num8 != 0f)
				{
					num2 /= num8;
					num3 /= num8;
					num4 /= num8;
					num = num2 * players[playerID].mv[uBufferID].M21 + num3 * players[playerID].mv[uBufferID].M22 + num4 * players[playerID].mv[uBufferID].M23;
					if (Math.Abs(num) > 0.707f)
					{
						b = 1;
					}
					else
					{
						num = num2 * players[playerID].mv[uBufferID].M11 + num3 * players[playerID].mv[uBufferID].M12 + num4 * players[playerID].mv[uBufferID].M13;
						b = 2;
					}
				}
				else
				{
					b = 0;
				}
				if (players[playerID].playerIsMoving != 32 && players[playerID].playerIsMoving != 256)
				{
					switch (b)
					{
					case 1:
						if (num8 < 35f)
						{
							if (num >= 0f && players[playerID].playerIsMoving != 2)
							{
								players[playerID].playerIsMoving = 2;
								mainC.avatarMain.Avatar_Movement_By_List_ID((byte)playerID, 1, loop: true, 1, cancelOtherGroupAnimations: true);
							}
							else if (num < 0f && players[playerID].playerIsMoving != 16)
							{
								players[playerID].playerIsMoving = 16;
								mainC.avatarMain.Avatar_Movement_By_List_ID((byte)playerID, 1, loop: true, 1, cancelOtherGroupAnimations: true);
							}
						}
						else if (players[playerID].playerIsMoving != 8)
						{
							players[playerID].playerIsMoving = 8;
							mainC.avatarMain.Avatar_Movement_By_List_ID((byte)playerID, 1, loop: true, 2, cancelOtherGroupAnimations: true);
						}
						break;
					case 2:
						if (num8 >= 0f && players[playerID].playerIsMoving != 4)
						{
							players[playerID].playerIsMoving = 4;
							mainC.avatarMain.Avatar_Movement_By_List_ID((byte)playerID, 1, loop: true, 4, cancelOtherGroupAnimations: true);
						}
						else if (num8 < 0f && players[playerID].playerIsMoving != 512)
						{
							players[playerID].playerIsMoving = 512;
							mainC.avatarMain.Avatar_Movement_By_List_ID((byte)playerID, 1, loop: true, 3, cancelOtherGroupAnimations: true);
						}
						break;
					default:
						if (players[playerID].playerIsMoving != 1)
						{
							players[playerID].playerIsMoving = 1;
							mainC.avatarMain.Avatar_Movement_By_List_ID((byte)playerID, 1, loop: true, 0, cancelOtherGroupAnimations: true);
						}
						break;
					}
				}
			}
			else if (players[playerID].onmap == 8)
			{
				cType = 0;
				if (players[playerID].impactX + players[playerID].impactY + players[playerID].impactZ != 0f)
				{
					players[playerID].charP.velocity.v[0] = (players[playerID].charP.velocity.v[0] * global::MainGame.MainGame.playerVehicles[playerID].ph1.mass + players[playerID].impactX) / global::MainGame.MainGame.playerVehicles[playerID].ph1.mass;
					players[playerID].charP.velocity.v[1] = (players[playerID].charP.velocity.v[1] * global::MainGame.MainGame.playerVehicles[playerID].ph1.mass + players[playerID].impactY) / global::MainGame.MainGame.playerVehicles[playerID].ph1.mass;
					players[playerID].charP.velocity.v[2] = (players[playerID].charP.velocity.v[2] * global::MainGame.MainGame.playerVehicles[playerID].ph1.mass + players[playerID].impactZ) / global::MainGame.MainGame.playerVehicles[playerID].ph1.mass;
				}
			}
			if (global::AI.AI.ais[aiID].speedHover != 0f)
			{
				players[playerID].charP.velocity.v[2] -= players[playerID].charP.acceleration.v[2] * (frameTime / global::Physics.Physics.timeMod);
			}
			players[playerID].charP.fz = players[playerID].charP.mass * -32.15223f;
			mainC.physicsMain.getPosition(ref players[playerID].charP, frameTime);
			players[playerID].charP.acceleration.v[2] = -32.15223f;
			players[playerID].charP.fx = 0f;
			players[playerID].charP.fy = 0f;
			players[playerID].charP.fz = 0f;
			p1AIHumanoid.pos2.v[0] = players[playerID].charP.position.v[0];
			p1AIHumanoid.pos2.v[1] = players[playerID].charP.position.v[1];
			p1AIHumanoid.pos2.v[2] = players[playerID].charP.position.v[2];
			mainC.collisionMain.ResetIgnoreList(threadID, 1);
			mainC.collisionMain.CheckCollision_Detailed_List(ref p1AIHumanoid, (int)players[playerID].charMain.numUsed, 0, 143, cType, threadID);
			if (global::Collision.Collision.floatArMovDir[threadID, 0] == 0f)
			{
				players[playerID].impactX = 0f;
				players[playerID].impactY = 0f;
				players[playerID].impactZ = 0f;
			}
			else
			{
				players[playerID].charP.velocity.v[0] = players[playerID].velX;
				players[playerID].charP.velocity.v[1] = players[playerID].velY;
				if (players[playerID].charP.velocity.v[2] < 0f)
				{
					players[playerID].charP.velocity.v[2] = 0f;
				}
			}
			if (players[playerID].onmap == 8)
			{
				players[playerID].charP.position.v[0] = p1AIHumanoid.pos2.v[0];
				players[playerID].charP.position.v[1] = p1AIHumanoid.pos2.v[1];
				for (num6 = 0; num6 < players[playerID].charMain.numUsed; num6++)
				{
					if (global::Collision.Collision.floatArStatus[threadID, num6] == 1 && global::Collision.Collision.floatArDir[threadID, num6, 2] > 0.2f)
					{
						players[playerID].charP.position.v[2] = p1AIHumanoid.pos2.v[2];
						break;
					}
				}
			}
			else
			{
				players[playerID].charP.position.v[2] = p1AIHumanoid.pos2.v[2];
			}
			players[playerID].charP.mass = global::MainGame.MainGame.playerVehicles[players[playerID].curVehicle].ph1.mass;
		}
		global::MainGame.MainGame.playerVehicles[playerID].ph1.x = players[playerID].charP.position.v[0];
		global::MainGame.MainGame.playerVehicles[playerID].ph1.y = players[playerID].charP.position.v[1];
		global::MainGame.MainGame.playerVehicles[playerID].ph1.z = players[playerID].charP.position.v[2];
		Update_Player_BoundingBox(playerID, players[playerID].charP.position.v[0], players[playerID].charP.position.v[1], players[playerID].charP.position.v[2], threadID);
		Confine_Player_Position_ToBoundaries(playerID, postCollision: false, threadID);
		mainC.weaponsMain.Process_Player_Weapons(playerID, players[playerID].primaryWeaponMountWeapon);
	}

	public void Move_AI_Player_Airplane(byte playerID, short aiID, float rotX, float rotY, float frameTime, byte threadID)
	{
		byte rBufferID = global::Rendering.Rendering.rBufferID;
		byte b = 3;
		byte b2 = 8;
		byte b3 = 6;
		float num = 0f;
		float num2 = 0f;
		float num3 = 1.5f;
		float num4 = 0f;
		float num5 = 0f;
		float num6 = 0f;
		float num7 = 0f;
		Matrix result = Matrix.Identity;
		byte uBufferID = global::Rendering.Rendering.uBufferID;
		rBufferID = global::Rendering.Rendering.rBufferID;
		global::MainGame.MainGame.playerVehicles[playerID].throttleSpeed = 1f;
		ushort curVehicle = players[playerID].curVehicle;
		Vehicles.vehicles[curVehicle].balanceFactor = 0.15f;
		Vehicles.vehicles[curVehicle].wheelTouchingFactor = 0.15f;
		players[playerID].charMain.pos1.v[0] = global::MainGame.MainGame.playerVehicles[playerID].ph1.x;
		players[playerID].charMain.pos1.v[1] = global::MainGame.MainGame.playerVehicles[playerID].ph1.y;
		players[playerID].charMain.pos1.v[2] = global::MainGame.MainGame.playerVehicles[playerID].ph1.z;
		if (players[playerID].zRotation < 0f)
		{
			players[playerID].zRotation += 360f;
		}
		else if (players[playerID].zRotation > 360f)
		{
			players[playerID].zRotation -= 360f;
		}
		if (global::MainGame.MainGame.arcadeModeRotAngle[playerID] > (float)Math.PI * 2f)
		{
			global::MainGame.MainGame.arcadeModeRotAngle[playerID] -= (float)Math.PI * 2f;
		}
		else if (global::MainGame.MainGame.arcadeModeRotAngle[playerID] < 0f)
		{
			global::MainGame.MainGame.arcadeModeRotAngle[playerID] += (float)Math.PI * 2f;
		}
		Matrix matrix = players[playerID].mv[rBufferID];
		float num8 = frameTime / global::Physics.Physics.timeMod;
		num3 = global::AI.AI.ais[aiID].speedRotationZ;
		_ = Vehicles.vehicles[curVehicle].ph1.mass;
		float data = Vehicles.vehicles[curVehicle].data6;
		float data2 = Vehicles.vehicles[curVehicle].data7;
		if (players[playerID].needToReload)
		{
			if (mainC.weaponsMain.Player_Has_Ammo_For_Weapon(playerID) > 1)
			{
				byte b4 = (byte)players[playerID].wpnIndex;
				if (global::Weapons.Weapons.ammo[global::Weapons.Weapons.ammoClips[players[playerID].weapon2[b4].curClip].ammoIndex].single)
				{
					players[playerID].weapon2[b4].fired = false;
					mainC.weaponsMain.Load_Ammo_Clip_Into_Player_Weapon(b4, (byte)players[playerID].primaryWeaponMountWeapon, playerID, players[playerID].ammoClips[players[playerID].weapon2[b4].curClip].numClips);
				}
				else
				{
					mainC.weaponsMain.Weapon_Reloaded(b4, playerID);
				}
			}
			players[playerID].needToReload = false;
			players[playerID].needToChamber = false;
		}
		float m = matrix.M21;
		float m2 = matrix.M22;
		float m3 = matrix.M23;
		num2 = (float)Math.Sqrt(global::MainGame.MainGame.playerVehicles[playerID].ph1.velocityX * global::MainGame.MainGame.playerVehicles[playerID].ph1.velocityX + global::MainGame.MainGame.playerVehicles[playerID].ph1.velocityY * global::MainGame.MainGame.playerVehicles[playerID].ph1.velocityY + global::MainGame.MainGame.playerVehicles[playerID].ph1.velocityZ * global::MainGame.MainGame.playerVehicles[playerID].ph1.velocityZ);
		if (num2 < 1E-11f || num2 > 1E+10f)
		{
			global::MainGame.MainGame.playerVehicles[playerID].ph1.velocityX = 0f;
			global::MainGame.MainGame.playerVehicles[playerID].ph1.velocityY = 0f;
			global::MainGame.MainGame.playerVehicles[playerID].ph1.velocityZ = 0f;
			num2 = 0f;
			num5 = 0f;
			num6 = 0f;
			num7 = 0f;
		}
		if (num2 > 0f)
		{
			num5 = global::MainGame.MainGame.playerVehicles[playerID].ph1.velocityX / num2;
			num6 = global::MainGame.MainGame.playerVehicles[playerID].ph1.velocityY / num2;
			num7 = global::MainGame.MainGame.playerVehicles[playerID].ph1.velocityZ / num2;
		}
		float num9 = num5 * m + num6 * m2 + num7 * m3;
		_ = global::MainGame.MainGame.playerVehicles[playerID].throttleSpeed * data * (data2 - num2 * num9) / data2;
		num2 += (global::MainGame.MainGame.playerVehicles[playerID].throttleSpeed * global::AI.AI.ais[aiID].speed - num2) * 0.1505f * num8;
		global::MainGame.MainGame.playerVehicles[playerID].ph1.velocityX = m * num2;
		global::MainGame.MainGame.playerVehicles[playerID].ph1.velocityY = m2 * num2;
		global::MainGame.MainGame.playerVehicles[playerID].ph1.velocityZ = m3 * num2;
		global::MainGame.MainGame.playerVehicles[playerID].ph1.velocity = num2;
		num4 = num2;
		float num10 = rotX;
		if (Math.Abs(rotX) > 90f)
		{
			num10 = (float)Math.Sign(rotX) * 90f;
		}
		num10 /= 90f;
		float num11 = rotY;
		if (Math.Abs(rotY) > 90f)
		{
			num11 = (float)Math.Sign(rotY) * 90f;
		}
		num11 /= 90f;
		num = -1f;
		float num13;
		float num12;
		if (players[playerID].onmap != 8)
		{
			num12 = mainC.terrainMain.Get_Terrain_Height(global::MainGame.MainGame.playerVehicles[playerID].ph1.x, global::MainGame.MainGame.playerVehicles[playerID].ph1.y, threadID);
			if (global::MainGame.MainGame.playerVehicles[playerID].ph1.z - num12 < 20f && num2 / data2 < 0.5f)
			{
				num10 = 0f;
				num11 = 0f;
			}
			if (global::MainGame.MainGame.playerVehicles[playerID].ph1.z < num12 + 50f || (!global::AI.AI.ais[aiID].targetVisible && global::MainGame.MainGame.playerVehicles[playerID].ph1.z < num12 + 500f))
			{
				num = 10f;
				num13 = mainC.terrainMain.Get_Terrain_Height(global::MainGame.MainGame.playerVehicles[playerID].ph1.x + 10f * players[playerID].mv[uBufferID].M21, global::MainGame.MainGame.playerVehicles[playerID].ph1.y + 10f * players[playerID].mv[uBufferID].M22, threadID);
				float num14 = mainC.terrainMain.Get_Terrain_Height(global::MainGame.MainGame.playerVehicles[playerID].ph1.x + 30f * players[playerID].mv[uBufferID].M21, global::MainGame.MainGame.playerVehicles[playerID].ph1.y + 30f * players[playerID].mv[uBufferID].M22, threadID);
				if ((num14 - num12) / 30f > (num13 - num12) / 10f)
				{
					num13 = num14;
					num = 30f;
				}
				num14 = mainC.terrainMain.Get_Terrain_Height(global::MainGame.MainGame.playerVehicles[playerID].ph1.x + 60f * players[playerID].mv[uBufferID].M21, global::MainGame.MainGame.playerVehicles[playerID].ph1.y + 60f * players[playerID].mv[uBufferID].M22, threadID);
				if ((num14 - num12) / 60f > (num13 - num12) / num)
				{
					num13 = num14;
					num = 60f;
				}
				num12 = num13 - num12;
				num12 /= num;
				if (num12 <= 1f)
				{
					num = -1f;
					num13 = (float)Math.Asin(num12) * 57.29578f + 10f;
					if (global::MainGame.MainGame.arcadeModeRisingAngle[playerID] <= num13)
					{
						num11 = (5f + (num13 - global::MainGame.MainGame.arcadeModeRisingAngle[playerID] * 57.29578f)) / 95f;
					}
				}
			}
		}
		num12 = Math.Abs(global::MainGame.MainGame.angularVelocity[playerID] / ((float)Math.PI / 2f));
		num13 = 0f;
		if (global::MainGame.MainGame.angularVelocity[playerID] != 0f)
		{
			num13 = rotX / (global::MainGame.MainGame.angularVelocity[playerID] * 1.5f * 57.29578f);
		}
		if (num12 > num13 && num13 > 0f)
		{
			num13 = (float)Math.PI / 2f * num8 * (float)Math.Sign(global::MainGame.MainGame.angularVelocity[playerID]);
			if (num13 < Math.Abs(global::MainGame.MainGame.angularVelocity[playerID]))
			{
				global::MainGame.MainGame.angularVelocity[playerID] -= num13 * 1.25f;
			}
			else
			{
				global::MainGame.MainGame.angularVelocity[playerID] = 0f;
			}
		}
		else
		{
			global::MainGame.MainGame.angularVelocity[playerID] += num10 * ((float)Math.PI / 2f) * num8;
			if (Math.Abs(global::MainGame.MainGame.angularVelocity[playerID]) > (float)Math.PI / 2f)
			{
				global::MainGame.MainGame.angularVelocity[playerID] = (float)Math.PI / 2f * (float)Math.Sign(global::MainGame.MainGame.angularVelocity[playerID]);
			}
		}
		if (num != -1f)
		{
			num12 = Math.Abs(global::MainGame.MainGame.arcadeModeRisingAngle[playerID] / ((float)Math.PI / 2f));
			num13 = Math.Abs(rotY / (global::MainGame.MainGame.arcadeModeRisingAngle[playerID] * 57.29578f));
			if (num12 > num13)
			{
				num11 = ((!(num12 >= 1f)) ? ((0f - num12) * (float)Math.Sign(num11)) : (-1f * (float)Math.Sign(num11)));
			}
		}
		global::MainGame.MainGame.arcadeModeRisingAngle[playerID] += num11 * ((float)Math.PI / 2f) * num8;
		if (Math.Abs(global::MainGame.MainGame.arcadeModeRisingAngle[playerID]) > (float)Math.PI / 2f)
		{
			global::MainGame.MainGame.arcadeModeRisingAngle[playerID] = (float)Math.PI / 2f * (float)Math.Sign(global::MainGame.MainGame.arcadeModeRisingAngle[playerID]);
		}
		num12 = global::MainGame.MainGame.angularVelocity[playerID] * num3 * num8;
		global::MainGame.MainGame.arcadeModeRotAngle[playerID] += num12;
		rotX -= num12;
		Quaternion quaternion = Quaternion.CreateFromYawPitchRoll(0f - global::MainGame.MainGame.angularVelocity[playerID], global::MainGame.MainGame.arcadeModeRisingAngle[playerID], 0f);
		Matrix.CreateFromQuaternion(ref quaternion, out result);
		result *= Matrix.CreateRotationZ(global::MainGame.MainGame.arcadeModeRotAngle[playerID]);
		global::MainGame.MainGame.playerVehicles[playerID].mv[uBufferID] = result;
		matrix = result;
		global::MainGame.MainGame.turnPlaneAround = 0f;
		global::MainGame.MainGame.playerVehicles[playerID].ph1.forceX = 0f;
		global::MainGame.MainGame.playerVehicles[playerID].ph1.forceY = 0f;
		global::MainGame.MainGame.playerVehicles[playerID].ph1.torqueX = 0f;
		global::MainGame.MainGame.playerVehicles[playerID].ph1.torqueY = 0f;
		global::MainGame.MainGame.playerVehicles[playerID].ph1.torqueZ = 0f;
		global::MainGame.MainGame.playerVehicles[playerID].ph1.angularVelocityX = 0f;
		global::MainGame.MainGame.playerVehicles[playerID].ph1.angularVelocityY = 0f;
		global::MainGame.MainGame.playerVehicles[playerID].ph1.angularVelocityZ = 0f;
		mainC.physicsMain.getPosition_new(ref global::MainGame.MainGame.playerVehicles[playerID].ph1, num8 * global::Physics.Physics.timeMod);
		global::MainGame.MainGame.playerVehicles[playerID].ph1.forceZ = 0f;
		players[playerID].charMain.pos2.v[0] = global::MainGame.MainGame.playerVehicles[playerID].ph1.x;
		players[playerID].charMain.pos2.v[1] = global::MainGame.MainGame.playerVehicles[playerID].ph1.y;
		players[playerID].charMain.pos2.v[2] = global::MainGame.MainGame.playerVehicles[playerID].ph1.z;
		num10 = num4 / data2 * 0.4f;
		num10 = global::MainGame.MainGame.playerVehicles[playerID].throttleSpeed * 0.4f + num10;
		mainC.soundsMain.Play_Moving_Continual_Sound(playerID, 2, stop: false, 0.5f + global::MainGame.MainGame.playerVehicles[playerID].throttleSpeed * 0.5f, num10 * 100f, global::MainGame.MainGame.playerVehicles[playerID].ph1.x, global::MainGame.MainGame.playerVehicles[playerID].ph1.y, global::MainGame.MainGame.playerVehicles[playerID].ph1.z, global::MainGame.MainGame.playerVehicles[playerID].ph1.velocityX, global::MainGame.MainGame.playerVehicles[playerID].ph1.velocityY, global::MainGame.MainGame.playerVehicles[playerID].ph1.velocityZ);
		if (players[playerID].damagePercentageCapped > 0.1f)
		{
			Damage_Particles_For_Damaged_Player_Vehicle(playerID, threadID);
		}
		int num15 = playerRaces[players[playerID].race].programTurnRight[players[playerID].type];
		GameLogic.propRot[playerID] = 0f;
		if (num15 != 255)
		{
			num10 = 465f + 14900f * global::MainGame.MainGame.playerVehicles[playerID].throttleSpeed;
			GameLogic.propRot[playerID] = num10 * frameTime;
			players[playerID].jt1[num15].pivot2Speed = num10;
			players[playerID].jt1[num15].targetPivot2 += num10;
			while (players[playerID].jt1[num15].targetPivot2 > 360f)
			{
				players[playerID].jt1[num15].targetPivot2 -= 360f;
			}
		}
		global::MainGame.MainGame.playerVehicles[playerID].ph1.x = players[playerID].charMain.pos2.v[0];
		global::MainGame.MainGame.playerVehicles[playerID].ph1.y = players[playerID].charMain.pos2.v[1];
		global::MainGame.MainGame.playerVehicles[playerID].ph1.z = players[playerID].charMain.pos2.v[2];
		players[playerID].charP.position.v[0] = players[playerID].charMain.pos2.v[0];
		players[playerID].charP.position.v[1] = players[playerID].charMain.pos2.v[1];
		players[playerID].charP.position.v[2] = players[playerID].charMain.pos2.v[2];
		players[playerID].charP.velocity.v[0] = global::MainGame.MainGame.playerVehicles[playerID].ph1.velocityX;
		players[playerID].charP.velocity.v[1] = global::MainGame.MainGame.playerVehicles[playerID].ph1.velocityY;
		players[playerID].charP.velocity.v[2] = global::MainGame.MainGame.playerVehicles[playerID].ph1.velocityZ;
		players[playerID].mv[uBufferID] = result;
		players[playerID].zRotation = (float)Math.Acos((double)result.M22 / Math.Sqrt(result.M21 * result.M21 + result.M22 * result.M22)) * 57.29578f;
		if (result.M21 > 0f)
		{
			players[playerID].zRotation = 360f - players[playerID].zRotation;
		}
		global::MainGame.MainGame.playerVehicles[playerID].velocity = num2;
		Update_Player_Vehicle_BoundingBox(playerID, threadID, (ushort)(Vehicles.vehicles[curVehicle].numWheels + Vehicles.vehicles[curVehicle].numColPoints), ref players[playerID].mv[uBufferID]);
		num10 = global::MainGame.MainGame.playerVehicles[playerID].ph1.z;
		Confine_Player_Position_ToBoundaries(playerID, postCollision: false, threadID);
		global::MainGame.MainGame.playerVehicles[playerID].ph1.x = players[playerID].charP.position.v[0];
		global::MainGame.MainGame.playerVehicles[playerID].ph1.y = players[playerID].charP.position.v[1];
		global::MainGame.MainGame.playerVehicles[playerID].ph1.z = players[playerID].charP.position.v[2];
		if (global::MainGame.MainGame.playerVehicles[playerID].ph1.z != num10)
		{
			global::MainGame.MainGame.playerVehicles[playerID].vehicleTimer1 = 0f;
			if (players[playerID].onmap == 8 || players[playerID].damage >= global::MainGame.MainGame.playerVehicles[playerID].damageThresholdForExplosion)
			{
				Player_Vehicle_Explodes(playerID, threadID);
			}
		}
		else if (players[playerID].damage >= global::MainGame.MainGame.playerVehicles[playerID].damageThresholdForExplosion)
		{
			Player_Vehicle_Explodes(playerID, threadID);
		}
		num15 = playerRaces[players[playerID].race].programTurnLeft[players[playerID].type];
		if (num15 < 255 && global::MainGame.MainGame.gearDown[playerID] == 1)
		{
			if (players[playerID].animations[num15].status != 2)
			{
				global::MainGame.MainGame.playerVehicles[playerID].vehicleTimer1 += frameTime / global::Physics.Physics.timeMod;
				if (global::MainGame.MainGame.playerVehicles[playerID].vehicleTimer1 > 1000f)
				{
					global::MainGame.MainGame.playerVehicles[playerID].vehicleTimer1 = 1000f;
				}
				if (global::MainGame.MainGame.playerVehicles[playerID].vehicleTimer1 > 2f && (float)global::MainGame.MainGame.mainRandom.Next(0, 32000) / 32000f < global::MainGame.MainGame.playerVehicles[playerID].vehicleTimer1 / 800f)
				{
					global::MainGame.MainGame.gearDown[playerID] = 0;
					players[playerID].pg1[num15].inReverse = false;
					if (players[playerID].animations[num15].status != 2)
					{
						gearSound = mainC.soundsMain.Play_Priority_Sound("Hydraulics", global::MainGame.MainGame.playerVehicles[playerID].ph1.x, global::MainGame.MainGame.playerVehicles[playerID].ph1.y, global::MainGame.MainGame.playerVehicles[playerID].ph1.z, 0f, 0f, 0f);
					}
					mainC.programsMain.Set_Animation_To_Reverse_Direction(ref players[playerID].animations, num15);
				}
			}
			else if (players[playerID].animations[num15].status == 2)
			{
				b = players[playerID].headJoint;
				b3 = players[playerID].shoulderJointL;
				b2 = players[playerID].shoulderJointR;
				mainC.vehicles.Update_Wheel_Positions(ref global::MainGame.MainGame.playerVehicles[playerID], curVehicle, 0, players[playerID].jt1[b].rotX, players[playerID].jt1[b].rotY, players[playerID].jt1[b].rotZ);
				mainC.vehicles.Update_Wheel_Positions(ref global::MainGame.MainGame.playerVehicles[playerID], curVehicle, 1, players[playerID].jt1[b2].rotX, players[playerID].jt1[b2].rotY, players[playerID].jt1[b2].rotZ);
				mainC.vehicles.Update_Wheel_Positions(ref global::MainGame.MainGame.playerVehicles[playerID], curVehicle, 2, players[playerID].jt1[b3].rotX, players[playerID].jt1[b3].rotY, players[playerID].jt1[b3].rotZ);
			}
		}
		mainC.weaponsMain.Process_Player_Weapons(playerID, players[playerID].primaryWeaponMountWeapon);
	}

	public void Move_AI_Player_SpaceShip(byte playerID, short aiID, float rotX, float rotY, float frameTime, byte threadID)
	{
		byte rBufferID = global::Rendering.Rendering.rBufferID;
		float num = 0f;
		float num2 = 0f;
		float num3 = 1.5f;
		float num4 = 0f;
		float num5 = 0f;
		float num6 = 0f;
		float num7 = 0f;
		Matrix result = Matrix.Identity;
		byte uBufferID = global::Rendering.Rendering.uBufferID;
		rBufferID = global::Rendering.Rendering.rBufferID;
		global::MainGame.MainGame.playerVehicles[playerID].throttleSpeed = 1f;
		ushort curVehicle = players[playerID].curVehicle;
		Vehicles.vehicles[curVehicle].balanceFactor = 0.15f;
		Vehicles.vehicles[curVehicle].wheelTouchingFactor = 0.15f;
		players[playerID].charMain.pos1.v[0] = global::MainGame.MainGame.playerVehicles[playerID].ph1.x;
		players[playerID].charMain.pos1.v[1] = global::MainGame.MainGame.playerVehicles[playerID].ph1.y;
		players[playerID].charMain.pos1.v[2] = global::MainGame.MainGame.playerVehicles[playerID].ph1.z;
		if (players[playerID].zRotation < 0f)
		{
			players[playerID].zRotation += 360f;
		}
		else if (players[playerID].zRotation > 360f)
		{
			players[playerID].zRotation -= 360f;
		}
		if (global::MainGame.MainGame.arcadeModeRotAngle[playerID] > (float)Math.PI * 2f)
		{
			global::MainGame.MainGame.arcadeModeRotAngle[playerID] -= (float)Math.PI * 2f;
		}
		else if (global::MainGame.MainGame.arcadeModeRotAngle[playerID] < 0f)
		{
			global::MainGame.MainGame.arcadeModeRotAngle[playerID] += (float)Math.PI * 2f;
		}
		Matrix matrix = players[playerID].mv[rBufferID];
		float num8 = frameTime / global::Physics.Physics.timeMod;
		num3 = global::AI.AI.ais[aiID].speedRotationZ;
		_ = Vehicles.vehicles[curVehicle].ph1.mass;
		float data = Vehicles.vehicles[curVehicle].data6;
		float data2 = Vehicles.vehicles[curVehicle].data7;
		if (players[playerID].needToReload)
		{
			if (mainC.weaponsMain.Player_Has_Ammo_For_Weapon(playerID) > 1)
			{
				byte b = (byte)players[playerID].wpnIndex;
				if (global::Weapons.Weapons.ammo[global::Weapons.Weapons.ammoClips[players[playerID].weapon2[b].curClip].ammoIndex].single)
				{
					players[playerID].weapon2[b].fired = false;
					mainC.weaponsMain.Load_Ammo_Clip_Into_Player_Weapon(b, (byte)players[playerID].primaryWeaponMountWeapon, playerID, players[playerID].ammoClips[players[playerID].weapon2[b].curClip].numClips);
				}
				else
				{
					mainC.weaponsMain.Weapon_Reloaded(b, playerID);
				}
			}
			players[playerID].needToReload = false;
			players[playerID].needToChamber = false;
		}
		float num10;
		if (global::AI.AI.ais[aiID].aiType == 0)
		{
			float m = matrix.M21;
			float m2 = matrix.M22;
			float m3 = matrix.M23;
			num2 = (float)Math.Sqrt(global::MainGame.MainGame.playerVehicles[playerID].ph1.velocityX * global::MainGame.MainGame.playerVehicles[playerID].ph1.velocityX + global::MainGame.MainGame.playerVehicles[playerID].ph1.velocityY * global::MainGame.MainGame.playerVehicles[playerID].ph1.velocityY + global::MainGame.MainGame.playerVehicles[playerID].ph1.velocityZ * global::MainGame.MainGame.playerVehicles[playerID].ph1.velocityZ);
			if (num2 < 1E-11f || num2 > 1E+10f)
			{
				global::MainGame.MainGame.playerVehicles[playerID].ph1.velocityX = 0f;
				global::MainGame.MainGame.playerVehicles[playerID].ph1.velocityY = 0f;
				global::MainGame.MainGame.playerVehicles[playerID].ph1.velocityZ = 0f;
				num2 = 0f;
				num5 = 0f;
				num6 = 0f;
				num7 = 0f;
			}
			if (num2 > 0f)
			{
				num5 = global::MainGame.MainGame.playerVehicles[playerID].ph1.velocityX / num2;
				num6 = global::MainGame.MainGame.playerVehicles[playerID].ph1.velocityY / num2;
				num7 = global::MainGame.MainGame.playerVehicles[playerID].ph1.velocityZ / num2;
			}
			float num9 = num5 * m + num6 * m2 + num7 * m3;
			_ = global::MainGame.MainGame.playerVehicles[playerID].throttleSpeed * data * (data2 - num2 * num9) / data2;
			num2 += (global::MainGame.MainGame.playerVehicles[playerID].throttleSpeed * global::AI.AI.ais[aiID].speed - num2) * 0.1505f * num8;
			global::MainGame.MainGame.playerVehicles[playerID].ph1.velocityX = m * num2;
			global::MainGame.MainGame.playerVehicles[playerID].ph1.velocityY = m2 * num2;
			global::MainGame.MainGame.playerVehicles[playerID].ph1.velocityZ = m3 * num2;
			global::MainGame.MainGame.playerVehicles[playerID].ph1.velocity = num2;
			num4 = num2;
			num10 = rotX;
			if (Math.Abs(rotX) > 90f)
			{
				num10 = (float)Math.Sign(rotX) * 90f;
			}
			num10 /= 90f;
			float num11 = rotY;
			if (Math.Abs(rotY) > 90f)
			{
				num11 = (float)Math.Sign(rotY) * 90f;
			}
			num11 /= 90f;
			num = -1f;
			float num13;
			float num12;
			if (players[playerID].onmap != 8)
			{
				num12 = mainC.terrainMain.Get_Terrain_Height(global::MainGame.MainGame.playerVehicles[playerID].ph1.x, global::MainGame.MainGame.playerVehicles[playerID].ph1.y, threadID);
				if (global::MainGame.MainGame.playerVehicles[playerID].ph1.z - num12 < 20f && num2 / data2 < 0.5f)
				{
					num10 = 0f;
					num11 = 0f;
				}
				if (global::MainGame.MainGame.playerVehicles[playerID].ph1.z < num12 + 50f || (!global::AI.AI.ais[aiID].targetVisible && global::MainGame.MainGame.playerVehicles[playerID].ph1.z < num12 + 500f))
				{
					num = 10f;
					num13 = mainC.terrainMain.Get_Terrain_Height(global::MainGame.MainGame.playerVehicles[playerID].ph1.x + 10f * players[playerID].mv[uBufferID].M21, global::MainGame.MainGame.playerVehicles[playerID].ph1.y + 10f * players[playerID].mv[uBufferID].M22, threadID);
					float num14 = mainC.terrainMain.Get_Terrain_Height(global::MainGame.MainGame.playerVehicles[playerID].ph1.x + 30f * players[playerID].mv[uBufferID].M21, global::MainGame.MainGame.playerVehicles[playerID].ph1.y + 30f * players[playerID].mv[uBufferID].M22, threadID);
					if ((num14 - num12) / 30f > (num13 - num12) / 10f)
					{
						num13 = num14;
						num = 30f;
					}
					num14 = mainC.terrainMain.Get_Terrain_Height(global::MainGame.MainGame.playerVehicles[playerID].ph1.x + 60f * players[playerID].mv[uBufferID].M21, global::MainGame.MainGame.playerVehicles[playerID].ph1.y + 60f * players[playerID].mv[uBufferID].M22, threadID);
					if ((num14 - num12) / 60f > (num13 - num12) / num)
					{
						num13 = num14;
						num = 60f;
					}
					num12 = num13 - num12;
					num12 /= num;
					if (num12 <= 1f)
					{
						num = -1f;
						num13 = (float)Math.Asin(num12) * 57.29578f + 10f;
						if (global::MainGame.MainGame.arcadeModeRisingAngle[playerID] <= num13)
						{
							num11 = (5f + (num13 - global::MainGame.MainGame.arcadeModeRisingAngle[playerID] * 57.29578f)) / 95f;
						}
					}
				}
			}
			num12 = Math.Abs(global::MainGame.MainGame.angularVelocity[playerID] / ((float)Math.PI / 2f));
			num13 = 0f;
			if (global::MainGame.MainGame.angularVelocity[playerID] != 0f)
			{
				num13 = rotX / (global::MainGame.MainGame.angularVelocity[playerID] * 1.5f * 57.29578f);
			}
			if (num12 > num13 && num13 > 0f)
			{
				num13 = (float)Math.PI / 2f * num8 * (float)Math.Sign(global::MainGame.MainGame.angularVelocity[playerID]);
				if (num13 < Math.Abs(global::MainGame.MainGame.angularVelocity[playerID]))
				{
					global::MainGame.MainGame.angularVelocity[playerID] -= num13 * 1.25f;
				}
				else
				{
					global::MainGame.MainGame.angularVelocity[playerID] = 0f;
				}
			}
			else
			{
				global::MainGame.MainGame.angularVelocity[playerID] += num10 * ((float)Math.PI / 2f) * num8;
				if (Math.Abs(global::MainGame.MainGame.angularVelocity[playerID]) > (float)Math.PI / 2f)
				{
					global::MainGame.MainGame.angularVelocity[playerID] = (float)Math.PI / 2f * (float)Math.Sign(global::MainGame.MainGame.angularVelocity[playerID]);
				}
			}
			if (num != -1f)
			{
				num12 = Math.Abs(global::MainGame.MainGame.arcadeModeRisingAngle[playerID] / ((float)Math.PI / 2f));
				num13 = Math.Abs(rotY / (global::MainGame.MainGame.arcadeModeRisingAngle[playerID] * 57.29578f));
				if (num12 > num13)
				{
					num11 = ((!(num12 >= 1f)) ? ((0f - num12) * (float)Math.Sign(num11)) : (-1f * (float)Math.Sign(num11)));
				}
			}
			global::MainGame.MainGame.arcadeModeRisingAngle[playerID] += num11 * ((float)Math.PI / 2f) * num8;
			if (Math.Abs(global::MainGame.MainGame.arcadeModeRisingAngle[playerID]) > (float)Math.PI / 2f)
			{
				global::MainGame.MainGame.arcadeModeRisingAngle[playerID] = (float)Math.PI / 2f * (float)Math.Sign(global::MainGame.MainGame.arcadeModeRisingAngle[playerID]);
			}
			num12 = global::MainGame.MainGame.angularVelocity[playerID] * num3 * num8;
			global::MainGame.MainGame.arcadeModeRotAngle[playerID] += num12;
			rotX -= num12;
			Quaternion quaternion = Quaternion.CreateFromYawPitchRoll(0f - global::MainGame.MainGame.angularVelocity[playerID], global::MainGame.MainGame.arcadeModeRisingAngle[playerID], 0f);
			Matrix.CreateFromQuaternion(ref quaternion, out result);
			result *= Matrix.CreateRotationZ(global::MainGame.MainGame.arcadeModeRotAngle[playerID]);
			global::MainGame.MainGame.playerVehicles[playerID].mv[uBufferID] = result;
			matrix = result;
			global::MainGame.MainGame.turnPlaneAround = 0f;
			global::MainGame.MainGame.playerVehicles[playerID].ph1.forceX = 0f;
			global::MainGame.MainGame.playerVehicles[playerID].ph1.forceY = 0f;
			global::MainGame.MainGame.playerVehicles[playerID].ph1.torqueX = 0f;
			global::MainGame.MainGame.playerVehicles[playerID].ph1.torqueY = 0f;
			global::MainGame.MainGame.playerVehicles[playerID].ph1.torqueZ = 0f;
			global::MainGame.MainGame.playerVehicles[playerID].ph1.angularVelocityX = 0f;
			global::MainGame.MainGame.playerVehicles[playerID].ph1.angularVelocityY = 0f;
			global::MainGame.MainGame.playerVehicles[playerID].ph1.angularVelocityZ = 0f;
			mainC.physicsMain.getPosition_new(ref global::MainGame.MainGame.playerVehicles[playerID].ph1, num8 * global::Physics.Physics.timeMod);
			global::MainGame.MainGame.playerVehicles[playerID].ph1.forceZ = 0f;
		}
		players[playerID].charMain.pos2.v[0] = global::MainGame.MainGame.playerVehicles[playerID].ph1.x;
		players[playerID].charMain.pos2.v[1] = global::MainGame.MainGame.playerVehicles[playerID].ph1.y;
		players[playerID].charMain.pos2.v[2] = global::MainGame.MainGame.playerVehicles[playerID].ph1.z;
		if (global::AI.AI.ais[aiID].aiType == 0)
		{
			num10 = num4 / data2 * 0.4f;
			num10 = global::MainGame.MainGame.playerVehicles[playerID].throttleSpeed * 0.4f + num10;
			mainC.soundsMain.Play_Moving_Continual_Sound(playerID, 2, stop: false, 0.5f + global::MainGame.MainGame.playerVehicles[playerID].throttleSpeed * 0.5f, num10 * 100f, global::MainGame.MainGame.playerVehicles[playerID].ph1.x, global::MainGame.MainGame.playerVehicles[playerID].ph1.y, global::MainGame.MainGame.playerVehicles[playerID].ph1.z, global::MainGame.MainGame.playerVehicles[playerID].ph1.velocityX, global::MainGame.MainGame.playerVehicles[playerID].ph1.velocityY, global::MainGame.MainGame.playerVehicles[playerID].ph1.velocityZ);
			if (players[playerID].damagePercentageCapped > 0.1f)
			{
				Damage_Particles_For_Damaged_Player_Vehicle(playerID, threadID);
			}
		}
		global::MainGame.MainGame.playerVehicles[playerID].ph1.x = players[playerID].charMain.pos2.v[0];
		global::MainGame.MainGame.playerVehicles[playerID].ph1.y = players[playerID].charMain.pos2.v[1];
		global::MainGame.MainGame.playerVehicles[playerID].ph1.z = players[playerID].charMain.pos2.v[2];
		players[playerID].charP.position.v[0] = players[playerID].charMain.pos2.v[0];
		players[playerID].charP.position.v[1] = players[playerID].charMain.pos2.v[1];
		players[playerID].charP.position.v[2] = players[playerID].charMain.pos2.v[2];
		players[playerID].charP.velocity.v[0] = global::MainGame.MainGame.playerVehicles[playerID].ph1.velocityX;
		players[playerID].charP.velocity.v[1] = global::MainGame.MainGame.playerVehicles[playerID].ph1.velocityY;
		players[playerID].charP.velocity.v[2] = global::MainGame.MainGame.playerVehicles[playerID].ph1.velocityZ;
		players[playerID].mv[uBufferID] = result;
		players[playerID].zRotation = (float)Math.Acos((double)result.M22 / Math.Sqrt(result.M21 * result.M21 + result.M22 * result.M22)) * 57.29578f;
		if (result.M21 > 0f)
		{
			players[playerID].zRotation = 360f - players[playerID].zRotation;
		}
		global::MainGame.MainGame.playerVehicles[playerID].velocity = num2;
		Update_Player_Vehicle_BoundingBox(playerID, threadID, (ushort)(Vehicles.vehicles[curVehicle].numWheels + Vehicles.vehicles[curVehicle].numColPoints), ref players[playerID].mv[uBufferID]);
		num10 = global::MainGame.MainGame.playerVehicles[playerID].ph1.z;
		Confine_Player_Position_ToBoundaries(playerID, postCollision: false, threadID);
		global::MainGame.MainGame.playerVehicles[playerID].ph1.x = players[playerID].charP.position.v[0];
		global::MainGame.MainGame.playerVehicles[playerID].ph1.y = players[playerID].charP.position.v[1];
		global::MainGame.MainGame.playerVehicles[playerID].ph1.z = players[playerID].charP.position.v[2];
		if (players[playerID].onmap == 8)
		{
			players[playerID].onmap = 1;
			if (global::AI.AI.ais[aiID].aiType == 0)
			{
				Player_Vehicle_Explodes(playerID, threadID);
			}
		}
		mainC.weaponsMain.Process_Player_Weapons(playerID, players[playerID].primaryWeaponMountWeapon);
	}

	public void Move_Commander(float frameTime, byte threadID)
	{
		byte uBufferID = global::Rendering.Rendering.uBufferID;
		frameTime = global::MainGame.MainGame.frametime;
		if ((moving & 4) > 0)
		{
			zRotation += playerSpeedRotateRightStick * frameTime;
			if (zRotation > 360f)
			{
				zRotation -= 360f;
			}
			if (zRotation < 0f)
			{
				zRotation += 360f;
			}
			players[0].zRotation = zRotation;
		}
		Matrix.CreateRotationZ(zRotation * ((float)Math.PI / 180f), out players[0].mv[uBufferID]);
		ref Matrix reference = ref players[0].jt1[players[0].weapon1.jointID].mv[uBufferID];
		reference = Matrix.CreateRotationX(xRotation * ((float)Math.PI / 180f)) * players[0].mv[uBufferID];
		players[0].charP.velocity.v[0] = 0f;
		players[0].charP.velocity.v[1] = 0f;
		players[0].charP.velocity.v[2] = 0f;
		if ((moving & 1) != 0)
		{
			players[0].charP.velocity.v[0] = playerSpeed * players[0].mv[uBufferID].M21;
			players[0].charP.velocity.v[1] = playerSpeed * players[0].mv[uBufferID].M22;
		}
		if ((moving & 2) != 0)
		{
			players[0].charP.velocity.v[0] += playerSpeed * players[0].mv[uBufferID].M11;
			players[0].charP.velocity.v[1] += playerSpeed * players[0].mv[uBufferID].M12;
		}
		if (commanderView == 0)
		{
			players[0].charP.velocity.v[0] *= 5f;
			players[0].charP.velocity.v[1] *= 5f;
			mainC.physicsMain.getPosition(ref players[0].charP, frameTime);
		}
		else if (commanderViewingPlayer < 1 || !players[commanderViewingPlayer].active || players[commanderViewingPlayer].dead || (players[commanderViewingPlayer].onmap & 6) == 0)
		{
			commanderX = 0f;
			commanderY = 0f;
			commanderZ = 0f;
			commanderViewingPlayer = 0;
			for (int i = 1; i < 44; i++)
			{
				if (players[i].team == players[0].team && (players[i].onmap & 6) > 0)
				{
					commanderViewingPlayer = (byte)i;
					zRotation = players[i].zRotation;
					float num = (float)Math.Cos(zRotation * ((float)Math.PI / 180f));
					float num2 = (float)Math.Sin(zRotation * ((float)Math.PI / 180f));
					players[0].charP.position.v[0] = players[i].charP.position.v[0] + 40f * num2 + 20f * num;
					players[0].charP.position.v[1] = players[i].charP.position.v[1] - 40f * num + 20f * num2;
					commanderX = players[i].charP.position.v[0];
					commanderY = players[i].charP.position.v[1];
					break;
				}
			}
			if (commanderViewingPlayer < 1)
			{
				float num = mainC.terrainMain.Get_Terrain_Height(players[0].charP.position.v[0], players[0].charP.position.v[1], threadID);
				if (num < global::MainGame.MainGame.MaxDown)
				{
					num = global::MainGame.MainGame.MaxDown;
				}
				commanderView = 0;
				players[0].charP.position.v[2] = 1.5f * (global::MainGame.MainGame.MaxUp - num);
			}
		}
		else
		{
			players[0].charP.position.v[0] -= commanderX;
			players[0].charP.position.v[1] -= commanderY;
			mainC.physicsMain.getPosition(ref players[0].charP, frameTime);
			float num;
			if ((num = (float)Math.Sqrt(players[0].charP.position.v[0] * players[0].charP.position.v[0] + players[0].charP.position.v[1] * players[0].charP.position.v[1])) > 100f)
			{
				num /= 100f;
				players[0].charP.position.v[0] /= num;
				players[0].charP.position.v[1] /= num;
			}
			commanderX = players[commanderViewingPlayer].charP.position.v[0];
			commanderY = players[commanderViewingPlayer].charP.position.v[1];
			players[0].charP.position.v[0] += commanderX;
			players[0].charP.position.v[1] += commanderY;
			players[0].charP.position.v[2] = players[commanderViewingPlayer].charP.position.v[2] + 25f;
		}
	}

	public void Move_MainPlayer_FPS_While_Paused_In_MP(float frameTime, byte threadID)
	{
		long num = -1L;
		float num2 = 0f;
		float num3 = 0f;
		float num4 = 0f;
		byte uBufferID = global::Rendering.Rendering.uBufferID;
		_ = global::Rendering.Rendering.rBufferID;
		frameTime = global::MainGame.MainGame.frametime;
		Matrix.CreateRotationZ(zRotation * ((float)Math.PI / 180f), out players[0].mv[uBufferID]);
		float num5 = players[0].charP.position.v[0];
		float num6 = players[0].charP.position.v[1];
		float num7 = players[0].charP.position.v[2];
		players[0].charMain.pos1.v[0] = players[0].charP.position.v[0];
		players[0].charMain.pos1.v[1] = players[0].charP.position.v[1];
		players[0].charMain.pos1.v[2] = players[0].charP.position.v[2];
		mainC.physicsMain.getPosition(ref players[0].charP, frameTime);
		int num8 = (int)players[0].charMain.numUsed * 3;
		int num9 = 0;
		int num10 = 0;
		int num11 = 129;
		while (num9 < num8)
		{
			global::Collision.Collision.floatAr[threadID, num9++] = players[0].particlePrev[num10].v[0];
			global::Collision.Collision.floatAr[threadID, num9++] = players[0].particlePrev[num10].v[1];
			global::Collision.Collision.floatAr[threadID, num9++] = players[0].particlePrev[num10].v[2];
			global::Collision.Collision.floatAr[threadID, num11++] = players[0].charMain.v1[num10].v[0];
			global::Collision.Collision.floatAr[threadID, num11++] = players[0].charMain.v1[num10].v[1];
			global::Collision.Collision.floatAr[threadID, num11++] = players[0].charMain.v1[num10].v[2];
			num10++;
		}
		Update_Player_BoundingBox(0, players[0].charP.position.v[0], players[0].charP.position.v[1], players[0].charP.position.v[2], threadID);
		Confine_Player_Position_ToBoundaries(0, postCollision: false, threadID);
		players[0].charMain.pos2.v[0] = players[0].charP.position.v[0];
		players[0].charMain.pos2.v[1] = players[0].charP.position.v[1];
		players[0].charMain.pos2.v[2] = players[0].charP.position.v[2];
		num9 = mainC.collisionMain.CheckCollision_Detailed_List(ref players[0].charMain, (int)players[0].charMain.numUsed, 129, 143, 0, threadID);
		num = global::Collision.Collision.colIDT[threadID];
		if ((num9 & 4) > 0 && num10 < 0)
		{
			float num12 = Math.Abs(players[0].charP.velocity.v[2]);
			if (num12 > players[0].velocityTerminalThreshold && Player_Injured_Threaded(num12))
			{
				Player_Over(0, playerDied: true, threadID);
			}
			players[0].charP.velocity.v[0] = num3;
			players[0].charP.velocity.v[1] = num4;
			players[0].charP.velocity.v[2] = 0f;
			global::Joints.Joints.Save_Player_Joint_Points(0);
			if (num > -1)
			{
				players[0].charP.velocity.v[0] += global::GameObjects.GameObjects.objMaster[num].phys1.velocity.v[0];
				players[0].charP.velocity.v[1] += global::GameObjects.GameObjects.objMaster[num].phys1.velocity.v[1];
				players[0].charP.velocity.v[2] = global::GameObjects.GameObjects.objMaster[num].phys1.velocity.v[2];
			}
		}
		else if (num10 > -1)
		{
			if ((moving & 4) > 0)
			{
				zRotation -= num2;
				if (zRotation > 360f)
				{
					zRotation -= 360f;
				}
				if (zRotation < 0f)
				{
					zRotation -= 360f;
				}
			}
			players[0].zRotation = zRotation;
			Matrix.CreateRotationZ(zRotation * ((float)Math.PI / 180f), out players[0].mv[uBufferID]);
			players[0].charP.velocity.v[0] = 0f;
			players[0].charP.velocity.v[1] = 0f;
			players[0].charP.velocity.v[2] = 0f;
			players[0].charMain.pos2.v[0] = num5;
			players[0].charMain.pos2.v[1] = num6;
			players[0].charMain.pos2.v[2] = num7;
			global::Joints.Joints.Undo_Joint_Movement(0);
		}
		else
		{
			global::Joints.Joints.Save_Player_Joint_Points(0);
			if ((num9 & 1) == 1)
			{
				players[0].charP.velocity.v[0] = 0f;
			}
			if ((num9 & 2) == 1)
			{
				players[0].charP.velocity.v[1] = 0f;
			}
		}
		players[0].charP.position.v[0] = players[0].charMain.pos2.v[0];
		players[0].charP.position.v[1] = players[0].charMain.pos2.v[1];
		players[0].charP.position.v[2] = players[0].charMain.pos2.v[2];
		Update_Player_BoundingBox(0, players[0].charP.position.v[0], players[0].charP.position.v[1], players[0].charP.position.v[2], threadID);
		Confine_Player_Position_ToBoundaries(0, postCollision: true, threadID);
	}

	public void Sync_Local_Player_View()
	{
		mainC.maingameMain.Add_End_Of_Frame_Message(0);
	}

	public void Process_Sync_Local_Player_View_Message()
	{
		byte rBufferID = global::Rendering.Rendering.rBufferID;
		byte uBufferID = global::Rendering.Rendering.uBufferID;
		global::Rendering.Rendering.renderMainPlayer[uBufferID] = global::Rendering.Rendering.renderMainPlayer[rBufferID];
		players[0].posX[uBufferID] = players[0].posX[rBufferID];
		players[0].posY[uBufferID] = players[0].posY[rBufferID];
		players[0].posZ[uBufferID] = players[0].posZ[rBufferID];
		global::Joints.Joints.Sync_Player_Matrices(0, rBufferID, uBufferID);
		global::Rendering.Rendering.camPosGoal[uBufferID].X = global::Rendering.Rendering.camPos[rBufferID].X;
		global::Rendering.Rendering.camPosGoal[uBufferID].Y = global::Rendering.Rendering.camPos[rBufferID].Y;
		global::Rendering.Rendering.camPosGoal[uBufferID].Z = global::Rendering.Rendering.camPos[rBufferID].Z;
		global::Rendering.Rendering.camObjectGoal[uBufferID].X = global::Rendering.Rendering.camObject[rBufferID].X;
		global::Rendering.Rendering.camObjectGoal[uBufferID].Y = global::Rendering.Rendering.camObject[rBufferID].Y;
		global::Rendering.Rendering.camObjectGoal[uBufferID].Z = global::Rendering.Rendering.camObject[rBufferID].Z;
		global::Rendering.Rendering.camUp[uBufferID].X = global::Rendering.Rendering.camUp[rBufferID].X;
		global::Rendering.Rendering.camUp[uBufferID].Y = global::Rendering.Rendering.camUp[rBufferID].Y;
		global::Rendering.Rendering.camUp[uBufferID].Z = global::Rendering.Rendering.camUp[rBufferID].Z;
	}

	public void Sync_All_Player_Positions()
	{
		byte rBufferID = global::Rendering.Rendering.rBufferID;
		byte uBufferID = global::Rendering.Rendering.uBufferID;
		for (ushort num = 0; num < global::MainGame.MainGame.maxGamePlayers; num++)
		{
			players[num].posX[uBufferID] = players[num].posX[rBufferID];
			players[num].posY[uBufferID] = players[num].posY[rBufferID];
			players[num].posZ[uBufferID] = players[num].posZ[rBufferID];
			global::Joints.Joints.Sync_Player_Matrices(num, rBufferID, uBufferID);
		}
	}

	public void Move_Camera_Past_Obstructions(byte threadID)
	{
		int num = 0;
		byte uBufferID = global::Rendering.Rendering.uBufferID;
		_ = global::Rendering.Rendering.rBufferID;
		float num2 = players[global::Rendering.Rendering.watchingPlayer].charP.position.v[0];
		float num3 = players[global::Rendering.Rendering.watchingPlayer].charP.position.v[1];
		float num4 = players[global::Rendering.Rendering.watchingPlayer].charP.position.v[2] + playerRaces[players[global::Rendering.Rendering.watchingPlayer].race].centerPoint[players[global::Rendering.Rendering.watchingPlayer].type] * 2f;
		float num5 = global::Rendering.Rendering.camPosGoal[uBufferID].X - num2;
		float num6 = global::Rendering.Rendering.camPosGoal[uBufferID].Y - num3;
		float num7 = global::Rendering.Rendering.camPosGoal[uBufferID].Z - num4;
		float num10;
		float num8;
		float num11;
		if (!global::MainGame.MainGame.viewFollowingObject)
		{
			num8 = (float)Math.Sqrt(num5 * num5 + num6 * num6 + num7 * num7);
			if (num8 != 0f)
			{
				global::Collision.Collision.floatAr[threadID, 0] = 0f;
				global::Collision.Collision.floatAr[threadID, 1] = 0f;
				global::Collision.Collision.floatAr[threadID, 2] = 0f;
				int Number = -1;
				Vector3 InitialRayStart = default(Vector3);
				Vector3 InitialRayEnd = default(Vector3);
				Vector3 IntersectPosition = default(Vector3);
				Vector3 IntersectNormal = default(Vector3);
				float num9 = num8;
				num10 = num5;
				num8 = num6;
				num11 = num7;
				InitialRayStart.X = num2;
				InitialRayStart.Y = num3;
				InitialRayStart.Z = num4;
				InitialRayEnd.X = num2 + num5;
				InitialRayEnd.Y = num3 + num6;
				InitialRayEnd.Z = num4 + num7;
				short returnValueZoneCheckIndex = 0;
				ushort returnValueZoneCheckObjID;
				while (mainC.zonesMain.Check_Zones_For_Point(InitialRayStart.X, InitialRayStart.Y, InitialRayStart.Z, 5f, returnValueZoneCheckIndex, 1, out returnValueZoneCheckIndex, out returnValueZoneCheckObjID))
				{
					int numObjects = Zones.zones[returnValueZoneCheckObjID].zoneList.numObjects;
					for (int i = 0; i < numObjects; i++)
					{
						if (mainC.collisionMain.Check_Polygon_Ray_Collision_Projectile(Zones.zones[returnValueZoneCheckObjID].zoneList.oList[i], Number, ref InitialRayStart, ref InitialRayEnd, ref Zones.zones[returnValueZoneCheckObjID].zoneList.matrixList[i], out var distance, out IntersectPosition, out IntersectNormal, out Number, threadID) && distance < num9)
						{
							num10 = IntersectPosition.X - num2;
							num8 = IntersectPosition.Y - num3;
							num11 = IntersectPosition.Z - num4;
							num9 = distance;
							num = 1;
						}
					}
				}
			}
			else
			{
				num10 = 0f;
				num8 = 1f;
				num11 = 0f;
				thirdPersonViewAdjustFactor = 0f;
			}
		}
		else
		{
			num10 = num5;
			num8 = num6;
			num11 = num7;
		}
		if (num != 0)
		{
			thirdPersonViewAdjustFactor += global::MainGame.MainGame.frametime * 1.6f;
			if (thirdPersonViewAdjustFactor > 0.34f)
			{
				thirdPersonViewAdjustFactor = 0.34f;
			}
		}
		else
		{
			thirdPersonViewAdjustFactor -= global::MainGame.MainGame.frametime * 0.8f;
			if (thirdPersonViewAdjustFactor < 0f)
			{
				thirdPersonViewAdjustFactor = 0f;
			}
		}
		float num12 = 1f - thirdPersonViewAdjustFactor;
		num10 *= num12;
		num8 *= num12;
		num11 *= num12;
		thirdPersonViewDistanceSqr[uBufferID] = num10 * num10 + num8 * num8 + num11 * num11;
		if (thirdPersonViewDistanceSqr[uBufferID] == 0f)
		{
			num10 = 0f;
		}
		if (num12 != 1f)
		{
			float x = global::Rendering.Rendering.camPosGoal[uBufferID].X;
			float y = global::Rendering.Rendering.camPosGoal[uBufferID].Y;
			float z = global::Rendering.Rendering.camPosGoal[uBufferID].Z;
			global::Rendering.Rendering.camPosGoal[uBufferID].X = num2 + num10;
			global::Rendering.Rendering.camPosGoal[uBufferID].Y = num3 + num8;
			global::Rendering.Rendering.camPosGoal[uBufferID].Z = num4 + num11;
			global::Rendering.Rendering.camObjectGoal[uBufferID].X += global::Rendering.Rendering.camPosGoal[uBufferID].X - x;
			global::Rendering.Rendering.camObjectGoal[uBufferID].Y += global::Rendering.Rendering.camPosGoal[uBufferID].Y - y;
			global::Rendering.Rendering.camObjectGoal[uBufferID].Z += global::Rendering.Rendering.camPosGoal[uBufferID].Z - z;
		}
		global::Rendering.Rendering.eyeVec[uBufferID].v[0] = num5;
		global::Rendering.Rendering.eyeVec[uBufferID].v[1] = num6;
		global::Rendering.Rendering.eyeVec[uBufferID].v[2] = num7;
	}

	public void Move_Camera_Past_Obstructions_Fixed_Distance(float maxDistanceSqr, byte threadID)
	{
		int num = 0;
		byte uBufferID = global::Rendering.Rendering.uBufferID;
		_ = global::Rendering.Rendering.rBufferID;
		float x = global::Rendering.Rendering.camPosGoal[uBufferID].X;
		float y = global::Rendering.Rendering.camPosGoal[uBufferID].Y;
		float z = global::Rendering.Rendering.camPosGoal[uBufferID].Z;
		float num2 = x - global::Rendering.Rendering.camObjectGoal[uBufferID].X;
		float num3 = y - global::Rendering.Rendering.camObjectGoal[uBufferID].Y;
		float num4 = z - global::Rendering.Rendering.camObjectGoal[uBufferID].Z;
		global::Rendering.Rendering.eyeVec[uBufferID].v[0] = num2;
		global::Rendering.Rendering.eyeVec[uBufferID].v[1] = num3;
		global::Rendering.Rendering.eyeVec[uBufferID].v[2] = num4;
		if (global::MainGame.MainGame.viewFollowingObject)
		{
			return;
		}
		float num5 = (float)Math.Sqrt(num2 * num2 + num3 * num3 + num4 * num4);
		float num7;
		float num8;
		float num9;
		if (num5 != 0f)
		{
			int Number = -1;
			Vector3 InitialRayStart = default(Vector3);
			Vector3 InitialRayEnd = default(Vector3);
			Vector3 IntersectPosition = default(Vector3);
			Vector3 IntersectNormal = default(Vector3);
			num2 /= num5;
			num3 /= num5;
			num4 /= num5;
			if (num5 > maxDistanceSqr)
			{
				num5 = maxDistanceSqr;
			}
			float num6 = num5;
			num7 = x;
			num8 = y;
			num9 = z;
			InitialRayStart.X = x - num2 * num6;
			InitialRayStart.Y = y - num3 * num6;
			InitialRayStart.Z = z - num4 * num6;
			InitialRayEnd.X = x;
			InitialRayEnd.Y = y;
			InitialRayEnd.Z = z;
			global::Collision.Collision.floatAr[threadID, 0] = 0f;
			global::Collision.Collision.floatAr[threadID, 1] = 0f;
			global::Collision.Collision.floatAr[threadID, 2] = 0f;
			short returnValueZoneCheckIndex = 0;
			ushort returnValueZoneCheckObjID;
			while (mainC.zonesMain.Check_Zones_For_Point(InitialRayStart.X, InitialRayStart.Y, InitialRayStart.Z, 5f, returnValueZoneCheckIndex, 1, out returnValueZoneCheckIndex, out returnValueZoneCheckObjID))
			{
				int numObjects = Zones.zones[returnValueZoneCheckObjID].zoneList.numObjects;
				for (int i = 0; i < numObjects; i++)
				{
					if (mainC.collisionMain.Check_Polygon_Ray_Collision_Projectile(Zones.zones[returnValueZoneCheckObjID].zoneList.oList[i], Number, ref InitialRayStart, ref InitialRayEnd, ref Zones.zones[returnValueZoneCheckObjID].zoneList.matrixList[i], out var distance, out IntersectPosition, out IntersectNormal, out Number, threadID) && distance < num6)
					{
						num7 = IntersectPosition.X;
						num8 = IntersectPosition.Y;
						num9 = IntersectPosition.Z;
						num6 = distance;
						num = 1;
					}
				}
			}
		}
		else
		{
			num7 = x;
			num8 = y + 1f;
			num9 = z;
			thirdPersonViewAdjustFactor = 0f;
		}
		if (num == 1)
		{
			thirdPersonViewAdjustFactor += global::MainGame.MainGame.frametime * 1.6f;
			if (thirdPersonViewAdjustFactor > 1f)
			{
				thirdPersonViewAdjustFactor = 1f;
			}
		}
		else
		{
			thirdPersonViewAdjustFactor -= global::MainGame.MainGame.frametime * 0.8f;
			if (thirdPersonViewAdjustFactor < 0f)
			{
				thirdPersonViewAdjustFactor = 0f;
			}
		}
		num2 = num7 - x;
		num3 = num8 - y;
		num4 = num9 - z;
		global::Rendering.Rendering.camPosGoal[uBufferID].X = x + num2 * thirdPersonViewAdjustFactor;
		global::Rendering.Rendering.camPosGoal[uBufferID].Y = y + num3 * thirdPersonViewAdjustFactor;
		global::Rendering.Rendering.camPosGoal[uBufferID].Z = z + num4 * thirdPersonViewAdjustFactor;
	}

	public void Player_Torque_Response(long pID, short jointID)
	{
		if (players[pID].shotImpulse != 0f || players[pID].shotTorque != 0f)
		{
			players[pID].jt1[jointID].rotX += players[pID].shotImpulse;
			players[pID].jt1[jointID].rotY += players[pID].shotTorque;
			players[pID].inRecoil = 10;
			players[pID].shotImpulse = 0f;
			players[pID].shotTorque = 0f;
		}
		players[pID].jt1[jointID].targetAngle = 0f - players[pID].xRotation;
		if (players[pID].jt1[jointID].targetAngle < players[pID].jt1[jointID].minAngle)
		{
			players[pID].jt1[jointID].targetAngle = players[pID].jt1[jointID].minAngle;
		}
		else if (players[pID].jt1[jointID].targetAngle > players[pID].jt1[jointID].maxAngle)
		{
			players[pID].jt1[jointID].targetAngle = players[pID].jt1[jointID].maxAngle;
		}
		float num = Math.Abs(players[pID].jt1[jointID].targetAngle - players[pID].jt1[jointID].rotX);
		if (players[pID].inRecoil == 0)
		{
			num /= 0.05f;
			if (num > players[pID].jt1[jointID].angleSpeed)
			{
				players[pID].jt1[jointID].angleSpeed = num;
			}
			return;
		}
		if (players[pID].shotImpulse != 0f || players[pID].shotTorque != 0f)
		{
			players[pID].jt1[jointID].angleSpeed = 10f;
			players[pID].jt1[jointID].pivot2Speed = 10f;
		}
		else
		{
			num /= 0.25f;
			if (num > players[pID].jt1[jointID].angleSpeed || num * 2f < players[pID].jt1[jointID].angleSpeed)
			{
				players[pID].jt1[jointID].angleSpeed = num;
			}
			num = Math.Abs(players[pID].jt1[jointID].targetPivot2 - players[pID].jt1[jointID].rotY);
			num /= 0.25f;
			if (num > players[pID].jt1[jointID].pivot2Speed || num * 2f < players[pID].jt1[jointID].pivot2Speed)
			{
				players[pID].jt1[jointID].pivot2Speed = num;
			}
		}
		players[pID].inRecoil--;
	}

	public float Set_Speed_If_Targeting_Enemy(ushort playerID, float frameTime, float minScreenDistSqr, float maxScreenDistSqr, float maxCheckDistance, bool doCollisionCheck, float rotSpeed, byte threadID)
	{
		byte uBufferID = global::Rendering.Rendering.uBufferID;
		float num = 1f;
		Vector3 InitialRayStart = default(Vector3);
		Vector3 InitialRayEnd = default(Vector3);
		Vector3 IntersectPosition = default(Vector3);
		Vector3 IntersectNormal = default(Vector3);
		targetingEnemy = false;
		if (!players[playerID].dead && (players[playerID].onmap & 6) > 0)
		{
			short num2 = -1;
			do
			{
				num2++;
				num2 = Find_Next_Player_Not_On_This_Team(num2, players[playerID].team);
				if (num2 <= -1 || rotSpeed == 0f)
				{
					continue;
				}
				targetVec2.v[0] = players[num2].charP.position.v[0];
				targetVec2.v[1] = players[num2].charP.position.v[1];
				targetVec2.v[2] = players[num2].charP.position.v[2] + playerRaces[players[num2].race].centerPoint[players[num2].type];
				float num3 = global::MainGame.MainGame.playerVehicles[playerID].weapons[global::MainGame.MainGame.playerVehicles[playerID].mounts[global::MainGame.MainGame.primaryWeaponMount].objectID].offset[0, 2].x + global::MainGame.MainGame.playerVehicles[playerID].weapons[global::MainGame.MainGame.playerVehicles[playerID].mounts[global::MainGame.MainGame.primaryWeaponMount].objectID].posX;
				float num4 = global::MainGame.MainGame.playerVehicles[playerID].weapons[global::MainGame.MainGame.playerVehicles[playerID].mounts[global::MainGame.MainGame.primaryWeaponMount].objectID].offset[0, 2].y + global::MainGame.MainGame.playerVehicles[playerID].weapons[global::MainGame.MainGame.playerVehicles[playerID].mounts[global::MainGame.MainGame.primaryWeaponMount].objectID].posY;
				float num5 = global::MainGame.MainGame.playerVehicles[playerID].weapons[global::MainGame.MainGame.playerVehicles[playerID].mounts[global::MainGame.MainGame.primaryWeaponMount].objectID].offset[0, 2].z + global::MainGame.MainGame.playerVehicles[playerID].weapons[global::MainGame.MainGame.playerVehicles[playerID].mounts[global::MainGame.MainGame.primaryWeaponMount].objectID].posZ;
				targetVec1.v[0] = targetVec2.v[0] - num3;
				targetVec1.v[1] = targetVec2.v[1] - num4;
				float num6 = (float)Math.Sqrt(targetVec1.v[0] * targetVec1.v[0] + targetVec1.v[1] * targetVec1.v[1]);
				if (!(num6 <= maxCheckDistance))
				{
					continue;
				}
				targetVec3.v[0] = global::MainGame.MainGame.playerVehicles[playerID].weapons[global::MainGame.MainGame.playerVehicles[playerID].mounts[global::MainGame.MainGame.primaryWeaponMount].objectID].offset[0, 5].x;
				targetVec3.v[1] = global::MainGame.MainGame.playerVehicles[playerID].weapons[global::MainGame.MainGame.playerVehicles[playerID].mounts[global::MainGame.MainGame.primaryWeaponMount].objectID].offset[0, 5].y;
				float num7 = targetVec3.v[0] * targetVec1.v[0] + targetVec3.v[1] * targetVec1.v[1];
				if (!(num7 > 0f))
				{
					continue;
				}
				float num8 = players[num2].playerBoudingRadius * 2f / ((float)Math.PI * 2f * num6);
				if (!(num8 <= 0.03f) || !(num8 >= 0.004f))
				{
					continue;
				}
				float num9 = num8 * 360f / Math.Abs(rotSpeed);
				if (!(num9 < 5.25f))
				{
					continue;
				}
				float num10 = num8 * 360f / 0.25f / Math.Abs(rotSpeed);
				Vector3 source = default(Vector3);
				Vector3 source2 = default(Vector3);
				Vector3 source3 = default(Vector3);
				source.X = targetVec2.v[0];
				source.Y = targetVec2.v[1];
				source.Z = targetVec2.v[2];
				source2.X = source.X + global::Rendering.Rendering.matrixVInverse.M21 * players[num2].playerBoudingRadius;
				source2.Y = source.Y + global::Rendering.Rendering.matrixVInverse.M22 * players[num2].playerBoudingRadius;
				source2.Z = source.Z + global::Rendering.Rendering.matrixVInverse.M23 * players[num2].playerBoudingRadius;
				source3.X = global::Rendering.Rendering.crossHairPositionGoal[global::Rendering.Rendering.rBufferID, 0];
				source3.Y = global::Rendering.Rendering.crossHairPositionGoal[global::Rendering.Rendering.rBufferID, 1];
				source3.Z = global::Rendering.Rendering.crossHairPositionGoal[global::Rendering.Rendering.rBufferID, 2];
				source = global::Rendering.Rendering.rGraphics.Viewport.Project(source, global::Rendering.Rendering.matrixPDB[uBufferID], global::Rendering.Rendering.matrixVDB[uBufferID], global::Rendering.Rendering.matrixI);
				if (float.IsNaN(source.X))
				{
					global::InputHandler.InputHandler.tw = 1f;
				}
				source2 = global::Rendering.Rendering.rGraphics.Viewport.Project(source2, global::Rendering.Rendering.matrixPDB[uBufferID], global::Rendering.Rendering.matrixVDB[uBufferID], global::Rendering.Rendering.matrixI);
				if (float.IsNaN(source2.X))
				{
					global::InputHandler.InputHandler.tw = 1f;
				}
				source3 = global::Rendering.Rendering.rGraphics.Viewport.Project(source3, global::Rendering.Rendering.matrixPDB[uBufferID], global::Rendering.Rendering.matrixVDB[uBufferID], global::Rendering.Rendering.matrixI);
				if (float.IsNaN(source3.X))
				{
					global::InputHandler.InputHandler.tw = 1f;
				}
				num8 = Math.Abs(source2.X - source.X);
				num9 = Math.Abs(source2.Y - source.Y);
				num8 = num8 * num8 + num9 * num9;
				num9 = Math.Abs(source3.X - source.X);
				float num11 = Math.Abs(source3.Y - source.Y);
				num9 = num9 * num9 + num11 * num11;
				if (!(num9 < num8) || !(num8 <= maxScreenDistSqr) || !(num8 >= minScreenDistSqr))
				{
					continue;
				}
				byte b = 0;
				int Number = -1;
				short returnValueZoneCheckIndex = 0;
				InitialRayStart.X = num3;
				InitialRayStart.Y = num4;
				InitialRayStart.Z = num5;
				InitialRayEnd.X = targetVec2.v[0];
				InitialRayEnd.Y = targetVec2.v[1];
				InitialRayEnd.Z = targetVec2.v[2];
				ushort returnValueZoneCheckObjID;
				while (b == 0 && mainC.zonesMain.Check_Zones_For_Ray(num3, num4, num5, InitialRayEnd.X, InitialRayEnd.Y, InitialRayEnd.Z, 5f, returnValueZoneCheckIndex, 1, out returnValueZoneCheckIndex, out returnValueZoneCheckObjID))
				{
					ushort numObjects = Zones.zones[returnValueZoneCheckObjID].zoneList.numObjects;
					for (ushort num12 = 0; num12 < numObjects; num12++)
					{
						if (mainC.collisionMain.Check_Polygon_Ray_Collision_Projectile(Zones.zones[returnValueZoneCheckObjID].zoneList.oList[num12], Number, ref InitialRayStart, ref InitialRayEnd, ref Zones.zones[returnValueZoneCheckObjID].zoneList.matrixList[num12], out num9, out IntersectPosition, out IntersectNormal, out Number, threadID))
						{
							b = 1;
						}
					}
				}
				if (b == 0)
				{
					num8 = num10;
					if (num8 < num)
					{
						targetingEnemy = true;
						num = num8;
						rotReturn = (num8 - 0.15f) * 0.1764f;
					}
				}
			}
			while (num2 != -1);
			if (targetingEnemy)
			{
				return num;
			}
			float num13 = rotReturn / 0.15f;
			rotReturn += frameTime;
			if (rotReturn > 0.15f)
			{
				rotReturn = 0.15f;
			}
			return 0.15f + 0.85f * num13;
		}
		return 1f;
	}

	public ushort Get_Team_Player_Is_Targeting(ushort playerID, byte threadID)
	{
		byte uBufferID = global::Rendering.Rendering.uBufferID;
		ushort result = 0;
		float num = 900f;
		if (!players[playerID].dead && (players[playerID].onmap & 6) > 0)
		{
			for (byte b = 1; b < global::MainGame.MainGame.maxGamePlayers; b++)
			{
				if (players[b].active && players[b].onmap == 4)
				{
					byte b2 = players[b].humanoidBackJoint;
					float num2 = players[b].jt1[b2].len / 2f;
					Matrix matrix = players[b].jt1[b2].mv[uBufferID];
					targetVec2.v[0] = num2 * matrix.M21 + matrix.M41 + players[b].charP.position.v[0];
					targetVec2.v[1] = num2 * matrix.M22 + matrix.M42 + players[b].charP.position.v[1];
					targetVec2.v[2] = num2 * matrix.M22 + matrix.M42 + players[b].charP.position.v[2];
					targetVec1.v[0] = targetVec2.v[0] - (players[playerID].weapon1.offset[0, 2].v[0] + players[playerID].charP.position.v[0]);
					targetVec1.v[1] = targetVec2.v[1] - (players[playerID].weapon1.offset[0, 2].v[1] + players[playerID].charP.position.v[1]);
					targetVec1.v[2] = targetVec2.v[2] - (players[playerID].weapon1.offset[0, 2].v[2] + players[playerID].charP.position.v[2]);
					num2 = (float)Math.Sqrt(targetVec1.v[0] * targetVec1.v[0] + targetVec1.v[1] * targetVec1.v[1] + targetVec1.v[2] * targetVec1.v[2]);
					if (num2 < num)
					{
						targetVec1.v[0] /= num2;
						targetVec1.v[1] /= num2;
						targetVec1.v[2] /= num2;
						targetVec3.v[0] = players[playerID].weapon1.offset[0, 5].v[0];
						targetVec3.v[1] = players[playerID].weapon1.offset[0, 5].v[1];
						targetVec3.v[2] = players[playerID].weapon1.offset[0, 5].v[2];
						float num3 = targetVec3.v[0] * targetVec1.v[0] + targetVec3.v[1] * targetVec1.v[1] + targetVec3.v[2] * targetVec1.v[2];
						if (num3 > 0f)
						{
							float num4 = num2 / num3;
							float num5 = num4 * num4 - num2 * num2;
							float maxPinH = players[b].jt1[b2].maxPinH;
							float num6 = (float)(Math.Sqrt(num5) / Math.Sqrt(maxPinH));
							if (num6 < 1f)
							{
								viewBox.pos1.v[0] = players[playerID].weapon1.offset[0, 2].v[0] + players[playerID].weapon1.box.pos1.v[0];
								viewBox.pos1.v[1] = players[playerID].weapon1.offset[0, 2].v[1] + players[playerID].weapon1.box.pos1.v[1];
								viewBox.pos1.v[2] = players[playerID].weapon1.offset[0, 2].v[2] + players[playerID].weapon1.box.pos1.v[2];
								viewBox.pos2.v[0] = targetVec2.v[0];
								viewBox.pos2.v[1] = targetVec2.v[1];
								viewBox.pos2.v[2] = targetVec2.v[2];
								float num7 = viewBox.pos2.v[0] - viewBox.pos1.v[0];
								float num8 = viewBox.pos2.v[1] - viewBox.pos1.v[1];
								float num9 = viewBox.pos2.v[2] - viewBox.pos1.v[2];
								num2 = (float)Math.Sqrt(Math.Pow(num7, 2.0) + Math.Pow(num8, 2.0) + Math.Pow(num9, 2.0));
								num7 /= num2;
								num8 /= num2;
								num9 /= num2;
								global::Collision.Collision.colIDT[threadID] = -1L;
								mainC.collisionMain.ResetIgnoreList(threadID, 1);
								int num10 = mainC.collisionMain.CheckRayCollision_Threaded(ref viewBox.pos1.v[0], ref viewBox.pos1.v[1], ref viewBox.pos1.v[2], num7, num8, num9, num2, -1L, 149, 0, threadID);
								if (num10 < 1)
								{
									num = num2;
									result = players[b].team;
								}
							}
						}
					}
				}
			}
		}
		return result;
	}

	public short Is_Commander_Targeting_Player(byte threadID, ushort team)
	{
		byte b = 0;
		byte uBufferID = global::Rendering.Rendering.uBufferID;
		short num = -1;
		float num2 = 20000f;
		float num3 = 0f;
		short num4 = -1;
		do
		{
			num4++;
			num4 = Find_Next_Player_Not_On_This_Team(num4, team);
			if (num4 <= -1)
			{
				continue;
			}
			byte b2 = players[num4].humanoidBackJoint;
			float num5 = players[num4].jt1[b2].len / 2f;
			Matrix matrix = players[num4].jt1[b2].mv[uBufferID];
			targetVec2.v[0] = num5 * matrix.M21 + matrix.M41 + players[num4].charP.position.v[0];
			targetVec2.v[1] = num5 * matrix.M22 + matrix.M42 + players[num4].charP.position.v[1];
			targetVec2.v[2] = num5 * matrix.M22 + matrix.M42 + players[num4].charP.position.v[2];
			targetVec1.v[0] = targetVec2.v[0] - players[0].charP.position.v[0];
			targetVec1.v[1] = targetVec2.v[1] - players[0].charP.position.v[1];
			targetVec1.v[2] = targetVec2.v[2] - players[0].charP.position.v[2];
			float num6 = (float)Math.Sqrt(targetVec1.v[0] * targetVec1.v[0] + targetVec1.v[1] * targetVec1.v[1] + targetVec1.v[2] * targetVec1.v[2]);
			if (!(num6 <= 1200f))
			{
				continue;
			}
			targetVec1.v[0] /= num6;
			targetVec1.v[1] /= num6;
			targetVec1.v[2] /= num6;
			matrix = players[0].jt1[players[0].weapon1.jointID].mv[uBufferID];
			targetVec3.v[0] = matrix.M21;
			targetVec3.v[1] = matrix.M22;
			targetVec3.v[2] = matrix.M23;
			float num7 = targetVec3.v[0] * targetVec1.v[0] + targetVec3.v[1] * targetVec1.v[1] + targetVec3.v[2] * targetVec1.v[2];
			if (!(num7 > 0f))
			{
				continue;
			}
			float num8 = num6 / num7;
			float num9 = num8 * num8 - num6 * num6;
			if (!(num9 <= 7f * num5 * num5))
			{
				continue;
			}
			viewBox.pos1.v[0] = players[0].charP.position.v[0];
			viewBox.pos1.v[1] = players[0].charP.position.v[1];
			viewBox.pos1.v[2] = players[0].charP.position.v[2];
			mainC.collisionMain.ResetIgnoreList(threadID, 1);
			int num10 = mainC.collisionMain.CheckRayCollision_Threaded(ref viewBox.pos1.v[0], ref viewBox.pos1.v[1], ref viewBox.pos1.v[2], matrix.M21, matrix.M22, matrix.M23, num8, -1L, 133, 0, threadID);
			if (num10 >= 1)
			{
				continue;
			}
			bool flag = false;
			float num11 = players[0].charP.position.v[2] + targetVec3.v[2] * num8;
			num11 = Math.Abs(num11 - players[num4].charP.position.v[2]);
			if (num11 < num5 * 2f)
			{
				num9 = players[0].charP.position.v[0] + targetVec3.v[0] * num8;
				float num12 = players[0].charP.position.v[1] + targetVec3.v[1] * num8;
				num9 -= players[num4].charP.position.v[0];
				num12 -= players[num4].charP.position.v[1];
				num9 = num9 * num9 + num12 * num12;
				if (num9 < players[num4].jt1[b2].maxPinH && (num < 0 || b < 1 || num2 > num6))
				{
					num2 = num8;
					num3 = num7;
					num = num4;
					b = 1;
					flag = true;
				}
			}
			if (!flag && (num < 0 || num3 < num7))
			{
				num3 = num7;
				num = num4;
			}
		}
		while (num4 != -1);
		return num;
	}

	public short Is_Commander_Targeting_Team(byte threadID, ulong teamMask)
	{
		byte b = 0;
		byte uBufferID = global::Rendering.Rendering.uBufferID;
		short num = -1;
		float num2 = 20000f;
		float num3 = 0f;
		short num4 = -1;
		do
		{
			num4++;
			num4 = Find_Next_Team_Player(num4, teamMask);
			if (num4 <= -1)
			{
				continue;
			}
			byte b2 = players[num4].humanoidBackJoint;
			float num5 = players[num4].jt1[b2].len / 2f;
			Matrix matrix = players[num4].jt1[b2].mv[uBufferID];
			targetVec2.v[0] = num5 * matrix.M21 + matrix.M41 + players[num4].charP.position.v[0];
			targetVec2.v[1] = num5 * matrix.M22 + matrix.M42 + players[num4].charP.position.v[1];
			targetVec2.v[2] = num5 * matrix.M22 + matrix.M42 + players[num4].charP.position.v[2];
			targetVec1.v[0] = targetVec2.v[0] - players[0].charP.position.v[0];
			targetVec1.v[1] = targetVec2.v[1] - players[0].charP.position.v[1];
			targetVec1.v[2] = targetVec2.v[2] - players[0].charP.position.v[2];
			float num6 = (float)Math.Sqrt(targetVec1.v[0] * targetVec1.v[0] + targetVec1.v[1] * targetVec1.v[1] + targetVec1.v[2] * targetVec1.v[2]);
			if (!(num6 <= 1200f))
			{
				continue;
			}
			targetVec1.v[0] /= num6;
			targetVec1.v[1] /= num6;
			targetVec1.v[2] /= num6;
			matrix = players[0].jt1[players[0].weapon1.jointID].mv[uBufferID];
			targetVec3.v[0] = matrix.M21;
			targetVec3.v[1] = matrix.M22;
			targetVec3.v[2] = matrix.M23;
			float num7 = targetVec3.v[0] * targetVec1.v[0] + targetVec3.v[1] * targetVec1.v[1] + targetVec3.v[2] * targetVec1.v[2];
			if (!(num7 > 0f))
			{
				continue;
			}
			float num8 = num6 / num7;
			float num9 = num8 * num8 - num6 * num6;
			if (!(num9 <= 7f * num5 * num5))
			{
				continue;
			}
			viewBox.pos1.v[0] = players[0].charP.position.v[0];
			viewBox.pos1.v[1] = players[0].charP.position.v[1];
			viewBox.pos1.v[2] = players[0].charP.position.v[2];
			mainC.collisionMain.ResetIgnoreList(threadID, 1);
			int num10 = mainC.collisionMain.CheckRayCollision_Threaded(ref viewBox.pos1.v[0], ref viewBox.pos1.v[1], ref viewBox.pos1.v[2], matrix.M21, matrix.M22, matrix.M23, num8, -1L, 133, 0, threadID);
			if (num10 >= 1)
			{
				continue;
			}
			bool flag = false;
			float num11 = players[0].charP.position.v[2] + targetVec3.v[2] * num8;
			num11 = Math.Abs(num11 - players[num4].charP.position.v[2]);
			if (num11 < num5 * 2f)
			{
				num9 = players[0].charP.position.v[0] + targetVec3.v[0] * num8;
				float num12 = players[0].charP.position.v[1] + targetVec3.v[1] * num8;
				num9 -= players[num4].charP.position.v[0];
				num12 -= players[num4].charP.position.v[1];
				num9 = num9 * num9 + num12 * num12;
				if (num9 < players[num4].jt1[b2].maxPinH && (num < 0 || b < 1 || num2 > num6))
				{
					num2 = num8;
					num3 = num7;
					num = num4;
					b = 1;
					flag = true;
				}
			}
			if (!flag && (num < 0 || num3 < num7))
			{
				num3 = num7;
				num = num4;
			}
		}
		while (num4 != -1);
		return num;
	}

	public void New_Frame_MP()
	{
		global::Rendering.Rendering.showSwapWeaponMessage[global::Rendering.Rendering.uBufferID] = false;
		if (global::Pickups.Pickups.playerPickupWeaponEnabled)
		{
			global::Rendering.Rendering.showSwapWeaponMessage[global::Rendering.Rendering.uBufferID] = true;
		}
		for (byte b = 0; b < 4; b++)
		{
			mpData[b].dataThisRound = false;
		}
		for (byte b = 0; b < 44; b++)
		{
			global::AI.AI.mpData[b].dataThisRound = false;
		}
		mainC.weaponsMain.New_Frame_Housekeeping();
	}

	public void New_Frame_SP()
	{
		global::Rendering.Rendering.showSwapWeaponMessage[global::Rendering.Rendering.uBufferID] = false;
		if (global::Pickups.Pickups.playerPickupWeaponEnabled)
		{
			global::Rendering.Rendering.showSwapWeaponMessage[global::Rendering.Rendering.uBufferID] = true;
		}
		mainC.weaponsMain.New_Frame_Housekeeping();
	}

	public void Damage_Particles_For_Damaged_Player_Vehicle(ushort vhID, byte threadID)
	{
		float damagePercentageCapped = players[vhID].damagePercentageCapped;
		global::MainGame.MainGame.playerVehicles[vhID].particleTimer += global::MainGame.MainGame.frametime;
		if (global::MainGame.MainGame.playerVehicles[vhID].particleTimer > 0.0385f - damagePercentageCapped * 0.0385f)
		{
			global::MainGame.MainGame.playerVehicles[vhID].particleTimer = 0f;
			byte uBufferID = global::Rendering.Rendering.uBufferID;
			Matrix matrix = global::MainGame.MainGame.playerVehicles[vhID].mv[uBufferID];
			global::Rendering.Rendering.npn.v[0] = 0.5f + damagePercentageCapped;
			global::Rendering.Rendering.npn.v[1] = 5f + damagePercentageCapped * 5f;
			global::Rendering.Rendering.npn.v[2] = 0.1f + damagePercentageCapped * 0.3f;
			damagePercentageCapped = global::MainGame.MainGame.playerVehicles[vhID].ph1.x + global::MainGame.MainGame.playerVehicles[vhID].damageParticleX * matrix.M11 + global::MainGame.MainGame.playerVehicles[vhID].damageParticleY * matrix.M21 + global::MainGame.MainGame.playerVehicles[vhID].damageParticleZ * matrix.M31;
			float y = global::MainGame.MainGame.playerVehicles[vhID].ph1.y + global::MainGame.MainGame.playerVehicles[vhID].damageParticleX * matrix.M12 + global::MainGame.MainGame.playerVehicles[vhID].damageParticleY * matrix.M22 + global::MainGame.MainGame.playerVehicles[vhID].damageParticleZ * matrix.M32;
			float z = global::MainGame.MainGame.playerVehicles[vhID].ph1.z + global::MainGame.MainGame.playerVehicles[vhID].damageParticleX * matrix.M13 + global::MainGame.MainGame.playerVehicles[vhID].damageParticleY * matrix.M23 + global::MainGame.MainGame.playerVehicles[vhID].damageParticleZ * matrix.M33;
			mainC.renderingMain.New_Particle_New(13, damagePercentageCapped, y, z, 0f - players[vhID].mv[uBufferID].M21, 0f - players[vhID].mv[uBufferID].M22, 0f - players[vhID].mv[uBufferID].M23, 0, threadID);
		}
	}

	public bool Player_Needs_To_Reload(ushort playerID)
	{
		bool result = false;
		for (byte b = 0; b < global::MainGame.MainGame.playerVehicles[playerID].numMounts; b++)
		{
			if (global::MainGame.MainGame.playerVehicles[playerID].mounts[b].type == 1 && global::MainGame.MainGame.playerVehicles[playerID].mounts[b].objectAttached == 1)
			{
				byte objectID = global::MainGame.MainGame.playerVehicles[playerID].mounts[b].objectID;
				if (global::MainGame.MainGame.playerVehicles[playerID].weapons[objectID].needToReload && mainC.weaponsMain.Player_Has_Ammo_For_Weapon(playerID, objectID) > 1)
				{
					result = true;
					sbyte animationReload = global::Weapons.Weapons.wp1[global::MainGame.MainGame.playerVehicles[playerID].weapons[objectID].weaponID].AnimationReload;
					if (players[playerID].animations[animationReload].status != 2)
					{
						players[playerID].animations[animationReload].var1 = objectID;
						mainC.programsMain.Start_Animation(playerID, ref players[playerID].jt1, ref players[playerID].animations, players[playerID].programCollection, animationReload, 1f, 1f);
					}
				}
			}
		}
		return result;
	}

	public bool Player_Needs_To_Chamber(ushort playerID)
	{
		bool result = false;
		for (byte b = 0; b < global::MainGame.MainGame.playerVehicles[playerID].numMounts; b++)
		{
			if (global::MainGame.MainGame.playerVehicles[playerID].mounts[b].type == 1 && global::MainGame.MainGame.playerVehicles[playerID].mounts[b].objectAttached == 1)
			{
				byte objectID = global::MainGame.MainGame.playerVehicles[playerID].mounts[b].objectID;
				if (global::MainGame.MainGame.playerVehicles[playerID].weapons[objectID].needToChamber && global::MainGame.MainGame.playerVehicles[playerID].weapons[objectID].currentRounds > 0)
				{
					result = true;
					sbyte animationChamber = global::Weapons.Weapons.wp1[global::MainGame.MainGame.playerVehicles[playerID].weapons[objectID].weaponID].AnimationChamber;
					if (players[playerID].animations[animationChamber].status != 2)
					{
						players[playerID].animations[animationChamber].var1 = objectID;
						mainC.programsMain.Start_Animation(playerID, ref players[playerID].jt1, ref players[playerID].animations, players[playerID].programCollection, animationChamber, 1f, 1f);
						if (global::Weapons.Weapons.wp1[global::MainGame.MainGame.playerVehicles[playerID].weapons[objectID].weaponID].snd_chamber != null)
						{
							mainC.soundsMain.Play_Priority_Sound(global::Weapons.Weapons.wp1[global::MainGame.MainGame.playerVehicles[playerID].weapons[objectID].weaponID].snd_chamber, players[playerID].charP.position.v[0], players[playerID].charP.position.v[1], players[playerID].charP.position.v[2], 0f, 0f, 0f);
						}
					}
				}
			}
		}
		return result;
	}

	public void Adjust_Player_Damage(ushort playerID, float damage, bool sendOnline)
	{
		players[playerID].damage += damage;
		players[playerID].damagePercentage = players[playerID].damage / players[playerID].maxDamage;
		players[playerID].damagePercentageCapped = players[playerID].damagePercentage;
		if (players[playerID].damagePercentageCapped > 1f)
		{
			players[playerID].damagePercentageCapped = 1f;
		}
		if (sendOnline && playerID < 1 && global::Networking.Networking.networkSessionReady)
		{
			mainC.maingameMain.Send_Special_Messages(6);
		}
	}

	public void Adjust_Player_Damage_To_Fixed_Amount(ushort playerID, float damage, bool sendOnline)
	{
		players[playerID].damage = damage;
		players[playerID].damagePercentage = players[playerID].damage / players[playerID].maxDamage;
		players[playerID].damagePercentageCapped = players[playerID].damagePercentage;
		if (players[playerID].damagePercentageCapped > 1f)
		{
			players[playerID].damagePercentageCapped = 1f;
		}
		if (sendOnline && playerID < 1 && global::Networking.Networking.networkSessionReady)
		{
			mainC.maingameMain.Send_Special_Messages(6);
		}
	}

	public void Adjust_Player_Damage_To_Zero(ushort playerID, bool sendOnline)
	{
		players[playerID].damage = 0f;
		players[playerID].damagePercentage = 0f;
		players[playerID].damagePercentageCapped = 0f;
		if (sendOnline && playerID < 1 && global::Networking.Networking.networkSessionReady)
		{
			mainC.maingameMain.Send_Special_Messages(6);
		}
	}

	public void Adjust_Player_Damage_By_Percent(ushort playerID, float adjAmount, bool sendOnline)
	{
		players[playerID].damage -= adjAmount * players[playerID].maxDamage;
		if (players[playerID].damage < 0f)
		{
			players[playerID].damage = 0f;
		}
		players[playerID].damagePercentage = players[playerID].damage / players[playerID].maxDamage;
		players[playerID].damagePercentageCapped = players[playerID].damagePercentage;
		if (players[playerID].damagePercentageCapped > 1f)
		{
			players[playerID].damagePercentageCapped = 1f;
		}
		if (sendOnline && playerID < 1 && global::Networking.Networking.networkSessionReady)
		{
			mainC.maingameMain.Send_Special_Messages(6);
		}
		if (global::MainGame.MainGame.commanderMode && global::MainGame.MainGame.Commander[players[playerID].team] > 0)
		{
			ref HalfSingle reference = ref global::Networking.Networking.networkHS[0];
			reference = new HalfSingle(players[playerID].damage);
			mainC.networkingMain.XBOX_Send_Network_Message_To_Gamer(16, mainC.networkingMain.XBOX_Get_Gamer_By_Act_ID(players[global::MainGame.MainGame.Commander[players[playerID].team]].id));
		}
	}

	public void Player_Hit(short victimID, short shooterID, sbyte damagedJoint, float damageAmount, int bulletID, StructsClass.vtex v1, byte threadID)
	{
		float num = 0.5f;
		if (players[victimID].team == players[shooterID].team && victimID != shooterID && bulletID > -1 && !global::MainGame.MainGame.allowTeamKills)
		{
			mainC.gameLogic.Game_Player_Hit((ushort)victimID, (ushort)shooterID);
		}
		else
		{
			if ((players[victimID].onmap & 0xC) == 0 || !players[victimID].active)
			{
				return;
			}
			bool dead = players[victimID].dead;
			players[victimID].deathFlyBackPercentage = 0f;
			if (damagedJoint < 0)
			{
				damagedJoint = (sbyte)players[victimID].humanoidBackJoint;
			}
			if (global::MainGame.MainGame.gameMode == 0)
			{
				float num2 = players[victimID].jt1[damagedJoint].damageMultiplier * damageAmount;
				if (victimID == 0)
				{
					if (!players[0].invincible)
					{
						if (shooterID != 0)
						{
							Adjust_Player_Damage(0, num2 * global::MainGame.MainGame.damageReduction, sendOnline: false);
						}
						else
						{
							Adjust_Player_Damage(0, num2, sendOnline: false);
						}
					}
					num = players[0].damagePercentageCapped;
					if (num < 1f)
					{
						global::Rendering.Rendering.hitColor[3] = 0.35f + 0.4f * num;
						mainC.inputMain.GamePad_Vibration_Set_Low(1f);
						if (global::Rendering.Rendering.numHitIndicators > 0)
						{
							float num3 = players[shooterID].charP.position.v[0] - players[victimID].charP.position.v[0];
							float num4 = players[shooterID].charP.position.v[1] - players[victimID].charP.position.v[1];
							float num5 = (float)Math.Sqrt(num3 * num3 + num4 * num4);
							if (num5 != 0f)
							{
								num4 /= num5;
								if (Math.Abs(num4) > 1f)
								{
									num4 = Math.Sign(num4);
								}
								num4 = (float)Math.Acos(num4);
								if (num3 > 0f)
								{
									num4 = (float)Math.PI * 2f - num4;
								}
								mainC.renderingMain.Add_Hit_Indicator(num4 - zRotation * ((float)Math.PI / 180f));
							}
						}
					}
					else if (!dead)
					{
						global::Rendering.Rendering.hitColor[3] = 0.75f;
					}
				}
				else
				{
					if (shooterID == 0)
					{
						Adjust_Player_Damage((ushort)victimID, num2 * global::MainGame.MainGame.damageIncrease, sendOnline: false);
					}
					else
					{
						Adjust_Player_Damage((ushort)victimID, num2, sendOnline: false);
					}
					num = players[victimID].damagePercentageCapped;
				}
				if (players[shooterID].team != players[victimID].team)
				{
					global::MainGame.MainGame.gameData.players[shooterID].shotsHit++;
				}
				if (num >= 1f)
				{
					if (dead)
					{
						return;
					}
					byte race = players[victimID].race;
					byte numBloodModels = playerRaces[race].numBloodModels;
					for (byte b = 0; b < numBloodModels; b++)
					{
						mainC.renderingMain.New_Solid_Particle(3, global::Weapons.Weapons.pfbV1T[threadID].v[0], global::Weapons.Weapons.pfbV1T[threadID].v[1], global::Weapons.Weapons.pfbV1T[threadID].v[2], players[victimID].mv[global::Rendering.Rendering.uBufferID].M21, players[victimID].mv[global::Rendering.Rendering.uBufferID].M22, players[victimID].mv[global::Rendering.Rendering.uBufferID].M23, players[victimID].mv[global::Rendering.Rendering.uBufferID].M11, players[victimID].mv[global::Rendering.Rendering.uBufferID].M12, players[victimID].mv[global::Rendering.Rendering.uBufferID].M13, players[victimID].charP.velocity.v[0], players[victimID].charP.velocity.v[1], players[victimID].charP.velocity.v[2] - -1.1253281f, 6f, 0.68f, playerRaces[race].bloodModelIDs[b]);
					}
					if (bulletID > -1)
					{
						players[victimID].deathFlyBackPercentage = global::Weapons.Weapons.ammo[global::Weapons.Weapons.bullet[bulletID].ammoIndex].deathFlyBackPercentage;
					}
					global::MainGame.MainGame.gameData.players[victimID].numDeaths++;
					if (players[shooterID].team != players[victimID].team)
					{
						global::MainGame.MainGame.gameData.players[shooterID].scoresI[0] += global::MainGame.MainGame.pointsForEnemyAiKill;
						global::MainGame.MainGame.gameData.players[shooterID].numKills++;
						teamPoints[players[shooterID].team] += global::MainGame.MainGame.teamPointsForEnemyAiDeath;
						players[shooterID].roundPts += global::MainGame.MainGame.pointsForEnemyAiKill;
						killStreak[shooterID]++;
						if (killStreak[shooterID] > maxKillStreak[shooterID])
						{
							maxKillStreak[shooterID] = killStreak[shooterID];
						}
						if (shooterID == 0)
						{
							global::AI.AI.levelKillCount++;
							if (global::MainGame.MainGame.soundWhenEnemeyKilled)
							{
								mainC.soundsMain.Play_Sound_NonPositional("Scored_Kill");
							}
							mainC.gameLogic.Game_Scored_Kill();
						}
					}
					else if (victimID != shooterID)
					{
						global::MainGame.MainGame.gameData.players[shooterID].scoresI[0] -= global::MainGame.MainGame.pointsForTeamKill;
						global::MainGame.MainGame.gameData.players[shooterID].teamKills++;
						teamPoints[players[shooterID].team] -= global::MainGame.MainGame.teamPointsForTeamKill;
						players[shooterID].roundPts -= global::MainGame.MainGame.pointsForTeamKill;
					}
					else
					{
						global::MainGame.MainGame.gameData.players[shooterID].scoresI[0] -= global::MainGame.MainGame.pointsForOwnDeath;
						global::MainGame.MainGame.gameData.players[shooterID].selfKills++;
						teamPoints[players[shooterID].team] -= global::MainGame.MainGame.teamPointsForOwnDeath;
						players[shooterID].roundPts -= global::MainGame.MainGame.pointsForOwnDeath;
					}
					mainC.userInterface.Mark_Window_As_Needing_Updating(10);
					mainC.userInterface.Mark_Window_As_Needing_Updating(9);
					Player_Over(victimID, playerDied: true, threadID);
				}
				else
				{
					if (players[victimID].speakingTimer <= 0f)
					{
						players[victimID].voiceCueID = mainC.soundsMain.Play_Voice(playerRaces[players[victimID].race].soundHurt[players[victimID].type], players[victimID].charP.position.v[0], players[victimID].charP.position.v[1], players[victimID].charP.position.v[2], 0f, 0f, 0f);
						players[victimID].speakingTimer = playerRaces[players[victimID].race].hurtTimerLength[players[victimID].type];
					}
					mainC.gameLogic.Game_Player_Hit((ushort)victimID, (ushort)shooterID);
				}
			}
			else if (global::MainGame.MainGame.maxHumanGamePlayers == global::MainGame.MainGame.maxGamePlayers)
			{
				if (victimID == 0)
				{
					mainC.renderingMain.Add_Camera_Shake(0.1f, 0.1f);
					if (shooterID != 0)
					{
						return;
					}
					float num2 = 0f;
					if (!players[0].invincible)
					{
						num2 = players[0].jt1[damagedJoint].damageMultiplier * damageAmount;
						Adjust_Player_Damage(0, num2, sendOnline: true);
					}
					num = players[0].damagePercentageCapped;
					if (global::MainGame.MainGame.commanderMode && global::MainGame.MainGame.Commander[players[0].team] > 0)
					{
						ref HalfSingle reference = ref global::Networking.Networking.networkHS[0];
						reference = new HalfSingle(players[0].damage);
						mainC.networkingMain.XBOX_Send_Network_Message_To_Gamer(16, mainC.networkingMain.XBOX_Get_Gamer_By_Act_ID(players[global::MainGame.MainGame.Commander[players[0].team]].id));
					}
					if (num < 1f)
					{
						global::Rendering.Rendering.hitColor[3] = 0.35f + 0.4f * num;
						mainC.inputMain.GamePad_Vibration_Set_Low(1f);
						if (players[0].speakingTimer <= 0f)
						{
							players[0].voiceCueID = mainC.soundsMain.Play_Voice(playerRaces[players[0].race].soundHurt[players[0].type], players[0].charP.position.v[0], players[0].charP.position.v[1], players[0].charP.position.v[2], 0f, 0f, 0f);
							players[0].speakingTimer = playerRaces[players[0].race].hurtTimerLength[players[0].type];
						}
					}
					else if (!dead)
					{
						global::MainGame.MainGame.gameData.players[0].selfKills++;
						global::MainGame.MainGame.gameData.players[0].numDeaths++;
						global::Rendering.Rendering.hitColor[3] = 0.75f;
						global::Networking.Networking.networkInts[0] = global::Networking.Networking.networkSession.LocalGamers[global::MainGame.MainGame.localNetworkGamerID].Id;
						global::Networking.Networking.networkInts[1] = global::Networking.Networking.networkSession.LocalGamers[global::MainGame.MainGame.localNetworkGamerID].Id;
						mainC.networkingMain.XBOX_Send_Network_Message3(3);
						if (global::Networking.Networking.isHost)
						{
							Update_Points_For_Human_Kill_By_Human_And_Send(0, 0);
						}
						Player_Over(0, playerDied: true, threadID);
					}
				}
				else if (shooterID == 0)
				{
					if (players[shooterID].team != players[victimID].team)
					{
						global::MainGame.MainGame.gameData.players[shooterID].shotsHit++;
					}
					float num2 = players[victimID].jt1[damagedJoint].damageMultiplier * damageAmount;
					global::Networking.Networking.networkInts[0] = players[victimID].id;
					global::Networking.Networking.networkInts[1] = global::Networking.Networking.networkSession.LocalGamers[global::MainGame.MainGame.localNetworkGamerID].Id;
					ref HalfSingle reference2 = ref global::Networking.Networking.networkHS[0];
					reference2 = new HalfSingle(num2);
					mainC.networkingMain.XBOX_Send_Network_Message_To_Gamer(2, mainC.networkingMain.XBOX_Get_Gamer_By_Act_ID(players[victimID].id));
				}
			}
			else if (victimID == 0)
			{
				mainC.renderingMain.Add_Camera_Shake(0.1f, 0.1f);
				num = players[0].damagePercentageCapped;
				if (shooterID == 0 || shooterID >= global::MainGame.MainGame.maxHumanGamePlayers)
				{
					float num2 = 0f;
					if (!players[0].invincible)
					{
						num2 = players[0].jt1[damagedJoint].damageMultiplier * damageAmount;
						if (shooterID != 0)
						{
							num2 *= global::MainGame.MainGame.damageReduction;
						}
						Adjust_Player_Damage(0, num2, sendOnline: true);
					}
					num = players[0].damagePercentageCapped;
					if (global::MainGame.MainGame.commanderMode && global::MainGame.MainGame.Commander[players[0].team] > 0)
					{
						ref HalfSingle reference3 = ref global::Networking.Networking.networkHS[0];
						reference3 = new HalfSingle(players[0].damage);
						mainC.networkingMain.XBOX_Send_Network_Message_To_Gamer(16, mainC.networkingMain.XBOX_Get_Gamer_By_Act_ID(players[global::MainGame.MainGame.Commander[players[0].team]].id));
					}
				}
				if (num < 1f)
				{
					global::Rendering.Rendering.hitColor[3] = 0.35f + 0.4f * num;
					mainC.inputMain.GamePad_Vibration_Set_Low(1f);
					if (global::Rendering.Rendering.numHitIndicators > 0)
					{
						float num3 = players[shooterID].charP.position.v[0] - players[victimID].charP.position.v[0];
						float num4 = players[shooterID].charP.position.v[1] - players[victimID].charP.position.v[1];
						float num5 = (float)Math.Sqrt(num3 * num3 + num4 * num4);
						if (num5 != 0f)
						{
							num4 /= num5;
							if (Math.Abs(num4) > 1f)
							{
								num4 = Math.Sign(num4);
							}
							num4 = (float)Math.Acos(num4);
							if (num3 > 0f)
							{
								num4 = (float)Math.PI * 2f - num4;
							}
							mainC.renderingMain.Add_Hit_Indicator(num4 - zRotation * ((float)Math.PI / 180f));
						}
					}
					if (players[0].speakingTimer <= 0f)
					{
						players[0].voiceCueID = mainC.soundsMain.Play_Voice(playerRaces[players[0].race].soundHurt[players[0].type], players[0].charP.position.v[0], players[0].charP.position.v[1], players[0].charP.position.v[2], 0f, 0f, 0f);
						players[0].speakingTimer = playerRaces[players[0].race].hurtTimerLength[players[0].type];
					}
				}
				else
				{
					if (dead)
					{
						return;
					}
					if (shooterID == 0)
					{
						global::MainGame.MainGame.gameData.players[0].selfKills++;
					}
					global::MainGame.MainGame.gameData.players[0].numDeaths++;
					global::Rendering.Rendering.hitColor[3] = 0.75f;
					global::Networking.Networking.networkInts[0] = global::Networking.Networking.networkSession.LocalGamers[global::MainGame.MainGame.localNetworkGamerID].Id;
					if (shooterID == 0)
					{
						global::Networking.Networking.networkInts[1] = global::Networking.Networking.networkSession.LocalGamers[global::MainGame.MainGame.localNetworkGamerID].Id;
					}
					else
					{
						global::Networking.Networking.networkBytes[0] = players[shooterID].aiID;
						global::Networking.Networking.networkInts[1] = -1;
					}
					mainC.networkingMain.XBOX_Send_Network_Message3(3);
					if (global::Networking.Networking.isHost)
					{
						if (shooterID == 0)
						{
							Update_Points_For_Human_Kill_By_Human_And_Send(0, 0);
						}
						else
						{
							Update_Points_For_Human_Kill_By_AI_And_Send(0, players[shooterID].aiID);
						}
					}
					if (shooterID != 0 && bulletID > -1)
					{
						players[victimID].deathFlyBackPercentage = global::Weapons.Weapons.ammo[global::Weapons.Weapons.bullet[bulletID].ammoIndex].deathFlyBackPercentage;
					}
					Player_Over(0, playerDied: true, threadID);
				}
			}
			else if (victimID < global::MainGame.MainGame.maxHumanGamePlayers)
			{
				if (shooterID == 0)
				{
					if (players[shooterID].team != players[victimID].team)
					{
						global::MainGame.MainGame.gameData.players[shooterID].shotsHit++;
					}
					float num2 = players[victimID].jt1[damagedJoint].damageMultiplier * damageAmount;
					if (shooterID >= global::MainGame.MainGame.maxHumanGamePlayers)
					{
						num2 *= global::MainGame.MainGame.damageReduction;
					}
					global::Networking.Networking.networkInts[0] = players[victimID].id;
					global::Networking.Networking.networkInts[1] = global::Networking.Networking.networkSession.LocalGamers[global::MainGame.MainGame.localNetworkGamerID].Id;
					ref HalfSingle reference4 = ref global::Networking.Networking.networkHS[0];
					reference4 = new HalfSingle(num2);
					mainC.networkingMain.XBOX_Send_Network_Message_To_Gamer(2, mainC.networkingMain.XBOX_Get_Gamer_By_Act_ID(players[victimID].id));
				}
			}
			else
			{
				if (shooterID != 0 && (shooterID < global::MainGame.MainGame.maxHumanGamePlayers || !global::AI.AI.ais[players[shooterID].aiID].locallyControlled))
				{
					return;
				}
				float num2 = players[victimID].jt1[damagedJoint].damageMultiplier * damageAmount;
				if (shooterID == 0)
				{
					num2 *= global::MainGame.MainGame.damageIncrease;
				}
				if (players[shooterID].team != players[victimID].team)
				{
					global::MainGame.MainGame.gameData.players[shooterID].shotsHit++;
				}
				if (!global::AI.AI.ais[players[victimID].aiID].locallyControlled)
				{
					global::Networking.Networking.networkBytes[0] = players[victimID].aiID;
					if (shooterID == 0)
					{
						global::Networking.Networking.networkInts[0] = global::Networking.Networking.networkSession.LocalGamers[global::MainGame.MainGame.localNetworkGamerID].Id;
					}
					else
					{
						global::Networking.Networking.networkInts[0] = -1;
						global::Networking.Networking.networkBytes[1] = players[shooterID].aiID;
					}
					ref HalfSingle reference5 = ref global::Networking.Networking.networkHS[0];
					reference5 = new HalfSingle(num2);
					mainC.networkingMain.XBOX_Send_Network_Message_To_Gamer(69, mainC.networkingMain.XBOX_Get_Gamer_By_Act_ID(global::AI.AI.ais[players[victimID].aiID].controllingPlayer));
					return;
				}
				Adjust_Player_Damage((ushort)victimID, num2, sendOnline: false);
				num = players[victimID].damagePercentageCapped;
				if (num >= 1f)
				{
					if (dead)
					{
						return;
					}
					global::AI.AI.levelKillCount++;
					global::MainGame.MainGame.gameData.players[victimID].numDeaths++;
					if (players[shooterID].team != players[victimID].team)
					{
						global::MainGame.MainGame.gameData.players[shooterID].numKills++;
						killStreak[shooterID]++;
						if (killStreak[shooterID] > maxKillStreak[shooterID])
						{
							maxKillStreak[shooterID] = killStreak[shooterID];
						}
					}
					else if (victimID != shooterID)
					{
						global::MainGame.MainGame.gameData.players[shooterID].teamKills++;
					}
					else
					{
						global::MainGame.MainGame.gameData.players[shooterID].selfKills++;
					}
					byte race = players[victimID].race;
					byte numBloodModels = playerRaces[race].numBloodModels;
					for (byte b = 0; b < numBloodModels; b++)
					{
						float num3 = (float)global::MainGame.MainGame.mainRandom.NextDouble();
						float num4 = (float)global::MainGame.MainGame.mainRandom.NextDouble();
						_ = Matrix.CreateRotationX(num4 * ((float)Math.PI / 4f) + (float)Math.PI / 4f) * Matrix.CreateRotationZ(num3 * ((float)Math.PI * 2f));
						mainC.renderingMain.New_Solid_Particle(3, global::Weapons.Weapons.pfbV1T[threadID].v[0], global::Weapons.Weapons.pfbV1T[threadID].v[1], global::Weapons.Weapons.pfbV1T[threadID].v[2], players[victimID].mv[global::Rendering.Rendering.uBufferID].M21, players[victimID].mv[global::Rendering.Rendering.uBufferID].M22, players[victimID].mv[global::Rendering.Rendering.uBufferID].M23, players[victimID].mv[global::Rendering.Rendering.uBufferID].M11, players[victimID].mv[global::Rendering.Rendering.uBufferID].M12, players[victimID].mv[global::Rendering.Rendering.uBufferID].M13, players[victimID].charP.velocity.v[0], players[victimID].charP.velocity.v[1], players[victimID].charP.velocity.v[2] - -1.1253281f, 6f, 0.68f, playerRaces[race].bloodModelIDs[b]);
					}
					mainC.userInterface.Mark_Window_As_Needing_Updating(10);
					mainC.userInterface.Mark_Window_As_Needing_Updating(9);
					if (bulletID > -1)
					{
						players[victimID].deathFlyBackPercentage = global::Weapons.Weapons.ammo[global::Weapons.Weapons.bullet[bulletID].ammoIndex].deathFlyBackPercentage;
					}
					Player_Over(victimID, playerDied: true, threadID);
					global::Networking.Networking.networkBytes[0] = players[victimID].aiID;
					global::Networking.Networking.networkBytes[1] = players[shooterID].aiID;
					global::Networking.Networking.networkInts[0] = -1;
					if (shooterID == 0)
					{
						global::Networking.Networking.networkInts[0] = global::Networking.Networking.networkSession.LocalGamers[global::MainGame.MainGame.localNetworkGamerID].Id;
					}
					mainC.networkingMain.XBOX_Send_Network_Message70(70);
					if (global::Networking.Networking.isHost)
					{
						if (shooterID == 0)
						{
							Update_Points_For_AI_Kill_And_Send(victimID, 0, 0);
						}
						else
						{
							Update_Points_For_AI_Kill_And_Send(victimID, -1, players[shooterID].aiID);
						}
					}
				}
				else
				{
					if (players[victimID].speakingTimer <= 0f)
					{
						players[victimID].voiceCueID = mainC.soundsMain.Play_Voice(playerRaces[players[victimID].race].soundHurt[players[victimID].type], players[victimID].charP.position.v[0], players[victimID].charP.position.v[1], players[victimID].charP.position.v[2], 0f, 0f, 0f);
						players[victimID].speakingTimer = playerRaces[players[victimID].race].hurtTimerLength[players[victimID].type];
					}
					mainC.gameLogic.Game_Player_Hit((ushort)victimID, (ushort)shooterID);
				}
			}
		}
	}

	public void Player_AI_Hit_From_Network(byte threadID)
	{
		int playerID = global::AI.AI.ais[global::Networking.Networking.networkBytes[0]].playerID;
		int num = Get_Player_Index(global::Networking.Networking.networkInts[0], -1);
		if (playerID < global::MainGame.MainGame.maxHumanGamePlayers || !players[playerID].active || (players[playerID].onmap & 0xC) == 0)
		{
			return;
		}
		Adjust_Player_Damage((ushort)playerID, global::Networking.Networking.networkHS[0].ToSingle(), sendOnline: true);
		float damagePercentageCapped = players[playerID].damagePercentageCapped;
		if (damagePercentageCapped < 1f)
		{
			if (players[playerID].speakingTimer <= 0f)
			{
				players[playerID].voiceCueID = mainC.soundsMain.Play_Voice(playerRaces[players[playerID].race].soundHurt[players[playerID].type], players[playerID].charP.position.v[0], players[playerID].charP.position.v[1], players[playerID].charP.position.v[2], 0f, 0f, 0f);
				players[playerID].speakingTimer = playerRaces[players[playerID].race].hurtTimerLength[players[playerID].type];
			}
		}
		else
		{
			if (players[playerID].dead)
			{
				return;
			}
			global::AI.AI.levelKillCount++;
			global::MainGame.MainGame.gameData.players[playerID].numDeaths++;
			mainC.networkingMain.XBOX_Send_Network_Message70(70);
			if (global::Networking.Networking.isHost)
			{
				if (num > -1)
				{
					Update_Points_For_AI_Kill_And_Send(playerID, num, 0);
				}
				else
				{
					Update_Points_For_AI_Kill_And_Send(playerID, -1, global::Networking.Networking.networkBytes[1]);
				}
			}
			Player_Over((short)playerID, playerDied: true, threadID);
		}
	}

	public void Player_AI_Killed_From_Network(byte threadID)
	{
		short playerID = global::AI.AI.ais[global::Networking.Networking.networkBytes[0]].playerID;
		short num = Get_Player_Index(global::Networking.Networking.networkInts[0], -1);
		global::AI.AI.levelKillCount++;
		if (playerID <= -1)
		{
			return;
		}
		byte race = players[playerID].race;
		byte numBloodModels = playerRaces[race].numBloodModels;
		for (byte b = 0; b < numBloodModels; b++)
		{
			mainC.renderingMain.New_Solid_Particle(3, players[playerID].charP.position.v[0], players[playerID].charP.position.v[1], players[playerID].charP.position.v[2], players[playerID].mv[global::Rendering.Rendering.uBufferID].M21, players[playerID].mv[global::Rendering.Rendering.uBufferID].M22, players[playerID].mv[global::Rendering.Rendering.uBufferID].M23, players[playerID].mv[global::Rendering.Rendering.uBufferID].M11, players[playerID].mv[global::Rendering.Rendering.uBufferID].M12, players[playerID].mv[global::Rendering.Rendering.uBufferID].M13, players[playerID].charP.velocity.v[0], players[playerID].charP.velocity.v[1], players[playerID].charP.velocity.v[2] - -1.1253281f, 6f, 0.68f, playerRaces[race].bloodModelIDs[b]);
		}
		Player_Over(playerID, playerDied: true, threadID);
		if (num == 0)
		{
			if (players[num].team != players[playerID].team)
			{
				global::MainGame.MainGame.gameData.players[num].numKills++;
				killStreak[num]++;
				if (killStreak[num] > maxKillStreak[num])
				{
					maxKillStreak[num] = killStreak[num];
				}
			}
			else if (playerID != num)
			{
				global::MainGame.MainGame.gameData.players[num].teamKills++;
			}
			else
			{
				global::MainGame.MainGame.gameData.players[num].selfKills++;
			}
		}
		if (global::Networking.Networking.isHost)
		{
			if (num > -1)
			{
				Update_Points_For_AI_Kill_And_Send(playerID, num, 0);
			}
			else
			{
				Update_Points_For_AI_Kill_And_Send(playerID, -1, global::Networking.Networking.networkBytes[1]);
			}
		}
	}

	public void Update_Points_For_AI_Kill_And_Send(int pID, int pID2, int aiID)
	{
		if (!global::MainGame.MainGame.roundOver && pID >= 0 && (pID2 >= 0 || (aiID >= 0 && aiID < global::AI.AI.numAI)))
		{
			if (pID2 < 0)
			{
				pID2 = global::AI.AI.ais[aiID].playerID;
			}
			if (players[pID].team != players[pID2].team)
			{
				teamPoints[players[pID2].team] += global::MainGame.MainGame.teamPointsForEnemyAiDeath;
				players[pID2].roundPts += global::MainGame.MainGame.pointsForEnemyAiKill;
				global::MainGame.MainGame.gameData.players[pID2].scoresI[0] += global::MainGame.MainGame.pointsForEnemyAiKill;
			}
			else if (pID == pID2)
			{
				teamPoints[players[pID2].team] -= global::MainGame.MainGame.teamPointsForOwnDeath;
				players[pID2].roundPts -= global::MainGame.MainGame.pointsForOwnDeath;
				global::MainGame.MainGame.gameData.players[pID2].scoresI[0] -= global::MainGame.MainGame.pointsForOwnDeath;
			}
			else
			{
				teamPoints[players[pID2].team] -= global::MainGame.MainGame.teamPointsForTeamKill;
				players[pID2].roundPts -= global::MainGame.MainGame.pointsForTeamKill;
				global::MainGame.MainGame.gameData.players[pID2].scoresI[0] -= global::MainGame.MainGame.pointsForTeamKill;
			}
			if (pID2 < global::MainGame.MainGame.maxHumanGamePlayers)
			{
				global::Networking.Networking.networkInts[0] = players[pID2].id;
				global::Networking.Networking.networkInts[1] = global::MainGame.MainGame.gameData.players[pID2].scoresI[0];
				global::Networking.Networking.networkInts[2] = players[pID2].objectivePoints;
				global::Networking.Networking.networkInts[3] = teamPoints[0];
				global::Networking.Networking.networkInts[4] = teamPoints[1];
				mainC.networkingMain.XBOX_Send_Network_Message4(4);
			}
			mainC.maingameMain.Send_Special_Messages(7);
			if (global::Networking.Networking.isHost && mainC.gameLogic.Game_Is_MP_Round_Over())
			{
				mainC.networkingMain.XBOX_MP_Round_Over();
			}
			mainC.maingameMain.Send_Special_Messages(5);
			mainC.userInterface.Mark_Window_As_Needing_Updating(10);
			mainC.userInterface.Mark_Window_As_Needing_Updating(9);
		}
	}

	public void Update_Points_For_Human_Kill_By_AI_And_Send(int pID, int aiID)
	{
		if (!global::MainGame.MainGame.roundOver && pID >= 0 && aiID >= 0 && aiID < global::AI.AI.numAI)
		{
			int playerID = global::AI.AI.ais[aiID].playerID;
			if (players[pID].team != players[playerID].team)
			{
				teamPoints[players[playerID].team] += global::MainGame.MainGame.teamPointsForEnemyDeath;
				players[playerID].roundPts += global::MainGame.MainGame.pointsForEnemyDeath;
				global::MainGame.MainGame.gameData.players[playerID].scoresI[0] += global::MainGame.MainGame.pointsForEnemyDeath;
			}
			else if (pID == playerID)
			{
				teamPoints[players[playerID].team] -= global::MainGame.MainGame.teamPointsForOwnDeath;
				players[playerID].roundPts -= global::MainGame.MainGame.pointsForOwnDeath;
				global::MainGame.MainGame.gameData.players[playerID].scoresI[0] -= global::MainGame.MainGame.pointsForOwnDeath;
			}
			else
			{
				teamPoints[players[playerID].team] -= global::MainGame.MainGame.teamPointsForTeamKill;
				players[playerID].roundPts -= global::MainGame.MainGame.pointsForTeamKill;
				global::MainGame.MainGame.gameData.players[playerID].scoresI[0] -= global::MainGame.MainGame.pointsForTeamKill;
			}
			if (global::Networking.Networking.isHost && mainC.gameLogic.Game_Is_MP_Round_Over())
			{
				mainC.networkingMain.XBOX_MP_Round_Over();
			}
			mainC.userInterface.Mark_Window_As_Needing_Updating(10);
			mainC.userInterface.Mark_Window_As_Needing_Updating(9);
		}
	}

	public void Player_Hit_From_Network(byte threadID)
	{
		float num = 0.5f;
		int num2 = Get_Player_Index(global::Networking.Networking.networkInts[0], -1);
		int num3 = Get_Player_Index(global::Networking.Networking.networkInts[1], -1);
		if (num2 != 0 || (num3 < 0 && (global::MainGame.MainGame.maxHumanGamePlayers >= global::MainGame.MainGame.maxGamePlayers || global::Networking.Networking.networkBytes[0] >= global::AI.AI.numAI)) || !players[0].active || (players[0].onmap & 0xC) == 0)
		{
			return;
		}
		mainC.soundsMain.Play_Sound_NonPositional("Player_Hit");
		if (!players[0].invincible)
		{
			Adjust_Player_Damage((ushort)num2, global::Networking.Networking.networkHS[0].ToSingle(), sendOnline: true);
			num = players[num2].damagePercentageCapped;
		}
		if (global::MainGame.MainGame.commanderMode && global::MainGame.MainGame.Commander[players[0].team] > 0)
		{
			ref HalfSingle reference = ref global::Networking.Networking.networkHS[0];
			reference = new HalfSingle(players[0].damage);
			mainC.networkingMain.XBOX_Send_Network_Message_To_Gamer(16, mainC.networkingMain.XBOX_Get_Gamer_By_Act_ID(players[global::MainGame.MainGame.Commander[players[0].team]].id));
		}
		if (num < 1f)
		{
			global::Rendering.Rendering.hitColor[3] = 0.35f + 0.4f * num;
			mainC.inputMain.GamePad_Vibration_Set_Low(1f);
			if (players[num2].speakingTimer <= 0f)
			{
				players[num2].voiceCueID = mainC.soundsMain.Play_Voice(playerRaces[players[num2].race].soundHurt[players[num2].type], players[num2].charP.position.v[0], players[num2].charP.position.v[1], players[num2].charP.position.v[2], 0f, 0f, 0f);
				players[num2].speakingTimer = playerRaces[players[num2].race].hurtTimerLength[players[num2].type];
			}
		}
		else
		{
			if (players[0].dead)
			{
				return;
			}
			global::MainGame.MainGame.gameData.players[0].numDeaths++;
			global::Rendering.Rendering.hitColor[3] = 0.75f;
			mainC.playersMain.Display_Player_Message(0);
			mainC.networkingMain.XBOX_Send_Network_Message3(3);
			if (global::Networking.Networking.isHost)
			{
				if (num3 > -1)
				{
					Update_Points_For_Human_Kill_By_Human_And_Send(num2, num3);
				}
				else
				{
					Update_Points_For_Human_Kill_By_AI_And_Send(num2, global::Networking.Networking.networkBytes[0]);
				}
			}
			Player_Over(0, playerDied: true, threadID);
		}
	}

	public void Player_Hit_Programatically(short victimID, float damage, StructsClass.vtex v1, byte threadID)
	{
		float num = 0f;
		if (victimID == 0)
		{
			if (!players[0].invincible)
			{
				Adjust_Player_Damage(0, damage, sendOnline: true);
				num = players[0].damagePercentageCapped;
			}
			if (num < 1f)
			{
				global::Rendering.Rendering.hitColor[3] = 0.35f + 0.4f * num;
				mainC.inputMain.GamePad_Vibration_Set_Low(1f);
			}
			else
			{
				global::Rendering.Rendering.hitColor[3] = 0.75f;
			}
		}
		else
		{
			Adjust_Player_Damage((ushort)victimID, damage, sendOnline: false);
			num = players[victimID].damagePercentageCapped;
		}
		if (num >= 1f)
		{
			byte race = players[victimID].race;
			ushort numBloodModels = playerRaces[race].numBloodModels;
			for (ushort num2 = 0; num2 < numBloodModels; num2++)
			{
				mainC.renderingMain.New_Solid_Particle(3, global::Weapons.Weapons.bulletBoxT[threadID].pos1.v[0], global::Weapons.Weapons.bulletBoxT[threadID].pos1.v[1], global::Weapons.Weapons.bulletBoxT[threadID].pos1.v[2], players[victimID].mv[global::Rendering.Rendering.uBufferID].M21, players[victimID].mv[global::Rendering.Rendering.uBufferID].M22, players[victimID].mv[global::Rendering.Rendering.uBufferID].M23, players[victimID].mv[global::Rendering.Rendering.uBufferID].M11, players[victimID].mv[global::Rendering.Rendering.uBufferID].M12, players[victimID].mv[global::Rendering.Rendering.uBufferID].M13, players[victimID].charP.velocity.v[0], players[victimID].charP.velocity.v[1], players[victimID].charP.velocity.v[2] - -1.1253281f, 6f, 1.68f, playerRaces[race].bloodModelIDs[num2]);
			}
			Player_Over(victimID, playerDied: true, threadID);
		}
		else if (players[victimID].speakingTimer <= 0f)
		{
			players[victimID].voiceCueID = mainC.soundsMain.Play_Voice(playerRaces[players[victimID].race].soundHurt[players[victimID].type], players[victimID].charP.position.v[0], players[victimID].charP.position.v[1], players[victimID].charP.position.v[2], 0f, 0f, 0f);
			players[victimID].speakingTimer = playerRaces[players[victimID].race].hurtTimerLength[players[victimID].type];
		}
	}

	public int Check_Player_Hit_By_Melee(byte playerStatus, float x1, float y1, float z1, float x2, float y2, float z2, ref StructsClass.vtex v1, ref StructsClass.vtex v2, ushort ignorePlayer, ushort teamID)
	{
		float num = x2 - x1;
		float num2 = y2 - y1;
		float num3 = z2 - z1;
		float num4 = 0f;
		float num5 = 0f;
		float num6 = (float)Math.Sqrt(num * num + num2 * num2);
		if (num6 > 0f)
		{
			num4 = num / num6;
			num5 = num2 / num6;
		}
		num6 = (float)Math.Sqrt(num * num + num2 * num2 + num3 * num3);
		float num7 = players[ignorePlayer].charP.position.v[0];
		float num8 = players[ignorePlayer].charP.position.v[1];
		float num9 = players[ignorePlayer].charP.position.v[2];
		for (int i = 0; i < global::MainGame.MainGame.maxGamePlayers; i++)
		{
			if (!players[i].active || (players[i].onmap & playerStatus) <= 0 || players[i].team == teamID || i == ignorePlayer)
			{
				continue;
			}
			float num10 = num7 - players[i].charP.position.v[0];
			float num11 = num8 - players[i].charP.position.v[1];
			float num12 = num9 - players[i].charP.position.v[2];
			float num13 = num10 * num10 + num11 * num11 + num12 * num12;
			float num14 = num6 + players[i].playerBoudingRadius;
			if (num13 <= num14 * num14)
			{
				num13 = (float)Math.Sqrt(num10 * num10 + num11 * num11);
				if (num13 == 0f)
				{
					v1.v[0] = num7;
					v1.v[1] = num8;
					v1.v[2] = num9;
					v2.v[0] = 0f;
					v2.v[1] = 0f;
					v2.v[2] = 1f;
					return i;
				}
				num10 /= num13;
				num11 /= num13;
				v1.v[0] = players[i].charP.position.v[0] + num10 * players[i].playerBoudingRadius;
				v1.v[1] = players[i].charP.position.v[1] + num11 * players[i].playerBoudingRadius;
				v1.v[2] = players[i].charP.position.v[2];
				v2.v[0] = num10;
				v2.v[1] = num11;
				v2.v[2] = 0f;
				num14 = num10 * num4 + num11 * num5;
				if (0f - num14 >= 0f)
				{
					return i;
				}
			}
		}
		return -1;
	}

	public float Check_Player_Impact_Threaded(ref StructsClass.particle_list p1, ref short pID, ref StructsClass.vtex v1, ref StructsClass.vtex v2, float bulletRadius, short ignorePlayer, byte threadID)
	{
		float num = -1f;
		pID = -1;
		float num2 = p1.pos2.v[0] - p1.pos1.v[0];
		float num3 = p1.pos2.v[1] - p1.pos1.v[1];
		float num4 = p1.pos2.v[2] - p1.pos1.v[2];
		float num5 = (float)Math.Sqrt(num2 * num2 + num3 * num3 + num4 * num4);
		if (num5 > 0f)
		{
			num2 /= num5;
			num3 /= num5;
			num4 /= num5;
		}
		v2.v[0] = num2;
		v2.v[1] = num3;
		v2.v[2] = num4;
		num5 += 1f;
		for (short num6 = 0; num6 < global::MainGame.MainGame.maxGamePlayers; num6++)
		{
			if (players[num6].active && (players[num6].onmap & 0xC) > 0 && num6 != ignorePlayer)
			{
				players[num6].charMain.b1.v[0] -= bulletRadius;
				players[num6].charMain.b1.v[1] -= bulletRadius;
				players[num6].charMain.b1.v[2] -= bulletRadius;
				players[num6].charMain.b2.v[0] += bulletRadius;
				players[num6].charMain.b2.v[1] += bulletRadius;
				players[num6].charMain.b2.v[2] += bulletRadius;
				cpiP2T[threadID].pos1.v[0] = p1.pos1.v[0];
				cpiP2T[threadID].pos1.v[1] = p1.pos1.v[1];
				cpiP2T[threadID].pos1.v[2] = p1.pos1.v[2];
				float num7 = mainC.collisionMain.CheckJointCollisionSingle_Threaded(num6, ref cpiP2T[threadID], ref players[num6].charMain.b1, ref players[num6].charMain.b2, num2, num3, num4, num5, bulletRadius, threadID);
				if (num7 >= 0f && (num < 0f || num7 < num))
				{
					num = num7;
					pID = num6;
					v1.v[0] = cpiP2T[threadID].pos1.v[0];
					v1.v[1] = cpiP2T[threadID].pos1.v[1];
					v1.v[2] = cpiP2T[threadID].pos1.v[2];
					num2 = v1.v[0] - p1.pos1.v[0];
					num3 = v1.v[1] - p1.pos1.v[1];
					num4 = v1.v[2] - p1.pos1.v[2];
					num5 = (float)Math.Sqrt(num2 * num2 + num3 * num3 + num4 * num4);
					if (num5 > 0f)
					{
						num2 /= num5;
						num3 /= num5;
						num4 /= num5;
					}
					num5 += 1f;
				}
				players[num6].charMain.b1.v[0] += bulletRadius;
				players[num6].charMain.b1.v[1] += bulletRadius;
				players[num6].charMain.b1.v[2] += bulletRadius;
				players[num6].charMain.b2.v[0] -= bulletRadius;
				players[num6].charMain.b2.v[1] -= bulletRadius;
				players[num6].charMain.b2.v[2] -= bulletRadius;
			}
		}
		return num;
	}

	public bool Check_Player_Impact_Melee(ushort playerID, float distance, byte ammoIndex, out ushort objectID, out float distanceHit)
	{
		byte rBufferID = global::Rendering.Rendering.rBufferID;
		float num = distance * distance;
		bool flag = false;
		float num2 = 0f;
		ushort num3 = 0;
		for (ushort num4 = 0; num4 < global::MainGame.MainGame.maxGamePlayers; num4++)
		{
			if (players[num4].onmap == 4 && num4 != playerID)
			{
				float num5 = players[num4].posX[rBufferID] - players[playerID].posX[rBufferID];
				float num6 = players[num4].posY[rBufferID] - players[playerID].posY[rBufferID];
				float num7 = players[num4].posZ[rBufferID] - players[playerID].posZ[rBufferID];
				if (!(Math.Abs(num5) > distance) && !(Math.Abs(num6) > distance) && !(Math.Abs(num7) > distance))
				{
					float num8 = num5 * num5 + num6 * num6 + num7 * num7;
					if (num8 < num && (num8 < num2 || !flag))
					{
						num8 = (float)Math.Sqrt(num5 * num5 + num6 * num6);
						if (num8 != 0f)
						{
							num5 /= num8;
							num6 /= num8;
						}
						num8 = num5 * players[playerID].mv[rBufferID].M21 + num6 * players[playerID].mv[rBufferID].M22;
						if (num8 > 0.707f)
						{
							num2 = num8;
							num3 = num4;
							flag = true;
						}
					}
				}
			}
		}
		objectID = num3;
		distanceHit = num2;
		return flag;
	}

	public float Check_Specific_Player_Impact_Threaded(ref StructsClass.particle_list p1, ref StructsClass.vtex v1, ref StructsClass.vtex v2, byte playerID, float bulletDiameter, byte threadID)
	{
		float num = p1.pos2.v[0] - p1.pos1.v[0];
		float num2 = p1.pos2.v[1] - p1.pos1.v[1];
		float num3 = p1.pos2.v[2] - p1.pos1.v[2];
		float num4 = (float)Math.Sqrt(num * num + num2 * num2 + num3 * num3);
		if (num4 > 0f)
		{
			num /= num4;
			num2 /= num4;
			num3 /= num4;
		}
		v2.v[0] = num;
		v2.v[1] = num2;
		v2.v[2] = num3;
		cpiP2T[threadID].pos1.v[0] = p1.pos1.v[0];
		cpiP2T[threadID].pos1.v[1] = p1.pos1.v[1];
		cpiP2T[threadID].pos1.v[2] = p1.pos1.v[2];
		float num5 = mainC.collisionMain.CheckJointCollisionSingle_Threaded(playerID, ref cpiP2T[threadID], ref players[playerID].charMain.b1, ref players[playerID].charMain.b2, num, num2, num3, num4, bulletDiameter, threadID);
		if (num5 >= 0f)
		{
			v1.v[0] = cpiP2T[threadID].pos1.v[0];
			v1.v[1] = cpiP2T[threadID].pos1.v[1];
			v1.v[2] = cpiP2T[threadID].pos1.v[2];
		}
		return num5;
	}

	public void Player_Respawn(byte threadID)
	{
		if (!respawnEnabled)
		{
			return;
		}
		global::MainGame.MainGame.roundStarting = false;
		mainC.maingameMain.Add_End_Of_Frame_Message(1);
		mainC.maingameMain.Add_End_Of_Frame_Message(2);
		global::Rendering.Rendering.mbRespawn = false;
		byte uBufferID = global::Rendering.Rendering.uBufferID;
		Reset_Local_Player_Variables();
		Set_Player_Race(0, players[0].race, players[0].type);
		switch (global::MainGame.MainGame.gameMode)
		{
		case 0:
			mainC.soundsMain.Reset_Round(stopNarrator: false);
			if (needSpawn)
			{
				if (global::MainGame.MainGame.useFixedSpawnPoint)
				{
					mainC.mapsMain.Get_Spawn_Point(ref players[0].charP.position, players[0].team, ref zRotation, global::MainGame.MainGame.maxLocalPlayerSpawnPoint, checkForEnemy: false, playerRaces[players[0].race].spawnHeight[players[0].type], 0f);
				}
				else
				{
					mainC.mapsMain.Get_Spawn_Point(ref players[0].charP.position, players[0].team, ref zRotation, -1, global::MainGame.MainGame.spSpawnCheckForEnemy, playerRaces[players[0].race].spawnHeight[players[0].type], playerRaces[players[0].race].boundingRadius[players[0].type]);
				}
			}
			mainC.levelsMain.Setup_Level();
			mainC.weaponsMain.Set_MainPlayer_Weapon(0, players[0].wpnIndex, reset: false);
			if (global::MainGame.MainGame.linearProgression)
			{
				mainC.weaponsMain.Reload_Player_Weapons_Immediately(0);
			}
			else
			{
				mainC.weaponsMain.Reset_Weapons_For_Respawn(0);
			}
			global::MainGame.MainGame.numLives--;
			if (global::MainGame.MainGame.curSpLevel != global::MainGame.MainGame.gameLevel)
			{
				global::MainGame.MainGame.gameLevel = (byte)global::MainGame.MainGame.curSpLevel;
			}
			break;
		case 1:
		{
			if (global::MainGame.MainGame.commanderMode)
			{
				if (global::MainGame.MainGame.Commander[players[0].team] == 0)
				{
					float num = mainC.terrainMain.Get_Terrain_Height(players[0].charP.position.v[0], players[0].charP.position.v[1], threadID);
					if (num < global::MainGame.MainGame.MaxDown)
					{
						num = global::MainGame.MainGame.MaxDown;
					}
					players[0].charP.acceleration.v[2] = 0f;
					zRotation = 0f;
					players[0].charP.position.v[0] = 0f;
					players[0].charP.position.v[1] = 0f;
					players[0].charP.position.v[2] = 0.9f * (global::MainGame.MainGame.MaxUp - num);
					global::MainGame.MainGame.isCommander = true;
				}
				else if (global::MainGame.MainGame.Commander[players[0].team] > 0)
				{
					ref HalfSingle reference = ref global::Networking.Networking.networkHS[0];
					reference = new HalfSingle(0f);
					mainC.networkingMain.XBOX_Send_Network_Message_To_Gamer(16, mainC.networkingMain.XBOX_Get_Gamer_By_Act_ID(players[global::MainGame.MainGame.Commander[players[0].team]].id));
				}
			}
			short num2;
			for (num2 = 1; num2 < global::MainGame.MainGame.maxHumanGamePlayers; num2++)
			{
				mpData[num2].currentPosX = players[num2].charP.position.v[0];
				mpData[num2].currentPosY = players[num2].charP.position.v[1];
				mpData[num2].currentPosZ = players[num2].charP.position.v[2];
			}
			while (num2 < global::MainGame.MainGame.maxGamePlayers)
			{
				if (players[num2].aiID < global::AI.AI.numAI)
				{
					global::AI.AI.mpData[players[num2].aiID].currentPosX = players[num2].charP.position.v[0];
					global::AI.AI.mpData[players[num2].aiID].currentPosY = players[num2].charP.position.v[1];
					global::AI.AI.mpData[players[num2].aiID].currentPosZ = players[num2].charP.position.v[2];
				}
				num2++;
			}
			if (needSpawn)
			{
				if (global::MainGame.MainGame.useFixedSpawnPoint)
				{
					mainC.mapsMain.Get_Spawn_Point(ref players[0].charP.position, players[0].team, ref zRotation, (sbyte)global::Networking.Networking.networkPlayers[0].playerArrayPosition, checkForEnemy: false, playerRaces[players[0].race].spawnHeight[players[0].type], 0f);
				}
				else
				{
					mainC.mapsMain.Get_Spawn_Point(ref players[0].charP.position, players[0].team, ref zRotation, -1, checkForEnemy: true, playerRaces[players[0].race].spawnHeight[players[0].type], playerRaces[players[0].race].boundingRadius[players[0].type]);
				}
				if (global::Networking.Networking.networkPlayers[0].playerArrayPosition < 0)
				{
					global::Networking.Networking.networkPlayers[0].playerArrayPosition = 0;
					global::InputHandler.InputHandler.tw = 5f;
				}
				players[0].charP.position.v[0] += remotePlayerPositionOffsetX[global::Networking.Networking.networkPlayers[0].playerArrayPosition];
				players[0].charP.position.v[1] += remotePlayerPositionOffsetY[global::Networking.Networking.networkPlayers[0].playerArrayPosition];
			}
			mainC.levelsMain.Setup_Level();
			mainC.weaponsMain.Set_MainPlayer_Weapon(0, players[0].wpnIndex, reset: false);
			mainC.maingameMain.Send_Special_Messages(2);
			if (global::MainGame.MainGame.linearProgression)
			{
				mainC.weaponsMain.Reload_Player_Weapons_Immediately(0);
			}
			else
			{
				mainC.weaponsMain.Reset_Weapons_For_Respawn(0);
			}
			break;
		}
		}
		if (zRotation < 0f)
		{
			zRotation += 360f;
		}
		if (zRotation > 360f)
		{
			zRotation -= 360f;
		}
		xRotation = 0f;
		players[0].posX[0] = players[0].charP.position.v[0];
		players[0].posY[0] = players[0].charP.position.v[1];
		players[0].posZ[0] = players[0].charP.position.v[2];
		players[0].posX[1] = players[0].charP.position.v[0];
		players[0].posY[1] = players[0].charP.position.v[1];
		players[0].posZ[1] = players[0].charP.position.v[2];
		players[0].programDeath = playerRaces[players[0].race].programDeath[players[0].type, global::MainGame.MainGame.mainRandom.Next(playerRaces[players[0].race].numDeathAnimations1)];
		players[0].programDeathBlownAway = playerRaces[players[0].race].programDeathBlownAway[players[0].type, global::MainGame.MainGame.mainRandom.Next(playerRaces[players[0].race].numDeathAnimations2)];
		global::Joints.Joints.Sync_Player_Matrices(0, uBufferID, global::Rendering.Rendering.rBufferID);
		global::Rendering.Rendering.moveViewToNewLocation = true;
		if (!freezeCamera)
		{
			mainC.soundsMain.Set_Listener_Position(players[0].charP.position.v[0], players[0].charP.position.v[1], players[0].charP.position.v[2], ref players[0].charP.velocity);
		}
		players[0].zRotation = zRotation;
		playerRot = Quaternion.CreateFromYawPitchRoll(0f, 0f, zRotation * ((float)Math.PI / 180f));
		Matrix.CreateRotationZ(zRotation * ((float)Math.PI / 180f), out players[0].mv[0]);
		ref Matrix reference2 = ref players[0].mv[1];
		reference2 = players[0].mv[0];
		mainC.vehicles.Reset_Player_Vehicle_Variables(0);
		mainC.vehicles.Set_Vehicle_Position(ref global::MainGame.MainGame.playerVehicles[0], players[0].charP.position.v[0], players[0].charP.position.v[1], players[0].charP.position.v[2], 0f, 0f, zRotation * ((float)Math.PI / 180f));
		if (!global::MainGame.MainGame.isCommander)
		{
			players[0].onmap = 2;
			players[0].transporter = -1f;
			players[0].transporterDirection = 1;
			players[0].respawnParticle = mainC.renderingMain.New_Particle_Spawn(9, players[0].respawnParticle, 0, players[0].charP.position.v[0], players[0].charP.position.v[1], players[0].charP.position.v[2]);
			mainC.gameLogic.Game_Reset_Joints_And_Programs();
			if (global::MainGame.MainGame.playerVehicles[0].numMounts > global::MainGame.MainGame.primaryWeaponMount && global::MainGame.MainGame.playerVehicles[0].mounts[global::MainGame.MainGame.primaryWeaponMount].type == 1 && global::MainGame.MainGame.playerVehicles[0].mounts[global::MainGame.MainGame.primaryWeaponMount].objectAttached == 1)
			{
				mainC.weaponsMain.Set_Weapon_View_Variables(global::MainGame.MainGame.playerVehicles[0].mounts[global::MainGame.MainGame.primaryWeaponMount].objectID);
			}
			else if (global::MainGame.MainGame.playerVehicles[0].numMounts > global::MainGame.MainGame.secondaryWeaponMount && global::MainGame.MainGame.playerVehicles[0].mounts[global::MainGame.MainGame.secondaryWeaponMount].type == 1 && global::MainGame.MainGame.playerVehicles[0].mounts[global::MainGame.MainGame.secondaryWeaponMount].objectAttached == 1)
			{
				mainC.weaponsMain.Set_Weapon_View_Variables(global::MainGame.MainGame.playerVehicles[0].mounts[global::MainGame.MainGame.secondaryWeaponMount].objectID);
			}
			if (global::MainGame.MainGame.gameMode == 1 && global::Networking.Networking.networkSessionReady)
			{
				Send_Position_Message(null, sendToGamer: false);
				mainC.maingameMain.Send_Special_Messages(3);
				Send_Position_Rotation_Race_Message();
			}
		}
		else
		{
			float num = mainC.terrainMain.Get_Terrain_Height(players[0].charP.position.v[0], players[0].charP.position.v[1], threadID);
			if (num < global::MainGame.MainGame.MaxDown)
			{
				num = global::MainGame.MainGame.MaxDown;
			}
			global::Joints.Joints.Translate_Player_Joints_To_Identity(0);
			players[0].onmap = 1;
			commanderViewingPlayer = 0;
			if (global::MainGame.MainGame.commanderLevel < 1)
			{
				global::MainGame.MainGame.commanderLevel = 1;
			}
			else if (global::MainGame.MainGame.commanderLevel > 6)
			{
				global::MainGame.MainGame.commanderLevel = 6;
			}
			players[0].charP.position.v[2] = 0.25f * (float)(int)global::MainGame.MainGame.commanderLevel * (global::MainGame.MainGame.MaxUp - num);
			global::MainGame.MainGame.commanderTeleportPlayer = 0;
			global::MainGame.MainGame.commanderTeleportTimer = 0f;
			global::MainGame.MainGame.commanderTeleportingPlayer = false;
			global::MainGame.MainGame.commanderItem = 1;
			for (short num2 = 1; num2 < 44; num2++)
			{
				if (players[num2].team == players[0].team && (players[num2].onmap & 6) > 0)
				{
					commanderViewingPlayer = (byte)num2;
					break;
				}
			}
		}
		eyeJoint = players[0].eyeJoint;
		humanoidBackJoint = players[0].humanoidBackJoint;
		headJoint = players[0].headJoint;
		shoulderJointL = players[0].shoulderJointL;
		shoulderJointR = players[0].shoulderJointR;
		playerViewJoint1 = playerRaces[players[0].race].firstPersonViewJoint1[players[0].type];
		players[0].weapon1.jointID = (sbyte)players[0].weaponJoint;
		mainC.gameLogic.Game_Respawn_Last(threadID);
		global::MainGame.MainGame.curAchievementReward = 0;
		mainC.maingameMain.Set_Current_Achievement_Reward(1);
		needSpawn = true;
		GC.Collect();
	}

	public void Player_Respawn_AI(ushort playerID, byte threadID)
	{
		Adjust_Player_Damage_To_Zero(playerID, sendOnline: false);
		killStreak[playerID] = 0;
		players[playerID].deathFlyBackPercentage = 0f;
		players[playerID].speakingTimer = 0f;
		players[playerID].weaponModifier = 0;
		players[playerID].shooting = false;
		players[playerID].shotOnce = 0;
		players[playerID].roundPts = 0;
		players[playerID].objectivePoints = 0;
		players[playerID].inRecoil = 0;
		players[playerID].dead = false;
		players[playerID].onmap = 2;
		players[playerID].playerIsMoving = 0;
		players[playerID].charP.velocity.v[0] = 0f;
		players[playerID].charP.velocity.v[1] = 0f;
		players[playerID].charP.velocity.v[2] = 0f;
		players[playerID].charP.angularVelocity.v[0] = 0f;
		players[playerID].charP.angularVelocity.v[1] = 0f;
		players[playerID].charP.angularVelocity.v[2] = 0f;
		players[playerID].velX = 0f;
		players[playerID].velY = 0f;
		players[playerID].velZ = 0f;
		players[playerID].impactX = 0f;
		players[playerID].impactY = 0f;
		players[playerID].impactZ = 0f;
		players[playerID].weapon1.jointID = (sbyte)players[playerID].weaponJoint;
		players[playerID].curVehicleIndex = 0;
		players[playerID].curVehicle = players[playerID].vehicles[0];
		players[playerID].programDeath = playerRaces[players[playerID].race].programDeath[players[playerID].type, global::MainGame.MainGame.mainRandom.Next(playerRaces[players[playerID].race].numDeathAnimations1)];
		players[playerID].programDeathBlownAway = playerRaces[players[playerID].race].programDeathBlownAway[players[playerID].type, global::MainGame.MainGame.mainRandom.Next(playerRaces[players[playerID].race].numDeathAnimations2)];
		players[playerID].voiceCueID = -1;
		global::MainGame.MainGame.gearDown[playerID] = 1;
		global::MainGame.MainGame.angularVelocity[playerID] = 0f;
		global::MainGame.MainGame.arcadeModeRisingAngle[playerID] = 0f;
		mainC.jointsMain.Reset_Player_Joint_Angles(playerID);
		Matrix.CreateRotationZ(players[playerID].zRotation * ((float)Math.PI / 180f), out players[playerID].mv[0]);
		ref Matrix reference = ref players[playerID].mv[1];
		reference = players[playerID].mv[0];
		global::MainGame.MainGame.arcadeModeRotAngle[playerID] = players[playerID].zRotation * ((float)Math.PI / 180f);
		for (short num = 0; num < 10; num++)
		{
			players[playerID].particles[num] = -1;
		}
		players[playerID].transporter = -1f;
		players[playerID].transporterDirection = 1;
		players[playerID].respawnParticle = mainC.renderingMain.New_Particle_Spawn(9, players[playerID].respawnParticle, playerID, players[playerID].charP.position.v[0], players[playerID].charP.position.v[1], players[playerID].charP.position.v[2]);
		global::Joints.Joints.Reset_Joint_Rotations_To_Zero((short)playerID);
		mainC.weaponsMain.firingStoppedAllPlayerWeapons(playerID);
		mainC.programsMain.Reset_Programs(ref players[playerID].pg1, ref players[playerID].animations, players[playerID].programCollection);
		mainC.jointsMain.Reset_Player_Joint_Targets(playerID);
		players[playerID].animations[global::Weapons.Weapons.wp1[players[playerID].primaryWeaponMountWeapon].AnimationHolding].status = 2;
		players[playerID].animations[players[playerID].programStationaryLegsBody].status = 2;
		global::Joints.Joints.Sync_Player_Matrices(playerID, global::Rendering.Rendering.uBufferID, global::Rendering.Rendering.rBufferID);
		mainC.weaponsMain.Reset_Weapons_For_Respawn((byte)playerID);
		mainC.weaponsMain.Update_Player_Weapon_Info((byte)playerID);
		ushort curVehicle = players[playerID].curVehicle;
		switch (global::MainGame.MainGame.playerVehicles[curVehicle].type)
		{
		case 0:
		case 8:
			Update_Player_BoundingBox(playerID, players[playerID].charP.position.v[0], players[playerID].charP.position.v[1], players[playerID].charP.position.v[2], threadID);
			break;
		case 1:
		case 2:
		case 5:
		case 6:
		case 7:
			Update_Player_Vehicle_BoundingBox(playerID, threadID, (ushort)(Vehicles.vehicles[curVehicle].numWheels + Vehicles.vehicles[curVehicle].numColPoints), ref players[playerID].mv[global::Rendering.Rendering.uBufferID]);
			break;
		}
		Confine_Player_Position_ToBoundaries((short)playerID, postCollision: true, threadID);
		mainC.vehicles.Reset_Player_Vehicle_Variables(playerID);
		mainC.vehicles.Set_Vehicle_Position(ref global::MainGame.MainGame.playerVehicles[playerID], players[playerID].charP.position.v[0], players[playerID].charP.position.v[1], players[playerID].charP.position.v[2], 0f, 0f, players[playerID].zRotation * ((float)Math.PI / 180f));
	}

	public void Player_Spawn_Time_Over(ushort pID)
	{
		switch (players[pID].transporterDirection)
		{
		case 1:
			players[pID].dead = false;
			players[pID].onmap = 4;
			if (pID == 0)
			{
				global::MainGame.MainGame.commanderTeleportPlayer = 1;
				global::MainGame.MainGame.commanderIsNotTeleporting = true;
			}
			players[pID].respawnParticle = -1;
			if (pID == 0 && global::Networking.Networking.inGame)
			{
				mainC.maingameMain.Send_Special_Messages(2);
				mainC.maingameMain.Send_Special_Messages(3);
			}
			break;
		case -1:
			if (pID == 0)
			{
				if (players[0].dead)
				{
					global::Rendering.Rendering.mbRespawn = true;
					if (global::MainGame.MainGame.gameMode == 0 && global::MainGame.MainGame.newRoundOnDeath)
					{
						mainC.playersMain.New_SinglePlayer_Round(minorRestart: true, 0);
					}
					else
					{
						respawnEnabled = false;
						global::MainGame.MainGame.gameState = 26;
					}
				}
				if (global::Networking.Networking.inGame)
				{
					mainC.maingameMain.Send_Special_Messages(2);
					mainC.maingameMain.Send_Special_Messages(3);
				}
				currentView = lastView;
			}
			mainC.soundsMain.Stop_Continual_Sound(pID);
			players[pID].respawnParticle = -1;
			players[pID].onmap = 1;
			break;
		case 0:
			break;
		}
	}

	public void Player_Respawn_From_Network(short actID)
	{
		short num = Get_Player_Index(actID, -1);
		if (num >= 0)
		{
			Adjust_Player_Damage_To_Zero((ushort)num, sendOnline: false);
			players[num].deathFlyBackPercentage = 0f;
			players[num].active = true;
			players[num].onmap = 2;
			players[num].playerIsMoving = 0;
			players[num].transporter = -1f;
			players[num].transporterDirection = 1;
			players[num].weapon1.jointID = (sbyte)players[num].weaponJoint;
			players[num].respawnParticle = mainC.renderingMain.New_Particle_Spawn(9, players[num].respawnParticle, num, players[num].charP.position.v[0], players[num].charP.position.v[1], players[num].charP.position.v[2]);
			players[num].impactX = 0f;
			players[num].impactY = 0f;
			players[num].impactZ = 0f;
			players[num].xRotation = (float)(int)global::Networking.Networking.networkBytes[0] / 255f * 180f - 90f;
			players[num].zRotation = (float)(int)global::Networking.Networking.networkBytes[1] / 255f * 360f;
			players[num].invincible = true;
			players[num].invincibleTimer = spawningInvincibleTime;
			players[num].programDeath = playerRaces[players[num].race].programDeath[players[num].type, global::MainGame.MainGame.mainRandom.Next(playerRaces[players[num].race].numDeathAnimations1)];
			players[num].programDeathBlownAway = playerRaces[players[num].race].programDeathBlownAway[players[num].type, global::MainGame.MainGame.mainRandom.Next(playerRaces[players[num].race].numDeathAnimations2)];
			mainC.weaponsMain.firingStoppedAllPlayerWeapons((ushort)num);
			Set_Player_Race((byte)num, global::Networking.Networking.networkBytes[2], (sbyte)global::Networking.Networking.networkBytes[3]);
			global::Joints.Joints.Reset_Joint_Rotations_To_Zero(num);
			mainC.programsMain.Reset_Programs(ref players[num].pg1, ref players[num].animations, players[num].programCollection);
			players[num].jt1[players[num].eyeJoint].rotX = 0f;
			players[num].jt1[players[num].eyeJoint].targetAngle = 0f;
			Update_Player_Matrix_From_Network(actID);
			players[num].charP.position.v[0] = (mpData[num].currentPosX = global::Networking.Networking.networkHS[9].ToSingle());
			players[num].charP.position.v[1] = (mpData[num].currentPosY = global::Networking.Networking.networkHS[10].ToSingle());
			players[num].charP.position.v[2] = (mpData[num].currentPosZ = global::Networking.Networking.networkHS[11].ToSingle());
			players[num].charP.velocity.v[0] = (mpData[num].velX = global::Networking.Networking.networkHS[12].ToSingle());
			players[num].charP.velocity.v[1] = (mpData[num].velY = global::Networking.Networking.networkHS[13].ToSingle());
			players[num].charP.velocity.v[2] = (mpData[num].velZ = global::Networking.Networking.networkHS[14].ToSingle());
			global::MainGame.MainGame.playerVehicles[num].ph1.x = players[num].charP.position.v[0];
			global::MainGame.MainGame.playerVehicles[num].ph1.y = players[num].charP.position.v[1];
			global::MainGame.MainGame.playerVehicles[num].ph1.z = players[num].charP.position.v[2];
			players[num].mv[global::Rendering.Rendering.uBufferID].M11 = global::Networking.Networking.networkHS[0].ToSingle();
			players[num].mv[global::Rendering.Rendering.uBufferID].M12 = global::Networking.Networking.networkHS[1].ToSingle();
			players[num].mv[global::Rendering.Rendering.uBufferID].M13 = global::Networking.Networking.networkHS[2].ToSingle();
			players[num].mv[global::Rendering.Rendering.uBufferID].M21 = global::Networking.Networking.networkHS[3].ToSingle();
			players[num].mv[global::Rendering.Rendering.uBufferID].M22 = global::Networking.Networking.networkHS[4].ToSingle();
			players[num].mv[global::Rendering.Rendering.uBufferID].M23 = global::Networking.Networking.networkHS[5].ToSingle();
			players[num].mv[global::Rendering.Rendering.uBufferID].M31 = global::Networking.Networking.networkHS[6].ToSingle();
			players[num].mv[global::Rendering.Rendering.uBufferID].M32 = global::Networking.Networking.networkHS[7].ToSingle();
			players[num].mv[global::Rendering.Rendering.uBufferID].M33 = global::Networking.Networking.networkHS[8].ToSingle();
			mpData[num].mv = players[num].mv[global::Rendering.Rendering.uBufferID];
			players[num].animations[global::Weapons.Weapons.wp1[players[num].primaryWeaponMountWeapon].AnimationHolding].status = 2;
			players[num].animations[players[num].programStationaryLegsBody].status = 2;
			mainC.programsMain.Start_Animation((ushort)num, ref players[num].jt1, ref players[num].animations, players[num].programCollection, global::Weapons.Weapons.wp1[players[num].primaryWeaponMountWeapon].AnimationHolding, 1f, 1f);
			mainC.programsMain.Start_Animation((ushort)num, ref players[num].jt1, ref players[num].animations, players[num].programCollection, players[num].programStationaryLegsBody, 1f, 1f);
			mainC.jointsMain.Update_Joints_For_New_Position(num);
			mainC.gameLogic.Game_Respawn_From_Network_Last((ushort)num);
		}
	}

	public void Teleport(sbyte direction)
	{
		switch (direction)
		{
		case 1:
		{
			players[0].transporter = -1f;
			players[0].transporterDirection = 1;
			float angle = xRotation;
			switch (global::MainGame.MainGame.gameMode)
			{
			case 0:
				mainC.mapsMain.Get_Spawn_Point(ref players[0].charP.position, players[0].team, ref angle, global::MainGame.MainGame.maxLocalPlayerSpawnPoint, global::MainGame.MainGame.spSpawnCheckForEnemy, playerRaces[players[0].race].spawnHeight[players[0].type], 0f);
				break;
			case 1:
			{
				mainC.mapsMain.Get_Spawn_Point(ref players[0].charP.position, players[0].team, ref angle, -1, checkForEnemy: true, playerRaces[players[0].race].spawnHeight[players[0].type], playerRaces[players[0].race].boundingRadius[players[0].type]);
				players[0].charP.position.v[0] += remotePlayerPositionOffsetX[global::Networking.Networking.networkPlayers[0].playerArrayPosition];
				players[0].charP.position.v[1] += remotePlayerPositionOffsetY[global::Networking.Networking.networkPlayers[0].playerArrayPosition];
				ref HalfSingle reference4 = ref global::Networking.Networking.networkHS[0];
				reference4 = new HalfSingle(players[0].charP.position.v[0]);
				ref HalfSingle reference5 = ref global::Networking.Networking.networkHS[1];
				reference5 = new HalfSingle(players[0].charP.position.v[1]);
				ref HalfSingle reference6 = ref global::Networking.Networking.networkHS[2];
				reference6 = new HalfSingle(players[0].charP.position.v[2]);
				global::Networking.Networking.networkBytes[0] = (byte)players[0].id;
				mainC.networkingMain.XBOX_Send_Network_Message23(23);
				break;
			}
			}
			break;
		}
		case -1:
			players[0].transporter = 2f;
			players[0].transporterDirection = -1;
			if (global::MainGame.MainGame.gameMode == 1)
			{
				ref HalfSingle reference = ref global::Networking.Networking.networkHS[0];
				reference = new HalfSingle(players[0].charP.position.v[0]);
				ref HalfSingle reference2 = ref global::Networking.Networking.networkHS[1];
				reference2 = new HalfSingle(players[0].charP.position.v[1]);
				ref HalfSingle reference3 = ref global::Networking.Networking.networkHS[2];
				reference3 = new HalfSingle(players[0].charP.position.v[2]);
				global::Networking.Networking.networkBytes[0] = (byte)players[0].id;
				mainC.networkingMain.XBOX_Send_Network_Message22(22);
			}
			break;
		}
		players[0].onmap = 2;
		global::MainGame.MainGame.needToTeleport = false;
		players[0].respawnParticle = mainC.renderingMain.New_Particle_Spawn(9, players[0].respawnParticle, 0, players[0].charP.position.v[0], players[0].charP.position.v[1], players[0].charP.position.v[2]);
		GC.Collect();
	}

	public void Teleport_In_From_Network()
	{
		players[0].transporter = 0f;
		players[0].transporterDirection = 1;
		players[0].charP.position.v[0] = global::Networking.Networking.networkHS[0].ToSingle();
		players[0].charP.position.v[1] = global::Networking.Networking.networkHS[1].ToSingle();
		players[0].charP.position.v[2] = global::Networking.Networking.networkHS[2].ToSingle();
		global::Networking.Networking.networkBytes[0] = (byte)players[0].id;
		mainC.networkingMain.XBOX_Send_Network_Message23(23);
		players[0].onmap = 2;
		global::MainGame.MainGame.needToTeleport = false;
		players[0].respawnParticle = mainC.renderingMain.New_Particle_Spawn(9, players[0].respawnParticle, 0, players[0].charP.position.v[0], players[0].charP.position.v[1], players[0].charP.position.v[2]);
		GC.Collect();
	}

	public void Player_Teleporting_Out()
	{
		short num = Get_Player_Index(global::Networking.Networking.networkBytes[0], -1);
		if (num >= 0)
		{
			players[num].transporter = 2f;
			players[num].transporterDirection = -1;
			players[num].onmap = 2;
			players[num].respawnParticle = mainC.renderingMain.New_Particle_Spawn(9, players[num].respawnParticle, num, players[num].charP.position.v[0], players[num].charP.position.v[1], players[num].charP.position.v[2]);
		}
	}

	public void Player_Teleporting_In()
	{
		short num = Get_Player_Index(global::Networking.Networking.networkBytes[0], -1);
		if (num >= 0)
		{
			players[num].playerIsMoving = 0;
			mainC.weaponsMain.firingStoppedAllPlayerWeapons((ushort)num);
			mainC.programsMain.Reset_Programs(ref players[num].pg1, ref players[num].animations, players[num].programCollection);
			players[num].animations[players[num].programStationaryArms].status = 2;
			players[num].animations[players[num].programStationaryLegsBody].status = 2;
			mainC.programsMain.Start_Animation((ushort)num, ref players[num].jt1, ref players[num].animations, players[num].programCollection, players[num].programStationaryArms, 1f, 1f);
			mainC.programsMain.Start_Animation((ushort)num, ref players[num].jt1, ref players[num].animations, players[num].programCollection, players[num].programStationaryLegsBody, 1f, 1f);
			mpData[num].currentPosX = global::Networking.Networking.networkHS[0].ToSingle();
			mpData[num].currentPosY = global::Networking.Networking.networkHS[1].ToSingle();
			mpData[num].currentPosZ = global::Networking.Networking.networkHS[2].ToSingle();
			mpData[num].velX = 0f;
			mpData[num].velY = 0f;
			mpData[num].velZ = 0f;
			players[num].charP.position.v[2] = global::Networking.Networking.networkHS[2].ToSingle();
			if (!mpData[num].dataThisRound)
			{
				mpData[num].timeFromLastUpdate = (float)(global::MainGame.MainGame.mainTime - mpData[num].lastUpdate) * 1E-07f;
				mpData[num].lastUpdate = global::MainGame.MainGame.mainTime;
				mpData[num].dataThisRound = true;
			}
			mpData[num].rotVelX = 0f;
			mpData[num].rotVelZ = 0f;
			players[num].onmap = 2;
			players[num].transporter = 0f;
			players[num].transporterDirection = 1;
			players[num].respawnParticle = mainC.renderingMain.New_Particle_Spawn(9, players[num].respawnParticle, num, mpData[num].currentPosX, mpData[num].currentPosY, mpData[num].currentPosZ);
		}
	}

	public void Player_Over(short playerID, bool playerDied, byte threadID)
	{
		byte uBufferID = global::Rendering.Rendering.uBufferID;
		byte rBufferID = global::Rendering.Rendering.rBufferID;
		mainC.vehicles.Player_Exits_All_Vehicles((ushort)playerID);
		if (playerDied)
		{
			Reset_Dying_Player_Variables((ushort)playerID);
		}
		else
		{
			if (playerID == 0)
			{
				global::MainGame.MainGame.viewChanged = false;
			}
			Reset_Player((ushort)playerID, isActive: true, players[playerID].race, (byte)players[playerID].type);
		}
		if (global::MainGame.MainGame.commanderSelect == playerID)
		{
			global::MainGame.MainGame.commanderSelect = -1;
		}
		if (playerID == 0)
		{
			moving = 0;
			global::Weapons.Weapons.recoilUp = 0f;
			global::Weapons.Weapons.recoilSide = 0f;
			global::MainGame.MainGame.usingIronSights = false;
			global::MainGame.MainGame.usingScope = false;
			global::MainGame.MainGame.quickScope = false;
			currentView = lastView;
			playerViewingDevice = false;
			respawnTimer = global::MainGame.MainGame.respawnTime;
			global::MainGame.MainGame.curTimeBeforeExitingMapOnDeath = global::MainGame.MainGame.timeBeforeExitingMapOnDeath;
			Sync_Local_Player_View();
			mainC.renderingMain.Set_Point_Light_Color(0, 0f, 0f, 0f, 0f);
			mainC.inputMain.GamePad_Vibration_Stop();
			if (global::MainGame.MainGame.gameMode == 0)
			{
				mainC.aiMain.Handle_Main_Player_Dying(threadID);
			}
			mainC.gameLogic.Game_Reset_Perks_For_Death();
		}
		else
		{
			global::Joints.Joints.Sync_Player_Matrices(playerID, rBufferID, uBufferID);
		}
		mainC.programsMain.Reset_Programs(ref players[playerID].pg1, ref players[playerID].animations, players[playerID].programCollection);
		if (playerDied)
		{
			players[playerID].voiceCueID = mainC.soundsMain.Play_Voice(playerRaces[players[playerID].race].soundDeath[players[playerID].type], players[playerID].charP.position.v[0], players[playerID].charP.position.v[1], players[playerID].charP.position.v[2], 0f, 0f, 0f);
			players[playerID].deathTime = 0f;
			players[playerID].onmap = 8;
			if (global::MainGame.MainGame.mainRandom.NextDouble() > (double)players[playerID].deathFlyBackPercentage || players[playerID].programDeathBlownAway == 0 || players[playerID].impactX + players[playerID].impactY + players[playerID].impactZ == 0f)
			{
				mainC.programsMain.Start_Animation((ushort)playerID, ref players[playerID].jt1, ref players[playerID].animations, players[playerID].programCollection, players[playerID].programDeath, 1f, 1f);
			}
			else
			{
				players[playerID].impactX *= 5f;
				players[playerID].impactY *= 5f;
				players[playerID].impactZ *= 5f;
				players[playerID].charP.velocity.v[2] += 16f;
				mainC.programsMain.Start_Animation((ushort)playerID, ref players[playerID].jt1, ref players[playerID].animations, players[playerID].programCollection, players[playerID].programDeathBlownAway, 1f, 1f);
			}
		}
		mainC.gameLogic.Game_Player_Over_Last((ushort)playerID);
		if (playerID == 0)
		{
			GC.Collect();
		}
	}

	public void Reset_Dying_Player_Variables(ushort playerID)
	{
		players[playerID].commanderTargeted = false;
		players[playerID].shooting = false;
		players[playerID].shotOnce = 0;
		players[playerID].dead = true;
		players[playerID].inRecoil = 0;
		players[playerID].shotImpulse = 0f;
		players[playerID].shotTorque = 0f;
		players[playerID].taunting = false;
		mainC.weaponsMain.firingStoppedAllPlayerWeapons(playerID);
		int i;
		for (i = 0; i < 10; i++)
		{
			players[0].particles[i] = -1;
		}
		i = players[playerID].transportParticle;
		if (i > -1 && global::Rendering.Rendering.particles[0, i].type == 8 && global::Rendering.Rendering.particles[0, i].refID == playerID)
		{
			global::Rendering.Rendering.particles[0, i].lifeTime = -1f;
		}
		if (i > -1 && global::Rendering.Rendering.particles[1, i].type == 8 && global::Rendering.Rendering.particles[1, i].refID == playerID)
		{
			global::Rendering.Rendering.particles[1, i].lifeTime = -1f;
		}
		mainC.avatarMain.Reset_Player(playerID);
	}

	public void Reset_Local_Player_Variables()
	{
		global::InputHandler.InputHandler.stickRightX = 0f;
		global::InputHandler.InputHandler.stickRightY = 0f;
		global::InputHandler.InputHandler.stickRightXVel = 0f;
		global::InputHandler.InputHandler.stickRightYVel = 0f;
		global::InputHandler.InputHandler.controlsSlowDownTimer = 0f;
		global::MainGame.MainGame.usingIronSights = false;
		global::MainGame.MainGame.usingScope = false;
		global::MainGame.MainGame.quickScope = false;
		global::MainGame.MainGame.overheadView = false;
		global::MainGame.MainGame.outOfRangeShown = false;
		global::MainGame.MainGame.playerVehicles[0].throttleSpeed = 0f;
		global::MainGame.MainGame.activateRetracts = false;
		global::MainGame.MainGame.viewFollowingObject = false;
		global::MainGame.MainGame.gearDown[0] = 1;
		global::MainGame.MainGame.needToResetPlane = false;
		global::MainGame.MainGame.healingAbility = 100.0;
		global::MainGame.MainGame.curIdleTime = 0f;
		global::MainGame.MainGame.spSaving = 0;
		global::Rendering.Rendering.scopeValue = (float)Math.PI / 4f;
		global::Rendering.Rendering.cameraAdjustmentHeight = 0f;
		global::Rendering.Rendering.cameraSpringDistance = 0.75f;
		global::Rendering.Rendering.curWeaponViewTime = 0f;
		global::Rendering.Rendering.viewPositionX = 0f;
		global::Rendering.Rendering.viewPositionY = 0f;
		global::Rendering.Rendering.viewVelocityX = 0f;
		global::Rendering.Rendering.viewVelocityY = 0f;
		groundCheckDistance = 0.25f;
		playerViewingDevice = false;
		killStreak[0] = 0;
		stickTimerX = 0f;
		stickTimerY = 0f;
		previousStickValueX = 0f;
		previousStickValueY = 0f;
		stickSpringAccelX = 1f;
		stickSpringAccelY = 1f;
		stickSpringVelX = 0f;
		stickSpringVelY = 0f;
		onGroundTimer = 0f;
		outOfBoundsTimer = 0f;
		outOfBoundsTimerRandom = 0f;
		controlsInUse = false;
		mainPlayerDeathTimer = 0f;
		needToChamber = false;
		needToReload = false;
		jumping = false;
		crouching = false;
		incover = false;
		reloading = false;
		chambering = false;
		changingWeapons = false;
		runTime = 4f;
		throwingGrenade = false;
		footStepTimer = 0f;
		fallingTimer = 0f;
		collisionWithGround = 0f;
		global::MainGame.MainGame.sprinting = false;
		global::MainGame.MainGame.usingScope = false;
		global::MainGame.MainGame.quickScope = false;
		global::MainGame.MainGame.usingIronSights = false;
		global::MainGame.MainGame.isCommander = false;
		global::MainGame.MainGame.needToTeleport = false;
		global::MainGame.MainGame.commanderTeleportPlayer = 1;
		global::MainGame.MainGame.commanderIsNotTeleporting = true;
		global::MainGame.MainGame.curTimeBeforeExitingMapOnDeath = global::MainGame.MainGame.timeBeforeExitingMapOnDeath;
		global::MainGame.MainGame.angularVelocity[0] = 0f;
		global::MainGame.MainGame.arcadeModeRisingAngle[0] = 0f;
		ref Vector3 reference = ref global::Rendering.Rendering.camUp[0];
		reference = global::Rendering.Rendering.worldUp;
		ref Vector3 reference2 = ref global::Rendering.Rendering.camUp[1];
		reference2 = global::Rendering.Rendering.worldUp;
		thirdPersonViewAdjustFactor = 0.34f;
		viewAdjX = 0f;
		viewAdjY = 0f;
		viewAdjZ = 0f;
		adjustmentAngleX = 0f;
		global::MainGame.MainGame.showCrossHairs[0] = 0;
		global::MainGame.MainGame.showCrossHairs[2] = 0;
		scopeValue = 0;
		scopeViewAdj = 1f;
		global::Weapons.Weapons.showTargetCrosshairTimer = 0f;
		mainC.gameLogic.Game_Reset_Perks_For_Death();
		mainC.jointsMain.Reset_Player_Joint_Angles(0);
		currentView = lastView;
		resetView = false;
		stepOver = 0f;
		Adjust_Player_Damage_To_Zero(0, sendOnline: false);
		players[0].deathFlyBackPercentage = 0f;
		players[0].taunting = false;
		players[0].charP.velocity.v[0] = 0f;
		players[0].charP.velocity.v[1] = 0f;
		players[0].charP.velocity.v[2] = 0f;
		players[0].charP.angularVelocity.v[0] = 0f;
		players[0].charP.angularVelocity.v[1] = 0f;
		players[0].charP.angularVelocity.v[2] = 0f;
		players[0].charP.acceleration.v[0] = 0f;
		players[0].charP.acceleration.v[1] = 0f;
		players[0].charP.acceleration.v[2] = -32.15223f;
		players[0].charP.angularAcceleration.v[0] = 0f;
		players[0].charP.angularAcceleration.v[1] = 0f;
		players[0].charP.angularAcceleration.v[2] = 0f;
		players[0].playerIsMoving = 32766;
		commanderX = 0f;
		commanderY = 0f;
		commanderZ = 0f;
		players[0].charP.fx = 0f;
		players[0].charP.fy = 0f;
		players[0].charP.fz = 0f;
		players[0].charP.rx = 0f;
		players[0].charP.ry = 0f;
		players[0].charP.rz = 0f;
		players[0].impactX = 0f;
		players[0].impactY = 0f;
		players[0].impactZ = 0f;
		players[0].speakingTimer = 0f;
		players[0].weaponModifier = 0;
		players[0].shootingAccuracy = 1f;
		players[0].dead = false;
		players[0].curVehicleIndex = 0;
		players[0].curVehicle = players[0].vehicles[0];
		players[0].invincible = true;
		players[0].invincibleTimer = spawningInvincibleTime;
		nextRemoteGamer = 0;
	}

	public void Reset_Local_Player_Variables_On_Round_Over()
	{
		global::InputHandler.InputHandler.stickRightX = 0f;
		global::InputHandler.InputHandler.stickRightY = 0f;
		global::InputHandler.InputHandler.stickRightXVel = 0f;
		global::InputHandler.InputHandler.stickRightYVel = 0f;
		global::InputHandler.InputHandler.controlsSlowDownTimer = 0f;
		global::MainGame.MainGame.usingIronSights = false;
		global::MainGame.MainGame.usingScope = false;
		global::MainGame.MainGame.quickScope = false;
		global::MainGame.MainGame.overheadView = false;
		global::MainGame.MainGame.outOfRangeShown = false;
		global::MainGame.MainGame.playerVehicles[0].throttleSpeed = 0f;
		global::MainGame.MainGame.activateRetracts = false;
		global::MainGame.MainGame.viewFollowingObject = false;
		global::MainGame.MainGame.gearDown[0] = 1;
		global::MainGame.MainGame.needToResetPlane = false;
		global::MainGame.MainGame.healingAbility = 100.0;
		global::MainGame.MainGame.curIdleTime = 0f;
		global::Rendering.Rendering.scopeValue = (float)Math.PI / 4f;
		global::MainGame.MainGame.sprinting = false;
		global::MainGame.MainGame.usingScope = false;
		global::MainGame.MainGame.quickScope = false;
		global::MainGame.MainGame.usingIronSights = false;
		global::MainGame.MainGame.needToTeleport = false;
		global::MainGame.MainGame.commanderTeleportPlayer = 1;
		global::MainGame.MainGame.commanderIsNotTeleporting = true;
		thirdPersonViewAdjustFactor = 0.34f;
		viewAdjX = 0f;
		viewAdjY = 0f;
		viewAdjZ = 0f;
		adjustmentAngleX = 0f;
		global::MainGame.MainGame.showCrossHairs[0] = 0;
		global::MainGame.MainGame.showCrossHairs[2] = 0;
		scopeValue = 0;
		scopeViewAdj = 1f;
		global::Weapons.Weapons.showTargetCrosshairTimer = 0f;
		currentView = lastView;
		resetView = false;
		players[0].taunting = false;
		players[0].playerIsMoving = 32766;
		commanderX = 0f;
		commanderY = 0f;
		commanderZ = 0f;
		players[0].speakingTimer = 0f;
		players[0].weaponModifier = 0;
	}

	public void SP_Player_Suicide()
	{
		if (global::MainGame.MainGame.restartIsDeath)
		{
			global::MainGame.MainGame.gameData.players[0].numDeaths++;
			global::MainGame.MainGame.gameData.players[0].scoresI[0] -= global::MainGame.MainGame.pointsForOwnDeath;
		}
	}

	public void MP_Player_Suicide()
	{
		if (global::MainGame.MainGame.restartIsDeath)
		{
			global::Networking.Networking.networkInts[0] = global::Networking.Networking.networkSession.LocalGamers[global::MainGame.MainGame.localNetworkGamerID].Id;
			global::Networking.Networking.networkInts[1] = global::Networking.Networking.networkSession.LocalGamers[global::MainGame.MainGame.localNetworkGamerID].Id;
			mainC.networkingMain.XBOX_Send_Network_Message3(3);
			if (global::Networking.Networking.isHost)
			{
				Update_Points_For_Human_Kill_By_Human_And_Send(0, 0);
			}
		}
	}

	public void Display_Player_Message(byte type)
	{
		if (type != 0)
		{
			return;
		}
		short num = Get_Player_Index(global::Networking.Networking.networkInts[0], -1);
		short num2 = Get_Player_Index(global::Networking.Networking.networkInts[1], -1);
		if (num > -1 && num2 > -1)
		{
			if (num != num2)
			{
				playerkMsg = players[num2].username + " killed " + players[num].username;
			}
			else
			{
				playerkMsg = players[num2].username + " shot himself";
			}
			mainC.fontmoduleMain.Add_Text_To_Onscreen_Text(playerkMsg);
		}
	}

	public void Player_Killed()
	{
		short num = Get_Player_Index(global::Networking.Networking.networkInts[0], -1);
		short num2 = Get_Player_Index(global::Networking.Networking.networkInts[1], -1);
		short num3 = num2;
		if (num3 < 0 && global::MainGame.MainGame.maxGamePlayers > global::MainGame.MainGame.maxHumanGamePlayers && global::Networking.Networking.networkBytes[0] < global::AI.AI.numAI)
		{
			num3 = global::AI.AI.ais[global::Networking.Networking.networkBytes[0]].playerID;
		}
		if (num > -1 && num3 > -1)
		{
			mainC.weaponsMain.firingStoppedAllPlayerWeapons((ushort)num);
			players[num].dead = true;
			if (num3 == 0)
			{
				if (players[num3].team != players[num].team)
				{
					global::MainGame.MainGame.gameData.players[num3].numKills++;
					if (global::MainGame.MainGame.soundWhenEnemeyKilled)
					{
						mainC.soundsMain.Play_Sound_NonPositional("Scored_Kill");
					}
					mainC.gameLogic.Game_Scored_Kill();
					killStreak[num3]++;
					if (killStreak[num3] > maxKillStreak[num3])
					{
						maxKillStreak[num3] = killStreak[num3];
					}
				}
				else if (num3 != num)
				{
					global::MainGame.MainGame.gameData.players[num3].teamKills++;
				}
				else
				{
					global::MainGame.MainGame.gameData.players[num3].selfKills++;
				}
			}
			if (num3 < global::MainGame.MainGame.maxHumanGamePlayers)
			{
				if (num != num3)
				{
					playerkMsg = players[num3].username + " killed " + players[num].username;
				}
				else
				{
					playerkMsg = players[num3].username + " shot himself";
				}
				mainC.fontmoduleMain.Add_Text_To_Onscreen_Text(playerkMsg);
			}
			if (global::Networking.Networking.isHost)
			{
				if (num2 > -1)
				{
					Update_Points_For_Human_Kill_By_Human_And_Send(num, num3);
				}
				else
				{
					Update_Points_For_Human_Kill_By_AI_And_Send(num, players[num3].aiID);
				}
			}
			if (global::MainGame.MainGame.commanderMode && global::MainGame.MainGame.isCommander && players[num].commanderTargeted && (players[num3].teamMask & enemyTeamMask) == 0 && num2 > -1)
			{
				if (!global::Networking.Networking.isHost)
				{
					global::Networking.Networking.networkSBytes[0] = 5;
					global::Networking.Networking.networkInts[0] = players[num3].id;
					mainC.networkingMain.XBOX_Send_Network_Message_To_Host(18);
				}
				else
				{
					players[num3].objectivePoints += 5;
					global::Networking.Networking.networkInts[0] = players[num3].id;
					global::Networking.Networking.networkInts[1] = global::MainGame.MainGame.gameData.players[num3].scoresI[0];
					global::Networking.Networking.networkInts[2] = players[num3].objectivePoints;
					mainC.networkingMain.XBOX_Send_Network_Message50(50);
				}
			}
		}
		mainC.soundsMain.Play_Priority_Sound(playerRaces[players[num].race].soundDeath[players[num].type], players[num].charP.position.v[0], players[num].charP.position.v[1], players[num].charP.position.v[2], 0f, 0f, 0f);
		if (players[num].onmap == 4)
		{
			players[num].voiceCueID = mainC.soundsMain.Play_Voice(playerRaces[players[num].race].soundDeath[players[num].type], players[num].charP.position.v[0], players[num].charP.position.v[1], players[num].charP.position.v[2], 0f, 0f, 0f);
			players[num].deathTime = 0f;
			players[num].onmap = 8;
			mainC.programsMain.Start_Animation((ushort)num, ref players[num].jt1, ref players[num].animations, players[num].programCollection, players[num].programDeath, 1f, 1f);
		}
	}

	public bool Player_Injured_Threaded(float velocity)
	{
		ushort num = 0;
		if (!players[num].active || players[num].dead)
		{
			return false;
		}
		float num2 = (velocity - players[num].velocityTerminalThreshold) / players[num].velocityTerminal;
		num2 = num2 * num2 * num2;
		Adjust_Player_Damage(num, players[num].maxDamage * num2, global::MainGame.MainGame.gameMode != 0);
		num2 = players[num].damagePercentageCapped;
		if (global::MainGame.MainGame.gameMode == 1)
		{
			if (global::MainGame.MainGame.commanderMode && global::MainGame.MainGame.Commander[players[0].team] > 0)
			{
				ref HalfSingle reference = ref global::Networking.Networking.networkHS[0];
				reference = new HalfSingle(players[0].damage);
				mainC.networkingMain.XBOX_Send_Network_Message_To_Gamer(16, mainC.networkingMain.XBOX_Get_Gamer_By_Act_ID(players[global::MainGame.MainGame.Commander[players[0].team]].id));
			}
			if (num2 >= 1f)
			{
				global::Networking.Networking.networkInts[0] = global::Networking.Networking.networkSession.LocalGamers[global::MainGame.MainGame.localNetworkGamerID].Id;
				global::Networking.Networking.networkInts[1] = global::Networking.Networking.networkSession.LocalGamers[global::MainGame.MainGame.localNetworkGamerID].Id;
				mainC.networkingMain.XBOX_Send_Network_Message3(3);
				if (global::Networking.Networking.isHost)
				{
					Update_Points_For_Human_Kill_By_Human_And_Send(0, 0);
				}
			}
		}
		else if (global::MainGame.MainGame.gameMode == 0 && num2 >= 1f)
		{
			teamPoints[players[0].team] -= global::MainGame.MainGame.teamPointsForOwnDeath;
			players[0].roundPts -= global::MainGame.MainGame.pointsForOwnDeath;
			global::MainGame.MainGame.gameData.players[0].scoresI[0] -= (short)global::MainGame.MainGame.pointsForOwnDeath;
		}
		_ = 0.5f;
		float num3 = num2;
		if (num3 > 0.5f)
		{
			num3 = 0.5f;
		}
		_ = global::Rendering.Rendering.uBufferID;
		if (num2 >= 1f)
		{
			global::Rendering.Rendering.hitColor[3] = 0.75f;
			return true;
		}
		global::Rendering.Rendering.hitColor[3] = 0.35f + 0.4f * num2;
		mainC.inputMain.GamePad_Vibration_Set_Low(1f);
		return false;
	}

	public bool Player_Vehicle_Damaged(float damage)
	{
		ushort num = 0;
		if (!players[num].active || players[num].dead)
		{
			return false;
		}
		Adjust_Player_Damage(num, damage, sendOnline: true);
		float damagePercentageCapped = players[num].damagePercentageCapped;
		if (global::MainGame.MainGame.gameMode == 1)
		{
			if (global::MainGame.MainGame.commanderMode && global::MainGame.MainGame.Commander[players[0].team] > 0)
			{
				ref HalfSingle reference = ref global::Networking.Networking.networkHS[0];
				reference = new HalfSingle(players[0].damage);
				mainC.networkingMain.XBOX_Send_Network_Message_To_Gamer(16, mainC.networkingMain.XBOX_Get_Gamer_By_Act_ID(players[global::MainGame.MainGame.Commander[players[0].team]].id));
			}
			if (damagePercentageCapped >= 1f)
			{
				global::Networking.Networking.networkInts[0] = global::Networking.Networking.networkSession.LocalGamers[global::MainGame.MainGame.localNetworkGamerID].Id;
				global::Networking.Networking.networkInts[1] = global::Networking.Networking.networkSession.LocalGamers[global::MainGame.MainGame.localNetworkGamerID].Id;
				mainC.networkingMain.XBOX_Send_Network_Message3(3);
				if (global::Networking.Networking.isHost)
				{
					Update_Points_For_Human_Kill_By_Human_And_Send(0, 0);
				}
			}
		}
		else if (global::MainGame.MainGame.gameMode == 0 && damagePercentageCapped >= 1f)
		{
			teamPoints[players[0].team] -= global::MainGame.MainGame.teamPointsForOwnDeath;
			players[0].roundPts -= global::MainGame.MainGame.pointsForOwnDeath;
			global::MainGame.MainGame.gameData.players[0].scoresI[0] -= (short)global::MainGame.MainGame.pointsForOwnDeath;
		}
		if (damagePercentageCapped < 1f)
		{
			mainC.inputMain.GamePad_Vibration_Set_Low(1f);
			return false;
		}
		return true;
	}

	public void Set_Player_Headlamp_For_Level(byte lightID, float intensity)
	{
		switch (global::MainGame.MainGame.gameMode)
		{
		case 0:
			mainC.renderingMain.Set_Point_Light_Color(lightID, intensity, intensity, intensity, 1f);
			break;
		case 1:
		{
			byte gameType = global::MainGame.MainGame.gameType;
			if (gameType == 1)
			{
				if (global::MainGame.MainGame.isGuard)
				{
					mainC.renderingMain.Set_Point_Light_Color(lightID, intensity, intensity, intensity, 1f);
				}
				else
				{
					mainC.renderingMain.Set_Point_Light_Color(lightID, 0f, 0f, 0f, 1f);
				}
			}
			break;
		}
		}
	}

	public void Give_Team_Points(ushort playerID, byte victimTeam, short points)
	{
		if ((players[playerID].teamMask & Get_Team_Mask(victimTeam)) == 0)
		{
			teamPoints[players[playerID].team] += points;
			global::MainGame.MainGame.gameData.players[playerID].scoresI[0] += points;
		}
		else
		{
			teamPoints[players[playerID].team] -= points;
			global::MainGame.MainGame.gameData.players[playerID].scoresI[0] -= points;
		}
	}

	public void Give_Player_Points(ushort playerID, byte victimTeam, short points)
	{
		if ((players[playerID].teamMask & Get_Team_Mask(victimTeam)) == 0)
		{
			global::MainGame.MainGame.gameData.players[playerID].scoresI[0] += points;
		}
		else
		{
			global::MainGame.MainGame.gameData.players[playerID].scoresI[0] -= points;
		}
	}

	public void Set_Player_Out_Of_Map(ushort playerID)
	{
		players[playerID].onmap = 1;
	}

	public void Player_Vehicle_Explodes(ushort playerID, byte threadID)
	{
		switch (Vehicles.vehicles[players[playerID].curVehicle].type)
		{
		case 1:
		case 3:
		case 5:
		case 6:
		case 7:
		{
			ushort race = players[playerID].race;
			mainC.soundsMain.Stop_Continual_Sound(playerID);
			mainC.renderingMain.New_Particle_New(16, global::MainGame.MainGame.playerVehicles[playerID].ph1.x, global::MainGame.MainGame.playerVehicles[playerID].ph1.y, global::MainGame.MainGame.playerVehicles[playerID].ph1.z, 1f, 0f, 0f, playerID, threadID);
			mainC.soundsMain.Play_Priority_Sound("Airplane_Crash", players[playerID].charP.position.v[0], players[playerID].charP.position.v[1], players[playerID].charP.position.v[2], 0f, 0f, 0f);
			ushort numBloodModels = playerRaces[race].numBloodModels;
			for (ushort num = 0; num < numBloodModels; num++)
			{
				mainC.renderingMain.New_Solid_Particle_From_Player_Vehicle_Explosion(playerID, playerRaces[race].bloodModelIDs[num]);
			}
			mainC.vehicles.Splash_Damage_From_Vehicle_Explosion(global::MainGame.MainGame.playerVehicles[playerID].ph1.x, global::MainGame.MainGame.playerVehicles[playerID].ph1.y, global::MainGame.MainGame.playerVehicles[playerID].ph1.z + 10f, players[playerID].curVehicle, (short)playerID, threadID);
			players[playerID].onmap = 1;
			break;
		}
		case 2:
		case 4:
			break;
		}
	}

	public void Receive_Invincible_Timer(int actID)
	{
		short num = Get_Player_Index(actID, -1);
		if (num >= 0)
		{
			players[num].invincibleTimer = global::Networking.Networking.networkHS[0].ToSingle();
			players[num].invincible = true;
		}
	}

	public void Receive_Kill_Streak_Message(int actID)
	{
		short num = Get_Player_Index(actID, -1);
		if (num >= 0)
		{
			killStreak[num] = global::Networking.Networking.networkUShorts[0];
			if (killStreak[num] > maxKillStreak[num])
			{
				maxKillStreak[num] = killStreak[num];
			}
			mainC.fontmoduleMain.Add_Text_To_Onscreen_Text(players[num].abreviateName + " achieved a " + killStreak[num].ToString(CultureInfo.InvariantCulture) + " kill streak.");
		}
	}

	public void Send_Player_Rank()
	{
		global::Networking.Networking.networkBytes[0] = playerRankMP;
		mainC.networkingMain.XBOX_Send_Network_Message31(31);
	}

	public void Receive_Player_Rank(int actID)
	{
		short num = Get_Player_Index(actID, -1);
		if (num >= 0)
		{
			if (remotePlayerRanks[num] > 0 && remotePlayerRanks[num] < global::Networking.Networking.networkBytes[0])
			{
				mainC.fontmoduleMain.Add_Text_To_Onscreen_Text(players[num].abreviateName + " ranked up to rank " + global::Networking.Networking.networkBytes[0].ToString(CultureInfo.InvariantCulture));
			}
			remotePlayerRanks[num] = global::Networking.Networking.networkBytes[0];
		}
	}

	public void Mark_Players_Points_To_Send(ushort playerID)
	{
		if (!mpData[playerID].delayedPointsSend)
		{
			mpData[playerID].delayedPointsSend = true;
			mpData[playerID].delayedPointsTime = 2f;
		}
	}

	public void Player_Vehicle_Exploded_From_Network(int actID, byte threadID)
	{
		short num = Get_Player_Index(actID, -1);
		if (num >= 0)
		{
			Player_Vehicle_Explodes((ushort)num, threadID);
		}
	}

	public void Sync_Network_Session_Players()
	{
		for (ushort num = 0; num < global::MainGame.MainGame.maxHumanGamePlayers; num++)
		{
			int num2;
			if (players[num].id > -1 && (num2 = Get_Player_Index(players[num].id, -1)) < 0)
			{
				Remove_Remote_Player_From_Game((short)num);
			}
		}
		for (ushort num = 0; num < global::Networking.Networking.networkSession.RemoteGamers.Count; num++)
		{
			int num2;
			if (mainC.playersMain.Get_Player_Index(global::Networking.Networking.networkSession.RemoteGamers[num].Id, -1) < 0 && (num2 = Find_Vacant_Player(0)) > -1)
			{
				global::Networking.Networking.networkMsg = global::Networking.Networking.networkSession.RemoteGamers[num].Gamertag;
				mainC.playersMain.Reset_Player((ushort)num2, isActive: true, 0, 0);
				mainC.playersMain.Init_New_Multiplayer_Gamer(global::Networking.Networking.networkMsg, num2, global::Networking.Networking.networkSession.RemoteGamers[num].Id);
				global::Networking.Networking.networkSession.RemoteGamers[num].BeginGetProfile(mainC.networkingMain.Get_Gamer_Profile, global::Networking.Networking.networkSession.RemoteGamers[num].Id);
			}
		}
	}

	public void Update_Points_For_Human_Kill_By_Human_And_Send(int pID, int pID2)
	{
		if (!global::MainGame.MainGame.roundOver && pID >= 0 && pID2 >= 0)
		{
			if (players[pID].team != players[pID2].team)
			{
				teamPoints[players[pID2].team] += global::MainGame.MainGame.teamPointsForEnemyDeath;
				players[pID2].roundPts += global::MainGame.MainGame.pointsForEnemyDeath;
				global::MainGame.MainGame.gameData.players[pID2].scoresI[0] += global::MainGame.MainGame.pointsForEnemyDeath;
			}
			else if (pID == pID2)
			{
				teamPoints[players[pID2].team] -= global::MainGame.MainGame.teamPointsForOwnDeath;
				players[pID2].roundPts -= global::MainGame.MainGame.pointsForOwnDeath;
				global::MainGame.MainGame.gameData.players[pID2].scoresI[0] -= global::MainGame.MainGame.pointsForOwnDeath;
			}
			else
			{
				teamPoints[players[pID2].team] -= global::MainGame.MainGame.teamPointsForTeamKill;
				players[pID2].roundPts -= global::MainGame.MainGame.pointsForTeamKill;
				global::MainGame.MainGame.gameData.players[pID2].scoresI[0] -= global::MainGame.MainGame.pointsForTeamKill;
			}
			global::Networking.Networking.networkInts[0] = players[pID2].id;
			global::Networking.Networking.networkInts[1] = global::MainGame.MainGame.gameData.players[pID2].scoresI[0];
			global::Networking.Networking.networkInts[2] = players[pID2].objectivePoints;
			global::Networking.Networking.networkInts[3] = teamPoints[0];
			global::Networking.Networking.networkInts[4] = teamPoints[1];
			mainC.networkingMain.XBOX_Send_Network_Message4(4);
			if (global::Networking.Networking.isHost && mainC.gameLogic.Game_Is_MP_Round_Over())
			{
				mainC.networkingMain.XBOX_MP_Round_Over();
			}
			mainC.userInterface.Mark_Window_As_Needing_Updating(10);
			mainC.userInterface.Mark_Window_As_Needing_Updating(9);
		}
	}

	public void Send_Player_Team(NetworkGamer sender)
	{
		short num = Get_Player_Index(sender.Id, -1);
		if (num >= 0)
		{
			global::Networking.Networking.networkBytes[0] = (byte)players[num].team;
			mainC.networkingMain.XBOX_Send_Network_Message_To_Gamer(11, sender);
		}
	}

	public void Send_Player_Points(ushort playerID)
	{
		global::Networking.Networking.networkInts[0] = players[playerID].id;
		global::Networking.Networking.networkInts[1] = global::MainGame.MainGame.gameData.players[playerID].scoresI[0];
		global::Networking.Networking.networkInts[2] = players[playerID].objectivePoints;
		mainC.networkingMain.XBOX_Send_Network_Message50(50);
	}

	public void Send_Player_Points_To_Gamer(ushort playerID, NetworkGamer rGamer)
	{
		global::Networking.Networking.networkInts[0] = players[playerID].id;
		global::Networking.Networking.networkInts[1] = global::MainGame.MainGame.gameData.players[playerID].scoresI[0];
		global::Networking.Networking.networkInts[2] = players[playerID].objectivePoints;
		mainC.networkingMain.XBOX_Send_Network_Message_To_Gamer(50, rGamer);
	}

	public void Receive_Player_Points()
	{
		short num = Get_Player_Index(global::Networking.Networking.networkInts[0], -1);
		if (num > -1 && num < global::MainGame.MainGame.maxHumanGamePlayers)
		{
			global::MainGame.MainGame.gameData.players[num].scoresI[0] = global::Networking.Networking.networkInts[1];
			players[num].objectivePoints = global::Networking.Networking.networkInts[2];
			mainC.userInterface.Mark_Window_As_Needing_Updating(10);
			mainC.userInterface.Mark_Window_As_Needing_Updating(9);
		}
	}

	public void Send_Team_Points()
	{
		global::Networking.Networking.networkInts[0] = teamPoints[0];
		global::Networking.Networking.networkInts[1] = teamPoints[1];
		mainC.networkingMain.XBOX_Send_Network_Message51(51);
	}

	public void Receive_Team_Points()
	{
		teamPoints[0] = global::Networking.Networking.networkInts[0];
		teamPoints[1] = global::Networking.Networking.networkInts[1];
		mainC.userInterface.Mark_Window_As_Needing_Updating(10);
		mainC.userInterface.Mark_Window_As_Needing_Updating(9);
	}

	public void XBOX_Send_Update_Of_Team_Points_For_NewPlayer(NetworkGamer newGamer)
	{
		global::Networking.Networking.networkInts[0] = players[0].id;
		global::Networking.Networking.networkInts[1] = global::MainGame.MainGame.gameData.players[0].scoresI[0];
		global::Networking.Networking.networkInts[2] = players[0].objectivePoints;
		global::Networking.Networking.networkInts[3] = teamPoints[0];
		global::Networking.Networking.networkInts[4] = teamPoints[1];
		mainC.networkingMain.XBOX_Send_Network_Message_To_Gamer(4, newGamer);
	}

	public void Send_Update_Of_Player_Points_For_NewPlayer(NetworkGamer newGamer)
	{
		global::Networking.Networking.networkInts[0] = players[0].id;
		global::Networking.Networking.networkInts[1] = global::MainGame.MainGame.gameData.players[0].scoresI[0];
		global::Networking.Networking.networkInts[2] = players[0].objectivePoints;
		mainC.networkingMain.XBOX_Send_Network_Message_To_Gamer(7, newGamer);
	}

	public void XBOX_Send_Update_Of_Player_ObjectivePoints_To_Host(int act, byte pID, sbyte objPts)
	{
		if (!global::Networking.Networking.isHost)
		{
			global::Networking.Networking.networkSBytes[0] = objPts;
			global::Networking.Networking.networkInts[0] = act;
			mainC.networkingMain.XBOX_Send_Network_Message_To_Host(18);
		}
		else
		{
			players[pID].objectivePoints += objPts;
			global::Networking.Networking.networkInts[0] = act;
			global::Networking.Networking.networkInts[1] = global::MainGame.MainGame.gameData.players[pID].scoresI[0];
			global::Networking.Networking.networkInts[2] = players[pID].objectivePoints;
			mainC.networkingMain.XBOX_Send_Network_Message50(50);
		}
	}

	public void Receive_Update_Of_Team_Points()
	{
		teamPoints[0] = global::Networking.Networking.networkInts[3];
		teamPoints[1] = global::Networking.Networking.networkInts[4];
		short num = Get_Player_Index(global::Networking.Networking.networkInts[0], -1);
		if (num > -1)
		{
			players[num].roundPts = global::Networking.Networking.networkInts[1];
			global::MainGame.MainGame.gameData.players[num].scoresI[0] = global::Networking.Networking.networkInts[1];
			players[num].objectivePoints = global::Networking.Networking.networkInts[2];
		}
		mainC.userInterface.Mark_Window_As_Needing_Updating(10);
		mainC.userInterface.Mark_Window_As_Needing_Updating(9);
	}

	public void Send_Player_Info_To_Gamer(NetworkGamer newGamer)
	{
		if (players[0].invincible)
		{
			ref HalfSingle reference = ref global::Networking.Networking.networkHS[0];
			reference = new HalfSingle(players[0].invincibleTimer);
			mainC.networkingMain.XBOX_Send_Network_Message_To_Gamer(63, newGamer);
		}
		Send_Update_Of_Player_Points_For_NewPlayer(newGamer);
		Send_Player_Rank();
		Send_Position_Rotation_Race_Message();
		Send_Player_Points_To_Gamer(0, newGamer);
		Send_Team_Information_To_Gamer(newGamer);
		Send_Local_Player_Array_Position_To_New_Gamer(newGamer);
		mainC.weaponsMain.Send_Player_Weapons(0, newGamer);
		mainC.gameLogic.Game_Send_Player_Status(newGamer);
	}

	public void Send_Team_Information()
	{
		global::Networking.Networking.networkUShorts[0] = players[0].team;
		mainC.networkingMain.XBOX_Send_Network_Message42(42);
	}

	public void Send_Team_Information_To_Gamer(NetworkGamer sender)
	{
		global::Networking.Networking.networkUShorts[0] = players[0].team;
		mainC.networkingMain.XBOX_Send_Network_Message_To_Gamer(42, sender);
	}

	public ushort Make_Player_Status_Bytes_FPS()
	{
		ushort num = 0;
		if (players[0].invincible)
		{
			num = 1;
		}
		else if (global::MainGame.MainGame.sideStepping)
		{
			num = 2;
		}
		else if (global::MainGame.MainGame.walking || global::MainGame.MainGame.walkingBackwards)
		{
			num = 1;
		}
		num <<= 4;
		num |= (ushort)players[0].primaryWeaponMountWeapon;
		num <<= 4;
		num |= players[0].team;
		num <<= 2;
		switch (players[0].onmap)
		{
		case 2:
			num |= 1;
			break;
		case 4:
			num |= 2;
			break;
		case 8:
			num |= 3;
			break;
		}
		num <<= 3;
		if (players[0].shooting)
		{
			num |= 2;
		}
		if (players[0].dead)
		{
			num |= 4;
		}
		return (ushort)(num << 1);
	}

	public byte Make_Player_Status_Bytes_Airplane()
	{
		byte b = global::MainGame.MainGame.gearDown[0];
		b <<= 4;
		b |= players[0].race;
		b <<= 2;
		switch (players[0].onmap)
		{
		case 2:
			b |= 1;
			break;
		case 4:
			b |= 2;
			break;
		case 8:
			b |= 3;
			break;
		}
		b <<= 1;
		if (players[0].dead)
		{
			b |= 1;
		}
		return b;
	}

	public void Send_Player_Location()
	{
		if (global::MainGame.MainGame.frameC1 <= 0 || !global::Networking.Networking.inGame)
		{
			return;
		}
		global::MainGame.MainGame.frameC1 = 0;
		if (nextRemoteGamer >= global::Networking.Networking.networkSession.RemoteGamers.Count)
		{
			global::Weapons.Weapons.mpSendWeaponFiredMsg = false;
			nextRemoteGamer = 0;
		}
		if (nextRemoteGamer < global::Networking.Networking.networkSession.RemoteGamers.Count)
		{
			short num = Get_Player_Index(global::Networking.Networking.networkSession.RemoteGamers[nextRemoteGamer].Id, -1);
			if (num > -1)
			{
				Send_Player_Special_Message(num, 0, global::Networking.Networking.networkSession.RemoteGamers[nextRemoteGamer]);
			}
			Send_Position_Message(global::Networking.Networking.networkSession.RemoteGamers[nextRemoteGamer++], sendToGamer: true);
		}
	}

	public void Send_Player_Special_Message(int receivingPlayerID, ushort localPlayerID, NetworkGamer sender)
	{
		ushort num = 1;
		ushort num2 = 16;
		ushort num3 = 0;
		while (mpData[receivingPlayerID].specialData > 0 && num3++ < num2)
		{
			switch ((ushort)(mpData[receivingPlayerID].specialData & num))
			{
			case 1:
			{
				ref HalfSingle reference = ref global::Networking.Networking.networkHS[0];
				reference = new HalfSingle(global::MainGame.MainGame.gameData.players[0].scoresF[0]);
				mainC.networkingMain.XBOX_Send_Network_Message_To_Gamer(34, sender);
				break;
			}
			case 2:
				mainC.playersMain.Send_Player_Points_To_Gamer(0, sender);
				break;
			case 4:
				mainC.weaponsMain.Send_Player_Weapons(localPlayerID, sender);
				break;
			case 8:
				mainC.gameLogic.Game_Send_Player_Status(sender);
				break;
			case 16:
				global::Networking.Networking.networkBytes[0] = global::MainGame.MainGame.laps[0];
				mainC.networkingMain.XBOX_Send_Network_Message_To_Gamer(40, sender);
				break;
			case 32:
				mainC.networkingMain.XBOX_Send_Network_Message_To_Gamer(41, sender);
				break;
			case 64:
				Send_Local_Player_Damage();
				break;
			case 128:
				mainC.aiMain.Send_KillCount(sender);
				break;
			}
			mpData[receivingPlayerID].specialData = (ushort)(mpData[receivingPlayerID].specialData & ~num);
			num <<= 1;
		}
		mpData[receivingPlayerID].specialData = 0;
	}

	public void Send_Player_Info_For_Lobby()
	{
		if (global::MainGame.MainGame.frameC1 <= 0 || !global::Networking.Networking.inGame)
		{
			return;
		}
		if (nextRemoteGamer >= global::Networking.Networking.networkSession.RemoteGamers.Count)
		{
			nextRemoteGamer = 0;
		}
		if (nextRemoteGamer < global::Networking.Networking.networkSession.RemoteGamers.Count)
		{
			global::MainGame.MainGame.frameC1 = 0;
			if (global::Networking.Networking.isHost)
			{
				global::Networking.Networking.networkFloats[0] = global::MainGame.MainGame.lobbyTimer;
				global::Networking.Networking.networkFloats[1] = global::MainGame.MainGame.lobbyMapVoteTimer;
				mainC.networkingMain.XBOX_Send_Network_Message_To_Gamer(13, global::Networking.Networking.networkSession.RemoteGamers[nextRemoteGamer]);
			}
			nextRemoteGamer++;
		}
	}

	public void Send_Position_Message(NetworkGamer sender, bool sendToGamer)
	{
		switch (global::Networking.Networking.posMessageType)
		{
		case 0:
		{
			ref HalfSingle reference16 = ref global::Networking.Networking.networkHS[0];
			reference16 = new HalfSingle(players[0].charP.position.v[0]);
			ref HalfSingle reference17 = ref global::Networking.Networking.networkHS[1];
			reference17 = new HalfSingle(players[0].charP.position.v[1]);
			ref HalfSingle reference18 = ref global::Networking.Networking.networkHS[2];
			reference18 = new HalfSingle(players[0].charP.position.v[2]);
			ref HalfSingle reference19 = ref global::Networking.Networking.networkHS[3];
			reference19 = new HalfSingle(players[0].charP.velocity.v[0]);
			ref HalfSingle reference20 = ref global::Networking.Networking.networkHS[4];
			reference20 = new HalfSingle(players[0].charP.velocity.v[1]);
			ref HalfSingle reference21 = ref global::Networking.Networking.networkHS[5];
			reference21 = new HalfSingle(players[0].charP.velocity.v[2]);
			ref HalfSingle reference22 = ref global::Networking.Networking.networkHS[6];
			reference22 = new HalfSingle(xRotation);
			ref HalfSingle reference23 = ref global::Networking.Networking.networkHS[7];
			reference23 = new HalfSingle(zRotation);
			if (sendToGamer)
			{
				mainC.networkingMain.XBOX_Send_Network_Message_To_Gamer(0, sender);
			}
			else
			{
				mainC.networkingMain.XBOX_Send_Network_Message0(0);
			}
			break;
		}
		case 1:
		{
			byte uBufferID = global::Rendering.Rendering.uBufferID;
			ref HalfSingle reference = ref global::Networking.Networking.networkHS[0];
			reference = new HalfSingle(players[0].mv[uBufferID].M11);
			ref HalfSingle reference2 = ref global::Networking.Networking.networkHS[1];
			reference2 = new HalfSingle(players[0].mv[uBufferID].M12);
			ref HalfSingle reference3 = ref global::Networking.Networking.networkHS[2];
			reference3 = new HalfSingle(players[0].mv[uBufferID].M13);
			ref HalfSingle reference4 = ref global::Networking.Networking.networkHS[3];
			reference4 = new HalfSingle(players[0].mv[uBufferID].M21);
			ref HalfSingle reference5 = ref global::Networking.Networking.networkHS[4];
			reference5 = new HalfSingle(players[0].mv[uBufferID].M22);
			ref HalfSingle reference6 = ref global::Networking.Networking.networkHS[5];
			reference6 = new HalfSingle(players[0].mv[uBufferID].M23);
			ref HalfSingle reference7 = ref global::Networking.Networking.networkHS[6];
			reference7 = new HalfSingle(players[0].mv[uBufferID].M31);
			ref HalfSingle reference8 = ref global::Networking.Networking.networkHS[7];
			reference8 = new HalfSingle(players[0].mv[uBufferID].M32);
			ref HalfSingle reference9 = ref global::Networking.Networking.networkHS[8];
			reference9 = new HalfSingle(players[0].mv[uBufferID].M33);
			ref HalfSingle reference10 = ref global::Networking.Networking.networkHS[9];
			reference10 = new HalfSingle(players[0].charP.position.v[0]);
			ref HalfSingle reference11 = ref global::Networking.Networking.networkHS[10];
			reference11 = new HalfSingle(players[0].charP.position.v[1]);
			ref HalfSingle reference12 = ref global::Networking.Networking.networkHS[11];
			reference12 = new HalfSingle(players[0].charP.position.v[2]);
			ref HalfSingle reference13 = ref global::Networking.Networking.networkHS[12];
			reference13 = new HalfSingle(players[0].charP.velocity.v[0]);
			ref HalfSingle reference14 = ref global::Networking.Networking.networkHS[13];
			reference14 = new HalfSingle(players[0].charP.velocity.v[1]);
			ref HalfSingle reference15 = ref global::Networking.Networking.networkHS[14];
			reference15 = new HalfSingle(players[0].charP.velocity.v[2]);
			global::Networking.Networking.networkBytes[0] = (byte)(global::MainGame.MainGame.playerVehicles[0].throttleSpeed * 255f);
			if (sendToGamer)
			{
				mainC.networkingMain.XBOX_Send_Network_Message_To_Gamer(29, sender);
			}
			else
			{
				mainC.networkingMain.XBOX_aSend_Network_Message(29);
			}
			break;
		}
		}
	}

	public void Send_Local_Player_Damage()
	{
		float damagePercentageCapped = players[0].damagePercentageCapped;
		global::Networking.Networking.networkBytes[0] = (byte)(255f * damagePercentageCapped);
		mainC.networkingMain.XBOX_Send_Network_Message45(45);
	}

	public void Send_Player_Shooting_Message(byte curStub, bool shooting)
	{
		global::Networking.Networking.networkBytes[0] = curStub;
		global::Networking.Networking.networkBools[0] = shooting;
		mainC.networkingMain.XBOX_Send_Network_Message44(44);
	}

	public void Send_Position_Rotation_Race_Message()
	{
		byte uBufferID = global::Rendering.Rendering.uBufferID;
		ref HalfSingle reference = ref global::Networking.Networking.networkHS[0];
		reference = new HalfSingle(players[0].mv[uBufferID].M11);
		ref HalfSingle reference2 = ref global::Networking.Networking.networkHS[1];
		reference2 = new HalfSingle(players[0].mv[uBufferID].M12);
		ref HalfSingle reference3 = ref global::Networking.Networking.networkHS[2];
		reference3 = new HalfSingle(players[0].mv[uBufferID].M13);
		ref HalfSingle reference4 = ref global::Networking.Networking.networkHS[3];
		reference4 = new HalfSingle(players[0].mv[uBufferID].M21);
		ref HalfSingle reference5 = ref global::Networking.Networking.networkHS[4];
		reference5 = new HalfSingle(players[0].mv[uBufferID].M22);
		ref HalfSingle reference6 = ref global::Networking.Networking.networkHS[5];
		reference6 = new HalfSingle(players[0].mv[uBufferID].M23);
		ref HalfSingle reference7 = ref global::Networking.Networking.networkHS[6];
		reference7 = new HalfSingle(players[0].mv[uBufferID].M31);
		ref HalfSingle reference8 = ref global::Networking.Networking.networkHS[7];
		reference8 = new HalfSingle(players[0].mv[uBufferID].M32);
		ref HalfSingle reference9 = ref global::Networking.Networking.networkHS[8];
		reference9 = new HalfSingle(players[0].mv[uBufferID].M33);
		ref HalfSingle reference10 = ref global::Networking.Networking.networkHS[9];
		reference10 = new HalfSingle(players[0].charP.position.v[0]);
		ref HalfSingle reference11 = ref global::Networking.Networking.networkHS[10];
		reference11 = new HalfSingle(players[0].charP.position.v[1]);
		ref HalfSingle reference12 = ref global::Networking.Networking.networkHS[11];
		reference12 = new HalfSingle(players[0].charP.position.v[2]);
		ref HalfSingle reference13 = ref global::Networking.Networking.networkHS[12];
		reference13 = new HalfSingle(players[0].charP.velocity.v[0]);
		ref HalfSingle reference14 = ref global::Networking.Networking.networkHS[13];
		reference14 = new HalfSingle(players[0].charP.velocity.v[1]);
		ref HalfSingle reference15 = ref global::Networking.Networking.networkHS[14];
		reference15 = new HalfSingle(players[0].charP.velocity.v[2]);
		global::Networking.Networking.networkBytes[0] = (byte)Math.Round((xRotation + 90f) / 180f * 255f);
		global::Networking.Networking.networkBytes[1] = (byte)Math.Round(zRotation / 360f * 255f);
		global::Networking.Networking.networkBytes[2] = players[0].race;
		global::Networking.Networking.networkBytes[3] = (byte)players[0].type;
		mainC.networkingMain.XBOX_Send_Network_Message5(5);
	}

	public void Update_Player_Damage_Amount(int actID)
	{
		short num = Get_Player_Index(actID, -1);
		if (num >= 0)
		{
			Adjust_Player_Damage_To_Fixed_Amount((ushort)num, (float)(int)global::Networking.Networking.networkBytes[0] / 255f * players[num].maxDamage, sendOnline: false);
		}
	}

	public void Update_Player_Position_From_Network(int actID)
	{
		short num = Get_Player_Index(actID, -1);
		if (num < 0)
		{
			Sync_Network_Session_Players();
			return;
		}
		global::Networking.Networking.networkPlayers[num].haveRemotePlayerPosition = true;
		mpData[num].currentPosX = global::Networking.Networking.networkHS[0].ToSingle();
		mpData[num].currentPosY = global::Networking.Networking.networkHS[1].ToSingle();
		mpData[num].currentPosZ = global::Networking.Networking.networkHS[2].ToSingle();
		mpData[num].velX = global::Networking.Networking.networkHS[3].ToSingle();
		mpData[num].velY = global::Networking.Networking.networkHS[4].ToSingle();
		mpData[num].velZ = global::Networking.Networking.networkHS[5].ToSingle();
		mpData[num].xRotation = global::Networking.Networking.networkHS[6].ToSingle();
		mpData[num].zRotation = global::Networking.Networking.networkHS[7].ToSingle();
		players[num].xRotation = mpData[num].xRotation;
		players[num].zRotation = mpData[num].zRotation;
		if (!mpData[num].dataThisRound)
		{
			mpData[num].timeFromLastUpdate = (float)(global::MainGame.MainGame.mainTime - mpData[num].lastUpdate) * 1E-07f;
			mpData[num].lastUpdate = global::MainGame.MainGame.mainTime;
			mpData[num].dataThisRound = true;
		}
		float timeFromLastUpdate = mpData[num].timeFromLastUpdate;
		mpData[num].rotVelX = (players[num].xRotation - players[num].xRotation) / timeFromLastUpdate;
		mpData[num].rotVelZ = (players[num].zRotation - players[num].zRotation) / timeFromLastUpdate;
	}

	public void Update_Player_Matrix_From_Network(int actID)
	{
		short num = Get_Player_Index(actID, -1);
		if (num >= 0)
		{
			global::Networking.Networking.networkPlayers[num].haveRemotePlayerPosition = true;
			mpData[num].mv.M11 = global::Networking.Networking.networkHS[0].ToSingle();
			mpData[num].mv.M12 = global::Networking.Networking.networkHS[1].ToSingle();
			mpData[num].mv.M13 = global::Networking.Networking.networkHS[2].ToSingle();
			mpData[num].mv.M21 = global::Networking.Networking.networkHS[3].ToSingle();
			mpData[num].mv.M22 = global::Networking.Networking.networkHS[4].ToSingle();
			mpData[num].mv.M23 = global::Networking.Networking.networkHS[5].ToSingle();
			mpData[num].mv.M31 = global::Networking.Networking.networkHS[6].ToSingle();
			mpData[num].mv.M32 = global::Networking.Networking.networkHS[7].ToSingle();
			mpData[num].mv.M33 = global::Networking.Networking.networkHS[8].ToSingle();
			mpData[num].currentPosX = global::Networking.Networking.networkHS[9].ToSingle();
			mpData[num].currentPosY = global::Networking.Networking.networkHS[10].ToSingle();
			mpData[num].currentPosZ = global::Networking.Networking.networkHS[11].ToSingle();
			mpData[num].velX = global::Networking.Networking.networkHS[12].ToSingle();
			mpData[num].velY = global::Networking.Networking.networkHS[13].ToSingle();
			mpData[num].velZ = global::Networking.Networking.networkHS[14].ToSingle();
			global::MainGame.MainGame.playerVehicles[num].throttleSpeed = (float)(int)global::Networking.Networking.networkBytes[0] / 255f;
			if (!mpData[num].dataThisRound)
			{
				mpData[num].timeFromLastUpdate = (float)(global::MainGame.MainGame.mainTime - mpData[num].lastUpdate) * 1E-07f;
				mpData[num].lastUpdate = global::MainGame.MainGame.mainTime;
				mpData[num].dataThisRound = true;
			}
		}
	}

	public void Update_Player_Status_From_Network_FPS(int actID)
	{
		short num = Get_Player_Index(actID, -1);
		if (num < 0)
		{
			return;
		}
		global::Networking.Networking.networkPlayers[num].haveRemotePlayerStatus = true;
		sbyte primaryWeaponMountWeapon = players[num].primaryWeaponMountWeapon;
		_ = players[num].team;
		uint num2 = global::Networking.Networking.networkUShorts[0];
		num2 >>= 1;
		if ((num2 & 2) != 0)
		{
			players[num].shooting = true;
			players[num].shotOnce |= 2;
		}
		else
		{
			if ((players[num].shotOnce & 2) == 0)
			{
				players[num].shooting = false;
			}
			players[num].shotOnce |= 4;
		}
		if ((num2 & 4) != 0)
		{
			players[num].dead = true;
		}
		else
		{
			players[num].dead = false;
		}
		num2 >>= 3;
		switch (num2 & 3)
		{
		case 0u:
			players[num].onmap = 1;
			break;
		case 1u:
			players[num].onmap = 2;
			break;
		case 2u:
			players[num].onmap = 4;
			break;
		case 3u:
			players[num].onmap = 8;
			break;
		}
		num2 >>= 2;
		players[num].team = (ushort)(num2 & 0xF);
		players[num].teamMask = mainC.playersMain.Get_Team_Mask(players[num].team);
		num2 >>= 4;
		players[num].primaryWeaponMountWeapon = (sbyte)(num2 & 0xF);
		num2 >>= 4;
		switch (num2 & 3)
		{
		case 0u:
			players[num].playerIsMoving = 1;
			break;
		case 1u:
			players[num].playerIsMoving = 2;
			break;
		case 2u:
			players[num].playerIsMoving = 4;
			break;
		case 3u:
			players[num].playerIsMoving = 8;
			break;
		}
		if (primaryWeaponMountWeapon != players[num].primaryWeaponMountWeapon)
		{
			players[num].wpnIndex = (sbyte)mainC.weaponsMain.Get_Weapon_Index((ushort)num, (byte)players[num].primaryWeaponMountWeapon);
			mainC.weaponsMain.Update_Player_Weapon_Info((byte)num);
		}
	}

	public void Update_Player_Status_From_Network_Airplane(int actID, short playerID)
	{
		if (playerID < 0)
		{
			playerID = Get_Player_Index(actID, -1);
		}
		if (playerID < 0)
		{
			return;
		}
		global::Networking.Networking.networkPlayers[playerID].haveRemotePlayerStatus = true;
		byte race = players[playerID].race;
		byte b = global::Networking.Networking.networkBytes[0];
		players[playerID].dead = false;
		if ((b & 1) > 0)
		{
			players[playerID].dead = true;
		}
		b >>= 1;
		switch ((byte)(b & 3))
		{
		case 0:
			players[playerID].onmap = 1;
			break;
		case 1:
			players[playerID].onmap = 2;
			break;
		case 2:
			players[playerID].onmap = 4;
			break;
		case 3:
			players[playerID].onmap = 8;
			break;
		}
		b >>= 2;
		race = (byte)(b & 0xF);
		b >>= 4;
		if (race != players[playerID].race)
		{
			Set_Player_Race((byte)playerID, race, players[playerID].type);
		}
		byte b2 = playerRaces[players[playerID].race].programTurnLeft[players[playerID].type];
		if (b2 < byte.MaxValue)
		{
			players[playerID].pg1[b2].status = 1;
			if ((b & 1) == 1)
			{
				mainC.programsMain.Start_Animation((ushort)playerID, ref players[playerID].jt1, ref players[playerID].animations, players[playerID].programCollection, b2, 1f, 1f);
			}
			else
			{
				mainC.programsMain.Start_Animation((ushort)playerID, ref players[playerID].jt1, ref players[playerID].animations, players[playerID].programCollection, b2, 1f, 1f);
			}
			global::Joints.Joints.Reset_Joint_Data(b2);
			global::Joints.Joints.Translate_Player_Joint_Vertex_Non_Particle(playerID);
		}
	}

	public void Update_Player_Shooting_Status(int actID)
	{
		short num = Get_Player_Index(actID, -1);
		if (num < 0)
		{
			return;
		}
		bool flag = global::Networking.Networking.networkBools[0];
		if (!flag)
		{
			players[num].shotOnce |= 4;
			if ((players[num].shotOnce & 2) == 2)
			{
				flag = true;
			}
		}
		else
		{
			players[num].shotOnce |= 2;
		}
		players[num].shooting = flag;
		if (global::Networking.Networking.networkBytes[0] < global::MainGame.MainGame.playerVehicles[num].numWeapons)
		{
			global::MainGame.MainGame.playerVehicles[num].weapons[global::Networking.Networking.networkBytes[0]].shooting = flag;
		}
	}

	public void Request_Remote_Payer_Status(ushort remotePlayerID)
	{
		int num = Get_RemoteGamer_Index((byte)players[remotePlayerID].id, -1);
		if (num >= 0)
		{
			mainC.networkingMain.XBOX_Send_Network_Message_To_Gamer(59, global::Networking.Networking.networkSession.RemoteGamers[num]);
		}
	}
}
