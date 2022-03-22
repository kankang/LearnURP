﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class CustomRenderPipeline : RenderPipeline
{
    protected override void Render(ScriptableRenderContext context, Camera[] cameras)
    {
        CameraRender render = new CameraRender();

        foreach (Camera camera in cameras)
        {
            render.Render(context, camera);
        }
    }
}