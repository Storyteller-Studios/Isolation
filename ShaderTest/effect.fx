sampler2D input : register(s0);

/// <summary>iResolution</summary>
/// <minValue>0,0,0/minValue>
/// <maxValue>1,1,1</maxValue>
/// <defaultValue>1,1,1</defaultValue>
float3 iResolution : register(c0);

/// <summary>iTime</summary>
/// <minValue>0/minValue>
/// <maxValue>1000</maxValue>
/// <defaultValue>0</defaultValue>
float iTime : register(c1);

/// <summary>color1</summary>
/// <minValue>0,0,0/minValue>
/// <maxValue>1,1,1</maxValue>
/// <defaultValue>.957, .804, .623</defaultValue>
float3 color1 : register(c2);

/// <summary>color2</summary>
/// <minValue>0,0,0/minValue>
/// <maxValue>1,1,1</maxValue>
/// <defaultValue>.192, .384, .933</defaultValue>
float3 color2 : register(c3);

/// <summary>color3</summary>
/// <minValue>0,0,0/minValue>
/// <maxValue>1,1,1</maxValue>
/// <defaultValue>.910, .510, .8</defaultValue>
float3 color3 : register(c4);

/// <summary>color4</summary>
/// <minValue>0,0,0/minValue>
/// <maxValue>1,1,1</maxValue>
/// <defaultValue>0.350, .71, .953</defaultValue>
float3 color4 : register(c5);

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


float4 main(float2 uv : TEXCOORD) : COLOR
{
    float2 _uv5678 = uv;
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
    float3 _col5692 = _finalComp5691;
    return float4(_col5692, 1.0);
}
