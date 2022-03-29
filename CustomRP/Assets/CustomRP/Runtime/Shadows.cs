using UnityEngine;
using UnityEngine.Rendering;

public class Shadows {
    const string bufferName = "Shadows";
	
    const int maxShadowedDirectionalLightCount = 1;
	
	struct ShadowedDirectionalLight {
        public int visibleLightIndex;
    }

    ShadowedDirectionalLight[] shadowedDirectionalLights =
        new ShadowedDirectionalLight[maxShadowedDirectionalLightCount];



    int shadowedDirectionalLightCount;
	
    CommandBuffer buffer = new CommandBuffer {
        name = bufferName
    };



    ScriptableRenderContext context;

    CullingResults cullingResults;

    ShadowSettings shadowSettings;


    public void Setup (
		ScriptableRenderContext context, CullingResults cullingResults,
		ShadowSettings shadowSettings
	) {
        this.context = context;
        this.cullingResults = cullingResults;
        this.shadowSettings = shadowSettings;

        shadowedDirectionalLightCount = 0;
    }


    public void ReserveDirectionalShadows(
        Light light, int visibleLightIndex
    ) {
        if (shadowedDirectionalLightCount < maxShadowedDirectionalLightCount &&
            light.shadows != LightShadows.None && light.shadowStrength > 0f &&
            cullingResults.GetShadowCasterBounds(visibleLightIndex, out Bounds b))
        {
            shadowedDirectionalLights[shadowedDirectionalLightCount++] =
                new ShadowedDirectionalLight {
                    visibleLightIndex = visibleLightIndex
                };
        }
    }
	
	void ExecuteBuffer() {
        context.ExecuteCommandBuffer(buffer);
        buffer.Clear();
    }

}
