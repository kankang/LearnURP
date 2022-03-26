#ifndef CUSTOM_LIGHT_INCLUDE
#define CUSTOM_LIGHT_INCLUDE


struct Light
{
	float3 color;
	float3 direction;
};

Light GetDirectionalLight()
{
	Light light;

	light.color = _DirectionalLightColor;
	light.direction = _DirectionalLightDirection;

	return light;
}

#endif
