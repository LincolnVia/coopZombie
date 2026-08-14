using System;
using System.Globalization;
using System.IO;
using MainGame;
using Microsoft.Xna.Framework;
using Players;
using Rendering;
using Terrain;
using Weapons;
using WindowsGame1;

namespace Levels;

public class Levels
{
	public static byte numSpLevels = 19;

	public static float[] slFar3 = new float[3];

	public static float[] slFar4 = new float[4];

	public static string mbRespawnMajor;

	public static string mbRespawnMinor;

	public Game1.MasterCollection mainC;

	public void Init_Vars(Game1.MasterCollection masterC)
	{
		mainC = masterC;
	}

	public void Init_Levels()
	{
		global::MainGame.MainGame.gameLevel = 0;
	}

	public void Load_Level(string fileName, byte threadID)
	{
		bool flag = false;
		switch (global::MainGame.MainGame.gameMode)
		{
		case 0:
			global::MainGame.MainGame.newVboID = global::MainGame.MainGame.curSpLevel;
			break;
		case 1:
			global::MainGame.MainGame.newVboID = global::MainGame.MainGame.mpLastLoadedLevel;
			break;
		}
		Stream stream = TitleContainer.OpenStream("The_CoOp_Zombie_Game\\Config_Files\\Levels\\" + fileName);
		byte[] array = new byte[stream.Length];
		mainC.maingameMain.Clear_Game_Items();
		mainC.aiMain.Clear_AI();
		global::MainGame.MainGame.idleTimeout = 10000000f;
		global::MainGame.MainGame.damageReduction = 0.15f;
		global::MainGame.MainGame.damageIncrease = 1f;
		mainC.soundsMain.Level_Reset();
		mbRespawnMinor = "MessageBox_Respawn";
		mbRespawnMajor = "MessageBox_Respawn";
		global::Weapons.Weapons.laserStartDistance = 10f;
		global::Weapons.Weapons.laserDistance = 1000f;
		global::Weapons.Weapons.numWeaponMounts = 0;
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
				if (array4[0].Equals("AI", StringComparison.OrdinalIgnoreCase))
				{
					num3 = 1;
				}
				else if (array4[0].Equals("Joints_Basic", StringComparison.OrdinalIgnoreCase))
				{
					num3 = 2;
				}
				else if (array4[0].Equals("Objects", StringComparison.OrdinalIgnoreCase))
				{
					num3 = 3;
				}
				else if (array4[0].Equals("Object_Collections", StringComparison.OrdinalIgnoreCase))
				{
					num3 = 4;
				}
				else if (array4[0].Equals("Pickups", StringComparison.OrdinalIgnoreCase))
				{
					num3 = 5;
				}
				else if (array4[0].Equals("Programs_Basic", StringComparison.OrdinalIgnoreCase))
				{
					num3 = 6;
				}
				else if (array4[0].Equals("Rendering_Instances", StringComparison.OrdinalIgnoreCase))
				{
					num3 = 7;
				}
				else if (array4[0].Equals("Spawn_Points", StringComparison.OrdinalIgnoreCase))
				{
					num3 = 8;
				}
				else if (array4[0].Equals("Switches", StringComparison.OrdinalIgnoreCase))
				{
					num3 = 9;
				}
				else if (array4[0].Equals("Collision", StringComparison.OrdinalIgnoreCase))
				{
					num3 = 10;
				}
				else if (array4[0].Equals("Ammo_Clips", StringComparison.OrdinalIgnoreCase))
				{
					num3 = 11;
				}
				else if (array4[0].Equals("Rendering", StringComparison.OrdinalIgnoreCase))
				{
					num3 = 12;
				}
				else if (array4[0].Equals("IdleTimeout", StringComparison.OrdinalIgnoreCase))
				{
					num3 = 13;
				}
				else if (array4[0].Equals("NavigationMesh", StringComparison.OrdinalIgnoreCase))
				{
					num3 = 14;
				}
				else if (array4[0].Equals("BossLevel", StringComparison.OrdinalIgnoreCase))
				{
					num3 = 18;
				}
				else if (array4[0].Equals("damageReduction", StringComparison.OrdinalIgnoreCase))
				{
					num3 = 19;
				}
				else if (array4[0].Equals("Textures", StringComparison.OrdinalIgnoreCase))
				{
					num3 = 20;
				}
				else if (array4[0].Equals("LevelMusic", StringComparison.OrdinalIgnoreCase))
				{
					num3 = 21;
				}
				else if (array4[0].Equals("MessageBoxMinorRespawn", StringComparison.OrdinalIgnoreCase))
				{
					num3 = 23;
				}
				else if (array4[0].Equals("MessageBoxMajorRespawn", StringComparison.OrdinalIgnoreCase))
				{
					num3 = 24;
				}
				else if (array4[0].Equals("CollisionModels", StringComparison.OrdinalIgnoreCase))
				{
					num3 = 25;
				}
				else if (array4[0].Equals("Avatars", StringComparison.OrdinalIgnoreCase))
				{
					num3 = 26;
				}
				else if (array4[0].Equals("TimeLimit", StringComparison.OrdinalIgnoreCase))
				{
					num3 = 27;
				}
				else if (array4[0].Equals("Laps", StringComparison.OrdinalIgnoreCase))
				{
					num3 = 28;
				}
				else if (array4[0].Equals("Level_Models", StringComparison.OrdinalIgnoreCase))
				{
					num3 = 29;
				}
				else if (array4[0].Equals("Graphs", StringComparison.OrdinalIgnoreCase))
				{
					num3 = 30;
				}
				else if (array4[0].Equals("Zones", StringComparison.OrdinalIgnoreCase))
				{
					num3 = 31;
				}
				else if (array4[0].Equals("Zone_Checks", StringComparison.OrdinalIgnoreCase))
				{
					num3 = 32;
				}
				else if (array4[0].Equals("Pickups_Ballistics", StringComparison.OrdinalIgnoreCase))
				{
					num3 = 33;
				}
				else if (array4[0].Equals("MiniMap_Data", StringComparison.OrdinalIgnoreCase))
				{
					num3 = 34;
				}
				else if (array4[0].Equals("Targets", StringComparison.OrdinalIgnoreCase))
				{
					num3 = 35;
				}
				else if (array4[0].Equals("Targets_Damage", StringComparison.OrdinalIgnoreCase))
				{
					num3 = 36;
				}
				else if (array4[0].Equals("Laser_Distance", StringComparison.OrdinalIgnoreCase))
				{
					num3 = 38;
				}
				else if (array4[0].Equals("Score_Limits", StringComparison.OrdinalIgnoreCase))
				{
					num3 = 39;
				}
				else if (array4[0].Equals("Weapon_Mounts", StringComparison.OrdinalIgnoreCase))
				{
					num3 = 40;
				}
				else if (array4[0].Equals("VBOID", StringComparison.OrdinalIgnoreCase))
				{
					num3 = 41;
				}
				else if (array4[0].Equals("Game_Objects", StringComparison.OrdinalIgnoreCase))
				{
					num3 = 42;
				}
				else if (array4[0].Equals("TerrainModel", StringComparison.OrdinalIgnoreCase))
				{
					num3 = 43;
				}
				else if (array4[0].Equals("TerrainCollisionModel", StringComparison.OrdinalIgnoreCase))
				{
					num3 = 44;
				}
				else if (array4[0].Equals("TerrainHeightMap", StringComparison.OrdinalIgnoreCase))
				{
					num3 = 45;
				}
				else if (array4[0].Equals("damageIncrease", StringComparison.OrdinalIgnoreCase))
				{
					num3 = 46;
				}
				else if (array4[0].Equals("TerrainCollisionModelTile", StringComparison.OrdinalIgnoreCase))
				{
					num3 = 47;
				}
				switch (num3)
				{
				case 1:
					if (array4.Length > 1)
					{
						mainC.aiMain.Load_AI_Data(array4[1]);
					}
					break;
				case 2:
					if (array4.Length > 1)
					{
						mainC.jointsMain.Load_Joints_Basic(array4[1]);
					}
					break;
				case 3:
					if (array4.Length > 1 && global::MainGame.MainGame.curVboID != global::MainGame.MainGame.newVboID)
					{
						mainC.gameobjectMain.loadObjects(array4[1], threadID);
					}
					break;
				case 4:
					if (array4.Length > 1 && global::MainGame.MainGame.curVboID != global::MainGame.MainGame.newVboID)
					{
						mainC.gameobjectMain.Load_Object_Collections(array4[1]);
					}
					break;
				case 5:
					if (array4.Length > 1)
					{
						mainC.pickupsMain.Load_Pickups_Data(array4[1]);
					}
					break;
				case 6:
					if (array4.Length > 1)
					{
						mainC.programsMain.Load_Programs_Basic(array4[1]);
					}
					break;
				case 7:
					if (array4.Length > 1 && global::MainGame.MainGame.curVboID != global::MainGame.MainGame.newVboID)
					{
						mainC.renderingMain.Load_Instancing_Data(array4[1], threadID);
					}
					break;
				case 8:
					if (array4.Length > 1)
					{
						mainC.mapsMain.Load_Map_Data(array4[1], needReset: true);
					}
					break;
				case 9:
					if (array4.Length > 1)
					{
						mainC.switchesMain.Load_Switches_Data(array4[1]);
					}
					break;
				case 10:
					if (array4.Length > 1 && global::MainGame.MainGame.curVboID != global::MainGame.MainGame.newVboID)
					{
						mainC.collisionMain.Load_Collsion_Boundaries(array4[1]);
					}
					break;
				case 11:
					if (array4.Length > 1)
					{
						mainC.weaponsMain.Load_Level_Data_Ammo_Clips(array4[1]);
					}
					break;
				case 12:
					if (array4.Length > 1)
					{
						mainC.renderingMain.Load_Rendering_Data(array4[1]);
					}
					break;
				case 13:
					if (array4.Length > 1)
					{
						global::MainGame.MainGame.idleTimeout = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 14:
					if (array4.Length > 2)
					{
						global::MainGame.MainGame.NavigationMesh.LoadNavigationMesh("The_CoOp_Zombie_Game\\CollisionData\\" + array4[1] + "Xbox360");
						global::MainGame.MainGame.numNavRoutes = ushort.Parse(array4[2], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
						global::MainGame.MainGame.routePolys = new ushort[global::MainGame.MainGame.numNavRoutes];
					}
					break;
				case 18:
					if (array4.Length > 1)
					{
						mainC.aiMain.Load_Boss_Data(array4[1], threadID);
					}
					break;
				case 19:
					global::MainGame.MainGame.damageReduction = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
					break;
				case 20:
					if (array4.Length > 1 && global::MainGame.MainGame.curVboID != global::MainGame.MainGame.newVboID)
					{
						mainC.texturesMain.Textures_Load_Level(array4[1]);
					}
					break;
				case 21:
					if (array4.Length > 1)
					{
						mainC.soundsMain.Set_Level_Starting_Music(byte.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat), playNow: false);
					}
					break;
				case 23:
					if (array4.Length > 1)
					{
						mbRespawnMinor = array4[1];
					}
					break;
				case 24:
					if (array4.Length > 1)
					{
						mbRespawnMajor = array4[1];
					}
					break;
				case 25:
					if (array4.Length > 1 && global::MainGame.MainGame.curVboID != global::MainGame.MainGame.newVboID)
					{
						mainC.collisionMain.Load_Collision_Models(array4[1]);
					}
					break;
				case 26:
					if (array4.Length > 1)
					{
						mainC.avatarMain.Load_Avatar_Data(array4[1]);
					}
					break;
				case 27:
					if (array4.Length > 1)
					{
						global::MainGame.MainGame.roundTimeLimit = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 28:
					if (array4.Length > 1)
					{
						global::MainGame.MainGame.levelLapsToFinish = byte.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 29:
					if (array4.Length > 1 && global::MainGame.MainGame.curVboID != global::MainGame.MainGame.newVboID)
					{
						mainC.modelsMain.Load_All_Level_Models(array4[1]);
					}
					break;
				case 30:
					if (array4.Length > 1)
					{
						mainC.graphingMain.Load_Graph_Data(array4[1]);
					}
					break;
				case 31:
					if (array4.Length > 1 && global::MainGame.MainGame.curVboID != global::MainGame.MainGame.newVboID)
					{
						mainC.zonesMain.Load_Zone_Data(array4[1]);
					}
					break;
				case 32:
					if (array4.Length > 1)
					{
						mainC.zonesMain.Load_Zone_Check_Data(array4[1]);
					}
					break;
				case 33:
					if (array4.Length > 1)
					{
						mainC.pickupsMain.Load_Pickups_Ballistic_Data(array4[1]);
					}
					break;
				case 34:
					if (array4.Length > 1)
					{
						mainC.renderingMain.Load_MiniMap_Data(array4[1]);
					}
					break;
				case 35:
					if (array4.Length > 1)
					{
						mainC.targetMain.Load_Target_Data(array4[1]);
					}
					break;
				case 36:
					if (array4.Length > 1)
					{
						mainC.targetMain.Load_Damage_Target_Data(array4[1]);
					}
					break;
				case 38:
					if (array4.Length > 2)
					{
						global::Weapons.Weapons.laserStartDistance = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
						global::Weapons.Weapons.laserDistance = float.Parse(array4[2], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 39:
					if (array4.Length > 2)
					{
						global::MainGame.MainGame.roundScoreLimit = int.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
						global::MainGame.MainGame.roundMinScoreLimit = int.Parse(array4[2], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 40:
					if (array4.Length > 1)
					{
						mainC.weaponsMain.Load_Weapon_Mounts_Player(array4[1]);
					}
					break;
				case 41:
					if (array4.Length > 1)
					{
						global::MainGame.MainGame.newVboID = short.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
						if (global::MainGame.MainGame.curVboID != global::MainGame.MainGame.newVboID)
						{
							mainC.maingameMain.Clear_Game_Items();
						}
					}
					break;
				case 42:
					if (array4.Length > 1 && global::MainGame.MainGame.curVboID != global::MainGame.MainGame.newVboID)
					{
						mainC.gameobjectMain.Load_Objects(array4[1], threadID);
					}
					break;
				case 43:
					if (array4.Length > 7)
					{
						flag = true;
						if (global::MainGame.MainGame.curVboID != global::MainGame.MainGame.newVboID)
						{
							mainC.terrainMain.Init_Terrain(array4[1], array4[7], useModelForTerrain: true, float.Parse(array4[2], CultureInfo.InvariantCulture.NumberFormat), float.Parse(array4[3], CultureInfo.InvariantCulture.NumberFormat), float.Parse(array4[4], CultureInfo.InvariantCulture.NumberFormat), float.Parse(array4[5], CultureInfo.InvariantCulture.NumberFormat), float.Parse(array4[6], CultureInfo.InvariantCulture.NumberFormat));
						}
					}
					break;
				case 44:
					if (array4.Length > 3 && global::MainGame.MainGame.curVboID != global::MainGame.MainGame.newVboID)
					{
						mainC.terrainMain.Set_Terrain_Collision_Model(array4[1], ushort.Parse(array4[2], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat), ushort.Parse(array4[3], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat));
					}
					break;
				case 45:
					if (array4.Length > 5)
					{
						flag = true;
						if (global::MainGame.MainGame.curVboID != global::MainGame.MainGame.newVboID)
						{
							mainC.terrainMain.Init_Terrain(array4[1], array4[5], useModelForTerrain: false, float.Parse(array4[2], CultureInfo.InvariantCulture.NumberFormat), float.Parse(array4[3], CultureInfo.InvariantCulture.NumberFormat), float.Parse(array4[4], CultureInfo.InvariantCulture.NumberFormat), float.Parse(array4[4], CultureInfo.InvariantCulture.NumberFormat), 0f);
						}
					}
					break;
				case 46:
					global::MainGame.MainGame.damageIncrease = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
					break;
				case 47:
					if (array4.Length > 4 && global::MainGame.MainGame.curVboID != global::MainGame.MainGame.newVboID)
					{
						mainC.terrainMain.Set_Terrain_Collision_Model_Tile(array4[1], ushort.Parse(array4[2], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat), float.Parse(array4[3], CultureInfo.InvariantCulture.NumberFormat), float.Parse(array4[4], CultureInfo.InvariantCulture.NumberFormat));
					}
					break;
				}
			}
		}
		stream.Close();
		if (!flag)
		{
			global::Terrain.Terrain.terrainMode = 0;
			global::Terrain.Terrain.terrainBaseHeight = global::MainGame.MainGame.MaxDown;
		}
		mainC.targetMain.Add_Damage_Targets_To_Minimap();
		mainC.zonesMain.Set_Indexes_To_IDs();
		mainC.texturesMain.Reset_Texture_IDs_After_Texture_Change();
		mainC.targetMain.Update_DamageTarget_MiniMap_Textures();
		mainC.renderingMain.Level_Reset_Texture_List();
		mainC.gameobjectMain.Setup_Object_Collections();
		mainC.renderingMain.Created_Rigged_Model_Texture_List();
		if (global::MainGame.MainGame.curVboID != global::MainGame.MainGame.newVboID)
		{
			mainC.modelsMain.Create_Level_Model_VBO_Initial();
			mainC.renderingMain.Initialize_Instancing_Objects(threadID);
		}
		if (global::Players.Players.numAllocLvlSortArray < global::Rendering.Rendering.numPtLight_lvl)
		{
			global::Players.Players.lvlLightSortArray = new float[global::Rendering.Rendering.numPtLight_lvl];
			global::Rendering.Rendering.closestLevelLightsIndices = new short[2, global::Rendering.Rendering.numPtLight_lvl];
		}
		mainC.aiMain.Create_Nav_Routes();
		mainC.programsMain.Reset_Round(minorRestart: false);
		mainC.gameobjectMain.Reset_Round(minorReset: false, threadID);
	}

	public void Load_Level_Section(string fileName, byte sectionNumber, byte threadID)
	{
		Stream stream = TitleContainer.OpenStream("The_CoOp_Zombie_Game\\Config_Files\\Levels\\" + fileName);
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
				if (array4[0].Equals("Spawn_Points", StringComparison.OrdinalIgnoreCase) && sectionNumber == 8)
				{
					num3 = 8;
				}
				int num4 = num3;
				if (num4 == 8 && array4.Length > 1)
				{
					mainC.mapsMain.Load_Map_Data(array4[1], needReset: false);
				}
			}
		}
		stream.Close();
	}

	public byte Get_Next_SP_Level()
	{
		if (!global::MainGame.MainGame.linearProgression)
		{
			return 0;
		}
		if (++global::MainGame.MainGame.gameLevel >= numSpLevels)
		{
			global::MainGame.MainGame.gameLevel = 0;
			return 1;
		}
		if (global::MainGame.MainGame.trialMode && global::MainGame.MainGame.gameLevel > 2)
		{
			global::MainGame.MainGame.gameLevel = 2;
			global::Rendering.Rendering.mbTrialOver = true;
			return 2;
		}
		return 0;
	}

	public void Set_Level(byte levelID, byte threadID)
	{
		switch (global::MainGame.MainGame.gameMode)
		{
		case 0:
			if (global::MainGame.MainGame.trialMode && levelID > 2)
			{
				levelID = 2;
			}
			if (levelID >= numSpLevels)
			{
				return;
			}
			break;
		case 1:
			if (levelID >= mainC.maingameMain.Get_Num_MP_Levels())
			{
				return;
			}
			break;
		}
		switch (global::MainGame.MainGame.gameMode)
		{
		case 0:
			global::MainGame.MainGame.curSpLevel = levelID;
			global::MainGame.MainGame.spLastLoadedLevel = levelID;
			global::MainGame.MainGame.spLastLoadeGameType = global::MainGame.MainGame.gameType;
			global::MainGame.MainGame.mpLastLoadedLevel = byte.MaxValue;
			Load_Level("SP_" + global::MainGame.MainGame.gameType.ToString() + "_" + global::MainGame.MainGame.curSpLevel + "_" + global::MainGame.MainGame.difficulty + ".txt", threadID);
			break;
		case 1:
			global::MainGame.MainGame.mpLastLoadedLevel = levelID;
			global::MainGame.MainGame.mpLastLoadeGameType = global::MainGame.MainGame.gameType;
			global::MainGame.MainGame.curSpLevel = -1;
			global::MainGame.MainGame.spLastLoadedLevel = byte.MaxValue;
			Load_Level("MP_" + global::MainGame.MainGame.gameType.ToString() + "_" + levelID + "_" + global::MainGame.MainGame.difficulty + ".txt", threadID);
			break;
		}
		mainC.renderingMain.Create_Rendering_VBOs();
	}

	public void Setup_Level()
	{
		if (global::Players.Players.numAllocLvlSortArray < global::Rendering.Rendering.numPtLight_lvl)
		{
			global::Players.Players.lvlLightSortArray = new float[global::Rendering.Rendering.numPtLight_lvl];
			global::Rendering.Rendering.closestLevelLightsIndices = new short[2, global::Rendering.Rendering.numPtLight_lvl];
		}
		mainC.renderingMain.Set_Level_Lights();
		mainC.playersMain.Set_Player_Headlamp_For_Level(0, 0f);
		global::Rendering.Rendering.effect1.Parameters["PtLightDistance0"].SetValue(8192f);
		global::Rendering.Rendering.effect1.Parameters["Ambient"].SetValue(global::Rendering.Rendering.ambientLevel);
		if (global::MainGame.MainGame.gameMode == 0)
		{
			mainC.aiMain.Change_AI_Difficulty(global::MainGame.MainGame.difficulty);
		}
	}

	public void Generate_Level_Data()
	{
		global::MainGame.MainGame.gameMode = 0;
		for (byte b = 0; b < numSpLevels; b++)
		{
			global::MainGame.MainGame.gameLevel = b;
			mainC.levelsMain.Set_Level(b, 1);
			mainC.renderingMain.Create_Rendering_VBOs();
		}
		global::MainGame.MainGame.gameMode = 1;
		ushort num_MP_Levels = mainC.maingameMain.Get_Num_MP_Levels();
		for (byte b = 0; b < num_MP_Levels; b++)
		{
			global::MainGame.MainGame.gameLevel = b;
			mainC.levelsMain.Set_Level(b, 1);
			mainC.renderingMain.Create_Rendering_VBOs();
		}
	}

	public string Get_ConfigFile_Name(string fileName, byte fileType)
	{
		string text = "";
		Stream stream = TitleContainer.OpenStream("The_CoOp_Zombie_Game\\Config_Files\\Levels\\" + fileName);
		byte[] array = new byte[stream.Length];
		if (stream.CanRead)
		{
			stream.Read(array, 0, array.Length);
			text = mainC.utilMain.Byte_Array_To_String(array);
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
				return text;
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
				if (array4[0].Equals("Textures", StringComparison.OrdinalIgnoreCase))
				{
					num3 = 1;
				}
				else if (array4[0].Equals("Level_Models", StringComparison.OrdinalIgnoreCase))
				{
					num3 = 2;
				}
				else if (array4[0].Equals("Objects", StringComparison.OrdinalIgnoreCase))
				{
					num3 = 3;
				}
				switch (num3)
				{
				case 1:
					if (fileType == 0 && array4.Length > 1)
					{
						stream.Close();
						return array4[1];
					}
					break;
				case 2:
					if (fileType == 1 && array4.Length > 1)
					{
						stream.Close();
						return array4[1];
					}
					break;
				case 3:
					if (fileType == 2 && array4.Length > 1)
					{
						stream.Close();
						return array4[1];
					}
					break;
				}
			}
		}
		stream.Close();
		return text;
	}
}
