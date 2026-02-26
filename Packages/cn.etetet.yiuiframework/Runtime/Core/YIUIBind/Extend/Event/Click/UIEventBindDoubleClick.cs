using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace YIUIFramework
{
    [LabelText("双击<null>")]
    [AddComponentMenu("YIUIBind/Event/双击 【DoubleClick】 UIEventBindDoubleClick")]
    public class UIEventBindDoubleClick : UIEventBind, IPointerClickHandler
    {
        [SerializeField]
        [LabelText("双击间隔时间")]
        private float m_DoubleClickInterval = 0.3f;

        [SerializeField]
        [LabelText("拖拽时不响应点击")]
        private bool m_SkipWhenDrag;

        [SerializeField]
        [LabelText("可选组件")]
        private Selectable m_Selectable;

        private float m_LastClickTime = 0f;

        public void OnPointerClick(PointerEventData eventData)
        {
            if (m_Selectable != null && !m_Selectable.interactable)
            {
                return;
            }

            if (m_SkipWhenDrag && eventData.dragging)
            {
                return;
            }

            try
            {
                float timeSinceLastClick = Time.time - m_LastClickTime;

                if (timeSinceLastClick <= m_DoubleClickInterval)
                {
                    OnUIEvent(eventData);
                    m_LastClickTime = 0;
                    return;
                }

                m_LastClickTime = Time.time;
            }
            catch (Exception e)
            {
                Logger.LogError(e);
                throw;
            }
        }

        protected override bool IsTaskEvent => false;

        [NonSerialized]
        private readonly List<EUIEventParamType> m_BaseFilterParamType = new();

        protected override List<EUIEventParamType> GetFilterParamType => m_BaseFilterParamType;

        private void Awake()
        {
            m_Selectable ??= GetComponent<Selectable>();
        }

        protected virtual void OnUIEvent(PointerEventData eventData)
        {
            m_UIEvent?.Invoke();
        }
    }
}