using UnityObject = UnityEngine.Object;

namespace ET.Client
{
    [FriendOf(typeof(YIUILoadComponent))]
    public static partial class YIUILoadComponentSystem
    {
        /// <summary>
        /// 资源验证
        /// </summary>
        internal static bool VerifyAssetValidity(this YIUILoadComponent self, string pkgName, string resName)
        {
            return YIUILoadDI.VerifyAssetValidityFunc(pkgName, resName);
        }

        /// <summary>
        /// 释放某个资源对象
        /// 不包含实例化的对象
        /// 实例化的对象请调用另外一个实例化释放 ReleaseInstantiate
        /// </summary>
        internal static void Release(this YIUILoadComponent self, UnityObject obj)
        {
            LoadHelper.GetLoadHandle(obj)?.RemoveRefCount();
        }

        /// <summary>
        /// 一键释放所有 慎用
        /// </summary>
        internal static void ReleaseAll(this YIUILoadComponent self)
        {
            LoadHelper.PutAll();
            YIUILoadDI.ReleaseAllAction?.Invoke();
        }
    }
}
