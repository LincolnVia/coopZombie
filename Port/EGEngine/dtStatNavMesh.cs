using System;
using System.Buffers.Binary;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.Xna.Framework;

namespace EGEngine;

public class dtStatNavMesh
{
	public struct dtStatPoly(int size)
	{
		public ushort[] v = new ushort[size];

		public ushort[] n = new ushort[size];

		public byte nv = 0;

		public byte flags = 0;
	}

	public struct dtStatPolyDetail
	{
		public ushort vbase;

		public ushort nverts;

		public ushort tbase;

		public ushort ntris;
	}

	public struct dtStatBVNode(int size)
	{
		public ushort[] bmin = new ushort[size];

		public ushort[] bmax = new ushort[size];

		public int i = 0;
	}

	public struct dtStatNavMeshHeader
	{
		public int magic;

		public int version;

		public int npolys;

		public int nverts;

		public int nnodes;

		public int ndmeshes;

		public int ndverts;

		public int ndtris;

		public float cs;

		public Vector3 bmin;

		public Vector3 bmax;

		public dtStatPoly[] polys;

		public Vector3[] verts;

		public dtStatBVNode[] bvtree;

		public dtStatPolyDetail[] dmeshes;

		public Vector3[] dverts;

		public byte[][] dtris;
	}

	public struct ReadStatPolyDetail
	{
		public ushort vbase;

		public ushort nverts;

		public ushort tbase;

		public ushort ntris;
	}

	public struct ReadStatPoly
	{
		public ushort v;

		public ushort v1;

		public ushort v2;

		public ushort v3;

		public ushort v4;

		public ushort v5;

		public ushort n;

		public ushort n1;

		public ushort n2;

		public ushort n3;

		public ushort n4;

		public ushort n5;

		public byte nv;

		public byte flags;
	}

	public struct ReadStatBVNode
	{
		public ushort bmin;

		public ushort bmin1;

		public ushort bmin2;

		public ushort bmax;

		public ushort bmax1;

		public ushort bmax2;

		public int i;
	}

	private struct ReadNavMeshHeader
	{
		public int magic;

		public int version;

		public int npolys;

		public int nverts;

		public int nnodes;

		public int ndmeshes;

		public int ndverts;

		public int ndtris;

		public float cs;

		public float bminX;

		public float bminY;

		public float bminZ;

		public float bmaxX;

		public float bmaxY;

		public float bmaxZ;

		public unsafe ReadStatPoly* polys;

		public unsafe float* verts;

		public unsafe ReadStatBVNode* bvtree;

		public unsafe ReadStatPolyDetail* dmeshes;

		public unsafe float* dverts;

		public unsafe byte* dtris;
	}

	private const byte findNearestPoly_polys_Size = 128;

	public const int Size_dtStatPoly = 26;

	public const int Size_dtStatPolyDetail = 8;

	public const int Size_dtStatBVNode = 16;

	private static ushort[] findNearestPoly_polys = new ushort[128];

	private static dtStatBVNode node_queryPolygons;

	private static dtStatBVNode end_queryPolygons;

	private static ushort[] bmin_queryPolygons = new ushort[3];

	private static ushort[] bmax_queryPolygons = new ushort[3];

	private static dtNode startNode_findPath;

	private static dtNode bestNode_findPath;

	private static dtNode newNode_findPath = new dtNode();

	private static dtNode lastBestNode_findPath;

	private static dtNode parent_findPath;

	private static dtNode actualNode_findPath;

	private static dtNode prev_findPath;

	private static dtNode node_findPath;

	private static dtNode next_findPath;

	private static dtStatPoly poly_findPath;

	private static float[] portalApex_findStraightPath = new float[3];

	private static float[] portalLeft_findStraightPath = new float[3];

	private static float[] portalRight_findStraightPath = new float[3];

	private static dtStatPoly p_closestPointToPoly;

	private static dtStatPoly result_getPolyByRef;

	private static dtStatPoly fromPoly_getPortalPoints;

	private static int DT_STAT_VERTS_PER_POLYGON = 6;

	private static int DT_STAT_NAVMESH_MAGIC = 1312904781;

	private static int DT_STAT_NAVMESH_VERSION = 3;

	public static int MAX_POLYS = 256;

	public static int MaxRoutesThisUpdate = 1;

	private static Random RandGen = new Random();

	private bool Initialized;

	private int headerSize;

	private int vertsSize;

	private int polysSize;

	private int nodesSize;

	private int detailMeshesSize;

	private int detailVertsSize;

	private int detailTrisSize;

	private Vector3 PickExtents = new Vector3(20f, 80f, 20f);

	private dtStatNavMeshHeader mHeader;

	private dtNodePool m_nodePool;

	private dtNodeQueue m_openList;

	private static float[][] straightPolys = new float[MAX_POLYS][];

	private static float H_SCALE = 1.1f;

	private static float thr = (float)Math.Sqrt(6.103515625E-05);

	private static float EPS = 0.0001f;

