using System;
using InputHandler;
using MainGame;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using Microsoft.Xna.Framework.Net;
using Players;
using Rendering;
using Structs;
using WindowsGame1;

namespace Networking;

public class Networking
{
	public static bool needToSendTeamScores;

	public static byte numUshortsToSend = 0;

	public static byte multiplayerNewGameStatus;

	public static byte[] avatarDescription;

	public static byte[] avatarDescriptionReceived;

	public static bool[] networkBools = new bool[20];

	public static sbyte[] networkSBytes = new sbyte[10];

	public static byte[] networkBytes = new byte[30];

	public static ushort[] networkUShorts = new ushort[10];

	public static short[] networkShorts = new short[10];

	public static int[] networkInts = new int[10];

	public static float sendTeamScoresTimer;

	public static float[] networkFloats = new float[20];

	public static double[] networkDoubles = new double[10];

	public static HalfSingle[] networkHS = new HalfSingle[16];

	public static byte networkState = 0;

	public static byte posMessageType = 0;

	public static bool gameListSearchFinished = false;

	public static bool basicShownGuide = false;

	public static bool shownMarketplace = false;

	public static bool shownGuide = false;

	public static bool networkSessionReady = false;

	public static bool isHost = false;

	public static bool wasHost = false;

	public static bool loggedIn = false;

	public static bool inGame = false;

	public static int numMPGames = 0;

	public static string networkMsg1 = " has joined the match";

	public static string networkMsg2 = " has left the match";

	public static string networkMsg;

	public static StructsClass.Network_Player[] networkPlayers = new StructsClass.Network_Player[4];

	public Game1.MasterCollection mainC;

	public static NetworkSession networkSession;

	public static PacketWriter packetWriter = new PacketWriter();

	public static PacketReader packetReader = new PacketReader();

	public static NetworkGamer sender;

	public static AvailableNetworkSessionCollection searchSessions;

	public static NetworkSessionType onlineSessionType;

	public void Init_Vars(Game1.MasterCollection master)
	{
		mainC = master;
	}

	public void Init_Networking()
	{
		NetworkSession.InviteAccepted += XBOX_Game_Invite_Accepted;
	}

	public void XBOX_Send_Network_Message0(byte type)
	{
		XBOX_aSend_Network_Message(0);
	}

	public void XBOX_Send_Network_Message1(byte type)
	{
		XBOX_aSend_Network_Message(1);
	}

	public void XBOX_Send_Network_Message3(byte type)
	{
		XBOX_aSend_Network_Message(3);
	}

	public void XBOX_Send_Network_Message4(byte type)
	{
		XBOX_aSend_Network_Message(4);
	}

	public void XBOX_Send_Network_Message5(byte type)
	{
		XBOX_aSend_Network_Message(5);
	}

	public void XBOX_Send_Network_Message6(byte type)
	{
		XBOX_aSend_Network_Message(6);
	}

	public void XBOX_Send_Network_Message7(byte type)
	{
		XBOX_aSend_Network_Message(7);
	}

	public void XBOX_Send_Network_Message8(byte type)
	{
		XBOX_aSend_Network_Message(8);
	}

	public void XBOX_Send_Network_Message9(byte type)
	{
		XBOX_aSend_Network_Message(9);
	}

	public void XBOX_Send_Network_Message13(byte type)
	{
		XBOX_aSend_Network_Message(13);
	}

	public void XBOX_Send_Network_Message15(byte type)
	{
		XBOX_aSend_Network_Message(15);
	}

	public void XBOX_Send_Network_Message22(byte type)
	{
		XBOX_aSend_Network_Message(22);
	}

	public void XBOX_Send_Network_Message23(byte type)
	{
		XBOX_aSend_Network_Message(23);
	}

	public void XBOX_Send_Network_Message24(byte type)
	{
		XBOX_aSend_Network_Message(24);
	}

	public void XBOX_Send_Network_Message26(byte type)
	{
		XBOX_aSend_Network_Message(26);
	}

	public void XBOX_Send_Network_Message30(byte type)
	{
		XBOX_aSend_Network_Message(30);
	}

	public void XBOX_Send_Network_Message31(byte type)
	{
		XBOX_aSend_Network_Message(31);
	}

	public void XBOX_Send_Network_Message33(byte type)
	{
		XBOX_aSend_Network_Message(33);
	}

	public void XBOX_Send_Network_Message35(byte type)
	{
		XBOX_aSend_Network_Message(35);
	}

	public void XBOX_Send_Network_Message36(byte type)
	{
		XBOX_aSend_Network_Message(36);
	}

	public void XBOX_Send_Network_Message37(byte type)
	{
		XBOX_aSend_Network_Message(37);
	}

	public void XBOX_Send_Network_Message38(byte type)
	{
		XBOX_aSend_Network_Message(38);
	}

	public void XBOX_Send_Network_Message39(byte type)
	{
		XBOX_aSend_Network_Message(39);
	}

	public void XBOX_Send_Network_Message42(byte type)
	{
		XBOX_aSend_Network_Message(42);
	}

	public void XBOX_Send_Network_Message44(byte type)
	{
		XBOX_aSend_Network_Message(44);
	}

	public void XBOX_Send_Network_Message45(byte type)
	{
		XBOX_aSend_Network_Message(45);
	}

	public void XBOX_Send_Network_Message48(byte type)
	{
		XBOX_aSend_Network_Message(48);
	}

	public void XBOX_Send_Network_Message49(byte type)
	{
		XBOX_aSend_Network_Message(49);
	}

	public void XBOX_Send_Network_Message50(byte type)
	{
		XBOX_aSend_Network_Message(50);
	}

	public void XBOX_Send_Network_Message51(byte type)
	{
		XBOX_aSend_Network_Message(51);
	}

	public void XBOX_Send_Network_Message53(byte type)
	{
		XBOX_aSend_Network_Message(53);
	}

	public void XBOX_Send_Network_Message55(byte type)
	{
		XBOX_aSend_Network_Message(55);
	}

	public void XBOX_Send_Network_Message56(byte type)
	{
		XBOX_aSend_Network_Message(56);
	}

	public void XBOX_Send_Network_Message57(byte type)
	{
		XBOX_aSend_Network_Message(57);
	}

	public void XBOX_Send_Network_Message58(byte type)
	{
		XBOX_aSend_Network_Message(58);
	}

	public void XBOX_Send_Network_Message61(byte type)
	{
		XBOX_aSend_Network_Message(61);
	}

	public void XBOX_Send_Network_Message62(byte type)
	{
		XBOX_aSend_Network_Message(62);
	}

	public void XBOX_Send_Network_Message68(byte type)
	{
		XBOX_aSend_Network_Message(68);
	}

	public void XBOX_Send_Network_Message70(byte type)
	{
		XBOX_aSend_Network_Message(70);
	}

	public void XBOX_Send_Network_Message71(byte type)
	{
		XBOX_aSend_Network_Message(71);
	}

	public void XBOX_Send_Network_Message72(byte type)
	{
		XBOX_aSend_Network_Message(72);
	}

	public void XBOX_Send_Network_Message73(byte type)
	{
		XBOX_aSend_Network_Message(73);
	}

	public void XBOX_Send_Network_Message75(byte type)
	{
		XBOX_aSend_Network_Message(75);
	}

	public void XBOX_Send_Network_Message77(byte type)
	{
		XBOX_aSend_Network_Message(77);
	}

	public void XBOX_Send_Network_Message80(byte type)
	{
		XBOX_aSend_Network_Message(80);
	}

	public void XBOX_Send_Network_Message_ToTeam(byte type, ushort teamID)
	{
	}

