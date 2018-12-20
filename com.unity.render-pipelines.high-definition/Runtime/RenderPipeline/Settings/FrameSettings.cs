using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine.Rendering;

namespace UnityEngine.Experimental.Rendering.HDPipeline
{
    public static partial class StringExtention
    {
        public static string CamelToPascalCaseWithSpace(this string text, bool preserveAcronyms = true)
        {
            if (string.IsNullOrWhiteSpace(text))
                return string.Empty;
            StringBuilder newText = new StringBuilder(text.Length * 2);
            newText.Append(char.ToUpper(text[0]));
            for (int i = 1; i < text.Length; i++)
            {
                if (char.IsUpper(text[i]))
                    if ((text[i - 1] != ' ' && !char.IsUpper(text[i - 1])) ||
                        (preserveAcronyms && char.IsUpper(text[i - 1]) &&
                            i < text.Length - 1 && !char.IsUpper(text[i + 1])))
                        newText.Append(' ');
                newText.Append(text[i]);
            }
            return newText.ToString();
        }
    }
    
    /// <summary Should only be used on enum value of field to describe aspect in DebugMenu </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class DebugMenuFieldAttribute : Attribute
    {
        public enum DisplayType { Checkbox, EnumPopup }
        public readonly DisplayType type;
        public readonly string displayedName;
        public readonly string path;
        public readonly Type enumTarget;
        public readonly string[] enumNames;

        public DebugMenuFieldAttribute(string displayedName = null, string path = null, DisplayType type = DisplayType.Checkbox, Type enumTarget = null, [CallerMemberName] string autoDisplayedNameWithPropertyName = null)
        {
            if(string.IsNullOrEmpty(displayedName))
                displayedName = autoDisplayedNameWithPropertyName.CamelToPascalCaseWithSpace();
            if (string.IsNullOrEmpty(path))
                path = "";
            this.type = type;
            this.displayedName = displayedName;
            this.path = path;
            this.enumTarget = enumTarget;
            enumNames = enumTarget == null ? null : enumTarget.GetEnumNames();
        }
    }

    public enum LitShaderMode
    {
        Forward,
        Deferred
    }

    public enum FrameSettingsRenderType
    {
        Camera,
        CustomOrBakedReflection,
        RealtimeReflection
    }

    public enum FrameSettingsField
    {
        //lighting settings from 0 to 19
        [DebugMenuField(path: "Light Settings")]
        Shadow = 0,
        [DebugMenuField(path: "Light Settings")]
        ContactShadow = 1,
        [DebugMenuField(path: "Light Settings")]
        ShadowMask = 2,
        [DebugMenuField(path: "Light Settings")]
        SSR = 3,
        [DebugMenuField(path: "Light Settings")]
        SSAO = 4,
        [DebugMenuField(path: "Light Settings")]
        SubsurfaceScattering = 5,
        [DebugMenuField(path: "Light Settings")]
        Transmission = 6,
        [DebugMenuField(path: "Light Settings")]
        AtmosphericScaterring = 7,
        [DebugMenuField(path: "Light Settings")]
        Volumetrics = 8,
        [DebugMenuField(path: "Light Settings")]
        ReprojectionForVolumetrics = 9,
        [DebugMenuField(path: "Light Settings")]
        LightLayers = 10,
        [DebugMenuField(path: "Light Settings")]
        MSAA = 11,

        //rendering pass from 20 to 39
        [DebugMenuField(path: "Rendering Passes")]
        TransparentPrepass = 20,
        [DebugMenuField(path: "Rendering Passes")]
        TransparentPostpass = 21,
        [DebugMenuField(path: "Rendering Passes")]
        MotionVectors = 22,
        [DebugMenuField(path: "Rendering Passes")]
        ObjectMotionVectors = 23,
        [DebugMenuField(path: "Rendering Passes")]
        Decals = 24,
        [DebugMenuField(path: "Rendering Passes")]
        RoughRefraction = 25,
        [DebugMenuField(path: "Rendering Passes")]
        Distortion = 26,
        [DebugMenuField(path: "Rendering Passes")]
        Postprocess = 27,

