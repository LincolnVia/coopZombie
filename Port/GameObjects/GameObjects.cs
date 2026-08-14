using System;
using System.Globalization;
using System.IO;
using AI;
using MainGame;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using Microsoft.Xna.Framework.Net;
using Models;
using Networking;
using Players;
using Rendering;
using Structs;
using Textures;
using Util;
using Weapons;
using WindowsGame1;

namespace GameObjects;

public class GameObjects
{
	public static bool needRegen;

	public static bool needNewVBO;

	public static bool mpScoreChanged;

	public static bool playersCanDamageTeamObjects = false;

	public static ushort numGameObjects = 0;

	public static ushort numAllocatedGameObjects;

	public static ushort[] objStat;

	public static ushort[] sortPosition;

	public static short numCollections = 0;

	public static short numAllocatedCollections = 0;

	public static short Door1;

	public static short Door2;

	public static short Lift1;

	public static short Lift1a;

	public static short Lift1b;

	public static short Panel1;

	public static short Panel2;

	public static short Panel3;

	public static short Panel4;

	public static short Panel1A;

	public static short Panel1B;

	public static short Panel1C;

	public static short Panel1D;

	public static short Panel1E;

	public static short Panel1F;

	public static short Panel1G;

	public static short Panel1H;

	public static short Panel1I;

	public static short Panel1J;

	public static short Panel2A;

	public static short Panel2B;

	public static short Panel2C;

	public static short Panel2D;

	public static short Panel2E;

	public static short Panel2F;

	public static int numObjects = 0;

	public static int numAllocatedObjects = 0;

	public static int vboIndex;

	public static int ibuffIndex;

	public static int numMainBufferFacesOpaque = 0;

	public static int numMainBufferFacesTransparent = 0;

	public static int[] objPoints = new int[1];

	private static int[,] spidIList = new int[50005, 7];

	private static uint spidIlistCnt = 50005u;

	public static float mpTimeBeforePointsUpdate;

	public static float flameTimer;

	public static long numFaces;

	private static StructsClass.vtex cpftV1;

	private static StructsClass.vtex cpftV2;

	private static StructsClass.vtex cpftV3;

	private static StructsClass.texcoord cpftT1 = new StructsClass.texcoord();

	private static StructsClass.texcoord cpftT2 = new StructsClass.texcoord();

	private static StructsClass.texcoord cpftT3 = new StructsClass.texcoord();

	private static Color cvbosoVcolor = default(Color);

	public static Matrix coMv1 = default(Matrix);

	public static Vector3 cvsoV1 = default(Vector3);

	public static Vector3 cvsoN1 = default(Vector3);

	public static Vector3 cvsoT1 = default(Vector3);

	public static StructsClass.gameobject[] objMaster;

	public static StructsClass.regen_holder[] roPt1 = new StructsClass.regen_holder[1];

	public static StructsClass.Object_Collection[] objCol;

	public static StructsClass.VertexPositionColorNormalTexture[] goStaticVtexOpaque;

	public static StructsClass.VertexPositionColorNormalTexture[] goStaticVtexTransparent;

	public static StructsClass.Game_Object[] Game_Objects;

	public Game1.MasterCollection mainC;

	public void Init_Vars(Game1.MasterCollection master)
	{
		mainC = master;
	}

	public void Init_Objects(byte threadID)
	{
		_ = global::Rendering.Rendering.uBufferID;
		for (int i = 0; i < 1; i++)
		{
			roPt1[i] = default(StructsClass.regen_holder);
			roPt1[i].height = new byte[6];
			roPt1[i].width = new byte[6];
		}
		Setup_Object_Collections();
	}

	public void Setup_Object_Collections()
	{
		byte active = 1;
		byte needsBB = 0;
		for (short num = 0; num < numCollections; num++)
		{
			for (short num2 = 0; num2 < objCol[num].numObj; num2++)
			{
				for (short num3 = 0; num3 < numObjects; num3++)
				{
					if (objStat[num3] > 1 && objMaster[num3].id == objCol[num].objList[num2])
					{
						objCol[num].objList[num2] = (ushort)num3;
						float x = objMaster[num3].x;
						float y = objMaster[num3].y;
						float z = objMaster[num3].z;
						float sx = objMaster[num3].dimX;
						float sy = objMaster[num3].dimY;
						float sz = objMaster[num3].dimZ;
						float tScaleX = objMaster[num3].tScaleX;
						float tScaleY = objMaster[num3].tScaleY;
						objMaster[num3].instanceID = (short)mainC.renderingMain.Add_New_Instance(0, 0, 1, mainC.modelsMain.Find_Model("Square.txt"), 1f, 1f, 1f, 1f, 0f, 0f, 0f, x, y, z, sx, sy, sz, tScaleX, tScaleY, active, needsBB, useVbo: false, usesCollisionModel: false, 10);
						num3 = (short)numObjects;
					}
				}
			}
		}
	}

