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

namespace Joints;

public class Joints
{
	public static volatile float[] programTime = new float[5];

	public static volatile float[] surplusTime = new float[5];

	public static volatile float[] stepTime = new float[5];

	public static short frameCnt = 7;

	public static int sizeCJVFloats = 0;

	public static long numJointsBasic = 0L;

	public static float[] jCol;

	public static StructsClass.joint_basic[] jtb1;

	public static StructsClass.vtex[] jV1;

	public static StructsClass.vtex[] jV2;

	public static StructsClass.vtex[] jV3;

	public static StructsClass.JointCollection[] playerJoints;

	public static float[,] cosA;

	public static float[,] sinA;

	public static StructsClass.vtex[] cjVv1 = new StructsClass.vtex[5];

	public static StructsClass.vtex[] cpjvVt1 = new StructsClass.vtex[5];

	public static StructsClass.vtex[] cpjvVt2 = new StructsClass.vtex[5];

	public static StructsClass.texcoord[] cjtT1 = new StructsClass.texcoord[5];

	public static StructsClass.texcoord[] cjtT2 = new StructsClass.texcoord[5];

	public static StructsClass.texcoord[] cjtT3 = new StructsClass.texcoord[5];

	public static StructsClass.vtex[] crnV1 = new StructsClass.vtex[5];

	public static StructsClass.vtex[] crnV2 = new StructsClass.vtex[5];

	public static StructsClass.vtex cjcwpV2 = new StructsClass.vtex();

	public static StructsClass.vtex cjcwpV3 = new StructsClass.vtex();

	public static StructsClass.vtex cjcwpV4 = new StructsClass.vtex();

	public static StructsClass.vtex[] cjcwpV2T = new StructsClass.vtex[5];

	public static StructsClass.vtex[] cjcwpV3T = new StructsClass.vtex[5];

	public static StructsClass.vtex[] cjcwpV4T = new StructsClass.vtex[5];

	public static object playerLock = new object();

	public static int pjNumJoints = 0;

	public static int[] pjJointsNumSubs;

	public static Matrix matrixI = Matrix.Identity;

	public static bool checkJointCollision = true;

	public static bool mt = true;

	public static float test = 49.4999f;

	public static byte[] testb = new byte[5];

	public static Game1.MasterCollection mainC;

	public static Color jColor = new Color(1f, 1f, 1f, 1f);

	public static Vector3 rpjVec = default(Vector3);

	public static Vector3 rpjNorm = default(Vector3);

	public static Vector3 rpjOrigin = default(Vector3);

	public static Vector3 rpjTemp = Vector3.Zero;

	public void Init_Vars(Game1.MasterCollection master)
	{
		mainC = master;
		for (int i = 0; i < 5; i++)
		{
			cjcwpV2T[i] = new StructsClass.vtex();
			cjcwpV3T[i] = new StructsClass.vtex();
			cjcwpV4T[i] = new StructsClass.vtex();
			cjVv1[i] = new StructsClass.vtex();
			cpjvVt1[i] = new StructsClass.vtex();
			cpjvVt2[i] = new StructsClass.vtex();
			cjtT1[i] = new StructsClass.texcoord();
			cjtT2[i] = new StructsClass.texcoord();
			cjtT3[i] = new StructsClass.texcoord();
			crnV1[i] = new StructsClass.vtex();
			crnV2[i] = new StructsClass.vtex();
		}
	}

	public static void Process_Joints_Threaded(short playerID, float progTime, byte threadID)
	{
		byte b = 5;
		_ = global::Rendering.Rendering.uBufferID;
		programTime[threadID] = progTime / global::Physics.Physics.timeMod;
		sbyte primaryWeaponMountWeapon = global::Players.Players.players[playerID].primaryWeaponMountWeapon;
		short numJoints = playerJoints[global::Players.Players.players[playerID].jointPackage].numJoints;
		while (programTime[threadID] > 0f && b-- > 0)
		{
			mainC.programsMain.Process_Program((byte)playerID, ref global::Players.Players.players[playerID].jt1, ref global::Players.Players.players[playerID].pg1, global::Players.Players.players[playerID].programCollection);
			programTime[threadID] = surplusTime[threadID];
		}
		mainC.programsMain.Process_Animations(progTime / global::Physics.Physics.timeMod, (ushort)playerID, ref global::Players.Players.players[playerID].jt1, ref global::Players.Players.players[playerID].animations, global::Players.Players.players[playerID].programCollection);
		Redo_Bounding_Box(playerID, primaryWeaponMountWeapon);
		Moved_Joint_Calculations(playerID, numJoints);
	}

	public static void Adjust_Joints_For_New_Rotation_Threaded(int playerID)
	{
		Matrix matrix = default(Matrix);
		Matrix result = default(Matrix);
		byte uBufferID = global::Rendering.Rendering.uBufferID;
		long num = playerJoints[global::Players.Players.players[playerID].jointPackage].numJoints;
		for (int i = 0; i < num; i++)
		{
			global::Players.Players.players[playerID].jt1[i].matrixReady = false;
		}
		for (int i = 0; i < num; i++)
		{
			if (global::Players.Players.players[playerID].jt1[i].matrixReady)
			{
				continue;
			}
			matrix = global::Players.Players.players[playerID].mv[uBufferID];
			for (int num2 = global::Players.Players.players[playerID].jt1[i].parentCount - 1; num2 > -1; num2--)
			{
				int num3 = global::Players.Players.players[playerID].jt1[i].parentList[num2];
				if (!global::Players.Players.players[playerID].jt1[num3].matrixReady)
				{
					Matrix.CreateTranslation(global::Players.Players.players[playerID].jt1[num3].x, global::Players.Players.players[playerID].jt1[num3].y, global::Players.Players.players[playerID].jt1[num3].z, out result);
					matrix = result * matrix;
					Matrix.CreateRotationX(global::Players.Players.players[playerID].jt1[num3].angleX * ((float)Math.PI / 180f), out result);
					matrix = result * matrix;
					Matrix.CreateRotationY(global::Players.Players.players[playerID].jt1[num3].angleY * ((float)Math.PI / 180f), out result);
					matrix = result * matrix;
					Matrix.CreateRotationZ(global::Players.Players.players[playerID].jt1[num3].angleZ * ((float)Math.PI / 180f), out result);
					matrix = result * matrix;
					Matrix.CreateRotationZ(global::Players.Players.players[playerID].jt1[num3].rotZ * ((float)Math.PI / 180f), out result);
					matrix = result * matrix;
					Matrix.CreateRotationY(global::Players.Players.players[playerID].jt1[num3].rotY * ((float)Math.PI / 180f), out result);
					matrix = result * matrix;
					Matrix.CreateRotationX(global::Players.Players.players[playerID].jt1[num3].rotX * ((float)Math.PI / 180f), out result);
					matrix = result * matrix;
					global::Players.Players.players[playerID].jt1[num3].mv[uBufferID].M11 = matrix.M11;
					global::Players.Players.players[playerID].jt1[num3].mv[uBufferID].M12 = matrix.M12;
					global::Players.Players.players[playerID].jt1[num3].mv[uBufferID].M13 = matrix.M13;
					global::Players.Players.players[playerID].jt1[num3].mv[uBufferID].M14 = matrix.M14;
					global::Players.Players.players[playerID].jt1[num3].mv[uBufferID].M21 = matrix.M21;
					global::Players.Players.players[playerID].jt1[num3].mv[uBufferID].M22 = matrix.M22;
					global::Players.Players.players[playerID].jt1[num3].mv[uBufferID].M23 = matrix.M23;
					global::Players.Players.players[playerID].jt1[num3].mv[uBufferID].M24 = matrix.M24;
					global::Players.Players.players[playerID].jt1[num3].mv[uBufferID].M31 = matrix.M31;
					global::Players.Players.players[playerID].jt1[num3].mv[uBufferID].M32 = matrix.M32;
					global::Players.Players.players[playerID].jt1[num3].mv[uBufferID].M33 = matrix.M33;
					global::Players.Players.players[playerID].jt1[num3].mv[uBufferID].M34 = matrix.M34;
					global::Players.Players.players[playerID].jt1[num3].mv[uBufferID].M41 = matrix.M41;
					global::Players.Players.players[playerID].jt1[num3].mv[uBufferID].M42 = matrix.M42;
					global::Players.Players.players[playerID].jt1[num3].mv[uBufferID].M43 = matrix.M43;
					global::Players.Players.players[playerID].jt1[num3].mv[uBufferID].M44 = matrix.M44;
					global::Players.Players.players[playerID].jt1[num3].matrixReady = true;
				}
				else if (num2 > 0)
				{
					matrix = global::Players.Players.players[playerID].jt1[num3].mv[uBufferID];
				}
			}
			global::Players.Players.players[playerID].jt1[i].mv[uBufferID].M11 = matrix.M11;
			global::Players.Players.players[playerID].jt1[i].mv[uBufferID].M12 = matrix.M12;
			global::Players.Players.players[playerID].jt1[i].mv[uBufferID].M13 = matrix.M13;
			global::Players.Players.players[playerID].jt1[i].mv[uBufferID].M14 = matrix.M14;
			global::Players.Players.players[playerID].jt1[i].mv[uBufferID].M21 = matrix.M21;
			global::Players.Players.players[playerID].jt1[i].mv[uBufferID].M22 = matrix.M22;
			global::Players.Players.players[playerID].jt1[i].mv[uBufferID].M23 = matrix.M23;
			global::Players.Players.players[playerID].jt1[i].mv[uBufferID].M24 = matrix.M24;
			global::Players.Players.players[playerID].jt1[i].mv[uBufferID].M31 = matrix.M31;
			global::Players.Players.players[playerID].jt1[i].mv[uBufferID].M32 = matrix.M32;
			global::Players.Players.players[playerID].jt1[i].mv[uBufferID].M33 = matrix.M33;
			global::Players.Players.players[playerID].jt1[i].mv[uBufferID].M34 = matrix.M34;
			global::Players.Players.players[playerID].jt1[i].mv[uBufferID].M41 = matrix.M41;
			global::Players.Players.players[playerID].jt1[i].mv[uBufferID].M42 = matrix.M42;
			global::Players.Players.players[playerID].jt1[i].mv[uBufferID].M43 = matrix.M43;
			global::Players.Players.players[playerID].jt1[i].mv[uBufferID].M44 = matrix.M44;
			global::Players.Players.players[playerID].jt1[i].matrixReady = true;
		}
	}

	public static void Reset_Joint_Data(short playerID)
	{
		short numJoints = playerJoints[global::Players.Players.players[playerID].jointPackage].numJoints;
		for (short num = 0; num < numJoints; num++)
		{
			global::Players.Players.players[playerID].jt1[num].redoNorms = false;
			global::Players.Players.players[playerID].jt1[num].matrixReady = false;
			global::Players.Players.players[playerID].jt1[num].angleMoved = 0f;
			global::Players.Players.players[playerID].jt1[num].pivotMoved = 0f;
			global::Players.Players.players[playerID].jt1[num].pivot2Moved = 0f;
		}
	}

	public static void Reset_Joint_Rotations_To_Zero(short playerID)
	{
		short numJoints = playerJoints[global::Players.Players.players[playerID].jointPackage].numJoints;
		for (short num = 0; num < numJoints; num++)
		{
			global::Players.Players.players[playerID].jt1[num].rotX = 0f;
			global::Players.Players.players[playerID].jt1[num].rotZ = 0f;
			global::Players.Players.players[playerID].jt1[num].rotY = 0f;
			global::Players.Players.players[playerID].jt1[num].targetAngle = 0f;
			global::Players.Players.players[playerID].jt1[num].targetPivot = 0f;
			global::Players.Players.players[playerID].jt1[num].targetPivot2 = 0f;
			global::Players.Players.players[playerID].jt1[num].mvAnimation = Matrix.Identity;
		}
	}

	public static void Do_Joint_Calculations(short playerID, short numPlayerJoints, byte threadID)
	{
		bool flag = false;
		stepTime[threadID] = programTime[threadID];
		surplusTime[threadID] = programTime[threadID];
		for (short num = 0; num < numPlayerJoints; num++)
		{
			if (global::Players.Players.players[playerID].jt1[num].rotX != global::Players.Players.players[playerID].jt1[num].targetAngle)
			{
				float num2 = 0f;
				float num3 = global::Players.Players.players[playerID].jt1[num].angleSpeed * stepTime[threadID];
				float num4 = global::Players.Players.players[playerID].jt1[num].targetAngle - global::Players.Players.players[playerID].jt1[num].rotX;
				if (num4 > 180f)
				{
					num4 -= 360f;
				}
				else if (num4 < -180f)
				{
					num4 += 360f;
				}
				if (num4 > 0f && num4 > num3)
				{
					num4 = num3;
				}
				else if (num4 < 0f && num4 < 0f - num3)
				{
					num4 = 0f - num3;
				}
				else if (num4 >= 0f && num4 < num3)
				{
					num2 = (num3 - num4) / global::Players.Players.players[playerID].jt1[num].angleSpeed;
				}
				else if (num4 < 0f && num4 > 0f - num3)
				{
					num2 = (num3 + num4) / global::Players.Players.players[playerID].jt1[num].angleSpeed;
				}
				if (num4 != 0f)
				{
					flag = true;
					global::Players.Players.players[playerID].jt1[num].rotX += num4;
					global::Players.Players.players[playerID].jt1[num].angleMoved += num4;
				}
				else
				{
					global::Players.Players.players[playerID].jt1[num].rotX = global::Players.Players.players[playerID].jt1[num].targetAngle;
				}
				if (num2 < surplusTime[threadID])
				{
					surplusTime[threadID] = num2;
				}
			}
			if (global::Players.Players.players[playerID].jt1[num].rotZ != global::Players.Players.players[playerID].jt1[num].targetPivot)
			{
				float num5 = 0f;
				float num3 = global::Players.Players.players[playerID].jt1[num].pivotSpeed * stepTime[threadID];
				float num4 = global::Players.Players.players[playerID].jt1[num].targetPivot - global::Players.Players.players[playerID].jt1[num].rotZ;
				if (num4 > 180f)
				{
					num4 -= 360f;
				}
				else if (num4 < -180f)
				{
					num4 += 360f;
				}
				if (num4 > 0f && num4 > num3)
				{
					num4 = num3;
				}
				else if (num4 < 0f && num4 < 0f - num3)
				{
					num4 = 0f - num3;
				}
				else if (num4 >= 0f && num4 < num3)
				{
					num5 = (num3 - num4) / global::Players.Players.players[playerID].jt1[num].pivotSpeed;
				}
				else if (num4 < 0f && num4 > 0f - num3)
				{
					num5 = (num3 + num4) / global::Players.Players.players[playerID].jt1[num].pivotSpeed;
				}
				if (num4 != 0f)
				{
					flag = true;
					global::Players.Players.players[playerID].jt1[num].rotZ += num4;
					global::Players.Players.players[playerID].jt1[num].pivotMoved += num4;
				}
				else
				{
					global::Players.Players.players[playerID].jt1[num].rotZ = global::Players.Players.players[playerID].jt1[num].targetPivot;
				}
				if (num5 < surplusTime[threadID])
				{
					surplusTime[threadID] = num5;
				}
			}
			if (global::Players.Players.players[playerID].jt1[num].rotY != global::Players.Players.players[playerID].jt1[num].targetPivot2)
			{
				float num6 = 0f;
				float num3 = global::Players.Players.players[playerID].jt1[num].pivot2Speed * stepTime[threadID];
				float num4 = global::Players.Players.players[playerID].jt1[num].targetPivot2 - global::Players.Players.players[playerID].jt1[num].rotY;
				if (num4 > 180f)
				{
					num4 -= 360f;
				}
				else if (num4 < -180f)
				{
					num4 += 360f;
				}
				if (num4 > 0f && num4 > num3)
				{
					num4 = num3;
				}
				else if (num4 < 0f && num4 < 0f - num3)
				{
					num4 = 0f - num3;
				}
				else if (num4 >= 0f && num4 < num3)
				{
					num6 = (num3 - num4) / global::Players.Players.players[playerID].jt1[num].pivot2Speed;
				}
				else if (num4 < 0f && num4 > 0f - num3)
				{
					num6 = (num3 + num4) / global::Players.Players.players[playerID].jt1[num].pivot2Speed;
				}
				if (num4 != 0f)
				{
					flag = true;
					global::Players.Players.players[playerID].jt1[num].rotY += num4;
					global::Players.Players.players[playerID].jt1[num].pivot2Moved += num4;
				}
				else
				{
					global::Players.Players.players[playerID].jt1[num].rotY = global::Players.Players.players[playerID].jt1[num].targetPivot2;
				}
				if (num6 < surplusTime[threadID])
				{
					surplusTime[threadID] = num6;
				}
			}
		}
		if (!flag)
		{
			surplusTime[threadID] = 0f;
		}
	}

