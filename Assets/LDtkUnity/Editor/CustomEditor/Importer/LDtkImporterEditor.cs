﻿using UnityEditor;
using UnityEngine;
using UnityEngine.Internal;

#if UNITY_2020_2_OR_NEWER
using UnityEditor.AssetImporters;
#else
using UnityEditor.Experimental.AssetImporters;
#endif

namespace LDtkUnity.Editor
{
    //we are not overriding RenderStaticPreview because it didn't work for scripted importers
    [ExcludeFromDocs]
    public abstract class LDtkImporterEditor : ScriptedImporterEditor
    {
        public override bool showImportedObject => false; // disables the interactable asset preview
        protected override bool useAssetDrawPreview => false; // disables the gray object under the UI
        //protected override bool ShouldHideOpenButton() => false;

        protected static void DrawBreakingError()
        {
            const string errorContent = "There was a breaking import error; Try reimporting this asset, which might fix it.\n" +
                                        "Check if there are any import errors in the console window, and report to the developer so that it can be addressed.";

            using (new LDtkIconSizeScope(Vector2.one * 32))
            {
                EditorGUILayout.HelpBox(errorContent, MessageType.Error);
            }
        }
    }
}