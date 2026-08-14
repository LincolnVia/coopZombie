using System;
using System.IO;
using Microsoft.Xna.Framework;
using Structs;

namespace Util;

public class Util
{
	public static byte numDigits;

	public static byte[] digits = new byte[6];

	public static uint maxUnsignedIntValue = 0u;

	public static uint maxIntValue = 0u;

	public static ushort maxUnsignedShortValue = 0;

	public static ushort maxShortValue = 0;

	public static ulong maxUnsignedLongValue = 0uL;

	public static ulong maxLongValue = 0uL;

	public static byte floatSize;

	public static byte floatSizeMinusOne;

	public static byte intSize;

	public static byte intSizeMinusOne;

	public static byte ushortSize;

	public static byte ushortSizeMinusOne;

	public static byte[] floatsToSwap;

	public static byte[] intsToSwap;

	public static byte[] ushortsToSwap;

	private static StructsClass.vtex ctX1 = new StructsClass.vtex();

	private static StructsClass.vtex ctX2 = new StructsClass.vtex();

	private static StructsClass.vtex ctX3 = new StructsClass.vtex();

	private static StructsClass.vtex pdV1 = new StructsClass.vtex();

	private static StructsClass.vtex cpnVp1 = new StructsClass.vtex();

	private static StructsClass.vtex cpnVp2 = new StructsClass.vtex();

	public void Init_Utility()
	{
		maxShortValue = (ushort)(~maxShortValue);
		maxUnsignedShortValue = maxShortValue;
		maxShortValue >>= 1;
		maxIntValue = ~maxIntValue;
		maxUnsignedIntValue = maxIntValue;
		maxIntValue >>= 1;
		maxLongValue = ~maxLongValue;
		maxUnsignedLongValue = maxLongValue;
		maxLongValue >>= 1;
		intSize = 4;
		intSizeMinusOne = (byte)(intSize - 1);
		floatSize = 4;
		floatSizeMinusOne = (byte)(floatSize - 1);
		ushortSize = 2;
		ushortSizeMinusOne = (byte)(ushortSize - 1);
		floatsToSwap = new byte[floatSize];
		intsToSwap = new byte[intSize];
		ushortsToSwap = new byte[ushortSize];
	}

	public void String_Sort(ref string[] incomingString)
	{
		int num = 0;
		int num2 = incomingString.Length - 1;
		while (num == 0)
		{
			num = 1;
			int num3 = 0;
			int num4 = 1;
			while (num3 < num2)
			{
				if (string.Compare(incomingString[num3], incomingString[num4], StringComparison.InvariantCultureIgnoreCase) > 0)
				{
					num = 0;
					string text = incomingString[num3];
					incomingString[num3] = incomingString[num4];
					incomingString[num4] = text;
				}
				num3++;
				num4++;
			}
		}
	}

	public void String_Trim(ref string[] incomingString)
	{
		int num = incomingString.Length;
		for (int i = 0; i < num; i++)
		{
			incomingString[i] = incomingString[i].Trim();
		}
	}

	public void String_Add(ref string[] outputString, string[] stringToAdd)
	{
		int num = outputString.Length;
		int num2 = stringToAdd.Length;
		string[] array = new string[num + num2];
		int i;
		for (i = 0; i < num; i++)
		{
			array[i] = outputString[i];
		}
		num = 0;
		while (num < num2)
		{
			array[i] = stringToAdd[num];
			num++;
			i++;
		}
		outputString = array;
	}

	public void String_Add_Single(ref string[] outputString, string stringToAdd)
	{
		int num = outputString.Length;
		string[] array = new string[num + 1];
		int i;
		for (i = 0; i < num; i++)
		{
			array[i] = outputString[i];
		}
		array[i] = stringToAdd;
		outputString = array;
	}