	public void XBOX_aSend_Network_Message(byte type)
	{
		try
		{
			switch (type)
			{
			case 0:
				packetWriter.Write((byte)0);
				packetWriter.Write(networkHS[0].PackedValue);
				packetWriter.Write(networkHS[1].PackedValue);
				packetWriter.Write(networkHS[2].PackedValue);
				packetWriter.Write(networkHS[3].PackedValue);
				packetWriter.Write(networkHS[4].PackedValue);
				packetWriter.Write(networkHS[5].PackedValue);
				packetWriter.Write(networkHS[6].PackedValue);
				packetWriter.Write(networkHS[7].PackedValue);
				networkSession.LocalGamers[global::MainGame.MainGame.localNetworkGamerID].SendData(packetWriter, SendDataOptions.InOrder);
				break;
			case 1:
				packetWriter.Write((byte)1);
				packetWriter.Write(networkHS[0].PackedValue);
				packetWriter.Write(networkHS[1].PackedValue);
				packetWriter.Write(networkHS[2].PackedValue);
				packetWriter.Write(networkHS[3].PackedValue);
				packetWriter.Write(networkHS[4].PackedValue);
				packetWriter.Write(networkHS[5].PackedValue);
				packetWriter.Write(networkHS[6].PackedValue);
				packetWriter.Write(networkSBytes[0]);
				packetWriter.Write(networkSBytes[1]);
				packetWriter.Write(networkSBytes[2]);
				packetWriter.Write(networkDoubles[0]);
				packetWriter.Write(networkBytes[0]);
				networkSession.LocalGamers[global::MainGame.MainGame.localNetworkGamerID].SendData(packetWriter, SendDataOptions.None);
				break;
			case 3:
				packetWriter.Write((byte)3);
				packetWriter.Write(networkBytes[0]);
				packetWriter.Write(networkInts[0]);
				packetWriter.Write(networkInts[1]);
				networkSession.LocalGamers[global::MainGame.MainGame.localNetworkGamerID].SendData(packetWriter, SendDataOptions.Reliable);
				break;
			case 4:
				packetWriter.Write((byte)4);
				packetWriter.Write(networkInts[0]);
				packetWriter.Write(networkInts[1]);
				packetWriter.Write(networkInts[2]);
				packetWriter.Write(networkInts[3]);
				packetWriter.Write(networkInts[4]);
				networkSession.LocalGamers[global::MainGame.MainGame.localNetworkGamerID].SendData(packetWriter, SendDataOptions.None);
				break;
			case 5:
				packetWriter.Write((byte)5);
				packetWriter.Write(networkHS[0].PackedValue);
				packetWriter.Write(networkHS[1].PackedValue);
				packetWriter.Write(networkHS[2].PackedValue);
				packetWriter.Write(networkHS[3].PackedValue);
				packetWriter.Write(networkHS[4].PackedValue);
				packetWriter.Write(networkHS[5].PackedValue);
				packetWriter.Write(networkHS[6].PackedValue);
				packetWriter.Write(networkHS[7].PackedValue);
				packetWriter.Write(networkHS[8].PackedValue);
				packetWriter.Write(networkHS[9].PackedValue);
				packetWriter.Write(networkHS[10].PackedValue);
				packetWriter.Write(networkHS[11].PackedValue);
				packetWriter.Write(networkHS[12].PackedValue);
				packetWriter.Write(networkHS[13].PackedValue);
				packetWriter.Write(networkHS[14].PackedValue);
				packetWriter.Write(networkBytes[0]);
				packetWriter.Write(networkBytes[1]);
				packetWriter.Write(networkBytes[2]);
				packetWriter.Write(networkBytes[3]);
				networkSession.LocalGamers[global::MainGame.MainGame.localNetworkGamerID].SendData(packetWriter, SendDataOptions.Reliable);
				break;
			case 6:
				packetWriter.Write((byte)6);
				packetWriter.Write(networkBytes[0]);
				packetWriter.Write(networkUShorts[0]);
				packetWriter.Write(networkShorts[0]);
				networkSession.LocalGamers[global::MainGame.MainGame.localNetworkGamerID].SendData(packetWriter, SendDataOptions.ReliableInOrder);
				break;
			case 7:
			{
				packetWriter.Write((byte)7);
				packetWriter.Write(numUshortsToSend);
				for (byte b2 = 0; b2 < numUshortsToSend; b2++)
				{
					packetWriter.Write(networkUShorts[b2]);
				}
				networkSession.LocalGamers[global::MainGame.MainGame.localNetworkGamerID].SendData(packetWriter, SendDataOptions.Reliable);
				break;
			}
			case 8:
				packetWriter.Write((byte)8);
				packetWriter.Write(networkBytes[0]);
				packetWriter.Write(networkBools[0]);
				networkSession.LocalGamers[global::MainGame.MainGame.localNetworkGamerID].SendData(packetWriter, SendDataOptions.ReliableInOrder);
				break;
			case 13:
				packetWriter.Write((byte)13);
				packetWriter.Write(networkFloats[0]);
				packetWriter.Write(networkFloats[1]);
				networkSession.LocalGamers[global::MainGame.MainGame.localNetworkGamerID].SendData(packetWriter, SendDataOptions.Reliable);
				break;
			case 15:
				packetWriter.Write((byte)15);
				packetWriter.Write(networkBytes[0]);
				packetWriter.Write(networkBytes[1]);
				packetWriter.Write(networkBytes[2]);
				packetWriter.Write(networkBytes[3]);
				packetWriter.Write(networkBytes[4]);
				packetWriter.Write(networkBytes[5]);
				packetWriter.Write(networkBytes[6]);
				packetWriter.Write(networkHS[0].PackedValue);
				networkSession.LocalGamers[global::MainGame.MainGame.localNetworkGamerID].SendData(packetWriter, SendDataOptions.ReliableInOrder);
				break;
			case 26:
				packetWriter.Write((byte)26);
				networkSession.LocalGamers[global::MainGame.MainGame.localNetworkGamerID].SendData(packetWriter, SendDataOptions.ReliableInOrder);
				break;
			case 29:
				packetWriter.Write((byte)29);
				packetWriter.Write(networkHS[0].PackedValue);
				packetWriter.Write(networkHS[1].PackedValue);
				packetWriter.Write(networkHS[2].PackedValue);
				packetWriter.Write(networkHS[3].PackedValue);
				packetWriter.Write(networkHS[4].PackedValue);
				packetWriter.Write(networkHS[5].PackedValue);
				packetWriter.Write(networkHS[6].PackedValue);
				packetWriter.Write(networkHS[7].PackedValue);
				packetWriter.Write(networkHS[8].PackedValue);
				packetWriter.Write(networkHS[9].PackedValue);
				packetWriter.Write(networkHS[10].PackedValue);
				packetWriter.Write(networkHS[11].PackedValue);
				packetWriter.Write(networkHS[12].PackedValue);
				packetWriter.Write(networkHS[13].PackedValue);
				packetWriter.Write(networkHS[14].PackedValue);
				packetWriter.Write(networkBytes[0]);
				networkSession.LocalGamers[global::MainGame.MainGame.localNetworkGamerID].SendData(packetWriter, SendDataOptions.InOrder);
				break;
			case 30:
				packetWriter.Write((byte)30);
				packetWriter.Write((short)avatarDescription.Length);
				packetWriter.Write(avatarDescription);
				networkSession.LocalGamers[global::MainGame.MainGame.localNetworkGamerID].SendData(packetWriter, SendDataOptions.Reliable);
				break;
			case 31:
				packetWriter.Write((byte)31);
				packetWriter.Write(networkBytes[0]);
				networkSession.LocalGamers[global::MainGame.MainGame.localNetworkGamerID].SendData(packetWriter, SendDataOptions.Reliable);
				break;
			case 33:
				packetWriter.Write((byte)33);
				packetWriter.Write(networkBytes[0]);
				networkSession.LocalGamers[global::MainGame.MainGame.localNetworkGamerID].SendData(packetWriter, SendDataOptions.Reliable);
				break;
			case 35:
				packetWriter.Write((byte)35);
				networkSession.LocalGamers[global::MainGame.MainGame.localNetworkGamerID].SendData(packetWriter, SendDataOptions.Reliable);
				break;
			case 36:
			{
				packetWriter.Write((byte)36);
				packetWriter.Write(networkBytes[0]);
				byte b = networkBytes[0];
				byte b2 = 0;
				byte b3 = 1;
				while (b2 < b)
				{
					packetWriter.Write(networkBytes[b3++]);
					b2++;
				}
				networkSession.LocalGamers[global::MainGame.MainGame.localNetworkGamerID].SendData(packetWriter, SendDataOptions.Reliable);
				break;
			}
			case 37:
				packetWriter.Write((byte)37);
				packetWriter.Write(networkInts[0]);
				packetWriter.Write(networkBytes[0]);
				networkSession.LocalGamers[global::MainGame.MainGame.localNetworkGamerID].SendData(packetWriter, SendDataOptions.Reliable);
				break;
			case 38:
				packetWriter.Write((byte)38);
				networkSession.LocalGamers[global::MainGame.MainGame.localNetworkGamerID].SendData(packetWriter, SendDataOptions.Reliable);
				break;
			case 39:
				packetWriter.Write((byte)39);
				packetWriter.Write(networkInts[0]);
				packetWriter.Write(networkShorts[0]);
				networkSession.LocalGamers[global::MainGame.MainGame.localNetworkGamerID].SendData(packetWriter, SendDataOptions.Reliable);
				break;
			case 42:
				packetWriter.Write((byte)42);
				packetWriter.Write(networkUShorts[0]);
				networkSession.LocalGamers[global::MainGame.MainGame.localNetworkGamerID].SendData(packetWriter, SendDataOptions.Reliable);
				break;
			case 44:
				packetWriter.Write((byte)44);
				packetWriter.Write(networkBools[0]);
				packetWriter.Write(networkBytes[0]);
				networkSession.LocalGamers[global::MainGame.MainGame.localNetworkGamerID].SendData(packetWriter, SendDataOptions.Reliable);
				break;
			case 45:
				packetWriter.Write((byte)45);
				packetWriter.Write(networkBytes[0]);
				networkSession.LocalGamers[global::MainGame.MainGame.localNetworkGamerID].SendData(packetWriter, SendDataOptions.Reliable);
				break;
			case 48:
				packetWriter.Write((byte)48);
				packetWriter.Write(networkUShorts[0]);
				packetWriter.Write(networkHS[0].PackedValue);
				networkSession.LocalGamers[global::MainGame.MainGame.localNetworkGamerID].SendData(packetWriter, SendDataOptions.Reliable);
				break;
			case 49:
				packetWriter.Write((byte)49);
				packetWriter.Write(networkUShorts[0]);
				packetWriter.Write(networkShorts[0]);
				networkSession.LocalGamers[global::MainGame.MainGame.localNetworkGamerID].SendData(packetWriter, SendDataOptions.Reliable);
				break;
			case 50:
				packetWriter.Write((byte)50);
				packetWriter.Write(networkInts[0]);
				packetWriter.Write(networkInts[1]);
				packetWriter.Write(networkInts[2]);
				networkSession.LocalGamers[global::MainGame.MainGame.localNetworkGamerID].SendData(packetWriter, SendDataOptions.Reliable);
				break;
			case 51:
				packetWriter.Write((byte)51);
				packetWriter.Write(networkInts[0]);
				packetWriter.Write(networkInts[1]);
				networkSession.LocalGamers[global::MainGame.MainGame.localNetworkGamerID].SendData(packetWriter, SendDataOptions.Reliable);
				break;
			case 53:
				packetWriter.Write((byte)53);
				networkSession.LocalGamers[global::MainGame.MainGame.localNetworkGamerID].SendData(packetWriter, SendDataOptions.Reliable);
				break;
			case 55:
				packetWriter.Write((byte)55);
				packetWriter.Write(networkUShorts[0]);
				packetWriter.Write(networkBytes[0]);
				networkSession.LocalGamers[global::MainGame.MainGame.localNetworkGamerID].SendData(packetWriter, SendDataOptions.Reliable);
				break;
			case 56:
				packetWriter.Write((byte)56);
				packetWriter.Write(networkBytes[0]);
				packetWriter.Write(networkBools[0]);
				networkSession.LocalGamers[global::MainGame.MainGame.localNetworkGamerID].SendData(packetWriter, SendDataOptions.Reliable);
				break;
			case 57:
				packetWriter.Write((byte)57);
				packetWriter.Write(networkUShorts[0]);
				networkSession.LocalGamers[global::MainGame.MainGame.localNetworkGamerID].SendData(packetWriter, SendDataOptions.None);
				break;
			case 58:
				packetWriter.Write((byte)58);
				packetWriter.Write(networkBytes[0]);
				packetWriter.Write(networkHS[0].PackedValue);
				packetWriter.Write(networkHS[1].PackedValue);
				networkSession.LocalGamers[global::MainGame.MainGame.localNetworkGamerID].SendData(packetWriter, SendDataOptions.Reliable);
				break;
			case 61:
				packetWriter.Write((byte)61);
				packetWriter.Write(networkUShorts[0]);
				networkSession.LocalGamers[global::MainGame.MainGame.localNetworkGamerID].SendData(packetWriter, SendDataOptions.Reliable);
				break;
			case 62:
				packetWriter.Write((byte)62);
				packetWriter.Write(networkHS[0].PackedValue);
				networkSession.LocalGamers[global::MainGame.MainGame.localNetworkGamerID].SendData(packetWriter, SendDataOptions.Reliable);
				break;
			case 68:
				packetWriter.Write((byte)68);
				packetWriter.Write(networkBytes[0]);
				packetWriter.Write(networkShorts[0]);
				networkSession.LocalGamers[global::MainGame.MainGame.localNetworkGamerID].SendData(packetWriter, SendDataOptions.Reliable);
				break;
			case 70:
				packetWriter.Write((byte)70);
				packetWriter.Write(networkBytes[0]);
				packetWriter.Write(networkBytes[1]);
				packetWriter.Write(networkInts[0]);
				networkSession.LocalGamers[global::MainGame.MainGame.localNetworkGamerID].SendData(packetWriter, SendDataOptions.Reliable);
				break;
			case 71:
				packetWriter.Write((byte)71);
				packetWriter.Write(networkBytes[0]);
				networkSession.LocalGamers[global::MainGame.MainGame.localNetworkGamerID].SendData(packetWriter, SendDataOptions.Reliable);
				break;
			case 72:
				packetWriter.Write((byte)72);
				packetWriter.Write(networkBytes[0]);
				packetWriter.Write(networkHS[0].PackedValue);
				packetWriter.Write(networkHS[1].PackedValue);
				packetWriter.Write(networkHS[2].PackedValue);
				packetWriter.Write(networkHS[3].PackedValue);
				packetWriter.Write(networkHS[4].PackedValue);
				packetWriter.Write(networkHS[5].PackedValue);
				networkSession.LocalGamers[global::MainGame.MainGame.localNetworkGamerID].SendData(packetWriter, SendDataOptions.Reliable);
				break;
			case 73:
			{
				packetWriter.Write((byte)73);
				ushort num = networkUShorts[0];
				packetWriter.Write(num);
				for (ushort num2 = 0; num2 < num; num2++)
				{
					packetWriter.Write(networkShorts[num2]);
				}
				packetWriter.Write(networkFloats[0]);
				networkSession.LocalGamers[global::MainGame.MainGame.localNetworkGamerID].SendData(packetWriter, SendDataOptions.Reliable);
				break;
			}
			case 75:
				packetWriter.Write((byte)75);
				packetWriter.Write(networkShorts[0]);
				packetWriter.Write(networkUShorts[0]);
				networkSession.LocalGamers[global::MainGame.MainGame.localNetworkGamerID].SendData(packetWriter, SendDataOptions.Reliable);
				break;
			case 77:
				packetWriter.Write((byte)77);
				packetWriter.Write(networkUShorts[0]);
				packetWriter.Write(networkHS[0].PackedValue);
				packetWriter.Write(networkHS[1].PackedValue);
				packetWriter.Write(networkHS[2].PackedValue);
				networkSession.LocalGamers[global::MainGame.MainGame.localNetworkGamerID].SendData(packetWriter, SendDataOptions.Reliable);
				break;
			case 80:
				packetWriter.Write((byte)80);
				networkSession.LocalGamers[global::MainGame.MainGame.localNetworkGamerID].SendData(packetWriter, SendDataOptions.Reliable);
				break;
			case 2:
			case 9:
			case 10:
			case 11:
			case 12:
			case 14:
			case 16:
			case 17:
			case 18:
			case 19:
			case 20:
			case 21:
			case 22:
			case 23:
			case 24:
			case 25:
			case 27:
			case 28:
			case 32:
			case 34:
			case 40:
			case 41:
			case 43:
			case 46:
			case 47:
			case 52:
			case 54:
			case 59:
			case 60:
			case 63:
			case 64:
			case 65:
			case 66:
			case 67:
			case 69:
			case 74:
			case 76:
			case 78:
			case 79:
				break;
			}
		}
		catch
		{
		}
	}

