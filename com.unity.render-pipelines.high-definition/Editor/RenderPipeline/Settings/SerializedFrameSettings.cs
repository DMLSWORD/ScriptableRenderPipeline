using UnityEditor.Rendering;
using UnityEngine.Experimental.Rendering.HDPipeline;

namespace UnityEditor.Experimental.Rendering.HDPipeline
{
    class SerializedFrameSettings
    {
        /////////////////////////////
        /// SERIALIZED PROPERTIES ///
        /////////////////////////////
        public SerializedProperty rootData;
        public SerializedProperty rootOveride;

        // Rendering Pass
        public SerializedProperty transparentPrepass;
        public SerializedProperty transparentPostpass;
        public SerializedProperty motionVectors;
        public SerializedProperty objectMotionVectors;
        public SerializedProperty decals;
        public SerializedProperty roughRefraction;
        public SerializedProperty distortion;
        public SerializedProperty postprocess;

        // Rendering Settings
        public SerializedProperty litShaderMode;
        public SerializedProperty depthPrepassWithDeferredRendering;
        public SerializedProperty opaqueObjects;
        public SerializedProperty transparentObjects;
        public SerializedProperty realtimePlanarReflection;
        public SerializedProperty msaa;

        // Lighting Settings
        public SerializedProperty ssr;
        public SerializedProperty ssao;
        public SerializedProperty subsurfaceScattering;
        public SerializedProperty transmission;
        public SerializedProperty shadow;
        public SerializedProperty contactShadow;
        public SerializedProperty shadowMask;
        public SerializedProperty atmosphericScattering;
        public SerializedProperty volumetrics;
        public SerializedProperty reprojectionForVolumetrics;
        public SerializedProperty lightLayers;

        // Async Compute Settings
        public SerializedProperty asyncCompute;
        public SerializedProperty buildLightListAsync;
        public SerializedProperty ssrAsync;
        public SerializedProperty ssaoAsync;
        public SerializedProperty contactShadowsAsync;
        public SerializedProperty volumeVoxelizationAsync;

        // Light Loop Settings
        public SerializedProperty tileAndCluster;
        public SerializedProperty fptlForForwardOpaque;
        public SerializedProperty bigTilePrepass;
        public SerializedProperty computeLightEvaluation;
        public SerializedProperty computeLightVariants;
        public SerializedProperty computeMaterialVariants;


        /////////////////////////////
        ///  OVERRIDES ACCESSOR   ///
        /////////////////////////////
        // Rendering Pass
        public bool overridesTransparentPrepass
        {
            get => rootOveride.GetArrayElementAtIndex((int)FrameSettingsField.TransparentPrepass).boolValue;
            set => rootOveride.GetArrayElementAtIndex((int)FrameSettingsField.TransparentPrepass).boolValue = value;
        }
        public bool overridesTransparentPostpass
        {
            get => rootOveride.GetArrayElementAtIndex((int)FrameSettingsField.TransparentPostpass).boolValue;
            set => rootOveride.GetArrayElementAtIndex((int)FrameSettingsField.TransparentPostpass).boolValue = value;
        }
        public bool overridesMotionVectors
        {
            get => rootOveride.GetArrayElementAtIndex((int)FrameSettingsField.MotionVectors).boolValue;
            set => rootOveride.GetArrayElementAtIndex((int)FrameSettingsField.MotionVectors).boolValue = value;
        }
        public bool overridesObjectMotionVectors
        {
            get => rootOveride.GetArrayElementAtIndex((int)FrameSettingsField.ObjectMotionVectors).boolValue;
            set => rootOveride.GetArrayElementAtIndex((int)FrameSettingsField.ObjectMotionVectors).boolValue = value;
        }
        public bool overridesDecals
        {
            get => rootOveride.GetArrayElementAtIndex((int)FrameSettingsField.Decals).boolValue;
            set => rootOveride.GetArrayElementAtIndex((int)FrameSettingsField.Decals).boolValue = value;
        }
        public bool overridesRoughRefraction
        {
            get => rootOveride.GetArrayElementAtIndex((int)FrameSettingsField.RoughRefraction).boolValue;
            set => rootOveride.GetArrayElementAtIndex((int)FrameSettingsField.RoughRefraction).boolValue = value;
        }
        public bool overridesDistortion
        {
            get => rootOveride.GetArrayElementAtIndex((int)FrameSettingsField.Distortion).boolValue;
            set => rootOveride.GetArrayElementAtIndex((int)FrameSettingsField.Distortion).boolValue = value;
        }
        public bool overridesPostprocess
        {
            get => rootOveride.GetArrayElementAtIndex((int)FrameSettingsField.Postprocess).boolValue;
            set => rootOveride.GetArrayElementAtIndex((int)FrameSettingsField.Postprocess).boolValue = value;
        }