	public void String_Remove_First(ref string[] outputString)
	{
		int num = 0;
		int num2 = outputString.Length;
		string[] array = new string[num2 - 1];
		int num3 = 1;
		while (num3 < num2)
		{
			array[num] = outputString[num3];
			num3++;
			num++;
		}
		outputString = array;
	}

	public int String_Remove_Duplicates(ref string[] outputString)
	{
		int num = 1;
		int num2 = outputString.Length;
		if (num2 < 1)
		{
			return 0;
		}
		string[] array = new string[num2];
		array[0] = outputString[0];
		int num3 = 1;
		int num4 = 1;
		int num5 = 0;
		while (num4 < num2)
		{
			if (string.Compare(outputString[num4], outputString[num5], StringComparison.InvariantCultureIgnoreCase) != 0)
			{
				array[num++] = outputString[num4];
				num3++;
			}
			num4++;
			num5++;
		}
		outputString = new string[num3];
		for (num4 = 0; num4 < num3; num4++)
		{
			outputString[num4] = array[num4];
		}
		return num3;
	}

	public int String_Find(string[] inputString, string stringToFind, int stringLength)
	{
		for (int i = 0; i < stringLength; i++)
		{
			if (inputString[i].Equals(stringToFind, StringComparison.OrdinalIgnoreCase))
			{
				return i;
			}
		}
		return -1;
	}

	public string Byte_Array_To_String(byte[] buffer)
	{
		int num = buffer.Length;
		char[] array = new char[num];
		for (int i = 0; i < num; i++)
		{
			array[i] = (char)buffer[i];
		}
		return new string(array);
	}

	public byte[] String_To_Byte_Array(string str1)
	{
		int length = str1.Length;
		byte[] array = new byte[length];
		for (int i = 0; i < length; i++)
		{
			array[i] = (byte)str1[i];
		}
		return array;
	}

	public static void Vertex_Subtract(ref StructsClass.vtex v1, ref StructsClass.vtex v2, ref StructsClass.vtex result)
	{
		result.v[0] = v1.v[0] - v2.v[0];
		result.v[1] = v1.v[1] - v2.v[1];
		result.v[2] = v1.v[2] - v2.v[2];
	}

	public static void Vertex_Multiply(float f1, ref StructsClass.vtex result)
	{
		result.v[0] = result.v[0] * f1;
		result.v[1] = result.v[1] * f1;
		result.v[2] = result.v[2] * f1;
	}

	public static void Vertex_Divide(float f1, ref StructsClass.vtex result)
	{
		result.v[0] = result.v[0] / f1;
		result.v[1] = result.v[1] / f1;
		result.v[2] = result.v[2] / f1;
	}

	public void Calc_Tangent(ref StructsClass.vtex v1, ref StructsClass.vtex v2, ref StructsClass.vtex v3, ref StructsClass.texcoord t1, ref StructsClass.texcoord t2, ref StructsClass.texcoord t3, ref StructsClass.vtex tan)
	{
		Vertex_Subtract(ref v2, ref v1, ref ctX1);
		Vertex_Subtract(ref v3, ref v1, ref ctX2);
		float num = t3.t[1] - t1.t[1];
		float num2 = t2.t[1] - t1.t[1];
		float num3 = t2.t[0] - t1.t[0];
		float num4 = t3.t[0] - t1.t[0];
		Vertex_Multiply(num, ref ctX1);
		Vertex_Multiply(num2, ref ctX2);
		Vertex_Subtract(ref ctX1, ref ctX2, ref ctX3);
		float num5 = num3 * num;
		float num6 = num2 * num4;
		float f = num5 - num6;
		Vertex_Divide(f, ref ctX3);
		tan.v[0] = ctX3.v[0];
		tan.v[1] = ctX3.v[1];
		tan.v[2] = ctX3.v[2];
	}

