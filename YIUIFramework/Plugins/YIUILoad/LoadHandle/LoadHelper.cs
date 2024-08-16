using System.Collections.Generic;

namespace YIUIFramework
{
    internal static partial class LoadHelper
    {
        /// <summary> 默认包名 </summary>
        private const string NullPkgName = "Default";

        // <包名，<资源名，加载资源对象句柄>>
        private static readonly Dictionary<string, Dictionary<string, LoadHandle>> s_AllLoadDic =
            new Dictionary<string, Dictionary<string, LoadHandle>>();


        /// <summary> 通过包名与资源名获取加载资源对象句柄 </summary>
        internal static LoadHandle GetLoad(string pkgName, string resName)
        {
            if (string.IsNullOrEmpty(pkgName))
            {
                pkgName = NullPkgName;
            }

            if (!s_AllLoadDic.ContainsKey(pkgName))
            {
                s_AllLoadDic.Add(pkgName, new Dictionary<string, LoadHandle>());
            }

            var pkgDic = s_AllLoadDic[pkgName];

            if (!pkgDic.ContainsKey(resName))
            {
                var group = RefPool.Get<LoadHandle>();
                group.SetGroupHandle(pkgName, resName);
                pkgDic.Add(resName, group);
            }

            return pkgDic[resName];
        }

        /// <summary> 通过包名与资源名移除加载资源对象句柄 </summary>
        internal static bool PutLoad(string pkgName, string resName)
        {
            if (string.IsNullOrEmpty(pkgName))
            {
                pkgName = NullPkgName;
            }

            if (!s_AllLoadDic.ContainsKey(pkgName))
            {
                return false;
            }

            var pkgDic = s_AllLoadDic[pkgName];

            if (!pkgDic.ContainsKey(resName))
            {
                return false;
            }

            var load = pkgDic[resName];
            pkgDic.Remove(resName);
            RemoveLoadHandle(load);
            RefPool.Put(load);
            return true;
        }

        /// <summary> 移除所有资源加载对象 </summary>
        internal static void PutAll()
        {
            foreach (var pkgDic in s_AllLoadDic.Values)
            {
                foreach (var load in pkgDic.Values)
                {
                    RefPool.Put(load);
                }
                pkgDic.Clear();
            }

            s_AllLoadDic.Clear();
            s_ObjLoadHandle.Clear();
        }
    }
}