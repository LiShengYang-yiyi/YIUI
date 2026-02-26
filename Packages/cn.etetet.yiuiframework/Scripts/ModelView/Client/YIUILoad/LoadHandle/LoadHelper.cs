using System.Collections.Generic;
using YIUIFramework;

namespace ET.Client
{
    [EnableClass]
    public static partial class LoadHelper
    {
        private const string c_NullPkgName = "Default";

        [StaticField]
        private static readonly Dictionary<string, Dictionary<string, LoadHandle>> m_AllLoadDic = new();

        public static LoadHandle GetLoad(string pkgName, string resName)
        {
            if (string.IsNullOrEmpty(pkgName))
            {
                pkgName = c_NullPkgName;
            }

            if (!m_AllLoadDic.ContainsKey(pkgName))
            {
                m_AllLoadDic.Add(pkgName, new Dictionary<string, LoadHandle>());
            }

            var pkgDic = m_AllLoadDic[pkgName];

            if (!pkgDic.ContainsKey(resName))
            {
                var group = RefPool.Get<LoadHandle>();
                group.SetGroupHandle(pkgName, resName);
                pkgDic.Add(resName, group);
            }

            return pkgDic[resName];
        }

        public static bool PutLoad(string pkgName, string resName)
        {
            if (string.IsNullOrEmpty(pkgName))
            {
                pkgName = c_NullPkgName;
            }

            if (!m_AllLoadDic.TryGetValue(pkgName, out var pkgDic))
            {
                return false;
            }

            if (!pkgDic.Remove(resName, out LoadHandle load))
            {
                return false;
            }

            RemoveLoadHandle(load);
            RefPool.Put(load);
            return true;
        }

        public static void PutAll()
        {
            foreach (var pkgDic in m_AllLoadDic.Values)
            {
                foreach (var load in pkgDic.Values)
                {
                    RefPool.Put(load);
                }
            }

            m_AllLoadDic.Clear();
            m_ObjLoadHandle.Clear();
        }
    }
}