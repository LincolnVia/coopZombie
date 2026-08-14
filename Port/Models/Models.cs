using System;
using System.Globalization;
using System.IO;
using Collision;
using InputHandler;
using Joints;
using MainGame;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Rendering;
using Structs;
using Textures;
using Util;
using WindowsGame1;

namespace Models;

public class Models
{
	public static byte numModelLists;

	public static byte numAllocatedModelLists;

	public static int modShield;

	public static int modBillboard;

	public static int modHalfSphere;

	public static int modCylinder;

	public static int modGeometryFlatCircle8;

	public static int modGeometryFlatCircle16;

	public static int modFlatPlane;

	public static int modSkyDome;

	public static int modSquare;

	public static int modTransporter;

	public static int modCHud;

	public static int modCHudBar;

	public static int modMarker;

	public static int modMarker2;

	public static int modMarker3;

	public static int modMarker4;

	public static int modMarker5;

	public static int modDoorSwitchScreen;

	public static int numLevelModels = 0;

	public static int numAllocatedLevelModels = 0;

	public static int numModels = 0;

	public static float loadingScreenModelDegPerSec = 320f;

	public static float[] texAdj = new float[2];

	public static StructsClass.physics weaponLoc;

	public static StructsClass.model[] mod1;

	public static StructsClass.model[] modVbo;

	public static StructsClass.model tempModel;

	public static StructsClass.model mod2;

	public static StructsClass.CollisionModel tempColModel;

	public static StructsClass.Model_List[] modelList;

	public static Matrix mainMV = default(Matrix);

	public static StructsClass.vtex ctV1;

	public static StructsClass.vtex ctV2;

	public static StructsClass.vtex ctV3;

	public static StructsClass.texcoord ctT1;

	public static StructsClass.texcoord ctT2;

	public static StructsClass.texcoord ctT3;

	public static Color cvsmC1 = default(Color);

	public static Vector3 cvsmPos = default(Vector3);

	public static Vector3 cvsmNorm = default(Vector3);

	public static Vector3 cvsmTang = default(Vector3);

	public static Vector3 cvsmModPos;

	public static Vector3 cvsmModNorm;

	public static Vector3 cvsmModTang;

	public static Vector3 cvsmOrigin;

	public static GraphicsDevice mGraphics;

	public static VertexBuffer mVertexBufferObjects;

	public static VertexBuffer mVertexBufferObjectsLevel;

	public static IndexBuffer mIndexBufferObjects;

	public static IndexBuffer mIndexBufferObjectsLevel;

	public static int[] mViObjects;

	public static int[] mViObjectsLevel;

	public static StructsClass.VertexPositionColorNormalTexture[] mVtexObjects;

	public static StructsClass.VertexPositionColorNormalTexture[] mVtexObjectsLevel;

	public Game1.MasterCollection mainC;

	public void Init_Vars(Game1.MasterCollection master, GraphicsDevice graphics)
	{
		mainC = master;
		mGraphics = graphics;
	}

