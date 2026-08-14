# The Co-Op Zombie Game: Xbox 360-to-Windows study port

This workspace is arranged so that evidence, reconstruction, and porting work
stay separate:

- `OriginalDump/game` is the untouched Xbox 360 `LIVE`/STFS package.
- `Extracted/` is the read-only package extraction.
- `Decompiled/` is ILSpy's archival C# reconstruction.
- `Port/` is the editable Windows/FNA port.
- `tools/` contains the source-visible extraction and shader tools.

The original dump and extracted executable currently retain these SHA-256
hashes:

```text
OriginalDump/game
5CD23B25C02AA7926B690A4C998CBB35A7752FA833559263DBED26F4222F5E91

Extracted/584E07D1/TheCoOpZombieGame.exe
D783D9697B6FF22C79336DDD89AF04AA8334D02AA92C01777BB770FCD7D95A22
```

## What the dump contains

The package metadata identifies the title as `The Co-Op Zombie Game`, Title ID
`0x584E07D2`. Its executable is a managed .NET/XNA 4.0 assembly, so this is a
much friendlier preservation target than a native PowerPC Xbox 360 game. ILSpy
can reconstruct C# from its MSIL; this is not the original source and does not
recover comments, original local names, or the original Visual Studio project.

The Windows port targets .NET 8 and references:

