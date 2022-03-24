using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public partial class CameraRender
{
    ScriptableRenderContext context;
    Camera camera;

    const string bufferName = "Render Camera";
    CommandBuffer buffer = new CommandBuffer { name = bufferName };

    CullingResults cullingResults;

    static ShaderTagId unlitShaderTagId = new ShaderTagId("SRPDefaultUnlit");


    public void Render(ScriptableRenderContext context, Camera camera)
    {
        this.context = context;
        this.camera = camera;

        PrepareBuffer();
        PrepareForSceneWindow();    // 可能会给场景添加几何体，所以必须在裁剪之前完成。

        if (!Cull())
            return;

        Setup();

        DrawVisiableGeometry();
        DrawUnsupportedShaders();
        DrawGizmos();

        Submit();
    }

    void DrawVisiableGeometry()
    {
        //DrawingSettings drawingSettings = new DrawingSettings();
        //FilteringSettings filteringSettings = new FilteringSettings();

        SortingSettings sortingSettings = new SortingSettings(camera)
        {
            criteria = SortingCriteria.CommonOpaque,
        };
        DrawingSettings drawingSettings = new DrawingSettings(unlitShaderTagId, sortingSettings)
        {
            enableDynamicBatching = true,
            enableInstancing = true,
        };

        FilteringSettings filteringSettings = new FilteringSettings(RenderQueueRange.opaque);

        context.DrawRenderers(cullingResults, ref drawingSettings, ref filteringSettings);

        context.DrawSkybox(camera);

        sortingSettings = new SortingSettings(camera)
        {
            criteria = SortingCriteria.CommonTransparent,
        };
        drawingSettings = new DrawingSettings(unlitShaderTagId, sortingSettings)
        {
            enableDynamicBatching = true,
            enableInstancing = true,
        };

        filteringSettings = new FilteringSettings(RenderQueueRange.transparent);

        context.DrawRenderers(cullingResults, ref drawingSettings, ref filteringSettings);

    }

    void Setup()
    {
        context.SetupCameraProperties(camera);
        
        buffer.BeginSample(SampleName);

        CameraClearFlags flags = camera.clearFlags;
        buffer.ClearRenderTarget(
            flags <= CameraClearFlags.Depth,        // 只有Detph才不会清除深度缓冲区
            flags == CameraClearFlags.Color,        // 当标志设置为Color时，需要清除颜色缓冲区
            flags == CameraClearFlags.Color ? camera.backgroundColor : Color.clear);
            // flags == CameraClearFlags.Color ? camera.backgroundColor.linear : Color.clear);

        ExecuteBuffer();
    }

    void Submit()
    {
        buffer.EndSample(SampleName);
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
