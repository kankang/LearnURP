using UnityEngine;
using UnityEngine.Rendering;

public class CustomRenderPipeline : RenderPipeline {

	CameraRenderer renderer = new CameraRenderer();
    bool useDynamicBatching, useGPUInstancing;
    ShadowSettings shadowSettings;
	public CustomRenderPipeline(
		bool useDynamicBatching, bool useGPUInstancing, bool useSRPBatching,
		 ShadowSettings shadowSettings
	) {

        this.shadowSettings = shadowSettings;
        this.useDynamicBatching = useDynamicBatching;
        this.useGPUInstancing = useGPUInstancing;
        GraphicsSettings.useScriptableRenderPipelineBatching = useSRPBatching;
        // GraphicsSettings.lightsUseLinearIntensity = true;
        GraphicsSettings.lightsUseLinearIntensity = false;
    }

    protected override void Render(
		ScriptableRenderContext context, Camera[] cameras
	) {
        foreach (Camera camera in cameras) {
            renderer.Render(
				context, camera, useDynamicBatching, useGPUInstancing
			);
        }
    }
}