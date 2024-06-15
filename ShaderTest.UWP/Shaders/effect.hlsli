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

float2x2 f_Rot(in float _a)
{
    float _s = sin(_a);
    float _c = cos(_a);
    return float2x2(_c, (-_s), _s, _c);
}

float2 f_hash(in float2 _p)
{
    (_p = float2(dot(_p, float2(2127.1001, 81.169998)), dot(_p, float2(1269.5, 283.37))));
    return frac((sin(_p) * 43758.547));
}

float f_noise(in float2 _p)
{
    float2 _i = floor(_p);
    float2 _f = frac(_p);
    float2 _u = ((_f * _f) * (3.0 - (2.0 * _f)));
    float _n = lerp(lerp(dot((-1.0 + (2.0 * f_hash((_i + float2(0.0, 0.0))))), (_f - float2(0.0, 0.0))), dot((-1.0 + (2.0 * f_hash((_i + float2(1.0, 0.0))))), (_f - float2(1.0, 0.0))), _u.x), lerp(dot((-1.0 + (2.0 * f_hash((_i + float2(0.0, 1.0))))), (_f - float2(0.0, 1.0))), dot((-1.0 + (2.0 * f_hash((_i + float2(1.0, 1.0))))), (_f - float2(1.0, 1.0))), _u.x), _u.y);
    return (0.5 + (0.5 * _n));
}

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

float range(float val, float mi, float ma)
{
    return val * (ma - mi) + mi;
}

D2D_PS_ENTRY(main)
{ 
    float2 _uv5678 = float2(D2DGetScenePosition().x / Width, D2DGetScenePosition().y / Height);
    float _ratio5679 = iResolution.x / iResolution.y;
    float2 _tuv5680 = _uv5678;
    (_tuv5680 -= 0.5);
    float _degree5681 = f_noise(float2((iTime * 0.1), (_tuv5680.x * _tuv5680.y)));
    (_tuv5680.y *= (1.0 / _ratio5679));
    (_tuv5680 = mul(_tuv5680, transpose(f_Rot(radians((((_degree5681 - 0.5) * 720.0) + 180.0))))));
    (_tuv5680.y *= _ratio5679);
    float _frequency5682 = { 5.0 };
    float _amplitude5683 = { 30.0 };
    float _speed5684 = (iTime * 2.0);
    (_tuv5680.x += (sin(((_tuv5680.y * _frequency5682) + _speed5684)) / _amplitude5683));
    (_tuv5680.y += (sin((((_tuv5680.x * _frequency5682) * 1.5) + _speed5684)) / (_amplitude5683 * 0.5)));
    float3 _layer15687 = lerp(color1, color2, smoothstep(-0.30000001, 0.2, mul(_tuv5680, transpose(f_Rot(-0.08726646))).x));
    float3 _layer25690 = lerp(color3, color4, smoothstep(-0.30000001, 0.2, mul(_tuv5680, transpose(f_Rot(-0.08726646))).x));
    float3 _finalComp5691 = lerp(_layer15687, _layer25690, smoothstep(0.5, -0.30000001, _tuv5680.y));
    float3 hsv = rgb2hsv(_finalComp5691);

    float2 p = -1.0 + 1.5 * _uv5678.xy / iResolution.xy;
	float t = iTime / 5.;
    
	float x = p.x;
	float y = p.y;
    
	float mov0 = x+y+cos(sin(t)*2.0)*100.+sin(x/100.)*1000.;
	float mov1 = y / 0.3 +  t;
	float mov2 = x / 0.2;
    
    float c1 = sin(mov1+t)/2.+mov2/2.-mov1-mov2+t;
    float c2 = cos(c1+sin(mov0/1000.+t)+sin(y/40.+t)+sin((x+y)/100.)*3.);
	float c3 = abs(sin(c2+cos(mov1+mov2+c2)+cos(mov2)+sin(x/1000.)));
    
    float3 col = hsv2rgb(float3(range(c2 , hsv.x,  hsv.x * 0.95) , range(c3 , hsv.y, hsv.y * 0.85), range(c3 , hsv.z,  hsv.z * 0.85)));
    return float4(col, 1.0);
}