	public void LoadNavigationMesh(string filename)
	{
		try
		{
			using FileStream fileStream = File.OpenRead(filename);
			using BinaryReader binaryReader = new BinaryReader(fileStream);
			int num = binaryReader.ReadInt32();
			if (num < 84)
			{
				throw new InvalidDataException($"Xbox navigation mesh is too small: {num} bytes.");
			}
			byte[] array = binaryReader.ReadBytes(num);
			if (array.Length != num)
			{
				throw new EndOfStreamException($"Expected {num} Xbox navigation-mesh bytes, read {array.Length}.");
			}
			InitializeXbox360(array);
		}
		catch (FileNotFoundException)
		{
		}
	}

	private void InitializeXbox360(byte[] data)
	{
		int offset = 0;
		mHeader.magic = ReadInt32BigEndian(data, ref offset);
		mHeader.version = ReadInt32BigEndian(data, ref offset);
		if (mHeader.magic != DT_STAT_NAVMESH_MAGIC || mHeader.version != DT_STAT_NAVMESH_VERSION)
		{
			throw new InvalidDataException($"Unsupported Xbox navigation mesh header: magic=0x{mHeader.magic:X8}, version={mHeader.version}.");
		}

		mHeader.npolys = ReadInt32BigEndian(data, ref offset);
		mHeader.nverts = ReadInt32BigEndian(data, ref offset);
		mHeader.nnodes = ReadInt32BigEndian(data, ref offset);
		mHeader.ndmeshes = ReadInt32BigEndian(data, ref offset);
		mHeader.ndverts = ReadInt32BigEndian(data, ref offset);
		mHeader.ndtris = ReadInt32BigEndian(data, ref offset);
		mHeader.cs = ReadSingleBigEndian(data, ref offset);
		mHeader.bmin = new Vector3(ReadSingleBigEndian(data, ref offset), ReadSingleBigEndian(data, ref offset), ReadSingleBigEndian(data, ref offset));
		mHeader.bmax = new Vector3(ReadSingleBigEndian(data, ref offset), ReadSingleBigEndian(data, ref offset), ReadSingleBigEndian(data, ref offset));

		if (mHeader.npolys < 0 || mHeader.nverts < 0 || mHeader.nnodes < 0 || mHeader.ndmeshes < 0 || mHeader.ndverts < 0 || mHeader.ndtris < 0)
		{
			throw new InvalidDataException("Xbox navigation mesh contains a negative element count.");
		}

		// The file was serialized on the 32-bit Xbox runtime. Six four-byte
		// pointer placeholders follow the 60-byte scalar header; array data
		// therefore begins at byte 84, regardless of the desktop pointer size.
		offset = 84;
		headerSize = 84;
		vertsSize = checked(12 * mHeader.nverts);
		polysSize = checked(Size_dtStatPoly * mHeader.npolys);
		nodesSize = checked(Size_dtStatBVNode * mHeader.npolys * 2);
		detailMeshesSize = checked(Size_dtStatPolyDetail * mHeader.ndmeshes);
		detailVertsSize = checked(12 * mHeader.ndverts);
		detailTrisSize = checked(4 * mHeader.ndtris);
		int expectedSize = checked(headerSize + vertsSize + polysSize + nodesSize + detailMeshesSize + detailVertsSize + detailTrisSize);
		if (expectedSize != data.Length)
		{
			throw new InvalidDataException($"Xbox navigation mesh size mismatch: header describes {expectedSize} bytes, file contains {data.Length}.");
		}

		mHeader.verts = new Vector3[mHeader.nverts];
		for (int i = 0; i < mHeader.nverts; i++)
		{
			mHeader.verts[i] = new Vector3(ReadSingleBigEndian(data, ref offset), ReadSingleBigEndian(data, ref offset), ReadSingleBigEndian(data, ref offset));
		}

		mHeader.polys = new dtStatPoly[mHeader.npolys];
		for (int i = 0; i < mHeader.npolys; i++)
		{
			mHeader.polys[i] = new dtStatPoly(DT_STAT_VERTS_PER_POLYGON);
			for (int j = 0; j < DT_STAT_VERTS_PER_POLYGON; j++)
			{
				mHeader.polys[i].v[j] = ReadUInt16BigEndian(data, ref offset);
			}
			for (int j = 0; j < DT_STAT_VERTS_PER_POLYGON; j++)
			{
				mHeader.polys[i].n[j] = ReadUInt16BigEndian(data, ref offset);
			}
			mHeader.polys[i].nv = data[offset++];
			mHeader.polys[i].flags = data[offset++];
		}

		mHeader.bvtree = new dtStatBVNode[mHeader.npolys * 2];
		for (int i = 0; i < mHeader.bvtree.Length; i++)
		{
			mHeader.bvtree[i] = new dtStatBVNode(3);
			for (int j = 0; j < 3; j++)
			{
				mHeader.bvtree[i].bmin[j] = ReadUInt16BigEndian(data, ref offset);
			}
			for (int j = 0; j < 3; j++)
			{
				mHeader.bvtree[i].bmax[j] = ReadUInt16BigEndian(data, ref offset);
			}
			mHeader.bvtree[i].i = ReadInt32BigEndian(data, ref offset);
		}

		mHeader.dmeshes = new dtStatPolyDetail[mHeader.ndmeshes];
		for (int i = 0; i < mHeader.ndmeshes; i++)
		{
			mHeader.dmeshes[i].vbase = ReadUInt16BigEndian(data, ref offset);
			mHeader.dmeshes[i].nverts = ReadUInt16BigEndian(data, ref offset);
			mHeader.dmeshes[i].tbase = ReadUInt16BigEndian(data, ref offset);
			mHeader.dmeshes[i].ntris = ReadUInt16BigEndian(data, ref offset);
		}

		mHeader.dverts = new Vector3[mHeader.ndverts];
		for (int i = 0; i < mHeader.ndverts; i++)
		{
			mHeader.dverts[i] = new Vector3(ReadSingleBigEndian(data, ref offset), ReadSingleBigEndian(data, ref offset), ReadSingleBigEndian(data, ref offset));
		}

		mHeader.dtris = new byte[mHeader.ndtris][];
		for (int i = 0; i < mHeader.ndtris; i++)
		{
			mHeader.dtris[i] = new byte[4] { data[offset++], data[offset++], data[offset++], data[offset++] };
		}

		m_nodePool = new dtNodePool(2048);
		m_openList = new dtNodeQueue(2048);
		for (int i = 0; i < MAX_POLYS; i++)
		{
			straightPolys[i] = new float[3];
		}
		Initialized = true;
		Console.WriteLine($"Loaded Xbox navigation mesh: polys={mHeader.npolys}, verts={mHeader.nverts}, detailTris={mHeader.ndtris}.");
	}