        // Rendering Settings
        public bool overridesShaderLitMode
        {
            get => rootOveride.GetArrayElementAtIndex((int)FrameSettingsField.ShaderLitMode).boolValue;
            set => rootOveride.GetArrayElementAtIndex((int)FrameSettingsField.ShaderLitMode).boolValue = value;
        }
        public bool overridesDepthPrepassWithDeferredRendering
        {
            get => rootOveride.GetArrayElementAtIndex((int)FrameSettingsField.DepthPrepassWithDeferredRendering).boolValue;
            set => rootOveride.GetArrayElementAtIndex((int)FrameSettingsField.DepthPrepassWithDeferredRendering).boolValue = value;
        }
        public bool overridesOpaqueObjects
        {
            get => rootOveride.GetArrayElementAtIndex((int)FrameSettingsField.OpaqueObjects).boolValue;
            set => rootOveride.GetArrayElementAtIndex((int)FrameSettingsField.OpaqueObjects).boolValue = value;
        }
        public bool overridesTransparentObjects
        {
            get => rootOveride.GetArrayElementAtIndex((int)FrameSettingsField.TransparentObjects).boolValue;
            set => rootOveride.GetArrayElementAtIndex((int)FrameSettingsField.TransparentObjects).boolValue = value;
        }
        public bool overridesRealtimePlanarReflection
        {
            get => rootOveride.GetArrayElementAtIndex((int)FrameSettingsField.RealtimePlanarReflection).boolValue;
            set => rootOveride.GetArrayElementAtIndex((int)FrameSettingsField.RealtimePlanarReflection).boolValue = value;
        }
        public bool overridesMSAA
        {
            get => rootOveride.GetArrayElementAtIndex((int)FrameSettingsField.MSAA).boolValue;
            set => rootOveride.GetArrayElementAtIndex((int)FrameSettingsField.MSAA).boolValue = value;
        }

        // Lighting Settings
        public bool overridesSSR
        {
            get => rootOveride.GetArrayElementAtIndex((int)FrameSettingsField.SSR).boolValue;
            set => rootOveride.GetArrayElementAtIndex((int)FrameSettingsField.SSR).boolValue = value;
        }
        public bool overridesSSAO
        {
            get => rootOveride.GetArrayElementAtIndex((int)FrameSettingsField.SSAO).boolValue;
            set => rootOveride.GetArrayElementAtIndex((int)FrameSettingsField.SSAO).boolValue = value;
        }
        public bool overridesSubsurfaceScattering
        {
            get => rootOveride.GetArrayElementAtIndex((int)FrameSettingsField.SubsurfaceScattering).boolValue;
            set => rootOveride.GetArrayElementAtIndex((int)FrameSettingsField.SubsurfaceScattering).boolValue = value;
        }
        public bool overridesTransmission
        {
            get => rootOveride.GetArrayElementAtIndex((int)FrameSettingsField.Transmission).boolValue;
            set => rootOveride.GetArrayElementAtIndex((int)FrameSettingsField.Transmission).boolValue = value;
        }
        public bool overridesShadow
        {
            get => rootOveride.GetArrayElementAtIndex((int)FrameSettingsField.Shadow).boolValue;
            set => rootOveride.GetArrayElementAtIndex((int)FrameSettingsField.Shadow).boolValue = value;
        }
        public bool overridesContactShadow
        {
            get => rootOveride.GetArrayElementAtIndex((int)FrameSettingsField.ContactShadow).boolValue;
            set => rootOveride.GetArrayElementAtIndex((int)FrameSettingsField.ContactShadow).boolValue = value;
        }
        public bool overridesShadowMask
        {
            get => rootOveride.GetArrayElementAtIndex((int)FrameSettingsField.ShadowMask).boolValue;
            set => rootOveride.GetArrayElementAtIndex((int)FrameSettingsField.ShadowMask).boolValue = value;
        }
        public bool overridesAtmosphericScaterring
        {
            get => rootOveride.GetArrayElementAtIndex((int)FrameSettingsField.AtmosphericScaterring).boolValue;
            set => rootOveride.GetArrayElementAtIndex((int)FrameSettingsField.AtmosphericScaterring).boolValue = value;
        }
        public bool overridesVolumetrics
        {
            get => rootOveride.GetArrayElementAtIndex((int)FrameSettingsField.Volumetrics).boolValue;
            set => rootOveride.GetArrayElementAtIndex((int)FrameSettingsField.Volumetrics).boolValue = value;
        }
        public bool overridesProjectionForVolumetrics
        {
            get => rootOveride.GetArrayElementAtIndex((int)FrameSettingsField.ReprojectionForVolumetrics).boolValue;
            set => rootOveride.GetArrayElementAtIndex((int)FrameSettingsField.ReprojectionForVolumetrics).boolValue = value;
        }
        public bool overridesLightLayers
        {
            get => rootOveride.GetArrayElementAtIndex((int)FrameSettingsField.LightLayers).boolValue;
            set => rootOveride.GetArrayElementAtIndex((int)FrameSettingsField.LightLayers).boolValue = value;
        }
        
