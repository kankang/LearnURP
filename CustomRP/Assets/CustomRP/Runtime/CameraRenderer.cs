using UnityEngine;
using UnityEngine.Rendering;

public partial class CameraRenderer {

    const string bufferName = "Render Camera";
    static ShaderTagId 
	unlitShaderTagId = new ShaderTagId("SRPDefaultUnlit"),
    litShaderId = new ShaderTagId("CustomLit");
    CommandBuffer buffer = new CommandBuffer {
		name = bufferName
	};
    ScriptableRenderContext context;
    Camera camera;

    CullingResults cullingResults;


    Lighting lighting = new Lighting();

    public void Render(
		ScriptableRenderContext context, Camera camera,
		bool useDynamicBatching, bool useGPUInstancing,
		ShadowSettings shadowSettings
	) {
		this.context = context;
		this.camera = camera;

        PrepareBuffer();
        PrepareForSceneWindow();    // 可能会给场景添加几何体，所以必须在裁剪之前完成。

		if (!Cull(shadowSettings.maxDistance)) {
            return;
		}
		
		lighting.Setup(context, cullingResults, shadowSettings);
        Setup();

        

        DrawVisibleGeometry(useDynamicBatching, useGPUInstancing);
        DrawUnsupportedShaders();
        DrawGizmos();

        Submit();
    }

	bool Cull (float maxShadowDistance) {
        if (camera.TryGetCullingParameters(out ScriptableCullingParameters p)) {
			p.shadowDistance = Mathf.Min(maxShadowDistance, camera.farClipPlane);
            cullingResults = context.Cull(ref p);
            return true;
        }

        return false;
    }
	
    void Setup() {
        context.SetupCameraProperties(camera);

        CameraClearFlags flags = camera.clearFlags;
        buffer.ClearRenderTarget(
            flags <= CameraClearFlags.Depth,        // 只有Detph才不会清除深度缓冲区
            flags == CameraClearFlags.Color,        // 当标志设置为Color时，需要清除颜色缓冲区
            flags == CameraClearFlags.Color ? camera.backgroundColor : Color.clear);
            // flags == CameraClearFlags.Color ? camera.backgroundColor.linear : Color.clear);
        
        buffer.BeginSample(SampleName);
        ExecuteBuffer();
    }

    void Submit() {
        buffer.EndSample(SampleName);
        ExecuteBuffer();
        context.Submit();
    }

    void ExecuteBuffer() {
        context.ExecuteCommandBuffer(buffer);
        buffer.Clear();
    }

    void DrawVisibleGeometry(bool useDynamicBatching, bool useGPUInstancing) {
        SortingSettings sortingSettings = new SortingSettings(camera) {
            criteria = SortingCriteria.CommonOpaque,
        };
        DrawingSettings drawingSettings = new DrawingSettings(
			unlitShaderTagId, sortingSettings
		) {
            enableDynamicBatching = useDynamicBatching,
            enableInstancing = useGPUInstancing,
        };
        drawingSettings.SetShaderPassName(1, litShaderId);

        FilteringSettings filteringSettings = new FilteringSettings(RenderQueueRange.opaque);

        context.DrawRenderers(
			cullingResults, ref drawingSettings, ref filteringSettings
			);

        context.DrawSkybox(camera);

        sortingSettings.criteria = SortingCriteria.CommonTransparent;
        drawingSettings.sortingSettings = sortingSettings;

        filteringSettings.renderQueueRange = RenderQueueRange.transparent;

        context.DrawRenderers(
			cullingResults, ref drawingSettings, ref filteringSettings
		);

    }

}
