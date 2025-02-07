Shader "Custom/SpriteShaderWithTransparentOutline"
{
    Properties
    {
        _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
        _OutlineColor ("Outline Color", Color) = (0,0,0,1)
        _OutlineThickness ("Outline Thickness", Range(0, 0.1)) = 0.02
    }

    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" "IgnoreProjector"="True" "PreviewType"="Plane" }
        LOD 100

        Cull Off
        Lighting Off
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        // Pass 1: Render the outline
        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _OutlineColor;
            float _OutlineThickness;

            v2f vert (appdata v)
            {
                v2f o;
                // Correct way to apply texture transformation in URP
                o.uv = v.uv * _MainTex_ST.xy + _MainTex_ST.zw;

                // Convert to clip space
                float4 clipPos = UnityObjectToClipPos(v.vertex);

                // Compute screen-space expansion direction
                float2 screenDir = normalize(mul(UNITY_MATRIX_MV, float4(v.vertex.xy, 0, 1)).xy);

                // Adjust outline expansion in screen space
                float2 screenOffset = screenDir * _OutlineThickness * clipPos.w; // Scale based on depth
                clipPos.xy += screenOffset;

                o.vertex = clipPos;
                return o;
            }

            half4 frag (v2f i) : SV_Target
            {
                return half4(_OutlineColor.rgb, _OutlineColor.a);
            }
            ENDHLSL
        }

        // Pass 2: Render the actual sprite
        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _Color;

            v2f vert (appdata v)
            {
                v2f o;
                // Correct UV transformation without TRANSFORM_TEX
                o.uv = v.uv * _MainTex_ST.xy + _MainTex_ST.zw;
                o.vertex = UnityObjectToClipPos(v.vertex);
                return o;
            }

            half4 frag (v2f i) : SV_Target
            {
                half4 texColor = tex2D(_MainTex, i.uv) * _Color;
                return texColor;
            }
            ENDHLSL
        }
    }
}
