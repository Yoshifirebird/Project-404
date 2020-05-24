﻿Shader "Project 404/Onion" {
	Properties{
		_Color("Color", Color) = (1,1,1,1)
		_MainTex("Albedo (RGB)", 2D) = "white" {}
		_SecondTex("Albedo (RGB) B", 2D) = "white" {}
		_ColorRamp("ColorRamp", 2D) = "white" {}
		_Glossiness("Smoothness", Range(0,1)) = 0.5
		_Specular("Specular", Range(0,1)) = 0.0
	}
		SubShader{
			Tags {"RenderType" = "Geometry"}
			//LOD 200

			CGPROGRAM
			// Physically based Standard lighting model, and enable shadows on all light types
			#pragma surface surf StandardSpecular fullforwardshadows


			// Use shader model 3.0 target, to get nicer looking lighting
			#pragma target 3.0

			sampler2D _MainTex;
			sampler2D _SecondTex;
			sampler2D _ColorRamp;

			half _Glossiness;
			half _Specular;
			fixed4 _Color;


			struct Input {
				float2 uv_MainTex;
				float2 uv_SecondTex;

			};



			void surf(Input IN, inout SurfaceOutputStandardSpecular o) {

				float2 uv = IN.uv_MainTex;
				float2 uvb = IN.uv_SecondTex;
				//float4 waves = _Waves;

				uv += fixed2((_Time.y / 20), (_Time.y / 10));
				uvb += fixed2(-(_Time.y / 10), (-(_Time.y / 20)));

				fixed4 c = tex2D(_MainTex, uv) / 2;
				fixed4 cb = tex2D(_SecondTex, uvb) / 2;

				o.Albedo = tex2D(_ColorRamp, c.rgb + cb.rgb);
				o.Smoothness = _Glossiness;
				o.Alpha = 1;
			}
			ENDCG
		}
			FallBack "Diffuse"
}