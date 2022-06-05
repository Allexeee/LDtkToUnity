﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Object = UnityEngine.Object;

#if UNITY_2020_2_OR_NEWER

#else
using UnityEditor.Experimental.AssetImporters;
#endif

namespace LDtkUnity.Editor
{
    public static class LDtkDependencyUtil
    {
        private const string NULL = "{instanceID: 0}";

        public static string[] LoadMetaLinesAtProjectPath(string projectPath)
        {
            string metaPath = projectPath + ".meta";

            if (!File.Exists(metaPath))
            {
                LDtkDebug.LogError($"The project meta file cannot be found at \"{metaPath}\", Check that there are no broken paths. Most likely the project was renamed but not re-saved in LDtk yet. save the project in LDtk to potentially fix this problem");
                return Array.Empty<string>();
            }

            string[] lines = File.ReadAllLines(metaPath, Encoding.ASCII);
            return lines;
        }


        public static List<ParsedMetaData> GetMetaDatas(string[] lines)
        {
            List<ParsedMetaData> metaData = new List<ParsedMetaData>();
            

            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];

                if (AddCustomPrefabLevel(line, metaData))
                {
                    continue;
                }

                if (!line.Contains(LDtkAsset<Object>.PROPERTY_KEY))
                {
                    continue;
                }
                string nextLine = lines[i+1];
                TryAddLDtkAsset(nextLine, line, metaData);
            }

            return metaData;
        }

        private static void TryAddLDtkAsset(string nextLine, string line, List<ParsedMetaData> metaData)
        {
            if (nextLine.Contains(NULL))
            {
                return;
            }
            
            ParsedMetaData meta = new ParsedMetaData();

            //name
            string[] strings = line.Split(' ');
            meta.Name = strings[4];

            //guid
            int indexOf = nextLine.IndexOf("guid:", StringComparison.InvariantCulture);
            string substring = nextLine.Substring(indexOf);
            string guid = substring.Split(' ')[1];
            meta.Guid = guid.TrimEnd(',');

            //Debug.Log($"parsed array item {meta}");
            metaData.Add(meta);
        }
        
        private static bool AddCustomPrefabLevel(string line, List<ParsedMetaData> metaData)
        {
            //custom level prefabs can look like this
            //_customLevelPrefab: {fileID: 8588321673598725224, guid: fe05cfa93bba52540971cb633e22bfbe, type: 3}
            //_customLevelPrefab: {instanceID: 0}
            if (line.Contains("_customLevelPrefab") && !line.Contains(NULL))
            {
                ParsedMetaData meta = new ParsedMetaData();

                //name
                string[] tokens = line.Split(' ');
                meta.Name = tokens[2].TrimEnd(':');

                //guid
                int indexOf = line.IndexOf("guid:", StringComparison.InvariantCulture);
                string substring = line.Substring(indexOf);
                string guid = substring.Split(' ')[1];
                meta.Guid = guid.TrimEnd(',');

                metaData.Add(meta);

                //Debug.Log($"parsed custom level {meta}");
                return true;
            }

            return false;
        }

        public static void TestLogDependencySet(string functionName, string importerPath, string dependencyPath)
        {
            //used for testing
            //Debug.Log($"LDtk: {functionName} <color=yellow>{Path.GetFileNameWithoutExtension(importerPath)}</color>:<color=navy>{Path.GetFileName(dependencyPath)}</color>");
        }
    }
}