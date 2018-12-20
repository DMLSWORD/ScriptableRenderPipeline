using System;
using System.Collections.Generic;
using UnityEngine.Rendering;
using UnityEngine.Serialization;

namespace UnityEngine.Experimental.Rendering.HDPipeline
{
    [Obsolete("For data migration")]
    public enum ObsoleteLitShaderMode
    {
        Forward,
        Deferred
    }

    [Flags, Obsolete("For data migration")]
    enum ObsoleteLightLoopSettingsOverrides
    {
        FptlForForwardOpaque = 1 << 0,
        BigTilePrepass = 1 << 1,
        ComputeLightEvaluation = 1 << 2,
        ComputeLightVariants = 1 << 3,
        ComputeMaterialVariants = 1 << 4,
        TileAndCluster = 1 << 5,
        //Fptl = 1 << 6, //isFptlEnabled set up by system
    }

    [Flags, Obsolete("For data migration")]
    enum ObsoleteFrameSettingsOverrides
    {
        //lighting settings
        Shadow = 1 << 0,
        ContactShadow = 1 << 1,
        ShadowMask = 1 << 2,
        SSR = 1 << 3,
        SSAO = 1 << 4,
        SubsurfaceScattering = 1 << 5,
        Transmission = 1 << 6,
        AtmosphericScaterring = 1 << 7,
        Volumetrics = 1 << 8,
        ReprojectionForVolumetrics = 1 << 9,
        LightLayers = 1 << 10,
        MSAA = 1 << 11,

        //rendering pass
        TransparentPrepass = 1 << 13,
        TransparentPostpass = 1 << 14,
        MotionVectors = 1 << 15,
        ObjectMotionVectors = 1 << 16,
        Decals = 1 << 17,
        RoughRefraction = 1 << 18,
        Distortion = 1 << 19,
        Postprocess = 1 << 20,

        //rendering settings
        ShaderLitMode = 1 << 21,
        DepthPrepassWithDeferredRendering = 1 << 22,
        OpaqueObjects = 1 << 24,
        TransparentObjects = 1 << 25,
        RealtimePlanarReflection = 1 << 26,

        // Async settings
        AsyncCompute = 1 << 23,
        LightListAsync = 1 << 27,
        SSRAsync = 1 << 28,
        SSAOAsync = 1 << 29,
        ContactShadowsAsync = 1 << 30,
        VolumeVoxelizationsAsync = 1 << 31,
    }

    [Serializable, Obsolete("For data migration")]
    class ObsoleteLightLoopSettings
    {
        public ObsoleteLightLoopSettingsOverrides overrides;
        public bool enableTileAndCluster;
        public bool enableComputeLightEvaluation;
        public bool enableComputeLightVariants;
        public bool enableComputeMaterialVariants;
        public bool enableFptlForForwardOpaque;
        public bool enableBigTilePrepass;
        public bool isFptlEnabled;
    }

    // The settings here are per frame settings.
    // Each camera must have its own per frame settings
    [Serializable]
    [System.Diagnostics.DebuggerDisplay("FrameSettings overriding {overrides.ToString(\"X\")}")]
    [Obsolete("For data migration")]
    class ObsoleteFrameSettings
    {
        public ObsoleteFrameSettingsOverrides overrides;

        public bool enableShadow;
        public bool enableContactShadows;
        public bool enableShadowMask;
        public bool enableSSR;
        public bool enableSSAO;
        public bool enableSubsurfaceScattering;
        public bool enableTransmission;  
        public bool enableAtmosphericScattering;
        public bool enableVolumetrics;
        public bool enableReprojectionForVolumetrics;
        public bool enableLightLayers;
        
        public float diffuseGlobalDimmer;
        public float specularGlobalDimmer;
        
        public ObsoleteLitShaderMode shaderLitMode;
        public bool enableDepthPrepassWithDeferredRendering;

        public bool enableTransparentPrepass;
        public bool enableMotionVectors; // Enable/disable whole motion vectors pass (Camera + Object).
        public bool enableObjectMotionVectors;
        [FormerlySerializedAs("enableDBuffer")]
        public bool enableDecals;
        public bool enableRoughRefraction; // Depends on DepthPyramid - If not enable, just do a copy of the scene color (?) - how to disable rough refraction ?
        public bool enableTransparentPostpass;
        public bool enableDistortion;
        public bool enablePostprocess;

        public bool enableOpaqueObjects;
        public bool enableTransparentObjects;
        public bool enableRealtimePlanarReflection;

        public bool enableMSAA;
        
        public bool enableAsyncCompute;
        public bool runLightListAsync;
        public bool runSSRAsync;
        public bool runSSAOAsync;
        public bool runContactShadowsAsync;
        public bool runVolumeVoxelizationAsync;
        
