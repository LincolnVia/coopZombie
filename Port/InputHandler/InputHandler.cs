using System;
using MainGame;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Net;
using Networking;
using Players;
using Rendering;
using Sounds;
using Threads;
using WindowsGame1;

namespace InputHandler;

public class InputHandler
{
	public static float ta;

	public static float tb;

	public static float tc;

	public static float td;

	public static float te;

	public static float tf;

	public static float tg;

	public static float th;

	public static float ti;

	public static float tj;

	public static float tk;

	public static float tl = 1f;

	public static float tm;

	public static float tn;

	public static float to;

	public static float tp;

	public static float tq;

	public static float tr;

	public static float ts;

	public static float tt;

	public static float tu;

	public static float tv;

	public static float tw;

	public static float tx;

	public static float ty;

	public static float tz;

	public static float ttime;

	public static byte[,] menuConfig = new byte[12, 3]
	{
		{ 8, 0, 255 },
		{ 6, 0, 255 },
		{ 5, 0, 255 },
		{ 1, 0, 255 },
		{ 3, 0, 255 },
		{ 12, 0, 255 },
		{ 7, 0, 255 },
		{ 1, 0, 255 },
		{ 1, 0, 255 },
		{ 1, 0, 255 },
		{ 8, 0, 255 },
		{ 1, 0, 255 }
	};

	public static byte menuType = 0;

	public static byte menuChangeItem = 0;

	public static byte menuStat = 0;

	public static byte currentMenu = 0;

	public static byte gpadID = 4;

	public static byte lastGpadID = 4;

	public static byte menuItem = 0;

	public static byte numMenuItems = 0;

	public static byte lookMode = 0;

	public static byte lookModeAdj;

	public static byte rumble = 1;

	public static bool standardController;

	public static bool checkForOtherControllers = false;

	public static bool gameWindowActive = false;

	public static bool mouseVisible = true;

	public static bool newSPGame = false;

	public static bool mpJoinListReady = false;

	public static bool swapSticks = false;

	public static bool slowSideStep = false;

	public static bool mpLive;

	public static bool inMenu = false;

	public static bool ySetFromFile = false;

	public static bool buyMeOnExit = false;

	public static bool confirmEndGameScreen = false;

	public static bool showMessageBox_QM_NoGames = false;

	public static bool showMessageBox_MP_Error = false;

	public static bool messageBox_Brightness = false;

	public static bool messageBox_ExitToTitle = false;

	public static bool messageBox_ExitToDash = false;

	public static bool[] secondCtrollerAButtonPressed;

	public static bool[] secondCtrollerBButtonPressed;

	public static bool[] secondCtrollerDPadUpPressed;

	public static bool[] secondCtrollerDPadDownPressed;

	public static bool[] secondCtrollerDPadLeftPressed;

	public static bool[] secondCtrollerDPadRightPressed;

	public static bool[] secondCtrollerAPress;

	public static bool[] secondCtrollerBPress;

	public static bool[] secondCtrollerDPadUp;

	public static bool[] secondCtrollerDPadDown;

	public static bool[] secondCtrollerDPadLeft;

	public static bool[] secondCtrollerDPadRight;

	public static bool[] secondCtrollerSticks;

	public static float[] secondCtrollerDPadRepeat;

	public static bool controllerButtonStart;

	public static bool controllerButtonBack;

	public static bool controllerButtonStartPressed;

	public static bool controllerButtonBackPressed;

	public static bool controllerButtonX;

	public static bool controllerButtonY;

	public static bool controllerButtonA;

	public static bool controllerButtonB;

	public static bool controllerButtonXPressed;

	public static bool controllerButtonYPressed;

	public static bool controllerButtonAPressed;

	public static bool controllerButtonBPressed;

	public static bool controllerButtonLeftShoulder;

	public static bool controllerButtonRightShoulder;

	public static bool controllerButtonLeftShoulderPressed;

	public static bool controllerButtonRightShoulderPressed;

	public static bool controllerTriggerRight;

	public static bool controllerTriggerLeft;

	public static bool controllerTriggerRightPressed;

	public static bool controllerTriggerLeftPressed;

	public static bool controllerStickButtonRight;

	public static bool controllerStickButtonLeft;

	public static bool controllerStickButtonRightPressed;

	public static bool controllerStickButtonLeftPressed;

	public static bool controllerDPadUp;

	public static bool controllerDPadDown;

	public static bool controllerDPadLeft;

	public static bool controllerDPadRight;

	public static bool controllerDPadUpPressed;

	public static bool controllerDPadDownPressed;

	public static bool controllerDPadLeftPressed;

	public static bool controllerDPadRightPressed;

	public static short chosenSessionID;

	public static short mpJoinListIndex;

	public static float controllerStickLeftValueX;

	public static float controllerStickLeftValueY;

	public static float controllerStickRightValueX;

	public static float controllerStickRightValueY;

	public static float controllerStickLastLeftValueX;

	public static float controllerStickLastLeftValueY;

	public static float controllerStickLastRightValueX;

	public static float controllerStickLastRightValueY;

	public static float controllerStickLeftRepeatX;

	public static float controllerStickLeftRepeatY;

	public static float controllerStickRightSmoothX;

	public static float controllerStickRightSmoothY;

	public static float controllerStickRightValX;

	public static float controllerStickRightValY;

	public static float controllerTriggerRightValue;

	public static float controllerTriggerLeftValue;

	public static float controllerTriggerRightLastValue;

	public static float controllerTriggerLeftLastValue;

	public static float stickRightX;

	public static float stickRightY;

	public static float stickRightXVel;

	public static float stickRightYVel;

	public static float controlsSlowDownTimer;

	public static bool[] savedSettingsBool;

	public static byte[] savedSettingsByte;

	public static float[] savedSettingsFloat;

	public static float refreshTimer;

	public static float[] lookSensitivity;

	public static float rumbleLow;

	public static float rumbleHigh;

	public static float xRotOri;

	public static float zRotOri;

	public static float dPadRightRepeat;

	public static float dPadLeftRepeat;

	public static float dPadUpRepeat;

	public static float dPadDownRepeat;

	public MouseState current_mouse;

	public GamePadState[] gamePadStates = new GamePadState[4];

	public GamePadDeadZone gamePadDeadZone;

	private Game1.MasterCollection mainC;

	public void Init_Vars(Game1.MasterCollection masterC)
	{
		mainC = masterC;
		gamePadDeadZone = GamePadDeadZone.Circular;
	}

	public void Handle_Resize()
	{
		global::MainGame.MainGame.width = mainC.curGame.GraphicsDevice.Viewport.Width;
		global::MainGame.MainGame.height = mainC.curGame.GraphicsDevice.Viewport.Height;
		global::MainGame.MainGame.safeWidth = mainC.curGame.GraphicsDevice.Viewport.TitleSafeArea.Width;
		global::MainGame.MainGame.safeHeight = mainC.curGame.GraphicsDevice.Viewport.TitleSafeArea.Height;
		global::MainGame.MainGame.safeX = mainC.curGame.GraphicsDevice.Viewport.TitleSafeArea.X;
		global::MainGame.MainGame.safeY = mainC.curGame.GraphicsDevice.Viewport.TitleSafeArea.Y;
		mainC.renderingMain.Handle_Screen_Resize();
	}

	public void Show_Mouse()
	{
		mouseVisible = true;
		mainC.MC_Show_Mouse();
	}

	public void Hide_Mouse()
	{
		mainC.MC_Hide_Mouse();
		mouseVisible = false;
	}

	public void Handle_Window_Activation()
	{
	}

	public void Handle_Window_Deactivation()
	{
	}

