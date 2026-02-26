using System.IO;
using UnityEngine;
using UnityEngine.Networking;

namespace YooAsset
{
    internal sealed class DownloadResumeFileOperation : DefaultDownloadFileOperation
    {
        private readonly DefaultCacheFileSystem _fileSystem;
        private DownloadHandlerFileRange _downloadHandle;
        private VerifyTempFileOperation _verifyOperation;
        private bool _isReuqestLocalFile;
        private long _fileOriginLength = 0;
        private string _tempFilePath;
        private ESteps _steps = ESteps.None;


        internal DownloadResumeFileOperation(DefaultCacheFileSystem fileSystem, PackageBundle bundle, DownloadParam param) : base(bundle, param)
        {
            _fileSystem = fileSystem;
        }
        internal override void InternalStart()
        {
            _isReuqestLocalFile = DownloadSystemHelper.IsRequestLocalFile(Param.MainURL);
            _tempFilePath = _fileSystem.GetTempFilePath(Bundle);
            _steps = ESteps.CheckExists;
        }
        internal override void InternalUpdate()
        {
            if (_steps == ESteps.None || _steps == ESteps.Done)
                return;

            // 检测文件是否存在
            if (_steps == ESteps.CheckExists)
            {
                if (_fileSystem.Exists(Bundle))
                {
                    _steps = ESteps.Done;
                    Status = EOperationStatus.Succeed;
                }
                else
                {
                    _steps = ESteps.CreateRequest;
                }
            }

            // 创建下载器
            if (_steps == ESteps.CreateRequest)
            {
                FileUtility.CreateFileDirectory(_tempFilePath);

                // 获取请求地址
                _requestURL = GetRequestURL();

                // 重置变量
                ResetRequestFiled();

                // 获取下载起始位置
                _fileOriginLength = 0;
                long fileBeginLength = -1;
                if (File.Exists(_tempFilePath))
                {
                    FileInfo fileInfo = new FileInfo(_tempFilePath);
                    if (fileInfo.Length >= Bundle.FileSize)
                    {
                        // 删除临时文件
                        File.Delete(_tempFilePath);
                    }
                    else
                    {
                        fileBeginLength = fileInfo.Length;
                        _fileOriginLength = fileBeginLength;
                        DownloadedBytes = _fileOriginLength;
                    }
                }

                // 创建下载器
                CreateWebRequest(fileBeginLength);

                _steps = ESteps.CheckRequest;
            }

            // 检测下载结果
            if (_steps == ESteps.CheckRequest)
            {
                DownloadProgress = _webRequest.downloadProgress;
                DownloadedBytes = _fileOriginLength + (long)_webRequest.downloadedBytes;
                if (_webRequest.isDone == false)
                {
                    CheckRequestTimeout();
                    return;
                }

                // 检查网络错误
                if (CheckRequestResult())
                    _steps = ESteps.VerifyTempFile;
                else
                    _steps = ESteps.TryAgain;

                // 在遇到特殊错误的时候删除文件
                ClearTempFileWhenError();

                // 注意：最终释放请求器
                DisposeWebRequest();
            }

            // 验证下载文件
            if (_steps == ESteps.VerifyTempFile)
            {
                var element = new TempFileElement(_tempFilePath, Bundle.FileCRC, Bundle.FileSize);
                _verifyOperation = new VerifyTempFileOperation(element);
                _verifyOperation.StartOperation();
                AddChildOperation(_verifyOperation);
                _steps = ESteps.CheckVerifyTempFile;
            }

            // 等待验证完成
            if (_steps == ESteps.CheckVerifyTempFile)
            {
                if (IsWaitForAsyncComplete)
                    _verifyOperation.WaitForAsyncComplete();

                _verifyOperation.UpdateOperation();
                if (_verifyOperation.IsDone == false)
                    return;

                if (_verifyOperation.Status == EOperationStatus.Succeed)
                {
                    if (_fileSystem.WriteCacheBundleFile(Bundle, _tempFilePath))
                    {
                        Status = EOperationStatus.Succeed;
                        _steps = ESteps.Done;
                    }
                    else
                    {
                        Error = $"{_fileSystem.GetType().FullName} failed to write file !";
                        Status = EOperationStatus.Failed;
                        _steps = ESteps.Done;
                    }
                }
                else
                {
                    Error = _verifyOperation.Error;
                    _steps = ESteps.TryAgain;
                }

                // 注意：验证完成后直接删除文件
                if (File.Exists(_tempFilePath))
                    File.Delete(_tempFilePath);
            }

            // 重新尝试下载
            if (_steps == ESteps.TryAgain)
            {
                //TODO 拷贝本地文件失败后不再尝试！
                if (_isReuqestLocalFile)
                {
                    Status = EOperationStatus.Failed;
                    _steps = ESteps.Done;
                    YooLogger.Error(Error);
                    return;
                }

                if (FailedTryAgain <= 0)
                {
                    Status = EOperationStatus.Failed;
                    _steps = ESteps.Done;
                    YooLogger.Error(Error);
                    return;
                }

                _tryAgainTimer += Time.unscaledDeltaTime;
                if (_tryAgainTimer > 1f)
                {
                    FailedTryAgain--;
                    _steps = ESteps.CreateRequest;
                    YooLogger.Warning(Error);
                }
            }
        }
        internal override void InternalAbort()
        {
            _steps = ESteps.Done;
            DisposeWebRequest();
        }
        internal override void InternalWaitForAsyncComplete()
        {
            while (true)
            {
                //TODO 如果是导入或解压本地文件，执行等待完毕
                if (_isReuqestLocalFile)
                {
                    InternalUpdate();
                    if (IsDone)
                        break;
                }
                else
                {
                    if (ExecuteWhileDone())
                    {
                        _steps = ESteps.Done;
                        break;
                    }
                }
            }
        }

        private void CreateWebRequest(long beginLength)
        {
            _webRequest = DownloadSystemHelper.NewUnityWebRequestGet(_requestURL);
#if UNITY_2019_4_OR_NEWER
            var handler = new DownloadHandlerFile(_tempFilePath, true);
            handler.removeFileOnAbort = false;
#else
            var handler = new DownloadHandlerFileRange(FileSavePath, Bundle.FileSize, _webRequest);
            _downloadHandle = handler;
#endif
            _webRequest.downloadHandler = handler;
            _webRequest.disposeDownloadHandlerOnDispose = true;
            if (beginLength > 0)
                _webRequest.SetRequestHeader("Range", $"bytes={beginLength}-");
            _webRequest.SendWebRequest();
        }
        private void DisposeWebRequest()
        {
            if (_downloadHandle != null)
            {
                _downloadHandle.Cleanup();
                _downloadHandle = null;
            }

            if (_webRequest != null)
            {
                //注意：引擎底层会自动调用Abort方法
                _webRequest.Dispose();
                _webRequest = null;
            }
        }
        private void ClearTempFileWhenError()
        {
            if (_fileSystem.ResumeDownloadResponseCodes == null)
                return;

            //说明：如果遇到以下错误返回码，验证失败直接删除文件
            if (_fileSystem.ResumeDownloadResponseCodes.Contains(HttpCode))
            {
                if (File.Exists(_tempFilePath))
                    File.Delete(_tempFilePath);
            }
        }
    }
}