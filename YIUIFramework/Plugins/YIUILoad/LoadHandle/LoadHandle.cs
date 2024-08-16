using Object = UnityEngine.Object;


namespace YIUIFramework
{
    /// <summary> 加载对象句柄 </summary>
    internal class LoadHandle : IRefPool
    {
        internal string PkgName     { get; private set; }
        internal string ResName     { get; private set; }
        internal Object AssetObject { get; private set; }
        internal int    Handle      { get; private set; }
        internal int    RefCount    { get; private set; }
        internal bool   WaitAsync   { get; private set; }

        /// <summary> 设置包名与资源名 </summary>
        internal void SetGroupHandle(string pkgName, string resName)
        {
            PkgName = pkgName;
            ResName = resName;
        }

        /// <summary> 重置句柄与对象 </summary>
        internal void ResetHandle(Object obj, int handle)
        {
            AssetObject = obj;
            Handle = handle;
        }

        /// <summary> 设置是否异步等待 </summary>
        internal void SetWaitAsync(bool value)
        {
            WaitAsync = value;
        }

        /// <summary> 添加引用 </summary>
        internal void AddRefCount()
        {
            RefCount++;
        }

        /// <summary> 移除引用 </summary>
        internal void RemoveRefCount()
        {
            RefCount--;
            if (RefCount <= 0)
            {
                Release();
            }
        }

        /// <summary> 对象池回收对象时操作 </summary>
        public void Recycle()
        {
            PkgName  = string.Empty;
            ResName  = string.Empty;
            Handle   = 0;
            RefCount = 0;
            AssetObject   = null;
        }
        
        /// <summary> 释放对象 </summary>
        private void Release()
        {
            if (Handle != 0)
                YIUILoadDI.ReleaseAction?.Invoke(Handle);
            LoadHelper.PutLoad(PkgName, ResName);
        }
    }
}