using System;
using System.IO;
using ET;
using Sirenix.Serialization;
using UnityEngine;
using UnityObject = UnityEngine.Object;

namespace YIUIFramework
{
    /// <summary>
    /// odin 序列化文件存储 适用于大数据
    /// 只读 存储必须在编辑器下
    /// </summary>
    public static class OdinSerializationHelper
    {
        #if UNITY_EDITOR

        public static T EditorLoad<T>(string path, DataFormat dataFormat = DataFormat.JSON)
        {
            var bytes = ReadAllBytes(path);
            if (bytes == null)
            {
                return default;
            }

            var data = SerializationUtility.DeserializeValue<T>(bytes, dataFormat);
            if (data == null)
            {
                Debug.LogError($"反序列化错误 {path}");
                return default;
            }

            return data;
        }

        private static byte[] ReadAllBytes(string path)
        {
            try
            {
                if (!File.Exists(path))
                {
                    return null;
                }

                return File.ReadAllBytes(path);
            }
            catch (Exception e)
            {
                Debug.LogError("读取文件失败: path =" + path + ", err=" + e);
                return null;
            }
        }

        #endif

        public static async ETTask<T> RuntimeLoad<T>(Scene scene, string assetName, DataFormat dataFormat = DataFormat.JSON)
        {
            var loadResult = await EventSystem.Instance?.YIUIInvokeEntityAsync<YIUIInvokeEntity_Load, ETTask<UnityObject>>(scene, new YIUIInvokeEntity_Load
            {
                LoadType = typeof(TextAsset),
                ResName = assetName
            });

            if (loadResult == null)
            {
                return default;
            }

            var bytes = ((TextAsset)loadResult).bytes;
            if (bytes == null)
            {
                return default;
            }

            var data = SerializationUtility.DeserializeValue<T>(bytes, dataFormat);
            if (data == null)
            {
                Debug.LogError($"反序列化错误 {typeof(T).Name} {assetName}");
                return default;
            }

            return data;
        }
    }
}