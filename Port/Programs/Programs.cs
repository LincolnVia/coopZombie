using System;
using System.Globalization;
using System.IO;
using System.Threading;
using InputHandler;
using Joints;
using MainGame;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Net;
using Networking;
using Players;
using Rendering;
using Structs;
using WindowsGame1;

namespace Programs;

public class Programs
{
	public static short numProgramsBasic = 0;

	public static short numAllocatedProgramsBasic;

	public static StructsClass.program[] pgBasic;

	public static StructsClass.program_collection[] pgC = new StructsClass.program_collection[2];

	private Thread InitPrograms;

	public Game1.MasterCollection mainC;

	public void Init_Vars(Game1.MasterCollection masterC)
	{
		mainC = masterC;
		pgC[0] = new StructsClass.program_collection();
		pgC[1] = new StructsClass.program_collection();
		InitPrograms = new Thread(Init_Programs);
		InitPrograms.Start();
	}

	public void Init_Programs()
	{
		_ = global::Rendering.Rendering.uBufferID;
		// Xbox 360 hardware-thread affinity hint omitted on desktop.
		Load_Programs("The_CoOp_Zombie_Game\\Config_Files\\programs.txt", ref pgC[0].pg1, ref pgC[0].animation1, 0);
		Load_Programs("The_CoOp_Zombie_Game\\Config_Files\\programs_camera.txt", ref pgC[1].pg1, ref pgC[1].animation1, 1);
	}

	public bool Create_Programs(int cnt, ref StructsClass.program[] createProgram)
	{
		createProgram = new StructsClass.program[cnt];
		for (int i = 0; i < cnt; i++)
		{
			createProgram[i] = default(StructsClass.program);
			createProgram[i].numJoints = 0;
			createProgram[i].numSteps = 0;
		}
		return true;
	}

	public void Load_Programs(string filename, ref StructsClass.program[] loadPg, ref StructsClass.animation[] loadAnimation, ushort pgcID)
	{
		int num = 0;
		int curItem = 0;
		int num2 = 0;
		int num3 = 0;
		_ = global::Rendering.Rendering.uBufferID;
		float radians = 1f;
		float speedFactor = 1f;
		pgC[pgcID].numPrograms = 0;
		pgC[pgcID].numAnimations = 0;
		Stream stream = TitleContainer.OpenStream(filename);
		byte[] array = new byte[stream.Length];
		if (stream.CanRead)
		{
			stream.Read(array, 0, array.Length);
			string text = mainC.utilMain.Byte_Array_To_String(array);
			string[] array2 = text.Split('\n', '\r');
			int i = 0;
			int num4 = 0;
			for (; i < array2.Length; i++)
			{
				if (array2[i].Length > 0)
				{
					num4++;
				}
			}
			if (num4 < 1)
			{
				stream.Close();
				return;
			}
			string[] array3 = new string[num4];
			i = 0;
			num4 = 0;
			for (; i < array2.Length; i++)
			{
				if (array2[i].Length > 0)
				{
					array3[num4++] = array2[i];
				}
			}
			for (i = 0; i < num4; i++)
			{
				array2 = array3[i].Split(' ', '\t');
				int j = 0;
				int num5 = 0;
				for (; j < array2.Length; j++)
				{
					if (array2[j].Length > 0)
					{
						num5++;
					}
				}
				if (num5 < 1)
				{
					continue;
				}
				string[] array4 = new string[num5];
				j = 0;
				num5 = 0;
				for (; j < array2.Length; j++)
				{
					if (array2[j].Length > 0)
					{
						array4[num5++] = array2[j];
					}
				}
				int num6 = 0;
				if (array4[0].Equals("num_programs", StringComparison.OrdinalIgnoreCase))
				{
					num6 = 1;
				}
				else if (array4[0].Equals("program", StringComparison.OrdinalIgnoreCase))
				{
					num6 = 2;
				}
				else if (array4[0].Equals("joints", StringComparison.OrdinalIgnoreCase))
				{
					num6 = 3;
				}
				else if (array4[0].Equals("steps", StringComparison.OrdinalIgnoreCase))
				{
					num6 = 4;
				}
				else if (array4[0].Equals("joint", StringComparison.OrdinalIgnoreCase))
				{
					num6 = 5;
				}
				else if (array4[0].Equals("angle", StringComparison.OrdinalIgnoreCase))
				{
					num6 = 6;
				}
				else if (array4[0].Equals("pivot2", StringComparison.OrdinalIgnoreCase))
				{
					num6 = 7;
				}
				else if (array4[0].Equals("pivot", StringComparison.OrdinalIgnoreCase))
				{
					num6 = 8;
				}
				else if (array4[0].Equals("angleSpeed", StringComparison.OrdinalIgnoreCase))
				{
					num6 = 9;
				}
				else if (array4[0].Equals("pivotSpeed", StringComparison.OrdinalIgnoreCase))
				{
					num6 = 10;
				}
				else if (array4[0].Equals("pivot2Speed", StringComparison.OrdinalIgnoreCase))
				{
					num6 = 11;
				}
				else if (array4[0].Equals("xcoord", StringComparison.OrdinalIgnoreCase))
				{
					num6 = 12;
				}
				else if (array4[0].Equals("ycoord", StringComparison.OrdinalIgnoreCase))
				{
					num6 = 13;
				}
				else if (array4[0].Equals("zcoord", StringComparison.OrdinalIgnoreCase))
				{
					num6 = 14;
				}
				else if (array4[0].Equals("xspeed", StringComparison.OrdinalIgnoreCase))
				{
					num6 = 15;
				}
				else if (array4[0].Equals("yspeed", StringComparison.OrdinalIgnoreCase))
				{
					num6 = 16;
				}
				else if (array4[0].Equals("zspeed", StringComparison.OrdinalIgnoreCase))
				{
					num6 = 17;
				}
				else if (array4[0].Equals("loop", StringComparison.OrdinalIgnoreCase))
				{
					num6 = 18;
				}
				else if (array4[0].Equals("reverse", StringComparison.OrdinalIgnoreCase))
				{
					num6 = 19;
				}
				else if (array4[0].Equals("callbacktype", StringComparison.OrdinalIgnoreCase))
				{
					num6 = 20;
				}
				else if (array4[0].Equals("group", StringComparison.OrdinalIgnoreCase))
				{
					num6 = 21;
				}
				else if (array4[0].Equals("radians", StringComparison.OrdinalIgnoreCase))
				{
					num6 = 22;
				}
				else if (array4[0].Equals("speedFactor", StringComparison.OrdinalIgnoreCase))
				{
					num6 = 23;
				}
				else if (array4[0].Equals("StaysActive", StringComparison.OrdinalIgnoreCase))
				{
					num6 = 24;
				}
				else if (array4[0].Equals("File", StringComparison.OrdinalIgnoreCase))
				{
					num6 = 25;
				}
				else if (array4[0].Equals("num_animations", StringComparison.OrdinalIgnoreCase))
				{
					num6 = 26;
				}
				else if (array4[0].Equals("aFile", StringComparison.OrdinalIgnoreCase))
				{
					num6 = 27;
				}
				else if (array4[0].Equals("animation", StringComparison.OrdinalIgnoreCase))
				{
					num6 = 28;
				}
				else if (array4[0].Equals("actionFile", StringComparison.OrdinalIgnoreCase))
				{
					num6 = 29;
				}
				switch (num6)
				{
				case 1:
					if (array4.Length > 1)
					{
						num2 = int.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
						Create_Programs(num2, ref loadPg);
						pgC[pgcID].numPrograms = num2;
					}
					break;
				case 2:
					if (array4.Length > 1)
					{
						speedFactor = 1f;
						int k = int.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
						if (k < num2)
						{
							num = k;
							loadPg[num].numJoints = 0;
							loadPg[num].numSteps = 0;
							loadPg[num].loop = false;
							loadPg[num].staysActiveAtEnd = false;
							loadPg[num].reverse = false;
							loadPg[num].group = 1;
							curItem = -1;
							radians = 1f;
						}
					}
					break;
				case 25:
					if (array4.Length > 1)
					{
						Load_Program_File(array4[1], ref loadPg, (ushort)num);
					}
					break;
				case 26:
					if (array4.Length > 1)
					{
						num3 = int.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
						loadAnimation = new StructsClass.animation[num3];
						for (int k = 0; k < num3; k++)
						{
							loadAnimation[k].numAnimationSequences = 0;
							loadAnimation[k].numActions = 0;
							loadAnimation[k].loop = false;
							loadAnimation[k].networked = false;
							loadAnimation[k].staysActiveAtEnd = false;
							loadAnimation[k].group = 1;
							loadAnimation[k].directionAndSpeed = 1f;
						}
						pgC[pgcID].numAnimations = num3;
					}
					break;
				case 27:
					if (array4.Length > 1)
					{
						Load_Animation_File(array4[1], ref loadAnimation, (ushort)num);
					}
					break;
				case 28:
					if (array4.Length > 1)
					{
						int k = int.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
						if (k < num3)
						{
							num = k;
						}
					}
					break;
				case 29:
					if (array4.Length > 1)
					{
						Load_Animation_Action_File(array4[1], ref loadAnimation, (ushort)num);
					}
					break;
				default:
					Load_Program_Action(num6, (ushort)num, array4, ref loadPg, ref curItem, ref radians, ref speedFactor);
					break;
				}
			}
		}
		stream.Close();
	}

