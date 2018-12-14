using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditorInternal;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Experimental.Rendering.HDPipeline;

namespace UnityEditor.Experimental.Rendering.HDPipeline
{
    public class HDEditorUtils
    {
        static readonly Action<SerializedProperty, GUIContent> k_DefaultDrawer = (p, l) => EditorGUILayout.PropertyField(p, l);

        delegate void MaterialResetter(Material material);
        static Dictionary<string, MaterialResetter> k_MaterialResetters = new Dictionary<string, MaterialResetter>()
        {
            { "HDRenderPipeline/LayeredLit",  LayeredLitGUI.SetupMaterialKeywordsAndPass },
            { "HDRenderPipeline/LayeredLitTessellation", LayeredLitGUI.SetupMaterialKeywordsAndPass },
            { "HDRenderPipeline/Lit", LitGUI.SetupMaterialKeywordsAndPass },
            { "HDRenderPipeline/LitTessellation", LitGUI.SetupMaterialKeywordsAndPass },
            { "HDRenderPipeline/Unlit", UnlitGUI.SetupMaterialKeywordsAndPass },
            // { "HDRenderPipeline/Fabric",  FabricGUI.SetupMaterialKeywordsAndPass },
            { "HDRenderPipeline/Decal", DecalUI.SetupMaterialKeywordsAndPass },
            { "HDRenderPipeline/TerrainLit", TerrainLitGUI.SetupMaterialKeywordsAndPass },
            { StackLitEditorGUI.k_StackLitShaderName, StackLitEditorGUI.SetupMaterialKeywordsAndPass },
        };

        const string k_MaterialShaderNameRegexPattern = @"\A"
            + TextureSamplerSharingShaderGenerator.k_StackLitFamilyFindRegexPattern //+ @"(?<shadername>HDRenderPipeline\/StackLit)(\/Generated\/(?<statecode>[0-9a-fA-F]{32}))?"
            //+ @"(?<shadername>HDRenderPipeline\/LayeredLit)"
            //+ @"|(?<shadername>HDRenderPipeline\/LayeredLitTessellation)"
            //+ @"|(?<shadername>HDRenderPipeline\/Unlit)"
            //+ @"|(?<shadername>HDRenderPipeline\/Fabric)"
            //+ @"|(?<shadername>HDRenderPipeline\/Decal)"
            //+ @"|(?<shadername>HDRenderPipeline\/TerrainLit)"
            + @"\z";
        static Regex k_MaterialShaderNameRegex = new Regex(k_MaterialShaderNameRegexPattern, RegexOptions.ExplicitCapture| RegexOptions.Compiled);

        private static bool TryGetMaterialResetter(string shaderName, out MaterialResetter resetter)
        {
            // First try to find without filtering the name
            if (k_MaterialResetters.TryGetValue(shaderName, out resetter))
            {
                return true;
            }
            Match match = k_MaterialShaderNameRegex.Match(shaderName);
            if (match.Success)
            {
                shaderName = match.Groups["shadername"].Value;
                if (k_MaterialResetters.TryGetValue(shaderName, out resetter))
                {
                    return true;
                }
            }
            return false;
        }

        public static T LoadAsset<T>(string relativePath) where T : UnityEngine.Object
        {
            return AssetDatabase.LoadAssetAtPath<T>(HDUtils.GetHDRenderPipelinePath() + relativePath);
        }

        public static bool ResetMaterialKeywords(Material material)
        {
            MaterialResetter resetter;
            if (TryGetMaterialResetter(material.shader.name, out resetter))
            {
                CoreEditorUtils.RemoveMaterialKeywords(material);
                // We need to reapply ToggleOff/Toggle keyword after reset via ApplyMaterialPropertyDrawers
                MaterialEditor.ApplyMaterialPropertyDrawers(material);
                resetter(material);
                EditorUtility.SetDirty(material);
                return true;
            }
            return false;
        }

        public static List<BaseShaderPreprocessor> GetBaseShaderPreprocessorList()
        {
            var baseType = typeof(BaseShaderPreprocessor);
            var assembly = baseType.Assembly;

            var types = assembly.GetTypes()
                .Where(t => t.IsSubclassOf(baseType))
                .Select(Activator.CreateInstance)
                .Cast<BaseShaderPreprocessor>()
                .ToList();

            return types;
        }

