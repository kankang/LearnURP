#ifndef CUSTOM_UNITY_INPUT_INCLUDE
#define CUSTOM_UNITY_INPUT_INCLUDE

CBUFFER_START(UnityPerDraw)
	float4x4 unity_ObjectToWorld;
	float4x4 unity_WorldToObject;
	float4 unity_LODFade;

	float4x4 glstate_matrix_projection;

	real4 unity_WorldTransformParams;
CBUFFER_END

CBUFFER_START(_CustomLight)
	float3 _DirectionalLightColor;
	float3 _DirectionalLightDirection;
CBUFFER_END

float4x4 unity_MatrixV;
float4x4 unity_MatrixVP;

#endif
