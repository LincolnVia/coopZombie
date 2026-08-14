using System;
using System.Globalization;
using System.IO;
using Microsoft.Xna.Framework;
using Models;
using Networking;
using Players;
using Programs;
using Rendering;
using Structs;
using WindowsGame1;

namespace Switches;

public class Switches
{
	private static short numSwitches = 0;

	private static short numAllocatedSwitches = 0;

	public static StructsClass.SwitchControl[] switch1;

	public Game1.MasterCollection mainC;

	public void Init_Vars(Game1.MasterCollection masterC)
	{
		mainC = masterC;
	}

	public void Initialize_Switches()
	{
	}

	public void Load_Switches_Data(string fileName)
	{
		int num = -1;
		for (int i = 0; i < numAllocatedSwitches; i++)
		{
			switch1[i].enabled = false;
			switch1[i].callBackType = 0;
			switch1[i].numFloatVars = 0;
			switch1[i].resetOnMinorStart = false;
			switch1[i].fixShowSwitch = false;
		}
		numSwitches = 0;
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
				if (array4[0].Equals("numSwitches", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 1;
				}
				else if (array4[0].Equals("Switch", StringComparison.OrdinalIgnoreCase))
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
				else if (array4[0].Equals("Models", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 7;
				}
				else if (array4[0].Equals("Type", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 8;
				}
				else if (array4[0].Equals("Enabled", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 9;
				}
				else if (array4[0].Equals("Position", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 10;
				}
				else if (array4[0].Equals("Rotation", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 11;
				}
				else if (array4[0].Equals("Box1", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 12;
				}
				else if (array4[0].Equals("Box2", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 13;
				}
				else if (array4[0].Equals("Group", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 14;
				}
				else if (array4[0].Equals("NumSounds", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 15;
				}
				else if (array4[0].Equals("Sounds", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 16;
				}
				else if (array4[0].Equals("CallBackType", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 17;
				}
				else if (array4[0].Equals("Floats", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 18;
				}
				else if (array4[0].Equals("resetOnMinorRestart", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 19;
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
					if (num5 > numAllocatedSwitches)
					{
						switch1 = new StructsClass.SwitchControl[num5];
						for (int i = numSwitches; i < num5; i++)
						{
							switch1[i] = new StructsClass.SwitchControl();
							switch1[i].callBackType = 0;
							switch1[i].numFloatVars = 0;
							switch1[i].numAllcoatedFloats = 0;
							switch1[i].resetOnMinorStart = false;
							switch1[i].fixShowSwitch = false;
						}
						numAllocatedSwitches = num5;
					}
					numSwitches = num5;
					break;
				}
				case 2:
					num++;
					if (num > -1 && num < numSwitches)
					{
						switch1[num].actionID = -1;
						switch1[num].id = -1;
						switch1[num].refID = -1;
						switch1[num].numModels = 0;
						switch1[num].enabled = false;
						switch1[num].type = 0;
						switch1[num].numFloatVars = 0;
						switch1[num].fixShowSwitch = false;
					}
					else
					{
						num = -1;
					}
					break;
				case 3:
					if (array4.Length > 1 && num > -1)
					{
						switch1[num].actionID = short.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 4:
					if (array4.Length > 1 && num > -1)
					{
						switch1[num].id = short.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 5:
					if (array4.Length > 1 && num > -1)
					{
						switch1[num].refID = short.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 6:
					if (array4.Length > 1 && num > -1)
					{
						switch1[num].numModels = byte.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
						switch1[num].modelList = new long[switch1[num].numModels];
						for (int i = 0; i < switch1[num].numModels; i++)
						{
							switch1[num].modelList[i] = -1L;
						}
					}
					break;
				case 7:
					if (array4.Length > 1 && num > -1)
					{
						for (int i = 0; i < switch1[num].numModels && i < array4.Length - 1; i++)
						{
							switch1[num].modelList[i] = mainC.modelsMain.Find_Model(array4[i + 1]);
						}
					}
					break;
				case 8:
					if (array4.Length > 1 && num > -1)
					{
						switch1[num].type = byte.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 9:
					if (array4.Length > 1 && num > -1 && short.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat) == 1)
					{
						switch1[num].enabled = true;
					}
					break;
				case 10:
					if (array4.Length > 3 && num > -1)
					{
						switch1[num].position.v[0] = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
						switch1[num].position.v[1] = float.Parse(array4[2], CultureInfo.InvariantCulture.NumberFormat);
						switch1[num].position.v[2] = float.Parse(array4[3], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 11:
					if (array4.Length > 3 && num > -1)
					{
						switch1[num].rotation.v[0] = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
						switch1[num].rotation.v[1] = float.Parse(array4[2], CultureInfo.InvariantCulture.NumberFormat);
						switch1[num].rotation.v[2] = float.Parse(array4[3], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 12:
					if (array4.Length > 3 && num > -1)
					{
						switch1[num].b1.v[0] = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
						switch1[num].b1.v[1] = float.Parse(array4[2], CultureInfo.InvariantCulture.NumberFormat);
						switch1[num].b1.v[2] = float.Parse(array4[3], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 13:
					if (array4.Length > 3 && num > -1)
					{
						switch1[num].b2.v[0] = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
						switch1[num].b2.v[1] = float.Parse(array4[2], CultureInfo.InvariantCulture.NumberFormat);
						switch1[num].b2.v[2] = float.Parse(array4[3], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 14:
					if (array4.Length > 1 && num > -1)
					{
						switch1[num].group = byte.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 15:
					if (array4.Length > 1 && num > -1)
					{
						byte b = byte.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
						if (b > switch1[num].numAllocatedSounds)
						{
							switch1[num].sounds = new string[b];
							switch1[num].numAllocatedSounds = b;
						}
						for (int i = 0; i < b; i++)
						{
							switch1[num].sounds[i] = "";
						}
						switch1[num].numSounds = b;
					}
					break;
				case 16:
					if (array4.Length > 1)
					{
						byte b = switch1[num].numSounds;
						for (int i = 0; i < b && i < array4.Length - 1; i++)
						{
							switch1[num].sounds[i] = array4[i + 1];
						}
					}
					break;
				case 17:
					if (array4.Length > 1 && num > -1)
					{
						switch1[num].callBackType = byte.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 18:
				{
					if (array4.Length <= 1 || num <= -1)
					{
						break;
					}
					byte b = byte.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
					if (b > switch1[num].numAllcoatedFloats)
					{
						switch1[num].fVar = new float[b];
						switch1[num].numAllcoatedFloats = b;
					}
					if (array4.Length > b + 1)
					{
						for (int i = 0; i < b; i++)
						{
							switch1[num].fVar[i] = float.Parse(array4[i + 2], CultureInfo.InvariantCulture.NumberFormat);
						}
					}
					switch1[num].numFloatVars = b;
					break;
				}
				case 19:
					switch1[num].resetOnMinorStart = true;
					break;
				}
			}
		}
		stream.Close();
		for (num = 0; num < numSwitches; num++)
		{
			Matrix.CreateTranslation(switch1[num].position.v[0], switch1[num].position.v[1], switch1[num].position.v[2], out switch1[num].mv);
			Matrix.CreateRotationX(switch1[num].rotation.v[0] * ((float)Math.PI / 180f), out var result);
			switch1[num].mv = result * switch1[num].mv;
			Matrix.CreateRotationY(switch1[num].rotation.v[1] * ((float)Math.PI / 180f), out result);
			switch1[num].mv = result * switch1[num].mv;
			Matrix.CreateRotationZ(switch1[num].rotation.v[2] * ((float)Math.PI / 180f), out result);
			switch1[num].mv = result * switch1[num].mv;
		}
	}

	public void FIX_Check_SwitchType_One_For_Onscreen_Message()
	{
		global::Rendering.Rendering.fixShowSwitch = false;
		for (short num = 0; num < numSwitches; num++)
		{
			if (switch1[num].enabled && switch1[num].type == 1 && switch1[num].state == 0)
			{
				if (global::Players.Players.players[0].charP.position.v[0] > switch1[num].position.v[0] + switch1[num].b1.v[0] && global::Players.Players.players[0].charP.position.v[0] < switch1[num].position.v[0] + switch1[num].b2.v[0] && global::Players.Players.players[0].charP.position.v[1] > switch1[num].position.v[1] + switch1[num].b1.v[1] && global::Players.Players.players[0].charP.position.v[1] < switch1[num].position.v[1] + switch1[num].b2.v[1] && global::Players.Players.players[0].charP.position.v[2] > switch1[num].position.v[2] + switch1[num].b1.v[2] && global::Players.Players.players[0].charP.position.v[2] < switch1[num].position.v[2] + switch1[num].b2.v[2])
				{
					if (!switch1[num].fixShowSwitch)
					{
						mainC.soundsMain.Play_Priority_Sound("DialogAppear", 0f, 0f, 0f, 0f, 0f, 0f);
						switch1[num].fixShowSwitch = true;
					}
					global::Rendering.Rendering.fixShowSwitch = true;
					break;
				}
				switch1[num].fixShowSwitch = false;
			}
		}
	}

	public bool Process_SwitchType_One()
	{
		for (short num = 0; num < numSwitches; num++)
		{
			if (switch1[num].enabled && switch1[num].state == 0 && global::Players.Players.players[0].charP.position.v[0] > switch1[num].position.v[0] + switch1[num].b1.v[0] && global::Players.Players.players[0].charP.position.v[0] < switch1[num].position.v[0] + switch1[num].b2.v[0] && global::Players.Players.players[0].charP.position.v[1] > switch1[num].position.v[1] + switch1[num].b1.v[1] && global::Players.Players.players[0].charP.position.v[1] < switch1[num].position.v[1] + switch1[num].b2.v[1] && global::Players.Players.players[0].charP.position.v[2] > switch1[num].position.v[2] + switch1[num].b1.v[2] && global::Players.Players.players[0].charP.position.v[2] < switch1[num].position.v[2] + switch1[num].b2.v[2])
			{
				byte type = switch1[num].type;
				if (type == 1)
				{
					short refID = switch1[num].refID;
					if (global::Programs.Programs.pgBasic[refID].status < 2)
					{
						mainC.programsMain.Run_Program_Basic(refID, toggleDirection: false, switch1[num].callBackType, (byte)num);
					}
					else
					{
						mainC.programsMain.Run_Program_Basic(refID, toggleDirection: true, switch1[num].callBackType, (byte)num);
					}
					short num2 = switch1[num].group;
					for (refID = 0; refID < numSwitches; refID++)
					{
						if (switch1[refID].group == num2)
						{
							switch1[refID].state = 1;
						}
					}
					switch (switch1[num].id)
					{
					case 0:
						mainC.soundsMain.Play_Narrator_Voice(switch1[num].sounds[0]);
						break;
					case 1:
						mainC.soundsMain.Play_Voice(switch1[num].sounds[0], global::Players.Players.players[0].charP.position.v[0], global::Players.Players.players[0].charP.position.v[1], global::Players.Players.players[0].charP.position.v[2], 0f, 0f, 0f);
						break;
					case 2:
						mainC.soundsMain.Play_Priority_Sound(switch1[num].sounds[0], switch1[num].position.v[0], switch1[num].position.v[1], switch1[num].position.v[2], 0f, 0f, 0f);
						break;
					}
					if (switch1[num].actionID == 0)
					{
						mainC.mapsMain.Change_SpawnPoint(0, switch1[num].fVar[0], switch1[num].fVar[1], switch1[num].fVar[2], switch1[num].fVar[3]);
					}
					return true;
				}
			}
		}
		return false;
	}

	public void Process_SwitchType_One_From_Network()
	{
		byte b = global::Networking.Networking.networkBytes[0];
		if (switch1[b].state != 0)
		{
			return;
		}
		byte b2 = (byte)switch1[b].refID;
		switch1[b].state = 1;
		if (global::Programs.Programs.pgBasic[b2].status < 2)
		{
			global::Networking.Networking.networkBools[0] = false;
			mainC.networkingMain.XBOX_Send_Network_Message8(8);
			mainC.programsMain.Run_Program_Basic(b2, toggleDirection: false, switch1[b].callBackType, b);
		}
		else
		{
			global::Networking.Networking.networkBools[0] = true;
			mainC.networkingMain.XBOX_Send_Network_Message8(8);
			mainC.programsMain.Run_Program_Basic(b2, toggleDirection: true, switch1[b].callBackType, b);
		}
		switch (switch1[b].id)
		{
		case 0:
			mainC.soundsMain.Play_Narrator_Voice(switch1[b].sounds[0]);
			break;
		case 1:
			mainC.soundsMain.Play_Voice(switch1[b].sounds[0], switch1[b].position.v[0], switch1[b].position.v[1], switch1[b].position.v[2], 0f, 0f, 0f);
			break;
		case 2:
			mainC.soundsMain.Play_Priority_Sound(switch1[b].sounds[0], switch1[b].position.v[0], switch1[b].position.v[1], switch1[b].position.v[2], 0f, 0f, 0f);
			break;
		}
		if (switch1[b].actionID == 0)
		{
			mainC.mapsMain.Change_SpawnPoint(0, switch1[b].fVar[0], switch1[b].fVar[1], switch1[b].fVar[2], switch1[b].fVar[3]);
		}
		b = switch1[b].group;
		for (b2 = 0; b2 < numSwitches; b2++)
		{
			if (switch1[b2].group == b)
			{
				switch1[b2].state = 1;
			}
		}
	}

	public void Process_SwitchType_One_From_Network_Host()
	{
		byte b = global::Networking.Networking.networkBytes[0];
		byte pgID = (byte)switch1[b].refID;
		switch (switch1[b].id)
		{
		case 0:
			mainC.soundsMain.Play_Narrator_Voice(switch1[b].sounds[0]);
			break;
		case 1:
			mainC.soundsMain.Play_Voice(switch1[b].sounds[0], switch1[b].position.v[0], switch1[b].position.v[1], switch1[b].position.v[2], 0f, 0f, 0f);
			break;
		case 2:
			mainC.soundsMain.Play_Priority_Sound(switch1[b].sounds[0], switch1[b].position.v[0], switch1[b].position.v[1], switch1[b].position.v[2], 0f, 0f, 0f);
			break;
		}
		if (switch1[b].actionID == 0)
		{
			mainC.mapsMain.Change_SpawnPoint(0, switch1[b].fVar[0], switch1[b].fVar[1], switch1[b].fVar[2], switch1[b].fVar[3]);
		}
		switch1[b].state = 1;
		mainC.programsMain.Run_Program_Basic(pgID, global::Networking.Networking.networkBools[0], switch1[b].callBackType, b);
		b = switch1[b].group;
		for (pgID = 0; pgID < numSwitches; pgID++)
		{
			if (switch1[pgID].group == b)
			{
				switch1[pgID].state = 1;
			}
		}
	}

	public void SwitchType_One_Callback(byte switchID, byte action)
	{
		byte b = switch1[switchID].group;
		for (byte b2 = 0; b2 < numSwitches; b2++)
		{
			if (switch1[b2].group == b)
			{
				switch1[b2].state = 0;
			}
		}
	}

	public void Render_Switches()
	{
		for (int i = 0; i < numSwitches; i++)
		{
			if (!switch1[i].enabled)
			{
				continue;
			}
			byte type = switch1[i].type;
			if (type == 1)
			{
				for (int j = 0; j < switch1[i].numModels; j++)
				{
					global::Models.Models.mod1[global::Models.Models.modDoorSwitchScreen].texMovY = -0.5f * (float)(int)switch1[i].state;
					mainC.modelsMain.Render_Model(switch1[i].modelList[j], ref switch1[i].mv);
				}
			}
		}
	}

	public void Reset_Round(bool minorRestart)
	{
		if (minorRestart)
		{
			for (int i = 0; i < numSwitches; i++)
			{
				if (switch1[i].resetOnMinorStart)
				{
					switch1[i].state = 0;
					switch1[i].fixShowSwitch = false;
				}
			}
		}
		else
		{
			for (int i = 0; i < numSwitches; i++)
			{
				switch1[i].state = 0;
				switch1[i].fixShowSwitch = false;
			}
		}
	}
}
