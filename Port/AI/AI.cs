using System;
using System.Globalization;
using System.IO;
using EGEngine;
using InputHandler;
using Joints;
using MainGame;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using Microsoft.Xna.Framework.Net;
using Networking;
using Physics;
using Players;
using Programs;
using Rendering;
using Structs;
using Util;
using Weapons;
using WindowsGame1;

namespace AI;

public class AI
{
	public static bool disableAI = false;

	public static float aiCheckTimer;

	public static float roundOverTimer;

	public static float startX;

	public static float startY;

	public static float startZ;

	public static float routeX;

	public static float routeY;

	public static float routeZ;

	public static float lastNetworkSend;

	public static float aiRandomFactor = 0.1f;

	public static float killCountScale = 1f;

	public static byte[] status;

	public static int numPtsX;

	public static int numPtsY;

	public static int numSqrs;

	public static int[] sqrX;

	public static int[] sqrY;

	public static int[] sqrW;

	public static int[] sqrH;

	public static int[] boxArea;

	public static int[] boxPtr;

	public static ushort[] boxID;

	public static ushort aiToCheck;

	public static ushort numAllocatedNavRoutes;

	public static ushort numAiRespawnedInWave;

	public static ushort numAiInWave;

	public static ushort currentAiWave;

	public static ushort numTotalAiSpawned;

	public static bool hostNeedsToResetAiMP;

	public static bool aiCompleted;

	public static bool endLevelOnLastAI = true;

	public static bool sendAIRoute = false;

	public static bool resetAIOnMainPlayerDeath = true;

	public static bool bossLevelComplete;

	public static bool finishRoute;

	public static bool routingInProcess = false;

	public static bool bossLevel = false;

	public static float aiSpeakingTime;

	public static float aiSpeakingTimeRandom;

	public static float roundOverWaitTime = 5f;

	public static float maxAIRoutingDistanceSqr;

	public static float currentAiRespawnTime = 100f;

	public static float levelRespawnTimer;

	public static float aiRespawnTimerStartValue;

	public static float aiRespawnTimer = 0f;

	public static float debugAccuracy = 10f;

	public static float playerPosX;

	public static float playerPosY;

	public static float[] waveRespawnTime;

	public static byte aiTargetingMode = 0;

	public static byte lastEnemyAiCount;

	public static byte numAiToRespawn = 1;

	public static byte numAiLeftToRespawn = 1;

	public static byte numAllocatedAI = 0;

	public static byte numAI = 0;

	public static byte curProcAI;

	public static byte currentAI;

	public static byte currentActiveEnemyAI;

	public static byte numBosses;

	public static byte numAllocatedBosses;

	public static byte curNetworkAI;

	public static ushort hostAuthorizedSpawn;

	public static ushort aiRouteToSend;

	public static ushort maxLevelSimultaneousAI = 10;

	public static ushort levelKillCount;

	public static ushort numAiWaves;

	public static ushort aiSpawnPoint = 0;

	public static ushort numAiKillsForLevelToEnd;

	public static ushort curRemotePlayer;

	public static ushort[] waveRespawnCount;

	public static int rtPtX;

	public static int rtPtIndex;

	public static StructsClass.aiEntity[] ais;

	public static StructsClass.AI_Boss[] bosses;

	public static StructsClass.vtex aiVec1 = new StructsClass.vtex();

	public static StructsClass.vtex aiVec2 = new StructsClass.vtex();

	public static StructsClass.vtex aiVec3 = new StructsClass.vtex();

	public static StructsClass.vtex aiVec4 = new StructsClass.vtex();

	public static StructsClass.particle_list viewBox;

	public static string aiName = "Bot ";

	public static Matrix mvPA = default(Matrix);

	public static StructsClass.Multiplayer_Data_AI[] mpData;

	public static HalfSingle findRoute_hx;

	public static HalfSingle findRoute_hy;

	public static HalfSingle findRoute_hz;

	public Game1.MasterCollection mainC;

	public void Init_Vars(Game1.MasterCollection master)
	{
		mainC = master;
	}

	public void Initialize_AI()
	{
		_ = global::Rendering.Rendering.uBufferID;
		viewBox = default(StructsClass.particle_list);
		StructsClass.Initialize_ParticleList(ref viewBox);
		viewBox.v1 = new StructsClass.vtex[1];
		viewBox.v1[0] = new StructsClass.vtex();
		viewBox.v1[0].v[0] = 0f;
		viewBox.v1[0].v[1] = 0f;
		viewBox.v1[0].v[2] = 0f;
		viewBox.numP = 1L;
		viewBox.numUsed = 1L;
		status = new byte[4004001];
		sqrX = new int[500];
		sqrY = new int[500];
		sqrW = new int[500];
		sqrH = new int[500];
		boxArea = new int[500];
		boxPtr = new int[500];
		boxID = new ushort[500];
		aiSpeakingTime = 6f;
		aiSpeakingTimeRandom = 2f;
		mpData = new StructsClass.Multiplayer_Data_AI[44];
		for (ushort num = 0; num < 44; num++)
		{
			mpData[num] = new StructsClass.Multiplayer_Data_AI();
		}
	}

