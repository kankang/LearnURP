using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Rendering;

public partial class CameraRender
{
    partial void DrawGizmos();

    partial void DrawUnsupportedShaders();

#if UNITY_EDITOR
    static Material errorMaterial = null;

    static ShaderTagId[] legacyShaderTagIds =
    {
        new ShaderTagId("Always"),
        new ShaderTagId("ForwardBase"),
        new ShaderTagId("PrepassBase"),
        new ShaderTagId("Vertex"),
        new ShaderTagId("VertexLMRGBM"),
        new ShaderTagId("VertexLM")
    };

    partial void DrawUnsupportedShaders()
    {

        if (null == errorMaterial)
        {
            errorMaterial = new Material(Shader.Find("Hidden/InternalErrorShader"));
        }

        DrawingSettings drawingSettings = new DrawingSettings(legacyShaderTagIds[0], new SortingSettings(camera))
        {
            overrideMaterial = errorMaterial
        };

        for (int i = 1; i < legacyShaderTagIds.Length; ++i)
        {
            drawingSettings.SetShaderPassName(i, legacyShaderTagIds[i]);
        }

        FilteringSettings filteringSettings = FilteringSettings.defaultValue;

        context.DrawRenderers(cullingResults, ref drawingSettings, ref filteringSettings);
    }

    partial void DrawGizmos()
    {
        if (Handles.ShouldRenderGizmos())
        {
            context.DrawGizmos(camera, GizmoSubset.PreImageEffects);
            context.DrawGizmos(camera, GizmoSubset.PostImageEffects);
        }
    }
#endif
}
