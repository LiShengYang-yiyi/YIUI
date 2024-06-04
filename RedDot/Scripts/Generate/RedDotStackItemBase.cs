using System;
using YIUIBind;
using YIUIFramework;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace MizuYIUI.RedDot
{



    /// <summary>
    /// 由YIUI工具自动创建 请勿手动修改
    /// </summary>
    public abstract class RedDotStackItemBase : BaseComponent
    {
        public const string PkgName = "RedDot";
        public const string ResName = "RedDotStackItem";
        
        public TMPro.TextMeshProUGUI u_ComStackText { get; private set; }
        public YIUIBind.UIDataValueBool u_DataShowStack { get; private set; }
        public YIUIBind.UIDataValueInt u_DataId { get; private set; }
        public YIUIBind.UIDataValueString u_DataTime { get; private set; }
        public YIUIBind.UIDataValueString u_DataOs { get; private set; }
        public YIUIBind.UIDataValueString u_DataSource { get; private set; }
        protected UIEventP0 u_EventShowStack { get; private set; }
        protected UIEventHandleP0 u_EventShowStackHandle { get; private set; }

        
        protected sealed override void UIBind()
        {
            u_ComStackText = ComponentTable.FindComponent<TMPro.TextMeshProUGUI>("u_ComStackText");
            u_DataShowStack = DataTable.FindDataValue<YIUIBind.UIDataValueBool>("u_DataShowStack");
            u_DataId = DataTable.FindDataValue<YIUIBind.UIDataValueInt>("u_DataId");
            u_DataTime = DataTable.FindDataValue<YIUIBind.UIDataValueString>("u_DataTime");
            u_DataOs = DataTable.FindDataValue<YIUIBind.UIDataValueString>("u_DataOs");
            u_DataSource = DataTable.FindDataValue<YIUIBind.UIDataValueString>("u_DataSource");
            u_EventShowStack = EventTable.FindEvent<UIEventP0>("u_EventShowStack");
            u_EventShowStackHandle = u_EventShowStack.Add(OnEventShowStackAction);

        }

        protected sealed override void UnUIBind()
        {
            u_EventShowStack.Remove(u_EventShowStackHandle);

        }
     
        protected virtual void OnEventShowStackAction(){}
   
   
    }
}