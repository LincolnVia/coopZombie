// Desktop compatibility baseline for the Xbox 360 Effect_Main asset.
//
// This intentionally preserves the parameter and technique interface used by
// the decompiled game while the original 23 Xbox shader programs are rebuilt
// one at a time. The first pass provides basic transformed, textured drawing.

float4x4 World = 1;
float4x4 View = 1;
float4x4 Projection = 1;
float4x4 ViewProjection = 1;
float4x4 ShadowProjection = 1;
float4x4 ShadowMainProjection = 1;
float4x4 Matrix[56];
float4x4 InstanceMatrix = 1;

texture BaseTexture;
texture MaterialTexture;
texture MaterialTexture2;
texture MaterialTexture3;
texture SpecularTexture;
texture ShadowMapTexture;
texture ShadowMapMainTexture;
texture Texture;

bool clipTexture = false;
bool FogEnabled = false;
int Specular = 0;
int numAmmoLights = 0;
int numLevelLights = 0;
int VertexCount = 0;
int VertexStart = 0;

float AlphaAdjust = 1.0;
float AmmoLightAdjust = 0.0;
float Brightness = 1.0;
float BrightnessAdj = 0.0;
// The compatibility shader does not yet reproduce the Xbox lighting pipeline,
// so unlit texture samples otherwise appear much brighter than the original.
// Keep this below 1.0 while preserving the game's brightness slider range.
static const float DesktopSceneExposure = 0.68;
float CurrentTime = 0.0;
float depth = 0.0;
float Duration = 1.0;
float DurationRandomness = 0.0;
float EndVelocity = 1.0;
float frameCount = 1.0;
float frameCountInverse = 1.0;
float height = 1.0;
float LaserLightDistance0 = 0.0;
float offset = 0.0;
float PtLightDistance0 = 8192.0;
float TextureMultiplier = 1.0;
float FogStart = 500.0;
float FogEnd = 3000.0;
float width = 1.0;

float2 EndSize = float2(1.0, 1.0);
float2 RotateSpeed = float2(0.0, 0.0);
float2 StartSize = float2(1.0, 1.0);
float2 texAdj = float2(0.0, 0.0);
float2 ViewportScale = float2(1.0, 1.0);

float3 cameraDir = float3(0.0, 0.0, 1.0);
float3 FogCameraPosition = float3(0.0, 0.0, 0.0);
float3 dLightDir = float3(0.0, 0.0, -1.0);
float3 Gravity = float3(0.0, 0.0, 0.0);
float3 LaserLight0 = float3(0.0, 0.0, 0.0);
float3 LaserLightDirection0 = float3(0.0, 0.0, 1.0);
float3 PtLight0 = float3(0.0, 0.0, 0.0);
float3 PtLightDirection0 = float3(0.0, 0.0, 1.0);

float4 Ambient = float4(1.0, 1.0, 1.0, 1.0);
float4 ColorAdjust = float4(1.0, 1.0, 1.0, 1.0);
float4 dLightBounce = float4(0.0, 0.0, 0.0, 0.0);
float4 dLightColor = float4(1.0, 1.0, 1.0, 1.0);
float4 Emissive = float4(0.0, 0.0, 0.0, 0.0);
float4 FogColor = float4(0.055, 0.070, 0.080, 1.0);
float4 InstanceColor = float4(1.0, 1.0, 1.0, 1.0);
float4 LaserLightColor0 = float4(0.0, 0.0, 0.0, 0.0);
float4 MaxColor = float4(1.0, 1.0, 1.0, 1.0);
float4 MinColor = float4(1.0, 1.0, 1.0, 1.0);
float4 PtLightColor0 = float4(1.0, 1.0, 1.0, 1.0);
float4 PtLightColor1 = float4(1.0, 1.0, 1.0, 1.0);
float4 PtLightColor2 = float4(1.0, 1.0, 1.0, 1.0);
float4 PtLightColor3 = float4(1.0, 1.0, 1.0, 1.0);
float4 AmmoLight[2];
float4 AmmoLightColor[2];
float4 InstancePosition[20];
float4 InstanceScale[20];
float4 PtLight_Lvl[2];
float4 PtLightColor_Lvl[2];

sampler2D BaseSampler = sampler_state
{
    Texture = <BaseTexture>;
    MinFilter = Linear;
    MagFilter = Linear;
    MipFilter = Linear;
    AddressU = Wrap;
    AddressV = Wrap;
};

struct CompatVertexInput
{
    float4 Position : POSITION0;
    float3 Normal : NORMAL0;
    float3 Tangent : TANGENT0;
    float4 Color : COLOR0;
    float2 TexCoord : TEXCOORD0;
    float4 BlendIndices : BLENDINDICES0;
    float4 BlendWeights : BLENDWEIGHT0;
};

struct CompatVertexOutput
{
    float4 Position : POSITION0;
    float4 Color : COLOR0;
    float2 TexCoord : TEXCOORD0;
    float FogDepth : TEXCOORD1;
};

