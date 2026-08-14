using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Rendering;
using Structs;
using WindowsGame1;

namespace FontModule;

public class FontModule
{
	public static SpriteBatch FontSprite1;

	public static SpriteFont[] GameFonts = new SpriteFont[4];

	public static StructsClass.Onscreen_Text multiplayerMessages = default(StructsClass.Onscreen_Text);

	public static Vector2 pfV1 = default(Vector2);

	public static Vector2 dscvrV1 = default(Vector2);

	public static Color dtextColor = default(Color);

	public Game1.MasterCollection mainC;

	public void Init_Vars(Game1.MasterCollection masterC)
	{
		mainC = masterC;
		FontSprite1 = new SpriteBatch(mainC.curGame.GraphicsDevice);
		GameFonts[0] = mainC.curGame.Content.Load<SpriteFont>("GameFont1");
		GameFonts[1] = mainC.curGame.Content.Load<SpriteFont>("GameFont2");
		GameFonts[2] = mainC.curGame.Content.Load<SpriteFont>("GameFont3");
		GameFonts[3] = mainC.curGame.Content.Load<SpriteFont>("GameFont4");
	}

	public void Init_FontModule()
	{
		multiplayerMessages.bottomLeftX = mainC.curGame.safeX;
		multiplayerMessages.bottomLeftY = (float)mainC.curGame.safeY + 231f;
		multiplayerMessages.numTextItems = 3;
		multiplayerMessages.scrollDirection = 4;
		multiplayerMessages.numTextItems = 3;
		multiplayerMessages.lifeTime = 5f;
		float[] timeRemaining = new float[3];
		multiplayerMessages.timeRemaining = timeRemaining;
		multiplayerMessages.textItems = new string[5] { "", "", "", "", "" };
		multiplayerMessages.spacingX = 0f;
		multiplayerMessages.spacingY = -20f;
		multiplayerMessages.fontID = 0;
		multiplayerMessages.fColor = new Color(255, 255, 255, 255);
		multiplayerMessages.curItems = 0;
	}

	public void Draw_String(string str1, ref Vector2 position, ref Color fontColor, byte fontType)
	{
		try
		{
			FontSprite1.Begin();
			FontSprite1.DrawString(GameFonts[fontType], str1, position, fontColor);
			FontSprite1.End();
		}
		catch (Exception)
		{
			try
			{
				FontSprite1.End();
			}
			catch
			{
			}
		}
	}

	public void Draw_Positioned_String(string str1, float x, float y, byte r, byte g, byte b, byte a, byte fontType)
	{
		dscvrV1.X = x + (float)mainC.curGame.halfWidth;
		dscvrV1.Y = 0f - y + (float)mainC.curGame.halfHeight;
		dtextColor.R = r;
		dtextColor.G = g;
		dtextColor.B = b;
		dtextColor.A = a;
		FontSprite1.Begin();
		FontSprite1.DrawString(GameFonts[fontType], str1, dscvrV1, dtextColor);
		FontSprite1.End();
	}

	public void Draw_Positioned_String_Centered(string str1, float x, float y, byte r, byte g, byte b, byte a, byte fontType)
	{
		dscvrV1 = GameFonts[fontType].MeasureString(str1);
		x -= dscvrV1.X / 2f;
		y += (float)GameFonts[fontType].LineSpacing / 2f;
		dscvrV1.X = x + (float)mainC.curGame.halfWidth;
		dscvrV1.Y = 0f - y + (float)mainC.curGame.halfHeight;
		dtextColor.R = r;
		dtextColor.G = g;
		dtextColor.B = b;
		dtextColor.A = a;
		FontSprite1.Begin();
		FontSprite1.DrawString(GameFonts[fontType], str1, dscvrV1, dtextColor);
		FontSprite1.End();
	}