	public void Handle_Game_Input(ref KeyboardState newState, ref KeyboardState oldState)
	{
		if (newState.IsKeyDown(Keys.F8) && oldState.IsKeyUp(Keys.F8))
		{
			mainC.renderingMain.Set_Fog_Enabled(!global::Rendering.Rendering.fogEnabled);
			global::MainGame.MainGame.needToSavePlayerSettings = true;
			Console.WriteLine("Distance fog: " + (global::Rendering.Rendering.fogEnabled ? "ON" : "OFF"));
		}
		if (global::MainGame.MainGame.debugRestart > 0)
		{
			global::MainGame.MainGame.debugRestart = 0;
			global::MainGame.MainGame.debugRenderCrashCount = 0;
			global::MainGame.MainGame.debugUpdateCrashCount = 0;
			global::MainGame.MainGame.gameState = 1;
			global::Rendering.Rendering.renderMenuScreen = 1;
			mainC.networkingMain.XBOX_Close_Session();
			mainC.inputMain.UI_Remove_All_Players_From_HUD();
			mainC.userInterface.Load_Main_Menu();
		}
		global::Networking.Networking.wasHost = global::Networking.Networking.isHost;
		if (global::MainGame.MainGame.trialMode)
		{
			global::MainGame.MainGame.trialMode = Guide.IsTrialMode;
			if (!global::MainGame.MainGame.trialMode)
			{
				global::Rendering.Rendering.mbTrialOver = false;
				mainC.gameLogic.Game_Update_Windows_For_Purchase();
			}
		}
		if (global::Networking.Networking.networkSession == null || global::Networking.Networking.networkSession.IsDisposed)
		{
			global::MainGame.MainGame.localNetworkGamerID = 4;
			global::Networking.Networking.networkSessionReady = false;
			global::Networking.Networking.isHost = false;
			global::Networking.Networking.networkState = 0;
			global::Networking.Networking.inGame = false;
		}
		else
		{
			try
			{
				global::Networking.Networking.networkSession.Update();
			}
			catch (Exception)
			{
				mainC.playersMain.Sync_Network_Session_Players();
			}
			global::Networking.Networking.networkSessionReady = true;
			global::Networking.Networking.isHost = global::Networking.Networking.networkSession.IsHost;
			switch (global::Networking.Networking.networkSession.SessionState)
			{
			case NetworkSessionState.Lobby:
				global::Networking.Networking.networkState = 1;
				break;
			case NetworkSessionState.Playing:
				global::Networking.Networking.networkState = 2;
				break;
			case NetworkSessionState.Ended:
				global::Networking.Networking.networkState = 3;
				break;
			}
			Networked_Gamer_Check();
		}
		controllerButtonAPressed = false;
		controllerButtonBPressed = false;
		controllerButtonXPressed = false;
		controllerButtonYPressed = false;
		controllerDPadUpPressed = false;
		controllerDPadDownPressed = false;
		controllerDPadLeftPressed = false;
		controllerDPadRightPressed = false;
		controllerButtonStartPressed = false;
		controllerButtonBackPressed = false;
		controllerButtonLeftShoulderPressed = false;
		controllerButtonRightShoulderPressed = false;
		controllerTriggerRightPressed = false;
		controllerTriggerLeftPressed = false;
		controllerStickButtonRightPressed = false;
		controllerStickButtonLeftPressed = false;
		controllerStickLeftValueX = 0f;
		controllerStickLeftValueY = 0f;
		controllerStickRightValueX = 0f;
		controllerStickRightValueY = 0f;
		if (global::MainGame.MainGame.gameState == 0)
		{
			ref GamePadState reference = ref gamePadStates[0];
			reference = GamePad.GetState(PlayerIndex.One, gamePadDeadZone);
			ref GamePadState reference2 = ref gamePadStates[1];
			reference2 = GamePad.GetState(PlayerIndex.Two, gamePadDeadZone);
			ref GamePadState reference3 = ref gamePadStates[2];
			reference3 = GamePad.GetState(PlayerIndex.Three, gamePadDeadZone);
			ref GamePadState reference4 = ref gamePadStates[3];
			reference4 = GamePad.GetState(PlayerIndex.Four, gamePadDeadZone);
			if ((gamePadStates[0].IsConnected && gamePadStates[0].Buttons.A == ButtonState.Pressed) || (gamePadStates[1].IsConnected && gamePadStates[1].Buttons.A == ButtonState.Pressed) || (gamePadStates[2].IsConnected && gamePadStates[2].Buttons.A == ButtonState.Pressed) || (gamePadStates[3].IsConnected && gamePadStates[3].Buttons.A == ButtonState.Pressed))
			{
				if (!controllerButtonA)
				{
					controllerButtonAPressed = true;
				}
				controllerButtonA = true;
			}
			else
			{
				controllerButtonA = false;
			}
			return;
		}
		if (Guide.IsVisible)
		{
			mainC.maingameMain.Player_Opened_Guide();
		}
		global::Players.Players.moving = 0;
		global::Players.Players.playerSpeedSideways = 0f;
		global::Players.Players.playerSpeed = 0f;
		global::Players.Players.playerSpeedRotateRightStick = 0f;
		global::Players.Players.playerSpeedRotateLeftStick = 0f;
		global::Players.Players.playerSpeedElevateRightStick = 0f;
		global::MainGame.MainGame.walking = false;
		global::MainGame.MainGame.walkingBackwards = false;
		global::MainGame.MainGame.sideStepping = false;
		_ = Guide.IsVisible;
		switch (gpadID)
		{
		case 4:
		{
			controllerTriggerLeft = false;
			controllerTriggerRight = false;
			controllerTriggerLeftValue = 0f;
			controllerTriggerRightValue = 0f;
			global::MainGame.MainGame.storageDeviceNotChosen = false;
			standardController = true;
			for (byte b = 0; b < 4; b++)
			{
				switch (b)
				{
				case 0:
				{
					ref GamePadState reference9 = ref gamePadStates[b];
					reference9 = GamePad.GetState(PlayerIndex.One, gamePadDeadZone);
					standardController = GamePad.GetCapabilities(PlayerIndex.One).GamePadType == GamePadType.GamePad;
					break;
				}
				case 1:
				{
					ref GamePadState reference8 = ref gamePadStates[b];
					reference8 = GamePad.GetState(PlayerIndex.Two, gamePadDeadZone);
					standardController = GamePad.GetCapabilities(PlayerIndex.Two).GamePadType == GamePadType.GamePad;
					break;
				}
				case 2:
				{
					ref GamePadState reference7 = ref gamePadStates[b];
					reference7 = GamePad.GetState(PlayerIndex.Three, gamePadDeadZone);
					standardController = GamePad.GetCapabilities(PlayerIndex.Three).GamePadType == GamePadType.GamePad;
					break;
				}
				case 3:
				{
					ref GamePadState reference6 = ref gamePadStates[b];
					reference6 = GamePad.GetState(PlayerIndex.Four, gamePadDeadZone);
					standardController = GamePad.GetCapabilities(PlayerIndex.Four).GamePadType == GamePadType.GamePad;
					break;
				}
				}
				if (GamePadActive(ref gamePadStates[b]))
				{
					gpadID = b;
					Get_Controller_Input();
					global::MainGame.MainGame.needToLoadPlayerSettings = true;
					break;
				}
			}
			break;
		}
		case 0:
		case 1:
		case 2:
		case 3:
		{
			ref GamePadState reference5 = ref gamePadStates[gpadID];
			reference5 = GamePad.GetState((PlayerIndex)gpadID, gamePadDeadZone);
			if (gamePadStates[gpadID].IsConnected)
			{
				Get_Controller_Input();
				lastGpadID = gpadID;
			}
			else
			{
				mainC.maingameMain.Controller_Disconnected();
				global::MainGame.MainGame.storageDeviceNotChosen = false;
			}
			break;
		}
		}
		Signed_In_Gamer_Check(forceReset: false);
		if (checkForOtherControllers && gpadID < 4)
		{
			byte b2 = gpadID;
			for (byte b = 0; b < 4; b++)
			{
				if (b != gpadID)
				{
					standardController = true;
					switch (b)
					{
					case 0:
					{
						ref GamePadState reference13 = ref gamePadStates[0];
						reference13 = GamePad.GetState(PlayerIndex.One, gamePadDeadZone);
						standardController = GamePad.GetCapabilities(PlayerIndex.One).GamePadType == GamePadType.GamePad;
						break;
					}
					case 1:
					{
						ref GamePadState reference12 = ref gamePadStates[1];
						reference12 = GamePad.GetState(PlayerIndex.Two, gamePadDeadZone);
						standardController = GamePad.GetCapabilities(PlayerIndex.Two).GamePadType == GamePadType.GamePad;
						break;
					}
					case 2:
					{
						ref GamePadState reference11 = ref gamePadStates[2];
						reference11 = GamePad.GetState(PlayerIndex.Three, gamePadDeadZone);
						standardController = GamePad.GetCapabilities(PlayerIndex.Three).GamePadType == GamePadType.GamePad;
						break;
					}
					case 3:
					{
						ref GamePadState reference10 = ref gamePadStates[3];
						reference10 = GamePad.GetState(PlayerIndex.Four, gamePadDeadZone);
						standardController = GamePad.GetCapabilities(PlayerIndex.Four).GamePadType == GamePadType.GamePad;
						break;
					}
					}
					if (gamePadStates[b].IsConnected)
					{
						secondCtrollerAButtonPressed[b] = false;
						secondCtrollerBButtonPressed[b] = false;
						secondCtrollerDPadDownPressed[b] = false;
						secondCtrollerDPadUpPressed[b] = false;
						secondCtrollerDPadLeftPressed[b] = false;
						secondCtrollerDPadRightPressed[b] = false;
						secondCtrollerSticks[b] = false;
						if (gamePadStates[b].Buttons.A == ButtonState.Pressed)
						{
							if (!secondCtrollerAPress[b])
							{
								secondCtrollerAButtonPressed[b] = true;
							}
							secondCtrollerAPress[b] = true;
							b2 = b;
						}
						else
						{
							secondCtrollerAPress[b] = false;
						}
						if (gamePadStates[b].Buttons.B == ButtonState.Pressed)
						{
							if (!secondCtrollerBPress[b])
							{
								secondCtrollerBButtonPressed[b] = true;
							}
							secondCtrollerBPress[b] = true;
							b2 = b;
						}
						else
						{
							secondCtrollerAPress[b] = false;
						}
						if (standardController)
						{
							if (Math.Abs(gamePadStates[b].ThumbSticks.Left.X) > 0.25f || Math.Abs(gamePadStates[b].ThumbSticks.Left.Y) > 0.25f || Math.Abs(gamePadStates[b].ThumbSticks.Right.X) > 0.25f || Math.Abs(gamePadStates[b].ThumbSticks.Right.Y) > 0.25f)
							{
								if (!secondCtrollerSticks[b])
								{
									b2 = b;
								}
								secondCtrollerSticks[b] = true;
								if (!swapSticks)
								{
									controllerStickLeftValueX = gamePadStates[b].ThumbSticks.Left.X;
									controllerStickLeftValueY = gamePadStates[b].ThumbSticks.Left.Y;
									controllerStickRightValueX = gamePadStates[b].ThumbSticks.Right.X;
									controllerStickRightValueY = gamePadStates[b].ThumbSticks.Right.Y;
								}
								else
								{
									controllerStickLeftValueX = gamePadStates[b].ThumbSticks.Right.X;
									controllerStickLeftValueY = gamePadStates[b].ThumbSticks.Right.Y;
									controllerStickRightValueX = gamePadStates[b].ThumbSticks.Left.X;
									controllerStickRightValueY = gamePadStates[b].ThumbSticks.Left.Y;
								}
							}
							else
							{
								secondCtrollerSticks[b] = false;
							}
						}
						if (gamePadStates[b].DPad.Up == ButtonState.Pressed)
						{
							secondCtrollerDPadRepeat[b] += global::MainGame.MainGame.frametime;
							if (!secondCtrollerDPadUp[b] || secondCtrollerDPadRepeat[b] > 0.13f)
							{
								secondCtrollerDPadUpPressed[b] = true;
								secondCtrollerDPadRepeat[b] = 0f;
							}
							secondCtrollerDPadUp[b] = true;
							b2 = b;
						}
						else
						{
							secondCtrollerDPadUp[b] = false;
							secondCtrollerDPadRepeat[b] = 0f;
						}
						if (gamePadStates[b].DPad.Down == ButtonState.Pressed)
						{
							secondCtrollerDPadRepeat[b] += global::MainGame.MainGame.frametime;
							if (!secondCtrollerDPadDown[b] || secondCtrollerDPadRepeat[b] > 0.13f)
							{
								secondCtrollerDPadUpPressed[b] = true;
								secondCtrollerDPadRepeat[b] = 0f;
							}
							secondCtrollerDPadDown[b] = true;
							b2 = b;
						}
						else
						{
							secondCtrollerDPadDown[b] = false;
							secondCtrollerDPadRepeat[b] = 0f;
						}
						if (gamePadStates[b].DPad.Left == ButtonState.Pressed)
						{
							secondCtrollerDPadRepeat[b] += global::MainGame.MainGame.frametime;
							if (!secondCtrollerDPadLeft[b] || secondCtrollerDPadRepeat[b] > 0.13f)
							{
								secondCtrollerDPadLeftPressed[b] = true;
								secondCtrollerDPadRepeat[b] = 0f;
							}
							secondCtrollerDPadLeft[b] = true;
							b2 = b;
						}
						else
						{
							secondCtrollerDPadLeft[b] = false;
							secondCtrollerDPadRepeat[b] = 0f;
						}
						if (gamePadStates[b].DPad.Right == ButtonState.Pressed)
						{
							secondCtrollerDPadRepeat[b] += global::MainGame.MainGame.frametime;
							if (!secondCtrollerDPadRight[b] || secondCtrollerDPadRepeat[b] > 0.13f)
							{
								secondCtrollerDPadRightPressed[b] = true;
								secondCtrollerDPadRepeat[b] = 0f;
							}
							secondCtrollerDPadRight[b] = true;
							b2 = b;
						}
						else
						{
							secondCtrollerDPadRight[b] = false;
							secondCtrollerDPadRepeat[b] = 0f;
						}
					}
				}
			}
			if (b2 != gpadID && b2 < 4)
			{
				gpadID = b2;
				global::MainGame.MainGame.localNetworkGamerID = 4;
				global::MainGame.MainGame.deviceGamer = null;
				global::MainGame.MainGame.playerSettingsLoaded = false;
				global::MainGame.MainGame.needToLoadPlayerSettings = true;
				global::MainGame.MainGame.storageDeviceNotChosen = false;
				controllerButtonAPressed = false;
				controllerButtonBPressed = false;
				controllerButtonXPressed = false;
				controllerButtonYPressed = false;
				controllerButtonStartPressed = false;
				controllerButtonBackPressed = false;
				controllerButtonLeftShoulderPressed = false;
				controllerButtonRightShoulderPressed = false;
				controllerDPadDownPressed = false;
				controllerDPadLeftPressed = false;
				controllerDPadRightPressed = false;
				controllerDPadUpPressed = false;
				if (secondCtrollerAButtonPressed[gpadID])
				{
					controllerButtonA = true;
					controllerButtonAPressed = true;
				}
				else if (secondCtrollerBButtonPressed[gpadID])
				{
					controllerButtonB = true;
					controllerButtonBPressed = true;
				}
				else if (secondCtrollerDPadDownPressed[gpadID])
				{
					controllerDPadDown = true;
					controllerDPadDownPressed = true;
				}
				else if (secondCtrollerDPadUpPressed[gpadID])
				{
					controllerDPadUp = true;
					controllerDPadUpPressed = true;
				}
				else if (secondCtrollerDPadLeftPressed[gpadID])
				{
					controllerDPadLeft = true;
					controllerDPadLeftPressed = true;
				}
				else if (secondCtrollerDPadRightPressed[gpadID])
				{
					controllerDPadRight = true;
					controllerDPadRightPressed = true;
				}
				Networked_Gamer_Check();
				Signed_In_Gamer_Check(forceReset: true);
			}
		}
		if (gpadID >= 4)
		{
			mainC.maingameMain.Controller_Disconnected();
		}
		if (global::MainGame.MainGame.gameState > 0)
		{
			mainC.userInterface.Process_Window_Input(global::MainGame.MainGame.frametime);
		}
	}