	public void XBOX_Send_Network_Message_To_Gamer(byte type, NetworkGamer newGamer)
	{
		try
		{
			switch (type)
			{
			case 0:
				packetWriter.Write((byte)0);
				packetWriter.Write(networkHS[0].PackedValue);
				packetWriter.Write(networkHS[1].PackedValue);
				packetWriter.Write(networkHS[2].PackedValue);
				packetWriter.Write(networkHS[3].PackedValue);
				packetWriter.Write(networkHS[4].PackedValue);
				packetWriter.Write(networkHS[5].PackedValue);
				packetWriter.Write(networkHS[6].PackedValue);
				packetWriter.Write(networkHS[7].PackedValue);
				networkSession.LocalGamers[global::MainGame.MainGame.localNetworkGamerID].SendData(packetWriter, SendDataOptions.InOrder, newGamer);
				break;
			case 2:
				packetWriter.Write((byte)2);
				packetWriter.Write(networkBytes[0]);
				packetWriter.Write(networkInts[0]);
				packetWriter.Write(networkInts[1]);
				packetWriter.Write(networkHS[0].PackedValue);
				networkSession.LocalGamers[global::MainGame.MainGame.localNetworkGamerID].SendData(packetWriter, SendDataOptions.Reliable, newGamer);
				break;
			case 4:
				packetWriter.Write((byte)4);
				packetWriter.Write(networkInts[0]);
				packetWriter.Write(networkInts[1]);
				packetWriter.Write(networkInts[2]);
				packetWriter.Write(networkInts[3]);
				packetWriter.Write(networkInts[4]);
				networkSession.LocalGamers[global::MainGame.MainGame.localNetworkGamerID].SendData(packetWriter, SendDataOptions.Reliable, newGamer);
				break;
			case 10:
			{
				packetWriter.Write((byte)10);
				packetWriter.Write((byte)networkInts[0]);
				for (ushort num2 = 0; num2 < networkInts[0]; num2++)
				{
					packetWriter.Write(networkBytes[num2]);
					packetWriter.Write(networkBools[num2]);
					packetWriter.Write(networkSBytes[num2]);
					packetWriter.Write(networkShorts[num2]);
				}
				networkSession.LocalGamers[global::MainGame.MainGame.localNetworkGamerID].SendData(packetWriter, SendDataOptions.Reliable, newGamer);
				break;
			}
			case 11:
				packetWriter.Write((byte)11);
				packetWriter.Write(networkBytes[0]);
				networkSession.LocalGamers[global::MainGame.MainGame.localNetworkGamerID].SendData(packetWriter, SendDataOptions.Reliable, newGamer);
				break;
			case 13:
				packetWriter.Write((byte)13);
				packetWriter.Write(networkFloats[0]);
				packetWriter.Write(networkFloats[1]);
				networkSession.LocalGamers[global::MainGame.MainGame.localNetworkGamerID].SendData(packetWriter, SendDataOptions.Reliable, newGamer);
				break;
			case 15:
				packetWriter.Write((byte)15);
				packetWriter.Write(networkBytes[0]);
				packetWriter.Write(networkBytes[1]);
				packetWriter.Write(networkBytes[2]);
				packetWriter.Write(networkBytes[3]);
				packetWriter.Write(networkBytes[4]);
				packetWriter.Write(networkBytes[5]);
				packetWriter.Write(networkBytes[6]);
				packetWriter.Write(networkHS[0].PackedValue);
				networkSession.LocalGamers[global::MainGame.MainGame.localNetworkGamerID].SendData(packetWriter, SendDataOptions.ReliableInOrder, newGamer);
				break;
			case 27:
				packetWriter.Write((byte)27);
				packetWriter.Write(networkHS[0].PackedValue);
				networkSession.LocalGamers[global::MainGame.MainGame.localNetworkGamerID].SendData(packetWriter, SendDataOptions.Reliable, newGamer);
				break;
			case 29:
				packetWriter.Write((byte)29);
				packetWriter.Write(networkHS[0].PackedValue);
				packetWriter.Write(networkHS[1].PackedValue);
				packetWriter.Write(networkHS[2].PackedValue);
				packetWriter.Write(networkHS[3].PackedValue);
				packetWriter.Write(networkHS[4].PackedValue);
				packetWriter.Write(networkHS[5].PackedValue);
				packetWriter.Write(networkHS[6].PackedValue);
				packetWriter.Write(networkHS[7].PackedValue);
				packetWriter.Write(networkHS[8].PackedValue);
				packetWriter.Write(networkHS[9].PackedValue);
				packetWriter.Write(networkHS[10].PackedValue);
				packetWriter.Write(networkHS[11].PackedValue);
				packetWriter.Write(networkHS[12].PackedValue);
				packetWriter.Write(networkHS[13].PackedValue);
				packetWriter.Write(networkHS[14].PackedValue);
				packetWriter.Write(networkBytes[0]);
				networkSession.LocalGamers[global::MainGame.MainGame.localNetworkGamerID].SendData(packetWriter, SendDataOptions.InOrder, newGamer);
				break;
			case 30:
				packetWriter.Write((byte)30);
				packetWriter.Write((short)avatarDescription.Length);
				packetWriter.Write(avatarDescription);
				networkSession.LocalGamers[global::MainGame.MainGame.localNetworkGamerID].SendData(packetWriter, SendDataOptions.Reliable, newGamer);
				break;
			case 32:
				packetWriter.Write((byte)32);
				packetWriter.Write(networkBytes[0]);
				networkSession.LocalGamers[global::MainGame.MainGame.localNetworkGamerID].SendData(packetWriter, SendDataOptions.Reliable, newGamer);
				break;
			case 34:
				packetWriter.Write((byte)34);
				packetWriter.Write(networkHS[0].PackedValue);
				networkSession.LocalGamers[global::MainGame.MainGame.localNetworkGamerID].SendData(packetWriter, SendDataOptions.InOrder, newGamer);
				break;
			case 36:
			{
				packetWriter.Write((byte)36);
				packetWriter.Write(networkBytes[0]);
				ushort num = networkBytes[0];
				ushort num2 = 0;
				ushort num3 = 1;
				while (num2 < num)
				{
					packetWriter.Write(networkBytes[num3++]);
					num2++;
				}
				networkSession.LocalGamers[global::MainGame.MainGame.localNetworkGamerID].SendData(packetWriter, SendDataOptions.Reliable, newGamer);
				break;
			}
			case 37:
				packetWriter.Write((byte)37);
				packetWriter.Write(networkInts[0]);
				packetWriter.Write(networkBytes[0]);
				networkSession.LocalGamers[global::MainGame.MainGame.localNetworkGamerID].SendData(packetWriter, SendDataOptions.Reliable, newGamer);
				break;
			case 39:
				packetWriter.Write((byte)39);
				packetWriter.Write(networkInts[0]);
				packetWriter.Write(networkShorts[0]);
				networkSession.LocalGamers[global::MainGame.MainGame.localNetworkGamerID].SendData(packetWriter, SendDataOptions.Reliable, newGamer);
				break;
			case 40:
				packetWriter.Write((byte)40);
				packetWriter.Write(networkBytes[0]);
				networkSession.LocalGamers[global::MainGame.MainGame.localNetworkGamerID].SendData(packetWriter, SendDataOptions.Reliable, newGamer);
				break;
			case 41:
				packetWriter.Write((byte)41);
				packetWriter.Write(networkInts[0]);
				packetWriter.Write(networkFloats[0]);
				packetWriter.Write(networkFloats[1]);
				packetWriter.Write(networkFloats[2]);
				networkSession.LocalGamers[global::MainGame.MainGame.localNetworkGamerID].SendData(packetWriter, SendDataOptions.Reliable, newGamer);
				break;
			case 42:
				packetWriter.Write((byte)42);
				packetWriter.Write(networkUShorts[0]);
				networkSession.LocalGamers[global::MainGame.MainGame.localNetworkGamerID].SendData(packetWriter, SendDataOptions.Reliable, newGamer);
				break;
			case 43:
				packetWriter.Write((byte)43);
				packetWriter.Write(networkUShorts[0]);
				networkSession.LocalGamers[global::MainGame.MainGame.localNetworkGamerID].SendData(packetWriter, SendDataOptions.Reliable, newGamer);
				break;
			case 46:
			{
				packetWriter.Write((byte)46);
				packetWriter.Write(networkUShorts[0]);
				ushort num = networkUShorts[0];
				for (ushort num2 = 0; num2 < num; num2++)
				{
					packetWriter.Write(networkHS[num2].PackedValue);
				}
				networkSession.LocalGamers[global::MainGame.MainGame.localNetworkGamerID].SendData(packetWriter, SendDataOptions.Reliable, newGamer);
				break;
			}
			case 47:
			{
				packetWriter.Write((byte)47);
				packetWriter.Write(networkUShorts[0]);
				packetWriter.Write(networkUShorts[1]);
				packetWriter.Write(networkUShorts[2]);
				ushort num = networkUShorts[0];
				ushort num2 = 0;
				ushort num3 = 0;
				while (num2 < num)
				{
					packetWriter.Write(networkBytes[num3++]);
					packetWriter.Write(networkBytes[num3++]);
					packetWriter.Write(networkHS[num2].PackedValue);
					num2++;
				}
				networkSession.LocalGamers[global::MainGame.MainGame.localNetworkGamerID].SendData(packetWriter, SendDataOptions.Reliable, newGamer);
				break;
			}
			case 50:
				packetWriter.Write((byte)50);
				packetWriter.Write(networkInts[0]);
				packetWriter.Write(networkInts[1]);
				packetWriter.Write(networkInts[2]);
				networkSession.LocalGamers[global::MainGame.MainGame.localNetworkGamerID].SendData(packetWriter, SendDataOptions.Reliable, newGamer);
				break;
			case 54:
				packetWriter.Write((byte)54);
				packetWriter.Write(networkUShorts[0]);
				packetWriter.Write(networkUShorts[1]);
				networkSession.LocalGamers[global::MainGame.MainGame.localNetworkGamerID].SendData(packetWriter, SendDataOptions.Reliable, newGamer);
				break;
			case 59:
				packetWriter.Write((byte)59);
				networkSession.LocalGamers[global::MainGame.MainGame.localNetworkGamerID].SendData(packetWriter, SendDataOptions.Reliable, newGamer);
				break;
			case 60:
			{
				packetWriter.Write((byte)60);
				packetWriter.Write(networkUShorts[0]);
				ushort num = networkUShorts[0];
				ushort num2 = 0;
				ushort num3 = 0;
				while (num2 < num)
				{
					packetWriter.Write(networkBytes[num2]);
					packetWriter.Write(networkBools[num3++]);
					packetWriter.Write(networkBools[num3++]);
					packetWriter.Write(networkHS[num2].PackedValue);
					num2++;
				}
				packetWriter.Write(networkHS[num2].PackedValue);
				networkSession.LocalGamers[global::MainGame.MainGame.localNetworkGamerID].SendData(packetWriter, SendDataOptions.Reliable, newGamer);
				break;
			}
			case 63:
				packetWriter.Write((byte)63);
				packetWriter.Write(networkHS[0].PackedValue);
				networkSession.LocalGamers[global::MainGame.MainGame.localNetworkGamerID].SendData(packetWriter, SendDataOptions.Reliable, newGamer);
				break;
			case 64:
				packetWriter.Write((byte)64);
				packetWriter.Write(networkBytes[0]);
				networkSession.LocalGamers[global::MainGame.MainGame.localNetworkGamerID].SendData(packetWriter, SendDataOptions.Reliable, newGamer);
				break;
			case 65:
				packetWriter.Write((byte)65);
				packetWriter.Write(networkBools[0]);
				packetWriter.Write(networkBytes[0]);
				packetWriter.Write(networkBytes[1]);
				packetWriter.Write(networkBytes[2]);
				packetWriter.Write(networkHS[0].PackedValue);
				packetWriter.Write(networkHS[1].PackedValue);
				networkSession.LocalGamers[global::MainGame.MainGame.localNetworkGamerID].SendData(packetWriter, SendDataOptions.Reliable, newGamer);
				break;
			case 66:
				packetWriter.Write((byte)66);
				packetWriter.Write(networkBytes[0]);
				packetWriter.Write(networkBytes[1]);
				packetWriter.Write(networkBytes[2]);
				packetWriter.Write(networkBytes[3]);
				packetWriter.Write(networkBools[0]);
				packetWriter.Write(networkBools[1]);
				packetWriter.Write(networkShorts[0]);
				packetWriter.Write(networkUShorts[0]);
				packetWriter.Write(networkUShorts[1]);
				packetWriter.Write(networkFloats[0]);
				packetWriter.Write(networkFloats[1]);
				packetWriter.Write(networkFloats[2]);
				packetWriter.Write(networkFloats[3]);
				packetWriter.Write(networkFloats[4]);
				packetWriter.Write(networkFloats[5]);
				packetWriter.Write(networkFloats[6]);
				packetWriter.Write(networkFloats[7]);
				packetWriter.Write(networkFloats[8]);
				packetWriter.Write(networkFloats[9]);
				packetWriter.Write(networkFloats[10]);
				packetWriter.Write(networkFloats[11]);
				packetWriter.Write(networkFloats[12]);
				packetWriter.Write(networkFloats[13]);
				packetWriter.Write(networkFloats[14]);
				packetWriter.Write(networkFloats[15]);
				packetWriter.Write(networkFloats[16]);
				packetWriter.Write(networkFloats[17]);
				packetWriter.Write(networkFloats[18]);
				packetWriter.Write(networkFloats[19]);
				packetWriter.Write(networkHS[0].PackedValue);
				packetWriter.Write(networkHS[1].PackedValue);
				networkSession.LocalGamers[global::MainGame.MainGame.localNetworkGamerID].SendData(packetWriter, SendDataOptions.Reliable, newGamer);
				break;
			case 67:
				packetWriter.Write((byte)67);
				packetWriter.Write(networkBytes[0]);
				packetWriter.Write(networkBytes[1]);
				packetWriter.Write(networkBytes[2]);
				packetWriter.Write(networkHS[0].PackedValue);
				packetWriter.Write(networkHS[1].PackedValue);
				packetWriter.Write(networkHS[2].PackedValue);
				packetWriter.Write(networkHS[3].PackedValue);
				packetWriter.Write(networkHS[4].PackedValue);
				packetWriter.Write(networkHS[5].PackedValue);
				packetWriter.Write(networkHS[6].PackedValue);
				networkSession.LocalGamers[global::MainGame.MainGame.localNetworkGamerID].SendData(packetWriter, SendDataOptions.InOrder, newGamer);
				break;
			case 69:
				packetWriter.Write((byte)69);
				packetWriter.Write(networkBytes[0]);
				packetWriter.Write(networkBytes[1]);
				packetWriter.Write(networkInts[0]);
				packetWriter.Write(networkHS[0].PackedValue);
				networkSession.LocalGamers[global::MainGame.MainGame.localNetworkGamerID].SendData(packetWriter, SendDataOptions.Reliable, newGamer);
				break;
			case 74:
				packetWriter.Write((byte)74);
				packetWriter.Write(networkBytes[0]);
				packetWriter.Write(networkBytes[1]);
				packetWriter.Write(networkBytes[2]);
				packetWriter.Write(networkHS[0].PackedValue);
				packetWriter.Write(networkHS[1].PackedValue);
				packetWriter.Write(networkHS[2].PackedValue);
				packetWriter.Write(networkHS[3].PackedValue);
				packetWriter.Write(networkHS[4].PackedValue);
				packetWriter.Write(networkHS[5].PackedValue);
				packetWriter.Write(networkHS[6].PackedValue);
				networkSession.LocalGamers[global::MainGame.MainGame.localNetworkGamerID].SendData(packetWriter, SendDataOptions.InOrder, newGamer);
				break;
			case 76:
			{
				packetWriter.Write((byte)76);
				packetWriter.Write(networkUShorts[0]);
				ushort num = networkUShorts[0];
				for (ushort num2 = 0; num2 < num; num2++)
				{
					packetWriter.Write(networkBytes[num2]);
					packetWriter.Write(networkShorts[num2]);
				}
				networkSession.LocalGamers[global::MainGame.MainGame.localNetworkGamerID].SendData(packetWriter, SendDataOptions.Reliable, newGamer);
				break;
			}
			case 77:
				packetWriter.Write((byte)77);
				packetWriter.Write(networkUShorts[0]);
				packetWriter.Write(networkHS[0].PackedValue);
				packetWriter.Write(networkHS[1].PackedValue);
				packetWriter.Write(networkHS[2].PackedValue);
				networkSession.LocalGamers[global::MainGame.MainGame.localNetworkGamerID].SendData(packetWriter, SendDataOptions.Reliable, newGamer);
				break;
			case 78:
				packetWriter.Write((byte)78);
				networkSession.LocalGamers[global::MainGame.MainGame.localNetworkGamerID].SendData(packetWriter, SendDataOptions.Reliable, newGamer);
				break;
			case 79:
			{
				packetWriter.Write((byte)79);
				packetWriter.Write(networkUShorts[0]);
				ushort num = networkUShorts[0];
				ushort num2 = 0;
				ushort num3 = 0;
				while (num2 < num)
				{
					packetWriter.Write(networkBytes[num2]);
					packetWriter.Write(networkHS[num3++].PackedValue);
					packetWriter.Write(networkHS[num3++].PackedValue);
					packetWriter.Write(networkHS[num3++].PackedValue);
					num2++;
				}
				networkSession.LocalGamers[global::MainGame.MainGame.localNetworkGamerID].SendData(packetWriter, SendDataOptions.Reliable, newGamer);
				break;
			}
			}
		}
		catch
		{
		}
	}

