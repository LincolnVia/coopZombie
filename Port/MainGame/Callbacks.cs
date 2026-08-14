using AI;
using Joints;
using Players;
using Programs;
using Weapons;
using WindowsGame1;

namespace MainGame;

public class Callbacks
{
	public Game1.MasterCollection mainC;

	public void Init_Vars(Game1.MasterCollection masterC)
	{
		mainC = masterC;
	}

	public void Init_Callbacks()
	{
	}

	public void CallBack(ushort playerID, byte callBackType, ushort callBack, ushort variable1, byte action)
	{
		switch (callBackType)
		{
		case 1:
			mainC.switchesMain.SwitchType_One_Callback((byte)callBack, 0);
			break;
		case 2:
			if (action == 0)
			{
				mainC.weaponsMain.Weapon_Chambered((byte)playerID, (byte)variable1);
			}
			else if (playerID == 0)
			{
				MainGame.showCrossHairs[0] = 0;
			}
			if (playerID == 0)
			{
				global::Players.Players.chambering = false;
				mainC.weaponsMain.Check_Weapon_Views();
			}
			break;
		case 3:
			if (action == 0)
			{
				mainC.weaponsMain.Player_Vehicle_Weapon_Reloaded(playerID, variable1, ammoLoadedAlready: false, useSurplusFirst: false);
			}
			else if (playerID == 0)
			{
				global::Players.Players.reloading = false;
				MainGame.showCrossHairs[0] = 0;
			}
			if (playerID == 0)
			{
				mainC.weaponsMain.Check_Weapon_Views();
			}
			break;
		case 4:
			if (action == 0)
			{
				global::Players.Players.viewAdjX = global::Joints.Joints.playerJoints[global::Players.Players.players[0].jointPackage].crouchAdjX;
				global::Players.Players.viewAdjY = global::Joints.Joints.playerJoints[global::Players.Players.players[0].jointPackage].crouchAdjY;
				global::Players.Players.viewAdjZ = global::Joints.Joints.playerJoints[global::Players.Players.players[0].jointPackage].crouchAdjZ;
			}
			break;
		case 5:
			if (action == 0)
			{
				global::Players.Players.viewAdjX = 0f;
				global::Players.Players.viewAdjY = 0f;
				global::Players.Players.viewAdjZ = 0f;
			}
			break;
		case 6:
			if (playerID < MainGame.maxHumanGamePlayers || global::AI.AI.ais[global::Players.Players.players[playerID].aiID].locallyControlled)
			{
				global::Players.Players.players[playerID].onmap = 2;
				global::Players.Players.players[playerID].transporter = 2f;
				global::Players.Players.players[playerID].transporterDirection = -1;
				global::Players.Players.players[playerID].respawnParticle = mainC.renderingMain.New_Particle_Spawn(9, global::Players.Players.players[playerID].respawnParticle, playerID, global::Players.Players.players[playerID].charP.position.v[0], global::Players.Players.players[playerID].charP.position.v[1], global::Players.Players.players[playerID].charP.position.v[2]);
				if (playerID == 0 && MainGame.numLives < 1 && MainGame.gameMode == 0 && MainGame.linearProgression)
				{
					mainC.gameLogic.Game_SP_Game_Ended();
				}
			}
			break;
		case 7:
			if (action == 0)
			{
				global::Players.Players.players[callBack].playerIsMoving = 32766;
				mainC.programsMain.Start_Animation(playerID, ref global::Players.Players.players[playerID].jt1, ref global::Players.Players.players[playerID].animations, global::Players.Players.players[playerID].programCollection, global::Weapons.Weapons.wp1[global::Players.Players.players[playerID].primaryWeaponMountWeapon].AnimationHolding, 1f, 1f);
			}
			break;
		case 8:
			if (action == 0)
			{
				mainC.vehicles.Set_Mount_Weapon(playerID, MainGame.primaryWeaponMount, MainGame.primaryWeaponMount);
				mainC.programsMain.Start_Animation(playerID, ref global::Players.Players.players[playerID].jt1, ref global::Players.Players.players[playerID].animations, global::Players.Players.players[playerID].programCollection, global::Weapons.Weapons.wp1[global::Players.Players.players[playerID].primaryWeaponMountWeapon].AnimationHolding, 1f, 1f);
				global::Players.Players.players[playerID].renderWeapon = (byte)(global::Players.Players.players[playerID].renderWeapon & -2);
				global::Players.Players.players[playerID].playerIsMoving = 32766;
			}
			else
			{
				mainC.weaponsMain.Cancel_Grenade(playerID, (byte)callBack);
			}
			break;
		case 10:
			MainGame.gearDown[playerID] = (byte)((MainGame.gearDown[playerID] + 1) % 2);
			mainC.inputMain.UI_HUD_Set_LandingGear_Position(MainGame.gearDown[0] == 0);
			break;
		case 12:
			if (action == 0)
			{
				if (global::Players.Players.players[playerID].animations[global::Players.Players.players[playerID].programSwitchWeapons].directionAndSpeed >= 0f)
				{
					mainC.weaponsMain.Switch_Weapons(playerID, (byte)callBack, variable1);
					global::Players.Players.players[playerID].primaryWeaponMountWeapon = (sbyte)variable1;
					global::Players.Players.players[playerID].programSwitchWeapons = global::Weapons.Weapons.wp1[global::Players.Players.players[playerID].primaryWeaponMountWeapon].AnimationChangeWeapon;
					mainC.programsMain.Start_Animation(playerID, ref global::Players.Players.players[playerID].jt1, ref global::Players.Players.players[playerID].animations, global::Players.Players.players[playerID].programCollection, global::Players.Players.players[playerID].programSwitchWeapons, -1f, 1f);
				}
				else
				{
					mainC.programsMain.Start_Animation(playerID, ref global::Players.Players.players[playerID].jt1, ref global::Players.Players.players[playerID].animations, global::Players.Players.players[playerID].programCollection, global::Weapons.Weapons.wp1[global::Players.Players.players[playerID].primaryWeaponMountWeapon].AnimationHolding, 1f, 1f);
				}
			}
			if (playerID == 0)
			{
				global::Players.Players.changingWeapons = true;
				if (global::Players.Players.players[playerID].animations[global::Players.Players.players[playerID].programSwitchWeapons].status < 2)
				{
					global::Players.Players.changingWeapons = false;
				}
				mainC.weaponsMain.Check_Weapon_Views();
			}
			break;
		case 13:
			global::Players.Players.players[playerID].primaryWeaponMountWeapon = (sbyte)callBack;
			mainC.weaponsMain.Switch_Weapons(playerID, MainGame.primaryWeaponMount, (byte)callBack);
			global::Players.Players.players[playerID].programSwitchWeapons = global::Weapons.Weapons.wp1[global::Players.Players.players[playerID].primaryWeaponMountWeapon].AnimationChangeWeapon;
			global::Players.Players.players[0].animations[global::Players.Players.players[0].programSwitchWeapons].callBackType = 12;
			mainC.programsMain.Start_Animation(playerID, ref global::Players.Players.players[playerID].jt1, ref global::Players.Players.players[playerID].animations, global::Players.Players.players[playerID].programCollection, global::Players.Players.players[playerID].programSwitchWeapons, -1f, 1f);
			global::Players.Players.players[playerID].renderWeapon = (byte)(global::Players.Players.players[playerID].renderWeapon & -2);
			break;
		case 14:
			mainC.programsMain.Start_Animation(playerID, ref global::Players.Players.players[playerID].jt1, ref global::Players.Players.players[playerID].animations, global::Players.Players.players[playerID].programCollection, callBack, 1f, 1f);
			break;
		case 15:
			MainGame.playerVehicles[playerID].weapons[MainGame.playerVehicles[playerID].mounts[MainGame.primaryWeaponMount].objectID].attachments[0].status = 1;
			mainC.vehicles.Remove_Object_In_Player_Vehicle_Mount(playerID, MainGame.primaryObjectMount);
			if (playerID == 0)
			{
				global::Players.Players.reloading = false;
				MainGame.showCrossHairs[0] = 0;
				mainC.weaponsMain.Check_Weapon_Views();
			}
			break;
		case 9:
		case 11:
			break;
		}
	}

