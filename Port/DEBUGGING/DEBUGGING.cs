using MainGame;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Models;
using Players;
using Rendering;
using Weapons;
using WindowsGame1;

namespace DEBUGGING;

public class DEBUGGING
{
	public int debugID;

	public byte breakpointID;

	public byte currentBreakPoint;

	public VertexPositionColor[] dVertex = new VertexPositionColor[4];

	public BasicEffect dEffect;

	public GraphicsDevice dbgGraphics;

	public Game1.MasterCollection mainC;

	public void Reset_Breakpoint()
	{
		currentBreakPoint = 0;
	}

	public bool Hit_Breakpoint()
	{
		currentBreakPoint++;
		if (breakpointID <= 0 || currentBreakPoint < breakpointID)
		{
			return false;
		}
		return true;
	}

	public void Set_BreakPoint(short newValue)
	{
		if (newValue < 0)
		{
			newValue = 0;
		}
		breakpointID = (byte)newValue;
	}

	public void Init_Vars(Game1.MasterCollection master, GraphicsDevice graphics)
	{
		mainC = master;
		dEffect = new BasicEffect(graphics);
		dEffect.TextureEnabled = false;
		dEffect.LightingEnabled = false;
		dVertex[0] = default(VertexPositionColor);
		dVertex[1] = default(VertexPositionColor);
		dVertex[2] = default(VertexPositionColor);
		dVertex[3] = default(VertexPositionColor);
		dVertex[0].Color = Color.Yellow;
		dVertex[1].Color = Color.Yellow;
		dVertex[2].Color = Color.Yellow;
		dVertex[3].Color = Color.Yellow;
	}

	public void Show_Bounding_Box(int pID)
	{
		byte rBufferID = global::Rendering.Rendering.rBufferID;
		global::Rendering.Rendering.rGraphics.DepthStencilState = global::Rendering.Rendering.depthBufferDisabled;
		byte type = global::MainGame.MainGame.playerVehicles[pID].type;
		if (type == 0 || type == 8)
		{
			for (int i = 0; i < global::Players.Players.players[pID].charMain.numUsed; i++)
			{
				Matrix mv = Matrix.CreateScale(0.1f) * Matrix.CreateTranslation(global::Players.Players.players[pID].posX[rBufferID] + global::Players.Players.players[pID].charMain.v1[i].v[0], global::Players.Players.players[pID].posY[rBufferID] + global::Players.Players.players[pID].charMain.v1[i].v[1], global::Players.Players.players[pID].posZ[rBufferID] + global::Players.Players.players[pID].charMain.v1[i].v[2]);
				mainC.modelsMain.Render_Model(global::Models.Models.modSquare, ref mv);
			}
		}
		else
		{
			int i = 0;
			int num = 0;
			while (i < global::MainGame.MainGame.playerVehicles[pID].momentum.numPoints)
			{
				Matrix mv = Matrix.CreateScale(0.1f) * Matrix.CreateTranslation(global::MainGame.MainGame.playerVehicles[pID].momentum.collisionPoints[num], global::MainGame.MainGame.playerVehicles[pID].momentum.collisionPoints[num + 1], global::MainGame.MainGame.playerVehicles[pID].momentum.collisionPoints[num + 2]) * global::Players.Players.players[pID].mv[rBufferID] * Matrix.CreateTranslation(global::Players.Players.players[pID].posX[rBufferID], global::Players.Players.players[pID].posY[rBufferID], global::Players.Players.players[pID].posZ[rBufferID]);
				mainC.modelsMain.Render_Model(global::Models.Models.modSquare, ref mv);
				i++;
				num += 3;
			}
		}
		global::Rendering.Rendering.rGraphics.DepthStencilState = global::Rendering.Rendering.depthBufferEnabled;
	}

	public void Draw_Weapon_Particle_Location(ushort pID)
	{
		Matrix matrix = default(Matrix);
		short primaryWeaponMountWeapon = global::Players.Players.players[pID].primaryWeaponMountWeapon;
		byte numBarrels = global::Weapons.Weapons.wp1[primaryWeaponMountWeapon].numBarrels;
		for (byte b = 0; b < numBarrels; b++)
		{
			float xPosition = global::Players.Players.players[pID].weapon1.offset[b, 9].v[0];
			float yPosition = global::Players.Players.players[pID].weapon1.offset[b, 9].v[1];
			float zPosition = global::Players.Players.players[pID].weapon1.offset[b, 9].v[2];
			matrix = Matrix.CreateTranslation(xPosition, yPosition, zPosition);
			mainC.modelsMain.Render_Model(global::Models.Models.modSquare, ref matrix);
		}
	}
}