	private static int ReadInt32BigEndian(byte[] data, ref int offset)
	{
		EnsureRemaining(data, offset, 4);
		int value = BinaryPrimitives.ReadInt32BigEndian(data.AsSpan(offset, 4));
		offset += 4;
		return value;
	}

	private static ushort ReadUInt16BigEndian(byte[] data, ref int offset)
	{
		EnsureRemaining(data, offset, 2);
		ushort value = BinaryPrimitives.ReadUInt16BigEndian(data.AsSpan(offset, 2));
		offset += 2;
		return value;
	}

	private static float ReadSingleBigEndian(byte[] data, ref int offset)
	{
		return BitConverter.Int32BitsToSingle(ReadInt32BigEndian(data, ref offset));
	}

	private static void EnsureRemaining(byte[] data, int offset, int count)
	{
		if (offset < 0 || count < 0 || offset > data.Length - count)
		{
			throw new EndOfStreamException("Xbox navigation mesh ended inside a structured field.");
		}
	}

	public unsafe bool Initialize(byte* data, int dataSize, bool ownsData)
	{
		ReadNavMeshHeader* ptr = (ReadNavMeshHeader*)(((long)data + 3L) & -4);
		float* ptr2 = (float*)((byte*)ptr + dataSize);
		ptr2 = (float*)(((long)ptr2 + 3L) & -4);
		if (ptr->magic != DT_STAT_NAVMESH_MAGIC)
		{
			return false;
		}
		if (ptr->version != DT_STAT_NAVMESH_VERSION)
		{
			return false;
		}
		headerSize = sizeof(ReadNavMeshHeader);
		vertsSize = 12 * ptr->nverts;
		polysSize = sizeof(ReadStatPoly) * ptr->npolys;
		nodesSize = sizeof(ReadStatBVNode) * ptr->npolys * 2;
		detailMeshesSize = sizeof(ReadStatPolyDetail) * ptr->ndmeshes;
		detailVertsSize = 12 * ptr->ndverts;
		detailTrisSize = 4 * ptr->ndtris;
		byte* ptr3 = (ptr->dtris = (byte*)(ptr->dverts = (float*)((byte*)(ptr->dmeshes = (ReadStatPolyDetail*)((byte*)(ptr->bvtree = (ReadStatBVNode*)((byte*)(ptr->polys = (ReadStatPoly*)((byte*)(ptr->verts = (float*)((byte*)ptr + headerSize)) + vertsSize)) + polysSize)) + nodesSize)) + detailMeshesSize)) + detailVertsSize) + detailTrisSize;
		mHeader.magic = ptr->magic;
		mHeader.version = ptr->version;
		mHeader.cs = ptr->cs;
		mHeader.bmin.X = ptr->bminX;
		mHeader.bmin.Y = ptr->bminY;
		mHeader.bmin.Z = ptr->bminZ;
		mHeader.bmax.X = ptr->bmaxX;
		mHeader.bmax.Y = ptr->bmaxY;
		mHeader.bmax.Z = ptr->bmaxZ;
		mHeader.nverts = ptr->nverts;
		mHeader.nnodes = ptr->nnodes;
		mHeader.npolys = ptr->npolys;
		mHeader.ndmeshes = ptr->ndmeshes;
		mHeader.ndverts = ptr->ndverts;
		mHeader.ndtris = ptr->ndtris;
		mHeader.verts = new Vector3[mHeader.nverts];
		mHeader.polys = new dtStatPoly[mHeader.npolys];
		mHeader.bvtree = new dtStatBVNode[mHeader.npolys * 2];
		mHeader.dmeshes = new dtStatPolyDetail[mHeader.ndmeshes];
		mHeader.dverts = new Vector3[mHeader.ndverts];
		mHeader.dtris = new byte[mHeader.ndtris][];
		for (int i = 0; i < mHeader.ndtris; i++)
		{
			mHeader.dtris[i] = new byte[4];
		}
		for (int j = 0; j < mHeader.nverts; j++)
		{
			ref Vector3 reference = ref mHeader.verts[j];
			reference = Vector3.Zero;
			mHeader.verts[j].X = ptr->verts[j * 3];
			mHeader.verts[j].Y = ptr->verts[j * 3 + 1];
			mHeader.verts[j].Z = ptr->verts[j * 3 + 2];
		}
		for (int k = 0; k < mHeader.npolys; k++)
		{
			ref dtStatPoly reference2 = ref mHeader.polys[k];
			reference2 = new dtStatPoly(DT_STAT_VERTS_PER_POLYGON);
			mHeader.polys[k].nv = ptr->polys[k].nv;
			mHeader.polys[k].flags = ptr->polys[k].flags;
			for (int l = 0; l < DT_STAT_VERTS_PER_POLYGON; l++)
			{
				mHeader.polys[k].v[l] = (&ptr->polys[k].v)[l];
				mHeader.polys[k].n[l] = (&ptr->polys[k].n)[l];
			}
		}
		for (int m = 0; m < mHeader.npolys * 2; m++)
		{
			ref dtStatBVNode reference3 = ref mHeader.bvtree[m];
			reference3 = new dtStatBVNode(3);
			mHeader.bvtree[m].i = ptr->bvtree[m].i;
			for (int n = 0; n < 3; n++)
			{
				mHeader.bvtree[m].bmin[n] = (&ptr->bvtree[m].bmin)[n];
				mHeader.bvtree[m].bmax[n] = (&ptr->bvtree[m].bmax)[n];
			}
		}
		for (int num = 0; num < mHeader.ndmeshes; num++)
		{
			mHeader.dmeshes[num] = default(dtStatPolyDetail);
			mHeader.dmeshes[num].vbase = ptr->dmeshes[num].vbase;
			mHeader.dmeshes[num].nverts = ptr->dmeshes[num].nverts;
			mHeader.dmeshes[num].tbase = ptr->dmeshes[num].tbase;
			mHeader.dmeshes[num].ntris = ptr->dmeshes[num].ntris;
		}
		for (int num2 = 0; num2 < mHeader.ndverts; num2++)
		{
			ref Vector3 reference4 = ref mHeader.dverts[num2];
			reference4 = Vector3.Zero;
			byte* ptr4 = (byte*)ptr2;
			byte* ptr5 = (byte*)(ptr->dverts + num2 * 3);
			for (byte b = 0; b < 12; b++)
			{
				*ptr4 = *ptr5;
				ptr4++;
				ptr5++;
			}
			mHeader.dverts[num2].X = *ptr2;
			mHeader.dverts[num2].Y = ptr2[1];
			mHeader.dverts[num2].Z = ptr2[2];
		}
		for (int num3 = 0; num3 < mHeader.ndtris; num3++)
		{
			mHeader.dtris[num3][0] = ptr->dtris[num3 * 4];
			mHeader.dtris[num3][1] = ptr->dtris[num3 * 4 + 1];
			mHeader.dtris[num3][2] = ptr->dtris[num3 * 4 + 2];
			mHeader.dtris[num3][3] = ptr->dtris[num3 * 4 + 3];
		}
		m_nodePool = new dtNodePool(2048);
		m_openList = new dtNodeQueue(2048);
		for (int num4 = 0; num4 < MAX_POLYS; num4++)
		{
			straightPolys[num4] = new float[3];
		}
		Initialized = true;
		return true;
	}

