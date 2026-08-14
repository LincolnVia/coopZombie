using System;

namespace Defines;

public class DefinesSharedClass
{
	public const float PI = (float)Math.PI;

	public const float PI_TIMES_2 = (float)Math.PI * 2f;

	public const float PI_OVER_180 = (float)Math.PI / 180f;

	public const float PI_OVER_2 = (float)Math.PI / 2f;

	public const float PI_OVER_4 = (float)Math.PI / 4f;

	public const float RAD_89DEG = (float)Math.PI * 89f / 180f;

	public const float RAD_5DEG = 0.08726646f;

	public const float RAD_15DEG = (float)Math.PI / 12f;

	public const float RAD2DEG = 57.29578f;

	public const float DEG2RAD = (float)Math.PI / 180f;

	public const float SCOPE_VALUE_IRONSIGHTS = 0.5235987f;

	public const byte NUM_GAME_ITEM_TYPES = 5;

	public const byte GAME_ITEM_TYPE_GAME_OBJECT = 0;

	public const byte GAME_ITEM_TYPE_PICKUP = 1;

	public const byte GAME_ITEM_TYPE_BALLISTIC_PICKUP = 2;

	public const byte GAME_ITEM_TYPE_INSTANCED_GEOMETRY = 3;

	public const byte GAME_ITEM_TYPE_MINIMAP_ITEM = 4;

	public const byte GAME_ITEM_TYPE_TERRAIN_GEOMETRY = 5;

	public const byte VEHICLE_TYPE_HUMANOID = 0;

	public const byte VEHICLE_TYPE_AIRPLANE = 1;

	public const byte VEHICLE_TYPE_SKATEBOARD = 2;

	public const byte VEHICLE_TYPE_FIXED_TURRET = 3;

	public const byte VEHICLE_TYPE_UNMANNED_TURRET = 4;

	public const byte VEHICLE_TYPE_SPACESHIP = 5;

	public const byte VEHICLE_TYPE_ARCADE_STYLE_AIRPLANE = 6;

	public const byte VEHICLE_TYPE_ARCADE_STYLE_HELICOPTER = 7;

	public const byte VEHICLE_TYPE_AVATAR = 8;

	public const byte DAMAGE_TYPE_STRUCTURE = 0;

	public const byte DAMAGE_TYPE_HUMAN = 1;

	public const byte DAMAGE_TYPE_ROBOT = 2;

	public const byte DAMAGE_TYPE_VEHICLE = 3;

	public const byte DAMAGE_TYPE_ARMOREDVEHICLE = 4;

	public const byte VIEW_THIRD_PERSON = 0;

	public const byte VIEW_FIRST_PERSON = 1;

	public const byte VIEW_IRON_SIGHTS = 2;

	public const byte VIEW_SCOPE = 3;

	public const byte VIEW_PLANE = 4;

	public const byte VIEW_FROZEN = 5;

	public const byte VIEW_AVATAR = 6;

	public const byte VIEW_SATELLITE = 7;

	public const byte NETWORK_GAME_STATUS_RESET = 0;

	public const byte NETWORK_GAME_STATUS_CREATED_SUCCESSFULLY = 1;

	public const byte NETWORK_GAME_STATUS_FAILED_NO_GAMES_FOUND = 2;

	public const byte NETWORK_GAME_STATUS_FAILED_NETWORKING_ERROR = 3;

	public const byte NETWORK_GAME_STATUS_FAILED_UNABLE_TO_JOIN = 4;

	public const byte NETWORK_GAME_STATUS_FAILED_UNABLE_TO_CREATE = 5;

	public const ushort NUM_AI_PACKET_BYTES = 65;

	public const byte COLLISION_MODEL_TYPE_PLAYER = 0;

	public const byte COLLISION_MODEL_TYPE_BALLISTIC = 1;

	public const byte COLLISION_MODEL_TYPE_AI_PLAYER = 2;

	public const byte COLLISION_MODEL_TYPE_VIEW = 3;

	public const byte GAME_OBJECT_TYPE_EXPLOSIVE_COMPOUND = 1;

	public const byte GAME_OBJECT_TYPE_BUILDING = 2;

	public const byte GAME_OBJECT_TYPE_BREAK_APART_ITEM = 3;

	public const byte GAMEMODE_SINGLE_PLAYER = 0;

	public const byte GAMEMODE_MULTI_PLAYER = 1;

	public const byte GAMEMODE_NONE = byte.MaxValue;

	public const byte NETWORK_STATE_NONE = 0;

	public const byte NETWORK_STATE_LOBBY = 1;

