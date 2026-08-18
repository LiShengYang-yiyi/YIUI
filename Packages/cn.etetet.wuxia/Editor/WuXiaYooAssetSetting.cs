using System.Collections.Generic;
using UnityEngine;
using YooAsset.Editor;

namespace ET.WuXia.Editor
{
    // 【武侠包建设 2026-08-18】定义 WuXia 自有的 YooAsset 收集配置模板类型。
    // 作用：把 WuXia 资源收集规则保存在自身包内，安装时再合并到宿主项目的 DefaultPackage。
    // 原因：YooAsset 运行时只有一份项目级配置，不能让业务包直接替换 StateSync 的完整配置。
    public sealed class WuXiaYooAssetSetting : ScriptableObject
    {
        public List<AssetBundleCollectorPackage> Packages = new();

        public AssetBundleCollectorPackage GetPackage(string packageName)
        {
            foreach (AssetBundleCollectorPackage package in this.Packages)
            {
                if (package.PackageName == packageName)
                {
                    return package;
                }
            }

            return null;
        }
    }
}
