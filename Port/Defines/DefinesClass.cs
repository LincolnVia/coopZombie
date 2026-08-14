namespace Defines;

public class DefinesClass
{
	public const byte UI_WINDOW_MAIN_MENU_ID = 1;

	public const byte UI_WINDOW_OPTIONS_ID = 0;

	public const byte UI_WINDOW_CONFIRM_ID = 3;

	public const byte UI_WINDOW_SIGNIN_ID = 2;

	public const byte UI_WINDOW_BUYME_ID = 4;

	public const byte UI_WINDOW_INGAME_MENU_ID = 5;

	public const byte UI_WINDOW_CONTROLS_WINDOW_ID = 6;

	public const byte UI_WINDOW_PLAY_WINDOW_ID = 7;

	public const byte UI_WINDOW_INFO_WINDOW_ID = 8;

	public const byte UI_WINDOW_SCORES_WINDOW_ID = 9;

	public const byte UI_WINDOW_RESULTS_WINDOW_ID = 10;

	public const byte UI_WINDOW_WEAPON_SELECT_WINDOW_ID = 11;

	public const byte UI_WINDOW_VEHICLE_SELECT_WINDOW_ID = byte.MaxValue;

	public const byte UI_WINDOW_CREDITS_WINDOW_ID = 12;

	public const byte UI_WINDOW_INSTRUCTIONS_WINDOW_ID = 13;

	public const byte UI_WINDOW_CANCEL_SAVING_WINDOW_ID = 14;

	public const byte UI_WINDOW_IDLE_TIMEOUT_WINDOW_ID = 15;

	public const byte UI_WINDOW_MESSAGE_ID = 16;

	public const byte UI_WINDOW_TIPS_ID = 17;

	public const byte UI_WINDOW_MISSION_OBJECTIVES = 22;

	public const byte UI_WINDOW_GAME_OVER_WINDOW_ID = 23;

	public const byte UI_WINDOW_CHOOSE_MAP = 24;

	public const byte UI_WINDOW_COMPONENT_RESTART_SP_LEVEL = 1;

	public const byte LOADING_ICON_TYPE = 1;

	public const float LOADING_ICON_PULSATING_WIDTH = 0.2f;

	public const byte NUM_PLACED_ITEMS = 14;

	public const byte MAX_PLAYER_RANK = 50;

	public const string PLAYER_KILL_MSG1 = " killed ";

	public const string PLAYER_KILL_MSG2 = " shot himself";

	public const string BALLISTIC_STRIKE_MESSAGE = " launched an air strike";

	public const string PLAYER_RANK_MSG = " ranked up to rank ";

	public const string CONTENT_PATH = "The_CoOp_Zombie_Game";

	public const float UNIT_CONVERSION_FACTOR = 1f;

	public const float COLLISION_BOX_PADDING = 0.25f;

	public const short MIN_SCREEN_PIXELS_TARGETING_SLOWDOWN_SQUARED = 225;

	public const short MAX_SCREEN_PIXELS_TARGETING_SLOWDOWN_SQUARED = 14400;

	public const float TARGETING_SLOWDOWN_MAX_DISTANCE = 300f;

	public const float DEFAULT_TIME_BEFORE_RESPAWN = 11f;

	public const float OVERHEAD_VIEW_HEIGHT = 20f;

	public const byte DEFAULT_VIEW = 1;

	public const byte NUM_LEVEL_LIGHTS = 2;

	public const byte NUM_PARTICLE_LIGHTS = 5;

	public const byte NUM_AMMO_LIGHTS = 2;

	public const float BRIGHTNESS_FACTOR = 0.5f;

	public const float COLLISION_DETECTION_STEP_DISTANCE = 1f;

	public const bool PRE_RENDER_TO_SET_DEPTH_BUFFER = true;

	public const short NUM_COLLISION_ARRAY_POINTS = 43;

	public const short NUM_COLLISION_ARRAY_POINTS_TIMES3 = 129;

	public const short NUM_COLLISION_ARRAY_FLOATS = 258;

	public const byte GAME_VERSION_NUMBER = 1;

	public const string TELL_A_FRIEND = "Hey! I'm playing a game called The Co-Op Zombie Game! I thought you would like it too. Go to the Games Marketplace, then Indie Games to download the FREE Trial.";

	public const string GAME_NAME = "The Co-Op Zombie Game";

	public const float AVATAR_SCALE = 4f;

	public const float JUMPING_SPEED = 14f;

	public const byte NUM_SP_LEVELS = 19;

	public const byte NUM_MP_LEVELS = 4;

	public const byte NUM_MP_COOP_LEVELS = 19;

	public const byte MAX_TRIAL_MODE_LEVEL = 2;

	public const byte MAX_TRIAL_MODE_RANK = 3;

	public const float PLAYER_SPEAKING_TIMER_DEFAULT = 0.75f;

	public const float PLAYER_HIT_TIMER_DEFAULT = 0.75f;

