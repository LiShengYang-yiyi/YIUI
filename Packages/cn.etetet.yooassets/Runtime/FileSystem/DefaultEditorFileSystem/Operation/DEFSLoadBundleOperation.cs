
namespace YooAsset
{
    internal class DEFSLoadBundleOperation : FSLoadBundleOperation
    {
        protected enum ESteps
        {
            None,
            DownloadFile,
            LoadAssetBundle,
            CheckResult,
            Done,
        }

        private readonly DefaultEditorFileSystem _fileSystem;
        private readonly PackageBundle _bundle;
        private int _asyncSimulateFrame;
        private ESteps _steps = ESteps.None;

        internal DEFSLoadBundleOperation(DefaultEditorFileSystem fileSystem, PackageBundle bundle)
        {
            _fileSystem = fileSystem;
            _bundle = bundle;
        }
        internal override void InternalStart()
        {
            _steps = ESteps.DownloadFile;
        }
        internal override void InternalUpdate()
        {
            if (_steps == ESteps.None || _steps == ESteps.Done)
                return;

            if (_steps == ESteps.DownloadFile)
            {
                _asyncSimulateFrame = _fileSystem.GetAsyncSimulateFrame();
                DownloadProgress = 1f;
                DownloadedBytes = _bundle.FileSize;
                _steps = ESteps.LoadAssetBundle;
            }

            if (_steps == ESteps.LoadAssetBundle)
            {
                if (IsWaitForAsyncComplete)
                {
                    _steps = ESteps.CheckResult;
                }
                else
                {
                    if (_asyncSimulateFrame <= 0)
                        _steps = ESteps.CheckResult;
                    else
                        _asyncSimulateFrame--;
                }
            }

            if (_steps == ESteps.CheckResult)
            {
                _steps = ESteps.Done;
                Result = new VirtualBundleResult(_fileSystem, _bundle);
                Status = EOperationStatus.Succeed;
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
    }
}