        // Async Compute Settings
        public bool overridesAsyncCompute
        {
            get => rootOveride.GetArrayElementAtIndex((int)FrameSettingsField.AsyncCompute).boolValue;
            set => rootOveride.GetArrayElementAtIndex((int)FrameSettingsField.AsyncCompute).boolValue = value;
        }
        public bool overridesLightListAsync
        {
            get => rootOveride.GetArrayElementAtIndex((int)FrameSettingsField.LightListAsync).boolValue;
            set => rootOveride.GetArrayElementAtIndex((int)FrameSettingsField.LightListAsync).boolValue = value;
        }
        public bool overridesSSRAsync
        {
            get => rootOveride.GetArrayElementAtIndex((int)FrameSettingsField.SSRAsync).boolValue;
            set => rootOveride.GetArrayElementAtIndex((int)FrameSettingsField.SSRAsync).boolValue = value;
        }
        public bool overridesSSAOAsync
        {
            get => rootOveride.GetArrayElementAtIndex((int)FrameSettingsField.SSAOAsync).boolValue;
            set => rootOveride.GetArrayElementAtIndex((int)FrameSettingsField.SSAOAsync).boolValue = value;
        }
        public bool overridesContactShadowsAsync
        {
            get => rootOveride.GetArrayElementAtIndex((int)FrameSettingsField.ContactShadowsAsync).boolValue;
            set => rootOveride.GetArrayElementAtIndex((int)FrameSettingsField.ContactShadowsAsync).boolValue = value;
        }
        public bool overridesVolumeVoxelizationAsync
        {
            get => rootOveride.GetArrayElementAtIndex((int)FrameSettingsField.VolumeVoxelizationsAsync).boolValue;
            set => rootOveride.GetArrayElementAtIndex((int)FrameSettingsField.VolumeVoxelizationsAsync).boolValue = value;
        }

        // Light Loop Settings
        public bool overridesTileAndCluster
        {
            get => rootOveride.GetArrayElementAtIndex((int)FrameSettingsField.TileAndCluster).boolValue;
            set => rootOveride.GetArrayElementAtIndex((int)FrameSettingsField.TileAndCluster).boolValue = value;
        }
        public bool overridesFptlForForwardOpaque
        {
            get => rootOveride.GetArrayElementAtIndex((int)FrameSettingsField.FptlForForwardOpaque).boolValue;
            set => rootOveride.GetArrayElementAtIndex((int)FrameSettingsField.FptlForForwardOpaque).boolValue = value;
        }
        public bool overridesBigTilePrepass
        {
            get => rootOveride.GetArrayElementAtIndex((int)FrameSettingsField.BigTilePrepass).boolValue;
            set => rootOveride.GetArrayElementAtIndex((int)FrameSettingsField.BigTilePrepass).boolValue = value;
        }
        public bool overridesComputeLightEvaluation
        {
            get => rootOveride.GetArrayElementAtIndex((int)FrameSettingsField.ComputeLightEvaluation).boolValue;
            set => rootOveride.GetArrayElementAtIndex((int)FrameSettingsField.ComputeLightEvaluation).boolValue = value;
        }
        public bool overridesComputeLightVariants
        {
            get => rootOveride.GetArrayElementAtIndex((int)FrameSettingsField.ComputeLightVariants).boolValue;
            set => rootOveride.GetArrayElementAtIndex((int)FrameSettingsField.ComputeLightVariants).boolValue = value;
        }
        public bool overridesComputeMaterialVariants
        {
            get => rootOveride.GetArrayElementAtIndex((int)FrameSettingsField.ComputeMaterialVariants).boolValue;
            set => rootOveride.GetArrayElementAtIndex((int)FrameSettingsField.ComputeMaterialVariants).boolValue = value;
        }

