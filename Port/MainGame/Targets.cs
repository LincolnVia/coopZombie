using System;
using System.Globalization;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using Microsoft.Xna.Framework.Net;
using Networking;
using Players;
using Rendering;
using Structs;
using WindowsGame1;

namespace MainGame;

public class Targets
{
	public static byte numTargets = 0;

	public static byte numAllocatedTargets = 0;

	public static byte numDamageTargets = 0;

	public static byte numAllocatedDamageTargets = 0;

	public static float teamColorR;

	public static float teamColorG;

	public static float teamColorB;

	public static float enemyColorR;

	public static float enemyColorG;

	public static float enemyColorB;

	public static float neutralColorR;

	public static float neutralColorG;

	public static float neutralColorB;

	public static ushort[,] curTarget;

	public static float[,] curTargetDistance;

	public static StructsClass.Target[] targets;

	public static StructsClass.Damage_Target[] damageTargets;

	public Game1.MasterCollection mainC;

	public void Init_Vars(Game1.MasterCollection master)
	{
		mainC = master;
	}

	public void Initialize_Targets()
	{
		curTarget = new ushort[1, 2];
		curTargetDistance = new float[1, 2];
	}

	public void Load_Target_Data(string fileName)
	{
		int num = -1;
		_ = global::Rendering.Rendering.uBufferID;
		for (int i = 0; i < numTargets; i++)
		{
			targets[i].startsEnabled = false;
			targets[i].startsVisible = false;
			targets[i].startsActive = false;
			targets[i].enabled = false;
			targets[i].visible = false;
			targets[i].active = false;
			mainC.physicsMain.Reset_Physics_Movement(ref targets[i].ph1);
			targets[i].pointsF = 0f;
			targets[i].pointsI = 0;
			targets[i].collisionModelID = 0;
			targets[i].callBackType = 0;
			targets[i].callBack = 0;
			targets[i].hitAction = 0;
			targets[i].type = 0;
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
				if (array4[0].Equals("numTargets", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 1;
				}
				else if (array4[0].Equals("Target", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 2;
				}
				else if (array4[0].Equals("Position", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 3;
				}
				else if (array4[0].Equals("Model", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 4;
				}
				else if (array4[0].Equals("Box1", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 5;
				}
				else if (array4[0].Equals("Box2", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 6;
				}
				else if (array4[0].Equals("StartsEnabled", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 7;
				}
				else if (array4[0].Equals("PointsInt", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 8;
				}
				else if (array4[0].Equals("PointsFloat", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 9;
				}
				else if (array4[0].Equals("CallBack", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 10;
				}
				else if (array4[0].Equals("CallBackType", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 11;
				}
				else if (array4[0].Equals("HitAction", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 12;
				}
				else if (array4[0].Equals("ResetTIme", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 13;
				}
				else if (array4[0].Equals("StartTime", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 14;
				}
				else if (array4[0].Equals("ProgramId", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 15;
				}
				else if (array4[0].Equals("StartsVisible", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 16;
				}
				else if (array4[0].Equals("StartsActive", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 17;
				}
				else if (array4[0].Equals("Type", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 18;
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
					if (num5 > numAllocatedTargets)
					{
						targets = new StructsClass.Target[num5];
						for (int i = 0; i < num5; i++)
						{
							targets[i].startsActive = false;
							targets[i].startsEnabled = false;
							targets[i].startsVisible = false;
							mainC.physicsMain.Reset_Physics_Movement(ref targets[i].ph1);
							targets[i].pointsF = 0f;
							targets[i].pointsI = 0;
							targets[i].collisionModelID = 0;
							targets[i].mv = new Matrix[2];
							targets[i].callBackType = 0;
							targets[i].callBack = 0;
							targets[i].hitAction = 0;
							targets[i].type = 0;
						}
						numAllocatedTargets = (byte)num5;
					}
					numTargets = (byte)num5;
					break;
				}
				case 2:
					if (array4.Length > 1)
					{
						num = int.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
						if (num < 0 || num >= numAllocatedTargets)
						{
							num = -1;
						}
						targets[num].enabled = false;
					}
					break;
				case 3:
					if (array4.Length > 3 && num > -1)
					{
						targets[num].ph1.x = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
						targets[num].ph1.y = float.Parse(array4[2], CultureInfo.InvariantCulture.NumberFormat);
						targets[num].ph1.z = float.Parse(array4[3], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 4:
					if (array4.Length > 1 && num > -1)
					{
						targets[num].modelName = array4[1];
						targets[num].modelID = mainC.modelsMain.Find_Model(array4[1]);
					}
					break;
				case 5:
					if (array4.Length > 3 && num > -1)
					{
						targets[num].boxX1 = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
						targets[num].boxY1 = float.Parse(array4[2], CultureInfo.InvariantCulture.NumberFormat);
						targets[num].boxZ1 = float.Parse(array4[3], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 6:
					if (array4.Length > 3 && num > -1)
					{
						targets[num].boxX2 = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
						targets[num].boxY2 = float.Parse(array4[2], CultureInfo.InvariantCulture.NumberFormat);
						targets[num].boxZ2 = float.Parse(array4[3], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 7:
					if (num > -1)
					{
						targets[num].startsEnabled = true;
					}
					break;
				case 8:
					if (array4.Length > 3 && num > -1)
					{
						targets[num].pointsI = ushort.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 9:
					if (array4.Length > 3 && num > -1)
					{
						targets[num].pointsF = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 10:
					if (array4.Length > 1 && num > -1)
					{
						targets[num].callBack = byte.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 11:
					if (array4.Length > 1 && num > -1)
					{
						targets[num].callBackType = byte.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 12:
					if (array4.Length > 1 && num > -1)
					{
						targets[num].hitAction = byte.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 13:
					if (array4.Length > 1 && num > -1)
					{
						targets[num].timeBeforeReset = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 14:
					if (array4.Length > 1 && num > -1)
					{
						targets[num].startTime = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 15:
					if (array4.Length > 1 && num > -1)
					{
						targets[num].programID = ushort.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 16:
					if (num > -1)
					{
						targets[num].startsVisible = true;
					}
					break;
				case 17:
					if (num > -1)
					{
						targets[num].startsActive = true;
					}
					break;
				case 18:
					if (num > -1)
					{
						targets[num].type = byte.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				}
			}
		}
		for (int i = 0; i < numTargets; i++)
		{
			ref Matrix reference = ref targets[i].mv[0];
			reference = Matrix.CreateTranslation(targets[i].ph1.x, targets[i].ph1.y, targets[i].ph1.z);
			ref Matrix reference2 = ref targets[i].mv[1];
			reference2 = targets[i].mv[0];
		}
		stream.Close();
	}

	public void Load_Damage_Target_Data(string fileName)
	{
		int num = -1;
		_ = global::Rendering.Rendering.uBufferID;
		enemyColorR = 1f;
		enemyColorG = 0f;
		enemyColorB = 0f;
		teamColorR = 0f;
		teamColorG = 0f;
		teamColorB = 1f;
		for (int i = 0; i < numDamageTargets; i++)
		{
			damageTargets[i].startsEnabled = false;
			damageTargets[i].startsActive = false;
			damageTargets[i].enabled = false;
			damageTargets[i].active = false;
			damageTargets[i].pointsF = 0f;
			damageTargets[i].pointsI = 0;
			damageTargets[i].callBackType = 0;
			damageTargets[i].callBack = 0;
			damageTargets[i].hitAction = 0;
			damageTargets[i].type = 0;
			damageTargets[i].repairMultiplier = 1f;
			damageTargets[i].showOnMiniMap = false;
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
				if (array4[0].Equals("numDamageTargets", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 1;
				}
				else if (array4[0].Equals("Target", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 2;
				}
				else if (array4[0].Equals("StartsEnabled", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 3;
				}
				else if (array4[0].Equals("PointsInt", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 4;
				}
				else if (array4[0].Equals("PointsFloat", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 5;
				}
				else if (array4[0].Equals("CallBack", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 6;
				}
				else if (array4[0].Equals("CallBackType", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 7;
				}
				else if (array4[0].Equals("HitAction", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 8;
				}
				else if (array4[0].Equals("ResetTIme", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 9;
				}
				else if (array4[0].Equals("StartTime", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 10;
				}
				else if (array4[0].Equals("ProgramId", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 11;
				}
				else if (array4[0].Equals("StartsActive", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 12;
				}
				else if (array4[0].Equals("Type", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 13;
				}
				else if (array4[0].Equals("MaxDamage", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 14;
				}
				else if (array4[0].Equals("RepairMultiplier", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 15;
				}
				else if (array4[0].Equals("Team", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 16;
				}
				else if (array4[0].Equals("MiniMap", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 17;
				}
				else if (array4[0].Equals("TeamColor", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 18;
				}
				else if (array4[0].Equals("EnemyColor", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 19;
				}
				else if (array4[0].Equals("NeutralColor", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 20;
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
					if (num5 > numAllocatedDamageTargets)
					{
						damageTargets = new StructsClass.Damage_Target[num5];
						for (int i = 0; i < num5; i++)
						{
							damageTargets[i].startsActive = false;
							damageTargets[i].startsEnabled = false;
							damageTargets[i].pointsF = 0f;
							damageTargets[i].pointsI = 0;
							damageTargets[i].callBackType = 0;
							damageTargets[i].callBack = 0;
							damageTargets[i].hitAction = 0;
							damageTargets[i].type = 0;
							damageTargets[i].repairMultiplier = 1f;
							damageTargets[i].maxDamage = 100f;
							damageTargets[i].team = 0;
							damageTargets[i].showOnMiniMap = false;
						}
						numAllocatedDamageTargets = (byte)num5;
					}
					numDamageTargets = (byte)num5;
					break;
				}
				case 2:
					if (array4.Length > 1)
					{
						num = int.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
						if (num < 0 || num >= numAllocatedDamageTargets)
						{
							num = -1;
						}
						damageTargets[num].enabled = false;
					}
					break;
				case 3:
					if (num > -1)
					{
						damageTargets[num].startsEnabled = true;
					}
					break;
				case 4:
					if (array4.Length > 1 && num > -1)
					{
						damageTargets[num].pointsI = ushort.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 5:
					if (array4.Length > 1 && num > -1)
					{
						damageTargets[num].pointsF = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 6:
					if (array4.Length > 1 && num > -1)
					{
						damageTargets[num].callBack = byte.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 7:
					if (array4.Length > 1 && num > -1)
					{
						damageTargets[num].callBackType = byte.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 8:
					if (array4.Length > 1 && num > -1)
					{
						damageTargets[num].hitAction = byte.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 9:
					if (array4.Length > 1 && num > -1)
					{
						damageTargets[num].timeBeforeReset = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 10:
					if (array4.Length > 1 && num > -1)
					{
						damageTargets[num].startTime = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 11:
					if (array4.Length > 1 && num > -1)
					{
						damageTargets[num].programID = ushort.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 12:
					if (num > -1)
					{
						damageTargets[num].startsActive = true;
					}
					break;
				case 13:
					if (array4.Length > 1 && num > -1)
					{
						damageTargets[num].type = byte.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 14:
					if (array4.Length > 1 && num > -1)
					{
						damageTargets[num].maxDamage = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 15:
					if (array4.Length > 1 && num > -1)
					{
						damageTargets[num].repairMultiplier = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 16:
					if (array4.Length > 1 && num > -1)
					{
						damageTargets[num].startingTeam = byte.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
						damageTargets[num].team = damageTargets[num].startingTeam;
						damageTargets[num].teamMask = mainC.playersMain.Get_Team_Mask(damageTargets[num].team);
					}
					break;
				case 17:
					if (array4.Length > 4 && num > -1)
					{
						damageTargets[num].showOnMiniMap = true;
						damageTargets[num].miniMapTexture = array4[1];
						damageTargets[num].colorR = float.Parse(array4[2], CultureInfo.InvariantCulture.NumberFormat);
						damageTargets[num].colorG = float.Parse(array4[3], CultureInfo.InvariantCulture.NumberFormat);
						damageTargets[num].colorB = float.Parse(array4[4], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 18:
					if (array4.Length > 3)
					{
						teamColorR = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
						teamColorG = float.Parse(array4[2], CultureInfo.InvariantCulture.NumberFormat);
						teamColorB = float.Parse(array4[3], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 19:
					if (array4.Length > 3)
					{
						enemyColorR = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
						enemyColorG = float.Parse(array4[2], CultureInfo.InvariantCulture.NumberFormat);
						enemyColorB = float.Parse(array4[3], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 20:
					if (array4.Length > 3)
					{
						neutralColorR = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
						neutralColorG = float.Parse(array4[2], CultureInfo.InvariantCulture.NumberFormat);
						neutralColorB = float.Parse(array4[3], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				}
			}
		}
		stream.Close();
	}

	public void Render_Targets()
	{
		byte uBufferID = global::Rendering.Rendering.uBufferID;
		for (ushort num = 0; num < numTargets; num++)
		{
			if (targets[num].active && targets[num].visible)
			{
				mainC.modelsMain.Render_Model(targets[num].modelID, ref targets[num].mv[uBufferID]);
			}
		}
	}

	public void Process_Targets(float frameTime)
	{
		for (ushort num = 0; num < numTargets; num++)
		{
			if (!targets[num].enabled)
			{
				switch (targets[num].hitAction)
				{
				case 0:
					targets[num].curTime -= frameTime;
					if (targets[num].curTime < 0f)
					{
						targets[num].enabled = true;
						targets[num].visible = true;
						targets[num].active = true;
						mainC.programsMain.Reset_Object(targets[num].programID);
					}
					break;
				case 1:
					targets[num].curTime -= frameTime;
					if (targets[num].curTime < 0f)
					{
						targets[num].enabled = true;
						mainC.callbackMain.CallBack(0, targets[num].callBackType, targets[num].callBack, 0, 0);
					}
					break;
				}
			}
		}
	}

	public float Is_Player_Aiming_At_A_Target_New(ushort playerID, float px, float py, float pz, ref Matrix mv, ref float elevationAngle)
	{
		sbyte b = 1;
		float num = 180f;
		byte uBufferID = global::Rendering.Rendering.uBufferID;
		for (ushort num2 = 0; num2 < numTargets; num2++)
		{
			if (targets[num2].enabled)
			{
				float num3 = (targets[num2].boxX2 - targets[num2].boxX1) / 2f;
				float num4 = (targets[num2].boxY2 - targets[num2].boxY1) / 2f;
				float num5 = (targets[num2].boxZ2 - targets[num2].boxZ1) / 2f;
				float num6 = targets[num2].mv[uBufferID].M41 + targets[num2].boxX1 + num3 - px;
				float num7 = targets[num2].mv[uBufferID].M42 + targets[num2].boxY1 + num4 - py;
				float num8 = targets[num2].mv[uBufferID].M43 + targets[num2].boxZ1 + num5 - pz;
				float num9 = (float)Math.Sqrt(num6 * num6 + num7 * num7 + num8 * num8);
				num6 = targets[num2].boxX1 - num3;
				num7 = targets[num2].boxY1 - num4;
				num8 = targets[num2].boxZ1 - num5;
				float num10 = num6 * targets[num2].mv[uBufferID].M11 + num7 * targets[num2].mv[uBufferID].M21 + num8 * targets[num2].mv[uBufferID].M31 + targets[num2].mv[uBufferID].M41;
				float num11 = num6 * targets[num2].mv[uBufferID].M21 + num7 * targets[num2].mv[uBufferID].M22 + num8 * targets[num2].mv[uBufferID].M32 + targets[num2].mv[uBufferID].M42;
				float num12 = num6 * targets[num2].mv[uBufferID].M31 + num7 * targets[num2].mv[uBufferID].M23 + num8 * targets[num2].mv[uBufferID].M33 + targets[num2].mv[uBufferID].M43;
				num6 = targets[num2].boxX2 + num3;
				num7 = targets[num2].boxY2 + num4;
				num8 = targets[num2].boxZ2 + num5;
				float num13 = num6 * targets[num2].mv[uBufferID].M11 + num7 * targets[num2].mv[uBufferID].M21 + num8 * targets[num2].mv[uBufferID].M31 + targets[num2].mv[uBufferID].M41;
				float num14 = num6 * targets[num2].mv[uBufferID].M21 + num7 * targets[num2].mv[uBufferID].M22 + num8 * targets[num2].mv[uBufferID].M32 + targets[num2].mv[uBufferID].M42;
				float num15 = num6 * targets[num2].mv[uBufferID].M31 + num7 * targets[num2].mv[uBufferID].M23 + num8 * targets[num2].mv[uBufferID].M33 + targets[num2].mv[uBufferID].M43;
				num6 = num9 * mv.M21 + px;
				num7 = num9 * mv.M22 + py;
				num8 = num9 * mv.M23 + pz;
				if (num6 > num10 && num6 < num13 && num7 > num11 && num7 < num14 && num8 > num12 && num8 < num15)
				{
					curTargetDistance[playerID, uBufferID] = num9;
					curTarget[playerID, uBufferID] = num2;
					if (num6 > targets[num2].ph1.x)
					{
						b = -1;
					}
					num6 = targets[num2].mv[uBufferID].M41 - px;
					num7 = targets[num2].mv[uBufferID].M42 - py;
					num8 = targets[num2].mv[uBufferID].M43 - pz;
					num12 = num8 / num9;
					elevationAngle = (float)(Math.Asin(num12) - Math.Asin(mv.M23)) * 57.29578f;
					float num16 = (float)Math.Sqrt(num6 * num6 + num7 * num7);
					if (num16 != 0f)
					{
						num10 = num6 / num16;
						num11 = num7 / num16;
						num16 = (float)Math.Sqrt(mv.M21 * mv.M21 + mv.M22 * mv.M22);
						if (num16 != 0f)
						{
							num13 = mv.M21 / num16;
							num14 = mv.M22 / num16;
							num16 = num13 * num10 + num14 * num11;
							num = (float)Math.Acos(num16) * 57.29578f;
							num16 = (float)Math.Acos(num11);
							if (num10 > 0f)
							{
								num16 *= -1f;
							}
							float num17 = (float)Math.Acos(num14);
							if (num13 > 0f)
							{
								num16 *= -1f;
							}
							b = (sbyte)Math.Sign(num16 - num17);
						}
					}
					return num * (float)b;
				}
			}
		}
		return num * (float)b;
	}

	public float Is_Player_Aiming_At_A_Target(ushort playerID, float px, float py, float pz, ref Matrix mv, ref float elevationAngle)
	{
		sbyte b = 1;
		float num = 180f;
		byte uBufferID = global::Rendering.Rendering.uBufferID;
		for (ushort num2 = 0; num2 < numTargets; num2++)
		{
			if (targets[num2].enabled)
			{
				float num3 = targets[num2].mv[uBufferID].M41 - px;
				float num4 = targets[num2].mv[uBufferID].M42 - py;
				float num5 = (float)Math.Sqrt(num3 * num3 + num4 * num4);
				if (num5 != 0f)
				{
					float num6 = num3 / num5;
					float num7 = num4 / num5;
					float num8 = (float)Math.Sqrt(mv.M21 * mv.M21 + mv.M22 * mv.M22);
					if (num8 != 0f)
					{
						float num9 = mv.M21 / num8;
						float num10 = mv.M22 / num8;
						num8 = num9 * num6 + num10 * num7;
						num8 = (float)Math.Acos(num8) * 57.29578f;
						if (num8 < num)
						{
							float num11 = targets[num2].mv[uBufferID].M43 - pz;
							float num12 = (float)Math.Sqrt(num3 * num3 + num4 * num4 + num11 * num11);
							if (num12 != 0f)
							{
								float num13 = num11 / num12;
								num5 = (float)(Math.Asin(num13) - Math.Asin(mv.M23)) * 57.29578f;
								if (Math.Abs(num5) < 15f)
								{
									b = 1;
									num = num8;
									elevationAngle = num5;
									if (num8 < 5f)
									{
										curTargetDistance[playerID, uBufferID] = num12;
									}
									num5 = (float)Math.Acos(num7);
									if (num6 > 0f)
									{
										num5 *= -1f;
									}
									num8 = (float)Math.Acos(num10);
									if (num9 > 0f)
									{
										num5 *= -1f;
									}
									b = (sbyte)Math.Sign(num5 - num8);
								}
							}
						}
					}
				}
			}
		}
		return num * (float)b;
	}

	public bool Check_Collision(float xStart, float yStart, float zStart, float x2, float y2, float z2)
	{
		bool result = false;
		int num = -1;
		byte uBufferID = global::Rendering.Rendering.uBufferID;
		float num2 = x2 - xStart;
		float num3 = y2 - yStart;
		float num4 = z2 - zStart;
		float num5 = (float)Math.Sqrt(num2 * num2 + num3 * num3 + num4 * num4);
		if (num5 == 0f)
		{
			return false;
		}
		num2 /= num5;
		num3 /= num5;
		num4 /= num5;
		for (ushort num6 = 0; num6 < numTargets; num6++)
		{
			if (targets[num6].active)
			{
				Matrix.Invert(ref targets[num6].mv[uBufferID], out var result2);
				float num7 = (xStart - targets[num6].ph1.x) * result2.M11 + (yStart - targets[num6].ph1.y) * result2.M21 + (zStart - targets[num6].ph1.z) * result2.M31;
				float num8 = (xStart - targets[num6].ph1.x) * result2.M12 + (yStart - targets[num6].ph1.y) * result2.M22 + (zStart - targets[num6].ph1.z) * result2.M32;
				float num9 = (xStart - targets[num6].ph1.x) * result2.M13 + (yStart - targets[num6].ph1.y) * result2.M23 + (zStart - targets[num6].ph1.z) * result2.M33;
				float num10 = (num2 * result2.M11 + num3 * result2.M21 + num4 * result2.M31) * 1f;
				float num11 = (num2 * result2.M12 + num3 * result2.M22 + num4 * result2.M32) * 1f;
				float num12 = (num2 * result2.M13 + num3 * result2.M23 + num4 * result2.M33) * 1f;
				float num13 = num5;
				if (num7 > targets[num6].boxX1 && num7 < targets[num6].boxX2 && num8 > targets[num6].boxY1 && num8 < targets[num6].boxY2 && num9 > targets[num6].boxZ1 && num9 < targets[num6].boxZ2)
				{
					result = true;
					num13 = -1f;
					num = num6;
					num6 = numTargets;
				}
				while (num13 > 1f)
				{
					num7 += num10;
					num8 += num11;
					num9 += num12;
					if (num7 > targets[num6].boxX1 && num7 < targets[num6].boxX2 && num8 > targets[num6].boxY1 && num8 < targets[num6].boxY2 && num9 > targets[num6].boxZ1 && num9 < targets[num6].boxZ2)
					{
						result = true;
						num13 = -1f;
						num = num6;
						num6 = numTargets;
					}
					num13 -= 1f;
				}
				if (num13 > 0f)
				{
					num7 += num10 * (num13 / 1f);
					num8 += num11 * (num13 / 1f);
					num9 += num12 * (num13 / 1f);
					if (num7 > targets[num6].boxX1 && num7 < targets[num6].boxX2 && num8 > targets[num6].boxY1 && num8 < targets[num6].boxY2 && num9 > targets[num6].boxZ1 && num9 < targets[num6].boxZ2)
					{
						result = true;
						num = num6;
						num6 = numTargets;
					}
				}
				if (num > -1)
				{
					switch (targets[num].hitAction)
					{
					case 0:
						mainC.callbackMain.CallBack(0, targets[num].callBackType, targets[num].callBack, 0, 0);
						targets[num].active = false;
						targets[num].enabled = false;
						targets[num].visible = false;
						targets[num].curTime = targets[num].timeBeforeReset;
						break;
					case 1:
						if (targets[num].enabled)
						{
							mainC.callbackMain.CallBack(0, targets[num].callBackType, targets[num].callBack, 0, 0);
							targets[num].enabled = false;
							targets[num].curTime = targets[num].timeBeforeReset;
						}
						break;
					}
				}
			}
		}
		return result;
	}

	public void Reset_Round()
	{
		for (ushort num = 0; num < numTargets; num++)
		{
			targets[num].active = targets[num].startsActive;
			targets[num].enabled = targets[num].startsEnabled;
			targets[num].visible = targets[num].startsVisible;
			targets[num].curTime = targets[num].startTime;
		}
		for (ushort num = 0; num < numDamageTargets; num++)
		{
			damageTargets[num].active = damageTargets[num].startsActive;
			damageTargets[num].enabled = damageTargets[num].startsEnabled;
			damageTargets[num].curTime = damageTargets[num].startTime;
			damageTargets[num].curDamage = 0f;
			Update_Damage_Target_Team(num, damageTargets[num].startingTeam);
		}
	}

	public void Set_DamageTarget_Max_Damage(ushort targetID, float damage)
	{
		if (targetID < numDamageTargets)
		{
			damageTargets[targetID].maxDamage = damage;
		}
	}

	public void Update_DamageTarget_MiniMap_Textures()
	{
		for (ushort num = 0; num < numDamageTargets; num++)
		{
			if (damageTargets[num].showOnMiniMap)
			{
				mainC.renderingMain.Update_MiniMap_Item_Texture(damageTargets[num].miniMapItem, damageTargets[num].miniMapTexture);
			}
		}
	}

	public void Add_Damage_Targets_To_Minimap()
	{
		mainC.gameobjectMain.Update_GameObject_Targets();
		for (ushort num = 0; num < numDamageTargets; num++)
		{
			if (damageTargets[num].showOnMiniMap)
			{
				damageTargets[num].miniMapItem = mainC.renderingMain.Add_MiniMap_Item(damageTargets[num].miniMapTexture, damageTargets[num].x, damageTargets[num].y, damageTargets[num].colorR, damageTargets[num].colorG, damageTargets[num].colorB, 2, 1);
			}
		}
	}

	public void Update_Damage_Targets_Color()
	{
		for (ushort num = 0; num < numDamageTargets; num++)
		{
			if (damageTargets[num].showOnMiniMap)
			{
				if ((global::Players.Players.enemyTeamMask & damageTargets[num].teamMask) != 0)
				{
					mainC.renderingMain.Set_MiniMap_Item_Color(damageTargets[num].miniMapItem, enemyColorR, enemyColorG, enemyColorB);
				}
				else if ((global::Players.Players.players[0].teamMask & damageTargets[num].teamMask) != 0)
				{
					mainC.renderingMain.Set_MiniMap_Item_Color(damageTargets[num].miniMapItem, teamColorR, teamColorG, teamColorB);
				}
				else
				{
					mainC.renderingMain.Set_MiniMap_Item_Color(damageTargets[num].miniMapItem, neutralColorR, neutralColorG, neutralColorB);
				}
			}
		}
	}

	public void Update_Damage_Target_Color(ushort targetID)
	{
		if (damageTargets[targetID].showOnMiniMap)
		{
			if ((global::Players.Players.enemyTeamMask & damageTargets[targetID].teamMask) != 0)
			{
				mainC.renderingMain.Set_MiniMap_Item_Color(damageTargets[targetID].miniMapItem, enemyColorR, enemyColorG, enemyColorB);
			}
			else if ((global::Players.Players.players[0].teamMask & damageTargets[targetID].teamMask) != 0)
			{
				mainC.renderingMain.Set_MiniMap_Item_Color(damageTargets[targetID].miniMapItem, teamColorR, teamColorG, teamColorB);
			}
			else
			{
				mainC.renderingMain.Set_MiniMap_Item_Color(damageTargets[targetID].miniMapItem, neutralColorR, neutralColorG, neutralColorB);
			}
		}
	}

	public void Update_Damage_Target_Team(ushort targetID, ushort teamID)
	{
		if (targetID < numDamageTargets)
		{
			damageTargets[targetID].team = (byte)teamID;
			damageTargets[targetID].teamMask = mainC.playersMain.Get_Team_Mask(damageTargets[targetID].team);
			Update_Damage_Target_Color(targetID);
		}
	}

	public void Set_DamageTarget_Location(ushort targetID, float x, float y, float z)
	{
		damageTargets[targetID].x = x;
		damageTargets[targetID].y = y;
		damageTargets[targetID].z = z;
	}

	public void Set_DamageTarget_Damage(ushort targetID, float damage)
	{
		if (targetID >= numDamageTargets)
		{
			return;
		}
		damageTargets[targetID].curDamage = damage;
		if (damageTargets[targetID].showOnMiniMap)
		{
			if (damageTargets[targetID].curDamage >= damageTargets[targetID].maxDamage)
			{
				mainC.renderingMain.Hide_MiniMap_Item(damageTargets[targetID].miniMapItem);
			}
			else
			{
				mainC.renderingMain.Show_MiniMap_Item(damageTargets[targetID].miniMapItem);
			}
		}
	}

	public void Update_Target_Location(ushort targetID, float x, float y, float z, float xRot, float yRot, float zRot)
	{
		targets[targetID].ph1.x = x;
		targets[targetID].ph1.y = y;
		targets[targetID].ph1.z = z;
		switch (targets[targetID].type)
		{
		case 0:
		{
			ref Matrix reference2 = ref targets[targetID].mv[global::Rendering.Rendering.uBufferID];
			reference2 = Matrix.CreateRotationX(xRot * ((float)Math.PI / 180f)) * Matrix.CreateRotationY(yRot * ((float)Math.PI / 180f)) * Matrix.CreateRotationZ(zRot * ((float)Math.PI / 180f)) * Matrix.CreateTranslation(x, y, z);
			break;
		}
		case 1:
		{
			ref Matrix reference = ref targets[targetID].mv[global::Rendering.Rendering.uBufferID];
			reference = Matrix.CreateRotationY(yRot * ((float)Math.PI / 180f)) * Matrix.CreateRotationZ(zRot * ((float)Math.PI / 180f)) * Matrix.CreateRotationX(xRot * ((float)Math.PI / 180f)) * Matrix.CreateTranslation(x, y, z);
			break;
		}
		}
	}

	public float Update_Damage_Target(ushort targetID, float damage)
	{
		if (targetID < numDamageTargets)
		{
			damageTargets[targetID].curDamage += damage;
			if (damage >= 0f)
			{
				if (damageTargets[targetID].curDamage >= damageTargets[targetID].maxDamage)
				{
					damage -= damageTargets[targetID].curDamage - damageTargets[targetID].maxDamage;
					damageTargets[targetID].curDamage = damageTargets[targetID].maxDamage;
					if (damageTargets[targetID].showOnMiniMap)
					{
						mainC.renderingMain.Hide_MiniMap_Item(damageTargets[targetID].miniMapItem);
					}
					mainC.callbackMain.CallBack(0, damageTargets[targetID].callBackType, damageTargets[targetID].callBack, 0, 0);
				}
				return damage / damageTargets[targetID].maxDamage * damageTargets[targetID].pointsF;
			}
			if (damageTargets[targetID].curDamage <= 0f)
			{
				damage -= damageTargets[targetID].curDamage;
				damageTargets[targetID].curDamage = 0f;
				mainC.callbackMain.CallBack(0, damageTargets[targetID].callBackType, damageTargets[targetID].callBack, 0, 0);
			}
			if (damageTargets[targetID].showOnMiniMap)
			{
				mainC.renderingMain.Show_MiniMap_Item(damageTargets[targetID].miniMapItem);
			}
			return (0f - damage) / damageTargets[targetID].maxDamage * damageTargets[targetID].pointsF * damageTargets[targetID].repairMultiplier;
		}
		return 0f;
	}

	public ushort Count_Team_Damage_Targets(ulong teamMask)
	{
		ushort num = 0;
		for (ushort num2 = 0; num2 < numDamageTargets; num2++)
		{
			if ((mainC.playersMain.Get_Team_Mask(damageTargets[num2].team) & teamMask) != 0 && damageTargets[num2].curDamage < damageTargets[num2].maxDamage)
			{
				num++;
			}
		}
		return num;
	}

	public byte Get_Team_For_Target(ushort targetID)
	{
		if (targetID < numDamageTargets)
		{
			return damageTargets[targetID].team;
		}
		return 0;
	}

	public bool Get_Damage_Target_Status(ushort targetID)
	{
		if (damageTargets[targetID].curDamage < damageTargets[targetID].maxDamage)
		{
			return true;
		}
		return false;
	}

	public int Get_Damage_Target_Points(ushort targetID)
	{
		return damageTargets[targetID].pointsI;
	}

	public void Send_DamageTargets_To_New_Player(NetworkGamer newGamer)
	{
		if (numDamageTargets >= 1)
		{
			global::Networking.Networking.networkUShorts[0] = numDamageTargets;
			for (ushort num = 0; num < numDamageTargets; num++)
			{
				ref HalfSingle reference = ref global::Networking.Networking.networkHS[num];
				reference = new HalfSingle(damageTargets[num].curDamage);
			}
			mainC.networkingMain.XBOX_Send_Network_Message_To_Gamer(46, newGamer);
		}
	}

	public void Receive_DamageTargets_From_Host()
	{
		numDamageTargets = (byte)global::Networking.Networking.networkUShorts[0];
		for (ushort num = 0; num < numDamageTargets; num++)
		{
			damageTargets[num].curDamage = global::Networking.Networking.networkHS[num].ToSingle();
		}
	}
}