        //rendering settings from 40 to 59
        [DebugMenuField(path: "Rendering Settings", type: DebugMenuFieldAttribute.DisplayType.EnumPopup, enumTarget: typeof(LitShaderMode))]
        ShaderLitMode = 40,
        [DebugMenuField(path: "Rendering Settings")]
        DepthPrepassWithDeferredRendering = 41,
        [DebugMenuField(path: "Rendering Settings")]
        OpaqueObjects = 42,
        [DebugMenuField(path: "Rendering Settings")]
        TransparentObjects = 43,
        [DebugMenuField(path: "Rendering Settings")]
        RealtimePlanarReflection = 44,

        //async settings from 60 to 79
        [DebugMenuField(path: "Async Compute Settings")]
        AsyncCompute = 60,
        [DebugMenuField(path: "Async Compute Settings")]
        LightListAsync = 61,
        [DebugMenuField(path: "Async Compute Settings")]
        SSRAsync = 62,
        [DebugMenuField(path: "Async Compute Settings")]
        SSAOAsync = 63,
        [DebugMenuField(path: "Async Compute Settings")]
        ContactShadowsAsync = 64,
        [DebugMenuField(path: "Async Compute Settings")]
        VolumeVoxelizationsAsync = 65,

        //from 80 to 119 : space for new scopes

        //lightLoop settings from 120 to 127
        [DebugMenuField(path: "Light Loop Settings")]
        FptlForForwardOpaque = 120,
        [DebugMenuField(path: "Light Loop Settings")]
        BigTilePrepass = 121,
        [DebugMenuField(path: "Light Loop Settings")]
        ComputeLightEvaluation = 122,
        [DebugMenuField(path: "Light Loop Settings")]
        ComputeLightVariants = 123,
        [DebugMenuField(path: "Light Loop Settings")]
        ComputeMaterialVariants = 124,
        [DebugMenuField(path: "Light Loop Settings")]
        TileAndCluster = 125,
        Reflection = 126, //set by engine, not for DebugMenu
        DebugMenuTriggerReset = 127 //set by engine for DebugMenu

        //only 128 booleans saved. For more, change the CheapBitArray used
    }

    [Serializable]
    [System.Diagnostics.DebuggerDisplay("FrameSettings overriding {mask.humanizedData}")]
    public struct FrameSettingsOverrideMask
    {
        [SerializeField]
        public CheapBitArray128 mask;
    }

   

