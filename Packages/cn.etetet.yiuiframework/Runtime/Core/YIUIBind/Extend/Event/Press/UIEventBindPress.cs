using ET;
using System;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using UnityEngine.EventSystems;
using System.Collections.Generic;

namespace YIUIFramework
{
    /// <summary>
    /// 长按
    /// 当长按时间超过配置时间时触发
    /// 只会触发一次, 主要用于 如长按技能图标 显示一个Tips说明这种需求
    /// 如果你想要的是按下后持续触发 那这个就不合适
    /// </summary>
    [LabelText("长按<obj>")]
    [AddComponentMenu("YIUIBind/Event/长按 【Press】 UIEventBindPress")]
    public class UIEventBindPress : UIEventBind, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
    {
        [SerializeField]
        [LabelText("长按时间")]
        private float m_PressTime = 0.5f; //按下后大于这个时间就会触发长按回调

        [SerializeField]
        [LabelText("按钮范围内长按才有效")]
        private bool m_SkipExit = true; //默认超过按钮范围后无效 与下面的有效范围不一样

        [SerializeField]
        [LabelText("有效范围")]
        private Vector2 m_EffectiveRange = new(50, 50); //按下时记录一个位置,触发时判断位置与按下位置范围 (进一步判断范围)

        [SerializeField]
        [LabelText("可选组件")]
        private Selectable m_Selectable;

        protected override bool IsTaskEvent => false;

        [NonSerialized]
        private readonly List<EUIEventParamType> m_FilterParamType = new() { EUIEventParamType.Object };

        protected override List<EUIEventParamType> GetFilterParamType => m_FilterParamType;

        private void Awake()
        {
            m_Selectable ??= GetComponent<Selectable>();
        }

        private new void OnDestroy()
        {
            RemoveCountDown();
        }

        private void OnDisable()
        {
            RemoveCountDown();
        }

        protected virtual void OnUIEvent()
        {
            m_UIEvent?.Invoke(m_PointerEventData as object);
        }

        private bool m_PointerDown;
        private Vector2 m_LastPos;
        private PointerEventData m_PointerEventData;

        public void OnPointerDown(PointerEventData eventData)
        {
            m_PointerDown = true;
            m_LastPos = Input.mousePosition;
            m_PointerEventData = eventData;

            if (m_PressTime <= 0)
            {
                m_PointerDown = false;
                PressEnd(0, 0, 0);
                return;
            }

            ET.EventSystem.Instance?.YIUIInvokeEntitySyncSafety(YIUISingletonHelper.YIUIMgr, new YIUIInvokeEntity_CountDownAdd { TimerCallback = PressEnd, TotalTime = m_PressTime });
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            RemoveCountDown();
        }

        private void RemoveCountDown()
        {
            if (m_PointerDown)
            {
                ET.EventSystem.Instance?.YIUIInvokeEntitySyncSafety(YIUISingletonHelper.YIUIMgr, new YIUIInvokeEntity_CountDownRemove { TimerCallback = PressEnd });
            }

            m_PointerEventData = null;
            m_PointerDown = false;
        }

        private void PressEnd(double residuetime, double elapsetime, double totaltime)
        {
            if (m_Selectable != null && !m_Selectable.interactable) return;

            var inputX = Input.mousePosition.x;
            var inputY = Input.mousePosition.y;
            if (Mathf.Abs(m_LastPos.x - inputX) > m_EffectiveRange.x || Mathf.Abs(m_LastPos.y - inputY) > m_EffectiveRange.y)
            {
                return;
            }

            try
            {
                OnUIEvent();
            }
            catch (Exception e)
            {
                Logger.LogError(e);
                throw;
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (m_SkipExit)
            {
                RemoveCountDown();
            }
        }
    }
}