	public const float PLAYER_HIT_TIMER_DEATH_DEFAULT = 2f;

	public const float COLLISION_BOUNCE_THRESHOLD = 0.75f;

	public const float COLLISION_ROTATIONAL_BOUNCE_THRESHOLD = 0.02f;

	public const float COLLISION_ROTATIONAL_TORQUE_THRESHOLD = 2E-06f;

	public const float FOOTSTEP_TIME = 0.6f;

	public const float CHARSPEED = 20f;

	public const float CHARSPEEDSIDEWAYS = 20f;

	public const float CHARSPEED_RUNNING = 30f;

	public const float CHARSPEED_RUNNING_NETWORK_SMOOTHING = 25f;

	public const float CHARSPEED_SMOOTHING_INCREASE = 1200f;

	public const float CHARSPEED_RUNNING_THRESHOLD = 35f;

	public const float CHARSPEEDROTATION = 140f;

	public const float CHARSPEEDELEVATION = 225f;

	public const float CHARSPEED_LEVELEDITOR = 60f;

	public const float CHARSPEED_ELEVATION_LEVELEDITOR = 10f;

	public const float ROTATIONSPEED_LEVELEDITOR = 180f;

	public const float SPAWNING_CHECK_DISTANCE_SQUARED = 900f;

	public const float THIRD_PERSON_TRANSPARENT_DISTANCE = 0.01f;

	public const float INCREMENTAL_CAMERA_MOVE_DISTANCE = 1f;

	public const float CAMERA_MOVE_PERCENTAGE = 0.34f;

	public const float CAMERA_ROTATE_SPEED_DEGREES = 360f;

	public const float CAMERA_MOVE_SPEED_AVOIDING_COLLISION = 1.6f;

	public const float CAMERA_MOVE_SPEED_RETURNING_AFTER_COLLISION = 0.8f;

	public const byte NUMBER_SIMULTANEOUS_VEHICLES = 3;

	public const float TARGET_RADIUS = 40f;

	public const float STARTINGTHIRDPERSONX = 1.3f;

	public const float STARTINGTHIRDPERSONY = -2.85f;

	public const float STARTINGTHIRDPERSONZ = 2.8f;

	public const float MELEEE_ANGLE = 0f;

	public const byte NUM_LOADING_SCREENS = 1;

	public const byte SPLASH_LOADING = 0;

	public const byte SPLASH_TRIALSCREEN = byte.MaxValue;

	public const byte SPLASH_CONTROLLER_SCREEN = byte.MaxValue;

	public const byte SPLASH_LEVEL_LOADING_SCREENS_START = 0;

	public const byte PARTICLE_EFFECT_BLOOD = 4;

	public const byte PARTICLE_EFFECT_SPARKS = 5;

	public const byte NUM_PLAYER_PARTICLES = 10;

	public const float COLLISION_BOUNDARY_PADDING = 100f;

	public const float ROUTE_SPACING = 0.1f;

	public const byte MIN_PASSAGE_WIDTH = 3;

	public const short MAX_ROUTE_DIMENSION = 1000;

	public const short MIN_ROUTE_DIMENSION = 160;

	public const short MAX_ROUTE_PTS = 2001;

	public const int MIN_ROUTE_PTS = 41;

	public const short MAX_ROUTE_BOXES = 500;

	public const float DEFAULT_DAMAGE_REDUCTION = 0.15f;

	public const float DEFAULT_DAMAGE_INCREASE = 1f;

	public const string MB_RESPAWN_MAJOR_RESTART = "MessageBox_Respawn";

	public const string MB_RESPAWN_MINOR_RESTART = "MessageBox_Respawn";

	public const byte NUM_MP_ROUND_TIMES = 4;

	public const float DEFAULT_MP_JOIN_REFRESH_TIMER = 15f;

	public const float DEFAULT_RUN_TIME = 4f;

	public const float DEFAULT_RUN_RESET_TIME = 4f;

	public const float LOOK_SENSITIVITY_MIN = 0.15f;

	public const float LOOK_SENSITIVITY_MAX = 1f;

	public const float DEFAULT_LOOK_SENSITIVITY = 0.5f;

	public const float DEFAULT_LOOK_PRECISE_SENSITIVITY = 0.25f;

	public const float DEFAULT_BRIGHTNESS = 0.5f;

	public const float BRIGHTNESS_LOW_LIMIT = 0.75f;

	public const float BRIGHTNESS_UPPER_LIMIT = 1.25f;

	public const bool DEFAULT_AUTO_PRECISE_AIM = false;

	public const bool DEFAULT_SLOW_SIDESTEP = false;

	public const byte NUM_LOCAL_PLAYERS = 1;

	public const byte NUM_TEAMS = 5;

	public const float STANDARD_HEALING_TIME = 60f;

	public const float ROUND_OVER_TIMER = 0f;

