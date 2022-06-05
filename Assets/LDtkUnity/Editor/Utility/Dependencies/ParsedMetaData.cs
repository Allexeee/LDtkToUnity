﻿using UnityEditor;

namespace LDtkUnity.Editor
{
    public struct ParsedMetaData
    {
        public string Name;
        public string Guid;
            
        public string GetAssetPath() => AssetDatabase.GUIDToAssetPath(Guid);
        public override string ToString() => $"\"{Name}\" {Guid}";
    }
}