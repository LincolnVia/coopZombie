using System;
using MainGame;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Rendering;
using Structs;
using WindowsGame1;

namespace Sounds;

public class Sounds
{
	public static bool soundSystemLoaded = false;

	public static bool randomLevelTracks = true;

	public static bool[] soundEnabled;

	public static bool[] musicIsPlaying;

	public static bool[] backgroundSoundsStatus;

	public static byte numLevelMusic;

	public static byte musicLoadingID;

	public static byte musicMenuID;

	public static byte numMusicCues;

	public static byte curLevelMusicID;

	public static byte curMusicID;

	public static byte activeLevelMusicID;

	public static byte levelStartingMusic;

	public static byte[] levelMusic;

	public static byte[] playerContinualSounds;

	public static byte curCue;

	public static byte curPriorityCue;

	public static byte curVoice1Cue;

	public static ushort numContinualSounds;

	public static ushort enabledBackgroundSounds;

	public static float fixVolume;

	public static float[] backgroundSoundTimes;

	public static float[] backgroundSoundVolumes;

	public static float[] backgroundSoundStartTimes;

	public static float[] sourcePos;

	public static float[] volume;

	public static float[] backgroundSoundRemainingTime;

	public static AudioEngine audioEngine;

	public static WaveBank soundsWaveBank;

	public static WaveBank musicWaveBank;

	public static WaveBank voicesWaveBank;

	public static SoundBank soundsSoundBank;

	public static SoundBank musicSoundBank;

	public static SoundBank voicesSoundBank;

	public static AudioListener listener;

	public static AudioEmitter newSoundPos;

	public static Cue narratorVoiceCue;

	public static Cue[] continualSoundsCue;

	public static Cue[] backgroundSoundCue;

	public static Cue[] voiceCuePlayer;

	public static Cue[] soundCue;

	public static Cue[] musicCue;

	public static Cue[] soundCuePriority;

	public static Vector3 listenerPos;

	public static Vector3 listenerVelocity;

	public static Vector3 vForward;

	public static Vector3 vUp;

	public static Vector3 vTemp;

	public static Vector3 psvPos;

	public static Vector3 uspPos;

	public static Vector3 psbrPos;

	public static Vector3 psPos;

	public static Vector3 ptvPos;

	public static Vector3 pvPos;

	public static Vector3[] backgroundSoundPos;

	public static string[] musicList;

	public static string[] continualSoundNames;

	public static string[] soundList;

	private Game1.MasterCollection mainC;

	public void Init_Vars(Game1.MasterCollection masterC)
	{
		mainC = masterC;
		audioEngine = new AudioEngine("The_CoOp_Zombie_Game/audio.xgs");
		soundsWaveBank = new WaveBank(audioEngine, "The_CoOp_Zombie_Game/Effects.xwb");
		musicWaveBank = new WaveBank(audioEngine, "The_CoOp_Zombie_Game/Music.xwb");
		voicesWaveBank = new WaveBank(audioEngine, "The_CoOp_Zombie_Game/Voices.xwb");
		soundsSoundBank = new SoundBank(audioEngine, "The_CoOp_Zombie_Game/Effects.xsb");
		musicSoundBank = new SoundBank(audioEngine, "The_CoOp_Zombie_Game/Music.xsb");
		voicesSoundBank = new SoundBank(audioEngine, "The_CoOp_Zombie_Game/Voices.xsb");
		newSoundPos.Up = Vector3.Backward;
		newSoundPos.Velocity = new Vector3(0f, 0f, 0f);
		newSoundPos.Forward = Vector3.Up;
		listener.Up = Vector3.Backward;
	}

