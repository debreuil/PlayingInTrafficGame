// Effect dynamically changes color saturation.

float satLevel;
sampler TextureSampler : register(s0);


float4 NoEffect(float4 color : COLOR0, float2 texCoord : TEXCOORD0) : COLOR0
{
    float4 tex = tex2D(TextureSampler, texCoord);    
    return tex;
}
float4 PixelShader1(float4 color : COLOR0, float2 texCoord : TEXCOORD0) : COLOR0
{
    // Look up the texture color.
    float4 tex = tex2D(TextureSampler, texCoord);
    
    // Convert it to greyscale. The constants 0.3, 0.59, and 0.11 are because
    // the human eye is more sensitive to green light, and less to blue.
    float greyscale = dot(tex.rgb, float3(0.3, 0.59, 0.11));
    
    // The input color alpha controls saturation level.
    tex.rgb = lerp(greyscale, tex.rgb, color.a * satLevel);
    
    return tex;
}


float4 PixelShader2(float4 color : COLOR0, float2 texCoord : TEXCOORD0) : COLOR0
{
    float4 tex = tex2D(TextureSampler, texCoord);
    float greyscale = dot(tex.rgb, float3(0.3, 0.59, 0.11));
    tex.rgb = lerp(greyscale, tex.rgb, color.a * .01);    
    return tex;
}
float4 PixelShader3(float4 color : COLOR0, float2 texCoord : TEXCOORD0) : COLOR0
{
    float4 tex = tex2D(TextureSampler, texCoord);
    tex.r = .8;    
    tex.g *= .5;    
    return tex;
}

technique Desaturate
{
    pass Pass1
    {
        PixelShader = compile ps_2_0 PixelShader1();
    }
}

technique Desaturate2
{
    pass NoEffect
    {
        PixelShader = compile ps_2_0 NoEffect();
    }
    pass Pass1
    {
        PixelShader = compile ps_2_0 PixelShader1();
    }
    pass Pass2
    {
        PixelShader = compile ps_2_0 PixelShader2();
    }
    pass Pass3
    {
        PixelShader = compile ps_2_0 PixelShader3();
    }
}
