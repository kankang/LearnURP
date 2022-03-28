﻿#ifndef CUSTOM_LIGHTING_INCLUDE
#define CUSTOM_LIGHTING_INCLUDE

float3 GetIncomingLight(Surface surface, Light light)
{
	return saturate(dot(surface.normal, light.direction)) * light.color;
}

float3 GetLighting(Surface surface, BRDF brdf, Light light)
{
	return GetIncomingLight(surface, light) * brdf.diffuse;
}

float3 GetLighting(Surface surface, BRDF brdf)
{
	float3 color = 0.0;
	for (int i = 0; i < GetDirectionalLightCount(); ++i)
	{
		color += GetLighting(surface, brdf, GetDirectionalLight(i));
	}
	return color;
}

#endif
