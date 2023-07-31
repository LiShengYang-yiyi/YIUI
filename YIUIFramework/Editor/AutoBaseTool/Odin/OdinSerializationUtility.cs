#if UNITY_EDITOR
using Sirenix.Serialization;
using UnityEngine;

namespace YIUIFramework.Editor
{
    /// <summary>
    /// odin 序列化文件存储 适用于大数据
    /// </summary>
    public static class OdinSerializationUtility
    {
        public static void Save<T>(T data, string path)
        {
            var bytes = SerializationUtility.SerializeValue(data, DataFormat.Binary);
            EditorHelper.WriteAllBytes(path, bytes);
        }

        public static T Load<T>(string path)
        {
            var bytes = EditorHelper.ReadAllBytes(path);
            if (bytes == null)
            {
                return default;
            }

            var data = SerializationUtility.DeserializeValue<T>(bytes, DataFormat.Binary);
            if (data == null)
            {
                Debug.LogError($"反序列化错误 {path}");
                return default;
            }

            return data;
        }
    }
}
#endif