	public static void Moved_Joint_Calculations(short playerID, short numPlayerJoints)
	{
		byte uBufferID = global::Rendering.Rendering.uBufferID;
		ref Matrix reference = ref global::Players.Players.players[playerID].jt1[0].mv[uBufferID];
		reference = global::Players.Players.players[playerID].jt1[0].mvAnimation * global::Players.Players.players[playerID].jt1[0].mvBase * global::Players.Players.players[playerID].ct1.mv * global::Players.Players.players[playerID].mv[uBufferID];
		int jointPackage = global::Players.Players.players[playerID].jointPackage;
		if (playerJoints[jointPackage].jt1[0].numParticles > 0)
		{
			Matrix matrix = global::Players.Players.players[playerID].jt1[0].mv[uBufferID];
			int num = global::Players.Players.players[playerID].jt1[0].pListStart;
			int num2 = playerJoints[jointPackage].jt1[0].numParticles * 3;
			int i = 0;
			int num3 = 1;
			int num4 = 2;
			for (; i < num2; i += 3)
			{
				global::Players.Players.players[playerID].charMain.v1[num].v[0] = playerJoints[jointPackage].jt1[0].particles[i] * matrix.M11 + playerJoints[jointPackage].jt1[0].particles[num3] * matrix.M21 + playerJoints[jointPackage].jt1[0].particles[num4] * matrix.M31 + matrix.M41;
				global::Players.Players.players[playerID].charMain.v1[num].v[1] = playerJoints[jointPackage].jt1[0].particles[i] * matrix.M12 + playerJoints[jointPackage].jt1[0].particles[num3] * matrix.M22 + playerJoints[jointPackage].jt1[0].particles[num4] * matrix.M32 + matrix.M42;
				global::Players.Players.players[playerID].charMain.v1[num].v[2] = playerJoints[jointPackage].jt1[0].particles[i] * matrix.M13 + playerJoints[jointPackage].jt1[0].particles[num3] * matrix.M23 + playerJoints[jointPackage].jt1[0].particles[num4] * matrix.M33 + matrix.M43;
				num++;
				num3 += 3;
				num4 += 3;
			}
		}
		for (ushort num5 = 1; num5 < numPlayerJoints; num5++)
		{
			ref Matrix reference2 = ref global::Players.Players.players[playerID].jt1[num5].mv[uBufferID];
			reference2 = global::Players.Players.players[playerID].jt1[num5].mvAnimation * global::Players.Players.players[playerID].jt1[num5].mvBase * global::Players.Players.players[playerID].jt1[global::Players.Players.players[playerID].jt1[num5].parentList[1]].mv[uBufferID];
			jointPackage = global::Players.Players.players[playerID].jointPackage;
			if (playerJoints[jointPackage].jt1[num5].numParticles > 0)
			{
				Matrix matrix = global::Players.Players.players[playerID].jt1[num5].mv[uBufferID];
				int num = global::Players.Players.players[playerID].jt1[num5].pListStart;
				int num2 = playerJoints[jointPackage].jt1[num5].numParticles * 3;
				int i = 0;
				int num3 = 1;
				int num4 = 2;
				for (; i < num2; i += 3)
				{
					global::Players.Players.players[playerID].charMain.v1[num].v[0] = playerJoints[jointPackage].jt1[num5].particles[i] * matrix.M11 + playerJoints[jointPackage].jt1[num5].particles[num3] * matrix.M21 + playerJoints[jointPackage].jt1[num5].particles[num4] * matrix.M31 + matrix.M41;
					global::Players.Players.players[playerID].charMain.v1[num].v[1] = playerJoints[jointPackage].jt1[num5].particles[i] * matrix.M12 + playerJoints[jointPackage].jt1[num5].particles[num3] * matrix.M22 + playerJoints[jointPackage].jt1[num5].particles[num4] * matrix.M32 + matrix.M42;
					global::Players.Players.players[playerID].charMain.v1[num].v[2] = playerJoints[jointPackage].jt1[num5].particles[i] * matrix.M13 + playerJoints[jointPackage].jt1[num5].particles[num3] * matrix.M23 + playerJoints[jointPackage].jt1[num5].particles[num4] * matrix.M33 + matrix.M43;
					num++;
					num3 += 3;
					num4 += 3;
				}
			}
		}
	}

	public static void Redo_Bounding_Box(short playerID, sbyte currentWeapon)
	{
		long num = 0L;
		num = playerJoints[global::Players.Players.players[playerID].jointPackage].numJointPoints + global::Weapons.Weapons.wp1[currentWeapon].box.numUsed;
		if (global::Players.Players.players[playerID].charMain.numUsed > num)
		{
			long numUsed = global::Players.Players.players[playerID].charMain.numUsed;
			for (long num2 = num; num2 < numUsed; num2++)
			{
				global::Players.Players.players[playerID].particlePrev[num2].v[0] = 0f;
				global::Players.Players.players[playerID].particlePrev[num2].v[1] = 0f;
				global::Players.Players.players[playerID].particlePrev[num2].v[2] = 0f;
				global::Players.Players.players[playerID].charMain.v1[num2].v[0] = 0f;
				global::Players.Players.players[playerID].charMain.v1[num2].v[1] = 0f;
				global::Players.Players.players[playerID].charMain.v1[num2].v[2] = 0f;
			}
			global::Players.Players.players[playerID].lastParticleCount = (short)num;
			global::Players.Players.players[playerID].charMain.numUsed = num;
		}
		if (global::Players.Players.players[playerID].charMain.numP < num)
		{
			global::Players.Players.players[playerID].charMain.numP = num;
			global::Players.Players.players[playerID].charMain.numUsed = num;
			global::Players.Players.players[playerID].lastParticleCount = (short)num;
			global::Players.Players.players[playerID].charMain.v1 = new StructsClass.vtex[num];
			global::Players.Players.players[playerID].particlePrev = new StructsClass.vtex[num];
			for (long num2 = 0L; num2 < num; num2++)
			{
				global::Players.Players.players[playerID].charMain.v1[num2] = new StructsClass.vtex();
				global::Players.Players.players[playerID].particlePrev[num2] = new StructsClass.vtex();
			}
		}
		else
		{
			global::Players.Players.players[playerID].lastParticleCount = (short)num;
			global::Players.Players.players[playerID].charMain.numUsed = num;
		}
	}

	public static void Undo_Joint_Movement(int playerID)
	{
		short numJoints = playerJoints[global::Players.Players.players[playerID].jointPackage].numJoints;
		for (short num = 0; num < numJoints; num++)
		{
			global::Players.Players.players[playerID].jt1[num].matrixReady = false;
		}
		for (short num = 0; num < numJoints; num++)
		{
			global::Players.Players.players[playerID].jt1[num].rotX -= global::Players.Players.players[playerID].jt1[num].angleMoved;
			global::Players.Players.players[playerID].jt1[num].rotZ -= global::Players.Players.players[playerID].jt1[num].pivotMoved;
			global::Players.Players.players[playerID].jt1[num].rotY -= global::Players.Players.players[playerID].jt1[num].pivot2Moved;
			Translate_Player_Joint_Vertex(playerID, num, copyToParticleList: false);
		}
		long numUsed = global::Players.Players.players[playerID].charMain.numUsed;
		for (long num2 = 0L; num2 < numUsed; num2++)
		{
			global::Players.Players.players[playerID].charMain.v1[num2].v[0] = global::Players.Players.players[playerID].particlePrev[num2].v[0];
			global::Players.Players.players[playerID].charMain.v1[num2].v[1] = global::Players.Players.players[playerID].particlePrev[num2].v[1];
			global::Players.Players.players[playerID].charMain.v1[num2].v[2] = global::Players.Players.players[playerID].particlePrev[num2].v[2];
		}
	}

