# The Co-Op Zombie Game — Windows Port

An experimental Windows port of an Xbox 360 Indie Game dump, reconstructed on
.NET 8 using FNA. The project currently boots, loads single-player levels,
renders skinned player/zombie models, converts Xbox collision and navigation
data, and advances enemy waves.

This repository does **not** contain the original Xbox package, executable,
music, models, textures, maps, or other extracted game content. You must supply
your own legally obtained dump. See [PORTING.md](PORTING.md) for the extraction,
conversion, build, and debugging record.

## AI authorship disclosure

All code written specifically for this Windows port—including project
scaffolding, endian conversion, compatibility shaders, diagnostics, and bug
fixes—was produced by OpenAI Codex using the **GPT-5.6-Sol** model.

No human-authored porting code was contributed. Human input was limited to
providing the personally obtained game dump, playtesting builds, reporting
observed behavior, and requesting fixes. This statement concerns the porting
work only; the original game and its decompiled logic were created by their
original human developers.

## Current status

- .NET 8/FNA desktop project builds on Windows x64.
- Xbox big-endian model and collision buffers are converted field by field.
- Xbox navigation meshes are parsed using their big-endian, 32-bit layout.
- The compatibility effect supports basic textured rendering and skeletal
  skinning for player and zombie models.
- Single-player AI navigation and wave progression are operational in the
  current playtest path.
- Rendering remains a compatibility baseline rather than a faithful recreation
  of all 23 original Xbox shader programs.
- Multiplayer uses `FNA.NetStub`; Xbox Live services are not implemented.
- Shutdown still needs conversion from `Thread.Abort()` to cooperative
  cancellation.

## Building

Requirements:

- Windows x64
- .NET 8 SDK
- a legally obtained dump of the game
- the native FNA libraries (`SDL3.dll`, `FNA3D.dll`, `FAudio.dll`, and
  `libtheorafile.dll`)

Clone with submodules:

```powershell
git clone --recurse-submodules <repository-url>
cd coopZombie-windows-port
dotnet restore .\Port\TheCoOpZombieGame.csproj
dotnet build .\Port\TheCoOpZombieGame.csproj
```

The project expects extracted content under
`Extracted/584E07D1/The_CoOp_Zombie_Game/`. That directory is intentionally
ignored and must never be committed. The native FNA DLLs are likewise local
dependencies; consult [PORTING.md](PORTING.md) for the tested layout and exact
effect-compilation command.

## Licensing and third-party rights

New porting work in this repository is released under the [MIT License](LICENSE).
That license does not grant rights to the original game, its decompiled source,
or its assets. Those remain the property of their respective rights holders.
FNA, FNA.NetStub, and included third-party utilities retain their own licenses;
see [THIRD_PARTY_NOTICES.md](THIRD_PARTY_NOTICES.md).

This is an independent preservation and interoperability project and is not
affiliated with or endorsed by the original developer, Microsoft, or Xbox.