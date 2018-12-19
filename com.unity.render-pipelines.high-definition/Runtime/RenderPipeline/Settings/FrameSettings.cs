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
        [DebugMenuField(path: "Rendering Pass")]
        TransparentPrepass = 20,
        [DebugMenuField(path: "Rendering Pass")]
        TransparentPostpass = 21,
        [DebugMenuField(path: "Rendering Pass")]
        MotionVectors = 22,
        [DebugMenuField(path: "Rendering Pass")]
        ObjectMotionVectors = 23,
        [DebugMenuField(path: "Rendering Pass")]
        Decals = 24,
        [DebugMenuField(path: "Rendering Pass")]
        RoughRefraction = 25,
        [DebugMenuField(path: "Rendering Pass")]
        Distortion = 26,
        [DebugMenuField(path: "Rendering Pass")]
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
        Fptl = 126, //set by engine, not for DebugMenu
        SpecularGlobalDimmer = 127, //set by engine, not for DebugMenu

        //only 128 booleans saved. For more, change the CheapBitArray used
    }

    public struct FrameSettingsOverrideMask
    {
        [SerializeField]
        public CheapBitArray128 mask;
    }
    
    // The settings here are per frame settings.
    // Each camera must have its own per frame settings
    [Serializable]
    [System.Diagnostics.DebuggerDisplay("FrameSettings overriding {overrides.ToString(\"X\")}")]
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
                (uint)FrameSettingsField.Fptl,
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
                (uint)FrameSettingsField.Fptl,
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
        /// <summary>Setup by system</summary>
        public bool fptl
        {
            get => bitDatas[(uint)FrameSettingsField.Fptl];
            set => bitDatas[(uint)FrameSettingsField.Fptl] = value;
        }
        /// <summary>Setup by system</summary>
        public float specularGlobalDimmer => bitDatas[(uint)FrameSettingsField.SpecularGlobalDimmer] ? 1f : 0f;

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
            sanitazedFrameSettings.bitDatas[(uint)FrameSettingsField.SpecularGlobalDimmer] = !reflection;

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

            // If Deferred, enable Fptl. If we are forward renderer only and not using Fptl for forward opaque, disable Fptl
            sanitazedFrameSettings.bitDatas[(uint)FrameSettingsField.Fptl] &= sanitazedFrameSettings.shaderLitMode == LitShaderMode.Deferred || fptlForwardOpaque;
        }
        
        internal static Dictionary<Camera, FrameSettings> debugFrameSettings = new Dictionary<Camera, FrameSettings>();
        public static void AggregateFrameSettings(ref FrameSettings aggregatedFrameSettings, Camera camera, HDAdditionalCameraData additionalData, HDRenderPipelineAsset hdrpAsset)
        {
            if (debugFrameSettings.ContainsKey(camera))
            {
                aggregatedFrameSettings = debugFrameSettings[camera];
                return;
            }

            aggregatedFrameSettings = hdrpAsset.GetDefaultFrameSettings(additionalData.defaultFrameSettings);
            if (additionalData && additionalData.customRenderingSettings)
                Override(ref aggregatedFrameSettings, additionalData.renderingPathCustomFrameSettings, additionalData.renderingPathCustomOverrideFrameSettings);
            Sanitize(ref aggregatedFrameSettings, camera, hdrpAsset.GetRenderPipelineSettings());
        }

        public bool BuildLightListRunsAsync()
        {
            return SystemInfo.supportsAsyncCompute && bitDatas[(uint)FrameSettingsField.AsyncCompute] && bitDatas[(uint)FrameSettingsField.LightListAsync];
        }

        public bool SSRRunsAsync()
        {
            return SystemInfo.supportsAsyncCompute && bitDatas[(uint)FrameSettingsField.AsyncCompute] && bitDatas[(uint)FrameSettingsField.SSRAsync];
        }

        public bool SSAORunsAsync()
        {
            return SystemInfo.supportsAsyncCompute && bitDatas[(uint)FrameSettingsField.AsyncCompute] && bitDatas[(uint)FrameSettingsField.SSAOAsync];
        }

        public bool ContactShadowsRunAsync()
        {
            return SystemInfo.supportsAsyncCompute && bitDatas[(uint)FrameSettingsField.AsyncCompute] && bitDatas[(uint)FrameSettingsField.ContactShadowsAsync];
        }

        public bool VolumeVoxelizationRunsAsync()
        {
            return SystemInfo.supportsAsyncCompute && bitDatas[(uint)FrameSettingsField.AsyncCompute] && bitDatas[(uint)FrameSettingsField.VolumeVoxelizationsAsync];
        }
    }

    public class DebugFrameSettings
    {
        public readonly FrameSettingsRenderType type;
        public Camera camera;
        public HDAdditionalCameraData additionalCameraData;
        public HDProbe probe;
        public FrameSettings debugOverride;

        FrameSettings @default;
        FrameSettings customOverride;
        FrameSettingsOverrideMask customOverrideMask;
        FrameSettings Sanitazed;

        public DebugFrameSettings(FrameSettingsRenderType type, Camera camera = null, HDAdditionalCameraData additionalCameraData = null, HDProbe probe = null)
        {
            this.type = type;
            switch(type)
            {
                case FrameSettingsRenderType.Camera:
                    Assertions.Assert.IsNotNull(camera);
                    Assertions.Assert.IsNotNull(additionalCameraData);
                    probe = null;
                    break;
                case FrameSettingsRenderType.CustomOrBakedReflection:
                case FrameSettingsRenderType.RealtimeReflection:
                    Assertions.Assert.IsNotNull(probe);
                    camera = null;
                    additionalCameraData = null;
                    break;
                default:
                    throw new ArgumentException("Unknown FrameSettingsRenderType");
            }
            debugOverride = Sanitazed = customOverride = @default;
            this.camera = camera;
            this.additionalCameraData = additionalCameraData;
            this.probe = probe;
            customOverrideMask = new FrameSettingsOverrideMask();

            //first init: fill all step
            Update();
            //first init: set debug override to no override
            Reset();
        }

        public void Reset()
        {
            debugOverride = Sanitazed;
        }

        public void Update()
        {
            var hdrpAsset = ((HDRenderPipeline)RenderPipelineManager.currentPipeline)?.asset;
            if (hdrpAsset == null)
                return;

            //duplicate of FrameSettings.Aggregate witgh step saves
            @default = hdrpAsset.GetDefaultFrameSettings(FrameSettingsRenderType.Camera);
            customOverride = @default;
            if (additionalCameraData.customRenderingSettings)
                FrameSettings.Override(ref customOverride, additionalCameraData.renderingPathCustomFrameSettings, customOverrideMask = additionalCameraData.renderingPathCustomOverrideFrameSettings);
            Sanitazed = customOverride;
            FrameSettings.Sanitize(ref Sanitazed, camera, hdrpAsset.GetRenderPipelineSettings());
        }

        ref FrameSettings persistantFrameSettings
        {
            get
            {
                unsafe
                {
                    fixed (FrameSettings* pthis = &this)
                        return ref *pthis;
                }
            }
        }

        public static void RegisterDebug(string menuName, FrameSettings frameSettings)
        {
            var persistant = frameSettings.persistantFrameSettings;
            List<DebugUI.Widget> widgets = new List<DebugUI.Widget>();
            widgets.AddRange(
            new DebugUI.Widget[]
            {
                new DebugUI.Foldout
                {
                    displayName = "Rendering Passes",
                    children =
                    {
                        new DebugUI.BoolField { displayName = "Enable Transparent Prepass", getter = () => persistant.transparentPrepass, setter = value => persistant.transparentPrepass = value },
                        new DebugUI.BoolField { displayName = "Enable Transparent Postpass", getter = () => persistant.transparentPostpass, setter = value => persistant.transparentPostpass = value },
                        new DebugUI.BoolField { displayName = "Enable Motion Vectors", getter = () => persistant.motionVectors, setter = value => persistant.motionVectors = value },
                        new DebugUI.BoolField { displayName = "  Enable Object Motion Vectors", getter = () => persistant.objectMotionVectors, setter = value => persistant.objectMotionVectors = value },
                        new DebugUI.BoolField { displayName = "Enable DBuffer", getter = () => persistant.decals, setter = value => persistant.decals = value },
                        new DebugUI.BoolField { displayName = "Enable Rough Refraction", getter = () => persistant.roughRefraction, setter = value => persistant.roughRefraction = value },
                        new DebugUI.BoolField { displayName = "Enable Distortion", getter = () => persistant.distortion, setter = value => persistant.distortion = value },
                        new DebugUI.BoolField { displayName = "Enable Postprocess", getter = () => persistant.postprocess, setter = value => persistant.postprocess = value },
                    }
                },
                new DebugUI.Foldout
                {
                    displayName = "Rendering Settings",
                    children =
                    {
                        new DebugUI.EnumField { displayName = "Lit Shader Mode", getter = () => (int)persistant.shaderLitMode, setter = value => persistant.shaderLitMode = (LitShaderMode)value, autoEnum = typeof(LitShaderMode), getIndex = () => persistant.m_LitShaderModeEnumIndex, setIndex = value => persistant.m_LitShaderModeEnumIndex = value },
                        new DebugUI.BoolField { displayName = "Deferred Depth Prepass", getter = () => persistant.depthPrepassWithDeferredRendering, setter = value => persistant.depthPrepassWithDeferredRendering = value },
                        new DebugUI.BoolField { displayName = "Enable Opaque Objects", getter = () => persistant.opaqueObjects, setter = value => persistant.opaqueObjects = value },
                        new DebugUI.BoolField { displayName = "Enable Transparent Objects", getter = () => persistant.transparentObjects, setter = value => persistant.transparentObjects = value },
                        new DebugUI.BoolField { displayName = "Enable Realtime Planar Reflection", getter = () => persistant.realtimePlanarReflection, setter = value => persistant.realtimePlanarReflection = value },
                        new DebugUI.BoolField { displayName = "Enable MSAA", getter = () => persistant.msaa, setter = value => persistant.msaa = value },
                    }
                },
                new DebugUI.Foldout
                {
                    displayName = "Lighting Settings",
                    children =
                    {
                        new DebugUI.BoolField { displayName = "Enable SSR", getter = () => persistant.ssr, setter = value => persistant.ssr = value },
                        new DebugUI.BoolField { displayName = "Enable SSAO", getter = () => persistant.ssao, setter = value => persistant.ssao = value },
                        new DebugUI.BoolField { displayName = "Enable SubsurfaceScattering", getter = () => persistant.subsurfaceScattering, setter = value => persistant.subsurfaceScattering = value },
                        new DebugUI.BoolField { displayName = "Enable Transmission", getter = () => persistant.transmission, setter = value => persistant.transmission = value },
                        new DebugUI.BoolField { displayName = "Enable Shadows", getter = () => persistant.shadow, setter = value => persistant.shadow = value },
                        new DebugUI.BoolField { displayName = "Enable Contact Shadows", getter = () => persistant.contactShadows, setter = value => persistant.contactShadows = value },
                        new DebugUI.BoolField { displayName = "Enable ShadowMask", getter = () => persistant.shadowMask, setter = value => persistant.shadowMask = value },
                        new DebugUI.BoolField { displayName = "Enable Atmospheric Scattering", getter = () => persistant.atmosphericScattering, setter = value => persistant.atmosphericScattering = value },
                        new DebugUI.BoolField { displayName = "Enable Volumetrics", getter = () => persistant.volumetrics, setter = value => persistant.volumetrics = value },
                        new DebugUI.BoolField { displayName = "Enable Reprojection For Volumetrics", getter = () => persistant.reprojectionForVolumetrics, setter = value => persistant.reprojectionForVolumetrics = value },
                        new DebugUI.BoolField { displayName = "Enable LightLayers", getter = () => persistant.lightLayers, setter = value => persistant.lightLayers = value },
                    }
                },
                new DebugUI.Foldout
                {
                    displayName = "Async Compute Settings",
                    children =
                    {
                        new DebugUI.BoolField { displayName = "Enable Async Compute", getter = () => persistant.asyncCompute, setter = value => persistant.asyncCompute = value },
                        new DebugUI.BoolField { displayName = "Run Build Light List Async", getter = () => persistant.lightListAsync, setter = value => persistant.lightListAsync = value },
                        new DebugUI.BoolField { displayName = "Run SSR Async", getter = () => persistant.ssrAsync, setter = value => persistant.ssrAsync = value },
                        new DebugUI.BoolField { displayName = "Run SSAO Async", getter = () => persistant.ssaoAsync, setter = value => persistant.ssaoAsync = value },
                        new DebugUI.BoolField { displayName = "Run Contact Shadows Async", getter = () => persistant.contactShadowsAsync, setter = value => persistant.contactShadowsAsync = value },
                        new DebugUI.BoolField { displayName = "Run Volume Voxelization Async", getter = () => persistant.volumeVoxelizationAsync, setter = value => persistant.volumeVoxelizationAsync = value },
                    }
                },
                new DebugUI.Foldout
                {
                    displayName = "Light Loop Settings",
                    children =
                    {
                        // Uncomment if you re-enable LIGHTLOOP_SINGLE_PASS multi_compile in lit*.shader
                        //new DebugUI.BoolField { displayName = "Enable Tile/Cluster", getter = () => persistant.enableTileAndCluster, setter = value => persistant.enableTileAndCluster = value },
                        new DebugUI.BoolField { displayName = "Enable Fptl for Forward Opaque", getter = () => persistant.fptlForForwardOpaque, setter = value => persistant.fptlForForwardOpaque = value },
                        new DebugUI.BoolField { displayName = "Enable Big Tile", getter = () => persistant.bigTilePrepass, setter = value => persistant.bigTilePrepass = value },
                        new DebugUI.BoolField { displayName = "Enable Compute Lighting", getter = () => persistant.computeLightEvaluation, setter = value => persistant.computeLightEvaluation = value },
                        new DebugUI.BoolField { displayName = "Enable Light Classification", getter = () => persistant.computeLightVariants, setter = value => persistant.computeLightVariants = value },
                        new DebugUI.BoolField { displayName = "Enable Material Classification", getter = () => persistant.computeMaterialVariants, setter = value => persistant.computeMaterialVariants = value }
                    }
                }
            });

            var panel = DebugManager.instance.GetPanel(menuName, true);
            panel.children.Add(widgets.ToArray());
        }

        public static void UnRegisterDebug(string menuName)
        {
            DebugManager.instance.RemovePanel(menuName);
        }
    }
}
