using System;
using System.Globalization;
using System.IO;
using Microsoft.Xna.Framework;
using Rendering;
using Structs;
using WindowsGame1;

namespace MainGame;

public class Graphs
{
	public static short numGraphs = 0;

	public static short numAllocatedGraphs = 0;

	public static StructsClass.Graphs[] graph1;

	public Game1.MasterCollection mainC;

	public void Init_Vars(Game1.MasterCollection master)
	{
		mainC = master;
	}

	public void Init_Graphs()
	{
	}

	public void Load_Graph_Data(string fileName)
	{
		int num = -1;
		_ = global::Rendering.Rendering.uBufferID;
		for (int i = 0; i < numGraphs; i++)
		{
			graph1[i].numPoints = 0;
		}
		Stream stream = TitleContainer.OpenStream("The_CoOp_Zombie_Game\\Config_Files\\" + fileName);
		byte[] array = new byte[stream.Length];
		if (stream.CanRead)
		{
			stream.Read(array, 0, array.Length);
			string[] array2 = mainC.utilMain.Byte_Array_To_String(array).Split(new char[2] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
			int num2 = array2.Length;
			for (int j = 0; j < num2; j++)
			{
				string[] array3 = array2[j].Split(new char[2] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
				if (array3.Length < 1)
				{
					continue;
				}
				int num3 = 0;
				if (array3[0].Equals("numGraphs", StringComparison.OrdinalIgnoreCase))
				{
					num3 = 1;
				}
				else if (array3[0].Equals("Graph", StringComparison.OrdinalIgnoreCase))
				{
					num3 = 2;
				}
				else if (array3[0].Equals("Type", StringComparison.OrdinalIgnoreCase))
				{
					num3 = 3;
				}
				else if (array3[0].Equals("Data", StringComparison.OrdinalIgnoreCase))
				{
					num3 = 4;
				}
				else if (array3[0].Equals("AxisRange", StringComparison.OrdinalIgnoreCase))
				{
					num3 = 5;
				}
				switch (num3)
				{
				case 1:
				{
					if (array3.Length <= 1)
					{
						break;
					}
					int num4 = int.Parse(array3[1], CultureInfo.InvariantCulture.NumberFormat);
					if (num4 > numAllocatedGraphs)
					{
						graph1 = new StructsClass.Graphs[num4];
						for (int i = 0; i < num4; i++)
						{
							graph1[i].type = byte.MaxValue;
							graph1[i].numPoints = 0;
							graph1[i].numAllocatedPts = 0;
						}
						numAllocatedGraphs = (byte)num4;
					}
					numGraphs = (byte)num4;
					break;
				}
				case 2:
					num++;
					if (num < 0 || num >= numGraphs)
					{
						num = -1;
					}
					break;
				case 3:
					if (array3.Length > 1 && num > -1)
					{
						byte b = byte.Parse(array3[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
						if (b != graph1[num].type)
						{
							graph1[num].numPoints = 0;
							graph1[num].numAllocatedPts = 0;
							graph1[num].floatVars = null;
							graph1[num].intVars = null;
						}
						graph1[num].type = b;
					}
					break;
				case 4:
				{
					if (array3.Length <= 2 || num <= -1)
					{
						break;
					}
					int num4 = short.Parse(array3[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
					switch (graph1[num].type)
					{
					case 0:
						if (array3.Length == num4 * 2 + 2)
						{
							if (graph1[num].numAllocatedPts < num4)
							{
								graph1[num].floatVars = new float[num4 * 2];
								graph1[num].intVars = null;
								graph1[num].numAllocatedPts = (ushort)num4;
							}
							graph1[num].numPoints = (ushort)num4;
							int i = 0;
							int num5 = 0;
							int num6 = 2;
							for (; i < num4; i++)
							{
								graph1[num].floatVars[num5++] = float.Parse(array3[num6++], CultureInfo.InvariantCulture.NumberFormat);
								graph1[num].floatVars[num5++] = float.Parse(array3[num6++], CultureInfo.InvariantCulture.NumberFormat);
							}
						}
						break;
					case 1:
						if (array3.Length == num4 * 3 + 2)
						{
							if (graph1[num].numAllocatedPts < num4)
							{
								graph1[num].floatVars = new float[num4 * 3];
								graph1[num].intVars = null;
								graph1[num].numAllocatedPts = (ushort)num4;
							}
							graph1[num].numPoints = (ushort)num4;
							int i = 0;
							int num5 = 0;
							int num6 = 2;
							for (; i < num4; i++)
							{
								graph1[num].floatVars[num5++] = float.Parse(array3[num6++], CultureInfo.InvariantCulture.NumberFormat);
								graph1[num].floatVars[num5++] = float.Parse(array3[num6++], CultureInfo.InvariantCulture.NumberFormat);
								graph1[num].floatVars[num5++] = float.Parse(array3[num6++], CultureInfo.InvariantCulture.NumberFormat);
							}
						}
						break;
					case 2:
						if (array3.Length == num4 * 2 + 2)
						{
							if (graph1[num].numAllocatedPts < num4)
							{
								graph1[num].intVars = new int[num4 * 2];
								graph1[num].floatVars = null;
								graph1[num].numAllocatedPts = (ushort)num4;
							}
							graph1[num].numPoints = (ushort)num4;
							int i = 0;
							int num5 = 0;
							int num6 = 2;
							for (; i < num4; i++)
							{
								graph1[num].intVars[num5++] = int.Parse(array3[num6++], CultureInfo.InvariantCulture.NumberFormat);
								graph1[num].intVars[num5++] = int.Parse(array3[num6++], CultureInfo.InvariantCulture.NumberFormat);
							}
						}
						break;
					case 3:
						if (array3.Length == num4 * 3 + 2)
						{
							if (graph1[num].numAllocatedPts < num4)
							{
								graph1[num].intVars = new int[num4 * 3];
								graph1[num].floatVars = null;
								graph1[num].numAllocatedPts = (ushort)num4;
							}
							graph1[num].numPoints = (ushort)num4;
							int i = 0;
							int num5 = 0;
							int num6 = 2;
							for (; i < num4; i++)
							{
								graph1[num].intVars[num5++] = int.Parse(array3[num6++], CultureInfo.InvariantCulture.NumberFormat);
								graph1[num].intVars[num5++] = int.Parse(array3[num6++], CultureInfo.InvariantCulture.NumberFormat);
								graph1[num].intVars[num5++] = int.Parse(array3[num6++], CultureInfo.InvariantCulture.NumberFormat);
							}
						}
						break;
					}
					break;
				}
				case 5:
					if (num <= -1)
					{
						break;
					}
					switch (graph1[num].type)
					{
					case 0:
						if (array3.Length > 4 && num > -1)
						{
							graph1[num].floatMinX = float.Parse(array3[1], CultureInfo.InvariantCulture.NumberFormat);
							graph1[num].floatMaxX = float.Parse(array3[2], CultureInfo.InvariantCulture.NumberFormat);
							graph1[num].floatMinY = float.Parse(array3[3], CultureInfo.InvariantCulture.NumberFormat);
							graph1[num].floatMaxY = float.Parse(array3[4], CultureInfo.InvariantCulture.NumberFormat);
						}
						break;
					case 1:
						if (array3.Length > 6 && num > -1)
						{
							graph1[num].floatMinX = float.Parse(array3[1], CultureInfo.InvariantCulture.NumberFormat);
							graph1[num].floatMaxX = float.Parse(array3[2], CultureInfo.InvariantCulture.NumberFormat);
							graph1[num].floatMinY = float.Parse(array3[3], CultureInfo.InvariantCulture.NumberFormat);
							graph1[num].floatMaxY = float.Parse(array3[4], CultureInfo.InvariantCulture.NumberFormat);
							graph1[num].floatMinZ = float.Parse(array3[5], CultureInfo.InvariantCulture.NumberFormat);
							graph1[num].floatMaxZ = float.Parse(array3[6], CultureInfo.InvariantCulture.NumberFormat);
						}
						break;
					case 2:
						if (array3.Length > 4 && num > -1)
						{
							graph1[num].intMinX = int.Parse(array3[1], CultureInfo.InvariantCulture.NumberFormat);
							graph1[num].intMaxX = int.Parse(array3[2], CultureInfo.InvariantCulture.NumberFormat);
							graph1[num].intMinY = int.Parse(array3[3], CultureInfo.InvariantCulture.NumberFormat);
							graph1[num].intMaxY = int.Parse(array3[4], CultureInfo.InvariantCulture.NumberFormat);
						}
						break;
					case 3:
						if (array3.Length > 6 && num > -1)
						{
							graph1[num].intMinX = int.Parse(array3[1], CultureInfo.InvariantCulture.NumberFormat);
							graph1[num].intMaxX = int.Parse(array3[2], CultureInfo.InvariantCulture.NumberFormat);
							graph1[num].intMinY = int.Parse(array3[3], CultureInfo.InvariantCulture.NumberFormat);
							graph1[num].intMaxY = int.Parse(array3[4], CultureInfo.InvariantCulture.NumberFormat);
							graph1[num].intMinZ = int.Parse(array3[5], CultureInfo.InvariantCulture.NumberFormat);
							graph1[num].intMaxZ = int.Parse(array3[6], CultureInfo.InvariantCulture.NumberFormat);
						}
						break;
					}
					break;
				}
			}
		}
		stream.Close();
	}

	public float Get_Point_2D_Float_Graph_Fixed_Width_X_Axis(float x, float y, byte axis, short graphID)
	{
		if (graphID >= numGraphs || graphID < 0)
		{
			return 0f;
		}
		if (axis == 0)
		{
			if (graph1[graphID].numPoints == 1)
			{
				return graph1[graphID].floatVars[1];
			}
			if (graph1[graphID].numPoints < 1)
			{
				return 0f;
			}
			int num = graph1[graphID].numPoints - 1;
			if (x < graph1[graphID].floatMinX)
			{
				return graph1[graphID].floatVars[1];
			}
			if (x > graph1[graphID].floatMaxX)
			{
				return graph1[graphID].floatVars[num * 2 + 1];
			}
			float num2 = (graph1[graphID].floatMaxX - graph1[graphID].floatMinX) / (float)num;
			int num3 = (int)Math.Floor((x - graph1[graphID].floatMinX) / num2);
			int num4 = num3 * 2 + 1;
			float num5 = graph1[graphID].floatVars[num4];
			if (num3 == num)
			{
				return num5;
			}
			float num6 = (x - (graph1[graphID].floatMinX + (float)num3 * num2)) / num2;
			return num5 + (graph1[graphID].floatVars[num4 + 2] - num5) * num6;
		}
		return 0f;
	}

	public float Get_Point_2D_Float_Graph(float x, float y, byte axis, short graphID)
	{
		if (graphID >= numGraphs || graphID < 0)
		{
			return 0f;
		}
		if (axis == 0)
		{
			if (graph1[graphID].numPoints == 1 || x <= graph1[graphID].floatMinX || x <= graph1[graphID].floatVars[0])
			{
				return graph1[graphID].floatVars[1];
			}
			int num = graph1[graphID].numPoints - 1;
			if (x >= graph1[graphID].floatMaxX || x >= graph1[graphID].floatVars[num * 2])
			{
				return graph1[graphID].floatVars[num * 2 + 1];
			}
			int i = 1;
			for (int numPoints = graph1[graphID].numPoints; i < numPoints && !(graph1[graphID].floatVars[i * 2] >= x); i++)
			{
			}
			int num2 = (i - 1) * 2;
			i *= 2;
			float num3 = (x - graph1[graphID].floatVars[num2]) / (graph1[graphID].floatVars[i] - graph1[graphID].floatVars[num2]);
			i++;
			num2++;
			return graph1[graphID].floatVars[num2] + (graph1[graphID].floatVars[i] - graph1[graphID].floatVars[num2]) * num3;
		}
		return 0f;
	}

	public void Get_Graph_Point_Float_2D(ushort point, out float x, out float y, ushort graphID)
	{
		if (graphID >= numGraphs || point >= graph1[graphID].numPoints)
		{
			x = 0f;
			y = 0f;
		}
		else
		{
			x = graph1[graphID].floatVars[point * 2];
			y = graph1[graphID].floatVars[point * 2 + 1];
		}
	}

	public void Get_Graph_Float_2D_Axis(out float x1, out float x2, out float y1, out float y2, ushort graphID)
	{
		if (graphID >= numGraphs)
		{
			x1 = 0f;
			x2 = 0f;
			y1 = 0f;
			y2 = 0f;
		}
		else
		{
			x1 = graph1[graphID].floatMinX;
			x2 = graph1[graphID].floatMaxX;
			y1 = graph1[graphID].floatMinY;
			y2 = graph1[graphID].floatMaxY;
		}
	}

	public ushort Get_Number_Graph_Points(ushort graphID)
	{
		if (graphID >= numGraphs)
		{
			return 0;
		}
		return graph1[graphID].numPoints;
	}
}
