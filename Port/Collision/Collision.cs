using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using GameObjects;
using MainGame;
using Microsoft.Xna.Framework;
using Models;
using Rendering;
using Structs;
using WindowsGame1;

namespace Collision;

public class Collision
{
	public static bool hitGround = false;

	public static byte[,] boxItemIndex;

	public static int[,] cBoxList;

	public static ushort numAllocatedCollisionModels = 0;

	public static ulong maxPositions;

	public static float BoxX;

	public static float BoxY;

	public static float BoxZ;

	public static float widthX;

	public static float widthY;

	public static float widthZ;

	public static float maxDistanceSqr = 1f;

	public static int curCollisionModelExportVertice;

	public static int curCollisionModelExportNormal;

	public static int numAllocatedCollisionBoxList;

	public static int numBoxes;

	public static int numAllocatedBoxes;

	public static int BoxSize;

	public static int BoxDimX;

	public static int BoxDimY;

	public static int BoxDimZ;

	public static long[] colX = new long[5];

	public static long[] colY = new long[5];

	public static long[] colZ = new long[5];

	public static long[] colIDT = new long[5];

	public static float[] colXPos = new float[5];

	public static float[] colYPos = new float[5];

	public static float[] colZPos = new float[5];

	public static float[] ccsVxT = new float[5];

	public static float[] ccsVyT = new float[5];

	public static float[] ccsVzT = new float[5];

	public static float[] adjX = new float[5];

	public static float[] adjY = new float[5];

	public static float[] adjZ = new float[5];

	public static StructsClass.particle_list thirdPersonView = default(StructsClass.particle_list);

	public static StructsClass.particle_list[] colPListT = new StructsClass.particle_list[5];

	public static StructsClass.boxList[] mainBox;

	public static StructsClass.gameobject[] colPtrT = new StructsClass.gameobject[5];

	public static StructsClass.CollisionModel[] cModels;

	public static int[] hitx = new int[9];

	public static int[] hity = new int[9];

	public static int[] hitz = new int[9];

	public static long[] boxList = new long[8];

	public static long[] boxItems0 = new long[100];

	public static long[] boxItems1 = new long[100];

	public static long[] boxItems2 = new long[100];

	public static long[] boxItems3 = new long[100];

	public static long[] boxItems0A = new long[100];

	public static long[] boxItems1A = new long[100];

	public static long[] boxItems2A = new long[100];

	public static long[] boxItems3A = new long[100];

	private static StructsClass.vtex[,] ccBB = new StructsClass.vtex[5, 8];

	private static StructsClass.particle_list[] ccP3 = new StructsClass.particle_list[5];

	private static StructsClass.vtex[,] cbprVp;

	private static StructsClass.vtex[,] cbprVxyz;

	private static StructsClass.vtex[,] cbprVxyz2;

	public static byte[,] cbprSide;

	public static byte[,] cbprLastSide;

	public static int cbprVpCnt = 100;

	public static int maxItemsInABox = 0;

	public static float[,] cbprCntPtr;

	public static long[,,] floatArIgnore = new long[5, 43, 15];

	public static int[,] floatArID = new int[5, 43];

	public static byte[,] floatArStatus = new byte[5, 43];

	public static byte[,] floatArCnt = new byte[5, 43];

	public static float[,] floatAr = new float[5, 258];

	public static float[,] floatArMovDir = new float[5, 1];

	public static float[,,] floatArDir = new float[5, 43, 7];

	public static Stopwatch[] ccdSw = new Stopwatch[5];

	public static StreamWriter fp;

	public Game1.MasterCollection mainC;

	public void Init_Vars(Game1.MasterCollection masterC)
	{
		numAllocatedCollisionBoxList = 0;
		mainC = masterC;
		StructsClass.Initialize_ParticleList(ref thirdPersonView);
		for (int i = 0; i < 5; i++)
		{
			for (int j = 0; j < 8; j++)
			{
				ccBB[i, j] = new StructsClass.vtex();
			}
		}
		cbprVp = new StructsClass.vtex[4, cbprVpCnt];
		cbprVxyz = new StructsClass.vtex[4, cbprVpCnt];
		cbprVxyz2 = new StructsClass.vtex[4, cbprVpCnt];
		for (int i = 0; i < cbprVpCnt; i++)
		{
			cbprVp[0, i] = new StructsClass.vtex();
			cbprVp[1, i] = new StructsClass.vtex();
			cbprVp[2, i] = new StructsClass.vtex();
			cbprVp[3, i] = new StructsClass.vtex();
			cbprVxyz[0, i] = new StructsClass.vtex();
			cbprVxyz[1, i] = new StructsClass.vtex();
			cbprVxyz[2, i] = new StructsClass.vtex();
			cbprVxyz[3, i] = new StructsClass.vtex();
			cbprVxyz2[0, i] = new StructsClass.vtex();
			cbprVxyz2[1, i] = new StructsClass.vtex();
			cbprVxyz2[2, i] = new StructsClass.vtex();
			cbprVxyz2[3, i] = new StructsClass.vtex();
		}
		cbprCntPtr = new float[4, cbprVpCnt];
		cbprSide = new byte[4, cbprVpCnt];
		cbprLastSide = new byte[4, cbprVpCnt];
		for (int i = 0; i < 5; i++)
		{
			colPListT[i] = default(StructsClass.particle_list);
			StructsClass.Initialize_ParticleList(ref colPListT[i]);
			colPListT[i].v1 = new StructsClass.vtex[1];
			colPListT[i].v1[0] = new StructsClass.vtex();
			colPListT[i].numP = 1L;
			colPListT[i].numUsed = 0L;
			ccdSw[i] = new Stopwatch();
		}
	}

	public float CheckJointCollisionSingle_Threaded(long pID, ref StructsClass.particle_list p1, ref StructsClass.vtex b1, ref StructsClass.vtex b2, float Vx, float Vy, float Vz, float len, float bulletRadius, byte threadID)
	{
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		float num4 = 0f;
		float maxDown = global::MainGame.MainGame.MaxDown;
		if ((Vx > 0f && p1.pos1.v[0] > b2.v[0]) || (Vx < 0f && p1.pos1.v[0] < b1.v[0]) || (Vy > 0f && p1.pos1.v[1] > b2.v[1]) || (Vy < 0f && p1.pos1.v[1] < b1.v[1]) || (Vz > 0f && p1.pos1.v[2] > b2.v[2]) || (Vz < 0f && p1.pos1.v[2] < b1.v[2]) || p1.pos1.v[2] < maxDown)
		{
			return -1f;
		}
		if (Vx > 0f)
		{
			if (p1.pos1.v[0] < b1.v[0])
			{
				num = (int)Math.Ceiling((b1.v[0] - p1.pos1.v[0]) / Vx);
			}
		}
		else if (Vx < 0f && p1.pos1.v[0] > b2.v[0])
		{
			num = (int)Math.Ceiling((b2.v[0] - p1.pos1.v[0]) / Vx);
		}
		p1.pos1.v[0] += Vx * (float)num;
		p1.pos1.v[1] += Vy * (float)num;
		p1.pos1.v[2] += Vz * (float)num;
		num4 += (float)num;
		if (Vy > 0f)
		{
			if (p1.pos1.v[1] < b1.v[1])
			{
				num2 = (int)Math.Ceiling((b1.v[1] - p1.pos1.v[1]) / Vy);
			}
		}
		else if (Vy < 0f && p1.pos1.v[1] > b2.v[1])
		{
			num2 = (int)Math.Ceiling((b2.v[1] - p1.pos1.v[1]) / Vy);
		}
		p1.pos1.v[0] += Vx * (float)num2;
		p1.pos1.v[1] += Vy * (float)num2;
		p1.pos1.v[2] += Vz * (float)num2;
		num4 += (float)num2;
		if (Vz > 0f)
		{
			if (p1.pos1.v[0] < b1.v[0])
			{
				num3 = (int)Math.Ceiling((b1.v[2] - p1.pos1.v[2]) / Vz);
			}
		}
		else if (Vz < 0f && p1.pos1.v[0] > b2.v[0])
		{
			num3 = (int)Math.Ceiling((b2.v[2] - p1.pos1.v[2]) / Vz);
		}
		p1.pos1.v[0] += Vx * (float)num3;
		p1.pos1.v[1] += Vy * (float)num3;
		p1.pos1.v[2] += Vz * (float)num3;
		num4 += (float)num3;
		maxDown = global::MainGame.MainGame.MaxDown;
		for (; num4 < len; num4 += 1f)
		{
			if (p1.pos1.v[2] < maxDown || p1.pos1.v[0] < b1.v[0] || p1.pos1.v[0] > b2.v[0] || p1.pos1.v[1] < b1.v[1] || p1.pos1.v[1] > b2.v[1] || p1.pos1.v[2] < b1.v[2] || p1.pos1.v[2] > b2.v[2])
			{
				return -1f;
			}
			if (mainC.jointsMain.Check_Joint_Collision_With_Point_Threaded(pID, ref p1.pos1, bulletRadius, threadID) != 0)
			{
				return num4;
			}
			p1.pos1.v[0] += Vx;
			p1.pos1.v[1] += Vy;
			p1.pos1.v[2] += Vz;
		}
		return -1f;
	}

	public int CheckCollsion(ref StructsClass.particle_list p1, bool altPaths, long objID, bool adjustParticles, byte threadID)
	{
		int num = 0;
		float len = 0f;
		float Vx = 0f;
		float Vy = 0f;
		float Vz = 0f;
		long num2 = 0L;
		colIDT[threadID] = -1L;
		Calculate_Length(ref p1, ref Vx, ref Vy, ref Vz, ref len);
		if (adjustParticles)
		{
			if (colPListT[threadID].numP < p1.numUsed)
			{
				colPListT[threadID].numP = 0L;
				if ((colPListT[threadID].v1 = new StructsClass.vtex[p1.numUsed]) == null)
				{
					return 7;
				}
				colPListT[threadID].numP = p1.numUsed;
				for (int i = 0; i < p1.numUsed; i++)
				{
					colPListT[threadID].v1[i] = new StructsClass.vtex();
				}
			}
			colPListT[threadID].numUsed = p1.numUsed;
			Matrix matrix = Matrix.CreateRotationX(adjX[threadID] * ((float)Math.PI / 180f)) * Matrix.CreateRotationY(adjY[threadID] * ((float)Math.PI / 180f)) * Matrix.CreateRotationZ(adjZ[threadID] * ((float)Math.PI / 180f));
			if (p1.bbDirty)
			{
				p1.b1.v[0] = p1.v1[0].v[0];
				p1.b1.v[1] = p1.v1[0].v[1];
				p1.b1.v[2] = p1.v1[0].v[2];
				p1.b2.v[0] = p1.v1[0].v[0];
				p1.b2.v[1] = p1.v1[0].v[1];
				p1.b2.v[2] = p1.v1[0].v[2];
				for (int i = 0; i < p1.numUsed; i++)
				{
					if (p1.v1[i].v[0] < p1.b1.v[0])
					{
						p1.b1.v[0] = p1.v1[i].v[0];
					}
					if (p1.v1[i].v[0] > p1.b2.v[0])
					{
						p1.b2.v[0] = p1.v1[i].v[0];
					}
					if (p1.v1[i].v[1] < p1.b1.v[1])
					{
						p1.b1.v[1] = p1.v1[i].v[1];
					}
					if (p1.v1[i].v[1] > p1.b2.v[1])
					{
						p1.b2.v[1] = p1.v1[i].v[1];
					}
					if (p1.v1[i].v[2] < p1.b1.v[2])
					{
						p1.b1.v[2] = p1.v1[i].v[2];
					}
					if (p1.v1[i].v[2] > p1.b2.v[2])
					{
						p1.b2.v[2] = p1.v1[i].v[2];
					}
				}
				p1.bbDirty = false;
			}
			ccBB[threadID, 0].v[0] = p1.b1.v[0] * matrix.M11 + p1.b1.v[1] * matrix.M21 + p1.b1.v[2] * matrix.M31;
			ccBB[threadID, 0].v[1] = p1.b1.v[0] * matrix.M12 + p1.b1.v[1] * matrix.M22 + p1.b1.v[2] * matrix.M32;
			ccBB[threadID, 0].v[2] = p1.b1.v[0] * matrix.M13 + p1.b1.v[1] * matrix.M23 + p1.b1.v[2] * matrix.M33;
			ccBB[threadID, 1].v[0] = p1.b2.v[0] * matrix.M11 + p1.b1.v[1] * matrix.M21 + p1.b1.v[2] * matrix.M31;
			ccBB[threadID, 1].v[1] = p1.b2.v[0] * matrix.M12 + p1.b1.v[1] * matrix.M22 + p1.b1.v[2] * matrix.M32;
			ccBB[threadID, 1].v[2] = p1.b2.v[0] * matrix.M13 + p1.b1.v[1] * matrix.M23 + p1.b1.v[2] * matrix.M33;
			ccBB[threadID, 2].v[0] = p1.b1.v[0] * matrix.M11 + p1.b2.v[1] * matrix.M21 + p1.b1.v[2] * matrix.M31;
			ccBB[threadID, 2].v[1] = p1.b1.v[0] * matrix.M12 + p1.b2.v[1] * matrix.M22 + p1.b1.v[2] * matrix.M32;
			ccBB[threadID, 2].v[2] = p1.b1.v[0] * matrix.M13 + p1.b2.v[1] * matrix.M23 + p1.b1.v[2] * matrix.M33;
			ccBB[threadID, 3].v[0] = p1.b2.v[0] * matrix.M11 + p1.b2.v[1] * matrix.M21 + p1.b1.v[2] * matrix.M31;
			ccBB[threadID, 3].v[1] = p1.b2.v[0] * matrix.M12 + p1.b2.v[1] * matrix.M22 + p1.b1.v[2] * matrix.M32;
			ccBB[threadID, 3].v[2] = p1.b2.v[0] * matrix.M13 + p1.b2.v[1] * matrix.M23 + p1.b1.v[2] * matrix.M33;
			ccBB[threadID, 4].v[0] = p1.b1.v[0] * matrix.M11 + p1.b1.v[1] * matrix.M21 + p1.b2.v[2] * matrix.M31;
			ccBB[threadID, 4].v[1] = p1.b1.v[0] * matrix.M12 + p1.b1.v[1] * matrix.M22 + p1.b2.v[2] * matrix.M32;
			ccBB[threadID, 4].v[2] = p1.b1.v[0] * matrix.M13 + p1.b1.v[1] * matrix.M23 + p1.b2.v[2] * matrix.M33;
			ccBB[threadID, 5].v[0] = p1.b2.v[0] * matrix.M11 + p1.b1.v[1] * matrix.M21 + p1.b2.v[2] * matrix.M31;
			ccBB[threadID, 5].v[1] = p1.b2.v[0] * matrix.M12 + p1.b1.v[1] * matrix.M22 + p1.b2.v[2] * matrix.M32;
			ccBB[threadID, 5].v[2] = p1.b2.v[0] * matrix.M13 + p1.b1.v[1] * matrix.M23 + p1.b2.v[2] * matrix.M33;
			ccBB[threadID, 6].v[0] = p1.b1.v[0] * matrix.M11 + p1.b2.v[1] * matrix.M21 + p1.b2.v[2] * matrix.M31;
			ccBB[threadID, 6].v[1] = p1.b1.v[0] * matrix.M12 + p1.b2.v[1] * matrix.M22 + p1.b2.v[2] * matrix.M32;
			ccBB[threadID, 6].v[2] = p1.b1.v[0] * matrix.M13 + p1.b2.v[1] * matrix.M23 + p1.b2.v[2] * matrix.M33;
			ccBB[threadID, 7].v[0] = p1.b2.v[0] * matrix.M11 + p1.b2.v[1] * matrix.M21 + p1.b2.v[2] * matrix.M31;
			ccBB[threadID, 7].v[1] = p1.b2.v[0] * matrix.M12 + p1.b2.v[1] * matrix.M22 + p1.b2.v[2] * matrix.M32;
			ccBB[threadID, 7].v[2] = p1.b2.v[0] * matrix.M13 + p1.b2.v[1] * matrix.M23 + p1.b2.v[2] * matrix.M33;
			for (int i = 0; i < p1.numUsed; i++)
			{
				colPListT[threadID].v1[i].v[0] = p1.v1[i].v[0] * matrix.M11 + p1.v1[i].v[1] * matrix.M21 + p1.v1[i].v[2] * matrix.M31;
				colPListT[threadID].v1[i].v[1] = p1.v1[i].v[0] * matrix.M12 + p1.v1[i].v[1] * matrix.M22 + p1.v1[i].v[2] * matrix.M32;
				colPListT[threadID].v1[i].v[2] = p1.v1[i].v[0] * matrix.M13 + p1.v1[i].v[1] * matrix.M23 + p1.v1[i].v[2] * matrix.M33;
			}
			ccP3[threadID].v1 = colPListT[threadID].v1;
		}
		else
		{
			if (p1.bbDirty)
			{
				p1.b1.v[0] = p1.v1[0].v[0];
				p1.b1.v[1] = p1.v1[0].v[1];
				p1.b1.v[2] = p1.v1[0].v[2];
				p1.b2.v[0] = p1.v1[0].v[0];
				p1.b2.v[1] = p1.v1[0].v[1];
				p1.b2.v[2] = p1.v1[0].v[2];
				for (int i = 0; i < p1.numUsed; i++)
				{
					if (p1.v1[i].v[0] < p1.b1.v[0])
					{
						p1.b1.v[0] = p1.v1[i].v[0];
					}
					if (p1.v1[i].v[0] > p1.b2.v[0])
					{
						p1.b2.v[0] = p1.v1[i].v[0];
					}
					if (p1.v1[i].v[1] < p1.b1.v[1])
					{
						p1.b1.v[1] = p1.v1[i].v[1];
					}
					if (p1.v1[i].v[1] > p1.b2.v[1])
					{
						p1.b2.v[1] = p1.v1[i].v[1];
					}
					if (p1.v1[i].v[2] < p1.b1.v[2])
					{
						p1.b1.v[2] = p1.v1[i].v[2];
					}
					if (p1.v1[i].v[2] > p1.b2.v[2])
					{
						p1.b2.v[2] = p1.v1[i].v[2];
					}
				}
				p1.bbDirty = false;
			}
			ccBB[threadID, 0].v[0] = p1.b1.v[0];
			ccBB[threadID, 0].v[1] = p1.b1.v[1];
			ccBB[threadID, 0].v[2] = p1.b1.v[2];
			ccBB[threadID, 1].v[0] = p1.b2.v[0];
			ccBB[threadID, 1].v[1] = p1.b1.v[1];
			ccBB[threadID, 1].v[2] = p1.b1.v[2];
			ccBB[threadID, 2].v[0] = p1.b1.v[0];
			ccBB[threadID, 2].v[1] = p1.b2.v[1];
			ccBB[threadID, 2].v[2] = p1.b1.v[2];
			ccBB[threadID, 3].v[0] = p1.b2.v[0];
			ccBB[threadID, 3].v[1] = p1.b2.v[1];
			ccBB[threadID, 3].v[2] = p1.b1.v[2];
			ccBB[threadID, 4].v[0] = p1.b1.v[0];
			ccBB[threadID, 4].v[1] = p1.b1.v[1];
			ccBB[threadID, 4].v[2] = p1.b2.v[2];
			ccBB[threadID, 5].v[0] = p1.b2.v[0];
			ccBB[threadID, 5].v[1] = p1.b1.v[1];
			ccBB[threadID, 5].v[2] = p1.b2.v[2];
			ccBB[threadID, 6].v[0] = p1.b1.v[0];
			ccBB[threadID, 6].v[1] = p1.b2.v[1];
			ccBB[threadID, 6].v[2] = p1.b2.v[2];
			ccBB[threadID, 7].v[0] = p1.b2.v[0];
			ccBB[threadID, 7].v[1] = p1.b2.v[1];
			ccBB[threadID, 7].v[2] = p1.b2.v[2];
			ccP3[threadID].v1 = p1.v1;
		}
		ccP3[threadID].numP = p1.numP;
		ccP3[threadID].numUsed = p1.numUsed;
		float num6;
		float num5;
		float num4;
		for (float num3 = 0f; num3 < len; num3 += 1f)
		{
			if ((num & 1) == 0)
			{
				p1.pos1.v[0] += Vx;
			}
			if ((num & 2) == 0)
			{
				p1.pos1.v[1] += Vy;
			}
			if ((num & 4) == 0)
			{
				p1.pos1.v[2] += Vz;
			}
			num4 = global::MainGame.MainGame.MaxDown - p1.pos1.v[2];
			for (int i = 0; i < p1.numUsed; i++)
			{
				if (!(ccP3[threadID].v1[i].v[2] < num4))
				{
					continue;
				}
				if (altPaths)
				{
					p1.pos1.v[2] -= Vz;
					num |= 4;
					if ((Vx == 0f || (num & 1) != 0) && (Vy == 0f || (num & 2) != 0))
					{
						p1.pos2.v[0] = p1.pos1.v[0];
						p1.pos2.v[1] = p1.pos1.v[1];
						p1.pos2.v[2] = p1.pos1.v[2];
						return num;
					}
					if ((num & 1) == 0)
					{
						p1.pos1.v[0] -= Vx;
					}
					if ((num & 2) == 0)
					{
						p1.pos1.v[1] -= Vy;
					}
					p1.pos2.v[2] = p1.pos1.v[2];
					Calculate_Length(ref p1, ref Vx, ref Vy, ref Vz, ref len);
					num3 = 0f;
					if ((num & 1) == 0)
					{
						p1.pos1.v[0] += Vx;
					}
					if ((num & 2) == 0)
					{
						p1.pos1.v[1] += Vy;
					}
					break;
				}
				num |= 0xC;
				if ((num & 1) == 0)
				{
					p1.pos1.v[0] -= Vx;
				}
				if ((num & 2) == 0)
				{
					p1.pos1.v[1] -= Vy;
				}
				p1.pos1.v[2] -= Vz;
				p1.pos2.v[0] = p1.pos1.v[0];
				p1.pos2.v[1] = p1.pos1.v[1];
				p1.pos2.v[2] = p1.pos1.v[2];
				return num;
			}
			num4 = p1.pos1.v[0];
			num5 = p1.pos1.v[1];
			num6 = p1.pos1.v[2];
			if (CheckBoxParticle(num4, num5, num6, ref ccP3[threadID], ref ccBB, objID, threadID) != 0)
			{
				if (!altPaths)
				{
					num |= 8;
					if ((num & 1) == 0)
					{
						p1.pos1.v[0] -= Vx;
					}
					if ((num & 2) == 0)
					{
						p1.pos1.v[1] -= Vy;
					}
					if ((num & 4) == 0)
					{
						p1.pos1.v[2] -= Vz;
					}
					p1.pos2.v[0] = p1.pos1.v[0];
					p1.pos2.v[1] = p1.pos1.v[1];
					p1.pos2.v[2] = p1.pos1.v[2];
					return num;
				}
				int num7 = 1;
				if ((num & 1) == 0)
				{
					num4 -= Vx;
					if ((num7 = CheckBoxParticle(num4, num5, num6, ref ccP3[threadID], ref ccBB, objID, threadID)) == 0)
					{
						p1.pos1.v[0] -= Vx;
						num |= 1;
						p1.pos2.v[0] = p1.pos1.v[0];
						Calculate_Length(ref p1, ref Vx, ref Vy, ref Vz, ref len);
						num3 = 0f;
					}
					else
					{
						num4 += Vx;
					}
				}
				if (num7 == 1 && (num & 2) == 0)
				{
					num5 -= Vy;
					if ((num7 = CheckBoxParticle(num4, num5, num6, ref ccP3[threadID], ref ccBB, objID, threadID)) == 0)
					{
						p1.pos1.v[1] -= Vy;
						num |= 2;
						p1.pos2.v[1] = p1.pos1.v[1];
						Calculate_Length(ref p1, ref Vx, ref Vy, ref Vz, ref len);
						num3 = 0f;
					}
					else
					{
						num5 += Vy;
					}
				}
				if (num7 == 1 && (num & 4) == 0)
				{
					num6 -= Vz;
					if ((num7 = CheckBoxParticle(num4, num5, num6, ref ccP3[threadID], ref ccBB, objID, threadID)) == 0)
					{
						p1.pos1.v[2] -= Vz;
						num |= 4;
						p1.pos2.v[2] = p1.pos1.v[2];
						Calculate_Length(ref p1, ref Vx, ref Vy, ref Vz, ref len);
						num3 = 0f;
					}
					else
					{
						num6 += Vz;
					}
				}
				if (num == 0 && num7 == 1)
				{
					num4 -= Vx;
					num5 -= Vy;
					if ((num7 = CheckBoxParticle(num4, num5, num6, ref ccP3[threadID], ref ccBB, objID, threadID)) == 0)
					{
						p1.pos1.v[0] -= Vx;
						p1.pos1.v[1] -= Vy;
						num |= 3;
						p1.pos2.v[0] = p1.pos1.v[0];
						p1.pos2.v[1] = p1.pos1.v[1];
						Calculate_Length(ref p1, ref Vx, ref Vy, ref Vz, ref len);
						num3 = 0f;
					}
					else
					{
						num4 += Vx;
						num6 -= Vz;
						if ((num7 = CheckBoxParticle(num4, num5, num6, ref ccP3[threadID], ref ccBB, objID, threadID)) == 0)
						{
							p1.pos1.v[1] -= Vy;
							p1.pos1.v[2] -= Vz;
							num |= 6;
							p1.pos2.v[1] = p1.pos1.v[1];
							p1.pos2.v[2] = p1.pos1.v[2];
							Calculate_Length(ref p1, ref Vx, ref Vy, ref Vz, ref len);
							num3 = 0f;
						}
						else
						{
							num5 += Vy;
							num4 -= Vx;
							if ((num7 = CheckBoxParticle(num4, num5, num6, ref ccP3[threadID], ref ccBB, objID, threadID)) == 0)
							{
								p1.pos1.v[0] -= Vx;
								p1.pos1.v[2] -= Vz;
								num |= 5;
								p1.pos2.v[0] = p1.pos1.v[0];
								p1.pos2.v[2] = p1.pos1.v[2];
								Calculate_Length(ref p1, ref Vx, ref Vy, ref Vz, ref len);
								num3 = 0f;
							}
						}
					}
				}
				if (num7 == 1)
				{
					num |= 8;
					if ((num & 1) == 0)
					{
						p1.pos1.v[0] -= Vx;
					}
					if ((num & 2) == 0)
					{
						p1.pos1.v[1] -= Vy;
					}
					if ((num & 4) == 0)
					{
						p1.pos1.v[2] -= Vz;
					}
					p1.pos2.v[0] = p1.pos1.v[0];
					p1.pos2.v[1] = p1.pos1.v[1];
					p1.pos2.v[2] = p1.pos1.v[2];
					return num;
				}
				if (((num & 1) != 0 || Vx == 0f) && ((num & 2) != 0 || Vy == 0f) && ((num & 4) != 0 || Vz == 0f))
				{
					num |= 8;
					p1.pos2.v[0] = p1.pos1.v[0];
					p1.pos2.v[1] = p1.pos1.v[1];
					p1.pos2.v[2] = p1.pos1.v[2];
					return num;
				}
			}
			num2++;
			if (num2 > 2000)
			{
				num3 = len + 10f;
			}
		}
		if ((num & 1) != 0)
		{
			p1.pos2.v[0] = p1.pos1.v[0];
		}
		if ((num & 2) != 0)
		{
			p1.pos2.v[1] = p1.pos1.v[1];
		}
		if ((num & 4) != 0)
		{
			p1.pos2.v[2] = p1.pos1.v[2];
		}
		num4 = global::MainGame.MainGame.MaxDown - p1.pos2.v[2];
		for (int i = 0; i < p1.numUsed; i++)
		{
			if (ccP3[threadID].v1[i].v[2] < num4)
			{
				if (altPaths)
				{
					p1.pos2.v[2] = p1.pos1.v[2];
					num |= 4;
					break;
				}
				num |= 0xC;
				p1.pos2.v[0] = p1.pos1.v[0];
				p1.pos2.v[1] = p1.pos1.v[1];
				p1.pos2.v[2] = (p1.pos1.v[2] = 0f);
				return num;
			}
		}
		num4 = p1.pos2.v[0];
		num5 = p1.pos2.v[1];
		num6 = p1.pos2.v[2];
		if (CheckBoxParticle(num4, num5, num6, ref ccP3[threadID], ref ccBB, objID, threadID) != 0)
		{
			if (!altPaths)
			{
				num |= 8;
				p1.pos2.v[0] = p1.pos1.v[0];
				p1.pos2.v[1] = p1.pos1.v[1];
				p1.pos2.v[2] = p1.pos1.v[2];
				return num;
			}
			int num7 = 1;
			if ((num & 1) == 0)
			{
				num4 = p1.pos1.v[0];
				if ((num7 = CheckBoxParticle(num4, num5, num6, ref ccP3[threadID], ref ccBB, objID, threadID)) == 0)
				{
					p1.pos2.v[0] = p1.pos1.v[0];
					num |= 1;
				}
				num4 = p1.pos2.v[0];
			}
			if (num7 == 1 && (num & 2) == 0)
			{
				num5 = p1.pos1.v[1];
				if ((num7 = CheckBoxParticle(num4, num5, num6, ref ccP3[threadID], ref ccBB, objID, threadID)) == 0)
				{
					p1.pos2.v[1] = p1.pos1.v[1];
					num |= 2;
				}
				num5 = p1.pos2.v[1];
			}
			if (num7 == 1 && (num & 4) == 0)
			{
				num6 = p1.pos1.v[2];
				if ((num7 = CheckBoxParticle(num4, num5, num6, ref ccP3[threadID], ref ccBB, objID, threadID)) == 0)
				{
					p1.pos2.v[2] = p1.pos1.v[2];
					num |= 4;
				}
				num6 = p1.pos2.v[2];
			}
			if (num == 0 && num7 == 1)
			{
				num4 = p1.pos1.v[0];
				num5 = p1.pos1.v[1];
				if ((num7 = CheckBoxParticle(num4, num5, num6, ref ccP3[threadID], ref ccBB, objID, threadID)) == 0)
				{
					p1.pos2.v[0] = p1.pos1.v[0];
					p1.pos2.v[1] = p1.pos1.v[1];
					num |= 3;
				}
				else
				{
					num4 = p1.pos2.v[0];
					num6 = p1.pos1.v[2];
					if ((num7 = CheckBoxParticle(num4, num5, num6, ref ccP3[threadID], ref ccBB, objID, threadID)) == 0)
					{
						p1.pos2.v[1] = p1.pos1.v[1];
						p1.pos2.v[2] = p1.pos1.v[2];
						num |= 6;
					}
					else
					{
						num5 = p1.pos2.v[2];
						num4 = p1.pos1.v[0];
						if ((num7 = CheckBoxParticle(num4, num5, num6, ref ccP3[threadID], ref ccBB, objID, threadID)) == 0)
						{
							p1.pos2.v[0] = p1.pos1.v[0];
							p1.pos2.v[2] = p1.pos1.v[2];
							num |= 5;
						}
					}
				}
			}
			if (num7 == 1)
			{
				num |= 8;
				p1.pos2.v[0] = p1.pos1.v[0];
				p1.pos2.v[1] = p1.pos1.v[1];
				p1.pos2.v[2] = p1.pos1.v[2];
				return num;
			}
		}
		return num;
	}