	public int GetPath(ref Vector3 startpos, ref Vector3 endpos, ushort[] polys, Vector3[] spolys, bool randomDestination)
	{
		int num = 0;
		if (MaxRoutesThisUpdate > 0)
		{
			MaxRoutesThisUpdate--;
			int num2 = 0;
			ushort num3 = 0;
			ushort num4 = 0;
			num3 = findNearestPoly(ref startpos, ref PickExtents);
			num4 = findNearestPoly(ref endpos, ref PickExtents);
			if (num3 != 0 && num4 != 0)
			{
				num2 = findPath(num3, num4, ref startpos, ref endpos, polys, MAX_POLYS);
			}
			if (num2 > 0)
			{
				num = findStraightPath(ref startpos, ref endpos, polys, num2, straightPolys, MAX_POLYS);
				for (int i = 0; i < num; i++)
				{
					spolys[i].X = straightPolys[i][0];
					spolys[i].Y = straightPolys[i][1];
					spolys[i].Z = straightPolys[i][2];
				}
			}
		}
		return num;
	}

	public ushort findNearestPoly(ref Vector3 center, ref Vector3 extents)
	{
		if (!Initialized)
		{
			return 0;
		}
		ushort result = 0;
		int num = queryPolygons(ref center, ref extents, findNearestPoly_polys, 128);
		float num2 = float.MaxValue;
		for (int i = 0; i < num; i++)
		{
			ushort num3 = findNearestPoly_polys[i];
			Vector3 closest = Vector3.Zero;
			if (closestPointToPoly(num3, ref center, ref closest))
			{
				float num4 = (center - closest).LengthSquared();
				if (num4 < num2)
				{
					num2 = num4;
					result = num3;
				}
			}
		}
		return result;
	}

