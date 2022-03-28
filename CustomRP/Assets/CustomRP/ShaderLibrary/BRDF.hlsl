#ifndef CUSTOM_BRDF_INCLUDE
#define CUSTOM_BRDF_INCLUDE

#define MIN_REFLECTIVITY 0.04

struct BRDF
{
	float3 diffuse;
	float3 specular;
	float roughness;
};

float OneMinusRelfectivity(float metallic)
{
	float range = 1.0 - MIN_REFLECTIVITY;
	return range - metallic * range;
}

BRDF GetBRDF(Surface surface)
{
	BRDF brdf;

	float oneMinusRelfectivity = OneMinusRelfectivity(surface.metallic);

	brdf.diffuse = surface.color * oneMinusRelfectivity;

	brdf.specular = lerp(MIN_REFLECTIVITY, surface.color, surface.metallic);

	float perceptualRoughness = PerceptualSmoothnessToPerceptualRoughness(surface.smoothness);
	brdf.roughness = PerceptualRoughnessToRoughness(perceptualRoughness);

	return brdf;
}

#endif