	public void XBOX_Send_Network_Message_To_Host(byte type)
	{
		try
		{
			switch (type)
			{
			case 9:
				packetWriter.Write((byte)9);
				packetWriter.Write(networkBytes[0]);
				networkSession.LocalGamers[global::MainGame.MainGame.localNetworkGamerID].SendData(packetWriter, SendDataOptions.Reliable, networkSession.Host);
				break;
			case 12:
				packetWriter.Write((byte)12);
				networkSession.LocalGamers[global::MainGame.MainGame.localNetworkGamerID].SendData(packetWriter, SendDataOptions.Reliable, networkSession.Host);
				break;
			case 28:
				packetWriter.Write((byte)28);
				networkSession.LocalGamers[global::MainGame.MainGame.localNetworkGamerID].SendData(packetWriter, SendDataOptions.Reliable, networkSession.Host);
				break;
			case 81:
				packetWriter.Write((byte)81);
				networkSession.LocalGamers[global::MainGame.MainGame.localNetworkGamerID].SendData(packetWriter, SendDataOptions.Reliable, networkSession.Host);
				break;
			case 82:
				packetWriter.Write((byte)82);
				networkSession.LocalGamers[global::MainGame.MainGame.localNetworkGamerID].SendData(packetWriter, SendDataOptions.Reliable, networkSession.Host);
				break;
			}
		}
		catch
		{
		}
	}

