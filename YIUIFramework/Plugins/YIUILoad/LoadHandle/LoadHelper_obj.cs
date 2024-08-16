using System.Collections.Generic;
using UnityEngine;


namespace YIUIFramework
{
    internal static partial class LoadHelper
    {
        // <句柄的实例对象, 资源加载对象句柄>
        private static readonly Dictionary<Object, LoadHandle> s_ObjLoadHandle 
            = new Dictionary<Object, LoadHandle>();

        internal static bool AddLoadHandle(Object obj, LoadHandle handle)
        {
            if (s_ObjLoadHandle.ContainsKey(obj))
            {
                if (s_ObjLoadHandle[obj] != handle)
                {
                    Debug.LogError($"此obj {obj.name} Handle 已存在 且前后不一致 请检查 请勿创建多个");
                    return false; 
                }
                return true;
            }

            s_ObjLoadHandle.Add(obj, handle);
            return true;
        }

        private static bool RemoveLoadHandle(LoadHandle handle)
        {
            var obj = handle.AssetObject;
            if (obj == null)
            {
                return false;
            }

            return RemoveLoadHandle(obj);
        }

        private static bool RemoveLoadHandle(Object obj)
        {
            if (!s_ObjLoadHandle.ContainsKey(obj))
            {
                Debug.LogError($"此obj {obj.name} Handle 不存在 请检查 请先创建设置");
                return false;
            }

            return s_ObjLoadHandle.Remove(obj);
        }

        internal static LoadHandle GetLoadHandle(Object obj)
        {
            if (!s_ObjLoadHandle.ContainsKey(obj))
            {
                Debug.LogError($"此obj {obj.name} Handle 不存在 请检查 请先创建设置");
                return null;
            }

            return s_ObjLoadHandle[obj];
        }
    }
}