CompatVertexOutput CompatVS(CompatVertexInput input)
{
    CompatVertexOutput output;
    float4 worldPosition = mul(input.Position, World);
    output.Position = mul(worldPosition, ViewProjection);
    output.Color = input.Color * ColorAdjust;
    output.TexCoord = input.TexCoord + texAdj;
    output.FogDepth = distance(worldPosition.xyz, FogCameraPosition);
    return output;
}

// Player and zombie models are stored in bind pose. The game uploads a
// 56-matrix skinning palette before drawing them with the Matrices technique.
// Byte4 blend indices arrive as unnormalised 0..255 values.
CompatVertexOutput CompatSkinnedVS(CompatVertexInput input)
{
    CompatVertexOutput output;
    int4 indices = (int4)input.BlendIndices;
    float4 weights = input.BlendWeights;

    // Version-zero models contain one bone index but no explicit weights.
    // Treat that representation as a rigid one-bone vertex.
    if (dot(weights, float4(1.0, 1.0, 1.0, 1.0)) < 0.0001)
    {
        weights = float4(1.0, 0.0, 0.0, 0.0);
    }

    float4 skinnedPosition =
        mul(input.Position, Matrix[indices.x]) * weights.x +
        mul(input.Position, Matrix[indices.y]) * weights.y +
        mul(input.Position, Matrix[indices.z]) * weights.z +
        mul(input.Position, Matrix[indices.w]) * weights.w;

    output.Position = mul(skinnedPosition, ViewProjection);
    output.Color = input.Color * ColorAdjust;
    output.TexCoord = input.TexCoord + texAdj;
    output.FogDepth = distance(skinnedPosition.xyz, FogCameraPosition);
    return output;
}

float4 CompatPS(CompatVertexOutput input) : COLOR0
{
    float4 color = tex2D(BaseSampler, input.TexCoord) * input.Color;
    color.rgb = saturate(color.rgb * DesktopSceneExposure * Brightness + BrightnessAdj);
    color.rgb += Emissive.rgb;
    color.a *= AlphaAdjust;
    if (clipTexture)
    {
        clip(color.a - 0.01);
    }
    return color;
}

float4 CompatFogPS(CompatVertexOutput input) : COLOR0
{
    float4 color = CompatPS(input);
    if (FogEnabled)
    {
        float fogRange = max(FogEnd - FogStart, 0.001);
        float fogAmount = saturate((input.FogDepth - FogStart) / fogRange);
        color.rgb = lerp(color.rgb, FogColor.rgb, fogAmount);
    }
    return color;
}

#define COMPAT_TECHNIQUE(name) \
    technique name \
    { \
        pass P0 \
        { \
            VertexShader = compile vs_3_0 CompatVS(); \
            PixelShader = compile ps_3_0 CompatPS(); \
        } \
    }

#define COMPAT_SKINNED_TECHNIQUE(name) \
    technique name \
    { \
        pass P0 \
        { \
            VertexShader = compile vs_3_0 CompatSkinnedVS(); \
            PixelShader = compile ps_3_0 CompatPS(); \
        } \
    }

#define COMPAT_FOG_TECHNIQUE(name) \
    technique name \
    { \
        pass P0 \
        { \
            VertexShader = compile vs_3_0 CompatVS(); \
            PixelShader = compile ps_3_0 CompatFogPS(); \
        } \
    }

#define COMPAT_FOG_SKINNED_TECHNIQUE(name) \
    technique name \
    { \
        pass P0 \
        { \
            VertexShader = compile vs_3_0 CompatSkinnedVS(); \
            PixelShader = compile ps_3_0 CompatFogPS(); \
        } \
    }

COMPAT_TECHNIQUE(Basic)
COMPAT_TECHNIQUE(BasicNonTextured)
COMPAT_FOG_TECHNIQUE(Billboards)
COMPAT_FOG_TECHNIQUE(ColorParticle)
COMPAT_FOG_TECHNIQUE(Instancing)
COMPAT_TECHNIQUE(InstancingSetDepth)
COMPAT_FOG_TECHNIQUE(Main)
COMPAT_SKINNED_TECHNIQUE(Matrrices)
COMPAT_FOG_SKINNED_TECHNIQUE(Matrices)
COMPAT_FOG_TECHNIQUE(MatrixInstancing)
COMPAT_TECHNIQUE(MiniMap)
COMPAT_FOG_TECHNIQUE(Particles)
COMPAT_FOG_TECHNIQUE(Particles_Animation)
COMPAT_TECHNIQUE(SetDepthBuffer)
COMPAT_TECHNIQUE(ShadowMap)
COMPAT_SKINNED_TECHNIQUE(ShadowMap_Matrix)
COMPAT_FOG_TECHNIQUE(Terrain)
COMPAT_FOG_TECHNIQUE(TextureMove)
COMPAT_TECHNIQUE(WeaponScope)