	public void XBOX_Process_Networking(byte threadID)
	{
		try
		{
			if (needToSendTeamScores)
			{
				sendTeamScoresTimer -= global::MainGame.MainGame.frametime;
				if (sendTeamScoresTimer < 0f)
				{
					needToSendTeamScores = false;
					mainC.playersMain.Send_Team_Points();
				}
			}
			while (networkSession.LocalGamers[global::MainGame.MainGame.localNetworkGamerID].IsDataAvailable)
			{
				networkSession.LocalGamers[global::MainGame.MainGame.localNetworkGamerID].ReceiveData(packetReader, out sender);
				if (sender.IsLocal || global::MainGame.MainGame.gameSetupRunning)
				{
					continue;
				}
				switch (packetReader.ReadByte())
				{
				case 0:
					networkHS[0].PackedValue = packetReader.ReadUInt16();
					networkHS[1].PackedValue = packetReader.ReadUInt16();
					networkHS[2].PackedValue = packetReader.ReadUInt16();
					networkHS[3].PackedValue = packetReader.ReadUInt16();
					networkHS[4].PackedValue = packetReader.ReadUInt16();
					networkHS[5].PackedValue = packetReader.ReadUInt16();
					networkHS[6].PackedValue = packetReader.ReadUInt16();
					networkHS[7].PackedValue = packetReader.ReadUInt16();
					mainC.playersMain.Update_Player_Position_From_Network(sender.Id);
					break;
				case 1:
					networkHS[0].PackedValue = packetReader.ReadUInt16();
					networkHS[1].PackedValue = packetReader.ReadUInt16();
					networkHS[2].PackedValue = packetReader.ReadUInt16();
					networkHS[3].PackedValue = packetReader.ReadUInt16();
					networkHS[4].PackedValue = packetReader.ReadUInt16();
					networkHS[5].PackedValue = packetReader.ReadUInt16();
					networkHS[6].PackedValue = packetReader.ReadUInt16();
					networkSBytes[0] = packetReader.ReadSByte();
					networkSBytes[1] = packetReader.ReadSByte();
					networkSBytes[2] = packetReader.ReadSByte();
					networkDoubles[0] = packetReader.ReadDouble();
					networkBytes[0] = packetReader.ReadByte();
					mainC.weaponsMain.Add_Bullet(sender.Id, threadID);
					break;
				case 2:
					networkBytes[0] = packetReader.ReadByte();
					networkInts[0] = packetReader.ReadInt32();
					networkInts[1] = packetReader.ReadInt32();
					networkHS[0].PackedValue = packetReader.ReadUInt16();
					mainC.playersMain.Player_Hit_From_Network(threadID);
					break;
				case 3:
					networkBytes[0] = packetReader.ReadByte();
					networkInts[0] = packetReader.ReadInt32();
					networkInts[1] = packetReader.ReadInt32();
					mainC.playersMain.Player_Killed();
					break;
				case 4:
					networkInts[0] = packetReader.ReadInt32();
					networkInts[1] = packetReader.ReadInt32();
					networkInts[2] = packetReader.ReadInt32();
					networkInts[3] = packetReader.ReadInt32();
					networkInts[4] = packetReader.ReadInt32();
					mainC.playersMain.Receive_Update_Of_Team_Points();
					break;
				case 5:
					networkHS[0].PackedValue = packetReader.ReadUInt16();
					networkHS[1].PackedValue = packetReader.ReadUInt16();
					networkHS[2].PackedValue = packetReader.ReadUInt16();
					networkHS[3].PackedValue = packetReader.ReadUInt16();
					networkHS[4].PackedValue = packetReader.ReadUInt16();
					networkHS[5].PackedValue = packetReader.ReadUInt16();
					networkHS[6].PackedValue = packetReader.ReadUInt16();
					networkHS[7].PackedValue = packetReader.ReadUInt16();
					networkHS[8].PackedValue = packetReader.ReadUInt16();
					networkHS[9].PackedValue = packetReader.ReadUInt16();
					networkHS[10].PackedValue = packetReader.ReadUInt16();
					networkHS[11].PackedValue = packetReader.ReadUInt16();
					networkHS[12].PackedValue = packetReader.ReadUInt16();
					networkHS[13].PackedValue = packetReader.ReadUInt16();
					networkHS[14].PackedValue = packetReader.ReadUInt16();
					networkBytes[0] = packetReader.ReadByte();
					networkBytes[1] = packetReader.ReadByte();
					networkBytes[2] = packetReader.ReadByte();
					networkBytes[3] = packetReader.ReadByte();
					mainC.playersMain.Player_Respawn_From_Network(sender.Id);
					break;
				case 6:
					networkBytes[0] = packetReader.ReadByte();
					networkUShorts[0] = packetReader.ReadUInt16();
					networkShorts[0] = packetReader.ReadInt16();
					mainC.programsMain.Receive_Network_Animation(sender.Id);
					break;
				case 7:
				{
					ushort num = packetReader.ReadByte();
					for (int i = 0; i < num; i++)
					{
						networkUShorts[i] = packetReader.ReadUInt16();
					}
					mainC.gameLogic.Game_Receive_Final_Player_Stats(sender.Id);
					break;
				}
				case 8:
					networkBytes[0] = packetReader.ReadByte();
					networkBools[0] = packetReader.ReadBoolean();
					mainC.switchesMain.Process_SwitchType_One_From_Network_Host();
					break;
				case 9:
					networkBytes[0] = packetReader.ReadByte();
					mainC.switchesMain.Process_SwitchType_One_From_Network();
					break;
				case 10:
				{
					ushort num = packetReader.ReadByte();
					for (int i = 0; i < num; i++)
					{
						networkBytes[i] = packetReader.ReadByte();
						networkBools[i] = packetReader.ReadBoolean();
						networkSBytes[i] = packetReader.ReadSByte();
						networkShorts[i] = packetReader.ReadInt16();
					}
					mainC.programsMain.XBOX_Update_Program_Status_From_Network((byte)num);
					break;
				}
				case 11:
					networkBytes[0] = packetReader.ReadByte();
					mainC.playersMain.Set_Team_From_Network();
					break;
				case 12:
					if (isHost)
					{
						mainC.playersMain.Team_Change_Request(sender.Id, sender);
					}
					break;
				case 13:
					networkFloats[0] = packetReader.ReadSingle();
					networkFloats[1] = packetReader.ReadSingle();
					mainC.maingameMain.Receive_LobbyTimer_Update();
					break;
				case 14:
					networkBytes[0] = packetReader.ReadByte();
					networkBytes[1] = packetReader.ReadByte();
					networkHS[0].PackedValue = packetReader.ReadUInt16();
					networkHS[1].PackedValue = packetReader.ReadUInt16();
					networkHS[2].PackedValue = packetReader.ReadUInt16();
					networkInts[0] = packetReader.ReadInt32();
					global::MainGame.MainGame.Commander_Add_Objective_From_Network();
					break;
				case 15:
					networkBytes[0] = packetReader.ReadByte();
					networkBytes[1] = packetReader.ReadByte();
					networkBytes[2] = packetReader.ReadByte();
					networkBytes[3] = packetReader.ReadByte();
					networkBytes[4] = packetReader.ReadByte();
					networkBytes[5] = packetReader.ReadByte();
					networkBytes[6] = packetReader.ReadByte();
					networkHS[0].PackedValue = packetReader.ReadUInt16();
					mainC.gameLogic.Game_Receive_Update_Of_GameSettings();
					break;
				case 16:
					networkHS[0].PackedValue = packetReader.ReadUInt16();
					global::MainGame.MainGame.Commander_Receive_Update_Of_PlayerHealth(sender.Id);
					break;
				case 17:
					networkBytes[0] = packetReader.ReadByte();
					global::MainGame.MainGame.Commander_Objective_Heal_Completed(sender.Id);
					break;
				case 18:
					networkSBytes[0] = packetReader.ReadSByte();
					networkInts[0] = packetReader.ReadInt32();
					global::MainGame.MainGame.Commander_Points_Received_From_Network();
					break;
				case 19:
					networkBytes[0] = packetReader.ReadByte();
					mainC.playersMain.Adjust_Player_Damage_By_Percent(0, (int)networkBytes[0], sendOnline: true);
					break;
				case 20:
					networkBytes[0] = packetReader.ReadByte();
					networkBytes[1] = packetReader.ReadByte();
					global::MainGame.MainGame.Commander_Remove_Objective_From_Network();
					break;
				case 21:
					networkBytes[0] = packetReader.ReadByte();
					global::MainGame.MainGame.Commander_Objective_Waypoint_Completed(sender.Id);
					break;
				case 22:
					networkBytes[0] = packetReader.ReadByte();
					mainC.playersMain.Player_Teleporting_Out();
					break;
				case 23:
					networkBytes[0] = packetReader.ReadByte();
					mainC.playersMain.Player_Teleporting_In();
					break;
				case 24:
					networkBytes[0] = packetReader.ReadByte();
					global::MainGame.MainGame.Commander_Teleporting_Player();
					break;
				case 25:
					networkHS[0].PackedValue = packetReader.ReadUInt16();
					networkHS[1].PackedValue = packetReader.ReadUInt16();
					networkHS[2].PackedValue = packetReader.ReadUInt16();
					mainC.playersMain.Teleport_In_From_Network();
					break;
				case 26:
					global::MainGame.MainGame.MP_Game_Started(threadID);
					break;
				case 27:
					networkBytes[0] = 3;
					networkHS[0].PackedValue = packetReader.ReadUInt16();
					mainC.gameLogic.Game_Receive_Update_Of_GameSettings();
					break;
				case 28:
					global::MainGame.MainGame.Send_ProgramData_RoundTimer_To_Player(sender);
					break;
				case 29:
					networkHS[0].PackedValue = packetReader.ReadUInt16();
					networkHS[1].PackedValue = packetReader.ReadUInt16();
					networkHS[2].PackedValue = packetReader.ReadUInt16();
					networkHS[3].PackedValue = packetReader.ReadUInt16();
					networkHS[4].PackedValue = packetReader.ReadUInt16();
					networkHS[5].PackedValue = packetReader.ReadUInt16();
					networkHS[6].PackedValue = packetReader.ReadUInt16();
					networkHS[7].PackedValue = packetReader.ReadUInt16();
					networkHS[8].PackedValue = packetReader.ReadUInt16();
					networkHS[9].PackedValue = packetReader.ReadUInt16();
					networkHS[10].PackedValue = packetReader.ReadUInt16();
					networkHS[11].PackedValue = packetReader.ReadUInt16();
					networkHS[12].PackedValue = packetReader.ReadUInt16();
					networkHS[13].PackedValue = packetReader.ReadUInt16();
					networkHS[14].PackedValue = packetReader.ReadUInt16();
					networkBytes[0] = packetReader.ReadByte();
					mainC.playersMain.Update_Player_Matrix_From_Network(sender.Id);
					break;
				case 30:
					avatarDescriptionReceived = packetReader.ReadBytes(packetReader.ReadInt16());
					mainC.avatarMain.Update_Player_Avatar_Description(avatarDescriptionReceived, sender.Id);
					break;
				case 31:
					networkBytes[0] = packetReader.ReadByte();
					mainC.playersMain.Receive_Player_Rank(sender.Id);
					break;
				case 32:
					networkBytes[0] = packetReader.ReadByte();
					mainC.playersMain.Update_Player_Status_From_Network_Airplane(sender.Id, -1);
					break;
				case 33:
					networkBytes[0] = packetReader.ReadByte();
					networkBytes[1] = sender.Id;
					mainC.gameLogic.Game_Misc_Threaded(2);
					break;
				case 34:
					networkBytes[0] = sender.Id;
					networkHS[0].PackedValue = packetReader.ReadUInt16();
					mainC.maingameMain.Receive_Update_Remote_Player_Score();
					break;
				case 35:
					networkBytes[0] = sender.Id;
					mainC.maingameMain.Player_Finished_Race();
					break;
				case 36:
				{
					ushort num = packetReader.ReadByte();
					for (int i = 0; i < num; i++)
					{
						networkBytes[i] = packetReader.ReadByte();
					}
					mainC.weaponsMain.Receive_Player_Weapons_From_Network(sender.Id);
					break;
				}
				case 37:
					networkInts[0] = packetReader.ReadInt32();
					networkBytes[0] = packetReader.ReadByte();
					mainC.maingameMain.Receive_Race_Participant_Status();
					break;
				case 38:
					Player_Loaded(sender);
					break;
				case 39:
					networkInts[0] = packetReader.ReadInt32();
					networkShorts[0] = packetReader.ReadInt16();
					mainC.playersMain.Set_Player_Array_Position_From_Network();
					break;
				case 40:
					networkBytes[0] = sender.Id;
					networkBytes[1] = packetReader.ReadByte();
					mainC.maingameMain.Player_Finished_Lap();
					break;
				case 41:
					networkBytes[0] = sender.Id;
					networkInts[0] = packetReader.ReadInt32();
					networkFloats[0] = packetReader.ReadSingle();
					networkFloats[1] = packetReader.ReadSingle();
					networkFloats[2] = packetReader.ReadSingle();
					mainC.maingameMain.Receive_Highscroe_And_FavoriteAward();
					break;
				case 42:
					networkBytes[0] = sender.Id;
					networkUShorts[0] = packetReader.ReadUInt16();
					mainC.playersMain.Update_Player_Team_From_Network();
					break;
				case 43:
					networkUShorts[0] = packetReader.ReadUInt16();
					mainC.playersMain.Update_Player_Status_From_Network_FPS(sender.Id);
					break;
				case 44:
					networkBools[0] = packetReader.ReadBoolean();
					networkBytes[0] = packetReader.ReadByte();
					mainC.playersMain.Update_Player_Shooting_Status(sender.Id);
					break;
				case 45:
					networkBytes[0] = packetReader.ReadByte();
					mainC.playersMain.Update_Player_Damage_Amount(sender.Id);
					break;
				case 46:
				{
					networkUShorts[0] = packetReader.ReadUInt16();
					ushort num = networkUShorts[0];
					for (int i = 0; i < num; i++)
					{
						networkHS[i].PackedValue = packetReader.ReadUInt16();
					}
					mainC.targetMain.Receive_DamageTargets_From_Host();
					break;
				}
				case 47:
				{
					networkUShorts[0] = packetReader.ReadUInt16();
					networkUShorts[1] = packetReader.ReadUInt16();
					networkUShorts[2] = packetReader.ReadUInt16();
					ushort num = networkUShorts[0];
					int i = 0;
					ushort num2 = 0;
					for (; i < num; i++)
					{
						networkBytes[num2++] = packetReader.ReadByte();
						networkBytes[num2++] = packetReader.ReadByte();
						networkHS[i].PackedValue = packetReader.ReadUInt16();
					}
					mainC.gameobjectMain.Receive_GameObjects_From_Host();
					break;
				}
				case 48:
					networkUShorts[0] = packetReader.ReadUInt16();
					networkHS[0].PackedValue = packetReader.ReadUInt16();
					mainC.gameobjectMain.Receive_Object_Damage(sender.Id);
					break;
				case 49:
					networkUShorts[0] = packetReader.ReadUInt16();
					networkShorts[0] = packetReader.ReadInt16();
					mainC.gameobjectMain.Receive_Object_Destroyed();
					break;
				case 50:
					networkInts[0] = packetReader.ReadInt32();
					networkInts[1] = packetReader.ReadInt32();
					networkInts[2] = packetReader.ReadInt32();
					mainC.playersMain.Receive_Player_Points();
					break;
				case 51:
					networkInts[0] = packetReader.ReadInt32();
					networkInts[1] = packetReader.ReadInt32();
					mainC.playersMain.Receive_Team_Points();
					break;
				case 52:
					networkInts[0] = packetReader.ReadInt32();
					networkInts[1] = packetReader.ReadInt32();
					mainC.playersMain.Receive_Team_Points();
					break;
				case 53:
					mainC.playersMain.Player_Vehicle_Exploded_From_Network(sender.Id, threadID);
					break;
				case 54:
					networkUShorts[0] = packetReader.ReadUInt16();
					networkUShorts[1] = packetReader.ReadUInt16();
					mainC.aiMain.Receive_KillCount_From_Host();
					break;
				case 55:
					networkUShorts[0] = packetReader.ReadUInt16();
					networkBytes[0] = packetReader.ReadByte();
					mainC.gameobjectMain.Update_Game_Object_Team(networkUShorts[0], networkBytes[0]);
					break;
				case 56:
					networkBytes[0] = packetReader.ReadByte();
					networkBools[0] = packetReader.ReadBoolean();
					mainC.avatarMain.Run_Avatar_Animation_From_Network(sender.Id);
					break;
				case 57:
					networkUShorts[0] = packetReader.ReadUInt16();
					mainC.playersMain.Receive_Kill_Streak_Message(sender.Id);
					break;
				case 58:
					networkBytes[0] = packetReader.ReadByte();
					networkHS[0].PackedValue = packetReader.ReadUInt16();
					networkHS[1].PackedValue = packetReader.ReadUInt16();
					mainC.weaponsMain.Launch_Ballistic_Strike_From_Network(sender.Id);
					break;
				case 59:
					mainC.playersMain.Send_Player_Info_To_Gamer(sender);
					break;
				case 60:
				{
					ushort num = packetReader.ReadUInt16();
					int i = 0;
					ushort num2 = 0;
					for (; i < num; i++)
					{
						networkBytes[i] = packetReader.ReadByte();
						networkBools[num2++] = packetReader.ReadBoolean();
						networkBools[num2++] = packetReader.ReadBoolean();
						networkHS[i].PackedValue = packetReader.ReadUInt16();
					}
					networkHS[i].PackedValue = packetReader.ReadUInt16();
					mainC.pickupsMain.Receive_Pickup_Data((byte)num);
					break;
				}
				case 61:
					mainC.pickupsMain.Receive_Pickup_Acquired_Message(packetReader.ReadUInt16());
					break;
				case 62:
					networkHS[0].PackedValue = packetReader.ReadUInt16();
					mainC.pickupsMain.Receive_New_Time_Modifier();
					break;
				case 63:
					networkHS[0].PackedValue = packetReader.ReadUInt16();
					mainC.playersMain.Receive_Invincible_Timer(sender.Id);
					break;
				case 64:
					networkBytes[0] = packetReader.ReadByte();
					mainC.playersMain.Receive_Invincible_Timer(sender.Id);
					break;
				case 65:
					networkBools[0] = packetReader.ReadBoolean();
					networkBytes[0] = packetReader.ReadByte();
					networkBytes[1] = packetReader.ReadByte();
					networkBytes[2] = packetReader.ReadByte();
					networkHS[0].PackedValue = packetReader.ReadUInt16();
					networkHS[1].PackedValue = packetReader.ReadUInt16();
					mainC.aiMain.Receive_AI_Players(0);
					break;
				case 66:
					networkBytes[0] = packetReader.ReadByte();
					networkBytes[1] = packetReader.ReadByte();
					networkBytes[2] = packetReader.ReadByte();
					networkBytes[3] = packetReader.ReadByte();
					networkBools[0] = packetReader.ReadBoolean();
					networkBools[1] = packetReader.ReadBoolean();
					networkShorts[0] = packetReader.ReadInt16();
					networkUShorts[0] = packetReader.ReadUInt16();
					networkUShorts[1] = packetReader.ReadUInt16();
					networkFloats[0] = packetReader.ReadSingle();
					networkFloats[1] = packetReader.ReadSingle();
					networkFloats[2] = packetReader.ReadSingle();
					networkFloats[3] = packetReader.ReadSingle();
					networkFloats[4] = packetReader.ReadSingle();
					networkFloats[5] = packetReader.ReadSingle();
					networkFloats[6] = packetReader.ReadSingle();
					networkFloats[7] = packetReader.ReadSingle();
					networkFloats[8] = packetReader.ReadSingle();
					networkFloats[9] = packetReader.ReadSingle();
					networkFloats[10] = packetReader.ReadSingle();
					networkFloats[11] = packetReader.ReadSingle();
					networkFloats[12] = packetReader.ReadSingle();
					networkFloats[13] = packetReader.ReadSingle();
					networkFloats[14] = packetReader.ReadSingle();
					networkFloats[15] = packetReader.ReadSingle();
					networkFloats[16] = packetReader.ReadSingle();
					networkFloats[17] = packetReader.ReadSingle();
					networkFloats[18] = packetReader.ReadSingle();
					networkFloats[19] = packetReader.ReadSingle();
					networkHS[0].PackedValue = packetReader.ReadUInt16();
					networkHS[1].PackedValue = packetReader.ReadUInt16();
					mainC.aiMain.Receive_AI_Players(1);
					break;
				case 67:
					networkBytes[0] = packetReader.ReadByte();
					networkBytes[1] = packetReader.ReadByte();
					networkBytes[2] = packetReader.ReadByte();
					networkHS[0].PackedValue = packetReader.ReadUInt16();
					networkHS[1].PackedValue = packetReader.ReadUInt16();
					networkHS[2].PackedValue = packetReader.ReadUInt16();
					networkHS[3].PackedValue = packetReader.ReadUInt16();
					networkHS[4].PackedValue = packetReader.ReadUInt16();
					networkHS[5].PackedValue = packetReader.ReadUInt16();
					networkHS[6].PackedValue = packetReader.ReadUInt16();
					mainC.aiMain.Receive_AI_Players(2);
					break;
				case 68:
					networkBytes[0] = packetReader.ReadByte();
					networkShorts[0] = packetReader.ReadInt16();
					mainC.aiMain.Receive_AI_Players(3);
					break;
				case 69:
					networkBytes[0] = packetReader.ReadByte();
					networkBytes[1] = packetReader.ReadByte();
					networkInts[0] = packetReader.ReadInt32();
					networkHS[0].PackedValue = packetReader.ReadUInt16();
					mainC.playersMain.Player_AI_Hit_From_Network(threadID);
					break;
				case 70:
					networkBytes[0] = packetReader.ReadByte();
					networkBytes[1] = packetReader.ReadByte();
					networkInts[0] = packetReader.ReadInt32();
					mainC.playersMain.Player_AI_Killed_From_Network(threadID);
					break;
				case 71:
					networkBytes[0] = packetReader.ReadByte();
					mainC.maingameMain.Receive_Map_Vote(networkBytes[0], sender.Id);
					break;
				case 72:
					networkBytes[0] = packetReader.ReadByte();
					networkHS[0].PackedValue = packetReader.ReadUInt16();
					networkHS[1].PackedValue = packetReader.ReadUInt16();
					networkHS[2].PackedValue = packetReader.ReadUInt16();
					networkHS[3].PackedValue = packetReader.ReadUInt16();
					networkHS[4].PackedValue = packetReader.ReadUInt16();
					networkHS[5].PackedValue = packetReader.ReadUInt16();
					mainC.aiMain.Receive_AI_Players(5);
					break;
				case 73:
				{
					ushort num = packetReader.ReadUInt16();
					networkUShorts[0] = num;
					for (int i = 0; i < num; i++)
					{
						networkShorts[i] = packetReader.ReadInt16();
					}
					networkFloats[0] = packetReader.ReadSingle();
					mainC.aiMain.Update_AI_Controlling_Players_From_Host();
					break;
				}
				case 74:
					networkBytes[0] = packetReader.ReadByte();
					networkBytes[1] = packetReader.ReadByte();
					networkBytes[2] = packetReader.ReadByte();
					networkHS[0].PackedValue = packetReader.ReadUInt16();
					networkHS[1].PackedValue = packetReader.ReadUInt16();
					networkHS[2].PackedValue = packetReader.ReadUInt16();
					networkHS[3].PackedValue = packetReader.ReadUInt16();
					networkHS[4].PackedValue = packetReader.ReadUInt16();
					networkHS[5].PackedValue = packetReader.ReadUInt16();
					networkHS[6].PackedValue = packetReader.ReadUInt16();
					mainC.aiMain.Receive_AI_Players(6);
					break;
				case 75:
					networkShorts[0] = packetReader.ReadInt16();
					mainC.pickupsMain.Receive_Weapon_Pickup_Change_Message(packetReader.ReadUInt16());
					break;
				case 76:
				{
					ushort num = packetReader.ReadUInt16();
					int i = 0;
					ushort num2 = 0;
					for (; i < num; i++)
					{
						networkBytes[i] = packetReader.ReadByte();
						networkShorts[i] = packetReader.ReadInt16();
					}
					mainC.pickupsMain.Receive_Weapon_Pickup_Data((byte)num);
					break;
				}
				case 77:
					networkUShorts[0] = packetReader.ReadUInt16();
					networkHS[0].PackedValue = packetReader.ReadUInt16();
					networkHS[1].PackedValue = packetReader.ReadUInt16();
					networkHS[2].PackedValue = packetReader.ReadUInt16();
					mainC.pickupsMain.Player_Drops_Weapon(networkUShorts[0], networkHS[0].ToSingle(), networkHS[1].ToSingle(), networkHS[2].ToSingle(), sendToNetwork: false);
					break;
				case 78:
					mainC.maingameMain.Receive_AI_Respawn_Authorization_From_Host();
					break;
				case 79:
				{
					ushort num = packetReader.ReadUInt16();
					int i = 0;
					ushort num2 = 0;
					for (; i < num; i++)
					{
						networkBytes[i] = packetReader.ReadByte();
						networkHS[num2++].PackedValue = packetReader.ReadUInt16();
						networkHS[num2++].PackedValue = packetReader.ReadUInt16();
						networkHS[num2++].PackedValue = packetReader.ReadUInt16();
					}
					mainC.pickupsMain.Pickup_Changed_Position((byte)num);
					break;
				}
				case 80:
					mainC.aiMain.Receive_AI_Completed_Message();
					break;
				case 81:
					if (isHost)
					{
						XBOX_MP_Round_Over();
					}
					break;
				case 82:
					mainC.playersMain.Send_Remote_Player_Array_Position_To_New_Gamer(sender);
					break;
				}
			}
		}
		catch (Exception)
		{
		}
	}