	public int CheckCollsion_Byte(ref StructsClass.particle_list_byte p1, bool altPaths, long objID, bool adjustParticles, byte threadID)
	{
		int num = 0;
		float len = 0f;
		float Vx = 0f;
		float Vy = 0f;
		float Vz = 0f;
		long num2 = 0L;
		colIDT[threadID] = -1L;
		Calculate_Length_Byte(ref p1, ref Vx, ref Vy, ref Vz, ref len);
		if (colPListT[threadID].numP < p1.numUsed)
		{
			colPListT[threadID].numP = 0L;
			if ((colPListT[threadID].v1 = new StructsClass.vtex[p1.numUsed]) == null)
			{
				return 7;
			}
			colPListT[threadID].numP = p1.numUsed;
			for (int i = 0; i < p1.numUsed; i++)
			{
				colPListT[threadID].v1[i] = new StructsClass.vtex();
			}
		}
		colPListT[threadID].numUsed = p1.numUsed;
		if (adjustParticles)
		{
			Matrix matrix = Matrix.CreateRotationX(adjX[threadID] * ((float)Math.PI / 180f)) * Matrix.CreateRotationY(adjY[threadID] * ((float)Math.PI / 180f)) * Matrix.CreateRotationZ(adjZ[threadID] * ((float)Math.PI / 180f));
			if (p1.bbDirty)
			{
				p1.b1.v[0] = (int)p1.v1[0].vx;
				p1.b1.v[1] = (int)p1.v1[0].vy;
				p1.b1.v[2] = (int)p1.v1[0].vz;
				p1.b2.v[0] = (int)p1.v1[0].vx;
				p1.b2.v[1] = (int)p1.v1[0].vy;
				p1.b2.v[2] = (int)p1.v1[0].vz;
				for (int i = 0; i < p1.numUsed; i++)
				{
					if ((float)(int)p1.v1[i].vx < p1.b1.v[0])
					{
						p1.b1.v[0] = (int)p1.v1[i].vx;
					}
					if ((float)(int)p1.v1[i].vx > p1.b2.v[0])
					{
						p1.b2.v[0] = (int)p1.v1[i].vx;
					}
					if ((float)(int)p1.v1[i].vy < p1.b1.v[1])
					{
						p1.b1.v[1] = (int)p1.v1[i].vy;
					}
					if ((float)(int)p1.v1[i].vy > p1.b2.v[1])
					{
						p1.b2.v[1] = (int)p1.v1[i].vy;
					}
					if ((float)(int)p1.v1[i].vz < p1.b1.v[2])
					{
						p1.b1.v[2] = (int)p1.v1[i].vz;
					}
					if ((float)(int)p1.v1[i].vz > p1.b2.v[2])
					{
						p1.b2.v[2] = (int)p1.v1[i].vz;
					}
				}
				p1.bbDirty = false;
			}
			ccBB[threadID, 0].v[0] = p1.b1.v[0] * matrix.M11 + p1.b1.v[1] * matrix.M21 + p1.b1.v[2] * matrix.M31;
			ccBB[threadID, 0].v[1] = p1.b1.v[0] * matrix.M12 + p1.b1.v[1] * matrix.M22 + p1.b1.v[2] * matrix.M32;
			ccBB[threadID, 0].v[2] = p1.b1.v[0] * matrix.M13 + p1.b1.v[1] * matrix.M23 + p1.b1.v[2] * matrix.M33;
			ccBB[threadID, 1].v[0] = p1.b2.v[0] * matrix.M11 + p1.b1.v[1] * matrix.M21 + p1.b1.v[2] * matrix.M31;
			ccBB[threadID, 1].v[1] = p1.b2.v[0] * matrix.M12 + p1.b1.v[1] * matrix.M22 + p1.b1.v[2] * matrix.M32;
			ccBB[threadID, 1].v[2] = p1.b2.v[0] * matrix.M13 + p1.b1.v[1] * matrix.M23 + p1.b1.v[2] * matrix.M33;
			ccBB[threadID, 2].v[0] = p1.b1.v[0] * matrix.M11 + p1.b2.v[1] * matrix.M21 + p1.b1.v[2] * matrix.M31;
			ccBB[threadID, 2].v[1] = p1.b1.v[0] * matrix.M12 + p1.b2.v[1] * matrix.M22 + p1.b1.v[2] * matrix.M32;
			ccBB[threadID, 2].v[2] = p1.b1.v[0] * matrix.M13 + p1.b2.v[1] * matrix.M23 + p1.b1.v[2] * matrix.M33;
			ccBB[threadID, 3].v[0] = p1.b2.v[0] * matrix.M11 + p1.b2.v[1] * matrix.M21 + p1.b1.v[2] * matrix.M31;
			ccBB[threadID, 3].v[1] = p1.b2.v[0] * matrix.M12 + p1.b2.v[1] * matrix.M22 + p1.b1.v[2] * matrix.M32;
			ccBB[threadID, 3].v[2] = p1.b2.v[0] * matrix.M13 + p1.b2.v[1] * matrix.M23 + p1.b1.v[2] * matrix.M33;
			ccBB[threadID, 4].v[0] = p1.b1.v[0] * matrix.M11 + p1.b1.v[1] * matrix.M21 + p1.b2.v[2] * matrix.M31;
			ccBB[threadID, 4].v[1] = p1.b1.v[0] * matrix.M12 + p1.b1.v[1] * matrix.M22 + p1.b2.v[2] * matrix.M32;
			ccBB[threadID, 4].v[2] = p1.b1.v[0] * matrix.M13 + p1.b1.v[1] * matrix.M23 + p1.b2.v[2] * matrix.M33;
			ccBB[threadID, 5].v[0] = p1.b2.v[0] * matrix.M11 + p1.b1.v[1] * matrix.M21 + p1.b2.v[2] * matrix.M31;
			ccBB[threadID, 5].v[1] = p1.b2.v[0] * matrix.M12 + p1.b1.v[1] * matrix.M22 + p1.b2.v[2] * matrix.M32;
			ccBB[threadID, 5].v[2] = p1.b2.v[0] * matrix.M13 + p1.b1.v[1] * matrix.M23 + p1.b2.v[2] * matrix.M33;
			ccBB[threadID, 6].v[0] = p1.b1.v[0] * matrix.M11 + p1.b2.v[1] * matrix.M21 + p1.b2.v[2] * matrix.M31;
			ccBB[threadID, 6].v[1] = p1.b1.v[0] * matrix.M12 + p1.b2.v[1] * matrix.M22 + p1.b2.v[2] * matrix.M32;
			ccBB[threadID, 6].v[2] = p1.b1.v[0] * matrix.M13 + p1.b2.v[1] * matrix.M23 + p1.b2.v[2] * matrix.M33;
			ccBB[threadID, 7].v[0] = p1.b2.v[0] * matrix.M11 + p1.b2.v[1] * matrix.M21 + p1.b2.v[2] * matrix.M31;
			ccBB[threadID, 7].v[1] = p1.b2.v[0] * matrix.M12 + p1.b2.v[1] * matrix.M22 + p1.b2.v[2] * matrix.M32;
			ccBB[threadID, 7].v[2] = p1.b2.v[0] * matrix.M13 + p1.b2.v[1] * matrix.M23 + p1.b2.v[2] * matrix.M33;
			for (int i = 0; i < p1.numUsed; i++)
			{
				colPListT[threadID].v1[i].v[0] = (float)(int)p1.v1[i].vx * matrix.M11 + (float)(int)p1.v1[i].vy * matrix.M21 + (float)(int)p1.v1[i].vz * matrix.M31;
				colPListT[threadID].v1[i].v[1] = (float)(int)p1.v1[i].vx * matrix.M12 + (float)(int)p1.v1[i].vy * matrix.M22 + (float)(int)p1.v1[i].vz * matrix.M32;
				colPListT[threadID].v1[i].v[2] = (float)(int)p1.v1[i].vx * matrix.M13 + (float)(int)p1.v1[i].vy * matrix.M23 + (float)(int)p1.v1[i].vz * matrix.M33;
			}
			ccP3[threadID].v1 = colPListT[threadID].v1;
		}
		else
		{
			if (p1.bbDirty)
			{
				p1.b1.v[0] = (int)p1.v1[0].vx;
				p1.b1.v[1] = (int)p1.v1[0].vy;
				p1.b1.v[2] = (int)p1.v1[0].vz;
				p1.b2.v[0] = (int)p1.v1[0].vx;
				p1.b2.v[1] = (int)p1.v1[0].vy;
				p1.b2.v[2] = (int)p1.v1[0].vz;
				for (int i = 0; i < p1.numUsed; i++)
				{
					if ((float)(int)p1.v1[i].vx < p1.b1.v[0])
					{
						p1.b1.v[0] = (int)p1.v1[i].vx;
					}
					if ((float)(int)p1.v1[i].vx > p1.b2.v[0])
					{
						p1.b2.v[0] = (int)p1.v1[i].vx;
					}
					if ((float)(int)p1.v1[i].vy < p1.b1.v[1])
					{
						p1.b1.v[1] = (int)p1.v1[i].vy;
					}
					if ((float)(int)p1.v1[i].vy > p1.b2.v[1])
					{
						p1.b2.v[1] = (int)p1.v1[i].vy;
					}
					if ((float)(int)p1.v1[i].vz < p1.b1.v[2])
					{
						p1.b1.v[2] = (int)p1.v1[i].vz;
					}
					if ((float)(int)p1.v1[i].vz > p1.b2.v[2])
					{
						p1.b2.v[2] = (int)p1.v1[i].vz;
					}
				}
				p1.bbDirty = false;
			}
			ccBB[threadID, 0].v[0] = p1.b1.v[0];
			ccBB[threadID, 0].v[1] = p1.b1.v[1];
			ccBB[threadID, 0].v[2] = p1.b1.v[2];
			ccBB[threadID, 1].v[0] = p1.b2.v[0];
			ccBB[threadID, 1].v[1] = p1.b1.v[1];
			ccBB[threadID, 1].v[2] = p1.b1.v[2];
			ccBB[threadID, 2].v[0] = p1.b1.v[0];
			ccBB[threadID, 2].v[1] = p1.b2.v[1];
			ccBB[threadID, 2].v[2] = p1.b1.v[2];
			ccBB[threadID, 3].v[0] = p1.b2.v[0];
			ccBB[threadID, 3].v[1] = p1.b2.v[1];
			ccBB[threadID, 3].v[2] = p1.b1.v[2];
			ccBB[threadID, 4].v[0] = p1.b1.v[0];
			ccBB[threadID, 4].v[1] = p1.b1.v[1];
			ccBB[threadID, 4].v[2] = p1.b2.v[2];
			ccBB[threadID, 5].v[0] = p1.b2.v[0];
			ccBB[threadID, 5].v[1] = p1.b1.v[1];
			ccBB[threadID, 5].v[2] = p1.b2.v[2];
			ccBB[threadID, 6].v[0] = p1.b1.v[0];
			ccBB[threadID, 6].v[1] = p1.b2.v[1];
			ccBB[threadID, 6].v[2] = p1.b2.v[2];
			ccBB[threadID, 7].v[0] = p1.b2.v[0];
			ccBB[threadID, 7].v[1] = p1.b2.v[1];
			ccBB[threadID, 7].v[2] = p1.b2.v[2];
			for (int i = 0; i < p1.numUsed; i++)
			{
				colPListT[threadID].v1[i].v[0] = (int)p1.v1[i].vx;
				colPListT[threadID].v1[i].v[1] = (int)p1.v1[i].vy;
				colPListT[threadID].v1[i].v[2] = (int)p1.v1[i].vz;
			}
		}
		ccP3[threadID].v1 = colPListT[threadID].v1;
		ccP3[threadID].numP = p1.numP;
		ccP3[threadID].numUsed = p1.numUsed;
		float num6;
		float num5;
		float num4;
		for (float num3 = 0f; num3 < len; num3 += 1f)
		{
			if ((num & 1) == 0)
			{
				p1.pos1.v[0] += Vx;
			}
			if ((num & 2) == 0)
			{
				p1.pos1.v[1] += Vy;
			}
			if ((num & 4) == 0)
			{
				p1.pos1.v[2] += Vz;
			}
			num4 = global::MainGame.MainGame.MaxDown - p1.pos1.v[2];
			for (int i = 0; i < p1.numUsed; i++)
			{
				if (!(ccP3[threadID].v1[i].v[2] < num4))
				{
					continue;
				}
				if (altPaths)
				{
					p1.pos1.v[2] -= Vz;
					num |= 4;
					if ((Vx == 0f || (num & 1) != 0) && (Vy == 0f || (num & 2) != 0))
					{
						p1.pos2.v[0] = p1.pos1.v[0];
						p1.pos2.v[1] = p1.pos1.v[1];
						p1.pos2.v[2] = p1.pos1.v[2];
						return num;
					}
					if ((num & 1) == 0)
					{
						p1.pos1.v[0] -= Vx;
					}
					if ((num & 2) == 0)
					{
						p1.pos1.v[1] -= Vy;
					}
					p1.pos2.v[2] = p1.pos1.v[2];
					Calculate_Length_Byte(ref p1, ref Vx, ref Vy, ref Vz, ref len);
					num3 = 0f;
					if ((num & 1) == 0)
					{
						p1.pos1.v[0] += Vx;
					}
					if ((num & 2) == 0)
					{
						p1.pos1.v[1] += Vy;
					}
					break;
				}
				num |= 0xC;
				if ((num & 1) == 0)
				{
					p1.pos1.v[0] -= Vx;
				}
				if ((num & 2) == 0)
				{
					p1.pos1.v[1] -= Vy;
				}
				p1.pos1.v[2] -= Vz;
				p1.pos2.v[0] = p1.pos1.v[0];
				p1.pos2.v[1] = p1.pos1.v[1];
				p1.pos2.v[2] = p1.pos1.v[2];
				return num;
			}
			num4 = p1.pos1.v[0];
			num5 = p1.pos1.v[1];
			num6 = p1.pos1.v[2];
			if (CheckBoxParticle(num4, num5, num6, ref ccP3[threadID], ref ccBB, objID, threadID) != 0)
			{
				if (!altPaths)
				{
					num |= 8;
					if ((num & 1) == 0)
					{
						p1.pos1.v[0] -= Vx;
					}
					if ((num & 2) == 0)
					{
						p1.pos1.v[1] -= Vy;
					}
					if ((num & 4) == 0)
					{
						p1.pos1.v[2] -= Vz;
					}
					p1.pos2.v[0] = p1.pos1.v[0];
					p1.pos2.v[1] = p1.pos1.v[1];
					p1.pos2.v[2] = p1.pos1.v[2];
					return num;
				}
				int num7 = 1;
				if ((num & 1) == 0)
				{
					num4 -= Vx;
					if ((num7 = CheckBoxParticle(num4, num5, num6, ref ccP3[threadID], ref ccBB, objID, threadID)) == 0)
					{
						p1.pos1.v[0] -= Vx;
						num |= 1;
						p1.pos2.v[0] = p1.pos1.v[0];
						Calculate_Length_Byte(ref p1, ref Vx, ref Vy, ref Vz, ref len);
						num3 = 0f;
					}
					else
					{
						num4 += Vx;
					}
				}
				if (num7 == 1 && (num & 2) == 0)
				{
					num5 -= Vy;
					if ((num7 = CheckBoxParticle(num4, num5, num6, ref ccP3[threadID], ref ccBB, objID, threadID)) == 0)
					{
						p1.pos1.v[1] -= Vy;
						num |= 2;
						p1.pos2.v[1] = p1.pos1.v[1];
						Calculate_Length_Byte(ref p1, ref Vx, ref Vy, ref Vz, ref len);
						num3 = 0f;
					}
					else
					{
						num5 += Vy;
					}
				}
				if (num7 == 1 && (num & 4) == 0)
				{
					num6 -= Vz;
					if ((num7 = CheckBoxParticle(num4, num5, num6, ref ccP3[threadID], ref ccBB, objID, threadID)) == 0)
					{
						p1.pos1.v[2] -= Vz;
						num |= 4;
						p1.pos2.v[2] = p1.pos1.v[2];
						Calculate_Length_Byte(ref p1, ref Vx, ref Vy, ref Vz, ref len);
						num3 = 0f;
					}
					else
					{
						num6 += Vz;
					}
				}
				if (num == 0 && num7 == 1)
				{
					num4 -= Vx;
					num5 -= Vy;
					if ((num7 = CheckBoxParticle(num4, num5, num6, ref ccP3[threadID], ref ccBB, objID, threadID)) == 0)
					{
						p1.pos1.v[0] -= Vx;
						p1.pos1.v[1] -= Vy;
						num |= 3;
						p1.pos2.v[0] = p1.pos1.v[0];
						p1.pos2.v[1] = p1.pos1.v[1];
						Calculate_Length_Byte(ref p1, ref Vx, ref Vy, ref Vz, ref len);
						num3 = 0f;
					}
					else
					{
						num4 += Vx;
						num6 -= Vz;
						if ((num7 = CheckBoxParticle(num4, num5, num6, ref ccP3[threadID], ref ccBB, objID, threadID)) == 0)
						{
							p1.pos1.v[1] -= Vy;
							p1.pos1.v[2] -= Vz;
							num |= 6;
							p1.pos2.v[1] = p1.pos1.v[1];
							p1.pos2.v[2] = p1.pos1.v[2];
							Calculate_Length_Byte(ref p1, ref Vx, ref Vy, ref Vz, ref len);
							num3 = 0f;
						}
						else
						{
							num5 += Vy;
							num4 -= Vx;
							if ((num7 = CheckBoxParticle(num4, num5, num6, ref ccP3[threadID], ref ccBB, objID, threadID)) == 0)
							{
								p1.pos1.v[0] -= Vx;
								p1.pos1.v[2] -= Vz;
								num |= 5;
								p1.pos2.v[0] = p1.pos1.v[0];
								p1.pos2.v[2] = p1.pos1.v[2];
								Calculate_Length_Byte(ref p1, ref Vx, ref Vy, ref Vz, ref len);
								num3 = 0f;
							}
						}
					}
				}
				if (num7 == 1)
				{
					num |= 8;
					if ((num & 1) == 0)
					{
						p1.pos1.v[0] -= Vx;
					}
					if ((num & 2) == 0)
					{
						p1.pos1.v[1] -= Vy;
					}
					if ((num & 4) == 0)
					{
						p1.pos1.v[2] -= Vz;
					}
					p1.pos2.v[0] = p1.pos1.v[0];
					p1.pos2.v[1] = p1.pos1.v[1];
					p1.pos2.v[2] = p1.pos1.v[2];
					return num;
				}
				if (((num & 1) != 0 || Vx == 0f) && ((num & 2) != 0 || Vy == 0f) && ((num & 4) != 0 || Vz == 0f))
				{
					num |= 8;
					p1.pos2.v[0] = p1.pos1.v[0];
					p1.pos2.v[1] = p1.pos1.v[1];
					p1.pos2.v[2] = p1.pos1.v[2];
					return num;
				}
			}
			num2++;
			if (num2 > 2000)
			{
				num3 = len + 10f;
			}
		}
		if ((num & 1) != 0)
		{
			p1.pos2.v[0] = p1.pos1.v[0];
		}
		if ((num & 2) != 0)
		{
			p1.pos2.v[1] = p1.pos1.v[1];
		}
		if ((num & 4) != 0)
		{
			p1.pos2.v[2] = p1.pos1.v[2];
		}
		num4 = global::MainGame.MainGame.MaxDown - p1.pos2.v[2];
		for (int i = 0; i < p1.numUsed; i++)
		{
			if (ccP3[threadID].v1[i].v[2] < num4)
			{
				if (altPaths)
				{
					p1.pos2.v[2] = p1.pos1.v[2];
					num |= 4;
					break;
				}
				num |= 0xC;
				p1.pos2.v[0] = p1.pos1.v[0];
				p1.pos2.v[1] = p1.pos1.v[1];
				p1.pos2.v[2] = (p1.pos1.v[2] = 0f);
				return num;
			}
		}
		num4 = p1.pos2.v[0];
		num5 = p1.pos2.v[1];
		num6 = p1.pos2.v[2];
		if (CheckBoxParticle(num4, num5, num6, ref ccP3[threadID], ref ccBB, objID, threadID) != 0)
		{
			if (!altPaths)
			{
				num |= 8;
				p1.pos2.v[0] = p1.pos1.v[0];
				p1.pos2.v[1] = p1.pos1.v[1];
				p1.pos2.v[2] = p1.pos1.v[2];
				return num;
			}
			int num7 = 1;
			if ((num & 1) == 0)
			{
				num4 = p1.pos1.v[0];
				if ((num7 = CheckBoxParticle(num4, num5, num6, ref ccP3[threadID], ref ccBB, objID, threadID)) == 0)
				{
					p1.pos2.v[0] = p1.pos1.v[0];
					num |= 1;
				}
				num4 = p1.pos2.v[0];
			}
			if (num7 == 1 && (num & 2) == 0)
			{
				num5 = p1.pos1.v[1];
				if ((num7 = CheckBoxParticle(num4, num5, num6, ref ccP3[threadID], ref ccBB, objID, threadID)) == 0)
				{
					p1.pos2.v[1] = p1.pos1.v[1];
					num |= 2;
				}
				num5 = p1.pos2.v[1];
			}
			if (num7 == 1 && (num & 4) == 0)
			{
				num6 = p1.pos1.v[2];
				if ((num7 = CheckBoxParticle(num4, num5, num6, ref ccP3[threadID], ref ccBB, objID, threadID)) == 0)
				{
					p1.pos2.v[2] = p1.pos1.v[2];
					num |= 4;
				}
				num6 = p1.pos2.v[2];
			}
			if (num == 0 && num7 == 1)
			{
				num4 = p1.pos1.v[0];
				num5 = p1.pos1.v[1];
				if ((num7 = CheckBoxParticle(num4, num5, num6, ref ccP3[threadID], ref ccBB, objID, threadID)) == 0)
				{
					p1.pos2.v[0] = p1.pos1.v[0];
					p1.pos2.v[1] = p1.pos1.v[1];
					num |= 3;
				}
				else
				{
					num4 = p1.pos2.v[0];
					num6 = p1.pos1.v[2];
					if ((num7 = CheckBoxParticle(num4, num5, num6, ref ccP3[threadID], ref ccBB, objID, threadID)) == 0)
					{
						p1.pos2.v[1] = p1.pos1.v[1];
						p1.pos2.v[2] = p1.pos1.v[2];
						num |= 6;
					}
					else
					{
						num5 = p1.pos2.v[2];
						num4 = p1.pos1.v[0];
						if ((num7 = CheckBoxParticle(num4, num5, num6, ref ccP3[threadID], ref ccBB, objID, threadID)) == 0)
						{
							p1.pos2.v[0] = p1.pos1.v[0];
							p1.pos2.v[2] = p1.pos1.v[2];
							num |= 5;
						}
					}
				}
			}
			if (num7 == 1)
			{
				num |= 8;
				p1.pos2.v[0] = p1.pos1.v[0];
				p1.pos2.v[1] = p1.pos1.v[1];
				p1.pos2.v[2] = p1.pos1.v[2];
				return num;
			}
		}
		return num;
	}

