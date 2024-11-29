using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using YIUIFramework;
using Logger = YIUIFramework.Logger;

namespace YIUIBind
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

            if (ClickTasking) return;

            TaskEvent(eventData).Forget();
        }

        protected override bool IsTaskEvent => true;

        [NonSerialized]
        private List<EUIEventParamType> m_BaseFilterParamType = new List<EUIEventParamType> { };

        protected override List<EUIEventParamType> GetFilterParamType()
        {
            return m_BaseFilterParamType;
        }

        [NonSerialized]
        protected bool ClickTasking; //异步中

        private void Awake()
        {
            m_Selectable ??= GetComponent<Selectable>();
            ClickTasking =   false;
        }

        protected async UniTask TaskEvent(PointerEventData eventData)
        {
            if (m_UIEvent == null) return;
            var banLayerCode = 0;
            if (m_BanLayerOption)
            {
                banLayerCode = PanelMgr.Inst.BanLayerOptionForever();
            }

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
                    PanelMgr.Inst.RecoverLayerOptionForever(banLayerCode);
                }
            }
        }

        protected virtual async UniTask OnUIEvent(PointerEventData eventData)
        {
            await m_UIEvent.InvokeAsync();
        }
    }
}