    // The settings here are per frame settings.
    // Each camera must have its own per frame settings
    [Serializable]
    [System.Diagnostics.DebuggerDisplay("FrameSettings overriding {bitDatas.humanizedData}")]
    public partial struct FrameSettings
    {
        public static readonly FrameSettings defaultCamera = new FrameSettings()
        {
            bitDatas = new CheapBitArray128(new uint[] {
                (uint)FrameSettingsField.Shadow,
                (uint)FrameSettingsField.ContactShadow,
                (uint)FrameSettingsField.ShadowMask,
                (uint)FrameSettingsField.SSAO,
                (uint)FrameSettingsField.SubsurfaceScattering,
                (uint)FrameSettingsField.Transmission,   // Caution: this is only for debug, it doesn't save the cost of Transmission execution
                (uint)FrameSettingsField.AtmosphericScaterring,
                (uint)FrameSettingsField.Volumetrics,
                (uint)FrameSettingsField.ReprojectionForVolumetrics,
                (uint)FrameSettingsField.LightLayers,
                (uint)FrameSettingsField.ShaderLitMode, //deffered ; enum with only two value saved as a bool
                (uint)FrameSettingsField.TransparentPrepass,
                (uint)FrameSettingsField.TransparentPostpass,
                (uint)FrameSettingsField.MotionVectors, // Enable/disable whole motion vectors pass (Camera + Object).
                (uint)FrameSettingsField.ObjectMotionVectors,
                (uint)FrameSettingsField.Decals,
                (uint)FrameSettingsField.RoughRefraction, // Depends on DepthPyramid - If not enable, just do a copy of the scene color (?) - how to disable rough refraction ?
                (uint)FrameSettingsField.Distortion,
                (uint)FrameSettingsField.Postprocess,
                (uint)FrameSettingsField.OpaqueObjects,
                (uint)FrameSettingsField.TransparentObjects,
                (uint)FrameSettingsField.RealtimePlanarReflection,
                (uint)FrameSettingsField.AsyncCompute,
                (uint)FrameSettingsField.LightListAsync,
                (uint)FrameSettingsField.SSRAsync,
                (uint)FrameSettingsField.SSRAsync,
                (uint)FrameSettingsField.SSAOAsync,
                (uint)FrameSettingsField.ContactShadowsAsync,
                (uint)FrameSettingsField.VolumeVoxelizationsAsync,
                (uint)FrameSettingsField.TileAndCluster,
                (uint)FrameSettingsField.ComputeLightEvaluation,
                (uint)FrameSettingsField.ComputeLightVariants,
                (uint)FrameSettingsField.ComputeMaterialVariants,
                (uint)FrameSettingsField.FptlForForwardOpaque,
                (uint)FrameSettingsField.BigTilePrepass,
            })
        };
        public static readonly FrameSettings defaultRealtimeReflectionProbe = new FrameSettings()
        {
            bitDatas = new CheapBitArray128(new uint[] {
                (uint)FrameSettingsField.Shadow,
                //(uint)FrameSettingsField.ContactShadow,
                //(uint)FrameSettingsField.ShadowMask,
                //(uint)FrameSettingsField.SSAO,
                (uint)FrameSettingsField.SubsurfaceScattering,
                (uint)FrameSettingsField.Transmission,   // Caution: this is only for debug, it doesn't save the cost of Transmission execution
                //(uint)FrameSettingsField.AtmosphericScaterring,
                (uint)FrameSettingsField.Volumetrics,
                (uint)FrameSettingsField.ReprojectionForVolumetrics,
                (uint)FrameSettingsField.LightLayers,
                (uint)FrameSettingsField.ShaderLitMode, //deffered ; enum with only two value saved as a bool
                (uint)FrameSettingsField.TransparentPrepass,
                (uint)FrameSettingsField.TransparentPostpass,
                (uint)FrameSettingsField.MotionVectors, // Enable/disable whole motion vectors pass (Camera + Object).
                (uint)FrameSettingsField.ObjectMotionVectors,
                (uint)FrameSettingsField.Decals,
                //(uint)FrameSettingsField.RoughRefraction, // Depends on DepthPyramid - If not enable, just do a copy of the scene color (?) - how to disable rough refraction ?
                //(uint)FrameSettingsField.Distortion,
                //(uint)FrameSettingsField.Postprocess,
                (uint)FrameSettingsField.OpaqueObjects,
                (uint)FrameSettingsField.TransparentObjects,
                (uint)FrameSettingsField.RealtimePlanarReflection,
                (uint)FrameSettingsField.AsyncCompute,
                (uint)FrameSettingsField.LightListAsync,
                (uint)FrameSettingsField.SSRAsync,
                (uint)FrameSettingsField.SSRAsync,
                (uint)FrameSettingsField.SSAOAsync,
                (uint)FrameSettingsField.ContactShadowsAsync,
                (uint)FrameSettingsField.VolumeVoxelizationsAsync,
                (uint)FrameSettingsField.TileAndCluster,
                (uint)FrameSettingsField.ComputeLightEvaluation,
                (uint)FrameSettingsField.ComputeLightVariants,
                (uint)FrameSettingsField.ComputeMaterialVariants,
                (uint)FrameSettingsField.FptlForForwardOpaque,
                (uint)FrameSettingsField.BigTilePrepass,
            })
        };
        public static readonly FrameSettings defaultCustomOrBakeReflectionProbe = defaultCamera;

