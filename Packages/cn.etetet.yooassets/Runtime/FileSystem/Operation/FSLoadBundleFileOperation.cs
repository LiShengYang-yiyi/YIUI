
namespace YooAsset
{
    internal abstract class FSLoadBundleOperation : AsyncOperationBase
    {
        /// <summary>
        /// 加载结果
        /// </summary>
        public BundleResult Result { protected set; get; }

        /// <summary>
        /// 下载进度
        /// </summary>
        public float DownloadProgress { protected set; get; } = 0;

        /// <summary>
        /// 下载大小
        /// </summary>
        public long DownloadedBytes { protected set; get; } = 0;
    }

    internal sealed class FSLoadBundleCompleteOperation : FSLoadBundleOperation
    {
        private readonly string _error;

        internal FSLoadBundleCompleteOperation(string error)
        {
            _error = error;
        }
        internal override void InternalStart()
        {
            Status = EOperationStatus.Failed;
            Error = _error;
        }
        internal override void InternalUpdate()
        {
        }
    }
}