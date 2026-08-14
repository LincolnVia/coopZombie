using System;
using System.Globalization;
using System.IO;
using Collision;
using GameObjects;
using InputHandler;
using MainGame;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using Microsoft.Xna.Framework.Net;
using Models;
using Networking;
using Physics;
using Players;
using Rendering;
using Structs;
using Util;
using WindowsGame1;

namespace Weapons;

public class Weapons
{
	public static bool projectileViewTimer = false;

	public static bool scopeViewEnabled;

	public static bool weaponViewEnabled;

	public static bool mpSendWeaponFiredMsg = false;

	public static bool destroy = true;

	public static sbyte[] laserLights;

	public static sbyte[,] laserLightsSorted;

	public static byte curMuzzleFlashTexture;

	public static byte numMuzzleFlashTexturesMainPlayer;

	public static byte numBallisticStrikeTypes;

	public static byte curBallisticStrike = 0;

	public static byte numBallisticStrikes = 0;

	public static byte statCounter;

	public static byte numWeaponMounts;

	public static byte numWeaponModifiers;

	public static byte maxBarrels = 1;

	public static byte mpNumFiredMsgsToSend = 0;

	public static byte numLaserLights = 2;

	public static byte curLaserLight;

	public static byte numWeapons = 0;

	public static byte numAllocatedWeapons = 0;

	public static byte numCrossHairs = 1;

	public static byte numAmmoClips = 0;

	public static byte curAmmoClips = 0;

	public static byte numScopes = 0;

	public static byte numForeGrips = 0;

	public static byte numBarrels = 0;

	public static byte numEnergyDevices = 0;

	public static byte[] lockedWeaponSkinLevels;

	public static byte[] lockedWeaponLevels;

	public static byte[] weaponSelectWeaponIDs;

	public static byte[] numActiveAmmoLights;

	public static byte[] laserDepth;

	public static byte[,] bulletActive;

	public static short numAmmo;

	public static double fs;

	public static short currentBullet;

	public static ushort numWeaponAttachments;

	public static ushort weaponAvailable;

	public static ushort[] muzzleFlashTexturesMainPlayer;

	public static float mobilityFactor;

	public static float movementSpeedFactor;

	public static float laserStartDistance;

	public static float laserDistance;

	public static float showTargetCrosshairTimer;

	public static float viewFollowingTimer;

	public static float recoilUp;

	public static float recoilSide;

	public static float recoilBack;

	public static float[] fau4;

	public static float[] far4;

	public static float[] far3;

	public static float[] laserPosX;

	public static float[] laserPosY;

	public static float[] laserPosZ;

	public static float[,] curModifierTime;

	public static string utilString1;

	public static string utilString2;

	public static string utilString3;

	private static StructsClass.vtex ppwV1;

	private static StructsClass.vtex ppwV2;

	private static StructsClass.vtex apffwV1;

	private static StructsClass.vtex apffwV2;

	private static StructsClass.vtex abV1;

	public static Vector4[,] ammoLightPos;

	public static Vector4[,] ammoLightColor;

	public static StructsClass.Ballistics[] bullet;

	public static StructsClass.physics lSite;

	public static StructsClass.particle_list[] bulletBoxT;

	public static StructsClass.weapon[] wp1;

	public static StructsClass.Ammo_Clips[] ammoClips;

	public static StructsClass.weapon_scope[] scopes;

	public static StructsClass.weapon_foregrip[] foreGrips;

	public static StructsClass.weapon_barrel[] barrels;

	public static StructsClass.weapon_energydevice[] energyDevices;

	public static StructsClass.Ammunition[] ammo;

	public static StructsClass.Weapon_Modifier[] wpnMod;

	public static StructsClass.Weapon_Mount_Player[] wpmMounts;

	public static StructsClass.Ballistic_Strike[] wpnStrike;

	public static StructsClass.Ballistic_Strike[] wpnStrikeConfig;

	public static StructsClass.Weapon_Attachment[] wpnAttachments;

	public static Vector3 ls1;

	public static Vector3 ls2;

	public static Vector3 ls3;

	public static Vector3 ls4;

	public static Vector3 lsCenter;

	public static Vector3 lsNormal;

	public static Vector3 lsTangent;

	public static StructsClass.vtex[] pfbV1T;

	public static StructsClass.vtex[] pfbV2T;

	public Game1.MasterCollection mainC;

	public void Init_Vars(Game1.MasterCollection masterC)
	{
		mainC = masterC;
		for (int i = 0; i < 5; i++)
		{
			bulletBoxT[i] = default(StructsClass.particle_list);
			StructsClass.Initialize_ParticleList(ref bulletBoxT[i]);
			bulletBoxT[i].v1 = new StructsClass.vtex[1];
			bulletBoxT[i].v1[0] = new StructsClass.vtex();
			bulletBoxT[i].v1[0].v[0] = 0f;
			bulletBoxT[i].v1[0].v[1] = 0f;
			bulletBoxT[i].v1[0].v[2] = 0f;
			bulletBoxT[i].numP = 1L;
			bulletBoxT[i].numUsed = 1L;
			pfbV1T[i] = new StructsClass.vtex();
			pfbV2T[i] = new StructsClass.vtex();
		}
		StructsClass.Initialize_Physics(ref lSite);
		for (int i = 0; i < 100; i++)
		{
			bullet[i] = new StructsClass.Ballistics();
			StructsClass.Initialize_Ballistics(ref bullet[i]);
		}
	}

	public void firingStopped(int pID, byte curStub)
	{
		global::MainGame.MainGame.playerVehicles[pID].weapons[curStub].fired = false;
		global::MainGame.MainGame.playerVehicles[pID].weapons[curStub].shooting = false;
		mainC.avatarMain.Set_Avatar_Animation_Stop_Interval((ushort)pID, global::MainGame.MainGame.playerVehicles[pID].weapons[curStub].secsPerBullet, mainC.gameLogic.Game_Map_Program_To_Avatar_Animation(global::MainGame.MainGame.playerVehicles[pID].weapons[curStub].AnimationFire));
		mainC.programsMain.Stop_Animation_If_Not_Looping(ref global::Players.Players.players[pID].animations, global::MainGame.MainGame.playerVehicles[pID].weapons[curStub].AnimationFire, (ushort)global::Players.Players.players[pID].programCollection);
	}

	public void firingStoppedAllPlayerWeapons(ushort pID)
	{
		for (ushort num = 0; num < global::MainGame.MainGame.playerVehicles[pID].numMounts; num++)
		{
			if (global::MainGame.MainGame.playerVehicles[pID].mounts[num].type == 1 && global::MainGame.MainGame.playerVehicles[pID].mounts[num].objectAttached == 1)
			{
				ushort objectID = global::MainGame.MainGame.playerVehicles[pID].mounts[num].objectID;
				global::MainGame.MainGame.playerVehicles[pID].weapons[objectID].fired = false;
				global::MainGame.MainGame.playerVehicles[pID].weapons[objectID].shooting = false;
				mainC.programsMain.Stop_Animation_If_Not_Looping(ref global::Players.Players.players[pID].animations, global::MainGame.MainGame.playerVehicles[pID].weapons[objectID].AnimationFire, (ushort)global::Players.Players.players[pID].programCollection);
				mainC.avatarMain.Set_Avatar_Animation_Stop_Interval(pID, global::MainGame.MainGame.playerVehicles[pID].weapons[objectID].secsPerBullet, mainC.gameLogic.Game_Map_Program_To_Avatar_Animation(global::MainGame.MainGame.playerVehicles[pID].weapons[objectID].AnimationFire));
			}
		}
	}

	public void Fire_Bullet(short playerID, byte curMount, byte curStub)
	{
		bool flag = false;
		byte b = 0;
		byte b2 = 0;
		short num = 0;
		byte uBufferID = global::Rendering.Rendering.uBufferID;
		sbyte b3 = (sbyte)global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].weaponID;
		b2 = global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].ammoIndex;
		b = ammo[b2].type;
		byte b4 = b;
		if ((b4 == 2 || b4 == 8) && (global::Players.Players.players[playerID].weaponModifier & 1) == 1)
		{
			global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].fireRateAdjustment = wpnMod[0].amount;
		}
		int num2 = wp1[b3].numBarrels;
		if (num2 > global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].currentRounds)
		{
			global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].currentRounds = 0;
		}
		int j;
		if (!wp1[b3].ChamberAfterShot)
		{
			float num3 = global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].secsPerBullet / global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].fireRateAdjustment;
			double num4 = (double)global::MainGame.MainGame.gameTime - global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].firingStart;
			if (!global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].fired && (double)global::MainGame.MainGame.gameTime - global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].firingStart >= (double)num3)
			{
				num4 = num3;
				global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].firingStart = (double)global::MainGame.MainGame.gameTime - num4;
			}
			global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].fired = true;
			while (num4 >= (double)num3 && (global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].currentRounds > 0 || wp1[b3].unLimitedAmmo))
			{
				num4 -= (double)num3;
				global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].firingStart += num3;
				byte b5 = 0;
				for (int i = 0; i < wp1[b3].numBarrels; i++)
				{
					bool flag2 = false;
					byte b6 = 0;
					for (j = 0; j < global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].shotCount; j++)
					{
						if ((bulletActive[uBufferID, currentBullet] & 3) > 0)
						{
							j = currentBullet++;
							while (currentBullet < 100 && (bulletActive[uBufferID, currentBullet] & 3) > 0)
							{
								currentBullet++;
							}
							if (currentBullet >= 100)
							{
								currentBullet = 0;
								while (currentBullet < j && (bulletActive[uBufferID, currentBullet] & 3) > 0)
								{
									currentBullet++;
								}
							}
							if ((bulletActive[uBufferID, currentBullet] & 3) > 0)
							{
								num4 = (double)num3 - 1.0;
								i = wp1[b3].numBarrels;
								break;
							}
						}
						flag2 = true;
						bullet[currentBullet].ammoType = b;
						bullet[currentBullet].ammoIndex = b2;
						bullet[currentBullet].weaponID = (byte)b3;
						bullet[currentBullet].barrelID = (byte)i;
						b6++;
						bullet[currentBullet].phys1.position.v[0] = global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].offset[i, 2].x + global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].posX;
						bullet[currentBullet].phys1.position.v[1] = global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].offset[i, 2].y + global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].posY;
						bullet[currentBullet].phys1.position.v[2] = global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].offset[i, 2].z + global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].posZ;
						bullet[currentBullet].startX[uBufferID] = bullet[currentBullet].phys1.position.v[0];
						bullet[currentBullet].startY[uBufferID] = bullet[currentBullet].phys1.position.v[1];
						bullet[currentBullet].startZ[uBufferID] = bullet[currentBullet].phys1.position.v[2];
						float num5 = (float)global::MainGame.MainGame.mainRandom.Next(0, 32000) / 32000f * global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].shootingAccuracy * global::Players.Players.players[playerID].shootingAccuracy * ((float)Math.PI / 180f);
						float num6 = (float)global::MainGame.MainGame.mainRandom.Next(0, 32000) / 32000f * ((float)Math.PI * 2f);
						float num7 = (float)Math.Cos(num6) * num5;
						float num8 = (float)Math.Sin(num6) * num5;
						float num9 = global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].offset[i, 6].x + num7;
						float num10 = global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].offset[i, 6].y + num8;
						bullet[currentBullet].phys1.angle.v[0] = num10 * 57.29578f - 90f;
						bullet[currentBullet].phys1.angle.v[2] = num9 * 57.29578f;
						float num11 = (float)Math.Sin(num10);
						switch (b)
						{
						case 3:
						case 6:
						case 12:
							bullet[currentBullet].phys1.velocity.v[0] = global::Players.Players.players[playerID].charP.velocity.v[0] + (float)((0.0 - Math.Sin(num9)) * (double)global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].muzzleVelocity * (double)num11);
							bullet[currentBullet].phys1.velocity.v[1] = global::Players.Players.players[playerID].charP.velocity.v[1] + (float)(Math.Cos(num9) * (double)global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].muzzleVelocity * (double)num11);
							bullet[currentBullet].phys1.velocity.v[2] = global::Players.Players.players[playerID].charP.velocity.v[2] + (float)((0.0 - Math.Cos(num10)) * (double)global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].muzzleVelocity);
							break;
						case 2:
							bullet[currentBullet].timer = ammo[b2].timer;
							bullet[currentBullet].phys1.velocity.v[0] = global::Players.Players.players[playerID].charP.velocity.v[0] + (float)((0.0 - Math.Sin(num9)) * (double)global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].muzzleVelocity * (double)num11);
							bullet[currentBullet].phys1.velocity.v[1] = global::Players.Players.players[playerID].charP.velocity.v[1] + (float)(Math.Cos(num9) * (double)global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].muzzleVelocity * (double)num11);
							bullet[currentBullet].phys1.velocity.v[2] = global::Players.Players.players[playerID].charP.velocity.v[2] + (float)((0.0 - Math.Cos(num10)) * (double)global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].muzzleVelocity);
							break;
						case 5:
							bullet[currentBullet].timer = 0f - ammo[b2].releaseTimer;
							break;
						case 9:
							bullet[currentBullet].phys1.velocity.v[0] = global::Players.Players.players[playerID].mv[uBufferID].M21 * global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].muzzleVelocity;
							bullet[currentBullet].phys1.velocity.v[1] = global::Players.Players.players[playerID].mv[uBufferID].M22 * global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].muzzleVelocity;
							bullet[currentBullet].phys1.velocity.v[2] = global::Players.Players.players[playerID].mv[uBufferID].M23 * global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].muzzleVelocity;
							break;
						case 11:
							bullet[currentBullet].timer = global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].ammoTimer;
							bullet[currentBullet].timer += bullet[currentBullet].timer * 0.3f * (-1f + (float)global::MainGame.MainGame.mainRandom.Next(0, 32000) / 16000f);
							bullet[currentBullet].phys1.velocity.v[0] = global::Players.Players.players[playerID].charP.velocity.v[0] * Math.Abs(global::Players.Players.players[playerID].mv[uBufferID].M21) + (float)((0.0 - Math.Sin(num9)) * (double)global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].muzzleVelocity * (double)num11);
							bullet[currentBullet].phys1.velocity.v[1] = global::Players.Players.players[playerID].charP.velocity.v[1] * Math.Abs(global::Players.Players.players[playerID].mv[uBufferID].M22) + (float)(Math.Cos(num9) * (double)global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].muzzleVelocity * (double)num11);
							bullet[currentBullet].phys1.velocity.v[2] = global::Players.Players.players[playerID].charP.velocity.v[2] * Math.Abs(global::Players.Players.players[playerID].mv[uBufferID].M23) + (float)((0.0 - Math.Cos(num10)) * (double)global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].muzzleVelocity);
							break;
						case 15:
							bullet[currentBullet].phys1.velocity.v[0] = (float)((0.0 - Math.Sin(num9)) * (double)num11);
							bullet[currentBullet].phys1.velocity.v[1] = (float)(Math.Cos(num9) * (double)num11);
							bullet[currentBullet].phys1.velocity.v[2] = (float)(0.0 - Math.Cos(num10));
							break;
						default:
							bullet[currentBullet].phys1.velocity.v[0] = global::Players.Players.players[playerID].charP.velocity.v[0] * Math.Abs(global::Players.Players.players[playerID].mv[uBufferID].M21) + (float)((0.0 - Math.Sin(num9)) * (double)global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].muzzleVelocity * (double)num11);
							bullet[currentBullet].phys1.velocity.v[1] = global::Players.Players.players[playerID].charP.velocity.v[1] * Math.Abs(global::Players.Players.players[playerID].mv[uBufferID].M22) + (float)(Math.Cos(num9) * (double)global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].muzzleVelocity * (double)num11);
							bullet[currentBullet].phys1.velocity.v[2] = global::Players.Players.players[playerID].charP.velocity.v[2] * Math.Abs(global::Players.Players.players[playerID].mv[uBufferID].M23) + (float)((0.0 - Math.Cos(num10)) * (double)global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].muzzleVelocity);
							break;
						}
						bullet[currentBullet].phys1.velocity.v[0] *= global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].muzzleVelocityAdjustment;
						bullet[currentBullet].phys1.velocity.v[1] *= global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].muzzleVelocityAdjustment;
						bullet[currentBullet].phys1.velocity.v[2] *= global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].muzzleVelocityAdjustment;
						bullet[currentBullet].phys1.fx = global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].offset[i, 5].x;
						bullet[currentBullet].phys1.fy = global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].offset[i, 5].y;
						bullet[currentBullet].phys1.fz = global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].offset[i, 5].z;
						bullet[currentBullet].phys1.initialTime = num4;
						bulletActive[uBufferID, currentBullet] = 1;
						bullet[currentBullet].playerID = playerID;
						bullet[currentBullet].tracer = 0;
						if (global::Players.Players.players[playerID].usingTracers && ++global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].tracerCnt > global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].roundsPerTracer)
						{
							bullet[currentBullet].tracer = 1;
							bullet[currentBullet].phys1.velocity.v[0] *= 0.05f;
							bullet[currentBullet].phys1.velocity.v[1] *= 0.05f;
							bullet[currentBullet].phys1.velocity.v[2] *= 0.05f;
							global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].tracerCnt = 0;
						}
						bullet[currentBullet].phys1.acceleration.v[2] = global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].ammoAccelerationZ;
						bullet[currentBullet].rotation = 0f;
						flag = true;
						num = currentBullet;
						currentBullet++;
						if (currentBullet >= 100)
						{
							currentBullet = 0;
						}
					}
					if (flag2 && !wp1[b3].unLimitedAmmo)
					{
						b5 += b6;
						global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].currentRounds--;
					}
					if (global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].currentRounds < 1)
					{
						global::Players.Players.players[playerID].needToReload = true;
						break;
					}
				}
				global::MainGame.MainGame.gameData.players[playerID].shotsFired += b5;
			}
		}
		else
		{
			if (!global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].roundChambered)
			{
				global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].needToChamber = true;
				global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].roundChambered = false;
				firingStopped(playerID, curStub);
				return;
			}
			float num6 = (float)global::MainGame.MainGame.mainRandom.Next(0, 32000) / 32000f * ((float)Math.PI * 2f);
			float num12 = (float)Math.PI * 2f / (float)global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].shotCount;
			byte b5 = 0;
			for (int i = 0; i < wp1[b3].numBarrels; i++)
			{
				bool flag2 = false;
				byte b6 = 0;
				for (j = 0; j < global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].shotCount; j++)
				{
					if ((bulletActive[uBufferID, currentBullet] & 3) > 0)
					{
						j = currentBullet++;
						while (currentBullet < 100 && (bulletActive[uBufferID, currentBullet] & 3) > 0)
						{
							currentBullet++;
						}
						if (currentBullet >= 100)
						{
							currentBullet = 0;
							while (currentBullet < j && (bulletActive[uBufferID, currentBullet] & 3) > 0)
							{
								currentBullet++;
							}
						}
						if ((bulletActive[uBufferID, currentBullet] & 3) > 0)
						{
							i = wp1[b3].numBarrels;
							break;
						}
					}
					flag2 = true;
					bullet[currentBullet].ammoType = b;
					bullet[currentBullet].ammoIndex = b2;
					bullet[currentBullet].weaponID = (byte)b3;
					bullet[currentBullet].barrelID = (byte)i;
					b6++;
					bullet[currentBullet].phys1.position.v[0] = global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].offset[i, 2].x + global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].posX;
					bullet[currentBullet].phys1.position.v[1] = global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].offset[i, 2].y + global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].posY;
					bullet[currentBullet].phys1.position.v[2] = global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].offset[i, 2].z + global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].posZ;
					bullet[currentBullet].startX[uBufferID] = bullet[currentBullet].phys1.position.v[0];
					bullet[currentBullet].startY[uBufferID] = bullet[currentBullet].phys1.position.v[1];
					bullet[currentBullet].startZ[uBufferID] = bullet[currentBullet].phys1.position.v[2];
					float num5 = (float)global::MainGame.MainGame.mainRandom.Next(0, 32000) / 32000f * global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].shootingAccuracy * global::Players.Players.players[playerID].shootingAccuracy * ((float)Math.PI / 180f);
					float num7 = (float)Math.Cos(num6) * num5;
					float num8 = (float)Math.Sin(num6) * num5;
					num6 += num12;
					float num9 = global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].offset[i, 6].x + num7;
					float num10 = global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].offset[i, 6].y + num8;
					bullet[currentBullet].phys1.angle.v[0] = num10 * 57.29578f - 90f;
					bullet[currentBullet].phys1.angle.v[2] = num9 * 57.29578f;
					float num11 = (float)Math.Sin(num10);
					switch (b)
					{
					case 3:
					case 6:
					case 12:
						bullet[currentBullet].phys1.velocity.v[0] = global::Players.Players.players[playerID].charP.velocity.v[0] + (float)((0.0 - Math.Sin(num9)) * (double)global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].muzzleVelocity * (double)num11);
						bullet[currentBullet].phys1.velocity.v[1] = global::Players.Players.players[playerID].charP.velocity.v[1] + (float)(Math.Cos(num9) * (double)global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].muzzleVelocity * (double)num11);
						bullet[currentBullet].phys1.velocity.v[2] = global::Players.Players.players[playerID].charP.velocity.v[2] + (float)((0.0 - Math.Cos(num10)) * (double)global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].muzzleVelocity);
						break;
					case 2:
						bullet[currentBullet].timer = ammo[b2].timer;
						bullet[currentBullet].phys1.velocity.v[0] = global::Players.Players.players[playerID].charP.velocity.v[0] + (float)((0.0 - Math.Sin(num9)) * (double)global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].muzzleVelocity * (double)num11);
						bullet[currentBullet].phys1.velocity.v[1] = global::Players.Players.players[playerID].charP.velocity.v[1] + (float)(Math.Cos(num9) * (double)global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].muzzleVelocity * (double)num11);
						bullet[currentBullet].phys1.velocity.v[2] = global::Players.Players.players[playerID].charP.velocity.v[2] + (float)((0.0 - Math.Cos(num10)) * (double)global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].muzzleVelocity);
						break;
					case 5:
						bullet[currentBullet].timer = 0f - ammo[b2].releaseTimer;
						break;
					case 9:
						bullet[currentBullet].phys1.velocity.v[0] = global::Players.Players.players[playerID].mv[uBufferID].M21 * global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].muzzleVelocity;
						bullet[currentBullet].phys1.velocity.v[1] = global::Players.Players.players[playerID].mv[uBufferID].M22 * global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].muzzleVelocity;
						bullet[currentBullet].phys1.velocity.v[2] = global::Players.Players.players[playerID].mv[uBufferID].M23 * global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].muzzleVelocity;
						break;
					case 11:
						bullet[currentBullet].timer = global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].ammoTimer;
						bullet[currentBullet].timer += bullet[currentBullet].timer * 0.2f * (-1f + (float)global::MainGame.MainGame.mainRandom.Next(0, 32000) / 16000f);
						bullet[currentBullet].phys1.velocity.v[0] = global::Players.Players.players[playerID].charP.velocity.v[0] * Math.Abs(global::Players.Players.players[playerID].mv[uBufferID].M21) + (float)((0.0 - Math.Sin(num9)) * (double)global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].muzzleVelocity * (double)num11);
						bullet[currentBullet].phys1.velocity.v[1] = global::Players.Players.players[playerID].charP.velocity.v[1] * Math.Abs(global::Players.Players.players[playerID].mv[uBufferID].M22) + (float)(Math.Cos(num9) * (double)global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].muzzleVelocity * (double)num11);
						bullet[currentBullet].phys1.velocity.v[2] = global::Players.Players.players[playerID].charP.velocity.v[2] * Math.Abs(global::Players.Players.players[playerID].mv[uBufferID].M23) + (float)((0.0 - Math.Cos(num10)) * (double)global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].muzzleVelocity);
						break;
					case 15:
						bullet[currentBullet].phys1.velocity.v[0] = (float)((0.0 - Math.Sin(num9)) * (double)num11);
						bullet[currentBullet].phys1.velocity.v[1] = (float)(Math.Cos(num9) * (double)num11);
						bullet[currentBullet].phys1.velocity.v[2] = (float)(0.0 - Math.Cos(num10));
						break;
					default:
						bullet[currentBullet].phys1.velocity.v[0] = global::Players.Players.players[playerID].charP.velocity.v[0] * Math.Abs(global::Players.Players.players[playerID].mv[uBufferID].M21) + (float)((0.0 - Math.Sin(num9)) * (double)global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].muzzleVelocity * (double)num11);
						bullet[currentBullet].phys1.velocity.v[1] = global::Players.Players.players[playerID].charP.velocity.v[1] * Math.Abs(global::Players.Players.players[playerID].mv[uBufferID].M22) + (float)(Math.Cos(num9) * (double)global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].muzzleVelocity * (double)num11);
						bullet[currentBullet].phys1.velocity.v[2] = global::Players.Players.players[playerID].charP.velocity.v[2] * Math.Abs(global::Players.Players.players[playerID].mv[uBufferID].M23) + (float)((0.0 - Math.Cos(num10)) * (double)global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].muzzleVelocity);
						break;
					}
					bullet[currentBullet].phys1.velocity.v[0] *= global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].muzzleVelocityAdjustment;
					bullet[currentBullet].phys1.velocity.v[1] *= global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].muzzleVelocityAdjustment;
					bullet[currentBullet].phys1.velocity.v[2] *= global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].muzzleVelocityAdjustment;
					bullet[currentBullet].phys1.fx = global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].offset[i, 5].x;
					bullet[currentBullet].phys1.fy = global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].offset[i, 5].y;
					bullet[currentBullet].phys1.fz = global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].offset[i, 5].z;
					bulletActive[uBufferID, currentBullet] = 1;
					bullet[currentBullet].playerID = playerID;
					bullet[currentBullet].tracer = 0;
					if (global::Players.Players.players[playerID].usingTracers && ++global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].tracerCnt > global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].roundsPerTracer)
					{
						bullet[currentBullet].tracer = 1;
						bullet[currentBullet].phys1.velocity.v[0] *= 0.05f;
						bullet[currentBullet].phys1.velocity.v[1] *= 0.05f;
						bullet[currentBullet].phys1.velocity.v[2] *= 0.05f;
						global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].tracerCnt = 0;
					}
					bullet[currentBullet].phys1.acceleration.v[2] = global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].ammoAccelerationZ;
					bullet[currentBullet].rotation = 0f;
					flag = true;
					num = currentBullet;
					currentBullet++;
					if (currentBullet >= 100)
					{
						currentBullet = 0;
					}
				}
				if (flag2 && !wp1[b3].unLimitedAmmo)
				{
					b5 += b6;
					global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].currentRounds--;
				}
				if (global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].currentRounds < 1)
				{
					global::Players.Players.players[playerID].needToReload = true;
					break;
				}
			}
			global::MainGame.MainGame.gameData.players[playerID].shotsFired += b5;
		}
		if (!flag)
		{
			return;
		}
		if (wp1[b3].ChamberAfterShot)
		{
			global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].needToChamber = true;
			global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].roundChambered = false;
		}
		j = global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].AnimationFire;
		if (global::Players.Players.players[playerID].animations[j].status != 2)
		{
			mainC.programsMain.Start_Animation((ushort)playerID, ref global::Players.Players.players[playerID].jt1, ref global::Players.Players.players[playerID].animations, global::Players.Players.players[playerID].programCollection, j, 1f, 1f);
			if (!wp1[b3].ChamberAfterShot)
			{
				mainC.avatarMain.Avatar_Movement_By_ID((ushort)playerID, queue: true, 1, global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].fullyAutomatic, mainC.gameLogic.Game_Map_Program_To_Avatar_Animation((ushort)j), cancelOtherGroupAnimations: false);
			}
		}
		bullet[num].lightID = -1;
		if (wp1[b3].snd_fire != null)
		{
			mainC.soundsMain.Play_Sound(wp1[b3].snd_fire, bullet[num].phys1.position.v[0], bullet[num].phys1.position.v[1], bullet[num].phys1.position.v[2], global::Players.Players.players[playerID].charP.velocity.v[0], global::Players.Players.players[playerID].charP.velocity.v[1], global::Players.Players.players[playerID].charP.velocity.v[2]);
		}
		if (ammo[b2].sound != null)
		{
			bullet[num].soundID = mainC.soundsMain.Play_Sound(ammo[b2].sound, bullet[num].phys1.position.v[0], bullet[num].phys1.position.v[1], bullet[num].phys1.position.v[2], global::Players.Players.players[playerID].charP.velocity.v[0] + bullet[num].phys1.velocity.v[0], global::Players.Players.players[playerID].charP.velocity.v[1] + bullet[num].phys1.velocity.v[1], global::Players.Players.players[playerID].charP.velocity.v[2] + bullet[num].phys1.velocity.v[2]);
		}
		bullet[num].soundID2 = -1;
		if (ammo[b2].sound2 != null)
		{
			bullet[num].soundID2 = b2;
		}
		switch (b)
		{
		case 0:
		case 1:
			global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].muzzleFlashTimer = 0.023f;
			break;
		case 2:
			if (laserLights[curLaserLight] > -1)
			{
				bullet[laserLights[curLaserLight]].lightID = -1;
			}
			bullet[num].lightID = (sbyte)curLaserLight;
			global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].muzzleFlashTimer = 0.023f;
			j = bullet[num].ammoIndex;
			ammoLightPos[uBufferID, curLaserLight].X = bullet[num].phys1.position.v[0];
			ammoLightPos[uBufferID, curLaserLight].Y = bullet[num].phys1.position.v[1];
			ammoLightPos[uBufferID, curLaserLight].Z = bullet[num].phys1.position.v[2];
			ammoLightPos[uBufferID, curLaserLight].W = ammo[j].colorIntensity;
			ammoLightColor[uBufferID, curLaserLight].X = ammo[j].lightColor[0];
			ammoLightColor[uBufferID, curLaserLight].Y = ammo[j].lightColor[1];
			ammoLightColor[uBufferID, curLaserLight].Z = ammo[j].lightColor[2];
			ammoLightColor[uBufferID, curLaserLight].W = ammo[j].lightColor[3];
			laserLights[curLaserLight] = (sbyte)num;
			curLaserLight++;
			if (curLaserLight >= numLaserLights)
			{
				curLaserLight = 0;
			}
			global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].fireRateAdjustment -= wp1[b3].fireRateReduction;
			if (global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].fireRateAdjustment < wp1[b3].fireRateAdjLowPerc)
			{
				global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].fireRateAdjustment = wp1[b3].fireRateAdjLowPerc;
			}
			break;
		case 3:
			bullet[num].timer = ammo[b2].timer;
			bullet[num].particleTimer = ammo[b2].particleTimer;
			if (laserLights[curLaserLight] > -1)
			{
				bullet[laserLights[curLaserLight]].lightID = -1;
			}
			bullet[num].lightID = (sbyte)curLaserLight;
			j = bullet[num].ammoIndex;
			ammoLightPos[uBufferID, curLaserLight].X = bullet[num].phys1.position.v[0];
			ammoLightPos[uBufferID, curLaserLight].Y = bullet[num].phys1.position.v[1];
			ammoLightPos[uBufferID, curLaserLight].Z = bullet[num].phys1.position.v[2];
			ammoLightPos[uBufferID, curLaserLight].W = ammo[j].colorIntensity;
			ammoLightColor[uBufferID, curLaserLight].X = ammo[j].lightColor[0];
			ammoLightColor[uBufferID, curLaserLight].Y = ammo[j].lightColor[1];
			ammoLightColor[uBufferID, curLaserLight].Z = ammo[j].lightColor[2];
			ammoLightColor[uBufferID, curLaserLight].W = ammo[j].lightColor[3];
			laserLights[curLaserLight] = (sbyte)num;
			curLaserLight++;
			if (curLaserLight >= numLaserLights)
			{
				curLaserLight = 0;
			}
			break;
		case 4:
			bullet[num].timer = ammo[b2].timer;
			bullet[num].phys1.angularVelocity.v[0] = 360f;
			j = wp1[b3].AnimationThrow;
			global::Players.Players.players[playerID].animations[j].callBack = (byte)global::Players.Players.players[playerID].wpnIndex;
			mainC.programsMain.Start_Animation((ushort)playerID, ref global::Players.Players.players[playerID].jt1, ref global::Players.Players.players[playerID].animations, global::Players.Players.players[playerID].programCollection, j, 1f, 1f);
			global::Players.Players.players[playerID].playerIsMoving = 256;
			break;
		case 5:
			global::MainGame.MainGame.playerVehicles[0].weapons[curStub].fired = false;
			break;
		case 6:
		case 12:
		{
			ref Matrix reference = ref bullet[num].mv[uBufferID];
			reference = Vehicles.vehicles[global::Players.Players.players[playerID].curVehicle].mounts[curMount].mvCurrent[uBufferID] * global::MainGame.MainGame.playerVehicles[playerID].mv[uBufferID];
			bullet[num].rot = Quaternion.CreateFromRotationMatrix(bullet[num].mv[uBufferID]);
			mainC.gameLogic.Game_Misc(7);
			break;
		}
		case 7:
		{
			ref Matrix reference2 = ref bullet[num].mv[uBufferID];
			reference2 = Vehicles.vehicles[global::Players.Players.players[playerID].curVehicle].mounts[curMount].mvCurrent[uBufferID] * global::MainGame.MainGame.playerVehicles[playerID].mv[uBufferID];
			bullet[num].mv[uBufferID].M41 = 0f;
			bullet[num].mv[uBufferID].M42 = 0f;
			bullet[num].mv[uBufferID].M43 = 0f;
			break;
		}
		case 8:
			if (laserLights[curLaserLight] > -1)
			{
				bullet[laserLights[curLaserLight]].lightID = -1;
			}
			bullet[num].lightID = (sbyte)curLaserLight;
			j = bullet[num].ammoIndex;
			ammoLightPos[uBufferID, curLaserLight].X = bullet[num].phys1.position.v[0];
			ammoLightPos[uBufferID, curLaserLight].Y = bullet[num].phys1.position.v[1];
			ammoLightPos[uBufferID, curLaserLight].Z = bullet[num].phys1.position.v[2];
			ammoLightPos[uBufferID, curLaserLight].W = ammo[j].colorIntensity;
			ammoLightColor[uBufferID, curLaserLight].X = ammo[j].lightColor[0];
			ammoLightColor[uBufferID, curLaserLight].Y = ammo[j].lightColor[1];
			ammoLightColor[uBufferID, curLaserLight].Z = ammo[j].lightColor[2];
			ammoLightColor[uBufferID, curLaserLight].W = ammo[j].lightColor[3];
			laserLights[curLaserLight] = (sbyte)num;
			curLaserLight++;
			if (curLaserLight >= numLaserLights)
			{
				curLaserLight = 0;
			}
			bullet[num].timer = 0f;
			global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].fireRateAdjustment -= wp1[b3].fireRateReduction;
			if (global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].fireRateAdjustment < wp1[b3].fireRateAdjLowPerc)
			{
				global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].fireRateAdjustment = wp1[b3].fireRateAdjLowPerc;
			}
			mainC.avatarMain.Avatar_Movement_By_List_ID((byte)playerID, 1, loop: true, 7, cancelOtherGroupAnimations: true);
			break;
		case 9:
			bullet[num].timer = ammo[b2].timer;
			bulletActive[uBufferID, num] = 2;
			break;
		case 11:
			global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].muzzleFlashTimer = 0.023f;
			global::Rendering.Rendering.npn.v[0] = wp1[b3].pfx1;
			global::Rendering.Rendering.npn.v[1] = wp1[b3].pfx2;
			global::Rendering.Rendering.npn.v[2] = wp1[b3].pfx3;
			break;
		case 15:
			bullet[num].phys1.position.v[0] = (int)curStub;
			bullet[num].phys1.position.v[1] = (int)curMount;
			bullet[num].timer = ammo[b2].timer;
			break;
		case 10:
		case 13:
		case 14:
			break;
		}
	}

	public bool Fire_Bullet_MainPlayer(byte curMount, byte curStub)
	{
		bool flag = false;
		byte b = 0;
		byte b2 = 0;
		byte b3 = 0;
		byte b4 = 0;
		short num = 0;
		float num2 = 0f;
		float num3 = 0f;
		byte uBufferID = global::Rendering.Rendering.uBufferID;
		byte weaponID = global::MainGame.MainGame.playerVehicles[0].weapons[curStub].weaponID;
		b3 = global::MainGame.MainGame.playerVehicles[0].weapons[curStub].ammoIndex;
		b2 = ammo[b3].type;
		byte b5 = b2;
		if ((b5 == 2 || b5 == 8) && (global::Players.Players.players[b].weaponModifier & 1) == 1)
		{
			global::MainGame.MainGame.playerVehicles[0].weapons[curStub].fireRateAdjustment = wpnMod[0].amount;
		}
		int num4 = wp1[weaponID].numBarrels;
		if (num4 > global::MainGame.MainGame.playerVehicles[0].weapons[curStub].currentRounds)
		{
			global::MainGame.MainGame.playerVehicles[0].weapons[curStub].currentRounds = 0;
		}
		if (!wp1[weaponID].ChamberAfterShot)
		{
			float num5 = global::MainGame.MainGame.playerVehicles[0].weapons[curStub].secsPerBullet / global::MainGame.MainGame.playerVehicles[0].weapons[curStub].fireRateAdjustment;
			double num6 = (double)global::MainGame.MainGame.gameTime - global::MainGame.MainGame.playerVehicles[0].weapons[curStub].firingStart;
			if (!global::MainGame.MainGame.playerVehicles[0].weapons[curStub].fired && (double)global::MainGame.MainGame.gameTime - global::MainGame.MainGame.playerVehicles[0].weapons[curStub].firingStart >= (double)num5)
			{
				num6 = num5;
				global::MainGame.MainGame.playerVehicles[0].weapons[curStub].firingStart = (double)global::MainGame.MainGame.gameTime - num6;
			}
			global::MainGame.MainGame.playerVehicles[0].weapons[curStub].fired = true;
			while (num6 >= (double)num5 && (global::MainGame.MainGame.playerVehicles[0].weapons[curStub].currentRounds > 0 || wp1[weaponID].unLimitedAmmo))
			{
				num6 -= (double)num5;
				global::MainGame.MainGame.playerVehicles[0].weapons[curStub].firingStart += num5;
				b4 = 0;
				for (int i = 0; i < wp1[weaponID].numBarrels; i++)
				{
					bool flag2 = false;
					byte b6 = 0;
					for (int j = 0; j < global::MainGame.MainGame.playerVehicles[0].weapons[curStub].shotCount; j++)
					{
						if ((bulletActive[uBufferID, currentBullet] & 3) > 0)
						{
							j = currentBullet++;
							while (currentBullet < 100 && (bulletActive[uBufferID, currentBullet] & 3) > 0)
							{
								currentBullet++;
							}
							if (currentBullet >= 100)
							{
								currentBullet = 0;
								while (currentBullet < j && (bulletActive[uBufferID, currentBullet] & 3) > 0)
								{
									currentBullet++;
								}
							}
							if ((bulletActive[uBufferID, currentBullet] & 3) > 0)
							{
								num6 = (double)num5 - 1.0;
								i = wp1[weaponID].numBarrels;
								break;
							}
						}
						flag2 = true;
						flag = true;
						num = currentBullet;
						b6++;
						bullet[currentBullet].ammoType = b2;
						bullet[currentBullet].ammoIndex = b3;
						bullet[currentBullet].weaponID = weaponID;
						bullet[currentBullet].barrelID = (byte)i;
						bullet[currentBullet].phys1.initialTime = num6;
						bullet[currentBullet].playerID = 0;
						bullet[currentBullet].tracer = 0;
						bullet[currentBullet].phys1.acceleration.v[2] = global::MainGame.MainGame.playerVehicles[0].weapons[curStub].ammoAccelerationZ;
						bullet[currentBullet].rotation = 0f;
						bulletActive[uBufferID, currentBullet] = 1;
						bullet[currentBullet].phys1.position.v[0] = global::MainGame.MainGame.playerVehicles[0].weapons[curStub].offset[i, 2].x + global::MainGame.MainGame.playerVehicles[0].weapons[curStub].posX;
						bullet[currentBullet].phys1.position.v[1] = global::MainGame.MainGame.playerVehicles[0].weapons[curStub].offset[i, 2].y + global::MainGame.MainGame.playerVehicles[0].weapons[curStub].posY;
						bullet[currentBullet].phys1.position.v[2] = global::MainGame.MainGame.playerVehicles[0].weapons[curStub].offset[i, 2].z + global::MainGame.MainGame.playerVehicles[0].weapons[curStub].posZ;
						bullet[currentBullet].startX[uBufferID] = bullet[currentBullet].phys1.position.v[0];
						bullet[currentBullet].startY[uBufferID] = bullet[currentBullet].phys1.position.v[1];
						bullet[currentBullet].startZ[uBufferID] = bullet[currentBullet].phys1.position.v[2];
						float num7 = (float)global::MainGame.MainGame.mainRandom.NextDouble() * global::MainGame.MainGame.playerVehicles[b].weapons[curStub].shootingAccuracy * global::Players.Players.players[b].shootingAccuracy * ((float)Math.PI / 180f);
						float num8 = (float)global::MainGame.MainGame.mainRandom.NextDouble() * ((float)Math.PI * 2f);
						float num9 = (float)Math.Cos(num8) * num7;
						float num10 = (float)Math.Sin(num8) * num7;
						num2 = global::MainGame.MainGame.playerVehicles[0].weapons[curStub].offset[i, 6].x + num9;
						num3 = global::MainGame.MainGame.playerVehicles[0].weapons[curStub].offset[i, 6].y + num10;
						bullet[currentBullet].phys1.angle.v[0] = num3 * 57.29578f - 90f;
						bullet[currentBullet].phys1.angle.v[2] = num2 * 57.29578f;
						recoilUp = global::MainGame.MainGame.playerVehicles[0].weapons[curStub].recoilUp[0] + (float)global::MainGame.MainGame.mainRandom.NextDouble() * global::MainGame.MainGame.playerVehicles[0].weapons[curStub].recoilUp[1];
						recoilSide = global::MainGame.MainGame.playerVehicles[0].weapons[curStub].recoilSide[0] - global::MainGame.MainGame.playerVehicles[0].weapons[curStub].recoilSide[1] + (float)global::MainGame.MainGame.mainRandom.NextDouble() * global::MainGame.MainGame.playerVehicles[0].weapons[curStub].recoilSide[2];
						recoilBack = global::MainGame.MainGame.playerVehicles[0].weapons[curStub].recoilBack[0] + (float)global::MainGame.MainGame.mainRandom.NextDouble() * global::MainGame.MainGame.playerVehicles[0].weapons[curStub].recoilBack[1];
						bullet[currentBullet].phys1.fx = global::MainGame.MainGame.playerVehicles[0].weapons[curStub].offset[i, 5].x;
						bullet[currentBullet].phys1.fy = global::MainGame.MainGame.playerVehicles[0].weapons[curStub].offset[i, 5].y;
						bullet[currentBullet].phys1.fz = global::MainGame.MainGame.playerVehicles[0].weapons[curStub].offset[i, 5].z;
						float num11 = (float)Math.Sin(num3);
						switch (b2)
						{
						case 2:
							bullet[currentBullet].timer = ammo[b3].timer;
							bullet[currentBullet].phys1.velocity.v[0] = global::Players.Players.players[b].charP.velocity.v[0] + (float)((0.0 - Math.Sin(num2)) * (double)global::MainGame.MainGame.playerVehicles[0].weapons[curStub].muzzleVelocity * (double)num11);
							bullet[currentBullet].phys1.velocity.v[1] = global::Players.Players.players[b].charP.velocity.v[1] + (float)(Math.Cos(num2) * (double)global::MainGame.MainGame.playerVehicles[0].weapons[curStub].muzzleVelocity * (double)num11);
							bullet[currentBullet].phys1.velocity.v[2] = global::Players.Players.players[b].charP.velocity.v[2] + (float)((0.0 - Math.Cos(num3)) * (double)global::MainGame.MainGame.playerVehicles[0].weapons[curStub].muzzleVelocity);
							break;
						case 3:
							bullet[currentBullet].phys1.velocity.v[0] = global::Players.Players.players[b].charP.velocity.v[0] + (float)((0.0 - Math.Sin(num2)) * (double)global::MainGame.MainGame.playerVehicles[0].weapons[curStub].muzzleVelocity * (double)num11);
							bullet[currentBullet].phys1.velocity.v[1] = global::Players.Players.players[b].charP.velocity.v[1] + (float)(Math.Cos(num2) * (double)global::MainGame.MainGame.playerVehicles[0].weapons[curStub].muzzleVelocity * (double)num11);
							bullet[currentBullet].phys1.velocity.v[2] = global::Players.Players.players[b].charP.velocity.v[2] + (float)((0.0 - Math.Cos(num3)) * (double)global::MainGame.MainGame.playerVehicles[0].weapons[curStub].muzzleVelocity);
							bullet[currentBullet].timer = ammo[b3].timer;
							bullet[currentBullet].particleTimer = ammo[b3].particleTimer;
							if (global::Networking.Networking.inGame)
							{
								ref HalfSingle reference31 = ref global::Networking.Networking.networkHS[0];
								reference31 = new HalfSingle(bullet[currentBullet].phys1.position.v[0]);
								ref HalfSingle reference32 = ref global::Networking.Networking.networkHS[1];
								reference32 = new HalfSingle(bullet[currentBullet].phys1.position.v[1]);
								ref HalfSingle reference33 = ref global::Networking.Networking.networkHS[2];
								reference33 = new HalfSingle(bullet[currentBullet].phys1.position.v[2]);
								ref HalfSingle reference34 = ref global::Networking.Networking.networkHS[3];
								reference34 = new HalfSingle(bullet[currentBullet].phys1.angle.v[0]);
								ref HalfSingle reference35 = ref global::Networking.Networking.networkHS[4];
								reference35 = new HalfSingle(bullet[currentBullet].phys1.angle.v[2]);
								ref HalfSingle reference36 = ref global::Networking.Networking.networkHS[5];
								reference36 = new HalfSingle(num2);
								ref HalfSingle reference37 = ref global::Networking.Networking.networkHS[6];
								reference37 = new HalfSingle(num3);
								global::Networking.Networking.networkBytes[0] = curMount;
								global::Networking.Networking.networkSBytes[0] = (sbyte)weaponID;
								global::Networking.Networking.networkSBytes[1] = bullet[currentBullet].tracer;
								global::Networking.Networking.networkSBytes[2] = (sbyte)b3;
								global::Networking.Networking.networkDoubles[0] = num6;
								mainC.networkingMain.XBOX_Send_Network_Message1(1);
							}
							break;
						case 4:
							bullet[currentBullet].timer = ammo[b3].timer;
							bullet[currentBullet].phys1.angularVelocity.v[0] = 360f;
							if (global::Networking.Networking.inGame)
							{
								ref HalfSingle reference17 = ref global::Networking.Networking.networkHS[0];
								reference17 = new HalfSingle(bullet[currentBullet].phys1.position.v[0]);
								ref HalfSingle reference18 = ref global::Networking.Networking.networkHS[1];
								reference18 = new HalfSingle(bullet[currentBullet].phys1.position.v[1]);
								ref HalfSingle reference19 = ref global::Networking.Networking.networkHS[2];
								reference19 = new HalfSingle(bullet[currentBullet].phys1.position.v[2]);
								ref HalfSingle reference20 = ref global::Networking.Networking.networkHS[3];
								reference20 = new HalfSingle(bullet[currentBullet].phys1.angle.v[0]);
								ref HalfSingle reference21 = ref global::Networking.Networking.networkHS[4];
								reference21 = new HalfSingle(bullet[currentBullet].phys1.angle.v[2]);
								ref HalfSingle reference22 = ref global::Networking.Networking.networkHS[5];
								reference22 = new HalfSingle(num2);
								ref HalfSingle reference23 = ref global::Networking.Networking.networkHS[6];
								reference23 = new HalfSingle(num3);
								global::Networking.Networking.networkBytes[0] = curMount;
								global::Networking.Networking.networkSBytes[0] = (sbyte)weaponID;
								global::Networking.Networking.networkSBytes[1] = bullet[currentBullet].tracer;
								global::Networking.Networking.networkSBytes[2] = (sbyte)b3;
								global::Networking.Networking.networkDoubles[0] = num6;
								mainC.networkingMain.XBOX_Send_Network_Message1(1);
							}
							break;
						case 5:
						{
							bullet[currentBullet].phys1.angle.v[0] = global::Players.Players.players[b].xRotation;
							bullet[currentBullet].phys1.angle.v[2] = global::Players.Players.players[b].zRotation;
							Matrix matrix = Matrix.CreateRotationX(global::Players.Players.players[b].xRotation * ((float)Math.PI / 180f)) * Matrix.CreateRotationZ(global::Players.Players.players[b].zRotation * ((float)Math.PI / 180f));
							bullet[currentBullet].phys1.velocity.v[0] = global::Players.Players.players[b].charP.velocity.v[0] + global::MainGame.MainGame.playerVehicles[b].weapons[curStub].muzzleVelocity * matrix.M21;
							bullet[currentBullet].phys1.velocity.v[1] = global::Players.Players.players[b].charP.velocity.v[1] + global::MainGame.MainGame.playerVehicles[b].weapons[curStub].muzzleVelocity * matrix.M22;
							bullet[currentBullet].phys1.velocity.v[2] = global::Players.Players.players[b].charP.velocity.v[2] + global::MainGame.MainGame.playerVehicles[b].weapons[curStub].muzzleVelocity * matrix.M23;
							if (global::Networking.Networking.inGame)
							{
								ref HalfSingle reference24 = ref global::Networking.Networking.networkHS[0];
								reference24 = new HalfSingle(bullet[currentBullet].phys1.position.v[0]);
								ref HalfSingle reference25 = ref global::Networking.Networking.networkHS[1];
								reference25 = new HalfSingle(bullet[currentBullet].phys1.position.v[1]);
								ref HalfSingle reference26 = ref global::Networking.Networking.networkHS[2];
								reference26 = new HalfSingle(bullet[currentBullet].phys1.position.v[2]);
								ref HalfSingle reference27 = ref global::Networking.Networking.networkHS[3];
								reference27 = new HalfSingle(bullet[currentBullet].phys1.angle.v[0]);
								ref HalfSingle reference28 = ref global::Networking.Networking.networkHS[4];
								reference28 = new HalfSingle(bullet[currentBullet].phys1.angle.v[2]);
								ref HalfSingle reference29 = ref global::Networking.Networking.networkHS[5];
								reference29 = new HalfSingle(num2);
								ref HalfSingle reference30 = ref global::Networking.Networking.networkHS[6];
								reference30 = new HalfSingle(num3);
								global::Networking.Networking.networkBytes[0] = curMount;
								global::Networking.Networking.networkSBytes[0] = (sbyte)weaponID;
								global::Networking.Networking.networkSBytes[1] = bullet[currentBullet].tracer;
								global::Networking.Networking.networkSBytes[2] = (sbyte)b3;
								global::Networking.Networking.networkDoubles[0] = -1.0;
								mainC.networkingMain.XBOX_Send_Network_Message1(1);
							}
							bullet[currentBullet].phys1.acceleration.v[2] = -32.15223f;
							bullet[currentBullet].timer = ammo[b3].releaseTimer;
							break;
						}
						case 6:
						case 12:
						{
							bullet[currentBullet].phys1.velocity.v[0] = global::Players.Players.players[b].charP.velocity.v[0] + (float)((0.0 - Math.Sin(num2)) * (double)global::MainGame.MainGame.playerVehicles[0].weapons[curStub].muzzleVelocity * (double)num11);
							bullet[currentBullet].phys1.velocity.v[1] = global::Players.Players.players[b].charP.velocity.v[1] + (float)(Math.Cos(num2) * (double)global::MainGame.MainGame.playerVehicles[0].weapons[curStub].muzzleVelocity * (double)num11);
							bullet[currentBullet].phys1.velocity.v[2] = global::Players.Players.players[b].charP.velocity.v[2] + (float)((0.0 - Math.Cos(num3)) * (double)global::MainGame.MainGame.playerVehicles[0].weapons[curStub].muzzleVelocity);
							ref Matrix reference9 = ref bullet[currentBullet].mv[uBufferID];
							reference9 = Vehicles.vehicles[global::Players.Players.players[b].curVehicle].mounts[curMount].mvCurrent[uBufferID] * global::MainGame.MainGame.playerVehicles[b].mv[uBufferID];
							bullet[currentBullet].rot = Quaternion.CreateFromRotationMatrix(bullet[currentBullet].mv[uBufferID]);
							if (global::Networking.Networking.inGame)
							{
								ref HalfSingle reference10 = ref global::Networking.Networking.networkHS[0];
								reference10 = new HalfSingle(bullet[currentBullet].phys1.position.v[0]);
								ref HalfSingle reference11 = ref global::Networking.Networking.networkHS[1];
								reference11 = new HalfSingle(bullet[currentBullet].phys1.position.v[1]);
								ref HalfSingle reference12 = ref global::Networking.Networking.networkHS[2];
								reference12 = new HalfSingle(bullet[currentBullet].phys1.position.v[2]);
								ref HalfSingle reference13 = ref global::Networking.Networking.networkHS[3];
								reference13 = new HalfSingle(bullet[currentBullet].phys1.angle.v[0]);
								ref HalfSingle reference14 = ref global::Networking.Networking.networkHS[4];
								reference14 = new HalfSingle(bullet[currentBullet].phys1.angle.v[2]);
								ref HalfSingle reference15 = ref global::Networking.Networking.networkHS[5];
								reference15 = new HalfSingle(num2);
								ref HalfSingle reference16 = ref global::Networking.Networking.networkHS[6];
								reference16 = new HalfSingle(num3);
								global::Networking.Networking.networkBytes[0] = curMount;
								global::Networking.Networking.networkSBytes[0] = (sbyte)weaponID;
								global::Networking.Networking.networkSBytes[1] = bullet[currentBullet].tracer;
								global::Networking.Networking.networkSBytes[2] = (sbyte)b3;
								global::Networking.Networking.networkDoubles[0] = num6;
								mainC.networkingMain.XBOX_Send_Network_Message1(1);
							}
							break;
						}
						case 7:
						{
							ref Matrix reference8 = ref bullet[currentBullet].mv[uBufferID];
							reference8 = Vehicles.vehicles[global::Players.Players.players[b].curVehicle].mounts[curMount].mvCurrent[uBufferID] * global::MainGame.MainGame.playerVehicles[b].mv[uBufferID];
							bullet[currentBullet].mv[uBufferID].M41 = 0f;
							bullet[currentBullet].mv[uBufferID].M42 = 0f;
							bullet[currentBullet].mv[uBufferID].M43 = 0f;
							break;
						}
						case 8:
							bullet[currentBullet].timer = 0f;
							break;
						case 9:
							bullet[currentBullet].phys1.velocity.v[0] = global::Players.Players.players[b].mv[uBufferID].M21 * global::MainGame.MainGame.playerVehicles[b].weapons[curStub].muzzleVelocity;
							bullet[currentBullet].phys1.velocity.v[1] = global::Players.Players.players[b].mv[uBufferID].M22 * global::MainGame.MainGame.playerVehicles[b].weapons[curStub].muzzleVelocity;
							bullet[currentBullet].phys1.velocity.v[2] = global::Players.Players.players[b].mv[uBufferID].M23 * global::MainGame.MainGame.playerVehicles[b].weapons[curStub].muzzleVelocity;
							break;
						case 11:
							bullet[currentBullet].timer = global::MainGame.MainGame.playerVehicles[0].weapons[curStub].ammoTimer;
							bullet[currentBullet].timer += bullet[currentBullet].timer * 0.2f * (-1f + (float)global::MainGame.MainGame.mainRandom.NextDouble() * 2f);
							bullet[currentBullet].phys1.velocity.v[0] = global::Players.Players.players[b].charP.velocity.v[0] * Math.Abs(global::Players.Players.players[b].mv[uBufferID].M21) + (float)((0.0 - Math.Sin(num2)) * (double)global::MainGame.MainGame.playerVehicles[0].weapons[curStub].muzzleVelocity * (double)num11);
							bullet[currentBullet].phys1.velocity.v[1] = global::Players.Players.players[b].charP.velocity.v[1] * Math.Abs(global::Players.Players.players[b].mv[uBufferID].M22) + (float)(Math.Cos(num2) * (double)global::MainGame.MainGame.playerVehicles[0].weapons[curStub].muzzleVelocity * (double)num11);
							bullet[currentBullet].phys1.velocity.v[2] = global::Players.Players.players[b].charP.velocity.v[2] * Math.Abs(global::Players.Players.players[b].mv[uBufferID].M23) + (float)((0.0 - Math.Cos(num3)) * (double)global::MainGame.MainGame.playerVehicles[0].weapons[curStub].muzzleVelocity);
							break;
						case 14:
							bullet[currentBullet].phys1.velocity.v[0] = global::Players.Players.players[b].charP.velocity.v[0] * Math.Abs(global::Players.Players.players[b].mv[uBufferID].M21) + (float)((0.0 - Math.Sin(num2)) * (double)global::MainGame.MainGame.playerVehicles[0].weapons[curStub].muzzleVelocity * (double)num11);
							bullet[currentBullet].phys1.velocity.v[1] = global::Players.Players.players[b].charP.velocity.v[1] * Math.Abs(global::Players.Players.players[b].mv[uBufferID].M22) + (float)(Math.Cos(num2) * (double)global::MainGame.MainGame.playerVehicles[0].weapons[curStub].muzzleVelocity * (double)num11);
							bullet[currentBullet].phys1.velocity.v[2] = global::Players.Players.players[b].charP.velocity.v[2] * Math.Abs(global::Players.Players.players[b].mv[uBufferID].M23) + (float)((0.0 - Math.Cos(num3)) * (double)global::MainGame.MainGame.playerVehicles[0].weapons[curStub].muzzleVelocity);
							if (global::Networking.Networking.inGame)
							{
								ref HalfSingle reference = ref global::Networking.Networking.networkHS[0];
								reference = new HalfSingle(bullet[currentBullet].phys1.position.v[0]);
								ref HalfSingle reference2 = ref global::Networking.Networking.networkHS[1];
								reference2 = new HalfSingle(bullet[currentBullet].phys1.position.v[1]);
								ref HalfSingle reference3 = ref global::Networking.Networking.networkHS[2];
								reference3 = new HalfSingle(bullet[currentBullet].phys1.position.v[2]);
								ref HalfSingle reference4 = ref global::Networking.Networking.networkHS[3];
								reference4 = new HalfSingle(bullet[currentBullet].phys1.angle.v[0]);
								ref HalfSingle reference5 = ref global::Networking.Networking.networkHS[4];
								reference5 = new HalfSingle(bullet[currentBullet].phys1.angle.v[2]);
								ref HalfSingle reference6 = ref global::Networking.Networking.networkHS[5];
								reference6 = new HalfSingle(num2);
								ref HalfSingle reference7 = ref global::Networking.Networking.networkHS[6];
								reference7 = new HalfSingle(num3);
								global::Networking.Networking.networkBytes[0] = curMount;
								global::Networking.Networking.networkSBytes[0] = (sbyte)weaponID;
								global::Networking.Networking.networkSBytes[1] = bullet[currentBullet].tracer;
								global::Networking.Networking.networkSBytes[2] = (sbyte)b3;
								global::Networking.Networking.networkDoubles[0] = num6;
								mainC.networkingMain.XBOX_Send_Network_Message1(1);
							}
							break;
						case 15:
							bullet[currentBullet].phys1.velocity.v[0] = (float)((0.0 - Math.Sin(num2)) * (double)num11);
							bullet[currentBullet].phys1.velocity.v[1] = (float)(Math.Cos(num2) * (double)num11);
							bullet[currentBullet].phys1.velocity.v[2] = (float)(0.0 - Math.Cos(num3));
							break;
						default:
							bullet[currentBullet].phys1.velocity.v[0] = global::Players.Players.players[b].charP.velocity.v[0] * Math.Abs(global::Players.Players.players[b].mv[uBufferID].M21) + (float)((0.0 - Math.Sin(num2)) * (double)global::MainGame.MainGame.playerVehicles[0].weapons[curStub].muzzleVelocity * (double)num11);
							bullet[currentBullet].phys1.velocity.v[1] = global::Players.Players.players[b].charP.velocity.v[1] * Math.Abs(global::Players.Players.players[b].mv[uBufferID].M22) + (float)(Math.Cos(num2) * (double)global::MainGame.MainGame.playerVehicles[0].weapons[curStub].muzzleVelocity * (double)num11);
							bullet[currentBullet].phys1.velocity.v[2] = global::Players.Players.players[b].charP.velocity.v[2] * Math.Abs(global::Players.Players.players[b].mv[uBufferID].M23) + (float)((0.0 - Math.Cos(num3)) * (double)global::MainGame.MainGame.playerVehicles[0].weapons[curStub].muzzleVelocity);
							break;
						}
						if (global::Players.Players.players[b].usingTracers && ++global::MainGame.MainGame.playerVehicles[0].weapons[curStub].tracerCnt > global::MainGame.MainGame.playerVehicles[0].weapons[curStub].roundsPerTracer)
						{
							bullet[currentBullet].tracer = 1;
							bullet[currentBullet].phys1.velocity.v[0] *= 0.05f;
							bullet[currentBullet].phys1.velocity.v[1] *= 0.05f;
							bullet[currentBullet].phys1.velocity.v[2] *= 0.05f;
							global::MainGame.MainGame.playerVehicles[0].weapons[curStub].tracerCnt = 0;
						}
						bullet[currentBullet].phys1.velocity.v[0] *= global::MainGame.MainGame.playerVehicles[b].weapons[curStub].muzzleVelocityAdjustment;
						bullet[currentBullet].phys1.velocity.v[1] *= global::MainGame.MainGame.playerVehicles[b].weapons[curStub].muzzleVelocityAdjustment;
						bullet[currentBullet].phys1.velocity.v[2] *= global::MainGame.MainGame.playerVehicles[b].weapons[curStub].muzzleVelocityAdjustment;
						currentBullet++;
						if (currentBullet >= 100)
						{
							currentBullet = 0;
						}
					}
					if (flag2 && !wp1[weaponID].unLimitedAmmo)
					{
						b4 += b6;
						global::MainGame.MainGame.playerVehicles[0].weapons[curStub].currentRounds--;
					}
					if (global::MainGame.MainGame.playerVehicles[0].weapons[curStub].currentRounds < 1)
					{
						global::Players.Players.needToReload = true;
						break;
					}
				}
				global::MainGame.MainGame.gameData.players[0].shotsFired += b4;
			}
		}
		else
		{
			if (!global::MainGame.MainGame.playerVehicles[0].weapons[curStub].roundChambered)
			{
				global::MainGame.MainGame.playerVehicles[b].weapons[curStub].needToChamber = true;
				global::Players.Players.needToChamber = true;
				global::MainGame.MainGame.playerVehicles[0].weapons[curStub].roundChambered = false;
				return false;
			}
			float num8 = (float)global::MainGame.MainGame.mainRandom.NextDouble() * ((float)Math.PI * 2f);
			float num12 = (float)Math.PI * 2f / (float)global::MainGame.MainGame.playerVehicles[0].weapons[curStub].shotCount;
			b4 = 0;
			for (int i = 0; i < wp1[weaponID].numBarrels; i++)
			{
				bool flag2 = false;
				byte b6 = 0;
				for (int j = 0; j < global::MainGame.MainGame.playerVehicles[0].weapons[curStub].shotCount; j++)
				{
					if ((bulletActive[uBufferID, currentBullet] & 3) > 0)
					{
						j = currentBullet++;
						while (currentBullet < 100 && (bulletActive[uBufferID, currentBullet] & 3) > 0)
						{
							currentBullet++;
						}
						if (currentBullet >= 100)
						{
							currentBullet = 0;
							while (currentBullet < j && (bulletActive[uBufferID, currentBullet] & 3) > 0)
							{
								currentBullet++;
							}
						}
						if ((bulletActive[uBufferID, currentBullet] & 3) > 0)
						{
							i = wp1[weaponID].numBarrels;
							break;
						}
					}
					flag2 = true;
					flag = true;
					num = currentBullet;
					b6++;
					bullet[currentBullet].ammoType = b2;
					bullet[currentBullet].ammoIndex = b3;
					bullet[currentBullet].weaponID = weaponID;
					bullet[currentBullet].barrelID = (byte)i;
					bullet[currentBullet].playerID = 0;
					bullet[currentBullet].tracer = 0;
					bullet[currentBullet].phys1.acceleration.v[2] = global::MainGame.MainGame.playerVehicles[0].weapons[curStub].ammoAccelerationZ;
					bullet[currentBullet].rotation = 0f;
					bulletActive[uBufferID, currentBullet] = 1;
					bullet[currentBullet].phys1.position.v[0] = global::MainGame.MainGame.playerVehicles[0].weapons[curStub].offset[i, 2].x + global::MainGame.MainGame.playerVehicles[0].weapons[curStub].posX;
					bullet[currentBullet].phys1.position.v[1] = global::MainGame.MainGame.playerVehicles[0].weapons[curStub].offset[i, 2].y + global::MainGame.MainGame.playerVehicles[0].weapons[curStub].posY;
					bullet[currentBullet].phys1.position.v[2] = global::MainGame.MainGame.playerVehicles[0].weapons[curStub].offset[i, 2].z + global::MainGame.MainGame.playerVehicles[0].weapons[curStub].posZ;
					bullet[currentBullet].startX[uBufferID] = bullet[currentBullet].phys1.position.v[0];
					bullet[currentBullet].startY[uBufferID] = bullet[currentBullet].phys1.position.v[1];
					bullet[currentBullet].startZ[uBufferID] = bullet[currentBullet].phys1.position.v[2];
					float num7 = (float)global::MainGame.MainGame.mainRandom.NextDouble() * global::MainGame.MainGame.playerVehicles[b].weapons[curStub].shootingAccuracy * global::Players.Players.players[b].shootingAccuracy * ((float)Math.PI / 180f);
					float num9 = (float)Math.Cos(num8) * num7;
					float num10 = (float)Math.Sin(num8) * num7;
					num8 += num12;
					num2 = global::MainGame.MainGame.playerVehicles[0].weapons[curStub].offset[i, 6].x + num9;
					num3 = global::MainGame.MainGame.playerVehicles[0].weapons[curStub].offset[i, 6].y + num10;
					bullet[currentBullet].phys1.angle.v[0] = num3 * 57.29578f - 90f;
					bullet[currentBullet].phys1.angle.v[2] = num2 * 57.29578f;
					recoilUp = global::MainGame.MainGame.playerVehicles[0].weapons[curStub].recoilUp[0] + (float)global::MainGame.MainGame.mainRandom.NextDouble() * global::MainGame.MainGame.playerVehicles[0].weapons[curStub].recoilUp[1];
					recoilSide = global::MainGame.MainGame.playerVehicles[0].weapons[curStub].recoilSide[0] - global::MainGame.MainGame.playerVehicles[0].weapons[curStub].recoilSide[1] + (float)global::MainGame.MainGame.mainRandom.NextDouble() * global::MainGame.MainGame.playerVehicles[0].weapons[curStub].recoilSide[2];
					recoilBack = global::MainGame.MainGame.playerVehicles[0].weapons[curStub].recoilBack[0] + (float)global::MainGame.MainGame.mainRandom.NextDouble() * global::MainGame.MainGame.playerVehicles[0].weapons[curStub].recoilBack[1];
					bullet[currentBullet].phys1.fx = global::MainGame.MainGame.playerVehicles[0].weapons[curStub].offset[i, 5].x;
					bullet[currentBullet].phys1.fy = global::MainGame.MainGame.playerVehicles[0].weapons[curStub].offset[i, 5].y;
					bullet[currentBullet].phys1.fz = global::MainGame.MainGame.playerVehicles[0].weapons[curStub].offset[i, 5].z;
					float num11 = (float)Math.Sin(num3);
					switch (b2)
					{
					case 2:
						bullet[currentBullet].timer = ammo[b3].timer;
						bullet[currentBullet].phys1.velocity.v[0] = global::Players.Players.players[b].charP.velocity.v[0] + (float)((0.0 - Math.Sin(num2)) * (double)global::MainGame.MainGame.playerVehicles[0].weapons[curStub].muzzleVelocity * (double)num11);
						bullet[currentBullet].phys1.velocity.v[1] = global::Players.Players.players[b].charP.velocity.v[1] + (float)(Math.Cos(num2) * (double)global::MainGame.MainGame.playerVehicles[0].weapons[curStub].muzzleVelocity * (double)num11);
						bullet[currentBullet].phys1.velocity.v[2] = global::Players.Players.players[b].charP.velocity.v[2] + (float)((0.0 - Math.Cos(num3)) * (double)global::MainGame.MainGame.playerVehicles[0].weapons[curStub].muzzleVelocity);
						break;
					case 3:
						bullet[currentBullet].phys1.velocity.v[0] = global::Players.Players.players[b].charP.velocity.v[0] + (float)((0.0 - Math.Sin(num2)) * (double)global::MainGame.MainGame.playerVehicles[0].weapons[curStub].muzzleVelocity * (double)num11);
						bullet[currentBullet].phys1.velocity.v[1] = global::Players.Players.players[b].charP.velocity.v[1] + (float)(Math.Cos(num2) * (double)global::MainGame.MainGame.playerVehicles[0].weapons[curStub].muzzleVelocity * (double)num11);
						bullet[currentBullet].phys1.velocity.v[2] = global::Players.Players.players[b].charP.velocity.v[2] + (float)((0.0 - Math.Cos(num3)) * (double)global::MainGame.MainGame.playerVehicles[0].weapons[curStub].muzzleVelocity);
						bullet[currentBullet].timer = ammo[b3].timer;
						bullet[currentBullet].particleTimer = ammo[b3].particleTimer;
						if (global::Networking.Networking.inGame)
						{
							ref HalfSingle reference61 = ref global::Networking.Networking.networkHS[0];
							reference61 = new HalfSingle(bullet[currentBullet].phys1.position.v[0]);
							ref HalfSingle reference62 = ref global::Networking.Networking.networkHS[1];
							reference62 = new HalfSingle(bullet[currentBullet].phys1.position.v[1]);
							ref HalfSingle reference63 = ref global::Networking.Networking.networkHS[2];
							reference63 = new HalfSingle(bullet[currentBullet].phys1.position.v[2]);
							ref HalfSingle reference64 = ref global::Networking.Networking.networkHS[3];
							reference64 = new HalfSingle(bullet[currentBullet].phys1.angle.v[0]);
							ref HalfSingle reference65 = ref global::Networking.Networking.networkHS[4];
							reference65 = new HalfSingle(bullet[currentBullet].phys1.angle.v[2]);
							ref HalfSingle reference66 = ref global::Networking.Networking.networkHS[5];
							reference66 = new HalfSingle(num2);
							ref HalfSingle reference67 = ref global::Networking.Networking.networkHS[6];
							reference67 = new HalfSingle(num3);
							global::Networking.Networking.networkBytes[0] = curMount;
							global::Networking.Networking.networkSBytes[0] = (sbyte)weaponID;
							global::Networking.Networking.networkSBytes[1] = bullet[currentBullet].tracer;
							global::Networking.Networking.networkSBytes[2] = (sbyte)b3;
							global::Networking.Networking.networkDoubles[0] = 0.0;
							mainC.networkingMain.XBOX_Send_Network_Message1(1);
						}
						break;
					case 4:
						bullet[currentBullet].timer = ammo[b3].timer;
						bullet[currentBullet].phys1.angularVelocity.v[0] = 360f;
						if (global::Networking.Networking.inGame)
						{
							ref HalfSingle reference47 = ref global::Networking.Networking.networkHS[0];
							reference47 = new HalfSingle(bullet[currentBullet].phys1.position.v[0]);
							ref HalfSingle reference48 = ref global::Networking.Networking.networkHS[1];
							reference48 = new HalfSingle(bullet[currentBullet].phys1.position.v[1]);
							ref HalfSingle reference49 = ref global::Networking.Networking.networkHS[2];
							reference49 = new HalfSingle(bullet[currentBullet].phys1.position.v[2]);
							ref HalfSingle reference50 = ref global::Networking.Networking.networkHS[3];
							reference50 = new HalfSingle(bullet[currentBullet].phys1.angle.v[0]);
							ref HalfSingle reference51 = ref global::Networking.Networking.networkHS[4];
							reference51 = new HalfSingle(bullet[currentBullet].phys1.angle.v[2]);
							ref HalfSingle reference52 = ref global::Networking.Networking.networkHS[5];
							reference52 = new HalfSingle(num2);
							ref HalfSingle reference53 = ref global::Networking.Networking.networkHS[6];
							reference53 = new HalfSingle(num3);
							global::Networking.Networking.networkBytes[0] = curMount;
							global::Networking.Networking.networkSBytes[0] = (sbyte)weaponID;
							global::Networking.Networking.networkSBytes[1] = bullet[currentBullet].tracer;
							global::Networking.Networking.networkSBytes[2] = (sbyte)b3;
							global::Networking.Networking.networkDoubles[0] = 0.0;
							mainC.networkingMain.XBOX_Send_Network_Message1(1);
						}
						break;
					case 5:
						if (global::Networking.Networking.inGame)
						{
							ref HalfSingle reference54 = ref global::Networking.Networking.networkHS[0];
							reference54 = new HalfSingle(bullet[currentBullet].phys1.position.v[0]);
							ref HalfSingle reference55 = ref global::Networking.Networking.networkHS[1];
							reference55 = new HalfSingle(bullet[currentBullet].phys1.position.v[1]);
							ref HalfSingle reference56 = ref global::Networking.Networking.networkHS[2];
							reference56 = new HalfSingle(bullet[currentBullet].phys1.position.v[2]);
							ref HalfSingle reference57 = ref global::Networking.Networking.networkHS[3];
							reference57 = new HalfSingle(bullet[currentBullet].phys1.angle.v[0]);
							ref HalfSingle reference58 = ref global::Networking.Networking.networkHS[4];
							reference58 = new HalfSingle(bullet[currentBullet].phys1.angle.v[2]);
							ref HalfSingle reference59 = ref global::Networking.Networking.networkHS[5];
							reference59 = new HalfSingle(num2);
							ref HalfSingle reference60 = ref global::Networking.Networking.networkHS[6];
							reference60 = new HalfSingle(num3);
							global::Networking.Networking.networkBytes[0] = curMount;
							global::Networking.Networking.networkSBytes[0] = (sbyte)weaponID;
							global::Networking.Networking.networkSBytes[1] = bullet[currentBullet].tracer;
							global::Networking.Networking.networkSBytes[2] = (sbyte)b3;
							global::Networking.Networking.networkDoubles[0] = -1.0;
							mainC.networkingMain.XBOX_Send_Network_Message1(1);
						}
						bullet[currentBullet].phys1.velocity.v[0] = global::Players.Players.players[b].charP.velocity.v[0] + (float)((0.0 - Math.Sin(num2)) * (double)global::MainGame.MainGame.playerVehicles[0].weapons[curStub].muzzleVelocity * (double)num11);
						bullet[currentBullet].phys1.velocity.v[1] = global::Players.Players.players[b].charP.velocity.v[1] + (float)(Math.Cos(num2) * (double)global::MainGame.MainGame.playerVehicles[0].weapons[curStub].muzzleVelocity * (double)num11);
						bullet[currentBullet].phys1.velocity.v[2] = global::Players.Players.players[b].charP.velocity.v[2] + (float)((0.0 - Math.Cos(num3)) * (double)global::MainGame.MainGame.playerVehicles[0].weapons[curStub].muzzleVelocity);
						bullet[currentBullet].timer = ammo[b3].releaseTimer;
						break;
					case 6:
					case 12:
					{
						bullet[currentBullet].phys1.velocity.v[0] = global::Players.Players.players[b].charP.velocity.v[0] + (float)((0.0 - Math.Sin(num2)) * (double)global::MainGame.MainGame.playerVehicles[0].weapons[curStub].muzzleVelocity * (double)num11);
						bullet[currentBullet].phys1.velocity.v[1] = global::Players.Players.players[b].charP.velocity.v[1] + (float)(Math.Cos(num2) * (double)global::MainGame.MainGame.playerVehicles[0].weapons[curStub].muzzleVelocity * (double)num11);
						bullet[currentBullet].phys1.velocity.v[2] = global::Players.Players.players[b].charP.velocity.v[2] + (float)((0.0 - Math.Cos(num3)) * (double)global::MainGame.MainGame.playerVehicles[0].weapons[curStub].muzzleVelocity);
						ref Matrix reference39 = ref bullet[currentBullet].mv[uBufferID];
						reference39 = Vehicles.vehicles[global::Players.Players.players[b].curVehicle].mounts[curMount].mvCurrent[uBufferID] * global::MainGame.MainGame.playerVehicles[b].mv[uBufferID];
						bullet[currentBullet].rot = Quaternion.CreateFromRotationMatrix(bullet[currentBullet].mv[uBufferID]);
						if (global::Networking.Networking.inGame)
						{
							ref HalfSingle reference40 = ref global::Networking.Networking.networkHS[0];
							reference40 = new HalfSingle(bullet[currentBullet].phys1.position.v[0]);
							ref HalfSingle reference41 = ref global::Networking.Networking.networkHS[1];
							reference41 = new HalfSingle(bullet[currentBullet].phys1.position.v[1]);
							ref HalfSingle reference42 = ref global::Networking.Networking.networkHS[2];
							reference42 = new HalfSingle(bullet[currentBullet].phys1.position.v[2]);
							ref HalfSingle reference43 = ref global::Networking.Networking.networkHS[3];
							reference43 = new HalfSingle(bullet[currentBullet].phys1.angle.v[0]);
							ref HalfSingle reference44 = ref global::Networking.Networking.networkHS[4];
							reference44 = new HalfSingle(bullet[currentBullet].phys1.angle.v[2]);
							ref HalfSingle reference45 = ref global::Networking.Networking.networkHS[5];
							reference45 = new HalfSingle(num2);
							ref HalfSingle reference46 = ref global::Networking.Networking.networkHS[6];
							reference46 = new HalfSingle(num3);
							global::Networking.Networking.networkBytes[0] = curMount;
							global::Networking.Networking.networkSBytes[0] = (sbyte)weaponID;
							global::Networking.Networking.networkSBytes[1] = bullet[currentBullet].tracer;
							global::Networking.Networking.networkSBytes[2] = (sbyte)b3;
							global::Networking.Networking.networkDoubles[0] = 0.0;
							mainC.networkingMain.XBOX_Send_Network_Message1(1);
						}
						break;
					}
					case 7:
					{
						ref Matrix reference38 = ref bullet[currentBullet].mv[uBufferID];
						reference38 = Vehicles.vehicles[global::Players.Players.players[b].curVehicle].mounts[curMount].mvCurrent[uBufferID] * global::MainGame.MainGame.playerVehicles[b].mv[uBufferID];
						bullet[currentBullet].mv[uBufferID].M41 = 0f;
						bullet[currentBullet].mv[uBufferID].M42 = 0f;
						bullet[currentBullet].mv[uBufferID].M43 = 0f;
						break;
					}
					case 8:
						bullet[currentBullet].timer = 0f;
						break;
					case 9:
						bullet[currentBullet].phys1.velocity.v[0] = global::Players.Players.players[b].mv[uBufferID].M21 * global::MainGame.MainGame.playerVehicles[b].weapons[curStub].muzzleVelocity;
						bullet[currentBullet].phys1.velocity.v[1] = global::Players.Players.players[b].mv[uBufferID].M22 * global::MainGame.MainGame.playerVehicles[b].weapons[curStub].muzzleVelocity;
						bullet[currentBullet].phys1.velocity.v[2] = global::Players.Players.players[b].mv[uBufferID].M23 * global::MainGame.MainGame.playerVehicles[b].weapons[curStub].muzzleVelocity;
						break;
					case 11:
						bullet[currentBullet].timer = global::MainGame.MainGame.playerVehicles[0].weapons[curStub].ammoTimer;
						bullet[currentBullet].timer += bullet[currentBullet].timer * 0.2f * (-1f + (float)global::MainGame.MainGame.mainRandom.NextDouble() * 2f);
						bullet[currentBullet].phys1.velocity.v[0] = global::Players.Players.players[b].charP.velocity.v[0] * Math.Abs(global::Players.Players.players[b].mv[uBufferID].M21) + (float)((0.0 - Math.Sin(num2)) * (double)global::MainGame.MainGame.playerVehicles[0].weapons[curStub].muzzleVelocity * (double)num11);
						bullet[currentBullet].phys1.velocity.v[1] = global::Players.Players.players[b].charP.velocity.v[1] * Math.Abs(global::Players.Players.players[b].mv[uBufferID].M22) + (float)(Math.Cos(num2) * (double)global::MainGame.MainGame.playerVehicles[0].weapons[curStub].muzzleVelocity * (double)num11);
						bullet[currentBullet].phys1.velocity.v[2] = global::Players.Players.players[b].charP.velocity.v[2] * Math.Abs(global::Players.Players.players[b].mv[uBufferID].M23) + (float)((0.0 - Math.Cos(num3)) * (double)global::MainGame.MainGame.playerVehicles[0].weapons[curStub].muzzleVelocity);
						break;
					case 15:
						bullet[currentBullet].phys1.velocity.v[0] = (float)((0.0 - Math.Sin(num2)) * (double)num11);
						bullet[currentBullet].phys1.velocity.v[1] = (float)(Math.Cos(num2) * (double)num11);
						bullet[currentBullet].phys1.velocity.v[2] = (float)(0.0 - Math.Cos(num3));
						break;
					default:
						bullet[currentBullet].phys1.velocity.v[0] = global::Players.Players.players[b].charP.velocity.v[0] * Math.Abs(global::Players.Players.players[b].mv[uBufferID].M21) + (float)((0.0 - Math.Sin(num2)) * (double)global::MainGame.MainGame.playerVehicles[0].weapons[curStub].muzzleVelocity * (double)num11);
						bullet[currentBullet].phys1.velocity.v[1] = global::Players.Players.players[b].charP.velocity.v[1] * Math.Abs(global::Players.Players.players[b].mv[uBufferID].M22) + (float)(Math.Cos(num2) * (double)global::MainGame.MainGame.playerVehicles[0].weapons[curStub].muzzleVelocity * (double)num11);
						bullet[currentBullet].phys1.velocity.v[2] = global::Players.Players.players[b].charP.velocity.v[2] * Math.Abs(global::Players.Players.players[b].mv[uBufferID].M23) + (float)((0.0 - Math.Cos(num3)) * (double)global::MainGame.MainGame.playerVehicles[0].weapons[curStub].muzzleVelocity);
						break;
					}
					if (global::Players.Players.players[b].usingTracers && ++global::MainGame.MainGame.playerVehicles[0].weapons[curStub].tracerCnt > global::MainGame.MainGame.playerVehicles[0].weapons[curStub].roundsPerTracer)
					{
						bullet[currentBullet].tracer = 1;
						bullet[currentBullet].phys1.velocity.v[0] *= 0.05f;
						bullet[currentBullet].phys1.velocity.v[1] *= 0.05f;
						bullet[currentBullet].phys1.velocity.v[2] *= 0.05f;
						global::MainGame.MainGame.playerVehicles[0].weapons[curStub].tracerCnt = 0;
					}
					bullet[currentBullet].phys1.velocity.v[0] *= global::MainGame.MainGame.playerVehicles[b].weapons[curStub].muzzleVelocityAdjustment;
					bullet[currentBullet].phys1.velocity.v[1] *= global::MainGame.MainGame.playerVehicles[b].weapons[curStub].muzzleVelocityAdjustment;
					bullet[currentBullet].phys1.velocity.v[2] *= global::MainGame.MainGame.playerVehicles[b].weapons[curStub].muzzleVelocityAdjustment;
					currentBullet++;
					if (currentBullet >= 100)
					{
						currentBullet = 0;
					}
				}
				if (flag2 && !wp1[weaponID].unLimitedAmmo)
				{
					b4 += b6;
					global::MainGame.MainGame.playerVehicles[0].weapons[curStub].currentRounds--;
				}
				if (global::MainGame.MainGame.playerVehicles[0].weapons[curStub].currentRounds < 1)
				{
					global::Players.Players.needToReload = true;
					break;
				}
			}
			global::MainGame.MainGame.gameData.players[0].shotsFired += b4;
		}
		if (flag)
		{
			if (wp1[weaponID].ChamberAfterShot)
			{
				global::MainGame.MainGame.playerVehicles[b].weapons[curStub].needToChamber = true;
				global::Players.Players.needToChamber = true;
				global::MainGame.MainGame.playerVehicles[0].weapons[curStub].roundChambered = false;
			}
			int j = global::MainGame.MainGame.playerVehicles[0].weapons[curStub].AnimationFire;
			mainC.programsMain.Start_Animation(b, ref global::Players.Players.players[b].jt1, ref global::Players.Players.players[b].animations, global::Players.Players.players[b].programCollection, j, 1f, 1f);
			if (!wp1[weaponID].ChamberAfterShot)
			{
				mainC.avatarMain.Avatar_Movement_By_ID(b, queue: true, 1, global::MainGame.MainGame.playerVehicles[b].weapons[curStub].fullyAutomatic, mainC.gameLogic.Game_Map_Program_To_Avatar_Animation((ushort)j), cancelOtherGroupAnimations: false);
			}
			if (global::Networking.Networking.networkSessionReady)
			{
				mpSendWeaponFiredMsg = true;
				mpNumFiredMsgsToSend = (byte)global::Networking.Networking.networkSession.RemoteGamers.Count;
			}
			bullet[num].lightID = -1;
			if (wp1[weaponID].snd_fire != null)
			{
				mainC.soundsMain.Play_Priority_Sound(wp1[weaponID].snd_fire, bullet[num].phys1.position.v[0], bullet[num].phys1.position.v[1], bullet[num].phys1.position.v[2], global::Players.Players.players[b].charP.velocity.v[0], global::Players.Players.players[b].charP.velocity.v[1], global::Players.Players.players[b].charP.velocity.v[2]);
			}
			if (ammo[b3].sound != null)
			{
				bullet[num].soundID = mainC.soundsMain.Play_Priority_Sound(ammo[b3].sound, bullet[num].phys1.position.v[0], bullet[num].phys1.position.v[1], bullet[num].phys1.position.v[2], global::Players.Players.players[b].charP.velocity.v[0] + bullet[num].phys1.velocity.v[0], global::Players.Players.players[b].charP.velocity.v[1] + bullet[num].phys1.velocity.v[1], global::Players.Players.players[b].charP.velocity.v[2] + bullet[num].phys1.velocity.v[2]);
			}
			bullet[num].soundID2 = -1;
			if (ammo[b3].sound2 != null)
			{
				bullet[num].soundID2 = b3;
			}
			switch (b2)
			{
			case 0:
			case 1:
				global::MainGame.MainGame.playerVehicles[0].weapons[curStub].muzzleFlashTimer = 0.023f;
				break;
			case 2:
				if (laserLights[curLaserLight] > -1)
				{
					bullet[laserLights[curLaserLight]].lightID = -1;
				}
				bullet[num].lightID = (sbyte)curLaserLight;
				j = bullet[num].ammoIndex;
				ammoLightPos[uBufferID, curLaserLight].X = bullet[num].phys1.position.v[0];
				ammoLightPos[uBufferID, curLaserLight].Y = bullet[num].phys1.position.v[1];
				ammoLightPos[uBufferID, curLaserLight].Z = bullet[num].phys1.position.v[2];
				ammoLightPos[uBufferID, curLaserLight].W = ammo[j].colorIntensity;
				ammoLightColor[uBufferID, curLaserLight].X = ammo[j].lightColor[0];
				ammoLightColor[uBufferID, curLaserLight].Y = ammo[j].lightColor[1];
				ammoLightColor[uBufferID, curLaserLight].Z = ammo[j].lightColor[2];
				ammoLightColor[uBufferID, curLaserLight].W = ammo[j].lightColor[3];
				laserLights[curLaserLight] = (sbyte)num;
				curLaserLight++;
				if (curLaserLight >= numLaserLights)
				{
					curLaserLight = 0;
				}
				global::MainGame.MainGame.playerVehicles[0].weapons[curStub].fireRateAdjustment -= wp1[weaponID].fireRateReduction;
				if (global::MainGame.MainGame.playerVehicles[0].weapons[curStub].fireRateAdjustment < wp1[weaponID].fireRateAdjLowPerc)
				{
					global::MainGame.MainGame.playerVehicles[0].weapons[curStub].fireRateAdjustment = wp1[weaponID].fireRateAdjLowPerc;
				}
				break;
			case 3:
				if (laserLights[curLaserLight] > -1)
				{
					bullet[laserLights[curLaserLight]].lightID = -1;
				}
				bullet[num].lightID = (sbyte)curLaserLight;
				j = bullet[num].ammoIndex;
				ammoLightPos[uBufferID, curLaserLight].X = bullet[num].phys1.position.v[0];
				ammoLightPos[uBufferID, curLaserLight].Y = bullet[num].phys1.position.v[1];
				ammoLightPos[uBufferID, curLaserLight].Z = bullet[num].phys1.position.v[2];
				ammoLightPos[uBufferID, curLaserLight].W = ammo[j].colorIntensity;
				ammoLightColor[uBufferID, curLaserLight].X = ammo[j].lightColor[0];
				ammoLightColor[uBufferID, curLaserLight].Y = ammo[j].lightColor[1];
				ammoLightColor[uBufferID, curLaserLight].Z = ammo[j].lightColor[2];
				ammoLightColor[uBufferID, curLaserLight].W = ammo[j].lightColor[3];
				laserLights[curLaserLight] = (sbyte)num;
				curLaserLight++;
				if (curLaserLight >= numLaserLights)
				{
					curLaserLight = 0;
				}
				break;
			case 4:
				global::MainGame.MainGame.showCrossHairs[0] = 1;
				j = wp1[weaponID].AnimationThrow;
				global::Players.Players.players[0].animations[j].callBack = (byte)global::Players.Players.players[0].wpnIndex;
				mainC.programsMain.Start_Animation(b, ref global::Players.Players.players[b].jt1, ref global::Players.Players.players[b].animations, global::Players.Players.players[b].programCollection, j, 1f, 1f);
				global::Players.Players.players[0].playerIsMoving = 256;
				break;
			case 5:
				global::MainGame.MainGame.playerVehicles[0].weapons[curStub].fired = false;
				break;
			case 6:
			case 12:
				if (global::MainGame.MainGame.bombViewEnabled && !global::MainGame.MainGame.viewFollowingObject)
				{
					global::MainGame.MainGame.overheadView = false;
					global::Players.Players.controlsInUse = true;
					projectileViewTimer = false;
					global::MainGame.MainGame.viewFollowingObject = true;
					global::MainGame.MainGame.viewFollowingObjectID = (ushort)num;
					global::MainGame.MainGame.showCrossHairs[3] = 1;
					mainC.inputMain.UI_HUD_Show_Exit_View_Message(showMessage: true);
					if (b2 == 12)
					{
						mainC.inputMain.UI_HUD_Show_Guided_Bomb_Message(showMessage: true);
					}
				}
				mainC.gameLogic.Game_Misc(7);
				break;
			case 8:
				if (laserLights[curLaserLight] > -1)
				{
					bullet[laserLights[curLaserLight]].lightID = -1;
				}
				bullet[num].lightID = (sbyte)curLaserLight;
				j = bullet[num].ammoIndex;
				ammoLightPos[uBufferID, curLaserLight].X = bullet[num].phys1.position.v[0];
				ammoLightPos[uBufferID, curLaserLight].Y = bullet[num].phys1.position.v[1];
				ammoLightPos[uBufferID, curLaserLight].Z = bullet[num].phys1.position.v[2];
				ammoLightPos[uBufferID, curLaserLight].W = ammo[j].colorIntensity;
				ammoLightColor[uBufferID, curLaserLight].X = ammo[j].lightColor[0];
				ammoLightColor[uBufferID, curLaserLight].Y = ammo[j].lightColor[1];
				ammoLightColor[uBufferID, curLaserLight].Z = ammo[j].lightColor[2];
				ammoLightColor[uBufferID, curLaserLight].W = ammo[j].lightColor[3];
				laserLights[curLaserLight] = (sbyte)num;
				curLaserLight++;
				if (curLaserLight >= numLaserLights)
				{
					curLaserLight = 0;
				}
				global::MainGame.MainGame.playerVehicles[0].weapons[curStub].fireRateAdjustment -= wp1[weaponID].fireRateReduction;
				if (global::MainGame.MainGame.playerVehicles[0].weapons[curStub].fireRateAdjustment < wp1[weaponID].fireRateAdjLowPerc)
				{
					global::MainGame.MainGame.playerVehicles[0].weapons[curStub].fireRateAdjustment = wp1[weaponID].fireRateAdjLowPerc;
				}
				break;
			case 9:
				bullet[num].timer = ammo[b3].timer;
				break;
			case 11:
				global::MainGame.MainGame.playerVehicles[0].weapons[curStub].muzzleFlashTimer = 0.023f;
				global::Rendering.Rendering.npn.v[0] = wp1[weaponID].pfx1;
				global::Rendering.Rendering.npn.v[1] = wp1[weaponID].pfx2;
				global::Rendering.Rendering.npn.v[2] = wp1[weaponID].pfx3;
				break;
			case 15:
				bullet[num].phys1.rx = (int)curStub;
				bullet[num].phys1.ry = (int)curMount;
				bullet[num].timer = ammo[b3].timer;
				break;
			}
			return true;
		}
		return false;
	}

	public void Recalculate_Bullet_Position(ushort bulletID)
	{
		byte b = 0;
		byte b2 = 0;
		float num = 0f;
		float num2 = 0f;
		int num3 = 0;
		b = bullet[bulletID].ammoType;
		b2 = bullet[bulletID].ammoIndex;
		byte uBufferID = global::Rendering.Rendering.uBufferID;
		byte b3 = (byte)bullet[bulletID].phys1.rx;
		byte b4 = (byte)bullet[bulletID].phys1.ry;
		ushort num4 = (ushort)bullet[bulletID].playerID;
		bullet[bulletID].phys1.position.v[0] = global::MainGame.MainGame.playerVehicles[num4].weapons[b3].offset[num3, 2].x + global::MainGame.MainGame.playerVehicles[num4].weapons[b3].posX;
		bullet[bulletID].phys1.position.v[1] = global::MainGame.MainGame.playerVehicles[num4].weapons[b3].offset[num3, 2].y + global::MainGame.MainGame.playerVehicles[num4].weapons[b3].posY;
		bullet[bulletID].phys1.position.v[2] = global::MainGame.MainGame.playerVehicles[num4].weapons[b3].offset[num3, 2].z + global::MainGame.MainGame.playerVehicles[num4].weapons[b3].posZ;
		float num5 = (float)global::MainGame.MainGame.mainRandom.NextDouble() * global::MainGame.MainGame.playerVehicles[num4].weapons[b3].shootingAccuracy * global::Players.Players.players[num4].shootingAccuracy * ((float)Math.PI / 180f);
		float num6 = (float)global::MainGame.MainGame.mainRandom.NextDouble() * ((float)Math.PI * 2f);
		float num7 = (float)Math.Cos(num6) * num5;
		float num8 = (float)Math.Sin(num6) * num5;
		num = global::MainGame.MainGame.playerVehicles[0].weapons[b3].offset[num3, 6].x + num7;
		num2 = global::MainGame.MainGame.playerVehicles[0].weapons[b3].offset[num3, 6].y + num8;
		bullet[bulletID].phys1.angle.v[0] = num2 * 57.29578f - 90f;
		bullet[bulletID].phys1.angle.v[2] = num * 57.29578f;
		bullet[bulletID].phys1.fx = global::MainGame.MainGame.playerVehicles[0].weapons[b3].offset[num3, 5].x;
		bullet[bulletID].phys1.fy = global::MainGame.MainGame.playerVehicles[0].weapons[b3].offset[num3, 5].y;
		bullet[bulletID].phys1.fz = global::MainGame.MainGame.playerVehicles[0].weapons[b3].offset[num3, 5].z;
		float num9 = (float)Math.Sin(num2);
		switch (b)
		{
		case 2:
			bullet[bulletID].timer = ammo[b2].timer;
			bullet[bulletID].phys1.velocity.v[0] = global::Players.Players.players[num4].charP.velocity.v[0] + (float)((0.0 - Math.Sin(num)) * (double)global::MainGame.MainGame.playerVehicles[0].weapons[b3].muzzleVelocity * (double)num9);
			bullet[bulletID].phys1.velocity.v[1] = global::Players.Players.players[num4].charP.velocity.v[1] + (float)(Math.Cos(num) * (double)global::MainGame.MainGame.playerVehicles[0].weapons[b3].muzzleVelocity * (double)num9);
			bullet[bulletID].phys1.velocity.v[2] = global::Players.Players.players[num4].charP.velocity.v[2] + (float)((0.0 - Math.Cos(num2)) * (double)global::MainGame.MainGame.playerVehicles[0].weapons[b3].muzzleVelocity);
			break;
		case 7:
		{
			ref Matrix reference = ref bullet[bulletID].mv[uBufferID];
			reference = Vehicles.vehicles[global::Players.Players.players[num4].curVehicle].mounts[b4].mvCurrent[uBufferID] * global::MainGame.MainGame.playerVehicles[num4].mv[uBufferID];
			bullet[bulletID].mv[uBufferID].M41 = 0f;
			bullet[bulletID].mv[uBufferID].M42 = 0f;
			bullet[bulletID].mv[uBufferID].M43 = 0f;
			break;
		}
		case 8:
			bullet[bulletID].timer = 0f;
			break;
		case 9:
			bullet[bulletID].phys1.velocity.v[0] = global::Players.Players.players[num4].mv[uBufferID].M21 * global::MainGame.MainGame.playerVehicles[num4].weapons[b3].muzzleVelocity;
			bullet[bulletID].phys1.velocity.v[1] = global::Players.Players.players[num4].mv[uBufferID].M22 * global::MainGame.MainGame.playerVehicles[num4].weapons[b3].muzzleVelocity;
			bullet[bulletID].phys1.velocity.v[2] = global::Players.Players.players[num4].mv[uBufferID].M23 * global::MainGame.MainGame.playerVehicles[num4].weapons[b3].muzzleVelocity;
			break;
		case 11:
			bullet[bulletID].timer = global::MainGame.MainGame.playerVehicles[0].weapons[b3].ammoTimer;
			bullet[bulletID].timer += bullet[bulletID].timer * 0.2f * (-1f + (float)global::MainGame.MainGame.mainRandom.NextDouble() * 2f);
			bullet[bulletID].phys1.velocity.v[0] = global::Players.Players.players[num4].charP.velocity.v[0] * Math.Abs(global::Players.Players.players[num4].mv[uBufferID].M21) + (float)((0.0 - Math.Sin(num)) * (double)global::MainGame.MainGame.playerVehicles[0].weapons[b3].muzzleVelocity * (double)num9);
			bullet[bulletID].phys1.velocity.v[1] = global::Players.Players.players[num4].charP.velocity.v[1] * Math.Abs(global::Players.Players.players[num4].mv[uBufferID].M22) + (float)(Math.Cos(num) * (double)global::MainGame.MainGame.playerVehicles[0].weapons[b3].muzzleVelocity * (double)num9);
			bullet[bulletID].phys1.velocity.v[2] = global::Players.Players.players[num4].charP.velocity.v[2] * Math.Abs(global::Players.Players.players[num4].mv[uBufferID].M23) + (float)((0.0 - Math.Cos(num2)) * (double)global::MainGame.MainGame.playerVehicles[0].weapons[b3].muzzleVelocity);
			break;
		case 15:
			bullet[bulletID].phys1.velocity.v[0] = (float)((0.0 - Math.Sin(num)) * (double)num9);
			bullet[bulletID].phys1.velocity.v[1] = (float)(Math.Cos(num) * (double)num9);
			bullet[bulletID].phys1.velocity.v[2] = (float)(0.0 - Math.Cos(num2));
			break;
		default:
			bullet[bulletID].phys1.velocity.v[0] = global::Players.Players.players[num4].charP.velocity.v[0] * Math.Abs(global::Players.Players.players[num4].mv[uBufferID].M21) + (float)((0.0 - Math.Sin(num)) * (double)global::MainGame.MainGame.playerVehicles[0].weapons[b3].muzzleVelocity * (double)num9);
			bullet[bulletID].phys1.velocity.v[1] = global::Players.Players.players[num4].charP.velocity.v[1] * Math.Abs(global::Players.Players.players[num4].mv[uBufferID].M22) + (float)(Math.Cos(num) * (double)global::MainGame.MainGame.playerVehicles[0].weapons[b3].muzzleVelocity * (double)num9);
			bullet[bulletID].phys1.velocity.v[2] = global::Players.Players.players[num4].charP.velocity.v[2] * Math.Abs(global::Players.Players.players[num4].mv[uBufferID].M23) + (float)((0.0 - Math.Cos(num2)) * (double)global::MainGame.MainGame.playerVehicles[0].weapons[b3].muzzleVelocity);
			break;
		}
		if (global::Players.Players.players[num4].usingTracers && ++global::MainGame.MainGame.playerVehicles[0].weapons[b3].tracerCnt > global::MainGame.MainGame.playerVehicles[0].weapons[b3].roundsPerTracer)
		{
			bullet[bulletID].tracer = 1;
			bullet[bulletID].phys1.velocity.v[0] *= 0.05f;
			bullet[bulletID].phys1.velocity.v[1] *= 0.05f;
			bullet[bulletID].phys1.velocity.v[2] *= 0.05f;
			global::MainGame.MainGame.playerVehicles[0].weapons[b3].tracerCnt = 0;
		}
		bullet[bulletID].phys1.velocity.v[0] *= global::MainGame.MainGame.playerVehicles[num4].weapons[b3].muzzleVelocityAdjustment;
		bullet[bulletID].phys1.velocity.v[1] *= global::MainGame.MainGame.playerVehicles[num4].weapons[b3].muzzleVelocityAdjustment;
		bullet[bulletID].phys1.velocity.v[2] *= global::MainGame.MainGame.playerVehicles[num4].weapons[b3].muzzleVelocityAdjustment;
	}

	public bool Ballistic_Strike_Available()
	{
		for (byte b = 0; b < numBallisticStrikes; b++)
		{
			if (wpnStrike[b].status == 0)
			{
				return true;
			}
		}
		return false;
	}

	public bool Do_Ballistic_Strike(ushort playerID, float posX, float posY, float posZ, float vx, float vy, float vz, float radius, byte ammoIndex, float duration, float timeBetweenFiring, byte variance)
	{
		if (wpnStrike[curBallisticStrike].status != 0)
		{
			ushort num = curBallisticStrike++;
			while (curBallisticStrike < numBallisticStrikes && wpnStrike[curBallisticStrike].status != 0)
			{
				curBallisticStrike++;
			}
			if (curBallisticStrike >= numBallisticStrikes)
			{
				curBallisticStrike = 0;
				while (curBallisticStrike < num && wpnStrike[curBallisticStrike].status != 0)
				{
					curBallisticStrike++;
				}
			}
		}
		if (wpnStrike[curBallisticStrike].status != 0)
		{
			return false;
		}
		wpnStrike[curBallisticStrike].status = 1;
		wpnStrike[curBallisticStrike].ammoIndex = ammoIndex;
		wpnStrike[curBallisticStrike].duration = duration;
		wpnStrike[curBallisticStrike].playerID = playerID;
		wpnStrike[curBallisticStrike].x = posX;
		wpnStrike[curBallisticStrike].y = posY;
		wpnStrike[curBallisticStrike].z = posZ;
		wpnStrike[curBallisticStrike].radius = radius;
		wpnStrike[curBallisticStrike].vx = vx;
		wpnStrike[curBallisticStrike].vy = vy;
		wpnStrike[curBallisticStrike].vz = vz;
		wpnStrike[curBallisticStrike].timeBetweenAmmo = timeBetweenFiring;
		wpnStrike[curBallisticStrike].variance = (byte)(variance + 1);
		wpnStrike[curBallisticStrike].curTime = 0f;
		wpnStrike[curBallisticStrike].curFiringTime = 0f;
		mainC.soundsMain.Play_Priority_Sound("AirRaid", posX, posY, posZ, 0f, 0f, 0f);
		return true;
	}

	public void Launch_Ballistic_Strike_From_Network(int actID)
	{
		short num = mainC.playersMain.Get_Player_Index(actID, -1);
		if (num < 0)
		{
			num = 0;
		}
		else
		{
			mainC.fontmoduleMain.Add_Text_To_Onscreen_Text(global::Players.Players.players[num].abreviateName + " launched an air strike");
		}
		byte b = global::Networking.Networking.networkBytes[0];
		if (b < numBallisticStrikeTypes)
		{
			Do_Ballistic_Strike((ushort)num, global::Networking.Networking.networkHS[0].ToSingle(), global::Networking.Networking.networkHS[1].ToSingle(), wpnStrikeConfig[b].z, wpnStrikeConfig[b].vx, wpnStrikeConfig[b].vy, wpnStrikeConfig[b].vz, wpnStrikeConfig[b].radius, wpnStrikeConfig[b].ammoIndex, wpnStrikeConfig[b].duration, wpnStrikeConfig[b].timeBetweenAmmo, wpnStrikeConfig[b].variance);
		}
	}

	public bool Launch_Ballistic_Strike(ushort playerID, float posX, float posY, byte strikeID)
	{
		return mainC.weaponsMain.Do_Ballistic_Strike(playerID, posX, posY, wpnStrikeConfig[strikeID].z, wpnStrikeConfig[strikeID].vx, wpnStrikeConfig[strikeID].vy, wpnStrikeConfig[strikeID].vz, wpnStrikeConfig[strikeID].radius, wpnStrikeConfig[strikeID].ammoIndex, wpnStrikeConfig[strikeID].duration, wpnStrikeConfig[strikeID].timeBetweenAmmo, wpnStrikeConfig[strikeID].variance);
	}

	public void Send_Ballistic_Strike_To_Network(byte strikeID, float posX, float posY)
	{
		global::Networking.Networking.networkBytes[0] = strikeID;
		ref HalfSingle reference = ref global::Networking.Networking.networkHS[0];
		reference = new HalfSingle(posX);
		ref HalfSingle reference2 = ref global::Networking.Networking.networkHS[1];
		reference2 = new HalfSingle(posY);
		mainC.networkingMain.XBOX_Send_Network_Message58(58);
	}

	public void Process_Weapon_strikes()
	{
		byte b = 0;
		float num = 0f;
		float num2 = 0f;
		byte uBufferID = global::Rendering.Rendering.uBufferID;
		for (ushort num3 = 0; num3 < numBallisticStrikes; num3++)
		{
			if (wpnStrike[num3].status == 1)
			{
				wpnStrike[num3].curTime += global::MainGame.MainGame.frametime / global::Physics.Physics.timeMod;
				if (wpnStrike[num3].curTime < wpnStrike[num3].duration)
				{
					wpnStrike[num3].curFiringTime += global::MainGame.MainGame.frametime / global::Physics.Physics.timeMod;
					while (wpnStrike[num3].curFiringTime > wpnStrike[num3].timeBetweenAmmo)
					{
						wpnStrike[num3].curFiringTime -= wpnStrike[num3].timeBetweenAmmo + wpnStrike[num3].timeBetweenAmmo * ((float)global::MainGame.MainGame.mainRandom.Next(0, wpnStrike[num3].variance) / 100f);
						if ((bulletActive[uBufferID, currentBullet] & 3) > 0)
						{
							int num4 = currentBullet++;
							while (currentBullet < 100 && (bulletActive[uBufferID, currentBullet] & 3) > 0)
							{
								currentBullet++;
							}
							if (currentBullet >= 100)
							{
								currentBullet = 0;
								while (currentBullet < num4 && (bulletActive[uBufferID, currentBullet] & 3) > 0)
								{
									currentBullet++;
								}
							}
						}
						if ((bulletActive[uBufferID, currentBullet] & 3) != 0)
						{
							continue;
						}
						byte ammoIndex = wpnStrike[num3].ammoIndex;
						b = ammo[ammoIndex].type;
						bullet[currentBullet].ammoType = b;
						bullet[currentBullet].ammoIndex = ammoIndex;
						bullet[currentBullet].weaponID = 0;
						bullet[currentBullet].barrelID = 0;
						bullet[currentBullet].phys1.initialTime = 0.0;
						bullet[currentBullet].playerID = (short)wpnStrike[num3].playerID;
						bullet[currentBullet].tracer = 0;
						bullet[currentBullet].phys1.acceleration.v[2] = ammo[ammoIndex].accelerationZ;
						bullet[currentBullet].rotation = 0f;
						bulletActive[uBufferID, currentBullet] = 1;
						bullet[currentBullet].phys1.position.v[0] = wpnStrike[num3].x - wpnStrike[num3].radius + (float)global::MainGame.MainGame.mainRandom.Next(0, (int)(wpnStrike[num3].radius * 2f));
						bullet[currentBullet].phys1.position.v[1] = wpnStrike[num3].y - wpnStrike[num3].radius + (float)global::MainGame.MainGame.mainRandom.Next(0, (int)(wpnStrike[num3].radius * 2f));
						bullet[currentBullet].phys1.position.v[2] = wpnStrike[num3].z;
						bullet[currentBullet].startX[uBufferID] = bullet[currentBullet].phys1.position.v[0];
						bullet[currentBullet].startY[uBufferID] = bullet[currentBullet].phys1.position.v[1];
						bullet[currentBullet].startZ[uBufferID] = bullet[currentBullet].phys1.position.v[2];
						bullet[currentBullet].phys1.angle.v[0] = num2 * 57.29578f - 90f;
						bullet[currentBullet].phys1.angle.v[2] = num * 57.29578f;
						bullet[currentBullet].phys1.fx = wpnStrike[num3].vx;
						bullet[currentBullet].phys1.fy = wpnStrike[num3].vy;
						bullet[currentBullet].phys1.fz = wpnStrike[num3].vz;
						bullet[currentBullet].phys1.velocity.v[0] = wpnStrike[num3].vx;
						bullet[currentBullet].phys1.velocity.v[1] = wpnStrike[num3].vy;
						bullet[currentBullet].phys1.velocity.v[2] = wpnStrike[num3].vz;
						Math.Sin(num2);
						switch (b)
						{
						case 3:
							bullet[currentBullet].timer = ammo[ammoIndex].timer;
							bullet[currentBullet].particleTimer = ammo[ammoIndex].particleTimer;
							if (!global::Networking.Networking.inGame)
							{
							}
							break;
						case 4:
							bullet[currentBullet].timer = ammo[ammoIndex].timer;
							bullet[currentBullet].phys1.angularVelocity.v[0] = 360f;
							if (!global::Networking.Networking.inGame)
							{
							}
							break;
						case 8:
							bullet[currentBullet].timer = 0f;
							break;
						}
						currentBullet++;
						if (currentBullet >= 100)
						{
							currentBullet = 0;
						}
					}
				}
				else
				{
					wpnStrike[num3].status = 0;
				}
			}
		}
	}

	public void Add_Particles_For_Fired_Weapon(short playerID, byte curStub, byte threadID)
	{
		byte b = 0;
		short primaryWeaponMountWeapon = global::Players.Players.players[playerID].primaryWeaponMountWeapon;
		_ = global::Rendering.Rendering.uBufferID;
		_ = global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].weaponID;
		b = global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].ammoIndex;
		_ = ammo[b].type;
		switch (ammo[wp1[primaryWeaponMountWeapon].ammoIndex].type)
		{
		case 0:
		case 1:
		case 2:
		{
			byte b2 = wp1[primaryWeaponMountWeapon].numBarrels;
			for (byte b3 = 0; b3 < b2; b3++)
			{
				mainC.renderingMain.Add_Muzzle_Flash((ushort)playerID, 0.05f, 0.05f, global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].offset[b3, 9].x, global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].offset[b3, 9].y, global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].offset[b3, 9].z);
			}
			break;
		}
		case 11:
		{
			byte b2 = wp1[primaryWeaponMountWeapon].numBarrels;
			for (byte b3 = 0; b3 < b2; b3++)
			{
				apffwV1.v[0] = global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].offset[b3, 9].x;
				apffwV1.v[1] = global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].offset[b3, 9].y;
				apffwV1.v[2] = global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].offset[b3, 9].z;
				apffwV2.v[0] = global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].offset[b3, 5].x;
				apffwV2.v[1] = global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].offset[b3, 5].y;
				apffwV2.v[2] = global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].offset[b3, 5].z;
			}
			break;
		}
		}
	}

	public void Add_Bullet(short actID, byte threadID)
	{
		byte uBufferID = global::Rendering.Rendering.uBufferID;
		if ((bulletActive[uBufferID, currentBullet] & 3) > 0)
		{
			short num = currentBullet++;
			while (currentBullet < 100 && (bulletActive[uBufferID, currentBullet] & 3) > 0)
			{
				currentBullet++;
			}
			if (currentBullet >= 100)
			{
				currentBullet = 0;
				while (currentBullet < num && (bulletActive[uBufferID, currentBullet] & 3) > 0)
				{
					currentBullet++;
				}
			}
			if ((bulletActive[uBufferID, currentBullet] & 3) > 0)
			{
				return;
			}
		}
		short num2 = mainC.playersMain.Get_Player_Index(actID, -1);
		if (num2 < 1)
		{
			return;
		}
		byte b = global::Networking.Networking.networkBytes[0];
		short num3 = global::Networking.Networking.networkSBytes[0];
		bullet[currentBullet].weaponID = (byte)num3;
		byte objectID = global::MainGame.MainGame.playerVehicles[num2].mounts[b].objectID;
		if (wp1[num3].snd_fire != null)
		{
			mainC.soundsMain.Play_Sound(wp1[num3].snd_fire, bullet[currentBullet].phys1.position.v[0], bullet[currentBullet].phys1.position.v[1], bullet[currentBullet].phys1.position.v[2], global::Players.Players.players[num2].charP.velocity.v[0], global::Players.Players.players[num2].charP.velocity.v[1], global::Players.Players.players[num2].charP.velocity.v[2]);
		}
		bullet[currentBullet].playerID = num2;
		byte b2 = (byte)global::Networking.Networking.networkSBytes[2];
		float muzzleVelocity = ammo[b2].muzzleVelocity;
		bullet[currentBullet].ammoType = ammo[b2].type;
		bullet[currentBullet].ammoIndex = b2;
		bullet[currentBullet].phys1.position.v[0] = global::Networking.Networking.networkHS[0].ToSingle();
		bullet[currentBullet].phys1.position.v[1] = global::Networking.Networking.networkHS[1].ToSingle();
		bullet[currentBullet].phys1.position.v[2] = global::Networking.Networking.networkHS[2].ToSingle();
		bullet[currentBullet].phys1.angle.v[0] = global::Networking.Networking.networkHS[3].ToSingle();
		bullet[currentBullet].phys1.angle.v[2] = global::Networking.Networking.networkHS[4].ToSingle();
		float num4 = global::Networking.Networking.networkHS[5].ToSingle();
		float num5 = global::Networking.Networking.networkHS[6].ToSingle();
		float num6 = (float)Math.Sin(num5);
		switch (bullet[currentBullet].ammoType)
		{
		case 3:
			bullet[currentBullet].phys1.velocity.v[0] = global::Players.Players.players[num2].charP.velocity.v[0] + (float)((0.0 - Math.Sin(num4)) * (double)muzzleVelocity * (double)num6);
			bullet[currentBullet].phys1.velocity.v[1] = global::Players.Players.players[num2].charP.velocity.v[1] + (float)(Math.Cos(num4) * (double)muzzleVelocity * (double)num6);
			bullet[currentBullet].phys1.velocity.v[2] = global::Players.Players.players[num2].charP.velocity.v[2] + (float)((0.0 - Math.Cos(num5)) * (double)muzzleVelocity);
			break;
		case 6:
		case 12:
		{
			bullet[currentBullet].phys1.velocity.v[0] = global::Players.Players.players[num2].charP.velocity.v[0] + (float)((0.0 - Math.Sin(num4)) * (double)muzzleVelocity * (double)num6);
			bullet[currentBullet].phys1.velocity.v[1] = global::Players.Players.players[num2].charP.velocity.v[1] + (float)(Math.Cos(num4) * (double)muzzleVelocity * (double)num6);
			bullet[currentBullet].phys1.velocity.v[2] = global::Players.Players.players[num2].charP.velocity.v[2] + (float)((0.0 - Math.Cos(num5)) * (double)muzzleVelocity);
			ref Matrix reference = ref bullet[currentBullet].mv[uBufferID];
			reference = global::MainGame.MainGame.playerVehicles[num2].mv[uBufferID];
			bullet[currentBullet].rot = Quaternion.CreateFromRotationMatrix(bullet[currentBullet].mv[uBufferID]);
			break;
		}
		default:
			bullet[currentBullet].phys1.velocity.v[0] = (float)((0.0 - Math.Sin(num4)) * (double)muzzleVelocity * (double)num6);
			bullet[currentBullet].phys1.velocity.v[1] = (float)(Math.Cos(num4) * (double)muzzleVelocity * (double)num6);
			bullet[currentBullet].phys1.velocity.v[2] = (float)((0.0 - Math.Cos(num5)) * (double)muzzleVelocity);
			break;
		}
		bullet[currentBullet].phys1.acceleration.v[2] = ammo[b2].accelerationZ;
		bullet[currentBullet].phys1.initialTime = global::Networking.Networking.networkDoubles[0];
		bulletActive[uBufferID, currentBullet] = 1;
		bullet[currentBullet].tracer = global::Networking.Networking.networkSBytes[1];
		float muzzleFlashTimer = global::MainGame.MainGame.playerVehicles[num2].weapons[objectID].muzzleFlashTimer;
		global::MainGame.MainGame.playerVehicles[0].weapons[objectID].muzzleFlashTimer = 0.023f;
		bullet[currentBullet].lightID = -1;
		if (ammo[b2].sound != null)
		{
			bullet[currentBullet].soundID = mainC.soundsMain.Play_Sound(ammo[b2].sound, bullet[currentBullet].phys1.position.v[0], bullet[currentBullet].phys1.position.v[1], bullet[currentBullet].phys1.position.v[2], global::Players.Players.players[num2].charP.velocity.v[0] + bullet[currentBullet].phys1.velocity.v[0], global::Players.Players.players[num2].charP.velocity.v[1] + bullet[currentBullet].phys1.velocity.v[1], global::Players.Players.players[num2].charP.velocity.v[2] + bullet[currentBullet].phys1.velocity.v[2]);
		}
		bullet[currentBullet].soundID2 = -1;
		if (ammo[b2].sound2 != null)
		{
			bullet[currentBullet].soundID2 = b2;
		}
		switch (bullet[currentBullet].ammoType)
		{
		case 0:
		case 1:
			if (global::Players.Players.players[num2].makeParticle)
			{
				abV1.v[0] = global::Players.Players.players[num2].weapon1.pfx1;
				abV1.v[1] = global::Players.Players.players[num2].weapon1.pfx2;
				abV1.v[2] = global::Players.Players.players[num2].weapon1.pfx3;
				mainC.renderingMain.New_Particle(1, ref bullet[currentBullet].phys1.position, ref bullet[currentBullet].phys1.velocity, ref abV1, ref abV1, num2, threadID);
			}
			global::Players.Players.players[num2].makeParticle = false;
			break;
		case 2:
			if (laserLights[curLaserLight] > -1)
			{
				bullet[laserLights[curLaserLight]].lightID = -1;
			}
			bullet[currentBullet].lightID = (sbyte)curLaserLight;
			laserLights[curLaserLight] = (sbyte)currentBullet;
			curLaserLight++;
			if (curLaserLight >= numLaserLights)
			{
				curLaserLight = 0;
			}
			break;
		case 3:
		{
			bullet[currentBullet].timer = ammo[b2].timer;
			bullet[currentBullet].particleTimer = ammo[b2].particleTimer;
			uBufferID = global::Rendering.Rendering.uBufferID;
			if (laserLights[curLaserLight] > -1)
			{
				bullet[laserLights[curLaserLight]].lightID = -1;
			}
			bullet[currentBullet].lightID = (sbyte)curLaserLight;
			byte b3 = bullet[currentBullet].ammoIndex;
			ammoLightPos[uBufferID, curLaserLight].X = bullet[currentBullet].phys1.position.v[0];
			ammoLightPos[uBufferID, curLaserLight].Y = bullet[currentBullet].phys1.position.v[1];
			ammoLightPos[uBufferID, curLaserLight].Z = bullet[currentBullet].phys1.position.v[2];
			ammoLightPos[uBufferID, curLaserLight].W = ammo[b3].colorIntensity;
			ammoLightColor[uBufferID, curLaserLight].X = ammo[b3].lightColor[0];
			ammoLightColor[uBufferID, curLaserLight].Y = ammo[b3].lightColor[1];
			ammoLightColor[uBufferID, curLaserLight].Z = ammo[b3].lightColor[2];
			ammoLightColor[uBufferID, curLaserLight].W = ammo[b3].lightColor[3];
			laserLights[curLaserLight] = (sbyte)currentBullet;
			curLaserLight++;
			if (curLaserLight >= numLaserLights)
			{
				curLaserLight = 0;
			}
			break;
		}
		case 4:
			bullet[currentBullet].timer = ammo[b2].timer;
			break;
		case 5:
			if (global::Networking.Networking.networkDoubles[0] >= 0.0)
			{
				bullet[currentBullet].phys1.velocity.v[0] = global::Players.Players.players[num2].charP.velocity.v[0] + (float)((0.0 - Math.Sin(num4)) * (double)muzzleVelocity * (double)num6);
				bullet[currentBullet].phys1.velocity.v[1] = global::Players.Players.players[num2].charP.velocity.v[1] + (float)(Math.Cos(num4) * (double)muzzleVelocity * (double)num6);
				bullet[currentBullet].phys1.velocity.v[2] = global::Players.Players.players[num2].charP.velocity.v[2] + (float)((0.0 - Math.Cos(num5)) * (double)muzzleVelocity);
				bullet[currentBullet].phys1.acceleration.v[2] = -32.15223f;
				bullet[currentBullet].timer = ammo[bullet[currentBullet].ammoIndex].timer;
				global::Players.Players.players[num2].renderWeapon = (byte)(global::Players.Players.players[num2].renderWeapon | 1);
				mainC.programsMain.Start_Animation((ushort)num2, ref global::Players.Players.players[num2].jt1, ref global::Players.Players.players[num2].animations, global::Players.Players.players[num2].programCollection, wp1[num3].AnimationSpecial1, 1f, 1f);
			}
			else
			{
				bulletActive[uBufferID, currentBullet] = 0;
				global::MainGame.MainGame.playerVehicles[num2].weapons[objectID].muzzleFlashTimer = muzzleFlashTimer;
				byte b3 = (byte)wp1[num3].AnimationThrow;
				global::Players.Players.players[num2].animations[b3].callBackType = 8;
				global::Players.Players.players[num2].animations[b3].callBack = (byte)currentBullet;
				mainC.programsMain.Start_Animation((ushort)num2, ref global::Players.Players.players[num2].jt1, ref global::Players.Players.players[num2].animations, global::Players.Players.players[num2].programCollection, b3, 1f, 1f);
				bullet[currentBullet].timer = -10f;
			}
			global::Players.Players.players[num2].playerIsMoving = 256;
			break;
		}
		bullet[currentBullet].rotation = 0f;
		currentBullet++;
		if (currentBullet >= 100)
		{
			currentBullet = 0;
		}
	}

	public void Release_Grenade(byte bulletID)
	{
		byte b = (byte)bullet[bulletID].playerID;
		byte objectID = global::MainGame.MainGame.playerVehicles[b].mounts[global::MainGame.MainGame.primaryWeaponMount].objectID;
		byte weaponID = bullet[bulletID].weaponID;
		float muzzleVelocity = ammo[bullet[bulletID].ammoIndex].muzzleVelocity;
		bullet[bulletID].phys1.angle.v[0] = global::Players.Players.players[b].xRotation;
		bullet[bulletID].phys1.angle.v[2] = global::Players.Players.players[b].zRotation;
		float num = bullet[bulletID].phys1.angle.v[2] / 57.29578f;
		float num2 = (bullet[bulletID].phys1.angle.v[0] + 90f) / 57.29578f;
		float num3 = (float)Math.Sin(num2);
		bullet[bulletID].phys1.velocity.v[0] = global::Players.Players.players[b].charP.velocity.v[0] + (float)((0.0 - Math.Sin(num)) * (double)muzzleVelocity * (double)num3);
		bullet[bulletID].phys1.velocity.v[1] = global::Players.Players.players[b].charP.velocity.v[1] + (float)(Math.Cos(num) * (double)muzzleVelocity * (double)num3);
		bullet[bulletID].phys1.velocity.v[2] = global::Players.Players.players[b].charP.velocity.v[2] + (float)((0.0 - Math.Cos(num2)) * (double)muzzleVelocity);
		bullet[bulletID].phys1.position.v[0] = global::MainGame.MainGame.playerVehicles[b].weapons[objectID].offset[0, 2].x + global::MainGame.MainGame.playerVehicles[b].weapons[objectID].posX;
		bullet[bulletID].phys1.position.v[1] = global::MainGame.MainGame.playerVehicles[b].weapons[objectID].offset[0, 2].y + global::MainGame.MainGame.playerVehicles[b].weapons[objectID].posY;
		bullet[bulletID].phys1.position.v[2] = global::MainGame.MainGame.playerVehicles[b].weapons[objectID].offset[0, 2].z + global::MainGame.MainGame.playerVehicles[b].weapons[objectID].posZ;
		Matrix matrix = Matrix.CreateRotationX(global::Players.Players.players[b].xRotation * ((float)Math.PI / 180f)) * Matrix.CreateRotationZ(global::Players.Players.players[b].zRotation * ((float)Math.PI / 180f));
		bullet[bulletID].phys1.velocity.v[0] = global::Players.Players.players[b].charP.velocity.v[0] + muzzleVelocity * matrix.M21;
		bullet[bulletID].phys1.velocity.v[1] = global::Players.Players.players[b].charP.velocity.v[1] + muzzleVelocity * matrix.M22;
		bullet[bulletID].phys1.velocity.v[2] = global::Players.Players.players[b].charP.velocity.v[2] + muzzleVelocity * matrix.M23;
		if (global::Networking.Networking.inGame && b == 0)
		{
			ref HalfSingle reference = ref global::Networking.Networking.networkHS[0];
			reference = new HalfSingle(bullet[bulletID].phys1.position.v[0]);
			ref HalfSingle reference2 = ref global::Networking.Networking.networkHS[1];
			reference2 = new HalfSingle(bullet[bulletID].phys1.position.v[1]);
			ref HalfSingle reference3 = ref global::Networking.Networking.networkHS[2];
			reference3 = new HalfSingle(bullet[bulletID].phys1.position.v[2]);
			ref HalfSingle reference4 = ref global::Networking.Networking.networkHS[3];
			reference4 = new HalfSingle(bullet[bulletID].phys1.angle.v[0]);
			ref HalfSingle reference5 = ref global::Networking.Networking.networkHS[4];
			reference5 = new HalfSingle(bullet[bulletID].phys1.angle.v[2]);
			ref HalfSingle reference6 = ref global::Networking.Networking.networkHS[5];
			reference6 = new HalfSingle(num);
			ref HalfSingle reference7 = ref global::Networking.Networking.networkHS[6];
			reference7 = new HalfSingle(num2);
			global::Networking.Networking.networkBytes[0] = global::MainGame.MainGame.primaryWeaponMount;
			global::Networking.Networking.networkSBytes[0] = (sbyte)weaponID;
			global::Networking.Networking.networkSBytes[1] = bullet[bulletID].tracer;
			global::Networking.Networking.networkSBytes[2] = (sbyte)bullet[bulletID].ammoIndex;
			global::Networking.Networking.networkDoubles[0] = bullet[bulletID].phys1.initialTime;
			mainC.networkingMain.XBOX_Send_Network_Message1(1);
		}
		bullet[bulletID].phys1.acceleration.v[2] = -32.15223f;
		bullet[bulletID].timer = ammo[bullet[bulletID].ammoIndex].timer;
		global::Players.Players.players[b].renderWeapon = (byte)(global::Players.Players.players[b].renderWeapon | 1);
	}

	public void Cancel_Grenade(ushort playerID, byte bulletID)
	{
		bulletActive[0, bulletID] = 0;
		bulletActive[1, bulletID] = 0;
		mainC.vehicles.Set_Mount_Weapon(playerID, global::MainGame.MainGame.primaryWeaponMount, global::MainGame.MainGame.primaryWeaponMount);
		global::Players.Players.players[playerID].animations[global::Players.Players.players[playerID].programSwitchWeapons].callBackType = 12;
		mainC.programsMain.Start_Animation(playerID, ref global::Players.Players.players[playerID].jt1, ref global::Players.Players.players[playerID].animations, global::Players.Players.players[playerID].programCollection, global::Players.Players.players[playerID].programSwitchWeapons, 1f, 1f);
	}

	public void Add_Weapon_Modifier(ushort playerID, byte modifierID)
	{
		global::Players.Players.players[playerID].weaponModifier |= wpnMod[modifierID].mask;
		curModifierTime[playerID, modifierID] = wpnMod[modifierID].time;
	}

	public void Move_Weapon_Mount_Player(ushort playerID, ushort mID, float rotX, float rotY, float rotZ)
	{
		byte uBufferID = global::Rendering.Rendering.uBufferID;
		if (mID < numWeaponMounts)
		{
			wpmMounts[mID].rotX += rotX * wpmMounts[mID].turretSpeedFactorX;
			if (wpmMounts[mID].rotX > wpmMounts[mID].maxRotX)
			{
				wpmMounts[mID].rotX = wpmMounts[mID].maxRotX;
			}
			else if (wpmMounts[mID].rotX < wpmMounts[mID].minRotX)
			{
				wpmMounts[mID].rotX = wpmMounts[mID].minRotX;
			}
			if (wpmMounts[mID].rotX > 180f)
			{
				wpmMounts[mID].rotX -= 360f;
			}
			else if (wpmMounts[mID].rotX < -180f)
			{
				wpmMounts[mID].rotX += 360f;
			}
			wpmMounts[mID].rotY += rotY * wpmMounts[mID].turretSpeedFactorY;
			if (wpmMounts[mID].rotY > wpmMounts[mID].maxRotY)
			{
				wpmMounts[mID].rotY = wpmMounts[mID].maxRotY;
			}
			else if (wpmMounts[mID].rotY < wpmMounts[mID].minRotY)
			{
				wpmMounts[mID].rotY = wpmMounts[mID].minRotY;
			}
			if (wpmMounts[mID].rotY > 180f)
			{
				wpmMounts[mID].rotY -= 360f;
			}
			else if (wpmMounts[mID].rotY < -180f)
			{
				wpmMounts[mID].rotY += 360f;
			}
			wpmMounts[mID].rotZ += rotZ * wpmMounts[mID].turretSpeedFactorZ;
			if (wpmMounts[mID].rotZ > wpmMounts[mID].maxRotZ)
			{
				wpmMounts[mID].rotZ = wpmMounts[mID].maxRotZ;
			}
			else if (wpmMounts[mID].rotZ < wpmMounts[mID].minRotZ)
			{
				wpmMounts[mID].rotZ = wpmMounts[mID].minRotZ;
			}
			if (wpmMounts[mID].rotZ > 180f)
			{
				wpmMounts[mID].rotZ -= 360f;
			}
			else if (wpmMounts[mID].rotZ < -180f)
			{
				wpmMounts[mID].rotZ += 360f;
			}
			ref Matrix reference = ref wpmMounts[mID].mv[uBufferID];
			reference = Matrix.CreateRotationY(wpmMounts[mID].rotY * wpmMounts[mID].turretRotFactorY * ((float)Math.PI / 180f)) * Matrix.CreateRotationX(wpmMounts[mID].rotX * wpmMounts[mID].turretRotFactorX * ((float)Math.PI / 180f)) * Matrix.CreateRotationZ(wpmMounts[mID].rotZ * wpmMounts[mID].turretRotFactorZ * ((float)Math.PI / 180f)) * wpmMounts[mID].mvo;
			ref Matrix reference2 = ref global::Players.Players.players[playerID].mv[uBufferID];
			reference2 = Matrix.CreateTranslation(wpmMounts[mID].playerPosX, wpmMounts[mID].playerPosY, wpmMounts[mID].playerPosZ) * Matrix.CreateRotationY(wpmMounts[mID].rotY * wpmMounts[mID].playerRotFactorY * ((float)Math.PI / 180f)) * Matrix.CreateRotationX(wpmMounts[mID].rotX * wpmMounts[mID].playerRotFactorX * ((float)Math.PI / 180f)) * Matrix.CreateRotationZ(wpmMounts[mID].rotZ * wpmMounts[mID].playerRotFactorZ * ((float)Math.PI / 180f)) * wpmMounts[mID].mvo;
		}
	}

	public void New_Frame_Housekeeping()
	{
		byte uBufferID = global::Rendering.Rendering.uBufferID;
		byte rBufferID = global::Rendering.Rendering.rBufferID;
		for (ushort num = 0; num < 100; num++)
		{
			bulletActive[uBufferID, num] = bulletActive[rBufferID, num];
		}
	}

	public void Process_Player_Weapons(int playerID, int currentWeapon)
	{
		byte uBufferID = global::Rendering.Rendering.uBufferID;
		switch (Vehicles.vehicles[global::Players.Players.players[playerID].curVehicle].type)
		{
		case 3:
		{
			ushort curVehicle = global::Players.Players.players[playerID].curVehicle;
			ushort num2 = Vehicles.vehicles[curVehicle].weaponMounts[0];
			_ = wpmMounts[num2].weaponID;
			Matrix matrix = wpmMounts[num2].mv[uBufferID];
			global::Players.Players.players[playerID].weapon1.box.pos1.v[0] = 0f;
			global::Players.Players.players[playerID].weapon1.box.pos1.v[1] = 0f;
			global::Players.Players.players[playerID].weapon1.box.pos1.v[2] = 0f;
			for (byte b2 = 0; b2 < wp1[currentWeapon].numBarrels; b2++)
			{
				ppwV1.v[0] = wp1[currentWeapon].offset[b2, 0].v[0] * matrix.M11 + wp1[currentWeapon].offset[b2, 0].v[1] * matrix.M21 + wp1[currentWeapon].offset[b2, 0].v[2] * matrix.M31 + matrix.M41;
				ppwV1.v[1] = wp1[currentWeapon].offset[b2, 0].v[0] * matrix.M12 + wp1[currentWeapon].offset[b2, 0].v[1] * matrix.M22 + wp1[currentWeapon].offset[b2, 0].v[2] * matrix.M32 + matrix.M42;
				ppwV1.v[2] = wp1[currentWeapon].offset[b2, 0].v[0] * matrix.M13 + wp1[currentWeapon].offset[b2, 0].v[1] * matrix.M23 + wp1[currentWeapon].offset[b2, 0].v[2] * matrix.M33 + matrix.M43;
				global::Players.Players.players[playerID].weapon1.offset[b2, 2].v[0] = ppwV1.v[0];
				global::Players.Players.players[playerID].weapon1.offset[b2, 2].v[1] = ppwV1.v[1];
				global::Players.Players.players[playerID].weapon1.offset[b2, 2].v[2] = ppwV1.v[2];
				global::Players.Players.players[playerID].weapon1.offset[b2, 7].v[0] = wp1[currentWeapon].offset[b2, 2].v[0] * matrix.M11 + wp1[currentWeapon].offset[b2, 2].v[1] * matrix.M21 + wp1[currentWeapon].offset[b2, 2].v[2] * matrix.M31;
				global::Players.Players.players[playerID].weapon1.offset[b2, 7].v[1] = wp1[currentWeapon].offset[b2, 2].v[0] * matrix.M12 + wp1[currentWeapon].offset[b2, 2].v[1] * matrix.M22 + wp1[currentWeapon].offset[b2, 2].v[2] * matrix.M32;
				global::Players.Players.players[playerID].weapon1.offset[b2, 7].v[2] = wp1[currentWeapon].offset[b2, 2].v[0] * matrix.M13 + wp1[currentWeapon].offset[b2, 2].v[1] * matrix.M23 + wp1[currentWeapon].offset[b2, 2].v[2] * matrix.M33;
				global::Players.Players.players[playerID].weapon1.offset[b2, 8].v[0] = wp1[currentWeapon].offset[b2, 3].v[0] * matrix.M11 + wp1[currentWeapon].offset[b2, 3].v[1] * matrix.M21 + wp1[currentWeapon].offset[b2, 3].v[2] * matrix.M31;
				global::Players.Players.players[playerID].weapon1.offset[b2, 8].v[1] = wp1[currentWeapon].offset[b2, 3].v[0] * matrix.M12 + wp1[currentWeapon].offset[b2, 3].v[1] * matrix.M22 + wp1[currentWeapon].offset[b2, 3].v[2] * matrix.M32;
				global::Players.Players.players[playerID].weapon1.offset[b2, 8].v[2] = wp1[currentWeapon].offset[b2, 3].v[0] * matrix.M13 + wp1[currentWeapon].offset[b2, 3].v[1] * matrix.M23 + wp1[currentWeapon].offset[b2, 3].v[2] * matrix.M33;
				ppwV2.v[0] = wp1[currentWeapon].offset[b2, 1].v[0] * matrix.M11 + wp1[currentWeapon].offset[b2, 1].v[1] * matrix.M21 + wp1[currentWeapon].offset[b2, 1].v[2] * matrix.M31 + matrix.M41;
				ppwV2.v[1] = wp1[currentWeapon].offset[b2, 1].v[0] * matrix.M12 + wp1[currentWeapon].offset[b2, 1].v[1] * matrix.M22 + wp1[currentWeapon].offset[b2, 1].v[2] * matrix.M32 + matrix.M42;
				ppwV2.v[2] = wp1[currentWeapon].offset[b2, 1].v[0] * matrix.M13 + wp1[currentWeapon].offset[b2, 1].v[1] * matrix.M23 + wp1[currentWeapon].offset[b2, 1].v[2] * matrix.M33 + matrix.M43;
				if (b2 == 0)
				{
					global::Players.Players.players[playerID].laserPos[uBufferID, 0] = ppwV2.v[0];
					global::Players.Players.players[playerID].laserPos[uBufferID, 1] = ppwV2.v[1];
					global::Players.Players.players[playerID].laserPos[uBufferID, 2] = ppwV2.v[2];
				}
				ppwV1.v[0] = ppwV2.v[0] - ppwV1.v[0];
				ppwV1.v[1] = ppwV2.v[1] - ppwV1.v[1];
				ppwV1.v[2] = ppwV2.v[2] - ppwV1.v[2];
				global::Players.Players.players[playerID].weapon1.offset[b2, 5].v[0] = ppwV1.v[0];
				global::Players.Players.players[playerID].weapon1.offset[b2, 5].v[1] = ppwV1.v[1];
				global::Players.Players.players[playerID].weapon1.offset[b2, 5].v[2] = ppwV1.v[2];
				global::Players.Players.players[playerID].weapon1.offset[b2, 9].v[0] = global::Players.Players.players[playerID].weapon1.offset[b2, 2].v[0] + ppwV1.v[0] * wp1[currentWeapon].particleDistance + global::Players.Players.players[playerID].weapon1.box.pos1.v[0];
				global::Players.Players.players[playerID].weapon1.offset[b2, 9].v[1] = global::Players.Players.players[playerID].weapon1.offset[b2, 2].v[1] + ppwV1.v[1] * wp1[currentWeapon].particleDistance + global::Players.Players.players[playerID].weapon1.box.pos1.v[1];
				global::Players.Players.players[playerID].weapon1.offset[b2, 9].v[2] = global::Players.Players.players[playerID].weapon1.offset[b2, 2].v[2] + ppwV1.v[2] * wp1[currentWeapon].particleDistance + global::Players.Players.players[playerID].weapon1.box.pos1.v[2];
				global::Util.Util.NormalizeVertex(ref ppwV1);
				if (b2 == 0)
				{
					global::Players.Players.players[playerID].laserDir[uBufferID, 0] = ppwV1.v[0];
					global::Players.Players.players[playerID].laserDir[uBufferID, 1] = ppwV1.v[1];
					global::Players.Players.players[playerID].laserDir[uBufferID, 2] = ppwV1.v[2];
				}
				float y = (float)Math.Acos(0f - ppwV1.v[2]);
				ppwV1.v[2] = 0f;
				global::Util.Util.NormalizeVertex(ref ppwV1);
				float num = (float)Math.Acos(ppwV1.v[1]);
				if (ppwV1.v[0] > 0f)
				{
					num = (float)Math.PI * 2f - num;
				}
				global::Players.Players.players[playerID].weapon1.offset[b2, 6].v[0] = num;
				global::Players.Players.players[playerID].weapon1.offset[b2, 6].v[1] = y;
			}
			break;
		}
		case 0:
		case 1:
		case 4:
		case 5:
		case 6:
		case 7:
		case 8:
		{
			byte numMounts = Vehicles.vehicles[global::Players.Players.players[playerID].curVehicle].numMounts;
			for (byte b = 0; b < numMounts; b++)
			{
				if (global::MainGame.MainGame.playerVehicles[playerID].mounts[b].type == 1 && global::MainGame.MainGame.playerVehicles[playerID].mounts[b].objectAttached == 1)
				{
					byte objectID = global::MainGame.MainGame.playerVehicles[playerID].mounts[b].objectID;
					currentWeapon = global::MainGame.MainGame.playerVehicles[playerID].weapons[objectID].weaponID;
					global::MainGame.MainGame.playerVehicles[playerID].weapons[objectID].curHeat -= wp1[currentWeapon].heatDissipation * global::MainGame.MainGame.frametime;
					if (global::MainGame.MainGame.playerVehicles[playerID].weapons[objectID].curHeat < 0f)
					{
						global::MainGame.MainGame.playerVehicles[playerID].weapons[objectID].curHeat = 0f;
					}
					Matrix matrix = global::MainGame.MainGame.playerVehicles[playerID].mounts[b].mvCurrent[uBufferID];
					global::MainGame.MainGame.playerVehicles[playerID].weapons[objectID].posX = global::Players.Players.players[playerID].charP.position.v[0] + matrix.M41;
					global::MainGame.MainGame.playerVehicles[playerID].weapons[objectID].posY = global::Players.Players.players[playerID].charP.position.v[1] + matrix.M42;
					global::MainGame.MainGame.playerVehicles[playerID].weapons[objectID].posZ = global::Players.Players.players[playerID].charP.position.v[2] + matrix.M43;
					global::Players.Players.players[playerID].laserPos[uBufferID, 0] = 0f;
					global::Players.Players.players[playerID].laserPos[uBufferID, 1] = 0f;
					global::Players.Players.players[playerID].laserPos[uBufferID, 2] = 0f;
					for (byte b2 = 0; b2 < wp1[currentWeapon].numBarrels; b2++)
					{
						ppwV1.v[0] = wp1[currentWeapon].offset[b2, 0].v[0] * matrix.M11 + wp1[currentWeapon].offset[b2, 0].v[1] * matrix.M21 + wp1[currentWeapon].offset[b2, 0].v[2] * matrix.M31;
						ppwV1.v[1] = wp1[currentWeapon].offset[b2, 0].v[0] * matrix.M12 + wp1[currentWeapon].offset[b2, 0].v[1] * matrix.M22 + wp1[currentWeapon].offset[b2, 0].v[2] * matrix.M32;
						ppwV1.v[2] = wp1[currentWeapon].offset[b2, 0].v[0] * matrix.M13 + wp1[currentWeapon].offset[b2, 0].v[1] * matrix.M23 + wp1[currentWeapon].offset[b2, 0].v[2] * matrix.M33;
						global::MainGame.MainGame.playerVehicles[playerID].weapons[objectID].offset[b2, 2].x = ppwV1.v[0];
						global::MainGame.MainGame.playerVehicles[playerID].weapons[objectID].offset[b2, 2].y = ppwV1.v[1];
						global::MainGame.MainGame.playerVehicles[playerID].weapons[objectID].offset[b2, 2].z = ppwV1.v[2];
						global::MainGame.MainGame.playerVehicles[playerID].weapons[objectID].offset[b2, 7].x = wp1[currentWeapon].offset[b2, 2].v[0] * matrix.M11 + wp1[currentWeapon].offset[b2, 2].v[1] * matrix.M21 + wp1[currentWeapon].offset[b2, 2].v[2] * matrix.M31;
						global::MainGame.MainGame.playerVehicles[playerID].weapons[objectID].offset[b2, 7].y = wp1[currentWeapon].offset[b2, 2].v[0] * matrix.M12 + wp1[currentWeapon].offset[b2, 2].v[1] * matrix.M22 + wp1[currentWeapon].offset[b2, 2].v[2] * matrix.M32;
						global::MainGame.MainGame.playerVehicles[playerID].weapons[objectID].offset[b2, 7].z = wp1[currentWeapon].offset[b2, 2].v[0] * matrix.M13 + wp1[currentWeapon].offset[b2, 2].v[1] * matrix.M23 + wp1[currentWeapon].offset[b2, 2].v[2] * matrix.M33;
						global::MainGame.MainGame.playerVehicles[playerID].weapons[objectID].offset[b2, 8].x = wp1[currentWeapon].offset[b2, 3].v[0] * matrix.M11 + wp1[currentWeapon].offset[b2, 3].v[1] * matrix.M21 + wp1[currentWeapon].offset[b2, 3].v[2] * matrix.M31;
						global::MainGame.MainGame.playerVehicles[playerID].weapons[objectID].offset[b2, 8].y = wp1[currentWeapon].offset[b2, 3].v[0] * matrix.M12 + wp1[currentWeapon].offset[b2, 3].v[1] * matrix.M22 + wp1[currentWeapon].offset[b2, 3].v[2] * matrix.M32;
						global::MainGame.MainGame.playerVehicles[playerID].weapons[objectID].offset[b2, 8].z = wp1[currentWeapon].offset[b2, 3].v[0] * matrix.M13 + wp1[currentWeapon].offset[b2, 3].v[1] * matrix.M23 + wp1[currentWeapon].offset[b2, 3].v[2] * matrix.M33;
						ppwV2.v[0] = wp1[currentWeapon].offset[b2, 1].v[0] * matrix.M11 + wp1[currentWeapon].offset[b2, 1].v[1] * matrix.M21 + wp1[currentWeapon].offset[b2, 1].v[2] * matrix.M31;
						ppwV2.v[1] = wp1[currentWeapon].offset[b2, 1].v[0] * matrix.M12 + wp1[currentWeapon].offset[b2, 1].v[1] * matrix.M22 + wp1[currentWeapon].offset[b2, 1].v[2] * matrix.M32;
						ppwV2.v[2] = wp1[currentWeapon].offset[b2, 1].v[0] * matrix.M13 + wp1[currentWeapon].offset[b2, 1].v[1] * matrix.M23 + wp1[currentWeapon].offset[b2, 1].v[2] * matrix.M33;
						global::Players.Players.players[playerID].laserPos[uBufferID, 0] += global::MainGame.MainGame.playerVehicles[playerID].weapons[objectID].posX + ppwV1.v[0];
						global::Players.Players.players[playerID].laserPos[uBufferID, 1] += global::MainGame.MainGame.playerVehicles[playerID].weapons[objectID].posY + ppwV1.v[1];
						global::Players.Players.players[playerID].laserPos[uBufferID, 2] += global::MainGame.MainGame.playerVehicles[playerID].weapons[objectID].posZ + ppwV1.v[2];
						ppwV1.v[0] = ppwV2.v[0] - ppwV1.v[0];
						ppwV1.v[1] = ppwV2.v[1] - ppwV1.v[1];
						ppwV1.v[2] = ppwV2.v[2] - ppwV1.v[2];
						float num;
						if (playerID == 0 && !global::MainGame.MainGame.usingIronSights && !global::MainGame.MainGame.usingScope)
						{
							ppwV1.v[0] = global::MainGame.MainGame.commanderObjectivePosition.x - global::Players.Players.players[playerID].laserPos[uBufferID, 0];
							ppwV1.v[1] = global::MainGame.MainGame.commanderObjectivePosition.y - global::Players.Players.players[playerID].laserPos[uBufferID, 1];
							ppwV1.v[2] = global::MainGame.MainGame.commanderObjectivePosition.z - global::Players.Players.players[playerID].laserPos[uBufferID, 2];
							num = (float)Math.Sqrt(ppwV1.v[0] * ppwV1.v[0] + ppwV1.v[1] * ppwV1.v[1] + ppwV1.v[2] * ppwV1.v[2]);
							if (num != 0f)
							{
								ppwV1.v[0] /= num;
								ppwV1.v[1] /= num;
								ppwV1.v[2] /= num;
							}
						}
						global::MainGame.MainGame.playerVehicles[playerID].weapons[objectID].offset[b2, 5].x = ppwV1.v[0];
						global::MainGame.MainGame.playerVehicles[playerID].weapons[objectID].offset[b2, 5].y = ppwV1.v[1];
						global::MainGame.MainGame.playerVehicles[playerID].weapons[objectID].offset[b2, 5].z = ppwV1.v[2];
						global::MainGame.MainGame.playerVehicles[playerID].weapons[objectID].offset[b2, 9].x = global::MainGame.MainGame.playerVehicles[playerID].weapons[objectID].offset[b2, 2].x + ppwV1.v[0] * wp1[currentWeapon].particleDistance + global::MainGame.MainGame.playerVehicles[playerID].weapons[objectID].posX;
						global::MainGame.MainGame.playerVehicles[playerID].weapons[objectID].offset[b2, 9].y = global::MainGame.MainGame.playerVehicles[playerID].weapons[objectID].offset[b2, 2].y + ppwV1.v[1] * wp1[currentWeapon].particleDistance + global::MainGame.MainGame.playerVehicles[playerID].weapons[objectID].posY;
						global::MainGame.MainGame.playerVehicles[playerID].weapons[objectID].offset[b2, 9].z = global::MainGame.MainGame.playerVehicles[playerID].weapons[objectID].offset[b2, 2].z + ppwV1.v[2] * wp1[currentWeapon].particleDistance + global::MainGame.MainGame.playerVehicles[playerID].weapons[objectID].posZ;
						global::Util.Util.NormalizeVertex(ref ppwV1);
						if (b2 == 0)
						{
							global::Players.Players.players[playerID].laserDir[uBufferID, 0] = ppwV1.v[0];
							global::Players.Players.players[playerID].laserDir[uBufferID, 1] = ppwV1.v[1];
							global::Players.Players.players[playerID].laserDir[uBufferID, 2] = ppwV1.v[2];
						}
						float y = (float)Math.Acos(0f - ppwV1.v[2]);
						ppwV1.v[2] = 0f;
						global::Util.Util.NormalizeVertex(ref ppwV1);
						num = (float)Math.Acos(ppwV1.v[1]);
						if (ppwV1.v[0] > 0f)
						{
							num = (float)Math.PI * 2f - num;
						}
						global::MainGame.MainGame.playerVehicles[playerID].weapons[objectID].offset[b2, 6].x = num;
						global::MainGame.MainGame.playerVehicles[playerID].weapons[objectID].offset[b2, 6].y = y;
					}
					global::Players.Players.players[playerID].laserPos[uBufferID, 0] /= (int)wp1[currentWeapon].numBarrels;
					global::Players.Players.players[playerID].laserPos[uBufferID, 1] /= (int)wp1[currentWeapon].numBarrels;
					global::Players.Players.players[playerID].laserPos[uBufferID, 2] /= (int)wp1[currentWeapon].numBarrels;
				}
			}
			break;
		}
		}
		if (!global::Players.Players.players[playerID].shooting)
		{
			byte b3 = (byte)global::Players.Players.players[playerID].wpnIndex;
			global::Players.Players.players[playerID].weapon2[b3].fireRateAdjustment += global::MainGame.MainGame.frametime * wp1[currentWeapon].fireRateRecharge;
			if (global::Players.Players.players[playerID].weapon2[b3].fireRateAdjustment > 1f)
			{
				global::Players.Players.players[playerID].weapon2[b3].fireRateAdjustment = 1f;
			}
		}
	}

	public void Process_Ballistics(byte threadID)
	{
		bool flag = false;
		ushort objectID = 0;
		int num = -1;
		short pID = 0;
		float num2 = 0f;
		float num3 = 0f;
		float num4 = 0f;
		float num5 = -1f;
		float num6 = 0f;
		float distance = 0f;
		float num7 = 0f;
		float num8 = 0f;
		float num9 = 0f;
		Vector3 InitialRayStart = default(Vector3);
		Vector3 InitialRayEnd = default(Vector3);
		Vector3 IntersectPosition = default(Vector3);
		Vector3 IntersectNormal = default(Vector3);
		byte uBufferID = global::Rendering.Rendering.uBufferID;
		_ = global::Rendering.Rendering.rBufferID;
		byte b;
		int i;
		int j;
		for (i = 0; i < 44; i++)
		{
			b = 1;
			for (j = 0; j < numWeaponModifiers; j++)
			{
				curModifierTime[i, j] -= global::MainGame.MainGame.frametime;
				if (curModifierTime[i, j] < 0f)
				{
					curModifierTime[i, j] = 0f;
					global::Players.Players.players[i].weaponModifier &= (byte)(~b);
				}
				b <<= 1;
			}
		}
		if (global::Players.Players.players[0].shooting)
		{
			for (byte b2 = 0; b2 < global::MainGame.MainGame.playerVehicles[0].numMounts; b2++)
			{
				if (global::MainGame.MainGame.playerVehicles[0].mounts[b2].type == 1 && global::MainGame.MainGame.playerVehicles[0].mounts[b2].objectAttached == 1 && global::MainGame.MainGame.playerVehicles[0].weapons[global::MainGame.MainGame.playerVehicles[0].mounts[b2].objectID].shooting)
				{
					byte objectID2 = global::MainGame.MainGame.playerVehicles[0].mounts[b2].objectID;
					if (!global::MainGame.MainGame.playerVehicles[0].weapons[objectID2].reloading && global::MainGame.MainGame.playerVehicles[0].weapons[objectID2].curHeat < (float)(int)wp1[global::MainGame.MainGame.playerVehicles[0].weapons[objectID2].weaponID].heatMax)
					{
						global::MainGame.MainGame.playerVehicles[0].weapons[objectID2].curHeat += wp1[global::MainGame.MainGame.playerVehicles[0].weapons[objectID2].weaponID].heatGeneration * global::MainGame.MainGame.frametime;
						if (global::MainGame.MainGame.playerVehicles[0].weapons[objectID2].curHeat >= (float)(int)wp1[global::MainGame.MainGame.playerVehicles[0].weapons[objectID2].weaponID].heatMax)
						{
							global::MainGame.MainGame.playerVehicles[0].weapons[objectID2].curHeat = wp1[global::MainGame.MainGame.playerVehicles[0].weapons[objectID2].weaponID].heatMax + wp1[global::MainGame.MainGame.playerVehicles[0].weapons[objectID2].weaponID].coolMin;
						}
						if (global::MainGame.MainGame.playerVehicles[0].weapons[objectID2].currentRounds > 0 || wp1[global::MainGame.MainGame.playerVehicles[0].weapons[objectID2].weaponID].unLimitedAmmo)
						{
							Fire_Bullet_MainPlayer(b2, objectID2);
							if (global::MainGame.MainGame.playerVehicles[0].weapons[objectID2].currentRounds == 0 && Player_Has_Ammo_For_Weapon(0, objectID2) > 0)
							{
								global::MainGame.MainGame.playerVehicles[0].weapons[objectID2].needToReload = true;
								global::Players.Players.needToReload = true;
							}
							mainC.aiMain.Target_AI_To_Shots(0);
						}
						else if (global::MainGame.MainGame.unlimitedAmmo)
						{
							global::MainGame.MainGame.playerVehicles[0].weapons[objectID2].currentRounds = 10;
							Fire_Bullet_MainPlayer(b2, objectID2);
							mainC.aiMain.Target_AI_To_Shots(0);
						}
						else
						{
							byte b3 = Player_Has_Ammo_For_Weapon(0, objectID2);
							if (wp1[global::MainGame.MainGame.playerVehicles[0].weapons[objectID2].weaponID].autoReload && b3 > 1)
							{
								mainC.weaponsMain.Load_Ammo_Clip_Into_Player_Vehicle_Weapon(0, objectID2, 1);
								if (global::MainGame.MainGame.playerVehicles[0].weapons[objectID2].currentRounds > 0)
								{
									Fire_Bullet_MainPlayer(b2, objectID2);
									mainC.aiMain.Target_AI_To_Shots(0);
								}
							}
							else
							{
								global::Players.Players.needToReload = true;
								global::Players.Players.players[0].shooting = false;
								if (b3 < 2 && !global::MainGame.MainGame.playerVehicles[0].weapons[objectID2].needToReload)
								{
									mainC.soundsMain.Play_Priority_Sound(wp1[global::MainGame.MainGame.playerVehicles[0].weapons[objectID2].weaponID].snd_fire_empty, global::MainGame.MainGame.playerVehicles[0].weapons[objectID2].offset[0, 2].x + global::MainGame.MainGame.playerVehicles[0].weapons[objectID2].posX, global::MainGame.MainGame.playerVehicles[0].weapons[objectID2].offset[0, 2].y + global::MainGame.MainGame.playerVehicles[0].weapons[objectID2].posY, global::MainGame.MainGame.playerVehicles[0].weapons[objectID2].offset[0, 2].z + global::MainGame.MainGame.playerVehicles[0].weapons[objectID2].posZ, 0f, 0f, 0f);
								}
								global::MainGame.MainGame.playerVehicles[0].weapons[objectID2].needToReload = true;
							}
						}
					}
				}
			}
		}
		for (i = 1; i < global::MainGame.MainGame.maxHumanGamePlayers; i++)
		{
			if (global::Players.Players.players[i].shooting)
			{
				for (byte b2 = 0; b2 < global::MainGame.MainGame.playerVehicles[i].numMounts; b2++)
				{
					byte objectID2 = global::MainGame.MainGame.playerVehicles[i].mounts[b2].objectID;
					if (global::MainGame.MainGame.playerVehicles[i].mounts[b2].type == 1 && global::MainGame.MainGame.playerVehicles[i].mounts[b2].objectAttached == 1 && global::MainGame.MainGame.playerVehicles[i].weapons[objectID2].shooting)
					{
						switch (ammo[wp1[global::MainGame.MainGame.playerVehicles[i].weapons[objectID2].weaponID].ammoIndex].type)
						{
						case 0:
						case 1:
						case 2:
						case 8:
						case 13:
							global::MainGame.MainGame.playerVehicles[i].weapons[objectID2].shooting = true;
							global::MainGame.MainGame.playerVehicles[i].weapons[objectID2].currentRounds = 100;
							global::MainGame.MainGame.playerVehicles[i].weapons[objectID2].roundChambered = true;
							Fire_Bullet((short)i, b2, objectID2);
							break;
						}
					}
				}
				if ((global::Players.Players.players[i].shotOnce & 4) == 4)
				{
					global::Players.Players.players[i].shooting = false;
				}
			}
			else
			{
				firingStoppedAllPlayerWeapons((ushort)i);
			}
			global::Players.Players.players[i].shotOnce = 0;
		}
		for (; i < global::MainGame.MainGame.maxGamePlayers; i++)
		{
			if (!global::Players.Players.players[i].shooting)
			{
				continue;
			}
			for (byte b2 = 0; b2 < global::MainGame.MainGame.playerVehicles[i].numMounts; b2++)
			{
				byte objectID2 = global::MainGame.MainGame.playerVehicles[i].mounts[b2].objectID;
				if (global::MainGame.MainGame.playerVehicles[i].mounts[b2].type == 1 && global::MainGame.MainGame.playerVehicles[i].mounts[b2].objectAttached == 1 && global::MainGame.MainGame.playerVehicles[i].weapons[objectID2].shooting)
				{
					if (global::MainGame.MainGame.playerVehicles[i].weapons[objectID2].currentRounds < 1 && !wp1[global::MainGame.MainGame.playerVehicles[i].weapons[objectID2].weaponID].unLimitedAmmo && !global::MainGame.MainGame.unlimitedAmmo)
					{
						if (Player_Has_Ammo_For_Weapon((ushort)i, objectID2) < 1)
						{
							Add_Ammo_Clip(global::MainGame.MainGame.playerVehicles[i].weapons[objectID2].curClip, 1, (byte)i);
						}
						Load_Ammo_Clip_Into_Player_Vehicle_Weapon((ushort)i, objectID2, 1);
					}
					Fire_Bullet((short)i, b2, objectID2);
				}
			}
		}
		Process_Weapon_strikes();
		numActiveAmmoLights[uBufferID] = 0;
		i = 0;
		j = 0;
		for (; i < numLaserLights; i++)
		{
			laserLightsSorted[uBufferID, i] = -1;
		}
		showTargetCrosshairTimer -= global::MainGame.MainGame.frametime;
		if (showTargetCrosshairTimer < 0f)
		{
			showTargetCrosshairTimer = 0f;
		}
		b = 149;
		for (j = 0; j < 100; j++)
		{
			if (bulletActive[uBufferID, j] <= 0)
			{
				continue;
			}
			statCounter = 1;
			float radius = ammo[bullet[j].ammoIndex].radius;
			flag = false;
			ushort returnValueZoneCheckObjID;
			float distance2;
			float distanceHit;
			switch (bullet[j].ammoType)
			{
			case 0:
			case 1:
			case 14:
				if (bulletActive[uBufferID, j] == 1)
				{
					num2 = bullet[j].phys1.position.v[0];
					num3 = bullet[j].phys1.position.v[1];
					num4 = bullet[j].phys1.position.v[2];
					mainC.physicsMain.getPosition(ref bullet[j].phys1, global::MainGame.MainGame.frametime);
					float num14 = bullet[j].phys1.position.v[0];
					float num15 = bullet[j].phys1.position.v[1];
					float num16 = bullet[j].phys1.position.v[2];
					mainC.mapsMain.Set_Position_OutsideBoundary(ref bulletBoxT[threadID]);
					bulletBoxT[threadID].pos1.v[0] = num2;
					bulletBoxT[threadID].pos1.v[1] = num3;
					bulletBoxT[threadID].pos1.v[2] = num4;
					bulletBoxT[threadID].pos2.v[0] = bullet[j].phys1.position.v[0];
					bulletBoxT[threadID].pos2.v[1] = bullet[j].phys1.position.v[1];
					bulletBoxT[threadID].pos2.v[2] = bullet[j].phys1.position.v[2];
					num5 = mainC.playersMain.Check_Player_Impact_Threaded(ref bulletBoxT[threadID], ref pID, ref pfbV1T[threadID], ref pfbV2T[threadID], radius, bullet[j].playerID, threadID);
					if (num5 > -1f)
					{
						num14 = pfbV1T[threadID].v[0];
						num15 = pfbV1T[threadID].v[1];
						num16 = pfbV1T[threadID].v[2];
					}
					num7 = bullet[j].phys1.position.v[0] - num2;
					num8 = bullet[j].phys1.position.v[1] - num3;
					num9 = bullet[j].phys1.position.v[2] - num4;
					float num13 = (float)Math.Sqrt(num7 * num7 + num8 * num8 + num9 * num9);
					if (num13 != 0f)
					{
						num7 /= num13;
						num8 /= num13;
						num9 /= num13;
					}
					num6 = num13;
					i = 0;
					num = -1;
					short returnValueZoneCheckIndex = 0;
					InitialRayStart.X = num2;
					InitialRayStart.Y = num3;
					InitialRayStart.Z = num4;
					InitialRayEnd.X = bullet[j].phys1.position.v[0];
					InitialRayEnd.Y = bullet[j].phys1.position.v[1];
					InitialRayEnd.Z = bullet[j].phys1.position.v[2];
					while (mainC.zonesMain.Check_Zones_For_Ray(num2, num3, num4, InitialRayEnd.X, InitialRayEnd.Y, InitialRayEnd.Z, 5f, returnValueZoneCheckIndex, 1, out returnValueZoneCheckIndex, out returnValueZoneCheckObjID))
					{
						int num11 = Zones.zones[returnValueZoneCheckObjID].zoneList.numObjects;
						for (int num10 = 0; num10 < num11; num10++)
						{
							if (mainC.collisionMain.Check_Polygon_Ray_Collision_Projectile(Zones.zones[returnValueZoneCheckObjID].zoneList.oList[num10], num, ref InitialRayStart, ref InitialRayEnd, ref Zones.zones[returnValueZoneCheckObjID].zoneList.matrixList[num10], out distance, out IntersectPosition, out IntersectNormal, out num, threadID) && (i == 0 || distance < num6) && (num5 == -1f || distance < num5))
							{
								num7 = IntersectNormal.X;
								num8 = IntersectNormal.Y;
								num9 = IntersectNormal.Z;
								num14 = IntersectPosition.X;
								num15 = IntersectPosition.Y;
								num16 = IntersectPosition.Z;
								num6 = distance;
								objectID = Zones.zones[returnValueZoneCheckObjID].zoneList.gidList[num10];
								i = 8;
							}
						}
					}
					if (i == 8)
					{
						flag = true;
						Handle_Ballistic_Impact((ushort)bullet[j].playerID, (ushort)j, objectID, bullet[j].ammoIndex, threadID);
						mainC.soundsMain.Play_Sound("BulletImpact_Metal", num14, num15, num16, 0f, 0f, 0f);
						mainC.renderingMain.Add_Particle(ammo[bullet[j].ammoIndex].particleID2, num14, num15, num16, num7, num8, num9, 0f, 0f, 0f);
						bulletActive[uBufferID, j] = 0;
					}
					else if (num5 >= 0f)
					{
						global::Players.Players.players[pID].impactX += pfbV2T[threadID].v[0] * ammo[bullet[j].ammoIndex].mass * Math.Abs(bullet[j].phys1.velocity.v[0]);
						global::Players.Players.players[pID].impactY += pfbV2T[threadID].v[1] * ammo[bullet[j].ammoIndex].mass * Math.Abs(bullet[j].phys1.velocity.v[1]);
						global::Players.Players.players[pID].impactZ += pfbV2T[threadID].v[2] * ammo[bullet[j].ammoIndex].mass * Math.Abs(bullet[j].phys1.velocity.v[2]);
						flag = true;
						mainC.soundsMain.Play_Sound("Player_Hit", global::Players.Players.players[pID].charP.position.v[0], global::Players.Players.players[pID].charP.position.v[1], global::Players.Players.players[pID].charP.position.v[2], global::Players.Players.players[pID].charP.velocity.v[0], global::Players.Players.players[pID].charP.velocity.v[1], global::Players.Players.players[pID].charP.velocity.v[2]);
						mainC.playersMain.Player_Hit(pID, bullet[j].playerID, global::Players.Players.players[pID].jointWasShot, ammo[bullet[j].ammoIndex].damage[global::Players.Players.players[pID].damageType], (ushort)j, pfbV2T[threadID], threadID);
						bulletActive[uBufferID, j] = 0;
						if (!global::Players.Players.players[pID].dead && global::Players.Players.playerRaces[global::Players.Players.players[pID].race].numBulletImpactAnimations > 0)
						{
							mainC.programsMain.Start_Animation((ushort)pID, ref global::Players.Players.players[pID].jt1, ref global::Players.Players.players[pID].animations, global::Players.Players.players[pID].programCollection, global::Players.Players.playerRaces[global::Players.Players.players[pID].race].programBulletHit[global::Players.Players.players[pID].type, global::Players.Players.players[pID].curBulletHit], 1f, 1f);
							global::Players.Players.players[pID].curBulletHit = (byte)((global::Players.Players.players[pID].curBulletHit + 1) % global::Players.Players.playerRaces[global::Players.Players.players[pID].race].numBulletImpactAnimations);
						}
						mainC.renderingMain.Add_Particle(ammo[bullet[j].ammoIndex].particleID, num14, num15, num16, num7, num8, num9, global::Players.Players.players[pID].charP.velocity.v[0], global::Players.Players.players[pID].charP.velocity.v[1], global::Players.Players.players[pID].charP.velocity.v[2]);
						if (bullet[j].playerID == 0 && (global::Players.Players.players[pID].teamMask & global::Players.Players.enemyTeamMask) != 0)
						{
							showTargetCrosshairTimer = 0.25f;
						}
					}
					else if (i == 12)
					{
						bulletActive[uBufferID, j] = 0;
					}
					else
					{
						distance2 = mainC.terrainMain.Get_Terrain_Height(bullet[j].phys1.position.v[0], bullet[j].phys1.position.v[1], threadID);
						if (bullet[j].phys1.position.v[0] > global::MainGame.MainGame.MaxRight || bullet[j].phys1.position.v[0] < global::MainGame.MainGame.MaxLeft || bullet[j].phys1.position.v[1] > global::MainGame.MainGame.MaxForward || bullet[j].phys1.position.v[1] < global::MainGame.MainGame.MaxRear || bullet[j].phys1.position.v[2] < distance2 || bullet[j].phys1.position.v[2] > global::MainGame.MainGame.MaxUp)
						{
							bulletActive[uBufferID, j] = 0;
						}
						else if (!mainC.collisionMain.Position_InsideBoundary(ref bullet[j].phys1))
						{
							bulletActive[uBufferID, j] = 2;
						}
					}
					if (bullet[j].ammoType == 14)
					{
						bullet[j].startX[uBufferID] = num2;
						bullet[j].startY[uBufferID] = num3;
						bullet[j].startZ[uBufferID] = num4;
						bullet[j].endX[uBufferID] = num14;
						bullet[j].endY[uBufferID] = num15;
						bullet[j].endZ[uBufferID] = num16;
						Render_Rail_Gun_Particle_Stream(j, uBufferID);
					}
				}
				else if (bulletActive[uBufferID, j] == 2)
				{
					mainC.physicsMain.getPosition(ref bullet[j].phys1, global::MainGame.MainGame.frametime);
					distance2 = mainC.terrainMain.Get_Terrain_Height(bullet[j].phys1.position.v[0], bullet[j].phys1.position.v[1], threadID);
					if (bullet[j].phys1.position.v[0] > global::MainGame.MainGame.MaxRight || bullet[j].phys1.position.v[0] < global::MainGame.MainGame.MaxLeft || bullet[j].phys1.position.v[1] > global::MainGame.MainGame.MaxForward || bullet[j].phys1.position.v[1] < global::MainGame.MainGame.MaxRear || bullet[j].phys1.position.v[2] < distance2 || bullet[j].phys1.position.v[2] > global::MainGame.MainGame.MaxUp)
					{
						bulletActive[uBufferID, j] = 0;
					}
				}
				else
				{
					bulletActive[uBufferID, j] = 0;
				}
				break;
			case 2:
				if (bulletActive[uBufferID, j] == 1)
				{
					bullet[j].timer -= global::MainGame.MainGame.frametime;
					num2 = (bulletBoxT[threadID].pos1.v[0] = bullet[j].phys1.position.v[0]);
					num3 = (bulletBoxT[threadID].pos1.v[1] = bullet[j].phys1.position.v[1]);
					num4 = (bulletBoxT[threadID].pos1.v[2] = bullet[j].phys1.position.v[2]);
					mainC.physicsMain.getPosition(ref bullet[j].phys1, global::MainGame.MainGame.frametime);
					bulletBoxT[threadID].pos2.v[0] = bullet[j].phys1.position.v[0];
					bulletBoxT[threadID].pos2.v[1] = bullet[j].phys1.position.v[1];
					bulletBoxT[threadID].pos2.v[2] = bullet[j].phys1.position.v[2];
					mainC.mapsMain.Set_Position_OutsideBoundary(ref bulletBoxT[threadID]);
					num5 = mainC.playersMain.Check_Player_Impact_Threaded(ref bulletBoxT[threadID], ref pID, ref pfbV1T[threadID], ref pfbV2T[threadID], radius, bullet[j].playerID, threadID);
					distance2 = bullet[j].phys1.position.v[0] - num2;
					distanceHit = bullet[j].phys1.position.v[1] - num3;
					float num12 = bullet[j].phys1.position.v[2] - num4;
					num6 = (float)Math.Sqrt(distance2 * distance2 + distanceHit * distanceHit + num12 * num12);
					i = 0;
					num = -1;
					short returnValueZoneCheckIndex = 0;
					InitialRayStart.X = num2;
					InitialRayStart.Y = num3;
					InitialRayStart.Z = num4;
					InitialRayEnd.X = bullet[j].phys1.position.v[0];
					InitialRayEnd.Y = bullet[j].phys1.position.v[1];
					InitialRayEnd.Z = bullet[j].phys1.position.v[2];
					while (mainC.zonesMain.Check_Zones_For_Ray(num2, num3, num4, InitialRayEnd.X, InitialRayEnd.Y, InitialRayEnd.Z, 5f, returnValueZoneCheckIndex, 1, out returnValueZoneCheckIndex, out returnValueZoneCheckObjID))
					{
						int num11 = Zones.zones[returnValueZoneCheckObjID].zoneList.numObjects;
						for (int num10 = 0; num10 < num11; num10++)
						{
							if (mainC.collisionMain.Check_Polygon_Ray_Collision_Projectile(Zones.zones[returnValueZoneCheckObjID].zoneList.oList[num10], num, ref InitialRayStart, ref InitialRayEnd, ref Zones.zones[returnValueZoneCheckObjID].zoneList.matrixList[num10], out distance, out IntersectPosition, out IntersectNormal, out num, threadID) && (i == 0 || distance < num6) && (num5 == -1f || distance < num5))
							{
								bulletBoxT[threadID].pos1.v[0] = IntersectPosition.X + IntersectNormal.X;
								bulletBoxT[threadID].pos1.v[1] = IntersectPosition.Y + IntersectNormal.Y;
								bulletBoxT[threadID].pos1.v[2] = IntersectPosition.Z + IntersectNormal.Z;
								bulletBoxT[threadID].pos2.v[0] = bulletBoxT[threadID].pos1.v[0];
								bulletBoxT[threadID].pos2.v[1] = bulletBoxT[threadID].pos1.v[1];
								bulletBoxT[threadID].pos2.v[2] = bulletBoxT[threadID].pos1.v[2];
								num6 = distance;
								objectID = Zones.zones[returnValueZoneCheckObjID].zoneList.gidList[num10];
								i = 8;
							}
						}
					}
					if (i == 8)
					{
						flag = true;
						if (num6 < num5 || num5 < 0f)
						{
							bulletBoxT[threadID].pos1.v[0] = 0f - global::Collision.Collision.ccsVxT[threadID];
							bulletBoxT[threadID].pos1.v[1] = 0f - global::Collision.Collision.ccsVyT[threadID];
							bulletBoxT[threadID].pos1.v[2] = 0f - global::Collision.Collision.ccsVzT[threadID];
							Handle_Ballistic_Impact((ushort)bullet[j].playerID, (ushort)j, objectID, bullet[j].ammoIndex, threadID);
							mainC.soundsMain.Play_Sound("BulletImpact_Metal", bulletBoxT[threadID].pos2.v[0], bulletBoxT[threadID].pos2.v[1], bulletBoxT[threadID].pos2.v[2], 0f, 0f, 0f);
							mainC.renderingMain.New_Particle(2, ref bulletBoxT[threadID].pos2, ref bullet[j].phys1.velocity, ref bullet[j].phys1.velocity, ref bullet[j].phys1.velocity, 0, threadID);
						}
						else if (pID > -1)
						{
							mainC.soundsMain.Play_Sound("BulletImpact_Metal", global::Players.Players.players[pID].charP.position.v[0], global::Players.Players.players[pID].charP.position.v[1], global::Players.Players.players[pID].charP.position.v[2], global::Players.Players.players[pID].charP.velocity.v[0], global::Players.Players.players[pID].charP.velocity.v[1], global::Players.Players.players[pID].charP.velocity.v[2]);
							mainC.playersMain.Player_Hit(pID, bullet[j].playerID, global::Players.Players.players[pID].jointWasShot, ammo[bullet[j].ammoIndex].damage[global::Players.Players.players[pID].damageType], (ushort)j, pfbV2T[threadID], threadID);
							num5 -= 0.3f;
							bulletBoxT[threadID].pos1.v[0] = pfbV1T[threadID].v[0];
							bulletBoxT[threadID].pos1.v[1] = pfbV1T[threadID].v[1];
							bulletBoxT[threadID].pos1.v[2] = pfbV1T[threadID].v[2];
							bulletBoxT[threadID].pos2.v[0] = 0f - bullet[j].phys1.velocity.v[0];
							bulletBoxT[threadID].pos2.v[1] = 0f - bullet[j].phys1.velocity.v[1];
							bulletBoxT[threadID].pos2.v[2] = 0f - bullet[j].phys1.velocity.v[2];
							mainC.renderingMain.New_Particle(global::Players.Players.playerRaces[global::Players.Players.players[pID].race].particleEffect[global::Players.Players.players[pID].type], ref bulletBoxT[threadID].pos1, ref bulletBoxT[threadID].pos2, ref bullet[j].phys1.angle, ref bullet[j].phys1.velocity, pID, threadID);
							if (bullet[j].playerID == 0 && (global::Players.Players.players[pID].teamMask & global::Players.Players.enemyTeamMask) != 0)
							{
								showTargetCrosshairTimer = 0.25f;
							}
						}
						bulletActive[uBufferID, j] = 0;
					}
					else if (num5 >= 0f)
					{
						flag = true;
						mainC.soundsMain.Play_Sound("BulletImpact_Metal", global::Players.Players.players[pID].charP.position.v[0], global::Players.Players.players[pID].charP.position.v[1], global::Players.Players.players[pID].charP.position.v[2], global::Players.Players.players[pID].charP.velocity.v[0], global::Players.Players.players[pID].charP.velocity.v[1], global::Players.Players.players[pID].charP.velocity.v[2]);
						mainC.playersMain.Player_Hit(pID, bullet[j].playerID, global::Players.Players.players[pID].jointWasShot, ammo[bullet[j].ammoIndex].damage[global::Players.Players.players[pID].damageType], (ushort)j, pfbV2T[threadID], threadID);
						bulletActive[uBufferID, j] = 0;
						num5 -= 0.3f;
						bulletBoxT[threadID].pos1.v[0] = pfbV1T[threadID].v[0];
						bulletBoxT[threadID].pos1.v[1] = pfbV1T[threadID].v[1];
						bulletBoxT[threadID].pos1.v[2] = pfbV1T[threadID].v[2];
						bulletBoxT[threadID].pos2.v[0] = 0f - pfbV2T[threadID].v[0];
						bulletBoxT[threadID].pos2.v[1] = 0f - pfbV2T[threadID].v[1];
						bulletBoxT[threadID].pos2.v[2] = 0f - pfbV2T[threadID].v[2];
						global::Rendering.Rendering.newParticle[threadID, 0] = ammo[bullet[j].ammoIndex].colorE[0];
						global::Rendering.Rendering.newParticle[threadID, 1] = ammo[bullet[j].ammoIndex].colorE[1];
						global::Rendering.Rendering.newParticle[threadID, 2] = ammo[bullet[j].ammoIndex].colorE[2];
						mainC.renderingMain.New_Particle(global::Players.Players.playerRaces[global::Players.Players.players[pID].race].particleEffect[global::Players.Players.players[pID].type], ref bulletBoxT[threadID].pos1, ref bulletBoxT[threadID].pos2, ref bullet[j].phys1.angle, ref bullet[j].phys1.velocity, pID, threadID);
						if (bullet[j].playerID == 0 && (global::Players.Players.players[pID].teamMask & global::Players.Players.enemyTeamMask) != 0)
						{
							showTargetCrosshairTimer = 0.25f;
						}
					}
					else if (i == 12)
					{
						bulletActive[uBufferID, j] = 0;
						mainC.renderingMain.New_Particle(2, ref bulletBoxT[threadID].pos2, ref bullet[j].phys1.velocity, ref bullet[j].phys1.velocity, ref bullet[j].phys1.velocity, 0, threadID);
					}
					else
					{
						distance2 = mainC.terrainMain.Get_Terrain_Height(bullet[j].phys1.position.v[0], bullet[j].phys1.position.v[1], threadID);
						if (bullet[j].phys1.position.v[0] > global::MainGame.MainGame.MaxRight || bullet[j].phys1.position.v[0] < global::MainGame.MainGame.MaxLeft || bullet[j].phys1.position.v[1] > global::MainGame.MainGame.MaxForward || bullet[j].phys1.position.v[1] < global::MainGame.MainGame.MaxRear || bullet[j].phys1.position.v[2] < distance2 || bullet[j].phys1.position.v[2] > global::MainGame.MainGame.MaxUp)
						{
							bulletActive[uBufferID, j] = 0;
						}
						else if (!mainC.collisionMain.Position_InsideBoundary(ref bullet[j].phys1))
						{
							bulletActive[uBufferID, j] = 2;
						}
					}
				}
				else if (bulletActive[uBufferID, j] == 2)
				{
					mainC.physicsMain.getPosition(ref bullet[j].phys1, global::MainGame.MainGame.frametime);
					distance2 = mainC.terrainMain.Get_Terrain_Height(bullet[j].phys1.position.v[0], bullet[j].phys1.position.v[1], threadID);
					if (bullet[j].phys1.position.v[0] > global::MainGame.MainGame.MaxRight || bullet[j].phys1.position.v[0] < global::MainGame.MainGame.MaxLeft || bullet[j].phys1.position.v[1] > global::MainGame.MainGame.MaxForward || bullet[j].phys1.position.v[1] < global::MainGame.MainGame.MaxRear || bullet[j].phys1.position.v[2] < distance2 || bullet[j].phys1.position.v[2] > global::MainGame.MainGame.MaxUp)
					{
						bulletActive[uBufferID, j] = 0;
					}
				}
				else
				{
					bulletActive[uBufferID, j] = 0;
				}
				if (bullet[j].timer < 0f)
				{
					bulletActive[uBufferID, j] = 0;
				}
				if (bulletActive[uBufferID, j] == 1 && bullet[j].lightID > -1)
				{
					i = bullet[j].lightID;
					int ammoIndex = bullet[j].ammoIndex;
					ammoLightPos[uBufferID, i].X = bullet[j].phys1.position.v[0];
					ammoLightPos[uBufferID, i].Y = bullet[j].phys1.position.v[1];
					ammoLightPos[uBufferID, i].Z = bullet[j].phys1.position.v[2];
					ammoLightPos[uBufferID, i].W = ammo[ammoIndex].colorIntensity;
					ammoLightColor[uBufferID, i].X = ammo[ammoIndex].lightColor[0];
					ammoLightColor[uBufferID, i].Y = ammo[ammoIndex].lightColor[1];
					ammoLightColor[uBufferID, i].Z = ammo[ammoIndex].lightColor[2];
					ammoLightColor[uBufferID, i].W = ammo[ammoIndex].lightColor[3];
				}
				else if (bullet[j].lightID > -1)
				{
					laserLights[bullet[j].lightID] = -1;
					bullet[j].lightID = -1;
				}
				break;
			case 3:
			{
				bool flag2 = false;
				if (bulletActive[uBufferID, j] == 1)
				{
					num2 = (bulletBoxT[threadID].pos1.v[0] = bullet[j].phys1.position.v[0]);
					num3 = (bulletBoxT[threadID].pos1.v[1] = bullet[j].phys1.position.v[1]);
					num4 = (bulletBoxT[threadID].pos1.v[2] = bullet[j].phys1.position.v[2]);
					mainC.physicsMain.getPosition(ref bullet[j].phys1, global::MainGame.MainGame.frametime);
					bulletBoxT[threadID].pos2.v[0] = bullet[j].phys1.position.v[0];
					bulletBoxT[threadID].pos2.v[1] = bullet[j].phys1.position.v[1];
					bulletBoxT[threadID].pos2.v[2] = ammo[bullet[j].ammoIndex].length;
					if (bullet[j].playerID == 0)
					{
						mainC.soundsMain.Update_Priority_Sound_Position(bullet[j].soundID, bullet[j].phys1.position.v[0], bullet[j].phys1.position.v[1], bullet[j].phys1.position.v[2]);
					}
					else
					{
						mainC.soundsMain.Update_Sound_Position(bullet[j].soundID, bullet[j].phys1.position.v[0], bullet[j].phys1.position.v[1], bullet[j].phys1.position.v[2]);
					}
					bullet[j].particleTimer -= global::MainGame.MainGame.frametime / global::Physics.Physics.timeMod;
					if (bullet[j].particleTimer < 0f)
					{
						mainC.renderingMain.New_Particle(6, ref bulletBoxT[threadID].pos1, ref bulletBoxT[threadID].pos2, ref bullet[j].phys1.velocity, ref bullet[j].phys1.velocity, 0, threadID);
						bullet[j].particleTimer = ammo[bullet[j].ammoIndex].particleTimer;
					}
					bulletBoxT[threadID].pos2.v[2] = bullet[j].phys1.position.v[2];
					mainC.mapsMain.Set_Position_OutsideBoundary(ref bulletBoxT[threadID]);
					num5 = mainC.playersMain.Check_Player_Impact_Threaded(ref bulletBoxT[threadID], ref pID, ref pfbV1T[threadID], ref pfbV2T[threadID], radius, bullet[j].playerID, threadID);
					distance2 = bullet[j].phys1.position.v[0] - num2;
					distanceHit = bullet[j].phys1.position.v[1] - num3;
					float num12 = bullet[j].phys1.position.v[2] - num4;
					num6 = (float)Math.Sqrt(distance2 * distance2 + distanceHit * distanceHit + num12 * num12);
					i = 0;
					num = -1;
					short returnValueZoneCheckIndex = 0;
					InitialRayStart.X = num2;
					InitialRayStart.Y = num3;
					InitialRayStart.Z = num4;
					InitialRayEnd.X = bullet[j].phys1.position.v[0];
					InitialRayEnd.Y = bullet[j].phys1.position.v[1];
					InitialRayEnd.Z = bullet[j].phys1.position.v[2];
					while (mainC.zonesMain.Check_Zones_For_Point(num2, num3, num4, 5f, returnValueZoneCheckIndex, 1, out returnValueZoneCheckIndex, out returnValueZoneCheckObjID))
					{
						int num11 = Zones.zones[returnValueZoneCheckObjID].zoneList.numObjects;
						for (int num10 = 0; num10 < num11; num10++)
						{
							if (mainC.collisionMain.Check_Polygon_Ray_Collision_Projectile(Zones.zones[returnValueZoneCheckObjID].zoneList.oList[num10], num, ref InitialRayStart, ref InitialRayEnd, ref Zones.zones[returnValueZoneCheckObjID].zoneList.matrixList[num10], out distance, out IntersectPosition, out IntersectNormal, out num, threadID) && (i == 0 || distance < num6) && (num5 == -1f || distance < num5))
							{
								bulletBoxT[threadID].pos1.v[0] = IntersectPosition.X + IntersectNormal.X;
								bulletBoxT[threadID].pos1.v[1] = IntersectPosition.Y + IntersectNormal.Y;
								bulletBoxT[threadID].pos1.v[2] = IntersectPosition.Z + IntersectNormal.Z;
								bulletBoxT[threadID].pos2.v[0] = bulletBoxT[threadID].pos1.v[0];
								bulletBoxT[threadID].pos2.v[1] = bulletBoxT[threadID].pos1.v[1];
								bulletBoxT[threadID].pos2.v[2] = bulletBoxT[threadID].pos1.v[2];
								num6 = distance;
								objectID = Zones.zones[returnValueZoneCheckObjID].zoneList.gidList[num10];
								i = 8;
							}
						}
					}
					if (i == 8)
					{
						flag = true;
						mainC.soundsMain.Play_Priority_Sound("MS_Explo4_Grenade.wav", bullet[j].phys1.position.v[0], bullet[j].phys1.position.v[1], bullet[j].phys1.position.v[2], 0f, 0f, 0f);
						Splash_Damage_From_Weapon(bulletBoxT[threadID].pos1.v[0], bulletBoxT[threadID].pos1.v[1], bulletBoxT[threadID].pos1.v[2], (short)j, bullet[j].playerID, bullet[j].ammoIndex, threadID);
						mainC.renderingMain.New_Particle_New(16, bulletBoxT[threadID].pos1.v[0], bulletBoxT[threadID].pos1.v[1], bulletBoxT[threadID].pos1.v[2], 0.5f, 0f, 0f, 0, threadID);
						if (num6 < num5 || num5 < 0f)
						{
							bulletBoxT[threadID].pos1.v[0] = 0f - global::Collision.Collision.ccsVxT[threadID];
							bulletBoxT[threadID].pos1.v[1] = 0f - global::Collision.Collision.ccsVyT[threadID];
							bulletBoxT[threadID].pos1.v[2] = 0f - global::Collision.Collision.ccsVzT[threadID];
						}
						else if (pID > -1)
						{
							mainC.playersMain.Player_Hit(pID, bullet[j].playerID, global::Players.Players.players[pID].jointWasShot, ammo[bullet[j].ammoIndex].damage[global::Players.Players.players[pID].damageType], (ushort)j, pfbV2T[threadID], threadID);
							num5 -= 0.3f;
							bulletBoxT[threadID].pos1.v[0] = pfbV1T[threadID].v[0];
							bulletBoxT[threadID].pos1.v[1] = pfbV1T[threadID].v[1];
							bulletBoxT[threadID].pos1.v[2] = pfbV1T[threadID].v[2];
							bulletBoxT[threadID].pos2.v[0] = 0f - pfbV2T[threadID].v[0];
							bulletBoxT[threadID].pos2.v[1] = 0f - pfbV2T[threadID].v[1];
							bulletBoxT[threadID].pos2.v[2] = 0f - pfbV2T[threadID].v[2];
							if (bullet[j].playerID == 0 && (global::Players.Players.players[pID].teamMask & global::Players.Players.enemyTeamMask) != 0)
							{
								showTargetCrosshairTimer = 0.25f;
							}
							mainC.renderingMain.New_Particle_New(16, bulletBoxT[threadID].pos1.v[0], bulletBoxT[threadID].pos1.v[1], bulletBoxT[threadID].pos1.v[2], 0.5f, 0f, 0f, 0, threadID);
						}
						flag2 = true;
						bulletActive[uBufferID, j] = 0;
					}
					else if (num5 >= 0f)
					{
						flag = true;
						mainC.soundsMain.Play_Priority_Sound("MS_Explo4_Grenade.wav", bullet[j].phys1.position.v[0], bullet[j].phys1.position.v[1], bullet[j].phys1.position.v[2], 0f, 0f, 0f);
						Splash_Damage_From_Weapon(bulletBoxT[threadID].pos1.v[0], bulletBoxT[threadID].pos1.v[1], bulletBoxT[threadID].pos1.v[2], (short)j, bullet[j].playerID, bullet[j].ammoIndex, threadID);
						bulletActive[uBufferID, j] = 0;
						num5 -= 0.3f;
						bulletBoxT[threadID].pos1.v[0] = pfbV1T[threadID].v[0];
						bulletBoxT[threadID].pos1.v[1] = pfbV1T[threadID].v[1];
						bulletBoxT[threadID].pos1.v[2] = pfbV1T[threadID].v[2];
						bulletBoxT[threadID].pos2.v[0] = 0f - pfbV2T[threadID].v[0];
						bulletBoxT[threadID].pos2.v[1] = 0f - pfbV2T[threadID].v[1];
						bulletBoxT[threadID].pos2.v[2] = 0f - pfbV2T[threadID].v[2];
						if (bullet[j].playerID == 0 && (global::Players.Players.players[pID].teamMask & global::Players.Players.enemyTeamMask) != 0)
						{
							showTargetCrosshairTimer = 0.25f;
						}
						flag2 = true;
					}
					else if (i == 12)
					{
						Splash_Damage_From_Weapon(bulletBoxT[threadID].pos1.v[0], bulletBoxT[threadID].pos1.v[1], bulletBoxT[threadID].pos1.v[2], (short)j, bullet[j].playerID, bullet[j].ammoIndex, threadID);
						bulletActive[uBufferID, j] = 0;
						mainC.renderingMain.New_Particle(2, ref bulletBoxT[threadID].pos2, ref bullet[j].phys1.velocity, ref bullet[j].phys1.velocity, ref bullet[j].phys1.velocity, 0, threadID);
						if (bullet[j].soundID > -1)
						{
							if (bullet[j].playerID == 0)
							{
								mainC.soundsMain.Stop_Priority_Sound(bullet[j].soundID);
							}
							else
							{
								mainC.soundsMain.Stop_Sound(bullet[j].soundID);
							}
						}
						mainC.renderingMain.New_Particle_New(16, bulletBoxT[threadID].pos1.v[0], bulletBoxT[threadID].pos1.v[1], bulletBoxT[threadID].pos1.v[2], 0.5f, 0f, 0f, 0, threadID);
						flag2 = true;
					}
					else
					{
						distance2 = mainC.terrainMain.Get_Terrain_Height(bullet[j].phys1.position.v[0], bullet[j].phys1.position.v[1], threadID);
						if (bullet[j].phys1.position.v[0] > global::MainGame.MainGame.MaxRight || bullet[j].phys1.position.v[0] < global::MainGame.MainGame.MaxLeft || bullet[j].phys1.position.v[1] > global::MainGame.MainGame.MaxForward || bullet[j].phys1.position.v[1] < global::MainGame.MainGame.MaxRear || bullet[j].phys1.position.v[2] < distance2 || bullet[j].phys1.position.v[2] > global::MainGame.MainGame.MaxUp)
						{
							bulletActive[uBufferID, j] = 0;
							if (bullet[j].phys1.position.v[2] < distance2)
							{
								flag2 = true;
							}
						}
						else if (!mainC.collisionMain.Position_InsideBoundary(ref bullet[j].phys1))
						{
							bulletActive[uBufferID, j] = 2;
						}
					}
				}
				else if (bulletActive[uBufferID, j] == 2)
				{
					mainC.physicsMain.getPosition(ref bullet[j].phys1, global::MainGame.MainGame.frametime);
					distance2 = mainC.terrainMain.Get_Terrain_Height(bullet[j].phys1.position.v[0], bullet[j].phys1.position.v[1], threadID);
					if (bullet[j].phys1.position.v[0] > global::MainGame.MainGame.MaxRight || bullet[j].phys1.position.v[0] < global::MainGame.MainGame.MaxLeft || bullet[j].phys1.position.v[1] > global::MainGame.MainGame.MaxForward || bullet[j].phys1.position.v[1] < global::MainGame.MainGame.MaxRear || bullet[j].phys1.position.v[2] < distance2 || bullet[j].phys1.position.v[2] > global::MainGame.MainGame.MaxUp)
					{
						bulletActive[uBufferID, j] = 0;
						if (bullet[j].phys1.position.v[2] < distance2)
						{
							flag2 = true;
						}
					}
				}
				else
				{
					bulletActive[uBufferID, j] = 0;
				}
				if (bulletActive[uBufferID, j] == 1)
				{
					bullet[j].timer -= global::MainGame.MainGame.frametime / global::Physics.Physics.timeMod;
					if (bullet[j].timer < 0f)
					{
						mainC.soundsMain.Play_Priority_Sound("MS_Explo4_Grenade.wav", bullet[j].phys1.position.v[0], bullet[j].phys1.position.v[1], bullet[j].phys1.position.v[2], 0f, 0f, 0f);
						Splash_Damage_From_Weapon(bulletBoxT[threadID].pos1.v[0], bulletBoxT[threadID].pos1.v[1], bulletBoxT[threadID].pos1.v[2], (short)j, bullet[j].playerID, bullet[j].ammoIndex, threadID);
						apffwV1.v[0] = bullet[j].phys1.position.v[0];
						apffwV1.v[1] = bullet[j].phys1.position.v[1];
						apffwV1.v[2] = bullet[j].phys1.position.v[2];
						mainC.renderingMain.New_Particle_New(16, bulletBoxT[threadID].pos1.v[0], bulletBoxT[threadID].pos1.v[1], bulletBoxT[threadID].pos1.v[2], 0.5f, 0f, 0f, 0, threadID);
						bulletActive[uBufferID, j] = 0;
						break;
					}
					if (bullet[j].lightID > -1)
					{
						i = bullet[j].lightID;
						int ammoIndex = bullet[j].ammoIndex;
						ammoLightPos[uBufferID, i].X = bullet[j].phys1.position.v[0];
						ammoLightPos[uBufferID, i].Y = bullet[j].phys1.position.v[1];
						ammoLightPos[uBufferID, i].Z = bullet[j].phys1.position.v[2];
						ammoLightPos[uBufferID, i].W = ammo[ammoIndex].colorIntensity;
						ammoLightColor[uBufferID, i].X = ammo[ammoIndex].lightColor[0];
						ammoLightColor[uBufferID, i].Y = ammo[ammoIndex].lightColor[1];
						ammoLightColor[uBufferID, i].Z = ammo[ammoIndex].lightColor[2];
						ammoLightColor[uBufferID, i].W = ammo[ammoIndex].lightColor[3];
					}
				}
				else if (bullet[j].lightID > -1)
				{
					laserLights[bullet[j].lightID] = -1;
					bullet[j].lightID = -1;
				}
				if (bulletActive[uBufferID, j] != 0)
				{
					break;
				}
				if (bullet[j].soundID > -1)
				{
					if (bullet[j].playerID == 0)
					{
						mainC.soundsMain.Stop_Priority_Sound(bullet[j].soundID);
					}
					else
					{
						mainC.soundsMain.Stop_Sound(bullet[j].soundID);
					}
				}
				if (flag2)
				{
					if (bullet[j].soundID2 > -1)
					{
						mainC.soundsMain.Play_Priority_Sound(ammo[bullet[j].soundID2].sound2, bulletBoxT[threadID].pos2.v[0], bulletBoxT[threadID].pos2.v[1], bulletBoxT[threadID].pos2.v[2], 0f, 0f, 0f);
					}
					apffwV1.v[0] = bulletBoxT[threadID].pos2.v[0];
					apffwV1.v[1] = bulletBoxT[threadID].pos2.v[1];
					apffwV1.v[2] = bulletBoxT[threadID].pos2.v[2];
					Splash_Damage_From_Weapon(bulletBoxT[threadID].pos1.v[0], bulletBoxT[threadID].pos1.v[1], bulletBoxT[threadID].pos1.v[2] + 20f, (short)j, bullet[j].playerID, bullet[j].ammoIndex, threadID);
					mainC.renderingMain.New_Particle_New(16, bulletBoxT[threadID].pos1.v[0], bulletBoxT[threadID].pos1.v[1], bulletBoxT[threadID].pos1.v[2], 0.5f, 0f, 0f, 0, threadID);
				}
				break;
			}
			case 4:
				if (bulletActive[uBufferID, j] == 1)
				{
					num2 = (bulletBoxT[threadID].pos1.v[0] = bullet[j].phys1.position.v[0]);
					num3 = (bulletBoxT[threadID].pos1.v[1] = bullet[j].phys1.position.v[1]);
					num4 = (bulletBoxT[threadID].pos1.v[2] = bullet[j].phys1.position.v[2]);
					mainC.physicsMain.getPosition(ref bullet[j].phys1, global::MainGame.MainGame.frametime);
					bulletBoxT[threadID].pos2.v[0] = bullet[j].phys1.position.v[0];
					bulletBoxT[threadID].pos2.v[1] = bullet[j].phys1.position.v[1];
					bulletBoxT[threadID].pos2.v[2] = bullet[j].phys1.position.v[2];
					mainC.mapsMain.Set_Position_OutsideBoundary(ref bulletBoxT[threadID]);
					num5 = mainC.playersMain.Check_Player_Impact_Threaded(ref bulletBoxT[threadID], ref pID, ref pfbV1T[threadID], ref pfbV2T[threadID], radius, bullet[j].playerID, threadID);
					distance2 = bullet[j].phys1.position.v[0] - num2;
					distanceHit = bullet[j].phys1.position.v[1] - num3;
					float num12 = bullet[j].phys1.position.v[2] - num4;
					num6 = (float)Math.Sqrt(distance2 * distance2 + distanceHit * distanceHit + num12 * num12);
					i = 0;
					num = -1;
					short returnValueZoneCheckIndex = 0;
					InitialRayStart.X = num2;
					InitialRayStart.Y = num3;
					InitialRayStart.Z = num4;
					InitialRayEnd.X = bullet[j].phys1.position.v[0];
					InitialRayEnd.Y = bullet[j].phys1.position.v[1];
					InitialRayEnd.Z = bullet[j].phys1.position.v[2];
					while (mainC.zonesMain.Check_Zones_For_Point(num2, num3, num4, 5f, returnValueZoneCheckIndex, 1, out returnValueZoneCheckIndex, out returnValueZoneCheckObjID))
					{
						int num11 = Zones.zones[returnValueZoneCheckObjID].zoneList.numObjects;
						for (int num10 = 0; num10 < num11; num10++)
						{
							if (mainC.collisionMain.Check_Polygon_Ray_Collision_Projectile(Zones.zones[returnValueZoneCheckObjID].zoneList.oList[num10], num, ref InitialRayStart, ref InitialRayEnd, ref Zones.zones[returnValueZoneCheckObjID].zoneList.matrixList[num10], out distance, out IntersectPosition, out IntersectNormal, out num, threadID) && (i == 0 || distance < num6) && (num5 == -1f || distance < num5))
							{
								bulletBoxT[threadID].pos1.v[0] = IntersectPosition.X;
								bulletBoxT[threadID].pos1.v[1] = IntersectPosition.Y;
								bulletBoxT[threadID].pos1.v[2] = IntersectPosition.Z;
								bulletBoxT[threadID].pos2.v[0] = bulletBoxT[threadID].pos1.v[0];
								bulletBoxT[threadID].pos2.v[1] = bulletBoxT[threadID].pos1.v[1];
								bulletBoxT[threadID].pos2.v[2] = bulletBoxT[threadID].pos1.v[2];
								num6 = distance;
								i = 8;
							}
						}
					}
					if (i == 8)
					{
						if (num6 < num5 || num5 < 0f)
						{
							distance2 = pfbV2T[threadID].v[0] * bullet[j].phys1.fx + pfbV2T[threadID].v[1] * bullet[j].phys1.fy + pfbV2T[threadID].v[2] * bullet[j].phys1.fz;
							if ((double)distance2 < 0.707)
							{
								distanceHit = (float)Math.Acos(distance2 * 0.999999f) * 57.29578f;
								if (distance2 >= 0f)
								{
									if (bullet[j].phys1.fz > pfbV2T[threadID].v[2])
									{
										bullet[j].rotation -= distanceHit - 45f;
									}
									else
									{
										bullet[j].rotation += distanceHit - 45f;
									}
								}
								else
								{
									if (0f - bullet[j].phys1.fz > pfbV2T[threadID].v[2])
									{
										bullet[j].rotation -= distanceHit - 45f;
									}
									else
									{
										bullet[j].rotation += distanceHit - 45f;
									}
									bullet[j].rotation += 180f;
								}
								Matrix matrix = Matrix.CreateRotationX((bullet[j].phys1.angle.v[0] + bullet[j].rotation) * ((float)Math.PI / 180f)) * Matrix.CreateRotationZ(bullet[j].phys1.angle.v[2] * ((float)Math.PI / 180f));
								bullet[j].phys1.fx = matrix.M21;
								bullet[j].phys1.fy = matrix.M22;
								bullet[j].phys1.fz = matrix.M23;
							}
							distanceHit = ammo[bullet[j].ammoIndex].length;
							num2 = (bullet[j].phys1.position.v[0] = bulletBoxT[threadID].pos1.v[0] - bullet[j].phys1.fx * distanceHit);
							num3 = (bullet[j].phys1.position.v[1] = bulletBoxT[threadID].pos1.v[1] - bullet[j].phys1.fy * distanceHit);
							num4 = (bullet[j].phys1.position.v[2] = bulletBoxT[threadID].pos1.v[2] - bullet[j].phys1.fz * distanceHit);
							bullet[j].phys1.angularVelocity.v[0] = 0f;
							bulletActive[uBufferID, j] = 4;
						}
						else if (pID > -1)
						{
							mainC.soundsMain.Play_Sound("MS_Body_Hits", global::Players.Players.players[pID].charP.position.v[0], global::Players.Players.players[pID].charP.position.v[1], global::Players.Players.players[pID].charP.position.v[2], 0f, 0f, 0f);
							mainC.playersMain.Player_Hit(pID, bullet[j].playerID, global::Players.Players.players[pID].jointWasShot, ammo[bullet[j].ammoIndex].damage[global::Players.Players.players[pID].damageType], (ushort)j, pfbV2T[threadID], threadID);
							num5 -= 0.3f;
							bulletBoxT[threadID].pos1.v[0] = pfbV1T[threadID].v[0];
							bulletBoxT[threadID].pos1.v[1] = pfbV1T[threadID].v[1];
							bulletBoxT[threadID].pos1.v[2] = pfbV1T[threadID].v[2];
							bulletBoxT[threadID].pos2.v[0] = 0f - pfbV2T[threadID].v[0];
							bulletBoxT[threadID].pos2.v[1] = 0f - pfbV2T[threadID].v[1];
							bulletBoxT[threadID].pos2.v[2] = 0f - pfbV2T[threadID].v[2];
							mainC.renderingMain.New_Particle(global::Players.Players.playerRaces[global::Players.Players.players[pID].race].particleEffect[global::Players.Players.players[pID].type], ref bulletBoxT[threadID].pos1, ref bulletBoxT[threadID].pos2, ref bullet[j].phys1.angle, ref bullet[j].phys1.velocity, pID, threadID);
							if (bullet[j].playerID == 0 && (global::Players.Players.players[pID].teamMask & global::Players.Players.enemyTeamMask) != 0)
							{
								showTargetCrosshairTimer = 0.25f;
							}
							bulletActive[uBufferID, j] = 0;
						}
					}
					else if (num5 >= 0f)
					{
						mainC.soundsMain.Play_Sound("MS_Body_Hits", global::Players.Players.players[pID].charP.position.v[0], global::Players.Players.players[pID].charP.position.v[1], global::Players.Players.players[pID].charP.position.v[2], 0f, 0f, 0f);
						mainC.playersMain.Player_Hit(pID, bullet[j].playerID, global::Players.Players.players[pID].jointWasShot, ammo[bullet[j].ammoIndex].damage[global::Players.Players.players[pID].damageType], (ushort)j, pfbV2T[threadID], threadID);
						bulletActive[uBufferID, j] = 0;
						num5 -= 0.3f;
						bulletBoxT[threadID].pos1.v[0] = pfbV1T[threadID].v[0];
						bulletBoxT[threadID].pos1.v[1] = pfbV1T[threadID].v[1];
						bulletBoxT[threadID].pos1.v[2] = pfbV1T[threadID].v[2];
						bulletBoxT[threadID].pos2.v[0] = 0f - pfbV2T[threadID].v[0];
						bulletBoxT[threadID].pos2.v[1] = 0f - pfbV2T[threadID].v[1];
						bulletBoxT[threadID].pos2.v[2] = 0f - pfbV2T[threadID].v[2];
						mainC.renderingMain.New_Particle(global::Players.Players.playerRaces[global::Players.Players.players[pID].race].particleEffect[global::Players.Players.players[pID].type], ref bulletBoxT[threadID].pos1, ref bulletBoxT[threadID].pos2, ref bullet[j].phys1.angle, ref bullet[j].phys1.velocity, pID, threadID);
						if (bullet[j].playerID == 0 && (global::Players.Players.players[pID].teamMask & global::Players.Players.enemyTeamMask) != 0)
						{
							showTargetCrosshairTimer = 0.25f;
						}
					}
					else if (i == 12)
					{
						bulletActive[uBufferID, j] = 0;
						mainC.renderingMain.New_Particle(2, ref bulletBoxT[threadID].pos2, ref bullet[j].phys1.velocity, ref bullet[j].phys1.velocity, ref bullet[j].phys1.velocity, 0, threadID);
					}
					else
					{
						distance2 = mainC.terrainMain.Get_Terrain_Height(bullet[j].phys1.position.v[0], bullet[j].phys1.position.v[1], threadID);
						if (bullet[j].phys1.position.v[0] > global::MainGame.MainGame.MaxRight || bullet[j].phys1.position.v[0] < global::MainGame.MainGame.MaxLeft || bullet[j].phys1.position.v[1] > global::MainGame.MainGame.MaxForward || bullet[j].phys1.position.v[1] < global::MainGame.MainGame.MaxRear || bullet[j].phys1.position.v[2] < distance2 || bullet[j].phys1.position.v[2] > global::MainGame.MainGame.MaxUp)
						{
							bulletActive[uBufferID, j] = 0;
						}
						else if (!mainC.collisionMain.Position_InsideBoundary(ref bullet[j].phys1))
						{
							bulletActive[uBufferID, j] = 2;
						}
					}
				}
				else if (bulletActive[uBufferID, j] == 2)
				{
					mainC.physicsMain.getPosition(ref bullet[j].phys1, global::MainGame.MainGame.frametime);
					distance2 = mainC.terrainMain.Get_Terrain_Height(bullet[j].phys1.position.v[0], bullet[j].phys1.position.v[1], threadID);
					if (bullet[j].phys1.position.v[0] > global::MainGame.MainGame.MaxRight || bullet[j].phys1.position.v[0] < global::MainGame.MainGame.MaxLeft || bullet[j].phys1.position.v[1] > global::MainGame.MainGame.MaxForward || bullet[j].phys1.position.v[1] < global::MainGame.MainGame.MaxRear || bullet[j].phys1.position.v[2] < distance2 || bullet[j].phys1.position.v[2] > global::MainGame.MainGame.MaxUp)
					{
						bulletActive[uBufferID, j] = 0;
					}
				}
				else if (bulletActive[uBufferID, j] == 4)
				{
					num2 = bullet[j].phys1.position.v[0];
					num3 = bullet[j].phys1.position.v[1];
					num4 = bullet[j].phys1.position.v[2];
				}
				else
				{
					bulletActive[uBufferID, j] = 0;
				}
				break;
			case 5:
				if (bullet[j].timer >= 0f)
				{
					if (bulletActive[uBufferID, j] == 1)
					{
						num2 = (bulletBoxT[threadID].pos1.v[0] = bullet[j].phys1.position.v[0]);
						num3 = (bulletBoxT[threadID].pos1.v[1] = bullet[j].phys1.position.v[1]);
						num4 = (bulletBoxT[threadID].pos1.v[2] = bullet[j].phys1.position.v[2]);
						mainC.physicsMain.getPosition(ref bullet[j].phys1, global::MainGame.MainGame.frametime);
						bulletBoxT[threadID].pos2.v[0] = bullet[j].phys1.position.v[0];
						bulletBoxT[threadID].pos2.v[1] = bullet[j].phys1.position.v[1];
						bulletBoxT[threadID].pos2.v[2] = bullet[j].phys1.position.v[2];
						mainC.mapsMain.Set_Position_OutsideBoundary(ref bulletBoxT[threadID]);
						num5 = mainC.playersMain.Check_Player_Impact_Threaded(ref bulletBoxT[threadID], ref pID, ref pfbV1T[threadID], ref pfbV2T[threadID], radius, bullet[j].playerID, threadID);
						distance2 = bullet[j].phys1.position.v[0] - num2;
						distanceHit = bullet[j].phys1.position.v[1] - num3;
						float num12 = bullet[j].phys1.position.v[2] - num4;
						num6 = (float)Math.Sqrt(distance2 * distance2 + distanceHit * distanceHit + num12 * num12);
						i = 0;
						num = -1;
						short returnValueZoneCheckIndex = 0;
						InitialRayStart.X = num2;
						InitialRayStart.Y = num3;
						InitialRayStart.Z = num4;
						InitialRayEnd.X = bullet[j].phys1.position.v[0];
						InitialRayEnd.Y = bullet[j].phys1.position.v[1];
						InitialRayEnd.Z = bullet[j].phys1.position.v[2];
						while (mainC.zonesMain.Check_Zones_For_Point(num2, num3, num4, 5f, returnValueZoneCheckIndex, 1, out returnValueZoneCheckIndex, out returnValueZoneCheckObjID))
						{
							int num11 = Zones.zones[returnValueZoneCheckObjID].zoneList.numObjects;
							for (int num10 = 0; num10 < num11; num10++)
							{
								if (mainC.collisionMain.Check_Polygon_Ray_Collision_Projectile(Zones.zones[returnValueZoneCheckObjID].zoneList.oList[num10], num, ref InitialRayStart, ref InitialRayEnd, ref Zones.zones[returnValueZoneCheckObjID].zoneList.matrixList[num10], out distance, out IntersectPosition, out IntersectNormal, out num, threadID) && (i == 0 || distance < num6) && (num5 == -1f || distance < num5))
								{
									bulletBoxT[threadID].pos1.v[0] = IntersectPosition.X + IntersectNormal.X * 0.001f;
									bulletBoxT[threadID].pos1.v[1] = IntersectPosition.Y + IntersectNormal.Y * 0.001f;
									bulletBoxT[threadID].pos1.v[2] = IntersectPosition.Z + IntersectNormal.Z * 0.001f;
									bulletBoxT[threadID].pos2.v[0] = bulletBoxT[threadID].pos1.v[0];
									bulletBoxT[threadID].pos2.v[1] = bulletBoxT[threadID].pos1.v[1];
									bulletBoxT[threadID].pos2.v[2] = bulletBoxT[threadID].pos1.v[2];
									num7 = IntersectNormal.X;
									num8 = IntersectNormal.Y;
									num9 = IntersectNormal.Z;
									num6 = distance;
									i = 8;
								}
							}
						}
						if (i == 8)
						{
							if (num6 < num5 || num5 < 0f)
							{
								float num13 = bullet[j].phys1.velocity.v[0] * bullet[j].phys1.velocity.v[0] + bullet[j].phys1.velocity.v[1] * bullet[j].phys1.velocity.v[1] + bullet[j].phys1.velocity.v[2] * bullet[j].phys1.velocity.v[2];
								num13 = -96f + num13 / 562500f * 90f;
								if (num13 > -90f)
								{
									mainC.soundsMain.Play_Sound_Volume("Clank", -96f + num13 / 562500f * 90f, bulletBoxT[threadID].pos1.v[0], bulletBoxT[threadID].pos1.v[1], bulletBoxT[threadID].pos1.v[2], 0f, 0f, 0f);
								}
								distance2 = bullet[j].phys1.velocity.v[0] * num7 + bullet[j].phys1.velocity.v[1] * num8 + bullet[j].phys1.velocity.v[2] * num9;
								if (distance2 < 0f)
								{
									distanceHit = 25.5f * global::MainGame.MainGame.frametime;
									if (distanceHit > 1f)
									{
										distanceHit = 1f;
									}
									num12 = (0f - distance2) * 0.25f;
									bullet[j].phys1.velocity.v[0] -= distance2 * num7;
									bullet[j].phys1.velocity.v[1] -= distance2 * num8;
									bullet[j].phys1.velocity.v[2] -= distance2 * num9;
									bullet[j].phys1.velocity.v[0] = bullet[j].phys1.velocity.v[0] - bullet[j].phys1.velocity.v[0] * distanceHit + num12 * num7;
									bullet[j].phys1.velocity.v[1] = bullet[j].phys1.velocity.v[1] - bullet[j].phys1.velocity.v[1] * distanceHit + num12 * num8;
									bullet[j].phys1.velocity.v[2] = bullet[j].phys1.velocity.v[2] - bullet[j].phys1.velocity.v[2] * distanceHit + num12 * num9;
								}
								bullet[j].phys1.position.v[0] = bulletBoxT[threadID].pos1.v[0];
								bullet[j].phys1.position.v[1] = bulletBoxT[threadID].pos1.v[1];
								bullet[j].phys1.position.v[2] = bulletBoxT[threadID].pos1.v[2];
							}
							else if (pID > -1)
							{
								mainC.soundsMain.Play_Sound("MS_Body_Hits", global::Players.Players.players[pID].charP.position.v[0], global::Players.Players.players[pID].charP.position.v[1], global::Players.Players.players[pID].charP.position.v[2], 0f, 0f, 0f);
								bullet[j].phys1.velocity.v[0] *= -0.3f;
								bullet[j].phys1.velocity.v[1] *= -0.3f;
								bullet[j].phys1.velocity.v[2] *= -0.3f;
							}
						}
						else if (num5 >= 0f)
						{
							mainC.soundsMain.Play_Sound("MS_Body_Hits", global::Players.Players.players[pID].charP.position.v[0], global::Players.Players.players[pID].charP.position.v[1], global::Players.Players.players[pID].charP.position.v[2], 0f, 0f, 0f);
							bullet[j].phys1.velocity.v[0] *= -0.3f;
							bullet[j].phys1.velocity.v[1] *= -0.3f;
							bullet[j].phys1.velocity.v[2] *= -0.3f;
						}
						else
						{
							distance2 = mainC.terrainMain.Get_Terrain_Height(bullet[j].phys1.position.v[0], bullet[j].phys1.position.v[1], threadID);
							if (bullet[j].phys1.position.v[0] > global::MainGame.MainGame.MaxRight || bullet[j].phys1.position.v[0] < global::MainGame.MainGame.MaxLeft || bullet[j].phys1.position.v[1] > global::MainGame.MainGame.MaxForward || bullet[j].phys1.position.v[1] < global::MainGame.MainGame.MaxRear || bullet[j].phys1.position.v[2] < distance2 - 1f || bullet[j].phys1.position.v[2] > global::MainGame.MainGame.MaxUp)
							{
								bulletActive[uBufferID, j] = 0;
							}
							else if (!mainC.collisionMain.Position_InsideBoundary(ref bullet[j].phys1))
							{
								bulletActive[uBufferID, j] = 2;
							}
						}
					}
					else if (bulletActive[uBufferID, j] == 2)
					{
						mainC.physicsMain.getPosition(ref bullet[j].phys1, global::MainGame.MainGame.frametime);
						distance2 = mainC.terrainMain.Get_Terrain_Height(bullet[j].phys1.position.v[0], bullet[j].phys1.position.v[1], threadID);
						if (bullet[j].phys1.position.v[0] > global::MainGame.MainGame.MaxRight || bullet[j].phys1.position.v[0] < global::MainGame.MainGame.MaxLeft || bullet[j].phys1.position.v[1] > global::MainGame.MainGame.MaxForward || bullet[j].phys1.position.v[1] < global::MainGame.MainGame.MaxRear || bullet[j].phys1.position.v[2] < distance2 || bullet[j].phys1.position.v[2] > global::MainGame.MainGame.MaxUp)
						{
							bulletActive[uBufferID, j] = 0;
						}
					}
					else
					{
						bulletActive[uBufferID, j] = 0;
					}
					bullet[j].timer -= global::MainGame.MainGame.frametime / global::Physics.Physics.timeMod;
					if (bullet[j].timer < 0f)
					{
						mainC.Explosions.New_Explosion(0, (ushort)bullet[j].playerID, bullet[j].phys1.position.v[0], bullet[j].phys1.position.v[1], bullet[j].phys1.position.v[2], threadID);
						bulletActive[uBufferID, j] = 0;
					}
				}
				else
				{
					bullet[j].timer += global::MainGame.MainGame.frametime / global::Physics.Physics.timeMod;
					if (bullet[j].timer >= 0f)
					{
						Release_Grenade((byte)j);
						global::Players.Players.players[bullet[j].playerID].renderWeapon = (byte)(global::Players.Players.players[bullet[j].playerID].renderWeapon | 1);
					}
				}
				break;
			case 6:
			{
				bool flag2 = false;
				if (bulletActive[uBufferID, j] == 1)
				{
					num2 = (bulletBoxT[threadID].pos1.v[0] = bullet[j].phys1.position.v[0]);
					num3 = (bulletBoxT[threadID].pos1.v[1] = bullet[j].phys1.position.v[1]);
					num4 = (bulletBoxT[threadID].pos1.v[2] = bullet[j].phys1.position.v[2]);
					mainC.physicsMain.getPosition(ref bullet[j].phys1, global::MainGame.MainGame.frametime / global::Physics.Physics.timeMod);
					bulletBoxT[threadID].pos2.v[0] = bullet[j].phys1.position.v[0];
					bulletBoxT[threadID].pos2.v[1] = bullet[j].phys1.position.v[1];
					bulletBoxT[threadID].pos2.v[2] = bullet[j].phys1.position.v[2];
					if (bullet[j].playerID == 0)
					{
						mainC.soundsMain.Update_Priority_Sound_Position(bullet[j].soundID, bullet[j].phys1.position.v[0], bullet[j].phys1.position.v[1], bullet[j].phys1.position.v[2]);
					}
					else
					{
						mainC.soundsMain.Update_Sound_Position(bullet[j].soundID, bullet[j].phys1.position.v[0], bullet[j].phys1.position.v[1], bullet[j].phys1.position.v[2]);
					}
					distance2 = (float)Math.Sqrt(bullet[j].phys1.velocity.v[0] * bullet[j].phys1.velocity.v[0] + bullet[j].phys1.velocity.v[1] * bullet[j].phys1.velocity.v[1] + bullet[j].phys1.velocity.v[2] * bullet[j].phys1.velocity.v[2]);
					bullet[j].phys1.totalVelocity = distance2;
					float num12;
					if (distance2 != 0f)
					{
						num7 = bullet[j].phys1.velocity.v[0] / distance2;
						num8 = bullet[j].phys1.velocity.v[1] / distance2;
						num9 = bullet[j].phys1.velocity.v[2] / distance2;
						num5 = distance2 * distance2 * 0.00017453f * global::MainGame.MainGame.frametime;
						distanceHit = num5 * (num7 * bullet[j].mv[uBufferID].M31 + num8 * bullet[j].mv[uBufferID].M32 + num9 * bullet[j].mv[uBufferID].M33);
						num12 = num5 * (num7 * bullet[j].mv[uBufferID].M11 + num8 * bullet[j].mv[uBufferID].M12 + num9 * bullet[j].mv[uBufferID].M13);
						bullet[j].rot *= Quaternion.CreateFromYawPitchRoll(0f, distanceHit, 0f - num12);
						Matrix.CreateFromQuaternion(ref bullet[j].rot, out bullet[j].mv[uBufferID]);
					}
					mainC.mapsMain.Set_Position_OutsideBoundary(ref bulletBoxT[threadID]);
					num5 = mainC.playersMain.Check_Player_Impact_Threaded(ref bulletBoxT[threadID], ref pID, ref pfbV1T[threadID], ref pfbV2T[threadID], radius, bullet[j].playerID, threadID);
					distance2 = bullet[j].phys1.position.v[0] - num2;
					distanceHit = bullet[j].phys1.position.v[1] - num3;
					num12 = bullet[j].phys1.position.v[2] - num4;
					num6 = (float)Math.Sqrt(distance2 * distance2 + distanceHit * distanceHit + num12 * num12);
					i = 0;
					num = -1;
					short returnValueZoneCheckIndex = 0;
					InitialRayStart.X = num2;
					InitialRayStart.Y = num3;
					InitialRayStart.Z = num4;
					InitialRayEnd.X = bullet[j].phys1.position.v[0];
					InitialRayEnd.Y = bullet[j].phys1.position.v[1];
					InitialRayEnd.Z = bullet[j].phys1.position.v[2];
					while (mainC.zonesMain.Check_Zones_For_Point(num2, num3, num4, 5f, returnValueZoneCheckIndex, 0, out returnValueZoneCheckIndex, out returnValueZoneCheckObjID))
					{
						int num11 = Zones.zones[returnValueZoneCheckObjID].zoneList.numObjects;
						for (int num10 = 0; num10 < num11; num10++)
						{
							if (mainC.collisionMain.Check_Polygon_Ray_Collision_Projectile(Zones.zones[returnValueZoneCheckObjID].zoneList.oList[num10], num, ref InitialRayStart, ref InitialRayEnd, ref Zones.zones[returnValueZoneCheckObjID].zoneList.matrixList[num10], out distance2, out IntersectPosition, out IntersectNormal, out num, threadID) && (i == 0 || distance < num6) && (num5 == -1f || distance < num5))
							{
								bulletBoxT[threadID].pos1.v[0] = IntersectPosition.X + IntersectNormal.X;
								bulletBoxT[threadID].pos1.v[1] = IntersectPosition.Y + IntersectNormal.Y;
								bulletBoxT[threadID].pos1.v[2] = IntersectPosition.Z + IntersectNormal.Z;
								bulletBoxT[threadID].pos2.v[0] = bulletBoxT[threadID].pos1.v[0];
								bulletBoxT[threadID].pos2.v[1] = bulletBoxT[threadID].pos1.v[1];
								bulletBoxT[threadID].pos2.v[2] = bulletBoxT[threadID].pos1.v[2];
								num6 = distance;
								i = 8;
							}
						}
					}
					if (i == 8)
					{
						flag = true;
						if (!(num6 < num5) && !(num5 < 0f) && pID > -1)
						{
							mainC.playersMain.Player_Hit(pID, bullet[j].playerID, global::Players.Players.players[pID].jointWasShot, ammo[bullet[j].ammoIndex].damage[global::Players.Players.players[pID].damageType], (ushort)j, pfbV2T[threadID], threadID);
							num5 -= 0.3f;
							bulletBoxT[threadID].pos1.v[0] = pfbV1T[threadID].v[0];
							bulletBoxT[threadID].pos1.v[1] = pfbV1T[threadID].v[1];
							bulletBoxT[threadID].pos1.v[2] = pfbV1T[threadID].v[2];
							bulletBoxT[threadID].pos2.v[0] = 0f - pfbV2T[threadID].v[0];
							bulletBoxT[threadID].pos2.v[1] = 0f - pfbV2T[threadID].v[1];
							bulletBoxT[threadID].pos2.v[2] = 0f - pfbV2T[threadID].v[2];
							mainC.renderingMain.New_Particle(global::Players.Players.playerRaces[global::Players.Players.players[pID].race].particleEffect[global::Players.Players.players[pID].type], ref bulletBoxT[threadID].pos1, ref bulletBoxT[threadID].pos2, ref bullet[j].phys1.angle, ref bullet[j].phys1.velocity, pID, threadID);
						}
						bulletActive[uBufferID, j] = 0;
						flag2 = true;
					}
					else if (num5 >= 0f)
					{
						flag = true;
						mainC.playersMain.Player_Hit(pID, bullet[j].playerID, global::Players.Players.players[pID].jointWasShot, ammo[bullet[j].ammoIndex].damage[global::Players.Players.players[pID].damageType], (ushort)j, pfbV2T[threadID], threadID);
						num5 -= 0.3f;
						bulletBoxT[threadID].pos1.v[0] = pfbV1T[threadID].v[0];
						bulletBoxT[threadID].pos1.v[1] = pfbV1T[threadID].v[1];
						bulletBoxT[threadID].pos1.v[2] = pfbV1T[threadID].v[2];
						bulletBoxT[threadID].pos2.v[0] = 0f - pfbV2T[threadID].v[0];
						bulletBoxT[threadID].pos2.v[1] = 0f - pfbV2T[threadID].v[1];
						bulletBoxT[threadID].pos2.v[2] = 0f - pfbV2T[threadID].v[2];
						mainC.renderingMain.New_Particle(global::Players.Players.playerRaces[global::Players.Players.players[pID].race].particleEffect[global::Players.Players.players[pID].type], ref bulletBoxT[threadID].pos1, ref bulletBoxT[threadID].pos2, ref bullet[j].phys1.angle, ref bullet[j].phys1.velocity, pID, threadID);
						bulletActive[uBufferID, j] = 0;
						flag2 = true;
					}
					else if (i == 12)
					{
						bulletActive[uBufferID, j] = 0;
						flag2 = true;
					}
					else
					{
						distance2 = mainC.terrainMain.Get_Terrain_Height(bullet[j].phys1.position.v[0], bullet[j].phys1.position.v[1], threadID);
						if (bullet[j].phys1.position.v[0] > global::MainGame.MainGame.MaxRight || bullet[j].phys1.position.v[0] < global::MainGame.MainGame.MaxLeft || bullet[j].phys1.position.v[1] > global::MainGame.MainGame.MaxForward || bullet[j].phys1.position.v[1] < global::MainGame.MainGame.MaxRear || bullet[j].phys1.position.v[2] < distance2 || bullet[j].phys1.position.v[2] > global::MainGame.MainGame.MaxUp)
						{
							bulletActive[uBufferID, j] = 0;
							if (bullet[j].phys1.position.v[2] < distance2)
							{
								flag2 = true;
							}
						}
						else if (!mainC.collisionMain.Position_InsideBoundary(ref bullet[j].phys1))
						{
							bulletActive[uBufferID, j] = 2;
						}
					}
				}
				else if (bulletActive[uBufferID, j] == 2)
				{
					mainC.physicsMain.getPosition(ref bullet[j].phys1, global::MainGame.MainGame.frametime);
					distance2 = mainC.terrainMain.Get_Terrain_Height(bullet[j].phys1.position.v[0], bullet[j].phys1.position.v[1], threadID);
					if (bullet[j].phys1.position.v[0] > global::MainGame.MainGame.MaxRight || bullet[j].phys1.position.v[0] < global::MainGame.MainGame.MaxLeft || bullet[j].phys1.position.v[1] > global::MainGame.MainGame.MaxForward || bullet[j].phys1.position.v[1] < global::MainGame.MainGame.MaxRear || bullet[j].phys1.position.v[2] < distance2 || bullet[j].phys1.position.v[2] > global::MainGame.MainGame.MaxUp)
					{
						bulletActive[uBufferID, j] = 0;
						if (bullet[j].phys1.position.v[2] < distance2)
						{
							flag2 = true;
						}
					}
				}
				else
				{
					bulletActive[uBufferID, j] = 0;
				}
				if (bullet[j].playerID == 0 && global::MainGame.MainGame.bombViewEnabled && j == global::MainGame.MainGame.viewFollowingObjectID)
				{
					global::Rendering.Rendering.cameraSpeed = bullet[j].phys1.totalVelocity;
					global::Rendering.Rendering.camObjectGoal[uBufferID].X = bullet[j].phys1.position.v[0];
					global::Rendering.Rendering.camObjectGoal[uBufferID].Y = bullet[j].phys1.position.v[1];
					global::Rendering.Rendering.camObjectGoal[uBufferID].Z = bullet[j].phys1.position.v[2];
					global::Rendering.Rendering.camPosGoal[uBufferID].X = bullet[j].phys1.position.v[0] - bullet[j].mv[uBufferID].M21 * 20f + bullet[j].mv[uBufferID].M31 * 10f;
					global::Rendering.Rendering.camPosGoal[uBufferID].Y = bullet[j].phys1.position.v[1] - bullet[j].mv[uBufferID].M22 * 20f + bullet[j].mv[uBufferID].M32 * 10f;
					global::Rendering.Rendering.camPosGoal[uBufferID].Z = bullet[j].phys1.position.v[2] - bullet[j].mv[uBufferID].M23 * 20f + bullet[j].mv[uBufferID].M33 * 10f;
					if (bulletActive[uBufferID, j] == 0)
					{
						global::Rendering.Rendering.camPosGoal[uBufferID].Z += 1500f;
					}
					global::Rendering.Rendering.camUp[uBufferID].X = bullet[j].mv[uBufferID].M31;
					global::Rendering.Rendering.camUp[uBufferID].Y = bullet[j].mv[uBufferID].M32;
					global::Rendering.Rendering.camUp[uBufferID].Z = bullet[j].mv[uBufferID].M33;
				}
				if (bulletActive[uBufferID, j] != 0)
				{
					break;
				}
				if (bullet[j].soundID > -1)
				{
					if (bullet[j].playerID == 0)
					{
						mainC.soundsMain.Stop_Priority_Sound(bullet[j].soundID);
					}
					else
					{
						mainC.soundsMain.Stop_Sound(bullet[j].soundID);
					}
				}
				if (flag2)
				{
					if (bullet[j].soundID2 > -1)
					{
						mainC.soundsMain.Play_Priority_Sound(ammo[bullet[j].soundID2].sound2, bulletBoxT[threadID].pos2.v[0], bulletBoxT[threadID].pos2.v[1], bulletBoxT[threadID].pos2.v[2], 0f, 0f, 0f);
					}
					apffwV1.v[0] = bulletBoxT[threadID].pos2.v[0];
					apffwV1.v[1] = bulletBoxT[threadID].pos2.v[1];
					apffwV1.v[2] = bulletBoxT[threadID].pos2.v[2];
					Splash_Damage_From_Weapon(bulletBoxT[threadID].pos1.v[0], bulletBoxT[threadID].pos1.v[1], bulletBoxT[threadID].pos1.v[2] + 20f, (short)j, bullet[j].playerID, bullet[j].ammoIndex, threadID);
					mainC.renderingMain.New_Particle_New(16, bulletBoxT[threadID].pos1.v[0], bulletBoxT[threadID].pos1.v[1], bulletBoxT[threadID].pos1.v[2], 1.5f, 0f, 0f, 0, threadID);
				}
				if (bullet[j].playerID == 0)
				{
					if (j == global::MainGame.MainGame.viewFollowingObjectID)
					{
						projectileViewTimer = true;
						viewFollowingTimer = 0.75f;
					}
					global::Players.Players.needToReload = !Is_Player_Weapon_Loaded(0);
				}
				break;
			}
			case 7:
			{
				bool flag2 = false;
				if (bulletActive[uBufferID, j] == 1)
				{
					num2 = (bulletBoxT[threadID].pos1.v[0] = bullet[j].phys1.position.v[0]);
					num3 = (bulletBoxT[threadID].pos1.v[1] = bullet[j].phys1.position.v[1]);
					num4 = (bulletBoxT[threadID].pos1.v[2] = bullet[j].phys1.position.v[2]);
					mainC.physicsMain.getPosition(ref bullet[j].phys1, global::MainGame.MainGame.frametime);
					bulletBoxT[threadID].pos2.v[0] = bullet[j].phys1.position.v[0];
					bulletBoxT[threadID].pos2.v[1] = bullet[j].phys1.position.v[1];
					bulletBoxT[threadID].pos2.v[2] = bullet[j].phys1.position.v[2];
					if (mainC.targetMain.Check_Collision(num2, num3, num4, bullet[j].phys1.position.v[0], bullet[j].phys1.position.v[1], bullet[j].phys1.position.v[2]))
					{
						bulletActive[uBufferID, j] = 0;
					}
					distance2 = bullet[j].phys1.position.v[0] - num2;
					distanceHit = bullet[j].phys1.position.v[1] - num3;
					float num12 = bullet[j].phys1.position.v[2] - num4;
					float num17 = (float)Math.Sqrt(distance2 * distance2 + distanceHit * distanceHit);
					num6 = num17;
					float num18 = (float)Math.Sqrt(distance2 * distance2 + distanceHit * distanceHit + num12 * num12);
					distanceHit = ((num17 == 0f) ? 0f : (distanceHit / num17));
					num17 = (float)Math.Acos(distanceHit);
					if (distance2 > 0f)
					{
						num17 = (float)Math.PI * 2f - num17;
					}
					ref Matrix reference = ref bullet[j].mv[uBufferID];
					reference = Matrix.CreateRotationZ(num17);
					num12 = ((num18 == 0f) ? 0f : (num12 / num18));
					num17 = (float)Math.Asin(num12);
					ref Matrix reference2 = ref bullet[j].mv[uBufferID];
					reference2 = Matrix.CreateRotationX(num17) * bullet[j].mv[uBufferID];
					distance2 = (float)Math.Sqrt(bullet[j].phys1.velocity.v[0] * bullet[j].phys1.velocity.v[0] + bullet[j].phys1.velocity.v[1] * bullet[j].phys1.velocity.v[1] + bullet[j].phys1.velocity.v[2] * bullet[j].phys1.velocity.v[2]);
					bullet[j].phys1.velocity.v[0] = distance2 * bullet[j].mv[uBufferID].M21;
					bullet[j].phys1.velocity.v[1] = distance2 * bullet[j].mv[uBufferID].M22;
					bullet[j].phys1.velocity.v[2] = distance2 * bullet[j].mv[uBufferID].M23;
					mainC.mapsMain.Set_Position_OutsideBoundary(ref bulletBoxT[threadID]);
					num5 = mainC.playersMain.Check_Player_Impact_Threaded(ref bulletBoxT[threadID], ref pID, ref pfbV1T[threadID], ref pfbV2T[threadID], radius, bullet[j].playerID, threadID);
					i = 0;
					num = -1;
					short returnValueZoneCheckIndex = 0;
					InitialRayStart.X = num2;
					InitialRayStart.Y = num3;
					InitialRayStart.Z = num4;
					InitialRayEnd.X = bullet[j].phys1.position.v[0];
					InitialRayEnd.Y = bullet[j].phys1.position.v[1];
					InitialRayEnd.Z = bullet[j].phys1.position.v[2];
					while (mainC.zonesMain.Check_Zones_For_Point(num2, num3, num4, 5f, returnValueZoneCheckIndex, 1, out returnValueZoneCheckIndex, out returnValueZoneCheckObjID))
					{
						int num11 = Zones.zones[returnValueZoneCheckObjID].zoneList.numObjects;
						for (int num10 = 0; num10 < num11; num10++)
						{
							if (mainC.collisionMain.Check_Polygon_Ray_Collision_Projectile(Zones.zones[returnValueZoneCheckObjID].zoneList.oList[num10], num, ref InitialRayStart, ref InitialRayEnd, ref Zones.zones[returnValueZoneCheckObjID].zoneList.matrixList[num10], out distance2, out IntersectPosition, out IntersectNormal, out num, threadID))
							{
								flag = true;
								if ((i == 0 || distance < num6) && (num5 == -1f || distance < num5))
								{
									bulletBoxT[threadID].pos1.v[0] = IntersectPosition.X + IntersectNormal.X;
									bulletBoxT[threadID].pos1.v[1] = IntersectPosition.Y + IntersectNormal.Y;
									bulletBoxT[threadID].pos1.v[2] = IntersectPosition.Z + IntersectNormal.Z;
									bulletBoxT[threadID].pos2.v[0] = bulletBoxT[threadID].pos1.v[0];
									bulletBoxT[threadID].pos2.v[1] = bulletBoxT[threadID].pos1.v[1];
									bulletBoxT[threadID].pos2.v[2] = bulletBoxT[threadID].pos1.v[2];
									num6 = distance;
									i = 8;
								}
							}
						}
					}
					if (i == 8)
					{
						flag = true;
						if (!(num6 < num5) && !(num5 < 0f) && pID > -1)
						{
							mainC.playersMain.Player_Hit(pID, bullet[j].playerID, global::Players.Players.players[pID].jointWasShot, ammo[bullet[j].ammoIndex].damage[global::Players.Players.players[pID].damageType], (ushort)j, pfbV2T[threadID], threadID);
							num5 -= 0.3f;
							bulletBoxT[threadID].pos1.v[0] = pfbV1T[threadID].v[0];
							bulletBoxT[threadID].pos1.v[1] = pfbV1T[threadID].v[1];
							bulletBoxT[threadID].pos1.v[2] = pfbV1T[threadID].v[2];
							bulletBoxT[threadID].pos2.v[0] = 0f - pfbV2T[threadID].v[0];
							bulletBoxT[threadID].pos2.v[1] = 0f - pfbV2T[threadID].v[1];
							bulletBoxT[threadID].pos2.v[2] = 0f - pfbV2T[threadID].v[2];
							if (bullet[j].playerID == 0 && (global::Players.Players.players[pID].teamMask & global::Players.Players.enemyTeamMask) != 0)
							{
								showTargetCrosshairTimer = 0.25f;
							}
						}
						bulletActive[uBufferID, j] = 0;
						flag2 = true;
						break;
					}
					if (num5 >= 0f)
					{
						flag = true;
						mainC.playersMain.Player_Hit(pID, bullet[j].playerID, global::Players.Players.players[pID].jointWasShot, ammo[bullet[j].ammoIndex].damage[global::Players.Players.players[pID].damageType], (ushort)j, pfbV2T[threadID], threadID);
						num5 -= 0.3f;
						bulletBoxT[threadID].pos1.v[0] = pfbV1T[threadID].v[0];
						bulletBoxT[threadID].pos1.v[1] = pfbV1T[threadID].v[1];
						bulletBoxT[threadID].pos1.v[2] = pfbV1T[threadID].v[2];
						bulletBoxT[threadID].pos2.v[0] = 0f - pfbV2T[threadID].v[0];
						bulletBoxT[threadID].pos2.v[1] = 0f - pfbV2T[threadID].v[1];
						bulletBoxT[threadID].pos2.v[2] = 0f - pfbV2T[threadID].v[2];
						if (bullet[j].playerID == 0 && (global::Players.Players.players[pID].teamMask & global::Players.Players.enemyTeamMask) != 0)
						{
							showTargetCrosshairTimer = 0.25f;
						}
						bulletActive[uBufferID, j] = 0;
						flag2 = true;
						break;
					}
					if (i == 12)
					{
						bulletActive[uBufferID, j] = 0;
						flag2 = true;
						mainC.renderingMain.New_Particle_New(2, num2, num3, num4, 0f, 0f, 31f, 0, threadID);
						break;
					}
					distance2 = mainC.terrainMain.Get_Terrain_Height(bullet[j].phys1.position.v[0], bullet[j].phys1.position.v[1], threadID);
					if (bullet[j].phys1.position.v[0] > global::MainGame.MainGame.MaxRight || bullet[j].phys1.position.v[0] < global::MainGame.MainGame.MaxLeft || bullet[j].phys1.position.v[1] > global::MainGame.MainGame.MaxForward || bullet[j].phys1.position.v[1] < global::MainGame.MainGame.MaxRear || bullet[j].phys1.position.v[2] < distance2 || bullet[j].phys1.position.v[2] > global::MainGame.MainGame.MaxUp)
					{
						bulletActive[uBufferID, j] = 0;
						if (bullet[j].phys1.position.v[2] < distance2)
						{
							flag2 = true;
						}
					}
					else if (!mainC.collisionMain.Position_InsideBoundary(ref bullet[j].phys1))
					{
						bulletActive[uBufferID, j] = 2;
					}
				}
				else if (bulletActive[uBufferID, j] == 2)
				{
					mainC.physicsMain.getPosition(ref bullet[j].phys1, global::MainGame.MainGame.frametime);
					distance2 = mainC.terrainMain.Get_Terrain_Height(bullet[j].phys1.position.v[0], bullet[j].phys1.position.v[1], threadID);
					if (bullet[j].phys1.position.v[0] > global::MainGame.MainGame.MaxRight || bullet[j].phys1.position.v[0] < global::MainGame.MainGame.MaxLeft || bullet[j].phys1.position.v[1] > global::MainGame.MainGame.MaxForward || bullet[j].phys1.position.v[1] < global::MainGame.MainGame.MaxRear || bullet[j].phys1.position.v[2] < distance2 || bullet[j].phys1.position.v[2] > global::MainGame.MainGame.MaxUp)
					{
						bulletActive[uBufferID, j] = 0;
						if (bullet[j].phys1.position.v[2] < distance2)
						{
							flag2 = true;
						}
					}
				}
				else
				{
					bulletActive[uBufferID, j] = 0;
				}
				break;
			}
			case 8:
				if (bulletActive[uBufferID, j] == 1)
				{
					num2 = (bulletBoxT[threadID].pos1.v[0] = bullet[j].phys1.position.v[0]);
					num3 = (bulletBoxT[threadID].pos1.v[1] = bullet[j].phys1.position.v[1]);
					num4 = (bulletBoxT[threadID].pos1.v[2] = bullet[j].phys1.position.v[2]);
					mainC.physicsMain.getPosition(ref bullet[j].phys1, global::MainGame.MainGame.frametime);
					bulletBoxT[threadID].pos2.v[0] = bullet[j].phys1.position.v[0];
					bulletBoxT[threadID].pos2.v[1] = bullet[j].phys1.position.v[1];
					bulletBoxT[threadID].pos2.v[2] = bullet[j].phys1.position.v[2];
					mainC.mapsMain.Set_Position_OutsideBoundary(ref bulletBoxT[threadID]);
					num7 = bulletBoxT[threadID].pos2.v[0] - bulletBoxT[threadID].pos1.v[0];
					num8 = bulletBoxT[threadID].pos2.v[1] - bulletBoxT[threadID].pos1.v[1];
					num9 = bulletBoxT[threadID].pos2.v[2] - bulletBoxT[threadID].pos1.v[2];
					float num13 = (float)Math.Sqrt(num7 * num7 + num8 * num8 + num9 * num9);
					num6 = num13;
					if (num13 != 0f)
					{
						num7 /= num13;
						num8 /= num13;
						num9 /= num13;
					}
					num5 = mainC.playersMain.Check_Player_Impact_Threaded(ref bulletBoxT[threadID], ref pID, ref pfbV1T[threadID], ref pfbV2T[threadID], radius, bullet[j].playerID, threadID);
					i = 0;
					float num14 = bulletBoxT[threadID].pos2.v[0];
					float num15 = bulletBoxT[threadID].pos2.v[1];
					float num16 = bulletBoxT[threadID].pos2.v[2];
					num = -1;
					short returnValueZoneCheckIndex = 0;
					InitialRayStart.X = num2;
					InitialRayStart.Y = num3;
					InitialRayStart.Z = num4;
					InitialRayEnd.X = bullet[j].phys1.position.v[0];
					InitialRayEnd.Y = bullet[j].phys1.position.v[1];
					InitialRayEnd.Z = bullet[j].phys1.position.v[2];
					while (mainC.zonesMain.Check_Zones_For_Point(num2, num3, num4, 5f, returnValueZoneCheckIndex, 1, out returnValueZoneCheckIndex, out returnValueZoneCheckObjID))
					{
						int num11 = Zones.zones[returnValueZoneCheckObjID].zoneList.numObjects;
						for (int num10 = 0; num10 < num11; num10++)
						{
							if (mainC.collisionMain.Check_Polygon_Ray_Collision_Projectile(Zones.zones[returnValueZoneCheckObjID].zoneList.oList[num10], num, ref InitialRayStart, ref InitialRayEnd, ref Zones.zones[returnValueZoneCheckObjID].zoneList.matrixList[num10], out distance, out IntersectPosition, out IntersectNormal, out num, threadID) && (i == 0 || distance < num6) && (num5 == -1f || distance < num5))
							{
								num7 = IntersectNormal.X;
								num8 = IntersectNormal.Y;
								num9 = IntersectNormal.Z;
								num14 = IntersectPosition.X + num7;
								num15 = IntersectPosition.Y + num8;
								num16 = IntersectPosition.Z + num9;
								bulletBoxT[threadID].pos1.v[0] = num14;
								bulletBoxT[threadID].pos1.v[1] = num15;
								bulletBoxT[threadID].pos1.v[2] = num16;
								bulletBoxT[threadID].pos2.v[0] = bulletBoxT[threadID].pos1.v[0];
								bulletBoxT[threadID].pos2.v[1] = bulletBoxT[threadID].pos1.v[1];
								bulletBoxT[threadID].pos2.v[2] = bulletBoxT[threadID].pos1.v[2];
								num6 = distance;
								i = 8;
							}
						}
					}
					if (i == 8)
					{
						flag = true;
						if (num6 < num5 || num5 < 0f)
						{
							bulletBoxT[threadID].pos1.v[0] = 0f - global::Collision.Collision.ccsVxT[threadID];
							bulletBoxT[threadID].pos1.v[1] = 0f - global::Collision.Collision.ccsVyT[threadID];
							bulletBoxT[threadID].pos1.v[2] = 0f - global::Collision.Collision.ccsVzT[threadID];
							mainC.soundsMain.Play_Sound("Laser_Hit", num14, num15, num16, 0f, 0f, 0f);
						}
						else if (pID > -1)
						{
							mainC.soundsMain.Play_Sound("Player_Hit", global::Players.Players.players[pID].charP.position.v[0], global::Players.Players.players[pID].charP.position.v[1], global::Players.Players.players[pID].charP.position.v[2], global::Players.Players.players[pID].charP.velocity.v[0], global::Players.Players.players[pID].charP.velocity.v[1], global::Players.Players.players[pID].charP.velocity.v[2]);
							mainC.playersMain.Player_Hit(pID, bullet[j].playerID, global::Players.Players.players[pID].jointWasShot, ammo[bullet[j].ammoIndex].damage[global::Players.Players.players[pID].damageType], (ushort)j, pfbV2T[threadID], threadID);
							num5 -= 0.3f;
							num14 = pfbV1T[threadID].v[0];
							num15 = pfbV1T[threadID].v[1];
							num16 = pfbV1T[threadID].v[2];
							bulletBoxT[threadID].pos1.v[0] = num14;
							bulletBoxT[threadID].pos1.v[1] = num15;
							bulletBoxT[threadID].pos1.v[2] = num16;
							bulletBoxT[threadID].pos2.v[0] = 0f - pfbV2T[threadID].v[0];
							bulletBoxT[threadID].pos2.v[1] = 0f - pfbV2T[threadID].v[1];
							bulletBoxT[threadID].pos2.v[2] = 0f - pfbV2T[threadID].v[2];
							mainC.renderingMain.New_Particle(global::Players.Players.playerRaces[global::Players.Players.players[pID].race].particleEffect[global::Players.Players.players[pID].type], ref bulletBoxT[threadID].pos1, ref bulletBoxT[threadID].pos2, ref bullet[j].phys1.angle, ref bullet[j].phys1.velocity, pID, threadID);
							if (bullet[j].playerID == 0 && (global::Players.Players.players[pID].teamMask & global::Players.Players.enemyTeamMask) != 0)
							{
								showTargetCrosshairTimer = 0.25f;
							}
						}
						bulletActive[uBufferID, j] = 0;
					}
					else if (num5 >= 0f)
					{
						flag = true;
						mainC.soundsMain.Play_Sound("Player_Hit", global::Players.Players.players[pID].charP.position.v[0], global::Players.Players.players[pID].charP.position.v[1], global::Players.Players.players[pID].charP.position.v[2], global::Players.Players.players[pID].charP.velocity.v[0], global::Players.Players.players[pID].charP.velocity.v[1], global::Players.Players.players[pID].charP.velocity.v[2]);
						mainC.playersMain.Player_Hit(pID, bullet[j].playerID, global::Players.Players.players[pID].jointWasShot, ammo[bullet[j].ammoIndex].damage[global::Players.Players.players[pID].damageType], (ushort)j, pfbV2T[threadID], threadID);
						bulletActive[uBufferID, j] = 0;
						num5 -= 0.3f;
						num14 = pfbV1T[threadID].v[0];
						num15 = pfbV1T[threadID].v[1];
						num16 = pfbV1T[threadID].v[2];
						bulletBoxT[threadID].pos1.v[0] = num14;
						bulletBoxT[threadID].pos1.v[1] = num15;
						bulletBoxT[threadID].pos1.v[2] = num16;
						bulletBoxT[threadID].pos2.v[0] = 0f - pfbV2T[threadID].v[0];
						bulletBoxT[threadID].pos2.v[1] = 0f - pfbV2T[threadID].v[1];
						bulletBoxT[threadID].pos2.v[2] = 0f - pfbV2T[threadID].v[2];
						num14 = global::Players.Players.players[pID].charP.position.v[0];
						num15 = global::Players.Players.players[pID].charP.position.v[1];
						num16 = global::Players.Players.players[pID].charP.position.v[2];
						if (bullet[j].playerID == 0 && (global::Players.Players.players[pID].teamMask & global::Players.Players.enemyTeamMask) != 0)
						{
							showTargetCrosshairTimer = 0.25f;
						}
					}
					else if (i == 12)
					{
						bulletActive[uBufferID, j] = 0;
						flag = true;
					}
					else
					{
						distance2 = mainC.terrainMain.Get_Terrain_Height(bullet[j].phys1.position.v[0], bullet[j].phys1.position.v[1], threadID);
						if (bullet[j].phys1.position.v[0] > global::MainGame.MainGame.MaxRight || bullet[j].phys1.position.v[0] < global::MainGame.MainGame.MaxLeft || bullet[j].phys1.position.v[1] > global::MainGame.MainGame.MaxForward || bullet[j].phys1.position.v[1] < global::MainGame.MainGame.MaxRear || bullet[j].phys1.position.v[2] < distance2 || bullet[j].phys1.position.v[2] > global::MainGame.MainGame.MaxUp)
						{
							bulletActive[uBufferID, j] = 0;
						}
						else if (!mainC.collisionMain.Position_InsideBoundary(ref bullet[j].phys1))
						{
							bulletActive[uBufferID, j] = 2;
						}
					}
					if (flag)
					{
						int ammoIndex = ammo[bullet[j].ammoIndex].numBreakApartModels;
						for (i = 0; i < ammoIndex; i++)
						{
							mainC.renderingMain.New_Solid_Particle(1, num14, num15, num16, bullet[j].mv[uBufferID].M21, bullet[j].mv[uBufferID].M22, bullet[j].mv[uBufferID].M23, bullet[j].mv[uBufferID].M11, bullet[j].mv[uBufferID].M12, bullet[j].mv[uBufferID].M13, bullet[j].phys1.velocity.v[0], bullet[j].phys1.velocity.v[1], bullet[j].phys1.velocity.v[2], 5f, 0.33f, ammo[bullet[j].ammoIndex].breakApartModelList[i]);
						}
					}
				}
				else if (bulletActive[uBufferID, j] == 2)
				{
					mainC.physicsMain.getPosition(ref bullet[j].phys1, global::MainGame.MainGame.frametime);
					distance2 = mainC.terrainMain.Get_Terrain_Height(bullet[j].phys1.position.v[0], bullet[j].phys1.position.v[1], threadID);
					if (bullet[j].phys1.position.v[0] > global::MainGame.MainGame.MaxRight || bullet[j].phys1.position.v[0] < global::MainGame.MainGame.MaxLeft || bullet[j].phys1.position.v[1] > global::MainGame.MainGame.MaxForward || bullet[j].phys1.position.v[1] < global::MainGame.MainGame.MaxRear || bullet[j].phys1.position.v[2] < distance2 || bullet[j].phys1.position.v[2] > global::MainGame.MainGame.MaxUp)
					{
						bulletActive[uBufferID, j] = 0;
					}
				}
				else
				{
					bulletActive[uBufferID, j] = 0;
				}
				if (bulletActive[uBufferID, j] == 1 && bullet[j].lightID > -1)
				{
					i = bullet[j].lightID;
					int ammoIndex = bullet[j].ammoIndex;
					ammoLightPos[uBufferID, i].X = bullet[j].phys1.position.v[0];
					ammoLightPos[uBufferID, i].Y = bullet[j].phys1.position.v[1];
					ammoLightPos[uBufferID, i].Z = bullet[j].phys1.position.v[2];
					ammoLightPos[uBufferID, i].W = ammo[ammoIndex].colorIntensity;
					ammoLightColor[uBufferID, i].X = ammo[ammoIndex].lightColor[0];
					ammoLightColor[uBufferID, i].Y = ammo[ammoIndex].lightColor[1];
					ammoLightColor[uBufferID, i].Z = ammo[ammoIndex].lightColor[2];
					ammoLightColor[uBufferID, i].W = ammo[ammoIndex].lightColor[3];
				}
				else if (bullet[j].lightID > -1)
				{
					laserLights[bullet[j].lightID] = -1;
					bullet[j].lightID = -1;
				}
				break;
			case 9:
				if (bulletActive[uBufferID, j] == 1)
				{
					bullet[j].timer -= global::MainGame.MainGame.frametime / global::Physics.Physics.timeMod;
					if (!(bullet[j].timer < 0f))
					{
						break;
					}
					num2 = (bullet[j].phys1.position.v[0] = global::Players.Players.players[bullet[j].playerID].charP.position.v[0]);
					num3 = (bullet[j].phys1.position.v[1] = global::Players.Players.players[bullet[j].playerID].charP.position.v[1]);
					num4 = (bullet[j].phys1.position.v[2] = global::Players.Players.players[bullet[j].playerID].charP.position.v[2]);
					mainC.physicsMain.getPosition(ref bullet[j].phys1, 1f);
					pID = (short)mainC.playersMain.Check_Player_Hit_By_Melee(4, num2, num3, num4, bullet[j].phys1.position.v[0], bullet[j].phys1.position.v[1], bullet[j].phys1.position.v[2], ref bulletBoxT[threadID].pos1, ref bulletBoxT[threadID].pos2, (ushort)bullet[j].playerID, global::Players.Players.players[bullet[j].playerID].team);
					if (pID < 0)
					{
						pID = (short)mainC.playersMain.Check_Player_Hit_By_Melee(8, num2, num3, num4, bullet[j].phys1.position.v[0], bullet[j].phys1.position.v[1], bullet[j].phys1.position.v[2], ref bulletBoxT[threadID].pos1, ref bulletBoxT[threadID].pos2, (ushort)bullet[j].playerID, global::Players.Players.players[bullet[j].playerID].team);
					}
					if (pID > -1)
					{
						mainC.soundsMain.Play_Sound("Player_Hit", global::Players.Players.players[pID].charP.position.v[0], global::Players.Players.players[pID].charP.position.v[1], global::Players.Players.players[pID].charP.position.v[2], global::Players.Players.players[pID].charP.velocity.v[0], global::Players.Players.players[pID].charP.velocity.v[1], global::Players.Players.players[pID].charP.velocity.v[2]);
						mainC.playersMain.Player_Hit(pID, bullet[j].playerID, -1, ammo[bullet[j].ammoIndex].damage[global::Players.Players.players[pID].damageType], j, pfbV2T[threadID], threadID);
						mainC.renderingMain.New_Particle(global::Players.Players.playerRaces[global::Players.Players.players[pID].race].particleEffect[global::Players.Players.players[pID].type], ref bulletBoxT[threadID].pos1, ref bulletBoxT[threadID].pos2, ref bullet[j].phys1.angle, ref bullet[j].phys1.velocity, pID, threadID);
						if (bullet[j].playerID == 0 && (global::Players.Players.players[pID].teamMask & global::Players.Players.enemyTeamMask) != 0)
						{
							showTargetCrosshairTimer = 0.25f;
						}
					}
					bulletActive[uBufferID, j] = 0;
					break;
				}
				num2 = (bullet[j].phys1.position.v[0] = global::Players.Players.players[bullet[j].playerID].charP.position.v[0]);
				num3 = (bullet[j].phys1.position.v[1] = global::Players.Players.players[bullet[j].playerID].charP.position.v[1]);
				num4 = (bullet[j].phys1.position.v[2] = global::Players.Players.players[bullet[j].playerID].charP.position.v[2]);
				mainC.physicsMain.getPosition(ref bullet[j].phys1, 1f);
				pID = (short)mainC.playersMain.Check_Player_Hit_By_Melee(4, num2, num3, num4, bullet[j].phys1.position.v[0], bullet[j].phys1.position.v[1], bullet[j].phys1.position.v[2], ref bulletBoxT[threadID].pos1, ref bulletBoxT[threadID].pos2, (ushort)bullet[j].playerID, global::Players.Players.players[bullet[j].playerID].team);
				if (pID > -1)
				{
					mainC.soundsMain.Play_Sound("Player_Hit", global::Players.Players.players[pID].charP.position.v[0], global::Players.Players.players[pID].charP.position.v[1], global::Players.Players.players[pID].charP.position.v[2], global::Players.Players.players[pID].charP.velocity.v[0], global::Players.Players.players[pID].charP.velocity.v[1], global::Players.Players.players[pID].charP.velocity.v[2]);
					mainC.playersMain.Player_Hit(pID, bullet[j].playerID, -1, ammo[bullet[j].ammoIndex].damage[global::Players.Players.players[pID].damageType], j, pfbV2T[threadID], threadID);
					mainC.renderingMain.New_Particle(global::Players.Players.playerRaces[global::Players.Players.players[pID].race].particleEffect[global::Players.Players.players[pID].type], ref bulletBoxT[threadID].pos1, ref bulletBoxT[threadID].pos2, ref bullet[j].phys1.angle, ref bullet[j].phys1.velocity, pID, threadID);
					if (bullet[j].playerID == 0 && (global::Players.Players.players[pID].teamMask & global::Players.Players.enemyTeamMask) != 0)
					{
						showTargetCrosshairTimer = 0.25f;
					}
				}
				bulletActive[uBufferID, j] = 1;
				break;
			case 10:
				if (bulletActive[uBufferID, j] == 1)
				{
					num2 = (bulletBoxT[threadID].pos1.v[0] = bullet[j].phys1.position.v[0]);
					num3 = (bulletBoxT[threadID].pos1.v[1] = bullet[j].phys1.position.v[1]);
					num4 = (bulletBoxT[threadID].pos1.v[2] = bullet[j].phys1.position.v[2]);
					mainC.physicsMain.getPosition(ref bullet[j].phys1, global::MainGame.MainGame.frametime);
					bulletBoxT[threadID].pos2.v[0] = bullet[j].phys1.position.v[0];
					bulletBoxT[threadID].pos2.v[1] = bullet[j].phys1.position.v[1];
					bulletBoxT[threadID].pos2.v[2] = bullet[j].phys1.position.v[2];
					mainC.mapsMain.Set_Position_OutsideBoundary(ref bulletBoxT[threadID]);
					num5 = mainC.playersMain.Check_Player_Impact_Threaded(ref bulletBoxT[threadID], ref pID, ref pfbV1T[threadID], ref pfbV2T[threadID], radius, bullet[j].playerID, threadID);
					distance2 = bullet[j].phys1.position.v[0] - num2;
					distanceHit = bullet[j].phys1.position.v[1] - num3;
					float num12 = bullet[j].phys1.position.v[2] - num4;
					num6 = (float)Math.Sqrt(distance2 * distance2 + distanceHit * distanceHit + num12 * num12);
					i = 0;
					num = -1;
					short returnValueZoneCheckIndex = 0;
					InitialRayStart.X = num2;
					InitialRayStart.Y = num3;
					InitialRayStart.Z = num4;
					InitialRayEnd.X = bullet[j].phys1.position.v[0];
					InitialRayEnd.Y = bullet[j].phys1.position.v[1];
					InitialRayEnd.Z = bullet[j].phys1.position.v[2];
					while (mainC.zonesMain.Check_Zones_For_Point(num2, num3, num4, 5f, returnValueZoneCheckIndex, 1, out returnValueZoneCheckIndex, out returnValueZoneCheckObjID))
					{
						int num11 = Zones.zones[returnValueZoneCheckObjID].zoneList.numObjects;
						for (int num10 = 0; num10 < num11; num10++)
						{
							if (mainC.collisionMain.Check_Polygon_Ray_Collision_Projectile(Zones.zones[returnValueZoneCheckObjID].zoneList.oList[num10], num, ref InitialRayStart, ref InitialRayEnd, ref Zones.zones[returnValueZoneCheckObjID].zoneList.matrixList[num10], out distance, out IntersectPosition, out IntersectNormal, out num, threadID) && (i == 0 || distance < num6) && (num5 == -1f || distance < num5))
							{
								bulletBoxT[threadID].pos1.v[0] = IntersectPosition.X + IntersectNormal.X;
								bulletBoxT[threadID].pos1.v[1] = IntersectPosition.Y + IntersectNormal.Y;
								bulletBoxT[threadID].pos1.v[2] = IntersectPosition.Z + IntersectNormal.Z;
								bulletBoxT[threadID].pos2.v[0] = bulletBoxT[threadID].pos1.v[0];
								bulletBoxT[threadID].pos2.v[1] = bulletBoxT[threadID].pos1.v[1];
								bulletBoxT[threadID].pos2.v[2] = bulletBoxT[threadID].pos1.v[2];
								num6 = distance;
								i = 8;
							}
						}
					}
					if (i == 8)
					{
						flag = true;
						if (num6 < num5 || num5 < 0f)
						{
							bulletBoxT[threadID].pos1.v[0] = 0f - global::Collision.Collision.ccsVxT[threadID];
							bulletBoxT[threadID].pos1.v[1] = 0f - global::Collision.Collision.ccsVyT[threadID];
							bulletBoxT[threadID].pos1.v[2] = 0f - global::Collision.Collision.ccsVzT[threadID];
							mainC.soundsMain.Play_Sound("BulletImpact_Metal", bulletBoxT[threadID].pos2.v[0], bulletBoxT[threadID].pos2.v[1], bulletBoxT[threadID].pos2.v[2], 0f, 0f, 0f);
							mainC.renderingMain.New_Particle(3, ref bulletBoxT[threadID].pos2, ref bulletBoxT[threadID].pos1, ref bullet[j].phys1.velocity, ref bullet[j].phys1.velocity, 0, threadID);
							mainC.renderingMain.New_Particle(2, ref bulletBoxT[threadID].pos2, ref bullet[j].phys1.velocity, ref bullet[j].phys1.velocity, ref bullet[j].phys1.velocity, 0, threadID);
						}
						else if (pID > -1)
						{
							global::Players.Players.players[pID].impactX += pfbV2T[threadID].v[0] * ammo[bullet[j].ammoIndex].mass * Math.Abs(bullet[j].phys1.velocity.v[0]);
							global::Players.Players.players[pID].impactY += pfbV2T[threadID].v[1] * ammo[bullet[j].ammoIndex].mass * Math.Abs(bullet[j].phys1.velocity.v[1]);
							global::Players.Players.players[pID].impactZ += pfbV2T[threadID].v[2] * ammo[bullet[j].ammoIndex].mass * Math.Abs(bullet[j].phys1.velocity.v[2]);
							mainC.soundsMain.Play_Sound("Splat", global::Players.Players.players[pID].charP.position.v[0], global::Players.Players.players[pID].charP.position.v[1], global::Players.Players.players[pID].charP.position.v[2], global::Players.Players.players[pID].charP.velocity.v[0], global::Players.Players.players[pID].charP.velocity.v[1], global::Players.Players.players[pID].charP.velocity.v[2]);
							mainC.playersMain.Player_Hit(pID, bullet[j].playerID, global::Players.Players.players[pID].jointWasShot, ammo[bullet[j].ammoIndex].damage[global::Players.Players.players[pID].damageType], (ushort)j, pfbV2T[threadID], threadID);
							num5 -= 0.3f;
							bulletBoxT[threadID].pos1.v[0] = pfbV1T[threadID].v[0];
							bulletBoxT[threadID].pos1.v[1] = pfbV1T[threadID].v[1];
							bulletBoxT[threadID].pos1.v[2] = pfbV1T[threadID].v[2];
							bulletBoxT[threadID].pos2.v[0] = 0f - bullet[j].phys1.velocity.v[0];
							bulletBoxT[threadID].pos2.v[1] = 0f - bullet[j].phys1.velocity.v[1];
							bulletBoxT[threadID].pos2.v[2] = 0f - bullet[j].phys1.velocity.v[2];
							mainC.renderingMain.New_Particle(global::Players.Players.playerRaces[global::Players.Players.players[pID].race].particleEffect[global::Players.Players.players[pID].type], ref bulletBoxT[threadID].pos1, ref bulletBoxT[threadID].pos2, ref bullet[j].phys1.angle, ref bullet[j].phys1.velocity, pID, threadID);
							if (bullet[j].playerID == 0 && (global::Players.Players.players[pID].teamMask & global::Players.Players.enemyTeamMask) != 0)
							{
								showTargetCrosshairTimer = 0.25f;
							}
						}
						bulletActive[uBufferID, j] = 0;
					}
					else if (num5 >= 0f)
					{
						global::Players.Players.players[pID].impactX += pfbV2T[threadID].v[0] * ammo[bullet[j].ammoIndex].mass * Math.Abs(bullet[j].phys1.velocity.v[0]);
						global::Players.Players.players[pID].impactY += pfbV2T[threadID].v[1] * ammo[bullet[j].ammoIndex].mass * Math.Abs(bullet[j].phys1.velocity.v[1]);
						global::Players.Players.players[pID].impactZ += pfbV2T[threadID].v[2] * ammo[bullet[j].ammoIndex].mass * Math.Abs(bullet[j].phys1.velocity.v[2]);
						flag = true;
						mainC.soundsMain.Play_Sound("Splat", global::Players.Players.players[pID].charP.position.v[0], global::Players.Players.players[pID].charP.position.v[1], global::Players.Players.players[pID].charP.position.v[2], global::Players.Players.players[pID].charP.velocity.v[0], global::Players.Players.players[pID].charP.velocity.v[1], global::Players.Players.players[pID].charP.velocity.v[2]);
						mainC.playersMain.Player_Hit(pID, bullet[j].playerID, global::Players.Players.players[pID].jointWasShot, ammo[bullet[j].ammoIndex].damage[global::Players.Players.players[pID].damageType], (ushort)j, pfbV2T[threadID], threadID);
						bulletActive[uBufferID, j] = 0;
						num5 -= 0.3f;
						bulletBoxT[threadID].pos1.v[0] = pfbV1T[threadID].v[0];
						bulletBoxT[threadID].pos1.v[1] = pfbV1T[threadID].v[1];
						bulletBoxT[threadID].pos1.v[2] = pfbV1T[threadID].v[2];
						bulletBoxT[threadID].pos2.v[0] = 0f - bullet[j].phys1.velocity.v[0];
						bulletBoxT[threadID].pos2.v[1] = 0f - bullet[j].phys1.velocity.v[1];
						bulletBoxT[threadID].pos2.v[2] = 0f - bullet[j].phys1.velocity.v[2];
						mainC.renderingMain.New_Particle(global::Players.Players.playerRaces[global::Players.Players.players[pID].race].particleEffect[global::Players.Players.players[pID].type], ref bulletBoxT[threadID].pos1, ref bulletBoxT[threadID].pos2, ref bullet[j].phys1.angle, ref bullet[j].phys1.velocity, pID, threadID);
						if (bullet[j].playerID == 0 && (global::Players.Players.players[pID].teamMask & global::Players.Players.enemyTeamMask) != 0)
						{
							showTargetCrosshairTimer = 0.25f;
						}
					}
					else if (i == 12)
					{
						bulletActive[uBufferID, j] = 0;
					}
					else
					{
						distance2 = mainC.terrainMain.Get_Terrain_Height(bullet[j].phys1.position.v[0], bullet[j].phys1.position.v[1], threadID);
						if (bullet[j].phys1.position.v[0] > global::MainGame.MainGame.MaxRight || bullet[j].phys1.position.v[0] < global::MainGame.MainGame.MaxLeft || bullet[j].phys1.position.v[1] > global::MainGame.MainGame.MaxForward || bullet[j].phys1.position.v[1] < global::MainGame.MainGame.MaxRear || bullet[j].phys1.position.v[2] < distance2 || bullet[j].phys1.position.v[2] > global::MainGame.MainGame.MaxUp)
						{
							bulletActive[uBufferID, j] = 0;
						}
						else if (!mainC.collisionMain.Position_InsideBoundary(ref bullet[j].phys1))
						{
							bulletActive[uBufferID, j] = 2;
						}
					}
				}
				else if (bulletActive[uBufferID, j] == 2)
				{
					mainC.physicsMain.getPosition(ref bullet[j].phys1, global::MainGame.MainGame.frametime);
					distance2 = mainC.terrainMain.Get_Terrain_Height(bullet[j].phys1.position.v[0], bullet[j].phys1.position.v[1], threadID);
					if (bullet[j].phys1.position.v[0] > global::MainGame.MainGame.MaxRight || bullet[j].phys1.position.v[0] < global::MainGame.MainGame.MaxLeft || bullet[j].phys1.position.v[1] > global::MainGame.MainGame.MaxForward || bullet[j].phys1.position.v[1] < global::MainGame.MainGame.MaxRear || bullet[j].phys1.position.v[2] < distance2 || bullet[j].phys1.position.v[2] > global::MainGame.MainGame.MaxUp)
					{
						bulletActive[uBufferID, j] = 0;
					}
				}
				else
				{
					bulletActive[uBufferID, j] = 0;
				}
				break;
			case 11:
			{
				bool flag2 = false;
				if (bulletActive[uBufferID, j] == 1)
				{
					bullet[j].timer -= global::MainGame.MainGame.frametime / global::Physics.Physics.timeMod;
					num2 = (bulletBoxT[threadID].pos1.v[0] = bullet[j].phys1.position.v[0]);
					num3 = (bulletBoxT[threadID].pos1.v[1] = bullet[j].phys1.position.v[1]);
					num4 = (bulletBoxT[threadID].pos1.v[2] = bullet[j].phys1.position.v[2]);
					mainC.physicsMain.getPosition(ref bullet[j].phys1, global::MainGame.MainGame.frametime);
					bulletBoxT[threadID].pos2.v[0] = bullet[j].phys1.position.v[0];
					bulletBoxT[threadID].pos2.v[1] = bullet[j].phys1.position.v[1];
					bulletBoxT[threadID].pos2.v[2] = bullet[j].phys1.position.v[2];
					mainC.mapsMain.Set_Position_OutsideBoundary(ref bulletBoxT[threadID]);
					num5 = mainC.playersMain.Check_Player_Impact_Threaded(ref bulletBoxT[threadID], ref pID, ref pfbV1T[threadID], ref pfbV2T[threadID], radius, bullet[j].playerID, threadID);
					distance2 = bullet[j].phys1.position.v[0] - num2;
					distanceHit = bullet[j].phys1.position.v[1] - num3;
					float num12 = bullet[j].phys1.position.v[2] - num4;
					num6 = (float)Math.Sqrt(distance2 * distance2 + distanceHit * distanceHit + num12 * num12);
					i = 0;
					num = -1;
					short returnValueZoneCheckIndex = 0;
					InitialRayStart.X = num2;
					InitialRayStart.Y = num3;
					InitialRayStart.Z = num4;
					InitialRayEnd.X = bullet[j].phys1.position.v[0];
					InitialRayEnd.Y = bullet[j].phys1.position.v[1];
					InitialRayEnd.Z = bullet[j].phys1.position.v[2];
					while (mainC.zonesMain.Check_Zones_For_Point(num2, num3, num4, 5f, returnValueZoneCheckIndex, 1, out returnValueZoneCheckIndex, out returnValueZoneCheckObjID))
					{
						int num11 = Zones.zones[returnValueZoneCheckObjID].zoneList.numObjects;
						for (int num10 = 0; num10 < num11; num10++)
						{
							if (mainC.collisionMain.Check_Polygon_Ray_Collision_Projectile(Zones.zones[returnValueZoneCheckObjID].zoneList.oList[num10], num, ref InitialRayStart, ref InitialRayEnd, ref Zones.zones[returnValueZoneCheckObjID].zoneList.matrixList[num10], out distance, out IntersectPosition, out IntersectNormal, out num, threadID) && (i == 0 || distance < num6) && (num5 == -1f || distance < num5))
							{
								bulletBoxT[threadID].pos1.v[0] = IntersectPosition.X + IntersectNormal.X;
								bulletBoxT[threadID].pos1.v[1] = IntersectPosition.Y + IntersectNormal.Y;
								bulletBoxT[threadID].pos1.v[2] = IntersectPosition.Z + IntersectNormal.Z;
								bulletBoxT[threadID].pos2.v[0] = bulletBoxT[threadID].pos1.v[0];
								bulletBoxT[threadID].pos2.v[1] = bulletBoxT[threadID].pos1.v[1];
								bulletBoxT[threadID].pos2.v[2] = bulletBoxT[threadID].pos1.v[2];
								num6 = distance;
								objectID = Zones.zones[returnValueZoneCheckObjID].zoneList.gidList[num10];
								i = 8;
							}
						}
					}
					if (i == 8)
					{
						flag = true;
						if (num6 < num5 || num5 < 0f)
						{
							bulletBoxT[threadID].pos1.v[0] = 0f - global::Collision.Collision.ccsVxT[threadID];
							bulletBoxT[threadID].pos1.v[1] = 0f - global::Collision.Collision.ccsVyT[threadID];
							bulletBoxT[threadID].pos1.v[2] = 0f - global::Collision.Collision.ccsVzT[threadID];
							mainC.soundsMain.Play_Sound("BulletImpact_Metal", bulletBoxT[threadID].pos2.v[0], bulletBoxT[threadID].pos2.v[1], bulletBoxT[threadID].pos2.v[2], 0f, 0f, 0f);
							mainC.renderingMain.New_Particle(2, ref bulletBoxT[threadID].pos2, ref bullet[j].phys1.velocity, ref bullet[j].phys1.velocity, ref bullet[j].phys1.velocity, 0, threadID);
						}
						else if (pID > -1)
						{
							global::Players.Players.players[pID].impactX += pfbV2T[threadID].v[0] * ammo[bullet[j].ammoIndex].mass * Math.Abs(bullet[j].phys1.velocity.v[0]);
							global::Players.Players.players[pID].impactY += pfbV2T[threadID].v[1] * ammo[bullet[j].ammoIndex].mass * Math.Abs(bullet[j].phys1.velocity.v[1]);
							global::Players.Players.players[pID].impactZ += pfbV2T[threadID].v[2] * ammo[bullet[j].ammoIndex].mass * Math.Abs(bullet[j].phys1.velocity.v[2]);
							mainC.soundsMain.Play_Sound("BulletImpact_Metal", global::Players.Players.players[pID].charP.position.v[0], global::Players.Players.players[pID].charP.position.v[1], global::Players.Players.players[pID].charP.position.v[2], global::Players.Players.players[pID].charP.velocity.v[0], global::Players.Players.players[pID].charP.velocity.v[1], global::Players.Players.players[pID].charP.velocity.v[2]);
							mainC.playersMain.Player_Hit(pID, bullet[j].playerID, global::Players.Players.players[pID].jointWasShot, ammo[bullet[j].ammoIndex].damage[global::Players.Players.players[pID].damageType], (ushort)j, pfbV2T[threadID], threadID);
							num5 -= 0.3f;
							bulletBoxT[threadID].pos1.v[0] = pfbV1T[threadID].v[0];
							bulletBoxT[threadID].pos1.v[1] = pfbV1T[threadID].v[1];
							bulletBoxT[threadID].pos1.v[2] = pfbV1T[threadID].v[2];
							bulletBoxT[threadID].pos2.v[0] = 0f - bullet[j].phys1.velocity.v[0];
							bulletBoxT[threadID].pos2.v[1] = 0f - bullet[j].phys1.velocity.v[1];
							bulletBoxT[threadID].pos2.v[2] = 0f - bullet[j].phys1.velocity.v[2];
							mainC.renderingMain.New_Particle(global::Players.Players.playerRaces[global::Players.Players.players[pID].race].particleEffect[global::Players.Players.players[pID].type], ref bulletBoxT[threadID].pos1, ref bulletBoxT[threadID].pos2, ref bullet[j].phys1.angle, ref bullet[j].phys1.velocity, pID, threadID);
							if (bullet[j].playerID == 0 && (global::Players.Players.players[pID].teamMask & global::Players.Players.enemyTeamMask) != 0)
							{
								showTargetCrosshairTimer = 0.25f;
							}
						}
						bulletActive[uBufferID, j] = 0;
						flag2 = true;
					}
					else if (num5 >= 0f)
					{
						global::Players.Players.players[pID].impactX += pfbV2T[threadID].v[0] * ammo[bullet[j].ammoIndex].mass * Math.Abs(bullet[j].phys1.velocity.v[0]);
						global::Players.Players.players[pID].impactY += pfbV2T[threadID].v[1] * ammo[bullet[j].ammoIndex].mass * Math.Abs(bullet[j].phys1.velocity.v[1]);
						global::Players.Players.players[pID].impactZ += pfbV2T[threadID].v[2] * ammo[bullet[j].ammoIndex].mass * Math.Abs(bullet[j].phys1.velocity.v[2]);
						flag = true;
						mainC.soundsMain.Play_Sound("BulletImpact_Metal", global::Players.Players.players[pID].charP.position.v[0], global::Players.Players.players[pID].charP.position.v[1], global::Players.Players.players[pID].charP.position.v[2], global::Players.Players.players[pID].charP.velocity.v[0], global::Players.Players.players[pID].charP.velocity.v[1], global::Players.Players.players[pID].charP.velocity.v[2]);
						mainC.playersMain.Player_Hit(pID, bullet[j].playerID, global::Players.Players.players[pID].jointWasShot, ammo[bullet[j].ammoIndex].damage[global::Players.Players.players[pID].damageType], (ushort)j, pfbV2T[threadID], threadID);
						num5 -= 0.3f;
						bulletBoxT[threadID].pos1.v[0] = pfbV1T[threadID].v[0];
						bulletBoxT[threadID].pos1.v[1] = pfbV1T[threadID].v[1];
						bulletBoxT[threadID].pos1.v[2] = pfbV1T[threadID].v[2];
						bulletBoxT[threadID].pos2.v[0] = 0f - bullet[j].phys1.velocity.v[0];
						bulletBoxT[threadID].pos2.v[1] = 0f - bullet[j].phys1.velocity.v[1];
						bulletBoxT[threadID].pos2.v[2] = 0f - bullet[j].phys1.velocity.v[2];
						mainC.renderingMain.New_Particle(global::Players.Players.playerRaces[global::Players.Players.players[pID].race].particleEffect[global::Players.Players.players[pID].type], ref bulletBoxT[threadID].pos1, ref bulletBoxT[threadID].pos2, ref bullet[j].phys1.angle, ref bullet[j].phys1.velocity, pID, threadID);
						if (bullet[j].playerID == 0 && (global::Players.Players.players[pID].teamMask & global::Players.Players.enemyTeamMask) != 0)
						{
							showTargetCrosshairTimer = 0.25f;
						}
						bulletActive[uBufferID, j] = 0;
						flag2 = true;
					}
					else if (i == 12)
					{
						bulletActive[uBufferID, j] = 0;
						flag2 = true;
					}
					else
					{
						distance2 = mainC.terrainMain.Get_Terrain_Height(bullet[j].phys1.position.v[0], bullet[j].phys1.position.v[1], threadID);
						if (bullet[j].phys1.position.v[0] > global::MainGame.MainGame.MaxRight || bullet[j].phys1.position.v[0] < global::MainGame.MainGame.MaxLeft || bullet[j].phys1.position.v[1] > global::MainGame.MainGame.MaxForward || bullet[j].phys1.position.v[1] < global::MainGame.MainGame.MaxRear || bullet[j].phys1.position.v[2] < distance2 || bullet[j].phys1.position.v[2] > global::MainGame.MainGame.MaxUp)
						{
							bulletActive[uBufferID, j] = 0;
							if (bullet[j].phys1.position.v[2] < distance2)
							{
								flag2 = true;
							}
						}
						else if (!mainC.collisionMain.Position_InsideBoundary(ref bullet[j].phys1))
						{
							bulletActive[uBufferID, j] = 2;
						}
					}
				}
				else if (bulletActive[uBufferID, j] == 2)
				{
					mainC.physicsMain.getPosition(ref bullet[j].phys1, global::MainGame.MainGame.frametime);
					distance2 = mainC.terrainMain.Get_Terrain_Height(bullet[j].phys1.position.v[0], bullet[j].phys1.position.v[1], threadID);
					if (bullet[j].phys1.position.v[0] > global::MainGame.MainGame.MaxRight || bullet[j].phys1.position.v[0] < global::MainGame.MainGame.MaxLeft || bullet[j].phys1.position.v[1] > global::MainGame.MainGame.MaxForward || bullet[j].phys1.position.v[1] < global::MainGame.MainGame.MaxRear || bullet[j].phys1.position.v[2] < distance2 || bullet[j].phys1.position.v[2] > global::MainGame.MainGame.MaxUp)
					{
						bulletActive[uBufferID, j] = 0;
					}
				}
				else
				{
					bulletActive[uBufferID, j] = 0;
				}
				if (flag2 || bullet[j].timer < 0f)
				{
					bulletActive[uBufferID, j] = 0;
					if (bullet[j].soundID2 > -1)
					{
						mainC.soundsMain.Play_Priority_Sound(ammo[bullet[j].soundID2].sound2, bulletBoxT[threadID].pos2.v[0], bulletBoxT[threadID].pos2.v[1], bulletBoxT[threadID].pos2.v[2], 0f, 0f, 0f);
					}
					apffwV1.v[0] = bulletBoxT[threadID].pos2.v[0];
					apffwV1.v[1] = bulletBoxT[threadID].pos2.v[1];
					apffwV1.v[2] = bulletBoxT[threadID].pos2.v[2];
					mainC.renderingMain.New_Particle(7, ref apffwV1, ref apffwV2, ref apffwV2, ref apffwV2, 0, threadID);
					Splash_Damage_From_Weapon(bulletBoxT[threadID].pos1.v[0], bulletBoxT[threadID].pos1.v[1], bulletBoxT[threadID].pos1.v[2], (short)j, bullet[j].playerID, bullet[j].ammoIndex, threadID);
				}
				break;
			}
			case 12:
			{
				bool flag2 = false;
				if (bulletActive[uBufferID, j] == 1)
				{
					float num12;
					if (bullet[j].playerID == 0 && global::MainGame.MainGame.bombViewEnabled && j == global::MainGame.MainGame.viewFollowingObjectID)
					{
						distance2 = global::InputHandler.InputHandler.controllerStickRightValX;
						distanceHit = global::InputHandler.InputHandler.controllerStickRightValY;
						num12 = distance2 * 60f * global::MainGame.MainGame.frametime;
						bullet[j].phys1.velocity.v[0] += bullet[j].mv[uBufferID].M11 * num12;
						bullet[j].phys1.velocity.v[1] += bullet[j].mv[uBufferID].M12 * num12;
						if ((num12 = bullet[j].mv[uBufferID].M13 * num12) < 0f)
						{
							bullet[j].phys1.velocity.v[2] += num12;
						}
						num12 = distanceHit * 60f * global::MainGame.MainGame.frametime;
						bullet[j].phys1.velocity.v[0] += bullet[j].mv[uBufferID].M31 * num12;
						bullet[j].phys1.velocity.v[1] += bullet[j].mv[uBufferID].M32 * num12;
						if ((num12 = bullet[j].mv[uBufferID].M33 * num12) < 0f)
						{
							bullet[j].phys1.velocity.v[2] += num12;
						}
					}
					num2 = (bulletBoxT[threadID].pos1.v[0] = bullet[j].phys1.position.v[0]);
					num3 = (bulletBoxT[threadID].pos1.v[1] = bullet[j].phys1.position.v[1]);
					num4 = (bulletBoxT[threadID].pos1.v[2] = bullet[j].phys1.position.v[2]);
					mainC.physicsMain.getPosition(ref bullet[j].phys1, global::MainGame.MainGame.frametime / global::Physics.Physics.timeMod);
					bulletBoxT[threadID].pos2.v[0] = bullet[j].phys1.position.v[0];
					bulletBoxT[threadID].pos2.v[1] = bullet[j].phys1.position.v[1];
					bulletBoxT[threadID].pos2.v[2] = bullet[j].phys1.position.v[2];
					if (bullet[j].playerID == 0)
					{
						mainC.soundsMain.Update_Priority_Sound_Position(bullet[j].soundID, bullet[j].phys1.position.v[0], bullet[j].phys1.position.v[1], bullet[j].phys1.position.v[2]);
					}
					else
					{
						mainC.soundsMain.Update_Sound_Position(bullet[j].soundID, bullet[j].phys1.position.v[0], bullet[j].phys1.position.v[1], bullet[j].phys1.position.v[2]);
					}
					distance2 = (float)Math.Sqrt(bullet[j].phys1.velocity.v[0] * bullet[j].phys1.velocity.v[0] + bullet[j].phys1.velocity.v[1] * bullet[j].phys1.velocity.v[1] + bullet[j].phys1.velocity.v[2] * bullet[j].phys1.velocity.v[2]);
					bullet[j].phys1.totalVelocity = distance2;
					if (distance2 != 0f)
					{
						num7 = bullet[j].phys1.velocity.v[0] / distance2;
						num8 = bullet[j].phys1.velocity.v[1] / distance2;
						num9 = bullet[j].phys1.velocity.v[2] / distance2;
						num5 = distance2 * distance2 * 1.7453E-05f * global::MainGame.MainGame.frametime;
						distanceHit = num5 * (num7 * bullet[j].mv[uBufferID].M31 + num8 * bullet[j].mv[uBufferID].M32 + num9 * bullet[j].mv[uBufferID].M33);
						num12 = num5 * (num7 * bullet[j].mv[uBufferID].M11 + num8 * bullet[j].mv[uBufferID].M12 + num9 * bullet[j].mv[uBufferID].M13);
						bullet[j].rot *= Quaternion.CreateFromYawPitchRoll(0f, distanceHit, 0f - num12);
						Matrix.CreateFromQuaternion(ref bullet[j].rot, out bullet[j].mv[uBufferID]);
					}
					mainC.mapsMain.Set_Position_OutsideBoundary(ref bulletBoxT[threadID]);
					num5 = mainC.playersMain.Check_Player_Impact_Threaded(ref bulletBoxT[threadID], ref pID, ref pfbV1T[threadID], ref pfbV2T[threadID], radius, bullet[j].playerID, threadID);
					distance2 = bullet[j].phys1.position.v[0] - num2;
					distanceHit = bullet[j].phys1.position.v[1] - num3;
					num12 = bullet[j].phys1.position.v[2] - num4;
					num6 = (float)Math.Sqrt(distance2 * distance2 + distanceHit * distanceHit + num12 * num12);
					i = 0;
					num = -1;
					short returnValueZoneCheckIndex = 0;
					InitialRayStart.X = num2;
					InitialRayStart.Y = num3;
					InitialRayStart.Z = num4;
					InitialRayEnd.X = bullet[j].phys1.position.v[0];
					InitialRayEnd.Y = bullet[j].phys1.position.v[1];
					InitialRayEnd.Z = bullet[j].phys1.position.v[2];
					while (mainC.zonesMain.Check_Zones_For_Point(num2, num3, num4, 5f, returnValueZoneCheckIndex, 0, out returnValueZoneCheckIndex, out returnValueZoneCheckObjID))
					{
						int num11 = Zones.zones[returnValueZoneCheckObjID].zoneList.numObjects;
						for (int num10 = 0; num10 < num11; num10++)
						{
							if (mainC.collisionMain.Check_Polygon_Ray_Collision_Projectile(Zones.zones[returnValueZoneCheckObjID].zoneList.oList[num10], num, ref InitialRayStart, ref InitialRayEnd, ref Zones.zones[returnValueZoneCheckObjID].zoneList.matrixList[num10], out distance2, out IntersectPosition, out IntersectNormal, out num, threadID) && (i == 0 || distance < num6) && (num5 == -1f || distance < num5))
							{
								bulletBoxT[threadID].pos1.v[0] = IntersectPosition.X + IntersectNormal.X;
								bulletBoxT[threadID].pos1.v[1] = IntersectPosition.Y + IntersectNormal.Y;
								bulletBoxT[threadID].pos1.v[2] = IntersectPosition.Z + IntersectNormal.Z;
								bulletBoxT[threadID].pos2.v[0] = bulletBoxT[threadID].pos1.v[0];
								bulletBoxT[threadID].pos2.v[1] = bulletBoxT[threadID].pos1.v[1];
								bulletBoxT[threadID].pos2.v[2] = bulletBoxT[threadID].pos1.v[2];
								num6 = distance;
								i = 8;
							}
						}
					}
					if (i == 8)
					{
						flag = true;
						if (!(num6 < num5) && !(num5 < 0f) && pID > -1)
						{
							mainC.playersMain.Player_Hit(pID, bullet[j].playerID, global::Players.Players.players[pID].jointWasShot, ammo[bullet[j].ammoIndex].damage[global::Players.Players.players[pID].damageType], (ushort)j, pfbV2T[threadID], threadID);
							num5 -= 0.3f;
							bulletBoxT[threadID].pos1.v[0] = pfbV1T[threadID].v[0];
							bulletBoxT[threadID].pos1.v[1] = pfbV1T[threadID].v[1];
							bulletBoxT[threadID].pos1.v[2] = pfbV1T[threadID].v[2];
							bulletBoxT[threadID].pos2.v[0] = 0f - pfbV2T[threadID].v[0];
							bulletBoxT[threadID].pos2.v[1] = 0f - pfbV2T[threadID].v[1];
							bulletBoxT[threadID].pos2.v[2] = 0f - pfbV2T[threadID].v[2];
							mainC.renderingMain.New_Particle(global::Players.Players.playerRaces[global::Players.Players.players[pID].race].particleEffect[global::Players.Players.players[pID].type], ref bulletBoxT[threadID].pos1, ref bulletBoxT[threadID].pos2, ref bullet[j].phys1.angle, ref bullet[j].phys1.velocity, pID, threadID);
						}
						bulletActive[uBufferID, j] = 0;
						flag2 = true;
					}
					else if (num5 >= 0f)
					{
						flag = true;
						mainC.playersMain.Player_Hit(pID, bullet[j].playerID, global::Players.Players.players[pID].jointWasShot, ammo[bullet[j].ammoIndex].damage[global::Players.Players.players[pID].damageType], (ushort)j, pfbV2T[threadID], threadID);
						num5 -= 0.3f;
						bulletBoxT[threadID].pos1.v[0] = pfbV1T[threadID].v[0];
						bulletBoxT[threadID].pos1.v[1] = pfbV1T[threadID].v[1];
						bulletBoxT[threadID].pos1.v[2] = pfbV1T[threadID].v[2];
						bulletBoxT[threadID].pos2.v[0] = 0f - pfbV2T[threadID].v[0];
						bulletBoxT[threadID].pos2.v[1] = 0f - pfbV2T[threadID].v[1];
						bulletBoxT[threadID].pos2.v[2] = 0f - pfbV2T[threadID].v[2];
						mainC.renderingMain.New_Particle(global::Players.Players.playerRaces[global::Players.Players.players[pID].race].particleEffect[global::Players.Players.players[pID].type], ref bulletBoxT[threadID].pos1, ref bulletBoxT[threadID].pos2, ref bullet[j].phys1.angle, ref bullet[j].phys1.velocity, pID, threadID);
						bulletActive[uBufferID, j] = 0;
						flag2 = true;
					}
					else if (i == 12)
					{
						bulletActive[uBufferID, j] = 0;
						flag2 = true;
					}
					else
					{
						distance2 = mainC.terrainMain.Get_Terrain_Height(bullet[j].phys1.position.v[0], bullet[j].phys1.position.v[1], threadID);
						if (bullet[j].phys1.position.v[0] > global::MainGame.MainGame.MaxRight || bullet[j].phys1.position.v[0] < global::MainGame.MainGame.MaxLeft || bullet[j].phys1.position.v[1] > global::MainGame.MainGame.MaxForward || bullet[j].phys1.position.v[1] < global::MainGame.MainGame.MaxRear || bullet[j].phys1.position.v[2] < distance2 || bullet[j].phys1.position.v[2] > global::MainGame.MainGame.MaxUp)
						{
							bulletActive[uBufferID, j] = 0;
							if (bullet[j].phys1.position.v[2] < distance2)
							{
								flag2 = true;
							}
						}
						else if (!mainC.collisionMain.Position_InsideBoundary(ref bullet[j].phys1))
						{
							bulletActive[uBufferID, j] = 2;
						}
					}
				}
				else if (bulletActive[uBufferID, j] == 2)
				{
					mainC.physicsMain.getPosition(ref bullet[j].phys1, global::MainGame.MainGame.frametime);
					distance2 = mainC.terrainMain.Get_Terrain_Height(bullet[j].phys1.position.v[0], bullet[j].phys1.position.v[1], threadID);
					if (bullet[j].phys1.position.v[0] > global::MainGame.MainGame.MaxRight || bullet[j].phys1.position.v[0] < global::MainGame.MainGame.MaxLeft || bullet[j].phys1.position.v[1] > global::MainGame.MainGame.MaxForward || bullet[j].phys1.position.v[1] < global::MainGame.MainGame.MaxRear || bullet[j].phys1.position.v[2] < distance2 || bullet[j].phys1.position.v[2] > global::MainGame.MainGame.MaxUp)
					{
						bulletActive[uBufferID, j] = 0;
						if (bullet[j].phys1.position.v[2] < distance2)
						{
							flag2 = true;
						}
					}
				}
				else
				{
					bulletActive[uBufferID, j] = 0;
				}
				if (bullet[j].playerID == 0 && global::MainGame.MainGame.bombViewEnabled && j == global::MainGame.MainGame.viewFollowingObjectID)
				{
					global::Rendering.Rendering.cameraSpeed = bullet[j].phys1.totalVelocity;
					global::Rendering.Rendering.camObjectGoal[uBufferID].X = bullet[j].phys1.position.v[0];
					global::Rendering.Rendering.camObjectGoal[uBufferID].Y = bullet[j].phys1.position.v[1];
					global::Rendering.Rendering.camObjectGoal[uBufferID].Z = bullet[j].phys1.position.v[2];
					global::Rendering.Rendering.camPosGoal[uBufferID].X = bullet[j].phys1.position.v[0] - bullet[j].mv[uBufferID].M21 * 20f + bullet[j].mv[uBufferID].M31 * 10f;
					global::Rendering.Rendering.camPosGoal[uBufferID].Y = bullet[j].phys1.position.v[1] - bullet[j].mv[uBufferID].M22 * 20f + bullet[j].mv[uBufferID].M32 * 10f;
					global::Rendering.Rendering.camPosGoal[uBufferID].Z = bullet[j].phys1.position.v[2] - bullet[j].mv[uBufferID].M23 * 20f + bullet[j].mv[uBufferID].M33 * 10f;
					if (bulletActive[uBufferID, j] == 0)
					{
						global::Rendering.Rendering.camPosGoal[uBufferID].Z = 1500f;
					}
					global::Rendering.Rendering.camUp[uBufferID].X = bullet[j].mv[uBufferID].M31;
					global::Rendering.Rendering.camUp[uBufferID].Y = bullet[j].mv[uBufferID].M32;
					global::Rendering.Rendering.camUp[uBufferID].Z = bullet[j].mv[uBufferID].M33;
				}
				if (bulletActive[uBufferID, j] != 0)
				{
					break;
				}
				if (bullet[j].soundID > -1)
				{
					if (bullet[j].playerID == 0)
					{
						mainC.soundsMain.Stop_Priority_Sound(bullet[j].soundID);
					}
					else
					{
						mainC.soundsMain.Stop_Sound(bullet[j].soundID);
					}
				}
				if (flag2)
				{
					if (bullet[j].soundID2 > -1)
					{
						mainC.soundsMain.Play_Priority_Sound(ammo[bullet[j].soundID2].sound2, bulletBoxT[threadID].pos2.v[0], bulletBoxT[threadID].pos2.v[1], bulletBoxT[threadID].pos2.v[2], 0f, 0f, 0f);
					}
					apffwV1.v[0] = bulletBoxT[threadID].pos2.v[0];
					apffwV1.v[1] = bulletBoxT[threadID].pos2.v[1];
					apffwV1.v[2] = bulletBoxT[threadID].pos2.v[2];
					Splash_Damage_From_Weapon(bulletBoxT[threadID].pos1.v[0], bulletBoxT[threadID].pos1.v[1], bulletBoxT[threadID].pos1.v[2] + 20f, (short)j, bullet[j].playerID, bullet[j].ammoIndex, threadID);
					mainC.renderingMain.New_Particle_New(16, bulletBoxT[threadID].pos1.v[0], bulletBoxT[threadID].pos1.v[1], bulletBoxT[threadID].pos1.v[2], 1.5f, 0f, 0f, 0, threadID);
				}
				if (bullet[j].playerID == 0)
				{
					if (j == global::MainGame.MainGame.viewFollowingObjectID)
					{
						projectileViewTimer = true;
						viewFollowingTimer = 0.75f;
					}
					global::Players.Players.needToReload = !Is_Player_Weapon_Loaded(0);
				}
				break;
			}
			case 13:
				if (bulletActive[uBufferID, j] == 1)
				{
					num2 = (bulletBoxT[threadID].pos1.v[0] = bullet[j].phys1.position.v[0]);
					num3 = (bulletBoxT[threadID].pos1.v[1] = bullet[j].phys1.position.v[1]);
					num4 = (bulletBoxT[threadID].pos1.v[2] = bullet[j].phys1.position.v[2]);
					mainC.physicsMain.getPosition(ref bullet[j].phys1, global::MainGame.MainGame.frametime);
					bulletBoxT[threadID].pos2.v[0] = bullet[j].phys1.position.v[0];
					bulletBoxT[threadID].pos2.v[1] = bullet[j].phys1.position.v[1];
					bulletBoxT[threadID].pos2.v[2] = bullet[j].phys1.position.v[2];
					mainC.mapsMain.Set_Position_OutsideBoundary(ref bulletBoxT[threadID]);
					num5 = mainC.playersMain.Check_Player_Impact_Threaded(ref bulletBoxT[threadID], ref pID, ref pfbV1T[threadID], ref pfbV2T[threadID], radius, bullet[j].playerID, threadID);
					float num14 = bulletBoxT[threadID].pos2.v[0];
					float num15 = bulletBoxT[threadID].pos2.v[1];
					float num16 = bulletBoxT[threadID].pos2.v[2];
					num7 = bullet[j].phys1.position.v[0] - num2;
					num8 = bullet[j].phys1.position.v[1] - num3;
					num9 = bullet[j].phys1.position.v[2] - num4;
					float num13 = (float)Math.Sqrt(num7 * num7 + num8 * num8 + num9 * num9);
					if (num13 != 0f)
					{
						num7 /= num13;
						num8 /= num13;
						num9 /= num13;
					}
					num6 = num13;
					i = 0;
					num = -1;
					short returnValueZoneCheckIndex = 0;
					InitialRayStart.X = num2;
					InitialRayStart.Y = num3;
					InitialRayStart.Z = num4;
					InitialRayEnd.X = bullet[j].phys1.position.v[0];
					InitialRayEnd.Y = bullet[j].phys1.position.v[1];
					InitialRayEnd.Z = bullet[j].phys1.position.v[2];
					while (mainC.zonesMain.Check_Zones_For_Ray(num2, num3, num4, InitialRayEnd.X, InitialRayEnd.Y, InitialRayEnd.Z, 5f, returnValueZoneCheckIndex, 1, out returnValueZoneCheckIndex, out returnValueZoneCheckObjID))
					{
						int num11 = Zones.zones[returnValueZoneCheckObjID].zoneList.numObjects;
						for (int num10 = 0; num10 < num11; num10++)
						{
							if (mainC.collisionMain.Check_Polygon_Ray_Collision_Projectile(Zones.zones[returnValueZoneCheckObjID].zoneList.oList[num10], num, ref InitialRayStart, ref InitialRayEnd, ref Zones.zones[returnValueZoneCheckObjID].zoneList.matrixList[num10], out distance, out IntersectPosition, out IntersectNormal, out num, threadID) && (i == 0 || distance < num6) && (num5 == -1f || distance < num5))
							{
								num7 = IntersectNormal.X;
								num8 = IntersectNormal.Y;
								num9 = IntersectNormal.Z;
								num14 = IntersectPosition.X;
								num15 = IntersectPosition.Y;
								num16 = IntersectPosition.Z;
								bulletBoxT[threadID].pos1.v[0] = num14;
								bulletBoxT[threadID].pos1.v[1] = num15;
								bulletBoxT[threadID].pos1.v[2] = num16;
								bulletBoxT[threadID].pos2.v[0] = bulletBoxT[threadID].pos1.v[0];
								bulletBoxT[threadID].pos2.v[1] = bulletBoxT[threadID].pos1.v[1];
								bulletBoxT[threadID].pos2.v[2] = bulletBoxT[threadID].pos1.v[2];
								num6 = distance;
								objectID = Zones.zones[returnValueZoneCheckObjID].zoneList.gidList[num10];
								i = 8;
							}
						}
					}
					if (i == 8)
					{
						flag = true;
						if (num6 < num5 || num5 < 0f)
						{
							Splash_Damage_From_Weapon(bulletBoxT[threadID].pos1.v[0], bulletBoxT[threadID].pos1.v[1], bulletBoxT[threadID].pos1.v[2], (short)j, bullet[j].playerID, bullet[j].ammoIndex, threadID);
							bulletBoxT[threadID].pos1.v[0] = 0f - global::Collision.Collision.ccsVxT[threadID];
							bulletBoxT[threadID].pos1.v[1] = 0f - global::Collision.Collision.ccsVyT[threadID];
							bulletBoxT[threadID].pos1.v[2] = 0f - global::Collision.Collision.ccsVzT[threadID];
							Handle_Ballistic_Impact((ushort)bullet[j].playerID, (ushort)j, objectID, bullet[j].ammoIndex, threadID);
							mainC.soundsMain.Play_Sound("Paintball_Splat", bulletBoxT[threadID].pos2.v[0], bulletBoxT[threadID].pos2.v[1], bulletBoxT[threadID].pos2.v[2], 0f, 0f, 0f);
							mainC.renderingMain.Add_Particle(ammo[bullet[j].ammoIndex].particleID, num14, num15, num16, num7, num8, num9, 0f, 0f, 0f);
						}
						else if (pID > -1)
						{
							mainC.soundsMain.Play_Sound("Paintball_Splat", global::Players.Players.players[pID].charP.position.v[0], global::Players.Players.players[pID].charP.position.v[1], global::Players.Players.players[pID].charP.position.v[2], global::Players.Players.players[pID].charP.velocity.v[0], global::Players.Players.players[pID].charP.velocity.v[1], global::Players.Players.players[pID].charP.velocity.v[2]);
							mainC.playersMain.Player_Hit(pID, bullet[j].playerID, global::Players.Players.players[pID].jointWasShot, ammo[bullet[j].ammoIndex].damage[global::Players.Players.players[pID].damageType], (ushort)j, pfbV2T[threadID], threadID);
							num14 = pfbV1T[threadID].v[0];
							num15 = pfbV1T[threadID].v[1];
							num16 = pfbV1T[threadID].v[2];
							num5 -= 0.3f;
							bulletBoxT[threadID].pos1.v[0] = num14;
							bulletBoxT[threadID].pos1.v[1] = num15;
							bulletBoxT[threadID].pos1.v[2] = num16;
							bulletBoxT[threadID].pos2.v[0] = 0f - bullet[j].phys1.velocity.v[0];
							bulletBoxT[threadID].pos2.v[1] = 0f - bullet[j].phys1.velocity.v[1];
							bulletBoxT[threadID].pos2.v[2] = 0f - bullet[j].phys1.velocity.v[2];
							mainC.renderingMain.Add_Particle(ammo[bullet[j].ammoIndex].particleID, num14, num15, num16, num7, num8, num9, global::Players.Players.players[pID].charP.velocity.v[0], global::Players.Players.players[pID].charP.velocity.v[1], global::Players.Players.players[pID].charP.velocity.v[2]);
							if (bullet[j].playerID == 0 && (global::Players.Players.players[pID].teamMask & global::Players.Players.enemyTeamMask) != 0)
							{
								showTargetCrosshairTimer = 0.25f;
							}
						}
						bulletActive[uBufferID, j] = 0;
					}
					else if (num5 >= 0f)
					{
						Splash_Damage_From_Weapon(bulletBoxT[threadID].pos1.v[0], bulletBoxT[threadID].pos1.v[1], bulletBoxT[threadID].pos1.v[2], (short)j, bullet[j].playerID, bullet[j].ammoIndex, threadID);
						flag = true;
						mainC.soundsMain.Play_Sound("Paintball_Splat", global::Players.Players.players[pID].charP.position.v[0], global::Players.Players.players[pID].charP.position.v[1], global::Players.Players.players[pID].charP.position.v[2], global::Players.Players.players[pID].charP.velocity.v[0], global::Players.Players.players[pID].charP.velocity.v[1], global::Players.Players.players[pID].charP.velocity.v[2]);
						mainC.playersMain.Player_Hit(pID, bullet[j].playerID, global::Players.Players.players[pID].jointWasShot, ammo[bullet[j].ammoIndex].damage[global::Players.Players.players[pID].damageType], (ushort)j, pfbV2T[threadID], threadID);
						bulletActive[uBufferID, j] = 0;
						num14 = pfbV1T[threadID].v[0];
						num15 = pfbV1T[threadID].v[1];
						num16 = pfbV1T[threadID].v[2];
						num5 -= 0.3f;
						bulletBoxT[threadID].pos1.v[0] = num14;
						bulletBoxT[threadID].pos1.v[1] = num15;
						bulletBoxT[threadID].pos1.v[2] = num16;
						bulletBoxT[threadID].pos2.v[0] = 0f - bullet[j].phys1.velocity.v[0];
						bulletBoxT[threadID].pos2.v[1] = 0f - bullet[j].phys1.velocity.v[1];
						bulletBoxT[threadID].pos2.v[2] = 0f - bullet[j].phys1.velocity.v[2];
						mainC.renderingMain.Add_Particle(ammo[bullet[j].ammoIndex].particleID, num14, num15, num16, num7, num8, num9, global::Players.Players.players[pID].charP.velocity.v[0], global::Players.Players.players[pID].charP.velocity.v[1], global::Players.Players.players[pID].charP.velocity.v[2]);
						if (bullet[j].playerID == 0 && (global::Players.Players.players[pID].teamMask & global::Players.Players.enemyTeamMask) != 0)
						{
							showTargetCrosshairTimer = 0.25f;
						}
					}
					else if (i == 12)
					{
						bulletActive[uBufferID, j] = 0;
						mainC.renderingMain.Add_Particle(ammo[bullet[j].ammoIndex].particleID, num14, num15, num16, num7, num8, num9, 0f, 0f, 0f);
					}
					else
					{
						distance2 = mainC.terrainMain.Get_Terrain_Height(bullet[j].phys1.position.v[0], bullet[j].phys1.position.v[1], threadID);
						if (bullet[j].phys1.position.v[0] > global::MainGame.MainGame.MaxRight || bullet[j].phys1.position.v[0] < global::MainGame.MainGame.MaxLeft || bullet[j].phys1.position.v[1] > global::MainGame.MainGame.MaxForward || bullet[j].phys1.position.v[1] < global::MainGame.MainGame.MaxRear || bullet[j].phys1.position.v[2] < distance2 || bullet[j].phys1.position.v[2] > global::MainGame.MainGame.MaxUp)
						{
							bulletActive[uBufferID, j] = 0;
						}
						else if (!mainC.collisionMain.Position_InsideBoundary(ref bullet[j].phys1))
						{
							bulletActive[uBufferID, j] = 2;
						}
					}
					if (flag)
					{
						if (ammo[bullet[j].ammoIndex].explosionID > -1)
						{
							mainC.Explosions.New_Explosion((byte)ammo[bullet[j].ammoIndex].explosionID, (ushort)bullet[j].playerID, num14, num15, num16, threadID);
						}
						int ammoIndex = ammo[bullet[j].ammoIndex].numBreakApartModels;
						for (i = 0; i < ammoIndex; i++)
						{
							mainC.renderingMain.New_Solid_Particle(1, num14, num15, num16, bullet[j].mv[uBufferID].M21, bullet[j].mv[uBufferID].M22, bullet[j].mv[uBufferID].M23, bullet[j].mv[uBufferID].M11, bullet[j].mv[uBufferID].M12, bullet[j].mv[uBufferID].M13, bullet[j].phys1.velocity.v[0], bullet[j].phys1.velocity.v[1], bullet[j].phys1.velocity.v[2], 10f, 0.13f, ammo[bullet[j].ammoIndex].breakApartModelList[i]);
						}
					}
				}
				else if (bulletActive[uBufferID, j] == 2)
				{
					mainC.physicsMain.getPosition(ref bullet[j].phys1, global::MainGame.MainGame.frametime);
					distance2 = mainC.terrainMain.Get_Terrain_Height(bullet[j].phys1.position.v[0], bullet[j].phys1.position.v[1], threadID);
					if (bullet[j].phys1.position.v[0] > global::MainGame.MainGame.MaxRight || bullet[j].phys1.position.v[0] < global::MainGame.MainGame.MaxLeft || bullet[j].phys1.position.v[1] > global::MainGame.MainGame.MaxForward || bullet[j].phys1.position.v[1] < global::MainGame.MainGame.MaxRear || bullet[j].phys1.position.v[2] < distance2 || bullet[j].phys1.position.v[2] > global::MainGame.MainGame.MaxUp)
					{
						bulletActive[uBufferID, j] = 0;
					}
				}
				else
				{
					bulletActive[uBufferID, j] = 0;
				}
				break;
			case 15:
			{
				bullet[j].timer -= global::MainGame.MainGame.frametime / global::Physics.Physics.timeMod;
				if (!(bullet[j].timer <= 0f))
				{
					break;
				}
				int num10 = (mainC.gameobjectMain.Melee_BreakApartItems((ushort)bullet[j].playerID, ammo[bullet[j].ammoIndex].length, bullet[j].ammoIndex, out objectID, out distance2) ? 1 : 0);
				int num11 = (mainC.playersMain.Check_Player_Impact_Melee((ushort)bullet[j].playerID, global::Players.Players.players[bullet[j].playerID].playerMeleeDistance, bullet[j].ammoIndex, out returnValueZoneCheckObjID, out distanceHit) ? 1 : 0);
				if (num10 == 1 && (num11 == 0 || distance2 < distanceHit))
				{
					mainC.gameobjectMain.Game_Object_Shot((ushort)bullet[j].playerID, objectID, ammo[bullet[j].ammoIndex].damage[global::GameObjects.GameObjects.Game_Objects[objectID].damageType], isExplosion: false, threadID);
				}
				else if (num11 == 1)
				{
					num10 = bullet[j].playerID;
					distance2 = global::Players.Players.players[returnValueZoneCheckObjID].charP.position.v[0] - global::Players.Players.players[num10].charP.position.v[0];
					distanceHit = global::Players.Players.players[returnValueZoneCheckObjID].charP.position.v[1] - global::Players.Players.players[num10].charP.position.v[1];
					float num12 = distance2 * distance2 + distanceHit * distanceHit;
					if (num12 != 0f)
					{
						distance2 /= num12;
						distanceHit /= num12;
					}
					global::Players.Players.players[returnValueZoneCheckObjID].impactX += distance2 * ammo[bullet[j].ammoIndex].mass * Math.Abs(bullet[j].phys1.velocity.v[0]);
					global::Players.Players.players[returnValueZoneCheckObjID].impactY += distanceHit * ammo[bullet[j].ammoIndex].mass * Math.Abs(bullet[j].phys1.velocity.v[1]);
					if (!global::Players.Players.players[returnValueZoneCheckObjID].dead && global::Players.Players.playerRaces[global::Players.Players.players[returnValueZoneCheckObjID].race].numBulletImpactAnimations > 0)
					{
						mainC.programsMain.Start_Animation(returnValueZoneCheckObjID, ref global::Players.Players.players[returnValueZoneCheckObjID].jt1, ref global::Players.Players.players[returnValueZoneCheckObjID].animations, global::Players.Players.players[returnValueZoneCheckObjID].programCollection, global::Players.Players.playerRaces[global::Players.Players.players[returnValueZoneCheckObjID].race].programBulletHit[global::Players.Players.players[returnValueZoneCheckObjID].type, global::Players.Players.players[returnValueZoneCheckObjID].curBulletHit], 1f, 1f);
						global::Players.Players.players[returnValueZoneCheckObjID].curBulletHit = (byte)((global::Players.Players.players[returnValueZoneCheckObjID].curBulletHit + 1) % global::Players.Players.playerRaces[global::Players.Players.players[returnValueZoneCheckObjID].race].numBulletImpactAnimations);
					}
					mainC.soundsMain.Play_Sound("Player_Hit", global::Players.Players.players[returnValueZoneCheckObjID].charP.position.v[0], global::Players.Players.players[returnValueZoneCheckObjID].charP.position.v[1], global::Players.Players.players[returnValueZoneCheckObjID].charP.position.v[2], global::Players.Players.players[returnValueZoneCheckObjID].charP.velocity.v[0], global::Players.Players.players[returnValueZoneCheckObjID].charP.velocity.v[1], global::Players.Players.players[returnValueZoneCheckObjID].charP.velocity.v[2]);
					mainC.playersMain.Player_Hit((short)returnValueZoneCheckObjID, bullet[j].playerID, global::Players.Players.players[returnValueZoneCheckObjID].jointWasShot, ammo[bullet[j].ammoIndex].damage[global::Players.Players.players[returnValueZoneCheckObjID].damageType], (ushort)j, pfbV2T[threadID], threadID);
					if (bullet[j].playerID == 0 && (global::Players.Players.players[returnValueZoneCheckObjID].teamMask & global::Players.Players.enemyTeamMask) != 0)
					{
						showTargetCrosshairTimer = 0.25f;
					}
				}
				bulletActive[uBufferID, j] = 0;
				break;
			}
			}
			if (bulletActive[uBufferID, j] != 0)
			{
				bullet[j].startX[uBufferID] = num2;
				bullet[j].startY[uBufferID] = num3;
				bullet[j].startZ[uBufferID] = num4;
				bullet[j].endX[uBufferID] = bullet[j].phys1.position.v[0];
				bullet[j].endY[uBufferID] = bullet[j].phys1.position.v[1];
				bullet[j].endZ[uBufferID] = bullet[j].phys1.position.v[2];
			}
		}
		i = 0;
		j = 0;
		for (; i < numLaserLights; i++)
		{
			if (laserLights[i] > -1)
			{
				laserLightsSorted[uBufferID, j] = (sbyte)i;
				j++;
			}
		}
		numActiveAmmoLights[uBufferID] = (byte)j;
	}

	public void Splash_Damage_From_Weapon(float startX, float startY, float startZ, short bulletID, short shooterID, byte amID, byte threadID)
	{
		ushort num = 0;
		ushort num2 = 0;
		ushort num3 = 0;
		float num4 = 0f;
		Vector3 InitialRayStart = default(Vector3);
		Vector3 InitialRayEnd = default(Vector3);
		Vector3 IntersectPosition = default(Vector3);
		Vector3 IntersectNormal = default(Vector3);
		float num5 = ammo[amID].splashFalloff * ammo[amID].splashFalloff;
		InitialRayStart.X = startX;
		InitialRayStart.Y = startY;
		InitialRayStart.Z = startZ;
		ushort returnValueZoneCheckObjID;
		float distance;
		int Number;
		for (byte b = 0; b < global::MainGame.MainGame.maxGamePlayers; b++)
		{
			if ((global::Players.Players.players[b].onmap & 0xC) > 0)
			{
				float num6 = global::Players.Players.players[b].charP.position.v[0] - startX;
				float num7 = global::Players.Players.players[b].charP.position.v[1] - startY;
				float num8 = global::Players.Players.players[b].charP.position.v[2] + 2.5f - startZ;
				float num9 = num6 * num6 + num7 * num7 + num8 * num8;
				if (num9 < num5)
				{
					int num10 = 0;
					num4 = num9;
					short returnValueZoneCheckIndex = 0;
					InitialRayEnd.X = global::Players.Players.players[b].charP.position.v[0];
					InitialRayEnd.Y = global::Players.Players.players[b].charP.position.v[1];
					InitialRayEnd.Z = global::Players.Players.players[b].charP.position.v[2] + 2.5f;
					while (num10 == 0 && mainC.zonesMain.Check_Zones_For_Point(startX, startY, startZ, 5f, returnValueZoneCheckIndex, 1, out returnValueZoneCheckIndex, out returnValueZoneCheckObjID))
					{
						ushort numObjects = Zones.zones[returnValueZoneCheckObjID].zoneList.numObjects;
						for (num2 = 0; num2 < numObjects; num2++)
						{
							if (mainC.collisionMain.Check_Polygon_Ray_Collision_Projectile(Zones.zones[returnValueZoneCheckObjID].zoneList.oList[num2], -1, ref InitialRayStart, ref InitialRayEnd, ref Zones.zones[returnValueZoneCheckObjID].zoneList.matrixList[num2], out distance, out IntersectPosition, out IntersectNormal, out Number, threadID) && distance * distance < num4)
							{
								num2 = numObjects;
								num10 = 8;
							}
						}
					}
					if (num10 == 0)
					{
						float num11 = (float)Math.Sqrt(num9);
						num6 /= num11;
						num7 /= num11;
						num8 /= num11;
						num9 /= num5;
						num9 *= num9;
						num11 = (1f - num9) * ammo[amID].mass;
						num9 = (1f - num9) * ammo[bullet[bulletID].ammoIndex].damage[global::Players.Players.players[b].damageType];
						num9 /= global::Players.Players.players[b].jt1[global::Players.Players.players[b].humanoidBackJoint].damageMultiplier;
						global::Players.Players.players[b].impactX += num11 * num6;
						global::Players.Players.players[b].impactY += num11 * num7;
						global::Players.Players.players[b].impactZ += num11 * num8;
						global::Players.Players.players[b].deathFlyBackPercentage = 1.5f;
						pfbV1T[threadID].v[0] = global::Players.Players.players[b].charP.position.v[0];
						pfbV1T[threadID].v[1] = global::Players.Players.players[b].charP.position.v[1];
						pfbV1T[threadID].v[2] = global::Players.Players.players[b].charP.position.v[2];
						mainC.playersMain.Player_Hit(b, shooterID, -1, num9, bulletID, pfbV1T[threadID], threadID);
						if (bullet[bulletID].playerID == 0 && (global::Players.Players.players[b].teamMask & global::Players.Players.enemyTeamMask) != 0)
						{
							showTargetCrosshairTimer = 0.25f;
						}
					}
				}
			}
		}
		for (ushort num12 = 0; num12 < global::GameObjects.GameObjects.numGameObjects; num12++)
		{
			if (global::GameObjects.GameObjects.Game_Objects[num12].state > 0)
			{
				float num6 = global::GameObjects.GameObjects.Game_Objects[num12].phy.x - startX;
				float num7 = global::GameObjects.GameObjects.Game_Objects[num12].phy.y - startY;
				float num8 = global::GameObjects.GameObjects.Game_Objects[num12].phy.z - startZ;
				float num9 = num6 * num6 + num7 * num7 + num8 * num8;
				if (num9 < num5)
				{
					int num10 = 0;
					num4 = num9;
					short returnValueZoneCheckIndex = 0;
					InitialRayEnd.X = global::GameObjects.GameObjects.Game_Objects[num12].phy.x;
					InitialRayEnd.Y = global::GameObjects.GameObjects.Game_Objects[num12].phy.y;
					InitialRayEnd.Z = global::GameObjects.GameObjects.Game_Objects[num12].phy.z;
					while (mainC.zonesMain.Check_Zones_For_Point(startX, startY, startZ, 5f, returnValueZoneCheckIndex, 1, out returnValueZoneCheckIndex, out returnValueZoneCheckObjID))
					{
						ushort numObjects = Zones.zones[returnValueZoneCheckObjID].zoneList.numObjects;
						for (num2 = 0; num2 < numObjects; num2++)
						{
							if (mainC.collisionMain.Check_Polygon_Ray_Collision_Projectile(Zones.zones[returnValueZoneCheckObjID].zoneList.oList[num2], -1, ref InitialRayStart, ref InitialRayEnd, ref Zones.zones[returnValueZoneCheckObjID].zoneList.matrixList[num2], out distance, out IntersectPosition, out IntersectNormal, out Number, threadID))
							{
								if (num10 == 0 || distance < num4)
								{
									num4 = distance;
									num = mainC.maingameMain.Get_Game_Item_Index(Zones.zones[returnValueZoneCheckObjID].zoneList.gidList[num2]);
									num3 = mainC.maingameMain.Get_Game_Item_Type(Zones.zones[returnValueZoneCheckObjID].zoneList.gidList[num2]);
								}
								num10 = 8;
							}
						}
					}
					if (num10 == 8 && num3 == 0 && num == num12)
					{
						num9 /= num5;
						num9 *= num9;
						num9 = (1f - num9) * ammo[bullet[bulletID].ammoIndex].damage[global::GameObjects.GameObjects.Game_Objects[num12].damageType];
						byte b2 = mainC.gameobjectMain.Game_Object_Shot((ushort)shooterID, num12, num9, isExplosion: true, threadID);
						if (shooterID == 0 && b2 == 1)
						{
							showTargetCrosshairTimer = 0.25f;
						}
					}
				}
			}
		}
	}

	public void Handle_Ballistic_Impact(ushort shooterID, ushort bulletID, ushort objectGid, byte ammoIndex, byte threadID)
	{
		if (mainC.maingameMain.Get_Game_Item_Type(objectGid) == 0)
		{
			ushort num = mainC.maingameMain.Get_Game_Item_Index(objectGid);
			byte b = mainC.gameobjectMain.Game_Object_Shot(shooterID, num, ammo[ammoIndex].damage[global::GameObjects.GameObjects.Game_Objects[num].damageType], isExplosion: false, threadID);
			if (b == 1 && shooterID == 0)
			{
				showTargetCrosshairTimer = 0.25f;
			}
		}
	}

	public void Render_Weapon(ushort weaponID, byte skinID, Matrix mvMount)
	{
		if (wp1[weaponID].numSkins <= skinID)
		{
			return;
		}
		global::Models.Models.mod1[wp1[weaponID].modelID].textureList[0] = wp1[weaponID].skins[skinID];
		mainC.modelsMain.Render_Model(wp1[weaponID].modelID, ref mvMount);
		ushort numAttachmentPoints = wp1[weaponID].numAttachmentPoints;
		for (ushort num = 0; num < numAttachmentPoints; num++)
		{
			if (wp1[weaponID].attachmentPoints[num].category == 0)
			{
				for (ushort num2 = 0; num2 < numWeaponAttachments; num2++)
				{
					if (wpnAttachments[num2].category == 0 && (wpnAttachments[num2].mount & wp1[weaponID].attachmentPoints[num].mount) != 0)
					{
						Matrix mv = Matrix.CreateTranslation(wp1[weaponID].attachmentPoints[num].x, wp1[weaponID].attachmentPoints[num].y, wp1[weaponID].attachmentPoints[num].z) * mvMount;
						mainC.modelsMain.Render_Model(wpnAttachments[num2].modID, ref mv);
						break;
					}
				}
			}
		}
	}

	public void Render_Player_Weapon(ushort playerID)
	{
		byte rBufferID = global::Rendering.Rendering.rBufferID;
		Matrix matrix = Matrix.CreateTranslation(global::Players.Players.players[playerID].posX[rBufferID], global::Players.Players.players[playerID].posY[rBufferID], global::Players.Players.players[playerID].posZ[rBufferID]);
		byte numMounts = Vehicles.vehicles[global::Players.Players.players[playerID].curVehicle].numMounts;
		for (byte b = 0; b < numMounts; b++)
		{
			if (global::MainGame.MainGame.playerVehicles[playerID].mounts[b].type == 1 && global::MainGame.MainGame.playerVehicles[playerID].mounts[b].objectAttached == 1)
			{
				byte objectID = global::MainGame.MainGame.playerVehicles[playerID].mounts[b].objectID;
				byte weaponID = global::MainGame.MainGame.playerVehicles[playerID].weapons[objectID].weaponID;
				if (wp1[weaponID].numSkins > 0)
				{
					Matrix mv = global::MainGame.MainGame.playerVehicles[playerID].mounts[b].mvCurrent[rBufferID] * matrix;
					global::Models.Models.mod1[wp1[weaponID].modelID].textureList[0] = wp1[weaponID].skins[global::MainGame.MainGame.playerVehicles[playerID].weapons[objectID].skinID];
					mainC.modelsMain.Render_Model(wp1[weaponID].modelID, ref mv);
					ushort num = 0;
					ushort numAttachmentPoints = wp1[weaponID].numAttachmentPoints;
					while (num < numAttachmentPoints)
					{
						if (global::MainGame.MainGame.playerVehicles[playerID].weapons[objectID].attachments[num].status == 1)
						{
							Matrix mv2 = Matrix.CreateTranslation(wp1[weaponID].attachmentPoints[num].x, wp1[weaponID].attachmentPoints[num].y, wp1[weaponID].attachmentPoints[num].z) * mv;
							mainC.modelsMain.Render_Model(wpnAttachments[global::MainGame.MainGame.playerVehicles[playerID].weapons[objectID].attachments[num].attachmentID].modID, ref mv2);
						}
						num++;
					}
				}
			}
		}
	}

	public void Render_Player_Weapons_ForDepth()
	{
		byte rBufferID = global::Rendering.Rendering.rBufferID;
		for (int i = 0; i < global::MainGame.MainGame.maxGamePlayers; i++)
		{
			if (global::Players.Players.players[i].active && !global::Players.Players.players[i].dead && (global::Players.Players.players[i].onmap & 0xE) > 0)
			{
				long num = global::Players.Players.players[i].weapon1.jointID;
				Matrix matrix = Matrix.CreateTranslation(global::Players.Players.players[i].posX[rBufferID], global::Players.Players.players[i].posY[rBufferID], global::Players.Players.players[i].posZ[rBufferID]);
				Matrix.Multiply(ref global::Players.Players.players[i].jt1[num].mv[rBufferID], ref matrix, out matrix);
				global::Rendering.Rendering.effect1.Parameters["World"].SetValue(matrix);
				global::Rendering.Rendering.effect1.CurrentTechnique.Passes[0].Apply();
				mainC.modelsMain.Render_Model_Basic(wp1[global::Players.Players.players[i].primaryWeaponMountWeapon].modelID);
			}
		}
	}

	public void Render_Weapons()
	{
		Render_Bullets();
	}

	public void Render_Bullets()
	{
		byte rBufferID = global::Rendering.Rendering.rBufferID;
		global::Rendering.Rendering.effect1.Parameters["AmmoLightAdjust"].SetValue(0f);
		global::Rendering.Rendering.rGraphics.RasterizerState = RasterizerState.CullClockwise;
		far4[0] = 0f;
		far4[1] = 0f;
		far4[2] = 0f;
		far4[3] = 1f;
		global::Rendering.Rendering.effect1.Parameters["LaserLightColor0"].SetValue(far4);
		far4[0] = 1f;
		far4[1] = 1f;
		far4[2] = 1f;
		far4[3] = 1f;
		global::Rendering.Rendering.effect1.Parameters["ColorAdjust"].SetValue(far4);
		for (int i = 0; i < 100; i++)
		{
			if (bulletActive[rBufferID, i] <= 0)
			{
				continue;
			}
			switch (bullet[i].ammoType)
			{
			case 2:
			{
				float num = bullet[i].endX[rBufferID] - bullet[i].startX[rBufferID];
				float num2 = bullet[i].endY[rBufferID] - bullet[i].startY[rBufferID];
				float num3 = bullet[i].endZ[rBufferID] - bullet[i].startZ[rBufferID];
				num3 = (float)Math.Sqrt(num * num + num2 * num2 + num3 * num3);
				Matrix mv = Matrix.CreateTranslation(bullet[i].startX[rBufferID], bullet[i].startY[rBufferID], bullet[i].startZ[rBufferID]);
				mv = Matrix.CreateRotationZ(bullet[i].phys1.angle.v[2] * ((float)Math.PI / 180f)) * mv;
				mv = Matrix.CreateRotationX((bullet[i].phys1.angle.v[0] + bullet[i].rotation) * ((float)Math.PI / 180f)) * mv;
				mv = Matrix.CreateTranslation(0f, num3, 0f) * mv;
				if ((num2 = num3 / 11.02f) > 1f)
				{
					mv = Matrix.CreateScale(1f, num2, 1f) * mv;
				}
				int num4 = ammo[bullet[i].ammoIndex].modelList[0];
				far4[0] = ammo[bullet[i].ammoIndex].color[0];
				far4[1] = ammo[bullet[i].ammoIndex].color[1];
				far4[2] = ammo[bullet[i].ammoIndex].color[2];
				global::Rendering.Rendering.effect1.Parameters["ColorAdjust"].SetValue(far4);
				far4[0] = ammo[bullet[i].ammoIndex].colorE[0];
				far4[1] = ammo[bullet[i].ammoIndex].colorE[1];
				far4[2] = ammo[bullet[i].ammoIndex].colorE[2];
				global::Rendering.Rendering.effect1.Parameters["Emissive"].SetValue(far4);
				mainC.modelsMain.Render_Model(num4, ref mv);
				global::Rendering.Rendering.effect1.Parameters["Emissive"].SetValue(global::Rendering.Rendering.ambient0);
				far4[0] = (far4[1] = (far4[2] = 1f));
				global::Rendering.Rendering.effect1.Parameters["ColorAdjust"].SetValue(far4);
				break;
			}
			case 3:
			case 11:
			{
				Matrix mv = Matrix.CreateTranslation(bullet[i].endX[rBufferID], bullet[i].endY[rBufferID], bullet[i].endZ[rBufferID]);
				mv = Matrix.CreateRotationZ(bullet[i].phys1.angle.v[2] * ((float)Math.PI / 180f)) * mv;
				mv = Matrix.CreateRotationX((bullet[i].phys1.angle.v[0] + bullet[i].rotation) * ((float)Math.PI / 180f)) * mv;
				mainC.modelsMain.Render_Model(ammo[bullet[i].ammoIndex].modelList[0], ref mv);
				break;
			}
			case 5:
				if (bullet[i].timer >= 0f)
				{
					Matrix mv = Matrix.CreateTranslation(bullet[i].endX[rBufferID], bullet[i].endY[rBufferID], bullet[i].endZ[rBufferID]);
					mv = Matrix.CreateRotationZ(bullet[i].phys1.angle.v[2] * ((float)Math.PI / 180f)) * mv;
					mv = Matrix.CreateRotationX((bullet[i].phys1.angle.v[0] + bullet[i].rotation) * ((float)Math.PI / 180f)) * mv;
					mainC.modelsMain.Render_Model(ammo[bullet[i].ammoIndex].modelList[0], ref mv);
				}
				break;
			case 4:
			{
				bullet[i].rotation -= global::MainGame.MainGame.frametime / global::Physics.Physics.timeMod * bullet[i].phys1.angularVelocity.v[0];
				Matrix mv = Matrix.CreateTranslation(bullet[i].endX[rBufferID], bullet[i].endY[rBufferID], bullet[i].endZ[rBufferID]);
				mv = Matrix.CreateRotationZ(bullet[i].phys1.angle.v[2] * ((float)Math.PI / 180f)) * mv;
				mv = Matrix.CreateRotationX((bullet[i].phys1.angle.v[0] + bullet[i].rotation) * ((float)Math.PI / 180f)) * mv;
				bullet[i].phys1.fx = mv.M21;
				bullet[i].phys1.fy = mv.M22;
				bullet[i].phys1.fz = mv.M23;
				mainC.modelsMain.Render_Model(ammo[bullet[i].ammoIndex].modelList[0], ref mv);
				break;
			}
			case 6:
			case 7:
			case 12:
			{
				Matrix mv = bullet[i].mv[rBufferID] * Matrix.CreateTranslation(bullet[i].endX[rBufferID], bullet[i].endY[rBufferID], bullet[i].endZ[rBufferID]);
				mainC.modelsMain.Render_Model(ammo[bullet[i].ammoIndex].modelList[0], ref mv);
				break;
			}
			case 8:
				if (bullet[i].timer > -10f)
				{
					Matrix mv = Matrix.CreateTranslation(bullet[i].endX[rBufferID], bullet[i].endY[rBufferID], bullet[i].endZ[rBufferID]);
					mv = Matrix.CreateRotationZ(bullet[i].phys1.angle.v[2] * ((float)Math.PI / 180f)) * mv;
					mv = Matrix.CreateRotationX((bullet[i].phys1.angle.v[0] + bullet[i].rotation) * ((float)Math.PI / 180f)) * mv;
					far4[0] = ammo[bullet[i].ammoIndex].colorE[0];
					far4[1] = ammo[bullet[i].ammoIndex].colorE[1];
					far4[2] = ammo[bullet[i].ammoIndex].colorE[2];
					global::Rendering.Rendering.effect1.Parameters["Emissive"].SetValue(far4);
					global::Rendering.Rendering.effect1.Parameters["Ambient"].SetValue(global::Rendering.Rendering.ambient0);
					mainC.modelsMain.Render_Model(ammo[bullet[i].ammoIndex].modelList[0], ref mv);
					global::Rendering.Rendering.effect1.Parameters["Ambient"].SetValue(global::Rendering.Rendering.ambientLevel);
					global::Rendering.Rendering.effect1.Parameters["Emissive"].SetValue(global::Rendering.Rendering.ambient0);
				}
				break;
			case 10:
			{
				Matrix mv = Matrix.CreateTranslation(bullet[i].endX[rBufferID], bullet[i].endY[rBufferID], bullet[i].endZ[rBufferID]);
				mv = Matrix.CreateRotationZ(bullet[i].phys1.angle.v[2] * ((float)Math.PI / 180f)) * mv;
				mv = Matrix.CreateRotationX((bullet[i].phys1.angle.v[0] + bullet[i].rotation) * ((float)Math.PI / 180f)) * mv;
				far4[0] = ammo[bullet[i].ammoIndex].color[0];
				far4[1] = ammo[bullet[i].ammoIndex].color[1];
				far4[2] = ammo[bullet[i].ammoIndex].color[2];
				global::Rendering.Rendering.effect1.Parameters["ColorAdjust"].SetValue(far4);
				far4[0] = ammo[bullet[i].ammoIndex].colorE[0];
				far4[1] = ammo[bullet[i].ammoIndex].colorE[1];
				far4[2] = ammo[bullet[i].ammoIndex].colorE[2];
				global::Rendering.Rendering.effect1.Parameters["Emissive"].SetValue(far4);
				mainC.modelsMain.Render_Model(ammo[bullet[i].ammoIndex].modelList[0], ref mv);
				global::Rendering.Rendering.effect1.Parameters["Emissive"].SetValue(global::Rendering.Rendering.ambient0);
				far4[0] = (far4[1] = (far4[2] = 1f));
				global::Rendering.Rendering.effect1.Parameters["ColorAdjust"].SetValue(far4);
				break;
			}
			case 13:
			{
				Matrix mv = Matrix.CreateTranslation(bullet[i].endX[rBufferID], bullet[i].endY[rBufferID], bullet[i].endZ[rBufferID]);
				mainC.modelsMain.Render_Model(ammo[bullet[i].ammoIndex].modelList[0], ref mv);
				break;
			}
			}
		}
		global::Rendering.Rendering.effect1.CurrentTechnique = global::Rendering.Rendering.effect1.Techniques["BasicNonTextured"];
		far4[0] = 1f;
		far4[1] = 0.95f;
		far4[2] = 0.6f;
		global::Rendering.Rendering.effect1.Parameters["ColorAdjust"].SetValue(far4);
		for (int i = 0; i < 100; i++)
		{
			if (bulletActive[rBufferID, i] <= 0)
			{
				continue;
			}
			switch (bullet[i].ammoType)
			{
			case 0:
			case 1:
			{
				float num2;
				float num3;
				float num;
				Matrix mv;
				if (bullet[i].tracer == 1)
				{
					num = bullet[i].endX[rBufferID] - bullet[i].startX[rBufferID];
					num2 = bullet[i].endY[rBufferID] - bullet[i].startY[rBufferID];
					num3 = bullet[i].endZ[rBufferID] - bullet[i].startZ[rBufferID];
					num3 = (float)Math.Sqrt(num * num + num2 * num2 + num3 * num3);
					mv = Matrix.CreateTranslation(bullet[i].startX[rBufferID], bullet[i].startY[rBufferID], bullet[i].startZ[rBufferID]);
					mv = Matrix.CreateRotationZ(bullet[i].phys1.angle.v[2] * ((float)Math.PI / 180f)) * mv;
					mv = Matrix.CreateRotationX((bullet[i].phys1.angle.v[0] + bullet[i].rotation) * ((float)Math.PI / 180f)) * mv;
					mv = Matrix.CreateTranslation(0f, num3, 0f) * mv;
					if (num3 > 3f)
					{
						num3 /= 1.5f;
						Matrix value = Matrix.CreateScale(1.5f, num3, 1.5f) * mv;
						global::Rendering.Rendering.effect1.Parameters["World"].SetValue(value);
						mainC.modelsMain.Render_Textured_Model_Basic(ammo[bullet[i].ammoIndex].modelList[0]);
					}
					else
					{
						global::Rendering.Rendering.effect1.Parameters["World"].SetValue(mv);
						mainC.modelsMain.Render_Textured_Model_Basic(ammo[bullet[i].ammoIndex].modelList[0]);
					}
					break;
				}
				num = bullet[i].endX[rBufferID] - bullet[i].startX[rBufferID];
				num2 = bullet[i].endY[rBufferID] - bullet[i].startY[rBufferID];
				num3 = bullet[i].endZ[rBufferID] - bullet[i].startZ[rBufferID];
				num3 = (float)Math.Sqrt(num * num + num2 * num2 + num3 * num3);
				mv = Matrix.CreateTranslation(bullet[i].startX[rBufferID], bullet[i].startY[rBufferID], bullet[i].startZ[rBufferID]);
				mv = Matrix.CreateRotationZ(bullet[i].phys1.angle.v[2] * ((float)Math.PI / 180f)) * mv;
				mv = Matrix.CreateRotationX((bullet[i].phys1.angle.v[0] + bullet[i].rotation) * ((float)Math.PI / 180f)) * mv;
				mv = Matrix.CreateTranslation(0f, num3, 0f) * mv;
				far4[0] = ammo[bullet[i].ammoIndex].color[0];
				far4[1] = ammo[bullet[i].ammoIndex].color[1];
				far4[2] = ammo[bullet[i].ammoIndex].color[2];
				global::Rendering.Rendering.effect1.Parameters["ColorAdjust"].SetValue(far4);
				far4[0] = 1f;
				far4[1] = 1f;
				far4[2] = 1f;
				global::Rendering.Rendering.effect1.Parameters["Emissive"].SetValue(far4);
				if (num3 > 3f)
				{
					num = Math.Abs(mv.M21 * global::Rendering.Rendering.eyeVec[rBufferID].v[0] + mv.M22 * global::Rendering.Rendering.eyeVec[rBufferID].v[1] + mv.M23 * global::Rendering.Rendering.eyeVec[rBufferID].v[2]);
					num = num * num * num * num;
					if (num >= 0.5f)
					{
						mv = Matrix.CreateScale(1f, num3 / 1.5f, 1f) * mv;
						global::Models.Models.mod1[ammo[bullet[i].ammoIndex].modelList[0]].defaultColor[3] = 0.5f;
						mainC.modelsMain.Render_Model(ammo[bullet[i].ammoIndex].modelList[0], ref mv);
						global::Models.Models.mod1[ammo[bullet[i].ammoIndex].modelList[0]].defaultColor[3] = 1f;
					}
				}
				else
				{
					mainC.modelsMain.Render_Model(ammo[bullet[i].ammoIndex].modelList[0], ref mv);
				}
				global::Rendering.Rendering.effect1.Parameters["Emissive"].SetValue(global::Rendering.Rendering.ambient0);
				far4[0] = (far4[1] = (far4[2] = 1f));
				global::Rendering.Rendering.effect1.Parameters["ColorAdjust"].SetValue(far4);
				break;
			}
			}
		}
		far4[0] = 0f;
		far4[1] = 0f;
		far4[2] = 0f;
		far4[3] = 1f;
		global::Rendering.Rendering.effect1.Parameters["LaserLightColor0"].SetValue(far4);
		global::Rendering.Rendering.effect1.Parameters["AmmoLightAdjust"].SetValue(0.6667f);
		far4[0] = (far4[1] = (far4[2] = 1f));
		global::Rendering.Rendering.effect1.Parameters["ColorAdjust"].SetValue(far4);
		global::Rendering.Rendering.effect1.CurrentTechnique = global::Rendering.Rendering.effect1.Techniques["Main"];
	}

	public void Render_Weapon_Mounts_Player()
	{
		byte rBufferID = global::Rendering.Rendering.rBufferID;
		for (ushort num = 0; num < numWeaponMounts; num++)
		{
			if (wpmMounts[num].type == 0)
			{
				mainC.modelsMain.Render_Model(wp1[wpmMounts[num].weaponID].modelID, ref wpmMounts[num].mv[rBufferID]);
			}
		}
	}

	public void Render_Rail_Gun_Particle_Stream(int bulletID, byte bufferID)
	{
		float length = ammo[bullet[bulletID].ammoIndex].length;
		if (length > 0f)
		{
			float num = bullet[bulletID].endX[bufferID];
			float num2 = bullet[bulletID].endY[bufferID];
			float num3 = bullet[bulletID].endZ[bufferID];
			float num4 = bullet[bulletID].startX[bufferID] - num;
			float num5 = bullet[bulletID].startY[bufferID] - num2;
			float num6 = bullet[bulletID].startZ[bufferID] - num3;
			float num7 = (float)Math.Sqrt(num4 * num4 + num5 * num5 + num6 * num6);
			if (num7 != 0f)
			{
				num4 = num4 / num7 * length;
				num5 = num5 / num7 * length;
				num6 = num6 / num7 * length;
			}
			int num8 = (int)(num7 / length);
			for (int i = 0; i < num8; i++)
			{
				mainC.renderingMain.Add_Particle(ammo[bullet[bulletID].ammoIndex].particleID3, num, num2, num3, 0f, 0f, 1f, 0f, 0f, 0f);
				num += num4;
				num2 += num5;
				num3 += num6;
			}
		}
	}

	public void Init_Weapons()
	{
		_ = global::Rendering.Rendering.uBufferID;
		for (int i = 0; i < 100; i++)
		{
			bulletActive[0, i] = 0;
			bulletActive[1, i] = 0;
			bullet[i].lightID = -1;
			bullet[i].phys1.initialTime = 0.0;
			bullet[i].phys1.angle.v[1] = 0f;
			bullet[i].phys1.acceleration.v[0] = 0f;
			bullet[i].phys1.acceleration.v[1] = 0f;
			bullet[i].phys1.acceleration.v[2] = -32.15223f;
		}
		laserLights = new sbyte[numLaserLights];
		laserLightsSorted = new sbyte[2, numLaserLights];
		for (int i = 0; i < numLaserLights; i++)
		{
			laserLights[i] = -1;
			laserLightsSorted[0, i] = -1;
			laserLightsSorted[1, i] = -1;
		}
		curLaserLight = 0;
		Load_Ammunition_Data("Ammunition.txt");
		Load_Ammo_Clips("Ammo_Clips.txt");
		Load_Attachment_Data("Weapon_Attachments.txt");
		Load_Weapons("weapons.txt");
		Load_Weapon_Modifiers("Weapon_Modifiers.txt");
		Load_Weapon_Attachments("Weapon_Attachments2.txt");
	}

	public void Load_Weapons(string filename)
	{
		_ = global::Rendering.Rendering.uBufferID;
		for (int i = 0; i < numWeapons; i++)
		{
			wp1[i].fireRate = 1f;
			wp1[i].autoReload = false;
			wp1[i].centerOfGravityAdjustmentX = 0f;
			wp1[i].CenterOfGravityAdjustmentY = 0f;
			wp1[i].IsHeld = true;
			wp1[i].unLimitedAmmo = false;
			wp1[i].modelID = 0;
			wp1[i].weaponLoc.acceleration.v[2] = 32.15223f;
			wp1[i].box.numP = 0L;
			wp1[i].box.numUsed = 0L;
			wp1[i].weaponLoc.initialTime = 0.0;
			wp1[i].gripType = 1;
			wp1[i].firstPersonViewX = (wp1[i].firstPersonViewY = (wp1[i].firstPersonViewZ = 0f));
			wp1[i].ironSightsViewX = (wp1[i].ironSightsViewY = (wp1[i].ironSightsViewZ = 0f));
			wp1[i].scopeViewX = (wp1[i].scopeViewY = (wp1[i].scopeViewZ = 0f));
			wp1[i].fireMode = 0;
			wp1[i].pfx1 = 2f;
			wp1[i].pfx2 = 20f;
			wp1[i].pfx3 = 0.075f;
			wp1[i].AnimationChamber = 0;
			wp1[i].AnimationReload = 0;
			wp1[i].AnimationThrow = 0;
			wp1[i].AnimationSpecial1 = 0;
			wp1[i].AnimationFire = 0;
			wp1[i].AnimationIronSights = -1;
			wp1[i].ChamberAfterShot = false;
			wp1[i].magazineCapacity = 10;
			wp1[i].snd_fire = null;
			wp1[i].snd_reload = null;
			wp1[i].snd_chamber = null;
			wp1[i].hasLaser = false;
			wp1[i].hasIronSights = false;
			wp1[i].numCrossHairs = 0;
			wp1[i].mountScope = 0;
			wp1[i].mountForeGrip = 0;
			wp1[i].mountBarrel = 0;
			wp1[i].mountEnergyDevice = 0;
			wp1[i].scopeID = 0;
			wp1[i].hudIcon = 0;
			wp1[i].secsPerBullet = 1f;
			wp1[i].IronSightsWileReloading = false;
			wp1[i].IronSightsWhileChambering = false;
			wp1[i].ScopeWileReloading = false;
			wp1[i].ScopeWhileChambering = false;
			wp1[i].fireRateAdjLowPerc = 1f;
			wp1[i].fireRateRecharge = 1f;
			wp1[i].fireRateReduction = 0f;
			wp1[i].maxAmmo = 0;
		}
		numWeapons = 0;
		Stream stream = TitleContainer.OpenStream("The_CoOp_Zombie_Game\\Config_Files\\" + filename);
		byte[] array = new byte[stream.Length];
		if (stream.CanRead)
		{
			long num = -2L;
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
				if (array4[0].Equals("numobjects", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 1;
				}
				else if (array4[0].Equals("object", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 2;
				}
				else if (array4[0].Equals("fire_rate", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 3;
				}
				else if (array4[0].Equals("weapon_file", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 4;
				}
				else if (array4[0].Equals("hudIcon", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 5;
				}
				else if (array4[0].Equals("autoReload", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 6;
				}
				else if (array4[0].Equals("Barrel_Start", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 7;
				}
				else if (array4[0].Equals("Barrel_End", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 8;
				}
				else if (array4[0].Equals("Hand", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 9;
				}
				else if (array4[0].Equals("Sound_Firing", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 10;
				}
				else if (array4[0].Equals("BoundingBox", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 11;
				}
				else if (array4[0].Equals("RecoilUp", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 12;
				}
				else if (array4[0].Equals("RecoilSide", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 13;
				}
				else if (array4[0].Equals("Barrel_Up", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 14;
				}
				else if (array4[0].Equals("Barrel_Right", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 15;
				}
				else if (array4[0].Equals("Sound_Reload", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 16;
				}
				else if (array4[0].Equals("Default_Ammo", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 17;
				}
				else if (array4[0].Equals("FirstPersonView", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 18;
				}
				else if (array4[0].Equals("FireModeSingleShot", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 19;
				}
				else if (array4[0].Equals("ParticleEffect", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 20;
				}
				else if (array4[0].Equals("WeaponName", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 21;
				}
				else if (array4[0].Equals("Reload", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 22;
				}
				else if (array4[0].Equals("Chamber", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 23;
				}
				else if (array4[0].Equals("Capacity", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 24;
				}
				else if (array4[0].Equals("ChamberAfterFire", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 25;
				}
				else if (array4[0].Equals("Holding", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 26;
				}
				else if (array4[0].Equals("FireModeMulti", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 27;
				}
				else if (array4[0].Equals("FireModeAutomatic", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 28;
				}
				else if (array4[0].Equals("HasLaser", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 29;
				}
				else if (array4[0].Equals("Sound_Chamber", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 30;
				}
				else if (array4[0].Equals("numberCrosshairs", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 31;
				}
				else if (array4[0].Equals("crossHairs", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 32;
				}
				else if (array4[0].Equals("scopeMount", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 33;
				}
				else if (array4[0].Equals("heatGeneration", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 34;
				}
				else if (array4[0].Equals("heatDissipation", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 35;
				}
				else if (array4[0].Equals("IronSightsView", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 36;
				}
				else if (array4[0].Equals("Throw", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 37;
				}
				else if (array4[0].Equals("Special1", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 38;
				}
				else if (array4[0].Equals("HasIronSights", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 39;
				}
				else if (array4[0].Equals("IronSightsWileReloading", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 40;
				}
				else if (array4[0].Equals("IronSightsWhileChambering", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 41;
				}
				else if (array4[0].Equals("ScopeWileReloading", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 42;
				}
				else if (array4[0].Equals("ScopeWhileChambering", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 43;
				}
				else if (array4[0].Equals("Sound_Empty", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 44;
				}
				else if (array4[0].Equals("Number_Barrels", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 45;
				}
				else if (array4[0].Equals("Fire_Rate_Adjusted_LowPerc", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 46;
				}
				else if (array4[0].Equals("Fire_Rate_Reduction", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 47;
				}
				else if (array4[0].Equals("Fire_Rate_Rate_Recharge", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 48;
				}
				else if (array4[0].Equals("Barrel_Particle_Distance", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 49;
				}
				else if (array4[0].Equals("MOA", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 50;
				}
				else if (array4[0].Equals("Animation_Fire", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 51;
				}
				else if (array4[0].Equals("NotHeld", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 52;
				}
				else if (array4[0].Equals("CenterOfGravityAdjustment", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 53;
				}
				else if (array4[0].Equals("UnlimitedAmmo", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 54;
				}
				else if (array4[0].Equals("maxHeat", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 55;
				}
				else if (array4[0].Equals("minCooling", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 56;
				}
				else if (array4[0].Equals("scopeID", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 57;
				}
				else if (array4[0].Equals("foreGripID", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 58;
				}
				else if (array4[0].Equals("barrelID", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 59;
				}
				else if (array4[0].Equals("energyDeviceID", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 60;
				}
				else if (array4[0].Equals("Attachment_Scope", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 61;
				}
				else if (array4[0].Equals("Attachment_ForeGrip", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 62;
				}
				else if (array4[0].Equals("Attachment_Barrel", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 63;
				}
				else if (array4[0].Equals("Attachment_EnergyDevice", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 64;
				}
				else if (array4[0].Equals("foreGripMount", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 65;
				}
				else if (array4[0].Equals("barrelMount", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 66;
				}
				else if (array4[0].Equals("energyDeviceMount", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 67;
				}
				else if (array4[0].Equals("skins", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 68;
				}
				else if (array4[0].Equals("skinIcons", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 69;
				}
				else if (array4[0].Equals("MovementFactor", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 70;
				}
				else if (array4[0].Equals("TurningFactor", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 71;
				}
				else if (array4[0].Equals("Animation_Walk", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 72;
				}
				else if (array4[0].Equals("Scope_View", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 73;
				}
				else if (array4[0].Equals("RecoilBack", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 74;
				}
				else if (array4[0].Equals("Animation_IronSights", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 75;
				}
				else if (array4[0].Equals("numAttachPoints", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 76;
				}
				else if (array4[0].Equals("attachPoint", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 77;
				}
				else if (array4[0].Equals("Animation_Run", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 78;
				}
				else if (array4[0].Equals("MaxAmmo", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 79;
				}
				else if (array4[0].Equals("Animation_Change_Weapon", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 80;
				}
				switch (num4)
				{
				case 1:
				{
					if (array4.Length <= 1)
					{
						break;
					}
					short num7 = short.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
					if (numAllocatedWeapons < num7)
					{
						wp1 = new StructsClass.weapon[num7];
						for (int i = 0; i < num7; i++)
						{
							wp1[i] = new StructsClass.weapon();
							StructsClass.Initialize_Weapon(ref wp1[i]);
						}
						numAllocatedWeapons = (byte)num7;
					}
					for (int i = 0; i < num7; i++)
					{
						wp1[i].fireRate = 1f;
						wp1[i].autoReload = false;
						wp1[i].modelID = global::Util.Util.maxUnsignedShortValue;
						wp1[i].IsHeld = true;
						wp1[i].unLimitedAmmo = false;
						wp1[i].centerOfGravityAdjustmentX = 0f;
						wp1[i].CenterOfGravityAdjustmentY = 0f;
						wp1[i].weaponLoc.acceleration.v[2] = 32.15223f;
						wp1[i].box.numP = 0L;
						wp1[i].box.numUsed = 0L;
						wp1[i].weaponLoc.initialTime = 0.0;
						wp1[i].gripType = 1;
						wp1[i].firstPersonViewX = (wp1[i].firstPersonViewY = (wp1[i].firstPersonViewZ = 0f));
						wp1[i].ironSightsViewX = (wp1[i].ironSightsViewY = (wp1[i].ironSightsViewZ = 0f));
						wp1[i].scopeViewX = (wp1[i].scopeViewY = (wp1[i].scopeViewZ = 0f));
						wp1[i].fireMode = 0;
						wp1[i].pfx1 = 2f;
						wp1[i].pfx2 = 20f;
						wp1[i].pfx3 = 0.075f;
						wp1[i].AnimationChamber = 0;
						wp1[i].AnimationReload = 0;
						wp1[i].AnimationThrow = 0;
						wp1[i].AnimationSpecial1 = 0;
						wp1[i].AnimationIronSights = 0;
						wp1[i].AnimationFire = 0;
						wp1[i].AnimationWalk = 0;
						wp1[i].AnimationRun = 0;
						wp1[i].AnimationChangeWeapon = 0;
						wp1[i].ChamberAfterShot = false;
						wp1[i].magazineCapacity = 10;
						wp1[i].snd_fire = null;
						wp1[i].snd_reload = null;
						wp1[i].snd_chamber = null;
						wp1[i].hasLaser = false;
						wp1[i].hasIronSights = false;
						wp1[i].numCrossHairs = 0;
						wp1[i].mountScope = 0;
						wp1[i].mountForeGrip = 0;
						wp1[i].mountBarrel = 0;
						wp1[i].mountEnergyDevice = 0;
						wp1[i].scopeID = 0;
						wp1[i].foreGripID = 0;
						wp1[i].barrelID = 0;
						wp1[i].energyDeviceID = 0;
						wp1[i].secsPerBullet = 1f;
						wp1[i].IronSightsWileReloading = false;
						wp1[i].IronSightsWhileChambering = false;
						wp1[i].ScopeWileReloading = false;
						wp1[i].ScopeWhileChambering = false;
						wp1[i].fireRateAdjLowPerc = 1f;
						wp1[i].fireRateRecharge = 1f;
						wp1[i].fireRateReduction = 0f;
						wp1[i].heatMax = 10;
						wp1[i].heatGeneration = 0f;
						wp1[i].heatDissipation = 0f;
						wp1[i].numSkins = 0;
						wp1[i].movementFactor = 1f;
						wp1[i].turningFactor = 1f;
						wp1[i].numAttachmentPoints = 0;
						wp1[i].maxAmmo = 0;
					}
					numWeapons = (byte)num7;
					break;
				}
				case 2:
					if (array4.Length > 1)
					{
						num = long.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 3:
					if (array4.Length > 1)
					{
						wp1[num].fireRate = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
						wp1[num].fireRate /= 60f;
						wp1[num].secsPerBullet = 1f / wp1[num].fireRate;
					}
					break;
				case 4:
					if (array4.Length > 1)
					{
						wp1[num].modName = array4[1];
						wp1[num].modelID = mainC.modelsMain.Find_Model(wp1[num].modName);
					}
					break;
				case 5:
					if (array4.Length > 1)
					{
						wp1[num].hudIcon = mainC.texturesMain.Find_Texture(array4[1], 0);
					}
					break;
				case 6:
					wp1[num].autoReload = true;
					break;
				case 7:
					if (array4.Length > 3 * wp1[num].numBarrels)
					{
						int i = 0;
						int num5 = 1;
						for (; i < wp1[num].numBarrels; i++)
						{
							wp1[num].offset[i, 0].v[0] = float.Parse(array4[num5++], CultureInfo.InvariantCulture.NumberFormat);
							wp1[num].offset[i, 0].v[1] = float.Parse(array4[num5++], CultureInfo.InvariantCulture.NumberFormat);
							wp1[num].offset[i, 0].v[2] = float.Parse(array4[num5++], CultureInfo.InvariantCulture.NumberFormat);
						}
					}
					break;
				case 8:
					if (array4.Length > 3 * wp1[num].numBarrels)
					{
						int i = 0;
						int num5 = 1;
						for (; i < wp1[num].numBarrels; i++)
						{
							wp1[num].offset[i, 1].v[0] = float.Parse(array4[num5++], CultureInfo.InvariantCulture.NumberFormat);
							wp1[num].offset[i, 1].v[1] = float.Parse(array4[num5++], CultureInfo.InvariantCulture.NumberFormat);
							wp1[num].offset[i, 1].v[2] = float.Parse(array4[num5++], CultureInfo.InvariantCulture.NumberFormat);
						}
					}
					break;
				case 9:
					wp1[num].gripType = (sbyte)(wp1[num].gripType | 2);
					break;
				case 10:
					if (array4.Length > 1)
					{
						wp1[num].snd_fire = array4[1];
					}
					break;
				case 11:
					if (array4.Length > 1)
					{
						int num5 = int.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
						wp1[num].box.numP = num5;
						wp1[num].box.numUsed = num5;
						wp1[num].box.v1 = new StructsClass.vtex[num5];
						int num6 = (array4.Length - 2) / 3;
						num6 = ((num6 >= num5) ? num5 : ((array4.Length - 2) / 3));
						for (int i = 0; i < num6; i++)
						{
							wp1[num].box.v1[i] = new StructsClass.vtex();
							wp1[num].box.v1[i].v[0] = float.Parse(array4[i * 3 + 2], CultureInfo.InvariantCulture.NumberFormat);
							wp1[num].box.v1[i].v[1] = float.Parse(array4[i * 3 + 3], CultureInfo.InvariantCulture.NumberFormat);
							wp1[num].box.v1[i].v[2] = float.Parse(array4[i * 3 + 4], CultureInfo.InvariantCulture.NumberFormat);
						}
					}
					break;
				case 12:
					if (array4.Length > 2)
					{
						wp1[num].recoilUp[0] = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
						wp1[num].recoilUp[1] = float.Parse(array4[2], CultureInfo.InvariantCulture.NumberFormat);
					}
					else
					{
						wp1[num].recoilUp[0] = 0f;
						wp1[num].recoilUp[1] = 0f;
					}
					break;
				case 13:
					if (array4.Length > 2)
					{
						wp1[num].recoilSide[0] = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
						wp1[num].recoilSide[1] = float.Parse(array4[2], CultureInfo.InvariantCulture.NumberFormat);
						wp1[num].recoilSide[2] = wp1[num].recoilSide[1] * 2f;
					}
					else
					{
						wp1[num].recoilSide[0] = 0f;
						wp1[num].recoilSide[1] = 0f;
						wp1[num].recoilSide[2] = 0f;
					}
					break;
				case 14:
					if (array4.Length > 3 * wp1[num].numBarrels)
					{
						int i = 0;
						int num5 = 1;
						for (; i < wp1[num].numBarrels; i++)
						{
							wp1[num].offset[i, 2].v[0] = float.Parse(array4[num5++], CultureInfo.InvariantCulture.NumberFormat);
							wp1[num].offset[i, 2].v[1] = float.Parse(array4[num5++], CultureInfo.InvariantCulture.NumberFormat);
							wp1[num].offset[i, 2].v[2] = float.Parse(array4[num5++], CultureInfo.InvariantCulture.NumberFormat);
						}
					}
					break;
				case 15:
					if (array4.Length > 3 * wp1[num].numBarrels)
					{
						int i = 0;
						int num5 = 1;
						for (; i < wp1[num].numBarrels; i++)
						{
							wp1[num].offset[i, 3].v[0] = float.Parse(array4[num5++], CultureInfo.InvariantCulture.NumberFormat);
							wp1[num].offset[i, 3].v[1] = float.Parse(array4[num5++], CultureInfo.InvariantCulture.NumberFormat);
							wp1[num].offset[i, 3].v[2] = float.Parse(array4[num5++], CultureInfo.InvariantCulture.NumberFormat);
						}
					}
					break;
				case 16:
					if (array4.Length > 1)
					{
						wp1[num].snd_reload = array4[1];
					}
					break;
				case 17:
					if (array4.Length > 1)
					{
						wp1[num].ammoIndex = sbyte.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 18:
					if (array4.Length > 3)
					{
						wp1[num].firstPersonViewX = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
						wp1[num].firstPersonViewY = float.Parse(array4[2], CultureInfo.InvariantCulture.NumberFormat);
						wp1[num].firstPersonViewZ = float.Parse(array4[3], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 19:
					wp1[num].fireMode = 1;
					break;
				case 20:
					if (array4.Length > 3)
					{
						wp1[num].pfx1 = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
						wp1[num].pfx2 = float.Parse(array4[2], CultureInfo.InvariantCulture.NumberFormat);
						wp1[num].pfx3 = float.Parse(array4[3], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 21:
				{
					if (array4.Length <= 1)
					{
						break;
					}
					for (int i = 1; i < array4.Length; i++)
					{
						if (i > 1)
						{
							wp1[num].weaponName = wp1[num].weaponName + " " + array4[i];
						}
						else
						{
							wp1[num].weaponName = array4[i];
						}
					}
					break;
				}
				case 22:
					if (array4.Length > 1)
					{
						wp1[num].AnimationReload = sbyte.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 23:
					if (array4.Length > 1)
					{
						wp1[num].AnimationChamber = sbyte.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 24:
					if (array4.Length > 1)
					{
						wp1[num].magazineCapacity = short.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 25:
					wp1[num].ChamberAfterShot = true;
					break;
				case 26:
					if (array4.Length > 1)
					{
						wp1[num].AnimationHolding = sbyte.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 27:
					wp1[num].fireMode = 2;
					break;
				case 28:
					wp1[num].fireMode = 0;
					break;
				case 29:
					wp1[num].hasLaser = true;
					break;
				case 30:
					if (array4.Length > 1)
					{
						wp1[num].snd_chamber = array4[1];
					}
					break;
				case 31:
					if (array4.Length > 1)
					{
						int i = byte.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
						wp1[num].numCrossHairs = (byte)i;
						wp1[num].crossHairs = new byte[i];
						for (i--; i > -1; i--)
						{
							wp1[num].crossHairs[i] = 0;
						}
					}
					break;
				case 32:
				{
					int i = wp1[num].numCrossHairs;
					if (array4.Length > i)
					{
						int num5 = 0;
						while (num5 < i)
						{
							wp1[num].crossHairs[num5] = byte.Parse(array4[++num5], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
						}
					}
					break;
				}
				case 33:
					if (array4.Length > 1)
					{
						wp1[num].mountScope = byte.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 34:
					if (array4.Length > 1)
					{
						wp1[num].heatGeneration = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 35:
					if (array4.Length > 1)
					{
						wp1[num].heatDissipation = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 36:
					if (array4.Length > 3)
					{
						wp1[num].ironSightsViewX = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
						wp1[num].ironSightsViewY = float.Parse(array4[2], CultureInfo.InvariantCulture.NumberFormat);
						wp1[num].ironSightsViewZ = float.Parse(array4[3], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 37:
					if (array4.Length > 1)
					{
						wp1[num].AnimationThrow = sbyte.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 38:
					if (array4.Length > 1)
					{
						wp1[num].AnimationSpecial1 = sbyte.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 39:
					wp1[num].hasIronSights = true;
					break;
				case 40:
					wp1[num].IronSightsWileReloading = true;
					break;
				case 41:
					wp1[num].IronSightsWhileChambering = true;
					break;
				case 42:
					wp1[num].ScopeWileReloading = true;
					break;
				case 43:
					wp1[num].ScopeWhileChambering = true;
					break;
				case 44:
					if (array4.Length > 1)
					{
						wp1[num].snd_fire_empty = array4[1];
					}
					break;
				case 45:
				{
					if (array4.Length <= 1)
					{
						break;
					}
					byte b = byte.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
					if (b > maxBarrels)
					{
						maxBarrels = b;
					}
					wp1[num].numBarrels = b;
					wp1[num].offset = new StructsClass.vtex[b, 10];
					for (int i = 0; i < b; i++)
					{
						for (int num5 = 0; num5 < 10; num5++)
						{
							wp1[num].offset[i, num5] = new StructsClass.vtex();
						}
					}
					break;
				}
				case 46:
					if (array4.Length > 1)
					{
						wp1[num].fireRateAdjLowPerc = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 47:
					if (array4.Length > 1)
					{
						wp1[num].fireRateReduction = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 48:
					if (array4.Length > 1)
					{
						wp1[num].fireRateRecharge = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 49:
					if (array4.Length > 1)
					{
						wp1[num].particleDistance = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 50:
					if (array4.Length > 1)
					{
						wp1[num].spread = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat) / 60f;
					}
					break;
				case 51:
					if (array4.Length > 1)
					{
						wp1[num].AnimationFire = ushort.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 52:
					wp1[num].IsHeld = false;
					break;
				case 53:
					if (array4.Length > 2)
					{
						wp1[num].centerOfGravityAdjustmentX = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
						wp1[num].CenterOfGravityAdjustmentY = float.Parse(array4[2], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 54:
					wp1[num].unLimitedAmmo = true;
					break;
				case 55:
					if (array4.Length > 1)
					{
						wp1[num].heatMax = byte.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 56:
					if (array4.Length > 1)
					{
						wp1[num].coolMin = byte.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 57:
					if (array4.Length > 1)
					{
						wp1[num].scopeID = byte.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 58:
					if (array4.Length > 1)
					{
						wp1[num].scopeID = byte.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 59:
					if (array4.Length > 1)
					{
						wp1[num].scopeID = byte.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 60:
					if (array4.Length > 1)
					{
						wp1[num].scopeID = byte.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 61:
					if (array4.Length > 3)
					{
						wp1[num].attachPointScopeX = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
						wp1[num].attachPointScopeY = float.Parse(array4[2], CultureInfo.InvariantCulture.NumberFormat);
						wp1[num].attachPointScopeZ = float.Parse(array4[3], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 62:
					if (array4.Length > 3)
					{
						wp1[num].attachPointForeGripX = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
						wp1[num].attachPointForeGripY = float.Parse(array4[2], CultureInfo.InvariantCulture.NumberFormat);
						wp1[num].attachPointForeGripZ = float.Parse(array4[3], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 63:
					if (array4.Length > 3)
					{
						wp1[num].attachPointBarrelX = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
						wp1[num].attachPointBarrelY = float.Parse(array4[2], CultureInfo.InvariantCulture.NumberFormat);
						wp1[num].attachPointBarrelZ = float.Parse(array4[3], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 64:
					if (array4.Length > 3)
					{
						wp1[num].attachPointEnergyDeviceX = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
						wp1[num].attachPointEnergyDeviceY = float.Parse(array4[2], CultureInfo.InvariantCulture.NumberFormat);
						wp1[num].attachPointEnergyDeviceZ = float.Parse(array4[3], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 65:
					if (array4.Length > 1)
					{
						wp1[num].mountForeGrip = byte.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 66:
					if (array4.Length > 1)
					{
						wp1[num].mountBarrel = byte.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 67:
					if (array4.Length > 1)
					{
						wp1[num].mountEnergyDevice = byte.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 68:
					if (array4.Length > 1)
					{
						byte b = (byte)(array4.Length - 1);
						wp1[num].numSkins = b;
						wp1[num].skins = new ushort[b];
						for (int num5 = 0; num5 < b; num5++)
						{
							wp1[num].skins[num5] = (ushort)mainC.texturesMain.Find_Texture(array4[num5 + 1], 0);
						}
					}
					break;
				case 69:
					if (array4.Length > 1)
					{
						byte b = (byte)(array4.Length - 1);
						wp1[num].skinIcons = new ushort[b];
						for (int num5 = 0; num5 < b; num5++)
						{
							wp1[num].skinIcons[num5] = (ushort)mainC.texturesMain.Find_Texture(array4[num5 + 1], 0);
						}
					}
					break;
				case 70:
					if (array4.Length > 1)
					{
						wp1[num].movementFactor = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 71:
					if (array4.Length > 1)
					{
						wp1[num].turningFactor = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 72:
					if (array4.Length > 1)
					{
						wp1[num].AnimationWalk = sbyte.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 73:
					if (array4.Length > 3)
					{
						wp1[num].scopeViewX = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
						wp1[num].scopeViewY = float.Parse(array4[2], CultureInfo.InvariantCulture.NumberFormat);
						wp1[num].scopeViewZ = float.Parse(array4[3], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 74:
					if (array4.Length > 2)
					{
						wp1[num].recoilBack[0] = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
						wp1[num].recoilBack[1] = float.Parse(array4[2], CultureInfo.InvariantCulture.NumberFormat);
					}
					else
					{
						wp1[num].recoilBack[0] = 0f;
						wp1[num].recoilBack[1] = 0f;
					}
					break;
				case 75:
					if (array4.Length > 1)
					{
						wp1[num].AnimationIronSights = sbyte.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 76:
					if (array4.Length > 1)
					{
						int num5 = byte.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
						wp1[num].numAttachmentPoints = (byte)num5;
						wp1[num].attachmentPoints = new StructsClass.Weapon_Attachment_Point[num5];
					}
					break;
				case 77:
					if (array4.Length > 6)
					{
						int num5 = byte.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
						if (num5 < wp1[num].numAttachmentPoints)
						{
							wp1[num].attachmentPoints[num5].x = float.Parse(array4[2], CultureInfo.InvariantCulture.NumberFormat);
							wp1[num].attachmentPoints[num5].y = float.Parse(array4[3], CultureInfo.InvariantCulture.NumberFormat);
							wp1[num].attachmentPoints[num5].z = float.Parse(array4[4], CultureInfo.InvariantCulture.NumberFormat);
							wp1[num].attachmentPoints[num5].mount = uint.Parse(array4[5], CultureInfo.InvariantCulture.NumberFormat);
							wp1[num].attachmentPoints[num5].category = byte.Parse(array4[6], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
						}
					}
					break;
				case 78:
					if (array4.Length > 1)
					{
						wp1[num].AnimationRun = sbyte.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 79:
					if (array4.Length > 1)
					{
						wp1[num].maxAmmo = byte.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 80:
					if (array4.Length > 1)
					{
						wp1[num].AnimationChangeWeapon = ushort.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				}
			}
		}
		stream.Close();
		for (int i = 0; i < numWeapons; i++)
		{
			if (wp1[i].numSkins < 1 && wp1[i].modelID < global::Models.Models.numModels)
			{
				wp1[i].numSkins = 1;
				wp1[i].skins = new ushort[1];
				wp1[i].skins[0] = (ushort)mainC.texturesMain.Find_Texture(global::Models.Models.mod1[wp1[i].modelID].texture, 0);
			}
			for (int num5 = 0; num5 < wp1[i].numBarrels; num5++)
			{
				float num8 = wp1[i].offset[num5, 1].v[0] - wp1[i].offset[num5, 0].v[0];
				float num9 = wp1[i].offset[num5, 1].v[1] - wp1[i].offset[num5, 0].v[1];
				float num10 = wp1[i].offset[num5, 1].v[2] - wp1[i].offset[num5, 0].v[2];
				float num11 = (float)Math.Sqrt(num8 * num8 + num9 * num9 + num10 * num10);
				if (num11 != 0f)
				{
					num8 /= num11;
					num9 /= num11;
					num10 /= num11;
				}
				wp1[i].offset[num5, 1].v[0] = wp1[i].offset[num5, 0].v[0] + num8;
				wp1[i].offset[num5, 1].v[1] = wp1[i].offset[num5, 0].v[1] + num9;
				wp1[i].offset[num5, 1].v[2] = wp1[i].offset[num5, 0].v[2] + num10;
			}
		}
		for (int i = 0; i < 44; i++)
		{
			global::Players.Players.players[i].weapon1.offset = new StructsClass.vtex[maxBarrels, 10];
			for (int num5 = 0; num5 < maxBarrels; num5++)
			{
				global::Players.Players.players[i].weapon1.offset[num5, 0] = new StructsClass.vtex();
				global::Players.Players.players[i].weapon1.offset[num5, 1] = new StructsClass.vtex();
				global::Players.Players.players[i].weapon1.offset[num5, 2] = new StructsClass.vtex();
				global::Players.Players.players[i].weapon1.offset[num5, 3] = new StructsClass.vtex();
				global::Players.Players.players[i].weapon1.offset[num5, 4] = new StructsClass.vtex();
				global::Players.Players.players[i].weapon1.offset[num5, 5] = new StructsClass.vtex();
				global::Players.Players.players[i].weapon1.offset[num5, 6] = new StructsClass.vtex();
				global::Players.Players.players[i].weapon1.offset[num5, 7] = new StructsClass.vtex();
				global::Players.Players.players[i].weapon1.offset[num5, 8] = new StructsClass.vtex();
				global::Players.Players.players[i].weapon1.offset[num5, 9] = new StructsClass.vtex();
			}
		}
	}

	public void Load_Ammunition_Data(string fileName)
	{
		int num = -1;
		Stream stream = TitleContainer.OpenStream("The_CoOp_Zombie_Game\\Config_Files\\" + fileName);
		byte[] array = new byte[stream.Length];
		if (stream.CanRead)
		{
			stream.Read(array, 0, array.Length);
			string text = mainC.utilMain.Byte_Array_To_String(array);
			string[] array2 = text.Split('\n', '\r');
			int i = 0;
			int num2 = 0;
			for (; i < array2.Length; i++)
			{
				if (array2[i].Length > 0)
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
			i = 0;
			num2 = 0;
			for (; i < array2.Length; i++)
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
				if (array4[0].Equals("num_objects", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 1;
				}
				if (array4[0].Equals("object", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 2;
				}
				else if (array4[0].Equals("Type", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 3;
				}
				else if (array4[0].Equals("Models", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 4;
				}
				else if (array4[0].Equals("muzzle_velocity", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 5;
				}
				else if (array4[0].Equals("deceleration", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 6;
				}
				else if (array4[0].Equals("shot_count", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 7;
				}
				else if (array4[0].Equals("spread", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 8;
				}
				else if (array4[0].Equals("Color", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 9;
				}
				else if (array4[0].Equals("ColorE", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 10;
				}
				else if (array4[0].Equals("ColorEIntensity", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 11;
				}
				else if (array4[0].Equals("AmmoAccelerationZ", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 12;
				}
				else if (array4[0].Equals("Damage", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 13;
				}
				else if (array4[0].Equals("Splash_Damage", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 14;
				}
				else if (array4[0].Equals("Length", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 15;
				}
				else if (array4[0].Equals("Timer", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 16;
				}
				else if (array4[0].Equals("Single", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 17;
				}
				else if (array4[0].Equals("Radius", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 18;
				}
				else if (array4[0].Equals("Sound_Fire", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 19;
				}
				else if (array4[0].Equals("LightingColor", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 20;
				}
				else if (array4[0].Equals("Sound_Hit", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 21;
				}
				else if (array4[0].Equals("particletimer", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 22;
				}
				else if (array4[0].Equals("BreakApartModels", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 23;
				}
				else if (array4[0].Equals("Mass", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 24;
				}
				else if (array4[0].Equals("ParticleType", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 25;
				}
				else if (array4[0].Equals("Explosion", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 26;
				}
				else if (array4[0].Equals("TimeBeforeRelease", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 27;
				}
				else if (array4[0].Equals("ParticleType2", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 28;
				}
				else if (array4[0].Equals("DeathFlyBackAnimationPercent", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 29;
				}
				else if (array4[0].Equals("ParticleType3", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 30;
				}
				switch (num4)
				{
				case 1:
					if (array4.Length > 1)
					{
						short num5 = short.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
						if (num5 > numAmmo)
						{
							ammo = new StructsClass.Ammunition[num5];
						}
						for (int k = numAmmo; k < num5; k++)
						{
							ammo[k] = new StructsClass.Ammunition();
						}
						numAmmo = num5;
					}
					break;
				case 2:
					num++;
					if (num > -1 && num < numAmmo)
					{
						ammo[num].deceleration = 0f;
						ammo[num].muzzleVelocity = 1000f;
						ammo[num].shotCount = 1;
						ammo[num].splash = 0;
						ammo[num].splashFalloff = 0f;
						ammo[num].spreadAngle = 0f;
						ammo[num].numModels = 0;
						ammo[num].numBreakApartModels = 0;
						ammo[num].timer = 0f;
						ammo[num].releaseTimer = 0f;
						ammo[num].single = false;
						ammo[num].colorIntensity = 0f;
						ammo[num].radius = 0f;
						ammo[num].mass = 0f;
						ammo[num].particleID = 0;
						ammo[num].particleID2 = 0;
						ammo[num].particleID3 = 0;
						ammo[num].explosionID = -1;
						for (int k = 0; k < 5; k++)
						{
							ammo[num].damage[k] = 0f;
						}
					}
					else
					{
						num = -1;
					}
					break;
				case 3:
					if (array4.Length > 1 && num > -1)
					{
						ammo[num].type = byte.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 4:
					if (array4.Length > 2 && num > -1)
					{
						ammo[num].numModels = byte.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
						ammo[num].modelList = new short[ammo[num].numModels];
						for (int k = 0; k < ammo[num].numModels; k++)
						{
							ammo[num].modelList[k] = 0;
						}
						for (int k = 0; k < ammo[num].numModels && k < array4.Length - 2; k++)
						{
							ammo[num].modelList[k] = (short)mainC.modelsMain.Find_Model(array4[k + 2]);
						}
					}
					break;
				case 5:
					if (array4.Length > 1 && num > -1)
					{
						ammo[num].muzzleVelocity = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 6:
					if (array4.Length > 1 && num > -1)
					{
						ammo[num].deceleration = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 7:
					if (array4.Length > 1 && num > -1)
					{
						ammo[num].shotCount = byte.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 8:
					if (array4.Length > 1 && num > -1)
					{
						ammo[num].spreadAngle = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 9:
					if (array4.Length > 4 && num > -1)
					{
						ammo[num].color[0] = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
						ammo[num].color[1] = float.Parse(array4[2], CultureInfo.InvariantCulture.NumberFormat);
						ammo[num].color[2] = float.Parse(array4[3], CultureInfo.InvariantCulture.NumberFormat);
						ammo[num].color[3] = float.Parse(array4[4], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 10:
					if (array4.Length > 4 && num > -1)
					{
						ammo[num].colorE[0] = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
						ammo[num].colorE[1] = float.Parse(array4[2], CultureInfo.InvariantCulture.NumberFormat);
						ammo[num].colorE[2] = float.Parse(array4[3], CultureInfo.InvariantCulture.NumberFormat);
						ammo[num].colorE[3] = float.Parse(array4[4], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 11:
					if (array4.Length > 1 && num > -1)
					{
						ammo[num].colorIntensity = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 12:
					if (array4.Length > 1 && num > -1)
					{
						ammo[num].accelerationZ = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 13:
					if (array4.Length > 2 && num > -1)
					{
						int num6 = int.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
						for (int k = 0; k < 5 && k < num6; k++)
						{
							ammo[num].damage[k] = float.Parse(array4[k + 2], CultureInfo.InvariantCulture.NumberFormat);
						}
					}
					break;
				case 14:
					if (array4.Length > 1 && num > -1)
					{
						ammo[num].splashFalloff = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 15:
					if (array4.Length > 1 && num > -1)
					{
						ammo[num].length = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 16:
					if (array4.Length > 1 && num > -1)
					{
						ammo[num].timer = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 17:
					ammo[num].single = true;
					break;
				case 18:
					ammo[num].radius = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
					break;
				case 19:
					ammo[num].sound = array4[1];
					break;
				case 20:
					if (array4.Length > 4 && num > -1)
					{
						ammo[num].lightColor[0] = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
						ammo[num].lightColor[1] = float.Parse(array4[2], CultureInfo.InvariantCulture.NumberFormat);
						ammo[num].lightColor[2] = float.Parse(array4[3], CultureInfo.InvariantCulture.NumberFormat);
						ammo[num].lightColor[3] = float.Parse(array4[4], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 21:
					ammo[num].sound2 = array4[1];
					break;
				case 22:
					if (array4.Length > 1 && num > -1)
					{
						ammo[num].particleTimer = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 23:
					if (array4.Length > 2 && num > -1)
					{
						ammo[num].numBreakApartModels = byte.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
						ammo[num].breakApartModelList = new short[ammo[num].numBreakApartModels];
						for (int k = 0; k < ammo[num].numBreakApartModels; k++)
						{
							ammo[num].breakApartModelList[k] = 0;
						}
						for (int k = 0; k < ammo[num].numBreakApartModels && k < array4.Length - 2; k++)
						{
							ammo[num].breakApartModelList[k] = (short)mainC.modelsMain.Find_Model(array4[k + 2]);
						}
					}
					break;
				case 24:
					if (array4.Length > 1 && num > -1)
					{
						ammo[num].mass = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 25:
					if (array4.Length > 1 && num > -1)
					{
						ammo[num].particleID = byte.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 26:
					if (array4.Length > 1 && num > -1)
					{
						ammo[num].explosionID = short.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 27:
					if (array4.Length > 1 && num > -1)
					{
						ammo[num].releaseTimer = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 28:
					if (array4.Length > 1 && num > -1)
					{
						ammo[num].particleID2 = byte.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 29:
					if (array4.Length > 1 && num > -1)
					{
						ammo[num].deathFlyBackPercentage = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 30:
					if (array4.Length > 1 && num > -1)
					{
						ammo[num].particleID3 = byte.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				}
			}
		}
		stream.Close();
	}

	public void Load_Ammo_Clips(string fileName)
	{
		int num = -1;
		Stream stream = TitleContainer.OpenStream("The_CoOp_Zombie_Game\\Config_Files\\" + fileName);
		byte[] array = new byte[stream.Length];
		if (stream.CanRead)
		{
			stream.Read(array, 0, array.Length);
			string text = mainC.utilMain.Byte_Array_To_String(array);
			string[] array2 = text.Split('\n', '\r');
			int i = 0;
			int num2 = 0;
			for (; i < array2.Length; i++)
			{
				if (array2[i].Length > 0)
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
			i = 0;
			num2 = 0;
			for (; i < array2.Length; i++)
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
				if (array4[0].Equals("num_objects", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 1;
				}
				if (array4[0].Equals("Clip", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 2;
				}
				if (array4[0].Equals("Ammo", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 3;
				}
				if (array4[0].Equals("Count", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 4;
				}
				if (array4[0].Equals("Size", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 5;
				}
				if (array4[0].Equals("startingNumber", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 6;
				}
				else if (array4[0].Equals("Max", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 7;
				}
				switch (num4)
				{
				case 1:
					if (array4.Length > 1)
					{
						short num5 = short.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
						if (num5 > numAmmoClips)
						{
							ammoClips = new StructsClass.Ammo_Clips[num5];
							numAmmoClips = (byte)num5;
						}
						curAmmoClips = (byte)num5;
					}
					break;
				case 2:
					if (array4.Length > 1)
					{
						num = int.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
					}
					if (num > -1 && num < curAmmoClips)
					{
						ammoClips[num].count = 2;
						ammoClips[num].size = 3;
						ammoClips[num].ammoIndex = 0;
						ammoClips[num].numClips = 2;
						ammoClips[num].startingNumClips = 2;
						ammoClips[num].maxCanCarry = 2;
					}
					else
					{
						num = -1;
					}
					break;
				case 3:
					if (array4.Length > 1 && num > -1)
					{
						ammoClips[num].ammoIndex = byte.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 4:
					if (array4.Length > 1 && num > -1)
					{
						ammoClips[num].count = ushort.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 5:
					if (array4.Length > 1 && num > -1)
					{
						ammoClips[num].size = byte.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 6:
					if (array4.Length > 1 && num > -1)
					{
						ammoClips[num].startingNumClips = ushort.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
						ammoClips[num].numClips = ammoClips[num].startingNumClips;
					}
					break;
				case 7:
					if (array4.Length > 1 && num > -1)
					{
						ammoClips[num].maxCanCarry = ushort.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				}
			}
		}
		stream.Close();
	}

	public void Load_Level_Data_Ammo_Clips(string fileName)
	{
		int num = -1;
		Stream stream = TitleContainer.OpenStream("The_CoOp_Zombie_Game\\Config_Files\\" + fileName);
		byte[] array = new byte[stream.Length];
		if (stream.CanRead)
		{
			stream.Read(array, 0, array.Length);
			string text = mainC.utilMain.Byte_Array_To_String(array);
			string[] array2 = text.Split('\n', '\r');
			int i = 0;
			int num2 = 0;
			for (; i < array2.Length; i++)
			{
				if (array2[i].Length > 0)
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
			i = 0;
			num2 = 0;
			for (; i < array2.Length; i++)
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
				if (array4[0].Equals("Clip", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 1;
				}
				else if (array4[0].Equals("startingNumber", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 2;
				}
				else if (array4[0].Equals("Max", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 3;
				}
				switch (num4)
				{
				case 1:
					if (array4.Length > 1)
					{
						num = int.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
					}
					if (num > -1 && num < curAmmoClips)
					{
						ammoClips[num].startingNumClips = 2;
						ammoClips[num].maxCanCarry = 2;
					}
					else
					{
						num = -1;
					}
					break;
				case 2:
					if (array4.Length > 1 && num > -1)
					{
						ammoClips[num].startingNumClips = ushort.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
						ammoClips[num].numClips = ammoClips[num].startingNumClips;
					}
					break;
				case 3:
					if (array4.Length > 1 && num > -1)
					{
						ammoClips[num].maxCanCarry = ushort.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				}
			}
		}
		stream.Close();
	}

	public void Load_Attachment_Data(string fileName)
	{
		byte b = 0;
		int num = -1;
		numScopes = 0;
		numForeGrips = 0;
		numBarrels = 0;
		numEnergyDevices = 0;
		Stream stream = TitleContainer.OpenStream("The_CoOp_Zombie_Game\\Config_Files\\" + fileName);
		byte[] array = new byte[stream.Length];
		if (stream.CanRead)
		{
			stream.Read(array, 0, array.Length);
			stream.Close();
			string text = mainC.utilMain.Byte_Array_To_String(array);
			string[] array2 = text.Split('\n', '\r');
			int i = 0;
			int num2 = 0;
			for (; i < array2.Length; i++)
			{
				if (array2[i].Length > 0)
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
			i = 0;
			num2 = 0;
			for (; i < array2.Length; i++)
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
				if (array4[0].Equals("numScopes", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 1;
				}
				else if (array4[0].Equals("Scope", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 2;
				}
				else if (array4[0].Equals("UnlockLevel", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 3;
				}
				else if (array4[0].Equals("Range", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 4;
				}
				else if (array4[0].Equals("ScopeViewAdj", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 5;
				}
				else if (array4[0].Equals("Mount", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 6;
				}
				else if (array4[0].Equals("Movement", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 7;
				}
				else if (array4[0].Equals("Turning", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 8;
				}
				else if (array4[0].Equals("Accuracy", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 9;
				}
				else if (array4[0].Equals("Model", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 10;
				}
				else if (array4[0].Equals("numForeGrips", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 20;
				}
				else if (array4[0].Equals("ForeGrip", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 21;
				}
				else if (array4[0].Equals("numBarrels", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 30;
				}
				else if (array4[0].Equals("Barrel", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 31;
				}
				else if (array4[0].Equals("MuzzleVelocity", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 32;
				}
				else if (array4[0].Equals("numEnergyDevices", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 40;
				}
				else if (array4[0].Equals("EnergyDevice", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 41;
				}
				else if (array4[0].Equals("Energy", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 42;
				}
				switch (num4)
				{
				case 1:
					if (array4.Length > 1)
					{
						short num5 = short.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
						scopes = new StructsClass.weapon_scope[num5];
						for (int k = 0; k < num5; k++)
						{
							scopes[k].modelName = "";
							scopes[k].modID = global::Util.Util.maxUnsignedShortValue;
							scopes[k].mount = 0;
							scopes[k].rangeLow = 0;
							scopes[k].rangeHigh = 0;
							scopes[k].scopeViewAdjX = 0f;
							scopes[k].scopeViewAdjY = 0f;
							scopes[k].scopeViewAdjZ = 0f;
							scopes[k].adjustmentAccuracy = 0f;
						}
						numScopes = (byte)num5;
					}
					break;
				case 2:
					if (array4.Length > 1)
					{
						num = int.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
						b = 1;
					}
					break;
				case 4:
					if (array4.Length > 2 && num > -1 && num < numScopes)
					{
						scopes[num].rangeLow = byte.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
						scopes[num].rangeHigh = byte.Parse(array4[2], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 5:
					if (array4.Length > 3 && num > -1 && num < numScopes)
					{
						scopes[num].scopeViewAdjX = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
						scopes[num].scopeViewAdjY = float.Parse(array4[2], CultureInfo.InvariantCulture.NumberFormat);
						scopes[num].scopeViewAdjZ = float.Parse(array4[3], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 3:
					if (array4.Length <= 1 || num <= -1)
					{
						break;
					}
					switch (b)
					{
					case 1:
						if (num < numScopes)
						{
							scopes[num].unlockLevel = byte.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
						}
						break;
					case 2:
						if (num < numForeGrips)
						{
							foreGrips[num].unlockLevel = byte.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
						}
						break;
					case 3:
						if (num < numBarrels)
						{
							barrels[num].unlockLevel = byte.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
						}
						break;
					case 4:
						if (num < numEnergyDevices)
						{
							energyDevices[num].unlockLevel = byte.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
						}
						break;
					}
					break;
				case 6:
					if (array4.Length <= 1 || num <= -1)
					{
						break;
					}
					switch (b)
					{
					case 1:
						if (num < numScopes)
						{
							scopes[num].mount = ushort.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
						}
						break;
					case 2:
						if (num < numForeGrips)
						{
							foreGrips[num].mount = ushort.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
						}
						break;
					case 3:
						if (num < numBarrels)
						{
							barrels[num].mount = ushort.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
						}
						break;
					case 4:
						if (num < numEnergyDevices)
						{
							energyDevices[num].mount = ushort.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
						}
						break;
					}
					break;
				case 7:
					if (array4.Length <= 1)
					{
						break;
					}
					switch (b)
					{
					case 1:
						if (num < numScopes)
						{
							scopes[num].adjustmentMovement = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
						}
						break;
					case 2:
						if (num < numForeGrips)
						{
							foreGrips[num].adjustmentMovement = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
						}
						break;
					case 3:
						if (num < numBarrels)
						{
							barrels[num].adjustmentMovement = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
						}
						break;
					case 4:
						if (num < numEnergyDevices)
						{
							energyDevices[num].adjustmentMovement = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
						}
						break;
					}
					break;
				case 8:
					if (array4.Length <= 1)
					{
						break;
					}
					switch (b)
					{
					case 1:
						if (num < numScopes)
						{
							scopes[num].adjustmentTurning = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
						}
						break;
					case 2:
						if (num < numForeGrips)
						{
							foreGrips[num].adjustmentTurning = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
						}
						break;
					case 3:
						if (num < numBarrels)
						{
							barrels[num].adjustmentTurning = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
						}
						break;
					case 4:
						if (num < numEnergyDevices)
						{
							energyDevices[num].adjustmentTurning = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
						}
						break;
					}
					break;
				case 9:
					if (array4.Length <= 1)
					{
						break;
					}
					switch (b)
					{
					case 1:
						if (num < numScopes)
						{
							scopes[num].adjustmentAccuracy = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
						}
						break;
					case 2:
						if (num < numForeGrips)
						{
							foreGrips[num].adjustmentAccuracy = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
						}
						break;
					case 3:
						if (num < numBarrels)
						{
							barrels[num].adjustmentAccuracy = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
						}
						break;
					}
					break;
				case 10:
					if (array4.Length <= 1 || num <= -1)
					{
						break;
					}
					switch (b)
					{
					case 1:
						if (num < numScopes)
						{
							scopes[num].modelName = array4[1];
						}
						break;
					case 2:
						if (num < numForeGrips)
						{
							foreGrips[num].modelName = array4[1];
						}
						break;
					case 3:
						if (num < numBarrels)
						{
							barrels[num].modelName = array4[1];
						}
						break;
					case 4:
						if (num < numEnergyDevices)
						{
							energyDevices[num].modelName = array4[1];
						}
						break;
					}
					break;
				case 20:
					if (array4.Length > 1)
					{
						short num5 = short.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
						foreGrips = new StructsClass.weapon_foregrip[num5];
						for (int k = 0; k < num5; k++)
						{
							foreGrips[k].modelName = "";
							foreGrips[k].modID = global::Util.Util.maxUnsignedShortValue;
							foreGrips[k].mount = 0;
							foreGrips[k].adjustmentAccuracy = 0f;
							foreGrips[k].adjustmentMovement = 0f;
							foreGrips[k].adjustmentTurning = 0f;
						}
						numForeGrips = (byte)num5;
					}
					break;
				case 21:
					if (array4.Length > 1)
					{
						num = int.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
						b = 2;
					}
					break;
				case 30:
					if (array4.Length > 1)
					{
						short num5 = short.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
						barrels = new StructsClass.weapon_barrel[num5];
						for (int k = 0; k < num5; k++)
						{
							barrels[k].modelName = "";
							barrels[k].modID = global::Util.Util.maxUnsignedShortValue;
							barrels[k].mount = 0;
							barrels[k].adjustmentAccuracy = 0f;
							barrels[k].adjustmentMovement = 0f;
							barrels[k].adjustmentTurning = 0f;
							barrels[k].adjustmentMuzzleVelocity = 0f;
						}
						numBarrels = (byte)num5;
					}
					break;
				case 31:
					if (array4.Length > 1)
					{
						num = int.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
						b = 3;
					}
					break;
				case 32:
					if (array4.Length > 1 && num < numBarrels)
					{
						barrels[num].adjustmentMuzzleVelocity = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 40:
					if (array4.Length > 1)
					{
						short num5 = short.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
						energyDevices = new StructsClass.weapon_energydevice[num5];
						for (int k = 0; k < num5; k++)
						{
							energyDevices[k].modID = global::Util.Util.maxUnsignedShortValue;
							energyDevices[k].mount = 0;
							energyDevices[k].adjustmentMovement = 0f;
							energyDevices[k].adjustmentTurning = 0f;
							energyDevices[k].energy = 100f;
							energyDevices[k].modelName = "";
						}
						numEnergyDevices = (byte)num5;
					}
					break;
				case 41:
					if (array4.Length > 1)
					{
						num = int.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
						b = 4;
					}
					break;
				case 42:
					if (array4.Length > 1 && num < numBarrels)
					{
						energyDevices[num].energy = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				}
			}
		}
		for (int k = 0; k < numScopes; k++)
		{
			scopes[k].modID = mainC.modelsMain.Find_Model_If_Exists(scopes[k].modelName);
		}
		for (int k = 0; k < numForeGrips; k++)
		{
			foreGrips[k].modID = mainC.modelsMain.Find_Model_If_Exists(foreGrips[k].modelName);
		}
		for (int k = 0; k < numBarrels; k++)
		{
			barrels[k].modID = mainC.modelsMain.Find_Model_If_Exists(barrels[k].modelName);
		}
		for (int k = 0; k < numEnergyDevices; k++)
		{
			energyDevices[k].modID = mainC.modelsMain.Find_Model_If_Exists(energyDevices[k].modelName);
		}
		stream.Close();
	}

	public void Load_Weapon_Modifiers(string fileName)
	{
		int num = -1;
		numWeaponModifiers = 0;
		Stream stream = TitleContainer.OpenStream("The_CoOp_Zombie_Game\\Config_Files\\" + fileName);
		byte[] array = new byte[stream.Length];
		if (stream.CanRead)
		{
			stream.Read(array, 0, array.Length);
			stream.Close();
			string text = mainC.utilMain.Byte_Array_To_String(array);
			string[] array2 = text.Split('\n', '\r');
			int i = 0;
			int num2 = 0;
			for (; i < array2.Length; i++)
			{
				if (array2[i].Length > 0)
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
			i = 0;
			num2 = 0;
			for (; i < array2.Length; i++)
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
				if (array4[0].Equals("numObjects", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 1;
				}
				else if (array4[0].Equals("Modifier", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 2;
				}
				else if (array4[0].Equals("Amount", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 3;
				}
				else if (array4[0].Equals("TimeModLasts", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 4;
				}
				switch (num4)
				{
				case 1:
					if (array4.Length > 1)
					{
						short num5 = short.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
						wpnMod = new StructsClass.Weapon_Modifier[num5];
						for (int k = 0; k < num5; k++)
						{
							wpnMod[k] = default(StructsClass.Weapon_Modifier);
							wpnMod[k].amount = 1f;
							wpnMod[k].time = 0f;
						}
						numWeaponModifiers = (byte)num5;
						curModifierTime = new float[44, numWeaponModifiers];
					}
					break;
				case 2:
					if (array4.Length > 1)
					{
						num = int.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
						if (num > -1 && num < numWeaponModifiers)
						{
							byte mask = (byte)(1 << num);
							wpnMod[num].mask = mask;
						}
					}
					break;
				case 3:
					if (array4.Length > 1 && num > -1 && num < numWeaponModifiers)
					{
						wpnMod[num].amount = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 4:
					if (array4.Length > 1 && num > -1 && num < numWeaponModifiers)
					{
						wpnMod[num].time = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				}
			}
		}
		stream.Close();
	}

	public void Load_Weapon_Mounts_Player(string fileName)
	{
		int num = -1;
		numWeaponMounts = 0;
		Stream stream = TitleContainer.OpenStream("The_CoOp_Zombie_Game\\Config_Files\\" + fileName);
		byte[] array = new byte[stream.Length];
		if (stream.CanRead)
		{
			stream.Read(array, 0, array.Length);
			stream.Close();
			string text = mainC.utilMain.Byte_Array_To_String(array);
			string[] array2 = text.Split('\n', '\r');
			int i = 0;
			int num2 = 0;
			for (; i < array2.Length; i++)
			{
				if (array2[i].Length > 0)
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
			i = 0;
			num2 = 0;
			for (; i < array2.Length; i++)
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
				if (array4[0].Equals("numObjects", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 1;
				}
				else if (array4[0].Equals("weaponMount", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 2;
				}
				else if (array4[0].Equals("type", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 3;
				}
				else if (array4[0].Equals("weapon", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 4;
				}
				else if (array4[0].Equals("position", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 5;
				}
				else if (array4[0].Equals("maxRotation", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 6;
				}
				else if (array4[0].Equals("minRotation", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 7;
				}
				else if (array4[0].Equals("rotation", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 8;
				}
				else if (array4[0].Equals("vehicle", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 9;
				}
				else if (array4[0].Equals("placementRot", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 10;
				}
				else if (array4[0].Equals("playerPosition", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 11;
				}
				else if (array4[0].Equals("playerRotationFactors", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 12;
				}
				else if (array4[0].Equals("turretRotationFactors", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 13;
				}
				else if (array4[0].Equals("turretRotationSpeedFactors", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 14;
				}
				switch (num4)
				{
				case 1:
					if (array4.Length > 1)
					{
						short num5 = short.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
						wpmMounts = new StructsClass.Weapon_Mount_Player[num5];
						for (int k = 0; k < num5; k++)
						{
							wpmMounts[k] = default(StructsClass.Weapon_Mount_Player);
							wpmMounts[k].type = 0;
							wpmMounts[k].weaponID = 0;
							wpmMounts[k].vehicleID = 0;
						}
						numWeaponMounts = (byte)num5;
					}
					break;
				case 2:
					if (array4.Length > 1)
					{
						num = int.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 3:
					if (array4.Length > 1 && num > -1 && num < numWeaponMounts)
					{
						wpmMounts[num].type = byte.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 4:
					if (array4.Length > 1 && num > -1 && num < numWeaponMounts)
					{
						wpmMounts[num].weaponID = byte.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 5:
					if (array4.Length > 3 && num > -1 && num < numWeaponMounts)
					{
						wpmMounts[num].posX = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
						wpmMounts[num].posY = float.Parse(array4[2], CultureInfo.InvariantCulture.NumberFormat);
						wpmMounts[num].posZ = float.Parse(array4[3], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 6:
					if (array4.Length > 3 && num > -1 && num < numWeaponMounts)
					{
						wpmMounts[num].maxRotX = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
						wpmMounts[num].maxRotY = float.Parse(array4[2], CultureInfo.InvariantCulture.NumberFormat);
						wpmMounts[num].maxRotZ = float.Parse(array4[3], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 7:
					if (array4.Length > 3 && num > -1 && num < numWeaponMounts)
					{
						wpmMounts[num].minRotX = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
						wpmMounts[num].minRotY = float.Parse(array4[2], CultureInfo.InvariantCulture.NumberFormat);
						wpmMounts[num].minRotZ = float.Parse(array4[3], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 8:
					if (array4.Length > 3 && num > -1 && num < numWeaponMounts)
					{
						wpmMounts[num].rotX = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
						wpmMounts[num].rotY = float.Parse(array4[2], CultureInfo.InvariantCulture.NumberFormat);
						wpmMounts[num].rotZ = float.Parse(array4[3], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 9:
					if (array4.Length > 1 && num > -1 && num < numWeaponMounts)
					{
						wpmMounts[num].vehicleID = byte.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 10:
					if (array4.Length > 3 && num > -1 && num < numWeaponMounts)
					{
						wpmMounts[num].oriRotX = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
						wpmMounts[num].oriRotY = float.Parse(array4[2], CultureInfo.InvariantCulture.NumberFormat);
						wpmMounts[num].oriRotZ = float.Parse(array4[3], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 11:
					if (array4.Length > 3 && num > -1 && num < numWeaponMounts)
					{
						wpmMounts[num].playerPosX = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
						wpmMounts[num].playerPosY = float.Parse(array4[2], CultureInfo.InvariantCulture.NumberFormat);
						wpmMounts[num].playerPosZ = float.Parse(array4[3], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 12:
					if (array4.Length > 3 && num > -1 && num < numWeaponMounts)
					{
						wpmMounts[num].playerRotFactorX = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
						wpmMounts[num].playerRotFactorY = float.Parse(array4[2], CultureInfo.InvariantCulture.NumberFormat);
						wpmMounts[num].playerRotFactorZ = float.Parse(array4[3], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 13:
					if (array4.Length > 3 && num > -1 && num < numWeaponMounts)
					{
						wpmMounts[num].turretRotFactorX = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
						wpmMounts[num].turretRotFactorY = float.Parse(array4[2], CultureInfo.InvariantCulture.NumberFormat);
						wpmMounts[num].turretRotFactorZ = float.Parse(array4[3], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 14:
					if (array4.Length > 3 && num > -1 && num < numWeaponMounts)
					{
						wpmMounts[num].turretSpeedFactorX = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
						wpmMounts[num].turretSpeedFactorY = float.Parse(array4[2], CultureInfo.InvariantCulture.NumberFormat);
						wpmMounts[num].turretSpeedFactorZ = float.Parse(array4[3], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				}
			}
		}
		for (int k = 0; k < numWeaponMounts; k++)
		{
			wpmMounts[k].mv = new Matrix[2];
			if (wpmMounts[k].type == 0)
			{
				if (wpmMounts[k].vehicleID < Vehicles.numVehicles)
				{
					wpmMounts[k].mvo = Matrix.CreateRotationY(wpmMounts[k].oriRotY * ((float)Math.PI / 180f)) * Matrix.CreateRotationX(wpmMounts[k].oriRotX * ((float)Math.PI / 180f)) * Matrix.CreateRotationZ(wpmMounts[k].oriRotZ * ((float)Math.PI / 180f)) * Matrix.CreateTranslation(wpmMounts[num].posX, wpmMounts[num].posY, wpmMounts[num].posZ) * Vehicles.vehicles[wpmMounts[k].vehicleID].mv[0] * Matrix.CreateTranslation(Vehicles.vehicles[wpmMounts[k].vehicleID].ph1.x, Vehicles.vehicles[wpmMounts[k].vehicleID].ph1.y, Vehicles.vehicles[wpmMounts[k].vehicleID].ph1.z);
					ref Matrix reference = ref wpmMounts[k].mv[0];
					reference = Matrix.CreateRotationY(wpmMounts[k].rotY * ((float)Math.PI / 180f)) * Matrix.CreateRotationX(wpmMounts[k].rotX * ((float)Math.PI / 180f)) * Matrix.CreateRotationZ(wpmMounts[k].rotZ * ((float)Math.PI / 180f)) * wpmMounts[k].mvo;
				}
			}
			else
			{
				wpmMounts[k].mvo = Matrix.CreateRotationY(wpmMounts[k].oriRotY * ((float)Math.PI / 180f)) * Matrix.CreateRotationX(wpmMounts[k].oriRotX * ((float)Math.PI / 180f)) * Matrix.CreateRotationZ(wpmMounts[k].oriRotZ * ((float)Math.PI / 180f));
				ref Matrix reference2 = ref wpmMounts[k].mv[0];
				reference2 = Matrix.CreateRotationY(wpmMounts[k].rotY * ((float)Math.PI / 180f)) * Matrix.CreateRotationX(wpmMounts[k].rotX * ((float)Math.PI / 180f)) * Matrix.CreateRotationZ(wpmMounts[k].rotZ * ((float)Math.PI / 180f)) * wpmMounts[k].mvo;
			}
			ref Matrix reference3 = ref wpmMounts[k].mv[1];
			reference3 = wpmMounts[k].mv[0];
		}
		stream.Close();
	}

	public void Load_Weapon_Attachments(string fileName)
	{
		ushort num = 0;
		Stream stream = TitleContainer.OpenStream("The_CoOp_Zombie_Game\\Config_Files\\" + fileName);
		byte[] array = new byte[stream.Length];
		if (stream.CanRead)
		{
			stream.Read(array, 0, array.Length);
			string text = mainC.utilMain.Byte_Array_To_String(array);
			string[] array2 = text.Split(new char[2] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
			int num2 = array2.Length;
			if (num2 < 1)
			{
				stream.Close();
				return;
			}
			for (int i = 0; i < num2; i++)
			{
				string[] array3 = array2[i].Split(new char[2] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
				if (array3.Length < 1)
				{
					continue;
				}
				int num3 = 0;
				if (array3[0].Equals("numItems", StringComparison.OrdinalIgnoreCase))
				{
					num3 = 1;
				}
				else if (array3[0].Equals("attachment", StringComparison.OrdinalIgnoreCase))
				{
					num3 = 2;
				}
				else if (array3[0].Equals("Model", StringComparison.OrdinalIgnoreCase))
				{
					num3 = 3;
				}
				else if (array3[0].Equals("category", StringComparison.OrdinalIgnoreCase))
				{
					num3 = 4;
				}
				else if (array3[0].Equals("mount", StringComparison.OrdinalIgnoreCase))
				{
					num3 = 5;
				}
				switch (num3)
				{
				case 1:
					if (array3.Length > 1)
					{
						numWeaponAttachments = ushort.Parse(array3[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
						wpnAttachments = new StructsClass.Weapon_Attachment[numWeaponAttachments];
						for (int j = 0; j < numWeaponAttachments; j++)
						{
							wpnAttachments[j].modelName = "";
							wpnAttachments[j].modID = global::Util.Util.maxUnsignedShortValue;
						}
					}
					break;
				case 2:
					if (array3.Length > 1)
					{
						num = ushort.Parse(array3[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 3:
					if (array3.Length > 1)
					{
						wpnAttachments[num].modelName = array3[1];
						wpnAttachments[num].modID = mainC.modelsMain.Find_Model(wpnAttachments[num].modelName);
					}
					break;
				case 4:
					if (array3.Length > 1)
					{
						wpnAttachments[num].category = byte.Parse(array3[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 5:
					if (array3.Length > 1)
					{
						wpnAttachments[num].mount = uint.Parse(array3[1], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				}
			}
		}
		stream.Close();
	}

	public void Switch_Weapons(ushort playerID, byte mountID, ushort newWeaponID)
	{
		mainC.vehicles.Set_Mount_Weapon(playerID, mountID, mainC.vehicles.Get_Player_Vehicle_Stub_Containing_Weapon(playerID, newWeaponID));
		if (playerID == 0)
		{
			Check_Weapon_Views();
		}
	}

	public void Set_MainPlayer_Weapon(byte playerID, sbyte wpnIndex, bool reset)
	{
		if (wpnIndex > -1 && wpnIndex < global::Players.Players.players[playerID].numAvailableWeapons && global::Players.Players.players[playerID].weaponList[wpnIndex] > -1)
		{
			sbyte b = global::Players.Players.players[playerID].weaponList[wpnIndex];
			global::Players.Players.players[playerID].wpnIndex = wpnIndex;
			global::Players.Players.players[playerID].weapon1.secsPerBullet = 1f / wp1[b].fireRate;
			sbyte animationHolding = wp1[b].AnimationHolding;
			global::Players.Players.players[playerID].playerIsMoving = 32766;
			global::Players.Players.players[playerID].programStationaryArms = animationHolding;
			if (reset)
			{
				Reset_Players_Weapon_Stub(ref global::Players.Players.players[playerID].weapon2[wpnIndex], (byte)wpnIndex, (byte)b, playerID);
			}
			global::Players.Players.players[playerID].weapon1.pfx1 = wp1[b].pfx1;
			global::Players.Players.players[playerID].weapon1.pfx2 = wp1[b].pfx2;
			global::Players.Players.players[playerID].weapon1.pfx3 = wp1[b].pfx3;
			global::Players.Players.fullyAutomatic = wp1[b].fireMode != 1;
			global::Players.Players.players[playerID].needToChamber = false;
			global::Players.Players.players[playerID].needToReload = false;
			if (global::Players.Players.players[playerID].weapon2[wpnIndex].currentRounds < 1)
			{
				global::Players.Players.players[playerID].needToReload = true;
			}
			else if (!global::Players.Players.players[playerID].weapon2[wpnIndex].roundChambered)
			{
				global::Players.Players.players[playerID].needToChamber = true;
			}
			if (playerID == 0)
			{
				global::MainGame.MainGame.showCrossHairs[0] = 0;
				global::Players.Players.scopeValue = 0;
				global::Players.Players.scopeViewAdj = 1f;
				global::MainGame.MainGame.usingScope = false;
				global::MainGame.MainGame.quickScope = false;
				global::Players.Players.currentView = global::Players.Players.lastView;
				global::Rendering.Rendering.hudIcon = wp1[b].hudIcon;
				global::Players.Players.needToChamber = global::Players.Players.players[playerID].needToChamber;
				global::Players.Players.needToReload = global::Players.Players.players[playerID].needToReload;
				Check_Weapon_Views();
			}
		}
	}

	public void nextWeapon()
	{
		sbyte b = (sbyte)(global::Players.Players.players[0].numAvailableWeapons - 1);
		sbyte b2 = b;
		sbyte b3 = (sbyte)(global::Players.Players.players[0].wpnIndex + 1);
		global::Players.Players.throwingGrenade = false;
		for (sbyte b4 = 0; b4 < b2; b4++)
		{
			if (b3 > b)
			{
				b3 = 0;
			}
			sbyte b5 = global::Players.Players.players[0].weaponList[b3];
			if (b5 > -1 && wp1[b5].IsHeld && (weaponAvailable & (1 << (int)b5)) > 0)
			{
				global::Players.Players.players[0].primaryWeaponMountWeapon = b5;
				global::Players.Players.players[0].wpnIndex = b3;
				byte ammoIndex = global::Players.Players.players[0].weapon2[b3].ammoIndex;
				global::MainGame.MainGame.showCrossHairs[0] = 0;
				global::Players.Players.scopeValue = 0;
				global::Players.Players.scopeViewAdj = 1f;
				global::MainGame.MainGame.usingScope = false;
				global::MainGame.MainGame.quickScope = false;
				global::Players.Players.currentView = global::Players.Players.lastView;
				global::Rendering.Rendering.hudIcon = wp1[b5].hudIcon;
				global::Players.Players.players[0].weapon1.secsPerBullet = 1f / wp1[b5].fireRate;
				b2 = wp1[b5].AnimationHolding;
				mainC.programsMain.Start_Animation(0, ref global::Players.Players.players[0].jt1, ref global::Players.Players.players[0].animations, global::Players.Players.players[0].programCollection, b2, 1f, 1f);
				global::Players.Players.players[0].playerIsMoving = 32766;
				global::Players.Players.players[0].programStationaryArms = b2;
				global::Players.Players.players[0].weapon2[b3].ammoAccelerationZ = ammo[ammoIndex].accelerationZ;
				global::Players.Players.players[0].weapon2[b3].muzzleVelocity = ammo[ammoIndex].muzzleVelocity;
				global::Players.Players.players[0].weapon2[b3].shotCount = ammo[ammoIndex].shotCount;
				global::Players.Players.players[0].weapon2[b3].fired = false;
				global::Players.Players.players[0].weapon2[b3].secsPerBullet = wp1[b5].secsPerBullet;
				global::Players.Players.players[0].weapon2[b3].fireRateAdjustment = 1f;
				global::Players.Players.players[0].weapon2[b3].accuracyAdjustment = 1f;
				global::Players.Players.players[0].weapon2[b3].muzzleVelocityAdjustment = 1f;
				global::Players.Players.players[0].weapon1.pfx1 = wp1[b5].pfx1;
				global::Players.Players.players[0].weapon1.pfx2 = wp1[b5].pfx2;
				global::Players.Players.players[0].weapon1.pfx3 = wp1[b5].pfx3;
				global::Players.Players.firstPersonViewX = global::Players.Players.players[0].weapon1.firstPersonViewX + global::Players.Players.firstPersonViewAdjX;
				global::Players.Players.firstPersonViewY = global::Players.Players.players[0].weapon1.firstPersonViewY + global::Players.Players.firstPersonViewAdjY;
				global::Players.Players.firstPersonViewZ = global::Players.Players.players[0].weapon1.firstPersonViewZ + global::Players.Players.firstPersonViewAdjZ;
				global::Players.Players.ironSightsViewX = global::Players.Players.players[0].weapon1.ironSightsViewX + global::Players.Players.ironSightsViewAdjX;
				global::Players.Players.ironSightsViewY = global::Players.Players.players[0].weapon1.ironSightsViewY + global::Players.Players.ironSightsViewAdjY;
				global::Players.Players.ironSightsViewZ = global::Players.Players.players[0].weapon1.ironSightsViewZ + global::Players.Players.ironSightsViewAdjZ;
				global::Players.Players.fullyAutomatic = wp1[b5].fireMode != 1;
				global::Players.Players.needToChamber = false;
				global::Players.Players.needToReload = false;
				if (global::Players.Players.players[0].weapon2[b3].currentRounds < 1)
				{
					global::Players.Players.needToReload = true;
				}
				else if (!global::Players.Players.players[0].weapon2[b3].roundChambered)
				{
					global::Players.Players.needToChamber = true;
				}
				Check_Weapon_Views();
				if (global::Networking.Networking.inGame && global::Networking.Networking.networkSession.SessionState == NetworkSessionState.Playing)
				{
					mainC.maingameMain.Send_Special_Messages(2);
				}
				break;
			}
			b3++;
		}
	}

	public void previousWeapon()
	{
		sbyte b = (sbyte)(global::Players.Players.players[0].numAvailableWeapons - 1);
		sbyte b2 = b;
		sbyte b3 = (sbyte)(global::Players.Players.players[0].wpnIndex - 1);
		global::Players.Players.throwingGrenade = false;
		for (sbyte b4 = 0; b4 < b2; b4++)
		{
			if (b3 < 0)
			{
				b3 = b;
			}
			sbyte b5 = global::Players.Players.players[0].weaponList[b3];
			if (b5 > -1 && wp1[b5].IsHeld && (weaponAvailable & (1 << (int)b5)) > 0)
			{
				global::Players.Players.players[0].primaryWeaponMountWeapon = b5;
				global::Players.Players.players[0].wpnIndex = b3;
				byte ammoIndex = global::Players.Players.players[0].weapon2[b3].ammoIndex;
				global::MainGame.MainGame.showCrossHairs[0] = 0;
				global::Players.Players.scopeValue = 0;
				global::Players.Players.scopeViewAdj = 1f;
				global::MainGame.MainGame.usingScope = false;
				global::MainGame.MainGame.quickScope = false;
				global::Players.Players.currentView = global::Players.Players.lastView;
				global::Rendering.Rendering.hudIcon = wp1[b5].hudIcon;
				global::Players.Players.players[0].weapon1.secsPerBullet = 1f / wp1[b5].fireRate;
				b2 = wp1[b5].AnimationHolding;
				mainC.programsMain.Start_Animation(0, ref global::Players.Players.players[0].jt1, ref global::Players.Players.players[0].animations, global::Players.Players.players[0].programCollection, b2, 1f, 1f);
				global::Players.Players.players[0].playerIsMoving = 32766;
				global::Players.Players.players[0].programStationaryArms = b2;
				global::Players.Players.players[0].weapon2[b3].ammoAccelerationZ = ammo[ammoIndex].accelerationZ;
				global::Players.Players.players[0].weapon2[b3].muzzleVelocity = ammo[ammoIndex].muzzleVelocity;
				global::Players.Players.players[0].weapon2[b3].shotCount = ammo[ammoIndex].shotCount;
				global::Players.Players.players[0].weapon2[b3].fired = false;
				global::Players.Players.players[0].weapon2[b3].secsPerBullet = wp1[b5].secsPerBullet;
				global::Players.Players.players[0].weapon2[b3].fireRateAdjustment = 1f;
				global::Players.Players.players[0].weapon2[b3].accuracyAdjustment = 1f;
				global::Players.Players.players[0].weapon2[b3].muzzleVelocityAdjustment = 1f;
				global::Players.Players.players[0].weapon1.pfx1 = wp1[b5].pfx1;
				global::Players.Players.players[0].weapon1.pfx2 = wp1[b5].pfx2;
				global::Players.Players.players[0].weapon1.pfx3 = wp1[b5].pfx3;
				global::Players.Players.fullyAutomatic = wp1[b5].fireMode != 1;
				global::Players.Players.firstPersonViewX = global::Players.Players.players[0].weapon1.firstPersonViewX + global::Players.Players.firstPersonViewAdjX;
				global::Players.Players.firstPersonViewY = global::Players.Players.players[0].weapon1.firstPersonViewY + global::Players.Players.firstPersonViewAdjY;
				global::Players.Players.firstPersonViewZ = global::Players.Players.players[0].weapon1.firstPersonViewZ + global::Players.Players.firstPersonViewAdjZ;
				global::Players.Players.ironSightsViewX = global::Players.Players.players[0].weapon1.ironSightsViewX + global::Players.Players.ironSightsViewAdjX;
				global::Players.Players.ironSightsViewY = global::Players.Players.players[0].weapon1.ironSightsViewY + global::Players.Players.ironSightsViewAdjY;
				global::Players.Players.ironSightsViewZ = global::Players.Players.players[0].weapon1.ironSightsViewZ + global::Players.Players.ironSightsViewAdjZ;
				global::Players.Players.needToChamber = false;
				global::Players.Players.needToReload = false;
				if (global::Players.Players.players[0].weapon2[b3].currentRounds < 1)
				{
					global::Players.Players.needToReload = true;
				}
				else if (!global::Players.Players.players[0].weapon2[b3].roundChambered)
				{
					global::Players.Players.needToChamber = true;
				}
				Check_Weapon_Views();
				if (global::Networking.Networking.inGame && global::Networking.Networking.networkSession.SessionState == NetworkSessionState.Playing)
				{
					mainC.maingameMain.Send_Special_Messages(2);
				}
				break;
			}
			b3--;
		}
	}

	public bool switchToWeapon(byte playerID, byte weaponID)
	{
		sbyte numAvailableWeapons = global::Players.Players.players[playerID].numAvailableWeapons;
		for (sbyte b = 0; b < numAvailableWeapons; b++)
		{
			if (global::Players.Players.players[playerID].weaponList[b] == (sbyte)weaponID)
			{
				sbyte b2 = global::Players.Players.players[playerID].weaponList[b];
				if ((weaponAvailable & (1 << (int)b2)) > 0)
				{
					global::Players.Players.players[playerID].primaryWeaponMountWeapon = b2;
					global::Players.Players.players[playerID].wpnIndex = b;
					byte ammoIndex = global::Players.Players.players[playerID].weapon2[b].ammoIndex;
					global::Players.Players.players[playerID].weapon1.secsPerBullet = 1f / wp1[b2].fireRate;
					sbyte animationHolding = wp1[b2].AnimationHolding;
					mainC.programsMain.Start_Animation(playerID, ref global::Players.Players.players[playerID].jt1, ref global::Players.Players.players[playerID].animations, global::Players.Players.players[playerID].programCollection, animationHolding, 1f, 1f);
					global::Players.Players.players[playerID].playerIsMoving = 32766;
					global::Players.Players.players[playerID].programStationaryArms = animationHolding;
					global::Players.Players.players[playerID].weapon2[b].ammoAccelerationZ = ammo[ammoIndex].accelerationZ;
					global::Players.Players.players[playerID].weapon2[b].muzzleVelocity = ammo[ammoIndex].muzzleVelocity;
					global::Players.Players.players[playerID].weapon2[b].shotCount = ammo[ammoIndex].shotCount;
					global::Players.Players.players[playerID].weapon2[b].fired = false;
					global::Players.Players.players[playerID].weapon2[b].secsPerBullet = wp1[b2].secsPerBullet;
					global::Players.Players.players[playerID].weapon2[b].fireRateAdjustment = 1f;
					global::Players.Players.players[playerID].weapon2[b].accuracyAdjustment = 1f;
					global::Players.Players.players[playerID].weapon2[b].muzzleVelocityAdjustment = 1f;
					global::Players.Players.players[playerID].weapon1.pfx1 = wp1[b2].pfx1;
					global::Players.Players.players[playerID].weapon1.pfx2 = wp1[b2].pfx2;
					global::Players.Players.players[playerID].weapon1.pfx3 = wp1[b2].pfx3;
					if (playerID == 0)
					{
						global::MainGame.MainGame.showCrossHairs[0] = 0;
						global::Players.Players.scopeValue = 0;
						global::Players.Players.scopeViewAdj = 1f;
						global::MainGame.MainGame.usingScope = false;
						global::MainGame.MainGame.quickScope = false;
						global::Players.Players.currentView = global::Players.Players.lastView;
						global::Rendering.Rendering.hudIcon = wp1[b2].hudIcon;
						global::Players.Players.fullyAutomatic = wp1[b2].fireMode != 1;
						global::Players.Players.firstPersonViewX = global::Players.Players.players[playerID].weapon1.firstPersonViewX + global::Players.Players.firstPersonViewAdjX;
						global::Players.Players.firstPersonViewY = global::Players.Players.players[playerID].weapon1.firstPersonViewY + global::Players.Players.firstPersonViewAdjY;
						global::Players.Players.firstPersonViewZ = global::Players.Players.players[playerID].weapon1.firstPersonViewZ + global::Players.Players.firstPersonViewAdjZ;
						global::Players.Players.ironSightsViewX = global::Players.Players.players[playerID].weapon1.ironSightsViewX + global::Players.Players.ironSightsViewAdjX;
						global::Players.Players.ironSightsViewY = global::Players.Players.players[playerID].weapon1.ironSightsViewY + global::Players.Players.ironSightsViewAdjY;
						global::Players.Players.ironSightsViewZ = global::Players.Players.players[playerID].weapon1.ironSightsViewZ + global::Players.Players.ironSightsViewAdjZ;
						global::Players.Players.needToChamber = false;
						global::Players.Players.needToReload = false;
						if (global::Players.Players.players[playerID].weapon2[b].currentRounds < 1)
						{
							global::Players.Players.needToReload = true;
						}
						else if (!global::Players.Players.players[playerID].weapon2[b].roundChambered)
						{
							global::Players.Players.needToChamber = true;
						}
						Check_Weapon_Views();
						if (global::Networking.Networking.inGame && global::Networking.Networking.networkSession.SessionState == NetworkSessionState.Playing)
						{
							mainC.maingameMain.Send_Special_Messages(2);
						}
					}
					return true;
				}
			}
		}
		return false;
	}

	public void Use_Weapon_Scope()
	{
		if (global::MainGame.MainGame.playerVehicles[0].weapons[global::MainGame.MainGame.playerVehicles[0].mounts[global::MainGame.MainGame.primaryWeaponMount].objectID].scopeLow > 0)
		{
			if (global::Players.Players.scopeValue > global::MainGame.MainGame.playerVehicles[0].weapons[global::MainGame.MainGame.playerVehicles[0].mounts[global::MainGame.MainGame.primaryWeaponMount].objectID].scopeHigh)
			{
				global::Players.Players.scopeValue = global::MainGame.MainGame.playerVehicles[0].weapons[global::MainGame.MainGame.playerVehicles[0].mounts[global::MainGame.MainGame.primaryWeaponMount].objectID].scopeHigh;
			}
			else if (global::Players.Players.scopeValue < global::MainGame.MainGame.playerVehicles[0].weapons[global::MainGame.MainGame.playerVehicles[0].mounts[global::MainGame.MainGame.primaryWeaponMount].objectID].scopeLow)
			{
				global::Players.Players.scopeValue = global::MainGame.MainGame.playerVehicles[0].weapons[global::MainGame.MainGame.playerVehicles[0].mounts[global::MainGame.MainGame.primaryWeaponMount].objectID].scopeLow;
			}
			global::MainGame.MainGame.usingScope = true;
			global::Players.Players.scopeViewAdj = 1f / (float)Math.Pow(2.0, (int)global::Players.Players.scopeValue);
			global::MainGame.MainGame.usingIronSights = false;
			global::Players.Players.currentView = 3;
			global::InputHandler.InputHandler.lookMode = 0;
			Check_Weapon_Views();
		}
	}

	public void Stop_Using_Weapon_Scope()
	{
		global::MainGame.MainGame.usingScope = false;
		global::MainGame.MainGame.quickScope = false;
		global::Players.Players.scopeValue = 0;
		global::Players.Players.scopeViewAdj = 1f;
		global::Players.Players.currentView = global::Players.Players.lastView;
		mainC.weaponsMain.Check_Weapon_Views();
	}

	public void Use_Iron_Sights()
	{
		global::MainGame.MainGame.usingIronSights = true;
		global::InputHandler.InputHandler.lookMode = 1;
		global::Players.Players.scopeViewAdj = 1f / (float)Math.Pow(2.0, 1.5);
		global::Players.Players.currentView = 2;
	}

	public void Stop_Using_Iron_Sights()
	{
		global::MainGame.MainGame.usingIronSights = false;
		global::InputHandler.InputHandler.lookMode = 0;
		global::Players.Players.currentView = global::Players.Players.lastView;
		global::Players.Players.scopeViewAdj = 1f / (float)Math.Pow(2.0, (int)global::Players.Players.scopeValue);
	}

	public void Stop_Using_Iron_Sights_Or_Weapon_Scope()
	{
		if (global::MainGame.MainGame.usingIronSights)
		{
			Stop_Using_Iron_Sights();
		}
		else if (global::MainGame.MainGame.usingScope)
		{
			Stop_Using_Weapon_Scope();
		}
	}

	public void Reload_Player_Weapons_Immediately(ushort playerID)
	{
		for (ushort num = 0; num < global::MainGame.MainGame.playerVehicles[playerID].numWeapons; num++)
		{
			if (global::MainGame.MainGame.playerVehicles[playerID].weapons[num].active && Player_Has_Ammo_For_Weapon(playerID, num) > 1)
			{
				Player_Vehicle_Weapon_Reloaded(playerID, num, ammoLoadedAlready: false, useSurplusFirst: true);
				global::MainGame.MainGame.playerVehicles[playerID].weapons[num].needToChamber = false;
			}
		}
	}

	public void Initialize_Ammo_Clip_Into_Player_Vehicle_Weapon(ushort vhID, byte stubID, byte ammoClipID, bool findAmmoClipID, ushort amount)
	{
		byte weaponID = global::MainGame.MainGame.playerVehicles[vhID].weapons[stubID].weaponID;
		byte b = (byte)wp1[weaponID].ammoIndex;
		if (ammoClipID >= curAmmoClips || findAmmoClipID)
		{
			ammoClipID = Find_Ammo_Clip(b);
		}
		if (ammo[b].single)
		{
			if (wp1[weaponID].maxAmmo > 0)
			{
				if (amount > wp1[weaponID].maxAmmo)
				{
					amount = wp1[weaponID].maxAmmo;
				}
				global::MainGame.MainGame.playerVehicles[vhID].weapons[stubID].currentRounds = amount;
				global::MainGame.MainGame.playerVehicles[vhID].weapons[stubID].magazineCapacity = wp1[weaponID].maxAmmo;
			}
			else
			{
				global::MainGame.MainGame.playerVehicles[vhID].weapons[stubID].currentRounds = (ushort)(ammoClips[ammoClipID].count * amount);
				if (global::MainGame.MainGame.playerVehicles[vhID].weapons[stubID].currentRounds > ammoClips[ammoClipID].maxCanCarry)
				{
					global::MainGame.MainGame.playerVehicles[vhID].weapons[stubID].currentRounds = ammoClips[ammoClipID].maxCanCarry;
				}
				global::MainGame.MainGame.playerVehicles[vhID].weapons[stubID].magazineCapacity = ammoClips[ammoClipID].maxCanCarry;
			}
		}
		else
		{
			global::MainGame.MainGame.playerVehicles[vhID].weapons[stubID].magazineCapacity = ammoClips[ammoClipID].count;
			if (amount > 1)
			{
				global::MainGame.MainGame.playerVehicles[vhID].weapons[stubID].magazineCapacity *= amount;
			}
			global::MainGame.MainGame.playerVehicles[vhID].weapons[stubID].currentRounds = (ushort)(ammoClips[ammoClipID].count * amount);
		}
		global::MainGame.MainGame.playerVehicles[vhID].weapons[stubID].curClip = ammoClipID;
		global::MainGame.MainGame.playerVehicles[vhID].weapons[stubID].needToReload = false;
		global::MainGame.MainGame.playerVehicles[vhID].weapons[stubID].needToChamber = false;
	}

	public byte Load_Ammo_Clip_Into_Player_Vehicle_Weapon(ushort playerID, ushort curStub, ushort amount)
	{
		ushort weaponID = global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].weaponID;
		byte result = Find_Ammo_Clip(global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].ammoIndex);
		byte curClip = global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].curClip;
		if (curClip < curAmmoClips && global::Players.Players.players[playerID].ammoClips[curClip].numClips > 0)
		{
			if (wp1[weaponID].ChamberAfterShot && !global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].roundChambered)
			{
				global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].needToChamber = true;
			}
			if (ammo[global::Players.Players.players[playerID].ammoClips[curClip].ammoIndex].single)
			{
				byte maxAmmo;
				if ((maxAmmo = wp1[weaponID].maxAmmo) > 0)
				{
					amount = (ushort)(maxAmmo - global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].currentRounds);
					if (global::Players.Players.players[playerID].ammoClips[curClip].numClips < amount)
					{
						amount = global::Players.Players.players[playerID].ammoClips[curClip].numClips;
					}
					global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].currentRounds += amount;
					global::Players.Players.players[playerID].ammoClips[curClip].numClips -= amount;
				}
				else
				{
					amount = global::Players.Players.players[playerID].ammoClips[curClip].numClips;
					global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].currentRounds += amount;
					if (global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].currentRounds > ammoClips[curClip].maxCanCarry)
					{
						global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].currentRounds = ammoClips[curClip].maxCanCarry;
					}
					global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].magazineCapacity = ammoClips[curClip].maxCanCarry;
					global::Players.Players.players[playerID].ammoClips[curClip].numClips -= amount;
				}
			}
			else
			{
				if (global::Players.Players.players[playerID].ammoClips[curClip].numClips < amount)
				{
					amount = global::Players.Players.players[playerID].ammoClips[curClip].numClips;
				}
				global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].magazineCapacity = global::Players.Players.players[playerID].ammoClips[curClip].count;
				if (amount > 1)
				{
					global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].magazineCapacity *= amount;
				}
				if (global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].currentRounds > 0)
				{
					global::Players.Players.players[playerID].ammoClips[curClip].surplus += global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].currentRounds;
					if (global::Players.Players.players[playerID].ammoClips[curClip].surplus >= global::Players.Players.players[playerID].ammoClips[curClip].count)
					{
						ushort num = (ushort)(global::Players.Players.players[playerID].ammoClips[curClip].surplus / global::Players.Players.players[playerID].ammoClips[curClip].count);
						global::Players.Players.players[playerID].ammoClips[curClip].numClips += num;
						global::Players.Players.players[playerID].ammoClips[curClip].surplus -= (ushort)(global::Players.Players.players[playerID].ammoClips[curClip].count * num);
					}
				}
				global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].currentRounds = (ushort)(global::Players.Players.players[playerID].ammoClips[curClip].count * amount);
				global::Players.Players.players[playerID].ammoClips[curClip].numClips -= amount;
			}
			global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].needToReload = false;
			mainC.gameLogic.Game_Ammo_Clip_Loaded(playerID, playerID, curStub);
			return curClip;
		}
		if (curClip < curAmmoClips && global::Players.Players.players[playerID].ammoClips[curClip].surplus > 0)
		{
			if (wp1[weaponID].ChamberAfterShot && !global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].roundChambered)
			{
				global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].needToChamber = true;
			}
			ushort num = (ushort)(global::Players.Players.players[playerID].ammoClips[curClip].count - global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].currentRounds);
			if (num > global::Players.Players.players[playerID].ammoClips[curClip].surplus)
			{
				num = global::Players.Players.players[playerID].ammoClips[curClip].surplus;
			}
			global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].currentRounds += num;
			global::Players.Players.players[playerID].ammoClips[curClip].surplus -= num;
			if (ammo[global::Players.Players.players[playerID].ammoClips[curClip].ammoIndex].single)
			{
				global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].magazineCapacity = ammoClips[curClip].maxCanCarry;
			}
			global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].needToReload = false;
			mainC.gameLogic.Game_Ammo_Clip_Loaded(playerID, playerID, curStub);
			return curClip;
		}
		for (curClip = 0; curClip < curAmmoClips; curClip++)
		{
			if (global::Players.Players.players[playerID].ammoClips[curClip].numClips > 0 && global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].ammoIndex == global::Players.Players.players[playerID].ammoClips[curClip].ammoIndex)
			{
				if (wp1[weaponID].ChamberAfterShot)
				{
					global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].needToChamber = true;
				}
				if (ammo[global::Players.Players.players[playerID].ammoClips[curClip].ammoIndex].single)
				{
					byte maxAmmo;
					if ((maxAmmo = wp1[weaponID].maxAmmo) > 0)
					{
						ushort num = (ushort)(maxAmmo - global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].currentRounds);
						if (num < amount)
						{
							amount = num;
						}
						global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].currentRounds += amount;
						global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].magazineCapacity = maxAmmo;
						global::Players.Players.players[playerID].ammoClips[curClip].numClips -= amount;
					}
					else
					{
						amount = global::Players.Players.players[playerID].ammoClips[curClip].numClips;
						global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].currentRounds += (ushort)(global::Players.Players.players[playerID].ammoClips[curClip].count * amount);
						if (global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].currentRounds > ammoClips[curClip].maxCanCarry)
						{
							global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].currentRounds = ammoClips[curClip].maxCanCarry;
						}
						global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].magazineCapacity = ammoClips[curClip].maxCanCarry;
						global::Players.Players.players[playerID].ammoClips[curClip].numClips -= amount;
					}
				}
				else
				{
					if (global::Players.Players.players[playerID].ammoClips[curClip].numClips < amount)
					{
						amount = global::Players.Players.players[playerID].ammoClips[curClip].numClips;
					}
					if (global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].currentRounds > 0)
					{
						global::Players.Players.players[playerID].ammoClips[curClip].surplus += global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].currentRounds;
						if (global::Players.Players.players[playerID].ammoClips[curClip].surplus >= global::Players.Players.players[playerID].ammoClips[curClip].count)
						{
							ushort num = (ushort)(global::Players.Players.players[playerID].ammoClips[curClip].surplus / global::Players.Players.players[playerID].ammoClips[curClip].count);
							global::Players.Players.players[playerID].ammoClips[curClip].numClips += num;
							global::Players.Players.players[playerID].ammoClips[curClip].surplus -= (ushort)(global::Players.Players.players[playerID].ammoClips[curClip].count * num);
						}
					}
					global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].currentRounds = (ushort)(global::Players.Players.players[playerID].ammoClips[curClip].count * amount);
					global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].magazineCapacity = global::Players.Players.players[playerID].ammoClips[curClip].count;
					if (amount > 1)
					{
						global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].magazineCapacity *= amount;
					}
					global::Players.Players.players[playerID].ammoClips[curClip].numClips -= amount;
				}
				global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].needToReload = false;
				mainC.gameLogic.Game_Ammo_Clip_Loaded(playerID, playerID, curStub);
				return curClip;
			}
		}
		return result;
	}

	public byte Load_Ammo_Clip_Surplus_Into_Player_Vehicle_Weapon(ushort playerID, ushort curStub, ushort amount)
	{
		ushort weaponID = global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].weaponID;
		byte result = Find_Ammo_Clip(global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].ammoIndex);
		byte curClip = global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].curClip;
		if (curClip < curAmmoClips && (global::Players.Players.players[playerID].ammoClips[curClip].surplus > 0 || global::Players.Players.players[playerID].ammoClips[curClip].numClips > 0))
		{
			if (wp1[weaponID].ChamberAfterShot && !global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].roundChambered)
			{
				global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].needToChamber = true;
			}
			if (ammo[global::Players.Players.players[playerID].ammoClips[curClip].ammoIndex].single)
			{
				byte maxAmmo;
				if ((maxAmmo = wp1[weaponID].maxAmmo) > 0)
				{
					amount = (ushort)(maxAmmo - global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].currentRounds);
					if (global::Players.Players.players[playerID].ammoClips[curClip].surplus > 0)
					{
						ushort num = amount;
						if (global::Players.Players.players[playerID].ammoClips[curClip].surplus < amount)
						{
							num = global::Players.Players.players[playerID].ammoClips[curClip].surplus;
						}
						global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].currentRounds += num;
						global::Players.Players.players[playerID].ammoClips[curClip].surplus -= num;
						amount -= num;
					}
					if (amount > 0)
					{
						if (global::Players.Players.players[playerID].ammoClips[curClip].numClips < amount)
						{
							amount = global::Players.Players.players[playerID].ammoClips[curClip].numClips;
						}
						global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].currentRounds += amount;
						global::Players.Players.players[playerID].ammoClips[curClip].numClips -= amount;
					}
				}
				else
				{
					amount = (ushort)(global::Players.Players.players[playerID].ammoClips[curClip].numClips + global::Players.Players.players[playerID].ammoClips[curClip].surplus);
					global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].currentRounds += amount;
					if (global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].currentRounds > ammoClips[curClip].maxCanCarry)
					{
						global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].currentRounds = ammoClips[curClip].maxCanCarry;
					}
					global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].magazineCapacity = ammoClips[curClip].maxCanCarry;
					global::Players.Players.players[playerID].ammoClips[curClip].numClips = 0;
					global::Players.Players.players[playerID].ammoClips[curClip].surplus = 0;
				}
			}
			else if (global::Players.Players.players[playerID].ammoClips[curClip].surplus > 0)
			{
				ushort num = (ushort)(global::Players.Players.players[playerID].ammoClips[curClip].count - global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].currentRounds);
				if (num > global::Players.Players.players[playerID].ammoClips[curClip].surplus)
				{
					num = global::Players.Players.players[playerID].ammoClips[curClip].surplus;
				}
				global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].currentRounds += num;
				global::Players.Players.players[playerID].ammoClips[curClip].surplus -= num;
				if (ammo[global::Players.Players.players[playerID].ammoClips[curClip].ammoIndex].single)
				{
					global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].magazineCapacity = ammoClips[curClip].maxCanCarry;
				}
			}
			else
			{
				if (global::Players.Players.players[playerID].ammoClips[curClip].numClips < amount)
				{
					amount = global::Players.Players.players[playerID].ammoClips[curClip].numClips;
				}
				global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].magazineCapacity = global::Players.Players.players[playerID].ammoClips[curClip].count;
				if (amount > 1)
				{
					global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].magazineCapacity *= amount;
				}
				if (global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].currentRounds > 0)
				{
					global::Players.Players.players[playerID].ammoClips[curClip].surplus += global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].currentRounds;
					if (global::Players.Players.players[playerID].ammoClips[curClip].surplus >= global::Players.Players.players[playerID].ammoClips[curClip].count)
					{
						ushort num = (ushort)(global::Players.Players.players[playerID].ammoClips[curClip].surplus / global::Players.Players.players[playerID].ammoClips[curClip].count);
						global::Players.Players.players[playerID].ammoClips[curClip].numClips += num;
						global::Players.Players.players[playerID].ammoClips[curClip].surplus -= (ushort)(global::Players.Players.players[playerID].ammoClips[curClip].count * num);
					}
				}
				global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].currentRounds = (ushort)(global::Players.Players.players[playerID].ammoClips[curClip].count * amount);
				global::Players.Players.players[playerID].ammoClips[curClip].numClips -= amount;
			}
			global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].needToReload = false;
			mainC.gameLogic.Game_Ammo_Clip_Loaded(playerID, playerID, curStub);
			return curClip;
		}
		for (curClip = 0; curClip < curAmmoClips; curClip++)
		{
			if ((global::Players.Players.players[playerID].ammoClips[curClip].surplus > 0 || global::Players.Players.players[playerID].ammoClips[curClip].numClips > 0) && global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].ammoIndex == global::Players.Players.players[playerID].ammoClips[curClip].ammoIndex)
			{
				if (wp1[weaponID].ChamberAfterShot)
				{
					global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].needToChamber = true;
				}
				if (ammo[global::Players.Players.players[playerID].ammoClips[curClip].ammoIndex].single)
				{
					byte maxAmmo;
					if ((maxAmmo = wp1[weaponID].maxAmmo) > 0)
					{
						ushort num = (ushort)(maxAmmo - global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].currentRounds);
						if (num < amount)
						{
							amount = num;
						}
						global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].currentRounds += amount;
						global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].magazineCapacity = maxAmmo;
						global::Players.Players.players[playerID].ammoClips[curClip].numClips -= amount;
					}
					else
					{
						amount = global::Players.Players.players[playerID].ammoClips[curClip].numClips;
						global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].currentRounds += (ushort)(global::Players.Players.players[playerID].ammoClips[curClip].count * amount);
						if (global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].currentRounds > ammoClips[curClip].maxCanCarry)
						{
							global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].currentRounds = ammoClips[curClip].maxCanCarry;
						}
						global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].magazineCapacity = ammoClips[curClip].maxCanCarry;
						global::Players.Players.players[playerID].ammoClips[curClip].numClips -= amount;
					}
				}
				else if (global::Players.Players.players[playerID].ammoClips[curClip].surplus > 0)
				{
					ushort num = (ushort)(global::Players.Players.players[playerID].ammoClips[curClip].count - global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].currentRounds);
					if (num > global::Players.Players.players[playerID].ammoClips[curClip].surplus)
					{
						num = global::Players.Players.players[playerID].ammoClips[curClip].surplus;
					}
					global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].currentRounds += num;
					global::Players.Players.players[playerID].ammoClips[curClip].surplus -= num;
					if (ammo[global::Players.Players.players[playerID].ammoClips[curClip].ammoIndex].single)
					{
						global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].magazineCapacity = ammoClips[curClip].maxCanCarry;
					}
				}
				else
				{
					if (global::Players.Players.players[playerID].ammoClips[curClip].numClips < amount)
					{
						amount = global::Players.Players.players[playerID].ammoClips[curClip].numClips;
					}
					if (global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].currentRounds > 0)
					{
						global::Players.Players.players[playerID].ammoClips[curClip].surplus += global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].currentRounds;
						if (global::Players.Players.players[playerID].ammoClips[curClip].surplus >= global::Players.Players.players[playerID].ammoClips[curClip].count)
						{
							ushort num = (ushort)(global::Players.Players.players[playerID].ammoClips[curClip].surplus / global::Players.Players.players[playerID].ammoClips[curClip].count);
							global::Players.Players.players[playerID].ammoClips[curClip].numClips += num;
							global::Players.Players.players[playerID].ammoClips[curClip].surplus -= (ushort)(global::Players.Players.players[playerID].ammoClips[curClip].count * num);
						}
					}
					global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].currentRounds = (ushort)(global::Players.Players.players[playerID].ammoClips[curClip].count * amount);
					global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].magazineCapacity = global::Players.Players.players[playerID].ammoClips[curClip].count;
					if (amount > 1)
					{
						global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].magazineCapacity *= amount;
					}
					global::Players.Players.players[playerID].ammoClips[curClip].numClips -= amount;
				}
				global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].needToReload = false;
				mainC.gameLogic.Game_Ammo_Clip_Loaded(playerID, playerID, curStub);
				return curClip;
			}
		}
		return result;
	}

	public bool Load_Ammo_Type_Single_Into_Player_Vehicle_Weapon_Immediately(ushort playerID, ushort curStub, ushort amount)
	{
		byte maxAmmo = wp1[global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].weaponID].maxAmmo;
		if (maxAmmo < 1 || maxAmmo == global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].currentRounds)
		{
			return false;
		}
		int num = -1;
		byte curClip = global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].curClip;
		if (curClip < curAmmoClips && global::Players.Players.players[playerID].ammoClips[curClip].numClips > 0)
		{
			num = curClip;
		}
		else
		{
			for (curClip = 0; curClip < curAmmoClips; curClip++)
			{
				if (global::Players.Players.players[playerID].ammoClips[curClip].numClips > 0 && global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].ammoIndex == global::Players.Players.players[playerID].ammoClips[curClip].ammoIndex)
				{
					num = curClip;
					break;
				}
			}
		}
		if (num < 0)
		{
			return false;
		}
		if (amount > global::Players.Players.players[playerID].ammoClips[num].numClips)
		{
			amount = global::Players.Players.players[playerID].ammoClips[num].numClips;
		}
		ushort num2 = (ushort)(maxAmmo - global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].currentRounds);
		if (num2 < amount)
		{
			amount = num2;
		}
		global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].currentRounds += amount;
		global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].magazineCapacity = maxAmmo;
		global::Players.Players.players[playerID].ammoClips[num].numClips -= amount;
		global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].needToReload = false;
		num2 = (ushort)(maxAmmo - global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].currentRounds);
		if (num2 > 1 && global::Players.Players.players[playerID].ammoClips[num].numClips < 1)
		{
			for (curClip = 0; curClip < curAmmoClips; curClip++)
			{
				if (global::Players.Players.players[playerID].ammoClips[curClip].numClips > 0 && global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].ammoIndex == global::Players.Players.players[playerID].ammoClips[curClip].ammoIndex)
				{
					return true;
				}
			}
		}
		return num2 > 0;
	}

	public byte Load_Ammo_Clip_Into_Player_Weapon(byte wpnIndex, byte wpnID, byte playerID, ushort amount)
	{
		byte result = Find_Ammo_Clip((byte)wp1[wpnID].ammoIndex);
		byte curClip = global::Players.Players.players[playerID].weapon2[wpnIndex].curClip;
		if (curClip < curAmmoClips && global::Players.Players.players[playerID].ammoClips[curClip].numClips > 0)
		{
			if (wp1[wpnID].ChamberAfterShot && !global::Players.Players.players[playerID].weapon2[wpnIndex].roundChambered && playerID == 0)
			{
				global::Players.Players.needToChamber = true;
			}
			if (ammo[global::Players.Players.players[playerID].ammoClips[curClip].ammoIndex].single)
			{
				amount = global::Players.Players.players[playerID].ammoClips[curClip].numClips;
				global::Players.Players.players[playerID].weapon2[wpnIndex].currentRounds += (ushort)(global::Players.Players.players[playerID].ammoClips[curClip].count * amount);
				if (global::Players.Players.players[playerID].weapon2[wpnIndex].currentRounds > ammoClips[curClip].maxCanCarry)
				{
					global::Players.Players.players[playerID].weapon2[wpnIndex].currentRounds = ammoClips[curClip].maxCanCarry;
				}
				global::Players.Players.players[playerID].weapon2[wpnIndex].magazineCapacity = ammoClips[curClip].maxCanCarry;
				global::Players.Players.players[playerID].ammoClips[curClip].numClips -= amount;
			}
			else
			{
				if (global::Players.Players.players[playerID].ammoClips[curClip].numClips < amount)
				{
					amount = global::Players.Players.players[playerID].ammoClips[curClip].numClips;
				}
				global::Players.Players.players[playerID].weapon2[wpnIndex].magazineCapacity = global::Players.Players.players[playerID].ammoClips[curClip].count;
				if (amount > 1)
				{
					global::Players.Players.players[playerID].weapon2[wpnIndex].magazineCapacity *= amount;
				}
				if (global::Players.Players.players[playerID].weapon2[wpnIndex].currentRounds > 0)
				{
					global::Players.Players.players[playerID].ammoClips[curClip].surplus += global::Players.Players.players[playerID].weapon2[wpnIndex].currentRounds;
					if (global::Players.Players.players[playerID].ammoClips[curClip].surplus >= global::Players.Players.players[playerID].ammoClips[curClip].count)
					{
						ushort num = (ushort)(global::Players.Players.players[playerID].ammoClips[curClip].surplus / global::Players.Players.players[playerID].ammoClips[curClip].count);
						global::Players.Players.players[playerID].ammoClips[curClip].numClips += num;
						global::Players.Players.players[playerID].ammoClips[curClip].surplus -= (ushort)(global::Players.Players.players[playerID].ammoClips[curClip].count * num);
					}
				}
				global::Players.Players.players[playerID].weapon2[wpnIndex].currentRounds = (ushort)(global::Players.Players.players[playerID].ammoClips[curClip].count * amount);
				global::Players.Players.players[playerID].ammoClips[curClip].numClips -= amount;
			}
			return curClip;
		}
		if (curClip < curAmmoClips && global::Players.Players.players[playerID].ammoClips[curClip].surplus > 0)
		{
			if (wp1[wpnID].ChamberAfterShot && !global::Players.Players.players[playerID].weapon2[wpnIndex].roundChambered && playerID == 0)
			{
				global::Players.Players.needToChamber = true;
			}
			ushort num = (ushort)(global::Players.Players.players[playerID].ammoClips[curClip].count - global::Players.Players.players[playerID].weapon2[wpnIndex].currentRounds);
			if (num > global::Players.Players.players[playerID].ammoClips[curClip].surplus)
			{
				num = global::Players.Players.players[playerID].ammoClips[curClip].surplus;
			}
			global::Players.Players.players[playerID].weapon2[wpnIndex].currentRounds += num;
			global::Players.Players.players[playerID].ammoClips[curClip].surplus -= num;
			if (ammo[global::Players.Players.players[playerID].ammoClips[curClip].ammoIndex].single)
			{
				global::Players.Players.players[playerID].weapon2[wpnIndex].magazineCapacity = ammoClips[curClip].maxCanCarry;
			}
			return curClip;
		}
		for (curClip = 0; curClip < curAmmoClips; curClip++)
		{
			if (global::Players.Players.players[playerID].ammoClips[curClip].numClips > 0 && wp1[wpnID].ammoIndex == global::Players.Players.players[playerID].ammoClips[curClip].ammoIndex)
			{
				if (wp1[wpnID].ChamberAfterShot && playerID == 0)
				{
					global::Players.Players.needToChamber = true;
				}
				if (ammo[global::Players.Players.players[playerID].ammoClips[curClip].ammoIndex].single)
				{
					amount = global::Players.Players.players[playerID].ammoClips[curClip].numClips;
					global::Players.Players.players[playerID].weapon2[wpnIndex].currentRounds += (ushort)(global::Players.Players.players[playerID].ammoClips[curClip].count * amount);
					if (global::Players.Players.players[playerID].weapon2[wpnIndex].currentRounds > ammoClips[curClip].maxCanCarry)
					{
						global::Players.Players.players[playerID].weapon2[wpnIndex].currentRounds = ammoClips[curClip].maxCanCarry;
					}
					global::Players.Players.players[playerID].weapon2[wpnIndex].magazineCapacity = ammoClips[curClip].maxCanCarry;
					global::Players.Players.players[playerID].ammoClips[curClip].numClips -= amount;
				}
				else
				{
					if (global::Players.Players.players[playerID].ammoClips[curClip].numClips < amount)
					{
						amount = global::Players.Players.players[playerID].ammoClips[curClip].numClips;
					}
					if (global::Players.Players.players[playerID].weapon2[wpnIndex].currentRounds > 0)
					{
						global::Players.Players.players[playerID].ammoClips[curClip].surplus += global::Players.Players.players[playerID].weapon2[wpnIndex].currentRounds;
						if (global::Players.Players.players[playerID].ammoClips[curClip].surplus >= global::Players.Players.players[playerID].ammoClips[curClip].count)
						{
							ushort num = (ushort)(global::Players.Players.players[playerID].ammoClips[curClip].surplus / global::Players.Players.players[playerID].ammoClips[curClip].count);
							global::Players.Players.players[playerID].ammoClips[curClip].numClips += num;
							global::Players.Players.players[playerID].ammoClips[curClip].surplus -= (ushort)(global::Players.Players.players[playerID].ammoClips[curClip].count * num);
						}
					}
					global::Players.Players.players[playerID].weapon2[wpnIndex].currentRounds = (ushort)(global::Players.Players.players[playerID].ammoClips[curClip].count * amount);
					global::Players.Players.players[playerID].weapon2[wpnIndex].magazineCapacity = global::Players.Players.players[playerID].ammoClips[curClip].count;
					if (amount > 1)
					{
						global::Players.Players.players[playerID].weapon2[wpnIndex].magazineCapacity *= amount;
					}
					global::Players.Players.players[playerID].ammoClips[curClip].numClips -= amount;
				}
				return curClip;
			}
		}
		return result;
	}

	public void Load_Ammo_Clip_Into_AI_Weapon(byte wpnIndex, byte wpnID, byte playerID, ushort amount)
	{
		byte curClip = global::Players.Players.players[playerID].weapon2[wpnIndex].curClip;
		if (curClip >= curAmmoClips)
		{
			return;
		}
		if (ammo[global::Players.Players.players[playerID].ammoClips[curClip].ammoIndex].single)
		{
			global::Players.Players.players[playerID].weapon2[wpnIndex].currentRounds += (ushort)(global::Players.Players.players[playerID].ammoClips[curClip].count * amount);
			global::Players.Players.players[playerID].weapon2[wpnIndex].magazineCapacity = ammoClips[curClip].maxCanCarry;
			return;
		}
		global::Players.Players.players[playerID].weapon2[wpnIndex].magazineCapacity = global::Players.Players.players[playerID].ammoClips[curClip].count;
		if (amount > 1)
		{
			global::Players.Players.players[playerID].weapon2[wpnIndex].magazineCapacity *= amount;
		}
		global::Players.Players.players[playerID].weapon2[wpnIndex].currentRounds = (ushort)(global::Players.Players.players[playerID].ammoClips[curClip].count * amount);
	}

	public bool Add_Ammo_Clip(byte clipID, byte amount, byte playerID)
	{
		if (amount < 0)
		{
			return true;
		}
		if (clipID < curAmmoClips)
		{
			if (ammo[ammoClips[clipID].ammoIndex].single)
			{
				for (ushort num = 0; num < global::MainGame.MainGame.playerVehicles[playerID].numWeapons; num++)
				{
					if (wp1[global::MainGame.MainGame.playerVehicles[playerID].weapons[num].weaponID].maxAmmo > 0)
					{
						if (wp1[global::MainGame.MainGame.playerVehicles[playerID].weapons[num].weaponID].ammoIndex == ammoClips[clipID].ammoIndex && global::Players.Players.players[playerID].ammoClips[clipID].numClips < ammoClips[clipID].maxCanCarry)
						{
							global::Players.Players.players[playerID].ammoClips[clipID].numClips += (ushort)(amount * wp1[global::MainGame.MainGame.playerVehicles[playerID].weapons[num].weaponID].maxAmmo);
							if (global::Players.Players.players[playerID].ammoClips[clipID].numClips >= ammoClips[clipID].maxCanCarry)
							{
								global::Players.Players.players[playerID].ammoClips[clipID].numClips = ammoClips[clipID].maxCanCarry;
							}
							return true;
						}
					}
					else if (wp1[global::MainGame.MainGame.playerVehicles[playerID].weapons[num].weaponID].ammoIndex == ammoClips[clipID].ammoIndex && global::MainGame.MainGame.playerVehicles[playerID].weapons[num].currentRounds < ammoClips[clipID].maxCanCarry)
					{
						global::Players.Players.players[playerID].ammoClips[clipID].numClips += amount;
						Load_Ammo_Clip_Into_Player_Vehicle_Weapon(playerID, num, global::Players.Players.players[playerID].ammoClips[clipID].numClips);
						return true;
					}
				}
				return false;
			}
			if (global::Players.Players.players[playerID].ammoClips[clipID].numClips == global::Players.Players.players[playerID].ammoClips[clipID].maxCanCarry)
			{
				return false;
			}
			global::Players.Players.players[playerID].ammoClips[clipID].numClips += amount;
			if (global::Players.Players.players[playerID].ammoClips[clipID].numClips > global::Players.Players.players[playerID].ammoClips[clipID].maxCanCarry)
			{
				global::Players.Players.players[playerID].ammoClips[clipID].numClips = global::Players.Players.players[playerID].ammoClips[clipID].maxCanCarry;
			}
			return true;
		}
		return false;
	}

	public byte Find_Ammo_Clip(byte ammoIndex)
	{
		for (byte b = 0; b < curAmmoClips; b++)
		{
			if (ammoClips[b].ammoIndex == ammoIndex)
			{
				return b;
			}
		}
		return 0;
	}

	public byte Player_Has_Ammo_For_Weapon(byte playerID)
	{
		byte b = (byte)global::Players.Players.players[playerID].primaryWeaponMountWeapon;
		byte b2 = (byte)wp1[b].ammoIndex;
		for (byte b3 = 0; b3 < curAmmoClips; b3++)
		{
			if (b2 == global::Players.Players.players[playerID].ammoClips[b3].ammoIndex && (global::Players.Players.players[playerID].ammoClips[b3].numClips > 0 || global::Players.Players.players[playerID].ammoClips[b3].surplus > 0))
			{
				if (global::Players.Players.players[playerID].weapon2[global::Players.Players.players[playerID].wpnIndex].magazineCapacity < 1 || global::Players.Players.players[playerID].weapon2[global::Players.Players.players[playerID].wpnIndex].currentRounds < global::Players.Players.players[playerID].weapon2[global::Players.Players.players[playerID].wpnIndex].magazineCapacity)
				{
					return 2;
				}
				return 1;
			}
		}
		return 0;
	}

	public byte Player_Has_Ammo_For_Weapon(ushort playerID, ushort curStub)
	{
		byte ammoIndex = global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].ammoIndex;
		for (byte b = 0; b < curAmmoClips; b++)
		{
			if (ammoIndex == global::Players.Players.players[playerID].ammoClips[b].ammoIndex && (global::Players.Players.players[playerID].ammoClips[b].numClips > 0 || global::Players.Players.players[playerID].ammoClips[b].surplus > 0))
			{
				if (global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].magazineCapacity < 1 || global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].currentRounds < global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].magazineCapacity)
				{
					return 2;
				}
				return 1;
			}
		}
		return 0;
	}

	public void Setup_Player_Ammo_Clips()
	{
		for (byte b = 0; b < 44; b++)
		{
			if (global::Players.Players.players[b].numAmmoClips < numAmmoClips)
			{
				global::Players.Players.players[b].ammoClips = new StructsClass.Ammo_Clips[numAmmoClips];
				global::Players.Players.players[b].numAmmoClips = numAmmoClips;
			}
			for (byte b2 = 0; b2 < numAmmoClips; b2++)
			{
				global::Players.Players.players[b].ammoClips[b2].ammoIndex = ammoClips[b2].ammoIndex;
				global::Players.Players.players[b].ammoClips[b2].count = ammoClips[b2].count;
				global::Players.Players.players[b].ammoClips[b2].size = ammoClips[b2].size;
				global::Players.Players.players[b].ammoClips[b2].maxCanCarry = ammoClips[b2].maxCanCarry;
				global::Players.Players.players[b].ammoClips[b2].surplus = 0;
			}
		}
	}

	public void Reset_Weapons_For_Respawn(byte playerID)
	{
		Set_Minimum_Ammo(playerID);
		Reload_Player_Weapons_Immediately(playerID);
	}

	public void Move_Weapon_Rounds_To_Ammo_Clip_Surplus(ushort playerID)
	{
		for (byte b = 0; b < curAmmoClips; b++)
		{
			for (byte b2 = 0; b2 < global::MainGame.MainGame.playerVehicles[playerID].numWeapons; b2++)
			{
				if (global::MainGame.MainGame.playerVehicles[playerID].weapons[b2].curClip == b)
				{
					global::Players.Players.players[playerID].ammoClips[b].surplus += global::MainGame.MainGame.playerVehicles[playerID].weapons[b2].currentRounds;
					global::MainGame.MainGame.playerVehicles[playerID].weapons[b2].currentRounds = 0;
				}
			}
		}
	}

	public bool Add_Ammo_Clip_For_All_Player_Vehicle_Weapons(byte playerID, byte amount, byte ammoType)
	{
		bool result = false;
		for (byte b = 0; b < global::MainGame.MainGame.playerVehicles[playerID].numWeapons; b++)
		{
			byte curClip = global::MainGame.MainGame.playerVehicles[playerID].weapons[b].curClip;
			if (curClip < numAmmoClips && ammo[ammoClips[curClip].ammoIndex].type == ammoType && Add_Ammo_Clip(curClip, amount, playerID))
			{
				result = true;
			}
		}
		return result;
	}

	public void Set_Minimum_Ammo(byte playerID)
	{
		for (byte b = 0; b < curAmmoClips; b++)
		{
			global::Players.Players.players[playerID].ammoClips[b].numClips = ammoClips[b].startingNumClips;
			global::Players.Players.players[playerID].ammoClips[b].surplus = 0;
		}
	}

	public float Get_Ammo_Muzzle_Velocity_Current_Weapon(ushort playerID)
	{
		byte curClip = global::Players.Players.players[playerID].weapon2[global::Players.Players.players[playerID].wpnIndex].curClip;
		if (curClip < curAmmoClips)
		{
			return ammo[ammoClips[curClip].ammoIndex].muzzleVelocity;
		}
		return 1f;
	}

	public void Update_Player_Weapon_Info(byte playerID)
	{
		short primaryWeaponMountWeapon = global::Players.Players.players[playerID].primaryWeaponMountWeapon;
		short wpnIndex = global::Players.Players.players[playerID].wpnIndex;
		global::Players.Players.players[playerID].weapon1.pfx1 = wp1[primaryWeaponMountWeapon].pfx1;
		global::Players.Players.players[playerID].weapon1.pfx2 = wp1[primaryWeaponMountWeapon].pfx2;
		global::Players.Players.players[playerID].weapon1.pfx3 = wp1[primaryWeaponMountWeapon].pfx3;
		global::Players.Players.players[playerID].weapon2[wpnIndex].ammoAccelerationZ = ammo[wp1[primaryWeaponMountWeapon].ammoIndex].accelerationZ;
		global::Players.Players.players[playerID].weapon2[wpnIndex].muzzleVelocity = ammo[wp1[primaryWeaponMountWeapon].ammoIndex].muzzleVelocity;
		global::Players.Players.players[playerID].weapon2[wpnIndex].shotCount = ammo[wp1[primaryWeaponMountWeapon].ammoIndex].shotCount;
		global::Players.Players.players[playerID].weapon2[wpnIndex].ammoIndex = (byte)wp1[primaryWeaponMountWeapon].ammoIndex;
		mainC.programsMain.Start_Animation(playerID, ref global::Players.Players.players[playerID].jt1, ref global::Players.Players.players[playerID].animations, global::Players.Players.players[playerID].programCollection, wp1[primaryWeaponMountWeapon].AnimationHolding, 1f, 1f);
	}

	public void Reset_Network_Player_Particle_Check()
	{
		for (short num = 0; num < global::Networking.Networking.networkSession.RemoteGamers.Count; num++)
		{
			global::Players.Players.players[mainC.playersMain.Get_Player_Index(global::Networking.Networking.networkSession.RemoteGamers[num].Id, 0)].makeParticle = true;
		}
	}

	public void Send_Player_Weapons(ushort playerID, NetworkGamer remoteGamer)
	{
		byte numMounts = Vehicles.vehicles[global::Players.Players.players[playerID].curVehicle].numMounts;
		for (byte b = 0; b < numMounts; b++)
		{
			if (global::MainGame.MainGame.playerVehicles[playerID].mounts[b].type == 1)
			{
				byte objectID = global::MainGame.MainGame.playerVehicles[playerID].mounts[b].objectID;
				global::Networking.Networking.networkBytes[0] = 9;
				global::Networking.Networking.networkBytes[1] = b;
				global::Networking.Networking.networkBytes[2] = global::MainGame.MainGame.playerVehicles[playerID].mounts[b].objectAttached;
				global::Networking.Networking.networkBytes[3] = global::MainGame.MainGame.playerVehicles[playerID].mounts[b].objectID;
				global::Networking.Networking.networkBytes[4] = global::MainGame.MainGame.playerVehicles[playerID].weapons[objectID].weaponID;
				global::Networking.Networking.networkBytes[5] = global::MainGame.MainGame.playerVehicles[playerID].weapons[objectID].skinID;
				global::Networking.Networking.networkBytes[6] = global::MainGame.MainGame.playerVehicles[playerID].weapons[objectID].scopeID;
				global::Networking.Networking.networkBytes[7] = global::MainGame.MainGame.playerVehicles[playerID].weapons[objectID].foreGripID;
				global::Networking.Networking.networkBytes[8] = global::MainGame.MainGame.playerVehicles[playerID].weapons[objectID].barrelID;
				global::Networking.Networking.networkBytes[9] = global::MainGame.MainGame.playerVehicles[playerID].weapons[objectID].energyDeviceID;
				mainC.networkingMain.XBOX_Send_Network_Message_To_Gamer(36, remoteGamer);
			}
		}
	}

	public void Receive_Player_Weapons_From_Network(byte account)
	{
		int num = mainC.playersMain.Get_Player_Index(account, -1);
		if (num != -1)
		{
			byte b = global::Networking.Networking.networkBytes[0];
			byte b2 = global::Networking.Networking.networkBytes[2];
			mainC.programsMain.Cancel_Animations_Of_Type((ushort)num, ref global::Players.Players.players[num].animations, global::Players.Players.players[num].programCollection, 15);
			Load_Weapon_Into_Player_Vehicle_Weapon_Stub((ushort)num, b2, global::Networking.Networking.networkBytes[3], 1);
			mainC.vehicles.Set_Mount_Weapon_Stub((ushort)num, b, b2);
			mainC.gameLogic.Game_Modify_Weapon_Programs_For_Attachments((ushort)num);
			global::MainGame.MainGame.playerVehicles[num].mounts[b].objectAttached = global::Networking.Networking.networkBytes[1];
			global::MainGame.MainGame.playerVehicles[num].weapons[b2].weaponID = global::Networking.Networking.networkBytes[3];
			global::MainGame.MainGame.playerVehicles[num].weapons[b2].skinID = global::Networking.Networking.networkBytes[4];
			global::MainGame.MainGame.playerVehicles[num].weapons[b2].scopeID = global::Networking.Networking.networkBytes[5];
			global::MainGame.MainGame.playerVehicles[num].weapons[b2].foreGripID = global::Networking.Networking.networkBytes[6];
			global::MainGame.MainGame.playerVehicles[num].weapons[b2].barrelID = global::Networking.Networking.networkBytes[7];
			global::MainGame.MainGame.playerVehicles[num].weapons[b2].energyDeviceID = global::Networking.Networking.networkBytes[8];
			mainC.gameLogic.Game_Received_Player_Weapon_Update((ushort)num, b2);
		}
	}

	public void Set_Weapon_View_Variables(ushort curStub)
	{
		global::Players.Players.firstPersonViewX = wp1[global::MainGame.MainGame.playerVehicles[0].weapons[curStub].weaponID].firstPersonViewX + global::Players.Players.firstPersonViewAdjX;
		global::Players.Players.firstPersonViewY = wp1[global::MainGame.MainGame.playerVehicles[0].weapons[curStub].weaponID].firstPersonViewY + global::Players.Players.firstPersonViewAdjY;
		global::Players.Players.firstPersonViewZ = wp1[global::MainGame.MainGame.playerVehicles[0].weapons[curStub].weaponID].firstPersonViewZ + global::Players.Players.firstPersonViewAdjZ;
		global::Players.Players.ironSightsViewX = wp1[global::MainGame.MainGame.playerVehicles[0].weapons[curStub].weaponID].ironSightsViewX + global::Players.Players.ironSightsViewAdjX;
		global::Players.Players.ironSightsViewY = wp1[global::MainGame.MainGame.playerVehicles[0].weapons[curStub].weaponID].ironSightsViewY + global::Players.Players.ironSightsViewAdjY;
		global::Players.Players.ironSightsViewZ = wp1[global::MainGame.MainGame.playerVehicles[0].weapons[curStub].weaponID].ironSightsViewZ + global::Players.Players.ironSightsViewAdjZ;
		global::Players.Players.scopeViewX = global::MainGame.MainGame.playerVehicles[0].weapons[curStub].scopeViewX;
		global::Players.Players.scopeViewY = global::MainGame.MainGame.playerVehicles[0].weapons[curStub].scopeViewY;
		global::Players.Players.scopeViewZ = global::MainGame.MainGame.playerVehicles[0].weapons[curStub].scopeViewZ;
	}

	public ushort Get_Weapon_ID_By_Name(string weaponName, ushort defaultValue)
	{
		for (ushort num = 0; num < numWeapons; num++)
		{
			if (string.Compare(wp1[num].weaponName, weaponName, StringComparison.CurrentCultureIgnoreCase) == 0)
			{
				return num;
			}
		}
		return defaultValue;
	}

	public byte Get_Primary_Weapon_Clip(ushort playerID)
	{
		if (global::MainGame.MainGame.primaryWeaponMount < global::MainGame.MainGame.playerVehicles[playerID].numMounts && global::MainGame.MainGame.playerVehicles[playerID].mounts[global::MainGame.MainGame.primaryWeaponMount].objectAttached == 1)
		{
			return global::MainGame.MainGame.playerVehicles[playerID].weapons[global::MainGame.MainGame.playerVehicles[playerID].mounts[global::MainGame.MainGame.primaryWeaponMount].objectID].curClip;
		}
		if (global::MainGame.MainGame.secondaryWeaponMount < global::MainGame.MainGame.playerVehicles[playerID].numMounts && global::MainGame.MainGame.playerVehicles[playerID].mounts[global::MainGame.MainGame.secondaryWeaponMount].objectAttached == 1)
		{
			return global::MainGame.MainGame.playerVehicles[playerID].weapons[global::MainGame.MainGame.playerVehicles[playerID].mounts[global::MainGame.MainGame.secondaryWeaponMount].objectID].curClip;
		}
		return 0;
	}

	public void Sync_Weapon_Mounts()
	{
		byte rBufferID = global::Rendering.Rendering.rBufferID;
		byte uBufferID = global::Rendering.Rendering.uBufferID;
		for (ushort num = 0; num < numWeaponMounts; num++)
		{
			ref Matrix reference = ref wpmMounts[num].mv[uBufferID];
			reference = wpmMounts[num].mv[rBufferID];
		}
	}

	public void Sync_Bullets()
	{
		byte rBufferID = global::Rendering.Rendering.rBufferID;
		byte uBufferID = global::Rendering.Rendering.uBufferID;
		for (ushort num = 0; num < 100; num++)
		{
			bullet[num].startX[uBufferID] = bullet[num].startX[rBufferID];
			bullet[num].startY[uBufferID] = bullet[num].startY[rBufferID];
			bullet[num].startZ[uBufferID] = bullet[num].startZ[rBufferID];
			bullet[num].endX[uBufferID] = bullet[num].endX[rBufferID];
			bullet[num].endY[uBufferID] = bullet[num].endY[rBufferID];
			bullet[num].endZ[uBufferID] = bullet[num].endZ[rBufferID];
			bulletActive[0, num] = 0;
			bulletActive[1, num] = 0;
		}
	}

	public float Get_Damage(byte ammoIndex, byte damageType)
	{
		return ammo[ammoIndex].damage[damageType];
	}

	public bool Is_Player_Weapon_Loaded(byte playerID)
	{
		return global::Players.Players.players[playerID].weapon2[global::Players.Players.players[playerID].wpnIndex].currentRounds > 0;
	}

	public void Reset_Players_Weapon_Stub(ref StructsClass.weapon_stub stub, byte wpnIndex, byte weaponID, byte playerID)
	{
		stub.active = true;
		stub.weaponID = weaponID;
		stub.fired = false;
		stub.roundChambered = true;
		stub.shooting = false;
		stub.shooting = false;
		stub.singleShot = false;
		stub.scopeID = wp1[weaponID].scopeID;
		stub.foreGripID = wp1[weaponID].foreGripID;
		stub.barrelID = wp1[weaponID].barrelID;
		stub.energyDeviceID = wp1[weaponID].energyDeviceID;
		stub.scopeLow = scopes[stub.scopeID].rangeLow;
		stub.scopeHigh = scopes[stub.scopeID].rangeHigh;
		stub.gripType = wp1[weaponID].gripType;
		stub.fireRate = wp1[weaponID].fireRate;
		stub.secsPerBullet = wp1[weaponID].secsPerBullet;
		stub.firingStart = global::MainGame.MainGame.gameTime - wp1[weaponID].secsPerBullet * global::Physics.Physics.timeMod;
		stub.tracerCnt = 0;
		stub.roundsPerTracer = wp1[weaponID].roundsPerTracer;
		stub.crossHair = wp1[weaponID].crossHairs[0];
		stub.fireRateAdjustment = 1f;
		stub.accuracyAdjustment = 1f;
		stub.muzzleVelocityAdjustment = 1f;
		stub.curHeat = 0f;
		stub.scopeViewX = wp1[weaponID].scopeViewX;
		stub.scopeViewY = wp1[weaponID].scopeViewY;
		stub.scopeViewZ = wp1[weaponID].scopeViewZ;
		stub.AnimationChamber = wp1[weaponID].AnimationChamber;
		stub.AnimationFire = wp1[weaponID].AnimationFire;
		stub.AnimationHolding = wp1[weaponID].AnimationHolding;
		stub.AnimationWalk = wp1[weaponID].AnimationWalk;
		stub.AnimationRun = wp1[weaponID].AnimationRun;
		stub.AnimationReload = wp1[weaponID].AnimationReload;
		stub.AnimationSpecial1 = wp1[weaponID].AnimationSpecial1;
		stub.AnimationThrow = wp1[weaponID].AnimationThrow;
		stub.AnimationIronSights = wp1[weaponID].AnimationIronSights;
		stub.AnimationChangeWeapon = wp1[weaponID].AnimationChangeWeapon;
		stub.recoilUp[0] = wp1[weaponID].recoilUp[0];
		stub.recoilUp[1] = wp1[weaponID].recoilUp[1];
		stub.recoilSide[0] = wp1[weaponID].recoilSide[0];
		stub.recoilSide[1] = wp1[weaponID].recoilSide[1];
		stub.recoilSide[2] = wp1[weaponID].recoilSide[2];
		stub.recoilBack[0] = wp1[weaponID].recoilBack[0];
		stub.recoilBack[1] = wp1[weaponID].recoilBack[1];
		stub.magazineCapacity = 0;
		stub.currentRounds = 0;
		stub.curClip = byte.MaxValue;
		stub.curClip = Load_Ammo_Clip_Into_Player_Weapon(wpnIndex, weaponID, playerID, 1);
		if (stub.curClip >= curAmmoClips)
		{
			stub.curClip = Find_Ammo_Clip(weaponID);
		}
		byte b = (stub.ammoIndex = ammoClips[stub.curClip].ammoIndex);
		stub.shotCount = ammo[b].shotCount;
		stub.ammoAccelerationZ = ammo[b].accelerationZ;
		stub.muzzleVelocity = ammo[b].muzzleVelocity;
	}

	public void Load_Weapon_Into_Player_Vehicle_Weapon_Stub(ushort vhID, byte stubID, byte weaponID, byte amount)
	{
		byte b = wp1[weaponID].numBarrels;
		global::MainGame.MainGame.playerVehicles[vhID].weapons[stubID].offset = new StructsClass.Coordinate_3D[b, 10];
		byte b2;
		byte b3;
		for (b2 = 0; b2 < 10; b2++)
		{
			for (b3 = 0; b3 < b; b3++)
			{
				global::MainGame.MainGame.playerVehicles[vhID].weapons[stubID].offset[b3, b2] = default(StructsClass.Coordinate_3D);
			}
		}
		global::MainGame.MainGame.playerVehicles[vhID].weapons[stubID].fullyAutomatic = wp1[weaponID].fireMode != 1;
		global::MainGame.MainGame.playerVehicles[vhID].weapons[stubID].active = true;
		global::MainGame.MainGame.playerVehicles[vhID].weapons[stubID].weaponID = weaponID;
		global::MainGame.MainGame.playerVehicles[vhID].weapons[stubID].fired = false;
		global::MainGame.MainGame.playerVehicles[vhID].weapons[stubID].roundChambered = true;
		global::MainGame.MainGame.playerVehicles[vhID].weapons[stubID].singleShot = false;
		global::MainGame.MainGame.playerVehicles[vhID].weapons[stubID].scopeID = wp1[weaponID].scopeID;
		global::MainGame.MainGame.playerVehicles[vhID].weapons[stubID].foreGripID = wp1[weaponID].foreGripID;
		global::MainGame.MainGame.playerVehicles[vhID].weapons[stubID].barrelID = wp1[weaponID].barrelID;
		global::MainGame.MainGame.playerVehicles[vhID].weapons[stubID].energyDeviceID = wp1[weaponID].energyDeviceID;
		global::MainGame.MainGame.playerVehicles[vhID].weapons[stubID].scopeLow = scopes[global::MainGame.MainGame.playerVehicles[vhID].weapons[stubID].scopeID].rangeLow;
		global::MainGame.MainGame.playerVehicles[vhID].weapons[stubID].scopeHigh = scopes[global::MainGame.MainGame.playerVehicles[vhID].weapons[stubID].scopeID].rangeHigh;
		global::MainGame.MainGame.playerVehicles[vhID].weapons[stubID].gripType = wp1[weaponID].gripType;
		global::MainGame.MainGame.playerVehicles[vhID].weapons[stubID].fireRate = wp1[weaponID].fireRate;
		global::MainGame.MainGame.playerVehicles[vhID].weapons[stubID].secsPerBullet = wp1[weaponID].secsPerBullet;
		global::MainGame.MainGame.playerVehicles[vhID].weapons[stubID].firingStart = global::MainGame.MainGame.gameTime - wp1[weaponID].secsPerBullet * global::Physics.Physics.timeMod;
		global::MainGame.MainGame.playerVehicles[vhID].weapons[stubID].tracerCnt = 0;
		global::MainGame.MainGame.playerVehicles[vhID].weapons[stubID].roundsPerTracer = wp1[weaponID].roundsPerTracer;
		global::MainGame.MainGame.playerVehicles[vhID].weapons[stubID].crossHair = wp1[weaponID].crossHairs[0];
		global::MainGame.MainGame.playerVehicles[vhID].weapons[stubID].fireRateAdjustment = 1f;
		global::MainGame.MainGame.playerVehicles[vhID].weapons[stubID].accuracyAdjustment = 1f;
		global::MainGame.MainGame.playerVehicles[vhID].weapons[stubID].muzzleVelocityAdjustment = 1f;
		global::MainGame.MainGame.playerVehicles[vhID].weapons[stubID].scopeViewX = wp1[weaponID].scopeViewX;
		global::MainGame.MainGame.playerVehicles[vhID].weapons[stubID].scopeViewY = wp1[weaponID].scopeViewY;
		global::MainGame.MainGame.playerVehicles[vhID].weapons[stubID].scopeViewZ = wp1[weaponID].scopeViewZ;
		global::MainGame.MainGame.playerVehicles[vhID].weapons[stubID].AnimationChamber = wp1[weaponID].AnimationChamber;
		global::MainGame.MainGame.playerVehicles[vhID].weapons[stubID].AnimationFire = wp1[weaponID].AnimationFire;
		global::MainGame.MainGame.playerVehicles[vhID].weapons[stubID].AnimationWalk = wp1[weaponID].AnimationWalk;
		global::MainGame.MainGame.playerVehicles[vhID].weapons[stubID].AnimationRun = wp1[weaponID].AnimationRun;
		global::MainGame.MainGame.playerVehicles[vhID].weapons[stubID].AnimationHolding = wp1[weaponID].AnimationHolding;
		global::MainGame.MainGame.playerVehicles[vhID].weapons[stubID].AnimationReload = wp1[weaponID].AnimationReload;
		global::MainGame.MainGame.playerVehicles[vhID].weapons[stubID].AnimationSpecial1 = wp1[weaponID].AnimationSpecial1;
		global::MainGame.MainGame.playerVehicles[vhID].weapons[stubID].AnimationThrow = wp1[weaponID].AnimationThrow;
		global::MainGame.MainGame.playerVehicles[vhID].weapons[stubID].AnimationIronSights = wp1[weaponID].AnimationIronSights;
		global::MainGame.MainGame.playerVehicles[vhID].weapons[stubID].AnimationChangeWeapon = wp1[weaponID].AnimationChangeWeapon;
		global::MainGame.MainGame.playerVehicles[vhID].weapons[stubID].recoilUp[0] = wp1[weaponID].recoilUp[0];
		global::MainGame.MainGame.playerVehicles[vhID].weapons[stubID].recoilUp[1] = wp1[weaponID].recoilUp[1];
		global::MainGame.MainGame.playerVehicles[vhID].weapons[stubID].recoilSide[0] = wp1[weaponID].recoilSide[0];
		global::MainGame.MainGame.playerVehicles[vhID].weapons[stubID].recoilSide[1] = wp1[weaponID].recoilSide[1];
		global::MainGame.MainGame.playerVehicles[vhID].weapons[stubID].recoilSide[2] = wp1[weaponID].recoilSide[2];
		global::MainGame.MainGame.playerVehicles[vhID].weapons[stubID].recoilBack[0] = wp1[weaponID].recoilBack[0];
		global::MainGame.MainGame.playerVehicles[vhID].weapons[stubID].recoilBack[1] = wp1[weaponID].recoilBack[1];
		global::MainGame.MainGame.playerVehicles[vhID].weapons[stubID].numAttachments = wp1[weaponID].numAttachmentPoints;
		global::MainGame.MainGame.playerVehicles[vhID].weapons[stubID].attachments = new StructsClass.Weapon_Stub_Attachment_Point[wp1[weaponID].numAttachmentPoints];
		b2 = 0;
		b3 = global::MainGame.MainGame.playerVehicles[vhID].weapons[stubID].numAttachments;
		while (b2 < b3)
		{
			global::MainGame.MainGame.playerVehicles[vhID].weapons[stubID].attachments[b2] = default(StructsClass.Weapon_Stub_Attachment_Point);
			global::MainGame.MainGame.playerVehicles[vhID].weapons[stubID].attachments[b2].status = 0;
			b2++;
		}
		Initialize_Ammo_Clip_Into_Player_Vehicle_Weapon(vhID, stubID, 0, findAmmoClipID: true, amount);
		b3 = ammoClips[global::MainGame.MainGame.playerVehicles[vhID].weapons[stubID].curClip].ammoIndex;
		global::MainGame.MainGame.playerVehicles[vhID].weapons[stubID].ammoIndex = b3;
		global::MainGame.MainGame.playerVehicles[vhID].weapons[stubID].shotCount = ammo[b3].shotCount;
		global::MainGame.MainGame.playerVehicles[vhID].weapons[stubID].ammoAccelerationZ = ammo[b3].accelerationZ;
		global::MainGame.MainGame.playerVehicles[vhID].weapons[stubID].muzzleVelocity = ammo[b3].muzzleVelocity;
		global::MainGame.MainGame.playerVehicles[vhID].weapons[stubID].shootingAccuracy = wp1[weaponID].spread;
	}

	public void Add_Weapon_Attachments(ushort playerID, byte mountID)
	{
		byte objectID = global::MainGame.MainGame.playerVehicles[playerID].mounts[mountID].objectID;
		byte weaponID = global::MainGame.MainGame.playerVehicles[playerID].weapons[global::MainGame.MainGame.playerVehicles[playerID].mounts[mountID].objectID].weaponID;
		global::MainGame.MainGame.playerVehicles[playerID].weapons[objectID].scopeID = (byte)User_Interface.newScopeID;
		global::MainGame.MainGame.playerVehicles[playerID].weapons[objectID].foreGripID = (byte)User_Interface.newForeGripID;
		global::MainGame.MainGame.playerVehicles[playerID].weapons[objectID].barrelID = (byte)User_Interface.newBarrellID;
		global::MainGame.MainGame.playerVehicles[playerID].weapons[objectID].energyDeviceID = (byte)User_Interface.newEnergyDeviceID;
		global::MainGame.MainGame.playerVehicles[playerID].weapons[objectID].skinID = (byte)User_Interface.newSkinID;
		global::MainGame.MainGame.playerVehicles[playerID].weapons[objectID].scopeLow = scopes[User_Interface.newScopeID].rangeLow;
		global::MainGame.MainGame.playerVehicles[playerID].weapons[objectID].scopeHigh = scopes[User_Interface.newScopeID].rangeHigh;
		global::MainGame.MainGame.playerVehicles[playerID].weapons[objectID].scopeViewX = wp1[weaponID].scopeViewX + scopes[User_Interface.newScopeID].scopeViewAdjX;
		global::MainGame.MainGame.playerVehicles[playerID].weapons[objectID].scopeViewY = wp1[weaponID].scopeViewY + scopes[User_Interface.newScopeID].scopeViewAdjY;
		global::MainGame.MainGame.playerVehicles[playerID].weapons[objectID].scopeViewZ = wp1[weaponID].scopeViewZ + scopes[User_Interface.newScopeID].scopeViewAdjZ;
		global::MainGame.MainGame.playerVehicles[playerID].weapons[objectID].accuracyAdjustment = 1f;
		global::MainGame.MainGame.playerVehicles[playerID].weapons[objectID].accuracyAdjustment += scopes[User_Interface.newScopeID].adjustmentAccuracy;
		global::MainGame.MainGame.playerVehicles[playerID].weapons[objectID].accuracyAdjustment += foreGrips[User_Interface.newForeGripID].adjustmentAccuracy;
		global::MainGame.MainGame.playerVehicles[playerID].weapons[objectID].accuracyAdjustment += barrels[User_Interface.newBarrellID].adjustmentAccuracy;
		if (global::MainGame.MainGame.playerVehicles[playerID].weapons[objectID].accuracyAdjustment < 0f)
		{
			global::MainGame.MainGame.playerVehicles[playerID].weapons[objectID].accuracyAdjustment = 0f;
		}
		global::MainGame.MainGame.playerVehicles[playerID].weapons[objectID].muzzleVelocityAdjustment += barrels[User_Interface.newBarrellID].adjustmentMuzzleVelocity;
		mobilityFactor = 1f + foreGrips[User_Interface.newForeGripID].adjustmentAccuracy * 5f;
		if (mobilityFactor < 0f)
		{
			mobilityFactor = 0f;
		}
		global::MainGame.MainGame.playerVehicles[playerID].weapons[objectID].recoilUp[0] *= mobilityFactor;
		global::MainGame.MainGame.playerVehicles[playerID].weapons[objectID].recoilUp[1] *= mobilityFactor;
		global::MainGame.MainGame.playerVehicles[playerID].weapons[objectID].recoilSide[0] *= mobilityFactor;
		global::MainGame.MainGame.playerVehicles[playerID].weapons[objectID].recoilSide[1] *= mobilityFactor;
		global::MainGame.MainGame.playerVehicles[playerID].weapons[objectID].recoilSide[2] *= mobilityFactor;
		global::MainGame.MainGame.playerVehicles[playerID].weapons[objectID].recoilBack[0] *= mobilityFactor;
		global::MainGame.MainGame.playerVehicles[playerID].weapons[objectID].recoilBack[1] *= mobilityFactor;
		mobilityFactor = 1f;
		mobilityFactor += scopes[User_Interface.newScopeID].adjustmentTurning;
		mobilityFactor += foreGrips[User_Interface.newForeGripID].adjustmentTurning;
		mobilityFactor += barrels[User_Interface.newBarrellID].adjustmentTurning;
		mobilityFactor += energyDevices[User_Interface.newEnergyDeviceID].adjustmentTurning;
		movementSpeedFactor = 1f;
		movementSpeedFactor += scopes[User_Interface.newScopeID].adjustmentMovement;
		movementSpeedFactor += foreGrips[User_Interface.newForeGripID].adjustmentMovement;
		movementSpeedFactor += barrels[User_Interface.newBarrellID].adjustmentMovement;
		movementSpeedFactor += energyDevices[User_Interface.newEnergyDeviceID].adjustmentMovement;
		global::MainGame.MainGame.playerVehicles[playerID].weapons[objectID].shootingAccuracy = wp1[weaponID].spread * global::MainGame.MainGame.playerVehicles[playerID].weapons[objectID].accuracyAdjustment;
	}

	public void Add_First_Available_Weapon_Attachment_To_All_Category_Attach_Points(ushort vhID, ushort curStub, ushort category)
	{
		ushort weaponID = global::MainGame.MainGame.playerVehicles[vhID].weapons[curStub].weaponID;
		ushort numAttachmentPoints = wp1[weaponID].numAttachmentPoints;
		for (ushort num = 0; num < numAttachmentPoints; num++)
		{
			if (wp1[weaponID].attachmentPoints[num].category == category)
			{
				for (ushort num2 = 0; num2 < numWeaponAttachments; num2++)
				{
					if (wpnAttachments[num2].category == category && (wpnAttachments[num2].mount & wp1[weaponID].attachmentPoints[num].mount) != 0)
					{
						global::MainGame.MainGame.playerVehicles[vhID].weapons[curStub].attachments[num].attachmentID = num2;
						global::MainGame.MainGame.playerVehicles[vhID].weapons[curStub].attachments[num].status = 1;
					}
				}
			}
		}
	}

	public ushort Does_Weapon_Have_Attachment_Point_For_Category(ushort weaponID, ushort category)
	{
		ushort numAttachmentPoints = wp1[weaponID].numAttachmentPoints;
		ushort num = 0;
		for (ushort num2 = 0; num2 < numAttachmentPoints; num2++)
		{
			if (wp1[weaponID].attachmentPoints[num2].category == category)
			{
				num++;
			}
		}
		return num;
	}

	public void Weapon_Chambered(byte playerID, byte curStub)
	{
		if (playerID == 0)
		{
			global::Players.Players.needToChamber = false;
			global::MainGame.MainGame.showCrossHairs[0] = 0;
			if (global::MainGame.MainGame.usingScope)
			{
				global::Players.Players.currentView = 3;
			}
			global::Players.Players.players[playerID].playerIsMoving = 0;
		}
		mainC.programsMain.Start_Animation(playerID, ref global::Players.Players.players[playerID].jt1, ref global::Players.Players.players[playerID].animations, global::Players.Players.players[playerID].programCollection, wp1[global::Players.Players.players[playerID].primaryWeaponMountWeapon].AnimationHolding, 1f, 1f);
		global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].roundChambered = true;
	}

	public void Weapon_Reloaded(byte wpnIndex, byte playerID)
	{
		byte weaponID = global::Players.Players.players[playerID].weapon2[wpnIndex].weaponID;
		if (playerID == 0)
		{
			global::MainGame.MainGame.showCrossHairs[0] = 0;
			if (global::MainGame.MainGame.usingScope)
			{
				global::Players.Players.currentView = 3;
			}
			Load_Ammo_Clip_Into_Player_Weapon(wpnIndex, weaponID, 0, 1);
			global::Players.Players.players[playerID].playerIsMoving = 0;
		}
		else
		{
			Load_Ammo_Clip_Into_AI_Weapon(wpnIndex, weaponID, playerID, 1);
		}
		mainC.programsMain.Start_Animation(playerID, ref global::Players.Players.players[playerID].jt1, ref global::Players.Players.players[playerID].animations, global::Players.Players.players[playerID].programCollection, wp1[global::Players.Players.players[playerID].primaryWeaponMountWeapon].AnimationHolding, 1f, 1f);
		global::Players.Players.players[playerID].weapon2[wpnIndex].fired = false;
	}

	public void Player_Vehicle_Weapon_Reloaded(ushort playerID, ushort curStub, bool ammoLoadedAlready, bool useSurplusFirst)
	{
		if (playerID == 0)
		{
			global::Players.Players.reloading = false;
			global::MainGame.MainGame.showCrossHairs[0] = 0;
			if (global::MainGame.MainGame.usingScope)
			{
				global::Players.Players.currentView = 3;
			}
		}
		if (!ammoLoadedAlready)
		{
			if (!useSurplusFirst)
			{
				Load_Ammo_Clip_Into_Player_Vehicle_Weapon(playerID, curStub, 1);
			}
			else
			{
				Load_Ammo_Clip_Surplus_Into_Player_Vehicle_Weapon(playerID, curStub, 1);
			}
		}
		global::MainGame.MainGame.playerVehicles[playerID].weapons[curStub].fired = false;
	}

	public void Change_Weapon_Mode()
	{
		if (wp1[global::Players.Players.players[0].primaryWeaponMountWeapon].fireMode == 2)
		{
			global::Players.Players.fullyAutomatic = !global::Players.Players.fullyAutomatic;
		}
	}

	public void Reset_Round()
	{
		curMuzzleFlashTexture = 0;
		currentBullet = 0;
		curBallisticStrike = 0;
		mpSendWeaponFiredMsg = false;
		for (int i = 0; i < 100; i++)
		{
			bulletActive[0, i] = 0;
			bulletActive[1, i] = 0;
			bullet[i].phys1.acceleration.v[0] = 0f;
			bullet[i].phys1.acceleration.v[1] = 0f;
			bullet[i].phys1.acceleration.v[2] = -32.15223f;
			bullet[i].lightID = -1;
		}
		for (int i = 0; i < numBallisticStrikes; i++)
		{
			wpnStrike[i].status = 0;
		}
		for (int i = 0; i < numLaserLights; i++)
		{
			laserLights[i] = -1;
			laserLightsSorted[0, i] = -1;
			laserLightsSorted[1, i] = -1;
		}
		numActiveAmmoLights[0] = 0;
		numActiveAmmoLights[1] = 0;
	}

	public void Check_Weapon_Views()
	{
		byte objectID = global::MainGame.MainGame.playerVehicles[0].mounts[global::MainGame.MainGame.primaryWeaponMount].objectID;
		weaponViewEnabled = true;
		if (global::Players.Players.changingWeapons || global::MainGame.MainGame.usingScope || (global::Players.Players.reloading && !wp1[global::Players.Players.players[0].primaryWeaponMountWeapon].IronSightsWileReloading) || (global::Players.Players.chambering && !wp1[global::Players.Players.players[0].primaryWeaponMountWeapon].IronSightsWhileChambering) || !wp1[global::Players.Players.players[0].primaryWeaponMountWeapon].hasIronSights || global::MainGame.MainGame.playerVehicles[0].weapons[objectID].scopeLow > 0 || global::Players.Players.playerViewingDevice)
		{
			weaponViewEnabled = false;
		}
		scopeViewEnabled = true;
		if (global::Players.Players.changingWeapons || (global::Players.Players.reloading && !wp1[global::Players.Players.players[0].primaryWeaponMountWeapon].ScopeWileReloading) || (global::Players.Players.chambering && !wp1[global::Players.Players.players[0].primaryWeaponMountWeapon].ScopeWhileChambering) || global::MainGame.MainGame.playerVehicles[0].weapons[objectID].scopeLow < 1 || global::Players.Players.playerViewingDevice)
		{
			scopeViewEnabled = false;
		}
	}

	public void Change_Weapon_View(bool toggleScope)
	{
		switch (global::Players.Players.currentView)
		{
		case 0:
			if (toggleScope)
			{
				mainC.weaponsMain.Use_Weapon_Scope();
			}
			else if (global::MainGame.MainGame.usingIronSights)
			{
				mainC.weaponsMain.Use_Iron_Sights();
			}
			break;
		case 1:
			if (toggleScope)
			{
				mainC.weaponsMain.Use_Weapon_Scope();
			}
			else if (global::MainGame.MainGame.usingIronSights)
			{
				mainC.weaponsMain.Use_Iron_Sights();
			}
			break;
		case 2:
			global::InputHandler.InputHandler.lookMode = 1;
			if (toggleScope)
			{
				mainC.weaponsMain.Use_Weapon_Scope();
			}
			else if (!global::MainGame.MainGame.usingIronSights)
			{
				mainC.weaponsMain.Stop_Using_Iron_Sights();
			}
			break;
		case 3:
			global::MainGame.MainGame.usingIronSights = false;
			if (!scopeViewEnabled)
			{
				toggleScope = true;
			}
			if (toggleScope)
			{
				mainC.weaponsMain.Stop_Using_Weapon_Scope();
				if (global::MainGame.MainGame.usingIronSights && weaponViewEnabled)
				{
					mainC.weaponsMain.Use_Iron_Sights();
				}
			}
			break;
		}
	}

	public void Set_Cross_Hair_Position(byte threadID)
	{
		Get_LaserSite_Position(0f, 1, global::MainGame.MainGame.commanderObjectivePosition, threadID);
		float num = global::MainGame.MainGame.commanderObjectivePosition.x - global::Rendering.Rendering.camPos[global::Rendering.Rendering.rBufferID].X;
		float num2 = global::MainGame.MainGame.commanderObjectivePosition.y - global::Rendering.Rendering.camPos[global::Rendering.Rendering.rBufferID].Y;
		float num3 = global::MainGame.MainGame.commanderObjectivePosition.z - global::Rendering.Rendering.camPos[global::Rendering.Rendering.rBufferID].Z;
		float distanceToCheck = (float)Math.Sqrt(num * num + num2 * num2 + num3 * num3);
		Get_LaserSite_Position(distanceToCheck, 0, global::MainGame.MainGame.commanderObjectivePosition, threadID);
		global::Rendering.Rendering.crossHairPositionGoal[global::Rendering.Rendering.uBufferID, 0] = global::MainGame.MainGame.commanderObjectivePosition.x;
		global::Rendering.Rendering.crossHairPositionGoal[global::Rendering.Rendering.uBufferID, 1] = global::MainGame.MainGame.commanderObjectivePosition.y;
		global::Rendering.Rendering.crossHairPositionGoal[global::Rendering.Rendering.uBufferID, 2] = global::MainGame.MainGame.commanderObjectivePosition.z;
	}

	public void Calculate_LaserSite_Position(int playerID, byte threadID)
	{
		byte uBufferID = global::Rendering.Rendering.uBufferID;
		short pID = 0;
		float num = 10000f;
		_ = global::Players.Players.players[playerID].primaryWeaponMountWeapon;
		float x = global::Rendering.Rendering.camPos[uBufferID].X;
		float y = global::Rendering.Rendering.camPos[uBufferID].Y;
		float z = global::Rendering.Rendering.camPos[uBufferID].Z;
		lSite.position.v[0] = x;
		lSite.position.v[1] = y;
		lSite.position.v[2] = z;
		lSite.velocity.v[0] = global::Players.Players.playerViewMatrix[0].M21 * 100f;
		lSite.velocity.v[1] = global::Players.Players.playerViewMatrix[0].M22 * 100f;
		lSite.velocity.v[2] = global::Players.Players.playerViewMatrix[0].M23 * 100f;
		mainC.physicsMain.getPosition(ref lSite, 1f * global::Physics.Physics.timeMod);
		float num2 = (float)(int)laserDepth[uBufferID] / 255f * laserDistance;
		bulletBoxT[threadID].pos1.v[0] = x;
		bulletBoxT[threadID].pos1.v[1] = y;
		bulletBoxT[threadID].pos1.v[2] = z;
		bulletBoxT[threadID].pos2.v[0] = lSite.position.v[0];
		bulletBoxT[threadID].pos2.v[1] = lSite.position.v[1];
		bulletBoxT[threadID].pos2.v[2] = lSite.position.v[2];
		num = mainC.playersMain.Check_Player_Impact_Threaded(ref bulletBoxT[threadID], ref pID, ref pfbV1T[threadID], ref pfbV2T[threadID], 0f, -1, threadID);
		if (num < 0f || num2 < num)
		{
			global::Players.Players.players[playerID].laserDist[global::Rendering.Rendering.uBufferID] = num2;
			if (playerID == 0)
			{
				laserPosX[uBufferID] = x + global::Players.Players.playerViewMatrix[0].M21 * num2;
				laserPosY[uBufferID] = y + global::Players.Players.playerViewMatrix[0].M22 * num2;
				laserPosZ[uBufferID] = z + global::Players.Players.playerViewMatrix[0].M23 * num2;
			}
		}
		else
		{
			global::Players.Players.players[playerID].laserDist[global::Rendering.Rendering.uBufferID] = num;
			if (playerID == 0)
			{
				laserPosX[uBufferID] = pfbV1T[threadID].v[0];
				laserPosY[uBufferID] = pfbV1T[threadID].v[1];
				laserPosZ[uBufferID] = pfbV1T[threadID].v[2];
			}
		}
	}

	public void Get_LaserSite_Position(float distanceToCheck, byte mode, StructsClass.Basic_Position endPosition, byte threadID)
	{
		short pID = -1;
		float num = 0f;
		float num2 = 0f;
		float num3 = 0f;
		Vector3 InitialRayStart = default(Vector3);
		Vector3 InitialRayEnd = default(Vector3);
		Vector3 IntersectPosition = default(Vector3);
		Vector3 IntersectNormal = default(Vector3);
		int num4 = 0;
		byte objectID = global::MainGame.MainGame.playerVehicles[num4].mounts[global::MainGame.MainGame.primaryWeaponMount].objectID;
		if (mode == 0)
		{
			lSite.position.v[0] = global::Players.Players.players[num4].laserPos[global::Rendering.Rendering.uBufferID, 0];
			lSite.position.v[1] = global::Players.Players.players[num4].laserPos[global::Rendering.Rendering.uBufferID, 1];
			lSite.position.v[2] = global::Players.Players.players[num4].laserPos[global::Rendering.Rendering.uBufferID, 2];
			lSite.velocity.v[0] = global::MainGame.MainGame.playerVehicles[num4].weapons[objectID].offset[0, 5].x * distanceToCheck;
			lSite.velocity.v[1] = global::MainGame.MainGame.playerVehicles[num4].weapons[objectID].offset[0, 5].y * distanceToCheck;
			lSite.velocity.v[2] = global::MainGame.MainGame.playerVehicles[num4].weapons[objectID].offset[0, 5].z * distanceToCheck;
		}
		else
		{
			lSite.position.v[0] = global::Rendering.Rendering.camPos[global::Rendering.Rendering.rBufferID].X;
			lSite.position.v[1] = global::Rendering.Rendering.camPos[global::Rendering.Rendering.rBufferID].Y;
			lSite.position.v[2] = global::Rendering.Rendering.camPos[global::Rendering.Rendering.rBufferID].Z;
			lSite.velocity.v[0] = (0f - global::Rendering.Rendering.matrixVInverse.M31) * 100f;
			lSite.velocity.v[1] = (0f - global::Rendering.Rendering.matrixVInverse.M32) * 100f;
			lSite.velocity.v[2] = (0f - global::Rendering.Rendering.matrixVInverse.M33) * 100f;
		}
		float num5 = lSite.position.v[0];
		float num6 = lSite.position.v[1];
		float num7 = lSite.position.v[2];
		mainC.physicsMain.getPosition(ref lSite, 1f * global::Physics.Physics.timeMod);
		float num8 = lSite.position.v[0] - num5;
		float num9 = lSite.position.v[1] - num6;
		float num10 = lSite.position.v[2] - num7;
		float num11 = (float)Math.Sqrt(Math.Pow(num8, 2.0) + Math.Pow(num9, 2.0) + Math.Pow(num10, 2.0));
		num8 /= num11;
		num9 /= num11;
		num10 /= num11;
		bulletBoxT[threadID].pos1.v[0] = num5;
		bulletBoxT[threadID].pos1.v[1] = num6;
		bulletBoxT[threadID].pos1.v[2] = num7;
		bulletBoxT[threadID].pos2.v[0] = lSite.position.v[0];
		bulletBoxT[threadID].pos2.v[1] = lSite.position.v[1];
		bulletBoxT[threadID].pos2.v[2] = lSite.position.v[2];
		mainC.mapsMain.Set_Position_OutsideBoundary(ref bulletBoxT[threadID]);
		float num12 = mainC.playersMain.Check_Player_Impact_Threaded(ref bulletBoxT[threadID], ref pID, ref pfbV1T[threadID], ref pfbV2T[threadID], 0.01f, 0, threadID);
		float num13 = num11;
		int num14 = 0;
		int Number = -1;
		short returnValueZoneCheckIndex = 0;
		InitialRayStart.X = num5;
		InitialRayStart.Y = num6;
		InitialRayStart.Z = num7;
		InitialRayEnd.X = lSite.position.v[0];
		InitialRayEnd.Y = lSite.position.v[1];
		InitialRayEnd.Z = lSite.position.v[2];
		ushort returnValueZoneCheckObjID;
		while (mainC.zonesMain.Check_Zones_For_Ray(num5, num6, num7, InitialRayEnd.X, InitialRayEnd.Y, InitialRayEnd.Z, 5f, returnValueZoneCheckIndex, 1, out returnValueZoneCheckIndex, out returnValueZoneCheckObjID))
		{
			int numObjects = Zones.zones[returnValueZoneCheckObjID].zoneList.numObjects;
			for (int i = 0; i < numObjects; i++)
			{
				if (!mainC.collisionMain.Check_Polygon_Ray_Collision_Projectile(Zones.zones[returnValueZoneCheckObjID].zoneList.oList[i], Number, ref InitialRayStart, ref InitialRayEnd, ref Zones.zones[returnValueZoneCheckObjID].zoneList.matrixList[i], out var distance, out IntersectPosition, out IntersectNormal, out Number, threadID))
				{
					continue;
				}
				if (num14 == 0 || distance < num13)
				{
					num = IntersectPosition.X;
					num2 = IntersectPosition.Y;
					num3 = IntersectPosition.Z;
					if (distance >= 0f)
					{
						InitialRayEnd.X = IntersectPosition.X;
						InitialRayEnd.Y = IntersectPosition.Y;
						InitialRayEnd.Z = IntersectPosition.Z;
					}
					num13 = distance;
					_ = Zones.zones[returnValueZoneCheckObjID].zoneList.gidList[i];
				}
				num14 = 8;
			}
		}
		if (num14 == 8)
		{
			if (num13 < num12 || num12 < 0f)
			{
				lSite.position.v[0] = num;
				lSite.position.v[1] = num2;
				lSite.position.v[2] = num3;
				num12 = num13;
			}
			else if (pID > -1)
			{
				lSite.position.v[0] = pfbV1T[threadID].v[0];
				lSite.position.v[1] = pfbV1T[threadID].v[1];
				lSite.position.v[2] = pfbV1T[threadID].v[2];
				num14 = 1;
			}
		}
		else if (num12 >= 0f)
		{
			lSite.position.v[0] = pfbV1T[threadID].v[0];
			lSite.position.v[1] = pfbV1T[threadID].v[1];
			lSite.position.v[2] = pfbV1T[threadID].v[2];
			num14 = 1;
		}
		global::Players.Players.players[0].laserDist[global::Rendering.Rendering.uBufferID] = num12;
		laserPosX[global::Rendering.Rendering.uBufferID] = lSite.position.v[0];
		laserPosY[global::Rendering.Rendering.uBufferID] = lSite.position.v[1];
		laserPosZ[global::Rendering.Rendering.uBufferID] = lSite.position.v[2];
		endPosition.x = lSite.position.v[0];
		endPosition.y = lSite.position.v[1];
		endPosition.z = lSite.position.v[2];
	}

	public void Get_LaserSite_Position_Static(int playerID, StructsClass.Basic_Position endPosition)
	{
		lSite.position.v[0] = global::Players.Players.players[playerID].laserPos[global::Rendering.Rendering.uBufferID, 0];
		lSite.position.v[1] = global::Players.Players.players[playerID].laserPos[global::Rendering.Rendering.uBufferID, 1];
		lSite.position.v[2] = global::Players.Players.players[playerID].laserPos[global::Rendering.Rendering.uBufferID, 2];
		lSite.velocity.v[0] = global::Players.Players.players[playerID].weapon1.offset[0, 5].v[0] * 100f;
		lSite.velocity.v[1] = global::Players.Players.players[playerID].weapon1.offset[0, 5].v[1] * 100f;
		lSite.velocity.v[2] = global::Players.Players.players[playerID].weapon1.offset[0, 5].v[2] * 100f;
		mainC.physicsMain.getPosition(ref lSite, 1f * global::Physics.Physics.timeMod);
		endPosition.x = lSite.position.v[0];
		endPosition.y = lSite.position.v[1];
		endPosition.z = lSite.position.v[2];
	}

	public void Find_Closest_Player_2D_Coordinates()
	{
		Vector3 source = default(Vector3);
		ushort num = global::MainGame.MainGame.maxGamePlayers;
		float num2 = global::Rendering.Rendering.middleOfScreenLenghtToCorner;
		for (ushort num3 = 1; num3 < global::MainGame.MainGame.maxGamePlayers; num3++)
		{
			float num4 = global::MainGame.MainGame.playerVehicles[num3].ph1.x - global::Players.Players.players[0].laserPos[global::Rendering.Rendering.uBufferID, 0];
			float num5 = global::MainGame.MainGame.playerVehicles[num3].ph1.y - global::Players.Players.players[0].laserPos[global::Rendering.Rendering.uBufferID, 1];
			float num6 = global::MainGame.MainGame.playerVehicles[num3].ph1.z - global::Players.Players.players[0].laserPos[global::Rendering.Rendering.uBufferID, 2];
			float num7 = num4 * global::Players.Players.players[0].laserDir[global::Rendering.Rendering.uBufferID, 0] + num5 * global::Players.Players.players[0].laserDir[global::Rendering.Rendering.uBufferID, 0] + num6 * global::Players.Players.players[0].laserDir[global::Rendering.Rendering.uBufferID, 0];
			if (num7 > 0f)
			{
				source.X = global::MainGame.MainGame.playerVehicles[num3].ph1.x;
				source.Y = global::MainGame.MainGame.playerVehicles[num3].ph1.y;
				source.Z = global::MainGame.MainGame.playerVehicles[num3].ph1.z;
				source = global::Rendering.Rendering.rGraphics.Viewport.Project(source, global::Rendering.Rendering.matrixP, global::Rendering.Rendering.matrixV, global::Rendering.Rendering.matrixI);
				num4 = Math.Abs(source.X - global::Rendering.Rendering.crossHairPositionGoal[global::Rendering.Rendering.rBufferID, 0]);
				num5 = Math.Abs(source.Y - global::Rendering.Rendering.crossHairPositionGoal[global::Rendering.Rendering.rBufferID, 1]);
				if ((num7 = (float)Math.Sqrt(num4 * num4 + num5 * num5)) <= num2)
				{
					num = num3;
					num2 = num7;
				}
			}
		}
		if (num < global::MainGame.MainGame.maxGamePlayers)
		{
			float num4 = global::MainGame.MainGame.playerVehicles[num].ph1.x - global::Players.Players.players[0].laserPos[global::Rendering.Rendering.uBufferID, 0];
			float num5 = global::MainGame.MainGame.playerVehicles[num].ph1.y - global::Players.Players.players[0].laserPos[global::Rendering.Rendering.uBufferID, 1];
			float num6 = global::MainGame.MainGame.playerVehicles[num].ph1.z - global::Players.Players.players[0].laserPos[global::Rendering.Rendering.uBufferID, 2];
			float num7 = (float)Math.Sqrt(num4 * num4 + num5 * num5 + num6 * num6);
			num4 = global::MainGame.MainGame.commanderObjectivePosition.x - global::Players.Players.players[0].laserPos[global::Rendering.Rendering.uBufferID, 0];
			num5 = global::MainGame.MainGame.commanderObjectivePosition.y - global::Players.Players.players[0].laserPos[global::Rendering.Rendering.uBufferID, 1];
			num6 = global::MainGame.MainGame.commanderObjectivePosition.z - global::Players.Players.players[0].laserPos[global::Rendering.Rendering.uBufferID, 2];
			num2 = (float)Math.Sqrt(num4 * num4 + num5 * num5 + num6 * num6);
			if (num2 != 0f)
			{
				num4 /= num2;
				num5 /= num2;
				num6 /= num2;
			}
			global::MainGame.MainGame.commanderObjectivePosition.x = global::Players.Players.players[0].laserPos[global::Rendering.Rendering.uBufferID, 0] + num4 * num7;
			global::MainGame.MainGame.commanderObjectivePosition.y = global::Players.Players.players[0].laserPos[global::Rendering.Rendering.uBufferID, 1] + num5 * num7;
			global::MainGame.MainGame.commanderObjectivePosition.z = global::Players.Players.players[0].laserPos[global::Rendering.Rendering.uBufferID, 2] + num6 * num7;
		}
	}

	public byte Get_Weapon_Index(ushort playerID, byte weaponID)
	{
		sbyte numAvailableWeapons = global::Players.Players.players[playerID].numAvailableWeapons;
		for (byte b = 0; b < numAvailableWeapons; b++)
		{
			if (global::Players.Players.players[playerID].weaponList[b] == (sbyte)weaponID)
			{
				return b;
			}
		}
		return 0;
	}

	public byte Count_Active_Weapons_Matching_Clip(ushort playerID, byte clipID)
	{
		byte b = 0;
		for (byte b2 = 0; b2 < global::MainGame.MainGame.playerVehicles[playerID].numWeapons; b2++)
		{
			if (global::MainGame.MainGame.playerVehicles[playerID].weapons[b2].curClip == clipID)
			{
				b++;
			}
		}
		return b;
	}

	public byte Count_Attachments_That_Fit_Mount(byte attachmentType, byte mountID)
	{
		byte b = 0;
		switch (attachmentType)
		{
		case 0:
		{
			for (byte b2 = 0; b2 < numScopes; b2++)
			{
				if ((scopes[b2].mount & mountID) > 0)
				{
					b++;
				}
			}
			break;
		}
		case 1:
		{
			for (byte b2 = 0; b2 < numForeGrips; b2++)
			{
				if ((foreGrips[b2].mount & mountID) > 0)
				{
					b++;
				}
			}
			break;
		}
		case 2:
		{
			for (byte b2 = 0; b2 < numBarrels; b2++)
			{
				if ((barrels[b2].mount & mountID) > 0)
				{
					b++;
				}
			}
			break;
		}
		case 3:
		{
			for (byte b2 = 0; b2 < numEnergyDevices; b2++)
			{
				if ((energyDevices[b2].mount & mountID) > 0)
				{
					b++;
				}
			}
			break;
		}
		}
		return b;
	}

	public void Initialize_Player_Weapon_Preferences(byte weaponCount)
	{
		for (ushort num = 0; num < 1; num++)
		{
			if (weaponCount > 0)
			{
				global::Players.Players.playerPrefsSP[num].weapons = new StructsClass.weapon_preference[weaponCount];
				global::Players.Players.playerPrefsMP[num].weapons = new StructsClass.weapon_preference[weaponCount];
			}
			global::Players.Players.playerPrefsSP[num].numWeapons = weaponCount;
			global::Players.Players.playerPrefsMP[num].numWeapons = weaponCount;
			for (ushort num2 = 0; num2 < weaponCount; num2++)
			{
				global::Players.Players.playerPrefsSP[num].weapons[num2].weaponID = byte.MaxValue;
				global::Players.Players.playerPrefsSP[num].weapons[num2].scopeID = 0;
				global::Players.Players.playerPrefsSP[num].weapons[num2].foreGripID = 0;
				global::Players.Players.playerPrefsSP[num].weapons[num2].barrelID = 0;
				global::Players.Players.playerPrefsSP[num].weapons[num2].energyDeviceID = 0;
				global::Players.Players.playerPrefsSP[num].weapons[num2].skinID = 0;
				global::Players.Players.playerPrefsSP[num].weapons[num2].tauntID = 0;
				global::Players.Players.playerPrefsMP[num].weapons[num2].weaponID = byte.MaxValue;
				global::Players.Players.playerPrefsMP[num].weapons[num2].scopeID = 0;
				global::Players.Players.playerPrefsMP[num].weapons[num2].foreGripID = 0;
				global::Players.Players.playerPrefsMP[num].weapons[num2].barrelID = 0;
				global::Players.Players.playerPrefsMP[num].weapons[num2].energyDeviceID = 0;
				global::Players.Players.playerPrefsMP[num].weapons[num2].skinID = 0;
				global::Players.Players.playerPrefsMP[num].weapons[num2].tauntID = 0;
			}
		}
	}

	public void Set_Weapon_Preference(byte gameTypeSPorMP, ushort playerID, byte weaponID, byte type, byte value)
	{
		switch (gameTypeSPorMP)
		{
		case 0:
		{
			ushort num = global::Players.Players.playerPrefsSP[playerID].numWeapons;
			for (ushort num2 = 0; num2 < num; num2++)
			{
				if (global::Players.Players.playerPrefsSP[playerID].weapons[num2].weaponID == weaponID)
				{
					num = num2;
					break;
				}
			}
			if (num == global::Players.Players.playerPrefsSP[playerID].numWeapons)
			{
				for (ushort num2 = 0; num2 < num; num2++)
				{
					if (global::Players.Players.playerPrefsSP[playerID].weapons[num2].weaponID == byte.MaxValue)
					{
						num = num2;
						break;
					}
				}
			}
			if (num == global::Players.Players.playerPrefsSP[playerID].numWeapons)
			{
				Expand_Weapon_Preferences(playerID, (byte)num);
			}
			global::Players.Players.playerPrefsSP[playerID].weapons[num].weaponID = weaponID;
			switch (type)
			{
			case 0:
				global::Players.Players.playerPrefsSP[playerID].weapons[num].scopeID = value;
				break;
			case 1:
				global::Players.Players.playerPrefsSP[playerID].weapons[num].foreGripID = value;
				break;
			case 2:
				global::Players.Players.playerPrefsSP[playerID].weapons[num].barrelID = value;
				break;
			case 3:
				global::Players.Players.playerPrefsSP[playerID].weapons[num].energyDeviceID = value;
				break;
			case 4:
				global::Players.Players.playerPrefsSP[playerID].weapons[num].skinID = value;
				break;
			case 5:
				global::Players.Players.playerPrefsSP[playerID].weapons[num].tauntID = value;
				break;
			}
			break;
		}
		case 1:
		{
			ushort num = global::Players.Players.playerPrefsMP[playerID].numWeapons;
			for (ushort num2 = 0; num2 < num; num2++)
			{
				if (global::Players.Players.playerPrefsMP[playerID].weapons[num2].weaponID == weaponID)
				{
					num = num2;
					break;
				}
			}
			if (num == global::Players.Players.playerPrefsMP[playerID].numWeapons)
			{
				for (ushort num2 = 0; num2 < num; num2++)
				{
					if (global::Players.Players.playerPrefsMP[playerID].weapons[num2].weaponID == byte.MaxValue)
					{
						num = num2;
						break;
					}
				}
			}
			if (num == global::Players.Players.playerPrefsMP[playerID].numWeapons)
			{
				Expand_Weapon_Preferences(playerID, (byte)num);
			}
			global::Players.Players.playerPrefsMP[playerID].weapons[num].weaponID = weaponID;
			switch (type)
			{
			case 0:
				global::Players.Players.playerPrefsMP[playerID].weapons[num].scopeID = value;
				break;
			case 1:
				global::Players.Players.playerPrefsMP[playerID].weapons[num].foreGripID = value;
				break;
			case 2:
				global::Players.Players.playerPrefsMP[playerID].weapons[num].barrelID = value;
				break;
			case 3:
				global::Players.Players.playerPrefsMP[playerID].weapons[num].energyDeviceID = value;
				break;
			case 4:
				global::Players.Players.playerPrefsMP[playerID].weapons[num].skinID = value;
				break;
			case 5:
				global::Players.Players.playerPrefsMP[playerID].weapons[num].tauntID = value;
				break;
			}
			break;
		}
		}
	}

	public byte Get_Weapon_Preference(ushort playerID, byte weaponID, byte type)
	{
		switch (global::MainGame.MainGame.gameMode)
		{
		case 0:
		{
			ushort num = global::Players.Players.playerPrefsSP[playerID].numWeapons;
			for (ushort num2 = 0; num2 < num; num2++)
			{
				if (global::Players.Players.playerPrefsSP[playerID].weapons[num2].weaponID == weaponID)
				{
					num = num2;
					break;
				}
			}
			if (num == global::Players.Players.playerPrefsSP[playerID].numWeapons)
			{
				return 0;
			}
			switch (type)
			{
			case 0:
				return global::Players.Players.playerPrefsSP[playerID].weapons[num].scopeID;
			case 1:
				return global::Players.Players.playerPrefsSP[playerID].weapons[num].foreGripID;
			case 2:
				return global::Players.Players.playerPrefsSP[playerID].weapons[num].barrelID;
			case 3:
				return global::Players.Players.playerPrefsSP[playerID].weapons[num].energyDeviceID;
			case 4:
				return global::Players.Players.playerPrefsSP[playerID].weapons[num].skinID;
			case 5:
				return global::Players.Players.playerPrefsSP[playerID].weapons[num].tauntID;
			}
			break;
		}
		case 1:
		{
			ushort num = global::Players.Players.playerPrefsMP[playerID].numWeapons;
			for (ushort num2 = 0; num2 < num; num2++)
			{
				if (global::Players.Players.playerPrefsMP[playerID].weapons[num2].weaponID == weaponID)
				{
					num = num2;
					break;
				}
			}
			if (num == global::Players.Players.playerPrefsMP[playerID].numWeapons)
			{
				return 0;
			}
			switch (type)
			{
			case 0:
				return global::Players.Players.playerPrefsMP[playerID].weapons[num].scopeID;
			case 1:
				return global::Players.Players.playerPrefsMP[playerID].weapons[num].foreGripID;
			case 2:
				return global::Players.Players.playerPrefsMP[playerID].weapons[num].barrelID;
			case 3:
				return global::Players.Players.playerPrefsMP[playerID].weapons[num].energyDeviceID;
			case 4:
				return global::Players.Players.playerPrefsMP[playerID].weapons[num].skinID;
			case 5:
				return global::Players.Players.playerPrefsMP[playerID].weapons[num].tauntID;
			}
			break;
		}
		}
		return 0;
	}

	public void Expand_Weapon_Preferences(ushort playerID, byte minimumIndex)
	{
		byte b = global::Players.Players.playerPrefsSP[playerID].numWeapons;
		StructsClass.weapon_preference[] array = new StructsClass.weapon_preference[1];
		StructsClass.weapon_preference[] array2 = new StructsClass.weapon_preference[1];
		byte b2;
		if (b > 0)
		{
			array = new StructsClass.weapon_preference[b];
			array2 = new StructsClass.weapon_preference[b];
			for (b2 = 0; b2 < b; b2++)
			{
				array[b2].weaponID = global::Players.Players.playerPrefsSP[playerID].weapons[b2].weaponID;
				array[b2].scopeID = global::Players.Players.playerPrefsSP[playerID].weapons[b2].scopeID;
				array[b2].foreGripID = global::Players.Players.playerPrefsSP[playerID].weapons[b2].foreGripID;
				array[b2].barrelID = global::Players.Players.playerPrefsSP[playerID].weapons[b2].barrelID;
				array[b2].energyDeviceID = global::Players.Players.playerPrefsSP[playerID].weapons[b2].energyDeviceID;
				array[b2].skinID = global::Players.Players.playerPrefsSP[playerID].weapons[b2].skinID;
				array[b2].tauntID = global::Players.Players.playerPrefsSP[playerID].weapons[b2].tauntID;
				array2[b2].weaponID = global::Players.Players.playerPrefsMP[playerID].weapons[b2].weaponID;
				array2[b2].scopeID = global::Players.Players.playerPrefsMP[playerID].weapons[b2].scopeID;
				array2[b2].foreGripID = global::Players.Players.playerPrefsMP[playerID].weapons[b2].foreGripID;
				array2[b2].barrelID = global::Players.Players.playerPrefsMP[playerID].weapons[b2].barrelID;
				array2[b2].energyDeviceID = global::Players.Players.playerPrefsMP[playerID].weapons[b2].energyDeviceID;
				array2[b2].skinID = global::Players.Players.playerPrefsMP[playerID].weapons[b2].skinID;
				array2[b2].tauntID = global::Players.Players.playerPrefsMP[playerID].weapons[b2].tauntID;
			}
		}
		minimumIndex++;
		global::Players.Players.playerPrefsSP[playerID].weapons = new StructsClass.weapon_preference[minimumIndex];
		global::Players.Players.playerPrefsSP[playerID].numWeapons = minimumIndex;
		global::Players.Players.playerPrefsMP[playerID].weapons = new StructsClass.weapon_preference[minimumIndex];
		global::Players.Players.playerPrefsMP[playerID].numWeapons = minimumIndex;
		for (b2 = 0; b2 < b; b2++)
		{
			global::Players.Players.playerPrefsSP[playerID].weapons[b2].weaponID = array[b2].weaponID;
			global::Players.Players.playerPrefsSP[playerID].weapons[b2].scopeID = array[b2].scopeID;
			global::Players.Players.playerPrefsSP[playerID].weapons[b2].foreGripID = array[b2].foreGripID;
			global::Players.Players.playerPrefsSP[playerID].weapons[b2].barrelID = array[b2].barrelID;
			global::Players.Players.playerPrefsSP[playerID].weapons[b2].energyDeviceID = array[b2].energyDeviceID;
			global::Players.Players.playerPrefsSP[playerID].weapons[b2].skinID = array[b2].skinID;
			global::Players.Players.playerPrefsSP[playerID].weapons[b2].tauntID = array[b2].tauntID;
			global::Players.Players.playerPrefsMP[playerID].weapons[b2].weaponID = array2[b2].weaponID;
			global::Players.Players.playerPrefsMP[playerID].weapons[b2].scopeID = array2[b2].scopeID;
			global::Players.Players.playerPrefsMP[playerID].weapons[b2].foreGripID = array2[b2].foreGripID;
			global::Players.Players.playerPrefsMP[playerID].weapons[b2].barrelID = array2[b2].barrelID;
			global::Players.Players.playerPrefsMP[playerID].weapons[b2].energyDeviceID = array2[b2].energyDeviceID;
			global::Players.Players.playerPrefsMP[playerID].weapons[b2].skinID = array2[b2].skinID;
			global::Players.Players.playerPrefsMP[playerID].weapons[b2].tauntID = array2[b2].tauntID;
		}
		while (b2 < minimumIndex)
		{
			global::Players.Players.playerPrefsSP[playerID].weapons[b2].weaponID = byte.MaxValue;
			global::Players.Players.playerPrefsMP[playerID].weapons[b2].weaponID = byte.MaxValue;
			b2++;
		}
	}

	static Weapons()
	{
		byte[] array = new byte[2];
		numActiveAmmoLights = array;
		laserDepth = new byte[2];
		bulletActive = new byte[2, 100];
		numAmmo = 0;
		fs = -1.0;
		currentBullet = 0;
		showTargetCrosshairTimer = 0f;
		fau4 = new float[4];
		far4 = new float[4];
		far3 = new float[3];
		laserPosX = new float[2];
		laserPosY = new float[2];
		laserPosZ = new float[2];
		utilString1 = "Killed ";
		utilString2 = " with the ";
		ppwV1 = new StructsClass.vtex();
		ppwV2 = new StructsClass.vtex();
		apffwV1 = new StructsClass.vtex();
		apffwV2 = new StructsClass.vtex();
		abV1 = new StructsClass.vtex();
		ammoLightPos = new Vector4[2, 2];
		ammoLightColor = new Vector4[2, 2];
		bullet = new StructsClass.Ballistics[100];
		lSite = default(StructsClass.physics);
		bulletBoxT = new StructsClass.particle_list[5];
		ls1 = default(Vector3);
		ls2 = new Vector3(0f, 0f, 0f);
		ls3 = new Vector3(0f, 0f, 1f);
		ls4 = new Vector3(1f, 0f, 0f);
		lsCenter = default(Vector3);
		lsNormal = default(Vector3);
		lsTangent = default(Vector3);
		pfbV1T = new StructsClass.vtex[5];
		pfbV2T = new StructsClass.vtex[5];
	}
}
