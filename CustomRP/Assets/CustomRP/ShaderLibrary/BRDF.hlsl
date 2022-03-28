#ifndef CUSTOM_BRDF_INCLUDE
#define CUSTOM_BRDF_INCLUDE

#define MIN_REFLECTIVITY 0.04

struct BRDF
{
	float diffuse;
	float specular;
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

	brdf.specular = 0.0;
	brdf.roughness = 1.0;

	return brdf;
}

#endif