	public void Calc_BiTangent(ref StructsClass.vtex v1, ref StructsClass.vtex v2, ref StructsClass.vtex v3, ref StructsClass.texcoord t1, ref StructsClass.texcoord t2, ref StructsClass.texcoord t3, ref StructsClass.vtex bitan)
	{
		Vertex_Subtract(ref v2, ref v1, ref ctX1);
		Vertex_Subtract(ref v3, ref v1, ref ctX2);
		float num = t3.t[0] - t1.t[0];
		float num2 = t2.t[0] - t1.t[0];
		float num3 = t2.t[1] - t1.t[1];
		float num4 = t3.t[1] - t1.t[1];
		Vertex_Multiply(num, ref ctX1);
		Vertex_Multiply(num2, ref ctX2);
		Vertex_Subtract(ref ctX1, ref ctX2, ref ctX3);
		float num5 = num3 * num;
		float num6 = num2 * num4;
		float f = num5 - num6;
		Vertex_Divide(f, ref ctX3);
		bitan.v[0] = ctX3.v[0];
		bitan.v[1] = ctX3.v[1];
		bitan.v[2] = ctX3.v[2];
	}

	public static void Normalize(ref StructsClass.vnorm n1)
	{
		float num = (float)Math.Sqrt(n1.n[0] * n1.n[0] + n1.n[1] * n1.n[1] + n1.n[2] * n1.n[2]);
		n1.n[0] /= num;
		n1.n[1] /= num;
		n1.n[2] /= num;
	}

	public static void NormalizeVertex(ref StructsClass.vtex v1)
	{
		float num = (float)Math.Sqrt(v1.v[0] * v1.v[0] + v1.v[1] * v1.v[1] + v1.v[2] * v1.v[2]);
		if (num > 0f)
		{
			v1.v[0] /= num;
			v1.v[1] /= num;
			v1.v[2] /= num;
		}
	}

	public static void CrossProduct(StructsClass.vtex vt1, StructsClass.vtex vt2, ref StructsClass.vtex vt3)
	{
		vt3.v[0] = vt1.v[1] * vt2.v[2] - vt1.v[2] * vt2.v[1];
		vt3.v[1] = vt1.v[2] * vt2.v[0] - vt1.v[0] * vt2.v[2];
		vt3.v[2] = vt1.v[0] * vt2.v[1] - vt1.v[1] * vt2.v[0];
		float num = (float)Math.Sqrt(vt3.v[0] * vt3.v[0] + vt3.v[1] * vt3.v[1] + vt3.v[2] * vt3.v[2]);
		vt3.v[0] /= num;
		vt3.v[1] /= num;
		vt3.v[2] /= num;
	}

	public void crossProductNorm(ref StructsClass.vtex v1, ref StructsClass.vtex v2, ref StructsClass.vtex v3, ref StructsClass.vnorm n1)
	{
		cpnVp1.v[0] = v1.v[0] - v2.v[0];
		cpnVp1.v[1] = v1.v[1] - v2.v[1];
		cpnVp1.v[2] = v1.v[2] - v2.v[2];
		cpnVp2.v[0] = v2.v[0] - v3.v[0];
		cpnVp2.v[1] = v2.v[1] - v3.v[1];
		cpnVp2.v[2] = v2.v[2] - v3.v[2];
		n1.n[0] = cpnVp1.v[1] * cpnVp2.v[2] - cpnVp1.v[2] * cpnVp2.v[1];
		n1.n[1] = cpnVp1.v[2] * cpnVp2.v[0] - cpnVp1.v[0] * cpnVp2.v[2];
		n1.n[2] = cpnVp1.v[0] * cpnVp2.v[1] - cpnVp1.v[1] * cpnVp2.v[0];
		float num = (float)Math.Sqrt(n1.n[0] * n1.n[0] + n1.n[1] * n1.n[1] + n1.n[2] * n1.n[2]);
		if (num > 0f)
		{
			n1.n[0] /= num;
			n1.n[1] /= num;
			n1.n[2] /= num;
		}
	}

