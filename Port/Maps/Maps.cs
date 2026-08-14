using System;
using System.Globalization;
using System.IO;
using MainGame;
using Microsoft.Xna.Framework;
using Players;
using Rendering;
using Structs;
using Util;
using WindowsGame1;

namespace Maps;

public class Maps
{
	public static byte aiSpawnMode;

	public static byte aiSpawnPointSeed;

	public static ushort aiPlayerToSpawnNear;

	public static float aiSpawnRadiusMaxSqr;

	public static float aiSpawnRadiusMinSqr;

	public static int numSpawn;

	public static int numAllocatedSpawn;

	public static int spawnPerTeams;

	public static StructsClass.Spawn_Point[] spawnPoints;

	public Game1.MasterCollection mainC;

	public void Init_Vars(Game1.MasterCollection master)
	{
		mainC = master;
	}

	public void Init_Maps()
	{
		aiSpawnMode = 0;
	}

	public void Load_Map_Data(string fileName, bool needReset)
	{
		int num = -1;
		numSpawn = 0;
		spawnPerTeams = 0;
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
				if (array4[0].Equals("numSpawnPoints", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 1;
				}
				else if (array4[0].Equals("SpawnPoint", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 2;
				}
				else if (array4[0].Equals("Position", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 3;
				}
				else if (array4[0].Equals("Rotation", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 4;
				}
				else if (array4[0].Equals("Active", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 5;
				}
				switch (num4)
				{
				case 1:
				{
					if (array4.Length <= 1)
					{
						break;
					}
					int num6 = short.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
					if (num6 > numAllocatedSpawn)
					{
						spawnPoints = new StructsClass.Spawn_Point[num6];
						for (int num5 = 0; num5 < num6; num5++)
						{
							spawnPoints[num5].active = true;
							spawnPoints[num5].startsActive = true;
						}
						numAllocatedSpawn = num6;
					}
					numSpawn = num6;
					break;
				}
				case 2:
					if (array4.Length > 1)
					{
						int num6 = int.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
						if (num6 > -1 && num6 < numAllocatedSpawn)
						{
							num = num6;
						}
					}
					break;
				case 3:
					if (num > -1 && num < numAllocatedSpawn && array4.Length > 3)
					{
						spawnPoints[num].x = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
						spawnPoints[num].y = float.Parse(array4[2], CultureInfo.InvariantCulture.NumberFormat);
						spawnPoints[num].z = float.Parse(array4[3], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 4:
					if (num > -1 && num < numAllocatedSpawn && array4.Length > 1)
					{
						spawnPoints[num].rotation = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 5:
					if (num > -1 && num < numAllocatedSpawn && array4.Length > 1)
					{
						int num5 = byte.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
						spawnPoints[num].startsActive = num5 == 1;
					}
					break;
				}
			}
		}
		stream.Close();
		if (needReset)
		{
			Reset_Round();
		}
	}

	public void Get_Spawn_Point(ref StructsClass.vtex v1, ushort team, ref float angle, sbyte useThisValue, bool checkForEnemy, float adjZ, float minRadius)
	{
		bool flag = false;
		int i = 0;
		if (useThisValue < 0)
		{
			if (global::MainGame.MainGame.gameType == 0)
			{
				short num = (short)(spawnPerTeams * team);
				if (checkForEnemy)
				{
					short num2 = (short)Math.Round((float)global::MainGame.MainGame.mainRandom.NextDouble() * (float)(spawnPerTeams - 1));
					for (short num3 = (short)(num2 + num); num3 < (short)(spawnPerTeams + num); num3++)
					{
						if (spawnPoints[num3].active)
						{
							float num4 = global::Players.Players.Find_Player_Within_Distance(0, team, 900f, spawnPoints[num3].x, spawnPoints[num3].y, spawnPoints[num3].z);
							if (!(num4 < 900f))
							{
								i = num3;
								flag = true;
								break;
							}
						}
					}
					if (!flag)
					{
						for (short num3 = 0; num3 < num2; num3++)
						{
							if (spawnPoints[num3].active)
							{
								float num4 = global::Players.Players.Find_Player_Within_Distance(0, team, 900f, spawnPoints[num3].x, spawnPoints[num3].y, spawnPoints[num3].z);
								if (!(num4 < 900f))
								{
									i = num3;
									flag = true;
									break;
								}
							}
						}
					}
				}
				if (!flag)
				{
					for (short num3 = 0; num3 < 10; num3++)
					{
						i = (int)Math.Round((float)global::MainGame.MainGame.mainRandom.NextDouble() * (float)(spawnPerTeams - 1));
						if (spawnPoints[i + num].active)
						{
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						i = num;
						for (short num2 = (short)(num + spawnPerTeams); i < num2; i++)
						{
							if (spawnPoints[i].active)
							{
								flag = true;
								break;
							}
						}
					}
				}
			}
			else
			{
				if (checkForEnemy)
				{
					short num = (short)Math.Round((float)global::MainGame.MainGame.mainRandom.NextDouble() * (float)(numSpawn - 1));
					for (short num3 = num; num3 < numSpawn; num3++)
					{
						if (spawnPoints[num3].active)
						{
							float num4 = global::Players.Players.Find_Player_Within_Distance(0, team, 900f, spawnPoints[num3].x, spawnPoints[num3].y, spawnPoints[num3].z);
							if (!(num4 < 900f))
							{
								num4 = global::Players.Players.Find_Player_Within_Distance(0, global::Util.Util.maxUnsignedShortValue, minRadius, spawnPoints[num3].x, spawnPoints[num3].y, spawnPoints[num3].z);
								if (!(num4 < minRadius))
								{
									i = num3;
									flag = true;
									break;
								}
							}
						}
					}
					if (!flag)
					{
						for (short num3 = 0; num3 < num; num3++)
						{
							if (spawnPoints[num3].active)
							{
								float num4 = global::Players.Players.Find_Player_Within_Distance(0, team, 900f, spawnPoints[num3].x, spawnPoints[num3].y, spawnPoints[num3].z);
								if (!(num4 < 900f))
								{
									i = num3;
									flag = true;
									break;
								}
							}
						}
					}
				}
				if (!flag)
				{
					for (short num3 = 0; num3 < 10; num3++)
					{
						i = (int)Math.Round((float)global::MainGame.MainGame.mainRandom.NextDouble() * (float)(numSpawn - 1));
						if (spawnPoints[i].active)
						{
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						for (i = 0; i < numSpawn; i++)
						{
							if (spawnPoints[i].active)
							{
								flag = true;
								break;
							}
						}
					}
				}
			}
		}
		else
		{
			i = useThisValue;
		}
		if (i < 0 || i >= numSpawn)
		{
			i = 0;
		}
		v1.v[0] = spawnPoints[i].x;
		v1.v[1] = spawnPoints[i].y;
		v1.v[2] = spawnPoints[i].z + adjZ;
		angle = spawnPoints[i].rotation;
	}

	public void Get_AI_Spawn_Point(ref StructsClass.vtex v1, ushort team, ref float angle, sbyte dontUseThisValue, bool checkForEnemy, float adjZ)
	{
		bool flag = false;
		int i = 0;
		switch (aiSpawnMode)
		{
		case 0:
			if (dontUseThisValue < 0)
			{
				if (global::MainGame.MainGame.gameType == 0)
				{
					short num7 = (short)(spawnPerTeams * team);
					if (checkForEnemy)
					{
						short num8 = (short)Math.Round((float)global::MainGame.MainGame.mainRandom.NextDouble() * (float)(spawnPerTeams - 1));
						for (short num9 = aiSpawnPointSeed; num9 < spawnPerTeams; num9++)
						{
							if (spawnPoints[num9 + num7].active)
							{
								float num6 = global::Players.Players.Find_Player_Within_Distance(0, team, 900f, spawnPoints[num9 + num7].x, spawnPoints[num9 + num7].y, spawnPoints[num9 + num7].z);
								if (!(num6 < 900f))
								{
									i = num9;
									flag = true;
									break;
								}
							}
						}
						if (!flag)
						{
							for (short num9 = 0; num9 < num8; num9++)
							{
								if (spawnPoints[num9 + num7].active)
								{
									float num6 = global::Players.Players.Find_Player_Within_Distance(0, team, 900f, spawnPoints[num9 + num7].x, spawnPoints[num9 + num7].y, spawnPoints[num9 + num7].z);
									if (!(num6 < 900f))
									{
										i = num9;
										flag = true;
										break;
									}
								}
							}
						}
					}
					if (!flag)
					{
						for (short num9 = 0; num9 < 10; num9++)
						{
							i = (int)Math.Round((float)global::MainGame.MainGame.mainRandom.NextDouble() * (float)(spawnPerTeams - 1));
							if (spawnPoints[i + num7].active)
							{
								flag = true;
								break;
							}
						}
						if (!flag)
						{
							for (i = 0; i < spawnPerTeams; i++)
							{
								if (spawnPoints[i + num7].active)
								{
									flag = true;
									break;
								}
							}
						}
					}
					i += num7;
				}
				else
				{
					aiSpawnPointSeed++;
					if (aiSpawnPointSeed >= numSpawn)
					{
						aiSpawnPointSeed = 0;
					}
					int num2 = aiSpawnPointSeed;
					if (checkForEnemy)
					{
						float num = -1f;
						for (short num9 = aiSpawnPointSeed; num9 < numSpawn; num9++)
						{
							if (spawnPoints[num9].active)
							{
								float num6 = global::Players.Players.Find_Player_Within_Distance(0, team, 900f, spawnPoints[num9].x, spawnPoints[num9].y, spawnPoints[num9].z);
								if (!(num6 < 900f))
								{
									i = num9;
									flag = true;
									break;
								}
								if (num6 > num)
								{
									num = num6;
									num2 = num9;
								}
							}
						}
						if (!flag)
						{
							for (short num9 = 0; num9 < aiSpawnPointSeed; num9++)
							{
								if (spawnPoints[num9].active)
								{
									float num6 = global::Players.Players.Find_Player_Within_Distance(0, team, 900f, spawnPoints[num9].x, spawnPoints[num9].y, spawnPoints[num9].z);
									if (!(num6 < 900f))
									{
										i = num9;
										flag = true;
										break;
									}
									if (num6 > num)
									{
										num = num6;
										num2 = num9;
									}
								}
							}
						}
					}
					if (!flag)
					{
						i = num2;
					}
					aiSpawnPointSeed = (byte)i;
				}
			}
			else
			{
				aiSpawnPointSeed++;
				if (aiSpawnPointSeed >= numSpawn)
				{
					aiSpawnPointSeed = (byte)(dontUseThisValue + 1);
				}
				if (aiSpawnPointSeed <= dontUseThisValue)
				{
					aiSpawnPointSeed = (byte)(dontUseThisValue + 1);
				}
				int num2 = aiSpawnPointSeed;
				if (checkForEnemy)
				{
					float num = -1f;
					for (short num9 = aiSpawnPointSeed; num9 < numSpawn; num9++)
					{
						if (num9 != dontUseThisValue && spawnPoints[num9].active)
						{
							float num6 = global::Players.Players.Find_Player_Within_Distance(0, team, 900f, spawnPoints[num9].x, spawnPoints[num9].y, spawnPoints[num9].z);
							if (!(num6 < 900f))
							{
								i = num9;
								flag = true;
								break;
							}
							if (num6 > num)
							{
								num = num6;
								num2 = num9;
							}
						}
					}
					if (!flag)
					{
						for (short num9 = (short)(dontUseThisValue + 1); num9 < aiSpawnPointSeed; num9++)
						{
							if (num9 != dontUseThisValue && spawnPoints[num9].active)
							{
								float num6 = global::Players.Players.Find_Player_Within_Distance(0, team, 900f, spawnPoints[num9].x, spawnPoints[num9].y, spawnPoints[num9].z);
								if (!(num6 < 900f))
								{
									i = num9;
									flag = true;
									break;
								}
								if (num6 > num)
								{
									num = num6;
									num2 = num9;
								}
							}
						}
					}
				}
				if (!flag)
				{
					i = num2;
				}
				if (i <= dontUseThisValue)
				{
					i++;
					if (i >= numSpawn)
					{
						i = 0;
					}
				}
				aiSpawnPointSeed = (byte)i;
			}
			if (i < 0 || i >= numSpawn)
			{
				i = 0;
			}
			v1.v[0] = spawnPoints[i].x;
			v1.v[1] = spawnPoints[i].y;
			v1.v[2] = spawnPoints[i].z + adjZ;
			angle = spawnPoints[i].rotation;
			break;
		case 1:
		{
			byte uBufferID = global::Rendering.Rendering.uBufferID;
			float num = 0f;
			int num2 = 0;
			float num3;
			float num4;
			float num6;
			for (i = aiSpawnPointSeed; i < numSpawn; i++)
			{
				if (spawnPoints[i].active)
				{
					num3 = spawnPoints[i].x - global::Players.Players.players[aiPlayerToSpawnNear].posX[uBufferID];
					num4 = spawnPoints[i].y - global::Players.Players.players[aiPlayerToSpawnNear].posY[uBufferID];
					float num5 = spawnPoints[i].z - global::Players.Players.players[aiPlayerToSpawnNear].posZ[uBufferID];
					num6 = num3 * num3 + num4 * num4 + num5 * num5;
					if ((num6 > aiSpawnRadiusMinSqr && num6 < aiSpawnRadiusMaxSqr) || global::Players.Players.players[aiPlayerToSpawnNear].onmap != 4)
					{
						flag = true;
						num2 = i;
						break;
					}
				}
			}
			if (!flag)
			{
				for (i = 0; i < aiSpawnPointSeed; i++)
				{
					if (spawnPoints[i].active)
					{
						num3 = spawnPoints[i].x - global::Players.Players.players[aiPlayerToSpawnNear].posX[uBufferID];
						num4 = spawnPoints[i].y - global::Players.Players.players[aiPlayerToSpawnNear].posY[uBufferID];
						float num5 = spawnPoints[i].z - global::Players.Players.players[aiPlayerToSpawnNear].posZ[uBufferID];
						num6 = num3 * num3 + num4 * num4 + num5 * num5;
						if (num6 > aiSpawnRadiusMinSqr && num6 < aiSpawnRadiusMaxSqr)
						{
							flag = true;
							num2 = i;
							break;
						}
					}
				}
			}
			if (!flag)
			{
				for (i = 0; i < numSpawn; i++)
				{
					if (spawnPoints[i].active)
					{
						num3 = spawnPoints[i].x - global::Players.Players.players[aiPlayerToSpawnNear].posX[uBufferID];
						num4 = spawnPoints[i].y - global::Players.Players.players[aiPlayerToSpawnNear].posY[uBufferID];
						float num5 = spawnPoints[i].z - global::Players.Players.players[aiPlayerToSpawnNear].posZ[uBufferID];
						num6 = num3 * num3 + num4 * num4 + num5 * num5;
						if (num6 > aiSpawnRadiusMinSqr)
						{
							flag = true;
							num2 = i;
							break;
						}
					}
				}
			}
			if (!flag)
			{
				for (i = 0; i < numSpawn; i++)
				{
					if (spawnPoints[i].active)
					{
						num2 = i;
						break;
					}
				}
			}
			if (num2 < 0 || num2 >= numSpawn)
			{
				num2 = 0;
			}
			v1.v[0] = spawnPoints[num2].x;
			v1.v[1] = spawnPoints[num2].y;
			v1.v[2] = spawnPoints[num2].z + adjZ;
			num3 = global::Players.Players.players[aiPlayerToSpawnNear].posX[uBufferID] - spawnPoints[num2].x;
			num4 = global::Players.Players.players[aiPlayerToSpawnNear].posY[uBufferID] - spawnPoints[num2].y;
			num6 = (float)Math.Sqrt(num3 * num3 + num4 * num4);
			if (num6 != 0f)
			{
				num4 /= num6;
			}
			angle = (float)Math.Cos(num4) * 57.29578f;
			if (num3 > 0f)
			{
				angle *= -1f;
			}
			aiSpawnPointSeed = (byte)(++num2);
			if (aiSpawnPointSeed >= numSpawn)
			{
				aiSpawnPointSeed = 0;
			}
			break;
		}
		}
	}

	public void Get_Random_Spawn_Point(out float x, out float y, out float z, out float angle)
	{
		int num = (int)Math.Round((float)global::MainGame.MainGame.mainRandom.NextDouble() * (float)(numSpawn - 1));
		if (!spawnPoints[num].active)
		{
			int i;
			for (i = num + 1; i < numSpawn; i++)
			{
				if (spawnPoints[i].active)
				{
					num = i;
					break;
				}
			}
			if (num != i)
			{
				for (i = num - 1; i > -1; i--)
				{
					if (spawnPoints[i].active)
					{
						num = i;
						break;
					}
				}
			}
		}
		if (num >= numSpawn)
		{
			num = 0;
		}
		x = spawnPoints[num].x;
		y = spawnPoints[num].y;
		z = spawnPoints[num].z;
		angle = spawnPoints[num].rotation;
	}

	public void Change_SpawnPoint(ushort spID, float x, float y, float z, float rotation)
	{
		if (spID >= numSpawn)
		{
			spID = 0;
		}
		spawnPoints[spID].x = x;
		spawnPoints[spID].y = y;
		spawnPoints[spID].z = z;
		spawnPoints[spID].rotation = rotation;
	}

	public void Update_Team_SpawnPoints()
	{
		spawnPerTeams = numSpawn / global::MainGame.MainGame.numTeams;
	}

	public void Set_Spawn_Points_Active_Status(ushort start, ushort end, bool status)
	{
		ushort num = start;
		while (num <= end && num < numSpawn)
		{
			spawnPoints[num].active = status;
			num++;
		}
	}

	public void Reset_Round()
	{
		spawnPerTeams = numSpawn / global::MainGame.MainGame.numTeams;
		aiSpawnPointSeed = (byte)Math.Round((float)global::MainGame.MainGame.mainRandom.NextDouble() * (float)(numSpawn - 1));
		for (ushort num = 0; num < numSpawn; num++)
		{
			spawnPoints[num].active = spawnPoints[num].startsActive;
		}
	}

	public void Reset_Round_Maps_Section(byte threadID)
	{
		if (global::MainGame.MainGame.gameMode == 0)
		{
			mainC.levelsMain.Load_Level_Section("SP_" + global::MainGame.MainGame.gameType + "_" + global::MainGame.MainGame.curSpLevel + "_" + global::MainGame.MainGame.difficulty + ".txt", 8, threadID);
		}
		spawnPerTeams = numSpawn / global::MainGame.MainGame.numTeams;
		for (ushort num = 0; num < numSpawn; num++)
		{
			spawnPoints[num].active = spawnPoints[num].startsActive;
		}
	}

	public void Set_Position_InsideBoundary(ref StructsClass.particle_list ph1)
	{
		float num = 0f;
		float num2 = 0f;
		if (ph1.pos2.v[0] > global::MainGame.MainGame.MaxRight)
		{
			num = (num2 = Math.Abs((ph1.pos2.v[0] - global::MainGame.MainGame.MaxRight) / (ph1.pos2.v[0] - ph1.pos1.v[0])));
		}
		else if (ph1.pos2.v[0] < global::MainGame.MainGame.MaxLeft)
		{
			num2 = Math.Abs((global::MainGame.MainGame.MaxLeft - ph1.pos2.v[0]) / (ph1.pos2.v[0] - ph1.pos1.v[0]));
			if (num2 > num)
			{
				num = num2;
			}
		}
		if (ph1.pos2.v[1] > global::MainGame.MainGame.MaxForward)
		{
			num2 = Math.Abs((ph1.pos2.v[1] - global::MainGame.MainGame.MaxForward) / (ph1.pos2.v[1] - ph1.pos1.v[1]));
			if (num2 > num)
			{
				num = num2;
			}
		}
		else if (ph1.pos2.v[1] < global::MainGame.MainGame.MaxRear)
		{
			num2 = Math.Abs((global::MainGame.MainGame.MaxRear - ph1.pos2.v[1]) / (ph1.pos2.v[1] - ph1.pos1.v[1]));
			if (num2 > num)
			{
				num = num2;
			}
		}
		if (ph1.pos2.v[2] > global::MainGame.MainGame.MaxUp)
		{
			num2 = Math.Abs((ph1.pos2.v[2] - global::MainGame.MainGame.MaxUp) / (ph1.pos2.v[2] - ph1.pos1.v[2]));
			if (num2 > num)
			{
				num = num2;
			}
		}
		else if (ph1.pos2.v[2] < global::MainGame.MainGame.MaxDown)
		{
			num2 = Math.Abs((global::MainGame.MainGame.MaxDown - ph1.pos2.v[2]) / (ph1.pos2.v[2] - ph1.pos1.v[2]));
			if (num2 > num)
			{
				num = num2;
			}
		}
		if (num > 0f)
		{
			num = 1f - num;
			ph1.pos2.v[0] = ph1.pos1.v[0] + num * (ph1.pos2.v[0] - ph1.pos1.v[0]);
			ph1.pos2.v[1] = ph1.pos1.v[1] + num * (ph1.pos2.v[1] - ph1.pos1.v[1]);
			ph1.pos2.v[2] = ph1.pos1.v[2] + num * (ph1.pos2.v[2] - ph1.pos1.v[2]);
		}
	}

	public void Set_Position_OutsideBoundary(ref StructsClass.particle_list ph1)
	{
		float num = 0f;
		float num2 = 0f;
		if (ph1.pos2.v[0] > global::MainGame.MainGame.MaxRight + 1f)
		{
			num = (num2 = Math.Abs((ph1.pos2.v[0] - (global::MainGame.MainGame.MaxRight + 1f)) / (ph1.pos2.v[0] - ph1.pos1.v[0])));
		}
		else if (ph1.pos2.v[0] < global::MainGame.MainGame.MaxLeft - 1f)
		{
			num2 = Math.Abs((global::MainGame.MainGame.MaxLeft - 1f - ph1.pos2.v[0]) / (ph1.pos2.v[0] - ph1.pos1.v[0]));
			if (num2 > num)
			{
				num = num2;
			}
		}
		if (ph1.pos2.v[1] > global::MainGame.MainGame.MaxForward + 1f)
		{
			num2 = Math.Abs((ph1.pos2.v[1] - (global::MainGame.MainGame.MaxForward + 1f)) / (ph1.pos2.v[1] - ph1.pos1.v[1]));
			if (num2 > num)
			{
				num = num2;
			}
		}
		else if (ph1.pos2.v[1] < global::MainGame.MainGame.MaxRear - 1f)
		{
			num2 = Math.Abs((global::MainGame.MainGame.MaxRear - 1f - ph1.pos2.v[1]) / (ph1.pos2.v[1] - ph1.pos1.v[1]));
			if (num2 > num)
			{
				num = num2;
			}
		}
		if (ph1.pos2.v[2] > global::MainGame.MainGame.MaxUp + 1f)
		{
			num2 = Math.Abs((ph1.pos2.v[2] - (global::MainGame.MainGame.MaxUp + 1f)) / (ph1.pos2.v[2] - ph1.pos1.v[2]));
			if (num2 > num)
			{
				num = num2;
			}
		}
		else if (ph1.pos2.v[2] < global::MainGame.MainGame.MaxDown - 1f)
		{
			num2 = Math.Abs((global::MainGame.MainGame.MaxDown - 1f - ph1.pos2.v[2]) / (ph1.pos2.v[2] - ph1.pos1.v[2]));
			if (num2 > num)
			{
				num = num2;
			}
		}
		if (num > 0f)
		{
			num = 1f - num;
			ph1.pos2.v[0] = ph1.pos1.v[0] + num * (ph1.pos2.v[0] - ph1.pos1.v[0]);
			ph1.pos2.v[1] = ph1.pos1.v[1] + num * (ph1.pos2.v[1] - ph1.pos1.v[1]);
			ph1.pos2.v[2] = ph1.pos1.v[2] + num * (ph1.pos2.v[2] - ph1.pos1.v[2]);
		}
	}

	public float Get_Map_Height(float x, float y, byte threadID)
	{
		Vector3 InitialRayStart = default(Vector3);
		Vector3 InitialRayEnd = default(Vector3);
		Vector3 IntersectPosition = default(Vector3);
		Vector3 IntersectNormal = default(Vector3);
		short returnValueZoneCheckIndex = 0;
		InitialRayStart.X = x;
		InitialRayStart.Y = y;
		InitialRayStart.Z = global::MainGame.MainGame.MaxUp;
		InitialRayEnd.X = x;
		InitialRayEnd.Y = y;
		InitialRayEnd.Z = global::MainGame.MainGame.MaxDown;
		float num = global::MainGame.MainGame.MaxUp - global::MainGame.MainGame.MaxDown;
		float result = global::MainGame.MainGame.MaxDown;
		ushort returnValueZoneCheckObjID;
		while (mainC.zonesMain.Check_Zones_For_Ray(x, y, global::MainGame.MainGame.MaxUp, InitialRayEnd.X, InitialRayEnd.Y, InitialRayEnd.Z, 5f, returnValueZoneCheckIndex, 1, out returnValueZoneCheckIndex, out returnValueZoneCheckObjID))
		{
			int numObjects = Zones.zones[returnValueZoneCheckObjID].zoneList.numObjects;
			for (int i = 0; i < numObjects; i++)
			{
				if (mainC.collisionMain.Check_Polygon_Ray_Collision_Projectile(Zones.zones[returnValueZoneCheckObjID].zoneList.oList[i], -1, ref InitialRayStart, ref InitialRayEnd, ref Zones.zones[returnValueZoneCheckObjID].zoneList.matrixList[i], out var distance, out IntersectPosition, out IntersectNormal, out var _, threadID) && distance < num)
				{
					num = distance;
					result = IntersectPosition.Z;
				}
			}
		}
		return result;
	}

	public float Get_Map_Height_From_Starting_Position(float x, float y, float z, byte threadID)
	{
		Vector3 InitialRayStart = default(Vector3);
		Vector3 InitialRayEnd = default(Vector3);
		Vector3 IntersectPosition = default(Vector3);
		Vector3 IntersectNormal = default(Vector3);
		short returnValueZoneCheckIndex = 0;
		InitialRayStart.X = x;
		InitialRayStart.Y = y;
		InitialRayStart.Z = z;
		InitialRayEnd.X = x;
		InitialRayEnd.Y = y;
		InitialRayEnd.Z = global::MainGame.MainGame.MaxDown;
		float num = global::MainGame.MainGame.MaxUp - global::MainGame.MainGame.MaxDown;
		float result = global::MainGame.MainGame.MaxDown;
		ushort returnValueZoneCheckObjID;
		while (mainC.zonesMain.Check_Zones_For_Ray(x, y, global::MainGame.MainGame.MaxUp, InitialRayEnd.X, InitialRayEnd.Y, InitialRayEnd.Z, 5f, returnValueZoneCheckIndex, 1, out returnValueZoneCheckIndex, out returnValueZoneCheckObjID))
		{
			int numObjects = Zones.zones[returnValueZoneCheckObjID].zoneList.numObjects;
			for (int i = 0; i < numObjects; i++)
			{
				if (mainC.collisionMain.Check_Polygon_Ray_Collision_Projectile(Zones.zones[returnValueZoneCheckObjID].zoneList.oList[i], -1, ref InitialRayStart, ref InitialRayEnd, ref Zones.zones[returnValueZoneCheckObjID].zoneList.matrixList[i], out var distance, out IntersectPosition, out IntersectNormal, out var _, threadID) && distance < num)
				{
					num = distance;
					result = IntersectPosition.Z;
				}
			}
		}
		return result;
	}
}
