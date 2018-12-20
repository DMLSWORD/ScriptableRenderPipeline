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
            get => SerializedBitArrayUrtilities.Get128(rootOveride, (uint)FrameSettingsField.TransparentPrepass);
            set => SerializedBitArrayUrtilities.Set128(rootOveride, (uint)FrameSettingsField.TransparentPrepass, value);
        }
        public bool overridesTransparentPostpass
        {
            get => SerializedBitArrayUrtilities.Get128(rootOveride, (uint)FrameSettingsField.TransparentPostpass);
            set => SerializedBitArrayUrtilities.Set128(rootOveride, (uint)FrameSettingsField.TransparentPostpass, value);
        }
        public bool overridesMotionVectors
        {
            get => SerializedBitArrayUrtilities.Get128(rootOveride, (uint)FrameSettingsField.MotionVectors);
            set => SerializedBitArrayUrtilities.Set128(rootOveride, (uint)FrameSettingsField.MotionVectors, value);
        }
        public bool overridesObjectMotionVectors
        {
            get => SerializedBitArrayUrtilities.Get128(rootOveride, (uint)FrameSettingsField.ObjectMotionVectors);
            set => SerializedBitArrayUrtilities.Set128(rootOveride, (uint)FrameSettingsField.ObjectMotionVectors, value);
        }
        public bool overridesDecals
        {
            get => SerializedBitArrayUrtilities.Get128(rootOveride, (uint)FrameSettingsField.Decals);
            set => SerializedBitArrayUrtilities.Set128(rootOveride, (uint)FrameSettingsField.Decals, value);
        }
        public bool overridesRoughRefraction
        {
            get => SerializedBitArrayUrtilities.Get128(rootOveride, (uint)FrameSettingsField.RoughRefraction);
            set => SerializedBitArrayUrtilities.Set128(rootOveride, (uint)FrameSettingsField.RoughRefraction, value);
        }
        public bool overridesDistortion
        {
            get => SerializedBitArrayUrtilities.Get128(rootOveride, (uint)FrameSettingsField.Distortion);
            set => SerializedBitArrayUrtilities.Set128(rootOveride, (uint)FrameSettingsField.Distortion, value);
        }
        public bool overridesPostprocess
        {
            get => SerializedBitArrayUrtilities.Get128(rootOveride, (uint)FrameSettingsField.Postprocess);
            set => SerializedBitArrayUrtilities.Set128(rootOveride, (uint)FrameSettingsField.Postprocess, value);
        }

        // Rendering Settings
        public bool overridesShaderLitMode
        {
            get => SerializedBitArrayUrtilities.Get128(rootOveride, (uint)FrameSettingsField.ShaderLitMode);
            set => SerializedBitArrayUrtilities.Set128(rootOveride, (uint)FrameSettingsField.ShaderLitMode, value);
        }
        public bool overridesDepthPrepassWithDeferredRendering
        {
            get => SerializedBitArrayUrtilities.Get128(rootOveride, (uint)FrameSettingsField.DepthPrepassWithDeferredRendering);
            set => SerializedBitArrayUrtilities.Set128(rootOveride, (uint)FrameSettingsField.DepthPrepassWithDeferredRendering, value);
        }
        public bool overridesOpaqueObjects
        {
            get => SerializedBitArrayUrtilities.Get128(rootOveride, (uint)FrameSettingsField.OpaqueObjects);
            set => SerializedBitArrayUrtilities.Set128(rootOveride, (uint)FrameSettingsField.OpaqueObjects, value);
        }
        public bool overridesTransparentObjects
        {
            get => SerializedBitArrayUrtilities.Get128(rootOveride, (uint)FrameSettingsField.TransparentObjects);
            set => SerializedBitArrayUrtilities.Set128(rootOveride, (uint)FrameSettingsField.TransparentObjects, value);
        }
        public bool overridesRealtimePlanarReflection
        {
            get => SerializedBitArrayUrtilities.Get128(rootOveride, (uint)FrameSettingsField.RealtimePlanarReflection);
            set => SerializedBitArrayUrtilities.Set128(rootOveride, (uint)FrameSettingsField.RealtimePlanarReflection, value);
        }
        public bool overridesMSAA
        {
            get => SerializedBitArrayUrtilities.Get128(rootOveride, (uint)FrameSettingsField.MSAA);
            set => SerializedBitArrayUrtilities.Set128(rootOveride, (uint)FrameSettingsField.MSAA, value);
        }

        // Lighting Settings
        public bool overridesSSR
        {
            get => SerializedBitArrayUrtilities.Get128(rootOveride, (uint)FrameSettingsField.SSR);
            set => SerializedBitArrayUrtilities.Set128(rootOveride, (uint)FrameSettingsField.SSR, value);
        }
        public bool overridesSSAO
        {
            get => SerializedBitArrayUrtilities.Get128(rootOveride, (uint)FrameSettingsField.SSAO);
            set => SerializedBitArrayUrtilities.Set128(rootOveride, (uint)FrameSettingsField.SSAO, value);
        }
        public bool overridesSubsurfaceScattering
        {
            get => SerializedBitArrayUrtilities.Get128(rootOveride, (uint)FrameSettingsField.SubsurfaceScattering);
            set => SerializedBitArrayUrtilities.Set128(rootOveride, (uint)FrameSettingsField.SubsurfaceScattering, value);
        }
        public bool overridesTransmission
        {
            get => SerializedBitArrayUrtilities.Get128(rootOveride, (uint)FrameSettingsField.Transmission);
            set => SerializedBitArrayUrtilities.Set128(rootOveride, (uint)FrameSettingsField.Transmission, value);
        }
        public bool overridesShadow
        {
            get => SerializedBitArrayUrtilities.Get128(rootOveride, (uint)FrameSettingsField.Shadow);
            set => SerializedBitArrayUrtilities.Set128(rootOveride, (uint)FrameSettingsField.Shadow, value);
        }
        public bool overridesContactShadow
        {
            get => SerializedBitArrayUrtilities.Get128(rootOveride, (uint)FrameSettingsField.ContactShadow);
            set => SerializedBitArrayUrtilities.Set128(rootOveride, (uint)FrameSettingsField.ContactShadow, value);
        }
        public bool overridesShadowMask
        {
            get => SerializedBitArrayUrtilities.Get128(rootOveride, (uint)FrameSettingsField.ShadowMask);
            set => SerializedBitArrayUrtilities.Set128(rootOveride, (uint)FrameSettingsField.ShadowMask, value);
        }
        public bool overridesAtmosphericScaterring
        {
            get => SerializedBitArrayUrtilities.Get128(rootOveride, (uint)FrameSettingsField.AtmosphericScaterring);
            set => SerializedBitArrayUrtilities.Set128(rootOveride, (uint)FrameSettingsField.AtmosphericScaterring, value);
        }
        public bool overridesVolumetrics
        {
            get => SerializedBitArrayUrtilities.Get128(rootOveride, (uint)FrameSettingsField.Volumetrics);
            set => SerializedBitArrayUrtilities.Set128(rootOveride, (uint)FrameSettingsField.Volumetrics, value);
        }
        public bool overridesProjectionForVolumetrics
        {
            get => SerializedBitArrayUrtilities.Get128(rootOveride, (uint)FrameSettingsField.ReprojectionForVolumetrics);
            set => SerializedBitArrayUrtilities.Set128(rootOveride, (uint)FrameSettingsField.ReprojectionForVolumetrics, value);
        }
        public bool overridesLightLayers
        {
            get => SerializedBitArrayUrtilities.Get128(rootOveride, (uint)FrameSettingsField.LightLayers);
            set => SerializedBitArrayUrtilities.Set128(rootOveride, (uint)FrameSettingsField.LightLayers, value);
        }
        
        // Async Compute Settings
        public bool overridesAsyncCompute
        {
            get => SerializedBitArrayUrtilities.Get128(rootOveride, (uint)FrameSettingsField.AsyncCompute);
            set => SerializedBitArrayUrtilities.Set128(rootOveride, (uint)FrameSettingsField.AsyncCompute, value);
        }
        public bool overridesLightListAsync
        {
            get => SerializedBitArrayUrtilities.Get128(rootOveride, (uint)FrameSettingsField.LightListAsync);
            set => SerializedBitArrayUrtilities.Set128(rootOveride, (uint)FrameSettingsField.LightListAsync, value);
        }
        public bool overridesSSRAsync
        {
            get => SerializedBitArrayUrtilities.Get128(rootOveride, (uint)FrameSettingsField.SSRAsync);
            set => SerializedBitArrayUrtilities.Set128(rootOveride, (uint)FrameSettingsField.SSRAsync, value);
        }
        public bool overridesSSAOAsync
        {
            get => SerializedBitArrayUrtilities.Get128(rootOveride, (uint)FrameSettingsField.SSAOAsync);
            set => SerializedBitArrayUrtilities.Set128(rootOveride, (uint)FrameSettingsField.SSAOAsync, value);
        }
        public bool overridesContactShadowsAsync
        {
            get => SerializedBitArrayUrtilities.Get128(rootOveride, (uint)FrameSettingsField.ContactShadowsAsync);
            set => SerializedBitArrayUrtilities.Set128(rootOveride, (uint)FrameSettingsField.ContactShadowsAsync, value);
        }
        public bool overridesVolumeVoxelizationAsync
        {
            get => SerializedBitArrayUrtilities.Get128(rootOveride, (uint)FrameSettingsField.VolumeVoxelizationsAsync);
            set => SerializedBitArrayUrtilities.Set128(rootOveride, (uint)FrameSettingsField.VolumeVoxelizationsAsync, value);
        }

        // Light Loop Settings
        public bool overridesTileAndCluster
        {
            get => SerializedBitArrayUrtilities.Get128(rootOveride, (uint)FrameSettingsField.TileAndCluster);
            set => SerializedBitArrayUrtilities.Set128(rootOveride, (uint)FrameSettingsField.TileAndCluster, value);
        }
        public bool overridesFptlForForwardOpaque
        {
            get => SerializedBitArrayUrtilities.Get128(rootOveride, (uint)FrameSettingsField.FptlForForwardOpaque);
            set => SerializedBitArrayUrtilities.Set128(rootOveride, (uint)FrameSettingsField.FptlForForwardOpaque, value);
        }
        public bool overridesBigTilePrepass
        {
            get => SerializedBitArrayUrtilities.Get128(rootOveride, (uint)FrameSettingsField.BigTilePrepass);
            set => SerializedBitArrayUrtilities.Set128(rootOveride, (uint)FrameSettingsField.BigTilePrepass, value);
        }
        public bool overridesComputeLightEvaluation
        {
            get => SerializedBitArrayUrtilities.Get128(rootOveride, (uint)FrameSettingsField.ComputeLightEvaluation);
            set => SerializedBitArrayUrtilities.Set128(rootOveride, (uint)FrameSettingsField.ComputeLightEvaluation, value);
        }
        public bool overridesComputeLightVariants
        {
            get => SerializedBitArrayUrtilities.Get128(rootOveride, (uint)FrameSettingsField.ComputeLightVariants);
            set => SerializedBitArrayUrtilities.Set128(rootOveride, (uint)FrameSettingsField.ComputeLightVariants, value);
        }
        public bool overridesComputeMaterialVariants
        {
            get => SerializedBitArrayUrtilities.Get128(rootOveride, (uint)FrameSettingsField.ComputeMaterialVariants);
            set => SerializedBitArrayUrtilities.Set128(rootOveride, (uint)FrameSettingsField.ComputeMaterialVariants, value);
        }

        public SerializedFrameSettings(SerializedProperty rootData, SerializedProperty rootOverride)
        {
            this.rootData = rootData;
            this.rootOveride = rootOverride.FindPropertyRelative("mask");

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
