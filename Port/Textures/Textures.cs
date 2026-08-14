using System;
using System.Globalization;
using System.IO;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Structs;
using WindowsGame1;

namespace Textures;

public class Textures
{
	public static byte numTargetingCrosshairs = 0;

	public static ushort[] texCrossHair;

	public static ushort[] texDigits;

	public static short numTextures = 0;

	public static short maxTextures = 0;

	public static short numBaseTextures = 0;

	public static short numAlphaTextures = 0;

	public static short texDefaultNormalMap;

	public static short texDefaultSpecularMap;

	public static short texTauntMessage;

	public static short texSwapWeapon;

	public static short texPopUp0;

	public static short texScope;

	public static short texWhite;

	public static short texBlack;

	public static short texUI_Icon_Dpad;

	public static short texDefaultGamerPicture;

	public static short texHUD_PressX;

	public static short texHudMiniMapFrame;

	public static short texHudMiniMap;

	public static ushort texRankInsignia;

	public static ushort texKillStreak;

	public static StructsClass.textureInfo texMain;

	public static EventWaitHandle texChangeStart = new AutoResetEvent(initialState: false);

	public static EventWaitHandle texChangeEnd = new AutoResetEvent(initialState: false);

	public Game1.MasterCollection mainC;

	public void Init_Vars(Game1.MasterCollection masterC)
	{
		mainC = masterC;
	}

	public void Init_Textures()
	{
		Load_Texture_Data("Textures.txt", 0);
		texTauntMessage = mainC.texturesMain.Find_Texture("HUD_Taunt", 0);
		texSwapWeapon = mainC.texturesMain.Find_Texture("HUD_Swap_Weapon", 0);
		texDefaultNormalMap = mainC.texturesMain.Find_Texture("default_normal", 0);
		texDefaultSpecularMap = mainC.texturesMain.Find_Texture("default_specular", 0);
		texDefaultGamerPicture = Find_Texture("DefaultGamerPicture", 0);
		texWhite = Find_Texture("White", 0);
		texBlack = Find_Texture("Black", 0);
		texScope = Find_Texture("Scope", 0);
		texUI_Icon_Dpad = Find_Texture("UI_Icon_Dpad", 0);
		texHUD_PressX = Find_Texture("HUD_PressX", 0);
		texDigits = new ushort[13];
		texDigits[0] = (ushort)Find_Texture("Hud_Digit_0", 0);
		texDigits[1] = (ushort)Find_Texture("Hud_Digit_1", 0);
		texDigits[2] = (ushort)Find_Texture("Hud_Digit_2", 0);
		texDigits[3] = (ushort)Find_Texture("Hud_Digit_3", 0);
		texDigits[4] = (ushort)Find_Texture("Hud_Digit_4", 0);
		texDigits[5] = (ushort)Find_Texture("Hud_Digit_5", 0);
		texDigits[6] = (ushort)Find_Texture("Hud_Digit_6", 0);
		texDigits[7] = (ushort)Find_Texture("Hud_Digit_7", 0);
		texDigits[8] = (ushort)Find_Texture("Hud_Digit_8", 0);
		texDigits[9] = (ushort)Find_Texture("Hud_Digit_9", 0);
		texDigits[10] = (ushort)Find_Texture("Hud_Digit_Comma", 0);
		texDigits[11] = (ushort)Find_Texture("Hud_Digit_Colon", 0);
		texDigits[12] = (ushort)Find_Texture("Hud_Digit_Minus", 0);
	}

	public void Textures_Load_Level(string fileName)
	{
		mainC.curGame.CM_Level.Unload();
		Load_Texture_Data(fileName, numBaseTextures);
	}

