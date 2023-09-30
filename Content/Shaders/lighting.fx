#if OPENGL
#define VS_SHADERMODEL vs_3_0
#define PS_SHADERMODEL ps_3_0
#else
#define VS_SHADERMODEL vs_4_0
#define PS_SHADERMODEL ps_4_0
#endif

struct Light
{
    float3 Color;
    float2 Position;
};

float2 translation;
float4x4 viewProjection;
float time;
float width;
float height;
float3 lightColors[64];
float2 lightPositions[64];
sampler colorSampler : register(s0);
sampler normalSampler : register(s1);

struct VertexInput
{
    float4 Position : POSITION0;
    float4 Color : COLOR0;
    float4 TexCoord : TEXCOORD0;
};
struct PixelInput
{
    float4 Position : SV_Position0;
    float4 Color : COLOR0;
    float4 TexCoord : TEXCOORD0;
};


PixelInput SpriteVertexShader(VertexInput v)
{
    PixelInput output;

    output.Position = mul(v.Position, viewProjection);
    output.Color = v.Color;
    output.TexCoord = v.TexCoord;
    
    return output;
}
float4 SpritePixelShader(PixelInput p) : SV_TARGET
{
    float4 diffuse = tex2D(colorSampler, p.TexCoord.xy);
    float4 normal = tex2D(normalSampler, p.TexCoord.xy)*2 - 1;
    float3 lighting = float3(0, 0, 0);
    float2 screenCords = float2(p.Position.x, height - p.Position.y);
    for (int i = 0; i < 64; i++)
    {
        float2 translatedPos = lightPositions[i] + translation;
        float2 distAxis = screenCords - translatedPos;
        float distSquared = distAxis.x * distAxis.x + distAxis.y * distAxis.y;
        lighting += (lightColors[i] / distSquared) * max(dot(normal.xyz, normalize(float3(distAxis.x, distAxis.y, 10))), 0); // not technically correct

    }
    return diffuse * float4(lighting, 1.0);
    //return float4(p.Position.x/width, p.Position.y/height, 0.0, 1.0);

}

technique SpriteBatch
{
    pass
    {
        VertexShader = compile VS_SHADERMODEL SpriteVertexShader();
        PixelShader = compile PS_SHADERMODEL SpritePixelShader();
    }
}