	public int queryPolygons(ref Vector3 center, ref Vector3 extents, ushort[] polys, int maxPolys)
	{
		if (!Initialized)
		{
			return 0;
		}
		int num = 0;
		int num2 = 0;
		node_queryPolygons = mHeader.bvtree[num2];
		end_queryPolygons = mHeader.bvtree[mHeader.nnodes];
		float num3 = 1f / mHeader.cs;
		float num4 = MathHelper.Clamp(center.X - extents.X, mHeader.bmin.X, mHeader.bmax.X) - mHeader.bmin.X;
		float num5 = MathHelper.Clamp(center.Y - extents.Y, mHeader.bmin.Y, mHeader.bmax.Y) - mHeader.bmin.Y;
		float num6 = MathHelper.Clamp(center.Z - extents.Z, mHeader.bmin.Z, mHeader.bmax.Z) - mHeader.bmin.Z;
		float num7 = MathHelper.Clamp(center.X + extents.X, mHeader.bmin.X, mHeader.bmax.X) - mHeader.bmin.X;
		float num8 = MathHelper.Clamp(center.Y + extents.Y, mHeader.bmin.Y, mHeader.bmax.Y) - mHeader.bmin.Y;
		float num9 = MathHelper.Clamp(center.Z + extents.Z, mHeader.bmin.Z, mHeader.bmax.Z) - mHeader.bmin.Z;
		bmin_queryPolygons[0] = (ushort)((int)(num3 * num4) & 0xFFFE);
		bmin_queryPolygons[1] = (ushort)((int)(num3 * num5) & 0xFFFE);
		bmin_queryPolygons[2] = (ushort)((int)(num3 * num6) & 0xFFFE);
		bmax_queryPolygons[0] = (ushort)((int)(num3 * num7 + 1f) | 1);
		bmax_queryPolygons[1] = (ushort)((int)(num3 * num8 + 1f) | 1);
		bmax_queryPolygons[2] = (ushort)((int)(num3 * num9 + 1f) | 1);
		while (num2 < mHeader.nnodes)
		{
			bool flag = checkOverlapBox(bmin_queryPolygons, bmax_queryPolygons, node_queryPolygons.bmin, node_queryPolygons.bmax);
			bool flag2 = node_queryPolygons.i >= 0;
			if (flag2 && flag && num < maxPolys)
			{
				polys[num] = (ushort)node_queryPolygons.i;
				num++;
			}
			if (flag || flag2)
			{
				num2++;
				node_queryPolygons = mHeader.bvtree[num2];
			}
			else
			{
				int num10 = -node_queryPolygons.i;
				num2 += num10;
				node_queryPolygons = mHeader.bvtree[num2];
			}
		}
		return num;
	}

	public int findPath(ushort startRef, ushort endRef, ref Vector3 startPos, ref Vector3 endPos, ushort[] path, int maxPathSize)
	{
		if (!Initialized)
		{
			return 0;
		}
		if (startRef == 0 || endRef == 0)
		{
			return 0;
		}
		if (maxPathSize == 0)
		{
			return 0;
		}
		if (startRef == endRef)
		{
			path[0] = startRef;
			return 1;
		}
		m_nodePool.clear();
		m_openList.clear();
		startNode_findPath = m_nodePool.getNode(startRef);
		startNode_findPath.pidx = 0u;
		startNode_findPath.cost = 0f;
		startNode_findPath.total = (startPos - endPos).Length() * H_SCALE;
		startNode_findPath.id = startRef;
		startNode_findPath.flags = 1u;
		m_openList.push(startNode_findPath);
		lastBestNode_findPath = startNode_findPath;
		float num = startNode_findPath.total;
		while (!m_openList.empty())
		{
			bestNode_findPath = m_openList.pop();
			if (bestNode_findPath.id == endRef)
			{
				lastBestNode_findPath = bestNode_findPath;
				break;
			}
			poly_findPath = getPoly((int)(bestNode_findPath.id - 1));
			for (int i = 0; i < poly_findPath.nv; i++)
			{
				ushort num2 = poly_findPath.n[i];
				if (num2 == 0 || (bestNode_findPath.pidx != 0 && m_nodePool.getNodeAtIdx(bestNode_findPath.pidx).id == num2))
				{
					continue;
				}
				parent_findPath = bestNode_findPath;
				newNode_findPath.pidx = m_nodePool.getNodeIdx(parent_findPath);
				newNode_findPath.id = num2;
				float num3 = 0f;
				Vector3 mid = Vector3.Zero;
				Vector3 mid2 = Vector3.Zero;
				if (parent_findPath.pidx == 0)
				{
					mid = startPos;
				}
				else
				{
					getEdgeMidPoint((ushort)m_nodePool.getNodeAtIdx(parent_findPath.pidx).id, (ushort)parent_findPath.id, ref mid);
				}
				getEdgeMidPoint((ushort)parent_findPath.id, (ushort)newNode_findPath.id, ref mid2);
				newNode_findPath.cost = parent_findPath.cost + (mid - mid2).Length();
				if (newNode_findPath.id == endRef)
				{
					newNode_findPath.cost += (mid2 - endPos).Length();
				}
				num3 = (mid2 - endPos).Length() * H_SCALE;
				newNode_findPath.total = newNode_findPath.cost + num3;
				actualNode_findPath = m_nodePool.getNode(newNode_findPath.id);
				if (actualNode_findPath != null && ((actualNode_findPath.flags & 1) == 0 || !(newNode_findPath.total > actualNode_findPath.total)) && ((actualNode_findPath.flags & 2) == 0 || !(newNode_findPath.total > actualNode_findPath.total)))
				{
					actualNode_findPath.flags &= 4294967293u;
					actualNode_findPath.pidx = newNode_findPath.pidx;
					actualNode_findPath.cost = newNode_findPath.cost;
					actualNode_findPath.total = newNode_findPath.total;
					if (num3 < num)
					{
						num = num3;
						lastBestNode_findPath = actualNode_findPath;
					}
					if ((actualNode_findPath.flags & 1) != 0)
					{
						m_openList.modify(actualNode_findPath);
						continue;
					}
					actualNode_findPath.flags |= 1u;
					m_openList.push(actualNode_findPath);
				}
			}
			bestNode_findPath.flags |= 2u;
		}
		prev_findPath = null;
		node_findPath = lastBestNode_findPath;
		do
		{
			next_findPath = m_nodePool.getNodeAtIdx(node_findPath.pidx);
			node_findPath.pidx = m_nodePool.getNodeIdx(prev_findPath);
			prev_findPath = node_findPath;
			node_findPath = next_findPath;
		}
		while (node_findPath != null);
		node_findPath = prev_findPath;
		int num4 = 0;
		do
		{
			path[num4++] = (ushort)node_findPath.id;
			node_findPath = m_nodePool.getNodeAtIdx(node_findPath.pidx);
		}
		while (node_findPath != null && num4 < maxPathSize);
		return num4;
	}