	public void Load_Texture_Data(string fileName, short startingTexture)
	{
		int num = -1;
		int numAllocTex = texMain.numAllocTex;
		for (int i = startingTexture; i < numAllocTex; i++)
		{
			texMain.isAlpha[i] = false;
		}
		num = startingTexture - 1;
		Stream stream = TitleContainer.OpenStream("The_CoOp_Zombie_Game\\Config_Files\\" + fileName);
		byte[] array = new byte[stream.Length];
		if (stream.CanRead)
		{
			stream.Read(array, 0, array.Length);
			string text = mainC.utilMain.Byte_Array_To_String(array);
			string[] array2 = text.Split('\n', '\r');
			numAllocTex = 0;
			int num2 = 0;
			for (; numAllocTex < array2.Length; numAllocTex++)
			{
				if (array2[numAllocTex].Length > 0)
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
			numAllocTex = 0;
			num2 = 0;
			for (; numAllocTex < array2.Length; numAllocTex++)
			{
				if (array2[numAllocTex].Length > 0)
				{
					array3[num2++] = array2[numAllocTex];
				}
			}
			for (numAllocTex = 0; numAllocTex < num2; numAllocTex++)
			{
				array2 = array3[numAllocTex].Split(' ', '\t');
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
				if (array4[0].Equals("numTextures", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 1;
				}
				else if (array4[0].Equals("Texture", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 2;
				}
				else if (array4[0].Equals("FileName", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 3;
				}
				else if (array4[0].Equals("AlphaTexture", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 4;
				}
				else if (array4[0].Equals("maxTextures", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 5;
				}
				else if (array4[0].Equals("numBasedTextures", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 6;
				}
				switch (num4)
				{
				case 1:
					if (array4.Length <= 1)
					{
						break;
					}
					numTextures = short.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
					numTextures += startingTexture;
					if (numTextures > maxTextures)
					{
						maxTextures = numTextures;
					}
					if (texMain.numAllocTex < maxTextures)
					{
						texMain.numAllocTex = maxTextures;
						texMain.texNames = new string[maxTextures];
						texMain.texID = new short[maxTextures];
						texMain.texData = new Texture2D[maxTextures];
						texMain.isAlpha = new bool[maxTextures];
						for (int i = 0; i < maxTextures; i++)
						{
							texMain.isAlpha[i] = false;
						}
					}
					texMain.numTex = numTextures;
					break;
				case 2:
					num++;
					if (num < 0 || num >= numTextures)
					{
						num = -1;
					}
					break;
				case 3:
					if (array4.Length > 1 && num > -1)
					{
						texMain.texData[num] = mainC.curGame.Content.Load<Texture2D>("Textures\\" + array4[1]);
						texMain.texID[num] = (short)num;
						texMain.texNames[num] = array4[1];
					}
					break;
				case 4:
					if (num > -1)
					{
						texMain.isAlpha[num] = true;
					}
					break;
				case 5:
					if (array4.Length > 1)
					{
						maxTextures = short.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 6:
					if (array4.Length > 1)
					{
						numBaseTextures = short.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				}
			}
		}
		stream.Close();
		numAlphaTextures = 0;
		for (int i = 0; i < numTextures; i++)
		{
			if (texMain.isAlpha[i])
			{
				numAlphaTextures++;
			}
		}
	}

	public short Find_Texture(string texture, short defaultVal)
	{
		short numTex = texMain.numTex;
		for (short num = 0; num < numTex; num++)
		{
			if (texMain.texNames[num].Equals(texture, StringComparison.OrdinalIgnoreCase))
			{
				return num;
			}
		}
		return defaultVal;
	}

	public void Reset_Texture_IDs_After_Texture_Change()
	{
		mainC.modelsMain.Set_Model_Textures();
		mainC.renderingMain.Update_Rendering_Textures();
		mainC.targetMain.Update_DamageTarget_MiniMap_Textures();
		mainC.terrainMain.Update_Terrain_Textures();
		mainC.playersMain.Set_Player_Textures();
		mainC.userInterface.Reset_User_Interface_Textures();
		mainC.gameLogic.Game_Reset_Textures();
	}
}