	public void XBOX_Game_Update_NetworkSession()
	{
		int count = networkSession.SessionProperties.Count;
		for (int i = 0; i < count; i++)
		{
			switch (i)
			{
			case 0:
				networkSession.SessionProperties[i] = 1;
				break;
			case 1:
				networkSession.SessionProperties[i] = global::MainGame.MainGame.mp_numPlayers_index;
				break;
			case 2:
				networkSession.SessionProperties[i] = global::MainGame.MainGame.mp_timeLimit_index;
				break;
			case 3:
				networkSession.SessionProperties[i] = global::MainGame.MainGame.gameLevel;
				break;
			case 4:
				networkSession.SessionProperties[i] = global::MainGame.MainGame.gameType;
				break;
			case 6:
				networkSession.SessionProperties[i] = global::MainGame.MainGame.difficulty;
				break;
			default:
				networkSession.SessionProperties[i] = 0;
				break;
			}
		}
	}

	public void XBOX_Create_Session(NetworkSessionType sType, byte privateGamerSlots)
	{
		NetworkSessionProperties networkSessionProperties = new NetworkSessionProperties();
		XBOX_Start_Multiplayer(creatingGame: true, sType);
		if (!loggedIn)
		{
			return;
		}
		try
		{
			XBOX_Close_Session();
			for (byte b = 0; b < networkSessionProperties.Count; b++)
			{
				switch (b)
				{
				case 0:
					networkSessionProperties[b] = 1;
					break;
				case 1:
					networkSessionProperties[b] = global::MainGame.MainGame.mp_numPlayers_index;
					break;
				case 2:
					networkSessionProperties[b] = global::MainGame.MainGame.mp_timeLimit_index;
					break;
				case 3:
					networkSessionProperties[b] = global::MainGame.MainGame.gameLevel;
					break;
				case 4:
					networkSessionProperties[b] = global::MainGame.MainGame.gameType;
					break;
				case 6:
					networkSessionProperties[b] = global::MainGame.MainGame.difficulty;
					break;
				case 7:
					networkSessionProperties[b] = global::MainGame.MainGame.numTeams;
					break;
				default:
					networkSessionProperties[b] = 0;
					break;
				}
			}
			SignedInGamer[] localGamers = new SignedInGamer[1] { Gamer.SignedInGamers[global::MainGame.MainGame.signedinGamerID] };
			networkSession = NetworkSession.Create(sType, localGamers, global::MainGame.MainGame.mp_numRemotePlayers[global::MainGame.MainGame.mp_numPlayers_index], privateGamerSlots, networkSessionProperties);
			byte b2 = (byte)networkSession.LocalGamers.Count;
			global::MainGame.MainGame.localNetworkGamerID = 4;
			switch (global::InputHandler.InputHandler.gpadID)
			{
			case 0:
			{
				for (byte b = 0; b < b2; b++)
				{
					if (networkSession.LocalGamers[b].SignedInGamer.PlayerIndex == PlayerIndex.One)
					{
						global::MainGame.MainGame.localNetworkGamerID = b;
						break;
					}
				}
				break;
			}
			case 1:
			{
				for (byte b = 0; b < b2; b++)
				{
					if (networkSession.LocalGamers[b].SignedInGamer.PlayerIndex == PlayerIndex.Two)
					{
						global::MainGame.MainGame.localNetworkGamerID = b;
						break;
					}
				}
				break;
			}
			case 2:
			{
				for (byte b = 0; b < b2; b++)
				{
					if (networkSession.LocalGamers[b].SignedInGamer.PlayerIndex == PlayerIndex.Three)
					{
						global::MainGame.MainGame.localNetworkGamerID = b;
						break;
					}
				}
				break;
			}
			case 3:
			{
				for (byte b = 0; b < b2; b++)
				{
					if (networkSession.LocalGamers[b].SignedInGamer.PlayerIndex == PlayerIndex.Four)
					{
						global::MainGame.MainGame.localNetworkGamerID = b;
						break;
					}
				}
				break;
			}
			}
			global::Players.Players.remotePlayerPositions[0] = networkSession.LocalGamers[global::MainGame.MainGame.localNetworkGamerID].Id;
			networkSession.SessionProperties[5] = (networkSession.LocalGamers[global::MainGame.MainGame.localNetworkGamerID].HasVoice ? 1 : 0);
			networkSession.AllowJoinInProgress = true;
			networkSession.AllowHostMigration = true;
			networkSessionReady = true;
			isHost = true;
			networkSession.GamerJoined += XBOX_New_Gamer;
			networkSession.GamerLeft += XBOX_Gamer_Left;
			networkSession.SessionEnded += XBOX_Session_Ended;
			networkSession.GameStarted += XBOX_Game_Started;
			networkSession.GameEnded += XBOX_MP_Game_Ended;
			multiplayerNewGameStatus = 1;
		}
		catch (Exception)
		{
			XBOX_Close_Session();
			mainC.inputMain.Multiplayer_Failed(5);
		}
	}

	public void XBOX_Join_First_Session(NetworkSessionType sType)
	{
		NetworkSessionProperties networkSessionProperties = new NetworkSessionProperties();
		networkSessionProperties[0] = 1;
		networkSessionProperties[4] = global::MainGame.MainGame.gameType;
		XBOX_Start_Multiplayer(creatingGame: false, sType);
		if (!loggedIn)
		{
			return;
		}
		try
		{
			XBOX_Close_Session();
			SignedInGamer[] localGamers = new SignedInGamer[1] { Gamer.SignedInGamers[global::MainGame.MainGame.signedinGamerID] };
			AvailableNetworkSessionCollection availableNetworkSessionCollection = NetworkSession.Find(sType, localGamers, networkSessionProperties);
			if (availableNetworkSessionCollection.Count == 0)
			{
				return;
			}
			int num;
			try
			{
				int i = 0;
				num = 0;
				int index = 0;
				for (; i < availableNetworkSessionCollection.Count; i++)
				{
					if (availableNetworkSessionCollection[i].CurrentGamerCount > num)
					{
						num = availableNetworkSessionCollection[i].CurrentGamerCount;
						index = i;
					}
				}
				networkSession = NetworkSession.Join(availableNetworkSessionCollection[index]);
			}
			catch
			{
				networkSession = NetworkSession.Join(availableNetworkSessionCollection[0]);
			}
			mainC.gameLogic.Game_Update_GameSettings_From_Main_NetworkSession();
			networkSessionReady = true;
			isHost = false;
			num = (byte)networkSession.LocalGamers.Count;
			global::MainGame.MainGame.localNetworkGamerID = 4;
			switch (global::InputHandler.InputHandler.gpadID)
			{
			case 0:
			{
				for (int i = 0; i < num; i++)
				{
					if (networkSession.LocalGamers[i].SignedInGamer.PlayerIndex == PlayerIndex.One)
					{
						global::MainGame.MainGame.localNetworkGamerID = (byte)i;
						break;
					}
				}
				break;
			}
			case 1:
			{
				for (int i = 0; i < num; i++)
				{
					if (networkSession.LocalGamers[i].SignedInGamer.PlayerIndex == PlayerIndex.Two)
					{
						global::MainGame.MainGame.localNetworkGamerID = (byte)i;
						break;
					}
				}
				break;
			}
			case 2:
			{
				for (int i = 0; i < num; i++)
				{
					if (networkSession.LocalGamers[i].SignedInGamer.PlayerIndex == PlayerIndex.Three)
					{
						global::MainGame.MainGame.localNetworkGamerID = (byte)i;
						break;
					}
				}
				break;
			}
			case 3:
			{
				for (int i = 0; i < num; i++)
				{
					if (networkSession.LocalGamers[i].SignedInGamer.PlayerIndex == PlayerIndex.Four)
					{
						global::MainGame.MainGame.localNetworkGamerID = (byte)i;
						break;
					}
				}
				break;
			}
			}
			networkSession.GamerJoined += XBOX_New_Gamer;
			networkSession.GamerLeft += XBOX_Gamer_Left;
			networkSession.SessionEnded += XBOX_Session_Ended;
			networkSession.GameStarted += XBOX_Game_Started;
			networkSession.GameEnded += XBOX_MP_Game_Ended;
			multiplayerNewGameStatus = 1;
		}
		catch (Exception)
		{
			XBOX_Close_Session();
			mainC.inputMain.Multiplayer_Failed(4);
		}
	}