        [SerializeField]
        CheapBitArray128 bitDatas;
        
        public LitShaderMode shaderLitMode
        {
            get => bitDatas[(uint)FrameSettingsField.ShaderLitMode] ? LitShaderMode.Deferred : LitShaderMode.Forward;
            set => bitDatas[(uint)FrameSettingsField.ShaderLitMode] = value == LitShaderMode.Deferred;
        }
        public bool shadow
        {
            get => bitDatas[(uint)FrameSettingsField.Shadow];
            set => bitDatas[(uint)FrameSettingsField.Shadow] = value;
        }
        public bool contactShadows
        {
            get => bitDatas[(uint)FrameSettingsField.ContactShadow];
            set => bitDatas[(uint)FrameSettingsField.ContactShadow] = value;
        }
        public bool shadowMask
        {
            get => bitDatas[(uint)FrameSettingsField.ShadowMask];
            set => bitDatas[(uint)FrameSettingsField.ShadowMask] = value;
        }
        public bool ssr
        {
            get => bitDatas[(uint)FrameSettingsField.SSR];
            set => bitDatas[(uint)FrameSettingsField.SSR] = value;
        }
        public bool ssao
        {
            get => bitDatas[(uint)FrameSettingsField.SSAO];
            set => bitDatas[(uint)FrameSettingsField.SSAO] = value;
        }
        public bool subsurfaceScattering
        {
            get => bitDatas[(uint)FrameSettingsField.SubsurfaceScattering];
            set => bitDatas[(uint)FrameSettingsField.SubsurfaceScattering] = value;
        }
        public bool transmission
        {
            get => bitDatas[(uint)FrameSettingsField.Transmission];
            set => bitDatas[(uint)FrameSettingsField.Transmission] = value;
        }
        public bool atmosphericScattering
        {
            get => bitDatas[(uint)FrameSettingsField.AtmosphericScaterring];
            set => bitDatas[(uint)FrameSettingsField.AtmosphericScaterring] = value;
        }
        public bool volumetrics
        {
            get => bitDatas[(uint)FrameSettingsField.Volumetrics];
            set => bitDatas[(uint)FrameSettingsField.Volumetrics] = value;
        }
        public bool reprojectionForVolumetrics
        {
            get => bitDatas[(uint)FrameSettingsField.ReprojectionForVolumetrics];
            set => bitDatas[(uint)FrameSettingsField.ReprojectionForVolumetrics] = value;
        }
        public bool lightLayers
        {
            get => bitDatas[(uint)FrameSettingsField.LightLayers];
            set => bitDatas[(uint)FrameSettingsField.LightLayers] = value;
        }
        public bool depthPrepassWithDeferredRendering
        {
            get => bitDatas[(uint)FrameSettingsField.DepthPrepassWithDeferredRendering];
            set => bitDatas[(uint)FrameSettingsField.DepthPrepassWithDeferredRendering] = value;
        }
        public bool transparentPrepass
        {
            get => bitDatas[(uint)FrameSettingsField.TransparentPrepass];
            set => bitDatas[(uint)FrameSettingsField.TransparentPrepass] = value;
        }
        public bool motionVectors
        {
            get => bitDatas[(uint)FrameSettingsField.MotionVectors];
            set => bitDatas[(uint)FrameSettingsField.MotionVectors] = value;
        }
        public bool objectMotionVectors
        {
            get => bitDatas[(uint)FrameSettingsField.ObjectMotionVectors];
            set => bitDatas[(uint)FrameSettingsField.ObjectMotionVectors] = value;
        }
        public bool decals
        {
            get => bitDatas[(uint)FrameSettingsField.Decals];
            set => bitDatas[(uint)FrameSettingsField.Decals] = value;
        }
        public bool roughRefraction
        {
            get => bitDatas[(uint)FrameSettingsField.RoughRefraction];
            set => bitDatas[(uint)FrameSettingsField.RoughRefraction] = value;
        }
        public bool transparentPostpass
        {
            get => bitDatas[(uint)FrameSettingsField.TransparentPostpass];
            set => bitDatas[(uint)FrameSettingsField.TransparentPostpass] = value;
        }
        public bool distortion
        {
            get => bitDatas[(uint)FrameSettingsField.Distortion];
            set => bitDatas[(uint)FrameSettingsField.Distortion] = value;
        }
        public bool postprocess
        {
            get => bitDatas[(uint)FrameSettingsField.Postprocess];
            set => bitDatas[(uint)FrameSettingsField.Postprocess] = value;
        }
        public bool opaqueObjects
        {
            get => bitDatas[(uint)FrameSettingsField.OpaqueObjects];
            set => bitDatas[(uint)FrameSettingsField.OpaqueObjects] = value;
        }
        public bool transparentObjects
        {
            get => bitDatas[(uint)FrameSettingsField.TransparentObjects];
            set => bitDatas[(uint)FrameSettingsField.TransparentObjects] = value;
        }
        public bool realtimePlanarReflection
        {
            get => bitDatas[(uint)FrameSettingsField.RealtimePlanarReflection];
            set => bitDatas[(uint)FrameSettingsField.RealtimePlanarReflection] = value;
        }
        public bool asyncCompute
        {
            get => bitDatas[(uint)FrameSettingsField.AsyncCompute];
            set => bitDatas[(uint)FrameSettingsField.AsyncCompute] = value;
        }
        public bool lightListAsync
        {
            get => bitDatas[(uint)FrameSettingsField.LightListAsync];
            set => bitDatas[(uint)FrameSettingsField.LightListAsync] = value;
        }
        public bool ssaoAsync
        {
            get => bitDatas[(uint)FrameSettingsField.SSAOAsync];
            set => bitDatas[(uint)FrameSettingsField.SSAOAsync] = value;
        }
        public bool ssrAsync
        {
            get => bitDatas[(uint)FrameSettingsField.SSRAsync];
            set => bitDatas[(uint)FrameSettingsField.SSRAsync] = value;
        }
        public bool contactShadowsAsync
        {
            get => bitDatas[(uint)FrameSettingsField.ContactShadowsAsync];
            set => bitDatas[(uint)FrameSettingsField.ContactShadowsAsync] = value;
        }
        public bool volumeVoxelizationAsync
        {
            get => bitDatas[(uint)FrameSettingsField.VolumeVoxelizationsAsync];
            set => bitDatas[(uint)FrameSettingsField.VolumeVoxelizationsAsync] = value;
        }
        public bool msaa
        {
            get => bitDatas[(uint)FrameSettingsField.MSAA];
            set => bitDatas[(uint)FrameSettingsField.MSAA] = value;
        }
        public bool tileAndCluster
        {
            get => bitDatas[(uint)FrameSettingsField.TileAndCluster];
            set => bitDatas[(uint)FrameSettingsField.TileAndCluster] = value;
        }
        public bool computeLightEvaluation
        {
            get => bitDatas[(uint)FrameSettingsField.ComputeLightEvaluation];
            set => bitDatas[(uint)FrameSettingsField.ComputeLightEvaluation] = value;
        }
        public bool computeLightVariants
        {
            get => bitDatas[(uint)FrameSettingsField.ComputeLightVariants];
            set => bitDatas[(uint)FrameSettingsField.ComputeLightVariants] = value;
        }
        public bool computeMaterialVariants
        {
            get => bitDatas[(uint)FrameSettingsField.ComputeMaterialVariants];
            set => bitDatas[(uint)FrameSettingsField.ComputeMaterialVariants] = value;
        }
        // Deferred opaque always use FPTL, forward opaque can use FPTL or cluster, transparent always use cluster
        // When MSAA is enabled, we only support cluster (Fptl is too slow with MSAA), and we don't support MSAA for deferred path (mean it is ok to keep fptl)
        public bool fptlForForwardOpaque
        {
            get => bitDatas[(uint)FrameSettingsField.FptlForForwardOpaque];
            set => bitDatas[(uint)FrameSettingsField.FptlForForwardOpaque] = value;
        }
        public bool bigTilePrepass
        {
            get => bitDatas[(uint)FrameSettingsField.BigTilePrepass];
            set => bitDatas[(uint)FrameSettingsField.BigTilePrepass] = value;
        }