	public void Get_Controller_Input()
	{
		GamePad_Vibration_Update();
		if (gamePadStates[gpadID].Buttons.X == ButtonState.Pressed)
		{
			if (!controllerButtonX)
			{
				controllerButtonXPressed = true;
			}
			controllerButtonX = true;
		}
		else
		{
			controllerButtonX = false;
		}
		if (gamePadStates[gpadID].Buttons.Y == ButtonState.Pressed)
		{
			if (!controllerButtonY)
			{
				controllerButtonYPressed = true;
			}
			controllerButtonY = true;
		}
		else
		{
			controllerButtonY = false;
		}
		if (gamePadStates[gpadID].Buttons.A == ButtonState.Pressed)
		{
			if (!controllerButtonA)
			{
				controllerButtonAPressed = true;
			}
			controllerButtonA = true;
		}
		else
		{
			controllerButtonA = false;
		}
		if (gamePadStates[gpadID].Buttons.B == ButtonState.Pressed)
		{
			if (!controllerButtonB)
			{
				controllerButtonBPressed = true;
			}
			controllerButtonB = true;
		}
		else
		{
			controllerButtonB = false;
		}
		if (gamePadStates[gpadID].Buttons.Start == ButtonState.Pressed)
		{
			if (!controllerButtonStart)
			{
				controllerButtonStartPressed = true;
			}
			controllerButtonStart = true;
		}
		else
		{
			controllerButtonStart = false;
		}
		if (gamePadStates[gpadID].Buttons.Back == ButtonState.Pressed)
		{
			if (!controllerButtonBack)
			{
				controllerButtonBackPressed = true;
			}
			controllerButtonBack = true;
		}
		else
		{
			controllerButtonBack = false;
		}
		if (!swapSticks)
		{
			if (gamePadStates[gpadID].Buttons.RightStick == ButtonState.Pressed)
			{
				if (!controllerStickButtonRight)
				{
					controllerStickButtonRightPressed = true;
				}
				controllerStickButtonRight = true;
			}
			else
			{
				controllerStickButtonRight = false;
			}
		}
		else if (gamePadStates[gpadID].Buttons.LeftStick == ButtonState.Pressed)
		{
			if (!controllerStickButtonRight)
			{
				controllerStickButtonRightPressed = true;
			}
			controllerStickButtonRight = true;
		}
		else
		{
			controllerStickButtonRight = false;
		}
		if (!swapSticks)
		{
			if (gamePadStates[gpadID].Buttons.LeftStick == ButtonState.Pressed)
			{
				if (!controllerStickButtonLeft)
				{
					controllerStickButtonLeftPressed = true;
				}
				controllerStickButtonLeft = true;
			}
			else
			{
				controllerStickButtonLeft = false;
			}
		}
		else if (gamePadStates[gpadID].Buttons.RightStick == ButtonState.Pressed)
		{
			if (!controllerStickButtonLeft)
			{
				controllerStickButtonLeftPressed = true;
			}
			controllerStickButtonLeft = true;
		}
		else
		{
			controllerStickButtonLeft = false;
		}
		controllerTriggerRightValue = gamePadStates[gpadID].Triggers.Right;
		if (controllerTriggerRightValue > 0.15f)
		{
			if (!controllerTriggerRight)
			{
				controllerTriggerRightPressed = true;
			}
			controllerTriggerRight = true;
		}
		else
		{
			controllerTriggerRight = false;
		}
		controllerTriggerLeftValue = gamePadStates[gpadID].Triggers.Left;
		if (controllerTriggerLeftValue > 0.15f)
		{
			if (!controllerTriggerLeft)
			{
				controllerTriggerLeftPressed = true;
			}
			controllerTriggerLeft = true;
		}
		else
		{
			controllerTriggerLeft = false;
		}
		if (gamePadStates[gpadID].Buttons.RightShoulder == ButtonState.Pressed)
		{
			if (!controllerButtonRightShoulder)
			{
				controllerButtonRightShoulderPressed = true;
			}
			controllerButtonRightShoulder = true;
		}
		else
		{
			controllerButtonRightShoulder = false;
		}
		if (gamePadStates[gpadID].Buttons.LeftShoulder == ButtonState.Pressed)
		{
			if (!controllerButtonLeftShoulder)
			{
				controllerButtonLeftShoulderPressed = true;
			}
			controllerButtonLeftShoulder = true;
		}
		else
		{
			controllerButtonLeftShoulder = false;
		}
		if (!swapSticks)
		{
			controllerStickLeftValueX = gamePadStates[gpadID].ThumbSticks.Left.X;
			controllerStickLeftValueY = gamePadStates[gpadID].ThumbSticks.Left.Y;
			controllerStickRightValueX = gamePadStates[gpadID].ThumbSticks.Right.X;
			controllerStickRightValueY = gamePadStates[gpadID].ThumbSticks.Right.Y;
		}
		else
		{
			controllerStickLeftValueX = gamePadStates[gpadID].ThumbSticks.Right.X;
			controllerStickLeftValueY = gamePadStates[gpadID].ThumbSticks.Right.Y;
			controllerStickRightValueX = gamePadStates[gpadID].ThumbSticks.Left.X;
			controllerStickRightValueY = gamePadStates[gpadID].ThumbSticks.Left.Y;
		}
		if (gamePadStates[gpadID].DPad.Up == ButtonState.Pressed)
		{
			dPadUpRepeat += global::MainGame.MainGame.frametime;
			if (!controllerDPadUp || dPadUpRepeat > 0.13f)
			{
				controllerDPadUpPressed = true;
				dPadUpRepeat = 0f;
			}
			controllerDPadUp = true;
		}
		else
		{
			controllerDPadUp = false;
			dPadUpRepeat = 0f;
		}
		if (gamePadStates[gpadID].DPad.Down == ButtonState.Pressed)
		{
			dPadDownRepeat += global::MainGame.MainGame.frametime;
			if (!controllerDPadDown || dPadDownRepeat > 0.13f)
			{
				controllerDPadDownPressed = true;
				dPadDownRepeat = 0f;
			}
			controllerDPadDown = true;
		}
		else
		{
			controllerDPadDown = false;
			dPadDownRepeat = 0f;
		}
		if (gamePadStates[gpadID].DPad.Left == ButtonState.Pressed)
		{
			dPadLeftRepeat += global::MainGame.MainGame.frametime;
			if (!controllerDPadLeft || dPadLeftRepeat > 0.13f)
			{
				controllerDPadLeftPressed = true;
				dPadLeftRepeat = 0f;
			}
			controllerDPadLeft = true;
		}
		else
		{
			controllerDPadLeft = false;
			dPadLeftRepeat = 0f;
		}
		if (gamePadStates[gpadID].DPad.Right == ButtonState.Pressed)
		{
			dPadRightRepeat += global::MainGame.MainGame.frametime;
			if (!controllerDPadRight || dPadRightRepeat > 0.13f)
			{
				controllerDPadRightPressed = true;
				dPadRightRepeat = 0f;
			}
			controllerDPadRight = true;
		}
		else
		{
			controllerDPadRight = false;
			dPadRightRepeat = 0f;
		}
		if (Math.Abs(controllerStickLastRightValueX - controllerStickRightValueX) > 0.2f || Math.Abs(controllerStickLastRightValueY - controllerStickRightValueY) > 0.2f || Math.Abs(controllerStickLastLeftValueX - controllerStickLeftValueX) > 0.2f || Math.Abs(controllerStickLastLeftValueY - controllerStickLeftValueY) > 0.2f || Math.Abs(controllerTriggerRightLastValue - controllerTriggerRightValue) > 0.2f || Math.Abs(controllerTriggerLeftLastValue - controllerTriggerLeftValue) > 0.2f || controllerButtonAPressed || controllerButtonBPressed || controllerButtonXPressed || controllerButtonYPressed || controllerButtonStartPressed || controllerButtonBackPressed || controllerButtonLeftShoulderPressed || controllerButtonRightShoulderPressed || controllerDPadDownPressed || controllerDPadUpPressed || controllerDPadRightPressed || controllerDPadLeftPressed)
		{
			global::MainGame.MainGame.curIdleTime = 0f;
		}
		controllerStickLastLeftValueX = controllerStickLeftValueX;
		controllerStickLastLeftValueY = controllerStickLeftValueY;
		controllerStickLastRightValueX = controllerStickRightValueX;
		controllerStickLastRightValueY = controllerStickRightValueY;
		controllerTriggerLeftLastValue = controllerTriggerLeftValue;
		controllerTriggerRightLastValue = controllerTriggerRightValue;
	}

	public void Reset_Second_Controller_Checks()
	{
		checkForOtherControllers = true;
		for (ushort num = 0; num < 4; num++)
		{
			secondCtrollerAPress[num] = false;
			secondCtrollerBPress[num] = false;
			secondCtrollerDPadUp[num] = false;
			secondCtrollerDPadDown[num] = false;
			secondCtrollerDPadLeft[num] = false;
			secondCtrollerDPadRight[num] = false;
			secondCtrollerDPadRepeat[num] = 0f;
			secondCtrollerSticks[num] = false;
		}
	}

	public void Signed_In_Gamer_Check(bool forceReset)
	{
		int signedinGamerID = global::MainGame.MainGame.signedinGamerID;
		global::MainGame.MainGame.signedinGamerID = -1;
		if (gpadID < 4)
		{
			ushort num = 0;
			while (num < Gamer.SignedInGamers.Count && global::MainGame.MainGame.signedinGamerID < 0)
			{
				switch (gpadID)
				{
				case 0:
					if (Gamer.SignedInGamers[num].PlayerIndex != PlayerIndex.One)
					{
						break;
					}
					global::MainGame.MainGame.signedinGamerID = num;
					if (num != signedinGamerID)
					{
						forceReset = true;
					}
					if (forceReset && !ySetFromFile)
					{
						global::Players.Players.invertY = 1f;
						global::Players.Players.invertYSecondary = 1f;
						if (Gamer.SignedInGamers[num].GameDefaults.InvertYAxis)
						{
							global::Players.Players.invertY = -1f;
							global::Players.Players.invertYSecondary = -1f;
						}
					}
					try
					{
						if (global::Players.Players.players[0].username.Length < 1 || forceReset)
						{
							global::Players.Players.players[0].username = Gamer.SignedInGamers[PlayerIndex.One].Gamertag;
							mainC.playersMain.Set_Player_Abbreviated_Name(0);
						}
					}
					catch
					{
						global::Players.Players.players[0].username = "Gamer_0";
						mainC.playersMain.Set_Player_Abbreviated_Name(0);
					}
					break;
				case 1:
					if (Gamer.SignedInGamers[num].PlayerIndex != PlayerIndex.Two)
					{
						break;
					}
					global::MainGame.MainGame.signedinGamerID = num;
					if (num != signedinGamerID)
					{
						forceReset = true;
					}
					if (forceReset && !ySetFromFile)
					{
						global::Players.Players.invertY = 1f;
						global::Players.Players.invertYSecondary = 1f;
						if (Gamer.SignedInGamers[num].GameDefaults.InvertYAxis)
						{
							global::Players.Players.invertY = -1f;
							global::Players.Players.invertYSecondary = -1f;
						}
					}
					try
					{
						if (global::Players.Players.players[0].username.Length < 1 || forceReset)
						{
							global::Players.Players.players[0].username = Gamer.SignedInGamers[PlayerIndex.Two].Gamertag;
							mainC.playersMain.Set_Player_Abbreviated_Name(0);
						}
					}
					catch
					{
						global::Players.Players.players[0].username = "Gamer_0";
						mainC.playersMain.Set_Player_Abbreviated_Name(0);
					}
					break;
				case 2:
					if (Gamer.SignedInGamers[num].PlayerIndex != PlayerIndex.Three)
					{
						break;
					}
					global::MainGame.MainGame.signedinGamerID = num;
					if (num != signedinGamerID)
					{
						forceReset = true;
					}
					if (forceReset && !ySetFromFile)
					{
						global::Players.Players.invertY = 1f;
						global::Players.Players.invertYSecondary = 1f;
						if (Gamer.SignedInGamers[num].GameDefaults.InvertYAxis)
						{
							global::Players.Players.invertY = -1f;
							global::Players.Players.invertYSecondary = -1f;
						}
					}
					try
					{
						if (global::Players.Players.players[0].username.Length < 1 || forceReset)
						{
							global::Players.Players.players[0].username = Gamer.SignedInGamers[PlayerIndex.Three].Gamertag;
							mainC.playersMain.Set_Player_Abbreviated_Name(0);
						}
					}
					catch
					{
						global::Players.Players.players[0].username = "Gamer_0";
						mainC.playersMain.Set_Player_Abbreviated_Name(0);
					}
					break;
				case 3:
					if (Gamer.SignedInGamers[num].PlayerIndex != PlayerIndex.Four)
					{
						break;
					}
					global::MainGame.MainGame.signedinGamerID = num;
					if (num != signedinGamerID)
					{
						forceReset = true;
					}
					if (forceReset && !ySetFromFile)
					{
						global::Players.Players.invertY = 1f;
						global::Players.Players.invertYSecondary = 1f;
						if (Gamer.SignedInGamers[num].GameDefaults.InvertYAxis)
						{
							global::Players.Players.invertY = -1f;
							global::Players.Players.invertYSecondary = -1f;
						}
					}
					try
					{
						if (global::Players.Players.players[0].username.Length < 1 || forceReset)
						{
							global::Players.Players.players[0].username = Gamer.SignedInGamers[PlayerIndex.Four].Gamertag;
							mainC.playersMain.Set_Player_Abbreviated_Name(0);
						}
					}
					catch
					{
						global::Players.Players.players[0].username = "Gamer_0";
						mainC.playersMain.Set_Player_Abbreviated_Name(0);
					}
					break;
				}
				num++;
			}
		}
		if (signedinGamerID != global::MainGame.MainGame.signedinGamerID)
		{
			global::MainGame.MainGame.deviceGamer = null;
			global::MainGame.MainGame.playerSettingsLoaded = false;
			global::MainGame.MainGame.needToLoadPlayerSettings = true;
		}
	}

