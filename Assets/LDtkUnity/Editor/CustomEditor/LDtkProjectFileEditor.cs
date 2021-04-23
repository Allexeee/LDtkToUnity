﻿using UnityEditor;

namespace LDtkUnity.Editor
{
    [CustomEditor(typeof(LDtkProjectFile))]
    public class LDtkProjectFileEditor : LDtkJsonFileEditor<LdtkJson>
    {
        private int? _levelCount = null;
        
        protected override void DrawInspectorGUI(LdtkJson project)
        {
            DrawVersion(project);

            Level[] levels = project.Levels;
            
            if (levels == null)
            {
                return;
            }

            DrawLevelCount(levels);
        }

        private static void DrawVersion(LdtkJson project)
        {
            string version = $"Json Version: {project.JsonVersion}";
            EditorGUILayout.LabelField(version);
        }

        private void DrawLevelCount(Level[] levels)
        {
            if (_levelCount == null)
            {
                _levelCount = levels.Length;
            }
            string levelNaming = _levelCount == 1 ? "Level" : "Levels";
            EditorGUILayout.LabelField($"{_levelCount} {levelNaming}");
        }
    }
}