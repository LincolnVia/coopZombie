using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Net;
using WindowsGame1;

namespace MainGame;

public class Avatars
{
	public Game1.MasterCollection mainC;

	public void Init_Vars(Game1.MasterCollection master)
	{
		mainC = master;
	}

	public void Init_Avatars()
	{
	}

	public void Load_Avatar_Data(string fileName)
	{
	}

	public void Process_Avatars()
	{
	}

	public void Copy_Bones(ushort playerID, byte animationID, byte category, byte ubID)
	{
	}

	public void Copy_Bone_Matrix(ushort playerID, int boneNumber, byte ubID, byte animationNumber)
	{
	}

	public void Run_Custom_Avatar_Animation(float frameTime, ushort playerID, byte animationID, byte category, byte ubID)
	{
	}

	public void Run_Standard_Avatar_Animation(float frameTime, ushort playerID, byte animationID, byte category, byte ubID)
	{
	}

	public void Render_Avatars()
	{
	}

	public void Render_Vehicle_Select_Avatar(ushort playerID, byte bufID)
	{
	}

	public void Render_Model_List_At_Avatar_Bone(byte listID, ushort itemID, ushort playerID, ushort boneID, ushort matrixID)
	{
	}

	public void Render_Model_At_Avatar_Bone(short modID, ushort playerID, ushort boneID, ushort matrixID)
	{
	}

	public void Set_Matrix_To_Avatar_Bone_Placement_Position(ushort playerID, ushort boneID, ushort matrixID)
	{
	}

	public void Set_View_Matrix(ushort playerID, ushort animationID, ushort boneID, ushort matrixID)
	{
	}

	public void Avatar_Animation_Change(ushort playerID, byte action, byte animationID)
	{
	}

	public void Avatar_Movement_By_List_ID(ushort playerID, byte action, bool loop, byte animationListID, bool cancelOtherGroupAnimations)
	{
	}

	public void Avatar_Movement_By_ID(ushort playerID, bool queue, byte action, bool loop, byte animationID, bool cancelOtherGroupAnimations)
	{
	}

	public bool Need_To_Change_Animation(ushort playerID, byte animationID, byte action)
	{
		return true;
	}

	public void Cancel_Animations_In_Group(ushort playerID, byte group)
	{
	}

	public void Cancel_Animations_In_Group_Except_One(ushort playerID, byte animationID, byte group)
	{
	}

	public void Cancel_All_Animations(ushort playerID)
	{
	}

	public void Avatar_Speed_Adjustment(ushort playerID, byte animationID, float speedFactor)
	{
	}

	public void Avatar_Speed_Adjustment_By_List_ID(ushort playerID, byte animationListID, float speedFactor, float minSpeed)
	{
	}

	public void Reset_Animation(ushort playerID, byte animationID)
	{
	}

	public void Reset_Round()
	{
	}

	public void Reset_Player(ushort playerID)
	{
	}

	public void Run_Avatar_Animation(byte animationID)
	{
	}

	public void Set_Program_Group(ushort playerID, byte category)
	{
	}

	public void Enable_Programs_In_Group(ushort playerID, byte category)
	{
	}

	public void Disable_Programs_In_Group(ushort playerID, byte category)
	{
	}

	public void Set_Avatar_Animation_Stop_Interval(ushort playerID, float interval, byte animationID)
	{
	}

	public void Sync_Avatar_Positions_With_Rendering_Frame()
	{
	}

	public void Update_All_Avatar_Lighting()
	{
	}

	public void Update_Avatar_Description(byte avatarID, AvatarDescription description)
	{
	}

	public void Update_Player_Avatar_Description(byte[] description, byte actID)
	{
	}

	public void Update_Player_Avatar_Location(byte playerID, float x, float y, float z, float rot)
	{
	}

	public void Send_Avatar_Description_To_All_Players(byte playerID)
	{
	}

	public void Send_Avatar_Description_To_New_Player(byte playerID, NetworkGamer newGamer)
	{
	}

	public void Run_Avatar_Animation_From_Network(int actID)
	{
	}

	public void Send_Avatar_Animation(ushort playerID, byte animationID, bool loop)
	{
	}
}
