Shader"Unlit/TextureScroll"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _ScrollSpeed ("Speed", float) = 1.
        _Color ("Color", Color) = (1.,1.,1.,1.)

        _HSLRangeMin ("HSL Affect Range Min", Range(0, 1)) = 0
        _HSLRangeMax ("HSL Affect Range Max", Range(0, 1)) = 1
        _Saturation ("Saturation", Vector) = (0, 0, 0, 0)
        _StencilComp ("Stencil Comparison", Float) = 8
        _Stencil ("Stencil ID", Float) = 0
        _StencilOp ("Stencil Operation", Float) = 0
        _StencilWriteMask ("Stencil Write Mask", Float) = 255
        _StencilReadMask ("Stencil Read Mask", Float) = 255
        _ColorMask ("Color Mask", Float) = 15
    }
    SubShader
    {
        Tags
        {
            "RenderType" = "Transparent"
            "Queue" = "Transparent"
        }

        Stencil
        {
            Ref [_Stencil]
            Comp [_StencilComp]
            Pass [_StencilOp]
            ReadMask [_StencilReadMask]
            WriteMask [_StencilWriteMask]
        }
        ColorMask [_ColorMask]

        Pass
        {
            Cull Off
            ZWrite Off
            Blend SrcAlpha OneMinusSrcAlpha

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fog
            #pragma multi_compile DUMMY PIXELSNAP_ON

            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _Color;
            float _HSLRangeMin;
            float _HSLRangeMax;
            float4 _Saturation;
            float4 _MainTex_ST;
            float _ScrollSpeed;

            struct Vertex
            {
                float4 vertex : POSITION;
                float2 uv_MainTex : TEXCOORD0;
            };

            struct Fragment
            {
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
                float2 uv_MainTex : TEXCOORD0;
            };

            Fragment vert (Vertex v)
            {
                Fragment o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv_MainTex = TRANSFORM_TEX(v.uv_MainTex, _MainTex);

                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            float Epsilon = 1e-10;

            float3 rgb2hcv(in float3 RGB)
            {
                // Based on work by Sam Hocevar and Emil Persson
                float4 P = lerp(float4(RGB.bg, -1.0, 2.0/3.0), float4(RGB.gb, 0.0, -1.0/3.0), step(RGB.b, RGB.g));
                float4 Q = lerp(float4(P.xyw, RGB.r), float4(RGB.r, P.yzx), step(P.x, RGB.r));
                float C = Q.x - min(Q.w, Q.y);
                float H = abs((Q.w - Q.y) / (6 * C + Epsilon) + Q.z);
                return float3(H, C, Q.x);
            }

            float3 rgb2hsl(in float3 RGB)
            {
                float3 HCV = rgb2hcv(RGB);
                float L = HCV.z - HCV.y * 0.5;
                float S = HCV.y / (1 - abs(L * 2 - 1) + Epsilon);
                return float3(HCV.x, S, L);
            }

            float3 hsl2rgb(float3 c)
            {
                c = float3(frac(c.x), clamp(c.yz, 0.0, 1.0));
                float3 rgb = clamp(abs(fmod(c.x * 6.0 + float3(0.0, 4.0, 2.0), 6.0) - 3.0) - 1.0, 0.0, 1.0);
                return c.z + c.y * (rgb - 0.5) * (1.0 - abs(2.0 * c.z - 1.0));
            }

            float4 frag(Fragment IN) : COLOR
            {
                IN.uv_MainTex.x = IN.uv_MainTex.x + _SinTime * _ScrollSpeed;
                IN.uv_MainTex.y = IN.uv_MainTex.y + _Time * _ScrollSpeed;

                float4 color = _Color.rgba * tex2D (_MainTex, IN.uv_MainTex);

                UNITY_APPLY_FOG(IN.fogCoord, color);
                
                float3 hsl = rgb2hsl(color.rgb);
                float affectMult = step(_HSLRangeMin, hsl.r) * step(hsl.r, _HSLRangeMax);
                
                float3 rgb = hsl2rgb(hsl + _Saturation.xyz * affectMult);
                return float4(rgb, color.a + _Saturation.a);
            }

            ENDCG
        }
    }
}