        static readonly GUIContent s_OverrideTooltip = CoreEditorUtils.GetContent("|Override this setting in component.");
        public static bool FlagToggle<TEnum>(TEnum v, SerializedProperty property)
            where TEnum : struct, IConvertible // restrict to ~enum
        {
            var intV = (int)(object)v;
            var isOn = (property.intValue & intV) != 0;
            var rect = ReserveAndGetFlagToggleRect();
            isOn = GUI.Toggle(rect, isOn, s_OverrideTooltip, CoreEditorStyles.smallTickbox);
            if (isOn)
                property.intValue |= intV;
            else
                property.intValue &= ~intV;

            return isOn;
        }

        public static Rect ReserveAndGetFlagToggleRect()
        {
            var rect = GUILayoutUtility.GetRect(11, 17, GUILayout.ExpandWidth(false));
            rect.y += 4;
            return rect;
        }

        public static void PropertyFieldWithOptionalFlagToggle<TEnum>(
            TEnum v, SerializedProperty property, GUIContent label,
            SerializedProperty @override, bool showOverrideButton,
            Action<SerializedProperty, GUIContent> drawer = null
        )
            where TEnum : struct, IConvertible // restrict to ~enum
        {
            EditorGUILayout.BeginHorizontal();

            var i = EditorGUI.indentLevel;
            var l = EditorGUIUtility.labelWidth;
            EditorGUI.indentLevel = 0;
            EditorGUIUtility.labelWidth = 0;

            if (showOverrideButton)
                GUI.enabled = GUI.enabled && FlagToggle(v, @override);
            else
                ReserveAndGetFlagToggleRect();
            (drawer ?? k_DefaultDrawer)(property, label);

            GUI.enabled = true;
            EditorGUI.indentLevel = i;
            EditorGUIUtility.labelWidth = l;

            EditorGUILayout.EndHorizontal();
        }

        public static void PropertyFieldWithFlagToggleIfDisplayed<TEnum>(
            TEnum v, SerializedProperty property, GUIContent label,
            SerializedProperty @override,
            TEnum displayed, TEnum overrideable,
            Action<SerializedProperty, GUIContent> drawer = null
        )
            where TEnum : struct, IConvertible // restrict to ~enum
        {
            var intDisplayed = (int)(object)displayed;
            var intV = (int)(object)v;
            if ((intDisplayed & intV) == intV)
            {
                var intOverridable = (int)(object)overrideable;
                var isOverrideable = (intOverridable & intV) == intV;
                PropertyFieldWithOptionalFlagToggle(v, property, label, @override, isOverrideable, drawer);
            }
        }

        public static bool DrawSectionFoldout(string title, bool isExpanded)
        {
            CoreEditorUtils.DrawSplitter(false);
            return CoreEditorUtils.DrawHeaderFoldout(title, isExpanded, false);
        }

        static internal void DrawToolBarButton<TEnum>(
            TEnum button, Editor owner,
            Dictionary<TEnum, EditMode.SceneViewEditMode> toolbarMode,
            Dictionary<TEnum, GUIContent> toolbarContent,
            params GUILayoutOption[] options
        )
            where TEnum : struct, IConvertible
        {
            var intButton = (int)(object)button;
            bool enabled = toolbarMode[button] == EditMode.editMode;
            EditorGUI.BeginChangeCheck();
            enabled = GUILayout.Toggle(enabled, toolbarContent[button], EditorStyles.miniButton, options);
            if (EditorGUI.EndChangeCheck())
            {
                EditMode.SceneViewEditMode targetMode = EditMode.editMode == toolbarMode[button] ? EditMode.SceneViewEditMode.None : toolbarMode[button];
                EditMode.ChangeEditMode(targetMode, GetBoundsGetter(owner)(), owner);
            }
        }

        internal static Func<Bounds> GetBoundsGetter(Editor o)
        {
            return () =>
            {
                var bounds = new Bounds();
                var rp = ((Component)o.target).transform;
                var b = rp.position;
                bounds.Encapsulate(b);
                return bounds;
            };
        }
    }
}