	public bool Init_Sounds()
	{
		_ = global::Rendering.Rendering.uBufferID;
		mainC.gameLogic.Game_Init_Music();
		for (int i = 0; i < 20; i++)
		{
			soundCue[i] = soundsSoundBank.GetCue("NewPlayer");
			soundCuePriority[i] = soundsSoundBank.GetCue("NewPlayer");
		}
		for (int i = 0; i < 6; i++)
		{
			backgroundSoundCue[i] = soundsSoundBank.GetCue("NewPlayer");
			backgroundSoundsStatus[i] = false;
			backgroundSoundTimes[i] = 1f;
			backgroundSoundStartTimes[i] = 0f;
			backgroundSoundVolumes[i] = -45f;
		}
		for (int i = 0; i < 20; i++)
		{
			voiceCuePlayer[i] = voicesSoundBank.GetCue("ZombieScowl");
		}
		narratorVoiceCue = voicesSoundBank.GetCue("ZombieScowl");
		for (int i = 0; i < numMusicCues; i++)
		{
			musicIsPlaying[i] = false;
		}
		if (numMusicCues > 0)
		{
			activeLevelMusicID = levelMusic[0];
		}
		curMusicID = musicLoadingID;
		Play_Music(musicLoadingID);
		numContinualSounds = 0;
		if (numContinualSounds > 0)
		{
			playerContinualSounds = new byte[numContinualSounds];
			continualSoundsCue = new Cue[numContinualSounds];
			continualSoundNames = new string[4];
			continualSoundNames[0] = "AirplaneEngine";
			continualSoundNames[1] = "Helicopter";
			continualSoundNames[2] = "Spaceship_Engine";
			continualSoundNames[3] = "Jet_Engine";
			for (int i = 0; i < numContinualSounds; i++)
			{
				playerContinualSounds[i] = 0;
				continualSoundsCue[i] = soundsSoundBank.GetCue("AirplaneEngine1");
			}
		}
		if (randomLevelTracks)
		{
			Set_Starting_Level_Music_To_Random_Track();
		}
		enabledBackgroundSounds = 0;
		soundSystemLoaded = true;
		return true;
	}

	public byte Play_Sound(string filename, float x, float y, float z, float vx, float vy, float vz)
	{
		if (!soundEnabled[0] || filename == null)
		{
			return byte.MaxValue;
		}
		byte b = 0;
		try
		{
			psPos.X = x;
			psPos.Y = y;
			psPos.Z = z;
			newSoundPos.Position = psPos;
			psPos.X = vx;
			psPos.Y = vy;
			psPos.Z = vz;
			newSoundPos.Velocity = psPos;
			if (soundCue[curCue].IsPlaying)
			{
				soundCue[curCue].Stop(AudioStopOptions.Immediate);
			}
			soundCue[curCue] = soundsSoundBank.GetCue(filename);
			soundCue[curCue].SetVariable("Volume", volume[0]);
			soundCue[curCue].Apply3D(listener, newSoundPos);
			soundCue[curCue].Play();
			b = curCue;
			curCue++;
			if (curCue >= 20)
			{
				curCue = 0;
			}
		}
		catch
		{
			b = byte.MaxValue;
		}
		return b;
	}

	public byte Play_Priority_Sound(string filename, float x, float y, float z, float vx, float vy, float vz)
	{
		if (!soundEnabled[0] || filename == null || !Is_Priority_Cue_Ready())
		{
			return byte.MaxValue;
		}
		byte b = 0;
		try
		{
			psPos.X = x;
			psPos.Y = y;
			psPos.Z = z;
			newSoundPos.Position = psPos;
			psPos.X = vx;
			psPos.Y = vy;
			psPos.Z = vz;
			newSoundPos.Velocity = psPos;
			soundCuePriority[curPriorityCue] = soundsSoundBank.GetCue(filename);
			soundCuePriority[curPriorityCue].SetVariable("Volume", volume[0]);
			soundCuePriority[curPriorityCue].Apply3D(listener, newSoundPos);
			soundCuePriority[curPriorityCue].Play();
			b = curPriorityCue;
			curPriorityCue++;
			if (curPriorityCue >= 20)
			{
				curPriorityCue = 0;
			}
		}
		catch
		{
			b = byte.MaxValue;
		}
		return b;
	}

	public byte Player_Priority_Sound_From_SoundList(ushort soundListID, float x, float y, float z, float vx, float vy, float vz)
	{
		try
		{
			return Play_Priority_Sound(soundList[soundListID], x, y, z, vx, vy, vz);
		}
		catch
		{
			return 0;
		}
	}

	public bool Is_Priority_Cue_Ready()
	{
		if (!soundCuePriority[curPriorityCue].IsPlaying)
		{
			return true;
		}
		bool flag = false;
		ushort num;
		for (num = curPriorityCue; num < 20; num++)
		{
			if (!soundCuePriority[num].IsPlaying)
			{
				flag = true;
				break;
			}
		}
		if (!flag)
		{
			for (num = 0; num < curPriorityCue; num++)
			{
				if (!soundCuePriority[num].IsPlaying)
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				return false;
			}
		}
		curPriorityCue = (byte)num;
		return true;
	}

