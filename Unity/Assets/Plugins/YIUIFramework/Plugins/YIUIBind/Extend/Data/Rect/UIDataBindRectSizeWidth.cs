using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace YIUIFramework
{
    [Serializable]
    [LabelText("UI大小-宽")]
    [RequireComponent(typeof(RectTransform))]
    [AddComponentMenu("YIUIBind/Data/UI大小-宽 【RectSizeWidth】 UIDataBindRectSizeWidth")]
    public class UIDataBindRectSizeWidth : UIDataBindSelectBase
    {
        protected override int Mask()
        {
            return 1 << (int)EUIBindDataType.Float;
        }

        protected override int SelectMax()
        {
            return 1;
        }

        [SerializeField]
        [ReadOnly]
        [Required("必须有此组件")]
        [LabelText("UI变换组件")]
        private RectTransform m_RectTransform;

        private Vector2 m_SizeData;
        protected override void OnRefreshData()
        {
            base.OnRefreshData();
            m_RectTransform ??= GetComponent<RectTransform>();
        }

        protected override void OnValueChanged()
        {
            if (m_RectTransform == null) return;

            m_SizeData.x = GetFirstValue<float>();
            m_SizeData.y = m_RectTransform.sizeDelta.y;
            m_RectTransform.sizeDelta = m_SizeData;
        }
    }
}