using System;
using UnityEngine;
using Object = UnityEngine.Object;
using Cysharp.Threading.Tasks;

namespace YIUIFramework
{
    /// <summary>
    /// 异步加载
    /// </summary>
    internal static partial class YIUILoadHelper
    {
        /// <summary>
        /// 异步加载资源对象
        /// 返回类型
        /// </summary>
        internal static async UniTask<T> LoadAssetAsync<T>(string pkgName, string resName) where T : Object
        {
            // 如果有缓存则取缓存里的
            var load = LoadHelper.GetLoad(pkgName, resName);
            load.AddRefCount();
            var loadObj = load.AssetObject;
            if (loadObj != null)
            {
                return (T)loadObj;
            }

            // 否则等待其他对象的异步加载，触发此处的原因在下面
            if (load.WaitAsync)
            {
                await UniTask.WaitUntil(() => !load.WaitAsync);

                loadObj = load.AssetObject;

                if (loadObj != null)
                {
                    return (T)loadObj;
                }

                load.RemoveRefCount();
                return null;
            }

            // 打开开始异步加载
            load.SetWaitAsync(true);
            var (obj, hashCode) = await YIUILoadDI.LoadAssetAsyncFunc(pkgName, resName, typeof(T));

            // 加载失败
            if (obj == null)
            {
                load.SetWaitAsync(false);
                load.RemoveRefCount();
                return null;
            }

            // 添加加载句柄失败，重复创建资产
            if (!LoadHelper.AddLoadHandle(obj, load))
            {
                load.SetWaitAsync(false);
                load.RemoveRefCount();
                return null;
            }

            load.ResetHandle(obj, hashCode);
            load.SetWaitAsync(false);
            return (T)obj;
        }

        /// <summary>
        /// 异步加载资源对象
        /// 回调类型，回调参数为加载成功的资源对象
        /// </summary>
        internal static void LoadAssetAsync<T>(string pkgName, string resName, Action<T> action) where T : Object
        {
            LoadAssetAsyncAction(pkgName, resName, action).Forget();
        }

        private static async UniTaskVoid LoadAssetAsyncAction<T>(string pkgName, string resName, Action<T> action)
            where T : Object
        {
            var asset = await LoadAssetAsync<T>(pkgName, resName);
            if (asset == null)
            {
                Debug.LogError($"异步加载对象失败 {pkgName} {resName}");
                return;
            }

            action?.Invoke(asset);
        }
    }
}