	public void Stop_Priority_Sound(int soundID)
	{
		if (soundID == 255)
		{
			return;
		}
		try
		{
			if (soundCuePriority[soundID].IsPlaying)
			{
				soundCuePriority[soundID].Stop(AudioStopOptions.AsAuthored);
			}
		}
		catch
		{
			soundID = 255;
		}
	}

	public void Update_Priority_Sound_Position(int soundID, float x, float y, float z)
	{
		if (!soundEnabled[0] || soundID == 255)
		{
			return;
		}
		try
		{
			uspPos.X = x;
			uspPos.Y = y;
			uspPos.Z = z;
			newSoundPos.Position = uspPos;
			if (soundCuePriority[soundID].IsPlaying)
			{
				soundCuePriority[soundID].Apply3D(listener, newSoundPos);
			}
		}
		catch
		{
			soundID = 255;
		}
	}

	public byte Play_Sound_NonPositional(string filename)
	{
		if (!soundEnabled[0] || filename == null || !Is_Priority_Cue_Ready())
		{
			return byte.MaxValue;
		}
		byte b = 0;
		try
		{
			if (soundCuePriority[curPriorityCue].IsPlaying)
			{
				soundCuePriority[curPriorityCue].Stop(AudioStopOptions.Immediate);
			}
			soundCuePriority[curPriorityCue] = soundsSoundBank.GetCue(filename);
			soundCuePriority[curPriorityCue].SetVariable("Volume", volume[0]);
			soundCuePriority[curPriorityCue].Play();
			b = curPriorityCue;
			curPriorityCue++;
			if (curPriorityCue >= 20)
			{
				curPriorityCue = 0;
			}
		}
		catch
		{
			b = byte.MaxValue;
		}
		return b;
	}

	public short Play_Repetitive_Sound(short soundID, string filename, float x, float y, float z, float vx, float vy, float vz)
	{
		if (!soundEnabled[0])
		{
			return soundID;
		}
		try
		{
			psPos.X = x;
			psPos.Y = y;
			psPos.Z = z;
			newSoundPos.Position = psPos;
			psPos.X = vx;
			psPos.Y = vy;
			psPos.Z = vz;
			newSoundPos.Velocity = psPos;
			if (soundID < 0)
			{
				if (soundCue[curCue].IsPlaying)
				{
					soundCue[curCue].Stop(AudioStopOptions.Immediate);
				}
				soundCue[curCue] = soundsSoundBank.GetCue(filename);
				soundID = curCue;
				curCue++;
				if (curCue >= 20)
				{
					curCue = 0;
				}
			}
			else
			{
				if (soundCue[soundID].IsPlaying)
				{
					return soundID;
				}
				soundCue[soundID] = soundsSoundBank.GetCue(filename);
			}
			soundCue[soundID].SetVariable("Volume", volume[0]);
			soundCue[soundID].Apply3D(listener, newSoundPos);
			soundCue[soundID].Play();
		}
		catch
		{
			soundID = -1;
		}
		return soundID;
	}

	public byte Play_Sound_Volume(string filename, float soundVolume, float x, float y, float z, float vx, float vy, float vz)
	{
		if (!soundEnabled[0])
		{
			return byte.MaxValue;
		}
		byte b = 0;
		try
		{
			psvPos.X = x;
			psvPos.Y = y;
			psvPos.Z = z;
			newSoundPos.Position = psvPos;
			psvPos.X = vx;
			psvPos.Y = vy;
			psvPos.Z = vz;
			newSoundPos.Velocity = psvPos;
			if (soundCue[curCue].IsPlaying)
			{
				soundCue[curCue].Stop(AudioStopOptions.Immediate);
			}
			soundCue[curCue] = soundsSoundBank.GetCue(filename);
			soundCue[curCue].SetVariable("Volume", soundVolume + volume[0]);
			soundCue[curCue].Apply3D(listener, newSoundPos);
			soundCue[curCue].Play();
			b = curCue;
			curCue++;
			if (curCue >= 20)
			{
				curCue = 0;
			}
		}
		catch
		{
			b = byte.MaxValue;
		}
		return b;
	}

