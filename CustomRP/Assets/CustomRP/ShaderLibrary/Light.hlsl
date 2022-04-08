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
	int lightIndex, ShadowData shadowData
) {
	DirectionalShadowData data;
	data.strength =
		_DirectionalLightShadowData[lightIndex].x * shadowData.strength;
	data.tileIndex =
		_DirectionalLightShadowData[lightIndex].y + shadowData.cascadeIndex;
		
	return data;
}

Light GetDirectionalLight(int index, Surface surfaceWS, ShadowData shadowData) {
	Light light;

	light.color = _DirectionalLightColors[index].rgb;
	light.direction = _DirectionalLightDirections[index].xyz;
	DirectionalShadowData dirShadowData = 
		GetDirectionalShadowData(index, shadowData);
	light.attenuation = GetDirectionalShadowAttenuation(dirShadowData, surfaceWS);
	return light;
}

#endif
