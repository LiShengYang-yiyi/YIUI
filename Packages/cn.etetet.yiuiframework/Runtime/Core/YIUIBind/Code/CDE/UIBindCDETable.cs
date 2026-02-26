//------------------------------------------------------------
// Author: 亦亦
// Mail: 379338943@qq.com
// Data: 2023年2月12日
//------------------------------------------------------------

using ET;
using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace YIUIFramework
{
    //[DetailedInfoBox("UI CDE总表 点击展开详细介绍", @"李胜扬")]
    [Serializable]
    [HideMonoScript]
    [LabelText("UI CDE总表")]
    [DisallowMultipleComponent]
    [AddComponentMenu("YIUIBind/★★★★★YIUI CDE Table 总表★★★★★")]
    public sealed partial class UIBindCDETable : SerializedMonoBehaviour
    {
        [NonSerialized]
        private EntityRef<Entity> m_EntityRef;

        internal Entity Entity
        {
            get
            {
                return m_EntityRef;
            }
            set
            {
                m_EntityRef = value;
            }
        }

        [HideInInspector]
        public UIBindComponentTable ComponentTable;

        [HideInInspector]
        public UIBindDataTable DataTable;

        [HideInInspector]
        public UIBindEventTable EventTable;

        /// <summary>
        /// 显式初始化 CDE 三个表
        /// 解决同一帧内激活-关闭导致 Awake 不触发的问题
        /// </summary>
        public void InitializeCDE()
        {
            ComponentTable?.InitComponentTable();
            DataTable?.InitDataTable();
            EventTable?.InitEventTable();
        }

        [LabelText("UI包名")]
        [ReadOnly]
        public string PkgName;

        [LabelText("UI资源名")]
        [ReadOnly]
        public string ResName;

        #region 关联

        [NonSerialized]
        [OdinSerialize]
        [LabelText("编辑时所有公共组件")]
        [ReadOnly]
        [PropertyOrder(1000)] //生成UI类时使用
        [ShowIf("@UIOperationHelper.CommonShowIf()")]
        internal List<UIBindCDETable> AllChildCdeTable = new();

        [NonSerialized]
        [ShowInInspector]
        [ReadOnly]
        [PropertyOrder(1000)]
        [LabelText("运行时所有公共组件")] //动态生成后的子类(公共组件) 运行时使用
        [HideIf("@UIOperationHelper.CommonShowIf()")]
        private Dictionary<string, EntityRef<Entity>> m_AllChildUIOwner = new();

        internal void AddUIOwner(string uiName, Entity uiBase)
        {
            if (!this.m_AllChildUIOwner.TryAdd(uiName, uiBase))
            {
                Debug.LogError($"{name} 已存在 {uiName} 请检查为何重复添加 是否存在同名组件");
            }
        }

        public Entity FindUIOwner(string uiName)
        {
            if (this.m_AllChildUIOwner.TryGetValue(uiName, out EntityRef<Entity> owner))
            {
                return owner;
            }

            Debug.LogError($"{this.name} 不存在 {uiName} 请检查");
            return null;
        }

        public T FindUIOwner<T>(string uiName) where T : Entity
        {
            return (T)this.FindUIOwner(uiName);
        }

        #endregion
    }
}