	public int CheckBoxParticle(float x, float y, float z, ref StructsClass.particle_list p1, ref StructsClass.vtex[,] v1, long objID, byte threadID)
	{
		int num = 0;
		for (long num2 = 0L; num2 < 8; num2++)
		{
			boxList[num2] = -1L;
		}
		for (long num2 = 0L; num2 < 8; num2++)
		{
			long num3 = (StructsClass.roundf(x + v1[(int)checked((nint)unchecked((long)threadID)), (int)checked((nint)num2)].v[0]) + (long)BoxX) / BoxSize;
			long num4 = (StructsClass.roundf(y + v1[(int)checked((nint)unchecked((long)threadID)), (int)checked((nint)num2)].v[1]) + (long)BoxY) / BoxSize;
			long num5 = (StructsClass.roundf(z + v1[(int)checked((nint)unchecked((long)threadID)), (int)checked((nint)num2)].v[2]) + (long)BoxZ) / BoxSize;
			long num6 = num5 + num4 * BoxDimZ + num3 * BoxDimZ * BoxDimY;
			if (num6 < 0 || num6 >= numBoxes)
			{
				continue;
			}
			long num7;
			for (num7 = 0L; num7 < num; num7++)
			{
				if (boxList[num7] == num6)
				{
					num7 = -1L;
					break;
				}
			}
			if (num7 > -1)
			{
				boxList[num++] = num6;
			}
		}
		while (num-- > 0)
		{
			long num6 = boxList[num];
			for (long num8 = 0L; num8 < mainBox[num6].cnt; num8++)
			{
				long num7 = mainBox[num6].oList[num8];
				if (num7 > -1 && global::GameObjects.GameObjects.objStat[num7] > 1 && num7 != objID)
				{
					float num15;
					float num16;
					float num17;
					float num18;
					float num19;
					float num20;
					if ((global::GameObjects.GameObjects.objMaster[num7].type & 1) > 0)
					{
						long num9 = global::GameObjects.GameObjects.objMaster[num7].dimX;
						long num10 = global::GameObjects.GameObjects.objMaster[num7].dimY;
						long num11 = global::GameObjects.GameObjects.objMaster[num7].dimZ;
						if (global::GameObjects.GameObjects.objMaster[num7].isRotated)
						{
							Matrix mvT = global::GameObjects.GameObjects.objMaster[num7].mvT;
							float num12 = x - global::GameObjects.GameObjects.objMaster[num7].x;
							float num13 = y - global::GameObjects.GameObjects.objMaster[num7].y;
							float num14 = z - global::GameObjects.GameObjects.objMaster[num7].z;
							num15 = num12 * mvT.M11 + num13 * mvT.M21 + num14 * mvT.M31 - 1f;
							num16 = num12 * mvT.M12 + num13 * mvT.M22 + num14 * mvT.M32 - 1f;
							num17 = num12 * mvT.M13 + num13 * mvT.M23 + num14 * mvT.M33 - 1f;
							num15 = global::GameObjects.GameObjects.objMaster[num7].x - num15 - 1f;
							num16 = global::GameObjects.GameObjects.objMaster[num7].y - num16 - 1f;
							num17 = global::GameObjects.GameObjects.objMaster[num7].z - num17 - 1f;
							num18 = num15 + (float)num9 + 1f;
							num19 = num16 + (float)num10 + 1f;
							num20 = num17 + (float)num11 + 1f;
							for (long num2 = 0L; num2 < p1.numUsed; num2++)
							{
								num12 = p1.v1[num2].v[0] * mvT.M11 + p1.v1[num2].v[1] * mvT.M21 + p1.v1[num2].v[2] * mvT.M31;
								num13 = p1.v1[num2].v[0] * mvT.M12 + p1.v1[num2].v[1] * mvT.M22 + p1.v1[num2].v[2] * mvT.M32;
								num14 = p1.v1[num2].v[0] * mvT.M13 + p1.v1[num2].v[1] * mvT.M23 + p1.v1[num2].v[2] * mvT.M33;
								if (num12 > num15 && num12 < num18 && num13 > num16 && num13 < num19 && num14 > num17 && num14 < num20)
								{
									long num21 = (long)(num12 - num15 - 1f);
									if (num21 < 0)
									{
										num21 = 0L;
									}
									long num22 = (long)(num13 - num16 - 1f);
									if (num22 < 0)
									{
										num22 = 0L;
									}
									long num23 = (long)(num14 - num17 - 1f);
									if (num23 < 0)
									{
										num23 = 0L;
									}
									long num24 = (long)(num12 - num15);
									if (num24 >= global::GameObjects.GameObjects.objMaster[num7].dimX)
									{
										num24 = (long)global::GameObjects.GameObjects.objMaster[num7].dimX - 1L;
									}
									long num25 = (long)(num13 - num16);
									if (num25 >= global::GameObjects.GameObjects.objMaster[num7].dimY)
									{
										num25 = (long)global::GameObjects.GameObjects.objMaster[num7].dimY - 1L;
									}
									long num26 = (long)(num14 - num17);
									if (num26 >= global::GameObjects.GameObjects.objMaster[num7].dimZ)
									{
										num26 = (long)global::GameObjects.GameObjects.objMaster[num7].dimZ - 1L;
									}
									if (global::GameObjects.GameObjects.objMaster[num7].pt1[num23 + num22 * num11 + num21 * num11 * num10].status != 0 || global::GameObjects.GameObjects.objMaster[num7].pt1[num23 + num25 * num11 + num24 * num11 * num10].status != 0 || global::GameObjects.GameObjects.objMaster[num7].pt1[num23 + num22 * num11 + num24 * num11 * num10].status != 0 || global::GameObjects.GameObjects.objMaster[num7].pt1[num23 + num25 * num11 + num21 * num11 * num10].status != 0 || global::GameObjects.GameObjects.objMaster[num7].pt1[num26 + num22 * num11 + num21 * num11 * num10].status != 0 || global::GameObjects.GameObjects.objMaster[num7].pt1[num26 + num25 * num11 + num24 * num11 * num10].status != 0 || global::GameObjects.GameObjects.objMaster[num7].pt1[num26 + num22 * num11 + num24 * num11 * num10].status != 0 || global::GameObjects.GameObjects.objMaster[num7].pt1[num26 + num25 * num11 + num21 * num11 * num10].status != 0)
									{
										colIDT[threadID] = num7;
										colPtrT[threadID] = global::GameObjects.GameObjects.objMaster[num7];
										colX[threadID] = num21;
										colY[threadID] = num22;
										colZ[threadID] = num23;
										return 1;
									}
								}
							}
							continue;
						}
						num15 = global::GameObjects.GameObjects.objMaster[num7].x - x - 1f;
						num16 = global::GameObjects.GameObjects.objMaster[num7].y - y - 1f;
						num17 = global::GameObjects.GameObjects.objMaster[num7].z - z - 1f;
						num18 = num15 + (float)num9 + 1f;
						num19 = num16 + (float)num10 + 1f;
						num20 = num17 + (float)num11 + 1f;
						for (long num2 = 0L; num2 < p1.numUsed; num2++)
						{
							float num12 = p1.v1[num2].v[0];
							float num13 = p1.v1[num2].v[1];
							float num14 = p1.v1[num2].v[2];
							if (num12 > num15 && num12 < num18 && num13 > num16 && num13 < num19 && num14 > num17 && num14 < num20)
							{
								long num21 = (long)(num12 - num15 - 1f);
								if (num21 < 0)
								{
									num21 = 0L;
								}
								long num22 = (long)(num13 - num16 - 1f);
								if (num22 < 0)
								{
									num22 = 0L;
								}
								long num23 = (long)(num14 - num17 - 1f);
								if (num23 < 0)
								{
									num23 = 0L;
								}
								long num24 = (long)(num12 - num15);
								if (num24 >= global::GameObjects.GameObjects.objMaster[num7].dimX)
								{
									num24 = (long)global::GameObjects.GameObjects.objMaster[num7].dimX - 1L;
								}
								long num25 = (long)(num13 - num16);
								if (num25 >= global::GameObjects.GameObjects.objMaster[num7].dimY)
								{
									num25 = (long)global::GameObjects.GameObjects.objMaster[num7].dimY - 1L;
								}
								long num26 = (long)(num14 - num17);
								if (num26 >= global::GameObjects.GameObjects.objMaster[num7].dimZ)
								{
									num26 = (long)global::GameObjects.GameObjects.objMaster[num7].dimZ - 1L;
								}
								if (global::GameObjects.GameObjects.objMaster[num7].pt1[num23 + num22 * num11 + num21 * num11 * num10].status != 0 || global::GameObjects.GameObjects.objMaster[num7].pt1[num23 + num25 * num11 + num24 * num11 * num10].status != 0 || global::GameObjects.GameObjects.objMaster[num7].pt1[num23 + num22 * num11 + num24 * num11 * num10].status != 0 || global::GameObjects.GameObjects.objMaster[num7].pt1[num23 + num25 * num11 + num21 * num11 * num10].status != 0 || global::GameObjects.GameObjects.objMaster[num7].pt1[num26 + num22 * num11 + num21 * num11 * num10].status != 0 || global::GameObjects.GameObjects.objMaster[num7].pt1[num26 + num25 * num11 + num24 * num11 * num10].status != 0 || global::GameObjects.GameObjects.objMaster[num7].pt1[num26 + num22 * num11 + num24 * num11 * num10].status != 0 || global::GameObjects.GameObjects.objMaster[num7].pt1[num26 + num25 * num11 + num21 * num11 * num10].status != 0)
								{
									colIDT[threadID] = num7;
									colPtrT[threadID] = global::GameObjects.GameObjects.objMaster[num7];
									colX[threadID] = num21;
									colY[threadID] = num22;
									colZ[threadID] = num23;
									return 1;
								}
							}
						}
						continue;
					}
					num15 = global::GameObjects.GameObjects.objMaster[num7].pList.b1.v[0] - 0.5f;
					num16 = global::GameObjects.GameObjects.objMaster[num7].pList.b1.v[1] - 0.5f;
					num17 = global::GameObjects.GameObjects.objMaster[num7].pList.b1.v[2] - 0.5f;
					num18 = global::GameObjects.GameObjects.objMaster[num7].pList.b2.v[0] + 0.5f;
					num19 = global::GameObjects.GameObjects.objMaster[num7].pList.b2.v[1] + 0.5f;
					num20 = global::GameObjects.GameObjects.objMaster[num7].pList.b2.v[2] + 0.5f;
					if (global::GameObjects.GameObjects.objMaster[num7].isRotated)
					{
						for (long num2 = 0L; num2 < p1.numUsed; num2++)
						{
							Matrix mvT = global::GameObjects.GameObjects.objMaster[num7].mvT;
							float num12 = p1.v1[num2].v[0] - global::GameObjects.GameObjects.objMaster[num7].x;
							float num13 = p1.v1[num2].v[1] - global::GameObjects.GameObjects.objMaster[num7].y;
							float num14 = p1.v1[num2].v[2] - global::GameObjects.GameObjects.objMaster[num7].z;
							float num27 = num12 * mvT.M11 + num13 * mvT.M21 + num14 * mvT.M31;
							float num28 = num12 * mvT.M12 + num13 * mvT.M22 + num14 * mvT.M32;
							float num29 = num12 * mvT.M13 + num13 * mvT.M23 + num14 * mvT.M33;
							num27 += global::GameObjects.GameObjects.objMaster[num7].x;
							num28 += global::GameObjects.GameObjects.objMaster[num7].y;
							num29 += global::GameObjects.GameObjects.objMaster[num7].z;
							if (num27 > num15 && num27 < num18 && num28 > num16 && num28 < num19 && num29 > num17 && num29 < num20)
							{
								colIDT[threadID] = num7;
								colPtrT[threadID] = global::GameObjects.GameObjects.objMaster[num7];
								colX[threadID] = (long)(num12 - num15 - 1f);
								colY[threadID] = (long)(num13 - num16 - 1f);
								colZ[threadID] = (long)(num14 - num17 - 1f);
								return 1;
							}
						}
						continue;
					}
					for (long num2 = 0L; num2 < p1.numUsed; num2++)
					{
						float num12 = p1.v1[num2].v[0];
						float num13 = p1.v1[num2].v[1];
						float num14 = p1.v1[num2].v[2];
						if (num12 > num15 && num12 < num18 && num13 > num16 && num13 < num19 && num14 > num17 && num14 < num20)
						{
							colIDT[threadID] = num7;
							colPtrT[threadID] = global::GameObjects.GameObjects.objMaster[num7];
							colX[threadID] = (long)(num12 - num15 - 1f);
							colY[threadID] = (long)(num13 - num16 - 1f);
							colZ[threadID] = (long)(num14 - num17 - 1f);
							return 1;
						}
					}
				}
				else if (num7 < 0)
				{
					num8 = mainBox[num6].cnt;
				}
			}
		}
		return 0;
	}

	public int CheckCollsionSingle_Threaded(ref StructsClass.particle_list p1, long objID, ref float distance, byte oType, byte threadID)
	{
		float len = 0f;
		float num = 1f;
		float num2 = 1f;
		float num3 = 1f;
		float Vx = 0f;
		float Vy = 0f;
		float Vz = 0f;
		long num4 = 0L;
		colIDT[threadID] = -1L;
		Calculate_Length(ref p1, ref Vx, ref Vy, ref Vz, ref len);
		ccsVxT[threadID] = Vx;
		ccsVyT[threadID] = Vy;
		ccsVzT[threadID] = Vz;
		float num5;
		float num6;
		for (num5 = 1f; num5 < len; num5 += 1f)
		{
			p1.pos1.v[0] += Vx;
			p1.pos1.v[1] += Vy;
			p1.pos1.v[2] += Vz;
			num = p1.pos1.v[0] + p1.v1[0].v[0];
			num2 = p1.pos1.v[1] + p1.v1[0].v[1];
			num3 = p1.pos1.v[2] + p1.v1[0].v[2];
			num6 = mainC.terrainMain.Get_Terrain_Height(num, num2, threadID);
			if (num6 < global::MainGame.MainGame.MaxDown)
			{
				num6 = global::MainGame.MainGame.MaxDown;
			}
			if (num3 < num6)
			{
				p1.pos1.v[0] -= Vx;
				p1.pos1.v[1] -= Vy;
				p1.pos1.v[2] -= Vz;
				p1.pos2.v[0] = p1.pos1.v[0];
				p1.pos2.v[1] = p1.pos1.v[1];
				p1.pos2.v[2] = p1.pos1.v[2];
				distance = num5;
				return 12;
			}
			if (CheckBoxParticleSingle(num, num2, num3, objID, oType, threadID) != 0)
			{
				p1.pos1.v[0] -= Vx;
				p1.pos1.v[1] -= Vy;
				p1.pos1.v[2] -= Vz;
				p1.pos2.v[0] = p1.pos1.v[0];
				p1.pos2.v[1] = p1.pos1.v[1];
				p1.pos2.v[2] = p1.pos1.v[2];
				distance = num5;
				return 8;
			}
			num4++;
			if (num4 > 10000)
			{
				num5 = len + 10f;
			}
		}
		distance = num5;
		num = p1.pos2.v[0] + p1.v1[0].v[0];
		num2 = p1.pos2.v[1] + p1.v1[0].v[1];
		num3 = p1.pos2.v[2] + p1.v1[0].v[2];
		num6 = mainC.terrainMain.Get_Terrain_Height(num, num2, threadID);
		if (num6 < global::MainGame.MainGame.MaxDown)
		{
			num6 = global::MainGame.MainGame.MaxDown;
		}
		if (num3 < num6)
		{
			p1.pos2.v[0] = p1.pos1.v[0];
			p1.pos2.v[1] = p1.pos1.v[1];
			p1.pos2.v[2] = p1.pos1.v[2];
			return 12;
		}
		if (CheckBoxParticleSingle(num, num2, num3, objID, oType, threadID) != 0)
		{
			p1.pos2.v[0] = p1.pos1.v[0];
			p1.pos2.v[1] = p1.pos1.v[1];
			p1.pos2.v[2] = p1.pos1.v[2];
			return 8;
		}
		return 0;
	}

	public int CheckBoxParticleSingle(float x, float y, float z, long objID, byte oType, byte threadID)
	{
		long num = StructsClass.roundf(x);
		long num2 = StructsClass.roundf(y);
		long num3 = StructsClass.roundf(z);
		long num4 = (num + (long)BoxX) / BoxSize;
		long num5 = (num2 + (long)BoxY) / BoxSize;
		long num6 = (num3 + (long)BoxZ) / BoxSize;
		long num7 = num6 + num5 * BoxDimZ + num4 * BoxDimZ * BoxDimY;
		if (num7 < 0 || num7 >= numBoxes)
		{
			return 0;
		}
		for (long num8 = 0L; num8 < mainBox[num7].cnt; num8++)
		{
			long num9 = mainBox[num7].oList[num8];
			if (num9 > -1 && (global::GameObjects.GameObjects.objMaster[num9].type & oType) > 128 && num9 != objID)
			{
				float num10 = 0.5f;
				if ((global::GameObjects.GameObjects.objStat[num9] & 1) == 0)
				{
					num10 = 0f;
				}
				if ((global::GameObjects.GameObjects.objMaster[num9].type & 1) > 0)
				{
					long num11 = global::GameObjects.GameObjects.objMaster[num9].dimX;
					long num12 = global::GameObjects.GameObjects.objMaster[num9].dimY;
					long num13 = global::GameObjects.GameObjects.objMaster[num9].dimZ;
					long num17;
					long num18;
					long num19;
					if (global::GameObjects.GameObjects.objMaster[num9].isRotated)
					{
						Matrix mvT = global::GameObjects.GameObjects.objMaster[num9].mvT;
						float num14 = x - global::GameObjects.GameObjects.objMaster[num9].x;
						float num15 = y - global::GameObjects.GameObjects.objMaster[num9].y;
						float num16 = z - global::GameObjects.GameObjects.objMaster[num9].z;
						num17 = (long)(num14 * mvT.M11 + num14 * mvT.M21 + num14 * mvT.M31 + num10);
						num18 = (long)(num14 * mvT.M12 + num14 * mvT.M22 + num14 * mvT.M32 + num10);
						num19 = (long)(num14 * mvT.M13 + num14 * mvT.M23 + num14 * mvT.M33 + num10);
					}
					else
					{
						num17 = (long)(x - (global::GameObjects.GameObjects.objMaster[num9].x - num10));
						num18 = (long)(y - (global::GameObjects.GameObjects.objMaster[num9].y - num10));
						num19 = (long)(z - (global::GameObjects.GameObjects.objMaster[num9].z - num10));
					}
					if (num17 >= 0 && num17 < num11 && num18 >= 0 && num18 < num12 && num19 >= 0 && num19 < num13 && global::GameObjects.GameObjects.objMaster[num9].pt1[num19 + num18 * num13 + num17 * num13 * num12].status != 0)
					{
						colIDT[threadID] = num9;
						colPtrT[threadID] = global::GameObjects.GameObjects.objMaster[num9];
						colX[threadID] = num17;
						colY[threadID] = num18;
						colZ[threadID] = num19;
						return 1;
					}
					continue;
				}
				float num20 = global::GameObjects.GameObjects.objMaster[num9].pList.b1.v[0] - num10;
				float num21 = global::GameObjects.GameObjects.objMaster[num9].pList.b1.v[1] - num10;
				float num22 = global::GameObjects.GameObjects.objMaster[num9].pList.b1.v[2] - num10;
				float num23 = global::GameObjects.GameObjects.objMaster[num9].pList.b2.v[0] + num10;
				float num24 = global::GameObjects.GameObjects.objMaster[num9].pList.b2.v[1] + num10;
				float num25 = global::GameObjects.GameObjects.objMaster[num9].pList.b2.v[2] + num10;
				if (global::GameObjects.GameObjects.objMaster[num9].isRotated)
				{
					Matrix mvT = global::GameObjects.GameObjects.objMaster[num9].mvT;
					float num14 = x - global::GameObjects.GameObjects.objMaster[num9].x;
					float num15 = y - global::GameObjects.GameObjects.objMaster[num9].y;
					float num16 = z - global::GameObjects.GameObjects.objMaster[num9].z;
					float num26 = num14 * mvT.M11 + num15 * mvT.M21 + num16 * mvT.M31;
					float num27 = num14 * mvT.M12 + num15 * mvT.M22 + num16 * mvT.M32;
					float num28 = num14 * mvT.M13 + num15 * mvT.M23 + num16 * mvT.M33;
					num26 += global::GameObjects.GameObjects.objMaster[num9].x;
					num27 += global::GameObjects.GameObjects.objMaster[num9].y;
					num28 += global::GameObjects.GameObjects.objMaster[num9].z;
					if (num26 > num20 && num26 < num23 && num27 > num21 && num27 < num24 && num28 > num22 && num28 < num25)
					{
						colIDT[threadID] = num9;
						colPtrT[threadID] = global::GameObjects.GameObjects.objMaster[num9];
						colX[threadID] = (long)(x - num20 - num10);
						colY[threadID] = (long)(y - num21 - num10);
						colZ[threadID] = (long)(z - num22 - num10);
						return 1;
					}
				}
				else if (x > num20 && x < num23 && y > num21 && y < num24 && z > num22 && z < num25)
				{
					colIDT[threadID] = num9;
					colPtrT[threadID] = global::GameObjects.GameObjects.objMaster[num9];
					colX[threadID] = (long)(x - num20 - num10);
					colY[threadID] = (long)(y - num21 - num10);
					colZ[threadID] = (long)(z - num22 - num10);
					return 1;
				}
			}
			else if (num9 < 0)
			{
				break;
			}
		}
		return 0;
	}

	public byte CheckBoxParticleSingle_Point(float x, float y, float z, byte oType, byte threadID)
	{
		long num = StructsClass.roundf(x);
		long num2 = StructsClass.roundf(y);
		long num3 = StructsClass.roundf(z);
		long num4 = (num + (long)BoxX) / BoxSize;
		long num5 = (num2 + (long)BoxY) / BoxSize;
		long num6 = (num3 + (long)BoxZ) / BoxSize;
		long num7 = num6 + num5 * BoxDimZ + num4 * BoxDimZ * BoxDimY;
		if (num7 < 0 || num7 >= numBoxes)
		{
			return 0;
		}
		for (long num8 = 0L; num8 < mainBox[num7].cnt; num8++)
		{
			long num9 = mainBox[num7].oList[num8];
			if (num9 > -1 && (global::GameObjects.GameObjects.objMaster[num9].type & oType) > 128)
			{
				float num10 = 0.5f;
				if ((global::GameObjects.GameObjects.objStat[num9] & 1) == 0)
				{
					num10 = 0f;
				}
				if ((global::GameObjects.GameObjects.objMaster[num9].type & 1) > 0)
				{
					long num11 = global::GameObjects.GameObjects.objMaster[num9].dimX;
					long num12 = global::GameObjects.GameObjects.objMaster[num9].dimY;
					long num13 = global::GameObjects.GameObjects.objMaster[num9].dimZ;
					long num17;
					long num18;
					long num19;
					if (global::GameObjects.GameObjects.objMaster[num9].isRotated)
					{
						Matrix mvT = global::GameObjects.GameObjects.objMaster[num9].mvT;
						float num14 = x - global::GameObjects.GameObjects.objMaster[num9].x;
						float num15 = y - global::GameObjects.GameObjects.objMaster[num9].y;
						float num16 = z - global::GameObjects.GameObjects.objMaster[num9].z;
						num17 = (long)(num14 * mvT.M11 + num14 * mvT.M21 + num14 * mvT.M31 + num10);
						num18 = (long)(num14 * mvT.M12 + num14 * mvT.M22 + num14 * mvT.M32 + num10);
						num19 = (long)(num14 * mvT.M13 + num14 * mvT.M23 + num14 * mvT.M33 + num10);
					}
					else
					{
						num17 = (long)(x - (global::GameObjects.GameObjects.objMaster[num9].x - num10));
						num18 = (long)(y - (global::GameObjects.GameObjects.objMaster[num9].y - num10));
						num19 = (long)(z - (global::GameObjects.GameObjects.objMaster[num9].z - num10));
					}
					if (num17 >= 0 && num17 < num11 && num18 >= 0 && num18 < num12 && num19 >= 0 && num19 < num13 && global::GameObjects.GameObjects.objMaster[num9].pt1[num19 + num18 * num13 + num17 * num13 * num12].status != 0)
					{
						colIDT[threadID] = num9;
						colPtrT[threadID] = global::GameObjects.GameObjects.objMaster[num9];
						colX[threadID] = num17;
						colY[threadID] = num18;
						colZ[threadID] = num19;
						return 1;
					}
					continue;
				}
				float num20 = global::GameObjects.GameObjects.objMaster[num9].pList.b1.v[0] - num10;
				float num21 = global::GameObjects.GameObjects.objMaster[num9].pList.b1.v[1] - num10;
				float num22 = global::GameObjects.GameObjects.objMaster[num9].pList.b1.v[2] - num10;
				float num23 = global::GameObjects.GameObjects.objMaster[num9].pList.b2.v[0] + num10;
				float num24 = global::GameObjects.GameObjects.objMaster[num9].pList.b2.v[1] + num10;
				float num25 = global::GameObjects.GameObjects.objMaster[num9].pList.b2.v[2] + num10;
				if (global::GameObjects.GameObjects.objMaster[num9].isRotated)
				{
					Matrix mvT = global::GameObjects.GameObjects.objMaster[num9].mvT;
					float num14 = x - global::GameObjects.GameObjects.objMaster[num9].x;
					float num15 = y - global::GameObjects.GameObjects.objMaster[num9].y;
					float num16 = z - global::GameObjects.GameObjects.objMaster[num9].z;
					float num26 = num14 * mvT.M11 + num15 * mvT.M21 + num16 * mvT.M31;
					float num27 = num14 * mvT.M12 + num15 * mvT.M22 + num16 * mvT.M32;
					float num28 = num14 * mvT.M13 + num15 * mvT.M23 + num16 * mvT.M33;
					num26 += global::GameObjects.GameObjects.objMaster[num9].x;
					num27 += global::GameObjects.GameObjects.objMaster[num9].y;
					num28 += global::GameObjects.GameObjects.objMaster[num9].z;
					if (num26 > num20 && num26 < num23 && num27 > num21 && num27 < num24 && num28 > num22 && num28 < num25)
					{
						colIDT[threadID] = num9;
						colPtrT[threadID] = global::GameObjects.GameObjects.objMaster[num9];
						colX[threadID] = (long)(x - num20 - num10);
						colY[threadID] = (long)(y - num21 - num10);
						colZ[threadID] = (long)(z - num22 - num10);
						return 1;
					}
				}
				else if (x > num20 && x < num23 && y > num21 && y < num24 && z > num22 && z < num25)
				{
					colIDT[threadID] = num9;
					colPtrT[threadID] = global::GameObjects.GameObjects.objMaster[num9];
					colX[threadID] = (long)(x - num20 - num10);
					colY[threadID] = (long)(y - num21 - num10);
					colZ[threadID] = (long)(z - num22 - num10);
					return 1;
				}
			}
			else if (num9 < 0)
			{
				break;
			}
		}
		return 0;
	}

