using System;
using System.Collections.Generic;
using System.IO;
using AI;
using Collision;
using DEBUGGING;
using FontModule;
using GameObjects;
using InputHandler;
using Joints;
using Levels;
using MainGame;
using Maps;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Models;
using Networking;
using Physics;
using Pickups;
using Players;
using Programs;
using Rendering;
using Sounds;
using Switches;
using Terrain;
using Textures;
using Threads;
using Util;
using Weapons;

namespace WindowsGame1;

public class Game1 : Game
{
	public class MasterCollection
	{
		public Game1 curGame;

		public global::AI.AI aiMain = new global::AI.AI();

		public Avatars avatarMain = new Avatars();

		public Callbacks callbackMain = new Callbacks();

		public global::Collision.Collision collisionMain = new global::Collision.Collision();

		public global::FontModule.FontModule fontmoduleMain = new global::FontModule.FontModule();

		public global::InputHandler.InputHandler inputMain = new global::InputHandler.InputHandler();

		public global::GameObjects.GameObjects gameobjectMain = new global::GameObjects.GameObjects();

		public global::Joints.Joints jointsMain = new global::Joints.Joints();

		public global::Terrain.Terrain terrainMain = new global::Terrain.Terrain();

		public global::Levels.Levels levelsMain = new global::Levels.Levels();

		public global::MainGame.MainGame maingameMain = new global::MainGame.MainGame();

		public global::Maps.Maps mapsMain = new global::Maps.Maps();

		public global::Models.Models modelsMain = new global::Models.Models();

		public global::Networking.Networking networkingMain = new global::Networking.Networking();

		public global::Pickups.Pickups pickupsMain = new global::Pickups.Pickups();

		public global::Physics.Physics physicsMain = new global::Physics.Physics();

		public global::Players.Players playersMain = new global::Players.Players();

		public global::Programs.Programs programsMain = new global::Programs.Programs();

		public global::Rendering.Rendering renderingMain = new global::Rendering.Rendering();

		public global::Sounds.Sounds soundsMain = new global::Sounds.Sounds();

		public global::Switches.Switches switchesMain = new global::Switches.Switches();

		public global::Textures.Textures texturesMain = new global::Textures.Textures();

		public global::Threads.Threads threadingMain = new global::Threads.Threads();

		public global::Util.Util utilMain = new global::Util.Util();

		public Vehicles vehicles = new Vehicles();

		public global::Weapons.Weapons weaponsMain = new global::Weapons.Weapons();

		public Level_Editor levelEditor = new Level_Editor();

		public global::DEBUGGING.DEBUGGING debuggingMain = new global::DEBUGGING.DEBUGGING();

		public Graphs graphingMain = new Graphs();

		public Zones zonesMain = new Zones();

		public Targets targetMain = new Targets();

		public GameLogic gameLogic = new GameLogic();

		public User_Interface userInterface = new User_Interface();

		public Explosions Explosions = new Explosions();

		public void Quit_Game()
		{
			curGame.timeToQuit = true;
		}

		public static int getFileCount(string dir, string pattern)
		{
			ICollection<string> files = Directory.GetFiles(Environment.CurrentDirectory + "\\" + dir, pattern);
			return files.Count;
		}

		public static int getFile(string dir, string pattern, ref string[] fList)
		{
			ICollection<string> files = Directory.GetFiles(Environment.CurrentDirectory + "\\" + dir, pattern);
			fList = (string[])files;
			for (int i = 0; i < fList.Length; i++)
			{
				string[] array = fList[i].Split('\\');
				for (int num = array.Length - 1; num >= 0; num--)
				{
					if (array[num].Length > 0)
					{
						fList[i] = array[num];
						break;
					}
				}
			}
			return files.Count;
		}

		public void MC_Hide_Mouse()
		{
			curGame.IsMouseVisible = false;
		}

		public void MC_Show_Mouse()
		{
			curGame.IsMouseVisible = true;
		}
	}

	public ContentManager CM_Level;

	public ContentManager CM_Loading;

	public MasterCollection MasterC = new MasterCollection();

	public PresentationParameters presentationParameters;

	public GraphicsDeviceManager graphics;

	private static KeyboardState keyState;

	private static KeyboardState oldKeyState;

	public bool timeToQuit;

	public int width;

	public int height;

	public int safeWidth;

	public int safeHeight;

	public int safeX;

	public int safeY;

	public int halfWidth;

	public int halfHeight;

	public int safeWidthHalf;

	public int safeHeightHalf;