        public bool fptl => shaderLitMode == LitShaderMode.Deferred || bitDatas[(uint)FrameSettingsField.FptlForForwardOpaque];
        public float specularGlobalDimmer => bitDatas[(uint)FrameSettingsField.Reflection] ? 1f : 0f;
        
        public bool BuildLightListRunsAsync() => SystemInfo.supportsAsyncCompute && bitDatas[(uint)FrameSettingsField.AsyncCompute] && bitDatas[(uint)FrameSettingsField.LightListAsync];
        public bool SSRRunsAsync() => SystemInfo.supportsAsyncCompute && bitDatas[(uint)FrameSettingsField.AsyncCompute] && bitDatas[(uint)FrameSettingsField.SSRAsync];
        public bool SSAORunsAsync() => SystemInfo.supportsAsyncCompute && bitDatas[(uint)FrameSettingsField.AsyncCompute] && bitDatas[(uint)FrameSettingsField.SSAOAsync];
        public bool ContactShadowsRunAsync() => SystemInfo.supportsAsyncCompute && bitDatas[(uint)FrameSettingsField.AsyncCompute] && bitDatas[(uint)FrameSettingsField.ContactShadowsAsync];
        public bool VolumeVoxelizationRunsAsync() => SystemInfo.supportsAsyncCompute && bitDatas[(uint)FrameSettingsField.AsyncCompute] && bitDatas[(uint)FrameSettingsField.VolumeVoxelizationsAsync];