	public void Stop_Sound(int soundID)
	{
		if (soundID == 255)
		{
			return;
		}
		try
		{
			if (soundCue[soundID].IsPlaying)
			{
				soundCue[soundID].Stop(AudioStopOptions.AsAuthored);
			}
		}
		catch
		{
			soundID = 255;
		}
	}

	public void Stop_All_Sound_Effects()
	{
		try
		{
			for (int i = 0; i < 20; i++)
			{
				if (soundCue[i].IsPlaying)
				{
					soundCue[i].Stop(AudioStopOptions.AsAuthored);
				}
				if (soundCuePriority[i].IsPlaying)
				{
					soundCuePriority[i].Stop(AudioStopOptions.AsAuthored);
				}
			}
		}
		catch
		{
		}
	}

	public void Stop_All_Background_Sounds()
	{
		try
		{
			for (int i = 0; i < 6; i++)
			{
				if (backgroundSoundCue[i].IsPlaying)
				{
					backgroundSoundCue[i].Stop(AudioStopOptions.Immediate);
				}
			}
		}
		catch
		{
		}
	}

	public void Update_Sound_Position(int soundID, float x, float y, float z)
	{
		if (!soundEnabled[0] || soundID == 255)
		{
			return;
		}
		try
		{
			uspPos.X = x;
			uspPos.Y = y;
			uspPos.Z = z;
			newSoundPos.Position = uspPos;
			if (soundCue[soundID].IsPlaying)
			{
				soundCue[soundID].Apply3D(listener, newSoundPos);
			}
		}
		catch
		{
			soundID = 255;
		}
	}

	public void Update_Sounds(float x, float y, float z)
	{
		try
		{
			if (soundEnabled[0] && !narratorVoiceCue.IsPlaying)
			{
				for (ushort num = 0; num < 6; num++)
				{
					if (backgroundSoundsStatus[num] && !backgroundSoundCue[num].IsPlaying)
					{
						backgroundSoundRemainingTime[num] -= global::MainGame.MainGame.frametime;
						if (backgroundSoundRemainingTime[num] < 0f)
						{
							backgroundSoundRemainingTime[num] = backgroundSoundTimes[num];
							backgroundSoundPos[num].X = 0f;
							backgroundSoundPos[num].Y = 0f;
							backgroundSoundPos[num].Z = 0f;
							newSoundPos.Velocity = backgroundSoundPos[num];
							backgroundSoundPos[num].X = x;
							backgroundSoundPos[num].Y = y;
							backgroundSoundPos[num].Z = z;
							newSoundPos.Position = backgroundSoundPos[num];
							backgroundSoundCue[num] = soundsSoundBank.GetCue("Background_Sounds_" + num);
							backgroundSoundCue[num].SetVariable("Volume", backgroundSoundVolumes[num]);
							backgroundSoundCue[num].Apply3D(listener, newSoundPos);
							backgroundSoundCue[num].Play();
						}
					}
				}
			}
			Update_Sound_Positions();
		}
		catch
		{
			ushort num = 255;
		}
	}

	public void Start_Background_Sound(ushort soundID, float startTime, float volume)
	{
		backgroundSoundsStatus[soundID] = true;
		backgroundSoundRemainingTime[soundID] = startTime;
		backgroundSoundStartTimes[soundID] = startTime;
		backgroundSoundVolumes[soundID] = volume;
	}

	public void Set_Background_Sound_Repeat_Interval(ushort soundID, float time)
	{
		backgroundSoundTimes[soundID] = time;
	}

	public void Set_Background_Sound_Volume(ushort soundID, float volume)
	{
		backgroundSoundVolumes[soundID] = volume;
	}

	public void Disable_Background_Sounds()
	{
		for (byte b = 0; b < 6; b++)
		{
			backgroundSoundsStatus[b] = false;
		}
	}

	public void Disable_Background_Sound(ushort soundID)
	{
		backgroundSoundsStatus[soundID] = false;
	}

	public void Update_Sound_Positions()
	{
		try
		{
			for (byte b = 0; b < 6; b++)
			{
				newSoundPos.Position = backgroundSoundPos[b];
				backgroundSoundCue[b].Apply3D(listener, newSoundPos);
			}
		}
		catch
		{
		}
	}

