Shader "UI/FrontOnly"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags {"Queue" = "Overlay"}
        Cull Back  // Hide back side
        Pass
        {
            SetTexture [_MainTex] { combine texture }
        }
    }
}
