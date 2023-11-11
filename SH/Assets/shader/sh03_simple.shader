Shader "Custom/sh03_simple"
{
    Properties
    {
        _R("red", Range(0,1)) = 1
        _G("green", Range(0,1)) = 1
        _B("blue", Range(0,1)) = 0
        _M("Brightness", Range(0,1)) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows

        float _R;
        float _G;
        float _B;
        float _M;

        struct Input
        {
            float2 uv_MainTex;
        };

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            o.Albedo = float3(_R, _G, _B) * _M;
            o.Alpha = 1;
        }
        ENDCG
    }
}
