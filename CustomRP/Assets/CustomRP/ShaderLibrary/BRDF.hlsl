#ifndef CUSTOM_BRDF_INCLUDE
#define CUSTOM_BRDF_INCLUDE

struct BRDF
{
	float diffuse;
	float specular;
	float roughness;
};

BRDF GetBRDF(Surface surface)
{
	BRDF brdf;
	brdf.diffuse = surface.color;
	brdf.specular = 0.0;
	brdf.roughness = 1.0;

	return brdf;
}

#endif