	public Matrix Matrix_From_Unit_Vector(float x, float y, float z)
	{
		Matrix result = Matrix.Identity;
		float num = (float)Math.Sqrt(x * x + y * y);
		if (num != 0f)
		{
			num = (float)Math.Acos(y / num);
			if (x > 0f)
			{
				num = (float)Math.PI * 2f - num;
			}
			result = Matrix.CreateRotationX((float)Math.Asin(z)) * Matrix.CreateRotationZ(num);
			num = (float)Math.Asin(z) * 57.29578f;
		}
		return result;
	}

	public Matrix Matrix_From_Non_Unit_Vector(float x, float y, float z)
	{
		Matrix result = Matrix.Identity;
		float num = (float)Math.Sqrt(x * x + y * y + z * z);
		if (num != 0f)
		{
			z /= num;
		}
		num = (float)Math.Sqrt(x * x + y * y);
		if (num != 0f)
		{
			num = (float)Math.Acos(y / num);
			if (x > 0f)
			{
				num = (float)Math.PI * 2f - num;
			}
			result = Matrix.CreateRotationX((float)Math.Asin(z)) * Matrix.CreateRotationZ(num);
			num = (float)Math.Asin(z) * 57.29578f;
		}
		return result;
	}

	public float Get_Z_Rotation_Of_Matrix(Matrix mv)
	{
		if (Math.Abs(mv.M23) == 1f)
		{
			return 0f;
		}
		float m = mv.M21;
		float m2 = mv.M22;
		float num = (float)Math.Sqrt(m * m + m2 * m2);
		if (num != 0f)
		{
			m2 /= num;
			if (m2 > 1f)
			{
				m2 = 1f;
			}
			m = (float)Math.Acos(m2) * 57.29578f;
			if (mv.M21 > 0f)
			{
				m = 360f - m;
			}
			return m;
		}
		return 0f;
	}

	public float Get_X_Rotation_Of_Matrix(Matrix mv)
	{
		return (float)Math.Asin(mv.M23) * 57.29578f;
	}

	public float Get_Y_Rotation_Of_Matrix(Matrix mv)
	{
		sbyte b = 1;
		float num2;
		float num;
		if (Math.Abs(mv.M23) == 1f)
		{
			num = (float)(-Math.Sign(mv.M23)) * mv.M32;
			if (Math.Abs(num) > 1f)
			{
				num = Math.Sign(num);
			}
			num2 = (float)Math.Acos(num) * 57.29578f;
			if (mv.M31 < 0f)
			{
				num2 = 360f - num2;
			}
			return num2;
		}
		if (Math.Abs(mv.M23) == 0f)
		{
			num = 0f - mv.M13;
			if (Math.Abs(num) > 1f)
			{
				num = Math.Sign(num);
			}
			num2 = (float)Math.Asin(num) * 57.29578f;
			if (mv.M33 < 0f)
			{
				num2 = 180f * (float)Math.Sign(num2) - num2;
			}
			return num2;
		}
		num2 = mv.M21;
		num = mv.M22;
		float m = mv.M23;
		float num3 = num2;
		float num4 = num;
		float num5 = 0f;
		float num6 = (0f - m) * num4;
		float num7 = m * num3;
		float num8 = num2 * num4 - num * num3;
		num3 = num6;
		num4 = num7;
		num5 = num8;
		num6 = (float)Math.Sqrt(num3 * num3 + num4 * num4 + num5 * num5);
		if (num6 != 0f)
		{
			num3 /= num6;
			num4 /= num6;
			num5 /= num6;
		}
		if (mv.M23 > 0f)
		{
			b = -1;
		}
		num6 = (float)b * mv.M12 * num5 - (float)b * mv.M13 * num4;
		num7 = (float)b * mv.M13 * num3 - (float)b * mv.M11 * num5;
		num8 = (float)b * mv.M11 * num4 - (float)b * mv.M12 * num3;
		num = (float)Math.Sqrt(num6 * num6 + num7 * num7 + num8 * num8);
		if (Math.Abs(num) > 1f)
		{
			num = Math.Sign(num);
		}
		num2 = (float)Math.Asin(num) * 57.29578f;
		num = num3 * (float)b * mv.M11 + num4 * (float)b * mv.M12 + num5 * (float)b * mv.M13;
		if (num < 0f)
		{
			num2 = ((num2 == 0f) ? 180f : (180f * (float)Math.Sign(num2) - num2));
		}
		m = num6 * mv.M21 + num7 * mv.M22 + num8 * mv.M23;
		if (m != 0f)
		{
			num2 = (float)(-Math.Sign(m)) * num2;
		}
		return num2;
	}