	public void Play_Continual_Sound(ushort cueID, byte soundID, bool stop, float desiredVolume, float pitch)
	{
		if (!soundEnabled[0])
		{
			return;
		}
		fixVolume = desiredVolume;
		float value = -96f + (volume[0] + 96f) * desiredVolume;
		try
		{
			psPos.X = 0f;
			psPos.Y = 0f;
			psPos.Z = 0f;
			newSoundPos.Velocity = psPos;
			psPos.X = listener.Position.X;
			psPos.Y = listener.Position.Y;
			psPos.Z = listener.Position.Z;
			if (stop)
			{
				if (continualSoundsCue[cueID].IsPlaying)
				{
					psPos.X += 1000f;
					newSoundPos.Position = psPos;
					continualSoundsCue[cueID].SetVariable("Volume", -96f);
					continualSoundsCue[cueID].Apply3D(listener, newSoundPos);
				}
				return;
			}
			float num = Math.Abs(1f - desiredVolume);
			psPos.X += 500f * num * num * num;
			newSoundPos.Position = psPos;
			if (continualSoundsCue[cueID].IsPlaying)
			{
				continualSoundsCue[cueID].SetVariable("Volume", value);
				continualSoundsCue[cueID].SetVariable("Pitch", pitch);
				continualSoundsCue[cueID].Apply3D(listener, newSoundPos);
			}
			else
			{
				continualSoundsCue[cueID] = soundsSoundBank.GetCue(continualSoundNames[playerContinualSounds[cueID]] + soundID);
				continualSoundsCue[cueID].SetVariable("Volume", value);
				continualSoundsCue[cueID].SetVariable("Pitch", pitch);
				continualSoundsCue[cueID].Apply3D(listener, newSoundPos);
				continualSoundsCue[cueID].Play();
			}
		}
		catch
		{
		}
	}

	public void Play_Moving_Continual_Sound(ushort cueID, byte soundID, bool stop, float desiredVolume, float pitch, float x, float y, float z, float vx, float vy, float vz)
	{
		if (!soundEnabled[0])
		{
			return;
		}
		fixVolume = desiredVolume;
		float value = -96f + (volume[0] + 96f) * desiredVolume;
		try
		{
			psPos.X = vx;
			psPos.Y = vy;
			psPos.Z = vz;
			newSoundPos.Velocity = psPos;
			psPos.X = x;
			psPos.Y = y;
			psPos.Z = z;
			if (stop)
			{
				if (continualSoundsCue[cueID].IsPlaying)
				{
					continualSoundsCue[cueID].Stop(AudioStopOptions.Immediate);
				}
				return;
			}
			newSoundPos.Position = psPos;
			if (continualSoundsCue[cueID].IsPlaying)
			{
				continualSoundsCue[cueID].SetVariable("Volume", value);
				continualSoundsCue[cueID].SetVariable("Pitch", pitch);
				continualSoundsCue[cueID].Apply3D(listener, newSoundPos);
			}
			else
			{
				continualSoundsCue[cueID] = soundsSoundBank.GetCue(continualSoundNames[playerContinualSounds[cueID]] + soundID);
				continualSoundsCue[cueID].SetVariable("Volume", value);
				continualSoundsCue[cueID].SetVariable("Pitch", pitch);
				continualSoundsCue[cueID].Apply3D(listener, newSoundPos);
				continualSoundsCue[cueID].Play();
			}
		}
		catch
		{
		}
	}

	public void Stop_All_Continual_Sounds()
	{
		for (ushort num = 0; num < numContinualSounds; num++)
		{
			if (continualSoundsCue[num].IsPlaying)
			{
				continualSoundsCue[num].Stop(AudioStopOptions.Immediate);
			}
		}
	}

	public void Stop_Continual_Sound(ushort cueID)
	{
		if (cueID < numContinualSounds && continualSoundsCue[cueID].IsPlaying)
		{
			continualSoundsCue[cueID].Stop(AudioStopOptions.Immediate);
		}
	}

	public void Set_Continual_Sounds_Volume()
	{
		for (ushort num = 0; num < numContinualSounds; num++)
		{
			try
			{
				continualSoundsCue[num].SetVariable("Volume", -96f + (volume[0] + 96f) * fixVolume);
			}
			catch
			{
			}
		}
	}

	public void Set_Continual_Sounds_Player_Index(ushort playerID, byte index)
	{
		if (index < numContinualSounds)
		{
			playerContinualSounds[playerID] = index;
		}
	}

