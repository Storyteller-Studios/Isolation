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

float smoothstep_custom(float edge0, float edge1, float x)
{
    float t = saturate((x - edge0) / (edge1 - edge0));
    return t * t * (3.0 - 2.0 * t);
}

float4 main(float2 uv : TEXCOORD) : COLOR
{
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
    float3 layer1 = lerp(color1, color2, smoothstep_custom(-0.3, 0.2, mul(tuv, transpose(f_Rot(radians(-5.0)))).x));
    float3 layer2 = lerp(color3, color4, smoothstep_custom(-0.3, 0.2, mul(tuv, transpose(f_Rot(radians(-5.0)))).x));
    float3 finalComp = lerp(layer1, layer2, smoothstep_custom(0.5, -0.3, tuv.y));
    float3 hsv = rgb2hsv(finalComp);

    float2 p = -1.0 + 1.5 * uv.xy / iResolution.xy;
	float t = iTime / 5.;
    
	float x = p.x;
	float y = p.y;
    
	float mov0 = x+y+cos(sin(t)*2.0)*100.+sin(x/100.)*1000.;
	float mov1 = y / 0.3 + t;
	float mov2 = x / 0.2;
    
    float c1 = sin(mov1+t)/2.+mov2/2.-mov1-mov2+t;
    float c2 = cos(c1+sin(mov0/1000.+t)+sin(y/40.+t)+sin((x+y)/100.)*3.);
	float c3 = abs(sin(c2+cos(mov1+mov2+c2)+cos(mov2)+sin(x/1000.)));
    
    float3 col = hsv2rgb(float3(range(abs(c2), hsv.x * 0.95, hsv.x), range(c3, hsv.y, hsv.y * 0.85), range(c3, hsv.z, hsv.z * 0.85)));
    return float4(col, 1.0);
}