	public void Actions(ushort playerID, byte type, byte actionID, ushort animationID, ushort animationAction)
	{
		switch (type)
		{
		case 0:
			switch (actionID)
			{
			case 0:
				if (mainC.weaponsMain.Load_Ammo_Type_Single_Into_Player_Vehicle_Weapon_Immediately(playerID, MainGame.playerVehicles[playerID].mounts[MainGame.primaryWeaponMount].objectID, 1))
				{
					mainC.programsMain.Re_Start_Animation(playerID, ref global::Players.Players.players[playerID].jt1, ref global::Players.Players.players[playerID].animations, global::Players.Players.players[playerID].programCollection, global::Programs.Programs.pgC[global::Players.Players.players[playerID].programCollection].animation1[animationID].actions[animationAction].var1, 1f, 1f);
				}
				else
				{
					mainC.programsMain.Re_Start_Animation(playerID, ref global::Players.Players.players[playerID].jt1, ref global::Players.Players.players[playerID].animations, global::Players.Players.players[playerID].programCollection, global::Programs.Programs.pgC[global::Players.Players.players[playerID].programCollection].animation1[animationID].actions[animationAction].var2, 1f, 1f);
				}
				mainC.weaponsMain.Player_Vehicle_Weapon_Reloaded(playerID, MainGame.playerVehicles[playerID].mounts[MainGame.primaryWeaponMount].objectID, ammoLoadedAlready: true, useSurplusFirst: false);
				mainC.weaponsMain.Check_Weapon_Views();
				break;
			case 1:
			{
				byte b = (byte)global::Programs.Programs.pgC[global::Players.Players.players[playerID].programCollection].animation1[animationID].actions[animationAction].var1;
				MainGame.playerVehicles[playerID].weapons[MainGame.playerVehicles[playerID].mounts[MainGame.primaryWeaponMount].objectID].attachments[0].status = b;
				if (b == 0)
				{
					mainC.vehicles.Place_Object_In_Player_Vehicle_Mount(playerID, MainGame.primaryObjectMount, (byte)global::Programs.Programs.pgC[global::Players.Players.players[playerID].programCollection].animation1[animationID].actions[animationAction].var2);
				}
				else
				{
					mainC.vehicles.Remove_Object_In_Player_Vehicle_Mount(playerID, MainGame.primaryObjectMount);
				}
				break;
			}
			case 2:
				global::Players.Players.players[playerID].primaryWeaponMountWeapon = (sbyte)global::Programs.Programs.pgC[global::Players.Players.players[playerID].programCollection].animation1[animationID].actions[animationAction].var1;
				mainC.weaponsMain.Switch_Weapons(playerID, MainGame.primaryWeaponMount, (byte)global::Programs.Programs.pgC[global::Players.Players.players[playerID].programCollection].animation1[animationID].actions[animationAction].var1);
				break;
			case 3:
				global::Players.Players.players[playerID].renderWeapon = (byte)(global::Players.Players.players[playerID].renderWeapon | 1);
				if (playerID == 0)
				{
					mainC.weaponsMain.Fire_Bullet_MainPlayer(MainGame.primaryWeaponMount, MainGame.playerVehicles[playerID].mounts[MainGame.primaryWeaponMount].objectID);
				}
				else
				{
					mainC.weaponsMain.Fire_Bullet((short)playerID, MainGame.primaryWeaponMount, MainGame.playerVehicles[playerID].mounts[MainGame.primaryWeaponMount].objectID);
				}
				break;
			}
			break;
		case 1:
			if (actionID == 0)
			{
				mainC.soundsMain.Player_Priority_Sound_From_SoundList(global::Programs.Programs.pgC[global::Players.Players.players[playerID].programCollection].animation1[animationID].actions[animationAction].var1, global::Players.Players.players[playerID].charP.position.v[0], global::Players.Players.players[playerID].charP.position.v[1], global::Players.Players.players[playerID].charP.position.v[2], 0f, 0f, 0f);
			}
			break;
		}
	}
}
