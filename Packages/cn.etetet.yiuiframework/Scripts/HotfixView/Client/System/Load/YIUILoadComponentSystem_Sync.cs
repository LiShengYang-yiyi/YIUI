#if !YIUIMACRO_SYNCLOAD_CLOSE
using UnityObject = UnityEngine.Object;

namespace ET.Client
{
    /// <summary>
    /// 同步加载
    /// </summary>
    [FriendOf(typeof(YIUILoadComponent))]
    public static partial class YIUILoadComponentSystem
    {
        /*
         * 这些方法都是给框架内部使用的 内部自行管理
         * 禁止  internal 改 public
         * 外部有什么加载 应该走自己框架中的加载方式 自行管理
         * 比如你想自己new一个obj 既然不是用UI框架内部提供的方法 那就应该你自行实现不要调用这个类
         *
         * 注意***  webgl 是不允许使用同步加载的  ***注意
         */

        internal static T LoadAsset<T>(this YIUILoadComponent self, string pkgName, string resName) where T : UnityObject
        {
            var load = LoadHelper.GetLoad(pkgName, resName);
            load.AddRefCount();
            var loadObj = load.Object;
            if (loadObj != null)
            {
                if (loadObj is T assetLoadObj)
                {
                    return assetLoadObj;
                }
                else
                {
                    Log.Error($"资源类型不匹配, 期望类型: {typeof(T).Name}, 实际类型 {loadObj.GetType().Name} ,请检查资源, {pkgName},{resName}");
                    return null;
                }
            }

            var (obj, hashCode) = YIUILoadDI.LoadAssetFunc(pkgName, resName, typeof(T));
            if (obj == null)
            {
                load.RemoveRefCount();
                return null;
            }

            if (!LoadHelper.AddLoadHandle(obj, load))
            {
                load.RemoveRefCount();
                return null;
            }

            load.ResetHandle(obj, hashCode);

            if (obj is T assetObj)
            {
                return assetObj;
            }
            else
            {
                Log.Error($"资源类型不匹配, 期望类型: {typeof(T).Name}, 实际类型 {obj.GetType().Name} ,请检查资源, {pkgName},{resName}");
                return null;
            }
        }
    }
}
#endif