	public void Networked_Gamer_Check()
	{
		if (!global::Networking.Networking.networkSessionReady)
		{
			return;
		}
		if (global::MainGame.MainGame.localNetworkGamerID < 4 && global::MainGame.MainGame.localNetworkGamerID >= global::Networking.Networking.networkSession.LocalGamers.Count)
		{
			global::MainGame.MainGame.localNetworkGamerID = 4;
		}
		switch (gpadID)
		{
		case 0:
		{
			if (global::MainGame.MainGame.localNetworkGamerID != 4 && global::Networking.Networking.networkSession.LocalGamers[global::MainGame.MainGame.localNetworkGamerID].SignedInGamer.PlayerIndex == PlayerIndex.One)
			{
				break;
			}
			global::MainGame.MainGame.localNetworkGamerID = 4;
			byte b = (byte)global::Networking.Networking.networkSession.LocalGamers.Count;
			for (byte b2 = 0; b2 < b; b2++)
			{
				if (global::Networking.Networking.networkSession.LocalGamers[b2].SignedInGamer.PlayerIndex == PlayerIndex.One)
				{
					global::MainGame.MainGame.localNetworkGamerID = b2;
					break;
				}
			}
			break;
		}
		case 1:
		{
			if (global::MainGame.MainGame.localNetworkGamerID != 4 && global::Networking.Networking.networkSession.LocalGamers[global::MainGame.MainGame.localNetworkGamerID].SignedInGamer.PlayerIndex == PlayerIndex.Two)
			{
				break;
			}
			global::MainGame.MainGame.localNetworkGamerID = 4;
			byte b = (byte)global::Networking.Networking.networkSession.LocalGamers.Count;
			for (byte b2 = 0; b2 < b; b2++)
			{
				if (global::Networking.Networking.networkSession.LocalGamers[b2].SignedInGamer.PlayerIndex == PlayerIndex.Two)
				{
					global::MainGame.MainGame.localNetworkGamerID = b2;
					break;
				}
			}
			break;
		}
		case 2:
		{
			if (global::MainGame.MainGame.localNetworkGamerID != 4 && global::Networking.Networking.networkSession.LocalGamers[global::MainGame.MainGame.localNetworkGamerID].SignedInGamer.PlayerIndex == PlayerIndex.Three)
			{
				break;
			}
			global::MainGame.MainGame.localNetworkGamerID = 4;
			byte b = (byte)global::Networking.Networking.networkSession.LocalGamers.Count;
			for (byte b2 = 0; b2 < b; b2++)
			{
				if (global::Networking.Networking.networkSession.LocalGamers[b2].SignedInGamer.PlayerIndex == PlayerIndex.Three)
				{
					global::MainGame.MainGame.localNetworkGamerID = b2;
					break;
				}
			}
			break;
		}
		case 3:
		{
			if (global::MainGame.MainGame.localNetworkGamerID != 4 && global::Networking.Networking.networkSession.LocalGamers[global::MainGame.MainGame.localNetworkGamerID].SignedInGamer.PlayerIndex == PlayerIndex.Four)
			{
				break;
			}
			global::MainGame.MainGame.localNetworkGamerID = 4;
			byte b = (byte)global::Networking.Networking.networkSession.LocalGamers.Count;
			for (byte b2 = 0; b2 < b; b2++)
			{
				if (global::Networking.Networking.networkSession.LocalGamers[b2].SignedInGamer.PlayerIndex == PlayerIndex.Four)
				{
					global::MainGame.MainGame.localNetworkGamerID = b2;
					break;
				}
			}
			break;
		}
		}
		if (global::MainGame.MainGame.localNetworkGamerID >= global::Networking.Networking.networkSession.LocalGamers.Count)
		{
			global::MainGame.MainGame.localNetworkGamerID = 0;
		}
	}

	public void GamePad_Vibration_Update()
	{
		if (rumbleHigh <= 0f)
		{
			rumbleHigh = 0f;
		}
		if (rumbleLow <= 0f)
		{
			rumbleLow = 0f;
		}
		switch (gpadID)
		{
		case 0:
		case 1:
		case 2:
		case 3:
			if (gamePadStates[gpadID].IsConnected)
			{
				GamePad.SetVibration((PlayerIndex)gpadID, rumbleLow * (float)(int)rumble, rumbleHigh * (float)(int)rumble);
			}
			break;
		}
		rumbleHigh -= global::MainGame.MainGame.frametime * 4f;
		rumbleLow -= global::MainGame.MainGame.frametime * 4f;
	}

	public void GamePad_Vibration_Set_Both(float low, float high)
	{
		if (rumbleLow < low)
		{
			rumbleLow = low;
		}
		if (rumbleHigh < high)
		{
			rumbleHigh = high;
		}
	}

	public void GamePad_Vibration_Set_Low(float low)
	{
		if (rumbleLow < low)
		{
			rumbleLow = low;
		}
	}

	public void GamePad_Vibration_Set_High(float high)
	{
		if (rumbleHigh < high)
		{
			rumbleHigh = high;
		}
	}

	public void GamePad_Vibration_Stop()
	{
		rumbleLow = 0f;
		rumbleHigh = 0f;
		switch (gpadID)
		{
		case 0:
		case 1:
		case 2:
		case 3:
			if (gamePadStates[gpadID].IsConnected)
			{
				GamePad.SetVibration((PlayerIndex)gpadID, rumbleLow, rumbleHigh);
			}
			break;
		}
	}

	public void Handle_Menu(byte threadID)
	{
		inMenu = true;
		menuChangeItem = 0;
		switch (currentMenu)
		{
		case 0:
			Menu_Main();
			break;
		case 1:
			Menu_Multiplayer(threadID);
			break;
		case 2:
			Menu_Multiplayer_Lobby_GameSettings();
			break;
		case 3:
			Menu_Controller();
			break;
		case 4:
			Menu_SinglePlayer(threadID);
			break;
		case 5:
			Menu_Settings();
			break;
		case 7:
			Menu_Credits();
			break;
		case 8:
			Menu_MultiPlayer_Create_Game(threadID);
			break;
		case 9:
			Menu_Multiplayer_Choose_LiveSystemLink();
			break;
		case 10:
			Menu_InGame_Menu();
			break;
		case 11:
			Menu_Multiplayer_Join_Game(threadID);
			break;
		case 13:
			Menu_Buy_Me();
			break;
		case 14:
			Menu_Instructions();
			break;
		case 15:
			Menu_Brightness();
			break;
		case 6:
		case 12:
		case 16:
		case 17:
			break;
		}
	}

	public void Menu_Main()
	{
		if (menuStat == 4)
		{
			global::Threads.Threads.thread3End.WaitOne();
			mainC.maingameMain.Leaving_Menu_State();
			mainC.inputMain.Leave_Menu_Completely();
			mainC.renderingMain.Rotate_Splash_For_Level_Loading();
		}
		else
		{
			if (menuStat == 3)
			{
				return;
			}
			if (menuStat == 2)
			{
				mainC.gameLogic.Game_SP_Initial_Setup();
				global::Threads.Threads.thread3Task = 5;
				global::Threads.Threads.thread3Start.Set();
				menuStat = 3;
			}
			else if (menuStat == 1)
			{
				switch (mainC.maingameMain.SaveGame_Exists())
				{
				case 1:
					menuStat = 2;
					break;
				case 2:
					menuConfig[0, 2] = 0;
					Switch_To_Menu(4);
					break;
				}
			}
			else if (messageBox_ExitToDash)
			{
				if (controllerButtonAPressed)
				{
					mainC.soundsMain.Play_Sound_NonPositional("ButtonClick");
					if (!global::MainGame.MainGame.trialMode)
					{
						mainC.Quit_Game();
						return;
					}
					messageBox_ExitToDash = false;
					buyMeOnExit = true;
					Switch_To_Menu(13);
				}
				else if (controllerButtonBPressed)
				{
					mainC.soundsMain.Play_Sound_NonPositional("ButtonClick");
					messageBox_ExitToDash = false;
				}
			}
			else if (controllerButtonAPressed)
			{
				switch (menuItem)
				{
				}
			}
		}
	}

	public void Menu_SinglePlayer(byte threadID)
	{
		if (menuStat == 5)
		{
			global::Threads.Threads.thread3End.WaitOne();
			mainC.inputMain.Leave_Menu_Completely();
			mainC.maingameMain.Leaving_Menu_State();
			mainC.renderingMain.Rotate_Splash_For_Level_Loading();
		}
		else
		{
			if (menuStat == 4)
			{
				return;
			}
			if (menuStat == 3)
			{
				mainC.gameLogic.Game_SP_Initial_Setup();
				global::Threads.Threads.thread3Task = 5;
				global::Threads.Threads.thread3Start.Set();
				menuStat = 4;
				return;
			}
			if (menuStat == 2)
			{
				if (mainC.maingameMain.Load_SP_Game())
				{
					menuStat = 3;
				}
				return;
			}
			if (menuStat == 1)
			{
				if (controllerButtonAPressed && global::Rendering.Rendering.mbSP_NewGameOverwrite)
				{
					global::Rendering.Rendering.mbSP_NewGameOverwrite = false;
					menuStat = 2;
					mainC.soundsMain.Play_Sound_NonPositional("ButtonClick");
				}
				else if (controllerButtonBPressed)
				{
					global::Rendering.Rendering.mbSP_NewGameOverwrite = false;
					menuStat = 0;
					mainC.soundsMain.Play_Sound_NonPositional("ButtonClick");
				}
				return;
			}
			if (controllerButtonAPressed)
			{
				switch (menuItem)
				{
				case 0:
					newSPGame = true;
					menuStat = 1;
					global::Rendering.Rendering.mbSP_NewGameOverwrite = true;
					mainC.soundsMain.Play_Sound_NonPositional("ButtonClick");
					break;
				case 1:
					newSPGame = false;
					menuStat = 2;
					mainC.soundsMain.Play_Sound_NonPositional("ButtonClick");
					break;
				case 2:
					mainC.soundsMain.Play_Sound_NonPositional("ButtonClick");
					mainC.userInterface.Load_Main_Menu();
					menuStat = 0;
					break;
				}
			}
			if ((menuChangeItem & 8) > 0)
			{
				menuItem++;
				if (menuItem >= numMenuItems)
				{
					menuItem = 0;
				}
				mainC.soundsMain.Play_Sound_NonPositional("MenuChange");
			}
			else if ((menuChangeItem & 4) > 0)
			{
				byte b = menuItem;
				menuItem--;
				if (menuItem > b)
				{
					menuItem = (byte)(numMenuItems - 1);
				}
				mainC.soundsMain.Play_Sound_NonPositional("MenuChange");
			}
			else if (controllerButtonBPressed || controllerButtonBackPressed || controllerButtonStartPressed || controllerButtonXPressed)
			{
				mainC.soundsMain.Play_Sound_NonPositional("ButtonClick");
				Leave_Menu_Completely();
			}
		}
	}

	public void Menu_Multiplayer(byte threadID)
	{
		if (menuStat == 0)
		{
			controllerButtonAPressed = true;
		}
		if (menuStat == 2)
		{
			if (mpLive)
			{
				mainC.networkingMain.XBOX_Join_First_Session(NetworkSessionType.PlayerMatch);
			}
			else
			{
				mainC.networkingMain.XBOX_Join_First_Session(NetworkSessionType.SystemLink);
			}
		}
		else if (menuStat == 1)
		{
			mainC.renderingMain.Set_Splash(byte.MaxValue);
			mainC.gameLogic.Game_MP_Initial_New_Game_Setup(threadID);
			mainC.networkingMain.XBOX_Close_Session();
			menuStat = 2;
		}
		else if (showMessageBox_MP_Error)
		{
			if (controllerButtonBPressed)
			{
				showMessageBox_MP_Error = false;
				mainC.soundsMain.Play_Sound_NonPositional("ButtonClick");
			}
		}
		else if (showMessageBox_QM_NoGames)
		{
			if (controllerButtonBPressed)
			{
				showMessageBox_QM_NoGames = false;
				mainC.soundsMain.Play_Sound_NonPositional("ButtonClick");
			}
		}
		else
		{
			if (!controllerButtonAPressed)
			{
				return;
			}
			switch (menuItem)
			{
			case 0:
				global::MainGame.MainGame.gameState = 1;
				menuStat = 1;
				global::Rendering.Rendering.mbQM_Searching = true;
				mainC.soundsMain.Play_Sound_NonPositional("ButtonClick");
				break;
			case 1:
				menuConfig[1, 2] = 1;
				mainC.soundsMain.Play_Sound_NonPositional("ButtonClick");
				Switch_To_Menu(8);
				break;
			case 2:
				menuStat = 1;
				menuConfig[1, 2] = 2;
				mainC.soundsMain.Play_Sound_NonPositional("ButtonClick");
				Switch_To_Menu(11);
				global::Rendering.Rendering.mbJoin_Searching = true;
				mpJoinListReady = false;
				break;
			case 3:
				global::Rendering.Rendering.mbQM_Searching = false;
				global::Rendering.Rendering.mbJoin_Searching = false;
				showMessageBox_QM_NoGames = false;
				showMessageBox_MP_Error = false;
				mainC.soundsMain.Play_Sound_NonPositional("ButtonClick");
				Switch_To_Menu(9);
				break;
			}
			if ((menuChangeItem & 8) > 0)
			{
				menuItem++;
				if (menuItem > 3)
				{
					menuItem = 0;
				}
				mainC.soundsMain.Play_Sound_NonPositional("MenuChange");
			}
			else if ((menuChangeItem & 4) > 0)
			{
				menuItem--;
				if (menuItem > 3)
				{
					menuItem = 3;
				}
				mainC.soundsMain.Play_Sound_NonPositional("MenuChange");
			}
			else if (controllerButtonBPressed || controllerButtonBackPressed)
			{
				global::Rendering.Rendering.mbQM_Searching = false;
				global::Rendering.Rendering.mbJoin_Searching = false;
				showMessageBox_QM_NoGames = false;
				showMessageBox_MP_Error = false;
				mainC.soundsMain.Play_Sound_NonPositional("ButtonClick");
				Switch_To_Menu(9);
			}
		}
	}

	public void Menu_Multiplayer_Choose_LiveSystemLink()
	{
		if (controllerButtonAPressed)
		{
			switch (menuItem)
			{
			case 0:
				mpLive = true;
				menuConfig[9, 2] = 0;
				Switch_To_Menu(1);
				break;
			case 1:
				mpLive = false;
				menuConfig[9, 2] = 1;
				Switch_To_Menu(1);
				break;
			case 2:
				mainC.userInterface.Load_Main_Menu();
				break;
			}
			mainC.soundsMain.Play_Sound_NonPositional("ButtonClick");
		}
		if ((menuChangeItem & 8) > 0)
		{
			menuItem++;
			if (menuItem > 2)
			{
				menuItem = 0;
			}
			mainC.soundsMain.Play_Sound_NonPositional("MenuChange");
		}
		else if ((menuChangeItem & 4) > 0)
		{
			menuItem--;
			if (menuItem > 2)
			{
				menuItem = 2;
			}
			mainC.soundsMain.Play_Sound_NonPositional("MenuChange");
		}
		else if (controllerButtonBPressed)
		{
			mainC.soundsMain.Play_Sound_NonPositional("ButtonClick");
			mainC.userInterface.Load_Main_Menu();
		}
		else if (controllerButtonBackPressed)
		{
			mainC.soundsMain.Play_Sound_NonPositional("ButtonClick");
			Leave_Menu_Completely();
		}
	}

