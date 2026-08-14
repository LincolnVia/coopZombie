using System;
using System.Globalization;
using System.IO;
using InputHandler;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Models;
using Players;
using Rendering;
using Sounds;
using Structs;
using Textures;
using Util;
using Weapons;
using WindowsGame1;

namespace MainGame;

public class User_Interface
{
	public static byte numWindows = 0;

	public static byte curInstructionPage = 0;

	public static byte weaponSelectWeaponID;

	public static byte weaponSelectAttachmentTypeColumnCount;

	public static byte weaponSelectAttachmentCount;

	public static byte weaponSelectAttachmentType;

	public static bool playerChangedWeaponOptions = false;

	public static bool hideVehicle = false;

	public static bool hideWeapon = false;

	public static bool weaponSelectScreenOpen = false;

	public static bool vehicleSelectScreenOpen = false;

	public static bool missionObjectivesScreenOpen = false;

	public static bool weaponSelectFinished = false;

	public static bool vehicleSelectFinished = false;

	public static bool missionObjectivesFinished = false;

	public static ushort curWindow = 0;

	public static ushort componentType;

	public static ushort curVehicleSelect = 0;

	public static ushort lastVehicleSelected = 0;

	public static ushort curWeaponSelectColumn;

	public static ushort curWeaponSelectColumn2;

	public static ushort lastWeaponSelected;

	public static ushort curWeaponSelectArea = 0;

	public static ushort weaponSelectAttachmentTypeColumn = 0;

	public static ushort weaponSelectAttachmentItemColumn = 0;

	public static ushort weaponSelectSkinColumn = 0;

	public static ushort tauntColumn = 0;

	public static ushort newScopeID;

	public static ushort newForeGripID;

	public static ushort newBarrellID;

	public static ushort newEnergyDeviceID;

	public static ushort newSkinID;

	public static ushort newTauntID;

	public static ushort nextWindowPlace = 0;

	public static ushort[] windowOrder;

	public static float mainMenuErrorTime = 0f;

	public static float idleTimeOut;

	public static float vehicleSelectTimer = 0f;

	public static float vehicleSelectAutoStartTime = 15f;

	public static float weaponSelectTimer = 0f;

	public static float weaponSelectAutoStartTime = 15f;

	public static float thumbStickRepeatValueX;

	public static float thumbStickRepeatValueY;

	public static StructsClass.UI_Window[] windows;

	public static SpriteBatch splashSprite;

	public static StructsClass.UI_Window_List[] windowIDs;

	public Game1.MasterCollection mainC;

	public void Init_Vars(Game1.MasterCollection master)
	{
		mainC = master;
	}

	public void Initialize_User_Interface()
	{
		splashSprite = new SpriteBatch(global::Rendering.Rendering.rGraphics);
		Load_User_Interface("UI.txt");
	}