	public static void Do_Joint_Basic_Calculations(float frameTime)
	{
		float num = 0f;
		float num2 = 0f;
		float num3 = 0f;
		frameTime /= global::Physics.Physics.timeMod;
		long num4 = (long)(50f * (frameTime / 0.033f));
		if (num4 < numJointsBasic)
		{
			num4 = numJointsBasic * 3;
		}
		for (long num5 = 0L; num5 < numJointsBasic; num5++)
		{
			switch (jtb1[num5].type)
			{
			case 1:
				if (jtb1[num5].objectType == 0)
				{
					long num6 = jtb1[num5].objID;
					global::GameObjects.GameObjects.objCol[num6].moved = false;
					global::GameObjects.GameObjects.objCol[num6].movedX = 0f;
					global::GameObjects.GameObjects.objCol[num6].movedY = 0f;
					global::GameObjects.GameObjects.objCol[num6].movedZ = 0f;
					global::GameObjects.GameObjects.objCol[num6].frameTime = frameTime;
				}
				break;
			case 2:
			case 3:
			{
				long num6 = jtb1[num5].objID;
				global::GameObjects.GameObjects.objCol[num6].moved = false;
				global::GameObjects.GameObjects.objCol[num6].movedX = 0f;
				global::GameObjects.GameObjects.objCol[num6].movedY = 0f;
				global::GameObjects.GameObjects.objCol[num6].movedZ = 0f;
				global::GameObjects.GameObjects.objCol[num6].frameTime = frameTime;
				break;
			}
			}
		}
		for (long num5 = 0L; num5 < numJointsBasic; num5++)
		{
			switch (jtb1[num5].type)
			{
			case 1:
			{
				float num7 = frameTime;
				float num9;
				float num8 = (num9 = 0f);
				float num12 = num7;
				num = (num2 = (num3 = 0f));
				bool flag2 = false;
				while (num7 > 0f && num4-- > 0)
				{
					bool flag = false;
					bool flag3 = false;
					if (jtb1[num5].angle != jtb1[num5].targetAngle)
					{
						float num13 = 0f;
						float num10 = jtb1[num5].angleSpeed * num7;
						float num11 = jtb1[num5].targetAngle - jtb1[num5].angle;
						if (num11 > 0f && num11 > num10)
						{
							num11 = num10;
						}
						else if (num11 < 0f && num11 < 0f - num10)
						{
							num11 = 0f - num10;
						}
						else if (num11 >= 0f && num11 < num10)
						{
							num13 = (num10 - num11) / jtb1[num5].angleSpeed;
						}
						else if (num11 < 0f && num11 > 0f - num10)
						{
							num13 = (num10 + num11) / jtb1[num5].angleSpeed;
						}
						flag = true;
						jtb1[num5].angle += num11;
						jtb1[num5].angleMoved = num11;
						if (num13 < num12)
						{
							num12 = num13;
						}
					}
					if (jtb1[num5].pivot != jtb1[num5].targetPivot)
					{
						num8 = 0f;
						float num10 = jtb1[num5].pivotSpeed * num7;
						float num11 = jtb1[num5].targetPivot - jtb1[num5].pivot;
						if (num11 > 0f && num11 > num10)
						{
							num11 = num10;
						}
						else if (num11 < 0f && num11 < 0f - num10)
						{
							num11 = 0f - num10;
						}
						else if (num11 >= 0f && num11 < num10)
						{
							num8 = (num10 - num11) / jtb1[num5].angleSpeed;
						}
						else if (num11 < 0f && num11 > 0f - num10)
						{
							num8 = (num10 + num11) / jtb1[num5].angleSpeed;
						}
						flag = true;
						jtb1[num5].pivot += num11;
						jtb1[num5].pivotMoved = num11;
						if (num8 < num12)
						{
							num12 = num8;
						}
					}
					if (jtb1[num5].pivot2 != jtb1[num5].targetPivot2)
					{
						num9 = 0f;
						float num10 = jtb1[num5].pivot2Speed * num7;
						float num11 = jtb1[num5].targetPivot2 - jtb1[num5].pivot2;
						if (num11 > 0f && num11 > num10)
						{
							num11 = num10;
						}
						else if (num11 < 0f && num11 < 0f - num10)
						{
							num11 = 0f - num10;
						}
						else if (num11 >= 0f && num11 < num10)
						{
							num9 = (num10 - num11) / jtb1[num5].angleSpeed;
						}
						else if (num11 < 0f && num11 > 0f - num10)
						{
							num9 = (num10 + num11) / jtb1[num5].angleSpeed;
						}
						flag = true;
						jtb1[num5].pivot2 += num11;
						jtb1[num5].pivot2Moved = num11;
						if (num9 < num12)
						{
							num12 = num9;
						}
					}
					if (jtb1[num5].x != jtb1[num5].targetX)
					{
						num9 = 0f;
						float num10 = jtb1[num5].xSpeed * num7;
						flag = true;
						_ = jtb1[num5].xSpeed;
						float num11 = jtb1[num5].targetX - jtb1[num5].x;
						if (num11 > 0f && num11 > num10)
						{
							num11 = num10;
						}
						else if (num11 < 0f && num11 < 0f - num10)
						{
							num11 = 0f - num10;
						}
						else if (num11 >= 0f && num11 < num10)
						{
							num9 = (num10 - num11) / jtb1[num5].angleSpeed;
						}
						else if (num11 < 0f && num11 > 0f - num10)
						{
							num9 = (num10 - num11) / jtb1[num5].angleSpeed;
						}
						else
						{
							Math.Abs(num11);
						}
						num += num11;
						if (num11 < 0f)
						{
							_ = jtb1[num5].xSpeed;
						}
						jtb1[num5].x += num11;
						if (num9 < num12)
						{
							num12 = num9;
						}
					}
					if (jtb1[num5].y != jtb1[num5].targetY)
					{
						num9 = 0f;
						float num10 = jtb1[num5].ySpeed * num7;
						flag = true;
						_ = jtb1[num5].ySpeed;
						float num11 = jtb1[num5].targetY - jtb1[num5].y;
						if (num11 > 0f && num11 > num10)
						{
							num11 = num10;
						}
						else if (num11 < 0f && num11 < 0f - num10)
						{
							num11 = 0f - num10;
						}
						else if (num11 >= 0f && num11 < num10)
						{
							num9 = (num10 - num11) / jtb1[num5].angleSpeed;
						}
						else if (num11 < 0f && num11 > 0f - num10)
						{
							num9 = (num10 - num11) / jtb1[num5].angleSpeed;
						}
						else
						{
							Math.Abs(num11);
						}
						num2 += num11;
						if (num11 < 0f)
						{
							_ = jtb1[num5].ySpeed;
						}
						jtb1[num5].y += num11;
						if (num9 < num12)
						{
							num12 = num9;
						}
					}
					if (jtb1[num5].z != jtb1[num5].targetZ)
					{
						num9 = 0f;
						float num10 = jtb1[num5].zSpeed * num7;
						flag = true;
						_ = jtb1[num5].zSpeed;
						float num11 = jtb1[num5].targetZ - jtb1[num5].z;
						if (num11 > 0f && num11 > num10)
						{
							num11 = num10;
						}
						else if (num11 < 0f && num11 < 0f - num10)
						{
							num11 = 0f - num10;
						}
						else if (num11 >= 0f && num11 < num10)
						{
							num9 = (num10 - num11) / jtb1[num5].angleSpeed;
						}
						else if (num11 < 0f && num11 > num10)
						{
							num9 = (num10 - num11) / jtb1[num5].angleSpeed;
						}
						else
						{
							Math.Abs(num11);
						}
						num3 += num11;
						if (num11 < 0f)
						{
							_ = jtb1[num5].zSpeed;
						}
						jtb1[num5].z += num11;
						if (num9 < num12)
						{
							num12 = num9;
						}
					}
					if (jtb1[num5].time > 0f)
					{
						num9 = 0f;
						float num10 = num7;
						flag3 = true;
						float num11 = jtb1[num5].time;
						if (num11 > 0f && num11 > num10)
						{
							num11 = num10;
						}
						else if (num11 >= 0f && num11 < num10)
						{
							num9 = num10 - num11;
						}
						jtb1[num5].time -= num11;
						if (num9 < num12)
						{
							num12 = num9;
						}
					}
					if (flag)
					{
						flag2 = true;
					}
					if (!flag && !flag3)
					{
						num12 = 0f;
					}
					num7 = num12;
					if (num12 > 0f || flag)
					{
						mainC.programsMain.Process_Programs_Basic();
					}
				}
				long num6 = jtb1[num5].objID;
				switch (jtb1[num5].objectType)
				{
				case 0:
					if (flag2)
					{
						global::GameObjects.GameObjects.objCol[num6].moved = true;
					}
					global::GameObjects.GameObjects.objCol[num6].movedX += num;
					global::GameObjects.GameObjects.objCol[num6].movedY += num2;
					global::GameObjects.GameObjects.objCol[num6].movedZ += num3;
					global::GameObjects.GameObjects.objCol[num6].x = jtb1[num5].x;
					global::GameObjects.GameObjects.objCol[num6].y = jtb1[num5].y;
					global::GameObjects.GameObjects.objCol[num6].z = jtb1[num5].z;
					global::GameObjects.GameObjects.objCol[num6].velX = num / frameTime;
					global::GameObjects.GameObjects.objCol[num6].velY = num2 / frameTime;
					global::GameObjects.GameObjects.objCol[num6].velZ = num3 / frameTime;
					break;
				case 1:
					mainC.targetMain.Update_Target_Location((ushort)jtb1[num5].objID, jtb1[num5].x, jtb1[num5].y, jtb1[num5].z, jtb1[num5].angle, jtb1[num5].pivot, jtb1[num5].pivot2);
					break;
				case 2:
				{
					ref Matrix reference = ref jtb1[num5].mv[global::Rendering.Rendering.uBufferID];
					reference = Matrix.CreateRotationY(jtb1[num5].pivot * ((float)Math.PI / 180f)) * Matrix.CreateRotationX(jtb1[num5].angle * ((float)Math.PI / 180f)) * Matrix.CreateRotationZ(jtb1[num5].pivot2 * ((float)Math.PI / 180f)) * Matrix.CreateTranslation(jtb1[num5].x, jtb1[num5].y, jtb1[num5].z);
					break;
				}
				}
				break;
			}
			case 2:
			{
				if (jtb1[num5].targetX >= 0f)
				{
					if (jtb1[num5].targetY > 0f)
					{
						mainC.programsMain.Start_Program_Basic((short)jtb1[num5].targetX, reverse: true, (short)jtb1[num5].targetZ);
					}
					else
					{
						mainC.programsMain.Start_Program_Basic((short)jtb1[num5].targetX, reverse: false, (short)jtb1[num5].targetZ);
					}
					jtb1[num5].targetX = -1f;
					jtb1[num5].targetY = 0f;
					jtb1[num5].targetZ = 0f;
				}
				float num7 = frameTime;
				float num9 = 0f;
				float num12 = num7;
				while (num7 > 0f && num4-- > 0)
				{
					if (jtb1[num5].time > 0f)
					{
						num9 = 0f;
						float num10 = num7;
						float num11 = jtb1[num5].time;
						if (num11 > 0f && num11 > num10)
						{
							num11 = num10;
						}
						else if (num11 >= 0f && num11 < num10)
						{
							num9 = num10 - num11;
						}
						jtb1[num5].time -= num11;
						if (num9 < num12)
						{
							num12 = num9;
						}
					}
					else
					{
						num12 = 0f;
					}
					num7 = num12;
					if (num12 == 0f)
					{
						mainC.programsMain.Process_Programs_Basic();
					}
				}
				break;
			}
			case 3:
			{
				if (jtb1[num5].targetX >= 0f)
				{
					long num6 = jtb1[(ushort)jtb1[num5].targetX].objID;
					global::GameObjects.GameObjects.objCol[num6].moved = true;
					global::GameObjects.GameObjects.objCol[num6].movedX += jtb1[num5].xSpeed - jtb1[(int)jtb1[num5].targetX].x;
					global::GameObjects.GameObjects.objCol[num6].movedY += jtb1[num5].ySpeed - jtb1[(int)jtb1[num5].targetX].y;
					global::GameObjects.GameObjects.objCol[num6].movedZ += jtb1[num5].zSpeed - jtb1[(int)jtb1[num5].targetX].z;
					global::GameObjects.GameObjects.objCol[num6].x = jtb1[num5].xSpeed;
					global::GameObjects.GameObjects.objCol[num6].y = jtb1[num5].ySpeed;
					global::GameObjects.GameObjects.objCol[num6].z = jtb1[num5].zSpeed;
					global::GameObjects.GameObjects.objCol[num6].velX = 0f;
					global::GameObjects.GameObjects.objCol[num6].velY = 0f;
					global::GameObjects.GameObjects.objCol[num6].velZ = 0f;
					num6 = (int)jtb1[num5].targetX;
					jtb1[num6].targetX = (jtb1[num6].x = jtb1[num5].xSpeed);
					jtb1[num6].targetY = (jtb1[num6].y = jtb1[num5].ySpeed);
					jtb1[num6].targetZ = (jtb1[num6].z = jtb1[num5].zSpeed);
					jtb1[num5].targetX = -1f;
				}
				float num7 = frameTime;
				float num9 = 0f;
				float num12 = num7;
				while (num7 > 0f && num4-- > 0)
				{
					if (jtb1[num5].time > 0f)
					{
						num9 = 0f;
						float num10 = num7;
						float num11 = jtb1[num5].time;
						if (num11 > 0f && num11 > num10)
						{
							num11 = num10;
						}
						else if (num11 >= 0f && num11 < num10)
						{
							num9 = num10 - num11;
						}
						jtb1[num5].time -= num11;
						if (num9 < num12)
						{
							num12 = num9;
						}
					}
					else
					{
						num12 = 0f;
					}
					num7 = num12;
					if (num12 == 0f)
					{
						mainC.programsMain.Process_Programs_Basic();
					}
				}
				break;
			}
			case 4:
			{
				float num7 = frameTime;
				float num9;
				float num8 = (num9 = 0f);
				bool flag = false;
				if (jtb1[num5].x != jtb1[num5].targetX)
				{
					num9 = 0f;
					float num10 = jtb1[num5].xSpeed * num7;
					_ = jtb1[num5].xSpeed;
					float num11 = jtb1[num5].targetX - jtb1[num5].x;
					if (num11 > 0f && num11 > num10)
					{
						num11 = num10;
					}
					else if (num11 < 0f && num11 < 0f - num10)
					{
						num11 = 0f - num10;
					}
					else if (num11 >= 0f && num11 < num10)
					{
						num9 = (num10 - num11) / jtb1[num5].angleSpeed;
					}
					else if (num11 < 0f && num11 > 0f - num10)
					{
						num9 = (num10 - num11) / jtb1[num5].angleSpeed;
					}
					jtb1[num5].x += num11;
					flag = true;
				}
				if (flag)
				{
					mainC.programsMain.Process_Programs_Basic();
				}
				break;
			}
			}
		}
	}

	public static void Reset_Joints_AfterStuckCollision(short playerID)
	{
		short numJoints = playerJoints[global::Players.Players.players[playerID].jointPackage].numJoints;
		for (short num = 0; num < numJoints; num++)
		{
			global::Players.Players.players[playerID].jt1[num].matrixReady = false;
			global::Players.Players.players[playerID].jt1[num].angleMoved = 0f;
			global::Players.Players.players[playerID].jt1[num].pivotMoved = 0f;
			global::Players.Players.players[playerID].jt1[num].pivot2Moved = 0f;
		}
	}

	public static void Calculate_Joint_Vertex(int playerID, long jID, byte threadID)
	{
		float num = 0f;
		global::Players.Players.players[playerID].jt1[jID].redoNorms = true;
		float pinOffset = global::Players.Players.players[playerID].jt1[jID].pinOffset;
		int rings = global::Players.Players.players[playerID].jt1[jID].rings;
		int ringPtCnt = global::Players.Players.players[playerID].jt1[jID].ringPtCnt;
		float pinAngleD = global::Players.Players.players[playerID].jt1[jID].pinAngleD;
		float ringYD = global::Players.Players.players[playerID].jt1[jID].ringYD;
		if (global::Players.Players.players[playerID].jt1[jID].adjustStartRing == 1)
		{
			global::Players.Players.players[playerID].jt1[jID].angles[0] = (0f - global::Players.Players.players[playerID].jt1[jID].rotX) / 2f;
		}
		else
		{
			global::Players.Players.players[playerID].jt1[jID].angles[0] = 0f;
		}
		if (global::Players.Players.players[playerID].jt1[jID].adjustEndRing == 1)
		{
			global::Players.Players.players[playerID].jt1[jID].angles[rings - 1] = global::Players.Players.players[playerID].jt1[global::Players.Players.players[playerID].jt1[jID].idList[0]].rotX / 2f;
		}
		else
		{
			global::Players.Players.players[playerID].jt1[jID].angles[rings - 1] = 0f;
		}
		float num2 = global::Players.Players.players[playerID].jt1[jID].angles[rings - 1] - global::Players.Players.players[playerID].jt1[jID].angles[0];
		if (rings > 2)
		{
			num2 /= (float)(rings - 1);
		}
		for (int i = 1; i < rings - 1; i++)
		{
			global::Players.Players.players[playerID].jt1[jID].angles[i] = global::Players.Players.players[playerID].jt1[jID].angles[i - 1] + num2;
		}
		for (int i = 0; i < rings; i++)
		{
			global::Players.Players.players[playerID].jt1[jID].angles[i] = global::Players.Players.players[playerID].jt1[jID].angles[i] * ((float)Math.PI / 180f);
		}
		if (ringPtCnt > sizeCJVFloats)
		{
			sinA = new float[4, ringPtCnt];
			cosA = new float[4, ringPtCnt];
			sizeCJVFloats = ringPtCnt;
		}
		for (int i = 0; i < ringPtCnt; i++)
		{
			sinA[threadID, i] = (float)Math.Sin(pinAngleD * (float)i);
			cosA[threadID, i] = (float)Math.Cos(pinAngleD * (float)i);
		}
		int num3 = 0;
		cjVv1[threadID].v[0] = 0f;
		for (int i = 0; i < rings; i++)
		{
			float num4 = (float)Math.Sin(global::Players.Players.players[playerID].jt1[jID].angles[i]);
			float num5 = (float)Math.Cos(global::Players.Players.players[playerID].jt1[jID].angles[i]);
			float num6 = ringYD * (float)i;
			cjVv1[threadID].v[1] = (0f - num4) * pinOffset;
			cjVv1[threadID].v[2] = num5 * pinOffset;
			for (int j = 0; j < global::Players.Players.players[playerID].jt1[jID].ringPtCnt; j++)
			{
				float num7 = global::Players.Players.players[playerID].jt1[jID].ringPins[num3];
				global::Players.Players.players[playerID].jt1[jID].ringPts[num3].v[0] = sinA[threadID, j] * num7;
				global::Players.Players.players[playerID].jt1[jID].ringPts[num3].v[1] = num6 - num4 * num7 * cosA[threadID, j];
				global::Players.Players.players[playerID].jt1[jID].ringPts[num3].v[2] = cosA[threadID, j] * num7 * num5;
				global::Players.Players.players[playerID].jt1[jID].ringPts[num3].v[1] -= num4 * pinOffset;
				global::Players.Players.players[playerID].jt1[jID].ringPts[num3].v[2] += num5 * pinOffset;
				num7 = global::Players.Players.players[playerID].jt1[jID].ringPts[num3].v[0] * global::Players.Players.players[playerID].jt1[jID].ringPts[num3].v[0] + global::Players.Players.players[playerID].jt1[jID].ringPts[num3].v[2] * global::Players.Players.players[playerID].jt1[jID].ringPts[num3].v[2];
				if (num7 > num)
				{
					num = num7;
				}
				num3++;
			}
		}
		global::Players.Players.players[playerID].jt1[jID].maxPinH = num;
	}