	public void XBOX_Join_Session(short sessionID, NetworkSessionType sType)
	{
		inGame = false;
		if (searchSessions == null || sessionID < 0 || sessionID > searchSessions.Count)
		{
			mainC.inputMain.Multiplayer_Failed(3);
			return;
		}
		if (!loggedIn)
		{
			mainC.inputMain.Multiplayer_Failed(1);
			return;
		}
		try
		{
			XBOX_Close_Session();
			networkSession = NetworkSession.Join(searchSessions[sessionID]);
			mainC.gameLogic.Game_Update_GameSettings_From_Main_NetworkSession();
			networkSessionReady = true;
			isHost = false;
			byte b = (byte)networkSession.LocalGamers.Count;
			global::MainGame.MainGame.localNetworkGamerID = 4;
			switch (global::InputHandler.InputHandler.gpadID)
			{
			case 0:
			{
				for (byte b2 = 0; b2 < b; b2++)
				{
					if (networkSession.LocalGamers[b2].SignedInGamer.PlayerIndex == PlayerIndex.One)
					{
						global::MainGame.MainGame.localNetworkGamerID = b2;
						break;
					}
				}
				break;
			}
			case 1:
			{
				for (byte b2 = 0; b2 < b; b2++)
				{
					if (networkSession.LocalGamers[b2].SignedInGamer.PlayerIndex == PlayerIndex.Two)
					{
						global::MainGame.MainGame.localNetworkGamerID = b2;
						break;
					}
				}
				break;
			}
			case 2:
			{
				for (byte b2 = 0; b2 < b; b2++)
				{
					if (networkSession.LocalGamers[b2].SignedInGamer.PlayerIndex == PlayerIndex.Three)
					{
						global::MainGame.MainGame.localNetworkGamerID = b2;
						break;
					}
				}
				break;
			}
			case 3:
			{
				for (byte b2 = 0; b2 < b; b2++)
				{
					if (networkSession.LocalGamers[b2].SignedInGamer.PlayerIndex == PlayerIndex.Four)
					{
						global::MainGame.MainGame.localNetworkGamerID = b2;
						break;
					}
				}
				break;
			}
			}
			networkSession.GamerJoined += XBOX_New_Gamer;
			networkSession.GamerLeft += XBOX_Gamer_Left;
			networkSession.SessionEnded += XBOX_Session_Ended;
			networkSession.GameStarted += XBOX_Game_Started;
			networkSession.GameEnded += XBOX_MP_Game_Ended;
			multiplayerNewGameStatus = 1;
		}
		catch (Exception)
		{
			XBOX_Close_Session();
		}
	}

	public void XBOX_Join_Game_Invite()
	{
		inGame = false;
		if (!loggedIn)
		{
			mainC.inputMain.Multiplayer_Failed(1);
			return;
		}
		try
		{
			XBOX_Close_Session();
			networkSession = NetworkSession.JoinInvited(1);
			mainC.gameLogic.Game_Update_GameSettings_From_Main_NetworkSession();
			networkSessionReady = true;
			isHost = false;
			byte b = (byte)networkSession.LocalGamers.Count;
			global::MainGame.MainGame.localNetworkGamerID = 4;
			switch (global::InputHandler.InputHandler.gpadID)
			{
			case 0:
			{
				for (byte b2 = 0; b2 < b; b2++)
				{
					if (networkSession.LocalGamers[b2].SignedInGamer.PlayerIndex == PlayerIndex.One)
					{
						global::MainGame.MainGame.localNetworkGamerID = b2;
						break;
					}
				}
				break;
			}
			case 1:
			{
				for (byte b2 = 0; b2 < b; b2++)
				{
					if (networkSession.LocalGamers[b2].SignedInGamer.PlayerIndex == PlayerIndex.Two)
					{
						global::MainGame.MainGame.localNetworkGamerID = b2;
						break;
					}
				}
				break;
			}
			case 2:
			{
				for (byte b2 = 0; b2 < b; b2++)
				{
					if (networkSession.LocalGamers[b2].SignedInGamer.PlayerIndex == PlayerIndex.Three)
					{
						global::MainGame.MainGame.localNetworkGamerID = b2;
						break;
					}
				}
				break;
			}
			case 3:
			{
				for (byte b2 = 0; b2 < b; b2++)
				{
					if (networkSession.LocalGamers[b2].SignedInGamer.PlayerIndex == PlayerIndex.Four)
					{
						global::MainGame.MainGame.localNetworkGamerID = b2;
						break;
					}
				}
				break;
			}
			}
			networkSession.GamerJoined += XBOX_New_Gamer;
			networkSession.GamerLeft += XBOX_Gamer_Left;
			networkSession.SessionEnded += XBOX_Session_Ended;
			networkSession.GameStarted += XBOX_Game_Started;
			networkSession.GameEnded += XBOX_MP_Game_Ended;
			multiplayerNewGameStatus = 1;
		}
		catch (Exception)
		{
			XBOX_Close_Session();
		}
	}

	public void XBOX_Game_Started(object sender, GameStartedEventArgs e)
	{
		if (isHost)
		{
			XBOX_aSend_Network_Message(26);
			global::MainGame.MainGame.gameState = 133;
		}
	}

	public void XBOX_Gamer_Left(object sender, GamerLeftEventArgs e)
	{
		if (!e.Gamer.IsLocal)
		{
			networkMsg = e.Gamer.Gamertag + networkMsg2;
			mainC.fontmoduleMain.Add_Text_To_Onscreen_Text(networkMsg);
			mainC.playersMain.Remove_Remote_Player_From_Game(mainC.playersMain.Get_Player_Index(e.Gamer.Id, -1));
			if (isHost)
			{
				mainC.aiMain.Update_AI_Controlling_Players();
				XBOX_Update_Session_Number_Of_Voice_Players();
			}
		}
		else
		{
			inGame = false;
			XBOX_Close_Session();
			mainC.inputMain.Multiplayer_Session_Over();
		}
	}

	public void XBOX_New_Gamer(object sender, GamerJoinedEventArgs e)
	{
		inGame = true;
		if (e.Gamer.IsLocal)
		{
			global::Players.Players.players[0].id = e.Gamer.Id;
			networkMsg = networkSession.LocalGamers[global::MainGame.MainGame.localNetworkGamerID].Gamertag;
			mainC.playersMain.XBOX_Update_Local_Player_GamerTag(networkSession.LocalGamers[global::MainGame.MainGame.localNetworkGamerID].Id, networkMsg);
			if (global::MainGame.MainGame.localNetworkGamerID < networkSession.LocalGamers.Count)
			{
				networkSession.LocalGamers[global::MainGame.MainGame.localNetworkGamerID].BeginGetProfile(Get_Gamer_Profile, e.Gamer.Id);
			}
			return;
		}
		networkSession.LocalGamers[global::MainGame.MainGame.localNetworkGamerID].EnableSendVoice(e.Gamer, enable: false);
		networkMsg = e.Gamer.Gamertag + networkMsg1;
		mainC.fontmoduleMain.Add_Text_To_Onscreen_Text(networkMsg);
		int num;
		if (mainC.playersMain.Get_Player_Index(e.Gamer.Id, -1) < 0 && (num = mainC.playersMain.Find_Vacant_Player(0)) > -1)
		{
			networkMsg = e.Gamer.Gamertag;
			if (num < global::MainGame.MainGame.maxHumanGamePlayers)
			{
				mainC.playersMain.Init_New_Multiplayer_Gamer(networkMsg, num, e.Gamer.Id);
				int index = mainC.playersMain.Get_RemoteGamer_Index(e.Gamer.Id, -1);
				networkSession.RemoteGamers[index].BeginGetProfile(Get_Gamer_Profile, networkSession.RemoteGamers[index].Id);
			}
			else
			{
				mainC.playersMain.Sync_Network_Session_Players();
			}
		}
		if (isHost)
		{
			mainC.playersMain.Host_Assigns_Team_To_New_Remote_Player(e.Gamer);
			XBOX_Update_Session_Number_Of_Voice_Players();
		}
	}

	public void XBOX_Session_Ended(object sender, NetworkSessionEndedEventArgs e)
	{
		inGame = false;
		XBOX_Close_Session();
		if (global::MainGame.MainGame.gameMode == 1)
		{
			mainC.inputMain.Multiplayer_Session_Over();
		}
	}

	public void XBOX_MP_Game_Ended(object sender, GameEndedEventArgs e)
	{
		mainC.maingameMain.MP_Game_Ended();
		mainC.playersMain.Send_Player_Points(0);
		mainC.gameLogic.Game_Send_Final_Player_Stats();
		if (isHost)
		{
			mainC.playersMain.Send_Team_Points();
		}
	}

	public void XBOX_Game_Invite_Accepted(object sender, InviteAcceptedEventArgs e)
	{
		global::MainGame.MainGame.difficulty = 0;
		global::Rendering.Rendering.renderMenuScreen = 1;
		mainC.maingameMain.Multiplayer_Start_Join_Game_Invite_Process();
	}

	public void XBOX_Game_Invite_Accepted()
	{
		global::MainGame.MainGame.difficulty = 0;
		global::Rendering.Rendering.renderMenuScreen = 1;
		mainC.maingameMain.Multiplayer_Start_Join_Game_Invite_Process();
	}

	public void Get_Gamer_Profile(IAsyncResult result)
	{
		try
		{
			short num;
			if ((num = mainC.playersMain.Get_Player_Index((byte)result.AsyncState, -1)) == -1)
			{
				return;
			}
			for (short num2 = 0; num2 < networkSession.RemoteGamers.Count; num2++)
			{
				if (networkSession.RemoteGamers[num2].Id == global::Players.Players.players[num].id)
				{
					networkPlayers[num].profile = networkSession.RemoteGamers[num2].EndGetProfile(result);
					networkPlayers[num].gamerPicture = Texture2D.FromStream(global::Rendering.Rendering.rGraphics, networkPlayers[num].profile.GetGamerPicture());
					return;
				}
			}
			for (short num2 = 0; num2 < networkSession.LocalGamers.Count; num2++)
			{
				if (networkSession.LocalGamers[num2].Id == global::Players.Players.players[num].id)
				{
					networkPlayers[num].profile = networkSession.LocalGamers[num2].EndGetProfile(result);
					networkPlayers[num].gamerPicture = Texture2D.FromStream(global::Rendering.Rendering.rGraphics, networkPlayers[num].profile.GetGamerPicture());
					break;
				}
			}
		}
		catch (Exception)
		{
		}
	}

	public void Send_Player_Array_Position(int actID, short arrayPosition)
	{
		networkInts[0] = actID;
		networkShorts[0] = arrayPosition;
		XBOX_Send_Network_Message39(39);
	}

	public bool All_Players_Ready()
	{
		for (byte b = 0; b < networkSession.LocalGamers.Count; b++)
		{
			if (!networkSession.LocalGamers[b].IsReady)
			{
				return false;
			}
		}
		for (byte b = 0; b < networkSession.RemoteGamers.Count; b++)
		{
			if (!networkSession.RemoteGamers[b].IsReady)
			{
				return false;
			}
		}
		return true;
	}

