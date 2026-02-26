using System;
using UnityEngine;
using UnityObject = UnityEngine.Object;

namespace ET.Client
{
    /// <summary>
    /// UI用 加载器
    /// 扩展 GameObject快捷方法 需成对使用
    /// </summary>
    [FriendOf(typeof(YIUILoadComponent))]
    public static partial class YIUILoadComponentSystem
    {
        #if !YIUIMACRO_SYNCLOAD_CLOSE
        /// <summary>
        /// 同步加载 并实例化
        /// </summary>
        public static GameObject LoadAssetInstantiate(this YIUILoadComponent self, string pkgName, string resName)
        {
            var asset = self.LoadAsset<GameObject>(pkgName, resName);
            if (asset == null) return null;
            var obj = UnityObject.Instantiate(asset);
            YIUILoadHelperStatic.g_ObjectMap.Add(obj, asset);
            return obj;
        }
        #endif

        /// <summary>
        /// 异步加载 并实例化
        /// </summary>
        public static async ETTask<GameObject> LoadAssetAsyncInstantiate(this YIUILoadComponent self, string pkgName, string resName)
        {
            var asset = await self.LoadAssetAsync<GameObject>(pkgName, resName);
            if (asset == null) return null;
            var obj = UnityObject.Instantiate(asset);
            YIUILoadHelperStatic.g_ObjectMap.Add(obj, asset);
            return obj;
        }

        /// <summary>
        /// 异步加载资源对象
        /// 回调类型
        /// </summary>
        public static async ETTask LoadAssetAsyncInstantiate(this YIUILoadComponent self, string pkgName, string resName, Action<UnityObject> action)
        {
            var obj = await self.LoadAssetAsyncInstantiate(pkgName, resName);
            if (obj == null)
            {
                Debug.LogError($"异步加载对象失败 {pkgName} {resName}");
                return;
            }

            action?.Invoke(obj);
        }

        /// <summary>
        /// 释放由 实例化出来的GameObject
        /// </summary>
        public static void ReleaseInstantiate(this YIUILoadComponent self, UnityObject gameObject)
        {
            if (!YIUILoadHelperStatic.g_ObjectMap.Remove(gameObject, out var asset)) return;
            self.Release(asset);
        }
    }
}