	public byte CheckBoxParticleSingle_Path_Point(float x, float y, float z, float vX, float vY, float vZ, float len, byte oType, byte threadID)
	{
		int num = (int)Math.Ceiling(len);
		for (int i = 0; i < num; i++)
		{
			long num2 = StructsClass.roundf(x);
			long num3 = StructsClass.roundf(y);
			long num4 = StructsClass.roundf(z);
			long num5 = (num2 + (long)BoxX) / BoxSize;
			long num6 = (num3 + (long)BoxY) / BoxSize;
			long num7 = (num4 + (long)BoxZ) / BoxSize;
			long num8 = num7 + num6 * BoxDimZ + num5 * BoxDimZ * BoxDimY;
			if (num8 < 0 || num8 >= numBoxes)
			{
				return 0;
			}
			for (long num9 = 0L; num9 < mainBox[num8].cnt; num9++)
			{
				long num10 = mainBox[num8].oList[num9];
				if (num10 > -1 && (global::GameObjects.GameObjects.objMaster[num10].type & oType) > 128)
				{
					float num11 = 0.5f;
					if ((global::GameObjects.GameObjects.objStat[num10] & 1) == 0)
					{
						num11 = 0f;
					}
					if ((global::GameObjects.GameObjects.objMaster[num10].type & 1) > 0)
					{
						long num12 = global::GameObjects.GameObjects.objMaster[num10].dimX;
						long num13 = global::GameObjects.GameObjects.objMaster[num10].dimY;
						long num14 = global::GameObjects.GameObjects.objMaster[num10].dimZ;
						long num18;
						long num19;
						long num20;
						if (global::GameObjects.GameObjects.objMaster[num10].isRotated)
						{
							Matrix mvT = global::GameObjects.GameObjects.objMaster[num10].mvT;
							float num15 = x - global::GameObjects.GameObjects.objMaster[num10].x;
							float num16 = y - global::GameObjects.GameObjects.objMaster[num10].y;
							float num17 = z - global::GameObjects.GameObjects.objMaster[num10].z;
							num18 = (long)(num15 * mvT.M11 + num15 * mvT.M21 + num15 * mvT.M31 + num11);
							num19 = (long)(num15 * mvT.M12 + num15 * mvT.M22 + num15 * mvT.M32 + num11);
							num20 = (long)(num15 * mvT.M13 + num15 * mvT.M23 + num15 * mvT.M33 + num11);
						}
						else
						{
							num18 = (long)(x - (global::GameObjects.GameObjects.objMaster[num10].x - num11));
							num19 = (long)(y - (global::GameObjects.GameObjects.objMaster[num10].y - num11));
							num20 = (long)(z - (global::GameObjects.GameObjects.objMaster[num10].z - num11));
						}
						if (num18 >= 0 && num18 < num12 && num19 >= 0 && num19 < num13 && num20 >= 0 && num20 < num14 && global::GameObjects.GameObjects.objMaster[num10].pt1[num20 + num19 * num14 + num18 * num14 * num13].status != 0)
						{
							colIDT[threadID] = num10;
							colPtrT[threadID] = global::GameObjects.GameObjects.objMaster[num10];
							colXPos[threadID] = x;
							colYPos[threadID] = y;
							colZPos[threadID] = z;
							return 1;
						}
						continue;
					}
					float num21 = global::GameObjects.GameObjects.objMaster[num10].pList.b1.v[0] - num11;
					float num22 = global::GameObjects.GameObjects.objMaster[num10].pList.b1.v[1] - num11;
					float num23 = global::GameObjects.GameObjects.objMaster[num10].pList.b1.v[2] - num11;
					float num24 = global::GameObjects.GameObjects.objMaster[num10].pList.b2.v[0] + num11;
					float num25 = global::GameObjects.GameObjects.objMaster[num10].pList.b2.v[1] + num11;
					float num26 = global::GameObjects.GameObjects.objMaster[num10].pList.b2.v[2] + num11;
					if (global::GameObjects.GameObjects.objMaster[num10].isRotated)
					{
						Matrix mvT = global::GameObjects.GameObjects.objMaster[num10].mvT;
						float num15 = x - global::GameObjects.GameObjects.objMaster[num10].x;
						float num16 = y - global::GameObjects.GameObjects.objMaster[num10].y;
						float num17 = z - global::GameObjects.GameObjects.objMaster[num10].z;
						float num27 = num15 * mvT.M11 + num16 * mvT.M21 + num17 * mvT.M31;
						float num28 = num15 * mvT.M12 + num16 * mvT.M22 + num17 * mvT.M32;
						float num29 = num15 * mvT.M13 + num16 * mvT.M23 + num17 * mvT.M33;
						num27 += global::GameObjects.GameObjects.objMaster[num10].x;
						num28 += global::GameObjects.GameObjects.objMaster[num10].y;
						num29 += global::GameObjects.GameObjects.objMaster[num10].z;
						if (num27 > num21 && num27 < num24 && num28 > num22 && num28 < num25 && num29 > num23 && num29 < num26)
						{
							colIDT[threadID] = num10;
							colPtrT[threadID] = global::GameObjects.GameObjects.objMaster[num10];
							colXPos[threadID] = x;
							colYPos[threadID] = y;
							colZPos[threadID] = z;
							return 1;
						}
					}
					else if (x > num21 && x < num24 && y > num22 && y < num25 && z > num23 && z < num26)
					{
						colIDT[threadID] = num10;
						colPtrT[threadID] = global::GameObjects.GameObjects.objMaster[num10];
						colXPos[threadID] = x;
						colYPos[threadID] = y;
						colZPos[threadID] = z;
						return 1;
					}
				}
				else if (num10 < 0)
				{
					break;
				}
			}
			x += vX;
			y += vY;
			z += vZ;
		}
		return 0;
	}

	public bool Ignore(byte threadID, ushort particleID, long objID)
	{
		for (byte b = 0; b < 15; b++)
		{
			if (floatArIgnore[threadID, particleID, b] == objID)
			{
				return true;
			}
		}
		return false;
	}

	public void AddToIgnoreList(byte threadID, ushort particleID, long objID)
	{
		for (byte b = 0; b < 15; b++)
		{
			if (floatArIgnore[threadID, particleID, b] == -1)
			{
				floatArIgnore[threadID, particleID, b] = objID;
				break;
			}
		}
	}

	public void ResetIgnoreList(byte threadID, ushort numParticles)
	{
		for (ushort num = 0; num < numParticles; num++)
		{
			for (byte b = 0; b < 15; b++)
			{
				floatArIgnore[threadID, num, b] = -1L;
			}
		}
	}

	public void ResetParticleIgnoreList(byte threadID, ushort particleID)
	{
		for (byte b = 0; b < 15; b++)
		{
			floatArIgnore[threadID, particleID, b] = -1L;
		}
	}

	public int CheckCollision_Detailed_List(ref StructsClass.particle_list p1, int numPoints, int offset, byte oType, byte cType, byte threadID)
	{
		bool flag = false;
		bool flag2 = false;
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		int num4 = 0;
		float num5 = 0f;
		float num6 = 0f;
		float num7 = 0f;
		float num8 = 0f;
		float num9 = 0f;
		float num10 = 0f;
		float num11 = 0f;
		float num12 = 0f;
		float num13 = 0f;
		float num14 = 0f;
		float num15 = 0f;
		float num16 = 0f;
		float num17 = 0f;
		float num18 = 0f;
		float num19 = 0f;
		float num20 = 0f;
		Vector3 InitialRayStart = default(Vector3);
		Vector3 InitialRayEnd = default(Vector3);
		Vector3 IntersectPosition = default(Vector3);
		Vector3 IntersectNormal = default(Vector3);
		float num21 = p1.pos1.v[0];
		float num22 = p1.pos1.v[1];
		float num23 = p1.pos1.v[2];
		float num24 = p1.pos2.v[0];
		float num25 = p1.pos2.v[1];
		float num26 = p1.pos2.v[2];
		num11 = num24 - num21;
		num12 = num25 - num22;
		num13 = num26 - num23;
		float num27 = (float)Math.Sqrt(num11 * num11 + num12 * num12 + num13 * num13);
		float num28 = num27;
		int num29 = 0;
		int num30 = offset;
		for (int i = 0; i < numPoints; i++)
		{
			floatArStatus[threadID, i] = 0;
			floatArID[threadID, i] = -1;
			floatArDir[threadID, i, 3] = num27;
			floatArDir[threadID, i, 4] = 0f;
			floatArDir[threadID, i, 5] = 0f;
			floatArDir[threadID, i, 6] = 0f;
			num11 = num21 + floatAr[threadID, num29++];
			num12 = num22 + floatAr[threadID, num29++];
			num13 = num23 + floatAr[threadID, num29++];
			num2 = num30;
			num14 = num24 + floatAr[threadID, num30++];
			num3 = num30;
			num15 = num25 + floatAr[threadID, num30++];
			num4 = num30;
			num16 = num26 + floatAr[threadID, num30++];
			num17 = num14 - num11;
			num18 = num15 - num12;
			num19 = num16 - num13;
			num20 = (float)Math.Sqrt(num17 * num17 + num18 * num18 + num19 * num19);
			colIDT[threadID] = -1L;
			flag = false;
			if (num20 != 0f)
			{
				num17 /= num20;
				num18 /= num20;
				num19 /= num20;
			}
			InitialRayStart.X = num11;
			InitialRayStart.Y = num12;
			InitialRayStart.Z = num13;
			InitialRayEnd.X = num14;
			InitialRayEnd.Y = num15;
			InitialRayEnd.Z = num16;
			int num31 = -1;
			bool flag3 = false;
			short returnValueZoneCheckIndex = 0;
			ushort returnValueZoneCheckObjID;
			while (mainC.zonesMain.Check_Zones_For_Point(InitialRayStart.X, InitialRayStart.Y, InitialRayStart.Z, 45f, returnValueZoneCheckIndex, cType, out returnValueZoneCheckIndex, out returnValueZoneCheckObjID))
			{
				int numObjects = Zones.zones[returnValueZoneCheckObjID].zoneList.numObjects;
				for (int j = 0; j < numObjects; j++)
				{
					if (mainC.collisionMain.Check_Polygon_Ray_Collision(Zones.zones[returnValueZoneCheckObjID].zoneList.oList[j], -1, ref InitialRayStart, ref InitialRayEnd, ref Zones.zones[returnValueZoneCheckObjID].zoneList.matrixList[j], out num20, out IntersectPosition, out IntersectNormal, out var Number, out var isStuck, threadID) && (num20 <= num28 || isStuck) && !Ignore(threadID, (ushort)i, Number) && (!isStuck || num28 >= 0f || num20 > num28))
					{
						flag = true;
						flag3 = true;
						num28 = num20;
						num31 = Number;
						num11 = IntersectPosition.X - num17 * 0.001f * 1f;
						num12 = IntersectPosition.Y - num18 * 0.001f * 1f;
						num13 = IntersectPosition.Z - num19 * 0.001f * 1f;
						floatArStatus[threadID, i] = 1;
						floatArDir[threadID, i, 0] = IntersectNormal.X;
						floatArDir[threadID, i, 1] = IntersectNormal.Y;
						floatArDir[threadID, i, 2] = IntersectNormal.Z;
						floatArDir[threadID, i, 3] = num20;
						floatArID[threadID, i] = -1;
						if (isStuck)
						{
							flag2 = true;
						}
					}
				}
			}
			if (flag3)
			{
				AddToIgnoreList(threadID, (ushort)i, num31);
			}
			if (flag)
			{
				num++;
				floatArDir[threadID, i, 4] = num24 + floatAr[threadID, num2] - num11;
				floatArDir[threadID, i, 5] = num25 + floatAr[threadID, num3] - num12;
				floatArDir[threadID, i, 6] = num26 + floatAr[threadID, num4] - num13;
			}
		}
		if (!flag2)
		{
			for (int i = 0; i < numPoints; i++)
			{
				if (floatArDir[threadID, i, 3] > num28)
				{
					floatArID[threadID, i] = -1;
					floatArStatus[threadID, i] = 0;
					ResetParticleIgnoreList(threadID, (ushort)i);
				}
			}
		}
		else
		{
			for (int i = 0; i < numPoints; i++)
			{
				if (floatArDir[threadID, i, 3] >= 0f || floatArDir[threadID, i, 3] < num28)
				{
					floatArID[threadID, i] = -1;
					floatArStatus[threadID, i] = 0;
					ResetParticleIgnoreList(threadID, (ushort)i);
				}
			}
		}
		floatArMovDir[threadID, 0] = 0f;
		for (int i = 0; i < numPoints; i++)
		{
			if (floatArStatus[threadID, i] != 1)
			{
				continue;
			}
			float num32 = floatArDir[threadID, i, 4];
			if (num32 > 0f)
			{
				if (num32 > num8)
				{
					num8 = num32;
				}
			}
			else if (num32 < num5)
			{
				num5 = num32;
			}
			num32 = floatArDir[threadID, i, 5];
			if (num32 > 0f)
			{
				if (num32 > num9)
				{
					num9 = num32;
				}
			}
			else if (num32 < num6)
			{
				num6 = num32;
			}
			num32 = floatArDir[threadID, i, 6];
			if (num32 > 0f)
			{
				if (num32 > num10)
				{
					num10 = num32;
				}
			}
			else if (num32 < num7)
			{
				num7 = num32;
			}
			if (floatArDir[threadID, i, 2] > 0.42f)
			{
				floatArMovDir[threadID, 0] = 1f;
			}
		}
		p1.pos2.v[0] -= num8 + num5;
		p1.pos2.v[1] -= num9 + num6;
		p1.pos2.v[2] -= num10 + num7;
		return num;
	}

	public bool CheckCollision_Detailed_List_Final_Pass(ref StructsClass.particle_list p1, int numPoints, int offset, byte oType, byte cType, bool rotationalCheck, byte threadID)
	{
		bool flag = false;
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
		float num13 = 0f;
		float num14 = 0f;
		float num15 = 0f;
		float num16 = 0f;
		Vector3 InitialRayStart = default(Vector3);
		Vector3 InitialRayEnd = default(Vector3);
		Vector3 IntersectPosition = default(Vector3);
		Vector3 IntersectNormal = default(Vector3);
		float num17 = p1.pos1.v[0];
		float num18 = p1.pos1.v[1];
		float num19 = p1.pos1.v[2];
		float num20 = p1.pos2.v[0];
		float num21 = p1.pos2.v[1];
		float num22 = p1.pos2.v[2];
		int num23 = offset;
		for (int i = 0; i < numPoints; i++)
		{
			floatArStatus[threadID, i] = 0;
			num10 = num20 + floatAr[threadID, num23++];
			num11 = num21 + floatAr[threadID, num23++];
			num12 = num22 + floatAr[threadID, num23++];
			num13 = num10 - num17;
			num14 = num11 - num18;
			num15 = num12 - num19;
			num16 = (float)Math.Sqrt(num13 * num13 + num14 * num14 + num15 * num15);
			flag = false;
			InitialRayStart.X = num17;
			InitialRayStart.Y = num18;
			InitialRayStart.Z = num19;
			InitialRayEnd.X = num10;
			InitialRayEnd.Y = num11;
			InitialRayEnd.Z = num12;
			float num24 = num16 + 1f;
			floatArDir[threadID, i, 3] = num16;
			short returnValueZoneCheckIndex = 0;
			ushort returnValueZoneCheckObjID;
			while (mainC.zonesMain.Check_Zones_For_Point(InitialRayStart.X, InitialRayStart.Y, InitialRayStart.Z, 45f, returnValueZoneCheckIndex, cType, out returnValueZoneCheckIndex, out returnValueZoneCheckObjID))
			{
				int numObjects = Zones.zones[returnValueZoneCheckObjID].zoneList.numObjects;
				for (int j = 0; j < numObjects; j++)
				{
					if (mainC.collisionMain.Check_Polygon_Ray_Collision(Zones.zones[returnValueZoneCheckObjID].zoneList.oList[j], -1, ref InitialRayStart, ref InitialRayEnd, ref Zones.zones[returnValueZoneCheckObjID].zoneList.matrixList[j], out num16, out IntersectPosition, out IntersectNormal, out var _, out var isStuck, threadID) && (num16 <= num24 || isStuck) && ((!isStuck && num16 < num24) || (isStuck && (num16 > num24 || num24 > 0f))))
					{
						flag = true;
						num24 = num16;
						num7 = IntersectPosition.X + IntersectNormal.X * 0.001f * 1f;
						num8 = IntersectPosition.Y + IntersectNormal.Y * 0.001f * 1f;
						num9 = IntersectPosition.Z + IntersectNormal.Z * 0.001f * 1f;
						floatArDir[threadID, i, 4] = IntersectNormal.X;
						floatArDir[threadID, i, 5] = IntersectNormal.Y;
						floatArDir[threadID, i, 6] = IntersectNormal.Z;
						floatArStatus[threadID, i] = 1;
					}
				}
			}
			if (flag)
			{
				float num25 = InitialRayEnd.X - num7;
				float num26 = InitialRayEnd.Y - num8;
				float num27 = InitialRayEnd.Z - num9;
				num24 = num25 * floatArDir[threadID, i, 4] + num26 * floatArDir[threadID, i, 5] + num27 * floatArDir[threadID, i, 6];
				floatArDir[threadID, i, 4] *= num24;
				floatArDir[threadID, i, 5] *= num24;
				floatArDir[threadID, i, 6] *= num24;
			}
		}
		for (int i = 0; i < numPoints; i++)
		{
			if (floatArStatus[threadID, i] != 1)
			{
				continue;
			}
			float num25 = floatArDir[threadID, i, 4];
			if (num25 > 0f)
			{
				if (num25 > num4)
				{
					num4 = num25;
				}
			}
			else if (num25 < num)
			{
				num = num25;
			}
			num25 = floatArDir[threadID, i, 5];
			if (num25 > 0f)
			{
				if (num25 > num5)
				{
					num5 = num25;
				}
			}
			else if (num25 < num2)
			{
				num2 = num25;
			}
			num25 = floatArDir[threadID, i, 6];
			if (num25 > 0f)
			{
				if (num25 > num6)
				{
					num6 = num25;
				}
			}
			else if (num25 < num3)
			{
				num3 = num25;
			}
		}
		if (rotationalCheck && ((num4 != 0f && num != 0f && Math.Sign(num4) != Math.Sign(num)) || (num5 != 0f && num2 != 0f && Math.Sign(num5) != Math.Sign(num2))))
		{
			return false;
		}
		p1.pos2.v[0] -= num4 + num;
		p1.pos2.v[1] -= num5 + num2;
		p1.pos2.v[2] -= num6 + num3;
		return true;
	}

	public int CheckCollision_Detailed_List_Rotational(ref StructsClass.particle_list p1, int numPoints, int offset, byte oType, byte cType, int ignoreID, byte threadID)
	{
		int num = 0;
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
		float num13 = 0f;
		float distance = 0f;
		Vector3 InitialRayStart = default(Vector3);
		Vector3 InitialRayEnd = default(Vector3);
		Vector3 IntersectPosition = default(Vector3);
		Vector3 IntersectNormal = default(Vector3);
		float num14 = p1.pos1.v[0];
		float num15 = p1.pos1.v[1];
		float num16 = p1.pos1.v[2];
		float num17 = p1.pos2.v[0];
		float num18 = p1.pos2.v[1];
		float num19 = p1.pos2.v[2];
		int num20 = 0;
		int num21 = offset;
		for (int i = 0; i < numPoints; i++)
		{
			floatArStatus[threadID, i] = 0;
			floatArDir[threadID, i, 0] = 0f;
			floatArDir[threadID, i, 1] = 0f;
			floatArDir[threadID, i, 2] = 0f;
			num8 = num14 + floatAr[threadID, num20++];
			num9 = num15 + floatAr[threadID, num20++];
			num10 = num16 + floatAr[threadID, num20++];
			num11 = num17 + floatAr[threadID, num21++];
			num12 = num18 + floatAr[threadID, num21++];
			num13 = num19 + floatAr[threadID, num21++];
			InitialRayStart.X = num8;
			InitialRayStart.Y = num9;
			InitialRayStart.Z = num10;
			InitialRayEnd.X = num11;
			InitialRayEnd.Y = num12;
			InitialRayEnd.Z = num13;
			float num22 = num11 - num8;
			float num23 = num12 - num9;
			float num24 = num13 - num10;
			float num25 = num22 * num22 + num23 * num23 + num24 * num24 + 1f;
			bool flag = false;
			short returnValueZoneCheckIndex = 0;
			ushort returnValueZoneCheckObjID;
			while (mainC.zonesMain.Check_Zones_For_Point(InitialRayStart.X, InitialRayStart.Y, InitialRayStart.Z, 45f, returnValueZoneCheckIndex, cType, out returnValueZoneCheckIndex, out returnValueZoneCheckObjID))
			{
				int numObjects = Zones.zones[returnValueZoneCheckObjID].zoneList.numObjects;
				for (int j = 0; j < numObjects; j++)
				{
					if (!mainC.collisionMain.Check_Polygon_Ray_Collision(Zones.zones[returnValueZoneCheckObjID].zoneList.oList[j], -1, ref InitialRayStart, ref InitialRayEnd, ref Zones.zones[returnValueZoneCheckObjID].zoneList.matrixList[j], out distance, out IntersectPosition, out IntersectNormal, out var _, out var isStuck, threadID) || (!((num22 = distance * distance) <= num25) && !isStuck) || (isStuck && !(num25 >= 0f) && !(num22 > num25)))
					{
						continue;
					}
					num++;
					num25 = num22;
					floatArStatus[threadID, i] = 1;
					if (flag)
					{
						num22 = IntersectPosition.X - InitialRayEnd.X;
						num23 = IntersectPosition.Y - InitialRayEnd.Y;
						num24 = IntersectPosition.Z - InitialRayEnd.Z;
						if ((num22 != 0f && floatArDir[threadID, i, 0] != 0f && Math.Sign(num22) != Math.Sign(floatArDir[threadID, i, 0])) || (num23 != 0f && floatArDir[threadID, i, 1] != 0f && Math.Sign(num23) != Math.Sign(floatArDir[threadID, i, 1])) || (num24 != 0f && floatArDir[threadID, i, 2] != 0f && Math.Sign(num24) != Math.Sign(floatArDir[threadID, i, 2])))
						{
							return -1;
						}
					}
					floatArDir[threadID, i, 0] = IntersectPosition.X - InitialRayEnd.X;
					floatArDir[threadID, i, 1] = IntersectPosition.Y - InitialRayEnd.Y;
					floatArDir[threadID, i, 2] = IntersectPosition.Z - InitialRayEnd.Z;
					flag = true;
				}
			}
		}
		num5 = 0f;
		num6 = 0f;
		num7 = 0f;
		num2 = 0f;
		num3 = 0f;
		num4 = 0f;
		for (int i = 0; i < numPoints; i++)
		{
			if (floatArStatus[threadID, i] != 1)
			{
				continue;
			}
			float num22 = floatArDir[threadID, i, 0];
			if (num22 > 0f)
			{
				if (num22 > num5)
				{
					num5 = num22;
				}
			}
			else if (num22 < num2)
			{
				num2 = num22;
			}
			num22 = floatArDir[threadID, i, 1];
			if (num22 > 0f)
			{
				if (num22 > num6)
				{
					num6 = num22;
				}
			}
			else if (num22 < num3)
			{
				num3 = num22;
			}
			num22 = floatArDir[threadID, i, 2];
			if (num22 > 0f)
			{
				if (num22 > num7)
				{
					num7 = num22;
				}
			}
			else if (num22 < num4)
			{
				num4 = num22;
			}
		}
		if (num5 != 0f && num2 != 0f)
		{
			num = -1;
		}
		else
		{
			float num22 = num5 + num2;
			p1.pos2.v[0] += num22 + 0.001f * (float)Math.Sign(num22);
		}
		if (num6 != 0f && num3 != 0f)
		{
			num = -1;
		}
		else
		{
			float num22 = num6 + num3;
			p1.pos2.v[1] += num22 + 0.001f * (float)Math.Sign(num22);
		}
		if (num7 != 0f && num4 != 0f)
		{
			num = -1;
		}
		else
		{
			float num22 = num7 + num4;
			p1.pos2.v[2] += num22 + 0.001f * (float)Math.Sign(num22);
		}
		return num;
	}