	public float Percentage_Of_Ready_Players()
	{
		float num = 0f;
		for (byte b = 0; b < networkSession.LocalGamers.Count; b++)
		{
			if (networkSession.LocalGamers[b].IsReady)
			{
				num += 1f;
			}
		}
		for (byte b = 0; b < networkSession.RemoteGamers.Count; b++)
		{
			if (networkSession.RemoteGamers[b].IsReady)
			{
				num += 1f;
			}
		}
		return num / (float)(networkSession.LocalGamers.Count + networkSession.RemoteGamers.Count);
	}

	public void Player_Loaded(NetworkGamer newGamer)
	{
		int num = mainC.playersMain.Get_Player_Index(newGamer.Id, -1);
		if (num > -1)
		{
			networkPlayers[num].playerLoaded = true;
		}
		if (networkSession.IsHost)
		{
			mainC.aiMain.Update_AI_Controlling_Players();
		}
		mainC.gameLogic.Game_New_Player_Ready(newGamer);
		if (networkSession.IsHost)
		{
			mainC.playersMain.Send_Remote_Player_Array_Position_To_New_Gamer(newGamer);
			mainC.playersMain.XBOX_Send_Update_Of_Team_Points_For_NewPlayer(newGamer);
			mainC.gameLogic.Game_Send_GameSettings_To_NewPlayer(15, newGamer);
			global::MainGame.MainGame.Send_ProgramData_RoundTimer_To_Player(newGamer);
			mainC.targetMain.Send_DamageTargets_To_New_Player(newGamer);
			mainC.gameobjectMain.Send_GameObjects_To_New_Player(newGamer);
			mainC.pickupsMain.Send_Pickup_To_New_Players(newGamer);
			mainC.aiMain.Send_All_AI_To_New_Player(newGamer);
			if (num > -1)
			{
				networkBytes[0] = (byte)global::Players.Players.players[num].team;
				mainC.networkingMain.XBOX_Send_Network_Message_To_Gamer(11, newGamer);
			}
			byte gameType = global::MainGame.MainGame.gameType;
			if (gameType == 4)
			{
				mainC.maingameMain.Send_New_Race_Participant_Status(newGamer);
				mainC.maingameMain.Send_Race_Participants_Status_To_NewGamer(newGamer);
			}
		}
		if (num > -1)
		{
			mainC.playersMain.Reset_Player((ushort)num, isActive: true, global::Players.Players.players[num].race, (byte)global::Players.Players.players[num].type);
		}
		mainC.playersMain.Send_Player_Info_To_Gamer(newGamer);
	}

	public void Kick_Player(byte remoteGamer)
	{
		networkSession.RemoteGamers[remoteGamer].Machine.RemoveFromSession();
	}

	public void Mark_Team_Points_To_Be_Sent(float timeBeforeSend)
	{
		if (!needToSendTeamScores)
		{
			sendTeamScoresTimer = timeBeforeSend;
		}
		needToSendTeamScores = true;
	}

	public void Setup_For_SP()
	{
		XBOX_Close_Session();
	}

	public void XBOX_Update_Session_Number_Of_Voice_Players()
	{
		ushort num = 0;
		ushort num2 = (ushort)networkSession.AllGamers.Count;
		for (ushort num3 = 0; num3 < num2; num3++)
		{
			num += (ushort)(networkSession.AllGamers[num3].HasVoice ? 1 : 0);
		}
		networkSession.SessionProperties[5] = num;
	}

	public void XBOX_Start_Multiplayer(bool creatingGame, NetworkSessionType sType)
	{
		bool flag = false;
		onlineSessionType = sType;
		loggedIn = XBOX_Profile_Valid(creatingGame);
		if (loggedIn)
		{
			XBOX_Reset_Guide_Status();
			return;
		}
		if (sType == NetworkSessionType.PlayerMatch || sType == NetworkSessionType.Ranked)
		{
			flag = true;
		}
		if (Guide.IsVisible)
		{
			return;
		}
		if (global::MainGame.MainGame.trialMode && flag && global::MainGame.MainGame.signedinGamerID > -1 && global::MainGame.MainGame.signedinGamerID < Gamer.SignedInGamers.Count && Gamer.SignedInGamers[global::MainGame.MainGame.signedinGamerID] != null && Gamer.SignedInGamers[global::MainGame.MainGame.signedinGamerID].IsSignedInToLive && !shownMarketplace)
		{
			switch (global::InputHandler.InputHandler.gpadID)
			{
			case 0:
				Guide.ShowMarketplace(PlayerIndex.One);
				break;
			case 1:
				Guide.ShowMarketplace(PlayerIndex.Two);
				break;
			case 2:
				Guide.ShowMarketplace(PlayerIndex.Three);
				break;
			case 3:
				Guide.ShowMarketplace(PlayerIndex.Four);
				break;
			}
			shownMarketplace = true;
		}
		else if (!shownGuide && !shownMarketplace)
		{
			Guide.ShowSignIn(1, onlineOnly: false);
			shownGuide = true;
		}
		else
		{
			mainC.inputMain.Multiplayer_Cancelled();
			XBOX_Reset_Guide_Status();
		}
	}

	public bool XBOX_Profile_Valid(bool creatingGame)
	{
		if (global::MainGame.MainGame.signedinGamerID > -1 && global::MainGame.MainGame.signedinGamerID < Gamer.SignedInGamers.Count && Gamer.SignedInGamers[global::MainGame.MainGame.signedinGamerID] != null)
		{
			if (onlineSessionType == NetworkSessionType.PlayerMatch || onlineSessionType == NetworkSessionType.Ranked)
			{
				if (!Gamer.SignedInGamers[global::MainGame.MainGame.signedinGamerID].IsSignedInToLive || !Gamer.SignedInGamers[global::MainGame.MainGame.signedinGamerID].Privileges.AllowOnlineSessions || Gamer.SignedInGamers[global::MainGame.MainGame.signedinGamerID].IsGuest)
				{
					return false;
				}
			}
			else if (creatingGame && Gamer.SignedInGamers[global::MainGame.MainGame.signedinGamerID].IsGuest)
			{
				return false;
			}
			return true;
		}
		return false;
	}

	public bool XBOX_SignedIn_And_CanBuy()
	{
		if (global::MainGame.MainGame.signedinGamerID > -1 && global::MainGame.MainGame.signedinGamerID < Gamer.SignedInGamers.Count && Gamer.SignedInGamers[global::MainGame.MainGame.signedinGamerID] != null && Gamer.SignedInGamers[global::MainGame.MainGame.signedinGamerID].IsSignedInToLive && Gamer.SignedInGamers[global::MainGame.MainGame.signedinGamerID].Privileges.AllowPurchaseContent)
		{
			return true;
		}
		return false;
	}

	public bool XBOX_Account_Is_Not_A_Guest_Account()
	{
		if (global::MainGame.MainGame.signedinGamerID > -1 && global::MainGame.MainGame.signedinGamerID < Gamer.SignedInGamers.Count && Gamer.SignedInGamers[global::MainGame.MainGame.signedinGamerID] != null && !Gamer.SignedInGamers[global::MainGame.MainGame.signedinGamerID].IsGuest)
		{
			return true;
		}
		return false;
	}

	public byte XBOX_SignIn_To_Buy()
	{
		try
		{
			if (XBOX_SignedIn_And_CanBuy())
			{
				return 1;
			}
			if (!Guide.IsVisible)
			{
				if (shownGuide)
				{
					shownGuide = false;
					return 2;
				}
				Guide.ShowSignIn(1, onlineOnly: false);
				shownGuide = true;
			}
		}
		catch (Exception)
		{
		}
		return 0;
	}

	public byte XBOX_Signed_In(bool creatingGame)
	{
		try
		{
			if (XBOX_Profile_Valid(creatingGame))
			{
				return 1;
			}
			if (!Guide.IsVisible)
			{
				if (shownGuide)
				{
					shownGuide = false;
					return 2;
				}
				Guide.ShowSignIn(1, onlineOnly: false);
				shownGuide = true;
			}
		}
		catch (Exception)
		{
		}
		return 0;
	}

	public void XBOX_Reset_Guide_Status()
	{
		shownGuide = false;
		shownMarketplace = false;
	}

	public byte XBOX_Purchase_Game()
	{
		if (!XBOX_SignedIn_And_CanBuy())
		{
			return 1;
		}
		try
		{
			if (!global::MainGame.MainGame.trialMode)
			{
				shownGuide = false;
				shownMarketplace = false;
				return 4;
			}
			if (!Guide.IsVisible)
			{
				shownGuide = false;
				if (!shownMarketplace)
				{
					switch (global::InputHandler.InputHandler.gpadID)
					{
					case 0:
						Guide.ShowMarketplace(PlayerIndex.One);
						break;
					case 1:
						Guide.ShowMarketplace(PlayerIndex.Two);
						break;
					case 2:
						Guide.ShowMarketplace(PlayerIndex.Three);
						break;
					case 3:
						Guide.ShowMarketplace(PlayerIndex.Four);
						break;
					}
					shownMarketplace = true;
					return 2;
				}
				shownMarketplace = false;
				return 3;
			}
			return 2;
		}
		catch
		{
			return 0;
		}
	}

	public NetworkGamer XBOX_Get_Gamer_By_Act_ID(int id)
	{
		int count = networkSession.RemoteGamers.Count;
		for (short num = 0; num < count; num++)
		{
			if (networkSession.RemoteGamers[num].Id == id)
			{
				return networkSession.RemoteGamers[num];
			}
		}
		return networkSession.LocalGamers[global::MainGame.MainGame.localNetworkGamerID];
	}

	public NetworkGamer XBOX_Get_Gamer_By_Player_Index(byte id)
	{
		int count = networkSession.RemoteGamers.Count;
		for (short num = 0; num < count; num++)
		{
			if (networkSession.RemoteGamers[num].Id == global::Players.Players.players[id].id)
			{
				return networkSession.RemoteGamers[num];
			}
		}
		return networkSession.LocalGamers[global::MainGame.MainGame.localNetworkGamerID];
	}

	public void XBOX_Close_Session()
	{
		if (networkSession != null && !networkSession.IsDisposed)
		{
			networkSession.Dispose();
		}
		global::MainGame.MainGame.localNetworkGamerID = 4;
		inGame = false;
		networkSessionReady = false;
		isHost = false;
		networkState = 0;
	}

	public void XBOX_MP_Round_Over()
	{
		if (!global::MainGame.MainGame.roundOver && isHost && networkSession.SessionState == NetworkSessionState.Playing)
		{
			networkSession.EndGame();
		}
	}

	public void XBOX_Start_Game()
	{
		networkSession.StartGame();
	}

	public int XBOX_Get_RemoteGamer_Index(short id, int defaultVal)
	{
		for (int i = 0; i < networkSession.RemoteGamers.Count; i++)
		{
			if (networkSession.RemoteGamers[i].Id == id)
			{
				return i;
			}
		}
		return defaultVal;
	}

	public void XBOX_Enable_Voice_All()
	{
		int count = networkSession.RemoteGamers.Count;
		for (short num = 0; num < count; num++)
		{
			networkSession.LocalGamers[global::MainGame.MainGame.localNetworkGamerID].EnableSendVoice(networkSession.RemoteGamers[num], enable: true);
		}
	}

	public void XBOX_Get_Game_List(NetworkSessionType sType)
	{
		NetworkSessionProperties networkSessionProperties = new NetworkSessionProperties();
		onlineSessionType = sType;
		gameListSearchFinished = false;
		numMPGames = 0;
		networkSessionProperties[0] = 1;
		networkSessionProperties[4] = global::MainGame.MainGame.gameType;
		inGame = false;
		XBOX_Start_Multiplayer(creatingGame: false, sType);
		if (!loggedIn)
		{
			return;
		}
		try
		{
			SignedInGamer[] localGamers = new SignedInGamer[1] { Gamer.SignedInGamers[global::MainGame.MainGame.signedinGamerID] };
			searchSessions = NetworkSession.Find(sType, localGamers, networkSessionProperties);
			numMPGames = searchSessions.Count;
			gameListSearchFinished = true;
			if (searchSessions.Count == 0)
			{
				mainC.inputMain.Multiplayer_Failed(2);
			}
			else
			{
				mainC.inputMain.Multiplayer_Join_List_Ready();
			}
		}
		catch (Exception)
		{
			numMPGames = 0;
			gameListSearchFinished = true;
			mainC.inputMain.Multiplayer_Failed(3);
		}
	}

	public void XBOX_Reset_Ready_Flags()
	{
		try
		{
			if (isHost && networkSessionReady && (networkSession.SessionState == NetworkSessionState.Ended || networkSession.SessionState == NetworkSessionState.Lobby))
			{
				networkSession.ResetReady();
			}
		}
		catch (Exception)
		{
		}
	}
}