	public void Draw_Positioned_String_Centered_Horizontally(string str1, float x, float y, byte r, byte g, byte b, byte a, byte fontType)
	{
		dscvrV1 = GameFonts[fontType].MeasureString(str1);
		x -= dscvrV1.X / 2f;
		dscvrV1.X = x + (float)mainC.curGame.halfWidth;
		dscvrV1.Y = 0f - y + (float)mainC.curGame.halfHeight;
		dtextColor.R = r;
		dtextColor.G = g;
		dtextColor.B = b;
		dtextColor.A = a;
		FontSprite1.Begin();
		FontSprite1.DrawString(GameFonts[fontType], str1, dscvrV1, dtextColor);
		FontSprite1.End();
	}

	public void Draw_String_Centered_Vertically(string str1, ref Vector2 position, ref Color fontColor, byte fontType)
	{
		float y = position.Y;
		position.Y -= (float)GameFonts[fontType].LineSpacing / 2f;
		FontSprite1.Begin();
		FontSprite1.DrawString(GameFonts[fontType], str1, position, fontColor);
		FontSprite1.End();
		position.Y = y;
	}

	public void Draw_String_Centered_Vertically_RightJustified(string str1, Vector2 position, ref Color fontColor, byte fontType)
	{
		float y = position.Y;
		dscvrV1 = GameFonts[fontType].MeasureString(str1);
		position.X -= dscvrV1.X;
		position.Y -= (float)GameFonts[fontType].LineSpacing / 2f;
		FontSprite1.Begin();
		FontSprite1.DrawString(GameFonts[fontType], str1, position, fontColor);
		FontSprite1.End();
		position.Y = y;
	}

	public void Draw_String_RightJustified(string str1, Vector2 position, Color fontColor, byte fontType)
	{
		float y = position.Y;
		dscvrV1 = GameFonts[fontType].MeasureString(str1);
		position.X -= dscvrV1.X;
		FontSprite1.Begin();
		FontSprite1.DrawString(GameFonts[fontType], str1, position, fontColor);
		FontSprite1.End();
		position.Y = y;
	}

	public void Draw_String_RightJustified(string str1, float x, float y, byte r, byte g, byte b, byte a, byte fontType)
	{
		dscvrV1 = GameFonts[fontType].MeasureString(str1);
		dscvrV1.X = x + (float)mainC.curGame.halfWidth - dscvrV1.X;
		dscvrV1.Y = 0f - y + (float)mainC.curGame.halfHeight;
		dtextColor.R = r;
		dtextColor.G = g;
		dtextColor.B = b;
		dtextColor.A = a;
		FontSprite1.Begin();
		FontSprite1.DrawString(GameFonts[fontType], str1, dscvrV1, dtextColor);
		FontSprite1.End();
	}

	public float Get_LineLength(string str1, byte fontType)
	{
		return GameFonts[fontType].MeasureString(str1).X;
	}

	public float Get_Line_Height(byte fontType)
	{
		return GameFonts[fontType].MeasureString("A").Y;
	}

	public void Draw_String_Centered(string str1, ref Vector2 position, Color fontColor, byte fontType)
	{
		float x = position.X;
		float y = position.Y;
		dscvrV1 = GameFonts[fontType].MeasureString(str1);
		position.X -= dscvrV1.X / 2f;
		position.Y -= (float)GameFonts[fontType].LineSpacing / 2f;
		FontSprite1.Begin();
		FontSprite1.DrawString(GameFonts[fontType], str1, position, fontColor);
		FontSprite1.End();
		position.X = x;
		position.Y = y;
	}

	public void Render_Onscreen_Text(float frameTime)
	{
		pfV1.X = multiplayerMessages.bottomLeftX;
		pfV1.Y = multiplayerMessages.bottomLeftY;
		Move_Text(frameTime);
		int numTextItems = multiplayerMessages.numTextItems;
		for (int i = 0; i < numTextItems; i++)
		{
			if (multiplayerMessages.timeRemaining[i] > 0f)
			{
				FontSprite1.Begin();
				FontSprite1.DrawString(GameFonts[multiplayerMessages.fontID], multiplayerMessages.textItems[i], pfV1, multiplayerMessages.fColor);
				FontSprite1.End();
				multiplayerMessages.timeRemaining[i] -= frameTime;
			}
			pfV1.X += multiplayerMessages.spacingX;
			pfV1.Y += multiplayerMessages.spacingY;
		}
	}

