
sampler TextureSampler : register(s0);
//sampler MeshTextureSampler =   
//sampler_state  
//{  
//    Texture = <Texture>;  
//}; 

float4 NoEffect(float2 texCoord : TEXCOORD0) : COLOR0
{
    return float4(texCoord.x, texCoord.y, 0, 1);//tex2D(TextureSampler, texCoord.xy);
}

technique PassThrough
{
    pass Pass1
    {
		alphablendenable = true;

	    magfilter[0] = LINEAR;
		minfilter[0] = LINEAR;
		mipfilter[0] = LINEAR;

        PixelShader = compile ps_2_0 NoEffect();
    }
}