#define D2D_REQUIRES_SCENE_POSITION
#define D2D_INPUT_COUNT 0
#include "d2d1effecthelpers.hlsli"

float3 iResolution = float3(1,1,1);
float iTime = 0.0;
float3 color1 = float3(0.957,0.804,0.623);
float3 color2 = float3(0.192,0.384,0.933);
float3 color3 = float3(0.910,0.510,0.8);
float3 color4 = float3(0.350,0.71,0.953);
float Width = 800;
float Height = 800;
float RandomValue1 = 0;
float RandomValue2 = 0;
float RandomValue3 = 0;
bool UseHSVBlending = false;
//Shader Utilities
float2x2 f_Rot(in float _a)
{
    float _s = sin(_a);
    float _c = cos(_a);
    return float2x2(_c, (-_s), _s, _c);
}
float2 f_hash(in float2 _p)
{
    (_p = float2(dot(_p, float2(2127.1, 81.17)), dot(_p, float2(1269.5, 283.37))));
    return frac((sin(_p) * 43758.5453));
}
float f_noise(in float2 _p)
{
    float2 _i = floor(_p);
    float2 _f = frac(_p);
    float2 _u = ((_f * _f) * (3.0 - (2.0 * _f)));
    float _n = lerp(lerp(dot((-1.0 + (2.0 * f_hash((_i + float2(0.0, 0.0))))), (_f - float2(0.0, 0.0))), dot((-1.0 + (2.0 * f_hash((_i + float2(1.0, 0.0))))), (_f - float2(1.0, 0.0))), _u.x), lerp(dot((-1.0 + (2.0 * f_hash((_i + float2(0.0, 1.0))))), (_f - float2(0.0, 1.0))), dot((-1.0 + (2.0 * f_hash((_i + float2(1.0, 1.0))))), (_f - float2(1.0, 1.0))), _u.x), _u.y);
    return (0.5 + (0.5 * _n));
}
float range(float val, float mi, float ma)
{
    return val * (ma - mi) + mi;
}
// Color Utilities
float3 hsv2rgb(float3 c)
{
    float4 K = float4(1.0, 2.0 / 3.0, 1.0 / 3.0, 3.0);
 
    float3 p = abs(frac(c.xxx + K.xyz) * 6.0 - K.www);
    return c.z * lerp(K.xxx, saturate(p - K.xxx), c.y);
}
float3 rgb2hsv(float3 c)
{
    float4 K = float4(0.0, -1.0 / 3.0, 2.0 / 3.0, -1.0);
    float4 p = lerp(float4(c.bg, K.wz), float4(c.gb, K.xy), step(c.b, c.g));
    float4 q = lerp(float4(p.xyw, c.r), float4(c.r, p.yzx), step(p.x, c.r));

    float d = q.x - min(q.w, q.y);
    float e = 1.0e-10;
    return float3(abs(q.z + (q.w - q.y) / (6.0 * d + e)), d / (q.x + e), q.x);
}

D2D_PS_ENTRY(main)
{ 
    float2 uv = float2(D2DGetScenePosition().x / Width, D2DGetScenePosition().y / Height);
    float ratio = iResolution.x / iResolution.y;
    float2 tuv = uv;
    tuv -= 0.5;
    float degree = f_noise(float2((iTime * 0.1), (tuv.x * tuv.y)));
    tuv.y *= (1.0 / ratio);
    tuv = mul(tuv, transpose(f_Rot(radians((((degree - 0.5) * 720.0) + 180.0)))));
    tuv.y *= ratio;
    float frequency = 5.0;
    float amplitude = 25.0;
    float speed = (iTime * 0.75);
    tuv.x += (sin(((tuv.y * frequency) + speed)) / amplitude);
    tuv.y += (sin((((tuv.x * frequency) * 1.5) + speed)) / (amplitude * 0.5));
    float3 c1,c2,c3,c4;
    if(UseHSVBlending)
    {
        c1 = rgb2hsv(color1);
        c2 = rgb2hsv(color2);
        c3 = rgb2hsv(color3);
        c4 = rgb2hsv(color4);
    }
    else
    {
        c1 = color1;
        c2 = color2;
        c3 = color3;
        c4 = color4;
    }
    float3 layer1 = lerp(c1, c2, smoothstep(-0.3, 0.2, mul(tuv, transpose(f_Rot(radians(-5.0)))).x));
    float3 layer2 = lerp(c3, c4, smoothstep(-0.3, 0.2, mul(tuv, transpose(f_Rot(radians(-5.0)))).x));
    float3 finalComp = lerp(layer1, layer2, smoothstep(0.5, -0.3, tuv.y));
    if(UseHSVBlending)
    {
        return float4(hsv2rgb(finalComp), 1.0);
    }
    else
    {
        return float4(finalComp, 1.0);
    }
}