Shader "Custom/sh11_MyLambert"
{
    Properties
    {
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf MyLambert noambient


        sampler2D _MainTex;

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
        }

        float4 LightingMyLambert(SurfaceOutput s, float3 lightDir, float atten)
        {
            float d = dot(s.Normal, lightDir);
            d = saturate(d);
            //d = saturate(d) * 0.5 + 0.5;
            float4 lc = _LightColor0 * d * atten;
            
            return float4(s.Albedo * lc.rgb, s.Alpha*lc.a);
        }
        ENDCG
    }
    FallBack "Diffuse"
}