	public int findStraightPath(ref Vector3 startPos, ref Vector3 endPos, ushort[] path, int pathSize, float[][] straightPath, int maxStraightPathSize)
	{
		if (!Initialized)
		{
			return 0;
		}
		if (maxStraightPathSize == 0)
		{
			return 0;
		}
		if (path[0] == 0)
		{
			return 0;
		}
		int num = 0;
		Vector3 closest = Vector3.Zero;
		if (!closestPointToPoly(path[0], ref startPos, ref closest))
		{
			return 0;
		}
		straightPath[num][0] = closest.X;
		straightPath[num][1] = closest.Y;
		straightPath[num][2] = closest.Z;
		num++;
		if (num >= maxStraightPathSize)
		{
			return num;
		}
		Vector3 closest2 = Vector3.Zero;
		if (!closestPointToPoly(path[pathSize - 1], ref endPos, ref closest2))
		{
			return 0;
		}
		if (pathSize > 1)
		{
			portalApex_findStraightPath[0] = closest.X;
			portalApex_findStraightPath[1] = closest.Y;
			portalApex_findStraightPath[2] = closest.Z;
			vcopy(portalLeft_findStraightPath, portalApex_findStraightPath);
			vcopy(portalRight_findStraightPath, portalApex_findStraightPath);
			int num2 = 0;
			int num3 = 0;
			int num4 = 0;
			for (int i = 0; i < pathSize; i++)
			{
				Vector3 left = Vector3.Zero;
				Vector3 right = Vector3.Zero;
				if (i < pathSize - 1)
				{
					getPortalPoints(path[i], path[i + 1], ref left, ref right);
				}
				else
				{
					left = closest2;
					right = closest2;
				}
				if (vequal(portalApex_findStraightPath, portalRight_findStraightPath))
				{
					portalRight_findStraightPath[0] = right.X;
					portalRight_findStraightPath[1] = right.Y;
					portalRight_findStraightPath[2] = right.Z;
					num4 = i;
				}
				else if (triArea2D(portalApex_findStraightPath, portalRight_findStraightPath, ref right) <= 0f)
				{
					if (!(triArea2D(portalApex_findStraightPath, portalLeft_findStraightPath, ref right) > 0f))
					{
						vcopy(portalApex_findStraightPath, portalLeft_findStraightPath);
						num2 = num3;
						if (!vequal(straightPath[num - 1], portalApex_findStraightPath))
						{
							vcopy(straightPath[num], portalApex_findStraightPath);
							num++;
							if (num >= maxStraightPathSize)
							{
								return num;
							}
						}
						vcopy(portalLeft_findStraightPath, portalApex_findStraightPath);
						vcopy(portalRight_findStraightPath, portalApex_findStraightPath);
						num3 = num2;
						num4 = num2;
						i = num2;
						continue;
					}
					portalRight_findStraightPath[0] = right.X;
					portalRight_findStraightPath[1] = right.Y;
					portalRight_findStraightPath[2] = right.Z;
					num4 = i;
				}
				if (vequal(portalApex_findStraightPath, portalLeft_findStraightPath))
				{
					portalLeft_findStraightPath[0] = left.X;
					portalLeft_findStraightPath[1] = left.Y;
					portalLeft_findStraightPath[2] = left.Z;
					num3 = i;
				}
				else
				{
					if (!(triArea2D(portalApex_findStraightPath, portalLeft_findStraightPath, ref left) >= 0f))
					{
						continue;
					}
					if (triArea2D(portalApex_findStraightPath, portalRight_findStraightPath, ref left) < 0f)
					{
						portalLeft_findStraightPath[0] = left.X;
						portalLeft_findStraightPath[1] = left.Y;
						portalLeft_findStraightPath[2] = left.Z;
						num3 = i;
						continue;
					}
					vcopy(portalApex_findStraightPath, portalRight_findStraightPath);
					num2 = num4;
					if (!vequal(straightPath[num - 1], portalApex_findStraightPath))
					{
						vcopy(straightPath[num], portalApex_findStraightPath);
						num++;
						if (num >= maxStraightPathSize)
						{
							return num;
						}
					}
					vcopy(portalLeft_findStraightPath, portalApex_findStraightPath);
					vcopy(portalRight_findStraightPath, portalApex_findStraightPath);
					num3 = num2;
					num4 = num2;
					i = num2;
				}
			}
		}
		straightPath[num][0] = closest2.X;
		straightPath[num][1] = closest2.Y;
		straightPath[num][2] = closest2.Z;
		return num + 1;
	}

