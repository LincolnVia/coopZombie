using System;
using System.Globalization;
using System.IO;
using Microsoft.Xna.Framework;
using Rendering;
using Structs;
using WindowsGame1;

namespace MainGame;

public class Zones
{
	public static byte numZones = 0;

	public static byte numAllocatedZones = 0;

	public static ushort playerZone = 0;

	public static ushort numZoneChecks;

	public static ushort numAllocatedZoneChecks;

	public static StructsClass.Zone[] zones;

	public static StructsClass.Zone_Check[] zoneChecks;

	public Game1.MasterCollection mainC;

	public void Init_Vars(Game1.MasterCollection master)
	{
		mainC = master;
	}

	public void Init_Zones()
	{
	}

	public void Load_Zone_Data(string fileName)
	{
		int num = 0;
		int num2 = -1;
		_ = global::Rendering.Rendering.uBufferID;
		for (int i = 0; i < numZones; i++)
		{
			zones[i].zoneList.numObjects = 0;
			zones[i].type = 0;
			zones[i].zoneID = 0;
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
				if (array4[0].Equals("numZones", StringComparison.OrdinalIgnoreCase))
				{
					num5 = 1;
				}
				else if (array4[0].Equals("Zone", StringComparison.OrdinalIgnoreCase))
				{
					num5 = 2;
				}
				else if (array4[0].Equals("Type", StringComparison.OrdinalIgnoreCase))
				{
					num5 = 3;
				}
				else if (array4[0].Equals("ID", StringComparison.OrdinalIgnoreCase))
				{
					num5 = 4;
				}
				else if (array4[0].Equals("Objects", StringComparison.OrdinalIgnoreCase))
				{
					num5 = 5;
				}
				else if (array4[0].Equals("Matrices", StringComparison.OrdinalIgnoreCase))
				{
					num5 = 6;
				}
				switch (num5)
				{
				case 1:
				{
					if (array4.Length <= 1)
					{
						break;
					}
					int num9 = int.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
					if (num9 > numAllocatedZones)
					{
						zones = new StructsClass.Zone[num9];
						for (int i = 0; i < num9; i++)
						{
							zones[i].zoneList = new StructsClass.Zone_List();
							zones[i].zoneList.numObjects = 0;
							zones[i].zoneList.numAllocatedObjects = 0;
							zones[i].type = 0;
							zones[i].zoneID = 0;
						}
						numAllocatedZones = (byte)num9;
					}
					numZones = (byte)num9;
					break;
				}
				case 2:
					num2++;
					if (num2 > -1 && num2 < numAllocatedZones)
					{
						num = 0;
					}
					else
					{
						num2 = -1;
					}
					break;
				case 3:
					if (array4.Length > 1 && num2 > -1)
					{
						zones[num2].type = byte.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 4:
					if (array4.Length > 1 && num2 > -1)
					{
						zones[num2].zoneID = ushort.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 5:
				{
					if (array4.Length <= 1 || num2 <= -1)
					{
						break;
					}
					int num9 = int.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
					if (array4.Length <= num9 * 2 + 1)
					{
						break;
					}
					if (zones[num2].zoneList.numAllocatedObjects < num9)
					{
						zones[num2].zoneList.oList = new ushort[num9];
						zones[num2].zoneList.gidList = new ushort[num9];
						zones[num2].zoneList.numAllocatedObjects = (ushort)num9;
						if (zones[num2].type == 1)
						{
							zones[num2].zoneList.matrixList = new Matrix[num9];
						}
					}
					zones[num2].zoneList.numObjects = (ushort)num9;
					int i = 0;
					int num10 = 2;
					for (; i < num9; i++)
					{
						zones[num2].zoneList.oList[i] = ushort.Parse(array4[num10++], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
						zones[num2].zoneList.gidList[i] = ushort.Parse(array4[num10++], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				}
				case 6:
					if (array4.Length > 6 && num2 > -1)
					{
						float xPosition = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
						float yPosition = float.Parse(array4[2], CultureInfo.InvariantCulture.NumberFormat);
						float zPosition = float.Parse(array4[3], CultureInfo.InvariantCulture.NumberFormat);
						float num6 = float.Parse(array4[4], CultureInfo.InvariantCulture.NumberFormat);
						float num7 = float.Parse(array4[5], CultureInfo.InvariantCulture.NumberFormat);
						float num8 = float.Parse(array4[6], CultureInfo.InvariantCulture.NumberFormat);
						ref Matrix reference = ref zones[num2].zoneList.matrixList[num++];
						reference = Matrix.CreateRotationZ(num8 * ((float)Math.PI / 180f)) * Matrix.CreateRotationY(num7 * ((float)Math.PI / 180f)) * Matrix.CreateRotationX(num6 * ((float)Math.PI / 180f)) * Matrix.CreateTranslation(xPosition, yPosition, zPosition);
					}
					break;
				}
			}
		}
		stream.Close();
	}

	public void Load_Zone_Check_Data(string fileName)
	{
		int num = -1;
		_ = global::Rendering.Rendering.uBufferID;
		for (int i = 0; i < numZoneChecks; i++)
		{
			zoneChecks[i].numBoxChecks = 0;
			zoneChecks[i].numSphereChecks = 0;
			zoneChecks[i].objID = 0;
			zoneChecks[i].type = 0;
			zoneChecks[i].zoneCheckID = 0;
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
				if (array4[0].Equals("numZoneChecks", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 1;
				}
				else if (array4[0].Equals("ZoneCheck", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 2;
				}
				else if (array4[0].Equals("Type", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 3;
				}
				else if (array4[0].Equals("ID", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 4;
				}
				else if (array4[0].Equals("ObjectID", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 5;
				}
				else if (array4[0].Equals("Spheres", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 6;
				}
				else if (array4[0].Equals("Boxes", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 7;
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
					if (num5 > numAllocatedZoneChecks)
					{
						zoneChecks = new StructsClass.Zone_Check[num5];
						for (int i = 0; i < num5; i++)
						{
							zoneChecks[i].numBoxChecks = 0;
							zoneChecks[i].numSphereChecks = 0;
							zoneChecks[i].numAllocatedBoxChecks = 0;
							zoneChecks[i].numAllocatedSphereChecks = 0;
							zoneChecks[i].type = 0;
							zoneChecks[i].zoneCheckID = 0;
							zoneChecks[i].objID = 0;
						}
						numAllocatedZoneChecks = (ushort)num5;
					}
					numZoneChecks = (ushort)num5;
					break;
				}
				case 2:
					num++;
					if (num < 0 || num >= numAllocatedZoneChecks)
					{
						num = -1;
					}
					break;
				case 3:
					if (array4.Length > 1 && num > -1)
					{
						zoneChecks[num].type = byte.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 4:
					if (array4.Length > 1 && num > -1)
					{
						zoneChecks[num].zoneCheckID = ushort.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 5:
					if (array4.Length > 1 && num > -1)
					{
						zoneChecks[num].objID = ushort.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 6:
				{
					if (array4.Length <= 1 || num <= -1)
					{
						break;
					}
					int num5 = int.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
					if (array4.Length > num5 * 4 + 1)
					{
						if (zoneChecks[num].numAllocatedSphereChecks < num5)
						{
							zoneChecks[num].Sphere = new float[num5 * 4];
							zoneChecks[num].numAllocatedSphereChecks = (byte)num5;
						}
						zoneChecks[num].numSphereChecks = (byte)num5;
						num5 *= 4;
						int i = 0;
						int num6 = 2;
						for (; i < num5; i++)
						{
							zoneChecks[num].Sphere[i++] = float.Parse(array4[num6++], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
							zoneChecks[num].Sphere[i++] = float.Parse(array4[num6++], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
							zoneChecks[num].Sphere[i++] = float.Parse(array4[num6++], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
							zoneChecks[num].Sphere[i] = float.Parse(array4[num6++], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
							zoneChecks[num].Sphere[i] = zoneChecks[num].Sphere[i] * zoneChecks[num].Sphere[i];
						}
					}
					break;
				}
				case 7:
				{
					if (array4.Length <= 1 || num <= -1)
					{
						break;
					}
					int num5 = int.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
					if (array4.Length > num5 * 6 + 1)
					{
						if (zoneChecks[num].numAllocatedBoxChecks < num5)
						{
							zoneChecks[num].Box = new float[num5 * 6];
							zoneChecks[num].numAllocatedBoxChecks = (byte)num5;
						}
						zoneChecks[num].numBoxChecks = (byte)num5;
						num5 *= 6;
						int i = 0;
						int num6 = 2;
						for (; i < num5; i++)
						{
							zoneChecks[num].Box[i] = float.Parse(array4[num6++], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
						}
					}
					break;
				}
				}
			}
		}
		stream.Close();
	}

	public void Set_Indexes_To_IDs()
	{
		for (ushort num = 0; num < numZoneChecks; num++)
		{
			zoneChecks[num].objIndex = 0;
			for (ushort num2 = 0; num2 < numZones; num2++)
			{
				if (zones[num2].zoneID == zoneChecks[num].objID)
				{
					zoneChecks[num].objIndex = num2;
				}
			}
		}
	}

	public bool Check_Zones_For_Point(float x, float y, float z, float distance, short start, byte type, out short returnValueZoneCheckIndex, out ushort returnValueZoneCheckObjID)
	{
		returnValueZoneCheckIndex = start;
		returnValueZoneCheckObjID = 0;
		for (short num = start; num < numZoneChecks; num++)
		{
			if (zoneChecks[num].type == type)
			{
				for (ushort num2 = 0; num2 < zoneChecks[num].numSphereChecks; num2++)
				{
					ushort num3 = (ushort)(num2 * 4);
					float num4 = zoneChecks[num].Sphere[num3++] - x;
					float num5 = zoneChecks[num].Sphere[num3++] - y;
					float num6 = zoneChecks[num].Sphere[num3++] - z;
					if (num4 * num4 + num5 * num5 + num6 * num6 < zoneChecks[num].Sphere[num3] + distance)
					{
						returnValueZoneCheckObjID = zoneChecks[num].objIndex;
						returnValueZoneCheckIndex = (short)(num + 1);
						return true;
					}
				}
				for (ushort num2 = 0; num2 < zoneChecks[num].numBoxChecks; num2++)
				{
					ushort num3 = (ushort)(num2 * 6);
					float num4;
					float num5;
					float num6;
					if ((num4 = x - zoneChecks[num].Box[num3++]) >= 0f && (num5 = y - zoneChecks[num].Box[num3++]) >= 0f && (num6 = z - zoneChecks[num].Box[num3++]) >= 0f && num4 <= zoneChecks[num].Box[num3++] && num5 <= zoneChecks[num].Box[num3++] && num6 <= zoneChecks[num].Box[num3])
					{
						returnValueZoneCheckObjID = zoneChecks[num].objIndex;
						returnValueZoneCheckIndex = (short)(num + 1);
						return true;
					}
				}
			}
		}
		return false;
	}

	public bool Check_Zones_For_Ray(float startX, float startY, float startZ, float endX, float endY, float endZ, float objectSize, short start, byte type, out short returnValueZoneCheckIndex, out ushort returnValueZoneCheckObjID)
	{
		float num = endX - startX;
		float num2 = endY - startY;
		float num3 = endZ - startZ;
		float num4 = (float)Math.Sqrt(num * num + num2 * num2 + num3 * num3);
		if (num4 != 0f)
		{
			num /= num4;
			num2 /= num4;
			num3 /= num4;
		}
		returnValueZoneCheckIndex = start;
		returnValueZoneCheckObjID = 0;
		for (short num5 = start; num5 < numZoneChecks; num5++)
		{
			if (zoneChecks[num5].type == type)
			{
				for (ushort num6 = 0; num6 < zoneChecks[num5].numBoxChecks; num6++)
				{
					ushort num7 = (ushort)(num6 * 6);
					if (mainC.utilMain.Does_Ray_Intersect_Box(zoneChecks[num5].Box[num7] - objectSize, zoneChecks[num5].Box[num7 + 1] - objectSize, zoneChecks[num5].Box[num7 + 2] - objectSize, zoneChecks[num5].Box[num7 + 3] + objectSize, zoneChecks[num5].Box[num7 + 4] + objectSize, zoneChecks[num5].Box[num7 + 5] + objectSize, startX, startY, startZ, num, num2, num3, num4))
					{
						returnValueZoneCheckObjID = zoneChecks[num5].objIndex;
						returnValueZoneCheckIndex = (short)(num5 + 1);
						return true;
					}
				}
			}
		}
		return false;
	}

	public bool Check_If_In_ZoneCheck(ushort b1, float x, float y, float z, float distance)
	{
		for (ushort num = 0; num < zoneChecks[b1].numSphereChecks; num++)
		{
			ushort num2 = (ushort)(num * 4);
			float num3 = zoneChecks[b1].Sphere[num2++] - x;
			float num4 = zoneChecks[b1].Sphere[num2++] - y;
			float num5 = zoneChecks[b1].Sphere[num2++] - z;
			if (num3 * num3 + num4 * num4 + num5 * num5 < zoneChecks[b1].Sphere[num2] + distance)
			{
				return true;
			}
		}
		for (ushort num = 0; num < zoneChecks[b1].numBoxChecks; num++)
		{
			ushort num2 = (ushort)(num * 6);
			float num3;
			float num4;
			float num5;
			if ((num3 = x - zoneChecks[b1].Box[num2++]) >= 0f && (num4 = y - zoneChecks[b1].Box[num2++]) >= 0f && (num5 = z - zoneChecks[b1].Box[num2++]) >= 0f && num3 <= zoneChecks[b1].Box[num2++] && num4 <= zoneChecks[b1].Box[num2++] && num5 <= zoneChecks[b1].Box[num2])
			{
				return true;
			}
		}
		return false;
	}

	public void Add_Box_Zone_Check(ushort zoneID, byte type, ushort objID, ushort objIndex, ushort zoneCheckID)
	{
		ushort num = (ushort)(numZoneChecks + 1);
		ushort num2;
		if (num > numAllocatedZoneChecks)
		{
			StructsClass.Zone_Check[] array = new StructsClass.Zone_Check[numAllocatedZoneChecks];
			for (num2 = 0; num2 < numAllocatedZoneChecks; num2++)
			{
				array[num2].type = zoneChecks[num2].type;
				array[num2].numBoxChecks = zoneChecks[num2].numBoxChecks;
				array[num2].numAllocatedBoxChecks = zoneChecks[num2].numAllocatedBoxChecks;
				array[num2].numSphereChecks = zoneChecks[num2].numSphereChecks;
				array[num2].numAllocatedSphereChecks = zoneChecks[num2].numAllocatedSphereChecks;
				array[num2].Box = new float[array[num2].numAllocatedBoxChecks * 6];
				array[num2].Sphere = new float[array[num2].numAllocatedSphereChecks * 4];
				array[num2].objID = zoneChecks[num2].objID;
				array[num2].objIndex = zoneChecks[num2].objIndex;
				array[num2].zoneCheckID = zoneChecks[num2].zoneCheckID;
				ushort num3 = (ushort)(array[num2].numBoxChecks * 6);
				for (ushort num4 = 0; num4 < num3; num4++)
				{
					array[num2].Box[num4] = zoneChecks[num2].Box[num4];
				}
				num3 = (ushort)(array[num2].numSphereChecks * 4);
				for (ushort num4 = 0; num4 < num3; num4++)
				{
					array[num2].Sphere[num4] = zoneChecks[num2].Sphere[num4];
				}
			}
			numAllocatedZoneChecks = (ushort)(num + 10);
			zoneChecks = new StructsClass.Zone_Check[numAllocatedZoneChecks];
			for (num2 = 0; num2 < numZoneChecks; num2++)
			{
				zoneChecks[num2].type = array[num2].type;
				zoneChecks[num2].numBoxChecks = array[num2].numBoxChecks;
				zoneChecks[num2].numAllocatedBoxChecks = array[num2].numAllocatedBoxChecks;
				zoneChecks[num2].numSphereChecks = array[num2].numSphereChecks;
				zoneChecks[num2].numAllocatedSphereChecks = array[num2].numAllocatedSphereChecks;
				zoneChecks[num2].Box = new float[zoneChecks[num2].numAllocatedBoxChecks * 6];
				zoneChecks[num2].Sphere = new float[zoneChecks[num2].numAllocatedSphereChecks * 4];
				zoneChecks[num2].objID = array[num2].objID;
				zoneChecks[num2].objIndex = array[num2].objIndex;
				zoneChecks[num2].zoneCheckID = array[num2].zoneCheckID;
				ushort num3 = (ushort)(zoneChecks[num2].numBoxChecks * 6);
				for (ushort num4 = 0; num4 < num3; num4++)
				{
					zoneChecks[num2].Box[num4] = array[num2].Box[num4];
				}
				num3 = (ushort)(zoneChecks[num2].numSphereChecks * 4);
				for (ushort num4 = 0; num4 < num3; num4++)
				{
					zoneChecks[num2].Sphere[num4] = array[num2].Sphere[num4];
				}
			}
			while (num2 < numAllocatedZoneChecks)
			{
				zoneChecks[num2].numAllocatedBoxChecks = 0;
				zoneChecks[num2].numAllocatedSphereChecks = 0;
				num2++;
			}
		}
		num2 = numZoneChecks;
		zoneChecks[num2].type = type;
		zoneChecks[num2].numBoxChecks = 1;
		if (zoneChecks[num2].numAllocatedBoxChecks < 1)
		{
			zoneChecks[num2].numAllocatedBoxChecks = 1;
			zoneChecks[num2].Box = new float[6];
		}
		zoneChecks[num2].numSphereChecks = 0;
		zoneChecks[num2].objID = objID;
		zoneChecks[num2].objIndex = 0;
		zoneChecks[num2].zoneCheckID = num2;
		for (ushort num4 = 0; num4 < numZones; num4++)
		{
			if (zones[num4].zoneID == objID)
			{
				zoneChecks[num2].objIndex = num4;
			}
		}
		numZoneChecks = num;
	}

	public void Add_CollisionModel_To_Zone(ushort zoneID, ushort objectID, ushort gid, ref Matrix mv1)
	{
		ushort num = 0;
		ushort num2 = 0;
		ushort num3 = 1;
		while (num2 < numZones)
		{
			if (zones[num2].zoneID == zoneID)
			{
				num = num2;
				num3 = 0;
				break;
			}
			num2++;
		}
		if (num3 == 1)
		{
			return;
		}
		num3 = zones[num].zoneList.numObjects;
		for (num2 = 0; num2 < num3; num2++)
		{
			if (zones[num].zoneList.gidList[num2] == gid && zones[num].zoneList.oList[num2] == objectID)
			{
				return;
			}
		}
		ushort num4 = (ushort)(zones[num].zoneList.numObjects + 1);
		if (num4 > zones[num].zoneList.numAllocatedObjects)
		{
			ushort numAllocatedObjects = zones[num].zoneList.numAllocatedObjects;
			ushort[] array = new ushort[numAllocatedObjects];
			ushort[] array2 = new ushort[numAllocatedObjects];
			Matrix[] array3 = new Matrix[numAllocatedObjects];
			for (num2 = 0; num2 < numAllocatedObjects; num2++)
			{
				array[num2] = zones[num].zoneList.oList[num2];
				array2[num2] = zones[num].zoneList.gidList[num2];
				ref Matrix reference = ref array3[num2];
				reference = zones[num].zoneList.matrixList[num2];
			}
			num3 = (ushort)(num4 + 10);
			zones[num].zoneList.numAllocatedObjects = num3;
			zones[num].zoneList.oList = new ushort[num3];
			zones[num].zoneList.gidList = new ushort[num3];
			zones[num].zoneList.matrixList = new Matrix[num3];
			for (num2 = 0; num2 < numAllocatedObjects; num2++)
			{
				zones[num].zoneList.oList[num2] = array[num2];
				zones[num].zoneList.gidList[num2] = array2[num2];
				ref Matrix reference2 = ref zones[num].zoneList.matrixList[num2];
				reference2 = array3[num2];
			}
		}
		num2 = zones[num].zoneList.numObjects;
		zones[num].zoneList.oList[num2] = objectID;
		zones[num].zoneList.gidList[num2] = gid;
		ref Matrix reference3 = ref zones[num].zoneList.matrixList[num2];
		reference3 = mv1;
		zones[num].zoneList.numObjects++;
	}

	public void Remove_CollisionModel_From_Zone(ushort zoneID, ushort gid)
	{
		ushort num = 0;
		ushort num2 = 0;
		ushort num3 = 1;
		while (num2 < numZones)
		{
			if (zones[num2].zoneID == zoneID)
			{
				num = num2;
				num3 = 0;
				break;
			}
			num2++;
		}
		if (num3 == 1)
		{
			return;
		}
		num3 = zones[num].zoneList.numObjects;
		for (num2 = 0; num2 < num3; num2++)
		{
			if (zones[num].zoneList.gidList[num2] == gid)
			{
				num3--;
				ushort num4 = (ushort)(num2 + 1);
				while (num2 < num3)
				{
					zones[num].zoneList.oList[num2] = zones[num].zoneList.oList[num4];
					zones[num].zoneList.gidList[num2] = zones[num].zoneList.gidList[num4];
					ref Matrix reference = ref zones[num].zoneList.matrixList[num2];
					reference = zones[num].zoneList.matrixList[num4];
					num2++;
					num4++;
				}
				zones[num].zoneList.numObjects--;
			}
		}
	}

	public void Scale_Zone_Check_Box(byte viewType, ushort zoneID, ushort boxID, float amount, float fx, float fy, float rx, float ry)
	{
		boxID = (ushort)(boxID * 3 + 3);
		ushort num = (ushort)(boxID + 1);
		ushort num2 = (ushort)(boxID + 2);
		if (viewType == 1)
		{
			zoneChecks[zoneID].Box[boxID] += amount * (fx + rx);
			zoneChecks[zoneID].Box[num] += amount * (fy + ry);
			if (zoneChecks[zoneID].Box[boxID] <= 0f)
			{
				zoneChecks[zoneID].Box[boxID] = 1f;
			}
			if (zoneChecks[zoneID].Box[num] <= 0f)
			{
				zoneChecks[zoneID].Box[num] = 1f;
			}
		}
		else
		{
			zoneChecks[zoneID].Box[num2] += amount;
			if (zoneChecks[zoneID].Box[num2] <= 0f)
			{
				zoneChecks[zoneID].Box[num2] = 1f;
			}
		}
	}
}
