Shader "Custom/sh06_uv" {
	Properties {
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows

		sampler2D _MainTex;

		struct Input {
			float2 uv_MainTex;
		};

		void surf (Input IN, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex);
			o.Albedo = c.rgb;
			//o.Emission = IN.uv_MainTex.x;
			o.Emission = float3(IN.uv_MainTex.x, IN.uv_MainTex.y, 0);
			o.Alpha = c.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}