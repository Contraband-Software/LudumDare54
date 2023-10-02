#if OPENGL
#define VS_SHADERMODEL vs_3_0
#define PS_SHADERMODEL ps_3_0
#else
#define VS_SHADERMODEL vs_4_0
#define PS_SHADERMODEL ps_4_0
#endif


float4x4 viewProjection;
float time;
float width;
float height;
//float strength;
sampler colorSampler : register(s0);

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
    float2 dist = abs(p.TexCoord.xy - 0.5);
    float2 warpedUV = float2(0.1 + p.TexCoord.x / dist.x, 0.1 + p.TexCoord.y / dist.y);
    float4 col = float4(1, 1, 1, 1);
    if (warpedUV.x > 1 || warpedUV.x < 0 || warpedUV.y > 1 || warpedUV.y < 0)
    {
        col = tex2D(colorSampler, warpedUV);
    }

    return col;
}

technique SpriteBatch
{
    pass
    {
        VertexShader = compile VS_SHADERMODEL SpriteVertexShader();
        PixelShader = compile PS_SHADERMODEL SpritePixelShader();
    }
}
