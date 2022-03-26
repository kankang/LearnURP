#ifndef CUSTOM_LIGHT_INCLUDE
#define CUSTOM_LIGHT_INCLUDE


struct Light
{
	float3 color;
	float3 direction;
};

int GetDirectionalLightCount()
{
	return _DirectionalLightCount;
}

Light GetDirectionalLight(int index)
{
	Light light;

	light.color = _DirectionalLightColors[index].rgb;
	light.direction = _DirectionalLightDirections[index].rgb;

	return light;
}

#endif
