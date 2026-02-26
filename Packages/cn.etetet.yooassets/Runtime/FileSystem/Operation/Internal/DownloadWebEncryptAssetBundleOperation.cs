using UnityEngine;
using UnityEngine.Networking;

namespace YooAsset
{
    internal class DownloadWebEncryptAssetBundleOperation : DownloadAssetBundleOperation
    {
        private readonly bool _checkTimeout;
        private readonly IWebDecryptionServices _decryptionServices;
        private DownloadHandlerBuffer _downloadhandler;
        private ESteps _steps = ESteps.None;

        internal DownloadWebEncryptAssetBundleOperation(bool checkTimeout, IWebDecryptionServices decryptionServices, PackageBundle bundle, DownloadParam param) : base(bundle, param)
        {
            _checkTimeout = checkTimeout;
            _decryptionServices = decryptionServices;
        }
        internal override void InternalStart()
        {
            _steps = ESteps.CreateRequest;
        }
        internal override void InternalUpdate()
        {
            if (_steps == ESteps.None || _steps == ESteps.Done)
                return;

            // 创建下载器
            if (_steps == ESteps.CreateRequest)
            {
                // 获取请求地址
                _requestURL = GetRequestURL();

                // 重置变量
                ResetRequestFiled();

                // 创建下载器
                CreateWebRequest();

                _steps = ESteps.CheckRequest;
            }

            // 检测下载结果
            if (_steps == ESteps.CheckRequest)
            {
                DownloadProgress = _webRequest.downloadProgress;
                DownloadedBytes = (long)_webRequest.downloadedBytes;
                Progress = DownloadProgress;
                if (_webRequest.isDone == false)
                {
                    if (_checkTimeout)
                        CheckRequestTimeout();
                    return;
                }

                // 检查网络错误
                if (CheckRequestResult())
                {
                    if (_decryptionServices == null)
                    {
                        _steps = ESteps.Done;
                        Status = EOperationStatus.Failed;
                        Error = $"The {nameof(IWebDecryptionServices)} is null !";
                        YooLogger.Error(Error);
                        return;
                    }

                    var fileData = _downloadhandler.data;
                    if (fileData == null || fileData.Length == 0)
                    {
                        _steps = ESteps.Done;
                        Status = EOperationStatus.Failed;
                        Error = $"The download handler data is null or empty !";
                        YooLogger.Error(Error);
                        return;
                    }

                    AssetBundle assetBundle = LoadEncryptedAssetBundle(fileData);
                    if (assetBundle == null)
                    {
                        _steps = ESteps.Done;
                        Status = EOperationStatus.Failed;
                        Error = "Download handler asset bundle object is null !";
                    }
                    else
                    {
                        _steps = ESteps.Done;
                        Result = assetBundle;
                        Status = EOperationStatus.Succeed;
                    }
                }
                else
                {
                    _steps = ESteps.TryAgain;
                }

                // 注意：最终释放请求器
                DisposeWebRequest();
            }

            // 重新尝试下载
            if (_steps == ESteps.TryAgain)
            {
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

        private void CreateWebRequest()
        {
            _downloadhandler = new DownloadHandlerBuffer();
            _webRequest = DownloadSystemHelper.NewUnityWebRequestGet(_requestURL);
            _webRequest.downloadHandler = _downloadhandler;
            _webRequest.disposeDownloadHandlerOnDispose = true;
            _webRequest.SendWebRequest();
        }
        private void DisposeWebRequest()
        {
            if (_webRequest != null)
            {
                //注意：引擎底层会自动调用Abort方法
                _webRequest.Dispose();
                _webRequest = null;
            }
        }

        /// <summary>
        /// 加载加密资源文件
        /// </summary>
        private AssetBundle LoadEncryptedAssetBundle(byte[] fileData)
        {
            var fileInfo = new WebDecryptFileInfo();
            fileInfo.BundleName = Bundle.BundleName;
            fileInfo.FileLoadCRC = Bundle.UnityCRC;
            fileInfo.FileData = fileData;
            var decryptResult = _decryptionServices.LoadAssetBundle(fileInfo);
            return decryptResult.Result;
        }
    }
}