	public void Menu_MultiPlayer_Create_Game(byte threadID)
	{
		if (menuStat == 0)
		{
			controllerButtonStartPressed = true;
		}
		if (menuStat == 2)
		{
			if (mpLive)
			{
				mainC.networkingMain.XBOX_Create_Session(NetworkSessionType.PlayerMatch, global::MainGame.MainGame.mpNumPrivateGamerSlots);
			}
			else
			{
				mainC.networkingMain.XBOX_Create_Session(NetworkSessionType.SystemLink, global::MainGame.MainGame.mpNumPrivateGamerSlots);
			}
			return;
		}
		if (menuStat == 1)
		{
			mainC.renderingMain.Set_Splash(byte.MaxValue);
			mainC.gameLogic.Game_MP_Initial_New_Game_Setup(threadID);
			mainC.networkingMain.XBOX_Close_Session();
			menuStat = 2;
			return;
		}
		if (showMessageBox_MP_Error)
		{
			if (controllerButtonBPressed)
			{
				showMessageBox_MP_Error = false;
				mainC.soundsMain.Play_Sound_NonPositional("ButtonClick");
			}
			return;
		}
		if (controllerButtonAPressed)
		{
			if (menuItem < 10)
			{
				global::MainGame.MainGame.mp_numPlayers_index = (byte)(menuItem - 6);
				global::MainGame.MainGame.maxGamePlayers = global::MainGame.MainGame.mp_numPlayers[global::MainGame.MainGame.mp_numPlayers_index];
				if (global::MainGame.MainGame.maxGamePlayers > 4)
				{
					global::MainGame.MainGame.maxGamePlayers = 4;
				}
			}
			else if (menuItem < 14)
			{
				global::MainGame.MainGame.mp_timeLimit_index = (byte)(menuItem - 10);
			}
			else if (menuItem < 15)
			{
				global::MainGame.MainGame.gameLevel--;
				if (global::MainGame.MainGame.gameLevel >= 4)
				{
					short num = 3;
					global::MainGame.MainGame.gameLevel = (byte)num;
				}
			}
			else if (menuItem < 16)
			{
				global::MainGame.MainGame.gameLevel++;
				if (global::MainGame.MainGame.gameLevel >= 4)
				{
					global::MainGame.MainGame.gameLevel = 0;
				}
			}
			mainC.soundsMain.Play_Sound_NonPositional("ButtonClick");
		}
		else if (controllerButtonStartPressed)
		{
			global::MainGame.MainGame.maxGamePlayers = global::MainGame.MainGame.mp_numPlayers[global::MainGame.MainGame.mp_numPlayers_index];
			if (global::MainGame.MainGame.maxGamePlayers > 4)
			{
				global::MainGame.MainGame.maxGamePlayers = 4;
			}
			messageBox_ExitToTitle = false;
			global::MainGame.MainGame.gameState = 1;
			menuStat = 1;
			mainC.soundsMain.Play_Sound_NonPositional("ButtonClick");
		}
		if ((menuChangeItem & 1) > 0)
		{
			menuItem++;
			if (menuItem > 15)
			{
				menuItem = 0;
			}
			mainC.soundsMain.Play_Sound_NonPositional("MenuChange");
		}
		else if ((menuChangeItem & 2) > 0)
		{
			menuItem--;
			if (menuItem > 15)
			{
				menuItem = 15;
			}
			mainC.soundsMain.Play_Sound_NonPositional("MenuChange");
		}
		else if ((menuChangeItem & 4) > 0)
		{
			if (menuItem > 13)
			{
				menuItem = 10;
			}
			else if (menuItem > 9)
			{
				menuItem = 4;
			}
			else
			{
				menuItem = 14;
			}
			mainC.soundsMain.Play_Sound_NonPositional("MenuChange");
		}
		else if ((menuChangeItem & 8) > 0)
		{
			if (menuItem < 10)
			{
				menuItem = 10;
			}
			else if (menuItem < 14)
			{
				menuItem = 14;
			}
			else
			{
				menuItem = 4;
			}
			mainC.soundsMain.Play_Sound_NonPositional("MenuChange");
		}
		else if (controllerButtonBPressed || controllerButtonBackPressed)
		{
			mainC.soundsMain.Play_Sound_NonPositional("ButtonClick");
			Switch_To_Menu(1);
		}
	}

	public void Menu_Multiplayer_Join_Game(byte threadID)
	{
		switch (menuStat)
		{
		case 5:
			refreshTimer += global::MainGame.MainGame.frametime;
			if (refreshTimer > 15f)
			{
				global::Rendering.Rendering.mbJoin_Searching = true;
				menuStat = 2;
			}
			break;
		case 4:
			refreshTimer = 0f;
			menuStat = 5;
			break;
		case 3:
			menuStat = 4;
			break;
		case 2:
			mpJoinListReady = false;
			if (mpLive)
			{
				mainC.networkingMain.XBOX_Get_Game_List(NetworkSessionType.PlayerMatch);
			}
			else
			{
				mainC.networkingMain.XBOX_Get_Game_List(NetworkSessionType.SystemLink);
			}
			break;
		case 1:
			mainC.renderingMain.Set_Splash(byte.MaxValue);
			mainC.gameLogic.Game_MP_Initial_New_Game_Setup(threadID);
			mainC.networkingMain.XBOX_Close_Session();
			menuStat = 2;
			break;
		}
		if (showMessageBox_MP_Error)
		{
			if (controllerButtonBPressed)
			{
				showMessageBox_MP_Error = false;
				mainC.soundsMain.Play_Sound_NonPositional("ButtonClick");
			}
		}
		else if (showMessageBox_QM_NoGames)
		{
			menuStat = 4;
			if (controllerButtonBPressed)
			{
				showMessageBox_QM_NoGames = false;
				mainC.soundsMain.Play_Sound_NonPositional("ButtonClick");
			}
		}
		else if (global::Rendering.Rendering.mbMessageBox_Join_NoGames)
		{
			menuStat = 4;
			if (controllerButtonBPressed)
			{
				mainC.soundsMain.Play_Sound_NonPositional("ButtonClick");
				global::Rendering.Rendering.mbMessageBox_Join_NoGames = false;
			}
		}
		else if (global::Rendering.Rendering.mbJoin_Searching)
		{
			if (controllerButtonBPressed || controllerButtonBackPressed)
			{
				mainC.networkingMain.XBOX_Close_Session();
				mainC.soundsMain.Play_Sound_NonPositional("ButtonClick");
				Leave_Menu_Completely();
			}
		}
		else if (controllerButtonBPressed || controllerButtonBackPressed)
		{
			mainC.soundsMain.Play_Sound_NonPositional("ButtonClick");
			Leave_Menu_Completely();
		}
		else if (controllerButtonStartPressed)
		{
			if (mpLive)
			{
				mainC.networkingMain.XBOX_Join_Session(mpJoinListIndex, NetworkSessionType.PlayerMatch);
			}
			else
			{
				mainC.networkingMain.XBOX_Join_Session(mpJoinListIndex, NetworkSessionType.SystemLink);
			}
			mainC.soundsMain.Play_Sound_NonPositional("ButtonClick");
		}
		else if (controllerButtonYPressed)
		{
			global::Rendering.Rendering.mbJoin_Searching = true;
			menuStat = 2;
			mainC.soundsMain.Play_Sound_NonPositional("ButtonClick");
		}
		else
		{
			if (!mpJoinListReady || global::Networking.Networking.searchSessions == null)
			{
				return;
			}
			if ((menuChangeItem & 8) > 0)
			{
				mpJoinListIndex++;
				if (mpJoinListIndex >= global::Networking.Networking.searchSessions.Count)
				{
					mpJoinListIndex = (byte)(global::Networking.Networking.searchSessions.Count - 1);
				}
				mainC.soundsMain.Play_Sound_NonPositional("MenuChange");
			}
			else if ((menuChangeItem & 4) > 0)
			{
				mpJoinListIndex--;
				if (mpJoinListIndex < 0)
				{
					mpJoinListIndex = 0;
				}
				mainC.soundsMain.Play_Sound_NonPositional("MenuChange");
			}
		}
	}

	public void Menu_Multiplayer_Lobby_GameSettings()
	{
		if (global::Networking.Networking.isHost)
		{
			if (controllerButtonAPressed)
			{
				if (menuItem < 3)
				{
					global::MainGame.MainGame.settingsTeamRace1 = menuItem;
					mainC.soundsMain.Play_Sound_NonPositional("ButtonClick");
				}
				else if (menuItem < 6)
				{
					global::MainGame.MainGame.settingsTeamRace2 = (byte)(menuItem - 3);
					mainC.soundsMain.Play_Sound_NonPositional("ButtonClick");
				}
				else if (menuItem > 9 && menuItem < 14)
				{
					global::MainGame.MainGame.settingsTimeLimit = (byte)(menuItem - 10);
					mainC.soundsMain.Play_Sound_NonPositional("ButtonClick");
				}
				else if (menuItem < 15)
				{
					global::MainGame.MainGame.settingsMap--;
					if (global::MainGame.MainGame.settingsMap >= 4)
					{
						short num = 3;
						global::MainGame.MainGame.settingsMap = (byte)num;
					}
					mainC.soundsMain.Play_Sound_NonPositional("ButtonClick");
				}
				else if (menuItem < 16)
				{
					global::MainGame.MainGame.settingsMap++;
					if (global::MainGame.MainGame.settingsMap >= 4)
					{
						global::MainGame.MainGame.settingsMap = 0;
					}
					mainC.soundsMain.Play_Sound_NonPositional("ButtonClick");
				}
			}
			if ((menuChangeItem & 1) > 0)
			{
				menuItem++;
				if (menuItem > 5 && menuItem < 10)
				{
					menuItem = 10;
				}
				mainC.soundsMain.Play_Sound_NonPositional("MenuChange");
			}
			else if ((menuChangeItem & 2) > 0)
			{
				menuItem--;
				if (menuItem > 5 && menuItem < 10)
				{
					menuItem = 5;
				}
				mainC.soundsMain.Play_Sound_NonPositional("MenuChange");
			}
			else if ((menuChangeItem & 4) > 0)
			{
				if (menuItem > 13)
				{
					menuItem = 10;
				}
				else if (menuItem > 9)
				{
					menuItem = 4;
				}
				else
				{
					menuItem = 14;
				}
				mainC.soundsMain.Play_Sound_NonPositional("MenuChange");
			}
			else if ((menuChangeItem & 8) > 0)
			{
				if (menuItem < 10)
				{
					menuItem = 10;
				}
				else if (menuItem < 14)
				{
					menuItem = 14;
				}
				else
				{
					menuItem = 4;
				}
				mainC.soundsMain.Play_Sound_NonPositional("MenuChange");
			}
			else if (controllerButtonBPressed || controllerButtonBackPressed)
			{
				mainC.soundsMain.Play_Sound_NonPositional("ButtonClick");
				Leave_Menu_Completely();
			}
			else if (controllerButtonStartPressed)
			{
				global::MainGame.MainGame.mp_timeLimit_index = global::MainGame.MainGame.settingsTimeLimit;
				global::MainGame.MainGame.gameLevel = global::MainGame.MainGame.settingsMap;
				global::MainGame.MainGame.roundTimeLimit = global::MainGame.MainGame.mp_timeLimit[global::MainGame.MainGame.mp_timeLimit_index] * 60;
				global::MainGame.MainGame.roundCurrentTime = global::MainGame.MainGame.mp_timeLimit[global::MainGame.MainGame.mp_timeLimit_index] * 60;
				mainC.gameLogic.Game_Send_GameSettings(8);
				mainC.soundsMain.Play_Sound_NonPositional("ButtonClick");
				Leave_Menu_Completely();
			}
			if (menuItem > 15)
			{
				menuItem = 0;
			}
			else if (menuItem < 0)
			{
				menuItem = 15;
			}
		}
		else if (controllerButtonBPressed || controllerButtonBackPressed || controllerButtonStartPressed || controllerButtonXPressed)
		{
			mainC.soundsMain.Play_Sound_NonPositional("ButtonClick");
			Leave_Menu_Completely();
		}
	}

