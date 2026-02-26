using System;
using System.Collections.Generic;
using ET;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace YIUIFramework
{
    /// <summary>
    /// 点击事件绑定
    /// 与按钮无关
    /// 只要是任何可以被射线检测的物体都可以响应点击事件
    /// </summary>
    [LabelText("点击<null>")]
    [AddComponentMenu("YIUIBind/TaskEvent/点击 【Click】 UITaskEventBindClick")]
    public class UITaskEventBindClick : UIEventBind, IPointerClickHandler
    {
        [SerializeField]
        [LabelText("拖拽时不响应点击")]
        private bool m_SkipWhenDrag;

        [SerializeField]
        [LabelText("可选组件")]
        private Selectable m_Selectable;

        [SerializeField]
        [LabelText("响应中 屏蔽所有操作")]
        private bool m_BanLayerOption = true;

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

            if (m_UIEvent == null) return;

            if (ClickTasking) return;

            TaskEvent(eventData).NoContext();
        }

        protected override bool IsTaskEvent => true;

        [NonSerialized]
        private readonly List<EUIEventParamType> m_BaseFilterParamType = new();

        protected override List<EUIEventParamType> GetFilterParamType => m_BaseFilterParamType;

        [NonSerialized]
        protected bool ClickTasking; //异步中

        private void Awake()
        {
            m_Selectable ??= GetComponent<Selectable>();
            ClickTasking = false;
        }

        private async ETTask TaskEvent(PointerEventData eventData)
        {
            var banLayerCode = m_BanLayerOption ? ET.EventSystem.Instance?.YIUIInvokeEntitySyncSafety<YIUIInvokeEntity_BanLayerOptionForever, long>(YIUISingletonHelper.YIUIMgr, new YIUIInvokeEntity_BanLayerOptionForever()) ?? 0 : 0;

            ClickTasking = true;

            try
            {
                await OnUIEvent(eventData);
            }
            catch (Exception e)
            {
                Logger.LogError(e);
                throw;
            }
            finally
            {
                ClickTasking = false;

                if (m_BanLayerOption)
                {
                    ET.EventSystem.Instance?.YIUIInvokeEntitySyncSafety(YIUISingletonHelper.YIUIMgr, new YIUIInvokeEntity_RecoverLayerOptionForever { ForeverCode = banLayerCode });
                }
            }
        }

        protected virtual async ETTask OnUIEvent(PointerEventData eventData)
        {
            await m_UIEvent?.InvokeAsync();
        }
    }
}