	public Game1()
	{
		base.IsFixedTimeStep = false;
		graphics = new GraphicsDeviceManager(this);
		base.Content.RootDirectory = "The_CoOp_Zombie_Game";
		CM_Level = new ContentManager(base.Content.ServiceProvider, "The_CoOp_Zombie_Game");
		CM_Loading = new ContentManager(base.Content.ServiceProvider, "The_CoOp_Zombie_Game");
		graphics.PreferredBackBufferWidth = 1280;
		graphics.PreferredBackBufferHeight = 720;
		graphics.SynchronizeWithVerticalRetrace = true;
		graphics.PreferMultiSampling = true;
		graphics.PreparingDeviceSettings += graphics_PreparingDeviceSettings;
		graphics.ApplyChanges();
		MasterC.curGame = this;
		width = base.GraphicsDevice.Viewport.Width;
		height = base.GraphicsDevice.Viewport.Height;
		halfWidth = width / 2;
		halfHeight = height / 2;
		safeWidth = base.GraphicsDevice.Viewport.TitleSafeArea.Width;
		safeHeight = base.GraphicsDevice.Viewport.TitleSafeArea.Height;
		safeX = base.GraphicsDevice.Viewport.TitleSafeArea.X;
		safeY = base.GraphicsDevice.Viewport.TitleSafeArea.Y;
		safeWidthHalf = safeWidth / 2;
		safeHeightHalf = safeHeight / 2;
		base.Components.Add(new GamerServicesComponent(this));
	}

	protected override void OnDeactivated(object sender, EventArgs args)
	{
		MasterC.inputMain.Handle_Window_Deactivation();
		base.OnDeactivated(sender, args);
	}

	protected override void OnActivated(object sender, EventArgs args)
	{
		MasterC.inputMain.Handle_Window_Activation();
		base.OnActivated(sender, args);
	}

	private void ClientSizeChanged(object sender, EventArgs e)
	{
		MasterC.inputMain.Handle_Resize();
	}

	private void graphics_PreparingDeviceSettings(object sender, PreparingDeviceSettingsEventArgs e)
	{
		e.GraphicsDeviceInformation.PresentationParameters.MultiSampleCount = 4;
	}

	protected override void Initialize()
	{
		presentationParameters = base.GraphicsDevice.PresentationParameters;
		MasterC.debuggingMain.Init_Vars(MasterC, base.GraphicsDevice);
		MasterC.fontmoduleMain.Init_Vars(MasterC);
		MasterC.levelsMain.Init_Vars(MasterC);
		MasterC.playersMain.Init_Vars(MasterC);
		MasterC.networkingMain.Init_Vars(MasterC);
		MasterC.renderingMain.Init_Vars(MasterC, base.GraphicsDevice);
		MasterC.texturesMain.Init_Vars(MasterC);
		MasterC.collisionMain.Init_Vars(MasterC);
		MasterC.modelsMain.Init_Vars(MasterC, base.GraphicsDevice);
		MasterC.soundsMain.Init_Vars(MasterC);
		MasterC.gameobjectMain.Init_Vars(MasterC);
		MasterC.weaponsMain.Init_Vars(MasterC);
		MasterC.jointsMain.Init_Vars(MasterC);
		MasterC.programsMain.Init_Vars(MasterC);
		MasterC.mapsMain.Init_Vars(MasterC);
		MasterC.terrainMain.Init_Vars(MasterC);
		MasterC.inputMain.Init_Vars(MasterC);
		MasterC.physicsMain.Init_Vars(MasterC);
		MasterC.aiMain.Init_Vars(MasterC);
		MasterC.pickupsMain.Init_Vars(MasterC);
		MasterC.switchesMain.Init_Vars(MasterC);
		MasterC.threadingMain.Init_Vars(MasterC);
		MasterC.callbackMain.Init_Vars(MasterC);
		MasterC.vehicles.Init_Vars(MasterC);
		MasterC.avatarMain.Init_Vars(MasterC);
		MasterC.graphingMain.Init_Vars(MasterC);
		MasterC.zonesMain.Init_Vars(MasterC);
		MasterC.targetMain.Init_Vars(MasterC);
		MasterC.gameLogic.Init_Vars(MasterC);
		MasterC.userInterface.Init_Vars(MasterC);
		MasterC.Explosions.Init_Vars(MasterC);
		base.Initialize();
		MasterC.maingameMain.Init_Vars(MasterC);
	}

	protected override void LoadContent()
	{
	}

	protected override void Update(GameTime gameTime)
	{
		keyState = Keyboard.GetState();
		MasterC.inputMain.Handle_Game_Input(ref keyState, ref oldKeyState);
		oldKeyState = keyState;
		MasterC.maingameMain.Main_Loop();
		if (timeToQuit)
		{
			Game_Exit();
		}
		base.Update(gameTime);
	}

	protected override void Draw(GameTime gameTime)
	{
		MasterC.renderingMain.Render_Scene(gameTime);
		MasterC.maingameMain.Process_End_Of_Frame_Messages();
		global::MainGame.MainGame.updateRunning = false;
		base.Draw(gameTime);
	}

	protected override void OnExiting(object sender, EventArgs args)
	{
		MasterC.threadingMain.Close();
	}

	public void Game_Exit()
	{
		MasterC.maingameMain.Close_All();
		Exit();
	}
}