	public bool Play_Narrator_Voice(string filename)
	{
		if (!soundEnabled[2])
		{
			return true;
		}
		try
		{
			Stop_All_Voices(stopNarrator: true);
			Stop_All_Background_Sounds();
			narratorVoiceCue = voicesSoundBank.GetCue(filename);
			narratorVoiceCue.SetVariable("Volume", volume[2]);
			narratorVoiceCue.Play();
			return true;
		}
		catch
		{
			return false;
		}
	}

	public void Stop_Narrator_Voice()
	{
		if (!soundEnabled[2])
		{
			return;
		}
		try
		{
			if (narratorVoiceCue.IsPlaying)
			{
				narratorVoiceCue.Stop(AudioStopOptions.Immediate);
			}
		}
		catch
		{
		}
	}

	public bool Is_Narrator_Playing()
	{
		return narratorVoiceCue.IsPlaying;
	}

	public int Play_Voice(string filename, float x, float y, float z, float vx, float vy, float vz)
	{
		if (!soundEnabled[2])
		{
			return -1;
		}
		try
		{
			ptvPos.X = x;
			ptvPos.Y = y;
			ptvPos.Z = z;
			newSoundPos.Position = ptvPos;
			ptvPos.X = vx;
			ptvPos.Y = vy;
			ptvPos.Z = vz;
			newSoundPos.Velocity = ptvPos;
			if (voiceCuePlayer[curVoice1Cue].IsPlaying)
			{
				voiceCuePlayer[curVoice1Cue].Stop(AudioStopOptions.Immediate);
			}
			voiceCuePlayer[curVoice1Cue] = voicesSoundBank.GetCue(filename);
			voiceCuePlayer[curVoice1Cue].SetVariable("Volume", volume[0]);
			voiceCuePlayer[curVoice1Cue].Apply3D(listener, newSoundPos);
			voiceCuePlayer[curVoice1Cue].Play();
		}
		catch
		{
			x = 255f;
		}
		int result = curVoice1Cue;
		curVoice1Cue++;
		if (curVoice1Cue >= 20)
		{
			curVoice1Cue = 0;
		}
		return result;
	}

	public void Stop_All_Voices(bool stopNarrator)
	{
		try
		{
			for (byte b = 0; b < 20; b++)
			{
				if (voiceCuePlayer[b].IsPlaying)
				{
					voiceCuePlayer[b].Stop(AudioStopOptions.Immediate);
				}
			}
			if (stopNarrator && narratorVoiceCue.IsPlaying)
			{
				narratorVoiceCue.Stop(AudioStopOptions.Immediate);
			}
		}
		catch
		{
		}
	}

	public void Update_Voice_Position(int soundID, float x, float y, float z)
	{
		try
		{
			uspPos.X = 0f;
			uspPos.Y = 0f;
			uspPos.Z = 0f;
			newSoundPos.Velocity = uspPos;
			uspPos.X = x;
			uspPos.Y = y;
			uspPos.Z = z;
			newSoundPos.Position = uspPos;
			voiceCuePlayer[soundID].Apply3D(listener, newSoundPos);
		}
		catch
		{
		}
	}

	public bool Is_Voice_Playing(int voiceCueID)
	{
		if (voiceCueID < 0 || voiceCueID >= 20)
		{
			return false;
		}
		return voiceCuePlayer[voiceCueID].IsPlaying;
	}

	public void Stop_Voice(int voiceCueID)
	{
		if (!soundEnabled[2] || voiceCueID < 0 || voiceCueID >= 20)
		{
			return;
		}
		try
		{
			if (voiceCuePlayer[voiceCueID].IsPlaying)
			{
				voiceCuePlayer[voiceCueID].Stop(AudioStopOptions.Immediate);
			}
		}
		catch
		{
		}
	}

	public void Set_Listener_Position(float x, float y, float z, ref StructsClass.vtex velocity)
	{
		listenerPos.X = x;
		listenerPos.Y = y;
		listenerPos.Z = z;
		listener.Position = listenerPos;
		listenerVelocity.X = velocity.v[0];
		listenerVelocity.Y = velocity.v[1];
		listenerVelocity.Z = velocity.v[2];
		listener.Velocity = listenerVelocity;
		Vector3.TransformNormal(ref vForward, ref global::Rendering.Rendering.matrixVInverse, out vTemp);
		Vector3.Normalize(ref vTemp, out vTemp);
		listener.Forward = vTemp;
		Vector3.TransformNormal(ref vUp, ref global::Rendering.Rendering.matrixVInverse, out vTemp);
		Vector3.Normalize(ref vTemp, out vTemp);
		listener.Up = vTemp;
	}

