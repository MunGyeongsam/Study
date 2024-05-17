Shader "Custom/sh12_MyBlinnPhong"
{
    Properties
    {
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        
        _SpecColor ("Specualr Color", Color) = (1,1,1,1)
        
        _SpecPow("Specualr Power", Range(0, 128)) = 60
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf MyBlinnPhong noambient

        sampler2D _MainTex;
        float3 _SpecPow;

        struct Input
        {
            float2 uv_MainTex;
            float3 viewDir;
        };


        void surf (Input IN, inout SurfaceOutput o)
        {
            // Albedo comes from a texture tinted by color
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex);
            o.Albedo = c.rgb;
            o.Alpha = c.a;

            float rim = 1 - dot(IN.viewDir, o.Normal);
            o.Emission = rim*rim;
        }

        float4 LightingMyBlinnPhong(SurfaceOutput s, float3 lightDir, float3 viewDir, float atten)
        {
            float3 diffuse;
            float3 specular;
            float4 final;
            
            float d = saturate(dot(s.Normal, lightDir));
            diffuse = s.Albedo * _LightColor0 * (d*atten);
            
            float3 halfVec = normalize(lightDir + viewDir);
            
            float spec = saturate(dot(halfVec, normalize(s.Normal)));
            spec = pow(spec, _SpecPow) * 0.1;
            specular = spec * _SpecColor.rgb;

            final.rgb = specular.rgb + diffuse.rgb;
            final.rgb = diffuse.rgb;
            final.rgb = specular.rgb;
            final.rgb = specular.rgb + diffuse.rgb;
            final.rgb = diffuse.rgb;
            final.rgb = specular.rgb;
            final.a = s.Alpha;

            //return spec;
            return final;
            //return float4(halfVec, 1);
        }
        ENDCG
    }
    FallBack "Diffuse"
}
