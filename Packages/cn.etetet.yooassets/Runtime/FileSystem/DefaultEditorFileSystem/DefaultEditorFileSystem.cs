using System;

namespace YooAsset
{
    /// <summary>
    /// 模拟文件系统
    /// </summary>
    internal class DefaultEditorFileSystem : IFileSystem
    {
        protected string _packageRoot;

        /// <summary>
        /// 包裹名称
        /// </summary>
        public string PackageName { private set; get; }

        /// <summary>
        /// 文件根目录
        /// </summary>
        public string FileRoot
        {
            get
            {
                return _packageRoot;
            }
        }

        /// <summary>
        /// 文件数量
        /// </summary>
        public int FileCount
        {
            get
            {
                return 0;
            }
        }

        #region 自定义参数
        /// <summary>
        /// 异步模拟加载最小帧数
        /// </summary>
        public int _asyncSimulateMinFrame = 1;

        /// <summary>
        /// 异步模拟加载最大帧数
        /// </summary>
        public int _asyncSimulateMaxFrame = 1;
        #endregion

        public DefaultEditorFileSystem()
        {
        }
        public virtual FSInitializeFileSystemOperation InitializeFileSystemAsync()
        {
            var operation = new DEFSInitializeOperation(this);
            return operation;
        }
        public virtual FSLoadPackageManifestOperation LoadPackageManifestAsync(string packageVersion, int timeout)
        {
            var operation = new DEFSLoadPackageManifestOperation(this, packageVersion);
            return operation;
        }
        public virtual FSRequestPackageVersionOperation RequestPackageVersionAsync(bool appendTimeTicks, int timeout)
        {
            var operation = new DEFSRequestPackageVersionOperation(this);
            return operation;
        }
        public virtual FSClearCacheFilesOperation ClearCacheFilesAsync(PackageManifest manifest, string clearMode, object clearParam)
        {
            var operation = new FSClearCacheFilesCompleteOperation();
            return operation;
        }
        public virtual FSDownloadFileOperation DownloadFileAsync(PackageBundle bundle, DownloadParam param)
        {
            throw new System.NotImplementedException();
        }
        public virtual FSLoadBundleOperation LoadBundleFile(PackageBundle bundle)
        {
            if (bundle.BundleType == (int)EBuildBundleType.VirtualBundle)
            {
                var operation = new DEFSLoadBundleOperation(this, bundle);
                return operation;
            }
            else
            {
                string error = $"{nameof(DefaultEditorFileSystem)} not support load bundle type : {bundle.BundleType}";
                var operation = new FSLoadBundleCompleteOperation(error);
                return operation;
            }
        }

        public virtual void SetParameter(string name, object value)
        {
            if (name == FileSystemParametersDefine.ASYNC_SIMULATE_MIN_FRAME)
            {
                _asyncSimulateMinFrame = Convert.ToInt32(value);
            }
            else if (name == FileSystemParametersDefine.ASYNC_SIMULATE_MAX_FRAME)
            {
                _asyncSimulateMaxFrame = Convert.ToInt32(value);
            }
            else
            {
                YooLogger.Warning($"Invalid parameter : {name}");
            }
        }
        public virtual void OnCreate(string packageName, string packageRoot)
        {
            PackageName = packageName;

            if (string.IsNullOrEmpty(packageRoot))
                throw new Exception($"{nameof(DefaultEditorFileSystem)} root directory is null or empty !");

            _packageRoot = packageRoot;
        }
        public virtual void OnDestroy()
        {
        }

        public virtual bool Belong(PackageBundle bundle)
        {
            return true;
        }
        public virtual bool Exists(PackageBundle bundle)
        {
            return true;
        }
        public virtual bool NeedDownload(PackageBundle bundle)
        {
            return false;
        }
        public virtual bool NeedUnpack(PackageBundle bundle)
        {
            return false;
        }
        public virtual bool NeedImport(PackageBundle bundle)
        {
            return false;
        }

        public virtual string GetBundleFilePath(PackageBundle bundle)
        {
            if (bundle.IncludeMainAssets.Count == 0)
                return string.Empty;

            var pacakgeAsset = bundle.IncludeMainAssets[0];
            return pacakgeAsset.AssetPath;
        }
        public virtual byte[] ReadBundleFileData(PackageBundle bundle)
        {
            if (bundle.IncludeMainAssets.Count == 0)
                return null;

            var pacakgeAsset = bundle.IncludeMainAssets[0];
            return FileUtility.ReadAllBytes(pacakgeAsset.AssetPath);
        }
        public virtual string ReadBundleFileText(PackageBundle bundle)
        {
            if (bundle.IncludeMainAssets.Count == 0)
                return null;

            var pacakgeAsset = bundle.IncludeMainAssets[0];
            return FileUtility.ReadAllText(pacakgeAsset.AssetPath);
        }

        #region 内部方法
        public string GetEditorPackageVersionFilePath()
        {
            string fileName = YooAssetSettingsData.GetPackageVersionFileName(PackageName);
            return PathUtility.Combine(_packageRoot, fileName);
        }
        public string GetEditorPackageHashFilePath(string packageVersion)
        {
            string fileName = YooAssetSettingsData.GetPackageHashFileName(PackageName, packageVersion);
            return PathUtility.Combine(_packageRoot, fileName);
        }
        public string GetEditorPackageManifestFilePath(string packageVersion)
        {
            string fileName = YooAssetSettingsData.GetManifestBinaryFileName(PackageName, packageVersion);
            return PathUtility.Combine(_packageRoot, fileName);
        }
        public int GetAsyncSimulateFrame()
        {
            if (_asyncSimulateMinFrame > _asyncSimulateMaxFrame)
            {
                _asyncSimulateMinFrame = _asyncSimulateMaxFrame;
            }

            return UnityEngine.Random.Range(_asyncSimulateMinFrame, _asyncSimulateMaxFrame + 1);
        }
        #endregion
    }
}