	public void Load_AI_Data(string fileName)
	{
		int num = -1;
		_ = global::Rendering.Rendering.uBufferID;
		resetAIOnMainPlayerDeath = true;
		numAiWaves = 0;
		currentAI = 0;
		numAllocatedNavRoutes = 0;
		for (int i = 0; i < numAllocatedAI; i++)
		{
			ais[i].aiType = 0;
			ais[i].numChildrenAI = 0;
			ais[i].hostID = global::Util.Util.maxUnsignedShortValue;
			ais[i].playerID = -1;
			ais[i].textureID = -1;
			ais[i].checkForEnemy = true;
			ais[i].textureNormalID = -1;
			ais[i].textureSpecularID = -1;
			ais[i].status = 0;
			ais[i].raceType = 0;
			ais[i].weapon = 0;
			ais[i].team = 1;
			ais[i].patrolling = 0;
			ais[i].enemyTeam = 0;
			ais[i].maxDamage = 10f;
			ais[i].damage = 0f;
			ais[i].onmap = 0;
			ais[i].jointPackage = 0;
			ais[i].programPackage = 0;
			ais[i].stationary = false;
			ais[i].bossID = byte.MaxValue;
			ais[i].optimalTargetDistance = 10f;
			ais[i].optimalTargetDistanceSqr = 100f;
			ais[i].maxDistanceToHearShotsSqr = 1600000f;
			ais[i].maxDistanceToSeeShotTeammateSqr = 160000f;
			ais[i].hostMatrix = Matrix.Identity;
			ais[i].leadsTarget = false;
			ais[i].needsRoute = false;
			ais[i].updateAIRoute = false;
			ais[i].resetSpeed = false;
			ais[i].aiRoute.numPts = 0;
			ais[i].targetMode = aiTargetingMode;
			ais[i].maxTargetAngle = 5f;
			ais[i].maxTargetMoveDistanceSqr = 10f;
		}
		aiRespawnTimer = -1f;
		levelRespawnTimer = -1f;
		numAiKillsForLevelToEnd = 0;
		numAiToRespawn = 1;
		numAiLeftToRespawn = 1;
		numAI = 0;
		aiRespawnTimerStartValue = -1f;
		maxLevelSimultaneousAI = 10;
		Stream stream = TitleContainer.OpenStream("The_CoOp_Zombie_Game\\Config_Files\\" + fileName);
		byte[] array = new byte[stream.Length];
		if (stream.CanRead)
		{
			stream.Read(array, 0, array.Length);
			string text = mainC.utilMain.Byte_Array_To_String(array);
			string[] array2 = text.Split('\n', '\r');
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
				stream.Close();
				return;
			}
			string[] array3 = new string[num2];
			j = 0;
			num2 = 0;
			for (; j < array2.Length; j++)
			{
				if (array2[j].Length > 0)
				{
					array3[num2++] = array2[j];
				}
			}
			for (j = 0; j < num2; j++)
			{
				array2 = array3[j].Split(' ', '\t');
				int k = 0;
				int num3 = 0;
				for (; k < array2.Length; k++)
				{
					if (array2[k].Length > 0)
					{
						num3++;
					}
				}
				if (num3 < 1)
				{
					continue;
				}
				string[] array4 = new string[num3];
				k = 0;
				num3 = 0;
				for (; k < array2.Length; k++)
				{
					if (array2[k].Length > 0)
					{
						array4[num3++] = array2[k];
					}
				}
				int num4 = 0;
				if (array4[0].Equals("numAI", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 1;
				}
				else if (array4[0].Equals("AI", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 2;
				}
				else if (array4[0].Equals("type", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 3;
				}
				else if (array4[0].Equals("position", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 4;
				}
				else if (array4[0].Equals("BossID", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 5;
				}
				else if (array4[0].Equals("weapon", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 6;
				}
				else if (array4[0].Equals("team", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 7;
				}
				else if (array4[0].Equals("damage", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 8;
				}
				else if (array4[0].Equals("maxDamage", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 9;
				}
				else if (array4[0].Equals("onmap", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 10;
				}
				else if (array4[0].Equals("jointPackage", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 11;
				}
				else if (array4[0].Equals("weaponJoint", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 12;
				}
				else if (array4[0].Equals("weaponJointRotate", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 13;
				}
				else if (array4[0].Equals("weaponJointElevate", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 14;
				}
				else if (array4[0].Equals("firingTime", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 15;
				}
				else if (array4[0].Equals("damageType", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 16;
				}
				else if (array4[0].Equals("timeBetweenFiring", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 17;
				}
				else if (array4[0].Equals("rotation", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 18;
				}
				else if (array4[0].Equals("fov", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 19;
				}
				else if (array4[0].Equals("weaponArmElevateDirection", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 20;
				}
				else if (array4[0].Equals("waves", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 21;
				}
				else if (array4[0].Equals("clone", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 22;
				}
				else if (array4[0].Equals("maxLevelSimultaneousAI", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 23;
				}
				else if (array4[0].Equals("xRotation", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 24;
				}
				else if (array4[0].Equals("maxTargetDistance", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 25;
				}
				else if (array4[0].Equals("speed", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 26;
				}
				else if (array4[0].Equals("hoverSpeed", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 27;
				}
				else if (array4[0].Equals("rotationSpeedX", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 28;
				}
				else if (array4[0].Equals("rotationSpeedZ", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 29;
				}
				else if (array4[0].Equals("CheckForEnemy", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 30;
				}
				else if (array4[0].Equals("programPackage", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 31;
				}
				else if (array4[0].Equals("terminalVelocity", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 32;
				}
				else if (array4[0].Equals("velocityDamageThreshold", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 33;
				}
				else if (array4[0].Equals("ResetAiOnMainPlayerDeath", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 34;
				}
				else if (array4[0].Equals("aiType", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 35;
				}
				else if (array4[0].Equals("host", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 36;
				}
				else if (array4[0].Equals("hostAttachMatrix", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 37;
				}
				else if (array4[0].Equals("leadsTarget", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 38;
				}
				else if (array4[0].Equals("Race", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 39;
				}
				else if (array4[0].Equals("Stationary", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 40;
				}
				else if (array4[0].Equals("respawnTime", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 41;
				}
				else if (array4[0].Equals("deathTimer", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 42;
				}
				else if (array4[0].Equals("respawnCount", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 43;
				}
				else if (array4[0].Equals("hearShotDistanceSqr", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 44;
				}
				else if (array4[0].Equals("seeShotTeammateDistanceSqr", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 45;
				}
				else if (array4[0].Equals("texture", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 46;
				}
				else if (array4[0].Equals("textureNormal", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 47;
				}
				else if (array4[0].Equals("targetAquisitionDistance", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 48;
				}
				else if (array4[0].Equals("defaultTargetMode", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 49;
				}
				else if (array4[0].Equals("levelKillCount", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 50;
				}
				else if (array4[0].Equals("initialRespawnTimer", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 51;
				}
				else if (array4[0].Equals("targetMode", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 52;
				}
				else if (array4[0].Equals("maxTargetAngle", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 53;
				}
				else if (array4[0].Equals("roundOverTimeLimit", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 54;
				}
				else if (array4[0].Equals("textureSpecular", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 55;
				}
				else if (array4[0].Equals("maxTargetMoveDistance", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 56;
				}
				switch (num4)
				{
				case 1:
				{
					if (array4.Length <= 1)
					{
						break;
					}
					int num6 = int.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
					if (global::MainGame.MainGame.gameMode == 0)
					{
						global::MainGame.MainGame.maxGamePlayers = (byte)(num6 + 1);
					}
					else
					{
						global::MainGame.MainGame.maxGamePlayers = (byte)(num6 + 4);
					}
					if (global::MainGame.MainGame.maxGamePlayers > 44)
					{
						global::MainGame.MainGame.maxGamePlayers = 44;
					}
					if (num6 > numAllocatedAI)
					{
						ais = new StructsClass.aiEntity[num6];
						for (int i = 0; i < num6; i++)
						{
							ais[i] = new StructsClass.aiEntity();
							ais[i].aiType = 0;
							ais[i].numChildrenAI = 0;
							ais[i].hostID = global::Util.Util.maxUnsignedShortValue;
							ais[i].playerID = -1;
							ais[i].textureID = -1;
							ais[i].textureNormalID = -1;
							ais[i].textureSpecularID = -1;
							ais[i].checkForEnemy = true;
							ais[i].status = 0;
							ais[i].raceType = 0;
							ais[i].weapon = 0;
							ais[i].team = 1;
							ais[i].patrolling = 0;
							ais[i].enemyTeam = 0;
							ais[i].maxDamage = 10f;
							ais[i].damage = 0f;
							ais[i].onmap = 0;
							ais[i].jointPackage = 0;
							ais[i].programPackage = 0;
							ais[i].optimalTargetDistance = 10f;
							ais[i].optimalTargetDistanceSqr = 100f;
							ais[i].stationary = false;
							ais[i].bossID = byte.MaxValue;
							ais[i].maxDistanceToHearShotsSqr = 1600000f;
							ais[i].maxDistanceToSeeShotTeammateSqr = 160000f;
							ais[i].hostMatrix = Matrix.Identity;
							ais[i].leadsTarget = false;
							ais[i].maxTargetAngle = 5f;
							ais[i].maxTargetMoveDistanceSqr = 10f;
							ais[i].aiRoute = default(StructsClass.Route);
							ais[i].aiRoute.numPts = 0;
							ais[i].aiRoute.curPt = 0;
							ais[i].targetMode = aiTargetingMode;
						}
						numAllocatedAI = (byte)num6;
					}
					numAI = (byte)num6;
					currentAI = numAI;
					break;
				}
				case 2:
					if (array4.Length > 1)
					{
						num = int.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
						if (num > -1 && num < numAllocatedAI)
						{
							ais[num].playerID = -1;
							ais[num].textureID = -1;
							ais[num].textureNormalID = -1;
							ais[num].textureSpecularID = -1;
							ais[num].status = 0;
							ais[num].raceType = 0;
							ais[num].weapon = 0;
							ais[num].team = 1;
							ais[num].enemyTeam = 0;
							ais[num].maxDamage = 10f;
							ais[num].damage = 0f;
							ais[num].onmap = 0;
							ais[num].jointPackage = 0;
							ais[num].programPackage = 0;
							ais[num].optimalTargetDistance = 10f;
							ais[num].optimalTargetDistanceSqr = 100f;
							ais[num].maxDistanceToHearShotsSqr = 1600000f;
							ais[num].maxDistanceToSeeShotTeammateSqr = 160000f;
							ais[num].stationary = false;
						}
						else
						{
							num = -1;
						}
					}
					break;
				case 3:
					if (array4.Length > 1 && num > -1)
					{
						ais[num].raceType = byte.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 4:
					if (array4.Length > 3 && num > -1)
					{
						ais[num].x = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
						ais[num].y = float.Parse(array4[2], CultureInfo.InvariantCulture.NumberFormat);
						ais[num].z = float.Parse(array4[3], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 5:
					if (array4.Length > 1 && num > -1)
					{
						ais[num].bossID = byte.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
						numBosses++;
					}
					break;
				case 6:
					if (array4.Length > 1 && num > -1)
					{
						ais[num].weapon = sbyte.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 7:
					if (array4.Length > 1 && num > -1)
					{
						ais[num].team = byte.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
						ais[num].enemyTeam = 0;
						if (ais[num].team == 0)
						{
							ais[num].enemyTeam = 1;
						}
					}
					break;
				case 8:
					if (array4.Length > 1 && num > -1)
					{
						ais[num].damage = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 9:
					if (array4.Length > 1 && num > -1)
					{
						ais[num].maxDamage = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 10:
					if (array4.Length > 1 && num > -1)
					{
						ais[num].onmap = byte.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 11:
					if (array4.Length > 1 && num > -1)
					{
						ais[num].jointPackage = short.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 12:
					if (array4.Length > 1 && num > -1)
					{
						ais[num].weaponJoint = sbyte.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 13:
					if (array4.Length > 1 && num > -1)
					{
						ais[num].weaponJointR = sbyte.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 14:
					if (array4.Length > 1 && num > -1)
					{
						ais[num].weaponJointE = sbyte.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 15:
					if (array4.Length > 2 && num > -1)
					{
						ais[num].firingTime = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
						ais[num].firingTimeAdj = float.Parse(array4[2], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 16:
					if (array4.Length > 1 && num > -1)
					{
						ais[num].damageType = byte.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 17:
					if (array4.Length > 1 && num > -1)
					{
						ais[num].timeBetweenFiring = 0f - float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 18:
					if (array4.Length > 1 && num > -1)
					{
						ais[num].zRotation = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 19:
					if (array4.Length > 2 && num > -1)
					{
						ais[num].fov = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
						ais[num].cosFov = float.Parse(array4[2], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 20:
					if (array4.Length > 1 && num > -1)
					{
						ais[num].weaponElevationDir = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 21:
				{
					if (array4.Length <= 1)
					{
						break;
					}
					ushort num7 = ushort.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
					if (array4.Length > 2 * num7 + 1)
					{
						waveRespawnCount = new ushort[num7];
						waveRespawnTime = new float[num7];
						int i = 0;
						int num6 = 2;
						for (; i < num7; i++)
						{
							waveRespawnCount[i] = ushort.Parse(array4[num6++], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
							waveRespawnTime[i] = (int)ushort.Parse(array4[num6++], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
						}
						numAiWaves = num7;
					}
					break;
				}
				case 22:
				{
					if (array4.Length <= 2 || num <= -1)
					{
						break;
					}
					int i = ushort.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
					int num5 = array4.Length;
					for (int l = 2; l < num5; l++)
					{
						int num6 = ushort.Parse(array4[l], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
						ais[num6].raceType = ais[i].raceType;
						ais[num6].x = ais[i].x;
						ais[num6].y = ais[i].y;
						ais[num6].z = ais[i].z;
						ais[num6].bossID = ais[i].bossID;
						if (ais[num6].bossID != byte.MaxValue)
						{
							numBosses++;
						}
						ais[num6].weapon = ais[i].weapon;
						ais[num6].team = ais[i].team;
						ais[num6].enemyTeam = ais[i].enemyTeam;
						ais[num6].damage = ais[i].damage;
						ais[num6].maxDamage = ais[i].maxDamage;
						ais[num6].onmap = ais[i].onmap;
						ais[num6].jointPackage = ais[i].jointPackage;
						ais[num6].weaponJoint = ais[i].weaponJoint;
						ais[num6].weaponJointR = ais[i].weaponJointR;
						ais[num6].weaponJointE = ais[i].weaponJointE;
						ais[num6].firingTime = ais[i].firingTime;
						ais[num6].firingTimeAdj = ais[i].firingTimeAdj;
						ais[num6].damageType = ais[i].damageType;
						ais[num6].timeBetweenFiring = ais[i].timeBetweenFiring;
						ais[num6].zRotation = ais[i].zRotation;
						ais[num6].fov = ais[i].fov;
						ais[num6].cosFov = ais[i].cosFov;
						ais[num6].weaponElevationDir = ais[i].weaponElevationDir;
						ais[num6].xRotation = ais[i].xRotation;
						ais[num6].optimalTargetDistance = ais[i].optimalTargetDistance;
						ais[num6].optimalTargetDistanceSqr = ais[i].optimalTargetDistanceSqr;
						ais[num6].maxTargetMoveDistanceSqr = ais[i].maxTargetMoveDistanceSqr;
						ais[num6].speed = ais[i].speed;
						ais[num6].speedHover = ais[i].speedHover;
						ais[num6].speedRotationX = ais[i].speedRotationX;
						ais[num6].speedRotationZ = ais[i].speedRotationZ;
						ais[num6].programPackage = ais[i].programPackage;
						ais[num6].velocityTerminal = ais[i].velocityTerminal;
						ais[num6].velocityTerminalThreshold = ais[i].velocityTerminalThreshold;
						ais[num6].race = ais[i].race;
						ais[num6].stationary = ais[i].stationary;
						ais[num6].maxDistanceToHearShotsSqr = ais[i].maxDistanceToHearShotsSqr;
						ais[num6].maxDistanceToSeeShotTeammateSqr = ais[i].maxDistanceToSeeShotTeammateSqr;
						ais[num6].textureID = ais[i].textureID;
						ais[num6].textureNormalID = ais[i].textureNormalID;
						ais[num6].textureSpecularID = ais[i].textureSpecularID;
						ais[num6].targetCanBeSeenDistance = ais[i].targetCanBeSeenDistance;
						ais[num6].deathTimer = ais[i].deathTimer;
						ais[num6].targetMode = ais[i].targetMode;
						ais[num6].maxTargetAngle = ais[i].maxTargetAngle;
						num = num6;
					}
					break;
				}
				case 23:
					if (array4.Length > 1)
					{
						maxLevelSimultaneousAI = ushort.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 24:
					if (array4.Length > 1 && num > -1)
					{
						ais[num].xRotation = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 25:
					if (array4.Length > 2 && num > -1)
					{
						ais[num].optimalTargetDistance = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
						ais[num].optimalTargetDistanceSqr = float.Parse(array4[2], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 26:
					if (array4.Length > 1 && num > -1)
					{
						ais[num].speed = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
						if (ais[num].speed == 0f)
						{
							ais[num].speed = 0.01f;
						}
					}
					break;
				case 27:
					if (array4.Length > 1 && num > -1)
					{
						ais[num].speedHover = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 28:
					if (array4.Length > 1 && num > -1)
					{
						ais[num].speedRotationX = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 29:
					if (array4.Length > 1 && num > -1)
					{
						ais[num].speedRotationZ = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 30:
					if (array4.Length > 1 && num > -1)
					{
						int num5 = byte.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
						ais[num].checkForEnemy = false;
						if (num5 == 1)
						{
							ais[num].checkForEnemy = true;
						}
					}
					break;
				case 31:
					if (array4.Length > 1 && num > -1)
					{
						ais[num].programPackage = short.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 32:
					if (array4.Length > 1 && num > -1)
					{
						ais[num].velocityTerminal = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 33:
					if (array4.Length > 1 && num > -1)
					{
						ais[num].velocityTerminalThreshold = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 34:
					if (array4.Length > 1)
					{
						resetAIOnMainPlayerDeath = byte.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat) == 1;
					}
					break;
				case 35:
					if (array4.Length > 1 && num > -1)
					{
						ais[num].aiType = byte.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 36:
					if (array4.Length > 1 && num > -1)
					{
						ais[num].hostID = ushort.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 37:
					if (array4.Length > 3 && num > -1)
					{
						ais[num].hostMatrix = Matrix.CreateRotationY(float.Parse(array4[2], CultureInfo.InvariantCulture.NumberFormat) * ((float)Math.PI / 180f)) * Matrix.CreateRotationX(float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat) * ((float)Math.PI / 180f)) * Matrix.CreateRotationZ(float.Parse(array4[3], CultureInfo.InvariantCulture.NumberFormat) * ((float)Math.PI / 180f));
					}
					break;
				case 38:
					if (num > -1)
					{
						ais[num].leadsTarget = true;
					}
					break;
				case 39:
					if (array4.Length > 1 && num > -1)
					{
						ais[num].race = byte.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 40:
					if (num > -1)
					{
						ais[num].stationary = true;
					}
					break;
				case 41:
					if (array4.Length > 1)
					{
						levelRespawnTimer = (currentAiRespawnTime = (aiRespawnTimer = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat)));
					}
					break;
				case 42:
					if (array4.Length > 1 && num > -1)
					{
						ais[num].deathTimer = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 43:
					if (array4.Length > 1)
					{
						numAiToRespawn = byte.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
						numAiLeftToRespawn = numAiToRespawn;
					}
					break;
				case 44:
					if (array4.Length > 1 && num > -1)
					{
						ais[num].maxDistanceToHearShotsSqr = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 45:
					if (array4.Length > 1 && num > -1)
					{
						ais[num].maxDistanceToSeeShotTeammateSqr = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 46:
					if (array4.Length > 1 && num > -1)
					{
						ais[num].textureID = mainC.texturesMain.Find_Texture(array4[1], -1);
					}
					break;
				case 47:
					if (array4.Length > 1 && num > -1)
					{
						ais[num].textureNormalID = mainC.texturesMain.Find_Texture(array4[1], -1);
					}
					break;
				case 48:
					if (array4.Length > 1 && num > -1)
					{
						ais[num].targetCanBeSeenDistance = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 49:
					if (array4.Length > 1)
					{
						aiTargetingMode = byte.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 50:
					if (array4.Length > 1)
					{
						numAiKillsForLevelToEnd = ushort.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 51:
					if (array4.Length > 1)
					{
						aiRespawnTimerStartValue = (int)ushort.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 52:
					if (array4.Length > 1 && num > -1)
					{
						ais[num].targetMode = byte.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 53:
					if (array4.Length > 1 && num > -1)
					{
						ais[num].maxTargetAngle = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 54:
					if (array4.Length > 1)
					{
						roundOverWaitTime = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 55:
					if (array4.Length > 1 && num > -1)
					{
						ais[num].textureSpecularID = mainC.texturesMain.Find_Texture(array4[1], -1);
					}
					break;
				case 56:
					if (array4.Length > 1 && num > -1)
					{
						ais[num].maxTargetMoveDistanceSqr = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
						ais[num].maxTargetMoveDistanceSqr *= ais[num].maxTargetMoveDistanceSqr;
					}
					break;
				}
			}
		}
		stream.Close();
		for (int i = 0; i < numAI; i++)
		{
			ais[i].controllingPlayer = -1;
			ais[i].authorizedToRespawn = false;
			ais[i].locallyControlled = false;
			if (global::MainGame.MainGame.gameMode == 0 || (global::MainGame.MainGame.gameMode == 1 && global::Networking.Networking.isHost))
			{
				ais[i].controllingPlayer = global::Players.Players.players[0].id;
				ais[i].locallyControlled = true;
			}
			int l = 0;
			int num6;
			for (num6 = 0; num6 < numAI; num6++)
			{
				if (ais[num6].hostID == i && i != num6)
				{
					l++;
				}
			}
			if (l <= 0)
			{
				continue;
			}
			ais[i].numChildrenAI = (byte)l;
			ais[i].childrenAI = new ushort[l];
			num6 = 0;
			l = 0;
			for (; num6 < numAI; num6++)
			{
				if (ais[num6].hostID == i && i != num6)
				{
					ais[i].childrenAI[l++] = (ushort)num6;
				}
			}
		}
		Assign_AI();
	}

	public void Load_Boss_Data(string fileName, byte threadID)
	{
		int num = -1;
		_ = global::Rendering.Rendering.uBufferID;
		for (int i = 0; i < numAllocatedBosses; i++)
		{
			bosses[i].aiID = 0;
			bosses[i].curPosition = 0;
			bosses[i].curWeapon = 0;
			bosses[i].numPositions = 0;
			bosses[i].numWeapons = 0;
			bosses[i].positionTime = 0f;
			bosses[i].weaponTime = 0f;
		}
		numBosses = 0;
		bossLevel = true;
		if (numAI < 1)
		{
			return;
		}
		Stream stream = TitleContainer.OpenStream("The_CoOp_Zombie_Game\\Config_Files\\" + fileName);
		byte[] array = new byte[stream.Length];
		if (stream.CanRead)
		{
			stream.Read(array, 0, array.Length);
			string text = mainC.utilMain.Byte_Array_To_String(array);
			string[] array2 = text.Split('\n', '\r');
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
				stream.Close();
				return;
			}
			string[] array3 = new string[num2];
			j = 0;
			num2 = 0;
			for (; j < array2.Length; j++)
			{
				if (array2[j].Length > 0)
				{
					array3[num2++] = array2[j];
				}
			}
			for (j = 0; j < num2; j++)
			{
				array2 = array3[j].Split(' ', '\t');
				int k = 0;
				int num3 = 0;
				for (; k < array2.Length; k++)
				{
					if (array2[k].Length > 0)
					{
						num3++;
					}
				}
				if (num3 < 1)
				{
					continue;
				}
				string[] array4 = new string[num3];
				k = 0;
				num3 = 0;
				for (; k < array2.Length; k++)
				{
					if (array2[k].Length > 0)
					{
						array4[num3++] = array2[k];
					}
				}
				int num4 = 0;
				if (array4[0].Equals("numBosses", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 1;
				}
				else if (array4[0].Equals("Boss", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 2;
				}
				else if (array4[0].Equals("numWeapons", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 3;
				}
				else if (array4[0].Equals("curWeapon", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 4;
				}
				else if (array4[0].Equals("numPositions", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 5;
				}
				else if (array4[0].Equals("curPosition", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 6;
				}
				else if (array4[0].Equals("weaponIDs", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 7;
				}
				else if (array4[0].Equals("weaponTimers", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 8;
				}
				else if (array4[0].Equals("positions", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 9;
				}
				else if (array4[0].Equals("positionTimers", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 10;
				}
				else if (array4[0].Equals("accuracy", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 11;
				}
				switch (num4)
				{
				case 1:
				{
					if (array4.Length <= 1)
					{
						break;
					}
					int num5 = int.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
					if (num5 > numAllocatedBosses)
					{
						bosses = new StructsClass.AI_Boss[num5];
						for (int i = 0; i < num5; i++)
						{
							bosses[i] = default(StructsClass.AI_Boss);
							bosses[i].aiID = 0;
							bosses[i].curPosition = 0;
							bosses[i].curWeapon = 0;
							bosses[i].numPositions = 0;
							bosses[i].numWeapons = 0;
							bosses[i].positionTime = 0f;
							bosses[i].weaponTime = 0f;
							bosses[i].numAllocatedPositions = 0;
							bosses[i].numAllocatedWeapons = 0;
						}
						numAllocatedBosses = (byte)num5;
					}
					numBosses = (byte)num5;
					break;
				}
				case 2:
					if (array4.Length > 1)
					{
						num = int.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
						if (num > -1 && num < numAllocatedBosses)
						{
							bosses[num].aiID = 0;
							bosses[num].curPosition = 0;
							bosses[num].curWeapon = 0;
							bosses[num].numPositions = 0;
							bosses[num].numWeapons = 0;
							bosses[num].positionTime = 0f;
							bosses[num].weaponTime = 0f;
						}
						else
						{
							num = -1;
						}
					}
					break;
				case 3:
					if (array4.Length <= 1 || num <= -1)
					{
						break;
					}
					bosses[num].numWeapons = byte.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
					if (bosses[num].numWeapons > bosses[num].numAllocatedWeapons)
					{
						bosses[num].weaponIDs = new byte[bosses[num].numWeapons];
						bosses[num].weaponTimers = new float[bosses[num].numWeapons];
						bosses[num].accuracy = new float[bosses[num].numWeapons];
						for (int i = 0; i < bosses[num].numAllocatedWeapons; i++)
						{
							bosses[num].weaponIDs[i] = 0;
							bosses[num].weaponTimers[i] = 0f;
							bosses[num].accuracy[i] = 1f;
						}
					}
					break;
				case 4:
					if (array4.Length > 1 && num > -1)
					{
						bosses[num].curWeapon = byte.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 5:
					if (array4.Length <= 1 || num <= -1)
					{
						break;
					}
					bosses[num].numPositions = byte.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
					if (bosses[num].numPositions > bosses[num].numAllocatedPositions)
					{
						bosses[num].positionX = new float[bosses[num].numPositions];
						bosses[num].positionY = new float[bosses[num].numPositions];
						bosses[num].positionZ = new float[bosses[num].numPositions];
						bosses[num].positionTimers = new float[bosses[num].numPositions];
						for (int i = 0; i < bosses[num].numAllocatedPositions; i++)
						{
							bosses[num].positionX[i] = 0f;
							bosses[num].positionY[i] = 0f;
							bosses[num].positionZ[i] = 0f;
							bosses[num].positionTimers[i] = 0f;
						}
						bosses[num].numAllocatedPositions = bosses[num].numPositions;
					}
					break;
				case 6:
					if (array4.Length > 1 && num > -1)
					{
						bosses[num].curPosition = byte.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 7:
					if (array4.Length > bosses[num].numWeapons && num > -1)
					{
						int i = 0;
						int num5 = 1;
						for (; i < bosses[num].numWeapons; i++)
						{
							bosses[num].weaponIDs[i] = byte.Parse(array4[num5++], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
						}
					}
					break;
				case 8:
					if (array4.Length > bosses[num].numWeapons && num > -1)
					{
						int i = 0;
						int num5 = 1;
						for (; i < bosses[num].numWeapons; i++)
						{
							bosses[num].weaponTimers[i] = float.Parse(array4[num5++], CultureInfo.InvariantCulture.NumberFormat);
						}
					}
					break;
				case 9:
					if (array4.Length > bosses[num].numPositions * 3 && num > -1)
					{
						int i = 0;
						int num5 = 1;
						for (; i < bosses[num].numPositions; i++)
						{
							bosses[num].positionX[i] = float.Parse(array4[num5++], CultureInfo.InvariantCulture.NumberFormat);
							bosses[num].positionY[i] = float.Parse(array4[num5++], CultureInfo.InvariantCulture.NumberFormat);
							bosses[num].positionZ[i] = float.Parse(array4[num5++], CultureInfo.InvariantCulture.NumberFormat);
						}
					}
					break;
				case 10:
					if (array4.Length > bosses[num].numPositions && num > -1)
					{
						int i = 0;
						int num5 = 1;
						for (; i < bosses[num].numPositions; i++)
						{
							bosses[num].positionTimers[i] = float.Parse(array4[num5++], CultureInfo.InvariantCulture.NumberFormat);
						}
					}
					break;
				case 11:
					if (array4.Length > bosses[num].numWeapons && num > -1)
					{
						int i = 0;
						int num5 = 1;
						for (; i < bosses[num].numWeapons; i++)
						{
							bosses[num].accuracy[i] = float.Parse(array4[num5++], CultureInfo.InvariantCulture.NumberFormat);
						}
					}
					break;
				}
			}
		}
		stream.Close();
		if (num < numBosses - 1)
		{
			numBosses = (byte)(num + 1);
		}
		for (int i = 0; i < numBosses; i++)
		{
			bosses[i].aiID = 0;
			for (int num5 = 0; num5 < numAI; num5++)
			{
				if (ais[num5].bossID == (byte)i)
				{
					bosses[i].aiID = (byte)num5;
					break;
				}
			}
		}
	}

	public void Process_AI_SP(float frameTime, byte threadID)
	{
		_ = global::Rendering.Rendering.uBufferID;
		currentActiveEnemyAI = 0;
		if (currentAI < 1)
		{
			return;
		}
		currentAiRespawnTime -= frameTime;
		if (currentAiRespawnTime < 0f)
		{
			currentAiRespawnTime = -1f;
		}
		for (short num = 0; num < currentAI; num++)
		{
			short playerID = ais[num].playerID;
			if (playerID > -1)
			{
				switch (Vehicles.vehicles[global::Players.Players.players[playerID].curVehicle].type)
				{
				case 0:
				case 8:
					Process_AI_Vehicle_Humanoid(playerID, num, frameTime, threadID);
					break;
				case 1:
				case 6:
				case 7:
					Process_AI_Vehicle_Airplane(playerID, num, frameTime, threadID);
					break;
				case 4:
					Process_AI_Vehicle_UnmannedTurret(playerID, num, frameTime, threadID);
					break;
				case 5:
					Process_AI_Vehicle_SpaceShip(playerID, num, frameTime, threadID);
					break;
				}
				mainC.vehicles.Update_Vehicle_Matrix((ushort)playerID);
			}
		}
		Separate_AI_Players();
		lastEnemyAiCount = currentActiveEnemyAI;
		if (numAiWaves > 0 && numAiInWave > 0 && numAiRespawnedInWave >= numAiInWave && currentActiveEnemyAI < 1 && currentAiWave + 1 < numAiWaves)
		{
			ushort nextWave = (ushort)(currentAiWave + 1);
			Trigger_AI_Wave(nextWave);
			currentAiRespawnTime = aiRespawnTimer;
			Console.WriteLine($"Starting AI wave {nextWave + 1}/{numAiWaves}: count={numAiInWave}, respawnInterval={aiRespawnTimer:0.###}s.");
		}
		if (numAiKillsForLevelToEnd > 0 && ((float)(int)levelKillCount >= (float)(int)numAiKillsForLevelToEnd * killCountScale || (currentActiveEnemyAI < 1 && (numAiWaves < 1 || currentAiWave >= numAiWaves - 1))))
		{
			roundOverTimer += frameTime;
			if (roundOverTimer > roundOverWaitTime && !global::MainGame.MainGame.roundOver)
			{
				if (endLevelOnLastAI)
				{
					mainC.maingameMain.Set_SP_Level_To_Completed();
				}
				else
				{
					aiCompleted = true;
				}
			}
		}
		else
		{
			roundOverTimer = 0f;
		}
	}

	public void Process_AI_MP(float frameTime, byte threadID)
	{
		_ = global::Rendering.Rendering.uBufferID;
		currentActiveEnemyAI = 0;
		if (currentAI < 1)
		{
			return;
		}
		currentAiRespawnTime -= frameTime;
		if (currentAiRespawnTime < 0f)
		{
			currentAiRespawnTime = -1f;
		}
		if (global::Networking.Networking.isHost && !global::Networking.Networking.wasHost)
		{
			Update_AI_Controlling_Players();
		}
		for (short num = 0; num < currentAI; num++)
		{
			short playerID = ais[num].playerID;
			if (playerID > -1)
			{
				byte type = Vehicles.vehicles[global::Players.Players.players[playerID].curVehicle].type;
				if (type == 0 || type == 8)
				{
					if (ais[num].locallyControlled)
					{
						Process_AI_Vehicle_Humanoid(playerID, num, frameTime, threadID);
					}
					else
					{
						Process_AI_Vehicle_Humanoid_Remote_Player(playerID, num, frameTime, threadID);
					}
				}
				mainC.vehicles.Update_Vehicle_Matrix((ushort)playerID);
			}
		}
		Separate_AI_Players();
		lastEnemyAiCount = currentActiveEnemyAI;
		if (global::Networking.Networking.isHost)
		{
			if (numAiKillsForLevelToEnd > 0 && ((float)(int)levelKillCount >= (float)(int)numAiKillsForLevelToEnd * killCountScale || (currentActiveEnemyAI < 1 && (numAiWaves < 1 || currentAiWave >= numAiWaves - 1))))
			{
				roundOverTimer += frameTime;
				if (roundOverTimer > roundOverWaitTime && !global::MainGame.MainGame.roundOver)
				{
					if (endLevelOnLastAI)
					{
						mainC.networkingMain.XBOX_MP_Round_Over();
					}
					else
					{
						aiCompleted = true;
						Send_AI_Completed();
					}
				}
			}
			else
			{
				roundOverTimer = 0f;
			}
		}
		mainC.aiMain.Send_AI_Players(frameTime);
	}

	public void Process_AI_MP_Lobby(float frameTime, byte threadID)
	{
		_ = global::Rendering.Rendering.uBufferID;
		currentActiveEnemyAI = 0;
		if (currentAI < 1)
		{
			return;
		}
		currentAiRespawnTime -= frameTime;
		if (currentAiRespawnTime < 0f)
		{
			currentAiRespawnTime = -1f;
		}
		if (global::Networking.Networking.isHost && !global::Networking.Networking.wasHost)
		{
			Update_AI_Controlling_Players();
		}
		for (short num = 0; num < currentAI; num++)
		{
			short playerID = ais[num].playerID;
			if (playerID > -1)
			{
				byte type = Vehicles.vehicles[global::Players.Players.players[playerID].curVehicle].type;
				if ((type == 0 || type == 8) && ais[num].locallyControlled)
				{
					Process_AI_Vehicle_Humanoid(playerID, num, frameTime, threadID);
				}
				mainC.vehicles.Update_Vehicle_Matrix((ushort)playerID);
			}
		}
		Separate_AI_Players();
		lastEnemyAiCount = currentActiveEnemyAI;
		Send_AI_Players(frameTime);
	}

	public void Process_AI_Vehicle_Humanoid(short pID, short aiID, float frameTime, byte threadID)
	{
		byte uBufferID = global::Rendering.Rendering.uBufferID;
		byte b = 0;
		int num = 0;
		float num2 = 0f;
		float num3 = 0f;
		switch (global::Players.Players.players[pID].onmap)
		{
		case 1:
			if ((global::MainGame.MainGame.gameMode != 1 || (global::Networking.Networking.isHost && !aiCompleted) || hostAuthorizedSpawn > 0) && lastEnemyAiCount < maxLevelSimultaneousAI && global::Players.Players.players[pID].dead && (numAiKillsForLevelToEnd < 1 || (float)(int)numTotalAiSpawned < (float)(int)numAiKillsForLevelToEnd * killCountScale) && (numAiInWave < 1 || numAiRespawnedInWave < numAiInWave) && currentAiRespawnTime < 0f && aiRespawnTimer != -1f && ais[aiID].bossID == byte.MaxValue)
			{
				if (hostAuthorizedSpawn > 0)
				{
					hostAuthorizedSpawn--;
				}
				numTotalAiSpawned++;
				numAiRespawnedInWave++;
				lastEnemyAiCount++;
				if (--numAiLeftToRespawn < 1)
				{
					currentAiRespawnTime = aiRespawnTimer;
					numAiLeftToRespawn = numAiToRespawn;
				}
				mainC.mapsMain.Get_AI_Spawn_Point(ref global::Players.Players.players[pID].charP.position, ais[aiID].team, ref global::Players.Players.players[pID].zRotation, global::MainGame.MainGame.maxLocalPlayerSpawnPoint, ais[aiID].checkForEnemy, global::Players.Players.playerRaces[global::Players.Players.players[pID].race].spawnHeight[global::Players.Players.players[pID].type]);
				mainC.playersMain.Player_Respawn_AI((ushort)pID, threadID);
				Reset_AI_Before_Respawn(aiID);
			}
			break;
		case 2:
			global::Players.Players.players[pID].timeBeforeRespawn[uBufferID] -= frameTime;
			if (global::Players.Players.players[pID].timeBeforeRespawn[uBufferID] < 0f)
			{
				mainC.playersMain.Player_Spawn_Time_Over((ushort)pID);
			}
			global::Joints.Joints.Sync_Player_Matrices(pID, global::Rendering.Rendering.rBufferID, uBufferID);
			break;
		case 4:
		{
			if (!ais[aiID].active)
			{
				break;
			}
			ais[aiID].canFire = false;
			if (global::Players.Players.players[pID].speakingTimer <= 0f)
			{
				ais[aiID].speakingTime -= frameTime;
				if (ais[aiID].speakingTime < 0f)
				{
					ais[aiID].speakingTime = aiSpeakingTime + aiSpeakingTimeRandom * (float)global::MainGame.MainGame.mainRandom.NextDouble();
					global::Players.Players.players[pID].voiceCueID = mainC.soundsMain.Play_Voice(global::Players.Players.playerRaces[ais[aiID].race].soundMain[ais[aiID].raceType], global::Players.Players.players[pID].charP.position.v[0], global::Players.Players.players[pID].charP.position.v[1], global::Players.Players.players[pID].charP.position.v[2], 0f, 0f, 0f);
					global::Players.Players.players[pID].speakingTimer = global::Players.Players.playerRaces[ais[aiID].race].mainSoundTimerLength[ais[aiID].raceType];
				}
			}
			float num4 = 0f;
			ais[aiID].velocityVariation += frameTime * (float)ais[aiID].velocityVariationDirection;
			if (Math.Abs(ais[aiID].velocityVariation) > 0.1f)
			{
				ais[aiID].velocityVariation -= ais[aiID].velocityVariation - 0.1f * (float)Math.Sign(ais[aiID].velocityVariation);
				ais[aiID].velocityVariationDirection *= -1;
			}
			byte objectID = global::MainGame.MainGame.playerVehicles[pID].mounts[global::MainGame.MainGame.primaryWeaponMount].objectID;
			if (ais[aiID].targetID > -1 && ais[aiID].targetVisible)
			{
				num = ais[aiID].targetID;
				if (!ais[aiID].targetInRange)
				{
					aiVec2.v[0] = ais[aiID].goalX;
					aiVec2.v[1] = ais[aiID].goalY;
				}
				else
				{
					aiVec2.v[0] = global::Players.Players.players[num].charP.position.v[0];
					aiVec2.v[1] = global::Players.Players.players[num].charP.position.v[1];
				}
				_ = global::Players.Players.players[pID].weapon1.jointRID;
				aiVec1.v[0] = aiVec2.v[0] - global::Players.Players.players[pID].charP.position.v[0];
				aiVec1.v[1] = aiVec2.v[1] - global::Players.Players.players[pID].charP.position.v[1];
				float num5 = (float)Math.Sqrt(aiVec1.v[0] * aiVec1.v[0] + aiVec1.v[1] * aiVec1.v[1]);
				if (num5 != 0f)
				{
					num2 = aiVec1.v[1] / num5;
				}
				num3 = (float)Math.Acos(num2) * 57.29578f;
				if (aiVec1.v[0] > 0f)
				{
					num3 = 360f - num3;
				}
				num4 = global::Players.Players.players[pID].zRotation - num3;
				num3 = num4;
				if (num4 > 180f)
				{
					num4 = 0f - (360f - num4);
				}
				else if (num4 < -180f)
				{
					num4 = 360f + num4;
				}
				float num6 = Math.Abs(num4) / 90f;
				if (num6 > 1f)
				{
					num6 = 1f;
				}
				num2 = ais[aiID].speedRotationZ * frameTime / global::Physics.Physics.timeMod;
				num2 = num2 * 0.2f + num2 * 0.8f * num6;
				if (num4 > 0f)
				{
					if (num4 > num2)
					{
						num4 = num2;
					}
				}
				else if (num4 < 0f - num2)
				{
					num4 = 0f - num2;
				}
				if (num4 > 3f)
				{
					global::Players.Players.players[pID].zRotation -= num4;
				}
				else if (num4 < 3f)
				{
					num4 *= 0.3f;
					global::Players.Players.players[pID].zRotation -= num4;
				}
				if (global::Players.Players.players[pID].zRotation >= 360f)
				{
					global::Players.Players.players[pID].zRotation -= 360f;
				}
				else if (global::Players.Players.players[pID].zRotation < 0f)
				{
					global::Players.Players.players[pID].zRotation += 360f;
				}
				ref Matrix reference = ref global::Players.Players.players[pID].mv[uBufferID];
				reference = Matrix.CreateRotationZ(global::Players.Players.players[pID].zRotation * ((float)Math.PI / 180f));
				global::Joints.Joints.Process_Joints_Threaded(pID, frameTime, threadID);
				ais[aiID].fireTimeRemaining -= frameTime / global::Physics.Physics.timeMod;
				while (num3 > 180f)
				{
					num3 -= 360f;
				}
				for (; num3 < -180f; num3 += 360f)
				{
				}
				if (ais[aiID].targetInRange && Math.Abs(num3) < ais[aiID].maxTargetAngle)
				{
					ais[aiID].canFire = true;
					if (ais[aiID].fireTimeRemaining <= 0f)
					{
						global::Players.Players.players[pID].shooting = false;
						global::MainGame.MainGame.playerVehicles[pID].weapons[objectID].shooting = false;
						if (ais[aiID].fireTimeRemaining < ais[aiID].timeBetweenFiringAdjusted)
						{
							ais[aiID].fireTimeRemaining = ais[aiID].firingTimeAdjusted + ais[aiID].firingTimeAdj * (float)global::MainGame.MainGame.mainRandom.NextDouble() * ais[aiID].firingTimeAdjusted;
						}
						mainC.weaponsMain.firingStopped((ushort)pID, objectID);
					}
					else
					{
						global::Players.Players.players[pID].shooting = true;
						global::MainGame.MainGame.playerVehicles[pID].weapons[objectID].shooting = true;
					}
				}
				else
				{
					if (ais[aiID].fireTimeRemaining <= 0f)
					{
						ais[aiID].fireTimeRemaining = ais[aiID].firingTimeAdjusted + ais[aiID].firingTimeAdj * (float)global::MainGame.MainGame.mainRandom.NextDouble() * ais[aiID].firingTimeAdjusted;
					}
					global::Players.Players.players[pID].shooting = false;
					global::MainGame.MainGame.playerVehicles[pID].weapons[objectID].shooting = false;
					mainC.weaponsMain.firingStopped((ushort)pID, objectID);
				}
			}
			else
			{
				ais[aiID].fireTimeRemaining = ais[aiID].timeBetweenFiringAdjusted;
				aiVec2.v[0] = ais[aiID].goalX;
				aiVec2.v[1] = ais[aiID].goalY;
				aiVec2.v[2] = ais[aiID].goalZ;
				aiVec1.v[0] = aiVec2.v[0] - global::Players.Players.players[pID].charP.position.v[0];
				aiVec1.v[1] = aiVec2.v[1] - global::Players.Players.players[pID].charP.position.v[1];
				aiVec1.v[2] = aiVec2.v[2] - global::Players.Players.players[pID].charP.position.v[2];
				float num5 = (float)Math.Sqrt(aiVec1.v[0] * aiVec1.v[0] + aiVec1.v[1] * aiVec1.v[1]);
				if (num5 > 10f)
				{
					aiVec1.v[1] /= num5;
					num3 = (float)Math.Acos(aiVec1.v[1]) * 57.29578f;
					if (aiVec1.v[0] > 0f)
					{
						num3 = 360f - num3;
					}
					num4 = global::MainGame.MainGame.playerVehicles[pID].weapons[global::MainGame.MainGame.playerVehicles[pID].mounts[global::MainGame.MainGame.primaryWeaponMount].objectID].offset[0, 6].x * 57.29578f - num3;
					num3 = num4;
					if (num4 > 180f)
					{
						num4 = 0f - (360f - num4);
					}
					else if (num4 < -180f)
					{
						num4 = 360f + num4;
					}
					float num6 = Math.Abs(num4) / 90f;
					if (num6 > 1f)
					{
						num6 = 1f;
					}
					num2 = ais[aiID].speedRotationZ * frameTime / global::Physics.Physics.timeMod;
					num2 = num2 * 0.2f + num2 * 0.8f * num6;
					if (num4 > 0f)
					{
						if (num4 > num2)
						{
							num4 = num2;
						}
					}
					else if (num4 < 0f - num2)
					{
						num4 = 0f - num2;
					}
					num4 *= 0.3f;
					if (num4 > 0f)
					{
						global::Players.Players.players[pID].zRotation -= num4;
					}
					else if (num4 < 0f)
					{
						global::Players.Players.players[pID].zRotation -= num4;
					}
					if (global::Players.Players.players[pID].zRotation >= 360f)
					{
						global::Players.Players.players[pID].zRotation -= 360f;
					}
					else if (global::Players.Players.players[pID].zRotation < 0f)
					{
						global::Players.Players.players[pID].zRotation += 360f;
					}
				}
				ref Matrix reference2 = ref global::Players.Players.players[pID].mv[uBufferID];
				reference2 = Matrix.CreateRotationZ(global::Players.Players.players[pID].zRotation * ((float)Math.PI / 180f));
				global::Joints.Joints.Process_Joints_Threaded(pID, frameTime, threadID);
				global::Players.Players.players[pID].shooting = false;
				mainC.weaponsMain.firingStopped((ushort)pID, objectID);
			}
			mainC.playersMain.Player_Torque_Response(pID, global::Players.Players.players[pID].humanoidBackJoint);
			if (!ais[aiID].stationary)
			{
				float num7 = ais[aiID].goalX - global::Players.Players.players[pID].charP.position.v[0];
				float num8 = ais[aiID].goalY - global::Players.Players.players[pID].charP.position.v[1];
				float num5 = num7 * num7 + num8 * num8;
				num2 = 0f;
				if (num5 != 0f)
				{
					num5 = (float)Math.Sqrt(num5);
					num7 /= num5;
					num8 /= num5;
					if (ais[aiID].aiRoute.numPts > 0 || num5 > 0.5f)
					{
						b = 1;
					}
				}
				else
				{
					ais[aiID].resetSpeed = true;
				}
				if (ais[aiID].aiRoute.numPts > 0)
				{
					float num6 = ais[aiID].speed * ais[aiID].randomFactor;
					if (num6 * frameTime >= num5)
					{
						if (++ais[aiID].aiRoute.curPt < ais[aiID].aiRoute.numPts)
						{
							Vector3 vector = ais[aiID].aiRoute.NavMeshRoute[ais[aiID].aiRoute.curPt];
							ais[aiID].goalX = vector.X;
							ais[aiID].goalY = 0f - vector.Z;
							ais[aiID].goalZ = vector.Y;
							num7 = ais[aiID].goalX - global::Players.Players.players[pID].charP.position.v[0];
							num8 = ais[aiID].goalY - global::Players.Players.players[pID].charP.position.v[1];
							num5 = num7 * num7 + num8 * num8;
							num2 = 0f;
							if (num5 != 0f)
							{
								num5 = (float)Math.Sqrt(num5);
								num7 /= num5;
								num8 /= num5;
								if (num5 > 0.5f)
								{
									b = 1;
								}
							}
						}
						else
						{
							ais[aiID].aiRoute.numPts = 0;
						}
					}
					global::Players.Players.players[pID].charP.velocity.v[0] = num6 * num7;
					global::Players.Players.players[pID].charP.velocity.v[1] = num6 * num8;
				}
				else if (num5 != 0f)
				{
					float num6 = ais[aiID].speed * ais[aiID].randomFactor;
					if (num6 * frameTime >= num5)
					{
						num2 = num5 / frameTime;
						global::Players.Players.players[pID].charP.velocity.v[0] = num2 * num7;
						global::Players.Players.players[pID].charP.velocity.v[1] = num2 * num8;
						global::Players.Players.players[pID].velX = 0f;
						global::Players.Players.players[pID].velY = 0f;
					}
					if (num5 < 3f)
					{
						global::Players.Players.players[pID].velX *= 0.5f;
						global::Players.Players.players[pID].velY *= 0.5f;
					}
					else
					{
						global::Players.Players.players[pID].velX = num6 * num7;
						global::Players.Players.players[pID].velY = num6 * num8;
					}
				}
				else
				{
					global::Players.Players.players[pID].velX = 0f;
					global::Players.Players.players[pID].velY = 0f;
				}
			}
			switch (b)
			{
			case 0:
				if (global::Players.Players.players[pID].programStationaryLegsBody > -1 && global::Players.Players.players[pID].animations[global::Players.Players.players[pID].programStationaryLegsBody].status != 2)
				{
					mainC.programsMain.Start_Animation((ushort)pID, ref global::Players.Players.players[pID].jt1, ref global::Players.Players.players[pID].animations, global::Players.Players.players[pID].programCollection, global::Players.Players.players[pID].programStationaryLegsBody, ais[aiID].randomFactor, 1f);
					ais[aiID].state = 0;
				}
				break;
			case 1:
				if (global::Players.Players.players[pID].programWalk > -1 && global::Players.Players.players[pID].animations[global::Players.Players.players[pID].programWalk].status != 2)
				{
					mainC.programsMain.Start_Animation((ushort)pID, ref global::Players.Players.players[pID].jt1, ref global::Players.Players.players[pID].animations, global::Players.Players.players[pID].programCollection, global::Players.Players.players[pID].programWalk, ais[aiID].randomFactor, 1f);
					ais[aiID].state = 1;
				}
				break;
			}
			if (global::Players.Players.players[pID].needToReload)
			{
				mainC.playersMain.Player_Needs_To_Reload((ushort)pID);
				global::Players.Players.players[pID].needToReload = false;
				global::Players.Players.players[pID].needToChamber = false;
			}
			else if (global::Players.Players.players[pID].needToChamber)
			{
				mainC.playersMain.Player_Needs_To_Chamber((ushort)pID);
				global::Players.Players.players[pID].needToChamber = false;
			}
			num2 = ais[aiID].speed * ais[aiID].randomFactor + 0.1f;
			try
			{
				float num6 = ((Math.Sign(global::Players.Players.players[pID].velX) != Math.Sign(global::Players.Players.players[pID].charP.velocity.v[0]) || !(Math.Abs(global::Players.Players.players[pID].velX) >= Math.Abs(global::Players.Players.players[pID].charP.velocity.v[0]))) ? (Math.Abs(global::Players.Players.players[pID].charP.velocity.v[0] - global::Players.Players.players[pID].velX) / num2 * 0.025f + 0.1f) : ((1f - (global::Players.Players.players[pID].velX - global::Players.Players.players[pID].charP.velocity.v[0]) / num2) * 0.025f + 0.005f));
				global::Players.Players.players[pID].velX = global::Players.Players.players[pID].charP.velocity.v[0] + (global::Players.Players.players[pID].velX - global::Players.Players.players[pID].charP.velocity.v[0]) * num6;
				num6 = ((Math.Sign(global::Players.Players.players[pID].velY) != Math.Sign(global::Players.Players.players[pID].charP.velocity.v[1]) || !(Math.Abs(global::Players.Players.players[pID].velY) >= Math.Abs(global::Players.Players.players[pID].charP.velocity.v[1]))) ? (Math.Abs(global::Players.Players.players[pID].charP.velocity.v[1] - global::Players.Players.players[pID].velY) / num2 * 0.025f + 0.1f) : ((1f - (global::Players.Players.players[pID].velY - global::Players.Players.players[pID].charP.velocity.v[1]) / num2) * 0.025f + 0.005f));
				global::Players.Players.players[pID].velY = global::Players.Players.players[pID].charP.velocity.v[1] + (global::Players.Players.players[pID].velY - global::Players.Players.players[pID].charP.velocity.v[1]) * num6;
			}
			catch
			{
				global::InputHandler.InputHandler.tw = 0f;
				global::Players.Players.players[pID].velX = 0f;
				global::Players.Players.players[pID].velY = 0f;
				global::Players.Players.players[pID].charP.velocity.v[0] = 0f;
				global::Players.Players.players[pID].charP.velocity.v[1] = 0f;
				global::Players.Players.players[pID].charP.velocity.v[2] = 0f;
			}
			mainC.playersMain.Move_AI_Humanoid_Player(pID, aiID, frameTime, threadID);
			break;
		}
		case 8:
			ais[aiID].aiRoute.numPts = 0;
			global::Joints.Joints.Reset_Joint_Data(pID);
			global::Joints.Joints.Process_Joints_Threaded(pID, frameTime, threadID);
			global::Players.Players.players[pID].velX = 0f;
			global::Players.Players.players[pID].velY = 0f;
			mainC.playersMain.Move_AI_Humanoid_Player(pID, aiID, frameTime, threadID);
			global::Players.Players.players[pID].deathTime += frameTime / global::Physics.Physics.timeMod;
			if (global::Players.Players.players[pID].deathTime >= global::Players.Players.players[pID].deathTimer)
			{
				global::Players.Players.players[pID].onmap = 2;
				global::Players.Players.players[pID].transporter = 2f;
				global::Players.Players.players[pID].transporterDirection = -1;
				global::Players.Players.players[pID].respawnParticle = mainC.renderingMain.New_Particle_Spawn(9, global::Players.Players.players[pID].respawnParticle, pID, global::Players.Players.players[pID].charP.position.v[0], global::Players.Players.players[pID].charP.position.v[1], global::Players.Players.players[pID].charP.position.v[2]);
			}
			break;
		}
		if ((global::Players.Players.players[pID].teamMask & global::Players.Players.enemyTeamMask) != 0 && global::Players.Players.players[pID].onmap == 4)
		{
			currentActiveEnemyAI++;
		}
	}

	public void Process_AI_Vehicle_Humanoid_Remote_Player(short pID, short aiID, float frameTime, byte threadID)
	{
		byte uBufferID = global::Rendering.Rendering.uBufferID;
		byte b = 0;
		float num = 0f;
		switch (global::Players.Players.players[pID].onmap)
		{
		case 1:
			if (global::Networking.Networking.isHost && !ais[aiID].authorizedToRespawn && lastEnemyAiCount < maxLevelSimultaneousAI && global::Players.Players.players[pID].dead && (numAiKillsForLevelToEnd < 1 || (float)(int)numTotalAiSpawned < (float)(int)numAiKillsForLevelToEnd * killCountScale) && (numAiInWave < 1 || numAiRespawnedInWave < numAiInWave) && currentAiRespawnTime < 0f && aiRespawnTimer != -1f && ais[aiID].bossID == byte.MaxValue && !aiCompleted)
			{
				ais[aiID].authorizedToRespawn = true;
				numTotalAiSpawned++;
				numAiRespawnedInWave++;
				lastEnemyAiCount++;
				if (--numAiLeftToRespawn < 1)
				{
					currentAiRespawnTime = aiRespawnTimer;
					numAiLeftToRespawn = numAiToRespawn;
				}
				try
				{
					mainC.maingameMain.Authorize_Remote_AI_Respawn((ushort)mainC.playersMain.Get_RemoteGamer_Index((byte)ais[aiID].controllingPlayer, 0));
				}
				catch
				{
					global::InputHandler.InputHandler.tw = 5.5f;
				}
			}
			break;
		case 2:
			ais[aiID].authorizedToRespawn = false;
			global::Joints.Joints.Sync_Player_Matrices(pID, global::Rendering.Rendering.rBufferID, uBufferID);
			break;
		case 4:
		{
			ais[aiID].authorizedToRespawn = false;
			if (!ais[aiID].active)
			{
				break;
			}
			if (global::Players.Players.players[pID].speakingTimer <= 0f)
			{
				ais[aiID].speakingTime -= frameTime;
				if (ais[aiID].speakingTime < 0f)
				{
					ais[aiID].speakingTime = aiSpeakingTime + aiSpeakingTimeRandom * (float)global::MainGame.MainGame.mainRandom.NextDouble();
					global::Players.Players.players[pID].voiceCueID = mainC.soundsMain.Play_Voice(global::Players.Players.playerRaces[ais[aiID].race].soundMain[ais[aiID].raceType], global::Players.Players.players[pID].charP.position.v[0], global::Players.Players.players[pID].charP.position.v[1], global::Players.Players.players[pID].charP.position.v[2], 0f, 0f, 0f);
					global::Players.Players.players[pID].speakingTimer = global::Players.Players.playerRaces[ais[aiID].race].mainSoundTimerLength[ais[aiID].raceType];
				}
			}
			byte objectID = global::MainGame.MainGame.playerVehicles[pID].mounts[global::MainGame.MainGame.primaryWeaponMount].objectID;
			ref Matrix reference = ref global::Players.Players.players[pID].mv[uBufferID];
			reference = Matrix.CreateRotationZ(global::Players.Players.players[pID].zRotation * ((float)Math.PI / 180f));
			global::Joints.Joints.Process_Joints_Threaded(pID, frameTime, threadID);
			if (ais[aiID].canFire)
			{
				ais[aiID].fireTimeRemaining -= frameTime / global::Physics.Physics.timeMod;
				if (ais[aiID].fireTimeRemaining <= 0f)
				{
					global::Players.Players.players[pID].shooting = false;
					global::MainGame.MainGame.playerVehicles[pID].weapons[objectID].shooting = false;
					if (ais[aiID].fireTimeRemaining < ais[aiID].timeBetweenFiringAdjusted)
					{
						ais[aiID].fireTimeRemaining = ais[aiID].firingTimeAdjusted + ais[aiID].firingTimeAdj * (float)global::MainGame.MainGame.mainRandom.NextDouble() * ais[aiID].firingTimeAdjusted;
					}
					mainC.weaponsMain.firingStopped((ushort)pID, objectID);
				}
				else
				{
					global::Players.Players.players[pID].shooting = true;
					global::MainGame.MainGame.playerVehicles[pID].weapons[objectID].shooting = true;
				}
			}
			else
			{
				global::Players.Players.players[pID].shooting = false;
				global::MainGame.MainGame.playerVehicles[pID].weapons[objectID].shooting = false;
				ais[aiID].fireTimeRemaining = ais[aiID].timeBetweenFiringAdjusted;
				mainC.weaponsMain.firingStopped((ushort)pID, objectID);
			}
			mainC.playersMain.Player_Torque_Response(pID, global::Players.Players.players[pID].humanoidBackJoint);
			if (!ais[aiID].stationary)
			{
				float num2 = ais[aiID].goalX - global::Players.Players.players[pID].charP.position.v[0];
				float num3 = ais[aiID].goalY - global::Players.Players.players[pID].charP.position.v[1];
				float num4 = num2 * num2 + num3 * num3;
				if (num4 != 0f)
				{
					num4 = (float)Math.Sqrt(num4);
					num2 /= num4;
					num3 /= num4;
					if (ais[aiID].aiRoute.numPts > 0 || num4 > 0.5f)
					{
						b = 1;
					}
				}
				if (ais[aiID].aiRoute.numPts > 0)
				{
					global::Players.Players.players[pID].velX = num2 * ais[aiID].speed * ais[aiID].randomFactor;
					global::Players.Players.players[pID].velY = num3 * ais[aiID].speed * ais[aiID].randomFactor;
				}
				else if (num4 != 0f)
				{
					if (num4 > 3f)
					{
						global::Players.Players.players[pID].velX = num2 * ais[aiID].speed * ais[aiID].randomFactor;
						global::Players.Players.players[pID].velY = num3 * ais[aiID].speed * ais[aiID].randomFactor;
					}
					else
					{
						global::Players.Players.players[pID].velX *= 0.5f;
						global::Players.Players.players[pID].velY *= 0.5f;
					}
				}
				else
				{
					global::Players.Players.players[pID].velX = 0f;
					global::Players.Players.players[pID].velY = 0f;
				}
			}
			switch (b)
			{
			case 0:
				if (global::Players.Players.players[pID].programStationaryLegsBody > -1 && global::Players.Players.players[pID].animations[global::Players.Players.players[pID].programStationaryLegsBody].status != 2)
				{
					mainC.programsMain.Start_Animation((ushort)pID, ref global::Players.Players.players[pID].jt1, ref global::Players.Players.players[pID].animations, global::Players.Players.players[pID].programCollection, global::Players.Players.players[pID].programStationaryLegsBody, ais[aiID].randomFactor, 1f);
					ais[aiID].state = 0;
				}
				break;
			case 1:
				if (global::Players.Players.players[pID].programWalk > -1 && global::Players.Players.players[pID].animations[global::Players.Players.players[pID].programWalk].status != 2)
				{
					mainC.programsMain.Start_Animation((ushort)pID, ref global::Players.Players.players[pID].jt1, ref global::Players.Players.players[pID].animations, global::Players.Players.players[pID].programCollection, global::Players.Players.players[pID].programWalk, ais[aiID].randomFactor, 1f);
					ais[aiID].state = 1;
				}
				break;
			}
			if (global::Players.Players.players[pID].needToReload)
			{
				mainC.playersMain.Player_Needs_To_Reload((ushort)pID);
				global::Players.Players.players[pID].needToReload = false;
				global::Players.Players.players[pID].needToChamber = false;
			}
			else if (global::Players.Players.players[pID].needToChamber)
			{
				mainC.playersMain.Player_Needs_To_Chamber((ushort)pID);
				global::Players.Players.players[pID].needToChamber = false;
			}
			num = ais[aiID].speed * ais[aiID].randomFactor + 0.1f;
			try
			{
				float num5 = ((Math.Sign(global::Players.Players.players[pID].velX) != Math.Sign(global::Players.Players.players[pID].charP.velocity.v[0]) || !(Math.Abs(global::Players.Players.players[pID].velX) >= Math.Abs(global::Players.Players.players[pID].charP.velocity.v[0]))) ? (Math.Abs(global::Players.Players.players[pID].charP.velocity.v[0] - global::Players.Players.players[pID].velX) / num * 0.025f + 0.1f) : ((1f - (global::Players.Players.players[pID].velX - global::Players.Players.players[pID].charP.velocity.v[0]) / num) * 0.025f + 0.005f));
				global::Players.Players.players[pID].velX = global::Players.Players.players[pID].charP.velocity.v[0] + (global::Players.Players.players[pID].velX - global::Players.Players.players[pID].charP.velocity.v[0]) * num5;
				num5 = ((Math.Sign(global::Players.Players.players[pID].velY) != Math.Sign(global::Players.Players.players[pID].charP.velocity.v[1]) || !(Math.Abs(global::Players.Players.players[pID].velY) >= Math.Abs(global::Players.Players.players[pID].charP.velocity.v[1]))) ? (Math.Abs(global::Players.Players.players[pID].charP.velocity.v[1] - global::Players.Players.players[pID].velY) / num * 0.025f + 0.1f) : ((1f - (global::Players.Players.players[pID].velY - global::Players.Players.players[pID].charP.velocity.v[1]) / num) * 0.025f + 0.005f));
				global::Players.Players.players[pID].velY = global::Players.Players.players[pID].charP.velocity.v[1] + (global::Players.Players.players[pID].velY - global::Players.Players.players[pID].charP.velocity.v[1]) * num5;
			}
			catch
			{
				global::InputHandler.InputHandler.tw = 0f;
				global::Players.Players.players[pID].velX = 0f;
				global::Players.Players.players[pID].velY = 0f;
				global::Players.Players.players[pID].charP.velocity.v[0] = 0f;
				global::Players.Players.players[pID].charP.velocity.v[1] = 0f;
				global::Players.Players.players[pID].charP.velocity.v[2] = 0f;
			}
			mainC.playersMain.Move_AI_Humanoid_Player(pID, aiID, frameTime, threadID);
			break;
		}
		case 8:
			ais[aiID].authorizedToRespawn = false;
			global::Joints.Joints.Process_Joints_Threaded(pID, frameTime, threadID);
			global::Players.Players.players[pID].velX = 0f;
			global::Players.Players.players[pID].velY = 0f;
			mainC.playersMain.Move_AI_Humanoid_Player(pID, aiID, frameTime, threadID);
			break;
		}
		if ((global::Players.Players.players[pID].teamMask & global::Players.Players.enemyTeamMask) != 0 && global::Players.Players.players[pID].onmap == 4)
		{
			currentActiveEnemyAI++;
		}
	}

	public void Process_AI_Vehicle_Airplane(short pID, short aiID, float frameTime, byte threadID)
	{
		byte uBufferID = global::Rendering.Rendering.uBufferID;
		int num = 0;
		switch (global::Players.Players.players[pID].onmap)
		{
		case 1:
			if (lastEnemyAiCount < maxLevelSimultaneousAI && global::Players.Players.players[pID].dead && (numAiKillsForLevelToEnd < 1 || (float)(int)numTotalAiSpawned < (float)(int)numAiKillsForLevelToEnd * killCountScale) && currentAiRespawnTime < 0f && aiRespawnTimer != -1f && ais[aiID].bossID == byte.MaxValue)
			{
				numTotalAiSpawned++;
				lastEnemyAiCount++;
				if (--numAiLeftToRespawn < 1)
				{
					currentAiRespawnTime = aiRespawnTimer;
					numAiLeftToRespawn = numAiToRespawn;
				}
				mainC.mapsMain.Get_AI_Spawn_Point(ref global::Players.Players.players[pID].charP.position, ais[aiID].team, ref global::Players.Players.players[pID].zRotation, global::MainGame.MainGame.maxLocalPlayerSpawnPoint, ais[aiID].checkForEnemy, global::Players.Players.playerRaces[global::Players.Players.players[pID].race].spawnHeight[global::Players.Players.players[pID].type]);
				mainC.playersMain.Player_Respawn_AI((ushort)pID, threadID);
				Reset_AI_Before_Respawn(aiID);
			}
			break;
		case 2:
			global::Players.Players.players[pID].timeBeforeRespawn[uBufferID] -= frameTime;
			if (global::Players.Players.players[pID].timeBeforeRespawn[uBufferID] < 0f)
			{
				mainC.playersMain.Player_Spawn_Time_Over((ushort)pID);
			}
			global::Joints.Joints.Sync_Player_Matrices(pID, global::Rendering.Rendering.rBufferID, uBufferID);
			break;
		case 4:
		{
			if (!ais[aiID].active)
			{
				break;
			}
			ais[aiID].velocityVariation += frameTime * (float)ais[aiID].velocityVariationDirection;
			if (Math.Abs(ais[aiID].velocityVariation) > 0.1f)
			{
				ais[aiID].velocityVariationDirection *= -1;
			}
			bool flag = false;
			float rotX = 0f;
			float rotY = 0f;
			float num2;
			float num9;
			float num6;
			if (ais[aiID].targetID > -1)
			{
				if (ais[aiID].targetVisible)
				{
					num = ais[aiID].targetID;
					aiVec2.v[0] = global::Players.Players.players[num].charP.position.v[0];
					aiVec2.v[1] = global::Players.Players.players[num].charP.position.v[1];
					aiVec2.v[2] = global::Players.Players.players[num].charP.position.v[2];
					num2 = 90f;
					if (global::MainGame.MainGame.angularVelocity[num] != 0f && global::MainGame.MainGame.playerVehicles[pID].velocity != 0f)
					{
						float num3 = (float)Math.PI * 2f / Math.Abs(global::MainGame.MainGame.angularVelocity[num] * 3.2f);
						float num4 = num3 * global::MainGame.MainGame.playerVehicles[num].velocity;
						float num5 = num4 / ((float)Math.PI * 2f);
						Matrix matrix = global::Players.Players.players[num].mv[global::Rendering.Rendering.rBufferID];
						float m = matrix.M11;
						float m2 = matrix.M12;
						num6 = (float)Math.Sqrt(m * m + m2 * m2);
						if (num6 != 0f)
						{
							if (global::MainGame.MainGame.angularVelocity[num] >= 0f)
							{
								num6 *= -1f;
							}
							m /= num6;
							m2 /= num6;
							float num7 = global::Players.Players.players[num].charP.position.v[0] + num5 * m;
							float num8 = global::Players.Players.players[num].charP.position.v[1] + num5 * m2;
							num9 = 0f;
							if (num6 != 0f)
							{
								num9 = (float)Math.Acos(0f - m2);
							}
							if (0f - m > 0f)
							{
								num9 = (float)Math.PI * 2f - num9;
							}
							aiVec1.v[0] = aiVec2.v[0] - global::Players.Players.players[pID].charP.position.v[0];
							aiVec1.v[1] = aiVec2.v[1] - global::Players.Players.players[pID].charP.position.v[1];
							aiVec1.v[2] = aiVec2.v[2] - global::Players.Players.players[pID].charP.position.v[2];
							num6 = (float)Math.Sqrt(aiVec1.v[0] * aiVec1.v[0] + aiVec1.v[1] * aiVec1.v[1]);
							float num10 = num6 / global::MainGame.MainGame.playerVehicles[pID].velocity;
							if (num10 > 0f)
							{
								num9 += global::MainGame.MainGame.angularVelocity[num] * 3.2f * num10;
								aiVec2.v[0] = num7 - (float)Math.Sin(num9) * num5;
								aiVec2.v[1] = num8 + (float)Math.Cos(num9) * num5;
							}
						}
					}
				}
				else
				{
					aiVec2.v[0] = ais[aiID].lastTargetX;
					aiVec2.v[1] = ais[aiID].lastTargetY;
					aiVec2.v[2] = ais[aiID].lastTargetZ;
					num2 = 70f;
				}
			}
			else
			{
				float num10 = global::MainGame.MainGame.MaxLeft + (global::MainGame.MainGame.MaxRight - global::MainGame.MainGame.MaxLeft) / 2f + (float)aiID * 100f;
				float num3 = global::MainGame.MainGame.MaxRear + (global::MainGame.MainGame.MaxForward - global::MainGame.MainGame.MaxRear) / 2f + (float)aiID * 100f;
				aiVec2.v[0] = num10;
				aiVec2.v[1] = num3;
				aiVec2.v[2] = mainC.terrainMain.Get_Terrain_Height(num10, num3, threadID) + 500f;
				num2 = 45f;
			}
			float num11 = global::Players.Players.players[num].charP.position.v[0] - global::Players.Players.players[pID].charP.position.v[0];
			float num12 = global::Players.Players.players[num].charP.position.v[1] - global::Players.Players.players[pID].charP.position.v[1];
			float num13 = global::Players.Players.players[num].charP.position.v[2] - global::Players.Players.players[pID].charP.position.v[2];
			num6 = (float)Math.Sqrt(num11 * num11 + num12 * num12);
			float num14 = 0f;
			if (num6 != 0f)
			{
				num14 = (float)Math.Acos(num12 / num6) * 57.29578f;
			}
			if (num11 > 0f)
			{
				num14 = 360f - num14;
			}
			num14 -= global::Players.Players.players[pID].zRotation;
			aiVec1.v[0] = aiVec2.v[0] - global::Players.Players.players[pID].charP.position.v[0];
			aiVec1.v[1] = aiVec2.v[1] - global::Players.Players.players[pID].charP.position.v[1];
			aiVec1.v[2] = aiVec2.v[2] - global::Players.Players.players[pID].charP.position.v[2];
			num6 = (float)Math.Sqrt(aiVec1.v[0] * aiVec1.v[0] + aiVec1.v[1] * aiVec1.v[1]);
			num9 = 0f;
			if (num6 != 0f)
			{
				num9 = (float)Math.Acos(aiVec1.v[1] / num6) * 57.29578f;
			}
			if (aiVec1.v[0] > 0f)
			{
				num9 = 360f - num9;
			}
			rotX = num9 - global::Players.Players.players[pID].zRotation;
			if (Math.Abs(rotX) < 0.1f)
			{
				rotX = 0f;
			}
			if (rotX > 180f)
			{
				rotX -= 360f;
			}
			else if (rotX < -180f)
			{
				rotX = 360f + rotX;
			}
			if (Math.Abs(rotX) > num2)
			{
				rotX = num2 * (float)Math.Sign(rotX);
			}
			num6 = (float)Math.Sqrt(num11 * num11 + num12 * num12 + num13 * num13);
			num9 = 0f;
			if (num6 != 0f)
			{
				num9 = (float)Math.Asin(num13 / num6) * 57.29578f;
			}
			rotY = num9 - (float)Math.Atan(global::Players.Players.players[pID].mv[uBufferID].M23) * 57.29578f;
			if (Math.Abs(rotY) < 0.1f)
			{
				rotY = 0f;
			}
			else if (rotY > 60f)
			{
				rotY = 60f;
			}
			if (global::Players.Players.players[pID].needToReload)
			{
				mainC.playersMain.Player_Needs_To_Reload((ushort)pID);
				global::Players.Players.players[pID].needToReload = false;
				global::Players.Players.players[pID].needToChamber = false;
			}
			else if (global::Players.Players.players[pID].needToChamber)
			{
				mainC.playersMain.Player_Needs_To_Chamber((ushort)pID);
				global::Players.Players.players[pID].needToChamber = false;
			}
			if (!flag)
			{
				Matrix.CreateRotationZ(global::Players.Players.players[pID].zRotation * ((float)Math.PI / 180f), out global::Players.Players.players[pID].mv[uBufferID]);
			}
			mainC.playersMain.Move_AI_Player_Airplane((byte)pID, aiID, rotX, rotY, frameTime, threadID);
			global::Joints.Joints.Reset_Joint_Data(pID);
			global::Joints.Joints.Process_Joints_Threaded(pID, frameTime, threadID);
			num11 = global::Players.Players.players[num].charP.position.v[0] - global::Players.Players.players[pID].charP.position.v[0];
			num12 = global::Players.Players.players[num].charP.position.v[1] - global::Players.Players.players[pID].charP.position.v[1];
			num13 = global::Players.Players.players[num].charP.position.v[2] - global::Players.Players.players[pID].charP.position.v[2];
			byte objectID = global::MainGame.MainGame.playerVehicles[pID].mounts[global::MainGame.MainGame.primaryWeaponMount].objectID;
			if (ais[aiID].targetVisible && ais[aiID].goalDistance < ais[aiID].optimalTargetDistance)
			{
				ais[aiID].fireTimeRemaining -= frameTime / global::Physics.Physics.timeMod;
				global::Players.Players.players[pID].shooting = false;
				global::MainGame.MainGame.playerVehicles[pID].weapons[objectID].shooting = false;
				ais[aiID].fireTimeRemaining = -1f;
				num14 = 0f;
				num6 = (float)Math.Sqrt(num11 * num11 + num12 * num12 + num13 * num13);
				if (num6 != 0f)
				{
					num11 /= num6;
					num12 /= num6;
					num13 /= num6;
					num14 = num11 * global::Players.Players.players[pID].mv[uBufferID].M21 + num12 * global::Players.Players.players[pID].mv[uBufferID].M22 + num13 * global::Players.Players.players[pID].mv[uBufferID].M23;
				}
				if (num14 > 0f && Math.Sin(Math.Acos(num14)) * (double)num6 < 40.0)
				{
					ais[aiID].fireTimeRemaining = ais[aiID].firingTimeAdjusted;
					global::Players.Players.players[pID].shooting = true;
					global::MainGame.MainGame.playerVehicles[pID].weapons[objectID].shooting = true;
				}
				if (!global::Players.Players.players[pID].shooting)
				{
					mainC.weaponsMain.firingStopped((ushort)pID, objectID);
				}
			}
			else
			{
				ais[aiID].fireTimeRemaining = -1f;
				global::Players.Players.players[pID].shooting = false;
				global::MainGame.MainGame.playerVehicles[pID].weapons[objectID].shooting = false;
				mainC.weaponsMain.firingStopped((ushort)pID, objectID);
			}
			break;
		}
		case 8:
		{
			global::Joints.Joints.Reset_Joint_Data(pID);
			global::Joints.Joints.Process_Joints_Threaded(pID, frameTime, threadID);
			float rotX = -5f;
			float rotY = -20f;
			mainC.playersMain.Move_AI_Player_Airplane((byte)pID, aiID, rotX, rotY, frameTime, threadID);
			break;
		}
		}
		if ((global::Players.Players.players[pID].teamMask & global::Players.Players.enemyTeamMask) != 0 && global::Players.Players.players[pID].onmap == 4)
		{
			currentActiveEnemyAI++;
		}
	}

	public void Process_AI_Vehicle_UnmannedTurret(short pID, short aiID, float frameTime, byte threadID)
	{
		byte uBufferID = global::Rendering.Rendering.uBufferID;
		int num = 0;
		float num2 = 0f;
		switch (global::Players.Players.players[pID].onmap)
		{
		case 1:
			if (ais[aiID].aiType == 0 && lastEnemyAiCount < maxLevelSimultaneousAI && global::Players.Players.players[pID].dead && (numAiKillsForLevelToEnd < 1 || (float)(int)numTotalAiSpawned < (float)(int)numAiKillsForLevelToEnd * killCountScale) && currentAiRespawnTime < 0f && aiRespawnTimer != -1f && ais[aiID].bossID == byte.MaxValue)
			{
				numTotalAiSpawned++;
				lastEnemyAiCount++;
				if (--numAiLeftToRespawn < 1)
				{
					currentAiRespawnTime = aiRespawnTimer;
					numAiLeftToRespawn = numAiToRespawn;
				}
				mainC.playersMain.Player_Respawn_AI((ushort)pID, threadID);
				Reset_AI_Before_Respawn(aiID);
			}
			break;
		case 2:
			global::Players.Players.players[pID].timeBeforeRespawn[uBufferID] -= frameTime;
			if (global::Players.Players.players[pID].timeBeforeRespawn[uBufferID] < 0f)
			{
				mainC.playersMain.Player_Spawn_Time_Over((ushort)pID);
			}
			global::Joints.Joints.Sync_Player_Matrices(pID, global::Rendering.Rendering.rBufferID, uBufferID);
			break;
		case 4:
		{
			if (!ais[aiID].active)
			{
				break;
			}
			float num5 = 0f;
			bool flag = false;
			float num6 = 0f;
			ais[aiID].velocityVariation += frameTime * (float)ais[aiID].velocityVariationDirection;
			if (Math.Abs(ais[aiID].velocityVariation) > 0.1f)
			{
				ais[aiID].velocityVariationDirection *= -1;
			}
			byte objectID = global::MainGame.MainGame.playerVehicles[pID].mounts[global::MainGame.MainGame.primaryWeaponMount].objectID;
			if (ais[aiID].targetID > -1 && ais[aiID].targetVisible)
			{
				float num7 = mainC.weaponsMain.Get_Ammo_Muzzle_Velocity_Current_Weapon((ushort)pID);
				if (num7 == 0f)
				{
					num7 = 1E-05f;
				}
				num = ais[aiID].targetID;
				Matrix matrix = Matrix.Invert(global::Players.Players.players[pID].mv[global::Rendering.Rendering.rBufferID]);
				int jointRID = global::Players.Players.players[pID].weapon1.jointRID;
				aiVec2.v[0] = global::Players.Players.players[num].charP.position.v[0] - global::Players.Players.players[pID].charP.position.v[0];
				aiVec2.v[1] = global::Players.Players.players[num].charP.position.v[1] - global::Players.Players.players[pID].charP.position.v[1];
				aiVec2.v[2] = global::Players.Players.players[num].charP.position.v[2] - global::Players.Players.players[pID].charP.position.v[2];
				float num8 = (float)Math.Sqrt(aiVec2.v[0] * aiVec2.v[0] + aiVec2.v[1] * aiVec2.v[1] + aiVec2.v[2] * aiVec2.v[2]);
				num2 = num8 / num7;
				if (ais[aiID].leadsTarget)
				{
					aiVec2.v[0] += global::MainGame.MainGame.playerVehicles[num].ph1.velocityX * num2;
					aiVec2.v[1] += global::MainGame.MainGame.playerVehicles[num].ph1.velocityY * num2;
					aiVec2.v[2] += global::MainGame.MainGame.playerVehicles[num].ph1.velocityZ * num2;
				}
				aiVec1.v[0] = aiVec2.v[0] * matrix.M11 + aiVec2.v[1] * matrix.M21 + aiVec2.v[2] * matrix.M31;
				aiVec1.v[1] = aiVec2.v[0] * matrix.M12 + aiVec2.v[1] * matrix.M22 + aiVec2.v[2] * matrix.M32;
				aiVec1.v[2] = aiVec2.v[0] * matrix.M13 + aiVec2.v[1] * matrix.M23 + aiVec2.v[2] * matrix.M33;
				num8 = (float)Math.Sqrt(aiVec1.v[0] * aiVec1.v[0] + aiVec1.v[1] * aiVec1.v[1]);
				num2 = aiVec1.v[1] / num8;
				float num9 = (float)Math.Acos(num2) * 57.29578f;
				if (aiVec1.v[0] > 0f)
				{
					num9 = 360f - num9;
				}
				float num10 = global::Players.Players.players[pID].weapon1.offset[0, 5].v[0];
				float num11 = global::Players.Players.players[pID].weapon1.offset[0, 5].v[1];
				float num12 = global::Players.Players.players[pID].weapon1.offset[0, 5].v[2];
				num2 = num10 * matrix.M11 + num11 * matrix.M21 + num12 * matrix.M31;
				float num3 = num10 * matrix.M12 + num11 * matrix.M22 + num12 * matrix.M32;
				num10 = (float)Math.Sqrt(num2 * num2 + num3 * num3);
				if (num10 != 0f)
				{
					num2 /= num10;
					num3 /= num10;
				}
				float num4 = (float)Math.Acos(num3);
				if (num2 > 0f)
				{
					num4 = (float)Math.PI * 2f - num4;
				}
				num5 = num9 - num4 * 57.29578f;
				num9 = num5;
				if (num5 > 180f)
				{
					num5 = 0f - (360f - num5);
				}
				else if (num5 < -180f)
				{
					num5 = 360f + num5;
				}
				global::Players.Players.players[pID].jt1[jointRID].pivotSpeed = ais[aiID].speedRotationZ;
				global::Players.Players.players[pID].jt1[jointRID].targetPivot = global::Players.Players.players[pID].jt1[jointRID].rotZ + num5;
				jointRID = global::Players.Players.players[pID].weapon1.jointEID;
				num8 = (float)Math.Sqrt(aiVec1.v[0] * aiVec1.v[0] + aiVec1.v[1] * aiVec1.v[1] + aiVec1.v[2] * aiVec1.v[2]);
				float num13 = (float)Math.Asin(aiVec1.v[2] / num8) * 57.29578f;
				global::Players.Players.players[pID].jt1[jointRID].targetAngle = ais[aiID].weaponElevationDir * num13;
				num2 = global::Players.Players.players[pID].weapon1.offset[0, 5].v[0] * matrix.M13 + global::Players.Players.players[pID].weapon1.offset[0, 5].v[1] * matrix.M23 + global::Players.Players.players[pID].weapon1.offset[0, 5].v[2] * matrix.M33;
				float num14 = num13 - (float)Math.Asin(num2) * 57.29578f;
				num13 = num14;
				if (Math.Abs(num14) < 0.25f)
				{
					global::Players.Players.players[pID].jt1[jointRID].targetAngle = global::Players.Players.players[pID].jt1[jointRID].rotX;
				}
				global::Players.Players.players[pID].jt1[jointRID].angleSpeed = ais[aiID].speedRotationX;
				if (global::Players.Players.players[pID].jt1[jointRID].targetAngle >= 360f)
				{
					global::Players.Players.players[pID].jt1[jointRID].targetAngle -= 360f;
				}
				else if (global::Players.Players.players[pID].jt1[jointRID].targetAngle < -360f)
				{
					global::Players.Players.players[pID].jt1[jointRID].targetAngle += 360f;
				}
				if (global::Players.Players.players[pID].jt1[jointRID].targetAngle > global::Players.Players.players[pID].jt1[jointRID].maxAngle)
				{
					global::Players.Players.players[pID].jt1[jointRID].targetAngle = global::Players.Players.players[pID].jt1[jointRID].maxAngle;
				}
				if (global::Players.Players.players[pID].jt1[jointRID].targetAngle < global::Players.Players.players[pID].jt1[jointRID].minAngle)
				{
					global::Players.Players.players[pID].jt1[jointRID].targetAngle = global::Players.Players.players[pID].jt1[jointRID].minAngle;
				}
				global::Players.Players.players[pID].weapon2[global::Players.Players.players[pID].wpnIndex].ammoTimer = num8 / num7;
				global::Joints.Joints.Reset_Joint_Data(pID);
				global::Joints.Joints.Process_Joints_Threaded(pID, frameTime, threadID);
				Matrix.CreateRotationZ(global::Players.Players.players[pID].zRotation * ((float)Math.PI / 180f), out global::Players.Players.players[pID].mv[uBufferID]);
				if (ais[aiID].hostID < global::MainGame.MainGame.maxGamePlayers)
				{
					ref Matrix reference = ref global::Players.Players.players[pID].mv[uBufferID];
					reference = global::Players.Players.players[pID].mv[uBufferID] * ais[aiID].hostMatrix * global::Players.Players.players[ais[ais[aiID].hostID].playerID].mv[uBufferID];
				}
				mainC.weaponsMain.Process_Player_Weapons(pID, global::Players.Players.players[pID].primaryWeaponMountWeapon);
				if (ais[aiID].targetInRange && Math.Abs(num9) < 5f && Math.Abs(num13) < 5f)
				{
					ais[aiID].fireTimeRemaining -= frameTime / global::Physics.Physics.timeMod;
					if (ais[aiID].fireTimeRemaining <= 0f)
					{
						global::Players.Players.players[pID].shooting = false;
						global::MainGame.MainGame.playerVehicles[pID].weapons[objectID].shooting = false;
						if (ais[aiID].fireTimeRemaining < ais[aiID].timeBetweenFiringAdjusted)
						{
							ais[aiID].fireTimeRemaining = ais[aiID].firingTimeAdjusted + ais[aiID].firingTimeAdj * ((float)global::MainGame.MainGame.mainRandom.Next(0, 32000) / 32000f);
						}
						mainC.weaponsMain.firingStopped((ushort)pID, objectID);
					}
					else
					{
						global::Players.Players.players[pID].shooting = true;
						global::MainGame.MainGame.playerVehicles[pID].weapons[objectID].shooting = true;
					}
				}
				else
				{
					global::Players.Players.players[pID].shooting = false;
					global::MainGame.MainGame.playerVehicles[pID].weapons[objectID].shooting = false;
					ais[aiID].fireTimeRemaining = ais[aiID].timeBetweenFiringAdjusted;
					mainC.weaponsMain.firingStopped((ushort)pID, objectID);
				}
			}
			else
			{
				ais[aiID].fireTimeRemaining = ais[aiID].timeBetweenFiringAdjusted;
				ref Matrix reference2 = ref global::Players.Players.players[pID].mv[uBufferID];
				reference2 = global::Players.Players.players[pID].mv[global::Rendering.Rendering.rBufferID];
				global::Joints.Joints.Reset_Joint_Data(pID);
				global::Joints.Joints.Process_Joints_Threaded(pID, frameTime, threadID);
				global::Players.Players.players[pID].shooting = false;
				mainC.weaponsMain.firingStopped((ushort)pID, objectID);
			}
			mainC.playersMain.Player_Torque_Response(pID, global::Players.Players.players[pID].humanoidBackJoint);
			num2 = 0f;
			if (flag)
			{
				num2 = 1f;
			}
			if (num6 > 2f)
			{
				num2 = 2f;
			}
			num2 = 2f;
			switch ((int)num2)
			{
			case 0:
				if (global::Players.Players.players[pID].programStationaryLegsBody > -1 && global::Players.Players.players[pID].animations[global::Players.Players.players[pID].programStationaryLegsBody].status != 2)
				{
					mainC.programsMain.Start_Animation((ushort)pID, ref global::Players.Players.players[pID].jt1, ref global::Players.Players.players[pID].animations, global::Players.Players.players[pID].programCollection, global::Players.Players.players[pID].programStationaryLegsBody, 1f, 1f);
					ais[aiID].state = 0;
				}
				break;
			case 1:
				if (global::Players.Players.players[pID].programTurnLeft > -1 && global::Players.Players.players[pID].animations[global::Players.Players.players[pID].programTurnLeft].status != 2)
				{
					mainC.programsMain.Start_Animation((ushort)pID, ref global::Players.Players.players[pID].jt1, ref global::Players.Players.players[pID].animations, global::Players.Players.players[pID].programCollection, global::Players.Players.players[pID].programTurnLeft, 1f, 1f);
					ais[aiID].state = 2;
				}
				break;
			case 2:
				if (global::Players.Players.players[pID].programWalk > -1 && global::Players.Players.players[pID].animations[global::Players.Players.players[pID].programWalk].status != 2)
				{
					mainC.programsMain.Start_Animation((ushort)pID, ref global::Players.Players.players[pID].jt1, ref global::Players.Players.players[pID].animations, global::Players.Players.players[pID].programCollection, global::Players.Players.players[pID].programWalk, 1f, 1f);
					ais[aiID].state = 1;
				}
				break;
			}
			if (global::Players.Players.players[pID].needToReload)
			{
				mainC.playersMain.Player_Needs_To_Reload((ushort)pID);
				global::Players.Players.players[pID].needToReload = false;
				global::Players.Players.players[pID].needToChamber = false;
			}
			else if (global::Players.Players.players[pID].needToChamber)
			{
				mainC.playersMain.Player_Needs_To_Chamber((ushort)pID);
				global::Players.Players.players[pID].needToChamber = false;
			}
			num2 = global::Players.Players.players[pID].damagePercentageCapped;
			if (num2 > 0.1f)
			{
				global::MainGame.MainGame.playerVehicles[pID].particleTimer += global::MainGame.MainGame.frametime;
				if (global::MainGame.MainGame.playerVehicles[pID].particleTimer > 0.33f - num2 * 0.3f)
				{
					global::MainGame.MainGame.playerVehicles[pID].particleTimer = 0f;
					global::Rendering.Rendering.npn.v[0] = 1.5f + num2;
					global::Rendering.Rendering.npn.v[1] = 10f + num2 * 200f;
					global::Rendering.Rendering.npn.v[2] = 0.1f + num2 * 0.3f;
					Matrix matrix = global::Players.Players.players[pID].mv[uBufferID];
					num2 = global::MainGame.MainGame.playerVehicles[pID].ph1.x + global::MainGame.MainGame.playerVehicles[pID].damageParticleX * matrix.M11 + global::MainGame.MainGame.playerVehicles[pID].damageParticleY * matrix.M21 + global::MainGame.MainGame.playerVehicles[pID].damageParticleZ * matrix.M31;
					float num3 = global::MainGame.MainGame.playerVehicles[pID].ph1.y + global::MainGame.MainGame.playerVehicles[pID].damageParticleX * matrix.M12 + global::MainGame.MainGame.playerVehicles[pID].damageParticleY * matrix.M22 + global::MainGame.MainGame.playerVehicles[pID].damageParticleZ * matrix.M32;
					float num4 = global::MainGame.MainGame.playerVehicles[pID].ph1.z + global::MainGame.MainGame.playerVehicles[pID].damageParticleX * matrix.M13 + global::MainGame.MainGame.playerVehicles[pID].damageParticleY * matrix.M23 + global::MainGame.MainGame.playerVehicles[pID].damageParticleZ * matrix.M33;
					mainC.renderingMain.New_Particle_New(13, global::Players.Players.players[pID].charP.position.v[0] + global::MainGame.MainGame.playerVehicles[pID].damageParticleX, global::Players.Players.players[pID].charP.position.v[1] + global::MainGame.MainGame.playerVehicles[pID].damageParticleY, global::Players.Players.players[pID].charP.position.v[2] + global::MainGame.MainGame.playerVehicles[pID].damageParticleZ, 0f, 0f, 25f, pID, threadID);
				}
			}
			break;
		}
		case 8:
			if (ais[aiID].aiType == 0)
			{
				global::Joints.Joints.Reset_Joint_Data(pID);
				global::Joints.Joints.Process_Joints_Threaded(pID, frameTime, threadID);
				num2 = global::Players.Players.players[pID].damagePercentageCapped;
				global::MainGame.MainGame.playerVehicles[pID].particleTimer += global::MainGame.MainGame.frametime;
				if (global::MainGame.MainGame.playerVehicles[pID].particleTimer > 0.33f - num2 * 0.3f)
				{
					global::MainGame.MainGame.playerVehicles[pID].particleTimer = 0f;
					global::Rendering.Rendering.npn.v[0] = 1.5f + num2;
					global::Rendering.Rendering.npn.v[1] = 10f + num2 * 200f;
					global::Rendering.Rendering.npn.v[2] = 0.1f + num2 * 0.3f;
					Matrix matrix = global::Players.Players.players[pID].mv[uBufferID];
					num2 = global::MainGame.MainGame.playerVehicles[pID].ph1.x + global::MainGame.MainGame.playerVehicles[pID].damageParticleX * matrix.M11 + global::MainGame.MainGame.playerVehicles[pID].damageParticleY * matrix.M21 + global::MainGame.MainGame.playerVehicles[pID].damageParticleZ * matrix.M31;
					float num3 = global::MainGame.MainGame.playerVehicles[pID].ph1.y + global::MainGame.MainGame.playerVehicles[pID].damageParticleX * matrix.M12 + global::MainGame.MainGame.playerVehicles[pID].damageParticleY * matrix.M22 + global::MainGame.MainGame.playerVehicles[pID].damageParticleZ * matrix.M32;
					float num4 = global::MainGame.MainGame.playerVehicles[pID].ph1.z + global::MainGame.MainGame.playerVehicles[pID].damageParticleX * matrix.M13 + global::MainGame.MainGame.playerVehicles[pID].damageParticleY * matrix.M23 + global::MainGame.MainGame.playerVehicles[pID].damageParticleZ * matrix.M33;
					mainC.renderingMain.New_Particle_New(13, global::Players.Players.players[pID].charP.position.v[0], global::Players.Players.players[pID].charP.position.v[1], global::Players.Players.players[pID].charP.position.v[2], 0f, 0f, 25f, pID, threadID);
				}
				mainC.playersMain.Adjust_Player_Damage((ushort)pID, 0f - frameTime, sendOnline: false);
				if (global::Players.Players.players[pID].damage < 0f)
				{
					mainC.playersMain.Adjust_Player_Damage_To_Zero((ushort)pID, sendOnline: false);
					global::Players.Players.players[pID].onmap = 2;
					global::Players.Players.players[pID].transporter = 2f;
					global::Players.Players.players[pID].transporterDirection = -1;
					global::Players.Players.players[pID].respawnParticle = mainC.renderingMain.New_Particle_Spawn(9, global::Players.Players.players[pID].respawnParticle, pID, global::Players.Players.players[pID].charP.position.v[0], global::Players.Players.players[pID].charP.position.v[1], global::Players.Players.players[pID].charP.position.v[2]);
				}
			}
			break;
		}
		if (ais[aiID].aiType == 0 && (global::Players.Players.players[pID].teamMask & global::Players.Players.enemyTeamMask) != 0 && global::Players.Players.players[pID].onmap == 4)
		{
			currentActiveEnemyAI++;
		}
	}

	public void Process_AI_Vehicle_SpaceShip(short pID, short aiID, float frameTime, byte threadID)
	{
		byte uBufferID = global::Rendering.Rendering.uBufferID;
		int num = 0;
		switch (global::Players.Players.players[pID].onmap)
		{
		case 1:
			if (lastEnemyAiCount < maxLevelSimultaneousAI && global::Players.Players.players[pID].dead && (numAiKillsForLevelToEnd < 1 || (float)(int)numTotalAiSpawned < (float)(int)numAiKillsForLevelToEnd * killCountScale) && currentAiRespawnTime < 0f && aiRespawnTimer != -1f && ais[aiID].bossID == byte.MaxValue)
			{
				numTotalAiSpawned++;
				lastEnemyAiCount++;
				if (--numAiLeftToRespawn < 1)
				{
					currentAiRespawnTime = aiRespawnTimer;
					numAiLeftToRespawn = numAiToRespawn;
				}
				mainC.mapsMain.Get_AI_Spawn_Point(ref global::Players.Players.players[pID].charP.position, ais[aiID].team, ref global::Players.Players.players[pID].zRotation, global::MainGame.MainGame.maxLocalPlayerSpawnPoint, ais[aiID].checkForEnemy, global::Players.Players.playerRaces[global::Players.Players.players[pID].race].spawnHeight[global::Players.Players.players[pID].type]);
				mainC.playersMain.Player_Respawn_AI((ushort)pID, threadID);
				Reset_AI_Before_Respawn(aiID);
				for (ushort num2 = 0; num2 < ais[aiID].numChildrenAI; num2++)
				{
					ushort num3 = ais[aiID].childrenAI[num2];
					int playerID = ais[num3].playerID;
					mainC.playersMain.Player_Respawn_AI((ushort)playerID, threadID);
					Reset_AI_Before_Respawn((short)num3);
				}
			}
			break;
		case 2:
			global::Players.Players.players[pID].timeBeforeRespawn[uBufferID] -= frameTime;
			if (global::Players.Players.players[pID].timeBeforeRespawn[uBufferID] < 0f)
			{
				mainC.playersMain.Player_Spawn_Time_Over((ushort)pID);
			}
			global::Joints.Joints.Sync_Player_Matrices(pID, global::Rendering.Rendering.rBufferID, uBufferID);
			break;
		case 4:
		{
			if (!ais[aiID].active)
			{
				break;
			}
			ais[aiID].velocityVariation += frameTime * (float)ais[aiID].velocityVariationDirection;
			if (Math.Abs(ais[aiID].velocityVariation) > 0.1f)
			{
				ais[aiID].velocityVariationDirection *= -1;
			}
			bool flag = false;
			float rotX = 0f;
			float rotY = 0f;
			float num4;
			float num11;
			float num8;
			if (ais[aiID].targetID > -1)
			{
				if (ais[aiID].targetVisible)
				{
					num = ais[aiID].targetID;
					aiVec2.v[0] = global::Players.Players.players[num].charP.position.v[0];
					aiVec2.v[1] = global::Players.Players.players[num].charP.position.v[1];
					aiVec2.v[2] = global::Players.Players.players[num].charP.position.v[2];
					num4 = 90f;
					if (global::MainGame.MainGame.angularVelocity[num] != 0f && global::MainGame.MainGame.playerVehicles[pID].velocity != 0f)
					{
						float num5 = (float)Math.PI * 2f / Math.Abs(global::MainGame.MainGame.angularVelocity[num] * 3.2f);
						float num6 = num5 * global::MainGame.MainGame.playerVehicles[num].velocity;
						float num7 = num6 / ((float)Math.PI * 2f);
						Matrix matrix = global::Players.Players.players[num].mv[global::Rendering.Rendering.rBufferID];
						float m = matrix.M11;
						float m2 = matrix.M12;
						num8 = (float)Math.Sqrt(m * m + m2 * m2);
						if (num8 != 0f)
						{
							if (global::MainGame.MainGame.angularVelocity[num] >= 0f)
							{
								num8 *= -1f;
							}
							m /= num8;
							m2 /= num8;
							float num9 = global::Players.Players.players[num].charP.position.v[0] + num7 * m;
							float num10 = global::Players.Players.players[num].charP.position.v[1] + num7 * m2;
							num11 = 0f;
							if (num8 != 0f)
							{
								num11 = (float)Math.Acos(0f - m2);
							}
							if (0f - m > 0f)
							{
								num11 = (float)Math.PI * 2f - num11;
							}
							aiVec1.v[0] = aiVec2.v[0] - global::Players.Players.players[pID].charP.position.v[0];
							aiVec1.v[1] = aiVec2.v[1] - global::Players.Players.players[pID].charP.position.v[1];
							aiVec1.v[2] = aiVec2.v[2] - global::Players.Players.players[pID].charP.position.v[2];
							num8 = (float)Math.Sqrt(aiVec1.v[0] * aiVec1.v[0] + aiVec1.v[1] * aiVec1.v[1]);
							float num12 = num8 / global::MainGame.MainGame.playerVehicles[pID].velocity;
							if (num12 > 0f)
							{
								num11 += global::MainGame.MainGame.angularVelocity[num] * 3.2f * num12;
								aiVec2.v[0] = num9 - (float)Math.Sin(num11) * num7;
								aiVec2.v[1] = num10 + (float)Math.Cos(num11) * num7;
							}
						}
					}
				}
				else
				{
					aiVec2.v[0] = ais[aiID].lastTargetX;
					aiVec2.v[1] = ais[aiID].lastTargetY;
					aiVec2.v[2] = ais[aiID].lastTargetZ;
					num4 = 70f;
				}
			}
			else
			{
				float num12 = global::MainGame.MainGame.MaxLeft + (global::MainGame.MainGame.MaxRight - global::MainGame.MainGame.MaxLeft) / 2f + (float)aiID * 100f;
				float num5 = global::MainGame.MainGame.MaxRear + (global::MainGame.MainGame.MaxForward - global::MainGame.MainGame.MaxRear) / 2f + (float)aiID * 100f;
				aiVec2.v[0] = num12;
				aiVec2.v[1] = num5;
				aiVec2.v[2] = mainC.terrainMain.Get_Terrain_Height(num12, num5, threadID) + 500f;
				num4 = 45f;
			}
			float num13 = global::Players.Players.players[num].charP.position.v[0] - global::Players.Players.players[pID].charP.position.v[0];
			float num14 = global::Players.Players.players[num].charP.position.v[1] - global::Players.Players.players[pID].charP.position.v[1];
			float num15 = global::Players.Players.players[num].charP.position.v[2] - global::Players.Players.players[pID].charP.position.v[2];
			num8 = (float)Math.Sqrt(num13 * num13 + num14 * num14);
			float num16 = 0f;
			if (num8 != 0f)
			{
				num16 = (float)Math.Acos(num14 / num8) * 57.29578f;
			}
			if (num13 > 0f)
			{
				num16 = 360f - num16;
			}
			num16 -= global::Players.Players.players[pID].zRotation;
			aiVec1.v[0] = aiVec2.v[0] - global::Players.Players.players[pID].charP.position.v[0];
			aiVec1.v[1] = aiVec2.v[1] - global::Players.Players.players[pID].charP.position.v[1];
			aiVec1.v[2] = aiVec2.v[2] - global::Players.Players.players[pID].charP.position.v[2];
			num8 = (float)Math.Sqrt(aiVec1.v[0] * aiVec1.v[0] + aiVec1.v[1] * aiVec1.v[1]);
			num11 = 0f;
			if (num8 != 0f)
			{
				num11 = (float)Math.Acos(aiVec1.v[1] / num8) * 57.29578f;
			}
			if (aiVec1.v[0] > 0f)
			{
				num11 = 360f - num11;
			}
			rotX = num11 - global::Players.Players.players[pID].zRotation;
			if (Math.Abs(rotX) < 0.1f)
			{
				rotX = 0f;
			}
			if (rotX > 180f)
			{
				rotX -= 360f;
			}
			else if (rotX < -180f)
			{
				rotX = 360f + rotX;
			}
			if (Math.Abs(rotX) > num4)
			{
				rotX = num4 * (float)Math.Sign(rotX);
			}
			num8 = (float)Math.Sqrt(num13 * num13 + num14 * num14 + num15 * num15);
			num11 = 0f;
			if (num8 != 0f)
			{
				num11 = (float)Math.Asin(num15 / num8) * 57.29578f;
			}
			rotY = num11 - (float)Math.Atan(global::Players.Players.players[pID].mv[uBufferID].M23) * 57.29578f;
			if (Math.Abs(rotY) < 0.1f)
			{
				rotY = 0f;
			}
			else if (rotY > 60f)
			{
				rotY = 60f;
			}
			if (global::Players.Players.players[pID].needToReload)
			{
				mainC.playersMain.Player_Needs_To_Reload((ushort)pID);
				global::Players.Players.players[pID].needToReload = false;
				global::Players.Players.players[pID].needToChamber = false;
			}
			else if (global::Players.Players.players[pID].needToChamber)
			{
				mainC.playersMain.Player_Needs_To_Chamber((ushort)pID);
				global::Players.Players.players[pID].needToChamber = false;
			}
			if (!flag)
			{
				Matrix.CreateRotationZ(global::Players.Players.players[pID].zRotation * ((float)Math.PI / 180f), out global::Players.Players.players[pID].mv[uBufferID]);
			}
			mainC.playersMain.Move_AI_Player_SpaceShip((byte)pID, aiID, rotX, rotY, frameTime, threadID);
			global::Joints.Joints.Reset_Joint_Data(pID);
			global::Joints.Joints.Process_Joints_Threaded(pID, frameTime, threadID);
			num13 = global::Players.Players.players[num].charP.position.v[0] - global::Players.Players.players[pID].charP.position.v[0];
			num14 = global::Players.Players.players[num].charP.position.v[1] - global::Players.Players.players[pID].charP.position.v[1];
			num15 = global::Players.Players.players[num].charP.position.v[2] - global::Players.Players.players[pID].charP.position.v[2];
			byte objectID = global::MainGame.MainGame.playerVehicles[pID].mounts[global::MainGame.MainGame.primaryWeaponMount].objectID;
			if (ais[aiID].targetInRange && Math.Abs(rotX) < 10f && Math.Abs(rotY) < 10f)
			{
				ais[aiID].fireTimeRemaining -= frameTime / global::Physics.Physics.timeMod;
				global::Players.Players.players[pID].shooting = false;
				global::MainGame.MainGame.playerVehicles[pID].weapons[objectID].shooting = false;
				ais[aiID].fireTimeRemaining = -1f;
				num16 = 0f;
				num8 = (float)Math.Sqrt(num13 * num13 + num14 * num14 + num15 * num15);
				if (num8 != 0f)
				{
					num13 /= num8;
					num14 /= num8;
					num15 /= num8;
					num16 = num13 * global::Players.Players.players[pID].mv[uBufferID].M21 + num14 * global::Players.Players.players[pID].mv[uBufferID].M22 + num15 * global::Players.Players.players[pID].mv[uBufferID].M23;
				}
				if (num16 > 0f && Math.Sin(Math.Acos(num16)) * (double)num8 < 40.0)
				{
					ais[aiID].fireTimeRemaining = ais[aiID].firingTimeAdjusted;
					global::Players.Players.players[pID].shooting = true;
					global::MainGame.MainGame.playerVehicles[pID].weapons[objectID].shooting = true;
				}
				if (!global::Players.Players.players[pID].shooting)
				{
					mainC.weaponsMain.firingStopped((ushort)pID, objectID);
				}
			}
			else
			{
				ais[aiID].fireTimeRemaining = -1f;
				global::Players.Players.players[pID].shooting = false;
				global::MainGame.MainGame.playerVehicles[pID].weapons[objectID].shooting = false;
				mainC.weaponsMain.firingStopped((ushort)pID, objectID);
			}
			for (ushort num2 = 0; num2 < ais[aiID].numChildrenAI; num2++)
			{
				ushort num3 = ais[aiID].childrenAI[num2];
				int playerID = ais[num3].playerID;
				ref Matrix reference = ref global::Players.Players.players[playerID].mv[uBufferID];
				reference = ais[num3].hostMatrix * Matrix.CreateTranslation(ais[num3].x, ais[num3].y, ais[num3].z) * global::Players.Players.players[pID].mv[uBufferID];
				global::MainGame.MainGame.playerVehicles[playerID].ph1.x = global::Players.Players.players[pID].charP.position.v[0] + global::Players.Players.players[playerID].mv[uBufferID].M41;
				global::MainGame.MainGame.playerVehicles[playerID].ph1.y = global::Players.Players.players[pID].charP.position.v[1] + global::Players.Players.players[playerID].mv[uBufferID].M42;
				global::MainGame.MainGame.playerVehicles[playerID].ph1.z = global::Players.Players.players[pID].charP.position.v[2] + global::Players.Players.players[playerID].mv[uBufferID].M43;
				global::Players.Players.players[playerID].charP.position.v[0] = global::MainGame.MainGame.playerVehicles[playerID].ph1.x;
				global::Players.Players.players[playerID].charP.position.v[1] = global::MainGame.MainGame.playerVehicles[playerID].ph1.y;
				global::Players.Players.players[playerID].charP.position.v[2] = global::MainGame.MainGame.playerVehicles[playerID].ph1.z;
				global::Players.Players.players[playerID].mv[uBufferID].M41 = 0f;
				global::Players.Players.players[playerID].mv[uBufferID].M42 = 0f;
				global::Players.Players.players[playerID].mv[uBufferID].M43 = 0f;
			}
			break;
		}
		case 8:
		{
			for (ushort num2 = 0; num2 < ais[aiID].numChildrenAI; num2++)
			{
				int playerID = ais[ais[aiID].childrenAI[num2]].playerID;
				global::Players.Players.players[playerID].onmap = 1;
			}
			global::Joints.Joints.Reset_Joint_Data(pID);
			global::Joints.Joints.Process_Joints_Threaded(pID, frameTime, threadID);
			float rotX = -5f;
			float rotY = -20f;
			mainC.playersMain.Move_AI_Player_SpaceShip((byte)pID, aiID, rotX, rotY, frameTime, threadID);
			break;
		}
		}
		if ((global::Players.Players.players[pID].teamMask & global::Players.Players.enemyTeamMask) != 0 && global::Players.Players.players[pID].onmap == 4)
		{
			currentActiveEnemyAI++;
		}
	}

	public void Separate_AI_Players()
	{
		for (short num = 0; num < currentAI; num++)
		{
			if (!ais[num].stationary && ais[num].locallyControlled)
			{
				short playerID = ais[num].playerID;
				if (playerID > -1 && global::Players.Players.players[playerID].onmap == 4 && Vehicles.vehicles[global::Players.Players.players[playerID].curVehicle].type == 0)
				{
					float num2 = ais[num].speed * ais[num].randomFactor;
					float num3 = global::Players.Players.players[playerID].playerBoudingRadius * 2f;
					for (short num4 = (short)(playerID + 1); num4 < global::MainGame.MainGame.maxGamePlayers; num4++)
					{
						if ((global::Players.Players.players[num4].onmap & 0xC) != 0)
						{
							float num5 = global::Players.Players.players[playerID].charP.position.v[0] - global::Players.Players.players[num4].charP.position.v[0];
							float num6 = global::Players.Players.players[playerID].charP.position.v[1] - global::Players.Players.players[num4].charP.position.v[1];
							float num7 = global::Players.Players.players[playerID].charP.position.v[2] - global::Players.Players.players[num4].charP.position.v[2];
							float num8 = num5 * num5 + num6 * num6 + num7 * num7;
							if (num8 < global::Players.Players.players[playerID].playerBoudingRadiusTimes2Sqr && (num8 = num5 * num5 + num6 * num6) < global::Players.Players.players[playerID].playerBoudingRadiusTimes2Sqr)
							{
								if (num8 != 0f)
								{
									num8 = (float)Math.Sqrt(num8);
									num5 /= num8;
									num6 /= num8;
								}
								else
								{
									num5 = 1f;
								}
								num8 /= num3;
								float num9 = (2f - 2f * num8) * num2;
								num8 = global::MainGame.MainGame.playerVehicles[playerID].ph1.mass * num9;
								if (global::Players.Players.players[playerID].charP.velocity.v[0] != 0f && Math.Sign(global::Players.Players.players[playerID].charP.velocity.v[0]) != Math.Sign(num5))
								{
									global::Players.Players.players[playerID].impactX += num5 * num8 - global::Players.Players.players[playerID].impactX;
								}
								if (global::Players.Players.players[playerID].charP.velocity.v[1] != 0f && Math.Sign(global::Players.Players.players[playerID].charP.velocity.v[1]) != Math.Sign(num6))
								{
									global::Players.Players.players[playerID].impactY += num6 * num8 - global::Players.Players.players[playerID].impactY;
								}
							}
						}
					}
					for (short num4 = global::MainGame.MainGame.maxHumanGamePlayers; num4 < playerID; num4++)
					{
						if ((global::Players.Players.players[num4].onmap & 0xC) != 0)
						{
							float num5 = global::Players.Players.players[playerID].charP.position.v[0] - global::Players.Players.players[num4].charP.position.v[0];
							float num6 = global::Players.Players.players[playerID].charP.position.v[1] - global::Players.Players.players[num4].charP.position.v[1];
							float num7 = global::Players.Players.players[playerID].charP.position.v[2] - global::Players.Players.players[num4].charP.position.v[2];
							float num8 = num5 * num5 + num6 * num6 + num7 * num7;
							if (num8 < global::Players.Players.players[playerID].playerBoudingRadiusTimes2Sqr && (num8 = num5 * num5 + num6 * num6) < global::Players.Players.players[playerID].playerBoudingRadiusTimes2Sqr)
							{
								num8 = num5 * num5 + num6 * num6;
								if (num8 != 0f)
								{
									num8 = (float)Math.Sqrt(num8);
									num5 /= num8;
									num6 /= num8;
								}
								else
								{
									num5 = 1f;
								}
								num8 /= num3;
								if (num8 < 0.5f)
								{
									num8 = global::MainGame.MainGame.playerVehicles[playerID].ph1.mass * num2;
									global::Players.Players.players[playerID].impactX += num5 * num8;
									global::Players.Players.players[playerID].impactY += num6 * num8;
									ais[num].resetSpeed = true;
								}
								else
								{
									float num9 = (2f - 2f * num8) * num2;
									num8 = global::MainGame.MainGame.playerVehicles[playerID].ph1.mass * num9;
									if (global::Players.Players.players[playerID].charP.velocity.v[0] != 0f && Math.Sign(global::Players.Players.players[playerID].charP.velocity.v[0]) != Math.Sign(num5))
									{
										global::Players.Players.players[playerID].impactX += num8 * num5;
									}
									if (global::Players.Players.players[playerID].charP.velocity.v[1] != 0f && Math.Sign(global::Players.Players.players[playerID].charP.velocity.v[1]) != Math.Sign(num6))
									{
										global::Players.Players.players[playerID].impactY += num6 * num8;
									}
								}
							}
						}
					}
				}
			}
		}
	}

	public void Trigger_Respawn(byte count)
	{
		currentAiRespawnTime = -1f;
		numAiLeftToRespawn = count;
	}

	public void Assign_AI()
	{
		if (numAllocatedNavRoutes < global::MainGame.MainGame.numNavRoutes)
		{
			Create_Nav_Routes();
		}
		for (int i = 0; i < numAI; i++)
		{
			if (ais[i].playerID < 0)
			{
				ais[i].playerID = (short)mainC.playersMain.Find_Vacant_Player(global::MainGame.MainGame.maxHumanGamePlayers);
			}
			if (ais[i].playerID > 0)
			{
				int playerID = ais[i].playerID;
				ais[i].lostTargetTimer = 0f;
				ais[i].randomFactor = 1f + (0f - aiRandomFactor + (float)global::MainGame.MainGame.mainRandom.NextDouble() * 2f * aiRandomFactor);
				ais[i].patrolling = 0;
				mainC.playersMain.Reset_Player((ushort)playerID, isActive: true, ais[i].race, ais[i].raceType);
				_ = Vehicles.vehicles[global::Players.Players.players[playerID].curVehicle].numModels;
				mainC.vehicles.Create_Vehicle_Texture_List(global::Players.Players.players[playerID].curVehicle, out global::Players.Players.players[playerID].textureID);
				if (ais[i].textureID > -1)
				{
					global::Players.Players.players[playerID].textureID[0] = (ushort)ais[i].textureID;
				}
				if (ais[i].textureNormalID > -1)
				{
					global::Players.Players.players[playerID].textureNormalID = ais[i].textureNormalID;
				}
				if (ais[i].textureSpecularID > -1)
				{
					global::Players.Players.players[playerID].textureSpecularID = ais[i].textureSpecularID;
				}
				global::Players.Players.players[playerID].aiID = (byte)i;
				global::Players.Players.players[playerID].active = true;
				global::Players.Players.players[playerID].weaponModifier = 0;
				global::Players.Players.players[playerID].respawnParticle = -1;
				global::Players.Players.players[playerID].charP.position.v[0] = ais[i].x;
				global::Players.Players.players[playerID].charP.position.v[1] = ais[i].y;
				global::Players.Players.players[playerID].charP.position.v[2] = ais[i].z;
				global::Players.Players.players[playerID].charP.velocity.v[0] = 0f;
				global::Players.Players.players[playerID].charP.velocity.v[1] = 0f;
				global::Players.Players.players[playerID].charP.velocity.v[2] = 0f;
				global::Players.Players.players[playerID].charP.angularVelocity.v[0] = 0f;
				global::Players.Players.players[playerID].charP.angularVelocity.v[1] = 0f;
				global::Players.Players.players[playerID].charP.angularVelocity.v[2] = 0f;
				global::Players.Players.players[playerID].zRotation = ais[i].zRotation;
				global::Players.Players.players[playerID].deathTimer = ais[i].deathTimer;
				global::MainGame.MainGame.arcadeModeRotAngle[playerID] = ais[i].zRotation * ((float)Math.PI / 180f);
				global::Players.Players.players[playerID].primaryWeaponMountWeapon = ais[i].weapon;
				mainC.gameLogic.Game_Set_Vehicle_Weapons((ushort)playerID);
				mainC.programsMain.Set_Joints_To_Animation_Start(ref global::Players.Players.players[playerID].jt1, global::Players.Players.players[playerID].programCollection, global::Players.Players.players[playerID].programStationaryLegsBody, (ushort)playerID, 1f);
				mainC.programsMain.Set_Joints_To_Animation_Start(ref global::Players.Players.players[playerID].jt1, global::Players.Players.players[playerID].programCollection, global::Players.Players.players[playerID].programStationaryArms, (ushort)playerID, 1f);
				global::Players.Players.players[playerID].wpnIndex = (sbyte)mainC.weaponsMain.Get_Weapon_Index((ushort)playerID, (byte)ais[i].weapon);
				global::Players.Players.players[playerID].team = ais[i].team;
				global::Players.Players.players[playerID].teamMask = mainC.playersMain.Get_Team_Mask(ais[i].team);
				global::Players.Players.players[playerID].maxDamage = ais[i].maxDamage;
				mainC.playersMain.Adjust_Player_Damage_To_Fixed_Amount((ushort)playerID, ais[i].damage, sendOnline: false);
				global::Players.Players.players[playerID].onmap = ais[i].onmap;
				global::Players.Players.players[playerID].dead = false;
				if (ais[i].onmap == 1)
				{
					global::Players.Players.players[playerID].dead = true;
				}
				else if (ais[i].onmap == 4)
				{
					ais[i].active = true;
				}
				global::Players.Players.players[playerID].weapon1.jointID = (sbyte)global::Players.Players.players[playerID].weaponJoint;
				global::Players.Players.players[playerID].weapon1.jointRID = ais[i].weaponJointR;
				global::Players.Players.players[playerID].weapon1.jointEID = ais[i].weaponJointE;
				global::Players.Players.players[playerID].xRotation = ais[i].xRotation;
				global::Players.Players.players[playerID].damageType = ais[i].damageType;
				global::Players.Players.players[playerID].velocityTerminal = ais[i].velocityTerminal;
				global::Players.Players.players[playerID].velocityTerminalThreshold = ais[i].velocityTerminalThreshold;
				mainC.weaponsMain.Update_Player_Weapon_Info((byte)playerID);
				global::Players.Players.players[playerID].username = aiName + playerID;
				mainC.playersMain.Set_Player_Abbreviated_Name((ushort)playerID);
			}
			else
			{
				ais[i].active = false;
			}
		}
		mainC.playersMain.Verify_Player_Team_Counts();
		if (numAllocatedNavRoutes < global::MainGame.MainGame.numNavRoutes)
		{
			Create_Nav_Routes();
		}
	}

	public int Assign_AI_Single(ushort aiID)
	{
		if (ais[aiID].playerID < 0)
		{
			ais[aiID].playerID = (short)mainC.playersMain.Find_Vacant_Player(global::MainGame.MainGame.maxHumanGamePlayers);
		}
		if (ais[aiID].playerID >= global::MainGame.MainGame.maxHumanGamePlayers)
		{
			int playerID = ais[aiID].playerID;
			ais[aiID].lostTargetTimer = 0f;
			ais[aiID].patrolling = 0;
			mainC.playersMain.Reset_Player((ushort)playerID, isActive: true, ais[aiID].race, ais[aiID].raceType);
			mainC.vehicles.Create_Vehicle_Texture_List(global::Players.Players.players[playerID].curVehicle, out global::Players.Players.players[playerID].textureID);
			if (ais[aiID].textureID > -1)
			{
				global::Players.Players.players[playerID].textureID[0] = (ushort)ais[aiID].textureID;
			}
			if (ais[aiID].textureNormalID > -1)
			{
				global::Players.Players.players[playerID].textureNormalID = ais[aiID].textureNormalID;
			}
			if (ais[aiID].textureSpecularID > -1)
			{
				global::Players.Players.players[playerID].textureSpecularID = ais[aiID].textureSpecularID;
			}
			global::Players.Players.players[playerID].aiID = (byte)aiID;
			global::Players.Players.players[playerID].active = true;
			global::Players.Players.players[playerID].weaponModifier = 0;
			global::Players.Players.players[playerID].respawnParticle = -1;
			global::Players.Players.players[playerID].charP.position.v[0] = ais[aiID].x;
			global::Players.Players.players[playerID].charP.position.v[1] = ais[aiID].y;
			global::Players.Players.players[playerID].charP.position.v[2] = ais[aiID].z;
			global::Players.Players.players[playerID].charP.velocity.v[0] = 0f;
			global::Players.Players.players[playerID].charP.velocity.v[1] = 0f;
			global::Players.Players.players[playerID].charP.velocity.v[2] = 0f;
			global::Players.Players.players[playerID].charP.angularVelocity.v[0] = 0f;
			global::Players.Players.players[playerID].charP.angularVelocity.v[1] = 0f;
			global::Players.Players.players[playerID].charP.angularVelocity.v[2] = 0f;
			global::Players.Players.players[playerID].zRotation = ais[aiID].zRotation;
			global::Players.Players.players[playerID].deathTimer = ais[aiID].deathTimer;
			global::MainGame.MainGame.arcadeModeRotAngle[playerID] = ais[aiID].zRotation * ((float)Math.PI / 180f);
			global::Players.Players.players[playerID].primaryWeaponMountWeapon = ais[aiID].weapon;
			mainC.gameLogic.Game_Set_Vehicle_Weapons((ushort)playerID);
			global::Players.Players.players[playerID].wpnIndex = (sbyte)mainC.weaponsMain.Get_Weapon_Index((ushort)playerID, (byte)ais[aiID].weapon);
			global::Players.Players.players[playerID].team = ais[aiID].team;
			global::Players.Players.players[playerID].teamMask = mainC.playersMain.Get_Team_Mask(ais[aiID].team);
			global::Players.Players.players[playerID].maxDamage = ais[aiID].maxDamage;
			mainC.playersMain.Adjust_Player_Damage_To_Fixed_Amount((ushort)playerID, ais[aiID].damage, sendOnline: false);
			global::Players.Players.players[playerID].onmap = ais[aiID].onmap;
			global::Players.Players.players[playerID].dead = false;
			if (ais[aiID].onmap == 1)
			{
				global::Players.Players.players[playerID].dead = true;
			}
			else if (ais[aiID].onmap == 4)
			{
				ais[aiID].active = true;
			}
			global::Players.Players.players[playerID].weapon1.jointID = (sbyte)global::Players.Players.players[playerID].weaponJoint;
			global::Players.Players.players[playerID].weapon1.jointRID = ais[aiID].weaponJointR;
			global::Players.Players.players[playerID].weapon1.jointEID = ais[aiID].weaponJointE;
			global::Players.Players.players[playerID].xRotation = ais[aiID].xRotation;
			global::Players.Players.players[playerID].damageType = ais[aiID].damageType;
			global::Players.Players.players[playerID].velocityTerminal = ais[aiID].velocityTerminal;
			global::Players.Players.players[playerID].velocityTerminalThreshold = ais[aiID].velocityTerminalThreshold;
			mainC.weaponsMain.Update_Player_Weapon_Info((byte)playerID);
			global::Players.Players.players[playerID].username = aiName + playerID;
			mainC.playersMain.Set_Player_Abbreviated_Name((ushort)playerID);
			return playerID;
		}
		ais[aiID].active = false;
		return -1;
	}

	public void Handle_Main_Player_Dying(byte threadID)
	{
		if (resetAIOnMainPlayerDeath)
		{
			Reset_Round(threadID);
		}
	}

	public void Create_Nav_Routes()
	{
		if (numAllocatedNavRoutes < global::MainGame.MainGame.numNavRoutes)
		{
			for (ushort num = 0; num < numAI; num++)
			{
				ais[num].aiRoute.NavMeshRoute = new Vector3[global::MainGame.MainGame.numNavRoutes];
			}
		}
		numAllocatedNavRoutes = global::MainGame.MainGame.numNavRoutes;
	}

	public void Reset_Round(byte threadID)
	{
		hostAuthorizedSpawn = 0;
		aiToCheck = 0;
		aiCheckTimer = 0f;
		aiCompleted = false;
		sendAIRoute = false;
		aiRouteToSend = 0;
		curNetworkAI = 0;
		curRemotePlayer = 0;
		roundOverTimer = 0f;
		lastEnemyAiCount = 0;
		numTotalAiSpawned = 0;
		numAiRespawnedInWave = 0;
		numAiInWave = 0;
		currentAiWave = 0;
		currentAI = numAI;
		aiSpawnPoint = 0;
		aiRespawnTimer = levelRespawnTimer;
		if (numAiWaves > 0)
		{
			numAiInWave = waveRespawnCount[0];
			aiRespawnTimer = waveRespawnTime[0];
		}
		currentAiRespawnTime = aiRespawnTimerStartValue;
		numAiLeftToRespawn = numAiToRespawn;
		Assign_AI();
		curProcAI = 0;
		routingInProcess = false;
		bossLevelComplete = false;
		float num = global::MainGame.MainGame.MaxRight - global::MainGame.MainGame.MaxLeft;
		float num2 = global::MainGame.MainGame.MaxForward - global::MainGame.MainGame.MaxRear;
		maxAIRoutingDistanceSqr = num * num + num2 * num2;
		if (global::MainGame.MainGame.gameMode != 0 || global::InputHandler.InputHandler.newSPGame)
		{
			levelKillCount = 0;
		}
		for (short num3 = 0; num3 < currentAI; num3++)
		{
			if (ais[num3].playerID > -1)
			{
				mpData[num3].lastTarget = -1;
				short playerID = ais[num3].playerID;
				ais[num3].active = false;
				ais[num3].canFire = false;
				ais[num3].patrolling = 0;
				ais[num3].resetSpeed = false;
				ais[num3].lostTargetTimer = 0f;
				ais[num3].needsRoute = false;
				ais[num3].updateAIRoute = false;
				ais[num3].fireTimeRemaining = 0f;
				ais[num3].targetID = -1;
				ais[num3].targetVisible = false;
				ais[num3].targetInRange = false;
				ais[num3].state = byte.MaxValue;
				ais[num3].stepOver = 0;
				ais[num3].colDir = 0;
				ais[num3].aiRoute.numPts = 0;
				ais[num3].aiRoute.routeError = false;
				ais[num3].goalDistance = 0f;
				ais[num3].velocityVariation = (-0.1f + (float)global::MainGame.MainGame.mainRandom.Next(0, 32000) / 160000f * 0.2f) * 1f;
				ais[num3].velocityVariationDirection = 1;
				if (ais[num3].velocityVariation < 0f)
				{
					ais[num3].velocityVariationDirection = -1;
				}
				global::Players.Players.players[playerID].active = true;
				global::Players.Players.players[playerID].aiID = (byte)num3;
				global::Players.Players.players[playerID].respawnParticle = -1;
				global::Players.Players.players[playerID].zRotation = ais[num3].zRotation;
				global::MainGame.MainGame.arcadeModeRotAngle[playerID] = ais[num3].zRotation * ((float)Math.PI / 180f);
				global::Players.Players.players[playerID].charP.position.v[0] = (ais[num3].positionGoalX = (ais[num3].targetX = (ais[num3].lastTargetX = (ais[num3].goalX = ais[num3].x))));
				global::Players.Players.players[playerID].charP.position.v[1] = (ais[num3].positionGoalY = (ais[num3].targetY = (ais[num3].lastTargetY = (ais[num3].goalY = ais[num3].y))));
				global::Players.Players.players[playerID].charP.position.v[2] = (ais[num3].positionGoalZ = (ais[num3].targetZ = (ais[num3].lastTargetZ = (ais[num3].goalZ = ais[num3].z))));
				global::Players.Players.players[playerID].velX = 0f;
				global::Players.Players.players[playerID].velY = 0f;
				global::Players.Players.players[playerID].velZ = 0f;
				global::Players.Players.players[playerID].voiceCueID = -1;
				global::Players.Players.players[playerID].shootingAccuracy = debugAccuracy;
				global::Players.Players.players[playerID].team = ais[num3].team;
				global::Players.Players.players[playerID].teamMask = mainC.playersMain.Get_Team_Mask(ais[num3].team);
				global::Players.Players.players[playerID].maxDamage = ais[num3].maxDamage;
				mainC.playersMain.Adjust_Player_Damage_To_Fixed_Amount((ushort)playerID, ais[num3].damage, sendOnline: false);
				global::Players.Players.players[playerID].onmap = ais[num3].onmap;
				global::Players.Players.players[playerID].dead = false;
				if (ais[num3].onmap == 1)
				{
					global::Players.Players.players[playerID].dead = true;
				}
				else if (ais[num3].onmap == 4)
				{
					ais[num3].active = true;
				}
				global::Players.Players.players[playerID].primaryWeaponMountWeapon = ais[num3].weapon;
				if ((ais[num3].onmap & 6) > 0)
				{
					mainC.weaponsMain.Reset_Weapons_For_Respawn((byte)playerID);
				}
				global::Players.Players.players[playerID].weapon1.jointID = (sbyte)global::Players.Players.players[playerID].weaponJoint;
				global::Players.Players.players[playerID].weapon1.jointRID = ais[num3].weaponJointR;
				global::Players.Players.players[playerID].weapon1.jointEID = ais[num3].weaponJointE;
				mainC.weaponsMain.firingStoppedAllPlayerWeapons((ushort)playerID);
				for (short num4 = 0; num4 < 10; num4++)
				{
					global::Players.Players.players[playerID].particles[num4] = -1;
				}
				mainC.programsMain.Reset_Programs(ref global::Players.Players.players[playerID].pg1, ref global::Players.Players.players[playerID].animations, global::Players.Players.players[playerID].programCollection);
				if (global::Players.Players.players[playerID].programStationaryArms < global::Programs.Programs.pgC[global::Players.Players.players[playerID].programCollection].numPrograms)
				{
					mainC.programsMain.Start_Animation((ushort)playerID, ref global::Players.Players.players[playerID].jt1, ref global::Players.Players.players[playerID].animations, global::Players.Players.players[playerID].programCollection, global::Players.Players.players[playerID].programStationaryArms, 1f, 1f);
				}
				if (global::Players.Players.players[playerID].programStationaryLegsBody < global::Programs.Programs.pgC[global::Players.Players.players[playerID].programCollection].numPrograms)
				{
					mainC.programsMain.Start_Animation((ushort)playerID, ref global::Players.Players.players[playerID].jt1, ref global::Players.Players.players[playerID].animations, global::Players.Players.players[playerID].programCollection, global::Players.Players.players[playerID].programStationaryLegsBody, 1f, 1f);
				}
				mainC.jointsMain.Update_Joints_For_New_Position(playerID);
				if (global::Weapons.Weapons.wp1[global::Players.Players.players[playerID].primaryWeaponMountWeapon].AnimationHolding < global::Programs.Programs.pgC[global::Players.Players.players[playerID].programCollection].numPrograms)
				{
					mainC.programsMain.Start_Animation((ushort)playerID, ref global::Players.Players.players[playerID].jt1, ref global::Players.Players.players[playerID].animations, global::Players.Players.players[playerID].programCollection, global::Weapons.Weapons.wp1[global::Players.Players.players[playerID].primaryWeaponMountWeapon].AnimationHolding, 1f, 1f);
				}
				global::Joints.Joints.Reset_Joint_Data(playerID);
				global::Joints.Joints.Process_Joints_Threaded(playerID, 0.016667f, threadID);
				mainC.vehicles.Reset_Player_Vehicle_Variables((ushort)playerID);
				mainC.vehicles.Set_Vehicle_Position(ref global::MainGame.MainGame.playerVehicles[playerID], global::Players.Players.players[playerID].charP.position.v[0], global::Players.Players.players[playerID].charP.position.v[1], global::Players.Players.players[playerID].charP.position.v[2], 0f, 0f, global::Players.Players.players[playerID].zRotation * ((float)Math.PI / 180f));
			}
			else
			{
				ais[num3].active = false;
			}
		}
		Reset_Bosses();
	}

	public void Reset_AI_Before_Respawn(short x)
	{
		mpData[x].lastTarget = -1;
		short playerID = ais[x].playerID;
		ais[x].active = false;
		ais[x].canFire = false;
		ais[x].fireTimeRemaining = 0f;
		ais[x].targetID = -1;
		ais[x].targetVisible = false;
		ais[x].targetInRange = false;
		ais[x].state = byte.MaxValue;
		ais[x].patrolling = 0;
		ais[x].stepOver = 0;
		ais[x].colDir = 0;
		ais[x].speakingTime = aiSpeakingTime + aiSpeakingTimeRandom * (float)global::MainGame.MainGame.mainRandom.NextDouble();
		ais[x].positionGoalX = (ais[x].targetX = (ais[x].lastTargetX = (ais[x].goalX = global::Players.Players.players[playerID].charP.position.v[0])));
		ais[x].positionGoalY = (ais[x].targetY = (ais[x].lastTargetY = (ais[x].goalY = global::Players.Players.players[playerID].charP.position.v[1])));
		ais[x].positionGoalZ = (ais[x].targetZ = (ais[x].lastTargetZ = (ais[x].goalZ = global::Players.Players.players[playerID].charP.position.v[2])));
		global::Players.Players.players[playerID].shootingAccuracy = debugAccuracy;
		global::Players.Players.players[playerID].team = ais[x].team;
		global::Players.Players.players[playerID].teamMask = mainC.playersMain.Get_Team_Mask(ais[x].team);
		global::Players.Players.players[playerID].maxDamage = ais[x].maxDamage;
		global::Players.Players.players[playerID].primaryWeaponMountWeapon = ais[x].weapon;
		global::Players.Players.players[playerID].weapon1.jointID = (sbyte)global::Players.Players.players[playerID].weaponJoint;
		global::Players.Players.players[playerID].weapon1.jointRID = ais[x].weaponJointR;
		global::Players.Players.players[playerID].weapon1.jointEID = ais[x].weaponJointE;
		mainC.weaponsMain.firingStoppedAllPlayerWeapons((ushort)playerID);
		ais[x].updateAIRoute = false;
		ais[x].needsRoute = false;
		ais[x].aiRoute.numPts = 0;
		ais[x].aiRoute.curPt = 0;
		ais[x].aiRoute.routeError = false;
		ais[x].resetSpeed = false;
		global::Players.Players.players[playerID].posX[0] = global::Players.Players.players[playerID].charP.position.v[0];
		global::Players.Players.players[playerID].posY[0] = global::Players.Players.players[playerID].charP.position.v[1];
		global::Players.Players.players[playerID].posZ[0] = global::Players.Players.players[playerID].charP.position.v[2];
		global::Players.Players.players[playerID].posX[1] = global::Players.Players.players[playerID].posX[0];
		global::Players.Players.players[playerID].posY[1] = global::Players.Players.players[playerID].posY[0];
		global::Players.Players.players[playerID].posZ[1] = global::Players.Players.players[playerID].posZ[0];
		mpData[x].dataThisRound = false;
		mpData[x].lastUpdate = -1L;
		mpData[x].currentPosX = global::Players.Players.players[playerID].charP.position.v[0];
		mpData[x].currentPosY = global::Players.Players.players[playerID].charP.position.v[1];
		mpData[x].currentPosZ = global::Players.Players.players[playerID].charP.position.v[2];
		mpData[x].mv = Matrix.Identity;
	}

	public void Clear_AI()
	{
		bossLevel = false;
		numBosses = 0;
		currentAI = 0;
		for (ushort num = 0; num < numAI; num++)
		{
			if (ais[num].playerID > -1)
			{
				global::Players.Players.players[ais[num].playerID].active = false;
			}
			ais[num].controllingPlayer = global::Players.Players.players[0].id;
			ais[num].locallyControlled = true;
			ais[num].authorizedToRespawn = false;
		}
		for (ushort num = global::MainGame.MainGame.maxHumanGamePlayers; num < global::MainGame.MainGame.maxGamePlayers; num++)
		{
			global::Players.Players.players[num].active = false;
			global::Players.Players.players[num].onmap = 1;
		}
	}

	public void Change_AI_Difficulty(byte difficulty)
	{
		float num = 1f;
		float num2 = 1f;
		switch (difficulty)
		{
		case 0:
			num = 1f;
			num2 = 1f;
			break;
		case 1:
			num = 1.5f;
			num2 = 0.75f;
			break;
		case 2:
			num = 2f;
			num2 = 0.5f;
			break;
		}
		for (int i = 0; i < currentAI; i++)
		{
			ais[i].firingTimeAdjusted = num * ais[i].firingTime;
			ais[i].timeBetweenFiringAdjusted = num2 * ais[i].timeBetweenFiring;
		}
	}

	public void Trigger_AI_Wave(ushort aiWaveID)
	{
		if (aiWaveID < numAiWaves)
		{
			currentAiWave = aiWaveID;
			numAiRespawnedInWave = 0;
			numAiInWave = waveRespawnCount[aiWaveID];
			aiRespawnTimer = waveRespawnTime[aiWaveID];
			if (aiWaveID >= numAiWaves - 1 && numAiKillsForLevelToEnd > 0 && (float)(numTotalAiSpawned + numAiInWave) < (float)(int)numAiKillsForLevelToEnd * killCountScale)
			{
				numAiInWave = (ushort)((float)(int)numAiKillsForLevelToEnd * killCountScale - (float)(int)numTotalAiSpawned);
			}
		}
	}

	public void Check_AI_To_See_If_Fell_Through_Map(float height)
	{
		if (numAI >= 1)
		{
			if (aiToCheck >= numAI)
			{
				aiToCheck = 0;
			}
			int playerID;
			if ((playerID = ais[aiToCheck].playerID) > -1 && ais[aiToCheck].locallyControlled && global::Players.Players.players[playerID].onmap == 4 && global::Players.Players.players[playerID].charP.position.v[2] < height)
			{
				mainC.mapsMain.Get_AI_Spawn_Point(ref global::Players.Players.players[playerID].charP.position, global::Players.Players.players[playerID].team, ref global::Players.Players.players[playerID].zRotation, global::MainGame.MainGame.maxLocalPlayerSpawnPoint, checkForEnemy: false, global::Players.Players.playerRaces[global::Players.Players.players[playerID].race].spawnHeight[global::Players.Players.players[playerID].type]);
			}
			aiToCheck++;
		}
	}

	public void Target_AI(byte threadID)
	{
		byte uBufferID = global::Rendering.Rendering.uBufferID;
		float length = 0f;
		float num = 0f;
		float num2 = 0f;
		if (currentAI < 1)
		{
			return;
		}
		curProcAI++;
		if (curProcAI >= currentAI)
		{
			curProcAI = 0;
		}
		while (curProcAI < currentAI && !ais[curProcAI].locallyControlled)
		{
			curProcAI++;
		}
		if (curProcAI >= currentAI)
		{
			curProcAI = 0;
		}
		if (!ais[curProcAI].locallyControlled)
		{
			return;
		}
		short playerID = ais[curProcAI].playerID;
		if (playerID < 0 || global::Players.Players.players[playerID].onmap != 4)
		{
			return;
		}
		if (ais[curProcAI].patrolling == 0)
		{
			ais[curProcAI].targetX = global::Players.Players.players[playerID].charP.position.v[0];
			ais[curProcAI].targetY = global::Players.Players.players[playerID].charP.position.v[1];
			ais[curProcAI].targetZ = global::Players.Players.players[playerID].charP.position.v[2] - global::Players.Players.playerRaces[global::Players.Players.players[playerID].race].spawnHeight[global::Players.Players.players[playerID].type];
		}
		ais[curProcAI].goalDistance = 0f;
		float cosFov = ais[curProcAI].cosFov;
		short targetID = ais[curProcAI].targetID;
		byte targetMode = ais[curProcAI].targetMode;
		bool flag = false;
		float angle;
		short num4;
		switch (targetMode)
		{
		case 0:
		{
			float num5 = 0f;
			int num6 = 180;
			short num7 = targetID;
			if (num7 > -1)
			{
				if (!global::Players.Players.players[num7].dead && global::Players.Players.players[num7].active)
				{
					flag = Is_Target_Visible((ushort)playerID, (ushort)num7, out length, threadID);
					ais[curProcAI].goalDistance = length;
					if (flag)
					{
						aiVec4.v[0] = global::Players.Players.players[num7].charP.position.v[0];
						aiVec4.v[1] = global::Players.Players.players[num7].charP.position.v[1];
						aiVec4.v[2] = global::Players.Players.players[num7].charP.position.v[2];
						aiVec3.v[0] = aiVec4.v[0] - global::Players.Players.players[playerID].charP.position.v[0];
						aiVec3.v[1] = aiVec4.v[1] - global::Players.Players.players[playerID].charP.position.v[1];
						aiVec3.v[2] = aiVec4.v[2] - global::Players.Players.players[playerID].charP.position.v[2];
						num5 = (float)Math.Sqrt(aiVec3.v[0] * aiVec3.v[0] + aiVec3.v[1] * aiVec3.v[1] + aiVec3.v[2] * aiVec3.v[2]);
						length = (float)Math.Sqrt(aiVec3.v[0] * aiVec3.v[0] + aiVec3.v[1] * aiVec3.v[1]);
						if (length != 0f)
						{
							aiVec3.v[0] /= length;
							aiVec3.v[1] /= length;
						}
						angle = aiVec3.v[0] * global::Players.Players.players[playerID].mv[uBufferID].M21 + aiVec3.v[1] * global::Players.Players.players[playerID].mv[uBufferID].M22;
						if (Math.Abs(angle) > 1f)
						{
							angle = Math.Sign(angle);
						}
						num6 = (int)(float)Math.Acos(angle);
						num = aiVec3.v[0];
						num2 = aiVec3.v[1];
					}
				}
				else
				{
					num7 = -1;
					flag = false;
				}
			}
			short num3 = 0;
			do
			{
				num3 = global::Players.Players.Find_Next_Player_Not_On_This_Team(num3, ais[curProcAI].team);
				if (num3 <= -1)
				{
					continue;
				}
				num4 = num3++;
				if (global::Players.Players.players[num4].onmap != 4 || num4 == num7)
				{
					continue;
				}
				aiVec4.v[0] = global::Players.Players.players[num4].charP.position.v[0];
				aiVec4.v[1] = global::Players.Players.players[num4].charP.position.v[1];
				aiVec4.v[2] = global::Players.Players.players[num4].charP.position.v[2];
				aiVec3.v[0] = aiVec4.v[0] - global::Players.Players.players[playerID].charP.position.v[0];
				aiVec3.v[1] = aiVec4.v[1] - global::Players.Players.players[playerID].charP.position.v[1];
				aiVec3.v[2] = aiVec4.v[2] - global::Players.Players.players[playerID].charP.position.v[2];
				float num8 = (float)Math.Sqrt(aiVec3.v[0] * aiVec3.v[0] + aiVec3.v[1] * aiVec3.v[1] + aiVec3.v[2] * aiVec3.v[2]);
				if (num8 != 0f)
				{
					aiVec3.v[0] /= num8;
					aiVec3.v[1] /= num8;
					aiVec3.v[2] /= num8;
				}
				angle = aiVec3.v[0] * global::Players.Players.players[playerID].mv[uBufferID].M21 + aiVec3.v[1] * global::Players.Players.players[playerID].mv[uBufferID].M22 + aiVec3.v[2] * global::Players.Players.players[playerID].mv[uBufferID].M23;
				if (angle >= cosFov && num8 < ais[curProcAI].targetCanBeSeenDistance && (num7 < 0 || num8 <= num5 || !flag))
				{
					aiVec3.v[0] = aiVec4.v[0] - global::Players.Players.players[playerID].charP.position.v[0];
					aiVec3.v[1] = aiVec4.v[1] - global::Players.Players.players[playerID].charP.position.v[1];
					length = (float)Math.Sqrt(aiVec3.v[0] * aiVec3.v[0] + aiVec3.v[1] * aiVec3.v[1]);
					if (length != 0f)
					{
						aiVec3.v[0] /= length;
						aiVec3.v[1] /= length;
					}
					angle = aiVec3.v[0] * global::Players.Players.players[playerID].mv[uBufferID].M21 + aiVec3.v[1] * global::Players.Players.players[playerID].mv[uBufferID].M22;
					if (Math.Abs(angle) > 1f)
					{
						angle = Math.Sign(angle);
					}
					int num9 = (int)(float)Math.Acos(angle);
					if ((!flag || num8 < 0.8f * num5 || (num9 < num6 && num9 >= 0) || (num9 > num6 && num9 < 0)) && Is_Target_Visible((ushort)playerID, (ushort)num4, out length, threadID))
					{
						num7 = num4;
						num6 = num9;
						num5 = num8;
						num = aiVec3.v[0];
						num2 = aiVec3.v[1];
						flag = true;
						ais[curProcAI].goalDistance = length;
					}
				}
			}
			while (num3 != -1);
			ais[curProcAI].targetID = num7;
			break;
		}
		case 1:
		{
			if (ais[curProcAI].targetID >= 0)
			{
				break;
			}
			short num3 = 0;
			do
			{
				num3 = global::Players.Players.Find_Next_Player_Not_On_This_Team(num3, ais[curProcAI].team);
				if (num3 <= -1)
				{
					continue;
				}
				num4 = num3;
				num3++;
				if (global::Players.Players.players[num3].onmap == 4)
				{
					_ = global::Players.Players.players[num4].humanoidBackJoint;
					aiVec4.v[0] = global::Players.Players.players[num4].charP.position.v[0];
					aiVec4.v[1] = global::Players.Players.players[num4].charP.position.v[1];
					aiVec4.v[2] = global::Players.Players.players[num4].charP.position.v[2];
					aiVec3.v[0] = aiVec4.v[0] - global::Players.Players.players[playerID].charP.position.v[0];
					aiVec3.v[1] = aiVec4.v[1] - global::Players.Players.players[playerID].charP.position.v[1];
					length = (float)Math.Sqrt(aiVec3.v[0] * aiVec3.v[0] + aiVec3.v[1] * aiVec3.v[1]);
					if (length != 0f)
					{
						aiVec3.v[0] /= length;
						aiVec3.v[1] /= length;
					}
					angle = aiVec3.v[0] * global::Players.Players.players[playerID].mv[uBufferID].M21 + aiVec3.v[1] * global::Players.Players.players[playerID].mv[uBufferID].M22;
					if (angle >= cosFov && length < ais[curProcAI].targetCanBeSeenDistance && Is_Target_Visible((ushort)playerID, (ushort)num4, out length, threadID))
					{
						flag = true;
						ais[curProcAI].targetID = num4;
						ais[curProcAI].goalDistance = length;
						num3 = -1;
					}
				}
			}
			while (num3 != -1);
			break;
		}
		case 2:
			if (global::Players.Players.players[0].dead)
			{
				ais[curProcAI].targetX = (ais[curProcAI].lastTargetX = global::Players.Players.players[playerID].charP.position.v[0]);
				ais[curProcAI].targetY = (ais[curProcAI].lastTargetY = global::Players.Players.players[playerID].charP.position.v[1]);
				ais[curProcAI].targetZ = (ais[curProcAI].lastTargetZ = global::Players.Players.players[playerID].charP.position.v[2]);
				ais[curProcAI].targetID = -1;
				ais[curProcAI].patrolling = 0;
				ais[curProcAI].needsRoute = false;
				ais[curProcAI].aiRoute.numPts = 0;
				global::Players.Players.players[playerID].shooting = false;
				global::Joints.Joints.Sync_Player_Matrices(playerID, global::Rendering.Rendering.rBufferID, global::Rendering.Rendering.uBufferID);
				return;
			}
			aiVec3.v[0] = global::Players.Players.players[0].charP.position.v[0] - global::Players.Players.players[playerID].charP.position.v[0];
			aiVec3.v[1] = global::Players.Players.players[0].charP.position.v[1] - global::Players.Players.players[playerID].charP.position.v[1];
			aiVec3.v[2] = global::Players.Players.players[0].charP.position.v[2] - global::Players.Players.players[playerID].charP.position.v[2];
			ais[curProcAI].lastTargetX = global::Players.Players.players[0].charP.position.v[0];
			ais[curProcAI].lastTargetY = global::Players.Players.players[0].charP.position.v[1];
			ais[curProcAI].lastTargetZ = global::Players.Players.players[0].charP.position.v[2] - global::Players.Players.playerRaces[global::Players.Players.players[0].race].spawnHeight[global::Players.Players.players[0].type] + global::Players.Players.playerRaces[global::Players.Players.players[playerID].race].spawnHeight[global::Players.Players.players[playerID].type];
			length = (float)Math.Sqrt(aiVec3.v[0] * aiVec3.v[0] + aiVec3.v[1] * aiVec3.v[1] + aiVec3.v[2] * aiVec3.v[2]);
			ais[curProcAI].goalDistance = length;
			if (ais[curProcAI].targetID != 0)
			{
				ais[curProcAI].aiRoute.numPts = 0;
				ais[curProcAI].needsRoute = false;
				if ((angle = length - ais[curProcAI].optimalTargetDistance) > 0f)
				{
					ais[curProcAI].needsRoute = true;
				}
				ais[curProcAI].targetX = global::Players.Players.players[0].charP.position.v[0];
				ais[curProcAI].targetY = global::Players.Players.players[0].charP.position.v[1];
				ais[curProcAI].targetZ = global::Players.Players.players[0].charP.position.v[2] - global::Players.Players.playerRaces[global::Players.Players.players[0].race].spawnHeight[global::Players.Players.players[0].type] + global::Players.Players.playerRaces[global::Players.Players.players[playerID].race].spawnHeight[global::Players.Players.players[playerID].type];
			}
			ais[curProcAI].targetID = 0;
			ais[curProcAI].active = true;
			ais[curProcAI].targetZ = ais[curProcAI].lastTargetZ;
			flag = true;
			break;
		case 3:
			if (targetID < 0 || global::Players.Players.players[targetID].dead)
			{
				ais[curProcAI].targetX = (ais[curProcAI].lastTargetX = global::Players.Players.players[playerID].charP.position.v[0]);
				ais[curProcAI].targetY = (ais[curProcAI].lastTargetY = global::Players.Players.players[playerID].charP.position.v[1]);
				ais[curProcAI].targetZ = (ais[curProcAI].lastTargetZ = global::Players.Players.players[playerID].charP.position.v[2]);
				ais[curProcAI].targetID = -1;
				ais[curProcAI].patrolling = 0;
				ais[curProcAI].needsRoute = false;
				ais[curProcAI].aiRoute.numPts = 0;
				global::Players.Players.players[playerID].shooting = false;
				global::Joints.Joints.Sync_Player_Matrices(playerID, global::Rendering.Rendering.rBufferID, global::Rendering.Rendering.uBufferID);
				return;
			}
			aiVec3.v[0] = global::Players.Players.players[targetID].charP.position.v[0] - global::Players.Players.players[playerID].charP.position.v[0];
			aiVec3.v[1] = global::Players.Players.players[targetID].charP.position.v[1] - global::Players.Players.players[playerID].charP.position.v[1];
			aiVec3.v[2] = global::Players.Players.players[targetID].charP.position.v[2] - global::Players.Players.players[playerID].charP.position.v[2];
			ais[curProcAI].lastTargetX = global::Players.Players.players[targetID].charP.position.v[0];
			ais[curProcAI].lastTargetY = global::Players.Players.players[targetID].charP.position.v[1];
			ais[curProcAI].lastTargetZ = global::Players.Players.players[targetID].charP.position.v[2] - global::Players.Players.playerRaces[global::Players.Players.players[targetID].race].spawnHeight[global::Players.Players.players[targetID].type] + global::Players.Players.playerRaces[global::Players.Players.players[playerID].race].spawnHeight[global::Players.Players.players[playerID].type];
			length = (float)Math.Sqrt(aiVec3.v[0] * aiVec3.v[0] + aiVec3.v[1] * aiVec3.v[1] + aiVec3.v[2] * aiVec3.v[2]);
			ais[curProcAI].goalDistance = length;
			if (ais[curProcAI].targetID < 0)
			{
				ais[curProcAI].aiRoute.numPts = 0;
				ais[curProcAI].needsRoute = false;
				if ((angle = length - ais[curProcAI].optimalTargetDistance) > 0f)
				{
					ais[curProcAI].needsRoute = true;
				}
				ais[curProcAI].targetX = global::Players.Players.players[targetID].charP.position.v[0];
				ais[curProcAI].targetY = global::Players.Players.players[targetID].charP.position.v[1];
				ais[curProcAI].targetZ = global::Players.Players.players[targetID].charP.position.v[2] - global::Players.Players.playerRaces[global::Players.Players.players[targetID].race].spawnHeight[global::Players.Players.players[targetID].type] + global::Players.Players.playerRaces[global::Players.Players.players[playerID].race].spawnHeight[global::Players.Players.players[playerID].type];
			}
			else if (ais[curProcAI].aiRoute.numPts < 1)
			{
				ais[curProcAI].needsRoute = true;
			}
			ais[curProcAI].active = true;
			ais[curProcAI].targetZ = ais[curProcAI].lastTargetZ;
			flag = true;
			break;
		case byte.MaxValue:
			if (targetID == -1 || global::Players.Players.players[targetID].onmap != 4)
			{
				ais[curProcAI].targetID = -1;
				return;
			}
			flag = Is_Target_Visible((ushort)playerID, (ushort)targetID, out length, threadID);
			ais[curProcAI].goalDistance = length;
			if (!flag || ais[curProcAI].goalDistance > ais[curProcAI].targetCanBeSeenDistance)
			{
				return;
			}
			break;
		}
		num4 = ais[curProcAI].targetID;
		if (num4 > -1)
		{
			if (flag && ais[curProcAI].goalDistance < ais[curProcAI].targetCanBeSeenDistance)
			{
				if (targetID != num4)
				{
					ais[curProcAI].aiRoute.numPts = 0;
					ais[curProcAI].needsRoute = true;
				}
				ais[curProcAI].lastTargetX = global::Players.Players.players[num4].charP.position.v[0];
				ais[curProcAI].lastTargetY = global::Players.Players.players[num4].charP.position.v[1];
				ais[curProcAI].lastTargetZ = global::Players.Players.players[num4].charP.position.v[2] - global::Players.Players.playerRaces[global::Players.Players.players[num4].race].spawnHeight[global::Players.Players.players[num4].type];
				if (ais[curProcAI].goalDistance * ais[curProcAI].goalDistance > ais[curProcAI].optimalTargetDistanceSqr)
				{
					ais[curProcAI].targetX = ais[curProcAI].lastTargetX;
					ais[curProcAI].targetY = ais[curProcAI].lastTargetY;
					ais[curProcAI].targetZ = ais[curProcAI].lastTargetZ;
					ais[curProcAI].targetInRange = false;
					if (ais[curProcAI].aiRoute.numPts < 1)
					{
						ais[curProcAI].needsRoute = true;
					}
					else
					{
						angle = ais[curProcAI].lastTargetX - ais[curProcAI].aiRoute.endX;
						float num10 = ais[curProcAI].lastTargetY - ais[curProcAI].aiRoute.endY;
						if (angle * angle + num10 * num10 > ais[curProcAI].maxTargetMoveDistanceSqr)
						{
							ais[curProcAI].needsRoute = true;
						}
					}
				}
				else
				{
					ais[curProcAI].targetInRange = true;
					ais[curProcAI].aiRoute.numPts = 0;
					ais[curProcAI].needsRoute = false;
					ais[curProcAI].goalX = global::Players.Players.players[playerID].charP.position.v[0];
					ais[curProcAI].goalY = global::Players.Players.players[playerID].charP.position.v[1];
					ais[curProcAI].targetZ = ais[curProcAI].lastTargetZ;
				}
				ais[curProcAI].active = true;
				ais[curProcAI].patrolling = 0;
				ais[curProcAI].targetVisible = true;
			}
			else if (ais[curProcAI].patrolling == 0)
			{
				if (targetID != num4 && targetID != -1 && !global::Players.Players.players[targetID].dead)
				{
					ais[curProcAI].targetID = targetID;
				}
				if (ais[curProcAI].targetVisible)
				{
					ais[curProcAI].aiRoute.numPts = 0;
					ais[curProcAI].needsRoute = true;
				}
				ais[curProcAI].targetVisible = false;
				ais[curProcAI].targetInRange = false;
				num = ais[curProcAI].lastTargetX - global::Players.Players.players[playerID].charP.position.v[0];
				num2 = ais[curProcAI].lastTargetY - global::Players.Players.players[playerID].charP.position.v[1];
				length = num * num + num2 * num2;
				if (length > global::Players.Players.players[num4].playerBoudingRadiusSqr)
				{
					ais[curProcAI].targetX = ais[curProcAI].lastTargetX;
					ais[curProcAI].targetY = ais[curProcAI].lastTargetY;
					ais[curProcAI].targetZ = ais[curProcAI].lastTargetZ;
				}
				else
				{
					ais[curProcAI].aiRoute.numPts = 0;
					ais[curProcAI].needsRoute = false;
					ais[curProcAI].targetID = -1;
					num4 = -1;
				}
			}
			else
			{
				num4 = -1;
			}
		}
		if (num4 < 0)
		{
			if (ais[curProcAI].patrolling == 0)
			{
				ais[curProcAI].targetID = -1;
				ais[curProcAI].targetVisible = false;
				ais[curProcAI].targetInRange = false;
				ais[curProcAI].needsRoute = false;
				ais[curProcAI].aiRoute.numPts = 0;
			}
			if (ais[curProcAI].aiRoute.numPts == 0 && !ais[curProcAI].needsRoute)
			{
				mainC.mapsMain.Get_Random_Spawn_Point(out ais[curProcAI].targetX, out ais[curProcAI].targetY, out ais[curProcAI].targetZ, out angle);
				ais[curProcAI].patrolling = 1;
				ais[curProcAI].active = true;
				ais[curProcAI].needsRoute = true;
			}
		}
	}

	public void Check_Vehicle_Target(byte threadID)
	{
		byte uBufferID = global::Rendering.Rendering.uBufferID;
		float num = -1f;
		if (currentAI < 1)
		{
			return;
		}
		short playerID = ais[curProcAI].playerID;
		if (playerID < 0 || global::Players.Players.players[playerID].onmap != 4)
		{
			return;
		}
		ais[curProcAI].lostTargetTimer -= global::MainGame.MainGame.frametime;
		if (ais[curProcAI].lostTargetTimer < 0f)
		{
			ais[curProcAI].lostTargetTimer = -1f;
			ais[curProcAI].targetID = -1;
		}
		short targetID;
		if ((targetID = ais[curProcAI].targetID) > -1 && global::Players.Players.players[targetID].dead)
		{
			ais[curProcAI].targetID = -1;
		}
		ais[curProcAI].active = true;
		ais[curProcAI].targetVisible = false;
		ais[curProcAI].targetInRange = false;
		float cosFov = ais[curProcAI].cosFov;
		short num2 = 0;
		do
		{
			num2 = global::Players.Players.Find_Next_Player_Not_On_This_Team(num2, ais[curProcAI].team);
			if (num2 <= -1)
			{
				continue;
			}
			targetID = num2;
			num2++;
			sbyte b = (sbyte)global::Players.Players.players[targetID].humanoidBackJoint;
			Matrix matrix = global::Players.Players.players[targetID].jt1[b].mv[uBufferID];
			aiVec4.v[0] = matrix.M41 + global::Players.Players.players[targetID].charP.position.v[0];
			aiVec4.v[1] = matrix.M42 + global::Players.Players.players[targetID].charP.position.v[1];
			aiVec4.v[2] = matrix.M43 + global::Players.Players.players[targetID].charP.position.v[2];
			aiVec3.v[0] = aiVec4.v[0] - global::Players.Players.players[playerID].charP.position.v[0];
			aiVec3.v[1] = aiVec4.v[1] - global::Players.Players.players[playerID].charP.position.v[1];
			aiVec3.v[2] = aiVec4.v[2] - global::Players.Players.players[playerID].charP.position.v[2];
			float num3 = (float)Math.Sqrt(aiVec3.v[0] * aiVec3.v[0] + aiVec3.v[1] * aiVec3.v[1] + aiVec3.v[2] * aiVec3.v[2]);
			if (num3 != 0f)
			{
				aiVec3.v[0] /= num3;
				aiVec3.v[1] /= num3;
				aiVec3.v[2] /= num3;
			}
			float num4 = aiVec3.v[0] * global::Players.Players.players[playerID].mv[uBufferID].M21 + aiVec3.v[1] * global::Players.Players.players[playerID].mv[uBufferID].M22 + aiVec3.v[2] * global::Players.Players.players[playerID].mv[uBufferID].M23;
			float num5 = global::Players.Players.players[targetID].mv[uBufferID].M21 * global::Players.Players.players[playerID].mv[uBufferID].M21 + global::Players.Players.players[targetID].mv[uBufferID].M22 * global::Players.Players.players[playerID].mv[uBufferID].M22 + global::Players.Players.players[targetID].mv[uBufferID].M23 * global::Players.Players.players[playerID].mv[uBufferID].M23;
			if (!(num4 >= cosFov) || !(num5 >= num) || !(num3 < ais[curProcAI].targetCanBeSeenDistance))
			{
				continue;
			}
			int Number = -1;
			Vector3 InitialRayStart = default(Vector3);
			Vector3 InitialRayEnd = default(Vector3);
			Vector3 IntersectPosition = default(Vector3);
			Vector3 IntersectNormal = default(Vector3);
			float num6 = num3;
			InitialRayStart.X = global::Players.Players.players[playerID].charP.position.v[0];
			InitialRayStart.Y = global::Players.Players.players[playerID].charP.position.v[1];
			InitialRayStart.Z = global::Players.Players.players[playerID].charP.position.v[2];
			InitialRayEnd.X = global::Players.Players.players[targetID].charP.position.v[0];
			InitialRayEnd.Y = global::Players.Players.players[targetID].charP.position.v[1];
			InitialRayEnd.Z = global::Players.Players.players[targetID].charP.position.v[2];
			int num7 = 0;
			short returnValueZoneCheckIndex = 0;
			ushort returnValueZoneCheckObjID;
			while (mainC.zonesMain.Check_Zones_For_Point(InitialRayStart.X, InitialRayStart.Y, InitialRayStart.Z, 5f, returnValueZoneCheckIndex, 1, out returnValueZoneCheckIndex, out returnValueZoneCheckObjID))
			{
				int numObjects = Zones.zones[returnValueZoneCheckObjID].zoneList.numObjects;
				for (int i = 0; i < numObjects; i++)
				{
					if (mainC.collisionMain.Check_Polygon_Ray_Collision_Projectile(Zones.zones[returnValueZoneCheckObjID].zoneList.oList[i], Number, ref InitialRayStart, ref InitialRayEnd, ref Zones.zones[returnValueZoneCheckObjID].zoneList.matrixList[i], out var _, out IntersectPosition, out IntersectNormal, out Number, threadID))
					{
						num7 = 1;
						i = numObjects;
					}
				}
				if (num7 == 1)
				{
					break;
				}
			}
			if (num7 < 1)
			{
				num = num5;
				ais[curProcAI].lostTargetTimer = 30f;
				ais[curProcAI].active = true;
				ais[curProcAI].goalDistance = num6;
				ais[curProcAI].targetID = targetID;
				ais[curProcAI].needsRoute = true;
				ais[curProcAI].targetVisible = true;
				if (num6 <= ais[curProcAI].optimalTargetDistanceSqr)
				{
					ais[curProcAI].targetInRange = true;
				}
				ais[curProcAI].aiRoute.numPts = 0;
				ais[curProcAI].lastTargetX = global::Players.Players.players[targetID].charP.position.v[0];
				ais[curProcAI].lastTargetY = global::Players.Players.players[targetID].charP.position.v[1];
				ais[curProcAI].lastTargetZ = global::Players.Players.players[targetID].charP.position.v[2];
				ais[curProcAI].targetZ = ais[curProcAI].lastTargetZ;
			}
		}
		while (num2 != -1);
	}

	public void Target_AI_To_Shots(byte playerID)
	{
		for (byte b = 0; b < currentAI; b++)
		{
			short playerID2 = ais[b].playerID;
			if (ais[b].targetID < 0 && playerID2 > -1 && global::Players.Players.players[playerID].team != ais[b].team && !global::Players.Players.players[playerID2].dead)
			{
				float num = global::Players.Players.players[playerID2].charP.position.v[0] - global::Players.Players.players[playerID].charP.position.v[0];
				float num2 = global::Players.Players.players[playerID2].charP.position.v[1] - global::Players.Players.players[playerID].charP.position.v[1];
				float num3 = num * num + num2 * num2;
				if (num3 < ais[b].maxDistanceToHearShotsSqr)
				{
					ais[b].active = true;
					ais[b].needsRoute = true;
					ais[b].aiRoute.numPts = 0;
					ais[b].targetID = playerID;
					ais[b].fireTimeRemaining = ais[b].firingTimeAdjusted;
					ais[b].lastTargetX = global::Players.Players.players[playerID].charP.position.v[0];
					ais[b].lastTargetY = global::Players.Players.players[playerID].charP.position.v[1];
					ais[b].lastTargetZ = global::Players.Players.players[playerID].charP.position.v[2] - global::Players.Players.playerRaces[global::Players.Players.players[playerID].race].spawnHeight[global::Players.Players.players[playerID].type] + global::Players.Players.playerRaces[global::Players.Players.players[playerID2].race].spawnHeight[global::Players.Players.players[playerID2].type];
				}
			}
		}
	}

	public void Target_AI_To_Shooter(byte aiID, byte aiPlayerID, byte shootingPlayerID)
	{
		if (aiID < currentAI)
		{
			if (ais[aiID].targetID < 0 && global::Players.Players.players[aiPlayerID].team != global::Players.Players.players[shootingPlayerID].team && aiPlayerID < global::MainGame.MainGame.maxGamePlayers)
			{
				ais[aiID].targetID = shootingPlayerID;
				ais[aiID].active = true;
				ais[aiID].needsRoute = true;
				ais[aiID].aiRoute.numPts = 0;
				ais[aiID].fireTimeRemaining = ais[aiID].firingTimeAdjusted;
				ais[aiID].lastTargetX = global::Players.Players.players[shootingPlayerID].charP.position.v[0];
				ais[aiID].lastTargetY = global::Players.Players.players[shootingPlayerID].charP.position.v[1];
				ais[aiID].lastTargetZ = global::Players.Players.players[shootingPlayerID].charP.position.v[2] - global::Players.Players.playerRaces[global::Players.Players.players[shootingPlayerID].race].spawnHeight[global::Players.Players.players[shootingPlayerID].type] + global::Players.Players.playerRaces[global::Players.Players.players[aiPlayerID].race].spawnHeight[global::Players.Players.players[aiPlayerID].type];
			}
			Target_AI_To_Fallen_Teammate(aiPlayerID, shootingPlayerID);
		}
	}

	public void Target_AI_To_Fallen_Teammate(byte aiPlayerID, byte shootingPlayerID)
	{
		for (byte b = 0; b < currentAI; b++)
		{
			short playerID = ais[b].playerID;
			if (ais[b].targetID < 0 && playerID > -1 && !global::Players.Players.players[playerID].dead && global::Players.Players.players[shootingPlayerID].team != ais[b].team)
			{
				float num = global::Players.Players.players[playerID].charP.position.v[0] - global::Players.Players.players[aiPlayerID].charP.position.v[0];
				float num2 = global::Players.Players.players[playerID].charP.position.v[1] - global::Players.Players.players[aiPlayerID].charP.position.v[1];
				float num3 = num * num + num2 * num2;
				if (num3 < ais[b].maxDistanceToSeeShotTeammateSqr)
				{
					ais[b].active = true;
					ais[b].needsRoute = true;
					ais[b].aiRoute.numPts = 0;
					ais[b].targetID = shootingPlayerID;
					ais[b].fireTimeRemaining = ais[b].firingTimeAdjusted;
					ais[b].lastTargetX = global::Players.Players.players[shootingPlayerID].charP.position.v[0];
					ais[b].lastTargetY = global::Players.Players.players[shootingPlayerID].charP.position.v[1];
					ais[b].lastTargetZ = global::Players.Players.players[shootingPlayerID].charP.position.v[2] - global::Players.Players.playerRaces[global::Players.Players.players[shootingPlayerID].race].spawnHeight[global::Players.Players.players[shootingPlayerID].type] + global::Players.Players.playerRaces[global::Players.Players.players[aiPlayerID].race].spawnHeight[global::Players.Players.players[aiPlayerID].type];
				}
			}
		}
	}

	public void Find_Route()
	{
		if (currentAI < 1)
		{
			return;
		}
		int i;
		for (i = curProcAI; i < currentAI; i++)
		{
			if (ais[i].updateAIRoute)
			{
				Update_Route_For_Remote_Controlled_AI((ushort)i);
				return;
			}
		}
		int num = 0;
		int num2 = curProcAI + 1;
		bool flag = false;
		for (i = curProcAI; i < currentAI; i++)
		{
			if (ais[i].active && ais[i].needsRoute && ais[i].locallyControlled && !ais[i].stationary && (num = ais[i].playerID) > -1 && !global::Players.Players.players[num].dead)
			{
				flag = true;
				break;
			}
		}
		if (!flag)
		{
			for (i = 0; i < num2; i++)
			{
				if (ais[i].active && ais[i].needsRoute && ais[i].locallyControlled && !ais[i].stationary && (num = ais[i].playerID) > -1 && !global::Players.Players.players[num].dead)
				{
					flag = true;
					break;
				}
			}
		}
		if (!flag)
		{
			return;
		}
		ushort num3 = (byte)i;
		float num4;
		float num5;
		float num6;
		if (ais[num3].bossID == byte.MaxValue)
		{
			num4 = ais[num3].targetX;
			num5 = ais[num3].targetY;
			num6 = ais[num3].targetZ;
		}
		else
		{
			num4 = ais[num3].positionGoalX;
			num5 = ais[num3].positionGoalY;
			num6 = ais[num3].positionGoalZ;
		}
		dtStatNavMesh.MaxRoutesThisUpdate = 1;
		Vector3 startpos;
		Vector3 endpos;
		if (global::MainGame.MainGame.gameMode == 0)
		{
			startpos = new Vector3(global::Players.Players.players[num].posX[global::Rendering.Rendering.rBufferID], global::Players.Players.players[num].posZ[global::Rendering.Rendering.rBufferID], 0f - global::Players.Players.players[num].posY[global::Rendering.Rendering.rBufferID]);
			endpos = new Vector3(num4, num6, 0f - num5);
		}
		else
		{
			findRoute_hx = new HalfSingle(global::Players.Players.players[num].posX[global::Rendering.Rendering.rBufferID]);
			findRoute_hy = new HalfSingle(global::Players.Players.players[num].posY[global::Rendering.Rendering.rBufferID]);
			findRoute_hz = new HalfSingle(global::Players.Players.players[num].posZ[global::Rendering.Rendering.rBufferID]);
			startpos = new Vector3(findRoute_hx.ToSingle(), findRoute_hz.ToSingle(), 0f - findRoute_hy.ToSingle());
			findRoute_hx = new HalfSingle(num4);
			findRoute_hy = new HalfSingle(num5);
			findRoute_hz = new HalfSingle(num6);
			endpos = new Vector3(findRoute_hx.ToSingle(), findRoute_hz.ToSingle(), 0f - findRoute_hy.ToSingle());
		}
		ais[num3].aiRoute.numPts = (ushort)global::MainGame.MainGame.NavigationMesh.GetPath(ref startpos, ref endpos, global::MainGame.MainGame.routePolys, ais[num3].aiRoute.NavMeshRoute, randomDestination: true);
		ais[num3].aiRoute.curPt = 0;
		ais[num3].needsRoute = false;
		ais[num3].updateAIRoute = false;
		ais[num3].aiRoute.startX = startpos.X;
		ais[num3].aiRoute.startY = 0f - startpos.Z;
		ais[num3].aiRoute.startZ = startpos.Y;
		ais[num3].aiRoute.endX = endpos.X;
		ais[num3].aiRoute.endY = 0f - endpos.Z;
		ais[num3].aiRoute.endZ = endpos.Y;
		if (ais[num3].aiRoute.numPts > 0)
		{
			Vector3 vector = ais[num3].aiRoute.NavMeshRoute[0];
			ais[num3].goalX = vector.X;
			ais[num3].goalY = 0f - vector.Z;
			ais[num3].goalZ = vector.Y;
			if (ais[num3].aiRoute.numPts == 2 && ais[num3].aiRoute.NavMeshRoute[0].X == ais[num3].aiRoute.NavMeshRoute[1].X && ais[num3].aiRoute.NavMeshRoute[0].Y == ais[num3].aiRoute.NavMeshRoute[1].Y && ais[num3].aiRoute.NavMeshRoute[0].Z == ais[num3].aiRoute.NavMeshRoute[1].Z && (startpos.X != endpos.X || startpos.Y != endpos.Y || startpos.Z != endpos.Z))
			{
				ais[num3].aiRoute.routeError = true;
			}
		}
		else
		{
			ais[num3].aiRoute.routeError = true;
		}
		if (global::MainGame.MainGame.gameMode == 1)
		{
			sendAIRoute = true;
			aiRouteToSend = num3;
		}
	}

	public void Update_Route_For_Remote_Controlled_AI(ushort aiID)
	{
		dtStatNavMesh.MaxRoutesThisUpdate = 1;
		Vector3 startpos = new Vector3(ais[aiID].aiRoute.startX, ais[aiID].aiRoute.startZ, 0f - ais[aiID].aiRoute.startY);
		Vector3 endpos = new Vector3(ais[aiID].aiRoute.endX, ais[aiID].aiRoute.endZ, 0f - ais[aiID].aiRoute.endY);
		ais[aiID].aiRoute.numPts = (ushort)global::MainGame.MainGame.NavigationMesh.GetPath(ref startpos, ref endpos, global::MainGame.MainGame.routePolys, ais[aiID].aiRoute.NavMeshRoute, randomDestination: true);
		ais[aiID].needsRoute = false;
		ais[aiID].updateAIRoute = false;
		if (ais[aiID].aiRoute.numPts > 0)
		{
			if (ais[aiID].aiRoute.curPt >= ais[aiID].aiRoute.numPts)
			{
				ais[aiID].aiRoute.curPt = 0;
			}
			Vector3 vector = ais[aiID].aiRoute.NavMeshRoute[ais[aiID].aiRoute.curPt];
			ais[aiID].goalX = vector.X;
			ais[aiID].goalY = 0f - vector.Z;
			ais[aiID].goalZ = vector.Y;
		}
	}

	public bool Is_Target_Visible(ushort aiPlayerID, ushort targetPlayeID, out float length, byte threadID)
	{
		byte uBufferID = global::Rendering.Rendering.uBufferID;
		float num = 0f;
		float num2 = 0f;
		int Number = -1;
		Vector3 InitialRayStart = default(Vector3);
		Vector3 InitialRayEnd = default(Vector3);
		Vector3 IntersectPosition = default(Vector3);
		Vector3 IntersectNormal = default(Vector3);
		byte headJoint = global::Players.Players.players[aiPlayerID].headJoint;
		InitialRayStart.X = global::Players.Players.players[aiPlayerID].jt1[headJoint].mv[uBufferID].M41 + global::Players.Players.players[aiPlayerID].charP.position.v[0];
		InitialRayStart.Y = global::Players.Players.players[aiPlayerID].jt1[headJoint].mv[uBufferID].M42 + global::Players.Players.players[aiPlayerID].charP.position.v[1];
		InitialRayStart.Z = global::Players.Players.players[aiPlayerID].jt1[headJoint].mv[uBufferID].M43 + global::Players.Players.players[aiPlayerID].charP.position.v[2];
		headJoint = global::Players.Players.players[targetPlayeID].headJoint;
		InitialRayEnd.X = global::Players.Players.players[targetPlayeID].jt1[headJoint].mv[uBufferID].M41 + global::Players.Players.players[targetPlayeID].charP.position.v[0];
		InitialRayEnd.Y = global::Players.Players.players[targetPlayeID].jt1[headJoint].mv[uBufferID].M42 + global::Players.Players.players[targetPlayeID].charP.position.v[1];
		InitialRayEnd.Z = global::Players.Players.players[targetPlayeID].jt1[headJoint].mv[uBufferID].M43 + global::Players.Players.players[targetPlayeID].charP.position.v[2];
		num = InitialRayEnd.X - InitialRayStart.X;
		num2 = InitialRayEnd.Y - InitialRayStart.Y;
		float num3 = InitialRayEnd.Z - InitialRayStart.Z;
		length = (float)Math.Sqrt(num * num + num2 * num2 + num3 * num3);
		short returnValueZoneCheckIndex = 0;
		ushort returnValueZoneCheckObjID;
		while (mainC.zonesMain.Check_Zones_For_Point(InitialRayStart.X, InitialRayStart.Y, InitialRayStart.Z, 5f, returnValueZoneCheckIndex, 1, out returnValueZoneCheckIndex, out returnValueZoneCheckObjID))
		{
			int numObjects = Zones.zones[returnValueZoneCheckObjID].zoneList.numObjects;
			for (int i = 0; i < numObjects; i++)
			{
				if (mainC.collisionMain.Check_Polygon_Ray_Collision_Projectile(Zones.zones[returnValueZoneCheckObjID].zoneList.oList[i], Number, ref InitialRayStart, ref InitialRayEnd, ref Zones.zones[returnValueZoneCheckObjID].zoneList.matrixList[i], out var _, out IntersectPosition, out IntersectNormal, out Number, threadID))
				{
					return false;
				}
			}
		}
		return true;
	}

	public float Get_Boss_Health(short bossID)
	{
		if (bossID < 0 || bossID >= numBosses)
		{
			return 0f;
		}
		short playerID = ais[bosses[bossID].aiID].playerID;
		if (playerID > -1)
		{
			return 1f - global::Players.Players.players[playerID].damagePercentageCapped;
		}
		return 0f;
	}

	public void Process_Bosses(float time)
	{
		bool flag = false;
		if (!bossLevel)
		{
			return;
		}
		for (byte b = 0; b < numBosses; b++)
		{
			short playerID = ais[bosses[b].aiID].playerID;
			if (playerID > -1 && !global::Players.Players.players[playerID].dead)
			{
				flag = true;
				byte aiID = bosses[b].aiID;
				byte curPosition = bosses[b].curPosition;
				bosses[b].positionTime -= time;
				if (bosses[b].positionTime < 0f)
				{
					bosses[b].curPosition++;
					if (bosses[b].curPosition >= bosses[b].numPositions)
					{
						bosses[b].curPosition = 0;
					}
					curPosition = bosses[b].curPosition;
					bosses[b].positionTime = bosses[b].positionTimers[curPosition];
					ais[aiID].positionGoalX = bosses[b].positionX[curPosition];
					ais[aiID].positionGoalY = bosses[b].positionY[curPosition];
					ais[aiID].positionGoalZ = bosses[b].positionZ[curPosition];
					ais[aiID].needsRoute = true;
					ais[aiID].aiRoute.numPts = 0;
				}
				bosses[b].weaponTime -= time;
				if (bosses[b].weaponTime < 0f)
				{
					bosses[b].curWeapon++;
					if (bosses[b].curWeapon >= bosses[b].numWeapons)
					{
						bosses[b].curWeapon = 0;
					}
					byte curWeapon = bosses[b].curWeapon;
					bosses[b].weaponTime = bosses[b].weaponTimers[curWeapon];
					global::Players.Players.players[playerID].shootingAccuracy = bosses[b].accuracy[curWeapon];
					byte b2 = bosses[b].weaponIDs[curWeapon];
					global::Players.Players.players[playerID].primaryWeaponMountWeapon = (sbyte)b2;
					ais[aiID].weapon = (sbyte)b2;
					global::Players.Players.players[playerID].wpnIndex = (sbyte)mainC.weaponsMain.Get_Weapon_Index((ushort)playerID, b2);
					mainC.weaponsMain.Reset_Players_Weapon_Stub(ref global::Players.Players.players[playerID].weapon2[global::Players.Players.players[playerID].wpnIndex], (byte)global::Players.Players.players[playerID].wpnIndex, b2, (byte)playerID);
					mainC.weaponsMain.Load_Ammo_Clip_Into_AI_Weapon((byte)global::Players.Players.players[playerID].wpnIndex, b2, (byte)playerID, 1);
					mainC.weaponsMain.Update_Player_Weapon_Info((byte)playerID);
				}
				ais[aiID].goalX = ais[aiID].positionGoalX;
				ais[aiID].goalY = ais[aiID].positionGoalY;
				ais[aiID].goalZ = ais[aiID].positionGoalZ;
			}
			else if (playerID < 0)
			{
				ais[bosses[b].aiID].active = false;
			}
		}
		if (!flag && !bossLevelComplete)
		{
			aiRespawnTimer = -1f;
			mainC.programsMain.Run_Program_Basic(0, toggleDirection: false, 0, 0);
			mainC.pickupsMain.Disable_Pickups();
			bossLevelComplete = true;
		}
	}

	public void Reset_Bosses()
	{
		for (byte b = 0; b < numBosses; b++)
		{
			bosses[b].curPosition = 0;
			bosses[b].curWeapon = 0;
			bosses[b].positionTime = bosses[b].positionTimers[0];
			short playerID = ais[bosses[b].aiID].playerID;
			if (playerID > -1)
			{
				global::Players.Players.players[playerID].primaryWeaponMountWeapon = (sbyte)bosses[b].weaponIDs[0];
				ais[bosses[b].aiID].weapon = (sbyte)bosses[b].weaponIDs[0];
				ais[bosses[b].aiID].active = true;
			}
			else
			{
				ais[bosses[b].aiID].active = false;
			}
		}
	}

	public void Send_AI_Completed()
	{
		mainC.networkingMain.XBOX_Send_Network_Message80(80);
	}

	public void Receive_AI_Completed_Message()
	{
		aiCompleted = true;
	}

	public void Receive_Ai_Respawn_Authorization()
	{
		hostAuthorizedSpawn++;
	}

	public void Receive_KillCount_From_Host()
	{
		levelKillCount = global::Networking.Networking.networkUShorts[0];
		numTotalAiSpawned = global::Networking.Networking.networkUShorts[1];
	}

	public void Send_KillCount(NetworkGamer gamer)
	{
		global::Networking.Networking.networkUShorts[0] = levelKillCount;
		global::Networking.Networking.networkUShorts[1] = numTotalAiSpawned;
		mainC.networkingMain.XBOX_Send_Network_Message_To_Gamer(54, gamer);
	}

	public void Update_KillCount_Scale()
	{
		killCountScale = 0f;
		for (ushort num = 0; num < global::MainGame.MainGame.maxHumanGamePlayers; num++)
		{
			if (global::Networking.Networking.networkPlayers[num].playerLoaded)
			{
				killCountScale += 1f;
			}
		}
		killCountScale = mainC.gameLogic.Game_Adjust_AI_KillCount_Scale(killCountScale);
	}

	public void Send_All_AI_To_New_Player(NetworkGamer gamer)
	{
		Send_KillCount(gamer);
		for (byte b = 0; b < numAI; b++)
		{
			short playerID;
			if ((playerID = ais[b].playerID) < 0)
			{
				global::Networking.Networking.networkBytes[0] = b;
				mainC.networkingMain.XBOX_Send_Network_Message_To_Gamer(64, gamer);
			}
			else
			{
				switch (global::Players.Players.players[playerID].onmap)
				{
				case 1:
				case 2:
				case 8:
				{
					global::Networking.Networking.networkBools[0] = ais[b].active;
					global::Networking.Networking.networkBytes[0] = b;
					global::Networking.Networking.networkBytes[1] = global::Players.Players.players[playerID].onmap;
					global::Networking.Networking.networkBytes[2] = (byte)ais[b].controllingPlayer;
					ref HalfSingle reference3 = ref global::Networking.Networking.networkHS[0];
					reference3 = new HalfSingle(global::Players.Players.players[playerID].deathTime);
					ref HalfSingle reference4 = ref global::Networking.Networking.networkHS[1];
					reference4 = new HalfSingle(global::Players.Players.players[playerID].timeBeforeRespawn[global::Rendering.Rendering.uBufferID]);
					mainC.networkingMain.XBOX_Send_Network_Message_To_Gamer(65, gamer);
					break;
				}
				case 4:
				{
					global::Networking.Networking.networkBytes[0] = b;
					global::Networking.Networking.networkBytes[1] = global::Players.Players.players[playerID].onmap;
					global::Networking.Networking.networkBytes[2] = ais[b].aiRoute.curPt;
					global::Networking.Networking.networkBytes[3] = (byte)ais[b].controllingPlayer;
					global::Networking.Networking.networkBools[0] = ais[b].targetVisible;
					global::Networking.Networking.networkBools[1] = ais[b].targetInRange;
					global::Networking.Networking.networkShorts[0] = ais[b].targetID;
					global::Networking.Networking.networkUShorts[0] = 0;
					global::Networking.Networking.networkUShorts[1] = 0;
					global::Networking.Networking.networkFloats[0] = global::Players.Players.players[playerID].damage;
					global::Networking.Networking.networkFloats[1] = global::Players.Players.players[playerID].charP.position.v[0];
					global::Networking.Networking.networkFloats[2] = global::Players.Players.players[playerID].charP.position.v[1];
					global::Networking.Networking.networkFloats[3] = global::Players.Players.players[playerID].charP.position.v[2];
					global::Networking.Networking.networkFloats[4] = global::Players.Players.players[playerID].charP.velocity.v[0];
					global::Networking.Networking.networkFloats[5] = global::Players.Players.players[playerID].charP.velocity.v[1];
					global::Networking.Networking.networkFloats[6] = global::Players.Players.players[playerID].charP.velocity.v[2];
					global::Networking.Networking.networkFloats[7] = global::Players.Players.players[playerID].velX;
					global::Networking.Networking.networkFloats[8] = global::Players.Players.players[playerID].velY;
					global::Networking.Networking.networkFloats[9] = global::Players.Players.players[playerID].velZ;
					global::Networking.Networking.networkFloats[10] = ais[b].goalX;
					global::Networking.Networking.networkFloats[11] = ais[b].goalY;
					global::Networking.Networking.networkFloats[12] = ais[b].goalZ;
					global::Networking.Networking.networkFloats[13] = ais[b].aiRoute.startX;
					global::Networking.Networking.networkFloats[14] = ais[b].aiRoute.startY;
					global::Networking.Networking.networkFloats[15] = ais[b].aiRoute.startZ;
					global::Networking.Networking.networkFloats[16] = ais[b].aiRoute.endX;
					global::Networking.Networking.networkFloats[17] = ais[b].aiRoute.endY;
					global::Networking.Networking.networkFloats[18] = ais[b].aiRoute.endZ;
					global::Networking.Networking.networkFloats[19] = ais[b].randomFactor;
					ref HalfSingle reference = ref global::Networking.Networking.networkHS[0];
					reference = new HalfSingle(global::Players.Players.players[playerID].zRotation);
					ref HalfSingle reference2 = ref global::Networking.Networking.networkHS[1];
					reference2 = new HalfSingle(ais[b].fireTimeRemaining);
					mainC.networkingMain.XBOX_Send_Network_Message_To_Gamer(66, gamer);
					break;
				}
				}
			}
		}
	}

	public void Receive_AI_Players(byte type)
	{
		try
		{
			switch (type)
			{
			case 0:
			{
				ushort num = global::Networking.Networking.networkBytes[0];
				ais[num].active = global::Networking.Networking.networkBools[0];
				ais[num].updateAIRoute = false;
				int playerID = Assign_AI_Single(num);
				if (playerID > -1)
				{
					global::Players.Players.players[playerID].onmap = global::Networking.Networking.networkBytes[1];
					global::Players.Players.players[playerID].dead = true;
					if (global::Players.Players.players[playerID].onmap == 4)
					{
						global::Players.Players.players[playerID].dead = false;
					}
					global::Players.Players.players[playerID].deathTime = global::Networking.Networking.networkHS[0].ToSingle();
					global::Players.Players.players[playerID].timeBeforeRespawn[global::Rendering.Rendering.uBufferID] = global::Networking.Networking.networkHS[1].ToSingle();
					ais[num].controllingPlayer = global::Networking.Networking.networkBytes[2];
					ais[num].authorizedToRespawn = false;
					ais[num].locallyControlled = false;
					if (ais[num].controllingPlayer == global::Players.Players.players[0].id)
					{
						ais[num].locallyControlled = true;
					}
				}
				break;
			}
			case 1:
			{
				ushort num = global::Networking.Networking.networkBytes[0];
				int playerID = Assign_AI_Single(num);
				if (playerID > -1)
				{
					global::Players.Players.players[playerID].onmap = global::Networking.Networking.networkBytes[1];
					global::Players.Players.players[playerID].dead = true;
					if (global::Players.Players.players[playerID].onmap == 4)
					{
						global::Players.Players.players[playerID].dead = false;
					}
					ais[num].updateAIRoute = true;
					ais[num].active = true;
					ais[num].aiRoute.curPt = global::Networking.Networking.networkBytes[2];
					ais[num].targetVisible = global::Networking.Networking.networkBools[0];
					ais[num].targetInRange = global::Networking.Networking.networkBools[1];
					ais[num].targetID = global::Networking.Networking.networkShorts[0];
					global::Players.Players.players[playerID].damage = global::Networking.Networking.networkFloats[0];
					global::Players.Players.players[playerID].charP.position.v[0] = global::Networking.Networking.networkFloats[1];
					global::Players.Players.players[playerID].charP.position.v[1] = global::Networking.Networking.networkFloats[2];
					global::Players.Players.players[playerID].charP.position.v[2] = global::Networking.Networking.networkFloats[3];
					global::Players.Players.players[playerID].charP.velocity.v[0] = global::Networking.Networking.networkFloats[4];
					global::Players.Players.players[playerID].charP.velocity.v[1] = global::Networking.Networking.networkFloats[5];
					global::Players.Players.players[playerID].charP.velocity.v[2] = global::Networking.Networking.networkFloats[6];
					global::Players.Players.players[playerID].velX = global::Networking.Networking.networkFloats[7];
					global::Players.Players.players[playerID].velY = global::Networking.Networking.networkFloats[8];
					global::Players.Players.players[playerID].velZ = global::Networking.Networking.networkFloats[9];
					ais[num].goalX = global::Networking.Networking.networkFloats[10];
					ais[num].goalY = global::Networking.Networking.networkFloats[11];
					ais[num].goalZ = global::Networking.Networking.networkFloats[12];
					ais[num].aiRoute.startX = global::Networking.Networking.networkFloats[13];
					ais[num].aiRoute.startY = global::Networking.Networking.networkFloats[14];
					ais[num].aiRoute.startZ = global::Networking.Networking.networkFloats[15];
					ais[num].aiRoute.endX = global::Networking.Networking.networkFloats[16];
					ais[num].aiRoute.endY = global::Networking.Networking.networkFloats[17];
					ais[num].aiRoute.endZ = global::Networking.Networking.networkFloats[18];
					ais[num].randomFactor = global::Networking.Networking.networkFloats[19];
					global::Players.Players.players[playerID].zRotation = global::Networking.Networking.networkHS[0].ToSingle();
					ais[num].fireTimeRemaining = global::Networking.Networking.networkHS[1].ToSingle();
					ais[num].controllingPlayer = global::Networking.Networking.networkBytes[3];
					ais[num].authorizedToRespawn = false;
					ais[num].locallyControlled = false;
					if (ais[num].controllingPlayer == global::Players.Players.players[0].id)
					{
						ais[num].locallyControlled = true;
					}
					mpData[num].currentPosX = global::Players.Players.players[playerID].charP.position.v[0];
					mpData[num].currentPosY = global::Players.Players.players[playerID].charP.position.v[1];
					mpData[num].currentPosZ = global::Players.Players.players[playerID].charP.position.v[2];
					mpData[num].velX = global::Players.Players.players[playerID].charP.velocity.v[0];
					mpData[num].velY = global::Players.Players.players[playerID].charP.velocity.v[1];
					mpData[num].velZ = global::Players.Players.players[playerID].charP.velocity.v[2];
					if (!mpData[num].dataThisRound)
					{
						mpData[num].timeFromLastUpdate = (float)(global::MainGame.MainGame.mainTime - mpData[num].lastUpdate) * 1E-07f;
						mpData[num].lastUpdate = global::MainGame.MainGame.mainTime;
						mpData[num].dataThisRound = true;
					}
				}
				break;
			}
			case 2:
			case 6:
			{
				ushort num = global::Networking.Networking.networkBytes[0];
				ais[num].active = true;
				if (type == 6)
				{
					ais[num].active = false;
				}
				int playerID = ais[num].playerID;
				if (playerID > -1)
				{
					global::Players.Players.players[playerID].zRotation = global::Networking.Networking.networkHS[6].ToSingle();
					mpData[num].currentPosX = global::Networking.Networking.networkHS[0].ToSingle();
					mpData[num].currentPosY = global::Networking.Networking.networkHS[1].ToSingle();
					mpData[num].currentPosZ = global::Networking.Networking.networkHS[2].ToSingle();
					mpData[num].velX = global::Networking.Networking.networkHS[3].ToSingle();
					mpData[num].velY = global::Networking.Networking.networkHS[4].ToSingle();
					mpData[num].velZ = global::Networking.Networking.networkHS[5].ToSingle();
					ais[num].canFire = (global::Networking.Networking.networkBytes[1] & 0x80) == 128;
					global::Networking.Networking.networkBytes[1] = (byte)(global::Networking.Networking.networkBytes[1] & 0x7F);
					if (global::Players.Players.players[playerID].onmap != global::Networking.Networking.networkBytes[1] && (global::Players.Players.players[playerID].onmap & 0xB) != 0)
					{
						global::Players.Players.players[playerID].charP.position.v[0] = mpData[num].currentPosX;
						global::Players.Players.players[playerID].charP.position.v[1] = mpData[num].currentPosY;
						global::Players.Players.players[playerID].charP.position.v[2] = mpData[num].currentPosZ;
					}
					global::Players.Players.players[playerID].onmap = global::Networking.Networking.networkBytes[1];
					global::Players.Players.players[playerID].dead = true;
					if (global::Players.Players.players[playerID].onmap == 4)
					{
						global::Players.Players.players[playerID].dead = false;
					}
					byte b = global::Networking.Networking.networkBytes[2];
					if (b != ais[num].aiRoute.curPt && b < ais[num].aiRoute.numPts)
					{
						Vector3 vector = ais[num].aiRoute.NavMeshRoute[ais[num].aiRoute.curPt];
						ais[num].goalX = vector.X;
						ais[num].goalY = 0f - vector.Z;
						ais[num].goalZ = vector.Y;
						ais[num].aiRoute.curPt = b;
					}
					if (!mpData[num].dataThisRound)
					{
						mpData[num].timeFromLastUpdate = (float)(global::MainGame.MainGame.mainTime - mpData[num].lastUpdate) * 1E-07f;
						mpData[num].lastUpdate = global::MainGame.MainGame.mainTime;
						mpData[num].dataThisRound = true;
					}
				}
				break;
			}
			case 3:
			{
				ushort num = global::Networking.Networking.networkBytes[0];
				ais[num].targetID = global::Networking.Networking.networkShorts[0];
				break;
			}
			case 4:
				ais[global::Networking.Networking.networkBytes[0]].playerID = -1;
				break;
			case 5:
			{
				ushort num = global::Networking.Networking.networkBytes[0];
				ais[num].updateAIRoute = true;
				ais[num].aiRoute.curPt = 0;
				ais[num].aiRoute.startX = global::Networking.Networking.networkHS[0].ToSingle();
				ais[num].aiRoute.startY = global::Networking.Networking.networkHS[1].ToSingle();
				ais[num].aiRoute.startZ = global::Networking.Networking.networkHS[2].ToSingle();
				ais[num].aiRoute.endX = global::Networking.Networking.networkHS[3].ToSingle();
				ais[num].aiRoute.endY = global::Networking.Networking.networkHS[4].ToSingle();
				ais[num].aiRoute.endZ = global::Networking.Networking.networkHS[5].ToSingle();
				break;
			}
			}
		}
		catch (Exception)
		{
		}
	}

	public void Send_AI_Players(float frameTime)
	{
		lastNetworkSend += frameTime;
		bool flag = true;
		ushort num = 0;
		while (lastNetworkSend * 5600f >= 65f && flag && num < numAI)
		{
			if (ais[curNetworkAI].locallyControlled)
			{
				byte type = 67;
				if (!ais[curNetworkAI].active)
				{
					type = 74;
				}
				short playerID = ais[curNetworkAI].playerID;
				flag = false;
				global::Networking.Networking.networkBytes[0] = curNetworkAI;
				global::Networking.Networking.networkBytes[1] = global::Players.Players.players[playerID].onmap;
				if (ais[curNetworkAI].canFire)
				{
					global::Networking.Networking.networkBytes[1] |= 128;
				}
				global::Networking.Networking.networkBytes[2] = ais[curNetworkAI].aiRoute.curPt;
				ref HalfSingle reference = ref global::Networking.Networking.networkHS[0];
				reference = new HalfSingle(global::Players.Players.players[playerID].charP.position.v[0]);
				ref HalfSingle reference2 = ref global::Networking.Networking.networkHS[1];
				reference2 = new HalfSingle(global::Players.Players.players[playerID].charP.position.v[1]);
				ref HalfSingle reference3 = ref global::Networking.Networking.networkHS[2];
				reference3 = new HalfSingle(global::Players.Players.players[playerID].charP.position.v[2]);
				ref HalfSingle reference4 = ref global::Networking.Networking.networkHS[3];
				reference4 = new HalfSingle(global::Players.Players.players[playerID].velX);
				ref HalfSingle reference5 = ref global::Networking.Networking.networkHS[4];
				reference5 = new HalfSingle(global::Players.Players.players[playerID].velY);
				ref HalfSingle reference6 = ref global::Networking.Networking.networkHS[5];
				reference6 = new HalfSingle(global::Players.Players.players[playerID].velZ);
				ref HalfSingle reference7 = ref global::Networking.Networking.networkHS[6];
				reference7 = new HalfSingle(global::Players.Players.players[playerID].zRotation);
				while (lastNetworkSend * 5600f >= 65f && curRemotePlayer < global::Networking.Networking.networkSession.RemoteGamers.Count)
				{
					if (curRemotePlayer < global::Networking.Networking.networkSession.RemoteGamers.Count)
					{
						short num2 = mainC.playersMain.Get_Player_Index(global::Networking.Networking.networkSession.RemoteGamers[curRemotePlayer].Id, -1);
						if (num2 > -1)
						{
							flag = true;
							lastNetworkSend -= 0.011607143f;
							mainC.networkingMain.XBOX_Send_Network_Message_To_Gamer(type, global::Networking.Networking.networkSession.RemoteGamers[curRemotePlayer]);
						}
					}
					curRemotePlayer++;
				}
				if (curRemotePlayer >= global::Networking.Networking.networkSession.RemoteGamers.Count)
				{
					curRemotePlayer = 0;
					if (++curNetworkAI >= numAI)
					{
						curNetworkAI = 0;
					}
				}
				num++;
			}
			else
			{
				if (++curNetworkAI >= numAI)
				{
					curNetworkAI = 0;
				}
				num++;
			}
		}
		if (lastNetworkSend * 5600f >= 65f)
		{
			lastNetworkSend = 0f;
		}
		for (num = 0; num < numAI; num++)
		{
			if (ais[num].locallyControlled && mpData[num].lastTarget != ais[num].targetID)
			{
				global::Networking.Networking.networkBytes[0] = (byte)num;
				global::Networking.Networking.networkShorts[0] = ais[num].targetID;
				mpData[num].lastTarget = ais[num].targetID;
				mainC.networkingMain.XBOX_Send_Network_Message68(68);
			}
		}
	}

	public void Send_AI_New_Route_Info()
	{
		global::Networking.Networking.networkBytes[0] = (byte)aiRouteToSend;
		ref HalfSingle reference = ref global::Networking.Networking.networkHS[0];
		reference = new HalfSingle(ais[aiRouteToSend].aiRoute.startX);
		ref HalfSingle reference2 = ref global::Networking.Networking.networkHS[1];
		reference2 = new HalfSingle(ais[aiRouteToSend].aiRoute.startY);
		ref HalfSingle reference3 = ref global::Networking.Networking.networkHS[2];
		reference3 = new HalfSingle(ais[aiRouteToSend].aiRoute.startZ);
		ref HalfSingle reference4 = ref global::Networking.Networking.networkHS[3];
		reference4 = new HalfSingle(ais[aiRouteToSend].aiRoute.endX);
		ref HalfSingle reference5 = ref global::Networking.Networking.networkHS[4];
		reference5 = new HalfSingle(ais[aiRouteToSend].aiRoute.endY);
		ref HalfSingle reference6 = ref global::Networking.Networking.networkHS[5];
		reference6 = new HalfSingle(ais[aiRouteToSend].aiRoute.endZ);
		mainC.networkingMain.XBOX_Send_Network_Message72(72);
		sendAIRoute = false;
	}

	public void Update_AI_Controlling_Players()
	{
		Update_KillCount_Scale();
		ushort num = 0;
		ushort num2 = 1;
		ushort num3 = 1;
		while (num2 < global::MainGame.MainGame.maxHumanGamePlayers)
		{
			if (global::Networking.Networking.networkPlayers[num2].playerLoaded)
			{
				num++;
			}
			num2++;
		}
		num2 = 0;
		while (num2 < numAI)
		{
			ais[num2].authorizedToRespawn = false;
			ais[num2].locallyControlled = true;
			ais[num2].controllingPlayer = global::Players.Players.players[0].id;
			num2++;
			num3 = 0;
			while (num3 < num && num2 < numAI)
			{
				ushort num4 = (ushort)ais[num2].playerID;
				if (num4 < global::MainGame.MainGame.maxGamePlayers)
				{
					mpData[num2].currentPosX = global::Players.Players.players[num4].charP.position.v[0];
					mpData[num2].currentPosY = global::Players.Players.players[num4].charP.position.v[1];
					mpData[num2].currentPosZ = global::Players.Players.players[num4].charP.position.v[2];
					mpData[num2].velX = global::Players.Players.players[num4].charP.velocity.v[0];
					mpData[num2].velY = global::Players.Players.players[num4].charP.velocity.v[1];
					mpData[num2].velZ = global::Players.Players.players[num4].charP.velocity.v[2];
				}
				ais[num2].authorizedToRespawn = false;
				ais[num2].locallyControlled = false;
				num2++;
				num3++;
			}
		}
		ushort num5 = 1;
		num++;
		for (num2 = 1; num2 < global::MainGame.MainGame.maxHumanGamePlayers; num2++)
		{
			if (global::Networking.Networking.networkPlayers[num2].playerLoaded)
			{
				for (num3 = num5; num3 < numAI; num3 += num)
				{
					ais[num3].controllingPlayer = global::Players.Players.players[num2].id;
				}
				num5++;
			}
		}
		num3 = num;
		if (num3 >= 10)
		{
			num3 = 10;
		}
		global::Networking.Networking.networkUShorts[0] = num3;
		num2 = 0;
		ushort num6 = 0;
		while (num2 < global::MainGame.MainGame.maxHumanGamePlayers && num6 < num3)
		{
			if (global::Networking.Networking.networkPlayers[num2].playerLoaded)
			{
				global::Networking.Networking.networkShorts[num6++] = global::Players.Players.players[num2].id;
			}
			num2++;
		}
		global::Networking.Networking.networkFloats[0] = killCountScale;
		mainC.networkingMain.XBOX_Send_Network_Message73(73);
	}

	public void Update_AI_Controlling_Players_From_Host()
	{
		ushort num = 0;
		ushort num2 = global::Networking.Networking.networkUShorts[0];
		for (ushort num3 = 0; num3 < num2; num3++)
		{
			short controllingPlayer = global::Networking.Networking.networkShorts[num3];
			for (ushort num4 = num; num4 < numAI; num4 += num2)
			{
				ais[num4].controllingPlayer = controllingPlayer;
			}
			num++;
		}
		for (ushort num4 = 0; num4 < numAI; num4++)
		{
			ais[num4].authorizedToRespawn = false;
			ais[num4].locallyControlled = false;
			if (ais[num4].controllingPlayer == global::Players.Players.players[0].id)
			{
				ais[num4].locallyControlled = true;
			}
		}
		killCountScale = global::Networking.Networking.networkFloats[0];
	}
}
