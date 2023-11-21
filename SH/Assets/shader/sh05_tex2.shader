Shader "Custom/sh05_tex2" {
	Properties {
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_MainTex2 ("Albedo2 (RGB)", 2D) = "white" {}
		_ratio ("ratio", Range(0,1)) = 0.0
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows

		sampler2D _MainTex;
		sampler2D _MainTex2;
		float _ratio;

		struct Input {
			float2 uv_MainTex;
			float2 uv_MainTex2;
		};


		void surf (Input IN, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex);
			fixed4 d = tex2D (_MainTex2, IN.uv_MainTex2);
			o.Albedo = lerp(c.rgb, d.rgb, _ratio);
			//o.Albedo = lerp(c.rgb, d.rgb, d.a);
			//o.Albedo = lerp(c.rgb, d.rgb, 1-d.a);
			o.Alpha = c.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}