﻿Shader "Custom/Waves" {
	Properties {
		_Color ("Colour", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		[NoScaleOffset] _FlowMap("Flow (RG, A noise)", 2D) = "black" {}
		[NoScaleOffset] _NormalMap("Normals", 2D) = "bump"{}
		_UJump("U jump per phase", Range(-0.25, 0.25)) = 0.25
		_VJump("V jump per phase", Range(-0.25, 0.25)) = 0.25
		_Tiling("Tiling", Float) = 1
		_Speed("Speed", Float) = 1
		_FlowStrength("Flow Strength", Float) = 1
		_FlowOffset("Flow Offset", Float) = 0
		_Glossiness("Smoothness", Range(0,1)) = 0.5
		_Metallic("Metallic", Range(0,1)) = 0.0
		_WaveA("Wave A (dir, steepness, wavelength)", Vector) = (1,0,0.5,10)
		_WaveB("Wave B", Vector) = (0,1,0.25,20)
		_WaveC("Wave C", Vector) = (1,1,0.15,10)
		_WaterFogColor("Water Fog Colour", Color) = (0,0,0,0)
		_WaterFogDensity("Water Fog Density", Range(0,2)) = 0.1
		_RefractionStrength("Refraction Strength", Range(0, 1)) = 0.25

	}
		SubShader{
			Tags { "RenderType" = "Transparent" "Queue" = "Transparent" }

			GrabPass{"_WaterBackground"}

			CGPROGRAM
			#pragma surface surf Standard alpha finalcolor:ResetAlpha vertex:vert
			#pragma target 3.0

			sampler2D _MainTex, _FlowMap, _NormalMap;

		struct Input {
			float2 uv_MainTex;
			float4 screenPos;
			float eyeDepth;
		};

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;
		float4 _WaveA, _WaveB, _WaveC;
		float _UJump, _VJump, _Tiling, _Speed, _FlowStrength, _FlowOffset;

		float3 GerstnerWave (float4 wave, float3 p, inout float3 tangent, inout float3 binormal)
		{
		    float steepness = wave.z;
		    float wavelength = wave.w;
		    float k = 2 * UNITY_PI / wavelength;
			float c = sqrt(9.8 / k);
			float2 d = normalize(wave.xy);
			float f = k * (dot(d, p.xz) - c * _Time.y);
			float a = steepness / k;

			tangent += float3(
				-d.x * d.x * (steepness * sin(f)),
				d.x * (steepness * cos(f)),
				-d.x * d.y * (steepness * sin(f))
			);
			binormal += float3(
				-d.x * d.y * (steepness * sin(f)),
				d.y * (steepness * cos(f)),
				-d.y * d.y * (steepness * sin(f))
			);
			return float3(
				d.x * (a * cos(f)),
				a * sin(f),
				d.y * (a * cos(f))
			);
		}

		void vert(inout appdata_full vertexData) 
		{
			float3 gridPoint = vertexData.vertex.xyz;
			float3 tangent = float3(1, 0, 0);
			float3 binormal = float3(0, 0, 1);
			float3 p = gridPoint;
			p += GerstnerWave(_WaveA, gridPoint, tangent, binormal);
			p += GerstnerWave(_WaveB, gridPoint, tangent, binormal);
			p += GerstnerWave(_WaveC, gridPoint, tangent, binormal);
			float3 normal = normalize(cross(binormal, tangent));
			vertexData.vertex.xyz = p;
			vertexData.normal = normal;
		}

		#include "LookingThroughWater.cginc"
		#include "Flow.cginc"

		void ResetAlpha(Input IN, SurfaceOutputStandard o, inout fixed4 color)
		{
			color.a = 1;
		}

		void surf (Input IN, inout SurfaceOutputStandard o) 
		{
			float3 flow = tex2D(_FlowMap, IN.uv_MainTex).rgb;
			flow.xy = flow.xy * 2 - 1;
			flow *= _FlowStrength;
			
			float noise = tex2D(_FlowMap, IN.uv_MainTex).a;
			float time = _Time.y * _Speed + noise;
			float2 jump = float2(_UJump, _VJump);
			
			float3 uvwA = FlowUVW(
				IN.uv_MainTex, flow.xy, jump,
				_FlowOffset, _Tiling, time, false
			);
			float3 uvwB = FlowUVW(
				IN.uv_MainTex, flow.xy, jump,
				_FlowOffset,_Tiling, time, true
			);

			float3 normalA = UnpackNormal(tex2D(_NormalMap, uvwA.xy)) * uvwA.z;
			float3 normalB = UnpackNormal(tex2D(_NormalMap, uvwB.xy)) * uvwB.z;
			o.Normal = normalize(normalA + normalB);

			fixed4 texA = tex2D(_MainTex, uvwA.xy) * uvwA.z;
			fixed4 texB = tex2D(_MainTex, uvwB.xy) * uvwB.z;
			fixed4 c = (texA + texB) * _Color;

			o.Albedo = c.rgb;

			
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = c.a;
			o.Emission = ColorBelowWater(IN.screenPos, o.Normal) * (1 - c.a);
		}
		ENDCG
	}
}