	public void loadObjects(string fileName, byte threadID)
	{
		short num = 0;
		int num2 = 0;
		float num3 = 0f;
		float num4 = 0f;
		float num5 = 0f;
		long num6 = 0L;
		short newID = 0;
		StructsClass.gameobject g = new StructsClass.gameobject();
		_ = global::Rendering.Rendering.uBufferID;
		for (int i = 0; i < numAllocatedObjects; i++)
		{
			if (objStat[i] > 1)
			{
				Delete_Object(i, threadID);
			}
		}
		numObjects = 0;
		Stream stream = TitleContainer.OpenStream("The_CoOp_Zombie_Game\\Config_Files\\" + fileName);
		byte[] array = new byte[stream.Length];
		if (stream.CanRead)
		{
			StructsClass.Initialize_GameObject(ref g);
			num6 = 0L;
			stream.Read(array, 0, array.Length);
			string text = mainC.utilMain.Byte_Array_To_String(array);
			string[] array2 = text.Split('\n', '\r');
			int j = 0;
			int num7 = 0;
			for (; j < array2.Length; j++)
			{
				if (array2[j].Length > 0)
				{
					num7++;
				}
			}
			if (num7 < 1)
			{
				stream.Close();
				return;
			}
			string[] array3 = new string[num7];
			j = 0;
			num7 = 0;
			for (; j < array2.Length; j++)
			{
				if (array2[j].Length > 0)
				{
					array3[num7++] = array2[j];
				}
			}
			for (j = 0; j < num7; j++)
			{
				array2 = array3[j].Split(' ', '\t');
				int k = 0;
				int num8 = 0;
				for (; k < array2.Length; k++)
				{
					if (array2[k].Length > 0)
					{
						num8++;
					}
				}
				if (num8 < 1)
				{
					continue;
				}
				string[] array4 = new string[num8];
				k = 0;
				num8 = 0;
				for (; k < array2.Length; k++)
				{
					if (array2[k].Length > 0)
					{
						array4[num8++] = array2[k];
					}
				}
				int num9 = 0;
				if (array4[0].Equals("object", StringComparison.OrdinalIgnoreCase))
				{
					num9 = 1;
				}
				else if (array4[0].Equals("angle", StringComparison.OrdinalIgnoreCase))
				{
					num9 = 2;
				}
				else if (array4[0].Equals("coords", StringComparison.OrdinalIgnoreCase))
				{
					num9 = 3;
				}
				else if (array4[0].Equals("texture", StringComparison.OrdinalIgnoreCase))
				{
					num9 = 4;
				}
				else if (array4[0].Equals("particles", StringComparison.OrdinalIgnoreCase))
				{
					num9 = 5;
				}
				else if (array4[0].Equals("pScale", StringComparison.OrdinalIgnoreCase))
				{
					num9 = 6;
				}
				else if (array4[0].Equals("tScale", StringComparison.OrdinalIgnoreCase))
				{
					num9 = 7;
				}
				else if (array4[0].Equals("color", StringComparison.OrdinalIgnoreCase))
				{
					num9 = 8;
				}
				else if (array4[0].Equals("id", StringComparison.OrdinalIgnoreCase))
				{
					num9 = 9;
				}
				else if (array4[0].Equals("type", StringComparison.OrdinalIgnoreCase))
				{
					num9 = 10;
				}
				else if (array4[0].Equals("numObjects", StringComparison.OrdinalIgnoreCase))
				{
					num9 = 11;
				}
				else if (array4[0].Equals("destructable", StringComparison.OrdinalIgnoreCase))
				{
					num9 = 12;
				}
				else if (array4[0].Equals("model", StringComparison.OrdinalIgnoreCase))
				{
					num9 = 13;
				}
				else if (array4[0].Equals("VBO", StringComparison.OrdinalIgnoreCase))
				{
					num9 = 14;
				}
				switch (num9)
				{
				case 1:
					if (num6 > 0)
					{
						if ((g.type & 5) > 0)
						{
							Create_Object(oIsNull: true, ref newID, g.x, g.y, g.z, g.x, g.y, g.z, num3, num4, num5, g.dimX, g.dimY, g.dimZ, ref g.texture, callRegen: true, 1f, g.tScaleX, 0f - g.tScaleY, g.pScale, ref g.color, g.id, g.faces, g.destructable, g.type, g.texIDs, threadID);
						}
						else if ((g.type & 0xA8) > 0)
						{
							Matrix matrix = Matrix.CreateRotationX(num3 * ((float)Math.PI / 180f));
							matrix = Matrix.CreateRotationY(num4 * ((float)Math.PI / 180f)) * matrix;
							matrix = Matrix.CreateRotationZ(num5 * ((float)Math.PI / 180f)) * matrix;
							Create_Collision_Object(ref newID, g.x, g.y, g.z, ref matrix, g.dimX, g.dimY, g.dimZ, num3, num4, num5, g.id, g.type, threadID);
						}
					}
					num++;
					g.tScaleX = (g.tScaleY = (g.pScale = 1f));
					g.x = (g.y = (g.z = 0f));
					num3 = (num4 = (num5 = 0f));
					g.dimX = 1u;
					g.dimY = 1u;
					g.dimZ = 1u;
					g.id = -1;
					g.texIDs[0] = -1;
					g.texIDs[1] = -1;
					g.texIDs[2] = -1;
					g.texIDs[3] = -1;
					g.texIDs[4] = -1;
					g.texIDs[5] = -1;
					g.faces = 63;
					g.destructable = false;
					g.color[0] = (g.color[1] = (g.color[2] = (g.color[3] = 1f)));
					g.type = 132;
					num2 = 0;
					num6 = 1L;
					break;
				case 2:
					if (array4.Length > 3)
					{
						num3 = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
						num4 = float.Parse(array4[2], CultureInfo.InvariantCulture.NumberFormat);
						num5 = float.Parse(array4[3], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 3:
					if (array4.Length > 3)
					{
						g.x = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
						g.y = float.Parse(array4[2], CultureInfo.InvariantCulture.NumberFormat);
						g.z = float.Parse(array4[3], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 4:
					if (array4.Length <= 1)
					{
						break;
					}
					g.texture = array4[1];
					if (num2 > -1 && num2 < 6)
					{
						if (num2 == 0)
						{
							g.texIDs[0] = mainC.texturesMain.Find_Texture(array4[1], -1);
							g.texIDs[1] = g.texIDs[0];
							g.texIDs[2] = g.texIDs[0];
							g.texIDs[3] = g.texIDs[0];
							g.texIDs[4] = g.texIDs[0];
							g.texIDs[5] = g.texIDs[0];
						}
						else
						{
							g.texIDs[num2] = mainC.texturesMain.Find_Texture(array4[1], -1);
						}
						num2++;
					}
					break;
				case 5:
					if (array4.Length > 3)
					{
						g.dimX = uint.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
						g.dimY = uint.Parse(array4[2], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
						g.dimZ = uint.Parse(array4[3], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 6:
					if (array4.Length > 1)
					{
						g.pScale = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 7:
					if (array4.Length > 2)
					{
						g.tScaleX = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
						g.tScaleY = float.Parse(array4[2], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 8:
					if (array4.Length > 4)
					{
						g.color[0] = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
						g.color[1] = float.Parse(array4[2], CultureInfo.InvariantCulture.NumberFormat);
						g.color[2] = float.Parse(array4[3], CultureInfo.InvariantCulture.NumberFormat);
						g.color[3] = float.Parse(array4[4], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 9:
					if (array4.Length > 1)
					{
						g.id = byte.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 10:
					if (array4.Length > 1)
					{
						g.type = byte.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 11:
				{
					if (array4.Length <= 1)
					{
						break;
					}
					int i = int.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
					if (i > numAllocatedObjects)
					{
						objStat = new ushort[i];
						objMaster = new StructsClass.gameobject[i];
						numAllocatedObjects = i;
						for (int l = 0; l < i; l++)
						{
							objStat[l] = 0;
						}
					}
					numObjects = i;
					break;
				}
				case 12:
					g.destructable = true;
					g.type = 129;
					break;
				}
			}
		}
		if (num6 > 0)
		{
			if ((g.type & 5) > 0)
			{
				Create_Object(oIsNull: true, ref newID, g.x, g.y, g.z, g.x, g.y, g.z, num3, num4, num5, g.dimX, g.dimY, g.dimZ, ref g.texture, callRegen: true, 1f, g.tScaleX, 0f - g.tScaleY, g.pScale, ref g.color, g.id, g.faces, g.destructable, g.type, g.texIDs, threadID);
			}
			else if ((g.type & 0xA8) > 0)
			{
				Matrix matrix = Matrix.CreateRotationX(num3 * ((float)Math.PI / 180f));
				matrix = Matrix.CreateRotationY(num4 * ((float)Math.PI / 180f)) * matrix;
				matrix = Matrix.CreateRotationZ(num5 * ((float)Math.PI / 180f)) * matrix;
				Create_Collision_Object(ref newID, g.x, g.y, g.z, ref matrix, g.dimX, g.dimY, g.dimZ, num3, num4, num5, g.id, g.type, threadID);
			}
		}
		stream.Close();
	}

	public void Load_Objects(string fileName, byte threadID)
	{
		_ = global::Rendering.Rendering.uBufferID;
		int num = -1;
		numGameObjects = 0;
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
				else if (array4[0].Equals("object", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 2;
				}
				else if (array4[0].Equals("Rotation", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 3;
				}
				else if (array4[0].Equals("Position", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 4;
				}
				else if (array4[0].Equals("Color", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 5;
				}
				else if (array4[0].Equals("id", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 6;
				}
				else if (array4[0].Equals("type", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 7;
				}
				else if (array4[0].Equals("Model", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 8;
				}
				else if (array4[0].Equals("Collision_Models", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 9;
				}
				else if (array4[0].Equals("Bounding_Sphere_Radius", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 10;
				}
				else if (array4[0].Equals("InActive", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 11;
				}
				else if (array4[0].Equals("Physics", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 12;
				}
				else if (array4[0].Equals("Collision_Points", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 13;
				}
				else if (array4[0].Equals("Particle_Models", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 14;
				}
				else if (array4[0].Equals("Scale", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 15;
				}
				else if (array4[0].Equals("DamageType", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 16;
				}
				else if (array4[0].Equals("Damage", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 17;
				}
				else if (array4[0].Equals("TargetID", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 18;
				}
				else if (array4[0].Equals("Sound_Destroyed", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 19;
				}
				else if (array4[0].Equals("Points", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 20;
				}
				else if (array4[0].Equals("DamageBarHeight", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 21;
				}
				else if (array4[0].Equals("ModelListID", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 22;
				}
				else if (array4[0].Equals("CollisionModelListID", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 23;
				}
				else if (array4[0].Equals("DestroyedParticle", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 24;
				}
				else if (array4[0].Equals("ObjectToDrop", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 25;
				}
				else if (array4[0].Equals("Explosion", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 26;
				}
				else if (array4[0].Equals("Sound_Repaired", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 27;
				}
				switch (num4)
				{
				case 1:
					if (array4.Length > 1)
					{
						int num5 = int.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
						Game_Objects = new StructsClass.Game_Object[num5];
						sortPosition = new ushort[num5];
						for (int num6 = 0; num6 < num5; num6++)
						{
							Game_Objects[num6].explosionID = -1;
							Game_Objects[num6].boundingRadius = 0f;
							Game_Objects[num6].colorR = 1f;
							Game_Objects[num6].colorG = 1f;
							Game_Objects[num6].colorB = 1f;
							Game_Objects[num6].colorA = 1f;
							Game_Objects[num6].ID = 0;
							Game_Objects[num6].numModels = 0;
							Game_Objects[num6].mv1 = Matrix.Identity;
							Game_Objects[num6].mvStart = Matrix.Identity;
							StructsClass.Initialize_Physics_New(ref Game_Objects[num6].phy);
							Game_Objects[num6].rotX = 0f;
							Game_Objects[num6].rotY = 0f;
							Game_Objects[num6].rotZ = 0f;
							Game_Objects[num6].scaleX = 1f;
							Game_Objects[num6].scaleY = 1f;
							Game_Objects[num6].scaleZ = 1f;
							Game_Objects[num6].type = 0;
							Game_Objects[num6].startsActive = true;
							Game_Objects[num6].numPts = 0;
							Game_Objects[num6].usesPhysics = false;
							Game_Objects[num6].numCollisionModels = 0;
							Game_Objects[num6].numParticleModels = 0;
							Game_Objects[num6].damageType = 0;
							Game_Objects[num6].isTarget = false;
							Game_Objects[num6].points = 0;
							Game_Objects[num6].modelListID = global::Util.Util.maxUnsignedShortValue;
							Game_Objects[num6].collisionModelListID = global::Util.Util.maxUnsignedShortValue;
							Game_Objects[num6].destroyedParticleID = 0;
							Game_Objects[num6].objectDroppedOnDestruction = global::Util.Util.maxUnsignedShortValue;
						}
						numGameObjects = (ushort)num5;
						numAllocatedGameObjects = (ushort)num5;
						num = -1;
					}
					break;
				case 2:
					num++;
					if (num < 0 || num >= numGameObjects)
					{
						num = -1;
					}
					break;
				case 3:
					if (array4.Length > 3 && num > -1)
					{
						Game_Objects[num].rotX = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
						Game_Objects[num].rotY = float.Parse(array4[2], CultureInfo.InvariantCulture.NumberFormat);
						Game_Objects[num].rotZ = float.Parse(array4[3], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 4:
					if (array4.Length > 3 && num > -1)
					{
						Game_Objects[num].x = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
						Game_Objects[num].y = float.Parse(array4[2], CultureInfo.InvariantCulture.NumberFormat);
						Game_Objects[num].z = float.Parse(array4[3], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 5:
					if (array4.Length > 4 && num > -1)
					{
						Game_Objects[num].colorR = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
						Game_Objects[num].colorG = float.Parse(array4[2], CultureInfo.InvariantCulture.NumberFormat);
						Game_Objects[num].colorB = float.Parse(array4[3], CultureInfo.InvariantCulture.NumberFormat);
						Game_Objects[num].colorA = float.Parse(array4[4], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 6:
					if (array4.Length > 1 && num > -1)
					{
						Game_Objects[num].ID = ushort.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 7:
					if (array4.Length > 1 && num > -1)
					{
						Game_Objects[num].type = byte.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 8:
					if (array4.Length > 1 && num > -1)
					{
						int num5 = array4.Length - 1;
						Game_Objects[num].numModels = (byte)num5;
						Game_Objects[num].modID = new ushort[num5];
						int num6 = 0;
						int num7 = 1;
						while (num6 < num5)
						{
							Game_Objects[num].modID[num6] = mainC.modelsMain.Find_Model(array4[num7]);
							num6++;
							num7++;
						}
					}
					break;
				case 9:
					if (array4.Length > 1 && num > -1)
					{
						int num5 = (array4.Length - 1) / 2;
						Game_Objects[num].numCollisionModels = (byte)num5;
						Game_Objects[num].colModels = new ushort[num5];
						Game_Objects[num].colModelZones = new ushort[num5];
						int num6 = 0;
						int num7 = 1;
						for (; num6 < num5; num6++)
						{
							Game_Objects[num].colModelZones[num6] = ushort.Parse(array4[num7++], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
							Game_Objects[num].colModels[num6] = mainC.collisionMain.Find_Collision_Model(array4[num7++], 0);
						}
					}
					break;
				case 10:
					if (array4.Length > 1 && num > -1)
					{
						Game_Objects[num].boundingRadius = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 11:
					if (array4.Length > 1 && num > -1)
					{
						Game_Objects[num].startsActive = false;
					}
					break;
				case 12:
					if (array4.Length > 1 && num > -1)
					{
						Game_Objects[num].usesPhysics = true;
					}
					break;
				case 13:
				{
					if (array4.Length <= 1 || num <= -1)
					{
						break;
					}
					int num5 = array4.Length - 1;
					int num6 = num5 / 3;
					Game_Objects[num6].numPts = (ushort)num6;
					if (array4.Length > num6 * 3)
					{
						Game_Objects[num].collisionPoints = new float[num5];
						num6 = 0;
						int num7 = 1;
						for (; num6 < num5; num6++)
						{
							Game_Objects[num].collisionPoints[num6] = float.Parse(array4[num7++], CultureInfo.InvariantCulture.NumberFormat);
						}
					}
					break;
				}
				case 14:
				{
					if (array4.Length <= 1 || num <= -1)
					{
						break;
					}
					int num5 = ushort.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
					Game_Objects[num].numParticleModels = (byte)num5;
					Game_Objects[num].particleModels = new ushort[num5];
					if (array4.Length > num5 + 1)
					{
						int num6 = 0;
						int num7 = 2;
						for (; num6 < num5; num6++)
						{
							Game_Objects[num].particleModels[num6] = mainC.modelsMain.Find_Model(array4[num7++]);
						}
					}
					else
					{
						Game_Objects[num].numParticleModels = 0;
					}
					break;
				}
				case 15:
					if (array4.Length > 3 && num > -1)
					{
						Game_Objects[num].scaleX = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
						Game_Objects[num].scaleY = float.Parse(array4[2], CultureInfo.InvariantCulture.NumberFormat);
						Game_Objects[num].scaleZ = float.Parse(array4[3], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 16:
					if (array4.Length > 1 && num > -1)
					{
						Game_Objects[num].damageType = byte.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 17:
					if (array4.Length > 1 && num > -1)
					{
						Game_Objects[num].maxDamage = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 18:
					if (array4.Length > 1 && num > -1)
					{
						Game_Objects[num].isTarget = true;
						Game_Objects[num].targetID = ushort.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 19:
					if (array4.Length > 1 && num > -1)
					{
						Game_Objects[num].snd_Destroyed = array4[1];
					}
					break;
				case 20:
					if (array4.Length > 1 && num > -1)
					{
						Game_Objects[num].points = int.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 21:
					if (array4.Length > 1 && num > -1)
					{
						Game_Objects[num].damageBarHeight = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 22:
					if (array4.Length > 1 && num > -1)
					{
						Game_Objects[num].modelListID = ushort.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 23:
					if (array4.Length > 1 && num > -1)
					{
						Game_Objects[num].collisionModelListID = ushort.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 24:
					if (array4.Length > 1 && num > -1)
					{
						Game_Objects[num].destroyedParticleID = ushort.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 25:
					if (array4.Length > 1 && num > -1)
					{
						Game_Objects[num].objectDroppedOnDestruction = ushort.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 26:
					if (array4.Length > 1 && num > -1)
					{
						Game_Objects[num].explosionID = sbyte.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 27:
					if (array4.Length > 1 && num > -1)
					{
						Game_Objects[num].snd_Repaired = array4[1];
					}
					break;
				}
			}
		}
		stream.Close();
		for (int num6 = 0; num6 < numGameObjects; num6++)
		{
			Game_Objects[num6].gid = mainC.maingameMain.Register_Game_Item(0, Game_Objects[num6].ID, (ushort)num6);
			Game_Objects[num6].damageBarHeight += Game_Objects[num6].z;
			Game_Objects[num6].mvStart = Matrix.CreateScale(Game_Objects[num6].scaleX, Game_Objects[num6].scaleY, Game_Objects[num6].scaleZ) * Matrix.CreateRotationY(Game_Objects[num6].rotY * ((float)Math.PI / 180f)) * Matrix.CreateRotationX(Game_Objects[num6].rotX * ((float)Math.PI / 180f)) * Matrix.CreateRotationZ(Game_Objects[num6].rotZ * ((float)Math.PI / 180f));
			Game_Objects[num6].mv1 = Game_Objects[num6].mvStart;
			Game_Objects[num6].qn1 = Quaternion.CreateFromRotationMatrix(Game_Objects[num6].mvStart);
			StructsClass.Reset_Physics_New(ref Game_Objects[num6].phy);
			Game_Objects[num6].phy.x = Game_Objects[num6].x;
			Game_Objects[num6].phy.y = Game_Objects[num6].y;
			Game_Objects[num6].phy.z = Game_Objects[num6].z;
			Game_Objects[num6].mv1.M41 = Game_Objects[num6].x;
			Game_Objects[num6].mv1.M42 = Game_Objects[num6].y;
			Game_Objects[num6].mv1.M43 = Game_Objects[num6].z;
			Game_Objects[num6].state = (byte)(Game_Objects[num6].startsActive ? 1u : 0u);
			Game_Objects[num6].doPhysics = Game_Objects[num6].usesPhysics;
			for (int num5 = 0; num5 < Game_Objects[num6].numCollisionModels; num5++)
			{
				mainC.zonesMain.Add_CollisionModel_To_Zone(Game_Objects[num6].colModelZones[num5], Game_Objects[num6].colModels[num5], Game_Objects[num6].gid, ref Game_Objects[num6].mv1);
			}
			if (Game_Objects[num6].collisionModelListID < global::MainGame.MainGame.numCollisionModels)
			{
				for (int num5 = 0; num5 < global::Models.Models.modelList[Game_Objects[num6].collisionModelListID].numModels; num5++)
				{
					global::Models.Models.modelList[Game_Objects[num6].collisionModelListID].modelID[num5] = (short)mainC.collisionMain.Find_Collision_Model(global::Models.Models.modelList[Game_Objects[num6].collisionModelListID].modelName[num5], 0);
				}
				mainC.zonesMain.Add_CollisionModel_To_Zone(global::Models.Models.modelList[Game_Objects[num6].collisionModelListID].bufferID[0], (ushort)global::Models.Models.modelList[Game_Objects[num6].collisionModelListID].modelID[0], Game_Objects[num6].gid, ref Game_Objects[num6].mv1);
				mainC.zonesMain.Add_CollisionModel_To_Zone(global::Models.Models.modelList[Game_Objects[num6].collisionModelListID].bufferID[1], (ushort)global::Models.Models.modelList[Game_Objects[num6].collisionModelListID].modelID[1], Game_Objects[num6].gid, ref Game_Objects[num6].mv1);
			}
		}
	}

	public void Load_Object_Collections(string fileName)
	{
		short num = 0;
		int num2 = -1;
		_ = global::Rendering.Rendering.uBufferID;
		for (int i = 0; i < numCollections; i++)
		{
			objCol[i].active = false;
			objCol[i].numObj = 0;
		}
		Stream stream = TitleContainer.OpenStream("The_CoOp_Zombie_Game\\Config_Files\\" + fileName);
		byte[] array = new byte[stream.Length];
		if (stream.CanRead)
		{
			stream.Read(array, 0, array.Length);
			string text = mainC.utilMain.Byte_Array_To_String(array);
			string[] array2 = text.Split('\n', '\r');
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
				stream.Close();
				return;
			}
			string[] array3 = new string[num3];
			j = 0;
			num3 = 0;
			for (; j < array2.Length; j++)
			{
				if (array2[j].Length > 0)
				{
					array3[num3++] = array2[j];
				}
			}
			for (j = 0; j < num3; j++)
			{
				array2 = array3[j].Split(' ', '\t');
				int k = 0;
				int num4 = 0;
				for (; k < array2.Length; k++)
				{
					if (array2[k].Length > 0)
					{
						num4++;
					}
				}
				if (num4 < 1)
				{
					continue;
				}
				string[] array4 = new string[num4];
				k = 0;
				num4 = 0;
				for (; k < array2.Length; k++)
				{
					if (array2[k].Length > 0)
					{
						array4[num4++] = array2[k];
					}
				}
				int num5 = 0;
				if (array4[0].Equals("Number_Collections", StringComparison.OrdinalIgnoreCase))
				{
					num5 = 1;
				}
				else if (array4[0].Equals("Collection", StringComparison.OrdinalIgnoreCase))
				{
					num5 = 2;
				}
				else if (array4[0].Equals("type", StringComparison.OrdinalIgnoreCase))
				{
					num5 = 3;
				}
				else if (array4[0].Equals("Number_Objects", StringComparison.OrdinalIgnoreCase))
				{
					num5 = 4;
				}
				else if (array4[0].Equals("Objects", StringComparison.OrdinalIgnoreCase))
				{
					num5 = 5;
				}
				else if (array4[0].Equals("Object_Offset", StringComparison.OrdinalIgnoreCase))
				{
					num5 = 6;
				}
				else if (array4[0].Equals("Render_Offset", StringComparison.OrdinalIgnoreCase))
				{
					num5 = 7;
				}
				switch (num5)
				{
				case 1:
					if (array4.Length > 1)
					{
						num = short.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
					}
					if (num > numAllocatedCollections)
					{
						objCol = new StructsClass.Object_Collection[num];
						for (int i = 0; i < num; i++)
						{
							objCol[i].active = false;
							objCol[i].numAllocatedObj = 0;
							objCol[i].numObj = 0;
						}
						numAllocatedCollections = num;
					}
					numCollections = num;
					break;
				case 2:
					if (array4.Length > 1)
					{
						num2 = int.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
						if (num2 > -1 && num2 < numCollections)
						{
							objCol[num2].type = 0;
							objCol[num2].active = true;
							objCol[num2].numObj = 0;
							objCol[num2].moved = false;
							objCol[num2].movedX = 0f;
							objCol[num2].movedY = 0f;
							objCol[num2].movedZ = 0f;
							objCol[num2].rotX = 0f;
							objCol[num2].rotY = 0f;
							objCol[num2].rotZ = 0f;
						}
						else
						{
							num2 = -1;
						}
					}
					break;
				case 3:
					if (array4.Length > 1 && num2 > -1)
					{
						objCol[num2].type = byte.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 4:
					if (array4.Length > 1 && num2 > -1)
					{
						int i = ushort.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
						if (i > objCol[num2].numAllocatedObj)
						{
							objCol[num2].objList = new ushort[i];
							objCol[num2].objOffsetX = new float[i];
							objCol[num2].objOffsetY = new float[i];
							objCol[num2].objOffsetZ = new float[i];
							objCol[num2].renderOffsetX = new float[i];
							objCol[num2].renderOffsetY = new float[i];
							objCol[num2].renderOffsetZ = new float[i];
							objCol[num2].numAllocatedObj = (ushort)i;
						}
						for (int l = 0; l < i; l++)
						{
							objCol[num2].objOffsetX[l] = 0f;
							objCol[num2].objOffsetY[l] = 0f;
							objCol[num2].objOffsetZ[l] = 0f;
							objCol[num2].renderOffsetX[l] = 0f;
							objCol[num2].renderOffsetY[l] = 0f;
							objCol[num2].renderOffsetZ[l] = 0f;
						}
						objCol[num2].numObj = (ushort)i;
					}
					break;
				case 5:
					if (array4.Length > 1 && num2 > -1)
					{
						int i;
						for (i = 0; i < objCol[num2].numObj && i < objCol[num2].numAllocatedObj && i < array4.Length - 1; i++)
						{
							objCol[num2].objList[i] = ushort.Parse(array4[i + 1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
						}
						for (; i < objCol[num2].numObj && i < objCol[num2].numAllocatedObj; i++)
						{
							objCol[num2].objList[i] = objCol[num2].objList[0];
						}
					}
					break;
				case 6:
					if (array4.Length > 4 && num2 > -1)
					{
						int i = int.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
						if (i >= 0 && i < objCol[num2].numObj)
						{
							objCol[num2].objOffsetX[i] = float.Parse(array4[2], CultureInfo.InvariantCulture.NumberFormat);
							objCol[num2].objOffsetY[i] = float.Parse(array4[3], CultureInfo.InvariantCulture.NumberFormat);
							objCol[num2].objOffsetZ[i] = float.Parse(array4[4], CultureInfo.InvariantCulture.NumberFormat);
						}
					}
					break;
				case 7:
					if (array4.Length > 4 && num2 > -1)
					{
						int i = int.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
						if (i >= 0 && i < objCol[num2].numObj)
						{
							objCol[num2].renderOffsetX[i] = float.Parse(array4[2], CultureInfo.InvariantCulture.NumberFormat);
							objCol[num2].renderOffsetY[i] = float.Parse(array4[3], CultureInfo.InvariantCulture.NumberFormat);
							objCol[num2].renderOffsetZ[i] = float.Parse(array4[4], CultureInfo.InvariantCulture.NumberFormat);
						}
					}
					break;
				}
			}
		}
		stream.Close();
	}

	public void Process_Objects(byte threadID)
	{
		flameTimer += global::MainGame.MainGame.frametime;
		global::Rendering.Rendering.npn.v[0] = 0.75f;
		for (int i = 0; i < numGameObjects; i++)
		{
			if (Game_Objects[i].state != 1)
			{
				continue;
			}
			switch (Game_Objects[i].type)
			{
			case 1:
			{
				float num = Game_Objects[i].curDamage / Game_Objects[i].maxDamage;
				if (num > 0f)
				{
					global::Rendering.Rendering.npn.v[1] = num * 25f;
					if (flameTimer > 0.175f)
					{
						mainC.renderingMain.New_Particle_New(18, Game_Objects[i].phy.x, Game_Objects[i].phy.y, Game_Objects[i].phy.z + 35f, 0f, 0f, 1f, 0, threadID);
					}
					Game_Objects[i].particleTimer += global::MainGame.MainGame.frametime;
					if (Game_Objects[i].particleTimer > 0.35f)
					{
						mainC.renderingMain.New_Particle_New(20, Game_Objects[i].phy.x, Game_Objects[i].phy.y, Game_Objects[i].phy.z + 30f, 0f, 0f, 5f, 0, threadID);
						Game_Objects[i].particleTimer = 0f;
					}
				}
				break;
			}
			}
		}
		if (flameTimer > 0.175f)
		{
			flameTimer -= 0.175f;
		}
		for (int i = 0; i < numObjects; i++)
		{
			if (objStat[i] > 1 && objMaster[i].doPhysics && objMaster[i].pList.numUsed > 0)
			{
				objMaster[i].pList.pos1.v[0] = objMaster[i].phys1.position.v[0];
				objMaster[i].pList.pos1.v[1] = objMaster[i].phys1.position.v[1];
				objMaster[i].pList.pos1.v[2] = objMaster[i].phys1.position.v[2];
				float num = objMaster[i].phys1.position.v[0];
				float num2 = objMaster[i].phys1.position.v[1];
				float num3 = objMaster[i].phys1.position.v[2];
				mainC.physicsMain.getPosition(ref objMaster[i].phys1, global::MainGame.MainGame.frametime);
				objMaster[i].moving = true;
				objMaster[i].pList.pos2.v[0] = objMaster[i].phys1.position.v[0];
				objMaster[i].pList.pos2.v[1] = objMaster[i].phys1.position.v[1];
				objMaster[i].pList.pos2.v[2] = objMaster[i].phys1.position.v[2];
				int num4 = mainC.collisionMain.CheckCollsion_Byte(ref objMaster[i].pList, altPaths: false, i, adjustParticles: false, threadID);
				if (num4 > 0)
				{
					objMaster[i].phys1.velocity.v[0] = 0f;
					objMaster[i].phys1.velocity.v[1] = 0f;
					objMaster[i].phys1.velocity.v[2] = 0f;
					objMaster[i].moving = false;
				}
				objMaster[i].phys1.position.v[0] = objMaster[i].pList.pos2.v[0];
				objMaster[i].phys1.position.v[1] = objMaster[i].pList.pos2.v[1];
				objMaster[i].phys1.position.v[2] = objMaster[i].pList.pos2.v[2];
				mainC.collisionMain.Validate_Position(ref objMaster[i].phys1, threadID);
				objMaster[i].x = objMaster[i].pList.pos2.v[0];
				objMaster[i].y = objMaster[i].pList.pos2.v[1];
				objMaster[i].z = objMaster[i].pList.pos2.v[2];
				if (num != objMaster[i].phys1.position.v[0] || num2 != objMaster[i].phys1.position.v[1] || num3 != objMaster[i].phys1.position.v[2])
				{
					Update_Object_BoundingBox(i);
					mainC.collisionMain.Update_CollisionBox((short)i, threadID);
					Update_VBO(i);
				}
			}
		}
		for (int i = 0; i < numCollections; i++)
		{
			if (!objCol[i].active)
			{
				continue;
			}
			if (objCol[i].moved)
			{
				float movedX = objCol[i].movedX;
				float movedY = objCol[i].movedY;
				float movedZ = objCol[i].movedZ;
				for (int num4 = 0; num4 < objCol[i].numObj; num4++)
				{
					_ = objCol[i].frameTime;
					int num5 = objCol[i].objList[num4];
					objMaster[num5].x = objCol[i].x + objCol[i].objOffsetX[num4];
					objMaster[num5].y = objCol[i].y + objCol[i].objOffsetY[num4];
					objMaster[num5].z = objCol[i].z + objCol[i].objOffsetZ[num4];
					mainC.renderingMain.Update_Instance_Position((ushort)objMaster[num5].instanceID, 0, objCol[i].x + objCol[i].renderOffsetX[num4], objCol[i].y + objCol[i].renderOffsetY[num4], objCol[i].z + objCol[i].renderOffsetZ[num4]);
					objMaster[num5].xMoved = movedX;
					objMaster[num5].yMoved = movedY;
					objMaster[num5].zMoved = movedZ;
					objMaster[num5].phys1.velocity.v[0] = objCol[i].velX;
					objMaster[num5].phys1.velocity.v[1] = objCol[i].velY;
					objMaster[num5].phys1.velocity.v[2] = objCol[i].velZ;
					objMaster[num5].moving = true;
					mainC.collisionMain.Update_CollisionBox((short)num5, threadID);
					Update_Object_BoundingBox(num5);
				}
			}
			else
			{
				for (int num4 = 0; num4 < objCol[i].numObj; num4++)
				{
					int num5 = objCol[i].objList[num4];
					objMaster[num5].moving = false;
					objMaster[num5].phys1.velocity.v[0] = 0f;
					objMaster[num5].phys1.velocity.v[1] = 0f;
					objMaster[num5].phys1.velocity.v[2] = 0f;
				}
			}
		}
		if (needRegen)
		{
			Regen_All_Objects(threadID);
		}
	}

	public void Regen_All_Objects(byte threadID)
	{
		long num = 0L;
		bool flag = true;
		while (flag)
		{
			flag = false;
			for (long num2 = 0L; num2 < numObjects; num2++)
			{
				if (objStat[num2] == 4)
				{
					num++;
					if (Consolidate_Object(num2, threadID))
					{
						flag = true;
					}
					objStat[num2] = 2;
					Regen_Object(num2, threadID);
				}
			}
		}
		if (num > 0)
		{
			needNewVBO = true;
		}
	}

	public byte Game_Object_Shot(ushort playerID, ushort objID, float damage, bool isExplosion, byte threadID)
	{
		byte b = 0;
		if (Game_Objects[objID].state == 2)
		{
			return 0;
		}
		switch (Game_Objects[objID].type)
		{
		case 3:
			switch (global::MainGame.MainGame.gameMode)
			{
			case 0:
				Game_Objects[objID].curDamage += damage;
				if (Game_Objects[objID].curDamage >= Game_Objects[objID].maxDamage)
				{
					Game_Objects[objID].curDamage = Game_Objects[objID].maxDamage;
					Game_Object_Destroyed(objID, playerID, threadID);
				}
				break;
			case 1:
				if (playerID == 0 || isExplosion || (playerID >= global::MainGame.MainGame.maxHumanGamePlayers && global::Players.Players.players[playerID].aiID < global::AI.AI.numAI && global::AI.AI.ais[global::Players.Players.players[playerID].aiID].locallyControlled))
				{
					Game_Objects[objID].curDamage += damage;
					if (Game_Objects[objID].curDamage >= Game_Objects[objID].maxDamage)
					{
						Game_Object_Destroyed(objID, playerID, threadID);
					}
					if (!global::Networking.Networking.isHost)
					{
						Send_Object_Damage(objID, damage);
						break;
					}
					if (Game_Objects[objID].curDamage < Game_Objects[objID].maxDamage)
					{
						Send_Object_Damage(objID, damage);
						break;
					}
					global::Networking.Networking.networkUShorts[0] = objID;
					global::Networking.Networking.networkShorts[0] = global::Players.Players.players[playerID].id;
					mainC.networkingMain.XBOX_Send_Network_Message49(49);
				}
				break;
			}
			break;
		case 1:
			b = 2;
			if ((global::Players.Players.players[playerID].teamMask & mainC.playersMain.Get_Team_Mask(mainC.targetMain.Get_Team_For_Target(Game_Objects[objID].targetID))) == 0)
			{
				b = 1;
			}
			else if (!playersCanDamageTeamObjects)
			{
				return 2;
			}
			switch (global::MainGame.MainGame.gameMode)
			{
			case 0:
				Game_Objects[objID].curDamage += damage;
				if (Game_Objects[objID].curDamage >= Game_Objects[objID].maxDamage)
				{
					Game_Objects[objID].curDamage = Game_Objects[objID].maxDamage;
				}
				if (Game_Objects[objID].isTarget)
				{
					mainC.targetMain.Set_DamageTarget_Damage(Game_Objects[objID].targetID, Game_Objects[objID].curDamage);
					global::MainGame.MainGame.gameData.players[playerID].shotsHit += global::Weapons.Weapons.statCounter;
					global::Weapons.Weapons.statCounter = 0;
				}
				if (b == 1)
				{
					Update_Points_For_Damaging_Object_And_Send(playerID, objID, damage);
				}
				if (Game_Objects[objID].curDamage >= Game_Objects[objID].maxDamage)
				{
					Game_Object_Destroyed(objID, playerID, threadID);
					if (b == 1)
					{
						mainC.gameLogic.Game_Scored_Target();
					}
				}
				break;
			case 1:
				if (playerID == 0)
				{
					Game_Objects[objID].curDamage += damage;
					if (Game_Objects[objID].curDamage >= Game_Objects[objID].maxDamage)
					{
						Game_Object_Destroyed(objID, playerID, threadID);
					}
					if (Game_Objects[objID].isTarget)
					{
						mainC.targetMain.Set_DamageTarget_Damage(Game_Objects[objID].targetID, Game_Objects[objID].curDamage);
						global::MainGame.MainGame.gameData.players[playerID].shotsHit += global::Weapons.Weapons.statCounter;
						global::Weapons.Weapons.statCounter = 0;
					}
					if (!global::Networking.Networking.isHost)
					{
						Send_Object_Damage(objID, damage);
					}
					else if (Game_Objects[objID].curDamage < Game_Objects[objID].maxDamage)
					{
						Send_Object_Damage(objID, damage);
					}
					else
					{
						Update_Points_For_Damaging_Object_And_Send(0, objID, damage);
					}
				}
				break;
			}
			break;
		case 2:
		{
			Game_Objects[objID].curDamage += damage;
			if (!(Game_Objects[objID].curDamage >= Game_Objects[objID].maxDamage))
			{
				break;
			}
			if (Game_Objects[objID].isTarget)
			{
				mainC.targetMain.Update_Damage_Target(Game_Objects[objID].targetID, damage);
				b = 2;
				if ((global::Players.Players.players[playerID].teamMask & mainC.playersMain.Get_Team_Mask(mainC.targetMain.Get_Team_For_Target(Game_Objects[objID].targetID))) == 0)
				{
					b = 1;
				}
			}
			ushort numCollisionModels = Game_Objects[objID].numCollisionModels;
			for (ushort num = 0; num < numCollisionModels; num++)
			{
				mainC.zonesMain.Remove_CollisionModel_From_Zone(Game_Objects[objID].colModelZones[num], Game_Objects[objID].gid);
			}
			if (Game_Objects[objID].collisionModelListID < global::MainGame.MainGame.numCollisionModels)
			{
				mainC.zonesMain.Remove_CollisionModel_From_Zone(global::Models.Models.modelList[Game_Objects[objID].collisionModelListID].bufferID[0], Game_Objects[objID].gid);
				mainC.zonesMain.Remove_CollisionModel_From_Zone(global::Models.Models.modelList[Game_Objects[objID].collisionModelListID].bufferID[1], Game_Objects[objID].gid);
			}
			if (b == 1)
			{
				mainC.gameLogic.Game_Scored_Target();
			}
			if (Game_Objects[objID].snd_Destroyed != null)
			{
				mainC.soundsMain.Play_Priority_Sound(Game_Objects[objID].snd_Destroyed, Game_Objects[objID].phy.x, Game_Objects[objID].phy.y, Game_Objects[objID].phy.z, Game_Objects[objID].phy.velocityX, Game_Objects[objID].phy.velocityY, Game_Objects[objID].phy.velocityZ);
			}
			break;
		}
		}
		return b;
	}

	public void Game_Object_Destroyed(ushort objID, ushort playerID, byte threadID)
	{
		Game_Objects[objID].curDamage = Game_Objects[objID].maxDamage;
		if (Game_Objects[objID].state != 1)
		{
			return;
		}
		switch (Game_Objects[objID].type)
		{
		case 3:
			Game_Objects[objID].state = 0;
			if (Game_Objects[objID].modelListID < global::Models.Models.numModelLists)
			{
				Game_Objects[objID].state = 2;
			}
			Set_Game_Objects_Collision_Models_To_Current_State(objID);
			mainC.maingameMain.Game_Object_Drops_Item(Game_Objects[objID].objectDroppedOnDestruction, Game_Objects[objID].x, Game_Objects[objID].y, Game_Objects[objID].z);
			break;
		case 1:
			Game_Objects[objID].state = 0;
			if (Game_Objects[objID].modelListID < global::Models.Models.numModelLists)
			{
				Game_Objects[objID].state = 2;
			}
			Set_Game_Objects_Collision_Models_To_Current_State(objID);
			if (Game_Objects[objID].explosionID > -1)
			{
				mainC.Explosions.New_Explosion((byte)Game_Objects[objID].explosionID, playerID, Game_Objects[objID].phy.x, Game_Objects[objID].phy.y, Game_Objects[objID].phy.z, threadID);
			}
			mainC.renderingMain.New_Particle_New(16, Game_Objects[objID].phy.x, Game_Objects[objID].phy.y, Game_Objects[objID].phy.z, 1.75f, 0f, 0f, 0, threadID);
			break;
		default:
			Game_Objects[objID].state = 2;
			break;
		}
		ushort numParticleModels = Game_Objects[objID].numParticleModels;
		for (ushort num = 0; num < numParticleModels; num++)
		{
			mainC.renderingMain.New_Solid_Particle(2, Game_Objects[objID].phy.x, Game_Objects[objID].phy.y, Game_Objects[objID].phy.z, 0f - Game_Objects[objID].mv1.M31, 0f - Game_Objects[objID].mv1.M32, 0f - Game_Objects[objID].mv1.M33, 0f - Game_Objects[objID].mv1.M11, 0f - Game_Objects[objID].mv1.M12, 0f - Game_Objects[objID].mv1.M13, Game_Objects[objID].phy.velocityX, Game_Objects[objID].phy.velocityY, Game_Objects[objID].phy.velocityZ, global::Rendering.Rendering.MS_Particles[Game_Objects[objID].destroyedParticleID].velocity, global::Rendering.Rendering.MS_Particles[Game_Objects[objID].destroyedParticleID].particleDuration, Game_Objects[objID].particleModels[num]);
		}
		if (Game_Objects[objID].isTarget)
		{
			mainC.targetMain.Set_DamageTarget_Damage(Game_Objects[objID].targetID, Game_Objects[objID].curDamage);
		}
		if (Game_Objects[objID].snd_Destroyed != null)
		{
			mainC.soundsMain.Play_Priority_Sound(Game_Objects[objID].snd_Destroyed, Game_Objects[objID].phy.x, Game_Objects[objID].phy.y, Game_Objects[objID].phy.z, Game_Objects[objID].phy.velocityX, Game_Objects[objID].phy.velocityY, Game_Objects[objID].phy.velocityZ);
		}
	}

	public void Game_Object_Repaired(ushort objID, byte threadID)
	{
		Game_Objects[objID].state = 1;
		Game_Objects[objID].curDamage = 0f;
		mainC.targetMain.Set_DamageTarget_Damage(Game_Objects[objID].targetID, 0f);
		Set_Game_Objects_Collision_Models_To_Current_State(objID);
		if (Game_Objects[objID].snd_Repaired != null)
		{
			mainC.soundsMain.Play_Priority_Sound(Game_Objects[objID].snd_Repaired, Game_Objects[objID].phy.x, Game_Objects[objID].phy.y, Game_Objects[objID].phy.z, Game_Objects[objID].phy.velocityX, Game_Objects[objID].phy.velocityY, Game_Objects[objID].phy.velocityZ);
		}
	}

	public void Render_Game_Objects()
	{
		if (numGameObjects < 1)
		{
			return;
		}
		mainC.modelsMain.Render_Textured_Model_Basic_Setup();
		for (ushort num = 0; num < numGameObjects; num++)
		{
			if (Game_Objects[num].state > 0)
			{
				global::Rendering.Rendering.effect1.Parameters["World"].SetValue(Game_Objects[num].mv1);
				for (ushort num2 = 0; num2 < Game_Objects[num].numModels; num2++)
				{
					mainC.modelsMain.Render_Textured_Model_Basic(Game_Objects[num].modID[num2]);
				}
				if (Game_Objects[num].modelListID < global::Models.Models.numModelLists)
				{
					mainC.modelsMain.Render_Model_List_Item(Game_Objects[num].modelListID, (byte)(Game_Objects[num].state - 1));
				}
			}
		}
		mainC.gameobjectMain.Render_Damage_Bars();
	}

	public void Render_Damage_Bars()
	{
		mainC.renderingMain.Render_Damage_Bar_3D_Setup();
		global::Rendering.Rendering.effect1.Parameters["depth"].SetValue(0);
		global::Rendering.Rendering.effect1.Parameters["BaseTexture"].SetValue(global::Textures.Textures.texMain.texData[global::Textures.Textures.texBlack]);
		for (ushort num = 0; num < numGameObjects; num++)
		{
			if (Game_Objects[num].state > 0 && Game_Objects[num].isTarget)
			{
				mainC.renderingMain.Render_Damage_Bar_Frame_3D(Game_Objects[num].x, Game_Objects[num].y, Game_Objects[num].damageBarHeight, 20, 8);
			}
		}
		global::Rendering.Rendering.effect1.Parameters["depth"].SetValue(0.1f);
		global::Rendering.Rendering.effect1.Parameters["BaseTexture"].SetValue(global::Textures.Textures.texMain.texData[global::Textures.Textures.texWhite]);
		for (ushort num = 0; num < numGameObjects; num++)
		{
			if (Game_Objects[num].state > 0 && Game_Objects[num].isTarget)
			{
				mainC.renderingMain.Render_Damage_Bar_3D(Game_Objects[num].curDamage / Game_Objects[num].maxDamage, Game_Objects[num].x, Game_Objects[num].y, Game_Objects[num].damageBarHeight, 18, 6, 8);
			}
		}
		mainC.renderingMain.Render_Damage_Bar_3D_Cleanup();
	}

	public void Delete_Object(long objID, byte threadID)
	{
		if (objID >= 0 && objID < numObjects)
		{
			mainC.collisionMain.Remove_Object_From_CollisionBoxes(objID);
			objStat[objID] = 1;
			objMaster[objID].type = 0;
		}
	}

	public bool Create_Object(bool oIsNull, ref short newID, float x, float y, float z, float oriX, float oriY, float oriZ, float ax, float ay, float az, uint dimX, uint dimY, uint dimZ, ref string texture, bool callRegen, float scale, float tScaleX, float tScaleY, float pScale, ref float[] color, short id, byte faces, bool destructable, byte oType, short[] texIDS, byte threadID)
	{
		newID = Register_Object();
		if (newID < 0)
		{
			return false;
		}
		if (objStat[newID] == 0)
		{
			objMaster[newID] = new StructsClass.gameobject();
			StructsClass.Initialize_GameObject(ref objMaster[newID]);
		}
		objStat[newID] = 2;
		objMaster[newID].objRefID = (ushort)newID;
		StructsClass.gameobject gameobject = objMaster[newID];
		gameobject.id = id;
		gameobject.instanceID = -1;
		gameobject.type = oType;
		gameobject.tScaleX = tScaleX;
		gameobject.tScaleY = tScaleY;
		gameobject.pScale = pScale;
		gameobject.x = x;
		gameobject.y = y;
		gameobject.z = z;
		gameobject.bbX1 = x - 0.5f;
		gameobject.bbY1 = y - 0.5f;
		gameobject.bbZ1 = z - 0.5f;
		gameobject.bbX2 = gameobject.bbX1 + (float)dimX;
		gameobject.bbY2 = gameobject.bbY1 + (float)dimY;
		gameobject.bbZ2 = gameobject.bbZ1 + (float)dimZ;
		gameobject.originX = oriX;
		gameobject.originY = oriY;
		gameobject.originZ = oriZ;
		gameobject.dimX = dimX;
		gameobject.dimY = dimY;
		gameobject.dimZ = dimZ;
		gameobject.ptX = dimY * dimZ;
		gameobject.doPhysics = false;
		gameobject.phys1.position.v[0] = gameobject.x;
		gameobject.phys1.position.v[1] = gameobject.y;
		gameobject.phys1.position.v[2] = gameobject.z;
		gameobject.phys1.velocity.v[0] = 0f;
		gameobject.phys1.velocity.v[1] = 0f;
		gameobject.phys1.velocity.v[2] = 0f;
		gameobject.phys1.angularVelocity.v[0] = 0f;
		gameobject.phys1.angularVelocity.v[1] = 0f;
		gameobject.phys1.angularVelocity.v[2] = 0f;
		gameobject.phys1.acceleration.v[0] = 0f;
		gameobject.phys1.acceleration.v[1] = 0f;
		gameobject.phys1.acceleration.v[2] = -32.15223f;
		gameobject.phys1.angularAcceleration.v[0] = 0f;
		gameobject.phys1.angularAcceleration.v[1] = 0f;
		gameobject.phys1.angularAcceleration.v[2] = 0f;
		gameobject.phys1.initialTime = 0.0;
		gameobject.color[0] = color[0];
		gameobject.color[1] = color[1];
		gameobject.color[2] = color[2];
		gameobject.color[3] = color[3];
		gameobject.moving = false;
		gameobject.faces = faces;
		gameobject.destructable = destructable;
		if (ax != 0f || ay != 0f || az != 0f)
		{
			gameobject.isRotated = true;
			gameobject.mv = Matrix.CreateRotationX(ax * ((float)Math.PI / 180f));
			Matrix.CreateRotationY(ay * ((float)Math.PI / 180f), out coMv1);
			gameobject.mv = coMv1 * gameobject.mv;
			Matrix.CreateRotationZ(az * ((float)Math.PI / 180f), out coMv1);
			gameobject.mv = coMv1 * gameobject.mv;
			gameobject.mvT = Matrix.Invert(gameobject.mv);
		}
		else
		{
			gameobject.mv = Matrix.Identity;
			gameobject.mvT = Matrix.Identity;
			gameobject.isRotated = false;
		}
		gameobject.texture = texture;
		if (destructable)
		{
			long num = (gameobject.pcount = gameobject.dimX * gameobject.dimY * gameobject.dimZ);
			gameobject.pt1 = new StructsClass.particle[num];
			sbyte b = 0;
			short num2 = 0;
			while (b < gameobject.dimX)
			{
				for (sbyte b2 = 0; b2 < gameobject.dimY; b2++)
				{
					sbyte b3 = 0;
					while (b3 < gameobject.dimZ && num2 < num)
					{
						gameobject.pt1[num2] = default(StructsClass.particle);
						gameobject.pt1[num2].status = 1;
						gameobject.pt1[num2].destroyOrientation = 0;
						b3++;
						num2++;
					}
				}
				b++;
			}
			gameobject.pList.v1 = new StructsClass.vtex_byte[1];
			gameobject.pList.v1[0] = default(StructsClass.vtex_byte);
			gameobject.pList.v1[0].vx = 0;
			gameobject.pList.v1[0].vy = 0;
			gameobject.pList.v1[0].vz = 0;
			gameobject.pList.numP = 1L;
			gameobject.pList.numUsed = 1L;
			gameobject.pList.bbDirty = true;
		}
		gameobject.f1 = new StructsClass.face[1];
		gameobject.texID = mainC.texturesMain.Find_Texture(gameobject.texture, -1);
		gameobject.texIDs[0] = texIDS[0];
		gameobject.texIDs[1] = texIDS[1];
		gameobject.texIDs[2] = texIDS[2];
		gameobject.texIDs[3] = texIDS[3];
		gameobject.texIDs[4] = texIDS[4];
		gameobject.texIDs[5] = texIDS[5];
		mainC.collisionMain.Update_CollisionBox(newID, threadID);
		Update_Object_BoundingBox(newID);
		if (callRegen)
		{
			Regen_Object(newID, threadID);
		}
		return true;
	}

	public bool Create_Collision_Object(ref short newID, float x, float y, float z, ref Matrix mv, uint dimX, uint dimY, uint dimZ, float rotX, float rotY, float rotZ, short id, byte oType, byte threadID)
	{
		newID = Register_Object();
		if (newID < 0)
		{
			return false;
		}
		if (objStat[newID] == 0)
		{
			objMaster[newID] = new StructsClass.gameobject();
			StructsClass.Initialize_GameObject(ref objMaster[newID]);
		}
		objStat[newID] = 8;
		objMaster[newID].objRefID = (ushort)newID;
		StructsClass.gameobject gameobject = objMaster[newID];
		gameobject.id = id;
		gameobject.instanceID = -1;
		gameobject.type = oType;
		gameobject.dimX = dimX;
		gameobject.dimY = dimY;
		gameobject.dimZ = dimZ;
		gameobject.x = x;
		gameobject.y = y;
		gameobject.z = z;
		gameobject.originX = x;
		gameobject.originY = y;
		gameobject.originZ = z;
		gameobject.rotX = rotX;
		gameobject.rotY = rotY;
		gameobject.rotZ = rotZ;
		if (mv != Matrix.Identity)
		{
			gameobject.isRotated = true;
			gameobject.mv = mv;
			gameobject.mvT = Matrix.Invert(mv);
		}
		else
		{
			gameobject.mv = Matrix.Identity;
			gameobject.mvT = Matrix.Identity;
			gameobject.isRotated = false;
		}
		gameobject.ptX = 0L;
		gameobject.f1 = null;
		gameobject.pcount = 0L;
		gameobject.moving = false;
		gameobject.destructable = false;
		Regen_Object(newID, threadID);
		mainC.collisionMain.Update_CollisionBox(newID, threadID);
		return true;
	}

	public short Copy_Collision_Object(byte dimension, int objID, byte threadID)
	{
		short newID = 0;
		if (objID < 0 || objID >= numObjects || (objStat[objID] & 8) == 0)
		{
			return -1;
		}
		switch (dimension)
		{
		case 1:
			Create_Collision_Object(ref newID, objMaster[objID].x + (float)objMaster[objID].dimX, objMaster[objID].y, objMaster[objID].z, ref objMaster[objID].mv, objMaster[objID].dimX, objMaster[objID].dimY, objMaster[objID].dimZ, objMaster[objID].rotX, objMaster[objID].rotY, objMaster[objID].rotZ, 0, objMaster[objID].type, threadID);
			break;
		case 2:
			Create_Collision_Object(ref newID, objMaster[objID].x, objMaster[objID].y + (float)objMaster[objID].dimY, objMaster[objID].z, ref objMaster[objID].mv, objMaster[objID].dimX, objMaster[objID].dimY, objMaster[objID].dimZ, objMaster[objID].rotX, objMaster[objID].rotY, objMaster[objID].rotZ, 0, objMaster[objID].type, threadID);
			break;
		default:
			Create_Collision_Object(ref newID, objMaster[objID].x, objMaster[objID].y, objMaster[objID].z + (float)objMaster[objID].dimZ, ref objMaster[objID].mv, objMaster[objID].dimX, objMaster[objID].dimY, objMaster[objID].dimZ, objMaster[objID].rotX, objMaster[objID].rotY, objMaster[objID].rotZ, 0, objMaster[objID].type, threadID);
			break;
		}
		return newID;
	}

	public bool Consolidate_Object(long objID, byte threadID)
	{
		bool result = false;
		bool flag = true;
		short newID = 0;
		long num = 0L;
		uint dimX = objMaster[objID].dimX;
		uint dimY = objMaster[objID].dimY;
		uint dimZ = objMaster[objID].dimZ;
		for (long num2 = 0L; num2 < dimX; num2++)
		{
			if (num != 0)
			{
				break;
			}
			for (long num3 = 0L; num3 < dimY; num3++)
			{
				if (num != 0)
				{
					break;
				}
				for (long num4 = 0L; num4 < dimZ; num4++)
				{
					if (num != 0)
					{
						break;
					}
					if (objMaster[objID].pt1[num4 + num3 * dimZ + num2 * dimY * dimZ].status == 0)
					{
						continue;
					}
					objMaster[objID].pt1[num4 + num3 * dimZ + num2 * dimY * dimZ].destroyOrientation = (byte)((objMaster[objID].pt1[num4 + num3 * dimZ + num2 * dimY * dimZ].destroyOrientation & 0x3F) + 64);
					Set_Particle_Ids(objID, num2, num3, num4, dimX, dimY, dimZ);
					do
					{
						long y;
						long z;
						long x = (y = (z = 0L));
						long x2 = dimX - 1;
						long y2 = dimY - 1;
						long z2 = dimZ - 1;
						if (Find_Separated_Particle(objID, dimX, dimY, dimZ) && Find_Particle_Dimensions(objID, ref x2, ref y2, ref z2, ref x, ref y, ref z, dimX, dimY, dimZ) && Create_Object(oIsNull: true, ref newID, objMaster[objID].x + (float)x2, objMaster[objID].y + (float)y2, objMaster[objID].z + (float)z2, objMaster[objID].originX + (float)x2, objMaster[objID].originY + (float)y2, objMaster[objID].originZ + (float)z2, 0f, 0f, 0f, (ushort)(x - x2 + 1), (ushort)(y - y2 + 1), (ushort)(z - z2 + 1), ref objMaster[objID].texture, callRegen: false, 1f, objMaster[objID].tScaleX, objMaster[objID].tScaleY, objMaster[objID].pScale, ref objMaster[objID].color, -1, objMaster[objID].faces, objMaster[objID].destructable, objMaster[objID].type, objMaster[objID].texIDs, threadID))
						{
							Move_Particles(objID, newID, x2, y2, z2, x, y, z, dimX, dimY, dimZ);
							objMaster[newID].doPhysics = true;
							objMaster[newID].phys1.position.v[0] = objMaster[newID].x;
							objMaster[newID].phys1.position.v[1] = objMaster[newID].y;
							objMaster[newID].phys1.position.v[2] = objMaster[newID].z;
							objMaster[newID].phys1.velocity.v[0] = 0f;
							objMaster[newID].phys1.velocity.v[1] = 0f;
							objMaster[newID].phys1.velocity.v[2] = 0f;
							objMaster[newID].phys1.angularVelocity.v[0] = 0f;
							objMaster[newID].phys1.angularVelocity.v[1] = 0f;
							objMaster[newID].phys1.angularVelocity.v[2] = 0f;
							objMaster[newID].phys1.acceleration.v[0] = 0f;
							objMaster[newID].phys1.acceleration.v[1] = 0f;
							objMaster[newID].phys1.acceleration.v[2] = -32.15223f;
							objMaster[newID].phys1.angularAcceleration.v[0] = 0f;
							objMaster[newID].phys1.angularAcceleration.v[1] = 0f;
							objMaster[newID].phys1.angularAcceleration.v[2] = 0f;
							objMaster[newID].phys1.initialTime = 0.0;
							Set_Object_For_Regen(newID);
							result = true;
						}
						else
						{
							flag = false;
						}
					}
					while (flag);
					num = 1L;
				}
			}
		}
		for (long num2 = 0L; num2 < dimX; num2++)
		{
			for (long num3 = 0L; num3 < dimY; num3++)
			{
				for (long num4 = 0L; num4 < dimZ; num4++)
				{
					if ((objMaster[objID].pt1[num4 + num3 * dimZ + num2 * dimY * dimZ].destroyOrientation & 0xC0) != 64 && objMaster[objID].pt1[num4 + num3 * dimZ + num2 * dimY * dimZ].status != 0)
					{
						objMaster[objID].pt1[num4 + num3 * dimZ + num2 * dimY * dimZ].status = 0;
					}
					objMaster[objID].pt1[num4 + num3 * dimZ + num2 * dimY * dimZ].destroyOrientation &= 63;
				}
			}
		}
		return result;
	}

	public bool Find_Separated_Particle(long objID, uint dX, uint dY, uint dZ)
	{
		for (long num = 0L; num < dX; num++)
		{
			for (long num2 = 0L; num2 < dY; num2++)
			{
				for (long num3 = 0L; num3 < dZ; num3++)
				{
					if ((objMaster[objID].pt1[num3 + num2 * dZ + num * dY * dZ].destroyOrientation & 0xC0) == 0 && objMaster[objID].pt1[num3 + num2 * dZ + num * dY * dZ].status != 0)
					{
						objMaster[objID].pt1[num3 + num2 * dZ + num * dY * dZ].destroyOrientation = (byte)((objMaster[objID].pt1[num3 + num2 * dZ + num * dY * dZ].destroyOrientation & 0x3F) + 192);
						Set_Particle_Ids(objID, num, num2, num3, dX, dY, dZ);
						return true;
					}
				}
			}
		}
		return false;
	}

	public bool Find_Particle_Dimensions(long objID, ref long x1, ref long y1, ref long z1, ref long x2, ref long y2, ref long z2, uint dX, uint dY, uint dZ)
	{
		bool result = false;
		for (long num = 0L; num < dX; num++)
		{
			for (long num2 = 0L; num2 < dY; num2++)
			{
				for (long num3 = 0L; num3 < dZ; num3++)
				{
					if ((objMaster[objID].pt1[num3 + num2 * dZ + num * dY * dZ].destroyOrientation & 0xC0) == 192 && objMaster[objID].pt1[num3 + num2 * dZ + num * dY * dZ].status != 0)
					{
						if (num < x1)
						{
							x1 = num;
						}
						if (num > x2)
						{
							x2 = num;
						}
						if (num2 < y1)
						{
							y1 = num2;
						}
						if (num2 > y2)
						{
							y2 = num2;
						}
						if (num3 < z1)
						{
							z1 = num3;
						}
						if (num3 > z2)
						{
							z2 = num3;
						}
						result = true;
					}
				}
			}
		}
		return result;
	}

	public void Move_Particles(long objID, long objID2, long x1, long y1, long z1, long x2, long y2, long z2, long dX, long dY, long dZ)
	{
		long num = 0L;
		for (long num2 = x1; num2 <= x2; num2++)
		{
			for (long num3 = y1; num3 <= y2; num3++)
			{
				long num4 = z1;
				while (num4 <= z2)
				{
					if ((objMaster[objID].pt1[num4 + num3 * dZ + num2 * dY * dZ].destroyOrientation & 0xC0) == 192)
					{
						objMaster[objID2].pt1[num].status = objMaster[objID].pt1[num4 + num3 * dZ + num2 * dY * dZ].status;
						objMaster[objID2].pt1[num].destroyOrientation = (byte)(objMaster[objID2].pt1[num].destroyOrientation & 0xC0);
						objMaster[objID2].pt1[num].destroyOrientation |= objMaster[objID].pt1[num4 + num3 * dZ + num2 * dY * dZ].destroyOrientation;
						objMaster[objID].pt1[num4 + num3 * dZ + num2 * dY * dZ].destroyOrientation = (byte)((objMaster[objID].pt1[num4 + num3 * dZ + num2 * dY * dZ].destroyOrientation & 0x3F) + 64);
						objMaster[objID].pt1[num4 + num3 * dZ + num2 * dY * dZ].status = 0;
						objMaster[objID].pt1[num4 + num3 * dZ + num2 * dY * dZ].destroyOrientation |= 63;
					}
					else
					{
						objMaster[objID2].pt1[num].status = 0;
					}
					num4++;
					num++;
				}
			}
		}
	}

	public void Set_Particle_Ids(long objID, long x1, long y1, long z1, uint dX, uint dY, uint dZ)
	{
		bool flag = true;
		int num = 0;
		if (spidIlistCnt < dX * dY * dZ + 5)
		{
			spidIlistCnt = dX * dY * dZ + 5;
			spidIList = new int[spidIlistCnt, 7];
		}
		int num2 = (int)x1;
		int num3 = (int)y1;
		int num4 = (int)z1;
		spidIList[0, 0] = num2;
		spidIList[0, 1] = num3;
		spidIList[0, 2] = num4;
		spidIList[0, 3] = 1;
		do
		{
			bool flag2 = false;
			num2 = spidIList[num, 0];
			num3 = spidIList[num, 1];
			num4 = spidIList[num, 2];
			if (!flag2 && (flag || spidIList[num + 1, 3] < 2))
			{
				long num5 = (flag ? (num2 + 1) : (spidIList[num + 1, 4] + 1));
				long num6 = num3;
				long num7 = num4;
				while (num5 < dX)
				{
					long num8 = num7 + num6 * dZ + num5 * dY * dZ;
					long num9 = num7 + num6 * dZ + (num5 - 1) * dY * dZ;
					if (objMaster[objID].pt1[num8].status != 0 && (byte)(objMaster[objID].pt1[num8].destroyOrientation & 0xC0) != (byte)(objMaster[objID].pt1[num9].destroyOrientation & 0xC0))
					{
						objMaster[objID].pt1[num8].destroyOrientation = (byte)((byte)(objMaster[objID].pt1[num8].destroyOrientation & 0x3F) + (byte)(objMaster[objID].pt1[num9].destroyOrientation & 0xC0));
						num++;
						spidIList[num, 0] = (int)num5;
						spidIList[num, 1] = (int)num6;
						spidIList[num, 2] = (int)num7;
						spidIList[num, 3] = 1;
						spidIList[num, 4] = num2;
						spidIList[num, 5] = num3;
						spidIList[num, 6] = num4;
						flag2 = true;
						flag = true;
						break;
					}
					num5 = dX;
					num5++;
				}
			}
			if (!flag2 && (flag || spidIList[num + 1, 3] < 3))
			{
				long num5 = (flag ? (num2 - 1) : (spidIList[num + 1, 4] - 1));
				long num6 = num3;
				long num7 = num4;
				while (num5 >= 0)
				{
					long num8 = num7 + num6 * dZ + num5 * dY * dZ;
					long num9 = num7 + num6 * dZ + (num5 + 1) * dY * dZ;
					if (objMaster[objID].pt1[num8].status != 0 && (byte)(objMaster[objID].pt1[num8].destroyOrientation & 0xC0) != (byte)(objMaster[objID].pt1[num9].destroyOrientation & 0xC0))
					{
						objMaster[objID].pt1[num8].destroyOrientation = (byte)((byte)(objMaster[objID].pt1[num8].destroyOrientation & 0x3F) + (byte)(objMaster[objID].pt1[num9].destroyOrientation & 0xC0));
						num++;
						spidIList[num, 0] = (int)num5;
						spidIList[num, 1] = (int)num6;
						spidIList[num, 2] = (int)num7;
						spidIList[num, 3] = 2;
						spidIList[num, 4] = num2;
						spidIList[num, 5] = num3;
						spidIList[num, 6] = num4;
						flag2 = true;
						flag = true;
						break;
					}
					num5 = 0L;
					num5--;
				}
			}
			if (!flag2 && (flag || spidIList[num + 1, 3] < 4))
			{
				long num6 = (flag ? (num3 + 1) : (spidIList[num + 1, 5] + 1));
				long num5 = num2;
				long num7 = num4;
				while (num6 < dY)
				{
					long num8 = num7 + num6 * dZ + num5 * dY * dZ;
					long num9 = num7 + (num6 - 1) * dZ + num5 * dY * dZ;
					if (objMaster[objID].pt1[num8].status != 0 && (byte)(objMaster[objID].pt1[num8].destroyOrientation & 0xC0) != (byte)(objMaster[objID].pt1[num9].destroyOrientation & 0xC0))
					{
						objMaster[objID].pt1[num8].destroyOrientation = (byte)((byte)(objMaster[objID].pt1[num8].destroyOrientation & 0x3F) + (byte)(objMaster[objID].pt1[num9].destroyOrientation & 0xC0));
						num++;
						spidIList[num, 0] = (int)num5;
						spidIList[num, 1] = (int)num6;
						spidIList[num, 2] = (int)num7;
						spidIList[num, 3] = 3;
						spidIList[num, 4] = num2;
						spidIList[num, 5] = num3;
						spidIList[num, 6] = num4;
						flag2 = true;
						flag = true;
						break;
					}
					num6 = dY;
					num6++;
				}
			}
			if (!flag2 && (flag || spidIList[num + 1, 3] < 5))
			{
				long num6 = (flag ? (num3 - 1) : (spidIList[num + 1, 5] - 1));
				long num5 = num2;
				long num7 = num4;
				while (num6 >= 0)
				{
					long num8 = num7 + num6 * dZ + num5 * dY * dZ;
					long num9 = num7 + (num6 + 1) * dZ + num5 * dY * dZ;
					if (objMaster[objID].pt1[num8].status != 0 && (byte)(objMaster[objID].pt1[num8].destroyOrientation & 0xC0) != (byte)(objMaster[objID].pt1[num9].destroyOrientation & 0xC0))
					{
						objMaster[objID].pt1[num8].destroyOrientation = (byte)((byte)(objMaster[objID].pt1[num8].destroyOrientation & 0x3F) + (byte)(objMaster[objID].pt1[num9].destroyOrientation & 0xC0));
						num++;
						spidIList[num, 0] = (int)num5;
						spidIList[num, 1] = (int)num6;
						spidIList[num, 2] = (int)num7;
						spidIList[num, 3] = 4;
						spidIList[num, 4] = num2;
						spidIList[num, 5] = num3;
						spidIList[num, 6] = num4;
						flag2 = true;
						flag = true;
						break;
					}
					num6 = 0L;
					num6--;
				}
			}
			if (!flag2 && (flag || spidIList[num + 1, 3] < 6))
			{
				long num7 = (flag ? (num4 + 1) : (spidIList[num + 1, 6] + 1));
				long num5 = num2;
				long num6 = num3;
				while (num7 < dZ)
				{
					long num8 = num7 + num6 * dZ + num5 * dY * dZ;
					long num9 = num7 - 1 + num6 * dZ + num5 * dY * dZ;
					if (objMaster[objID].pt1[num8].status != 0 && (byte)(objMaster[objID].pt1[num8].destroyOrientation & 0xC0) != (byte)(objMaster[objID].pt1[num9].destroyOrientation & 0xC0))
					{
						objMaster[objID].pt1[num8].destroyOrientation = (byte)((byte)(objMaster[objID].pt1[num8].destroyOrientation & 0x3F) + (byte)(objMaster[objID].pt1[num9].destroyOrientation & 0xC0));
						num++;
						spidIList[num, 0] = (int)num5;
						spidIList[num, 1] = (int)num6;
						spidIList[num, 2] = (int)num7;
						spidIList[num, 3] = 5;
						spidIList[num, 4] = num2;
						spidIList[num, 5] = num3;
						spidIList[num, 6] = num4;
						flag2 = true;
						flag = true;
						break;
					}
					num7 = dZ;
					num7++;
				}
			}
			if (!flag2)
			{
				long num7 = (flag ? (num4 - 1) : (spidIList[num + 1, 6] - 1));
				long num5 = num2;
				long num6 = num3;
				while (num7 >= 0)
				{
					long num8 = num7 + num6 * dZ + num5 * dY * dZ;
					long num9 = num7 + 1 + num6 * dZ + num5 * dY * dZ;
					if (objMaster[objID].pt1[num8].status != 0 && (byte)(objMaster[objID].pt1[num8].destroyOrientation & 0xC0) != (byte)(objMaster[objID].pt1[num9].destroyOrientation & 0xC0))
					{
						objMaster[objID].pt1[num8].destroyOrientation = (byte)((byte)(objMaster[objID].pt1[num8].destroyOrientation & 0x3F) + (byte)(objMaster[objID].pt1[num9].destroyOrientation & 0xC0));
						num++;
						spidIList[num, 0] = (int)num5;
						spidIList[num, 1] = (int)num6;
						spidIList[num, 2] = (int)num7;
						spidIList[num, 3] = 6;
						spidIList[num, 4] = num2;
						spidIList[num, 5] = num3;
						spidIList[num, 6] = num4;
						flag2 = true;
						flag = true;
						break;
					}
					num7 = 0L;
					num7--;
				}
			}
			if (!flag2)
			{
				flag = false;
				num--;
			}
		}
		while (num > -1);
	}

	public void Update_Object_BoundingBox(long oID)
	{
		float num = 0.5f;
		if ((objStat[oID] & 0x18) > 0)
		{
			num = 0f;
		}
		objMaster[oID].pList.b1.v[0] = objMaster[oID].x - num;
		objMaster[oID].pList.b1.v[1] = objMaster[oID].y - num;
		objMaster[oID].pList.b1.v[2] = objMaster[oID].z - num;
		objMaster[oID].pList.b2.v[0] = objMaster[oID].x + (float)objMaster[oID].dimX - num;
		objMaster[oID].pList.b2.v[1] = objMaster[oID].y + (float)objMaster[oID].dimY - num;
		objMaster[oID].pList.b2.v[2] = objMaster[oID].z + (float)objMaster[oID].dimZ - num;
	}

	public void Regen_Object(long oID, byte threadID)
	{
		int num = 0;
		long num2 = 0L;
		long num3 = 0L;
		if (!objMaster[oID].destructable)
		{
			Regen_Object_NonDestructable(oID, threadID);
			return;
		}
		long num4 = objMaster[oID].dimX;
		long num5 = objMaster[oID].dimY;
		long num6 = objMaster[oID].dimZ;
		for (long num7 = 0L; num7 < num4; num7++)
		{
			long num8 = num7 * num5 * num6;
			for (long num9 = 0L; num9 < num5; num9++)
			{
				long num10 = num9 * num6;
				long num11 = 0L;
				long num12 = num8 + num10;
				for (; num11 < num6; num11++)
				{
					objMaster[oID].pt1[num12].side = 0;
					roPt1[num12].width[0] = 0;
					roPt1[num12].width[1] = 0;
					roPt1[num12].width[2] = 0;
					roPt1[num12].width[3] = 0;
					roPt1[num12].width[4] = 0;
					roPt1[num12].width[5] = 0;
					roPt1[num12].height[0] = 0;
					roPt1[num12].height[1] = 0;
					roPt1[num12].height[2] = 0;
					roPt1[num12].height[3] = 0;
					roPt1[num12].height[4] = 0;
					roPt1[num12].height[5] = 0;
					if (objMaster[oID].pt1[num12].status > 0)
					{
						if (num7 > 0 && num9 > 0 && num11 > 0 && num7 < num4 - 1 && num9 < num5 - 1 && num11 < num6 - 1 && objMaster[oID].pt1[num11 + num9 * num6 + (num7 - 1) * num5 * num6].status > 0 && objMaster[oID].pt1[num11 + num9 * num6 + (num7 + 1) * num5 * num6].status > 0 && objMaster[oID].pt1[num11 + (num9 - 1) * num6 + num7 * num5 * num6].status > 0 && objMaster[oID].pt1[num11 + (num9 + 1) * num6 + num7 * num5 * num6].status > 0 && objMaster[oID].pt1[num11 - 1 + num9 * num6 + num7 * num5 * num6].status > 0 && objMaster[oID].pt1[num11 + 1 + num9 * num6 + num7 * num5 * num6].status > 0)
						{
							objMaster[oID].pt1[num12].status = 2;
						}
						else
						{
							objMaster[oID].pt1[num12].status = 1;
							num3++;
						}
					}
					else if (objMaster[oID].pt1[num12].status < 0)
					{
						num3++;
					}
					num12++;
				}
			}
		}
		if (num3 < 1)
		{
			Delete_Object(oID, threadID);
			return;
		}
		for (long num7 = 0L; num7 < num4; num7++)
		{
			long num8 = num7 * num5 * num6;
			for (long num9 = 0L; num9 < num5; num9++)
			{
				long num10 = num9 * num6;
				long num11 = 0L;
				long num12 = num8 + num10;
				for (; num11 < num6; num11++)
				{
					if (objMaster[oID].pt1[num12].status < 1)
					{
						byte b = (byte)(objMaster[oID].pt1[num12].destroyOrientation & 0x3F);
						if (objMaster[oID].pt1[num12].status == 0 && b != 63)
						{
							if (num7 == 0 || objMaster[oID].pt1[num11 + num9 * num6 + (num7 - 1) * num5 * num6].status == 0)
							{
								objMaster[oID].pt1[num12].destroyOrientation = (byte)(objMaster[oID].pt1[num12].destroyOrientation | 1);
							}
							if (num7 == num4 - 1 || objMaster[oID].pt1[num11 + num9 * num6 + (num7 + 1) * num5 * num6].status == 0)
							{
								objMaster[oID].pt1[num12].destroyOrientation = (byte)(objMaster[oID].pt1[num12].destroyOrientation | 2);
							}
							if (num9 == 0 || objMaster[oID].pt1[num11 + (num9 - 1) * num6 + num7 * num5 * num6].status == 0)
							{
								objMaster[oID].pt1[num12].destroyOrientation = (byte)(objMaster[oID].pt1[num12].destroyOrientation | 4);
							}
							if (num9 == num5 - 1 || objMaster[oID].pt1[num11 + (num9 + 1) * num6 + num7 * num5 * num6].status == 0)
							{
								objMaster[oID].pt1[num12].destroyOrientation = (byte)(objMaster[oID].pt1[num12].destroyOrientation | 8);
							}
							if (num11 == 0 || objMaster[oID].pt1[num11 - 1 + num9 * num6 + num7 * num5 * num6].status == 0)
							{
								objMaster[oID].pt1[num12].destroyOrientation = (byte)(objMaster[oID].pt1[num12].destroyOrientation | 0x10);
							}
							if (num11 == num6 - 1 || objMaster[oID].pt1[num11 + 1 + num9 * num6 + num7 * num5 * num9].status == 0)
							{
								objMaster[oID].pt1[num12].destroyOrientation = (byte)(objMaster[oID].pt1[num12].destroyOrientation | 0x20);
							}
						}
						else if (b != 63 && b != 3 && b != 12 && b != 48)
						{
							if (num7 == 0 || objMaster[oID].pt1[num11 + num9 * num6 + (num7 - 1) * num5 * num6].status == 0)
							{
								objMaster[oID].pt1[num12].destroyOrientation = (byte)(objMaster[oID].pt1[num12].destroyOrientation | 1);
							}
							if (num7 == num4 - 1 || objMaster[oID].pt1[num11 + num9 * num6 + (num7 + 1) * num5 * num6].status == 0)
							{
								objMaster[oID].pt1[num12].destroyOrientation = (byte)(objMaster[oID].pt1[num12].destroyOrientation | 2);
							}
							if (num9 == 0 || objMaster[oID].pt1[num11 + (num9 - 1) * num6 + num7 * num5 * num6].status == 0)
							{
								objMaster[oID].pt1[num12].destroyOrientation = (byte)(objMaster[oID].pt1[num12].destroyOrientation | 4);
							}
							if (num9 == num5 - 1 || objMaster[oID].pt1[num11 + (num9 + 1) * num6 + num7 * num5 * num6].status == 0)
							{
								objMaster[oID].pt1[num12].destroyOrientation = (byte)(objMaster[oID].pt1[num12].destroyOrientation | 8);
							}
							if (num11 == 0 || objMaster[oID].pt1[num11 - 1 + num9 * num6 + num7 * num5 * num6].status == 0)
							{
								objMaster[oID].pt1[num12].destroyOrientation = (byte)(objMaster[oID].pt1[num12].destroyOrientation | 0x10);
							}
							if (num11 == num6 - 1 || objMaster[oID].pt1[num11 + 1 + num9 * num6 + num7 * num5 * num9].status == 0)
							{
								objMaster[oID].pt1[num12].destroyOrientation = (byte)(objMaster[oID].pt1[num12].destroyOrientation | 0x20);
							}
						}
					}
					num12++;
				}
			}
		}
		long num13 = 0L;
		objMaster[oID].pList.v1 = null;
		objMaster[oID].pList.numP = num3;
		objMaster[oID].pList.numUsed = num3;
		objMaster[oID].pList.v1 = new StructsClass.vtex_byte[num3];
		objMaster[oID].pList.b1.v[0] = 0f;
		objMaster[oID].pList.b1.v[1] = 0f;
		objMaster[oID].pList.b1.v[2] = 0f;
		objMaster[oID].pList.b2.v[0] = 0f;
		objMaster[oID].pList.b2.v[1] = 0f;
		objMaster[oID].pList.b2.v[2] = 0f;
		for (byte b2 = 0; b2 < num4; b2++)
		{
			long num7 = (int)(b2 * num5 * num6);
			for (byte b3 = 0; b3 < num5; b3++)
			{
				long num9 = (int)(b3 * num6);
				byte b4 = 0;
				long num11 = num9 + num7;
				while (b4 < num6)
				{
					if (objMaster[oID].pt1[num11].status != 0 && objMaster[oID].pt1[num11].status != 2)
					{
						objMaster[oID].pList.v1[num13] = default(StructsClass.vtex_byte);
						objMaster[oID].pList.v1[num13].vx = b2;
						objMaster[oID].pList.v1[num13].vy = b3;
						objMaster[oID].pList.v1[num13].vz = b4;
						if (objMaster[oID].pList.b1.v[0] > (float)(int)b2)
						{
							objMaster[oID].pList.b1.v[0] = (int)b2;
						}
						if (objMaster[oID].pList.b2.v[0] < (float)(int)b2)
						{
							objMaster[oID].pList.b2.v[0] = (int)b2;
						}
						if (objMaster[oID].pList.b1.v[1] > (float)(int)b3)
						{
							objMaster[oID].pList.b1.v[1] = (int)b3;
						}
						if (objMaster[oID].pList.b2.v[1] < (float)(int)b3)
						{
							objMaster[oID].pList.b2.v[1] = (int)b3;
						}
						if (objMaster[oID].pList.b1.v[2] > (float)(int)b4)
						{
							objMaster[oID].pList.b1.v[2] = (int)b4;
						}
						if (objMaster[oID].pList.b2.v[2] < (float)(int)b4)
						{
							objMaster[oID].pList.b2.v[2] = (int)b4;
						}
						num13++;
					}
					num11++;
					b4++;
				}
			}
		}
		objMaster[oID].pList.b1.v[0] += objMaster[oID].x - 0.5f;
		objMaster[oID].pList.b2.v[0] += objMaster[oID].x + 0.5f;
		objMaster[oID].pList.b1.v[1] += objMaster[oID].y - 0.5f;
		objMaster[oID].pList.b2.v[1] += objMaster[oID].y + 0.5f;
		objMaster[oID].pList.b1.v[2] += objMaster[oID].z - 0.5f;
		objMaster[oID].pList.b2.v[2] += objMaster[oID].z + 0.5f;
		for (long num7 = 0L; num7 < num4; num7++)
		{
			long num8 = num7 * num5 * num6;
			for (long num9 = 0L; num9 < num5; num9++)
			{
				long num10 = num9 * num6;
				long num11 = 0L;
				long num12 = num8 + num10;
				for (; num11 < num6; num11++)
				{
					if (objMaster[oID].pt1[num12].status == 1)
					{
						if (num7 == 0 || objMaster[oID].pt1[num11 + num10 + (num7 - 1) * num5 * num6].status < 1)
						{
							objMaster[oID].pt1[num12].side = (byte)(objMaster[oID].pt1[num12].side | 1);
							num2++;
						}
						if (num7 == num4 - 1 || objMaster[oID].pt1[num11 + num10 + (num7 + 1) * num5 * num6].status < 1)
						{
							objMaster[oID].pt1[num12].side = (byte)(objMaster[oID].pt1[num12].side | 2);
							num2++;
						}
						if (num9 == 0 || objMaster[oID].pt1[num11 + (num9 - 1) * num6 + num8].status < 1)
						{
							objMaster[oID].pt1[num12].side = (byte)(objMaster[oID].pt1[num12].side | 0x10);
							num2++;
						}
						if (num9 == num5 - 1 || objMaster[oID].pt1[num11 + (num9 + 1) * num6 + num8].status < 1)
						{
							objMaster[oID].pt1[num12].side = (byte)(objMaster[oID].pt1[num12].side | 0x20);
							num2++;
						}
						if (num11 == 0 || objMaster[oID].pt1[num11 - 1 + num10 + num8].status < 1)
						{
							objMaster[oID].pt1[num12].side = (byte)(objMaster[oID].pt1[num12].side | 8);
							num2++;
						}
						if (num11 == num6 - 1 || objMaster[oID].pt1[num11 + 1 + num10 + num8].status < 1)
						{
							objMaster[oID].pt1[num12].side = (byte)(objMaster[oID].pt1[num12].side | 4);
							num2++;
						}
					}
					num12++;
				}
			}
		}
		long num14 = num5 * num6;
		for (long num11 = 0L; num11 < num6; num11++)
		{
			for (long num7 = 0L; num7 < num4; num7++)
			{
				long num8 = num11 + num7 * num5 * num6;
				for (long num9 = 0L; num9 < num5; num9++)
				{
					long num10 = num8 + num9 * num6;
					if (objMaster[oID].pt1[num10].status != 1)
					{
						continue;
					}
					ushort num15 = 4;
					num = 2;
					while (num15 <= 8)
					{
						if ((objMaster[oID].pt1[num10].side & num15) != 0)
						{
							int i = 0;
							num13 = 0L;
							long num16 = num5;
							int num17 = 0;
							int num18 = 0;
							for (; i + num7 < num4; i++)
							{
								if (num16 <= 0)
								{
									break;
								}
								int j;
								for (j = 0; j + num9 < num5 && j < num16 && (objMaster[oID].pt1[num11 + (num9 + j) * num6 + (num7 + i) * num14].side & num15) != 0; j++)
								{
								}
								if (j == num16 || (i + 1) * j > num13)
								{
									num17 = i + 1;
									num18 = j;
									num13 = num17 * num18;
								}
								num16 = j;
							}
							if (num13 > 0)
							{
								num2 -= num13 - 1;
								num15 = (ushort)(~num15);
								for (i = 0; i < num17; i++)
								{
									for (int j = 0; j < num18; j++)
									{
										objMaster[oID].pt1[num11 + (num9 + j) * num6 + (num7 + i) * num14].side = (byte)(objMaster[oID].pt1[num11 + (num9 + j) * num6 + (num7 + i) * num14].side & num15);
									}
								}
								num15 = (ushort)(~num15);
								roPt1[num10].width[num] = (byte)num17;
								roPt1[num10].height[num] = (byte)num18;
								objMaster[oID].pt1[num10].side = (byte)(objMaster[oID].pt1[num10].side | num15);
							}
						}
						num15 <<= 1;
						num++;
					}
				}
			}
		}
		for (long num7 = 0L; num7 < num4; num7++)
		{
			long num8 = num7 * num14;
			for (long num9 = 0L; num9 < num5; num9++)
			{
				long num10 = num9 * num6;
				long num11 = 0L;
				long num12 = num8 + num10;
				for (; num11 < num6; num11++)
				{
					if (objMaster[oID].pt1[num12].status == 1)
					{
						ushort num15 = 1;
						num = 0;
						while (num15 <= 2)
						{
							if ((objMaster[oID].pt1[num12].side & num15) != 0)
							{
								int i = 0;
								num13 = 0L;
								long num16 = num6;
								int num17 = 0;
								int num18 = 0;
								for (; i + num9 < num5; i++)
								{
									if (num16 <= 0)
									{
										break;
									}
									int j;
									for (j = 0; j + num11 < num6 && j < num16 && (objMaster[oID].pt1[num11 + j + (num9 + i) * num6 + num8].side & num15) != 0; j++)
									{
									}
									if (j == num16 || (i + 1) * j > num13)
									{
										num17 = i + 1;
										num18 = j;
										num13 = num17 * num18;
									}
									num16 = j;
								}
								if (num13 > 0)
								{
									num2 -= num13 - 1;
									num15 = (ushort)(~num15);
									for (i = 0; i < num17; i++)
									{
										for (int j = 0; j < num18; j++)
										{
											objMaster[oID].pt1[num11 + j + (num9 + i) * num6 + num8].side = (byte)(objMaster[oID].pt1[num11 + j + (num9 + i) * num6 + num8].side & num15);
										}
									}
									num15 = (ushort)(~num15);
									roPt1[num12].width[num] = (byte)num17;
									roPt1[num12].height[num] = (byte)num18;
									objMaster[oID].pt1[num12].side = (byte)(objMaster[oID].pt1[num12].side | num15);
								}
							}
							num15 <<= 1;
							num++;
						}
					}
					num12++;
				}
			}
		}
		for (long num9 = 0L; num9 < num5; num9++)
		{
			long num10 = num9 * num6;
			for (long num7 = 0L; num7 < num4; num7++)
			{
				long num8 = num7 * num14;
				long num11 = 0L;
				long num12 = num8 + num10;
				for (; num11 < num6; num11++)
				{
					if (objMaster[oID].pt1[num12].status == 1)
					{
						ushort num15 = 16;
						num = 4;
						while (num15 <= 32)
						{
							if ((objMaster[oID].pt1[num12].side & num15) != 0)
							{
								int i = 0;
								num13 = 0L;
								long num16 = num6;
								int num17 = 0;
								int num18 = 0;
								for (; i + num7 < num4; i++)
								{
									if (num16 <= 0)
									{
										break;
									}
									int j;
									for (j = 0; j + num11 < num6 && j < num16 && (objMaster[oID].pt1[num11 + j + num10 + (num7 + i) * num14].side & num15) != 0; j++)
									{
									}
									if (j == num16 || (i + 1) * j > num13)
									{
										num17 = i + 1;
										num18 = j;
										num13 = num17 * num18;
									}
									num16 = j;
								}
								if (num13 > 0)
								{
									num2 -= num13 - 1;
									num15 = (ushort)(~num15);
									for (i = 0; i < num17; i++)
									{
										for (int j = 0; j < num18; j++)
										{
											objMaster[oID].pt1[num11 + j + num10 + (num7 + i) * num14].side = (byte)(objMaster[oID].pt1[num11 + j + num10 + (num7 + i) * num14].side & num15);
										}
									}
									num15 = (ushort)(~num15);
									roPt1[num12].width[num] = (byte)num17;
									roPt1[num12].height[num] = (byte)num18;
									objMaster[oID].pt1[num12].side = (byte)(objMaster[oID].pt1[num12].side | num15);
								}
							}
							num15 <<= 1;
							num++;
						}
					}
					num12++;
				}
			}
		}
		objMaster[oID].f1 = null;
		objMaster[oID].f1 = new StructsClass.face[num2];
		num2 = 0L;
		for (long num7 = 0L; num7 < num4; num7++)
		{
			long num8 = num7 * num14;
			for (long num9 = 0L; num9 < num5; num9++)
			{
				long num10 = num9 * num6;
				long num11 = 0L;
				long num12 = num8 + num10;
				for (; num11 < num6; num11++)
				{
					if (objMaster[oID].pt1[num12].status == 1)
					{
						if ((objMaster[oID].pt1[num12].side & 4) != 0)
						{
							objMaster[oID].f1[num2].height = roPt1[num12].height[2];
							objMaster[oID].f1[num2].width = roPt1[num12].width[2];
							objMaster[oID].f1[num2].u = objMaster[oID].tScaleX / 100f * (float)(int)objMaster[oID].f1[num2].width;
							objMaster[oID].f1[num2].v = objMaster[oID].tScaleY / 100f * (float)(int)objMaster[oID].f1[num2].height;
							objMaster[oID].f1[num2].texOffsetX = objMaster[oID].tScaleX / 100f * (objMaster[oID].originX + (float)num7) - (float)(long)(objMaster[oID].tScaleX / 100f * (objMaster[oID].originX + (float)num7));
							objMaster[oID].f1[num2].texOffsetY = objMaster[oID].tScaleY / 100f * (objMaster[oID].originY + (float)num9) - (float)(long)(objMaster[oID].tScaleY / 100f * (objMaster[oID].originY + (float)num9));
							objMaster[oID].f1[num2].v1 = new StructsClass.vtex[4];
							objMaster[oID].f1[num2].v1[0] = new StructsClass.vtex();
							objMaster[oID].f1[num2].v1[1] = new StructsClass.vtex();
							objMaster[oID].f1[num2].v1[2] = new StructsClass.vtex();
							objMaster[oID].f1[num2].v1[3] = new StructsClass.vtex();
							objMaster[oID].f1[num2].v1[0].v[0] = num7;
							objMaster[oID].f1[num2].v1[0].v[1] = num9;
							objMaster[oID].f1[num2].v1[0].v[2] = num11 + 1;
							objMaster[oID].f1[num2].v1[1].v[0] = num7 + (int)roPt1[num12].width[2];
							objMaster[oID].f1[num2].v1[1].v[1] = num9;
							objMaster[oID].f1[num2].v1[1].v[2] = num11 + 1;
							objMaster[oID].f1[num2].v1[2].v[0] = num7 + (int)roPt1[num12].width[2];
							objMaster[oID].f1[num2].v1[2].v[1] = num9 + (int)roPt1[num12].height[2];
							objMaster[oID].f1[num2].v1[2].v[2] = num11 + 1;
							objMaster[oID].f1[num2].v1[3].v[0] = num7;
							objMaster[oID].f1[num2].v1[3].v[1] = num9 + (int)roPt1[num12].height[2];
							objMaster[oID].f1[num2].v1[3].v[2] = num11 + 1;
							Calculate_ParticleFace_Tangents(oID, num2);
							objMaster[oID].f1[num2].n1 = new StructsClass.vnorm[4];
							objMaster[oID].f1[num2].n1[0] = new StructsClass.vnorm();
							objMaster[oID].f1[num2].n1[1] = new StructsClass.vnorm();
							objMaster[oID].f1[num2].n1[2] = new StructsClass.vnorm();
							objMaster[oID].f1[num2].n1[3] = new StructsClass.vnorm();
							crossProductandNorm(oID, num2, 0);
							crossProductandNorm(oID, num2, 1);
							crossProductandNorm(oID, num2, 2);
							crossProductandNorm(oID, num2, 3);
							num2++;
						}
						if ((objMaster[oID].pt1[num12].side & 8) != 0)
						{
							objMaster[oID].f1[num2].height = roPt1[num12].height[3];
							objMaster[oID].f1[num2].width = roPt1[num12].width[3];
							objMaster[oID].f1[num2].u = objMaster[oID].tScaleX / 100f * (float)(int)objMaster[oID].f1[num2].width;
							objMaster[oID].f1[num2].v = objMaster[oID].tScaleY / 100f * (float)(int)objMaster[oID].f1[num2].height;
							objMaster[oID].f1[num2].texOffsetX = (float)(long)(objMaster[oID].tScaleX / 100f * (objMaster[oID].originX + (float)num7 + (float)(int)objMaster[oID].f1[num2].width)) - objMaster[oID].tScaleX / 100f * (objMaster[oID].originX + (float)num7 + (float)(int)objMaster[oID].f1[num2].width);
							objMaster[oID].f1[num2].texOffsetY = objMaster[oID].tScaleY / 100f * (objMaster[oID].originY + (float)num9) - (float)(long)(objMaster[oID].tScaleY / 100f * (objMaster[oID].originY + (float)num9));
							objMaster[oID].f1[num2].v1 = new StructsClass.vtex[4];
							objMaster[oID].f1[num2].v1[0] = new StructsClass.vtex();
							objMaster[oID].f1[num2].v1[1] = new StructsClass.vtex();
							objMaster[oID].f1[num2].v1[2] = new StructsClass.vtex();
							objMaster[oID].f1[num2].v1[3] = new StructsClass.vtex();
							objMaster[oID].f1[num2].v1[0].v[0] = num7 + (int)roPt1[num12].width[3];
							objMaster[oID].f1[num2].v1[0].v[1] = num9;
							objMaster[oID].f1[num2].v1[0].v[2] = num11;
							objMaster[oID].f1[num2].v1[1].v[0] = num7;
							objMaster[oID].f1[num2].v1[1].v[1] = num9;
							objMaster[oID].f1[num2].v1[1].v[2] = num11;
							objMaster[oID].f1[num2].v1[2].v[0] = num7;
							objMaster[oID].f1[num2].v1[2].v[1] = num9 + (int)roPt1[num12].height[3];
							objMaster[oID].f1[num2].v1[2].v[2] = num11;
							objMaster[oID].f1[num2].v1[3].v[0] = num7 + (int)roPt1[num12].width[3];
							objMaster[oID].f1[num2].v1[3].v[1] = num9 + (int)roPt1[num12].height[3];
							objMaster[oID].f1[num2].v1[3].v[2] = num11;
							objMaster[oID].f1[num2].n1 = new StructsClass.vnorm[4];
							objMaster[oID].f1[num2].n1[0] = new StructsClass.vnorm();
							objMaster[oID].f1[num2].n1[1] = new StructsClass.vnorm();
							objMaster[oID].f1[num2].n1[2] = new StructsClass.vnorm();
							objMaster[oID].f1[num2].n1[3] = new StructsClass.vnorm();
							Calculate_ParticleFace_Tangents(oID, num2);
							crossProductandNorm(oID, num2, 0);
							crossProductandNorm(oID, num2, 1);
							crossProductandNorm(oID, num2, 2);
							crossProductandNorm(oID, num2, 3);
							num2++;
						}
						if ((objMaster[oID].pt1[num12].side & 0x10) != 0)
						{
							objMaster[oID].f1[num2].height = roPt1[num12].height[4];
							objMaster[oID].f1[num2].width = roPt1[num12].width[4];
							objMaster[oID].f1[num2].u = objMaster[oID].tScaleX / 100f * (float)(int)objMaster[oID].f1[num2].width;
							objMaster[oID].f1[num2].v = objMaster[oID].tScaleY / 100f * (float)(int)objMaster[oID].f1[num2].height;
							objMaster[oID].f1[num2].texOffsetX = objMaster[oID].tScaleX / 100f * (objMaster[oID].originX + (float)num7) - (float)(long)(objMaster[oID].tScaleX / 100f * (objMaster[oID].originX + (float)num7));
							objMaster[oID].f1[num2].texOffsetY = objMaster[oID].tScaleY / 100f * (objMaster[oID].originZ + (float)num11) - (float)(long)(objMaster[oID].tScaleY / 100f * (objMaster[oID].originZ + (float)num11));
							objMaster[oID].f1[num2].v1 = new StructsClass.vtex[4];
							objMaster[oID].f1[num2].v1[0] = new StructsClass.vtex();
							objMaster[oID].f1[num2].v1[1] = new StructsClass.vtex();
							objMaster[oID].f1[num2].v1[2] = new StructsClass.vtex();
							objMaster[oID].f1[num2].v1[3] = new StructsClass.vtex();
							objMaster[oID].f1[num2].v1[0].v[0] = num7;
							objMaster[oID].f1[num2].v1[0].v[1] = num9;
							objMaster[oID].f1[num2].v1[0].v[2] = num11;
							objMaster[oID].f1[num2].v1[1].v[0] = num7 + (int)roPt1[num12].width[4];
							objMaster[oID].f1[num2].v1[1].v[1] = num9;
							objMaster[oID].f1[num2].v1[1].v[2] = num11;
							objMaster[oID].f1[num2].v1[2].v[0] = num7 + (int)roPt1[num12].width[4];
							objMaster[oID].f1[num2].v1[2].v[1] = num9;
							objMaster[oID].f1[num2].v1[2].v[2] = num11 + (int)roPt1[num12].height[4];
							objMaster[oID].f1[num2].v1[3].v[0] = num7;
							objMaster[oID].f1[num2].v1[3].v[1] = num9;
							objMaster[oID].f1[num2].v1[3].v[2] = num11 + (int)roPt1[num12].height[4];
							objMaster[oID].f1[num2].n1 = new StructsClass.vnorm[4];
							objMaster[oID].f1[num2].n1[0] = new StructsClass.vnorm();
							objMaster[oID].f1[num2].n1[1] = new StructsClass.vnorm();
							objMaster[oID].f1[num2].n1[2] = new StructsClass.vnorm();
							objMaster[oID].f1[num2].n1[3] = new StructsClass.vnorm();
							Calculate_ParticleFace_Tangents(oID, num2);
							crossProductandNorm(oID, num2, 0);
							crossProductandNorm(oID, num2, 1);
							crossProductandNorm(oID, num2, 2);
							crossProductandNorm(oID, num2, 3);
							num2++;
						}
						if ((objMaster[oID].pt1[num12].side & 0x20) != 0)
						{
							objMaster[oID].f1[num2].height = roPt1[num12].height[5];
							objMaster[oID].f1[num2].width = roPt1[num12].width[5];
							objMaster[oID].f1[num2].u = objMaster[oID].tScaleX / 100f * (float)(int)objMaster[oID].f1[num2].width;
							objMaster[oID].f1[num2].v = objMaster[oID].tScaleY / 100f * (float)(int)objMaster[oID].f1[num2].height;
							objMaster[oID].f1[num2].texOffsetX = (float)(long)(objMaster[oID].tScaleX / 100f * (objMaster[oID].originX + (float)num7 + (float)(int)objMaster[oID].f1[num2].width)) - objMaster[oID].tScaleX / 100f * (objMaster[oID].originX + (float)num7 + (float)(int)objMaster[oID].f1[num2].width);
							objMaster[oID].f1[num2].texOffsetY = objMaster[oID].tScaleY / 100f * (objMaster[oID].originZ + (float)num11) - (float)(long)(objMaster[oID].tScaleY / 100f * (objMaster[oID].originZ + (float)num11));
							objMaster[oID].f1[num2].v1 = new StructsClass.vtex[4];
							objMaster[oID].f1[num2].v1[0] = new StructsClass.vtex();
							objMaster[oID].f1[num2].v1[1] = new StructsClass.vtex();
							objMaster[oID].f1[num2].v1[2] = new StructsClass.vtex();
							objMaster[oID].f1[num2].v1[3] = new StructsClass.vtex();
							objMaster[oID].f1[num2].v1[0].v[0] = num7 + (int)roPt1[num12].width[5];
							objMaster[oID].f1[num2].v1[0].v[1] = num9 + 1;
							objMaster[oID].f1[num2].v1[0].v[2] = num11;
							objMaster[oID].f1[num2].v1[1].v[0] = num7;
							objMaster[oID].f1[num2].v1[1].v[1] = num9 + 1;
							objMaster[oID].f1[num2].v1[1].v[2] = num11;
							objMaster[oID].f1[num2].v1[2].v[0] = num7;
							objMaster[oID].f1[num2].v1[2].v[1] = num9 + 1;
							objMaster[oID].f1[num2].v1[2].v[2] = num11 + (int)roPt1[num12].height[5];
							objMaster[oID].f1[num2].v1[3].v[0] = num7 + (int)roPt1[num12].width[5];
							objMaster[oID].f1[num2].v1[3].v[1] = num9 + 1;
							objMaster[oID].f1[num2].v1[3].v[2] = num11 + (int)roPt1[num12].height[5];
							objMaster[oID].f1[num2].n1 = new StructsClass.vnorm[4];
							objMaster[oID].f1[num2].n1[0] = new StructsClass.vnorm();
							objMaster[oID].f1[num2].n1[1] = new StructsClass.vnorm();
							objMaster[oID].f1[num2].n1[2] = new StructsClass.vnorm();
							objMaster[oID].f1[num2].n1[3] = new StructsClass.vnorm();
							Calculate_ParticleFace_Tangents(oID, num2);
							crossProductandNorm(oID, num2, 0);
							crossProductandNorm(oID, num2, 1);
							crossProductandNorm(oID, num2, 2);
							crossProductandNorm(oID, num2, 3);
							num2++;
						}
						if ((objMaster[oID].pt1[num12].side & 2) != 0)
						{
							objMaster[oID].f1[num2].height = roPt1[num12].height[1];
							objMaster[oID].f1[num2].width = roPt1[num12].width[1];
							objMaster[oID].f1[num2].u = objMaster[oID].tScaleX / 100f * (float)(int)objMaster[oID].f1[num2].width;
							objMaster[oID].f1[num2].v = objMaster[oID].tScaleY / 100f * (float)(int)objMaster[oID].f1[num2].height;
							objMaster[oID].f1[num2].texOffsetX = objMaster[oID].tScaleX / 100f * (objMaster[oID].originY + (float)num9) - (float)(long)(objMaster[oID].tScaleX / 100f * (objMaster[oID].originY + (float)num9));
							objMaster[oID].f1[num2].texOffsetY = objMaster[oID].tScaleY / 100f * (objMaster[oID].originZ + (float)num11) - (float)(long)(objMaster[oID].tScaleY / 100f * (objMaster[oID].originZ + (float)num11));
							objMaster[oID].f1[num2].v1 = new StructsClass.vtex[4];
							objMaster[oID].f1[num2].v1[0] = new StructsClass.vtex();
							objMaster[oID].f1[num2].v1[1] = new StructsClass.vtex();
							objMaster[oID].f1[num2].v1[2] = new StructsClass.vtex();
							objMaster[oID].f1[num2].v1[3] = new StructsClass.vtex();
							objMaster[oID].f1[num2].v1[0].v[0] = num7 + 1;
							objMaster[oID].f1[num2].v1[0].v[1] = num9;
							objMaster[oID].f1[num2].v1[0].v[2] = num11;
							objMaster[oID].f1[num2].v1[1].v[0] = num7 + 1;
							objMaster[oID].f1[num2].v1[1].v[1] = num9 + (int)roPt1[num12].width[1];
							objMaster[oID].f1[num2].v1[1].v[2] = num11;
							objMaster[oID].f1[num2].v1[2].v[0] = num7 + 1;
							objMaster[oID].f1[num2].v1[2].v[1] = num9 + (int)roPt1[num12].width[1];
							objMaster[oID].f1[num2].v1[2].v[2] = num11 + (int)roPt1[num12].height[1];
							objMaster[oID].f1[num2].v1[3].v[0] = num7 + 1;
							objMaster[oID].f1[num2].v1[3].v[1] = num9;
							objMaster[oID].f1[num2].v1[3].v[2] = num11 + (int)roPt1[num12].height[1];
							objMaster[oID].f1[num2].n1 = new StructsClass.vnorm[4];
							objMaster[oID].f1[num2].n1[0] = new StructsClass.vnorm();
							objMaster[oID].f1[num2].n1[1] = new StructsClass.vnorm();
							objMaster[oID].f1[num2].n1[2] = new StructsClass.vnorm();
							objMaster[oID].f1[num2].n1[3] = new StructsClass.vnorm();
							Calculate_ParticleFace_Tangents(oID, num2);
							crossProductandNorm(oID, num2, 0);
							crossProductandNorm(oID, num2, 1);
							crossProductandNorm(oID, num2, 2);
							crossProductandNorm(oID, num2, 3);
							num2++;
						}
						if ((objMaster[oID].pt1[num12].side & 1) != 0)
						{
							objMaster[oID].f1[num2].height = roPt1[num12].height[0];
							objMaster[oID].f1[num2].width = roPt1[num12].width[0];
							objMaster[oID].f1[num2].u = objMaster[oID].tScaleX / 100f * (float)(int)objMaster[oID].f1[num2].width;
							objMaster[oID].f1[num2].v = objMaster[oID].tScaleY / 100f * (float)(int)objMaster[oID].f1[num2].height;
							objMaster[oID].f1[num2].texOffsetX = (float)(long)(objMaster[oID].tScaleX / 100f * (objMaster[oID].originY + (float)num9 + (float)(int)objMaster[oID].f1[num2].width)) - objMaster[oID].tScaleX / 100f * (objMaster[oID].originY + (float)num9 + (float)(int)objMaster[oID].f1[num2].width);
							objMaster[oID].f1[num2].texOffsetY = (objMaster[oID].originZ + (float)num11) * objMaster[oID].tScaleY / 100f - (float)(long)((objMaster[oID].originZ + (float)num11) * objMaster[oID].tScaleY / 100f);
							objMaster[oID].f1[num2].v1 = new StructsClass.vtex[4];
							objMaster[oID].f1[num2].v1[0] = new StructsClass.vtex();
							objMaster[oID].f1[num2].v1[1] = new StructsClass.vtex();
							objMaster[oID].f1[num2].v1[2] = new StructsClass.vtex();
							objMaster[oID].f1[num2].v1[3] = new StructsClass.vtex();
							objMaster[oID].f1[num2].v1[0].v[0] = num7;
							objMaster[oID].f1[num2].v1[0].v[1] = num9 + (int)roPt1[num12].width[0];
							objMaster[oID].f1[num2].v1[0].v[2] = num11;
							objMaster[oID].f1[num2].v1[1].v[0] = num7;
							objMaster[oID].f1[num2].v1[1].v[1] = num9;
							objMaster[oID].f1[num2].v1[1].v[2] = num11;
							objMaster[oID].f1[num2].v1[2].v[0] = num7;
							objMaster[oID].f1[num2].v1[2].v[1] = num9;
							objMaster[oID].f1[num2].v1[2].v[2] = num11 + (int)roPt1[num12].height[0];
							objMaster[oID].f1[num2].v1[3].v[0] = num7;
							objMaster[oID].f1[num2].v1[3].v[1] = num9 + (int)roPt1[num12].width[0];
							objMaster[oID].f1[num2].v1[3].v[2] = num11 + (int)roPt1[num12].height[0];
							objMaster[oID].f1[num2].n1 = new StructsClass.vnorm[4];
							objMaster[oID].f1[num2].n1[0] = new StructsClass.vnorm();
							objMaster[oID].f1[num2].n1[1] = new StructsClass.vnorm();
							objMaster[oID].f1[num2].n1[2] = new StructsClass.vnorm();
							objMaster[oID].f1[num2].n1[3] = new StructsClass.vnorm();
							Calculate_ParticleFace_Tangents(oID, num2);
							crossProductandNorm(oID, num2, 0);
							crossProductandNorm(oID, num2, 1);
							crossProductandNorm(oID, num2, 2);
							crossProductandNorm(oID, num2, 3);
							num2++;
						}
					}
					num12++;
				}
			}
		}
		objMaster[oID].fcount = num2;
	}

	public void Regen_Object_NonDestructable(long oID, byte threadID)
	{
		long num = 0L;
		float num2 = 0.5f;
		if ((objStat[oID] & 0x18) > 0)
		{
			num2 = 0f;
		}
		objMaster[oID].pList.b1.v[0] = objMaster[oID].x - num2;
		objMaster[oID].pList.b1.v[1] = objMaster[oID].y - num2;
		objMaster[oID].pList.b1.v[2] = objMaster[oID].z - num2;
		objMaster[oID].pList.b2.v[0] = objMaster[oID].x + (float)objMaster[oID].dimX - num2;
		objMaster[oID].pList.b2.v[1] = objMaster[oID].y + (float)objMaster[oID].dimY - num2;
		objMaster[oID].pList.b2.v[2] = objMaster[oID].z + (float)objMaster[oID].dimZ - num2;
		objMaster[oID].pList.v1 = null;
		objMaster[oID].pt1 = null;
		objMaster[oID].f1 = null;
		objMaster[oID].f1 = new StructsClass.face[6];
		if ((objStat[oID] & 6) != 0)
		{
			num = 0L;
			uint dimX = objMaster[oID].dimX;
			uint dimY = objMaster[oID].dimY;
			uint dimZ = objMaster[oID].dimZ;
			if ((objMaster[oID].faces & 1) != 0)
			{
				objMaster[oID].f1[num].faceID = 4;
				objMaster[oID].f1[num].u = objMaster[oID].tScaleX / 100f * (float)dimY;
				objMaster[oID].f1[num].v = objMaster[oID].tScaleY / 100f * (float)dimZ;
				objMaster[oID].f1[num].texOffsetX = objMaster[oID].tScaleX / 100f * objMaster[oID].originY - (float)(long)(objMaster[oID].tScaleX / 100f * objMaster[oID].originY);
				objMaster[oID].f1[num].texOffsetY = objMaster[oID].tScaleY / 100f * objMaster[oID].originZ - (float)(long)(objMaster[oID].tScaleY / 100f * objMaster[oID].originZ);
				objMaster[oID].f1[num].v1 = new StructsClass.vtex[4];
				objMaster[oID].f1[num].v1[0] = new StructsClass.vtex();
				objMaster[oID].f1[num].v1[1] = new StructsClass.vtex();
				objMaster[oID].f1[num].v1[2] = new StructsClass.vtex();
				objMaster[oID].f1[num].v1[3] = new StructsClass.vtex();
				objMaster[oID].f1[num].v1[0].v[0] = dimX;
				objMaster[oID].f1[num].v1[0].v[1] = 0f;
				objMaster[oID].f1[num].v1[0].v[2] = 0f;
				objMaster[oID].f1[num].v1[1].v[0] = dimX;
				objMaster[oID].f1[num].v1[1].v[1] = dimY;
				objMaster[oID].f1[num].v1[1].v[2] = 0f;
				objMaster[oID].f1[num].v1[2].v[0] = dimX;
				objMaster[oID].f1[num].v1[2].v[1] = dimY;
				objMaster[oID].f1[num].v1[2].v[2] = dimZ;
				objMaster[oID].f1[num].v1[3].v[0] = dimX;
				objMaster[oID].f1[num].v1[3].v[1] = 0f;
				objMaster[oID].f1[num].v1[3].v[2] = dimZ;
				objMaster[oID].f1[num].n1 = new StructsClass.vnorm[4];
				objMaster[oID].f1[num].n1[0] = new StructsClass.vnorm();
				objMaster[oID].f1[num].n1[1] = new StructsClass.vnorm();
				objMaster[oID].f1[num].n1[2] = new StructsClass.vnorm();
				objMaster[oID].f1[num].n1[3] = new StructsClass.vnorm();
				Calculate_ParticleFace_Tangents(oID, num);
				crossProductandNorm(oID, num, 0);
				crossProductandNorm(oID, num, 1);
				crossProductandNorm(oID, num, 2);
				crossProductandNorm(oID, num, 3);
				num++;
			}
			if ((objMaster[oID].faces & 2) != 0)
			{
				objMaster[oID].f1[num].faceID = 5;
				objMaster[oID].f1[num].u = objMaster[oID].tScaleX / 100f * (float)dimY;
				objMaster[oID].f1[num].v = objMaster[oID].tScaleY / 100f * (float)dimZ;
				objMaster[oID].f1[num].texOffsetX = (float)(long)(objMaster[oID].tScaleX / 100f * (objMaster[oID].originY + (float)dimY)) - objMaster[oID].tScaleX / 100f * (objMaster[oID].originY + (float)dimY);
				objMaster[oID].f1[num].texOffsetY = objMaster[oID].originZ * objMaster[oID].tScaleY / 100f - (float)(long)(objMaster[oID].originZ * objMaster[oID].tScaleY / 100f);
				objMaster[oID].f1[num].v1 = new StructsClass.vtex[4];
				objMaster[oID].f1[num].v1[0] = new StructsClass.vtex();
				objMaster[oID].f1[num].v1[1] = new StructsClass.vtex();
				objMaster[oID].f1[num].v1[2] = new StructsClass.vtex();
				objMaster[oID].f1[num].v1[3] = new StructsClass.vtex();
				objMaster[oID].f1[num].v1[0].v[0] = 0f;
				objMaster[oID].f1[num].v1[0].v[1] = dimY;
				objMaster[oID].f1[num].v1[0].v[2] = 0f;
				objMaster[oID].f1[num].v1[1].v[0] = 0f;
				objMaster[oID].f1[num].v1[1].v[1] = 0f;
				objMaster[oID].f1[num].v1[1].v[2] = 0f;
				objMaster[oID].f1[num].v1[2].v[0] = 0f;
				objMaster[oID].f1[num].v1[2].v[1] = 0f;
				objMaster[oID].f1[num].v1[2].v[2] = dimZ;
				objMaster[oID].f1[num].v1[3].v[0] = 0f;
				objMaster[oID].f1[num].v1[3].v[1] = dimY;
				objMaster[oID].f1[num].v1[3].v[2] = dimZ;
				objMaster[oID].f1[num].n1 = new StructsClass.vnorm[4];
				objMaster[oID].f1[num].n1[0] = new StructsClass.vnorm();
				objMaster[oID].f1[num].n1[1] = new StructsClass.vnorm();
				objMaster[oID].f1[num].n1[2] = new StructsClass.vnorm();
				objMaster[oID].f1[num].n1[3] = new StructsClass.vnorm();
				Calculate_ParticleFace_Tangents(oID, num);
				crossProductandNorm(oID, num, 0);
				crossProductandNorm(oID, num, 1);
				crossProductandNorm(oID, num, 2);
				crossProductandNorm(oID, num, 3);
				num++;
			}
			if ((objMaster[oID].faces & 5) != 0)
			{
				objMaster[oID].f1[num].faceID = 0;
				objMaster[oID].f1[num].u = objMaster[oID].tScaleX / 100f * (float)dimX;
				objMaster[oID].f1[num].v = objMaster[oID].tScaleY / 100f * (float)dimY;
				objMaster[oID].f1[num].texOffsetX = objMaster[oID].tScaleX / 100f * objMaster[oID].originX - (float)(long)(objMaster[oID].tScaleX / 100f * objMaster[oID].originX);
				objMaster[oID].f1[num].texOffsetY = objMaster[oID].tScaleY / 100f * objMaster[oID].originY - (float)(long)(objMaster[oID].tScaleY / 100f * objMaster[oID].originY);
				objMaster[oID].f1[num].v1 = new StructsClass.vtex[4];
				objMaster[oID].f1[num].v1[0] = new StructsClass.vtex();
				objMaster[oID].f1[num].v1[1] = new StructsClass.vtex();
				objMaster[oID].f1[num].v1[2] = new StructsClass.vtex();
				objMaster[oID].f1[num].v1[3] = new StructsClass.vtex();
				objMaster[oID].f1[num].v1[0].v[0] = 0f;
				objMaster[oID].f1[num].v1[0].v[1] = 0f;
				objMaster[oID].f1[num].v1[0].v[2] = dimZ;
				objMaster[oID].f1[num].v1[1].v[0] = dimX;
				objMaster[oID].f1[num].v1[1].v[1] = 0f;
				objMaster[oID].f1[num].v1[1].v[2] = dimZ;
				objMaster[oID].f1[num].v1[2].v[0] = dimX;
				objMaster[oID].f1[num].v1[2].v[1] = dimY;
				objMaster[oID].f1[num].v1[2].v[2] = dimZ;
				objMaster[oID].f1[num].v1[3].v[0] = 0f;
				objMaster[oID].f1[num].v1[3].v[1] = dimY;
				objMaster[oID].f1[num].v1[3].v[2] = dimZ;
				Calculate_ParticleFace_Tangents(oID, num);
				objMaster[oID].f1[num].n1 = new StructsClass.vnorm[4];
				objMaster[oID].f1[num].n1[0] = new StructsClass.vnorm();
				objMaster[oID].f1[num].n1[1] = new StructsClass.vnorm();
				objMaster[oID].f1[num].n1[2] = new StructsClass.vnorm();
				objMaster[oID].f1[num].n1[3] = new StructsClass.vnorm();
				crossProductandNorm(oID, num, 0);
				crossProductandNorm(oID, num, 1);
				crossProductandNorm(oID, num, 2);
				crossProductandNorm(oID, num, 3);
				num++;
			}
			if ((objMaster[oID].faces & 6) != 0)
			{
				objMaster[oID].f1[num].faceID = 1;
				objMaster[oID].f1[num].u = objMaster[oID].tScaleX / 100f * (float)dimX;
				objMaster[oID].f1[num].v = objMaster[oID].tScaleY / 100f * (float)dimY;
				objMaster[oID].f1[num].texOffsetX = (float)(long)(objMaster[oID].tScaleX / 100f * (objMaster[oID].originX + (float)dimX)) - objMaster[oID].tScaleX / 100f * (objMaster[oID].originX + (float)dimX);
				objMaster[oID].f1[num].texOffsetY = objMaster[oID].tScaleY / 100f * objMaster[oID].originY - (float)(long)(objMaster[oID].tScaleY / 100f * objMaster[oID].originY);
				objMaster[oID].f1[num].v1 = new StructsClass.vtex[4];
				objMaster[oID].f1[num].v1[0] = new StructsClass.vtex();
				objMaster[oID].f1[num].v1[1] = new StructsClass.vtex();
				objMaster[oID].f1[num].v1[2] = new StructsClass.vtex();
				objMaster[oID].f1[num].v1[3] = new StructsClass.vtex();
				objMaster[oID].f1[num].v1[0].v[0] = dimX;
				objMaster[oID].f1[num].v1[0].v[1] = 0f;
				objMaster[oID].f1[num].v1[0].v[2] = 0f;
				objMaster[oID].f1[num].v1[1].v[0] = 0f;
				objMaster[oID].f1[num].v1[1].v[1] = 0f;
				objMaster[oID].f1[num].v1[1].v[2] = 0f;
				objMaster[oID].f1[num].v1[2].v[0] = 0f;
				objMaster[oID].f1[num].v1[2].v[1] = dimY;
				objMaster[oID].f1[num].v1[2].v[2] = 0f;
				objMaster[oID].f1[num].v1[3].v[0] = dimX;
				objMaster[oID].f1[num].v1[3].v[1] = dimY;
				objMaster[oID].f1[num].v1[3].v[2] = 0f;
				objMaster[oID].f1[num].n1 = new StructsClass.vnorm[4];
				objMaster[oID].f1[num].n1[0] = new StructsClass.vnorm();
				objMaster[oID].f1[num].n1[1] = new StructsClass.vnorm();
				objMaster[oID].f1[num].n1[2] = new StructsClass.vnorm();
				objMaster[oID].f1[num].n1[3] = new StructsClass.vnorm();
				Calculate_ParticleFace_Tangents(oID, num);
				crossProductandNorm(oID, num, 0);
				crossProductandNorm(oID, num, 1);
				crossProductandNorm(oID, num, 2);
				crossProductandNorm(oID, num, 3);
				num++;
			}
			if ((objMaster[oID].faces & 4) != 0)
			{
				objMaster[oID].f1[num].faceID = 2;
				objMaster[oID].f1[num].u = objMaster[oID].tScaleX / 100f * (float)dimX;
				objMaster[oID].f1[num].v = objMaster[oID].tScaleY / 100f * (float)dimZ;
				objMaster[oID].f1[num].texOffsetX = objMaster[oID].tScaleX / 100f * objMaster[oID].originX - (float)(long)(objMaster[oID].tScaleX / 100f * objMaster[oID].originX);
				objMaster[oID].f1[num].texOffsetY = objMaster[oID].tScaleY / 100f * objMaster[oID].originZ - (float)(long)(objMaster[oID].tScaleY / 100f * objMaster[oID].originZ);
				objMaster[oID].f1[num].v1 = new StructsClass.vtex[4];
				objMaster[oID].f1[num].v1[0] = new StructsClass.vtex();
				objMaster[oID].f1[num].v1[1] = new StructsClass.vtex();
				objMaster[oID].f1[num].v1[2] = new StructsClass.vtex();
				objMaster[oID].f1[num].v1[3] = new StructsClass.vtex();
				objMaster[oID].f1[num].v1[0].v[0] = 0f;
				objMaster[oID].f1[num].v1[0].v[1] = 0f;
				objMaster[oID].f1[num].v1[0].v[2] = 0f;
				objMaster[oID].f1[num].v1[1].v[0] = dimX;
				objMaster[oID].f1[num].v1[1].v[1] = 0f;
				objMaster[oID].f1[num].v1[1].v[2] = 0f;
				objMaster[oID].f1[num].v1[2].v[0] = dimX;
				objMaster[oID].f1[num].v1[2].v[1] = 0f;
				objMaster[oID].f1[num].v1[2].v[2] = dimZ;
				objMaster[oID].f1[num].v1[3].v[0] = 0f;
				objMaster[oID].f1[num].v1[3].v[1] = 0f;
				objMaster[oID].f1[num].v1[3].v[2] = dimZ;
				objMaster[oID].f1[num].n1 = new StructsClass.vnorm[4];
				objMaster[oID].f1[num].n1[0] = new StructsClass.vnorm();
				objMaster[oID].f1[num].n1[1] = new StructsClass.vnorm();
				objMaster[oID].f1[num].n1[2] = new StructsClass.vnorm();
				objMaster[oID].f1[num].n1[3] = new StructsClass.vnorm();
				Calculate_ParticleFace_Tangents(oID, num);
				crossProductandNorm(oID, num, 0);
				crossProductandNorm(oID, num, 1);
				crossProductandNorm(oID, num, 2);
				crossProductandNorm(oID, num, 3);
				num++;
			}
			if ((objMaster[oID].faces & 3) != 0)
			{
				objMaster[oID].f1[num].faceID = 3;
				objMaster[oID].f1[num].u = objMaster[oID].tScaleX / 100f * (float)dimX;
				objMaster[oID].f1[num].v = objMaster[oID].tScaleY / 100f * (float)dimZ;
				objMaster[oID].f1[num].texOffsetX = (float)(long)(objMaster[oID].tScaleX / 100f * (objMaster[oID].originX + (float)dimX)) - objMaster[oID].tScaleX / 100f * (objMaster[oID].originX + (float)dimX);
				objMaster[oID].f1[num].texOffsetY = objMaster[oID].tScaleY / 100f * objMaster[oID].originZ - (float)(long)(objMaster[oID].tScaleY / 100f * objMaster[oID].originZ);
				objMaster[oID].f1[num].v1 = new StructsClass.vtex[4];
				objMaster[oID].f1[num].v1[0] = new StructsClass.vtex();
				objMaster[oID].f1[num].v1[1] = new StructsClass.vtex();
				objMaster[oID].f1[num].v1[2] = new StructsClass.vtex();
				objMaster[oID].f1[num].v1[3] = new StructsClass.vtex();
				objMaster[oID].f1[num].v1[0].v[0] = dimX;
				objMaster[oID].f1[num].v1[0].v[1] = dimY;
				objMaster[oID].f1[num].v1[0].v[2] = 0f;
				objMaster[oID].f1[num].v1[1].v[0] = 0f;
				objMaster[oID].f1[num].v1[1].v[1] = dimY;
				objMaster[oID].f1[num].v1[1].v[2] = 0f;
				objMaster[oID].f1[num].v1[2].v[0] = 0f;
				objMaster[oID].f1[num].v1[2].v[1] = dimY;
				objMaster[oID].f1[num].v1[2].v[2] = dimZ;
				objMaster[oID].f1[num].v1[3].v[0] = dimX;
				objMaster[oID].f1[num].v1[3].v[1] = dimY;
				objMaster[oID].f1[num].v1[3].v[2] = dimZ;
				objMaster[oID].f1[num].n1 = new StructsClass.vnorm[4];
				objMaster[oID].f1[num].n1[0] = new StructsClass.vnorm();
				objMaster[oID].f1[num].n1[1] = new StructsClass.vnorm();
				objMaster[oID].f1[num].n1[2] = new StructsClass.vnorm();
				objMaster[oID].f1[num].n1[3] = new StructsClass.vnorm();
				Calculate_ParticleFace_Tangents(oID, num);
				crossProductandNorm(oID, num, 0);
				crossProductandNorm(oID, num, 1);
				crossProductandNorm(oID, num, 2);
				crossProductandNorm(oID, num, 3);
				num++;
			}
			objMaster[oID].fcount = 6L;
		}
	}

	public bool Melee_BreakApartItems(ushort playerID, float distance, byte ammoIndex, out ushort objectID, out float distanceHit)
	{
		byte rBufferID = global::Rendering.Rendering.rBufferID;
		float num = distance * distance;
		bool flag = false;
		float num2 = 0f;
		ushort num3 = 0;
		for (ushort num4 = 0; num4 < numGameObjects; num4++)
		{
			if (Game_Objects[num4].state == 1)
			{
				float num5 = Game_Objects[num4].x - global::Players.Players.players[playerID].posX[rBufferID];
				float num6 = Game_Objects[num4].y - global::Players.Players.players[playerID].posY[rBufferID];
				float num7 = Game_Objects[num4].z - global::Players.Players.players[playerID].posZ[rBufferID];
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
						num8 = num5 * global::Players.Players.players[playerID].mv[rBufferID].M21 + num6 * global::Players.Players.players[playerID].mv[rBufferID].M22;
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

	public void Set_Game_Objects_Collision_Models_To_Current_State(ushort objID)
	{
		if (Game_Objects[objID].state == 2)
		{
			ushort numCollisionModels = Game_Objects[objID].numCollisionModels;
			for (ushort num = 0; num < numCollisionModels; num++)
			{
				mainC.zonesMain.Remove_CollisionModel_From_Zone(Game_Objects[objID].colModelZones[num], Game_Objects[objID].gid);
			}
			if (Game_Objects[objID].collisionModelListID < global::MainGame.MainGame.numCollisionModels)
			{
				mainC.zonesMain.Remove_CollisionModel_From_Zone(global::Models.Models.modelList[Game_Objects[objID].collisionModelListID].bufferID[0], Game_Objects[objID].gid);
				mainC.zonesMain.Remove_CollisionModel_From_Zone(global::Models.Models.modelList[Game_Objects[objID].collisionModelListID].bufferID[1], Game_Objects[objID].gid);
				mainC.zonesMain.Add_CollisionModel_To_Zone(global::Models.Models.modelList[Game_Objects[objID].collisionModelListID].bufferID[2], (ushort)global::Models.Models.modelList[Game_Objects[objID].collisionModelListID].modelID[2], Game_Objects[objID].gid, ref Game_Objects[objID].mv1);
				mainC.zonesMain.Add_CollisionModel_To_Zone(global::Models.Models.modelList[Game_Objects[objID].collisionModelListID].bufferID[3], (ushort)global::Models.Models.modelList[Game_Objects[objID].collisionModelListID].modelID[3], Game_Objects[objID].gid, ref Game_Objects[objID].mv1);
			}
		}
		else if (Game_Objects[objID].state == 1)
		{
			ushort numCollisionModels = Game_Objects[objID].numCollisionModels;
			for (ushort num = 0; num < numCollisionModels; num++)
			{
				mainC.zonesMain.Add_CollisionModel_To_Zone(Game_Objects[objID].colModelZones[num], Game_Objects[objID].colModels[num], Game_Objects[objID].gid, ref Game_Objects[objID].mv1);
			}
			if (Game_Objects[objID].collisionModelListID < global::MainGame.MainGame.numCollisionModels)
			{
				mainC.zonesMain.Remove_CollisionModel_From_Zone(global::Models.Models.modelList[Game_Objects[objID].collisionModelListID].bufferID[2], Game_Objects[objID].gid);
				mainC.zonesMain.Remove_CollisionModel_From_Zone(global::Models.Models.modelList[Game_Objects[objID].collisionModelListID].bufferID[3], Game_Objects[objID].gid);
				mainC.zonesMain.Add_CollisionModel_To_Zone(global::Models.Models.modelList[Game_Objects[objID].collisionModelListID].bufferID[0], (ushort)global::Models.Models.modelList[Game_Objects[objID].collisionModelListID].modelID[0], Game_Objects[objID].gid, ref Game_Objects[objID].mv1);
				mainC.zonesMain.Add_CollisionModel_To_Zone(global::Models.Models.modelList[Game_Objects[objID].collisionModelListID].bufferID[1], (ushort)global::Models.Models.modelList[Game_Objects[objID].collisionModelListID].modelID[1], Game_Objects[objID].gid, ref Game_Objects[objID].mv1);
			}
		}
		else
		{
			ushort numCollisionModels = Game_Objects[objID].numCollisionModels;
			for (ushort num = 0; num < numCollisionModels; num++)
			{
				mainC.zonesMain.Remove_CollisionModel_From_Zone(Game_Objects[objID].colModelZones[num], Game_Objects[objID].gid);
			}
			if (Game_Objects[objID].collisionModelListID < global::MainGame.MainGame.numCollisionModels)
			{
				mainC.zonesMain.Remove_CollisionModel_From_Zone(global::Models.Models.modelList[Game_Objects[objID].collisionModelListID].bufferID[0], Game_Objects[objID].gid);
				mainC.zonesMain.Remove_CollisionModel_From_Zone(global::Models.Models.modelList[Game_Objects[objID].collisionModelListID].bufferID[1], Game_Objects[objID].gid);
				mainC.zonesMain.Remove_CollisionModel_From_Zone(global::Models.Models.modelList[Game_Objects[objID].collisionModelListID].bufferID[2], Game_Objects[objID].gid);
				mainC.zonesMain.Remove_CollisionModel_From_Zone(global::Models.Models.modelList[Game_Objects[objID].collisionModelListID].bufferID[3], Game_Objects[objID].gid);
			}
		}
	}

	public void Update_Game_Object_Team(ushort objID, ushort teamID)
	{
		if (Game_Objects[objID].isTarget)
		{
			mainC.targetMain.Update_Damage_Target_Team(Game_Objects[objID].targetID, teamID);
		}
	}

	public ushort Add_New_GameObject(float x, float y, float z, ushort modID, ushort cloneID, byte incrementAmount)
	{
		ushort num = numGameObjects;
		if (++numGameObjects >= numAllocatedGameObjects)
		{
			StructsClass.Game_Object[] array = new StructsClass.Game_Object[num];
			for (ushort num2 = 0; num2 < num; num2++)
			{
				ref StructsClass.Game_Object reference = ref array[num2];
				reference = Game_Objects[num2];
			}
			numAllocatedGameObjects = (ushort)(numGameObjects + incrementAmount);
			Game_Objects = new StructsClass.Game_Object[numAllocatedGameObjects];
			for (ushort num2 = 0; num2 < num; num2++)
			{
				ref StructsClass.Game_Object reference2 = ref Game_Objects[num2];
				reference2 = array[num2];
			}
		}
		StructsClass.Initialize_Physics_New(ref Game_Objects[num].phy);
		ref StructsClass.Game_Object reference3 = ref Game_Objects[num];
		reference3 = Game_Objects[cloneID];
		Game_Objects[num].ID = Get_Unique_Game_Object_ID();
		Game_Objects[num].gid = mainC.maingameMain.Register_Game_Item(0, Game_Objects[num].ID, num);
		Game_Objects[num].numModels = 1;
		Game_Objects[num].modID = new ushort[1];
		Game_Objects[num].modID[0] = modID;
		Game_Objects[num].x = x;
		Game_Objects[num].y = y;
		Game_Objects[num].z = z;
		Set_GameObject_Starting_Position(num);
		return num;
	}

	public ushort Get_Unique_Game_Object_ID()
	{
		bool flag = false;
		for (ushort num = 0; num < numGameObjects; num++)
		{
			if (Game_Objects[num].ID >= numGameObjects)
			{
				flag = true;
				break;
			}
		}
		if (!flag)
		{
			return numGameObjects;
		}
		for (ushort num = 0; num < numGameObjects; num++)
		{
			bool flag2 = false;
			for (ushort num2 = 0; num2 < numGameObjects; num2++)
			{
				if (Game_Objects[num2].ID == num)
				{
					flag2 = true;
					break;
				}
			}
			if (!flag2)
			{
				return num;
			}
		}
		return 0;
	}

	public void Set_GameObject_Starting_Position(ushort objectID)
	{
		Game_Objects[objectID].mvStart = Matrix.CreateScale(Game_Objects[objectID].scaleX, Game_Objects[objectID].scaleY, Game_Objects[objectID].scaleZ) * Matrix.CreateRotationY(Game_Objects[objectID].rotY * ((float)Math.PI / 180f)) * Matrix.CreateRotationX(Game_Objects[objectID].rotX * ((float)Math.PI / 180f)) * Matrix.CreateRotationZ(Game_Objects[objectID].rotZ * ((float)Math.PI / 180f));
		Game_Objects[objectID].mv1 = Game_Objects[objectID].mvStart;
		Game_Objects[objectID].qn1 = Quaternion.CreateFromRotationMatrix(Game_Objects[objectID].mvStart);
		StructsClass.Reset_Physics_New(ref Game_Objects[objectID].phy);
		Game_Objects[objectID].phy.x = Game_Objects[objectID].x;
		Game_Objects[objectID].phy.y = Game_Objects[objectID].y;
		Game_Objects[objectID].phy.z = Game_Objects[objectID].z;
		Game_Objects[objectID].mv1.M41 = Game_Objects[objectID].x;
		Game_Objects[objectID].mv1.M42 = Game_Objects[objectID].y;
		Game_Objects[objectID].mv1.M43 = Game_Objects[objectID].z;
	}

	public void Rotate_Game_Object(ushort objectID, float rotX, float rotY, float rotZ)
	{
		Game_Objects[objectID].rotX += rotX;
		Game_Objects[objectID].rotY += rotY;
		Game_Objects[objectID].rotZ += rotZ;
		Game_Objects[objectID].mvStart = Matrix.CreateScale(Game_Objects[objectID].scaleX, Game_Objects[objectID].scaleY, Game_Objects[objectID].scaleZ) * Matrix.CreateRotationY(Game_Objects[objectID].rotY * ((float)Math.PI / 180f)) * Matrix.CreateRotationX(Game_Objects[objectID].rotX * ((float)Math.PI / 180f)) * Matrix.CreateRotationZ(Game_Objects[objectID].rotZ * ((float)Math.PI / 180f));
		Game_Objects[objectID].mv1 = Game_Objects[objectID].mvStart;
		Game_Objects[objectID].qn1 = Quaternion.CreateFromRotationMatrix(Game_Objects[objectID].mvStart);
		StructsClass.Reset_Physics_New(ref Game_Objects[objectID].phy);
		Game_Objects[objectID].mv1.M41 = Game_Objects[objectID].x;
		Game_Objects[objectID].mv1.M42 = Game_Objects[objectID].y;
		Game_Objects[objectID].mv1.M43 = Game_Objects[objectID].z;
	}

	public void Update_GameObject_Targets()
	{
		for (ushort num = 0; num < numGameObjects; num++)
		{
			if (Game_Objects[num].isTarget)
			{
				mainC.targetMain.Set_DamageTarget_Location(Game_Objects[num].targetID, Game_Objects[num].phy.x, Game_Objects[num].phy.y, Game_Objects[num].phy.z);
			}
		}
	}

	public ushort Find_Closest_Game_Object(float x, float y)
	{
		ushort result = 0;
		if (numGameObjects < 1)
		{
			return 0;
		}
		float num = Game_Objects[0].x - x;
		float num2 = Game_Objects[0].y - y;
		float num3 = num * num + num2 * num2;
		for (ushort num4 = 1; num4 < numGameObjects; num4++)
		{
			num = Game_Objects[num4].x - x;
			num2 = Game_Objects[num4].y - y;
			float num5 = num * num + num2 * num2;
			if (num5 < num3)
			{
				num3 = num5;
				result = num4;
			}
		}
		return result;
	}

	public void Resize_Object(int objID, byte dimension, int amount, byte threadID)
	{
		if (objID < 0 || objID >= numObjects || (objStat[objID] & 8) == 0)
		{
			return;
		}
		switch (dimension)
		{
		case 1:
		{
			long num = objMaster[objID].dimX + amount;
			if (num > global::Util.Util.maxUnsignedIntValue)
			{
				num = global::Util.Util.maxUnsignedIntValue;
			}
			else if (num < 1)
			{
				num = 1L;
			}
			objMaster[objID].dimX = (uint)num;
			break;
		}
		case 2:
		{
			long num = objMaster[objID].dimY + amount;
			if (num > global::Util.Util.maxUnsignedIntValue)
			{
				num = global::Util.Util.maxUnsignedIntValue;
			}
			else if (num < 1)
			{
				num = 1L;
			}
			objMaster[objID].dimY = (uint)num;
			break;
		}
		default:
		{
			long num = objMaster[objID].dimZ + amount;
			if (num > global::Util.Util.maxUnsignedIntValue)
			{
				num = global::Util.Util.maxUnsignedIntValue;
			}
			else if (num < 1)
			{
				num = 1L;
			}
			objMaster[objID].dimZ = (uint)num;
			break;
		}
		}
		Regen_Object(objID, threadID);
		mainC.collisionMain.Update_CollisionBox((short)objID, threadID);
	}

	public void Move_Object(int objID, byte dimension, float amount, byte threadID)
	{
		if (objID >= 0 && objID < numObjects && (objStat[objID] & 8) != 0)
		{
			switch (dimension)
			{
			case 1:
				objMaster[objID].x += amount;
				break;
			case 2:
				objMaster[objID].y += amount;
				break;
			default:
				objMaster[objID].z += amount;
				break;
			}
			Update_Object_BoundingBox(objID);
			mainC.collisionMain.Update_CollisionBox((short)objID, threadID);
		}
	}

	public void Rotate_Object(int objID, byte dimension, float amount, byte threadID)
	{
		if (objID >= 0 && objID < numObjects && (objStat[objID] & 8) != 0)
		{
			switch (dimension)
			{
			case 1:
				objMaster[objID].rotX += amount;
				break;
			case 2:
				objMaster[objID].rotY += amount;
				break;
			default:
				objMaster[objID].rotZ += amount;
				break;
			}
			if (objMaster[objID].rotX == 0f && objMaster[objID].rotY == 0f && objMaster[objID].rotZ == 0f)
			{
				objMaster[objID].isRotated = false;
				objMaster[objID].mv = Matrix.Identity;
				objMaster[objID].mvT = Matrix.Identity;
				mainC.collisionMain.Update_CollisionBox((short)objID, threadID);
			}
			else
			{
				objMaster[objID].isRotated = true;
				objMaster[objID].mv = Matrix.CreateRotationY(objMaster[objID].rotY * ((float)Math.PI / 180f)) * Matrix.CreateRotationX(objMaster[objID].rotX * ((float)Math.PI / 180f)) * Matrix.CreateRotationZ(objMaster[objID].rotZ * ((float)Math.PI / 180f));
				objMaster[objID].mvT = Matrix.Invert(objMaster[objID].mv);
				mainC.collisionMain.Update_CollisionBox((short)objID, threadID);
			}
		}
	}

	public void Reset_Round(bool minorReset, byte threadID)
	{
		mpScoreChanged = false;
		mpTimeBeforePointsUpdate = 2f;
		for (int i = 0; i < numCollections; i++)
		{
			if (objCol[i].active)
			{
				for (int j = 0; j < objCol[i].numObj; j++)
				{
					int num = objCol[i].objList[j];
					objMaster[num].x = objCol[i].x + objCol[i].objOffsetX[j];
					objMaster[num].y = objCol[i].y + objCol[i].objOffsetY[j];
					objMaster[num].z = objCol[i].z + objCol[i].objOffsetZ[j];
					mainC.renderingMain.Update_Instance_Position((ushort)objMaster[num].instanceID, 0, objCol[i].x + objCol[i].renderOffsetX[j], objCol[i].y + objCol[i].renderOffsetY[j], objCol[i].z + objCol[i].renderOffsetZ[j]);
					mainC.collisionMain.Update_CollisionBox((short)num, threadID);
					Update_Object_BoundingBox(num);
				}
			}
		}
		for (int i = 0; i < numGameObjects; i++)
		{
			Game_Objects[i].particleTimer = 0f;
			Game_Objects[i].mv1 = Game_Objects[i].mvStart;
			Game_Objects[i].qn1 = Quaternion.CreateFromRotationMatrix(Game_Objects[i].mvStart);
			StructsClass.Reset_Physics_New(ref Game_Objects[i].phy);
			Game_Objects[i].phy.x = Game_Objects[i].x;
			Game_Objects[i].phy.y = Game_Objects[i].y;
			Game_Objects[i].phy.z = Game_Objects[i].z;
			Game_Objects[i].mv1.M41 = Game_Objects[i].x;
			Game_Objects[i].mv1.M42 = Game_Objects[i].y;
			Game_Objects[i].mv1.M43 = Game_Objects[i].z;
			Game_Objects[i].state = (byte)(Game_Objects[i].startsActive ? 1u : 0u);
			Game_Objects[i].doPhysics = Game_Objects[i].usesPhysics;
			Game_Objects[i].curDamage = 0f;
			if (Game_Objects[i].isTarget)
			{
				mainC.targetMain.Set_DamageTarget_Max_Damage(Game_Objects[i].targetID, Game_Objects[i].maxDamage);
			}
			int num = Game_Objects[i].numCollisionModels;
			for (int j = 0; j < num; j++)
			{
				mainC.zonesMain.Add_CollisionModel_To_Zone(Game_Objects[i].colModelZones[j], Game_Objects[i].colModels[j], Game_Objects[i].gid, ref Game_Objects[i].mv1);
			}
			if (Game_Objects[i].collisionModelListID < global::MainGame.MainGame.numCollisionModels)
			{
				mainC.zonesMain.Remove_CollisionModel_From_Zone(global::Models.Models.modelList[Game_Objects[i].collisionModelListID].bufferID[0], Game_Objects[i].gid);
				mainC.zonesMain.Remove_CollisionModel_From_Zone(global::Models.Models.modelList[Game_Objects[i].collisionModelListID].bufferID[1], Game_Objects[i].gid);
				mainC.zonesMain.Add_CollisionModel_To_Zone(global::Models.Models.modelList[Game_Objects[i].collisionModelListID].bufferID[0], (ushort)global::Models.Models.modelList[Game_Objects[i].collisionModelListID].modelID[0], Game_Objects[i].gid, ref Game_Objects[i].mv1);
				mainC.zonesMain.Add_CollisionModel_To_Zone(global::Models.Models.modelList[Game_Objects[i].collisionModelListID].bufferID[1], (ushort)global::Models.Models.modelList[Game_Objects[i].collisionModelListID].modelID[1], Game_Objects[i].gid, ref Game_Objects[i].mv1);
			}
		}
	}

	public void crossProductandNorm(long oID, long x, int a)
	{
		int num = a + 1;
		int num2 = a + 2;
		switch (a)
		{
		case 2:
			num2 = 0;
			break;
		case 3:
			num = 0;
			num2 = 1;
			break;
		}
		mainC.utilMain.crossProductNorm(ref objMaster[oID].f1[x].v1[a], ref objMaster[oID].f1[x].v1[num], ref objMaster[oID].f1[x].v1[num2], ref objMaster[oID].f1[x].n1[a]);
	}

	public void Calculate_ParticleFace_Tangents(long objID, long pID)
	{
		cpftV1 = objMaster[objID].f1[pID].v1[0];
		cpftV2 = objMaster[objID].f1[pID].v1[1];
		cpftV3 = objMaster[objID].f1[pID].v1[3];
		cpftT1.t[0] = 0f;
		cpftT1.t[1] = 0f;
		cpftT2.t[0] = objMaster[objID].f1[pID].u;
		cpftT2.t[1] = 0f;
		cpftT3.t[0] = 0f;
		cpftT3.t[1] = objMaster[objID].f1[pID].v;
		objMaster[objID].f1[pID].tangent = new StructsClass.vtex();
		mainC.utilMain.Calc_Tangent(ref cpftV1, ref cpftV2, ref cpftV3, ref cpftT1, ref cpftT2, ref cpftT3, ref objMaster[objID].f1[pID].tangent);
	}

	public short Register_Object()
	{
		for (short num = 0; num < numObjects; num++)
		{
			if (objStat[num] < 2)
			{
				return num;
			}
		}
		return -1;
	}

	public long Find_Object(ref StructsClass.gameobject localO)
	{
		for (long num = 0L; num < numObjects; num++)
		{
			if (objStat[num] > 1 && objMaster[num].objRefID == localO.objRefID)
			{
				return num;
			}
		}
		return -1L;
	}

	public bool Set_Object_For_Regen(long oID)
	{
		if ((objStat[oID] & 6) != 0)
		{
			objStat[oID] = 4;
		}
		return true;
	}

	public int Find_Next_Object_Type(int curID, byte type, byte status)
	{
		for (int i = curID + 1; i < numObjects; i++)
		{
			if (objStat[i] == status && (objMaster[i].type & type) > 128)
			{
				return i;
			}
		}
		for (int i = 0; i < curID; i++)
		{
			if (objStat[i] == status && (objMaster[i].type & type) > 128)
			{
				return i;
			}
		}
		return curID;
	}

	public int Find_Previous_Object_Type(int curID, byte type, byte status)
	{
		for (int num = curID - 1; num > -1; num--)
		{
			if (objStat[num] == status && (objMaster[num].type & type) > 128)
			{
				return num;
			}
		}
		for (int num = numObjects - 1; num > curID; num--)
		{
			if (objStat[num] == status && (objMaster[num].type & type) > 128)
			{
				return num;
			}
		}
		return curID;
	}

	public void Send_GameObjects_To_New_Player(NetworkGamer newGamer)
	{
		if (numGameObjects < 1)
		{
			return;
		}
		ushort num = (ushort)((numGameObjects <= 16) ? numGameObjects : 16);
		num = (ushort)((num * 2 <= 30) ? num : 15);
		ushort num2 = 0;
		ushort num3 = 0;
		ushort num4 = 0;
		ushort num5 = 0;
		while (num2 < numGameObjects)
		{
			global::Networking.Networking.networkBytes[num3++] = Game_Objects[num2].state;
			global::Networking.Networking.networkBytes[num3++] = mainC.targetMain.Get_Team_For_Target(Game_Objects[num2].targetID);
			ref HalfSingle reference = ref global::Networking.Networking.networkHS[num4];
			reference = new HalfSingle(Game_Objects[num2].curDamage);
			if (++num4 >= num)
			{
				global::Networking.Networking.networkUShorts[0] = num;
				global::Networking.Networking.networkUShorts[1] = num5;
				global::Networking.Networking.networkUShorts[2] = numGameObjects;
				mainC.networkingMain.XBOX_Send_Network_Message_To_Gamer(47, newGamer);
				num4 = 0;
				num3 = 0;
				num5 = (ushort)(num2 + 1);
			}
			num2++;
		}
		if (num4 > 0)
		{
			global::Networking.Networking.networkUShorts[0] = num4;
			global::Networking.Networking.networkUShorts[1] = num5;
			global::Networking.Networking.networkUShorts[2] = numGameObjects;
			mainC.networkingMain.XBOX_Send_Network_Message_To_Gamer(47, newGamer);
		}
	}

	public void Receive_GameObjects_From_Host()
	{
		ushort num = global::Networking.Networking.networkUShorts[0];
		numGameObjects = global::Networking.Networking.networkUShorts[2];
		ushort num2 = global::Networking.Networking.networkUShorts[1];
		ushort num3 = 0;
		while (num2 < num)
		{
			Game_Objects[num2].state = global::Networking.Networking.networkBytes[num3++];
			mainC.targetMain.Update_Damage_Target_Team(Game_Objects[num2].targetID, global::Networking.Networking.networkBytes[num3++]);
			Game_Objects[num2].curDamage = global::Networking.Networking.networkHS[num2].ToSingle();
			if (Game_Objects[num2].isTarget)
			{
				mainC.targetMain.Set_DamageTarget_Damage(Game_Objects[num2].targetID, Game_Objects[num2].curDamage);
			}
			Set_Game_Objects_Collision_Models_To_Current_State(num2);
			num2++;
		}
	}

	public void Update_Points_For_Damaging_Object_And_Send(ushort playerID, ushort objID, float damage)
	{
		byte threadID = 0;
		if (global::MainGame.MainGame.roundOver)
		{
			return;
		}
		bool flag = Game_Objects[objID].curDamage >= Game_Objects[objID].maxDamage;
		if (Game_Objects[objID].isTarget)
		{
			mainC.targetMain.Set_DamageTarget_Damage(Game_Objects[objID].targetID, Game_Objects[objID].curDamage);
		}
		if ((global::Players.Players.players[playerID].teamMask & mainC.playersMain.Get_Team_Mask(mainC.targetMain.Get_Team_For_Target(Game_Objects[objID].targetID))) == 0)
		{
			int num = 0;
			if (flag)
			{
				num = Game_Objects[objID].points;
			}
			if (Game_Objects[objID].isTarget)
			{
				float num2 = damage / Game_Objects[objID].maxDamage;
				if (num2 > 1f)
				{
					num2 = 1f;
				}
				num += (int)Math.Round(num2 * (float)mainC.targetMain.Get_Damage_Target_Points(Game_Objects[objID].targetID));
			}
			global::Players.Players.players[playerID].objectivePoints += num;
			global::Players.Players.teamPoints[global::Players.Players.players[playerID].team] += num;
			global::MainGame.MainGame.gameData.players[playerID].scoresI[0] += num;
			mainC.userInterface.Mark_Window_As_Needing_Updating(10);
			mainC.userInterface.Mark_Window_As_Needing_Updating(9);
		}
		if (global::MainGame.MainGame.gameMode != 0)
		{
			if (flag)
			{
				global::Networking.Networking.networkUShorts[0] = objID;
				global::Networking.Networking.networkShorts[0] = global::Players.Players.players[playerID].id;
				mainC.networkingMain.XBOX_Send_Network_Message49(49);
				Game_Object_Destroyed(objID, playerID, threadID);
			}
			mainC.networkingMain.Mark_Team_Points_To_Be_Sent(2f);
			mainC.playersMain.Mark_Players_Points_To_Send(playerID);
			if (global::Networking.Networking.isHost && mainC.gameLogic.Game_Is_MP_Round_Over())
			{
				mainC.networkingMain.XBOX_MP_Round_Over();
			}
		}
	}

	public void Send_Object_Damage(ushort objID, float damage)
	{
		global::Networking.Networking.networkUShorts[0] = objID;
		ref HalfSingle reference = ref global::Networking.Networking.networkHS[0];
		reference = new HalfSingle(damage);
		mainC.networkingMain.XBOX_Send_Network_Message48(48);
	}

	public void Receive_Object_Damage(byte actID)
	{
		ushort num = global::Networking.Networking.networkUShorts[0];
		float num2 = global::Networking.Networking.networkHS[0].ToSingle();
		Game_Objects[num].curDamage += num2;
		if (!global::Networking.Networking.isHost)
		{
			if (Game_Objects[num].curDamage >= Game_Objects[num].maxDamage)
			{
				Game_Objects[num].curDamage = Game_Objects[num].maxDamage;
			}
			if (Game_Objects[num].isTarget)
			{
				mainC.targetMain.Set_DamageTarget_Damage(Game_Objects[num].targetID, Game_Objects[num].curDamage);
			}
		}
		else
		{
			short num3 = mainC.playersMain.Get_Player_Index(actID, -1);
			if (num3 >= 0)
			{
				Update_Points_For_Damaging_Object_And_Send((ushort)num3, num, num2);
			}
		}
	}

	public void Receive_Object_Destroyed()
	{
		short num = mainC.playersMain.Get_Player_Index(global::Networking.Networking.networkShorts[0], 0);
		Game_Object_Destroyed(global::Networking.Networking.networkUShorts[0], (ushort)num, 0);
		if (Game_Objects[global::Networking.Networking.networkUShorts[0]].isTarget && num == 0)
		{
			mainC.gameLogic.Game_Scored_Target();
		}
	}

	public void Update_VBO(long id)
	{
	}

	public void Create_VBO_Static_Opaque(int textureID, int vboType, ref StructsClass.VertexPositionColorNormalTexture[] vtexObjects, ref int[] viObjects, ref int vPtr, ref int iPtr, ref int primitiveCount)
	{
		float x = 0f;
		float y = 0f;
		if (vboType != 1 || numMainBufferFacesOpaque < 1)
		{
			return;
		}
		for (long num = 0L; num < numObjects; num++)
		{
			if ((objStat[num] & 6) == 0 || objMaster[num].instanceID >= 0 || objMaster[num].color[3] != 1f)
			{
				continue;
			}
			for (byte b = 0; b < 6; b++)
			{
				if (objMaster[num].texIDs[b] == textureID)
				{
					objMaster[num].vboPtr = vPtr;
					objMaster[num].ibPtr = iPtr;
					for (long num2 = 0L; num2 < objMaster[num].fcount; num2++)
					{
						if (objMaster[num].f1[num2].faceID != b)
						{
							continue;
						}
						primitiveCount += 2;
						viObjects[iPtr++] = vPtr;
						viObjects[iPtr++] = vPtr + 1;
						viObjects[iPtr++] = vPtr + 2;
						viObjects[iPtr++] = vPtr;
						viObjects[iPtr++] = vPtr + 2;
						viObjects[iPtr++] = vPtr + 3;
						for (long num3 = 0L; num3 < 4; num3++)
						{
							float u = objMaster[num].f1[num2].u;
							float v = objMaster[num].f1[num2].v;
							float texOffsetX = objMaster[num].f1[num2].texOffsetX;
							float texOffsetY = objMaster[num].f1[num2].texOffsetY;
							long num4 = num3;
							if (num4 <= 3 && num4 >= 0)
							{
								switch ((int)num4)
								{
								case 0:
									x = texOffsetX;
									y = texOffsetY;
									break;
								case 1:
									x = u + texOffsetX;
									y = texOffsetY;
									break;
								case 2:
									x = u + texOffsetX;
									y = v + texOffsetY;
									break;
								case 3:
									x = texOffsetX;
									y = v + texOffsetY;
									break;
								}
							}
							cvbosoVcolor.R = (byte)(objMaster[num].color[0] * 255f);
							cvbosoVcolor.G = (byte)(objMaster[num].color[1] * 255f);
							cvbosoVcolor.B = (byte)(objMaster[num].color[2] * 255f);
							cvbosoVcolor.A = (byte)(objMaster[num].color[3] * 255f);
							cvsoN1.X = objMaster[num].f1[num2].n1[num3].n[0];
							cvsoN1.Y = objMaster[num].f1[num2].n1[num3].n[1];
							cvsoN1.Z = objMaster[num].f1[num2].n1[num3].n[2];
							cvsoT1.X = objMaster[num].f1[num2].tangent.v[0];
							cvsoT1.Y = objMaster[num].f1[num2].tangent.v[1];
							cvsoT1.Z = objMaster[num].f1[num2].tangent.v[2];
							if (objMaster[num].isRotated)
							{
								cvsoV1.X = objMaster[num].f1[num2].v1[num3].v[0] - 0.5f;
								cvsoV1.Y = objMaster[num].f1[num2].v1[num3].v[1] - 0.5f;
								cvsoV1.Z = objMaster[num].f1[num2].v1[num3].v[2] - 0.5f;
								Vector3.Transform(ref cvsoV1, ref objMaster[num].mv, out cvsoV1);
								Vector3.Transform(ref cvsoN1, ref objMaster[num].mv, out cvsoN1);
								cvsoV1.X += objMaster[num].x;
								cvsoV1.Y += objMaster[num].y;
								cvsoV1.Z += objMaster[num].z;
							}
							else
							{
								cvsoV1.X = objMaster[num].x + objMaster[num].f1[num2].v1[num3].v[0] - 0.5f;
								cvsoV1.Y = objMaster[num].y + objMaster[num].f1[num2].v1[num3].v[1] - 0.5f;
								cvsoV1.Z = objMaster[num].z + objMaster[num].f1[num2].v1[num3].v[2] - 0.5f;
							}
							ref StructsClass.VertexPositionColorNormalTexture reference = ref vtexObjects[vPtr++];
							reference = new StructsClass.VertexPositionColorNormalTexture(cvsoV1, cvbosoVcolor, cvsoN1, cvsoT1, new Vector2(x, y));
						}
					}
				}
			}
		}
	}

	public void Create_VBO_Static_Transparent(int textureID, int vboType, ref StructsClass.VertexPositionColorNormalTexture[] vtexObjects, ref int[] viObjects, ref int vPtr, ref int iPtr, ref int primitiveCount)
	{
		float x = 0f;
		float y = 0f;
		if (vboType != 5 || numMainBufferFacesTransparent < 1)
		{
			return;
		}
		for (long num = 0L; num < numObjects; num++)
		{
			if ((objStat[num] & 6) == 0 || objMaster[num].instanceID >= 0 || objMaster[num].texID != textureID || !(objMaster[num].color[3] < 1f))
			{
				continue;
			}
			objMaster[num].vboPtr = vPtr;
			objMaster[num].ibPtr = iPtr;
			for (long num2 = 0L; num2 < objMaster[num].fcount; num2++)
			{
				primitiveCount += 2;
				viObjects[iPtr++] = vPtr;
				viObjects[iPtr++] = vPtr + 1;
				viObjects[iPtr++] = vPtr + 2;
				viObjects[iPtr++] = vPtr;
				viObjects[iPtr++] = vPtr + 2;
				viObjects[iPtr++] = vPtr + 3;
				float u = objMaster[num].f1[num2].u;
				float v = objMaster[num].f1[num2].v;
				float texOffsetX = objMaster[num].f1[num2].texOffsetX;
				float texOffsetY = objMaster[num].f1[num2].texOffsetY;
				for (long num3 = 0L; num3 < 4; num3++)
				{
					long num4 = num3;
					if (num4 <= 3 && num4 >= 0)
					{
						switch ((int)num4)
						{
						case 0:
							x = texOffsetX;
							y = texOffsetY;
							break;
						case 1:
							x = u + texOffsetX;
							y = texOffsetY;
							break;
						case 2:
							x = u + texOffsetX;
							y = v + texOffsetY;
							break;
						case 3:
							x = texOffsetX;
							y = v + texOffsetY;
							break;
						}
					}
					ref StructsClass.VertexPositionColorNormalTexture reference = ref vtexObjects[vPtr++];
					reference = new StructsClass.VertexPositionColorNormalTexture(new Vector3(objMaster[num].x + objMaster[num].f1[num2].v1[num3].v[0] - 0.5f, objMaster[num].y + objMaster[num].f1[num2].v1[num3].v[1] - 0.5f, objMaster[num].z + objMaster[num].f1[num2].v1[num3].v[2] - 0.5f), new Color(objMaster[num].color[0], objMaster[num].color[1], objMaster[num].color[2], objMaster[num].color[3]), new Vector3(objMaster[num].f1[num2].n1[num3].n[0], objMaster[num].f1[num2].n1[num3].n[1], objMaster[num].f1[num2].n1[num3].n[2]), new Vector3(objMaster[num].f1[num2].tangent.v[0], objMaster[num].f1[num2].tangent.v[1], objMaster[num].f1[num2].tangent.v[2]), new Vector2(x, y));
				}
			}
		}
	}

	public int Count_Static_Buffer_Faces_Opaque()
	{
		numMainBufferFacesOpaque = 0;
		for (int i = 0; i < numObjects; i++)
		{
			if ((objStat[i] & 6) != 0 && objMaster[i].instanceID < 0 && objMaster[i].color[3] == 1f)
			{
				numMainBufferFacesOpaque += (int)objMaster[i].fcount;
			}
		}
		return numMainBufferFacesOpaque;
	}

	public int Count_Static_Buffer_Faces_Transparent()
	{
		numMainBufferFacesTransparent = 0;
		for (int i = 0; i < numObjects; i++)
		{
			if ((objStat[i] & 6) != 0 && objMaster[i].instanceID < 0 && objMaster[i].color[3] < 1f)
			{
				numMainBufferFacesTransparent += (int)objMaster[i].fcount;
			}
		}
		return numMainBufferFacesTransparent;
	}
}
