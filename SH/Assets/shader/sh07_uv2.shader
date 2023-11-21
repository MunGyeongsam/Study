Shader "Custom/sh07_uv2"
{
    Properties
    {
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _FlowSpeed("Flow Speed", Range(-5, 5)) = 0.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows


        sampler2D _MainTex;
        float _FlowSpeed;

        struct Input
        {
            float2 uv_MainTex;
        };

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Albedo comes from a texture tinted by color
            //fixed4 c = tex2D (_MainTex, IN.uv_MainTex + 0.5);
            //fixed4 c = tex2D (_MainTex, IN.uv_MainTex * 3.0);
            //fixed4 c = tex2D (_MainTex, IN.uv_MainTex + float2(_SinTime.x,0.0));
            //fixed4 c = tex2D (_MainTex, IN.uv_MainTex + float2(_SinTime.z,0.0));
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex + float2(0, _Time.y * _FlowSpeed));
            
            //o.Albedo = c.rgb;
            o.Emission = c.rgb;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
