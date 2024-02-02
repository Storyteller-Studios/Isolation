#define D2D_INPUT_COUNT 1
#define D2D_INPUT0_COMPLEX
#include "d2d1effecthelpers.hlsli"

float _iTime = 0.5;
float2 _iScale = float2(0.5,0.5);
float _iPower = 3;

float f_random(in float2 _st)
{
    return frac((sin(dot(_st.xy, float2(12.9898, 78.233002))) * 43758.547));
}

float f_noise(in float2 _st)
{
    float2 i = floor(_st);
    float2 f = frac(_st);
    float a = f_random(i);
    float b = f_random((i + float2(1.0, 0.0)));
    float c = f_random((i + float2(0.0, 1.0)));
    float d = f_random((i + float2(1.0, 1.0)));
    float2 u = ((f * f) * (3.0 - (2.0 * f)));
    return ((lerp(a, b, u.x) + (((c - a) * u.y) * (1.0 - u.x))) + (((d - b) * u.x) * u.y));
}

float4 draw_image(in float2 uv)
{
    float speed = 0.1;
    float power = _iPower;
    float nXAmp = (((sin((_iTime * speed)) + 1.0) * 0.5) * power);
    float nX = f_noise((uv * nXAmp));
    float nYAmp = (((cos((_iTime * speed)) + 1.0) * 0.5) * power);
    float nY = f_noise((uv * nYAmp));
    float2 uv2 = float2(nX, nY) + uv;
    
    float2 mirroredUV = abs(frac(uv2 - 0.5) - 0.5) * 2;

    return D2DSampleInput(0, mirroredUV);
}

D2D_PS_ENTRY(main)
{
    float2 uv = D2DGetInputCoordinate(0).xy;
	float2 scale = float2(_iScale.x * 3, _iScale.y * 2.1781 + 0.0389);
	if(scale.x < 0.1) scale.x = 0.1;
	return draw_image(uv / scale);
}