	public void Get_Digit_Indexes(int number)
	{
		number = Math.Abs(number);
		if (number < 1000000)
		{
			digits[5] = (byte)(number / 100000);
			number -= 100000 * digits[5];
			digits[4] = (byte)(number / 10000);
			number -= 10000 * digits[4];
			digits[3] = (byte)(number / 1000);
			number -= 1000 * digits[3];
			digits[2] = (byte)(number / 100);
			number -= 100 * digits[2];
			digits[1] = (byte)(number / 10);
			number -= 10 * digits[1];
			digits[0] = (byte)number;
			if (digits[5] > 0)
			{
				numDigits = 6;
			}
			else if (digits[4] > 0)
			{
				numDigits = 5;
			}
			else if (digits[3] > 0)
			{
				numDigits = 4;
			}
			else if (digits[2] > 0)
			{
				numDigits = 3;
			}
			else if (digits[1] > 0)
			{
				numDigits = 2;
			}
			else
			{
				numDigits = 1;
			}
		}
	}

	public ushort Read_File_List(string fileName, ref string[] fileList)
	{
		int num = 0;
		Stream stream = TitleContainer.OpenStream(fileName);
		byte[] array = new byte[stream.Length];
		if (stream.CanRead)
		{
			stream.Read(array, 0, array.Length);
			string text = Byte_Array_To_String(array);
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
				return 0;
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
			num = 0;
			for (i = 0; i < num2; i++)
			{
				array2 = array3[i].Split(' ', '\t');
				for (int j = 0; j < array2.Length; j++)
				{
					if (array2[j].Length > 0)
					{
						num++;
					}
				}
			}
			fileList = new string[num];
			i = 0;
			num = 0;
			for (; i < num2; i++)
			{
				array2 = array3[i].Split(' ', '\t');
				for (int j = 0; j < array2.Length; j++)
				{
					if (array2[j].Length > 0)
					{
						fileList[num++] = array2[j];
					}
				}
			}
		}
		stream.Close();
		return (ushort)num;
	}

	public unsafe void Write_Float_Reversed(BinaryWriter bw, byte* bptr)
	{
		bptr += (int)floatSizeMinusOne;
		for (int num = floatSize; num > 0; num--)
		{
			bw.Write(*bptr);
			bptr--;
		}
	}

	public unsafe void Write_Int_Reversed(BinaryWriter bw, byte* bptr)
	{
		bptr += (int)intSizeMinusOne;
		for (int num = intSize; num > 0; num--)
		{
			bw.Write(*bptr);
			bptr--;
		}
	}

	public unsafe void Write_Ushort_Reversed(BinaryWriter bw, byte* bptr)
	{
		bptr += (int)ushortSizeMinusOne;
		for (ushort num = ushortSize; num > 0; num--)
		{
			bw.Write(*bptr);
			bptr--;
		}
	}

	public void Reverse_Float_Array(ref byte[] b1, int numFloats)
	{
		int num = 0;
		int num2 = 0;
		while (num < numFloats)
		{
			int num3 = floatSizeMinusOne;
			int num4 = num2;
			while (num3 > -1)
			{
				floatsToSwap[num3] = b1[num4++];
				num3--;
			}
			num3 = 0;
			num4 = num2;
			for (; num3 < floatSize; num3++)
			{
				b1[num4++] = floatsToSwap[num3];
			}
			num++;
			num2 += floatSize;
		}
	}

