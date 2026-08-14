using System;
using System.Globalization;
using System.IO;
using FontModule;
using InputHandler;
using Joints;
using MainGame;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using Models;
using Physics;
using Players;
using Structs;
using Textures;
using Threads;
using Weapons;
using WindowsGame1;

namespace Rendering;

public class Rendering
{
	public static bool needLevelShadowMap = true;

	public static byte numParticleTypes;

	public static byte numParticleEmitterTypes;

	public static byte numParticleEmitters;

	public static ushort nextParticleEmitter;

	public static EffectParameterCollection particleParameters;

	public static EffectParameter effectViewParameter;

	public static EffectParameter effectProjectionParameter;

	public static EffectParameter effectViewportScaleParameter;

	public static EffectParameter effectTimeParameter;

	public static StructsClass.Particle_Type[] MS_Particles;

	public static StructsClass.Particle_Emitter[] emitterDefs;

	public static StructsClass.Particle_Emitter_Instance[] particleEmitters;

	public static bool[] showTauntMessage = new bool[2];

	public static bool[] showSwapWeaponMessage = new bool[2];

	public static bool[] renderMainPlayer = new bool[2];

	public static bool[] disableSplashSlider = new bool[2];

	public static bool[] enableSplashSlider = new bool[2];

	public static bool[] updateSplashText = new bool[2];

	public static bool[] updateSplashSliderValue = new bool[2];

	public static bool useShadowMap = true;

	public static bool renderHud = true;

	public static bool renderMinimap = true;

	public static bool miniMapStaticTexture = false;

	public static bool moveViewToNewLocation = false;

	public static bool renderCrossHairs = false;

	public static bool mbTrialOver = false;

	public static bool mbRestart = false;

	public static bool mb_Purchased = false;

	public static bool mbSignedToBuy = false;

	public static bool mbShowBuyMe = false;

	public static bool mbSP_NewGameOverwrite = false;

	public static bool mbRespawn = false;

	public static bool mbMessageBox_Join_NoGames = false;

	public static bool mbQM_Searching;

	public static bool mbJoin_Searching = false;

	public static bool rotateSplash = false;

	public static bool fixShowSwitch = false;

	public static bool fixShowSwitch2 = false;

	public static bool renderTransporterBar;

	public static float[] splashSlider = new float[2];

	public static string[] splashText = new string[2];

	public static bool splashReady = false;

	public static bool watchedPlayerIsInvalid = false;

	public static byte renderGamerTags = 0;

	public static byte currentHitIndicator;

	public static byte numHitIndicatorTextures;

	public static byte numHitIndicators;

	public static byte renderGamerTagMask = 14;

	public static byte gamerTagFont;

	public static byte skyBoxType = 0;

	public static byte numAvatarCameraPositions = 0;

	public static byte numAllocatedAvatarCameraPostions = 0;

	public static byte numCameraPositions = 0;

	public static byte numAllocatedCameraPostions = 0;

	public static byte watchingPlayer = 0;

	public static byte initialLoading = 0;

	public static byte curLoadingScreen = 0;

	public static byte particleIndex1 = 0;

	public static byte particleIndex2 = 1;

	public static byte particleIndex3 = 2;

	public static byte particleIndex4 = 5;

	public static byte renderMenuScreen = 0;

	public static byte rBufferID = 0;

	public static byte uBufferID = 1;

	public static byte nextParticleLight = 0;

	public static byte viewMatrixID;

	public static byte[] popUps = new byte[1];

	public static byte[] laserDepthArray = new byte[4];

	public static sbyte colorFluctuationDirection = 1;

	public static sbyte colorFluctuationDirection2 = 1;

	public static string fpsMsg;

	public static string msg;

	public static string[] errMsg = new string[10];

	public static string[] hitIndicatorTextures;

	public static short miniMapBorderHalfWidth;

	public static short miniMapBorderHalfHeight;

	public static short miniMapBorderX;

	public static short miniMapBorderY;

	public static short miniMapX;

	public static short miniMapY;

	public static short numAllocatedVBOList;

	public static short numAllocatedAlphaVBOList;

	public static short numRenderingInstances = 0;

	public static short numAllocatedInstances = 0;

	public static short numInstanceCounts = 0;

	public static short hudIcon = 0;

	public static short[] particleLights = new short[5];

	public static short[,] closestLevelLightsIndices = new short[2, 2];

	public static ushort loadingIconX;

	public static ushort loadingIconY;

	public static ushort texVehicleSelectFrame;

	public static ushort texVehicleSelectLocked;

	public static ushort texVehicleSelectLockedSmall;

	public static ushort texWeaponSelectLocked;

	public static ushort texWeaponSelectLockedSmall;

	public static ushort numPlayerModelTextures = 0;

	public static ushort miniMapPlayerTexture = 0;

	public static ushort soldParticleIndex;

	public static ushort numMiniMapItems = 0;

	public static ushort numAllocatedMiniMapItems = 0;

	public static ushort[] hitIndicatorTextureIDs;

	public static ushort[] playerModelTextures;

	public static ushort[] wpnSelectAttachmentTypeTextures;

	public static ushort[] wpnSelectScopeTextures;

	public static ushort[] wpnSelectForeGripTextures;

	public static ushort[] wpnSelectBarrelTextures;

	public static ushort[] wpnSelectEnergyDeviceTextures;

	public static int[] errMsgStat;

	public static int[,] vboList;

	public static int[,] alphaVboList;

	public static int listIndex;

	public static int alphaListIndex;

	public static int screenWidthCenter;

	public static int screenHeightCenter;

	public static int pCount;

	public static int pStart;

	public static int numWeeds;

	public static int primitivePerWeed;

	public static int verticesPerWeed;

	public static int indicesPerWeed;

	public static float cameraShakeX;

	public static float cameraShakeY;

	public static float cameraShakeZ;

	public static float lastWeaponViewValue;

	public static float curWeaponViewTime;

	public static float weaponViewTime;

	public static float viewVelocityY;

	public static float viewPositionY;

	public static float viewVelocityX;

	public static float viewPositionX;

	public static float viewVelocityZ;

	public static float viewPositionZ;

	public static float crossHairMovementSpeed;

	public static float cameraSpringDistance;

	public static float cameraAdjustmentHeight;

	public static float skySideLength;

	public static float negativeSkySideHalfLength;

	public static float skyHeighAdjustment;

	public static float miniMapInnerRadiusInset;

	public static float miniMapInnerRadius;

	public static float miniMapInnerRadiusSqr;

	public static float watchingPlayerTimer;

	public static float cameraSpeed;

	public static float camerObjectSpeed;

	public static float miniMapZoomFactor;

	public static float miniMapPlayerOriginX;

	public static float miniMapPlayerOriginY;

	public static float miniMapScale;

	public static float miniMapItemScale;

	public static float rotateFluctation;

	public static float cosineFluctuation;

	public static float loadingScreenIconRotation;

	public static float cosineFluctuation3;

	public static float cosineFluctuationModelView;

	public static float viewSwayDir;

	public static float viewMovement;

	public static float doorSwitchScreenPos;

	public static float particleRotation1;

	public static float particleRotation2;

	public static float particleRotation3;

	public static float particleRotation4;

	public static float particleRotation6;

	public static float solidParticleRotation1;

	public static float initialCamPosX;

	public static float initialCamPosY;

	public static float initialCamPosZ;

	public static float initialCamObjX;

	public static float initialCamObjY;

	public static float initialCamObjZ;

	public static float initialWorldX;

	public static float initialWorldY;

	public static float initialWorldZ;

	public static float initialCameraRotation;

	public static float satelliteViewX;

	public static float satelliteViewY;

	public static float satelliteViewZ;

	public static float viewAdjX;

	public static float viewAdjY;

	public static float viewAdjZ;

	public static float gunRunAdjX;

	public static float gunRunAdjY;

	public static float gunRunAdjZ;

	public static float[] satelliteCrossHairX;

	public static float[] satelliteCrossHairY;

	public static float[] crossHairPosition;

	public static float[] projectionNearPlane;

	public static float[] rpmFAR;

	public static float[] hitColor;

	public static float[] texAdj1;

	public static float[] camerDir;

	public static float[] cameraPositionsX;

	public static float[] cameraPositionsY;

	public static float[] cameraPositionsZ;

	public static float[] avatarCameraPositionsX;

	public static float[] avatarCameraPositionsY;

	public static float[] avatarCameraPositionsZ;

	public static Vector4[] slFar4a;

	public static Vector4[] slFar4b;

	public static float fps;

	public static float aspectRatio;

	public static float brightness;

	public static float shadowMapHeight;

	public static float commanderTeleporterVal;

	public static float commanderTeleporterEnergyVal;

	public static float scopeValue;

	public static float middleOfScreenX;

	public static float middleOfScreenY;

	public static float middleOfScreenLenghtToCorner;

	public static float[,] crossHairPositionGoal;

	public static float[,] newParticle;

	public static double frameend;

	public static double framestart;

	public static StructsClass.vtex npn;

	public static StructsClass.vtex[] eyeVec;

	public static StructsClass.particle_effect[,] particles;

	public static StructsClass.solid_particle_effect[,] solidParticles;

	public static StructsClass.JointCollection cameraJointCollection;

	public static StructsClass.joint[] cameraJoint;

	public static StructsClass.program_instance[] cameraProgram;

	public static StructsClass.Hit_Indicator hitIndicatorConfig;

	public static StructsClass.Hit_Indicator_Instance[] hitIndicators;

	public static StructsClass.Muzzle_Flash[,] muzzleFlashes;

	public static StructsClass.sort_list[] pSort;

	public static string[,] dialogUpdate;

	public static float[] texAdj;

	public static bool updateFps;

	public static double t1;

	public static double t2;

	public static StructsClass.RenderInstance[] renderingInstances;

	public static StructsClass.MiniMapItem[] mapItems;

	public static Vector4[] pos;

	public static Vector4[] scale;

	public static Vector4[] pcolor;

	public static Vector3 rgtV2;

	public static Vector3 v3;

	public static Vector3 v4;

	public static Vector2 rgtV1;

	public static Matrix[] effectMatrix;

	public static Matrix vehicleSelectMatrix;

	public static Matrix weaponSelectMatrix;

	public static Matrix weaponSelectMatrix2;

	public static Matrix weaponSelectMatrix3;

	public static byte showCollisionBoxes;

	public static bool usingVsync;

	public static bool shadersAvail;

	public static bool multiTexAvail;

	public static bool usingShaders;

	public static bool showParticleBox;

	public static float[,] ptLight_lvl;

	public static float[,] ptLightColor_lvl;

	public static float[,] ptLightDistance_lvl;

	public static byte numPtLight_lvl;

	public static byte allocatedPtLight_lvl;

	public static int specular;

	public static Vector3 diffuseLight;

	public static Vector3 specularLight;

	public static Vector3 LightPosition;

	public static float[] ptLight0;

	public static float[] ptLight1;

	public static float[] ambientLevel;

	public static float[] ambientAvatar;

	public static float[] directionalLightVector;

	public static float[] directionalLightColor;

	public static float[] directionalLightBounce;

	public static float[] ambient0;

	public static float[] ptLightDir0;

	public static float[] ptLightColor1;

	public static float[] fau3;

	public static float[] fau4;

	public static float[] far3;

	public static float[] far4;

	public static float[] far4b;

	public static float[] splFar4;

	public static Vector4[] rFar4a;

	public static Vector4[] rFar4b;

	public static float[,,] particleLightLocation;

	public static float[,,] particleLightColor;

	public static float[,] particleDistance;

	public static Vector3[] worldPos;

	public static Matrix mvRendering;

	public static Matrix mvRendering2;

	public static byte[] fb;

	public static byte[] fb2;

	public static int fbia;

	public static bool showBoundingBox;

	public static float curFps;

	public static float curFpsTotal;

	public static int fpsCnt;

	public static int fpsCntTo;

	public static byte curParticleID;

	public static Matrix matrixP;

	public static Matrix matrixV;

	public static Matrix matrixVInverse;

	public static Matrix matrixO;

	public static Matrix matrixW;

	public static Matrix matrixI;

	public static Matrix matrixVP;

	public static Matrix matrixShadow;

	public static Matrix matrixShadowMain;

	public static Matrix[] matrixVDB;

	public static Matrix[] matrixPDB;

	public static GraphicsDevice rGraphics;

	public static VertexDeclaration rDecVPCNT;

	public static BasicEffect rEffect;

	private static Color hsrC1;

	private static StructsClass.vtex[] vForward;

	public static VertexBuffer mainVBO;

	public static VertexBuffer mainAlphaVBO;

	public static IndexBuffer mainIndexBuffer;

	public static IndexBuffer mainAlphaIndexBuffer;

	public static IndexBuffer particleIndexBuffer;

	public static StructsClass.VertexPositionColorNormalTexture[] vertexArray;

	public static int[] vertexIndexArray;

	public static int numStaticVertices;

	public static int numStaticIndexes;

	public static int numStaticAlphaVertices;

	public static int numStaticAlphaIndexes;

	public static StructsClass.VertexPositionColorNormalTexture[] playerHitVtex;

	public static int[] playerHitIndex;

	public static Vector3[] camPos;

	public static Vector3[] camPosGoal;

	public static Vector3[] camObject;

	public static Vector3[] camObjectGoal;

	public static Vector3[] camUp;

	public static Vector3 camPosShadowMap;

	public static Vector3 camPosShadowMapUp;

	public static Vector3 worldUp;

	public static Color backColor;

	public static Texture2D splashScreen;

	public static Texture2D texLoadingScreenIcon;

	public static Texture2D texLoadingScreenSkip;

	public static Texture2D texLoadingScreenPlay;

	public static Texture2D shadowMap;

	public static Texture2D shadowMapPlayer;

	public static Texture2D depthTexureLaser;

	public static Texture2D texButtonA;

	public static Texture2D[] splashTexture;

	public static Rectangle splashRect;

	public static SpriteBatch splashSprite;

	public static Vector2 splashPos;

	public static Vector2 rsPos;

	public static Vector2 tauntPos;

	public static Vector2 swapWeaponPos;

	public static Color cMenu;

	public static Color cWhite;

	public static Color cBlack;

	public static Color cRed;

	public static Color cGreen;

	public static Color cBlue;

	public static Color cYellow;

	public static Color miniMapRed;

	public static Color miniMapBlue;

	public static Color barColor;

	public static Color vecColor;

	public static int[] vecQuadInd;

	public static Effect effect1;

	private static StructsClass.VertexPositionColorNormalTexture[] planeVtex;

	private static StructsClass.VertexPositionColorNormalTexture[] scoreBoardVtex;

	private static StructsClass.VertexPositionColorNormalTexture[] ScopeVtex;

	public static RenderTarget2D renderTargetPlayer;

	public static RenderTarget2D renderTargetWorld;

	public static RenderTarget2D renderTargetLaser;

	public static BlendState blendSourceAlpha;

	public static BlendState blendSourceOne;

	public static BlendState blendSolidAlpha;

	public static DepthStencilState depthBufferEnabled;

	public static DepthStencilState depthBufferWriteDisabled;

	public static DepthStencilState depthBufferDisabled;

	public static RasterizerState rasterizerState;

	public static SamplerState textureSamplerState;

	public static SamplerState textureSamplerStatePoint;

	public static SamplerState textureSamplerStateClamp;

	public Game1.MasterCollection mainC;

	public void Init_Vars(Game1.MasterCollection master, GraphicsDevice graphics)
	{
		mainC = master;
		rGraphics = graphics;
		textureSamplerState.AddressU = TextureAddressMode.Wrap;
		textureSamplerState.AddressV = TextureAddressMode.Wrap;
		textureSamplerState.AddressW = TextureAddressMode.Wrap;
		textureSamplerState.Filter = TextureFilter.MinPointMagLinearMipLinear;
		textureSamplerState.MaxAnisotropy = 8;
		loadingIconX = 640;
		loadingIconY = 360;
		depthBufferEnabled.DepthBufferEnable = true;
		depthBufferEnabled.DepthBufferWriteEnable = true;
		depthBufferEnabled.DepthBufferFunction = CompareFunction.LessEqual;
		rasterizerState.CullMode = CullMode.CullClockwiseFace;
		rasterizerState.FillMode = FillMode.Solid;
		ref Matrix reference = ref matrixVDB[0];
		reference = Matrix.Identity;
		ref Matrix reference2 = ref matrixVDB[1];
		reference2 = Matrix.Identity;
		ref Matrix reference3 = ref matrixPDB[0];
		reference3 = Matrix.Identity;
		ref Matrix reference4 = ref matrixPDB[1];
		reference4 = Matrix.Identity;
		blendSourceAlpha.ColorBlendFunction = BlendFunction.Add;
		blendSourceAlpha.ColorSourceBlend = Blend.SourceAlpha;
		blendSourceAlpha.ColorDestinationBlend = Blend.InverseSourceAlpha;
		blendSourceAlpha.AlphaBlendFunction = BlendFunction.Add;
		blendSourceAlpha.AlphaSourceBlend = Blend.SourceAlpha;
		blendSourceAlpha.AlphaDestinationBlend = Blend.InverseSourceAlpha;
		blendSourceOne.ColorBlendFunction = BlendFunction.Add;
		blendSourceOne.ColorSourceBlend = Blend.One;
		blendSourceOne.ColorDestinationBlend = Blend.Zero;
		blendSourceOne.AlphaBlendFunction = BlendFunction.Add;
		blendSourceOne.AlphaSourceBlend = Blend.One;
		blendSourceOne.AlphaDestinationBlend = Blend.Zero;
		rDecVPCNT = new VertexDeclaration(StructsClass.VertexElements);
		matrixP = Matrix.CreatePerspectiveFieldOfView((float)Math.PI / 4f, (float)rGraphics.Viewport.Width / (float)rGraphics.Viewport.Height, 0.1f, 10000f);
		matrixO = Matrix.CreateOrthographic(rGraphics.Viewport.Width, rGraphics.Viewport.Height, 0.01f, 10000f);
		matrixW = Matrix.Identity;
		matrixI = Matrix.Identity;
		matrixV = Matrix.Identity;
		matrixVP = Matrix.Identity;
		middleOfScreenX = (float)rGraphics.Viewport.Width / 2f;
		middleOfScreenY = (float)rGraphics.Viewport.Height / 2f;
		rEffect = new BasicEffect(rGraphics);
		for (int i = 0; i < 1500; i++)
		{
			particles[0, i] = new StructsClass.particle_effect();
			StructsClass.Initialize_ParticleEffect(particles[0, i]);
			particles[1, i] = new StructsClass.particle_effect();
			StructsClass.Initialize_ParticleEffect(particles[1, i]);
			pSort[i] = default(StructsClass.sort_list);
		}
		for (int i = 0; i < 50; i++)
		{
			solidParticles[0, i] = new StructsClass.solid_particle_effect();
			StructsClass.Initialize_Solid_ParticleEffect(solidParticles[0, i]);
			solidParticles[1, i] = new StructsClass.solid_particle_effect();
			StructsClass.Initialize_Solid_ParticleEffect(solidParticles[1, i]);
		}
		for (int i = 0; i < 1; i++)
		{
			splashTexture[i] = mainC.curGame.CM_Loading.Load<Texture2D>("Textures\\UI_Loading_" + i);
		}
		splashScreen = splashTexture[0];
		texLoadingScreenIcon = mainC.curGame.CM_Loading.Load<Texture2D>("Textures\\UI_Loading_Icon");
		texLoadingScreenSkip = mainC.curGame.CM_Loading.Load<Texture2D>("Textures\\UI_Loading_Skip");
		texLoadingScreenPlay = mainC.curGame.CM_Loading.Load<Texture2D>("Textures\\UI_Icon_PlayIntro");
		texButtonA = mainC.curGame.CM_Loading.Load<Texture2D>("Textures\\UI_Button_A");
		splashRect = new Rectangle(rGraphics.Viewport.X, rGraphics.Viewport.Y, rGraphics.Viewport.Width, rGraphics.Viewport.Height);
		splashPos = new Vector2(splashRect.Left, splashRect.Top);
		splashSprite = new SpriteBatch(rGraphics);
		playerHitVtex = new StructsClass.VertexPositionColorNormalTexture[4];
		playerHitIndex = new int[6];
		playerHitIndex[0] = 0;
		playerHitIndex[1] = 1;
		playerHitIndex[2] = 2;
		playerHitIndex[3] = 0;
		playerHitIndex[4] = 2;
		playerHitIndex[5] = 3;
		for (int i = 0; i < 2; i++)
		{
			eyeVec[i] = new StructsClass.vtex();
		}
		for (int i = 0; i < 5; i++)
		{
			vForward[i] = new StructsClass.vtex();
		}
		effect1 = mainC.curGame.Content.Load<Effect>("Effect_Main");
	}

	public void ExitSP()
	{
		Render_FPS_Message();
		mainC.fontmoduleMain.Reset_Graphics_Adapter_After_SpriteBatch();
		global::Threads.Threads.thread1End.WaitOne();
		if (renderMenuScreen == 0 || global::MainGame.MainGame.gameMode == 1)
		{
			rBufferID = uBufferID;
			uBufferID = (byte)((uBufferID + 1) % 2);
		}
	}

	public void Render_Scene(GameTime gTime)
	{
		bool flag = false;
		float num = 0f;
		float frametime = global::MainGame.MainGame.frametime;
		if (initialLoading != 3)
		{
			if (global::MainGame.MainGame.loadGlobalSettings)
			{
				mainC.maingameMain.Load_Global_Settings();
			}
			mainC.gameLogic.Game_Load_In_Progress();
			rBufferID = uBufferID;
			uBufferID = (byte)((uBufferID + 1) % 2);
			cosineFluctuation += global::MainGame.MainGame.frametime;
			cosineFluctuation3 += 1f;
			if (cosineFluctuation > 6f && cosineFluctuation3 > 3f)
			{
				if (curLoadingScreen >= 0)
				{
					curLoadingScreen = (byte)(1f * ((float)global::MainGame.MainGame.mainRandom.Next(0, 32000) / 32000f));
				}
				else
				{
					curLoadingScreen++;
				}
				if (curLoadingScreen >= 1)
				{
					curLoadingScreen = 0;
				}
				if (curLoadingScreen == byte.MaxValue && !global::MainGame.MainGame.trialMode)
				{
					curLoadingScreen++;
				}
				if (curLoadingScreen == 0)
				{
					curLoadingScreen++;
				}
				if (curLoadingScreen >= 1)
				{
					curLoadingScreen = 0;
				}
				splashScreen = splashTexture[curLoadingScreen];
				cosineFluctuation = 0f;
				cosineFluctuation3 = 5f;
			}
			if (initialLoading == 1 && !global::MainGame.MainGame.loadGlobalSettings)
			{
				initialLoading = 2;
				cosineFluctuation = 0f;
				loadingScreenIconRotation = 0f;
				cosineFluctuation3 = 0f;
				mainC.gameLogic.Game_Load_Finished();
			}
			if (initialLoading == 2 && mainC.gameLogic.Game_Ready_To_Show_Main_Menu())
			{
				mainC.userInterface.Load_Main_Menu();
				initialLoading = 3;
			}
			return;
		}
		rGraphics.Clear(ClearOptions.Target | ClearOptions.DepthBuffer, backColor, 1f, 0);
		cosineFluctuation += frametime * (float)colorFluctuationDirection;
		if (cosineFluctuation > 1f)
		{
			cosineFluctuation = 2f - cosineFluctuation;
			colorFluctuationDirection = -1;
		}
		else if (cosineFluctuation < 0f)
		{
			cosineFluctuation *= -1f;
			colorFluctuationDirection = 1;
		}
		rotateFluctation += frametime;
		if (rotateFluctation > 1f)
		{
			rotateFluctation -= 1f;
		}
		Matrix matrix = Matrix.CreateRotationZ(rotateFluctation * ((float)Math.PI * 2f));
		if ((renderMenuScreen & 0x23) > 0)
		{
			if ((renderMenuScreen & 0x10) != 16)
			{
				if ((renderMenuScreen & 0x20) == 32)
				{
					Render_Splash();
					Render_Loading_Graphic();
					mainC.gameLogic.Game_Render_Splash();
					mainC.fontmoduleMain.Reset_Graphics_Adapter_After_SpriteBatch();
					global::Threads.Threads.thread1End.WaitOne();
					rBufferID = uBufferID;
					uBufferID = (byte)((uBufferID + 1) % 2);
					return;
				}
				rGraphics.Clear(ClearOptions.Target | ClearOptions.DepthBuffer, Color.Black, 1f, 0);
				Render_Splash();
				Render_Loading_Graphic();
				mainC.gameLogic.Game_Render_Splash();
				splashSprite.Begin();
				rsPos.X = 606f;
				rsPos.Y = 371f;
				splashSprite.End();
			}
			mainC.userInterface.Render_Windows();
			mainC.fontmoduleMain.Reset_Graphics_Adapter_After_SpriteBatch();
			if (global::InputHandler.InputHandler.menuType == 0 || (renderMenuScreen & 2) == 2)
			{
				global::Threads.Threads.thread1End.WaitOne();
				rBufferID = uBufferID;
				uBufferID = (byte)((uBufferID + 1) % 2);
				return;
			}
		}
		effect1.Parameters["MaterialTexture"].SetValue(global::Textures.Textures.texMain.texData[global::Textures.Textures.texDefaultNormalMap]);
		effect1.Parameters["SpecularTexture"].SetValue(global::Textures.Textures.texMain.texData[global::Textures.Textures.texDefaultSpecularMap]);
		effect1.Parameters["Specular"].SetValue(specular);
		effect1.Parameters["clipTexture"].SetValue(value: false);
		cameraShakeX = 0f;
		cameraShakeY = 0f;
		cameraShakeZ = 0f;
		float value;
		if (viewPositionX != 0f)
		{
			global::Weapons.Weapons.recoilSide = 0f;
			value = viewPositionX + viewVelocityX * global::MainGame.MainGame.frametime / global::Physics.Physics.timeMod;
			if (Math.Sign(value) != Math.Sign(viewPositionX))
			{
				viewPositionX = 0f;
			}
			else
			{
				viewPositionX = value;
				cameraShakeX = matrixVInverse.M11 * viewPositionX;
				cameraShakeY = matrixVInverse.M12 * viewPositionX;
				cameraShakeZ = matrixVInverse.M13 * viewPositionX;
			}
		}
		if (viewPositionY != 0f)
		{
			global::Weapons.Weapons.recoilUp = 0f;
			value = viewPositionY + viewVelocityY * global::MainGame.MainGame.frametime / global::Physics.Physics.timeMod;
			if (Math.Sign(value) != Math.Sign(viewPositionY))
			{
				viewPositionY = 0f;
			}
			else
			{
				viewPositionY = value;
				cameraShakeX += matrixVInverse.M21 * viewPositionY;
				cameraShakeY += matrixVInverse.M22 * viewPositionY;
				cameraShakeZ += matrixVInverse.M23 * viewPositionY;
			}
		}
		if (viewPositionZ != 0f)
		{
			global::Weapons.Weapons.recoilBack = 0f;
			value = viewPositionZ + viewVelocityZ * global::MainGame.MainGame.frametime / global::Physics.Physics.timeMod;
			if (Math.Sign(value) != Math.Sign(viewPositionZ))
			{
				viewPositionZ = 0f;
			}
			else
			{
				viewPositionZ = value;
				if (global::Players.Players.currentView != 2)
				{
					cameraShakeX += matrixVInverse.M31 * viewPositionZ;
					cameraShakeY += matrixVInverse.M32 * viewPositionZ;
					cameraShakeZ += matrixVInverse.M33 * viewPositionZ;
				}
			}
		}
		global::Players.Players.players[0].posX[rBufferID] += cameraShakeX;
		global::Players.Players.players[0].posY[rBufferID] += cameraShakeY;
		global::Players.Players.players[0].posZ[rBufferID] += cameraShakeZ;
		camPos[rBufferID].Z += matrixVInverse.M13 * viewPositionX + matrixVInverse.M23 * viewPositionY + matrixVInverse.M33 * viewPositionZ;
		camObject[rBufferID].Z += matrixVInverse.M13 * viewPositionX + matrixVInverse.M23 * viewPositionY + matrixVInverse.M33 * viewPositionZ;
		camPos[rBufferID].X += matrixVInverse.M11 * viewPositionX + matrixVInverse.M21 * viewPositionY + matrixVInverse.M31 * viewPositionZ;
		camPos[rBufferID].Y += matrixVInverse.M12 * viewPositionX + matrixVInverse.M22 * viewPositionY + matrixVInverse.M32 * viewPositionZ;
		camObject[rBufferID].X += matrixVInverse.M11 * viewPositionX + matrixVInverse.M21 * viewPositionY + matrixVInverse.M31 * viewPositionZ;
		camObject[rBufferID].Y += matrixVInverse.M12 * viewPositionX + matrixVInverse.M22 * viewPositionY + matrixVInverse.M32 * viewPositionZ;
		byte eyeJoint = global::Players.Players.eyeJoint;
		byte headJoint = global::Players.Players.headJoint;
		_ = global::Players.Players.humanoidBackJoint;
		matrixV = Matrix.CreateLookAt(camPos[rBufferID], camObject[rBufferID], camUp[rBufferID]);
		if (float.IsNaN(matrixV.M11) || float.IsNaN(matrixV.M21))
		{
			matrixV = Matrix.CreateLookAt(new Vector3(0f, 0f, 0f), new Vector3(0f, 1f, 0f), new Vector3(0f, 0f, 1f));
		}
		camPos[rBufferID].Z -= matrixVInverse.M13 * viewPositionX + matrixVInverse.M23 * viewPositionY + matrixVInverse.M33 * viewPositionZ;
		camObject[rBufferID].Z -= matrixVInverse.M13 * viewPositionX + matrixVInverse.M23 * viewPositionY + matrixVInverse.M33 * viewPositionZ;
		camPos[rBufferID].X -= matrixVInverse.M11 * viewPositionX + matrixVInverse.M21 * viewPositionY + matrixVInverse.M31 * viewPositionZ;
		camPos[rBufferID].Y -= matrixVInverse.M12 * viewPositionX + matrixVInverse.M22 * viewPositionY + matrixVInverse.M32 * viewPositionZ;
		camObject[rBufferID].X -= matrixVInverse.M11 * viewPositionX + matrixVInverse.M21 * viewPositionY + matrixVInverse.M31 * viewPositionZ;
		camObject[rBufferID].Y -= matrixVInverse.M12 * viewPositionX + matrixVInverse.M22 * viewPositionY + matrixVInverse.M32 * viewPositionZ;
		matrixVInverse = Matrix.Invert(matrixV);
		matrixP = Matrix.CreatePerspectiveFieldOfView(scopeValue, aspectRatio, projectionNearPlane[rBufferID], 3200000f);
		matrixVP = matrixV * matrixP;
		flag = false;
		if (global::MainGame.MainGame.showCrossHairs[0] + global::MainGame.MainGame.showCrossHairs[1] + global::MainGame.MainGame.showCrossHairs[2] + global::MainGame.MainGame.showCrossHairs[3] == 0)
		{
			flag = true;
		}
		ref Matrix reference = ref matrixVDB[rBufferID];
		reference = matrixV;
		ref Matrix reference2 = ref matrixPDB[rBufferID];
		reference2 = matrixP;
		effect1.Parameters["numAmmoLights"].SetValue(global::Weapons.Weapons.numActiveAmmoLights[rBufferID]);
		for (int i = 0; i < global::Weapons.Weapons.numActiveAmmoLights[rBufferID]; i++)
		{
			int num2 = global::Weapons.Weapons.laserLightsSorted[rBufferID, i];
			rFar4a[i].X = global::Weapons.Weapons.ammoLightPos[rBufferID, num2].X;
			rFar4a[i].Y = global::Weapons.Weapons.ammoLightPos[rBufferID, num2].Y;
			rFar4a[i].Z = global::Weapons.Weapons.ammoLightPos[rBufferID, num2].Z;
			rFar4a[i].W = global::Weapons.Weapons.ammoLightPos[rBufferID, num2].W;
			rFar4b[i].X = global::Weapons.Weapons.ammoLightColor[rBufferID, num2].X;
			rFar4b[i].Y = global::Weapons.Weapons.ammoLightColor[rBufferID, num2].Y;
			rFar4b[i].Z = global::Weapons.Weapons.ammoLightColor[rBufferID, num2].Z;
			rFar4b[i].W = global::Weapons.Weapons.ammoLightColor[rBufferID, num2].W;
		}
		effect1.Parameters["AmmoLight"].SetValue(rFar4a);
		effect1.Parameters["AmmoLightColor"].SetValue(rFar4b);
		far3[0] = global::Players.Players.players[0].laserPos[rBufferID, 0];
		far3[1] = global::Players.Players.players[0].laserPos[rBufferID, 1];
		far3[2] = global::Players.Players.players[0].laserPos[rBufferID, 2];
		effect1.Parameters["LaserLight0"].SetValue(far3);
		far3[0] = global::Players.Players.players[0].laserDir[rBufferID, 0];
		far3[1] = global::Players.Players.players[0].laserDir[rBufferID, 1];
		far3[2] = global::Players.Players.players[0].laserDir[rBufferID, 2];
		effect1.Parameters["LaserLightDirection0"].SetValue(far3);
		value = global::Players.Players.players[0].laserDist[rBufferID];
		value += 20f + value * 0.1f;
		effect1.Parameters["LaserLightDistance0"].SetValue(value);
		ptLight0[0] = global::Players.Players.players[0].posX[rBufferID] + 10f * global::Players.Players.players[0].jt1[headJoint].mv[rBufferID].M21 + 5f * global::Players.Players.players[0].jt1[headJoint].mv[rBufferID].M31 + global::Players.Players.players[0].jt1[headJoint].mv[rBufferID].M41;
		ptLight0[1] = global::Players.Players.players[0].posY[rBufferID] + 10f * global::Players.Players.players[0].jt1[headJoint].mv[rBufferID].M22 + 5f * global::Players.Players.players[0].jt1[headJoint].mv[rBufferID].M32 + global::Players.Players.players[0].jt1[headJoint].mv[rBufferID].M42;
		ptLight0[2] = global::Players.Players.players[0].posZ[rBufferID] + 10f * global::Players.Players.players[0].jt1[headJoint].mv[rBufferID].M23 + 5f * global::Players.Players.players[0].jt1[headJoint].mv[rBufferID].M33 + global::Players.Players.players[0].jt1[headJoint].mv[rBufferID].M43;
		ptLightDir0[0] = global::Players.Players.players[0].jt1[eyeJoint].mv[rBufferID].M31;
		ptLightDir0[1] = global::Players.Players.players[0].jt1[eyeJoint].mv[rBufferID].M32;
		ptLightDir0[2] = global::Players.Players.players[0].jt1[eyeJoint].mv[rBufferID].M33;
		effect1.Parameters["PtLight0"].SetValue(ptLight0);
		effect1.Parameters["PtLightDirection0"].SetValue(ptLightDir0);
		eyeVec[rBufferID].v[0] = global::Players.Players.players[0].jt1[global::Players.Players.playerViewJoint1].mv[rBufferID].M21;
		eyeVec[rBufferID].v[1] = global::Players.Players.players[0].jt1[global::Players.Players.playerViewJoint1].mv[rBufferID].M22;
		eyeVec[rBufferID].v[2] = global::Players.Players.players[0].jt1[global::Players.Players.playerViewJoint1].mv[rBufferID].M23;
		mvRendering2 = Matrix.Invert(matrixV);
		camerDir[0] = mvRendering2.M31;
		camerDir[1] = mvRendering2.M32;
		camerDir[2] = mvRendering2.M33;
		effect1.Parameters["cameraDir"].SetValue(camerDir);
		Set_Level_Lights_Closest_To_Player();
		if (useShadowMap)
		{
			rGraphics.SamplerStates[0] = textureSamplerStatePoint;
			effect1.CurrentTechnique = effect1.Techniques["ShadowMap"];
			global::Weapons.Weapons.laserDepth[rBufferID] = byte.MaxValue;
			if (needLevelShadowMap)
			{
				Vector3 cameraTarget = default(Vector3);
				Vector3 vector = default(Vector3);
				Vector3 cameraUpVector = new Vector3(0f, 1f, 0f);
				rGraphics.SetRenderTarget(renderTargetWorld);
				rGraphics.Clear(ClearOptions.Target | ClearOptions.DepthBuffer, Color.White, 1f, 0);
				value = global::MainGame.MainGame.MaxRight - global::MainGame.MainGame.MaxLeft;
				float num3 = global::MainGame.MainGame.MaxForward - global::MainGame.MainGame.MaxRear;
				camPosShadowMap.X = global::MainGame.MainGame.MaxLeft + value / 2f;
				camPosShadowMap.Y = global::MainGame.MainGame.MaxRear + num3 / 2f;
				camPosShadowMap.Z = shadowMapHeight;
				cameraTarget.X = camPosShadowMap.X - directionalLightVector[0];
				cameraTarget.Y = camPosShadowMap.Y - directionalLightVector[1];
				cameraTarget.Z = camPosShadowMap.Z - directionalLightVector[2];
				vector.X = 0f - directionalLightVector[1];
				vector.Y = directionalLightVector[0];
				vector.Z = 0f;
				if (vector.X != 0f || vector.Y != 0f)
				{
					cameraUpVector.X = vector.Y * (0f - directionalLightVector[2]);
					cameraUpVector.Y = vector.X * (0f - directionalLightVector[2]);
					cameraUpVector.Z = vector.X * (0f - directionalLightVector[1]) - vector.Y * (0f - directionalLightVector[0]);
				}
				num = (float)Math.Sqrt(cameraUpVector.X * cameraUpVector.X + cameraUpVector.Y * cameraUpVector.Y + cameraUpVector.Z * cameraUpVector.Z);
				if (num != 0f)
				{
					cameraUpVector.X /= num;
					cameraUpVector.Y /= num;
					cameraUpVector.Z /= num;
				}
				matrixShadowMain = Matrix.CreateLookAt(camPosShadowMap, cameraTarget, cameraUpVector) * Matrix.CreateOrthographic(value, num3, 1f, shadowMapHeight);
				cameraTarget.X = camPosShadowMap.X;
				cameraTarget.Y = camPosShadowMap.Y;
				cameraTarget.Z = camPosShadowMap.Z - 1f;
				matrixShadowMain = Matrix.CreateLookAt(camPosShadowMap, cameraTarget, Vector3.Up) * Matrix.CreateOrthographic(value, num3, 1f, shadowMapHeight);
				if (numStaticVertices > 0)
				{
					rGraphics.SetVertexBuffer(mainVBO);
					rGraphics.Indices = mainIndexBuffer;
					rGraphics.BlendState = BlendState.Opaque;
					effect1.Parameters["ShadowProjection"].SetValue(matrixShadowMain);
					effect1.CurrentTechnique.Passes[0].Apply();
					for (long num4 = 0L; num4 < numAllocatedVBOList && vboList[(int)checked((nint)num4), 1] > -1 && vboList[(int)checked((nint)num4), 5] == 1; num4++)
					{
						rGraphics.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, vboList[(int)checked((nint)num4), 1], vboList[(int)checked((nint)num4), 3], vboList[(int)checked((nint)num4), 2], vboList[(int)checked((nint)num4), 4]);
					}
				}
				if (numStaticAlphaVertices > 0)
				{
					rGraphics.SetVertexBuffer(mainAlphaVBO);
					rGraphics.Indices = mainAlphaIndexBuffer;
					for (long num4 = 0L; num4 < global::Textures.Textures.numAlphaTextures && alphaVboList[(int)checked((nint)num4), 1] > -1 && alphaVboList[(int)checked((nint)num4), 5] == 1; num4++)
					{
						rGraphics.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, alphaVboList[(int)checked((nint)num4), 1], alphaVboList[(int)checked((nint)num4), 3], alphaVboList[(int)checked((nint)num4), 2], alphaVboList[(int)checked((nint)num4), 4]);
					}
				}
				mainC.terrainMain.Render_Terrain_Shadowmap();
				rGraphics.SetRenderTarget(null);
				shadowMap = renderTargetWorld;
				effect1.Parameters["ShadowMapMainTexture"].SetValue(shadowMap);
				effect1.Parameters["ShadowMainProjection"].SetValue(matrixShadowMain);
				effect1.CurrentTechnique.Passes[0].Apply();
				needLevelShadowMap = false;
			}
			rGraphics.SetRenderTarget(renderTargetPlayer);
			rGraphics.Clear(ClearOptions.Target | ClearOptions.DepthBuffer, Color.White, 1f, 0);
			camPosShadowMap.X = global::Players.Players.players[watchingPlayer].posX[rBufferID];
			camPosShadowMap.Y = global::Players.Players.players[watchingPlayer].posY[rBufferID];
			camPosShadowMap.Z = global::Players.Players.players[watchingPlayer].posZ[rBufferID];
			camPosShadowMapUp.X = camPosShadowMap.X + directionalLightVector[0] * 100f;
			camPosShadowMapUp.Y = camPosShadowMap.Y + directionalLightVector[1] * 100f;
			camPosShadowMapUp.Z = camPosShadowMap.Z + directionalLightVector[2] * 100f;
			matrixShadow = Matrix.CreateLookAt(camPosShadowMapUp, camPosShadowMap, worldUp) * Matrix.CreatePerspectiveFieldOfView(scopeValue, aspectRatio, 85f, 150f);
			if ((global::Players.Players.players[watchingPlayer].onmap & 0xC) > 0)
			{
				effect1.Parameters["ShadowProjection"].SetValue(matrixShadow);
				Matrix matrix2 = Matrix.CreateTranslation(global::Players.Players.players[watchingPlayer].posX[rBufferID], global::Players.Players.players[watchingPlayer].posY[rBufferID], global::Players.Players.players[watchingPlayer].posZ[rBufferID]);
				long num5 = global::Joints.Joints.playerJoints[global::Players.Players.players[watchingPlayer].jointPackage].numJoints;
				for (long num6 = 0L; num6 < num5; num6++)
				{
					ref Matrix reference3 = ref effectMatrix[num6];
					reference3 = global::Players.Players.players[watchingPlayer].jt1[num6].mv[rBufferID] * matrix2;
				}
				effect1.Parameters["Matrix"].SetValue(effectMatrix);
				int j = 0;
				for (int numModels = Vehicles.vehicles[global::Players.Players.players[watchingPlayer].curVehicle].numModels; j < numModels; j++)
				{
					mainC.modelsMain.Render_Rigged_Model_For_ShadowMap(global::Players.Players.players[watchingPlayer].playerModel[j]);
				}
			}
			rGraphics.SetRenderTarget(null);
			shadowMapPlayer = renderTargetPlayer;
			rGraphics.Clear(ClearOptions.Target | ClearOptions.DepthBuffer, backColor, 1f, 0);
			effect1.Parameters["ShadowProjection"].SetValue(matrixShadow);
			effect1.Parameters["ShadowMapTexture"].SetValue(shadowMapPlayer);
		}
		rGraphics.SamplerStates[0] = textureSamplerState;
		effect1.Parameters["ViewProjection"].SetValue(matrixVP);
		effect1.Parameters["World"].SetValue(matrixW);
		rGraphics.SetVertexBuffer(mainVBO);
		rGraphics.Indices = mainIndexBuffer;
		effect1.CurrentTechnique = effect1.Techniques["Main"];
		effect1.CurrentTechnique.Passes[0].Apply();
		if ((global::Players.Players.players[0].onmap & 0x1C) > 0 && global::Players.Players.players[0].renderWeapon == 0)
		{
			mainC.weaponsMain.Render_Player_Weapon(0);
		}
		if (renderMainPlayer[rBufferID])
		{
			Matrix matrix2 = Matrix.CreateTranslation(global::Players.Players.players[0].posX[rBufferID], global::Players.Players.players[0].posY[rBufferID], global::Players.Players.players[0].posZ[rBufferID]);
			if (global::MainGame.MainGame.playerVehicles[global::Players.Players.players[0].curVehicle].type != 8)
			{
				rGraphics.BlendState = BlendState.Opaque;
				rGraphics.SetVertexBuffer(global::Models.Models.mVertexBufferObjects);
				rGraphics.Indices = global::Models.Models.mIndexBufferObjects;
				effect1.CurrentTechnique = effect1.Techniques["Matrices"];
				long num5 = global::Joints.Joints.playerJoints[global::Players.Players.players[0].jointPackage].numJoints;
				for (long num6 = 0L; num6 < num5; num6++)
				{
					ref Matrix reference4 = ref effectMatrix[num6];
					reference4 = global::Joints.Joints.playerJoints[global::Players.Players.players[0].jointPackage].InvBindPose[num6] * global::Players.Players.players[0].jt1[num6].mv[rBufferID] * matrix2;
				}
				effect1.Parameters["Matrix"].SetValue(effectMatrix);
				int j = 0;
				int num7 = 0;
				for (int numModels = Vehicles.vehicles[global::Players.Players.players[0].curVehicle].numModels; j < numModels; j++)
				{
					int num2 = 0;
					while (num2 < global::Models.Models.mod1[global::Players.Players.players[0].playerModel[j]].numTextures)
					{
						mainC.modelsMain.Render_Rigged_Model_Texture(global::Players.Players.players[0].playerModel[j], global::Players.Players.players[0].textureNormalID, global::Players.Players.players[0].textureSpecularID, (byte)num2, (byte)global::Players.Players.players[0].textureID[num7]);
						num2++;
						num7++;
					}
				}
			}
			if (global::Players.Players.players[0].invincible)
			{
				matrix2.M43 += global::Players.Players.playerRaces[global::Players.Players.players[0].race].iconHeight[global::Players.Players.players[0].type];
				matrix2 = matrix * matrix2;
				mainC.modelsMain.Render_Model(global::Models.Models.modShield, ref matrix2);
				effect1.Parameters["World"].SetValue(matrixW);
			}
		}
		if (!global::MainGame.MainGame.commanderMode)
		{
			rGraphics.BlendState = BlendState.Opaque;
			rGraphics.SetVertexBuffer(global::Models.Models.mVertexBufferObjects);
			rGraphics.Indices = global::Models.Models.mIndexBufferObjects;
			effect1.CurrentTechnique = effect1.Techniques["Matrices"];
			for (int i = 0; i < numPlayerModelTextures; i++)
			{
				for (int k = 1; k < global::MainGame.MainGame.maxGamePlayers; k++)
				{
					if ((global::Players.Players.players[k].onmap & 0xC) <= 0 || global::MainGame.MainGame.playerVehicles[global::Players.Players.players[k].curVehicle].type == 8)
					{
						continue;
					}
					int j = 0;
					int num7 = 0;
					for (int numModels = Vehicles.vehicles[global::Players.Players.players[k].curVehicle].numModels; j < numModels; j++)
					{
						int num2 = 0;
						while (num2 < global::Models.Models.mod1[global::Players.Players.players[k].playerModel[j]].numTextures)
						{
							if (global::Players.Players.players[k].textureID[num7] == playerModelTextures[i])
							{
								Matrix matrix2 = Matrix.CreateScale(global::Players.Players.players[k].renderScale, global::Players.Players.players[k].renderScale, global::Players.Players.players[k].renderScale) * Matrix.CreateTranslation(global::Players.Players.players[k].posX[rBufferID], global::Players.Players.players[k].posY[rBufferID], global::Players.Players.players[k].posZ[rBufferID]);
								long num5 = global::Joints.Joints.playerJoints[global::Players.Players.players[k].jointPackage].numJoints;
								for (long num6 = 0L; num6 < num5; num6++)
								{
									ref Matrix reference5 = ref effectMatrix[num6];
									reference5 = global::Joints.Joints.playerJoints[global::Players.Players.players[k].jointPackage].InvBindPose[num6] * global::Players.Players.players[k].jt1[num6].mv[rBufferID] * matrix2;
								}
								effect1.Parameters["Matrix"].SetValue(effectMatrix);
								mainC.modelsMain.Render_Rigged_Model_Texture(global::Players.Players.players[k].playerModel[j], global::Players.Players.players[k].textureNormalID, global::Players.Players.players[k].textureSpecularID, (byte)num2, (byte)global::Players.Players.players[k].textureID[num7]);
							}
							num2++;
							num7++;
						}
					}
				}
			}
			effect1.CurrentTechnique = effect1.Techniques["Main"];
			effect1.CurrentTechnique.Passes[0].Apply();
			for (int k = 1; k < global::MainGame.MainGame.maxGamePlayers; k++)
			{
				if ((global::Players.Players.players[k].onmap & 0xC) > 0 && global::Players.Players.players[k].invincible)
				{
					Matrix matrix2 = matrix * Matrix.CreateTranslation(global::Players.Players.players[k].posX[rBufferID], global::Players.Players.players[k].posY[rBufferID], global::Players.Players.players[k].posZ[rBufferID] + global::Players.Players.playerRaces[global::Players.Players.players[k].race].iconHeight[global::Players.Players.players[k].type]);
					mainC.modelsMain.Render_Model(global::Models.Models.modShield, ref matrix2);
				}
			}
		}
		else
		{
			for (int k = 1; k < global::MainGame.MainGame.maxGamePlayers; k++)
			{
				if ((global::Players.Players.players[k].onmap & 0xC) <= 0)
				{
					continue;
				}
				Matrix matrix2 = Matrix.CreateTranslation(global::Players.Players.players[k].posX[rBufferID], global::Players.Players.players[k].posY[rBufferID], global::Players.Players.players[k].posZ[rBufferID]);
				long num5 = global::Joints.Joints.playerJoints[global::Players.Players.players[k].jointPackage].numJoints;
				for (long num6 = 0L; num6 < num5; num6++)
				{
					ref Matrix reference6 = ref effectMatrix[num6];
					reference6 = Matrix.CreateScale(global::Players.Players.players[k].renderScale, global::Players.Players.players[k].renderScale, global::Players.Players.players[k].renderScale) * global::Players.Players.players[k].jt1[num6].mv[rBufferID] * matrix2;
				}
				effect1.Parameters["Matrix"].SetValue(effectMatrix);
				if (global::MainGame.MainGame.commanderSelect != k && !global::Players.Players.players[k].commanderTargeted)
				{
					mainC.modelsMain.Render_Player_Rigged_Model(global::Players.Players.players[k].playerModel[0], global::Players.Players.players[k].textureNormalID, global::Players.Players.players[k].textureSpecularID, byte.MaxValue);
					continue;
				}
				if (global::Players.Players.players[k].team == global::Players.Players.players[0].team)
				{
					if (global::MainGame.MainGame.commanderSelect == k)
					{
						far4[0] = 0.2f;
						far4[1] = 0.2f;
						far4[2] = 1f;
					}
					else
					{
						far4[0] = 0.2f + cosineFluctuation * 0.6f;
						far4[1] = 0.2f + cosineFluctuation * 0.6f;
						far4[2] = 1f;
					}
				}
				else if (global::MainGame.MainGame.commanderSelect == k)
				{
					far4[0] = 1f;
					far4[1] = 0.2f;
					far4[2] = 0.2f;
				}
				else
				{
					far4[0] = 1f;
					far4[1] = 0.2f + cosineFluctuation * 0.6f;
					far4[2] = 0.2f + cosineFluctuation * 0.6f;
				}
				far4[3] = 1f;
				effect1.Parameters["ColorAdjust"].SetValue(far4);
				mainC.modelsMain.Render_Player_Rigged_Model(global::Players.Players.players[k].playerModel[0], global::Players.Players.players[k].textureNormalID, global::Players.Players.players[k].textureSpecularID, byte.MaxValue);
				far4[0] = 1f;
				far4[1] = 1f;
				far4[2] = 1f;
				effect1.Parameters["ColorAdjust"].SetValue(far4);
			}
		}
		effect1.Parameters["World"].SetValue(matrixW);
		if (numStaticVertices > 0)
		{
			rGraphics.SetVertexBuffer(mainVBO);
			rGraphics.Indices = mainIndexBuffer;
			rGraphics.BlendState = BlendState.Opaque;
			rGraphics.SamplerStates[0] = textureSamplerState;
			effect1.CurrentTechnique = effect1.Techniques["Main"];
			for (long num4 = 0L; num4 < numAllocatedVBOList && vboList[(int)checked((nint)num4), 1] > -1 && vboList[(int)checked((nint)num4), 5] == 1; num4++)
			{
				effect1.Parameters["BaseTexture"].SetValue(global::Textures.Textures.texMain.texData[vboList[(int)checked((nint)num4), 0]]);
				effect1.Parameters["MaterialTexture"].SetValue(global::Textures.Textures.texMain.texData[vboList[(int)checked((nint)num4), 6]]);
				effect1.CurrentTechnique.Passes[0].Apply();
				rGraphics.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, vboList[(int)checked((nint)num4), 1], vboList[(int)checked((nint)num4), 3], vboList[(int)checked((nint)num4), 2], vboList[(int)checked((nint)num4), 4]);
			}
		}
		if (numStaticAlphaVertices > 0)
		{
			effect1.Parameters["clipTexture"].SetValue(value: true);
			effect1.CurrentTechnique.Passes[0].Apply();
			rGraphics.SetVertexBuffer(mainAlphaVBO);
			rGraphics.Indices = mainAlphaIndexBuffer;
			rGraphics.RasterizerState = RasterizerState.CullNone;
			for (long num4 = 0L; num4 < global::Textures.Textures.numAlphaTextures && alphaVboList[(int)checked((nint)num4), 1] > -1 && alphaVboList[(int)checked((nint)num4), 5] == 1; num4++)
			{
				effect1.Parameters["BaseTexture"].SetValue(global::Textures.Textures.texMain.texData[alphaVboList[(int)checked((nint)num4), 0]]);
				effect1.Parameters["MaterialTexture"].SetValue(global::Textures.Textures.texMain.texData[alphaVboList[(int)checked((nint)num4), 6]]);
				effect1.CurrentTechnique.Passes[0].Apply();
				rGraphics.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, alphaVboList[(int)checked((nint)num4), 1], alphaVboList[(int)checked((nint)num4), 3], alphaVboList[(int)checked((nint)num4), 2], alphaVboList[(int)checked((nint)num4), 4]);
			}
			rGraphics.RasterizerState = RasterizerState.CullClockwise;
			effect1.Parameters["clipTexture"].SetValue(value: false);
			effect1.CurrentTechnique.Passes[0].Apply();
		}
		mainC.targetMain.Render_Targets();
		mainC.pickupsMain.Render_Pickups();
		mainC.switchesMain.Render_Switches();
		mainC.vehicles.Render_Vehicles();
		mainC.weaponsMain.Render_Weapon_Mounts_Player();
		Render_Solid_Particles();
		for (int k = 1; k < global::MainGame.MainGame.maxGamePlayers; k++)
		{
			if ((global::Players.Players.players[k].onmap & 0x1C) > 0 && global::Players.Players.players[k].renderWeapon == 0)
			{
				mainC.weaponsMain.Render_Player_Weapon((ushort)k);
			}
		}
		mainC.gameobjectMain.Render_Game_Objects();
		mainC.avatarMain.Render_Avatars();
		texAdj[0] = 0f;
		texAdj[1] = 0f;
		effect1.Parameters["texAdj"].SetValue(texAdj);
		mainC.fontmoduleMain.Reset_Graphics_Adapter_After_SpriteBatch();
		mainC.terrainMain.Render_Terrain();
		switch (skyBoxType)
		{
		case 1:
			Render_Sky_Box();
			break;
		case 2:
			Render_Space();
			break;
		case 3:
			Render_Sky_Dome();
			break;
		}
		if (global::Players.Players.currentView > 1 && (global::Players.Players.players[0].onmap & 0xC) > 0)
		{
			_ = global::Players.Players.thirdPersonViewDistanceSqr[rBufferID];
			_ = 0.01f;
		}
		mainC.weaponsMain.Render_Weapons();
		Render_Particles();
		Render_Muzzle_Flashes();
		if (hitColor[3] != 0f)
		{
			Render_Hit_Screen();
		}
		Render_HitIndicators();
		if (renderMinimap)
		{
			Render_MiniMap();
		}
		if (flag)
		{
			byte type = Vehicles.vehicles[global::Players.Players.players[0].curVehicle].type;
			if (type == 0 || type == 8)
			{
				Render_Crosshair_Static();
			}
			else
			{
				Render_Crosshair_Vehicle();
			}
		}
		switch (renderGamerTags)
		{
		case 1:
			Render_GamerTags_HumanPlayers_Player_Team_Only();
			break;
		case 2:
			Render_GamerTags_HumanPlayers_All();
			break;
		case 3:
			Render_GamerTags_All_Players();
			break;
		}
		if (global::Players.Players.currentView == 3 && curWeaponViewTime == weaponViewTime)
		{
			Render_Scope();
		}
		mainC.fontmoduleMain.Reset_Graphics_Adapter_After_SpriteBatch();
		mainC.fontmoduleMain.Render_Onscreen_Text(frametime);
		splashSprite.Begin();
		if (global::Players.Players.currentView != 3 && global::Players.Players.currentView != 7)
		{
			if (showSwapWeaponMessage[rBufferID])
			{
				Render_Swap_Weapon_Message();
			}
			else if (showTauntMessage[rBufferID] && !global::Players.Players.players[0].taunting)
			{
				Render_Taunt_Message();
			}
		}
		if (fixShowSwitch)
		{
			rsPos.X = screenWidthCenter - global::Textures.Textures.texMain.texData[global::Textures.Textures.texHUD_PressX].Width / 2;
			rsPos.Y = screenHeightCenter - global::Textures.Textures.texMain.texData[global::Textures.Textures.texHUD_PressX].Height / 2;
			splashSprite.Draw(global::Textures.Textures.texMain.texData[global::Textures.Textures.texHUD_PressX], rsPos, cWhite);
		}
		if (popUps[0] == 1)
		{
			rsPos.X = screenWidthCenter - 64;
			rsPos.Y = splashRect.Top + mainC.curGame.safeY + 100;
			splashSprite.Draw(global::Textures.Textures.texMain.texData[global::Textures.Textures.texPopUp0], rsPos, cWhite);
		}
		splashSprite.End();
		popUps[0] = 0;
		mainC.gameLogic.Game_Render_HUD();
		mainC.gameLogic.Game_Render_Last();
		mainC.userInterface.Render_Windows();
		mainC.fontmoduleMain.Reset_Graphics_Adapter_After_SpriteBatch();
		global::MainGame.MainGame.debugRenderCrashCount = 0;
		global::Threads.Threads.thread1End.WaitOne();
		if (renderMenuScreen == 0 || global::MainGame.MainGame.gameMode == 1)
		{
			rBufferID = uBufferID;
			uBufferID = (byte)((uBufferID + 1) % 2);
		}
	}

	public void Render_FPS_Message()
	{
		Vector2 position = default(Vector2);
		frameend = global::MainGame.MainGame.mainTime;
		if (frameend != framestart)
		{
			fps = (float)(1.0 / ((frameend - framestart) * 1.0000000116860974E-07));
			framestart = frameend;
			if (fpsCnt++ >= fpsCntTo)
			{
				curFpsTotal = curFps / (float)fpsCnt;
				fpsCntTo = (int)Math.Ceiling(fps / 8f);
				fpsCnt = 1;
				curFps = fps;
			}
			else
			{
				curFps += fps;
			}
		}
		position.X = 550f;
		position.Y = (float)mainC.curGame.safeHeight + 20f;
		global::InputHandler.InputHandler.ttime += global::MainGame.MainGame.frametime;
		if (global::InputHandler.InputHandler.ttime > 0.2f)
		{
			fpsMsg = "FPS " + $"{curFpsTotal:F3}" + " x " + $"{(global::InputHandler.InputHandler.tx):F2}" + " y " + $"{(global::InputHandler.InputHandler.ty):F2}" + " z " + $"{(global::InputHandler.InputHandler.tz):F2}";
			global::InputHandler.InputHandler.ttime = 0f;
		}
		mainC.fontmoduleMain.Draw_String(fpsMsg, ref position, ref cWhite, 1);
	}

	public void Render_Player_Transporter_Effect(int pID, int particleID)
	{
	}

	public void Render_Player_Spawning_No_Effect(int pID, int particleID)
	{
		global::Players.Players.players[pID].timeBeforeRespawn[rBufferID] = -1f;
	}

	public void Update_Camera_Position(float cameraMovementSpeed, float cameraObjMovementSpeed, byte threadID)
	{
		if (moveViewToNewLocation)
		{
			mainC.renderingMain.Move_Camera_To_Goal_Positions_On_Next_Frame();
			moveViewToNewLocation = false;
			return;
		}
		float num = (cameraMovementSpeed + cameraSpeed) * global::MainGame.MainGame.frametime;
		float num2 = camPosGoal[uBufferID].X - camPos[rBufferID].X;
		float num3 = camPosGoal[uBufferID].Y - camPos[rBufferID].Y;
		float num4 = camPosGoal[uBufferID].Z - camPos[rBufferID].Z;
		float num5 = (float)Math.Sqrt(num2 * num2 + num3 * num3 + num4 * num4);
		if (num5 < 10f)
		{
			global::MainGame.MainGame.cameraMovementSpeed = global::MainGame.MainGame.cameraMovementSpeedDefault;
		}
		if (num5 != 0f)
		{
			num2 /= num5;
			num3 /= num5;
			num4 /= num5;
		}
		if (num5 > num)
		{
			num5 = num;
		}
		camPos[uBufferID].X = camPos[rBufferID].X + num2 * num5;
		camPos[uBufferID].Y = camPos[rBufferID].Y + num3 * num5;
		camPos[uBufferID].Z = camPos[rBufferID].Z + num4 * num5;
		num2 = mainC.terrainMain.Get_Terrain_Height(camPos[uBufferID].X, camPos[uBufferID].Y, threadID);
		if (num2 > camPos[uBufferID].Z - 2f)
		{
			camPos[uBufferID].Z = num2 + 2f;
		}
		float num6 = (camerObjectSpeed + cameraObjMovementSpeed) * global::MainGame.MainGame.frametime;
		num = num6;
		float num7 = 1f;
		num2 = camObjectGoal[uBufferID].X - camObject[rBufferID].X;
		num3 = camObjectGoal[uBufferID].Y - camObject[rBufferID].Y;
		num4 = camObjectGoal[uBufferID].Z - camObject[rBufferID].Z;
		num5 = (float)Math.Sqrt(num2 * num2 + num3 * num3 + num4 * num4);
		if (num5 != 0f)
		{
			num2 /= num5;
			num3 /= num5;
			num4 /= num5;
		}
		if (num5 > 1f)
		{
			if (num5 > num)
			{
				num5 = num;
			}
			num7 = num5 / 100f;
			if (num7 > 1f)
			{
				num7 = 1f;
			}
			if (num7 < 0.2f)
			{
				num7 = 0.2f;
			}
		}
		num6 += num5 * num7;
		if (num6 > num5)
		{
			num6 = num5;
		}
		camObject[uBufferID].X = camObject[rBufferID].X + num2 * num6;
		camObject[uBufferID].Y = camObject[rBufferID].Y + num3 * num6;
		camObject[uBufferID].Z = camObject[rBufferID].Z + num4 * num6;
	}

	public void Set_Camera_To_Camera_Goal_Positions()
	{
		camObjectGoal[rBufferID].X = camObjectGoal[uBufferID].X;
		camObjectGoal[rBufferID].Y = camObjectGoal[uBufferID].Y;
		camObjectGoal[rBufferID].Z = camObjectGoal[uBufferID].Z;
		camPosGoal[rBufferID].X = camPosGoal[uBufferID].X;
		camPosGoal[rBufferID].Y = camPosGoal[uBufferID].Y;
		camPosGoal[rBufferID].Z = camPosGoal[uBufferID].Z;
		camPos[rBufferID].X = camPosGoal[uBufferID].X;
		camPos[rBufferID].Y = camPosGoal[uBufferID].Y;
		camPos[rBufferID].Z = camPosGoal[uBufferID].Z;
		camPos[uBufferID].X = camPosGoal[uBufferID].X;
		camPos[uBufferID].Y = camPosGoal[uBufferID].Y;
		camPos[uBufferID].Z = camPosGoal[uBufferID].Z;
		camObject[rBufferID].X = camObjectGoal[uBufferID].X;
		camObject[rBufferID].Y = camObjectGoal[uBufferID].Y;
		camObject[rBufferID].Z = camObjectGoal[uBufferID].Z;
		camObject[uBufferID].X = camObjectGoal[uBufferID].X;
		camObject[uBufferID].Y = camObjectGoal[uBufferID].Y;
		camObject[uBufferID].Z = camObjectGoal[uBufferID].Z;
	}

	public void Move_Camera_To_Goal_Positions_On_Next_Frame()
	{
		camPos[uBufferID].X = camPosGoal[uBufferID].X;
		camPos[uBufferID].Y = camPosGoal[uBufferID].Y;
		camPos[uBufferID].Z = camPosGoal[uBufferID].Z;
		camObject[uBufferID].X = camObjectGoal[uBufferID].X;
		camObject[uBufferID].Y = camObjectGoal[uBufferID].Y;
		camObject[uBufferID].Z = camObjectGoal[uBufferID].Z;
	}

	public void Init_Rendering(byte threadID)
	{
		muzzleFlashes = new StructsClass.Muzzle_Flash[2, 44];
		cameraJointCollection = new StructsClass.JointCollection();
		mainC.jointsMain.Load_Joints("The_CoOp_Zombie_Game\\Config_Files\\Joints_Camera.txt", ref cameraJointCollection, -1);
		cameraJoint = new StructsClass.joint[cameraJointCollection.numJoints];
		for (ushort num = 0; num < cameraJointCollection.numJoints; num++)
		{
			cameraJoint[num] = new StructsClass.joint();
			cameraJoint[num].parentCount = cameraJointCollection.jt1[num].parentCount;
			cameraJoint[num].parentID = cameraJointCollection.jt1[num].parentID;
			if (cameraJoint[num].parentCount > 0)
			{
				cameraJoint[num].parentList = new short[cameraJoint[num].parentCount];
				for (ushort num2 = 0; num2 < cameraJoint[num].parentCount; num2++)
				{
					cameraJoint[num].parentList[num2] = cameraJointCollection.jt1[num].parentList[num2];
				}
			}
			cameraJoint[num].x = cameraJointCollection.jt1[num].x;
			cameraJoint[num].y = cameraJointCollection.jt1[num].y;
			cameraJoint[num].z = cameraJointCollection.jt1[num].z;
			cameraJoint[num].angleX = cameraJointCollection.jt1[num].angleX;
			cameraJoint[num].angleY = cameraJointCollection.jt1[num].angleY;
			cameraJoint[num].angleZ = cameraJointCollection.jt1[num].angleZ;
		}
		cameraProgram = new StructsClass.program_instance[1];
		cameraProgram[0].curStep = 0;
		cameraProgram[0].status = 0;
		matrixVInverse = Matrix.Identity;
		pos = new Vector4[20];
		scale = new Vector4[20];
		pcolor = new Vector4[20];
		numInstanceCounts = 20;
		tauntPos.X = (float)screenWidthCenter * 1.5f - (float)global::Textures.Textures.texMain.texData[global::Textures.Textures.texTauntMessage].Width / 2f;
		tauntPos.Y = (float)screenHeightCenter - (float)global::Textures.Textures.texMain.texData[global::Textures.Textures.texTauntMessage].Height / 2f;
		swapWeaponPos.X = (float)screenWidthCenter - (float)global::Textures.Textures.texMain.texData[global::Textures.Textures.texSwapWeapon].Width / 2f;
		swapWeaponPos.Y = (float)screenHeightCenter - (float)global::Textures.Textures.texMain.texData[global::Textures.Textures.texSwapWeapon].Height / 2f;
		worldPos = new Vector3[2];
		ref Vector3 reference = ref worldPos[0];
		reference = new Vector3(1f, 1f, 1f);
		ref Vector3 reference2 = ref worldPos[1];
		reference2 = new Vector3(1f, 1f, 1f);
		weaponViewTime = 0.265f;
		gamerTagFont = 0;
		numAllocatedCameraPostions = 1;
		numCameraPositions = 1;
		float[] array = new float[1];
		cameraPositionsX = array;
		float[] array2 = new float[1];
		cameraPositionsY = array2;
		float[] array3 = new float[1];
		cameraPositionsZ = array3;
		numAvatarCameraPositions = 1;
		numAllocatedAvatarCameraPostions = 1;
		float[] array4 = new float[1];
		avatarCameraPositionsX = array4;
		float[] array5 = new float[1];
		avatarCameraPositionsY = array5;
		float[] array6 = new float[1];
		avatarCameraPositionsZ = array6;
		camPos[0].X = 0f;
		camPos[0].Y = 0f;
		camPos[0].Z = 0f;
		camPos[1].X = 0f;
		camPos[1].Y = 0f;
		camPos[1].Z = 0f;
		camObject[0].X = 0f;
		camObject[0].Y = 1f;
		camObject[0].Z = 0f;
		camObject[1].X = 0f;
		camObject[1].Y = 1f;
		camObject[1].Z = 0f;
		hitColor[0] = 1f;
		hitColor[1] = 0f;
		hitColor[2] = 0f;
		hitColor[3] = 0f;
		renderTargetPlayer = new RenderTarget2D(mainC.curGame.GraphicsDevice, mainC.curGame.presentationParameters.BackBufferWidth, mainC.curGame.presentationParameters.BackBufferHeight, mipMap: false, SurfaceFormat.Color, DepthFormat.Depth24Stencil8);
		renderTargetWorld = new RenderTarget2D(mainC.curGame.GraphicsDevice, mainC.curGame.presentationParameters.BackBufferWidth, mainC.curGame.presentationParameters.BackBufferHeight, mipMap: false, SurfaceFormat.Color, DepthFormat.Depth24Stencil8);
		brightness = 0.5f;
		Set_Brightness();
		effect1.Parameters["AlphaAdjust"].SetValue(1f);
		effect1.Parameters["PtLightDistance0"].SetValue(8192f);
		far4[0] = 1f;
		far4[1] = 1f;
		far4[2] = 1f;
		far4[3] = 1f;
		effect1.Parameters["ColorAdjust"].SetValue(far4);
		far4[0] = 1f;
		far4[1] = 0f;
		far4[2] = 0f;
		far4[3] = 1f;
		effect1.Parameters["LaserLightColor0"].SetValue(far4);
		textureSamplerStatePoint.AddressU = TextureAddressMode.Wrap;
		textureSamplerStatePoint.AddressV = TextureAddressMode.Wrap;
		textureSamplerStatePoint.AddressW = TextureAddressMode.Wrap;
		textureSamplerStatePoint.Filter = TextureFilter.Point;
		textureSamplerStatePoint.MaxAnisotropy = 8;
		textureSamplerStateClamp.AddressU = TextureAddressMode.Clamp;
		textureSamplerStateClamp.AddressV = TextureAddressMode.Clamp;
		depthBufferDisabled.DepthBufferEnable = false;
		depthBufferDisabled.DepthBufferWriteEnable = false;
		depthBufferDisabled.DepthBufferFunction = CompareFunction.LessEqual;
		depthBufferWriteDisabled.DepthBufferEnable = true;
		depthBufferWriteDisabled.DepthBufferWriteEnable = false;
		depthBufferWriteDisabled.DepthBufferFunction = CompareFunction.LessEqual;
		rGraphics.DepthStencilState = depthBufferEnabled;
		rGraphics.RasterizerState = rasterizerState;
		numAllocatedVBOList = global::Textures.Textures.numTextures;
		vboList = new int[numAllocatedVBOList, 7];
		numAllocatedAlphaVBOList = global::Textures.Textures.numAlphaTextures;
		alphaVboList = new int[numAllocatedAlphaVBOList, 7];
		scoreBoardVtex[0] = default(StructsClass.VertexPositionColorNormalTexture);
		scoreBoardVtex[1] = default(StructsClass.VertexPositionColorNormalTexture);
		scoreBoardVtex[2] = default(StructsClass.VertexPositionColorNormalTexture);
		scoreBoardVtex[3] = default(StructsClass.VertexPositionColorNormalTexture);
		scoreBoardVtex[0].Set_Values(-320f, -240f, -1f, 0f, 0f, 1f, 1f, 0f, 0f, 0f, 0f, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
		scoreBoardVtex[1].Set_Values(320f, -240f, -1f, 0f, 0f, 1f, 1f, 0f, 0f, 1f, 0f, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
		scoreBoardVtex[2].Set_Values(320f, 300f, -1f, 0f, 0f, 1f, 1f, 0f, 0f, 1f, 1f, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
		scoreBoardVtex[3].Set_Values(-320f, 300f, -1f, 0f, 0f, 1f, 1f, 0f, 0f, 0f, 1f, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
		ScopeVtex[0] = default(StructsClass.VertexPositionColorNormalTexture);
		ScopeVtex[1] = default(StructsClass.VertexPositionColorNormalTexture);
		ScopeVtex[2] = default(StructsClass.VertexPositionColorNormalTexture);
		ScopeVtex[3] = default(StructsClass.VertexPositionColorNormalTexture);
		ScopeVtex[0].Set_Values(-640f, -512f, -0.2f, 0f, 0f, 1f, 1f, 0f, 0f, 0.875f, 0f, 0, 0, 0, byte.MaxValue);
		ScopeVtex[1].Set_Values(640f, -512f, -0.2f, 0f, 0f, 1f, 1f, 0f, 0f, 2.125f, 0f, 0, 0, 0, byte.MaxValue);
		ScopeVtex[2].Set_Values(640f, 512f, -0.2f, 0f, 0f, 1f, 1f, 0f, 0f, 2.125f, 1f, 0, 0, 0, byte.MaxValue);
		ScopeVtex[3].Set_Values(-640f, 512f, -0.2f, 0f, 0f, 1f, 1f, 0f, 0f, 0.875f, 1f, 0, 0, 0, byte.MaxValue);
		planeVtex[0] = default(StructsClass.VertexPositionColorNormalTexture);
		planeVtex[1] = default(StructsClass.VertexPositionColorNormalTexture);
		planeVtex[2] = default(StructsClass.VertexPositionColorNormalTexture);
		planeVtex[3] = default(StructsClass.VertexPositionColorNormalTexture);
		planeVtex[0].Set_Values(-64f, -64f, -1f, 0f, 0f, 1f, 1f, 0f, 0f, 0f, 1f, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
		planeVtex[1].Set_Values(64f, -64f, -1f, 0f, 0f, 1f, 1f, 0f, 0f, 1f, 1f, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
		planeVtex[2].Set_Values(64f, 64f, -1f, 0f, 0f, 1f, 1f, 0f, 0f, 1f, 0f, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
		planeVtex[3].Set_Values(-64f, 64f, -1f, 0f, 0f, 1f, 1f, 0f, 0f, 0f, 0f, byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);
		Init_Particles();
		dialogUpdate[0, 3] = "FPS: ";
		dialogUpdate[1, 3] = "FPS: ";
		Init_MiniMap();
		Initialize_Particles();
	}

	public void Level_Reset_Texture_List()
	{
		short numTextures = global::Textures.Textures.numTextures;
		if (numTextures > numAllocatedVBOList)
		{
			numAllocatedVBOList = numTextures;
			vboList = new int[numTextures, 7];
		}
		numTextures = global::Textures.Textures.numAlphaTextures;
		if (numTextures > numAllocatedAlphaVBOList)
		{
			numAllocatedAlphaVBOList = numTextures;
			alphaVboList = new int[numTextures, 7];
		}
	}

	public void Init_Particles()
	{
		for (int i = 0; i < 1500; i++)
		{
			particles[0, i].lifeTime = 0f;
			particles[1, i].lifeTime = 0f;
			particles[0, i].type = -1;
			particles[1, i].type = -1;
			pSort[i].next = -1;
			pSort[i].prev = -1;
			pSort[i].index = -1;
		}
		pCount = 0;
		pStart = 0;
	}

	public void Load_Instancing_Data(string fileName, byte threadID)
	{
		int num = -1;
		short num2 = 0;
		short num3 = 1;
		Stream stream = TitleContainer.OpenStream("The_CoOp_Zombie_Game\\Config_Files\\" + fileName);
		byte[] array = new byte[stream.Length];
		for (int i = 0; i < numRenderingInstances; i++)
		{
			int numItems = renderingInstances[i].numItems;
			for (int j = 0; j < numItems; j++)
			{
				renderingInstances[i].bbType[j] = 132;
			}
			numItems = renderingInstances[i].numCollisionModels;
			for (int j = 0; j < numItems; j++)
			{
				renderingInstances[i].zoneID[j] = 0;
			}
			renderingInstances[i].numItems = 0;
			renderingInstances[i].numModels = 0;
			renderingInstances[i].useVbo = false;
			renderingInstances[i].usesCollisionModel = false;
			numItems = renderingInstances[i].numObjects;
			for (int j = 0; j < numItems; j++)
			{
				if (renderingInstances[i].objList[j] > -1)
				{
					mainC.gameobjectMain.Delete_Object(renderingInstances[i].objList[j], threadID);
				}
			}
			renderingInstances[i].numObjects = 0;
		}
		numRenderingInstances = 0;
		if (stream.CanRead)
		{
			stream.Read(array, 0, array.Length);
			string text = mainC.utilMain.Byte_Array_To_String(array);
			string[] array2 = text.Split('\n', '\r');
			int k = 0;
			int num4 = 0;
			for (; k < array2.Length; k++)
			{
				if (array2[k].Length > 0)
				{
					num4++;
				}
			}
			if (num4 < 1)
			{
				stream.Close();
				return;
			}
			string[] array3 = new string[num4];
			k = 0;
			num4 = 0;
			for (; k < array2.Length; k++)
			{
				if (array2[k].Length > 0)
				{
					array3[num4++] = array2[k];
				}
			}
			for (k = 0; k < num4; k++)
			{
				array2 = array3[k].Split(' ', '\t');
				int l = 0;
				int num5 = 0;
				for (; l < array2.Length; l++)
				{
					if (array2[l].Length > 0)
					{
						num5++;
					}
				}
				if (num5 < 1)
				{
					continue;
				}
				string[] array4 = new string[num5];
				l = 0;
				num5 = 0;
				for (; l < array2.Length; l++)
				{
					if (array2[l].Length > 0)
					{
						array4[num5++] = array2[l];
					}
				}
				int num6 = 0;
				if (array4[0].Equals("numInstances", StringComparison.OrdinalIgnoreCase))
				{
					num6 = 1;
				}
				else if (array4[0].Equals("Instance", StringComparison.OrdinalIgnoreCase))
				{
					num6 = 2;
				}
				else if (array4[0].Equals("type", StringComparison.OrdinalIgnoreCase))
				{
					num6 = 3;
				}
				else if (array4[0].Equals("NumModels", StringComparison.OrdinalIgnoreCase))
				{
					num6 = 4;
				}
				else if (array4[0].Equals("Models", StringComparison.OrdinalIgnoreCase))
				{
					num6 = 5;
				}
				else if (array4[0].Equals("Color", StringComparison.OrdinalIgnoreCase))
				{
					num6 = 6;
				}
				else if (array4[0].Equals("RotX", StringComparison.OrdinalIgnoreCase))
				{
					num6 = 7;
				}
				else if (array4[0].Equals("RotY", StringComparison.OrdinalIgnoreCase))
				{
					num6 = 8;
				}
				else if (array4[0].Equals("RotZ", StringComparison.OrdinalIgnoreCase))
				{
					num6 = 9;
				}
				else if (array4[0].Equals("Count", StringComparison.OrdinalIgnoreCase))
				{
					num6 = 10;
				}
				else if (array4[0].Equals("PositionX", StringComparison.OrdinalIgnoreCase))
				{
					num6 = 11;
				}
				else if (array4[0].Equals("PositionY", StringComparison.OrdinalIgnoreCase))
				{
					num6 = 12;
				}
				else if (array4[0].Equals("PositionZ", StringComparison.OrdinalIgnoreCase))
				{
					num6 = 13;
				}
				else if (array4[0].Equals("ScaleX", StringComparison.OrdinalIgnoreCase))
				{
					num6 = 14;
				}
				else if (array4[0].Equals("ScaleY", StringComparison.OrdinalIgnoreCase))
				{
					num6 = 15;
				}
				else if (array4[0].Equals("ScaleZ", StringComparison.OrdinalIgnoreCase))
				{
					num6 = 16;
				}
				else if (array4[0].Equals("ScaleTx", StringComparison.OrdinalIgnoreCase))
				{
					num6 = 17;
				}
				else if (array4[0].Equals("ScaleTy", StringComparison.OrdinalIgnoreCase))
				{
					num6 = 18;
				}
				else if (array4[0].Equals("active", StringComparison.OrdinalIgnoreCase))
				{
					num6 = 19;
				}
				else if (array4[0].Equals("BoundingBox", StringComparison.OrdinalIgnoreCase))
				{
					num6 = 20;
				}
				else if (array4[0].Equals("BoundingBoxType", StringComparison.OrdinalIgnoreCase))
				{
					num6 = 21;
				}
				else if (array4[0].Equals("VBO", StringComparison.OrdinalIgnoreCase))
				{
					num6 = 22;
				}
				else if (array4[0].Equals("CollisionModel", StringComparison.OrdinalIgnoreCase))
				{
					num6 = 23;
				}
				else if (array4[0].Equals("ZoneID", StringComparison.OrdinalIgnoreCase))
				{
					num6 = 24;
				}
				switch (num6)
				{
				case 1:
					if (array4.Length <= 1)
					{
						break;
					}
					num3 = short.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
					if (num3 > numAllocatedInstances)
					{
						renderingInstances = new StructsClass.RenderInstance[num3];
						for (int i = 0; i < num3; i++)
						{
							renderingInstances[i].numObjects = 0;
							renderingInstances[i].numModels = 0;
							renderingInstances[i].numItems = 0;
							renderingInstances[i].numAllocatedObjects = 0;
							renderingInstances[i].useVbo = false;
							renderingInstances[i].usesCollisionModel = false;
						}
						numAllocatedInstances = num3;
					}
					break;
				case 2:
					if (array4.Length > 1)
					{
						num = int.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
						if (num < 0 || num >= numAllocatedInstances)
						{
							num = 0;
						}
						if (numRenderingInstances <= num)
						{
							numRenderingInstances = (short)(num + 1);
						}
					}
					break;
				case 3:
					if (array4.Length > 1 && num > -1)
					{
						renderingInstances[num].type = byte.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 4:
					if (array4.Length > 1 && num > -1)
					{
						int j = int.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
						if (j > 0)
						{
							renderingInstances[num].modelList = new short[j];
						}
						for (int i = 0; i < j; i++)
						{
							renderingInstances[num].modelList[i] = -1;
						}
						renderingInstances[num].numModels = (byte)j;
					}
					break;
				case 5:
					if (array4.Length > 1)
					{
						int j = renderingInstances[num].numModels;
						if (array4.Length - 1 < j)
						{
							j = array4.Length - 1;
						}
						for (int i = 0; i < j; i++)
						{
							renderingInstances[num].modelList[i] = mainC.modelsMain.Find_Level_Model(array4[i + 1]);
						}
					}
					break;
				case 6:
					if (array4.Length > 4 && num > -1)
					{
						renderingInstances[num].color.X = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
						renderingInstances[num].color.Y = float.Parse(array4[2], CultureInfo.InvariantCulture.NumberFormat);
						renderingInstances[num].color.Z = float.Parse(array4[3], CultureInfo.InvariantCulture.NumberFormat);
						renderingInstances[num].color.W = float.Parse(array4[4], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 7:
					if (array4.Length > 1 && num > -1)
					{
						renderingInstances[num].rotX = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 8:
					if (array4.Length > 1 && num > -1)
					{
						renderingInstances[num].rotY = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 9:
					if (array4.Length > 1 && num > -1)
					{
						renderingInstances[num].rotZ = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 10:
					if (array4.Length > 1 && num > -1)
					{
						num2 = short.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
						renderingInstances[num].x = new float[num2];
						renderingInstances[num].y = new float[num2];
						renderingInstances[num].z = new float[num2];
						renderingInstances[num].sx = new float[num2];
						renderingInstances[num].sy = new float[num2];
						renderingInstances[num].sz = new float[num2];
						renderingInstances[num].tx = new float[num2];
						renderingInstances[num].ty = new float[num2];
						renderingInstances[num].active = new byte[num2];
						renderingInstances[num].needsBB = new byte[num2];
						renderingInstances[num].bbType = new byte[num2];
						for (int i = 0; i < num2; i++)
						{
							renderingInstances[num].bbType[i] = 132;
						}
						renderingInstances[num].numItems = num2;
						if (num2 > numInstanceCounts)
						{
							pos = new Vector4[num2];
							scale = new Vector4[num2];
							numInstanceCounts = num2;
						}
					}
					break;
				case 11:
					if (array4.Length > num2 && num > -1 && num2 > 0)
					{
						for (int i = 0; i < num2 && i < array4.Length - 1; i++)
						{
							renderingInstances[num].x[i] = float.Parse(array4[i + 1], CultureInfo.InvariantCulture.NumberFormat);
						}
					}
					break;
				case 12:
					if (array4.Length > num2 && num > -1 && num2 > 0)
					{
						for (int i = 0; i < num2 && i < array4.Length - 1; i++)
						{
							renderingInstances[num].y[i] = float.Parse(array4[i + 1], CultureInfo.InvariantCulture.NumberFormat);
						}
					}
					break;
				case 13:
					if (array4.Length > num2 && num > -1 && num2 > 0)
					{
						for (int i = 0; i < num2 && i < array4.Length - 1; i++)
						{
							renderingInstances[num].z[i] = float.Parse(array4[i + 1], CultureInfo.InvariantCulture.NumberFormat);
						}
					}
					break;
				case 14:
					if (array4.Length > num2 && num > -1 && num2 > 0)
					{
						for (int i = 0; i < num2 && i < array4.Length - 1; i++)
						{
							renderingInstances[num].sx[i] = float.Parse(array4[i + 1], CultureInfo.InvariantCulture.NumberFormat);
						}
					}
					break;
				case 15:
					if (array4.Length > num2 && num > -1 && num2 > 0)
					{
						for (int i = 0; i < num2 && i < array4.Length - 1; i++)
						{
							renderingInstances[num].sy[i] = float.Parse(array4[i + 1], CultureInfo.InvariantCulture.NumberFormat);
						}
					}
					break;
				case 16:
					if (array4.Length > num2 && num > -1 && num2 > 0)
					{
						for (int i = 0; i < num2 && i < array4.Length - 1; i++)
						{
							renderingInstances[num].sz[i] = float.Parse(array4[i + 1], CultureInfo.InvariantCulture.NumberFormat);
						}
					}
					break;
				case 17:
					if (array4.Length > num2 && num > -1 && num2 > 0)
					{
						for (int i = 0; i < num2 && i < array4.Length - 1; i++)
						{
							renderingInstances[num].tx[i] = float.Parse(array4[i + 1], CultureInfo.InvariantCulture.NumberFormat);
						}
					}
					break;
				case 18:
					if (array4.Length > num2 && num > -1 && num2 > 0)
					{
						for (int i = 0; i < num2 && i < array4.Length - 1; i++)
						{
							renderingInstances[num].ty[i] = float.Parse(array4[i + 1], CultureInfo.InvariantCulture.NumberFormat);
						}
					}
					break;
				case 19:
					if (array4.Length > num2 && num > -1 && num2 > 0)
					{
						for (int i = 0; i < num2 && i < array4.Length - 1; i++)
						{
							renderingInstances[num].active[i] = byte.Parse(array4[i + 1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
						}
					}
					break;
				case 20:
					if (array4.Length > num2 && num > -1 && num2 > 0)
					{
						for (int i = 0; i < num2 && i < array4.Length - 1; i++)
						{
							renderingInstances[num].needsBB[i] = byte.Parse(array4[i + 1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
						}
					}
					break;
				case 21:
					if (array4.Length > num2 && num > -1 && num2 > 0)
					{
						for (int i = 0; i < num2 && i < array4.Length - 1; i++)
						{
							renderingInstances[num].bbType[i] = byte.Parse(array4[i + 1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
						}
					}
					break;
				case 22:
					renderingInstances[num].useVbo = true;
					break;
				case 23:
					if (array4.Length > 1 && num > -1)
					{
						int j = array4.Length - 1;
						renderingInstances[num].usesCollisionModel = true;
						renderingInstances[num].numCollisionModels = (byte)j;
						renderingInstances[num].collisionModel = new string[j];
						renderingInstances[num].zoneID = new ushort[j];
						int numItems = 1;
						for (int i = 0; i < j; i++)
						{
							renderingInstances[num].collisionModel[i] = array4[numItems++];
							renderingInstances[num].zoneID[i] = 0;
						}
					}
					break;
				case 24:
				{
					int j = renderingInstances[num].numCollisionModels;
					if (array4.Length > j && num > -1)
					{
						int numItems = 1;
						int num7 = array4.Length - 1;
						for (int i = 0; i < j && i < num7; i++)
						{
							renderingInstances[num].zoneID[i] = ushort.Parse(array4[numItems++], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
						}
					}
					break;
				}
				}
			}
		}
		stream.Close();
		for (int i = 0; i < numRenderingInstances; i++)
		{
			switch (renderingInstances[i].type)
			{
			case 0:
			case 10:
			{
				renderingInstances[i].mv = Matrix.CreateRotationZ(renderingInstances[i].rotZ * ((float)Math.PI / 180f)) * Matrix.CreateRotationY(renderingInstances[i].rotY * ((float)Math.PI / 180f)) * Matrix.CreateRotationX(renderingInstances[i].rotX * ((float)Math.PI / 180f));
				if (!renderingInstances[i].usesCollisionModel)
				{
					break;
				}
				int numItems = renderingInstances[i].numItems;
				for (int j = 0; j < numItems; j++)
				{
					ushort gid = mainC.maingameMain.Register_Game_Item(3, (ushort)i, (ushort)i);
					Matrix mv = Matrix.CreateScale(renderingInstances[i].sx[j], renderingInstances[i].sy[j], renderingInstances[i].sz[j]) * renderingInstances[i].mv * Matrix.CreateTranslation(renderingInstances[i].x[j], renderingInstances[i].y[j], renderingInstances[i].z[j]);
					for (int k = 0; k < renderingInstances[i].numCollisionModels; k++)
					{
						mainC.zonesMain.Add_CollisionModel_To_Zone(renderingInstances[i].zoneID[k], mainC.collisionMain.Find_Collision_Model(renderingInstances[i].collisionModel[k], 0), gid, ref mv);
					}
				}
				break;
			}
			case 2:
			{
				int numItems = renderingInstances[i].numItems;
				for (int j = 0; j < numItems; j++)
				{
					renderingInstances[i].bbType[j] = 0;
				}
				renderingInstances[i].mv = Matrix.CreateRotationZ(renderingInstances[i].rotZ * ((float)Math.PI / 180f)) * Matrix.CreateRotationY(renderingInstances[i].rotY * ((float)Math.PI / 180f)) * Matrix.CreateRotationX(renderingInstances[i].rotX * ((float)Math.PI / 180f));
				if (!renderingInstances[i].usesCollisionModel)
				{
					break;
				}
				for (int j = 0; j < numItems; j++)
				{
					ushort gid = mainC.maingameMain.Register_Game_Item(3, (ushort)i, (ushort)i);
					Matrix mv = Matrix.CreateScale(renderingInstances[i].sx[j], renderingInstances[i].sy[j], renderingInstances[i].sz[j]) * renderingInstances[i].mv * Matrix.CreateTranslation(renderingInstances[i].x[j], renderingInstances[i].y[j], renderingInstances[i].z[j]);
					for (int k = 0; k < renderingInstances[i].numCollisionModels; k++)
					{
						mainC.zonesMain.Add_CollisionModel_To_Zone(renderingInstances[i].zoneID[k], mainC.collisionMain.Find_Collision_Model(renderingInstances[i].collisionModel[k], 0), gid, ref mv);
					}
				}
				break;
			}
			}
		}
	}

	public void Initialize_Instancing_Objects(byte threadID)
	{
		int num = 0;
		short newID = 0;
		Matrix identity = Matrix.Identity;
		Matrix identity2 = Matrix.Identity;
		for (int i = 0; i < numRenderingInstances; i++)
		{
			switch (renderingInstances[i].type)
			{
			case 0:
			case 1:
			{
				int j = 0;
				int num2 = 0;
				for (; j < renderingInstances[i].numModels; j++)
				{
					num2 += global::Models.Models.modVbo[renderingInstances[i].modelList[j]].numObjects;
				}
				if (num2 <= 0)
				{
					break;
				}
				j = 0;
				int num3 = 0;
				for (; j < renderingInstances[i].numItems; j++)
				{
					if (renderingInstances[i].needsBB[j] == 1)
					{
						num3++;
					}
				}
				num2 *= num3;
				if (num2 > renderingInstances[i].numAllocatedObjects)
				{
					renderingInstances[i].objList = new short[num2];
					renderingInstances[i].numAllocatedObjects = (short)num2;
				}
				renderingInstances[i].numObjects = (short)num2;
				num2 = (int)(renderingInstances[i].rotX / 90f);
				num3 = (int)(renderingInstances[i].rotY / 90f);
				num = (int)(renderingInstances[i].rotZ / 90f);
				bool flag = false;
				if ((float)num2 * 90f == renderingInstances[i].rotX && (float)num3 * 90f == renderingInstances[i].rotY && (float)num * 90f == renderingInstances[i].rotZ)
				{
					flag = true;
					identity = renderingInstances[i].mv;
				}
				else
				{
					flag = false;
					identity = Matrix.Identity;
				}
				num3 = 0;
				for (j = 0; j < renderingInstances[i].numItems; j++)
				{
					if (renderingInstances[i].needsBB[j] != 1)
					{
						continue;
					}
					for (int k = 0; k < renderingInstances[i].numModels; k++)
					{
						for (num = 0; num < global::Models.Models.modVbo[renderingInstances[i].modelList[k]].numObjects; num++)
						{
							if (flag)
							{
								if (global::Models.Models.modVbo[renderingInstances[i].modelList[k]].numObjectRotations > 0)
								{
									float num4 = global::Models.Models.modVbo[renderingInstances[i].modelList[k]].x[num] * renderingInstances[i].sx[j];
									float num5 = global::Models.Models.modVbo[renderingInstances[i].modelList[k]].y[num] * renderingInstances[i].sy[j];
									float num6 = global::Models.Models.modVbo[renderingInstances[i].modelList[k]].z[num] * renderingInstances[i].sz[j];
									float x = num4 * identity.M11 + num5 * identity.M21 + num6 * identity.M31 + renderingInstances[i].x[j];
									float y = num4 * identity.M12 + num5 * identity.M22 + num6 * identity.M32 + renderingInstances[i].y[j];
									float z = num4 * identity.M13 + num5 * identity.M23 + num6 * identity.M33 + renderingInstances[i].z[j];
									identity2 = Matrix.CreateRotationX(global::Models.Models.modVbo[renderingInstances[i].modelList[k]].rotX[num] * ((float)Math.PI / 180f)) * identity;
									identity2 = Matrix.CreateRotationY(global::Models.Models.modVbo[renderingInstances[i].modelList[k]].rotY[num] * ((float)Math.PI / 180f)) * identity2;
									identity2 = Matrix.CreateRotationZ(global::Models.Models.modVbo[renderingInstances[i].modelList[k]].rotZ[num] * ((float)Math.PI / 180f)) * identity2;
									mainC.gameobjectMain.Create_Collision_Object(ref newID, x, y, z, ref identity2, (uint)Math.Ceiling((float)global::Models.Models.modVbo[renderingInstances[i].modelList[k]].dimX[num] * renderingInstances[i].sx[j]), (uint)Math.Ceiling((float)global::Models.Models.modVbo[renderingInstances[i].modelList[k]].dimY[num] * renderingInstances[i].sy[j]), (uint)Math.Ceiling((float)global::Models.Models.modVbo[renderingInstances[i].modelList[k]].dimZ[num] * renderingInstances[i].sz[j]), 0f, 0f, 0f, -1, renderingInstances[i].bbType[j], threadID);
								}
								else
								{
									float num4 = global::Models.Models.modVbo[renderingInstances[i].modelList[k]].x[num] * renderingInstances[i].sx[j];
									float num5 = global::Models.Models.modVbo[renderingInstances[i].modelList[k]].y[num] * renderingInstances[i].sy[j];
									float num6 = global::Models.Models.modVbo[renderingInstances[i].modelList[k]].z[num] * renderingInstances[i].sz[j];
									float num7 = (float)global::Models.Models.modVbo[renderingInstances[i].modelList[k]].dimX[num] * renderingInstances[i].sx[j];
									float num8 = (float)global::Models.Models.modVbo[renderingInstances[i].modelList[k]].dimY[num] * renderingInstances[i].sy[j];
									float num9 = (float)global::Models.Models.modVbo[renderingInstances[i].modelList[k]].dimZ[num] * renderingInstances[i].sz[j];
									float x = num4 * identity.M11 + num5 * identity.M21 + num6 * identity.M31 + renderingInstances[i].x[j];
									float y = num4 * identity.M12 + num5 * identity.M22 + num6 * identity.M32 + renderingInstances[i].y[j];
									float z = num4 * identity.M13 + num5 * identity.M23 + num6 * identity.M33 + renderingInstances[i].z[j];
									float num10 = num7 * identity.M11 + num8 * identity.M21 + num9 * identity.M31;
									float num11 = num7 * identity.M12 + num8 * identity.M22 + num9 * identity.M32;
									float num12 = num7 * identity.M13 + num8 * identity.M23 + num9 * identity.M33;
									if (num10 < 0f)
									{
										x += num10;
										num10 = 0f - num10;
									}
									if (num11 < 0f)
									{
										y += num11;
										num11 = 0f - num11;
									}
									if (num12 < 0f)
									{
										z += num12;
										num12 = 0f - num12;
									}
									mainC.gameobjectMain.Create_Collision_Object(ref newID, x, y, z, ref matrixI, (uint)Math.Ceiling(num10), (uint)Math.Ceiling(num11), (uint)Math.Ceiling(num12), 0f, 0f, 0f, -1, renderingInstances[i].bbType[j], threadID);
								}
							}
							else if (global::Models.Models.modVbo[renderingInstances[i].modelList[k]].numObjectRotations > 0)
							{
								identity2 = Matrix.CreateRotationX(global::Models.Models.modVbo[renderingInstances[i].modelList[k]].rotX[num] * ((float)Math.PI / 180f)) * identity;
								identity2 = Matrix.CreateRotationY(global::Models.Models.modVbo[renderingInstances[i].modelList[k]].rotY[num] * ((float)Math.PI / 180f)) * identity2;
								identity2 = Matrix.CreateRotationZ(global::Models.Models.modVbo[renderingInstances[i].modelList[k]].rotZ[num] * ((float)Math.PI / 180f)) * identity2;
								mainC.gameobjectMain.Create_Collision_Object(ref newID, renderingInstances[i].x[j] + global::Models.Models.modVbo[renderingInstances[i].modelList[k]].x[num] * renderingInstances[i].sx[j], renderingInstances[i].y[j] + global::Models.Models.modVbo[renderingInstances[i].modelList[k]].y[num] * renderingInstances[i].sy[j], renderingInstances[i].z[j] + global::Models.Models.modVbo[renderingInstances[i].modelList[k]].z[num] * renderingInstances[i].sz[j], ref identity2, (uint)Math.Ceiling((float)global::Models.Models.modVbo[renderingInstances[i].modelList[k]].dimX[num] * renderingInstances[i].sx[j]), (uint)Math.Ceiling((float)global::Models.Models.modVbo[renderingInstances[i].modelList[k]].dimY[num] * renderingInstances[i].sy[j]), (uint)Math.Ceiling((float)global::Models.Models.modVbo[renderingInstances[i].modelList[k]].dimZ[num] * renderingInstances[i].sz[j]), 0f, 0f, 0f, -1, renderingInstances[i].bbType[j], threadID);
							}
							else
							{
								mainC.gameobjectMain.Create_Collision_Object(ref newID, renderingInstances[i].x[j] + global::Models.Models.modVbo[renderingInstances[i].modelList[k]].x[num] * renderingInstances[i].sx[j], renderingInstances[i].y[j] + global::Models.Models.modVbo[renderingInstances[i].modelList[k]].y[num] * renderingInstances[i].sy[j], renderingInstances[i].z[j] + global::Models.Models.modVbo[renderingInstances[i].modelList[k]].z[num] * renderingInstances[i].sz[j], ref identity, (uint)Math.Ceiling((float)global::Models.Models.modVbo[renderingInstances[i].modelList[k]].dimX[num] * renderingInstances[i].sx[j]), (uint)Math.Ceiling((float)global::Models.Models.modVbo[renderingInstances[i].modelList[k]].dimY[num] * renderingInstances[i].sy[j]), (uint)Math.Ceiling((float)global::Models.Models.modVbo[renderingInstances[i].modelList[k]].dimZ[num] * renderingInstances[i].sz[j]), 0f, 0f, 0f, -1, renderingInstances[i].bbType[j], threadID);
							}
							renderingInstances[i].objList[num3++] = newID;
						}
					}
				}
				break;
			}
			}
		}
	}

	public void Load_Rendering_Data(string fileName)
	{
		int num = -1;
		if (global::Textures.Textures.numTargetingCrosshairs < 1)
		{
			ushort[] texCrossHair = new ushort[3];
			global::Textures.Textures.texCrossHair = texCrossHair;
		}
		shadowMapHeight = global::MainGame.MainGame.MaxUp;
		if (numCameraPositions < 1)
		{
			if (numAllocatedCameraPostions < 1)
			{
				cameraPositionsX = new float[1];
				cameraPositionsY = new float[1];
				cameraPositionsZ = new float[1];
				numAllocatedCameraPostions = 1;
			}
			numCameraPositions = 1;
			cameraPositionsX[0] = 0f;
			cameraPositionsY[0] = 0f;
			cameraPositionsZ[0] = 0f;
		}
		if (numAvatarCameraPositions < numCameraPositions)
		{
			if (numAllocatedAvatarCameraPostions < numCameraPositions)
			{
				avatarCameraPositionsX = new float[numCameraPositions];
				avatarCameraPositionsY = new float[numCameraPositions];
				avatarCameraPositionsZ = new float[numCameraPositions];
				numAllocatedAvatarCameraPostions = numCameraPositions;
			}
			numAvatarCameraPositions = numCameraPositions;
			for (int i = 0; i < numAvatarCameraPositions; i++)
			{
				avatarCameraPositionsX[i] = cameraPositionsX[i];
				avatarCameraPositionsY[i] = cameraPositionsY[i];
				avatarCameraPositionsZ[i] = cameraPositionsZ[i];
			}
		}
		numPtLight_lvl = 0;
		needLevelShadowMap = true;
		numHitIndicators = 0;
		numHitIndicatorTextures = 0;
		currentHitIndicator = 0;
		backColor.R = 0;
		backColor.G = 0;
		backColor.B = 0;
		backColor.A = 1;
		skySideLength = 0f;
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
				if (array4[0].Equals("numObjects", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 1;
				}
				else if (array4[0].Equals("HitIndicotorTextures", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 2;
				}
				else if (array4[0].Equals("HitIndicators", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 3;
				}
				else if (array4[0].Equals("Light", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 5;
				}
				else if (array4[0].Equals("Position", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 6;
				}
				else if (array4[0].Equals("Color", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 7;
				}
				else if (array4[0].Equals("Distance", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 8;
				}
				else if (array4[0].Equals("clearColor", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 9;
				}
				else if (array4[0].Equals("ambient", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 10;
				}
				else if (array4[0].Equals("directionalColor", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 11;
				}
				else if (array4[0].Equals("directionalVector", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 12;
				}
				else if (array4[0].Equals("directionalLightAmbient", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 13;
				}
				else if (array4[0].Equals("cameraPositions", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 14;
				}
				else if (array4[0].Equals("startingView", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 15;
				}
				else if (array4[0].Equals("avatarPositions", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 16;
				}
				else if (array4[0].Equals("specular", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 17;
				}
				else if (array4[0].Equals("shadowMapHeight", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 18;
				}
				else if (array4[0].Equals("noTimeLimit", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 19;
				}
				else if (array4[0].Equals("FreeForAll", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 20;
				}
				else if (array4[0].Equals("avatarAmbient", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 21;
				}
				else if (array4[0].Equals("Targeting_Cursors", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 22;
				}
				else if (array4[0].Equals("initialView", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 23;
				}
				else if (array4[0].Equals("Sky_Cube_Side_Length", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 24;
				}
				else if (array4[0].Equals("Sky_Cube_Height_Adjustment", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 25;
				}
				else if (array4[0].Equals("Sky_Cube_Model", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 26;
				}
				switch (num4)
				{
				case 1:
					if (array4.Length > 1)
					{
						short num5 = short.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
						if (num5 > allocatedPtLight_lvl)
						{
							ptLight_lvl = new float[num5, 3];
							ptLightColor_lvl = new float[num5, 4];
							ptLightDistance_lvl = new float[num5, 1];
							allocatedPtLight_lvl = (byte)num5;
						}
						numPtLight_lvl = (byte)num5;
					}
					break;
				case 2:
					if (array4.Length <= 1)
					{
						break;
					}
					numHitIndicatorTextures = byte.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
					if (numHitIndicatorTextures > 0 && array4.Length > numHitIndicatorTextures + 1)
					{
						hitIndicatorTextureIDs = new ushort[numHitIndicatorTextures];
						hitIndicatorTextures = new string[numHitIndicatorTextures];
						int i = 0;
						int l = 2;
						while (i < numHitIndicatorTextures)
						{
							hitIndicatorTextures[i] = array4[l];
							hitIndicatorTextureIDs[i] = (ushort)mainC.texturesMain.Find_Texture(array4[l], 0);
							i++;
							l++;
						}
					}
					break;
				case 3:
					if (array4.Length > 1)
					{
						numHitIndicators = byte.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
						if (numHitIndicators > 0 && array4.Length > 5)
						{
							hitIndicators = new StructsClass.Hit_Indicator_Instance[numHitIndicators];
							hitIndicatorConfig.starTime = float.Parse(array4[2], CultureInfo.InvariantCulture.NumberFormat);
							hitIndicatorConfig.originX = ushort.Parse(array4[3], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
							hitIndicatorConfig.originY = ushort.Parse(array4[4], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
							hitIndicatorConfig.radius = ushort.Parse(array4[5], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
						}
					}
					break;
				case 5:
					if (array4.Length > 1)
					{
						num = int.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
						if (num >= numPtLight_lvl)
						{
							num = -1;
						}
					}
					break;
				case 6:
					if (array4.Length > 3 && num > -1 && num < allocatedPtLight_lvl)
					{
						ptLight_lvl[num, 0] = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
						ptLight_lvl[num, 1] = float.Parse(array4[2], CultureInfo.InvariantCulture.NumberFormat);
						ptLight_lvl[num, 2] = float.Parse(array4[3], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 7:
					if (array4.Length > 4 && num > -1 && num < allocatedPtLight_lvl)
					{
						ptLightColor_lvl[num, 0] = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
						ptLightColor_lvl[num, 1] = float.Parse(array4[2], CultureInfo.InvariantCulture.NumberFormat);
						ptLightColor_lvl[num, 2] = float.Parse(array4[3], CultureInfo.InvariantCulture.NumberFormat);
						ptLightColor_lvl[num, 3] = float.Parse(array4[4], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 8:
					if (array4.Length > 1 && num > -1 && num < allocatedPtLight_lvl)
					{
						ptLightDistance_lvl[num, 0] = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 9:
					if (array4.Length > 4)
					{
						backColor.R = (byte)(float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat) * 255f);
						backColor.G = (byte)(float.Parse(array4[2], CultureInfo.InvariantCulture.NumberFormat) * 255f);
						backColor.B = (byte)(float.Parse(array4[3], CultureInfo.InvariantCulture.NumberFormat) * 255f);
						backColor.A = (byte)(float.Parse(array4[4], CultureInfo.InvariantCulture.NumberFormat) * 255f);
					}
					break;
				case 10:
					if (array4.Length > 4)
					{
						ambientLevel[0] = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
						ambientLevel[1] = float.Parse(array4[2], CultureInfo.InvariantCulture.NumberFormat);
						ambientLevel[2] = float.Parse(array4[3], CultureInfo.InvariantCulture.NumberFormat);
						ambientLevel[3] = float.Parse(array4[4], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 11:
					if (array4.Length > 4)
					{
						directionalLightColor[0] = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
						directionalLightColor[1] = float.Parse(array4[2], CultureInfo.InvariantCulture.NumberFormat);
						directionalLightColor[2] = float.Parse(array4[3], CultureInfo.InvariantCulture.NumberFormat);
						directionalLightColor[3] = float.Parse(array4[4], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 12:
					if (array4.Length > 3)
					{
						directionalLightVector[0] = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
						directionalLightVector[1] = float.Parse(array4[2], CultureInfo.InvariantCulture.NumberFormat);
						directionalLightVector[2] = float.Parse(array4[3], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 13:
					if (array4.Length > 4)
					{
						directionalLightBounce[0] = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
						directionalLightBounce[1] = float.Parse(array4[2], CultureInfo.InvariantCulture.NumberFormat);
						directionalLightBounce[2] = float.Parse(array4[3], CultureInfo.InvariantCulture.NumberFormat);
						directionalLightBounce[3] = float.Parse(array4[4], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 14:
				{
					if (array4.Length <= 1)
					{
						break;
					}
					int i = int.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
					if (array4.Length > i * 3 + 1)
					{
						if (numAllocatedCameraPostions < i)
						{
							cameraPositionsX = new float[i];
							cameraPositionsY = new float[i];
							cameraPositionsZ = new float[i];
							numAllocatedCameraPostions = (byte)i;
						}
						numCameraPositions = (byte)i;
						int l = 0;
						int num6 = 2;
						for (; l < i; l++)
						{
							cameraPositionsX[l] = float.Parse(array4[num6++], CultureInfo.InvariantCulture.NumberFormat);
							cameraPositionsY[l] = float.Parse(array4[num6++], CultureInfo.InvariantCulture.NumberFormat);
							cameraPositionsZ[l] = float.Parse(array4[num6++], CultureInfo.InvariantCulture.NumberFormat);
						}
					}
					break;
				}
				case 15:
					if (array4.Length > 1)
					{
						global::Players.Players.lastView = byte.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
						global::Players.Players.currentView = global::Players.Players.lastView;
					}
					break;
				case 16:
				{
					if (array4.Length <= 1)
					{
						break;
					}
					int i = int.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
					if (array4.Length > i * 3 + 1)
					{
						if (numAllocatedAvatarCameraPostions < i)
						{
							avatarCameraPositionsX = new float[i];
							avatarCameraPositionsY = new float[i];
							avatarCameraPositionsZ = new float[i];
							numAllocatedAvatarCameraPostions = (byte)i;
						}
						numAvatarCameraPositions = (byte)i;
						int l = 0;
						int num6 = 2;
						for (; l < i; l++)
						{
							avatarCameraPositionsX[l] = float.Parse(array4[num6++], CultureInfo.InvariantCulture.NumberFormat);
							avatarCameraPositionsY[l] = float.Parse(array4[num6++], CultureInfo.InvariantCulture.NumberFormat);
							avatarCameraPositionsZ[l] = float.Parse(array4[num6++], CultureInfo.InvariantCulture.NumberFormat);
						}
					}
					break;
				}
				case 17:
					if (array4.Length > 1)
					{
						specular = int.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 18:
					if (array4.Length > 1)
					{
						shadowMapHeight = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 21:
					if (array4.Length > 3)
					{
						ambientAvatar[0] = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
						ambientAvatar[1] = float.Parse(array4[2], CultureInfo.InvariantCulture.NumberFormat);
						ambientAvatar[2] = float.Parse(array4[3], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 22:
					if (array4.Length <= 1)
					{
						break;
					}
					global::Textures.Textures.numTargetingCrosshairs = byte.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
					if (array4.Length > global::Textures.Textures.numTargetingCrosshairs + 1)
					{
						if (global::Textures.Textures.numTargetingCrosshairs > 0)
						{
							global::Textures.Textures.texCrossHair = new ushort[global::Textures.Textures.numTargetingCrosshairs];
							global::Textures.Textures.texCrossHair[0] = (ushort)mainC.texturesMain.Find_Texture(array4[2], 0);
							global::Textures.Textures.texCrossHair[1] = (ushort)mainC.texturesMain.Find_Texture(array4[3], 0);
							global::Textures.Textures.texCrossHair[2] = (ushort)mainC.texturesMain.Find_Texture(array4[4], 0);
						}
						else
						{
							ushort[] texCrossHair2 = new ushort[3];
							global::Textures.Textures.texCrossHair = texCrossHair2;
						}
					}
					break;
				case 23:
					if (array4.Length > 6)
					{
						initialCamPosX = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
						initialCamPosY = float.Parse(array4[2], CultureInfo.InvariantCulture.NumberFormat);
						initialCamPosZ = float.Parse(array4[3], CultureInfo.InvariantCulture.NumberFormat);
						initialCamObjX = float.Parse(array4[4], CultureInfo.InvariantCulture.NumberFormat);
						initialCamObjY = float.Parse(array4[5], CultureInfo.InvariantCulture.NumberFormat);
						initialCamObjZ = float.Parse(array4[6], CultureInfo.InvariantCulture.NumberFormat);
						initialWorldX = float.Parse(array4[7], CultureInfo.InvariantCulture.NumberFormat);
						initialWorldY = float.Parse(array4[8], CultureInfo.InvariantCulture.NumberFormat);
						initialWorldZ = float.Parse(array4[9], CultureInfo.InvariantCulture.NumberFormat);
						initialCameraRotation = float.Parse(array4[10], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 24:
					if (array4.Length > 1)
					{
						skySideLength = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
						negativeSkySideHalfLength = (0f - skySideLength) / 2f;
					}
					break;
				case 25:
					if (array4.Length > 1)
					{
						skyHeighAdjustment = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 26:
					if (array4.Length > 1)
					{
						global::Models.Models.modSkyDome = mainC.modelsMain.Find_Level_Model(array4[1]);
						global::Models.Models.modVbo[global::Models.Models.modSkyDome].inLevelVBO = true;
					}
					break;
				}
			}
		}
		stream.Close();
		float num7 = (float)Math.Sqrt(directionalLightVector[0] * directionalLightVector[0] + directionalLightVector[1] * directionalLightVector[1] + directionalLightVector[2] * directionalLightVector[2]);
		if (num7 != 0f)
		{
			directionalLightVector[0] /= num7;
			directionalLightVector[1] /= num7;
			directionalLightVector[2] /= num7;
		}
		effect1.Parameters["dLightDir"].SetValue(directionalLightVector);
		effect1.Parameters["dLightColor"].SetValue(directionalLightColor);
		effect1.Parameters["dLightBounce"].SetValue(directionalLightBounce);
		effect1.Parameters["Ambient"].SetValue(ambientLevel);
		effect1.Parameters["Specular"].SetValue(specular);
	}

	public void Init_MiniMap()
	{
		miniMapX = (short)((float)(mainC.curGame.width - mainC.curGame.safeWidth) / 2f);
		miniMapY = (short)((float)(mainC.curGame.height - mainC.curGame.safeHeight) / 2f);
		miniMapScale = 1f;
		miniMapItemScale = 1f;
		global::Textures.Textures.texHudMiniMap = mainC.texturesMain.Find_Texture("MiniMap1", 0);
		global::Textures.Textures.texHudMiniMapFrame = mainC.texturesMain.Find_Texture("MiniMapBorder", 0);
		miniMapBorderX = (short)(miniMapX - global::Textures.Textures.texMain.texData[global::Textures.Textures.texHudMiniMapFrame].Width);
		miniMapBorderY = (short)(miniMapY - global::Textures.Textures.texMain.texData[global::Textures.Textures.texHudMiniMapFrame].Height);
		miniMapInnerRadius = 87.5f;
		miniMapInnerRadiusInset = 72.5f;
		miniMapInnerRadiusSqr = miniMapInnerRadiusInset * miniMapInnerRadiusInset;
	}

	public void Load_MiniMap_Data(string fileName)
	{
		int num = -1;
		miniMapRed.R = 247;
		miniMapRed.G = 3;
		miniMapRed.B = 3;
		miniMapBlue.R = 0;
		miniMapBlue.G = 174;
		miniMapBlue.B = byte.MaxValue;
		miniMapStaticTexture = false;
		for (int i = 0; i < numAllocatedMiniMapItems; i++)
		{
			mapItems[i].status = 0;
			mapItems[i].startingStatus = 0;
			mapItems[i].type = 0;
			mapItems[i].colorR = byte.MaxValue;
			mapItems[i].colorG = byte.MaxValue;
			mapItems[i].colorB = byte.MaxValue;
		}
		numMiniMapItems = 0;
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
				if (array4[0].Equals("numObjects", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 1;
				}
				else if (array4[0].Equals("mapItem", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 2;
				}
				else if (array4[0].Equals("type", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 3;
				}
				else if (array4[0].Equals("status", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 4;
				}
				else if (array4[0].Equals("texture", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 5;
				}
				else if (array4[0].Equals("position", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 6;
				}
				else if (array4[0].Equals("mapTexture", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 7;
				}
				else if (array4[0].Equals("mapPosition", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 8;
				}
				else if (array4[0].Equals("Map_Item_Scale_Factor", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 9;
				}
				else if (array4[0].Equals("playerIcon", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 10;
				}
				else if (array4[0].Equals("color", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 11;
				}
				else if (array4[0].Equals("activeColor", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 12;
				}
				else if (array4[0].Equals("Map_Origin", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 13;
				}
				else if (array4[0].Equals("Map_Scale_Factor", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 14;
				}
				else if (array4[0].Equals("Map_Zoom", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 15;
				}
				else if (array4[0].Equals("mapBorderTexture", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 16;
				}
				else if (array4[0].Equals("mapBorderHalfSize", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 17;
				}
				else if (array4[0].Equals("mapInnerRadius", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 18;
				}
				else if (array4[0].Equals("staticTexture", StringComparison.OrdinalIgnoreCase))
				{
					num4 = 19;
				}
				else if (array4[0].Equals("mapInnerRadiusInset", StringComparison.OrdinalIgnoreCase))
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
					ushort num5 = ushort.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
					if (num5 > numAllocatedMiniMapItems)
					{
						numAllocatedMiniMapItems = num5;
						mapItems = new StructsClass.MiniMapItem[num5];
						for (int i = 0; i < num5; i++)
						{
							mapItems[i] = default(StructsClass.MiniMapItem);
							mapItems[i].status = 0;
							mapItems[i].startingStatus = 0;
							mapItems[i].type = 0;
							mapItems[i].colorR = byte.MaxValue;
							mapItems[i].colorG = byte.MaxValue;
							mapItems[i].colorB = byte.MaxValue;
						}
					}
					numMiniMapItems = num5;
					break;
				}
				case 2:
					if (array4.Length > 1)
					{
						num = int.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
						if (num >= numMiniMapItems)
						{
							num = -1;
						}
					}
					break;
				case 3:
					if (array4.Length > 1 && num > -1)
					{
						mapItems[num].type = byte.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 4:
					if (array4.Length > 1 && num > -1)
					{
						mapItems[num].startingStatus = byte.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 5:
					if (array4.Length > 1 && num > -1)
					{
						mapItems[num].texture = array4[1];
						mapItems[num].texID = (ushort)mainC.texturesMain.Find_Texture(array4[1], 0);
					}
					break;
				case 6:
					if (array4.Length > 2 && num > -1)
					{
						mapItems[num].x1 = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
						mapItems[num].y1 = float.Parse(array4[2], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 7:
					if (array4.Length > 1)
					{
						global::Textures.Textures.texHudMiniMap = mainC.texturesMain.Find_Texture(array4[1], 0);
					}
					break;
				case 8:
					if (array4.Length > 2)
					{
						miniMapX = (short)((float)(mainC.curGame.width - mainC.curGame.safeWidth) / 2f + (float)short.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat));
						miniMapY = (short)((float)(mainC.curGame.height - mainC.curGame.safeHeight) / 2f + (float)short.Parse(array4[2], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat));
					}
					break;
				case 9:
					if (array4.Length > 1)
					{
						miniMapItemScale = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 10:
					if (array4.Length > 1)
					{
						miniMapPlayerTexture = (ushort)mainC.texturesMain.Find_Texture(array4[1], 0);
					}
					break;
				case 11:
					if (array4.Length > 3 && num > -1)
					{
						mapItems[num].normalColorR = byte.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
						mapItems[num].normalColorG = byte.Parse(array4[2], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
						mapItems[num].normalColorB = byte.Parse(array4[3], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 12:
					if (array4.Length > 3 && num > -1)
					{
						mapItems[num].highlightColorR = byte.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
						mapItems[num].highlightColorG = byte.Parse(array4[2], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
						mapItems[num].highlightColorB = byte.Parse(array4[3], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 13:
					if (array4.Length > 2)
					{
						miniMapPlayerOriginX = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
						miniMapPlayerOriginY = float.Parse(array4[2], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 14:
					if (array4.Length > 1)
					{
						miniMapScale = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 15:
					if (array4.Length > 1)
					{
						miniMapZoomFactor = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 16:
					if (array4.Length > 1)
					{
						global::Textures.Textures.texHudMiniMapFrame = mainC.texturesMain.Find_Texture(array4[1], 0);
					}
					break;
				case 17:
					if (array4.Length > 2)
					{
						miniMapBorderHalfWidth = short.Parse(array4[1], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
						miniMapBorderHalfHeight = short.Parse(array4[2], NumberStyles.Any, CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 18:
					if (array4.Length > 1)
					{
						miniMapInnerRadius = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				case 19:
					miniMapStaticTexture = true;
					break;
				case 20:
					if (array4.Length > 1)
					{
						miniMapInnerRadiusInset = float.Parse(array4[1], CultureInfo.InvariantCulture.NumberFormat);
					}
					break;
				}
			}
		}
		stream.Close();
		miniMapInnerRadiusSqr = miniMapInnerRadiusInset * miniMapInnerRadiusInset;
		miniMapBorderX = (short)(miniMapX - miniMapBorderHalfWidth);
		miniMapBorderY = (short)(miniMapY - miniMapBorderHalfHeight);
		for (int i = 0; i < numMiniMapItems; i++)
		{
			mapItems[i].colorR = mapItems[i].normalColorR;
			mapItems[i].colorG = mapItems[i].normalColorG;
			mapItems[i].colorB = mapItems[i].normalColorB;
			mapItems[i].status = mapItems[i].startingStatus;
		}
	}

	public void Render_MiniMap()
	{
		Rectangle destinationRectangle = default(Rectangle);
		Color color = new Color(255, 255, 255, 255);
		float num = (global::Players.Players.players[0].charP.position.v[0] - miniMapPlayerOriginX) * miniMapScale;
		float num2 = (global::Players.Players.players[0].charP.position.v[1] - miniMapPlayerOriginY) * miniMapScale;
		short num3 = miniMapX;
		short num4 = miniMapY;
		Matrix matrix = Matrix.CreateRotationZ((0f - global::Players.Players.players[0].zRotation) * ((float)Math.PI / 180f));
		rGraphics.SetVertexBuffer(global::Models.Models.mVertexBufferObjects);
		rGraphics.Indices = global::Models.Models.mIndexBufferObjects;
		rGraphics.BlendState = BlendState.Opaque;
		rGraphics.DepthStencilState = depthBufferDisabled;
		effect1.Parameters["ViewProjection"].SetValue(matrixO);
		if (!miniMapStaticTexture)
		{
			effect1.Parameters["TextureMultiplier"].SetValue(miniMapZoomFactor);
			texAdj1[0] = 0.5f - miniMapZoomFactor * 0.5f + num;
			texAdj1[1] = 0.5f - miniMapZoomFactor * 0.5f - num2;
			effect1.Parameters["World"].SetValue(Matrix.CreateScale(miniMapInnerRadius, miniMapInnerRadius, 1f) * matrix * Matrix.CreateTranslation(-mainC.curGame.halfWidth + num3, mainC.curGame.halfHeight - num4, -10f));
		}
		else
		{
			effect1.Parameters["World"].SetValue(Matrix.CreateScale(miniMapInnerRadius, miniMapInnerRadius, 1f) * Matrix.CreateTranslation(-mainC.curGame.halfWidth + num3, mainC.curGame.halfHeight - num4, -10f));
		}
		effect1.Parameters["texAdj"].SetValue(texAdj1);
		effect1.CurrentTechnique = effect1.Techniques["MiniMap"];
		effect1.CurrentTechnique.Passes[0].Apply();
		effect1.Parameters["BaseTexture"].SetValue(global::Textures.Textures.texMain.texData[global::Textures.Textures.texHudMiniMap]);
		effect1.Parameters["MaterialTexture"].SetValue(global::Textures.Textures.texMain.texData[global::Textures.Textures.texDefaultNormalMap]);
		effect1.CurrentTechnique.Passes[0].Apply();
		mainC.modelsMain.Render_Model_Basic(global::Models.Models.modGeometryFlatCircle16);
		rGraphics.DepthStencilState = depthBufferEnabled;
		destinationRectangle.Width = 8;
		destinationRectangle.Height = 8;
		splashSprite.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
		float num5 = miniMapItemScale / miniMapZoomFactor;
		for (short num6 = 0; num6 < numMiniMapItems; num6++)
		{
			if (mapItems[num6].status == 2)
			{
				num = (mapItems[num6].x1 - global::Players.Players.players[0].charP.position.v[0]) * num5;
				num2 = (mapItems[num6].y1 - global::Players.Players.players[0].charP.position.v[1]) * num5;
				if (num * num + num2 * num2 < miniMapInnerRadiusSqr)
				{
					color.R = mapItems[num6].colorR;
					color.G = mapItems[num6].colorG;
					color.B = mapItems[num6].colorB;
					destinationRectangle.X = (int)((float)num3 + num * matrix.M11 + num2 * matrix.M21);
					destinationRectangle.Y = (int)((float)num4 - num * matrix.M12 - num2 * matrix.M22);
					splashSprite.Draw(global::Textures.Textures.texMain.texData[mapItems[num6].texID], destinationRectangle, null, color, 0f, new Vector2(4f, 4f), SpriteEffects.None, 0f);
				}
				else if (mapItems[num6].type == 1)
				{
					float num7 = (float)Math.Sqrt(num * num + num2 * num2);
					if (num7 != 0f)
					{
						num = num / num7 * miniMapInnerRadiusInset;
						num2 = num2 / num7 * miniMapInnerRadiusInset;
					}
					color.R = mapItems[num6].colorR;
					color.G = mapItems[num6].colorG;
					color.B = mapItems[num6].colorB;
					destinationRectangle.X = (int)((float)num3 + num * matrix.M11 + num2 * matrix.M21);
					destinationRectangle.Y = (int)((float)num4 - num * matrix.M12 - num2 * matrix.M22);
					splashSprite.Draw(global::Textures.Textures.texMain.texData[mapItems[num6].texID], destinationRectangle, null, color, 0f, new Vector2(4f, 4f), SpriteEffects.None, 0f);
				}
			}
		}
		for (short num6 = 1; num6 < global::MainGame.MainGame.maxGamePlayers; num6++)
		{
			if ((global::Players.Players.players[num6].onmap & 4) > 0)
			{
				num = (global::Players.Players.players[num6].charP.position.v[0] - global::Players.Players.players[0].charP.position.v[0]) * num5;
				num2 = (global::Players.Players.players[num6].charP.position.v[1] - global::Players.Players.players[0].charP.position.v[1]) * num5;
				if (num * num + num2 * num2 < miniMapInnerRadiusSqr)
				{
					destinationRectangle.X = (int)((float)num3 + num * matrix.M11 + num2 * matrix.M21);
					destinationRectangle.Y = (int)((float)num4 - num * matrix.M12 - num2 * matrix.M22);
					if ((global::Players.Players.players[num6].teamMask & global::Players.Players.enemyTeamMask) != 0)
					{
						splashSprite.Draw(global::Textures.Textures.texMain.texData[global::Players.Players.playerRaces[global::Players.Players.players[num6].race].miniMapIconID[global::Players.Players.players[num6].type]], destinationRectangle, null, miniMapRed, (0f - global::Players.Players.players[num6].zRotation + global::Players.Players.players[0].zRotation) * ((float)Math.PI / 180f), new Vector2(4f, 4f), SpriteEffects.None, 0f);
					}
					else
					{
						splashSprite.Draw(global::Textures.Textures.texMain.texData[global::Players.Players.playerRaces[global::Players.Players.players[num6].race].miniMapIconID[global::Players.Players.players[num6].type]], destinationRectangle, null, miniMapBlue, (0f - global::Players.Players.players[num6].zRotation + global::Players.Players.players[0].zRotation) * ((float)Math.PI / 180f), new Vector2(4f, 4f), SpriteEffects.None, 0f);
					}
				}
			}
		}
		if ((global::Players.Players.players[0].onmap & 0xC) > 0)
		{
			destinationRectangle.X = num3;
			destinationRectangle.Y = num4;
			splashSprite.Draw(global::Textures.Textures.texMain.texData[miniMapPlayerTexture], destinationRectangle, null, Color.White, 0f, new Vector2(4f, 4f), SpriteEffects.None, 0f);
		}
		rsPos.X = miniMapBorderX;
		rsPos.Y = miniMapBorderY;
		splashSprite.Draw(global::Textures.Textures.texMain.texData[global::Textures.Textures.texHudMiniMapFrame], rsPos, Color.White);
		splashSprite.End();
		effect1.Parameters["ViewProjection"].SetValue(matrixVP);
		effect1.Parameters["World"].SetValue(matrixW);
		mainC.fontmoduleMain.Reset_Graphics_Adapter_After_SpriteBatch();
	}

	public ushort Add_MiniMap_Item(string textureName, float posX, float posY, float R, float G, float B, byte status, byte type)
	{
		ushort num;
		for (num = 0; num < numMiniMapItems; num++)
		{
			if (mapItems[num].status < 2)
			{
				mapItems[num].startingStatus = status;
				mapItems[num].status = status;
				mapItems[num].x1 = posX;
				mapItems[num].y1 = posY;
				mapItems[num].normalColorR = (byte)Math.Round(R * 255f);
				mapItems[num].normalColorG = (byte)Math.Round(G * 255f);
				mapItems[num].normalColorB = (byte)Math.Round(B * 255f);
				mapItems[num].type = type;
				mapItems[num].texture = textureName;
				mapItems[num].texID = (ushort)mainC.texturesMain.Find_Texture(textureName, 0);
				return num;
			}
		}
		if (numMiniMapItems < numAllocatedMiniMapItems)
		{
			mapItems[numMiniMapItems].startingStatus = status;
			mapItems[numMiniMapItems].status = status;
			mapItems[numMiniMapItems].x1 = posX;
			mapItems[numMiniMapItems].y1 = posY;
			mapItems[numMiniMapItems].normalColorR = (byte)Math.Round(R * 255f);
			mapItems[numMiniMapItems].normalColorG = (byte)Math.Round(G * 255f);
			mapItems[numMiniMapItems].normalColorB = (byte)Math.Round(B * 255f);
			mapItems[numMiniMapItems].type = type;
			mapItems[numMiniMapItems].texture = textureName;
			mapItems[numMiniMapItems].texID = (ushort)mainC.texturesMain.Find_Texture(textureName, 0);
			return numMiniMapItems++;
		}
		StructsClass.MiniMapItem[] array = new StructsClass.MiniMapItem[numAllocatedMiniMapItems];
		for (num = 0; num < numAllocatedMiniMapItems; num++)
		{
			array[num].startingStatus = mapItems[num].startingStatus;
			array[num].status = mapItems[num].status;
			array[num].x1 = mapItems[num].x1;
			array[num].y1 = mapItems[num].y1;
			array[num].normalColorR = mapItems[num].normalColorR;
			array[num].normalColorG = mapItems[num].normalColorG;
			array[num].normalColorB = mapItems[num].normalColorB;
			array[num].type = mapItems[num].type;
			array[num].texture = mapItems[num].texture;
			array[num].texID = mapItems[num].texID;
		}
		ushort num2 = (ushort)(numAllocatedMiniMapItems + 10);
		mapItems = new StructsClass.MiniMapItem[num2];
		for (num = 0; num < numAllocatedMiniMapItems; num++)
		{
			mapItems[num].startingStatus = array[num].startingStatus;
			mapItems[num].status = array[num].status;
			mapItems[num].x1 = array[num].x1;
			mapItems[num].y1 = array[num].y1;
			mapItems[num].normalColorR = array[num].normalColorR;
			mapItems[num].normalColorG = array[num].normalColorG;
			mapItems[num].normalColorB = array[num].normalColorB;
			mapItems[num].type = array[num].type;
			mapItems[num].texture = array[num].texture;
			mapItems[num].texID = array[num].texID;
		}
		while (num < num2)
		{
			mapItems[num].status = 0;
			num++;
		}
		mapItems[numMiniMapItems].startingStatus = status;
		mapItems[numMiniMapItems].status = status;
		mapItems[numMiniMapItems].x1 = posX;
		mapItems[numMiniMapItems].y1 = posY;
		mapItems[numMiniMapItems].normalColorR = (byte)Math.Round(R * 255f);
		mapItems[numMiniMapItems].normalColorG = (byte)Math.Round(G * 255f);
		mapItems[numMiniMapItems].normalColorB = (byte)Math.Round(B * 255f);
		mapItems[numMiniMapItems].type = type;
		mapItems[numMiniMapItems].texture = textureName;
		mapItems[numMiniMapItems].texID = (ushort)mainC.texturesMain.Find_Texture(textureName, 0);
		numAllocatedMiniMapItems = num2;
		return numMiniMapItems++;
	}

	public void Set_MiniMap_Item_Color(ushort itemID, float R, float G, float B)
	{
		if (itemID < numMiniMapItems)
		{
			mapItems[itemID].normalColorR = (byte)Math.Round(R * 255f);
			mapItems[itemID].normalColorG = (byte)Math.Round(G * 255f);
			mapItems[itemID].normalColorB = (byte)Math.Round(B * 255f);
			mapItems[itemID].colorR = mapItems[itemID].normalColorR;
			mapItems[itemID].colorG = mapItems[itemID].normalColorG;
			mapItems[itemID].colorB = mapItems[itemID].normalColorB;
		}
	}

	public void Remove_MiniMap_Item(ushort itemID)
	{
		mapItems[itemID].status = 0;
	}

	public void Hide_MiniMap_Item(ushort itemID)
	{
		mapItems[itemID].status = 1;
	}

	public void Show_MiniMap_Item(ushort itemID)
	{
		mapItems[itemID].status = 2;
	}

	public void Update_MiniMap_Item_Texture(ushort itemID, string textureName)
	{
		mapItems[itemID].texture = textureName;
		mapItems[itemID].texID = (ushort)mainC.texturesMain.Find_Texture(mapItems[itemID].texture, 0);
	}

	public void Highlight_MiniMap_Item(ushort mapID)
	{
		mapItems[mapID].colorR = mapItems[mapID].highlightColorR;
		mapItems[mapID].colorG = mapItems[mapID].highlightColorG;
		mapItems[mapID].colorB = mapItems[mapID].highlightColorB;
	}

	public void Remove_Highlight_MiniMap_Item(ushort mapID)
	{
		mapItems[mapID].colorR = mapItems[mapID].normalColorR;
		mapItems[mapID].colorG = mapItems[mapID].normalColorG;
		mapItems[mapID].colorB = mapItems[mapID].normalColorB;
	}

	public void Reset_Round_MiniMap()
	{
		for (short num = 0; num < numMiniMapItems; num++)
		{
			mapItems[num].status = mapItems[num].startingStatus;
			mapItems[num].colorR = mapItems[num].normalColorR;
			mapItems[num].colorG = mapItems[num].normalColorG;
			mapItems[num].colorB = mapItems[num].normalColorB;
		}
		if (miniMapStaticTexture)
		{
			effect1.Parameters["TextureMultiplier"].SetValue(1f);
		}
		texAdj1[0] = 0f;
		texAdj1[1] = 0f;
	}

	public ushort Add_New_Instance(byte mode, short curItem, byte type, ushort modelID, float colorR, float colorG, float colorB, float colorA, float rotX, float rotY, float rotZ, float px, float py, float pz, float sx, float sy, float sz, float tx, float ty, byte active, byte needsBB, bool useVbo, bool usesCollisionModel, byte incrementAmount)
	{
		switch (mode)
		{
		case 0:
			curItem = numRenderingInstances;
			if (++numRenderingInstances >= numAllocatedInstances)
			{
				StructsClass.RenderInstance[] array = new StructsClass.RenderInstance[curItem];
				for (short num2 = 0; num2 < curItem; num2++)
				{
					ref StructsClass.RenderInstance reference = ref array[num2];
					reference = renderingInstances[num2];
				}
				numAllocatedInstances = (short)(numRenderingInstances + incrementAmount);
				renderingInstances = new StructsClass.RenderInstance[numAllocatedInstances];
				for (short num2 = 0; num2 < curItem; num2++)
				{
					ref StructsClass.RenderInstance reference2 = ref renderingInstances[num2];
					reference2 = array[num2];
				}
			}
			renderingInstances[curItem].useVbo = useVbo;
			renderingInstances[curItem].usesCollisionModel = usesCollisionModel;
			renderingInstances[curItem].numCollisionModels = 0;
			renderingInstances[curItem].type = type;
			renderingInstances[curItem].numModels = 1;
			renderingInstances[curItem].modelList = new short[1];
			renderingInstances[curItem].modelList[0] = (short)modelID;
			renderingInstances[curItem].color.X = colorR;
			renderingInstances[curItem].color.Y = colorG;
			renderingInstances[curItem].color.Z = colorB;
			renderingInstances[curItem].color.W = colorA;
			renderingInstances[curItem].rotX = rotX;
			renderingInstances[curItem].rotY = rotY;
			renderingInstances[curItem].rotZ = rotZ;
			renderingInstances[curItem].x = new float[1];
			renderingInstances[curItem].y = new float[1];
			renderingInstances[curItem].z = new float[1];
			renderingInstances[curItem].sx = new float[1];
			renderingInstances[curItem].sy = new float[1];
			renderingInstances[curItem].sz = new float[1];
			renderingInstances[curItem].tx = new float[1];
			renderingInstances[curItem].ty = new float[1];
			renderingInstances[curItem].active = new byte[1];
			renderingInstances[curItem].needsBB = new byte[1];
			renderingInstances[curItem].bbType = new byte[1];
			renderingInstances[curItem].numItems = 1;
			renderingInstances[curItem].x[0] = px;
			renderingInstances[curItem].y[0] = py;
			renderingInstances[curItem].z[0] = pz;
			renderingInstances[curItem].sx[0] = sx;
			renderingInstances[curItem].sy[0] = sy;
			renderingInstances[curItem].sz[0] = sz;
			renderingInstances[curItem].tx[0] = tx;
			renderingInstances[curItem].ty[0] = ty;
			renderingInstances[curItem].active[0] = active;
			renderingInstances[curItem].needsBB[0] = needsBB;
			renderingInstances[curItem].bbType[0] = 132;
			switch (renderingInstances[curItem].type)
			{
			case 0:
			case 10:
				renderingInstances[curItem].mv = Matrix.CreateRotationZ(rotZ * ((float)Math.PI / 180f)) * Matrix.CreateRotationY(rotY * ((float)Math.PI / 180f)) * Matrix.CreateRotationX(rotX * ((float)Math.PI / 180f));
				break;
			case 2:
			{
				short num3 = renderingInstances[curItem].numItems;
				for (short num = 0; num < num3; num++)
				{
					renderingInstances[curItem].bbType[num] = 0;
				}
				break;
			}
			}
			if (renderingInstances[curItem].usesCollisionModel)
			{
				ushort gid = mainC.maingameMain.Register_Game_Item(3, (ushort)curItem, (ushort)curItem);
				short num3 = renderingInstances[curItem].numItems;
				for (short num = 0; num < num3; num++)
				{
					Matrix mv = Matrix.CreateScale(renderingInstances[curItem].sx[num], renderingInstances[curItem].sy[num], renderingInstances[curItem].sz[num]) * renderingInstances[curItem].mv * Matrix.CreateTranslation(renderingInstances[curItem].x[num], renderingInstances[curItem].y[num], renderingInstances[curItem].z[num]);
					for (short num2 = 0; num2 < renderingInstances[curItem].numCollisionModels; num2++)
					{
						mainC.zonesMain.Add_CollisionModel_To_Zone(renderingInstances[curItem].zoneID[num2], mainC.collisionMain.Find_Collision_Model(renderingInstances[curItem].collisionModel[num2], 0), gid, ref mv);
					}
				}
			}
			return (ushort)curItem;
		case 1:
		{
			StructsClass.RenderInstance renderInstance = renderingInstances[curItem];
			short num = ++renderingInstances[curItem].numItems;
			renderingInstances[curItem].x = new float[num];
			renderingInstances[curItem].y = new float[num];
			renderingInstances[curItem].z = new float[num];
			renderingInstances[curItem].sx = new float[num];
			renderingInstances[curItem].sy = new float[num];
			renderingInstances[curItem].sz = new float[num];
			renderingInstances[curItem].tx = new float[num];
			renderingInstances[curItem].ty = new float[num];
			renderingInstances[curItem].active = new byte[num];
			renderingInstances[curItem].needsBB = new byte[num];
			renderingInstances[curItem].bbType = new byte[num];
			short num2 = 0;
			short num3 = (short)(num - 1);
			while (num2 < num3)
			{
				renderingInstances[curItem].x[num2] = renderInstance.x[num2];
				renderingInstances[curItem].y[num2] = renderInstance.y[num2];
				renderingInstances[curItem].z[num2] = renderInstance.z[num2];
				renderingInstances[curItem].sx[num2] = renderInstance.sx[num2];
				renderingInstances[curItem].sy[num2] = renderInstance.sy[num2];
				renderingInstances[curItem].sz[num2] = renderInstance.sz[num2];
				renderingInstances[curItem].tx[num2] = renderInstance.tx[num2];
				renderingInstances[curItem].ty[num2] = renderInstance.ty[num2];
				renderingInstances[curItem].active[num2] = renderInstance.active[num2];
				renderingInstances[curItem].needsBB[num2] = renderInstance.needsBB[num2];
				renderingInstances[curItem].bbType[num2] = renderInstance.bbType[num2];
				num2++;
			}
			renderingInstances[curItem].x[num2] = px;
			renderingInstances[curItem].y[num2] = py;
			renderingInstances[curItem].z[num2] = pz;
			renderingInstances[curItem].sx[num2] = sx;
			renderingInstances[curItem].sy[num2] = sy;
			renderingInstances[curItem].sz[num2] = sz;
			renderingInstances[curItem].tx[num2] = tx;
			renderingInstances[curItem].ty[num2] = ty;
			renderingInstances[curItem].active[num2] = active;
			renderingInstances[curItem].needsBB[num2] = needsBB;
			renderingInstances[curItem].bbType[num2] = 132;
			renderingInstances[curItem].type = 10;
			return (ushort)num2;
		}
		default:
			return 0;
		}
	}

	public ushort Add_Model_To_Instance(ushort instanceID, ushort modID)
	{
		ushort num = renderingInstances[instanceID].numModels++;
		ushort num2 = (ushort)(num + 1);
		short[] array = new short[num];
		ushort num3;
		for (num3 = 0; num3 < num; num3++)
		{
			array[num3] = renderingInstances[instanceID].modelList[num3];
		}
		renderingInstances[instanceID].modelList = new short[num2];
		for (num3 = 0; num3 < num; num3++)
		{
			renderingInstances[instanceID].modelList[num3] = array[num3];
		}
		renderingInstances[instanceID].modelList[num3] = (short)modID;
		return num3;
	}

	public void Update_Instance_Position(ushort id, short count, float x, float y, float z)
	{
		if (id < numRenderingInstances)
		{
			renderingInstances[id].x[count] = x;
			renderingInstances[id].y[count] = y;
			renderingInstances[id].z[count] = z;
		}
	}

	public void Render_Muzzle_Flashes()
	{
		rGraphics.BlendState = blendSourceAlpha;
		effect1.Parameters["ViewProjection"].SetValue(matrixP);
		effect1.CurrentTechnique = effect1.Techniques["ColorParticle"];
		for (ushort num = 0; num < global::MainGame.MainGame.maxGamePlayers; num++)
		{
			if (muzzleFlashes[rBufferID, num].timeRemaining > 0f)
			{
				float num2 = 0.75f;
				if (muzzleFlashes[rBufferID, num].timeRemaining < muzzleFlashes[rBufferID, num].fadeoutTime)
				{
					num2 -= muzzleFlashes[rBufferID, num].timeRemaining / muzzleFlashes[rBufferID, num].fadeoutTime * 0.75f;
				}
				float xPosition = (muzzleFlashes[rBufferID, num].x + cameraShakeX) * matrixV.M11 + (muzzleFlashes[rBufferID, num].y + cameraShakeY) * matrixV.M21 + (muzzleFlashes[rBufferID, num].z + cameraShakeZ) * matrixV.M31 + matrixV.M41;
				float yPosition = (muzzleFlashes[rBufferID, num].x + cameraShakeX) * matrixV.M12 + (muzzleFlashes[rBufferID, num].y + cameraShakeY) * matrixV.M22 + (muzzleFlashes[rBufferID, num].z + cameraShakeZ) * matrixV.M32 + matrixV.M42;
				float zPosition = (muzzleFlashes[rBufferID, num].x + cameraShakeX) * matrixV.M13 + (muzzleFlashes[rBufferID, num].y + cameraShakeY) * matrixV.M23 + (muzzleFlashes[rBufferID, num].z + cameraShakeZ) * matrixV.M33 + matrixV.M43;
				Matrix value = Matrix.CreateRotationZ(cosineFluctuation * 360f * ((float)Math.PI / 180f)) * Matrix.CreateTranslation(xPosition, yPosition, zPosition);
				effect1.Parameters["World"].SetValue(value);
				effect1.Parameters["AlphaAdjust"].SetValue(num2);
				effect1.Parameters["BaseTexture"].SetValue(global::Textures.Textures.texMain.texData[global::Weapons.Weapons.muzzleFlashTexturesMainPlayer[muzzleFlashes[rBufferID, num].textureIndex]]);
				effect1.CurrentTechnique.Passes[0].Apply();
				mainC.modelsMain.Render_Model_Basic(global::Models.Models.modFlatPlane);
			}
		}
		effect1.Parameters["AlphaAdjust"].SetValue(1f);
	}

	public void Render_HitIndicators()
	{
		if (numHitIndicators < 1)
		{
			return;
		}
		Rectangle destinationRectangle = default(Rectangle);
		Color color = new Color(255, 255, 255, 255);
		Vector2 origin = default(Vector2);
		destinationRectangle.Width = global::Textures.Textures.texMain.texData[hitIndicatorTextureIDs[0]].Width;
		destinationRectangle.Height = global::Textures.Textures.texMain.texData[hitIndicatorTextureIDs[0]].Height;
		origin.X = (int)hitIndicatorConfig.originX;
		origin.Y = (int)hitIndicatorConfig.originY;
		splashSprite.Begin();
		for (ushort num = 0; num < numHitIndicators; num++)
		{
			if (hitIndicators[num].curTime > 0f)
			{
				color.R = (byte)(hitIndicators[num].curTime / hitIndicatorConfig.starTime * 255f);
				color.G = color.R;
				color.B = color.R;
				color.A = color.R;
				hitIndicators[num].curTime -= global::MainGame.MainGame.frametime;
				destinationRectangle.X = mainC.curGame.halfWidth - (int)(Math.Sin(hitIndicators[num].rotation) * (double)(int)hitIndicatorConfig.radius);
				destinationRectangle.Y = mainC.curGame.halfHeight - (int)(Math.Cos(hitIndicators[num].rotation) * (double)(int)hitIndicatorConfig.radius);
				splashSprite.Draw(global::Textures.Textures.texMain.texData[hitIndicatorTextureIDs[num]], destinationRectangle, null, color, 0f - hitIndicators[num].rotation, origin, SpriteEffects.None, 0f);
			}
		}
		splashSprite.End();
	}

	public void Render_Model_Ortho(ushort modID, float x, float y, float scale, float alphaValue, ushort texID)
	{
		mainC.fontmoduleMain.Reset_Graphics_Adapter_After_SpriteBatch();
		effect1.CurrentTechnique = effect1.Techniques["Basic"];
		effect1.Parameters["ViewProjection"].SetValue(matrixO);
		effect1.Parameters["AlphaAdjust"].SetValue(alphaValue);
		rGraphics.DepthStencilState = depthBufferDisabled;
		rGraphics.SetVertexBuffer(global::Models.Models.mVertexBufferObjects);
		rGraphics.Indices = global::Models.Models.mIndexBufferObjects;
		rGraphics.BlendState = blendSourceAlpha;
		Matrix value = Matrix.CreateScale(scale, scale, 1f) * Matrix.CreateTranslation(x, y, -5000f);
		effect1.Parameters["World"].SetValue(value);
		effect1.Parameters["BaseTexture"].SetValue(global::Textures.Textures.texMain.texData[texID]);
		effect1.CurrentTechnique.Passes[0].Apply();
		mainC.modelsMain.Render_Model_Basic(modID);
	}

	public void Render_Taunt_Message()
	{
		splashSprite.Draw(global::Textures.Textures.texMain.texData[global::Textures.Textures.texTauntMessage], tauntPos, cWhite);
	}

	public void Render_Swap_Weapon_Message()
	{
		splashSprite.Draw(global::Textures.Textures.texMain.texData[global::Textures.Textures.texSwapWeapon], swapWeaponPos, cWhite);
	}

	public void Render_Damage_Bar_2D(float damage, ushort x, ushort y, ushort width, ushort height, byte border)
	{
		Rectangle destinationRectangle = new Rectangle
		{
			X = x,
			Y = y,
			Width = width,
			Height = height
		};
		barColor.B = 0;
		if (damage <= 0.25f)
		{
			barColor.R = 0;
			barColor.G = byte.MaxValue;
		}
		else if (damage <= 0.75f)
		{
			barColor.G = byte.MaxValue;
			barColor.R = byte.MaxValue;
		}
		else
		{
			barColor.R = byte.MaxValue;
			barColor.G = 0;
		}
		splashSprite.Begin();
		splashSprite.Draw(global::Textures.Textures.texMain.texData[global::Textures.Textures.texWhite], destinationRectangle, Color.Black);
		destinationRectangle.X += border;
		destinationRectangle.Y += border;
		border *= 2;
		destinationRectangle.Width = (int)((1f - damage) * (float)(destinationRectangle.Width - border));
		destinationRectangle.Height -= border;
		splashSprite.Draw(global::Textures.Textures.texMain.texData[global::Textures.Textures.texWhite], destinationRectangle, barColor);
		splashSprite.End();
	}

	public void Render_Bar_2D(ushort x, ushort y, byte R, byte G, byte B, float length, ushort width, ushort height, byte border)
	{
		Rectangle destinationRectangle = new Rectangle
		{
			X = x,
			Y = y,
			Width = width,
			Height = height
		};
		barColor.R = R;
		barColor.G = G;
		barColor.B = B;
		barColor.A = byte.MaxValue;
		splashSprite.Begin();
		splashSprite.Draw(global::Textures.Textures.texMain.texData[global::Textures.Textures.texWhite], destinationRectangle, Color.Black);
		destinationRectangle.X += border;
		destinationRectangle.Y += border;
		border *= 2;
		destinationRectangle.Width = (int)(length * (float)(destinationRectangle.Width - border));
		destinationRectangle.Height -= border;
		splashSprite.Draw(global::Textures.Textures.texMain.texData[global::Textures.Textures.texWhite], destinationRectangle, barColor);
		splashSprite.End();
	}

	public void Render_Damage_Bar_3D_Setup()
	{
		far4[2] = 0f;
		far4[3] = 1f;
		mainC.fontmoduleMain.Reset_Graphics_Adapter_After_SpriteBatch();
		effect1.Parameters["ViewProjection"].SetValue(matrixVP);
		effect1.Parameters["View"].SetValue(matrixV);
		effect1.Parameters["Projection"].SetValue(matrixP);
		effect1.CurrentTechnique = effect1.Techniques["Billboards"];
		rGraphics.BlendState = BlendState.Opaque;
		rGraphics.SetVertexBuffer(global::Models.Models.mVertexBufferObjects);
		rGraphics.Indices = global::Models.Models.mIndexBufferObjects;
	}

	public void Render_Damage_Bar_3D(float damage, float x, float y, float z, ushort width, ushort height, ushort frameHeight)
	{
		rgtV2.X = x;
		rgtV2.Y = y;
		rgtV2.Z = z;
		rgtV2 = rGraphics.Viewport.Project(rgtV2, matrixP, matrixV, matrixI);
		if (rgtV2.Z < 1f)
		{
			if (damage <= 0.25f)
			{
				far4[0] = 0f;
				far4[1] = 1f;
			}
			else if (damage <= 0.5f)
			{
				far4[0] = 1f;
				far4[1] = 1f;
			}
			else
			{
				far4[0] = 1f;
				far4[1] = 0f;
			}
			effect1.Parameters["ColorAdjust"].SetValue(far4);
			float y2 = rgtV2.Y;
			rgtV2.X = x + matrixVInverse.M21 * (float)(int)height;
			rgtV2.Y = y + matrixVInverse.M22 * (float)(int)height;
			rgtV2.Z = z + matrixVInverse.M23 * (float)(int)height;
			rgtV2 = rGraphics.Viewport.Project(rgtV2, matrixP, matrixV, matrixI);
			y2 = Math.Abs(y2 - rgtV2.Y);
			y2 = Math.Abs((float)(int)height / y2);
			float value = (float)(int)width * (1f - damage) * y2;
			effect1.Parameters["offset"].SetValue((0f - (float)(int)width * damage * y2) / 2f);
			effect1.Parameters["width"].SetValue(value);
			effect1.Parameters["height"].SetValue((float)(int)height * y2);
			Matrix value2 = Matrix.CreateTranslation(x, y, z + y2 * ((float)(int)frameHeight / 2f));
			effect1.Parameters["World"].SetValue(value2);
			effect1.CurrentTechnique.Passes[0].Apply();
			mainC.modelsMain.Render_Model_Basic(global::Models.Models.modBillboard);
		}
	}

	public void Render_Damage_Bar_Frame_3D(float x, float y, float z, ushort width, ushort height)
	{
		rgtV2.X = x;
		rgtV2.Y = y;
		rgtV2.Z = z;
		rgtV2 = rGraphics.Viewport.Project(rgtV2, matrixP, matrixV, matrixI);
		if (rgtV2.Z < 1f)
		{
			float y2 = rgtV2.Y;
			rgtV2.X = x + matrixVInverse.M21 * (float)(int)height;
			rgtV2.Y = y + matrixVInverse.M22 * (float)(int)height;
			rgtV2.Z = z + matrixVInverse.M23 * (float)(int)height;
			rgtV2 = rGraphics.Viewport.Project(rgtV2, matrixP, matrixV, matrixI);
			y2 = Math.Abs(y2 - rgtV2.Y);
			y2 = Math.Abs((float)(int)height / y2);
			effect1.Parameters["width"].SetValue((float)(int)width * y2);
			effect1.Parameters["height"].SetValue((float)(int)height * y2);
			Matrix value = Matrix.CreateTranslation(x, y, z + y2 * ((float)(int)height / 2f));
			effect1.Parameters["World"].SetValue(value);
			effect1.CurrentTechnique.Passes[0].Apply();
			mainC.modelsMain.Render_Model_Basic(global::Models.Models.modBillboard);
		}
	}

	public void Render_Damage_Bar_3D_Cleanup()
	{
		far4[0] = 1f;
		far4[1] = 1f;
		far4[2] = 1f;
		far4[3] = 1f;
		effect1.Parameters["ColorAdjust"].SetValue(far4);
		effect1.Parameters["ViewProjection"].SetValue(matrixVP);
		effect1.Parameters["offset"].SetValue(0f);
		effect1.CurrentTechnique = effect1.Techniques["Main"];
	}

	public void Render_Weapon_Select(float frameTime)
	{
		mainC.gameLogic.Game_UI_Render_Weapon_Select(frameTime);
	}

	public void Render_Vehicle_Select(float frameTime)
	{
		Vector2 position = default(Vector2);
		ushort curVehicle = global::Players.Players.players[0].curVehicle;
		position.X = 148f;
		position.Y = 89f;
		short num = 69;
		splashSprite.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
		short num2;
		for (num2 = 0; num2 < Vehicles.numVehicles; num2++)
		{
			if (Vehicles.vehicelSelectVehicleIDs[num2] == curVehicle)
			{
				splashSprite.Draw(global::Textures.Textures.texMain.texData[texVehicleSelectFrame], position, Color.White);
			}
			position.X += num;
		}
		if (global::Players.Players.currentPlayerRank < Vehicles.lockedVehicleLevels[User_Interface.curVehicleSelect])
		{
			position.X = 159f;
			position.Y = 510f;
			splashSprite.Draw(global::Textures.Textures.texMain.texData[texVehicleSelectLocked], position, Color.White);
		}
		splashSprite.End();
		mainC.fontmoduleMain.Reset_Graphics_Adapter_After_SpriteBatch();
		if (User_Interface.hideVehicle)
		{
			rGraphics.DepthStencilState = depthBufferDisabled;
			rGraphics.SetVertexBuffer(global::Models.Models.mVertexBufferObjects);
			rGraphics.Indices = global::Models.Models.mIndexBufferObjects;
			rGraphics.BlendState = blendSourceAlpha;
			return;
		}
		cosineFluctuation3 += frameTime / global::Physics.Physics.timeMod;
		if (cosineFluctuation3 > 4f)
		{
			cosineFluctuation3 -= 4f;
		}
		Matrix mv = vehicleSelectMatrix * Matrix.CreateRotationZ(cosineFluctuation3 / 4f * 360f * ((float)Math.PI / 180f)) * Matrix.CreateRotationX(-(float)Math.PI / 2f) * Matrix.CreateTranslation(-313f, -24f, -1200f);
		far4[0] = 0.25f;
		far4[1] = 0.25f;
		far4[2] = 0.25f;
		far4[3] = 1f;
		effect1.Parameters["Ambient"].SetValue(far4);
		far3[0] = 0f;
		far3[1] = 1f;
		far3[2] = 0f;
		effect1.Parameters["dLightDir"].SetValue(far3);
		far4[0] = 1f;
		far4[1] = 1f;
		far4[2] = 1f;
		effect1.Parameters["dLightColor"].SetValue(far4);
		far4[0] = 0f;
		far4[1] = 0.5f;
		far4[2] = 0.25f;
		far4[3] = 1000f;
		effect1.Parameters["dLightBounce"].SetValue(far4);
		effect1.Parameters["World"].SetValue(mv);
		effect1.Parameters["MaterialTexture"].SetValue(global::Textures.Textures.texMain.texData[global::Textures.Textures.texDefaultNormalMap]);
		effect1.Parameters["Specular"].SetValue(specular);
		effect1.CurrentTechnique.Passes[0].Apply();
		rGraphics.BlendState = BlendState.Opaque;
		rGraphics.SetVertexBuffer(global::Models.Models.mVertexBufferObjects);
		rGraphics.Indices = global::Models.Models.mIndexBufferObjects;
		rGraphics.DepthStencilState = depthBufferEnabled;
		num = global::Joints.Joints.playerJoints[global::Players.Players.players[0].jointPackage].numJoints;
		for (num2 = 0; num2 < num; num2++)
		{
			ref Matrix reference = ref effectMatrix[num2];
			reference = global::Players.Players.players[0].jt1[num2].mv[rBufferID] * mv;
		}
		effect1.Parameters["Matrix"].SetValue(effectMatrix);
		num2 = 0;
		num = Vehicles.vehicles[curVehicle].numModels;
		while (num2 < num)
		{
			mainC.modelsMain.Render_Player_Rigged_Model(global::Players.Players.players[0].playerModel[num2], global::Players.Players.players[0].textureNormalID, global::Players.Players.players[0].textureSpecularID, byte.MaxValue);
			num2++;
		}
		mainC.vehicles.Set_Avatar_Matrix_For_Vehicle_Select(0, ref mv);
		mainC.avatarMain.Render_Vehicle_Select_Avatar(0, uBufferID);
		effect1.Parameters["Ambient"].SetValue(ambientLevel);
		effect1.Parameters["dLightDir"].SetValue(directionalLightVector);
		effect1.Parameters["dLightColor"].SetValue(directionalLightColor);
		effect1.Parameters["dLightBounce"].SetValue(directionalLightBounce);
		rGraphics.DepthStencilState = depthBufferDisabled;
		rGraphics.SetVertexBuffer(global::Models.Models.mVertexBufferObjects);
		rGraphics.Indices = global::Models.Models.mIndexBufferObjects;
		rGraphics.BlendState = blendSourceAlpha;
	}

	public void Render_Rotating_Model_Onscreen(byte bufferType, ushort modID, float tilt, float yOffset, float speed, float modelScale, float frameTime)
	{
		cosineFluctuationModelView += frameTime;
		if (cosineFluctuationModelView > speed)
		{
			cosineFluctuationModelView -= speed;
		}
		effect1.Parameters["ViewProjection"].SetValue(matrixO);
		Matrix mv = Matrix.CreateScale(modelScale) * Matrix.CreateTranslation(0f, yOffset, 0f) * Matrix.CreateRotationX(tilt * ((float)Math.PI / 180f)) * Matrix.CreateRotationZ(cosineFluctuationModelView / speed * 360f * ((float)Math.PI / 180f)) * Matrix.CreateRotationX(-(float)Math.PI / 2f) * Matrix.CreateTranslation(0f, 0f, -5000f);
		if (bufferType == 0)
		{
			mainC.modelsMain.Render_Model(modID, ref mv);
		}
		else if (global::Models.Models.modVbo[modID].inLevelVBO)
		{
			mainC.modelsMain.Render_Level_Model(modID, ref mv);
		}
	}

	public void Render_Model_Onscreen(byte bufferType, ushort modID, float roll, float tilt, float yOffset, float zRotation, float modelScale)
	{
		if (modID >= global::Models.Models.numModels)
		{
			return;
		}
		effect1.Parameters["ViewProjection"].SetValue(matrixO);
		Matrix matrix = Matrix.CreateScale(modelScale) * Matrix.CreateTranslation(0f, yOffset, 0f) * Matrix.CreateRotationY(roll * ((float)Math.PI / 180f)) * Matrix.CreateRotationX(tilt * ((float)Math.PI / 180f)) * Matrix.CreateRotationZ(zRotation * ((float)Math.PI / 180f)) * Matrix.CreateRotationX(-(float)Math.PI / 2f) * Matrix.CreateTranslation(0f, 0f, -5000f);
		matrix = Matrix.CreateScale(modelScale) * Matrix.CreateRotationX(-(float)Math.PI / 2f) * Matrix.CreateTranslation(0f, 0f, -5000f);
		if (bufferType == 0)
		{
			if (global::Models.Models.mod1[modID].usesRigging != 1)
			{
				mainC.modelsMain.Render_Model(modID, ref matrix);
				return;
			}
			for (ushort num = 0; num < 58; num++)
			{
				effectMatrix[num] = matrix;
			}
			effect1.Parameters["Matrix"].SetValue(effectMatrix);
			mainC.modelsMain.Render_Player_Rigged_Model(modID, global::Players.Players.players[0].textureNormalID, global::Players.Players.players[0].textureSpecularID, byte.MaxValue);
		}
		else if (global::Models.Models.modVbo[modID].inLevelVBO)
		{
			mainC.modelsMain.Render_Level_Model(modID, ref matrix);
		}
	}

	public void Render_Graph(ushort graphID)
	{
	}

	private void From_Render()
	{
	}

	public void Render_Sky_Box()
	{
		if (skySideLength != 0f)
		{
			effect1.CurrentTechnique = effect1.Techniques["Basic"];
			rGraphics.SamplerStates[0] = textureSamplerStateClamp;
			Matrix matrix = Matrix.CreateRotationX((float)Math.PI / 2f) * Matrix.CreateTranslation(global::Players.Players.players[0].posX[rBufferID], global::Players.Players.players[0].posY[rBufferID], global::Players.Players.players[0].posZ[rBufferID] + skyHeighAdjustment);
			Matrix matrix2 = Matrix.CreateScale(skySideLength, skySideLength, 1f) * Matrix.CreateTranslation(0f, 0f, negativeSkySideHalfLength);
			Matrix value = matrix2 * matrix;
			effect1.Parameters["World"].SetValue(value);
			mainC.modelsMain.Render_Model_List_Item_Basic(0, 0);
			value = matrix2 * Matrix.CreateRotationY(-(float)Math.PI / 2f) * matrix;
			effect1.Parameters["World"].SetValue(value);
			mainC.modelsMain.Render_Model_List_Item_Basic(0, 1);
			value = matrix2 * Matrix.CreateRotationY(-(float)Math.PI) * matrix;
			effect1.Parameters["World"].SetValue(value);
			mainC.modelsMain.Render_Model_List_Item_Basic(0, 2);
			value = matrix2 * Matrix.CreateRotationY((float)Math.PI / 2f) * matrix;
			effect1.Parameters["World"].SetValue(value);
			mainC.modelsMain.Render_Model_List_Item_Basic(0, 3);
			value = matrix2 * Matrix.CreateRotationX((float)Math.PI / 2f) * matrix;
			effect1.Parameters["World"].SetValue(value);
			mainC.modelsMain.Render_Model_List_Item_Basic(0, 4);
			effect1.CurrentTechnique = effect1.Techniques["Main"];
			rGraphics.SamplerStates[0] = textureSamplerState;
		}
	}

	public void Render_Sky_Dome()
	{
		effect1.CurrentTechnique = effect1.Techniques["Basic"];
		Matrix value = Matrix.CreateScale(skySideLength, skySideLength, skySideLength) * Matrix.CreateTranslation(global::Players.Players.players[0].posX[rBufferID], global::Players.Players.players[0].posY[rBufferID], global::Players.Players.players[0].posZ[rBufferID] + skyHeighAdjustment);
		effect1.Parameters["World"].SetValue(value);
		effect1.Parameters["BaseTexture"].SetValue(global::Textures.Textures.texMain.texData[global::Models.Models.modVbo[global::Models.Models.modSkyDome].texID]);
		effect1.Parameters["MaterialTexture"].SetValue(global::Textures.Textures.texMain.texData[global::Models.Models.modVbo[global::Models.Models.modSkyDome].texNormalID]);
		rGraphics.BlendState = BlendState.Opaque;
		effect1.CurrentTechnique.Passes[0].Apply();
		rGraphics.SetVertexBuffer(global::Models.Models.mVertexBufferObjectsLevel);
		rGraphics.Indices = global::Models.Models.mIndexBufferObjectsLevel;
		mainC.modelsMain.Render_Level_Model_Basic(global::Models.Models.modSkyDome);
		effect1.CurrentTechnique = effect1.Techniques["Main"];
	}

	public void Render_Space()
	{
		if (skySideLength != 0f)
		{
			effect1.CurrentTechnique = effect1.Techniques["Basic"];
			rGraphics.SamplerStates[0] = textureSamplerStateClamp;
			Matrix matrix = Matrix.CreateTranslation(global::Players.Players.players[0].posX[rBufferID], global::Players.Players.players[0].posY[rBufferID], global::Players.Players.players[0].posZ[rBufferID]);
			Matrix matrix2 = Matrix.CreateScale(skySideLength, skySideLength, 1f) * Matrix.CreateTranslation(0f, 0f, negativeSkySideHalfLength);
			effect1.Parameters["World"].SetValue(matrix2 * matrix);
			mainC.modelsMain.Render_Model_List_Item_Basic(0, 5);
			matrix = Matrix.CreateRotationX((float)Math.PI / 2f) * matrix;
			Matrix value = matrix2 * matrix;
			effect1.Parameters["World"].SetValue(value);
			mainC.modelsMain.Render_Model_List_Item_Basic(0, 0);
			value = matrix2 * Matrix.CreateRotationY(-(float)Math.PI / 2f) * matrix;
			effect1.Parameters["World"].SetValue(value);
			mainC.modelsMain.Render_Model_List_Item_Basic(0, 1);
			value = matrix2 * Matrix.CreateRotationY(-(float)Math.PI) * matrix;
			effect1.Parameters["World"].SetValue(value);
			mainC.modelsMain.Render_Model_List_Item_Basic(0, 2);
			value = matrix2 * Matrix.CreateRotationY((float)Math.PI / 2f) * matrix;
			effect1.Parameters["World"].SetValue(value);
			mainC.modelsMain.Render_Model_List_Item_Basic(0, 3);
			value = matrix2 * Matrix.CreateRotationX((float)Math.PI / 2f) * matrix;
			effect1.Parameters["World"].SetValue(value);
			mainC.modelsMain.Render_Model_List_Item_Basic(0, 4);
			effect1.CurrentTechnique = effect1.Techniques["Main"];
			rGraphics.SamplerStates[0] = textureSamplerState;
		}
	}

	public void Render_Scope()
	{
		rGraphics.BlendState = blendSourceAlpha;
		effect1.CurrentTechnique = effect1.Techniques["WeaponScope"];
		effect1.Parameters["ViewProjection"].SetValue(matrixO);
		effect1.Parameters["BaseTexture"].SetValue(global::Textures.Textures.texMain.texData[global::Textures.Textures.texScope]);
		effect1.CurrentTechnique.Passes[0].Apply();
		rGraphics.DrawUserIndexedPrimitives(PrimitiveType.TriangleList, ScopeVtex, 0, 4, vecQuadInd, 0, 2, rDecVPCNT);
		effect1.CurrentTechnique = effect1.Techniques["Main"];
		effect1.CurrentTechnique.Passes[0].Apply();
	}

	public void Render_Text_At_Coords(string inputStr, float x, float y)
	{
		rgtV1 = global::FontModule.FontModule.GameFonts[1].MeasureString(inputStr);
		float num = rgtV1.X / 2f;
		float num2 = rgtV1.Y / 2f;
		rgtV2 = rGraphics.Viewport.Project(new Vector3(x, y, 0.15f), matrixP, matrixV, matrixI);
		rgtV2.Z = 0.15f;
		rgtV1.X = rgtV2.X - num;
		rgtV1.Y = rgtV2.Y - num2;
		global::FontModule.FontModule.FontSprite1.Begin();
		global::FontModule.FontModule.FontSprite1.DrawString(global::FontModule.FontModule.GameFonts[2], inputStr, rgtV1, Color.White);
		global::FontModule.FontModule.FontSprite1.End();
	}

	public void Render_GamerTags_HumanPlayers_Player_Team_Only()
	{
		for (int i = 1; i < global::MainGame.MainGame.maxHumanGamePlayers; i++)
		{
			if (global::Players.Players.players[i].active && (global::Players.Players.players[i].teamMask & global::Players.Players.enemyTeamMask) == 0 && (global::Players.Players.players[i].onmap & renderGamerTagMask) > 0)
			{
				rgtV1 = global::FontModule.FontModule.GameFonts[2].MeasureString(global::Players.Players.players[i].abreviateName);
				float num = rgtV1.X / 2f;
				rgtV2.X = global::Players.Players.players[i].posX[rBufferID];
				rgtV2.Y = global::Players.Players.players[i].posY[rBufferID];
				rgtV2.Z = global::Players.Players.players[i].posZ[rBufferID] + global::Players.Players.playerRaces[global::Players.Players.players[i].race].gamerTagHeight[global::Players.Players.players[i].type];
				rgtV2 = rGraphics.Viewport.Project(rgtV2, matrixP, matrixV, matrixI);
				rgtV1.X = rgtV2.X - num;
				rgtV1.Y = rgtV2.Y - (float)(global::FontModule.FontModule.GameFonts[2].LineSpacing * 2);
				if (rgtV2.Z < 1f)
				{
					global::FontModule.FontModule.FontSprite1.Begin();
					global::FontModule.FontModule.FontSprite1.DrawString(global::FontModule.FontModule.GameFonts[gamerTagFont], global::Players.Players.players[i].abreviateName, rgtV1, Color.White);
					global::FontModule.FontModule.FontSprite1.End();
				}
			}
		}
		mainC.fontmoduleMain.Reset_Graphics_Adapter_After_SpriteBatch();
	}

	public void Render_GamerTags_HumanPlayers_All()
	{
		for (int i = 1; i < global::MainGame.MainGame.maxHumanGamePlayers; i++)
		{
			if (!global::Players.Players.players[i].active || (global::Players.Players.players[i].onmap & renderGamerTagMask) <= 0)
			{
				continue;
			}
			rgtV1 = global::FontModule.FontModule.GameFonts[2].MeasureString(global::Players.Players.players[i].abreviateName);
			float num = rgtV1.X / 2f;
			rgtV2.X = global::Players.Players.players[i].posX[rBufferID];
			rgtV2.Y = global::Players.Players.players[i].posY[rBufferID];
			rgtV2.Z = global::Players.Players.players[i].posZ[rBufferID] + global::Players.Players.playerRaces[global::Players.Players.players[i].race].gamerTagHeight[global::Players.Players.players[i].type];
			rgtV2 = rGraphics.Viewport.Project(rgtV2, matrixP, matrixV, matrixI);
			rgtV1.X = rgtV2.X - num;
			rgtV1.Y = rgtV2.Y - (float)(global::FontModule.FontModule.GameFonts[2].LineSpacing * 2);
			if (rgtV2.Z < 1f)
			{
				global::FontModule.FontModule.FontSprite1.Begin();
				if ((global::Players.Players.players[i].teamMask & global::Players.Players.enemyTeamMask) == 0)
				{
					global::FontModule.FontModule.FontSprite1.DrawString(global::FontModule.FontModule.GameFonts[gamerTagFont], global::Players.Players.players[i].abreviateName, rgtV1, Color.White);
				}
				else
				{
					global::FontModule.FontModule.FontSprite1.DrawString(global::FontModule.FontModule.GameFonts[gamerTagFont], global::Players.Players.players[i].abreviateName, rgtV1, Color.Red);
				}
				global::FontModule.FontModule.FontSprite1.End();
			}
		}
		mainC.fontmoduleMain.Reset_Graphics_Adapter_After_SpriteBatch();
	}

	public void Render_GamerTags_All_Players()
	{
		for (int i = 1; i < global::MainGame.MainGame.maxGamePlayers; i++)
		{
			if (!global::Players.Players.players[i].active || (global::Players.Players.players[i].onmap & renderGamerTagMask) <= 0)
			{
				continue;
			}
			rgtV1 = global::FontModule.FontModule.GameFonts[2].MeasureString(global::Players.Players.players[i].abreviateName);
			float num = rgtV1.X / 2f;
			rgtV2.X = global::Players.Players.players[i].posX[rBufferID];
			rgtV2.Y = global::Players.Players.players[i].posY[rBufferID];
			rgtV2.Z = global::Players.Players.players[i].posZ[rBufferID] + global::Players.Players.playerRaces[global::Players.Players.players[i].race].gamerTagHeight[global::Players.Players.players[i].type];
			rgtV2 = rGraphics.Viewport.Project(rgtV2, matrixP, matrixV, matrixI);
			rgtV1.X = rgtV2.X - num;
			rgtV1.Y = rgtV2.Y - (float)(global::FontModule.FontModule.GameFonts[2].LineSpacing * 2);
			if (rgtV2.Z < 1f)
			{
				global::FontModule.FontModule.FontSprite1.Begin();
				if ((global::Players.Players.players[i].teamMask & global::Players.Players.enemyTeamMask) == 0)
				{
					global::FontModule.FontModule.FontSprite1.DrawString(global::FontModule.FontModule.GameFonts[gamerTagFont], global::Players.Players.players[i].abreviateName, rgtV1, Color.White);
				}
				else
				{
					global::FontModule.FontModule.FontSprite1.DrawString(global::FontModule.FontModule.GameFonts[gamerTagFont], global::Players.Players.players[i].abreviateName, rgtV1, Color.Red);
				}
				global::FontModule.FontModule.FontSprite1.End();
			}
		}
		mainC.fontmoduleMain.Reset_Graphics_Adapter_After_SpriteBatch();
	}

	public void Render_Crosshair_Static()
	{
		rsPos.X = mainC.curGame.halfWidth - (int)((float)global::Textures.Textures.texMain.texData[global::Textures.Textures.texCrossHair[0]].Width / 2f);
		rsPos.Y = mainC.curGame.halfHeight - (int)((float)global::Textures.Textures.texMain.texData[global::Textures.Textures.texCrossHair[0]].Height / 2f);
		switch (global::Players.Players.currentView)
		{
		case 7:
			rsPos.X = satelliteCrossHairX[rBufferID];
			rsPos.Y = satelliteCrossHairY[rBufferID];
			break;
		}
		splashSprite.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
		if (global::Weapons.Weapons.showTargetCrosshairTimer > 0f)
		{
			splashSprite.Draw(global::Textures.Textures.texMain.texData[global::Textures.Textures.texCrossHair[2]], rsPos, Color.White);
		}
		else if (global::Players.Players.targetingEnemy)
		{
			splashSprite.Draw(global::Textures.Textures.texMain.texData[global::Textures.Textures.texCrossHair[1]], rsPos, Color.White);
		}
		else
		{
			splashSprite.Draw(global::Textures.Textures.texMain.texData[global::Textures.Textures.texCrossHair[0]], rsPos, Color.White);
		}
		splashSprite.End();
	}

	public void Render_Crosshair_FPS()
	{
	}

	public void Render_Crosshair_Vehicle()
	{
		if ((global::Players.Players.players[0].onmap == 4 && !global::Players.Players.players[0].dead) || global::MainGame.MainGame.isCommander)
		{
			far4[0] = 1f;
			far4[1] = 1f;
			far4[2] = 1f;
			far4[3] = 1f;
			rgtV2.X = crossHairPositionGoal[rBufferID, 0];
			rgtV2.Y = crossHairPositionGoal[rBufferID, 1];
			rgtV2.Z = crossHairPositionGoal[rBufferID, 2];
			rgtV2 = rGraphics.Viewport.Project(rgtV2, matrixP, matrixV, matrixI);
			float value = rgtV2.X - crossHairPosition[0];
			float value2 = rgtV2.Y - crossHairPosition[1];
			float num = crossHairMovementSpeed * global::MainGame.MainGame.frametime;
			if (Math.Abs(value) < num || Math.Abs(value) > 125f)
			{
				crossHairPosition[0] = rgtV2.X;
			}
			else
			{
				crossHairPosition[0] += num * (float)Math.Sign(value);
			}
			if (Math.Abs(value2) < num || Math.Abs(value2) > 125f)
			{
				crossHairPosition[1] = rgtV2.Y;
			}
			else
			{
				crossHairPosition[1] += num * (float)Math.Sign(value2);
			}
			Matrix value3 = Matrix.CreateScale(100f, 100f, 1f);
			value3.M41 = crossHairPosition[0] - (float)mainC.curGame.halfWidth;
			value3.M42 = (float)mainC.curGame.halfHeight - crossHairPosition[1];
			value3.M43 = -3f;
			rGraphics.DepthStencilState = depthBufferDisabled;
			rGraphics.BlendState = blendSourceAlpha;
			rGraphics.SetVertexBuffer(global::Models.Models.mVertexBufferObjects);
			rGraphics.Indices = global::Models.Models.mIndexBufferObjects;
			effect1.Parameters["ViewProjection"].SetValue(matrixO);
			if (global::Weapons.Weapons.showTargetCrosshairTimer <= 0f)
			{
				effect1.Parameters["BaseTexture"].SetValue(global::Textures.Textures.texMain.texData[global::Textures.Textures.texCrossHair[0]]);
			}
			else
			{
				far4[1] = 0f;
				far4[2] = 0f;
				effect1.Parameters["BaseTexture"].SetValue(global::Textures.Textures.texMain.texData[global::Textures.Textures.texCrossHair[2]]);
			}
			effect1.Parameters["World"].SetValue(value3);
			effect1.CurrentTechnique = effect1.Techniques["Basic"];
			effect1.CurrentTechnique.Passes[0].Apply();
			effect1.Parameters["ColorAdjust"].SetValue(far4);
			effect1.CurrentTechnique.Passes[0].Apply();
			mainC.modelsMain.Render_Model_Basic(global::Models.Models.modFlatPlane);
			far4[1] = 1f;
			far4[2] = 1f;
			effect1.Parameters["ColorAdjust"].SetValue(far4);
			effect1.CurrentTechnique = effect1.Techniques["Main"];
			effect1.Parameters["ViewProjection"].SetValue(matrixVP);
			effect1.CurrentTechnique.Passes[0].Apply();
			rGraphics.DepthStencilState = depthBufferEnabled;
		}
	}

	public void Render_Commander_Transporter_Bar()
	{
		Matrix identity = Matrix.Identity;
		rgtV2.X = global::Weapons.Weapons.laserPosX[rBufferID];
		rgtV2.Y = global::Weapons.Weapons.laserPosY[rBufferID];
		rgtV2.Z = global::Weapons.Weapons.laserPosZ[rBufferID];
		rgtV2 = rGraphics.Viewport.Project(rgtV2, matrixP, matrixV, matrixI);
		identity.M41 = rgtV2.X - (float)mainC.curGame.halfWidth;
		identity.M42 = (float)mainC.curGame.halfHeight - rgtV2.Y;
		identity.M43 = -1f;
		rGraphics.BlendState = BlendState.Opaque;
		effect1.CurrentTechnique = effect1.Techniques["Main"];
		effect1.CurrentTechnique.Passes[0].Apply();
		effect1.Parameters["ViewProjection"].SetValue(matrixO);
		effect1.Parameters["World"].SetValue(matrixI);
		effect1.Parameters["BaseTexture"].SetValue(global::Textures.Textures.texMain.texData[global::Models.Models.mod1[global::Models.Models.modCHud].texID]);
		effect1.Parameters["MaterialTexture"].SetValue(global::Textures.Textures.texMain.texData[global::Models.Models.mod1[global::Models.Models.modCHud].texNormalID]);
		far4[0] = 0f;
		far4[1] = 0f;
		far4[2] = 0f;
		far4[3] = 1f;
		effect1.Parameters["ColorAdjust"].SetValue(far4);
		Matrix matrix = Matrix.CreateScale(1.1f, 1.1f, 1f) * identity;
		matrix = Matrix.CreateTranslation(-36.5f, 64.25f, 0f) * matrix;
		mainC.modelsMain.Render_Model_Basic_With_Matrix(global::Models.Models.modCHudBar, ref matrix);
		if (global::MainGame.MainGame.commanderTeleportReadyToDeploy)
		{
			far4[0] = 0f;
			far4[1] = 0.66f;
			far4[2] = 0.259f;
		}
		else
		{
			far4[0] = 0.663f;
			far4[1] = 0.125f;
			far4[2] = 0.02f;
		}
		far4[3] = 1f;
		effect1.Parameters["ColorAdjust"].SetValue(far4);
		matrix = Matrix.CreateScale(commanderTeleporterVal, 1f, 1f) * identity;
		matrix = Matrix.CreateTranslation(-36.5f, 64.25f, 0f) * matrix;
		mainC.modelsMain.Render_Model_Basic_With_Matrix(global::Models.Models.modCHudBar, ref matrix);
		effect1.Parameters["ViewProjection"].SetValue(matrixVP);
		far4[0] = 1f;
		far4[1] = 1f;
		far4[2] = 1f;
		far4[3] = 1f;
		effect1.Parameters["ColorAdjust"].SetValue(far4);
	}

	public void Render_Commander_Transporter_Energy_Bar()
	{
		Matrix matrix = matrixI;
		matrix.M41 = 0f - (float)mainC.curGame.safeWidth / 2f + 72f;
		matrix.M42 = 0f - (float)mainC.curGame.safeHeight / 2f + 54f;
		matrix.M43 = -1f;
		rGraphics.BlendState = BlendState.Opaque;
		effect1.CurrentTechnique = effect1.Techniques["Main"];
		effect1.CurrentTechnique.Passes[0].Apply();
		effect1.Parameters["ViewProjection"].SetValue(matrixO);
		effect1.Parameters["World"].SetValue(matrixI);
		effect1.Parameters["BaseTexture"].SetValue(global::Textures.Textures.texMain.texData[global::Models.Models.mod1[global::Models.Models.modCHud].texID]);
		effect1.Parameters["MaterialTexture"].SetValue(global::Textures.Textures.texMain.texData[global::Models.Models.mod1[global::Models.Models.modCHud].texNormalID]);
		far4[0] = 0f;
		far4[1] = 0f;
		far4[2] = 0f;
		far4[3] = 1f;
		effect1.Parameters["ColorAdjust"].SetValue(far4);
		Matrix matrix2 = Matrix.CreateScale(1.1f, 1.1f, 1f) * matrix;
		matrix2 = Matrix.CreateTranslation(-36.5f, 64.25f, 0f) * matrix2;
		mainC.modelsMain.Render_Model_Basic_With_Matrix(global::Models.Models.modCHudBar, ref matrix2);
		far4[0] = 0.871f;
		far4[1] = 0.898f;
		far4[2] = 0.024f;
		far4[3] = 1f;
		effect1.Parameters["ColorAdjust"].SetValue(far4);
		matrix2 = Matrix.CreateScale(commanderTeleporterEnergyVal, 1f, 1f) * matrix;
		matrix2 = Matrix.CreateTranslation(-36.5f, 64.25f, 0f) * matrix2;
		mainC.modelsMain.Render_Model_Basic_With_Matrix(global::Models.Models.modCHudBar, ref matrix2);
		effect1.Parameters["ViewProjection"].SetValue(matrixVP);
		far4[0] = 1f;
		far4[1] = 1f;
		far4[2] = 1f;
		far4[3] = 1f;
		effect1.Parameters["ColorAdjust"].SetValue(far4);
	}

	public void Draw_Rotated_Square(float x1, float y1, float z1, float x2, float y2, float z2, ref Matrix mv, Color c1)
	{
		float[] array = new float[7];
		float[] array2 = new float[7];
		float[] array3 = new float[7];
		StructsClass.VertexPositionColorNormalTexture[] array4 = new StructsClass.VertexPositionColorNormalTexture[3]
		{
			default(StructsClass.VertexPositionColorNormalTexture),
			default(StructsClass.VertexPositionColorNormalTexture),
			default(StructsClass.VertexPositionColorNormalTexture)
		};
		array[0] = x2 * mv.M11 + x1;
		array2[0] = x2 * mv.M12 + y1;
		array3[0] = x2 * mv.M13 + z1;
		array[1] = x2 * mv.M11 + z2 * mv.M31 + x1;
		array2[1] = x2 * mv.M12 + z2 * mv.M32 + y1;
		array3[1] = x2 * mv.M13 + z2 * mv.M33 + z1;
		array[2] = z2 * mv.M31 + x1;
		array2[2] = z2 * mv.M32 + y1;
		array3[2] = z2 * mv.M33 + z1;
		array[3] = y2 * mv.M21 + x1;
		array2[3] = y2 * mv.M22 + y1;
		array3[3] = y2 * mv.M23 + z1;
		array[4] = x2 * mv.M11 + y2 * mv.M21 + x1;
		array2[4] = x2 * mv.M12 + y2 * mv.M22 + y1;
		array3[4] = x2 * mv.M13 + y2 * mv.M23 + z1;
		array[5] = x2 * mv.M11 + y2 * mv.M21 + z2 * mv.M31 + x1;
		array2[5] = x2 * mv.M12 + y2 * mv.M22 + z2 * mv.M32 + y1;
		array3[5] = x2 * mv.M13 + y2 * mv.M23 + z2 * mv.M33 + z1;
		array[6] = z2 * mv.M31 + y2 * mv.M21 + x1;
		array2[6] = z2 * mv.M32 + y2 * mv.M22 + y1;
		array3[6] = z2 * mv.M33 + y2 * mv.M23 + z1;
		int num = (byte)rEffect.CurrentTechnique.Passes.Count;
		for (int i = 0; i < num; i++)
		{
			array4[0].Set_Values(x1, y1, z1, 0f, -1f, 0f, 0f, 0f, 0f, 1f, 0f, c1.R, c1.G, c1.B, c1.A);
			array4[1].Set_Values(array[0], array2[0], array3[0], 0f, -1f, 0f, 0f, 0f, 0f, 1f, 0f, c1.R, c1.G, c1.B, c1.A);
			array4[2].Set_Values(array[1], array2[1], array3[1], 0f, -1f, 0f, 0f, 0f, 0f, 1f, 0f, c1.R, c1.G, c1.B, c1.A);
			rGraphics.DrawUserPrimitives(PrimitiveType.TriangleList, array4, 0, 1, rDecVPCNT);
			array4[1].Set_Values(array[1], array2[1], array3[1], 0f, -1f, 0f, 0f, 0f, 0f, 1f, 0f, c1.R, c1.G, c1.B, c1.A);
			array4[2].Set_Values(array[2], array2[2], array3[2], 0f, -1f, 0f, 0f, 0f, 0f, 1f, 0f, c1.R, c1.G, c1.B, c1.A);
			rGraphics.DrawUserPrimitives(PrimitiveType.TriangleList, array4, 0, 1, rDecVPCNT);
			array4[0].Set_Values(array[0], array2[0], array3[0], 1f, 0f, 0f, 0f, 0f, 0f, 1f, 0f, c1.R, c1.G, c1.B, c1.A);
			array4[1].Set_Values(array[4], array2[4], array3[4], 1f, 0f, 0f, 0f, 0f, 0f, 1f, 0f, c1.R, c1.G, c1.B, c1.A);
			array4[2].Set_Values(array[5], array2[5], array3[5], 1f, 0f, 0f, 0f, 0f, 0f, 1f, 0f, c1.R, c1.G, c1.B, c1.A);
			rGraphics.DrawUserPrimitives(PrimitiveType.TriangleList, array4, 0, 1, rDecVPCNT);
			array4[1].Set_Values(array[5], array2[5], array3[5], 1f, 0f, 0f, 0f, 0f, 0f, 1f, 0f, c1.R, c1.G, c1.B, c1.A);
			array4[2].Set_Values(array[1], array2[1], array3[1], 1f, 0f, 0f, 0f, 0f, 0f, 1f, 0f, c1.R, c1.G, c1.B, c1.A);
			rGraphics.DrawUserPrimitives(PrimitiveType.TriangleList, array4, 0, 1, rDecVPCNT);
			array4[0].Set_Values(array[4], array2[4], array3[4], 0f, 1f, 0f, 0f, 0f, 0f, 1f, 0f, c1.R, c1.G, c1.B, c1.A);
			array4[1].Set_Values(array[3], array2[3], array3[3], 0f, 1f, 0f, 0f, 0f, 0f, 1f, 0f, c1.R, c1.G, c1.B, c1.A);
			array4[2].Set_Values(array[6], array2[6], array3[6], 0f, 1f, 0f, 0f, 0f, 0f, 1f, 0f, c1.R, c1.G, c1.B, c1.A);
			rGraphics.DrawUserPrimitives(PrimitiveType.TriangleList, array4, 0, 1, rDecVPCNT);
			array4[1].Set_Values(array[6], array2[6], array3[6], 0f, 1f, 0f, 0f, 0f, 0f, 1f, 0f, c1.R, c1.G, c1.B, c1.A);
			array4[2].Set_Values(array[5], array2[5], array3[5], 0f, 1f, 0f, 0f, 0f, 0f, 1f, 0f, c1.R, c1.G, c1.B, c1.A);
			rGraphics.DrawUserPrimitives(PrimitiveType.TriangleList, array4, 0, 1, rDecVPCNT);
			array4[0].Set_Values(array[3], array2[3], array3[3], -1f, 0f, 0f, 0f, 0f, 0f, 1f, 0f, c1.R, c1.G, c1.B, c1.A);
			array4[1].Set_Values(x1, y1, z1, -1f, 0f, 0f, 0f, 0f, 0f, 1f, 0f, c1.R, c1.G, c1.B, c1.A);
			array4[2].Set_Values(array[2], array2[2], array3[2], -1f, 0f, 0f, 0f, 0f, 0f, 1f, 0f, c1.R, c1.G, c1.B, c1.A);
			rGraphics.DrawUserPrimitives(PrimitiveType.TriangleList, array4, 0, 1, rDecVPCNT);
			array4[1].Set_Values(array[2], array2[2], array3[2], -1f, 0f, 0f, 0f, 0f, 0f, 1f, 0f, c1.R, c1.G, c1.B, c1.A);
			array4[2].Set_Values(array[6], array2[6], array3[6], -1f, 0f, 0f, 0f, 0f, 0f, 1f, 0f, c1.R, c1.G, c1.B, c1.A);
			rGraphics.DrawUserPrimitives(PrimitiveType.TriangleList, array4, 0, 1, rDecVPCNT);
			array4[0].Set_Values(array[2], array2[2], array3[2], 0f, 0f, 1f, 0f, 0f, 0f, 1f, 0f, c1.R, c1.G, c1.B, c1.A);
			array4[1].Set_Values(array[1], array2[1], array3[1], 0f, 0f, 1f, 0f, 0f, 0f, 1f, 0f, c1.R, c1.G, c1.B, c1.A);
			array4[2].Set_Values(array[5], array2[5], array3[5], 0f, 0f, 1f, 0f, 0f, 0f, 1f, 0f, c1.R, c1.G, c1.B, c1.A);
			rGraphics.DrawUserPrimitives(PrimitiveType.TriangleList, array4, 0, 1, rDecVPCNT);
			array4[1].Set_Values(array[5], array2[5], array3[5], 0f, 0f, 1f, 0f, 0f, 0f, 1f, 0f, c1.R, c1.G, c1.B, c1.A);
			array4[2].Set_Values(array[6], array2[6], array3[6], 0f, 0f, 1f, 0f, 0f, 0f, 1f, 0f, c1.R, c1.G, c1.B, c1.A);
			rGraphics.DrawUserPrimitives(PrimitiveType.TriangleList, array4, 0, 1, rDecVPCNT);
			array4[0].Set_Values(x1, y1, z1, 0f, 0f, -1f, 0f, 0f, 0f, 1f, 0f, c1.R, c1.G, c1.B, c1.A);
			array4[1].Set_Values(array[3], array2[3], array3[3], 0f, 0f, -1f, 0f, 0f, 0f, 1f, 0f, c1.R, c1.G, c1.B, c1.A);
			array4[2].Set_Values(array[4], array2[4], array3[4], 0f, 0f, -1f, 0f, 0f, 0f, 1f, 0f, c1.R, c1.G, c1.B, c1.A);
			rGraphics.DrawUserPrimitives(PrimitiveType.TriangleList, array4, 0, 1, rDecVPCNT);
			array4[1].Set_Values(array[4], array2[4], array3[4], 0f, 0f, -1f, 0f, 0f, 0f, 1f, 0f, c1.R, c1.G, c1.B, c1.A);
			array4[2].Set_Values(array[0], array2[0], array3[0], 0f, 0f, -1f, 0f, 0f, 0f, 1f, 0f, c1.R, c1.G, c1.B, c1.A);
			rGraphics.DrawUserPrimitives(PrimitiveType.TriangleList, array4, 0, 1, rDecVPCNT);
		}
	}

	public void Render_Splash()
	{
		splashSprite.Begin();
		splashSprite.Draw(splashScreen, splashPos, Color.White);
		splashSprite.End();
	}

	public void Render_Loading_Graphic()
	{
		Render_Pulsating_Loading_Graphic();
	}

	public void Render_Spinning_Loading_Graphic()
	{
		Rectangle destinationRectangle = default(Rectangle);
		splashSprite.Begin();
		float num = global::MainGame.MainGame.frametime;
		if (num > 0.03333f)
		{
			num = 0.03333f;
		}
		loadingScreenIconRotation += global::Models.Models.loadingScreenModelDegPerSec * num;
		if (loadingScreenIconRotation > 360f)
		{
			loadingScreenIconRotation -= 360f;
		}
		destinationRectangle.X = loadingIconX;
		destinationRectangle.Y = loadingIconY;
		destinationRectangle.Width = texLoadingScreenIcon.Width;
		destinationRectangle.Height = texLoadingScreenIcon.Height;
		splashSprite.Draw(texLoadingScreenIcon, destinationRectangle, null, Color.White, loadingScreenIconRotation * ((float)Math.PI / 180f), new Vector2((float)texLoadingScreenIcon.Width / 2f, (float)texLoadingScreenIcon.Height / 2f), SpriteEffects.None, 0f);
		splashSprite.End();
	}

	public void Render_Pulsating_Loading_Graphic()
	{
		Rectangle destinationRectangle = default(Rectangle);
		splashSprite.Begin();
		loadingScreenIconRotation += global::MainGame.MainGame.frametime;
		while (loadingScreenIconRotation > global::Models.Models.loadingScreenModelDegPerSec)
		{
			loadingScreenIconRotation -= global::Models.Models.loadingScreenModelDegPerSec;
		}
		float num = loadingScreenIconRotation / global::Models.Models.loadingScreenModelDegPerSec * 0.2f;
		destinationRectangle.X = loadingIconX;
		destinationRectangle.Y = loadingIconY;
		destinationRectangle.Width = (int)((float)texLoadingScreenIcon.Width * (1f + num));
		destinationRectangle.Height = texLoadingScreenIcon.Height;
		splashSprite.Draw(texLoadingScreenIcon, destinationRectangle, null, Color.White, 0f, new Vector2((float)texLoadingScreenIcon.Width / 2f, (float)texLoadingScreenIcon.Height / 2f), SpriteEffects.None, 0f);
		splashSprite.End();
	}

	public void Rotate_Splash_For_Level_Loading()
	{
		curLoadingScreen = (byte)(1f * ((float)global::MainGame.MainGame.mainRandom.Next(0, 32000) / 32000f));
		if (curLoadingScreen >= 1)
		{
			curLoadingScreen = 0;
		}
		if (curLoadingScreen >= 1)
		{
			curLoadingScreen = 0;
		}
		mainC.curGame.CM_Loading.Unload();
		splashScreen = mainC.curGame.CM_Loading.Load<Texture2D>("Textures\\UI_Loading_" + curLoadingScreen);
		texLoadingScreenIcon = mainC.curGame.CM_Loading.Load<Texture2D>("Textures\\UI_Loading_Icon");
		texLoadingScreenSkip = mainC.curGame.CM_Loading.Load<Texture2D>("Textures\\UI_Loading_Skip");
		texLoadingScreenPlay = mainC.curGame.CM_Loading.Load<Texture2D>("Textures\\UI_Icon_PlayIntro");
		texButtonA = mainC.curGame.CM_Loading.Load<Texture2D>("Textures\\UI_Button_A");
		rotateSplash = false;
	}

	public void Set_Splash(byte splashID)
	{
		if (splashID >= 1)
		{
			splashID = 0;
		}
		mainC.curGame.CM_Loading.Unload();
		splashScreen = mainC.curGame.CM_Loading.Load<Texture2D>("Textures\\UI_Loading_" + splashID);
		texLoadingScreenIcon = mainC.curGame.CM_Loading.Load<Texture2D>("Textures\\UI_Loading_Icon");
		texLoadingScreenSkip = mainC.curGame.CM_Loading.Load<Texture2D>("Textures\\UI_Loading_Skip");
		texLoadingScreenPlay = mainC.curGame.CM_Loading.Load<Texture2D>("Textures\\UI_Icon_PlayIntro");
		texButtonA = mainC.curGame.CM_Loading.Load<Texture2D>("Textures\\UI_Icon_Button_A");
		rotateSplash = false;
	}

	public void Render_Instances()
	{
		byte b = 20;
		effect1.CurrentTechnique = effect1.Techniques["Instancing"];
		effect1.CurrentTechnique.Passes[0].Apply();
		for (int i = 0; i < numRenderingInstances; i++)
		{
			if (renderingInstances[i].useVbo)
			{
				continue;
			}
			short numItems = renderingInstances[i].numItems;
			effect1.Parameters["InstanceMatrix"].SetValue(renderingInstances[i].mv);
			effect1.Parameters["InstanceColor"].SetValue(renderingInstances[i].color);
			for (int j = 0; j < renderingInstances[i].numModels; j++)
			{
				int num = renderingInstances[i].modelList[j];
				short num2 = (numItems = renderingInstances[i].numItems);
				if (numItems > global::Models.Models.mod1[num].instanceCount)
				{
					numItems = global::Models.Models.mod1[num].instanceCount;
				}
				if (numItems > b)
				{
					numItems = b;
				}
				int num3 = 0;
				while (num2 > 0)
				{
					for (int k = 0; k < numItems && k < num2; k++)
					{
						pos[k].X = renderingInstances[i].x[num3];
						pos[k].Y = renderingInstances[i].y[num3];
						pos[k].Z = renderingInstances[i].z[num3];
						pos[k].W = renderingInstances[i].tx[num3];
						scale[k].X = renderingInstances[i].sx[num3];
						scale[k].Y = renderingInstances[i].sy[num3];
						scale[k].Z = renderingInstances[i].sz[num3];
						scale[k].W = renderingInstances[i].ty[num3];
						num3++;
					}
					effect1.Parameters["InstancePosition"].SetValue(pos);
					effect1.Parameters["InstanceScale"].SetValue(scale);
					effect1.Parameters["VertexCount"].SetValue(global::Models.Models.mod1[num].vertexCount);
					effect1.Parameters["VertexStart"].SetValue(global::Models.Models.mod1[num].vbStart);
					if (num2 < numItems)
					{
						numItems = num2;
					}
					mainC.modelsMain.Render_Instanced_Model(num, numItems);
					num2 -= numItems;
				}
			}
		}
	}

	public void Render_Instances_PC()
	{
		for (int i = 0; i < numRenderingInstances; i++)
		{
			if (renderingInstances[i].useVbo)
			{
				continue;
			}
			short numItems = renderingInstances[i].numItems;
			for (int j = 0; j < numItems; j++)
			{
				Matrix matrix = Matrix.CreateTranslation(renderingInstances[i].x[j], renderingInstances[i].y[j], renderingInstances[i].z[j]);
				matrix = renderingInstances[i].mv * matrix;
				matrix = Matrix.CreateScale(renderingInstances[i].sx[j], renderingInstances[i].sy[j], renderingInstances[i].sz[j]) * matrix;
				for (int k = 0; k < renderingInstances[i].numModels; k++)
				{
					mainC.modelsMain.Render_Level_Model(renderingInstances[i].modelList[k], ref matrix);
				}
			}
		}
		float[] value = new float[4] { 1f, 1f, 1f, 1f };
		effect1.Parameters["ColorAdjust"].SetValue(value);
	}

	public void Render_Instances_ToSetDepth()
	{
		effect1.CurrentTechnique = effect1.Techniques["InstancingSetDepth"];
		effect1.CurrentTechnique.Passes[0].Apply();
		for (int i = 0; i < numRenderingInstances; i++)
		{
			if (renderingInstances[i].useVbo)
			{
				continue;
			}
			short numItems = renderingInstances[i].numItems;
			for (int j = 0; j < numItems; j++)
			{
				pos[j].X = renderingInstances[i].x[j];
				pos[j].Y = renderingInstances[i].y[j];
				pos[j].Z = renderingInstances[i].z[j];
				pos[j].W = renderingInstances[i].tx[j];
				scale[j].X = renderingInstances[i].sx[j];
				scale[j].Y = renderingInstances[i].sy[j];
				scale[j].Z = renderingInstances[i].sz[j];
				scale[j].W = renderingInstances[i].ty[j];
			}
			effect1.Parameters["InstanceMatrix"].SetValue(renderingInstances[i].mv);
			effect1.Parameters["InstancePosition"].SetValue(pos);
			effect1.Parameters["InstanceScale"].SetValue(scale);
			for (int j = 0; j < renderingInstances[i].numModels; j++)
			{
				int num = renderingInstances[i].modelList[j];
				short num2 = (numItems = renderingInstances[i].numItems);
				if (numItems > global::Models.Models.mod1[num].instanceCount)
				{
					numItems = global::Models.Models.mod1[num].instanceCount;
				}
				_ = global::Models.Models.mod1[num].instanceCount;
				while (num2 > 0)
				{
					effect1.Parameters["VertexCount"].SetValue(global::Models.Models.mod1[num].vertexCount);
					effect1.Parameters["VertexStart"].SetValue(global::Models.Models.mod1[num].vbStart);
					mainC.modelsMain.Render_Instanced_Model_ForDepthOnly(num, numItems);
					num2 -= numItems;
				}
			}
		}
	}

	public void Render_ToSet_DepthBuffer()
	{
		effect1.CurrentTechnique = effect1.Techniques["SetDepthBuffer"];
		effect1.CurrentTechnique.Passes[0].Apply();
		rGraphics.BlendState = BlendState.Opaque;
		rGraphics.SetVertexBuffer(mainVBO);
		rGraphics.Indices = mainIndexBuffer;
		if (numStaticVertices > 0)
		{
			for (long num = 0L; num < numAllocatedVBOList && vboList[(int)checked((nint)num), 1] > -1 && vboList[(int)checked((nint)num), 5] == 1; num++)
			{
				rGraphics.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, vboList[(int)checked((nint)num), 1], vboList[(int)checked((nint)num), 3], vboList[(int)checked((nint)num), 2], vboList[(int)checked((nint)num), 4]);
			}
		}
		if (numStaticAlphaVertices > 0)
		{
			rGraphics.SetVertexBuffer(mainAlphaVBO);
			rGraphics.Indices = mainAlphaIndexBuffer;
			for (long num = 0L; num < global::Textures.Textures.numAlphaTextures && alphaVboList[(int)checked((nint)num), 1] > -1 && alphaVboList[(int)checked((nint)num), 5] == 1; num++)
			{
				rGraphics.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, alphaVboList[(int)checked((nint)num), 1], alphaVboList[(int)checked((nint)num), 3], alphaVboList[(int)checked((nint)num), 2], alphaVboList[(int)checked((nint)num), 4]);
			}
		}
	}

	public void Initialize_Particles()
	{
		LoadParticles("Particles.txt");
		particleParameters = effect1.Parameters;
		effectViewParameter = particleParameters["View"];
		effectProjectionParameter = particleParameters["Projection"];
		effectViewportScaleParameter = particleParameters["ViewportScale"];
		effectTimeParameter = particleParameters["CurrentTime"];
	}

	public void Process_Particles_New()
	{
		for (ushort num = 0; num < numParticleEmitters; num++)
		{
			if (particleEmitters[num].status == 1)
			{
				particleEmitters[num].curTime += global::MainGame.MainGame.frametime / global::Physics.Physics.timeMod;
				if (particleEmitters[num].curTime >= particleEmitters[num].length)
				{
					particleEmitters[num].status = 0;
					particleEmitters[num].curTime = particleEmitters[num].length;
				}
				ushort num2 = (ushort)Math.Round(particleEmitters[num].particlesPerSecond * particleEmitters[num].curTime);
				while (particleEmitters[num].count < num2)
				{
					particleEmitters[num].count++;
					Create_Particle(particleEmitters[num].particleID, particleEmitters[num].x, particleEmitters[num].y, particleEmitters[num].z, particleEmitters[num].nx, particleEmitters[num].ny, particleEmitters[num].nz, particleEmitters[num].vx, particleEmitters[num].vy, particleEmitters[num].vz);
				}
			}
		}
		for (ushort num = 0; num < numParticleTypes; num++)
		{
			MS_Particles[num].currentTime += global::MainGame.MainGame.frametime / global::Physics.Physics.timeMod;
		}
		RetireActiveParticles();
		FreeRetiredParticles();
		for (ushort num = 0; num < numParticleTypes; num++)
		{
			if (MS_Particles[num].firstActiveParticle == MS_Particles[num].firstFreeParticle)
			{
				MS_Particles[num].currentTime = 0f;
			}
			if (MS_Particles[num].firstRetiredParticle == MS_Particles[num].firstActiveParticle)
			{
				MS_Particles[num].drawCounter = 0;
			}
		}
	}

	public void RetireActiveParticles()
	{
		for (byte b = 0; b < numParticleTypes; b++)
		{
			while (MS_Particles[b].firstActiveParticle != MS_Particles[b].firstNewParticle)
			{
				float num = MS_Particles[b].currentTime - MS_Particles[b].MS_Particles[MS_Particles[b].firstActiveParticle * 4].Time;
				if (num < MS_Particles[b].particleDuration)
				{
					break;
				}
				MS_Particles[b].MS_Particles[MS_Particles[b].firstActiveParticle * 4].Time = MS_Particles[b].drawCounter;
				MS_Particles[b].firstActiveParticle++;
				if (MS_Particles[b].firstActiveParticle >= MS_Particles[b].maxParticles)
				{
					MS_Particles[b].firstActiveParticle = 0;
				}
			}
		}
	}

	public void FreeRetiredParticles()
	{
		for (byte b = 0; b < numParticleTypes; b++)
		{
			while (MS_Particles[b].firstRetiredParticle != MS_Particles[b].firstActiveParticle)
			{
				int num = MS_Particles[b].drawCounter - (int)MS_Particles[b].MS_Particles[MS_Particles[b].firstRetiredParticle * 4].Time;
				if (num < 3)
				{
					break;
				}
				MS_Particles[b].firstRetiredParticle++;
				if (MS_Particles[b].firstRetiredParticle >= MS_Particles[b].maxParticles)
				{
					MS_Particles[b].firstRetiredParticle = 0;
				}
			}
		}
	}

	public void Render_Particles_New()
	{
		effect1.Parameters["View"].SetValue(matrixV);
		effect1.Parameters["Projection"].SetValue(matrixP);
		rGraphics.BlendState = BlendState.AlphaBlend;
		rGraphics.DepthStencilState = DepthStencilState.DepthRead;
		effectViewportScaleParameter.SetValue(new Vector2(0.5f / rGraphics.Viewport.AspectRatio, -0.5f));
		effectViewportScaleParameter.SetValue(new Vector2(1f / rGraphics.Viewport.AspectRatio, 1f));
		for (byte b = 0; b < numParticleTypes; b++)
		{
			switch (MS_Particles[b].effectType)
			{
			case 0:
				effect1.CurrentTechnique = effect1.Techniques["Particles"];
				particleParameters["StartSize"].SetValue(new Vector2(MS_Particles[b].startSizeMin, MS_Particles[b].startSizeMin));
				particleParameters["EndSize"].SetValue(new Vector2(MS_Particles[b].endSizeMin, MS_Particles[b].endSizeMin));
				particleParameters["Duration"].SetValue(MS_Particles[b].particleDuration);
				particleParameters["RotateSpeed"].SetValue(new Vector2(MS_Particles[b].rotateSpeedMin, MS_Particles[b].rotateSpeedMax));
				particleParameters["DurationRandomness"].SetValue(MS_Particles[b].durationRandomFactor);
				particleParameters["Gravity"].SetValue(new Vector3(0f, 0f, 0f));
				particleParameters["EndVelocity"].SetValue(MS_Particles[b].velocityScaleFactor);
				particleParameters["MinColor"].SetValue(new Vector4(1f, 1f, 1f, 1f));
				particleParameters["MaxColor"].SetValue(new Vector4(1f, 1f, 1f, 1f));
				break;
			case 1:
				effect1.CurrentTechnique = effect1.Techniques["Particles_Animation"];
				particleParameters["StartSize"].SetValue(new Vector2(MS_Particles[b].startSizeMin, MS_Particles[b].startSizeMax));
				particleParameters["EndSize"].SetValue(new Vector2(MS_Particles[b].endSizeMin, MS_Particles[b].endSizeMax));
				particleParameters["frameCount"].SetValue(MS_Particles[b].frameCount);
				particleParameters["frameCountInverse"].SetValue(MS_Particles[b].invFrameCount);
				particleParameters["Duration"].SetValue(MS_Particles[b].particleDuration);
				particleParameters["RotateSpeed"].SetValue(new Vector2(0f, 0f));
				particleParameters["Gravity"].SetValue(new Vector3(0f, 0f, 0f));
				particleParameters["EndVelocity"].SetValue(0f);
				particleParameters["MinColor"].SetValue(new Vector4(1f, 1f, 1f, 1f));
				particleParameters["MaxColor"].SetValue(new Vector4(1f, 1f, 1f, 1f));
				break;
			}
			particleParameters["Texture"].SetValue(global::Textures.Textures.texMain.texData[MS_Particles[b].textureID]);
			if (MS_Particles[b].newparticleVertexBuffer.IsContentLost)
			{
				MS_Particles[b].newparticleVertexBuffer.SetData(MS_Particles[b].MS_Particles);
			}
			if (MS_Particles[b].firstNewParticle != MS_Particles[b].firstFreeParticle)
			{
				AddNewParticlesToVertexBuffer(b);
			}
			if (MS_Particles[b].firstActiveParticle != MS_Particles[b].firstFreeParticle)
			{
				effectTimeParameter.SetValue(MS_Particles[b].currentTime);
				rGraphics.SetVertexBuffer(MS_Particles[b].newparticleVertexBuffer);
				rGraphics.Indices = MS_Particles[b].newparticleIndexBuffer;
				effect1.CurrentTechnique.Passes[0].Apply();
				if (MS_Particles[b].firstActiveParticle < MS_Particles[b].firstFreeParticle)
				{
					rGraphics.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, MS_Particles[b].firstActiveParticle * 4, (MS_Particles[b].firstFreeParticle - MS_Particles[b].firstActiveParticle) * 4, MS_Particles[b].firstActiveParticle * 6, (MS_Particles[b].firstFreeParticle - MS_Particles[b].firstActiveParticle) * 2);
				}
				else
				{
					rGraphics.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, MS_Particles[b].firstActiveParticle * 4, (MS_Particles[b].maxParticles - MS_Particles[b].firstActiveParticle) * 4, MS_Particles[b].firstActiveParticle * 6, (MS_Particles[b].maxParticles - MS_Particles[b].firstActiveParticle) * 2);
					if (MS_Particles[b].firstFreeParticle > 0)
					{
						rGraphics.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, MS_Particles[b].firstFreeParticle * 4, 0, MS_Particles[b].firstFreeParticle * 2);
					}
				}
			}
			MS_Particles[b].drawCounter++;
		}
		rGraphics.DepthStencilState = DepthStencilState.Default;
	}

	public void AddNewParticlesToVertexBuffer(byte curParticleType)
	{
		int num = 36;
		if (MS_Particles[curParticleType].firstNewParticle < MS_Particles[curParticleType].firstFreeParticle)
		{
			MS_Particles[curParticleType].newparticleVertexBuffer.SetData(MS_Particles[curParticleType].firstNewParticle * num * 4, MS_Particles[curParticleType].MS_Particles, MS_Particles[curParticleType].firstNewParticle * 4, (MS_Particles[curParticleType].firstFreeParticle - MS_Particles[curParticleType].firstNewParticle) * 4, num, SetDataOptions.NoOverwrite);
		}
		else
		{
			MS_Particles[curParticleType].newparticleVertexBuffer.SetData(MS_Particles[curParticleType].firstNewParticle * num * 4, MS_Particles[curParticleType].MS_Particles, MS_Particles[curParticleType].firstNewParticle * 4, (MS_Particles[curParticleType].maxParticles - MS_Particles[curParticleType].firstNewParticle) * 4, num, SetDataOptions.NoOverwrite);
			if (MS_Particles[curParticleType].firstFreeParticle > 0)
			{
				MS_Particles[curParticleType].newparticleVertexBuffer.SetData(0, MS_Particles[curParticleType].MS_Particles, 0, MS_Particles[curParticleType].firstFreeParticle * 4, num, SetDataOptions.NoOverwrite);
			}
		}
		MS_Particles[curParticleType].firstNewParticle = MS_Particles[curParticleType].firstFreeParticle;
	}

	public void AddParticle(byte curParticleType, float x, float y, float z, float vx, float vy, float vz, float startRotation)
	{
		int num = MS_Particles[curParticleType].firstFreeParticle + 1;
		if (num >= MS_Particles[curParticleType].maxParticles)
		{
			num = 0;
		}
		if (num != MS_Particles[curParticleType].firstRetiredParticle)
		{
			Color random = new Color
			{
				R = (byte)global::MainGame.MainGame.mainRandom.Next(255),
				G = (byte)global::MainGame.MainGame.mainRandom.Next(255),
				B = (byte)(startRotation / 360f * 255f),
				A = (byte)global::MainGame.MainGame.mainRandom.Next(255)
			};
			for (int i = 0; i < 4; i++)
			{
				MS_Particles[curParticleType].MS_Particles[MS_Particles[curParticleType].firstFreeParticle * 4 + i].Position.X = x;
				MS_Particles[curParticleType].MS_Particles[MS_Particles[curParticleType].firstFreeParticle * 4 + i].Position.Y = y;
				MS_Particles[curParticleType].MS_Particles[MS_Particles[curParticleType].firstFreeParticle * 4 + i].Position.Z = z;
				MS_Particles[curParticleType].MS_Particles[MS_Particles[curParticleType].firstFreeParticle * 4 + i].Velocity.X = vx;
				MS_Particles[curParticleType].MS_Particles[MS_Particles[curParticleType].firstFreeParticle * 4 + i].Velocity.Y = vy;
				MS_Particles[curParticleType].MS_Particles[MS_Particles[curParticleType].firstFreeParticle * 4 + i].Velocity.Z = vz;
				MS_Particles[curParticleType].MS_Particles[MS_Particles[curParticleType].firstFreeParticle * 4 + i].Random = random;
				MS_Particles[curParticleType].MS_Particles[MS_Particles[curParticleType].firstFreeParticle * 4 + i].Time = MS_Particles[curParticleType].currentTime;
			}
			MS_Particles[curParticleType].firstFreeParticle = num;
		}
	}

	public void LoadParticles(string fileName)
	{
		int num = -1;
		int num2 = -1;
		_ = uBufferID;
		int num3 = -1;
		Stream stream = TitleContainer.OpenStream("The_CoOp_Zombie_Game\\Config_Files\\" + fileName);
		byte[] array = new byte[stream.Length];
		if (!stream.CanRead)
		{
			return;
		}
		stream.Read(array, 0, array.Length);
		string[] array2 = mainC.utilMain.Byte_Array_To_String(array).Split(new char[2] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
		int num4 = array2.Length;
		for (int i = 0; i < num4; i++)
		{
			string[] array3 = array2[i].Split(new char[2] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
			if (array3.Length < 1)
			{
				continue;
			}
			int num5 = 0;
			if (array3[0].Equals("numberOfParticles", StringComparison.OrdinalIgnoreCase))
			{
				num5 = 1;
			}
			else if (array3[0].Equals("numParticleEmitterTypes", StringComparison.OrdinalIgnoreCase))
			{
				num5 = 2;
			}
			else if (array3[0].Equals("particle", StringComparison.OrdinalIgnoreCase))
			{
				num5 = 3;
			}
			else if (array3[0].Equals("effectType", StringComparison.OrdinalIgnoreCase))
			{
				num5 = 4;
			}
			else if (array3[0].Equals("particleEmitter", StringComparison.OrdinalIgnoreCase))
			{
				num5 = 5;
			}
			else if (array3[0].Equals("numParticles", StringComparison.OrdinalIgnoreCase))
			{
				num5 = 6;
			}
			else if (array3[0].Equals("velocity", StringComparison.OrdinalIgnoreCase))
			{
				num5 = 7;
			}
			else if (array3[0].Equals("velocityVariance", StringComparison.OrdinalIgnoreCase))
			{
				num5 = 8;
			}
			else if (array3[0].Equals("positionVariance", StringComparison.OrdinalIgnoreCase))
			{
				num5 = 9;
			}
			else if (array3[0].Equals("positionVarianceHalf", StringComparison.OrdinalIgnoreCase))
			{
				num5 = 10;
			}
			else if (array3[0].Equals("particleDuration", StringComparison.OrdinalIgnoreCase))
			{
				num5 = 11;
			}
			else if (array3[0].Equals("durationRandomFactor", StringComparison.OrdinalIgnoreCase))
			{
				num5 = 12;
			}
			else if (array3[0].Equals("velocityScaleFactor", StringComparison.OrdinalIgnoreCase))
			{
				num5 = 13;
			}
			else if (array3[0].Equals("startSizeMin", StringComparison.OrdinalIgnoreCase))
			{
				num5 = 14;
			}
			else if (array3[0].Equals("startSizeMax", StringComparison.OrdinalIgnoreCase))
			{
				num5 = 15;
			}
			else if (array3[0].Equals("rotateSpeedMin", StringComparison.OrdinalIgnoreCase))
			{
				num5 = 16;
			}
			else if (array3[0].Equals("rotateSpeedMax", StringComparison.OrdinalIgnoreCase))
			{
				num5 = 17;
			}
			else if (array3[0].Equals("endSizeMin", StringComparison.OrdinalIgnoreCase))
			{
				num5 = 18;
			}
			else if (array3[0].Equals("endSizeMax", StringComparison.OrdinalIgnoreCase))
			{
				num5 = 19;
			}
			else if (array3[0].Equals("frameCount", StringComparison.OrdinalIgnoreCase))
			{
				num5 = 20;
			}
			else if (array3[0].Equals("invFrameCount", StringComparison.OrdinalIgnoreCase))
			{
				num5 = 21;
			}
			else if (array3[0].Equals("maxParticle", StringComparison.OrdinalIgnoreCase))
			{
				num5 = 22;
			}
			else if (array3[0].Equals("texture", StringComparison.OrdinalIgnoreCase))
			{
				num5 = 23;
			}
			else if (array3[0].Equals("particleEmitterNumber", StringComparison.OrdinalIgnoreCase))
			{
				num5 = 24;
			}
			else if (array3[0].Equals("particleID", StringComparison.OrdinalIgnoreCase))
			{
				num5 = 25;
			}
			else if (array3[0].Equals("length", StringComparison.OrdinalIgnoreCase))
			{
				num5 = 26;
			}
			else if (array3[0].Equals("particlesPerSecond", StringComparison.OrdinalIgnoreCase))
			{
				num5 = 27;
			}
			else if (array3[0].Equals("angleOffSet", StringComparison.OrdinalIgnoreCase))
			{
				num5 = 28;
			}
			switch (num5)
			{
			case 1:
				if (array3.Length > 1)
				{
					int num6 = int.Parse(array3[1], CultureInfo.InvariantCulture.NumberFormat);
					num3 = num6;
					numParticleTypes = (byte)num6;
					MS_Particles = new StructsClass.Particle_Type[numParticleTypes];
					for (int j = 0; j < num6; j++)
					{
						MS_Particles[j].effectType = 0;
						MS_Particles[j].particleEmitter = -1;
						MS_Particles[j].numParticles = 1;
						MS_Particles[j].velocity = 0f;
						MS_Particles[j].velocityVariance = 0f;
						MS_Particles[j].positionVariance = 0f;
						MS_Particles[j].positionVarianceHalf = 0f;
						MS_Particles[j].particleDuration = 0f;
						MS_Particles[j].durationRandomFactor = 0f;
						MS_Particles[j].startSizeMin = 0f;
						MS_Particles[j].startSizeMax = 0f;
						MS_Particles[j].rotateSpeedMin = 0f;
						MS_Particles[j].rotateSpeedMax = 0f;
						MS_Particles[j].endSizeMin = 0f;
						MS_Particles[j].endSizeMax = 0f;
						MS_Particles[j].frameCount = 0f;
						MS_Particles[j].invFrameCount = 0f;
						MS_Particles[j].maxParticles = 5000;
						MS_Particles[j].angleOffSetMin = -1f;
						MS_Particles[j].range = -1f;
					}
				}
				break;
			case 2:
				if (array3.Length > 1)
				{
					int num6 = int.Parse(array3[1], CultureInfo.InvariantCulture.NumberFormat);
					numParticleEmitterTypes = (byte)num6;
					emitterDefs = new StructsClass.Particle_Emitter[numParticleEmitterTypes];
					for (int j = 0; j < num6; j++)
					{
						emitterDefs[j].particleID = 0;
						emitterDefs[j].length = 0f;
						emitterDefs[j].particlesPerSecond = 0f;
					}
				}
				break;
			case 3:
				if (array3.Length > 1)
				{
					num = int.Parse(array3[1], CultureInfo.InvariantCulture.NumberFormat);
					if (num < 0 || num >= num3)
					{
						num = -1;
					}
				}
				break;
			case 4:
				if (array3.Length > 1 && num > -1)
				{
					MS_Particles[num].effectType = byte.Parse(array3[1], CultureInfo.InvariantCulture.NumberFormat);
				}
				break;
			case 5:
				if (array3.Length > 1 && num > -1)
				{
					MS_Particles[num].particleEmitter = sbyte.Parse(array3[1], CultureInfo.InvariantCulture.NumberFormat);
				}
				break;
			case 6:
				if (array3.Length > 1 && num > -1)
				{
					MS_Particles[num].numParticles = byte.Parse(array3[1], CultureInfo.InvariantCulture.NumberFormat);
				}
				break;
			case 7:
				if (array3.Length > 1 && num > -1)
				{
					MS_Particles[num].velocity = float.Parse(array3[1], CultureInfo.InvariantCulture.NumberFormat);
				}
				break;
			case 8:
				if (array3.Length > 1 && num > -1)
				{
					MS_Particles[num].velocityVariance = float.Parse(array3[1], CultureInfo.InvariantCulture.NumberFormat);
				}
				break;
			case 9:
				if (array3.Length > 1 && num > -1)
				{
					MS_Particles[num].positionVariance = float.Parse(array3[1], CultureInfo.InvariantCulture.NumberFormat);
				}
				break;
			case 10:
				if (array3.Length > 1 && num > -1)
				{
					MS_Particles[num].positionVarianceHalf = float.Parse(array3[1], CultureInfo.InvariantCulture.NumberFormat);
				}
				break;
			case 11:
				if (array3.Length > 1 && num > -1)
				{
					MS_Particles[num].particleDuration = float.Parse(array3[1], CultureInfo.InvariantCulture.NumberFormat);
				}
				break;
			case 12:
				if (array3.Length > 1 && num > -1)
				{
					MS_Particles[num].durationRandomFactor = float.Parse(array3[1], CultureInfo.InvariantCulture.NumberFormat);
				}
				break;
			case 13:
				if (array3.Length > 1 && num > -1)
				{
					MS_Particles[num].velocityScaleFactor = float.Parse(array3[1], CultureInfo.InvariantCulture.NumberFormat);
				}
				break;
			case 14:
				if (array3.Length > 1 && num > -1)
				{
					MS_Particles[num].startSizeMin = float.Parse(array3[1], CultureInfo.InvariantCulture.NumberFormat);
				}
				break;
			case 15:
				if (array3.Length > 1 && num > -1)
				{
					MS_Particles[num].startSizeMax = float.Parse(array3[1], CultureInfo.InvariantCulture.NumberFormat);
				}
				break;
			case 16:
				if (array3.Length > 1 && num > -1)
				{
					MS_Particles[num].rotateSpeedMin = float.Parse(array3[1], CultureInfo.InvariantCulture.NumberFormat);
				}
				break;
			case 17:
				if (array3.Length > 1 && num > -1)
				{
					MS_Particles[num].rotateSpeedMax = float.Parse(array3[1], CultureInfo.InvariantCulture.NumberFormat);
				}
				break;
			case 18:
				if (array3.Length > 1 && num > -1)
				{
					MS_Particles[num].endSizeMin = float.Parse(array3[1], CultureInfo.InvariantCulture.NumberFormat);
				}
				break;
			case 19:
				if (array3.Length > 1 && num > -1)
				{
					MS_Particles[num].endSizeMax = float.Parse(array3[1], CultureInfo.InvariantCulture.NumberFormat);
				}
				break;
			case 20:
				if (array3.Length > 1 && num > -1)
				{
					MS_Particles[num].frameCount = float.Parse(array3[1], CultureInfo.InvariantCulture.NumberFormat);
				}
				break;
			case 21:
				if (array3.Length > 1 && num > -1)
				{
					MS_Particles[num].invFrameCount = float.Parse(array3[1], CultureInfo.InvariantCulture.NumberFormat);
				}
				break;
			case 22:
				if (array3.Length > 1 && num > -1)
				{
					MS_Particles[num].maxParticles = ushort.Parse(array3[1], CultureInfo.InvariantCulture.NumberFormat);
				}
				break;
			case 23:
				if (array3.Length > 1 && num > -1)
				{
					MS_Particles[num].texture = array3[1];
					MS_Particles[num].textureID = (ushort)mainC.texturesMain.Find_Texture(MS_Particles[num].texture, 0);
				}
				break;
			case 24:
				num2 = int.Parse(array3[1], CultureInfo.InvariantCulture.NumberFormat);
				if (num2 < 0 || num2 >= numParticleEmitterTypes)
				{
					num2 = -1;
				}
				break;
			case 25:
				if (array3.Length > 1 && num2 > -1)
				{
					emitterDefs[num2].particleID = byte.Parse(array3[1], CultureInfo.InvariantCulture.NumberFormat);
				}
				break;
			case 26:
				if (array3.Length > 1 && num2 > -1)
				{
					emitterDefs[num2].length = float.Parse(array3[1], CultureInfo.InvariantCulture.NumberFormat);
				}
				break;
			case 27:
				if (array3.Length > 1 && num2 > -1)
				{
					emitterDefs[num2].particlesPerSecond = float.Parse(array3[1], CultureInfo.InvariantCulture.NumberFormat);
				}
				break;
			case 28:
				if (array3.Length > 2 && num > -1)
				{
					MS_Particles[num].angleOffSetMin = float.Parse(array3[1], CultureInfo.InvariantCulture.NumberFormat);
					MS_Particles[num].range = float.Parse(array3[2], CultureInfo.InvariantCulture.NumberFormat);
				}
				break;
			}
		}
		num = 0;
		int num7 = 0;
		while (num7 < num3)
		{
			int num6 = (ushort)(MS_Particles[num].maxParticles * 4);
			MS_Particles[num].MS_Particles = new StructsClass.ParticleVertex[num6];
			int j = 0;
			while (j < num6)
			{
				MS_Particles[num].MS_Particles[j++].Corner = new Short2(-1f, -1f);
				MS_Particles[num].MS_Particles[j++].Corner = new Short2(1f, -1f);
				MS_Particles[num].MS_Particles[j++].Corner = new Short2(1f, 1f);
				MS_Particles[num].MS_Particles[j++].Corner = new Short2(-1f, 1f);
			}
			MS_Particles[num].newparticleVertexBuffer = new DynamicVertexBuffer(rGraphics, StructsClass.ParticleVertex.VertexDeclaration, num6, BufferUsage.WriteOnly);
			num6 = MS_Particles[num].maxParticles * 6;
			ushort[] array4 = new ushort[num6];
			j = 0;
			num6 = 0;
			int num8 = 0;
			while (j < MS_Particles[num].maxParticles)
			{
				array4[num6++] = (ushort)num8;
				array4[num6++] = (ushort)(num8 + 1);
				array4[num6++] = (ushort)(num8 + 2);
				array4[num6++] = (ushort)num8;
				array4[num6++] = (ushort)(num8 + 2);
				array4[num6++] = (ushort)(num8 + 3);
				j++;
				num8 += 4;
			}
			MS_Particles[num].newparticleIndexBuffer = new IndexBuffer(rGraphics, typeof(ushort), num6, BufferUsage.WriteOnly);
			MS_Particles[num].newparticleIndexBuffer.SetData(array4);
			num7++;
			num++;
		}
		particleParameters = effect1.Parameters;
		particleParameters["Gravity"].SetValue(new Vector3(0f, 0f, 0f));
		particleParameters["EndVelocity"].SetValue(1f);
		particleParameters["MinColor"].SetValue(new Vector4(1f, 1f, 1f, 1f));
		particleParameters["MaxColor"].SetValue(new Vector4(1f, 1f, 1f, 1f));
		particleParameters["RotateSpeed"].SetValue(new Vector2(-1f, 1f));
		particleParameters["StartSize"].SetValue(new Vector2(0.02f, 0.04f));
		particleParameters["EndSize"].SetValue(new Vector2(0.04f, 0.25f));
		numParticleEmitters = 20;
		particleEmitters = new StructsClass.Particle_Emitter_Instance[numParticleEmitters];
		for (int j = 0; j < numParticleEmitters; j++)
		{
			particleEmitters[j].status = 0;
		}
	}

	public void SaveParticle()
	{
		string path = Environment.CurrentDirectory + "\\The_CoOp_Zombie_Game\\Config_Files\\Particles.txt";
		StreamWriter streamWriter = File.CreateText(path);
		streamWriter.WriteLine("numberOfParticles " + numParticleTypes.ToString(CultureInfo.InvariantCulture) + "\r\n");
		streamWriter.WriteLine("numParticleEmitters " + numParticleEmitters.ToString(CultureInfo.InvariantCulture) + "\r\n");
		streamWriter.WriteLine("numParticleEmitterTypes " + numParticleEmitterTypes.ToString(CultureInfo.InvariantCulture) + "\r\n");
		for (ushort num = 0; num < numParticleEmitterTypes; num++)
		{
			streamWriter.WriteLine("# ParticleEmitter" + num.ToString(CultureInfo.InvariantCulture));
			streamWriter.WriteLine("particleEmitterNumber " + num.ToString(CultureInfo.InvariantCulture) + "\r\n");
			streamWriter.WriteLine("\tparticleID\t" + emitterDefs[num].particleID.ToString(CultureInfo.InvariantCulture));
			streamWriter.WriteLine("\tlength\t" + emitterDefs[num].length.ToString(CultureInfo.InvariantCulture));
			streamWriter.WriteLine("\tparticlesPerSecond\t" + emitterDefs[num].particlesPerSecond.ToString(CultureInfo.InvariantCulture) + "\r\n");
		}
		for (ushort num = 0; num < numParticleTypes; num++)
		{
			streamWriter.WriteLine("# Particle" + num.ToString(CultureInfo.InvariantCulture));
			streamWriter.WriteLine("particle " + num.ToString(CultureInfo.InvariantCulture) + "\r\n");
			streamWriter.WriteLine("\teffectType\t" + MS_Particles[num].effectType.ToString(CultureInfo.InvariantCulture));
			streamWriter.WriteLine("\tparticleEmitter\t" + MS_Particles[num].particleEmitter.ToString(CultureInfo.InvariantCulture));
			streamWriter.WriteLine("\tnumParticles\t" + MS_Particles[num].numParticles.ToString(CultureInfo.InvariantCulture));
			streamWriter.WriteLine("\tvelocity\t" + MS_Particles[num].velocity.ToString(CultureInfo.InvariantCulture));
			streamWriter.WriteLine("\tvelocityVariance\t" + MS_Particles[num].velocityVariance.ToString(CultureInfo.InvariantCulture));
			streamWriter.WriteLine("\tpossitionVarianceHalf\t" + MS_Particles[num].positionVarianceHalf.ToString(CultureInfo.InvariantCulture));
			streamWriter.WriteLine("\tparticleDuration\t" + MS_Particles[num].particleDuration.ToString(CultureInfo.InvariantCulture));
			streamWriter.WriteLine("\tdurationRandomFactor\t" + MS_Particles[num].durationRandomFactor.ToString(CultureInfo.InvariantCulture));
			streamWriter.WriteLine("\tvelocityScaleFactor\t" + MS_Particles[num].velocityScaleFactor.ToString(CultureInfo.InvariantCulture));
			streamWriter.WriteLine("\tstartSizeMin\t" + MS_Particles[num].startSizeMin.ToString(CultureInfo.InvariantCulture));
			streamWriter.WriteLine("\tstartSizeMax\t" + MS_Particles[num].startSizeMax.ToString(CultureInfo.InvariantCulture));
			streamWriter.WriteLine("\trotateSpeedMin\t" + MS_Particles[num].rotateSpeedMin.ToString(CultureInfo.InvariantCulture));
			streamWriter.WriteLine("\trotateSpeedMax\t" + MS_Particles[num].rotateSpeedMax.ToString(CultureInfo.InvariantCulture));
			streamWriter.WriteLine("\tendSizeMin\t" + MS_Particles[num].endSizeMin.ToString(CultureInfo.InvariantCulture));
			streamWriter.WriteLine("\tendSizeMax\t" + MS_Particles[num].endSizeMax.ToString(CultureInfo.InvariantCulture));
			streamWriter.WriteLine("\tframeCount\t" + MS_Particles[num].frameCount.ToString(CultureInfo.InvariantCulture));
			streamWriter.WriteLine("\tinvFrameCount\t" + MS_Particles[num].invFrameCount.ToString(CultureInfo.InvariantCulture));
			streamWriter.WriteLine("\tmaxParticle\t" + MS_Particles[num].maxParticles.ToString(CultureInfo.InvariantCulture));
			streamWriter.WriteLine("\ttexture\t" + MS_Particles[num].texture.ToString(CultureInfo.InvariantCulture) + "\r\n");
		}
		streamWriter.Close();
	}

	public void Process_Particles()
	{
		Process_Particles_New();
		float num = global::MainGame.MainGame.frametime / global::Physics.Physics.timeMod;
		particleDistance[uBufferID, 0] = particleDistance[rBufferID, 0];
		particleLightColor[uBufferID, 0, 0] = particleLightColor[rBufferID, 0, 0];
		particleLightColor[uBufferID, 0, 1] = particleLightColor[rBufferID, 0, 1];
		particleLightColor[uBufferID, 0, 2] = particleLightColor[rBufferID, 0, 2];
		particleLightColor[uBufferID, 0, 3] = particleLightColor[rBufferID, 0, 3];
		particleLightLocation[uBufferID, 0, 0] = particleLightLocation[rBufferID, 0, 0];
		particleLightLocation[uBufferID, 0, 1] = particleLightLocation[rBufferID, 0, 1];
		particleLightLocation[uBufferID, 0, 2] = particleLightLocation[rBufferID, 0, 2];
		particleDistance[uBufferID, 1] = particleDistance[rBufferID, 1];
		particleLightColor[uBufferID, 1, 0] = particleLightColor[rBufferID, 1, 0];
		particleLightColor[uBufferID, 1, 1] = particleLightColor[rBufferID, 1, 1];
		particleLightColor[uBufferID, 1, 2] = particleLightColor[rBufferID, 1, 2];
		particleLightColor[uBufferID, 1, 3] = particleLightColor[rBufferID, 1, 3];
		particleLightLocation[uBufferID, 1, 0] = particleLightLocation[rBufferID, 1, 0];
		particleLightLocation[uBufferID, 1, 1] = particleLightLocation[rBufferID, 1, 1];
		particleLightLocation[uBufferID, 1, 2] = particleLightLocation[rBufferID, 1, 2];
		particleDistance[uBufferID, 2] = particleDistance[rBufferID, 2];
		particleLightColor[uBufferID, 2, 0] = particleLightColor[rBufferID, 2, 0];
		particleLightColor[uBufferID, 2, 1] = particleLightColor[rBufferID, 2, 1];
		particleLightColor[uBufferID, 2, 2] = particleLightColor[rBufferID, 2, 2];
		particleLightColor[uBufferID, 2, 3] = particleLightColor[rBufferID, 2, 3];
		particleLightLocation[uBufferID, 2, 0] = particleLightLocation[rBufferID, 2, 0];
		particleLightLocation[uBufferID, 2, 1] = particleLightLocation[rBufferID, 2, 1];
		particleLightLocation[uBufferID, 2, 2] = particleLightLocation[rBufferID, 2, 2];
		particleDistance[uBufferID, 3] = particleDistance[rBufferID, 3];
		particleLightColor[uBufferID, 3, 0] = particleLightColor[rBufferID, 3, 0];
		particleLightColor[uBufferID, 3, 1] = particleLightColor[rBufferID, 3, 1];
		particleLightColor[uBufferID, 3, 2] = particleLightColor[rBufferID, 3, 2];
		particleLightColor[uBufferID, 3, 3] = particleLightColor[rBufferID, 3, 3];
		particleLightLocation[uBufferID, 3, 0] = particleLightLocation[rBufferID, 3, 0];
		particleLightLocation[uBufferID, 3, 1] = particleLightLocation[rBufferID, 3, 1];
		particleLightLocation[uBufferID, 3, 2] = particleLightLocation[rBufferID, 3, 2];
		particleDistance[uBufferID, 4] = particleDistance[rBufferID, 4];
		particleLightColor[uBufferID, 4, 0] = particleLightColor[rBufferID, 4, 0];
		particleLightColor[uBufferID, 4, 1] = particleLightColor[rBufferID, 4, 1];
		particleLightColor[uBufferID, 4, 2] = particleLightColor[rBufferID, 4, 2];
		particleLightColor[uBufferID, 4, 3] = particleLightColor[rBufferID, 4, 3];
		particleLightLocation[uBufferID, 4, 0] = particleLightLocation[rBufferID, 4, 0];
		particleLightLocation[uBufferID, 4, 1] = particleLightLocation[rBufferID, 4, 1];
		particleLightLocation[uBufferID, 4, 2] = particleLightLocation[rBufferID, 4, 2];
		for (int i = 0; i < 1500; i++)
		{
			if (particles[rBufferID, i].lifeTime > 0f)
			{
				sbyte type = particles[rBufferID, i].type;
				if (type == 10)
				{
					continue;
				}
				particles[uBufferID, i].lifeTime = particles[rBufferID, i].lifeTime - num;
				if (particles[uBufferID, i].lifeTime > 0f)
				{
					particles[uBufferID, i].texID = particles[rBufferID, i].texID;
					particles[uBufferID, i].modID = particles[rBufferID, i].modID;
					particles[uBufferID, i].fadeOutTimer = particles[rBufferID, i].fadeOutTimer;
					particles[uBufferID, i].rotation = particles[rBufferID, i].rotation;
					particles[uBufferID, i].phys1.position.v[0] = particles[rBufferID, i].phys1.position.v[0];
					particles[uBufferID, i].phys1.position.v[1] = particles[rBufferID, i].phys1.position.v[1];
					particles[uBufferID, i].phys1.position.v[2] = particles[rBufferID, i].phys1.position.v[2];
					particles[uBufferID, i].phys1.velocity.v[0] = particles[rBufferID, i].phys1.velocity.v[0];
					particles[uBufferID, i].phys1.velocity.v[1] = particles[rBufferID, i].phys1.velocity.v[1];
					particles[uBufferID, i].phys1.velocity.v[2] = particles[rBufferID, i].phys1.velocity.v[2];
					particles[uBufferID, i].phys1.acceleration.v[0] = particles[rBufferID, i].phys1.acceleration.v[0];
					particles[uBufferID, i].phys1.acceleration.v[1] = particles[rBufferID, i].phys1.acceleration.v[1];
					particles[uBufferID, i].phys1.acceleration.v[2] = particles[rBufferID, i].phys1.acceleration.v[2];
					particles[uBufferID, i].size = particles[rBufferID, i].size;
					particles[uBufferID, i].sizeChange = particles[rBufferID, i].sizeChange;
					particles[uBufferID, i].color[0] = particles[rBufferID, i].color[0];
					particles[uBufferID, i].color[1] = particles[rBufferID, i].color[1];
					particles[uBufferID, i].color[2] = particles[rBufferID, i].color[2];
					particles[uBufferID, i].color[3] = particles[rBufferID, i].color[3];
					particles[uBufferID, i].colorChange[0] = particles[rBufferID, i].colorChange[0];
					particles[uBufferID, i].colorChange[1] = particles[rBufferID, i].colorChange[1];
					particles[uBufferID, i].colorChange[2] = particles[rBufferID, i].colorChange[2];
					particles[uBufferID, i].colorChange[3] = particles[rBufferID, i].colorChange[3];
					particles[uBufferID, i].lightID = particles[rBufferID, i].lightID;
					particles[uBufferID, i].type = particles[rBufferID, i].type;
					particles[uBufferID, i].refID = particles[rBufferID, i].refID;
					mainC.physicsMain.getPosition(ref particles[uBufferID, i].phys1, global::MainGame.MainGame.frametime);
					if (particles[uBufferID, i].type == 8)
					{
						particles[uBufferID, i].soundID = particles[rBufferID, i].soundID;
						mainC.soundsMain.Update_Sound_Position(particles[uBufferID, i].soundID, particles[uBufferID, i].phys1.position.v[0], particles[uBufferID, i].phys1.position.v[1], particles[uBufferID, i].phys1.position.v[2]);
					}
					particles[uBufferID, i].size += particles[uBufferID, i].sizeChange * num;
					particles[uBufferID, i].color[0] += particles[uBufferID, i].colorChange[0] * num;
					particles[uBufferID, i].color[1] += particles[uBufferID, i].colorChange[1] * num;
					particles[uBufferID, i].color[2] += particles[uBufferID, i].colorChange[2] * num;
					if (particles[uBufferID, i].lifeTime <= particles[uBufferID, i].fadeOutTimer)
					{
						particles[uBufferID, i].color[3] += particles[uBufferID, i].colorChange[3] * num;
					}
					if (particles[uBufferID, i].lightID > -1)
					{
						float num2 = particles[uBufferID, i].lifeTime / (particles[uBufferID, i].lifeTime + num);
						int lightID = particles[uBufferID, i].lightID;
						particleLightColor[uBufferID, lightID, 0] = particleLightColor[rBufferID, lightID, 0] * num2;
						particleLightColor[uBufferID, lightID, 1] = particleLightColor[rBufferID, lightID, 1] * num2;
						particleLightColor[uBufferID, lightID, 2] = particleLightColor[rBufferID, lightID, 2] * num2;
					}
				}
				else
				{
					switch (particles[rBufferID, i].type)
					{
					case 1:
						global::Players.Players.players[particles[rBufferID, i].refID].particles[1] = -1;
						break;
					case 12:
						global::Players.Players.players[particles[rBufferID, i].refID].particles[6] = -1;
						break;
					}
					if (particles[rBufferID, i].lightID > -1)
					{
						int lightID = particles[rBufferID, i].lightID;
						particleLightColor[uBufferID, lightID, 0] = 0f;
						particleLightColor[uBufferID, lightID, 1] = 0f;
						particleLightColor[uBufferID, lightID, 2] = 0f;
						particleLightColor[uBufferID, lightID, 3] = 0f;
						particles[uBufferID, i].lightID = -1;
						particleLights[lightID] = -1;
					}
				}
			}
			else
			{
				particles[uBufferID, i].lifeTime = -1f;
				int lightID;
				if ((lightID = particles[rBufferID, i].lightID) > -1)
				{
					particleLightColor[uBufferID, lightID, 0] = 0f;
					particleLightColor[uBufferID, lightID, 1] = 0f;
					particleLightColor[uBufferID, lightID, 2] = 0f;
					particleLightColor[uBufferID, lightID, 3] = 0f;
					particleLights[lightID] = -1;
				}
				particles[uBufferID, i].lightID = -1;
			}
		}
		for (int i = 0; i < 50; i++)
		{
			if (solidParticles[rBufferID, i].lifeTime > 0f)
			{
				_ = solidParticles[rBufferID, i].type;
				solidParticles[uBufferID, i].lifeTime = solidParticles[rBufferID, i].lifeTime - num;
				if (solidParticles[uBufferID, i].lifeTime > 0f)
				{
					solidParticles[uBufferID, i].texID = solidParticles[rBufferID, i].texID;
					solidParticles[uBufferID, i].rotX = solidParticles[rBufferID, i].rotX;
					solidParticles[uBufferID, i].rotY = solidParticles[rBufferID, i].rotY;
					solidParticles[uBufferID, i].rotZ = solidParticles[rBufferID, i].rotZ;
					solidParticles[uBufferID, i].particleRot = solidParticles[rBufferID, i].particleRot * Quaternion.CreateFromYawPitchRoll(solidParticles[rBufferID, i].rotY * global::MainGame.MainGame.frametime, solidParticles[rBufferID, i].rotX * global::MainGame.MainGame.frametime, solidParticles[rBufferID, i].rotZ * global::MainGame.MainGame.frametime);
					solidParticles[uBufferID, i].phys1.position.v[0] = solidParticles[rBufferID, i].phys1.position.v[0];
					solidParticles[uBufferID, i].phys1.position.v[1] = solidParticles[rBufferID, i].phys1.position.v[1];
					solidParticles[uBufferID, i].phys1.position.v[2] = solidParticles[rBufferID, i].phys1.position.v[2];
					solidParticles[uBufferID, i].phys1.velocity.v[0] = solidParticles[rBufferID, i].phys1.velocity.v[0];
					solidParticles[uBufferID, i].phys1.velocity.v[1] = solidParticles[rBufferID, i].phys1.velocity.v[1];
					solidParticles[uBufferID, i].phys1.velocity.v[2] = solidParticles[rBufferID, i].phys1.velocity.v[2];
					solidParticles[uBufferID, i].phys1.acceleration.v[0] = solidParticles[rBufferID, i].phys1.acceleration.v[0];
					solidParticles[uBufferID, i].phys1.acceleration.v[1] = solidParticles[rBufferID, i].phys1.acceleration.v[1];
					solidParticles[uBufferID, i].phys1.acceleration.v[2] = solidParticles[rBufferID, i].phys1.acceleration.v[2];
					solidParticles[uBufferID, i].size = solidParticles[rBufferID, i].size;
					solidParticles[uBufferID, i].sizeChange = solidParticles[rBufferID, i].sizeChange;
					solidParticles[uBufferID, i].color[0] = solidParticles[rBufferID, i].color[0];
					solidParticles[uBufferID, i].color[1] = solidParticles[rBufferID, i].color[1];
					solidParticles[uBufferID, i].color[2] = solidParticles[rBufferID, i].color[2];
					solidParticles[uBufferID, i].color[3] = solidParticles[rBufferID, i].color[3];
					solidParticles[uBufferID, i].colorChange[0] = solidParticles[rBufferID, i].colorChange[0];
					solidParticles[uBufferID, i].colorChange[1] = solidParticles[rBufferID, i].colorChange[1];
					solidParticles[uBufferID, i].colorChange[2] = solidParticles[rBufferID, i].colorChange[2];
					solidParticles[uBufferID, i].colorChange[3] = solidParticles[rBufferID, i].colorChange[3];
					solidParticles[uBufferID, i].lightID = solidParticles[rBufferID, i].lightID;
					solidParticles[uBufferID, i].type = solidParticles[rBufferID, i].type;
					solidParticles[uBufferID, i].modID = solidParticles[rBufferID, i].modID;
					mainC.physicsMain.getPosition(ref solidParticles[uBufferID, i].phys1, global::MainGame.MainGame.frametime);
					if (solidParticles[uBufferID, i].type == 8)
					{
						solidParticles[uBufferID, i].soundID = solidParticles[rBufferID, i].soundID;
						mainC.soundsMain.Update_Sound_Position(solidParticles[uBufferID, i].soundID, solidParticles[uBufferID, i].phys1.position.v[0], solidParticles[uBufferID, i].phys1.position.v[1], solidParticles[uBufferID, i].phys1.position.v[2]);
					}
					solidParticles[uBufferID, i].size += solidParticles[uBufferID, i].sizeChange * num;
					solidParticles[uBufferID, i].color[0] += solidParticles[uBufferID, i].colorChange[0] * num;
					solidParticles[uBufferID, i].color[1] += solidParticles[uBufferID, i].colorChange[1] * num;
					solidParticles[uBufferID, i].color[2] += solidParticles[uBufferID, i].colorChange[2] * num;
					solidParticles[uBufferID, i].color[3] += solidParticles[uBufferID, i].colorChange[3] * num;
				}
			}
			else
			{
				solidParticles[uBufferID, i].lifeTime = -1f;
			}
		}
	}

	public void Sync_Particles()
	{
		particleDistance[rBufferID, 0] = particleDistance[uBufferID, 0];
		particleLightColor[rBufferID, 0, 0] = particleLightColor[uBufferID, 0, 0];
		particleLightColor[rBufferID, 0, 1] = particleLightColor[uBufferID, 0, 1];
		particleLightColor[rBufferID, 0, 2] = particleLightColor[uBufferID, 0, 2];
		particleLightColor[rBufferID, 0, 3] = particleLightColor[uBufferID, 0, 3];
		particleLightLocation[rBufferID, 0, 0] = particleLightLocation[uBufferID, 0, 0];
		particleLightLocation[rBufferID, 0, 1] = particleLightLocation[uBufferID, 0, 1];
		particleLightLocation[rBufferID, 0, 2] = particleLightLocation[uBufferID, 0, 2];
		particleDistance[rBufferID, 1] = particleDistance[uBufferID, 1];
		particleLightColor[rBufferID, 1, 0] = particleLightColor[uBufferID, 1, 0];
		particleLightColor[rBufferID, 1, 1] = particleLightColor[uBufferID, 1, 1];
		particleLightColor[rBufferID, 1, 2] = particleLightColor[uBufferID, 1, 2];
		particleLightColor[rBufferID, 1, 3] = particleLightColor[uBufferID, 1, 3];
		particleLightLocation[rBufferID, 1, 0] = particleLightLocation[uBufferID, 1, 0];
		particleLightLocation[rBufferID, 1, 1] = particleLightLocation[uBufferID, 1, 1];
		particleLightLocation[rBufferID, 1, 2] = particleLightLocation[uBufferID, 1, 2];
		particleDistance[rBufferID, 2] = particleDistance[uBufferID, 2];
		particleLightColor[rBufferID, 2, 0] = particleLightColor[uBufferID, 2, 0];
		particleLightColor[rBufferID, 2, 1] = particleLightColor[uBufferID, 2, 1];
		particleLightColor[rBufferID, 2, 2] = particleLightColor[uBufferID, 2, 2];
		particleLightColor[rBufferID, 2, 3] = particleLightColor[uBufferID, 2, 3];
		particleLightLocation[rBufferID, 2, 0] = particleLightLocation[uBufferID, 2, 0];
		particleLightLocation[rBufferID, 2, 1] = particleLightLocation[uBufferID, 2, 1];
		particleLightLocation[rBufferID, 2, 2] = particleLightLocation[uBufferID, 2, 2];
		particleDistance[rBufferID, 3] = particleDistance[uBufferID, 3];
		particleLightColor[rBufferID, 3, 0] = particleLightColor[uBufferID, 3, 0];
		particleLightColor[rBufferID, 3, 1] = particleLightColor[uBufferID, 3, 1];
		particleLightColor[rBufferID, 3, 2] = particleLightColor[uBufferID, 3, 2];
		particleLightColor[rBufferID, 3, 3] = particleLightColor[uBufferID, 3, 3];
		particleLightLocation[rBufferID, 3, 0] = particleLightLocation[uBufferID, 3, 0];
		particleLightLocation[rBufferID, 3, 1] = particleLightLocation[uBufferID, 3, 1];
		particleLightLocation[rBufferID, 3, 2] = particleLightLocation[uBufferID, 3, 2];
		particleDistance[rBufferID, 4] = particleDistance[uBufferID, 4];
		particleLightColor[rBufferID, 4, 0] = particleLightColor[uBufferID, 4, 0];
		particleLightColor[rBufferID, 4, 1] = particleLightColor[uBufferID, 4, 1];
		particleLightColor[rBufferID, 4, 2] = particleLightColor[uBufferID, 4, 2];
		particleLightColor[rBufferID, 4, 3] = particleLightColor[uBufferID, 4, 3];
		particleLightLocation[rBufferID, 4, 0] = particleLightLocation[uBufferID, 4, 0];
		particleLightLocation[rBufferID, 4, 1] = particleLightLocation[uBufferID, 4, 1];
		particleLightLocation[rBufferID, 4, 2] = particleLightLocation[uBufferID, 4, 2];
		for (int i = 0; i < 1500; i++)
		{
			if (particles[uBufferID, i].lifeTime > 0f && particles[rBufferID, i].lifeTime <= 0f)
			{
				sbyte type = particles[uBufferID, i].type;
				if (type != 10)
				{
					particles[rBufferID, i].lifeTime = particles[uBufferID, i].lifeTime;
					particles[rBufferID, i].texID = particles[uBufferID, i].texID;
					particles[rBufferID, i].modID = particles[uBufferID, i].modID;
					particles[rBufferID, i].fadeOutTimer = particles[uBufferID, i].fadeOutTimer;
					particles[rBufferID, i].rotation = particles[uBufferID, i].rotation;
					particles[rBufferID, i].phys1.position.v[0] = particles[uBufferID, i].phys1.position.v[0];
					particles[rBufferID, i].phys1.position.v[1] = particles[uBufferID, i].phys1.position.v[1];
					particles[rBufferID, i].phys1.position.v[2] = particles[uBufferID, i].phys1.position.v[2];
					particles[rBufferID, i].phys1.velocity.v[0] = particles[uBufferID, i].phys1.velocity.v[0];
					particles[rBufferID, i].phys1.velocity.v[1] = particles[uBufferID, i].phys1.velocity.v[1];
					particles[rBufferID, i].phys1.velocity.v[2] = particles[uBufferID, i].phys1.velocity.v[2];
					particles[rBufferID, i].phys1.acceleration.v[0] = particles[uBufferID, i].phys1.acceleration.v[0];
					particles[rBufferID, i].phys1.acceleration.v[1] = particles[uBufferID, i].phys1.acceleration.v[1];
					particles[rBufferID, i].phys1.acceleration.v[2] = particles[uBufferID, i].phys1.acceleration.v[2];
					particles[rBufferID, i].size = particles[uBufferID, i].size;
					particles[rBufferID, i].sizeChange = particles[uBufferID, i].sizeChange;
					particles[rBufferID, i].color[0] = particles[uBufferID, i].color[0];
					particles[rBufferID, i].color[1] = particles[uBufferID, i].color[1];
					particles[rBufferID, i].color[2] = particles[uBufferID, i].color[2];
					particles[rBufferID, i].color[3] = particles[uBufferID, i].color[3];
					particles[rBufferID, i].colorChange[0] = particles[uBufferID, i].colorChange[0];
					particles[rBufferID, i].colorChange[1] = particles[uBufferID, i].colorChange[1];
					particles[rBufferID, i].colorChange[2] = particles[uBufferID, i].colorChange[2];
					particles[rBufferID, i].colorChange[3] = particles[uBufferID, i].colorChange[3];
					particles[rBufferID, i].lightID = particles[uBufferID, i].lightID;
					particles[rBufferID, i].type = particles[uBufferID, i].type;
					particles[rBufferID, i].refID = particles[uBufferID, i].refID;
					particles[rBufferID, i].soundID = particles[uBufferID, i].soundID;
					particles[rBufferID, i].lightID = particles[uBufferID, i].lightID;
					if (particles[uBufferID, i].lightID > -1)
					{
						int lightID = particles[rBufferID, i].lightID;
						particleLightColor[rBufferID, lightID, 0] = particleLightColor[uBufferID, lightID, 0];
						particleLightColor[rBufferID, lightID, 1] = particleLightColor[uBufferID, lightID, 1];
						particleLightColor[rBufferID, lightID, 2] = particleLightColor[uBufferID, lightID, 2];
					}
				}
			}
			else
			{
				if (!(particles[rBufferID, i].lifeTime > 0f))
				{
					continue;
				}
				sbyte type2 = particles[uBufferID, i].type;
				if (type2 != 10)
				{
					particles[uBufferID, i].lifeTime = particles[rBufferID, i].lifeTime;
					particles[uBufferID, i].texID = particles[rBufferID, i].texID;
					particles[uBufferID, i].modID = particles[rBufferID, i].modID;
					particles[uBufferID, i].fadeOutTimer = particles[rBufferID, i].fadeOutTimer;
					particles[uBufferID, i].rotation = particles[rBufferID, i].rotation;
					particles[uBufferID, i].phys1.position.v[0] = particles[rBufferID, i].phys1.position.v[0];
					particles[uBufferID, i].phys1.position.v[1] = particles[rBufferID, i].phys1.position.v[1];
					particles[uBufferID, i].phys1.position.v[2] = particles[rBufferID, i].phys1.position.v[2];
					particles[uBufferID, i].phys1.velocity.v[0] = particles[rBufferID, i].phys1.velocity.v[0];
					particles[uBufferID, i].phys1.velocity.v[1] = particles[rBufferID, i].phys1.velocity.v[1];
					particles[uBufferID, i].phys1.velocity.v[2] = particles[rBufferID, i].phys1.velocity.v[2];
					particles[uBufferID, i].phys1.acceleration.v[0] = particles[rBufferID, i].phys1.acceleration.v[0];
					particles[uBufferID, i].phys1.acceleration.v[1] = particles[rBufferID, i].phys1.acceleration.v[1];
					particles[uBufferID, i].phys1.acceleration.v[2] = particles[rBufferID, i].phys1.acceleration.v[2];
					particles[uBufferID, i].size = particles[rBufferID, i].size;
					particles[uBufferID, i].sizeChange = particles[rBufferID, i].sizeChange;
					particles[uBufferID, i].color[0] = particles[rBufferID, i].color[0];
					particles[uBufferID, i].color[1] = particles[rBufferID, i].color[1];
					particles[uBufferID, i].color[2] = particles[rBufferID, i].color[2];
					particles[uBufferID, i].color[3] = particles[rBufferID, i].color[3];
					particles[uBufferID, i].colorChange[0] = particles[rBufferID, i].colorChange[0];
					particles[uBufferID, i].colorChange[1] = particles[rBufferID, i].colorChange[1];
					particles[uBufferID, i].colorChange[2] = particles[rBufferID, i].colorChange[2];
					particles[uBufferID, i].colorChange[3] = particles[rBufferID, i].colorChange[3];
					particles[uBufferID, i].lightID = particles[rBufferID, i].lightID;
					particles[uBufferID, i].type = particles[rBufferID, i].type;
					particles[uBufferID, i].refID = particles[rBufferID, i].refID;
					particles[uBufferID, i].soundID = particles[rBufferID, i].soundID;
					particles[uBufferID, i].lightID = particles[rBufferID, i].lightID;
					if (particles[uBufferID, i].lightID > -1)
					{
						int lightID = particles[uBufferID, i].lightID;
						particleLightColor[uBufferID, lightID, 0] = particleLightColor[rBufferID, lightID, 0];
						particleLightColor[uBufferID, lightID, 1] = particleLightColor[rBufferID, lightID, 1];
						particleLightColor[uBufferID, lightID, 2] = particleLightColor[rBufferID, lightID, 2];
					}
				}
			}
		}
		for (int i = 0; i < 50; i++)
		{
			if (solidParticles[uBufferID, i].lifeTime > 0f && solidParticles[rBufferID, i].lifeTime <= 0f)
			{
				_ = solidParticles[uBufferID, i].type;
				solidParticles[rBufferID, i].lifeTime = solidParticles[uBufferID, i].lifeTime;
				solidParticles[rBufferID, i].texID = solidParticles[uBufferID, i].texID;
				solidParticles[rBufferID, i].rotX = solidParticles[uBufferID, i].rotX;
				solidParticles[rBufferID, i].rotY = solidParticles[uBufferID, i].rotY;
				solidParticles[rBufferID, i].rotZ = solidParticles[uBufferID, i].rotZ;
				solidParticles[rBufferID, i].particleRot = solidParticles[uBufferID, i].particleRot;
				solidParticles[rBufferID, i].phys1.position.v[0] = solidParticles[uBufferID, i].phys1.position.v[0];
				solidParticles[rBufferID, i].phys1.position.v[1] = solidParticles[uBufferID, i].phys1.position.v[1];
				solidParticles[rBufferID, i].phys1.position.v[2] = solidParticles[uBufferID, i].phys1.position.v[2];
				solidParticles[rBufferID, i].phys1.velocity.v[0] = solidParticles[uBufferID, i].phys1.velocity.v[0];
				solidParticles[rBufferID, i].phys1.velocity.v[1] = solidParticles[uBufferID, i].phys1.velocity.v[1];
				solidParticles[rBufferID, i].phys1.velocity.v[2] = solidParticles[uBufferID, i].phys1.velocity.v[2];
				solidParticles[rBufferID, i].phys1.acceleration.v[0] = solidParticles[uBufferID, i].phys1.acceleration.v[0];
				solidParticles[rBufferID, i].phys1.acceleration.v[1] = solidParticles[uBufferID, i].phys1.acceleration.v[1];
				solidParticles[rBufferID, i].phys1.acceleration.v[2] = solidParticles[uBufferID, i].phys1.acceleration.v[2];
				solidParticles[rBufferID, i].size = solidParticles[uBufferID, i].size;
				solidParticles[rBufferID, i].sizeChange = solidParticles[uBufferID, i].sizeChange;
				solidParticles[rBufferID, i].color[0] = solidParticles[uBufferID, i].color[0];
				solidParticles[rBufferID, i].color[1] = solidParticles[uBufferID, i].color[1];
				solidParticles[rBufferID, i].color[2] = solidParticles[uBufferID, i].color[2];
				solidParticles[rBufferID, i].color[3] = solidParticles[uBufferID, i].color[3];
				solidParticles[rBufferID, i].colorChange[0] = solidParticles[uBufferID, i].colorChange[0];
				solidParticles[rBufferID, i].colorChange[1] = solidParticles[uBufferID, i].colorChange[1];
				solidParticles[rBufferID, i].colorChange[2] = solidParticles[uBufferID, i].colorChange[2];
				solidParticles[rBufferID, i].colorChange[3] = solidParticles[uBufferID, i].colorChange[3];
				solidParticles[rBufferID, i].lightID = solidParticles[uBufferID, i].lightID;
				solidParticles[rBufferID, i].type = solidParticles[uBufferID, i].type;
				solidParticles[rBufferID, i].modID = solidParticles[uBufferID, i].modID;
				solidParticles[rBufferID, i].soundID = solidParticles[uBufferID, i].soundID;
			}
			else if (solidParticles[rBufferID, i].lifeTime > 0f)
			{
				_ = solidParticles[rBufferID, i].type;
				solidParticles[uBufferID, i].lifeTime = solidParticles[rBufferID, i].lifeTime;
				solidParticles[uBufferID, i].texID = solidParticles[rBufferID, i].texID;
				solidParticles[uBufferID, i].rotX = solidParticles[rBufferID, i].rotX;
				solidParticles[uBufferID, i].rotY = solidParticles[rBufferID, i].rotY;
				solidParticles[uBufferID, i].rotZ = solidParticles[rBufferID, i].rotZ;
				solidParticles[uBufferID, i].particleRot = solidParticles[rBufferID, i].particleRot;
				solidParticles[uBufferID, i].phys1.position.v[0] = solidParticles[rBufferID, i].phys1.position.v[0];
				solidParticles[uBufferID, i].phys1.position.v[1] = solidParticles[rBufferID, i].phys1.position.v[1];
				solidParticles[uBufferID, i].phys1.position.v[2] = solidParticles[rBufferID, i].phys1.position.v[2];
				solidParticles[uBufferID, i].phys1.velocity.v[0] = solidParticles[rBufferID, i].phys1.velocity.v[0];
				solidParticles[uBufferID, i].phys1.velocity.v[1] = solidParticles[rBufferID, i].phys1.velocity.v[1];
				solidParticles[uBufferID, i].phys1.velocity.v[2] = solidParticles[rBufferID, i].phys1.velocity.v[2];
				solidParticles[uBufferID, i].phys1.acceleration.v[0] = solidParticles[rBufferID, i].phys1.acceleration.v[0];
				solidParticles[uBufferID, i].phys1.acceleration.v[1] = solidParticles[rBufferID, i].phys1.acceleration.v[1];
				solidParticles[uBufferID, i].phys1.acceleration.v[2] = solidParticles[rBufferID, i].phys1.acceleration.v[2];
				solidParticles[uBufferID, i].size = solidParticles[rBufferID, i].size;
				solidParticles[uBufferID, i].sizeChange = solidParticles[rBufferID, i].sizeChange;
				solidParticles[uBufferID, i].color[0] = solidParticles[rBufferID, i].color[0];
				solidParticles[uBufferID, i].color[1] = solidParticles[rBufferID, i].color[1];
				solidParticles[uBufferID, i].color[2] = solidParticles[rBufferID, i].color[2];
				solidParticles[uBufferID, i].color[3] = solidParticles[rBufferID, i].color[3];
				solidParticles[uBufferID, i].colorChange[0] = solidParticles[rBufferID, i].colorChange[0];
				solidParticles[uBufferID, i].colorChange[1] = solidParticles[rBufferID, i].colorChange[1];
				solidParticles[uBufferID, i].colorChange[2] = solidParticles[rBufferID, i].colorChange[2];
				solidParticles[uBufferID, i].colorChange[3] = solidParticles[rBufferID, i].colorChange[3];
				solidParticles[uBufferID, i].lightID = solidParticles[rBufferID, i].lightID;
				solidParticles[uBufferID, i].type = solidParticles[rBufferID, i].type;
				solidParticles[uBufferID, i].modID = solidParticles[rBufferID, i].modID;
				solidParticles[uBufferID, i].soundID = solidParticles[rBufferID, i].soundID;
			}
		}
	}

	public void Reset_Particles()
	{
		for (int i = 0; i < 1500; i++)
		{
			sbyte type = particles[rBufferID, i].type;
			if (type != 10)
			{
				particles[0, i].lifeTime = -1f;
				particles[1, i].lifeTime = -1f;
				particles[0, i].lightID = -1;
				particles[1, i].lightID = -1;
			}
		}
		soldParticleIndex = 0;
		for (int i = 0; i < 50; i++)
		{
			solidParticles[0, i].lifeTime = -1f;
			solidParticles[1, i].lifeTime = -1f;
			solidParticles[0, i].type = 0;
			solidParticles[1, i].type = 0;
		}
		for (int i = 0; i < 5; i++)
		{
			particleLights[i] = -1;
			particleLightColor[0, i, 0] = 0f;
			particleLightColor[0, i, 1] = 0f;
			particleLightColor[0, i, 2] = 0f;
			particleLightColor[0, i, 3] = 0f;
		}
	}

	public void Render_Particles()
	{
		Render_Particles_New();
		rGraphics.DepthStencilState = depthBufferWriteDisabled;
		rGraphics.BlendState = blendSourceAlpha;
		Render_Particle_Model_Setup();
		for (int i = 0; i < 1500 && !(particles[rBufferID, i].lifeTime < 0f); i++)
		{
			switch (particles[rBufferID, i].type)
			{
			case 9:
				Render_Player_Spawning_No_Effect(particles[rBufferID, i].refID, i);
				break;
			case 10:
				Render_Particle_Model(particles[rBufferID, i].refID, particles[rBufferID, i].refID2, particles[rBufferID, i].refID3);
				break;
			}
		}
		rGraphics.SetVertexBuffer(global::Models.Models.mVertexBufferObjects);
		rGraphics.Indices = global::Models.Models.mIndexBufferObjects;
		rGraphics.RasterizerState = RasterizerState.CullClockwise;
		rGraphics.DepthStencilState = depthBufferEnabled;
		effect1.Parameters["ViewProjection"].SetValue(matrixVP);
		effect1.Parameters["World"].SetValue(matrixI);
		far4[0] = 1f;
		far4[1] = 1f;
		far4[2] = 1f;
		far4[3] = 1f;
		effect1.Parameters["ColorAdjust"].SetValue(far4);
	}

	public void Render_Solid_Particles()
	{
		try
		{
			effect1.CurrentTechnique = effect1.Techniques["Main"];
			effect1.CurrentTechnique.Passes[0].Apply();
			rGraphics.BlendState = BlendState.Opaque;
			rGraphics.SetVertexBuffer(global::Models.Models.mVertexBufferObjects);
			rGraphics.Indices = global::Models.Models.mIndexBufferObjects;
			for (int i = 0; i < 50; i++)
			{
				if (solidParticles[rBufferID, i].lifeTime > 0f)
				{
					_ = particles[rBufferID, i].type;
					Matrix value = Matrix.CreateFromQuaternion(solidParticles[rBufferID, i].particleRot) * Matrix.CreateTranslation(solidParticles[rBufferID, i].phys1.position.v[0], solidParticles[rBufferID, i].phys1.position.v[1], solidParticles[rBufferID, i].phys1.position.v[2]);
					effect1.Parameters["World"].SetValue(value);
					mainC.modelsMain.Render_Solid_Particle_Model(solidParticles[rBufferID, i].modID);
				}
			}
		}
		catch (Exception)
		{
		}
	}

	public void Render_Particle_Model(int instanceID, int itemID, int modelID)
	{
		effect1.Parameters["BaseTexture"].SetValue(global::Textures.Textures.texMain.texData[global::Models.Models.modVbo[renderingInstances[instanceID].modelList[modelID]].texID]);
		effect1.Parameters["MaterialTexture"].SetValue(global::Textures.Textures.texMain.texData[global::Models.Models.modVbo[renderingInstances[instanceID].modelList[modelID]].texNormalID]);
		effect1.CurrentTechnique.Passes[0].Apply();
		mainC.modelsMain.Render_Level_Model_Basic(renderingInstances[instanceID].modelList[modelID]);
	}

	public void Render_Particle_Model_Setup()
	{
		rpmFAR[0] = 1f;
		rpmFAR[1] = 1f;
		rpmFAR[2] = 1f;
		rpmFAR[3] = 1f;
		rGraphics.RasterizerState = RasterizerState.CullNone;
		rGraphics.SetVertexBuffer(global::Models.Models.mVertexBufferObjectsLevel);
		rGraphics.Indices = global::Models.Models.mIndexBufferObjectsLevel;
		effect1.Parameters["World"].SetValue(matrixI);
		effect1.Parameters["ColorAdjust"].SetValue(rpmFAR);
		effect1.Parameters["ViewProjection"].SetValue(matrixVP);
		effect1.CurrentTechnique = effect1.Techniques["Basic"];
		effect1.CurrentTechnique.Passes[0].Apply();
	}

	public void Add_Particle(byte particleID, float posX, float posY, float posZ, float nx, float ny, float nz, float velocityX, float velocityY, float velocityZ)
	{
		if (particleID < numParticleTypes)
		{
			if (MS_Particles[particleID].particleEmitter > -1)
			{
				Add_Particle_Emitter((byte)MS_Particles[particleID].particleEmitter, posX, posY, posZ, nx, ny, nz, velocityX, velocityY, velocityZ);
			}
			else
			{
				Create_Particle(particleID, posX, posY, posZ, nx, ny, nz, velocityX, velocityY, velocityZ);
			}
		}
	}

	public void Create_Particle(byte particleID, float posX, float posY, float posZ, float nx, float ny, float nz, float velocityX, float velocityY, float velocityZ)
	{
		float num = 0f;
		float num2 = 0f;
		float num3 = 0f;
		for (ushort num4 = 0; num4 < MS_Particles[particleID].numParticles; num4++)
		{
			num = 0f;
			num2 = 0f;
			num3 = 0f;
			float num5;
			if (MS_Particles[particleID].velocity + MS_Particles[particleID].velocityVariance != 0f)
			{
				float num6;
				float num7;
				if (nz != 1f)
				{
					num5 = ny;
					num6 = 0f - nx;
					num7 = (float)Math.Sqrt(num5 * num5 + num6 * num6);
					if (num7 != 0f)
					{
						num5 /= num7;
						num6 /= num7;
					}
				}
				else
				{
					num5 = 1f;
					num6 = 0f;
				}
				float num8 = num6 * nz;
				float num9 = (0f - num5) * nz;
				float num10 = num5 * ny - num6 * nx;
				float num11 = -1f + 2f * (float)global::MainGame.MainGame.mainRandom.NextDouble();
				sbyte b = (sbyte)Math.Sign(-1.0 + 2.0 * global::MainGame.MainGame.mainRandom.NextDouble());
				if (b == 0)
				{
					b = 1;
				}
				float num12 = (float)Math.Sqrt(1f - num11 * num11) * (float)b;
				num5 = num5 * num11 + num8 * num12;
				num6 = num6 * num11 + num9 * num12;
				num7 = num10 * num12;
				num11 = ((!(MS_Particles[particleID].angleOffSetMin > -1f)) ? (0.6f + 0.2f * (float)global::MainGame.MainGame.mainRandom.NextDouble()) : (MS_Particles[particleID].angleOffSetMin + MS_Particles[particleID].range * (float)global::MainGame.MainGame.mainRandom.NextDouble()));
				num12 = (float)Math.Sqrt(1f - num11 * num11);
				num8 = MS_Particles[particleID].velocity + MS_Particles[particleID].velocityVariance * (float)global::MainGame.MainGame.mainRandom.NextDouble();
				num = (nx * num12 + num5 * num11) * num8;
				num2 = (ny * num12 + num6 * num11) * num8;
				num3 = (nz * num12 + num7 * num11) * num8;
			}
			num5 = (float)global::MainGame.MainGame.mainRandom.NextDouble() * MS_Particles[particleID].positionVariance - MS_Particles[particleID].positionVarianceHalf;
			MS_Particles[particleID].curRotation += MS_Particles[particleID].rotation;
			if (MS_Particles[particleID].curRotation > 360f)
			{
				MS_Particles[particleID].curRotation -= 360f;
			}
			AddParticle(particleID, posX + num5 * nx, posY + num5 * ny, posZ + num5 * nz, velocityX + num, velocityY + num2, velocityZ + num3, MS_Particles[particleID].curRotation);
		}
	}

	public void Add_Particle_Emitter(byte emitterID, float posX, float posY, float posZ, float nx, float ny, float nz, float velocityX, float velocityY, float velocityZ)
	{
		ushort num = nextParticleEmitter;
		while (nextParticleEmitter < numParticleEmitters)
		{
			if (particleEmitters[nextParticleEmitter].status == 0)
			{
				particleEmitters[nextParticleEmitter].status = 1;
				particleEmitters[nextParticleEmitter].particleID = emitterDefs[emitterID].particleID;
				particleEmitters[nextParticleEmitter].length = emitterDefs[emitterID].length;
				particleEmitters[nextParticleEmitter].particlesPerSecond = emitterDefs[emitterID].particlesPerSecond;
				particleEmitters[nextParticleEmitter].x = posX;
				particleEmitters[nextParticleEmitter].y = posY;
				particleEmitters[nextParticleEmitter].z = posZ;
				particleEmitters[nextParticleEmitter].nx = nx;
				particleEmitters[nextParticleEmitter].ny = ny;
				particleEmitters[nextParticleEmitter].nz = nz;
				particleEmitters[nextParticleEmitter].vx = velocityX;
				particleEmitters[nextParticleEmitter].vy = velocityY;
				particleEmitters[nextParticleEmitter].vz = velocityZ;
				particleEmitters[nextParticleEmitter].curTime = 0f;
				particleEmitters[nextParticleEmitter].count = 0;
				nextParticleEmitter++;
				return;
			}
			nextParticleEmitter++;
		}
		for (nextParticleEmitter = 0; nextParticleEmitter < num; nextParticleEmitter++)
		{
			if (particleEmitters[nextParticleEmitter].status == 0)
			{
				particleEmitters[nextParticleEmitter].status = 1;
				particleEmitters[nextParticleEmitter].particleID = emitterDefs[emitterID].particleID;
				particleEmitters[nextParticleEmitter].length = emitterDefs[emitterID].length;
				particleEmitters[nextParticleEmitter].particlesPerSecond = emitterDefs[emitterID].particlesPerSecond;
				particleEmitters[nextParticleEmitter].x = posX;
				particleEmitters[nextParticleEmitter].y = posY;
				particleEmitters[nextParticleEmitter].z = posZ;
				particleEmitters[nextParticleEmitter].nx = nx;
				particleEmitters[nextParticleEmitter].ny = ny;
				particleEmitters[nextParticleEmitter].nz = nz;
				particleEmitters[nextParticleEmitter].vx = velocityX;
				particleEmitters[nextParticleEmitter].vy = velocityY;
				particleEmitters[nextParticleEmitter].vz = velocityZ;
				particleEmitters[nextParticleEmitter].curTime = 0f;
				particleEmitters[nextParticleEmitter].count = 0;
				nextParticleEmitter++;
				break;
			}
		}
	}

	public void New_Solid_Particle(short pType, float x1, float y1, float z1, float vx, float vy, float vz, float nx, float ny, float nz, float initialVelX, float initialVelY, float initialVelZ, float velocity, float particleTime, int modID)
	{
		bool flag = false;
		Matrix identity = Matrix.Identity;
		int i;
		for (i = soldParticleIndex; i < 50; i++)
		{
			if (solidParticles[uBufferID, i].lifeTime <= 0f)
			{
				flag = true;
				break;
			}
		}
		if (!flag)
		{
			soldParticleIndex++;
			if (soldParticleIndex >= 50)
			{
				soldParticleIndex = 49;
			}
			for (i = 0; i < soldParticleIndex; i++)
			{
				if (solidParticles[uBufferID, i].lifeTime <= 0f)
				{
					flag = true;
					break;
				}
			}
		}
		if (!flag)
		{
			return;
		}
		switch (pType)
		{
		case 1:
		{
			identity.M21 = 0f - vx;
			identity.M22 = 0f - vy;
			identity.M23 = 0f - vz;
			identity.M11 = 0f - nx;
			identity.M12 = 0f - ny;
			identity.M13 = 0f - nz;
			identity.M31 = ny * vz - nz * vy;
			identity.M32 = nz * vx - nx * vz;
			identity.M33 = nx * vy - ny * vx;
			float num = -1f + (float)global::MainGame.MainGame.mainRandom.NextDouble() * 2f;
			float num2 = -1f + (float)global::MainGame.MainGame.mainRandom.NextDouble() * 2f;
			identity = Matrix.CreateRotationX(num2 * ((float)Math.PI / 4f)) * Matrix.CreateRotationY(num * (float)Math.PI) * identity;
			solidParticles[uBufferID, i].phys1.velocity.v[0] = initialVelX + identity.M21 * velocity;
			solidParticles[uBufferID, i].phys1.velocity.v[1] = initialVelY + identity.M22 * velocity;
			solidParticles[uBufferID, i].phys1.velocity.v[2] = initialVelZ + identity.M23 * velocity;
			solidParticles[uBufferID, i].type = 1;
			solidParticles[uBufferID, i].rotX = 0f;
			solidParticles[uBufferID, i].rotY = 0f;
			solidParticles[uBufferID, i].rotZ = 0f;
			solidParticles[uBufferID, i].particleRot = Quaternion.CreateFromYawPitchRoll(0f, 0f, 0f);
			solidParticles[uBufferID, i].modID = modID;
			solidParticleRotation1 += 17f;
			solidParticles[uBufferID, i].phys1.position.v[0] = x1;
			solidParticles[uBufferID, i].phys1.position.v[1] = y1;
			solidParticles[uBufferID, i].phys1.position.v[2] = z1;
			solidParticles[uBufferID, i].phys1.acceleration.v[0] = 0f;
			solidParticles[uBufferID, i].phys1.acceleration.v[1] = 0f;
			solidParticles[uBufferID, i].phys1.acceleration.v[2] = 0f;
			solidParticles[uBufferID, i].lifeTime = particleTime;
			solidParticles[uBufferID, i].fadeOutTimer = particleTime;
			soldParticleIndex = (ushort)(i + 1);
			if (soldParticleIndex >= 50)
			{
				soldParticleIndex = 0;
			}
			break;
		}
		case 2:
		{
			identity.M21 = 0f - vx;
			identity.M22 = 0f - vy;
			identity.M23 = 0f - vz;
			identity.M11 = 0f - nx;
			identity.M12 = 0f - ny;
			identity.M13 = 0f - nz;
			identity.M31 = ny * vz - nz * vy;
			identity.M32 = nz * vx - nx * vz;
			identity.M33 = nx * vy - ny * vx;
			float num = -1f + (float)global::MainGame.MainGame.mainRandom.NextDouble() * 2f;
			float num2 = -1f + (float)global::MainGame.MainGame.mainRandom.NextDouble() * 2f;
			identity = Matrix.CreateRotationX(num2 * ((float)Math.PI / 4f)) * Matrix.CreateRotationY(num * (float)Math.PI) * identity;
			solidParticles[uBufferID, i].phys1.velocity.v[0] = initialVelX + identity.M21 * velocity;
			solidParticles[uBufferID, i].phys1.velocity.v[1] = initialVelY + identity.M22 * velocity;
			solidParticles[uBufferID, i].phys1.velocity.v[2] = initialVelZ + identity.M23 * velocity;
			solidParticles[uBufferID, i].type = 1;
			solidParticles[uBufferID, i].rotX = -3f + (float)global::MainGame.MainGame.mainRandom.NextDouble() * 6f;
			solidParticles[uBufferID, i].rotY = -3f + (float)global::MainGame.MainGame.mainRandom.NextDouble() * 6f;
			solidParticles[uBufferID, i].rotZ = -3f + (float)global::MainGame.MainGame.mainRandom.NextDouble() * 6f;
			solidParticles[uBufferID, i].particleRot = Quaternion.CreateFromYawPitchRoll(0f, 0f, 0f);
			solidParticles[uBufferID, i].modID = modID;
			solidParticleRotation1 += 17f;
			solidParticles[uBufferID, i].phys1.position.v[0] = x1 + identity.M21;
			solidParticles[uBufferID, i].phys1.position.v[1] = y1 + identity.M22;
			solidParticles[uBufferID, i].phys1.position.v[2] = z1 + identity.M23;
			solidParticles[uBufferID, i].phys1.acceleration.v[0] = 0f;
			solidParticles[uBufferID, i].phys1.acceleration.v[1] = 0f;
			solidParticles[uBufferID, i].phys1.acceleration.v[2] = -32.15223f;
			solidParticles[uBufferID, i].lifeTime = particleTime;
			solidParticles[uBufferID, i].fadeOutTimer = particleTime;
			soldParticleIndex = (ushort)(i + 1);
			if (soldParticleIndex >= 50)
			{
				soldParticleIndex = 0;
			}
			break;
		}
		case 3:
		{
			float num = (0.75f + 0.25f * (float)global::MainGame.MainGame.mainRandom.NextDouble()) * velocity;
			solidParticles[uBufferID, i].phys1.velocity.v[0] = initialVelX + vx * num;
			solidParticles[uBufferID, i].phys1.velocity.v[1] = initialVelY + vy * num;
			solidParticles[uBufferID, i].phys1.velocity.v[2] = initialVelZ + vz * num;
			solidParticles[uBufferID, i].type = 1;
			solidParticles[uBufferID, i].rotX = -8f + (float)global::MainGame.MainGame.mainRandom.NextDouble() * 16f;
			solidParticles[uBufferID, i].rotY = -8f + (float)global::MainGame.MainGame.mainRandom.NextDouble() * 16f;
			solidParticles[uBufferID, i].rotZ = -8f + (float)global::MainGame.MainGame.mainRandom.NextDouble() * 16f;
			solidParticles[uBufferID, i].particleRot = Quaternion.CreateFromYawPitchRoll(0f, 0f, 0f);
			solidParticles[uBufferID, i].modID = modID;
			solidParticleRotation1 += 17f;
			solidParticles[uBufferID, i].phys1.position.v[0] = x1 + vx;
			solidParticles[uBufferID, i].phys1.position.v[1] = y1 + vy;
			solidParticles[uBufferID, i].phys1.position.v[2] = z1 + vz;
			solidParticles[uBufferID, i].phys1.acceleration.v[0] = 0f;
			solidParticles[uBufferID, i].phys1.acceleration.v[1] = 0f;
			solidParticles[uBufferID, i].phys1.acceleration.v[2] = -32.15223f;
			solidParticles[uBufferID, i].lifeTime = particleTime;
			solidParticles[uBufferID, i].fadeOutTimer = particleTime;
			soldParticleIndex = (ushort)(i + 1);
			if (soldParticleIndex >= 50)
			{
				soldParticleIndex = 0;
			}
			break;
		}
		}
	}

	public void New_Solid_Particle_From_Player_Vehicle_Explosion(ushort vhID, int modID)
	{
		bool flag = false;
		Matrix identity = Matrix.Identity;
		switch (global::MainGame.MainGame.playerVehicles[vhID].type)
		{
		case 1:
		case 6:
		case 7:
		{
			int i;
			for (i = soldParticleIndex; i < 50; i++)
			{
				if (solidParticles[uBufferID, i].lifeTime <= 0f)
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				soldParticleIndex++;
				if (soldParticleIndex >= 50)
				{
					soldParticleIndex = 50;
					soldParticleIndex--;
				}
				for (i = 0; i < soldParticleIndex; i++)
				{
					if (solidParticles[uBufferID, i].lifeTime <= 0f)
					{
						flag = true;
						break;
					}
				}
			}
			if (flag)
			{
				identity = global::MainGame.MainGame.playerVehicles[vhID].mv[uBufferID];
				float num = -1f + (float)global::MainGame.MainGame.mainRandom.Next(0, 32000) / 32000f * 2f;
				float num2 = -1f + (float)global::MainGame.MainGame.mainRandom.Next(0, 32000) / 32000f * 2f;
				identity = Matrix.CreateRotationX(num2 * ((float)Math.PI / 12f)) * Matrix.CreateRotationY(num * (float)Math.PI) * identity;
				num = global::MainGame.MainGame.playerVehicles[vhID].ph1.velocity * 0.75f;
				solidParticles[uBufferID, i].phys1.velocity.v[0] = identity.M21 * num;
				solidParticles[uBufferID, i].phys1.velocity.v[1] = identity.M22 * num;
				solidParticles[uBufferID, i].phys1.velocity.v[2] = identity.M23 * num;
				solidParticles[uBufferID, i].type = 1;
				solidParticles[uBufferID, i].rotX = -2f + (float)global::MainGame.MainGame.mainRandom.Next(0, 32000) / 32000f * 9f;
				solidParticles[uBufferID, i].rotY = -2f + (float)global::MainGame.MainGame.mainRandom.Next(0, 32000) / 32000f * 4f;
				solidParticles[uBufferID, i].rotZ = -2f + (float)global::MainGame.MainGame.mainRandom.Next(0, 32000) / 32000f * 4f;
				solidParticles[uBufferID, i].particleRot = Quaternion.CreateFromYawPitchRoll(0f, 0f, 0f);
				solidParticles[uBufferID, i].modID = modID;
				solidParticleRotation1 += 17f;
				solidParticles[uBufferID, i].phys1.position.v[0] = global::MainGame.MainGame.playerVehicles[vhID].ph1.x;
				solidParticles[uBufferID, i].phys1.position.v[1] = global::MainGame.MainGame.playerVehicles[vhID].ph1.y;
				solidParticles[uBufferID, i].phys1.position.v[2] = global::MainGame.MainGame.playerVehicles[vhID].ph1.z;
				solidParticles[uBufferID, i].phys1.acceleration.v[0] = 0f;
				solidParticles[uBufferID, i].phys1.acceleration.v[1] = 0f;
				solidParticles[uBufferID, i].phys1.acceleration.v[2] = -32.15223f;
				solidParticles[uBufferID, i].lifeTime = 2f;
				solidParticles[uBufferID, i].fadeOutTimer = 2f;
				soldParticleIndex = (ushort)(i + 1);
				if (soldParticleIndex >= 50)
				{
					soldParticleIndex = 0;
				}
			}
			break;
		}
		}
	}

	public void New_Particle(short pType, ref StructsClass.vtex v1, ref StructsClass.vtex v2, ref StructsClass.vtex vUp, ref StructsClass.vtex vRight, int refID, byte threadID)
	{
	}

	public void New_Particle_New(short pType, float x1, float y1, float z1, float x2, float y2, float z2, int refID, byte threadID)
	{
	}

	public int New_Particle_Spawn(short pType, int particleID, int playerID, float x, float y, float z)
	{
		if (particleID == -1 || particles[rBufferID, particleID].type != pType || particles[rBufferID, particleID].lifeTime <= 0f || particles[rBufferID, particleID].refID != playerID)
		{
			switch (pType)
			{
			case 8:
			{
				for (int i = 0; i < 1500; i++)
				{
					if (particles[uBufferID, i].lifeTime <= 0f)
					{
						particles[uBufferID, i].lifeTime = 5f;
						particles[uBufferID, i].type = 8;
						particles[uBufferID, i].lightID = -1;
						particles[uBufferID, i].rotation = 0f;
						particles[uBufferID, i].refID = playerID;
						particles[rBufferID, i].lifeTime = 5f;
						particles[rBufferID, i].type = 8;
						particles[rBufferID, i].lightID = -1;
						particles[rBufferID, i].rotation = 0f;
						particles[rBufferID, i].refID = playerID;
						particles[uBufferID, i].phys1.position.v[0] = x;
						particles[uBufferID, i].phys1.position.v[1] = y;
						particles[uBufferID, i].phys1.position.v[2] = z;
						if (global::Players.Players.players[playerID].transporterDirection > 0)
						{
							particles[uBufferID, i].soundID = mainC.soundsMain.Play_Priority_Sound(global::Players.Players.playerRaces[global::Players.Players.players[playerID].race].teleportInSound, x, y, z, 0f, 0f, 0f);
						}
						else
						{
							particles[uBufferID, i].soundID = mainC.soundsMain.Play_Priority_Sound(global::Players.Players.playerRaces[global::Players.Players.players[playerID].race].teleportOutSound, x, y, z, 0f, 0f, 0f);
						}
						global::Players.Players.players[playerID].timeBeforeRespawn[uBufferID] = 2f;
						return i;
					}
				}
				for (int i = 0; i < 1500; i++)
				{
					switch ((byte)particles[uBufferID, i].type)
					{
					case 8:
					case 9:
					case 10:
						continue;
					}
					particles[uBufferID, i].lifeTime = 5f;
					particles[uBufferID, i].type = 8;
					particles[uBufferID, i].lightID = -1;
					particles[uBufferID, i].rotation = 0f;
					particles[uBufferID, i].refID = playerID;
					particles[rBufferID, i].lifeTime = 5f;
					particles[rBufferID, i].type = 8;
					particles[rBufferID, i].lightID = -1;
					particles[rBufferID, i].rotation = 0f;
					particles[rBufferID, i].refID = playerID;
					particles[uBufferID, i].phys1.position.v[0] = x;
					particles[uBufferID, i].phys1.position.v[1] = y;
					particles[uBufferID, i].phys1.position.v[2] = z;
					if (global::Players.Players.players[playerID].transporterDirection > 0)
					{
						particles[uBufferID, i].soundID = mainC.soundsMain.Play_Priority_Sound(global::Players.Players.playerRaces[global::Players.Players.players[playerID].race].teleportInSound, x, y, z, 0f, 0f, 0f);
					}
					else
					{
						particles[uBufferID, i].soundID = mainC.soundsMain.Play_Priority_Sound(global::Players.Players.playerRaces[global::Players.Players.players[playerID].race].teleportOutSound, x, y, z, 0f, 0f, 0f);
					}
					global::Players.Players.players[playerID].timeBeforeRespawn[uBufferID] = 2f;
					return i;
				}
				return -1;
			}
			case 9:
			{
				for (int i = 0; i < 1500; i++)
				{
					if (particles[uBufferID, i].lifeTime <= 0f)
					{
						particles[uBufferID, i].lifeTime = 5f;
						particles[uBufferID, i].type = 9;
						particles[uBufferID, i].lightID = -1;
						particles[uBufferID, i].refID = playerID;
						particles[rBufferID, i].lifeTime = 5f;
						particles[rBufferID, i].type = 9;
						particles[rBufferID, i].lightID = -1;
						particles[rBufferID, i].refID = playerID;
						if (global::Players.Players.players[playerID].transporterDirection > 0)
						{
							particles[uBufferID, i].soundID = mainC.soundsMain.Play_Priority_Sound(global::Players.Players.playerRaces[global::Players.Players.players[playerID].race].teleportInSound, x, y, z, 0f, 0f, 0f);
						}
						else
						{
							particles[uBufferID, i].soundID = mainC.soundsMain.Play_Priority_Sound(global::Players.Players.playerRaces[global::Players.Players.players[playerID].race].teleportOutSound, x, y, z, 0f, 0f, 0f);
						}
						global::Players.Players.players[playerID].timeBeforeRespawn[uBufferID] = 0f;
						return i;
					}
				}
				for (int i = 0; i < 1500; i++)
				{
					switch ((byte)particles[uBufferID, i].type)
					{
					case 8:
					case 9:
					case 10:
						continue;
					}
					particles[uBufferID, i].lifeTime = 5f;
					particles[uBufferID, i].type = 9;
					particles[uBufferID, i].lightID = -1;
					particles[uBufferID, i].refID = playerID;
					particles[rBufferID, i].lifeTime = 5f;
					particles[rBufferID, i].type = 9;
					particles[rBufferID, i].lightID = -1;
					particles[rBufferID, i].refID = playerID;
					if (global::Players.Players.players[playerID].transporterDirection > 0)
					{
						particles[uBufferID, i].soundID = mainC.soundsMain.Play_Priority_Sound(global::Players.Players.playerRaces[global::Players.Players.players[playerID].race].teleportInSound, x, y, z, 0f, 0f, 0f);
					}
					else
					{
						particles[uBufferID, i].soundID = mainC.soundsMain.Play_Priority_Sound(global::Players.Players.playerRaces[global::Players.Players.players[playerID].race].teleportOutSound, x, y, z, 0f, 0f, 0f);
					}
					global::Players.Players.players[playerID].timeBeforeRespawn[uBufferID] = 0f;
					return i;
				}
				return -1;
			}
			}
		}
		else
		{
			switch (pType)
			{
			case 8:
				particles[uBufferID, particleID].lifeTime = 5f;
				particles[uBufferID, particleID].type = 8;
				particles[uBufferID, particleID].lightID = -1;
				particles[uBufferID, particleID].rotation = 0f;
				particles[uBufferID, particleID].refID = playerID;
				particles[rBufferID, particleID].lifeTime = 5f;
				particles[rBufferID, particleID].type = 8;
				particles[rBufferID, particleID].lightID = -1;
				particles[rBufferID, particleID].rotation = 0f;
				particles[rBufferID, particleID].refID = playerID;
				particles[uBufferID, particleID].phys1.position.v[0] = x;
				particles[uBufferID, particleID].phys1.position.v[1] = y;
				particles[uBufferID, particleID].phys1.position.v[2] = z;
				global::Players.Players.players[playerID].timeBeforeRespawn[uBufferID] = 2f;
				return particleID;
			case 9:
				particles[uBufferID, particleID].lifeTime = 5f;
				particles[uBufferID, particleID].type = 9;
				particles[uBufferID, particleID].lightID = -1;
				particles[uBufferID, particleID].refID = playerID;
				particles[rBufferID, particleID].lifeTime = 5f;
				particles[rBufferID, particleID].type = 9;
				particles[rBufferID, particleID].lightID = -1;
				particles[rBufferID, particleID].refID = playerID;
				global::Players.Players.players[playerID].timeBeforeRespawn[uBufferID] = 0f;
				return particleID;
			}
		}
		return -1;
	}

	public void New_Particle_Model(short pType, float x1, float y1, float z1, int instanceID, short itemID, short modelID)
	{
		short num = pType;
		if (num != 10)
		{
			return;
		}
		for (int i = 0; i < 1500; i++)
		{
			if (particles[uBufferID, i].lifeTime <= 0f)
			{
				particles[uBufferID, i].type = 10;
				particles[uBufferID, i].refID = instanceID;
				particles[uBufferID, i].refID2 = itemID;
				particles[uBufferID, i].refID3 = modelID;
				particles[uBufferID, i].lifeTime = 1f;
				particles[uBufferID, i].phys1.position.v[0] = x1;
				particles[uBufferID, i].phys1.position.v[1] = y1;
				particles[uBufferID, i].phys1.position.v[2] = z1;
				particles[rBufferID, i].type = 10;
				particles[rBufferID, i].cullRadius = 15;
				particles[rBufferID, i].refID = instanceID;
				particles[rBufferID, i].refID2 = itemID;
				particles[rBufferID, i].refID3 = modelID;
				particles[rBufferID, i].lifeTime = 1f;
				particles[rBufferID, i].phys1.position.v[0] = x1;
				particles[rBufferID, i].phys1.position.v[1] = y1;
				particles[rBufferID, i].phys1.position.v[2] = z1;
				break;
			}
		}
	}

	public void Clear_Model_Particles()
	{
		for (int i = 0; i < 1500; i++)
		{
			if (particles[0, i].type == 10 || particles[1, i].type == 10)
			{
				particles[0, i].lifeTime = -1f;
				particles[1, i].lifeTime = -1f;
				particles[0, i].type = 0;
				particles[1, i].type = 0;
			}
		}
	}

	public void Set_Particle_Light(int particleID, float distance)
	{
		switch (nextParticleLight)
		{
		case 0:
			if (particleLights[0] != -1)
			{
				particles[uBufferID, particleLights[0]].lightID = -1;
			}
			particleLights[0] = (short)particleID;
			particles[uBufferID, particleID].lightID = 0;
			particleLightLocation[uBufferID, 0, 0] = fau3[0];
			particleLightLocation[uBufferID, 0, 1] = fau3[1];
			particleLightLocation[uBufferID, 0, 2] = fau3[2];
			particleLightColor[uBufferID, 0, 0] = fau4[0];
			particleLightColor[uBufferID, 0, 1] = fau4[1];
			particleLightColor[uBufferID, 0, 2] = fau4[2];
			particleLightColor[uBufferID, 0, 3] = fau4[3];
			particleDistance[uBufferID, 0] = distance;
			nextParticleLight = 1;
			break;
		case 1:
			if (particleLights[1] != -1)
			{
				particles[uBufferID, particleLights[1]].lightID = -1;
			}
			particleLights[1] = (short)particleID;
			particles[uBufferID, particleID].lightID = 1;
			particleLightLocation[uBufferID, 1, 0] = fau3[0];
			particleLightLocation[uBufferID, 1, 1] = fau3[1];
			particleLightLocation[uBufferID, 1, 2] = fau3[2];
			particleLightColor[uBufferID, 1, 0] = fau4[0];
			particleLightColor[uBufferID, 1, 1] = fau4[1];
			particleLightColor[uBufferID, 1, 2] = fau4[2];
			particleLightColor[uBufferID, 1, 3] = fau4[3];
			particleDistance[uBufferID, 1] = distance;
			nextParticleLight = 2;
			break;
		case 2:
			if (particleLights[2] != -1)
			{
				particles[uBufferID, particleLights[2]].lightID = -1;
			}
			particleLights[2] = (short)particleID;
			particles[uBufferID, particleID].lightID = 2;
			particleLightLocation[uBufferID, 2, 0] = fau3[0];
			particleLightLocation[uBufferID, 2, 1] = fau3[1];
			particleLightLocation[uBufferID, 2, 2] = fau3[2];
			particleLightColor[uBufferID, 2, 0] = fau4[0];
			particleLightColor[uBufferID, 2, 1] = fau4[1];
			particleLightColor[uBufferID, 2, 2] = fau4[2];
			particleLightColor[uBufferID, 2, 3] = fau4[3];
			particleDistance[uBufferID, 2] = distance;
			nextParticleLight = 3;
			break;
		case 3:
			if (particleLights[3] != -1)
			{
				particles[uBufferID, particleLights[3]].lightID = -1;
			}
			particleLights[3] = (short)particleID;
			particles[uBufferID, particleID].lightID = 3;
			particleLightLocation[uBufferID, 3, 0] = fau3[0];
			particleLightLocation[uBufferID, 3, 1] = fau3[1];
			particleLightLocation[uBufferID, 3, 2] = fau3[2];
			particleLightColor[uBufferID, 3, 0] = fau4[0];
			particleLightColor[uBufferID, 3, 1] = fau4[1];
			particleLightColor[uBufferID, 3, 2] = fau4[2];
			particleLightColor[uBufferID, 3, 3] = fau4[3];
			particleDistance[uBufferID, 3] = distance;
			nextParticleLight = 4;
			break;
		case 4:
			if (particleLights[4] != -1)
			{
				particles[uBufferID, particleLights[4]].lightID = -1;
			}
			particleLights[4] = (short)particleID;
			particles[uBufferID, particleID].lightID = 4;
			particleLightLocation[uBufferID, 4, 0] = fau3[0];
			particleLightLocation[uBufferID, 4, 1] = fau3[1];
			particleLightLocation[uBufferID, 4, 2] = fau3[2];
			particleLightColor[uBufferID, 4, 0] = fau4[0];
			particleLightColor[uBufferID, 4, 1] = fau4[1];
			particleLightColor[uBufferID, 4, 2] = fau4[2];
			particleLightColor[uBufferID, 4, 3] = fau4[3];
			particleDistance[uBufferID, 4] = distance;
			nextParticleLight = 0;
			break;
		}
	}

	public void Add_Camera_Shake(float amount, float variance)
	{
		if (viewPositionX == 0f)
		{
			viewPositionX = amount + variance * (float)global::MainGame.MainGame.mainRandom.NextDouble();
			viewVelocityX = Math.Abs(viewPositionX) / 0.083f * (float)(-Math.Sign(viewPositionX));
		}
		if (viewPositionY == 0f)
		{
			viewPositionY = amount + variance * (float)global::MainGame.MainGame.mainRandom.NextDouble();
			viewVelocityY = Math.Abs(viewPositionY) / 0.083f * (float)(-Math.Sign(viewPositionY));
		}
	}

	public void Add_Muzzle_Flash(ushort playerID, float time, float fadeOutTime, float x, float y, float z)
	{
		muzzleFlashes[uBufferID, playerID].timeRemaining = time;
		muzzleFlashes[uBufferID, playerID].fadeoutTime = fadeOutTime;
		muzzleFlashes[uBufferID, playerID].x = x;
		muzzleFlashes[uBufferID, playerID].y = y;
		muzzleFlashes[uBufferID, playerID].z = z;
		muzzleFlashes[uBufferID, playerID].textureIndex = global::Weapons.Weapons.curMuzzleFlashTexture++;
		if (global::Weapons.Weapons.curMuzzleFlashTexture >= global::Weapons.Weapons.numMuzzleFlashTexturesMainPlayer)
		{
			global::Weapons.Weapons.curMuzzleFlashTexture = 0;
		}
	}

	public void Update_Rendering_Textures()
	{
		for (short num = 0; num < numParticleTypes; num++)
		{
			MS_Particles[num].textureID = (ushort)mainC.texturesMain.Find_Texture(MS_Particles[num].texture, 0);
		}
		for (short num = 0; num < numMiniMapItems; num++)
		{
			mapItems[num].texID = (ushort)mainC.texturesMain.Find_Texture(mapItems[num].texture, 0);
		}
		for (short num = 0; num < numHitIndicatorTextures; num++)
		{
			hitIndicatorTextureIDs[num] = (ushort)mainC.texturesMain.Find_Texture(hitIndicatorTextures[num], 0);
		}
	}

	public void Add_Hit_Indicator(float rotation)
	{
		hitIndicators[currentHitIndicator].curTime = hitIndicatorConfig.starTime;
		hitIndicators[currentHitIndicator].rotation = rotation;
	}

	public void Reset_Round_HitIndicators()
	{
		for (ushort num = 0; num < numHitIndicators; num++)
		{
			hitIndicators[num].curTime = 0f;
		}
		currentHitIndicator = 0;
	}

	public void Game_Over_Cleanup()
	{
		hitColor[3] = 0f;
		for (ushort num = 0; num < global::MainGame.MainGame.maxGamePlayers; num++)
		{
			muzzleFlashes[0, num].timeRemaining = 0f;
			muzzleFlashes[1, num].timeRemaining = 0f;
		}
	}

	public void Created_Rigged_Model_Texture_List()
	{
		numPlayerModelTextures = 0;
		for (ushort num = 0; num < Vehicles.numVehicles; num++)
		{
			ushort num2 = 0;
			ushort numModels = Vehicles.vehicles[num].numModels;
			while (num2 < numModels)
			{
				numPlayerModelTextures += (ushort)(global::Models.Models.mod1[Vehicles.vehicles[num].vehicleModel[num2]].numTextures + Vehicles.vehicles[num].numAlternateTextures);
				num2++;
			}
		}
		ushort[] array = new ushort[numPlayerModelTextures];
		numPlayerModelTextures = 0;
		for (ushort num = 0; num < Vehicles.numVehicles; num++)
		{
			ushort num2 = 0;
			ushort numModels = Vehicles.vehicles[num].numModels;
			while (num2 < numModels)
			{
				for (ushort num3 = 0; num3 < global::Models.Models.mod1[Vehicles.vehicles[num].vehicleModel[num2]].numTextures; num3++)
				{
					bool flag = false;
					for (ushort num4 = 0; num4 < numPlayerModelTextures; num4++)
					{
						if (array[num4] == global::Models.Models.mod1[Vehicles.vehicles[num].vehicleModel[num2]].textureList[num3])
						{
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						array[numPlayerModelTextures++] = (ushort)global::Models.Models.mod1[Vehicles.vehicles[num].vehicleModel[num2]].textureList[num3];
					}
				}
				num2++;
			}
			num2 = 0;
			numModels = Vehicles.vehicles[num].numAlternateTextures;
			while (num2 < numModels)
			{
				bool flag = false;
				for (ushort num4 = 0; num4 < numPlayerModelTextures; num4++)
				{
					if (array[num4] == Vehicles.vehicles[num].alternateTextureIDs[num2])
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					array[numPlayerModelTextures++] = Vehicles.vehicles[num].alternateTextureIDs[num2];
				}
				num2++;
			}
		}
		playerModelTextures = new ushort[numPlayerModelTextures];
		for (ushort num = 0; num < numPlayerModelTextures; num++)
		{
			playerModelTextures[num] = array[num];
		}
	}

	public void Set_Brightness()
	{
		if (brightness < 0.5f)
		{
			float value = 1f - (1f - brightness / 0.5f) * 0.5f;
			float num = 0f;
			effect1.Parameters["Brightness"].SetValue(value);
			effect1.Parameters["BrightnessAdj"].SetValue(0f);
		}
		else
		{
			float value = 1f - (brightness - 0.5f) / 0.5f * 0.5f;
			float num = 1f - value;
			effect1.Parameters["Brightness"].SetValue(value);
			effect1.Parameters["BrightnessAdj"].SetValue(num);
		}
	}

	public void Sync_Rendering_Variables()
	{
		eyeVec[uBufferID].v[0] = eyeVec[rBufferID].v[0];
		eyeVec[uBufferID].v[1] = eyeVec[rBufferID].v[1];
		eyeVec[uBufferID].v[2] = eyeVec[rBufferID].v[2];
	}

	public void Reset_Round()
	{
		global::MainGame.MainGame.showCrossHairs[3] = 0;
		matrixVInverse = Matrix.Identity;
		viewPositionX = 0f;
		viewPositionY = 0f;
		viewVelocityX = 0f;
		viewVelocityY = 0f;
		watchedPlayerIsInvalid = false;
		mbRespawn = true;
		watchingPlayer = 0;
		moveViewToNewLocation = false;
		satelliteCrossHairX[0] = 0f;
		satelliteCrossHairY[0] = 0f;
		satelliteCrossHairX[1] = 0f;
		satelliteCrossHairY[1] = 0f;
		Reset_Particles();
		Reset_New_Particles();
		Reset_Round_MiniMap();
		Reset_Round_HitIndicators();
		renderMainPlayer[0] = true;
		renderMainPlayer[1] = true;
		popUps[0] = 0;
		ref Vector3 reference = ref camUp[0];
		reference = worldUp;
		ref Vector3 reference2 = ref camUp[1];
		reference2 = worldUp;
		hitColor[3] = 0f;
		mbRespawn = true;
		if (global::Players.Players.players[0].weaponList[0] > -1)
		{
			hudIcon = global::Weapons.Weapons.wp1[global::Players.Players.players[0].weaponList[0]].hudIcon;
		}
		mainC.renderingMain.Set_Point_Light_Color(0, 0f, 0f, 0f, 0f);
	}

	public void Reset_New_Particles()
	{
		nextParticleEmitter = 0;
		for (ushort num = 0; num < numParticleEmitters; num++)
		{
			particleEmitters[num].status = 0;
		}
		for (ushort num = 0; num < numParticleTypes; num++)
		{
			MS_Particles[num].firstActiveParticle = 0;
			MS_Particles[num].firstFreeParticle = 0;
			MS_Particles[num].firstNewParticle = 0;
			MS_Particles[num].firstRetiredParticle = 0;
			MS_Particles[num].drawCounter = 0;
			MS_Particles[num].currentTime = 0f;
		}
	}

	public void Handle_Screen_Resize()
	{
		hsrC1.R = (byte)(hitColor[0] * 255f);
		hsrC1.G = (byte)(hitColor[1] * 255f);
		hsrC1.B = (byte)(hitColor[2] * 255f);
		hsrC1.A = (byte)(hitColor[3] * 255f);
		aspectRatio = (float)rGraphics.Viewport.Width / (float)rGraphics.Viewport.Height;
		middleOfScreenX = (float)rGraphics.Viewport.Width / 2f;
		middleOfScreenY = (float)rGraphics.Viewport.Height / 2f;
		middleOfScreenLenghtToCorner = (float)Math.Sqrt(middleOfScreenX * middleOfScreenX + middleOfScreenY * middleOfScreenY);
		matrixP = Matrix.CreatePerspectiveFieldOfView((float)Math.PI / 4f, aspectRatio, 0.1f, 10000f);
		matrixO = Matrix.CreateOrthographic(rGraphics.Viewport.Width, rGraphics.Viewport.Height, 0.01f, 10000f);
		matrixVP = matrixV * matrixP;
		rEffect.Projection = matrixP;
		playerHitVtex[0].Set_Values(-global::MainGame.MainGame.width, -global::MainGame.MainGame.height, -1f, 0f, 0f, 1f, 1f, 0f, 0f, 0f, 0f, hsrC1.R, hsrC1.G, hsrC1.B, hsrC1.A);
		playerHitVtex[1].Set_Values(global::MainGame.MainGame.width, -global::MainGame.MainGame.height, -1f, 0f, 0f, 1f, 1f, 0f, 0f, 0f, 0f, hsrC1.R, hsrC1.G, hsrC1.B, hsrC1.A);
		playerHitVtex[2].Set_Values(global::MainGame.MainGame.width, global::MainGame.MainGame.height, -1f, 0f, 0f, 1f, 1f, 0f, 0f, 0f, 0f, hsrC1.R, hsrC1.G, hsrC1.B, hsrC1.A);
		playerHitVtex[3].Set_Values(-global::MainGame.MainGame.width, global::MainGame.MainGame.height, -1f, 0f, 0f, 1f, 1f, 0f, 0f, 0f, 0f, hsrC1.R, hsrC1.G, hsrC1.B, hsrC1.A);
	}

	public void Set_Point_Light_Color(byte lightID, float R, float G, float B, float A)
	{
		splFar4[0] = R;
		splFar4[1] = G;
		splFar4[2] = B;
		splFar4[3] = A;
		effect1.Parameters["PtLightColor" + lightID].SetValue(splFar4);
	}

	public void Set_Level_Lights()
	{
		int num = numPtLight_lvl;
		if (num > 2)
		{
			num = 2;
		}
		effect1.Parameters["numLevelLights"].SetValue(num);
		for (int i = 0; i < num; i++)
		{
			slFar4a[i].X = ptLight_lvl[i, 0];
			slFar4a[i].Y = ptLight_lvl[i, 1];
			slFar4a[i].Z = ptLight_lvl[i, 2];
			slFar4a[i].W = ptLightDistance_lvl[i, 0];
			slFar4b[i].X = ptLightColor_lvl[i, 0];
			slFar4b[i].Y = ptLightColor_lvl[i, 1];
			slFar4b[i].Z = ptLightColor_lvl[i, 2];
			slFar4b[i].W = ptLightColor_lvl[i, 3];
		}
		effect1.Parameters["PtLight_Lvl"].SetValue(slFar4a);
		effect1.Parameters["PtLightColor_Lvl"].SetValue(slFar4b);
	}

	public void Set_Level_Lights_Closest_To_Player()
	{
		int num = numPtLight_lvl;
		if (num > 2)
		{
			num = 2;
		}
		effect1.Parameters["numLevelLights"].SetValue(num);
		for (int i = 0; i < num; i++)
		{
			short num2 = closestLevelLightsIndices[rBufferID, i];
			slFar4a[i].X = ptLight_lvl[num2, 0];
			slFar4a[i].Y = ptLight_lvl[num2, 1];
			slFar4a[i].Z = ptLight_lvl[num2, 2];
			slFar4a[i].W = ptLightDistance_lvl[num2, 0];
			slFar4b[i].X = ptLightColor_lvl[num2, 0];
			slFar4b[i].Y = ptLightColor_lvl[num2, 1];
			slFar4b[i].Z = ptLightColor_lvl[num2, 2];
			slFar4b[i].W = ptLightColor_lvl[num2, 3];
		}
		effect1.Parameters["PtLight_Lvl"].SetValue(slFar4a);
		effect1.Parameters["PtLightColor_Lvl"].SetValue(slFar4b);
	}

	public void Set_Camera_To_Initial_View()
	{
		camPos[0].X = initialCamPosX;
		camPos[1].X = initialCamPosX;
		camPos[0].Y = initialCamPosY;
		camPos[1].Y = initialCamPosY;
		camPos[0].Z = initialCamPosZ;
		camPos[1].Z = initialCamPosZ;
		camObject[0].X = initialCamObjX;
		camObject[1].X = initialCamObjX;
		camObject[0].Y = initialCamObjY;
		camObject[1].Y = initialCamObjY;
		camObject[0].Z = initialCamObjZ;
		camObject[1].Z = initialCamObjZ;
		camUp[0].X = initialWorldX;
		camUp[1].X = initialWorldX;
		camUp[0].Y = initialWorldY;
		camUp[1].Y = initialWorldY;
		camUp[0].Z = initialWorldZ;
		camUp[1].Z = initialWorldZ;
		global::Players.Players.cameraRotationZ = initialCameraRotation;
	}

	public void Verify_Watched_Player_Is_Valid()
	{
		if (watchingPlayer != 0 && ((global::Players.Players.players[watchingPlayer].onmap & 0xC) == 0 || !global::Players.Players.players[watchingPlayer].active))
		{
			watchingPlayerTimer += global::MainGame.MainGame.frametime;
			if (watchingPlayerTimer > 2f)
			{
				watchedPlayerIsInvalid = true;
				watchingPlayerTimer = 0f;
			}
		}
		else
		{
			watchingPlayerTimer = 0f;
		}
	}

	public void Change_View_To_Next_Player()
	{
		byte b = watchingPlayer;
		for (byte b2 = (byte)(watchingPlayer + 1); b2 < global::MainGame.MainGame.maxGamePlayers; b2++)
		{
			if ((global::Players.Players.players[b2].onmap & 0xC) > 0)
			{
				watchingPlayer = b2;
				break;
			}
		}
		if (b == watchingPlayer)
		{
			for (byte b2 = 0; b2 < watchingPlayer; b2++)
			{
				if ((global::Players.Players.players[b2].onmap & 0xC) > 0)
				{
					watchingPlayer = b2;
					break;
				}
			}
		}
		moveViewToNewLocation = true;
	}

	public void Change_View_To_Previous_Player()
	{
		byte b = watchingPlayer;
		for (short num = (short)(watchingPlayer - 1); num > -1; num--)
		{
			if ((global::Players.Players.players[num].onmap & 0xC) > 0)
			{
				watchingPlayer = (byte)num;
				break;
			}
		}
		if (b == watchingPlayer)
		{
			short num = (short)(global::MainGame.MainGame.maxGamePlayers - 1);
			while (num > b && num > -1)
			{
				if ((global::Players.Players.players[num].onmap & 0xC) > 0)
				{
					watchingPlayer = (byte)num;
					break;
				}
				num--;
			}
		}
		moveViewToNewLocation = true;
	}

	public void Free_Rendering_VBOs()
	{
		global::MainGame.MainGame.curVboID = -1;
		rGraphics.SetVertexBuffer(global::Models.Models.mVertexBufferObjects);
		rGraphics.Indices = global::Models.Models.mIndexBufferObjects;
		try
		{
			if (mainVBO != null)
			{
				mainVBO.Dispose();
				mainVBO = null;
			}
		}
		catch (Exception)
		{
		}
		try
		{
			if (mainIndexBuffer != null)
			{
				mainIndexBuffer.Dispose();
				mainIndexBuffer = null;
			}
		}
		catch (Exception)
		{
		}
		try
		{
			if (mainAlphaVBO != null)
			{
				mainAlphaVBO.Dispose();
				mainAlphaVBO = null;
			}
		}
		catch (Exception)
		{
		}
		try
		{
			if (mainAlphaIndexBuffer != null)
			{
				mainAlphaIndexBuffer.Dispose();
				mainAlphaIndexBuffer = null;
			}
		}
		catch (Exception)
		{
		}
		mainC.modelsMain.Free_Level_VBO();
		GC.Collect();
	}

	public void Create_Rendering_VBOs()
	{
		if (global::MainGame.MainGame.curVboID == global::MainGame.MainGame.newVboID)
		{
			return;
		}
		mainC.renderingMain.Clear_Model_Particles();
		Create_Static_VBO_Opaque();
		if (global::MainGame.MainGame.debugRestart <= 0)
		{
			Create_Static_VBO_Alpha();
			if (global::MainGame.MainGame.debugRestart <= 0)
			{
				mainC.modelsMain.Create_Level_Model_VBO_Final();
				global::MainGame.MainGame.curVboID = global::MainGame.MainGame.newVboID;
			}
		}
	}

	public void Create_Static_VBO_Opaque()
	{
		try
		{
			if (mainVBO != null)
			{
				mainVBO.Dispose();
				mainVBO = null;
			}
		}
		catch (Exception)
		{
		}
		try
		{
			if (mainIndexBuffer != null)
			{
				mainIndexBuffer.Dispose();
				mainIndexBuffer = null;
			}
		}
		catch (Exception)
		{
		}
		try
		{
			GC.Collect();
			listIndex = 0;
			int num = 0;
			int num2 = 0;
			int vPtr = 0;
			int iPtr = 0;
			num += mainC.gameobjectMain.Count_Static_Buffer_Faces_Opaque();
			num2 += Count_Static_Buffer_Faces_Opaque();
			if (num < 1 && num2 < 1)
			{
				numStaticVertices = 0;
				numStaticIndexes = 0;
				return;
			}
			numStaticVertices = num * 4 + num2 * 3;
			numStaticIndexes = num * 6 + num2 * 3;
			vertexArray = new StructsClass.VertexPositionColorNormalTexture[numStaticVertices];
			vertexIndexArray = new int[numStaticIndexes];
			int numTextures = global::Textures.Textures.numTextures;
			for (int i = 0; i < numTextures; i++)
			{
				vboList[i, 1] = -1;
			}
			for (int i = 0; i < global::Textures.Textures.numTextures; i++)
			{
				if (global::Textures.Textures.texMain.isAlpha[i])
				{
					continue;
				}
				numTextures = vPtr;
				int num3 = iPtr;
				int primitiveCount = 0;
				vboList[listIndex, 1] = -1;
				mainC.gameobjectMain.Create_VBO_Static_Opaque(i, 1, ref vertexArray, ref vertexIndexArray, ref vPtr, ref iPtr, ref primitiveCount);
				for (int j = 0; j < numRenderingInstances; j++)
				{
					if (!renderingInstances[j].useVbo)
					{
						continue;
					}
					int numItems = renderingInstances[j].numItems;
					for (int k = 0; k < numItems; k++)
					{
						for (int l = 0; l < renderingInstances[j].numModels; l++)
						{
							if (global::Models.Models.modVbo[renderingInstances[j].modelList[l]].texID == i)
							{
								Matrix mv = Matrix.CreateScale(renderingInstances[j].sx[k], renderingInstances[j].sy[k], renderingInstances[j].sz[k]) * renderingInstances[j].mv * Matrix.CreateTranslation(renderingInstances[j].x[k], renderingInstances[j].y[k], renderingInstances[j].z[k]);
								mainC.modelsMain.Create_VBO_Shared_Level_Model(renderingInstances[j].modelList[l], ref vertexArray, ref vertexIndexArray, ref vPtr, ref iPtr, ref primitiveCount, 1f, 1f, 1f, 1f, ref mv);
							}
						}
					}
				}
				if (num3 != iPtr)
				{
					vboList[listIndex, 0] = i;
					vboList[listIndex, 1] = numTextures;
					vboList[listIndex, 2] = num3;
					vboList[listIndex, 3] = vPtr - numTextures;
					vboList[listIndex, 4] = primitiveCount;
					vboList[listIndex, 5] = 1;
					vboList[listIndex, 6] = mainC.texturesMain.Find_Texture(global::Textures.Textures.texMain.texNames[i] + "_nm", global::Textures.Textures.texDefaultNormalMap);
					listIndex++;
				}
			}
			for (int j = 0; j < numRenderingInstances; j++)
			{
				if (!renderingInstances[j].useVbo || renderingInstances[j].type == 2)
				{
					for (int l = 0; l < renderingInstances[j].numModels; l++)
					{
						global::Models.Models.modVbo[renderingInstances[j].modelList[l]].inLevelVBO = true;
					}
				}
			}
			mainVBO = new VertexBuffer(rGraphics, rDecVPCNT, numStaticVertices, BufferUsage.WriteOnly);
			mainVBO.SetData(vertexArray);
			vertexArray = null;
			GC.Collect();
			mainIndexBuffer = new IndexBuffer(rGraphics, typeof(int), numStaticIndexes, BufferUsage.WriteOnly);
			mainIndexBuffer.SetData(vertexIndexArray);
			vertexIndexArray = null;
			GC.Collect();
		}
		catch (Exception)
		{
			global::MainGame.MainGame.debugRestart = 1;
		}
	}

	public void Create_Static_VBO_Alpha()
	{
		int num = 0;
		try
		{
			if (mainAlphaVBO != null)
			{
				mainAlphaVBO.Dispose();
				mainAlphaVBO = null;
			}
		}
		catch (Exception)
		{
		}
		try
		{
			if (mainAlphaIndexBuffer != null)
			{
				mainAlphaIndexBuffer.Dispose();
				mainAlphaIndexBuffer = null;
			}
		}
		catch (Exception)
		{
		}
		try
		{
			GC.Collect();
			alphaListIndex = 0;
			int num2 = 0;
			int num3 = 0;
			int vPtr = 0;
			num = 0;
			num2 += mainC.gameobjectMain.Count_Static_Buffer_Faces_Transparent();
			num3 += Count_Static_Buffer_Faces_Alpha();
			if (num2 < 1 && num3 < 1)
			{
				numStaticAlphaVertices = 0;
				numStaticAlphaIndexes = 0;
				return;
			}
			numStaticAlphaVertices = num2 * 4 + num3 * 3;
			numStaticAlphaIndexes = num2 * 6 + num3 * 3;
			vertexArray = new StructsClass.VertexPositionColorNormalTexture[numStaticAlphaVertices];
			vertexIndexArray = new int[numStaticAlphaIndexes];
			int numAlphaTextures = global::Textures.Textures.numAlphaTextures;
			for (int i = 0; i < numAlphaTextures; i++)
			{
				alphaVboList[i, 1] = -1;
			}
			for (int i = 0; i < global::Textures.Textures.numTextures; i++)
			{
				if (!global::Textures.Textures.texMain.isAlpha[i])
				{
					continue;
				}
				numAlphaTextures = vPtr;
				int num4 = num;
				int primitiveCount = 0;
				alphaVboList[alphaListIndex, 1] = -1;
				mainC.gameobjectMain.Create_VBO_Static_Transparent(i, 1, ref vertexArray, ref vertexIndexArray, ref vPtr, ref num, ref primitiveCount);
				for (int j = 0; j < numRenderingInstances; j++)
				{
					int numItems = renderingInstances[j].numItems;
					for (int k = 0; k < numItems; k++)
					{
						for (int l = 0; l < renderingInstances[j].numModels; l++)
						{
							if (global::Models.Models.modVbo[renderingInstances[j].modelList[l]].texID == i)
							{
								if (renderingInstances[j].useVbo)
								{
									Matrix matrix = Matrix.CreateTranslation(renderingInstances[j].x[k], renderingInstances[j].y[k], renderingInstances[j].z[k]);
									matrix = renderingInstances[j].mv * matrix;
									matrix = Matrix.CreateScale(renderingInstances[j].sx[k], renderingInstances[j].sy[k], renderingInstances[j].sz[k]) * matrix;
									mainC.modelsMain.Create_VBO_Shared_Level_Model(renderingInstances[j].modelList[l], ref vertexArray, ref vertexIndexArray, ref vPtr, ref num, ref primitiveCount, 1f, 1f, 1f, 1f, ref matrix);
								}
								global::Models.Models.modVbo[renderingInstances[j].modelList[l]].inLevelVBO = true;
								New_Particle_Model(10, renderingInstances[j].x[k], renderingInstances[j].y[k], renderingInstances[j].z[k], j, (short)k, (short)l);
							}
						}
					}
				}
				if (num4 != num)
				{
					alphaVboList[alphaListIndex, 0] = i;
					alphaVboList[alphaListIndex, 1] = numAlphaTextures;
					alphaVboList[alphaListIndex, 2] = num4;
					alphaVboList[alphaListIndex, 3] = vPtr - numAlphaTextures;
					alphaVboList[alphaListIndex, 4] = primitiveCount;
					alphaVboList[alphaListIndex, 5] = 1;
					alphaVboList[alphaListIndex, 6] = mainC.texturesMain.Find_Texture(global::Textures.Textures.texMain.texNames[i] + "_nm", global::Textures.Textures.texDefaultNormalMap);
					alphaListIndex++;
				}
			}
			mainAlphaVBO = new VertexBuffer(rGraphics, rDecVPCNT, numStaticAlphaVertices, BufferUsage.WriteOnly);
			mainAlphaVBO.SetData(vertexArray);
			vertexArray = null;
			GC.Collect();
			mainAlphaIndexBuffer = new IndexBuffer(rGraphics, typeof(int), numStaticAlphaIndexes, BufferUsage.WriteOnly);
			mainAlphaIndexBuffer.SetData(vertexIndexArray);
			vertexIndexArray = null;
			GC.Collect();
		}
		catch
		{
			global::MainGame.MainGame.debugRestart = 1;
		}
	}

	public int Count_Static_Buffer_Faces_Opaque()
	{
		int num = 0;
		for (int i = 0; i < numRenderingInstances; i++)
		{
			if (!renderingInstances[i].useVbo)
			{
				continue;
			}
			short numItems = renderingInstances[i].numItems;
			for (int j = 0; j < numItems; j++)
			{
				for (int k = 0; k < renderingInstances[i].numModels; k++)
				{
					if (!global::Textures.Textures.texMain.isAlpha[global::Models.Models.modVbo[renderingInstances[i].modelList[k]].texID])
					{
						num += mainC.modelsMain.Count_Static_Buffer_Faces_Level_Model_Opaque(renderingInstances[i].modelList[k]);
					}
				}
			}
		}
		return num;
	}

	public int Count_Static_Buffer_Faces_Alpha()
	{
		int num = 0;
		for (int i = 0; i < numRenderingInstances; i++)
		{
			if (!renderingInstances[i].useVbo)
			{
				continue;
			}
			short numItems = renderingInstances[i].numItems;
			for (int j = 0; j < numItems; j++)
			{
				for (int k = 0; k < renderingInstances[i].numModels; k++)
				{
					if (global::Textures.Textures.texMain.isAlpha[global::Models.Models.modVbo[renderingInstances[i].modelList[k]].texID])
					{
						num += mainC.modelsMain.Count_Static_Buffer_Faces_Level_Model_Opaque(renderingInstances[i].modelList[k]);
					}
				}
			}
		}
		return num;
	}

	public void Render_Hit_Screen()
	{
		rGraphics.RasterizerState = RasterizerState.CullClockwise;
		rGraphics.BlendState = blendSourceAlpha;
		rEffect.Projection = matrixO;
		rEffect.View = matrixI;
		rEffect.TextureEnabled = false;
		rEffect.VertexColorEnabled = true;
		byte b = (byte)rEffect.CurrentTechnique.Passes.Count;
		byte alpha = (byte)(hitColor[3] * 255f);
		playerHitVtex[0].Set_Alpha(alpha);
		playerHitVtex[1].Set_Alpha(alpha);
		playerHitVtex[2].Set_Alpha(alpha);
		playerHitVtex[3].Set_Alpha(alpha);
		for (byte b2 = 0; b2 < b; b2++)
		{
			rEffect.CurrentTechnique.Passes[b2].Apply();
			rGraphics.DrawUserIndexedPrimitives(PrimitiveType.TriangleList, playerHitVtex, 0, 4, playerHitIndex, 0, 2, rDecVPCNT);
		}
	}

	static Rendering()
	{
		ushort[] array = new ushort[1];
		playerModelTextures = array;
		errMsgStat = new int[10];
		screenWidthCenter = 640;
		screenHeightCenter = 360;
		curWeaponViewTime = 0f;
		viewVelocityY = 3f;
		viewVelocityX = 3f;
		viewVelocityZ = 3f;
		crossHairMovementSpeed = 35f;
		watchingPlayerTimer = 0f;
		rotateFluctation = 0f;
		cosineFluctuation = 0f;
		loadingScreenIconRotation = 0f;
		cosineFluctuation3 = 0f;
		cosineFluctuationModelView = 0f;
		viewSwayDir = 0f;
		viewMovement = 0f;
		doorSwitchScreenPos = 0f;
		particleRotation1 = 0f;
		particleRotation2 = 0f;
		particleRotation3 = 0f;
		particleRotation4 = 0f;
		particleRotation6 = 0f;
		solidParticleRotation1 = 0f;
		initialCamPosX = 0f;
		initialCamPosY = 0f;
		initialCamPosZ = 1f;
		initialCamObjX = 0f;
		initialCamObjY = 0f;
		initialCamObjZ = 0f;
		initialWorldX = 0f;
		initialWorldY = 1f;
		initialWorldZ = 0f;
		satelliteCrossHairX = new float[2];
		satelliteCrossHairY = new float[2];
		crossHairPosition = new float[3];
		projectionNearPlane = new float[2] { 0.3f, 0.3f };
		rpmFAR = new float[4];
		hitColor = new float[4];
		texAdj1 = new float[2];
		camerDir = new float[3];
		slFar4a = new Vector4[2];
		slFar4b = new Vector4[2];
		fps = 30f;
		aspectRatio = 1.7777778f;
		brightness = 0.5f;
		scopeValue = 1f;
		crossHairPositionGoal = new float[2, 3];
		newParticle = new float[5, 3];
		npn = new StructsClass.vtex();
		eyeVec = new StructsClass.vtex[2];
		particles = new StructsClass.particle_effect[2, 1500];
		solidParticles = new StructsClass.solid_particle_effect[2, 50];
		pSort = new StructsClass.sort_list[1500];
		dialogUpdate = new string[2, 4];
		texAdj = new float[2];
		updateFps = false;
		rgtV2 = default(Vector3);
		v3 = default(Vector3);
		v4 = default(Vector3);
		rgtV1 = default(Vector2);
		effectMatrix = new Matrix[56];
		showCollisionBoxes = 0;
		showParticleBox = false;
		allocatedPtLight_lvl = 0;
		specular = 200;
		diffuseLight = new Vector3(0.9f, 0.9f, 0.9f);
		specularLight = new Vector3(0.5f, 0.5f, 0.5f);
		LightPosition = Vector3.Normalize(new Vector3(-1f, -0.5f, -0.5f));
		ptLight0 = new float[3];
		ptLight1 = new float[3];
		ambientLevel = new float[4] { 0.15f, 0.15f, 0.15f, 1f };
		ambientAvatar = new float[4] { 0.5f, 0.5f, 0.5f, 1f };
		directionalLightVector = new float[3] { 0.065232806f, 0.19569841f, 0.9784921f };
		directionalLightColor = new float[4] { 0.85f, 0.85f, 0.85f, 1f };
		directionalLightBounce = new float[4] { 28.942f, 0.5f, 0.4f, 55f };
		ambient0 = new float[4] { 0f, 0f, 0f, 1f };
		ptLightDir0 = new float[3];
		ptLightColor1 = new float[4];
		fau3 = new float[3];
		fau4 = new float[4];
		far3 = new float[3];
		far4 = new float[4];
		far4b = new float[4];
		splFar4 = new float[4];
		rFar4a = new Vector4[2];
		rFar4b = new Vector4[2];
		particleLightLocation = new float[2, 5, 3];
		particleLightColor = new float[2, 5, 4];
		particleDistance = new float[2, 5];
		worldPos = new Vector3[2];
		mvRendering = default(Matrix);
		mvRendering2 = default(Matrix);
		fb = new byte[789507];
		fb2 = new byte[1048576];
		showBoundingBox = false;
		curFps = 0f;
		curFpsTotal = 0f;
		fpsCnt = 0;
		fpsCntTo = 5;
		curParticleID = 0;
		matrixVDB = new Matrix[2];
		matrixPDB = new Matrix[2];
		hsrC1 = default(Color);
		vForward = new StructsClass.vtex[5];
		numStaticVertices = 0;
		numStaticIndexes = 0;
		numStaticAlphaVertices = 0;
		numStaticAlphaIndexes = 0;
		camPos = new Vector3[2];
		camPosGoal = new Vector3[2];
		camObject = new Vector3[2];
		camObjectGoal = new Vector3[2];
		camUp = new Vector3[2];
		camPosShadowMap = default(Vector3);
		camPosShadowMapUp = default(Vector3);
		worldUp = new Vector3(0f, 0f, 1f);
		backColor = new Color(0f, 0f, 0f, 1f);
		splashTexture = new Texture2D[1];
		rsPos = default(Vector2);
		tauntPos = default(Vector2);
		swapWeaponPos = default(Vector2);
		cWhite = Color.White;
		cBlack = Color.Black;
		cRed = Color.Red;
		cGreen = Color.Green;
		cBlue = Color.Blue;
		cYellow = Color.Yellow;
		miniMapRed = default(Color);
		miniMapBlue = default(Color);
		barColor = default(Color);
		vecColor = default(Color);
		vecQuadInd = new int[6] { 0, 1, 2, 0, 2, 3 };
		planeVtex = new StructsClass.VertexPositionColorNormalTexture[4];
		scoreBoardVtex = new StructsClass.VertexPositionColorNormalTexture[4];
		ScopeVtex = new StructsClass.VertexPositionColorNormalTexture[4];
		blendSourceAlpha = new BlendState();
		blendSourceOne = new BlendState();
		depthBufferEnabled = new DepthStencilState();
		depthBufferWriteDisabled = new DepthStencilState();
		depthBufferDisabled = new DepthStencilState();
		rasterizerState = new RasterizerState();
		textureSamplerState = new SamplerState();
		textureSamplerStatePoint = new SamplerState();
		textureSamplerStateClamp = new SamplerState();
	}
}