- [FNA](https://github.com/FNA-XNA/FNA), an XNA 4 reimplementation.
- [FNA.NetStub](https://github.com/FNA-XNA/FNA.NetStub), legacy Xbox Live API
  stubs used by old XNA games.

## Reproduce the main stages

Run these commands from the workspace root in PowerShell.

List or extract the STFS package:

```powershell
.\tools\Extract-STFS\Extract-STFS.ps1 -Path .\OriginalDump\game -ListOnly
.\tools\Extract-STFS\Extract-STFS.ps1 -Path .\OriginalDump\game -OutputDir .\Extracted
```

Reconstruct the managed project with ILSpy:

```powershell
.\tools\ilspy\ilspycmd.exe -p -o .\Decompiled --nested-directories `
  .\Extracted\584E07D1\TheCoOpZombieGame.exe
```

Extract the raw Xbox Effects Framework object from its compressed XNB:

```powershell
dotnet build .\tools\XnbEffectExtractor\XnbEffectExtractor.csproj
.\tools\XnbEffectExtractor\bin\Debug\net8.0\XnbEffectExtractor.exe `
  .\Extracted\584E07D1\The_CoOp_Zombie_Game\Effect_Main.xnb `
  .\Port\ContentSource\Effect_Main.cso
```

The XNB is 10,318 bytes and expands to a 70,584-byte Xbox effect object. It
contains 23 Xbox shader markers. `Effect_Main.first-translated-shader.hlsl` is
the XenosRecomp translation of the first program, an animated-texture pixel
shader. It is a study artifact, not a complete replacement for the multi-pass
effect.

Compile the current Windows compatibility effect with the Windows SDK's legacy
HLSL compiler:

```powershell
& 'C:\Program Files (x86)\Windows Kits\10\bin\10.0.26100.0\x64\fxc.exe' `
  /nologo /T fx_2_0 `
  /Fo .\Port\ContentSource\Effect_Main.fxb `
  .\Port\ContentSource\Effect_Main.compat.fx
```

Build and run the port:

```powershell
dotnet build .\Port\TheCoOpZombieGame.csproj --no-restore
Set-Location .\Port\bin\Debug\net8.0
.\TheCoOpZombieGame.exe
```

If restore metadata is absent, run this first:

```powershell
dotnet restore .\Port\TheCoOpZombieGame.csproj --configfile .\NuGet.Config
```

## Exact Xbox-to-Windows conversion ledger

This section records the conversion differences implemented in `Port/`. It is
intended to be kept in sync with the code. `Decompiled/` remains the unchanged
reference reconstruction.

### Runtime and project system

| Xbox/decompiled state | Windows port | Reason or consequence |
| --- | --- | --- |
| Old non-SDK C# project targeting .NET 2.0/XNA 4.0 assemblies | SDK-style `Port/TheCoOpZombieGame.csproj` targeting `net8.0` | Current .NET SDK can build and debug the reconstructed code. |
| Direct references to `Microsoft.Xna.Framework.*` Xbox/Windows assemblies | Project reference to `FNA/FNA.Core.csproj` | FNA supplies the XNA 4-compatible desktop API. |
| Xbox Live Gamer Services assembly | `Port/FNA.NetStub.Core.csproj`, which builds the FNA.NetStub source for .NET 8 | Old Gamer Services calls can resolve, but online Xbox Live behavior is stubbed. |
| Xbox-native graphics/audio/input runtime | `FNA3D.dll`, `FAudio.dll`, `SDL3.dll`, and `libtheorafile.dll` copied from `tools/fnalibs/x64` | Supplies the Windows x64 native backend used by FNA. |
| Content deployed by the Xbox project | Extracted content is linked into the output under the original `The_CoOp_Zombie_Game` root | Existing `TitleContainer` and `ContentManager` paths continue to work without mass source edits. |
| Decompiled `AnyCPU`-style managed executable assumptions | Windows x64 native DLL set beside the .NET executable | The managed code remains portable, but this particular output requires the matching x64 native libraries. |

The port preserves `AllowUnsafeBlocks=true` and
`CheckForOverflowUnderflow=false`. Both matter because the original game uses
pointer casts over packed model/collision buffers and relies on unchecked
integer behavior in several hot paths.

### Endianness: the important mixed-format rule

Xbox 360 uses a big-endian PowerPC CPU; the current Windows machine uses a
little-endian x64 CPU. The custom game files are **mixed format**, not wholly
big-endian:

- Strings, counts, flags, and ordinary fields written through .NET
  `BinaryWriter.Write(...)` are read normally with `BinaryReader`. Do not swap
  the whole file.
- Selected bulk arrays were emitted with the game's explicit
  `Write_Float_Reversed`, `Write_Int_Reversed`, and `Write_Ushort_Reversed`
  helpers. The Xbox loader casts those bytes directly to native pointers.
- On Windows, only those raw blocks are reversed in-place before the same
  pointer code runs.
- Conversion is guarded by `BitConverter.IsLittleEndian`, preventing a second
  swap if the code is ever run on a big-endian target.

The first diagnostic proved the issue exactly. A model vertex index appeared
on Windows as `0x3C000000` (`1006632960`), which would index far outside the
array. Reversing its four bytes produces `0x0000003C` (`60`), valid for that
model's 67 vertices.

#### Condensed main and level model buffers

`Models.ConvertModelBuffersFromXboxEndian` reverses every four-byte element in
the following arrays after both version 0 and version 1 model loaders read
them:

| Buffer | Logical element | Conversion |
| --- | --- | --- |
| `vertexBytes` | `float` X/Y/Z components | Reverse each 4-byte float |
| `normalBytes` | `float` normal X/Y/Z components | Reverse each 4-byte float |
| `textureBytes` | `float` U/V components | Reverse each 4-byte float |
| `vIndexBytes` | 32-bit vertex indices | Reverse each 4-byte integer |
| `nIndexBytes` | 32-bit normal indices | Reverse each 4-byte integer |
| `tIndexBytes` | 32-bit texture-coordinate indices | Reverse each 4-byte integer |
| `tangentBytes` | `float` tangent X/Y/Z components | Reverse each 4-byte float |
| `bwBytes0` through `bwBytes3` | Four separate `float` skin weights | Reverse each 4-byte float |

The one-byte `blendIndex0` through `blendIndex3` arrays are deliberately not
changed; a single byte has no endian ordering. The normal .NET metadata fields
such as `vcount`, `ncount`, `tcount`, `pcount`, texture names, colors, flags,
and instance counts are also not changed.

The conversion is applied to:

- main models loaded by `Load_Condensed_Model_Version_0`;
- main models loaded by `Load_Condensed_Model_Version_1`;
- level models loaded by `Load_Condensed_Level_Model_Version_0`;
- level models loaded by `Load_Condensed_Level_Model_Version_1`.

`Create_Main_Model_VBO` now also checks the destination vertex index and color
array before dereferencing. A bad asset therefore produces an
`InvalidDataException` containing the model, triangle, index, and counts rather
than a generic `IndexOutOfRangeException`.

#### Condensed collision buffers

Starting a single-player match exposed the same issue in
`Load_Condensed_Collision_Model`. The byte-swapped vertex index was followed by
unsafe pointer code, causing a process-level `AccessViolationException` rather
than a recoverable managed exception.

The Windows loader now performs these targeted conversions:

| Collision data | Width | Conversion |
| --- | --- | --- |
| `tempModel.vertexBytes` | 32-bit floats | Reverse every 4 bytes |
| `tempModel.normalBytes` | 32-bit floats | Reverse every 4 bytes |
| `tempModel.vIndexBytes` | 32-bit integers | Reverse every 4 bytes |
| `tempModel.nIndexBytes` | 32-bit integers | Reverse every 4 bytes |
| collision-box `id` array | 16-bit unsigned integers | Reverse every 2 bytes |
| collision-box X/Y/Z and X2/Y2/Z2 array | 32-bit floats | Reverse every 4 bytes |
| collision-box `numIDs` array | 16-bit unsigned integers | Reverse every 2 bytes |
| flattened collision-box polygon `ids` | 16-bit unsigned integers | Reverse every 2 bytes |

The one-byte collision-box `type` array is unchanged. Collision metadata read
normally before the raw blocks—including `collisionScheme`, `curDiv`, `dx`,
`dy`, `minX`, `minY`, `numBoxes`, the model ID, and the total ID count—is also
unchanged because the original writer used ordinary `BinaryWriter.Write` for
those fields.

`Collision.ValidateCollisionIndices` checks every triangle corner against the
loaded vertex and normal counts before unsafe pointer access. Future damaged or
misunderstood collision assets should now identify their filename, polygon,
corner, and invalid indices in a managed exception.

#### Xbox navigation meshes

Each level configuration names a navigation mesh and deliberately appends the
`Xbox360` suffix. These files have an ordinary little-endian 32-bit byte-count
prefix, followed by a big-endian Detour-style `NAVM` object. The original
decompiled loader cast that object directly to native structs. On little-endian
x64 Windows the magic comparison silently failed, navigation remained
uninitialized, and route queries returned no path; zombies could spawn and
render but could not navigate.

The Windows loader now parses the Xbox payload field by field:

| Navigation data | Width | Conversion |
| --- | --- | --- |
| magic, version, and six element counts | big-endian 32-bit integers | Read explicitly as big-endian |
| cell size and min/max bounds | big-endian 32-bit floats | Read integer bits as big-endian, reinterpret as `float` |
| navigation vertices and detail vertices | big-endian 32-bit floats | Convert each component |
| polygon vertex and neighbour arrays | big-endian 16-bit unsigned integers | Convert each element |
| polygon vertex count and flags | bytes | Unchanged |
| BV-tree min/max bounds | big-endian 16-bit unsigned integers | Convert each element |
| BV-tree polygon index | big-endian 32-bit integer | Convert each element |
| detail-mesh bases and counts | big-endian 16-bit unsigned integers | Convert each element |
| detail triangle indices/flags | bytes | Unchanged |

The serialized header is always 84 bytes: 60 bytes of scalar data followed by
six four-byte Xbox pointer placeholders. It must not use
`sizeof(ReadNavMeshHeader)` on x64, where six eight-byte pointers change the
layout to 112 bytes. The loader validates the complete calculated payload size
before allocating or reading any arrays.

### Xbox hardware-thread affinity

The Xbox Compact Framework exposed `Thread.SetProcessorAffinity(int[])` for
pinning work to specific Xbox 360 hardware threads. Desktop .NET 8 does not
provide that instance API, and copying the numeric Xbox hardware-thread IDs to
Windows would not preserve their original meaning.

The following affinity hints are omitted while retaining the original worker
threads, events, and work loops:

| Code path | Original Xbox hardware-thread ID |
| --- | ---: |
| `Threads.Thread0_Main` | 1 |
| `Threads.Thread1_Main` | 3 |
| `Threads.Thread2_Main` | 4 |
| `Threads.Thread3_Main` | 5 |
| `Programs.Init_Programs` | 3 |
| `MainGame.SP_Initial_Setup` | 4 |

Windows and the .NET runtime now schedule those threads. This may change timing
and performance but should not change their intended work. The separate
`Thread.Abort()` shutdown incompatibility has not yet been converted.

### Effects and shaders

The original `Effect_Main.xnb` is an XNA 4 HiDef Xbox (`XNBx`, version 5)
asset compressed with LZX. The local `XnbEffectExtractor` performs only the XNB
container conversion:

1. validate the XNB header and platform;
2. decompress the framed LZX payload using FNA's existing decoder;
3. read the XNA type-reader table;
4. verify that the primary object uses `EffectReader`;
5. write the length-prefixed raw effect object as `Effect_Main.cso`.

The 10,318-byte XNB expands to a 70,584-byte Xbox Effects Framework object with
23 Xbox shader markers. FNA's desktop Effects Framework parser cannot execute
the embedded Xenos GPU microcode. Therefore:

- the extracted Xbox `Effect_Main.xnb` is excluded from the port output but is
  retained unchanged under `Extracted/`;
- `Effect_Main.compat.fx` supplies a source-visible desktop compatibility
  effect;
- its pixel shader applies the game's `Brightness`/`BrightnessAdj` controls and
  a 0.68 desktop scene-exposure factor. This compensates for the compatibility
  effect's simplified lighting while leaving SpriteBatch HUD and menu rendering
  at full brightness;
- fog-capable scene techniques interpolate camera-to-vertex distance and blend
  from clear rendering at 500 world units to dark blue-gray fog at 3,000 units.
  `F8` toggles the effect and `GlobalSettings.txt` persists `FogEnabled`; Basic,
  minimap, depth, shadow-map, and weapon-scope techniques remain fog-free;
- Windows `fxc.exe /T fx_2_0` compiles it to `Effect_Main.fxb`;
- FNA's raw-effect fallback finds `.fxb` when the game calls
  `Content.Load<Effect>("Effect_Main")` and no `.xnb` exists in the output.

The compatibility effect preserves all parameter names/types currently set by
the C# game and supplies these named techniques:

```text
Basic, BasicNonTextured, Billboards, ColorParticle, Instancing,
InstancingSetDepth, Main, Matrices, MatrixInstancing, MiniMap, Particles,
Particles_Animation, SetDepthBuffer, ShadowMap, ShadowMap_Matrix, Terrain,
TextureMove, WeaponScope
```

Most techniques currently share a basic Shader Model 3 vertex/pixel pass that
transforms by `World * ViewProjection`, samples `BaseTexture`, applies
`ColorAdjust`, `Emissive`, `AlphaAdjust`, `texAdj`, optional alpha clipping, and
for scene techniques, optional distance fog.

Rigged player, arm, and zombie meshes require a distinct conversion. Their
vertex declaration carries an unnormalized `Byte4` `BLENDINDICES0` at byte 48
and a `Vector4` `BLENDWEIGHT0` at byte 52. Before each draw the C# renderer
uploads up to 56 final skin matrices through `Matrix[]`. The desktop
`Matrices`, misspelled compatibility alias `Matrrices`, and
`ShadowMap_Matrix` techniques therefore:

1. cast the four 0..255 blend-index components to integer palette indices;
2. transform the bind-pose position by each selected matrix;
3. combine the results with the four blend weights; and
4. transform the skinned result by `ViewProjection`.

Version-zero rigged models contain one bone index but no explicit weight
buffers. When the four weights sum to zero, the compatibility shader treats
the vertex as weight 1 on its first bone. Without this skeletal shader path,
animated meshes remain in bind pose at their player/world origin: arms appear
in front of the camera and zombies appear attached to the player and lying
flat.

This remains an interface-compatible bootstrapping shader, not a complete
visual recreation. Lighting, normal/specular mapping, accurate shadow/depth
passes, particles, terrain, billboards, and instancing require individual
reconstruction from the 23 Xbox programs.
`Effect_Main.first-translated-shader.hlsl` records the first XenosRecomp result
for study; it is not loaded by the game.

### XNB textures and other content

Xbox-targeted texture XNBs are currently copied without offline conversion.
FNA recognizes the Xbox platform byte and its texture reader performs the
required Xbox texture-data handling at load time. Directory names and asset
names remain unchanged so the decompiled game's string paths still resolve.

This behavior differs from the effect path: FNA supports the relevant Xbox
texture representation, but not the Xbox shader microcode inside the effect.
Do not exclude or replace every Xbox XNB merely because `Effect_Main.xnb`
needed replacement.

### Native desktop backend

The native runtime mapping is:

| XNA-era responsibility | Windows port component |
| --- | --- |
| XNA graphics device and effect integration | FNA managed API plus `FNA3D.dll` |
| Xbox graphics hardware | SDL3 GPU/Vulkan backend selected by FNA3D |
| XACT/XAudio-compatible audio | `FAudio.dll` |
| Xbox controller input | SDL3 gamepad mapping through FNA |
| Theora video decoding | `libtheorafile.dll` |

Observed test hardware uses the Vulkan backend on an NVIDIA GeForce GTX 1070,
and SDL3 recognizes an Xbox One controller. Vulkan validation-layer warnings
about 3D-image layer ranges are backend diagnostics, not the managed crash that
was fixed in the collision loader.

### Implemented source-difference inventory

Compared with the unchanged `Decompiled/` tree, intentional source changes are
currently confined to:

- `Port/TheCoOpZombieGame.csproj`: .NET 8/FNA project and content/native-file
  deployment;
- `Port/FNA.NetStub.Core.csproj`: .NET 8 wrapper for the legacy service stubs;
- `Port/MainGame/MainGame.cs`: omit one Xbox affinity hint;
- `Port/Programs/Programs.cs`: omit one Xbox affinity hint;
- `Port/Threads/Threads.cs`: omit four Xbox affinity hints;
- `Port/Models/Models.cs`: targeted 32-bit/16-bit endian conversion and model
  index diagnostics;
- `Port/Collision/Collision.cs`: pre-pointer collision index validation;
- `Port/EGEngine/dtStatNavMesh.cs`: managed parsing of the big-endian,
  32-bit-layout Xbox navigation mesh;
- `Port/AI/AI.cs`: advance exhausted single-player waves and retain each
  wave's configured respawn interval;
- `Port/ContentSource/`: extracted shader evidence, compatibility HLSL, and the
  compiled Windows effect.

FNA and FNA.NetStub are kept as dependency source trees; game-specific changes
are made in `Port/`, not patched into FNA.

### Verification status

- Package extraction produced 897 files and preserved the original dump and
  executable hashes listed above.
- The reconstructed project builds successfully for .NET 8.
- FNA initializes SDL3, Vulkan, the NVIDIA GPU, and an Xbox One controller.
- A 15-second menu/main-loop smoke test completes without a managed exception.
- Starting a single-player match originally revealed the collision endian
  access violation described above. The collision conversion and pointer bounds
  checks are now implemented.
- The subsequent playtest no longer reproduced that collision access violation
  and reached the normal exit path. Exiting currently throws
  `PlatformNotSupportedException` at `Thread.Abort()`; this is a separate
  shutdown conversion that remains pending.
- Navigation now initializes with the expected Map 0 counts (1,846 polygons,
  3,908 vertices, and 7,039 detail triangles). Live playtesting confirmed that
  clearing the first five-enemy group starts wave 2/3 with ten enemies.

## Known incomplete work

- The compatibility shader is functional scaffolding, not a faithful rendering
  of the original lighting, shadows, particles, instancing, terrain, or scope
  effects.
- Additional custom binary systems may expose more selectively byte-reversed
  raw buffers as broader gameplay paths are exercised. Apply conversion only
  where the original writer used an explicit `*_Reversed` helper.
- `Thread.Abort()` remains in shutdown code and is unsupported on .NET 8. The
  latest playtest observed the resulting `PlatformNotSupportedException` on
  exit; replace it with cooperative cancellation and thread joins.
- Multiplayer services supplied by `FNA.NetStub` are stubs, not Xbox Live.
- Audio banks still need broader gameplay testing even though the native FAudio
  runtime is present.

## A productive learning order

Start with `Basic`, `BasicNonTextured`, and `Main` in the compatibility effect.
For each original Xbox shader program, record its constant registers, sampler
names, input semantics, and the techniques/passes that reference it. Translate
the program, rewrite descriptor-heap operations into XNA-style samplers, compile
with `fxc`, and test one technique at a time. Keep the unmodified `.cso` beside
the reconstruction so every inference remains auditable.

Only redistribute code and assets when you have the necessary rights. Owning a
personal dump is useful for private preservation and interoperability research,
but it does not by itself grant redistribution rights to the game or its assets.