	public const float DEFAULT_LOBBY_TIMER = 25f;

	public const float MAP_VOTE_TIMER = 8f;

	public const float NEWGAME_LOBBY_TIMER = 0f;

	public const byte MAX_SHADER_INSTANCES = 20;

	public const byte MAX_PARTICLE_SHADER_INSTANCES = 20;

	public const byte NUM_CONTINUAL_SOUNDS = 0;

	public const byte NUM_BACKGROUND_SOUND_TYPES = 6;

	public const byte NUM_TYPE1_VOICE_CUES = 20;

	public const float DISABLE_BACKGROUND_SOUNDS_TIME = 10000f;

	public const byte NUM_SOUNDCUES = 20;

	public const string DEFAULT_SOUND = "NewPlayer";

	public const string DEFAULT_VOICE_SOUND = "ZombieScowl";

	public const float CONTROLLER_BUTTON_REPEAT = 0.13f;

	public const float DEFAULT_COMMANDER_TELEPORT_TIMER = 5f;

	public const float DEFAULT_COMMANDER_TELEPORT_LOCK_TIMER = 3f;

	public const float DEFAULT_COMMANDER_ENERGY_TIMER = 10f;

	public const float RESPAWN_TIME_MULTIPLAYER = 5f;

	public const float RESPAWN_TIME_SINGLEPLAYER = 8f;

	public const int NUM_PARTICLE_TYPES = 4;

	public const int NUM_PARTICLES = 1500;

	public const int NUM_SOLID_PARTICLES = 50;

	public const int MAX_PLAYERS = 44;

	public const byte MAX_PLAYERS_ONLINE = 4;

	public const float MAX_HUMAN_PLAYER_DAMAGE = 100f;

	public const float VIEWINGANGLE = 45f;

	public const float CLIPNEAR = 0.2f;

	public const float CLIPFAR = 10500f;

	public const int DEFAULTJOINTSPEED = 100;

	public const int DEFAULTPIVOTSPEED = 20;

	public const byte NUM_PLAYER_WEAPON_STUBS = 10;

	public const byte MAXPLAYERWEAPONS = 8;

	public const byte WEAPON_GRENADE = 5;

	public const int MAXDIALOGBOXES = 10;

	public const int MAXDIALOGBOXTEXT = 5;

	public const int MAXDIALOGBOXSLIDERS = 5;

	public const int TEXTURESCALE = 100;

	public const float TEXTURESCALE_F = 100f;

	public const ushort NUM_BYTES_PER_SECOND_FOR_AI_MESSAGES = 5600;

	public const float NUM_SECONDS_PER_AI_PACKET = 0.011607143f;

	public const float NUM_SECONDS_PER_NETWORK_MESSAGE = 1f / 30f;

	public const float NUM_SECONDS_PER_LOBBY_NETWORK_MESSAGE = 0.1f;

	public const float GRAVITY = -32.15223f;

	public const float GRAVITY_TIMES_0_35 = -1.1253281f;

	public const float DENSITY_AIR_SEA_LEVEL = 0.07648f;

	public const float LASERSPEED = 100f;

	public const float FLASHATTENUATION = 10f;

	public const float FLASHTIME = 0.1f;

	public const string MODEL_DOORSWITCHSCREEN = "Door_Switch_Screen.txt";

	public const string MODEL_MARKER = "marker.txt";

	public const string MODEL_MARKER2 = "marker2.txt";

	public const string MODEL_MARKER3 = "marker3.txt";

	public const string MODEL_MARKER4 = "marker4.txt";

	public const string MODEL_MARKER5 = "marker5.txt";

	public const string MODEL_CHUD = "Commander-Hud.txt";

	public const string MODEL_CHUD_BAR = "Commander-Hud-Bar.txt";

	public const string MODEL_TRANSPORTER = "Transporter.txt";

	public const string MODEL_SKYDOME = "SkyDome.txt";

	public const string MODEL_SQUARE = "Square.txt";

	public const string MODEL_FLAT_PLANE = "FlatPlane.txt";

	public const string MODEL_HALF_SPHERE = "HalfSphere.txt";

	public const int NUM_ACTIVE_BULLETS = 100;

	public const int MAXWEAPONS = 10;

	public const int MAXJOINTS = 100;

	public const int MAXPROGRAMSTEPSPERFRAME = 50;

	public const ushort NUMREGENHOLDERS = 1;

	public const byte NUM_NETWORK_BUFFERS = 10;

	public const byte NUM_NETWORK_BOOL_BUFFERS = 20;

	public const byte NUM_NETWORK_FLOAT_BUFFERS = 20;

	public const byte NUM_NETWORK_BYTE_BUFFERS = 30;

	public const byte NUM_NETWORK_HALF_SINGLE_BUFFERS = 16;

	public const byte NUM_NETWORK_SHORTS = 10;

	public const byte NUM_NETWORK_UNSIGNED_SHORTS = 10;
}