        public static void Override(ref FrameSettings overridedFrameSettings, FrameSettings overridingFrameSettings, FrameSettingsOverrideMask frameSettingsOverideMask)
        {
            //quick override of all booleans
            overridedFrameSettings.bitDatas = (overridingFrameSettings.bitDatas & frameSettingsOverideMask.mask) | (~frameSettingsOverideMask.mask & overridedFrameSettings.bitDatas);

            //override remaining values here if needed


            //refresh enums for DebugMenu
            //[TODO: save this value in serialized element of DebugMenu once serialization is fixed]
            if (frameSettingsOverideMask.mask[(uint)FrameSettingsField.ShaderLitMode])
            {
                // the property update the enum index each time. Force to pass by that part of the code when overriding it
                overridedFrameSettings.shaderLitMode = overridedFrameSettings.shaderLitMode;
            }
        }
        
        public static void Sanitize(ref FrameSettings sanitazedFrameSettings, Camera camera, RenderPipelineSettings renderPipelineSettings)
        {
            bool reflection = camera.cameraType == CameraType.Reflection;
            bool preview = HDUtils.IsRegularPreviewCamera(camera);
            bool sceneViewFog = CoreUtils.IsSceneViewFogEnabled(camera);
            bool stereo = camera.stereoEnabled;

            // When rendering reflection probe we disable specular as it is view dependent
            sanitazedFrameSettings.bitDatas[(uint)FrameSettingsField.Reflection] = !reflection;

            // We have to fall back to forward-only rendering when scene view is using wireframe rendering mode
            // as rendering everything in wireframe + deferred do not play well together
            if (GL.wireframe || stereo) //force forward mode for wireframe
            {
                // Stereo deferred rendering still has the following problems:
                // VR TODO: Dispatch tile light-list compute per-eye
                // VR TODO: Update compute lighting shaders for stereo
                sanitazedFrameSettings.shaderLitMode = LitShaderMode.Forward;
            }
            else
            {
                switch (renderPipelineSettings.supportedLitShaderMode)
                {
                    case RenderPipelineSettings.SupportedLitShaderMode.ForwardOnly:
                        sanitazedFrameSettings.shaderLitMode = LitShaderMode.Forward;
                        break;
                    case RenderPipelineSettings.SupportedLitShaderMode.DeferredOnly:
                        sanitazedFrameSettings.shaderLitMode = LitShaderMode.Deferred;
                        break;
                    case RenderPipelineSettings.SupportedLitShaderMode.Both:
                        //nothing to do: keep previous value
                        break;
                }
            }

            sanitazedFrameSettings.bitDatas[(uint)FrameSettingsField.Shadow] &= !preview;
            sanitazedFrameSettings.bitDatas[(uint)FrameSettingsField.ShadowMask] &= renderPipelineSettings.supportShadowMask && !preview;
            sanitazedFrameSettings.bitDatas[(uint)FrameSettingsField.ContactShadow] &= !preview;

            //MSAA only supported in forward
            // TODO: The work will be implemented piecemeal to support all passes
            bool msaa = sanitazedFrameSettings.bitDatas[(uint)FrameSettingsField.MSAA] &= renderPipelineSettings.supportMSAA && sanitazedFrameSettings.shaderLitMode == LitShaderMode.Forward;

            // VR TODO: The work will be implemented piecemeal to support all passes
            // No recursive reflections
            sanitazedFrameSettings.bitDatas[(uint)FrameSettingsField.SSR] &= !reflection && renderPipelineSettings.supportSSR && !msaa && !preview && stereo;
            sanitazedFrameSettings.bitDatas[(uint)FrameSettingsField.SSAO] &= renderPipelineSettings.supportSSAO && !preview;
            sanitazedFrameSettings.bitDatas[(uint)FrameSettingsField.SubsurfaceScattering] &= !reflection && renderPipelineSettings.supportSubsurfaceScattering;

            // We must take care of the scene view fog flags in the editor
            bool atmosphericScattering = sanitazedFrameSettings.bitDatas[(uint)FrameSettingsField.AtmosphericScaterring] &= !sceneViewFog && !preview;

            // Volumetric are disabled if there is no atmospheric scattering
            sanitazedFrameSettings.bitDatas[(uint)FrameSettingsField.Volumetrics] &= renderPipelineSettings.supportVolumetrics && atmosphericScattering; //&& !preview induced by atmospheric scattering
            sanitazedFrameSettings.bitDatas[(uint)FrameSettingsField.ReprojectionForVolumetrics] &= !preview;

            sanitazedFrameSettings.bitDatas[(uint)FrameSettingsField.LightLayers] &= renderPipelineSettings.supportLightLayers && !preview;

            // Planar and real time cubemap doesn't need post process and render in FP16
            bool postprocess = sanitazedFrameSettings.bitDatas[(uint)FrameSettingsField.Postprocess] &= !reflection && !preview;

            sanitazedFrameSettings.bitDatas[(uint)FrameSettingsField.TransparentPrepass] &= renderPipelineSettings.supportTransparentDepthPrepass && !preview;

            // VR TODO: The work will be implemented piecemeal to support all passes
            // VR TODO: check why '=' and not '&=' and if we can merge these lines
            bool motionVector;
            if (stereo)
                motionVector = sanitazedFrameSettings.bitDatas[(uint)FrameSettingsField.MotionVectors] = postprocess && !msaa && !preview;
            else
                motionVector = sanitazedFrameSettings.bitDatas[(uint)FrameSettingsField.MotionVectors] &= !reflection && renderPipelineSettings.supportMotionVectors && !preview;

            // Object motion vector are disabled if motion vector are disabled
            sanitazedFrameSettings.bitDatas[(uint)FrameSettingsField.ObjectMotionVectors] &= motionVector && !preview;
            sanitazedFrameSettings.bitDatas[(uint)FrameSettingsField.Decals] &= renderPipelineSettings.supportDecals && !preview;
            sanitazedFrameSettings.bitDatas[(uint)FrameSettingsField.TransparentPostpass] &= renderPipelineSettings.supportTransparentDepthPostpass && !preview;
            sanitazedFrameSettings.bitDatas[(uint)FrameSettingsField.Distortion] &= !reflection && renderPipelineSettings.supportDistortion && !msaa && !preview;

            bool async = sanitazedFrameSettings.bitDatas[(uint)FrameSettingsField.AsyncCompute] &= SystemInfo.supportsAsyncCompute;
            sanitazedFrameSettings.bitDatas[(uint)FrameSettingsField.LightListAsync] &= async;
            sanitazedFrameSettings.bitDatas[(uint)FrameSettingsField.SSRAsync] &= async;
            sanitazedFrameSettings.bitDatas[(uint)FrameSettingsField.SSAOAsync] &= async;
            sanitazedFrameSettings.bitDatas[(uint)FrameSettingsField.ContactShadowsAsync] &= async;
            sanitazedFrameSettings.bitDatas[(uint)FrameSettingsField.VolumeVoxelizationsAsync] &= async;

            // Deferred opaque are always using Fptl. Forward opaque can use Fptl or Cluster, transparent use cluster.
            // When MSAA is enabled we disable Fptl as it become expensive compare to cluster
            // In HD, MSAA is only supported for forward only rendering, no MSAA in deferred mode (for code complexity reasons)
            // Disable FPTL for stereo for now
            bool fptlForwardOpaque = sanitazedFrameSettings.bitDatas[(uint)FrameSettingsField.FptlForForwardOpaque] &= !msaa && !XRGraphics.enabled;
        }
        
