using Object = UnityEngine.Object;

namespace YIUIFramework
{
    /// <summary>
    /// 同步加载
    /// </summary>
    internal static partial class YIUILoadHelper
    {
        /*
         * 这些方法都是给框架内部使用的 内部自行管理
         * 禁止  internal 改 public
         * 外部有什么加载 应该走自己框架中的加载方式 自行管理
         * 比如你想自己new一个obj 既然不是用UI框架内部提供的方法 那就应该你自行实现不要调用这个类
         */

        internal static T LoadAsset<T>(string pkgName, string resName) where T : Object
        {
            var load = LoadHelper.GetLoad(pkgName, resName);
            load.AddRefCount();
            var loadObj = load.Object;
            if (loadObj != null)
            {
                return (T)loadObj;
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
            return (T)obj;
        }
    }
}