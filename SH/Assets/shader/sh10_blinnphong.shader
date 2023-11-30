Shader "Custom/sh10_blinnphong"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _SpecColor ("Specualr Color", Color) = (1,1,1,1)
        
        _Gloss ("Gloss", Range(-2, 2)) = 1
        _Specular("Specualr", Range(-2, 2)) = 0.5
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf BlinnPhong

        sampler2D _MainTex;
        float _Gloss;
        float _Specular;

        struct Input
        {
            float2 uv_MainTex;
        };

        void surf (Input IN, inout SurfaceOutput o)
        {
            // Albedo comes from a texture tinted by color
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex);
            o.Albedo = c.rgb;
            o.Alpha = c.a;

            o.Gloss = _Gloss;
            o.Specular = _Specular;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
