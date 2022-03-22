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

    public void Render(ScriptableRenderContext context, Camera camera)
    {
        this.context = context;
        this.camera = camera;

        Setup();

        DrawVisiableGeometry();

        Submit();
    }

    void DrawVisiableGeometry()
    {
        context.DrawSkybox(camera);
    }

    void Setup()
    {
        buffer.BeginSample(buffName);
        ExecuteBuffer();

        context.SetupCameraProperties(camera);  // 设置视图投影矩阵unity_MatrixVP
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
}
