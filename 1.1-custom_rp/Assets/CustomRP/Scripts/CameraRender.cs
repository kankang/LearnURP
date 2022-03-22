using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public partial class CameraRender
{
    ScriptableRenderContext context;
    Camera camera;

    const string buffName = "Render Camera";
    CommandBuffer buffer = new CommandBuffer { name = buffName };

    CullingResults cullingResults;

    static ShaderTagId unlitShaderTagId = new ShaderTagId("SRPDefaultUnlit");

    public void Render(ScriptableRenderContext context, Camera camera)
    {
        this.context = context;
        this.camera = camera;

        if (!Cull())
            return;

        Setup();

        DrawVisiableGeometry();

        Submit();
    }

    void DrawVisiableGeometry()
    {
        SortingSettings sortingSettings = new SortingSettings(camera)
        {
            criteria = SortingCriteria.CommonOpaque
        };
        DrawingSettings drawingSettings = new DrawingSettings(unlitShaderTagId, sortingSettings);

        FilteringSettings filteringSettings = new FilteringSettings(RenderQueueRange.opaque);

        context.DrawRenderers(cullingResults, ref drawingSettings, ref filteringSettings);

        context.DrawSkybox(camera);

        sortingSettings = new SortingSettings(camera)
        {
            criteria = SortingCriteria.CommonTransparent,
        };
        drawingSettings = new DrawingSettings(unlitShaderTagId, sortingSettings);

        filteringSettings = new FilteringSettings(RenderQueueRange.transparent);

        context.DrawRenderers(cullingResults, ref drawingSettings, ref filteringSettings);
    }

    void Setup()
    {
        context.SetupCameraProperties(camera);  // 设置视图投影矩阵unity_MatrixVP

        buffer.ClearRenderTarget(true, true, Color.clear);
        buffer.BeginSample(buffName);
        ExecuteBuffer();
    }

    void Submit()
    {
        buffer.EndSample(buffName);
        ExecuteBuffer();
        context.Submit();
    }

    void ExecuteBuffer()
    {
        context.ExecuteCommandBuffer(buffer);
        buffer.Clear();
    }

    bool Cull()
    {
        if (camera.TryGetCullingParameters(out ScriptableCullingParameters p))
        {
            cullingResults = context.Cull(ref p);
            return true;
        }

        return false;
    }
}