	public void Load_Program_File(string filename, ref StructsClass.program[] loadPg, ushort curProgram)
	{
		int curItem = -1;
		_ = global::Rendering.Rendering.uBufferID;
		float radians = 1f;
		float speedFactor = 1f;
		Stream stream = TitleContainer.OpenStream("The_CoOp_Zombie_Game\\Config_Files\\Programs\\" + filename);
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
				int action = 0;
				if (array4[0].Equals("joints", StringComparison.OrdinalIgnoreCase))
				{
					action = 3;
				}
				else if (array4[0].Equals("steps", StringComparison.OrdinalIgnoreCase))
				{
					action = 4;
				}
				else if (array4[0].Equals("joint", StringComparison.OrdinalIgnoreCase))
				{
					action = 5;
				}
				else if (array4[0].Equals("angle", StringComparison.OrdinalIgnoreCase))
				{
					action = 6;
				}
				else if (array4[0].Equals("pivot2", StringComparison.OrdinalIgnoreCase))
				{
					action = 7;
				}
				else if (array4[0].Equals("pivot", StringComparison.OrdinalIgnoreCase))
				{
					action = 8;
				}
				else if (array4[0].Equals("angleSpeed", StringComparison.OrdinalIgnoreCase))
				{
					action = 9;
				}
				else if (array4[0].Equals("pivotSpeed", StringComparison.OrdinalIgnoreCase))
				{
					action = 10;
				}
				else if (array4[0].Equals("pivot2Speed", StringComparison.OrdinalIgnoreCase))
				{
					action = 11;
				}
				else if (array4[0].Equals("xcoord", StringComparison.OrdinalIgnoreCase))
				{
					action = 12;
				}
				else if (array4[0].Equals("ycoord", StringComparison.OrdinalIgnoreCase))
				{
					action = 13;
				}
				else if (array4[0].Equals("zcoord", StringComparison.OrdinalIgnoreCase))
				{
					action = 14;
				}
				else if (array4[0].Equals("xspeed", StringComparison.OrdinalIgnoreCase))
				{
					action = 15;
				}
				else if (array4[0].Equals("yspeed", StringComparison.OrdinalIgnoreCase))
				{
					action = 16;
				}
				else if (array4[0].Equals("zspeed", StringComparison.OrdinalIgnoreCase))
				{
					action = 17;
				}
				else if (array4[0].Equals("loop", StringComparison.OrdinalIgnoreCase))
				{
					action = 18;
				}
				else if (array4[0].Equals("reverse", StringComparison.OrdinalIgnoreCase))
				{
					action = 19;
				}
				else if (array4[0].Equals("callbacktype", StringComparison.OrdinalIgnoreCase))
				{
					action = 20;
				}
				else if (array4[0].Equals("group", StringComparison.OrdinalIgnoreCase))
				{
					action = 21;
				}
				else if (array4[0].Equals("radians", StringComparison.OrdinalIgnoreCase))
				{
					action = 22;
				}
				else if (array4[0].Equals("speedFactor", StringComparison.OrdinalIgnoreCase))
				{
					action = 23;
				}
				else if (array4[0].Equals("StaysActive", StringComparison.OrdinalIgnoreCase))
				{
					action = 24;
				}
				Load_Program_Action(action, curProgram, array4, ref loadPg, ref curItem, ref radians, ref speedFactor);
			}
		}
		stream.Close();
	}

	public void Load_Program_Action(int action, ushort curProgram, string[] str4, ref StructsClass.program[] loadPg, ref int curItem, ref float radians, ref float speedFactor)
	{
		switch (action)
		{
		case 3:
			if (loadPg[curProgram].numJoints == 0 && str4.Length > 1)
			{
				loadPg[curProgram].numJoints = short.Parse(str4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
				if (loadPg[curProgram].numJoints > 0 && loadPg[curProgram].numSteps > 0)
				{
					Create_Program_Data(curProgram, ref loadPg);
				}
			}
			break;
		case 4:
			if (loadPg[curProgram].numSteps == 0 && str4.Length > 1)
			{
				loadPg[curProgram].numSteps = short.Parse(str4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
				if (loadPg[curProgram].numJoints > 0 && loadPg[curProgram].numSteps > 0)
				{
					Create_Program_Data(curProgram, ref loadPg);
				}
			}
			break;
		case 5:
			if (loadPg[curProgram].status == 1 && loadPg[curProgram].numJoints > 0 && loadPg[curProgram].numSteps > 0 && ++curItem > -1 && curItem < loadPg[curProgram].numJoints * loadPg[curProgram].numSteps && str4.Length > 1)
			{
				loadPg[curProgram].jt[curItem] = short.Parse(str4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
			}
			break;
		case 6:
			if (loadPg[curProgram].status == 1 && loadPg[curProgram].numJoints > 0 && loadPg[curProgram].numSteps > 0 && curItem > -1 && str4.Length > 1)
			{
				loadPg[curProgram].xRot[curItem] = float.Parse(str4[1], CultureInfo.InvariantCulture.NumberFormat) * radians;
			}
			break;
		case 7:
			if (loadPg[curProgram].status == 1 && loadPg[curProgram].numJoints > 0 && loadPg[curProgram].numSteps > 0 && curItem > -1 && str4.Length > 1)
			{
				loadPg[curProgram].yRot[curItem] = float.Parse(str4[1], CultureInfo.InvariantCulture.NumberFormat) * radians;
			}
			break;
		case 8:
			if (loadPg[curProgram].status == 1 && loadPg[curProgram].numJoints > 0 && loadPg[curProgram].numSteps > 0 && curItem > -1 && str4.Length > 1)
			{
				loadPg[curProgram].zRot[curItem] = float.Parse(str4[1], CultureInfo.InvariantCulture.NumberFormat) * radians;
			}
			break;
		case 9:
			if (loadPg[curProgram].status == 1 && loadPg[curProgram].numJoints > 0 && loadPg[curProgram].numSteps > 0 && curItem > -1 && str4.Length > 1)
			{
				loadPg[curProgram].rotXSpeed[curItem] = speedFactor * float.Parse(str4[1], CultureInfo.InvariantCulture.NumberFormat) * radians;
			}
			break;
		case 10:
			if (loadPg[curProgram].status == 1 && loadPg[curProgram].numJoints > 0 && loadPg[curProgram].numSteps > 0 && curItem > -1 && str4.Length > 1)
			{
				loadPg[curProgram].rotZSpeed[curItem] = speedFactor * float.Parse(str4[1], CultureInfo.InvariantCulture.NumberFormat) * radians;
			}
			break;
		case 11:
			if (loadPg[curProgram].status == 1 && loadPg[curProgram].numJoints > 0 && loadPg[curProgram].numSteps > 0 && curItem > -1 && str4.Length > 1)
			{
				loadPg[curProgram].rotYSpeed[curItem] = speedFactor * float.Parse(str4[1], CultureInfo.InvariantCulture.NumberFormat) * radians;
			}
			break;
		case 12:
			if (loadPg[curProgram].status == 1 && loadPg[curProgram].numJoints > 0 && loadPg[curProgram].numSteps > 0 && curItem > -1 && str4.Length > 1)
			{
				loadPg[curProgram].x[curItem] = float.Parse(str4[1], CultureInfo.InvariantCulture.NumberFormat);
			}
			break;
		case 13:
			if (loadPg[curProgram].status == 1 && loadPg[curProgram].numJoints > 0 && loadPg[curProgram].numSteps > 0 && curItem > -1 && str4.Length > 1)
			{
				loadPg[curProgram].y[curItem] = float.Parse(str4[1], CultureInfo.InvariantCulture.NumberFormat);
			}
			break;
		case 14:
			if (loadPg[curProgram].status == 1 && loadPg[curProgram].numJoints > 0 && loadPg[curProgram].numSteps > 0 && curItem > -1 && str4.Length > 1)
			{
				loadPg[curProgram].z[curItem] = float.Parse(str4[1], CultureInfo.InvariantCulture.NumberFormat);
			}
			break;
		case 15:
			if (loadPg[curProgram].status == 1 && loadPg[curProgram].numJoints > 0 && loadPg[curProgram].numSteps > 0 && curItem > -1 && str4.Length > 1)
			{
				loadPg[curProgram].xSpeed[curItem] = float.Parse(str4[1], CultureInfo.InvariantCulture.NumberFormat);
			}
			break;
		case 16:
			if (loadPg[curProgram].status == 1 && loadPg[curProgram].numJoints > 0 && loadPg[curProgram].numSteps > 0 && curItem > -1 && str4.Length > 1)
			{
				loadPg[curProgram].ySpeed[curItem] = float.Parse(str4[1], CultureInfo.InvariantCulture.NumberFormat);
			}
			break;
		case 17:
			if (loadPg[curProgram].status == 1 && loadPg[curProgram].numJoints > 0 && loadPg[curProgram].numSteps > 0 && curItem > -1 && str4.Length > 1)
			{
				loadPg[curProgram].zSpeed[curItem] = float.Parse(str4[1], CultureInfo.InvariantCulture.NumberFormat);
			}
			break;
		case 18:
			loadPg[curProgram].loop = true;
			break;
		case 19:
			loadPg[curProgram].reverse = true;
			break;
		case 20:
			if (str4.Length > 1)
			{
				loadPg[curProgram].callBackType = byte.Parse(str4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
			}
			break;
		case 21:
			if (str4.Length > 1)
			{
				loadPg[curProgram].group = byte.Parse(str4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
			}
			break;
		case 22:
			radians = 57.29578f;
			break;
		case 23:
			if (str4.Length > 1)
			{
				speedFactor = float.Parse(str4[1], CultureInfo.InvariantCulture.NumberFormat);
			}
			break;
		case 24:
			loadPg[curProgram].staysActiveAtEnd = true;
			break;
		}
	}

	public void Load_Programs_Basic(string fileName)
	{
		int num = 0;
		int num2 = 0;
		for (int i = 0; i < numAllocatedProgramsBasic; i++)
		{
			pgBasic[i].status = 0;
			pgBasic[i].resetOnMinorStart = false;
		}
		numProgramsBasic = 0;
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
				if (array4[0].Equals("num_programs", StringComparison.OrdinalIgnoreCase))
				{
					num5 = 1;
				}
				else if (array4[0].Equals("program", StringComparison.OrdinalIgnoreCase))
				{
					num5 = 2;
				}
				else if (array4[0].Equals("joints", StringComparison.OrdinalIgnoreCase))
				{
					num5 = 3;
				}
				else if (array4[0].Equals("steps", StringComparison.OrdinalIgnoreCase))
				{
					num5 = 4;
				}
				else if (array4[0].Equals("joint", StringComparison.OrdinalIgnoreCase))
				{
					num5 = 5;
				}
				else if (array4[0].Equals("angle", StringComparison.OrdinalIgnoreCase))
				{
					num5 = 6;
				}
				else if (array4[0].Equals("pivot2", StringComparison.OrdinalIgnoreCase))
				{
					num5 = 7;
				}
				else if (array4[0].Equals("pivot", StringComparison.OrdinalIgnoreCase))
				{
					num5 = 8;
				}
				else if (array4[0].Equals("angleSpeed", StringComparison.OrdinalIgnoreCase))
				{
					num5 = 9;
				}
				else if (array4[0].Equals("pivotSpeed", StringComparison.OrdinalIgnoreCase))
				{
					num5 = 10;
				}
				else if (array4[0].Equals("pivot2Speed", StringComparison.OrdinalIgnoreCase))
				{
					num5 = 11;
				}
				else if (array4[0].Equals("xcoordStart", StringComparison.OrdinalIgnoreCase))
				{
					num5 = 12;
				}
				else if (array4[0].Equals("ycoordStart", StringComparison.OrdinalIgnoreCase))
				{
					num5 = 13;
				}
				else if (array4[0].Equals("zcoordStart", StringComparison.OrdinalIgnoreCase))
				{
					num5 = 14;
				}
				else if (array4[0].Equals("xspeedForward", StringComparison.OrdinalIgnoreCase))
				{
					num5 = 15;
				}
				else if (array4[0].Equals("yspeedForward", StringComparison.OrdinalIgnoreCase))
				{
					num5 = 16;
				}
				else if (array4[0].Equals("zspeedForward", StringComparison.OrdinalIgnoreCase))
				{
					num5 = 17;
				}
				else if (array4[0].Equals("xcoordEnd", StringComparison.OrdinalIgnoreCase))
				{
					num5 = 18;
				}
				else if (array4[0].Equals("ycoordEnd", StringComparison.OrdinalIgnoreCase))
				{
					num5 = 19;
				}
				else if (array4[0].Equals("zcoordEnd", StringComparison.OrdinalIgnoreCase))
				{
					num5 = 20;
				}
				else if (array4[0].Equals("xspeedReverse", StringComparison.OrdinalIgnoreCase))
				{
					num5 = 21;
				}
				else if (array4[0].Equals("yspeedReverse", StringComparison.OrdinalIgnoreCase))
				{
					num5 = 22;
				}
				else if (array4[0].Equals("zspeedReverse", StringComparison.OrdinalIgnoreCase))
				{
					num5 = 23;
				}
				else if (array4[0].Equals("loop", StringComparison.OrdinalIgnoreCase))
				{
					num5 = 24;
				}
				else if (array4[0].Equals("reverse", StringComparison.OrdinalIgnoreCase))
				{
					num5 = 25;
				}
				else if (array4[0].Equals("timeStart", StringComparison.OrdinalIgnoreCase))
				{
					num5 = 26;
				}
				else if (array4[0].Equals("timeEnd", StringComparison.OrdinalIgnoreCase))
				{
					num5 = 27;
				}
				else if (array4[0].Equals("group", StringComparison.OrdinalIgnoreCase))
				{
					num5 = 28;
				}
				else if (array4[0].Equals("resetOnMinorRestart", StringComparison.OrdinalIgnoreCase))
				{
					num5 = 29;
				}
				switch (num5)
				{
				case 1:
				{
					if (array4.Length <= 1)
					{
						break;
					}
					int num6 = int.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
					if (num6 > numAllocatedProgramsBasic)
					{
						pgBasic = new StructsClass.program[num6];
						for (int i = 0; i < num6; i++)
						{
							pgBasic[i] = default(StructsClass.program);
							pgBasic[i].status = 0;
							pgBasic[i].numJoints = 0;
							pgBasic[i].numSteps = 0;
							pgBasic[i].curStep = -1;
							pgBasic[i].loop = false;
							pgBasic[i].reverse = false;
							pgBasic[i].group = 1;
							pgBasic[i].resetOnMinorStart = false;
						}
						numAllocatedProgramsBasic = (short)num6;
					}
					numProgramsBasic = (short)num6;
					break;
				}
				case 2:
					if (array4.Length > 1)
					{
						int i = int.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
						if (i < numProgramsBasic && i > -1)
						{
							num = i;
							num2 = -1;
							pgBasic[num].numJoints = 0;
							pgBasic[num].numSteps = 0;
						}
					}
					break;
				case 3:
					if (pgBasic[num].numJoints == 0 && array4.Length > 1)
					{
						pgBasic[num].numJoints = short.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
						if (pgBasic[num].numJoints > 0 && pgBasic[num].numSteps > 0)
						{
							Create_Program_Data_Basic(num);
						}
					}
					break;
				case 4:
					if (pgBasic[num].numSteps == 0 && array4.Length > 1)
					{
						pgBasic[num].numSteps = short.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
						if (pgBasic[num].numJoints > 0 && pgBasic[num].numSteps > 0)
						{
							Create_Program_Data_Basic(num);
						}
					}
					break;
				case 5:
					if (pgBasic[num].status == 1 && pgBasic[num].numJoints > 0 && pgBasic[num].numSteps > 0 && ++num2 > -1 && num2 < pgBasic[num].numJoints * pgBasic[num].numSteps && array4.Length > 1)
					{
						pgBasic[num].jt[num2] = short.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 6:
					if (pgBasic[num].status == 1 && pgBasic[num].numJoints > 0 && pgBasic[num].numSteps > 0 && num2 > -1 && array4.Length > 1)
					{
						pgBasic[num].xRot[num2] = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 7:
					if (pgBasic[num].status == 1 && pgBasic[num].numJoints > 0 && pgBasic[num].numSteps > 0 && num2 > -1 && array4.Length > 1)
					{
						pgBasic[num].yRot[num2] = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 8:
					if (pgBasic[num].status == 1 && pgBasic[num].numJoints > 0 && pgBasic[num].numSteps > 0 && num2 > -1 && array4.Length > 1)
					{
						pgBasic[num].zRot[num2] = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 9:
					if (pgBasic[num].status == 1 && pgBasic[num].numJoints > 0 && pgBasic[num].numSteps > 0 && num2 > -1 && array4.Length > 1)
					{
						pgBasic[num].rotXSpeed[num2] = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 10:
					if (pgBasic[num].status == 1 && pgBasic[num].numJoints > 0 && pgBasic[num].numSteps > 0 && num2 > -1 && array4.Length > 1)
					{
						pgBasic[num].rotZSpeed[num2] = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 11:
					if (pgBasic[num].status == 1 && pgBasic[num].numJoints > 0 && pgBasic[num].numSteps > 0 && num2 > -1 && array4.Length > 1)
					{
						pgBasic[num].rotYSpeed[num2] = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 12:
					if (pgBasic[num].status == 1 && pgBasic[num].numJoints > 0 && pgBasic[num].numSteps > 0 && num2 > -1 && array4.Length > 1)
					{
						pgBasic[num].x[num2] = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 13:
					if (pgBasic[num].status == 1 && pgBasic[num].numJoints > 0 && pgBasic[num].numSteps > 0 && num2 > -1 && array4.Length > 1)
					{
						pgBasic[num].y[num2] = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 14:
					if (pgBasic[num].status == 1 && pgBasic[num].numJoints > 0 && pgBasic[num].numSteps > 0 && num2 > -1 && array4.Length > 1)
					{
						pgBasic[num].z[num2] = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 15:
					if (pgBasic[num].status == 1 && pgBasic[num].numJoints > 0 && pgBasic[num].numSteps > 0 && num2 > -1 && array4.Length > 1)
					{
						pgBasic[num].xSpeed[num2] = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 16:
					if (pgBasic[num].status == 1 && pgBasic[num].numJoints > 0 && pgBasic[num].numSteps > 0 && num2 > -1 && array4.Length > 1)
					{
						pgBasic[num].ySpeed[num2] = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 17:
					if (pgBasic[num].status == 1 && pgBasic[num].numJoints > 0 && pgBasic[num].numSteps > 0 && num2 > -1 && array4.Length > 1)
					{
						pgBasic[num].zSpeed[num2] = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 18:
					if (pgBasic[num].status == 1 && pgBasic[num].numJoints > 0 && pgBasic[num].numSteps > 0 && num2 > -1 && array4.Length > 1)
					{
						pgBasic[num].x2[num2] = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 19:
					if (pgBasic[num].status == 1 && pgBasic[num].numJoints > 0 && pgBasic[num].numSteps > 0 && num2 > -1 && array4.Length > 1)
					{
						pgBasic[num].y2[num2] = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 20:
					if (pgBasic[num].status == 1 && pgBasic[num].numJoints > 0 && pgBasic[num].numSteps > 0 && num2 > -1 && array4.Length > 1)
					{
						pgBasic[num].z2[num2] = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 21:
					if (pgBasic[num].status == 1 && pgBasic[num].numJoints > 0 && pgBasic[num].numSteps > 0 && num2 > -1 && array4.Length > 1)
					{
						pgBasic[num].xSpeed2[num2] = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 22:
					if (pgBasic[num].status == 1 && pgBasic[num].numJoints > 0 && pgBasic[num].numSteps > 0 && num2 > -1 && array4.Length > 1)
					{
						pgBasic[num].ySpeed2[num2] = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 23:
					if (pgBasic[num].status == 1 && pgBasic[num].numJoints > 0 && pgBasic[num].numSteps > 0 && num2 > -1 && array4.Length > 1)
					{
						pgBasic[num].zSpeed2[num2] = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 24:
					if (array4.Length > 1 && int.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat) > 0)
					{
						pgBasic[num].loop = true;
					}
					break;
				case 25:
					if (array4.Length > 1 && int.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat) > 0)
					{
						pgBasic[num].reverse = true;
					}
					break;
				case 26:
					if (pgBasic[num].status == 1 && pgBasic[num].numJoints > 0 && pgBasic[num].numSteps > 0 && num2 > -1 && array4.Length > 1)
					{
						pgBasic[num].time[num2] = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 27:
					if (pgBasic[num].status == 1 && pgBasic[num].numJoints > 0 && pgBasic[num].numSteps > 0 && num2 > -1 && array4.Length > 1)
					{
						pgBasic[num].time2[num2] = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 28:
					if (array4.Length > 1)
					{
						pgBasic[num].group = byte.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 29:
					pgBasic[num].resetOnMinorStart = true;
					break;
				}
			}
		}
		stream.Close();
	}

	public void Create_Program_Data(long pgID, ref StructsClass.program[] loadPg)
	{
		if (pgID < 0)
		{
			return;
		}
		short numJoints = loadPg[pgID].numJoints;
		short numSteps = loadPg[pgID].numSteps;
		short num = (short)(numJoints * numSteps);
		if (num < 1)
		{
			loadPg[pgID].numJoints = 0;
			loadPg[pgID].numSteps = 0;
			return;
		}
		loadPg[pgID].jt = new short[num];
		loadPg[pgID].xRot = new float[num];
		loadPg[pgID].yRot = new float[num];
		loadPg[pgID].zRot = new float[num];
		loadPg[pgID].rotXSpeed = new float[num];
		loadPg[pgID].rotZSpeed = new float[num];
		loadPg[pgID].rotYSpeed = new float[num];
		loadPg[pgID].x = new float[num];
		loadPg[pgID].y = new float[num];
		loadPg[pgID].z = new float[num];
		loadPg[pgID].xSpeed = new float[num];
		loadPg[pgID].ySpeed = new float[num];
		loadPg[pgID].zSpeed = new float[num];
		short num2 = 0;
		short num3 = 0;
		while (num2 < numJoints * numSteps)
		{
			loadPg[pgID].jt[num2] = num3++;
			loadPg[pgID].xRot[num2] = 0f;
			loadPg[pgID].zRot[num2] = 0f;
			loadPg[pgID].yRot[num2] = 0f;
			loadPg[pgID].rotXSpeed[num2] = 200f;
			loadPg[pgID].rotZSpeed[num2] = 200f;
			loadPg[pgID].rotYSpeed[num2] = 200f;
			loadPg[pgID].x[num2] = 0f;
			loadPg[pgID].y[num2] = 0f;
			loadPg[pgID].z[num2] = 0f;
			loadPg[pgID].xSpeed[num2] = 0f;
			loadPg[pgID].ySpeed[num2] = 0f;
			loadPg[pgID].zSpeed[num2] = 0f;
			if (num3 >= numJoints)
			{
				num3 = 0;
			}
			num2++;
		}
		loadPg[pgID].status = 1;
	}

	public void Create_Program_Data_Basic(long pgID)
	{
		if (pgID >= numProgramsBasic || pgID < 0)
		{
			return;
		}
		short numJoints = pgBasic[pgID].numJoints;
		short numSteps = pgBasic[pgID].numSteps;
		short num = (short)(numJoints * numSteps);
		if (num < 1)
		{
			pgBasic[pgID].numJoints = 0;
			pgBasic[pgID].numSteps = 0;
			return;
		}
		pgBasic[pgID].jt = new short[num];
		pgBasic[pgID].xRot = new float[num];
		pgBasic[pgID].yRot = new float[num];
		pgBasic[pgID].zRot = new float[num];
		pgBasic[pgID].rotXSpeed = new float[num];
		pgBasic[pgID].rotZSpeed = new float[num];
		pgBasic[pgID].rotYSpeed = new float[num];
		pgBasic[pgID].time = new float[num];
		pgBasic[pgID].x = new float[num];
		pgBasic[pgID].y = new float[num];
		pgBasic[pgID].z = new float[num];
		pgBasic[pgID].time2 = new float[num];
		pgBasic[pgID].x2 = new float[num];
		pgBasic[pgID].y2 = new float[num];
		pgBasic[pgID].z2 = new float[num];
		pgBasic[pgID].xSpeed = new float[num];
		pgBasic[pgID].ySpeed = new float[num];
		pgBasic[pgID].zSpeed = new float[num];
		pgBasic[pgID].xSpeed2 = new float[num];
		pgBasic[pgID].ySpeed2 = new float[num];
		pgBasic[pgID].zSpeed2 = new float[num];
		short num2 = 0;
		short num3 = 0;
		while (num2 < num)
		{
			pgBasic[pgID].jt[num2] = num3++;
			pgBasic[pgID].xRot[num2] = 0f;
			pgBasic[pgID].zRot[num2] = 0f;
			pgBasic[pgID].yRot[num2] = 0f;
			pgBasic[pgID].rotXSpeed[num2] = 200f;
			pgBasic[pgID].rotZSpeed[num2] = 200f;
			pgBasic[pgID].rotYSpeed[num2] = 200f;
			pgBasic[pgID].time[num2] = 0f;
			pgBasic[pgID].x[num2] = 0f;
			pgBasic[pgID].y[num2] = 0f;
			pgBasic[pgID].z[num2] = 0f;
			pgBasic[pgID].time2[num2] = 0f;
			pgBasic[pgID].x2[num2] = 0f;
			pgBasic[pgID].y2[num2] = 0f;
			pgBasic[pgID].z2[num2] = 0f;
			pgBasic[pgID].xSpeed[num2] = 0f;
			pgBasic[pgID].ySpeed[num2] = 0f;
			pgBasic[pgID].zSpeed[num2] = 0f;
			pgBasic[pgID].xSpeed2[num2] = 0f;
			pgBasic[pgID].ySpeed2[num2] = 0f;
			pgBasic[pgID].zSpeed2[num2] = 0f;
			if (num3 >= numJoints)
			{
				num3 = 0;
			}
			num2++;
		}
		pgBasic[pgID].status = 1;
	}

	public void Load_Animation_File(string filename, ref StructsClass.animation[] animation, ushort animationID)
	{
		int num = -1;
		ushort num2 = 0;
		_ = global::Rendering.Rendering.uBufferID;
		Stream stream = TitleContainer.OpenStream("The_CoOp_Zombie_Game\\Config_Files\\Programs\\" + filename);
		byte[] array = new byte[stream.Length];
		if (stream.CanRead)
		{
			stream.Read(array, 0, array.Length);
			string text = mainC.utilMain.Byte_Array_To_String(array);
			string[] array2 = text.Split(new char[2] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
			int num3 = array2.Length;
			if (num3 < 1)
			{
				stream.Close();
				return;
			}
			for (int i = 0; i < num3; i++)
			{
				string[] array3 = array2[i].Split(new char[2] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
				if (array3.Length < 1)
				{
					continue;
				}
				int num4 = 0;
				if (array3[0].Equals("joints", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 1;
				}
				else if (array3[0].Equals("length", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 2;
				}
				else if (array3[0].Equals("group", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 3;
				}
				else if (array3[0].Equals("loop", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 4;
				}
				else if (array3[0].Equals("callback", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 5;
				}
				else if (array3[0].Equals("callbacktype", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 6;
				}
				else if (array3[0].Equals("speedFactor", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 7;
				}
				else if (array3[0].Equals("StaysActive", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 8;
				}
				else if (array3[0].Equals("joint", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 9;
				}
				else if (array3[0].Equals("steps", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 10;
				}
				else if (array3[0].Equals("rot", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 11;
				}
				else if (array3[0].Equals("reverse", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 12;
				}
				else if (array3[0].Equals("cancelledCallback", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 13;
				}
				else if (array3[0].Equals("cancelledCallbacktype", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 14;
				}
				else if (array3[0].Equals("matrix", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 15;
				}
				else if (array3[0].Equals("networked", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 16;
				}
				switch (num4)
				{
				case 1:
					if (array3.Length > 1)
					{
						ushort num5 = ushort.Parse(array3[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
						animation[animationID].numAnimationSequences = num5;
						animation[animationID].animation_sequences = new StructsClass.animation_sequence[num5];
					}
					break;
				case 2:
					if (array3.Length > 1)
					{
						animation[animationID].length = float.Parse(array3[1], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 3:
					if (array3.Length > 1)
					{
						animation[animationID].group = byte.Parse(array3[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 4:
					animation[animationID].loop = true;
					break;
				case 5:
					if (array3.Length > 1)
					{
						animation[animationID].callBack = byte.Parse(array3[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 6:
					if (array3.Length > 1)
					{
						animation[animationID].callBackType = byte.Parse(array3[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 7:
					if (array3.Length > 1)
					{
						animation[animationID].directionAndSpeed = float.Parse(array3[1], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 8:
					animation[animationID].staysActiveAtEnd = true;
					break;
				case 9:
					if (array3.Length > 1)
					{
						num++;
						if (num < animation[animationID].numAnimationSequences)
						{
							animation[animationID].animation_sequences[num].jointID = ushort.Parse(array3[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
						}
						else
						{
							num = -1;
						}
					}
					break;
				case 10:
					if (num > -1 && array3.Length > 1)
					{
						ushort num5 = ushort.Parse(array3[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
						if (num5 > 0)
						{
							animation[animationID].animation_sequences[num].numAnimationFrames = num5;
							animation[animationID].animation_sequences[num].animation_frames = new StructsClass.animation_rotation_frame[num5];
							num2 = 0;
						}
						else
						{
							num--;
						}
					}
					break;
				case 11:
					if (array3.Length > 4 && num > -1 && num2 < animation[animationID].animation_sequences[num].numAnimationFrames)
					{
						animation[animationID].animation_sequences[num].animation_frames[num2].xRot = float.Parse(array3[1], CultureInfo.InvariantCulture.NumberFormat);
						animation[animationID].animation_sequences[num].animation_frames[num2].yRot = float.Parse(array3[2], CultureInfo.InvariantCulture.NumberFormat);
						animation[animationID].animation_sequences[num].animation_frames[num2].zRot = float.Parse(array3[3], CultureInfo.InvariantCulture.NumberFormat);
						animation[animationID].animation_sequences[num].animation_frames[num2].mv = Matrix.CreateRotationY(animation[animationID].animation_sequences[num].animation_frames[num2].yRot * ((float)Math.PI / 180f)) * Matrix.CreateRotationX(animation[animationID].animation_sequences[num].animation_frames[num2].xRot * ((float)Math.PI / 180f)) * Matrix.CreateRotationZ(animation[animationID].animation_sequences[num].animation_frames[num2].zRot * ((float)Math.PI / 180f));
						animation[animationID].animation_sequences[num].animation_frames[num2].time = float.Parse(array3[4], CultureInfo.InvariantCulture.NumberFormat);
						num2++;
					}
					break;
				case 12:
					animation[animationID].directionAndSpeed = -1f;
					break;
				case 13:
					if (array3.Length > 1)
					{
						animation[animationID].cancelledCallBack = byte.Parse(array3[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 14:
					if (array3.Length > 1)
					{
						animation[animationID].cancelledCallBackType = byte.Parse(array3[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 15:
					if (array3.Length > 13 && num > -1 && num2 < animation[animationID].animation_sequences[num].numAnimationFrames)
					{
						animation[animationID].animation_sequences[num].animation_frames[num2].mv = Matrix.CreateTranslation(float.Parse(array3[10], CultureInfo.InvariantCulture.NumberFormat), float.Parse(array3[11], CultureInfo.InvariantCulture.NumberFormat), float.Parse(array3[12], CultureInfo.InvariantCulture.NumberFormat));
						animation[animationID].animation_sequences[num].animation_frames[num2].mv.M11 = float.Parse(array3[1], CultureInfo.InvariantCulture.NumberFormat);
						animation[animationID].animation_sequences[num].animation_frames[num2].mv.M12 = float.Parse(array3[2], CultureInfo.InvariantCulture.NumberFormat);
						animation[animationID].animation_sequences[num].animation_frames[num2].mv.M13 = float.Parse(array3[3], CultureInfo.InvariantCulture.NumberFormat);
						animation[animationID].animation_sequences[num].animation_frames[num2].mv.M21 = float.Parse(array3[4], CultureInfo.InvariantCulture.NumberFormat);
						animation[animationID].animation_sequences[num].animation_frames[num2].mv.M22 = float.Parse(array3[5], CultureInfo.InvariantCulture.NumberFormat);
						animation[animationID].animation_sequences[num].animation_frames[num2].mv.M23 = float.Parse(array3[6], CultureInfo.InvariantCulture.NumberFormat);
						animation[animationID].animation_sequences[num].animation_frames[num2].mv.M31 = float.Parse(array3[7], CultureInfo.InvariantCulture.NumberFormat);
						animation[animationID].animation_sequences[num].animation_frames[num2].mv.M32 = float.Parse(array3[8], CultureInfo.InvariantCulture.NumberFormat);
						animation[animationID].animation_sequences[num].animation_frames[num2].mv.M33 = float.Parse(array3[9], CultureInfo.InvariantCulture.NumberFormat);
						animation[animationID].animation_sequences[num].animation_frames[num2].time = float.Parse(array3[13], CultureInfo.InvariantCulture.NumberFormat);
						num2++;
					}
					break;
				case 16:
					animation[animationID].networked = true;
					break;
				}
			}
		}
		stream.Close();
	}

	public void Load_Animation_Action_File(string filename, ref StructsClass.animation[] animation, ushort animationID)
	{
		int num = -1;
		_ = global::Rendering.Rendering.uBufferID;
		Stream stream = TitleContainer.OpenStream("The_CoOp_Zombie_Game\\Config_Files\\Programs\\" + filename);
		byte[] array = new byte[stream.Length];
		if (stream.CanRead)
		{
			stream.Read(array, 0, array.Length);
			string text = mainC.utilMain.Byte_Array_To_String(array);
			string[] array2 = text.Split(new char[2] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
			int num2 = array2.Length;
			if (num2 < 1)
			{
				stream.Close();
				return;
			}
			for (int i = 0; i < num2; i++)
			{
				string[] array3 = array2[i].Split(new char[2] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
				if (array3.Length < 1)
				{
					continue;
				}
				int num3 = 0;
				if (array3[0].Equals("numActions", StringComparison.OrdinalIgnoreCase))
				{
					num3 = 1;
				}
				else if (array3[0].Equals("action", StringComparison.OrdinalIgnoreCase))
				{
					num3 = 2;
				}
				else if (array3[0].Equals("type", StringComparison.OrdinalIgnoreCase))
				{
					num3 = 3;
				}
				else if (array3[0].Equals("id", StringComparison.OrdinalIgnoreCase))
				{
					num3 = 4;
				}
				else if (array3[0].Equals("direction", StringComparison.OrdinalIgnoreCase))
				{
					num3 = 5;
				}
				else if (array3[0].Equals("time", StringComparison.OrdinalIgnoreCase))
				{
					num3 = 6;
				}
				else if (array3[0].Equals("var1", StringComparison.OrdinalIgnoreCase))
				{
					num3 = 7;
				}
				else if (array3[0].Equals("var2", StringComparison.OrdinalIgnoreCase))
				{
					num3 = 8;
				}
				switch (num3)
				{
				case 1:
					if (array3.Length > 1)
					{
						ushort num4 = ushort.Parse(array3[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
						animation[animationID].numActions = num4;
						animation[animationID].actions = new StructsClass.animation_action[num4];
						for (ushort num5 = 0; num5 < num4; num5++)
						{
							animation[animationID].actions[num5].type = 0;
							animation[animationID].actions[num5].actionID = 0;
							animation[animationID].actions[num5].direction = 1;
							animation[animationID].actions[num5].var1 = 0;
							animation[animationID].actions[num5].var2 = 0;
						}
					}
					break;
				case 2:
					if (array3.Length > 1)
					{
						num = int.Parse(array3[1], CultureInfo.InvariantCulture.NumberFormat);
						if (num >= animation[animationID].numActions)
						{
							num = -1;
						}
					}
					break;
				case 3:
					if (array3.Length > 1)
					{
						animation[animationID].actions[num].type = byte.Parse(array3[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 4:
					if (array3.Length > 1)
					{
						animation[animationID].actions[num].actionID = byte.Parse(array3[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 5:
					if (array3.Length > 1)
					{
						animation[animationID].actions[num].direction = sbyte.Parse(array3[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 6:
					if (array3.Length > 1)
					{
						animation[animationID].actions[num].time = float.Parse(array3[1], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 7:
					if (array3.Length > 1)
					{
						animation[animationID].actions[num].var1 = ushort.Parse(array3[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 8:
					if (array3.Length > 1)
					{
						animation[animationID].actions[num].var2 = ushort.Parse(array3[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				}
			}
		}
		stream.Close();
	}

	public void Run_Program(byte playerID, ref StructsClass.joint[] jt1, ref StructsClass.program_instance[] pg1, int pcID, int pgID, bool programStarting)
	{
		short curStep = pg1[pgID].curStep;
		pg1[pgID].curStep++;
		if (pg1[pgID].inReverse)
		{
			pg1[pgID].curStep -= 2;
			if (pg1[pgID].curStep < 0)
			{
				if (!programStarting && !pgC[pcID].pg1[pgID].loop)
				{
					pg1[pgID].status = 1;
					pg1[pgID].curStep = 0;
					pg1[pgID].inReverse = false;
					mainC.callbackMain.CallBack(playerID, pg1[pgID].callBackType, pg1[pgID].callBack, 0, 0);
					return;
				}
				pg1[pgID].curStep = 0;
			}
		}
		else if (pg1[pgID].curStep >= pgC[pcID].pg1[pgID].numSteps)
		{
			if (!programStarting && pg1[pgID].staysActiveAtEnd)
			{
				pg1[pgID].curStep = (short)(pgC[pcID].pg1[pgID].numSteps - 1);
				return;
			}
			if (!programStarting && !pgC[pcID].pg1[pgID].loop)
			{
				pg1[pgID].status = 1;
				pg1[pgID].curStep--;
				mainC.callbackMain.CallBack(playerID, pg1[pgID].callBackType, pg1[pgID].callBack, 0, 0);
				return;
			}
			if (!pg1[pgID].reverse)
			{
				pg1[pgID].curStep = 0;
			}
			else
			{
				pg1[pgID].curStep = (short)(pgC[pcID].pg1[pgID].numSteps - 1);
			}
		}
		int numJoints = pgC[pcID].pg1[pgID].numJoints;
		int num = pg1[pgID].curStep * numJoints;
		int num2 = curStep * numJoints;
		try
		{
			float num3 = 0.0167f;
			int i;
			for (i = 0; i < numJoints; i++)
			{
				int num4 = num + i;
				int num5 = num2 + i;
				if (pgC[pcID].pg1[pgID].xRot[num4] != pgC[pcID].pg1[pgID].xRot[num5])
				{
					num3 = Math.Abs(pgC[pcID].pg1[pgID].xRot[num + i] - pgC[pcID].pg1[pgID].xRot[num2 + i]);
					if (num3 > 180f)
					{
						num3 = Math.Abs(num3 - 360f);
					}
					num3 = (pg1[pgID].inReverse ? (num3 / pgC[pcID].pg1[pgID].rotXSpeed[num2 + i]) : (num3 / pgC[pcID].pg1[pgID].rotXSpeed[num + i]));
					break;
				}
				if (pgC[pcID].pg1[pgID].zRot[num4] != pgC[pcID].pg1[pgID].zRot[num5])
				{
					num3 = Math.Abs(pgC[pcID].pg1[pgID].zRot[num + i] - pgC[pcID].pg1[pgID].zRot[num2 + i]);
					if (num3 > 180f)
					{
						num3 = Math.Abs(num3 - 360f);
					}
					num3 = (pg1[pgID].inReverse ? (num3 / pgC[pcID].pg1[pgID].rotZSpeed[num2 + i]) : (num3 / pgC[pcID].pg1[pgID].rotZSpeed[num + i]));
					break;
				}
				if (pgC[pcID].pg1[pgID].yRot[num4] != pgC[pcID].pg1[pgID].yRot[num5])
				{
					num3 = Math.Abs(pgC[pcID].pg1[pgID].yRot[num + i] - pgC[pcID].pg1[pgID].yRot[num2 + i]);
					if (num3 > 180f)
					{
						num3 = Math.Abs(num3 - 360f);
					}
					num3 = (pg1[pgID].inReverse ? (num3 / pgC[pcID].pg1[pgID].rotYSpeed[num2 + i]) : (num3 / pgC[pcID].pg1[pgID].rotYSpeed[num + i]));
					break;
				}
			}
			pg1[pgID].timingJoint = 0;
			if (i < numJoints)
			{
				pg1[pgID].timingJoint = (ushort)i;
			}
			pg1[pgID].stepTime = num3;
			for (i = 0; i < numJoints; i++)
			{
				int num4 = num + i;
				int num5 = num2 + i;
				long num6 = pgC[pcID].pg1[pgID].jt[num4];
				jt1[num6].targetAngle = pgC[pcID].pg1[pgID].xRot[num4];
				jt1[num6].targetPivot = pgC[pcID].pg1[pgID].zRot[num4];
				jt1[num6].targetPivot2 = pgC[pcID].pg1[pgID].yRot[num4];
				jt1[num6].angleSpeed = Math.Abs((jt1[num6].targetAngle - jt1[num6].rotX) / num3);
				jt1[num6].pivotSpeed = Math.Abs((jt1[num6].targetPivot - jt1[num6].rotZ) / num3);
				jt1[num6].pivot2Speed = Math.Abs((jt1[num6].targetPivot2 - jt1[num6].rotY) / num3);
			}
		}
		catch
		{
		}
	}

	public void Run_Program_Basic(int pgID, bool toggleDirection, byte callBackType, byte callBack)
	{
		pgBasic[pgID].callBackType = callBackType;
		pgBasic[pgID].callBack = callBack;
		if (pgBasic[pgID].status == 1)
		{
			pgBasic[pgID].inReverse = false;
			pgBasic[pgID].status = 2;
			pgBasic[pgID].curStep = -1;
		}
		if (!toggleDirection)
		{
			if (!pgBasic[pgID].inReverse)
			{
				pgBasic[pgID].curStep++;
				if (pgBasic[pgID].curStep >= pgBasic[pgID].numSteps)
				{
					if (!pgBasic[pgID].loop)
					{
						pgBasic[pgID].curStep = (short)(pgBasic[pgID].numSteps - 1);
						pgBasic[pgID].status = 3;
						mainC.callbackMain.CallBack(0, pgBasic[pgID].callBackType, pgBasic[pgID].callBack, 0, 0);
						return;
					}
					if (pgBasic[pgID].reverse)
					{
						pgBasic[pgID].curStep--;
						pgBasic[pgID].inReverse = true;
					}
					else
					{
						pgBasic[pgID].curStep = 0;
					}
				}
			}
			else
			{
				pgBasic[pgID].curStep--;
				if (pgBasic[pgID].curStep < 0)
				{
					if (!pgBasic[pgID].loop)
					{
						pgBasic[pgID].curStep = 0;
						pgBasic[pgID].status = 3;
						mainC.callbackMain.CallBack(0, pgBasic[pgID].callBackType, pgBasic[pgID].callBack, 0, 0);
						return;
					}
					if (pgBasic[pgID].reverse)
					{
						pgBasic[pgID].curStep = 0;
						pgBasic[pgID].inReverse = false;
					}
					else
					{
						pgBasic[pgID].curStep = (short)(pgBasic[pgID].numSteps - 1);
						pgBasic[pgID].inReverse = true;
					}
				}
			}
		}
		else
		{
			if (!pgBasic[pgID].reverse)
			{
				return;
			}
			if (pgBasic[pgID].inReverse)
			{
				pgBasic[pgID].inReverse = false;
			}
			else
			{
				pgBasic[pgID].inReverse = true;
			}
			pgBasic[pgID].status = 2;
		}
		int numJoints = pgBasic[pgID].numJoints;
		int curStep = pgBasic[pgID].curStep;
		int num = curStep * numJoints;
		if (!pgBasic[pgID].inReverse)
		{
			for (int i = 0; i < numJoints; i++)
			{
				long num2 = pgBasic[pgID].jt[num + i];
				global::Joints.Joints.jtb1[num2].targetAngle = pgBasic[pgID].xRot[num + i];
				global::Joints.Joints.jtb1[num2].targetPivot2 = pgBasic[pgID].yRot[num + i];
				global::Joints.Joints.jtb1[num2].targetPivot = pgBasic[pgID].zRot[num + i];
				global::Joints.Joints.jtb1[num2].angleSpeed = pgBasic[pgID].rotXSpeed[num + i];
				global::Joints.Joints.jtb1[num2].pivotSpeed = pgBasic[pgID].rotZSpeed[num + i];
				global::Joints.Joints.jtb1[num2].pivot2Speed = pgBasic[pgID].rotYSpeed[num + i];
				global::Joints.Joints.jtb1[num2].time = pgBasic[pgID].time2[num + i];
				global::Joints.Joints.jtb1[num2].targetX = pgBasic[pgID].x2[num + i];
				global::Joints.Joints.jtb1[num2].targetY = pgBasic[pgID].y2[num + i];
				global::Joints.Joints.jtb1[num2].targetZ = pgBasic[pgID].z2[num + i];
				global::Joints.Joints.jtb1[num2].xSpeed = pgBasic[pgID].xSpeed[num + i];
				global::Joints.Joints.jtb1[num2].ySpeed = pgBasic[pgID].ySpeed[num + i];
				global::Joints.Joints.jtb1[num2].zSpeed = pgBasic[pgID].zSpeed[num + i];
				if (global::Joints.Joints.jtb1[num2].targetAngle > global::Joints.Joints.jtb1[num2].maxAngle)
				{
					global::Joints.Joints.jtb1[num2].targetAngle = global::Joints.Joints.jtb1[num2].maxAngle;
				}
				if (global::Joints.Joints.jtb1[num2].targetAngle < global::Joints.Joints.jtb1[num2].minAngle)
				{
					global::Joints.Joints.jtb1[num2].targetAngle = global::Joints.Joints.jtb1[num2].minAngle;
				}
				if (global::Joints.Joints.jtb1[num2].targetPivot2 > global::Joints.Joints.jtb1[num2].maxPivot2)
				{
					global::Joints.Joints.jtb1[num2].targetPivot2 = global::Joints.Joints.jtb1[num2].maxPivot2;
				}
				if (global::Joints.Joints.jtb1[num2].targetPivot2 < global::Joints.Joints.jtb1[num2].minPivot2)
				{
					global::Joints.Joints.jtb1[num2].targetPivot2 = global::Joints.Joints.jtb1[num2].minPivot2;
				}
				if (global::Joints.Joints.jtb1[num2].targetPivot > global::Joints.Joints.jtb1[num2].maxPivot)
				{
					global::Joints.Joints.jtb1[num2].targetPivot = global::Joints.Joints.jtb1[num2].maxPivot;
				}
				if (global::Joints.Joints.jtb1[num2].targetPivot < global::Joints.Joints.jtb1[num2].minPivot)
				{
					global::Joints.Joints.jtb1[num2].targetPivot = global::Joints.Joints.jtb1[num2].minPivot;
				}
			}
			return;
		}
		for (int i = 0; i < numJoints; i++)
		{
			long num2 = pgBasic[pgID].jt[num + i];
			global::Joints.Joints.jtb1[num2].targetAngle = pgBasic[pgID].xRot[num + i];
			global::Joints.Joints.jtb1[num2].targetPivot2 = pgBasic[pgID].yRot[num + i];
			global::Joints.Joints.jtb1[num2].targetPivot = pgBasic[pgID].zRot[num + i];
			global::Joints.Joints.jtb1[num2].angleSpeed = pgBasic[pgID].rotXSpeed[num + i];
			global::Joints.Joints.jtb1[num2].pivotSpeed = pgBasic[pgID].rotZSpeed[num + i];
			global::Joints.Joints.jtb1[num2].pivot2Speed = pgBasic[pgID].rotYSpeed[num + i];
			global::Joints.Joints.jtb1[num2].time = pgBasic[pgID].time[num + i];
			global::Joints.Joints.jtb1[num2].targetX = pgBasic[pgID].x[num + i];
			global::Joints.Joints.jtb1[num2].targetY = pgBasic[pgID].y[num + i];
			global::Joints.Joints.jtb1[num2].targetZ = pgBasic[pgID].z[num + i];
			global::Joints.Joints.jtb1[num2].xSpeed = pgBasic[pgID].xSpeed2[num + i];
			global::Joints.Joints.jtb1[num2].ySpeed = pgBasic[pgID].ySpeed2[num + i];
			global::Joints.Joints.jtb1[num2].zSpeed = pgBasic[pgID].zSpeed2[num + i];
			if (global::Joints.Joints.jtb1[num2].targetAngle > global::Joints.Joints.jtb1[num2].maxAngle)
			{
				global::Joints.Joints.jtb1[num2].targetAngle = global::Joints.Joints.jtb1[num2].maxAngle;
			}
			if (global::Joints.Joints.jtb1[num2].targetAngle < global::Joints.Joints.jtb1[num2].minAngle)
			{
				global::Joints.Joints.jtb1[num2].targetAngle = global::Joints.Joints.jtb1[num2].minAngle;
			}
			if (global::Joints.Joints.jtb1[num2].targetPivot2 > global::Joints.Joints.jtb1[num2].maxPivot2)
			{
				global::Joints.Joints.jtb1[num2].targetPivot2 = global::Joints.Joints.jtb1[num2].maxPivot2;
			}
			if (global::Joints.Joints.jtb1[num2].targetPivot2 < global::Joints.Joints.jtb1[num2].minPivot2)
			{
				global::Joints.Joints.jtb1[num2].targetPivot2 = global::Joints.Joints.jtb1[num2].minPivot2;
			}
			if (global::Joints.Joints.jtb1[num2].targetPivot > global::Joints.Joints.jtb1[num2].maxPivot)
			{
				global::Joints.Joints.jtb1[num2].targetPivot = global::Joints.Joints.jtb1[num2].maxPivot;
			}
			if (global::Joints.Joints.jtb1[num2].targetPivot < global::Joints.Joints.jtb1[num2].minPivot)
			{
				global::Joints.Joints.jtb1[num2].targetPivot = global::Joints.Joints.jtb1[num2].minPivot;
			}
		}
	}

	public void Start_Program_Basic(short pgID, bool reverse, short step)
	{
		pgBasic[pgID].callBack = 0;
		pgBasic[pgID].inReverse = reverse;
		pgBasic[pgID].status = 2;
		pgBasic[pgID].curStep = step;
		int numJoints = pgBasic[pgID].numJoints;
		int num = step * numJoints;
		if (!reverse)
		{
			for (int i = 0; i < numJoints; i++)
			{
				long num2 = pgBasic[pgID].jt[num + i];
				global::Joints.Joints.jtb1[num2].targetAngle = pgBasic[pgID].xRot[num + i];
				global::Joints.Joints.jtb1[num2].targetPivot2 = pgBasic[pgID].yRot[num + i];
				global::Joints.Joints.jtb1[num2].targetPivot = pgBasic[pgID].zRot[num + i];
				global::Joints.Joints.jtb1[num2].angleSpeed = pgBasic[pgID].rotXSpeed[num + i];
				global::Joints.Joints.jtb1[num2].pivotSpeed = pgBasic[pgID].rotZSpeed[num + i];
				global::Joints.Joints.jtb1[num2].pivot2Speed = pgBasic[pgID].rotYSpeed[num + i];
				global::Joints.Joints.jtb1[num2].time = pgBasic[pgID].time2[num + i];
				global::Joints.Joints.jtb1[num2].targetX = pgBasic[pgID].x2[num + i];
				global::Joints.Joints.jtb1[num2].targetY = pgBasic[pgID].y2[num + i];
				global::Joints.Joints.jtb1[num2].targetZ = pgBasic[pgID].z2[num + i];
				global::Joints.Joints.jtb1[num2].xSpeed = pgBasic[pgID].xSpeed[num + i];
				global::Joints.Joints.jtb1[num2].ySpeed = pgBasic[pgID].ySpeed[num + i];
				global::Joints.Joints.jtb1[num2].zSpeed = pgBasic[pgID].zSpeed[num + i];
				if (global::Joints.Joints.jtb1[num2].targetAngle > global::Joints.Joints.jtb1[num2].maxAngle)
				{
					global::Joints.Joints.jtb1[num2].targetAngle = global::Joints.Joints.jtb1[num2].maxAngle;
				}
				if (global::Joints.Joints.jtb1[num2].targetAngle < global::Joints.Joints.jtb1[num2].minAngle)
				{
					global::Joints.Joints.jtb1[num2].targetAngle = global::Joints.Joints.jtb1[num2].minAngle;
				}
				if (global::Joints.Joints.jtb1[num2].targetPivot2 > global::Joints.Joints.jtb1[num2].maxPivot2)
				{
					global::Joints.Joints.jtb1[num2].targetPivot2 = global::Joints.Joints.jtb1[num2].maxPivot2;
				}
				if (global::Joints.Joints.jtb1[num2].targetPivot2 < global::Joints.Joints.jtb1[num2].minPivot2)
				{
					global::Joints.Joints.jtb1[num2].targetPivot2 = global::Joints.Joints.jtb1[num2].minPivot2;
				}
				if (global::Joints.Joints.jtb1[num2].targetPivot > global::Joints.Joints.jtb1[num2].maxPivot)
				{
					global::Joints.Joints.jtb1[num2].targetPivot = global::Joints.Joints.jtb1[num2].maxPivot;
				}
				if (global::Joints.Joints.jtb1[num2].targetPivot < global::Joints.Joints.jtb1[num2].minPivot)
				{
					global::Joints.Joints.jtb1[num2].targetPivot = global::Joints.Joints.jtb1[num2].minPivot;
				}
			}
			return;
		}
		for (int i = 0; i < numJoints; i++)
		{
			long num2 = pgBasic[pgID].jt[num + i];
			global::Joints.Joints.jtb1[num2].targetAngle = pgBasic[pgID].xRot[num + i];
			global::Joints.Joints.jtb1[num2].targetPivot2 = pgBasic[pgID].yRot[num + i];
			global::Joints.Joints.jtb1[num2].targetPivot = pgBasic[pgID].zRot[num + i];
			global::Joints.Joints.jtb1[num2].angleSpeed = pgBasic[pgID].rotXSpeed[num + i];
			global::Joints.Joints.jtb1[num2].pivotSpeed = pgBasic[pgID].rotZSpeed[num + i];
			global::Joints.Joints.jtb1[num2].pivot2Speed = pgBasic[pgID].rotYSpeed[num + i];
			global::Joints.Joints.jtb1[num2].time = pgBasic[pgID].time[num + i];
			global::Joints.Joints.jtb1[num2].targetX = pgBasic[pgID].x[num + i];
			global::Joints.Joints.jtb1[num2].targetY = pgBasic[pgID].y[num + i];
			global::Joints.Joints.jtb1[num2].targetZ = pgBasic[pgID].z[num + i];
			global::Joints.Joints.jtb1[num2].xSpeed = pgBasic[pgID].xSpeed2[num + i];
			global::Joints.Joints.jtb1[num2].ySpeed = pgBasic[pgID].ySpeed2[num + i];
			global::Joints.Joints.jtb1[num2].zSpeed = pgBasic[pgID].zSpeed2[num + i];
			if (global::Joints.Joints.jtb1[num2].targetAngle > global::Joints.Joints.jtb1[num2].maxAngle)
			{
				global::Joints.Joints.jtb1[num2].targetAngle = global::Joints.Joints.jtb1[num2].maxAngle;
			}
			if (global::Joints.Joints.jtb1[num2].targetAngle < global::Joints.Joints.jtb1[num2].minAngle)
			{
				global::Joints.Joints.jtb1[num2].targetAngle = global::Joints.Joints.jtb1[num2].minAngle;
			}
			if (global::Joints.Joints.jtb1[num2].targetPivot2 > global::Joints.Joints.jtb1[num2].maxPivot2)
			{
				global::Joints.Joints.jtb1[num2].targetPivot2 = global::Joints.Joints.jtb1[num2].maxPivot2;
			}
			if (global::Joints.Joints.jtb1[num2].targetPivot2 < global::Joints.Joints.jtb1[num2].minPivot2)
			{
				global::Joints.Joints.jtb1[num2].targetPivot2 = global::Joints.Joints.jtb1[num2].minPivot2;
			}
			if (global::Joints.Joints.jtb1[num2].targetPivot > global::Joints.Joints.jtb1[num2].maxPivot)
			{
				global::Joints.Joints.jtb1[num2].targetPivot = global::Joints.Joints.jtb1[num2].maxPivot;
			}
			if (global::Joints.Joints.jtb1[num2].targetPivot < global::Joints.Joints.jtb1[num2].minPivot)
			{
				global::Joints.Joints.jtb1[num2].targetPivot = global::Joints.Joints.jtb1[num2].minPivot;
			}
		}
	}

	public void Toggle_Program_Direction(byte playerID, ref StructsClass.joint[] jt1, ref StructsClass.program_instance[] pg1, int pcID, int pgID)
	{
		bool flag = true;
		int num;
		if (pg1[pgID].status != 2)
		{
			flag = false;
			num = pgC[pcID].pg1[pgID].group;
			for (int i = 0; i < pgC[pcID].numPrograms; i++)
			{
				if ((pgC[pcID].pg1[i].group & num) > 0 && pg1[i].status == 2)
				{
					pg1[i].status = 1;
					mainC.callbackMain.CallBack(playerID, pg1[i].callBackType, pg1[i].callBack, 0, 1);
				}
			}
		}
		if (pg1[pgID].inReverse)
		{
			pg1[pgID].inReverse = false;
			pg1[pgID].curStep--;
			if (flag)
			{
				pg1[pgID].curStep++;
			}
		}
		else
		{
			pg1[pgID].inReverse = true;
			pg1[pgID].curStep++;
			if (flag)
			{
				pg1[pgID].curStep--;
			}
		}
		pg1[pgID].status = 2;
		Run_Program(playerID, ref jt1, ref pg1, pcID, pgID, programStarting: true);
		if (flag)
		{
			return;
		}
		num = pgC[pcID].pg1[pgID].numJoints;
		for (int i = 0; i < num; i++)
		{
			int num2 = pgC[pcID].pg1[pgID].jt[i];
			if (jt1[num2].rotX != jt1[num2].targetAngle)
			{
				jt1[num2].angleSpeed = Math.Abs(jt1[num2].targetAngle - jt1[num2].rotX) / 0.15f;
				if (jt1[num2].angleSpeed < 25f)
				{
					jt1[num2].angleSpeed = 25f;
				}
			}
			else
			{
				jt1[num2].angleSpeed = 700f;
			}
			if (jt1[num2].rotZ != jt1[num2].targetPivot)
			{
				jt1[num2].pivotSpeed = Math.Abs(jt1[num2].targetPivot - jt1[num2].rotZ) / 0.15f;
				if (jt1[num2].pivotSpeed < 25f)
				{
					jt1[num2].pivotSpeed = 25f;
				}
			}
			else
			{
				jt1[num2].pivotSpeed = 700f;
			}
			if (jt1[num2].rotY != jt1[num2].targetPivot2)
			{
				jt1[num2].pivot2Speed = Math.Abs(jt1[num2].targetPivot2 - jt1[num2].rotY) / 0.15f;
				if (jt1[num2].pivot2Speed < 25f)
				{
					jt1[num2].pivot2Speed = 25f;
				}
			}
			else
			{
				jt1[num2].pivot2Speed = 700f;
			}
		}
	}

	public void Set_Program_To_Reverse_Direction(ref StructsClass.program_instance[] pg1, int pgID)
	{
		pg1[pgID].inReverse = true;
	}

	public void Set_Program_To_Forward_Direction(ref StructsClass.program_instance[] pg1, int pgID)
	{
		pg1[pgID].inReverse = false;
	}

	public void Stop_Program(ref StructsClass.program_instance[] pg1, int pgID)
	{
		if (pg1[pgID].status > 1)
		{
			pg1[pgID].status = 1;
		}
	}

	public void Stop_Program_If_Not_Looping(ref StructsClass.program_instance[] pg1, int pgID, ushort pgcID)
	{
		if (pgC[pgcID].pg1[pgID].loop && pg1[pgID].status > 1)
		{
			pg1[pgID].status = 1;
		}
	}

	public void Switch_Program(byte playerID, ref StructsClass.joint[] jt1, ref StructsClass.program_instance[] pg1, int pcID, int newPID)
	{
		if (pg1[newPID].status == 2)
		{
			return;
		}
		int num = pgC[pcID].pg1[newPID].group;
		for (int i = 0; i < pgC[pcID].numPrograms; i++)
		{
			if ((pgC[pcID].pg1[i].group & num) > 0 && pg1[i].status != 1)
			{
				pg1[i].status = 1;
				mainC.callbackMain.CallBack(playerID, pg1[i].callBackType, pg1[i].callBack, 0, 1);
			}
		}
		pg1[newPID].status = 2;
		pg1[newPID].curStep = (short)(pgC[pcID].pg1[newPID].numSteps - 1);
		Run_Program(playerID, ref jt1, ref pg1, pcID, newPID, programStarting: true);
	}

	public void Process_Program(byte playerID, ref StructsClass.joint[] jt1, ref StructsClass.program_instance[] pg1, int pcID)
	{
		for (long num = 0L; num < pgC[pcID].numPrograms; num++)
		{
			if (pg1[num].status != 2)
			{
				continue;
			}
			int numJoints = pgC[pcID].pg1[num].numJoints;
			int num2 = pg1[num].curStep * numJoints;
			long num3;
			try
			{
				for (int i = 0; i < numJoints; i++)
				{
					num3 = num2 + i;
					long num4 = pgC[pcID].pg1[num].jt[num3];
					jt1[num4].targetAngle = pgC[pcID].pg1[num].xRot[num3];
					jt1[num4].targetPivot = pgC[pcID].pg1[num].zRot[num3];
					jt1[num4].targetPivot2 = pgC[pcID].pg1[num].yRot[num3];
					jt1[num4].angleSpeed = pgC[pcID].pg1[num].rotXSpeed[num3];
					jt1[num4].pivotSpeed = pgC[pcID].pg1[num].rotZSpeed[num3];
					jt1[num4].pivot2Speed = pgC[pcID].pg1[num].rotYSpeed[num3];
					float num5 = Math.Abs((jt1[num4].targetAngle - jt1[num4].rotX) / (jt1[num4].angleSpeed + 1E-05f));
					if (num5 > pg1[num].stepTime)
					{
						jt1[num4].angleSpeed = Math.Abs((jt1[num4].targetAngle - jt1[num4].rotX) / pg1[num].stepTime) * 2f;
					}
					num5 = Math.Abs((jt1[num4].targetPivot - jt1[num4].rotZ) / (jt1[num4].pivotSpeed + 1E-05f));
					if (num5 > pg1[num].stepTime)
					{
						jt1[num4].pivotSpeed = Math.Abs((jt1[num4].targetPivot - jt1[num4].rotZ) / pg1[num].stepTime) * 2f;
					}
					num5 = Math.Abs((jt1[num4].targetPivot2 - jt1[num4].rotY) / (jt1[num4].pivot2Speed + 1E-05f));
					if (num5 > pg1[num].stepTime)
					{
						jt1[num4].pivot2Speed = Math.Abs((jt1[num4].targetPivot2 - jt1[num4].rotY) / pg1[num].stepTime) * 2f;
					}
				}
			}
			catch
			{
			}
			num3 = pgC[pcID].pg1[num].jt[pg1[num].timingJoint + pgC[pcID].pg1[num].numJoints * pg1[num].curStep];
			if (jt1[num3].rotX == jt1[num3].targetAngle && jt1[num3].rotY == jt1[num3].targetPivot2 && jt1[num3].rotZ == jt1[num3].targetPivot)
			{
				Run_Program(playerID, ref jt1, ref pg1, pcID, (int)num, programStarting: false);
			}
		}
	}

	public void Process_Programs_Basic()
	{
		for (long num = 0L; num < numProgramsBasic; num++)
		{
			if (pgBasic[num].status != 2)
			{
				continue;
			}
			short num2 = 0;
			long num3 = pgBasic[num].numJoints * pgBasic[num].curStep;
			for (long num4 = 0L; num4 < pgBasic[num].numJoints; num4++)
			{
				if (global::Joints.Joints.jtb1[pgBasic[num].jt[num3 + num4]].angle != global::Joints.Joints.jtb1[pgBasic[num].jt[num3 + num4]].targetAngle || global::Joints.Joints.jtb1[pgBasic[num].jt[num3 + num4]].pivot2 != global::Joints.Joints.jtb1[pgBasic[num].jt[num3 + num4]].targetPivot2 || global::Joints.Joints.jtb1[pgBasic[num].jt[num3 + num4]].pivot != global::Joints.Joints.jtb1[pgBasic[num].jt[num3 + num4]].targetPivot || global::Joints.Joints.jtb1[pgBasic[num].jt[num3 + num4]].x != global::Joints.Joints.jtb1[pgBasic[num].jt[num3 + num4]].targetX || global::Joints.Joints.jtb1[pgBasic[num].jt[num3 + num4]].y != global::Joints.Joints.jtb1[pgBasic[num].jt[num3 + num4]].targetY || global::Joints.Joints.jtb1[pgBasic[num].jt[num3 + num4]].z != global::Joints.Joints.jtb1[pgBasic[num].jt[num3 + num4]].targetZ || global::Joints.Joints.jtb1[pgBasic[num].jt[num3 + num4]].time > 0f)
				{
					num2 = 1;
					break;
				}
			}
			if (num2 == 0)
			{
				Run_Program_Basic((int)num, toggleDirection: false, pgBasic[num].callBackType, pgBasic[num].callBack);
			}
		}
	}

	public void Process_Animations(float frameTime, ushort playerID, ref StructsClass.joint[] jt1, ref StructsClass.animation_instance[] animation, int pcID)
	{
		for (ushort num = 0; num < pgC[pcID].numAnimations; num++)
		{
			if (animation[num].status != 2)
			{
				continue;
			}
			animation[num].curTime += frameTime * animation[num].directionAndSpeed;
			ushort num2 = 0;
			ushort numActions = pgC[pcID].animation1[num].numActions;
			while (num2 < numActions)
			{
				if (Math.Sign(animation[num].directionAndSpeed) == Math.Sign(pgC[pcID].animation1[num].actions[num2].direction) && !animation[num].actionComplete[num2] && ((animation[num].directionAndSpeed >= 0f && animation[num].curTime >= pgC[pcID].animation1[num].actions[num2].time) || (animation[num].directionAndSpeed < 0f && animation[num].curTime < pgC[pcID].animation1[num].actions[num2].time)))
				{
					animation[num].actionComplete[num2] = true;
					mainC.callbackMain.Actions(playerID, pgC[pcID].animation1[num].actions[num2].type, pgC[pcID].animation1[num].actions[num2].actionID, num, num2);
				}
				num2++;
			}
			if (!pgC[pcID].animation1[num].loop)
			{
				if (animation[num].curTime >= pgC[pcID].animation1[num].length)
				{
					num2 = 0;
					numActions = pgC[pcID].animation1[num].numAnimationSequences;
					while (num2 < numActions)
					{
						ushort numAnimationFrames = pgC[pcID].animation1[num].animation_sequences[num2].numAnimationFrames;
						if (numAnimationFrames > 0)
						{
							numAnimationFrames--;
							ushort jointID = pgC[pcID].animation1[num].animation_sequences[num2].jointID;
							jt1[jointID].mvAnimation = pgC[pcID].animation1[num].animation_sequences[num2].animation_frames[numAnimationFrames].mv;
						}
						num2++;
					}
					animation[num].curTime = pgC[pcID].animation1[num].length;
					if (!pgC[pcID].animation1[num].staysActiveAtEnd)
					{
						animation[num].status = 1;
						mainC.callbackMain.CallBack(playerID, animation[num].callBackType, animation[num].callBack, animation[num].var1, 0);
					}
					continue;
				}
				if (animation[num].curTime <= 0f && animation[num].directionAndSpeed < 0f)
				{
					num2 = 0;
					numActions = pgC[pcID].animation1[num].numAnimationSequences;
					while (num2 < numActions)
					{
						if (pgC[pcID].animation1[num].animation_sequences[num2].numAnimationFrames > 0)
						{
							ushort jointID = pgC[pcID].animation1[num].animation_sequences[num2].jointID;
							jt1[jointID].mvAnimation = pgC[pcID].animation1[num].animation_sequences[num2].animation_frames[0].mv;
						}
						num2++;
					}
					animation[num].curTime = 0f;
					animation[num].status = 1;
					mainC.callbackMain.CallBack(playerID, animation[num].callBackType, animation[num].callBack, animation[num].var1, 0);
					continue;
				}
			}
			while (animation[num].curTime > pgC[pcID].animation1[num].length)
			{
				animation[num].curTime -= pgC[pcID].animation1[num].length;
			}
			while (animation[num].curTime < 0f)
			{
				animation[num].curTime += pgC[pcID].animation1[num].length;
			}
			num2 = 0;
			numActions = pgC[pcID].animation1[num].numAnimationSequences;
			while (num2 < numActions)
			{
				ushort jointID = pgC[pcID].animation1[num].animation_sequences[num2].jointID;
				ushort numAnimationFrames = pgC[pcID].animation1[num].animation_sequences[num2].numAnimationFrames;
				if (numAnimationFrames < 2)
				{
					jt1[jointID].mvAnimation = pgC[pcID].animation1[num].animation_sequences[num2].animation_frames[0].mv;
				}
				else
				{
					ushort num3 = animation[num].curFrames[num2];
					num3 = (ushort)((!(pgC[pcID].animation1[num].animation_sequences[num2].animation_frames[num3].time > animation[num].curTime)) ? ((ushort)(num3 + 1)) : 0);
					while (num3 < numAnimationFrames && !(pgC[pcID].animation1[num].animation_sequences[num2].animation_frames[num3].time > animation[num].curTime))
					{
						num3++;
					}
					ushort num4 = (ushort)(numAnimationFrames - 1);
					if (num3 >= numAnimationFrames)
					{
						num3 = 0;
					}
					else if (num3 > 0)
					{
						num4 = (ushort)(num3 - 1);
					}
					animation[num].curFrames[num2] = num4;
					float num7;
					if (num3 > num4)
					{
						float num5 = pgC[pcID].animation1[num].animation_sequences[num2].animation_frames[num3].time - pgC[pcID].animation1[num].animation_sequences[num2].animation_frames[num4].time;
						float num6 = animation[num].curTime - pgC[pcID].animation1[num].animation_sequences[num2].animation_frames[num4].time;
						num7 = 1f;
						if (num5 != 0f)
						{
							num7 = num6 / num5;
						}
					}
					else
					{
						float num5 = pgC[pcID].animation1[num].length - pgC[pcID].animation1[num].animation_sequences[num2].animation_frames[num4].time + pgC[pcID].animation1[num].animation_sequences[num2].animation_frames[num3].time;
						float num6 = ((!(animation[num].curTime >= pgC[pcID].animation1[num].animation_sequences[num2].animation_frames[num4].time)) ? (pgC[pcID].animation1[num].length - pgC[pcID].animation1[num].animation_sequences[num2].animation_frames[num4].time + animation[num].curTime) : (animation[num].curTime - pgC[pcID].animation1[num].animation_sequences[num2].animation_frames[num4].time));
						num7 = 1f;
						if (num5 != 0f)
						{
							num7 = num6 / num5;
						}
					}
					if (num7 > 1f)
					{
						global::InputHandler.InputHandler.tw += 1f;
					}
					else if (num7 < 0f)
					{
						global::InputHandler.InputHandler.tw -= 1f;
					}
					jt1[jointID].mvAnimation = Matrix.Lerp(pgC[pcID].animation1[num].animation_sequences[num2].animation_frames[num4].mv, pgC[pcID].animation1[num].animation_sequences[num2].animation_frames[num3].mv, num7);
				}
				num2++;
			}
		}
	}

	public void Start_Animation(ushort playerID, ref StructsClass.joint[] jt1, ref StructsClass.animation_instance[] animations, int pcID, int animationID, float directionAndSpeed, float scaleFactor)
	{
		animations[animationID].directionAndSpeed = directionAndSpeed;
		if (directionAndSpeed == 0f)
		{
			animations[animationID].directionAndSpeed = pgC[pcID].animation1[animationID].directionAndSpeed;
		}
		animations[animationID].scaleFactor = scaleFactor;
		if (animations[animationID].status == 2)
		{
			return;
		}
		int num = pgC[pcID].animation1[animationID].group;
		for (int i = 0; i < pgC[pcID].numAnimations; i++)
		{
			if ((pgC[pcID].animation1[i].group & num) > 0 && animations[i].status != 1)
			{
				animations[i].status = 1;
				mainC.callbackMain.CallBack(playerID, animations[i].cancelledCallBackType, animations[i].cancelledCallBack, animations[i].var1, 1);
			}
		}
		for (int i = 0; i < pgC[pcID].animation1[animationID].numAnimationSequences; i++)
		{
			animations[animationID].curFrames[i] = 0;
		}
		for (int i = 0; i < pgC[pcID].animation1[animationID].numActions; i++)
		{
			animations[animationID].actionComplete[i] = false;
		}
		Set_Joints_To_Animation_Start(ref jt1, pcID, animationID, playerID, directionAndSpeed);
		animations[animationID].status = 2;
		animations[animationID].curTime = 0f;
		if (directionAndSpeed < 0f)
		{
			animations[animationID].curTime = pgC[pcID].animation1[animationID].length;
		}
		if (playerID == 0 && pgC[pcID].animation1[animationID].networked && global::Networking.Networking.inGame)
		{
			Send_Network_Animation((byte)animationID, animations[animationID].var1, (short)directionAndSpeed);
		}
	}

	public void Re_Start_Animation(ushort playerID, ref StructsClass.joint[] jt1, ref StructsClass.animation_instance[] animations, int pcID, int animationID, float directionAndSpeed, float scaleFactor)
	{
		animations[animationID].directionAndSpeed = directionAndSpeed;
		if (directionAndSpeed == 0f)
		{
			animations[animationID].directionAndSpeed = pgC[pcID].animation1[animationID].directionAndSpeed;
		}
		animations[animationID].scaleFactor = scaleFactor;
		for (int i = 0; i < pgC[pcID].animation1[animationID].numAnimationSequences; i++)
		{
			animations[animationID].curFrames[i] = 0;
		}
		for (int i = 0; i < pgC[pcID].animation1[animationID].numActions; i++)
		{
			animations[animationID].actionComplete[i] = false;
		}
		animations[animationID].status = 2;
		animations[animationID].curTime = 0f;
	}

	public void Stop_Animation_If_Not_Looping(ref StructsClass.animation_instance[] animations, ushort animationID, ushort pgcID)
	{
		if (pgC[pgcID].animation1[animationID].loop && animations[animationID].status > 1)
		{
			animations[animationID].status = 1;
		}
	}

	public void Stop_Animation(ref StructsClass.animation_instance[] animations, int pgID)
	{
		if (animations[pgID].status > 1)
		{
			animations[pgID].status = 1;
		}
	}

	public void Set_Animation_To_Reverse_Direction(ref StructsClass.animation_instance[] animations, int animationID)
	{
		if (animations[animationID].directionAndSpeed > 0f)
		{
			animations[animationID].directionAndSpeed *= -1f;
		}
		else if (animations[animationID].directionAndSpeed == 0f)
		{
			animations[animationID].directionAndSpeed = -1f;
		}
	}

	public void Set_Joints_To_Animation_Start(ref StructsClass.joint[] jt1, int pcID, int animationID, ushort playerID, float direction)
	{
		if (direction >= 0f)
		{
			int i = 1;
			for (int numAnimationSequences = pgC[pcID].animation1[animationID].numAnimationSequences; i < numAnimationSequences; i++)
			{
				ushort jointID = pgC[pcID].animation1[animationID].animation_sequences[i].jointID;
				jt1[jointID].matrixReady = true;
				jt1[jointID].mvAnimation = pgC[pcID].animation1[animationID].animation_sequences[i].animation_frames[0].mv;
			}
		}
		else
		{
			int i = 1;
			for (int numAnimationSequences = pgC[pcID].animation1[animationID].numAnimationSequences; i < numAnimationSequences; i++)
			{
				ushort jointID = pgC[pcID].animation1[animationID].animation_sequences[i].jointID;
				jt1[jointID].matrixReady = true;
				jt1[jointID].mvAnimation = pgC[pcID].animation1[animationID].animation_sequences[i].animation_frames[pgC[pcID].animation1[animationID].animation_sequences[i].numAnimationFrames - 1].mv;
			}
		}
	}

	public void Set_Joints_To_Animation_Step_Percentage(ref StructsClass.joint[] jt1, int pcID, ushort animationID, float percent)
	{
		if (percent > 1f)
		{
			percent = 1f;
		}
		else if (percent < 0f)
		{
			percent = 0f;
		}
		int numAnimationSequences = pgC[pcID].animation1[animationID].numAnimationSequences;
		for (ushort num = 0; num < numAnimationSequences; num++)
		{
			int num2 = pgC[pcID].animation1[animationID].animation_sequences[num].numAnimationFrames - 1;
			if (num2 > 0)
			{
				float num3 = (float)num2 * percent;
				int num4 = (int)Math.Floor(num3);
				num3 -= (float)num4;
				int num5 = num4;
				if (num4 < num2)
				{
					num5 = num4 + 1;
				}
				jt1[pgC[pcID].animation1[animationID].animation_sequences[num].jointID].mvAnimation = Matrix.Lerp(pgC[pcID].animation1[animationID].animation_sequences[num].animation_frames[num4].mv, pgC[pcID].animation1[animationID].animation_sequences[num].animation_frames[num5].mv, num3);
			}
			else
			{
				jt1[pgC[pcID].animation1[animationID].animation_sequences[num].jointID].mvAnimation = pgC[pcID].animation1[animationID].animation_sequences[num].animation_frames[0].mv;
			}
		}
	}

	public void Send_Network_Animation(byte animationID, ushort var1, short directionAndSpeed)
	{
		global::Networking.Networking.networkBytes[0] = animationID;
		global::Networking.Networking.networkUShorts[0] = var1;
		global::Networking.Networking.networkShorts[0] = directionAndSpeed;
		mainC.networkingMain.XBOX_Send_Network_Message6(6);
	}

	public void Receive_Network_Animation(int actID)
	{
		short num = mainC.playersMain.Get_Player_Index(actID, -1);
		if (num >= 0)
		{
			global::Players.Players.players[num].animations[global::Networking.Networking.networkBytes[0]].var1 = global::Networking.Networking.networkUShorts[0];
			Start_Animation((ushort)num, ref global::Players.Players.players[num].jt1, ref global::Players.Players.players[num].animations, global::Players.Players.players[num].programCollection, global::Networking.Networking.networkBytes[0], global::Networking.Networking.networkShorts[0], 1f);
		}
	}

	public void Cancel_Animations_Of_Type(ushort playerID, ref StructsClass.animation_instance[] animations, int pcID, byte animationType)
	{
		for (int i = 0; i < pgC[pcID].numAnimations; i++)
		{
			if (animations[i].cancelledCallBackType == animationType && animations[i].status != 1)
			{
				animations[i].status = 1;
				mainC.callbackMain.CallBack(playerID, animations[i].cancelledCallBackType, animations[i].cancelledCallBack, animations[i].var1, 1);
			}
		}
	}

	public void Reset_Object(ushort pgID)
	{
		mainC.programsMain.Set_BasicJoints_To_Program_Step(pgID, 0, 1);
		mainC.programsMain.Run_Program_Basic(pgID, toggleDirection: false, 0, 0);
		pgBasic[pgID].inReverse = false;
	}

	public void Set_Joints_To_Program_Step_Percentage(ref StructsClass.joint[] jt1, ref StructsClass.program_instance[] pg1, short pcID, short pgID, short step, float percent, bool loop)
	{
		pg1[pgID].curStep = step;
		int numJoints = pgC[pcID].pg1[pgID].numJoints;
		int num = step - 1;
		if (num < 0)
		{
			num = ((!loop) ? step : (pgC[pcID].pg1[pgID].numSteps - 1));
		}
		num *= numJoints;
		int num2 = step * numJoints;
		for (int i = 0; i < numJoints; i++)
		{
			long num3 = pgC[pcID].pg1[pgID].jt[num2 + i];
			jt1[num3].targetAngle = (jt1[num3].rotX = pgC[pcID].pg1[pgID].xRot[num + i] + (pgC[pcID].pg1[pgID].xRot[num2 + i] - pgC[pcID].pg1[pgID].xRot[num + i]) * percent);
			jt1[num3].rotZ = (jt1[num3].targetPivot = pgC[pcID].pg1[pgID].zRot[num + i] + (pgC[pcID].pg1[pgID].zRot[num2 + i] - pgC[pcID].pg1[pgID].zRot[num + i]) * percent);
			jt1[num3].rotY = (jt1[num3].targetPivot2 = pgC[pcID].pg1[pgID].yRot[num + i] + (pgC[pcID].pg1[pgID].yRot[num2 + i] - pgC[pcID].pg1[pgID].yRot[num + i]) * percent);
			jt1[num3].angleSpeed = pgC[pcID].pg1[pgID].rotXSpeed[num2 + i];
			jt1[num3].pivotSpeed = pgC[pcID].pg1[pgID].rotZSpeed[num2 + i];
			jt1[num3].pivot2Speed = pgC[pcID].pg1[pgID].rotYSpeed[num2 + i];
		}
	}

	public void Set_BasicJoints_To_Program_Step(int pgID, short step, sbyte status)
	{
		if (pgID < numProgramsBasic && pgID >= 0 && pgBasic[pgID].status >= 1)
		{
			int numJoints = pgBasic[pgID].numJoints;
			int num = step * numJoints;
			pgBasic[pgID].status = status;
			pgBasic[pgID].curStep = step;
			for (int i = 0; i < numJoints; i++)
			{
				long num2 = pgBasic[pgID].jt[num + i];
				global::Joints.Joints.jtb1[num2].angle = (global::Joints.Joints.jtb1[num2].targetAngle = pgBasic[pgID].xRot[num + i]);
				global::Joints.Joints.jtb1[num2].pivot2 = (global::Joints.Joints.jtb1[num2].targetPivot2 = pgBasic[pgID].yRot[num + i]);
				global::Joints.Joints.jtb1[num2].pivot = (global::Joints.Joints.jtb1[num2].targetPivot = pgBasic[pgID].zRot[num + i]);
				global::Joints.Joints.jtb1[num2].x = (global::Joints.Joints.jtb1[num2].targetX = pgBasic[pgID].x[num + i]);
				global::Joints.Joints.jtb1[num2].y = (global::Joints.Joints.jtb1[num2].targetY = pgBasic[pgID].y[num + i]);
				global::Joints.Joints.jtb1[num2].z = (global::Joints.Joints.jtb1[num2].targetZ = pgBasic[pgID].z[num + i]);
				global::Joints.Joints.jtb1[num2].time = pgBasic[pgID].time[num + i];
			}
		}
	}

	public void Reset_Round(bool minorRestart)
	{
		if (minorRestart)
		{
			for (long num = 0L; num < numProgramsBasic; num++)
			{
				if (pgBasic[num].resetOnMinorStart)
				{
					pgBasic[num].status = 1;
					pgBasic[num].curStep = 0;
				}
			}
		}
		else
		{
			for (long num = 0L; num < numProgramsBasic; num++)
			{
				pgBasic[num].status = 1;
				pgBasic[num].curStep = 0;
			}
		}
		global::MainGame.MainGame.haveProgramData = true;
		global::Joints.Joints.Do_Joint_Basic_Calculations(1f);
	}

	public void Reset_Programs(ref StructsClass.program_instance[] pg1, ref StructsClass.animation_instance[] animations, int pcID)
	{
		for (ushort num = 0; num < pgC[pcID].numPrograms; num++)
		{
			pg1[num].status = 1;
			pg1[num].curStep = 0;
			pg1[num].inReverse = false;
		}
		for (ushort num = 0; num < pgC[pcID].numAnimations; num++)
		{
			animations[num].status = 1;
			animations[num].curTime = 0f;
			ushort num2 = 0;
			ushort numAnimationSequences = pgC[pcID].animation1[num].numAnimationSequences;
			while (num2 < numAnimationSequences)
			{
				animations[num].curFrames[num2] = 0;
				num2++;
			}
			num2 = 0;
			numAnimationSequences = pgC[pcID].animation1[num].numActions;
			while (num2 < numAnimationSequences)
			{
				animations[num].actionComplete[num2] = false;
				num2++;
			}
		}
	}

	public void Request_Program_Status()
	{
		mainC.networkingMain.XBOX_Send_Network_Message_To_Host(28);
	}

	public void XBOX_Send_Program_Status(NetworkGamer newGamer)
	{
		byte b = 0;
		if (numProgramsBasic < 1)
		{
			global::Networking.Networking.networkInts[0] = 0;
			mainC.networkingMain.XBOX_Send_Network_Message_To_Gamer(10, newGamer);
			return;
		}
		byte b2 = 10;
		for (short num = 0; num < numProgramsBasic; num++)
		{
			global::Networking.Networking.networkBytes[b] = (byte)num;
			global::Networking.Networking.networkBools[b] = pgBasic[num].inReverse;
			global::Networking.Networking.networkSBytes[b] = pgBasic[num].status;
			global::Networking.Networking.networkShorts[b] = pgBasic[num].curStep;
			b++;
			if (b >= b2)
			{
				global::Networking.Networking.networkInts[0] = b;
				mainC.networkingMain.XBOX_Send_Network_Message_To_Gamer(10, newGamer);
				b = 0;
			}
		}
		if (b > 0)
		{
			global::Networking.Networking.networkInts[0] = b;
			mainC.networkingMain.XBOX_Send_Network_Message_To_Gamer(10, newGamer);
		}
	}

	public void XBOX_Update_Program_Status_From_Network(byte cnt)
	{
		try
		{
			for (short num = 0; num < cnt; num++)
			{
				short num2 = global::Networking.Networking.networkBytes[num];
				pgBasic[num2].inReverse = global::Networking.Networking.networkBools[num];
				pgBasic[num2].curStep = global::Networking.Networking.networkShorts[num];
				pgBasic[num2].status = global::Networking.Networking.networkSBytes[num];
				if (pgBasic[num2].status == 2)
				{
					Start_Program_Basic(num2, global::Networking.Networking.networkBools[num], global::Networking.Networking.networkShorts[num]);
				}
				else if (pgBasic[num2].status == 3)
				{
					pgBasic[num2].curStep = 0;
					Start_Program_Basic(num2, global::Networking.Networking.networkBools[num], global::Networking.Networking.networkShorts[num]);
				}
			}
			global::MainGame.MainGame.haveProgramData = true;
		}
		catch
		{
		}
	}
}
