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
        //set by engine, not for DebugMenu
        Fptl = 126, 

        //only 128 booleans saved. For more, change the CheapBoolArray used
    }

    public struct FrameSettingsOverrideMask
    {
        [SerializeField]
        public CheapBoolArray128 mask;
    }

    // The settings here are per frame settings.
    // Each camera must have its own per frame settings
    [Serializable]
    [System.Diagnostics.DebuggerDisplay("FrameSettings overriding {overrides.ToString(\"X\")}")]
    public partial struct FrameSettings
    {
        //static Dictionary<FrameSettingsField, Action<FrameSettings, FrameSettings>> s_Overrides = new Dictionary<FrameSettingsField, Action<FrameSettings, FrameSettings>>
        //{
        //    {FrameSettingsField.Shadow, (a, b) => { a.shadow = b.shadow; } },
        //    {FrameSettingsField.ContactShadow, (a, b) => { a.contactShadows = b.contactShadows; } },
        //    {FrameSettingsField.ShadowMask, (a, b) => { a.shadowMask = b.shadowMask; } },
        //    {FrameSettingsField.SSR, (a, b) => { a.ssr = b.ssr; } },
        //    {FrameSettingsField.SSAO, (a, b) => { a.ssao = b.ssao; } },
        //    {FrameSettingsField.SubsurfaceScattering, (a, b) => { a.subsurfaceScattering = b.subsurfaceScattering; } },
        //    {FrameSettingsField.Transmission, (a, b) => { a.transmission = b.transmission; } },
        //    {FrameSettingsField.AtmosphericScaterring, (a, b) => { a.atmosphericScattering = b.atmosphericScattering; } },
        //    {FrameSettingsField.Volumetrics, (a, b) => { a.volumetrics = b.volumetrics; } },
        //    {FrameSettingsField.ReprojectionForVolumetrics, (a, b) => { a.reprojectionForVolumetrics = b.reprojectionForVolumetrics; } },
        //    {FrameSettingsField.LightLayers, (a, b) => { a.lightLayers = b.lightLayers; } },
        //    {FrameSettingsField.MSAA, (a, b) => { a.msaa = b.msaa; } },
        //    {FrameSettingsField.TransparentPrepass, (a, b) => { a.transparentPrepass = b.transparentPrepass; } },
        //    {FrameSettingsField.TransparentPostpass, (a, b) => { a.transparentPostpass = b.transparentPostpass; } },
        //    {FrameSettingsField.MotionVectors, (a, b) => { a.motionVectors = b.motionVectors; } },
        //    {FrameSettingsField.ObjectMotionVectors, (a, b) => { a.objectMotionVectors = b.objectMotionVectors; } },
        //    {FrameSettingsField.Decals, (a, b) => { a.decals = b.decals; } },
        //    {FrameSettingsField.RoughRefraction, (a, b) => { a.roughRefraction = b.roughRefraction; } },
        //    {FrameSettingsField.Distortion, (a, b) => { a.distortion = b.distortion; } },
        //    {FrameSettingsField.Postprocess, (a, b) => { a.postprocess = b.postprocess; } },
        //    {FrameSettingsField.ShaderLitMode, (a, b) => { a.shaderLitMode = b.shaderLitMode; } },
        //    {FrameSettingsField.DepthPrepassWithDeferredRendering, (a, b) => { a.depthPrepassWithDeferredRendering = b.depthPrepassWithDeferredRendering; } },
        //    {FrameSettingsField.AsyncCompute, (a, b) => { a.asyncCompute = b.asyncCompute; } },
        //    {FrameSettingsField.OpaqueObjects, (a, b) => { a.opaqueObjects = b.opaqueObjects; } },
        //    {FrameSettingsField.TransparentObjects, (a, b) => { a.transparentObjects = b.transparentObjects; } },
        //    {FrameSettingsField.RealtimePlanarReflection, (a, b) => { a.realtimePlanarReflection = b.realtimePlanarReflection; } },
        //    {FrameSettingsField.LightListAsync, (a, b) => { a.lightListAsync = b.lightListAsync; } },
        //    {FrameSettingsField.SSRAsync, (a, b) => { a.ssrAsync = b.ssrAsync; } },
        //    {FrameSettingsField.SSAOAsync, (a, b) => { a.ssaoAsync = b.ssaoAsync; } },
        //    {FrameSettingsField.ContactShadowsAsync, (a, b) => { a.contactShadowsAsync = b.contactShadowsAsync; } },
        //    {FrameSettingsField.VolumeVoxelizationsAsync, (a, b) => { a.volumeVoxelizationAsync = b.volumeVoxelizationAsync; } },
        //    {FrameSettingsField.FptlForForwardOpaque, (a, b) => { a.fptlForForwardOpaque = b.fptlForForwardOpaque; } },
        //    {FrameSettingsField.BigTilePrepass, (a, b) => { a.bigTilePrepass = b.bigTilePrepass; } },
        //    {FrameSettingsField.ComputeLightEvaluation, (a, b) => { a.computeLightEvaluation = b.computeLightEvaluation; } },
        //    {FrameSettingsField.ComputeLightVariants, (a, b) => { a.computeLightVariants = b.computeLightVariants; } },
        //    {FrameSettingsField.ComputeMaterialVariants, (a, b) => { a.computeMaterialVariants = b.computeMaterialVariants; } },
        //    {FrameSettingsField.TileAndCluster, (a, b) => { a.tileAndCluster = b.tileAndCluster; } },
        //};

        [SerializeField]
        CheapBoolArray128 boolData;

        // Setup by system
        // No need to override the two dimmer. They are stocked here for conveniance only.
        // No need to serialize them either -> Property
        public float diffuseGlobalDimmer { get; private set; }
        public float specularGlobalDimmer { get; private set; }

        //saved enum fields for when repainting Debug Menu
        // TODO DebugMenu: move this to serialized debug menu once fixed
        int m_LitShaderModeEnumIndex;

        public static readonly FrameSettings defaultCamera = new FrameSettings()
        {
            boolData = new CheapBoolArray128(new uint[] {
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
            }),
            diffuseGlobalDimmer = 1f,
            specularGlobalDimmer = 1f,
            m_LitShaderModeEnumIndex = 1 //match Deferred index
        };

        public static readonly FrameSettings defaultRealtimeReflectionProbe = new FrameSettings()
        {
            boolData = new CheapBoolArray128(new uint[] {
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
            }),
            diffuseGlobalDimmer = 1f,
            specularGlobalDimmer = 1f,
            m_LitShaderModeEnumIndex = 1 //match Deferred index
        };
        public static readonly FrameSettings defaultCustomOrBakeReflectionProbe = new FrameSettings()
        {
            boolData = new CheapBoolArray128(new uint[] {
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
            }),
            diffuseGlobalDimmer = 1f,
            specularGlobalDimmer = 1f,
            m_LitShaderModeEnumIndex = 1 //match Deferred index
        };

        public LitShaderMode shaderLitMode
        {
            get => boolData[(uint)FrameSettingsField.ShaderLitMode] ? LitShaderMode.Deferred : LitShaderMode.Forward;
            set
            {
                // actually, we need to sync up changes done in the debug menu too
                // TODO DebugMenu : store this value in serialization of of debug menu once its fixed
                switch (value)
                {
                    case LitShaderMode.Forward:
                        m_LitShaderModeEnumIndex = 0;
                        break;
                    case LitShaderMode.Deferred:
                        m_LitShaderModeEnumIndex = 1;
                        break;
                    default:
                        throw new ArgumentException("Unknown LitShaderMode");
                }

                boolData[(uint)FrameSettingsField.ShaderLitMode] = value == LitShaderMode.Deferred;
            }
        }
        public bool shadow
        {
            get => boolData[(uint)FrameSettingsField.Shadow];
            set => boolData[(uint)FrameSettingsField.Shadow] = value;
        }
        public bool contactShadows
        {
            get => boolData[(uint)FrameSettingsField.ContactShadow];
            set => boolData[(uint)FrameSettingsField.ContactShadow] = value;
        }
        public bool shadowMask
        {
            get => boolData[(uint)FrameSettingsField.ShadowMask];
            set => boolData[(uint)FrameSettingsField.ShadowMask] = value;
        }
        public bool ssr
        {
            get => boolData[(uint)FrameSettingsField.SSR];
            set => boolData[(uint)FrameSettingsField.SSR] = value;
        }
        public bool ssao
        {
            get => boolData[(uint)FrameSettingsField.SSAO];
            set => boolData[(uint)FrameSettingsField.SSAO] = value;
        }
        public bool subsurfaceScattering
        {
            get => boolData[(uint)FrameSettingsField.SubsurfaceScattering];
            set => boolData[(uint)FrameSettingsField.SubsurfaceScattering] = value;
        }
        public bool transmission
        {
            get => boolData[(uint)FrameSettingsField.Transmission];
            set => boolData[(uint)FrameSettingsField.Transmission] = value;
        }
        public bool atmosphericScattering
        {
            get => boolData[(uint)FrameSettingsField.AtmosphericScaterring];
            set => boolData[(uint)FrameSettingsField.AtmosphericScaterring] = value;
        }
        public bool volumetrics
        {
            get => boolData[(uint)FrameSettingsField.Volumetrics];
            set => boolData[(uint)FrameSettingsField.Volumetrics] = value;
        }
        public bool reprojectionForVolumetrics
        {
            get => boolData[(uint)FrameSettingsField.ReprojectionForVolumetrics];
            set => boolData[(uint)FrameSettingsField.ReprojectionForVolumetrics] = value;
        }
        public bool lightLayers
        {
            get => boolData[(uint)FrameSettingsField.LightLayers];
            set => boolData[(uint)FrameSettingsField.LightLayers] = value;
        }
        public bool depthPrepassWithDeferredRendering
        {
            get => boolData[(uint)FrameSettingsField.DepthPrepassWithDeferredRendering];
            set => boolData[(uint)FrameSettingsField.DepthPrepassWithDeferredRendering] = value;
        }
        public bool transparentPrepass
        {
            get => boolData[(uint)FrameSettingsField.TransparentPrepass];
            set => boolData[(uint)FrameSettingsField.TransparentPrepass] = value;
        }
        public bool motionVectors
        {
            get => boolData[(uint)FrameSettingsField.MotionVectors];
            set => boolData[(uint)FrameSettingsField.MotionVectors] = value;
        }
        public bool objectMotionVectors
        {
            get => boolData[(uint)FrameSettingsField.ObjectMotionVectors];
            set => boolData[(uint)FrameSettingsField.ObjectMotionVectors] = value;
        }
        public bool decals
        {
            get => boolData[(uint)FrameSettingsField.Decals];
            set => boolData[(uint)FrameSettingsField.Decals] = value;
        }
        public bool roughRefraction
        {
            get => boolData[(uint)FrameSettingsField.RoughRefraction];
            set => boolData[(uint)FrameSettingsField.RoughRefraction] = value;
        }
        public bool transparentPostpass
        {
            get => boolData[(uint)FrameSettingsField.TransparentPostpass];
            set => boolData[(uint)FrameSettingsField.TransparentPostpass] = value;
        }
        public bool distortion
        {
            get => boolData[(uint)FrameSettingsField.Distortion];
            set => boolData[(uint)FrameSettingsField.Distortion] = value;
        }
        public bool postprocess
        {
            get => boolData[(uint)FrameSettingsField.Postprocess];
            set => boolData[(uint)FrameSettingsField.Postprocess] = value;
        }
        public bool opaqueObjects
        {
            get => boolData[(uint)FrameSettingsField.OpaqueObjects];
            set => boolData[(uint)FrameSettingsField.OpaqueObjects] = value;
        }
        public bool transparentObjects
        {
            get => boolData[(uint)FrameSettingsField.TransparentObjects];
            set => boolData[(uint)FrameSettingsField.TransparentObjects] = value;
        }
        public bool realtimePlanarReflection
        {
            get => boolData[(uint)FrameSettingsField.RealtimePlanarReflection];
            set => boolData[(uint)FrameSettingsField.RealtimePlanarReflection] = value;
        }
        public bool asyncCompute
        {
            get => boolData[(uint)FrameSettingsField.AsyncCompute];
            set => boolData[(uint)FrameSettingsField.AsyncCompute] = value;
        }
        public bool lightListAsync
        {
            get => boolData[(uint)FrameSettingsField.LightListAsync];
            set => boolData[(uint)FrameSettingsField.LightListAsync] = value;
        }
        public bool ssaoAsync
        {
            get => boolData[(uint)FrameSettingsField.SSAOAsync];
            set => boolData[(uint)FrameSettingsField.SSAOAsync] = value;
        }
        public bool ssrAsync
        {
            get => boolData[(uint)FrameSettingsField.SSRAsync];
            set => boolData[(uint)FrameSettingsField.SSRAsync] = value;
        }
        public bool contactShadowsAsync
        {
            get => boolData[(uint)FrameSettingsField.ContactShadowsAsync];
            set => boolData[(uint)FrameSettingsField.ContactShadowsAsync] = value;
        }
        public bool volumeVoxelizationAsync
        {
            get => boolData[(uint)FrameSettingsField.VolumeVoxelizationsAsync];
            set => boolData[(uint)FrameSettingsField.VolumeVoxelizationsAsync] = value;
        }
        public bool msaa
        {
            get => boolData[(uint)FrameSettingsField.MSAA];
            set => boolData[(uint)FrameSettingsField.MSAA] = value;
        }
        public bool tileAndCluster
        {
            get => boolData[(uint)FrameSettingsField.TileAndCluster];
            set => boolData[(uint)FrameSettingsField.TileAndCluster] = value;
        }
        public bool computeLightEvaluation
        {
            get => boolData[(uint)FrameSettingsField.ComputeLightEvaluation];
            set => boolData[(uint)FrameSettingsField.ComputeLightEvaluation] = value;
        }
        public bool computeLightVariants
        {
            get => boolData[(uint)FrameSettingsField.ComputeLightVariants];
            set => boolData[(uint)FrameSettingsField.ComputeLightVariants] = value;
        }
        public bool computeMaterialVariants
        {
            get => boolData[(uint)FrameSettingsField.ComputeMaterialVariants];
            set => boolData[(uint)FrameSettingsField.ComputeMaterialVariants] = value;
        }
        // Deferred opaque always use FPTL, forward opaque can use FPTL or cluster, transparent always use cluster
        // When MSAA is enabled, we only support cluster (Fptl is too slow with MSAA), and we don't support MSAA for deferred path (mean it is ok to keep fptl)
        public bool fptlForForwardOpaque
        {
            get => boolData[(uint)FrameSettingsField.FptlForForwardOpaque];
            set => boolData[(uint)FrameSettingsField.FptlForForwardOpaque] = value;
        }
        public bool bigTilePrepass
        {
            get => boolData[(uint)FrameSettingsField.BigTilePrepass];
            set => boolData[(uint)FrameSettingsField.BigTilePrepass] = value;
        }
        // Setup by system
        public bool fptl
        {
            get => boolData[(uint)FrameSettingsField.Fptl];
            set => boolData[(uint)FrameSettingsField.Fptl] = value;
        }

        static void Override(ref FrameSettings overridedFrameSettings, FrameSettings overridingFrameSettings, FrameSettingsOverrideMask frameSettingsOverideMask)
        {
            var overrides = frameSettingsOverideMask.mask;
            if (overrides.allFalse)
                return;

            if (overrides.allTrue)
            {
                overridedFrameSettings = overridingFrameSettings;
                return;
            }

            //overrides booleans
            Array values = Enum.GetValues(typeof(FrameSettingsField));
            foreach (FrameSettingsField val in values)
            {
                if (overrides[(uint)val])
                    overridedFrameSettings.boolData[(uint)val] = overridingFrameSettings.boolData[(uint)val];
            }

            //override remaining values here if needed


            //refresh enums for DebugMenu
            //[TODO: save this value in serialized element of DebugMenu once serialization is fixed]
            if (overrides[(uint)FrameSettingsField.ShaderLitMode])
            {
                // the property update the enum index each time. Force to pass by that part of the code when overriding it
                overridedFrameSettings.shaderLitMode = overridedFrameSettings.shaderLitMode;
            }
        }

        // Init a FrameSettings from renderpipeline settings, frame settings and debug settings (if any)
        // This will aggregate the various option
        static void Sanitize(ref FrameSettings sanitazedFrameSettings, Camera camera, RenderPipelineSettings renderPipelineSettings)
        {
            bool reflection = camera.cameraType == CameraType.Reflection;
            bool preview = HDUtils.IsRegularPreviewCamera(camera);
            bool sceneViewFog = CoreUtils.IsSceneViewFogEnabled(camera);

            sanitazedFrameSettings.diffuseGlobalDimmer = 1.0f;

            // When rendering reflection probe we disable specular as it is view dependent
            sanitazedFrameSettings.specularGlobalDimmer = reflection ? 0.0f : 1.0f;

            // We have to fall back to forward-only rendering when scene view is using wireframe rendering mode
            // as rendering everything in wireframe + deferred do not play well together
            if (GL.wireframe || camera.stereoEnabled) //force forward mode for wireframe
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

            sanitazedFrameSettings.boolData[(uint)FrameSettingsField.Shadow] &= !preview;
            sanitazedFrameSettings.boolData[(uint)FrameSettingsField.ShadowMask] &= renderPipelineSettings.supportShadowMask && !preview;
            sanitazedFrameSettings.boolData[(uint)FrameSettingsField.ContactShadow] &= !preview;

            //MSAA only supported in forward
            // TODO: The work will be implemented piecemeal to support all passes
            bool msaa = sanitazedFrameSettings.boolData[(uint)FrameSettingsField.MSAA] &= renderPipelineSettings.supportMSAA && sanitazedFrameSettings.shaderLitMode == LitShaderMode.Forward;

            // VR TODO: The work will be implemented piecemeal to support all passes
            // No recursive reflections
            sanitazedFrameSettings.boolData[(uint)FrameSettingsField.SSR] &= !reflection && renderPipelineSettings.supportSSR && !msaa && !preview && camera.stereoEnabled;
            sanitazedFrameSettings.boolData[(uint)FrameSettingsField.SSAO] &= renderPipelineSettings.supportSSAO && !preview;
            sanitazedFrameSettings.boolData[(uint)FrameSettingsField.SubsurfaceScattering] &= !reflection && renderPipelineSettings.supportSubsurfaceScattering;

            // We must take care of the scene view fog flags in the editor
            bool atmosphericScattering = sanitazedFrameSettings.boolData[(uint)FrameSettingsField.AtmosphericScaterring] &= !sceneViewFog && !preview;

            // Volumetric are disabled if there is no atmospheric scattering
            sanitazedFrameSettings.boolData[(uint)FrameSettingsField.Volumetrics] &= renderPipelineSettings.supportVolumetrics && atmosphericScattering; //&& !preview induced by atmospheric scattering
            sanitazedFrameSettings.boolData[(uint)FrameSettingsField.ReprojectionForVolumetrics] &= !preview;

            sanitazedFrameSettings.boolData[(uint)FrameSettingsField.LightLayers] &= renderPipelineSettings.supportLightLayers && !preview;

            // Planar and real time cubemap doesn't need post process and render in FP16
            bool postprocess = sanitazedFrameSettings.boolData[(uint)FrameSettingsField.Postprocess] &= !reflection && !preview;

            sanitazedFrameSettings.boolData[(uint)FrameSettingsField.TransparentPrepass] &= renderPipelineSettings.supportTransparentDepthPrepass && !preview;

            // VR TODO: The work will be implemented piecemeal to support all passes
            // VR TODO: check why '=' and not '&=' and if we can merge these lines
            bool motionVector;
            if (camera.stereoEnabled)
                motionVector = sanitazedFrameSettings.boolData[(uint)FrameSettingsField.MotionVectors] = postprocess && !msaa && !preview;
            else
                motionVector = sanitazedFrameSettings.boolData[(uint)FrameSettingsField.MotionVectors] &= !reflection && renderPipelineSettings.supportMotionVectors && !preview;

            // Object motion vector are disabled if motion vector are disabled
            sanitazedFrameSettings.boolData[(uint)FrameSettingsField.ObjectMotionVectors] &= motionVector && !preview;
            sanitazedFrameSettings.boolData[(uint)FrameSettingsField.Decals] &= renderPipelineSettings.supportDecals && !preview;
            sanitazedFrameSettings.boolData[(uint)FrameSettingsField.TransparentPostpass] &= renderPipelineSettings.supportTransparentDepthPostpass && !preview;
            sanitazedFrameSettings.boolData[(uint)FrameSettingsField.Distortion] &= !reflection && renderPipelineSettings.supportDistortion && !msaa && !preview;

            bool async = sanitazedFrameSettings.boolData[(uint)FrameSettingsField.AsyncCompute] &= SystemInfo.supportsAsyncCompute;
            sanitazedFrameSettings.boolData[(uint)FrameSettingsField.LightListAsync] &= async;
            sanitazedFrameSettings.boolData[(uint)FrameSettingsField.SSRAsync] &= async;
            sanitazedFrameSettings.boolData[(uint)FrameSettingsField.SSAOAsync] &= async;
            sanitazedFrameSettings.boolData[(uint)FrameSettingsField.ContactShadowsAsync] &= async;
            sanitazedFrameSettings.boolData[(uint)FrameSettingsField.VolumeVoxelizationsAsync] &= async;

            // Deferred opaque are always using Fptl. Forward opaque can use Fptl or Cluster, transparent use cluster.
            // When MSAA is enabled we disable Fptl as it become expensive compare to cluster
            // In HD, MSAA is only supported for forward only rendering, no MSAA in deferred mode (for code complexity reasons)
            // Disable FPTL for stereo for now
            bool fptlForwardOpaque = sanitazedFrameSettings.boolData[(uint)FrameSettingsField.FptlForForwardOpaque] &= !msaa && !XRGraphics.enabled;

            // If Deferred, enable Fptl. If we are forward renderer only and not using Fptl for forward opaque, disable Fptl
            sanitazedFrameSettings.boolData[(uint)FrameSettingsField.Fptl] &= sanitazedFrameSettings.shaderLitMode == LitShaderMode.Deferred || fptlForwardOpaque;
        }
        
        public static void AggregateFrameSettings(ref FrameSettings aggregatedFrameSettings, Camera camera, HDAdditionalCameraData additionalData, HDRenderPipelineAsset hdrpAsset)
        {
            aggregatedFrameSettings = hdrpAsset.GetFrameSettings();
            if (additionalData && additionalData.customRenderingSettings)
                Override(ref aggregatedFrameSettings, additionalData.renderingPathCustomFrameSettings, additionalData.renderingPathCustomOverrideFrameSettings);
            Sanitize(ref aggregatedFrameSettings, camera, hdrpAsset.GetRenderPipelineSettings());
        }

        //For debugging only
        public static void AggregateFrameSettingsDebug(ref FrameSettings aggregatedFrameSettings, Camera camera, HDAdditionalCameraData additionalData, HDRenderPipelineAsset hdrpAsset,
            ref FrameSettings hdrpAssetFrameSettings, ref FrameSettings afterCustomOverride, ref FrameSettings afterSanitazation, ref FrameSettings afterDebugOverride)
        {
            hdrpAssetFrameSettings = aggregatedFrameSettings = hdrpAsset.GetFrameSettings();
            if (additionalData && additionalData.customRenderingSettings)
                Override(ref aggregatedFrameSettings, additionalData.renderingPathCustomFrameSettings, additionalData.renderingPathCustomOverrideFrameSettings);
            afterCustomOverride = aggregatedFrameSettings;
            Sanitize(ref aggregatedFrameSettings, camera, hdrpAsset.GetRenderPipelineSettings());
            afterSanitazation = aggregatedFrameSettings;
            //if (debug)
            //    FrameSettings.Override(ref aggregatedFrameSettings, debug.renderingPathCustomFrameSettings, debug.renderingPathCustomOverrideFrameSettings);
            afterDebugOverride = aggregatedFrameSettings;
        }

        void RefreshDebugMenu()
        {
            // actually, we need to sync up changes done in the debug menu too
            // TODO DebugMenu : store this value in serialization of of debug menu once its fixed
            switch (shaderLitMode)
            {
                case LitShaderMode.Forward:
                    m_LitShaderModeEnumIndex = 0;
                    break;
                case LitShaderMode.Deferred:
                    m_LitShaderModeEnumIndex = 1;
                    break;
                default:
                    throw new ArgumentException("Unknown LitShaderMode");
            }
        }

        public bool BuildLightListRunsAsync()
        {
            return SystemInfo.supportsAsyncCompute && boolData[(uint)FrameSettingsField.AsyncCompute] && boolData[(uint)FrameSettingsField.LightListAsync];
        }

        public bool SSRRunsAsync()
        {
            return SystemInfo.supportsAsyncCompute && boolData[(uint)FrameSettingsField.AsyncCompute] && boolData[(uint)FrameSettingsField.SSRAsync];
        }

        public bool SSAORunsAsync()
        {
            return SystemInfo.supportsAsyncCompute && boolData[(uint)FrameSettingsField.AsyncCompute] && boolData[(uint)FrameSettingsField.SSAOAsync];
        }

        public bool ContactShadowsRunAsync()
        {
            return SystemInfo.supportsAsyncCompute && boolData[(uint)FrameSettingsField.AsyncCompute] && boolData[(uint)FrameSettingsField.ContactShadowsAsync];
        }

        public bool VolumeVoxelizationRunsAsync()
        {
            return SystemInfo.supportsAsyncCompute && boolData[(uint)FrameSettingsField.AsyncCompute] && boolData[(uint)FrameSettingsField.VolumeVoxelizationsAsync];
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
