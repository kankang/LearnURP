
using UnityEditor;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.Rendering;

partial class CameraRenderer {
    partial void DrawGizmos();

    partial void DrawUnsupportedShaders();

    partial void PrepareForSceneWindow();

    partial void PrepareBuffer();

#if UNITY_EDITOR
    static ShaderTagId[] legacyShaderTagIds = {
        new ShaderTagId("Always"),
        new ShaderTagId("ForwardBase"),
        new ShaderTagId("PrepassBase"),
        new ShaderTagId("Vertex"),
        new ShaderTagId("VertexLMRGBM"),
        new ShaderTagId("VertexLM")
    };
	
    static Material errorMaterial = null;
    string SampleName { get; set; }

    partial void DrawGizmos() {
        if (Handles.ShouldRenderGizmos()) {
            context.DrawGizmos(camera, GizmoSubset.PreImageEffects);
            context.DrawGizmos(camera, GizmoSubset.PostImageEffects);
        }
    }
	
    partial void DrawUnsupportedShaders() {
        if (null == errorMaterial) {
			errorMaterial =
				new Material(Shader.Find("Hidden/InternalErrorShader"));
        }

        DrawingSettings drawingSettings = new DrawingSettings(
			legacyShaderTagIds[0], new SortingSettings(camera)
		) {
			overrideMaterial = errorMaterial
        };

        for (int i = 1; i < legacyShaderTagIds.Length; ++i) {
            drawingSettings.SetShaderPassName(i, legacyShaderTagIds[i]);
        }

        FilteringSettings filteringSettings = FilteringSettings.defaultValue;

        context.DrawRenderers(
			cullingResults, ref drawingSettings, ref filteringSettings
		);
    }


    partial void PrepareForSceneWindow() {
        if (camera.cameraType == CameraType.SceneView) {
            ScriptableRenderContext.EmitWorldGeometryForSceneView(camera);
        }
    }

    partial void PrepareBuffer() {
        Profiler.BeginSample("Editor Only");
        buffer.name = SampleName = camera.name;  // Frame Debugger中，具有相同名称的相邻Sample作用域会被合并，所以我们最终看到一个Render Camera作用域，这样使用摄像机名称指定作用域名称
        Profiler.EndSample();
    }
#else
    const string SampleName = bufferName;
#endif
}
