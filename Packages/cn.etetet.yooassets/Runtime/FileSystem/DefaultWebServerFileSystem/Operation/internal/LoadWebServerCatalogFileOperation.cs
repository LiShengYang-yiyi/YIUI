using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

namespace YooAsset
{
    internal sealed class LoadWebServerCatalogFileOperation : AsyncOperationBase
    {
        private enum ESteps
        {
            None,
            RequestData,
            LoadCatalog,
            Done,
        }

        private readonly DefaultWebServerFileSystem _fileSystem;
        private UnityWebDataRequestOperation _webDataRequestOp;
        private ESteps _steps = ESteps.None;

        internal LoadWebServerCatalogFileOperation(DefaultWebServerFileSystem fileSystem)
        {
            _fileSystem = fileSystem;
        }
        internal override void InternalStart()
        {
            _steps = ESteps.RequestData;
        }
        internal override void InternalUpdate()
        {
            if (_steps == ESteps.None || _steps == ESteps.Done)
                return;

            if (_steps == ESteps.RequestData)
            {
                if (_webDataRequestOp == null)
                {
                    string filePath = _fileSystem.GetCatalogBinaryFileLoadPath();
                    string url = DownloadSystemHelper.ConvertToWWWPath(filePath);
                    _webDataRequestOp = new UnityWebDataRequestOperation(url);
                    _webDataRequestOp.StartOperation();
                    AddChildOperation(_webDataRequestOp);
                }

                _webDataRequestOp.UpdateOperation();
                if (_webDataRequestOp.IsDone == false)
                    return;

                if (_webDataRequestOp.Status == EOperationStatus.Succeed)
                {
                    _steps = ESteps.LoadCatalog;
                }
                else
                {
                    _steps = ESteps.Done;
                    Status = EOperationStatus.Failed;
                    Error = _webDataRequestOp.Error;
                }
            }

            if (_steps == ESteps.LoadCatalog)
            {
                try
                {
                    var catalog = CatalogTools.DeserializeFromBinary(_webDataRequestOp.Result);
                    if (catalog.PackageName != _fileSystem.PackageName)
                    {
                        _steps = ESteps.Done;
                        Status = EOperationStatus.Failed;
                        Error = $"Catalog file package name {catalog.PackageName} cannot match the file system package name {_fileSystem.PackageName}";
                        return;
                    }

                    foreach (var wrapper in catalog.Wrappers)
                    {
                        var fileWrapper = new DefaultWebServerFileSystem.FileWrapper(wrapper.FileName);
                        _fileSystem.RecordCatalogFile(wrapper.BundleGUID, fileWrapper);
                    }

                    YooLogger.Log($"Package '{_fileSystem.PackageName}' buildin catalog files count : {catalog.Wrappers.Count}");
                    _steps = ESteps.Done;
                    Status = EOperationStatus.Succeed;
                }
                catch (Exception e)
                {
                    _steps = ESteps.Done;
                    Status = EOperationStatus.Failed;
                    Error = $"Failed to load catalog file : {e.Message}";
                }
            }
        }
    }
}