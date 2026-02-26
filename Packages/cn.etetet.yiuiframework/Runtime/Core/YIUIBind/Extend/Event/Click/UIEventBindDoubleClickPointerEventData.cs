using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;

namespace YIUIFramework
{
    [InfoBox("提示: 可用事件参数 1个 , Object(PointerEventData)")]
    [LabelText("双击<PointerEventData>")]
    [AddComponentMenu("YIUIBind/Event/双击 【DoubleClick PointerEventData】 UIEventBindDoubleClickPointerEventData")]
    public class UIEventBindDoubleClickPointerEventData : UIEventBindDoubleClick
    {
        [NonSerialized]
        private readonly List<EUIEventParamType> m_FilterParamType = new() { EUIEventParamType.Object };

        protected override List<EUIEventParamType> GetFilterParamType => m_FilterParamType;

        protected override void OnUIEvent(PointerEventData eventData)
        {
            //额外添加 如果想要这个点击事件 使用此监听
            //响应方法那边参数是obj 自己在转一次 没有扩展这个参数 因为没必要
            m_UIEvent?.Invoke(eventData as object);
        }
    }
}