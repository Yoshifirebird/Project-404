Shader "Custom/SMG Water" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_SecondTex ("Albedo (RGB) B", 2D) = "white" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Specular ("Specular", Range(0,1)) = 0.0

		_LowThreshold ("Low Threshold", Range(0,1)) = 0.2
		_HiThreshold ("High Threshold", Range(0,1)) = 0.2

		_Waves("WaveSpeed", Vector) = (1,1,1,1)
	}
	SubShader {
		Tags {"IgnoreProjector"="True" "RenderType"="Transparent"}
		Cull Off
		LOD 200

		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf StandardSpecular fullforwardshadows alpha

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;
		sampler2D _SecondTex;

		half _Glossiness;
		half _Specular;
		fixed4 _Color;

		half _LowThreshold;
		half _HiThreshold;
		float4 _Waves;

		struct Input {
			float2 uv_MainTex;
			float2 uv_SecondTex;

		};

		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		//UNITY_INSTANCING_BUFFER_START(Props)
			// put more per-instance properties here
		//UNITY_INSTANCING_BUFFER_END(Props)

		void surf (Input IN, inout SurfaceOutputStandardSpecular o) {

			float2 uv = IN.uv_MainTex;
			float2 uvb = IN.uv_SecondTex;
			//float4 waves = _Waves;

			uv += fixed2((_Time.y/60)*_Waves.r, (_Time.y/60)*_Waves.g);
			uvb += fixed2((_Time.y/60)*_Waves.b, (_Time.y/60)*_Waves.a);

			fixed4 c = tex2D (_MainTex, uv);
			fixed4 cb = tex2D (_SecondTex, uvb);
			o.Albedo = _Color;


			o.Smoothness = _Glossiness;

			//o.Alpha = _Color.a;
			o.Alpha = 0;
			o.Alpha += ceil(_LowThreshold - (c.a+cb.a));
			o.Alpha += ceil((c.a+cb.a)-_HiThreshold);
			o.Alpha *= (c.a+cb.a)*_Color.a;
			o.Specular = _Specular*((c.a+cb.a)*_Color.a);
		}
		ENDCG
	}
	FallBack "Diffuse"
}
