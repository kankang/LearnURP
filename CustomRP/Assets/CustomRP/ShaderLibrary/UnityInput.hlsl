#ifndef CUSTOM_UNITY_INPUT_INCLUDE
#define CUSTOM_UNITY_INPUT_INCLUDE

CBUFFER_START(UnityPerDraw)
	float4x4 unity_ObjectToWorld;
	float4x4 unity_WorldToObject;
	float4 unity_LODFade;

	float4x4 glstate_matrix_projection;

	real4 unity_WorldTransformParams;

	float3 _WorldSpaceCameraPos;
CBUFFER_END

#define MAX_DIRECTIONAL_LIGHT_COUNT 4

CBUFFER_START(_CustomLight)
	int _DirectionalLightCount;
	float3 _DirectionalLightColors[MAX_DIRECTIONAL_LIGHT_COUNT];
	float3 _DirectionalLightDirections[MAX_DIRECTIONAL_LIGHT_COUNT];
	float4 _DirectionalLightShadowData[MAX_DIRECTIONAL_LIGHT_COUNT];
CBUFFER_END

float4x4 unity_MatrixV;
float4x4 unity_MatrixVP;

#endif
