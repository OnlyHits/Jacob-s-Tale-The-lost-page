Shader "Custom/PageFlipShaderURP"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _FlipAmount ("Flip Progress", Range(0, 1)) = 0.0
        _BendStrength ("Bend Strength", Range(0, 5)) = 2.0
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct appdata_t
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
            float _FlipAmount;
            float _BendStrength;

            v2f vert(appdata_t IN)
            {
                v2f OUT;

                // Simulating page bending
                float bend = sin(IN.uv.x * _FlipAmount * 3.1415) * _BendStrength;
                IN.vertex.y += bend;

                OUT.vertex = TransformObjectToHClip(IN.vertex);
                OUT.uv = IN.uv;

                return OUT;
            }

            half4 frag(v2f IN) : SV_Target
            {
                // Sample texture
                half4 color = tex2D(_MainTex, IN.uv);

                // Fade out the back part of the page
                color.a *= smoothstep(1.0, 0.7, IN.uv.x);

                return color;
            }
            ENDHLSL
        }
    }
}
