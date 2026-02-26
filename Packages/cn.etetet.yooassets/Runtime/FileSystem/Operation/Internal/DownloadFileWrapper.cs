
namespace YooAsset
{
    internal class DownloadFileWrapper : FSDownloadFileOperation
    {
        private enum ESteps
        {
            None,
            Download,
            Done,
        }

        private readonly FSDownloadFileOperation _downloadFileOp;
        private ESteps _steps = ESteps.None;

        internal DownloadFileWrapper(FSDownloadFileOperation downloadFileOp) : base(downloadFileOp.Bundle)
        {
            _downloadFileOp = downloadFileOp;
        }
        internal override void InternalStart()
        {
            _steps = ESteps.Download;
        }
        internal override void InternalUpdate()
        {
            if (_steps == ESteps.None || _steps == ESteps.Done)
                return;

            if (_steps == ESteps.Download)
            {
                if (IsWaitForAsyncComplete)
                    _downloadFileOp.WaitForAsyncComplete();

                if (_downloadFileOp.Status == EOperationStatus.None)
                    return;

                _downloadFileOp.UpdateOperation();
                Progress = _downloadFileOp.Progress;
                DownloadedBytes = _downloadFileOp.DownloadedBytes;
                DownloadProgress = _downloadFileOp.DownloadProgress;
                if (_downloadFileOp.IsDone == false)
                    return;

                _steps = ESteps.Done;
                Status = _downloadFileOp.Status;
                Error = _downloadFileOp.Error;
                HttpCode = _downloadFileOp.HttpCode;
            }
        }
        internal override void InternalWaitForAsyncComplete()
        {
            while (true)
            {
                if (ExecuteWhileDone())
                {
                    _steps = ESteps.Done;
                    break;
                }
            }
        }

        public override void Release()
        {
            _downloadFileOp.Release();
        }
        public override void Reference()
        {
            _downloadFileOp.Reference();
        }
    }
}