        public ObsoleteLightLoopSettings lightLoopSettings;

        int m_LitShaderModeEnumIndex; 
    }

    public partial struct FrameSettings
    {
#pragma warning disable 618 // Type or member is obsolete
        internal static void MigrateFromClassVersion(ref ObsoleteFrameSettings oldFrameSettingsFormat, ref FrameSettings newFrameSettingsFormat, ref FrameSettingsOverrideMask newFrameSettingsOverrideMask)
        {
            if (oldFrameSettingsFormat == null)
                return;

            // no need to migrate those computed at frame value
            //newFrameSettingsFormat.diffuseGlobalDimmer = oldFrameSettingsFormat.diffuseGlobalDimmer;
            //newFrameSettingsFormat.specularGlobalDimmer = oldFrameSettingsFormat.specularGlobalDimmer;

            // Data
            switch (oldFrameSettingsFormat.shaderLitMode)
            {
                case ObsoleteLitShaderMode.Forward:
                    newFrameSettingsFormat.shaderLitMode = LitShaderMode.Forward;
                    break;
                case ObsoleteLitShaderMode.Deferred:
                    newFrameSettingsFormat.shaderLitMode = LitShaderMode.Deferred;
                    break;
                default:
                    throw new ArgumentException("Unknown ObsoleteLitShaderMode");
            }

            newFrameSettingsFormat.shadow = oldFrameSettingsFormat.enableShadow;
            newFrameSettingsFormat.contactShadows = oldFrameSettingsFormat.enableContactShadows;
            newFrameSettingsFormat.shadowMask = oldFrameSettingsFormat.enableShadowMask;
            newFrameSettingsFormat.ssr = oldFrameSettingsFormat.enableSSR;
            newFrameSettingsFormat.ssao = oldFrameSettingsFormat.enableSSAO;
            newFrameSettingsFormat.subsurfaceScattering = oldFrameSettingsFormat.enableSubsurfaceScattering;
            newFrameSettingsFormat.transmission = oldFrameSettingsFormat.enableTransmission;
            newFrameSettingsFormat.atmosphericScattering = oldFrameSettingsFormat.enableAtmosphericScattering;
            newFrameSettingsFormat.volumetrics = oldFrameSettingsFormat.enableVolumetrics;
            newFrameSettingsFormat.reprojectionForVolumetrics = oldFrameSettingsFormat.enableReprojectionForVolumetrics;
            newFrameSettingsFormat.lightLayers = oldFrameSettingsFormat.enableLightLayers;
            newFrameSettingsFormat.depthPrepassWithDeferredRendering = oldFrameSettingsFormat.enableDepthPrepassWithDeferredRendering;
            newFrameSettingsFormat.transparentPrepass = oldFrameSettingsFormat.enableTransparentPrepass;
            newFrameSettingsFormat.motionVectors = oldFrameSettingsFormat.enableMotionVectors;
            newFrameSettingsFormat.objectMotionVectors = oldFrameSettingsFormat.enableObjectMotionVectors;
            newFrameSettingsFormat.decals = oldFrameSettingsFormat.enableDecals;
            newFrameSettingsFormat.roughRefraction = oldFrameSettingsFormat.enableRoughRefraction;
            newFrameSettingsFormat.transparentPostpass = oldFrameSettingsFormat.enableTransparentPostpass;
            newFrameSettingsFormat.distortion = oldFrameSettingsFormat.enableDistortion;
            newFrameSettingsFormat.postprocess = oldFrameSettingsFormat.enablePostprocess;
            newFrameSettingsFormat.opaqueObjects = oldFrameSettingsFormat.enableOpaqueObjects;
            newFrameSettingsFormat.transparentObjects = oldFrameSettingsFormat.enableTransparentObjects;
            newFrameSettingsFormat.realtimePlanarReflection = oldFrameSettingsFormat.enableRealtimePlanarReflection;
            newFrameSettingsFormat.msaa = oldFrameSettingsFormat.enableMSAA;
            newFrameSettingsFormat.asyncCompute = oldFrameSettingsFormat.enableAsyncCompute;
            newFrameSettingsFormat.lightListAsync = oldFrameSettingsFormat.runLightListAsync;
            newFrameSettingsFormat.ssrAsync = oldFrameSettingsFormat.runSSRAsync;
            newFrameSettingsFormat.ssaoAsync = oldFrameSettingsFormat.runSSAOAsync;
            newFrameSettingsFormat.contactShadowsAsync = oldFrameSettingsFormat.runContactShadowsAsync;
            newFrameSettingsFormat.volumeVoxelizationAsync = oldFrameSettingsFormat.runVolumeVoxelizationAsync;
            newFrameSettingsFormat.tileAndCluster = oldFrameSettingsFormat.lightLoopSettings.enableTileAndCluster;
            newFrameSettingsFormat.computeLightEvaluation = oldFrameSettingsFormat.lightLoopSettings.enableComputeLightEvaluation;
            newFrameSettingsFormat.computeLightVariants = oldFrameSettingsFormat.lightLoopSettings.enableComputeLightVariants;
            newFrameSettingsFormat.computeMaterialVariants = oldFrameSettingsFormat.lightLoopSettings.enableComputeMaterialVariants;
            newFrameSettingsFormat.fptlForForwardOpaque = oldFrameSettingsFormat.lightLoopSettings.enableFptlForForwardOpaque;
            newFrameSettingsFormat.bigTilePrepass = oldFrameSettingsFormat.lightLoopSettings.enableBigTilePrepass;

            // OverrideMask
            newFrameSettingsOverrideMask.mask = new CheapBitArray128();
            Array values = Enum.GetValues(typeof(ObsoleteFrameSettingsOverrides));
            foreach (ObsoleteFrameSettingsOverrides val in values)
            {
                if ((val & oldFrameSettingsFormat.overrides) > 0)
                {
                    switch(val)
                    {
                        case ObsoleteFrameSettingsOverrides.Shadow:
                            newFrameSettingsOverrideMask.mask[(int)FrameSettingsField.Shadow] = true;
                            break;
                        case ObsoleteFrameSettingsOverrides.ContactShadow:
                            newFrameSettingsOverrideMask.mask[(int)FrameSettingsField.ContactShadow] = true;
                            break;
                        case ObsoleteFrameSettingsOverrides.ShadowMask:
                            newFrameSettingsOverrideMask.mask[(int)FrameSettingsField.ShadowMask] = true;
                            break;
                        case ObsoleteFrameSettingsOverrides.SSR:
                            newFrameSettingsOverrideMask.mask[(int)FrameSettingsField.SSR] = true;
                            break;
                        case ObsoleteFrameSettingsOverrides.SSAO:
                            newFrameSettingsOverrideMask.mask[(int)FrameSettingsField.SSAO] = true;
                            break;
                        case ObsoleteFrameSettingsOverrides.SubsurfaceScattering:
                            newFrameSettingsOverrideMask.mask[(int)FrameSettingsField.SubsurfaceScattering] = true;
                            break;
                        case ObsoleteFrameSettingsOverrides.Transmission:
                            newFrameSettingsOverrideMask.mask[(int)FrameSettingsField.Transmission] = true;
                            break;
                        case ObsoleteFrameSettingsOverrides.AtmosphericScaterring:
                            newFrameSettingsOverrideMask.mask[(int)FrameSettingsField.AtmosphericScaterring] = true;
                            break;
                        case ObsoleteFrameSettingsOverrides.Volumetrics:
                            newFrameSettingsOverrideMask.mask[(int)FrameSettingsField.Volumetrics] = true;
                            break;
                        case ObsoleteFrameSettingsOverrides.ReprojectionForVolumetrics:
                            newFrameSettingsOverrideMask.mask[(int)FrameSettingsField.ReprojectionForVolumetrics] = true;
                            break;
                        case ObsoleteFrameSettingsOverrides.LightLayers:
                            newFrameSettingsOverrideMask.mask[(int)FrameSettingsField.LightLayers] = true;
                            break;
                        case ObsoleteFrameSettingsOverrides.DepthPrepassWithDeferredRendering:
                            newFrameSettingsOverrideMask.mask[(int)FrameSettingsField.DepthPrepassWithDeferredRendering] = true;
                            break;
                        case ObsoleteFrameSettingsOverrides.TransparentPrepass:
                            newFrameSettingsOverrideMask.mask[(int)FrameSettingsField.TransparentPrepass] = true;
                            break;
                        case ObsoleteFrameSettingsOverrides.MotionVectors:
                            newFrameSettingsOverrideMask.mask[(int)FrameSettingsField.MotionVectors] = true;
                            break;
                        case ObsoleteFrameSettingsOverrides.ObjectMotionVectors:
                            newFrameSettingsOverrideMask.mask[(int)FrameSettingsField.ObjectMotionVectors] = true;
                            break;
                        case ObsoleteFrameSettingsOverrides.Decals:
                            newFrameSettingsOverrideMask.mask[(int)FrameSettingsField.Decals] = true;
                            break;
                        case ObsoleteFrameSettingsOverrides.RoughRefraction:
                            newFrameSettingsOverrideMask.mask[(int)FrameSettingsField.RoughRefraction] = true;
                            break;
                        case ObsoleteFrameSettingsOverrides.TransparentPostpass:
                            newFrameSettingsOverrideMask.mask[(int)FrameSettingsField.TransparentPostpass] = true;
                            break;
                        case ObsoleteFrameSettingsOverrides.Distortion:
                            newFrameSettingsOverrideMask.mask[(int)FrameSettingsField.Distortion] = true;
                            break;
                        case ObsoleteFrameSettingsOverrides.Postprocess:
                            newFrameSettingsOverrideMask.mask[(int)FrameSettingsField.Postprocess] = true;
                            break;
                        case ObsoleteFrameSettingsOverrides.OpaqueObjects:
                            newFrameSettingsOverrideMask.mask[(int)FrameSettingsField.OpaqueObjects] = true;
                            break;
                        case ObsoleteFrameSettingsOverrides.TransparentObjects:
                            newFrameSettingsOverrideMask.mask[(int)FrameSettingsField.TransparentObjects] = true;
                            break;
                        case ObsoleteFrameSettingsOverrides.RealtimePlanarReflection:
                            newFrameSettingsOverrideMask.mask[(int)FrameSettingsField.RealtimePlanarReflection] = true;
                            break;
                        case ObsoleteFrameSettingsOverrides.MSAA:
                            newFrameSettingsOverrideMask.mask[(int)FrameSettingsField.MSAA] = true;
                            break;
                        case ObsoleteFrameSettingsOverrides.AsyncCompute:
                            newFrameSettingsOverrideMask.mask[(int)FrameSettingsField.AsyncCompute] = true;
                            break;
                        case ObsoleteFrameSettingsOverrides.LightListAsync:
                            newFrameSettingsOverrideMask.mask[(int)FrameSettingsField.LightListAsync] = true;
                            break;
                        case ObsoleteFrameSettingsOverrides.SSRAsync:
                            newFrameSettingsOverrideMask.mask[(int)FrameSettingsField.SSRAsync] = true;
                            break;
                        case ObsoleteFrameSettingsOverrides.SSAOAsync:
                            newFrameSettingsOverrideMask.mask[(int)FrameSettingsField.SSAOAsync] = true;
                            break;
                        case ObsoleteFrameSettingsOverrides.ContactShadowsAsync:
                            newFrameSettingsOverrideMask.mask[(int)FrameSettingsField.ContactShadowsAsync] = true;
                            break;
                        case ObsoleteFrameSettingsOverrides.VolumeVoxelizationsAsync:
                            newFrameSettingsOverrideMask.mask[(int)FrameSettingsField.VolumeVoxelizationsAsync] = true;
                            break;
                        default:
                            throw new ArgumentException("Unknown ObsoleteFrameSettingsOverride");
                    }
                }
            }
            values = Enum.GetValues(typeof(ObsoleteLightLoopSettingsOverrides));
            foreach (ObsoleteLightLoopSettingsOverrides val in values)
            {
                if ((val & oldFrameSettingsFormat.lightLoopSettings.overrides) > 0)
                {
                    switch (val)
                    {
                        case ObsoleteLightLoopSettingsOverrides.TileAndCluster:
                            newFrameSettingsOverrideMask.mask[(int)FrameSettingsField.TileAndCluster] = true;
                            break;
                        case ObsoleteLightLoopSettingsOverrides.BigTilePrepass:
                            newFrameSettingsOverrideMask.mask[(int)FrameSettingsField.BigTilePrepass] = true;
                            break;
                        case ObsoleteLightLoopSettingsOverrides.ComputeLightEvaluation:
                            newFrameSettingsOverrideMask.mask[(int)FrameSettingsField.ComputeLightEvaluation] = true;
                            break;
                        case ObsoleteLightLoopSettingsOverrides.ComputeLightVariants:
                            newFrameSettingsOverrideMask.mask[(int)FrameSettingsField.ComputeLightVariants] = true;
                            break;
                        case ObsoleteLightLoopSettingsOverrides.ComputeMaterialVariants:
                            newFrameSettingsOverrideMask.mask[(int)FrameSettingsField.ComputeMaterialVariants] = true;
                            break;
                        case ObsoleteLightLoopSettingsOverrides.FptlForForwardOpaque:
                            newFrameSettingsOverrideMask.mask[(int)FrameSettingsField.FptlForForwardOpaque] = true;
                            break;
                        default:
                            throw new ArgumentException("Unknown ObsoleteLightLoopSettingsOverrides");
                    }
                }
            }

            //free space:
            oldFrameSettingsFormat = null;
        }
#pragma warning restore 618 // Type or member is obsolete
    }
}