	public const byte NETWORK_STATE_PLAYING = 2;

	public const byte NETWORK_STATE_GAME_ENDED = 3;

	public const byte GAMETYPE_DEATHMATCH = 0;

	public const byte GAMETYPE_PRISONBREAK = 1;

	public const byte GAMETYPE_CO_OP = 2;

	public const byte GAMETYPE_NO_TIME_LIMIT = 3;

	public const byte GAMETYPE_RACE = 4;

	public const byte GAMETYPE_BALLOON_POP = 5;

	public const byte GAMETYPE_BOMB_DROP_CHALLENGE = 6;

	public const byte GAMETYPE_FREE_FOR_ALL = 7;

	public const byte GAMESTATE_INITIAL_LOADING = 0;

	public const byte GAMESTATE_MAIN_MENU = 1;

	public const byte GAMESTATE_SP_NORMAL_PLAY = 2;

	public const byte GAMESTATE_SP_BEGINNING_TO_PAUSE_FOR_MENUS = 3;

	public const byte GAMESTATE_SP_PAUSED_FOR_MENUS = 4;

	public const byte GAMESTATE_SP_LEAVING_MENUS = 5;

	public const byte GAMESTATE_SP_LEVEL_COMPLETE = 6;

	public const byte GAMESTATE_SP_LOADING_NEW_LEVEL = 7;

	public const byte GAMESTATE_SP_GAME_COMPLETED = 8;

	public const byte GAMESTATE_SP_SAVING_CHECKPOINT = 9;

	public const byte GAMESTATE_SP_TRIAL_OVER = 10;

	public const byte GAMESTATE_SP_TRIAL_OVER_PURCHASING = 11;

	public const byte GAMESTATE_SP_ROUND_OVER_NO_AUTO_ADVANCE = 12;

	public const byte GAMESTATE_SP_ROUND_OVER_BEGINNING_PAUSE_FOR_MENU = 13;

	public const byte GAMESTATE_SP_ROUND_OVER_PAUSED_FOR_MENUS = 14;

	public const byte GAMESTATE_SP_ROUND_OVER_LEAVING_MENUS_RETURNING_TO_SUMMARY = 15;

	public const byte GAMESTATE_LEVEL_EDITOR = 16;

	public const byte GAMESTATE_SP_NORMAL_PLAY_FIRST_FRAME = 17;

	public const byte GAMESTATE_SP_RACE_COUNTDOWN_TO_START = 18;

	public const byte GAMESTATE_SP_LOADING_LEVEL = 20;

	public const byte GAMESTATE_SP_WAITING_TO_SPAWN = 21;

	public const byte GAMESTATE_SP_GAME_ENDED = 22;

	public const byte GAMESTATE_SP_GAME_OVER_SCREEN = 23;

	public const byte GAMESTATE_SHOWING_MISSION_OBJECTIVES = 24;

	public const byte GAMESTATE_WEAPON_SELECT = 25;

	public const byte GAMESTATE_VEHICLE_SELECT = 26;

	public const byte GAMESTATE_MP_IN_LOBBY = 129;

	public const byte GAMESTATE_MP_IN_LOBBY_ENTERING_MENUS = 130;

	public const byte GAMESTATE_MP_IN_LOBBY_LEAVING_MENUS = 132;

	public const byte GAMESTATE_MP_IN_LOBBY_GAME_IS_STARTING = 133;

	public const byte GAMESTATE_MP_IN_LOBBY_GAME_STARTED_WAITING_FOR_DATA = 136;

	public const byte GAMESTATE_MP_LEAVING_PAUSED_MODE_NOT_PLAYING = 140;

	public const byte GAMESTATE_MP_NORMAL_PLAY = 141;

	public const byte GAMESTATE_MP_NORMAL_PLAY_BEGINNING_TO_PAUSE = 142;

	public const byte GAMESTATE_MP_NORMAL_PLAY_IN_MENUS = 143;

	public const byte GAMESTATE_MP_NORMAL_PLAY_LEAVING_MENUS = 144;

	public const byte GAMESTATE_MP_ROUND_ENDED = 145;

	public const byte GAMESTATE_MP_GAME_COMPLETED = 147;

	public const byte GAMESTATE_MP_RACE_COUNTDOWN_TO_START = 148;

	public const byte GAMESTATE_MP_WAITING_TO_SPAWN = 149;

	public const byte GAMESTATE_MP_JOINING_GAME_INVITE = 150;

	public const byte GAMESTATE_EXITING_VIA_MENUS = 254;

	public const byte NUM_COMMANDER_OBJECTIVE_TYPES = 7;