        public SerializedFrameSettings(SerializedProperty rootData, SerializedProperty rootOverride)
        {
            this.rootData = rootData;
            this.rootOveride = rootOverride;

            // Rendering Pass
            transparentPrepass = rootData.Find((FrameSettings d) => d.transparentPrepass);
            transparentPostpass = rootData.Find((FrameSettings d) => d.transparentPostpass);
            motionVectors = rootData.Find((FrameSettings d) => d.motionVectors);
            objectMotionVectors = rootData.Find((FrameSettings d) => d.objectMotionVectors);
            decals = rootData.Find((FrameSettings d) => d.decals);
            roughRefraction = rootData.Find((FrameSettings d) => d.roughRefraction);
            distortion = rootData.Find((FrameSettings d) => d.distortion);
            postprocess = rootData.Find((FrameSettings d) => d.postprocess);

            // Rendering Settings
            litShaderMode = rootData.Find((FrameSettings d) => d.shaderLitMode);
            depthPrepassWithDeferredRendering = rootData.Find((FrameSettings d) => d.depthPrepassWithDeferredRendering);
            opaqueObjects = rootData.Find((FrameSettings d) => d.opaqueObjects);
            transparentObjects = rootData.Find((FrameSettings d) => d.transparentObjects);
            realtimePlanarReflection = rootData.Find((FrameSettings d) => d.realtimePlanarReflection);
            msaa = rootData.Find((FrameSettings d) => d.msaa);

            // Lighting Settings
            ssr = rootData.Find((FrameSettings d) => d.ssr);
            ssao = rootData.Find((FrameSettings d) => d.ssao);
            subsurfaceScattering = rootData.Find((FrameSettings d) => d.subsurfaceScattering);
            transmission = rootData.Find((FrameSettings d) => d.transmission);
            shadow = rootData.Find((FrameSettings d) => d.shadow);
            contactShadow = rootData.Find((FrameSettings d) => d.contactShadows);
            shadowMask = rootData.Find((FrameSettings d) => d.shadowMask);
            atmosphericScattering = rootData.Find((FrameSettings d) => d.atmosphericScattering);
            volumetrics = rootData.Find((FrameSettings d) => d.volumetrics);
            reprojectionForVolumetrics = rootData.Find((FrameSettings d) => d.reprojectionForVolumetrics);
            lightLayers = rootData.Find((FrameSettings d) => d.lightLayers);

            // Async Compute Settings
            asyncCompute = rootData.Find((FrameSettings d) => d.asyncCompute);
            buildLightListAsync = rootData.Find((FrameSettings d) => d.lightListAsync);
            ssrAsync = rootData.Find((FrameSettings d) => d.ssrAsync);
            ssaoAsync = rootData.Find((FrameSettings d) => d.ssaoAsync);
            contactShadowsAsync = rootData.Find((FrameSettings d) => d.contactShadowsAsync);
            volumeVoxelizationAsync = rootData.Find((FrameSettings d) => d.volumeVoxelizationAsync);

            // Light Loop Settings
            tileAndCluster = rootData.Find((FrameSettings l) => l.tileAndCluster);
            fptlForForwardOpaque = rootData.Find((FrameSettings l) => l.fptlForForwardOpaque);
            bigTilePrepass = rootData.Find((FrameSettings l) => l.bigTilePrepass);
            computeLightEvaluation = rootData.Find((FrameSettings l) => l.computeLightEvaluation);
            computeLightVariants = rootData.Find((FrameSettings l) => l.computeLightVariants);
            computeMaterialVariants = rootData.Find((FrameSettings l) => l.computeMaterialVariants);
        }
    }
}