	public void Close_Sound()
	{
		soundsSoundBank.Dispose();
		soundsWaveBank.Dispose();
		audioEngine.Dispose();
	}

	public void Update_Sound_Settings(byte soundType)
	{
		switch (soundType)
		{
		case 0:
			if (!soundEnabled[0])
			{
				Stop_All_Sound_Effects();
				Stop_All_Continual_Sounds();
				Stop_All_Background_Sounds();
			}
			if (!soundEnabled[2])
			{
				Stop_All_Voices(stopNarrator: true);
			}
			break;
		case 1:
			if (soundEnabled[1])
			{
				Enable_Music();
			}
			else
			{
				Disable_Music();
			}
			break;
		}
	}

	public void Process_Sounds()
	{
		audioEngine.Update();
	}

	public void Level_Reset()
	{
		Rotate_Level_Music();
	}

	public void Stop_All_Game_Sounds(bool stopNarrator)
	{
		Stop_All_Sound_Effects();
		Stop_All_Continual_Sounds();
		Stop_All_Background_Sounds();
		Stop_All_Voices(stopNarrator);
	}

	public void Reset_Round(bool stopNarrator)
	{
		Stop_All_Game_Sounds(stopNarrator);
		Reset_Background_Sounds();
	}

	public void Reset_Background_Sounds()
	{
		for (ushort num = 0; num < 6; num++)
		{
			backgroundSoundRemainingTime[num] = backgroundSoundStartTimes[num];
		}
	}

	public void Check_Music_Status()
	{
		byte b = 0;
		try
		{
			byte b2;
			for (b2 = 0; b2 < numMusicCues; b2++)
			{
				if (musicIsPlaying[b2])
				{
					curMusicID = b2;
					b = b2;
					b2++;
					break;
				}
			}
			while (b2 < numMusicCues)
			{
				if (musicCue[b2] != null && !musicCue[b2].IsDisposed)
				{
					if (musicCue[b2].IsPlaying)
					{
						musicCue[b2].Stop(AudioStopOptions.Immediate);
					}
					musicCue[b2].Dispose();
				}
				musicIsPlaying[b2] = false;
				b2++;
			}
		}
		catch
		{
			byte b2 = byte.MaxValue;
		}
		if (musicIsPlaying[b])
		{
			Play_Music(b);
		}
	}

	public void Set_Music(byte musicID)
	{
		try
		{
			for (byte b = 0; b < numMusicCues; b++)
			{
				if (b != musicID)
				{
					musicIsPlaying[b] = false;
					if (musicCue[b] != null && !musicCue[b].IsDisposed)
					{
						if (musicCue[b].IsPlaying)
						{
							musicCue[b].Stop(AudioStopOptions.Immediate);
						}
						musicCue[b].Dispose();
					}
				}
				else
				{
					musicIsPlaying[b] = true;
				}
			}
		}
		catch
		{
			byte b = byte.MaxValue;
		}
		Play_Music(musicID);
		curMusicID = musicID;
	}

	public void Set_Music_Volume()
	{
		try
		{
			if (musicCue[curMusicID] != null && !musicCue[curMusicID].IsDisposed)
			{
				musicCue[curMusicID].SetVariable("Volume", volume[1]);
			}
		}
		catch
		{
		}
	}

	public void Rotate_Level_Music()
	{
		if (numMusicCues >= 1)
		{
			curLevelMusicID++;
			if (curLevelMusicID >= numLevelMusic)
			{
				curLevelMusicID = 0;
			}
			activeLevelMusicID = levelMusic[curLevelMusicID];
		}
	}

	public void Set_Starting_Level_Music_To_Random_Track()
	{
		curLevelMusicID = (byte)((float)global::MainGame.MainGame.mainRandom.Next(0, 32000) / 32000f * (float)(int)numLevelMusic);
		if (curLevelMusicID >= numLevelMusic)
		{
			curLevelMusicID = 0;
		}
	}

	public void Play_Level_Music()
	{
		Set_Music(activeLevelMusicID);
	}

	public void Change_Level_Music(byte musicID, bool playNow)
	{
		if (numMusicCues >= 1)
		{
			activeLevelMusicID = musicID;
			if (playNow)
			{
				Set_Music(activeLevelMusicID);
			}
		}
	}

