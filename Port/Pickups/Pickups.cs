using System;
using System.Globalization;
using System.IO;
using InputHandler;
using MainGame;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using Microsoft.Xna.Framework.Net;
using Networking;
using Physics;
using Players;
using Rendering;
using Structs;
using Weapons;
using WindowsGame1;

namespace Pickups;

public class Pickups
{
	public static bool receivedMPData;

	public static bool receivedPickupData;

	public static bool receivedPickupWeaponData;

	public static bool playerPickupWeaponEnabled;

	public static bool playerPickingUp;

	public static byte ballisticReturnVarB1;

	public static short ballisticReturnVarS1;

	public static short numPickups = 0;

	public static short numAllocatedPickups = 0;

	public static short numBallisticPickups = 0;

	public static short numAllocatedBallisticPickups = 0;

	public static float cosine;

	public static float cosineTime;

	public static float cosinePeriod;

	public static float timeModifier;

	public static float ballisticReturnVarF1;

	public static float enabledReturnVarF1;

	public static float enabledReturnVarF2;

	public static float enabledReturnVarF3;

	public static float enabledReturnVarF4;

	public static float[] em = new float[4] { 0f, 0f, 0f, 1f };

	public static StructsClass.Pickups[] pick1;

	public static StructsClass.Pickups[] pick2;

	public Game1.MasterCollection mainC;

	public void Init_Vars(Game1.MasterCollection masterC)
	{
		mainC = masterC;
	}

	public void Initialize_Pickups()
	{
		timeModifier = 1f;
		cosinePeriod = 1f;
	}

