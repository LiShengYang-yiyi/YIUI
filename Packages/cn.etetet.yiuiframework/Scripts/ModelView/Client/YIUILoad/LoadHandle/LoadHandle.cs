using System;
using YIUIFramework;
using UnityObject = UnityEngine.Object;

namespace ET.Client
{
    [IgnoreCircularDependency]
    [EnableClass]
    public class LoadHandle : IRefPool
    {
        internal string PkgName { get; private set; }
        internal string ResName { get; private set; }
        internal int Handle { get; private set; }
        internal int RefCount { get; private set; }
        public UnityObject Object { get; private set; }
        private long m_NameCode;
        public long NameCode => m_NameCode;

        internal void SetGroupHandle(string pkgName, string resName)
        {
            PkgName = pkgName;
            ResName = resName;
            m_NameCode = IdGenerater.Instance.GenerateId();
        }

        public void Recycle()
        {
            PkgName = string.Empty;
            ResName = string.Empty;
            Handle = 0;
            RefCount = 0;
            m_NameCode = 0;
            Object = null;
        }

        public void ResetHandle(UnityObject obj, int handle)
        {
            Object = obj;
            Handle = handle;
        }

        public void AddRefCount()
        {
            RefCount++;
        }

        public void RemoveRefCount()
        {
            RefCount--;
            if (RefCount <= 0)
            {
                Release();
            }
        }

        private void Release()
        {
            if (Handle != 0)
            {
                YIUILoadDI.ReleaseAction?.Invoke(Handle);
            }

            LoadHelper.PutLoad(PkgName, ResName);
        }
    }
}