	public void Render_Model_List_Main_VBO(ushort listID, ref Matrix mv)
	{
		int num = modelList[listID].numModels;
		global::Rendering.Rendering.effect1.Parameters["World"].SetValue(mv);
		for (int i = 0; i < num; i++)
		{
			int num2 = modelList[listID].modelID[i];
			if (mod1[num2].bufferType == 1)
			{
				global::Rendering.Rendering.effect1.CurrentTechnique = global::Rendering.Rendering.effect1.Techniques["TextureMove"];
				texAdj[0] = mod1[num2].texMovX;
				texAdj[1] = mod1[num2].texMovY;
				global::Rendering.Rendering.effect1.Parameters["texAdj"].SetValue(texAdj);
				global::Rendering.Rendering.effect1.Parameters["BaseTexture"].SetValue(global::Textures.Textures.texMain.texData[modelList[listID].texID[i]]);
				global::Rendering.Rendering.effect1.Parameters["MaterialTexture"].SetValue(global::Textures.Textures.texMain.texData[modelList[listID].texID2[i]]);
			}
			else if (modelList[listID].texID[i] > -1)
			{
				global::Rendering.Rendering.effect1.CurrentTechnique = global::Rendering.Rendering.effect1.Techniques["Main"];
				global::Rendering.Rendering.effect1.Parameters["BaseTexture"].SetValue(global::Textures.Textures.texMain.texData[modelList[listID].texID[i]]);
				global::Rendering.Rendering.effect1.Parameters["MaterialTexture"].SetValue(global::Textures.Textures.texMain.texData[modelList[listID].texID2[i]]);
			}
			if (modelList[listID].alphaBlend[i] == 1)
			{
				switch (mod1[num2].blendFunction)
				{
				case 0:
					mGraphics.BlendState = global::Rendering.Rendering.blendSourceAlpha;
					break;
				case 1:
					mGraphics.BlendState = global::Rendering.Rendering.blendSourceOne;
					break;
				default:
					mGraphics.BlendState = BlendState.Opaque;
					break;
				}
			}
			else
			{
				mGraphics.BlendState = BlendState.Opaque;
			}
			mGraphics.SetVertexBuffer(mVertexBufferObjects);
			mGraphics.Indices = mIndexBufferObjects;
			mGraphics.RasterizerState = RasterizerState.CullClockwise;
			global::Rendering.Rendering.effect1.CurrentTechnique.Passes[0].Apply();
			mGraphics.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, mod1[num2].ibStart, mod1[num2].vcount, mod1[num2].ibStart, mod1[num2].triangleCount);
			global::Rendering.Rendering.effect1.Parameters["AlphaAdjust"].SetValue(1f);
			if (mod1[num2].bufferType == 1)
			{
				global::Rendering.Rendering.effect1.CurrentTechnique = global::Rendering.Rendering.effect1.Techniques["Main"];
			}
			global::Rendering.Rendering.effect1.CurrentTechnique.Passes[0].Apply();
		}
	}

	public void Render_Model_List_Level_VBO(byte listID, ref Matrix mv)
	{
		int num = modelList[listID].numModels;
		for (int i = 0; i < num; i++)
		{
			int num2 = modelList[listID].modelID[i];
			global::Rendering.Rendering.effect1.Parameters["World"].SetValue(mv);
			if (modVbo[num2].bufferType == 1)
			{
				global::Rendering.Rendering.effect1.CurrentTechnique = global::Rendering.Rendering.effect1.Techniques["TextureMove"];
				texAdj[0] = modVbo[num2].texMovX;
				texAdj[1] = modVbo[num2].texMovY;
				global::Rendering.Rendering.effect1.Parameters["texAdj"].SetValue(texAdj);
				global::Rendering.Rendering.effect1.Parameters["BaseTexture"].SetValue(global::Textures.Textures.texMain.texData[modelList[listID].texID[i]]);
				global::Rendering.Rendering.effect1.Parameters["MaterialTexture"].SetValue(global::Textures.Textures.texMain.texData[modelList[listID].texID2[i]]);
			}
			else if (modelList[listID].texID[i] > -1)
			{
				global::Rendering.Rendering.effect1.CurrentTechnique = global::Rendering.Rendering.effect1.Techniques["Main"];
				global::Rendering.Rendering.effect1.Parameters["BaseTexture"].SetValue(global::Textures.Textures.texMain.texData[modelList[listID].texID[i]]);
				global::Rendering.Rendering.effect1.Parameters["MaterialTexture"].SetValue(global::Textures.Textures.texMain.texData[modelList[listID].texID2[i]]);
			}
			if (modelList[listID].alphaBlend[i] == 1)
			{
				switch (mod1[num2].blendFunction)
				{
				case 0:
					mGraphics.BlendState = global::Rendering.Rendering.blendSourceAlpha;
					break;
				case 1:
					mGraphics.BlendState = global::Rendering.Rendering.blendSourceOne;
					break;
				default:
					mGraphics.BlendState = BlendState.Opaque;
					break;
				}
			}
			else
			{
				mGraphics.BlendState = BlendState.Opaque;
			}
			mGraphics.SetVertexBuffer(mVertexBufferObjectsLevel);
			mGraphics.Indices = mIndexBufferObjectsLevel;
			mGraphics.RasterizerState = RasterizerState.CullClockwise;
			global::Rendering.Rendering.effect1.CurrentTechnique.Passes[0].Apply();
			mGraphics.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, modVbo[num2].vbStart, modVbo[num2].vcount, modVbo[num2].vbStart, modVbo[num2].triangleCount);
			global::Rendering.Rendering.effect1.Parameters["AlphaAdjust"].SetValue(1f);
			if (modVbo[num2].bufferType == 1)
			{
				global::Rendering.Rendering.effect1.CurrentTechnique = global::Rendering.Rendering.effect1.Techniques["Main"];
			}
			global::Rendering.Rendering.effect1.CurrentTechnique.Passes[0].Apply();
		}
	}

	public void Render_Model_List_Single(ref StructsClass.Model_List ml1, ref Matrix mv)
	{
		int num = ml1.numModels;
		for (int i = 0; i < num; i++)
		{
			int num2 = ml1.modelID[i];
			global::Rendering.Rendering.effect1.Parameters["World"].SetValue(mv);
			if (mod1[num2].bufferType == 1)
			{
				global::Rendering.Rendering.effect1.CurrentTechnique = global::Rendering.Rendering.effect1.Techniques["TextureMove"];
				texAdj[0] = mod1[num2].texMovX;
				texAdj[1] = mod1[num2].texMovY;
				global::Rendering.Rendering.effect1.Parameters["texAdj"].SetValue(texAdj);
				global::Rendering.Rendering.effect1.Parameters["BaseTexture"].SetValue(global::Textures.Textures.texMain.texData[ml1.texID[i]]);
				global::Rendering.Rendering.effect1.Parameters["MaterialTexture"].SetValue(global::Textures.Textures.texMain.texData[ml1.texID2[i]]);
			}
			else if (ml1.texID[i] > -1)
			{
				global::Rendering.Rendering.effect1.CurrentTechnique = global::Rendering.Rendering.effect1.Techniques["Main"];
				global::Rendering.Rendering.effect1.Parameters["BaseTexture"].SetValue(global::Textures.Textures.texMain.texData[ml1.texID[i]]);
				global::Rendering.Rendering.effect1.Parameters["MaterialTexture"].SetValue(global::Textures.Textures.texMain.texData[ml1.texID2[i]]);
			}
			if (ml1.alphaBlend[i] == 1)
			{
				switch (mod1[num2].blendFunction)
				{
				case 0:
					mGraphics.BlendState = global::Rendering.Rendering.blendSourceAlpha;
					break;
				case 1:
					mGraphics.BlendState = global::Rendering.Rendering.blendSourceOne;
					break;
				default:
					mGraphics.BlendState = BlendState.Opaque;
					break;
				}
			}
			else
			{
				mGraphics.BlendState = BlendState.Opaque;
			}
			mGraphics.SetVertexBuffer(mVertexBufferObjects);
			mGraphics.Indices = mIndexBufferObjects;
			mGraphics.RasterizerState = RasterizerState.CullClockwise;
			global::Rendering.Rendering.effect1.CurrentTechnique.Passes[0].Apply();
			mGraphics.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, mod1[num2].ibStart, mod1[num2].vcount, mod1[num2].ibStart, mod1[num2].triangleCount);
			global::Rendering.Rendering.effect1.Parameters["AlphaAdjust"].SetValue(1f);
			if (mod1[num2].bufferType == 1)
			{
				global::Rendering.Rendering.effect1.CurrentTechnique = global::Rendering.Rendering.effect1.Techniques["Main"];
			}
			global::Rendering.Rendering.effect1.CurrentTechnique.Passes[0].Apply();
		}
	}

	public void Render_Model_List_Item(ushort listID, ushort itemID)
	{
		int num = modelList[listID].modelID[itemID];
		if (num >= numModels || num < 0)
		{
			return;
		}
		if (mod1[num].bufferType == 1)
		{
			global::Rendering.Rendering.effect1.CurrentTechnique = global::Rendering.Rendering.effect1.Techniques["TextureMove"];
			texAdj[0] = mod1[num].texMovX;
			texAdj[1] = mod1[num].texMovY;
			global::Rendering.Rendering.effect1.Parameters["texAdj"].SetValue(texAdj);
			global::Rendering.Rendering.effect1.Parameters["BaseTexture"].SetValue(global::Textures.Textures.texMain.texData[modelList[listID].texID[itemID]]);
			global::Rendering.Rendering.effect1.Parameters["MaterialTexture"].SetValue(global::Textures.Textures.texMain.texData[mod1[num].texNormalID]);
		}
		else if (mod1[num].texID > -1)
		{
			global::Rendering.Rendering.effect1.CurrentTechnique = global::Rendering.Rendering.effect1.Techniques["Main"];
			global::Rendering.Rendering.effect1.Parameters["BaseTexture"].SetValue(global::Textures.Textures.texMain.texData[modelList[listID].texID[itemID]]);
			global::Rendering.Rendering.effect1.Parameters["MaterialTexture"].SetValue(global::Textures.Textures.texMain.texData[mod1[num].texNormalID]);
		}
		if (mod1[num].defaultColor[3] < 1f || mod1[num].usesAlpha)
		{
			global::Rendering.Rendering.effect1.Parameters["AlphaAdjust"].SetValue(mod1[num].defaultColor[3]);
			switch (mod1[num].blendFunction)
			{
			case 0:
				mGraphics.BlendState = global::Rendering.Rendering.blendSourceAlpha;
				break;
			case 1:
				mGraphics.BlendState = global::Rendering.Rendering.blendSourceOne;
				break;
			default:
				mGraphics.BlendState = BlendState.Opaque;
				break;
			}
		}
		else
		{
			mGraphics.BlendState = BlendState.Opaque;
		}
		mGraphics.SetVertexBuffer(mVertexBufferObjects);
		mGraphics.Indices = mIndexBufferObjects;
		mGraphics.RasterizerState = RasterizerState.CullClockwise;
		global::Rendering.Rendering.effect1.CurrentTechnique.Passes[0].Apply();
		mGraphics.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, mod1[num].vbStart, mod1[num].vcount, mod1[num].ibStart + mod1[num].textureListStart[0], mod1[num].textureListPrimitiveCnt[0]);
		global::Rendering.Rendering.effect1.Parameters["AlphaAdjust"].SetValue(1f);
		if (mod1[num].bufferType == 1)
		{
			global::Rendering.Rendering.effect1.CurrentTechnique = global::Rendering.Rendering.effect1.Techniques["Main"];
		}
		global::Rendering.Rendering.effect1.CurrentTechnique.Passes[0].Apply();
	}

	public void Render_Model_List_Item_Basic(ushort listID, ushort itemID)
	{
		int num = modelList[listID].modelID[itemID];
		if (num >= numModels || num < 0)
		{
			return;
		}
		global::Rendering.Rendering.effect1.Parameters["BaseTexture"].SetValue(global::Textures.Textures.texMain.texData[modelList[listID].texID[itemID]]);
		global::Rendering.Rendering.effect1.Parameters["MaterialTexture"].SetValue(global::Textures.Textures.texMain.texData[mod1[num].texNormalID]);
		if (mod1[num].defaultColor[3] < 1f || mod1[num].usesAlpha)
		{
			global::Rendering.Rendering.effect1.Parameters["AlphaAdjust"].SetValue(mod1[num].defaultColor[3]);
			switch (mod1[num].blendFunction)
			{
			case 0:
				mGraphics.BlendState = global::Rendering.Rendering.blendSourceAlpha;
				break;
			case 1:
				mGraphics.BlendState = global::Rendering.Rendering.blendSourceOne;
				break;
			default:
				mGraphics.BlendState = BlendState.Opaque;
				break;
			}
		}
		else
		{
			mGraphics.BlendState = BlendState.Opaque;
		}
		mGraphics.SetVertexBuffer(mVertexBufferObjects);
		mGraphics.Indices = mIndexBufferObjects;
		mGraphics.RasterizerState = RasterizerState.CullClockwise;
		global::Rendering.Rendering.effect1.CurrentTechnique.Passes[0].Apply();
		mGraphics.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, mod1[num].vbStart, mod1[num].vcount, mod1[num].ibStart + mod1[num].textureListStart[0], mod1[num].textureListPrimitiveCnt[0]);
		global::Rendering.Rendering.effect1.Parameters["AlphaAdjust"].SetValue(1f);
		global::Rendering.Rendering.effect1.CurrentTechnique.Passes[0].Apply();
	}

	public void Render_Model(long modID, ref Matrix mv)
	{
		if (modID >= numModels || modID < 0)
		{
			return;
		}
		global::Rendering.Rendering.effect1.Parameters["World"].SetValue(mv);
		if (mod1[modID].bufferType == 1)
		{
			global::Rendering.Rendering.effect1.CurrentTechnique = global::Rendering.Rendering.effect1.Techniques["TextureMove"];
			texAdj[0] = mod1[modID].texMovX;
			texAdj[1] = mod1[modID].texMovY;
			global::Rendering.Rendering.effect1.Parameters["texAdj"].SetValue(texAdj);
			global::Rendering.Rendering.effect1.Parameters["MaterialTexture"].SetValue(global::Textures.Textures.texMain.texData[mod1[modID].texNormalID]);
		}
		else if (mod1[modID].texID > -1)
		{
			global::Rendering.Rendering.effect1.CurrentTechnique = global::Rendering.Rendering.effect1.Techniques["Main"];
			global::Rendering.Rendering.effect1.Parameters["MaterialTexture"].SetValue(global::Textures.Textures.texMain.texData[mod1[modID].texNormalID]);
			global::Rendering.Rendering.effect1.Parameters["SpecularTexture"].SetValue(global::Textures.Textures.texMain.texData[mod1[modID].texSpecularID]);
		}
		if (mod1[modID].defaultColor[3] < 1f || mod1[modID].usesAlpha)
		{
			mGraphics.RasterizerState = RasterizerState.CullNone;
			global::Rendering.Rendering.effect1.Parameters["AlphaAdjust"].SetValue(mod1[modID].defaultColor[3]);
			switch (mod1[modID].blendFunction)
			{
			case 0:
				mGraphics.BlendState = global::Rendering.Rendering.blendSourceAlpha;
				break;
			case 1:
				mGraphics.BlendState = global::Rendering.Rendering.blendSourceOne;
				break;
			default:
				mGraphics.BlendState = BlendState.Opaque;
				break;
			}
		}
		else
		{
			mGraphics.BlendState = BlendState.Opaque;
		}
		mGraphics.SetVertexBuffer(mVertexBufferObjects);
		mGraphics.Indices = mIndexBufferObjects;
		int numTextures = mod1[modID].numTextures;
		for (int i = 0; i < numTextures; i++)
		{
			global::Rendering.Rendering.effect1.Parameters["BaseTexture"].SetValue(global::Textures.Textures.texMain.texData[mod1[modID].textureList[i]]);
			global::Rendering.Rendering.effect1.CurrentTechnique.Passes[0].Apply();
			mGraphics.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, mod1[modID].vbStart, mod1[modID].vcount, mod1[modID].ibStart + mod1[modID].textureListStart[i], mod1[modID].textureListPrimitiveCnt[i]);
		}
		mGraphics.RasterizerState = RasterizerState.CullClockwise;
		global::Rendering.Rendering.effect1.Parameters["AlphaAdjust"].SetValue(1f);
		if (mod1[modID].bufferType == 1)
		{
			global::Rendering.Rendering.effect1.CurrentTechnique = global::Rendering.Rendering.effect1.Techniques["Main"];
		}
		global::Rendering.Rendering.effect1.CurrentTechnique.Passes[0].Apply();
	}

	public void Render_Level_Model(long modID, ref Matrix mv)
	{
		if (modID >= numLevelModels || modID < 0)
		{
			return;
		}
		global::Rendering.Rendering.effect1.Parameters["World"].SetValue(mv);
		if (modVbo[modID].bufferType == 1)
		{
			global::Rendering.Rendering.effect1.CurrentTechnique = global::Rendering.Rendering.effect1.Techniques["TextureMove"];
			texAdj[0] = modVbo[modID].texMovX;
			texAdj[1] = modVbo[modID].texMovY;
			global::Rendering.Rendering.effect1.Parameters["texAdj"].SetValue(texAdj);
			global::Rendering.Rendering.effect1.Parameters["BaseTexture"].SetValue(global::Textures.Textures.texMain.texData[modVbo[modID].texID]);
			global::Rendering.Rendering.effect1.Parameters["MaterialTexture"].SetValue(global::Textures.Textures.texMain.texData[modVbo[modID].texNormalID]);
		}
		else if (modVbo[modID].texID > -1)
		{
			global::Rendering.Rendering.effect1.CurrentTechnique = global::Rendering.Rendering.effect1.Techniques["Main"];
			global::Rendering.Rendering.effect1.Parameters["BaseTexture"].SetValue(global::Textures.Textures.texMain.texData[modVbo[modID].texID]);
			global::Rendering.Rendering.effect1.Parameters["MaterialTexture"].SetValue(global::Textures.Textures.texMain.texData[modVbo[modID].texNormalID]);
		}
		if (modVbo[modID].defaultColor[3] < 1f || modVbo[modID].usesAlpha)
		{
			global::Rendering.Rendering.effect1.Parameters["AlphaAdjust"].SetValue(modVbo[modID].defaultColor[3]);
			switch (mod1[modID].blendFunction)
			{
			case 0:
				mGraphics.BlendState = global::Rendering.Rendering.blendSourceAlpha;
				break;
			case 1:
				mGraphics.BlendState = global::Rendering.Rendering.blendSourceOne;
				break;
			default:
				mGraphics.BlendState = BlendState.Opaque;
				break;
			}
		}
		else
		{
			mGraphics.BlendState = BlendState.Opaque;
		}
		mGraphics.SetVertexBuffer(mVertexBufferObjectsLevel);
		mGraphics.Indices = mIndexBufferObjectsLevel;
		global::Rendering.Rendering.effect1.CurrentTechnique.Passes[0].Apply();
		int numTextures = modVbo[modID].numTextures;
		for (int i = 0; i < numTextures; i++)
		{
			mGraphics.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, modVbo[modID].vbStart, modVbo[modID].vcount, modVbo[modID].ibStart + modVbo[modID].textureListStart[i], modVbo[modID].textureListPrimitiveCnt[i]);
		}
		global::Rendering.Rendering.effect1.Parameters["AlphaAdjust"].SetValue(1f);
		if (modVbo[modID].bufferType == 1)
		{
			global::Rendering.Rendering.effect1.CurrentTechnique = global::Rendering.Rendering.effect1.Techniques["Main"];
		}
		global::Rendering.Rendering.effect1.CurrentTechnique.Passes[0].Apply();
	}

	public void Render_Level_Model_Basic(long modID)
	{
		int numTextures = modVbo[modID].numTextures;
		for (int i = 0; i < numTextures; i++)
		{
			mGraphics.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, modVbo[modID].vbStart, modVbo[modID].vcount, modVbo[modID].ibStart + modVbo[modID].textureListStart[i], modVbo[modID].textureListPrimitiveCnt[i]);
		}
	}

	public void Render_Player_Rigged_Model(long modID, int textureNormalID, int textureSpecularID, byte skipTexture)
	{
		if (modID >= numModels || modID < 0)
		{
			return;
		}
		if (mod1[modID].defaultColor[3] < 1f)
		{
			global::Rendering.Rendering.effect1.Parameters["AlphaAdjust"].SetValue(mod1[modID].defaultColor[3]);
			switch (mod1[modID].blendFunction)
			{
			case 0:
				mGraphics.BlendState = global::Rendering.Rendering.blendSourceAlpha;
				break;
			case 1:
				mGraphics.BlendState = global::Rendering.Rendering.blendSourceOne;
				break;
			default:
				mGraphics.BlendState = BlendState.Opaque;
				break;
			}
		}
		else
		{
			mGraphics.BlendState = BlendState.Opaque;
		}
		mGraphics.SetVertexBuffer(mVertexBufferObjects);
		mGraphics.Indices = mIndexBufferObjects;
		global::Rendering.Rendering.effect1.CurrentTechnique = global::Rendering.Rendering.effect1.Techniques["Matrices"];
		global::Rendering.Rendering.effect1.Parameters["MaterialTexture"].SetValue(global::Textures.Textures.texMain.texData[textureNormalID]);
		global::Rendering.Rendering.effect1.Parameters["SpecularTexture"].SetValue(global::Textures.Textures.texMain.texData[textureSpecularID]);
		int numTextures = mod1[modID].numTextures;
		for (int i = 0; i < numTextures; i++)
		{
			if (i != skipTexture)
			{
				global::Rendering.Rendering.effect1.Parameters["BaseTexture"].SetValue(global::Textures.Textures.texMain.texData[mod1[modID].textureList[i]]);
				global::Rendering.Rendering.effect1.CurrentTechnique.Passes[0].Apply();
				mGraphics.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, mod1[modID].vbStart, mod1[modID].vcount, mod1[modID].ibStart + mod1[modID].textureListStart[i], mod1[modID].textureListPrimitiveCnt[i]);
			}
		}
		global::Rendering.Rendering.effect1.Parameters["AlphaAdjust"].SetValue(1f);
		global::Rendering.Rendering.effect1.CurrentTechnique = global::Rendering.Rendering.effect1.Techniques["Main"];
		global::Rendering.Rendering.effect1.CurrentTechnique.Passes[0].Apply();
	}

	public void Render_Rigged_Model_Texture(long modID, int textureNormalID, int textureSpecularID, byte textureListID, byte texture)
	{
		global::Rendering.Rendering.effect1.Parameters["SpecularTexture"].SetValue(global::Textures.Textures.texMain.texData[textureSpecularID]);
		global::Rendering.Rendering.effect1.Parameters["MaterialTexture"].SetValue(global::Textures.Textures.texMain.texData[textureNormalID]);
		global::Rendering.Rendering.effect1.Parameters["BaseTexture"].SetValue(global::Textures.Textures.texMain.texData[texture]);
		global::Rendering.Rendering.effect1.CurrentTechnique.Passes[0].Apply();
		mGraphics.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, mod1[modID].vbStart, mod1[modID].vcount, mod1[modID].ibStart + mod1[modID].textureListStart[textureListID], mod1[modID].textureListPrimitiveCnt[textureListID]);
	}

	public void Render_Rigged_Model_Piece_Preparations(long modID, int textureNormalID, int textureSpecularID, byte renderCount, byte pieceID, bool starting)
	{
		if (starting)
		{
			int num = mod1[modID].textureListIndexCnt[pieceID];
			int num2 = num * renderCount;
			if (num2 != mod1[modID].indexBufferSize)
			{
				int num3 = 0;
				int[] array = new int[num2];
				mod1[modID].indexBufferSize = num2;
				mod1[modID].mInstanceIndex = new IndexBuffer(mGraphics, typeof(int), num2, BufferUsage.None);
				int num4 = mod1[modID].ibStart + mod1[modID].textureListStart[pieceID];
				int num5 = mod1[modID].vcount * 3;
				mod1[modID].vertexBufferStart = (int)global::Util.Util.maxIntValue;
				int i = 0;
				int num6 = 0;
				for (; i < renderCount; i++)
				{
					int num7 = i * num5;
					for (int j = 0; j < num; j++)
					{
						num3 = mViObjects[num4 + j];
						if (num3 < mod1[modID].vertexBufferStart)
						{
							mod1[modID].vertexBufferStart = num3;
						}
						array[num6++] = num3 + num7;
					}
				}
				mod1[modID].mInstanceIndex.SetData(array);
			}
			if (mod1[modID].defaultColor[3] < 1f)
			{
				global::Rendering.Rendering.effect1.Parameters["AlphaAdjust"].SetValue(mod1[modID].defaultColor[3]);
				switch (mod1[modID].blendFunction)
				{
				case 0:
					mGraphics.BlendState = global::Rendering.Rendering.blendSourceAlpha;
					break;
				case 1:
					mGraphics.BlendState = global::Rendering.Rendering.blendSourceOne;
					break;
				default:
					mGraphics.BlendState = BlendState.Opaque;
					break;
				}
			}
			else
			{
				mGraphics.BlendState = BlendState.Opaque;
			}
			mGraphics.SetVertexBuffer(mVertexBufferObjects);
			mGraphics.Indices = mod1[modID].mInstanceIndex;
			global::Rendering.Rendering.effect1.CurrentTechnique = global::Rendering.Rendering.effect1.Techniques["MatrixInstancing"];
			global::Rendering.Rendering.effect1.CurrentTechnique.Passes[0].Apply();
			global::Rendering.Rendering.effect1.Parameters["BaseTexture"].SetValue(global::Textures.Textures.texMain.texData[mod1[modID].textureList[pieceID]]);
			global::Rendering.Rendering.effect1.Parameters["MaterialTexture"].SetValue(global::Textures.Textures.texMain.texData[textureNormalID]);
			global::Rendering.Rendering.effect1.Parameters["SpecularTexture"].SetValue(global::Textures.Textures.texMain.texData[textureSpecularID]);
			mGraphics.RasterizerState = RasterizerState.CullClockwise;
		}
		else
		{
			mGraphics.Indices = mIndexBufferObjects;
			global::Rendering.Rendering.effect1.Parameters["AlphaAdjust"].SetValue(1f);
			global::Rendering.Rendering.effect1.CurrentTechnique = global::Rendering.Rendering.effect1.Techniques["Main"];
		}
	}

	public void Render_Rigged_Model_Piece(long modID, byte renderCount, byte pieceID)
	{
		global::Rendering.Rendering.effect1.Parameters["VertexCount"].SetValue(mod1[modID].vcount * 3);
		global::Rendering.Rendering.effect1.Parameters["VertexStart"].SetValue(mod1[modID].vertexBufferStart);
		global::Rendering.Rendering.effect1.CurrentTechnique = global::Rendering.Rendering.effect1.Techniques["MatrixInstancing"];
		global::Rendering.Rendering.effect1.CurrentTechnique.Passes[0].Apply();
		mGraphics.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, mod1[modID].vbStart, mod1[modID].vcount, 0, mod1[modID].textureListPrimitiveCnt[pieceID] * renderCount);
	}

	public void Render_Model_With_Preset_Texture(long modID, ref Matrix mv)
	{
		if (modID < numModels && modID >= 0)
		{
			global::Rendering.Rendering.effect1.Parameters["World"].SetValue(mv);
			global::Rendering.Rendering.effect1.CurrentTechnique = global::Rendering.Rendering.effect1.Techniques["Main"];
			global::Rendering.Rendering.effect1.CurrentTechnique.Passes[0].Apply();
			mGraphics.BlendState = BlendState.Opaque;
			mGraphics.SetVertexBuffer(mVertexBufferObjects);
			mGraphics.Indices = mIndexBufferObjects;
			mGraphics.RasterizerState = RasterizerState.CullClockwise;
			mGraphics.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, mod1[modID].vbStart, mod1[modID].vcount, mod1[modID].vbStart, mod1[modID].triangleCount);
		}
	}

	public void Render_Model_Basic(long modID)
	{
		if (modID < numModels && modID >= 0)
		{
			int numTextures = mod1[modID].numTextures;
			for (int i = 0; i < numTextures; i++)
			{
				mGraphics.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, mod1[modID].vbStart, mod1[modID].vcount, mod1[modID].ibStart + mod1[modID].textureListStart[i], mod1[modID].textureListPrimitiveCnt[i]);
			}
		}
	}

	public void Render_Textured_Model_Basic_Setup()
	{
		global::Rendering.Rendering.effect1.CurrentTechnique = global::Rendering.Rendering.effect1.Techniques["Main"];
		global::Rendering.Rendering.effect1.Parameters["MaterialTexture"].SetValue(global::Textures.Textures.texMain.texData[global::Textures.Textures.texDefaultNormalMap]);
		global::Rendering.Rendering.rGraphics.BlendState = BlendState.Opaque;
		global::Rendering.Rendering.rGraphics.SetVertexBuffer(mVertexBufferObjects);
		global::Rendering.Rendering.rGraphics.Indices = mIndexBufferObjects;
		global::Rendering.Rendering.rGraphics.RasterizerState = RasterizerState.CullClockwise;
	}

	public void Render_Textured_Model_Basic(long modID)
	{
		if (modID < numModels && modID >= 0)
		{
			int numTextures = mod1[modID].numTextures;
			for (int i = 0; i < numTextures; i++)
			{
				global::Rendering.Rendering.effect1.Parameters["BaseTexture"].SetValue(global::Textures.Textures.texMain.texData[mod1[modID].textureList[i]]);
				global::Rendering.Rendering.effect1.CurrentTechnique.Passes[0].Apply();
				mGraphics.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, mod1[modID].vbStart, mod1[modID].vcount, mod1[modID].ibStart + mod1[modID].textureListStart[i], mod1[modID].textureListPrimitiveCnt[i]);
			}
		}
	}

	public void Render_Solid_Particle_Model(long modID)
	{
		if (modID < numModels && modID >= 0)
		{
			global::Rendering.Rendering.effect1.Parameters["MaterialTexture"].SetValue(global::Textures.Textures.texMain.texData[mod1[modID].texNormalID]);
			int numTextures = mod1[modID].numTextures;
			for (int i = 0; i < numTextures; i++)
			{
				global::Rendering.Rendering.effect1.Parameters["BaseTexture"].SetValue(global::Textures.Textures.texMain.texData[mod1[modID].textureList[i]]);
				global::Rendering.Rendering.effect1.CurrentTechnique.Passes[0].Apply();
				mGraphics.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, mod1[modID].vbStart, mod1[modID].vcount, mod1[modID].ibStart + mod1[modID].textureListStart[i], mod1[modID].textureListPrimitiveCnt[i]);
			}
		}
	}

	public void Render_Model_Basic_With_Matrix(long modID, ref Matrix mv)
	{
		if (modID < numModels && modID >= 0)
		{
			global::Rendering.Rendering.effect1.Parameters["World"].SetValue(mv);
			global::Rendering.Rendering.effect1.CurrentTechnique.Passes[0].Apply();
			mGraphics.SetVertexBuffer(mVertexBufferObjects);
			mGraphics.Indices = mIndexBufferObjects;
			mGraphics.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, mod1[modID].vbStart, mod1[modID].vcount, mod1[modID].vbStart, mod1[modID].triangleCount);
		}
	}

	public void Render_Instanced_Model(long modID, short count)
	{
		if (modID < numModels && modID >= 0)
		{
			global::Rendering.Rendering.effect1.Parameters["ViewProjection"].SetValue(global::Rendering.Rendering.matrixVP);
			global::Rendering.Rendering.effect1.Parameters["BaseTexture"].SetValue(global::Textures.Textures.texMain.texData[mod1[modID].texID]);
			global::Rendering.Rendering.effect1.Parameters["MaterialTexture"].SetValue(global::Textures.Textures.texMain.texData[mod1[modID].texNormalID]);
			global::Rendering.Rendering.effect1.Parameters["PtLight0"].SetValue(global::Rendering.Rendering.ptLight0);
			global::Rendering.Rendering.effect1.Parameters["PtLightDirection0"].SetValue(global::Rendering.Rendering.ptLightDir0);
			global::Rendering.Rendering.effect1.CurrentTechnique.Passes[0].Apply();
			mGraphics.BlendState = BlendState.Opaque;
			mGraphics.RasterizerState = RasterizerState.CullClockwise;
			mGraphics.SetVertexBuffer(mVertexBufferObjects);
			mGraphics.Indices = mod1[modID].mInstanceIndex;
			mGraphics.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, mod1[modID].vcount, 0, mod1[modID].triangleCount * count);
		}
	}

	public void Render_Instanced_Particle_Model(long modID, short count)
	{
		if (modID < numModels && modID >= 0 && count >= 1)
		{
			int numTextures = mod1[modID].numTextures;
			for (int i = 0; i < numTextures; i++)
			{
				mGraphics.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, mod1[modID].vcount, mod1[modID].textureListStart[i], mod1[modID].textureListPrimitiveCnt[i] * count);
			}
		}
	}

	public void Render_Instanced_Model_ForDepthOnly(long modID, short count)
	{
		if (modID < numModels && modID >= 0)
		{
			global::Rendering.Rendering.effect1.Parameters["ViewProjection"].SetValue(global::Rendering.Rendering.matrixVP);
			global::Rendering.Rendering.effect1.CurrentTechnique.Passes[0].Apply();
			mGraphics.RasterizerState = RasterizerState.CullClockwise;
			mGraphics.SetVertexBuffer(mVertexBufferObjects);
			mGraphics.Indices = mod1[modID].mInstanceIndex;
			mGraphics.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, mod1[modID].vcount, 0, mod1[modID].triangleCount * count);
		}
	}

	public void Render_Model_For_ShadowMap(long modID)
	{
		if (modID < numModels && modID >= 0)
		{
			mGraphics.SetVertexBuffer(mVertexBufferObjects);
			mGraphics.Indices = mIndexBufferObjects;
			mGraphics.RasterizerState = RasterizerState.CullClockwise;
			mGraphics.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, mod1[modID].ibStart, mod1[modID].vcount, mod1[modID].ibStart, mod1[modID].pcount);
		}
	}

	public void Render_Rigged_Model_For_ShadowMap(long modID)
	{
		if (modID < numModels && modID >= 0)
		{
			global::Rendering.Rendering.effect1.CurrentTechnique = global::Rendering.Rendering.effect1.Techniques["ShadowMap_Matrix"];
			global::Rendering.Rendering.effect1.CurrentTechnique.Passes[0].Apply();
			mGraphics.SetVertexBuffer(mVertexBufferObjects);
			mGraphics.Indices = mIndexBufferObjects;
			mGraphics.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, mod1[modID].vbStart, mod1[modID].vcount, mod1[modID].ibStart, mod1[modID].pcount);
		}
	}

	public void Init_Models()
	{
		bool flag = false;
		string[] fileList = new string[1];
		_ = global::Rendering.Rendering.uBufferID;
		tempColModel = default(StructsClass.CollisionModel);
		StructsClass.Initialize_Collision_Model(ref tempColModel);
		tempModel = default(StructsClass.model);
		StructsClass.Initialize_Model(ref tempModel);
		if (!global::MainGame.MainGame.Load_Buffer_Config_Data(1, "Models.txt"))
		{
			int num = mainC.utilMain.Read_File_List("The_CoOp_Zombie_Game\\Config_Files\\MainModelVBOList.txt", ref fileList);
			if (num < 1)
			{
				numModels = 0;
				return;
			}
			mod1 = new StructsClass.model[num];
			for (int i = 0; i < num; i++)
			{
				Load_Condensed_Model(fileList[i], i);
			}
		}
		modSquare = Find_Model("Square.txt");
		modDoorSwitchScreen = Find_Model("Door_Switch_Screen.txt");
		modMarker = Find_Model("marker.txt");
		modMarker2 = Find_Model("marker2.txt");
		modMarker3 = Find_Model("marker3.txt");
		modMarker4 = Find_Model("marker4.txt");
		modMarker5 = Find_Model("marker5.txt");
		modCHud = Find_Model("Commander-Hud.txt");
		modCHudBar = Find_Model("Commander-Hud-Bar.txt");
		modTransporter = Find_Model("Transporter.txt");
		modSkyDome = Find_Level_Model("SkyDome.txt");
		modFlatPlane = Find_Model("FlatPlane.txt");
		modCylinder = Find_Model("Cylinder.txt");
		modGeometryFlatCircle16 = Find_Model("Geometry_Flat_Circle16.txt");
		modGeometryFlatCircle8 = Find_Model("Geometry_Flat_Circle8.txt");
		modHalfSphere = Find_Model("HalfSphere.txt");
		modBillboard = Find_Model("Billboard.txt");
		modShield = Find_Model("Object_Shield.txt");
		Load_Model_Lists("ModelLists.txt");
	}

	public void Load_Model(string filename)
	{
		byte b = byte.MaxValue;
		byte b2 = 0;
		string text = "";
		int num = -1;
		int num2 = 0;
		int num3 = 0;
		int num4 = 0;
		int num5 = 0;
		int num6 = 0;
		int num7 = 0;
		int num8 = 0;
		int[] array = new int[1];
		int[] array2 = array;
		int[] array3 = new int[1];
		int[] array4 = array3;
		int[] array5 = new int[1];
		int[] array6 = array5;
		short num9 = 0;
		short texNormalID = global::Textures.Textures.texDefaultNormalMap;
		Stream stream = TitleContainer.OpenStream("The_CoOp_Zombie_Game\\Models_To_Convert\\" + filename);
		byte[] array7 = new byte[stream.Length];
		tempModel.numTextures = 0;
		if (!stream.CanRead)
		{
			return;
		}
		stream.Read(array7, 0, array7.Length);
		string text2 = mainC.utilMain.Byte_Array_To_String(array7);
		string[] array8 = text2.Split('\n', '\r');
		int i = 0;
		int num10 = 0;
		for (; i < array8.Length; i++)
		{
			if (array8[i].Length > 0)
			{
				num10++;
			}
		}
		if (num10 < 1)
		{
			stream.Close();
			return;
		}
		string[] array9 = new string[num10];
		i = 0;
		num10 = 0;
		for (; i < array8.Length; i++)
		{
			if (array8[i].Length > 0)
			{
				array9[num10++] = array8[i];
			}
		}
		string[] array10 = array9;
		int num11;
		foreach (string text3 in array10)
		{
			string text4 = text3.Substring(0, 1);
			string text5 = text3.Substring(1, 1);
			string text6 = text3.Substring(2, 1);
			if (text4.Equals("v", StringComparison.OrdinalIgnoreCase))
			{
				text4 = text3.Substring(1, 1);
				if (text4.Equals(" "))
				{
					num2++;
				}
				else if (text4.Equals("n", StringComparison.OrdinalIgnoreCase))
				{
					num3++;
				}
				else if (text4.Equals("t", StringComparison.OrdinalIgnoreCase))
				{
					num5++;
				}
			}
			else if (text4.Equals("t", StringComparison.OrdinalIgnoreCase))
			{
				text4 = text3.Substring(1, 1);
				text5 = text3.Substring(2, 1);
				if (!text4.Equals("1") || !text5.Equals(" "))
				{
					continue;
				}
				text2 = text3.Substring(3);
				string[] array11 = text2.Split(' ', '\t');
				int k = 0;
				num11 = 0;
				for (; k < array11.Length; k++)
				{
					if (array11[k].Length > 0)
					{
						num11++;
					}
				}
				if (num11 <= 0)
				{
					continue;
				}
				tempModel.numTextures = (byte)num11;
				tempModel.textureList = new int[num11];
				tempModel.textureListStart = new int[num11];
				tempModel.textureListEnd = new int[num11];
				tempModel.textureListIndexCnt = new int[num11];
				tempModel.textureListPrimitiveCnt = new int[num11];
				tempModel.textureListNames = new string[num11];
				string[] array12 = new string[num11];
				k = 0;
				num11 = 0;
				for (; k < array11.Length; k++)
				{
					if (array11[k].Length > 0)
					{
						tempModel.textureListNames[num11] = array11[k];
						tempModel.textureList[num11++] = mainC.texturesMain.Find_Texture(array11[k], 0);
					}
				}
			}
			else if (text4.Equals("f", StringComparison.OrdinalIgnoreCase) && text3.Substring(1, 1).Equals(" "))
			{
				num4++;
			}
			else if (text4.Equals("r", StringComparison.OrdinalIgnoreCase))
			{
				if (text5.Equals("2") && text6.Equals(" "))
				{
					text2 = text3.Substring(3);
					int k = byte.Parse(text2, NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
					array2 = new int[k];
					array4 = new int[k];
					array6 = new int[k];
					for (k--; k > -1; k--)
					{
						array2[k] = 0;
						array4[k] = 0;
						array6[k] = 0;
					}
				}
			}
			else if (text4.Equals("j", StringComparison.OrdinalIgnoreCase) && text5.Equals(" "))
			{
				string[] array11 = text3.Split(' ', '\t');
				b = byte.Parse(array11[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
				if (b > num)
				{
					num = b;
				}
			}
		}
		num11 = num4 * 3;
		tempModel.v1 = new StructsClass.vtex[num11];
		tempModel.blendIndex0 = new byte[num11];
		if (num3 > 0)
		{
			tempModel.n1 = new StructsClass.vnorm[num11];
		}
		if (num5 > 0)
		{
			tempModel.t1 = new StructsClass.texcoord[num11];
		}
		if (num4 > 0)
		{
			tempModel.p1 = new StructsClass.poly[num4];
		}
		for (int k = 0; k < num11; k++)
		{
			tempModel.v1[k] = new StructsClass.vtex();
			tempModel.blendIndex0[k] = 0;
		}
		for (int k = 0; k < num11; k++)
		{
			tempModel.n1[k] = new StructsClass.vnorm();
		}
		for (int k = 0; k < num11; k++)
		{
			tempModel.t1[k] = new StructsClass.texcoord();
		}
		for (int k = 0; k < num4; k++)
		{
			tempModel.p1[k] = default(StructsClass.poly);
			StructsClass.Initialize_Poly(ref tempModel.p1[k]);
		}
		stream.Seek(0L, SeekOrigin.Begin);
		tempModel.usesAlpha = false;
		tempModel.texMovX = 0f;
		tempModel.texMovY = 0f;
		tempModel.defaultColor[0] = 1f;
		tempModel.defaultColor[1] = 1f;
		tempModel.defaultColor[2] = 1f;
		tempModel.defaultColor[3] = 1f;
		tempModel.bufferType = 0;
		tempModel.instanceCount = 0;
		tempModel.blendFunction = 0;
		tempModel.texture = "";
		tempModel.textureNormal = "default_normal";
		tempModel.numObjects = 0;
		tempModel.numObjectRotations = 0;
		tempModel.usesRigging = 0;
		tempModel.texMultX = 1f;
		tempModel.texMultY = 1f;
		tempModel.texXadj = 0f;
		tempModel.texYadj = 0f;
		tempModel.vcount = num2;
		tempModel.ncount = num3;
		tempModel.tcount = num5;
		tempModel.pcount = num4;
		if (tempModel.numTextures < 1)
		{
			tempModel.numTextures = 1;
			int[] textureList = new int[1];
			tempModel.textureList = textureList;
			int[] textureListStart = new int[1];
			tempModel.textureListStart = textureListStart;
			int[] textureListEnd = new int[1];
			tempModel.textureListEnd = textureListEnd;
			int[] textureListIndexCnt = new int[1];
			tempModel.textureListIndexCnt = textureListIndexCnt;
			int[] textureListPrimitiveCnt = new int[1];
			tempModel.textureListPrimitiveCnt = textureListPrimitiveCnt;
			tempModel.textureListNames = new string[1] { "" };
		}
		num2 = 0;
		num4 = 0;
		num3 = 0;
		num5 = 0;
		float num12 = 1f;
		Matrix[] array13;
		if (num > -1)
		{
			array13 = new Matrix[++num];
		}
		else
		{
			array13 = new Matrix[1];
			num = 1;
		}
		for (int k = 0; k < num; k++)
		{
			ref Matrix reference = ref array13[k];
			reference = Matrix.Identity;
		}
		string[] array14 = array9;
		foreach (string text7 in array14)
		{
			if (text7.Length <= 2)
			{
				continue;
			}
			string text4 = text7.Substring(0, 1);
			string text5 = text7.Substring(1, 1);
			string text6 = text7.Substring(2, 1);
			if (text7.Length > 3)
			{
				text = text7.Substring(3, 1);
			}
			if (text4.Equals("r", StringComparison.OrdinalIgnoreCase))
			{
				if (text5.Equals("1"))
				{
					tempModel.usesAlpha = true;
				}
				else if (text5.Equals("3") && text6.Equals(" "))
				{
					text2 = text7.Substring(3);
					int k = byte.Parse(text2, NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
					array2[k] = num2;
					array4[k] = num3;
					array6[k] = num5;
				}
			}
			else if (text4.Equals("z", StringComparison.OrdinalIgnoreCase))
			{
				if (text5.Equals("1") && text6.Equals("1"))
				{
					if (!text.Equals(" "))
					{
						continue;
					}
					text2 = text7.Substring(4);
					array8 = text2.Split(' ', '\t');
					int k = 0;
					num11 = 0;
					for (; k < array8.Length; k++)
					{
						if (array8[k].Length > 0)
						{
							num11++;
						}
					}
					if (num11 <= 0)
					{
						continue;
					}
					string[] array12 = new string[num11];
					k = 0;
					num11 = 0;
					for (; k < array8.Length; k++)
					{
						if (array8[k].Length > 0)
						{
							array12[num11++] = array8[k];
						}
					}
					if ((num11 = int.Parse(array12[0], CultureInfo.InvariantCulture.NumberFormat)) <= 0)
					{
						continue;
					}
					k = array12.Length - 1;
					if (k == k / 9 * 9)
					{
						mod1[numModels].numObjects = (byte)num11;
						mod1[numModels].numObjectRotations = (byte)num11;
						mod1[numModels].x = new float[num11];
						mod1[numModels].y = new float[num11];
						mod1[numModels].z = new float[num11];
						mod1[numModels].dimX = new short[num11];
						mod1[numModels].dimY = new short[num11];
						mod1[numModels].dimZ = new short[num11];
						mod1[numModels].rotX = new float[num11];
						mod1[numModels].rotY = new float[num11];
						mod1[numModels].rotZ = new float[num11];
						k = 0;
						int num13 = 1;
						for (; k < num11; k++)
						{
							mod1[numModels].x[k] = float.Parse(array12[num13++], CultureInfo.InvariantCulture.NumberFormat);
							mod1[numModels].y[k] = float.Parse(array12[num13++], CultureInfo.InvariantCulture.NumberFormat);
							mod1[numModels].z[k] = float.Parse(array12[num13++], CultureInfo.InvariantCulture.NumberFormat);
							mod1[numModels].rotX[k] = float.Parse(array12[num13++], CultureInfo.InvariantCulture.NumberFormat);
							mod1[numModels].rotY[k] = float.Parse(array12[num13++], CultureInfo.InvariantCulture.NumberFormat);
							mod1[numModels].rotZ[k] = float.Parse(array12[num13++], CultureInfo.InvariantCulture.NumberFormat);
							mod1[numModels].dimX[k] = short.Parse(array12[num13++], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
							mod1[numModels].dimY[k] = short.Parse(array12[num13++], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
							mod1[numModels].dimZ[k] = short.Parse(array12[num13++], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
						}
					}
				}
				else if (text5.Equals("1") && text6.Equals("2"))
				{
					if (!text.Equals(" "))
					{
						continue;
					}
					text2 = text7.Substring(4);
					array8 = text2.Split(' ', '\t');
					int k = 0;
					num11 = 0;
					for (; k < array8.Length; k++)
					{
						if (array8[k].Length > 0)
						{
							num11++;
						}
					}
					if (num11 <= 0)
					{
						continue;
					}
					string[] array12 = new string[num11];
					k = 0;
					num11 = 0;
					for (; k < array8.Length; k++)
					{
						if (array8[k].Length > 0)
						{
							array12[num11++] = array8[k];
						}
					}
					if (num11 > 2 && (k = int.Parse(array12[0], CultureInfo.InvariantCulture.NumberFormat)) < num)
					{
						switch (int.Parse(array12[1], CultureInfo.InvariantCulture.NumberFormat))
						{
						case 0:
						{
							ref Matrix reference4 = ref array13[k];
							reference4 = Matrix.CreateRotationX(float.Parse(array12[2], CultureInfo.InvariantCulture.NumberFormat) * ((float)Math.PI / 180f)) * array13[k];
							break;
						}
						case 1:
						{
							ref Matrix reference3 = ref array13[k];
							reference3 = Matrix.CreateRotationY(float.Parse(array12[2], CultureInfo.InvariantCulture.NumberFormat) * ((float)Math.PI / 180f)) * array13[k];
							break;
						}
						case 2:
						{
							ref Matrix reference2 = ref array13[k];
							reference2 = Matrix.CreateRotationZ(float.Parse(array12[2], CultureInfo.InvariantCulture.NumberFormat) * ((float)Math.PI / 180f)) * array13[k];
							break;
						}
						}
					}
				}
				else if (text5.Equals("1"))
				{
					if (text6.Equals(" "))
					{
						tempModel.texture = text7.Substring(3);
						num9 = mainC.texturesMain.Find_Texture(tempModel.texture, 0);
						tempModel.textureList[0] = num9;
						tempModel.textureListNames[0] = tempModel.texture;
					}
				}
				else if (text5.Equals("2"))
				{
					if (!text6.Equals(" "))
					{
						continue;
					}
					string[] array11 = text7.Split(' ', '\t');
					int k = 1;
					num11 = 0;
					for (; k < array11.Length; k++)
					{
						if (num11 >= 4)
						{
							break;
						}
						if (array11[k].Length > 0)
						{
							tempModel.defaultColor[num11] = float.Parse(array11[k], CultureInfo.InvariantCulture.NumberFormat);
							num11++;
						}
					}
				}
				else if (text5.Equals("3"))
				{
					if (text6.Equals(" "))
					{
						tempModel.textureNormal = text7.Substring(3);
						texNormalID = mainC.texturesMain.Find_Texture(tempModel.textureNormal, global::Textures.Textures.texDefaultNormalMap);
					}
				}
				else if (text5.Equals("4"))
				{
					if (text6.Equals(" "))
					{
						string[] array11 = text7.Split(' ', '\t');
						if (array11[1].Length > 0)
						{
							num12 = float.Parse(array11[1], CultureInfo.InvariantCulture.NumberFormat);
						}
					}
				}
				else if (text5.Equals("5"))
				{
					if (text6.Equals(" "))
					{
						tempModel.bufferType = 1;
					}
				}
				else if (text5.Equals("6"))
				{
					if (text6.Equals(" "))
					{
						tempModel.instanceCount = short.Parse(text7.Substring(3), NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
					}
				}
				else if (text5.Equals("7"))
				{
					if (!text6.Equals(" "))
					{
						continue;
					}
					text2 = text7.Substring(3);
					array8 = text2.Split(' ', '\t');
					int k = 0;
					num11 = 0;
					for (; k < array8.Length; k++)
					{
						if (array8[k].Length > 0)
						{
							num11++;
						}
					}
					if (num11 <= 0)
					{
						continue;
					}
					string[] array12 = new string[num11];
					k = 0;
					num11 = 0;
					for (; k < array8.Length; k++)
					{
						if (array8[k].Length > 0)
						{
							array12[num11++] = array8[k];
						}
					}
					if ((num11 = int.Parse(array12[0], CultureInfo.InvariantCulture.NumberFormat)) <= 0)
					{
						continue;
					}
					k = array12.Length - 1;
					if (k == k / 6 * 6)
					{
						tempModel.numObjects = (byte)num11;
						tempModel.x = new float[num11];
						tempModel.y = new float[num11];
						tempModel.z = new float[num11];
						tempModel.dimX = new short[num11];
						tempModel.dimY = new short[num11];
						tempModel.dimZ = new short[num11];
						k = 0;
						int num13 = 1;
						for (; k < num11; k++)
						{
							tempModel.x[k] = float.Parse(array12[num13++], CultureInfo.InvariantCulture.NumberFormat);
							tempModel.y[k] = float.Parse(array12[num13++], CultureInfo.InvariantCulture.NumberFormat);
							tempModel.z[k] = float.Parse(array12[num13++], CultureInfo.InvariantCulture.NumberFormat);
							tempModel.dimX[k] = short.Parse(array12[num13++], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
							tempModel.dimY[k] = short.Parse(array12[num13++], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
							tempModel.dimZ[k] = short.Parse(array12[num13++], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
						}
					}
				}
				else if (text5.Equals("8"))
				{
					if (text6.Equals(" "))
					{
						tempModel.blendFunction = byte.Parse(text7.Substring(3), NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
					}
				}
				else if (text5.Equals("9") && text6.Equals(" "))
				{
					num11 = int.Parse(text7.Substring(3), CultureInfo.InvariantCulture.NumberFormat);
					int num13 = tempModel.pcount * 3;
					for (int k = 0; k < num13; k++)
					{
						tempModel.blendIndex0[k] = (byte)num11;
					}
					tempModel.usesRigging = 1;
					b = (byte)num11;
				}
			}
			else if (text4.Equals("v", StringComparison.OrdinalIgnoreCase))
			{
				if (text5.Equals(" "))
				{
					string[] array11 = text7.Split(' ');
					if (array11.Length > 3)
					{
						num11 = 0;
						if (tempModel.usesRigging == 1)
						{
							float num14 = float.Parse(array11[1], CultureInfo.InvariantCulture.NumberFormat) * num12;
							float num15 = float.Parse(array11[2], CultureInfo.InvariantCulture.NumberFormat) * num12;
							float num16 = float.Parse(array11[3], CultureInfo.InvariantCulture.NumberFormat) * num12;
							tempModel.v1[num2].v[0] = array13[b].M11 * num14 + array13[b].M21 * num15 + array13[b].M31 * num16;
							tempModel.v1[num2].v[1] = array13[b].M12 * num14 + array13[b].M22 * num15 + array13[b].M32 * num16;
							tempModel.v1[num2].v[2] = array13[b].M13 * num14 + array13[b].M23 * num15 + array13[b].M33 * num16;
							tempModel.blendIndex0[num2] = b;
						}
						else
						{
							tempModel.v1[num2].v[0] = float.Parse(array11[1], CultureInfo.InvariantCulture.NumberFormat) * num12;
							tempModel.v1[num2].v[1] = float.Parse(array11[2], CultureInfo.InvariantCulture.NumberFormat) * num12;
							tempModel.v1[num2].v[2] = float.Parse(array11[3], CultureInfo.InvariantCulture.NumberFormat) * num12;
						}
					}
					num2++;
				}
				else if (text5.Equals("n", StringComparison.OrdinalIgnoreCase))
				{
					if (!text6.Equals(" "))
					{
						continue;
					}
					string[] array11 = text7.Split(' ');
					int k = 1;
					num11 = 0;
					for (; k < array11.Length; k++)
					{
						if (num11 >= 3)
						{
							break;
						}
						tempModel.n1[num3].n[num11] = float.Parse(array11[k], CultureInfo.InvariantCulture.NumberFormat);
						num11++;
					}
					num3++;
				}
				else
				{
					if (!text5.Equals("t", StringComparison.OrdinalIgnoreCase) || !text6.Equals(" "))
					{
						continue;
					}
					string[] array11 = text7.Split(' ');
					int k = 1;
					num11 = 0;
					for (; k < array11.Length; k++)
					{
						if (num11 >= 2)
						{
							break;
						}
						if (num11 == 0)
						{
							tempModel.t1[num5].t[num11] = float.Parse(array11[k], CultureInfo.InvariantCulture.NumberFormat);
						}
						else
						{
							tempModel.t1[num5].t[num11] = 1f - float.Parse(array11[k], CultureInfo.InvariantCulture.NumberFormat);
						}
						num11++;
					}
					num5++;
				}
			}
			else if (text4.Equals("j", StringComparison.OrdinalIgnoreCase) && text5.Equals(" "))
			{
				string[] array11 = text7.Split(' ', '\t');
				b = byte.Parse(array11[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
			}
		}
		array10 = array9;
		foreach (string text8 in array10)
		{
			if (text8.Length <= 2)
			{
				continue;
			}
			string text4 = text8.Substring(0, 1);
			string text5 = text8.Substring(1, 1);
			string text6 = text8.Substring(2, 1);
			if (text4.Equals("f", StringComparison.OrdinalIgnoreCase))
			{
				if (!text5.Equals(" "))
				{
					continue;
				}
				int num17 = 0;
				string[] array11 = text8.Split(' ');
				for (int k = 1; k < array11.Length; k++)
				{
					if (array11[k].Length <= 0)
					{
						continue;
					}
					tempModel.p1[num4].v[num17] = 0;
					tempModel.p1[num4].n[num17] = 0;
					tempModel.p1[num4].t[num17] = 0;
					string[] array12 = array11[k].Split('/');
					int num18 = int.Parse(array12[0], CultureInfo.InvariantCulture.NumberFormat) - 1 + num6;
					int num19 = int.Parse(array12[1], CultureInfo.InvariantCulture.NumberFormat) - 1 + num8;
					int num20 = int.Parse(array12[2], CultureInfo.InvariantCulture.NumberFormat) - 1 + num7;
					int num13 = 0;
					for (num11 = 0; num11 < num4; num11++)
					{
						if (tempModel.p1[num11].v[0] == num18)
						{
							if (tempModel.p1[num11].n[0] != num20 || tempModel.p1[num11].t[0] != num19)
							{
								num13 = 1;
								break;
							}
						}
						else if (tempModel.p1[num11].v[1] == num18)
						{
							if (tempModel.p1[num11].n[1] != num20 || tempModel.p1[num11].t[1] != num19)
							{
								num13 = 1;
								break;
							}
						}
						else if (tempModel.p1[num11].v[2] == num18 && (tempModel.p1[num11].n[2] != num20 || tempModel.p1[num11].t[2] != num19))
						{
							num13 = 1;
							break;
						}
					}
					if (num13 == 1)
					{
						num13 = 0;
						for (num11 = 0; num11 < num4; num11++)
						{
							if (tempModel.p1[num11].n[0] == num20 && tempModel.p1[num11].t[0] == num19 && tempModel.v1[tempModel.p1[num11].v[0]].v[0] == tempModel.v1[num18].v[0] && tempModel.v1[tempModel.p1[num11].v[0]].v[1] == tempModel.v1[num18].v[1] && tempModel.v1[tempModel.p1[num11].v[0]].v[2] == tempModel.v1[num18].v[2])
							{
								num18 = tempModel.p1[num11].v[0];
								num13 = 1;
								break;
							}
							if (tempModel.p1[num11].n[1] == num20 && tempModel.p1[num11].t[1] == num19 && tempModel.v1[tempModel.p1[num11].v[1]].v[0] == tempModel.v1[num18].v[0] && tempModel.v1[tempModel.p1[num11].v[1]].v[1] == tempModel.v1[num18].v[1] && tempModel.v1[tempModel.p1[num11].v[1]].v[2] == tempModel.v1[num18].v[2])
							{
								num18 = tempModel.p1[num11].v[1];
								num13 = 1;
								break;
							}
							if (tempModel.p1[num11].n[2] == num20 && tempModel.p1[num11].t[2] == num19 && tempModel.v1[tempModel.p1[num11].v[2]].v[0] == tempModel.v1[num18].v[0] && tempModel.v1[tempModel.p1[num11].v[2]].v[1] == tempModel.v1[num18].v[1] && tempModel.v1[tempModel.p1[num11].v[2]].v[2] == tempModel.v1[num18].v[2])
							{
								num18 = tempModel.p1[num11].v[2];
								num13 = 1;
								break;
							}
						}
						if (num13 == 0)
						{
							num13 = tempModel.vcount;
							tempModel.v1[num13].v[0] = tempModel.v1[num18].v[0];
							tempModel.v1[num13].v[1] = tempModel.v1[num18].v[1];
							tempModel.v1[num13].v[2] = tempModel.v1[num18].v[2];
							if (tempModel.usesRigging == 1)
							{
								tempModel.blendIndex0[num13] = tempModel.blendIndex0[num18];
							}
							num18 = num13;
							tempModel.vcount++;
						}
					}
					tempModel.p1[num4].v[num17] = num18;
					tempModel.p1[num4].t[num17] = num19;
					tempModel.p1[num4].n[num17] = num20;
					num17++;
				}
				Calculate_Tangents(numModels, num4);
				tempModel.textureListEnd[b2] = num4;
				num4++;
			}
			else if (text4.Equals("t", StringComparison.OrdinalIgnoreCase))
			{
				if (text5.Equals("2") && text6.Equals(" "))
				{
					b2 = byte.Parse(text8.Substring(3), NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
					tempModel.textureListStart[b2] = num4;
					tempModel.textureListEnd[b2] = num4;
				}
			}
			else if (text4.Equals("r", StringComparison.OrdinalIgnoreCase) && text5.Equals("4") && text6.Equals(" "))
			{
				int k = byte.Parse(text8.Substring(3), NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
				num6 = array2[k];
				num7 = array4[k];
				num8 = array6[k];
			}
		}
		tempModel.name = filename;
		tempModel.texID = num9;
		tempModel.texNormalID = texNormalID;
		tempModel.vertexCount = tempModel.vcount;
		tempModel.triangleCount = tempModel.pcount;
		num11 = tempModel.numTextures;
		for (int k = 0; k < num11; k++)
		{
			tempModel.textureListPrimitiveCnt[k] = tempModel.textureListEnd[k] - tempModel.textureListStart[k] + 1;
			tempModel.textureListIndexCnt[k] = tempModel.textureListPrimitiveCnt[k] * 3;
			tempModel.textureListStart[k] *= 3;
		}
		stream.Close();
		Save_Model_In_Condensed_Format(filename, numModels);
	}

	public unsafe void Save_Model_In_Condensed_Format(string filename, int modID)
	{
		FileStream fileStream = File.Open("The_CoOp_Zombie_Game\\Models_Converted\\" + filename, FileMode.Create, FileAccess.Write);
		if (fileStream.CanWrite)
		{
			BinaryWriter binaryWriter = new BinaryWriter(fileStream);
			binaryWriter.Write(tempModel.name);
			binaryWriter.Write(tempModel.texture);
			binaryWriter.Write(tempModel.textureNormal);
			binaryWriter.Write(tempModel.numTextures);
			int i = 0;
			int numTextures;
			for (numTextures = tempModel.numTextures; i < numTextures; i++)
			{
				binaryWriter.Write(tempModel.textureListNames[i]);
				binaryWriter.Write(tempModel.textureListStart[i]);
				binaryWriter.Write(tempModel.textureListPrimitiveCnt[i]);
				binaryWriter.Write(tempModel.textureListIndexCnt[i]);
			}
			binaryWriter.Write(tempModel.vcount);
			binaryWriter.Write(tempModel.ncount);
			binaryWriter.Write(tempModel.tcount);
			binaryWriter.Write(tempModel.pcount);
			binaryWriter.Write(tempModel.usesAlpha);
			binaryWriter.Write(tempModel.defaultColor[0]);
			binaryWriter.Write(tempModel.defaultColor[1]);
			binaryWriter.Write(tempModel.defaultColor[2]);
			binaryWriter.Write(tempModel.defaultColor[3]);
			binaryWriter.Write(tempModel.bufferType);
			binaryWriter.Write(tempModel.instanceCount);
			binaryWriter.Write(tempModel.blendFunction);
			binaryWriter.Write(tempModel.numObjects);
			binaryWriter.Write(tempModel.numObjectRotations);
			binaryWriter.Write(tempModel.usesRigging);
			numTextures = tempModel.numObjects;
			if (tempModel.numObjectRotations > 0)
			{
				for (i = 0; i < numTextures; i++)
				{
					binaryWriter.Write(tempModel.x[i]);
					binaryWriter.Write(tempModel.y[i]);
					binaryWriter.Write(tempModel.z[i]);
					binaryWriter.Write(tempModel.rotX[i]);
					binaryWriter.Write(tempModel.rotY[i]);
					binaryWriter.Write(tempModel.rotZ[i]);
					binaryWriter.Write(tempModel.dimX[i]);
					binaryWriter.Write(tempModel.dimY[i]);
					binaryWriter.Write(tempModel.dimZ[i]);
				}
			}
			else
			{
				for (i = 0; i < numTextures; i++)
				{
					binaryWriter.Write(tempModel.x[i]);
					binaryWriter.Write(tempModel.y[i]);
					binaryWriter.Write(tempModel.z[i]);
					binaryWriter.Write(tempModel.dimX[i]);
					binaryWriter.Write(tempModel.dimY[i]);
					binaryWriter.Write(tempModel.dimZ[i]);
				}
			}
			numTextures = tempModel.vcount;
			if (tempModel.usesRigging == 0)
			{
				for (i = 0; i < numTextures; i++)
				{
					fixed (float* ptr = &tempModel.v1[i].v[0])
					{
						fixed (float* ptr2 = &tempModel.v1[i].v[1])
						{
							fixed (float* ptr3 = &tempModel.v1[i].v[2])
							{
								byte* bptr = (byte*)ptr;
								mainC.utilMain.Write_Float_Reversed(binaryWriter, bptr);
								bptr = (byte*)ptr2;
								mainC.utilMain.Write_Float_Reversed(binaryWriter, bptr);
								bptr = (byte*)ptr3;
								mainC.utilMain.Write_Float_Reversed(binaryWriter, bptr);
							}
						}
					}
				}
			}
			else
			{
				for (i = 0; i < numTextures; i++)
				{
					fixed (float* ptr4 = &tempModel.v1[i].v[0])
					{
						fixed (float* ptr5 = &tempModel.v1[i].v[1])
						{
							fixed (float* ptr6 = &tempModel.v1[i].v[2])
							{
								byte* bptr = (byte*)ptr4;
								mainC.utilMain.Write_Float_Reversed(binaryWriter, bptr);
								bptr = (byte*)ptr5;
								mainC.utilMain.Write_Float_Reversed(binaryWriter, bptr);
								bptr = (byte*)ptr6;
								mainC.utilMain.Write_Float_Reversed(binaryWriter, bptr);
							}
						}
					}
				}
				for (i = 0; i < numTextures; i++)
				{
					binaryWriter.Write(tempModel.blendIndex0[i]);
				}
			}
			numTextures = tempModel.ncount;
			for (i = 0; i < numTextures; i++)
			{
				fixed (float* ptr7 = &tempModel.n1[i].n[0])
				{
					fixed (float* ptr8 = &tempModel.n1[i].n[1])
					{
						fixed (float* ptr9 = &tempModel.n1[i].n[2])
						{
							byte* bptr = (byte*)ptr7;
							mainC.utilMain.Write_Float_Reversed(binaryWriter, bptr);
							bptr = (byte*)ptr8;
							mainC.utilMain.Write_Float_Reversed(binaryWriter, bptr);
							bptr = (byte*)ptr9;
							mainC.utilMain.Write_Float_Reversed(binaryWriter, bptr);
						}
					}
				}
			}
			numTextures = tempModel.tcount;
			for (i = 0; i < numTextures; i++)
			{
				fixed (float* ptr10 = &tempModel.t1[i].t[0])
				{
					fixed (float* ptr11 = &tempModel.t1[i].t[1])
					{
						byte* bptr = (byte*)ptr10;
						mainC.utilMain.Write_Float_Reversed(binaryWriter, bptr);
						bptr = (byte*)ptr11;
						mainC.utilMain.Write_Float_Reversed(binaryWriter, bptr);
					}
				}
			}
			numTextures = tempModel.pcount;
			for (i = 0; i < numTextures; i++)
			{
				fixed (int* ptr12 = &tempModel.p1[i].v[0])
				{
					fixed (int* ptr13 = &tempModel.p1[i].v[1])
					{
						fixed (int* ptr14 = &tempModel.p1[i].v[2])
						{
							byte* bptr = (byte*)ptr12;
							mainC.utilMain.Write_Int_Reversed(binaryWriter, bptr);
							bptr = (byte*)ptr13;
							mainC.utilMain.Write_Int_Reversed(binaryWriter, bptr);
							bptr = (byte*)ptr14;
							mainC.utilMain.Write_Int_Reversed(binaryWriter, bptr);
						}
					}
				}
			}
			for (i = 0; i < numTextures; i++)
			{
				fixed (int* ptr15 = &tempModel.p1[i].n[0])
				{
					fixed (int* ptr16 = &tempModel.p1[i].n[1])
					{
						fixed (int* ptr17 = &tempModel.p1[i].n[2])
						{
							byte* bptr = (byte*)ptr15;
							mainC.utilMain.Write_Int_Reversed(binaryWriter, bptr);
							bptr = (byte*)ptr16;
							mainC.utilMain.Write_Int_Reversed(binaryWriter, bptr);
							bptr = (byte*)ptr17;
							mainC.utilMain.Write_Int_Reversed(binaryWriter, bptr);
						}
					}
				}
			}
			for (i = 0; i < numTextures; i++)
			{
				fixed (int* ptr18 = &tempModel.p1[i].t[0])
				{
					fixed (int* ptr19 = &tempModel.p1[i].t[1])
					{
						fixed (int* ptr20 = &tempModel.p1[i].t[2])
						{
							byte* bptr = (byte*)ptr18;
							mainC.utilMain.Write_Int_Reversed(binaryWriter, bptr);
							bptr = (byte*)ptr19;
							mainC.utilMain.Write_Int_Reversed(binaryWriter, bptr);
							bptr = (byte*)ptr20;
							mainC.utilMain.Write_Int_Reversed(binaryWriter, bptr);
						}
					}
				}
			}
			for (i = 0; i < numTextures; i++)
			{
				fixed (float* ptr21 = &tempModel.p1[i].tangent.v[0])
				{
					fixed (float* ptr22 = &tempModel.p1[i].tangent.v[1])
					{
						fixed (float* ptr23 = &tempModel.p1[i].tangent.v[2])
						{
							byte* bptr = (byte*)ptr21;
							mainC.utilMain.Write_Float_Reversed(binaryWriter, bptr);
							bptr = (byte*)ptr22;
							mainC.utilMain.Write_Float_Reversed(binaryWriter, bptr);
							bptr = (byte*)ptr23;
							mainC.utilMain.Write_Float_Reversed(binaryWriter, bptr);
						}
					}
				}
			}
		}
		fileStream.Close();
	}

	private static void ConvertModelBuffersFromXboxEndian(ref StructsClass.model model)
	{
		if (!BitConverter.IsLittleEndian)
		{
			return;
		}

		ReverseFourByteElements(model.vertexBytes);
		ReverseFourByteElements(model.normalBytes);
		ReverseFourByteElements(model.textureBytes);
		ReverseFourByteElements(model.vIndexBytes);
		ReverseFourByteElements(model.nIndexBytes);
		ReverseFourByteElements(model.tIndexBytes);
		ReverseFourByteElements(model.tangentBytes);
		ReverseFourByteElements(model.bwBytes0);
		ReverseFourByteElements(model.bwBytes1);
		ReverseFourByteElements(model.bwBytes2);
		ReverseFourByteElements(model.bwBytes3);
	}

	private static void ReverseFourByteElements(byte[] buffer)
	{
		if (buffer == null)
		{
			return;
		}
		if ((buffer.Length & 3) != 0)
		{
			throw new InvalidDataException($"Expected a four-byte-aligned Xbox model buffer, got {buffer.Length} bytes.");
		}

		for (int i = 0; i < buffer.Length; i += 4)
		{
			Array.Reverse(buffer, i, 4);
		}
	}

	private static void ReverseTwoByteElements(byte[] buffer)
	{
		if (buffer == null || !BitConverter.IsLittleEndian)
		{
			return;
		}
		if ((buffer.Length & 1) != 0)
		{
			throw new InvalidDataException($"Expected a two-byte-aligned Xbox collision buffer, got {buffer.Length} bytes.");
		}

		for (int i = 0; i < buffer.Length; i += 2)
		{
			(buffer[i], buffer[i + 1]) = (buffer[i + 1], buffer[i]);
		}
	}

	public void Load_Condensed_Model(string filename, int modID)
	{
		Stream stream = TitleContainer.OpenStream("The_CoOp_Zombie_Game\\Models\\" + filename);
		if (stream.CanRead)
		{
			BinaryReader binaryReader = new BinaryReader(stream);
			mod1[modID] = default(StructsClass.model);
			StructsClass.Initialize_Model(ref mod1[modID]);
			if (binaryReader.ReadByte() != 0)
			{
				binaryReader.Close();
				stream.Close();
				Load_Condensed_Model_Version_0("\\Models\\" + filename, modID);
			}
			else
			{
				ushort num = binaryReader.ReadUInt16();
				ushort num2 = num;
				if (num2 != 1)
				{
					binaryReader.Close();
					stream.Close();
					return;
				}
				Load_Condensed_Model_Version_1(stream, binaryReader, modID);
			}
		}
		mod1[modID].vertexCount = 3 * mod1[modID].pcount;
		mod1[modID].triangleCount = mod1[modID].pcount;
		mod1[modID].indexBufferSize = 0;
		numModels++;
	}

	public void Load_Condensed_Model_Version_0(string filename, int modID)
	{
		Stream stream = TitleContainer.OpenStream("The_CoOp_Zombie_Game" + filename);
		if (stream.CanRead)
		{
			BinaryReader binaryReader = new BinaryReader(stream);
			mod1[modID].riggingStatus = 0;
			mod1[modID].name = binaryReader.ReadString();
			mod1[modID].texture = binaryReader.ReadString();
			mod1[modID].texID = mainC.texturesMain.Find_Texture(mod1[modID].texture, 0);
			mod1[modID].textureNormal = binaryReader.ReadString();
			mod1[modID].texNormalID = mainC.texturesMain.Find_Texture(mod1[modID].textureNormal, 0);
			mod1[modID].numTextures = binaryReader.ReadByte();
			int numTextures = mod1[modID].numTextures;
			int i;
			if (numTextures > 0)
			{
				mod1[modID].textureList = new int[numTextures];
				mod1[modID].textureListStart = new int[numTextures];
				mod1[modID].textureListNames = new string[numTextures];
				mod1[modID].textureListPrimitiveCnt = new int[numTextures];
				mod1[modID].textureListIndexCnt = new int[numTextures];
				for (i = 0; i < numTextures; i++)
				{
					mod1[modID].textureListNames[i] = binaryReader.ReadString();
					mod1[modID].textureList[i] = mainC.texturesMain.Find_Texture(mod1[modID].textureListNames[i], 0);
					mod1[modID].textureListStart[i] = binaryReader.ReadInt32();
					mod1[modID].textureListPrimitiveCnt[i] = binaryReader.ReadInt32();
					mod1[modID].textureListIndexCnt[i] = binaryReader.ReadInt32();
				}
				mod1[modID].texID = mod1[modID].textureList[0];
				mod1[modID].texture = mod1[modID].textureListNames[0];
			}
			mod1[modID].vcount = binaryReader.ReadInt32();
			mod1[modID].ncount = binaryReader.ReadInt32();
			mod1[modID].tcount = binaryReader.ReadInt32();
			mod1[modID].pcount = binaryReader.ReadInt32();
			mod1[modID].usesAlpha = binaryReader.ReadBoolean();
			mod1[modID].defaultColor[0] = binaryReader.ReadSingle();
			mod1[modID].defaultColor[1] = binaryReader.ReadSingle();
			mod1[modID].defaultColor[2] = binaryReader.ReadSingle();
			mod1[modID].defaultColor[3] = binaryReader.ReadSingle();
			mod1[modID].bufferType = binaryReader.ReadByte();
			mod1[modID].instanceCount = binaryReader.ReadInt16();
			mod1[modID].blendFunction = binaryReader.ReadByte();
			mod1[modID].numObjects = binaryReader.ReadByte();
			mod1[modID].numObjectRotations = binaryReader.ReadByte();
			mod1[modID].usesRigging = binaryReader.ReadByte();
			numTextures = mod1[modID].numObjects;
			if (numTextures > 0)
			{
				mod1[modID].x = new float[numTextures];
				mod1[modID].y = new float[numTextures];
				mod1[modID].z = new float[numTextures];
				mod1[modID].dimX = new short[numTextures];
				mod1[modID].dimY = new short[numTextures];
				mod1[modID].dimZ = new short[numTextures];
				if (mod1[modID].numObjectRotations > 0)
				{
					mod1[modID].rotX = new float[numTextures];
					mod1[modID].rotY = new float[numTextures];
					mod1[modID].rotZ = new float[numTextures];
					for (i = 0; i < numTextures; i++)
					{
						mod1[modID].x[i] = binaryReader.ReadSingle();
						mod1[modID].y[i] = binaryReader.ReadSingle();
						mod1[modID].z[i] = binaryReader.ReadSingle();
						mod1[modID].rotX[i] = binaryReader.ReadSingle();
						mod1[modID].rotY[i] = binaryReader.ReadSingle();
						mod1[modID].rotZ[i] = binaryReader.ReadSingle();
						mod1[modID].dimX[i] = binaryReader.ReadInt16();
						mod1[modID].dimY[i] = binaryReader.ReadInt16();
						mod1[modID].dimZ[i] = binaryReader.ReadInt16();
					}
				}
				else
				{
					for (i = 0; i < numTextures; i++)
					{
						mod1[modID].x[i] = binaryReader.ReadSingle();
						mod1[modID].y[i] = binaryReader.ReadSingle();
						mod1[modID].z[i] = binaryReader.ReadSingle();
						mod1[modID].dimX[i] = binaryReader.ReadInt16();
						mod1[modID].dimY[i] = binaryReader.ReadInt16();
						mod1[modID].dimZ[i] = binaryReader.ReadInt16();
					}
				}
			}
			if (mod1[modID].usesRigging == 0)
			{
				numTextures = mod1[modID].vcount * 3 * 4;
				mod1[modID].vertexBytes = new byte[numTextures];
				mod1[modID].vertexBytes = binaryReader.ReadBytes(numTextures);
			}
			else
			{
				i = mod1[modID].vcount;
				numTextures = i * 3 * 4;
				mod1[modID].vertexBytes = new byte[numTextures];
				mod1[modID].blendIndex0 = new byte[i];
				mod1[modID].blendIndex1 = new byte[i];
				mod1[modID].blendIndex2 = new byte[i];
				mod1[modID].blendIndex3 = new byte[i];
				mod1[modID].bwBytes0 = new byte[numTextures];
				mod1[modID].bwBytes1 = new byte[numTextures];
				mod1[modID].bwBytes2 = new byte[numTextures];
				mod1[modID].bwBytes3 = new byte[numTextures];
				mod1[modID].vertexBytes = binaryReader.ReadBytes(numTextures);
				mod1[modID].blendIndex0 = binaryReader.ReadBytes(i);
				for (numTextures = 0; numTextures < i; numTextures++)
				{
					mod1[modID].blendIndex1[numTextures] = mod1[modID].blendIndex0[numTextures];
					mod1[modID].blendIndex2[numTextures] = mod1[modID].blendIndex0[numTextures];
					mod1[modID].blendIndex3[numTextures] = mod1[modID].blendIndex0[numTextures];
				}
			}
			numTextures = mod1[modID].ncount * 3 * 4;
			mod1[modID].normalBytes = new byte[numTextures];
			mod1[modID].normalBytes = binaryReader.ReadBytes(numTextures);
			numTextures = mod1[modID].tcount * 2 * 4;
			mod1[modID].textureBytes = new byte[numTextures];
			mod1[modID].textureBytes = binaryReader.ReadBytes(numTextures);
			i = mod1[modID].pcount;
			numTextures = i * 3 * 4;
			i = i * 3 * 4;
			mod1[modID].vIndexBytes = new byte[numTextures];
			mod1[modID].nIndexBytes = new byte[numTextures];
			mod1[modID].tIndexBytes = new byte[numTextures];
			mod1[modID].tangentBytes = new byte[i];
			mod1[modID].vIndexBytes = binaryReader.ReadBytes(numTextures);
			mod1[modID].nIndexBytes = binaryReader.ReadBytes(numTextures);
			mod1[modID].tIndexBytes = binaryReader.ReadBytes(numTextures);
			mod1[modID].tangentBytes = binaryReader.ReadBytes(i);
			ConvertModelBuffersFromXboxEndian(ref mod1[modID]);
			binaryReader.Close();
		}
		stream.Close();
	}

	public void Load_Condensed_Model_Version_1(Stream fp, BinaryReader br, int modID)
	{
		if (fp.CanRead)
		{
			mod1[modID].riggingStatus = 0;
			mod1[modID].name = br.ReadString();
			mod1[modID].texture = br.ReadString();
			mod1[modID].texID = mainC.texturesMain.Find_Texture(mod1[modID].texture, 0);
			mod1[modID].textureNormal = br.ReadString();
			mod1[modID].texNormalID = mainC.texturesMain.Find_Texture(mod1[modID].textureNormal, 0);
			mod1[modID].texSpecular = br.ReadString();
			mod1[modID].texSpecularID = mainC.texturesMain.Find_Texture(mod1[modID].texSpecular, 0);
			mod1[modID].numTextures = br.ReadByte();
			int numTextures = mod1[modID].numTextures;
			int i;
			if (numTextures > 0)
			{
				mod1[modID].textureList = new int[numTextures];
				mod1[modID].textureListStart = new int[numTextures];
				mod1[modID].textureListNames = new string[numTextures];
				mod1[modID].textureListPrimitiveCnt = new int[numTextures];
				mod1[modID].textureListIndexCnt = new int[numTextures];
				for (i = 0; i < numTextures; i++)
				{
					mod1[modID].textureListNames[i] = br.ReadString();
					mod1[modID].textureList[i] = mainC.texturesMain.Find_Texture(mod1[modID].textureListNames[i], 0);
					mod1[modID].textureListStart[i] = br.ReadInt32();
					mod1[modID].textureListPrimitiveCnt[i] = br.ReadInt32();
					mod1[modID].textureListIndexCnt[i] = br.ReadInt32();
				}
				mod1[modID].texID = mod1[modID].textureList[0];
				mod1[modID].texture = mod1[modID].textureListNames[0];
			}
			mod1[modID].vcount = br.ReadInt32();
			mod1[modID].ncount = br.ReadInt32();
			mod1[modID].tcount = br.ReadInt32();
			mod1[modID].pcount = br.ReadInt32();
			mod1[modID].usesAlpha = br.ReadBoolean();
			mod1[modID].defaultColor[0] = br.ReadSingle();
			mod1[modID].defaultColor[1] = br.ReadSingle();
			mod1[modID].defaultColor[2] = br.ReadSingle();
			mod1[modID].defaultColor[3] = br.ReadSingle();
			mod1[modID].bufferType = br.ReadByte();
			mod1[modID].instanceCount = br.ReadInt16();
			mod1[modID].blendFunction = br.ReadByte();
			mod1[modID].numObjects = br.ReadByte();
			mod1[modID].numObjectRotations = br.ReadByte();
			mod1[modID].usesRigging = br.ReadByte();
			numTextures = mod1[modID].numObjects;
			if (numTextures > 0)
			{
				mod1[modID].x = new float[numTextures];
				mod1[modID].y = new float[numTextures];
				mod1[modID].z = new float[numTextures];
				mod1[modID].dimX = new short[numTextures];
				mod1[modID].dimY = new short[numTextures];
				mod1[modID].dimZ = new short[numTextures];
				if (mod1[modID].numObjectRotations > 0)
				{
					mod1[modID].rotX = new float[numTextures];
					mod1[modID].rotY = new float[numTextures];
					mod1[modID].rotZ = new float[numTextures];
					for (i = 0; i < numTextures; i++)
					{
						mod1[modID].x[i] = br.ReadSingle();
						mod1[modID].y[i] = br.ReadSingle();
						mod1[modID].z[i] = br.ReadSingle();
						mod1[modID].rotX[i] = br.ReadSingle();
						mod1[modID].rotY[i] = br.ReadSingle();
						mod1[modID].rotZ[i] = br.ReadSingle();
						mod1[modID].dimX[i] = br.ReadInt16();
						mod1[modID].dimY[i] = br.ReadInt16();
						mod1[modID].dimZ[i] = br.ReadInt16();
					}
				}
				else
				{
					for (i = 0; i < numTextures; i++)
					{
						mod1[modID].x[i] = br.ReadSingle();
						mod1[modID].y[i] = br.ReadSingle();
						mod1[modID].z[i] = br.ReadSingle();
						mod1[modID].dimX[i] = br.ReadInt16();
						mod1[modID].dimY[i] = br.ReadInt16();
						mod1[modID].dimZ[i] = br.ReadInt16();
					}
				}
			}
			if (mod1[modID].usesRigging == 0)
			{
				numTextures = mod1[modID].vcount * 3 * 4;
				mod1[modID].vertexBytes = new byte[numTextures];
				mod1[modID].vertexBytes = br.ReadBytes(numTextures);
			}
			else
			{
				i = mod1[modID].vcount;
				numTextures = i * 3 * 4;
				mod1[modID].vertexBytes = new byte[numTextures];
				mod1[modID].vertexBytes = br.ReadBytes(numTextures);
				mod1[modID].blendIndex0 = new byte[i];
				mod1[modID].blendIndex0 = br.ReadBytes(i);
				mod1[modID].blendIndex1 = new byte[i];
				mod1[modID].blendIndex1 = br.ReadBytes(i);
				mod1[modID].blendIndex2 = new byte[i];
				mod1[modID].blendIndex2 = br.ReadBytes(i);
				mod1[modID].blendIndex3 = new byte[i];
				mod1[modID].blendIndex3 = br.ReadBytes(i);
				numTextures = i * 4;
				mod1[modID].bwBytes0 = new byte[numTextures];
				mod1[modID].bwBytes0 = br.ReadBytes(numTextures);
				mod1[modID].bwBytes1 = new byte[numTextures];
				mod1[modID].bwBytes1 = br.ReadBytes(numTextures);
				mod1[modID].bwBytes2 = new byte[numTextures];
				mod1[modID].bwBytes2 = br.ReadBytes(numTextures);
				mod1[modID].bwBytes3 = new byte[numTextures];
				mod1[modID].bwBytes3 = br.ReadBytes(numTextures);
			}
			numTextures = mod1[modID].ncount * 3 * 4;
			mod1[modID].normalBytes = new byte[numTextures];
			mod1[modID].normalBytes = br.ReadBytes(numTextures);
			numTextures = mod1[modID].tcount * 2 * 4;
			mod1[modID].textureBytes = new byte[numTextures];
			mod1[modID].textureBytes = br.ReadBytes(numTextures);
			i = mod1[modID].pcount;
			numTextures = i * 3 * 4;
			i = i * 3 * 4;
			mod1[modID].vIndexBytes = new byte[numTextures];
			mod1[modID].nIndexBytes = new byte[numTextures];
			mod1[modID].tIndexBytes = new byte[numTextures];
			mod1[modID].tangentBytes = new byte[i];
			mod1[modID].vIndexBytes = br.ReadBytes(numTextures);
			mod1[modID].nIndexBytes = br.ReadBytes(numTextures);
			mod1[modID].tIndexBytes = br.ReadBytes(numTextures);
			mod1[modID].tangentBytes = br.ReadBytes(i);
			ConvertModelBuffersFromXboxEndian(ref mod1[modID]);
			br.Close();
		}
		fp.Close();
	}

	public void Load_Collision_Model(string filename)
	{
	}

	public unsafe void Save_Collision_Model_In_Condensed_Format(string filename)
	{
		FileStream fileStream = File.Open(Environment.CurrentDirectory + "\\The_CoOp_Zombie_Game\\Models_Collision_Converted\\" + filename, FileMode.Create, FileAccess.Write);
		if (fileStream.CanWrite)
		{
			BinaryWriter binaryWriter = new BinaryWriter(fileStream);
			binaryWriter.Write(tempModel.name);
			binaryWriter.Write(tempModel.vcount);
			binaryWriter.Write(tempModel.ncount);
			binaryWriter.Write(tempModel.pcount);
			int vcount = tempModel.vcount;
			for (int i = 0; i < vcount; i++)
			{
				fixed (float* ptr = &tempModel.v1[i].v[0])
				{
					fixed (float* ptr2 = &tempModel.v1[i].v[1])
					{
						fixed (float* ptr3 = &tempModel.v1[i].v[2])
						{
							byte* bptr = (byte*)ptr;
							mainC.utilMain.Write_Float_Reversed(binaryWriter, bptr);
							bptr = (byte*)ptr2;
							mainC.utilMain.Write_Float_Reversed(binaryWriter, bptr);
							bptr = (byte*)ptr3;
							mainC.utilMain.Write_Float_Reversed(binaryWriter, bptr);
						}
					}
				}
			}
			vcount = tempModel.ncount;
			for (int i = 0; i < vcount; i++)
			{
				float num = tempModel.n1[i].n[0];
				float num2 = tempModel.n1[i].n[1];
				float num3 = tempModel.n1[i].n[2];
				float num4 = (float)Math.Sqrt(num * num + num2 * num2 + num3 * num3);
				if (num4 != 0f)
				{
					num /= num4;
					num2 /= num4;
					num3 /= num4;
				}
				tempModel.n1[i].n[0] = num;
				tempModel.n1[i].n[1] = num2;
				tempModel.n1[i].n[2] = num3;
			}
			for (int i = 0; i < vcount; i++)
			{
				fixed (float* ptr4 = &tempModel.n1[i].n[0])
				{
					fixed (float* ptr5 = &tempModel.n1[i].n[1])
					{
						fixed (float* ptr6 = &tempModel.n1[i].n[2])
						{
							byte* bptr = (byte*)ptr4;
							mainC.utilMain.Write_Float_Reversed(binaryWriter, bptr);
							bptr = (byte*)ptr5;
							mainC.utilMain.Write_Float_Reversed(binaryWriter, bptr);
							bptr = (byte*)ptr6;
							mainC.utilMain.Write_Float_Reversed(binaryWriter, bptr);
						}
					}
				}
			}
			vcount = tempModel.pcount;
			for (int i = 0; i < vcount; i++)
			{
				fixed (int* ptr7 = &tempModel.p1[i].v[0])
				{
					fixed (int* ptr8 = &tempModel.p1[i].v[1])
					{
						fixed (int* ptr9 = &tempModel.p1[i].v[2])
						{
							byte* bptr = (byte*)ptr7;
							mainC.utilMain.Write_Int_Reversed(binaryWriter, bptr);
							bptr = (byte*)ptr8;
							mainC.utilMain.Write_Int_Reversed(binaryWriter, bptr);
							bptr = (byte*)ptr9;
							mainC.utilMain.Write_Int_Reversed(binaryWriter, bptr);
						}
					}
				}
			}
			for (int i = 0; i < vcount; i++)
			{
				fixed (int* ptr10 = &tempModel.p1[i].n[0])
				{
					fixed (int* ptr11 = &tempModel.p1[i].n[1])
					{
						fixed (int* ptr12 = &tempModel.p1[i].n[2])
						{
							byte* bptr = (byte*)ptr10;
							mainC.utilMain.Write_Int_Reversed(binaryWriter, bptr);
							bptr = (byte*)ptr11;
							mainC.utilMain.Write_Int_Reversed(binaryWriter, bptr);
							bptr = (byte*)ptr12;
							mainC.utilMain.Write_Int_Reversed(binaryWriter, bptr);
						}
					}
				}
			}
			int num5 = 0;
			vcount = tempColModel.numBoxes;
			for (int i = 0; i < vcount; i++)
			{
				num5 += tempColModel.cb[i].numIDs;
			}
			binaryWriter.Write(tempColModel.collisionScheme);
			binaryWriter.Write(tempColModel.curDiv);
			binaryWriter.Write(tempColModel.dx);
			binaryWriter.Write(tempColModel.dy);
			binaryWriter.Write(tempColModel.minX);
			binaryWriter.Write(tempColModel.minY);
			binaryWriter.Write(tempColModel.numBoxes);
			binaryWriter.Write(tempColModel.id);
			binaryWriter.Write(num5);
			for (int i = 0; i < vcount; i++)
			{
				binaryWriter.Write(tempColModel.cb[i].type);
			}
			for (int i = 0; i < vcount; i++)
			{
				fixed (ushort* id = &tempColModel.cb[i].id)
				{
					byte* bptr = (byte*)id;
					mainC.utilMain.Write_Ushort_Reversed(binaryWriter, bptr);
				}
			}
			for (int i = 0; i < vcount; i++)
			{
				fixed (float* x = &tempColModel.cb[i].x)
				{
					fixed (float* y = &tempColModel.cb[i].y)
					{
						fixed (float* z = &tempColModel.cb[i].z)
						{
							fixed (float* x2 = &tempColModel.cb[i].x2)
							{
								fixed (float* y2 = &tempColModel.cb[i].y2)
								{
									fixed (float* z2 = &tempColModel.cb[i].z2)
									{
										byte* bptr = (byte*)x;
										mainC.utilMain.Write_Float_Reversed(binaryWriter, bptr);
										bptr = (byte*)y;
										mainC.utilMain.Write_Float_Reversed(binaryWriter, bptr);
										bptr = (byte*)z;
										mainC.utilMain.Write_Float_Reversed(binaryWriter, bptr);
										bptr = (byte*)x2;
										mainC.utilMain.Write_Float_Reversed(binaryWriter, bptr);
										bptr = (byte*)y2;
										mainC.utilMain.Write_Float_Reversed(binaryWriter, bptr);
										bptr = (byte*)z2;
										mainC.utilMain.Write_Float_Reversed(binaryWriter, bptr);
									}
								}
							}
						}
					}
				}
			}
			for (int i = 0; i < vcount; i++)
			{
				fixed (ushort* numIDs = &tempColModel.cb[i].numIDs)
				{
					byte* bptr = (byte*)numIDs;
					mainC.utilMain.Write_Ushort_Reversed(binaryWriter, bptr);
				}
			}
			for (int i = 0; i < vcount; i++)
			{
				for (int j = 0; j < tempColModel.cb[i].numIDs; j++)
				{
					fixed (ushort* ptr13 = &tempColModel.cb[i].ids[j])
					{
						byte* bptr = (byte*)ptr13;
						mainC.utilMain.Write_Ushort_Reversed(binaryWriter, bptr);
					}
				}
			}
		}
		fileStream.Close();
	}

	public unsafe void Export_Collision_Model()
	{
		for (ushort num = 0; num < global::MainGame.MainGame.numCollisionModels; num++)
		{
			Load_Condensed_Collision_Model(global::Collision.Collision.cModels[num].fileName);
			tempModel.v1 = new StructsClass.vtex[tempModel.vcount];
			tempModel.n1 = new StructsClass.vnorm[tempModel.ncount];
			tempModel.p1 = new StructsClass.poly[tempModel.pcount];
			fixed (byte* vertexBytes = tempModel.vertexBytes)
			{
				fixed (byte* normalBytes = tempModel.normalBytes)
				{
					fixed (byte* vIndexBytes = tempModel.vIndexBytes)
					{
						fixed (byte* nIndexBytes = tempModel.nIndexBytes)
						{
							float* ptr = (float*)vertexBytes;
							float* ptr2 = (float*)normalBytes;
							int* ptr3 = (int*)vIndexBytes;
							int* ptr4 = (int*)nIndexBytes;
							for (int i = 0; i < tempModel.vcount; i++)
							{
								tempModel.v1[i] = new StructsClass.vtex();
								tempModel.v1[i].v[0] = *(ptr++);
								tempModel.v1[i].v[1] = *(ptr++);
								tempModel.v1[i].v[2] = *(ptr++);
							}
							for (int i = 0; i < tempModel.ncount; i++)
							{
								tempModel.n1[i] = new StructsClass.vnorm();
								tempModel.n1[i].n[0] = *(ptr2++);
								tempModel.n1[i].n[1] = *(ptr2++);
								tempModel.n1[i].n[2] = *(ptr2++);
							}
							for (int i = 0; i < tempModel.pcount; i++)
							{
								tempModel.p1[i] = default(StructsClass.poly);
								tempModel.p1[i].v = new int[3];
								tempModel.p1[i].n = new int[3];
								tempModel.p1[i].v[0] = *(ptr3++);
								tempModel.p1[i].n[0] = *(ptr4++);
								tempModel.p1[i].v[1] = *(ptr3++);
								tempModel.p1[i].n[1] = *(ptr4++);
								tempModel.p1[i].v[2] = *(ptr3++);
								tempModel.p1[i].n[2] = *(ptr4++);
							}
						}
					}
				}
			}
			mainC.collisionMain.Create_Collision_Model_Bounding_Box(num);
			tempColModel.collisionScheme = global::Collision.Collision.cModels[num].collisionScheme;
			tempColModel.curDiv = global::Collision.Collision.cModels[num].curDiv;
			tempColModel.dx = global::Collision.Collision.cModels[num].dx;
			tempColModel.dy = global::Collision.Collision.cModels[num].dy;
			tempColModel.minX = global::Collision.Collision.cModels[num].minX;
			tempColModel.minY = global::Collision.Collision.cModels[num].minY;
			tempColModel.numBoxes = global::Collision.Collision.cModels[num].numBoxes;
			tempColModel.id = global::Collision.Collision.cModels[num].id;
			int numBoxes = global::Collision.Collision.cModels[num].numBoxes;
			tempColModel.cb = new StructsClass.Collision_Model_Box[numBoxes];
			for (int i = 0; i < numBoxes; i++)
			{
				tempColModel.cb[i] = default(StructsClass.Collision_Model_Box);
			}
			for (int i = 0; i < numBoxes; i++)
			{
				tempColModel.cb[i].type = global::Collision.Collision.cModels[num].cb[i].type;
				tempColModel.cb[i].id = global::Collision.Collision.cModels[num].cb[i].id;
				tempColModel.cb[i].x = global::Collision.Collision.cModels[num].cb[i].x;
				tempColModel.cb[i].y = global::Collision.Collision.cModels[num].cb[i].y;
				tempColModel.cb[i].z = global::Collision.Collision.cModels[num].cb[i].z;
				tempColModel.cb[i].x2 = global::Collision.Collision.cModels[num].cb[i].x2;
				tempColModel.cb[i].y2 = global::Collision.Collision.cModels[num].cb[i].y2;
				tempColModel.cb[i].z2 = global::Collision.Collision.cModels[num].cb[i].z2;
				tempColModel.cb[i].numIDs = global::Collision.Collision.cModels[num].cb[i].numIDs;
				tempColModel.cb[i].ids = new ushort[global::Collision.Collision.cModels[num].cb[i].numIDs];
				for (int j = 0; j < global::Collision.Collision.cModels[num].cb[i].numIDs; j++)
				{
					tempColModel.cb[i].ids[j] = global::Collision.Collision.cModels[num].cb[i].ids[j];
				}
			}
			Save_Collision_Model_In_Condensed_Format(global::Collision.Collision.cModels[num].fileName);
		}
	}

	public unsafe void Load_Condensed_Collision_Model(string filename)
	{
		Stream stream = TitleContainer.OpenStream("The_CoOp_Zombie_Game\\CollisionData\\" + filename);
		if (stream.CanRead)
		{
			BinaryReader binaryReader = new BinaryReader(stream);
			tempModel.name = binaryReader.ReadString();
			tempModel.vcount = binaryReader.ReadInt32();
			tempModel.ncount = binaryReader.ReadInt32();
			tempModel.pcount = binaryReader.ReadInt32();
			int num = tempModel.vcount * 3 * 4;
			tempModel.vertexBytes = new byte[num];
			tempModel.vertexBytes = binaryReader.ReadBytes(num);
			num = tempModel.ncount * 3 * 4;
			tempModel.normalBytes = new byte[num];
			tempModel.normalBytes = binaryReader.ReadBytes(num);
			int pcount = tempModel.pcount;
			num = pcount * 3 * 4;
			pcount = pcount * 3 * 4;
			tempModel.vIndexBytes = new byte[num];
			tempModel.nIndexBytes = new byte[num];
			tempModel.vIndexBytes = binaryReader.ReadBytes(num);
			tempModel.nIndexBytes = binaryReader.ReadBytes(num);
			if (BitConverter.IsLittleEndian)
			{
				ReverseFourByteElements(tempModel.vertexBytes);
				ReverseFourByteElements(tempModel.normalBytes);
				ReverseFourByteElements(tempModel.vIndexBytes);
				ReverseFourByteElements(tempModel.nIndexBytes);
			}
			try
			{
				tempColModel.collisionScheme = binaryReader.ReadByte();
				tempColModel.curDiv = binaryReader.ReadInt32();
				tempColModel.dx = binaryReader.ReadSingle();
				tempColModel.dy = binaryReader.ReadSingle();
				tempColModel.minX = binaryReader.ReadSingle();
				tempColModel.minY = binaryReader.ReadSingle();
				tempColModel.numBoxes = binaryReader.ReadInt32();
				tempColModel.id = binaryReader.ReadUInt16();
				int num2 = binaryReader.ReadInt32();
				int numBoxes = tempColModel.numBoxes;
				tempColModel.cb = new StructsClass.Collision_Model_Box[numBoxes];
				for (pcount = 0; pcount < numBoxes; pcount++)
				{
					tempColModel.cb[pcount] = default(StructsClass.Collision_Model_Box);
				}
				byte[] array = new byte[numBoxes];
				array = binaryReader.ReadBytes(numBoxes);
				fixed (byte* ptr = array)
				{
					for (pcount = 0; pcount < numBoxes; pcount++)
					{
						tempColModel.cb[pcount].type = ptr[pcount];
					}
				}
				num = numBoxes * 2;
				array = new byte[num];
				array = binaryReader.ReadBytes(num);
				ReverseTwoByteElements(array);
				fixed (byte* ptr2 = array)
				{
					ushort* ptr3 = (ushort*)ptr2;
					for (pcount = 0; pcount < numBoxes; pcount++)
					{
						tempColModel.cb[pcount].id = ptr3[pcount];
					}
				}
				num = numBoxes * 6 * 4;
				array = new byte[num];
				array = binaryReader.ReadBytes(num);
				if (BitConverter.IsLittleEndian)
				{
					ReverseFourByteElements(array);
				}
				fixed (byte* ptr4 = array)
				{
					float* ptr5 = (float*)ptr4;
					pcount = 0;
					num = 0;
					for (; pcount < numBoxes; pcount++)
					{
						tempColModel.cb[pcount].x = ptr5[num++];
						tempColModel.cb[pcount].y = ptr5[num++];
						tempColModel.cb[pcount].z = ptr5[num++];
						tempColModel.cb[pcount].x2 = ptr5[num++];
						tempColModel.cb[pcount].y2 = ptr5[num++];
						tempColModel.cb[pcount].z2 = ptr5[num++];
					}
				}
				num = numBoxes * 2;
				array = new byte[num];
				array = binaryReader.ReadBytes(num);
				ReverseTwoByteElements(array);
				fixed (byte* ptr6 = array)
				{
					ushort* ptr3 = (ushort*)ptr6;
					for (pcount = 0; pcount < numBoxes; pcount++)
					{
						tempColModel.cb[pcount].numIDs = ptr3[pcount];
					}
				}
				num = num2 * 2;
				array = new byte[num];
				array = binaryReader.ReadBytes(num);
				ReverseTwoByteElements(array);
				fixed (byte* ptr7 = array)
				{
					ushort* ptr3 = (ushort*)ptr7;
					pcount = 0;
					int num3 = 0;
					for (; pcount < numBoxes; pcount++)
					{
						tempColModel.cb[pcount].ids = new ushort[tempColModel.cb[pcount].numIDs];
						for (num = 0; num < tempColModel.cb[pcount].numIDs; num++)
						{
							tempColModel.cb[pcount].ids[num] = ptr3[num3++];
						}
					}
				}
			}
			catch
			{
				global::InputHandler.InputHandler.tw = 230f;
			}
		}
		stream.Close();
		tempModel.vertexCount = 3 * tempModel.pcount;
	}

	public void Load_Model_Lists(string fileName)
	{
		int num = -1;
		_ = global::Rendering.Rendering.uBufferID;
		for (int i = 0; i < numModelLists; i++)
		{
			modelList[i].numModels = 0;
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
				if (array4[0].Equals("numModelLists", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 1;
				}
				else if (array4[0].Equals("modelList", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 2;
				}
				else if (array4[0].Equals("numModels", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 3;
				}
				else if (array4[0].Equals("texture1", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 4;
				}
				else if (array4[0].Equals("texture2", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 5;
				}
				else if (array4[0].Equals("buffer", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 6;
				}
				else if (array4[0].Equals("id", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 7;
				}
				else if (array4[0].Equals("alphaBlend", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 8;
				}
				else if (array4[0].Equals("position", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 9;
				}
				switch (num4)
				{
				case 1:
				{
					if (array4.Length <= 1)
					{
						break;
					}
					int l = int.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
					if (l > numAllocatedModelLists)
					{
						modelList = new StructsClass.Model_List[l];
						for (int i = 0; i < l; i++)
						{
							modelList[i].numAllocatedModels = 0;
							modelList[i].numModels = 0;
						}
						numAllocatedModelLists = (byte)l;
					}
					numModelLists = (byte)l;
					break;
				}
				case 2:
					num++;
					if (num >= numAllocatedModelLists)
					{
						num = -1;
					}
					break;
				case 3:
				{
					if (array4.Length <= 1 || num <= -1)
					{
						break;
					}
					byte b = byte.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
					if (b > modelList[num].numAllocatedModels)
					{
						modelList[num].modelID = new short[b];
						modelList[num].x = new float[b];
						modelList[num].y = new float[b];
						modelList[num].z = new float[b];
						modelList[num].modelName = new string[b];
						modelList[num].texID = new short[b];
						modelList[num].texID2 = new short[b];
						modelList[num].bufferID = new byte[b];
						modelList[num].alphaBlend = new byte[b];
						modelList[num].numAllocatedModels = b;
					}
					modelList[num].numModels = b;
					int l = 0;
					int i = 0;
					for (; l < b; l++)
					{
						modelList[num].modelID[l] = 0;
						modelList[num].modelName[l] = "";
						modelList[num].texID[l] = -1;
						modelList[num].texID2[l] = -1;
						modelList[num].bufferID[l] = byte.MaxValue;
						modelList[num].alphaBlend[l] = byte.MaxValue;
					}
					if (array4.Length > b + 1)
					{
						l = 0;
						i = 2;
						while (l < b)
						{
							modelList[num].modelName[l] = array4[i];
							modelList[num].modelID[l] = (short)Find_Model(array4[i]);
							l++;
							i++;
						}
					}
					break;
				}
				case 4:
					if (num > -1 && array4.Length > modelList[num].numModels)
					{
						byte b = modelList[num].numModels;
						int l = 0;
						int i = 1;
						while (l < b)
						{
							modelList[num].texID[l] = mainC.texturesMain.Find_Texture(array4[i], 0);
							l++;
							i++;
						}
					}
					break;
				case 5:
					if (num > -1 && array4.Length > modelList[num].numModels)
					{
						byte b = modelList[num].numModels;
						int l = 0;
						int i = 1;
						while (l < b)
						{
							modelList[num].texID2[l] = mainC.texturesMain.Find_Texture(array4[i], 0);
							l++;
							i++;
						}
					}
					break;
				case 6:
					if (num > -1 && array4.Length > modelList[num].numModels)
					{
						byte b = modelList[num].numModels;
						int l = 0;
						int i = 1;
						while (l < b)
						{
							modelList[num].bufferID[l] = byte.Parse(array4[i], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
							l++;
							i++;
						}
					}
					break;
				case 7:
					if (array4.Length > 1 && num > -1)
					{
						modelList[num].id = short.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 8:
					if (num > -1 && array4.Length > modelList[num].numModels)
					{
						byte b = modelList[num].numModels;
						int l = 0;
						int i = 1;
						while (l < b)
						{
							modelList[num].alphaBlend[l] = byte.Parse(array4[i], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
							l++;
							i++;
						}
					}
					break;
				case 9:
					if (num > -1 && array4.Length > modelList[num].numModels * 3)
					{
						byte b = modelList[num].numModels;
						int l = 0;
						int i = 1;
						for (; l < b; l++)
						{
							modelList[num].x[l] = float.Parse(array4[i++], CultureInfo.InvariantCulture.NumberFormat);
							modelList[num].y[l] = float.Parse(array4[i++], CultureInfo.InvariantCulture.NumberFormat);
							modelList[num].z[l] = float.Parse(array4[i++], CultureInfo.InvariantCulture.NumberFormat);
						}
					}
					break;
				}
			}
		}
		stream.Close();
		for (int i = 0; i < numModelLists; i++)
		{
			for (int l = 0; l < modelList[i].numModels; l++)
			{
				if (modelList[i].texID[l] == -1)
				{
					modelList[i].texID[l] = (short)mod1[modelList[i].modelID[l]].texID;
				}
				if (modelList[i].texID2[l] == -1)
				{
					modelList[i].texID2[l] = (short)mod1[modelList[i].modelID[l]].texNormalID;
				}
				if (modelList[i].bufferID[l] == byte.MaxValue)
				{
					modelList[i].bufferID[l] = 0;
				}
				if (modelList[i].alphaBlend[l] == byte.MaxValue)
				{
					modelList[i].alphaBlend[l] = 0;
				}
			}
		}
	}

	public void Load_Model_List(ref StructsClass.Model_List ml1, string fileName)
	{
		_ = global::Rendering.Rendering.uBufferID;
		ml1.numModels = 0;
		Stream stream = TitleContainer.OpenStream("The_CoOp_Zombie_Game\\Config_Files\\" + fileName);
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
				if (array4[0].Equals("numModels", StringComparison.OrdinalIgnoreCase))
				{
					num3 = 3;
				}
				else if (array4[0].Equals("texture1", StringComparison.OrdinalIgnoreCase))
				{
					num3 = 4;
				}
				else if (array4[0].Equals("texture2", StringComparison.OrdinalIgnoreCase))
				{
					num3 = 5;
				}
				else if (array4[0].Equals("buffer", StringComparison.OrdinalIgnoreCase))
				{
					num3 = 6;
				}
				else if (array4[0].Equals("id", StringComparison.OrdinalIgnoreCase))
				{
					num3 = 7;
				}
				else if (array4[0].Equals("alphaBlend", StringComparison.OrdinalIgnoreCase))
				{
					num3 = 8;
				}
				else if (array4[0].Equals("position", StringComparison.OrdinalIgnoreCase))
				{
					num3 = 9;
				}
				switch (num3)
				{
				case 3:
				{
					if (array4.Length <= 1)
					{
						break;
					}
					byte b = byte.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
					ml1.modelID = new short[b];
					ml1.x = new float[b];
					ml1.y = new float[b];
					ml1.z = new float[b];
					ml1.modelName = new string[b];
					ml1.texID = new short[b];
					ml1.texID2 = new short[b];
					ml1.bufferID = new byte[b];
					ml1.alphaBlend = new byte[b];
					ml1.numAllocatedModels = b;
					ml1.numModels = b;
					int k = 0;
					int num4 = 0;
					for (; k < b; k++)
					{
						ml1.modelID[k] = 0;
						ml1.modelName[k] = "";
						ml1.texID[k] = -1;
						ml1.texID2[k] = -1;
						ml1.bufferID[k] = byte.MaxValue;
						ml1.alphaBlend[k] = byte.MaxValue;
					}
					if (array4.Length > b + 1)
					{
						k = 0;
						num4 = 2;
						while (k < b)
						{
							ml1.modelName[k] = array4[num4];
							ml1.modelID[k] = (short)Find_Model(array4[num4]);
							k++;
							num4++;
						}
					}
					break;
				}
				case 4:
					if (array4.Length > ml1.numModels)
					{
						byte b = ml1.numModels;
						int k = 0;
						int num4 = 1;
						while (k < b)
						{
							ml1.texID[k] = mainC.texturesMain.Find_Texture(array4[num4], 0);
							k++;
							num4++;
						}
					}
					break;
				case 5:
					if (array4.Length > ml1.numModels)
					{
						byte b = ml1.numModels;
						int k = 0;
						int num4 = 1;
						while (k < b)
						{
							ml1.texID2[k] = mainC.texturesMain.Find_Texture(array4[num4], 0);
							k++;
							num4++;
						}
					}
					break;
				case 6:
					if (array4.Length > ml1.numModels)
					{
						byte b = ml1.numModels;
						int k = 0;
						int num4 = 1;
						while (k < b)
						{
							ml1.bufferID[k] = byte.Parse(array4[num4], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
							k++;
							num4++;
						}
					}
					break;
				case 7:
					if (array4.Length > 1)
					{
						ml1.id = short.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 8:
					if (array4.Length > ml1.numModels)
					{
						byte b = ml1.numModels;
						int k = 0;
						int num4 = 1;
						while (k < b)
						{
							ml1.alphaBlend[k] = byte.Parse(array4[num4], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
							k++;
							num4++;
						}
					}
					break;
				case 9:
					if (array4.Length > ml1.numModels * 3)
					{
						byte b = ml1.numModels;
						int k = 0;
						int num4 = 1;
						for (; k < b; k++)
						{
							ml1.x[k] = float.Parse(array4[num4++], CultureInfo.InvariantCulture.NumberFormat);
							ml1.y[k] = float.Parse(array4[num4++], CultureInfo.InvariantCulture.NumberFormat);
							ml1.z[k] = float.Parse(array4[num4++], CultureInfo.InvariantCulture.NumberFormat);
						}
					}
					break;
				}
			}
		}
		stream.Close();
		for (int k = 0; k < ml1.numModels; k++)
		{
			if (ml1.texID[k] == -1)
			{
				ml1.texID[k] = (short)mod1[ml1.modelID[k]].texID;
			}
			if (ml1.texID2[k] == -1)
			{
				ml1.texID2[k] = (short)mod1[ml1.modelID[k]].texNormalID;
			}
			if (ml1.bufferID[k] == byte.MaxValue)
			{
				ml1.bufferID[k] = 0;
			}
			if (ml1.alphaBlend[k] == byte.MaxValue)
			{
				ml1.alphaBlend[k] = 0;
			}
		}
	}

	public void Load_All_Level_Models(string filename)
	{
		_ = global::Rendering.Rendering.uBufferID;
		numLevelModels = 0;
		numAllocatedLevelModels = 0;
		modVbo = null;
		Stream stream = TitleContainer.OpenStream("The_CoOp_Zombie_Game\\Config_Files\\" + filename);
		byte[] array = new byte[stream.Length];
		if (!stream.CanRead)
		{
			return;
		}
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
			numLevelModels = array4.Length;
			if (numLevelModels > numAllocatedLevelModels)
			{
				modVbo = new StructsClass.model[numLevelModels];
				for (int k = 0; k < numLevelModels; k++)
				{
					modVbo[k] = default(StructsClass.model);
					StructsClass.Initialize_Model(ref modVbo[k]);
				}
				numAllocatedLevelModels = numLevelModels;
			}
			for (int k = 0; k < numLevelModels; k++)
			{
				Load_Condensed_Level_Model(array4[k], k);
			}
		}
		stream.Close();
	}

	public void Load_Condensed_Level_Model(string filename, int modID)
	{
		Stream stream = TitleContainer.OpenStream("The_CoOp_Zombie_Game\\Models_Level\\" + filename);
		if (stream.CanRead)
		{
			BinaryReader binaryReader = new BinaryReader(stream);
			modVbo[modID] = default(StructsClass.model);
			StructsClass.Initialize_Model(ref modVbo[modID]);
			if (binaryReader.ReadByte() != 0)
			{
				binaryReader.Close();
				stream.Close();
				Load_Condensed_Level_Model_Version_0("\\Models_Level\\" + filename, modID);
			}
			else
			{
				ushort num = binaryReader.ReadUInt16();
				ushort num2 = num;
				if (num2 != 1)
				{
					binaryReader.Close();
					stream.Close();
					return;
				}
				Load_Condensed_Level_Model_Version_1(stream, binaryReader, modID);
			}
		}
		modVbo[modID].vertexCount = 3 * modVbo[modID].pcount;
		modVbo[modID].triangleCount = modVbo[modID].pcount;
		modVbo[modID].indexBufferSize = 0;
	}

	public void Load_Condensed_Level_Model_Version_0(string filename, int modID)
	{
		Stream stream = TitleContainer.OpenStream("The_CoOp_Zombie_Game" + filename);
		if (stream.CanRead)
		{
			BinaryReader binaryReader = new BinaryReader(stream);
			modVbo[modID].inLevelVBO = false;
			modVbo[modID].riggingStatus = 0;
			modVbo[modID].name = binaryReader.ReadString();
			modVbo[modID].texture = binaryReader.ReadString();
			modVbo[modID].texID = mainC.texturesMain.Find_Texture(modVbo[modID].texture, 0);
			modVbo[modID].textureNormal = binaryReader.ReadString();
			modVbo[modID].texNormalID = mainC.texturesMain.Find_Texture(modVbo[modID].textureNormal, 0);
			modVbo[modID].numTextures = binaryReader.ReadByte();
			int numTextures = modVbo[modID].numTextures;
			int i;
			if (numTextures > 0)
			{
				modVbo[modID].textureList = new int[numTextures];
				modVbo[modID].textureListStart = new int[numTextures];
				modVbo[modID].textureListNames = new string[numTextures];
				modVbo[modID].textureListPrimitiveCnt = new int[numTextures];
				modVbo[modID].textureListIndexCnt = new int[numTextures];
				for (i = 0; i < numTextures; i++)
				{
					modVbo[modID].textureListNames[i] = binaryReader.ReadString();
					modVbo[modID].textureList[i] = mainC.texturesMain.Find_Texture(modVbo[modID].textureListNames[i], 0);
					modVbo[modID].textureListStart[i] = binaryReader.ReadInt32();
					modVbo[modID].textureListPrimitiveCnt[i] = binaryReader.ReadInt32();
					modVbo[modID].textureListIndexCnt[i] = binaryReader.ReadInt32();
				}
				modVbo[modID].texID = modVbo[modID].textureList[0];
				modVbo[modID].texture = modVbo[modID].textureListNames[0];
			}
			modVbo[modID].vcount = binaryReader.ReadInt32();
			modVbo[modID].ncount = binaryReader.ReadInt32();
			modVbo[modID].tcount = binaryReader.ReadInt32();
			modVbo[modID].pcount = binaryReader.ReadInt32();
			modVbo[modID].usesAlpha = binaryReader.ReadBoolean();
			modVbo[modID].defaultColor[0] = binaryReader.ReadSingle();
			modVbo[modID].defaultColor[1] = binaryReader.ReadSingle();
			modVbo[modID].defaultColor[2] = binaryReader.ReadSingle();
			modVbo[modID].defaultColor[3] = binaryReader.ReadSingle();
			modVbo[modID].bufferType = binaryReader.ReadByte();
			modVbo[modID].instanceCount = binaryReader.ReadInt16();
			modVbo[modID].blendFunction = binaryReader.ReadByte();
			modVbo[modID].numObjects = binaryReader.ReadByte();
			modVbo[modID].numObjectRotations = binaryReader.ReadByte();
			modVbo[modID].usesRigging = binaryReader.ReadByte();
			numTextures = modVbo[modID].numObjects;
			if (numTextures > 0)
			{
				modVbo[modID].x = new float[numTextures];
				modVbo[modID].y = new float[numTextures];
				modVbo[modID].z = new float[numTextures];
				modVbo[modID].dimX = new short[numTextures];
				modVbo[modID].dimY = new short[numTextures];
				modVbo[modID].dimZ = new short[numTextures];
				if (modVbo[modID].numObjectRotations > 0)
				{
					modVbo[modID].rotX = new float[numTextures];
					modVbo[modID].rotY = new float[numTextures];
					modVbo[modID].rotZ = new float[numTextures];
					for (i = 0; i < numTextures; i++)
					{
						modVbo[modID].x[i] = binaryReader.ReadSingle();
						modVbo[modID].y[i] = binaryReader.ReadSingle();
						modVbo[modID].z[i] = binaryReader.ReadSingle();
						modVbo[modID].rotX[i] = binaryReader.ReadSingle();
						modVbo[modID].rotY[i] = binaryReader.ReadSingle();
						modVbo[modID].rotZ[i] = binaryReader.ReadSingle();
						modVbo[modID].dimX[i] = binaryReader.ReadInt16();
						modVbo[modID].dimY[i] = binaryReader.ReadInt16();
						modVbo[modID].dimZ[i] = binaryReader.ReadInt16();
					}
				}
				else
				{
					for (i = 0; i < numTextures; i++)
					{
						modVbo[modID].x[i] = binaryReader.ReadSingle();
						modVbo[modID].y[i] = binaryReader.ReadSingle();
						modVbo[modID].z[i] = binaryReader.ReadSingle();
						modVbo[modID].dimX[i] = binaryReader.ReadInt16();
						modVbo[modID].dimY[i] = binaryReader.ReadInt16();
						modVbo[modID].dimZ[i] = binaryReader.ReadInt16();
					}
				}
			}
			if (modVbo[modID].usesRigging == 0)
			{
				numTextures = modVbo[modID].vcount * 3 * 4;
				modVbo[modID].vertexBytes = new byte[numTextures];
				modVbo[modID].vertexBytes = binaryReader.ReadBytes(numTextures);
			}
			else
			{
				i = modVbo[modID].vcount;
				numTextures = i * 3 * 4;
				modVbo[modID].vertexBytes = new byte[numTextures];
				modVbo[modID].vertexBytes = binaryReader.ReadBytes(numTextures);
				modVbo[modID].blendIndex0 = new byte[i];
				modVbo[modID].blendIndex0 = binaryReader.ReadBytes(i);
			}
			numTextures = modVbo[modID].ncount * 3 * 4;
			modVbo[modID].normalBytes = new byte[numTextures];
			modVbo[modID].normalBytes = binaryReader.ReadBytes(numTextures);
			numTextures = modVbo[modID].tcount * 2 * 4;
			modVbo[modID].textureBytes = new byte[numTextures];
			modVbo[modID].textureBytes = binaryReader.ReadBytes(numTextures);
			i = modVbo[modID].pcount;
			numTextures = i * 3 * 4;
			i = i * 3 * 4;
			modVbo[modID].vIndexBytes = new byte[numTextures];
			modVbo[modID].nIndexBytes = new byte[numTextures];
			modVbo[modID].tIndexBytes = new byte[numTextures];
			modVbo[modID].tangentBytes = new byte[i];
			modVbo[modID].vIndexBytes = binaryReader.ReadBytes(numTextures);
			modVbo[modID].nIndexBytes = binaryReader.ReadBytes(numTextures);
			modVbo[modID].tIndexBytes = binaryReader.ReadBytes(numTextures);
			modVbo[modID].tangentBytes = binaryReader.ReadBytes(i);
			ConvertModelBuffersFromXboxEndian(ref modVbo[modID]);
		}
		stream.Close();
	}

	public void Load_Condensed_Level_Model_Version_1(Stream fp, BinaryReader br, int modID)
	{
		if (fp.CanRead)
		{
			br = new BinaryReader(fp);
			modVbo[modID].inLevelVBO = false;
			modVbo[modID].riggingStatus = 0;
			modVbo[modID].name = br.ReadString();
			modVbo[modID].texture = br.ReadString();
			modVbo[modID].texID = mainC.texturesMain.Find_Texture(modVbo[modID].texture, 0);
			modVbo[modID].textureNormal = br.ReadString();
			modVbo[modID].texNormalID = mainC.texturesMain.Find_Texture(modVbo[modID].textureNormal, 0);
			modVbo[modID].texSpecular = br.ReadString();
			modVbo[modID].texSpecularID = mainC.texturesMain.Find_Texture(modVbo[modID].texSpecular, 0);
			modVbo[modID].numTextures = br.ReadByte();
			int numTextures = modVbo[modID].numTextures;
			int i;
			if (numTextures > 0)
			{
				modVbo[modID].textureList = new int[numTextures];
				modVbo[modID].textureListStart = new int[numTextures];
				modVbo[modID].textureListNames = new string[numTextures];
				modVbo[modID].textureListPrimitiveCnt = new int[numTextures];
				modVbo[modID].textureListIndexCnt = new int[numTextures];
				for (i = 0; i < numTextures; i++)
				{
					modVbo[modID].textureListNames[i] = br.ReadString();
					modVbo[modID].textureList[i] = mainC.texturesMain.Find_Texture(modVbo[modID].textureListNames[i], 0);
					modVbo[modID].textureListStart[i] = br.ReadInt32();
					modVbo[modID].textureListPrimitiveCnt[i] = br.ReadInt32();
					modVbo[modID].textureListIndexCnt[i] = br.ReadInt32();
				}
				modVbo[modID].texID = modVbo[modID].textureList[0];
				modVbo[modID].texture = modVbo[modID].textureListNames[0];
			}
			modVbo[modID].vcount = br.ReadInt32();
			modVbo[modID].ncount = br.ReadInt32();
			modVbo[modID].tcount = br.ReadInt32();
			modVbo[modID].pcount = br.ReadInt32();
			modVbo[modID].usesAlpha = br.ReadBoolean();
			modVbo[modID].defaultColor[0] = br.ReadSingle();
			modVbo[modID].defaultColor[1] = br.ReadSingle();
			modVbo[modID].defaultColor[2] = br.ReadSingle();
			modVbo[modID].defaultColor[3] = br.ReadSingle();
			modVbo[modID].bufferType = br.ReadByte();
			modVbo[modID].instanceCount = br.ReadInt16();
			modVbo[modID].blendFunction = br.ReadByte();
			modVbo[modID].numObjects = br.ReadByte();
			modVbo[modID].numObjectRotations = br.ReadByte();
			modVbo[modID].usesRigging = br.ReadByte();
			numTextures = modVbo[modID].numObjects;
			if (numTextures > 0)
			{
				modVbo[modID].x = new float[numTextures];
				modVbo[modID].y = new float[numTextures];
				modVbo[modID].z = new float[numTextures];
				modVbo[modID].dimX = new short[numTextures];
				modVbo[modID].dimY = new short[numTextures];
				modVbo[modID].dimZ = new short[numTextures];
				if (modVbo[modID].numObjectRotations > 0)
				{
					modVbo[modID].rotX = new float[numTextures];
					modVbo[modID].rotY = new float[numTextures];
					modVbo[modID].rotZ = new float[numTextures];
					for (i = 0; i < numTextures; i++)
					{
						modVbo[modID].x[i] = br.ReadSingle();
						modVbo[modID].y[i] = br.ReadSingle();
						modVbo[modID].z[i] = br.ReadSingle();
						modVbo[modID].rotX[i] = br.ReadSingle();
						modVbo[modID].rotY[i] = br.ReadSingle();
						modVbo[modID].rotZ[i] = br.ReadSingle();
						modVbo[modID].dimX[i] = br.ReadInt16();
						modVbo[modID].dimY[i] = br.ReadInt16();
						modVbo[modID].dimZ[i] = br.ReadInt16();
					}
				}
				else
				{
					for (i = 0; i < numTextures; i++)
					{
						modVbo[modID].x[i] = br.ReadSingle();
						modVbo[modID].y[i] = br.ReadSingle();
						modVbo[modID].z[i] = br.ReadSingle();
						modVbo[modID].dimX[i] = br.ReadInt16();
						modVbo[modID].dimY[i] = br.ReadInt16();
						modVbo[modID].dimZ[i] = br.ReadInt16();
					}
				}
			}
			if (modVbo[modID].usesRigging == 0)
			{
				numTextures = modVbo[modID].vcount * 3 * 4;
				modVbo[modID].vertexBytes = new byte[numTextures];
				modVbo[modID].vertexBytes = br.ReadBytes(numTextures);
			}
			else
			{
				i = modVbo[modID].vcount;
				numTextures = i * 3 * 4;
				modVbo[modID].vertexBytes = new byte[numTextures];
				modVbo[modID].vertexBytes = br.ReadBytes(numTextures);
				modVbo[modID].blendIndex0 = new byte[i];
				modVbo[modID].blendIndex0 = br.ReadBytes(i);
			}
			numTextures = modVbo[modID].ncount * 3 * 4;
			modVbo[modID].normalBytes = new byte[numTextures];
			modVbo[modID].normalBytes = br.ReadBytes(numTextures);
			numTextures = modVbo[modID].tcount * 2 * 4;
			modVbo[modID].textureBytes = new byte[numTextures];
			modVbo[modID].textureBytes = br.ReadBytes(numTextures);
			i = modVbo[modID].pcount;
			numTextures = i * 3 * 4;
			i = i * 3 * 4;
			modVbo[modID].vIndexBytes = new byte[numTextures];
			modVbo[modID].nIndexBytes = new byte[numTextures];
			modVbo[modID].tIndexBytes = new byte[numTextures];
			modVbo[modID].tangentBytes = new byte[i];
			modVbo[modID].vIndexBytes = br.ReadBytes(numTextures);
			modVbo[modID].nIndexBytes = br.ReadBytes(numTextures);
			modVbo[modID].tIndexBytes = br.ReadBytes(numTextures);
			modVbo[modID].tangentBytes = br.ReadBytes(i);
			ConvertModelBuffersFromXboxEndian(ref modVbo[modID]);
		}
		fp.Close();
	}

	public void Set_Model_Textures()
	{
		for (short num = 0; num < numModels; num++)
		{
			mod1[num].texID = mainC.texturesMain.Find_Texture(mod1[num].texture, 0);
			mod1[num].texNormalID = mainC.texturesMain.Find_Texture(mod1[num].textureNormal, 0);
			short numTextures = mod1[num].numTextures;
			for (short num2 = 0; num2 < numTextures; num2++)
			{
				mod1[num].textureList[num2] = mainC.texturesMain.Find_Texture(mod1[num].textureListNames[num2], 0);
			}
		}
	}

	public void Calculate_Tangents(long modID, long pID)
	{
		ctV1 = tempModel.v1[tempModel.p1[pID].v[0]];
		ctV2 = tempModel.v1[tempModel.p1[pID].v[1]];
		ctV3 = tempModel.v1[tempModel.p1[pID].v[2]];
		ctT1 = tempModel.t1[tempModel.p1[pID].t[0]];
		ctT2 = tempModel.t1[tempModel.p1[pID].t[1]];
		ctT3 = tempModel.t1[tempModel.p1[pID].t[2]];
		mainC.utilMain.Calc_Tangent(ref ctV1, ref ctV2, ref ctV3, ref ctT1, ref ctT2, ref ctT3, ref tempModel.p1[pID].tangent);
	}

	public void Calculate_Tangents2(long modID, long pID)
	{
		ctV1 = tempModel.v1[modVbo[modID].p1[pID].v[0]];
		ctV2 = tempModel.v1[modVbo[modID].p1[pID].v[1]];
		ctV3 = tempModel.v1[modVbo[modID].p1[pID].v[2]];
		ctT1 = tempModel.t1[modVbo[modID].p1[pID].t[0]];
		ctT2 = tempModel.t1[modVbo[modID].p1[pID].t[1]];
		ctT3 = tempModel.t1[modVbo[modID].p1[pID].t[2]];
		mainC.utilMain.Calc_Tangent(ref ctV1, ref ctV2, ref ctV3, ref ctT1, ref ctT2, ref ctT3, ref tempModel.p1[pID].tangent);
	}

	public void Calculate_Tangents_For_tempModel(long pID)
	{
		ctV1 = tempModel.v1[tempModel.p1[pID].v[0]];
		ctV2 = tempModel.v1[tempModel.p1[pID].v[1]];
		ctV3 = tempModel.v1[tempModel.p1[pID].v[2]];
		ctT1 = tempModel.t1[tempModel.p1[pID].t[0]];
		ctT2 = tempModel.t1[tempModel.p1[pID].t[1]];
		ctT3 = tempModel.t1[tempModel.p1[pID].t[2]];
		mainC.utilMain.Calc_Tangent(ref ctV1, ref ctV2, ref ctV3, ref ctT1, ref ctT2, ref ctT3, ref tempModel.p1[pID].tangent);
	}

	public ushort Find_Model(string str1)
	{
		for (ushort num = 0; num < numModels; num++)
		{
			if (mod1[num].name.Equals(str1, StringComparison.OrdinalIgnoreCase))
			{
				return num;
			}
		}
		return 0;
	}

	public ushort Find_Model_If_Exists(string str1)
	{
		for (ushort num = 0; num < numModels; num++)
		{
			if (mod1[num].name.Equals(str1, StringComparison.OrdinalIgnoreCase))
			{
				return num;
			}
		}
		return global::Util.Util.maxUnsignedShortValue;
	}

	public short Find_Level_Model(string str1)
	{
		for (short num = 0; num < numLevelModels; num++)
		{
			if (modVbo[num].name.Equals(str1, StringComparison.OrdinalIgnoreCase))
			{
				return num;
			}
		}
		return 0;
	}

	public unsafe void Update_Model_For_Rigging(byte jointID, short jointColID, short modelID)
	{
		_ = global::Rendering.Rendering.uBufferID;
		float num = 0f;
		float num2 = 0f;
		float num3 = 0f;
		mod1[modelID].riggingStatus = 1;
		mod2 = mod1[modelID];
		if (mod2.usesRigging < 1 || mod1[modelID].riggingStatus == 1)
		{
			return;
		}
		int vcount = mod2.vcount;
		fixed (byte* vertexBytes = mod2.vertexBytes)
		{
			fixed (byte* bwBytes = mod2.bwBytes0)
			{
				fixed (byte* bwBytes2 = mod2.bwBytes1)
				{
					fixed (byte* bwBytes3 = mod2.bwBytes2)
					{
						fixed (byte* bwBytes4 = mod2.bwBytes3)
						{
							float* ptr = (float*)vertexBytes;
							int num4 = 0;
							int num5 = 0;
							while (num4 < vcount)
							{
								jointID = mod2.blendIndex0[num4];
								num = global::Joints.Joints.playerJoints[jointColID].jt1[jointID].mv[0].M41;
								num2 = global::Joints.Joints.playerJoints[jointColID].jt1[jointID].mv[0].M42;
								num3 = global::Joints.Joints.playerJoints[jointColID].jt1[jointID].mv[0].M43;
								ptr[num5] -= num;
								(ptr + num5)[1] -= num2;
								(ptr + num5)[2] -= num3;
								num4++;
								num5 += 3;
							}
						}
					}
				}
			}
		}
		mod1[modelID].riggingStatus = 1;
	}

	public void Get_Textures_Used_By_Models(string[] modelList, byte vboList, ref string[] textureList)
	{
		int num = 0;
		string[] array = new string[1] { "" };
		switch (vboList)
		{
		case 0:
		{
			int num2 = modelList.Length;
			for (int i = 0; i < num2; i++)
			{
				int num3 = Find_Model(modelList[i]);
				num += mod1[num3].numTextures;
				if (mod1[num3].textureNormal != "")
				{
					num++;
				}
			}
			array = new string[num];
			int num4 = 0;
			for (int i = 0; i < num2; i++)
			{
				int num3 = Find_Model(modelList[i]);
				int numTextures = mod1[num3].numTextures;
				for (int j = 0; j < numTextures; j++)
				{
					array[num4++] = mod1[num3].textureListNames[j];
				}
				if (mod1[num3].textureNormal != "")
				{
					array[num4++] = modVbo[num3].textureNormal;
				}
			}
			break;
		}
		case 1:
		{
			int num2 = modelList.Length;
			for (int i = 0; i < num2; i++)
			{
				int num3 = Find_Level_Model(modelList[i]);
				num += modVbo[num3].numTextures;
				if (modVbo[num3].textureNormal != "")
				{
					num++;
				}
			}
			array = new string[num];
			int num4 = 0;
			for (int i = 0; i < num2; i++)
			{
				int num3 = Find_Level_Model(modelList[i]);
				int numTextures = modVbo[num3].numTextures;
				for (int j = 0; j < numTextures; j++)
				{
					array[num4++] = modVbo[num3].textureListNames[j];
				}
				if (modVbo[num3].textureNormal != "")
				{
					array[num4++] = modVbo[num3].textureNormal;
				}
			}
			break;
		}
		}
		textureList = array;
	}

	public void Get_Textures_Used_By_Model_VBO(byte vboList, ref string[] textureList)
	{
		int num = 0;
		string[] array = new string[1] { "" };
		switch (vboList)
		{
		case 0:
		{
			for (int i = 0; i < numModels; i++)
			{
				num += mod1[i].numTextures;
				if (mod1[i].textureNormal != "")
				{
					num++;
				}
			}
			array = new string[num];
			int num2 = 0;
			for (int i = 0; i < numModels; i++)
			{
				int numTextures = mod1[i].numTextures;
				for (int j = 0; j < numTextures; j++)
				{
					array[num2++] = mod1[i].textureListNames[j];
				}
				if (mod1[i].textureNormal != "")
				{
					array[num2++] = mod1[i].textureNormal;
				}
			}
			break;
		}
		case 1:
		{
			for (int i = 0; i < numLevelModels; i++)
			{
				num += modVbo[i].numTextures;
				if (modVbo[i].textureNormal != "")
				{
					num++;
				}
			}
			array = new string[num];
			int num2 = 0;
			for (int i = 0; i < numLevelModels; i++)
			{
				int numTextures = modVbo[i].numTextures;
				for (int j = 0; j < numTextures; j++)
				{
					array[num2++] = modVbo[i].textureListNames[j];
				}
				if (modVbo[i].textureNormal != "")
				{
					array[num2++] = modVbo[i].textureNormal;
				}
			}
			break;
		}
		}
		textureList = array;
	}

	public bool Level_Model_Exists(string str1)
	{
		for (short num = 0; num < numLevelModels; num++)
		{
			if (modVbo[num].name.Equals(str1, StringComparison.OrdinalIgnoreCase))
			{
				return true;
			}
		}
		return false;
	}

	public unsafe void Create_Main_Model_VBO()
	{
		int num = 0;
		int[] array = new int[1];
		int num2 = 0;
		int num3 = 0;
		for (long num4 = 0L; num4 < numModels; num4++)
		{
			num2 += mod1[num4].vcount;
			num3 += mod1[num4].pcount;
		}
		num3 *= 3;
		mVtexObjects = new StructsClass.VertexPositionColorNormalTexture[num2];
		mViObjects = new int[num3];
		int num5 = 0;
		int ibStart = 0;
		for (long num4 = 0L; num4 < numModels; num4++)
		{
			fixed (byte* vertexBytes = mod1[num4].vertexBytes)
			{
				fixed (byte* normalBytes = mod1[num4].normalBytes)
				{
					fixed (byte* textureBytes = mod1[num4].textureBytes)
					{
						fixed (byte* vIndexBytes = mod1[num4].vIndexBytes)
						{
							fixed (byte* nIndexBytes = mod1[num4].nIndexBytes)
							{
								fixed (byte* tIndexBytes = mod1[num4].tIndexBytes)
								{
									fixed (byte* tangentBytes = mod1[num4].tangentBytes)
									{
										fixed (byte* bwBytes = mod1[num4].bwBytes0)
										{
											fixed (byte* bwBytes2 = mod1[num4].bwBytes1)
											{
												fixed (byte* bwBytes3 = mod1[num4].bwBytes2)
												{
													fixed (byte* bwBytes4 = mod1[num4].bwBytes3)
													{
														float* ptr = (float*)vertexBytes;
														float* ptr2 = (float*)normalBytes;
														float* ptr3 = (float*)textureBytes;
														int* ptr4 = (int*)vIndexBytes;
														int* ptr5 = (int*)nIndexBytes;
														int* ptr6 = (int*)tIndexBytes;
														float* ptr7 = (float*)tangentBytes;
														mod1[num4].vbStart = num5;
														mod1[num4].ibStart = ibStart;
														if (mod1[num4].usesRigging == 0)
														{
															for (long num6 = 0L; num6 < mod1[num4].pcount; num6++)
															{
																long num7 = num6 * 3;
																int num8 = ptr4[num7];
																int num9 = num8 * 3;
																int num10 = ptr5[num7] * 3;
																int num11 = ptr6[num7] * 2;
																int num12 = num8 + num5;
																if (num12 < 0 || num12 >= mVtexObjects.Length || mod1[num4].defaultColor == null || mod1[num4].defaultColor.Length < 4)
																{
																	throw new InvalidDataException($"Invalid main-model vertex data: model={num4} name={mod1[num4].name}, triangle={num6}, localVertex={num8}, vertexBase={num5}, destination={num12}/{mVtexObjects.Length}, vcount={mod1[num4].vcount}, ncount={mod1[num4].ncount}, tcount={mod1[num4].tcount}, pcount={mod1[num4].pcount}, defaultColorLength={mod1[num4].defaultColor?.Length ?? 0}.");
																}
																mVtexObjects[num12].Set_Values(ptr[num9], (ptr + num9)[1], (ptr + num9)[2], ptr2[num10], (ptr2 + num10)[1], (ptr2 + num10)[2], ptr7[num7], (ptr7 + num7)[1], (ptr7 + num7)[2], ptr3[num11], (ptr3 + num11)[1], mod1[num4].defaultColor[0], mod1[num4].defaultColor[1], mod1[num4].defaultColor[2], mod1[num4].defaultColor[3], 0, 0, 0, 0, 1f, 0f, 0f, 0f);
																mViObjects[ibStart++] = num12;
																num8 = (ptr4 + num7)[1];
																num9 = num8 * 3;
																num10 = (ptr5 + num7)[1] * 3;
																num11 = (ptr6 + num7)[1] * 2;
																num12 = num8 + num5;
																mVtexObjects[num12].Set_Values(ptr[num9], (ptr + num9)[1], (ptr + num9)[2], ptr2[num10], (ptr2 + num10)[1], (ptr2 + num10)[2], ptr7[num7], (ptr7 + num7)[1], (ptr7 + num7)[2], ptr3[num11], (ptr3 + num11)[1], mod1[num4].defaultColor[0], mod1[num4].defaultColor[1], mod1[num4].defaultColor[2], mod1[num4].defaultColor[3], 0, 0, 0, 0, 1f, 0f, 0f, 0f);
																mViObjects[ibStart++] = num12;
																num8 = (ptr4 + num7)[2];
																num9 = num8 * 3;
																num10 = (ptr5 + num7)[2] * 3;
																num11 = (ptr6 + num7)[2] * 2;
																num12 = num8 + num5;
																mVtexObjects[num12].Set_Values(ptr[num9], (ptr + num9)[1], (ptr + num9)[2], ptr2[num10], (ptr2 + num10)[1], (ptr2 + num10)[2], ptr7[num7], (ptr7 + num7)[1], (ptr7 + num7)[2], ptr3[num11], (ptr3 + num11)[1], mod1[num4].defaultColor[0], mod1[num4].defaultColor[1], mod1[num4].defaultColor[2], mod1[num4].defaultColor[3], 0, 0, 0, 0, 1f, 0f, 0f, 0f);
																mViObjects[ibStart++] = num12;
															}
														}
														else
														{
															float* ptr8 = (float*)bwBytes;
															float* ptr9 = (float*)bwBytes2;
															float* ptr10 = (float*)bwBytes3;
															float* ptr11 = (float*)bwBytes4;
															for (long num6 = 0L; num6 < mod1[num4].pcount; num6++)
															{
																long num7 = num6 * 3;
																int num8 = ptr4[num7];
																int num9 = num8 * 3;
																int num10 = ptr5[num7] * 3;
																int num11 = ptr6[num7] * 2;
																int num12 = num8 + num5;
																mVtexObjects[num12].Set_Values(ptr[num9], (ptr + num9)[1], (ptr + num9)[2], ptr2[num10], (ptr2 + num10)[1], (ptr2 + num10)[2], ptr7[num7], (ptr7 + num7)[1], (ptr7 + num7)[2], ptr3[num11], (ptr3 + num11)[1], mod1[num4].defaultColor[0], mod1[num4].defaultColor[1], mod1[num4].defaultColor[2], mod1[num4].defaultColor[3], mod1[num4].blendIndex0[num8], mod1[num4].blendIndex1[num8], mod1[num4].blendIndex2[num8], mod1[num4].blendIndex3[num8], ptr8[num8], ptr9[num8], ptr10[num8], ptr11[num8]);
																mViObjects[ibStart++] = num12;
																num8 = (ptr4 + num7)[1];
																num9 = num8 * 3;
																num10 = (ptr5 + num7)[1] * 3;
																num11 = (ptr6 + num7)[1] * 2;
																num12 = num8 + num5;
																mVtexObjects[num12].Set_Values(ptr[num9], (ptr + num9)[1], (ptr + num9)[2], ptr2[num10], (ptr2 + num10)[1], (ptr2 + num10)[2], ptr7[num7], (ptr7 + num7)[1], (ptr7 + num7)[2], ptr3[num11], (ptr3 + num11)[1], mod1[num4].defaultColor[0], mod1[num4].defaultColor[1], mod1[num4].defaultColor[2], mod1[num4].defaultColor[3], mod1[num4].blendIndex0[num8], mod1[num4].blendIndex1[num8], mod1[num4].blendIndex2[num8], mod1[num4].blendIndex3[num8], ptr8[num8], ptr9[num8], ptr10[num8], ptr11[num8]);
																mViObjects[ibStart++] = num12;
																num8 = (ptr4 + num7)[2];
																num9 = num8 * 3;
																num10 = (ptr5 + num7)[2] * 3;
																num11 = (ptr6 + num7)[2] * 2;
																num12 = num8 + num5;
																mVtexObjects[num12].Set_Values(ptr[num9], (ptr + num9)[1], (ptr + num9)[2], ptr2[num10], (ptr2 + num10)[1], (ptr2 + num10)[2], ptr7[num7], (ptr7 + num7)[1], (ptr7 + num7)[2], ptr3[num11], (ptr3 + num11)[1], mod1[num4].defaultColor[0], mod1[num4].defaultColor[1], mod1[num4].defaultColor[2], mod1[num4].defaultColor[3], mod1[num4].blendIndex0[num8], mod1[num4].blendIndex1[num8], mod1[num4].blendIndex2[num8], mod1[num4].blendIndex3[num8], ptr8[num8], ptr9[num8], ptr10[num8], ptr11[num8]);
																mViObjects[ibStart++] = num12;
															}
														}
														num5 += mod1[num4].vcount;
													}
												}
											}
										}
									}
								}
							}
						}
					}
				}
			}
		}
		_ = StructsClass.VertexPositionColorNormalTexture.SizeInBytes;
		mVertexBufferObjects = new VertexBuffer(mGraphics, global::Rendering.Rendering.rDecVPCNT, num2, BufferUsage.WriteOnly);
		mVertexBufferObjects.SetData(mVtexObjects);
		mIndexBufferObjects = new IndexBuffer(mGraphics, typeof(int), num3, BufferUsage.None);
		mIndexBufferObjects.SetData(mViObjects);
		for (long num4 = 0L; num4 < numModels; num4++)
		{
			if (mod1[num4].instanceCount <= 0)
			{
				continue;
			}
			int num13 = mod1[num4].pcount * 3;
			int instanceCount = mod1[num4].instanceCount;
			int num14 = num13 * instanceCount;
			if (num14 != num)
			{
				num = num14;
				array = new int[num];
			}
			num14 = mod1[num4].ibStart;
			int i = 0;
			int num15 = 0;
			for (; i < instanceCount; i++)
			{
				int num16 = i * num13;
				for (int j = 0; j < num13; j++)
				{
					array[num15++] = mViObjects[num14 + j] + num16;
				}
			}
			mod1[num4].mInstanceIndex = new IndexBuffer(mGraphics, typeof(int), num, BufferUsage.None);
			mod1[num4].mInstanceIndex.SetData(array);
			mod1[num4].indexBufferSize = num;
		}
		mainC.maingameMain.Save_Buffer_Config_Data(1, "Models.txt");
	}

	public unsafe void Create_Level_Model_VBO_Initial()
	{
		mVtexObjectsLevel = null;
		mViObjectsLevel = null;
		GC.Collect();
		int num = 0;
		int num2 = 0;
		for (long num3 = 0L; num3 < numLevelModels; num3++)
		{
			num += modVbo[num3].vcount;
			num2 += modVbo[num3].pcount;
		}
		num2 *= 3;
		mVtexObjectsLevel = new StructsClass.VertexPositionColorNormalTexture[num];
		mViObjectsLevel = new int[num2];
		int num4 = 0;
		int ibStart = 0;
		for (long num3 = 0L; num3 < numLevelModels; num3++)
		{
			if (!modVbo[num3].inLevelVBO)
			{
				continue;
			}
			fixed (byte* vertexBytes = modVbo[num3].vertexBytes)
			{
				fixed (byte* normalBytes = modVbo[num3].normalBytes)
				{
					fixed (byte* textureBytes = modVbo[num3].textureBytes)
					{
						fixed (byte* vIndexBytes = modVbo[num3].vIndexBytes)
						{
							fixed (byte* nIndexBytes = modVbo[num3].nIndexBytes)
							{
								fixed (byte* tIndexBytes = modVbo[num3].tIndexBytes)
								{
									fixed (byte* tangentBytes = modVbo[num3].tangentBytes)
									{
										fixed (byte* bwBytes = modVbo[num3].bwBytes0)
										{
											fixed (byte* bwBytes2 = modVbo[num3].bwBytes1)
											{
												fixed (byte* bwBytes3 = modVbo[num3].bwBytes2)
												{
													fixed (byte* bwBytes4 = modVbo[num3].bwBytes3)
													{
														float* ptr = (float*)vertexBytes;
														float* ptr2 = (float*)normalBytes;
														float* ptr3 = (float*)textureBytes;
														int* ptr4 = (int*)vIndexBytes;
														int* ptr5 = (int*)nIndexBytes;
														int* ptr6 = (int*)tIndexBytes;
														float* ptr7 = (float*)tangentBytes;
														modVbo[num3].vbStart = num4;
														modVbo[num3].ibStart = ibStart;
														if (modVbo[num3].usesRigging == 0)
														{
															for (long num5 = 0L; num5 < modVbo[num3].pcount; num5++)
															{
																long num6 = num5 * 3;
																int num7 = ptr4[num6];
																int num8 = num7 * 3;
																int num9 = ptr5[num6] * 3;
																int num10 = ptr6[num6] * 2;
																int num11 = num7 + num4;
																mVtexObjectsLevel[num11].Set_Values(ptr[num8], (ptr + num8)[1], (ptr + num8)[2], ptr2[num9], (ptr2 + num9)[1], (ptr2 + num9)[2], ptr7[num6], (ptr7 + num6)[1], (ptr7 + num6)[2], ptr3[num10], (ptr3 + num10)[1], modVbo[num3].defaultColor[0], modVbo[num3].defaultColor[1], modVbo[num3].defaultColor[2], modVbo[num3].defaultColor[3], 0, 0, 0, 0, 1f, 0f, 0f, 0f);
																mViObjectsLevel[ibStart++] = num11;
																num7 = (ptr4 + num6)[1];
																num8 = num7 * 3;
																num9 = (ptr5 + num6)[1] * 3;
																num10 = (ptr6 + num6)[1] * 2;
																num11 = num7 + num4;
																mVtexObjectsLevel[num11].Set_Values(ptr[num8], (ptr + num8)[1], (ptr + num8)[2], ptr2[num9], (ptr2 + num9)[1], (ptr2 + num9)[2], ptr7[num6], (ptr7 + num6)[1], (ptr7 + num6)[2], ptr3[num10], (ptr3 + num10)[1], modVbo[num3].defaultColor[0], modVbo[num3].defaultColor[1], modVbo[num3].defaultColor[2], modVbo[num3].defaultColor[3], 0, 0, 0, 0, 1f, 0f, 0f, 0f);
																mViObjectsLevel[ibStart++] = num11;
																num7 = (ptr4 + num6)[2];
																num8 = num7 * 3;
																num9 = (ptr5 + num6)[2] * 3;
																num10 = (ptr6 + num6)[2] * 2;
																num11 = num7 + num4;
																mVtexObjectsLevel[num11].Set_Values(ptr[num8], (ptr + num8)[1], (ptr + num8)[2], ptr2[num9], (ptr2 + num9)[1], (ptr2 + num9)[2], ptr7[num6], (ptr7 + num6)[1], (ptr7 + num6)[2], ptr3[num10], (ptr3 + num10)[1], modVbo[num3].defaultColor[0], modVbo[num3].defaultColor[1], modVbo[num3].defaultColor[2], modVbo[num3].defaultColor[3], 0, 0, 0, 0, 1f, 0f, 0f, 0f);
																mViObjectsLevel[ibStart++] = num11;
															}
														}
														else
														{
															float* ptr8 = (float*)bwBytes;
															float* ptr9 = (float*)bwBytes2;
															float* ptr10 = (float*)bwBytes3;
															float* ptr11 = (float*)bwBytes4;
															for (long num5 = 0L; num5 < modVbo[num3].pcount; num5++)
															{
																long num6 = num5 * 3;
																int num7 = ptr4[num6];
																int num8 = num7 * 3;
																int num9 = ptr5[num6] * 3;
																int num10 = ptr6[num6] * 2;
																int num11 = num7 + num4;
																mVtexObjectsLevel[num11].Set_Values(ptr[num8], (ptr + num8)[1], (ptr + num8)[2], ptr2[num9], (ptr2 + num9)[1], (ptr2 + num9)[2], ptr7[num6], (ptr7 + num6)[1], (ptr7 + num6)[2], ptr3[num10], (ptr3 + num10)[1], modVbo[num3].defaultColor[0], modVbo[num3].defaultColor[1], modVbo[num3].defaultColor[2], modVbo[num3].defaultColor[3], modVbo[num3].blendIndex0[num7], modVbo[num3].blendIndex1[num7], modVbo[num3].blendIndex3[num7], modVbo[num3].blendIndex2[num7], ptr8[num7], ptr9[num7], ptr10[num7], ptr11[num7]);
																mViObjectsLevel[ibStart++] = num11;
																num7 = (ptr4 + num6)[1];
																num8 = num7 * 3;
																num9 = (ptr5 + num6)[1] * 3;
																num10 = (ptr6 + num6)[1] * 2;
																num11 = num7 + num4;
																mVtexObjectsLevel[num11].Set_Values(ptr[num8], (ptr + num8)[1], (ptr + num8)[2], ptr2[num9], (ptr2 + num9)[1], (ptr2 + num9)[2], ptr7[num6], (ptr7 + num6)[1], (ptr7 + num6)[2], ptr3[num10], (ptr3 + num10)[1], modVbo[num3].defaultColor[0], modVbo[num3].defaultColor[1], modVbo[num3].defaultColor[2], modVbo[num3].defaultColor[3], modVbo[num3].blendIndex0[num7], modVbo[num3].blendIndex1[num7], modVbo[num3].blendIndex3[num7], modVbo[num3].blendIndex2[num7], ptr8[num7], ptr9[num7], ptr10[num7], ptr11[num7]);
																mViObjectsLevel[ibStart++] = num11;
																num7 = (ptr4 + num6)[2];
																num8 = num7 * 3;
																num9 = (ptr5 + num6)[2] * 3;
																num10 = (ptr6 + num6)[2] * 2;
																num11 = num7 + num4;
																mVtexObjectsLevel[num11].Set_Values(ptr[num8], (ptr + num8)[1], (ptr + num8)[2], ptr2[num9], (ptr2 + num9)[1], (ptr2 + num9)[2], ptr7[num6], (ptr7 + num6)[1], (ptr7 + num6)[2], ptr3[num10], (ptr3 + num10)[1], modVbo[num3].defaultColor[0], modVbo[num3].defaultColor[1], modVbo[num3].defaultColor[2], modVbo[num3].defaultColor[3], modVbo[num3].blendIndex0[num7], modVbo[num3].blendIndex1[num7], modVbo[num3].blendIndex3[num7], modVbo[num3].blendIndex2[num7], ptr8[num7], ptr9[num7], ptr10[num7], ptr11[num7]);
																mViObjectsLevel[ibStart++] = num11;
															}
														}
														num4 += modVbo[num3].vcount;
													}
												}
											}
										}
									}
								}
							}
						}
					}
				}
			}
		}
		for (long num3 = 0L; num3 < numLevelModels; num3++)
		{
			if (modVbo[num3].inLevelVBO)
			{
				continue;
			}
			fixed (byte* vertexBytes2 = modVbo[num3].vertexBytes)
			{
				fixed (byte* normalBytes2 = modVbo[num3].normalBytes)
				{
					fixed (byte* textureBytes2 = modVbo[num3].textureBytes)
					{
						fixed (byte* vIndexBytes2 = modVbo[num3].vIndexBytes)
						{
							fixed (byte* nIndexBytes2 = modVbo[num3].nIndexBytes)
							{
								fixed (byte* tIndexBytes2 = modVbo[num3].tIndexBytes)
								{
									fixed (byte* tangentBytes2 = modVbo[num3].tangentBytes)
									{
										fixed (byte* bwBytes5 = modVbo[num3].bwBytes0)
										{
											fixed (byte* bwBytes6 = modVbo[num3].bwBytes1)
											{
												fixed (byte* bwBytes7 = modVbo[num3].bwBytes2)
												{
													fixed (byte* bwBytes8 = modVbo[num3].bwBytes3)
													{
														float* ptr = (float*)vertexBytes2;
														float* ptr2 = (float*)normalBytes2;
														float* ptr3 = (float*)textureBytes2;
														int* ptr4 = (int*)vIndexBytes2;
														int* ptr5 = (int*)nIndexBytes2;
														int* ptr6 = (int*)tIndexBytes2;
														float* ptr7 = (float*)tangentBytes2;
														modVbo[num3].vbStart = num4;
														modVbo[num3].ibStart = ibStart;
														if (modVbo[num3].usesRigging == 0)
														{
															for (long num5 = 0L; num5 < modVbo[num3].pcount; num5++)
															{
																long num6 = num5 * 3;
																int num7 = ptr4[num6];
																int num8 = num7 * 3;
																int num9 = ptr5[num6] * 3;
																int num10 = ptr6[num6] * 2;
																int num11 = num7 + num4;
																mVtexObjectsLevel[num11].Set_Values(ptr[num8], (ptr + num8)[1], (ptr + num8)[2], ptr2[num9], (ptr2 + num9)[1], (ptr2 + num9)[2], ptr7[num6], (ptr7 + num6)[1], (ptr7 + num6)[2], ptr3[num10], (ptr3 + num10)[1], modVbo[num3].defaultColor[0], modVbo[num3].defaultColor[1], modVbo[num3].defaultColor[2], modVbo[num3].defaultColor[3], 0, 0, 0, 0, 1f, 0f, 0f, 0f);
																mViObjectsLevel[ibStart++] = num11;
																num7 = (ptr4 + num6)[1];
																num8 = num7 * 3;
																num9 = (ptr5 + num6)[1] * 3;
																num10 = (ptr6 + num6)[1] * 2;
																num11 = num7 + num4;
																mVtexObjectsLevel[num11].Set_Values(ptr[num8], (ptr + num8)[1], (ptr + num8)[2], ptr2[num9], (ptr2 + num9)[1], (ptr2 + num9)[2], ptr7[num6], (ptr7 + num6)[1], (ptr7 + num6)[2], ptr3[num10], (ptr3 + num10)[1], modVbo[num3].defaultColor[0], modVbo[num3].defaultColor[1], modVbo[num3].defaultColor[2], modVbo[num3].defaultColor[3], 0, 0, 0, 0, 1f, 0f, 0f, 0f);
																mViObjectsLevel[ibStart++] = num11;
																num7 = (ptr4 + num6)[2];
																num8 = num7 * 3;
																num9 = (ptr5 + num6)[2] * 3;
																num10 = (ptr6 + num6)[2] * 2;
																num11 = num7 + num4;
																mVtexObjectsLevel[num11].Set_Values(ptr[num8], (ptr + num8)[1], (ptr + num8)[2], ptr2[num9], (ptr2 + num9)[1], (ptr2 + num9)[2], ptr7[num6], (ptr7 + num6)[1], (ptr7 + num6)[2], ptr3[num10], (ptr3 + num10)[1], modVbo[num3].defaultColor[0], modVbo[num3].defaultColor[1], modVbo[num3].defaultColor[2], modVbo[num3].defaultColor[3], 0, 0, 0, 0, 1f, 0f, 0f, 0f);
																mViObjectsLevel[ibStart++] = num11;
															}
														}
														else
														{
															float* ptr8 = (float*)bwBytes5;
															float* ptr9 = (float*)bwBytes6;
															float* ptr10 = (float*)bwBytes7;
															float* ptr11 = (float*)bwBytes8;
															for (long num5 = 0L; num5 < modVbo[num3].pcount; num5++)
															{
																long num6 = num5 * 3;
																int num7 = ptr4[num6];
																int num8 = num7 * 3;
																int num9 = ptr5[num6] * 3;
																int num10 = ptr6[num6] * 2;
																int num11 = num7 + num4;
																mVtexObjectsLevel[num11].Set_Values(ptr[num8], (ptr + num8)[1], (ptr + num8)[2], ptr2[num9], (ptr2 + num9)[1], (ptr2 + num9)[2], ptr7[num6], (ptr7 + num6)[1], (ptr7 + num6)[2], ptr3[num10], (ptr3 + num10)[1], modVbo[num3].defaultColor[0], modVbo[num3].defaultColor[1], modVbo[num3].defaultColor[2], modVbo[num3].defaultColor[3], modVbo[num3].blendIndex0[num7], modVbo[num3].blendIndex1[num7], modVbo[num3].blendIndex2[num7], modVbo[num3].blendIndex3[num7], ptr8[num7], ptr9[num7], ptr10[num7], ptr11[num7]);
																mViObjectsLevel[ibStart++] = num11;
																num7 = (ptr4 + num6)[1];
																num8 = num7 * 3;
																num9 = (ptr5 + num6)[1] * 3;
																num10 = (ptr6 + num6)[1] * 2;
																num11 = num7 + num4;
																mVtexObjectsLevel[num11].Set_Values(ptr[num8], (ptr + num8)[1], (ptr + num8)[2], ptr2[num9], (ptr2 + num9)[1], (ptr2 + num9)[2], ptr7[num6], (ptr7 + num6)[1], (ptr7 + num6)[2], ptr3[num10], (ptr3 + num10)[1], modVbo[num3].defaultColor[0], modVbo[num3].defaultColor[1], modVbo[num3].defaultColor[2], modVbo[num3].defaultColor[3], modVbo[num3].blendIndex0[num7], modVbo[num3].blendIndex1[num7], modVbo[num3].blendIndex2[num7], modVbo[num3].blendIndex3[num7], ptr8[num7], ptr9[num7], ptr10[num7], ptr11[num7]);
																mViObjectsLevel[ibStart++] = num11;
																num7 = (ptr4 + num6)[2];
																num8 = num7 * 3;
																num9 = (ptr5 + num6)[2] * 3;
																num10 = (ptr6 + num6)[2] * 2;
																num11 = num7 + num4;
																mVtexObjectsLevel[num11].Set_Values(ptr[num8], (ptr + num8)[1], (ptr + num8)[2], ptr2[num9], (ptr2 + num9)[1], (ptr2 + num9)[2], ptr7[num6], (ptr7 + num6)[1], (ptr7 + num6)[2], ptr3[num10], (ptr3 + num10)[1], modVbo[num3].defaultColor[0], modVbo[num3].defaultColor[1], modVbo[num3].defaultColor[2], modVbo[num3].defaultColor[3], modVbo[num3].blendIndex0[num7], modVbo[num3].blendIndex1[num7], modVbo[num3].blendIndex2[num7], modVbo[num3].blendIndex3[num7], ptr8[num7], ptr9[num7], ptr10[num7], ptr11[num7]);
																mViObjectsLevel[ibStart++] = num11;
															}
														}
														num4 += modVbo[num3].vcount;
													}
												}
											}
										}
									}
								}
							}
						}
					}
				}
			}
		}
	}

	public void Free_Level_VBO()
	{
		try
		{
			if (mVertexBufferObjectsLevel != null)
			{
				mVertexBufferObjectsLevel.Dispose();
				mVertexBufferObjectsLevel = null;
			}
		}
		catch (Exception)
		{
		}
		try
		{
			if (mIndexBufferObjectsLevel != null)
			{
				mIndexBufferObjectsLevel.Dispose();
				mIndexBufferObjectsLevel = null;
			}
		}
		catch (Exception)
		{
		}
	}

	public unsafe void Create_Level_Model_VBO_Final()
	{
		byte[] array = new byte[1];
		int[] array2 = new int[1];
		int num = 0;
		mVtexObjectsLevel = null;
		mViObjectsLevel = null;
		try
		{
			if (mVertexBufferObjectsLevel != null)
			{
				mVertexBufferObjectsLevel.Dispose();
				mVertexBufferObjectsLevel = null;
			}
		}
		catch (Exception)
		{
		}
		try
		{
			if (mIndexBufferObjectsLevel != null)
			{
				mIndexBufferObjectsLevel.Dispose();
				mIndexBufferObjectsLevel = null;
			}
		}
		catch (Exception)
		{
		}
		GC.Collect();
		int num2 = 0;
		int num3 = 0;
		for (long num4 = 0L; num4 < numLevelModels; num4++)
		{
			if (modVbo[num4].inLevelVBO)
			{
				num2 += modVbo[num4].vcount;
				num3 += modVbo[num4].pcount;
			}
		}
		num3 *= 3;
		if (num3 < 1)
		{
			return;
		}
		mVtexObjectsLevel = new StructsClass.VertexPositionColorNormalTexture[num2];
		mViObjectsLevel = new int[num3];
		int num5 = 0;
		int ibStart = 0;
		for (long num4 = 0L; num4 < numLevelModels; num4++)
		{
			if (!modVbo[num4].inLevelVBO)
			{
				continue;
			}
			fixed (byte* vertexBytes = modVbo[num4].vertexBytes)
			{
				fixed (byte* normalBytes = modVbo[num4].normalBytes)
				{
					fixed (byte* textureBytes = modVbo[num4].textureBytes)
					{
						fixed (byte* vIndexBytes = modVbo[num4].vIndexBytes)
						{
							fixed (byte* nIndexBytes = modVbo[num4].nIndexBytes)
							{
								fixed (byte* tIndexBytes = modVbo[num4].tIndexBytes)
								{
									fixed (byte* tangentBytes = modVbo[num4].tangentBytes)
									{
										fixed (byte* bwBytes = modVbo[num4].bwBytes0)
										{
											fixed (byte* bwBytes2 = modVbo[num4].bwBytes1)
											{
												fixed (byte* bwBytes3 = modVbo[num4].bwBytes2)
												{
													fixed (byte* bwBytes4 = modVbo[num4].bwBytes3)
													{
														float* ptr = (float*)vertexBytes;
														float* ptr2 = (float*)normalBytes;
														float* ptr3 = (float*)textureBytes;
														int* ptr4 = (int*)vIndexBytes;
														int* ptr5 = (int*)nIndexBytes;
														int* ptr6 = (int*)tIndexBytes;
														float* ptr7 = (float*)tangentBytes;
														modVbo[num4].vbStart = num5;
														modVbo[num4].ibStart = ibStart;
														if (modVbo[num4].usesRigging == 0)
														{
															for (long num6 = 0L; num6 < modVbo[num4].pcount; num6++)
															{
																long num7 = num6 * 3;
																int num8 = ptr4[num7];
																int num9 = num8 * 3;
																int num10 = ptr5[num7] * 3;
																int num11 = ptr6[num7] * 2;
																int num12 = num8 + num5;
																mVtexObjectsLevel[num12].Set_Values(ptr[num9], (ptr + num9)[1], (ptr + num9)[2], ptr2[num10], (ptr2 + num10)[1], (ptr2 + num10)[2], ptr7[num7], (ptr7 + num7)[1], (ptr7 + num7)[2], ptr3[num11], (ptr3 + num11)[1], modVbo[num4].defaultColor[0], modVbo[num4].defaultColor[1], modVbo[num4].defaultColor[2], modVbo[num4].defaultColor[3], 0, 0, 0, 0, 1f, 0f, 0f, 0f);
																mViObjectsLevel[ibStart++] = num12;
																num8 = (ptr4 + num7)[1];
																num9 = num8 * 3;
																num10 = (ptr5 + num7)[1] * 3;
																num11 = (ptr6 + num7)[1] * 2;
																num12 = num8 + num5;
																mVtexObjectsLevel[num12].Set_Values(ptr[num9], (ptr + num9)[1], (ptr + num9)[2], ptr2[num10], (ptr2 + num10)[1], (ptr2 + num10)[2], ptr7[num7], (ptr7 + num7)[1], (ptr7 + num7)[2], ptr3[num11], (ptr3 + num11)[1], modVbo[num4].defaultColor[0], modVbo[num4].defaultColor[1], modVbo[num4].defaultColor[2], modVbo[num4].defaultColor[3], 0, 0, 0, 0, 1f, 0f, 0f, 0f);
																mViObjectsLevel[ibStart++] = num12;
																num8 = (ptr4 + num7)[2];
																num9 = num8 * 3;
																num10 = (ptr5 + num7)[2] * 3;
																num11 = (ptr6 + num7)[2] * 2;
																num12 = num8 + num5;
																mVtexObjectsLevel[num12].Set_Values(ptr[num9], (ptr + num9)[1], (ptr + num9)[2], ptr2[num10], (ptr2 + num10)[1], (ptr2 + num10)[2], ptr7[num7], (ptr7 + num7)[1], (ptr7 + num7)[2], ptr3[num11], (ptr3 + num11)[1], modVbo[num4].defaultColor[0], modVbo[num4].defaultColor[1], modVbo[num4].defaultColor[2], modVbo[num4].defaultColor[3], 0, 0, 0, 0, 1f, 0f, 0f, 0f);
																mViObjectsLevel[ibStart++] = num12;
															}
														}
														else
														{
															float* ptr8 = (float*)bwBytes;
															float* ptr9 = (float*)bwBytes2;
															float* ptr10 = (float*)bwBytes3;
															float* ptr11 = (float*)bwBytes4;
															for (long num6 = 0L; num6 < modVbo[num4].pcount; num6++)
															{
																long num7 = num6 * 3;
																int num8 = ptr4[num7];
																int num9 = num8 * 3;
																int num10 = ptr5[num7] * 3;
																int num11 = ptr6[num7] * 2;
																int num12 = num8 + num5;
																mVtexObjectsLevel[num12].Set_Values(ptr[num9], (ptr + num9)[1], (ptr + num9)[2], ptr2[num10], (ptr2 + num10)[1], (ptr2 + num10)[2], ptr7[num7], (ptr7 + num7)[1], (ptr7 + num7)[2], ptr3[num11], (ptr3 + num11)[1], modVbo[num4].defaultColor[0], modVbo[num4].defaultColor[1], modVbo[num4].defaultColor[2], modVbo[num4].defaultColor[3], modVbo[num4].blendIndex0[num8], modVbo[num4].blendIndex1[num8], modVbo[num4].blendIndex2[num8], modVbo[num4].blendIndex3[num8], ptr8[num8], ptr9[num8], ptr10[num8], ptr11[num8]);
																mViObjectsLevel[ibStart++] = num12;
																num8 = (ptr4 + num7)[1];
																num9 = num8 * 3;
																num10 = (ptr5 + num7)[1] * 3;
																num11 = (ptr6 + num7)[1] * 2;
																num12 = num8 + num5;
																mVtexObjectsLevel[num12].Set_Values(ptr[num9], (ptr + num9)[1], (ptr + num9)[2], ptr2[num10], (ptr2 + num10)[1], (ptr2 + num10)[2], ptr7[num7], (ptr7 + num7)[1], (ptr7 + num7)[2], ptr3[num11], (ptr3 + num11)[1], modVbo[num4].defaultColor[0], modVbo[num4].defaultColor[1], modVbo[num4].defaultColor[2], modVbo[num4].defaultColor[3], modVbo[num4].blendIndex0[num8], modVbo[num4].blendIndex1[num8], modVbo[num4].blendIndex2[num8], modVbo[num4].blendIndex3[num8], ptr8[num8], ptr9[num8], ptr10[num8], ptr11[num8]);
																mViObjectsLevel[ibStart++] = num12;
																num8 = (ptr4 + num7)[2];
																num9 = num8 * 3;
																num10 = (ptr5 + num7)[2] * 3;
																num11 = (ptr6 + num7)[2] * 2;
																num12 = num8 + num5;
																mVtexObjectsLevel[num12].Set_Values(ptr[num9], (ptr + num9)[1], (ptr + num9)[2], ptr2[num10], (ptr2 + num10)[1], (ptr2 + num10)[2], ptr7[num7], (ptr7 + num7)[1], (ptr7 + num7)[2], ptr3[num11], (ptr3 + num11)[1], modVbo[num4].defaultColor[0], modVbo[num4].defaultColor[1], modVbo[num4].defaultColor[2], modVbo[num4].defaultColor[3], modVbo[num4].blendIndex0[num8], modVbo[num4].blendIndex1[num8], modVbo[num4].blendIndex2[num8], modVbo[num4].blendIndex3[num8], ptr8[num8], ptr9[num8], ptr10[num8], ptr11[num8]);
																mViObjectsLevel[ibStart++] = num12;
															}
														}
														num5 += modVbo[num4].vcount;
													}
												}
											}
										}
									}
								}
							}
						}
					}
				}
			}
		}
		_ = StructsClass.VertexPositionColorNormalTexture.SizeInBytes;
		mVertexBufferObjectsLevel = new VertexBuffer(mGraphics, global::Rendering.Rendering.rDecVPCNT, num2, BufferUsage.None);
		mVertexBufferObjectsLevel.SetData(mVtexObjectsLevel);
		mIndexBufferObjectsLevel = new IndexBuffer(mGraphics, typeof(int), num3, BufferUsage.None);
		mIndexBufferObjectsLevel.SetData(mViObjectsLevel);
		for (long num4 = 0L; num4 < numLevelModels; num4++)
		{
			if (!modVbo[num4].inLevelVBO || modVbo[num4].instanceCount <= 0)
			{
				continue;
			}
			int num13 = modVbo[num4].pcount * 3;
			int instanceCount = modVbo[num4].instanceCount;
			int num14 = num13 * instanceCount;
			if (num14 != num)
			{
				num = num14;
				array2 = new int[num];
			}
			num14 = modVbo[num4].vbStart;
			int i = 0;
			int num15 = 0;
			for (; i < instanceCount; i++)
			{
				int num16 = i * num13;
				for (int j = 0; j < num13; j++)
				{
					array2[num15++] = mViObjectsLevel[num14 + j] + num16;
				}
			}
			modVbo[num4].mInstanceIndex = new IndexBuffer(mGraphics, typeof(int), num, BufferUsage.None);
			modVbo[num4].mInstanceIndex.SetData(array2);
			modVbo[num4].indexBufferSize = num;
			num *= 4;
			array = new byte[num];
			modVbo[num4].mInstanceIndex.GetData(array);
			mainC.maingameMain.Save_Buffer_Data(ref array, 1, num, "Model" + num4 + "InstanceBuffer.txt");
		}
		mVtexObjectsLevel = null;
		mViObjectsLevel = null;
	}

	public void Create_VBO_Shared_Model(long modID, ref StructsClass.VertexPositionColorNormalTexture[] vtexObjects, ref int[] viObjects, ref int vPtr, ref int iPtr, ref int primitiveCount, float R, float G, float B, float A, ref Matrix mv)
	{
		cvsmC1.R = (byte)(R * 255f);
		cvsmC1.G = (byte)(G * 255f);
		cvsmC1.B = (byte)(B * 255f);
		cvsmC1.A = (byte)(A * 255f);
		if (modID >= numModels || modID < 0)
		{
			return;
		}
		_ = mod1[modID].texMultX;
		_ = mod1[modID].texMultY;
		_ = mod1[modID].texXadj;
		_ = mod1[modID].texYadj;
		int vcount;
		int i;
		if (mod1[modID].defaultColor[3] < 1f)
		{
			byte alpha = (byte)(255f * mod1[modID].defaultColor[3]);
			int vbStart = mod1[modID].vbStart;
			vcount = mod1[modID].vcount;
			for (i = 0; i < vcount; i++)
			{
				mVtexObjects[vbStart++].Set_Alpha(alpha);
			}
		}
		cvsmOrigin = Vector3.Transform(Vector3.Zero, mv);
		vcount = mod1[modID].vbStart;
		int num = vcount + mod1[modID].vcount;
		for (i = vcount; i < num; i++)
		{
			viObjects[iPtr++] = vPtr;
			viObjects[iPtr++] = vPtr + 1;
			viObjects[iPtr++] = vPtr + 2;
			cvsmModPos = mVtexObjects[i].Position;
			cvsmModNorm = mVtexObjects[i].Normal;
			cvsmModTang = mVtexObjects[i].Tangent;
			Vector3.Transform(ref cvsmModPos, ref mv, out cvsmPos);
			Vector3.Transform(ref cvsmModNorm, ref mv, out cvsmNorm);
			Vector3.Transform(ref cvsmModTang, ref mv, out cvsmTang);
			cvsmNorm -= cvsmOrigin;
			cvsmNorm.Normalize();
			cvsmTang -= cvsmOrigin;
			cvsmTang.Normalize();
			ref StructsClass.VertexPositionColorNormalTexture reference = ref vtexObjects[vPtr++];
			reference = new StructsClass.VertexPositionColorNormalTexture(cvsmPos, cvsmC1, cvsmNorm, cvsmModTang, mVtexObjects[i].Texture);
			i++;
			cvsmModPos = mVtexObjects[i].Position;
			cvsmModNorm = mVtexObjects[i].Normal;
			cvsmModTang = mVtexObjects[i].Tangent;
			Vector3.Transform(ref cvsmModPos, ref mv, out cvsmPos);
			Vector3.Transform(ref cvsmModNorm, ref mv, out cvsmNorm);
			Vector3.Transform(ref cvsmModTang, ref mv, out cvsmTang);
			cvsmNorm -= cvsmOrigin;
			cvsmNorm.Normalize();
			cvsmTang -= cvsmOrigin;
			cvsmTang.Normalize();
			ref StructsClass.VertexPositionColorNormalTexture reference2 = ref vtexObjects[vPtr++];
			reference2 = new StructsClass.VertexPositionColorNormalTexture(cvsmPos, cvsmC1, cvsmNorm, cvsmModTang, mVtexObjects[i].Texture);
			i++;
			cvsmModPos = mVtexObjects[i].Position;
			cvsmModNorm = mVtexObjects[i].Normal;
			cvsmModTang = mVtexObjects[i].Tangent;
			Vector3.Transform(ref cvsmModPos, ref mv, out cvsmPos);
			Vector3.Transform(ref cvsmModNorm, ref mv, out cvsmNorm);
			Vector3.Transform(ref cvsmModTang, ref mv, out cvsmTang);
			cvsmNorm -= cvsmOrigin;
			cvsmNorm.Normalize();
			cvsmTang -= cvsmOrigin;
			cvsmTang.Normalize();
			ref StructsClass.VertexPositionColorNormalTexture reference3 = ref vtexObjects[vPtr++];
			reference3 = new StructsClass.VertexPositionColorNormalTexture(cvsmPos, cvsmC1, cvsmNorm, cvsmTang, mVtexObjects[i].Texture);
		}
		primitiveCount += mod1[modID].triangleCount;
	}

	public void Create_VBO_Shared_Level_Model(long modID, ref StructsClass.VertexPositionColorNormalTexture[] vtexObjects, ref int[] viObjects, ref int vPtr, ref int iPtr, ref int primitiveCount, float R, float G, float B, float A, ref Matrix mv)
	{
		cvsmC1.R = (byte)(R * 255f);
		cvsmC1.G = (byte)(G * 255f);
		cvsmC1.B = (byte)(B * 255f);
		cvsmC1.A = (byte)(A * 255f);
		int vcount;
		if (modVbo[modID].defaultColor[3] < 1f)
		{
			byte alpha = (byte)(255f * modVbo[modID].defaultColor[3]);
			int vbStart = modVbo[modID].vbStart;
			vcount = modVbo[modID].vcount;
			for (int i = 0; i < vcount; i++)
			{
				mVtexObjectsLevel[vbStart++].Set_Alpha(alpha);
			}
		}
		int num = vPtr - modVbo[modID].vbStart;
		vcount = modVbo[modID].pcount;
		int ibStart = modVbo[modID].ibStart;
		for (int i = 0; i < vcount; i++)
		{
			viObjects[iPtr++] = mViObjectsLevel[ibStart++] + num;
			viObjects[iPtr++] = mViObjectsLevel[ibStart++] + num;
			viObjects[iPtr++] = mViObjectsLevel[ibStart++] + num;
		}
		cvsmOrigin = Vector3.Transform(Vector3.Zero, mv);
		vcount = modVbo[modID].vbStart;
		ibStart = vcount + modVbo[modID].vcount;
		for (int i = vcount; i < ibStart; i++)
		{
			cvsmModPos = mVtexObjectsLevel[i].Position;
			cvsmModNorm = mVtexObjectsLevel[i].Normal;
			cvsmModTang = mVtexObjectsLevel[i].Tangent;
			Vector3.Transform(ref cvsmModPos, ref mv, out cvsmPos);
			Vector3.Transform(ref cvsmModNorm, ref mv, out cvsmNorm);
			Vector3.Transform(ref cvsmModTang, ref mv, out cvsmTang);
			cvsmNorm -= cvsmOrigin;
			cvsmTang -= cvsmOrigin;
			cvsmNorm.Normalize();
			cvsmTang.Normalize();
			ref StructsClass.VertexPositionColorNormalTexture reference = ref vtexObjects[vPtr++];
			reference = new StructsClass.VertexPositionColorNormalTexture(cvsmPos, cvsmC1, cvsmNorm, cvsmModTang, mVtexObjectsLevel[i].Texture);
		}
		cvsmModPos = Vector3.Down;
		cvsmModNorm = Vector3.Down;
		cvsmModTang = Vector3.Down;
		primitiveCount += modVbo[modID].pcount;
	}

	public void Create_VBO_Main_Model(long modID, ref StructsClass.VertexPositionColorNormalTexture[] vtexObjects, ref int[] viObjects, ref int vPtr, ref int iPtr, ref int primitiveCount, float R, float G, float B, float A, ref Matrix mv)
	{
		cvsmC1.R = (byte)(R * 255f);
		cvsmC1.G = (byte)(G * 255f);
		cvsmC1.B = (byte)(B * 255f);
		cvsmC1.A = (byte)(A * 255f);
		int vcount;
		if (mod1[modID].defaultColor[3] < 1f)
		{
			byte alpha = (byte)(255f * mod1[modID].defaultColor[3]);
			int vbStart = mod1[modID].vbStart;
			vcount = mod1[modID].vcount;
			for (int i = 0; i < vcount; i++)
			{
				mVtexObjects[vbStart++].Set_Alpha(alpha);
			}
		}
		int num = vPtr - mod1[modID].vbStart;
		vcount = mod1[modID].pcount;
		int ibStart = mod1[modID].ibStart;
		for (int i = 0; i < vcount; i++)
		{
			viObjects[iPtr++] = mViObjects[ibStart++] + num;
			viObjects[iPtr++] = mViObjects[ibStart++] + num;
			viObjects[iPtr++] = mViObjects[ibStart++] + num;
		}
		cvsmOrigin = Vector3.Transform(Vector3.Zero, mv);
		vcount = mod1[modID].vbStart;
		ibStart = vcount + mod1[modID].vcount;
		for (int i = vcount; i < ibStart; i++)
		{
			cvsmModPos = mVtexObjects[i].Position;
			cvsmModNorm = mVtexObjects[i].Normal;
			cvsmModTang = mVtexObjects[i].Tangent;
			Vector3.Transform(ref cvsmModPos, ref mv, out cvsmPos);
			Vector3.Transform(ref cvsmModNorm, ref mv, out cvsmNorm);
			Vector3.Transform(ref cvsmModTang, ref mv, out cvsmTang);
			cvsmNorm -= cvsmOrigin;
			cvsmTang -= cvsmOrigin;
			cvsmNorm.Normalize();
			cvsmTang.Normalize();
			ref StructsClass.VertexPositionColorNormalTexture reference = ref vtexObjects[vPtr++];
			reference = new StructsClass.VertexPositionColorNormalTexture(cvsmPos, cvsmC1, cvsmNorm, cvsmModTang, mVtexObjects[i].Texture);
		}
		cvsmModPos = Vector3.Down;
		cvsmModNorm = Vector3.Down;
		cvsmModTang = Vector3.Down;
		primitiveCount += mod1[modID].pcount;
	}

	public int Count_Static_Buffer_Faces_Model_Opaque(long modID)
	{
		return mod1[modID].pcount;
	}

	public int Count_Static_Buffer_Faces_Level_Model_Opaque(long modID)
	{
		return modVbo[modID].pcount;
	}
}
