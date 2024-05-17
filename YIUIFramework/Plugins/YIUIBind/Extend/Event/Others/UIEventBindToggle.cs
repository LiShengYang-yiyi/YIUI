﻿using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using YIUIFramework;

namespace YIUIBind
{
    [InfoBox("提示: 可用事件参数 <参数1:bool(当前状态)>")]
    [LabelText("开关<bool>")]
    [RequireComponent(typeof(Toggle))]
    [AddComponentMenu("YIUIBind/Event/开关 【Toggle】 UIEventBindToggle")]
    public class UIEventBindToggle : UIEventBind
    {
        [SerializeField]
        [ReadOnly]
        [Required("必须有此组件")]
        [LabelText("下拉菜单")]
        private Toggle m_Toggle;

        private List<EUIEventParamType> m_FilterParamType = new List<EUIEventParamType>
        {
            EUIEventParamType.Bool
        };

        protected override List<EUIEventParamType> GetFilterParamType()
        {
            return m_FilterParamType;
        }

        private void Awake()
        {
            m_Toggle ??= GetComponent<Toggle>();
        }

        private void OnEnable()
        {
            m_Toggle.onValueChanged.AddListener(OnValueChanged);
        }

        private void OnDisable()
        {
            m_Toggle.onValueChanged.RemoveListener(OnValueChanged);
        }

        private void OnValueChanged(bool value)
        {
            try
            {
                m_UIEvent?.Invoke(value);
            }
            catch (Exception e)
            {
                Log.Error(e);
                throw;
            }
        }
    }
}