	public void Reverse_Int_Array(ref byte[] b1, int numInts)
	{
		int num = 0;
		int num2 = 0;
		while (num < numInts)
		{
			int num3 = intSizeMinusOne;
			int num4 = num2;
			while (num3 > -1)
			{
				intsToSwap[num3] = b1[num4++];
				num3--;
			}
			num3 = 0;
			num4 = num2;
			for (; num3 < intSize; num3++)
			{
				b1[num4++] = intsToSwap[num3];
			}
			num++;
			num2 += intSize;
		}
	}

	public void Reverse_Ushort_Array(ref byte[] b1, int numUShorts)
	{
		int num = 0;
		int num2 = 0;
		while (num < numUShorts)
		{
			int num3 = ushortSizeMinusOne;
			int num4 = num2;
			while (num3 > -1)
			{
				ushortsToSwap[num3] = b1[num4++];
				num3--;
			}
			num3 = 0;
			num4 = num2;
			for (; num3 < ushortSize; num3++)
			{
				b1[num4++] = ushortsToSwap[num3];
			}
			num++;
			num2 += ushortSize;
		}
	}

	public bool Does_Ray_Intersect_Box(float boxX1, float boxY1, float boxZ1, float width, float depth, float height, float x, float y, float z, float vx, float vy, float vz, float distance)
	{
		float num = 0f;
		float num2 = 0f;
		float num3 = 0f;
		if ((x < boxX1 && vx <= 0f) || (x > boxX1 + width && vx >= 0f) || (y < boxY1 && vy <= 0f) || (y > boxY1 + depth && vy >= 0f) || (z < boxZ1 && vz <= 0f) || (z > boxZ1 + height && vz >= 0f))
		{
			return false;
		}
		num = x - boxX1;
		num2 = y - boxY1;
		num3 = z - boxZ1;
		if (vx != 0f)
		{
			if (num < 0f)
			{
				float num4 = (0f - num) / vx;
				distance -= num4;
				if (distance < 0f)
				{
					return false;
				}
				num = 0f;
				num2 += vy * num4;
				num3 += vz * num4;
			}
			else if (num > width)
			{
				float num4 = (width - num) / vx;
				distance -= num4;
				if (distance < 0f)
				{
					return false;
				}
				num = width;
				num2 += vy * num4;
				num3 += vz * num4;
			}
		}
		if (vy != 0f)
		{
			if (num2 < 0f)
			{
				float num4 = (0f - num2) / vy;
				distance -= num4;
				if (distance < 0f || num4 < 0f)
				{
					return false;
				}
				num2 = 0f;
				num += vx * num4;
				num3 += vz * num4;
			}
			else if (num2 > depth)
			{
				float num4 = (depth - num2) / vy;
				distance -= num4;
				if (distance < 0f || num4 < 0f)
				{
					return false;
				}
				num2 = depth;
				num += vx * num4;
				num3 += vz * num4;
			}
		}
		if (vz != 0f)
		{
			if (num3 < 0f)
			{
				float num4 = (0f - num3) / vz;
				distance -= num4;
				if (distance < 0f || num4 < 0f)
				{
					return false;
				}
				num += vx * num4;
				num2 += vy * num4;
				num3 = 0f;
			}
			else if (num3 > height)
			{
				float num4 = (height - num3) / vz;
				distance -= num4;
				if (distance < 0f || num4 < 0f)
				{
					return false;
				}
				num3 = height;
				num += vx * num4;
				num2 += vy * num4;
			}
		}
		if (num >= 0f && num <= width && num2 >= 0f && num2 <= depth && num3 >= 0f && num3 <= height)
		{
			return true;
		}
		return false;
	}
}