	public int CheckCollision_Detailed_List_Single(ref StructsClass.Object_Position p1, byte cType, byte threadID)
	{
		int num = 0;
		int num2 = 0;
		float num3 = 0f;
		float num4 = 0f;
		float num5 = 0f;
		float num6 = 0f;
		float num7 = 0f;
		float num8 = 0f;
		Vector3 InitialRayStart = default(Vector3);
		Vector3 InitialRayEnd = default(Vector3);
		Vector3 IntersectPosition = default(Vector3);
		Vector3 IntersectNormal = default(Vector3);
		colIDT[threadID] = -1L;
		float num9 = maxDistanceSqr;
		float num10 = num9;
		float num11 = p1.x2 - p1.x1;
		float num12 = p1.y2 - p1.y1;
		float num13 = p1.z2 - p1.z1;
		float num14 = (float)Math.Sqrt(num11 * num11 + num12 * num12 + num13 * num13);
		if (num14 != 0f)
		{
			num11 /= num14;
			num12 /= num14;
			num13 /= num14;
		}
		p1.length = num14;
		num = maxItemsInABox * 2;
		if (cbprVpCnt < num)
		{
			cbprVpCnt = num;
			cbprVp = new StructsClass.vtex[4, num];
			cbprVxyz = new StructsClass.vtex[4, num];
			cbprVxyz2 = new StructsClass.vtex[4, num];
			for (int i = 0; i < num; i++)
			{
				cbprVp[0, i] = new StructsClass.vtex();
				cbprVp[1, i] = new StructsClass.vtex();
				cbprVp[2, i] = new StructsClass.vtex();
				cbprVp[3, i] = new StructsClass.vtex();
				cbprVxyz[0, i] = new StructsClass.vtex();
				cbprVxyz[1, i] = new StructsClass.vtex();
				cbprVxyz[2, i] = new StructsClass.vtex();
				cbprVxyz[3, i] = new StructsClass.vtex();
				cbprVxyz2[0, i] = new StructsClass.vtex();
				cbprVxyz2[1, i] = new StructsClass.vtex();
				cbprVxyz2[2, i] = new StructsClass.vtex();
				cbprVxyz2[3, i] = new StructsClass.vtex();
			}
			cbprCntPtr = new float[4, num];
			cbprLastSide = new byte[4, num];
			cbprSide = new byte[4, num];
		}
		floatArStatus[threadID, 0] = 0;
		floatArID[threadID, 0] = -1;
		floatArDir[threadID, 0, 3] = num14;
		num3 = p1.x1;
		num4 = p1.y1;
		num5 = p1.z1;
		num6 = p1.x1 + num11 * num14;
		num7 = p1.y1 + num12 * num14;
		num8 = p1.z1 + num13 * num14;
		colIDT[threadID] = -1L;
		InitialRayStart.X = p1.x1;
		InitialRayStart.Y = p1.y1;
		InitialRayStart.Z = p1.z1;
		InitialRayEnd.X = num6;
		InitialRayEnd.Y = num7;
		InitialRayEnd.Z = num8;
		num3 = num6;
		num4 = num7;
		num5 = num8;
		short returnValueZoneCheckIndex = 0;
		ushort returnValueZoneCheckObjID;
		while (mainC.zonesMain.Check_Zones_For_Point(InitialRayStart.X, InitialRayStart.Y, InitialRayStart.Z, 4.5f, returnValueZoneCheckIndex, cType, out returnValueZoneCheckIndex, out returnValueZoneCheckObjID))
		{
			int numObjects = Zones.zones[returnValueZoneCheckObjID].zoneList.numObjects;
			for (int j = 0; j < numObjects; j++)
			{
				if (mainC.collisionMain.Check_Polygon_Ray_Collision_Projectile(Zones.zones[returnValueZoneCheckObjID].zoneList.oList[j], -1, ref InitialRayStart, ref InitialRayEnd, ref Zones.zones[returnValueZoneCheckObjID].zoneList.matrixList[j], out var distance, out IntersectPosition, out IntersectNormal, out var _, threadID) && distance <= num10 && (num10 >= 0f || distance < num10))
				{
					num2 = 1;
					num10 = distance;
					num3 = IntersectPosition.X + IntersectNormal.X * 0.001f;
					num4 = IntersectPosition.Y + IntersectNormal.Y * 0.001f;
					num5 = IntersectPosition.Z + IntersectNormal.Z * 0.001f;
					floatArStatus[threadID, 0] = 1;
					floatArDir[threadID, 0, 0] = IntersectNormal.X;
					floatArDir[threadID, 0, 1] = IntersectNormal.Y;
					floatArDir[threadID, 0, 2] = IntersectNormal.Z;
					floatArDir[threadID, 0, 3] = num14;
					floatArDir[threadID, 0, 4] = IntersectPosition.X;
					floatArDir[threadID, 0, 5] = IntersectPosition.Y;
					floatArDir[threadID, 0, 6] = IntersectPosition.Z;
					floatArID[threadID, 0] = -1;
					InitialRayEnd.X = num3;
					InitialRayEnd.Y = num4;
					InitialRayEnd.Z = num5;
				}
			}
		}
		p1.x2 = num3;
		p1.y2 = num4;
		p1.z2 = num5;
		if (num2 > 0)
		{
			p1.length = num10;
		}
		return num2;
	}

	public int CheckRayCollision_Threaded(ref float x1, ref float y1, ref float z1, float Vx, float Vy, float Vz, float length, long objLastID, byte oType, ushort particleID, byte threadID)
	{
		bool moveToNewBoxX = false;
		bool moveToNewBoxY = false;
		bool moveToNewBoxZ = false;
		float num = 0f;
		long num2 = 0L;
		float num3 = mainC.terrainMain.Get_Terrain_Height(x1, y1, threadID);
		if (num3 < global::MainGame.MainGame.MaxDown)
		{
			num3 = global::MainGame.MainGame.MaxDown;
		}
		float num4;
		if (z1 < num3)
		{
			cbprCntPtr[threadID, 0] = 0f;
			num4 = 1f;
			if (Vz != 0f)
			{
				num4 = Math.Abs((num3 - z1) / Vz);
				cbprCntPtr[threadID, 0] = 0f - (length - num4);
			}
			z1 = num3;
			if (Vz != 0f)
			{
				cbprVxyz[threadID, 0].v[0] = Vx;
				cbprVxyz[threadID, 0].v[1] = Vy;
				cbprVxyz[threadID, 0].v[2] = 0f;
				cbprVxyz2[threadID, 0].v[0] = 0f;
				cbprVxyz2[threadID, 0].v[1] = 0f;
				cbprVxyz2[threadID, 0].v[2] = -1f;
				colIDT[threadID] = -1L;
				hitGround = true;
				return 12;
			}
		}
		num4 = length;
		float xStart = x1;
		float yStart = y1;
		float zStart = z1;
		while (num4 > 0f)
		{
			int num5 = CheckBoxParticleRay_Threaded(ref xStart, ref yStart, ref zStart, ref num4, Vx, Vy, Vz, objLastID, ref moveToNewBoxX, ref moveToNewBoxY, ref moveToNewBoxZ, oType, particleID, threadID);
			if (num5 > 0)
			{
				cbprCntPtr[threadID, 0] += num;
				x1 = xStart;
				y1 = yStart;
				z1 = zStart;
				return num5;
			}
			num = length - num4;
			num2++;
			if (num2 > 1000)
			{
				num4 = 0f;
			}
		}
		if (zStart < num3)
		{
			if (Vz != 0f)
			{
				cbprVxyz[threadID, 0].v[0] = Vx;
				cbprVxyz[threadID, 0].v[1] = Vy;
				cbprVxyz[threadID, 0].v[2] = 0f;
				cbprVxyz2[threadID, 0].v[0] = 0f;
				cbprVxyz2[threadID, 0].v[1] = 0f;
				cbprVxyz2[threadID, 0].v[2] = -1f;
				num4 = 0f;
				cbprCntPtr[threadID, 0] = 0f;
				if (Vz != 0f)
				{
					num4 = Math.Abs((num3 - zStart) / Vz);
					cbprCntPtr[threadID, 0] = length - num4;
				}
				z1 = num3;
				Vx *= num4;
				Vy *= num4;
				x1 = xStart - Vx;
				y1 = yStart - Vy;
				colIDT[threadID] = -1L;
				hitGround = true;
				return 4;
			}
			num4 = 1f;
			if (Vz != 0f)
			{
				num4 = Math.Abs((num3 - zStart) / Vz);
			}
			z1 = num3;
		}
		x1 = xStart;
		y1 = yStart;
		z1 = zStart;
		return 0;
	}

	public int CheckBoxParticleRay_Threaded(ref float xStart, ref float yStart, ref float zStart, ref float len, float Vx, float Vy, float Vz, long objLastID, ref bool moveToNewBoxX, ref bool moveToNewBoxY, ref bool moveToNewBoxZ, byte oType, ushort particleID, byte threadID)
	{
		sbyte b = 1;
		sbyte b2 = 1;
		sbyte b3 = 1;
		sbyte b4 = 0;
		sbyte b5 = 0;
		sbyte b6 = 0;
		bool flag = false;
		byte b7 = 0;
		float num = 0f;
		float num2 = 0f;
		float num3 = 0f;
		long num4 = (long)((xStart + 0.5f + BoxX) / (float)BoxSize);
		long num5 = (long)((yStart + 0.5f + BoxY) / (float)BoxSize);
		long num6 = (long)((zStart + 0.5f + BoxZ) / (float)BoxSize);
		float num7;
		if (Vx > 0f)
		{
			num7 = (float)BoxSize - (xStart + 0.5f + BoxX - (float)(num4 * BoxSize));
			num7 /= Vx;
		}
		else if (Vx < 0f)
		{
			num7 = xStart + 0.5f + BoxX - (float)(num4 * BoxSize);
			num7 /= 0f - Vx;
			if (num7 == 0f)
			{
				if (num4 < 1)
				{
					len = 0f;
				}
				else if (moveToNewBoxX)
				{
					num4--;
					num7 = xStart + 0.5f + BoxX - (float)(num4 * BoxSize);
					num7 /= 0f - Vx;
				}
				else
				{
					flag = true;
				}
				moveToNewBoxX = true;
			}
		}
		else
		{
			num7 = len;
		}
		float num8;
		if (Vy > 0f)
		{
			num8 = (float)BoxSize - (yStart + 0.5f + BoxY - (float)(num5 * BoxSize));
			num8 /= Vy;
		}
		else if (Vy < 0f)
		{
			num8 = yStart + 0.5f + BoxY - (float)(num5 * BoxSize);
			num8 /= 0f - Vy;
			if (num8 == 0f)
			{
				if (num5 < 1)
				{
					len = 0f;
				}
				else if (moveToNewBoxY)
				{
					num5--;
					num8 = yStart + 0.5f + BoxY - (float)(num5 * BoxSize);
					num8 /= 0f - Vy;
				}
				else
				{
					flag = true;
				}
				moveToNewBoxY = true;
			}
		}
		else
		{
			num8 = len;
		}
		float num9;
		if (Vz > 0f)
		{
			num9 = (float)BoxSize - (zStart + 0.5f + BoxZ - (float)(num6 * BoxSize));
			num9 /= Vz;
		}
		else if (Vz < 0f)
		{
			num9 = zStart + 0.5f + BoxZ - (float)(num6 * BoxSize);
			num9 /= 0f - Vz;
			if (num9 == 0f)
			{
				if (num6 < 1)
				{
					len = 0f;
				}
				else if (moveToNewBoxZ)
				{
					num6--;
					num9 = zStart + 0.5f + BoxZ - (float)(num6 * BoxSize);
					num9 /= 0f - Vz;
				}
				else
				{
					flag = true;
				}
				moveToNewBoxZ = true;
			}
		}
		else
		{
			num9 = len;
		}
		float num10 = (flag ? 0f : ((num7 < num8 && num7 < num9 && num7 > 0f) ? num7 : ((!(num8 < num9) || !(num8 > 0f)) ? num9 : num8)));
		num10 = ((!(num10 > len)) ? Math.Abs(num10) : len);
		long num11 = num6 + num5 * BoxDimZ + num4 * BoxDimZ * BoxDimY;
		if (num11 < 0 || num11 >= numBoxes)
		{
			len = 0f;
			return 0;
		}
		if (mainBox[num11].numObjects < 1)
		{
			len -= num10;
			xStart += Vx * num10;
			yStart += Vy * num10;
			zStart += Vz * num10;
			return 0;
		}
		long num12 = mainBox[num11].cnt * 2;
		long num13;
		for (num13 = 0L; num13 < num12; num13++)
		{
			cbprCntPtr[(int)checked((nint)unchecked((long)threadID)), (int)checked((nint)num13)] = -1f;
			cbprSide[(int)checked((nint)unchecked((long)threadID)), (int)checked((nint)num13)] = 0;
		}
		float num16;
		float num17;
		float num18;
		float num22;
		long num23;
		long num14;
		for (num13 = 0L; num13 < mainBox[num11].cnt; num13++)
		{
			num14 = mainBox[num11].oList[num13];
			if (num14 > -1 && (global::GameObjects.GameObjects.objMaster[num14].type & oType) > 128 && num14 != objLastID && !Ignore(threadID, particleID, num14))
			{
				bool flag3;
				bool flag4;
				bool flag2 = (flag3 = (flag4 = false));
				float num15 = 0.5f;
				if ((global::GameObjects.GameObjects.objStat[num14] & 0x18) > 0)
				{
					num15 = 0f;
				}
				b = (b2 = (b3 = 1));
				if (global::GameObjects.GameObjects.objMaster[num14].isRotated)
				{
					Matrix mvT = global::GameObjects.GameObjects.objMaster[num14].mvT;
					num = (xStart - global::GameObjects.GameObjects.objMaster[num14].x) * mvT.M11 + (yStart - global::GameObjects.GameObjects.objMaster[num14].y) * mvT.M21 + (zStart - global::GameObjects.GameObjects.objMaster[num14].z) * mvT.M31;
					num2 = (xStart - global::GameObjects.GameObjects.objMaster[num14].x) * mvT.M12 + (yStart - global::GameObjects.GameObjects.objMaster[num14].y) * mvT.M22 + (zStart - global::GameObjects.GameObjects.objMaster[num14].z) * mvT.M32;
					num3 = (xStart - global::GameObjects.GameObjects.objMaster[num14].x) * mvT.M13 + (yStart - global::GameObjects.GameObjects.objMaster[num14].y) * mvT.M23 + (zStart - global::GameObjects.GameObjects.objMaster[num14].z) * mvT.M33;
					num += num15;
					num2 += num15;
					num3 += num15;
					num16 = Vx * mvT.M11 + Vy * mvT.M21 + Vz * mvT.M31;
					num17 = Vx * mvT.M12 + Vy * mvT.M22 + Vz * mvT.M32;
					num18 = Vx * mvT.M13 + Vy * mvT.M23 + Vz * mvT.M33;
					if (num16 < 0f)
					{
						b = -1;
					}
					if (num17 < 0f)
					{
						b2 = -1;
					}
					if (num18 < 0f)
					{
						b3 = -1;
					}
				}
				else
				{
					num = xStart - global::GameObjects.GameObjects.objMaster[num14].x + num15;
					num2 = yStart - global::GameObjects.GameObjects.objMaster[num14].y + num15;
					num3 = zStart - global::GameObjects.GameObjects.objMaster[num14].z + num15;
					num16 = Vx;
					num17 = Vy;
					num18 = Vz;
					if (Vx < 0f)
					{
						b = -1;
					}
					if (Vy < 0f)
					{
						b2 = -1;
					}
					if (Vz < 0f)
					{
						b3 = -1;
					}
				}
				long num19 = global::GameObjects.GameObjects.objMaster[num14].dimX;
				long num20 = global::GameObjects.GameObjects.objMaster[num14].dimY;
				long num21 = global::GameObjects.GameObjects.objMaster[num14].dimZ;
				b7 = 8;
				num22 = num10;
				if ((num <= 0f && num16 <= 0f) || (num >= (float)num19 && num16 >= 0f))
				{
					num22 = -1f;
				}
				else if ((num2 <= 0f && num17 <= 0f) || (num2 >= (float)num20 && num17 >= 0f))
				{
					num22 = -1f;
				}
				else if ((num3 <= 0f && num18 <= 0f) || (num3 >= (float)num21 && num18 >= 0f))
				{
					num22 = -1f;
				}
				if (num22 == -1f)
				{
					continue;
				}
				num23 = 0L;
				if (num23 >= 1)
				{
					continue;
				}
				if (num16 != 0f)
				{
					if (num < 0f || (num == 0f && num16 > 0f))
					{
						float num24 = (0f - num) / num16;
						num22 -= num24;
						if (num22 < 0f)
						{
							continue;
						}
						num = 0f;
						num2 += num17 * num24;
						num3 += num18 * num24;
						b4 = 0;
						flag2 = true;
					}
					else if (num > (float)num19 || (num == (float)num19 && num16 < 0f))
					{
						float num24 = ((float)num19 - num) / num16;
						num22 -= num24;
						if (num22 < 0f)
						{
							continue;
						}
						num = num19;
						num2 += num17 * num24;
						num3 += num18 * num24;
						b4 = b;
						flag2 = true;
					}
				}
				if (num17 != 0f)
				{
					if (num2 < 0f || (num2 == 0f && num17 > 0f))
					{
						float num24 = (0f - num2) / num17;
						num22 -= num24;
						if (num22 < 0f || num24 < 0f)
						{
							continue;
						}
						num += num16 * num24;
						num2 = 0f;
						num3 += num18 * num24;
						flag3 = true;
						b5 = 0;
						flag2 = false;
					}
					else if (num2 > (float)num20 || (num2 == (float)num20 && num17 < 0f))
					{
						float num24 = ((float)num20 - num2) / num17;
						num22 -= num24;
						if (num22 < 0f || num24 < 0f)
						{
							continue;
						}
						num += num16 * num24;
						num2 = num20;
						num3 += num18 * num24;
						b5 = b2;
						flag3 = true;
						flag2 = false;
					}
				}
				if (num18 != 0f)
				{
					if (num3 < 0f || (num3 == 0f && num18 > 0f))
					{
						float num24 = (0f - num3) / num18;
						num22 -= num24;
						if (num22 < 0f || num24 < 0f)
						{
							continue;
						}
						num += num16 * num24;
						num2 += num17 * num24;
						num3 = 0f;
						b6 = 0;
						flag4 = true;
						flag2 = false;
						flag3 = false;
					}
					else if (num3 > (float)num21 || (num3 == (float)num21 && num18 < 0f))
					{
						float num24 = ((float)num21 - num3) / num18;
						num22 -= num24;
						if (num22 < 0f || num24 < 0f)
						{
							continue;
						}
						num += num16 * num24;
						num2 += num17 * num24;
						num3 = num21;
						b6 = b3;
						flag4 = true;
						flag2 = false;
						flag3 = false;
					}
				}
				do
				{
					long num25 = (long)num;
					long num26 = (long)num2;
					long num27 = (long)num3;
					if (flag2)
					{
						num25 += b4;
					}
					else if (flag3)
					{
						num26 += b5;
					}
					else if (flag4)
					{
						num27 += b6;
					}
					if (num25 >= 0 && num25 < num19 && num26 >= 0 && num26 < num20 && num27 >= 0 && num27 < num21 && (!global::GameObjects.GameObjects.objMaster[num14].destructable || global::GameObjects.GameObjects.objMaster[num14].pt1[num27 + num26 * num21 + num25 * num21 * num20].status != 0))
					{
						cbprCntPtr[(int)checked((nint)unchecked((long)threadID)), (int)checked((nint)num13)] = num10 - num22;
						if (flag2)
						{
							cbprLastSide[(int)checked((nint)unchecked((long)threadID)), (int)checked((nint)num13)] = 1;
							cbprSide[(int)checked((nint)unchecked((long)threadID)), (int)checked((nint)num13)] = 1;
						}
						else if (flag3)
						{
							cbprLastSide[(int)checked((nint)unchecked((long)threadID)), (int)checked((nint)num13)] = 2;
							cbprSide[(int)checked((nint)unchecked((long)threadID)), (int)checked((nint)num13)] = 2;
						}
						else if (flag4)
						{
							cbprLastSide[(int)checked((nint)unchecked((long)threadID)), (int)checked((nint)num13)] = 4;
							cbprSide[(int)checked((nint)unchecked((long)threadID)), (int)checked((nint)num13)] = 4;
						}
						else
						{
							num7 = (float)num19 - num;
							if (num < num7)
							{
								num7 = num;
							}
							num8 = (float)num20 - num2;
							if (num2 < num8)
							{
								num8 = num2;
							}
							num9 = (float)num21 - num3;
							if (num3 < num9)
							{
								num9 = num3;
							}
							b7 = 1;
							if (num8 < num7 && num8 < num9)
							{
								b7 = 2;
							}
							else if (num9 < num7)
							{
								b7 = 4;
							}
							switch (b7)
							{
							case 1:
								if (num <= num7)
								{
									cbprCntPtr[(int)checked((nint)unchecked((long)threadID)), (int)checked((nint)num13)] = 0f - num;
									num16 = Math.Abs(num16);
									num = 0f;
								}
								else
								{
									cbprCntPtr[(int)checked((nint)unchecked((long)threadID)), (int)checked((nint)num13)] = 0f - num7;
									num16 = 0f - Math.Abs(num16);
									num = num19;
								}
								cbprLastSide[(int)checked((nint)unchecked((long)threadID)), (int)checked((nint)num13)] = 1;
								cbprSide[(int)checked((nint)unchecked((long)threadID)), (int)checked((nint)num13)] = 9;
								break;
							case 2:
								if (num2 <= num8)
								{
									cbprCntPtr[(int)checked((nint)unchecked((long)threadID)), (int)checked((nint)num13)] = 0f - num2;
									num17 = Math.Abs(num17);
									num2 = 0f;
								}
								else
								{
									cbprCntPtr[(int)checked((nint)unchecked((long)threadID)), (int)checked((nint)num13)] = 0f - num8;
									num17 = 0f - Math.Abs(num17);
									num2 = num20;
								}
								cbprLastSide[(int)checked((nint)unchecked((long)threadID)), (int)checked((nint)num13)] = 2;
								cbprSide[(int)checked((nint)unchecked((long)threadID)), (int)checked((nint)num13)] = 10;
								break;
							case 4:
								if (num3 <= num9)
								{
									cbprCntPtr[(int)checked((nint)unchecked((long)threadID)), (int)checked((nint)num13)] = 0f - num3;
									num18 = Math.Abs(num18);
									num3 = 0f;
								}
								else
								{
									cbprCntPtr[(int)checked((nint)unchecked((long)threadID)), (int)checked((nint)num13)] = 0f - num9;
									num18 = 0f - Math.Abs(num18);
									num3 = num21;
								}
								cbprLastSide[(int)checked((nint)unchecked((long)threadID)), (int)checked((nint)num13)] = 4;
								cbprSide[(int)checked((nint)unchecked((long)threadID)), (int)checked((nint)num13)] = 12;
								break;
							}
						}
						float num24 = 1f;
						if (global::GameObjects.GameObjects.objMaster[num14].isRotated)
						{
							Matrix mvT = global::GameObjects.GameObjects.objMaster[num14].mv;
							num -= num15;
							num2 -= num15;
							num3 -= num15;
							cbprVp[(int)checked((nint)unchecked((long)threadID)), (int)checked((nint)num13)].v[0] = num * mvT.M11 + num2 * mvT.M21 + num3 * mvT.M31 + global::GameObjects.GameObjects.objMaster[num14].x;
							cbprVp[(int)checked((nint)unchecked((long)threadID)), (int)checked((nint)num13)].v[1] = num * mvT.M12 + num2 * mvT.M22 + num3 * mvT.M32 + global::GameObjects.GameObjects.objMaster[num14].y;
							cbprVp[(int)checked((nint)unchecked((long)threadID)), (int)checked((nint)num13)].v[2] = num * mvT.M13 + num2 * mvT.M23 + num3 * mvT.M33 + global::GameObjects.GameObjects.objMaster[num14].z;
							switch (cbprSide[(int)checked((nint)unchecked((long)threadID)), (int)checked((nint)num13)])
							{
							case 1:
								if (num16 < 0f)
								{
									num24 = -1f;
								}
								cbprVxyz2[(int)checked((nint)unchecked((long)threadID)), (int)checked((nint)num13)].v[0] = num24 * mvT.M11;
								cbprVxyz2[(int)checked((nint)unchecked((long)threadID)), (int)checked((nint)num13)].v[1] = num24 * mvT.M12;
								cbprVxyz2[(int)checked((nint)unchecked((long)threadID)), (int)checked((nint)num13)].v[2] = num24 * mvT.M13;
								num16 = 0f;
								break;
							case 2:
								if (num17 < 0f)
								{
									num24 = -1f;
								}
								cbprVxyz2[(int)checked((nint)unchecked((long)threadID)), (int)checked((nint)num13)].v[0] = num24 * mvT.M21;
								cbprVxyz2[(int)checked((nint)unchecked((long)threadID)), (int)checked((nint)num13)].v[1] = num24 * mvT.M22;
								cbprVxyz2[(int)checked((nint)unchecked((long)threadID)), (int)checked((nint)num13)].v[2] = num24 * mvT.M23;
								num17 = 0f;
								break;
							case 4:
								if (num18 < 0f)
								{
									num24 = -1f;
								}
								cbprVxyz2[(int)checked((nint)unchecked((long)threadID)), (int)checked((nint)num13)].v[0] = num24 * mvT.M31;
								cbprVxyz2[(int)checked((nint)unchecked((long)threadID)), (int)checked((nint)num13)].v[1] = num24 * mvT.M32;
								cbprVxyz2[(int)checked((nint)unchecked((long)threadID)), (int)checked((nint)num13)].v[2] = num24 * mvT.M33;
								num18 = 0f;
								break;
							}
							cbprVxyz[(int)checked((nint)unchecked((long)threadID)), (int)checked((nint)num13)].v[0] = num16 * mvT.M11 + num17 * mvT.M21 + num18 * mvT.M31;
							cbprVxyz[(int)checked((nint)unchecked((long)threadID)), (int)checked((nint)num13)].v[1] = num16 * mvT.M12 + num17 * mvT.M22 + num18 * mvT.M32;
							cbprVxyz[(int)checked((nint)unchecked((long)threadID)), (int)checked((nint)num13)].v[2] = num16 * mvT.M13 + num17 * mvT.M23 + num18 * mvT.M33;
							break;
						}
						cbprVp[(int)checked((nint)unchecked((long)threadID)), (int)checked((nint)num13)].v[0] = num + global::GameObjects.GameObjects.objMaster[num14].x - num15;
						cbprVp[(int)checked((nint)unchecked((long)threadID)), (int)checked((nint)num13)].v[1] = num2 + global::GameObjects.GameObjects.objMaster[num14].y - num15;
						cbprVp[(int)checked((nint)unchecked((long)threadID)), (int)checked((nint)num13)].v[2] = num3 + global::GameObjects.GameObjects.objMaster[num14].z - num15;
						switch (cbprSide[(int)checked((nint)unchecked((long)threadID)), (int)checked((nint)num13)])
						{
						case 1:
							if (num16 < 0f)
							{
								num24 = -1f;
							}
							cbprVxyz2[(int)checked((nint)unchecked((long)threadID)), (int)checked((nint)num13)].v[0] = num24;
							cbprVxyz2[(int)checked((nint)unchecked((long)threadID)), (int)checked((nint)num13)].v[1] = 0f;
							cbprVxyz2[(int)checked((nint)unchecked((long)threadID)), (int)checked((nint)num13)].v[2] = 0f;
							num16 = 0f;
							break;
						case 2:
							if (num17 < 0f)
							{
								num24 = -1f;
							}
							cbprVxyz2[(int)checked((nint)unchecked((long)threadID)), (int)checked((nint)num13)].v[0] = 0f;
							cbprVxyz2[(int)checked((nint)unchecked((long)threadID)), (int)checked((nint)num13)].v[1] = num24;
							cbprVxyz2[(int)checked((nint)unchecked((long)threadID)), (int)checked((nint)num13)].v[2] = 0f;
							num17 = 0f;
							break;
						case 4:
							if (num18 < 0f)
							{
								num24 = -1f;
							}
							cbprVxyz2[(int)checked((nint)unchecked((long)threadID)), (int)checked((nint)num13)].v[0] = 0f;
							cbprVxyz2[(int)checked((nint)unchecked((long)threadID)), (int)checked((nint)num13)].v[1] = 0f;
							cbprVxyz2[(int)checked((nint)unchecked((long)threadID)), (int)checked((nint)num13)].v[2] = num24;
							num18 = 0f;
							break;
						}
						cbprVxyz[(int)checked((nint)unchecked((long)threadID)), (int)checked((nint)num13)].v[0] = num16;
						cbprVxyz[(int)checked((nint)unchecked((long)threadID)), (int)checked((nint)num13)].v[1] = num17;
						cbprVxyz[(int)checked((nint)unchecked((long)threadID)), (int)checked((nint)num13)].v[2] = num18;
						break;
					}
					if (num22 >= 1f)
					{
						num += num16;
						num2 += num17;
						num3 += num18;
					}
					else
					{
						num += num16 * num22;
						num2 += num17 * num22;
						num3 += num18 * num22;
					}
				}
				while (num22-- > 0f);
			}
			else if (num14 < 0)
			{
				num13 = mainBox[num11].cnt;
			}
		}
		num = xStart;
		num2 = yStart;
		num3 = zStart;
		num16 = Vx;
		num17 = Vy;
		num18 = Vz;
		b7 = 8;
		float num28 = mainC.terrainMain.Get_Terrain_Height(xStart, yStart, threadID);
		if (num28 < global::MainGame.MainGame.MaxDown)
		{
			num28 = global::MainGame.MainGame.MaxDown;
		}
		if (num3 >= num28 && num18 < 0f)
		{
			float num24 = (num28 - num3) / num18;
			num22 = num10 - num24;
			if (num22 >= 0f)
			{
				num13 = mainBox[num11].cnt;
				num3 = num28;
				cbprCntPtr[(int)checked((nint)unchecked((long)threadID)), (int)checked((nint)num13)] = num10 - num22;
				cbprLastSide[(int)checked((nint)unchecked((long)threadID)), (int)checked((nint)num13)] = 4;
				cbprSide[(int)checked((nint)unchecked((long)threadID)), (int)checked((nint)num13)] = 4;
				cbprVp[(int)checked((nint)unchecked((long)threadID)), (int)checked((nint)num13)].v[0] = num + num16 * num24;
				cbprVp[(int)checked((nint)unchecked((long)threadID)), (int)checked((nint)num13)].v[1] = num2 + num17 * num24;
				cbprVp[(int)checked((nint)unchecked((long)threadID)), (int)checked((nint)num13)].v[2] = num3;
				cbprVxyz2[(int)checked((nint)unchecked((long)threadID)), (int)checked((nint)num13)].v[0] = 0f;
				cbprVxyz2[(int)checked((nint)unchecked((long)threadID)), (int)checked((nint)num13)].v[1] = 0f;
				cbprVxyz2[(int)checked((nint)unchecked((long)threadID)), (int)checked((nint)num13)].v[2] = -1f;
				cbprVxyz[(int)checked((nint)unchecked((long)threadID)), (int)checked((nint)num13)].v[0] = num16;
				cbprVxyz[(int)checked((nint)unchecked((long)threadID)), (int)checked((nint)num13)].v[1] = num17;
				cbprVxyz[(int)checked((nint)unchecked((long)threadID)), (int)checked((nint)num13)].v[2] = 0f;
			}
		}
		num22 = num10 + 2f;
		num14 = -1L;
		num23 = mainBox[num11].cnt;
		for (num13 = 0L; num13 < num23; num13++)
		{
			if (cbprSide[(int)checked((nint)unchecked((long)threadID)), (int)checked((nint)num13)] > 0 && cbprCntPtr[(int)checked((nint)unchecked((long)threadID)), (int)checked((nint)num13)] < num22)
			{
				num22 = cbprCntPtr[(int)checked((nint)unchecked((long)threadID)), (int)checked((nint)num13)];
				num14 = mainBox[num11].oList[num13];
				if (num14 < 0)
				{
					num14 = 0L;
					return 0;
				}
				colPtrT[threadID] = global::GameObjects.GameObjects.objMaster[num14];
				colIDT[threadID] = num14;
				num = cbprVp[(int)checked((nint)unchecked((long)threadID)), (int)checked((nint)num13)].v[0];
				num2 = cbprVp[(int)checked((nint)unchecked((long)threadID)), (int)checked((nint)num13)].v[1];
				num3 = cbprVp[(int)checked((nint)unchecked((long)threadID)), (int)checked((nint)num13)].v[2];
				b7 = cbprSide[(int)checked((nint)unchecked((long)threadID)), (int)checked((nint)num13)];
				num14 = num13;
			}
		}
		if (cbprCntPtr[(int)checked((nint)unchecked((long)threadID)), (int)checked((nint)num13)] >= 0f && cbprCntPtr[(int)checked((nint)unchecked((long)threadID)), (int)checked((nint)num13)] < num22)
		{
			num22 = cbprCntPtr[(int)checked((nint)unchecked((long)threadID)), (int)checked((nint)num13)];
			colIDT[threadID] = -1L;
			num = cbprVp[(int)checked((nint)unchecked((long)threadID)), (int)checked((nint)num13)].v[0];
			num2 = cbprVp[(int)checked((nint)unchecked((long)threadID)), (int)checked((nint)num13)].v[1];
			num3 = cbprVp[(int)checked((nint)unchecked((long)threadID)), (int)checked((nint)num13)].v[2];
			b7 = cbprSide[(int)checked((nint)unchecked((long)threadID)), (int)checked((nint)num13)];
			num14 = num13;
			hitGround = true;
		}
		if (num14 > -1)
		{
			num28 = mainC.terrainMain.Get_Terrain_Height(num, num2, threadID);
			if (num28 < global::MainGame.MainGame.MaxDown)
			{
				num28 = global::MainGame.MainGame.MaxDown;
			}
			if (num3 < num28)
			{
				num3 = num28;
				cbprVxyz[threadID, 0].v[0] = 0f;
				cbprVxyz[threadID, 0].v[1] = 0f;
				cbprVxyz[threadID, 0].v[2] = 0f;
				num22 = len;
			}
			xStart = num;
			yStart = num2;
			zStart = num3;
			cbprVxyz[threadID, 0].v[0] = cbprVxyz[(int)checked((nint)unchecked((long)threadID)), (int)checked((nint)num14)].v[0];
			cbprVxyz[threadID, 0].v[1] = cbprVxyz[(int)checked((nint)unchecked((long)threadID)), (int)checked((nint)num14)].v[1];
			cbprVxyz[threadID, 0].v[2] = cbprVxyz[(int)checked((nint)unchecked((long)threadID)), (int)checked((nint)num14)].v[2];
			cbprVxyz2[threadID, 0].v[0] = cbprVxyz2[(int)checked((nint)unchecked((long)threadID)), (int)checked((nint)num14)].v[0];
			cbprVxyz2[threadID, 0].v[1] = cbprVxyz2[(int)checked((nint)unchecked((long)threadID)), (int)checked((nint)num14)].v[1];
			cbprVxyz2[threadID, 0].v[2] = cbprVxyz2[(int)checked((nint)unchecked((long)threadID)), (int)checked((nint)num14)].v[2];
			cbprCntPtr[threadID, 0] = num22;
			return b7;
		}
		len -= num10;
		xStart += Vx * num10;
		yStart += Vy * num10;
		zStart += Vz * num10;
		return 0;
	}