	public static void Translate_Player_Joint_Vertex(int playerID, int jID, bool copyToParticleList)
	{
		Matrix matrix = default(Matrix);
		Matrix result = default(Matrix);
		byte uBufferID = global::Rendering.Rendering.uBufferID;
		_ = playerJoints[global::Players.Players.players[playerID].jointPackage].numJoints;
		if (!global::Players.Players.players[playerID].jt1[jID].matrixReady)
		{
			matrix = global::Players.Players.players[playerID].ct1.mv * global::Players.Players.players[playerID].mv[uBufferID];
			for (int num = global::Players.Players.players[playerID].jt1[jID].parentCount - 1; num > -1; num--)
			{
				int num2 = global::Players.Players.players[playerID].jt1[jID].parentList[num];
				if (!global::Players.Players.players[playerID].jt1[num2].matrixReady)
				{
					Matrix.CreateTranslation(global::Players.Players.players[playerID].jt1[num2].x, global::Players.Players.players[playerID].jt1[num2].y, global::Players.Players.players[playerID].jt1[num2].z, out result);
					matrix = result * matrix;
					Matrix.CreateRotationZ(global::Players.Players.players[playerID].jt1[num2].angleZ * ((float)Math.PI / 180f), out result);
					matrix = result * matrix;
					Matrix.CreateRotationX(global::Players.Players.players[playerID].jt1[num2].angleX * ((float)Math.PI / 180f), out result);
					matrix = result * matrix;
					Matrix.CreateRotationY(global::Players.Players.players[playerID].jt1[num2].angleY * ((float)Math.PI / 180f), out result);
					matrix = result * matrix;
					Matrix.CreateRotationZ(global::Players.Players.players[playerID].jt1[num2].rotZ * ((float)Math.PI / 180f), out result);
					matrix = result * matrix;
					Matrix.CreateRotationX(global::Players.Players.players[playerID].jt1[num2].rotX * ((float)Math.PI / 180f), out result);
					matrix = result * matrix;
					Matrix.CreateRotationY(global::Players.Players.players[playerID].jt1[num2].rotY * ((float)Math.PI / 180f), out result);
					matrix = result * matrix;
					global::Players.Players.players[playerID].jt1[num2].mv[uBufferID].M11 = matrix.M11;
					global::Players.Players.players[playerID].jt1[num2].mv[uBufferID].M12 = matrix.M12;
					global::Players.Players.players[playerID].jt1[num2].mv[uBufferID].M13 = matrix.M13;
					global::Players.Players.players[playerID].jt1[num2].mv[uBufferID].M14 = matrix.M14;
					global::Players.Players.players[playerID].jt1[num2].mv[uBufferID].M21 = matrix.M21;
					global::Players.Players.players[playerID].jt1[num2].mv[uBufferID].M22 = matrix.M22;
					global::Players.Players.players[playerID].jt1[num2].mv[uBufferID].M23 = matrix.M23;
					global::Players.Players.players[playerID].jt1[num2].mv[uBufferID].M24 = matrix.M24;
					global::Players.Players.players[playerID].jt1[num2].mv[uBufferID].M31 = matrix.M31;
					global::Players.Players.players[playerID].jt1[num2].mv[uBufferID].M32 = matrix.M32;
					global::Players.Players.players[playerID].jt1[num2].mv[uBufferID].M33 = matrix.M33;
					global::Players.Players.players[playerID].jt1[num2].mv[uBufferID].M34 = matrix.M34;
					global::Players.Players.players[playerID].jt1[num2].mv[uBufferID].M41 = matrix.M41;
					global::Players.Players.players[playerID].jt1[num2].mv[uBufferID].M42 = matrix.M42;
					global::Players.Players.players[playerID].jt1[num2].mv[uBufferID].M43 = matrix.M43;
					global::Players.Players.players[playerID].jt1[num2].mv[uBufferID].M44 = matrix.M44;
					global::Players.Players.players[playerID].jt1[num2].matrixReady = true;
				}
				else if (num > 0)
				{
					matrix = global::Players.Players.players[playerID].jt1[num2].mv[uBufferID];
				}
			}
			global::Players.Players.players[playerID].jt1[jID].mv[uBufferID].M11 = matrix.M11;
			global::Players.Players.players[playerID].jt1[jID].mv[uBufferID].M12 = matrix.M12;
			global::Players.Players.players[playerID].jt1[jID].mv[uBufferID].M13 = matrix.M13;
			global::Players.Players.players[playerID].jt1[jID].mv[uBufferID].M14 = matrix.M14;
			global::Players.Players.players[playerID].jt1[jID].mv[uBufferID].M21 = matrix.M21;
			global::Players.Players.players[playerID].jt1[jID].mv[uBufferID].M22 = matrix.M22;
			global::Players.Players.players[playerID].jt1[jID].mv[uBufferID].M23 = matrix.M23;
			global::Players.Players.players[playerID].jt1[jID].mv[uBufferID].M24 = matrix.M24;
			global::Players.Players.players[playerID].jt1[jID].mv[uBufferID].M31 = matrix.M31;
			global::Players.Players.players[playerID].jt1[jID].mv[uBufferID].M32 = matrix.M32;
			global::Players.Players.players[playerID].jt1[jID].mv[uBufferID].M33 = matrix.M33;
			global::Players.Players.players[playerID].jt1[jID].mv[uBufferID].M34 = matrix.M34;
			global::Players.Players.players[playerID].jt1[jID].mv[uBufferID].M41 = matrix.M41;
			global::Players.Players.players[playerID].jt1[jID].mv[uBufferID].M42 = matrix.M42;
			global::Players.Players.players[playerID].jt1[jID].mv[uBufferID].M43 = matrix.M43;
			global::Players.Players.players[playerID].jt1[jID].mv[uBufferID].M44 = matrix.M44;
			global::Players.Players.players[playerID].jt1[jID].matrixReady = true;
		}
		if (copyToParticleList)
		{
			Matrix matrix2 = global::Players.Players.players[playerID].jt1[jID].mv[uBufferID];
			int num = global::Players.Players.players[playerID].jt1[jID].pListStart;
			int jointPackage = global::Players.Players.players[playerID].jointPackage;
			int num3 = playerJoints[jointPackage].jt1[jID].numParticles * 3;
			int i = 0;
			int num4 = 1;
			int num5 = 2;
			for (; i < num3; i += 3)
			{
				global::Players.Players.players[playerID].charMain.v1[num].v[0] = playerJoints[jointPackage].jt1[jID].particles[i] * matrix2.M11 + playerJoints[jointPackage].jt1[jID].particles[num4] * matrix2.M21 + playerJoints[jointPackage].jt1[jID].particles[num5] * matrix2.M31 + matrix2.M41;
				global::Players.Players.players[playerID].charMain.v1[num].v[1] = playerJoints[jointPackage].jt1[jID].particles[i] * matrix2.M12 + playerJoints[jointPackage].jt1[jID].particles[num4] * matrix2.M22 + playerJoints[jointPackage].jt1[jID].particles[num5] * matrix2.M32 + matrix2.M42;
				global::Players.Players.players[playerID].charMain.v1[num].v[2] = playerJoints[jointPackage].jt1[jID].particles[i] * matrix2.M13 + playerJoints[jointPackage].jt1[jID].particles[num4] * matrix2.M23 + playerJoints[jointPackage].jt1[jID].particles[num5] * matrix2.M33 + matrix2.M43;
				num++;
				num4 += 3;
				num5 += 3;
			}
		}
	}

	public static void Translate_Player_Joint_Vertex_Non_Particle(int playerID)
	{
		Matrix matrix = default(Matrix);
		Matrix result = default(Matrix);
		byte uBufferID = global::Rendering.Rendering.uBufferID;
		int numJoints = playerJoints[global::Players.Players.players[playerID].jointPackage].numJoints;
		for (short num = 0; num < numJoints; num++)
		{
			if (!global::Players.Players.players[playerID].jt1[num].matrixReady)
			{
				matrix = global::Players.Players.players[playerID].ct1.mv * global::Players.Players.players[playerID].mv[uBufferID];
				for (int num2 = global::Players.Players.players[playerID].jt1[num].parentCount - 1; num2 > -1; num2--)
				{
					int num3 = global::Players.Players.players[playerID].jt1[num].parentList[num2];
					if (!global::Players.Players.players[playerID].jt1[num3].matrixReady)
					{
						Matrix.CreateTranslation(global::Players.Players.players[playerID].jt1[num3].x, global::Players.Players.players[playerID].jt1[num3].y, global::Players.Players.players[playerID].jt1[num3].z, out result);
						matrix = result * matrix;
						Matrix.CreateRotationZ(global::Players.Players.players[playerID].jt1[num3].angleZ * ((float)Math.PI / 180f), out result);
						matrix = result * matrix;
						Matrix.CreateRotationX(global::Players.Players.players[playerID].jt1[num3].angleX * ((float)Math.PI / 180f), out result);
						matrix = result * matrix;
						Matrix.CreateRotationY(global::Players.Players.players[playerID].jt1[num3].angleY * ((float)Math.PI / 180f), out result);
						matrix = result * matrix;
						Matrix.CreateRotationZ(global::Players.Players.players[playerID].jt1[num3].rotZ * ((float)Math.PI / 180f), out result);
						matrix = result * matrix;
						Matrix.CreateRotationX(global::Players.Players.players[playerID].jt1[num3].rotX * ((float)Math.PI / 180f), out result);
						matrix = result * matrix;
						Matrix.CreateRotationY(global::Players.Players.players[playerID].jt1[num3].rotY * ((float)Math.PI / 180f), out result);
						matrix = result * matrix;
						global::Players.Players.players[playerID].jt1[num3].mv[uBufferID].M11 = matrix.M11;
						global::Players.Players.players[playerID].jt1[num3].mv[uBufferID].M12 = matrix.M12;
						global::Players.Players.players[playerID].jt1[num3].mv[uBufferID].M13 = matrix.M13;
						global::Players.Players.players[playerID].jt1[num3].mv[uBufferID].M14 = matrix.M14;
						global::Players.Players.players[playerID].jt1[num3].mv[uBufferID].M21 = matrix.M21;
						global::Players.Players.players[playerID].jt1[num3].mv[uBufferID].M22 = matrix.M22;
						global::Players.Players.players[playerID].jt1[num3].mv[uBufferID].M23 = matrix.M23;
						global::Players.Players.players[playerID].jt1[num3].mv[uBufferID].M24 = matrix.M24;
						global::Players.Players.players[playerID].jt1[num3].mv[uBufferID].M31 = matrix.M31;
						global::Players.Players.players[playerID].jt1[num3].mv[uBufferID].M32 = matrix.M32;
						global::Players.Players.players[playerID].jt1[num3].mv[uBufferID].M33 = matrix.M33;
						global::Players.Players.players[playerID].jt1[num3].mv[uBufferID].M34 = matrix.M34;
						global::Players.Players.players[playerID].jt1[num3].mv[uBufferID].M41 = matrix.M41;
						global::Players.Players.players[playerID].jt1[num3].mv[uBufferID].M42 = matrix.M42;
						global::Players.Players.players[playerID].jt1[num3].mv[uBufferID].M43 = matrix.M43;
						global::Players.Players.players[playerID].jt1[num3].mv[uBufferID].M44 = matrix.M44;
						global::Players.Players.players[playerID].jt1[num3].matrixReady = true;
					}
					else if (num2 > 0)
					{
						matrix = global::Players.Players.players[playerID].jt1[num3].mv[uBufferID];
					}
				}
				global::Players.Players.players[playerID].jt1[num].mv[uBufferID].M11 = matrix.M11;
				global::Players.Players.players[playerID].jt1[num].mv[uBufferID].M12 = matrix.M12;
				global::Players.Players.players[playerID].jt1[num].mv[uBufferID].M13 = matrix.M13;
				global::Players.Players.players[playerID].jt1[num].mv[uBufferID].M14 = matrix.M14;
				global::Players.Players.players[playerID].jt1[num].mv[uBufferID].M21 = matrix.M21;
				global::Players.Players.players[playerID].jt1[num].mv[uBufferID].M22 = matrix.M22;
				global::Players.Players.players[playerID].jt1[num].mv[uBufferID].M23 = matrix.M23;
				global::Players.Players.players[playerID].jt1[num].mv[uBufferID].M24 = matrix.M24;
				global::Players.Players.players[playerID].jt1[num].mv[uBufferID].M31 = matrix.M31;
				global::Players.Players.players[playerID].jt1[num].mv[uBufferID].M32 = matrix.M32;
				global::Players.Players.players[playerID].jt1[num].mv[uBufferID].M33 = matrix.M33;
				global::Players.Players.players[playerID].jt1[num].mv[uBufferID].M34 = matrix.M34;
				global::Players.Players.players[playerID].jt1[num].mv[uBufferID].M41 = matrix.M41;
				global::Players.Players.players[playerID].jt1[num].mv[uBufferID].M42 = matrix.M42;
				global::Players.Players.players[playerID].jt1[num].mv[uBufferID].M43 = matrix.M43;
				global::Players.Players.players[playerID].jt1[num].mv[uBufferID].M44 = matrix.M44;
				global::Players.Players.players[playerID].jt1[num].matrixReady = true;
			}
		}
	}

