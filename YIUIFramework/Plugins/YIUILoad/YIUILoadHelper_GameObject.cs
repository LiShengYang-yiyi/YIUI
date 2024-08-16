using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;
using Cysharp.Threading.Tasks;

namespace YIUIFramework
{
    /// <summary>
    /// UI用 加载器
    /// 扩展 GameObject快捷方法 需成对使用
    /// </summary>
    internal static partial class YIUILoadHelper
    {
        private static readonly Dictionary<Object, Object> s_ObjectMap = new Dictionary<Object, Object>();

        /// <summary>
        /// 同步加载 并实例化
        /// </summary>
        internal static GameObject LoadAssetInstantiate(string pkgName, string resName)
        {
            var asset = LoadAsset<GameObject>(pkgName, resName);
            if (asset == null) return null;
            var obj = Object.Instantiate(asset);
            s_ObjectMap.Add(obj, asset);
            return obj;
        }

        /// <summary>
        /// 异步加载 并实例化
        /// </summary>
        internal static async UniTask<GameObject> LoadAssetAsyncInstantiate(string pkgName, string resName)
        {
            var asset = await LoadAssetAsync<GameObject>(pkgName, resName);
            if (asset == null) return null;
            var obj = Object.Instantiate(asset);
            s_ObjectMap.Add(obj, asset);
            return obj;
        }

        /// <summary>
        /// 异步加载资源对象
        /// 回调类型，回调参数为加载成功的资源实例对象
        /// </summary>
        internal static async UniTask LoadAssetAsyncInstantiate(string pkgName, string resName, Action<Object> action)
        {
            var obj = await LoadAssetAsyncInstantiate(pkgName, resName);
            if (obj == null)
            {
                Debug.LogError($"异步加载对象失败 {pkgName} {resName}");
                return;
            }

            action?.Invoke(obj);
        }

        /// <summary> 释放由资源对象实例化出来的实例GameObject </summary>
        internal static void ReleaseInstantiate(Object gameObject)
        {
            if (!s_ObjectMap.Remove(gameObject, out var asset))
            {
                return;
            }
            
            Release(asset);
        }
    }
}