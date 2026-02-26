//------------------------------------------------------------
// Author: 亦亦
// Mail: 379338943@qq.com
// Data: 2023年2月12日
//------------------------------------------------------------

using System;
using System.Collections.Generic;
using ET;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace YIUIFramework
{
    //[DetailedInfoBox("UI 事件表 点击展开详细介绍", @"李胜扬")]
    //[AddComponentMenu("YIUIBind/★★★YIUI Event Table 事件表★★★")]
    [HideLabel]
    [Serializable]
    [HideMonoScript]
    [DisallowMultipleComponent]
    [AddComponentMenu("")]
    public sealed partial class UIBindEventTable : SerializedMonoBehaviour
    {
        [OdinSerialize]
        [ShowInInspector]
        [HideLabel]
        [Searchable]
        [Title("所有事件", TitleAlignment = TitleAlignments.Centered)]
        [OnStateUpdate("@$property.State.Expanded = true")]
        [DictionaryDrawerSettings(KeyLabel = "事件名称", ValueLabel = "事件内容", IsReadOnly = true, DisplayMode = DictionaryDisplayOptions.ExpandedFoldout)]
        [Delayed]
        private Dictionary<string, UIEventBase> m_EventDic = new Dictionary<string, UIEventBase>();

        public IReadOnlyDictionary<string, UIEventBase> EventDic => m_EventDic;

        private bool m_Initialized;

        private void Awake()
        {
            InitEventTable();
        }

        /// <summary>
        /// 显式初始化事件表
        /// 解决同一帧内激活-关闭导致 Awake 不触发的问题
        /// </summary>
        public void InitEventTable()
        {
            if (m_Initialized) return;
            m_Initialized = true;
            InitializeBinds(transform);
        }

        public UIEventBase FindEvent(string eventName)
        {
            if (string.IsNullOrEmpty(eventName))
            {
                Logger.LogError($"空的事件名称  请检查");
                return null;
            }

            return m_EventDic.TryGetValue(eventName, out var value) ? value : null;
        }

        public T FindEvent<T>(string eventName) where T : UIEventBase
        {
            return (T)FindEvent(eventName);
        }

        /// <summary>
        /// 清除指定事件
        /// </summary>
        public bool ClearEvent(string eventName)
        {
            if (!m_EventDic.TryGetValue(eventName, out var uiEvent))
            {
                Logger.LogError($"没有这个事件定义{eventName}");
                return false;
            }

            return uiEvent.Clear();
        }

        /// <summary>
        /// 清除所有事件
        /// 危险!! 运行时没这个需求
        /// </summary>
        public void ClearAllEvents()
        {
            foreach (var uiEvent in m_EventDic.Values)
            {
                uiEvent.Clear();
            }

            m_EventDic.Clear();
        }

        private void OnDestroy()
        {
            ClearAllEvents();
        }

        #region 递归初始化所有绑定数据

        private void InitializeBinds(Transform transform)
        {
            #if YIUIMACRO_BIND_INITIALIZE
            Logger.LogErrorContext(transform,$"{transform.name} 初始化调用所有子类 UIEventBind 绑定");
            #endif

            var binds = ListPool<UIEventBind>.Get();
            transform.GetComponents(binds);
            foreach (var bind in binds)
            {
                bind.Initialize(true);
            }

            ListPool<UIEventBind>.Put(binds);

            foreach (Transform child in transform)
            {
                InitializeBindsDeep(child);
            }
        }

        private void InitializeBindsDeep(Transform transform)
        {
            if (transform.HasComponent<UIBindEventTable>())
            {
                return;
            }

            var binds = ListPool<UIEventBind>.Get();
            transform.GetComponents(binds);
            foreach (var bind in binds)
            {
                bind.Initialize(true);
            }

            ListPool<UIEventBind>.Put(binds);

            foreach (Transform child in transform)
            {
                InitializeBindsDeep(child);
            }
        }

        #endregion
    }
}