	public const byte MAX_MULTIPLAYER_ONSCREEN_GAMERTAG = 15;

	public const short MAX_MULTIPLAYER_ONSCREEN_GAMERTAG_PIXEL_LENGTH = 120;

	public const byte NUM_AMMO_DAMAGE_TYPES = 5;

	public const byte NUMCONTROLLERS = 4;

	public const byte PARTICLE_TYPE_ALPHA_MODEL = 10;

	public const byte HUMAN_PROGRAM_COVER_SIDEWAYS = 11;

	public const byte HUMAN_PROGRAM_STATIONARY_LEGSBODY = 13;

	public const byte HUMAN_PROGRAM_STATIONARY_ARMS = 5;

	public const byte HUMAN_PROGRAM_WALK = 14;

	public const byte HUMAN_PROGRAM_SIDESTEP = 2;

	public const byte HUMAN_PROGRAM_RUN = 0;

	public const byte HUMAN_PROGRAM_JUMP = 9;

	public const byte HUMAN_PROGRAM_WALK_BACKWARDS = 16;

	public const byte HUMAN_PROGRAM_TURNING_LEFT = 10;

	public const byte HUMAN_PROGRAM_TURNING_RIGHT = 11;

	public const byte HUMAN_PROGRAM_DEATH_1 = 17;

	public const byte HUMAN_PROGRAM_DEATH_2 = 18;

	public const byte HOVER_BOT_DEATH = 22;

	public const byte HOVER_BOT_IDLE = 23;

	public const byte HUMAN_TORQUE_JOINT = 0;

	public const byte HUMAN_HEAD_JOINT = 0;

	public const byte HUMAN_EYE_JOINT = 0;

	public const byte HUMAN_THIRDPERSON_JOINT = 0;

	public const byte HUMAN_LSHOULDER_JOINT = 0;

	public const byte HUMAN_RSHOULDER_JOINT = 0;

	public const byte PLAYER_WEAPON_VIEW_JOINT = 0;

	public const byte PLAYER_WEAPON_JOINT = 0;

	public const short PLAYER_NEEDS_RESET = 32766;

	public const short PLAYER_STATIONARY = 1;

	public const short PLAYER_WALKING = 2;

	public const short PLAYER_SIDESTEPPING_RIGHT = 4;

	public const short PLAYER_RUNNING = 8;

	public const short PLAYER_WALKING_BACKWARDS = 16;

	public const short PLAYER_JUMPING = 32;

	public const short PLAYER_TURNING_LEFT = 64;

	public const short PLAYER_TURNING_RIGHT = 128;

	public const short PLAYER_THROWING = 256;

	public const short PLAYER_SIDESTEPPING_LEFT = 512;

	public const short PLAYER_TAUNTING = 1024;

	public const int WEAPONSHOULDER = 1;

	public const byte WEAPONHAND = 2;

	public const byte FIREMODE_AUTOMATIC = 0;

	public const byte FIREMODE_SINGLE_SHOT = 1;

	public const byte FIREMODE_SELECTABLE = 2;

	public const float COEFFICIENT_FRICTION_GLASS_GLASS = 0.94f;

	public const float COEFFICIENT_FRICTION_STEEL_TEFLON = 0.04f;

	public const float COEFFICIENT_FRICTION_STEEL_STEEL = 0.8f;

	public const float COEFFICIENT_FRICTION_WOOD_WOOD = 0.37f;

	public const float COEFFICIENT_FRICTION_CONCRETE_DRY_RUBBER = 2.6f;

	public const float COEFFICIENT_FRICTION_CONCRETE_WET_RUBBER = 0.3f;

	public const float COEFFICIENT_FRICTION_ALUMINUM_STEEL = 0.61f;

	public const float COEFFICIENT_FRICTION_POLYTHENE_STEEL = 0.2f;

	public const float COEFFICIENT_FRICTION_COPPER_STEEL = 0.53f;

	public const float COEFFICIENT_FRICTION_BRASS_STEEL = 0.51f;

	public const float COEFFICIENT_FRICTION_CONCRETE_WOOD = 0.62f;

	public const float COEFFICIENT_FRICTION_METAL_WOOD = 0.4f;

	public const float COEFFICIENT_FRICTION_COPPER_GLASS = 0.68f;

	public const float COEFFICIENT_FRICTION_CASTIRON_COPPER = 1.05f;

	public const float COEFFICIENT_FRICTION_CASTIRON_ZINC = 0.85f;

	public const byte OBJECT_NORMAL = 1;

	public const byte OBJECT_PLAYER = 2;

