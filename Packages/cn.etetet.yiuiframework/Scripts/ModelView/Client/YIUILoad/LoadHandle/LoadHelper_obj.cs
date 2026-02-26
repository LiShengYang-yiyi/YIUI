using System.Collections.Generic;
using UnityEngine;
using UnityObject = UnityEngine.Object;

namespace ET.Client
{
    public static partial class LoadHelper
    {
        [StaticField]
        private static readonly Dictionary<UnityObject, LoadHandle> m_ObjLoadHandle = new();

        public static bool AddLoadHandle(UnityObject obj, LoadHandle handle)
        {
            if (m_ObjLoadHandle.TryGetValue(obj, out LoadHandle value))
            {
                if (value != handle)
                {
                    Debug.LogError($"此obj {obj.name} Handle 已存在 且前后不一致 请检查 请勿创建多个");
                    return false;
                }

                return true;
            }

            m_ObjLoadHandle.Add(obj, handle);
            return true;
        }

        private static bool RemoveLoadHandle(LoadHandle handle)
        {
            var obj = handle.Object;
            if (obj == null)
            {
                return false;
            }

            return RemoveLoadHandle(obj);
        }

        private static bool RemoveLoadHandle(UnityObject obj)
        {
            if (!m_ObjLoadHandle.ContainsKey(obj))
            {
                Debug.LogError($"此obj {obj.name} Handle 不存在 请检查 请先创建设置");
                return false;
            }

            return m_ObjLoadHandle.Remove(obj);
        }

        public static LoadHandle GetLoadHandle(UnityObject obj)
        {
            if (!m_ObjLoadHandle.TryGetValue(obj, out LoadHandle handle))
            {
                Debug.LogError($"此obj {obj.name} Handle 不存在 请检查 请先创建设置");
                return null;
            }

            return handle;
        }
    }
}