	public void Menu_InGame_Menu()
	{
		if (global::Rendering.Rendering.mbRestart)
		{
			if (controllerButtonAPressed)
			{
				global::Rendering.Rendering.mbRestart = false;
				if (global::MainGame.MainGame.gameMode == 1)
				{
					mainC.playersMain.MP_Player_Suicide();
				}
				else
				{
					mainC.maingameMain.SP_Restart(0);
				}
				mainC.soundsMain.Play_Sound_NonPositional("ButtonClick");
				Leave_Menu_Completely();
			}
			else if (controllerButtonBPressed)
			{
				global::Rendering.Rendering.mbRestart = false;
				mainC.soundsMain.Play_Sound_NonPositional("ButtonClick");
			}
			return;
		}
		if (messageBox_ExitToTitle)
		{
			if (controllerButtonAPressed)
			{
				global::MainGame.MainGame.gameState = 1;
				global::Rendering.Rendering.renderMenuScreen = 1;
				if (global::Networking.Networking.networkSessionReady)
				{
					mainC.networkingMain.XBOX_Close_Session();
				}
				mainC.soundsMain.Play_Sound_NonPositional("ButtonClick");
				mainC.inputMain.Switch_To_Menu(0);
				messageBox_ExitToTitle = false;
			}
			else if (controllerButtonBPressed)
			{
				messageBox_ExitToTitle = false;
				mainC.soundsMain.Play_Sound_NonPositional("ButtonClick");
			}
			return;
		}
		if (controllerButtonAPressed)
		{
			switch (menuItem)
			{
			case 0:
				menuConfig[10, 2] = 0;
				Switch_To_Menu(5);
				break;
			case 1:
				menuConfig[10, 2] = 1;
				Switch_To_Menu(3);
				break;
			case 2:
				menuConfig[10, 2] = 2;
				Switch_To_Menu(14);
				break;
			case 3:
				mainC.weaponsMain.Stop_Using_Iron_Sights_Or_Weapon_Scope();
				global::Players.Players.currentView = (byte)((global::Players.Players.currentView + 1) % 2);
				global::Players.Players.lastView = global::Players.Players.currentView;
				break;
			case 4:
				if (global::MainGame.MainGame.trialMode)
				{
					buyMeOnExit = false;
					menuConfig[10, 2] = 4;
					Switch_To_Menu(17);
				}
				else
				{
					mainC.maingameMain.Tell_A_Friend();
				}
				break;
			case 5:
				global::Rendering.Rendering.mbRestart = true;
				break;
			case 6:
				messageBox_ExitToTitle = true;
				return;
			case 7:
				Leave_Menu_Completely();
				break;
			}
			mainC.soundsMain.Play_Sound_NonPositional("ButtonClick");
		}
		if ((menuChangeItem & 8) > 0)
		{
			menuItem++;
			if (menuItem >= numMenuItems)
			{
				menuItem = 0;
			}
			mainC.soundsMain.Play_Sound_NonPositional("MenuChange");
		}
		else if ((menuChangeItem & 4) > 0)
		{
			menuItem--;
			if (menuItem >= numMenuItems)
			{
				menuItem = (byte)(numMenuItems - 1);
			}
			mainC.soundsMain.Play_Sound_NonPositional("MenuChange");
		}
		else if (controllerButtonStartPressed || controllerButtonBPressed || controllerButtonBackPressed)
		{
			mainC.soundsMain.Play_Sound_NonPositional("ButtonClick");
			Leave_Menu_Completely();
		}
	}

	public void Menu_Settings()
	{
		if (menuStat == 1)
		{
			mainC.soundsMain.Play_Sound_NonPositional("ButtonClick");
			Leave_Menu_Completely();
		}
		else if (messageBox_Brightness)
		{
			if (controllerButtonAPressed || controllerButtonBPressed)
			{
				messageBox_Brightness = false;
				mainC.soundsMain.Play_Sound_NonPositional("ButtonClick");
			}
		}
		else if (controllerButtonXPressed)
		{
			global::Rendering.Rendering.brightness = 0.5f;
			lookSensitivity[0] = 0.5f;
			lookSensitivity[1] = 0.25f;
			global::Sounds.Sounds.volume[0] = -45f;
			global::Sounds.Sounds.volume[1] = -45f;
			global::Sounds.Sounds.volume[2] = -45f;
			global::Sounds.Sounds.soundEnabled[0] = true;
			global::Sounds.Sounds.soundEnabled[1] = true;
			global::Sounds.Sounds.soundEnabled[2] = true;
			rumble = 1;
			global::Players.Players.currentView = 0;
			global::Players.Players.invertY = 1f;
			global::Players.Players.invertYSecondary = 1f;
			swapSticks = false;
			slowSideStep = false;
			mainC.soundsMain.Update_Sound_Settings(0);
			mainC.soundsMain.Update_Sound_Settings(1);
			mainC.soundsMain.Update_Sound_Settings(2);
			mainC.soundsMain.Set_Music_Volume();
			mainC.soundsMain.Play_Sound_NonPositional("ButtonClick");
		}
		else if (controllerButtonAPressed)
		{
			switch (menuItem)
			{
			case 0:
				mainC.soundsMain.Play_Sound_NonPositional("ButtonClick");
				if (global::MainGame.MainGame.gameState > 1)
				{
					menuStat = 0;
					menuConfig[5, 2] = 0;
					Switch_To_Menu(15);
				}
				else
				{
					messageBox_Brightness = true;
				}
				break;
			case 3:
			case 4:
			case 5:
			{
				mainC.soundsMain.Play_Sound_NonPositional("ButtonClick");
				byte b = (byte)(menuItem - 3);
				global::Sounds.Sounds.soundEnabled[b] = !global::Sounds.Sounds.soundEnabled[b];
				mainC.soundsMain.Update_Sound_Settings(b);
				break;
			}
			case 6:
				mainC.soundsMain.Play_Sound_NonPositional("ButtonClick");
				rumble = (byte)((rumble + 1) % 2);
				break;
			case 7:
				mainC.soundsMain.Play_Sound_NonPositional("ButtonClick");
				global::Players.Players.currentView = (byte)((global::Players.Players.currentView + 1) % 2);
				global::Players.Players.lastView = global::Players.Players.currentView;
				break;
			case 8:
				mainC.soundsMain.Play_Sound_NonPositional("ButtonClick");
				global::Players.Players.invertY *= -1f;
				break;
			case 9:
				mainC.soundsMain.Play_Sound_NonPositional("ButtonClick");
				swapSticks = !swapSticks;
				break;
			case 10:
				mainC.soundsMain.Play_Sound_NonPositional("ButtonClick");
				slowSideStep = !slowSideStep;
				break;
			case 1:
			case 2:
				break;
			}
		}
		else if ((menuChangeItem & 3) > 0)
		{
			switch (menuItem)
			{
			case 1:
			case 2:
				lookModeAdj = (byte)(menuItem - 1);
				if (controllerDPadRightPressed)
				{
					controllerStickLeftValueX = 1f;
				}
				else if (controllerDPadLeftPressed)
				{
					controllerStickLeftValueX = -1f;
				}
				lookSensitivity[lookModeAdj] += 0.02f * controllerStickLeftValueX;
				if (lookSensitivity[lookModeAdj] > 1f)
				{
					lookSensitivity[lookModeAdj] = 1f;
				}
				if (lookSensitivity[lookModeAdj] < 0.15f)
				{
					lookSensitivity[lookModeAdj] = 0.15f;
				}
				mainC.soundsMain.Play_Sound_NonPositional("MenuChange");
				break;
			case 3:
			case 4:
			case 5:
			{
				byte b = (byte)(menuItem - 3);
				if (controllerDPadRightPressed)
				{
					controllerStickLeftValueX = 1f;
				}
				else if (controllerDPadLeftPressed)
				{
					controllerStickLeftValueX = -1f;
				}
				global::Sounds.Sounds.volume[b] += 2.9315f * controllerStickLeftValueX;
				if (global::Sounds.Sounds.volume[b] < -96f)
				{
					global::Sounds.Sounds.volume[b] = -96f;
				}
				if (global::Sounds.Sounds.volume[b] > 6f)
				{
					global::Sounds.Sounds.volume[b] = 6f;
				}
				if (b == 1)
				{
					mainC.soundsMain.Set_Music_Volume();
				}
				mainC.soundsMain.Play_Sound_NonPositional("MenuChange");
				break;
			}
			}
		}
		else if (controllerButtonBackPressed)
		{
			global::Rendering.Rendering.brightness = savedSettingsFloat[0];
			lookSensitivity[0] = savedSettingsFloat[1];
			lookSensitivity[1] = savedSettingsFloat[2];
			global::Sounds.Sounds.volume[0] = savedSettingsFloat[3];
			global::Sounds.Sounds.volume[1] = savedSettingsFloat[4];
			global::Sounds.Sounds.volume[2] = savedSettingsFloat[5];
			rumble = savedSettingsByte[0];
			global::Players.Players.currentView = savedSettingsByte[1];
			global::Players.Players.invertY = savedSettingsFloat[6];
			swapSticks = savedSettingsBool[0];
			slowSideStep = savedSettingsBool[1];
			global::Sounds.Sounds.soundEnabled[0] = savedSettingsBool[3];
			global::Sounds.Sounds.soundEnabled[1] = savedSettingsBool[4];
			global::Sounds.Sounds.soundEnabled[2] = savedSettingsBool[5];
			mainC.soundsMain.Update_Sound_Settings(0);
			mainC.soundsMain.Update_Sound_Settings(1);
			mainC.soundsMain.Update_Sound_Settings(2);
			mainC.soundsMain.Set_Music_Volume();
			menuStat = 0;
			mainC.soundsMain.Play_Sound_NonPositional("ButtonClick");
			Leave_Menu_Completely();
		}
		else if (controllerButtonStartPressed)
		{
			menuStat = 1;
			mainC.soundsMain.Play_Sound_NonPositional("ButtonClick");
		}
		else if ((menuChangeItem & 8) > 0)
		{
			menuItem++;
			if (menuItem >= numMenuItems)
			{
				menuItem = 0;
			}
			mainC.soundsMain.Play_Sound_NonPositional("MenuChange");
		}
		else if ((menuChangeItem & 4) > 0)
		{
			menuItem--;
			if (menuItem >= numMenuItems)
			{
				menuItem = (byte)(numMenuItems - 1);
			}
			mainC.soundsMain.Play_Sound_NonPositional("MenuChange");
		}
	}

	public void Menu_Controller()
	{
		if (controllerButtonAPressed || controllerButtonBPressed || controllerButtonBackPressed || controllerButtonStartPressed || controllerButtonXPressed || controllerButtonYPressed)
		{
			global::Rendering.Rendering.renderMenuScreen &= 251;
			mainC.soundsMain.Play_Sound_NonPositional("ButtonClick");
			if (global::MainGame.MainGame.gameState == 1)
			{
				mainC.userInterface.Load_Main_Menu();
			}
			else
			{
				Switch_To_Menu(10);
			}
		}
	}

	public void Menu_Credits()
	{
		if (controllerButtonAPressed || controllerButtonBPressed || controllerButtonBackPressed || controllerButtonStartPressed || controllerButtonXPressed || controllerButtonYPressed)
		{
			mainC.soundsMain.Play_Sound_NonPositional("ButtonClick");
			mainC.userInterface.Load_Main_Menu();
		}
	}

	public void Menu_Buy_Me()
	{
		if (global::Rendering.Rendering.mb_Purchased)
		{
			if (controllerButtonAPressed || controllerButtonBPressed || controllerButtonBackPressed || controllerButtonStartPressed)
			{
				global::Rendering.Rendering.mbSignedToBuy = false;
				global::Rendering.Rendering.mb_Purchased = false;
				mainC.soundsMain.Play_Sound_NonPositional("ButtonClick");
				if (global::MainGame.MainGame.gameState == 1)
				{
					mainC.userInterface.Load_Main_Menu();
					return;
				}
				global::Rendering.Rendering.mbTrialOver = true;
				Leave_Menu_Completely();
			}
		}
		else if (menuStat == 1)
		{
			switch (mainC.networkingMain.XBOX_SignIn_To_Buy())
			{
			case 1:
				menuStat = 2;
				break;
			case 2:
				menuStat = 0;
				break;
			}
		}
		else if (menuStat == 2)
		{
			if (global::MainGame.MainGame.trialMode)
			{
				byte b = mainC.networkingMain.XBOX_Purchase_Game();
				if (b == 3)
				{
					menuStat = 0;
				}
			}
			else
			{
				menuStat = 3;
			}
		}
		else if (menuStat == 3)
		{
			global::Rendering.Rendering.mb_Purchased = true;
		}
		else if (global::Rendering.Rendering.mbSignedToBuy)
		{
			if (controllerButtonBPressed)
			{
				global::Rendering.Rendering.mbSignedToBuy = false;
				mainC.soundsMain.Play_Sound_NonPositional("ButtonClick");
			}
			else if (controllerButtonAPressed)
			{
				global::Rendering.Rendering.mbSignedToBuy = false;
				menuStat = 1;
				mainC.soundsMain.Play_Sound_NonPositional("ButtonClick");
			}
		}
		else if (controllerButtonAPressed && buyMeOnExit)
		{
			mainC.soundsMain.Play_Sound_NonPositional("ButtonClick");
			mainC.userInterface.Load_Main_Menu();
			mainC.Quit_Game();
		}
		else if (controllerButtonYPressed)
		{
			if (!mainC.networkingMain.XBOX_SignedIn_And_CanBuy())
			{
				global::Rendering.Rendering.mbSignedToBuy = true;
				menuStat = 0;
				mainC.soundsMain.Play_Sound_NonPositional("ButtonClick");
			}
			else
			{
				menuStat = 1;
				mainC.networkingMain.XBOX_Reset_Guide_Status();
				mainC.soundsMain.Play_Sound_NonPositional("ButtonClick");
			}
		}
		else if (controllerButtonBPressed || controllerButtonBackPressed || controllerButtonStartPressed || controllerButtonXPressed)
		{
			mainC.soundsMain.Play_Sound_NonPositional("ButtonClick");
			Leave_Menu_Completely();
		}
	}

