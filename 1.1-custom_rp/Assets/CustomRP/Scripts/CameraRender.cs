using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public partial class CameraRender
{
    ScriptableRenderContext context;
    Camera camera;

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
        context.SetupCameraProperties(camera);  // 设置视图投影矩阵unity_MatrixVP
    }

    void Submit()
    {
        context.Submit();
    }

}
