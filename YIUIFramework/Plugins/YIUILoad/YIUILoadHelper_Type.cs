﻿using System;
using UnityEngine;
using Object = UnityEngine.Object;
using Cysharp.Threading.Tasks;

namespace YIUIFramework
{
    /// <summary>
    /// 不使用泛型 使用type加载的方式
    /// </summary>
    internal static partial class YIUILoadHelper
    {
        internal static Object LoadAsset(string pkgName, string resName, Type assetType)
        {
            // 如果有缓存则取缓存里的
            var load = LoadHelper.GetLoad(pkgName, resName);
            load.AddRefCount();
            var loadObj = load.AssetObject;
            if (loadObj != null)
            {
                return loadObj;
            }

            // 开始加载
            var (obj, hashCode) = YIUILoadDI.LoadAssetFunc(pkgName, resName, assetType);
            
            // 加载失败
            if (obj == null)
            {
                load.RemoveRefCount();
                return null;
            }

            // 添加加载句柄失败，重复创建资产
            if (!LoadHelper.AddLoadHandle(obj, load))
            {
                load.RemoveRefCount();
                return null;
            }

            load.ResetHandle(obj, hashCode);
            return obj;
        }

        internal static async UniTask<Object> LoadAssetAsync(string pkgName, string resName, Type assetType)
        {
            // 如果有缓存则取缓存里的
            var load = LoadHelper.GetLoad(pkgName, resName);
            load.AddRefCount();
            var loadObj = load.AssetObject;
            if (loadObj != null)
            {
                return loadObj;
            }

            // 否则等待其他对象的异步加载，触发此处的原因在下面
            if (load.WaitAsync)
            {
                await UniTask.WaitUntil(() => !load.WaitAsync);

                loadObj = load.AssetObject;
                if (loadObj != null)
                {
                    return loadObj;
                }

                load.RemoveRefCount();
                return null;
            }

            // 打开开始异步加载
            load.SetWaitAsync(true);
            var (obj, hashCode) = await YIUILoadDI.LoadAssetAsyncFunc(pkgName, resName, assetType);

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
            return obj;
        }

        /// <summary>
        /// 异步加载资产对象
        /// 回调参数为加载成功的资产对象
        /// </summary>
        internal static void LoadAssetAsync(string pkgName, string resName, Type assetType, Action<Object> action)
        {
            LoadAssetAsyncAction(pkgName, resName, assetType, action).Forget();
        }

        private static async UniTaskVoid LoadAssetAsyncAction(
            string         pkgName,
            string         resName,
            Type           assetType,
            Action<Object> action)
        {
            var asset = await LoadAssetAsync(pkgName, resName, assetType);
            if (asset == null)
            {
                Debug.LogError($"异步加载对象失败 {pkgName} {resName}");
                return;
            }

            action?.Invoke(asset);
        }
    }
}