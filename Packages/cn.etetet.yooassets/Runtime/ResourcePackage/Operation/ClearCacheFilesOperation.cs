using System.Collections.Generic;
using System.Linq;

namespace YooAsset
{
    public sealed class ClearCacheFilesOperation : AsyncOperationBase
    {
        private enum ESteps
        {
            None,
            Prepare,
            ClearCacheFiles,
            CheckClearResult,
            Done,
        }

        private readonly PlayModeImpl _impl;
        private readonly string _clearMode;
        private readonly object _clearParam;
        private List<IFileSystem> _cloneList;
        private FSClearCacheFilesOperation _clearCacheFilesOp;
        private ESteps _steps = ESteps.None;

        internal ClearCacheFilesOperation(PlayModeImpl impl, string clearMode, object clearParam)
        {
            _impl = impl;
            _clearMode = clearMode;
            _clearParam = clearParam;
        }
        internal override void InternalStart()
        {
            _steps = ESteps.Prepare;
        }
        internal override void InternalUpdate()
        {
            if (_steps == ESteps.None || _steps == ESteps.Done)
                return;

            if (_steps == ESteps.Prepare)
            {
                var fileSytems = _impl.FileSystems;
                if (fileSytems == null || fileSytems.Count == 0)
                {
                    _steps = ESteps.Done;
                    Status = EOperationStatus.Failed;
                    Error = "The file system is empty !";
                    return;
                }

                foreach (var fileSystem in fileSytems)
                {
                    if (fileSystem == null)
                    {
                        _steps = ESteps.Done;
                        Status = EOperationStatus.Failed;
                        Error = "An empty object exists in the list!";
                        return;
                    }
                }

                _cloneList = fileSytems.ToList();
                _steps = ESteps.ClearCacheFiles;
            }

            if (_steps == ESteps.ClearCacheFiles)
            {
                if (_cloneList.Count == 0)
                {
                    _steps = ESteps.Done;
                    Status = EOperationStatus.Succeed;
                }
                else
                {
                    var fileSystem = _cloneList[0];
                    _cloneList.RemoveAt(0);

                    _clearCacheFilesOp = fileSystem.ClearCacheFilesAsync(_impl.ActiveManifest, _clearMode, _clearParam);
                    _clearCacheFilesOp.StartOperation();
                    AddChildOperation(_clearCacheFilesOp);
                    _steps = ESteps.CheckClearResult;
                }
            }

            if (_steps == ESteps.CheckClearResult)
            {
                _clearCacheFilesOp.UpdateOperation();
                Progress = _clearCacheFilesOp.Progress;
                if (_clearCacheFilesOp.IsDone == false)
                    return;

                if (_clearCacheFilesOp.Status == EOperationStatus.Succeed)
                {
                    _steps = ESteps.ClearCacheFiles;
                }
                else
                {
                    _steps = ESteps.Done;
                    Status = EOperationStatus.Failed;
                    Error = _clearCacheFilesOp.Error;
                }
            }
        }
        internal override string InternalGetDesc()
        {
            return $"ClearMode : {_clearMode}";
        }
    }
}