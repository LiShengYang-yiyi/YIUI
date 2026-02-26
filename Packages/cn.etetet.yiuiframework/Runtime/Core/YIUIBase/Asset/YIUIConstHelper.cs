using ET;
using System;
using UnityEngine;

#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using Sirenix.Serialization;
#endif

namespace YIUIFramework
{
    public static class YIUIConstHelper
    {
        public const string YIUIConstAssetName = "YIUIConstAsset";
        public const string YIUIConstAssetPath = "Assets/GameRes/YIUI/YIUISettings/YIUIConstAsset.txt";

        private static YIUIConstAsset m_YIUIConstAsset;

        #if !UNITY_EDITOR
        public static YIUIConstAsset Const => m_YIUIConstAsset;
        #endif

        public static async ETTask<bool> LoadAsset(Scene scene)
        {
            m_YIUIConstAsset = await OdinSerializationHelper.RuntimeLoad<YIUIConstAsset>(scene, YIUIConstAssetName);
            return m_YIUIConstAsset != null;
        }

        #if UNITY_EDITOR

        public static YIUIConstAsset Const
        {
            get
            {
                return m_YIUIConstAsset ??= LoadEditorAsset();
            }
        }

        private static YIUIConstAsset LoadEditorAsset()
        {
            var constAsset = OdinSerializationHelper.EditorLoad<YIUIConstAsset>(YIUIConstAssetPath);
            constAsset ??= CreateAsset();
            return constAsset;
        }

        private static void WriteAllBytes(string path, byte[] bytes)
        {
            try
            {
                string directoryPath = Path.GetDirectoryName(path);
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                File.WriteAllBytes(path, bytes);
            }
            catch (Exception e)
            {
                Debug.LogError("写入文件失败: path =" + path + ", err=" + e);
            }
        }

        private static YIUIConstAsset CreateAsset()
        {
            var constAsset = new YIUIConstAsset();
            var bytes = Sirenix.Serialization.SerializationUtility.SerializeValue(constAsset, DataFormat.JSON);
            WriteAllBytes($"{Application.dataPath}/../{YIUIConstAssetPath}", bytes);
            return constAsset;
        }

        public static YIUIConstAsset ResetAsset()
        {
            return m_YIUIConstAsset = CreateAsset();
        }

        #endif
    }
}