	public static void Translate_Player_Joint_Collection(short jColID)
	{
		Matrix identity = Matrix.Identity;
		Matrix result = default(Matrix);
		int numJoints = playerJoints[jColID].numJoints;
		for (int i = 0; i < numJoints; i++)
		{
			playerJoints[jColID].jt1[i].matrixReady = false;
		}
		for (short num = 0; num < numJoints; num++)
		{
			if (!playerJoints[jColID].jt1[num].matrixReady)
			{
				identity = playerJoints[jColID].ct1.mv;
				for (int i = playerJoints[jColID].jt1[num].parentCount - 1; i > -1; i--)
				{
					int num2 = playerJoints[jColID].jt1[num].parentList[i];
					if (!playerJoints[jColID].jt1[num2].matrixReady)
					{
						Matrix.CreateTranslation(playerJoints[jColID].jt1[num2].x, playerJoints[jColID].jt1[num2].y, playerJoints[jColID].jt1[num2].z, out result);
						identity = result * identity;
						Matrix.CreateRotationZ(playerJoints[jColID].jt1[num2].angleZ * ((float)Math.PI / 180f), out result);
						identity = result * identity;
						Matrix.CreateRotationX(playerJoints[jColID].jt1[num2].angleX * ((float)Math.PI / 180f), out result);
						identity = result * identity;
						Matrix.CreateRotationY(playerJoints[jColID].jt1[num2].angleY * ((float)Math.PI / 180f), out result);
						identity = result * identity;
						playerJoints[jColID].jt1[num2].mv[0].M11 = identity.M11;
						playerJoints[jColID].jt1[num2].mv[0].M12 = identity.M12;
						playerJoints[jColID].jt1[num2].mv[0].M13 = identity.M13;
						playerJoints[jColID].jt1[num2].mv[0].M14 = identity.M14;
						playerJoints[jColID].jt1[num2].mv[0].M21 = identity.M21;
						playerJoints[jColID].jt1[num2].mv[0].M22 = identity.M22;
						playerJoints[jColID].jt1[num2].mv[0].M23 = identity.M23;
						playerJoints[jColID].jt1[num2].mv[0].M24 = identity.M24;
						playerJoints[jColID].jt1[num2].mv[0].M31 = identity.M31;
						playerJoints[jColID].jt1[num2].mv[0].M32 = identity.M32;
						playerJoints[jColID].jt1[num2].mv[0].M33 = identity.M33;
						playerJoints[jColID].jt1[num2].mv[0].M34 = identity.M34;
						playerJoints[jColID].jt1[num2].mv[0].M41 = identity.M41;
						playerJoints[jColID].jt1[num2].mv[0].M42 = identity.M42;
						playerJoints[jColID].jt1[num2].mv[0].M43 = identity.M43;
						playerJoints[jColID].jt1[num2].mv[0].M44 = identity.M44;
						playerJoints[jColID].jt1[num2].matrixReady = true;
					}
					else
					{
						identity = playerJoints[jColID].jt1[num2].mv[0];
					}
				}
			}
		}
	}

	public static void Translate_Player_Joints_To_Identity(int playerID)
	{
		int numJoints = playerJoints[global::Players.Players.players[playerID].jointPackage].numJoints;
		for (int i = 0; i < numJoints; i++)
		{
			global::Players.Players.players[playerID].jt1[i].mv[0].M11 = 1f;
			global::Players.Players.players[playerID].jt1[i].mv[0].M12 = 0f;
			global::Players.Players.players[playerID].jt1[i].mv[0].M13 = 0f;
			global::Players.Players.players[playerID].jt1[i].mv[0].M21 = 0f;
			global::Players.Players.players[playerID].jt1[i].mv[0].M22 = 1f;
			global::Players.Players.players[playerID].jt1[i].mv[0].M23 = 0f;
			global::Players.Players.players[playerID].jt1[i].mv[0].M31 = 0f;
			global::Players.Players.players[playerID].jt1[i].mv[0].M32 = 0f;
			global::Players.Players.players[playerID].jt1[i].mv[0].M33 = 1f;
			ref Matrix reference = ref global::Players.Players.players[playerID].jt1[i].mv[1];
			reference = global::Players.Players.players[playerID].jt1[i].mv[0];
			global::Players.Players.players[playerID].jt1[i].matrixReady = true;
		}
	}

	public static void Sync_Player_Matrices(int playerID, byte src, byte dst)
	{
		ref Matrix reference = ref global::Players.Players.players[playerID].mv[dst];
		reference = global::Players.Players.players[playerID].mv[src];
		int numJoints = playerJoints[global::Players.Players.players[playerID].jointPackage].numJoints;
		for (int i = 0; i < numJoints; i++)
		{
			ref Matrix reference2 = ref global::Players.Players.players[playerID].jt1[i].mv[dst];
			reference2 = global::Players.Players.players[playerID].jt1[i].mv[src];
		}
	}

	public static void Translate_Joints(ref StructsClass.joint[] jt1, ref StructsClass.JointCollection jc)
	{
		Matrix matrix = default(Matrix);
		Matrix result = default(Matrix);
		byte uBufferID = global::Rendering.Rendering.uBufferID;
		int numJoints = jc.numJoints;
		for (short num = 0; num < numJoints; num++)
		{
			jt1[num].matrixReady = false;
		}
		for (short num = 0; num < numJoints; num++)
		{
			matrix = Matrix.Identity;
			for (int num2 = jt1[num].parentCount - 1; num2 > -1; num2--)
			{
				int num3 = jt1[num].parentList[num2];
				if (!jt1[num3].matrixReady)
				{
					Matrix.CreateTranslation(jt1[num3].x, jt1[num3].y, jt1[num3].z, out result);
					matrix = result * matrix;
					Matrix.CreateRotationZ(jt1[num3].angleZ * ((float)Math.PI / 180f), out result);
					matrix = result * matrix;
					Matrix.CreateRotationX(jt1[num3].angleX * ((float)Math.PI / 180f), out result);
					matrix = result * matrix;
					Matrix.CreateRotationY(jt1[num3].angleY * ((float)Math.PI / 180f), out result);
					matrix = result * matrix;
					Matrix.CreateRotationZ(jt1[num3].rotZ * ((float)Math.PI / 180f), out result);
					matrix = result * matrix;
					Matrix.CreateRotationX(jt1[num3].rotX * ((float)Math.PI / 180f), out result);
					matrix = result * matrix;
					Matrix.CreateRotationY(jt1[num3].rotY * ((float)Math.PI / 180f), out result);
					matrix = result * matrix;
					jt1[num3].mv[uBufferID].M11 = matrix.M11;
					jt1[num3].mv[uBufferID].M12 = matrix.M12;
					jt1[num3].mv[uBufferID].M13 = matrix.M13;
					jt1[num3].mv[uBufferID].M14 = matrix.M14;
					jt1[num3].mv[uBufferID].M21 = matrix.M21;
					jt1[num3].mv[uBufferID].M22 = matrix.M22;
					jt1[num3].mv[uBufferID].M23 = matrix.M23;
					jt1[num3].mv[uBufferID].M24 = matrix.M24;
					jt1[num3].mv[uBufferID].M31 = matrix.M31;
					jt1[num3].mv[uBufferID].M32 = matrix.M32;
					jt1[num3].mv[uBufferID].M33 = matrix.M33;
					jt1[num3].mv[uBufferID].M34 = matrix.M34;
					jt1[num3].mv[uBufferID].M41 = matrix.M41;
					jt1[num3].mv[uBufferID].M42 = matrix.M42;
					jt1[num3].mv[uBufferID].M43 = matrix.M43;
					jt1[num3].mv[uBufferID].M44 = matrix.M44;
					jt1[num3].matrixReady = true;
				}
				else if (num2 > 0)
				{
					matrix = jt1[num3].mv[uBufferID];
				}
			}
			jt1[num].mv[uBufferID].M11 = matrix.M11;
			jt1[num].mv[uBufferID].M12 = matrix.M12;
			jt1[num].mv[uBufferID].M13 = matrix.M13;
			jt1[num].mv[uBufferID].M14 = matrix.M14;
			jt1[num].mv[uBufferID].M21 = matrix.M21;
			jt1[num].mv[uBufferID].M22 = matrix.M22;
			jt1[num].mv[uBufferID].M23 = matrix.M23;
			jt1[num].mv[uBufferID].M24 = matrix.M24;
			jt1[num].mv[uBufferID].M31 = matrix.M31;
			jt1[num].mv[uBufferID].M32 = matrix.M32;
			jt1[num].mv[uBufferID].M33 = matrix.M33;
			jt1[num].mv[uBufferID].M34 = matrix.M34;
			jt1[num].mv[uBufferID].M41 = matrix.M41;
			jt1[num].mv[uBufferID].M42 = matrix.M42;
			jt1[num].mv[uBufferID].M43 = matrix.M43;
			jt1[num].mv[uBufferID].M44 = matrix.M44;
			jt1[num].matrixReady = true;
		}
	}

	public static void Save_Player_Joint_Points(int playerID)
	{
		long numUsed = global::Players.Players.players[playerID].charMain.numUsed;
		for (long num = 0L; num < numUsed; num++)
		{
			global::Players.Players.players[playerID].particlePrev[num].v[0] = global::Players.Players.players[playerID].charMain.v1[num].v[0];
			global::Players.Players.players[playerID].particlePrev[num].v[1] = global::Players.Players.players[playerID].charMain.v1[num].v[1];
			global::Players.Players.players[playerID].particlePrev[num].v[2] = global::Players.Players.players[playerID].charMain.v1[num].v[2];
		}
		global::Players.Players.players[playerID].charMain.bbDirty = true;
	}

	public static void Check_Joint_Vertex_Addtl_Particles(long playerID, long jID, ref StructsClass.vtex[] v1, long numP, long vIndex, ref StructsClass.particle_list p1)
	{
		Matrix matrix = default(Matrix);
		Matrix result = default(Matrix);
		byte uBufferID = global::Rendering.Rendering.uBufferID;
		long num;
		if (!global::Players.Players.players[playerID].jt1[jID].matrixReady)
		{
			matrix = global::Players.Players.players[playerID].ct1.mv;
			for (num = global::Players.Players.players[playerID].jt1[jID].parentCount - 1; num > -1; num--)
			{
				long num2 = global::Players.Players.players[playerID].jt1[jID].parentList[num];
				if (!global::Players.Players.players[playerID].jt1[num2].matrixReady)
				{
					Matrix.CreateTranslation(global::Players.Players.players[playerID].jt1[num2].x, global::Players.Players.players[playerID].jt1[num2].y, global::Players.Players.players[playerID].jt1[num2].z, out result);
					matrix = result * matrix;
					Matrix.CreateRotationZ(global::Players.Players.players[playerID].jt1[num2].rotZ * ((float)Math.PI / 180f), out result);
					matrix = result * matrix;
					Matrix.CreateRotationX(global::Players.Players.players[playerID].jt1[num2].rotX * ((float)Math.PI / 180f), out result);
					matrix = result * matrix;
					Matrix.CreateRotationY(global::Players.Players.players[playerID].jt1[num2].rotY * ((float)Math.PI / 180f), out result);
					matrix = result * matrix;
					global::Players.Players.players[playerID].jt1[num2].mv[uBufferID].M11 = matrix.M11;
					global::Players.Players.players[playerID].jt1[num2].mv[uBufferID].M12 = matrix.M12;
					global::Players.Players.players[playerID].jt1[num2].mv[uBufferID].M13 = matrix.M13;
					global::Players.Players.players[playerID].jt1[num2].mv[uBufferID].M14 = matrix.M14;
					global::Players.Players.players[playerID].jt1[num2].mv[uBufferID].M21 = matrix.M21;
					global::Players.Players.players[playerID].jt1[num2].mv[uBufferID].M22 = matrix.M22;
					global::Players.Players.players[playerID].jt1[num2].mv[uBufferID].M23 = matrix.M23;
					global::Players.Players.players[playerID].jt1[num2].mv[uBufferID].M24 = matrix.M24;
					global::Players.Players.players[playerID].jt1[num2].mv[uBufferID].M31 = matrix.M31;
					global::Players.Players.players[playerID].jt1[num2].mv[uBufferID].M32 = matrix.M32;
					global::Players.Players.players[playerID].jt1[num2].mv[uBufferID].M33 = matrix.M33;
					global::Players.Players.players[playerID].jt1[num2].mv[uBufferID].M34 = matrix.M34;
					global::Players.Players.players[playerID].jt1[num2].mv[uBufferID].M41 = matrix.M41;
					global::Players.Players.players[playerID].jt1[num2].mv[uBufferID].M42 = matrix.M42;
					global::Players.Players.players[playerID].jt1[num2].mv[uBufferID].M43 = matrix.M43;
					global::Players.Players.players[playerID].jt1[num2].mv[uBufferID].M44 = matrix.M44;
					global::Players.Players.players[playerID].jt1[num2].matrixReady = true;
				}
			}
		}
		Matrix matrix2 = global::Players.Players.players[playerID].jt1[jID].mv[uBufferID];
		num = 0L;
		long num3 = vIndex;
		for (; num < p1.numUsed; num++)
		{
			v1[num3].v[0] = p1.v1[num].v[0] * matrix2.M11 + p1.v1[num].v[1] * matrix2.M21 + p1.v1[num].v[2] * matrix2.M31 + matrix2.M41;
			v1[num3].v[1] = p1.v1[num].v[0] * matrix2.M12 + p1.v1[num].v[1] * matrix2.M22 + p1.v1[num].v[2] * matrix2.M32 + matrix2.M42;
			v1[num3].v[2] = p1.v1[num].v[0] * matrix2.M13 + p1.v1[num].v[1] * matrix2.M23 + p1.v1[num].v[2] * matrix2.M33 + matrix2.M43;
			num3++;
		}
		for (num += vIndex; num < numP; num++)
		{
			v1[num].v[0] = 0f;
			v1[num].v[1] = 0f;
			v1[num].v[2] = 0f;
		}
	}

	public static void Calculate_Joint_Tangents(ref StructsClass.vtex v1, ref StructsClass.vtex v2, ref StructsClass.vtex v3, float u, float v, ref StructsClass.vtex tangent, ref StructsClass.vtex bitangent, byte threadID)
	{
		cjtT1[threadID].t[0] = 0f;
		cjtT1[threadID].t[1] = 0f;
		cjtT2[threadID].t[0] = 1f;
		cjtT2[threadID].t[1] = 0f;
		cjtT3[threadID].t[0] = 0f;
		cjtT3[threadID].t[1] = (float)Math.Sqrt(Math.Pow(v3.v[0] - v1.v[0], 2.0) + Math.Pow(v3.v[1] - v1.v[1], 2.0) + Math.Pow(v3.v[2] - v1.v[2], 2.0));
		mainC.utilMain.Calc_Tangent(ref v1, ref v2, ref v3, ref cjtT1[threadID], ref cjtT2[threadID], ref cjtT3[threadID], ref tangent);
	}