	public short Check_Collision_With_BoundingBox(ref StructsClass.particle_list p1, ref StructsClass.vtex[] bb1, ref StructsClass.vtex[] bb2, byte threadID)
	{
		byte b = 0;
		byte b2 = 0;
		float num = 0f;
		float num2 = 0f;
		float num3 = 0f;
		float num4 = 0f;
		float num5 = 0f;
		float num6 = 0f;
		float num7;
		num = (num7 = bb1[threadID].v[0]);
		float num8 = bb2[threadID].v[0];
		float num9;
		num2 = (num9 = bb1[threadID].v[1]);
		float num10 = bb2[threadID].v[1];
		float num11;
		num3 = (num11 = bb1[threadID].v[2]);
		float num12 = bb2[threadID].v[2];
		float num13 = p1.pos2.v[0] - p1.pos1.v[0];
		float num14 = p1.pos2.v[1] - p1.pos1.v[1];
		float num15 = p1.pos2.v[2] - p1.pos1.v[2];
		float num16 = (float)Math.Sqrt(num13 * num13 + num14 * num14 + num15 * num15);
		num13 /= num16;
		num14 /= num16;
		num15 /= num16;
		if (num13 == 0f)
		{
			b2 = 1;
		}
		if (num14 == 0f)
		{
			b2 |= 2;
		}
		if (num15 == 0f)
		{
			b2 |= 4;
		}
		while (num16 >= 1f)
		{
			num7 += num13;
			num8 += num13;
			num9 += num14;
			num10 += num14;
			num11 += num15;
			num12 += num15;
			short num17 = (short)Math.Floor((num7 + BoxX) / (float)BoxSize);
			short num18 = (short)Math.Floor((num9 + BoxY) / (float)BoxSize);
			short num19 = (short)Math.Floor((num11 + BoxZ) / (float)BoxSize);
			short num20 = (short)Math.Floor((num8 + BoxX) / (float)BoxSize);
			short num21 = (short)Math.Floor((num10 + BoxY) / (float)BoxSize);
			short num22 = (short)Math.Floor((num12 + BoxZ) / (float)BoxSize);
			for (short num23 = num17; num23 <= num20; num23++)
			{
				for (short num24 = num18; num24 <= num21; num24++)
				{
					for (short num25 = num19; num25 <= num22; num25++)
					{
						short num26 = (short)(num25 + num24 * BoxDimZ + num23 * BoxDimY * BoxDimZ);
						short num27 = 0;
						bool flag = false;
						while (num27 < mainBox[num26].cnt && !flag)
						{
							short num28;
							if ((num28 = mainBox[num26].oList[num27]) > -1)
							{
								float num29 = global::GameObjects.GameObjects.objMaster[num28].x - 0.5f;
								float num30 = global::GameObjects.GameObjects.objMaster[num28].y - 0.5f;
								float num31 = global::GameObjects.GameObjects.objMaster[num28].z - 0.5f;
								float num32 = num29 + (float)global::GameObjects.GameObjects.objMaster[num28].dimX;
								float num33 = num30 + (float)global::GameObjects.GameObjects.objMaster[num28].dimY;
								float num34 = num31 + (float)global::GameObjects.GameObjects.objMaster[num28].dimZ;
								if (!(num8 <= num29) && !(num7 >= num32) && !(num10 <= num30) && !(num9 >= num33) && !(num12 <= num31) && !(num11 >= num34))
								{
									num7 -= num13;
									num8 -= num13;
									num9 -= num14;
									num10 -= num14;
									num11 -= num15;
									num12 -= num15;
									float num36;
									float num37;
									float num35 = (num36 = (num37 = 0f));
									float num38;
									float num39;
									if (!(num8 <= num29) && !(num7 >= num32) && !(num10 <= num30) && !(num9 >= num33) && !(num12 <= num31) && !(num11 >= num34))
									{
										if (num7 <= num29 && num8 < num32)
										{
											num35 = 0f - (num8 - num29);
										}
										else if (num7 > num29 && num8 >= num32)
										{
											num35 = num32 - num7;
										}
										else
										{
											num38 = num8 - num29;
											num39 = num32 - num8;
											num35 = ((!(num38 < num39)) ? num39 : (0f - num38));
										}
										if (num9 <= num30 && num10 < num33)
										{
											num36 = 0f - (num10 - num30);
										}
										else if (num9 > num30 && num10 >= num33)
										{
											num36 = num33 - num9;
										}
										else
										{
											num38 = num10 - num30;
											num39 = num33 - num10;
											num36 = ((!(num38 < num39)) ? num39 : (0f - num38));
										}
										if (num11 <= num31 && num12 < num34)
										{
											num37 = 0f - (num12 - num31);
										}
										else if (num11 > num31 && num12 >= num34)
										{
											num37 = num34 - num11;
										}
										else
										{
											num38 = num12 - num31;
											num39 = num34 - num12;
											num37 = ((!(num38 < num39)) ? num39 : (0f - num38));
										}
									}
									else
									{
										if (num8 <= num29 && num13 > 0f)
										{
											num35 = num29 - num8;
										}
										else if (num7 >= num32 && num13 < 0f)
										{
											num35 = 0f - (num7 - num32);
										}
										if (num10 <= num30 && num14 > 0f)
										{
											num36 = num30 - num10;
										}
										else if (num9 >= num33 && num14 < 0f)
										{
											num36 = 0f - (num9 - num33);
										}
										if (num12 <= num31 && num15 > 0f)
										{
											num37 = num31 - num12;
										}
										else if (num11 >= num34 && num15 < 0f)
										{
											num37 = 0f - (num11 - num34);
										}
									}
									num38 = Math.Abs(num35);
									num39 = Math.Abs(num36);
									float num40 = Math.Abs(num37);
									if (num38 <= num39 && num38 <= num40)
									{
										if (num4 == 0f)
										{
											num4 = num35;
										}
										else if ((num4 < 0f && num35 > 0f) || (num4 > 0f && num35 < 0f))
										{
											return 8;
										}
										num7 += num35;
										num8 += num35;
										b |= 1;
										b2 |= 1;
										num13 = 0f;
									}
									if (num39 <= num38 && num39 <= num40)
									{
										if (num5 == 0f)
										{
											num5 = num36;
										}
										else if ((num5 < 0f && num36 > 0f) || (num5 > 0f && num36 < 0f))
										{
											return 8;
										}
										num9 += num36;
										num10 += num36;
										b |= 2;
										b2 |= 2;
										num14 = 0f;
									}
									if (num40 <= num38 && num40 <= num39)
									{
										if (num6 == 0f)
										{
											num6 = num37;
										}
										else if ((num6 < 0f && num37 > 0f) || (num6 > 0f && num37 < 0f))
										{
											return 8;
										}
										num11 += num37;
										num12 += num37;
										b |= 4;
										b2 |= 4;
										num15 = 0f;
									}
									colIDT[threadID] = num28;
								}
							}
							num27++;
						}
					}
				}
			}
			num16 -= 1f;
		}
		if (num16 > 0f)
		{
			num13 *= num16;
			num14 *= num16;
			num15 *= num16;
			num7 += num13;
			num8 += num13;
			num9 += num14;
			num10 += num14;
			num11 += num15;
			num12 += num15;
			short num17 = (short)Math.Floor((num7 + BoxX) / (float)BoxSize);
			short num18 = (short)Math.Floor((num9 + BoxY) / (float)BoxSize);
			short num19 = (short)Math.Floor((num11 + BoxZ) / (float)BoxSize);
			short num20 = (short)Math.Floor((num8 + BoxX) / (float)BoxSize);
			short num21 = (short)Math.Floor((num10 + BoxY) / (float)BoxSize);
			short num22 = (short)Math.Floor((num12 + BoxZ) / (float)BoxSize);
			for (short num23 = num17; num23 <= num20; num23++)
			{
				for (short num24 = num18; num24 <= num21; num24++)
				{
					for (short num25 = num19; num25 <= num22; num25++)
					{
						short num26 = (short)(num25 + num24 * BoxDimZ + num23 * BoxDimY * BoxDimZ);
						short num27 = 0;
						bool flag = false;
						while (num27 < mainBox[num26].cnt && !flag)
						{
							short num28;
							if ((num28 = mainBox[num26].oList[num27]) > -1)
							{
								float num29 = global::GameObjects.GameObjects.objMaster[num28].x - 0.5f;
								float num30 = global::GameObjects.GameObjects.objMaster[num28].y - 0.5f;
								float num31 = global::GameObjects.GameObjects.objMaster[num28].z - 0.5f;
								float num32 = num29 + (float)global::GameObjects.GameObjects.objMaster[num28].dimX;
								float num33 = num30 + (float)global::GameObjects.GameObjects.objMaster[num28].dimY;
								float num34 = num31 + (float)global::GameObjects.GameObjects.objMaster[num28].dimZ;
								if (!(num8 <= num29) && !(num7 >= num32) && !(num10 <= num30) && !(num9 >= num33) && !(num12 <= num31) && !(num11 >= num34))
								{
									num7 -= num13;
									num8 -= num13;
									num9 -= num14;
									num10 -= num14;
									num11 -= num15;
									num12 -= num15;
									float num36;
									float num37;
									float num35 = (num36 = (num37 = 0f));
									float num38;
									float num39;
									if (!(num8 <= num29) && !(num7 >= num32) && !(num10 <= num30) && !(num9 >= num33) && !(num12 <= num31) && !(num11 >= num34))
									{
										if (num7 <= num29 && num8 < num32)
										{
											num35 = 0f - (num8 - num29);
										}
										else if (num7 > num29 && num8 >= num32)
										{
											num35 = num32 - num7;
										}
										else
										{
											num38 = num8 - num29;
											num39 = num32 - num8;
											num35 = ((!(num38 < num39)) ? num39 : (0f - num38));
										}
										if (num9 <= num30 && num10 < num33)
										{
											num36 = 0f - (num10 - num30);
										}
										else if (num9 > num30 && num10 >= num33)
										{
											num36 = num33 - num9;
										}
										else
										{
											num38 = num10 - num30;
											num39 = num33 - num10;
											num36 = ((!(num38 < num39)) ? num39 : (0f - num38));
										}
										if (num11 <= num31 && num12 < num34)
										{
											num37 = 0f - (num12 - num31);
										}
										else if (num11 > num31 && num12 >= num34)
										{
											num37 = num34 - num11;
										}
										else
										{
											num38 = num12 - num31;
											num39 = num34 - num12;
											num37 = ((!(num38 < num39)) ? num39 : (0f - num38));
										}
									}
									else
									{
										if (num8 <= num29 && num13 > 0f)
										{
											num35 = num29 - num8;
										}
										else if (num7 >= num32 && num13 < 0f)
										{
											num35 = 0f - (num7 - num32);
										}
										if (num10 <= num30 && num14 > 0f)
										{
											num36 = num30 - num10;
										}
										else if (num9 >= num33 && num14 < 0f)
										{
											num36 = 0f - (num9 - num33);
										}
										if (num12 <= num31 && num15 > 0f)
										{
											num37 = num31 - num12;
										}
										else if (num11 >= num34 && num15 < 0f)
										{
											num37 = 0f - (num11 - num34);
										}
									}
									num38 = Math.Abs(num35);
									num39 = Math.Abs(num36);
									float num40 = Math.Abs(num37);
									if (num38 <= num39 && num38 <= num40)
									{
										if (num4 == 0f)
										{
											num4 = num35;
										}
										else if ((num4 < 0f && num35 > 0f) || (num4 > 0f && num35 < 0f))
										{
											return 8;
										}
										num7 += num35;
										num8 += num35;
										b |= 1;
										b2 |= 1;
										num13 = 0f;
									}
									if (num39 <= num38 && num39 <= num40)
									{
										if (num5 == 0f)
										{
											num5 = num36;
										}
										else if ((num5 < 0f && num36 > 0f) || (num5 > 0f && num36 < 0f))
										{
											return 8;
										}
										num9 += num36;
										num10 += num36;
										b |= 2;
										b2 |= 2;
										num14 = 0f;
									}
									if (num40 <= num38 && num40 <= num39)
									{
										if (num6 == 0f)
										{
											num6 = num37;
										}
										else if ((num6 < 0f && num37 > 0f) || (num6 > 0f && num37 < 0f))
										{
											return 8;
										}
										num11 += num37;
										num12 += num37;
										b |= 4;
										b2 |= 4;
										num15 = 0f;
									}
									colIDT[threadID] = num28;
								}
							}
							num27++;
						}
					}
				}
			}
		}
		p1.pos1.v[0] = (p1.pos2.v[0] = p1.pos1.v[0] + num7 - num);
		p1.pos1.v[1] = (p1.pos2.v[1] = p1.pos1.v[1] + num9 - num2);
		p1.pos1.v[2] = (p1.pos2.v[2] = p1.pos1.v[2] + num11 - num3);
		return b;
	}

	public bool Check_Polygon_Ray_Collision(int collisionModelID, int IgnoreNumber, ref Vector3 InitialRayStart, ref Vector3 InitialRayEnd, ref Matrix Transform, out float distance, out Vector3 IntersectPosition, out Vector3 IntersectNormal, out int Number, out bool isStuck, byte threadID)
	{
		bool flag = false;
		bool stuck = false;
		int num = 1;
		distance = 0f;
		IntersectPosition = new Vector3(0f, 0f, 0f);
		IntersectNormal = new Vector3(0f, 0f, 0f);
		Number = 0;
		isStuck = false;
		Vector3 ray_direction = default(Vector3);
		IntersectPosition = Vector3.Zero;
		IntersectNormal = Vector3.Up;
		Number = -1;
		isStuck = false;
		Matrix.Invert(ref Transform, out var result);
		Vector3.Transform(ref InitialRayStart, ref result, out var result2);
		Vector3.Transform(ref InitialRayEnd, ref result, out var result3);
		float num2 = result3.X - result2.X;
		float num3 = result3.Y - result2.Y;
		float num4 = result3.Z - result2.Z;
		float num5 = (float)Math.Sqrt(num2 * num2 + num3 * num3 + num4 * num4);
		if (num5 != 0f)
		{
			num2 /= num5;
			num3 /= num5;
			num4 /= num5;
		}
		ray_direction.X = num2;
		ray_direction.Y = num3;
		ray_direction.Z = num4;
		int num6 = cModels[collisionModelID].numBoxes - 1;
		boxItemIndex[threadID, 1] = 0;
		cBoxList[threadID, 1] = num6;
		Vector3 colPt;
		Vector3 normal;
		switch (cModels[collisionModelID].collisionScheme)
		{
		case 0:
		{
			int curDiv = cModels[collisionModelID].curDiv;
			float num7 = (result2.X - cModels[collisionModelID].minX) / cModels[collisionModelID].dx;
			float num8 = (result2.Y - cModels[collisionModelID].minY) / cModels[collisionModelID].dy;
			if (!(num7 >= 0f) || !(num7 < (float)curDiv) || !(num8 >= 0f) || !(num8 < (float)curDiv))
			{
				break;
			}
			num6 = cModels[collisionModelID].polygonCount + (int)num7 * curDiv + (int)num8;
			ushort numIDs = cModels[collisionModelID].cb[num6].numIDs;
			for (ushort num9 = 0; num9 < numIDs; num9++)
			{
				int num10 = cModels[collisionModelID].cb[num6].ids[num9];
				if (num10 != IgnoreNumber && Check_Collision_With_Polygon(collisionModelID, num10, ref result2, ref ray_direction, num5, out distance, out colPt, out normal, out stuck) && ((num5 >= 0f && distance < num5) || (num5 < 0f && distance < 0f && distance > num5)))
				{
					isStuck = stuck;
					IntersectPosition = colPt;
					IntersectNormal = normal;
					flag = true;
					num5 = distance;
					Number = num10;
				}
			}
			break;
		}
		case 1:
			while (num > 0)
			{
				switch (cModels[collisionModelID].cb[num6].type)
				{
				case 0:
					if (Does_Ray_Intersect_Box(collisionModelID, num6, result2.X, result2.Y, result2.Z, num2, num3, num4, num5) && cModels[collisionModelID].cb[num6].id != IgnoreNumber && Check_Collision_With_Polygon(collisionModelID, cModels[collisionModelID].cb[num6].id, ref result2, ref ray_direction, num5, out distance, out colPt, out normal, out stuck) && ((num5 >= 0f && distance < num5) || (num5 < 0f && distance < 0f && distance > num5)))
					{
						isStuck = stuck;
						IntersectPosition = colPt;
						IntersectNormal = normal;
						flag = true;
						num5 = distance;
						Number = cModels[collisionModelID].cb[num6].id;
					}
					num6 = cBoxList[threadID, --num];
					break;
				case 1:
					if (boxItemIndex[threadID, num] == 0)
					{
						if (Does_Ray_Intersect_Box(collisionModelID, num6, result2.X, result2.Y, result2.Z, num2, num3, num4, num5))
						{
							boxItemIndex[threadID, num] = 1;
							cBoxList[threadID, num++] = num6;
							num6 = cModels[collisionModelID].cb[num6].ids[0];
						}
						else
						{
							num6 = cBoxList[threadID, --num];
						}
					}
					else if (boxItemIndex[threadID, num] < cModels[collisionModelID].cb[num6].numIDs)
					{
						cBoxList[threadID, num] = num6;
						num6 = cModels[collisionModelID].cb[num6].ids[boxItemIndex[threadID, num]++];
						num++;
					}
					else
					{
						boxItemIndex[threadID, num] = 0;
						num6 = cBoxList[threadID, --num];
					}
					break;
				}
			}
			break;
		}
		distance = num5;
		if (!flag)
		{
			return false;
		}
		Vector3.Transform(ref IntersectPosition, ref Transform, out IntersectPosition);
		Vector3.Transform(ref IntersectNormal, ref Transform, out IntersectNormal);
		IntersectNormal.X -= Transform.M41;
		IntersectNormal.Y -= Transform.M42;
		IntersectNormal.Z -= Transform.M43;
		float num11 = (float)Math.Sqrt(IntersectNormal.X * IntersectNormal.X + IntersectNormal.Y * IntersectNormal.Y + IntersectNormal.Z * IntersectNormal.Z);
		if (num11 != 0f)
		{
			IntersectNormal.X /= num11;
			IntersectNormal.Y /= num11;
			IntersectNormal.Z /= num11;
		}
		return flag;
	}

