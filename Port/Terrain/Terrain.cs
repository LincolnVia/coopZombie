using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Models;
using Rendering;
using Structs;
using Textures;
using WindowsGame1;

namespace Terrain;

public class Terrain
{
	public static byte terrainMode = 0;

	public static byte[] terrainTextureData;

	public static ushort terrainCollisionModelGID;

	public static ushort texTerrainFar;

	public static ushort texTerrainNear;

	public static ushort texTerrainMedium;

	public static ushort texHeightMap;

	public static ushort modTerrain = 0;

	public static ushort modTerrainCollision = 0;

	public static ushort numTilesX;

	public static ushort numTilesY;

	public static ushort numTilesXPlus1;

	public static ushort numTilesYPlus1;

	public static ushort textureWidth;

	public static ushort textureHeight;

	public static ushort textureWidthMinusOne;

	public static ushort textureHeightMinusOne;

	public static float terrainTexureMultiplier = 1f;

	public static float terrainBaseHeight = 0f;

	public static float terrainMaxHeight = 1f;

	public static float terrainHeightAdj = 1f;

	public static float terrainHeightMultiplier;

	public static float terrainWidth;

	public static float terrainHeight;

	public static float terrainOriginX;

	public static float terrainOriginY;

	public static float terrainX;

	public static float terrainY;

	public static float[,] terrainPoints;

	public static int numTerrainPrimitives;

	public static int numTerrainVertices;

	public static string terrainFarTexture;

	public static string terrainMediumTexture;

	public static string terrainNearTexture;

	public static Matrix mvT;

	public static VertexBuffer terrainVertexBufferObjects;

	public static IndexBuffer terrainIndexBufferObjects;

	public Game1.MasterCollection mainC;

	public void Init_Vars(Game1.MasterCollection masterC)
	{
		mainC = masterC;
	}

