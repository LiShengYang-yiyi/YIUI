using System.Collections.Generic;
using ET;
using UnityEngine;
using UnityObject = UnityEngine.Object;

namespace YIUIFramework
{
    public partial class RedDotMgr
    {
        private const string RedDotKeyAssetName = "RedDotKeyAsset";

        private readonly Dictionary<int, RedDotKeyData> m_AllRedDotKeyData = new();

        public IReadOnlyDictionary<int, RedDotKeyData> AllRedDotKeyData => m_AllRedDotKeyData;

        private RedDotKeyAsset m_RedDotKeyAsset;

        /// <summary>
        /// 加载key
        /// </summary>
        private async ETTask<bool> LoadKeyAsset()
        {
            var loadResult = await ET.EventSystem.Instance?.YIUIInvokeEntityAsyncSafety<YIUIInvokeEntity_Load, ETTask<UnityObject>>(Entity, new YIUIInvokeEntity_Load
            {
                LoadType = typeof(RedDotKeyAsset),
                ResName = RedDotKeyAssetName
            });

            if (loadResult == null)
            {
                Debug.LogError($"初始化失败 没有加载到目标数据 {RedDotKeyAssetName}");
                return false;
            }

            m_RedDotKeyAsset = (RedDotKeyAsset)loadResult;

            InitKeyData();

            ET.EventSystem.Instance?.YIUIInvokeEntitySyncSafety(Entity, new YIUIInvokeEntity_Release
            {
                obj = loadResult
            });

            return true;
        }

        /// <summary>
        /// 初始化Key相关
        /// </summary>
        private void InitKeyData()
        {
            m_AllRedDotKeyData.Clear();

            foreach (var keyData in m_RedDotKeyAsset.AllRedDotDic.Values)
            {
                m_AllRedDotKeyData.Add(keyData.Id, keyData);
            }
        }

        /// <summary>
        /// 获取key描述
        /// </summary>
        public string GetKeyDes(int keyType)
        {
            if (!m_AllRedDotKeyData.ContainsKey(keyType))
            {
                Debug.LogError($"不存在这个key {keyType}");
                return "";
            }

            var keyData = m_AllRedDotKeyData[keyType];
            return keyData.Des;
        }
    }
}