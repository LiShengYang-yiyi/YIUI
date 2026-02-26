using System;
using System.Collections.Generic;
using System.IO;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using UnityEngine;

namespace YIUIFramework.Editor
{
    [BsonIgnoreExtraElements]
    public class PackageGit
    {
        public int                          Id;
        public string                       Name;
        public Dictionary<string, string>   GitDependencies;
        public Dictionary<string, string[]> ScriptsReferences;
    }

    public static class PackageGitHelper
    {
        public static PackageGit Load(string packageJsonPath)
        {
            if (!File.Exists(packageJsonPath))
            {
                Debug.LogError($"此文件不存在: {packageJsonPath}");
                return null;
            }

            string packageJsonContent = File.ReadAllText(packageJsonPath);

            PackageGit packageGit = BsonSerializer.Deserialize<PackageGit>(packageJsonContent);
            return packageGit;
        }
    }
}