	public void Init_Terrain(string fileName, string textureName, bool useModelForTerrain, float offsetX, float offsetY, float offsetZ, float baseHeight, float maxHeight)
	{
		_ = global::Rendering.Rendering.uBufferID;
		int num = 0;
		int num2 = 0;
		terrainCollisionModelGID = mainC.maingameMain.Register_Game_Item(5, 0, 0);
		terrainBaseHeight = baseHeight;
		terrainMaxHeight = maxHeight;
		mvT = Matrix.CreateTranslation(offsetX, offsetY, offsetZ);
		terrainFarTexture = textureName;
		terrainMediumTexture = "cartoongrass2";
		terrainNearTexture = "cartoongrass";
		texTerrainFar = (ushort)mainC.texturesMain.Find_Texture(textureName, 0);
		texTerrainMedium = (ushort)mainC.texturesMain.Find_Texture(terrainMediumTexture, 0);
		texTerrainNear = (ushort)mainC.texturesMain.Find_Texture(terrainNearTexture, 0);
		if (useModelForTerrain)
		{
			modTerrain = (ushort)mainC.modelsMain.Find_Level_Model(fileName);
			terrainMode = 1;
			terrainHeightAdj = 0.05f * terrainMaxHeight;
			return;
		}
		terrainMode = 2;
		terrainMaxHeight = terrainBaseHeight - 1f;
		new StructsClass.vtex();
		new StructsClass.vtex();
		new StructsClass.vtex();
		new StructsClass.vtex();
		new StructsClass.texcoord();
		new StructsClass.texcoord();
		new StructsClass.texcoord();
		texHeightMap = (ushort)mainC.texturesMain.Find_Texture(fileName, 0);
		terrainHeightMultiplier = 4f;
		terrainWidth = 16000f;
		terrainHeight = 16000f;
		terrainOriginX = 0f;
		terrainOriginY = 0f;
		textureWidth = 1024;
		textureHeight = 1024;
		textureWidthMinusOne = (ushort)(textureWidth - 1);
		textureHeightMinusOne = (ushort)(textureWidth - 1);
		terrainTexureMultiplier = 2f;
		numTilesX = 100;
		numTilesY = 100;
		numTilesXPlus1 = (ushort)(numTilesX + 1);
		numTilesYPlus1 = (ushort)(numTilesY + 1);
		terrainX = terrainWidth / (float)(int)numTilesX;
		terrainY = terrainHeight / (float)(int)numTilesY;
		terrainTextureData = new byte[textureWidth * textureHeight * 4];
		global::Textures.Textures.texMain.texData[texHeightMap].GetData(terrainTextureData);
		int[] array = new int[numTilesX * numTilesY * 6];
		terrainPoints = new float[numTilesXPlus1 * numTilesYPlus1, 4];
		numTerrainPrimitives = numTilesX * numTilesY * 2;
		numTerrainVertices = numTilesXPlus1 * numTilesYPlus1;
		StructsClass.VertexPositionColorNormalTexture[] array2 = new StructsClass.VertexPositionColorNormalTexture[numTerrainVertices];
		int i = 0;
		float num3 = terrainHeight + terrainOriginY;
		for (; i < numTilesYPlus1; i++)
		{
			float num4 = terrainOriginX;
			for (int j = 0; j < numTilesXPlus1; j++)
			{
				num2 = (int)((float)j / (float)(int)numTilesX * (float)(int)textureWidthMinusOne) + (int)((float)i / (float)(int)numTilesY * (float)(int)textureHeightMinusOne) * textureWidth;
				float num5 = (float)(int)terrainTextureData[num2 * 4 + 1] * terrainHeightMultiplier;
				if (num5 > terrainMaxHeight)
				{
					terrainMaxHeight = num5;
				}
				array2[num].Set_Values(num4, num3, num5, 0f, 0f, 1f, 1f, 0f, 0f, (float)j / (float)(int)numTilesX * terrainTexureMultiplier, (float)i / (float)(int)numTilesY * terrainTexureMultiplier, 1f, 1f, 1f, 1f, 0, 0, 0, 0, 1f, 0f, 0f, 0f);
				terrainPoints[num, 0] = num5;
				terrainPoints[num, 1] = 0f;
				terrainPoints[num, 2] = 0f;
				terrainPoints[num, 3] = 1f;
				num++;
				num4 += terrainX;
			}
			num3 -= terrainY;
		}
		terrainMaxHeight += terrainBaseHeight;
		terrainHeightAdj = 0.05f * terrainMaxHeight;
		i = 1;
		num = numTilesXPlus1 + 1;
		for (; i < numTilesY; i++)
		{
			for (int j = 1; j < numTilesX; j++)
			{
				float num6 = array2[num - numTilesXPlus1].Z - array2[num].Z;
				float num7 = (float)Math.Sqrt(terrainY * terrainY + num6 * num6);
				num6 /= num7;
				num7 = terrainY / num7;
				float num8 = array2[num + numTilesXPlus1].Z - array2[num].Z;
				float num9 = (float)Math.Sqrt(terrainY * terrainY + num8 * num8);
				num8 /= num9;
				num9 = (0f - terrainY) / num9;
				float num10 = array2[num + 1].Z - array2[num].Z;
				float num11 = (float)Math.Sqrt(terrainX * terrainX + num10 * num10);
				num10 /= num11;
				num11 = terrainX / num11;
				float num12 = array2[num - 1].Z - array2[num].Z;
				float num13 = (float)Math.Sqrt(terrainX * terrainX + num12 * num12);
				num12 /= num13;
				num13 = (0f - terrainX) / num13;
				float num4 = 0f - num10 + num12;
				num3 = 0f - num6 + num8;
				float num14 = num7 - num9 + num11 - num13;
				float num5 = (float)Math.Sqrt(num4 * num4 + num3 * num3 + num14 * num14);
				if (num5 != 0f)
				{
					num4 /= num5;
					num3 /= num5;
					num14 /= num5;
					array2[num].nX = num4;
					array2[num].nY = num3;
					array2[num].nZ = num14;
					terrainPoints[num, 1] = num4;
					terrainPoints[num, 2] = num3;
					terrainPoints[num, 3] = num14;
				}
				num++;
			}
			num += 2;
		}
		i = 0;
		num2 = 0;
		for (; i < numTilesY; i++)
		{
			num = i * numTilesXPlus1;
			for (int j = 0; j < numTilesX; j++)
			{
				array[num2++] = num + j;
				array[num2++] = num + j + numTilesXPlus1;
				array[num2++] = num + j + 1;
				array[num2++] = num + j + 1;
				array[num2++] = num + j + numTilesXPlus1;
				array[num2++] = num + j + numTilesXPlus1 + 1;
			}
		}
		terrainIndexBufferObjects = new IndexBuffer(global::Rendering.Rendering.rGraphics, typeof(int), array.Length, BufferUsage.WriteOnly);
		terrainIndexBufferObjects.SetData(array);
		terrainVertexBufferObjects = new VertexBuffer(global::Rendering.Rendering.rGraphics, global::Rendering.Rendering.rDecVPCNT, numTerrainVertices, BufferUsage.None);
		terrainVertexBufferObjects.SetData(array2);
	}

