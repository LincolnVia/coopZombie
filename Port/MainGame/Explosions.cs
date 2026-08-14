using System;
using System.Globalization;
using System.IO;
using GameObjects;
using Microsoft.Xna.Framework;
using Physics;
using Players;
using Rendering;
using Structs;
using Util;
using Weapons;
using WindowsGame1;

namespace MainGame;

public class Explosions
{
	public static float cameraShakeX;

	public static float cameraShakeY;

	public static float cameraShakeZ;

	public static byte numExplosions = 0;

	public static ushort numGameExplosions;

	public static StructsClass.Explosion[] explosions;

	public static StructsClass.Explosion_Occurance[] gameExplosions;

	public static StructsClass.vtex[] pfbV1T = new StructsClass.vtex[5];

	public Game1.MasterCollection mainC;

	public void Init_Vars(Game1.MasterCollection master)
	{
		mainC = master;
		for (byte b = 0; b < 5; b++)
		{
			pfbV1T[b] = new StructsClass.vtex();
		}
	}

	public void Initialize_Explosions()
	{
		Load_Explosion_Data("Explosions.txt");
	}

	public void Load_Explosion_Data(string fileName)
	{
		int num = -1;
		numExplosions = 0;
		Stream stream = TitleContainer.OpenStream("The_CoOp_Zombie_Game\\Config_Files\\" + fileName);
		byte[] array = new byte[stream.Length];
		if (!stream.CanRead)
		{
			return;
		}
		stream.Read(array, 0, array.Length);
		stream.Close();
		string text = mainC.utilMain.Byte_Array_To_String(array);
		string[] array2 = text.Split(new char[2] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
		int num2 = array2.Length;
		if (num2 < 1)
		{
			return;
		}
		for (int i = 0; i < num2; i++)
		{
			string[] array3 = array2[i].Split(new char[2] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
			int num3 = array3.Length;
			if (num3 < 1)
			{
				continue;
			}
			int num4 = 0;
			if (array3[0].Equals("numExplosions", StringComparison.OrdinalIgnoreCase))
			{
				num4 = 1;
			}
			else if (array3[0].Equals("Explosion", StringComparison.OrdinalIgnoreCase))
			{
				num4 = 2;
			}
			else if (array3[0].Equals("Sound", StringComparison.OrdinalIgnoreCase))
			{
				num4 = 3;
			}
			else if (array3[0].Equals("BlastRadius", StringComparison.OrdinalIgnoreCase))
			{
				num4 = 4;
			}
			else if (array3[0].Equals("Duration", StringComparison.OrdinalIgnoreCase))
			{
				num4 = 5;
			}
			else if (array3[0].Equals("CameraShake", StringComparison.OrdinalIgnoreCase))
			{
				num4 = 6;
			}
			else if (array3[0].Equals("numGameExplosions", StringComparison.OrdinalIgnoreCase))
			{
				num4 = 7;
			}
			else if (array3[0].Equals("CameraShakeVariance", StringComparison.OrdinalIgnoreCase))
			{
				num4 = 8;
			}
			else if (array3[0].Equals("Particle", StringComparison.OrdinalIgnoreCase))
			{
				num4 = 9;
			}
			else if (array3[0].Equals("Damage", StringComparison.OrdinalIgnoreCase))
			{
				num4 = 10;
			}
			else if (array3[0].Equals("Splash_Damage", StringComparison.OrdinalIgnoreCase))
			{
				num4 = 11;
			}
			else if (array3[0].Equals("ImpactForce", StringComparison.OrdinalIgnoreCase))
			{
				num4 = 12;
			}
			switch (num4)
			{
			case 1:
				if (array3.Length > 1)
				{
					int num5 = int.Parse(array3[1], CultureInfo.InvariantCulture.NumberFormat);
					explosions = new StructsClass.Explosion[num5];
					for (int j = 0; j < num5; j++)
					{
						explosions[j].cameraShakeVariance = 0.25f;
						explosions[j].particleID = global::Util.Util.maxUnsignedShortValue;
						explosions[j].damage = new float[5];
						explosions[j].impactForce = 0f;
					}
					numExplosions = (byte)num5;
				}
				break;
			case 2:
				if (array3.Length > 1)
				{
					num = int.Parse(array3[1], CultureInfo.InvariantCulture.NumberFormat);
					if (num < 0 || num >= numExplosions)
					{
						num = -1;
					}
				}
				break;
			case 3:
				if (array3.Length > 1 && num > -1)
				{
					explosions[num].sound = array3[1];
				}
				break;
			case 4:
				if (array3.Length > 1 && num > -1)
				{
					explosions[num].blastRadius = float.Parse(array3[1], CultureInfo.InvariantCulture.NumberFormat);
				}
				break;
			case 5:
				if (array3.Length > 1 && num > -1)
				{
					explosions[num].duration = float.Parse(array3[1], CultureInfo.InvariantCulture.NumberFormat);
				}
				break;
			case 6:
				if (array3.Length > 3 && num > -1)
				{
					explosions[num].cameraShakeX = float.Parse(array3[1], CultureInfo.InvariantCulture.NumberFormat);
					explosions[num].cameraShakeY = float.Parse(array3[2], CultureInfo.InvariantCulture.NumberFormat);
					explosions[num].cameraShakeZ = float.Parse(array3[3], CultureInfo.InvariantCulture.NumberFormat);
				}
				break;
			case 7:
				if (array3.Length > 1)
				{
					numGameExplosions = ushort.Parse(array3[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
					gameExplosions = new StructsClass.Explosion_Occurance[numGameExplosions];
					for (int j = 0; j < numGameExplosions; j++)
					{
						gameExplosions[j].status = 0;
					}
				}
				break;
			case 8:
				if (array3.Length > 1 && num > -1)
				{
					explosions[num].cameraShakeVariance = float.Parse(array3[1], CultureInfo.InvariantCulture.NumberFormat);
				}
				break;
			case 9:
				if (array3.Length > 1 && num > -1)
				{
					explosions[num].particleID = ushort.Parse(array3[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
				}
				break;
			case 10:
				if (array3.Length > 2 && num > -1)
				{
					int num5 = int.Parse(array3[1], CultureInfo.InvariantCulture.NumberFormat);
					for (int j = 0; j < 5 && j < num5; j++)
					{
						explosions[num].damage[j] = float.Parse(array3[j + 2], CultureInfo.InvariantCulture.NumberFormat);
					}
				}
				break;
			case 11:
				if (array3.Length > 1 && num > -1)
				{
					explosions[num].splashFalloff = float.Parse(array3[1], CultureInfo.InvariantCulture.NumberFormat);
				}
				break;
			case 12:
				if (array3.Length > 1 && num > -1)
				{
					explosions[num].impactForce = float.Parse(array3[1], CultureInfo.InvariantCulture.NumberFormat);
				}
				break;
			}
		}
	}

	public void Process_Explosions(byte threadID)
	{
		cameraShakeX = 0f;
		cameraShakeY = 0f;
		cameraShakeZ = 0f;
		for (ushort num = 0; num < numGameExplosions; num++)
		{
			if (gameExplosions[num].status > 0)
			{
				gameExplosions[num].curTime += MainGame.frametime / global::Physics.Physics.timeMod;
				ushort explosionID = gameExplosions[num].explosionID;
				if (gameExplosions[num].curTime > explosions[explosionID].duration)
				{
					gameExplosions[num].status = 0;
					gameExplosions[num].curTime = explosions[explosionID].duration;
				}
				float num2 = global::Rendering.Rendering.camPos[global::Rendering.Rendering.rBufferID].X - gameExplosions[num].x;
				float num3 = global::Rendering.Rendering.camPos[global::Rendering.Rendering.rBufferID].Y - gameExplosions[num].y;
				float num4 = global::Rendering.Rendering.camPos[global::Rendering.Rendering.rBufferID].Z - gameExplosions[num].z;
				float num5 = (float)Math.Sqrt(num2 * num2 + num3 * num3 + num4 * num4);
				if (num5 < explosions[explosionID].blastRadius)
				{
					num5 = 1f - num5 / explosions[explosionID].blastRadius;
					num2 = explosions[explosionID].cameraShakeX + explosions[explosionID].cameraShakeX * (float)MainGame.mainRandom.NextDouble() * explosions[explosionID].cameraShakeVariance;
					if (MainGame.mainRandom.NextDouble() >= 0.5)
					{
						num2 *= -1f;
					}
					cameraShakeX += num2 * num5;
					num2 = explosions[explosionID].cameraShakeY + explosions[explosionID].cameraShakeY * (float)MainGame.mainRandom.NextDouble() * explosions[explosionID].cameraShakeVariance;
					if (MainGame.mainRandom.NextDouble() >= 0.5)
					{
						num2 *= -1f;
					}
					cameraShakeY += num2 * num5;
					num2 = explosions[explosionID].cameraShakeZ + explosions[explosionID].cameraShakeZ * (float)MainGame.mainRandom.NextDouble() * explosions[explosionID].cameraShakeVariance;
					if (MainGame.mainRandom.NextDouble() >= 0.5)
					{
						num2 *= -1f;
					}
					cameraShakeZ += num2 * num5;
				}
				if (gameExplosions[num].status == 1)
				{
					Splash_Damage_From_Explosion((byte)gameExplosions[num].explosionID, gameExplosions[num].playerID, gameExplosions[num].x, gameExplosions[num].y, gameExplosions[num].z, threadID);
					Process_Explosion_On_Game_Objects((byte)explosionID, gameExplosions[num].playerID, gameExplosions[num].x, gameExplosions[num].y, gameExplosions[num].z, explosions[explosionID].splashFalloff * explosions[explosionID].splashFalloff, threadID);
					gameExplosions[num].status = 2;
				}
			}
		}
	}

	public void New_Explosion(byte explosionID, ushort playerID, float x, float y, float z, byte threadID)
	{
		for (ushort num = 0; num < numGameExplosions; num++)
		{
			if (gameExplosions[num].status == 0)
			{
				gameExplosions[num].curTime = 0f;
				gameExplosions[num].explosionID = explosionID;
				gameExplosions[num].x = x;
				gameExplosions[num].y = y;
				gameExplosions[num].z = z;
				gameExplosions[num].status = 1;
				gameExplosions[num].playerID = playerID;
				mainC.soundsMain.Play_Sound(explosions[explosionID].sound, x, y, z, 0f, 0f, 0f);
				mainC.renderingMain.Add_Particle((byte)explosions[explosionID].particleID, x, y, z, 0f, 0f, 1f, 0f, 0f, 0f);
				break;
			}
		}
	}

	public void Splash_Damage_From_Explosion(byte explosionID, ushort playerCausingExplosion, float startX, float startY, float startZ, byte threadID)
	{
		ushort num = 0;
		float num2 = 0f;
		Vector3 InitialRayStart = default(Vector3);
		Vector3 InitialRayEnd = default(Vector3);
		Vector3 IntersectPosition = default(Vector3);
		Vector3 IntersectNormal = default(Vector3);
		float num3 = explosions[explosionID].splashFalloff * explosions[explosionID].splashFalloff;
		InitialRayStart.X = startX;
		InitialRayStart.Y = startY;
		InitialRayStart.Z = startZ;
		for (short num4 = 0; num4 < MainGame.maxGamePlayers; num4++)
		{
			if ((global::Players.Players.players[num4].onmap & 0xC) > 0)
			{
				float num5 = global::Players.Players.players[num4].charP.position.v[0] - startX;
				float num6 = global::Players.Players.players[num4].charP.position.v[1] - startY;
				float num7 = global::Players.Players.players[num4].charP.position.v[2] + 2.5f - startZ;
				float num8 = num5 * num5 + num6 * num6 + num7 * num7;
				if (num8 < num3)
				{
					int num9 = 0;
					num2 = num8;
					short returnValueZoneCheckIndex = 0;
					InitialRayEnd.X = global::Players.Players.players[num4].charP.position.v[0];
					InitialRayEnd.Y = global::Players.Players.players[num4].charP.position.v[1];
					InitialRayEnd.Z = global::Players.Players.players[num4].charP.position.v[2] + 2.5f;
					ushort returnValueZoneCheckObjID;
					while (num9 == 0 && mainC.zonesMain.Check_Zones_For_Point(startX, startY, startZ, 5f, returnValueZoneCheckIndex, 1, out returnValueZoneCheckIndex, out returnValueZoneCheckObjID))
					{
						ushort numObjects = Zones.zones[returnValueZoneCheckObjID].zoneList.numObjects;
						for (num = 0; num < numObjects; num++)
						{
							if (mainC.collisionMain.Check_Polygon_Ray_Collision_Projectile(Zones.zones[returnValueZoneCheckObjID].zoneList.oList[num], -1, ref InitialRayStart, ref InitialRayEnd, ref Zones.zones[returnValueZoneCheckObjID].zoneList.matrixList[num], out var distance, out IntersectPosition, out IntersectNormal, out var _, threadID) && distance * distance < num2)
							{
								num9 = 8;
								num = numObjects;
							}
						}
					}
					if (num9 == 0)
					{
						float num10 = (float)Math.Sqrt(num8);
						num5 /= num10;
						num6 /= num10;
						num7 /= num10;
						num8 /= num3;
						num8 *= num8;
						num10 = (1f - num8) * explosions[explosionID].impactForce;
						num8 = (1f - num8) * explosions[explosionID].damage[global::Players.Players.players[num4].damageType];
						num8 /= global::Players.Players.players[num4].jt1[global::Players.Players.players[num4].humanoidBackJoint].damageMultiplier;
						global::Players.Players.players[num4].impactX += num10 * num5;
						global::Players.Players.players[num4].impactY += num10 * num6;
						global::Players.Players.players[num4].impactZ += 2f * num10 * num7;
						global::Players.Players.players[num4].deathFlyBackPercentage = 1.5f;
						pfbV1T[threadID].v[0] = global::Players.Players.players[num4].charP.position.v[0];
						pfbV1T[threadID].v[1] = global::Players.Players.players[num4].charP.position.v[1];
						pfbV1T[threadID].v[2] = global::Players.Players.players[num4].charP.position.v[2];
						mainC.playersMain.Player_Hit(num4, (short)playerCausingExplosion, -1, num8, -1, pfbV1T[threadID], threadID);
						if (playerCausingExplosion == 0 && (global::Players.Players.players[num4].teamMask & global::Players.Players.enemyTeamMask) != 0)
						{
							global::Weapons.Weapons.showTargetCrosshairTimer = 0.25f;
						}
					}
				}
			}
		}
	}

	public void Process_Explosion_On_Game_Objects(ushort explosionID, ushort playerCausingExplosion, float startX, float startY, float startZ, float splashDistance, byte threadID)
	{
		ushort num = 0;
		ushort num2 = 0;
		float num3 = 0f;
		Vector3 InitialRayStart = default(Vector3);
		Vector3 InitialRayEnd = default(Vector3);
		Vector3 IntersectPosition = default(Vector3);
		Vector3 IntersectNormal = default(Vector3);
		InitialRayStart.X = startX;
		InitialRayStart.Y = startY;
		InitialRayStart.Z = startZ;
		for (ushort num4 = 0; num4 < global::GameObjects.GameObjects.numGameObjects; num4++)
		{
			float num5 = global::GameObjects.GameObjects.Game_Objects[num4].phy.x - startX;
			float num6 = global::GameObjects.GameObjects.Game_Objects[num4].phy.y - startY;
			float num7 = global::GameObjects.GameObjects.Game_Objects[num4].phy.z - startZ;
			global::GameObjects.GameObjects.Game_Objects[num4].distanceFromExplosion = num5 * num5 + num6 * num6 + num7 * num7;
			global::GameObjects.GameObjects.sortPosition[num4] = num4;
		}
		int num8 = 0;
		while (num8 == 0)
		{
			num8 = 1;
			int num9 = global::GameObjects.GameObjects.numGameObjects - 1;
			int num10 = 0;
			int num11 = 1;
			while (num10 < num9)
			{
				if (global::GameObjects.GameObjects.Game_Objects[global::GameObjects.GameObjects.sortPosition[num10]].distanceFromExplosion > global::GameObjects.GameObjects.Game_Objects[global::GameObjects.GameObjects.sortPosition[num11]].distanceFromExplosion)
				{
					num8 = 0;
					int num12 = global::GameObjects.GameObjects.sortPosition[num10];
					global::GameObjects.GameObjects.sortPosition[num10] = global::GameObjects.GameObjects.sortPosition[num11];
					global::GameObjects.GameObjects.sortPosition[num11] = (ushort)num12;
				}
				num10++;
				num11++;
			}
		}
		for (int num10 = 0; num10 < global::GameObjects.GameObjects.numGameObjects; num10++)
		{
			ushort num4 = global::GameObjects.GameObjects.sortPosition[num10];
			float distanceFromExplosion;
			if ((distanceFromExplosion = global::GameObjects.GameObjects.Game_Objects[num4].distanceFromExplosion) >= splashDistance)
			{
				break;
			}
			if (global::GameObjects.GameObjects.Game_Objects[num4].state <= 0)
			{
				continue;
			}
			num8 = 0;
			num3 = distanceFromExplosion;
			short returnValueZoneCheckIndex = 0;
			InitialRayEnd.X = global::GameObjects.GameObjects.Game_Objects[num4].phy.x;
			InitialRayEnd.Y = global::GameObjects.GameObjects.Game_Objects[num4].phy.y;
			InitialRayEnd.Z = global::GameObjects.GameObjects.Game_Objects[num4].phy.z;
			ushort returnValueZoneCheckObjID;
			while (num8 == 0 && mainC.zonesMain.Check_Zones_For_Point(startX, startY, startZ, 5f, returnValueZoneCheckIndex, 1, out returnValueZoneCheckIndex, out returnValueZoneCheckObjID))
			{
				ushort numObjects = Zones.zones[returnValueZoneCheckObjID].zoneList.numObjects;
				for (num2 = 0; num2 < numObjects; num2++)
				{
					if (mainC.collisionMain.Check_Polygon_Ray_Collision_Projectile(Zones.zones[returnValueZoneCheckObjID].zoneList.oList[num2], -1, ref InitialRayStart, ref InitialRayEnd, ref Zones.zones[returnValueZoneCheckObjID].zoneList.matrixList[num2], out var distance, out IntersectPosition, out IntersectNormal, out var _, threadID) && distance * distance < num3)
					{
						num = mainC.maingameMain.Get_Game_Item_Index(Zones.zones[returnValueZoneCheckObjID].zoneList.gidList[num2]);
						if (num != num4)
						{
							num8 = 8;
						}
					}
				}
			}
			if (num8 == 0)
			{
				distanceFromExplosion /= splashDistance;
				distanceFromExplosion *= distanceFromExplosion;
				distanceFromExplosion = (1f - distanceFromExplosion) * explosions[explosionID].damage[global::GameObjects.GameObjects.Game_Objects[num4].damageType];
				mainC.gameobjectMain.Game_Object_Shot(playerCausingExplosion, num4, distanceFromExplosion, isExplosion: true, threadID);
			}
		}
	}
}