	public bool closestPointToPoly(ushort Ref, ref Vector3 pos, ref Vector3 closest)
	{
		int polyIndexByRef = getPolyIndexByRef(Ref);
		if (polyIndexByRef == -1)
		{
			return false;
		}
		float num = float.MaxValue;
		p_closestPointToPoly = getPoly(polyIndexByRef);
		dtStatPolyDetail polyDetail = getPolyDetail(polyIndexByRef);
		for (int i = 0; i < polyDetail.ntris; i++)
		{
			byte[] detailTri = getDetailTri(polyDetail.tbase + i);
			Vector3[] array = new Vector3[3];
			for (int j = 0; j < 3; j++)
			{
				ref Vector3 reference = ref array[j];
				reference = Vector3.Zero;
				if (detailTri[j] < p_closestPointToPoly.nv)
				{
					array[j].X = getVertex(p_closestPointToPoly.v[detailTri[j]]).X;
					array[j].Y = getVertex(p_closestPointToPoly.v[detailTri[j]]).Y;
					array[j].Z = getVertex(p_closestPointToPoly.v[detailTri[j]]).Z;
				}
				else
				{
					array[j].X = getDetailVertex(polyDetail.vbase + (detailTri[j] - p_closestPointToPoly.nv)).X;
					array[j].Y = getDetailVertex(polyDetail.vbase + (detailTri[j] - p_closestPointToPoly.nv)).Y;
					array[j].Z = getDetailVertex(polyDetail.vbase + (detailTri[j] - p_closestPointToPoly.nv)).Z;
				}
			}
			Vector3 closest2 = Vector3.Zero;
			closestPtPointTriangle(ref closest2, ref pos, ref array[0], ref array[1], ref array[2]);
			float num2 = (pos - closest2).LengthSquared();
			if (num2 < num)
			{
				closest = closest2;
				num = num2;
			}
		}
		return true;
	}

	public dtStatPoly getPolyByRef(ushort Ref)
	{
		if (!Initialized || Ref == 0 || Ref > mHeader.npolys)
		{
			result_getPolyByRef = mHeader.polys[Ref - 1];
		}
		return mHeader.polys[Ref - 1];
	}

	public int getPolyIndexByRef(ushort Ref)
	{
		if (!Initialized || Ref == 0 || Ref > mHeader.npolys)
		{
			return -1;
		}
		return Ref - 1;
	}

	public int getPolyCount()
	{
		if (!Initialized)
		{
			return 0;
		}
		return mHeader.npolys;
	}

	public dtStatPoly getPoly(int i)
	{
		return mHeader.polys[i];
	}

	public int getVertexCount()
	{
		if (Initialized)
		{
			return 0;
		}
		return mHeader.nverts;
	}

	public Vector3 getVertex(int i)
	{
		return mHeader.verts[i];
	}

	public int getPolyDetailCount()
	{
		if (!Initialized)
		{
			return 0;
		}
		return mHeader.ndmeshes;
	}

	public dtStatPolyDetail getPolyDetail(int i)
	{
		return mHeader.dmeshes[i];
	}

	public int getDetailVertexCount()
	{
		if (Initialized)
		{
			return 0;
		}
		return mHeader.ndverts;
	}

	public Vector3 getDetailVertex(int i)
	{
		return mHeader.dverts[i];
	}

	public int getDetailTriCount()
	{
		if (Initialized)
		{
			return 0;
		}
		return mHeader.ndtris;
	}

	public byte[] getDetailTri(int i)
	{
		return mHeader.dtris[i];
	}

	public bool isInClosedList(ushort Ref)
	{
		if (m_nodePool == null)
		{
			return false;
		}
		if (m_nodePool.findNode(Ref) != null)
		{
			return (m_nodePool.findNode(Ref).flags & 2) != 0;
		}
		return false;
	}

	public int getBvTreeNodeCount()
	{
		if (Initialized)
		{
			return 0;
		}
		return mHeader.nnodes;
	}

	private bool getPortalPoints(ushort from, ushort to, ref Vector3 left, ref Vector3 right)
	{
		fromPoly_getPortalPoints = getPolyByRef(from);
		int num = 0;
		int num2 = fromPoly_getPortalPoints.nv - 1;
		while (num < fromPoly_getPortalPoints.nv)
		{
			ushort num3 = fromPoly_getPortalPoints.n[num2];
			if (num3 == to)
			{
				left = getVertex(fromPoly_getPortalPoints.v[num2]);
				right = getVertex(fromPoly_getPortalPoints.v[num]);
				return true;
			}
			num2 = num++;
		}
		return false;
	}

