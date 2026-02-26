using UnityEngine;

namespace YooAsset
{
    internal abstract class DownloadAssetBundleOperation : DefaultDownloadFileOperation
    {
        internal DownloadAssetBundleOperation(PackageBundle bundle, DownloadParam param) : base(bundle, param)
        {
        }

        public AssetBundle Result;
    }
}