	public static void Calculate_Ring_Normals(int playerID, long jID, byte threadID)
	{
		int rings = global::Players.Players.players[playerID].jt1[jID].rings;
		int ringPtCnt = global::Players.Players.players[playerID].jt1[jID].ringPtCnt;
		int num = 0;
		for (int i = 0; i < rings; i++)
		{
			for (int j = 0; j < ringPtCnt; j++)
			{
				if (i == 0 || i == rings - 1)
				{
					global::Players.Players.players[playerID].jt1[jID].ringNorms[num].n[0] = global::Players.Players.players[playerID].jt1[jID].ringPts[num].v[0];
					global::Players.Players.players[playerID].jt1[jID].ringNorms[num].n[1] = global::Players.Players.players[playerID].jt1[jID].ringPts[num].v[1] - crnV1[threadID].v[1];
					global::Players.Players.players[playerID].jt1[jID].ringNorms[num].n[2] = global::Players.Players.players[playerID].jt1[jID].ringPts[num].v[2] - crnV1[threadID].v[2];
				}
				else
				{
					crnV1[threadID].v[0] = global::Players.Players.players[playerID].jt1[jID].ringPts[num].v[0] - global::Players.Players.players[playerID].jt1[jID].ringPts[num - ringPtCnt].v[0];
					crnV1[threadID].v[1] = global::Players.Players.players[playerID].jt1[jID].ringPts[num].v[1] - global::Players.Players.players[playerID].jt1[jID].ringPts[num - ringPtCnt].v[1];
					crnV1[threadID].v[2] = global::Players.Players.players[playerID].jt1[jID].ringPts[num].v[2] - global::Players.Players.players[playerID].jt1[jID].ringPts[num - ringPtCnt].v[2];
					crnV2[threadID].v[0] = global::Players.Players.players[playerID].jt1[jID].ringPts[num + ringPtCnt].v[0] - global::Players.Players.players[playerID].jt1[jID].ringPts[num].v[0];
					crnV2[threadID].v[1] = global::Players.Players.players[playerID].jt1[jID].ringPts[num + ringPtCnt].v[1] - global::Players.Players.players[playerID].jt1[jID].ringPts[num].v[1];
					crnV2[threadID].v[2] = global::Players.Players.players[playerID].jt1[jID].ringPts[num + ringPtCnt].v[2] - global::Players.Players.players[playerID].jt1[jID].ringPts[num].v[2];
					global::Util.Util.NormalizeVertex(ref crnV1[threadID]);
					global::Util.Util.NormalizeVertex(ref crnV2[threadID]);
					float num2 = (float)Math.Asin(crnV1[threadID].v[1]);
					float num3 = (float)Math.Asin(crnV2[threadID].v[1]);
					float num4 = (float)(Math.Pow(global::Players.Players.players[playerID].jt1[jID].ringPts[num - ringPtCnt].v[0], 2.0) + Math.Pow(global::Players.Players.players[playerID].jt1[jID].ringPts[num - ringPtCnt].v[2], 2.0));
					float num5 = (float)(Math.Pow(global::Players.Players.players[playerID].jt1[jID].ringPts[num].v[0], 2.0) + Math.Pow(global::Players.Players.players[playerID].jt1[jID].ringPts[num].v[2], 2.0));
					float num6 = (float)(Math.Pow(global::Players.Players.players[playerID].jt1[jID].ringPts[num + ringPtCnt].v[0], 2.0) + Math.Pow(global::Players.Players.players[playerID].jt1[jID].ringPts[num + ringPtCnt].v[2], 2.0));
					if (num4 > num5)
					{
						num2 *= -1f;
					}
					if (num5 > num6)
					{
						num3 *= -1f;
					}
					num3 += num2 / 2f;
					global::Players.Players.players[playerID].jt1[jID].ringNorms[num].n[0] = global::Players.Players.players[playerID].jt1[jID].ringPts[num].v[0];
					global::Players.Players.players[playerID].jt1[jID].ringNorms[num].n[1] = 0f;
					global::Players.Players.players[playerID].jt1[jID].ringNorms[num].n[2] = global::Players.Players.players[playerID].jt1[jID].ringPts[num].v[2];
					global::Players.Players.players[playerID].jt1[jID].ringNorms[num].n[1] += (float)Math.Sin(num3 / 180f * (float)Math.PI) * num5;
				}
				num++;
			}
		}
	}

	public void Update_Joints_For_New_Position(short playerID)
	{
		Redo_Bounding_Box(playerID, global::Players.Players.players[playerID].primaryWeaponMountWeapon);
		Reset_Joint_Data(playerID);
		short numJoints = playerJoints[global::Players.Players.players[playerID].jointPackage].numJoints;
		Matrix.CreateRotationZ(global::Players.Players.players[playerID].zRotation * ((float)Math.PI / 180f), out global::Players.Players.players[playerID].mv[global::Rendering.Rendering.uBufferID]);
		for (short num = 0; num < numJoints; num++)
		{
			Translate_Player_Joint_Vertex(playerID, num, copyToParticleList: true);
		}
		Check_Joint_Vertex_Addtl_Particles(playerID, global::Players.Players.players[playerID].weapon1.jointID, ref global::Players.Players.players[playerID].charMain.v1, global::Players.Players.players[playerID].charMain.numP, playerJoints[global::Players.Players.players[playerID].jointPackage].numJointPoints, ref global::Weapons.Weapons.wp1[global::Players.Players.players[playerID].primaryWeaponMountWeapon].box);
		Save_Player_Joint_Points(playerID);
		Sync_Player_Matrices(playerID, global::Rendering.Rendering.uBufferID, global::Rendering.Rendering.rBufferID);
	}

	public void Init_Joints(byte threadID)
	{
	}

	public void Load_Player_Joints()
	{
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		int num4 = 0;
		num2 = global::Players.Players.numPlayerRaces;
		for (int i = 0; i < num2; i++)
		{
			num4 += global::Players.Players.playerRaces[i].numTypes;
		}
		playerJoints = new StructsClass.JointCollection[num4];
		for (int i = 0; i < num2; i++)
		{
			num3 = global::Players.Players.playerRaces[i].numTypes;
			for (int j = 0; j < num3; j++)
			{
				playerJoints[num] = new StructsClass.JointCollection();
				Load_Joints("The_CoOp_Zombie_Game\\Config_Files\\" + global::Players.Players.playerRaces[i].jointPackageName[j], ref playerJoints[num], (short)num);
				global::Players.Players.playerRaces[i].jointPackage[j] = (byte)num;
				if (playerJoints[num].numJoints > global::Players.Players.maxNumPlayerRaceJoints)
				{
					global::Players.Players.raceWithMostJoints = (byte)i;
					global::Players.Players.raceTypeWithMostJoints = (byte)j;
					global::Players.Players.maxNumPlayerRaceJoints = (ushort)playerJoints[num].numJoints;
				}
				num++;
			}
		}
	}

