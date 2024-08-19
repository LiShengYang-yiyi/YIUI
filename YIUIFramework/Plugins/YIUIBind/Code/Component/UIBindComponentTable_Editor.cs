#if UNITY_EDITOR

using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEditor;
using UnityEngine;
using YIUIFramework;
using Logger = YIUIFramework.Logger;

namespace YIUIBind
{
    //Editor
    public sealed partial class UIBindComponentTable
    {
        [OdinSerialize]
        [LabelText("所有绑定数据 编辑数据")]
        [Searchable]
        [HideReferenceObjectPicker]
        [PropertyOrder(-10)]
        [ShowIf("@UIOperationHelper.CommonShowIf()")]
        private List<UIBindPairData> m_AllBindPair = new List<UIBindPairData>();

        [GUIColor(0, 1, 1)]
        [LabelText("空名称自动设置")]
        [SerializeField]
        [HorizontalGroup("SetGroup")]
        [PropertyOrder(-99)]
        [ShowIf("@UIOperationHelper.CommonShowIf()")]
        private bool m_AutoSetNullName = true;

        [GUIColor(0, 1, 1)]
        [LabelText("空名称额外添加类型后缀")]
        [SerializeField]
        [ShowIf("m_AutoSetNullName")]
        [HorizontalGroup("SetGroup")]
        [PropertyOrder(-99)]
        [ShowIf("@UIOperationHelper.CommonShowIf()")]
        private bool m_NullNameAddTypeName = true;

        [GUIColor(1, 1, 0)]
        [Button("自动检查", 30)]
        [PropertyOrder(-100)]
        [ShowIf("@UIOperationHelper.CommonShowIf()")]
        public void AutoCheck()
        {
            if (!UIOperationHelper.CheckUIOperation(this)) return;

            CheckAllBindName();
        }

        public void EditorAddComponent(Component component, string setName = "")
        {
            if (!UIOperationHelper.CheckUIOperation(this)) return;

            AutoCheck();

            setName = GetAutoGenName(component, setName);

            if (m_AllBindDic.ContainsValue(component))
            {
                UnityTipsHelper.CallBackOk(
                    $" {name} 对象上的 {component.GetType()} 这个组件已经存在了 重复对象 {component.name} 相同组件只能有一个",
                    () => { Selection.activeGameObject = this.gameObject; });
                return;
            }

            if (m_AllBindDic.ContainsKey(setName))
            {
                UnityTipsHelper.CallBackOk(
                    $"{setName} 这个命名已经存在了 重复添加 必须命名唯一的名称",
                    () => { Selection.activeGameObject = this.gameObject; });
                return;
            }

            m_AllBindPair.Add(
                new UIBindPairData
                {
                    Name      = setName,
                    Component = component
                });

            AutoCheck();
        }

        public string GetAutoGenName(Component component, string name)
        {
            if (component == null)
            {
                Logger.LogErrorContext(this, $"{name} 空对象  所以 {name} 已忽略");
                return "";
            }

            if (!name.CheckFirstName(NameUtility.ComponentName))
            {
                if (string.IsNullOrEmpty(name))
                {
                    if (!m_AutoSetNullName)
                    {
                        return "";
                    }

                    if (m_NullNameAddTypeName)
                    {
                        name = $"{NameUtility.FirstName}{NameUtility.ComponentName}{component.name}{component.GetType().Name}";
                    }
                    else
                    {
                        name = $"{NameUtility.FirstName}{NameUtility.ComponentName}{component.name}";
                    }
                }
                else
                {
                    name = $"{NameUtility.FirstName}{NameUtility.ComponentName}{name}";
                }
            }

            name = name.ChangeToBigName(NameUtility.ComponentName);

            return name;
        }

        /// <summary>
        /// 检查所有绑定命名
        /// 必须m_ 开头
        /// 如果没用命名则使用对象的名字拼接
        /// 会尝试强制修改
        /// 如果还有同名则报错
        /// </summary>
        private void CheckAllBindName()
        {
            m_AllBindDic.Clear();
            if (m_AllBindPair == null || m_AllBindPair.Count < 1) return;

            for (var i = 0; i < m_AllBindPair.Count; i++)
            {
                var bindPair  = m_AllBindPair[i];
                var oldName   = bindPair.Name;
                var component = bindPair.Component;
                if (component == null)
                {
                    Logger.LogErrorContext(this, $"{name} 空对象  所以 {oldName} 已忽略");
                    continue;
                }

                var newName = GetAutoGenName(component, oldName);
                if (string.IsNullOrEmpty(newName))
                {
                    Logger.LogErrorContext(this, $"{name} 空名称  所以已忽略");
                    continue;
                }

                if (oldName != newName)
                {
                    bindPair.Name = newName;
                }

                if (string.IsNullOrEmpty(bindPair.Name))
                {
                    Logger.LogErrorContext(this, $"{name} 存在空名称 {bindPair.Component?.name} 已忽略");
                    continue;
                }

                if (bindPair.Component == null)
                {
                    Logger.LogErrorContext(this, $"{name} 空对象  所以 {bindPair.Name} 已忽略");
                    continue;
                }

                if (m_AllBindDic.ContainsValue(bindPair.Component))
                {
                    Logger.LogErrorContext(bindPair.Component, $"{name} 这个组件已经存在了 重复对象 {bindPair.Component.name} 已忽略");
                    continue;
                }

                if (m_AllBindDic.ContainsKey(bindPair.Name))
                {
                    Logger.LogErrorContext(bindPair.Component, $"{name} 这个命名已经存在了 重复添加 {bindPair.Name} 已忽略");
                    continue;
                }

                m_AllBindDic.Add(bindPair.Name, bindPair.Component);
            }
        }
    }
    
    /// <summary>
    /// 绑定数据对应关系
    /// </summary>
    [Serializable]
    [HideLabel]
    [HideReferenceObjectPicker]
    internal class UIBindPairData
    {
        [LabelText("名称")]
        public string Name ;

        [LabelText("对象")]
        public Component Component;
    }
}
#endif