	public bool Check_Polygon_Ray_Collision_Projectile(int collisionModelID, int IgnoreNumber, ref Vector3 InitialRayStart, ref Vector3 InitialRayEnd, ref Matrix Transform, out float distance, out Vector3 IntersectPosition, out Vector3 IntersectNormal, out int Number, byte threadID)
	{
		bool flag = false;
		int num = 1;
		Vector3 ray_direction = default(Vector3);
		IntersectPosition = Vector3.Zero;
		IntersectNormal = Vector3.Up;
		Number = -1;
		bool stuck = false;
		Matrix.Invert(ref Transform, out var result);
		Vector3.Transform(ref InitialRayStart, ref result, out var result2);
		Vector3.Transform(ref InitialRayEnd, ref result, out var result3);
		float num2 = result3.X - result2.X;
		float num3 = result3.Y - result2.Y;
		float num4 = result3.Z - result2.Z;
		float num5 = (float)Math.Sqrt(num2 * num2 + num3 * num3 + num4 * num4);
		if (num5 != 0f)
		{
			num2 /= num5;
			num3 /= num5;
			num4 /= num5;
		}
		ray_direction.X = num2;
		ray_direction.Y = num3;
		ray_direction.Z = num4;
		int num6 = cModels[collisionModelID].numBoxes - 1;
		boxItemIndex[threadID, 1] = 0;
		cBoxList[threadID, 1] = num6;
		Vector3 colPt;
		Vector3 normal;
		switch (cModels[collisionModelID].collisionScheme)
		{
		case 0:
		{
			int curDiv = cModels[collisionModelID].curDiv;
			float num7 = (result2.X - cModels[collisionModelID].minX) / cModels[collisionModelID].dx;
			float num8 = (result2.Y - cModels[collisionModelID].minY) / cModels[collisionModelID].dy;
			if (!(num7 >= 0f) || !(num7 < (float)curDiv) || !(num8 >= 0f) || !(num8 < (float)curDiv))
			{
				break;
			}
			num6 = cModels[collisionModelID].polygonCount + (int)num7 * curDiv + (int)num8;
			ushort numIDs = cModels[collisionModelID].cb[num6].numIDs;
			for (ushort num9 = 0; num9 < numIDs; num9++)
			{
				int num10 = cModels[collisionModelID].cb[num6].ids[num9];
				if (num10 != IgnoreNumber && Check_Collision_With_Polygon(collisionModelID, num10, ref result2, ref ray_direction, num5, out distance, out colPt, out normal, out stuck) && ((num5 >= 0f && distance < num5) || (num5 < 0f && distance < 0f && distance > num5)))
				{
					IntersectPosition = colPt;
					IntersectNormal = normal;
					flag = true;
					num5 = distance;
					Number = num10;
				}
			}
			break;
		}
		case 1:
			while (num > 0)
			{
				switch (cModels[collisionModelID].cb[num6].type)
				{
				case 0:
					if (Does_Ray_Intersect_Box(collisionModelID, num6, result2.X, result2.Y, result2.Z, num2, num3, num4, num5) && cModels[collisionModelID].cb[num6].id != IgnoreNumber && Check_Collision_With_Polygon(collisionModelID, cModels[collisionModelID].cb[num6].id, ref result2, ref ray_direction, num5, out distance, out colPt, out normal, out stuck) && ((num5 >= 0f && distance < num5) || (num5 < 0f && distance < 0f && distance > num5)))
					{
						IntersectPosition = colPt;
						IntersectNormal = normal;
						flag = true;
						num5 = distance;
						Number = cModels[collisionModelID].cb[num6].id;
					}
					num6 = cBoxList[threadID, --num];
					break;
				case 1:
					if (boxItemIndex[threadID, num] == 0)
					{
						if (Does_Ray_Intersect_Box(collisionModelID, num6, result2.X, result2.Y, result2.Z, num2, num3, num4, num5))
						{
							boxItemIndex[threadID, num] = 1;
							cBoxList[threadID, num++] = num6;
							num6 = cModels[collisionModelID].cb[num6].ids[0];
						}
						else
						{
							num6 = cBoxList[threadID, --num];
						}
					}
					else if (boxItemIndex[threadID, num] < cModels[collisionModelID].cb[num6].numIDs)
					{
						cBoxList[threadID, num] = num6;
						num6 = cModels[collisionModelID].cb[num6].ids[boxItemIndex[threadID, num]++];
						num++;
					}
					else
					{
						boxItemIndex[threadID, num] = 0;
						num6 = cBoxList[threadID, --num];
					}
					break;
				}
			}
			break;
		}
		distance = num5;
		if (!flag)
		{
			return false;
		}
		Vector3.Transform(ref IntersectPosition, ref Transform, out IntersectPosition);
		Vector3.Transform(ref IntersectNormal, ref Transform, out IntersectNormal);
		IntersectNormal.X -= Transform.M41;
		IntersectNormal.Y -= Transform.M42;
		IntersectNormal.Z -= Transform.M43;
		float num11 = (float)Math.Sqrt(IntersectNormal.X * IntersectNormal.X + IntersectNormal.Y * IntersectNormal.Y + IntersectNormal.Z * IntersectNormal.Z);
		if (num11 != 0f)
		{
			IntersectNormal.X /= num11;
			IntersectNormal.Y /= num11;
			IntersectNormal.Z /= num11;
		}
		return flag;
	}

	public bool Check_Collision_With_Polygon(int modID, int polygonID, ref Vector3 ray_origin, ref Vector3 ray_direction, float maxDistance, out float distance, out Vector3 colPt, out Vector3 normal, out bool stuck)
	{
		Vector3 vector = default(Vector3);
		Vector3 vector2 = default(Vector3);
		Vector3 vector3 = default(Vector3);
		Vector3 vector4 = default(Vector3);
		polygonID *= 3;
		vector.X = cModels[modID].v[polygonID].X;
		vector.Y = cModels[modID].v[polygonID].Y;
		vector.Z = cModels[modID].v[polygonID].Z;
		vector4.X = cModels[modID].n[polygonID].X;
		vector4.Y = cModels[modID].n[polygonID].Y;
		vector4.Z = cModels[modID].n[polygonID++].Z;
		vector2.X = cModels[modID].v[polygonID].X;
		vector2.Y = cModels[modID].v[polygonID].Y;
		vector2.Z = cModels[modID].v[polygonID++].Z;
		vector3.X = cModels[modID].v[polygonID].X;
		vector3.Y = cModels[modID].v[polygonID].Y;
		vector3.Z = cModels[modID].v[polygonID].Z;
		stuck = false;
		distance = 0f;
		colPt = Vector3.Zero;
		Vector3 vector5 = vector2 - vector;
		Vector3 vector6 = vector3 - vector;
		Vector3.Cross(ref vector5, ref vector6, out normal);
		if (Vector3.Dot(normal, vector4) < 0f)
		{
			normal *= -1f;
		}
		normal.Normalize();
		float num = Vector3.Dot(normal, ray_direction);
		Vector3 vector7 = ray_origin - vector;
		float num2 = Vector3.Dot(normal, vector7);
		if (num2 < -0.1f)
		{
			return false;
		}
		if (num != 0f)
		{
			distance = num2 / (0f - num);
		}
		if (num2 >= 0f && num >= 0f)
		{
			return false;
		}
		float num3 = 1f;
		Vector3 vector8 = vector2 - vector;
		Vector3 vector9 = vector3 - vector;
		Vector3 vector10 = Vector3.Cross(vector8, vector9);
		if (Vector3.Dot(normal, vector10) < 0f)
		{
			num3 = -1f;
		}
		Vector3 vector11;
		if (num2 < 0f)
		{
			colPt = ray_origin - num2 * normal;
			vector11 = colPt - vector;
			vector10 = Vector3.Cross(vector8, vector11);
			if (num3 * Vector3.Dot(vector10, normal) < 0f)
			{
				return false;
			}
			vector10 = Vector3.Cross(vector11, vector9);
			if (num3 * Vector3.Dot(vector10, normal) < 0f)
			{
				return false;
			}
			vector11 = colPt - vector3;
			vector10 = vector2 - vector3;
			vector10 = Vector3.Cross(vector11, vector10);
			if (num3 * Vector3.Dot(vector10, normal) < 0f)
			{
				return false;
			}
			distance = 0f - Math.Abs(distance);
			stuck = true;
			return true;
		}
		if (maxDistance <= distance)
		{
			return false;
		}
		colPt = ray_origin + ray_direction * distance;
		vector11 = colPt - vector;
		vector10 = Vector3.Cross(vector8, vector11);
		if (num3 * Vector3.Dot(vector10, normal) < 0f)
		{
			return false;
		}
		vector10 = Vector3.Cross(vector11, vector9);
		if (num3 * Vector3.Dot(vector10, normal) < 0f)
		{
			return false;
		}
		vector11 = colPt - vector3;
		vector10 = vector2 - vector3;
		vector10 = Vector3.Cross(vector11, vector10);
		if (num3 * Vector3.Dot(vector10, normal) < 0f)
		{
			return false;
		}
		return true;
	}

	public bool Does_Ray_Intersect_Box(int modID, int boxID, float x, float y, float z, float vx, float vy, float vz, float distance)
	{
		float num = 0f;
		float num2 = 0f;
		float num3 = 0f;
		if ((x < cModels[modID].cb[boxID].x && vx <= 0f) || (x > cModels[modID].cb[boxID].x2 && vx >= 0f) || (y < cModels[modID].cb[boxID].y && vy <= 0f) || (y > cModels[modID].cb[boxID].y2 && vy >= 0f) || (z < cModels[modID].cb[boxID].z && vz <= 0f) || (z > cModels[modID].cb[boxID].z2 && vz >= 0f))
		{
			return false;
		}
		num = x - cModels[modID].cb[boxID].x;
		num2 = y - cModels[modID].cb[boxID].y;
		num3 = z - cModels[modID].cb[boxID].z;
		float num4 = cModels[modID].cb[boxID].x2 - cModels[modID].cb[boxID].x;
		float num5 = cModels[modID].cb[boxID].y2 - cModels[modID].cb[boxID].y;
		float num6 = cModels[modID].cb[boxID].z2 - cModels[modID].cb[boxID].z;
		if (vx != 0f)
		{
			if (num < 0f)
			{
				float num7 = (0f - num) / vx;
				distance -= num7;
				if (distance < 0f)
				{
					return false;
				}
				num = 0f;
				num2 += vy * num7;
				num3 += vz * num7;
			}
			else if (num > num4)
			{
				float num7 = (num4 - num) / vx;
				distance -= num7;
				if (distance < 0f)
				{
					return false;
				}
				num = num4;
				num2 += vy * num7;
				num3 += vz * num7;
			}
		}
		if (vy != 0f)
		{
			if (num2 < 0f)
			{
				float num7 = (0f - num2) / vy;
				distance -= num7;
				if (distance < 0f || num7 < 0f)
				{
					return false;
				}
				num2 = 0f;
				num += vx * num7;
				num3 += vz * num7;
			}
			else if (num2 > num5)
			{
				float num7 = (num5 - num2) / vy;
				distance -= num7;
				if (distance < 0f || num7 < 0f)
				{
					return false;
				}
				num2 = num5;
				num += vx * num7;
				num3 += vz * num7;
			}
		}
		if (vz != 0f)
		{
			if (num3 < 0f)
			{
				float num7 = (0f - num3) / vz;
				distance -= num7;
				if (distance < 0f || num7 < 0f)
				{
					return false;
				}
				num += vx * num7;
				num2 += vy * num7;
				num3 = 0f;
			}
			else if (num3 > num6)
			{
				float num7 = (num6 - num3) / vz;
				distance -= num7;
				if (distance < 0f || num7 < 0f)
				{
					return false;
				}
				num3 = num6;
				num += vx * num7;
				num2 += vy * num7;
			}
		}
		if (num >= 0f && num <= num4 && num2 >= 0f && num2 <= num5 && num3 >= 0f && num3 <= num6)
		{
			return true;
		}
		return false;
	}

	public void Create_Consolidated_Collision_Model(string fileName, byte action, ushort collisionModelID, ref Matrix mv1)
	{
		Vector3 vector = default(Vector3);
		switch (action)
		{
		case 0:
			curCollisionModelExportVertice = 1;
			curCollisionModelExportNormal = 1;
			fp = File.CreateText(Environment.CurrentDirectory + "\\The_CoOp_Zombie_Game\\LevelEditor\\Consolidated_Collision_" + fileName);
			fp.WriteLine("vt 0.0 0.0");
			break;
		case 1:
		{
			int num = cModels[collisionModelID].polygonCount * 3;
			int num2 = curCollisionModelExportNormal;
			int num3 = curCollisionModelExportVertice;
			int num4 = num3 + 1;
			int num5 = num4 + 1;
			int num6 = 0;
			while (num6 < num)
			{
				vector = Vector3.TransformNormal(cModels[collisionModelID].n[num6], mv1);
				fp.WriteLine("vn " + vector.X + " " + vector.Y + " " + vector.Z);
				vector = Vector3.Transform(cModels[collisionModelID].v[num6], mv1);
				fp.WriteLine("v " + vector.X + " " + vector.Y + " " + vector.Z);
				num6++;
				vector = Vector3.Transform(cModels[collisionModelID].v[num6], mv1);
				fp.WriteLine("v " + vector.X + " " + vector.Y + " " + vector.Z);
				num6++;
				vector = Vector3.Transform(cModels[collisionModelID].v[num6], mv1);
				fp.WriteLine("v " + vector.X + " " + vector.Y + " " + vector.Z);
				fp.WriteLine("f " + num3 + "/1/" + num2 + " " + num4 + "/1/" + num2 + " " + num5 + "/1/" + num2);
				num2++;
				num6++;
				num3 += 3;
				num4 += 3;
				num5 += 3;
			}
			curCollisionModelExportVertice += cModels[collisionModelID].polygonCount * 3;
			curCollisionModelExportNormal = num2;
			fp.WriteLine("#new model");
			break;
		}
		case 2:
			fp.Flush();
			fp.Close();
			break;
		}
	}

	public void Calculate_Length(ref StructsClass.particle_list p1, ref float Vx, ref float Vy, ref float Vz, ref float len)
	{
		Vx = p1.pos2.v[0] - p1.pos1.v[0];
		Vy = p1.pos2.v[1] - p1.pos1.v[1];
		Vz = p1.pos2.v[2] - p1.pos1.v[2];
		len = (float)(Math.Pow(Vx, 2.0) + Math.Pow(Vy, 2.0) + Math.Pow(Vz, 2.0));
		if (len > 1f)
		{
			len = (float)Math.Sqrt(len);
			Vx /= len;
			Vy /= len;
			Vz /= len;
		}
		else
		{
			len = 1f;
		}
	}

	public void Calculate_Length_Byte(ref StructsClass.particle_list_byte p1, ref float Vx, ref float Vy, ref float Vz, ref float len)
	{
		Vx = p1.pos2.v[0] - p1.pos1.v[0];
		Vy = p1.pos2.v[1] - p1.pos1.v[1];
		Vz = p1.pos2.v[2] - p1.pos1.v[2];
		len = (float)(Math.Pow(Vx, 2.0) + Math.Pow(Vy, 2.0) + Math.Pow(Vz, 2.0));
		if (len > 1f)
		{
			len = (float)Math.Sqrt(len);
			Vx /= len;
			Vy /= len;
			Vz /= len;
		}
		else
		{
			len = 1f;
		}
	}

	public void Calculate_Length_Float(ref StructsClass.vtex p1, ref StructsClass.vtex p2, ref float Vx, ref float Vy, ref float Vz, ref float len)
	{
		Vx = p2.v[0] - p1.v[0];
		Vy = p2.v[1] - p1.v[1];
		Vz = p2.v[2] - p1.v[2];
		len = (float)Math.Sqrt(Math.Pow(Vx, 2.0) + Math.Pow(Vy, 2.0) + Math.Pow(Vz, 2.0));
		Vx /= len;
		Vy /= len;
		Vz /= len;
	}

	public void Validate_Position(ref StructsClass.physics ph1, byte threadID)
	{
		if (ph1.position.v[0] > global::MainGame.MainGame.MaxRight)
		{
			ph1.position.v[0] = global::MainGame.MainGame.MaxRight;
			ph1.velocity.v[0] = 0f;
		}
		if (ph1.position.v[0] < global::MainGame.MainGame.MaxLeft)
		{
			ph1.position.v[0] = global::MainGame.MainGame.MaxLeft;
			ph1.velocity.v[0] = 0f;
		}
		if (ph1.position.v[1] > global::MainGame.MainGame.MaxForward)
		{
			ph1.position.v[1] = global::MainGame.MainGame.MaxForward;
			ph1.velocity.v[1] = 0f;
		}
		if (ph1.position.v[1] < global::MainGame.MainGame.MaxRear)
		{
			ph1.position.v[1] = global::MainGame.MainGame.MaxRear;
			ph1.velocity.v[1] = 0f;
		}
		if (ph1.position.v[2] > global::MainGame.MainGame.MaxUp)
		{
			ph1.position.v[2] = global::MainGame.MainGame.MaxUp;
			ph1.velocity.v[2] = 0f;
		}
		float num = mainC.terrainMain.Get_Terrain_Height(ph1.position.v[0], ph1.position.v[1], threadID);
		if (num < global::MainGame.MainGame.MaxDown)
		{
			num = global::MainGame.MainGame.MaxDown;
		}
		if (ph1.position.v[2] < num)
		{
			ph1.position.v[2] = num;
			ph1.velocity.v[2] = 0f;
		}
	}

	public bool Position_InsideBoundary(ref StructsClass.physics ph1)
	{
		if (ph1.position.v[0] > global::MainGame.MainGame.MaxRight)
		{
			return false;
		}
		if (ph1.position.v[0] < global::MainGame.MainGame.MaxLeft)
		{
			return false;
		}
		if (ph1.position.v[1] > global::MainGame.MainGame.MaxForward)
		{
			return false;
		}
		if (ph1.position.v[1] < global::MainGame.MainGame.MaxRear)
		{
			return false;
		}
		if (ph1.position.v[2] > global::MainGame.MainGame.MaxUp)
		{
			return false;
		}
		if (ph1.position.v[2] < global::MainGame.MainGame.MaxDown)
		{
			return false;
		}
		return true;
	}

	public void Remove_Object_From_CollisionBoxes(long oID)
	{
		for (long num = 0L; num < BoxDimX; num++)
		{
			for (long num2 = 0L; num2 < BoxDimY; num2++)
			{
				for (long num3 = 0L; num3 < BoxDimZ; num3++)
				{
					long num4 = num3 + num2 * BoxDimZ + num * BoxDimY * BoxDimZ;
					long num5 = 0L;
					bool flag = false;
					for (; num5 < mainBox[num4].cnt; num5++)
					{
						if (flag)
						{
							break;
						}
						if (mainBox[num4].oList[num5] != oID)
						{
							continue;
						}
						mainBox[num4].oList[num5] = -1;
						long num6;
						for (num6 = num5; num6 < mainBox[num4].cnt - 1; num6++)
						{
							mainBox[num4].oList[num6] = mainBox[num4].oList[num6 + 1];
						}
						mainBox[num4].oList[num6] = -1;
						mainBox[num4].numObjects = 0;
						for (num6 = 0L; num6 < mainBox[num4].cnt; num6++)
						{
							if (mainBox[num4].oList[num6] > -1)
							{
								mainBox[num4].numObjects++;
							}
						}
						flag = true;
					}
				}
			}
		}
	}

	public void Update_CollisionBox(short oID, byte threadID)
	{
		long num = BoxDimY;
		long num2 = BoxDimZ;
		float num3;
		float num4;
		float num5;
		float num6;
		float num7;
		float num8;
		if (!global::GameObjects.GameObjects.objMaster[oID].isRotated)
		{
			num3 = global::GameObjects.GameObjects.objMaster[oID].x - 0.5f + BoxX;
			num4 = global::GameObjects.GameObjects.objMaster[oID].y - 0.5f + BoxY;
			num5 = global::GameObjects.GameObjects.objMaster[oID].z - 0.5f + BoxZ;
			num6 = num3 + (float)global::GameObjects.GameObjects.objMaster[oID].dimX + 0.5f;
			num7 = num4 + (float)global::GameObjects.GameObjects.objMaster[oID].dimY + 0.5f;
			num8 = num5 + (float)global::GameObjects.GameObjects.objMaster[oID].dimZ + 0.5f;
		}
		else
		{
			Matrix mv = global::GameObjects.GameObjects.objMaster[oID].mv;
			float num9 = (float)global::GameObjects.GameObjects.objMaster[oID].dimX - 0.5f;
			float num10 = (float)global::GameObjects.GameObjects.objMaster[oID].dimY - 0.5f;
			float num11 = (float)global::GameObjects.GameObjects.objMaster[oID].dimZ - 0.5f;
			float num12 = -0.5f * mv.M11 + -0.5f * mv.M21 + -0.5f * mv.M31;
			float num13 = -0.5f * mv.M12 + -0.5f * mv.M22 + -0.5f * mv.M32;
			float num14 = -0.5f * mv.M13 + -0.5f * mv.M23 + -0.5f * mv.M33;
			float num15 = num9 * mv.M11 + -0.5f * mv.M21 + -0.5f * mv.M31;
			float num16 = num9 * mv.M12 + -0.5f * mv.M22 + -0.5f * mv.M32;
			float num17 = num9 * mv.M13 + -0.5f * mv.M23 + -0.5f * mv.M33;
			float num18 = num9 * mv.M11 + num10 * mv.M21 + -0.5f * mv.M31;
			float num19 = num9 * mv.M12 + num10 * mv.M22 + -0.5f * mv.M32;
			float num20 = num9 * mv.M13 + num10 * mv.M23 + -0.5f * mv.M33;
			float num21 = -0.5f * mv.M11 + num10 * mv.M21 + -0.5f * mv.M31;
			float num22 = -0.5f * mv.M12 + num10 * mv.M22 + -0.5f * mv.M32;
			float num23 = -0.5f * mv.M13 + num10 * mv.M23 + -0.5f * mv.M33;
			float num24 = -0.5f * mv.M11 + -0.5f * mv.M21 + num11 * mv.M31;
			float num25 = -0.5f * mv.M12 + -0.5f * mv.M22 + num11 * mv.M32;
			float num26 = -0.5f * mv.M13 + -0.5f * mv.M23 + num11 * mv.M33;
			float num27 = num9 * mv.M11 + -0.5f * mv.M21 + num11 * mv.M31;
			float num28 = num9 * mv.M12 + -0.5f * mv.M22 + num11 * mv.M32;
			float num29 = num9 * mv.M13 + -0.5f * mv.M23 + num11 * mv.M33;
			float num30 = num9 * mv.M11 + num10 * mv.M21 + num11 * mv.M31;
			float num31 = num9 * mv.M12 + num10 * mv.M22 + num11 * mv.M32;
			float num32 = num9 * mv.M13 + num10 * mv.M23 + num11 * mv.M33;
			float num33 = -0.5f * mv.M11 + num10 * mv.M21 + num11 * mv.M31;
			float num34 = -0.5f * mv.M12 + num10 * mv.M22 + num11 * mv.M32;
			float num35 = -0.5f * mv.M13 + num10 * mv.M23 + num11 * mv.M33;
			float num36;
			num9 = (num36 = num12);
			float num37;
			num10 = (num37 = num13);
			float num38;
			num11 = (num38 = num14);
			if (num15 < num9)
			{
				num9 = num15;
			}
			if (num18 < num9)
			{
				num9 = num18;
			}
			if (num21 < num9)
			{
				num9 = num21;
			}
			if (num24 < num9)
			{
				num9 = num24;
			}
			if (num27 < num9)
			{
				num9 = num27;
			}
			if (num30 < num9)
			{
				num9 = num30;
			}
			if (num33 < num9)
			{
				num9 = num33;
			}
			if (num16 < num10)
			{
				num10 = num16;
			}
			if (num19 < num10)
			{
				num10 = num19;
			}
			if (num22 < num10)
			{
				num10 = num22;
			}
			if (num25 < num10)
			{
				num10 = num25;
			}
			if (num28 < num10)
			{
				num10 = num28;
			}
			if (num31 < num10)
			{
				num10 = num31;
			}
			if (num34 < num10)
			{
				num10 = num34;
			}
			if (num17 < num11)
			{
				num11 = num17;
			}
			if (num20 < num11)
			{
				num11 = num20;
			}
			if (num23 < num11)
			{
				num11 = num23;
			}
			if (num26 < num11)
			{
				num11 = num26;
			}
			if (num29 < num11)
			{
				num11 = num29;
			}
			if (num32 < num11)
			{
				num11 = num32;
			}
			if (num35 < num11)
			{
				num11 = num35;
			}
			if (num15 > num36)
			{
				num36 = num15;
			}
			if (num18 > num36)
			{
				num36 = num18;
			}
			if (num21 > num36)
			{
				num36 = num21;
			}
			if (num24 > num36)
			{
				num36 = num24;
			}
			if (num27 > num36)
			{
				num36 = num27;
			}
			if (num30 > num36)
			{
				num36 = num30;
			}
			if (num33 > num36)
			{
				num36 = num33;
			}
			if (num16 > num37)
			{
				num37 = num16;
			}
			if (num19 > num37)
			{
				num37 = num19;
			}
			if (num22 > num37)
			{
				num37 = num22;
			}
			if (num25 > num37)
			{
				num37 = num25;
			}
			if (num28 > num37)
			{
				num37 = num28;
			}
			if (num31 > num37)
			{
				num37 = num31;
			}
			if (num34 > num37)
			{
				num37 = num34;
			}
			if (num17 > num38)
			{
				num38 = num17;
			}
			if (num20 > num38)
			{
				num38 = num20;
			}
			if (num23 > num38)
			{
				num38 = num23;
			}
			if (num26 > num38)
			{
				num38 = num26;
			}
			if (num29 > num38)
			{
				num38 = num29;
			}
			if (num32 > num38)
			{
				num38 = num32;
			}
			if (num35 > num38)
			{
				num38 = num35;
			}
			num3 = num9 + global::GameObjects.GameObjects.objMaster[oID].x + BoxX;
			num4 = num10 + global::GameObjects.GameObjects.objMaster[oID].y + BoxY;
			num5 = num11 + global::GameObjects.GameObjects.objMaster[oID].z + BoxZ;
			num6 = num36 + global::GameObjects.GameObjects.objMaster[oID].x + BoxX;
			num7 = num37 + global::GameObjects.GameObjects.objMaster[oID].y + BoxY;
			num8 = num38 + global::GameObjects.GameObjects.objMaster[oID].z + BoxZ;
		}
		num3 = (float)Math.Floor(num3 / (float)BoxSize);
		num4 = (float)Math.Floor(num4 / (float)BoxSize);
		num5 = (float)Math.Floor(num5 / (float)BoxSize);
		num6 = (float)Math.Floor(num6 / (float)BoxSize);
		num7 = (float)Math.Floor(num7 / (float)BoxSize);
		num8 = (float)Math.Floor(num8 / (float)BoxSize);
		long num39 = ((long)num6 - (long)num3 + 1) * ((long)num7 - (long)num4 + 1) * ((long)num8 - (long)num5 + 1);
		if (num39 < 0 || num39 > numBoxes)
		{
			return;
		}
		if (global::GameObjects.GameObjects.objMaster[oID].boxCount < num39)
		{
			int[] array = new int[num39];
			for (long num40 = 0L; num40 < global::GameObjects.GameObjects.objMaster[oID].curBoxes; num40++)
			{
				array[num40] = global::GameObjects.GameObjects.objMaster[oID].boxList[num40];
			}
			global::GameObjects.GameObjects.objMaster[oID].boxList = array;
			global::GameObjects.GameObjects.objMaster[oID].boxCount = (short)num39;
		}
		num39 = global::GameObjects.GameObjects.objMaster[oID].curBoxes;
		for (long num40 = 0L; num40 < num39; num40++)
		{
			long num41 = global::GameObjects.GameObjects.objMaster[oID].boxList[num40];
			for (long num42 = 0L; num42 < mainBox[num41].cnt; num42++)
			{
				if (mainBox[num41].oList[num42] != oID)
				{
					continue;
				}
				mainBox[num41].oList[num42] = -1;
				long num43;
				for (num43 = num42; num43 < mainBox[num41].cnt - 1; num43++)
				{
					mainBox[num41].oList[num43] = mainBox[num41].oList[num43 + 1];
				}
				mainBox[num41].oList[num43] = -1;
				mainBox[num41].numObjects = 0;
				for (num43 = 0L; num43 < mainBox[num41].cnt; num43++)
				{
					if (mainBox[num41].oList[num43] > -1)
					{
						mainBox[num41].numObjects++;
					}
				}
				if (mainBox[num41].numObjects > maxItemsInABox)
				{
					maxItemsInABox = mainBox[num41].numObjects;
				}
				num42 = mainBox[num41].cnt;
			}
		}
		long num44 = 0L;
		bool flag = false;
		for (long num40 = (long)num3; num40 <= (long)num6; num40++)
		{
			for (num39 = (long)num4; num39 <= (long)num7; num39++)
			{
				for (long num45 = (long)num5; num45 <= (long)num8; num45++)
				{
					long num41 = num45 + num39 * num2 + num40 * num * num2;
					global::GameObjects.GameObjects.objMaster[oID].boxList[num44++] = (int)num41;
					long num42 = 0L;
					flag = false;
					for (; num42 < mainBox[num41].cnt; num42++)
					{
						if (flag)
						{
							break;
						}
						if (mainBox[num41].oList[num42] < 0)
						{
							mainBox[num41].oList[num42] = oID;
							flag = true;
						}
					}
					if (!flag)
					{
						num42 = mainBox[num41].cnt;
						if (Expand_Box(num41))
						{
							mainBox[num41].oList[num42] = oID;
						}
					}
					mainBox[num41].numObjects = 0;
					for (long num43 = 0L; num43 < mainBox[num41].cnt; num43++)
					{
						if (mainBox[num41].oList[num43] > -1)
						{
							mainBox[num41].numObjects++;
						}
					}
					if (mainBox[num41].numObjects > maxItemsInABox)
					{
						maxItemsInABox = mainBox[num41].numObjects;
					}
				}
			}
		}
		global::GameObjects.GameObjects.objMaster[oID].curBoxes = (short)num44;
	}