	public void Load_Pickups_Data(string fileName)
	{
		short num = -1;
		for (int i = 0; i < numPickups; i++)
		{
			pick1[i].refID = -1;
			pick1[i].id2 = 0;
			pick1[i].numModels = 0;
			pick1[i].numFloatVars = 0;
			pick1[i].onmap = false;
			pick1[i].numSounds = 0;
			pick1[i].bool1 = false;
			pick1[i].enabled = true;
			pick1[i].startsOnMap = false;
			pick1[i].startsEnabled = true;
			pick1[i].willRespawn = false;
			pick1[i].bool1 = false;
			pick1[i].b1.v[0] = 0f;
			pick1[i].b1.v[1] = 0f;
			pick1[i].b1.v[2] = 0f;
			pick1[i].b2.v[0] = 0f;
			pick1[i].b2.v[1] = 0f;
			pick1[i].b2.v[2] = 0f;
			pick1[i].offsetX = 0f;
			pick1[i].offsetY = 0f;
			pick1[i].offsetZ = 0f;
			pick1[i].emissive[0] = 0f;
			pick1[i].emissive[1] = 0f;
			pick1[i].emissive[2] = 0f;
			pick1[i].emissive[3] = 1f;
			pick1[i].movementZ = 0f;
			pick1[i].movementRotationX = 0f;
			pick1[i].movementRotationY = 0f;
			pick1[i].movementRotationZ = 0f;
			pick1[i].renderOffsetZ = 0f;
		}
		numPickups = 0;
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
				if (array4[0].Equals("numPickups", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 1;
				}
				else if (array4[0].Equals("Pickup", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 2;
				}
				else if (array4[0].Equals("ActionID", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 3;
				}
				else if (array4[0].Equals("ID", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 4;
				}
				else if (array4[0].Equals("refID", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 5;
				}
				else if (array4[0].Equals("NumModels", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 6;
				}
				else if (array4[0].Equals("Type", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 7;
				}
				else if (array4[0].Equals("OnMap", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 8;
				}
				else if (array4[0].Equals("WillRespawn", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 9;
				}
				else if (array4[0].Equals("RespawnTime", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 10;
				}
				else if (array4[0].Equals("Models", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 11;
				}
				else if (array4[0].Equals("Position", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 12;
				}
				else if (array4[0].Equals("Rotation", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 13;
				}
				else if (array4[0].Equals("Box1", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 14;
				}
				else if (array4[0].Equals("Box2", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 15;
				}
				else if (array4[0].Equals("Floats", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 16;
				}
				else if (array4[0].Equals("NumSounds", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 17;
				}
				else if (array4[0].Equals("Sounds", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 18;
				}
				else if (array4[0].Equals("Boolean", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 19;
				}
				else if (array4[0].Equals("SoundLocation", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 20;
				}
				else if (array4[0].Equals("Enabled", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 21;
				}
				else if (array4[0].Equals("ID2", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 22;
				}
				else if (array4[0].Equals("RotationMovement", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 23;
				}
				else if (array4[0].Equals("Emissive", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 24;
				}
				else if (array4[0].Equals("TranslationMovement", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 25;
				}
				else if (array4[0].Equals("RenderOffsetZ", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 26;
				}
				switch (num4)
				{
				case 1:
				{
					if (array4.Length <= 1)
					{
						break;
					}
					short num5 = short.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
					if (num5 > numAllocatedPickups)
					{
						pick1 = new StructsClass.Pickups[num5];
						for (int i = 0; i < num5; i++)
						{
							pick1[i] = new StructsClass.Pickups();
						}
						numAllocatedPickups = num5;
						for (int i = 0; i < num5; i++)
						{
							pick1[i].actionID = -1;
							pick1[i].id = -1;
							pick1[i].id2 = 0;
							pick1[i].refID = -1;
							pick1[i].numModels = 0;
							pick1[i].numAllocatedModels = 0;
							pick1[i].numAllcoatedFloats = 0;
							pick1[i].numFloatVars = 0;
							pick1[i].onmap = false;
							pick1[i].type = 0;
							pick1[i].startsOnMap = false;
							pick1[i].startsEnabled = true;
							pick1[i].willRespawn = false;
							pick1[i].respawnTime = 0f;
							pick1[i].numSounds = 0;
							pick1[i].numAllocatedSounds = 0;
							pick1[i].bool1 = false;
							pick1[i].enabled = true;
							pick1[i].offsetX = 0f;
							pick1[i].offsetY = 0f;
							pick1[i].offsetZ = 0f;
							pick1[i].emissive = new float[4] { 0f, 0f, 0f, 1f };
							pick1[i].movementZ = 0f;
							pick1[i].movementRotationX = 0f;
							pick1[i].movementRotationY = 0f;
							pick1[i].movementRotationZ = 0f;
							pick1[i].renderOffsetZ = 0f;
						}
					}
					numPickups = num5;
					break;
				}
				case 2:
					num++;
					if (num < 0 || num >= numPickups)
					{
						num = -1;
					}
					break;
				case 3:
					if (array4.Length > 1 && num > -1)
					{
						pick1[num].actionID = short.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 4:
					if (array4.Length > 1 && num > -1)
					{
						pick1[num].id = short.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 5:
					if (array4.Length > 1 && num > -1)
					{
						pick1[num].refID = short.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 6:
					if (array4.Length > 1 && num > -1)
					{
						int numModels = byte.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
						if (numModels > pick1[num].numAllocatedModels)
						{
							pick1[num].modelList = new long[numModels];
							pick1[num].numAllocatedModels = (byte)numModels;
						}
						for (int i = 0; i < numModels; i++)
						{
							pick1[num].modelList[i] = -1L;
						}
						pick1[num].numModels = (byte)numModels;
					}
					break;
				case 7:
					if (array4.Length > 1 && num > -1)
					{
						pick1[num].type = byte.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 8:
					if (array4.Length > 1 && num > -1)
					{
						pick1[num].startsOnMap = false;
						if (short.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat) == 1)
						{
							pick1[num].startsOnMap = true;
						}
					}
					break;
				case 9:
					if (array4.Length > 1 && num > -1)
					{
						pick1[num].willRespawn = false;
						if (short.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat) == 1)
						{
							pick1[num].willRespawn = true;
						}
						pick1[num].startWillRespawn = pick1[num].willRespawn;
					}
					break;
				case 10:
					if (array4.Length > 1 && num > -1)
					{
						pick1[num].respawnTime = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 11:
					if (array4.Length > 1 && num > -1)
					{
						int numModels = pick1[num].numModels;
						for (int i = 0; i < numModels && i < array4.Length - 1; i++)
						{
							pick1[num].modelList[i] = mainC.modelsMain.Find_Model(array4[i + 1]);
						}
					}
					break;
				case 12:
					if (array4.Length > 3 && num > -1)
					{
						pick1[num].position.v[0] = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
						pick1[num].position.v[1] = float.Parse(array4[2], CultureInfo.InvariantCulture.NumberFormat);
						pick1[num].position.v[2] = float.Parse(array4[3], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 13:
					if (array4.Length > 3 && num > -1)
					{
						pick1[num].rotation.v[0] = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
						pick1[num].rotation.v[1] = float.Parse(array4[2], CultureInfo.InvariantCulture.NumberFormat);
						pick1[num].rotation.v[2] = float.Parse(array4[3], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 14:
					if (array4.Length > 3 && num > -1)
					{
						pick1[num].b1.v[0] = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
						pick1[num].b1.v[1] = float.Parse(array4[2], CultureInfo.InvariantCulture.NumberFormat);
						pick1[num].b1.v[2] = float.Parse(array4[3], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 15:
					if (array4.Length > 3 && num > -1)
					{
						pick1[num].b2.v[0] = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
						pick1[num].b2.v[1] = float.Parse(array4[2], CultureInfo.InvariantCulture.NumberFormat);
						pick1[num].b2.v[2] = float.Parse(array4[3], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 16:
				{
					if (array4.Length <= 1 || num <= -1)
					{
						break;
					}
					int numModels = int.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
					if (numModels > pick1[num].numAllcoatedFloats)
					{
						pick1[num].fVar = new float[numModels];
						pick1[num].numFloatVars = (byte)numModels;
					}
					if (array4.Length > numModels + 1)
					{
						for (int i = 0; i < numModels; i++)
						{
							pick1[num].fVar[i] = float.Parse(array4[i + 2], CultureInfo.InvariantCulture.NumberFormat);
						}
					}
					pick1[num].numFloatVars = (byte)numModels;
					break;
				}
				case 17:
					if (array4.Length > 1 && num > -1)
					{
						int numModels = byte.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
						if (numModels > pick1[num].numAllocatedSounds)
						{
							pick1[num].sounds = new string[numModels];
							pick1[num].numAllocatedSounds = (byte)numModels;
						}
						for (int i = 0; i < numModels; i++)
						{
							pick1[num].sounds[i] = "";
						}
						pick1[num].numSounds = (byte)numModels;
					}
					break;
				case 18:
					if (array4.Length > 1 && num > -1)
					{
						int numModels = pick1[num].numSounds;
						for (int i = 0; i < numModels && i < array4.Length - 1; i++)
						{
							pick1[num].sounds[i] = array4[i + 1];
						}
					}
					break;
				case 19:
					if (array4.Length > 1 && num > -1)
					{
						pick1[num].bool1 = bool.Parse(array4[1]);
					}
					break;
				case 20:
					if (array4.Length > 3 && num > -1)
					{
						pick1[num].soundX = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
						pick1[num].soundY = float.Parse(array4[2], CultureInfo.InvariantCulture.NumberFormat);
						pick1[num].soundZ = float.Parse(array4[3], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 21:
					if (array4.Length > 1 && num > -1)
					{
						pick1[num].startsEnabled = false;
						if (short.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat) == 1)
						{
							pick1[num].startsEnabled = true;
						}
					}
					break;
				case 22:
					if (array4.Length > 1 && num > -1)
					{
						pick1[num].id2 = ushort.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 23:
					if (array4.Length > 3 && num > -1)
					{
						pick1[num].movementRotationX = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
						pick1[num].movementRotationY = float.Parse(array4[2], CultureInfo.InvariantCulture.NumberFormat);
						pick1[num].movementRotationZ = float.Parse(array4[3], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 24:
					if (array4.Length > 4 && num > -1)
					{
						pick1[num].emissive[0] = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
						pick1[num].emissive[1] = float.Parse(array4[2], CultureInfo.InvariantCulture.NumberFormat);
						pick1[num].emissive[2] = float.Parse(array4[3], CultureInfo.InvariantCulture.NumberFormat);
						pick1[num].emissive[3] = float.Parse(array4[4], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 25:
					if (array4.Length > 1 && num > -1)
					{
						pick1[num].movementZ = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 26:
					if (array4.Length > 1 && num > -1)
					{
						pick1[num].renderOffsetZ = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				}
			}
		}
		stream.Close();
		if (num < numPickups - 1)
		{
			numPickups = (short)(num + 1);
		}
		for (num = 0; num < numPickups; num++)
		{
			Matrix.CreateTranslation(pick1[num].position.v[0], pick1[num].position.v[1], pick1[num].position.v[2], out pick1[num].mv);
			Matrix.CreateRotationX(pick1[num].rotation.v[0] * ((float)Math.PI / 180f), out var result);
			pick1[num].mv = result * pick1[num].mv;
			Matrix.CreateRotationY(pick1[num].rotation.v[1] * ((float)Math.PI / 180f), out result);
			pick1[num].mv = result * pick1[num].mv;
			Matrix.CreateRotationZ(pick1[num].rotation.v[2] * ((float)Math.PI / 180f), out result);
			pick1[num].mv = result * pick1[num].mv;
			pick1[num].mv2 = pick1[num].mv;
			pick1[num].b1.v[0] += pick1[num].position.v[0];
			pick1[num].b1.v[1] += pick1[num].position.v[1];
			pick1[num].b1.v[2] += pick1[num].position.v[2];
			pick1[num].b2.v[0] += pick1[num].position.v[0];
			pick1[num].b2.v[1] += pick1[num].position.v[1];
			pick1[num].b2.v[2] += pick1[num].position.v[2];
		}
	}

	public void Load_Pickups_Ballistic_Data(string fileName)
	{
		short num = -1;
		for (int i = 0; i < numBallisticPickups; i++)
		{
			pick2[i].refID = -1;
			pick2[i].numModels = 0;
			pick2[i].numFloatVars = 0;
			pick2[i].onmap = false;
			pick2[i].numSounds = 0;
			pick2[i].bool1 = false;
			pick2[i].enabled = true;
			pick2[i].startsOnMap = false;
			pick2[i].startsEnabled = true;
			pick2[i].willRespawn = false;
			pick2[i].bool1 = false;
			pick2[i].b1.v[0] = 0f;
			pick2[i].b1.v[1] = 0f;
			pick2[i].b1.v[2] = 0f;
			pick2[i].b2.v[0] = 0f;
			pick2[i].b2.v[1] = 0f;
			pick2[i].b2.v[2] = 0f;
			pick2[i].offsetX = 0f;
			pick2[i].offsetY = 0f;
			pick2[i].offsetZ = 0f;
		}
		numBallisticPickups = 0;
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
				if (array4[0].Equals("numBallisticPickups", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 1;
				}
				else if (array4[0].Equals("Pickup", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 2;
				}
				else if (array4[0].Equals("ActionID", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 3;
				}
				else if (array4[0].Equals("ID", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 4;
				}
				else if (array4[0].Equals("refID", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 5;
				}
				else if (array4[0].Equals("NumModels", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 6;
				}
				else if (array4[0].Equals("Type", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 7;
				}
				else if (array4[0].Equals("OnMap", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 8;
				}
				else if (array4[0].Equals("WillRespawn", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 9;
				}
				else if (array4[0].Equals("RespawnTime", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 10;
				}
				else if (array4[0].Equals("Models", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 11;
				}
				else if (array4[0].Equals("Position", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 12;
				}
				else if (array4[0].Equals("Rotation", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 13;
				}
				else if (array4[0].Equals("Box1", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 14;
				}
				else if (array4[0].Equals("Box2", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 15;
				}
				else if (array4[0].Equals("Floats", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 16;
				}
				else if (array4[0].Equals("NumSounds", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 17;
				}
				else if (array4[0].Equals("Sounds", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 18;
				}
				else if (array4[0].Equals("Boolean", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 19;
				}
				else if (array4[0].Equals("SoundLocation", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 20;
				}
				else if (array4[0].Equals("Enabled", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 21;
				}
				switch (num4)
				{
				case 1:
				{
					if (array4.Length <= 1)
					{
						break;
					}
					short num6 = short.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
					if (num6 > numAllocatedBallisticPickups)
					{
						pick2 = new StructsClass.Pickups[num6];
						for (int i = 0; i < num6; i++)
						{
							pick2[i] = new StructsClass.Pickups();
						}
						numAllocatedBallisticPickups = num6;
						for (int i = 0; i < num6; i++)
						{
							pick2[i].actionID = -1;
							pick2[i].id = -1;
							pick2[i].refID = -1;
							pick2[i].numModels = 0;
							pick2[i].numAllocatedModels = 0;
							pick2[i].numAllcoatedFloats = 0;
							pick2[i].numFloatVars = 0;
							pick2[i].onmap = false;
							pick2[i].type = 0;
							pick2[i].startsOnMap = false;
							pick2[i].startsEnabled = true;
							pick2[i].willRespawn = false;
							pick2[i].respawnTime = 0f;
							pick2[i].numSounds = 0;
							pick2[i].numAllocatedSounds = 0;
							pick2[i].bool1 = false;
							pick2[i].enabled = true;
							pick2[i].offsetX = 0f;
							pick2[i].offsetY = 0f;
							pick2[i].offsetZ = 0f;
						}
					}
					numBallisticPickups = num6;
					break;
				}
				case 2:
					num++;
					if (num < 0 || num >= numBallisticPickups)
					{
						num = -1;
					}
					break;
				case 3:
					if (array4.Length > 1 && num > -1)
					{
						pick2[num].actionID = short.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 4:
					if (array4.Length > 1 && num > -1)
					{
						pick2[num].id = short.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 5:
					if (array4.Length > 1 && num > -1)
					{
						pick2[num].refID = short.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 6:
					if (array4.Length > 1 && num > -1)
					{
						int num5 = byte.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
						if (num5 > pick2[num].numAllocatedModels)
						{
							pick2[num].modelList = new long[num5];
							pick2[num].numAllocatedModels = (byte)num5;
						}
						for (int i = 0; i < num5; i++)
						{
							pick2[num].modelList[i] = -1L;
						}
						pick2[num].numModels = (byte)num5;
					}
					break;
				case 7:
					if (array4.Length > 1 && num > -1)
					{
						pick2[num].type = byte.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 8:
					if (array4.Length > 1 && num > -1)
					{
						pick2[num].startsOnMap = false;
						if (short.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat) == 1)
						{
							pick2[num].startsOnMap = true;
						}
					}
					break;
				case 9:
					if (array4.Length > 1 && num > -1)
					{
						pick2[num].willRespawn = false;
						if (short.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat) == 1)
						{
							pick2[num].willRespawn = true;
						}
					}
					break;
				case 10:
					if (array4.Length > 1 && num > -1)
					{
						pick2[num].respawnTime = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 11:
					if (array4.Length > 1 && num > -1)
					{
						int num5 = pick2[num].numModels;
						for (int i = 0; i < num5 && i < array4.Length - 1; i++)
						{
							pick2[num].modelList[i] = mainC.modelsMain.Find_Model(array4[i + 1]);
						}
					}
					break;
				case 12:
					if (array4.Length > 3 && num > -1)
					{
						pick2[num].position.v[0] = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
						pick2[num].position.v[1] = float.Parse(array4[2], CultureInfo.InvariantCulture.NumberFormat);
						pick2[num].position.v[2] = float.Parse(array4[3], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 13:
					if (array4.Length > 3 && num > -1)
					{
						pick2[num].rotation.v[0] = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
						pick2[num].rotation.v[1] = float.Parse(array4[2], CultureInfo.InvariantCulture.NumberFormat);
						pick2[num].rotation.v[2] = float.Parse(array4[3], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 14:
					if (array4.Length > 3 && num > -1)
					{
						pick2[num].b1.v[0] = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
						pick2[num].b1.v[1] = float.Parse(array4[2], CultureInfo.InvariantCulture.NumberFormat);
						pick2[num].b1.v[2] = float.Parse(array4[3], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 15:
					if (array4.Length > 3 && num > -1)
					{
						pick2[num].b2.v[0] = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
						pick2[num].b2.v[1] = float.Parse(array4[2], CultureInfo.InvariantCulture.NumberFormat);
						pick2[num].b2.v[2] = float.Parse(array4[3], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 16:
				{
					if (array4.Length <= 1 || num <= -1)
					{
						break;
					}
					int num5 = int.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
					if (num5 > pick2[num].numAllcoatedFloats)
					{
						pick2[num].fVar = new float[num5];
						pick2[num].numFloatVars = (byte)num5;
					}
					if (array4.Length > num5 + 1)
					{
						for (int i = 0; i < num5; i++)
						{
							pick2[num].fVar[i] = float.Parse(array4[i + 2], CultureInfo.InvariantCulture.NumberFormat);
						}
					}
					pick2[num].numFloatVars = (byte)num5;
					break;
				}
				case 17:
					if (array4.Length > 1 && num > -1)
					{
						int num5 = byte.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
						if (num5 > pick2[num].numAllocatedSounds)
						{
							pick2[num].sounds = new string[num5];
							pick2[num].numAllocatedSounds = (byte)num5;
						}
						for (int i = 0; i < num5; i++)
						{
							pick2[num].sounds[i] = "";
						}
						pick2[num].numSounds = (byte)num5;
					}
					break;
				case 18:
					if (array4.Length > 1 && num > -1)
					{
						int num5 = pick2[num].numSounds;
						for (int i = 0; i < num5 && i < array4.Length - 1; i++)
						{
							pick2[num].sounds[i] = array4[i + 1];
						}
					}
					break;
				case 19:
					if (array4.Length > 1 && num > -1)
					{
						pick2[num].bool1 = bool.Parse(array4[1]);
					}
					break;
				case 20:
					if (array4.Length > 3 && num > -1)
					{
						pick2[num].soundX = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
						pick2[num].soundY = float.Parse(array4[2], CultureInfo.InvariantCulture.NumberFormat);
						pick2[num].soundZ = float.Parse(array4[3], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 21:
					if (array4.Length > 1 && num > -1)
					{
						pick2[num].startsEnabled = false;
						if (short.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat) == 1)
						{
							pick2[num].startsEnabled = true;
						}
					}
					break;
				}
			}
		}
		stream.Close();
		if (num < numBallisticPickups - 1)
		{
			numBallisticPickups = (short)(num + 1);
		}
		for (num = 0; num < numBallisticPickups; num++)
		{
			Matrix.CreateTranslation(pick2[num].position.v[0], pick2[num].position.v[1], pick2[num].position.v[2], out pick2[num].mv);
			Matrix.CreateRotationX(pick2[num].rotation.v[0] * ((float)Math.PI / 180f), out var result);
			pick2[num].mv = result * pick2[num].mv;
			Matrix.CreateRotationY(pick2[num].rotation.v[1] * ((float)Math.PI / 180f), out result);
			pick2[num].mv = result * pick2[num].mv;
			Matrix.CreateRotationZ(pick2[num].rotation.v[2] * ((float)Math.PI / 180f), out result);
			pick2[num].mv = result * pick2[num].mv;
			pick2[num].b1.v[0] += pick2[num].position.v[0];
			pick2[num].b1.v[1] += pick2[num].position.v[1];
			pick2[num].b1.v[2] += pick2[num].position.v[2];
			pick2[num].b2.v[0] += pick2[num].position.v[0];
			pick2[num].b2.v[1] += pick2[num].position.v[1];
			pick2[num].b2.v[2] += pick2[num].position.v[2];
		}
	}

	public void Process_Pickups_MP(byte threadID)
	{
		short num = 0;
		Calculate_Cosine();
		playerPickupWeaponEnabled = false;
		if (global::Players.Players.players[0].onmap != 4)
		{
			return;
		}
		for (short num2 = 0; num2 < numPickups; num2++)
		{
			if (pick1[num2].onmap)
			{
				byte type = pick1[num2].type;
				if (type == 12)
				{
					float num3 = (pick1[num2].fVar[0] + pick1[num2].fVar[3]) * global::MainGame.MainGame.frametime;
					float num4 = (pick1[num2].fVar[1] + pick1[num2].fVar[4]) * global::MainGame.MainGame.frametime;
					float num5 = (pick1[num2].fVar[2] + pick1[num2].fVar[5]) * global::MainGame.MainGame.frametime;
					pick1[num2].mv.M41 += num3;
					pick1[num2].mv.M42 += num4;
					pick1[num2].mv.M43 += num5;
					pick1[num2].b1.v[0] += num3;
					pick1[num2].b1.v[1] += num4;
					pick1[num2].b1.v[2] += num5;
					pick1[num2].b2.v[0] += num3;
					pick1[num2].b2.v[1] += num4;
					pick1[num2].b2.v[2] += num5;
					pick1[num2].offsetX += num3;
					pick1[num2].offsetY += num4;
					pick1[num2].offsetZ += num5;
					if (Math.Abs(pick1[num2].offsetX) > pick1[num2].fVar[6] || Math.Abs(pick1[num2].offsetY) > pick1[num2].fVar[7] || Math.Abs(pick1[num2].offsetZ) > pick1[num2].fVar[8])
					{
						pick1[num2].b1.v[0] -= pick1[num2].offsetX;
						pick1[num2].b1.v[1] -= pick1[num2].offsetY;
						pick1[num2].b1.v[2] -= pick1[num2].offsetZ;
						pick1[num2].b2.v[0] -= pick1[num2].offsetX;
						pick1[num2].b2.v[1] -= pick1[num2].offsetY;
						pick1[num2].b2.v[2] -= pick1[num2].offsetZ;
						pick1[num2].offsetX = 0f;
						pick1[num2].offsetY = 0f;
						pick1[num2].offsetZ = 0f;
						pick1[num2].mv.M41 = pick1[num2].position.v[0];
						pick1[num2].mv.M42 = pick1[num2].position.v[1];
						pick1[num2].mv.M43 = pick1[num2].position.v[2];
					}
				}
				if (global::Players.Players.players[num].charP.position.v[0] > pick1[num2].b1.v[0] && global::Players.Players.players[num].charP.position.v[0] < pick1[num2].b2.v[0] && global::Players.Players.players[num].charP.position.v[1] > pick1[num2].b1.v[1] && global::Players.Players.players[num].charP.position.v[1] < pick1[num2].b2.v[1] && global::Players.Players.players[num].charP.position.v[2] > pick1[num2].b1.v[2] && global::Players.Players.players[num].charP.position.v[2] < pick1[num2].b2.v[2])
				{
					bool flag = false;
					ushort weaponID;
					switch (pick1[num2].type)
					{
					case 1:
					case 17:
					{
						if (mainC.vehicles.Does_Player_Vehicle_Have_Weapon(0, (ushort)pick1[num2].refID))
						{
							break;
						}
						if (mainC.vehicles.Player_Vehicle_Has_Available_Weapon_Stub(0, out var stubID))
						{
							mainC.vehicles.Add_Weapon_To_Player_Vehicle_Stub(0, stubID, (byte)pick1[num2].refID, 1);
							mainC.gameLogic.Game_New_Weapon_Picked_Up(stubID);
							pick1[num2].onmap = false;
							pick1[num2].changed = true;
							flag = true;
							if (pick1[num2].numSounds != 0)
							{
								mainC.soundsMain.Play_Priority_Sound(pick1[num2].sounds[0], global::Players.Players.players[0].charP.position.v[0], global::Players.Players.players[0].charP.position.v[1], global::Players.Players.players[0].charP.position.v[2], 0f, 0f, 0f);
							}
							break;
						}
						playerPickupWeaponEnabled = true;
						if (playerPickingUp)
						{
							pick1[num2].changed = true;
							playerPickingUp = false;
							short refID = pick1[num2].refID;
							if (mainC.vehicles.Get_Player_Vehicle_Weapon_Id(0, global::MainGame.MainGame.primaryWeaponMount, out weaponID))
							{
								pick1[num2].refID = (short)weaponID;
								pick1[num2].modelList[0] = global::Weapons.Weapons.wp1[weaponID].modelID;
								global::Networking.Networking.networkShorts[0] = (short)weaponID;
								global::Networking.Networking.networkUShorts[0] = (ushort)num2;
								mainC.networkingMain.XBOX_Send_Network_Message75(75);
							}
							else
							{
								pick1[num2].onmap = false;
								flag = true;
							}
							mainC.vehicles.Add_Weapon_To_Player_Vehicle_Mount(0, global::MainGame.MainGame.primaryWeaponMount, (byte)refID, 1);
							mainC.gameLogic.Game_Vehicle_Primary_Mount_Weapon_Changed(0);
							mainC.maingameMain.Send_Special_Messages(2);
							if (pick1[num2].numSounds != 0)
							{
								mainC.soundsMain.Play_Priority_Sound(pick1[num2].sounds[0], global::Players.Players.players[0].charP.position.v[0], global::Players.Players.players[0].charP.position.v[1], global::Players.Players.players[0].charP.position.v[2], 0f, 0f, 0f);
							}
						}
						break;
					}
					case 2:
						if (global::Players.Players.players[num].damage > 0f)
						{
							mainC.playersMain.Adjust_Player_Damage_By_Percent((ushort)num, pick1[num2].fVar[0], sendOnline: true);
							pick1[num2].onmap = false;
							pick1[num2].changed = true;
							if (pick1[num2].numSounds != 0)
							{
								mainC.soundsMain.Play_Priority_Sound(pick1[num2].sounds[0], global::Players.Players.players[0].charP.position.v[0], global::Players.Players.players[0].charP.position.v[1], global::Players.Players.players[0].charP.position.v[2], 0f, 0f, 0f);
							}
							pick1[num2].timeBeforeRespawn = pick1[num2].respawnTime;
							flag = true;
						}
						break;
					case 3:
					{
						short refID = mainC.weaponsMain.Count_Active_Weapons_Matching_Clip(0, (byte)pick1[num2].refID);
						if (mainC.weaponsMain.Add_Ammo_Clip((byte)pick1[num2].refID, (byte)(pick1[num2].fVar[0] * (float)refID), 0))
						{
							global::Players.Players.reloading = mainC.playersMain.Player_Needs_To_Reload(0);
							mainC.weaponsMain.Check_Weapon_Views();
							pick1[num2].onmap = false;
							pick1[num2].changed = true;
							if (pick1[num2].numSounds != 0)
							{
								mainC.soundsMain.Play_Priority_Sound(pick1[num2].sounds[0], global::Players.Players.players[0].charP.position.v[0], global::Players.Players.players[0].charP.position.v[1], global::Players.Players.players[0].charP.position.v[2], 0f, 0f, 0f);
							}
							pick1[num2].timeBeforeRespawn = pick1[num2].respawnTime;
							flag = true;
						}
						break;
					}
					case 4:
						mainC.soundsMain.Play_Narrator_Voice(pick1[num2].sounds[0]);
						pick1[num2].onmap = false;
						pick1[num2].changed = true;
						break;
					case 5:
						mainC.soundsMain.Play_Voice(pick1[num2].sounds[0], pick1[num2].position.v[0], pick1[num2].position.v[1], pick1[num2].position.v[2], 0f, 0f, 0f);
						pick1[num2].onmap = false;
						pick1[num2].changed = true;
						break;
					case 6:
						if (pick1[num2].numSounds != 0)
						{
							mainC.soundsMain.Play_Priority_Sound(pick1[num2].sounds[0], global::Players.Players.players[0].charP.position.v[0], global::Players.Players.players[0].charP.position.v[1], global::Players.Players.players[0].charP.position.v[2], 0f, 0f, 0f);
						}
						pick1[num2].onmap = false;
						pick1[num2].changed = true;
						if (global::Networking.Networking.isHost)
						{
							mainC.networkingMain.XBOX_MP_Round_Over();
						}
						else
						{
							mainC.networkingMain.XBOX_Send_Network_Message_To_Host(81);
						}
						break;
					case 7:
						mainC.soundsMain.Change_Level_Music((byte)pick1[num2].refID, playNow: true);
						pick1[num2].onmap = false;
						pick1[num2].changed = true;
						break;
					case 8:
						mainC.programsMain.Run_Program_Basic(pick1[num2].refID, pick1[num2].bool1, 0, 0);
						pick1[num2].onmap = false;
						pick1[num2].changed = true;
						flag = true;
						break;
					case 10:
						pick1[num2].onmap = mainC.gameLogic.Game_Points_Pickup(pick1[num2].actionID, pick1[num2].id, pick1[num2].refID, (ushort)num, pick1[num2].bool1);
						if (pick1[num2].onmap != pick1[num2].startsOnMap)
						{
							pick1[num2].changed = true;
						}
						if (pick1[num2].numSounds != 0)
						{
							mainC.soundsMain.Play_Sound_NonPositional(pick1[num2].sounds[0]);
						}
						pick1[num2].timeBeforeRespawn = pick1[num2].respawnTime;
						flag = true;
						break;
					case 11:
						if (pick1[num2 + pick1[num2].id].enabled)
						{
							pick1[num2].bool1 = true;
							mainC.gameLogic.Game_Lap_Checkpoint(pick1[num2].actionID, pick1[num2].id, pick1[num2].refID, pick1[num2].bool1, threadID);
							mainC.renderingMain.Highlight_MiniMap_Item(pick1[num2 + pick1[num2].refID].id2);
							mainC.renderingMain.Remove_Highlight_MiniMap_Item(pick1[num2].id2);
							pick1[num2].enabled = true;
							pick1[num2 + pick1[num2].id].enabled = false;
							if (pick1[num2].numSounds > 1 && pick1[num2].timeBeforeRespawn > 0f)
							{
								mainC.soundsMain.Play_Priority_Sound(pick1[num2].sounds[1], global::Players.Players.players[0].charP.position.v[0], global::Players.Players.players[0].charP.position.v[1], global::Players.Players.players[0].charP.position.v[2], 0f, 0f, 0f);
							}
						}
						else if (!pick1[num2].enabled)
						{
							pick1[num2].bool1 = false;
							mainC.gameLogic.Game_Lap_Checkpoint(pick1[num2 + pick1[num2].refID].actionID, pick1[num2].id, pick1[num2].refID, pick1[num2].bool1, threadID);
							if (pick1[num2].numSounds != 0 && pick1[num2].timeBeforeRespawn > 0f)
							{
								mainC.soundsMain.Play_Priority_Sound(pick1[num2].sounds[0], global::Players.Players.players[0].charP.position.v[0], global::Players.Players.players[0].charP.position.v[1], global::Players.Players.players[0].charP.position.v[2], 0f, 0f, 0f);
							}
						}
						pick1[num2].timeBeforeRespawn = 0f;
						pick1[num2 + pick1[num2].id].timeBeforeRespawn = 1f;
						pick1[num2 + pick1[num2].refID].timeBeforeRespawn = 1f;
						break;
					case 12:
						pick1[num2].onmap = mainC.gameLogic.Game_Points_Pickup(pick1[num2].actionID, pick1[num2].id, pick1[num2].refID, (ushort)num, pick1[num2].bool1);
						if (pick1[num2].onmap != pick1[num2].startsOnMap)
						{
							pick1[num2].changed = true;
						}
						if (pick1[num2].numSounds != 0)
						{
							mainC.soundsMain.Play_Sound_NonPositional(pick1[num2].sounds[0]);
						}
						pick1[num2].timeBeforeRespawn = pick1[num2].respawnTime;
						flag = true;
						break;
					case 13:
						mainC.weaponsMain.Add_Weapon_Modifier((ushort)num, (byte)pick1[num2].actionID);
						pick1[num2].onmap = false;
						pick1[num2].changed = true;
						if (pick1[num2].numSounds != 0)
						{
							mainC.soundsMain.Play_Priority_Sound(pick1[num2].sounds[0], global::Players.Players.players[0].charP.position.v[0], global::Players.Players.players[0].charP.position.v[1], global::Players.Players.players[0].charP.position.v[2], 0f, 0f, 0f);
						}
						pick1[num2].timeBeforeRespawn = pick1[num2].respawnTime;
						flag = true;
						break;
					case 14:
						if (mainC.gameLogic.Game_Pickup_Type_14_MP(pick1[num2].id, pick1[num2].id2, pick1[num2].refID, pick1[num2].actionID))
						{
							pick1[num2].onmap = false;
							pick1[num2].changed = true;
							if (pick1[num2].numSounds != 0)
							{
								mainC.soundsMain.Play_Priority_Sound(pick1[num2].sounds[0], global::Players.Players.players[0].charP.position.v[0], global::Players.Players.players[0].charP.position.v[1], global::Players.Players.players[0].charP.position.v[2], 0f, 0f, 0f);
							}
							pick1[num2].timeBeforeRespawn = pick1[num2].respawnTime;
							flag = true;
						}
						break;
					case 15:
						if (mainC.maingameMain.Can_Achievement_Reward_Be_Acquired((byte)pick1[num2].refID))
						{
							mainC.maingameMain.Enable_Achievement_Reward((byte)pick1[num2].refID);
							pick1[num2].onmap = false;
							pick1[num2].changed = true;
							if (pick1[num2].numSounds != 0)
							{
								mainC.soundsMain.Play_Priority_Sound(pick1[num2].sounds[0], global::Players.Players.players[0].charP.position.v[0], global::Players.Players.players[0].charP.position.v[1], global::Players.Players.players[0].charP.position.v[2], 0f, 0f, 0f);
							}
							pick1[num2].timeBeforeRespawn = pick1[num2].respawnTime;
							flag = true;
						}
						break;
					case 16:
					{
						bool flag2 = false;
						for (short refID = 0; refID < pick1[num2].numFloatVars; refID++)
						{
							if (mainC.weaponsMain.Add_Ammo_Clip_For_All_Player_Vehicle_Weapons(0, (byte)pick1[num2].refID, (byte)pick1[num2].fVar[refID]))
							{
								flag2 = true;
							}
						}
						if (flag2)
						{
							global::Players.Players.reloading = mainC.playersMain.Player_Needs_To_Reload(0);
							pick1[num2].onmap = false;
							pick1[num2].changed = true;
							if (pick1[num2].numSounds != 0)
							{
								mainC.soundsMain.Play_Priority_Sound(pick1[num2].sounds[0], global::Players.Players.players[0].charP.position.v[0], global::Players.Players.players[0].charP.position.v[1], global::Players.Players.players[0].charP.position.v[2], 0f, 0f, 0f);
							}
							pick1[num2].timeBeforeRespawn = pick1[num2].respawnTime;
							flag = true;
						}
						break;
					}
					case 18:
						for (weaponID = 0; weaponID < numPickups; weaponID++)
						{
							if (pick1[weaponID].type == 18 && pick1[weaponID].id == pick1[num2].id)
							{
								pick1[weaponID].onmap = true;
							}
						}
						pick1[num2].onmap = false;
						pick1[num2].changed = true;
						pick1[num2].timeBeforeRespawn = pick1[num2].respawnTime;
						mainC.maingameMain.Set_Spawn_Points_Active_Status((ushort)pick1[num2].fVar[0], (ushort)pick1[num2].fVar[1], status: false);
						mainC.maingameMain.Set_Spawn_Points_Active_Status((ushort)pick1[num2].fVar[2], (ushort)pick1[num2].fVar[3], status: true);
						flag = true;
						break;
					case 19:
						pick1[num2].onmap = false;
						pick1[num2].changed = true;
						pick1[num2].timeBeforeRespawn = pick1[num2].respawnTime;
						mainC.maingameMain.Trigger_AI_Wave((ushort)pick1[num2].id);
						flag = true;
						break;
					}
					if (flag)
					{
						Send_Pickup_Acquired((ushort)num2);
					}
				}
			}
			else if (pick1[num2].willRespawn && pick1[num2].enabled)
			{
				pick1[num2].timeBeforeRespawn -= global::MainGame.MainGame.frametime * timeModifier;
				if (pick1[num2].timeBeforeRespawn < 0f)
				{
					pick1[num2].onmap = true;
				}
			}
		}
	}

	public void Process_Pickups_MP_Lobby()
	{
		if (!receivedMPData)
		{
			return;
		}
		for (short num = 0; num < numPickups; num++)
		{
			if (!pick1[num].onmap && pick1[num].willRespawn && pick1[num].enabled)
			{
				pick1[num].timeBeforeRespawn -= global::MainGame.MainGame.frametime * timeModifier;
				if (pick1[num].timeBeforeRespawn < 0f)
				{
					pick1[num].onmap = true;
					pick1[num].changed = true;
				}
			}
		}
	}

	public void Process_Pickups_SP(byte threadID)
	{
		Calculate_Cosine();
		playerPickupWeaponEnabled = false;
		if (global::Players.Players.players[0].onmap != 4)
		{
			return;
		}
		for (short num = 0; num < numPickups; num++)
		{
			if (pick1[num].onmap)
			{
				byte type = pick1[num].type;
				if (type == 12)
				{
					float num2 = (pick1[num].fVar[0] + pick1[num].fVar[3]) * global::MainGame.MainGame.frametime;
					float num3 = (pick1[num].fVar[1] + pick1[num].fVar[4]) * global::MainGame.MainGame.frametime;
					float num4 = (pick1[num].fVar[2] + pick1[num].fVar[5]) * global::MainGame.MainGame.frametime;
					pick1[num].mv.M41 += num2;
					pick1[num].mv.M42 += num3;
					pick1[num].mv.M43 += num4;
					pick1[num].b1.v[0] += num2;
					pick1[num].b1.v[1] += num3;
					pick1[num].b1.v[2] += num4;
					pick1[num].b2.v[0] += num2;
					pick1[num].b2.v[1] += num3;
					pick1[num].b2.v[2] += num4;
					pick1[num].offsetX += num2;
					pick1[num].offsetY += num3;
					pick1[num].offsetZ += num4;
					if (Math.Abs(pick1[num].offsetX) > pick1[num].fVar[6] || Math.Abs(pick1[num].offsetY) > pick1[num].fVar[7] || Math.Abs(pick1[num].offsetZ) > pick1[num].fVar[8])
					{
						pick1[num].b1.v[0] -= pick1[num].offsetX;
						pick1[num].b1.v[1] -= pick1[num].offsetY;
						pick1[num].b1.v[2] -= pick1[num].offsetZ;
						pick1[num].b2.v[0] -= pick1[num].offsetX;
						pick1[num].b2.v[1] -= pick1[num].offsetY;
						pick1[num].b2.v[2] -= pick1[num].offsetZ;
						pick1[num].offsetX = 0f;
						pick1[num].offsetY = 0f;
						pick1[num].offsetZ = 0f;
						pick1[num].mv.M41 = pick1[num].position.v[0];
						pick1[num].mv.M42 = pick1[num].position.v[1];
						pick1[num].mv.M43 = pick1[num].position.v[2];
					}
				}
				if (global::Players.Players.players[0].charP.position.v[0] > pick1[num].b1.v[0] && global::Players.Players.players[0].charP.position.v[0] < pick1[num].b2.v[0] && global::Players.Players.players[0].charP.position.v[1] > pick1[num].b1.v[1] && global::Players.Players.players[0].charP.position.v[1] < pick1[num].b2.v[1] && global::Players.Players.players[0].charP.position.v[2] > pick1[num].b1.v[2] && global::Players.Players.players[0].charP.position.v[2] < pick1[num].b2.v[2])
				{
					ushort weaponID;
					switch (pick1[num].type)
					{
					case 1:
					case 17:
					{
						if (mainC.vehicles.Does_Player_Vehicle_Have_Weapon(0, (ushort)pick1[num].refID))
						{
							break;
						}
						if (mainC.vehicles.Player_Vehicle_Has_Available_Weapon_Stub(0, out var stubID))
						{
							mainC.vehicles.Add_Weapon_To_Player_Vehicle_Stub(0, stubID, (byte)pick1[num].refID, 1);
							mainC.gameLogic.Game_New_Weapon_Picked_Up(stubID);
							pick1[num].onmap = false;
							if (pick1[num].numSounds != 0)
							{
								mainC.soundsMain.Play_Priority_Sound(pick1[num].sounds[0], global::Players.Players.players[0].charP.position.v[0], global::Players.Players.players[0].charP.position.v[1], global::Players.Players.players[0].charP.position.v[2], 0f, 0f, 0f);
							}
							break;
						}
						playerPickupWeaponEnabled = true;
						if (playerPickingUp)
						{
							playerPickingUp = false;
							short num5 = pick1[num].refID;
							if (mainC.vehicles.Get_Player_Vehicle_Weapon_Id(0, global::MainGame.MainGame.primaryWeaponMount, out weaponID))
							{
								pick1[num].refID = (short)weaponID;
								pick1[num].modelList[0] = global::Weapons.Weapons.wp1[weaponID].modelID;
							}
							else
							{
								pick1[num].onmap = false;
							}
							mainC.vehicles.Add_Weapon_To_Player_Vehicle_Mount(0, global::MainGame.MainGame.primaryWeaponMount, (byte)num5, 1);
							mainC.gameLogic.Game_Vehicle_Primary_Mount_Weapon_Changed(0);
							if (pick1[num].numSounds != 0)
							{
								mainC.soundsMain.Play_Priority_Sound(pick1[num].sounds[0], global::Players.Players.players[0].charP.position.v[0], global::Players.Players.players[0].charP.position.v[1], global::Players.Players.players[0].charP.position.v[2], 0f, 0f, 0f);
							}
						}
						break;
					}
					case 2:
						if (global::Players.Players.players[0].damage > 0f)
						{
							mainC.playersMain.Adjust_Player_Damage_By_Percent(0, pick1[num].fVar[0], sendOnline: false);
							pick1[num].onmap = false;
							if (pick1[num].numSounds != 0)
							{
								mainC.soundsMain.Play_Priority_Sound(pick1[num].sounds[0], global::Players.Players.players[0].charP.position.v[0], global::Players.Players.players[0].charP.position.v[1], global::Players.Players.players[0].charP.position.v[2], 0f, 0f, 0f);
							}
							pick1[num].timeBeforeRespawn = pick1[num].respawnTime;
						}
						break;
					case 3:
					{
						short num5 = mainC.weaponsMain.Count_Active_Weapons_Matching_Clip(0, (byte)pick1[num].refID);
						if (mainC.weaponsMain.Add_Ammo_Clip((byte)pick1[num].refID, (byte)(pick1[num].fVar[0] * (float)num5), 0))
						{
							global::Players.Players.reloading = mainC.playersMain.Player_Needs_To_Reload(0);
							mainC.weaponsMain.Check_Weapon_Views();
							pick1[num].onmap = false;
							if (pick1[num].numSounds != 0)
							{
								mainC.soundsMain.Play_Priority_Sound(pick1[num].sounds[0], global::Players.Players.players[0].charP.position.v[0], global::Players.Players.players[0].charP.position.v[1], global::Players.Players.players[0].charP.position.v[2], 0f, 0f, 0f);
							}
							pick1[num].timeBeforeRespawn = pick1[num].respawnTime;
						}
						break;
					}
					case 4:
						mainC.soundsMain.Play_Narrator_Voice(pick1[num].sounds[0]);
						pick1[num].onmap = false;
						break;
					case 5:
						mainC.soundsMain.Play_Voice(pick1[num].sounds[0], global::Players.Players.players[0].charP.position.v[0], global::Players.Players.players[0].charP.position.v[1], global::Players.Players.players[0].charP.position.v[2], 0f, 0f, 0f);
						pick1[num].onmap = false;
						break;
					case 6:
						mainC.maingameMain.Set_SP_Level_To_Completed();
						if (pick1[num].numSounds != 0)
						{
							mainC.soundsMain.Play_Priority_Sound(pick1[num].sounds[0], global::Players.Players.players[0].charP.position.v[0], global::Players.Players.players[0].charP.position.v[1], global::Players.Players.players[0].charP.position.v[2], 0f, 0f, 0f);
						}
						break;
					case 7:
						mainC.soundsMain.Change_Level_Music((byte)pick1[num].refID, playNow: true);
						pick1[num].onmap = false;
						break;
					case 8:
						mainC.programsMain.Run_Program_Basic(pick1[num].refID, pick1[num].bool1, 0, 0);
						if (pick1[num].numSounds != 0)
						{
							mainC.soundsMain.Play_Priority_Sound(pick1[num].sounds[0], pick1[num].soundX, pick1[num].soundY, pick1[num].soundZ, 0f, 0f, 0f);
						}
						pick1[num].onmap = false;
						break;
					case 9:
						mainC.aiMain.Trigger_Respawn((byte)pick1[num].refID);
						pick1[num].onmap = false;
						break;
					case 10:
						pick1[num].onmap = mainC.gameLogic.Game_Points_Pickup(pick1[num].actionID, pick1[num].id, pick1[num].refID, 0, pick1[num].bool1);
						if (pick1[num].numSounds != 0)
						{
							mainC.soundsMain.Play_Sound_NonPositional(pick1[num].sounds[0]);
						}
						pick1[num].timeBeforeRespawn = pick1[num].respawnTime;
						break;
					case 11:
						if (pick1[num + pick1[num].id].enabled)
						{
							mainC.gameLogic.Game_Lap_Checkpoint(pick1[num].actionID, pick1[num].id, pick1[num].refID, bool1: true, threadID);
							mainC.renderingMain.Highlight_MiniMap_Item(pick1[num + pick1[num].refID].id2);
							mainC.renderingMain.Remove_Highlight_MiniMap_Item(pick1[num].id2);
							pick1[num].enabled = true;
							pick1[num + pick1[num].id].enabled = false;
							if (pick1[num].numSounds > 1 && pick1[num].timeBeforeRespawn > 0f)
							{
								mainC.soundsMain.Play_Sound_NonPositional(pick1[num].sounds[1]);
							}
						}
						else if (!pick1[num].enabled)
						{
							mainC.gameLogic.Game_Lap_Checkpoint(pick1[num + pick1[num].refID].actionID, pick1[num].id, pick1[num].refID, bool1: false, threadID);
							if (pick1[num].numSounds != 0 && pick1[num].timeBeforeRespawn > 0f)
							{
								mainC.soundsMain.Play_Sound_NonPositional(pick1[num].sounds[0]);
							}
						}
						pick1[num].timeBeforeRespawn = 0f;
						pick1[num + pick1[num].id].timeBeforeRespawn = 1f;
						pick1[num + pick1[num].refID].timeBeforeRespawn = 1f;
						break;
					case 12:
						pick1[num].onmap = mainC.gameLogic.Game_Points_Pickup(pick1[num].actionID, pick1[num].id, pick1[num].refID, 0, pick1[num].bool1);
						if (pick1[num].numSounds != 0)
						{
							mainC.soundsMain.Play_Sound_NonPositional(pick1[num].sounds[0]);
						}
						pick1[num].timeBeforeRespawn = pick1[num].respawnTime;
						break;
					case 13:
						mainC.weaponsMain.Add_Weapon_Modifier(0, (byte)pick1[num].actionID);
						pick1[num].onmap = false;
						if (pick1[num].numSounds != 0)
						{
							mainC.soundsMain.Play_Priority_Sound(pick1[num].sounds[0], global::Players.Players.players[0].charP.position.v[0], global::Players.Players.players[0].charP.position.v[1], global::Players.Players.players[0].charP.position.v[2], 0f, 0f, 0f);
						}
						pick1[num].timeBeforeRespawn = pick1[num].respawnTime;
						break;
					case 14:
						if (mainC.gameLogic.Game_Pickup_Type_14_SP(pick1[num].id, pick1[num].id2, pick1[num].refID, pick1[num].actionID))
						{
							pick1[num].onmap = false;
							if (pick1[num].numSounds != 0)
							{
								mainC.soundsMain.Play_Priority_Sound(pick1[num].sounds[0], global::Players.Players.players[0].charP.position.v[0], global::Players.Players.players[0].charP.position.v[1], global::Players.Players.players[0].charP.position.v[2], 0f, 0f, 0f);
							}
							pick1[num].timeBeforeRespawn = pick1[num].respawnTime;
						}
						break;
					case 15:
						if (mainC.maingameMain.Can_Achievement_Reward_Be_Acquired((byte)pick1[num].refID))
						{
							mainC.maingameMain.Enable_Achievement_Reward((byte)pick1[num].refID);
							pick1[num].onmap = false;
							if (pick1[num].numSounds != 0)
							{
								mainC.soundsMain.Play_Priority_Sound(pick1[num].sounds[0], global::Players.Players.players[0].charP.position.v[0], global::Players.Players.players[0].charP.position.v[1], global::Players.Players.players[0].charP.position.v[2], 0f, 0f, 0f);
							}
							pick1[num].timeBeforeRespawn = pick1[num].respawnTime;
						}
						break;
					case 16:
					{
						bool flag = false;
						for (short num5 = 0; num5 < pick1[num].numFloatVars; num5++)
						{
							if (mainC.weaponsMain.Add_Ammo_Clip_For_All_Player_Vehicle_Weapons(0, (byte)pick1[num].refID, (byte)pick1[num].fVar[num5]))
							{
								flag = true;
							}
						}
						if (flag)
						{
							global::Players.Players.reloading = mainC.playersMain.Player_Needs_To_Reload(0);
							pick1[num].onmap = false;
							if (pick1[num].numSounds != 0)
							{
								mainC.soundsMain.Play_Priority_Sound(pick1[num].sounds[0], global::Players.Players.players[0].charP.position.v[0], global::Players.Players.players[0].charP.position.v[1], global::Players.Players.players[0].charP.position.v[2], 0f, 0f, 0f);
							}
							pick1[num].timeBeforeRespawn = pick1[num].respawnTime;
						}
						break;
					}
					case 18:
						for (weaponID = 0; weaponID < numPickups; weaponID++)
						{
							if (pick1[weaponID].type == 18 && pick1[weaponID].id == pick1[num].id)
							{
								pick1[weaponID].onmap = true;
							}
						}
						pick1[num].onmap = false;
						pick1[num].timeBeforeRespawn = pick1[num].respawnTime;
						mainC.maingameMain.Set_Spawn_Points_Active_Status((ushort)pick1[num].fVar[0], (ushort)pick1[num].fVar[1], status: false);
						mainC.maingameMain.Set_Spawn_Points_Active_Status((ushort)pick1[num].fVar[2], (ushort)pick1[num].fVar[3], status: true);
						break;
					case 19:
						pick1[num].onmap = false;
						pick1[num].timeBeforeRespawn = pick1[num].respawnTime;
						mainC.maingameMain.Trigger_AI_Wave((ushort)pick1[num].id);
						break;
					}
				}
			}
			else if (pick1[num].willRespawn && pick1[num].enabled)
			{
				pick1[num].timeBeforeRespawn -= global::MainGame.MainGame.frametime / global::Physics.Physics.timeMod * timeModifier;
				if (pick1[num].timeBeforeRespawn < 0f)
				{
					pick1[num].onmap = true;
				}
			}
		}
	}

	public void Calculate_Cosine()
	{
		cosineTime += global::MainGame.MainGame.frametime;
		while (cosineTime > cosinePeriod)
		{
			cosineTime -= cosinePeriod;
		}
		cosine = (float)Math.Cos(cosineTime / cosinePeriod * ((float)Math.PI * 2f));
	}

	public bool Check_Pickup_Ballistic(ushort shooterID, byte ammoIndex, float x1, float y1, float z1)
	{
		for (int i = 0; i < numBallisticPickups; i++)
		{
			if (pick2[i].onmap)
			{
				if (!(x1 > pick2[i].b1.v[0]) || !(x1 < pick2[i].b2.v[0]) || !(y1 > pick2[i].b1.v[1]) || !(y1 < pick2[i].b2.v[1]) || !(z1 > pick2[i].b1.v[2]) || !(z1 < pick2[i].b2.v[2]))
				{
					continue;
				}
				switch (pick2[i].type)
				{
				case 0:
				{
					if (!pick2[i].enabled)
					{
						break;
					}
					float num = pick2[i].position.v[0] - x1;
					float num2 = pick2[i].position.v[1] - y1;
					num = num * num + num2 * num2;
					int num3 = pick2[i].numFloatVars / 2;
					int num4 = 0;
					int num5 = 0;
					while (num4 < num3)
					{
						if (num <= pick2[i].fVar[num5])
						{
							ballisticReturnVarB1 = (byte)(num5 / 2);
							ballisticReturnVarF1 = pick2[i].fVar[num5 + 1];
							pick2[i].onmap = pick2[i].bool1;
							pick2[i].timeBeforeRespawn = pick2[i].respawnTime;
							return true;
						}
						num4++;
						num5 += 2;
					}
					break;
				}
				case 1:
					if (pick2[i].enabled)
					{
						ballisticReturnVarS1 = pick2[i].id;
						ballisticReturnVarF1 = mainC.targetMain.Update_Damage_Target((ushort)pick2[i].actionID, mainC.weaponsMain.Get_Damage(ammoIndex, (byte)pick2[i].refID));
						return true;
					}
					break;
				}
			}
			else if (pick2[i].willRespawn && pick2[i].enabled)
			{
				pick2[i].timeBeforeRespawn -= global::MainGame.MainGame.frametime;
				if (pick2[i].timeBeforeRespawn < 0f)
				{
					pick2[i].onmap = true;
				}
			}
		}
		return false;
	}

	public void Reset_Round()
	{
		playerPickingUp = false;
		playerPickupWeaponEnabled = false;
		for (short num = 0; num < numPickups; num++)
		{
			pick1[num].timeBeforeRespawn = pick1[num].respawnTime;
			pick1[num].willRespawn = pick1[num].startWillRespawn;
			pick1[num].onmap = pick1[num].startsOnMap;
			pick1[num].enabled = pick1[num].startsEnabled;
			pick1[num].mv2 = pick1[num].mv;
			pick1[num].curRotX = 0f;
			pick1[num].curRotY = 0f;
			pick1[num].curRotZ = 0f;
			pick1[num].changed = false;
			pick1[num].positionChanged = false;
			switch (pick1[num].type)
			{
			case 1:
				pick1[num].refID = (short)pick1[num].fVar[0];
				pick1[num].modelList[0] = global::Weapons.Weapons.wp1[pick1[num].refID].modelID;
				break;
			case 17:
				pick1[num].refID = (short)pick1[num].fVar[0];
				pick1[num].modelList[0] = global::Weapons.Weapons.wp1[pick1[num].refID].modelID;
				pick1[num].onmap = false;
				break;
			case 12:
				pick1[num].b1.v[0] -= pick1[num].offsetX;
				pick1[num].b1.v[1] -= pick1[num].offsetY;
				pick1[num].b1.v[2] -= pick1[num].offsetZ;
				pick1[num].b2.v[0] -= pick1[num].offsetX;
				pick1[num].b2.v[1] -= pick1[num].offsetY;
				pick1[num].b2.v[2] -= pick1[num].offsetZ;
				pick1[num].offsetX = 0f;
				pick1[num].offsetY = 0f;
				pick1[num].offsetZ = 0f;
				pick1[num].mv.M41 = pick1[num].position.v[0];
				pick1[num].mv.M42 = pick1[num].position.v[1];
				pick1[num].mv.M43 = pick1[num].position.v[2];
				break;
			}
		}
		for (short num = 0; num < numBallisticPickups; num++)
		{
			pick2[num].timeBeforeRespawn = pick2[num].respawnTime;
			pick2[num].onmap = pick2[num].startsOnMap;
			pick2[num].enabled = pick2[num].startsEnabled;
			byte type = pick2[num].type;
			if (type == 12)
			{
				pick2[num].b1.v[0] -= pick2[num].offsetX;
				pick2[num].b1.v[1] -= pick2[num].offsetY;
				pick2[num].b1.v[2] -= pick2[num].offsetZ;
				pick2[num].b2.v[0] -= pick2[num].offsetX;
				pick2[num].b2.v[1] -= pick2[num].offsetY;
				pick2[num].b2.v[2] -= pick2[num].offsetZ;
				pick2[num].offsetX = 0f;
				pick2[num].offsetY = 0f;
				pick2[num].offsetZ = 0f;
				pick2[num].mv.M41 = pick2[num].position.v[0];
				pick2[num].mv.M42 = pick2[num].position.v[1];
				pick2[num].mv.M43 = pick2[num].position.v[2];
			}
		}
	}

	public void Render_Pickups()
	{
		for (int i = 0; i < numPickups; i++)
		{
			if (!pick1[i].onmap)
			{
				continue;
			}
			switch (pick1[i].type)
			{
			case 1:
			case 17:
				global::Rendering.Rendering.effect1.Parameters["Emissive"].SetValue(pick1[i].emissive);
				pick1[i].curRotZ += pick1[i].movementRotationZ * global::MainGame.MainGame.frametime;
				if (Math.Abs(pick1[i].curRotZ) >= 360f)
				{
					pick1[i].curRotZ -= 360f * (float)Math.Sign(pick1[i].curRotZ);
				}
				pick1[i].mv2 = Matrix.CreateTranslation(global::Weapons.Weapons.wp1[pick1[i].refID].centerOfGravityAdjustmentX, global::Weapons.Weapons.wp1[pick1[i].refID].CenterOfGravityAdjustmentY, (1f + cosine) * pick1[i].movementZ) * Matrix.CreateRotationZ(pick1[i].curRotZ * ((float)Math.PI / 180f)) * pick1[i].mv;
				pick1[i].mv2.M43 += pick1[i].renderOffsetZ;
				mainC.weaponsMain.Render_Weapon((ushort)pick1[i].refID, 0, pick1[i].mv2);
				break;
			case 2:
			case 3:
			case 13:
			case 14:
			case 15:
			case 16:
			{
				global::Rendering.Rendering.effect1.Parameters["Emissive"].SetValue(pick1[i].emissive);
				pick1[i].mv2 = Matrix.CreateRotationZ(pick1[i].movementRotationZ * global::MainGame.MainGame.frametime * ((float)Math.PI / 180f)) * pick1[i].mv2;
				pick1[i].mv2.M43 = pick1[i].position.v[2] + (1f + cosine) * pick1[i].movementZ + pick1[i].renderOffsetZ;
				for (int j = 0; j < pick1[i].numModels; j++)
				{
					mainC.modelsMain.Render_Model(pick1[i].modelList[j], ref pick1[i].mv2);
				}
				break;
			}
			case 10:
			case 12:
				pick1[i].mv2 = Matrix.CreateRotationZ(pick1[i].movementRotationZ * global::MainGame.MainGame.frametime * ((float)Math.PI / 180f)) * pick1[i].mv2;
				pick1[i].mv2.M43 += pick1[i].renderOffsetZ;
				global::Rendering.Rendering.effect1.Parameters["World"].SetValue(pick1[i].mv2);
				mainC.modelsMain.Render_Model_List_Item((ushort)pick1[i].id, (ushort)pick1[i].refID);
				break;
			}
		}
		global::Rendering.Rendering.effect1.Parameters["Emissive"].SetValue(em);
	}

	public void Disable_Pickups()
	{
		for (short num = 0; num < numPickups; num++)
		{
			pick1[num].enabled = false;
			pick1[num].changed = true;
		}
	}

	public void Send_Pickup_To_New_Players(NetworkGamer gamer)
	{
		byte b = 16;
		b = (byte)((b * 2 > 20) ? 10 : b);
		byte b2 = 0;
		byte b3 = 0;
		byte b4 = 0;
		while (b3 < numPickups)
		{
			if (pick1[b3].changed)
			{
				global::Networking.Networking.networkBytes[b2] = b3;
				global::Networking.Networking.networkBools[b4++] = pick1[b3].enabled;
				global::Networking.Networking.networkBools[b4++] = pick1[b3].onmap;
				ref HalfSingle reference = ref global::Networking.Networking.networkHS[b2];
				reference = new HalfSingle(pick1[b3].timeBeforeRespawn);
				b2++;
				if (b2 >= b)
				{
					ref HalfSingle reference2 = ref global::Networking.Networking.networkHS[b2];
					reference2 = new HalfSingle(timeModifier);
					global::Networking.Networking.networkUShorts[0] = b2;
					mainC.networkingMain.XBOX_Send_Network_Message_To_Gamer(60, gamer);
					b2 = 0;
					b4 = 0;
				}
			}
			b3++;
		}
		ref HalfSingle reference3 = ref global::Networking.Networking.networkHS[b2];
		reference3 = new HalfSingle(timeModifier);
		global::Networking.Networking.networkUShorts[0] = b2;
		mainC.networkingMain.XBOX_Send_Network_Message_To_Gamer(60, gamer);
		b = 5;
		b2 = 0;
		b3 = 0;
		b4 = 0;
		while (b3 < numPickups)
		{
			if (pick1[b3].positionChanged)
			{
				global::Networking.Networking.networkBytes[b2] = b3;
				ref HalfSingle reference4 = ref global::Networking.Networking.networkHS[b4++];
				reference4 = new HalfSingle(pick1[b3].position.v[0]);
				ref HalfSingle reference5 = ref global::Networking.Networking.networkHS[b4++];
				reference5 = new HalfSingle(pick1[b3].position.v[1]);
				ref HalfSingle reference6 = ref global::Networking.Networking.networkHS[b4++];
				reference6 = new HalfSingle(pick1[b3].position.v[2]);
				b2++;
				if (b2 >= b)
				{
					global::Networking.Networking.networkUShorts[0] = b2;
					mainC.networkingMain.XBOX_Send_Network_Message_To_Gamer(79, gamer);
					b2 = 0;
					b4 = 0;
				}
			}
			b3++;
		}
		global::Networking.Networking.networkUShorts[0] = b2;
		mainC.networkingMain.XBOX_Send_Network_Message_To_Gamer(79, gamer);
		b = (byte)((b > 10) ? 10 : b);
		b2 = 0;
		b3 = 0;
		b4 = 0;
		while (b3 < numPickups)
		{
			if ((pick1[b3].type == 1 && pick1[b3].fVar[0] != (float)pick1[b3].refID) || (pick1[b3].type == 17 && pick1[b3].onmap))
			{
				global::Networking.Networking.networkBytes[b2] = b3;
				global::Networking.Networking.networkShorts[b4++] = pick1[b3].refID;
				b2++;
				if (b2 >= b)
				{
					global::Networking.Networking.networkUShorts[0] = b2;
					mainC.networkingMain.XBOX_Send_Network_Message_To_Gamer(76, gamer);
					b2 = 0;
					b4 = 0;
				}
			}
			b3++;
		}
		global::Networking.Networking.networkUShorts[0] = b2;
		mainC.networkingMain.XBOX_Send_Network_Message_To_Gamer(76, gamer);
		b3 = 0;
		b4 = 0;
		while (b3 < numPickups)
		{
			if (pick1[b3].type == 17 && pick1[b3].onmap)
			{
				global::Networking.Networking.networkUShorts[0] = (ushort)pick1[b3].refID;
				ref HalfSingle reference7 = ref global::Networking.Networking.networkHS[0];
				reference7 = new HalfSingle(pick1[b3].position.v[0]);
				ref HalfSingle reference8 = ref global::Networking.Networking.networkHS[1];
				reference8 = new HalfSingle(pick1[b3].position.v[1]);
				ref HalfSingle reference9 = ref global::Networking.Networking.networkHS[2];
				reference9 = new HalfSingle(pick1[b3].position.v[2]);
				mainC.networkingMain.XBOX_Send_Network_Message_To_Gamer(77, gamer);
			}
			b3++;
		}
	}

	public void Send_Pickup_Acquired(ushort pickupID)
	{
		global::Networking.Networking.networkUShorts[0] = pickupID;
		mainC.networkingMain.XBOX_Send_Network_Message61(61);
	}

	public void Receive_Pickup_Acquired_Message(ushort pickupID)
	{
		switch (pick1[pickupID].type)
		{
		case 18:
		{
			for (ushort num = 0; num < numPickups; num++)
			{
				if (pick1[num].type == 18 && pick1[num].id == pick1[pickupID].id)
				{
					pick1[num].onmap = true;
				}
			}
			break;
		}
		case 19:
			mainC.maingameMain.Trigger_AI_Wave((ushort)pick1[pickupID].id);
			break;
		}
		pick1[pickupID].onmap = false;
		pick1[pickupID].changed = true;
		pick1[pickupID].timeBeforeRespawn = pick1[pickupID].respawnTime;
	}

	public void Receive_Pickup_Data(byte numToUpdate)
	{
		byte b = 0;
		byte b2 = 0;
		while (b < numToUpdate)
		{
			pick1[global::Networking.Networking.networkBytes[b]].enabled = global::Networking.Networking.networkBools[b2++];
			pick1[global::Networking.Networking.networkBytes[b]].onmap = global::Networking.Networking.networkBools[b2++];
			pick1[global::Networking.Networking.networkBytes[b]].timeBeforeRespawn = global::Networking.Networking.networkHS[b].ToSingle();
			pick1[global::Networking.Networking.networkBytes[b]].changed = true;
			b++;
		}
		receivedPickupData = true;
		if (receivedPickupWeaponData)
		{
			receivedMPData = true;
		}
		timeModifier = global::Networking.Networking.networkHS[b].ToSingle();
	}

	public void Receive_Weapon_Pickup_Data(byte numToUpdate)
	{
		ushort num = 0;
		ushort num2 = 0;
		while (num < numToUpdate)
		{
			ushort num3 = global::Networking.Networking.networkBytes[num];
			pick1[num3].refID = global::Networking.Networking.networkShorts[num2++];
			pick1[num3].modelList[0] = global::Weapons.Weapons.wp1[pick1[num3].refID].modelID;
			pick1[num3].changed = true;
			num++;
		}
		receivedPickupWeaponData = true;
		if (receivedPickupData)
		{
			receivedMPData = true;
		}
	}

	public void Receive_Weapon_Pickup_Change_Message(ushort pickupID)
	{
		pick1[pickupID].refID = global::Networking.Networking.networkShorts[0];
		pick1[pickupID].modelList[0] = global::Weapons.Weapons.wp1[global::Networking.Networking.networkShorts[0]].modelID;
		pick1[pickupID].changed = true;
	}

	public void Receive_New_Time_Modifier()
	{
		timeModifier = global::Networking.Networking.networkHS[0].ToSingle();
	}

	public void Player_Drops_Weapon(ushort weaponID, float x, float y, float z, bool sendToNetwork)
	{
		if (global::MainGame.MainGame.gameMode == 1 && sendToNetwork)
		{
			global::Networking.Networking.networkUShorts[0] = weaponID;
			ref HalfSingle reference = ref global::Networking.Networking.networkHS[0];
			reference = new HalfSingle(x);
			ref HalfSingle reference2 = ref global::Networking.Networking.networkHS[1];
			reference2 = new HalfSingle(y);
			ref HalfSingle reference3 = ref global::Networking.Networking.networkHS[2];
			reference3 = new HalfSingle(z);
			mainC.networkingMain.XBOX_Send_Network_Message77(77);
			x = global::Networking.Networking.networkHS[0].ToSingle();
			y = global::Networking.Networking.networkHS[1].ToSingle();
			z = global::Networking.Networking.networkHS[2].ToSingle();
		}
		for (ushort num = 0; num < numPickups; num++)
		{
			if (pick1[num].type == 17 && !pick1[num].onmap)
			{
				pick1[num].refID = (short)weaponID;
				pick1[num].modelList[0] = global::Weapons.Weapons.wp1[weaponID].modelID;
				pick1[num].onmap = true;
				pick1[num].changed = true;
				pick1[num].positionChanged = true;
				pick1[num].offsetX = x - pick1[num].position.v[0];
				pick1[num].offsetY = y - pick1[num].position.v[1];
				pick1[num].offsetZ = z - pick1[num].position.v[2];
				pick1[num].position.v[0] = x;
				pick1[num].position.v[1] = y;
				pick1[num].position.v[2] = z;
				pick1[num].b1.v[0] += pick1[num].offsetX;
				pick1[num].b2.v[0] += pick1[num].offsetX;
				pick1[num].b1.v[1] += pick1[num].offsetY;
				pick1[num].b2.v[1] += pick1[num].offsetY;
				pick1[num].b1.v[2] += pick1[num].offsetZ;
				pick1[num].b2.v[2] += pick1[num].offsetZ;
				pick1[num].mv.M41 = x;
				pick1[num].mv.M42 = y;
				pick1[num].mv.M43 = z;
				pick1[num].mv2.M41 = x;
				pick1[num].mv2.M42 = y;
				pick1[num].mv2.M43 = z;
				break;
			}
		}
	}

	public void Pickup_Changed_Position(byte numToUpdate)
	{
		try
		{
			ushort num = 0;
			ushort num2 = 0;
			while (num < numToUpdate)
			{
				ushort num3 = global::Networking.Networking.networkBytes[num];
				float num4 = global::Networking.Networking.networkHS[num2++].ToSingle();
				float num5 = global::Networking.Networking.networkHS[num2++].ToSingle();
				float num6 = global::Networking.Networking.networkHS[num2++].ToSingle();
				pick1[num3].offsetX = num4 - pick1[num3].position.v[0];
				pick1[num3].offsetY = num5 - pick1[num3].position.v[1];
				pick1[num3].offsetZ = num6 - pick1[num3].position.v[2];
				pick1[num3].position.v[0] = num4;
				pick1[num3].position.v[1] = num5;
				pick1[num3].position.v[2] = num6;
				pick1[num3].b1.v[0] += pick1[num3].offsetX;
				pick1[num3].b2.v[0] += pick1[num3].offsetX;
				pick1[num3].b1.v[1] += pick1[num3].offsetY;
				pick1[num3].b2.v[1] += pick1[num3].offsetY;
				pick1[num3].b1.v[2] += pick1[num3].offsetZ;
				pick1[num3].b2.v[2] += pick1[num3].offsetZ;
				pick1[num3].mv.M41 = num4;
				pick1[num3].mv.M42 = num5;
				pick1[num3].mv.M43 = num6;
				pick1[num3].mv2.M41 = num4;
				pick1[num3].mv2.M42 = num5;
				pick1[num3].mv2.M43 = num6;
				pick1[num3].changed = true;
				pick1[num3].positionChanged = true;
				num++;
			}
		}
		catch (Exception)
		{
			global::InputHandler.InputHandler.tw = 5f;
		}
	}

	public bool Find_Nearest_Pickup_Of_Type(byte pickupType, float posX, float posY, float posZ, out float pickupX, out float pickupY, out float pickupZ, out float pickupDistance)
	{
		bool flag = false;
		float num = 0f;
		ushort num2 = 0;
		pickupX = 0f;
		pickupY = 0f;
		pickupZ = 0f;
		pickupDistance = 0f;
		ushort num3;
		for (num3 = 0; num3 < numPickups; num3++)
		{
			if (pick1[num3].onmap && pick1[num3].type == pickupType)
			{
				float num4 = posX - pick1[num3].position.v[0];
				float num5 = posY - pick1[num3].position.v[1];
				float num6 = posZ - pick1[num3].position.v[2];
				num = num4 * num4 + num5 * num5 + num6 * num6;
				num2 = num3;
				flag = true;
				break;
			}
		}
		for (num3++; num3 < numPickups; num3++)
		{
			if (pick1[num3].onmap && pick1[num3].type == pickupType)
			{
				float num4 = posX - pick1[num3].position.v[0];
				float num5 = posY - pick1[num3].position.v[1];
				float num6 = posZ - pick1[num3].position.v[2];
				float num7 = num4 * num4 + num5 * num5 + num6 * num6;
				if (num7 < num || !flag)
				{
					num = num7;
					num2 = num3;
					flag = true;
				}
			}
		}
		if (flag)
		{
			pickupX = pick1[num2].position.v[0];
			pickupY = pick1[num2].position.v[1];
			pickupZ = pick1[num2].position.v[2];
			pickupDistance = (float)Math.Sqrt(num);
		}
		return flag;
	}

	public bool Find_First_Pickup_Of_Type_And_RefID(byte pickupType, short refID, ushort startID, out ushort pickupID)
	{
		pickupID = 0;
		for (ushort num = startID; num < numPickups; num++)
		{
			if (pick1[num].onmap && pick1[num].type == pickupType && pick1[num].refID == refID)
			{
				pickupID = num;
				return true;
			}
		}
		return false;
	}

	public void Activate_Pickup(ushort pickupID, float x, float y, float z)
	{
		if (pickupID < numPickups)
		{
			pick1[pickupID].changed = true;
			pick1[pickupID].positionChanged = true;
			pick1[pickupID].onmap = true;
			pick1[pickupID].enabled = true;
			Set_Pickup_Position(pickupID, x, y, z);
			byte type = pick1[pickupID].type;
			if (type == 12)
			{
				pick1[pickupID].b1.v[0] -= pick1[pickupID].offsetX;
				pick1[pickupID].b1.v[1] -= pick1[pickupID].offsetY;
				pick1[pickupID].b1.v[2] -= pick1[pickupID].offsetZ;
				pick1[pickupID].b2.v[0] -= pick1[pickupID].offsetX;
				pick1[pickupID].b2.v[1] -= pick1[pickupID].offsetY;
				pick1[pickupID].b2.v[2] -= pick1[pickupID].offsetZ;
				pick1[pickupID].offsetX = 0f;
				pick1[pickupID].offsetY = 0f;
				pick1[pickupID].offsetZ = 0f;
				pick1[pickupID].mv.M41 = pick1[pickupID].position.v[0];
				pick1[pickupID].mv.M42 = pick1[pickupID].position.v[1];
				pick1[pickupID].mv.M43 = pick1[pickupID].position.v[2];
			}
		}
	}

	public void Activate_End_Of_Level_Pickup()
	{
		for (short num = 0; num < numPickups; num++)
		{
			if (pick1[num].type == 6)
			{
				pick1[num].enabled = true;
				pick1[num].onmap = true;
				pick1[num].changed = true;
				break;
			}
		}
	}

	public void Update_Time_Modifier(float newValue)
	{
		timeModifier = newValue;
		if (global::MainGame.MainGame.gameMode == 1)
		{
			ref HalfSingle reference = ref global::Networking.Networking.networkHS[0];
			reference = new HalfSingle(newValue);
			mainC.networkingMain.XBOX_Send_Network_Message62(62);
			timeModifier = global::Networking.Networking.networkHS[0].ToSingle();
		}
	}

	public short Get_Next_Checkpoint_Pickup()
	{
		for (short num = 0; num < numPickups; num++)
		{
			if (pick1[num].onmap && pick1[num].type == 11 && pick1[num].enabled)
			{
				return pick1[num].refID;
			}
		}
		return -1;
	}

	public void Find_Enabled_Pickup(byte type)
	{
		enabledReturnVarF1 = 0f;
		enabledReturnVarF2 = 0f;
		enabledReturnVarF3 = 0f;
		enabledReturnVarF4 = 0f;
		for (short num = 0; num < numPickups; num++)
		{
			if (pick1[num].type == type && pick1[num].enabled)
			{
				enabledReturnVarF1 = pick1[num].fVar[0];
				enabledReturnVarF2 = pick1[num].fVar[1];
				enabledReturnVarF3 = pick1[num].fVar[2];
				enabledReturnVarF4 = pick1[num].fVar[3];
				break;
			}
		}
	}

	public void Set_Pickup_Position(ushort pickupID, float x, float y, float z)
	{
		if (global::MainGame.MainGame.gameMode == 1)
		{
			ref HalfSingle reference = ref global::Networking.Networking.networkHS[0];
			reference = new HalfSingle(x);
			ref HalfSingle reference2 = ref global::Networking.Networking.networkHS[1];
			reference2 = new HalfSingle(y);
			ref HalfSingle reference3 = ref global::Networking.Networking.networkHS[2];
			reference3 = new HalfSingle(z);
			x = global::Networking.Networking.networkHS[0].ToSingle();
			y = global::Networking.Networking.networkHS[1].ToSingle();
			z = global::Networking.Networking.networkHS[2].ToSingle();
		}
		float num = x - pick1[pickupID].position.v[0];
		pick1[pickupID].position.v[0] += num;
		pick1[pickupID].b1.v[0] += num;
		pick1[pickupID].b2.v[0] += num;
		num = y - pick1[pickupID].position.v[1];
		pick1[pickupID].position.v[1] += num;
		pick1[pickupID].b1.v[1] += num;
		pick1[pickupID].b2.v[1] += num;
		num = z - pick1[pickupID].position.v[2];
		pick1[pickupID].position.v[2] += num;
		pick1[pickupID].b1.v[2] += num;
		pick1[pickupID].b2.v[2] += num;
		pick1[pickupID].mv = Matrix.CreateRotationZ(pick1[pickupID].rotation.v[2] * ((float)Math.PI / 180f)) * Matrix.CreateRotationY(pick1[pickupID].rotation.v[1] * ((float)Math.PI / 180f)) * Matrix.CreateRotationX(pick1[pickupID].rotation.v[0] * ((float)Math.PI / 180f));
		pick1[pickupID].mv.M41 = x;
		pick1[pickupID].mv.M42 = y;
		pick1[pickupID].mv.M43 = z;
		pick1[pickupID].mv2 = pick1[pickupID].mv;
	}

	public void Set_Will_Respawn_False(byte pick_up_type)
	{
		for (short num = 0; num < numPickups; num++)
		{
			if (pick1[num].type == pick_up_type)
			{
				pick1[num].willRespawn = false;
			}
		}
	}
}
