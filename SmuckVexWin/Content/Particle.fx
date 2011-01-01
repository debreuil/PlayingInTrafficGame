
sampler TextureSampler : register(s0);


float4 NoEffect(float4 color : COLOR0, float2 texCoord : TEXCOORD0) : COLOR0
{
    return color;
}

technique PassThrough
{
    pass Pass1
    {
        PixelShader = compile ps_2_0 NoEffect();
    }
}