	public bool Expand_Box(long x)
	{
		short cnt = mainBox[x].cnt;
		short num = (short)(cnt + 10);
		mainBox[x].cnt = num;
		short[] array = new short[num];
		short num2;
		for (num2 = 0; num2 < cnt; num2++)
		{
			array[num2] = mainBox[x].oList[num2];
		}
		while (num2 < num)
		{
			array[num2] = -1;
			num2++;
		}
		mainBox[x].oList = array;
		return true;
	}

	public void Init_Collision()
	{
		_ = global::Rendering.Rendering.uBufferID;
		maxPositions = 0uL;
		maxPositions = ~maxPositions;
		global::MainGame.MainGame.numCollisionModels = 0;
		cModels = new StructsClass.CollisionModel[15];
		numAllocatedCollisionModels = 15;
	}

	public void Load_Collsion_Boundaries(string fileName)
	{
		Stream stream = TitleContainer.OpenStream("The_CoOp_Zombie_Game\\Config_Files\\" + fileName);
		byte[] array = new byte[stream.Length];
		if (stream.CanRead)
		{
			stream.Read(array, 0, array.Length);
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
				stream.Close();
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
				if (array4[0].Equals("Left", StringComparison.OrdinalIgnoreCase))
				{
					num3 = 1;
				}
				else if (array4[0].Equals("Right", StringComparison.OrdinalIgnoreCase))
				{
					num3 = 2;
				}
				else if (array4[0].Equals("Forward", StringComparison.OrdinalIgnoreCase))
				{
					num3 = 3;
				}
				else if (array4[0].Equals("Rear", StringComparison.OrdinalIgnoreCase))
				{
					num3 = 4;
				}
				else if (array4[0].Equals("Up", StringComparison.OrdinalIgnoreCase))
				{
					num3 = 5;
				}
				else if (array4[0].Equals("Down", StringComparison.OrdinalIgnoreCase))
				{
					num3 = 6;
				}
				else if (array4[0].Equals("BoxSize", StringComparison.OrdinalIgnoreCase))
				{
					num3 = 7;
				}
				switch (num3)
				{
				case 1:
					if (array4.Length > 1)
					{
						global::MainGame.MainGame.MaxLeft = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 2:
					if (array4.Length > 1)
					{
						global::MainGame.MainGame.MaxRight = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 3:
					if (array4.Length > 1)
					{
						global::MainGame.MainGame.MaxForward = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 4:
					if (array4.Length > 1)
					{
						global::MainGame.MainGame.MaxRear = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 5:
					if (array4.Length > 1)
					{
						global::MainGame.MainGame.MaxUp = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 6:
					if (array4.Length > 1)
					{
						global::MainGame.MainGame.MaxDown = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 7:
					if (array4.Length > 1)
					{
						BoxSize = int.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				}
			}
		}
		stream.Close();
		BoxX = 0f - global::MainGame.MainGame.MaxLeft + 100f;
		BoxY = 0f - global::MainGame.MainGame.MaxRear + 100f;
		BoxZ = 0f - global::MainGame.MainGame.MaxDown + 100f;
		widthX = global::MainGame.MainGame.MaxRight - global::MainGame.MainGame.MaxLeft + 200f;
		widthY = global::MainGame.MainGame.MaxForward - global::MainGame.MainGame.MaxRear + 200f;
		widthZ = global::MainGame.MainGame.MaxUp - global::MainGame.MainGame.MaxDown + 200f;
		BoxDimX = (int)Math.Ceiling(widthX / (float)BoxSize);
		BoxDimY = (int)Math.Ceiling(widthY / (float)BoxSize);
		BoxDimZ = (int)Math.Ceiling(widthZ / (float)BoxSize);
		numBoxes = BoxDimX * BoxDimY * BoxDimZ;
		int num4 = numBoxes;
		if (numBoxes > numAllocatedBoxes)
		{
			mainBox = new StructsClass.boxList[numBoxes];
			numAllocatedBoxes = numBoxes;
			for (int k = 0; k < num4; k++)
			{
				mainBox[k] = default(StructsClass.boxList);
				mainBox[k].cnt = 0;
				mainBox[k].numObjects = 0;
			}
		}
		else
		{
			for (int k = 0; k < num4; k++)
			{
				mainBox[k].cnt = 0;
				mainBox[k].numObjects = 0;
			}
		}
		float num5 = global::MainGame.MainGame.MaxRight - global::MainGame.MainGame.MaxLeft;
		float num6 = global::MainGame.MainGame.MaxForward - global::MainGame.MainGame.MaxRear;
		float num7 = global::MainGame.MainGame.MaxUp - global::MainGame.MainGame.MaxDown;
		maxDistanceSqr = num5 * num5 + num6 * num6 + num7 * num7;
	}

	public unsafe void Load_Collision_Models(string fileName)
	{
		_ = global::Rendering.Rendering.uBufferID;
		for (int i = 0; i < global::MainGame.MainGame.numCollisionModels; i++)
		{
			cModels[i].fileName = "";
			cModels[i].id = 0;
		}
		Stream stream = TitleContainer.OpenStream("The_CoOp_Zombie_Game\\Config_Files\\" + fileName);
		byte[] array = new byte[stream.Length];
		if (!stream.CanRead)
		{
			return;
		}
		stream.Read(array, 0, array.Length);
		string text = mainC.utilMain.Byte_Array_To_String(array);
		string[] array2 = text.Split('\n', '\r');
		int j = 0;
		int num = 0;
		for (; j < array2.Length; j++)
		{
			if (array2[j].Length > 0)
			{
				num++;
			}
		}
		if (num < 1)
		{
			stream.Close();
			return;
		}
		string[] array3 = new string[num];
		j = 0;
		num = 0;
		for (; j < array2.Length; j++)
		{
			if (array2[j].Length > 0)
			{
				array3[num++] = array2[j];
			}
		}
		for (j = 0; j < num; j++)
		{
			array2 = array3[j].Split(' ', '\t');
			int k = 0;
			int num2 = 0;
			for (; k < array2.Length; k++)
			{
				if (array2[k].Length > 0)
				{
					num2++;
				}
			}
			if (num2 < 1)
			{
				continue;
			}
			string[] array4 = new string[num2];
			k = 0;
			num2 = 0;
			for (; k < array2.Length; k++)
			{
				if (array2[k].Length > 0)
				{
					array4[num2++] = array2[k];
				}
			}
			global::MainGame.MainGame.numCollisionModels = (ushort)array4.Length;
			if (global::MainGame.MainGame.numCollisionModels > numAllocatedCollisionModels)
			{
				cModels = new StructsClass.CollisionModel[global::MainGame.MainGame.numCollisionModels];
				for (int i = 0; i < global::MainGame.MainGame.numCollisionModels; i++)
				{
					cModels[i].fileName = "";
					cModels[i].id = 0;
				}
				numAllocatedCollisionModels = global::MainGame.MainGame.numCollisionModels;
			}
			for (int l = 0; l < global::MainGame.MainGame.numCollisionModels; l++)
			{
				cModels[l].fileName = array4[l];
				cModels[l].id = (ushort)l;
				mainC.modelsMain.Load_Condensed_Collision_Model(array4[l]);
				int num3 = global::Models.Models.tempModel.pcount * 3;
				cModels[l].v = new Vector3[num3];
				cModels[l].n = new Vector3[num3];
				int m;
				fixed (byte* vertexBytes = global::Models.Models.tempModel.vertexBytes)
				{
					fixed (byte* normalBytes = global::Models.Models.tempModel.normalBytes)
					{
						fixed (byte* vIndexBytes = global::Models.Models.tempModel.vIndexBytes)
						{
							fixed (byte* nIndexBytes = global::Models.Models.tempModel.nIndexBytes)
							{
								float* ptr = (float*)vertexBytes;
								float* ptr2 = (float*)normalBytes;
								int* ptr3 = (int*)vIndexBytes;
								int* ptr4 = (int*)nIndexBytes;
								int i = 0;
								for (m = 0; m < global::Models.Models.tempModel.pcount; m++)
								{
									int num4 = m * 3;
									int num5 = ptr3[num4];
									int num7 = ptr4[num4];
									ValidateCollisionIndices(array4[l], m, 0, num5, num7);
									int num6 = num5 * 3;
									num7 *= 3;
									cModels[l].v[i].X = ptr[num6];
									cModels[l].v[i].Y = (ptr + num6)[1];
									cModels[l].v[i].Z = (ptr + num6)[2];
									cModels[l].n[i].X = ptr2[num7];
									cModels[l].n[i].Y = (ptr2 + num7)[1];
									cModels[l].n[i].Z = (ptr2 + num7)[2];
									i++;
									num5 = (ptr3 + num4)[1];
									num6 = num5 * 3;
									num7 = (ptr4 + num4)[1];
									ValidateCollisionIndices(array4[l], m, 1, num5, num7);
									num7 *= 3;
									cModels[l].v[i].X = ptr[num6];
									cModels[l].v[i].Y = (ptr + num6)[1];
									cModels[l].v[i].Z = (ptr + num6)[2];
									cModels[l].n[i].X = ptr2[num7];
									cModels[l].n[i].Y = (ptr2 + num7)[1];
									cModels[l].n[i].Z = (ptr2 + num7)[2];
									i++;
									num5 = (ptr3 + num4)[2];
									num6 = num5 * 3;
									num7 = (ptr4 + num4)[2];
									ValidateCollisionIndices(array4[l], m, 2, num5, num7);
									num7 *= 3;
									cModels[l].v[i].X = ptr[num6];
									cModels[l].v[i].Y = (ptr + num6)[1];
									cModels[l].v[i].Z = (ptr + num6)[2];
									cModels[l].n[i].X = ptr2[num7];
									cModels[l].n[i].Y = (ptr2 + num7)[1];
									cModels[l].n[i].Z = (ptr2 + num7)[2];
									i++;
								}
							}
						}
					}
				}
				if (global::Models.Models.tempModel.pcount == 0)
				{
					Create_Collision_Model_Bounding_Box((ushort)l);
					continue;
				}
				cModels[l].polygonCount = (ushort)global::Models.Models.tempModel.pcount;
				cModels[l].collisionScheme = global::Models.Models.tempColModel.collisionScheme;
				cModels[l].curDiv = global::Models.Models.tempColModel.curDiv;
				cModels[l].dx = global::Models.Models.tempColModel.dx;
				cModels[l].dy = global::Models.Models.tempColModel.dy;
				cModels[l].minX = global::Models.Models.tempColModel.minX;
				cModels[l].minY = global::Models.Models.tempColModel.minY;
				cModels[l].numBoxes = global::Models.Models.tempColModel.numBoxes;
				cModels[l].id = global::Models.Models.tempColModel.id;
				numBoxes = global::Models.Models.tempColModel.numBoxes;
				cModels[l].cb = new StructsClass.Collision_Model_Box[numBoxes];
				for (int i = 0; i < numBoxes; i++)
				{
					cModels[l].cb[i] = default(StructsClass.Collision_Model_Box);
				}
				m = numBoxes - cModels[l].polygonCount + 1;
				if (m > numAllocatedCollisionBoxList)
				{
					cBoxList = new int[4, m];
					boxItemIndex = new byte[4, m];
					numAllocatedCollisionBoxList = m;
				}
				for (int i = 0; i < numBoxes; i++)
				{
					cModels[l].cb[i].type = global::Models.Models.tempColModel.cb[i].type;
					cModels[l].cb[i].id = global::Models.Models.tempColModel.cb[i].id;
					cModels[l].cb[i].x = global::Models.Models.tempColModel.cb[i].x;
					cModels[l].cb[i].y = global::Models.Models.tempColModel.cb[i].y;
					cModels[l].cb[i].z = global::Models.Models.tempColModel.cb[i].z;
					cModels[l].cb[i].x2 = global::Models.Models.tempColModel.cb[i].x2;
					cModels[l].cb[i].y2 = global::Models.Models.tempColModel.cb[i].y2;
					cModels[l].cb[i].z2 = global::Models.Models.tempColModel.cb[i].z2;
					cModels[l].cb[i].numIDs = global::Models.Models.tempColModel.cb[i].numIDs;
					cModels[l].cb[i].ids = new ushort[cModels[l].cb[i].numIDs];
					for (m = 0; m < cModels[l].cb[i].numIDs; m++)
					{
						cModels[l].cb[i].ids[m] = global::Models.Models.tempColModel.cb[i].ids[m];
					}
				}
			}
			stream.Close();
		}
	}

	private static void ValidateCollisionIndices(string fileName, int polygon, int corner, int vertexIndex, int normalIndex)
	{
		int vertexCount = global::Models.Models.tempModel.vcount;
		int normalCount = global::Models.Models.tempModel.ncount;
		if ((uint)vertexIndex >= (uint)vertexCount || (uint)normalIndex >= (uint)normalCount)
		{
			throw new InvalidDataException($"Invalid collision indices in {fileName}: polygon={polygon}, corner={corner}, vertex={vertexIndex}/{vertexCount}, normal={normalIndex}/{normalCount}.");
		}
	}

	public void Create_Collision_Model_Bounding_Box(ushort modID)
	{
		_ = global::Rendering.Rendering.uBufferID;
		ushort num = 2;
		int num2 = 64;
		int num3 = 0;
		int num4;
		int num5;
		if (cModels[modID].fileName.StartsWith("scheme1_"))
		{
			num4 = num2 * num2;
			cModels[modID].collisionScheme = 0;
			cModels[modID].curDiv = num2;
		}
		else
		{
			cModels[modID].collisionScheme = 1;
			num5 = (global::Models.Models.tempModel.pcount + (num - 1)) / num;
			num4 = num5;
			while (num5 > 1)
			{
				num5 = (num5 + (num - 1)) / num;
				num4 += num5;
			}
		}
		num5 = num4 + 1;
		if (num5 > numAllocatedCollisionBoxList)
		{
			cBoxList = new int[4, num5];
			boxItemIndex = new byte[4, num5];
			numAllocatedCollisionBoxList = num5;
		}
		num4 += global::Models.Models.tempModel.pcount;
		cModels[modID].numBoxes = num4;
		cModels[modID].cb = new StructsClass.Collision_Model_Box[num4];
		int i;
		for (i = 0; i < num4; i++)
		{
			cModels[modID].cb[i] = default(StructsClass.Collision_Model_Box);
			cModels[modID].cb[i].ids = new ushort[num];
		}
		num3 = 0;
		float num6 = cModels[modID].v[0].X;
		float num7 = num6;
		float num8 = cModels[modID].v[0].Y;
		float num9 = num8;
		float num10 = cModels[modID].v[0].Z;
		float num11 = num10;
		i = 0;
		for (num5 = 0; num5 < global::Models.Models.tempModel.pcount; num5++)
		{
			float x = cModels[modID].v[i].X;
			float x2 = cModels[modID].v[i].X;
			float y = cModels[modID].v[i].Y;
			float y2 = cModels[modID].v[i].Y;
			float z = cModels[modID].v[i].Z;
			float z2 = cModels[modID].v[i].Z;
			i++;
			if (cModels[modID].v[i].X < x)
			{
				x = cModels[modID].v[i].X;
			}
			if (cModels[modID].v[i].X > x2)
			{
				x2 = cModels[modID].v[i].X;
			}
			if (cModels[modID].v[i].Y < y)
			{
				y = cModels[modID].v[i].Y;
			}
			if (cModels[modID].v[i].Y > y2)
			{
				y2 = cModels[modID].v[i].Y;
			}
			if (cModels[modID].v[i].Z < z)
			{
				z = cModels[modID].v[i].Z;
			}
			if (cModels[modID].v[i].Z > z2)
			{
				z2 = cModels[modID].v[i].Z;
			}
			i++;
			if (cModels[modID].v[i].X < x)
			{
				x = cModels[modID].v[i].X;
			}
			if (cModels[modID].v[i].X > x2)
			{
				x2 = cModels[modID].v[i].X;
			}
			if (cModels[modID].v[i].Y < y)
			{
				y = cModels[modID].v[i].Y;
			}
			if (cModels[modID].v[i].Y > y2)
			{
				y2 = cModels[modID].v[i].Y;
			}
			if (cModels[modID].v[i].Z < z)
			{
				z = cModels[modID].v[i].Z;
			}
			if (cModels[modID].v[i].Z > z2)
			{
				z2 = cModels[modID].v[i].Z;
			}
			i++;
			x -= 0.25f;
			x2 += 0.25f;
			y -= 0.25f;
			y2 += 0.25f;
			z -= 0.25f;
			z2 += 0.25f;
			cModels[modID].cb[num3].id = (ushort)num5;
			cModels[modID].cb[num3].x = x;
			cModels[modID].cb[num3].x2 = x2;
			cModels[modID].cb[num3].y = y;
			cModels[modID].cb[num3].y2 = y2;
			cModels[modID].cb[num3].z = z;
			cModels[modID].cb[num3].z2 = z2;
			cModels[modID].cb[num3].type = 0;
			cModels[modID].cb[num3].status = 0;
			num3++;
			if (x < num6)
			{
				num6 = x;
			}
			if (x2 > num7)
			{
				num7 = x2;
			}
			if (y < num8)
			{
				num8 = y;
			}
			if (y2 > num9)
			{
				num9 = y2;
			}
			if (z < num10)
			{
				num10 = z;
			}
			if (z2 > num11)
			{
				num11 = z2;
			}
		}
		if (cModels[modID].collisionScheme == 0)
		{
			float num12 = (num7 - num6) / (float)num2;
			float num13 = (num9 - num8) / (float)num2;
			cModels[modID].dx = num12;
			cModels[modID].dy = num13;
			cModels[modID].minX = num6;
			cModels[modID].minY = num8;
			cModels[modID].polygonCount = (ushort)global::Models.Models.tempModel.pcount;
			for (i = 0; i < num2; i++)
			{
				float x = num6 + num12 * (float)i;
				float x2 = x + num12;
				int num14 = num3 + i * num2;
				num5 = 0;
				int num15 = 0;
				for (; num5 < num2; num5++)
				{
					float y = num8 + num13 * (float)num5;
					float y2 = y + num13;
					int num16 = num14 + num5;
					int num17 = 0;
					int num18 = 0;
					num15 = 0;
					int num19 = 1;
					int num20 = 2;
					while (num18 < global::Models.Models.tempModel.pcount)
					{
						if ((cModels[modID].v[num15].X >= x && cModels[modID].v[num15].X <= x2 && cModels[modID].v[num15].Y >= y && cModels[modID].v[num15].Y <= y2) || (cModels[modID].v[num19].X >= x && cModels[modID].v[num19].X <= x2 && cModels[modID].v[num19].Y >= y && cModels[modID].v[num19].Y <= y2) || (cModels[modID].v[num20].X >= x && cModels[modID].v[num20].X <= x2 && cModels[modID].v[num20].Y >= y && cModels[modID].v[num20].Y <= y2))
						{
							num17++;
						}
						num18++;
						num15 += 3;
						num19 += 3;
						num20 += 3;
					}
					cModels[modID].cb[num16].numIDs = (ushort)num17;
					if (num17 > 0)
					{
						cModels[modID].cb[num16].ids = new ushort[num17];
						num17 = 0;
						num18 = 0;
						num15 = 0;
						num19 = 1;
						num20 = 2;
						while (num18 < global::Models.Models.tempModel.pcount)
						{
							if ((cModels[modID].v[num15].X >= x && cModels[modID].v[num15].X <= x2 && cModels[modID].v[num15].Y >= y && cModels[modID].v[num15].Y <= y2) || (cModels[modID].v[num19].X >= x && cModels[modID].v[num19].X <= x2 && cModels[modID].v[num19].Y >= y && cModels[modID].v[num19].Y <= y2) || (cModels[modID].v[num20].X >= x && cModels[modID].v[num20].X <= x2 && cModels[modID].v[num20].Y >= y && cModels[modID].v[num20].Y <= y2))
							{
								cModels[modID].cb[num16].ids[num17++] = (ushort)num18;
							}
							num18++;
							num15 += 3;
							num19 += 3;
							num20 += 3;
						}
					}
					cModels[modID].cb[num16].type = 1;
				}
			}
			return;
		}
		int num21 = num3;
		for (i = num3; i < num4; i++)
		{
			cModels[modID].cb[i].status = 0;
		}
		float num22 = (num7 - num6) * (num9 - num8) * (num11 - num10);
		num22 *= 1.5f;
		while (num21 < num4)
		{
			i = 0;
			int num23 = 0;
			for (; i < num21; i++)
			{
				if (cModels[modID].cb[i].status > 0)
				{
					continue;
				}
				float num24 = num22;
				float x = cModels[modID].cb[i].x;
				float x2 = cModels[modID].cb[i].x2;
				float y = cModels[modID].cb[i].y;
				float y2 = cModels[modID].cb[i].y2;
				float z = cModels[modID].cb[i].z;
				float z2 = cModels[modID].cb[i].z2;
				cModels[modID].cb[num3].numIDs = 1;
				cModels[modID].cb[num3].ids[0] = (ushort)i;
				int num25 = 0;
				for (num23 = 1; num23 < num; num23++)
				{
					if (num25 <= -1)
					{
						break;
					}
					num25 = -1;
					num24 = num22;
					for (num5 = i + 1; num5 < num21; num5++)
					{
						if (cModels[modID].cb[num5].status <= 0)
						{
							float x3 = cModels[modID].cb[num5].x;
							if (cModels[modID].cb[i].x < cModels[modID].cb[num5].x)
							{
								x3 = cModels[modID].cb[i].x;
							}
							float y3 = cModels[modID].cb[num5].y;
							if (cModels[modID].cb[i].y < cModels[modID].cb[num5].y)
							{
								y3 = cModels[modID].cb[i].y;
							}
							float z3 = cModels[modID].cb[num5].z;
							if (cModels[modID].cb[i].z < cModels[modID].cb[num5].z)
							{
								z3 = cModels[modID].cb[i].z;
							}
							float num26 = cModels[modID].cb[num5].x2 - x3;
							if (cModels[modID].cb[i].x2 > cModels[modID].cb[num5].x2)
							{
								num26 = cModels[modID].cb[i].x2 - x3;
							}
							float num27 = cModels[modID].cb[num5].y2 - y3;
							if (cModels[modID].cb[i].y2 > cModels[modID].cb[num5].y2)
							{
								num27 = cModels[modID].cb[i].y2 - y3;
							}
							float num28 = cModels[modID].cb[num5].z2 - z3;
							if (cModels[modID].cb[i].z2 > cModels[modID].cb[num5].z2)
							{
								num28 = cModels[modID].cb[i].z2 - z3;
							}
							if (num26 * num27 * num28 < num24)
							{
								num25 = num5;
								num24 = num26 * num27 * num28;
							}
						}
					}
					if (num25 > -1)
					{
						cModels[modID].cb[num25].status = 1;
						cModels[modID].cb[num3].ids[cModels[modID].cb[num3].numIDs++] = (ushort)num25;
					}
				}
				for (num5 = 0; num5 < cModels[modID].cb[num3].numIDs; num5++)
				{
					if (x > cModels[modID].cb[cModels[modID].cb[num3].ids[num5]].x)
					{
						x = cModels[modID].cb[cModels[modID].cb[num3].ids[num5]].x;
					}
					if (x2 < cModels[modID].cb[cModels[modID].cb[num3].ids[num5]].x2)
					{
						x2 = cModels[modID].cb[cModels[modID].cb[num3].ids[num5]].x2;
					}
					if (y > cModels[modID].cb[cModels[modID].cb[num3].ids[num5]].y)
					{
						y = cModels[modID].cb[cModels[modID].cb[num3].ids[num5]].y;
					}
					if (y2 < cModels[modID].cb[cModels[modID].cb[num3].ids[num5]].y2)
					{
						y2 = cModels[modID].cb[cModels[modID].cb[num3].ids[num5]].y2;
					}
					if (z > cModels[modID].cb[cModels[modID].cb[num3].ids[num5]].z)
					{
						z = cModels[modID].cb[cModels[modID].cb[num3].ids[num5]].z;
					}
					if (z2 < cModels[modID].cb[cModels[modID].cb[num3].ids[num5]].z2)
					{
						z2 = cModels[modID].cb[cModels[modID].cb[num3].ids[num5]].z2;
					}
				}
				cModels[modID].cb[num3].x = x;
				cModels[modID].cb[num3].y = y;
				cModels[modID].cb[num3].z = z;
				cModels[modID].cb[num3].x2 = x2;
				cModels[modID].cb[num3].y2 = y2;
				cModels[modID].cb[num3].z2 = z2;
				cModels[modID].cb[num3].type = 1;
				cModels[modID].cb[num3].status = 0;
				cModels[modID].cb[i].status = 1;
				num3++;
			}
			num21 = num3;
		}
		cModels[modID].cb[num4 - 1].status = 1;
	}

	public ushort Find_Collision_Model(string fileName, ushort defaultValue)
	{
		for (ushort num = 0; num < global::MainGame.MainGame.numCollisionModels; num++)
		{
			if (cModels[num].fileName.Equals(fileName, StringComparison.OrdinalIgnoreCase))
			{
				return num;
			}
		}
		return defaultValue;
	}
}
