using System;
using System.Collections.Generic;
using YooAsset;

namespace ET.Client
{
    /// <summary>
    /// YIUI资源管理器 yooasset扩展
    /// </summary>
    [ComponentOf(typeof(YIUILoadComponent))]
    public class YIUIYooAssetsLoadComponent : Entity, IAwake, IDestroy
    {
        public Dictionary<int, AssetHandle> m_AllHandle = new();
        public ResourcePackage m_Package;
    }
}