	private bool getEdgeMidPoint(ushort from, ushort to, ref Vector3 mid)
	{
		Vector3 right = Vector3.Zero;
		Vector3 left = Vector3.Zero;
		if (!getPortalPoints(from, to, ref left, ref right))
		{
			return false;
		}
		mid.X = (left.X + right.X) * 0.5f;
		mid.Y = (left.Y + right.Y) * 0.5f;
		mid.Z = (left.Z + right.Z) * 0.5f;
		return true;
	}

	private float vdist(float[] v1, float[] v2)
	{
		float num = v2[0] - v1[0];
		float num2 = v2[1] - v1[1];
		float num3 = v2[2] - v1[2];
		return (float)Math.Sqrt(num * num + num2 * num2 + num3 * num3);
	}

	private void vcopy(float[] dest, float[] a)
	{
		dest[0] = a[0];
		dest[1] = a[1];
		dest[2] = a[2];
	}

	private void vsub(float[] dest, float[] v1, float[] v2)
	{
		dest[0] = v1[0] - v2[0];
		dest[1] = v1[1] - v2[1];
		dest[2] = v1[2] - v2[2];
	}

	private float vdot(float[] v1, float[] v2)
	{
		return v1[0] * v2[0] + v1[1] * v2[1] + v1[2] * v2[2];
	}

	private float vdistSqr(float[] v1, float[] v2)
	{
		float num = v2[0] - v1[0];
		float num2 = v2[1] - v1[1];
		float num3 = v2[2] - v1[2];
		return num * num + num2 * num2 + num3 * num3;
	}

	private float triArea2D(float[] a, float[] b, ref Vector3 c)
	{
		return (b[0] * a[2] - a[0] * b[2] + (c.X * b[2] - b[0] * c.Z) + (a[0] * c.Z - c.X * a[2])) * 0.5f;
	}

	private bool checkOverlapBox(ushort[] amin, ushort[] amax, ushort[] bmin, ushort[] bmax)
	{
		bool flag = true;
		flag = amin[0] <= bmax[0] && amax[0] >= bmin[0] && flag;
		flag = amin[1] <= bmax[1] && amax[1] >= bmin[1] && flag;
		return amin[2] <= bmax[2] && amax[2] >= bmin[2] && flag;
	}

	private bool vequal(float[] p0, float[] p1)
	{
		float num = vdistSqr(p0, p1);
		return num < thr;
	}

	private float distancePtLine2d(ref Vector3 pt, ref Vector3 p, ref Vector3 q)
	{
		float num = q.X - p.X;
		float num2 = q.Z - p.Z;
		float num3 = pt.X - p.X;
		float num4 = pt.Z - p.Z;
		float num5 = num * num + num2 * num2;
		float num6 = num * num3 + num2 * num4;
		if (num5 != 0f)
		{
			num6 /= num5;
		}
		num3 = p.X + num6 * num - pt.X;
		num4 = p.Z + num6 * num2 - pt.Z;
		return num3 * num3 + num4 * num4;
	}

	private void closestPtPointTriangle(ref Vector3 closest, ref Vector3 p, ref Vector3 a, ref Vector3 b, ref Vector3 c)
	{
		Vector3 vector = b - a;
		Vector3 vector2 = c - a;
		Vector3 vector3 = p - a;
		float num = Vector3.Dot(vector, vector3);
		float num2 = Vector3.Dot(vector2, vector3);
		if (num <= 0f && num2 <= 0f)
		{
			closest = a;
			return;
		}
		Vector3 vector4 = p - b;
		float num3 = Vector3.Dot(vector, vector4);
		float num4 = Vector3.Dot(vector2, vector4);
		if (num3 >= 0f && num4 <= num3)
		{
			closest = b;
			return;
		}
		float num5 = num * num4 - num3 * num2;
		if (num5 <= 0f && num >= 0f && num3 <= 0f)
		{
			float num6 = num / (num - num3);
			closest = a + num6 * vector;
			return;
		}
		Vector3 vector5 = p - c;
		float num7 = Vector3.Dot(vector, vector5);
		float num8 = Vector3.Dot(vector2, vector5);
		if (num8 >= 0f && num7 <= num8)
		{
			closest = c;
			return;
		}
		float num9 = num7 * num2 - num * num8;
		if (num9 <= 0f && num2 >= 0f && num8 <= 0f)
		{
			float num10 = num2 / (num2 - num8);
			closest = a + num10 * vector2;
			return;
		}
		float num11 = num3 * num8 - num7 * num4;
		if (num11 <= 0f && num4 - num3 >= 0f && num7 - num8 >= 0f)
		{
			float num12 = (num4 - num3) / (num4 - num3 + (num7 - num8));
			closest = b + num12 * (c - b);
			return;
		}
		float num13 = 1f / (num11 + num9 + num5);
		float num14 = num9 * num13;
		float num15 = num5 * num13;
		closest = a + vector * num14 + vector2 * num15;
	}
}
