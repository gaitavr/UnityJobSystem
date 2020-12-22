// Upgrade NOTE: upgraded instancing buffer 'Props' to new syntax.

Shader "Custom/ColoredPoint" {
	Properties {
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		#pragma surface surf Standard fullforwardshadows

		#pragma target 3.0

		struct Input {
			float3 worldPos;
		};

		half _Glossiness;

		UNITY_INSTANCING_BUFFER_START(Props)
		    UNITY_DEFINE_INSTANCED_PROP(half, _Metallic)
		UNITY_INSTANCING_BUFFER_END(Props)

		void surf (Input IN, inout SurfaceOutputStandard o) {
			o.Albedo.rgb = abs(IN.worldPos.xyz + 0.5) * 0.3;
			o.Metallic = UNITY_ACCESS_INSTANCED_PROP(Props, _Metallic);
			o.Smoothness = _Glossiness;
			o.Alpha = 1;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