	public void Set_Level_Starting_Music(byte musicID, bool playNow)
	{
		if (numMusicCues < 1)
		{
			return;
		}
		levelStartingMusic = musicID;
		activeLevelMusicID = musicID;
		curLevelMusicID = 0;
		for (byte b = 0; b < numLevelMusic; b++)
		{
			if (levelMusic[b] == musicID)
			{
				curLevelMusicID = b;
				break;
			}
		}
		if (playNow)
		{
			Set_Music(activeLevelMusicID);
		}
	}

	public void Restore_Level_Music()
	{
		if (numMusicCues < 1)
		{
			return;
		}
		activeLevelMusicID = levelStartingMusic;
		curLevelMusicID = 0;
		for (byte b = 0; b < numLevelMusic; b++)
		{
			if (levelMusic[b] == levelStartingMusic)
			{
				curLevelMusicID = b;
				break;
			}
		}
		Set_Music(activeLevelMusicID);
	}

	public void Disable_Music()
	{
		soundEnabled[1] = false;
		try
		{
			for (byte b = 0; b < numMusicCues; b++)
			{
				if (musicCue[b] != null && !musicCue[b].IsDisposed)
				{
					if (musicCue[b].IsPlaying)
					{
						musicCue[b].Stop(AudioStopOptions.Immediate);
					}
					musicCue[b].Dispose();
				}
			}
		}
		catch
		{
			byte b = byte.MaxValue;
		}
	}

	public void Enable_Music()
	{
		if (numMusicCues >= 1)
		{
			soundEnabled[1] = true;
			Check_Music_Status();
		}
	}

	public void Stop_Music()
	{
		try
		{
			for (byte b = 0; b < numMusicCues; b++)
			{
				if (musicCue[b] != null && !musicCue[b].IsDisposed)
				{
					if (musicCue[b].IsPlaying)
					{
						musicCue[b].Stop(AudioStopOptions.Immediate);
					}
					musicCue[b].Dispose();
				}
			}
		}
		catch
		{
			byte b = byte.MaxValue;
		}
	}

	public void Play_Music(byte musicID)
	{
		try
		{
			if (numMusicCues >= 1 && soundEnabled[1] && (musicCue[musicID] == null || musicCue[musicID].IsDisposed || !musicCue[musicID].IsPlaying))
			{
				musicCue[musicID] = musicSoundBank.GetCue(musicList[musicID]);
				musicCue[musicID].Play();
				musicCue[musicID].SetVariable("Volume", volume[1]);
			}
		}
		catch
		{
		}
	}

	static Sounds()
	{
		bool[] array = new bool[3];
		soundEnabled = array;
		backgroundSoundsStatus = new bool[6];
		numLevelMusic = 0;
		musicLoadingID = 0;
		musicMenuID = 0;
		numMusicCues = 0;
		curLevelMusicID = 0;
		curMusicID = 0;
		activeLevelMusicID = 0;
		levelStartingMusic = 0;
		curCue = 0;
		curVoice1Cue = 0;
		numContinualSounds = 0;
		fixVolume = -45f;
		backgroundSoundTimes = new float[6];
		backgroundSoundVolumes = new float[6];
		backgroundSoundStartTimes = new float[6];
		float[] array2 = new float[3];
		sourcePos = array2;
		volume = new float[3] { -45f, -45f, -12f };
		backgroundSoundRemainingTime = new float[6];
		listener = new AudioListener();
		newSoundPos = new AudioEmitter();
		backgroundSoundCue = new Cue[6];
		voiceCuePlayer = new Cue[20];
		soundCue = new Cue[20];
		soundCuePriority = new Cue[20];
		listenerPos = new Vector3(0f, 0f, 0f);
		listenerVelocity = new Vector3(0f, 0f, 0f);
		vForward = new Vector3(0f, 0f, -1f);
		vUp = new Vector3(0f, 1f, 0f);
		vTemp = default(Vector3);
		psvPos = new Vector3(0f, 0f, 0f);
		uspPos = new Vector3(0f, 0f, 0f);
		psbrPos = new Vector3(0f, 0f, 0f);
		psPos = new Vector3(0f, 0f, 0f);
		ptvPos = new Vector3(0f, 0f, 0f);
		pvPos = new Vector3(0f, 0f, 0f);
		backgroundSoundPos = new Vector3[6];
	}
}