	public void Load_Joints(string fileName, ref StructsClass.JointCollection loadJC, short jColID)
	{
		_ = global::Rendering.Rendering.uBufferID;
		long num = 0L;
		long num2 = 0L;
		long num3 = 0L;
		short num4 = 0;
		Matrix[] array = new Matrix[1];
		Stream stream = TitleContainer.OpenStream(fileName);
		byte[] array2 = new byte[stream.Length];
		if (stream.CanRead)
		{
			loadJC.usingParticles = false;
			stream.Read(array2, 0, array2.Length);
			string text = mainC.utilMain.Byte_Array_To_String(array2);
			string[] array3 = text.Split('\n', '\r');
			int i = 0;
			int num5 = 0;
			for (; i < array3.Length; i++)
			{
				if (array3[i].Length > 0)
				{
					num5++;
				}
			}
			if (num5 < 1)
			{
				stream.Close();
				return;
			}
			float num6 = 1f;
			string[] array4 = new string[num5];
			i = 0;
			num5 = 0;
			for (; i < array3.Length; i++)
			{
				if (array3[i].Length > 0)
				{
					array4[num5++] = array3[i];
				}
			}
			for (i = 0; i < num5; i++)
			{
				array3 = array4[i].Split(' ', '\t');
				int j = 0;
				int num7 = 0;
				for (; j < array3.Length; j++)
				{
					if (array3[j].Length > 0)
					{
						num7++;
					}
				}
				if (num7 < 1)
				{
					continue;
				}
				string[] array5 = new string[num7];
				j = 0;
				num7 = 0;
				for (; j < array3.Length; j++)
				{
					if (array3[j].Length > 0)
					{
						array5[num7++] = array3[j];
					}
				}
				int num8 = 0;
				if (array5[0].Equals("joint", StringComparison.OrdinalIgnoreCase))
				{
					num8 = 1;
				}
				else if (array5[0].Equals("coords", StringComparison.OrdinalIgnoreCase))
				{
					num8 = 2;
				}
				else if (array5[0].Equals("angle", StringComparison.OrdinalIgnoreCase))
				{
					num8 = 3;
				}
				else if (array5[0].Equals("scale", StringComparison.OrdinalIgnoreCase))
				{
					num8 = 4;
				}
				else if (array5[0].Equals("length", StringComparison.OrdinalIgnoreCase))
				{
					num8 = 5;
				}
				else if (array5[0].Equals("minAngle", StringComparison.OrdinalIgnoreCase))
				{
					num8 = 6;
				}
				else if (array5[0].Equals("maxAngle", StringComparison.OrdinalIgnoreCase))
				{
					num8 = 7;
				}
				else if (array5[0].Equals("minPivot2", StringComparison.OrdinalIgnoreCase))
				{
					num8 = 8;
				}
				else if (array5[0].Equals("maxPivot2", StringComparison.OrdinalIgnoreCase))
				{
					num8 = 9;
				}
				else if (array5[0].Equals("minPivot", StringComparison.OrdinalIgnoreCase))
				{
					num8 = 10;
				}
				else if (array5[0].Equals("maxPivot", StringComparison.OrdinalIgnoreCase))
				{
					num8 = 11;
				}
				else if (array5[0].Equals("startAngle", StringComparison.OrdinalIgnoreCase))
				{
					num8 = 12;
				}
				else if (array5[0].Equals("startPivot2", StringComparison.OrdinalIgnoreCase))
				{
					num8 = 13;
				}
				else if (array5[0].Equals("startPivot", StringComparison.OrdinalIgnoreCase))
				{
					num8 = 14;
				}
				else if (array5[0].Equals("texture", StringComparison.OrdinalIgnoreCase))
				{
					num8 = 15;
				}
				else if (array5[0].Equals("RingPts", StringComparison.OrdinalIgnoreCase))
				{
					num8 = 16;
				}
				else if (array5[0].Equals("Connector", StringComparison.OrdinalIgnoreCase))
				{
					num8 = 17;
				}
				else if (array5[0].Equals("num_joints", StringComparison.OrdinalIgnoreCase))
				{
					num8 = 18;
				}
				else if (array5[0].Equals("model", StringComparison.OrdinalIgnoreCase))
				{
					num8 = 19;
				}
				else if (array5[0].Equals("pinOffset", StringComparison.OrdinalIgnoreCase))
				{
					num8 = 20;
				}
				else if (array5[0].Equals("RingAdjust", StringComparison.OrdinalIgnoreCase))
				{
					num8 = 21;
				}
				else if (array5[0].Equals("ParticleListSkip", StringComparison.OrdinalIgnoreCase))
				{
					num8 = 22;
				}
				else if (array5[0].Equals("invBindPose", StringComparison.OrdinalIgnoreCase))
				{
					num8 = 23;
				}
				else if (array5[0].Equals("Rings", StringComparison.OrdinalIgnoreCase))
				{
					num8 = 24;
				}
				else if (array5[0].Equals("Ring-Point-Count", StringComparison.OrdinalIgnoreCase))
				{
					num8 = 25;
				}
				else if (array5[0].Equals("ParentID", StringComparison.OrdinalIgnoreCase))
				{
					num8 = 26;
				}
				else if (array5[0].Equals("ChildIDs", StringComparison.OrdinalIgnoreCase))
				{
					num8 = 27;
				}
				else if (array5[0].Equals("Damage", StringComparison.OrdinalIgnoreCase))
				{
					num8 = 28;
				}
				else if (array5[0].Equals("Particles", StringComparison.OrdinalIgnoreCase))
				{
					num8 = 29;
				}
				else if (array5[0].Equals("crouchAdj", StringComparison.OrdinalIgnoreCase))
				{
					num8 = 32;
				}
				else if (array5[0].Equals("CollisionData", StringComparison.OrdinalIgnoreCase))
				{
					num8 = 34;
				}
				switch (num8)
				{
				case 1:
					if (array5.Length > 1)
					{
						num2 = long.Parse(array5[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
						if (num2 >= num4)
						{
							num2 = num4 - 1;
						}
						loadJC.InvBindPose[num2].M11 = 0f;
						loadJC.InvBindPose[num2].M12 = 0f;
						loadJC.InvBindPose[num2].M13 = 0f;
					}
					num = -1L;
					break;
				case 2:
					if (num2 >= 0 && array5.Length > 3)
					{
						loadJC.jt1[num2].x = float.Parse(array5[1], CultureInfo.InvariantCulture.NumberFormat) * num6;
						loadJC.jt1[num2].y = float.Parse(array5[2], CultureInfo.InvariantCulture.NumberFormat) * num6;
						loadJC.jt1[num2].z = float.Parse(array5[3], CultureInfo.InvariantCulture.NumberFormat) * num6;
					}
					else if (num >= 0 && array5.Length > 3)
					{
						loadJC.ct1.x = float.Parse(array5[1], CultureInfo.InvariantCulture.NumberFormat) * num6;
						loadJC.ct1.y = float.Parse(array5[2], CultureInfo.InvariantCulture.NumberFormat) * num6;
						loadJC.ct1.z = float.Parse(array5[3], CultureInfo.InvariantCulture.NumberFormat) * num6;
					}
					break;
				case 3:
					if (num2 >= 0 && array5.Length > 3)
					{
						loadJC.jt1[num2].angleX = float.Parse(array5[1], CultureInfo.InvariantCulture.NumberFormat);
						loadJC.jt1[num2].angleY = float.Parse(array5[2], CultureInfo.InvariantCulture.NumberFormat);
						loadJC.jt1[num2].angleZ = float.Parse(array5[3], CultureInfo.InvariantCulture.NumberFormat);
						loadJC.jt1[num2].mvBase = Matrix.CreateRotationY(loadJC.jt1[num2].angleY * ((float)Math.PI / 180f)) * Matrix.CreateRotationX(loadJC.jt1[num2].angleX * ((float)Math.PI / 180f)) * Matrix.CreateRotationZ(loadJC.jt1[num2].angleZ * ((float)Math.PI / 180f));
					}
					else if (num >= 0 && array5.Length > 3)
					{
						loadJC.ct1.angleX = float.Parse(array5[1], CultureInfo.InvariantCulture.NumberFormat);
						loadJC.ct1.angleY = float.Parse(array5[2], CultureInfo.InvariantCulture.NumberFormat);
						loadJC.ct1.angleZ = float.Parse(array5[3], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 4:
					if (array5.Length > 1)
					{
						num6 = float.Parse(array5[1], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 5:
					if (num2 >= 0 && array5.Length > 1)
					{
						loadJC.jt1[num2].len = float.Parse(array5[1], CultureInfo.InvariantCulture.NumberFormat) * num6;
					}
					break;
				case 6:
					if (num2 >= 0 && array5.Length > 1)
					{
						loadJC.jt1[num2].minAngle = float.Parse(array5[1], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 7:
					if (num2 >= 0 && array5.Length > 1)
					{
						loadJC.jt1[num2].maxAngle = float.Parse(array5[1], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 8:
					if (num2 >= 0 && array5.Length > 1)
					{
						loadJC.jt1[num2].minPivot2 = float.Parse(array5[1], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 9:
					if (num2 >= 0 && array5.Length > 1)
					{
						loadJC.jt1[num2].maxPivot2 = float.Parse(array5[1], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 10:
					if (num2 >= 0 && array5.Length > 1)
					{
						loadJC.jt1[num2].minPivot = float.Parse(array5[1], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 11:
					if (num2 >= 0 && array5.Length > 1)
					{
						loadJC.jt1[num2].maxPivot = float.Parse(array5[1], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 12:
					if (num2 >= 0 && array5.Length > 1)
					{
						loadJC.jt1[num2].rotX = (loadJC.jt1[num2].targetAngle = float.Parse(array5[1], CultureInfo.InvariantCulture.NumberFormat));
					}
					break;
				case 13:
					if (num2 >= 0 && array5.Length > 1)
					{
						loadJC.jt1[num2].rotY = (loadJC.jt1[num2].targetPivot2 = float.Parse(array5[1], CultureInfo.InvariantCulture.NumberFormat));
					}
					break;
				case 14:
					if (num2 >= 0 && array5.Length > 1)
					{
						loadJC.jt1[num2].rotZ = (loadJC.jt1[num2].targetPivot = float.Parse(array5[1], CultureInfo.InvariantCulture.NumberFormat));
					}
					break;
				case 15:
					if (num2 >= 0 && array5.Length > 1)
					{
						loadJC.jt1[num2].texID = mainC.texturesMain.Find_Texture(array5[1], 0);
					}
					else if (num >= 0 && array5.Length > 1)
					{
						loadJC.ct1.texID = mainC.texturesMain.Find_Texture(array5[1], 0);
					}
					break;
				case 16:
					if (num3 > 0 && num2 >= 0 && loadJC.jt1[num2].rings > 0 && loadJC.jt1[num2].ringPtCnt > 0 && array5.Length > 1)
					{
						for (int l = 0; l < num3 && l < array5.Length - 1; l++)
						{
							loadJC.jt1[num2].ringPins[l] = float.Parse(array5[l + 1], CultureInfo.InvariantCulture.NumberFormat);
						}
					}
					break;
				case 17:
					if (array5.Length > 1)
					{
						num = 0L;
						loadJC.ct1.modID = -1L;
					}
					num2 = -1L;
					break;
				case 18:
					if (array5.Length > 1)
					{
						num4 = short.Parse(array5[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
						loadJC.numJoints = num4;
						loadJC.jt1 = new StructsClass.joint[num4];
						loadJC.jStat = new short[num4];
						loadJC.InvBindPose = new Matrix[num4];
						array = new Matrix[num4];
						Create_Joints(num4, ref loadJC.jt1, ref loadJC.jStat);
						for (int l = 0; l < num4; l++)
						{
							loadJC.jt1[l].status = 1;
							loadJC.jt1[l].angleSpeed = 100f;
							loadJC.jt1[l].pivotSpeed = 20f;
							loadJC.jt1[l].pivot2Speed = 20f;
							loadJC.jt1[l].pinOffset = 1f;
							loadJC.jt1[l].rings = 0;
							loadJC.jt1[l].ringPtCnt = 0;
							loadJC.jt1[l].adjustStartRing = 0;
							loadJC.jt1[l].adjustEndRing = 0;
							loadJC.jt1[l].rotX = 0f;
							loadJC.jt1[l].rotY = 0f;
							loadJC.jt1[l].damageJoint = false;
							loadJC.jt1[l].parentID = -1;
							loadJC.jt1[l].subIDCount = 0;
							loadJC.jt1[l].pSkip1 = 1;
							loadJC.jt1[l].pSkip2 = 1;
							loadJC.jt1[l].modID = -1;
							loadJC.jt1[l].len = 0f;
							loadJC.jt1[l].damageMultiplier = 0f;
						}
					}
					break;
				case 19:
					if (num2 >= 0 && array5.Length > 1)
					{
						loadJC.jt1[num2].modID = (short)mainC.modelsMain.Find_Model(array5[1]);
					}
					else if (num >= 0 && array5.Length > 1)
					{
						loadJC.ct1.modID = mainC.modelsMain.Find_Model(array5[1]);
					}
					break;
				case 20:
					if (num2 >= 0 && array5.Length > 1)
					{
						loadJC.jt1[num2].pinOffset = float.Parse(array5[1], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 21:
					if (num2 >= 0 && array5.Length > 2)
					{
						loadJC.jt1[num2].adjustStartRing = short.Parse(array5[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
						loadJC.jt1[num2].adjustEndRing = short.Parse(array5[2], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 22:
					if (num2 >= 0 && array5.Length > 2)
					{
						loadJC.jt1[num2].pSkip1 = short.Parse(array5[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
						loadJC.jt1[num2].pSkip2 = short.Parse(array5[2], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 23:
					if (num2 >= 0 && array5.Length > 12)
					{
						ref Matrix reference = ref loadJC.InvBindPose[num2];
						reference = Matrix.CreateTranslation(float.Parse(array5[10], CultureInfo.InvariantCulture.NumberFormat), float.Parse(array5[11], CultureInfo.InvariantCulture.NumberFormat), float.Parse(array5[12], CultureInfo.InvariantCulture.NumberFormat));
						loadJC.InvBindPose[num2].M11 = float.Parse(array5[1], CultureInfo.InvariantCulture.NumberFormat);
						loadJC.InvBindPose[num2].M12 = float.Parse(array5[2], CultureInfo.InvariantCulture.NumberFormat);
						loadJC.InvBindPose[num2].M13 = float.Parse(array5[3], CultureInfo.InvariantCulture.NumberFormat);
						loadJC.InvBindPose[num2].M21 = float.Parse(array5[4], CultureInfo.InvariantCulture.NumberFormat);
						loadJC.InvBindPose[num2].M22 = float.Parse(array5[5], CultureInfo.InvariantCulture.NumberFormat);
						loadJC.InvBindPose[num2].M23 = float.Parse(array5[6], CultureInfo.InvariantCulture.NumberFormat);
						loadJC.InvBindPose[num2].M31 = float.Parse(array5[7], CultureInfo.InvariantCulture.NumberFormat);
						loadJC.InvBindPose[num2].M32 = float.Parse(array5[8], CultureInfo.InvariantCulture.NumberFormat);
						loadJC.InvBindPose[num2].M33 = float.Parse(array5[9], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 24:
					if (num2 >= 0 && array5.Length > 1)
					{
						loadJC.jt1[num2].rings = int.Parse(array5[1], CultureInfo.InvariantCulture.NumberFormat);
					}
					if (loadJC.jt1[num2].rings > 0 && loadJC.jt1[num2].ringPtCnt > 0)
					{
						num3 = loadJC.jt1[num2].rings * loadJC.jt1[num2].ringPtCnt;
						loadJC.jt1[num2].ringPts = new StructsClass.vtex[num3];
						loadJC.jt1[num2].ringPins = new float[num3];
						loadJC.jt1[num2].ringNorms = new StructsClass.vnorm[num3];
						loadJC.jt1[num2].tangent = new StructsClass.vtex[num3];
						loadJC.jt1[num2].bitangent = new StructsClass.vtex[num3];
						loadJC.jt1[num2].angles = new float[loadJC.jt1[num2].rings];
						for (int k = 0; k < num3; k++)
						{
							loadJC.jt1[num2].ringPins[k] = 1f;
							loadJC.jt1[num2].ringPts[k] = new StructsClass.vtex();
							loadJC.jt1[num2].tangent[k] = new StructsClass.vtex();
							loadJC.jt1[num2].bitangent[k] = new StructsClass.vtex();
							loadJC.jt1[num2].ringNorms[k] = new StructsClass.vnorm();
						}
					}
					break;
				case 25:
					if (num2 >= 0 && array5.Length > 1)
					{
						loadJC.jt1[num2].ringPtCnt = int.Parse(array5[1], CultureInfo.InvariantCulture.NumberFormat);
					}
					if (loadJC.jt1[num2].rings > 0 && loadJC.jt1[num2].ringPtCnt > 0)
					{
						num3 = loadJC.jt1[num2].rings * loadJC.jt1[num2].ringPtCnt;
						loadJC.jt1[num2].ringPts = new StructsClass.vtex[num3];
						loadJC.jt1[num2].ringPins = new float[num3];
						loadJC.jt1[num2].ringNorms = new StructsClass.vnorm[num3];
						loadJC.jt1[num2].tangent = new StructsClass.vtex[num3];
						loadJC.jt1[num2].bitangent = new StructsClass.vtex[num3];
						loadJC.jt1[num2].angles = new float[loadJC.jt1[num2].rings];
						for (int k = 0; k < num3; k++)
						{
							loadJC.jt1[num2].ringPins[k] = 1f;
							loadJC.jt1[num2].ringPts[k] = new StructsClass.vtex();
							loadJC.jt1[num2].tangent[k] = new StructsClass.vtex();
							loadJC.jt1[num2].bitangent[k] = new StructsClass.vtex();
							loadJC.jt1[num2].ringNorms[k] = new StructsClass.vnorm();
						}
					}
					break;
				case 26:
					if (num2 >= 0 && array5.Length > 1)
					{
						loadJC.jt1[num2].parentID = short.Parse(array5[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 27:
					if (num2 < 0 || array5.Length <= 2)
					{
						break;
					}
					loadJC.jt1[num2].subIDCount = int.Parse(array5[1], CultureInfo.InvariantCulture.NumberFormat);
					if (loadJC.jt1[num2].subIDCount > array5.Length - 2)
					{
						loadJC.jt1[num2].subIDCount = array5.Length - 2;
					}
					if (loadJC.jt1[num2].subIDCount > 0)
					{
						loadJC.jt1[num2].idList = new short[loadJC.jt1[num2].subIDCount];
						for (int l = 0; l < loadJC.jt1[num2].subIDCount; l++)
						{
							loadJC.jt1[num2].idList[l] = short.Parse(array5[l + 2], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
						}
					}
					else
					{
						loadJC.jt1[num2].subIDCount = 0;
					}
					break;
				case 28:
					if (num2 >= 0 && array5.Length > 1)
					{
						float num10 = float.Parse(array5[1], CultureInfo.InvariantCulture.NumberFormat);
						if (num10 > 0f)
						{
							loadJC.jt1[num2].damageMultiplier = num10;
							loadJC.jt1[num2].damageJoint = true;
						}
					}
					break;
				case 29:
				{
					if (num2 < 0 || array5.Length <= 1)
					{
						break;
					}
					short num9 = short.Parse(array5[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
					loadJC.jt1[num2].numParticles = num9;
					num9 *= 3;
					if (array5.Length > num9 + 1)
					{
						loadJC.usingParticles = true;
						loadJC.jt1[num2].particles = new float[num9];
						for (int k = 0; k < num9; k++)
						{
							loadJC.jt1[num2].particles[k] = float.Parse(array5[k + 2], CultureInfo.InvariantCulture.NumberFormat) * num6;
						}
					}
					else
					{
						loadJC.jt1[num2].numParticles = 0;
					}
					break;
				}
				case 32:
					if (array5.Length > 3)
					{
						loadJC.crouchAdjX = float.Parse(array5[1], CultureInfo.InvariantCulture.NumberFormat) * num6;
						loadJC.crouchAdjY = float.Parse(array5[2], CultureInfo.InvariantCulture.NumberFormat) * num6;
						loadJC.crouchAdjZ = float.Parse(array5[3], CultureInfo.InvariantCulture.NumberFormat) * num6;
					}
					break;
				case 34:
					if (array5.Length > 4)
					{
						loadJC.jt1[num2].dirX = float.Parse(array5[1], CultureInfo.InvariantCulture.NumberFormat) * num6;
						loadJC.jt1[num2].dirY = float.Parse(array5[2], CultureInfo.InvariantCulture.NumberFormat) * num6;
						loadJC.jt1[num2].dirZ = float.Parse(array5[3], CultureInfo.InvariantCulture.NumberFormat) * num6;
						loadJC.jt1[num2].radius = float.Parse(array5[4], CultureInfo.InvariantCulture.NumberFormat) * num6;
						loadJC.jt1[num2].radSqr = loadJC.jt1[num2].radius * loadJC.jt1[num2].radius;
					}
					break;
				}
			}
		}
		stream.Close();
		loadJC.numJointPoints = 0;
		for (int k = 0; k < loadJC.numJoints; k++)
		{
			loadJC.jt1[k].mvBase = loadJC.jt1[k].mvBase * Matrix.CreateTranslation(loadJC.jt1[k].x, loadJC.jt1[k].y, loadJC.jt1[k].z);
			loadJC.jt1[k].targetAngle = loadJC.jt1[k].rotX;
			loadJC.jt1[k].targetPivot2 = loadJC.jt1[k].rotY;
			loadJC.jt1[k].targetPivot = loadJC.jt1[k].rotZ;
			loadJC.jt1[k].lenSquared = loadJC.jt1[k].dirX * loadJC.jt1[k].dirX + loadJC.jt1[k].dirY * loadJC.jt1[k].dirY + loadJC.jt1[k].dirZ * loadJC.jt1[k].dirZ;
			loadJC.jt1[k].len = (float)Math.Sqrt(loadJC.jt1[k].lenSquared);
			loadJC.jt1[k].matrixReady = false;
			loadJC.jt1[k].pinAngleD = (float)Math.PI * 2f / (float)loadJC.jt1[k].ringPtCnt;
			loadJC.jt1[k].ringYD = loadJC.jt1[k].len / (float)(loadJC.jt1[k].rings - 1);
			loadJC.jt1[k].pListStart = (short)loadJC.numJointPoints;
			if (loadJC.usingParticles)
			{
				loadJC.numJointPoints += loadJC.jt1[k].numParticles;
			}
			int l = 0;
			int num11 = k;
			int i = 1;
			for (; l < loadJC.numJoints; l++)
			{
				if (loadJC.jt1[num11].parentID <= -1)
				{
					break;
				}
				i++;
				num11 = loadJC.jt1[num11].parentID;
			}
			loadJC.jt1[k].parentList = new short[i];
			loadJC.jt1[k].parentCount = (short)i;
			loadJC.jt1[k].parentList[0] = (short)k;
			l = 0;
			num11 = k;
			int j = 1;
			for (; l < loadJC.numJoints; l++)
			{
				if (j >= i)
				{
					break;
				}
				if (loadJC.jt1[num11].parentID <= -1)
				{
					break;
				}
				num11 = loadJC.jt1[num11].parentID;
				loadJC.jt1[k].parentList[j++] = (short)num11;
			}
			if (k == 34)
			{
				k = 34;
			}
			ref Matrix reference2 = ref array[k];
			reference2 = loadJC.jt1[k].mvBase;
			if (loadJC.jt1[k].parentID == -1)
			{
				if (loadJC.InvBindPose[k].M11 == 0f && loadJC.InvBindPose[k].M12 == 0f && loadJC.InvBindPose[k].M13 == 0f)
				{
					ref Matrix reference3 = ref loadJC.InvBindPose[k];
					reference3 = Matrix.Invert(array[k]);
				}
				continue;
			}
			ref Matrix reference4 = ref array[k];
			reference4 = array[k] * array[loadJC.jt1[k].parentID];
			if (loadJC.InvBindPose[k].M11 == 0f && loadJC.InvBindPose[k].M12 == 0f && loadJC.InvBindPose[k].M13 == 0f)
			{
				ref Matrix reference5 = ref loadJC.InvBindPose[k];
				reference5 = Matrix.Invert(array[k]);
			}
		}
		loadJC.ct1.mv = Matrix.Identity;
		if (jColID > -1)
		{
			Translate_Player_Joint_Collection(jColID);
			for (int k = 0; k < loadJC.numJoints; k++)
			{
				if (loadJC.jt1[k].modID > -1)
				{
					mainC.modelsMain.Update_Model_For_Rigging((byte)k, jColID, loadJC.jt1[k].modID);
				}
			}
		}
		loadJC.ct1.mv = Matrix.CreateRotationZ(loadJC.ct1.angleZ * ((float)Math.PI / 180f)) * Matrix.CreateRotationY(loadJC.ct1.angleY * ((float)Math.PI / 180f)) * Matrix.CreateRotationX(loadJC.ct1.angleX * ((float)Math.PI / 180f));
		loadJC.ct1.mv.M41 = loadJC.ct1.x;
		loadJC.ct1.mv.M42 = loadJC.ct1.y;
		loadJC.ct1.mv.M43 = loadJC.ct1.z;
	}

	public void Load_Joints_Basic(string fileName)
	{
		long num = 0L;
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
				if (array4[0].Equals("joint", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 1;
				}
				else if (array4[0].Equals("coords", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 2;
				}
				else if (array4[0].Equals("angle", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 3;
				}
				else if (array4[0].Equals("minAngle", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 4;
				}
				else if (array4[0].Equals("maxAngle", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 5;
				}
				else if (array4[0].Equals("minPivot2", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 6;
				}
				else if (array4[0].Equals("maxPivot2", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 7;
				}
				else if (array4[0].Equals("minPivot", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 8;
				}
				else if (array4[0].Equals("maxPivot", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 9;
				}
				else if (array4[0].Equals("startAngle", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 10;
				}
				else if (array4[0].Equals("startPivot2", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 11;
				}
				else if (array4[0].Equals("startPivot", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 12;
				}
				else if (array4[0].Equals("num_joints", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 14;
				}
				else if (array4[0].Equals("model", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 15;
				}
				else if (array4[0].Equals("objectID", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 16;
				}
				else if (array4[0].Equals("type", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 17;
				}
				else if (array4[0].Equals("time", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 18;
				}
				switch (num4)
				{
				case 1:
					if (array4.Length > 1)
					{
						num = long.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
						if (num < numJointsBasic)
						{
							jtb1[num].status = 1;
							jtb1[num].angle = 0f;
							jtb1[num].pivot = 0f;
							jtb1[num].pivot2 = 0f;
							jtb1[num].angleSpeed = 100f;
							jtb1[num].pivotSpeed = 20f;
							jtb1[num].pivot2Speed = 20f;
							jtb1[num].time = 0f;
							jtb1[num].x = 0f;
							jtb1[num].y = 0f;
							jtb1[num].z = 0f;
							jtb1[num].xSpeed = 200f;
							jtb1[num].ySpeed = 200f;
							jtb1[num].zSpeed = 200f;
							jtb1[num].time = 0f;
							jtb1[num].modID = -1;
							jtb1[num].type = 0;
							jtb1[num].objID = -1;
						}
						else
						{
							num = numJointsBasic - 1;
						}
					}
					break;
				case 2:
					if (num >= 0 && array4.Length > 3)
					{
						jtb1[num].x = (jtb1[num].targetX = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat));
						jtb1[num].y = (jtb1[num].targetY = float.Parse(array4[2], CultureInfo.InvariantCulture.NumberFormat));
						jtb1[num].z = (jtb1[num].targetZ = float.Parse(array4[3], CultureInfo.InvariantCulture.NumberFormat));
					}
					break;
				case 3:
					if (num >= 0 && array4.Length > 3)
					{
						jtb1[num].angleX = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
						jtb1[num].angleY = float.Parse(array4[2], CultureInfo.InvariantCulture.NumberFormat);
						jtb1[num].angleZ = float.Parse(array4[3], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 4:
					if (num >= 0 && array4.Length > 1)
					{
						jtb1[num].minAngle = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 5:
					if (num >= 0 && array4.Length > 1)
					{
						jtb1[num].maxAngle = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 6:
					if (num >= 0 && array4.Length > 1)
					{
						jtb1[num].minPivot2 = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 7:
					if (num >= 0 && array4.Length > 1)
					{
						jtb1[num].maxPivot2 = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 8:
					if (num >= 0 && array4.Length > 1)
					{
						jtb1[num].minPivot = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 9:
					if (num >= 0 && array4.Length > 1)
					{
						jtb1[num].maxPivot = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 10:
					if (num >= 0 && array4.Length > 2)
					{
						jtb1[num].angle = (jtb1[num].targetAngle = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat));
					}
					break;
				case 11:
					if (num >= 0 && array4.Length > 2)
					{
						jtb1[num].pivot2 = (jtb1[num].targetPivot2 = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat));
					}
					break;
				case 12:
					if (num >= 0 && array4.Length > 1)
					{
						jtb1[num].pivot = (jtb1[num].targetPivot = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat));
					}
					break;
				case 14:
					if (array4.Length <= 1)
					{
						break;
					}
					numJointsBasic = long.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
					if (numJointsBasic > 0)
					{
						jtb1 = new StructsClass.joint_basic[numJointsBasic];
						for (int k = 0; k < numJointsBasic; k++)
						{
							jtb1[k] = default(StructsClass.joint_basic);
						}
					}
					break;
				case 15:
					if (num >= 0 && array4.Length > 1)
					{
						jtb1[num].modID = (short)mainC.modelsMain.Find_Model(array4[1]);
					}
					break;
				case 16:
					if (num >= 0 && array4.Length > 1)
					{
						jtb1[num].objID = int.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 17:
					if (num >= 0 && array4.Length > 1)
					{
						jtb1[num].type = byte.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 18:
					if (num >= 0 && array4.Length > 1)
					{
						jtb1[num].time = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				}
			}
		}
		stream.Close();
	}

	public bool Create_Joints(long cnt, ref StructsClass.joint[] createJoint, ref short[] statList)
	{
		long num = 0L;
		long num2 = 0L;
		for (; num < cnt; num++)
		{
			for (; num2 < 100; num2++)
			{
				if (statList[num2] < 1)
				{
					createJoint[num2] = new StructsClass.joint();
					createJoint[num2].status = 0;
					statList[num2] = 1;
					num2++;
					break;
				}
			}
		}
		return true;
	}

	public void Reset_Player_Joint_Angles(ushort playerID)
	{
		int numJoints = playerJoints[global::Players.Players.players[playerID].jointPackage].numJoints;
		for (byte b = 0; b < numJoints; b++)
		{
			global::Players.Players.players[playerID].jt1[b].rotX = 0f;
			global::Players.Players.players[playerID].jt1[b].rotZ = 0f;
			global::Players.Players.players[playerID].jt1[b].rotY = 0f;
		}
	}

	public void Reset_Player_Joint_Targets(ushort playerID)
	{
		int numJoints = playerJoints[global::Players.Players.players[playerID].jointPackage].numJoints;
		for (byte b = 0; b < numJoints; b++)
		{
			global::Players.Players.players[playerID].jt1[b].targetAngle = 0f;
			global::Players.Players.players[playerID].jt1[b].targetPivot = 0f;
			global::Players.Players.players[playerID].jt1[b].targetPivot2 = 0f;
		}
	}

	public int Check_Joint_Collision_With_Point_Threaded(long pID, ref StructsClass.vtex v1, float bulletRadius, byte threadID)
	{
		Matrix matrix = default(Matrix);
		Vector3 position = default(Vector3);
		byte uBufferID = global::Rendering.Rendering.uBufferID;
		int numJoints = playerJoints[global::Players.Players.players[pID].jointPackage].numJoints;
		Reset_Joint_Collision_Check_Threaded(pID, threadID);
		Matrix matrix2 = Matrix.CreateTranslation(global::Players.Players.players[pID].charP.position.v[0], global::Players.Players.players[pID].charP.position.v[1], global::Players.Players.players[pID].charP.position.v[2]);
		for (byte b = 0; b < numJoints; b++)
		{
			if (global::Players.Players.players[pID].jt1[b].damageJoint)
			{
				position.X = global::Players.Players.players[pID].jt1[b].dirX;
				position.Y = global::Players.Players.players[pID].jt1[b].dirY;
				position.Z = global::Players.Players.players[pID].jt1[b].dirZ;
				matrix = global::Players.Players.players[pID].jt1[b].mv[uBufferID];
				matrix.M41 = 0f;
				matrix.M42 = 0f;
				matrix.M43 = 0f;
				Vector3.Transform(ref position, ref matrix, out position);
				matrix = global::Players.Players.players[pID].jt1[b].mv[uBufferID] * matrix2;
				cjcwpV2T[threadID].v[0] = position.X;
				cjcwpV2T[threadID].v[1] = position.Y;
				cjcwpV2T[threadID].v[2] = position.Z;
				cjcwpV3T[threadID].v[0] = v1.v[0] - matrix.M41;
				cjcwpV3T[threadID].v[1] = v1.v[1] - matrix.M42;
				cjcwpV3T[threadID].v[2] = v1.v[2] - matrix.M43;
				float num = cjcwpV2T[threadID].v[0] * cjcwpV3T[threadID].v[0] + cjcwpV2T[threadID].v[1] * cjcwpV3T[threadID].v[1] + cjcwpV2T[threadID].v[2] * cjcwpV3T[threadID].v[2];
				if (num >= 0f && num < global::Players.Players.players[pID].jt1[b].lenSquared)
				{
					num /= global::Players.Players.players[pID].jt1[b].len;
					float num2 = num * position.X / global::Players.Players.players[pID].jt1[b].len;
					float num3 = num * position.Y / global::Players.Players.players[pID].jt1[b].len;
					float num4 = num * position.Z / global::Players.Players.players[pID].jt1[b].len;
					cjcwpV2T[threadID].v[0] = cjcwpV3T[threadID].v[0] - num2;
					cjcwpV2T[threadID].v[1] = cjcwpV3T[threadID].v[1] - num3;
					cjcwpV2T[threadID].v[2] = cjcwpV3T[threadID].v[2] - num4;
					float num5 = (float)Math.Sqrt(cjcwpV2T[threadID].v[0] * cjcwpV2T[threadID].v[0] + cjcwpV2T[threadID].v[1] * cjcwpV2T[threadID].v[1] + cjcwpV2T[threadID].v[2] * cjcwpV2T[threadID].v[2]) - bulletRadius;
					if (num5 < global::Players.Players.players[pID].jt1[b].radius)
					{
						global::Players.Players.players[pID].jointWasShot = (sbyte)b;
						global::Players.Players.players[pID].jColT[threadID, b] = num;
						global::Players.Players.players[pID].jVect1T[threadID, b].v[0] = matrix.M31;
						global::Players.Players.players[pID].jVect1T[threadID, b].v[1] = matrix.M32;
						global::Players.Players.players[pID].jVect1T[threadID, b].v[2] = matrix.M33;
						global::Players.Players.players[pID].jVect2T[threadID, b].v[0] = cjcwpV2T[threadID].v[0];
						global::Players.Players.players[pID].jVect2T[threadID, b].v[1] = cjcwpV2T[threadID].v[1];
						global::Players.Players.players[pID].jVect2T[threadID, b].v[2] = cjcwpV2T[threadID].v[2];
						global::Players.Players.players[pID].jVect3T[threadID, b].v[0] = matrix.M21;
						global::Players.Players.players[pID].jVect3T[threadID, b].v[1] = matrix.M22;
						global::Players.Players.players[pID].jVect3T[threadID, b].v[2] = matrix.M23;
						return 1;
					}
				}
			}
		}
		return 0;
	}

	public void Reset_Joint_Collision_Check_Threaded(long pID, byte threadID)
	{
		int numJoints = playerJoints[global::Players.Players.players[pID].jointPackage].numJoints;
		for (int i = 0; i < numJoints; i++)
		{
			global::Players.Players.players[pID].jColT[threadID, i] = -1f;
			global::Players.Players.players[pID].jVect1T[threadID, i].v[0] = 0f;
			global::Players.Players.players[pID].jVect1T[threadID, i].v[1] = 0f;
			global::Players.Players.players[pID].jVect1T[threadID, i].v[2] = 0f;
			global::Players.Players.players[pID].jVect2T[threadID, i].v[0] = 0f;
			global::Players.Players.players[pID].jVect2T[threadID, i].v[1] = 0f;
			global::Players.Players.players[pID].jVect2T[threadID, i].v[2] = 0f;
		}
	}
}
