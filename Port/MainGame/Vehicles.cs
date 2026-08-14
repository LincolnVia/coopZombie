using System;
using System.Globalization;
using System.IO;
using Collision;
using GameObjects;
using Joints;
using Microsoft.Xna.Framework;
using Models;
using Players;
using Rendering;
using Structs;
using Util;
using Weapons;
using WindowsGame1;

namespace MainGame;

public class Vehicles
{
	public static byte numVehicles = 0;

	public static byte numAllocatedVehicles = 0;

	public static byte[] lockedVehicleLevels;

	public static byte[] vehicelSelectVehicleIDs;

	public static StructsClass.Vehicle[] vehicles;

	public Game1.MasterCollection mainC;

	public void Init_Vars(Game1.MasterCollection master)
	{
		mainC = master;
	}

	public void Initialize_Vehicles()
	{
		Load_Vehicle_Data("Vehicles.txt");
	}

	public void Load_Vehicle_Data(string fileName)
	{
		int num = -1;
		_ = global::Rendering.Rendering.uBufferID;
		for (int i = 0; i < numVehicles; i++)
		{
			vehicles[i].numAlternateTextures = 0;
			vehicles[i].numWheels = 0;
			vehicles[i].numMounts = 0;
			vehicles[i].numWeaponMounts = 0;
			vehicles[i].numColPoints = 0;
			vehicles[i].ph1.momentInertiaAxisX = 1f;
			vehicles[i].ph1.momentInertiaAxisY = 1f;
			vehicles[i].ph1.momentInertiaAxisZ = 1f;
			vehicles[i].ph1.mass = 1f;
			vehicles[i].type = 0;
			vehicles[i].startX = 0f;
			vehicles[i].startY = 0f;
			vehicles[i].startZ = 0f;
			vehicles[i].startRotX = 0f;
			vehicles[i].startRotY = 0f;
			vehicles[i].startRotZ = 0f;
			vehicles[i].currentOccupants = 0;
			vehicles[i].maxOccupants = 1;
			vehicles[i].playerIDs = new ushort[1] { global::Util.Util.maxUnsignedShortValue };
			for (int j = 0; j < vehicles[i].numModels; j++)
			{
				vehicles[i].textureID[j] = -1;
			}
		}
		Stream stream = TitleContainer.OpenStream("The_CoOp_Zombie_Game\\Config_Files\\" + fileName);
		byte[] array = new byte[stream.Length];
		if (stream.CanRead)
		{
			stream.Read(array, 0, array.Length);
			string text = mainC.utilMain.Byte_Array_To_String(array);
			string[] array2 = text.Split('\n', '\r');
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
				stream.Close();
				return;
			}
			string[] array3 = new string[num2];
			k = 0;
			num2 = 0;
			for (; k < array2.Length; k++)
			{
				if (array2[k].Length > 0)
				{
					array3[num2++] = array2[k];
				}
			}
			for (k = 0; k < num2; k++)
			{
				array2 = array3[k].Split(' ', '\t');
				int l = 0;
				int num3 = 0;
				for (; l < array2.Length; l++)
				{
					if (array2[l].Length > 0)
					{
						num3++;
					}
				}
				if (num3 < 1)
				{
					continue;
				}
				string[] array4 = new string[num3];
				l = 0;
				num3 = 0;
				for (; l < array2.Length; l++)
				{
					if (array2[l].Length > 0)
					{
						array4[num3++] = array2[l];
					}
				}
				int num4 = 0;
				if (array4[0].Equals("numVehicles", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 1;
				}
				else if (array4[0].Equals("Vehicle", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 2;
				}
				else if (array4[0].Equals("numWheels", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 3;
				}
				else if (array4[0].Equals("numColPoints", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 4;
				}
				else if (array4[0].Equals("driveWheels", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 5;
				}
				else if (array4[0].Equals("maxWheelRot", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 6;
				}
				else if (array4[0].Equals("wheelPositions", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 7;
				}
				else if (array4[0].Equals("wheelColPoints", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 8;
				}
				else if (array4[0].Equals("wheelRotationMultiplier", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 9;
				}
				else if (array4[0].Equals("wheelAttachPoint", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 10;
				}
				else if (array4[0].Equals("momentInertiaX", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 11;
				}
				else if (array4[0].Equals("momentInertiaY", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 12;
				}
				else if (array4[0].Equals("momentInertiaZ", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 13;
				}
				else if (array4[0].Equals("mass", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 14;
				}
				else if (array4[0].Equals("data", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 15;
				}
				else if (array4[0].Equals("numMounts", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 16;
				}
				else if (array4[0].Equals("model", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 17;
				}
				else if (array4[0].Equals("type", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 18;
				}
				else if (array4[0].Equals("startPosition", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 19;
				}
				else if (array4[0].Equals("startRotation", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 20;
				}
				else if (array4[0].Equals("maxOccupants", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 21;
				}
				else if (array4[0].Equals("mount", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 22;
				}
				else if (array4[0].Equals("Texture", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 23;
				}
				else if (array4[0].Equals("Model_List", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 24;
				}
				else if (array4[0].Equals("numWeaponMounts", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 25;
				}
				else if (array4[0].Equals("explosiveDamage", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 26;
				}
				else if (array4[0].Equals("maxDamage", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 27;
				}
				else if (array4[0].Equals("accelerationFactor", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 28;
				}
				else if (array4[0].Equals("ControllerSpring", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 29;
				}
				else if (array4[0].Equals("ControllerDampening", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 30;
				}
				else if (array4[0].Equals("damageThresholdForExplosion", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 31;
				}
				else if (array4[0].Equals("damageParticles", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 32;
				}
				else if (array4[0].Equals("heatParameters", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 33;
				}
				else if (array4[0].Equals("weaponCapacity", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 34;
				}
				else if (array4[0].Equals("alternateTextures", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 35;
				}
				switch (num4)
				{
				case 1:
				{
					if (array4.Length <= 1)
					{
						break;
					}
					int j = int.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
					if (j > numAllocatedVehicles)
					{
						vehicles = new StructsClass.Vehicle[j];
						for (int i = 0; i < j; i++)
						{
							vehicles[i].numAlternateTextures = 0;
							vehicles[i].numWheels = 0;
							vehicles[i].numColPoints = 0;
							vehicles[i].numMounts = 0;
							vehicles[i].numWeaponMounts = 0;
							vehicles[i].mv = new Matrix[2];
							ref Matrix reference5 = ref vehicles[i].mv[0];
							reference5 = Matrix.Identity;
							ref Matrix reference6 = ref vehicles[i].mv[1];
							reference6 = Matrix.Identity;
							vehicles[i].ph1.momentInertiaAxisX = 1f;
							vehicles[i].ph1.momentInertiaAxisY = 1f;
							vehicles[i].ph1.momentInertiaAxisZ = 1f;
							vehicles[i].ph1.mass = 1f;
							vehicles[i].type = 0;
							vehicles[i].startX = 0f;
							vehicles[i].startY = 0f;
							vehicles[i].startZ = 0f;
							vehicles[i].startRotX = 0f;
							vehicles[i].startRotY = 0f;
							vehicles[i].startRotZ = 0f;
							vehicles[i].currentOccupants = 0;
							vehicles[i].maxOccupants = 1;
							vehicles[i].playerIDs = new ushort[1] { global::Util.Util.maxUnsignedShortValue };
							vehicles[i].mainModel.numModels = 0;
							vehicles[i].explosionDamage = 150f;
							vehicles[i].explosionImpactForce = 10f;
							vehicles[i].splashDamageFalloff = 50f;
							vehicles[i].maxDamage = 100f;
							vehicles[i].damageThresholdForExplosion = -1f;
							vehicles[i].damageParticleX = 0f;
							vehicles[i].damageParticleY = 0f;
							vehicles[i].damageParticleZ = 0f;
							vehicles[i].maxHeat = 10;
							vehicles[i].curHeat = 0f;
							vehicles[i].heatGeneration = 0f;
							vehicles[i].heatDissipation = 1f;
							vehicles[i].overHeatingDamage = 0f;
							vehicles[i].numWeapons = 0;
						}
						numAllocatedVehicles = (byte)j;
					}
					numVehicles = (byte)j;
					break;
				}
				case 2:
					if (array4.Length > 1)
					{
						num = int.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
						if (num < 0 || num >= numAllocatedVehicles)
						{
							num = -1;
						}
					}
					break;
				case 3:
					if (array4.Length > 1 && num > -1)
					{
						byte b = byte.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
						vehicles[num].numWheels = b;
						vehicles[num].driveWheel = new bool[b];
						vehicles[num].wheelRot = new float[b];
						vehicles[num].maxWheelRot = new float[b];
						vehicles[num].wheelRotationMultiplier = new float[b];
						vehicles[num].wheelColPoints = new float[b * 3];
						vehicles[num].wheelAttachPoint = new float[b * 3];
						vehicles[num].wheelColPointsRadiusAxisZ = new float[b];
						vehicles[num].wheelsMatrix = new Matrix[b];
						int j = 0;
						int i = 0;
						for (; j < b; j++)
						{
							vehicles[num].driveWheel[j] = false;
							vehicles[num].wheelRot[j] = 0f;
							vehicles[num].maxWheelRot[j] = 0f;
							vehicles[num].wheelRotationMultiplier[j] = 1f;
							vehicles[num].wheelColPoints[i] = 0f;
							vehicles[num].wheelAttachPoint[i++] = 0f;
							vehicles[num].wheelColPoints[i] = 0f;
							vehicles[num].wheelAttachPoint[i++] = 0f;
							vehicles[num].wheelColPoints[i] = 0f;
							vehicles[num].wheelAttachPoint[i++] = 0f;
							ref Matrix reference4 = ref vehicles[num].wheelsMatrix[j];
							reference4 = Matrix.Identity;
						}
					}
					break;
				case 4:
				{
					if (array4.Length <= 1 || num <= -1)
					{
						break;
					}
					short num5 = short.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
					if (array4.Length > num5 * 3 + 1)
					{
						vehicles[num].numColPoints = num5;
						num5 *= 3;
						vehicles[num].colPoints = new float[num5];
						int j = 0;
						int i = 2;
						while (j < num5)
						{
							vehicles[num].colPoints[j++] = float.Parse(array4[i++], CultureInfo.InvariantCulture.NumberFormat);
							vehicles[num].colPoints[j++] = float.Parse(array4[i++], CultureInfo.InvariantCulture.NumberFormat);
							vehicles[num].colPoints[j++] = float.Parse(array4[i++], CultureInfo.InvariantCulture.NumberFormat);
						}
					}
					break;
				}
				case 5:
				{
					if (array4.Length <= 1 || num <= -1 || array4.Length <= vehicles[num].numWheels)
					{
						break;
					}
					int i = 0;
					int j = 1;
					for (; i < vehicles[num].numWheels; i++)
					{
						byte b = byte.Parse(array4[j++], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
						vehicles[num].driveWheel[i] = false;
						if (b == 1)
						{
							vehicles[num].driveWheel[i] = true;
						}
					}
					break;
				}
				case 6:
					if (array4.Length > 1 && num > -1 && array4.Length > vehicles[num].numWheels)
					{
						int i = 0;
						int j = 1;
						for (; i < vehicles[num].numWheels; i++)
						{
							vehicles[num].maxWheelRot[i] = float.Parse(array4[j++], CultureInfo.InvariantCulture.NumberFormat);
						}
					}
					break;
				case 7:
					if (array4.Length > 1 && num > -1 && array4.Length > vehicles[num].numWheels * 6)
					{
						int i = 0;
						int j = 1;
						for (; i < vehicles[num].numWheels; i++)
						{
							float xPosition = float.Parse(array4[j++], CultureInfo.InvariantCulture.NumberFormat);
							float yPosition = float.Parse(array4[j++], CultureInfo.InvariantCulture.NumberFormat);
							float zPosition = float.Parse(array4[j++], CultureInfo.InvariantCulture.NumberFormat);
							float num7 = float.Parse(array4[j++], CultureInfo.InvariantCulture.NumberFormat);
							float num8 = float.Parse(array4[j++], CultureInfo.InvariantCulture.NumberFormat);
							float num9 = float.Parse(array4[j++], CultureInfo.InvariantCulture.NumberFormat);
							ref Matrix reference3 = ref vehicles[num].wheelsMatrix[i];
							reference3 = Matrix.CreateRotationZ(num9 * 57.29578f) * Matrix.CreateRotationY(num8 * 57.29578f) * Matrix.CreateRotationX(num7 * 57.29578f) * Matrix.CreateTranslation(xPosition, yPosition, zPosition);
						}
					}
					break;
				case 8:
				{
					if (array4.Length <= 1 || num <= -1)
					{
						break;
					}
					int j = vehicles[num].numWheels * 3;
					if (array4.Length > j)
					{
						int i = 0;
						int num11 = 1;
						for (; i < j; i++)
						{
							vehicles[num].wheelColPoints[i] = float.Parse(array4[num11++], CultureInfo.InvariantCulture.NumberFormat);
						}
					}
					break;
				}
				case 9:
					if (array4.Length > 1 && num > -1 && array4.Length > vehicles[num].numWheels)
					{
						int i = 0;
						int j = 1;
						for (; i < vehicles[num].numWheels; i++)
						{
							vehicles[num].wheelRotationMultiplier[i] = float.Parse(array4[j++], CultureInfo.InvariantCulture.NumberFormat);
						}
					}
					break;
				case 10:
					if (array4.Length > 1 && num > -1 && array4.Length > vehicles[num].numWheels * 3)
					{
						int i = 0;
						int j = 1;
						int num10 = 0;
						for (; i < vehicles[num].numWheels; i++)
						{
							vehicles[num].wheelAttachPoint[num10++] = float.Parse(array4[j++], CultureInfo.InvariantCulture.NumberFormat);
							vehicles[num].wheelAttachPoint[num10++] = float.Parse(array4[j++], CultureInfo.InvariantCulture.NumberFormat);
							vehicles[num].wheelAttachPoint[num10++] = float.Parse(array4[j++], CultureInfo.InvariantCulture.NumberFormat);
						}
					}
					break;
				case 11:
					if (array4.Length > 1 && num > -1)
					{
						vehicles[num].ph1.momentInertiaAxisX = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 12:
					if (array4.Length > 1 && num > -1)
					{
						vehicles[num].ph1.momentInertiaAxisY = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 13:
					if (array4.Length > 1 && num > -1)
					{
						vehicles[num].ph1.momentInertiaAxisZ = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 14:
					if (array4.Length > 1 && num > -1)
					{
						vehicles[num].ph1.mass = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 15:
				{
					if (array4.Length <= 1 || num <= -1)
					{
						break;
					}
					int i = int.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
					if (array4.Length <= i + 1)
					{
						break;
					}
					for (int j = 0; j < i; j++)
					{
						switch (j)
						{
						case 0:
							vehicles[num].data1 = float.Parse(array4[2], CultureInfo.InvariantCulture.NumberFormat);
							break;
						case 1:
							vehicles[num].data2 = float.Parse(array4[3], CultureInfo.InvariantCulture.NumberFormat);
							break;
						case 2:
							vehicles[num].data3 = float.Parse(array4[4], CultureInfo.InvariantCulture.NumberFormat);
							break;
						case 3:
							vehicles[num].data4 = float.Parse(array4[5], CultureInfo.InvariantCulture.NumberFormat);
							break;
						case 4:
							vehicles[num].data5 = float.Parse(array4[6], CultureInfo.InvariantCulture.NumberFormat);
							break;
						case 5:
							vehicles[num].data6 = float.Parse(array4[7], CultureInfo.InvariantCulture.NumberFormat);
							break;
						case 6:
							vehicles[num].data7 = float.Parse(array4[8], CultureInfo.InvariantCulture.NumberFormat);
							break;
						case 7:
							vehicles[num].data8 = float.Parse(array4[9], CultureInfo.InvariantCulture.NumberFormat);
							break;
						case 8:
							vehicles[num].data9 = float.Parse(array4[10], CultureInfo.InvariantCulture.NumberFormat);
							break;
						case 9:
							vehicles[num].data10 = float.Parse(array4[11], CultureInfo.InvariantCulture.NumberFormat);
							break;
						case 10:
							vehicles[num].data11 = float.Parse(array4[12], CultureInfo.InvariantCulture.NumberFormat);
							break;
						case 11:
							vehicles[num].data12 = float.Parse(array4[13], CultureInfo.InvariantCulture.NumberFormat);
							break;
						case 12:
							vehicles[num].data13 = float.Parse(array4[14], CultureInfo.InvariantCulture.NumberFormat);
							break;
						case 13:
							vehicles[num].data14 = float.Parse(array4[15], CultureInfo.InvariantCulture.NumberFormat);
							break;
						case 14:
							vehicles[num].data15 = float.Parse(array4[16], CultureInfo.InvariantCulture.NumberFormat);
							break;
						case 15:
							vehicles[num].data16 = float.Parse(array4[17], CultureInfo.InvariantCulture.NumberFormat);
							break;
						}
					}
					break;
				}
				case 16:
				{
					if (array4.Length <= 1 || num <= -1)
					{
						break;
					}
					short num5 = short.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
					if (array4.Length > 0)
					{
						vehicles[num].numMounts = (byte)num5;
						vehicles[num].mounts = new StructsClass.Mounting_Point_Fixed[num5];
						for (int j = 0; j < num5; j++)
						{
							vehicles[num].mounts[j] = default(StructsClass.Mounting_Point_Fixed);
							vehicles[num].mounts[j].type = 0;
							vehicles[num].mounts[j].objectAttached = 0;
							vehicles[num].mounts[j].itemPlacmentMatrixID = 0;
							vehicles[num].mounts[j].mvCurrent = new Matrix[2];
						}
					}
					break;
				}
				case 17:
					if (array4.Length > 1 && num > -1)
					{
						int j = array4.Length - 1;
						vehicles[num].modelName = new string[j];
						vehicles[num].numModels = (byte)j;
						vehicles[num].vehicleModel = new short[j];
						vehicles[num].textureID = new short[j];
						int i = 0;
						while (i < j)
						{
							vehicles[num].textureID[i] = -1;
							vehicles[num].modelName[i] = array4[++i];
						}
					}
					break;
				case 18:
					if (array4.Length > 1 && num > -1)
					{
						vehicles[num].type = byte.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 19:
					if (array4.Length > 3 && num > -1)
					{
						vehicles[num].startX = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
						vehicles[num].startY = float.Parse(array4[2], CultureInfo.InvariantCulture.NumberFormat);
						vehicles[num].startZ = float.Parse(array4[3], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 20:
					if (array4.Length > 3 && num > -1)
					{
						vehicles[num].startRotX = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
						vehicles[num].startRotY = float.Parse(array4[2], CultureInfo.InvariantCulture.NumberFormat);
						vehicles[num].startRotZ = float.Parse(array4[3], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 21:
					if (array4.Length > 1 && num > -1)
					{
						ushort num6 = ushort.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
						vehicles[num].maxOccupants = num6;
						vehicles[num].playerIDs = new ushort[num6];
						for (int i = 0; i < num6; i++)
						{
							vehicles[num].playerIDs[i] = global::Util.Util.maxUnsignedShortValue;
						}
					}
					break;
				case 22:
					if (array4.Length > 10 && num > -1)
					{
						ushort num6 = ushort.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
						if (num6 < vehicles[num].numMounts)
						{
							vehicles[num].mounts[num6].type = byte.Parse(array4[2], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
							vehicles[num].mounts[num6].objectID = byte.Parse(array4[3], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
							vehicles[num].mounts[num6].objectAttached = byte.Parse(array4[4], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
							vehicles[num].mounts[num6].jointID = byte.Parse(array4[5], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
							vehicles[num].mounts[num6].itemPlacmentMatrixID = byte.Parse(array4[6], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
							vehicles[num].mounts[num6].mvStart = Matrix.CreateRotationY(float.Parse(array4[11], CultureInfo.InvariantCulture.NumberFormat) * ((float)Math.PI / 180f)) * Matrix.CreateRotationX(float.Parse(array4[10], CultureInfo.InvariantCulture.NumberFormat) * ((float)Math.PI / 180f)) * Matrix.CreateRotationZ(float.Parse(array4[12], CultureInfo.InvariantCulture.NumberFormat) * ((float)Math.PI / 180f)) * Matrix.CreateTranslation(float.Parse(array4[7], CultureInfo.InvariantCulture.NumberFormat), float.Parse(array4[8], CultureInfo.InvariantCulture.NumberFormat), float.Parse(array4[9], CultureInfo.InvariantCulture.NumberFormat));
							ref Matrix reference = ref vehicles[num].mounts[num6].mvCurrent[0];
							reference = vehicles[num].mounts[num6].mvStart;
							ref Matrix reference2 = ref vehicles[num].mounts[num6].mvCurrent[1];
							reference2 = vehicles[num].mounts[num6].mvStart;
						}
					}
					break;
				case 23:
					if (array4.Length > 1 && num > -1)
					{
						int j = array4.Length - 1;
						vehicles[num].textureID = new short[j];
						for (int i = 0; i < j; i++)
						{
							vehicles[num].textureID[i] = mainC.texturesMain.Find_Texture(array4[i + 1], -1);
						}
					}
					break;
				case 24:
					if (array4.Length > 1 && num > -1)
					{
						mainC.modelsMain.Load_Model_List(ref vehicles[num].mainModel, array4[1]);
					}
					break;
				case 25:
				{
					if (array4.Length <= 1 || num <= -1)
					{
						break;
					}
					short num5 = short.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
					if (array4.Length > num5 + 1)
					{
						vehicles[num].numWeaponMounts = (byte)num5;
						vehicles[num].weaponMounts = new ushort[num5];
						int j = 0;
						int i = 2;
						while (j < num5)
						{
							vehicles[num].weaponMounts[j++] = ushort.Parse(array4[i++], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
						}
					}
					break;
				}
				case 26:
					if (array4.Length > 1 && num > -1)
					{
						vehicles[num].explosionDamage = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 27:
					if (array4.Length > 1 && num > -1)
					{
						vehicles[num].maxDamage = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 28:
					if (array4.Length > 1 && num > -1)
					{
						vehicles[num].accelerationFactor = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 29:
					if (array4.Length > 1 && num > -1)
					{
						vehicles[num].controllerSpring = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 30:
					if (array4.Length > 1 && num > -1)
					{
						vehicles[num].controllerDampening = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 31:
					if (array4.Length > 1 && num > -1)
					{
						vehicles[num].damageThresholdForExplosion = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 32:
					if (array4.Length > 3 && num > -1)
					{
						vehicles[num].damageParticleX = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
						vehicles[num].damageParticleY = float.Parse(array4[2], CultureInfo.InvariantCulture.NumberFormat);
						vehicles[num].damageParticleZ = float.Parse(array4[3], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 33:
					if (array4.Length > 4 && num > -1)
					{
						vehicles[num].maxHeat = byte.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
						vehicles[num].heatGeneration = float.Parse(array4[2], CultureInfo.InvariantCulture.NumberFormat);
						vehicles[num].heatDissipation = float.Parse(array4[3], CultureInfo.InvariantCulture.NumberFormat);
						vehicles[num].overHeatingDamage = float.Parse(array4[4], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 34:
					if (array4.Length > 1 && num > -1)
					{
						vehicles[num].numWeapons = byte.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 35:
					if (array4.Length > 1 && num > -1)
					{
						vehicles[num].numAlternateTextures = (byte)(array4.Length - 1);
						vehicles[num].alternateTextureIDs = new ushort[vehicles[num].numAlternateTextures];
						int i = 0;
						int j = 1;
						while (i < vehicles[num].numAlternateTextures)
						{
							vehicles[num].alternateTextureIDs[i] = (ushort)mainC.texturesMain.Find_Texture(array4[j], 0);
							i++;
							j++;
						}
					}
					break;
				}
			}
		}
		stream.Close();
		for (int i = 0; i < numVehicles; i++)
		{
			if (vehicles[i].damageThresholdForExplosion == -1f)
			{
				vehicles[i].damageThresholdForExplosion = vehicles[i].maxDamage * 2f;
			}
			int num11 = vehicles[i].numMounts;
			int num10 = 0;
			int j;
			for (j = 0; j < num11; j++)
			{
				if (vehicles[i].mounts[j].type == 1)
				{
					num10++;
				}
			}
			if (num10 > vehicles[i].numWeapons)
			{
				vehicles[i].numWeapons = (byte)num10;
			}
			else
			{
				num10 = vehicles[i].numWeapons;
			}
			if (num10 > 0)
			{
				vehicles[i].weapons = new StructsClass.weapon_stub[num10];
				for (j = 0; j < num10; j++)
				{
					vehicles[i].weapons[j] = default(StructsClass.weapon_stub);
					StructsClass.Initialize_Weapon_Stub(ref vehicles[i].weapons[j]);
				}
			}
			j = 0;
			num10 = 0;
			for (; j < num11; j++)
			{
				if (vehicles[i].mounts[j].type == 1)
				{
					vehicles[i].mounts[j].objectID = (byte)num10++;
				}
			}
			num11 = vehicles[i].numWheels + vehicles[i].numColPoints;
			vehicles[i].momentum = default(StructsClass.Momentum);
			vehicles[i].momentum.numPoints = (short)num11;
			vehicles[i].momentum.collisionPoints = new float[num11 * 3];
			j = num11 * 3;
			vehicles[i].momentum.buffimpactForceId = new short[1000];
			j = num11 * 9;
			vehicles[i].momentum.buffForceValue = new float[3000];
			ref Matrix reference7 = ref vehicles[i].mv[0];
			reference7 = Matrix.CreateRotationY(vehicles[i].startRotY * ((float)Math.PI / 180f)) * Matrix.CreateRotationX(vehicles[i].startRotX * ((float)Math.PI / 180f)) * Matrix.CreateRotationZ(vehicles[i].startRotZ * ((float)Math.PI / 180f));
			ref Matrix reference8 = ref vehicles[i].mv[1];
			reference8 = vehicles[i].mv[0];
			vehicles[i].ph1.x = vehicles[i].startX;
			vehicles[i].ph1.y = vehicles[i].startY;
			vehicles[i].ph1.z = vehicles[i].startZ;
			j = 0;
			num11 = 0;
			while (j < vehicles[i].numWheels)
			{
				int num12 = num11 + 1;
				int num13 = num11 + 2;
				Matrix matrix = Matrix.CreateTranslation(vehicles[i].wheelColPoints[num11], vehicles[i].wheelColPoints[num12], vehicles[i].wheelColPoints[num13]) * vehicles[i].wheelsMatrix[j] * Matrix.CreateTranslation(vehicles[i].wheelAttachPoint[num11], vehicles[i].wheelAttachPoint[num12], vehicles[i].wheelAttachPoint[num13]);
				float num7 = matrix.M41;
				float num8 = matrix.M42;
				float num9 = matrix.M43;
				vehicles[i].wheelColPointsRadiusAxisZ[j] = (float)Math.Sqrt(num7 * num7 + num8 * num8);
				if (vehicles[i].wheelColPointsRadiusAxisZ[j] == 0f)
				{
					vehicles[i].wheelColPointsRadiusAxisZ[j] = 0.0001f;
				}
				if (num7 == 0f && num8 == 0f && num9 == 0f)
				{
					num7 = 0.0001f;
				}
				vehicles[i].momentum.collisionPoints[num11] = num7;
				vehicles[i].momentum.collisionPoints[num12] = num8;
				vehicles[i].momentum.collisionPoints[num13] = num9;
				j++;
				num11 += 3;
			}
			j = 0;
			num10 = 0;
			for (; j < vehicles[i].numColPoints; j++)
			{
				if (vehicles[i].colPoints[num10] == 0f && vehicles[i].colPoints[num10 + 1] == 0f && vehicles[i].colPoints[num10 + 2] == 0f)
				{
					vehicles[i].colPoints[num10] = 0.0001f;
				}
				vehicles[i].momentum.collisionPoints[num11++] = vehicles[i].colPoints[num10++];
				vehicles[i].momentum.collisionPoints[num11++] = vehicles[i].colPoints[num10++];
				vehicles[i].momentum.collisionPoints[num11++] = vehicles[i].colPoints[num10++];
			}
		}
		mainC.gameLogic.Game_Initialize_Vehicle_Data();
	}

	public void Render_Vehicles()
	{
		byte rBufferID = global::Rendering.Rendering.rBufferID;
		for (ushort num = 0; num < numVehicles; num++)
		{
			byte type = vehicles[num].type;
			if (type == 3)
			{
				Matrix mv = vehicles[num].mv[rBufferID] * Matrix.CreateTranslation(vehicles[num].ph1.x, vehicles[num].ph1.y, vehicles[num].ph1.z);
				ushort num2 = 0;
				ushort numModels = vehicles[num].numModels;
				while (num2 < numModels)
				{
					mainC.modelsMain.Render_Model(vehicles[num].vehicleModel[num2], ref mv);
					num2++;
				}
			}
		}
		for (ushort num = 0; num < MainGame.maxGamePlayers; num++)
		{
			if ((global::Players.Players.players[num].onmap & 0xC) > 0)
			{
				ushort numMounts = MainGame.playerVehicles[num].numMounts;
				ushort numModels = (ushort)global::Joints.Joints.playerJoints[global::Players.Players.players[num].jointPackage].numJoints;
				for (ushort num2 = 0; num2 < numMounts; num2++)
				{
					if (MainGame.playerVehicles[num].mounts[num2].type == 2 && MainGame.playerVehicles[num].mounts[num2].objectAttached == 1)
					{
						Matrix mv = MainGame.playerVehicles[num].mounts[num2].mvCurrent[global::Rendering.Rendering.rBufferID] * Matrix.CreateTranslation(global::Players.Players.players[num].posX[global::Rendering.Rendering.rBufferID], global::Players.Players.players[num].posY[global::Rendering.Rendering.rBufferID], global::Players.Players.players[num].posZ[global::Rendering.Rendering.rBufferID]);
						mainC.modelsMain.Render_Model(MainGame.playerVehicles[num].mounts[num2].objectID, ref mv);
					}
				}
			}
		}
	}

	public void Place_Object_In_Player_Vehicle_Mount(ushort playerID, ushort mountID, byte objectID)
	{
		MainGame.playerVehicles[playerID].mounts[mountID].objectID = objectID;
		MainGame.playerVehicles[playerID].mounts[mountID].objectAttached = 1;
	}

	public void Remove_Object_In_Player_Vehicle_Mount(ushort playerID, ushort mountID)
	{
		MainGame.playerVehicles[playerID].mounts[mountID].objectAttached = 0;
	}

	public void Update_Vehicle_Matrix(ushort playerID)
	{
		if (MainGame.playerVehicles[playerID].type == 8)
		{
			return;
		}
		_ = global::Players.Players.players[playerID].curVehicle;
		ushort num = (ushort)global::Joints.Joints.playerJoints[global::Players.Players.players[playerID].jointPackage].numJoints;
		ushort numMounts = MainGame.playerVehicles[playerID].numMounts;
		for (ushort num2 = 0; num2 < numMounts; num2++)
		{
			if (MainGame.playerVehicles[playerID].mounts[num2].jointID < num)
			{
				ref Matrix reference = ref MainGame.playerVehicles[playerID].mounts[num2].mvCurrent[global::Rendering.Rendering.uBufferID];
				reference = MainGame.itemPlacementMatrix[MainGame.playerVehicles[playerID].mounts[num2].itemPlacmentMatrixID] * MainGame.playerVehicles[playerID].mounts[num2].mvStart * global::Players.Players.players[playerID].jt1[MainGame.playerVehicles[playerID].mounts[num2].jointID].mv[global::Rendering.Rendering.uBufferID];
			}
			else
			{
				ref Matrix reference2 = ref MainGame.playerVehicles[playerID].mounts[num2].mvCurrent[global::Rendering.Rendering.uBufferID];
				reference2 = MainGame.playerVehicles[playerID].mounts[num2].mvStart * MainGame.playerVehicles[playerID].mv[global::Rendering.Rendering.uBufferID];
			}
		}
	}

	public void Update_Vehicle_Avatar_Matrix(ushort playerID)
	{
	}

	public void Set_Avatar_Matrix_For_Vehicle_Select(ushort playerID, ref Matrix mv1)
	{
	}

	public void Calculate_Vehicle_Collision_Data(ref StructsClass.Vehicle vh1, ushort vhID, byte ubID, byte threadID)
	{
		ushort num = 0;
		float num2 = 0.33f;
		float num3 = 0f;
		float num4 = 0f;
		float num5 = 0f;
		float num6 = 0f;
		float num7 = 0f;
		float num8 = 0f;
		float num9 = 0f;
		float num10 = 0f;
		float num11 = 0f;
		_ = vehicles[vhID].numWheels;
		StructsClass.Momentum momentum = vehicles[vhID].momentum;
		short numPoints = momentum.numPoints;
		Matrix matrix = Matrix.Invert(vh1.mv[ubID]);
		momentum.forceX = 0f;
		momentum.forceY = 0f;
		momentum.forceZ = 0f;
		momentum.countForces = 0;
		int i;
		int num16;
		for (i = 0; i < numPoints; i++)
		{
			if (global::Collision.Collision.floatArStatus[threadID, i] == 1)
			{
				int num12 = momentum.countForces * 3;
				momentum.buffForceValue[num12++] = 0f;
				momentum.buffForceValue[num12++] = 0f;
				momentum.buffForceValue[num12] = 0f;
				float num13 = global::Collision.Collision.floatArDir[threadID, i, 0];
				float num14 = global::Collision.Collision.floatArDir[threadID, i, 1];
				float num15 = global::Collision.Collision.floatArDir[threadID, i, 2];
				momentum.forceX += Math.Abs(num13);
				momentum.forceY += Math.Abs(num14);
				momentum.forceZ += Math.Abs(num15);
				num16 = momentum.countForces * 3;
				momentum.buffForceValue[num16++] = num13;
				momentum.buffForceValue[num16++] = num14;
				momentum.buffForceValue[num16] = num15;
				momentum.buffimpactForceId[momentum.countForces] = (short)i;
				momentum.countForces++;
			}
		}
		if (momentum.countForces < 1)
		{
			return;
		}
		i = 0;
		num16 = 0;
		for (; i < momentum.countForces; i++)
		{
			num16 = i * 3;
			int num17 = momentum.buffimpactForceId[i] * 3;
			int num18 = num17 + 129;
			float num19 = global::Collision.Collision.floatAr[threadID, num18];
			float num20 = global::Collision.Collision.floatAr[threadID, num18 + 1];
			float num21 = global::Collision.Collision.floatAr[threadID, num18 + 2];
			float num22 = momentum.buffForceValue[num16];
			float num23 = momentum.buffForceValue[num16 + 1];
			float num24 = momentum.buffForceValue[num16 + 2];
			num3 = num20 * num24 - num21 * num23;
			num4 = num21 * num22 - num19 * num24;
			num5 = num19 * num23 - num20 * num22;
			float num13 = (float)Math.Sqrt(num3 * num3 + num4 * num4 + num5 * num5);
			float num25 = 0f;
			float num26 = 0f;
			float num27 = 0f;
			if (num13 != 0f)
			{
				num25 = num3 / num13;
				num26 = num4 / num13;
				num27 = num5 / num13;
			}
			num3 = 0f - (num20 * num27 - num21 * num26);
			num4 = 0f - (num21 * num25 - num19 * num27);
			num5 = 0f - (num19 * num26 - num20 * num25);
			num13 = num3 * num22 + num4 * num23 + num5 * num24;
			num13 = 1f / vh1.ph1.mass + num13 * num13 / vh1.ph1.momentInertiaAxisX;
			num25 = vh1.ph1.velocityX;
			num26 = vh1.ph1.velocityY;
			num27 = vh1.ph1.velocityZ;
			float num28 = vh1.momentum.collisionPoints[num17];
			float num29 = vh1.momentum.collisionPoints[num17 + 2];
			float num14 = 0f - vh1.ph1.angularVelocityY * (float)Math.Sqrt(num28 * num28 + num29 * num29);
			float num15;
			if (num28 == 0f)
			{
				num15 = (float)Math.PI / 2f;
				if (num29 < 0f)
				{
					num15 *= -1f;
				}
			}
			else
			{
				num15 = (float)Math.Atan(num29 / num28);
				if (num28 < 0f)
				{
					num15 += (float)Math.PI;
				}
			}
			float num30 = (0f - (float)Math.Sin(num15)) * num14;
			float num31 = (float)Math.Cos(num15) * num14;
			num25 += num30 * vh1.mv[ubID].M11 + num31 * vh1.mv[ubID].M31;
			num26 += num30 * vh1.mv[ubID].M12 + num31 * vh1.mv[ubID].M32;
			num27 += num30 * vh1.mv[ubID].M13 + num31 * vh1.mv[ubID].M33;
			num28 = vh1.momentum.collisionPoints[num17 + 1];
			num29 = vh1.momentum.collisionPoints[num17 + 2];
			num14 = vh1.ph1.angularVelocityX * (float)Math.Sqrt(num28 * num28 + num29 * num29);
			if (num28 == 0f)
			{
				num15 = (float)Math.PI / 2f;
				if (num29 < 0f)
				{
					num15 *= -1f;
				}
			}
			else
			{
				num15 = (float)Math.Atan(num29 / num28);
				if (num28 < 0f)
				{
					num15 += (float)Math.PI;
				}
			}
			num30 = (0f - (float)Math.Sin(num15)) * num14;
			num31 = (float)Math.Cos(num15) * num14;
			num25 += num30 * vh1.mv[ubID].M21 + num31 * vh1.mv[ubID].M31;
			num26 += num30 * vh1.mv[ubID].M22 + num31 * vh1.mv[ubID].M32;
			num27 += num30 * vh1.mv[ubID].M23 + num31 * vh1.mv[ubID].M33;
			num28 = vh1.momentum.collisionPoints[num17];
			num29 = vh1.momentum.collisionPoints[num17 + 1];
			num14 = vh1.ph1.angularVelocityZ * (float)Math.Sqrt(num28 * num28 + num29 * num29);
			if (num28 == 0f)
			{
				num15 = (float)Math.PI / 2f;
				if (num29 < 0f)
				{
					num15 *= -1f;
				}
			}
			else
			{
				num15 = (float)Math.Atan(num29 / num28);
				if (num28 < 0f)
				{
					num15 += (float)Math.PI;
				}
			}
			num30 = (0f - (float)Math.Sin(num15)) * num14;
			num31 = (float)Math.Cos(num15) * num14;
			num25 += num30 * vh1.mv[ubID].M11 + num31 * vh1.mv[ubID].M21;
			num26 += num30 * vh1.mv[ubID].M12 + num31 * vh1.mv[ubID].M22;
			num27 += num30 * vh1.mv[ubID].M13 + num31 * vh1.mv[ubID].M23;
			num25 *= num22;
			num26 *= num23;
			num27 *= num24;
			float num32 = (0f - (1f + num2)) * (num25 + num26 + num27) / num13;
			float num33 = num32 * num22;
			float num34 = num32 * num23;
			float num35 = num32 * num24;
			num6 += num33;
			num7 += num34;
			num8 += num35;
			num19 = num33 * matrix.M11 + num34 * matrix.M21 + num35 * matrix.M31;
			num20 = num33 * matrix.M12 + num34 * matrix.M22 + num35 * matrix.M32;
			num21 = num33 * matrix.M13 + num34 * matrix.M23 + num35 * matrix.M33;
			num9 += (0f - num20) * vh1.momentum.collisionPoints[num17 + 2] + num21 * vh1.momentum.collisionPoints[num17 + 1];
			num10 += num19 * vh1.momentum.collisionPoints[num17 + 2] - num21 * vh1.momentum.collisionPoints[num17];
			num++;
		}
		if (momentum.forceX != 0f)
		{
			if (Math.Abs(vh1.ph1.velocityX) < 0.75f)
			{
				vh1.newVelX = 0f;
			}
			else
			{
				vh1.newVelX = vh1.ph1.velocityX + num6 / momentum.forceX / vh1.ph1.mass;
			}
			vh1.ph1.forceX = 0f;
			vh1.ph1.velocityX = 0f;
			vh1.ph1.accelerationX = 0f;
		}
		if (momentum.forceY != 0f)
		{
			if (Math.Abs(vh1.ph1.velocityY) < 0.75f)
			{
				vh1.newVelY = 0f;
			}
			else
			{
				vh1.newVelY = vh1.ph1.velocityY + num7 / momentum.forceY / vh1.ph1.mass;
			}
			vh1.ph1.forceY = 0f;
			vh1.ph1.velocityY = 0f;
			vh1.ph1.accelerationY = 0f;
		}
		if (momentum.forceZ != 0f)
		{
			if (Math.Abs(vh1.ph1.velocityZ) < 0.75f)
			{
				vh1.newVelZ = 0f;
			}
			else
			{
				vh1.newVelZ = vh1.ph1.velocityZ + num8 / momentum.forceZ / vh1.ph1.mass;
			}
			vh1.ph1.forceZ = 0f;
			vh1.ph1.velocityZ = 0f;
			vh1.ph1.accelerationZ = 0f;
		}
		vh1.ph1.angularVelocityX += num9 / (float)(int)num / vh1.ph1.momentInertiaAxisX;
		vh1.ph1.angularVelocityY += num10 / (float)(int)num / vh1.ph1.momentInertiaAxisX;
		vh1.ph1.angularVelocityZ += num11 / (float)(int)num / vh1.ph1.momentInertiaAxisX;
	}

	public void Calculate_Vehicle_Collision_Data_Ver1(ref StructsClass.Vehicle vh1, ushort vhID, byte ubID, float frameTime, byte threadID)
	{
		float num = 0.33f;
		float num2 = 0f;
		float num3 = 0f;
		float num4 = 0f;
		float num5 = 0f;
		float num6 = 0f;
		float num7 = 0f;
		float num8 = 0f;
		float num9 = 0f;
		float num10 = 0f;
		_ = vehicles[vhID].numWheels;
		StructsClass.Momentum momentum = vehicles[vhID].momentum;
		short numPoints = momentum.numPoints;
		Matrix matrix = Matrix.Invert(vh1.mv[ubID]);
		momentum.forceX = 0f;
		momentum.forceY = 0f;
		momentum.forceZ = 0f;
		momentum.countForces = 0;
		int num15;
		int i;
		float num12;
		float num13;
		for (i = 0; i < numPoints; i++)
		{
			if (global::Collision.Collision.floatArStatus[threadID, i] == 1)
			{
				int num11 = momentum.countForces * 3;
				momentum.buffForceValue[num11++] = 0f;
				momentum.buffForceValue[num11++] = 0f;
				momentum.buffForceValue[num11] = 0f;
				num12 = global::Collision.Collision.floatArDir[threadID, i, 0];
				num13 = global::Collision.Collision.floatArDir[threadID, i, 1];
				float num14 = global::Collision.Collision.floatArDir[threadID, i, 2];
				momentum.forceX += Math.Abs(num12);
				momentum.forceY += Math.Abs(num13);
				momentum.forceZ += Math.Abs(num14);
				num15 = momentum.countForces * 3;
				momentum.buffForceValue[num15++] = num12;
				momentum.buffForceValue[num15++] = num13;
				momentum.buffForceValue[num15] = num14;
				momentum.buffimpactForceId[momentum.countForces] = (short)i;
				momentum.countForces++;
			}
		}
		if (momentum.countForces < 1)
		{
			return;
		}
		i = 0;
		num15 = 0;
		float num30;
		float num21;
		float num22;
		float num28;
		float num29;
		for (; i < momentum.countForces; i++)
		{
			num15 = i * 3;
			int num16 = momentum.buffimpactForceId[i] * 3;
			int num17 = num16 + 129;
			float num18 = global::Collision.Collision.floatAr[threadID, num17];
			float num19 = global::Collision.Collision.floatAr[threadID, num17 + 1];
			float num20 = global::Collision.Collision.floatAr[threadID, num17 + 2];
			num21 = momentum.buffForceValue[num15];
			num22 = momentum.buffForceValue[num15 + 1];
			float num23 = momentum.buffForceValue[num15 + 2];
			num2 = num19 * num23 - num20 * num22;
			num3 = num20 * num21 - num18 * num23;
			num4 = num18 * num22 - num19 * num21;
			num12 = 1f / vh1.ph1.mass + (num2 * num2 + num3 * num3 + num4 * num4) / vh1.ph1.momentInertiaAxisX;
			num18 = vh1.ph1.angularVelocityX;
			num19 = vh1.ph1.angularVelocityY;
			num20 = vh1.ph1.angularVelocityZ;
			num21 = global::Collision.Collision.floatAr[threadID, num17];
			num22 = global::Collision.Collision.floatAr[threadID, num17 + 1];
			num23 = global::Collision.Collision.floatAr[threadID, num17 + 2];
			float num24 = vh1.ph1.velocityX + num19 * num23 - num20 * num22;
			float num25 = vh1.ph1.velocityY + num20 * num21 - num18 * num23;
			float num26 = vh1.ph1.velocityZ + num18 * num22 - num19 * num21;
			num24 *= momentum.buffForceValue[num15];
			num25 *= momentum.buffForceValue[num15 + 1];
			num26 *= momentum.buffForceValue[num15 + 2];
			float num27 = (0f - (1f + num)) * (num24 + num25 + num26) / num12;
			num28 = num27 * momentum.buffForceValue[num15];
			num29 = num27 * momentum.buffForceValue[num15 + 1];
			num30 = num27 * momentum.buffForceValue[num15 + 2];
			num5 += num28;
			num6 += num29;
			num7 += num30;
			num18 = global::Collision.Collision.floatAr[threadID, num17];
			num19 = global::Collision.Collision.floatAr[threadID, num17 + 1];
			num20 = global::Collision.Collision.floatAr[threadID, num17 + 2];
			num21 = num28;
			num22 = num29;
			num23 = num30;
			num2 = num19 * num23 - num20 * num22;
			num3 = num20 * num21 - num18 * num23;
			num4 = num18 * num22 - num19 * num21;
			num12 = num2 * matrix.M11 + num3 * matrix.M21 + num4 * matrix.M31;
			num13 = num2 * matrix.M12 + num3 * matrix.M22 + num4 * matrix.M32;
			float num14 = num2 * matrix.M13 + num3 * matrix.M23 + num4 * matrix.M33;
			num8 += num12;
			num9 += num13;
			num10 += num14;
		}
		if (momentum.forceX != 0f)
		{
			vh1.newVelX = vh1.ph1.velocityX + num5 / momentum.forceX / vh1.ph1.mass;
			vh1.ph1.forceX = 0f;
			vh1.ph1.velocityX = 0f;
			vh1.ph1.accelerationX = 0f;
		}
		if (momentum.forceY != 0f)
		{
			vh1.newVelY = vh1.ph1.velocityY + num6 / momentum.forceY / vh1.ph1.mass;
			vh1.ph1.forceY = 0f;
			vh1.ph1.velocityY = 0f;
			vh1.ph1.accelerationY = 0f;
		}
		if (momentum.forceZ != 0f)
		{
			vh1.newVelZ = vh1.ph1.velocityZ + num7 / momentum.forceZ / vh1.ph1.mass;
			vh1.ph1.forceZ = 0f;
			vh1.ph1.velocityZ = 0f;
			vh1.ph1.accelerationZ = 0f;
		}
		num8 = num8 / (float)momentum.countForces / vh1.ph1.momentInertiaAxisX;
		num9 = num9 / (float)momentum.countForces / vh1.ph1.momentInertiaAxisX;
		num10 = num10 / (float)momentum.countForces / vh1.ph1.momentInertiaAxisX;
		if (Math.Abs(vh1.ph1.velocityX) < 0.75f)
		{
			vh1.ph1.velocityX = 0f;
		}
		if (Math.Abs(vh1.ph1.velocityY) < 0.75f)
		{
			vh1.ph1.velocityY = 0f;
		}
		if (Math.Abs(vh1.ph1.velocityZ) < 0.75f)
		{
			vh1.ph1.velocityZ = 0f;
		}
		float num31 = 0f;
		float num32 = 0f;
		float num33 = 0f;
		float num34 = 0f;
		ushort num35 = 0;
		ushort num36 = 0;
		ushort num37 = 0;
		ushort num38 = 0;
		num28 = 0f;
		num29 = 0f;
		num30 = 0f;
		for (i = 0; i < numPoints; i++)
		{
			if (global::Collision.Collision.floatArStatus[threadID, i] != 1)
			{
				continue;
			}
			float num14 = global::Collision.Collision.floatArDir[threadID, i, 2];
			if (num14 > 0f)
			{
				int num11 = i * 3 + 129;
				num12 = global::Collision.Collision.floatAr[threadID, num11++];
				num13 = global::Collision.Collision.floatAr[threadID, num11];
				if (num12 > 0f)
				{
					num31 += num12 * num14;
					num35++;
				}
				else if (num12 < 0f)
				{
					num33 -= num12 * num14;
					num37++;
				}
				if (num13 > 0f)
				{
					num32 += num13 * num14;
					num36++;
				}
				else if (num13 < 0f)
				{
					num34 -= num13 * num14;
					num38++;
				}
			}
		}
		if (num35 > 0)
		{
			num31 /= (float)(int)num35;
		}
		if (num37 > 0)
		{
			num33 /= (float)(int)num37;
		}
		if (num36 > 0)
		{
			num32 /= (float)(int)num36;
		}
		if (num38 > 0)
		{
			num34 /= (float)(int)num38;
		}
		num12 = vh1.ph1.mass * 32.15223f;
		if (num35 > 0 && num37 > 0)
		{
			num13 = num31 + num33;
			if (num13 != 0f)
			{
				float num14 = num12 * num33 / num13;
				num33 = num12 * num31 / num13;
				num31 = num14;
			}
		}
		else if (num37 > 0)
		{
			num33 = num12;
		}
		else if (num35 > 0)
		{
			num31 = num12;
		}
		if (num36 > 0 && num38 > 0)
		{
			num13 = num32 + num34;
			if (num13 != 0f)
			{
				float num14 = num12 * num34 / num13;
				num34 = num12 * num32 / num13;
				num32 = num14;
			}
		}
		else if (num38 > 0)
		{
			num34 = num12;
		}
		else if (num36 > 0)
		{
			num32 = num12;
		}
		num13 = num31 + num33 + num32 + num34;
		if (num13 != 0f)
		{
			num13 = num12 / num13;
		}
		num31 *= num13;
		num33 *= num13;
		num32 *= num13;
		num34 *= num13;
		if (num35 > 0)
		{
			num31 /= (float)(int)num35;
		}
		if (num37 > 0)
		{
			num33 /= (float)(int)num37;
		}
		if (num36 > 0)
		{
			num32 /= (float)(int)num36;
		}
		if (num38 > 0)
		{
			num34 /= (float)(int)num38;
		}
		num21 = 0f;
		num22 = 0f;
		for (i = 0; i < numPoints; i++)
		{
			if (global::Collision.Collision.floatArStatus[threadID, i] != 1)
			{
				continue;
			}
			float num14 = global::Collision.Collision.floatArDir[threadID, i, 2];
			if (num14 > 0f)
			{
				int num17 = i * 3 + 129;
				float num18 = global::Collision.Collision.floatAr[threadID, num17++];
				float num19 = global::Collision.Collision.floatAr[threadID, num17++];
				float num20 = global::Collision.Collision.floatAr[threadID, num17];
				num12 = num18;
				num13 = num19;
				float num39 = 0f;
				float num40 = 0f;
				if (num12 > 0f)
				{
					float num23 = num31;
					num2 = num19 * num23;
					num3 = (0f - num18) * num23;
				}
				else if (num12 < 0f)
				{
					float num23 = num33;
					num2 = num19 * num23;
					num3 = (0f - num18) * num23;
				}
				num39 = num2 * matrix.M11 + num3 * matrix.M21;
				num40 = num2 * matrix.M12 + num3 * matrix.M22;
				num28 += num39;
				num29 += num40;
				num39 = 0f;
				num40 = 0f;
				if (num13 > 0f)
				{
					float num23 = num32;
					num2 = num19 * num23;
					num3 = (0f - num18) * num23;
				}
				else if (num13 < 0f)
				{
					float num23 = num34;
					num2 = num19 * num23;
					num3 = (0f - num18) * num23;
				}
				num39 = num2 * matrix.M11 + num3 * matrix.M21;
				num40 = num2 * matrix.M12 + num3 * matrix.M22;
				num28 += num39;
				num29 += num40;
			}
		}
		num12 = num28 / vh1.ph1.momentInertiaAxisX * frameTime;
		if (Math.Abs(num12) > Math.Abs(num8) || Math.Abs(num8) < 0.02f)
		{
			num13 = 1f;
			if ((num12 < 0f && vh1.ph1.angularVelocityX > 0f) || (num12 > 0f && vh1.ph1.angularVelocityX < 0f))
			{
				num13 = 20f;
				if (Math.Abs(num12 * num13) > Math.Abs(vh1.ph1.angularVelocityX))
				{
					vh1.ph1.angularVelocityX = 0f;
					num13 = 0f;
				}
			}
			vh1.ph1.angularVelocityX += num12 * num13;
			if (Math.Abs(vh1.ph1.angularVelocityX) < 2E-06f)
			{
				vh1.ph1.angularVelocityX = 0f;
			}
		}
		else
		{
			vh1.ph1.angularVelocityX += num8;
		}
		num12 = num29 / vh1.ph1.momentInertiaAxisX * frameTime;
		if (Math.Abs(num12) > Math.Abs(num9) || Math.Abs(num9) < 0.02f)
		{
			num13 = 1f;
			if ((num12 < 0f && vh1.ph1.angularVelocityY > 0f) || (num12 > 0f && vh1.ph1.angularVelocityY < 0f))
			{
				num13 = 20f;
				if (Math.Abs(num12 * num13) > Math.Abs(vh1.ph1.angularVelocityY))
				{
					vh1.ph1.angularVelocityY = 0f;
					num13 = 0f;
				}
			}
			vh1.ph1.angularVelocityY += num12 * num13;
			if (Math.Abs(vh1.ph1.angularVelocityY) < 2E-06f)
			{
				vh1.ph1.angularVelocityY = 0f;
			}
		}
		else
		{
			vh1.ph1.angularVelocityY += num9;
		}
		vh1.ph1.angularVelocityZ += num10;
		if (Math.Abs(num10) < 0.02f)
		{
			vh1.ph1.angularVelocityZ = 0f;
		}
	}

	public void Update_Wheel_Positions(ref StructsClass.Vehicle vh1, ushort vhID, byte wheelID, float rotX, float rotY, float rotZ)
	{
		if (vehicles[vhID].numWheels >= 1)
		{
			int num = wheelID * 3;
			Matrix matrix = Matrix.CreateRotationX(rotX * ((float)Math.PI / 180f)) * Matrix.CreateRotationY(rotY * ((float)Math.PI / 180f)) * Matrix.CreateRotationZ(rotZ * ((float)Math.PI / 180f)) * Matrix.CreateTranslation(vehicles[vhID].wheelAttachPoint[num], vehicles[vhID].wheelAttachPoint[num + 1], vehicles[vhID].wheelAttachPoint[num + 2]);
			matrix = vehicles[vhID].wheelsMatrix[wheelID] * matrix;
			matrix = Matrix.CreateTranslation(vehicles[vhID].wheelColPoints[num], vehicles[vhID].wheelColPoints[num + 1], vehicles[vhID].wheelColPoints[num + 2]) * matrix;
			float num2 = matrix.M41;
			float m = matrix.M42;
			float m2 = matrix.M43;
			vh1.wheelColPointsRadiusAxisZ[wheelID] = (float)Math.Sqrt(num2 * num2 + m * m);
			if (vh1.wheelColPointsRadiusAxisZ[wheelID] == 0f)
			{
				vh1.wheelColPointsRadiusAxisZ[wheelID] = 0.0001f;
			}
			if (num2 == 0f && m == 0f && m2 == 0f)
			{
				num2 = 0.0001f;
			}
			vh1.momentum.collisionPoints[num++] = num2;
			vh1.momentum.collisionPoints[num++] = m;
			vh1.momentum.collisionPoints[num] = m2;
		}
	}

	public void Swap_Vehicles(byte newVhID, byte oldVhID, float adjZ)
	{
		if (newVhID >= numVehicles || oldVhID >= numVehicles)
		{
			return;
		}
		vehicles[newVhID].ph1.x = vehicles[oldVhID].ph1.x;
		vehicles[newVhID].ph1.y = vehicles[oldVhID].ph1.y;
		vehicles[newVhID].ph1.z = vehicles[oldVhID].ph1.z + adjZ;
		int numWeaponMounts = vehicles[oldVhID].numWeaponMounts;
		if (vehicles[newVhID].numWeaponMounts < numWeaponMounts)
		{
			vehicles[newVhID].weaponMounts = new ushort[numWeaponMounts];
		}
		vehicles[newVhID].numWeaponMounts = (byte)numWeaponMounts;
		for (int i = 0; i < numWeaponMounts; i++)
		{
			vehicles[newVhID].weaponMounts[i] = vehicles[oldVhID].weaponMounts[i];
		}
		numWeaponMounts = vehicles[oldVhID].numMounts;
		if (vehicles[newVhID].numMounts < numWeaponMounts)
		{
			vehicles[newVhID].mounts = new StructsClass.Mounting_Point_Fixed[numWeaponMounts];
			for (int i = 0; i < numWeaponMounts; i++)
			{
				vehicles[newVhID].mounts[i] = default(StructsClass.Mounting_Point_Fixed);
				vehicles[newVhID].mounts[i].mvCurrent = new Matrix[2];
			}
		}
		vehicles[newVhID].numMounts = (byte)numWeaponMounts;
		for (int i = 0; i < numWeaponMounts; i++)
		{
			vehicles[newVhID].mounts[i].type = vehicles[oldVhID].mounts[i].type;
			vehicles[newVhID].mounts[i].jointID = vehicles[oldVhID].mounts[i].jointID;
			vehicles[newVhID].mounts[i].mvStart = vehicles[oldVhID].mounts[i].mvStart;
			ref Matrix reference = ref vehicles[newVhID].mounts[i].mvCurrent[0];
			reference = vehicles[oldVhID].mounts[i].mvCurrent[0];
			ref Matrix reference2 = ref vehicles[newVhID].mounts[i].mvCurrent[1];
			reference2 = vehicles[oldVhID].mounts[i].mvCurrent[1];
			vehicles[newVhID].mounts[i].objectID = vehicles[oldVhID].mounts[i].objectID;
			vehicles[newVhID].mounts[i].itemPlacmentMatrixID = vehicles[oldVhID].mounts[i].itemPlacmentMatrixID;
			vehicles[newVhID].mounts[i].objectAttached = vehicles[oldVhID].mounts[i].objectAttached;
		}
		numWeaponMounts = vehicles[oldVhID].numWeapons;
		if (vehicles[newVhID].numWeapons < numWeaponMounts)
		{
			vehicles[newVhID].weapons = new StructsClass.weapon_stub[numWeaponMounts];
			vehicles[newVhID].numWeapons = (byte)numWeaponMounts;
			for (int i = 0; i < numWeaponMounts; i++)
			{
				vehicles[newVhID].weapons[i] = default(StructsClass.weapon_stub);
				StructsClass.Initialize_Weapon_Stub(ref vehicles[newVhID].weapons[i]);
			}
		}
		numWeaponMounts = vehicles[oldVhID].numWeapons;
		for (int i = 0; i < numWeaponMounts; i++)
		{
			ref StructsClass.weapon_stub reference3 = ref vehicles[newVhID].weapons[i];
			reference3 = vehicles[oldVhID].weapons[i];
		}
		numWeaponMounts = vehicles[oldVhID].maxOccupants;
		if (vehicles[newVhID].maxOccupants < numWeaponMounts)
		{
			vehicles[newVhID].playerIDs = new ushort[numWeaponMounts];
		}
		vehicles[newVhID].maxOccupants = (ushort)numWeaponMounts;
		vehicles[newVhID].currentOccupants = vehicles[oldVhID].currentOccupants;
		for (int i = 0; i < numWeaponMounts; i++)
		{
			vehicles[newVhID].playerIDs[i] = vehicles[oldVhID].playerIDs[i];
		}
		vehicles[newVhID].maxOccupants = vehicles[oldVhID].maxOccupants;
		vehicles[newVhID].currentOccupants = vehicles[oldVhID].currentOccupants;
		vehicles[newVhID].startX = vehicles[oldVhID].startX;
		vehicles[newVhID].startY = vehicles[oldVhID].startY;
		vehicles[newVhID].startZ = vehicles[oldVhID].startZ;
		vehicles[newVhID].startRotX = vehicles[oldVhID].startRotX;
		vehicles[newVhID].startRotY = vehicles[oldVhID].startRotY;
		vehicles[newVhID].startRotZ = vehicles[oldVhID].startRotZ;
		vehicles[newVhID].ph1.forceX = vehicles[oldVhID].ph1.forceX;
		vehicles[newVhID].ph1.forceY = vehicles[oldVhID].ph1.forceY;
		vehicles[newVhID].ph1.forceZ = vehicles[oldVhID].ph1.forceZ;
		vehicles[newVhID].ph1.accelerationX = vehicles[oldVhID].ph1.accelerationX;
		vehicles[newVhID].ph1.accelerationY = vehicles[oldVhID].ph1.accelerationY;
		vehicles[newVhID].ph1.accelerationZ = vehicles[oldVhID].ph1.accelerationZ;
		vehicles[newVhID].ph1.velocityX = vehicles[oldVhID].ph1.velocityX;
		vehicles[newVhID].ph1.velocityY = vehicles[oldVhID].ph1.velocityY;
		vehicles[newVhID].ph1.velocityZ = vehicles[oldVhID].ph1.velocityZ;
		vehicles[newVhID].ph1.torqueX = vehicles[oldVhID].ph1.torqueX;
		vehicles[newVhID].ph1.torqueY = vehicles[oldVhID].ph1.torqueY;
		vehicles[newVhID].ph1.torqueZ = vehicles[oldVhID].ph1.torqueZ;
		vehicles[newVhID].ph1.angularAccerlationX = vehicles[oldVhID].ph1.angularAccerlationX;
		vehicles[newVhID].ph1.angularAccerlationY = vehicles[oldVhID].ph1.angularAccerlationY;
		vehicles[newVhID].ph1.angularAccerlationZ = vehicles[oldVhID].ph1.angularAccerlationZ;
		vehicles[newVhID].ph1.angularVelocityX = vehicles[oldVhID].ph1.angularVelocityX;
		vehicles[newVhID].ph1.angularVelocityY = vehicles[oldVhID].ph1.angularVelocityY;
		vehicles[newVhID].ph1.angularVelocityZ = vehicles[oldVhID].ph1.angularVelocityZ;
		vehicles[newVhID].ph1.initialTime = vehicles[oldVhID].ph1.initialTime;
		vehicles[newVhID].ph1.velocity = vehicles[oldVhID].ph1.velocity;
		ref Matrix reference4 = ref vehicles[newVhID].mv[0];
		reference4 = vehicles[oldVhID].mv[0];
		ref Matrix reference5 = ref vehicles[newVhID].mv[1];
		reference5 = vehicles[oldVhID].mv[1];
		vehicles[newVhID].xBalanceTimer = vehicles[oldVhID].xBalanceTimer;
		vehicles[newVhID].yBalanceTimer = vehicles[oldVhID].yBalanceTimer;
		vehicles[newVhID].zBalanceTimer = vehicles[oldVhID].zBalanceTimer;
		vehicles[newVhID].vehicleTimer1 = vehicles[oldVhID].vehicleTimer1;
		vehicles[newVhID].particleTimer = vehicles[oldVhID].particleTimer;
		vehicles[newVhID].wheelTouchingTimer = vehicles[oldVhID].wheelTouchingTimer;
		vehicles[newVhID].Vx = vehicles[oldVhID].Vx;
		vehicles[newVhID].Vy = vehicles[oldVhID].Vy;
		vehicles[newVhID].Vz = vehicles[oldVhID].Vz;
		vehicles[newVhID].throttleSpeed = vehicles[oldVhID].throttleSpeed;
		vehicles[newVhID].maxDamage = vehicles[oldVhID].maxDamage;
		vehicles[newVhID].damageThresholdForExplosion = vehicles[oldVhID].damageThresholdForExplosion;
		vehicles[newVhID].explosionDamage = vehicles[oldVhID].explosionDamage;
		vehicles[newVhID].explosionImpactForce = vehicles[oldVhID].explosionImpactForce;
		vehicles[newVhID].accelerationFactor = vehicles[oldVhID].accelerationFactor;
		vehicles[newVhID].controllerSpring = vehicles[oldVhID].controllerSpring;
		vehicles[newVhID].controllerDampening = vehicles[oldVhID].controllerDampening;
		vehicles[newVhID].damageParticleX = vehicles[oldVhID].damageParticleX;
		vehicles[newVhID].damageParticleY = vehicles[oldVhID].damageParticleY;
		vehicles[newVhID].damageParticleZ = vehicles[oldVhID].damageParticleZ;
		vehicles[newVhID].maxHeat = vehicles[oldVhID].maxHeat;
		vehicles[newVhID].heatGeneration = vehicles[oldVhID].heatGeneration;
		vehicles[newVhID].heatDissipation = vehicles[oldVhID].heatDissipation;
		vehicles[newVhID].overHeatingDamage = vehicles[oldVhID].overHeatingDamage;
		vehicles[newVhID].curHeat = vehicles[oldVhID].curHeat;
	}

	public void Clone_Vehicle_Data(ref StructsClass.Vehicle vh1, byte vhID)
	{
		vh1.maxHeat = vehicles[vhID].maxHeat;
		vh1.heatGeneration = vehicles[vhID].heatGeneration;
		vh1.heatDissipation = vehicles[vhID].heatDissipation;
		vh1.overHeatingDamage = vehicles[vhID].overHeatingDamage;
		vh1.curHeat = vehicles[vhID].curHeat;
		vh1.damageParticleX = vehicles[vhID].damageParticleX;
		vh1.damageParticleY = vehicles[vhID].damageParticleY;
		vh1.damageParticleZ = vehicles[vhID].damageParticleZ;
		vh1.numWheels = vehicles[vhID].numWheels;
		vh1.type = vehicles[vhID].type;
		vh1.numColPoints = vehicles[vhID].numColPoints;
		ref Matrix reference = ref vh1.mv[0];
		reference = vehicles[vhID].mv[0];
		ref Matrix reference2 = ref vh1.mv[1];
		reference2 = vehicles[vhID].mv[1];
		vh1.ph1.momentInertiaAxisX = vehicles[vhID].ph1.momentInertiaAxisX;
		vh1.ph1.momentInertiaAxisY = vehicles[vhID].ph1.momentInertiaAxisY;
		vh1.ph1.momentInertiaAxisZ = vehicles[vhID].ph1.momentInertiaAxisZ;
		vh1.ph1.mass = vehicles[vhID].ph1.mass;
		vh1.throttleSpeed = vehicles[vhID].throttleSpeed;
		vh1.damageThresholdForExplosion = vehicles[vhID].damageThresholdForExplosion;
		vh1.accelerationFactor = vehicles[vhID].accelerationFactor;
		vh1.controllerSpring = vehicles[vhID].controllerSpring;
		vh1.controllerDampening = vehicles[vhID].controllerDampening;
		vh1.maxDamage = vehicles[vhID].maxDamage;
		vh1.explosionDamage = vehicles[vhID].explosionDamage;
		vh1.explosionImpactForce = vehicles[vhID].explosionImpactForce;
		vh1.startX = vehicles[vhID].startX;
		vh1.startY = vehicles[vhID].startY;
		vh1.startZ = vehicles[vhID].startZ;
		vh1.startRotX = vehicles[vhID].startRotX;
		vh1.startRotY = vehicles[vhID].startRotY;
		vh1.startRotZ = vehicles[vhID].startRotZ;
		vh1.vehicleTimer1 = vehicles[vhID].vehicleTimer1;
		vh1.particleTimer = vehicles[vhID].particleTimer;
		int maxOccupants = vehicles[vhID].maxOccupants;
		if (vh1.maxOccupants < maxOccupants)
		{
			vh1.playerIDs = new ushort[maxOccupants];
		}
		vh1.maxOccupants = (ushort)maxOccupants;
		vh1.currentOccupants = vehicles[vhID].currentOccupants;
		int i;
		for (i = 0; i < maxOccupants; i++)
		{
			vh1.playerIDs[i] = vehicles[vhID].playerIDs[i];
		}
		maxOccupants = vehicles[vhID].numWeaponMounts;
		if (vh1.numWeaponMounts < maxOccupants)
		{
			vh1.weaponMounts = new ushort[maxOccupants];
		}
		vh1.numWeaponMounts = (byte)maxOccupants;
		for (i = 0; i < maxOccupants; i++)
		{
			vh1.weaponMounts[i] = vehicles[vhID].weaponMounts[i];
		}
		maxOccupants = vehicles[vhID].numMounts;
		if (vh1.numMounts < maxOccupants)
		{
			vh1.mounts = new StructsClass.Mounting_Point_Fixed[maxOccupants];
			for (i = 0; i < maxOccupants; i++)
			{
				vh1.mounts[i] = default(StructsClass.Mounting_Point_Fixed);
				vh1.mounts[i].mvCurrent = new Matrix[2];
			}
		}
		vh1.numMounts = (byte)maxOccupants;
		for (i = 0; i < maxOccupants; i++)
		{
			vh1.mounts[i].type = vehicles[vhID].mounts[i].type;
			vh1.mounts[i].jointID = vehicles[vhID].mounts[i].jointID;
			vh1.mounts[i].mvStart = vehicles[vhID].mounts[i].mvStart;
			ref Matrix reference3 = ref vh1.mounts[i].mvCurrent[0];
			reference3 = vehicles[vhID].mounts[i].mvCurrent[0];
			ref Matrix reference4 = ref vh1.mounts[i].mvCurrent[1];
			reference4 = vehicles[vhID].mounts[i].mvCurrent[1];
			vh1.mounts[i].objectID = vehicles[vhID].mounts[i].objectID;
			vh1.mounts[i].itemPlacmentMatrixID = vehicles[vhID].mounts[i].itemPlacmentMatrixID;
			vh1.mounts[i].objectAttached = vehicles[vhID].mounts[i].objectAttached;
		}
		maxOccupants = vehicles[vhID].numWeapons;
		vh1.weapons = new StructsClass.weapon_stub[maxOccupants];
		vh1.numWeapons = (byte)maxOccupants;
		for (i = 0; i < maxOccupants; i++)
		{
			vh1.weapons[i] = default(StructsClass.weapon_stub);
			StructsClass.Initialize_Weapon_Stub(ref vh1.weapons[i]);
			ref StructsClass.weapon_stub reference5 = ref vh1.weapons[i];
			reference5 = vehicles[vhID].weapons[i];
		}
		vh1.currentOccupants = vehicles[vhID].currentOccupants;
		byte numWheels = vh1.numWheels;
		vh1.wheelRot = new float[numWheels];
		vh1.wheelColPoints = new float[numWheels * 3];
		vh1.wheelColPointsRadiusAxisZ = new float[numWheels];
		maxOccupants = 0;
		i = 0;
		for (; maxOccupants < numWheels; maxOccupants++)
		{
			vh1.wheelRot[maxOccupants] = 0f;
			vh1.wheelColPoints[i++] = 0f;
			vh1.wheelColPoints[i++] = 0f;
			vh1.wheelColPoints[i++] = 0f;
		}
		int num = numWheels + vh1.numColPoints;
		vh1.momentum.numPoints = (short)num;
		vh1.momentum.collisionPoints = new float[num * 3];
		maxOccupants = 0;
		num = 0;
		for (; maxOccupants < numWheels; maxOccupants++)
		{
			vh1.wheelColPointsRadiusAxisZ[maxOccupants] = vehicles[vhID].wheelColPointsRadiusAxisZ[maxOccupants];
			vh1.momentum.collisionPoints[num] = vehicles[vhID].momentum.collisionPoints[num];
			num++;
			vh1.momentum.collisionPoints[num] = vehicles[vhID].momentum.collisionPoints[num];
			num++;
			vh1.momentum.collisionPoints[num] = vehicles[vhID].momentum.collisionPoints[num];
			num++;
		}
		maxOccupants = 0;
		i = 0;
		for (; maxOccupants < vehicles[vhID].numColPoints; maxOccupants++)
		{
			vh1.momentum.collisionPoints[num++] = vehicles[vhID].colPoints[i++];
			vh1.momentum.collisionPoints[num++] = vehicles[vhID].colPoints[i++];
			vh1.momentum.collisionPoints[num++] = vehicles[vhID].colPoints[i++];
		}
	}

	public void Set_Vehicle_Position(ref StructsClass.Vehicle vh1, float x, float y, float z, float rotX, float rotY, float rotZ)
	{
		vh1.ph1.x = x;
		vh1.ph1.y = y;
		vh1.ph1.z = z;
		Matrix matrix = Matrix.CreateRotationZ(rotZ) * Matrix.CreateRotationY(rotY) * Matrix.CreateRotationX(rotX);
		vh1.mv[0] = matrix;
		vh1.mv[1] = matrix;
	}

	public void Set_All_Vehicle_Models()
	{
		for (byte b = 0; b < numVehicles; b++)
		{
			byte b2 = 0;
			byte numModels = vehicles[b].numModels;
			while (b2 < numModels)
			{
				vehicles[b].vehicleModel[b2] = (short)mainC.modelsMain.Find_Model(vehicles[b].modelName[b2]);
				if (vehicles[b].textureID[b2] < 0)
				{
					vehicles[b].textureID[b2] = (short)global::Models.Models.mod1[vehicles[b].vehicleModel[b2]].textureList[0];
				}
				b2++;
			}
		}
	}

	public void Player_Enters_Vehicle(ushort playerID, ushort vhID)
	{
		if (++global::Players.Players.players[playerID].curVehicleIndex >= 3)
		{
			global::Players.Players.players[playerID].curVehicleIndex--;
			return;
		}
		byte type = vehicles[vhID].type;
		if (type == 3)
		{
			bool flag = false;
			bool flag2 = false;
			byte b = 0;
			if (vehicles[vhID].currentOccupants > 0)
			{
				global::Players.Players.players[playerID].curVehicleIndex--;
				return;
			}
			global::Players.Players.players[playerID].curVehicle = vhID;
			global::Players.Players.players[playerID].vehicles[global::Players.Players.players[playerID].curVehicleIndex] = vhID;
			vehicles[vhID].currentOccupants = 1;
			vehicles[vhID].playerIDs[0] = playerID;
			ushort weaponID = global::Weapons.Weapons.wpmMounts[vehicles[vhID].weaponMounts[0]].weaponID;
			for (byte b2 = 0; b2 < global::Players.Players.players[0].numAvailableWeapons; b2++)
			{
				if (global::Players.Players.players[0].weaponList[b2] < 0)
				{
					flag = true;
					b = b2;
					break;
				}
				if (global::Players.Players.players[0].weaponList[b2] == weaponID)
				{
					flag2 = true;
					break;
				}
			}
			if (flag && !flag2)
			{
				global::Players.Players.players[0].weaponList[b] = (sbyte)weaponID;
				mainC.weaponsMain.Add_Ammo_Clip(mainC.weaponsMain.Find_Ammo_Clip((byte)global::Weapons.Weapons.wp1[weaponID].ammoIndex), 1, 0);
				mainC.weaponsMain.Set_MainPlayer_Weapon(0, (sbyte)b, reset: true);
				mainC.weaponsMain.Reset_Players_Weapon_Stub(ref global::Players.Players.players[0].weapon2[b], b, (byte)weaponID, 0);
				global::Players.Players.players[0].renderWeapon = (byte)(global::Players.Players.players[0].renderWeapon | 4);
				return;
			}
			global::Players.Players.players[playerID].curVehicleIndex--;
			global::Players.Players.players[playerID].curVehicle = global::Players.Players.players[playerID].vehicles[global::Players.Players.players[playerID].curVehicleIndex];
			if (vehicles[vhID].currentOccupants > 0)
			{
				vehicles[vhID].currentOccupants--;
			}
			vehicles[vhID].playerIDs[0] = global::Util.Util.maxUnsignedShortValue;
		}
		else
		{
			global::Players.Players.players[playerID].curVehicleIndex--;
		}
	}

	public void Player_Leaves_Vehicle(ushort playerID)
	{
		if (global::Players.Players.players[playerID].curVehicleIndex == 0)
		{
			return;
		}
		ushort curVehicle = global::Players.Players.players[playerID].curVehicle;
		byte type = vehicles[curVehicle].type;
		if (type == 3)
		{
			if (vehicles[curVehicle].currentOccupants > 0)
			{
				vehicles[curVehicle].currentOccupants--;
			}
			global::Players.Players.players[playerID].curVehicleIndex--;
			global::Players.Players.players[playerID].curVehicle = global::Players.Players.players[playerID].vehicles[global::Players.Players.players[playerID].curVehicleIndex];
			ushort weaponID = global::Weapons.Weapons.wpmMounts[vehicles[curVehicle].weaponMounts[0]].weaponID;
			ushort num = (ushort)global::Players.Players.players[0].numAvailableWeapons;
			for (ushort num2 = 0; num2 < num; num2++)
			{
				if (global::Players.Players.players[0].weaponList[num2] == (sbyte)weaponID)
				{
					global::Players.Players.players[0].weaponList[num2] = -1;
					mainC.weaponsMain.previousWeapon();
					break;
				}
			}
		}
		else
		{
			global::Players.Players.players[playerID].curVehicleIndex--;
			global::Players.Players.players[playerID].curVehicle = global::Players.Players.players[playerID].vehicles[global::Players.Players.players[playerID].curVehicleIndex];
		}
		global::Players.Players.players[0].renderWeapon = (byte)(global::Players.Players.players[0].renderWeapon & -5);
		global::Players.Players.xRotation = 0f;
	}

	public void Player_Exits_All_Vehicles(ushort playerID)
	{
		ushort num = 3;
		while (global::Players.Players.players[playerID].curVehicleIndex != 0 && num-- > 0)
		{
			ushort curVehicle = global::Players.Players.players[playerID].curVehicle;
			byte type = vehicles[curVehicle].type;
			if (type == 3)
			{
				if (vehicles[curVehicle].currentOccupants > 0)
				{
					vehicles[curVehicle].currentOccupants--;
				}
				global::Players.Players.players[playerID].curVehicleIndex--;
				global::Players.Players.players[playerID].curVehicle = global::Players.Players.players[playerID].vehicles[global::Players.Players.players[playerID].curVehicleIndex];
				ushort weaponID = global::Weapons.Weapons.wpmMounts[vehicles[curVehicle].weaponMounts[0]].weaponID;
				ushort num2 = (ushort)global::Players.Players.players[0].numAvailableWeapons;
				for (ushort num3 = 0; num3 < num2; num3++)
				{
					if (global::Players.Players.players[0].weaponList[num3] == (sbyte)weaponID)
					{
						global::Players.Players.players[0].weaponList[num3] = -1;
						mainC.weaponsMain.previousWeapon();
						break;
					}
				}
			}
			else
			{
				global::Players.Players.players[playerID].curVehicleIndex--;
				global::Players.Players.players[playerID].curVehicle = global::Players.Players.players[playerID].vehicles[global::Players.Players.players[playerID].curVehicleIndex];
			}
			global::Players.Players.players[0].renderWeapon = (byte)(global::Players.Players.players[0].renderWeapon & -5);
			global::Players.Players.xRotation = 0f;
		}
	}

	public void Reset_Round()
	{
		for (ushort num = 0; num < numVehicles; num++)
		{
			byte type = vehicles[num].type;
			if (type == 3)
			{
				vehicles[num].currentOccupants = 0;
				vehicles[num].vehicleTimer1 = 0f;
				vehicles[num].particleTimer = 0f;
			}
			else
			{
				vehicles[num].vehicleTimer1 = 0f;
				vehicles[num].particleTimer = 0f;
			}
		}
	}

	public void Reset_Player_Vehicle_Variables(ushort vhID)
	{
		StructsClass.Reset_Physics_New(ref MainGame.playerVehicles[vhID].ph1);
		MainGame.playerVehicles[vhID].vehicleTimer1 = 0f;
		MainGame.playerVehicles[vhID].particleTimer = 0f;
		MainGame.playerVehicles[vhID].throttleSpeed = 0f;
		ushort numMounts = MainGame.playerVehicles[vhID].numMounts;
		for (ushort num = 0; num < numMounts; num++)
		{
			ref Matrix reference = ref MainGame.playerVehicles[vhID].mounts[num].mvCurrent[0];
			reference = MainGame.playerVehicles[vhID].mounts[num].mvStart;
			ref Matrix reference2 = ref MainGame.playerVehicles[vhID].mounts[num].mvCurrent[1];
			reference2 = MainGame.playerVehicles[vhID].mounts[num].mvStart;
		}
	}

	public void Reset_Vehicle_Velocity(ref StructsClass.Vehicle vh1)
	{
		vh1.ph1.forceX = 0f;
		vh1.ph1.forceY = 0f;
		vh1.ph1.forceZ = 0f;
		vh1.ph1.accelerationX = 0f;
		vh1.ph1.accelerationY = 0f;
		vh1.ph1.accelerationZ = 0f;
		vh1.ph1.velocityX = 0f;
		vh1.ph1.velocityY = 0f;
		vh1.ph1.velocityZ = 0f;
		vh1.ph1.torqueX = 0f;
		vh1.ph1.torqueY = 0f;
		vh1.ph1.torqueZ = 0f;
		vh1.ph1.angularAccerlationX = 0f;
		vh1.ph1.angularAccerlationY = 0f;
		vh1.ph1.angularAccerlationZ = 0f;
		vh1.ph1.angularVelocityX = 0f;
		vh1.ph1.angularVelocityY = 0f;
		vh1.ph1.angularVelocityZ = 0f;
		vh1.ph1.initialTime = 0.0;
		vh1.ph1.velocity = 0f;
		vh1.throttleSpeed = 0f;
	}

	public void Sync_All_Vehicle_Matrices()
	{
		byte rBufferID = global::Rendering.Rendering.rBufferID;
		byte uBufferID = global::Rendering.Rendering.uBufferID;
		for (ushort num = 0; num < numVehicles; num++)
		{
			ref Matrix reference = ref vehicles[num].mv[uBufferID];
			reference = vehicles[num].mv[rBufferID];
		}
	}

	public void Sync_Player_Vehicle_Mount_Matrices()
	{
		byte uBufferID = global::Rendering.Rendering.uBufferID;
		byte rBufferID = global::Rendering.Rendering.rBufferID;
		for (ushort num = 0; num < MainGame.maxGamePlayers; num++)
		{
			if ((global::Players.Players.players[num].onmap & 0xC) > 0)
			{
				byte numMounts = vehicles[global::Players.Players.players[num].curVehicle].numMounts;
				for (ushort num2 = 0; num2 < numMounts; num2++)
				{
					ref Matrix reference = ref MainGame.playerVehicles[num].mounts[num2].mvCurrent[uBufferID];
					reference = MainGame.playerVehicles[num].mounts[num2].mvCurrent[rBufferID];
				}
			}
		}
	}

	public void Clear_Weapon_Mount_On_Player_Vehicle(ushort vhID, ushort mountID)
	{
		if (mountID < MainGame.playerVehicles[vhID].numMounts && MainGame.playerVehicles[vhID].mounts[mountID].type == 1)
		{
			MainGame.playerVehicles[vhID].mounts[mountID].objectAttached = 0;
		}
	}

	public void Set_Player_Vehicle_Primary_Player_Mount(ushort vhID, ushort mountID)
	{
		if (mountID < MainGame.playerVehicles[vhID].numMounts && MainGame.playerVehicles[vhID].mounts[mountID].type == 0)
		{
			MainGame.playerVehicles[vhID].mounts[mountID].objectAttached = 1;
		}
	}

	public void Create_Vehicle_Texture_List(ushort vhID, out ushort[] textureList)
	{
		ushort numModels = vehicles[vhID].numModels;
		ushort num = 0;
		ushort num2 = 0;
		while (num < numModels)
		{
			num2 += global::Models.Models.mod1[vehicles[vhID].vehicleModel[num]].numTextures;
			num++;
		}
		textureList = new ushort[num2];
		ushort num3 = 0;
		for (num = 0; num < numModels; num++)
		{
			for (num2 = 0; num2 < global::Models.Models.mod1[vehicles[vhID].vehicleModel[num]].numTextures; num2++)
			{
				textureList[num3++] = (ushort)global::Models.Models.mod1[vehicles[vhID].vehicleModel[num]].textureList[num2];
			}
		}
	}

	public void Add_Weapon_To_Player_Vehicle_Mount(ushort vhID, ushort mountID, byte weaponID, byte amount)
	{
		if (mountID < MainGame.playerVehicles[vhID].numMounts && MainGame.playerVehicles[vhID].mounts[mountID].type == 1)
		{
			MainGame.playerVehicles[vhID].mounts[mountID].objectAttached = 1;
			mainC.weaponsMain.Load_Weapon_Into_Player_Vehicle_Weapon_Stub(vhID, MainGame.playerVehicles[vhID].mounts[mountID].objectID, weaponID, amount);
		}
	}

	public void Add_Weapon_To_Player_Vehicle_Stub(ushort vhID, byte curStub, byte weaponID, byte amount)
	{
		if (curStub < MainGame.playerVehicles[vhID].numWeapons)
		{
			mainC.weaponsMain.Load_Weapon_Into_Player_Vehicle_Weapon_Stub(vhID, curStub, weaponID, amount);
		}
	}

	public void Remove_Weapon_From_Player_Vehicle(ushort vhID, byte curStub)
	{
		if (curStub < MainGame.playerVehicles[vhID].numWeapons)
		{
			MainGame.playerVehicles[vhID].weapons[curStub].active = false;
		}
	}

	public void Set_Mount_Weapon(ushort vhID, byte mountID, byte newStub)
	{
		if (mountID < MainGame.playerVehicles[vhID].numMounts && MainGame.playerVehicles[vhID].mounts[mountID].type == 1 && newStub < MainGame.playerVehicles[vhID].numWeapons)
		{
			MainGame.playerVehicles[vhID].mounts[mountID].objectAttached = 1;
			MainGame.playerVehicles[vhID].mounts[mountID].objectID = newStub;
			if (mountID == MainGame.primaryWeaponMount)
			{
				mainC.gameLogic.Game_Vehicle_Primary_Mount_Weapon_Changed(vhID);
			}
			if (vhID == 0 && MainGame.gameMode == 1)
			{
				mainC.maingameMain.Send_Special_Messages(2);
			}
		}
	}

	public void Set_Mount_Weapon_Stub(ushort vhID, byte mountID, byte newStub)
	{
		if (mountID < MainGame.playerVehicles[vhID].numMounts && MainGame.playerVehicles[vhID].mounts[mountID].type == 1 && newStub < MainGame.playerVehicles[vhID].numWeapons)
		{
			MainGame.playerVehicles[vhID].mounts[mountID].objectAttached = 1;
			MainGame.playerVehicles[vhID].mounts[mountID].objectID = newStub;
		}
	}

	public byte Get_Player_Vehicle_Stub_Containing_Weapon(ushort playerID, ushort weaponID)
	{
		byte numWeapons = MainGame.playerVehicles[playerID].numWeapons;
		for (byte b = 0; b < numWeapons; b++)
		{
			if (MainGame.playerVehicles[playerID].weapons[b].active && MainGame.playerVehicles[playerID].weapons[b].weaponID == weaponID)
			{
				return b;
			}
		}
		return 0;
	}

	public bool Does_Player_Vehicle_Have_Weapon(ushort playerID, ushort weaponID)
	{
		byte numWeapons = MainGame.playerVehicles[playerID].numWeapons;
		for (byte b = 0; b < numWeapons; b++)
		{
			if (MainGame.playerVehicles[playerID].weapons[b].active && MainGame.playerVehicles[playerID].weapons[b].weaponID == weaponID)
			{
				return true;
			}
		}
		return false;
	}

	public bool Get_Player_Vehicle_Weapon_Id(ushort playerID, ushort mountID, out ushort weaponID)
	{
		weaponID = 0;
		if (mountID >= MainGame.playerVehicles[playerID].numMounts || MainGame.playerVehicles[playerID].mounts[mountID].type != 1 || MainGame.playerVehicles[playerID].mounts[mountID].objectAttached != 1 || !MainGame.playerVehicles[playerID].weapons[MainGame.playerVehicles[playerID].mounts[mountID].objectID].active)
		{
			return false;
		}
		weaponID = MainGame.playerVehicles[playerID].weapons[MainGame.playerVehicles[playerID].mounts[mountID].objectID].weaponID;
		return true;
	}

	public bool Player_Vehicle_Stub_Has_Weapon(ushort vhID, byte curStub)
	{
		return MainGame.playerVehicles[vhID].weapons[curStub].active;
	}

	public bool Player_Vehicle_Has_Available_Weapon_Stub(ushort vhID, out byte stubID)
	{
		for (byte b = 0; b < MainGame.playerVehicles[vhID].numWeapons; b++)
		{
			if (!MainGame.playerVehicles[vhID].weapons[b].active)
			{
				stubID = b;
				return true;
			}
		}
		stubID = 0;
		return false;
	}

	public byte Get_Player_Vehicle_Primary_Weapon_Mount_Stub(ushort vhID)
	{
		return MainGame.playerVehicles[vhID].mounts[MainGame.primaryWeaponMount].objectID;
	}

	public void Splash_Damage_From_Vehicle_Explosion(float startX, float startY, float startZ, ushort vehicleID, short playerCausingExplosion, byte threadID)
	{
		ushort num = 0;
		ushort num2 = 0;
		ushort num3 = 0;
		float num4 = 0f;
		Vector3 InitialRayStart = default(Vector3);
		Vector3 InitialRayEnd = default(Vector3);
		Vector3 IntersectPosition = default(Vector3);
		Vector3 IntersectNormal = default(Vector3);
		float num5 = vehicles[vehicleID].splashDamageFalloff * vehicles[vehicleID].splashDamageFalloff;
		InitialRayStart.X = startX;
		InitialRayStart.Y = startY;
		InitialRayStart.Z = startZ;
		ushort returnValueZoneCheckObjID;
		float distance;
		int Number;
		for (byte b = 0; b < MainGame.maxGamePlayers; b++)
		{
			if (global::Players.Players.players[b].onmap == 4 && !global::Players.Players.players[b].dead)
			{
				float num6 = global::Players.Players.players[b].charP.position.v[0] - startX;
				float num7 = global::Players.Players.players[b].charP.position.v[1] - startY;
				float num8 = global::Players.Players.players[b].charP.position.v[2] + 25f - startZ;
				float num9 = num6 * num6 + num7 * num7 + num8 * num8;
				if (num9 < num5)
				{
					int num10 = 0;
					num4 = num9;
					short returnValueZoneCheckIndex = 0;
					InitialRayEnd.X = global::Players.Players.players[b].charP.position.v[0];
					InitialRayEnd.Y = global::Players.Players.players[b].charP.position.v[1];
					InitialRayEnd.Z = global::Players.Players.players[b].charP.position.v[2];
					while (num10 == 0 && mainC.zonesMain.Check_Zones_For_Point(startX, startY, startZ, 5f, returnValueZoneCheckIndex, 1, out returnValueZoneCheckIndex, out returnValueZoneCheckObjID))
					{
						ushort numObjects = Zones.zones[returnValueZoneCheckObjID].zoneList.numObjects;
						for (num2 = 0; num2 < numObjects; num2++)
						{
							if (mainC.collisionMain.Check_Polygon_Ray_Collision_Projectile(Zones.zones[returnValueZoneCheckObjID].zoneList.oList[num2], -1, ref InitialRayStart, ref InitialRayEnd, ref Zones.zones[returnValueZoneCheckObjID].zoneList.matrixList[num2], out distance, out IntersectPosition, out IntersectNormal, out Number, threadID) && distance * distance < num4)
							{
								num10 = 8;
								num2 = numObjects;
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
						num11 = (1f - num9) * vehicles[vehicleID].explosionImpactForce;
						num9 = (1f - num9) * vehicles[vehicleID].explosionDamage;
						num9 /= global::Players.Players.players[b].jt1[global::Players.Players.players[b].humanoidBackJoint].damageMultiplier;
						global::Players.Players.players[b].impactX += num11 * num6;
						global::Players.Players.players[b].impactY += num11 * num7;
						global::Players.Players.players[b].impactZ += num11 * num8;
						global::Players.Players.players[b].deathFlyBackPercentage = 1.5f;
						mainC.playersMain.Player_Hit(b, playerCausingExplosion, -1, num9, -1, global::Weapons.Weapons.pfbV2T[threadID], threadID);
						if (playerCausingExplosion == 0 && (global::Players.Players.players[b].teamMask & global::Players.Players.enemyTeamMask) != 0)
						{
							global::Weapons.Weapons.showTargetCrosshairTimer = 0.25f;
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
						num9 = vehicles[vehicleID].explosionDamage;
						byte teamID = mainC.gameobjectMain.Game_Object_Shot((ushort)playerCausingExplosion, num12, num9, isExplosion: true, threadID);
						if ((mainC.playersMain.Get_Team_Mask(teamID) & global::Players.Players.players[0].teamMask) == 0 && playerCausingExplosion == 0)
						{
							global::Weapons.Weapons.showTargetCrosshairTimer = 0.25f;
						}
					}
				}
			}
		}
	}
}
