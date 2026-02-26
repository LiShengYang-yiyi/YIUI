using System.IO;
using System.Collections;
using System.Collections.Generic;
using System;

namespace YooAsset
{
    internal class DeserializeManifestOperation : AsyncOperationBase
    {
        private enum ESteps
        {
            None,
            DeserializeFileHeader,
            PrepareAssetList,
            DeserializeAssetList,
            PrepareBundleList,
            DeserializeBundleList,
            InitManifest,
            Done,
        }

        private readonly BufferReader _buffer;
        private int _packageAssetCount;
        private int _packageBundleCount;
        private int _progressTotalValue;
        private ESteps _steps = ESteps.None;

        /// <summary>
        /// 解析的清单实例
        /// </summary>
        public PackageManifest Manifest { private set; get; }

        public DeserializeManifestOperation(byte[] binaryData)
        {
            _buffer = new BufferReader(binaryData);
        }
        internal override void InternalStart()
        {
            _steps = ESteps.DeserializeFileHeader;
        }
        internal override void InternalUpdate()
        {
            if (_steps == ESteps.None || _steps == ESteps.Done)
                return;

            try
            {
                if (_steps == ESteps.DeserializeFileHeader)
                {
                    if (_buffer.IsValid == false)
                    {
                        _steps = ESteps.Done;
                        Status = EOperationStatus.Failed;
                        Error = "Buffer is invalid !";
                        return;
                    }

                    // 读取文件标记
                    uint fileSign = _buffer.ReadUInt32();
                    if (fileSign != ManifestDefine.FileSign)
                    {
                        _steps = ESteps.Done;
                        Status = EOperationStatus.Failed;
                        Error = "The manifest file format is invalid !";
                        return;
                    }

                    // 读取文件版本
                    string fileVersion = _buffer.ReadUTF8();
                    if (fileVersion != ManifestDefine.FileVersion)
                    {
                        _steps = ESteps.Done;
                        Status = EOperationStatus.Failed;
                        Error = $"The manifest file version are not compatible : {fileVersion} != {ManifestDefine.FileVersion}";
                        return;
                    }

                    // 读取文件头信息
                    Manifest = new PackageManifest();
                    Manifest.FileVersion = fileVersion;
                    Manifest.EnableAddressable = _buffer.ReadBool();
                    Manifest.LocationToLower = _buffer.ReadBool();
                    Manifest.IncludeAssetGUID = _buffer.ReadBool();
                    Manifest.OutputNameStyle = _buffer.ReadInt32();
                    Manifest.BuildBundleType = _buffer.ReadInt32();
                    Manifest.BuildPipeline = _buffer.ReadUTF8();
                    Manifest.PackageName = _buffer.ReadUTF8();
                    Manifest.PackageVersion = _buffer.ReadUTF8();
                    Manifest.PackageNote = _buffer.ReadUTF8();

                    // 检测配置
                    if (Manifest.EnableAddressable && Manifest.LocationToLower)
                        throw new System.Exception("Addressable not support location to lower !");

                    _steps = ESteps.PrepareAssetList;
                }

                if (_steps == ESteps.PrepareAssetList)
                {
                    _packageAssetCount = _buffer.ReadInt32();
                    _progressTotalValue = _packageAssetCount;
                    ManifestTools.CreateAssetCollection(Manifest, _packageAssetCount);
                    _steps = ESteps.DeserializeAssetList;
                }
                if (_steps == ESteps.DeserializeAssetList)
                {
                    while (_packageAssetCount > 0)
                    {
                        var packageAsset = new PackageAsset();
                        packageAsset.Address = _buffer.ReadUTF8();
                        packageAsset.AssetPath = _buffer.ReadUTF8();
                        packageAsset.AssetGUID = _buffer.ReadUTF8();
                        packageAsset.AssetTags = _buffer.ReadUTF8Array();
                        packageAsset.BundleID = _buffer.ReadInt32();
                        packageAsset.DependBundleIDs = _buffer.ReadInt32Array();
                        ManifestTools.FillAssetCollection(Manifest, packageAsset);

                        _packageAssetCount--;
                        Progress = 1f - _packageAssetCount / _progressTotalValue;
                        if (OperationSystem.IsBusy)
                            break;
                    }

                    if (_packageAssetCount <= 0)
                    {
                        _steps = ESteps.PrepareBundleList;
                    }
                }

                if (_steps == ESteps.PrepareBundleList)
                {
                    _packageBundleCount = _buffer.ReadInt32();
                    _progressTotalValue = _packageBundleCount;
                    ManifestTools.CreateBundleCollection(Manifest, _packageBundleCount);
                    _steps = ESteps.DeserializeBundleList;
                }
                if (_steps == ESteps.DeserializeBundleList)
                {
                    while (_packageBundleCount > 0)
                    {
                        var packageBundle = new PackageBundle();
                        packageBundle.BundleName = _buffer.ReadUTF8();
                        packageBundle.UnityCRC = _buffer.ReadUInt32();
                        packageBundle.FileHash = _buffer.ReadUTF8();
                        packageBundle.FileCRC = _buffer.ReadUTF8();
                        packageBundle.FileSize = _buffer.ReadInt64();
                        packageBundle.Encrypted = _buffer.ReadBool();
                        packageBundle.Tags = _buffer.ReadUTF8Array();
                        packageBundle.DependBundleIDs = _buffer.ReadInt32Array();
                        ManifestTools.FillBundleCollection(Manifest, packageBundle);

                        _packageBundleCount--;
                        Progress = 1f - _packageBundleCount / _progressTotalValue;
                        if (OperationSystem.IsBusy)
                            break;
                    }

                    if (_packageBundleCount <= 0)
                    {
                        _steps = ESteps.InitManifest;
                    }
                }

                if (_steps == ESteps.InitManifest)
                {
                    ManifestTools.InitManifest(Manifest);
                    _steps = ESteps.Done;
                    Status = EOperationStatus.Succeed;
                }
            }
            catch (System.Exception e)
            {
                Manifest = null;
                _steps = ESteps.Done;
                Status = EOperationStatus.Failed;
                Error = e.Message;
            }
        }
    }
}