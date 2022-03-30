#ifndef CUSTOM_LIGHT_INCLUDE
#define CUSTOM_LIGHT_INCLUDE


struct Light {
	float3 color;
	float3 direction;
	float attenuation;
};

int GetDirectionalLightCount() {
	return _DirectionalLightCount;
}

DirectionalShadowData GetDirectionalShadowData(
	int lightIndex
) {
	DirectionalShadowData data;
	data.strength = _DirectionalLightShadowData[lightIndex].x;
	data.tileIndex = _DirectionalLightShadowData[lightIndex].y;

	return data;
}

Light GetDirectionalLight(int index, Surface surfaceWS) {
	Light light;

	light.color = _DirectionalLightColors[index].rgb;
	light.direction = _DirectionalLightDirections[index].xyz;
	DirectionalShadowData shadowData = 
		GetDirectionalShadowData(index);
	light.attenuation = 
		GetDirectionalShadowAttenuation(shadowData, surfaceWS);

	return light;
}

#endif