	public void Render_Terrain()
	{
		if (terrainMode != 0)
		{
			global::Rendering.Rendering.rGraphics.BlendState = BlendState.Opaque;
			global::Rendering.Rendering.effect1.CurrentTechnique = global::Rendering.Rendering.effect1.Techniques["Terrain"];
			global::Rendering.Rendering.effect1.Parameters["World"].SetValue(mvT);
			global::Rendering.Rendering.effect1.Parameters["MaterialTexture"].SetValue(global::Textures.Textures.texMain.texData[global::Textures.Textures.texDefaultNormalMap]);
			global::Rendering.Rendering.effect1.Parameters["BaseTexture"].SetValue(global::Textures.Textures.texMain.texData[texTerrainFar]);
			global::Rendering.Rendering.effect1.Parameters["MaterialTexture2"].SetValue(global::Textures.Textures.texMain.texData[texTerrainMedium]);
			global::Rendering.Rendering.effect1.Parameters["MaterialTexture3"].SetValue(global::Textures.Textures.texMain.texData[texTerrainNear]);
			global::Rendering.Rendering.effect1.CurrentTechnique.Passes[0].Apply();
			switch (terrainMode)
			{
			case 1:
				global::Rendering.Rendering.rGraphics.SetVertexBuffer(global::Models.Models.mVertexBufferObjectsLevel);
				global::Rendering.Rendering.rGraphics.Indices = global::Models.Models.mIndexBufferObjectsLevel;
				global::Rendering.Rendering.rGraphics.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, global::Models.Models.modVbo[modTerrain].vbStart, global::Models.Models.modVbo[modTerrain].vcount, global::Models.Models.modVbo[modTerrain].ibStart + global::Models.Models.modVbo[modTerrain].textureListStart[0], global::Models.Models.modVbo[modTerrain].textureListPrimitiveCnt[0]);
				break;
			case 2:
				global::Rendering.Rendering.rGraphics.Indices = terrainIndexBufferObjects;
				global::Rendering.Rendering.rGraphics.SetVertexBuffer(terrainVertexBufferObjects);
				global::Rendering.Rendering.rGraphics.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, numTerrainVertices, 0, numTerrainPrimitives);
				break;
			}
			global::Rendering.Rendering.effect1.CurrentTechnique.Passes[0].Apply();
		}
	}

	public void Render_Terrain_Shadowmap()
	{
		switch (terrainMode)
		{
		case 1:
			global::Rendering.Rendering.rGraphics.SetVertexBuffer(global::Models.Models.mVertexBufferObjectsLevel);
			global::Rendering.Rendering.rGraphics.Indices = global::Models.Models.mIndexBufferObjectsLevel;
			global::Rendering.Rendering.rGraphics.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, global::Models.Models.modVbo[modTerrain].vbStart, global::Models.Models.modVbo[modTerrain].vcount, global::Models.Models.modVbo[modTerrain].ibStart + global::Models.Models.modVbo[modTerrain].textureListStart[0], global::Models.Models.modVbo[modTerrain].textureListPrimitiveCnt[0]);
			break;
		case 2:
			global::Rendering.Rendering.rGraphics.Indices = terrainIndexBufferObjects;
			global::Rendering.Rendering.rGraphics.SetVertexBuffer(terrainVertexBufferObjects);
			global::Rendering.Rendering.rGraphics.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, numTerrainVertices, 0, numTerrainPrimitives);
			break;
		}
	}

	public float Get_Terrain_Height(float x, float y, byte threadID)
	{
		try
		{
			switch (terrainMode)
			{
			case 0:
				return terrainBaseHeight;
			case 1:
			{
				Vector3 InitialRayStart = new Vector3(x, y, terrainMaxHeight + terrainHeightAdj);
				Vector3 InitialRayEnd = new Vector3(x, y, terrainBaseHeight - terrainHeightAdj);
				Vector3 IntersectPosition = default(Vector3);
				Vector3 IntersectNormal = default(Vector3);
				if (mainC.collisionMain.Check_Polygon_Ray_Collision(modTerrainCollision, -1, ref InitialRayStart, ref InitialRayEnd, ref mvT, out var _, out IntersectPosition, out IntersectNormal, out var _, out var _, threadID))
				{
					return IntersectPosition.Z;
				}
				return terrainBaseHeight;
			}
			case 2:
			{
				x -= terrainOriginX;
				y -= terrainOriginY;
				if (x < 0f || y < 0f || x > terrainWidth || y > terrainHeight)
				{
					return terrainBaseHeight;
				}
				int num = (int)(x / terrainX);
				int num2 = numTilesY - (int)Math.Ceiling(y / terrainY);
				if (num != numTilesX && num2 != numTilesY)
				{
					float num3 = x % terrainX / terrainX;
					float num4 = y % terrainY / terrainY;
					int num5 = num + num2 * numTilesXPlus1;
					float num6 = terrainPoints[num5, 0] + (terrainPoints[num5 + 1, 0] - terrainPoints[num5, 0]) * num3;
					num5 += numTilesXPlus1;
					float num7 = terrainPoints[num5, 0] + (terrainPoints[num5 + 1, 0] - terrainPoints[num5, 0]) * num3;
					return num7 + (num6 - num7) * num4 + terrainBaseHeight;
				}
				return terrainBaseHeight;
			}
			default:
				return terrainBaseHeight;
			}
		}
		catch
		{
			return terrainBaseHeight;
		}
	}

	public void Set_Terrain_Collision_Model(string collisionModel, ushort pcZone, ushort wcZone)
	{
		modTerrainCollision = mainC.collisionMain.Find_Collision_Model(collisionModel, 0);
		mainC.zonesMain.Add_CollisionModel_To_Zone(pcZone, modTerrainCollision, terrainCollisionModelGID, ref mvT);
		mainC.zonesMain.Add_CollisionModel_To_Zone(wcZone, modTerrainCollision, terrainCollisionModelGID, ref mvT);
	}

	public void Set_Terrain_Collision_Model_Tile(string collisionModel, ushort pcZone, float xOffset, float yOffset)
	{
		Matrix mv = Matrix.CreateTranslation(xOffset, yOffset, 0f) * mvT;
		modTerrainCollision = mainC.collisionMain.Find_Collision_Model(collisionModel, 0);
		mainC.zonesMain.Add_CollisionModel_To_Zone(pcZone, modTerrainCollision, terrainCollisionModelGID, ref mv);
	}

	public void Update_Terrain_Textures()
	{
		texTerrainFar = (ushort)mainC.texturesMain.Find_Texture(terrainFarTexture, 0);
		texTerrainMedium = (ushort)mainC.texturesMain.Find_Texture(terrainMediumTexture, 0);
		texTerrainNear = (ushort)mainC.texturesMain.Find_Texture(terrainNearTexture, 0);
	}
}
