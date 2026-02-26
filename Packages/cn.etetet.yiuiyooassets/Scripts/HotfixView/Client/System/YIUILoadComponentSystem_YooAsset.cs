using System;
using UnityObject = UnityEngine.Object;

namespace ET.Client
{
    /// <summary>
    /// YooAsset扩展  因为他不需要pkgName
    /// </summary>
    [FriendOf(typeof(YIUILoadComponent))]
    public static partial class YIUILoadComponentSystem
    {
        #if !YIUIMACRO_SYNCLOAD_CLOSE
        internal static T LoadAsset<T>(this YIUILoadComponent self, string resName) where T : UnityObject
        {
            return self.LoadAsset<T>("", resName);
        }
        #endif

        internal static async ETTask<T> LoadAssetAsync<T>(this YIUILoadComponent self, string resName) where T : UnityObject
        {
            return await self.LoadAssetAsync<T>("", resName);
        }

        internal static void LoadAssetAsync<T>(this YIUILoadComponent self, string resName, Action<T> action) where T : UnityObject
        {
            self.LoadAssetAsync<T>("", resName, action);
        }

        internal static bool VerifyAssetValidity(this YIUILoadComponent self, string resName)
        {
            return self.VerifyAssetValidity("", resName);
        }

        #region 非泛型

        #if !YIUIMACRO_SYNCLOAD_CLOSE
        internal static UnityObject LoadAsset(this YIUILoadComponent self, string resName, Type assetType)
        {
            return self.LoadAsset("", resName, assetType);
        }
        #endif

        internal static async ETTask<UnityObject> LoadAssetAsync(this YIUILoadComponent self, string resName, Type assetType)
        {
            return await self.LoadAssetAsync("", resName, assetType);
        }

        internal static void LoadAssetAsync(this YIUILoadComponent self, string resName, Type assetType, Action<UnityObject> action)
        {
            self.LoadAssetAsync("", resName, assetType, action);
        }

        #endregion
    }
}