	public void Load_User_Interface(string fileName)
	{
		ushort num = 0;
		ushort num2 = 0;
		ushort num3 = 0;
		ushort num4 = 0;
		ushort num5 = 0;
		ushort num6 = 0;
		ushort num7 = 0;
		ushort num8 = 0;
		int num9 = -1;
		_ = global::Rendering.Rendering.uBufferID;
		for (int i = 0; i < numWindows; i++)
		{
			windows[i].status = 0;
			windows[i].state = 0;
		}
		Stream stream = TitleContainer.OpenStream("The_CoOp_Zombie_Game\\Config_Files\\" + fileName);
		byte[] array = new byte[stream.Length];
		if (stream.CanRead)
		{
			stream.Read(array, 0, array.Length);
			string text = mainC.utilMain.Byte_Array_To_String(array);
			string[] array2 = text.Split('\n', '\r');
			int j = 0;
			int num10 = 0;
			for (; j < array2.Length; j++)
			{
				if (array2[j].Length > 0)
				{
					num10++;
				}
			}
			if (num10 < 1)
			{
				stream.Close();
				return;
			}
			string[] array3 = new string[num10];
			j = 0;
			num10 = 0;
			for (; j < array2.Length; j++)
			{
				if (array2[j].Length > 0)
				{
					array3[num10++] = array2[j];
				}
			}
			for (j = 0; j < num10; j++)
			{
				array2 = array3[j].Split(' ', '\t');
				int k = 0;
				int num11 = 0;
				for (; k < array2.Length; k++)
				{
					if (array2[k].Length > 0)
					{
						num11++;
					}
				}
				if (num11 < 1)
				{
					continue;
				}
				string[] array4 = new string[num11];
				k = 0;
				num11 = 0;
				for (; k < array2.Length; k++)
				{
					if (array2[k].Length > 0)
					{
						array4[num11++] = array2[k];
					}
				}
				int num12 = 0;
				if (array4[0].Equals("numWindows", StringComparison.OrdinalIgnoreCase))
				{
					num12 = 1;
				}
				else if (array4[0].Equals("window", StringComparison.OrdinalIgnoreCase))
				{
					num12 = 2;
				}
				else if (array4[0].Equals("position", StringComparison.OrdinalIgnoreCase))
				{
					num12 = 3;
				}
				else if (array4[0].Equals("label", StringComparison.OrdinalIgnoreCase))
				{
					num12 = 4;
				}
				else if (array4[0].Equals("model", StringComparison.OrdinalIgnoreCase))
				{
					num12 = 5;
				}
				else if (array4[0].Equals("numCheckBoxes", StringComparison.OrdinalIgnoreCase))
				{
					num12 = 6;
				}
				else if (array4[0].Equals("button", StringComparison.OrdinalIgnoreCase))
				{
					num12 = 7;
				}
				else if (array4[0].Equals("numTextComponents", StringComparison.OrdinalIgnoreCase))
				{
					num12 = 8;
				}
				else if (array4[0].Equals("numButtonComponents", StringComparison.OrdinalIgnoreCase))
				{
					num12 = 9;
				}
				else if (array4[0].Equals("MainComponent", StringComparison.OrdinalIgnoreCase))
				{
					num12 = 10;
				}
				else if (array4[0].Equals("numSliders", StringComparison.OrdinalIgnoreCase))
				{
					num12 = 11;
				}
				else if (array4[0].Equals("checkbox", StringComparison.OrdinalIgnoreCase))
				{
					num12 = 12;
				}
				else if (array4[0].Equals("slider", StringComparison.OrdinalIgnoreCase))
				{
					num12 = 13;
				}
				else if (array4[0].Equals("numGroups", StringComparison.OrdinalIgnoreCase))
				{
					num12 = 14;
				}
				else if (array4[0].Equals("group", StringComparison.OrdinalIgnoreCase))
				{
					num12 = 15;
				}
				else if (array4[0].Equals("WindowSounds", StringComparison.OrdinalIgnoreCase))
				{
					num12 = 16;
				}
				else if (array4[0].Equals("model_texture", StringComparison.OrdinalIgnoreCase))
				{
					num12 = 17;
				}
				else if (array4[0].Equals("model_Scale", StringComparison.OrdinalIgnoreCase))
				{
					num12 = 18;
				}
				else if (array4[0].Equals("numTextButtonComponents", StringComparison.OrdinalIgnoreCase))
				{
					num12 = 19;
				}
				else if (array4[0].Equals("textbutton", StringComparison.OrdinalIgnoreCase))
				{
					num12 = 20;
				}
				else if (array4[0].Equals("numTextAreaComponents", StringComparison.OrdinalIgnoreCase))
				{
					num12 = 21;
				}
				else if (array4[0].Equals("textArea", StringComparison.OrdinalIgnoreCase))
				{
					num12 = 22;
				}
				else if (array4[0].Equals("windowType", StringComparison.OrdinalIgnoreCase))
				{
					num12 = 23;
				}
				else if (array4[0].Equals("ButtonFlags", StringComparison.OrdinalIgnoreCase))
				{
					num12 = 24;
				}
				else if (array4[0].Equals("WindowCloseFlags", StringComparison.OrdinalIgnoreCase))
				{
					num12 = 25;
				}
				else if (array4[0].Equals("Static_Graphic", StringComparison.OrdinalIgnoreCase))
				{
					num12 = 26;
				}
				else if (array4[0].Equals("numStaticGraphics", StringComparison.OrdinalIgnoreCase))
				{
					num12 = 27;
				}
				switch (num12)
				{
				case 1:
					if (array4.Length > 1)
					{
						int num13 = int.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
						windows = new StructsClass.UI_Window[num13];
						for (int i = 0; i < num13; i++)
						{
							windows[i].type = 0;
							windows[i].status = 0;
							windows[i].state = 0;
							windows[i].curButton = 0;
							windows[i].curCheckBox = 0;
							windows[i].curSlider = 0;
							windows[i].curLabel = 0;
							windows[i].numButtons = 0;
							windows[i].numCheckBoxes = 0;
							windows[i].numSliders = 0;
							windows[i].numLabels = 0;
							windows[i].numTextAreas = 0;
							windows[i].modScaleX = 1f;
							windows[i].modScaleY = 1f;
							windows[i].modScaleZ = 1f;
							windows[i].buttonFlags = 0;
							windows[i].windowCloseFlags = 0;
							windows[i].needsUpdating = false;
							windows[i].curTab = 0;
							windows[i].ignoreStickInputs = false;
						}
						numWindows = (byte)num13;
					}
					break;
				case 2:
					if (array4.Length > 1)
					{
						num9 = int.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
						if (num9 < 0 || num9 >= numWindows)
						{
							num9 = -1;
						}
						num = 0;
						num2 = 0;
					}
					break;
				case 3:
					if (array4.Length > 2 && num9 > -1)
					{
						windows[num9].x = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
						windows[num9].y = float.Parse(array4[2], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 4:
					if (array4.Length > 12 && num9 > -1)
					{
						windows[num9].labels[num].id = ushort.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
						windows[num9].labels[num].fontID = byte.Parse(array4[2], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
						windows[num9].labels[num].centering = byte.Parse(array4[3], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
						windows[num9].labels[num].x = float.Parse(array4[4], CultureInfo.InvariantCulture.NumberFormat);
						windows[num9].labels[num].y = float.Parse(array4[5], CultureInfo.InvariantCulture.NumberFormat);
						windows[num9].labels[num].r = byte.Parse(array4[6], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
						windows[num9].labels[num].g = byte.Parse(array4[7], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
						windows[num9].labels[num].b = byte.Parse(array4[8], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
						windows[num9].labels[num].a = byte.MaxValue;
						windows[num9].labels[num].hr = byte.Parse(array4[9], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
						windows[num9].labels[num].hg = byte.Parse(array4[10], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
						windows[num9].labels[num].hb = byte.Parse(array4[11], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
						windows[num9].labels[num].ha = byte.MaxValue;
						windows[num9].labels[num].labelText = array4[12];
						int num13 = array4.Length;
						for (int i = 13; i < num13; i++)
						{
							windows[num9].labels[num].labelText = windows[num9].labels[num].labelText + " " + array4[i];
						}
						windows[num9].labels[num].labelText = windows[num9].labels[num].labelText.Replace("\\n", "\n");
						num++;
					}
					break;
				case 5:
					if (array4.Length > 1 && num9 > -1)
					{
						windows[num9].modID = mainC.modelsMain.Find_Model(array4[1]);
					}
					break;
				case 6:
					if (array4.Length <= 1)
					{
						break;
					}
					num4 = 0;
					windows[num9].numCheckBoxes = byte.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
					if (windows[num9].numCheckBoxes > 0)
					{
						windows[num9].checkBoxes = new StructsClass.UI_Window_Component_Checkbox[windows[num9].numCheckBoxes];
						for (int i = 0; i < windows[num9].numCheckBoxes; i++)
						{
							windows[num9].checkBoxes[i].status = 0;
							windows[num9].checkBoxes[i].iconID = global::Util.Util.maxUnsignedShortValue;
						}
					}
					break;
				case 7:
					if (array4.Length > 17 && num9 > -1)
					{
						windows[num9].buttons[num2].id = ushort.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
						windows[num9].buttons[num2].x = float.Parse(array4[2], CultureInfo.InvariantCulture.NumberFormat);
						windows[num9].buttons[num2].y = float.Parse(array4[3], CultureInfo.InvariantCulture.NumberFormat);
						windows[num9].buttons[num2].type = byte.Parse(array4[4], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
						windows[num9].buttons[num2].clickGroup = byte.Parse(array4[5], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
						windows[num9].buttons[num2].clickAction = byte.Parse(array4[6], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
						windows[num9].buttons[num2].componentDown = byte.Parse(array4[7], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
						windows[num9].buttons[num2].componentUp = byte.Parse(array4[8], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
						windows[num9].buttons[num2].componentLeft = byte.Parse(array4[9], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
						windows[num9].buttons[num2].componentRight = byte.Parse(array4[10], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
						windows[num9].buttons[num2].icon = array4[11];
						windows[num9].buttons[num2].iconX = float.Parse(array4[12], CultureInfo.InvariantCulture.NumberFormat);
						windows[num9].buttons[num2].iconY = float.Parse(array4[13], CultureInfo.InvariantCulture.NumberFormat);
						windows[num9].buttons[num2].soundClick = array4[14];
						windows[num9].buttons[num2].soundFocus = array4[15];
						windows[num9].buttons[num2].button1 = array4[16];
						windows[num9].buttons[num2].button2 = array4[17];
						windows[num9].buttons[num2].inGroup = false;
						num2++;
					}
					break;
				case 8:
					if (array4.Length <= 1)
					{
						break;
					}
					num = 0;
					windows[num9].numLabels = byte.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
					if (windows[num9].numLabels > 0)
					{
						windows[num9].labels = new StructsClass.UI_Window_Component_TextLabel[windows[num9].numLabels];
						for (int i = 0; i < windows[num9].numLabels; i++)
						{
							windows[num9].labels[i].status = 0;
							windows[num9].labels[i].iconID = global::Util.Util.maxUnsignedShortValue;
						}
					}
					break;
				case 9:
					if (array4.Length <= 1)
					{
						break;
					}
					num2 = 0;
					windows[num9].numButtons = byte.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
					if (windows[num9].numButtons > 0)
					{
						windows[num9].buttons = new StructsClass.UI_Window_Component_Button[windows[num9].numButtons];
						for (int i = 0; i < windows[num9].numButtons; i++)
						{
							windows[num9].buttons[i].status = 0;
							windows[num9].buttons[i].iconID = global::Util.Util.maxUnsignedShortValue;
						}
					}
					break;
				case 10:
					if (array4.Length > 1)
					{
						windows[num9].startComponent = byte.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 11:
					if (array4.Length > 1)
					{
						num5 = 0;
						windows[num9].numSliders = byte.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
						if (windows[num9].numSliders > 0)
						{
							windows[num9].sliders = new StructsClass.UI_Window_Component_Slider[windows[num9].numSliders];
						}
					}
					break;
				case 12:
					if (array4.Length > 18 && num9 > -1)
					{
						windows[num9].checkBoxes[num4].id = ushort.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
						windows[num9].checkBoxes[num4].status = byte.Parse(array4[2], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
						windows[num9].checkBoxes[num4].value = byte.Parse(array4[3], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
						windows[num9].checkBoxes[num4].x = float.Parse(array4[4], CultureInfo.InvariantCulture.NumberFormat);
						windows[num9].checkBoxes[num4].y = float.Parse(array4[5], CultureInfo.InvariantCulture.NumberFormat);
						windows[num9].checkBoxes[num4].clickGroup = byte.Parse(array4[6], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
						windows[num9].checkBoxes[num4].clickAction = byte.Parse(array4[7], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
						windows[num9].checkBoxes[num4].componentDown = byte.Parse(array4[8], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
						windows[num9].checkBoxes[num4].componentUp = byte.Parse(array4[9], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
						windows[num9].checkBoxes[num4].componentLeft = byte.Parse(array4[10], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
						windows[num9].checkBoxes[num4].componentRight = byte.Parse(array4[11], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
						windows[num9].checkBoxes[num4].icon = array4[12];
						windows[num9].checkBoxes[num4].iconX = float.Parse(array4[13], CultureInfo.InvariantCulture.NumberFormat);
						windows[num9].checkBoxes[num4].iconY = float.Parse(array4[14], CultureInfo.InvariantCulture.NumberFormat);
						windows[num9].checkBoxes[num4].texture1 = array4[15];
						windows[num9].checkBoxes[num4].texture2 = array4[16];
						windows[num9].checkBoxes[num4].soundClick = array4[17];
						windows[num9].checkBoxes[num4].soundFocus = array4[18];
						windows[num9].checkBoxes[num4].inGroup = false;
						num4++;
					}
					break;
				case 13:
					if (array4.Length > 22 && num9 > -1)
					{
						windows[num9].sliders[num5].id = ushort.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
						windows[num9].sliders[num5].status = byte.Parse(array4[2], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
						windows[num9].sliders[num5].value = float.Parse(array4[3], CultureInfo.InvariantCulture.NumberFormat);
						windows[num9].sliders[num5].x = float.Parse(array4[4], CultureInfo.InvariantCulture.NumberFormat);
						windows[num9].sliders[num5].y = float.Parse(array4[5], CultureInfo.InvariantCulture.NumberFormat);
						windows[num9].sliders[num5].componentDown = byte.Parse(array4[6], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
						windows[num9].sliders[num5].componentUp = byte.Parse(array4[7], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
						windows[num9].sliders[num5].componentLeft = byte.Parse(array4[8], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
						windows[num9].sliders[num5].componentRight = byte.Parse(array4[9], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
						windows[num9].sliders[num5].icon = array4[10];
						windows[num9].sliders[num5].iconX = float.Parse(array4[11], CultureInfo.InvariantCulture.NumberFormat);
						windows[num9].sliders[num5].iconY = float.Parse(array4[12], CultureInfo.InvariantCulture.NumberFormat);
						windows[num9].sliders[num5].texture1 = array4[13];
						windows[num9].sliders[num5].texture2 = array4[14];
						windows[num9].sliders[num5].soundChange = array4[15];
						windows[num9].sliders[num5].soundFocus = array4[16];
						windows[num9].sliders[num5].minX = float.Parse(array4[17], CultureInfo.InvariantCulture.NumberFormat);
						windows[num9].sliders[num5].maxX = float.Parse(array4[18], CultureInfo.InvariantCulture.NumberFormat);
						windows[num9].sliders[num5].height = ushort.Parse(array4[19], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
						windows[num9].sliders[num5].movementSpeed = float.Parse(array4[20], CultureInfo.InvariantCulture.NumberFormat);
						windows[num9].sliders[num5].clickGroup = byte.Parse(array4[21], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
						windows[num9].sliders[num5].clickAction = byte.Parse(array4[22], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
						windows[num9].sliders[num5].inGroup = false;
						num5++;
					}
					break;
				case 14:
					if (array4.Length <= 1)
					{
						break;
					}
					num6 = 0;
					windows[num9].numGroups = byte.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
					if (windows[num9].numGroups > 0)
					{
						windows[num9].groups = new StructsClass.UI_Window_Component_Group[windows[num9].numGroups];
						for (int i = 0; i < windows[num9].numGroups; i++)
						{
							windows[num9].groups[i].status = 0;
							windows[num9].groups[i].iconID = global::Util.Util.maxUnsignedShortValue;
						}
					}
					break;
				case 15:
					if (array4.Length > 15 && num9 > -1)
					{
						windows[num9].groups[num6].id = ushort.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
						windows[num9].groups[num6].status = byte.Parse(array4[2], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
						windows[num9].groups[num6].type = byte.Parse(array4[3], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
						windows[num9].groups[num6].x = float.Parse(array4[4], CultureInfo.InvariantCulture.NumberFormat);
						windows[num9].groups[num6].y = float.Parse(array4[5], CultureInfo.InvariantCulture.NumberFormat);
						windows[num9].groups[num6].componentDown = byte.Parse(array4[6], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
						windows[num9].groups[num6].componentUp = byte.Parse(array4[7], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
						windows[num9].groups[num6].componentLeft = byte.Parse(array4[8], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
						windows[num9].groups[num6].componentRight = byte.Parse(array4[9], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
						windows[num9].groups[num6].icon = array4[10];
						windows[num9].groups[num6].iconX = float.Parse(array4[11], CultureInfo.InvariantCulture.NumberFormat);
						windows[num9].groups[num6].iconY = float.Parse(array4[12], CultureInfo.InvariantCulture.NumberFormat);
						windows[num9].groups[num6].soundClick = array4[13];
						windows[num9].groups[num6].soundFocus = array4[14];
						windows[num9].groups[num6].numItems = (byte)(array4.Length - 15);
						windows[num9].groups[num6].items = new ushort[windows[num9].groups[num6].numItems];
						int i = 0;
						int num13 = 15;
						while (i < windows[num9].groups[num6].numItems)
						{
							windows[num9].groups[num6].items[i] = ushort.Parse(array4[num13], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
							i++;
							num13++;
						}
						num6++;
					}
					break;
				case 16:
					if (array4.Length > 2 && num9 > -1)
					{
						windows[num9].soundOpen = array4[1];
						windows[num9].soundClose = array4[2];
					}
					break;
				case 17:
					if (array4.Length > 1 && num9 > -1)
					{
						windows[num9].modTexture = array4[1];
					}
					break;
				case 18:
					if (array4.Length > 3 && num9 > -1)
					{
						windows[num9].modScaleX = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
						windows[num9].modScaleY = float.Parse(array4[2], CultureInfo.InvariantCulture.NumberFormat);
						windows[num9].modScaleZ = float.Parse(array4[3], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 19:
					if (array4.Length <= 1)
					{
						break;
					}
					num3 = 0;
					windows[num9].numTextButtons = byte.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
					if (windows[num9].numTextButtons > 0)
					{
						windows[num9].textButtons = new StructsClass.UI_Window_Component_Text_Button[windows[num9].numTextButtons];
						for (int i = 0; i < windows[num9].numTextButtons; i++)
						{
							windows[num9].textButtons[i].status = 0;
							windows[num9].textButtons[i].iconID = global::Util.Util.maxUnsignedShortValue;
						}
					}
					break;
				case 20:
					if (array4.Length > 24 && num9 > -1)
					{
						windows[num9].textButtons[num3].id = ushort.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
						windows[num9].textButtons[num3].fontID = byte.Parse(array4[2], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
						windows[num9].textButtons[num3].centering = byte.Parse(array4[3], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
						windows[num9].textButtons[num3].x = float.Parse(array4[4], CultureInfo.InvariantCulture.NumberFormat);
						windows[num9].textButtons[num3].y = float.Parse(array4[5], CultureInfo.InvariantCulture.NumberFormat);
						windows[num9].textButtons[num3].r = byte.Parse(array4[6], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
						windows[num9].textButtons[num3].g = byte.Parse(array4[7], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
						windows[num9].textButtons[num3].b = byte.Parse(array4[8], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
						windows[num9].textButtons[num3].a = byte.MaxValue;
						windows[num9].textButtons[num3].hr = byte.Parse(array4[9], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
						windows[num9].textButtons[num3].hg = byte.Parse(array4[10], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
						windows[num9].textButtons[num3].hb = byte.Parse(array4[11], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
						windows[num9].textButtons[num3].ha = byte.MaxValue;
						windows[num9].textButtons[num3].type = byte.Parse(array4[12], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
						windows[num9].textButtons[num3].clickGroup = byte.Parse(array4[13], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
						windows[num9].textButtons[num3].clickAction = byte.Parse(array4[14], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
						windows[num9].textButtons[num3].componentDown = byte.Parse(array4[15], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
						windows[num9].textButtons[num3].componentUp = byte.Parse(array4[16], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
						windows[num9].textButtons[num3].componentLeft = byte.Parse(array4[17], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
						windows[num9].textButtons[num3].componentRight = byte.Parse(array4[18], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
						windows[num9].textButtons[num3].icon = array4[19];
						windows[num9].textButtons[num3].iconX = float.Parse(array4[20], CultureInfo.InvariantCulture.NumberFormat);
						windows[num9].textButtons[num3].iconY = float.Parse(array4[21], CultureInfo.InvariantCulture.NumberFormat);
						windows[num9].textButtons[num3].soundClick = array4[22];
						windows[num9].textButtons[num3].soundFocus = array4[23];
						windows[num9].textButtons[num3].buttonText = "";
						windows[num9].textButtons[num3].inGroup = false;
						int num13 = array4.Length;
						windows[num9].textButtons[num3].buttonText = array4[24];
						for (int i = 25; i < num13; i++)
						{
							windows[num9].textButtons[num3].buttonText = windows[num9].textButtons[num3].buttonText + " " + array4[i];
						}
						num3++;
					}
					break;
				case 21:
					if (array4.Length <= 1)
					{
						break;
					}
					num7 = 0;
					windows[num9].numTextAreas = byte.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
					if (windows[num9].numTextAreas > 0)
					{
						windows[num9].textAreas = new StructsClass.UI_Window_Component_Text_Area[windows[num9].numTextAreas];
						for (int i = 0; i < windows[num9].numTextAreas; i++)
						{
							windows[num9].textAreas[i].status = 0;
						}
					}
					break;
				case 22:
					if (array4.Length > 10 && num9 > -1)
					{
						windows[num9].textAreas[num7].id = ushort.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
						windows[num9].textAreas[num7].fontID = byte.Parse(array4[2], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
						windows[num9].textAreas[num7].centering = byte.Parse(array4[3], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
						windows[num9].textAreas[num7].x = float.Parse(array4[4], CultureInfo.InvariantCulture.NumberFormat);
						windows[num9].textAreas[num7].y = float.Parse(array4[5], CultureInfo.InvariantCulture.NumberFormat);
						windows[num9].textAreas[num7].status = byte.Parse(array4[6], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
						windows[num9].textAreas[num7].r = byte.Parse(array4[7], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
						windows[num9].textAreas[num7].g = byte.Parse(array4[8], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
						windows[num9].textAreas[num7].b = byte.Parse(array4[9], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
						windows[num9].textAreas[num7].a = byte.MaxValue;
						windows[num9].textAreas[num7].fontHeight = mainC.fontmoduleMain.Get_Line_Height(windows[num9].textAreas[num7].fontID);
						int num13 = array4.Length;
						text = array4[10];
						for (int i = 11; i < num13; i++)
						{
							text = text + " " + array4[i];
						}
						Set_Text_Area_Text((ushort)num9, num7, text);
						num7++;
					}
					break;
				case 23:
					if (array4.Length > 1)
					{
						windows[num9].type = byte.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 24:
					if (array4.Length > 1)
					{
						windows[num9].buttonFlags = byte.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 25:
					if (array4.Length > 1)
					{
						windows[num9].windowCloseFlags = byte.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 26:
					if (array4.Length > 5 && num9 > -1)
					{
						windows[num9].staticGraphics[num8].id = ushort.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
						windows[num9].staticGraphics[num8].status = byte.Parse(array4[2], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
						windows[num9].staticGraphics[num8].x = float.Parse(array4[3], CultureInfo.InvariantCulture.NumberFormat);
						windows[num9].staticGraphics[num8].y = float.Parse(array4[4], CultureInfo.InvariantCulture.NumberFormat);
						windows[num9].staticGraphics[num8].graphic = array4[5];
						num8++;
					}
					break;
				case 27:
					if (array4.Length <= 1)
					{
						break;
					}
					num8 = 0;
					windows[num9].numStaticGraphics = byte.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
					if (windows[num9].numStaticGraphics > 0)
					{
						windows[num9].staticGraphics = new StructsClass.UI_Window_Component_Static_Graphic[windows[num9].numStaticGraphics];
						for (int i = 0; i < windows[num9].numStaticGraphics; i++)
						{
							windows[num9].staticGraphics[i].status = 0;
							windows[num9].staticGraphics[i].graphicID = global::Util.Util.maxUnsignedShortValue;
						}
					}
					break;
				}
			}
		}
		stream.Close();
		for (int i = 0; i < numWindows; i++)
		{
			windows[i].modTexID = (ushort)mainC.texturesMain.Find_Texture(windows[i].modTexture, 0);
			for (int l = 0; l < windows[i].numLabels; l++)
			{
				windows[i].labels[l].status = 1;
			}
			for (int l = 0; l < windows[i].numStaticGraphics; l++)
			{
				if (string.Compare(windows[i].staticGraphics[l].graphic, ".") != 0)
				{
					windows[i].staticGraphics[l].graphicID = (ushort)mainC.texturesMain.Find_Texture(windows[i].staticGraphics[l].graphic, 0);
				}
				else
				{
					windows[i].staticGraphics[l].graphicID = global::Util.Util.maxUnsignedShortValue;
				}
			}
			for (int l = 0; l < windows[i].numButtons; l++)
			{
				windows[i].buttons[l].status = 1;
				if (string.Compare(windows[i].buttons[l].button1, ".") != 0)
				{
					windows[i].buttons[l].button1ID = (ushort)mainC.texturesMain.Find_Texture(windows[i].buttons[l].button1, 0);
				}
				else
				{
					windows[i].buttons[l].button1ID = global::Util.Util.maxUnsignedShortValue;
				}
				if (string.Compare(windows[i].buttons[l].button2, ".") != 0)
				{
					windows[i].buttons[l].button2ID = (ushort)mainC.texturesMain.Find_Texture(windows[i].buttons[l].button2, 0);
				}
				else
				{
					windows[i].buttons[l].button2ID = global::Util.Util.maxUnsignedShortValue;
				}
				if (string.Compare(windows[i].buttons[l].icon, ".") != 0)
				{
					windows[i].buttons[l].iconID = (ushort)mainC.texturesMain.Find_Texture(windows[i].buttons[l].icon, 0);
				}
				else
				{
					windows[i].buttons[l].iconID = global::Util.Util.maxUnsignedShortValue;
				}
			}
			for (int l = 0; l < windows[i].numTextButtons; l++)
			{
				windows[i].textButtons[l].status = 1;
				if (string.Compare(windows[i].textButtons[l].icon, ".") != 0)
				{
					windows[i].textButtons[l].iconID = (ushort)mainC.texturesMain.Find_Texture(windows[i].textButtons[l].icon, 0);
				}
				else
				{
					windows[i].textButtons[l].iconID = global::Util.Util.maxUnsignedShortValue;
				}
			}
			for (int l = 0; l < windows[i].numCheckBoxes; l++)
			{
				if (string.Compare(windows[i].checkBoxes[l].texture1, ".") != 0)
				{
					windows[i].checkBoxes[l].textureID1 = (ushort)mainC.texturesMain.Find_Texture(windows[i].checkBoxes[l].texture1, 0);
				}
				else
				{
					windows[i].checkBoxes[l].textureID1 = global::Util.Util.maxUnsignedShortValue;
				}
				if (string.Compare(windows[i].checkBoxes[l].texture2, ".") != 0)
				{
					windows[i].checkBoxes[l].textureID2 = (ushort)mainC.texturesMain.Find_Texture(windows[i].checkBoxes[l].texture2, 0);
				}
				else
				{
					windows[i].checkBoxes[l].textureID2 = global::Util.Util.maxUnsignedShortValue;
				}
				if (string.Compare(windows[i].checkBoxes[l].icon, ".") != 0)
				{
					windows[i].checkBoxes[l].iconID = (ushort)mainC.texturesMain.Find_Texture(windows[i].checkBoxes[l].icon, 0);
				}
				else
				{
					windows[i].checkBoxes[l].iconID = global::Util.Util.maxUnsignedShortValue;
				}
			}
			for (int l = 0; l < windows[i].numSliders; l++)
			{
				if (string.Compare(windows[i].sliders[l].texture1, ".") != 0)
				{
					windows[i].sliders[l].textureID1 = (ushort)mainC.texturesMain.Find_Texture(windows[i].sliders[l].texture1, 0);
				}
				else
				{
					windows[i].sliders[l].textureID1 = global::Util.Util.maxUnsignedShortValue;
				}
				if (string.Compare(windows[i].sliders[l].texture2, ".") != 0)
				{
					windows[i].sliders[l].textureID2 = (ushort)mainC.texturesMain.Find_Texture(windows[i].sliders[l].texture2, 0);
				}
				else
				{
					windows[i].sliders[l].textureID2 = global::Util.Util.maxUnsignedShortValue;
				}
				if (string.Compare(windows[i].sliders[l].icon, ".") != 0)
				{
					windows[i].sliders[l].iconID = (ushort)mainC.texturesMain.Find_Texture(windows[i].sliders[l].icon, 0);
				}
				else
				{
					windows[i].sliders[l].iconID = global::Util.Util.maxUnsignedShortValue;
				}
			}
			for (int l = 0; l < windows[i].numGroups; l++)
			{
				windows[i].groups[l].iconID = (ushort)mainC.texturesMain.Find_Texture(windows[i].groups[l].icon, 0);
				if (windows[i].groups[l].type != 0)
				{
					continue;
				}
				for (int num13 = 0; num13 < windows[i].numCheckBoxes; num13++)
				{
					if (windows[i].checkBoxes[num13].id == windows[i].groups[l].items[0])
					{
						windows[i].checkBoxes[num13].inGroup = true;
						break;
					}
				}
				for (int num13 = 0; num13 < windows[i].numSliders; num13++)
				{
					if (windows[i].sliders[num13].id == windows[i].groups[l].items[1])
					{
						windows[i].sliders[num13].inGroup = true;
						break;
					}
				}
			}
			Set_Window_Start_Component((ushort)i);
		}
		windowOrder = new ushort[numWindows];
		nextWindowPlace = 0;
	}

	public void Render_Windows()
	{
		if (nextWindowPlace == 0)
		{
			return;
		}
		mainC.fontmoduleMain.Reset_Graphics_Adapter_After_SpriteBatch();
		global::Rendering.Rendering.effect1.CurrentTechnique = global::Rendering.Rendering.effect1.Techniques["Basic"];
		global::Rendering.Rendering.effect1.Parameters["ViewProjection"].SetValue(global::Rendering.Rendering.matrixO);
		global::Rendering.Rendering.effect1.Parameters["AlphaAdjust"].SetValue(1f);
		global::Rendering.Rendering.rGraphics.DepthStencilState = global::Rendering.Rendering.depthBufferDisabled;
		global::Models.Models.mGraphics.SetVertexBuffer(global::Models.Models.mVertexBufferObjects);
		global::Models.Models.mGraphics.Indices = global::Models.Models.mIndexBufferObjects;
		global::Models.Models.mGraphics.BlendState = global::Rendering.Rendering.blendSourceAlpha;
		for (ushort num = 0; num < nextWindowPlace; num++)
		{
			ushort num2 = windowOrder[num];
			if (windows[num2].status == 1)
			{
				Matrix value = Matrix.CreateScale(windows[num2].modScaleX, windows[num2].modScaleY, windows[num2].modScaleZ) * Matrix.CreateTranslation(windows[num2].x, windows[num2].y, -5000f);
				global::Rendering.Rendering.effect1.Parameters["World"].SetValue(value);
				global::Rendering.Rendering.effect1.Parameters["BaseTexture"].SetValue(global::Textures.Textures.texMain.texData[windows[num2].modTexID]);
				global::Rendering.Rendering.effect1.CurrentTechnique.Passes[0].Apply();
				mainC.modelsMain.Render_Model_Basic(windows[num2].modID);
				try
				{
					ushort numStaticGraphics = windows[num2].numStaticGraphics;
					for (ushort num3 = 0; num3 < numStaticGraphics; num3++)
					{
						if (windows[num2].staticGraphics[num3].status > 0 && windows[num2].staticGraphics[num3].graphicID < global::Textures.Textures.numTextures)
						{
							value = Matrix.CreateScale(global::Textures.Textures.texMain.texData[windows[num2].staticGraphics[num3].graphicID].Width, global::Textures.Textures.texMain.texData[windows[num2].staticGraphics[num3].graphicID].Height, 1f) * Matrix.CreateTranslation(windows[num2].x + windows[num2].staticGraphics[num3].x, windows[num2].y + windows[num2].staticGraphics[num3].y, -5000f);
							global::Rendering.Rendering.effect1.Parameters["World"].SetValue(value);
							global::Rendering.Rendering.effect1.Parameters["BaseTexture"].SetValue(global::Textures.Textures.texMain.texData[windows[num2].staticGraphics[num3].graphicID]);
							global::Rendering.Rendering.effect1.CurrentTechnique.Passes[0].Apply();
							mainC.modelsMain.Render_Model_Basic(global::Models.Models.modFlatPlane);
						}
					}
					numStaticGraphics = windows[num2].numLabels;
					for (ushort num3 = 0; num3 < numStaticGraphics; num3++)
					{
						switch (windows[num2].labels[num3].centering)
						{
						case 0:
							mainC.fontmoduleMain.Draw_Positioned_String(windows[num2].labels[num3].labelText, windows[num2].x + windows[num2].labels[num3].x, windows[num2].y + windows[num2].labels[num3].y, windows[num2].labels[num3].r, windows[num2].labels[num3].g, windows[num2].labels[num3].b, windows[num2].labels[num3].a, windows[num2].labels[num3].fontID);
							break;
						case 1:
							mainC.fontmoduleMain.Draw_Positioned_String_Centered_Horizontally(windows[num2].labels[num3].labelText, windows[num2].x + windows[num2].labels[num3].x, windows[num2].y + windows[num2].labels[num3].y, windows[num2].labels[num3].r, windows[num2].labels[num3].g, windows[num2].labels[num3].b, windows[num2].labels[num3].a, windows[num2].labels[num3].fontID);
							break;
						case 2:
							mainC.fontmoduleMain.Draw_Positioned_String(windows[num2].labels[num3].labelText, windows[num2].x + windows[num2].labels[num3].x, windows[num2].y + windows[num2].labels[num3].y, windows[num2].labels[num3].r, windows[num2].labels[num3].g, windows[num2].labels[num3].b, windows[num2].labels[num3].a, windows[num2].labels[num3].fontID);
							break;
						}
					}
					if (windows[num2].numLabels > 0)
					{
						mainC.fontmoduleMain.Reset_Graphics_Adapter_After_SpriteBatch();
						global::Rendering.Rendering.effect1.CurrentTechnique = global::Rendering.Rendering.effect1.Techniques["Basic"];
						global::Rendering.Rendering.effect1.Parameters["ViewProjection"].SetValue(global::Rendering.Rendering.matrixO);
						global::Rendering.Rendering.effect1.Parameters["AlphaAdjust"].SetValue(1f);
						global::Rendering.Rendering.rGraphics.DepthStencilState = global::Rendering.Rendering.depthBufferDisabled;
						global::Models.Models.mGraphics.SetVertexBuffer(global::Models.Models.mVertexBufferObjects);
						global::Models.Models.mGraphics.Indices = global::Models.Models.mIndexBufferObjects;
						global::Models.Models.mGraphics.BlendState = global::Rendering.Rendering.blendSourceAlpha;
					}
					numStaticGraphics = windows[num2].numTextAreas;
					for (ushort num3 = 0; num3 < numStaticGraphics; num3++)
					{
						if (windows[num2].textAreas[num3].status == 1)
						{
							ushort numLines = windows[num2].textAreas[num3].numLines;
							float fontHeight = windows[num2].textAreas[num3].fontHeight;
							float num4 = windows[num2].y + windows[num2].textAreas[num3].y;
							switch (windows[num2].textAreas[num3].centering)
							{
							case 0:
							{
								for (ushort num5 = 0; num5 < numLines; num5++)
								{
									mainC.fontmoduleMain.Draw_Positioned_String(windows[num2].textAreas[num3].lines[num5], windows[num2].x + windows[num2].textAreas[num3].x, num4, windows[num2].textAreas[num3].r, windows[num2].textAreas[num3].g, windows[num2].textAreas[num3].b, windows[num2].textAreas[num3].a, windows[num2].textAreas[num3].fontID);
									num4 -= fontHeight;
								}
								break;
							}
							case 1:
							{
								for (ushort num5 = 0; num5 < numLines; num5++)
								{
									mainC.fontmoduleMain.Draw_Positioned_String_Centered_Horizontally(windows[num2].textAreas[num3].lines[num5], windows[num2].x + windows[num2].textAreas[num3].x, num4, windows[num2].textAreas[num3].r, windows[num2].textAreas[num3].g, windows[num2].textAreas[num3].b, windows[num2].textAreas[num3].a, windows[num2].textAreas[num3].fontID);
									num4 -= fontHeight;
								}
								break;
							}
							case 2:
							{
								for (ushort num5 = 0; num5 < numLines; num5++)
								{
									mainC.fontmoduleMain.Draw_String_RightJustified(windows[num2].textAreas[num3].lines[num5], windows[num2].x + windows[num2].textAreas[num3].x, num4, windows[num2].textAreas[num3].r, windows[num2].textAreas[num3].g, windows[num2].textAreas[num3].b, windows[num2].textAreas[num3].a, windows[num2].textAreas[num3].fontID);
									num4 -= fontHeight;
								}
								break;
							}
							case 3:
							{
								for (ushort num5 = 0; num5 < numLines; num5++)
								{
									mainC.fontmoduleMain.Draw_Positioned_String_Centered(windows[num2].textAreas[num3].lines[num5], windows[num2].x + windows[num2].textAreas[num3].x, num4, windows[num2].textAreas[num3].r, windows[num2].textAreas[num3].g, windows[num2].textAreas[num3].b, windows[num2].textAreas[num3].a, windows[num2].textAreas[num3].fontID);
									num4 -= fontHeight;
								}
								break;
							}
							}
						}
					}
					if (windows[num2].numTextAreas > 0)
					{
						mainC.fontmoduleMain.Reset_Graphics_Adapter_After_SpriteBatch();
						global::Rendering.Rendering.effect1.CurrentTechnique = global::Rendering.Rendering.effect1.Techniques["Basic"];
						global::Rendering.Rendering.effect1.Parameters["ViewProjection"].SetValue(global::Rendering.Rendering.matrixO);
						global::Rendering.Rendering.effect1.Parameters["AlphaAdjust"].SetValue(1f);
						global::Rendering.Rendering.rGraphics.DepthStencilState = global::Rendering.Rendering.depthBufferDisabled;
						global::Models.Models.mGraphics.SetVertexBuffer(global::Models.Models.mVertexBufferObjects);
						global::Models.Models.mGraphics.Indices = global::Models.Models.mIndexBufferObjects;
						global::Models.Models.mGraphics.BlendState = global::Rendering.Rendering.blendSourceAlpha;
					}
					numStaticGraphics = windows[num2].numButtons;
					for (ushort num3 = 0; num3 < numStaticGraphics; num3++)
					{
						switch (windows[num2].buttons[num3].status)
						{
						case 1:
							if (windows[num2].buttons[num3].button1ID < global::Textures.Textures.numTextures)
							{
								value = Matrix.CreateScale(global::Textures.Textures.texMain.texData[windows[num2].buttons[num3].button1ID].Width, global::Textures.Textures.texMain.texData[windows[num2].buttons[num3].button1ID].Height, 1f) * Matrix.CreateTranslation(windows[num2].x + windows[num2].buttons[num3].x, windows[num2].y + windows[num2].buttons[num3].y, -5000f);
								global::Rendering.Rendering.effect1.Parameters["World"].SetValue(value);
								global::Rendering.Rendering.effect1.Parameters["BaseTexture"].SetValue(global::Textures.Textures.texMain.texData[windows[num2].buttons[num3].button1ID]);
								global::Rendering.Rendering.effect1.CurrentTechnique.Passes[0].Apply();
								mainC.modelsMain.Render_Model_Basic(global::Models.Models.modFlatPlane);
							}
							break;
						case 2:
							if (windows[num2].buttons[num3].button2ID < global::Textures.Textures.numTextures)
							{
								value = Matrix.CreateScale(global::Textures.Textures.texMain.texData[windows[num2].buttons[num3].button2ID].Width, global::Textures.Textures.texMain.texData[windows[num2].buttons[num3].button2ID].Height, 1f) * Matrix.CreateTranslation(windows[num2].x + windows[num2].buttons[num3].x, windows[num2].y + windows[num2].buttons[num3].y, -5000f);
								global::Rendering.Rendering.effect1.Parameters["World"].SetValue(value);
								global::Rendering.Rendering.effect1.Parameters["BaseTexture"].SetValue(global::Textures.Textures.texMain.texData[windows[num2].buttons[num3].button2ID]);
								global::Rendering.Rendering.effect1.CurrentTechnique.Passes[0].Apply();
								mainC.modelsMain.Render_Model_Basic(global::Models.Models.modFlatPlane);
							}
							if (windows[num2].buttons[num3].iconID < global::Textures.Textures.numTextures)
							{
								value = Matrix.CreateScale(global::Textures.Textures.texMain.texData[windows[num2].buttons[num3].iconID].Width, global::Textures.Textures.texMain.texData[windows[num2].buttons[num3].iconID].Height, 1f) * Matrix.CreateTranslation(windows[num2].x + windows[num2].buttons[num3].iconX, windows[num2].y + windows[num2].buttons[num3].iconY, -5000f);
								global::Rendering.Rendering.effect1.Parameters["World"].SetValue(value);
								global::Rendering.Rendering.effect1.Parameters["BaseTexture"].SetValue(global::Textures.Textures.texMain.texData[windows[num2].buttons[num3].iconID]);
								global::Rendering.Rendering.effect1.CurrentTechnique.Passes[0].Apply();
								mainC.modelsMain.Render_Model_Basic(global::Models.Models.modFlatPlane);
							}
							break;
						}
					}
					numStaticGraphics = windows[num2].numTextButtons;
					for (ushort num3 = 0; num3 < numStaticGraphics; num3++)
					{
						switch (windows[num2].textButtons[num3].status)
						{
						case 1:
							switch (windows[num2].textButtons[num3].centering)
							{
							case 0:
								mainC.fontmoduleMain.Draw_Positioned_String(windows[num2].textButtons[num3].buttonText, windows[num2].x + windows[num2].textButtons[num3].x, windows[num2].y + windows[num2].textButtons[num3].y, windows[num2].textButtons[num3].r, windows[num2].textButtons[num3].g, windows[num2].textButtons[num3].b, windows[num2].textButtons[num3].a, windows[num2].textButtons[num3].fontID);
								break;
							case 1:
								mainC.fontmoduleMain.Draw_Positioned_String_Centered_Horizontally(windows[num2].textButtons[num3].buttonText, windows[num2].x + windows[num2].textButtons[num3].x, windows[num2].y + windows[num2].textButtons[num3].y, windows[num2].textButtons[num3].r, windows[num2].textButtons[num3].g, windows[num2].textButtons[num3].b, windows[num2].textButtons[num3].a, windows[num2].textButtons[num3].fontID);
								break;
							case 2:
								mainC.fontmoduleMain.Draw_Positioned_String(windows[num2].textButtons[num3].buttonText, windows[num2].x + windows[num2].textButtons[num3].x, windows[num2].y + windows[num2].textButtons[num3].y, windows[num2].textButtons[num3].r, windows[num2].textButtons[num3].g, windows[num2].textButtons[num3].b, windows[num2].textButtons[num3].a, windows[num2].textButtons[num3].fontID);
								break;
							case 3:
								mainC.fontmoduleMain.Draw_Positioned_String_Centered(windows[num2].textButtons[num3].buttonText, windows[num2].x + windows[num2].textButtons[num3].x, windows[num2].y + windows[num2].textButtons[num3].y, windows[num2].textButtons[num3].r, windows[num2].textButtons[num3].g, windows[num2].textButtons[num3].b, windows[num2].textButtons[num3].a, windows[num2].textButtons[num3].fontID);
								break;
							}
							break;
						case 2:
							if (windows[num2].textButtons[num3].iconID < global::Textures.Textures.numTextures)
							{
								mainC.fontmoduleMain.Reset_Graphics_Adapter_After_SpriteBatch();
								global::Rendering.Rendering.effect1.CurrentTechnique = global::Rendering.Rendering.effect1.Techniques["Basic"];
								global::Rendering.Rendering.effect1.Parameters["ViewProjection"].SetValue(global::Rendering.Rendering.matrixO);
								global::Rendering.Rendering.effect1.Parameters["AlphaAdjust"].SetValue(1f);
								global::Rendering.Rendering.rGraphics.DepthStencilState = global::Rendering.Rendering.depthBufferDisabled;
								global::Models.Models.mGraphics.SetVertexBuffer(global::Models.Models.mVertexBufferObjects);
								global::Models.Models.mGraphics.Indices = global::Models.Models.mIndexBufferObjects;
								global::Models.Models.mGraphics.BlendState = global::Rendering.Rendering.blendSourceAlpha;
								value = Matrix.CreateScale(global::Textures.Textures.texMain.texData[windows[num2].textButtons[num3].iconID].Width, global::Textures.Textures.texMain.texData[windows[num2].textButtons[num3].iconID].Height, 1f) * Matrix.CreateTranslation(windows[num2].x + windows[num2].textButtons[num3].iconX, windows[num2].y + windows[num2].textButtons[num3].iconY, -5000f);
								global::Rendering.Rendering.effect1.Parameters["World"].SetValue(value);
								global::Rendering.Rendering.effect1.Parameters["BaseTexture"].SetValue(global::Textures.Textures.texMain.texData[windows[num2].textButtons[num3].iconID]);
								global::Rendering.Rendering.effect1.CurrentTechnique.Passes[0].Apply();
								mainC.modelsMain.Render_Model_Basic(global::Models.Models.modFlatPlane);
							}
							switch (windows[num2].textButtons[num3].centering)
							{
							case 0:
								mainC.fontmoduleMain.Draw_Positioned_String(windows[num2].textButtons[num3].buttonText, windows[num2].x + windows[num2].textButtons[num3].x, windows[num2].y + windows[num2].textButtons[num3].y, windows[num2].textButtons[num3].hr, windows[num2].textButtons[num3].hg, windows[num2].textButtons[num3].hb, windows[num2].textButtons[num3].ha, windows[num2].textButtons[num3].fontID);
								break;
							case 1:
								mainC.fontmoduleMain.Draw_Positioned_String_Centered_Horizontally(windows[num2].textButtons[num3].buttonText, windows[num2].x + windows[num2].textButtons[num3].x, windows[num2].y + windows[num2].textButtons[num3].y, windows[num2].textButtons[num3].hr, windows[num2].textButtons[num3].hg, windows[num2].textButtons[num3].hb, windows[num2].textButtons[num3].ha, windows[num2].textButtons[num3].fontID);
								break;
							case 2:
								mainC.fontmoduleMain.Draw_Positioned_String(windows[num2].textButtons[num3].buttonText, windows[num2].x + windows[num2].textButtons[num3].x, windows[num2].y + windows[num2].textButtons[num3].y, windows[num2].textButtons[num3].hr, windows[num2].textButtons[num3].hg, windows[num2].textButtons[num3].hb, windows[num2].textButtons[num3].ha, windows[num2].textButtons[num3].fontID);
								break;
							case 3:
								mainC.fontmoduleMain.Draw_Positioned_String_Centered(windows[num2].textButtons[num3].buttonText, windows[num2].x + windows[num2].textButtons[num3].x, windows[num2].y + windows[num2].textButtons[num3].y, windows[num2].textButtons[num3].hr, windows[num2].textButtons[num3].hg, windows[num2].textButtons[num3].hb, windows[num2].textButtons[num3].ha, windows[num2].textButtons[num3].fontID);
								break;
							}
							break;
						}
					}
					if (windows[num2].numTextButtons > 0)
					{
						mainC.fontmoduleMain.Reset_Graphics_Adapter_After_SpriteBatch();
						global::Rendering.Rendering.effect1.CurrentTechnique = global::Rendering.Rendering.effect1.Techniques["Basic"];
						global::Rendering.Rendering.effect1.Parameters["ViewProjection"].SetValue(global::Rendering.Rendering.matrixO);
						global::Rendering.Rendering.effect1.Parameters["AlphaAdjust"].SetValue(1f);
						global::Rendering.Rendering.rGraphics.DepthStencilState = global::Rendering.Rendering.depthBufferDisabled;
						global::Models.Models.mGraphics.SetVertexBuffer(global::Models.Models.mVertexBufferObjects);
						global::Models.Models.mGraphics.Indices = global::Models.Models.mIndexBufferObjects;
						global::Models.Models.mGraphics.BlendState = global::Rendering.Rendering.blendSourceAlpha;
					}
					numStaticGraphics = windows[num2].numCheckBoxes;
					for (ushort num3 = 0; num3 < numStaticGraphics; num3++)
					{
						if (windows[num2].checkBoxes[num3].status > 0)
						{
							if (windows[num2].checkBoxes[num3].status == 2 && windows[num2].checkBoxes[num3].iconID < global::Textures.Textures.numTextures)
							{
								value = Matrix.CreateScale(global::Textures.Textures.texMain.texData[windows[num2].checkBoxes[num3].iconID].Width, global::Textures.Textures.texMain.texData[windows[num2].checkBoxes[num3].iconID].Height, 1f) * Matrix.CreateTranslation(windows[num2].x + windows[num2].checkBoxes[num3].iconX, windows[num2].y + windows[num2].checkBoxes[num3].iconY, -5000f);
								global::Rendering.Rendering.effect1.Parameters["World"].SetValue(value);
								global::Rendering.Rendering.effect1.Parameters["BaseTexture"].SetValue(global::Textures.Textures.texMain.texData[windows[num2].checkBoxes[num3].iconID]);
								global::Rendering.Rendering.effect1.CurrentTechnique.Passes[0].Apply();
								mainC.modelsMain.Render_Model_Basic(global::Models.Models.modFlatPlane);
							}
							if (windows[num2].checkBoxes[num3].value == 1)
							{
								if (windows[num2].checkBoxes[num3].textureID1 < global::Textures.Textures.numTextures)
								{
									value = Matrix.CreateScale(global::Textures.Textures.texMain.texData[windows[num2].checkBoxes[num3].textureID1].Width, global::Textures.Textures.texMain.texData[windows[num2].checkBoxes[num3].textureID1].Height, 1f) * Matrix.CreateTranslation(windows[num2].x + windows[num2].checkBoxes[num3].x, windows[num2].y + windows[num2].checkBoxes[num3].y, -5000f);
									global::Rendering.Rendering.effect1.Parameters["World"].SetValue(value);
									global::Rendering.Rendering.effect1.Parameters["BaseTexture"].SetValue(global::Textures.Textures.texMain.texData[windows[num2].checkBoxes[num3].textureID1]);
									global::Rendering.Rendering.effect1.CurrentTechnique.Passes[0].Apply();
									mainC.modelsMain.Render_Model_Basic(global::Models.Models.modFlatPlane);
								}
							}
							else if (windows[num2].checkBoxes[num3].value == 0 && windows[num2].checkBoxes[num3].textureID2 < global::Textures.Textures.numTextures)
							{
								value = Matrix.CreateScale(global::Textures.Textures.texMain.texData[windows[num2].checkBoxes[num3].textureID2].Width, global::Textures.Textures.texMain.texData[windows[num2].checkBoxes[num3].textureID2].Height, 1f) * Matrix.CreateTranslation(windows[num2].x + windows[num2].checkBoxes[num3].x, windows[num2].y + windows[num2].checkBoxes[num3].y, -5000f);
								global::Rendering.Rendering.effect1.Parameters["World"].SetValue(value);
								global::Rendering.Rendering.effect1.Parameters["BaseTexture"].SetValue(global::Textures.Textures.texMain.texData[windows[num2].checkBoxes[num3].textureID2]);
								global::Rendering.Rendering.effect1.CurrentTechnique.Passes[0].Apply();
								mainC.modelsMain.Render_Model_Basic(global::Models.Models.modFlatPlane);
							}
						}
					}
					numStaticGraphics = windows[num2].numSliders;
					for (ushort num3 = 0; num3 < numStaticGraphics; num3++)
					{
						if (windows[num2].sliders[num3].status > 0)
						{
							if (windows[num2].sliders[num3].status == 2 && windows[num2].sliders[num3].iconID < global::Textures.Textures.numTextures)
							{
								value = Matrix.CreateScale(global::Textures.Textures.texMain.texData[windows[num2].sliders[num3].iconID].Width, global::Textures.Textures.texMain.texData[windows[num2].sliders[num3].iconID].Height, 1f) * Matrix.CreateTranslation(windows[num2].x + windows[num2].sliders[num3].iconX, windows[num2].y + windows[num2].sliders[num3].iconY, -5000f);
								global::Rendering.Rendering.effect1.Parameters["World"].SetValue(value);
								global::Rendering.Rendering.effect1.Parameters["BaseTexture"].SetValue(global::Textures.Textures.texMain.texData[windows[num2].checkBoxes[num3].iconID]);
								global::Rendering.Rendering.effect1.CurrentTechnique.Passes[0].Apply();
								mainC.modelsMain.Render_Model_Basic(global::Models.Models.modFlatPlane);
							}
							float num4 = (float)Math.Ceiling(windows[num2].sliders[num3].maxX * windows[num2].sliders[num3].value);
							if (num4 < windows[num2].sliders[num3].minX)
							{
								num4 = windows[num2].sliders[num3].minX;
							}
							if (windows[num2].sliders[num3].textureID1 < global::Textures.Textures.numTextures)
							{
								num4 += 10f;
								value = Matrix.CreateScale(num4, (int)windows[num2].sliders[num3].height, 1f) * Matrix.CreateTranslation(windows[num2].x + windows[num2].sliders[num3].x + num4 / 2f, windows[num2].y + windows[num2].sliders[num3].y, -5000f);
								global::Rendering.Rendering.effect1.Parameters["World"].SetValue(value);
								global::Rendering.Rendering.effect1.Parameters["BaseTexture"].SetValue(global::Textures.Textures.texMain.texData[windows[num2].sliders[num3].textureID1]);
								global::Rendering.Rendering.effect1.CurrentTechnique.Passes[0].Apply();
								mainC.modelsMain.Render_Model_Basic(global::Models.Models.modFlatPlane);
							}
							if (windows[num2].sliders[num3].textureID2 < global::Textures.Textures.numTextures)
							{
								value = Matrix.CreateScale(global::Textures.Textures.texMain.texData[windows[num2].sliders[num3].textureID2].Width, (int)windows[num2].sliders[num3].height, 1f) * Matrix.CreateTranslation(windows[num2].x + windows[num2].sliders[num3].x + num4 + (float)global::Textures.Textures.texMain.texData[windows[num2].sliders[num3].textureID2].Width / 2f, windows[num2].y + windows[num2].sliders[num3].y, -5000f);
								global::Rendering.Rendering.effect1.Parameters["World"].SetValue(value);
								global::Rendering.Rendering.effect1.Parameters["BaseTexture"].SetValue(global::Textures.Textures.texMain.texData[windows[num2].sliders[num3].textureID2]);
								global::Rendering.Rendering.effect1.CurrentTechnique.Passes[0].Apply();
								mainC.modelsMain.Render_Model_Basic(global::Models.Models.modFlatPlane);
							}
						}
					}
					numStaticGraphics = windows[num2].numGroups;
					for (ushort num3 = 0; num3 < numStaticGraphics; num3++)
					{
						if (windows[num2].groups[num3].status > 0 && windows[num2].groups[num3].status == 2 && windows[num2].groups[num3].iconID < global::Textures.Textures.numTextures)
						{
							value = Matrix.CreateScale(global::Textures.Textures.texMain.texData[windows[num2].groups[num3].iconID].Width, global::Textures.Textures.texMain.texData[windows[num2].groups[num3].iconID].Height, 1f) * Matrix.CreateTranslation(windows[num2].x + windows[num2].groups[num3].iconX, windows[num2].y + windows[num2].groups[num3].iconY, -5000f);
							global::Rendering.Rendering.effect1.Parameters["World"].SetValue(value);
							global::Rendering.Rendering.effect1.Parameters["BaseTexture"].SetValue(global::Textures.Textures.texMain.texData[windows[num2].checkBoxes[num3].iconID]);
							global::Rendering.Rendering.effect1.CurrentTechnique.Passes[0].Apply();
							mainC.modelsMain.Render_Model_Basic(global::Models.Models.modFlatPlane);
						}
					}
				}
				catch
				{
				}
				mainC.gameLogic.Game_Render_Additional_Window_Objects(num2);
			}
		}
		global::Rendering.Rendering.effect1.Parameters["ViewProjection"].SetValue(global::Rendering.Rendering.matrixVP);
	}

	public void Process_Window_Input(float frameTime)
	{
		ushort num = 0;
		if (numWindows < 1)
		{
			return;
		}
		if (windows[curWindow].status == 1)
		{
			if (!Guide.IsVisible)
			{
				if (!windows[curWindow].ignoreStickInputs)
				{
					float num2 = 0f;
					if (Math.Abs(global::InputHandler.InputHandler.controllerStickLeftValueX) > 0.25f)
					{
						num2 = global::InputHandler.InputHandler.controllerStickLeftValueX;
					}
					else if (Math.Abs(global::InputHandler.InputHandler.controllerStickRightValueX) > 0.25f)
					{
						num2 = global::InputHandler.InputHandler.controllerStickRightValueX;
					}
					if (num2 > 0.25f)
					{
						float num3 = MainGame.frametime * num2;
						if (global::InputHandler.InputHandler.controllerStickLeftRepeatX == 0f || (global::InputHandler.InputHandler.controllerStickLeftRepeatX += num3) > 0.13f)
						{
							global::InputHandler.InputHandler.controllerDPadRightPressed = true;
							global::InputHandler.InputHandler.controllerStickLeftRepeatX = -0.2f + thumbStickRepeatValueX;
							thumbStickRepeatValueX = 0.25f;
						}
						global::InputHandler.InputHandler.controllerStickLeftValueX = 1f;
						global::InputHandler.InputHandler.controllerStickRightValueX = 1f;
					}
					else if (num2 < -0.25f)
					{
						float num3 = MainGame.frametime * (0f - num2);
						if (global::InputHandler.InputHandler.controllerStickLeftRepeatX == 0f || (global::InputHandler.InputHandler.controllerStickLeftRepeatX += num3) > 0.13f)
						{
							global::InputHandler.InputHandler.controllerDPadLeftPressed = true;
							global::InputHandler.InputHandler.controllerStickLeftRepeatX = -0.2f + thumbStickRepeatValueX;
							thumbStickRepeatValueX = 0.25f;
						}
						global::InputHandler.InputHandler.controllerStickLeftValueX = -1f;
						global::InputHandler.InputHandler.controllerStickRightValueX = -1f;
					}
					else
					{
						global::InputHandler.InputHandler.controllerStickLeftValueX = 0f;
						global::InputHandler.InputHandler.controllerStickRightValueX = 0f;
						global::InputHandler.InputHandler.controllerStickLeftRepeatX = 0f;
						thumbStickRepeatValueX = 0f;
						num2 = 0f;
						if (Math.Abs(global::InputHandler.InputHandler.controllerStickLeftValueY) > 0.25f)
						{
							num2 = global::InputHandler.InputHandler.controllerStickLeftValueY;
						}
						else if (Math.Abs(global::InputHandler.InputHandler.controllerStickRightValueY) > 0.25f)
						{
							num2 = global::InputHandler.InputHandler.controllerStickRightValueY;
						}
						if (num2 > 0.25f)
						{
							float num3 = MainGame.frametime * num2;
							if (global::InputHandler.InputHandler.controllerStickLeftRepeatY == 0f || (global::InputHandler.InputHandler.controllerStickLeftRepeatY += num3) > 0.13f)
							{
								global::InputHandler.InputHandler.controllerDPadUpPressed = true;
								global::InputHandler.InputHandler.controllerStickLeftRepeatY = -0.2f + thumbStickRepeatValueY;
								thumbStickRepeatValueY = 0.25f;
							}
							global::InputHandler.InputHandler.controllerStickLeftValueY = 1f;
						}
						else if (num2 < -0.25f)
						{
							float num3 = MainGame.frametime * (0f - num2);
							if (global::InputHandler.InputHandler.controllerStickLeftRepeatY == 0f || (global::InputHandler.InputHandler.controllerStickLeftRepeatY += num3) > 0.13f)
							{
								global::InputHandler.InputHandler.controllerDPadDownPressed = true;
								global::InputHandler.InputHandler.controllerStickLeftRepeatY = -0.2f + thumbStickRepeatValueY;
								thumbStickRepeatValueY = 0.25f;
							}
							global::InputHandler.InputHandler.controllerStickLeftValueY = -1f;
						}
						else
						{
							global::InputHandler.InputHandler.controllerStickLeftValueY = 0f;
							global::InputHandler.InputHandler.controllerStickRightValueY = 0f;
							global::InputHandler.InputHandler.controllerStickLeftRepeatY = 0f;
							thumbStickRepeatValueY = 0f;
						}
					}
				}
				if (windows[curWindow].numButtons > 0)
				{
					if (windows[curWindow].buttonFlags != 0)
					{
						if (global::InputHandler.InputHandler.controllerButtonAPressed && (windows[curWindow].buttonFlags & 1) > 0)
						{
							for (num = 0; num < windows[curWindow].numButtons; num++)
							{
								if (windows[curWindow].buttons[num].clickAction == 1)
								{
									Process_Window_Action(curWindow, windows[curWindow].buttons[num].clickGroup, 1, windows[curWindow].buttons[num].id, 1f);
									global::InputHandler.InputHandler.controllerButtonAPressed = false;
									mainC.soundsMain.Play_Sound_NonPositional(windows[curWindow].buttons[num].soundClick);
									break;
								}
							}
						}
						else if (global::InputHandler.InputHandler.controllerButtonBPressed && (windows[curWindow].buttonFlags & 2) > 0)
						{
							for (num = 0; num < windows[curWindow].numButtons; num++)
							{
								if (windows[curWindow].buttons[num].clickAction == 2)
								{
									Process_Window_Action(curWindow, windows[curWindow].buttons[num].clickGroup, 2, windows[curWindow].buttons[num].id, 1f);
									global::InputHandler.InputHandler.controllerButtonBPressed = false;
									mainC.soundsMain.Play_Sound_NonPositional(windows[curWindow].buttons[num].soundClick);
									break;
								}
							}
						}
						else if (global::InputHandler.InputHandler.controllerButtonXPressed && (windows[curWindow].buttonFlags & 4) > 0)
						{
							for (num = 0; num < windows[curWindow].numButtons; num++)
							{
								if (windows[curWindow].buttons[num].clickAction == 4)
								{
									Process_Window_Action(curWindow, windows[curWindow].buttons[num].clickGroup, 4, windows[curWindow].buttons[num].id, 1f);
									global::InputHandler.InputHandler.controllerButtonXPressed = false;
									mainC.soundsMain.Play_Sound_NonPositional(windows[curWindow].buttons[num].soundClick);
									break;
								}
							}
						}
						else if (global::InputHandler.InputHandler.controllerButtonYPressed && (windows[curWindow].buttonFlags & 8) > 0)
						{
							for (num = 0; num < windows[curWindow].numButtons; num++)
							{
								if (windows[curWindow].buttons[num].clickAction == 8)
								{
									Process_Window_Action(curWindow, windows[curWindow].buttons[num].clickGroup, 8, windows[curWindow].buttons[num].id, 1f);
									global::InputHandler.InputHandler.controllerButtonYPressed = false;
									mainC.soundsMain.Play_Sound_NonPositional(windows[curWindow].buttons[num].soundClick);
									break;
								}
							}
						}
						else if (global::InputHandler.InputHandler.controllerButtonStartPressed && (windows[curWindow].buttonFlags & 0x10) > 0)
						{
							for (num = 0; num < windows[curWindow].numButtons; num++)
							{
								if (windows[curWindow].buttons[num].clickAction == 16)
								{
									Process_Window_Action(curWindow, windows[curWindow].buttons[num].clickGroup, 16, windows[curWindow].buttons[num].id, 1f);
									global::InputHandler.InputHandler.controllerButtonStartPressed = false;
									mainC.soundsMain.Play_Sound_NonPositional(windows[curWindow].buttons[num].soundClick);
									break;
								}
							}
						}
						else if (global::InputHandler.InputHandler.controllerButtonBackPressed && (windows[curWindow].buttonFlags & 0x20) > 0)
						{
							for (num = 0; num < windows[curWindow].numButtons; num++)
							{
								if (windows[curWindow].buttons[num].clickAction == 32)
								{
									Process_Window_Action(curWindow, windows[curWindow].buttons[num].clickGroup, 32, windows[curWindow].buttons[num].id, 1f);
									global::InputHandler.InputHandler.controllerButtonBackPressed = false;
									mainC.soundsMain.Play_Sound_NonPositional(windows[curWindow].buttons[num].soundClick);
									break;
								}
							}
						}
					}
					else
					{
						ushort curButton = windows[curWindow].curButton;
						if (curButton < windows[curWindow].numButtons && windows[curWindow].buttons[curButton].status == 2)
						{
							int num4 = -1;
							if (global::InputHandler.InputHandler.controllerButtonAPressed)
							{
								Process_Window_Action(curWindow, windows[curWindow].buttons[curButton].clickGroup, windows[curWindow].buttons[curButton].clickAction, windows[curWindow].buttons[curButton].id, 1f);
								global::InputHandler.InputHandler.controllerButtonAPressed = false;
								mainC.soundsMain.Play_Sound_NonPositional(windows[curWindow].buttons[curButton].soundClick);
							}
							else if (global::InputHandler.InputHandler.controllerDPadUpPressed)
							{
								num4 = 0;
								num = Get_Window_Object(curWindow, windows[curWindow].buttons[curButton].componentUp);
								global::InputHandler.InputHandler.controllerDPadUpPressed = false;
							}
							else if (global::InputHandler.InputHandler.controllerDPadDownPressed)
							{
								num4 = 1;
								num = Get_Window_Object(curWindow, windows[curWindow].buttons[curButton].componentDown);
								global::InputHandler.InputHandler.controllerDPadDownPressed = false;
							}
							else if (global::InputHandler.InputHandler.controllerDPadLeftPressed)
							{
								num4 = 2;
								num = Get_Window_Object(curWindow, windows[curWindow].buttons[curButton].componentLeft);
								global::InputHandler.InputHandler.controllerDPadLeftPressed = false;
							}
							else if (global::InputHandler.InputHandler.controllerDPadRightPressed)
							{
								num4 = 3;
								num = Get_Window_Object(curWindow, windows[curWindow].buttons[curButton].componentRight);
								global::InputHandler.InputHandler.controllerDPadRightPressed = false;
							}
							if (num4 > -1 && componentType != 0)
							{
								windows[curWindow].buttons[curButton].status = 1;
								switch (componentType)
								{
								case 1:
									windows[curWindow].buttons[num].status = 2;
									windows[curWindow].curButton = (byte)num;
									if (num != curButton)
									{
										mainC.soundsMain.Play_Sound_NonPositional(windows[curWindow].buttons[num].soundFocus);
									}
									break;
								case 2:
									windows[curWindow].checkBoxes[num].status = 2;
									windows[curWindow].curCheckBox = (byte)num;
									windows[curWindow].curButton = byte.MaxValue;
									mainC.soundsMain.Play_Sound_NonPositional(windows[curWindow].checkBoxes[num].soundFocus);
									break;
								case 4:
									windows[curWindow].labels[num].status = 2;
									windows[curWindow].curLabel = (byte)num;
									windows[curWindow].curButton = byte.MaxValue;
									break;
								case 3:
									windows[curWindow].sliders[num].status = 2;
									windows[curWindow].curSlider = (byte)num;
									windows[curWindow].curButton = byte.MaxValue;
									mainC.soundsMain.Play_Sound_NonPositional(windows[curWindow].sliders[num].soundFocus);
									break;
								case 5:
									windows[curWindow].groups[num].status = 2;
									windows[curWindow].curGroup = (byte)num;
									windows[curWindow].curButton = byte.MaxValue;
									mainC.soundsMain.Play_Sound_NonPositional(windows[curWindow].groups[num].soundFocus);
									break;
								}
							}
						}
					}
				}
				if (windows[curWindow].numTextButtons > 0)
				{
					ushort curButton = windows[curWindow].curTextButton;
					if (curButton < windows[curWindow].numTextButtons && windows[curWindow].textButtons[curButton].status == 2)
					{
						int num4 = -1;
						if (global::InputHandler.InputHandler.controllerButtonAPressed)
						{
							Process_Window_Action(curWindow, windows[curWindow].textButtons[curButton].clickGroup, windows[curWindow].textButtons[curButton].clickAction, windows[curWindow].textButtons[curButton].id, 1f);
							global::InputHandler.InputHandler.controllerButtonAPressed = false;
							mainC.soundsMain.Play_Sound_NonPositional(windows[curWindow].textButtons[curButton].soundClick);
						}
						else if (global::InputHandler.InputHandler.controllerDPadUpPressed)
						{
							num4 = 0;
							num = Get_Window_Object(curWindow, windows[curWindow].textButtons[curButton].componentUp);
							global::InputHandler.InputHandler.controllerDPadUpPressed = false;
						}
						else if (global::InputHandler.InputHandler.controllerDPadDownPressed)
						{
							num4 = 1;
							num = Get_Window_Object(curWindow, windows[curWindow].textButtons[curButton].componentDown);
							global::InputHandler.InputHandler.controllerDPadDownPressed = false;
						}
						else if (global::InputHandler.InputHandler.controllerDPadLeftPressed)
						{
							num4 = 2;
							num = Get_Window_Object(curWindow, windows[curWindow].textButtons[curButton].componentLeft);
							global::InputHandler.InputHandler.controllerDPadLeftPressed = false;
						}
						else if (global::InputHandler.InputHandler.controllerDPadRightPressed)
						{
							num4 = 3;
							num = Get_Window_Object(curWindow, windows[curWindow].textButtons[curButton].componentRight);
							global::InputHandler.InputHandler.controllerDPadRightPressed = false;
						}
						if (num4 > -1 && componentType != 0)
						{
							windows[curWindow].textButtons[curButton].status = 1;
							switch (componentType)
							{
							case 6:
								windows[curWindow].textButtons[num].status = 2;
								windows[curWindow].curTextButton = (byte)num;
								if (num != curButton)
								{
									mainC.soundsMain.Play_Sound_NonPositional(windows[curWindow].textButtons[num].soundFocus);
								}
								break;
							case 1:
								windows[curWindow].buttons[num].status = 2;
								windows[curWindow].curButton = (byte)num;
								windows[curWindow].curTextButton = byte.MaxValue;
								if (num != curButton)
								{
									mainC.soundsMain.Play_Sound_NonPositional(windows[curWindow].buttons[num].soundFocus);
								}
								break;
							case 2:
								windows[curWindow].checkBoxes[num].status = 2;
								windows[curWindow].curCheckBox = (byte)num;
								windows[curWindow].curTextButton = byte.MaxValue;
								mainC.soundsMain.Play_Sound_NonPositional(windows[curWindow].checkBoxes[num].soundFocus);
								break;
							case 4:
								windows[curWindow].labels[num].status = 2;
								windows[curWindow].curLabel = (byte)num;
								windows[curWindow].curTextButton = byte.MaxValue;
								break;
							case 3:
								windows[curWindow].sliders[num].status = 2;
								windows[curWindow].curSlider = (byte)num;
								windows[curWindow].curTextButton = byte.MaxValue;
								mainC.soundsMain.Play_Sound_NonPositional(windows[curWindow].sliders[num].soundFocus);
								break;
							case 5:
								windows[curWindow].groups[num].status = 2;
								windows[curWindow].curGroup = (byte)num;
								windows[curWindow].curTextButton = byte.MaxValue;
								mainC.soundsMain.Play_Sound_NonPositional(windows[curWindow].groups[num].soundFocus);
								break;
							}
						}
					}
				}
				if (windows[curWindow].numCheckBoxes > 0)
				{
					ushort curButton = windows[curWindow].curCheckBox;
					if (curButton < windows[curWindow].numCheckBoxes && windows[curWindow].checkBoxes[curButton].status == 2)
					{
						int num4 = -1;
						if (global::InputHandler.InputHandler.controllerButtonAPressed)
						{
							windows[curWindow].checkBoxes[curButton].value = (byte)(++windows[curWindow].checkBoxes[curButton].value % 2);
							Process_Window_Action(curWindow, windows[curWindow].checkBoxes[curButton].clickGroup, windows[curWindow].checkBoxes[curButton].clickAction, windows[curWindow].checkBoxes[curButton].id, (int)windows[curWindow].checkBoxes[curButton].value);
							global::InputHandler.InputHandler.controllerButtonAPressed = false;
							mainC.soundsMain.Play_Sound_NonPositional(windows[curWindow].checkBoxes[curButton].soundClick);
						}
						else if (global::InputHandler.InputHandler.controllerDPadUpPressed)
						{
							num4 = 0;
							num = Get_Window_Object(curWindow, windows[curWindow].checkBoxes[curButton].componentUp);
							global::InputHandler.InputHandler.controllerDPadUpPressed = false;
						}
						else if (global::InputHandler.InputHandler.controllerDPadDownPressed)
						{
							num4 = 1;
							num = Get_Window_Object(curWindow, windows[curWindow].checkBoxes[curButton].componentDown);
							global::InputHandler.InputHandler.controllerDPadDownPressed = false;
						}
						else if (global::InputHandler.InputHandler.controllerDPadLeftPressed)
						{
							num4 = 2;
							num = Get_Window_Object(curWindow, windows[curWindow].checkBoxes[curButton].componentLeft);
							global::InputHandler.InputHandler.controllerDPadLeftPressed = false;
						}
						else if (global::InputHandler.InputHandler.controllerDPadRightPressed)
						{
							num4 = 3;
							num = Get_Window_Object(curWindow, windows[curWindow].checkBoxes[curButton].componentRight);
							global::InputHandler.InputHandler.controllerDPadRightPressed = false;
						}
						if (num4 > -1 && componentType != 0)
						{
							windows[curWindow].checkBoxes[curButton].status = 1;
							switch (componentType)
							{
							case 1:
								windows[curWindow].buttons[num].status = 2;
								windows[curWindow].curButton = (byte)num;
								windows[curWindow].curCheckBox = byte.MaxValue;
								mainC.soundsMain.Play_Sound_NonPositional(windows[curWindow].buttons[num].soundFocus);
								break;
							case 2:
								windows[curWindow].checkBoxes[num].status = 2;
								windows[curWindow].curCheckBox = (byte)num;
								if (num != curButton)
								{
									mainC.soundsMain.Play_Sound_NonPositional(windows[curWindow].checkBoxes[num].soundFocus);
								}
								break;
							case 4:
								windows[curWindow].labels[num].status = 2;
								windows[curWindow].curLabel = (byte)num;
								windows[curWindow].curCheckBox = byte.MaxValue;
								break;
							case 3:
								windows[curWindow].sliders[num].status = 2;
								windows[curWindow].curSlider = (byte)num;
								windows[curWindow].curCheckBox = byte.MaxValue;
								mainC.soundsMain.Play_Sound_NonPositional(windows[curWindow].sliders[num].soundFocus);
								break;
							case 5:
								windows[curWindow].groups[num].status = 2;
								windows[curWindow].curGroup = (byte)num;
								windows[curWindow].curCheckBox = byte.MaxValue;
								mainC.soundsMain.Play_Sound_NonPositional(windows[curWindow].groups[num].soundFocus);
								break;
							}
						}
					}
				}
				if (windows[curWindow].numSliders > 0)
				{
					ushort curButton = windows[curWindow].curSlider;
					if (curButton < windows[curWindow].numSliders && windows[curWindow].sliders[curButton].status == 2)
					{
						int num4 = -1;
						if (global::InputHandler.InputHandler.controllerDPadUpPressed)
						{
							num4 = 0;
							num = Get_Window_Object(curWindow, windows[curWindow].sliders[curButton].componentUp);
							global::InputHandler.InputHandler.controllerDPadUpPressed = false;
						}
						else if (global::InputHandler.InputHandler.controllerDPadDownPressed)
						{
							num4 = 1;
							num = Get_Window_Object(curWindow, windows[curWindow].sliders[curButton].componentDown);
							global::InputHandler.InputHandler.controllerDPadDownPressed = false;
						}
						else if (global::InputHandler.InputHandler.controllerDPadLeftPressed)
						{
							if (windows[curWindow].sliders[curButton].value > 0f)
							{
								mainC.soundsMain.Play_Sound_NonPositional(windows[curWindow].sliders[curButton].soundChange);
							}
							windows[curWindow].sliders[curButton].value -= windows[curWindow].sliders[curButton].movementSpeed;
							if (windows[curWindow].sliders[curButton].value < 0f)
							{
								windows[curWindow].sliders[curButton].value = 0f;
							}
							Process_Window_Action(curWindow, windows[curWindow].sliders[curButton].clickGroup, windows[curWindow].sliders[curButton].clickAction, windows[curWindow].sliders[curButton].id, windows[curWindow].sliders[curButton].value);
							global::InputHandler.InputHandler.controllerDPadLeftPressed = false;
						}
						else if (global::InputHandler.InputHandler.controllerDPadRightPressed)
						{
							if (windows[curWindow].sliders[curButton].value < 1f)
							{
								mainC.soundsMain.Play_Sound_NonPositional(windows[curWindow].sliders[curButton].soundChange);
							}
							windows[curWindow].sliders[curButton].value += windows[curWindow].sliders[curButton].movementSpeed;
							if (windows[curWindow].sliders[curButton].value > 1f)
							{
								windows[curWindow].sliders[curButton].value = 1f;
							}
							Process_Window_Action(curWindow, windows[curWindow].sliders[curButton].clickGroup, windows[curWindow].sliders[curButton].clickAction, windows[curWindow].sliders[curButton].id, windows[curWindow].sliders[curButton].value);
							global::InputHandler.InputHandler.controllerDPadRightPressed = false;
						}
						if (num4 > -1 && componentType != 0)
						{
							windows[curWindow].sliders[curButton].status = 1;
							switch (componentType)
							{
							case 1:
								windows[curWindow].buttons[num].status = 2;
								windows[curWindow].curButton = (byte)num;
								windows[curWindow].curSlider = byte.MaxValue;
								mainC.soundsMain.Play_Sound_NonPositional(windows[curWindow].buttons[num].soundFocus);
								break;
							case 2:
								windows[curWindow].checkBoxes[num].status = 2;
								windows[curWindow].curCheckBox = (byte)num;
								windows[curWindow].curSlider = byte.MaxValue;
								mainC.soundsMain.Play_Sound_NonPositional(windows[curWindow].checkBoxes[num].soundFocus);
								break;
							case 4:
								windows[curWindow].labels[num].status = 2;
								windows[curWindow].curLabel = (byte)num;
								windows[curWindow].curSlider = byte.MaxValue;
								break;
							case 3:
								windows[curWindow].sliders[num].status = 2;
								windows[curWindow].curSlider = (byte)num;
								if (num != curButton)
								{
									mainC.soundsMain.Play_Sound_NonPositional(windows[curWindow].sliders[num].soundFocus);
								}
								break;
							case 5:
								windows[curWindow].groups[num].status = 2;
								windows[curWindow].curGroup = (byte)num;
								windows[curWindow].curSlider = byte.MaxValue;
								mainC.soundsMain.Play_Sound_NonPositional(windows[curWindow].groups[num].soundFocus);
								break;
							}
						}
					}
				}
				if (windows[curWindow].numGroups > 0)
				{
					ushort curButton = windows[curWindow].curGroup;
					if (curButton < windows[curWindow].numGroups && windows[curWindow].groups[curButton].status == 2)
					{
						int num4 = -1;
						if (windows[curWindow].groups[curButton].type == 0)
						{
							if (global::InputHandler.InputHandler.controllerButtonAPressed)
							{
								ushort num5 = Get_Checkbox_Index(curWindow, windows[curWindow].groups[curButton].items[0]);
								windows[curWindow].checkBoxes[num5].value = (byte)(++windows[curWindow].checkBoxes[num5].value % 2);
								Process_Window_Action(curWindow, windows[curWindow].checkBoxes[num5].clickGroup, windows[curWindow].checkBoxes[num5].clickAction, windows[curWindow].checkBoxes[num5].id, (int)windows[curWindow].checkBoxes[num5].value);
								global::InputHandler.InputHandler.controllerButtonAPressed = false;
								mainC.soundsMain.Play_Sound_NonPositional(windows[curWindow].checkBoxes[num5].soundClick);
							}
							else if (global::InputHandler.InputHandler.controllerDPadUpPressed)
							{
								num4 = 0;
								num = Get_Window_Object(curWindow, windows[curWindow].groups[curButton].componentUp);
								global::InputHandler.InputHandler.controllerDPadUpPressed = false;
							}
							else if (global::InputHandler.InputHandler.controllerDPadDownPressed)
							{
								num4 = 1;
								num = Get_Window_Object(curWindow, windows[curWindow].groups[curButton].componentDown);
								global::InputHandler.InputHandler.controllerDPadDownPressed = false;
							}
							else if (global::InputHandler.InputHandler.controllerDPadLeftPressed)
							{
								ushort num5 = Get_Slider_Index(curWindow, windows[curWindow].groups[curButton].items[1]);
								if (windows[curWindow].sliders[num5].value > 0f)
								{
									mainC.soundsMain.Play_Sound_NonPositional(windows[curWindow].sliders[num5].soundChange);
								}
								windows[curWindow].sliders[curButton].value -= windows[curWindow].sliders[num5].movementSpeed;
								if (windows[curWindow].sliders[num5].value < 0f)
								{
									windows[curWindow].sliders[num5].value = 0f;
								}
								Process_Window_Action(curWindow, windows[curWindow].sliders[num5].clickGroup, windows[curWindow].sliders[num5].clickAction, windows[curWindow].sliders[num5].id, windows[curWindow].sliders[num5].value);
								global::InputHandler.InputHandler.controllerDPadLeftPressed = false;
							}
							else if (global::InputHandler.InputHandler.controllerDPadRightPressed)
							{
								ushort num5 = Get_Slider_Index(curWindow, windows[curWindow].groups[curButton].items[1]);
								if (windows[curWindow].sliders[num5].value < 1f)
								{
									mainC.soundsMain.Play_Sound_NonPositional(windows[curWindow].sliders[num5].soundChange);
								}
								windows[curWindow].sliders[curButton].value += windows[curWindow].sliders[num5].movementSpeed;
								if (windows[curWindow].sliders[num5].value > 1f)
								{
									windows[curWindow].sliders[num5].value = 1f;
								}
								Process_Window_Action(curWindow, windows[curWindow].sliders[num5].clickGroup, windows[curWindow].sliders[num5].clickAction, windows[curWindow].sliders[num5].id, windows[curWindow].sliders[num5].value);
								global::InputHandler.InputHandler.controllerDPadRightPressed = false;
							}
							if (num4 > -1 && componentType != 0)
							{
								windows[curWindow].groups[curButton].status = 1;
								switch (componentType)
								{
								case 1:
									windows[curWindow].buttons[num].status = 2;
									windows[curWindow].curButton = (byte)num;
									windows[curWindow].curGroup = byte.MaxValue;
									mainC.soundsMain.Play_Sound_NonPositional(windows[curWindow].buttons[num].soundFocus);
									break;
								case 2:
									windows[curWindow].checkBoxes[num].status = 2;
									windows[curWindow].curCheckBox = (byte)num;
									windows[curWindow].curGroup = byte.MaxValue;
									mainC.soundsMain.Play_Sound_NonPositional(windows[curWindow].checkBoxes[num].soundFocus);
									break;
								case 4:
									windows[curWindow].labels[num].status = 2;
									windows[curWindow].curLabel = (byte)num;
									windows[curWindow].curGroup = byte.MaxValue;
									break;
								case 3:
									windows[curWindow].sliders[num].status = 2;
									windows[curWindow].curSlider = (byte)num;
									windows[curWindow].curGroup = byte.MaxValue;
									mainC.soundsMain.Play_Sound_NonPositional(windows[curWindow].sliders[num].soundFocus);
									break;
								case 5:
									windows[curWindow].groups[num].status = 2;
									windows[curWindow].curGroup = (byte)num;
									if (num != curButton)
									{
										mainC.soundsMain.Play_Sound_NonPositional(windows[curWindow].groups[num].soundFocus);
									}
									break;
								}
							}
						}
					}
				}
				if ((windows[curWindow].buttonFlags & 0x40) != 0)
				{
					if (global::InputHandler.InputHandler.controllerDPadUpPressed)
					{
						Process_Window_Action(curWindow, global::Util.Util.maxUnsignedShortValue, 1, 0, 1f);
					}
					else if (global::InputHandler.InputHandler.controllerDPadDownPressed)
					{
						Process_Window_Action(curWindow, global::Util.Util.maxUnsignedShortValue, 2, 0, 1f);
					}
					else if (global::InputHandler.InputHandler.controllerDPadLeftPressed)
					{
						Process_Window_Action(curWindow, global::Util.Util.maxUnsignedShortValue, 3, 0, 1f);
					}
					else if (global::InputHandler.InputHandler.controllerDPadRightPressed)
					{
						Process_Window_Action(curWindow, global::Util.Util.maxUnsignedShortValue, 4, 0, 1f);
					}
				}
				if (((global::InputHandler.InputHandler.controllerButtonBPressed && (windows[curWindow].windowCloseFlags & 2) > 0) || (global::InputHandler.InputHandler.controllerButtonBackPressed && (windows[curWindow].windowCloseFlags & 0x20) > 0) || (global::InputHandler.InputHandler.controllerButtonStartPressed && (windows[curWindow].windowCloseFlags & 0x10) > 0)) && Ok_To_Close_Window(curWindow))
				{
					Close_Window(curWindow);
				}
			}
			Process_Window(curWindow);
			global::InputHandler.InputHandler.controllerButtonAPressed = false;
			global::InputHandler.InputHandler.controllerButtonBPressed = false;
			global::InputHandler.InputHandler.controllerButtonXPressed = false;
			global::InputHandler.InputHandler.controllerButtonYPressed = false;
			global::InputHandler.InputHandler.controllerButtonStartPressed = false;
			global::InputHandler.InputHandler.controllerButtonBackPressed = false;
			global::InputHandler.InputHandler.controllerButtonLeftShoulderPressed = false;
			global::InputHandler.InputHandler.controllerButtonRightShoulderPressed = false;
			global::InputHandler.InputHandler.controllerDPadLeftPressed = false;
			global::InputHandler.InputHandler.controllerDPadRightPressed = false;
			global::InputHandler.InputHandler.controllerDPadUpPressed = false;
			global::InputHandler.InputHandler.controllerDPadDownPressed = false;
		}
		for (ushort num6 = 0; num6 < numWindows; num6++)
		{
			if (windows[num6].autoHideTimer > 0f)
			{
				windows[num6].autoHideTimer -= frameTime;
				if (windows[num6].autoHideTimer <= 0f)
				{
					windows[num6].actions = 4;
					Close_Window_Actions(num6);
				}
			}
			if ((windows[num6].actions & 3) > 0)
			{
				windows[num6].status = 1;
				if (windows[num6].type == 0)
				{
					curWindow = num6;
				}
				windowOrder[nextWindowPlace++] = num6;
				if ((windows[num6].actions & 1) > 0)
				{
					Reset_Components(num6);
					Set_Window_Start_Component(num6);
					windows[num6].state = 0;
				}
				windows[num6].actions = 0;
				Process_Show_Window(num6);
			}
			else if ((windows[num6].actions & 4) == 4)
			{
				windows[num6].status = 0;
				windows[num6].autoHideTimer = 0f;
				windows[num6].ignoreStickInputs = false;
				for (num = 0; num < nextWindowPlace; num++)
				{
					if (windowOrder[num] == num6)
					{
						ushort num5 = (ushort)(nextWindowPlace - 1);
						for (ushort curButton = num; curButton < num5; curButton++)
						{
							windowOrder[curButton] = windowOrder[curButton + 1];
						}
						break;
					}
				}
				if (nextWindowPlace > 0)
				{
					nextWindowPlace--;
				}
				if (windows[num6].type == 0)
				{
					curWindow = 0;
					if (nextWindowPlace > 0)
					{
						for (int num4 = nextWindowPlace - 1; num4 > -1; num4--)
						{
							if (windows[windowOrder[num4]].type == 0)
							{
								curWindow = windowOrder[num4];
								break;
							}
						}
					}
				}
				windows[num6].actions = 0;
			}
		}
	}

	public void Process_Show_Window(ushort windowID)
	{
		ushort num = windowID;
		if (num == 24)
		{
			mainC.gameLogic.Game_Handle_Choose_Map(254);
		}
	}

	public void Process_Window_Action(ushort windowID, ushort groupID, ushort actionID, ushort componentID, float value)
	{
		ushort num = 0;
		switch (groupID)
		{
		case 0:
		{
			ushort num3 = actionID;
			if (num3 == 2)
			{
				mainC.gameLogic.Game_Handle_Options(actionID, componentID, value);
				num = 2;
			}
			break;
		}
		case 1:
			mainC.gameLogic.Game_Handle_Main_Menu(actionID);
			num = 2;
			break;
		case 2:
			mainC.gameLogic.Game_Handle_BuyMe_Window(actionID);
			num = 2;
			break;
		case 3:
			num = mainC.gameLogic.Game_Handle_Confirm_Window(actionID);
			break;
		case 4:
			mainC.gameLogic.Game_Handle_SignIn_Window(actionID);
			num = 2;
			break;
		case 5:
			num = mainC.gameLogic.Game_Handle_In_Game_Menu(actionID);
			break;
		case 6:
			num = mainC.gameLogic.Game_Handle_Play_Menu(actionID);
			break;
		case 7:
			switch (windowID)
			{
			case 10:
				mainC.gameLogic.Game_Handle_Results_Window(actionID);
				num = 2;
				break;
			case 255:
				num = mainC.gameLogic.Game_Handle_Vehicle_Select(actionID);
				break;
			case 11:
				num = mainC.gameLogic.Game_Handle_Weapon_Select(actionID);
				break;
			case 22:
				num = mainC.gameLogic.Game_Handle_Mission_Objectives(actionID);
				break;
			case 23:
				num = mainC.gameLogic.Game_Handle_Game_Over(actionID);
				break;
			case 24:
				num = mainC.gameLogic.Game_Handle_Choose_Map(actionID);
				break;
			case 14:
				num = 2;
				switch (actionID)
				{
				case 1:
					MainGame.storageDeviceNotChosen = true;
					num = 0;
					break;
				case 2:
					num = 0;
					break;
				}
				break;
			}
			break;
		default:
		{
			ushort num2 = windowID;
			if (num2 == 13)
			{
				mainC.gameLogic.Game_Handle_Instructions_Window(actionID);
				num = 2;
			}
			break;
		}
		}
		if (actionID == 0 || num == 0)
		{
			Close_Window(windowID);
		}
	}

	public bool Ok_To_Close_Window(ushort windowID)
	{
		ushort num = 0;
		ushort num2 = windowID;
		if (num2 == 7)
		{
			num = mainC.gameLogic.Game_Handle_Play_Menu(255);
		}
		if (num != 0)
		{
			return false;
		}
		return true;
	}

	public void Process_Window(ushort windowID)
	{
		switch (windowID)
		{
		case 1:
			mainC.gameLogic.Game_Handle_Main_Menu(0);
			break;
		case 4:
			mainC.gameLogic.Game_Handle_BuyMe_Window(0);
			break;
		case 2:
			mainC.gameLogic.Game_Handle_SignIn_Window(0);
			break;
		case 5:
			mainC.gameLogic.Game_Handle_In_Game_Menu(0);
			break;
		case 7:
			mainC.gameLogic.Game_Handle_Play_Menu(0);
			break;
		case 10:
			mainC.gameLogic.Game_Handle_Results_Window(0);
			break;
		case 255:
			mainC.gameLogic.Game_Handle_Vehicle_Select(0);
			break;
		case 9:
			mainC.gameLogic.Game_Handle_Scores_Window();
			break;
		case 15:
			Handle_Idle_Timeout_Window();
			break;
		case 11:
			mainC.gameLogic.Game_Handle_Weapon_Select(0);
			break;
		case 22:
			mainC.gameLogic.Game_Handle_Mission_Objectives(0);
			break;
		case 23:
			mainC.gameLogic.Game_Handle_Game_Over(0);
			break;
		case 24:
			mainC.gameLogic.Game_Handle_Choose_Map(0);
			break;
		}
	}

	public void Show_Window(ushort windowID, ushort parentID, bool resetButtons)
	{
		windows[windowID].autoHideTimer = 0f;
		if (windows[windowID].status != 1)
		{
			windows[windowID].parentID = parentID;
			if (resetButtons)
			{
				windows[windowID].actions |= 1;
			}
			else
			{
				windows[windowID].actions |= 2;
			}
			if (windows[windowID].soundOpen != null)
			{
				mainC.soundsMain.Play_Sound_NonPositional(windows[windowID].soundOpen);
			}
		}
	}

	public void Show_Window_Specified_Time(ushort windowID, ushort parentID, bool resetButtons, float timeToShow)
	{
		windows[windowID].autoHideTimer = timeToShow;
		if (windows[windowID].status != 1)
		{
			windows[windowID].parentID = parentID;
			if (resetButtons)
			{
				windows[windowID].actions |= 1;
			}
			else
			{
				windows[windowID].actions |= 2;
			}
			if (windows[windowID].soundOpen != null)
			{
				mainC.soundsMain.Play_Sound_NonPositional(windows[windowID].soundOpen);
			}
		}
	}

	public void Ignore_Stick_Input(ushort windowID)
	{
		windows[windowID].ignoreStickInputs = true;
	}

	public void Close_Window(ushort windowID)
	{
		if (windows[windowID].status != 0)
		{
			windows[windowID].actions |= 4;
			Close_Window_Actions(windowID);
		}
	}

	public void Close_Window_After_Specified_Time(ushort windowID, float time)
	{
		if (windows[windowID].status != 0)
		{
			windows[windowID].autoHideTimer = time;
		}
	}

	public void Close_Window_Actions(ushort windowID)
	{
		if (windows[windowID].soundClose != null)
		{
			mainC.soundsMain.Play_Sound_NonPositional(windows[windowID].soundClose);
		}
		switch (windowID)
		{
		case 255:
			vehicleSelectFinished = true;
			switch (MainGame.gameMode)
			{
			case 0:
				global::Players.Players.lastSPVehicle = curVehicleSelect;
				break;
			case 1:
				global::Players.Players.lastMPVehicle = curVehicleSelect;
				break;
			}
			lastVehicleSelected = curVehicleSelect;
			break;
		case 11:
			weaponSelectFinished = true;
			global::Players.Players.players[0].primaryWeaponMountWeapon = (sbyte)global::Weapons.Weapons.weaponSelectWeaponIDs[curWeaponSelectColumn];
			mainC.gameLogic.Game_Set_Vehicle_Weapons(0);
			switch (MainGame.gameMode)
			{
			case 0:
				global::Players.Players.lastSPWeapon = curWeaponSelectColumn;
				global::Players.Players.lastSPWeapon2 = curWeaponSelectColumn2;
				break;
			case 1:
				global::Players.Players.lastMPWeapon = curWeaponSelectColumn;
				global::Players.Players.lastMPWeapon2 = curWeaponSelectColumn2;
				break;
			}
			lastWeaponSelected = curWeaponSelectColumn;
			break;
		}
		mainC.gameLogic.Game_Close_Window(windowID);
	}

	public bool Toggle_Window(ushort windowID, ushort parentID, bool resetButtons)
	{
		if (windows[windowID].status == 0)
		{
			Show_Window(windowID, parentID, resetButtons);
			return true;
		}
		Close_Window(windowID);
		return false;
	}

	public void Set_Window_Start_Component(ushort windowID)
	{
		windows[windowID].curButton = 0;
		windows[windowID].curCheckBox = 0;
		windows[windowID].curGroup = 0;
		windows[windowID].curLabel = 0;
		windows[windowID].curSlider = 0;
		windows[windowID].curStaticGraphic = 0;
		windows[windowID].curTab = 0;
		windows[windowID].curTextArea = 0;
		windows[windowID].curTextButton = 0;
		ushort num = 0;
		for (ushort num2 = 0; num2 < windows[windowID].numButtons; num2++)
		{
			if (windows[windowID].buttons[num2].id == windows[windowID].startComponent)
			{
				windows[windowID].curButton = (byte)num2;
				windows[windowID].buttons[num2].status = 2;
				num = 1;
				return;
			}
		}
		if (num == 0)
		{
			for (ushort num2 = 0; num2 < windows[windowID].numTextButtons; num2++)
			{
				if (windows[windowID].textButtons[num2].id == windows[windowID].startComponent)
				{
					windows[windowID].curTextButton = (byte)num2;
					windows[windowID].textButtons[num2].status = 2;
					num = 1;
					return;
				}
			}
		}
		if (num == 0)
		{
			for (ushort num2 = 0; num2 < windows[windowID].numCheckBoxes; num2++)
			{
				if (windows[windowID].checkBoxes[num2].id == windows[windowID].startComponent)
				{
					windows[windowID].curCheckBox = (byte)num2;
					windows[windowID].checkBoxes[num2].status = 2;
					num = 1;
					return;
				}
			}
		}
		if (num == 0)
		{
			for (ushort num2 = 0; num2 < windows[windowID].numSliders; num2++)
			{
				if (windows[windowID].sliders[num2].id == windows[windowID].startComponent)
				{
					windows[windowID].curSlider = (byte)num2;
					windows[windowID].sliders[num2].status = 2;
					num = 1;
					return;
				}
			}
		}
		if (num != 0)
		{
			return;
		}
		for (ushort num2 = 0; num2 < windows[windowID].numGroups; num2++)
		{
			if (windows[windowID].groups[num2].id == windows[windowID].startComponent)
			{
				windows[windowID].curGroup = (byte)num2;
				windows[windowID].groups[num2].status = 2;
				num = 1;
				break;
			}
		}
	}

	public void Reset_Components(ushort windowID)
	{
		for (ushort num = 0; num < windows[windowID].numLabels; num++)
		{
			if (windows[windowID].labels[num].status == 2)
			{
				windows[windowID].labels[num].status = 1;
			}
		}
		for (ushort num = 0; num < windows[windowID].numButtons; num++)
		{
			if (windows[windowID].buttons[num].status == 2)
			{
				windows[windowID].buttons[num].status = 1;
			}
		}
		for (ushort num = 0; num < windows[windowID].numTextButtons; num++)
		{
			if (windows[windowID].textButtons[num].status == 2)
			{
				windows[windowID].textButtons[num].status = 1;
			}
		}
		for (ushort num = 0; num < windows[windowID].numCheckBoxes; num++)
		{
			if (windows[windowID].checkBoxes[num].status == 2)
			{
				windows[windowID].checkBoxes[num].status = 1;
			}
		}
		for (ushort num = 0; num < windows[windowID].numSliders; num++)
		{
			if (windows[windowID].sliders[num].status == 2)
			{
				windows[windowID].sliders[num].status = 1;
			}
		}
		for (ushort num = 0; num < windows[windowID].numGroups; num++)
		{
			if (windows[windowID].groups[num].status == 2)
			{
				windows[windowID].groups[num].status = 1;
			}
		}
		for (ushort num = 0; num < windows[windowID].numTextAreas; num++)
		{
			if (windows[windowID].textAreas[num].status == 2)
			{
				windows[windowID].textAreas[num].status = 1;
			}
		}
	}

	public void Load_Main_Menu()
	{
		mainMenuErrorTime = 0f;
		mainC.inputMain.Reset_Second_Controller_Checks();
		mainC.soundsMain.Set_Music(global::Sounds.Sounds.musicMenuID);
		mainC.soundsMain.Stop_All_Game_Sounds(stopNarrator: true);
		global::InputHandler.InputHandler.controllerStickLeftRepeatX = 0f;
		global::InputHandler.InputHandler.controllerStickLeftRepeatY = 0f;
		global::Rendering.Rendering.renderMenuScreen = 2;
		mainC.gameLogic.Game_Show_MainMenu_Window();
	}

	public void Load_In_Game_Menu()
	{
		mainC.soundsMain.Stop_All_Game_Sounds(stopNarrator: false);
		global::InputHandler.InputHandler.controllerStickLeftRepeatX = 0f;
		global::InputHandler.InputHandler.controllerStickLeftRepeatY = 0f;
		global::Rendering.Rendering.renderMenuScreen = 4;
		mainC.gameLogic.Game_UI_Update_In_Game_Window(5);
		Show_Window(5, 5, resetButtons: false);
	}

	public void Load_Vehicle_Select()
	{
		hideVehicle = false;
		vehicleSelectTimer = 0f;
		vehicleSelectScreenOpen = true;
		vehicleSelectFinished = false;
		Show_Window(255, 255, resetButtons: false);
		ref Matrix reference = ref global::Players.Players.players[0].mv[0];
		reference = Matrix.Identity;
		ref Matrix reference2 = ref global::Players.Players.players[0].mv[1];
		reference2 = Matrix.Identity;
		curVehicleSelect = 0;
		switch (MainGame.gameMode)
		{
		case 0:
			curVehicleSelect = global::Players.Players.lastSPVehicle;
			break;
		case 1:
			curVehicleSelect = global::Players.Players.lastMPVehicle;
			break;
		}
		mainC.gameLogic.Game_UI_Update_Vehicle_Select_Set();
	}

	public void Load_Weapon_Select()
	{
		curWeaponSelectArea = 0;
		weaponSelectAttachmentItemColumn = 0;
		weaponSelectAttachmentTypeColumn = 0;
		weaponSelectSkinColumn = 0;
		tauntColumn = 0;
		hideWeapon = false;
		weaponSelectTimer = 0f;
		weaponSelectScreenOpen = true;
		weaponSelectFinished = false;
		playerChangedWeaponOptions = false;
		Show_Window(11, 11, resetButtons: false);
		curWeaponSelectColumn = 0;
		curWeaponSelectColumn2 = 0;
		switch (MainGame.gameMode)
		{
		case 0:
			curWeaponSelectColumn = global::Players.Players.lastSPWeapon;
			curWeaponSelectColumn2 = global::Players.Players.lastSPWeapon2;
			break;
		case 1:
			curWeaponSelectColumn = global::Players.Players.lastMPWeapon;
			curWeaponSelectColumn2 = global::Players.Players.lastMPWeapon2;
			break;
		}
		Set_Weapon_Select_Variables();
		Get_Weapon_Variables();
		Set_Weapon_Select_Attachment_Item_Column();
		Set_Weapon_Select_Skin_Column();
		Set_Taunt_Column();
		mainC.gameLogic.Game_UI_Update_Weapon_Select_Set();
	}

	public void Load_Mission_Objectives()
	{
		missionObjectivesScreenOpen = true;
		missionObjectivesFinished = false;
		Show_Window(22, 22, resetButtons: false);
		mainC.gameLogic.Game_UI_Update_Mission_Objectives(22);
	}

	public void Load_Game_Over()
	{
		Show_Window(23, 23, resetButtons: false);
		mainC.gameLogic.Game_UI_Update_Game_Over(23);
	}

	public void Load_Idle_Timeout_Window()
	{
		if (windows[15].status != 1)
		{
			idleTimeOut = 1f;
		}
		Show_Window(15, 15, resetButtons: false);
	}

	public void Handle_Idle_Timeout_Window()
	{
		float num = MainGame.idleTimeout - MainGame.curIdleTime;
		windows[15].textAreas[0].lines[0] = "Timeout in " + ((int)num + 1).ToString(CultureInfo.InvariantCulture) + " seconds.";
		if (num <= 5f)
		{
			idleTimeOut += MainGame.frametime;
			if (idleTimeOut >= 1f)
			{
				mainC.soundsMain.Play_Sound_NonPositional("TimeoutWarning");
				idleTimeOut = 0f;
			}
		}
	}

	public void Close_All_Windows()
	{
		for (ushort num = 0; num < numWindows; num++)
		{
			windows[num].status = 0;
		}
	}

	public void Close_Windows_For_End_Of_Round()
	{
		Close_Window(9);
		Close_Window(15);
		Close_Window(0);
		Close_Window(6);
		Close_Window(3);
		Close_Window(5);
		Close_Window(24);
		Close_Window(16);
	}

	public void Set_Weapon_Select_Variables()
	{
		weaponSelectAttachmentCount = 0;
		weaponSelectAttachmentType = 0;
		weaponSelectAttachmentTypeColumnCount = 0;
		weaponSelectWeaponID = global::Weapons.Weapons.weaponSelectWeaponIDs[curWeaponSelectColumn];
		byte b = mainC.weaponsMain.Count_Attachments_That_Fit_Mount(0, global::Weapons.Weapons.wp1[weaponSelectWeaponID].mountScope);
		if (b > 0)
		{
			if (weaponSelectAttachmentTypeColumn == weaponSelectAttachmentTypeColumnCount)
			{
				weaponSelectAttachmentType = 0;
				weaponSelectAttachmentCount = b;
			}
			weaponSelectAttachmentTypeColumnCount++;
		}
		b = mainC.weaponsMain.Count_Attachments_That_Fit_Mount(1, global::Weapons.Weapons.wp1[weaponSelectWeaponID].mountForeGrip);
		if (b > 0)
		{
			if (weaponSelectAttachmentTypeColumn == weaponSelectAttachmentTypeColumnCount)
			{
				weaponSelectAttachmentType = 1;
				weaponSelectAttachmentCount = b;
			}
			weaponSelectAttachmentTypeColumnCount++;
		}
		b = mainC.weaponsMain.Count_Attachments_That_Fit_Mount(2, global::Weapons.Weapons.wp1[weaponSelectWeaponID].mountBarrel);
		if (b > 0)
		{
			if (weaponSelectAttachmentTypeColumn == weaponSelectAttachmentTypeColumnCount)
			{
				weaponSelectAttachmentType = 2;
				weaponSelectAttachmentCount = b;
			}
			weaponSelectAttachmentTypeColumnCount++;
		}
		b = mainC.weaponsMain.Count_Attachments_That_Fit_Mount(3, global::Weapons.Weapons.wp1[weaponSelectWeaponID].mountEnergyDevice);
		if (b > 0)
		{
			if (weaponSelectAttachmentTypeColumn == weaponSelectAttachmentTypeColumnCount)
			{
				weaponSelectAttachmentType = 3;
				weaponSelectAttachmentCount = b;
			}
			weaponSelectAttachmentTypeColumnCount++;
		}
	}

	public void Get_Weapon_Variables()
	{
		newScopeID = mainC.weaponsMain.Get_Weapon_Preference(0, weaponSelectWeaponID, 0);
		newForeGripID = mainC.weaponsMain.Get_Weapon_Preference(0, weaponSelectWeaponID, 1);
		newBarrellID = mainC.weaponsMain.Get_Weapon_Preference(0, weaponSelectWeaponID, 2);
		newEnergyDeviceID = mainC.weaponsMain.Get_Weapon_Preference(0, weaponSelectWeaponID, 3);
		newSkinID = mainC.weaponsMain.Get_Weapon_Preference(0, weaponSelectWeaponID, 4);
		newTauntID = mainC.weaponsMain.Get_Weapon_Preference(0, 0, 5);
	}

	public void Set_Weapon_Select_Attachment_Item_Column()
	{
		weaponSelectAttachmentItemColumn = 0;
		switch (weaponSelectAttachmentType)
		{
		case 0:
		{
			for (byte b = 0; b < global::Weapons.Weapons.numScopes; b++)
			{
				if ((global::Weapons.Weapons.wp1[weaponSelectWeaponID].mountScope & global::Weapons.Weapons.scopes[b].mount) > 0)
				{
					if (b == newScopeID)
					{
						return;
					}
					weaponSelectAttachmentItemColumn++;
				}
			}
			break;
		}
		case 1:
		{
			for (byte b = 0; b < global::Weapons.Weapons.numForeGrips; b++)
			{
				if ((global::Weapons.Weapons.wp1[weaponSelectWeaponID].mountForeGrip & global::Weapons.Weapons.foreGrips[b].mount) > 0)
				{
					if (b == newForeGripID)
					{
						return;
					}
					weaponSelectAttachmentItemColumn++;
				}
			}
			break;
		}
		case 2:
		{
			for (byte b = 0; b < global::Weapons.Weapons.numBarrels; b++)
			{
				if ((global::Weapons.Weapons.wp1[weaponSelectWeaponID].mountBarrel & global::Weapons.Weapons.barrels[b].mount) > 0)
				{
					if (b == newBarrellID)
					{
						return;
					}
					weaponSelectAttachmentItemColumn++;
				}
			}
			break;
		}
		case 3:
		{
			for (byte b = 0; b < global::Weapons.Weapons.numEnergyDevices; b++)
			{
				if ((global::Weapons.Weapons.wp1[weaponSelectWeaponID].mountEnergyDevice & global::Weapons.Weapons.energyDevices[b].mount) > 0)
				{
					if (b == newEnergyDeviceID)
					{
						return;
					}
					weaponSelectAttachmentItemColumn++;
				}
			}
			break;
		}
		}
		if (weaponSelectAttachmentItemColumn >= weaponSelectAttachmentCount)
		{
			weaponSelectAttachmentItemColumn = 0;
		}
	}

	public void Set_Taunt_Column()
	{
		tauntColumn = newTauntID;
		if (tauntColumn > MainGame.numTaunts || (tauntColumn < MainGame.numTaunts && global::Players.Players.currentPlayerRank < MainGame.lockedTauntLevels[tauntColumn]))
		{
			tauntColumn = 0;
		}
	}

	public void Set_Weapon_Select_Skin_Column()
	{
		weaponSelectSkinColumn = newSkinID;
		if (weaponSelectSkinColumn >= global::Weapons.Weapons.wp1[weaponSelectWeaponID].numSkins || global::Players.Players.currentPlayerRank < global::Weapons.Weapons.lockedWeaponSkinLevels[weaponSelectSkinColumn])
		{
			weaponSelectSkinColumn = 0;
		}
	}

	public void Update_Weapon_Select_Attachment_Item_ID()
	{
		byte b = 0;
		switch (weaponSelectAttachmentType)
		{
		case 0:
		{
			for (byte b2 = 0; b2 < global::Weapons.Weapons.numScopes; b2++)
			{
				if ((global::Weapons.Weapons.wp1[weaponSelectWeaponID].mountScope & global::Weapons.Weapons.scopes[b2].mount) > 0)
				{
					if (b == weaponSelectAttachmentItemColumn)
					{
						newScopeID = b2;
						break;
					}
					b++;
				}
			}
			break;
		}
		case 1:
		{
			for (byte b2 = 0; b2 < global::Weapons.Weapons.numForeGrips; b2++)
			{
				if ((global::Weapons.Weapons.wp1[weaponSelectWeaponID].mountForeGrip & global::Weapons.Weapons.foreGrips[b2].mount) > 0)
				{
					if (b == weaponSelectAttachmentItemColumn)
					{
						newForeGripID = b2;
						break;
					}
					b++;
				}
			}
			break;
		}
		case 2:
		{
			for (byte b2 = 0; b2 < global::Weapons.Weapons.numBarrels; b2++)
			{
				if ((global::Weapons.Weapons.wp1[weaponSelectWeaponID].mountBarrel & global::Weapons.Weapons.barrels[b2].mount) > 0)
				{
					if (b == weaponSelectAttachmentItemColumn)
					{
						newBarrellID = b2;
						break;
					}
					b++;
				}
			}
			break;
		}
		case 3:
		{
			for (byte b2 = 0; b2 < global::Weapons.Weapons.numEnergyDevices; b2++)
			{
				if ((global::Weapons.Weapons.wp1[weaponSelectWeaponID].mountEnergyDevice & global::Weapons.Weapons.energyDevices[b2].mount) > 0)
				{
					if (b == weaponSelectAttachmentItemColumn)
					{
						newEnergyDeviceID = b2;
						break;
					}
					b++;
				}
			}
			break;
		}
		}
	}

	public void Swap_Buttons(ushort windowID, byte buttonToMakeVisible, byte buttonToHide)
	{
		for (ushort num = 0; num < windows[windowID].numButtons; num++)
		{
			if (num != buttonToHide && num != buttonToMakeVisible)
			{
				if (windows[windowID].buttons[num].componentDown == buttonToHide)
				{
					windows[windowID].buttons[num].componentDown = buttonToMakeVisible;
				}
				if (windows[windowID].buttons[num].componentUp == buttonToHide)
				{
					windows[windowID].buttons[num].componentUp = buttonToMakeVisible;
				}
				if (windows[windowID].buttons[num].componentRight == buttonToHide)
				{
					windows[windowID].buttons[num].componentRight = buttonToMakeVisible;
				}
				if (windows[windowID].buttons[num].componentLeft == buttonToHide)
				{
					windows[windowID].buttons[num].componentLeft = buttonToMakeVisible;
				}
			}
		}
		windows[windowID].buttons[buttonToHide].status = 0;
		windows[windowID].buttons[buttonToMakeVisible].status = 1;
		if (windows[windowID].curButton == buttonToHide || windows[windowID].curButton == buttonToMakeVisible)
		{
			windows[windowID].buttons[buttonToMakeVisible].status = 2;
			windows[windowID].curButton = buttonToMakeVisible;
		}
	}

	public void Swap_Text_Buttons(ushort windowID, byte buttonToMakeVisible, byte buttonToHide)
	{
		for (ushort num = 0; num < windows[windowID].numTextButtons; num++)
		{
			if (num != buttonToHide && num != buttonToMakeVisible)
			{
				if (windows[windowID].textButtons[num].componentDown == buttonToHide)
				{
					windows[windowID].textButtons[num].componentDown = buttonToMakeVisible;
				}
				if (windows[windowID].textButtons[num].componentUp == buttonToHide)
				{
					windows[windowID].textButtons[num].componentUp = buttonToMakeVisible;
				}
				if (windows[windowID].textButtons[num].componentRight == buttonToHide)
				{
					windows[windowID].textButtons[num].componentRight = buttonToMakeVisible;
				}
				if (windows[windowID].textButtons[num].componentLeft == buttonToHide)
				{
					windows[windowID].textButtons[num].componentLeft = buttonToMakeVisible;
				}
			}
		}
		windows[windowID].textButtons[buttonToHide].status = 0;
		windows[windowID].textButtons[buttonToMakeVisible].status = 1;
		if (windows[windowID].curTextButton == buttonToHide || windows[windowID].curTextButton == buttonToMakeVisible)
		{
			windows[windowID].textButtons[buttonToMakeVisible].status = 2;
			windows[windowID].curTextButton = buttonToMakeVisible;
		}
	}

	public void Set_Text_Button_Font(ushort windowID, byte buttonID, byte fontID)
	{
		windows[windowID].textButtons[buttonID].fontID = fontID;
	}

	public void Reset_Text_Buttons_Font(ushort windowID, byte fontID)
	{
		for (byte b = 0; b < windows[windowID].numTextButtons; b++)
		{
			windows[windowID].textButtons[b].fontID = fontID;
		}
	}

	public void Hide_Button(ushort windowID, byte buttonToHide)
	{
		for (ushort num = 0; num < windows[windowID].numButtons; num++)
		{
			if (num != buttonToHide)
			{
				if (windows[windowID].buttons[num].componentDown == buttonToHide)
				{
					windows[windowID].buttons[num].componentDown = windows[windowID].buttons[buttonToHide].componentDown;
				}
				if (windows[windowID].buttons[num].componentUp == buttonToHide)
				{
					windows[windowID].buttons[num].componentUp = windows[windowID].buttons[buttonToHide].componentUp;
				}
				if (windows[windowID].buttons[num].componentRight == buttonToHide)
				{
					windows[windowID].buttons[num].componentRight = windows[windowID].buttons[buttonToHide].componentRight;
				}
				if (windows[windowID].buttons[num].componentLeft == buttonToHide)
				{
					windows[windowID].buttons[num].componentLeft = windows[windowID].buttons[buttonToHide].componentLeft;
				}
			}
		}
		if (windows[windowID].curButton == buttonToHide)
		{
			for (ushort num = 0; num < 5; num++)
			{
				switch (num)
				{
				case 0:
					if (windows[windowID].buttons[windows[windowID].buttons[buttonToHide].componentDown].status > 0)
					{
						windows[windowID].curButton = (byte)windows[windowID].buttons[buttonToHide].componentDown;
						windows[windowID].buttons[windows[windowID].buttons[buttonToHide].componentDown].status = 2;
					}
					num = 5;
					break;
				case 1:
					if (windows[windowID].buttons[windows[windowID].buttons[buttonToHide].componentUp].status > 0)
					{
						windows[windowID].curButton = (byte)windows[windowID].buttons[buttonToHide].componentUp;
						windows[windowID].buttons[windows[windowID].buttons[buttonToHide].componentUp].status = 2;
					}
					num = 5;
					break;
				case 2:
					if (windows[windowID].buttons[windows[windowID].buttons[buttonToHide].componentLeft].status > 0)
					{
						windows[windowID].curButton = (byte)windows[windowID].buttons[buttonToHide].componentLeft;
						windows[windowID].buttons[windows[windowID].buttons[buttonToHide].componentLeft].status = 2;
					}
					num = 5;
					break;
				case 3:
					if (windows[windowID].buttons[windows[windowID].buttons[buttonToHide].componentRight].status > 0)
					{
						windows[windowID].curButton = (byte)windows[windowID].buttons[buttonToHide].componentRight;
						windows[windowID].buttons[windows[windowID].buttons[buttonToHide].componentRight].status = 2;
					}
					num = 5;
					break;
				case 4:
					for (num = 0; num < windows[windowID].numButtons; num++)
					{
						if (windows[windowID].buttons[num].status > 0)
						{
							windows[windowID].curButton = (byte)num;
							windows[windowID].buttons[num].status = 2;
							break;
						}
					}
					break;
				}
			}
		}
		windows[windowID].buttons[buttonToHide].status = 0;
	}

	public void Show_Button(ushort windowID, byte buttonToShow)
	{
		for (ushort num = 0; num < windows[windowID].numButtons; num++)
		{
			if (num != buttonToShow)
			{
				if (windows[windowID].buttons[num].componentDown == windows[windowID].buttons[buttonToShow].componentDown)
				{
					windows[windowID].buttons[num].componentDown = buttonToShow;
				}
				if (windows[windowID].buttons[num].componentUp == windows[windowID].buttons[buttonToShow].componentUp)
				{
					windows[windowID].buttons[num].componentUp = buttonToShow;
				}
				if (windows[windowID].buttons[num].componentRight == windows[windowID].buttons[buttonToShow].componentRight)
				{
					windows[windowID].buttons[num].componentRight = buttonToShow;
				}
				if (windows[windowID].buttons[num].componentLeft == windows[windowID].buttons[buttonToShow].componentLeft)
				{
					windows[windowID].buttons[num].componentLeft = buttonToShow;
				}
			}
		}
		windows[windowID].buttons[buttonToShow].status = 1;
		if (windows[windowID].curButton == buttonToShow)
		{
			windows[windowID].buttons[buttonToShow].status = 2;
		}
	}

	public void Hide_TextButton(ushort windowID, byte textButtonToHide)
	{
		for (ushort num = 0; num < windows[windowID].numTextButtons; num++)
		{
			if (num != textButtonToHide)
			{
				if (windows[windowID].textButtons[num].componentDown == textButtonToHide)
				{
					windows[windowID].textButtons[num].componentDown = windows[windowID].textButtons[textButtonToHide].componentDown;
				}
				if (windows[windowID].textButtons[num].componentUp == textButtonToHide)
				{
					windows[windowID].textButtons[num].componentUp = windows[windowID].textButtons[textButtonToHide].componentUp;
				}
				if (windows[windowID].textButtons[num].componentRight == textButtonToHide)
				{
					windows[windowID].textButtons[num].componentRight = windows[windowID].textButtons[textButtonToHide].componentRight;
				}
				if (windows[windowID].textButtons[num].componentLeft == textButtonToHide)
				{
					windows[windowID].textButtons[num].componentLeft = windows[windowID].textButtons[textButtonToHide].componentLeft;
				}
			}
		}
		if (windows[windowID].curTextButton == textButtonToHide)
		{
			for (ushort num = 0; num < 5; num++)
			{
				switch (num)
				{
				case 0:
					if (windows[windowID].textButtons[windows[windowID].textButtons[textButtonToHide].componentDown].status > 0)
					{
						windows[windowID].curTextButton = (byte)windows[windowID].textButtons[textButtonToHide].componentDown;
						windows[windowID].textButtons[windows[windowID].textButtons[textButtonToHide].componentDown].status = 2;
					}
					num = 5;
					break;
				case 1:
					if (windows[windowID].textButtons[windows[windowID].textButtons[textButtonToHide].componentUp].status > 0)
					{
						windows[windowID].curTextButton = (byte)windows[windowID].textButtons[textButtonToHide].componentUp;
						windows[windowID].textButtons[windows[windowID].textButtons[textButtonToHide].componentUp].status = 2;
					}
					num = 5;
					break;
				case 2:
					if (windows[windowID].textButtons[windows[windowID].textButtons[textButtonToHide].componentLeft].status > 0)
					{
						windows[windowID].curTextButton = (byte)windows[windowID].textButtons[textButtonToHide].componentLeft;
						windows[windowID].textButtons[windows[windowID].textButtons[textButtonToHide].componentLeft].status = 2;
					}
					num = 5;
					break;
				case 3:
					if (windows[windowID].textButtons[windows[windowID].textButtons[textButtonToHide].componentRight].status > 0)
					{
						windows[windowID].curTextButton = (byte)windows[windowID].textButtons[textButtonToHide].componentRight;
						windows[windowID].textButtons[windows[windowID].textButtons[textButtonToHide].componentRight].status = 2;
					}
					num = 5;
					break;
				case 4:
					for (num = 0; num < windows[windowID].numTextButtons; num++)
					{
						if (windows[windowID].textButtons[num].status > 0)
						{
							windows[windowID].curTextButton = (byte)num;
							windows[windowID].textButtons[num].status = 2;
							break;
						}
					}
					break;
				}
			}
		}
		windows[windowID].textButtons[textButtonToHide].status = 0;
	}

	public void Show_TextButton(ushort windowID, byte textButtonToShow)
	{
		for (ushort num = 0; num < windows[windowID].numTextButtons; num++)
		{
			if (num != textButtonToShow)
			{
				if (windows[windowID].textButtons[num].componentDown == windows[windowID].textButtons[textButtonToShow].componentDown)
				{
					windows[windowID].textButtons[num].componentDown = textButtonToShow;
				}
				if (windows[windowID].textButtons[num].componentUp == windows[windowID].textButtons[textButtonToShow].componentUp)
				{
					windows[windowID].textButtons[num].componentUp = textButtonToShow;
				}
				if (windows[windowID].textButtons[num].componentRight == windows[windowID].textButtons[textButtonToShow].componentRight)
				{
					windows[windowID].textButtons[num].componentRight = textButtonToShow;
				}
				if (windows[windowID].textButtons[num].componentLeft == windows[windowID].textButtons[textButtonToShow].componentLeft)
				{
					windows[windowID].textButtons[num].componentLeft = textButtonToShow;
				}
			}
		}
		windows[windowID].textButtons[textButtonToShow].status = 1;
		if (windows[windowID].curTextButton == textButtonToShow)
		{
			windows[windowID].textButtons[textButtonToShow].status = 2;
		}
	}

	public void Mark_Window_As_Needing_Updating(ushort windowID)
	{
		windows[windowID].needsUpdating = true;
	}

	public void Hide_Text_Areas(ushort windowID)
	{
		ushort numTextAreas = windows[windowID].numTextAreas;
		for (ushort num = 0; num < numTextAreas; num++)
		{
			windows[windowID].textAreas[num].status = 0;
		}
	}

	public void Set_Component_Status(byte componentType, ushort windowID, ushort componentIndex, byte status)
	{
		switch (componentType)
		{
		case 1:
			if (windows[windowID].numButtons > componentIndex)
			{
				windows[windowID].buttons[componentIndex].status = status;
			}
			break;
		case 2:
			if (windows[windowID].numCheckBoxes > componentIndex)
			{
				windows[windowID].checkBoxes[componentIndex].status = status;
			}
			break;
		case 5:
			if (windows[windowID].numGroups > componentIndex)
			{
				windows[windowID].groups[componentIndex].status = status;
			}
			break;
		case 4:
			if (windows[windowID].numLabels > componentIndex)
			{
				windows[windowID].labels[componentIndex].status = status;
			}
			break;
		case 3:
			if (windows[windowID].numSliders > componentIndex)
			{
				windows[windowID].sliders[componentIndex].status = status;
			}
			break;
		case 7:
			if (windows[windowID].numTextAreas > componentIndex)
			{
				windows[windowID].textAreas[componentIndex].status = status;
			}
			break;
		case 6:
			if (windows[windowID].numTextButtons > componentIndex)
			{
				windows[windowID].textButtons[componentIndex].status = status;
			}
			break;
		case 8:
			if (windows[windowID].numStaticGraphics > componentIndex)
			{
				windows[windowID].staticGraphics[componentIndex].status = status;
			}
			break;
		}
	}

	public void Set_Component_To_Current_Component(byte componentType, ushort windowID, ushort componentIndex)
	{
		switch (componentType)
		{
		case 1:
			if (windows[windowID].numButtons > componentIndex)
			{
				windows[windowID].curButton = (byte)componentIndex;
			}
			break;
		case 2:
			if (windows[windowID].numCheckBoxes > componentIndex)
			{
				windows[windowID].curCheckBox = (byte)componentIndex;
			}
			break;
		case 5:
			if (windows[windowID].numGroups > componentIndex)
			{
				windows[windowID].curGroup = (byte)componentIndex;
			}
			break;
		case 4:
			if (windows[windowID].numLabels > componentIndex)
			{
				windows[windowID].curLabel = (byte)componentIndex;
			}
			break;
		case 3:
			if (windows[windowID].numSliders > componentIndex)
			{
				windows[windowID].curSlider = (byte)componentIndex;
			}
			break;
		case 7:
			if (windows[windowID].numTextAreas > componentIndex)
			{
				windows[windowID].curTextArea = (byte)componentIndex;
			}
			break;
		case 6:
			if (windows[windowID].numTextButtons > componentIndex)
			{
				windows[windowID].curTextButton = (byte)componentIndex;
			}
			break;
		case 8:
			if (windows[windowID].numStaticGraphics > componentIndex)
			{
				windows[windowID].curStaticGraphic = (byte)componentIndex;
			}
			break;
		}
	}

	public void Set_Component_Position(byte componentType, ushort windowID, ushort componentIndex, float x, float y)
	{
		switch (componentType)
		{
		case 1:
			if (windows[windowID].numButtons > componentIndex)
			{
				windows[windowID].buttons[componentIndex].x = x;
				windows[windowID].buttons[componentIndex].y = y;
			}
			break;
		case 2:
			if (windows[windowID].numCheckBoxes > componentIndex)
			{
				windows[windowID].checkBoxes[componentIndex].x = x;
				windows[windowID].checkBoxes[componentIndex].y = y;
			}
			break;
		case 5:
			if (windows[windowID].numGroups > componentIndex)
			{
				windows[windowID].groups[componentIndex].x = x;
				windows[windowID].groups[componentIndex].y = y;
			}
			break;
		case 4:
			if (windows[windowID].numLabels > componentIndex)
			{
				windows[windowID].labels[componentIndex].x = x;
				windows[windowID].labels[componentIndex].y = y;
			}
			break;
		case 3:
			if (windows[windowID].numSliders > componentIndex)
			{
				windows[windowID].sliders[componentIndex].x = x;
				windows[windowID].sliders[componentIndex].y = y;
			}
			break;
		case 7:
			if (windows[windowID].numTextAreas > componentIndex)
			{
				windows[windowID].textAreas[componentIndex].x = x;
				windows[windowID].textAreas[componentIndex].y = y;
			}
			break;
		case 6:
			if (windows[windowID].numTextButtons > componentIndex)
			{
				windows[windowID].textButtons[componentIndex].x = x;
				windows[windowID].textButtons[componentIndex].y = y;
			}
			break;
		case 8:
			if (windows[windowID].numStaticGraphics > componentIndex)
			{
				windows[windowID].staticGraphics[componentIndex].x = x;
				windows[windowID].staticGraphics[componentIndex].y = y;
			}
			break;
		}
	}

	public void Set_All_Component_Status(byte componentType, ushort windowID, byte status)
	{
		switch (componentType)
		{
		case 1:
		{
			for (ushort num = 0; num < windows[windowID].numButtons; num++)
			{
				windows[windowID].buttons[num].status = status;
			}
			break;
		}
		case 2:
		{
			for (ushort num = 0; num < windows[windowID].numCheckBoxes; num++)
			{
				windows[windowID].checkBoxes[num].status = status;
			}
			break;
		}
		case 5:
		{
			for (ushort num = 0; num < windows[windowID].numGroups; num++)
			{
				windows[windowID].groups[num].status = status;
			}
			break;
		}
		case 4:
		{
			for (ushort num = 0; num < windows[windowID].numLabels; num++)
			{
				windows[windowID].labels[num].status = status;
			}
			break;
		}
		case 3:
		{
			for (ushort num = 0; num < windows[windowID].numSliders; num++)
			{
				windows[windowID].sliders[num].status = status;
			}
			break;
		}
		case 7:
		{
			for (ushort num = 0; num < windows[windowID].numTextAreas; num++)
			{
				windows[windowID].textAreas[num].status = status;
			}
			break;
		}
		case 6:
		{
			for (ushort num = 0; num < windows[windowID].numTextButtons; num++)
			{
				windows[windowID].textButtons[num].status = status;
			}
			break;
		}
		case 8:
		{
			for (ushort num = 0; num < windows[windowID].numStaticGraphics; num++)
			{
				windows[windowID].staticGraphics[num].status = status;
			}
			break;
		}
		}
	}

	public byte Get_Component_Status(byte componentType, ushort windowID, ushort componentIndex)
	{
		switch (componentType)
		{
		case 1:
			if (windows[windowID].numButtons > componentIndex)
			{
				return windows[windowID].buttons[componentIndex].status;
			}
			break;
		case 2:
			if (windows[windowID].numCheckBoxes > componentIndex)
			{
				return windows[windowID].checkBoxes[componentIndex].status;
			}
			break;
		case 5:
			if (windows[windowID].numGroups > componentIndex)
			{
				return windows[windowID].groups[componentIndex].status;
			}
			break;
		case 4:
			if (windows[windowID].numLabels > componentIndex)
			{
				return windows[windowID].labels[componentIndex].status;
			}
			break;
		case 3:
			if (windows[windowID].numSliders > componentIndex)
			{
				return windows[windowID].sliders[componentIndex].status;
			}
			break;
		case 7:
			if (windows[windowID].numTextAreas > componentIndex)
			{
				return windows[windowID].textAreas[componentIndex].status;
			}
			break;
		case 6:
			if (windows[windowID].numTextButtons > componentIndex)
			{
				return windows[windowID].textButtons[componentIndex].status;
			}
			break;
		case 8:
			if (windows[windowID].numStaticGraphics > componentIndex)
			{
				return windows[windowID].staticGraphics[componentIndex].status;
			}
			break;
		}
		return 0;
	}

	public void Set_Window_Return_Value(ushort windowID, byte value)
	{
		windows[windows[windowID].parentID].returnValue = value;
	}

	public void Set_Text_Area_Text(ushort windowID, ushort componentID, string text)
	{
		string text2 = text;
		int num = 0;
		do
		{
			if (text2.Length <= 0)
			{
				continue;
			}
			int num2 = text2.IndexOf("^");
			if (num2 < 0)
			{
				text2 = "";
				num++;
				continue;
			}
			text2 = ((text2.Length <= num2 + 1) ? "" : text2.Substring(num2 + 1));
			if (num2 > 0)
			{
				num++;
			}
		}
		while (text2.Length > 0);
		windows[windowID].textAreas[componentID].numLines = (ushort)num;
		if (num == 0)
		{
			return;
		}
		windows[windowID].textAreas[componentID].lines = new string[num];
		for (int num2 = 0; num2 < num; num2++)
		{
			windows[windowID].textAreas[componentID].lines[num2] = "";
		}
		text2 = text.Replace("~", " ");
		num = 0;
		do
		{
			if (text2.Length <= 0)
			{
				continue;
			}
			int num2 = text2.IndexOf("^");
			if (num2 < 0)
			{
				windows[windowID].textAreas[componentID].lines[num] = text2;
				text2 = "";
				num++;
				continue;
			}
			if (num2 > 0)
			{
				windows[windowID].textAreas[componentID].lines[num] = text2.Substring(0, num2);
				num++;
			}
			text2 = ((text2.Length <= num2 + 1) ? "" : text2.Substring(num2 + 1));
		}
		while (text2.Length > 0);
	}

	public ushort Get_Window_Object(ushort windowID, ushort componentID)
	{
		ushort num = 0;
		ushort numButtons = windows[windowID].numButtons;
		while (num < numButtons)
		{
			if (componentID == windows[windowID].buttons[num].id)
			{
				componentType = 1;
				return num;
			}
			num++;
		}
		num = 0;
		numButtons = windows[windowID].numTextButtons;
		while (num < numButtons)
		{
			if (componentID == windows[windowID].textButtons[num].id)
			{
				componentType = 6;
				return num;
			}
			num++;
		}
		num = 0;
		numButtons = windows[windowID].numCheckBoxes;
		while (num < numButtons)
		{
			if (componentID == windows[windowID].checkBoxes[num].id)
			{
				componentType = 2;
				return num;
			}
			num++;
		}
		num = 0;
		numButtons = windows[windowID].numSliders;
		while (num < numButtons)
		{
			if (componentID == windows[windowID].sliders[num].id)
			{
				componentType = 3;
				return num;
			}
			num++;
		}
		num = 0;
		numButtons = windows[windowID].numGroups;
		while (num < numButtons)
		{
			if (componentID == windows[windowID].groups[num].id)
			{
				componentType = 5;
				return num;
			}
			num++;
		}
		num = 0;
		numButtons = windows[windowID].numLabels;
		while (num < numButtons)
		{
			if (componentID == windows[windowID].labels[num].id)
			{
				componentType = 4;
				return num;
			}
			num++;
		}
		num = 0;
		numButtons = windows[windowID].numTextAreas;
		while (num < numButtons)
		{
			if (componentID == windows[windowID].textAreas[num].id)
			{
				componentType = 7;
				return num;
			}
			num++;
		}
		num = 0;
		numButtons = windows[windowID].numStaticGraphics;
		while (num < numButtons)
		{
			if (componentID == windows[windowID].staticGraphics[num].id)
			{
				componentType = 8;
				return num;
			}
			num++;
		}
		componentType = 0;
		return 0;
	}

	public ushort Get_Checkbox_Index(ushort windowID, ushort componentID)
	{
		ushort num = 0;
		ushort numCheckBoxes = windows[windowID].numCheckBoxes;
		while (num < numCheckBoxes)
		{
			if (componentID == windows[windowID].checkBoxes[num].id)
			{
				return num;
			}
			num++;
		}
		return 0;
	}

	public ushort Get_Slider_Index(ushort windowID, ushort componentID)
	{
		ushort num = 0;
		ushort numSliders = windows[windowID].numSliders;
		while (num < numSliders)
		{
			if (componentID == windows[windowID].sliders[num].id)
			{
				return num;
			}
			num++;
		}
		return 0;
	}

	public void Toggle_Window_Visibility(ushort windowID, ushort parentID)
	{
		if (windows[windowID].status == 1)
		{
			Close_Window(windowID);
		}
		else
		{
			Show_Window(windowID, parentID, resetButtons: false);
		}
	}

	public void Set_Label_Text(ushort windowID, ushort labelID, string newText)
	{
		if (windowID < numWindows && labelID < windows[windowID].numLabels)
		{
			windows[windowID].labels[labelID].labelText = newText;
		}
	}

	public void Reset_User_Interface_Textures()
	{
		for (ushort num = 0; num < numWindows; num++)
		{
			windows[num].modTexID = (ushort)mainC.texturesMain.Find_Texture(windows[num].modTexture, 0);
			for (ushort num2 = 0; num2 < windows[num].numLabels; num2++)
			{
				windows[num].labels[num2].iconID = (ushort)mainC.texturesMain.Find_Texture(windows[num].labels[num2].icon, 0);
			}
			for (ushort num2 = 0; num2 < windows[num].numStaticGraphics; num2++)
			{
				if (string.Compare(windows[num].staticGraphics[num2].graphic, ".") != 0)
				{
					windows[num].staticGraphics[num2].graphicID = (ushort)mainC.texturesMain.Find_Texture(windows[num].staticGraphics[num2].graphic, 0);
				}
				else
				{
					windows[num].staticGraphics[num2].graphicID = global::Util.Util.maxUnsignedShortValue;
				}
			}
			for (ushort num2 = 0; num2 < windows[num].numCheckBoxes; num2++)
			{
				if (string.Compare(windows[num].checkBoxes[num2].texture1, ".") != 0)
				{
					windows[num].checkBoxes[num2].textureID1 = (ushort)mainC.texturesMain.Find_Texture(windows[num].checkBoxes[num2].texture1, 0);
				}
				else
				{
					windows[num].checkBoxes[num2].textureID1 = global::Util.Util.maxUnsignedShortValue;
				}
				if (string.Compare(windows[num].checkBoxes[num2].texture2, ".") != 0)
				{
					windows[num].checkBoxes[num2].textureID2 = (ushort)mainC.texturesMain.Find_Texture(windows[num].checkBoxes[num2].texture2, 0);
				}
				else
				{
					windows[num].checkBoxes[num2].textureID2 = global::Util.Util.maxUnsignedShortValue;
				}
				if (string.Compare(windows[num].checkBoxes[num2].icon, ".") != 0)
				{
					windows[num].checkBoxes[num2].iconID = (ushort)mainC.texturesMain.Find_Texture(windows[num].checkBoxes[num2].icon, 0);
				}
				else
				{
					windows[num].checkBoxes[num2].iconID = global::Util.Util.maxUnsignedShortValue;
				}
			}
			for (ushort num2 = 0; num2 < windows[num].numButtons; num2++)
			{
				if (string.Compare(windows[num].buttons[num2].button1, ".") != 0)
				{
					windows[num].buttons[num2].button1ID = (ushort)mainC.texturesMain.Find_Texture(windows[num].buttons[num2].button1, 0);
				}
				else
				{
					windows[num].buttons[num2].button1ID = global::Util.Util.maxUnsignedShortValue;
				}
				if (string.Compare(windows[num].buttons[num2].button2, ".") != 0)
				{
					windows[num].buttons[num2].button2ID = (ushort)mainC.texturesMain.Find_Texture(windows[num].buttons[num2].button2, 0);
				}
				else
				{
					windows[num].buttons[num2].button2ID = global::Util.Util.maxUnsignedShortValue;
				}
				if (string.Compare(windows[num].buttons[num2].icon, ".") != 0)
				{
					windows[num].buttons[num2].iconID = (ushort)mainC.texturesMain.Find_Texture(windows[num].buttons[num2].icon, 0);
				}
				else
				{
					windows[num].buttons[num2].iconID = global::Util.Util.maxUnsignedShortValue;
				}
			}
			for (ushort num2 = 0; num2 < windows[num].numSliders; num2++)
			{
				if (string.Compare(windows[num].sliders[num2].icon, ".") != 0)
				{
					windows[num].sliders[num2].iconID = (ushort)mainC.texturesMain.Find_Texture(windows[num].sliders[num2].icon, 0);
				}
				else
				{
					windows[num].sliders[num2].iconID = global::Util.Util.maxUnsignedShortValue;
				}
				if (string.Compare(windows[num].sliders[num2].texture1, ".") != 0)
				{
					windows[num].sliders[num2].textureID1 = (ushort)mainC.texturesMain.Find_Texture(windows[num].sliders[num2].texture1, 0);
				}
				else
				{
					windows[num].sliders[num2].textureID1 = global::Util.Util.maxUnsignedShortValue;
				}
				if (string.Compare(windows[num].sliders[num2].texture2, ".") != 0)
				{
					windows[num].sliders[num2].textureID2 = (ushort)mainC.texturesMain.Find_Texture(windows[num].sliders[num2].texture2, 0);
				}
				else
				{
					windows[num].sliders[num2].textureID2 = global::Util.Util.maxUnsignedShortValue;
				}
			}
		}
	}
}
