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

float hash(float p)
{
    p = frac(p * .1031);
    p *= p + 33.33;
    p *= p + p;
    return frac(p);
}
float hash12(float2 p)
{
    float3 p3 = frac(float3(p.xyx) * .1031);
    p3 += dot(p3, p3.yzx + 33.33);
    return frac((p3.x + p3.y) * p3.z);
}

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
    float4 col = float4(0, 0, 0, 0);
    if (pow(p.TexCoord.x - 0.5, 2) + pow(p.TexCoord.y - 0.5, 2) < 0.01 + tex2D(colorSampler, p.TexCoord.xy*0.2 + float2((time * 0.2) % 0.25, (time * 0.2) % 0.25)).r * 0.015)
    {
        col = float4(0, 0, 0, 1);

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
