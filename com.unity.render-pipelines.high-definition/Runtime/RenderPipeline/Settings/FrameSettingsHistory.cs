using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace UnityEngine.Experimental.Rendering.HDPipeline
{
    public enum FrameSettingsRenderType
    {
        Camera,
        CustomOrBakedReflection,
        RealtimeReflection
    }

    public struct FrameSettingsHistory : IDebugData
    {
        public FrameSettingsRenderType defaultType;
        public FrameSettings custom;
        public FrameSettingsOverrideMask customMask;
        public FrameSettings sanitazed;
        public FrameSettings debug;
        bool m_DebugMenuResetTriggered;
        int m_LitShaderModeEnumIndex;

        internal static Dictionary<Camera, FrameSettingsHistory> frameSettingsHistory = new Dictionary<Camera, FrameSettingsHistory>();

        public static void AggregateFrameSettings(ref FrameSettings aggregatedFrameSettings, Camera camera, HDAdditionalCameraData additionalData, HDRenderPipelineAsset hdrpAsset)
        {
            FrameSettingsHistory history = new FrameSettingsHistory();
            history.defaultType = additionalData ? additionalData.defaultFrameSettings : FrameSettingsRenderType.Camera;
            aggregatedFrameSettings = hdrpAsset.GetDefaultFrameSettings(history.defaultType);
            if (additionalData && additionalData.customRenderingSettings)
            {
                FrameSettings.Override(ref aggregatedFrameSettings, additionalData.renderingPathCustomFrameSettings, additionalData.renderingPathCustomOverrideFrameSettings);
                history.customMask = additionalData.renderingPathCustomOverrideFrameSettings;
            }
            history.custom = aggregatedFrameSettings;
            FrameSettings.Sanitize(ref aggregatedFrameSettings, camera, hdrpAsset.GetRenderPipelineSettings());

            bool dirty =
                history.sanitazed != aggregatedFrameSettings                // updated components/asset
                || !frameSettingsHistory.ContainsKey(camera)                // no history yet
                || frameSettingsHistory[camera].m_DebugMenuResetTriggered;  // reset requested by debug menu on previous frame

            history.sanitazed = aggregatedFrameSettings;

            if (dirty)
            {
                history.debug = history.sanitazed;
                switch (history.debug.litShaderMode)
                {
                    case LitShaderMode.Forward:
                        history.m_LitShaderModeEnumIndex = 0;
                        break;
                    case LitShaderMode.Deferred:
                        history.m_LitShaderModeEnumIndex = 1;
                        break;
                    default:
                        throw new ArgumentException("Unknown LitShaderMode");
                }
            }

            aggregatedFrameSettings = history.debug;
            frameSettingsHistory[camera] = history;
        }

        ref FrameSettingsHistory persistantFrameSettingsHistory
        {
            get
            {
                unsafe
                {
                    fixed (FrameSettingsHistory* pthis = &this)
                        return ref *pthis;
                }
            }
        }

        static void RegisterDebug(string menuName, FrameSettingsHistory frameSettings)
        {
            var persistant = frameSettings.persistantFrameSettingsHistory;
            List<DebugUI.Widget> widgets = new List<DebugUI.Widget>();
            widgets.AddRange(
            new DebugUI.Widget[]
            {
                new DebugUI.Foldout
                {
                    displayName = "Rendering Passes",
                    children =
                    {
                        new DebugUI.BoolField { displayName = "Enable Transparent Prepass", getter = () => persistant.debug.transparentPrepass, setter = value => persistant.debug.transparentPrepass = value },
                        new DebugUI.BoolField { displayName = "Enable Transparent Postpass", getter = () => persistant.debug.transparentPostpass, setter = value => persistant.debug.transparentPostpass = value },
                        new DebugUI.BoolField { displayName = "Enable Motion Vectors", getter = () => persistant.debug.motionVectors, setter = value => persistant.debug.motionVectors = value },
                        new DebugUI.BoolField { displayName = "  Enable Object Motion Vectors", getter = () => persistant.debug.objectMotionVectors, setter = value => persistant.debug.objectMotionVectors = value },
                        new DebugUI.BoolField { displayName = "Enable DBuffer", getter = () => persistant.debug.decals, setter = value => persistant.debug.decals = value },
                        new DebugUI.BoolField { displayName = "Enable Rough Refraction", getter = () => persistant.debug.roughRefraction, setter = value => persistant.debug.roughRefraction = value },
                        new DebugUI.BoolField { displayName = "Enable Distortion", getter = () => persistant.debug.distortion, setter = value => persistant.debug.distortion = value },
                        new DebugUI.BoolField { displayName = "Enable Postprocess", getter = () => persistant.debug.postprocess, setter = value => persistant.debug.postprocess = value },
                    }
                },
                new DebugUI.Foldout
                {
                    displayName = "Rendering Settings",
                    children =
                    {
                        new DebugUI.EnumField { displayName = "Lit Shader Mode", getter = () => (int)persistant.debug.litShaderMode, setter = value => persistant.debug.litShaderMode = (LitShaderMode)value, autoEnum = typeof(LitShaderMode), getIndex = () => persistant.m_LitShaderModeEnumIndex, setIndex = value => persistant.m_LitShaderModeEnumIndex = value },
                        new DebugUI.BoolField { displayName = "Deferred Depth Prepass", getter = () => persistant.debug.depthPrepassWithDeferredRendering, setter = value => persistant.debug.depthPrepassWithDeferredRendering = value },
                        new DebugUI.BoolField { displayName = "Enable Opaque Objects", getter = () => persistant.debug.opaqueObjects, setter = value => persistant.debug.opaqueObjects = value },
                        new DebugUI.BoolField { displayName = "Enable Transparent Objects", getter = () => persistant.debug.transparentObjects, setter = value => persistant.debug.transparentObjects = value },
                        new DebugUI.BoolField { displayName = "Enable Realtime Planar Reflection", getter = () => persistant.debug.realtimePlanarReflection, setter = value => persistant.debug.realtimePlanarReflection = value },
                        new DebugUI.BoolField { displayName = "Enable MSAA", getter = () => persistant.debug.msaa, setter = value => persistant.debug.msaa = value },
                    }
                },
                new DebugUI.Foldout
                {
                    displayName = "Lighting Settings",
                    children =
                    {
                        new DebugUI.BoolField { displayName = "Enable SSR", getter = () => persistant.debug.ssr, setter = value => persistant.debug.ssr = value },
                        new DebugUI.BoolField { displayName = "Enable SSAO", getter = () => persistant.debug.ssao, setter = value => persistant.debug.ssao = value },
                        new DebugUI.BoolField { displayName = "Enable SubsurfaceScattering", getter = () => persistant.debug.subsurfaceScattering, setter = value => persistant.debug.subsurfaceScattering = value },
                        new DebugUI.BoolField { displayName = "Enable Transmission", getter = () => persistant.debug.transmission, setter = value => persistant.debug.transmission = value },
                        new DebugUI.BoolField { displayName = "Enable Shadows", getter = () => persistant.debug.shadow, setter = value => persistant.debug.shadow = value },
                        new DebugUI.BoolField { displayName = "Enable Contact Shadows", getter = () => persistant.debug.contactShadows, setter = value => persistant.debug.contactShadows = value },
                        new DebugUI.BoolField { displayName = "Enable ShadowMask", getter = () => persistant.debug.shadowMask, setter = value => persistant.debug.shadowMask = value },
                        new DebugUI.BoolField { displayName = "Enable Atmospheric Scattering", getter = () => persistant.debug.atmosphericScattering, setter = value => persistant.debug.atmosphericScattering = value },
                        new DebugUI.BoolField { displayName = "Enable Volumetrics", getter = () => persistant.debug.volumetrics, setter = value => persistant.debug.volumetrics = value },
                        new DebugUI.BoolField { displayName = "Enable Reprojection For Volumetrics", getter = () => persistant.debug.reprojectionForVolumetrics, setter = value => persistant.debug.reprojectionForVolumetrics = value },
                        new DebugUI.BoolField { displayName = "Enable LightLayers", getter = () => persistant.debug.lightLayers, setter = value => persistant.debug.lightLayers = value },
                    }
                },
                new DebugUI.Foldout
                {
                    displayName = "Async Compute Settings",
                    children =
                    {
                        new DebugUI.BoolField { displayName = "Enable Async Compute", getter = () => persistant.debug.asyncCompute, setter = value => persistant.debug.asyncCompute = value },
                        new DebugUI.BoolField { displayName = "Run Build Light List Async", getter = () => persistant.debug.lightListAsync, setter = value => persistant.debug.lightListAsync = value },
                        new DebugUI.BoolField { displayName = "Run SSR Async", getter = () => persistant.debug.ssrAsync, setter = value => persistant.debug.ssrAsync = value },
                        new DebugUI.BoolField { displayName = "Run SSAO Async", getter = () => persistant.debug.ssaoAsync, setter = value => persistant.debug.ssaoAsync = value },
                        new DebugUI.BoolField { displayName = "Run Contact Shadows Async", getter = () => persistant.debug.contactShadowsAsync, setter = value => persistant.debug.contactShadowsAsync = value },
                        new DebugUI.BoolField { displayName = "Run Volume Voxelization Async", getter = () => persistant.debug.volumeVoxelizationAsync, setter = value => persistant.debug.volumeVoxelizationAsync = value },
                    }
                },
                new DebugUI.Foldout
                {
                    displayName = "Light Loop Settings",
                    children =
                    {
                        // Uncomment if you re-enable LIGHTLOOP_SINGLE_PASS multi_compile in lit*.shader
                        //new DebugUI.BoolField { displayName = "Enable Tile/Cluster", getter = () => persistant.debug.enableTileAndCluster, setter = value => persistant.debug.enableTileAndCluster = value },
                        new DebugUI.BoolField { displayName = "Enable Fptl for Forward Opaque", getter = () => persistant.debug.fptlForForwardOpaque, setter = value => persistant.debug.fptlForForwardOpaque = value },
                        new DebugUI.BoolField { displayName = "Enable Big Tile", getter = () => persistant.debug.bigTilePrepass, setter = value => persistant.debug.bigTilePrepass = value },
                        new DebugUI.BoolField { displayName = "Enable Compute Lighting", getter = () => persistant.debug.computeLightEvaluation, setter = value => persistant.debug.computeLightEvaluation = value },
                        new DebugUI.BoolField { displayName = "Enable Light Classification", getter = () => persistant.debug.computeLightVariants, setter = value => persistant.debug.computeLightVariants = value },
                        new DebugUI.BoolField { displayName = "Enable Material Classification", getter = () => persistant.debug.computeMaterialVariants, setter = value => persistant.debug.computeMaterialVariants = value }
                    }
                }
            });

            var panel = DebugManager.instance.GetPanel(menuName, true);
            panel.children.Add(widgets.ToArray());
        }

        public static IDebugData RegisterDebug(Camera camera, HDAdditionalCameraData additionalCameraData)
        {
            HDRenderPipelineAsset hdrpAsset = GraphicsSettings.renderPipelineAsset as HDRenderPipelineAsset;
            Assertions.Assert.IsNotNull(hdrpAsset);

            // complete frame settings history is required for displaying debug menu.
            // AggregateFrameSettings will finish the registration if it is not yet registered
            FrameSettings registering = new FrameSettings();
            AggregateFrameSettings(ref registering, camera, additionalCameraData, hdrpAsset);
            RegisterDebug(camera.name, frameSettingsHistory[camera]);
            return frameSettingsHistory[camera];
        }

        public static void UnRegisterDebug(Camera camera)
        {
            DebugManager.instance.RemovePanel(camera.name);
            frameSettingsHistory.Remove(camera);
        }

        public static IDebugData GetPersistantDebugData(Camera camera) => frameSettingsHistory[camera].persistantFrameSettingsHistory;

        void TriggerReset() => m_DebugMenuResetTriggered = true;
        Action IDebugData.GetReset() => TriggerReset;
    }
}
