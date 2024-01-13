using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using YooAsset;

namespace ET
{
    /// <summary>
    /// 资源文件查询服务类
    /// </summary>
    public class GameQueryServices : IBuildinQueryServices
    {
        public bool QueryStreamingAssets(string packageName, string fileName)
        {
            // 注意：fileName包含文件格式
            //TODO 因为新版本 YooAssetSettings访问权限的修改 所以强制使用yoo 不要修改这个
            //临时处理
            //string filePath = Path.Combine(YooAssetSettings.DefaultYooFolderName, packageName, fileName);
            string filePath = Path.Combine("yoo", packageName, fileName);
            return BetterStreamingAssets.FileExists(filePath);
        }
    }
    
    /// <summary>
    /// 远端资源地址查询服务类
    /// </summary>
    public class RemoteServices : IRemoteServices
    {
        private readonly string _defaultHostServer;
        private readonly string _fallbackHostServer;

        public RemoteServices(string defaultHostServer, string fallbackHostServer)
        {
            _defaultHostServer = defaultHostServer;
            _fallbackHostServer = fallbackHostServer;
        }
        string IRemoteServices.GetRemoteMainURL(string fileName)
        {
            return $"{_defaultHostServer}/{fileName}";
        }
        string IRemoteServices.GetRemoteFallbackURL(string fileName)
        {
            return $"{_fallbackHostServer}/{fileName}";
        }
    }
    
    /// <summary>
    /// 资源管理组件
    /// 游戏中的资源应该使用ResourcesLoaderComponent来加载
    /// </summary>
    public partial class ResourcesComponent: Singleton<ResourcesComponent>, ISingletonAwake
    {
        private ResourcePackage _Package;
        
        public void Awake()
        {
            YooAssets.Initialize();
            BetterStreamingAssets.Initialize();
        }

        public override void Dispose()
        {
            YooAssets.Destroy();
        }
        
        public async ETTask CreatePackageAsync(string packageName, bool isDefault = false)
        {
            _Package = YooAssets.CreatePackage(packageName);
            if (isDefault)
            {
                YooAssets.SetDefaultPackage(_Package);
            }

            GlobalConfig globalConfig = Resources.Load<GlobalConfig>("GlobalConfig");
            EPlayMode ePlayMode = globalConfig.EPlayMode;

            #if !UNITY_EDITOR
            if (ePlayMode == EPlayMode.EditorSimulateMode)
            {
                ePlayMode = EPlayMode.OfflinePlayMode;
                Log.Error($"当前处于非编辑器模式 但是选择的是编辑器模式加载资源 强制修改为 OfflinePlayMode");
            }
            #endif
            
            // 编辑器下的模拟模式
            switch (ePlayMode)
            {
                case EPlayMode.EditorSimulateMode:
                {
                    EditorSimulateModeParameters createParameters = new();
                    createParameters.SimulateManifestFilePath = EditorSimulateModeHelper.SimulateBuild(packageName);
                    await _Package.InitializeAsync(createParameters).Task;
                    break;
                }
                case EPlayMode.OfflinePlayMode:
                {
                    OfflinePlayModeParameters createParameters = new();
                    await _Package.InitializeAsync(createParameters).Task;
                    break;
                }
                case EPlayMode.HostPlayMode:
                {
                    string defaultHostServer = GetHostServerURL();
                    string fallbackHostServer = GetHostServerURL();
                    HostPlayModeParameters createParameters = new();
                    createParameters.BuildinQueryServices = new GameQueryServices();
                    createParameters.RemoteServices       = new RemoteServices(defaultHostServer, fallbackHostServer);
                    await _Package.InitializeAsync(createParameters).Task;
                    break;
                }
                case EPlayMode.WebPlayMode:
                default:
                    Log.Error($"没有实现这个模式 {ePlayMode}");
                    throw new ArgumentOutOfRangeException();
            }

            return;

            string GetHostServerURL()
            {
                //string hostServerIP = "http://10.0.2.2"; //安卓模拟器地址
                string hostServerIP = "http://127.0.0.1";
                string appVersion = "v1.0";

#if UNITY_EDITOR
                if (UnityEditor.EditorUserBuildSettings.activeBuildTarget == UnityEditor.BuildTarget.Android)
                    return $"{hostServerIP}/CDN/Android/{appVersion}";
                else if (UnityEditor.EditorUserBuildSettings.activeBuildTarget == UnityEditor.BuildTarget.iOS)
                    return $"{hostServerIP}/CDN/IPhone/{appVersion}";
                else if (UnityEditor.EditorUserBuildSettings.activeBuildTarget == UnityEditor.BuildTarget.WebGL)
                    return $"{hostServerIP}/CDN/WebGL/{appVersion}";
                else
                    return $"{hostServerIP}/CDN/PC/{appVersion}";
#else
		        if (Application.platform == RuntimePlatform.Android)
		        	return $"{hostServerIP}/CDN/Android/{appVersion}";
		        else if (Application.platform == RuntimePlatform.IPhonePlayer)
		        	return $"{hostServerIP}/CDN/IPhone/{appVersion}";
		        else if (Application.platform == RuntimePlatform.WebGLPlayer)
		        	return $"{hostServerIP}/CDN/WebGL/{appVersion}";
		        else
		        	return $"{hostServerIP}/CDN/PC/{appVersion}";
#endif
            }
        }
        
        public void DestroyPackage(string packageName)
        {
            ResourcePackage package = YooAssets.GetPackage(packageName);
            package.UnloadUnusedAssets();
        }

        
    }
}