	public void Menu_Instructions()
	{
		if ((menuChangeItem & 9) > 0)
		{
			menuItem++;
			if (menuItem >= numMenuItems)
			{
				menuItem = 0;
			}
			mainC.soundsMain.Play_Sound_NonPositional("MenuChange");
		}
		else if ((menuChangeItem & 6) > 0)
		{
			menuItem--;
			if (menuItem >= numMenuItems)
			{
				menuItem = (byte)(numMenuItems - 1);
			}
			mainC.soundsMain.Play_Sound_NonPositional("MenuChange");
		}
		else if (controllerButtonBPressed || controllerButtonBackPressed)
		{
			mainC.soundsMain.Play_Sound_NonPositional("ButtonClick");
			Leave_Menu_Completely();
		}
	}

	public void Menu_Brightness()
	{
		bool flag = true;
		if (global::Players.Players.players[0].onmap == 1 && global::Players.Players.players[0].dead)
		{
			global::Players.Players.respawnTimer -= global::MainGame.MainGame.frametime;
			if (global::Players.Players.respawnTimer < 0f)
			{
				global::Players.Players.respawnTimer = -1f;
			}
		}
		if (controllerButtonBPressed)
		{
			global::Rendering.Rendering.brightness = savedSettingsFloat[0];
			mainC.renderingMain.Set_Brightness();
			menuStat = 0;
			mainC.soundsMain.Play_Sound_NonPositional("ButtonClick");
			Leave_Menu_Completely();
		}
		else if (controllerButtonAPressed)
		{
			menuStat = 0;
			mainC.soundsMain.Play_Sound_NonPositional("ButtonClick");
			Leave_Menu_Completely();
		}
		else if (controllerButtonYPressed)
		{
			global::Rendering.Rendering.brightness = 0.5f;
			mainC.renderingMain.Set_Brightness();
			mainC.soundsMain.Play_Sound_NonPositional("ButtonClick");
		}
		else
		{
			if ((menuChangeItem & 0xF) <= 0)
			{
				return;
			}
			if ((menuChangeItem & 3) > 0)
			{
				if (Math.Abs(controllerStickLeftValueY) > Math.Abs(controllerStickLeftValueX))
				{
					controllerStickLeftValueX = controllerStickLeftValueY;
				}
				if (controllerDPadRightPressed)
				{
					controllerStickLeftValueX = 1f;
				}
				else if (controllerDPadLeftPressed)
				{
					controllerStickLeftValueX = -1f;
				}
				global::Rendering.Rendering.brightness += 0.02f * controllerStickLeftValueX;
				if (global::Rendering.Rendering.brightness > 1.25f)
				{
					flag = false;
					global::Rendering.Rendering.brightness = 1.25f;
				}
				if (global::Rendering.Rendering.brightness < 0.75f)
				{
					flag = false;
					global::Rendering.Rendering.brightness = 0.75f;
				}
				mainC.renderingMain.Set_Brightness();
				if (flag)
				{
					mainC.soundsMain.Play_Sound_NonPositional("MenuChange");
				}
			}
			else if ((menuChangeItem & 0xC) > 0)
			{
				if (Math.Abs(controllerStickLeftValueY) > Math.Abs(controllerStickLeftValueX))
				{
					controllerStickLeftValueX = controllerStickLeftValueY;
				}
				if (controllerDPadUpPressed)
				{
					controllerStickLeftValueX = 1f;
				}
				else if (controllerDPadDownPressed)
				{
					controllerStickLeftValueX = -1f;
				}
				global::Rendering.Rendering.brightness += 0.02f * controllerStickLeftValueX;
				if (global::Rendering.Rendering.brightness > 1.25f)
				{
					flag = false;
					global::Rendering.Rendering.brightness = 1.25f;
				}
				if (global::Rendering.Rendering.brightness < 0.75f)
				{
					flag = false;
					global::Rendering.Rendering.brightness = 0.75f;
				}
				mainC.renderingMain.Set_Brightness();
				if (flag)
				{
					mainC.soundsMain.Play_Sound_NonPositional("MenuChange");
				}
			}
		}
	}

	public void Menu_Dummy_1()
	{
	}

	public void Menu_Dummy_2()
	{
	}

	public void Menu_Dummy_3()
	{
	}

	public void Multiplayer_Cancelled()
	{
		global::Rendering.Rendering.mbQM_Searching = false;
		global::Rendering.Rendering.mbJoin_Searching = false;
		menuStat = 3;
	}

	public void Multiplayer_Failed(byte reason)
	{
		switch (reason)
		{
		case 0:
			showMessageBox_QM_NoGames = true;
			break;
		case 1:
			refreshTimer = 0f;
			showMessageBox_MP_Error = true;
			mainC.userInterface.Set_All_Component_Status(7, 16, 0);
			mainC.userInterface.Set_Component_Status(7, 16, 1, 1);
			mainC.userInterface.Show_Window_Specified_Time(16, 7, resetButtons: false, 2f);
			break;
		case 2:
			global::Rendering.Rendering.mbMessageBox_Join_NoGames = true;
			global::Rendering.Rendering.mbJoin_Searching = false;
			mainC.userInterface.Set_All_Component_Status(7, 16, 0);
			mainC.userInterface.Set_Component_Status(7, 16, 3, 1);
			mainC.userInterface.Show_Window_Specified_Time(16, 7, resetButtons: false, 2f);
			global::Networking.Networking.multiplayerNewGameStatus = 2;
			break;
		case 3:
			mainC.userInterface.Set_All_Component_Status(7, 16, 0);
			mainC.userInterface.Set_Component_Status(7, 16, 2, 1);
			mainC.userInterface.Show_Window_Specified_Time(16, 7, resetButtons: false, 2f);
			global::Networking.Networking.multiplayerNewGameStatus = 3;
			break;
		case 4:
			mainC.userInterface.Set_All_Component_Status(7, 16, 0);
			mainC.userInterface.Set_Component_Status(7, 16, 1, 1);
			mainC.userInterface.Show_Window_Specified_Time(16, 7, resetButtons: false, 2f);
			global::Networking.Networking.multiplayerNewGameStatus = 4;
			break;
		case 5:
			mainC.userInterface.Set_All_Component_Status(7, 16, 0);
			mainC.userInterface.Set_Component_Status(7, 16, 0, 1);
			mainC.userInterface.Show_Window_Specified_Time(16, 7, resetButtons: false, 2f);
			global::Networking.Networking.multiplayerNewGameStatus = 5;
			break;
		}
		global::Rendering.Rendering.mbQM_Searching = false;
		global::Rendering.Rendering.mbJoin_Searching = false;
		menuStat = 3;
	}

	public void Singleplayer_Session_Ready()
	{
		menuStat++;
	}

	public void Multiplayer_Session_Over()
	{
		global::Rendering.Rendering.renderMenuScreen = 1;
		global::MainGame.MainGame.gameMode = byte.MaxValue;
		global::MainGame.MainGame.gameState = 1;
		mainC.userInterface.Load_Main_Menu();
		menuType = 0;
		mainC.gameLogic.Game_Misc_Threaded(1);
	}

	public void Multiplayer_Join_List_Ready()
	{
		global::Rendering.Rendering.mbJoin_Searching = false;
		mpJoinListReady = true;
		menuStat = 3;
	}

	public static bool GamePadActive(ref GamePadState gamePadState)
	{
		if (gamePadState.IsConnected)
		{
			if (gamePadState.Buttons.A != ButtonState.Pressed && gamePadState.Buttons.B != ButtonState.Pressed && gamePadState.Buttons.X != ButtonState.Pressed && gamePadState.Buttons.Y != ButtonState.Pressed && gamePadState.Buttons.Start != ButtonState.Pressed && gamePadState.Buttons.Back != ButtonState.Pressed && gamePadState.Buttons.LeftShoulder != ButtonState.Pressed && gamePadState.Buttons.RightShoulder != ButtonState.Pressed && gamePadState.Buttons.LeftStick != ButtonState.Pressed && gamePadState.Buttons.RightStick != ButtonState.Pressed && gamePadState.DPad.Up != ButtonState.Pressed && gamePadState.DPad.Left != ButtonState.Pressed && gamePadState.DPad.Right != ButtonState.Pressed && gamePadState.DPad.Down != ButtonState.Pressed && !(gamePadState.Triggers.Right > 0f) && !(gamePadState.Triggers.Left > 0f))
			{
				if (standardController)
				{
					if (!(Math.Abs(gamePadState.ThumbSticks.Right.X) > 0.1f) && !(Math.Abs(gamePadState.ThumbSticks.Right.Y) > 0.1f) && !(Math.Abs(gamePadState.ThumbSticks.Left.X) > 0.1f))
					{
						return Math.Abs(gamePadState.ThumbSticks.Left.Y) > 0.1f;
					}
					return true;
				}
				return false;
			}
			return true;
		}
		return false;
	}

