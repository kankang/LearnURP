using UnityEngine;
using UnityEngine.Rendering;

public class Shadows {
    const string bufferName = "Shadows";

    const int maxShadowedDirectionalLightCount = 4;
	
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

    static int dirShadowAtlasId = Shader.PropertyToID("_DirectionalShadowAtlas");

    public void Setup (
		ScriptableRenderContext context, CullingResults cullingResults,
		ShadowSettings shadowSettings
	) {
        this.context = context;
        this.cullingResults = cullingResults;
        this.shadowSettings = shadowSettings;

        shadowedDirectionalLightCount = 0;
    }

    public void Cleanup() {
        if (shadowedDirectionalLightCount > 0) {
            buffer.ReleaseTemporaryRT(dirShadowAtlasId);
            ExecuteBuffer();
        }
    }

    public void ReserveDirectionalShadows(
        Light light, int visibleLightIndex
    ) {
        if (
			shadowedDirectionalLightCount < maxShadowedDirectionalLightCount &&
            light.shadows != LightShadows.None && light.shadowStrength > 0f &&
            cullingResults.GetShadowCasterBounds(visibleLightIndex, out Bounds b))
        {
            shadowedDirectionalLights[shadowedDirectionalLightCount++] =
                new ShadowedDirectionalLight {
                    visibleLightIndex = visibleLightIndex
                };
        }
    }
	

    public void Render()
    {
        if (shadowedDirectionalLightCount > 0)
        {
            RenderDirectionalShadows();
        }
    }

    void RenderDirectionalShadows() {
        int atlasSize = (int)shadowSettings.directional.atlasSize;
        buffer.GetTemporaryRT(
			dirShadowAtlasId, atlasSize, atlasSize,
            32, FilterMode.Bilinear, RenderTextureFormat.Shadowmap
		);
        buffer.SetRenderTarget(
            dirShadowAtlasId,
            RenderBufferLoadAction.DontCare,
            RenderBufferStoreAction.Store);
        buffer.ClearRenderTarget(true, false, Color.clear);

        buffer.BeginSample(bufferName);
        ExecuteBuffer();

        int split = shadowedDirectionalLightCount <= 1 ? 1 : 2;
        int tileSize = atlasSize / split;

        for (int i = 0; i < shadowedDirectionalLightCount; ++i)
        {
            RenderDirectionalShadows(i, split, tileSize);
        }

        buffer.EndSample(bufferName);
        ExecuteBuffer();
    }

    void RenderDirectionalShadows(int index, int split, int tileSize)
    {
        ShadowedDirectionalLight light = shadowedDirectionalLights[index];

        ShadowDrawingSettings shadowSettings =
            new ShadowDrawingSettings(cullingResults, light.visibleLightIndex);

        cullingResults.ComputeDirectionalShadowMatricesAndCullingPrimitives(
            light.visibleLightIndex, 0, 1, Vector3.zero, tileSize, 0f,
            out Matrix4x4 viewMatrix, out Matrix4x4 projectionMatrix,
            out ShadowSplitData splitData);
        shadowSettings.splitData = splitData;
        SetTileViewPort(index, split, tileSize);
        buffer.SetViewProjectionMatrices(viewMatrix, projectionMatrix);

        ExecuteBuffer();
        context.DrawShadows(ref shadowSettings);
    }

    void SetTileViewPort(int index, int split, float tileSize) {
        Vector2 offset = new Vector2(index % split, index / split);
        buffer.SetViewport(new Rect(
            offset.x * tileSize, offset.y * tileSize, tileSize, tileSize
		));
    }
    void ExecuteBuffer() {
        context.ExecuteCommandBuffer(buffer);
        buffer.Clear();
    }
}
