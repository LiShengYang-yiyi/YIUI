using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using YooAsset;

namespace ET
{
    //沙盒
    public partial class ResourcesComponent
    {
        /// <summary>
        /// 获取包裹的内置文件根路径
        /// </summary>
        public string GetPackageBuildinRootDirectory()
        {
            return _Package.GetPackageBuildinRootDirectory();
        }

        /// <summary>
        /// 获取包裹的沙盒文件根路径
        /// </summary>
        public string GetPackageSandboxRootDirectory()
        {
            return _Package.GetPackageSandboxRootDirectory();
        }

        /// <summary>
        /// 清空包裹的沙盒目录
        /// </summary>
        public void ClearPackageSandbox()
        {
            _Package.ClearPackageSandbox();
        }
    }
}