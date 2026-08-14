using System.Threading;
using MainGame;
using WindowsGame1;

namespace Threads;

public class Threads
{
	public static volatile byte thread0Task;

	public static volatile byte thread1Task;

	public static volatile byte thread2Task;

	public static volatile byte thread3Task;

	public static volatile bool thread0Active = false;

	public static volatile bool thread1Active = false;

	public static volatile bool thread2Active = false;

	public static volatile bool thread3Active = false;

	public static volatile short dataS0;

	public static volatile short dataS1;

	public static volatile short dataS2;

	public static volatile short dataS3;

	public static volatile int dataI0;

	public static volatile int dataI1;

	public static volatile int dataI2;

	public static volatile int dataI3;

	public static Thread Thread0;

	public static Thread Thread1;

	public static Thread Thread2;

	public static Thread Thread3;

	public static EventWaitHandle thread0Start = new AutoResetEvent(initialState: false);

	public static EventWaitHandle thread0End = new AutoResetEvent(initialState: false);

	public static EventWaitHandle thread1Start = new AutoResetEvent(initialState: false);

	public static EventWaitHandle thread1End = new AutoResetEvent(initialState: false);

	public static EventWaitHandle thread2Start = new AutoResetEvent(initialState: false);

	public static EventWaitHandle thread2End = new AutoResetEvent(initialState: false);

	public static EventWaitHandle thread3Start = new AutoResetEvent(initialState: false);

	public static EventWaitHandle thread3End = new AutoResetEvent(initialState: false);

	public static EventWaitHandle playerAvatarUpdateEnd = new AutoResetEvent(initialState: false);

	public Game1.MasterCollection mainC;

	public void Init_Vars(Game1.MasterCollection master)
	{
		mainC = master;
	}

	public void Init_Threading()
	{
		Thread1 = new Thread(Thread1_Main);
		Thread1.Name = "1";
		Thread2 = new Thread(Thread2_Main);
		Thread2.Name = "2";
		Thread3 = new Thread(Thread3_Main);
		Thread3.Name = "3";
		thread1Start.Reset();
		thread2Start.Reset();
		thread3Start.Reset();
		Thread1.Start();
		Thread2.Start();
		Thread3.Start();
	}

	public void Thread0_Main()
	{
		// Xbox 360 hardware-thread affinity hint omitted on desktop.
		while (true)
		{
			thread0Start.WaitOne();
			thread0Active = true;
			byte b = thread0Task;
			if (b == byte.MaxValue)
			{
				break;
			}
			thread0Active = false;
			thread0End.Set();
		}
		thread0Active = false;
		thread0End.Set();
	}

	public void Thread1_Main()
	{
		// Xbox 360 hardware-thread affinity hint omitted on desktop.
		while (true)
		{
			thread1Start.WaitOne();
			thread1Active = true;
			switch (thread1Task)
			{
			case 0:
				global::MainGame.MainGame.Main_Loop_Threaded_MP_Gameplay(1);
				break;
			case 1:
				global::MainGame.MainGame.Main_Loop_Threaded_MP_Gameplay_Paused_For_Menus(1);
				break;
			case 2:
				global::MainGame.MainGame.Main_Loop_Threaded_MP_Paused_For_Menus_Not_Playing(1);
				break;
			case 3:
				global::MainGame.MainGame.Main_Loop_Threaded_MP_Commander_Gameplay(1);
				break;
			case 4:
				global::MainGame.MainGame.Main_Loop_Threaded_MP_Commander_Gameplay_Paused_For_Menus(1);
				break;
			case 6:
				global::MainGame.MainGame.Main_Loop_Threaded_SP_Gameplay(1);
				break;
			case byte.MaxValue:
				thread1Active = false;
				thread1End.Set();
				return;
			}
			thread1Active = false;
			thread1End.Set();
		}
	}

	public void Thread2_Main()
	{
		// Xbox 360 hardware-thread affinity hint omitted on desktop.
		while (true)
		{
			thread2Start.WaitOne();
			thread2Active = true;
			switch (thread2Task)
			{
			case 1:
				mainC.playersMain.New_SinglePlayer_Round(minorRestart: false, 2);
				break;
			case 2:
				mainC.aiMain.Target_AI(2);
				mainC.aiMain.Find_Route();
				break;
			case byte.MaxValue:
				thread2Active = false;
				thread2End.Set();
				return;
			}
			thread2Active = false;
			thread2End.Set();
		}
	}

	public void Thread3_Main()
	{
		// Xbox 360 hardware-thread affinity hint omitted on desktop.
		while (true)
		{
			thread3Start.WaitOne();
			thread3Active = true;
			switch (thread3Task)
			{
			case 2:
				thread2Task = 2;
				thread2Start.Set();
				mainC.avatarMain.Process_Avatars();
				thread2End.WaitOne();
				break;
			case 5:
				mainC.maingameMain.SP_Initial_Setup(3);
				break;
			case 6:
				mainC.maingameMain.Multiplayer_Create_Game_Session(3);
				break;
			case 7:
				mainC.maingameMain.Multiplayer_Join_First_Game_Session(3);
				break;
			case 8:
				mainC.maingameMain.Multiplayer_Start_New_Game(3);
				break;
			case 9:
				mainC.maingameMain.Multiplayer_Join_Game_Session(3);
				break;
			case 10:
				mainC.maingameMain.Multiplayer_Join_Game_Invite(3);
				break;
			case byte.MaxValue:
				thread3Active = false;
				thread3End.Set();
				return;
			}
			thread3Active = false;
			thread3End.Set();
		}
	}

	public void Close()
	{
		if (Thread0 != null)
		{
			Thread0.Abort();
		}
		if (Thread1 != null)
		{
			Thread1.Abort();
		}
		if (Thread2 != null)
		{
			Thread2.Abort();
		}
		if (Thread3 != null)
		{
			Thread3.Abort();
		}
	}
}