        public static void AggregateFrameSettings(ref FrameSettings aggregatedFrameSettings, Camera camera, HDAdditionalCameraData additionalData, HDRenderPipelineAsset hdrpAsset)
        {
            aggregatedFrameSettings = hdrpAsset.GetDefaultFrameSettings(additionalData.defaultFrameSettings);
            if (additionalData && additionalData.customRenderingSettings)
                Override(ref aggregatedFrameSettings, additionalData.renderingPathCustomFrameSettings, additionalData.renderingPathCustomOverrideFrameSettings);
            Sanitize(ref aggregatedFrameSettings, camera, hdrpAsset.GetRenderPipelineSettings());
        }
        
        public static bool operator ==(FrameSettings a, FrameSettings b) => a.bitDatas == b.bitDatas;
        public static bool operator !=(FrameSettings a, FrameSettings b) => a.bitDatas != b.bitDatas;
        public override bool Equals(object obj) => (obj is FrameSettings) && bitDatas.Equals((FrameSettings)obj);
        public override int GetHashCode() => -1690259335 + bitDatas.GetHashCode();
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
            history.defaultType = additionalData.defaultFrameSettings;
            aggregatedFrameSettings = hdrpAsset.GetDefaultFrameSettings(additionalData.defaultFrameSettings);
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
                switch (history.debug.shaderLitMode)
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
                        new DebugUI.EnumField { displayName = "Lit Shader Mode", getter = () => (int)persistant.debug.shaderLitMode, setter = value => persistant.debug.shaderLitMode = (LitShaderMode)value, autoEnum = typeof(LitShaderMode), getIndex = () => persistant.m_LitShaderModeEnumIndex, setIndex = value => persistant.m_LitShaderModeEnumIndex = value },
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