	public const byte OBJECT_COLLISION = 4;

	public const byte OBJECT_PLAYER_COLLISION = 8;

	public const byte OBJECT_LASER_COLLISION = 16;

	public const byte OBJECT_ROUTING_COLLISION = 32;

	public const byte OBJECT_NM_CL_PCL = 141;

	public const byte OBJECT_NM_PL_CL_PCL = 143;

	public const byte OBJECT_NM_PL_CL_PCL_RC = 175;

	public const byte OBJECT_NM_PL_CL = 135;

	public const byte OBJECT_NM_PCL_CL_LSR = 157;

	public const byte OBJECT_NM_CL = 133;

	public const byte OBJECT_NM_CL_LSR = 149;

	public const byte OBJECT_LSR = 144;

	public const byte OBJECT_RC = 160;

	public const byte OBJECT_ACTIVE = 128;

	public const byte OBJECT_STATUS_COLLISION = 8;

	public const byte UI_WINDOW_GROUP_MAIN = 0;

	public const byte UI_WINDOW_GROUP_LEVEL_EDITOR = 1;

	public const byte UI_GROUP_OPTIONS_WINDOW = 0;

	public const byte UI_GROUP_MAIN_MENU = 1;

	public const byte UI_GROUP_BUYME_WINDOW = 2;

	public const byte UI_GROUP_CONFIRM_WINDOW = 3;

	public const byte UI_GROUP_SIGNIN_WINDOW = 4;

	public const byte UI_GROUP_INGAME_MENU = 5;

	public const byte UI_GROUP_PLAY_MENU = 6;

	public const byte UI_GROUP_GAME_WINDOWS = 7;

	public const byte UI_GROUP_LEVEL_EDITOR = 10;

	public const byte UI_GROUP_LEVEL_EDITOR_WEAPON_EDITOR = 11;

	public const byte UI_ACTION_CLOSE_WINDOW = 1;

	public const byte UI_ACTION_CHANGE_VALUE = 2;

	public const byte UI_ACTION_A_PRESSED = 1;

	public const byte UI_ACTION_B_PRESSED = 2;

	public const byte UI_ACTION_X_PRESSED = 4;

	public const byte UI_ACTION_Y_PRESSED = 8;

	public const byte UI_ACTION_DPAD_LEFT_PRESSED = 5;

	public const byte UI_ACTION_DPAD_RIGHT_PRESSED = 6;

	public const byte UI_ACTION_SHOW_WINDOW = 254;

	public const byte UI_ACTION_CHECK_IF_OK_TO_CLOSE_WINDOW = byte.MaxValue;

	public const byte WINDOW_COMPONENT_NONE = 0;

	public const byte WINDOW_COMPONENT_BUTTON = 1;

	public const byte WINDOW_COMPONENT_CHECKBOX = 2;

	public const byte WINDOW_COMPONENT_SLIDER = 3;

	public const byte WINDOW_COMPONENT_LABEL = 4;

	public const byte WINDOW_COMPONENT_GROUP = 5;

	public const byte WINDOW_COMPONENT_TEXT_BUTTON = 6;

	public const byte WINDOW_COMPONENT_TEXT_AREA = 7;

	public const byte WINDOW_COMPONENT_STATIC_GRAPHIC = 8;

	public const byte MOUNT_FIXED_PLAYER = 0;

	public const byte MOUNT_FIXED_WEAPON = 1;

	public const byte MOUNT_FIXED_OBJECT = 2;

	public const byte WEAPON_ATTACHMENT_SCOPE = 0;

	public const byte WEAPON_ATTACHMENT_FOREGRIP = 1;

	public const byte WEAPON_ATTACHMENT_BARREL = 2;

	public const byte WEAPON_ATTACHMENT_ENERGYDEVICE = 3;

	public const byte WEAPON_ATTACHMENT_SKIN = 4;

	public const byte PLAYER_PREFERENCE_SCOPE = 0;

	public const byte PLAYER_PREFERENCE_FOREGRIP = 1;

	public const byte PLAYER_PREFERENCE_BARREL = 2;

	public const byte PLAYER_PREFERENCE_ENERGYDEVICE = 3;

	public const byte PLAYER_PREFERENCE_SKIN = 4;

	public const byte PLAYER_PREFERENCE_TAUNT = 5;

	public const byte SKYBOX_TYPE_NONE = 0;

	public const byte SKYBOX_TYPE_NORMAL_SKY = 1;

	public const byte SKYBOX_TYPE_SPACE = 2;

	public const byte SKYDOME_TYPE_NORMAL_SKY = 3;
}