	public void Switch_To_Menu(byte menuID)
	{
		switch (menuID)
		{
		case 0:
			global::Rendering.Rendering.renderMenuScreen = 1;
			currentMenu = 0;
			numMenuItems = menuConfig[0, 0];
			menuItem = menuConfig[0, 1];
			if (menuConfig[0, 2] < byte.MaxValue)
			{
				menuItem = menuConfig[0, 2];
			}
			menuConfig[0, 2] = 0;
			menuType = 0;
			menuStat = 0;
			mainC.soundsMain.Set_Music(global::Sounds.Sounds.musicMenuID);
			messageBox_ExitToDash = false;
			mainC.soundsMain.Stop_All_Game_Sounds(stopNarrator: true);
			break;
		case 1:
			global::Rendering.Rendering.renderMenuScreen = 1;
			menuStat = 0;
			currentMenu = 1;
			numMenuItems = menuConfig[1, 0];
			menuItem = menuConfig[1, 1];
			if (menuConfig[1, 2] < byte.MaxValue)
			{
				menuItem = menuConfig[1, 2];
			}
			menuConfig[1, 2] = 0;
			menuType = 0;
			showMessageBox_QM_NoGames = false;
			mainC.soundsMain.Set_Music(global::Sounds.Sounds.musicMenuID);
			break;
		case 2:
			global::Rendering.Rendering.renderMenuScreen |= 1;
			menuStat = 0;
			currentMenu = 2;
			numMenuItems = menuConfig[2, 0];
			menuItem = menuConfig[2, 1];
			menuType = 0;
			mainC.soundsMain.Set_Music(global::Sounds.Sounds.musicMenuID);
			break;
		case 3:
			if (global::MainGame.MainGame.gameState == 1)
			{
				global::Rendering.Rendering.renderMenuScreen = 1;
			}
			else
			{
				global::Rendering.Rendering.renderMenuScreen = 4;
			}
			menuStat = 0;
			currentMenu = 3;
			numMenuItems = menuConfig[3, 0];
			menuItem = menuConfig[3, 1];
			menuType = 1;
			if (global::MainGame.MainGame.gameState == 1)
			{
				menuType = 0;
			}
			break;
		case 4:
			global::Rendering.Rendering.renderMenuScreen = 1;
			menuStat = 0;
			currentMenu = 4;
			numMenuItems = menuConfig[4, 0];
			menuItem = menuConfig[4, 1];
			if (menuConfig[4, 2] < byte.MaxValue)
			{
				menuItem = menuConfig[4, 2];
			}
			menuConfig[4, 2] = 0;
			menuType = 0;
			newSPGame = false;
			global::Rendering.Rendering.mbSP_NewGameOverwrite = false;
			break;
		case 5:
			global::Rendering.Rendering.renderMenuScreen = 1;
			menuStat = 0;
			currentMenu = 5;
			numMenuItems = menuConfig[5, 0];
			menuItem = menuConfig[5, 1];
			if (menuConfig[5, 2] < byte.MaxValue)
			{
				menuItem = menuConfig[5, 2];
			}
			menuConfig[5, 2] = 0;
			menuType = 0;
			mainC.soundsMain.Set_Music(global::Sounds.Sounds.musicMenuID);
			savedSettingsFloat[0] = global::Rendering.Rendering.brightness;
			savedSettingsFloat[1] = lookSensitivity[0];
			savedSettingsFloat[2] = lookSensitivity[1];
			savedSettingsFloat[3] = global::Sounds.Sounds.volume[0];
			savedSettingsFloat[4] = global::Sounds.Sounds.volume[1];
			savedSettingsFloat[5] = global::Sounds.Sounds.volume[2];
			savedSettingsByte[0] = rumble;
			savedSettingsByte[1] = global::Players.Players.currentView;
			savedSettingsFloat[6] = global::Players.Players.invertY;
			savedSettingsBool[0] = swapSticks;
			savedSettingsBool[1] = slowSideStep;
			savedSettingsBool[3] = global::Sounds.Sounds.soundEnabled[0];
			savedSettingsBool[4] = global::Sounds.Sounds.soundEnabled[1];
			savedSettingsBool[5] = global::Sounds.Sounds.soundEnabled[2];
			break;
		case 6:
			global::Rendering.Rendering.renderMenuScreen = 1;
			menuStat = 0;
			currentMenu = 6;
			numMenuItems = menuConfig[6, 0];
			menuItem = menuConfig[6, 1];
			if (menuConfig[6, 2] < byte.MaxValue)
			{
				menuItem = menuConfig[6, 2];
			}
			menuConfig[6, 2] = 0;
			menuType = 0;
			break;
		case 7:
			global::Rendering.Rendering.renderMenuScreen = 1;
			menuStat = 0;
			currentMenu = 7;
			numMenuItems = menuConfig[7, 0];
			menuItem = menuConfig[7, 1];
			if (menuConfig[0, 2] < byte.MaxValue)
			{
				menuItem = menuConfig[0, 2];
			}
			menuConfig[7, 2] = 0;
			mainC.soundsMain.Set_Music(global::Sounds.Sounds.musicMenuID);
			menuType = 0;
			break;
		case 8:
			global::Rendering.Rendering.renderMenuScreen = 1;
			currentMenu = 8;
			numMenuItems = menuConfig[8, 0];
			menuItem = menuConfig[8, 1];
			if (menuConfig[8, 2] < byte.MaxValue)
			{
				menuItem = menuConfig[8, 2];
			}
			menuConfig[8, 2] = 0;
			mainC.soundsMain.Set_Music(global::Sounds.Sounds.musicMenuID);
			menuType = 0;
			break;
		case 9:
			global::Rendering.Rendering.renderMenuScreen = 1;
			currentMenu = 9;
			numMenuItems = menuConfig[9, 0];
			menuItem = menuConfig[9, 1];
			if (menuConfig[9, 2] < byte.MaxValue)
			{
				menuItem = menuConfig[9, 2];
			}
			menuConfig[9, 2] = 0;
			menuType = 0;
			mpLive = false;
			mainC.soundsMain.Set_Music(global::Sounds.Sounds.musicMenuID);
			break;
		case 10:
			global::Rendering.Rendering.renderMenuScreen = 4;
			currentMenu = 10;
			numMenuItems = menuConfig[10, 0];
			menuItem = menuConfig[10, 1];
			if (menuConfig[10, 2] < byte.MaxValue)
			{
				menuItem = menuConfig[10, 2];
			}
			menuConfig[10, 2] = 0;
			menuType = 1;
			messageBox_ExitToTitle = false;
			global::Rendering.Rendering.mbRestart = false;
			mainC.soundsMain.Stop_All_Game_Sounds(stopNarrator: true);
			break;
		case 11:
			global::Rendering.Rendering.renderMenuScreen = 1;
			currentMenu = 11;
			numMenuItems = menuConfig[11, 0];
			menuItem = menuConfig[11, 1];
			if (menuConfig[11, 2] < byte.MaxValue)
			{
				menuItem = menuConfig[11, 2];
			}
			menuConfig[11, 2] = 0;
			menuType = 0;
			mpJoinListIndex = 0;
			break;
		case 12:
			global::Rendering.Rendering.renderMenuScreen = 1;
			currentMenu = 12;
			menuType = 0;
			break;
		case 13:
			global::Rendering.Rendering.renderMenuScreen = 1;
			currentMenu = 13;
			menuStat = 0;
			global::Rendering.Rendering.mbShowBuyMe = true;
			global::Rendering.Rendering.mbSignedToBuy = false;
			global::Rendering.Rendering.mb_Purchased = false;
			break;
		case 14:
			currentMenu = 14;
			menuItem = 0;
			numMenuItems = 7;
			break;
		case 15:
			if (global::MainGame.MainGame.gameState > 1)
			{
				currentMenu = 15;
				menuItem = 0;
				numMenuItems = 0;
				global::Rendering.Rendering.renderMenuScreen = 4;
			}
			break;
		case 16:
			global::Rendering.Rendering.renderMenuScreen = 4;
			currentMenu = 16;
			menuType = 1;
			break;
		case 17:
			global::Rendering.Rendering.renderMenuScreen = 4;
			currentMenu = 13;
			menuStat = 0;
			global::Rendering.Rendering.mbShowBuyMe = true;
			global::Rendering.Rendering.mbSignedToBuy = false;
			global::Rendering.Rendering.mb_Purchased = false;
			mainC.soundsMain.Stop_All_Game_Sounds(stopNarrator: true);
			break;
		case 18:
			global::Rendering.Rendering.renderMenuScreen = 32;
			currentMenu = 18;
			break;
		}
		menuChangeItem = 0;
		controllerStickLeftRepeatX = 0f;
		controllerStickLeftRepeatY = 0f;
	}

	public void Leave_Menu_Completely()
	{
		menuStat = 0;
		if (global::MainGame.MainGame.gameState == 1 || (global::MainGame.MainGame.gameMode != 0 && global::MainGame.MainGame.gameMode != 1))
		{
			mainC.userInterface.Load_Main_Menu();
			global::Rendering.Rendering.renderMenuScreen = 1;
			global::MainGame.MainGame.gameState = 1;
			mainC.userInterface.Load_Main_Menu();
		}
		else
		{
			inMenu = false;
			menuConfig[0, 2] = byte.MaxValue;
			menuConfig[1, 2] = byte.MaxValue;
			menuConfig[10, 2] = byte.MaxValue;
			GC.Collect();
		}
	}

	public void UI_Message_Handler(int Data1, int Data2)
	{
	}

	public void UI_Set_Sound_Effects_Volume(float value)
	{
		global::Sounds.Sounds.volume[0] = -96f + value * 102f;
		mainC.soundsMain.Set_Continual_Sounds_Volume();
	}

	public void UI_Set_Music_Volume(float value)
	{
		global::Sounds.Sounds.volume[1] = -96f + value * 102f;
		mainC.soundsMain.Set_Music_Volume();
	}

	public void UI_Set_Sound_Effects_Status(bool effectsOn)
	{
		global::Sounds.Sounds.soundEnabled[0] = effectsOn;
		global::Sounds.Sounds.soundEnabled[2] = effectsOn;
		mainC.soundsMain.Update_Sound_Settings(0);
	}

	public void UI_Set_Music_Status(bool musicOn)
	{
		global::Sounds.Sounds.soundEnabled[1] = musicOn;
		mainC.soundsMain.Update_Sound_Settings(1);
	}

	public void UI_Set_Sensitivity(float value)
	{
		lookSensitivity[0] = 0.15f + value * 0.85f;
		if (lookSensitivity[0] > 1f)
		{
			lookSensitivity[0] = 1f;
		}
		else if (lookSensitivity[0] < 0.15f)
		{
			lookSensitivity[0] = 0.15f;
		}
		lookSensitivity[1] = lookSensitivity[0] * 0.5f;
	}

	public void UI_Set_Brightness(float value)
	{
		global::Rendering.Rendering.brightness = value;
		mainC.renderingMain.Set_Brightness();
	}

	public void UI_Set_Rumble(byte value)
	{
		rumble = value;
	}

	public void UI_Set_SwapSticks(bool value)
	{
		swapSticks = value;
	}

	public void UI_Set_Invert_Y(byte value)
	{
		global::Players.Players.invertY = 1f - 2f * (float)(int)value;
	}

	public void UI_Set_Invert_Y_Secondary(byte value)
	{
		global::Players.Players.invertYSecondary = 1f - 2f * (float)(int)value;
	}

	public void UI_Set_Default_First_Person_View(byte value)
	{
		switch (value)
		{
		case 0:
			global::Players.Players.lastView = 0;
			global::Players.Players.currentView = 0;
			break;
		case 1:
			global::Players.Players.lastView = 1;
			global::Players.Players.currentView = 1;
			break;
		}
	}

	public void UI_Open_Plane_Selection()
	{
	}

	public void UI_Start_Wizard()
	{
		Switch_To_Menu(18);
	}

	public void UI_Results_Hide()
	{
	}

	public void UI_Results_Show()
	{
	}

	public void UI_HUD_Show_Thread0()
	{
	}

	public void UI_Initiailize()
	{
	}

	public void UI_Remove_All_Players_From_HUD()
	{
	}

	public void UI_End_Current_Game()
	{
	}

	public void UI_Set_Map(byte mapID)
	{
	}

	public bool UI_Get_ScoreCard_Status()
	{
		return false;
	}

	public void Setup_For_SP()
	{
		try
		{
			if (global::MainGame.MainGame.signedinGamerID > -1 && global::MainGame.MainGame.signedinGamerID < Gamer.SignedInGamers.Count && Gamer.SignedInGamers[global::MainGame.MainGame.signedinGamerID] != null && Gamer.SignedInGamers[global::MainGame.MainGame.signedinGamerID].IsSignedInToLive)
			{
				Gamer.SignedInGamers[global::MainGame.MainGame.signedinGamerID].Presence.PresenceMode = GamerPresenceMode.SinglePlayer;
			}
		}
		catch (Exception)
		{
		}
	}

	public void UI_HUD_Set_Lives()
	{
	}

	public void UI_HUD_Set_Player_Velocity(float velocity)
	{
	}

	public void UI_HUD_Set_Player_Height(float altitude)
	{
	}

	public void UI_HUD_Set_Flaps(bool flapsAreUp)
	{
	}

	public void UI_HUD_Set_LandingGear_Position(bool gearAreUp)
	{
	}

	public void UI_Set_Player_Team(byte playerID, byte teamID)
	{
	}

	public void UI_HUD_Show_SP_Level_Objectives(string Title, string Message, string ButtonText, bool HasAButton, float Timer)
	{
	}

	public void UI_HUD_Show_Exit_View_Message(bool showMessage)
	{
	}

	public void UI_HUD_Show_Guided_Bomb_Message(bool showMessage)
	{
	}

	static InputHandler()
	{
		bool[] array = new bool[4];
		secondCtrollerAButtonPressed = array;
		bool[] array2 = new bool[4];
		secondCtrollerBButtonPressed = array2;
		bool[] array3 = new bool[4];
		secondCtrollerDPadUpPressed = array3;
		bool[] array4 = new bool[4];
		secondCtrollerDPadDownPressed = array4;
		bool[] array5 = new bool[4];
		secondCtrollerDPadLeftPressed = array5;
		bool[] array6 = new bool[4];
		secondCtrollerDPadRightPressed = array6;
		bool[] array7 = new bool[4];
		secondCtrollerAPress = array7;
		bool[] array8 = new bool[4];
		secondCtrollerBPress = array8;
		bool[] array9 = new bool[4];
		secondCtrollerDPadUp = array9;
		bool[] array10 = new bool[4];
		secondCtrollerDPadDown = array10;
		bool[] array11 = new bool[4];
		secondCtrollerDPadLeft = array11;
		bool[] array12 = new bool[4];
		secondCtrollerDPadRight = array12;
		bool[] array13 = new bool[4];
		secondCtrollerSticks = array13;
		float[] array14 = new float[4];
		secondCtrollerDPadRepeat = array14;
		controllerButtonStart = false;
		controllerButtonBack = false;
		controllerButtonStartPressed = false;
		controllerButtonBackPressed = false;
		controllerButtonX = false;
		controllerButtonY = false;
		controllerButtonA = false;
		controllerButtonB = false;
		controllerButtonXPressed = false;
		controllerButtonYPressed = false;
		controllerButtonAPressed = false;
		controllerButtonBPressed = false;
		controllerButtonLeftShoulder = false;
		controllerButtonRightShoulder = false;
		controllerButtonLeftShoulderPressed = false;
		controllerButtonRightShoulderPressed = false;
		controllerTriggerRight = false;
		controllerTriggerLeft = false;
		controllerTriggerRightPressed = false;
		controllerTriggerLeftPressed = false;
		controllerStickButtonRight = false;
		controllerStickButtonLeft = false;
		controllerStickButtonRightPressed = false;
		controllerStickButtonLeftPressed = false;
		controllerDPadUp = false;
		controllerDPadDown = false;
		controllerDPadLeft = false;
		controllerDPadRight = false;
		controllerDPadUpPressed = false;
		controllerDPadDownPressed = false;
		controllerDPadLeftPressed = false;
		controllerDPadRightPressed = false;
		mpJoinListIndex = 0;
		controllerStickLeftRepeatX = 0f;
		controllerStickLeftRepeatY = 0f;
		controllerStickRightSmoothX = 0f;
		controllerStickRightSmoothY = 0f;
		controllerStickRightValX = 0f;
		controllerStickRightValY = 0f;
		controllerTriggerRightValue = 0f;
		controllerTriggerLeftValue = 0f;
		controllerTriggerRightLastValue = 0f;
		controllerTriggerLeftLastValue = 0f;
		savedSettingsBool = new bool[6];
		savedSettingsByte = new byte[2];
		savedSettingsFloat = new float[7];
		refreshTimer = 0f;
		lookSensitivity = new float[2] { 0.5f, 0.25f };
		rumbleLow = 0f;
		rumbleHigh = 0f;
		dPadRightRepeat = 0f;
		dPadLeftRepeat = 0f;
		dPadUpRepeat = 0f;
		dPadDownRepeat = 0f;
	}
}
