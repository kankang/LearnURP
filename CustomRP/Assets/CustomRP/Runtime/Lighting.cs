using Unity.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class Lighting
{
    const string bufferName = "Lighting";

    CommandBuffer buffer = new CommandBuffer { name = bufferName };

    static int
        dirLightColorId = Shader.PropertyToID("_DirectionalLightColor"),
        dirLightDirectionId = Shader.PropertyToID("_DirectionalLightDirection");

    CullingResults cullingResults;

    public void Setup(ScriptableRenderContext context, CullingResults cullingResults)
    {
        this.cullingResults = cullingResults;

        buffer.BeginSample(bufferName);

        // SetupDirectionalLight();
        SetupLights();

        buffer.EndSample(bufferName);
        context.ExecuteCommandBuffer(buffer);
        buffer.Clear();
    }

    public void SetupDirectionalLight()
    {
        Light light = RenderSettings.sun;

        // buffer.SetGlobalVector(dirLightColorId, light.color.linear * light.intensity);
        buffer.SetGlobalVector(dirLightColorId, light.color * light.intensity);
        buffer.SetGlobalVector(dirLightDirectionId, -light.transform.forward);
    }

    public void SetupLights()
    {
        NativeArray<VisibleLight> visibleLights = cullingResults.visibleLights;
    }
}