	public void Move_Text(float frameTime)
	{
		byte b = (byte)(multiplayerMessages.scrollDirection & 5);
		byte b2 = (byte)(multiplayerMessages.numTextItems - 1);
		switch (b)
		{
		case 0:
			if (multiplayerMessages.timeRemaining[0] > 0f && multiplayerMessages.timeRemaining[0] < frameTime)
			{
				byte b3 = 0;
				while (b3 < b2)
				{
					multiplayerMessages.textItems[b3] = multiplayerMessages.textItems[b3 + 1];
					multiplayerMessages.timeRemaining[b3] = multiplayerMessages.timeRemaining[++b3];
				}
				multiplayerMessages.textItems[b2] = "";
				multiplayerMessages.timeRemaining[b2] = 0f;
			}
			break;
		case 1:
		case 4:
		case 5:
			if (multiplayerMessages.timeRemaining[b2] > 0f && multiplayerMessages.timeRemaining[b2] < frameTime)
			{
				byte b3 = b2;
				while (b3 > 0)
				{
					multiplayerMessages.textItems[b3] = multiplayerMessages.textItems[b3 - 1];
					multiplayerMessages.timeRemaining[b3] = multiplayerMessages.timeRemaining[--b3];
				}
				multiplayerMessages.textItems[0] = "";
				multiplayerMessages.timeRemaining[0] = 0f;
			}
			break;
		case 2:
		case 3:
			break;
		}
	}

	public void Add_Text_To_Onscreen_Text(string str1)
	{
		byte b = (byte)(multiplayerMessages.scrollDirection & 5);
		byte b2 = (byte)(multiplayerMessages.numTextItems - 1);
		switch (b)
		{
		case 0:
		{
			byte b3 = 0;
			while (b3 < b2)
			{
				multiplayerMessages.textItems[b3] = multiplayerMessages.textItems[b3 + 1];
				multiplayerMessages.timeRemaining[b3] = multiplayerMessages.timeRemaining[++b3];
			}
			multiplayerMessages.textItems[b2] = str1;
			multiplayerMessages.timeRemaining[b2] = multiplayerMessages.lifeTime;
			break;
		}
		case 1:
		case 4:
		case 5:
		{
			byte b3 = b2;
			while (b3 > 0)
			{
				multiplayerMessages.textItems[b3] = multiplayerMessages.textItems[b3 - 1];
				multiplayerMessages.timeRemaining[b3] = multiplayerMessages.timeRemaining[--b3];
			}
			multiplayerMessages.textItems[0] = str1;
			multiplayerMessages.timeRemaining[0] = multiplayerMessages.lifeTime;
			break;
		}
		case 2:
		case 3:
			break;
		}
	}

	public void Clear_Onscreen_Text()
	{
		byte numTextItems = multiplayerMessages.numTextItems;
		for (byte b = 0; b < numTextItems; b++)
		{
			multiplayerMessages.timeRemaining[b] = 0f;
		}
	}

	public void Reset_Graphics_Adapter_After_SpriteBatch()
	{
		mainC.curGame.GraphicsDevice.DepthStencilState = global::Rendering.Rendering.depthBufferEnabled;
		mainC.curGame.GraphicsDevice.BlendState = BlendState.Opaque;
		mainC.curGame.GraphicsDevice.RasterizerState = global::Rendering.Rendering.rasterizerState;
		mainC.curGame.GraphicsDevice.SamplerStates[0] = global::Rendering.Rendering.textureSamplerState;
	}

	public void Reset_Round()
	{
		int numTextItems = multiplayerMessages.numTextItems;
		for (int i = 0; i < numTextItems; i++)
		{
			multiplayerMessages.textItems[i] = "";
			multiplayerMessages.timeRemaining[i] = 0f;
		}
	}
}
