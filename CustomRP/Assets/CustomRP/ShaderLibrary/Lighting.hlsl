#ifndef CUSTOM_LIGHTING_INCLUDE
#define CUSTOM_LIGHTING_INCLUDE

float3 GetIncomingLight(Surface surface, Light light)
{
	return saturate(dot(surface.normal, light.direction)) * light.color;
}

float3 GetLighting(Surface surface, Light light)
{
	return GetIncomingLight(surface, light) * surface.color;
}

float3 GetLighting(Surface surface)
{
	return GetLighting(surface, GetDirectionalLight());
}

#endif
