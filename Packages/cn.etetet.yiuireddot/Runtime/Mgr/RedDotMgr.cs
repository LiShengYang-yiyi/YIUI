//------------------------------------------------------------
// Author: 亦亦
// Mail: 379338943@qq.com
// Data: 2023年2月12日
//------------------------------------------------------------

using ET;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityObject = UnityEngine.Object;

namespace YIUIFramework
{
    /// <summary>
    /// 红点 管理器
    /// 文档: https://lib9kmxvq7k.feishu.cn/wiki/XzyawmryHitNVNk9QVtcDAftn5O
    /// </summary>
    [YIUISingleton(1000)]
    public partial class RedDotMgr : YIUISingleton<RedDotMgr>
    {
        //实时修改红点还是异步脏标定时修改
        private static readonly bool SyncSetCount = false; //默认异步

        private const string RedDotConfigAssetName = "RedDotConfigAsset";

        private readonly Dictionary<int, RedDotData> m_AllRedDotData = new();

        public IReadOnlyDictionary<int, RedDotData> AllRedDotData => m_AllRedDotData;

        private RedDotConfigAsset m_RedDotConfigAsset;

        protected override void OnDispose()
        {
            DisposeDirty();
        }

        protected override async ETTask<bool> MgrAsyncInit()
        {
            var resultConfig = await LoadConfigAsset();
            if (!resultConfig) return false;
            #if UNITY_EDITOR || YIUIMACRO_REDDOT_STACK
            var resultKey = await LoadKeyAsset();
            if (!resultKey) return false;
            #endif

            if (!SyncSetCount)
            {
                InitAsyncDirty();
            }

            return true;
        }

        /// <summary>
        /// 加载config
        /// </summary>
        private async ETTask<bool> LoadConfigAsset()
        {
            var loadResult = await ET.EventSystem.Instance?.YIUIInvokeEntityAsyncSafety<YIUIInvokeEntity_Load, ETTask<UnityObject>>(Entity, new YIUIInvokeEntity_Load
            {
                LoadType = typeof(RedDotConfigAsset),
                ResName = RedDotConfigAssetName
            });

            if (loadResult == null)
            {
                Debug.LogError($"初始化失败 没有加载到目标数据 {RedDotConfigAssetName}");
                return false;
            }

            m_RedDotConfigAsset = (RedDotConfigAsset)loadResult;

            InitNewAllData();
            InitLinkData();

            ET.EventSystem.Instance?.YIUIInvokeEntitySyncSafety(Entity, new YIUIInvokeEntity_Release
            {
                obj = loadResult
            });

            return true;
        }

        /// <summary>
        /// 初始化创建所有数据
        /// </summary>
        private void InitNewAllData()
        {
            m_AllRedDotData.Clear();

            foreach (int key in RedDotKeyHelper.GetKeys(true))
            {
                //有配置则使用配置 没有则使用默认配置
                var config = m_RedDotConfigAsset.GetConfigData(key) ?? new RedDotConfigData { Key = key };
                var data = new RedDotData(config);
                m_AllRedDotData.Add(key, data);
            }
        }

        /// <summary>
        /// 初始化红点关联数据
        /// </summary>
        private void InitLinkData()
        {
            foreach (var config in m_RedDotConfigAsset.AllRedDotConfigDic.Values)
            {
                var data = GetData(config.Key);
                if (data == null) continue;
                foreach (var parentId in config.ParentList)
                {
                    var parentData = GetData(parentId);
                    if (parentData != null)
                    {
                        data.AddParent(parentData);
                    }
                }
            }
        }
    }
}