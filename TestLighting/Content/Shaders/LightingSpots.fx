float3 AmbientColor = 0.35;

static const int MaxLights = 4;
float3 LightPositions[MaxLights];
float3 LightDirections[MaxLights];
float3 LightColors[MaxLights];
float LightRadii[MaxLights];
float LightOpacities[MaxLights];
float SpotAngles[MaxLights];
int LightCount;

float4x4 World;
float4x4 ViewProjection;

Texture2D ScreenTexture;
Texture2D NormalTexture;

SamplerState TextureSampler = sampler_state
{
    Texture = <ScreenTexture>;
};

SamplerState NormalSampler = sampler_state
{
    Texture = <NormalTexture>;
};

struct VertexShaderInput
{
    float4 Position : POSITION0;
    float2 TexCoords : TEXCOORD0;
    float4 Color : COLOR0;
};

struct VertexShaderOutput
{
    float4 Position : POSITION0;
    float4 PosWorld : POSITION1;
    float2 TexCoords : TEXCOORD0;
    float4 Color : COLOR0;
};

VertexShaderOutput VS(VertexShaderInput input)
{
    VertexShaderOutput output;

    float4 pos = mul(input.Position, World);
    output.PosWorld = pos;
    output.Position = mul(pos, ViewProjection);

    output.TexCoords = input.TexCoords;
    output.Color = input.Color;

    return output;
}

float4 PS(VertexShaderOutput input) : COLOR0
{
    float3 finalLighting = AmbientColor;

    float4 tex = ScreenTexture.Sample(TextureSampler, input.TexCoords);
    float3 normal = normalize((2 * NormalTexture.Sample(NormalSampler, input.TexCoords)) - 1);
    normal = normalize(normal);
    normal.y *= -1;

    for (int i = 0; i < LightCount; i++)
    {
        float3 lightDir = LightPositions[i] - input.PosWorld.xyz;

        float distanceSquared = dot(lightDir, lightDir);

        lightDir = normalize(lightDir);

        float spotEffect = dot(-lightDir, normalize(LightDirections[i]));
        
        if (spotEffect > cos(SpotAngles[i]))
        {
            float attenuation = saturate(1.0 - distanceSquared / (LightRadii[i] * LightRadii[i]));
            float diffuseLighting = saturate(dot(normal, lightDir)) * attenuation;

            diffuseLighting *= pow(spotEffect, 2.0);

            finalLighting += LightColors[i] * diffuseLighting * LightOpacities[i];
        }
    }

    input.Color.rgb *= finalLighting;

    return input.Color * tex;
}

technique SpotLightNormalMap
{
    pass Pass1
    {
        VertexShader = compile vs_4_0_level_9_3 VS();
        PixelShader = compile ps_4_0_level_9_3 PS();
    }
}
