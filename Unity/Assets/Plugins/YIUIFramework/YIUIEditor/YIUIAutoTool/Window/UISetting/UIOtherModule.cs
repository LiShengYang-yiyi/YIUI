#if UNITY_EDITOR
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEditor;
using UnityEditor.U2D;
using UnityEngine;
using UnityEngine.U2D;

namespace YIUIFramework.Editor
{
    /// <summary>
    /// 其他设置
    /// </summary>
    [HideReferenceObjectPicker]
    [HideLabel]
    public class UIOtherModule : BaseCreateModule
    {
        private EnumPrefs<UIBindEventTable.EUITaskEventType> m_UITaskEventTypePrefs;

        [ShowInInspector]
        [BoxGroup("EventTable 默认事件类型")]
        [EnumToggleButtons]
        [HideLabel]
        private UIBindEventTable.EUITaskEventType m_UITaskEventType;

        public override void Initialize()
        {
            m_UITaskEventTypePrefs =
                    new EnumPrefs<UIBindEventTable.EUITaskEventType>("YIUIAutoTool_OtherModule_TaskEventType", null,
                        UIBindEventTable.EUITaskEventType.Async);
            m_UITaskEventType = m_UITaskEventTypePrefs.Value;
        }

        public override void OnDestroy()
        {
            m_UITaskEventTypePrefs.Value = m_UITaskEventType;
        }
    }
}
#endif