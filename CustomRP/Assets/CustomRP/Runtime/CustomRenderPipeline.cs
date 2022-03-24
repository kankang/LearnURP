using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class CustomRenderPipeline : RenderPipeline
{
    bool useDynamicBatching;
    bool useGPUInstancing;

    public CustomRenderPipeline(bool useDynamicBatching, bool useGPUInstancing, bool useSRPBatching)
    {
        this.useDynamicBatching = useDynamicBatching;
        this.useGPUInstancing = useGPUInstancing;
        GraphicsSettings.useScriptableRenderPipelineBatching = useSRPBatching;
    }

    protected override void Render(ScriptableRenderContext context, Camera[] cameras)
    {
        CameraRender render = new CameraRender();

        foreach (Camera camera in cameras)
        {
            render.Render(context, camera, useDynamicBatching, useGPUInstancing);
        }
    }
}